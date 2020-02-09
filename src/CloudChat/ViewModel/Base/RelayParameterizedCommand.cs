using System;
using System.Windows.Input;

namespace CloudChat
{
    /// <summary>
    /// A basic command that runs an Action
    /// </summary>
    public class RelayParameterizedCommand : ICommand
    {
        #region Private Members

        /// <summary>
        /// The action to run
        /// </summary>
        private readonly Action<object> mAction;

        #endregion Private Members

        #region Public events

        /// <summary>
        /// The event thats fired when the <see cref="CanExecute(object)"/> value has changed
        /// </summary>
        public event EventHandler CanExecuteChanged = (sender, e) => { };

        #endregion Public events

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="action"></param>
        public RelayParameterizedCommand(Action<object> action)
        {
            mAction = action;
        }

        #endregion Constructor

        #region Command Methods

        /// <summary>
        /// A relay parameterized command always execute
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            mAction?.Invoke(parameter);
        }

        #endregion Command Methods
    }
}