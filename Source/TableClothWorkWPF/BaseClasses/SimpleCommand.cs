using System;
using System.Windows.Input;

namespace TableClothWork
{
    public class SimpleCommand : ICommand
    {
        public Predicate<object> CanExecuteDelegate { get; set; }
        public Action<object> ExecuteDelegate { get; set; }

        public SimpleCommand( Action<object> executeDelegate )
        {
            ExecuteDelegate = executeDelegate;
        }

        public bool CanExecute( object parameter )
        {
            return ( CanExecuteDelegate == null )
				|| CanExecuteDelegate( parameter );
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute( object parameter )
        {
            if ( ExecuteDelegate != null )
            {
                ExecuteDelegate( parameter );
            }
        }
    }
}
