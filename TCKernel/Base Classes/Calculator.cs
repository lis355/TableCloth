using System;
using System.Linq;

namespace TableClothKernel
{
	/// <summary>
	/// Выполняет рассчел выражения
	/// </summary>
	public class Calculator
	{
		readonly VariableList _variables;

		public Calculator( VariableList variables )
		{
			_variables = variables;
		}

		Operand ReplaceVariable( Variable variable )
		{
			var varExp = _variables.Find( variable.Name );
			if ( varExp != null )
			{
				return Calc( varExp.Root );
			}

			return  variable;
		}

		public Expression Calc( Expression exp )
		{
			return new Expression { Root = Calc( exp.Root ) };
		}

		Operand Calc( Operand operand )
		{
			if ( operand == null )
				throw new NullReferenceException();

			if ( operand is Constant )
			{
				return operand;
			}
			else if ( operand is Variable )
			{
				return ReplaceVariable( operand as Variable );
			}
			else if ( operand is Operator )
			{
				return CalcOperator( operand as Operator );
			}
			else if ( operand is Function )
			{
				return CalcFunction( operand as Function );
			}
			else if ( operand is OperandList )
			{
				return operand;
				//throw new TcException( "Can't calc OperandList" );
			}

			throw new TcException( "Unknown operand" );
		}

		Operand CalcOperator( Operator op )
		{
			var calcedOperands = CalcOperands( op.Operands );

			// TODO
			//Operator.

			return  op;
		}

		Operand CalcFunction( Function function )
		{
			var calcedOperands = CalcOperands( function.Operands );

			var result = CalcProvider.TryCalc( function.Name, calcedOperands );
			if ( result == null )
			{
				return new Function
				{
					Name = function.Name,
					Operands = new OperandList( calcedOperands )
				};
			}

			return result;
		}

		Operand[] CalcOperands( OperandList operands )
		{
			return operands.Select( Calc ).ToArray();
		}
	}
}
