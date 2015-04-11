using System.Collections.Generic;
using System.Linq;

namespace TableClothKernel
{
    public partial class Parser
    {
		readonly Stack<TcToken> _stack = new Stack<TcToken>( 100 );

	    void ClearStack()
	    {
		    _stack.Clear();
	    }

	    public TcToken ParseResult
	    {
		    get
		    {
				if ( _stack.Count != 1 )
					throw new TcException( new ParserErrors.Data { Text = "Bad TcTokens stack." } );

			    return _stack.Peek();
		    }
	    }

	    void PushToken( TcToken token )
	    {
			_stack.Push( token );

		    if ( TcDebug.PrintLog ) 
			{ 
				TcDebug.Log( token.ToDebugString() );
			}
        }

        T PopToken<T>() where T : TcToken
        {
            if ( _stack.Count == 0 )
                throw new TcException( new ParserErrors.Data { Text = "Empty TcTokens stack." } );

            var popTcToken = _stack.Pop() as T;
            if ( popTcToken == null )
                throw new TcException( new ParserErrors.Data { Text = "Another TcToken in stack." } );

            return popTcToken;
        }

        void PushTrueConstant()
        {
            PushToken( new Constant { Value = EBooleanConstants.True } );
        }

        void PushFalseConstant()
        {
            PushToken( new Constant { Value = EBooleanConstants.False } );
        }

        void PushNot()
        {
            PushOperator( EOperator.Not, PopToken<Operand>() );
        }

        void PushPirse()
        {
            PushOperator( EOperator.Pirse, PopToken<Operand>() );
        }

        void PushSheffer()
        {
            PushOperator( EOperator.Sheffer, PopToken<Operand>() );
        }

	    void PushSimplyBinaryOperator( EOperator type )
	    {
			var t1 = PopToken<Operand>();
			var t2 = PopToken<Operand>();
            PushOperator( type, t2, t1 );
	    }

        void PushEquivalence()
        {
			PushSimplyBinaryOperator( EOperator.Equivalence );
        }

        void PushImplication()
        {
			PushSimplyBinaryOperator( EOperator.Implication );
        }

        void PushXor()
        {
            PushSimplyBinaryOperator( EOperator.Xor );
        }

        void PushOr()
        {
            PushSimplyBinaryOperator( EOperator.Or );
        }

        void PushAnd()
        {
            PushSimplyBinaryOperator( EOperator.And );
        }

        void PushOperator( EOperator type, params Operand[] operands )
        {
            PushToken( new Operator { Type = type, Operands = operands.ToList() } );
        }

        void PushVariable( string name )
        {
            PushToken( new Variable { Name = name } );
        }

        void PopVariablePushFunction()
        {
            var variable = PopToken<Variable>();
			PushToken( new Function { Name = variable.Name } );
        }

	    void PushArgumentToFunction()
	    {
			var arg = PopToken<Operand>();
		    var func = PopToken<Function>();
			func.Operands.Add( arg );
			PushToken( func );
	    }
        partial void ProductionBegin( ENonTerminal production )
        {
			switch ( production )
	        {
		        case ENonTerminal.TableCloth: ClearStack(); break;
				case ENonTerminal.FunctionBracketsAndArguments: PopVariablePushFunction(); break;
	        }
        }

        partial void ProductionEnd( ENonTerminal production )
        {
			switch ( production )
            {
                case ENonTerminal.Command: break;
                case ENonTerminal.Expression: break;
                case ENonTerminal.ConstantT: PushTrueConstant(); break;
                case ENonTerminal.ConstantF: PushFalseConstant(); break;
                case ENonTerminal.Identifier: PushVariable( CurrentToken ); break;
                case ENonTerminal.FunctionBracketsAndArguments: break;
            }
        }
    }
}
