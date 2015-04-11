using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;

namespace TableClothKernel
{
	public class DotGraphPlot
	{
		public class Graph
		{
			public List<TcToken> V;
			public List<Tuple<TcToken, TcToken>> E;

			public Graph()
			{
				V = new List<TcToken>();
				E = new List<Tuple<TcToken, TcToken>>();
			}
		}

		public TcToken Root { get; set; }
		public Graph RootGraph { get; set; }
		public string Dot { get; set; }

		public DotGraphPlot( TcToken root )
		{
			Root = root;
		}

		public void PlotGraph()
		{
			RootGraph = new Graph();
			RecursivePlot( Root );
		}

		void RecursivePlot( TcToken root )
		{
			if ( root is Function )
			{
				RootGraph.V.Add( root );
				foreach ( var child in ( root as Function ).Operands )
				{
					RootGraph.E.Add( new Tuple<TcToken, TcToken>( root, child ) );
					RecursivePlot( child );
				}
			}
			else if ( root is Operand )
			{
				RootGraph.V.Add( root );
			}
			else
			{
				throw new Exception( "Bad tokens." );
			}
		}

		public void PlotDot()
		{
			Dot = String.Empty;

			if ( RootGraph.V.Count == 0 )
				return;

			var writer = new StringWriter( CultureInfo.InvariantCulture );

			writer.WriteLine( "graph" );
			writer.WriteLine( "{" );

			for ( int i = 0; i < RootGraph.V.Count; ++i )
			{
				writer.WriteLine( "\t{0} [ label = {1} ];", "V" + i, RootGraph.V[i].ToDebugString() );
			}

			foreach ( var e in RootGraph.E ) 
			{
				writer.WriteLine( "\t{0} -- {1};", 
					"V" + RootGraph.V.IndexOf( e.Item1 ),
					"V" + RootGraph.V.IndexOf( e.Item2 ) );
			}

			writer.WriteLine( "}" );

			writer.Flush();
			Dot = writer.ToString();
		}

		public void SaveDotAndImage( string dotExe, string pngFile )
		{
			var dotPath = Path.ChangeExtension( pngFile, "dot" );
			File.WriteAllText( dotPath, Dot );
			var args = dotPath + " -Tpng -o" + pngFile;
			Process.Start( dotExe, args );
		}
	}
}
