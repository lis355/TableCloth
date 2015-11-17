using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Input;
using GJson;

namespace TableClothWork
{
	public class NoteViewModel : ViewModelBase
	{
		public NoteData Data { get; private set; }

		public NoteViewModel():
			this( CreateNewData() )
		{
			Changed = false;
			Saved = false;

			_inputs.CollectionChanged += Inputs_CollectionChanged;
		}

		public NoteViewModel( NoteData data )
		{
			Data = data;

			foreach ( var space in Data.Spaces )
			{
				_inputs.Add( new UserInputViewModel( space ) );
			}

			Changed = false;
			Saved = true;
		}

		static NoteData CreateNewData()
		{
			var data = new NoteData();
			var space = new NoteData.UserInputData();
			data.Spaces.Add( space );

			return data;
		}

		void Inputs_CollectionChanged( object sender, NotifyCollectionChangedEventArgs e )
		{
			// TODO по нормальному запилить

			Data.Spaces.Clear();

			foreach ( var input in _inputs )
			{
				Data.Spaces.Add( input.Data );
			}
		}

		/// <summary>
		/// Имя тетради
		/// </summary>
        public string NoteName
        {
            get
            {
				return Data.Name;
            }
            set
            {
				Data.Name = value;
                OnPropertyChanged( "NoteName" );
            }
        }

		/// <summary>
		/// Список пространств ввода
		/// </summary>
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

		/// <summary>
		/// Были какие-то изменения. 
		/// </summary>
		bool _changed;
		public bool Changed
		{
			get
			{
				return _changed;
			}
			set
			{
				_changed = value;
				OnPropertyChanged( "Changed" );
			}
		}

		/// <summary>
		/// Сохранялась или нет
		/// </summary>
		bool _saved;
		public bool Saved
		{
			get
			{
				return _saved;
			}
			set
			{
				_saved = value;
				OnPropertyChanged( "Saved" );
			}
		}

		public bool NeedSave
		{
			get
			{
				return !Changed && Saved;
			}
		}

		ICommand _addNewUserInputCommand;
		public ICommand AddNewUserInputCommand
		{
			get
			{
				if ( _addNewUserInputCommand == null )
				{
					_addNewUserInputCommand = new SimpleCommand( x =>
					{
						Inputs.Add( new UserInputViewModel() );
					} );
				}

				return _addNewUserInputCommand;
			}
		}

		void SaveDataToTempSettingFile( string file )
		{
			Setting.Instance.WriteTextFile( file, Serializator.Serialize( Data ).ToStringIdent() );
		}

		void SaveDataToFile( string file )
		{
			
		}
	}
}
