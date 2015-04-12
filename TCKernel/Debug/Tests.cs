using System.Diagnostics;
using System.Linq;

namespace TableClothKernel
{
	class Tests
	{
		readonly string[] _trueCases =
		{
			"x = y", "y"
		};

		readonly string[] _badCases =
		{
			"x := y"
		};

		public void Run()
		{
			var solution = new Solution();

			for ( int i = 0; i < _trueCases.Length; i += 2 )
			{
				//Debug.Assert( solution.Input.Process( _trueCases[i] ).Output == _trueCases[i + 1],
				//	_trueCases[i] + " != " + _trueCases[i + 1] );
			}

			for ( int i = 0; i < _badCases.Length; ++i )
			{
				Debug.Assert( !solution.Input.Process( _badCases[i] ).Success );
			}
		}
	}
}
