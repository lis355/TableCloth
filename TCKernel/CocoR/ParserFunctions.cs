using System;
using System.Collections.Generic;
using System.Text;

namespace TableClothKernel
{
    // ParserFunctions
    partial class Parser
    {
        public RootNode Tree = new RootNode();
        Node Cur;

        void AddCh( Node c ) { Cur.Childs.Add( c ); Cur = c; }
        void AddChC( Node c ) { Cur.Childs.Add( c ); }

        Stack<ExpressionVertex> EV = new Stack<ExpressionVertex>( 50 );


        public string
            tmpIdentifier = "",
            errorString = "";

        Expression tmpExpression = null;

        void PushTrueConstant()
        {
            TcDebug.Log( "const 1" );
            EV.Push( new ConstantVertex( BooleanConstants.True ) );
        }

        void PushFalseConstant()
        {
            TcDebug.Log("const 0");
            EV.Push( new ConstantVertex( BooleanConstants.False ) );
        }

        void PushPirse()
        {
            TcDebug.Log("pirse");
            EV.Push( new OperandVertex( CommandsCode.Pirse, EV.Pop(), EV.Pop() ) );
        }

        void PushShef()
        {
            TcDebug.Log("shef");
            EV.Push( new OperandVertex( CommandsCode.Sheffer, EV.Pop(), EV.Pop() ) );
        }

        void PushEqu()
        {
            TcDebug.Log("equ");
            EV.Push( new OperandVertex( CommandsCode.Equivalence, EV.Pop(), EV.Pop() ) );
        }

        void PushImpl()
        {
            TcDebug.Log("impl");
            EV.Push( new OperandVertex( CommandsCode.Implication, EV.Pop(), EV.Pop() ) );
        }

        void PushXor()
        {
            TcDebug.Log("xor");
            EV.Push( new OperandVertex( CommandsCode.Xor, EV.Pop(), EV.Pop() ) );
        }

        void PushOr()
        {
            TcDebug.Log("or");
            EV.Push( new OperandVertex( CommandsCode.Or, EV.Pop(), EV.Pop() ) );
        }

        void PushAnd()
        {
            TcDebug.Log("and");
            EV.Push( new OperandVertex( CommandsCode.And, EV.Pop(), EV.Pop() ) );
        }

        void PushNot()
        {
            // !!
            TcDebug.Log("not");
            ExpressionVertex PreviosExp = EV.Pop();
            if ( PreviosExp.IsOperand() )
            {
                if ( ( ( OperandVertex )PreviosExp ).OperationCode != CommandsCode.Not )
                {
                    EV.Push( new OperandVertex( CommandsCode.Not, PreviosExp, null ) );
                }
                else
                {
                    EV.Push( ( ( OperandVertex )PreviosExp ).L ); 
                    TcDebug.Log( "delete previous not" );
                }
            }
            else
            {
                // предыдущая вершина - константа или переменная, если константа, то сразу заменяем ее
                if ( PreviosExp.IsConstant() )
                {
                    ( ( ConstantVertex )PreviosExp ).Constant =
                        ( ( ( ConstantVertex )PreviosExp ).Constant == BooleanConstants.False ) ?
                        BooleanConstants.True : BooleanConstants.False;
                    EV.Push( PreviosExp );
                }
                else
                {
                    EV.Push( new OperandVertex( CommandsCode.Not, PreviosExp, null ) );
                }
            }
        }

        void PushVariable( string Name )
        {
            TcDebug.Log( "var " + Name );
            EV.Push( new VariableVertex( Name ) );
            tmpExpression.AddVariable( Name );
        }
    }
}
