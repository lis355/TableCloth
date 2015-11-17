using System.IO;
using System.Reflection;
using System.Text;
using GJson;

namespace TableClothWork
{
	public class Setting
	{
		static Setting _instance;

		public static Setting Instance
		{
			get
			{
				if ( _instance == null )
				{
					_instance = new Setting();
				}

				return _instance;
			}
		}

		const string _kSettingDirectory = "\\setting\\";

		string _directory;

		public void Initialaze()
		{
			var uri = new System.Uri( Assembly.GetExecutingAssembly().GetName().CodeBase );
			string path = Path.GetDirectoryName( uri.LocalPath );

			_directory = path + _kSettingDirectory;
			if ( !Directory.Exists( _directory ) )
			{
				Directory.CreateDirectory( _directory );
			}
		}

		string GetFileName( string file )
		{
			return _directory + file;
		}

		public void WriteTextFile( string file, string content )
		{
			File.WriteAllText( GetFileName( file ), content, Encoding.UTF8 );
		}

		public string ReadTextFile( string file )
		{
			return File.ReadAllText( GetFileName( file ), Encoding.UTF8 );
		}

		public bool IsFileExists( string file )
		{
			return File.Exists( GetFileName( file ) );
		}

		public void WriteAbsoluteTextFile( string file, string content )
		{
			File.WriteAllText( file, content, Encoding.UTF8 );
		}

		public string ReadAbsoluteTextFile( string file )
		{
			return File.ReadAllText( file, Encoding.UTF8 );
		}

		public bool IsAbsoluteFileExists( string file )
		{
			return File.Exists( file );
		}

		public T DeserializeFile<T>( string file )
		{
			var json = JsonValue.TryParse( ReadTextFile( file ) );
			return Serializator.TryDeserialize<T>( json );
		}

		public void SerializeFile<T>( string file, T content )
		{
			WriteTextFile( file, Serializator.Serialize( content ).ToStringIdent() );
		}

		public T DeserializeAbsoluteFile<T>( string file )
		{
			var json = JsonValue.TryParse( ReadAbsoluteTextFile( file ) );
			return Serializator.TryDeserialize<T>( json );
		}

		public void SerializeAbsoluteFile<T>( string file, T content )
		{
			WriteAbsoluteTextFile( file, Serializator.Serialize( content ).ToStringIdent() );
		}
	}
}