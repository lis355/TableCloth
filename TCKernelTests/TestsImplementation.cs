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
        void SingleCommands( JsonValue cases )
        {
            for ( int i = 0; i < cases.Count; i += 2 )
            {
                CheckSingleCmd( cases[i], cases[i + 1] );
            }
        }

        void IncorrectCommands( JsonValue cases )
        {
            foreach ( var cs in cases.AsArray )
            {
                Debug.Assert( !_solution.Input.Process( cs ).Success );
            }
        }

        string CalcSingleCmd( string cmd )
        {
            return _solution.Input.Process( cmd ).Output[0].Output;
        }

        void CheckSingleCmd( string cmd, string result )
        {
            Debug.Assert( CalcSingleCmd( cmd ) == result, cmd + "!=" + result );
        }

        void Variables( JsonValue cases )
        {
        }
    }
}
