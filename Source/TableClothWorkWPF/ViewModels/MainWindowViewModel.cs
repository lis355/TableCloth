using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls.Dialogs;

namespace TableClothWork
{
	public partial class MainWindowViewModel : ViewModelBase
	{
        const string _kNextNoteName = "Note {0}";

		public MainWindow View { get; set; }

		public RecentFilesManager RecentFiles { get; set; }

		public MainWindowViewModel()
		{
			RecentFiles = new RecentFilesManager();
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

		ICommand _onInitializeCommand;
		public ICommand OnInitializeCommand
		{
			get
			{
				if ( _onInitializeCommand == null )
				{
					_onInitializeCommand = new SimpleCommand( OnInitialize );
				}

				return _onInitializeCommand;
			}
		}

		ICommand _onClosingCommand;
		public ICommand OnClosingCommand
		{
			get
			{
				if ( _onClosingCommand == null )
				{
					_onClosingCommand = new SimpleCommand( OnClosing );
				}

				return _onClosingCommand;
			}
		}

		ICommand _exitCommand;
		public ICommand ExitCommand
		{
			get
			{
				if ( _exitCommand == null )
				{
					_exitCommand = new SimpleCommand( Exit );
				}

				return _exitCommand;
			}
		}

		ICommand _addAndSelectNewNoteCommand;
		public ICommand AddAndSelectNewNoteCommand
		{
			get
			{
				if ( _addAndSelectNewNoteCommand == null )
				{
					_addAndSelectNewNoteCommand = new SimpleCommand( async x =>
					{
						// TODO correct name function

						string result = "";

						do
						{
							result = await View.ShowInputAsync( "!!!", "fds" );
							if ( result == null )
								return;

							if ( result == "" )
								continue;

							AddAndSelectNewNote( result );
						}
						while ( result == "" );
					} );
				}

				return _addAndSelectNewNoteCommand;
			}
		}

        void Exit( object parameter )
        {
            Application.Current.Shutdown();
        }

		void OnClosing( object parameter )
		{
			Preferences.Instance.Save();
		}

        #endregion

        void AddAndSelectNewNote( string name )
        {
            var note = new NoteViewModel();
            note.NoteName = name;
            Notes.Add( note );
            SelectedNote = note;
        }

        string GetNextNewNoteName()
        {
            return String.Format( _kNextNoteName, Notes.Count + 1 );
        }
	}
}
