using System.Collections.Generic;

namespace TableClothKernel
{
    public partial class Parser
    {
        class SToken { }

        class Name : SToken
        {
            public string String { set; get; }
        }

        class Operand : SToken
        {
        }

        class Variable : Operand
        {
            public string String { set; get; }
        }

        class ConstantToken : Operand
        {
            public EBooleanConstants Value { set; get; }
        }

        class Operator : SToken
        {
        }

        class MonoOperator : Operator
        {
        }

        static readonly Stack<SToken> _stack = new Stack<SToken>( 100 );

        void PushToken( SToken t )
        {
            TcDebug.Log( t.GetType().Name );
            _stack.Push( t );
        }

        T PopToken<T>() where T : SToken
        {
            if ( _stack.Count == 0 )
                throw new TcException( new ParserErrors.Data { Text = "Empty tokens stack." } );

            T t = _stack.Pop() as T;
            if ( t == null )
                throw new TcException( new ParserErrors.Data { Text = "Another token in stack." } );

            return t;
        }

        void PushTrueConstant()
        {
            PushToken( new ConstantToken { Value = EBooleanConstants.True } );
        }

        void PushFalseConstant()
        {
            PushToken( new ConstantToken { Value = EBooleanConstants.False } );
        }

        void PushPirse()
        {
            //_stack.Push( new OperandVertex( ECommandsCode.Pirse, _stack.Pop(), _stack.Pop() ) );
        }

        void PushSheffer()
        {
            //_stack.Push( new OperandVertex( ECommandsCode.Sheffer, _stack.Pop(), _stack.Pop() ) );
        }

        void PushEqu()
        {
            //_stack.Push( new OperandVertex( ECommandsCode.Equivalence, _stack.Pop(), _stack.Pop() ) );
        }

        void PushImplication()
        {
            //_stack.Push( new OperandVertex( ECommandsCode.Implication, _stack.Pop(), _stack.Pop() ) );
        }

        void PushXor()
        {
            //_stack.Push( new OperandVertex( ECommandsCode.Xor, _stack.Pop(), _stack.Pop() ) );
        }

        void PushOr()
        {
            //_stack.Push( new OperandVertex( ECommandsCode.Or, _stack.Pop(), _stack.Pop() ) );
        }

        void PushAnd()
        {
            //_stack.Push( new OperandVertex( ECommandsCode.And, _stack.Pop(), _stack.Pop() ) );
        }

        void PushNot()
        {
            //// !!
            //ExpressionVertex PreviosExp = _stack.Pop();
            //if ( PreviosExp.IsOperand() )
            //{
            //    if ( ( ( OperandVertex )PreviosExp ).OperationCode != ECommandsCode.Not )
            //    {
            //        _stack.Push( new OperandVertex( ECommandsCode.Not, PreviosExp, null ) );
            //    }
            //    else
            //    {
            //        _stack.Push( ( ( OperandVertex )PreviosExp ).L );
            //        TcDebug.Log( "delete previous not" );
            //    }
            //}
            //else
            //{
            //    // предыдущая вершина - константа или переменная, если константа, то сразу заменяем ее
            //    if ( PreviosExp.IsConstant() )
            //    {
            //        ( ( ConstantVertex )PreviosExp ).Constant =
            //            ( ( ( ConstantVertex )PreviosExp ).Constant == EBooleanConstants.False )
            //                ? EBooleanConstants.True
            //                : EBooleanConstants.False;
            //        _stack.Push( PreviosExp );
            //    }
            //    else
            //    {
            //        _stack.Push( new OperandVertex( ECommandsCode.Not, PreviosExp, null ) );
            //    }
            //}
        }

        void PushString( string s )
        {
            PushToken( new Name { String = s } );
        }

        void PushVariable( string name )
        {
            //TcDebug.Log( "var " + name );
            //_stack.Push( new VariableVertex( name ) );
            //tmpExpression.AddVariable( name );
        }
    }
}
