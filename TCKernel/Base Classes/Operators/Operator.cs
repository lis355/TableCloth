namespace TableClothKernel
{
	public abstract class Operator : Function
	{
		public EOperator Type { get; private set; }

		protected Operator( EOperator type ):
			base()
		{
			Type = type;
		}

		public override string ToExpressionString()
		{
			return ExpressionTranscription.GetOperatorTranscription( this );
		}

		public static NotOperator Not( Operand operand )
		{
			return new NotOperator( operand );
		}

		public static AndOperator And( Operand firstOperand, Operand secondOperand )
		{
			return new AndOperator( firstOperand, secondOperand );
		}

		public static OrOperator Or( Operand firstOperand, Operand secondOperand )
		{
			return new OrOperator( firstOperand, secondOperand );
		}

		public static XorOperator Xor( Operand firstOperand, Operand secondOperand )
		{
			return new XorOperator( firstOperand, secondOperand );
		}

		public static EquivalenceOperator Equivalence( Operand firstOperand, Operand secondOperand )
		{
			return new EquivalenceOperator( firstOperand, secondOperand );
		}

		public static ImplicationOperator Implication( Operand firstOperand, Operand secondOperand )
		{
			return new ImplicationOperator( firstOperand, secondOperand );
		}

		public static ShefferOperator Sheffer( Operand firstOperand, Operand secondOperand )
		{
			return new ShefferOperator( firstOperand, secondOperand );
		}

		public static PirseOperator Pirse( Operand firstOperand, Operand secondOperand )
		{
			return new PirseOperator( firstOperand, secondOperand );
		}
	}
}
