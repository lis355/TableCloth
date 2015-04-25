namespace TableClothKernel
{
	public class Variable : Operand
	{
	    public string Name { get; private set; }

		public Variable( string name )
		{
			Name = name;
		}

		public override string ToExpressionString()
		{
			return ExpressionTranscription.GetVariableTranscription( this );
		}

		public override Operand Calc( Calculator calculator )
		{
			return calculator.CalcVariable( this );
		}
	}
}
