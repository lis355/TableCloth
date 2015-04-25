namespace TableClothKernel
{
	public class Operator : Function
	{
	    public EOperator Type { get; set; }

		Operator():
			base()
		{
		}

		public Operator( EOperator type, params Operand[] operands ):
			this()
        {
			Type = type;

	        if ( operands != null )
	        {
		        foreach ( var operand in operands )
		        {
			        Operands.Add( operand );
		        }
	        }
        }

		public override string ToExpressionString()
		{
			return ExpressionTranscription.GetOperatorTranscription( this );
		}

		public override Operand Calc( Calculator calculator )
		{
			Operator op;
			if ( Operands.Count == 1 )
			{
				op = new Operator( Type, FirstOperand.Calc( calculator ) );
			}
			else
			{
				op = new Operator( Type, FirstOperand.Calc( calculator ), SecondOperand.Calc( calculator ) );
			}

			return Simplify();
		}

		public static Operand Not( Operand operand )
		{
			var result = new Operator { Type = EOperator.Not };
			result.Operands.Add( operand );
			return result.Simplify();
		}

		static Operand BinaryOperator( EOperator type, Operand firstOperand, Operand secondOperand )
		{
			var result = new Operator { Type = type };
			result.Operands.Add( firstOperand );
			result.Operands.Add( secondOperand );
			return result.Simplify();
		}

		public static Operand And( Operand firstOperand, Operand secondOperand )
		{
			return BinaryOperator( EOperator.And, firstOperand, secondOperand );
		}

		public static Operand Or( Operand firstOperand, Operand secondOperand )
		{
			return BinaryOperator( EOperator.Or, firstOperand, secondOperand );
		}

		public static Operand Xor( Operand firstOperand, Operand secondOperand )
		{
			return BinaryOperator( EOperator.Xor, firstOperand, secondOperand );
		}

		public static Operand Equivalence( Operand firstOperand, Operand secondOperand )
		{
			return BinaryOperator( EOperator.Equivalence, firstOperand, secondOperand );
		}

		public static Operand Implication( Operand firstOperand, Operand secondOperand )
		{
			return BinaryOperator( EOperator.Implication, firstOperand, secondOperand );
		}

		public static Operand Sheffer( Operand firstOperand, Operand secondOperand )
		{
			return BinaryOperator( EOperator.Sheffer, firstOperand, secondOperand );
		}

		public static Operand Pirse( Operand firstOperand, Operand secondOperand )
		{
			return BinaryOperator( EOperator.Pirse, firstOperand, secondOperand );
		}

		public override void Validate()
		{
			base.Validate();

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

			foreach ( var operand in Operands )
			{
				if ( operand is OperandList )
					throw new TcException( "Operands in's simply" );
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

		public Operand FirstOperand { get { return Operands[0]; } set { Operands[0] = value; } }
		public Operand SecondOperand { get { return Operands[1]; } set { Operands[1] = value; } }

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
					return secondNot.FirstOperand;
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
