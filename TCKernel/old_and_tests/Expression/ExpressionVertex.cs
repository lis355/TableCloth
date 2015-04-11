namespace TableClothKernel
{
    /// <summary>
    /// Представление выражения как дерева. У каждой вершины либо нет сыновей (переменная или константа)
    /// либо один левый сын, либо оба.
    /// </summary>
    abstract class ExpressionVertex
    {
        public abstract bool CalcExpressionOnThisVertex();
        public abstract bool IsOperand();
        public abstract bool IsConstant();
        public abstract bool IsVariable();
    }

    class ConstantVertex : ExpressionVertex
    {
        public EBooleanConstants Constant;

        public ConstantVertex( EBooleanConstants bc )
        {
            Constant = bc;
        }

        public override bool CalcExpressionOnThisVertex()
        {
            return Constant == EBooleanConstants.True;
        }

        public override bool IsOperand() { return false; }
        public override bool IsConstant() { return true; }
        public override bool IsVariable() { return false; }
        public override string ToString() { return ( Constant == EBooleanConstants.True ) ? "1" : "0"; }
    }

    class VariableVertex : ExpressionVertex
    {
        public string Name;

        public VariableVertex( string n )
        {
            Name = n;
        }

        public override bool CalcExpressionOnThisVertex() { return GlobalVariableList.GetCalcValue( Name ); }

        public override bool IsOperand() { return false; }
        public override bool IsConstant() { return false; }
        public override bool IsVariable() { return true; }
        public override string ToString() { return "VAR"; }
    }

    class OperandVertex : ExpressionVertex
    {
        public EOperator OperationCode;
        public ExpressionVertex L, R;

        public OperandVertex( EOperator c, ExpressionVertex lv, ExpressionVertex rv )
        {
            OperationCode = c;
            L = lv;
            R = rv;
        }

        public override bool CalcExpressionOnThisVertex()
        {
            if ( OperationCode == EOperator.Not )
            {
                return !L.CalcExpressionOnThisVertex();
            }
            else
            {
                bool t1 = L.CalcExpressionOnThisVertex(), t2 = R.CalcExpressionOnThisVertex();
                switch ( OperationCode )
                {
                    case EOperator.And: return t1 && t2;
                    case EOperator.Or: return t1 || t2;
                    case EOperator.Xor: return t1 ^ t2;
                    case EOperator.Equivalence: return ( !t1 || t2 ) && ( t1 || !t2 );
                    case EOperator.Implication: return !t1 || t2;
                    case EOperator.Sheffer: return !( t1 && t2 );
                    case EOperator.Pirse: return !( t1 || t2 );
                }
            }
            return false;
        }

        public override bool IsOperand() { return true; }
        public override bool IsConstant() { return false; }
        public override bool IsVariable() { return false; }
        public override string ToString()
        {
            if ( OperationCode == EOperator.Not )
            {
                return ExpressionString.CommandsCodeToString( OperationCode ) + L;
            }
            
            return "(" + L + " " + ExpressionString.CommandsCodeToString( OperationCode ) + " " + R + ")";
        }
    }
}
