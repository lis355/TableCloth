using System;
using System.Collections.Generic;

namespace TableClothKernel.MathFunctions
{
	public class Algorithms
	{
		public static void BreadthFirstSearch( Operand op, Action<Operand> visitor )
		{
			var q = new Queue<Operand>();
			q.Enqueue( op );

			while ( q.Count > 0  )
			{
				var item = q.Dequeue();
				visitor( item );

				if ( item is Function )
				{
					foreach ( var operand in ( item as Function ).Operands )
					{
						q.Enqueue( operand );
					}	
				}
			}
		}
	}
}
