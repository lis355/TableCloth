using System;

namespace TableClothKernel
{
    class Tests
    {
        static readonly string[] _expressions =
        {  
            "!( !( 1 ^ 0 ) => !( 1 == wy ) ) && r || !q", "True"
            //"x = false", "False",
            //"y = true || false", "True",
            //"y => x", "True",
            //"q || !0 && 1", "True",
            //"!!!1 && 0 && 1", "False",
            //"!(0 && 1)", "True",
            //"1", "True",
            //"!1", "False",
            //"0 || 0 && !1", "0; ",
            //"!!(0 && 1)", "0; ",
            //"true", "1; "
        };

        public void Run()
        {
            try
            {
                App();
            }
            catch( Exception e )
            {
                TcDebug.Log( "Exception : " + e.Message );
            } 
        }

        void App()
        {
            foreach ( var exp in _expressions )
            {
                bool result = Calc.CalcExpression( exp );
            }

            Options.ConstantOutType = StringConstantType.Word;

            for ( int i = 0; i < _expressions.Length; i += 2 )
            {
                CheckExp( i / 2, _expressions[i], _expressions[i + 1] );
            }

            foreach ( var vp in GlobalVariableList.V.GetAllVariablesNames() )
            {
                TcDebug.Log( "{ " + vp + " := " + GlobalVariableList.GetCalcValue( vp ) + " } " );
            }

            TcDebug.Log( "[not] [and] [or] [xor \u2295] [equ] [impl \u21D2] [shef] [pirse \u2193]" );
        }

        void CheckExp( int num, string exp, string res )
        {
            TcDebug.Log( String.Format( "Exp {0} : {1}\r\n", num, exp ) );

            var result = Calc.CalcExpression( exp );
            TcDebug.Log( String.Format( "ARRAY RES:{0}\r\nCALT RES:{1}\r\n", res, result ) );
            if ( String.Compare( result.ToString(), res, StringComparison.InvariantCultureIgnoreCase ) == 0 )
            {
                TcDebug.Log( String.Format( "SUCCESS" ) );
            }
            TcDebug.Log( Environment.NewLine );
        }
    }
}
