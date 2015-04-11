namespace TableClothKernel
{
	/// <summary>
	/// Математическое выражение
	/// Это либо константа, либо переменная, либо функция,
	/// либо суперпозиция вышеперечисленного
	/// </summary>
    /*public class Expression
    {
		/// <summary>
		/// Создает выражение по входящему операнду
		/// Проверяет семантику, упрощает по возможности
		/// </summary>
		public static Expression CreateExpression( Operand root )
		{
			throw new TcException( "Bad operands tree" );
		}

		public Operand Root;

		public Expression()
		{
			//Root
		}

		public void Validate()
		{
			
		}

		public void Simplify()
		{
			
		}

		public Expression Calc()
		{
			return this;
		}

		/// <summary>
		/// Рассчитать на наборе переменных
		/// </summary>
		public Expression Calc( VariableList variables )
		{
			return this;
		}
		

        //public override bool CalcExpressionOnThisVertex()
        //{
        //    if ( OperationCode == EOperator.Not )
        //    {
        //        return !L.CalcExpressionOnThisVertex();
        //    }
        //    else
        //    {
        //        bool t1 = L.CalcExpressionOnThisVertex(), t2 = R.CalcExpressionOnThisVertex();
        //        switch ( OperationCode )
        //        {
        //            case EOperator.And: return t1 && t2;
        //            case EOperator.Or: return t1 || t2;
        //            case EOperator.Xor: return t1 ^ t2;
        //            case EOperator.Equivalence: return ( !t1 || t2 ) && ( t1 || !t2 );
        //            case EOperator.Implication: return !t1 || t2;
        //            case EOperator.Sheffer: return !( t1 && t2 );
        //            case EOperator.Pirse: return !( t1 || t2 );
        //        }
        //    }
        //    return false;
        //}
    }*/
}
