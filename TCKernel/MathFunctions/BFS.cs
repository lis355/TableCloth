using System;
using System.Collections.Generic;

namespace TableClothKernel.MathFunctions
{
	public class Algorithms
	{
		public static void BreadthFirstSearch( Operand op, Action<Operand> visitor )
		{
			if ( visitor == null )
				throw new ArgumentNullException( "visitor" );

			var q = new Queue<Operand>();
			q.Enqueue( op );

			while ( q.Count > 0 )
			{
				var item = q.Dequeue();
				visitor( item );

				var func = item as Function;
				if ( func != null )
				{
					foreach ( var operand in func.Operands )
					{
						q.Enqueue( operand );
					}	
				}
			}
		}

		public static void DepthFirstSearch( Operand op, Action<Operand> visitor )
		{
			if ( visitor == null )
				throw new ArgumentNullException( "visitor" );

			var q = new Stack<Operand>();
			q.Push( op );

			while ( q.Count > 0 )
			{
				var item = q.Pop();
				visitor( item );

				var func = item as Function;
				if ( func != null )
				{
					foreach ( var operand in func.Operands )
					{
						q.Push( operand );
					}
				}
			}
		}
	}
}
