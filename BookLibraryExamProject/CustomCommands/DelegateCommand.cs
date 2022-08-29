using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookLibraryExamProject.CustomCommands
{
    public class DelegateCommand<T> : ICommand
    {
        #region Fields
        private readonly Predicate<T> _canExecute;
        private readonly Action<T> _execute;
        #endregion
        #region Constructors
        public DelegateCommand(Action<T> execute)
           : this(execute, null)
        {
            _execute = execute;
        }
        public DelegateCommand(Action<T> execute, Predicate<T> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            _execute = execute;
            _canExecute = canExecute;
        }
        #endregion
        #region Events
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        #endregion
        #region Methods
        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute((T)parameter);
        }
        public void Execute(object parameter)
        {
            _execute((T)parameter);
        }

        #endregion
    }
}
