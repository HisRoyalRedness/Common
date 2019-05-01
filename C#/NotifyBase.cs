using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

#if REACTIVE
using System.Reactive.Subjects;
using System.Reactive.Linq;
#endif

namespace HisRoyalRedness.com
{
    #region NotifyBase
    public abstract class NotifyBase : NotifyBase<object>
    {
        protected NotifyBase()
            : base(null, null)
        { }

        protected NotifyBase(object propertyLock)
            : base(null, propertyLock)
        { }

        protected NotifyBase(Dispatcher dispatcher)
            : base(dispatcher, null)
        { }

        protected NotifyBase(Dispatcher dispatcher, object propertyLock)
            : base(dispatcher, propertyLock)
        { }
    }
    #endregion NotifyBase

    #region NotifyBase<TLock>
    public abstract class NotifyBase<TLock> : INotifyPropertyChanged
        where TLock : class
    {
        #region Constructors
        protected NotifyBase()
            : this(null, null)
        { }

        protected NotifyBase(TLock propertyLock)
            : this(null, propertyLock)
        { }

        protected NotifyBase(Dispatcher dispatcher)
            : this(dispatcher, null)
        { }

        protected NotifyBase(Dispatcher dispatcher, TLock propertyLock)
        {
            _dispatcher = dispatcher;
            _propertyLock = propertyLock;
        }
        #endregion Constructors

        #region PropertyChanged event handling
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raise a PropertyChanged event for the given properties
        /// </summary>
        [DebuggerStepThrough]
        protected void NotifyPropertyChanged(params string[] properties)
        {
            foreach (var property in properties)
            {
#if REACTIVE
                _changes.OnNext(property);
#endif
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
            }
        }

#if REACTIVE
        readonly Subject<string> _changes = new Subject<string>();

        public IObservable<string> PropertyChanges
        { get { return _changes; } }

        /// <summary>
        /// Project all echoFrom properties onto echoTo.
        /// This has the effect of making echoFrom changes appears as echoTo changes as well.
        /// </summary>
        protected void EchoChanges(string echoTo, params string[] echoFrom)
        {
            _changes.Where(prop => echoFrom.Contains(prop)).Subscribe(prop => NotifyPropertyChanged(echoTo));
        }
#endif
        #endregion PropertyChanged event handling

        #region Getters
        [DebuggerStepThrough]
        protected TValue GetProperty<TValue>(ref TValue propertyMember)
        {
            if (_propertyLock == null)
                return propertyMember;

            TValue value;
            lock (_propertyLock)
                value = propertyMember;
            return value;
        }
        #endregion Getters

        #region Setters
        /// <summary>
        /// Attempt to set the property. If the given value is different from the current value,
        /// <paramref name="actionIfChanged"/> will be executed (if it's not null), 
        /// and then a PropertyChanged event will be raised.
        /// </summary>
        [DebuggerStepThrough]
        protected bool SetProperty<TValue>(ref TValue propertyMember, TValue newValue, Action<TValue> actionIfChanged = null, [CallerMemberName]string propertyName = "")
            => SetProperty(ref propertyMember, newValue, _dispatcher, actionIfChanged, propertyName);

        /// <summary>
        /// Attempt to set the property. If the given value is different from the current value,
        /// <paramref name="actionIfChanged"/> will be executed (if it's not null), 
        /// and then a PropertyChanged event will be raised.
        /// </summary>
        [DebuggerStepThrough]
        protected bool SetProperty<TValue>(ref TValue propertyMember, TValue newValue, Dispatcher dispatcher, Action<TValue> actionIfChanged = null, [CallerMemberName]string propertyName = "")
        {
            if (propertyName == null)
                throw new ArgumentNullException(nameof(propertyName));

            var hasChanged = SetValueInLock(ref propertyMember, newValue, _propertyLock);
            if (hasChanged)
                NotifyHasChanged(newValue, dispatcher, actionIfChanged, propertyName);

            return hasChanged;
        }

        /// <summary>
        /// Attempt to set the property. If the given value is different from the current value,
        /// <paramref name="actionIfChanged"/> will be executed (if it's not null), 
        /// and then a PropertyChanged event will be raised.
        /// </summary>
        [DebuggerStepThrough]
        protected Task<bool> SetPropertyAsync<TValue>(ref TValue propertyMember, TValue newValue, Action<TValue> actionIfChanged = null, [CallerMemberName]string propertyName = "")
            => SetPropertyAsync(ref propertyMember, newValue, null, actionIfChanged, propertyName);

        /// <summary>
        /// Attempt to set the property. If the given value is different from the current value,
        /// <paramref name="actionIfChanged"/> will be executed (if it's not null), 
        /// and then a PropertyChanged event will be raised.
        /// </summary>
        [DebuggerStepThrough]
        protected Task<bool> SetPropertyAsync<TValue>(ref TValue propertyMember, TValue newValue, Dispatcher dispatcher, Action<TValue> actionIfChanged = null, [CallerMemberName]string propertyName = "")
        {
            if (propertyName == null)
                throw new ArgumentNullException(nameof(propertyName));

            var hasChanged = SetValueInLock(ref propertyMember, newValue, _propertyLock);
            return hasChanged
                ? NotifyHasChanged(newValue, dispatcher, actionIfChanged, propertyName)
                : Task.FromResult(false);
        }

        /// <summary>
        /// Attempt to set the property. If the given value is different from the current value,
        /// <paramref name="actionIfChanged"/> will be executed (if it's not null), 
        /// and then a PropertyChanged event will be raised.
        /// </summary>
        [DebuggerStepThrough]
        protected bool SetProperty<TValue>(Func<TValue> getValue, Action<TValue> setValue, TValue newValue, Action<TValue> actionIfChanged = null, [CallerMemberName]string propertyName = "")
            => SetProperty(getValue, setValue, newValue, _dispatcher, actionIfChanged, propertyName);

        /// <summary>
        /// Attempt to set the property. If the given value is different from the current value,
        /// <paramref name="actionIfChanged"/> will be executed (if it's not null), 
        /// and then a PropertyChanged event will be raised.
        /// </summary>
        [DebuggerStepThrough]
        protected bool SetProperty<TValue>(Func<TValue> getValue, Action<TValue> setValue, TValue newValue, Dispatcher dispatcher, Action<TValue> actionIfChanged = null, [CallerMemberName]string propertyName = "")
        {
            if (propertyName == null)
                throw new ArgumentNullException(nameof(propertyName));

            var hasChanged = SetValueInLock(getValue, setValue, newValue, _propertyLock);
            if (hasChanged)
                NotifyHasChanged(newValue, dispatcher, actionIfChanged, propertyName);

            return hasChanged;
        }

        #region Helpers
        Task<bool> NotifyHasChanged<TValue>(TValue newValue, Dispatcher dispatcher, Action<TValue> actionIfChanged, string propertyName)
        {
            if (dispatcher == null || dispatcher == Dispatcher.CurrentDispatcher)
            {
                actionIfChanged?.Invoke(newValue);
                NotifyPropertyChanged(propertyName);
                return Task.FromResult(true);
            }
            else
            {
                return dispatcher.InvokeAsync(() =>
                {
                    actionIfChanged?.Invoke(newValue);
                    NotifyPropertyChanged(propertyName);
                    return true;
                }).Task;
            }
        }

        /// <summary>
        /// Set <paramref name="propertyMember"/> to <paramref name="newValue"/> if it is determined that it differs
        /// from <paramref name="newValue"/>.
        /// If <paramref name="lockObject"/> is not null, do the equality check and value assignment withing a lock.
        /// </summary>
        bool SetValueInLock<TValue>(ref TValue propertyMember, TValue newValue, TLock lockObject)
        {
            var hasChanged = false;
            if (lockObject == null)
            {
                hasChanged = HasChanged(propertyMember, newValue);
                if (hasChanged)
                    propertyMember = newValue;
            }
            else
            {
                lock (lockObject)
                {
                    hasChanged = HasChanged(propertyMember, newValue);
                    if (hasChanged)
                        propertyMember = newValue;
                }
            }
            return hasChanged;
        }

        /// <summary>
        /// If <paramref name="getValue"/> returns a value other than <paramref name="newValue"/>, call
        /// <paramref name="setValue"/> with <paramref name="newValue"/> as it's argument.
        /// If <paramref name="lockObject"/> is not null, do the equality check and value assignment withing a lock.
        /// </summary>
        bool SetValueInLock<TValue>(Func<TValue> getValue, Action<TValue> setValue, TValue newValue, TLock lockObject)
        {
            var hasChanged = false;
            if (lockObject == null)
            {
                hasChanged = HasChanged(getValue(), newValue);
                if (hasChanged)
                    setValue(newValue);
            }
            else
            {
                lock (lockObject)
                {
                    hasChanged = HasChanged(getValue(), newValue);
                    if (hasChanged)
                        setValue(newValue);
                }
            }
            return hasChanged;
        }

        /// <summary>
        /// Test if <paramref name="propertyMember"/> differs from <paramref name="newValue"/>.
        /// If both are null, then false is returned.
        /// If one or the other is null, true is returned.
        /// Otherwise, the value of <code>!propertyMember.Equals(newValue)</code> is returned.
        /// </summary>
        bool HasChanged<TValue>(TValue propertyMember, TValue newValue)
        {
            if (propertyMember == null && newValue == null)
                return false;
            if (propertyMember == null || newValue == null)
                return true;

            return (propertyMember is IEquatable<TValue> propEquatable)
                ? !propEquatable.Equals(newValue)
                : !propertyMember.Equals(newValue);
        }
        #endregion Helpers
        #endregion Setters

        /// <summary>
        /// Returns true if the code is being run from the Designer. False otherwise
        /// </summary>
        public bool IsInDesigner => DesignerProperties.GetIsInDesignMode(new DependencyObject());

        protected readonly TLock _propertyLock = null;
        protected readonly Dispatcher _dispatcher;

        #region RelayCommand
        public class RelayCommand : ICommand
        {
            #region Constructors
            public RelayCommand(Action<object> execute)
                : this(execute, null)
            { }

            public RelayCommand(Action<object> execute, Predicate<object> canExecute)
            {
                _executeAction = execute ?? throw new ArgumentNullException(nameof(execute));
                _canExecutePredicate = canExecute ?? new Predicate<object>(_ => true);
            }
            #endregion Constructors

            #region ICommand Members
            [DebuggerStepThrough]
            public void Execute(object parameter) => _executeAction(parameter);
            public bool CanExecute(object parameter) => _canExecutePredicate(parameter);

            public event EventHandler CanExecuteChanged
            {
                add { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }
            #endregion ICommand Members

            readonly Action<object> _executeAction;
            readonly Predicate<object> _canExecutePredicate;
        }
        #endregion RelayCommand
    }
    #endregion NotifyBase<TLock>
}
