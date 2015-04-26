namespace TableClothKernel
{
	public abstract class BinaryOperator : Operator
	{
		public Operand FirstOperand { get { return Operands[0]; } set { Operands[0] = value; } }
		public Operand SecondOperand { get { return Operands[1]; } set { Operands[1] = value; } }

		protected BinaryOperator( EOperator type, Operand firstOperand, Operand secondOperand ):
			base( type )
		{
			Operands.Add( firstOperand.Simplify() );
			Operands.Add( secondOperand.Simplify() );
		}

		public static Operator CreateBinaryOperator( EOperator type, Operand firstOperand, Operand secondOperand )
		{
			switch ( type )
	        {
		        case EOperator.And: return And( firstOperand, secondOperand );
		        case EOperator.Or: return Or( firstOperand, secondOperand );
		        case EOperator.Xor: return Xor( firstOperand, secondOperand );
		        case EOperator.Equivalence: return Equivalence( firstOperand, secondOperand );
		        case EOperator.Implication: return Implication( firstOperand, secondOperand );
		        case EOperator.Sheffer: return Sheffer( firstOperand, secondOperand );
		        case EOperator.Pirse: return Pirse( firstOperand, secondOperand );
		        default: throw new TcException( "Unknown operator type" );
	        }
		}

		public override void Validate()
		{
			base.Validate();

			if ( Operands.Count != 2 )
					throw new TcException( "Uncorrect operands count" );

			foreach ( var operand in Operands )
			{
				if ( operand is OperandList )
					throw new TcException( "Operands in's simply" );
			}
		}

		protected void SimplifyBinaryOperands()
		{
			FirstOperand = FirstOperand.Simplify();
			SecondOperand = SecondOperand.Simplify();
		}

		protected Operand SimplifyBinaryOperator()
		{
			if ( FirstOperand is Constant
				&& SecondOperand is Constant )
			{
				return Expression.CalcSimplyBinaryFunction( Type, 
					FirstOperand as Constant, SecondOperand as Constant );
			}

			return this;
		}

		public override Operand Calc( Calculator calculator )
		{
			var fo = FirstOperand.Calc( calculator );
			var so = SecondOperand.Calc( calculator );

			var op = CreateBinaryOperator( Type, fo, so );
			return op.Simplify();
		}
	}
}
