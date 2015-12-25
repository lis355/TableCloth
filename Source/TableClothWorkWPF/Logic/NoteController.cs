using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TableClothKernel;

namespace TableClothWork
{
	public class NoteController
	{
		readonly Solution _solution = new Solution();

		public string Calc( string exp )
		{
			var result = _solution.Input.Process( exp );
			return String.Join( " ", result.Output.Select( x => x.Output ) );
		}
	}
}
