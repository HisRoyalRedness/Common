using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Collections;
using System.Windows.Input;
using System.Diagnostics;

#if REACTIVE
using System.Reactive.Subjects;
using System.Reactive.Linq;
#endif

namespace fletcher.org
{
    public abstract class NotifyBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

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

        /// <summary>
        /// Attempt to set the property. If the given value is different from the current value,
        /// a PropertyChanged event will be raised.
        /// </summary>
        [DebuggerStepThrough]
        protected bool SetProperty<T>(string propertyName, T newValue, ref T propertyMember)
        {
            if (propertyName == null)
                throw new ArgumentNullException(nameof(propertyName));

            if ((propertyMember == null && newValue == null) ||
                (propertyMember != null && propertyMember.Equals(newValue)))
                return false;
            else
            {
                propertyMember = newValue;
                NotifyPropertyChanged(propertyName);
                return true;
            }
        }

        /// <summary>
        /// Attempt to set the property. If the given value is different from the current value,
        /// a PropertyChanged event will be raised, and the actionIfChanged Action will be run.
        /// </summary>
        [DebuggerStepThrough]
        protected bool SetProperty<T>(string propertyName, T newValue, ref T propertyMember, Action<T> actionIfChanged)
        {
            if (actionIfChanged == null)
                throw new ArgumentNullException(nameof(actionIfChanged));
            var res = SetProperty<T>(propertyName, newValue, ref propertyMember);
            if (res)
                actionIfChanged(newValue);
            return res;
        }

        [DebuggerStepThrough]
        protected bool SetProperty<T>(Expression<Func<T>> property, T newValue, ref T propertyMember)
        {
            if (property == null)
                throw new ArgumentNullException(nameof(property));
            return SetProperty(GetPropertyName(property), newValue, ref propertyMember); 
        }

        /// <summary>
        /// Attempt to set the property. If the given value is different from the current value,
        /// a PropertyChanged event will be raised, and the actionIfChanged Action will be run.
        /// </summary>
		[DebuggerStepThrough]
        protected bool SetProperty<T>(Expression<Func<T>> property, T newValue, ref T propertyMember, Action<T> actionIfChanged)
        {
            if (property == null)
                throw new ArgumentNullException(nameof(property));
            return SetProperty<T>(GetPropertyName(property), newValue, ref propertyMember, actionIfChanged);
        }

        /// <summary>
        /// Raise a PropertyChanged event for this given properties
        /// </summary>        
        protected void NotifyPropertyChanged(params string[] properties)
        {
            foreach (var property in properties)
            {
#if REACTIVE
                _changes.OnNext(property);
#endif
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        /// <summary>
        /// An overloaded method to use an expression tree to create our PropertyChangedEventArgs,
        /// meaning we can be a bit lazy and not use a static string. Usage would be NotifyPropertyChanged(() => NameOfProperty);
        /// </summary>
        /// <param name="property">The changed property we want to notify.</param>
        protected virtual void NotifyPropertyChanged<T>(params Expression<Func<T>>[] properties)
        {
            if (properties == null)
                throw new ArgumentNullException(nameof(properties));
            NotifyPropertyChanged(properties.Select(p => GetPropertyName(p)).ToArray());
        }

        private string GetPropertyName<T>(Expression<Func<T>> property)
        {
            if (property == null)
                throw new ArgumentNullException(nameof(property));
            
            var body = property.Body as MemberExpression;
            if (body == null)
                throw new ArgumentException("Lambda must return a property.");

            return body.Member.Name;
        }

        public class RelayCommand : ICommand
        {
            public RelayCommand(Action<object> execute)
                : this(execute, null)
            { }

            public RelayCommand(Action<object> execute, Predicate<object> canExecute)
            {
                if (execute == null)
                    throw new ArgumentNullException(nameof(execute));
                _executeAction = execute;
                _canExecutePredicate = canExecute;
            }

            [DebuggerStepThrough]
            public bool CanExecute(object parameter) => (_canExecutePredicate == null ? true : _canExecutePredicate(parameter));

            public event EventHandler CanExecuteChanged
            {
                add { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }

            public void Execute(object parameter) => _executeAction(parameter);

            readonly Action<object> _executeAction;
            readonly Predicate<object> _canExecutePredicate;
        }
    }
}
