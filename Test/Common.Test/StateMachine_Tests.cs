using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;

namespace HisRoyalRedness.com
{
    [TestClass]
    public  class StateMachine_Tests
    {
        public enum StateIds
        {
            UnhandledState,
            SameState,
            State1,
            State2,
            State3,
            State4,
            State5,
            State6,
            State7,
            State8,
            State9,
            State10
        }

        public enum StateEvents
        {
            Event1,
            Event2
        }

        public class SampleState : StateBase<StateIds, StateEvents>
        {
            public SampleState(StateIds stateId, Action<StateIds> addFn = null, Action<StateIds, StateIds> enterFn = null, Action<StateIds, StateIds> exitFn = null, Func<StateEvents, StateIds> handleFn = null) : base(stateId)
            {
                _addFn = addFn;
                _enterFn = enterFn;
                _exitFn = exitFn;
                _handleEvtFn = handleFn;
            }

            public int AddOccurrences { get; private set; } = 0;

            protected override void StateAddedInternal()
            {
                AddOccurrences++;
                _addFn?.Invoke(StateId);
            }

            protected override void EnterStateInternal(StateIds old_state) => _enterFn?.Invoke(StateId, old_state);

            protected override void ExitStateInternal(StateIds new_state) => _exitFn?.Invoke(StateId, new_state);

            protected override StateIds HandleEventInternal(StateEvents evnt) => _handleEvtFn?.Invoke(evnt) ?? StateIds.UnhandledState;

            Action<StateIds> _addFn;
            Action<StateIds, StateIds> _enterFn;
            Action<StateIds, StateIds> _exitFn;
            Func<StateEvents, StateIds> _handleEvtFn;
        }

        public void PopulateStateLineage(IStateMachine<StateIds, StateEvents> stateMachine, Action<StateIds> addFn = null, Action<StateIds, StateIds> enterFn = null, Action<StateIds, StateIds> exitFn = null)
        {
            // State1 -----+-----State2 ----+----State3
            //             |                |
            //             |                +----State4 --+-- State5 ----- State6
            //             |                |             |
            //             |                |             +-- State7
            //             |                |
            //             |                +----State8
            //             |
            //             +----State9
            //
            stateMachine.AddState(new SampleState(StateIds.State1, addFn, enterFn, exitFn));
            stateMachine.AddState(new SampleState(StateIds.State2, addFn, enterFn, exitFn), StateIds.State1);
            stateMachine.AddState(new SampleState(StateIds.State3, addFn, enterFn, exitFn), StateIds.State2);
            stateMachine.AddState(new SampleState(StateIds.State4, addFn, enterFn, exitFn), StateIds.State2);
            stateMachine.AddState(new SampleState(StateIds.State5, addFn, enterFn, exitFn), StateIds.State4);
            stateMachine.AddState(new SampleState(StateIds.State6, addFn, enterFn, exitFn), StateIds.State5);
            stateMachine.AddState(new SampleState(StateIds.State7, addFn, enterFn, exitFn), StateIds.State4);
            stateMachine.AddState(new SampleState(StateIds.State8, addFn, enterFn, exitFn), StateIds.State2);
            stateMachine.AddState(new SampleState(StateIds.State9, addFn, enterFn, exitFn), StateIds.State1);
        }

        [TestMethod]
        public void StateMachine_ctor_SameStateAndInvalidStateMustBeDifferent()
        {
            new Action(() => new StateMachine<StateIds, StateEvents>(StateIds.SameState, StateIds.SameState))
                .Should().Throw<ArgumentException>("sameState cannot be the same value as unhandledState");
        }

        [TestMethod]
        public void StateMachine_ctor_SameStateAndInvalidStateAreDifferent()
        {
            StateMachine<StateIds, StateEvents> sm = new(StateIds.SameState, StateIds.UnhandledState);
            
            sm.SameState.Should().Be(StateIds.SameState);
            sm.UnhandledState.Should().Be(StateIds.UnhandledState);
        }

        [TestMethod]
        public void StateMachine_ctor_InitialStateDefaultsToUnhandled()
        {
            StateMachine<StateIds, StateEvents> sm = new(StateIds.SameState, StateIds.UnhandledState);

            sm.InitialState.Should().Be(StateIds.UnhandledState);
        }

        [TestMethod]
        public void StateMachine_ctor_InitialStateCantBeSameorUnhandledState()
        {
            new Action(() => new StateMachine<StateIds, StateEvents>(StateIds.SameState, StateIds.UnhandledState, StateIds.SameState))
                .Should().Throw<ArgumentException>("initialState cannot be the same value as sameState");
            new Action(() => new StateMachine<StateIds, StateEvents>(StateIds.SameState, StateIds.UnhandledState, StateIds.UnhandledState))
                .Should().Throw<ArgumentException>("initialState cannot be the same value as unhandledState");
        }

        [TestMethod]
        public void StateMachine_ctor_ValidWithInitialState()
        {
            StateMachine<StateIds, StateEvents> sm = new(StateIds.SameState, StateIds.UnhandledState, StateIds.State1);

            sm.SameState.Should().Be(StateIds.SameState);
            sm.UnhandledState.Should().Be(StateIds.UnhandledState);
            sm.InitialState.Should().Be(StateIds.State1);
        }

        [TestMethod]
        public void StateMachine_CantAddStateWithSameState()
        {
            StateMachine<StateIds, StateEvents> sm = new(StateIds.SameState, StateIds.UnhandledState);

            new Action(() => sm.AddState(new SampleState(StateIds.SameState)))
                .Should().Throw<ArgumentException>("state cannot be the SameState state");
        }

        [TestMethod]
        public void StateMachine_CantAddStateWithUnhandledState()
        {
            StateMachine<StateIds, StateEvents> sm = new(StateIds.SameState, StateIds.UnhandledState);

            new Action(() => sm.AddState(new SampleState(StateIds.UnhandledState)))
                .Should().Throw<ArgumentException>("state cannot be the UnhandledState state");
        }

        [TestMethod]
        public void StateMachine_CantAddAnExistingState()
        {
            StateMachine<StateIds, StateEvents> sm = new(StateIds.SameState, StateIds.UnhandledState);
            sm.AddState(new SampleState(StateIds.State1));

            new Action(() => sm.AddState(new SampleState(StateIds.State1)))
                .Should().Throw<ArgumentException>("A state with the state id of State1 already exists");
        }

        [TestMethod]
        public void StateMachine_AddingAStateCallsStateAdded()
        {
            StateMachine<StateIds, StateEvents> sm = new(StateIds.SameState, StateIds.UnhandledState);
            var state = new SampleState(StateIds.State1);

            state.AddOccurrences.Should().Be(0);
            sm.AddState(state);
            state.AddOccurrences.Should().Be(1);

            new Action(() => sm.AddState(state))
                .Should().Throw<ArgumentException>("A state with the state id of State1 already exists");
            state.AddOccurrences.Should().Be(1);
        }

        [TestMethod]
        public void StateMachine_CantStartWithoutStates()
        {
            StateMachine<StateIds, StateEvents> sm = new(StateIds.SameState, StateIds.UnhandledState);

            new Action(() => sm.Start())
                .Should().Throw<ApplicationException>("No states have been added");
        }

        [TestMethod]
        public void StateMachine_CantStartWithoutAnInitialState_ctor()
        {
            StateMachine<StateIds, StateEvents> sm = new(StateIds.SameState, StateIds.UnhandledState); // Note no initial state in ctor
            sm.AddState(new SampleState(StateIds.State1));

            new Action(() => sm.Start())
                .Should().Throw<ApplicationException>("The initial state is not valid");

            sm = new(StateIds.SameState, StateIds.UnhandledState, StateIds.State2); // Non-existent state
            sm.AddState(new SampleState(StateIds.State1));

            new Action(() => sm.Start())
                .Should().Throw<ApplicationException>("The initial state is not valid");
        }

        [TestMethod]
        public void StateMachine_CantStartWithoutAnInitialState_AddState()
        {
            StateMachine<StateIds, StateEvents> sm = new(StateIds.SameState, StateIds.UnhandledState); // Note no initial state in ctor
            sm.AddState(new SampleState(StateIds.State1));

            new Action(() => sm.Start(StateIds.State2)) // Non-existent state
                .Should().Throw<ApplicationException>("The initial state is not valid");
        }

        [TestMethod]
        public void StateMachine_InitialStateCtorSetsInitialState()
        {
            StateMachine<StateIds, StateEvents> sm = new(StateIds.SameState, StateIds.UnhandledState, StateIds.State1); 
            sm.InitialState.Should().Be(StateIds.State1);
        }

        [TestMethod]
        public void StateMachine_StartDoesNotSetInitialState()
        {
            StateMachine<StateIds, StateEvents> sm = new(StateIds.SameState, StateIds.UnhandledState, StateIds.State1);
            StringBuilder sb = new();
            Action<StateIds, StateIds> enterAction = new((current, old) => sb.AppendLine($"{old} --> {current}"));

            sm.AddState(new SampleState(StateIds.State1, enterFn: enterAction));
            sm.AddState(new SampleState(StateIds.State2, enterFn: enterAction));
            sm.Start(StateIds.State2);
            sm.InitialState.Should().Be(StateIds.State1);

            // AddState initial state should override ctor initial state (but not store it)
            sb.ToString().Should().Be(
                "UnhandledState --> State2\r\n");
        }

        [TestMethod]
        public void StateMachine_StartAndStopUpdateRunState()
        {
            StateMachine<StateIds, StateEvents> sm = new(StateIds.SameState, StateIds.UnhandledState, StateIds.State1); 
            sm.AddState(new SampleState(StateIds.State1));
            sm.AddState(new SampleState(StateIds.State2));
            sm.IsStarted.Should().BeFalse();
            sm.Start();
            sm.IsStarted.Should().BeTrue();
            sm.Stop();
            sm.IsStarted.Should().BeFalse();
        }

        [TestMethod]
        public void StateMachine_CurrentStateShouldBeValidAfterStart()
        {
            StateMachine<StateIds, StateEvents> sm = new(StateIds.SameState, StateIds.UnhandledState, StateIds.State1); 
            sm.AddState(new SampleState(StateIds.State1));

            sm.CurrentState.Should().Be(StateIds.UnhandledState);
            sm.Start();
            sm.CurrentState.Should().Be(StateIds.State1);
            sm.Stop();
            sm.CurrentState.Should().Be(StateIds.UnhandledState);
        }

        [TestMethod]
        public void StateMachine_StartShouldEnterStatesInOrder()
        {
            // State1 -----+-----State2 ----+----State3
            //             |                |
            //             |                +----State4 --+-- State5 ----- State6
            //             |                |             |
            //             |                |             +-- State7
            //             |                |
            //             |                +----State8
            //             |
            //             +----State9
            //
            StateMachine<StateIds, StateEvents> sm = new(StateIds.SameState, StateIds.UnhandledState, StateIds.State2);
            StringBuilder sb = new();

            Action<StateIds, StateIds> enterAction = new((current, old) => sb.AppendLine($"{old} --> {current}"));
            PopulateStateLineage(sm, enterFn: enterAction);
            sm.Start(StateIds.State7);
            sb.ToString().Should().Be(
                "UnhandledState --> State1\r\n" +
                "UnhandledState --> State2\r\n" +
                "UnhandledState --> State4\r\n" +
                "UnhandledState --> State7\r\n");
        }

        [TestMethod]
        public void StateMachine_StopShouldExitStatesInOrder()
        {
            // State1 -----+-----State2 ----+----State3
            //             |                |
            //             |                +----State4 --+-- State5 ----- State6
            //             |                |             |
            //             |                |             +-- State7
            //             |                |
            //             |                +----State8
            //             |
            //             +----State9
            //
            StateMachine<StateIds, StateEvents> sm = new(StateIds.SameState, StateIds.UnhandledState, StateIds.State2);
            StringBuilder sb = new();

            Action<StateIds, StateIds> exitAction = new((current, neww) => sb.AppendLine($"{current} --> {neww}"));
            PopulateStateLineage(sm, exitFn: exitAction);
            sm.Start(StateIds.State7);
            sm.Stop();
            sb.ToString().Should().Be(
                "State7 --> UnhandledState\r\n" +
                "State4 --> UnhandledState\r\n" +
                "State2 --> UnhandledState\r\n" +
                "State1 --> UnhandledState\r\n");
        }

        [TestMethod]
        public void StateMachine_IsInStateShouldWalkTree()
        {
            // State1 -----+-----State2 ----+----State3
            //             |                |
            //             |                +----State4 --+-- State5 ----- State6
            //             |                |             |
            //             |                |             +-- State7
            //             |                |
            //             |                +----State8
            //             |
            //             +----State9
            //
            StateMachine<StateIds, StateEvents> sm = new(StateIds.SameState, StateIds.UnhandledState, StateIds.State4);
            PopulateStateLineage(sm);
            sm.Start();

            sm.CurrentState.Should().Be(StateIds.State4);

            sm.IsInState(StateIds.State1).Should().BeTrue();
            sm.IsInState(StateIds.State2).Should().BeTrue();
            sm.IsInState(StateIds.State3).Should().BeFalse();
            sm.IsInState(StateIds.State4).Should().BeTrue();
            sm.IsInState(StateIds.State5).Should().BeFalse();
            sm.IsInState(StateIds.State6).Should().BeFalse();
            sm.IsInState(StateIds.State7).Should().BeFalse();
            sm.IsInState(StateIds.State8).Should().BeFalse();
            sm.IsInState(StateIds.State9).Should().BeFalse();
        }

        [TestMethod]
        public void StateMachine_SimpleTransition()
        {
            StateMachine<StateIds, StateEvents> sm = new(StateIds.SameState, StateIds.UnhandledState, StateIds.State1);
            StringBuilder sbEnter = new();
            StringBuilder sbExit = new();
            Action<StateIds, StateIds> enterAction = new((current, old) => sbEnter.AppendLine($"{old} --> {current}"));
            Action<StateIds, StateIds> exitAction = new((current, neww) => sbExit.AppendLine($"{current} --> {neww}"));
            sm.AddState(new SampleState(StateIds.State1, enterFn: enterAction, exitFn: exitAction));
            sm.AddState(new SampleState(StateIds.State2, enterFn: enterAction, exitFn: exitAction));

            // Start up in intial state
            sm.Start();
            sbEnter.ToString().Should().Be(
                "UnhandledState --> State1\r\n");
            sbExit.ToString().Should().Be(String.Empty);
            sbEnter.Clear();
            sbExit.Clear();
            sm.CurrentState.Should().Be(StateIds.State1);

            // Perform the transition
            ((IManualTransition<StateIds>)sm).Transition(StateIds.State2).Should().BeTrue();
            sbEnter.ToString().Should().Be(
                "State1 --> State2\r\n");
            sbExit.ToString().Should().Be(
                "State1 --> State2\r\n");
            sbEnter.Clear();
            sbExit.Clear();
            sm.CurrentState.Should().Be(StateIds.State2);

            // And back to 1
            ((IManualTransition<StateIds>)sm).Transition(StateIds.State1).Should().BeTrue();
            sbEnter.ToString().Should().Be(
                "State2 --> State1\r\n");
            sbExit.ToString().Should().Be(
                "State2 --> State1\r\n");
            sbEnter.Clear();
            sbExit.Clear();
            sm.CurrentState.Should().Be(StateIds.State1);
        }

        [TestMethod]
        public void StateMachine_TransitionWithAnLCA()
        {
            // State1 -----+-----State2 ----+----State3
            //             |                |
            //             |                +----State4 --+-- State5 ----- State6
            //             |                |             |
            //             |                |             +-- State7
            //             |                |
            //             |                +----State8
            //             |
            //             +----State9
            //
            StateMachine<StateIds, StateEvents> sm = new(StateIds.SameState, StateIds.UnhandledState, StateIds.State9);
            StringBuilder sbEnter = new();
            StringBuilder sbExit = new();
            Action<StateIds, StateIds> enterAction = new((current, old) => sbEnter.AppendLine($"{old} --> {current}"));
            Action<StateIds, StateIds> exitAction = new((current, neww) => sbExit.AppendLine($"{current} --> {neww}"));
            PopulateStateLineage(sm, enterFn: enterAction, exitFn: exitAction);
            
            // Start up in intial state
            sm.Start();
            sbEnter.ToString().Should().Be(
                "UnhandledState --> State1\r\n" + 
                "UnhandledState --> State9\r\n");
            sbExit.ToString().Should().Be(String.Empty);
            sbEnter.Clear();
            sbExit.Clear();
            sm.CurrentState.Should().Be(StateIds.State9);

            // Perform the transition
            ((IManualTransition<StateIds>)sm).Transition(StateIds.State7).Should().BeTrue();
            sbEnter.ToString().Should().Be(
                "State9 --> State2\r\n" +
                "State9 --> State4\r\n" +
                "State9 --> State7\r\n");
            sbExit.ToString().Should().Be(
                "State9 --> State7\r\n");
            sbEnter.Clear();
            sbExit.Clear();
            sm.CurrentState.Should().Be(StateIds.State7);

            // And back to 9
            ((IManualTransition<StateIds>)sm).Transition(StateIds.State9).Should().BeTrue();
            sbEnter.ToString().Should().Be(
                "State7 --> State9\r\n");
            sbExit.ToString().Should().Be(
                "State7 --> State9\r\n" +
                "State4 --> State9\r\n" +
                "State2 --> State9\r\n");
            sbEnter.Clear();
            sbExit.Clear();
            sm.CurrentState.Should().Be(StateIds.State9);
        }

        [TestMethod]
        public void StateMachine_TransitionToSameNamedState()
        {
            StateMachine<StateIds, StateEvents> sm = new(StateIds.SameState, StateIds.UnhandledState, StateIds.State1);
            StringBuilder sbEnter = new();
            StringBuilder sbExit = new();
            Action<StateIds, StateIds> enterAction = new((current, old) => sbEnter.AppendLine($"{old} --> {current}"));
            Action<StateIds, StateIds> exitAction = new((current, neww) => sbExit.AppendLine($"{current} --> {neww}"));
            sm.AddState(new SampleState(StateIds.State1, enterFn: enterAction, exitFn: exitAction));

            // Start up in intial state
            sm.Start();
            sbEnter.ToString().Should().Be(
                "UnhandledState --> State1\r\n");
            sbExit.ToString().Should().Be(String.Empty);
            sbEnter.Clear();
            sbExit.Clear();
            sm.CurrentState.Should().Be(StateIds.State1);

            // Perform the transition
            ((IManualTransition<StateIds>)sm).Transition(StateIds.State1).Should().BeTrue();
            sbEnter.ToString().Should().Be(
                "State1 --> State1\r\n");
            sbExit.ToString().Should().Be(
                "State1 --> State1\r\n");
            sbEnter.Clear();
            sbExit.Clear();
            sm.CurrentState.Should().Be(StateIds.State1);
        }

        [TestMethod]
        public void StateMachine_TransitionToSameSpecialState()
        {
            StateMachine<StateIds, StateEvents> sm = new(StateIds.SameState, StateIds.UnhandledState, StateIds.State1);
            StringBuilder sbEnter = new();
            StringBuilder sbExit = new();
            Action<StateIds, StateIds> enterAction = new((current, old) => sbEnter.AppendLine($"{old} --> {current}"));
            Action<StateIds, StateIds> exitAction = new((current, neww) => sbExit.AppendLine($"{current} --> {neww}"));
            sm.AddState(new SampleState(StateIds.State1, enterFn: enterAction, exitFn: exitAction));

            // Start up in intial state
            sm.Start();
            sbEnter.ToString().Should().Be(
                "UnhandledState --> State1\r\n");
            sbExit.ToString().Should().Be(String.Empty);
            sbEnter.Clear();
            sbExit.Clear();
            sm.CurrentState.Should().Be(StateIds.State1);

            // Perform the transition
            ((IManualTransition<StateIds>)sm).Transition(StateIds.SameState).Should().BeTrue();
            sbEnter.ToString().Should().Be(String.Empty);
            sbExit.ToString().Should().Be(String.Empty);
            sbEnter.Clear();
            sbExit.Clear();
            sm.CurrentState.Should().Be(StateIds.State1);
        }

        [TestMethod]
        public void StateMachine_TransitionToUnhandledState()
        {
            StateMachine<StateIds, StateEvents> sm = new(StateIds.SameState, StateIds.UnhandledState, StateIds.State1);
            StringBuilder sbEnter = new();
            StringBuilder sbExit = new();
            Action<StateIds, StateIds> enterAction = new((current, old) => sbEnter.AppendLine($"{old} --> {current}"));
            Action<StateIds, StateIds> exitAction = new((current, neww) => sbExit.AppendLine($"{current} --> {neww}"));
            sm.AddState(new SampleState(StateIds.State1, enterFn: enterAction, exitFn: exitAction));

            // Start up in intial state
            sm.Start();
            sbEnter.ToString().Should().Be(
                "UnhandledState --> State1\r\n");
            sbExit.ToString().Should().Be(String.Empty);
            sbEnter.Clear();
            sbExit.Clear();
            sm.CurrentState.Should().Be(StateIds.State1);

            // Perform the transition
            ((IManualTransition<StateIds>)sm).Transition(StateIds.UnhandledState).Should().BeTrue();
            sbEnter.ToString().Should().Be(String.Empty);
            sbExit.ToString().Should().Be(String.Empty);
            sbEnter.Clear();
            sbExit.Clear();
            sm.CurrentState.Should().Be(StateIds.State1);
        }

        [TestMethod]
        public void StateMachine_CantTransitionAnUnstartedMachine()
        {
            StateMachine<StateIds, StateEvents> sm = new(StateIds.SameState, StateIds.UnhandledState);
            sm.AddState(new SampleState(StateIds.State1));

            // Perform the transition
            ((IManualTransition<StateIds>)sm).Transition(StateIds.UnhandledState).Should().BeFalse();
            sm.CurrentState.Should().Be(StateIds.UnhandledState);
        }

        [TestMethod]
        public void StateMachine_AcceptEventWithStateChange()
        {
            StateMachine<StateIds, StateEvents> sm = new(StateIds.SameState, StateIds.UnhandledState, StateIds.State1);
            sm.AddState(new SampleState(StateIds.State1, handleFn: _evt => _evt.Equals(StateEvents.Event1) ? StateIds.State1 : StateIds.State2));
            sm.AddState(new SampleState(StateIds.State2, handleFn: _evt => _evt.Equals(StateEvents.Event1) ? StateIds.State1 : StateIds.State2));

            // Start up in intial state
            sm.Start();
            sm.CurrentState.Should().Be(StateIds.State1);

            // Perform the transition
            sm.Accept(StateEvents.Event2).Should().BeTrue();
            sm.CurrentState.Should().Be(StateIds.State2);

            // Perform another
            sm.Accept(StateEvents.Event1).Should().BeTrue();
            sm.CurrentState.Should().Be(StateIds.State1);
        }

        [TestMethod]
        public void StateMachine_AcceptEventWithSameState()
        {
            StateMachine<StateIds, StateEvents> sm = new(StateIds.SameState, StateIds.UnhandledState, StateIds.State1);
            StringBuilder sbEnter = new();
            StringBuilder sbExit = new();
            Action<StateIds, StateIds> enterAction = new((current, old) => sbEnter.AppendLine($"{old} --> {current}"));
            Action<StateIds, StateIds> exitAction = new((current, neww) => sbExit.AppendLine($"{current} --> {neww}"));
            sm.AddState(new SampleState(StateIds.State1, enterFn: enterAction, exitFn: exitAction, handleFn: _evt => StateIds.SameState));

            // Start up in intial state
            sm.Start();
            sm.CurrentState.Should().Be(StateIds.State1);
            sbEnter.Clear();
            sbExit.Clear();

            // Perform the transition
            sm.Accept(StateEvents.Event2).Should().BeTrue();
            sm.CurrentState.Should().Be(StateIds.State1);
            sbEnter.ToString().Should().Be(string.Empty); // No transition
            sbExit.ToString().Should().Be(string.Empty); // No transition
        }

        [TestMethod]
        public void StateMachine_AcceptEventWithSameNamedState()
        {
            StateMachine<StateIds, StateEvents> sm = new(StateIds.SameState, StateIds.UnhandledState, StateIds.State1);
            StringBuilder sbEnter = new();
            StringBuilder sbExit = new();
            Action<StateIds, StateIds> enterAction = new((current, old) => sbEnter.AppendLine($"{old} --> {current}"));
            Action<StateIds, StateIds> exitAction = new((current, neww) => sbExit.AppendLine($"{current} --> {neww}"));
            sm.AddState(new SampleState(StateIds.State1, enterFn: enterAction, exitFn: exitAction, handleFn: _evt => StateIds.State1));

            // Start up in intial state
            sm.Start();
            sm.CurrentState.Should().Be(StateIds.State1);
            sbEnter.Clear();
            sbExit.Clear();

            // Perform the transition
            sm.Accept(StateEvents.Event2).Should().BeTrue();
            sm.CurrentState.Should().Be(StateIds.State1);
            sbEnter.ToString().Should().Be("State1 --> State1\r\n"); // No transition
            sbExit.ToString().Should().Be("State1 --> State1\r\n"); // No transition
        }

        [TestMethod]
        public void StateMachine_AcceptEventWithUnhandledState()
        {
            StateMachine<StateIds, StateEvents> sm = new(StateIds.SameState, StateIds.UnhandledState, StateIds.State1);
            StringBuilder sbEnter = new();
            StringBuilder sbExit = new();
            Action<StateIds, StateIds> enterAction = new((current, old) => sbEnter.AppendLine($"{old} --> {current}"));
            Action<StateIds, StateIds> exitAction = new((current, neww) => sbExit.AppendLine($"{current} --> {neww}"));
            sm.AddState(new SampleState(StateIds.State1, enterFn: enterAction, exitFn: exitAction, handleFn: _evt => StateIds.UnhandledState));

            // Start up in intial state
            sm.Start();
            sm.CurrentState.Should().Be(StateIds.State1);
            sbEnter.Clear();
            sbExit.Clear();

            // Perform the transition
            sm.Accept(StateEvents.Event2).Should().BeFalse();
            sm.CurrentState.Should().Be(StateIds.State1);
            sbEnter.ToString().Should().Be(string.Empty); // No transition
            sbExit.ToString().Should().Be(string.Empty); // No transition
        }

        [TestMethod]
        public void StateMachine_AcceptEventWithInvalidState()
        {
            StateMachine<StateIds, StateEvents> sm = new(StateIds.SameState, StateIds.UnhandledState, StateIds.State1);
            StringBuilder sbEnter = new();
            StringBuilder sbExit = new();
            Action<StateIds, StateIds> enterAction = new((current, old) => sbEnter.AppendLine($"{old} --> {current}"));
            Action<StateIds, StateIds> exitAction = new((current, neww) => sbExit.AppendLine($"{current} --> {neww}"));
            sm.AddState(new SampleState(StateIds.State1, enterFn: enterAction, exitFn: exitAction, handleFn: _evt => StateIds.State2));

            // Start up in intial state
            sm.Start();
            sm.CurrentState.Should().Be(StateIds.State1);
            sbEnter.Clear();
            sbExit.Clear();

            // Perform the transition
            sm.Accept(StateEvents.Event2).Should().BeFalse();
            sm.CurrentState.Should().Be(StateIds.State1);
            sbEnter.ToString().Should().Be(string.Empty); // No transition
            sbExit.ToString().Should().Be(string.Empty); // No transition
        }
    }
}
