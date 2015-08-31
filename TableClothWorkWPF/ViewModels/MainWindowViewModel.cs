using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace TableClothWork
{
	public class MainWindowViewModel : ViewModelBase
	{
        const string _kTitleWithNodeFormat = "{0} - {1}";
        const string _kNextNoteName = "Note {0}";

		public MainWindowViewModel()
		{
			OnInitializeCommand = new SimpleCommand( OnInitialize );
            ExitCommand = new SimpleCommand( Exit );
		}

        #region Properties

        ObservableCollection<NoteViewModel> _notes = new ObservableCollection<NoteViewModel>();
        public ObservableCollection<NoteViewModel> Notes
        {
            get
            {
                return _notes;
            }
            set
            {
                if ( _notes == value )
                    return;

                _notes = value;
                OnPropertyChanged( "Notes" );
            }
        }

		NoteViewModel _selectedNote;
		public NoteViewModel SelectedNote
		{
			get
			{
				return _selectedNote;
			}
			set
			{
				_selectedNote = value;
				OnPropertyChanged( "SelectedNote" );

                SelectedNote_Changed();
			}
		}

        string _viewModelTitle;
        public string ViewModelTitle
        {
            get
            {
                return _viewModelTitle;
            }
            set
            {
                _viewModelTitle = value;
                OnPropertyChanged( "ViewModelTitle" );
            }
        }

        #endregion

        #region Commands

        public ICommand OnInitializeCommand { get; private set; }
        public ICommand ExitCommand { get; private set; }

        void OnInitialize( object parameter )
        {
            ViewModelTitle = Information.CurrentAssemblyTitleName;
            
            // test
            AddAndSelectNewNote( GetNextNoteName() );
        }

        void Exit( object parameter )
        {
            App.Current.Shutdown();
        }

        #endregion

        void AddAndSelectNewNote( string name = "" )
        {
            var note = new NoteViewModel();
            note.NoteName = name;
            Notes.Add( note );
            SelectedNote = note;
        }

        string GetNextNoteName()
        {
            return String.Format( _kNextNoteName, Notes.Count + 1 );
        }

        void SelectedNote_Changed()
        {
            ViewModelTitle = String.Format( _kTitleWithNodeFormat,
                Information.CurrentAssemblyTitleName,
                SelectedNote.NoteName );
        }
	}
}
