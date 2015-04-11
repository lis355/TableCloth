using System;
using System.Drawing;
using System.IO;
using System.Linq;

namespace TableClothKernel
{
	class TestFormController
	{
		readonly TestForm _form;

		readonly Solution _solution;

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
			"!(!1); x = 1 || 0; new( y, 1 ); clear( x );",
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

			Options.PrettyPrint = true;
			
			//TcDebug.PrintLog = true;
			TcDebug.LogDelegate = s => _form.OutBox.Text += s + Environment.NewLine;

			_solution = new Solution();

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
			Options.ConstantOutType = ( EStringConstantType )_form.ConstantType.SelectedItem;
			Options.OperatorOutType = ( EStringOperatorType )_form.OperatorsType.SelectedItem;

			CalcExpression( _form.InBox.Text );
		}

		void CalcExpression( string s )
		{
			_form.OutBox.Text = String.Empty;

			var result = _solution.Input.Process( s );

			if ( !result.Success )
				return;
			
			//TcDebug.Log( result.Output );

			var exp = result.Input.Commands.FirstOrDefault() as Expression;
			if ( exp != null
				&& _form.GenDotCheckBox.Checked )
			{
				GenerateDot( exp );
			}
		}

		void GenerateDot( Expression expression )
		{
			const string imageName = "test.png";

			if ( _form.DotGraphImage.Image != null )
			{
				_form.DotGraphImage.Image.Dispose();
			}

			File.Delete( "test.dot" );
			File.Delete( imageName );

			var g = new DotGraphPlot( expression );
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
