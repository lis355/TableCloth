using System;
using System.IO;

namespace TableClothWork
{
	public class PreferencesBase
	{
		protected PreferencesBase() { }

		protected static T Load<T>( string filePath ) where T : new()
		{
			T result;

			try
			{
				result = GJson.Serializator.Deserialize<T>( GJson.JsonValue.Parse( File.ReadAllText( filePath ) ) );
			}
			catch ( Exception ex )
			{
				result = new T();
			}

			return result;
		}

		protected static void Save<T>( T preferences, string filePath )
		{
			var writer = new GJson.IdentWriter();
			GJson.Serializator.Serialize( preferences ).Write( writer );
			File.WriteAllText( filePath, writer.ToString() );
		}
	}
}
