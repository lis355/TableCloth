namespace TableClothWork
{
	public class UserInputViewModel : ViewModelBase
	{
		public NoteController Controller { get; set; }

		string _inputText;
		public string InputText
		{
			get
			{
				return _inputText;
			}
			set
			{
				_inputText = value;
				OnPropertyChanged( "InputText" );
			}
		}

		string _outputText;
		public string OutputText
		{
			get
			{
				return _outputText;
			}
			set
			{
				_outputText = value;
				OnPropertyChanged( "OutputText" );
			}
		}
	}
}
