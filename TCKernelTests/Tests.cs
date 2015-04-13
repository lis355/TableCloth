using System;
using System.Diagnostics;
using System.Linq;
using System.IO;

using GJson;
using TableClothKernel;

namespace TCKernelTests
{
	partial class Tests
	{
        Solution _solution = new Solution();

        public void Run()
        {
            var json = JsonValue.Parse( File.ReadAllText( ".\\..\\..\\tests.json" ) );

            Part( json["single"], SingleCommands );
            Part( json["incorrect"], IncorrectCommands );
            Part( json["variables"], Variables );
        }

        void Part( JsonValue json, Action<JsonValue> action )
        {
            ReadOptions( json );

            action( json["cases"] );
        }

        void ReadOptions( JsonValue json )
        {
            var options = json["options"];
            if ( options.Count == 0 )
                return;

            Options.ConstantOutType = ( EStringConstantType )Enum.Parse( typeof( EStringConstantType ), options["ConstantOutType"] );
            Options.OperatorOutType = ( EStringOperatorType )Enum.Parse( typeof( EStringOperatorType ), options["OperatorOutType"] );
            Options.PrettyPrint = options["PrettyPrint"];
        }
	}
}
