using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace TableClothKernel
{
	public class DotGraphPloter
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
			
			public ReadOnlyCollection<Vertex> V { get { return _v.AsReadOnly(); } }
			public ReadOnlyCollection<Edge> E { get { return _e.AsReadOnly(); } }

			private List<Vertex> _v = new List<Vertex>();
			private List<Edge> _e = new List<Edge>();

			private bool _changed = true;
			private string _dotString;

			public Graph()
			{
			}

			public void AddVertex( Vertex v )
			{
				_v.Add( v );
				_changed = true;
			}

			public void AddEdge( Edge e )
			{
				_e.Add( e );
				_changed = true;
			}

			public string DotString
			{
				get
				{
					if ( _changed )
					{
						_changed = false;
						_dotString = String.Empty;

						if ( _v.Count > 0 )
						{
							var writer = new StringWriter( CultureInfo.InvariantCulture );

							writer.WriteLine( "graph" );
							writer.WriteLine( "{" );

							for ( int i = 0; i < _v.Count; ++i )
							{
								writer.WriteLine( "\t{0} [ label = {1} ];", "V" + i, GetLabelForVertex( _v[i] ) );
							}

							foreach ( var e in _e )
							{
								writer.WriteLine( "\t{0} -- {1};",
									"V" + _v.IndexOf( e.Item1 ),
									"V" + _v.IndexOf( e.Item2 ) );
							}

							writer.WriteLine( "}" );

							writer.Flush();
							_dotString = writer.ToString();
						}
					}

					return _dotString;
                }
			}

			private string GetLabelForVertex( Vertex v )
			{
				return "\"" + v.Token.ToDebugString() + "\"";
			}
		}

		private Expression _expression;
		private Graph _rootGraph;

		public DotGraphPloter( Expression expression )
		{
			_expression = expression;
			PlotGraph();
        }

		private void PlotGraph()
		{
			if ( _rootGraph == null )
			{
				_rootGraph = new Graph();
				RecursivePlot( _expression.Root );
			}
		}

		private Graph.Vertex RecursivePlot( Operand root )
		{
			var vertex = new Graph.Vertex { Token = root };
			_rootGraph.AddVertex( vertex );

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

		private void RecursivePlotOperandList( Graph.Vertex root, OperandList operands )
		{
			foreach ( var child in operands )
			{
				var childVertex = RecursivePlot( child );
				_rootGraph.AddEdge( new Graph.Edge( root, childVertex ) );
			}
		}

		public void SaveDotAndImage( string dotExe, string pngFile )
		{
			PlotGraph();

			var dotPath = Path.ChangeExtension( pngFile, "dot" );
			File.WriteAllText( dotPath, _rootGraph.DotString );

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
