namespace TableClothWork
{
	public class UserInputViewModel : ViewModelBase
	{
		public NoteData.UserInputData Data { get; private set; }

		public UserInputViewModel() :
			this( new NoteData.UserInputData() )
		{
		}

		public UserInputViewModel( NoteData.UserInputData data )
		{
			Data = data;
		}

		public string InputText
		{
			get
			{
				return Data.In;
			}
			set
			{
				Data.In = value;
				OnPropertyChanged( "InputText" );
			}
		}

		public string OutputText
		{
			get
			{
				return Data.Out;
			}
			set
			{
				Data.Out = value;
				OnPropertyChanged( "OutputText" );
			}
		}

		public bool Minimized
		{
			get
			{
				return Data.Minimized;
			}
			set
			{
				Data.Minimized = value;
				OnPropertyChanged( "Minimized" );
			}
		}
	}
}
