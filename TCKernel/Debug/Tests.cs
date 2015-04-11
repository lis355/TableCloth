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
            //"!( !( 1 ^ 0 ) => !( 1 == wy ) ) && r || !q", 
            //"Or(Not(q),And(r,Not(Implication(Not(Xor(False,True)),Not(Equivalence(wy,True))))))",
			"g(f(x,y,t(x),y),x,y,t(y))",
            //"y = true || false",
            //"y => x()",
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

                var result = Calc.CalcExpression( exp );

                TcDebug.Log( "RESULT:", result.Success );
	            if ( result.Success )
	            {
					//Options.ConstantOutType = EStringConstantType.Number;
					//
					//Options.OperatorOutType = EStringOperatorType.Symbol;
					//TcDebug.Log( result.Result.ToExpressionString() );
					//
					//Options.OperatorOutType = EStringOperatorType.Word;
					//TcDebug.Log( result.Result.ToExpressionString() );
					//
					//Options.OperatorOutType = EStringOperatorType.Function;
					//TcDebug.Log( result.Result.ToExpressionString() );
					//
					//Options.ConstantOutType = EStringConstantType.Word;
					//
					//Options.OperatorOutType = EStringOperatorType.Symbol;
					//TcDebug.Log( result.Result.ToExpressionString() );
					//
					//Options.OperatorOutType = EStringOperatorType.Word;
					//TcDebug.Log( result.Result.ToExpressionString() );
					//
					//Options.OperatorOutType = EStringOperatorType.Function;
					//TcDebug.Log( result.Result.ToExpressionString() );

					Options.ConstantOutType = EStringConstantType.Number;
					Options.OperatorOutType = EStringOperatorType.Symbol;
					TcDebug.Log( result.Result.ToExpressionString() );

					var g = new DotGraphPlot( result.Result );
					g.PlotGraph();
					g.PlotDot();
					g.SaveDotAndImage( @"C:\Program Files (x86)\Graphviz2.38\bin\dot.exe", "test.png" );
	            }

                TcDebug.Log( "" );
            }
        }
    }
}
