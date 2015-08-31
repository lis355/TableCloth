using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace TableClothWork
{
	public class NoteViewModel : ViewModelBase
	{
		readonly NoteController _controller = new NoteController();

		public NoteViewModel()
		{
			AddNewUserInputCommand = new SimpleCommand( AddNewUserInput );
		}

        string _noteName;
        public string NoteName
        {
            get
            {
                return _noteName;
            }
            set
            {
                _noteName = value;
                OnPropertyChanged( "NoteName" );
            }
        }

		ObservableCollection<UserInputViewModel> _inputs = new ObservableCollection<UserInputViewModel>();
		public ObservableCollection<UserInputViewModel> Inputs
		{
			get
			{
				return _inputs;
			}
			set
			{
				if ( _inputs == value )
					return;

				_inputs = value;
				OnPropertyChanged( "Inputs" );
			}
		}

		public ICommand AddNewUserInputCommand { get; private set; }

		void AddNewUserInput( object parameter )
		{
			Inputs.Add( new UserInputViewModel() );
		}
	}
}
