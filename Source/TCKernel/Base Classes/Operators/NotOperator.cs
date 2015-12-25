namespace TableClothKernel
{
	public class NotOperator : Operator
	{
		public Operand Operand { get { return Operands[0]; } set { Operands[0] = value; } }

		public NotOperator( Operand operand ):
			base( EOperator.Not )
		{
			Operands.Add( operand.Simplify() );
		}

		public override void Validate()
		{
			base.Validate();

			if ( Operands.Count != 1 )
				throw new TcException( "Uncorrect operands count" );
			
			if ( Operand is OperandList )
					throw new TcException( "Operands in's simply" );
		}

		public override Operand Simplify()
		{
			Operand = Operand.Simplify();

			if ( Operand is Constant )
			{
				return Expression.CalcSimplyMonoFunction( Type, Operand as Constant );
			}
			else
			{
				var secondNot = Operand as Operator;
				if ( secondNot is NotOperator )
				{
					return ( secondNot as NotOperator ).Operand;
				}
			}

			return this;
		}

		public override Operand Calc( Calculator calculator )
		{
			var op = new NotOperator( Operand.Calc( calculator ) );
			return op.Simplify();
		}
	}
}
