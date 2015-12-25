namespace TableClothKernel
{
	/// <summary>
	/// Выполняет рассчет выражения
	/// </summary>
	public class Calculator
	{
		readonly VariableList _variables;

		public Calculator( VariableList variables )
		{
			_variables = variables;
		}

		public Operand CalcVariable( Variable variable )
		{
			var varExp = _variables.Find( variable.Name );
			if ( varExp != null )
			{
				return varExp.Root.Calc( this );
			}

			return variable;
		}

		public Expression Calc( Expression exp )
		{
			return new Expression { Root = exp.Root.Calc( this ) };
		}

		public Operand TryCalcDefinedFunction( string name, params Operand[] operands )
		{
			return CalcProvider.TryCalc( name, operands );
		}
	}
}
