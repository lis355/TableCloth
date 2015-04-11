using System;

namespace TableClothKernel
{
	/// <summary>
	/// Математическое выражение
	/// Это либо константа, либо переменная, либо функция,
	/// либо суперпозиция вышеперечисленного
	/// </summary>
    public class Expression : Command
    {
		public Operand Root { get; set; }

		public Expression()
		{
		}

		public override void Validate()
		{
			Root.Validate();
		}

		public void Simplify()
		{
			Root = Root.Simplify();
		}

		public Expression Calc()
		{
			return this;
		}

		public override string ToDebugString()
		{
			return DebugExpressionTranscription.GetOperandCurrentTranscription( Root );
		}

		public override string ToExpressionString()
		{
			return ExpressionTranscription.GetOperandCurrentTranscription( Root );
		}

		///// <summary>
		///// Рассчитать на наборе переменных
		///// </summary>
		//public Expression Calc( VariableList variables )
		//{
		//	return this;
		//}

		public static Constant CalcSimplyMonoFunction( EOperator type, Constant constant )
		{
			switch ( type )
			{
				case EOperator.Not: return !constant;
				default: throw new TcException( "Not a mono function" );
			}
		}

		public static Constant CalcSimplyBinaryFunction( EOperator type, Constant t1, Constant t2 )
		{
			switch ( type )
			{
				case EOperator.And: return t1 && t2;
				case EOperator.Or: return t1 || t2;
				case EOperator.Xor: return t1 ^ t2;
				case EOperator.Equivalence: return ( !t1 || t2 ) && ( t1 || !t2 );
				case EOperator.Implication: return !t1 || t2;
				case EOperator.Sheffer: return !( t1 && t2 );
				case EOperator.Pirse: return !( t1 || t2 );
				default: throw new ArgumentOutOfRangeException( "type" );
			}
		}
    }
}
