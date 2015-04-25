using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace TableClothKernel
{
	public class DotGraphPlot
	{
		public class Graph
		{
			public class Vertex
			{
				public TcToken Token { get; set; }
			}

			public class Edge : Tuple<Vertex, Vertex>
			{
				public Edge( Vertex v1, Vertex v2 ) :
					base( v1, v2 )
				{
				}
			}

			public List<Vertex> V;
			public List<Edge> E;

			public Graph()
			{
				V = new List<Vertex>();
				E = new List<Edge>();
			}
		}

		public Expression Expression { get; set; }
		public Graph RootGraph { get; set; }
		public string Dot { get; set; }

		public DotGraphPlot( Expression expression )
		{
			Expression = expression;
		}

		public void PlotGraph()
		{
			RootGraph = new Graph();
			RecursivePlot( Expression.Root );
		}

		Graph.Vertex RecursivePlot( Operand root )
		{
			var vertex = new Graph.Vertex { Token = root };
			RootGraph.V.Add( vertex );

			if ( root is Function )
			{
				RecursivePlotOperandList( vertex, ( root as Function ).Operands );
			}
			else if ( root is OperandList )
			{
				RecursivePlotOperandList( vertex, root as OperandList );
			}

			return vertex;
		}

		void RecursivePlotOperandList( Graph.Vertex root, OperandList operands )
		{
			foreach ( var child in operands )
			{
				var childVertex = RecursivePlot( child );
				RootGraph.E.Add( new Graph.Edge( root, childVertex ) );
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
				writer.WriteLine( "\t{0} [ label = {1} ];", "V" + i, GetLabelForVertex( RootGraph.V[i] ) );
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

		string GetLabelForVertex( Graph.Vertex v )
		{
			return "\"" + v.Token.ToDebugString() + "\"";
		}

		public void SaveDotAndImage( string dotExe, string pngFile )
		{
			var dotPath = Path.ChangeExtension( pngFile, "dot" );
			File.WriteAllText( dotPath, Dot );

			var startInfo = new ProcessStartInfo();
			startInfo.FileName = dotExe;
			startInfo.Arguments = dotPath + " -Tpng -o" + pngFile;
			startInfo.CreateNoWindow = true;
			startInfo.WindowStyle = ProcessWindowStyle.Hidden;

			var process = Process.Start( startInfo );
			if ( process != null )
			{
				process.WaitForExit();
			}
		}
	}
}
