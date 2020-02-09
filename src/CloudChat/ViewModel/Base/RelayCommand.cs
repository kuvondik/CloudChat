using System;
using System.Windows.Input;

namespace CloudChat
{
    public class RelayCommand : ICommand
    {
        public Action mAction;

        public event EventHandler CanExecuteChanged = (sender, e) => { };

        public RelayCommand(Action action)
        {
            mAction = action;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            mAction?.Invoke();
        }
    }
}