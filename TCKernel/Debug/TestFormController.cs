using System;
using System.Drawing;
using System.IO;

namespace TableClothKernel
{
	class TestFormController
	{
		readonly TestForm _form;

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

		public TestFormController( TestForm form )
		{
			_form = form;

			_form.Text = Information.KernelName + " " + Information.KernelVersion + " " + Information.KernelAssembly;

			TcDebug.LogDelegate = s => _form.OutBox.Text += s + Environment.NewLine;

			_form.ConstantType.DataSource = Enum.GetValues( typeof( EStringConstantType ) );
			_form.OperatorsType.DataSource = Enum.GetValues( typeof( EStringOperatorType ) );

			_form.InBox.Text = _expressions[0];
			CalcExpression( _form.InBox.Text );
		}

		public void InBoxMouseDoubleClick()
		{
			_form.InBox.Text = String.Empty;
		}

		public void InBoxReturnDown()
		{
			CalcExpression( _form.InBox.Text );
		}

		void CalcExpression( string s )
		{
			_form.OutBox.Text = String.Empty;

			TcDebug.Log( s );
			
			var result = Calc.CalcExpression( s );
			
			TcDebug.Log( "RESULT:", result.Success );

			if ( !result.Success )
				return;

			Options.ConstantOutType = ( EStringConstantType )_form.ConstantType.SelectedItem;
			Options.OperatorOutType = ( EStringOperatorType )_form.OperatorsType.SelectedItem;
			TcDebug.Log( result.Result.ToExpressionString() );

			GenerateDot( result.Result );
		}

		void GenerateDot( TcToken root )
		{
			if ( !_form.GenDotCheckBox.Checked )
				return;

			const string imageName = "test.png";

			if ( _form.DotGraphImage.Image != null )
			{
				_form.DotGraphImage.Image.Dispose();
			}

			File.Delete( "test.dot" );
			File.Delete( imageName );

			var g = new DotGraphPlot( root );
			g.PlotGraph();
			g.PlotDot();
			g.SaveDotAndImage( @"C:\Program Files (x86)\Graphviz2.38\bin\dot.exe", imageName );

			if ( File.Exists( imageName ) )
			{
				var img = Image.FromFile( imageName );
				_form.DotGraphImage.Image = img;
				_form.DotGraphImage.Size = img.Size;
			}
		}
	}
}
