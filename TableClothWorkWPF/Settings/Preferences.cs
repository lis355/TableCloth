using System.Timers;
using GJson;

namespace TableClothWork
{
	public class Preferences
	{
		static Preferences _instance;
		public static Preferences Instance
		{
			get
			{
				if ( _instance == null )
				{
					_instance = new Preferences();
				}

				return _instance;
			}
		}
		
		public static string FileName { get; set; }

		static Preferences()
		{
			FileName = "preferences.json";
		}

		JsonValue _json;
		bool _isDirty;
		Timer _timer;

		public Preferences()
		{
			Load();

			_timer = new Timer( 1000 * 20 );
			_timer.Elapsed +=TimerElapsed;
			_timer.Start();
		}

		void TimerElapsed( object sender, ElapsedEventArgs e )
		{
			if ( _isDirty )
			{
				Save();
			}
		}

		void Load()
		{
			try
			{
				_json = JsonValue.Parse( Setting.Instance.ReadTextFile( FileName ) );
			}
			catch
			{
				_json = new JsonValue();
				this["version"] = Information.CurrentAssemblyTitleName;
			}

			_isDirty = false;
		}

		public void Save()
		{
			var writer = new IdentWriter();
			_json.Write( writer );
			Setting.Instance.WriteTextFile( FileName, writer.ToString() );

			_isDirty = false;
		}

		public JsonValue this[string s]
		{
			get
			{
				return _json[s];
			}
			set
			{
				_json[s] = value;

				_isDirty = true;
			}
		}
	}
}
