namespace TableClothWork
{
	public class Preferences : PreferencesBase
	{
		const string _kFilePath = "preferences.json";

		static Preferences _instance;
		public static Preferences Instance
		{
			get
			{
				if ( _instance == null )
				{
					_instance = Load<Preferences>( _kFilePath );
				}

				return _instance;
			}
		}

		public void Save()
		{
			Save( this, _kFilePath );
		}

		public Preferences()
		{
		}
	}
}
