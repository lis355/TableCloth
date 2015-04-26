namespace TableClothKernel
{
	public class AndOperator : BinaryOperator
	{
		public AndOperator( Operand firstOperand, Operand secondOperand ):
			base( EOperator.And, firstOperand, secondOperand )
		{
		}

		public override Operand Simplify()
		{
			SimplifyBinaryOperands();

			if ( FirstOperand == Constant.False 
				|| SecondOperand == Constant.False )
			{
				return Constant.False;
			}
			else if ( FirstOperand == Constant.True )
			{
				return SecondOperand;
			}
			else if ( SecondOperand == Constant.True )
			{
				return FirstOperand;
			}
			
			return SimplifyBinaryOperator();
		}
	}

	public class OrOperator : BinaryOperator
	{
		public OrOperator( Operand firstOperand, Operand secondOperand ):
			base( EOperator.Or, firstOperand, secondOperand )
		{
		}

		public override Operand Simplify()
		{
			SimplifyBinaryOperands();

			if ( FirstOperand == Constant.True 
				|| SecondOperand == Constant.True )
			{
				return Constant.True;
			}
			else if ( FirstOperand == Constant.False )
			{
				return SecondOperand;
			}
			else if ( SecondOperand == Constant.False )
			{
				return FirstOperand;
			}
			
			return SimplifyBinaryOperator();
		}
	}

	public class XorOperator : BinaryOperator
	{
		public XorOperator( Operand firstOperand, Operand secondOperand ):
			base( EOperator.Xor, firstOperand, secondOperand )
		{
		}

		public override Operand Simplify()
		{
			SimplifyBinaryOperands();

			return SimplifyBinaryOperator();
		}
	}

	public class EquivalenceOperator : BinaryOperator
	{
		public EquivalenceOperator( Operand firstOperand, Operand secondOperand ):
			base( EOperator.Equivalence, firstOperand, secondOperand )
		{
		}

		public override Operand Simplify()
		{
			SimplifyBinaryOperands();

			return SimplifyBinaryOperator();
		}
	}

	public class ImplicationOperator : BinaryOperator
	{
		public ImplicationOperator( Operand firstOperand, Operand secondOperand ):
			base( EOperator.Implication, firstOperand, secondOperand )
		{
		}

		public override Operand Simplify()
		{
			SimplifyBinaryOperands();

			if ( FirstOperand == Constant.False )
			{
				return Constant.True;
			}

			return SimplifyBinaryOperator();
		}
	}

	public class ShefferOperator : BinaryOperator
	{
		public ShefferOperator( Operand firstOperand, Operand secondOperand ):
			base( EOperator.Sheffer, firstOperand, secondOperand )
		{
		}

		public override Operand Simplify()
		{
			SimplifyBinaryOperands();

			if ( FirstOperand == Constant.False 
				|| SecondOperand == Constant.False )
			{
				return Constant.True;
			}

			return SimplifyBinaryOperator();
		}
	}

	public class PirseOperator : BinaryOperator
	{
		public PirseOperator( Operand firstOperand, Operand secondOperand ):
			base( EOperator.Pirse, firstOperand, secondOperand )
		{
		}

		public override Operand Simplify()
		{
			SimplifyBinaryOperands();

			if ( FirstOperand == Constant.True 
				|| SecondOperand == Constant.True )
			{
				return Constant.False;
			}

			return SimplifyBinaryOperator();
		}
	}
}
