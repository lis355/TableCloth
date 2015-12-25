namespace TableClothKernel
{
	public class Function : Operand
	{
	    public string Name { set; get; }
	    public OperandList Operands { set; get; }
	
	    public Function():
	        base()
	    {
	        Operands = new OperandList();
	    }

		public override void Validate()
		{
			Operands.Validate();
		}

		public override Operand Simplify()
		{
			Operands = Operands.Simplify() as OperandList;

			return this;
		}

		public override string ToExpressionString()
		{
			return ExpressionTranscription.GetFunctionTranscription( this );
		}

		public override Operand Calc( Calculator calculator )
		{
			var calcedOperands = new Operand[Operands.Count];

			for ( int i = 0; i < Operands.Count; ++i )
			{
				calcedOperands[i] = Operands[i].Calc( calculator );
			}

			var result = calculator.TryCalcDefinedFunction( Name, calcedOperands );
			if ( result == null )
			{
				return new Function
				{
					Name = Name,
					Operands = new OperandList( calcedOperands )
				};
			}

			return result;
		}
	}
}
