﻿using System;
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
			public List<TcToken> V;
			public List<Tuple<TcToken, TcToken>> E;

			public Graph()
			{
				V = new List<TcToken>();
				E = new List<Tuple<TcToken, TcToken>>();
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

		void RecursivePlot( Operand root )
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

			RootGraph.V.Add( root );
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

		string GetLabelForVertex( TcToken token )
		{
			return "\"" + token.ToDebugString() + "\"";
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