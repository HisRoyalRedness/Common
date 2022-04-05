using System;
using System.Collections.Generic;

#nullable enable

namespace HisRoyalRedness.com
{
    #region IState interface and StateBase
    public interface IState<TStateId, TEvent>
        where TStateId : struct
    {
        /// <summary>
        /// Called when transitioning into a new state
        /// </summary>
        /// <param name="old_state">The state from which the transition originated</param>
        void EnterState(TStateId old_state);

        /// <summary>
        /// Called when a transitioning from a state
        /// </summary>
        /// <param name="new_state">The state to which the machine will transition</param>
        void ExitState(TStateId new_state);

        /// <summary>
        /// The <typeparamref name="TStateId"/> that represents this state
        /// </summary>
        TStateId StateId { get; }

        /// <summary>
        /// Called when this state is added to a state machine
        /// </summary>
        void StateAdded();

        /// <summary>
        /// Called when this state is the current state and the state machine has
        /// received an event
        /// </summary>
        /// <param name="evnt">The event received</param>
        /// <returns>The state to transition to</returns>
        TStateId HandleEvent(TEvent evnt);
    }

    public abstract class StateBase<TStateId, TEvent> : IState<TStateId, TEvent>
        where TStateId : struct
    {
        protected StateBase(TStateId stateId)
        {
            StateId = stateId;
        }

        /// <inheritdoc />
        public TStateId StateId { get; private set; }

        /// <inheritdoc />
        public void EnterState(TStateId old_state) => EnterStateInternal(old_state);

        /// <inheritdoc />
        public void ExitState(TStateId new_state) => ExitStateInternal(new_state);

        /// <inheritdoc />
        public void StateAdded() => StateAddedInternal();

        public TStateId HandleEvent(TEvent evnt) => HandleEventInternal(evnt);

        /// <summary>
        /// Called when the state is added to the state machine.
        /// Can be used for some initialisation that is only done when added
        /// </summary>
        protected virtual void StateAddedInternal() { }

        // Override these in inheriting classes
        protected virtual void EnterStateInternal(TStateId old_state) { }
        protected virtual void ExitStateInternal(TStateId new_state) { }
        protected virtual TStateId HandleEventInternal(TEvent evnt) { return default; }
    }
    #endregion IState interface and StateBase

    #region IStateMachine interface and StateMachine
    /// <summary>
    /// Used to manually trigger a transition between states. The usual way is to invoke a transition
    /// through an event <see cref="StateMachine{TStateId}.Accept{TEvent}(TEvent)"/>.
    /// </summary>
    /// <typeparam name="TStateId">The State Id enum</typeparam>
    public interface IManualTransition<TStateId>
        where TStateId : struct
    {
        /// <summary>
        /// Manually transition to <paramref name="stateId"/>
        /// </summary>
        /// <param name="stateId">The state to transition to</param>
        /// <returns>Return true if the transition was valid, false otherwise</returns>
        bool Transition(TStateId stateId);
    }

    public interface IStateMachine<TStateId, TEvent>
        where TStateId : struct
    {
        void Start();
        void Start(TStateId initialState);

        void Stop();

        void AddState(IState<TStateId, TEvent> state);
        void AddState(IState<TStateId, TEvent> state, TStateId parentStateId);

        bool Accept(TEvent evnt);

        bool IsStarted { get; }

        bool IsInState(TStateId stateId);

        TStateId CurrentState { get; }
        TStateId InitialState { get; }
        bool IsValidState(TStateId stateId);
    }

    public class StateMachine<TStateId, TEvent> : IStateMachine<TStateId, TEvent>, IManualTransition<TStateId>
        where TStateId : struct
    {
        public StateMachine(TStateId sameState, TStateId unhandledState)
        {
            SameState = sameState;
            UnhandledState = unhandledState;
            InitialState = unhandledState;
            InternalCurrentState = unhandledState;

            if (SameState.Equals(UnhandledState))
                throw new ArgumentException($"{nameof(sameState)} cannot be the same value as {nameof(unhandledState)}", nameof(sameState));
        }

        public StateMachine(TStateId sameState, TStateId unhandledState, TStateId initialState)
        {
            SameState = sameState;
            UnhandledState = unhandledState;
            InitialState = initialState;
            InternalCurrentState = unhandledState;

            if (SameState.Equals(UnhandledState))
                throw new ArgumentException($"{nameof(sameState)} cannot be the same value as {nameof(unhandledState)}", nameof(sameState));
            if (InitialState.Equals(SameState))
                throw new ArgumentException($"{nameof(initialState)} cannot be the same value as {nameof(sameState)}", nameof(initialState));
            if (InitialState.Equals(UnhandledState))
                throw new ArgumentException($"{nameof(initialState)} cannot be the same value as {nameof(unhandledState)}", nameof(initialState));
        }

        public void AddState(IState<TStateId, TEvent> state) => AddStateInternal(state, null);

        public void AddState(IState<TStateId, TEvent> state, TStateId parentStateId)
        {
            if (IsExistingState(parentStateId))
                AddStateInternal(state, _stateMap[parentStateId]);
            else
                throw new ArgumentException($"A parent state with the state id of {parentStateId} does not exist", nameof(state));
        }

        void AddStateInternal(IState<TStateId, TEvent> state, StateLineage? parent)
        {
            var stateId = state.StateId;

            if (IsUnhandledState(stateId))
                throw new ArgumentException($"{nameof(state)} cannot be the {nameof(UnhandledState)} state", nameof(state));
            if (IsSameState(stateId))
                throw new ArgumentException($"{nameof(state)} cannot be the {nameof(SameState)} state", nameof(state));
            if (IsExistingState(stateId))
                throw new ArgumentException($"A state with the state id of {state.StateId} already exists", nameof(state));

            _stateMap.Add(stateId, new StateLineage(stateId, state, parent));
            _stateMap[stateId].State.StateAdded();
        }

        public void Start() => Start(InitialState);

        public void Start(TStateId initialState)
        {
            if (IsStarted)
                return;

            if (_stateMap.Count == 0)
                throw new ApplicationException("No states have been added");
            if (!IsValidState(initialState))
                throw new ApplicationException("The initial state is not valid");

            // Get the list of states from initial to the top state,
            // in reverse order, then call EnterState on each in turn
            Stack<StateLineage> stack = new();
            for (var state = _stateMap[initialState]; state != null; state = state.Parent)
                stack.Push(state);

            while (stack.Count > 0)
            {
                var state = stack.Pop().State;
                state.EnterState(UnhandledState);
                InternalCurrentState = state.StateId;
            }

            IsStarted = true;
        }

        public void Stop()
        {
            if (!IsStarted)
                return;

            // Exit states from the current to the top
            for (var state = _stateMap[CurrentState]; state != null; state = state.Parent)
                state.State.ExitState(UnhandledState);

            InternalCurrentState = UnhandledState;
            IsStarted = false;
        }

        public bool IsInState(TStateId stateId)
        {
            // Determine if the current state (top-level or descendant) matches the given state ID

            if (!IsStarted || !IsValidState(CurrentState) || !IsValidState(stateId))
                return false;

            var state = _stateMap[CurrentState];
            while(state != null)
            {
                if (state.State.StateId.Equals(stateId))
                    return true;
                state = state.Parent;
            }
            return false;
        }

        public bool Accept(TEvent evnt)
        {
            if (!IsStarted || _stateMap.Count == 0 || !IsValidState(CurrentState))
                return false;

            TStateId nextStateId = UnhandledState;
            for (var state = _stateMap[CurrentState]; state != null && IsUnhandledState(nextStateId); state = state.Parent)
                nextStateId = state.State.HandleEvent(evnt);

            // The event was handled, but no state change resulted
            if (IsSameState(nextStateId))
                return true;

            // The event wasn't handled by any states
            else if (IsUnhandledState(nextStateId))
                return false;

            // The event was handled, and a valid different state resulted
            else if (IsValidState(nextStateId))
                return ((IManualTransition<TStateId>)this).Transition(nextStateId);

            // Some other invalid condition
            else
                return false;
        }

        bool IManualTransition<TStateId>.Transition(TStateId newStateId)
        {
            // Transition from the current state to next_state_ID
            //
            // Post conditions:
            //
            //      1). Next state is the same_state token or unhandled_event token, then remain in the current state. Return true
            //            or
            //      2). Next state is the same state as the current state, then exit and enter the current state. Return true
            //            or
            //      3). Next state is a different state to the current state. Then:
            //
            //         Determine the lowest common ancestor (lca).
            //
            //        * We have an lca.
            //             Exit from the current state to the lca
            //             Enter from the lca to the new state
            //        * We have no common ancestor
            //             Exit from the current state to the root
            //             Enter from the root to the new state
            //        * Return true
            //
            //

            if (!IsStarted || _stateMap.Count == 0)
                return false;

            if (IsSameState(newStateId) || IsUnhandledState(newStateId))
                return true;

            if (!IsValidState(newStateId))
                return false;

            TStateId oldStateId = CurrentState;

            // Special case: If the old and new state are the same, exit and re-enter
            if (oldStateId.Equals(newStateId))
            {
                _stateMap[CurrentState].State.ExitState(oldStateId);
                _stateMap[CurrentState].State.EnterState(newStateId);
            }
            else
            {
                // Find the lowest common ancestor (lca)
                // Do this by walking backward from the desired state to the root (entering).
                //    At each step, walk backward from the current state to the root (exiting).
                //    At some point, entering and exiting should point to the same state. This will be the lowest common ancestor.
                // If lca is null, there is no common ancestor.
                Stack<StateLineage> enterStack = new();
                StateLineage? lca = null;

                for (var entering = _stateMap[newStateId]; entering != null && lca == null; entering = entering.Parent)
                {
                    enterStack.Push(entering);
                    for (var exiting = _stateMap[CurrentState]; exiting != null && lca == null; exiting = exiting.Parent)
                    {
                        if (exiting.StateId.Equals(entering.StateId))
                            lca = entering;
                    }
                    if (lca != null)
                        enterStack.Pop();
                }

                // Exit states up to the lca (or root if no lca)
                for (var exiting = _stateMap[CurrentState]; exiting != null && (lca == null || !exiting.StateId.Equals(lca.StateId)); exiting = exiting.Parent)
                {
                    InternalCurrentState = exiting.Parent != null ? exiting.Parent.StateId : UnhandledState;
                    exiting.State.ExitState(newStateId);
                }

                // Enter states down from the lca (or root if no lca) to the next_state_id
                while (enterStack.Count > 0)
                {
                    StateLineage state = enterStack.Pop();
                    InternalCurrentState = state.StateId;
                    state.State.EnterState(oldStateId);
                }
            }
            
            return true;
        }

        public bool IsValidState(TStateId stateId) => !IsSameState(stateId) && !IsUnhandledState(stateId) && IsExistingState(stateId);
        bool IsSameState(TStateId stateId) => SameState.Equals(stateId);
        bool IsUnhandledState(TStateId stateId) => UnhandledState.Equals(stateId);
        bool IsExistingState(TStateId stateId) => _stateMap.ContainsKey(stateId);

        public TStateId SameState { get; private set; }
        public TStateId UnhandledState { get; private set; }
        public TStateId InitialState { get; private set; }
        public TStateId CurrentState => IsStarted ? InternalCurrentState : UnhandledState;

        public bool IsStarted { get; private set; }
        private TStateId InternalCurrentState { get; set; }

        Dictionary<TStateId, StateLineage> _stateMap = new Dictionary<TStateId, StateLineage>();

        // Internal structure for keeping tabs on the parentage
        // of the states
        class StateLineage
        {
            internal StateLineage(TStateId stateId, IState<TStateId, TEvent> state, StateLineage? parent = null)
            {
                StateId = stateId;
                State = state;
                Parent = parent;
            }

            internal TStateId StateId { get; private set; }
            internal IState<TStateId, TEvent> State { get; private set; }

            internal StateLineage? Parent { get; private set; }
        };
    }
    #endregion IStateMachine interface and StateMachine
}
