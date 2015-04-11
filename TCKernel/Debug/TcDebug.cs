using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace TableClothKernel
{
    public class TcDebug
    {
	    static void Main()
	    {
		    Log( Information.KernelName + " " + Information.KernelVersion + " " + Information.KernelAssembly );

			Application.EnableVisualStyles();
		    Application.SetCompatibleTextRenderingDefault( false );
		    Application.Run( new TestForm() );

			SaveLog();
	    }

	    static readonly List<string> _logStrings = new List<string>();

		public static bool PrintLog { get; set; }

		public static Action<string> LogDelegate { get; set; }

        public static void Log( params object[] output )
        {
            foreach ( var obj in output )
            {
				var s = obj.ToString();
                var log = String.Format( "[{0}] : {1}", DateTime.Now.ToUniversalTime(), obj );
                _logStrings.Add( log );
	            if ( LogDelegate != null )
	            {
		            LogDelegate( s );
	            }
            }
        }

        public static void SaveLog()
        {
            File.WriteAllLines( "log.txt", _logStrings, Encoding.UTF8 );
        }
    }
}
