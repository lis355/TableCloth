using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        public BooleanConstants Constant;

        public ConstantVertex( BooleanConstants BC )
        {
            Constant = BC;
        }

        public override bool CalcExpressionOnThisVertex()
        {
            return ( Constant == BooleanConstants.True ) ? true : false;
        }

        public override bool IsOperand() { return false; }
        public override bool IsConstant() { return true; }
        public override bool IsVariable() { return false; }
        public override string ToString() { return ( Constant == BooleanConstants.True ) ? "1" : "0"; }
    }

    class VariableVertex : ExpressionVertex
    {
        public string Name;

        public VariableVertex( string N )
        {
            Name = N;
        }

        public override bool CalcExpressionOnThisVertex() { return GlobalVariableList.GetCalcValue( Name ); }

        public override bool IsOperand() { return false; }
        public override bool IsConstant() { return false; }
        public override bool IsVariable() { return true; }
        public override string ToString() { return "VAR"; }
    }

    class OperandVertex : ExpressionVertex
    {
        public CommandsCode OperationCode;
        public ExpressionVertex L, R;

        public OperandVertex( CommandsCode C, ExpressionVertex LV, ExpressionVertex RV )
        {
            OperationCode = C;
            L = LV;
            R = RV;
        }

        public override bool CalcExpressionOnThisVertex()
        {
            if ( OperationCode == CommandsCode.Not )
            {
                return !L.CalcExpressionOnThisVertex();
            }
            else
            {
                bool t1 = L.CalcExpressionOnThisVertex(), t2 = R.CalcExpressionOnThisVertex();
                switch ( OperationCode )
                {
                    case CommandsCode.And: return t1 && t2;
                    case CommandsCode.Or: return t1 || t2;
                    case CommandsCode.Xor: return t1 ^ t2;
                    case CommandsCode.Equivalence: return ( !t1 || t2 ) && ( t1 || !t2 );
                    case CommandsCode.Implication: return !t1 || t2;
                    case CommandsCode.Sheffer: return !( t1 && t2 );
                    case CommandsCode.Pirse: return !( t1 || t2 );
                }
            }
            return false;
        }

        public override bool IsOperand() { return true; }
        public override bool IsConstant() { return false; }
        public override bool IsVariable() { return false; }
        public override string ToString()
        {
            if ( OperationCode == CommandsCode.Not )
            {
                return ExpressionString.CommandsCodeToString( OperationCode ) + L.ToString();
            }
            else
            {
                return "(" + L.ToString() + " " + ExpressionString.CommandsCodeToString( OperationCode ) + " " + R.ToString() + ")";
            }
        }
    }
}
