using System.Collections.Generic;

namespace TableClothKernel
{
	public class Function : Operand
	{
	    public string Name { set; get; }
	    public List<Operand> Operands { set; get; }
	
	    public Function():
	        base()
	    {
	        Operands = new List<Operand>();
	    }

		public override void Validate()
		{
			foreach ( var operand in Operands )
			{
				operand.Validate();
			}
		}
	}
	
	public class Operator : Function
	{
	    public EOperator Type { get; set; }

		public Operator()
		{
		}

		public override void Validate()
		{
			if ( ExpressionTranscription.SimplyOperatorIsMono( this ) )
			{
				if ( Operands.Count != 1 )
					throw new TcException( "Uncorrect operands count" );
			}
			else
			{
				if ( Operands.Count != 2 )
					throw new TcException( "Uncorrect operands count" );
			}
		}

		public override Operand Simplify()
		{
			switch ( Type )
			{
				case EOperator.Not: return SimplifyNot();
				case EOperator.And: return SimplifyAnd(); 
				case EOperator.Or: return SimplifyOr(); 
				case EOperator.Xor: return SimplifyXor(); 
				case EOperator.Equivalence: return SimplifyEquivalence(); 
				case EOperator.Implication: return SimplifyImplication(); 
				case EOperator.Sheffer: return SimplifySheffer(); 
				case EOperator.Pirse: return SimplifyPirse();
			}

			return this;
		}

		Operand FirstOperand { get { return Operands[0]; } set { Operands[0] = value; } }
		Operand SecondOperand { get { return Operands[1]; } set { Operands[1] = value; } }

		Operand SimplifyNot()
		{
			FirstOperand = FirstOperand.Simplify();

			if ( FirstOperand == Constant.True )
			{
				return Constant.False;
			}
			else if ( FirstOperand == Constant.False )
			{
				return Constant.True;
			}
			else
			{
				var secondNot = FirstOperand as Operator;
				if ( secondNot != null
					&& secondNot.Type == EOperator.Not )
				{
					return FirstOperand;
				}
			}

			return this;
		}

		Operand SimplifyAnd()
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

        Operand SimplifyOr()
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

        Operand SimplifyXor()
		{
			SimplifyBinaryOperands();

			return SimplifyBinaryOperator();
		}

        Operand SimplifyEquivalence()
		{
			SimplifyBinaryOperands();

			return SimplifyBinaryOperator();
		}

        Operand SimplifyImplication()
		{
			SimplifyBinaryOperands();

			if ( FirstOperand == Constant.False )
			{
				return Constant.True;
			}

			return SimplifyBinaryOperator();
		}

        Operand SimplifySheffer()
		{
			SimplifyBinaryOperands();

			if ( FirstOperand == Constant.False 
				|| SecondOperand == Constant.False )
			{
				return Constant.True;
			}

			return SimplifyBinaryOperator();
		}

        Operand SimplifyPirse()
		{
			SimplifyBinaryOperands();

			if ( FirstOperand == Constant.True 
				|| SecondOperand == Constant.True )
			{
				return Constant.False;
			}

			return SimplifyBinaryOperator();
		}

		void SimplifyBinaryOperands()
		{
			FirstOperand = FirstOperand.Simplify();
			SecondOperand = SecondOperand.Simplify();
		}

		Operand SimplifyBinaryOperator()
		{
			if ( FirstOperand is Constant
				&& SecondOperand is Constant )
			{
				return Expression.CalcSimplyBinaryFunction( Type, 
					FirstOperand as Constant, SecondOperand as Constant );
			}

			return this;
		}
	}
}
