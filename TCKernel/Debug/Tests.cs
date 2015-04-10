using System;

namespace TableClothKernel
{
    class Tests
    {
        //Equal = '=' .
	    //Not  = '!'  | "[not]" .
	    //And  = "&&" | "[and]" .
	    //Or   = "||" | "[or]" .
	    //Xor  = "^"  | "[xor]" .
	    //Equivalence = "==" | "[equ]" .
	    //Implication = "=>" | "[impl]" .
	    //Sheffer = "[shef]" .
	    //Pirse = "[pirse]" .

        static readonly string[] _expressions =
        {  
            "!( !( 1 ^ 0 ) => !( 1 == wy ) ) && r || !q", 
            "x = false",
            "y = true || false",
            "y => x()",
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
            foreach ( var exp in _expressions )
            {
                TcDebug.Log( exp );
                bool result = Calc.CalcExpression( exp );
                TcDebug.Log( "RESULT:", result );
                TcDebug.Log( "" );
            }

            //Options.ConstantOutType = StringConstantType.Word;
            //
            //for ( int i = 0; i < _expressions.Length; i += 2 )
            //{
            //    CheckExp( i / 2, _expressions[i], _expressions[i + 1] );
            //}
            //
            //foreach ( var vp in GlobalVariableList.V.GetAllVariablesNames() )
            //{
            //    TcDebug.Log( "{ " + vp + " := " + GlobalVariableList.GetCalcValue( vp ) + " } " );
            //}
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
