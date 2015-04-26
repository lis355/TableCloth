using System.Collections.Generic;

namespace TableClothKernel
{
    public partial class Parser
    {
		readonly Stack<TcToken> _stack = new Stack<TcToken>( 100 );

	    void ClearStack()
	    {
		    _stack.Clear();
	    }

	    public UserInput ParseResult
	    {
		    get
		    {
				if ( _stack.Count != 1 
					|| !( _stack.Peek() is UserInput ) )
					throw new TcException( new ParserErrors.Data { Text = "Bad TcTokens stack." } );

			    return _stack.Peek() as UserInput;
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

            var popTcToken = _stack.Pop();
			var popTokenAsT = popTcToken as T;
            if ( popTokenAsT == null )
                throw new TcException( new ParserErrors.Data { Text = "Another TcToken in stack." } );

            return popTokenAsT;
        }

        void PushTrueConstant()
        {
            PushToken( TableClothKernel.Constant.True );
        }

        void PushFalseConstant()
        {
            PushToken( TableClothKernel.Constant.False );
        }

        void PushNot()
        {
            PushOperator( EOperator.Not, PopToken<Operand>() );
        }

	    void PushSimplyBinaryOperator( EOperator type )
	    {
			var t1 = PopToken<Operand>();
			var t2 = PopToken<Operand>();
            PushOperator( type, t2, t1 );
	    }

		void PushPirse()
        {
            PushSimplyBinaryOperator( EOperator.Pirse );
        }

        void PushSheffer()
        {
            PushSimplyBinaryOperator( EOperator.Sheffer );
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
	        PushToken( BinaryOperator.CreateBinaryOperator( type, operands[0], operands[1] ) );
        }

        void PushVariable( string name )
        {
            PushToken( new Variable( name ) );
        }

        void PopVariablePushFunction()
        {
            var variable = PopToken<Variable>();
			var simplyOperator = ExpressionTranscription.GetEOperatorFromName( variable.Name );
	        if ( !simplyOperator.HasValue )
	        {
				PushToken( new Function { Name = variable.Name } );
	        }
	        else
	        {
				PushOperator( simplyOperator.Value, null );
	        }
        }

	    void PushArgumentToFunction()
	    {
			var arg = PopToken<Operand>();
		    var func = PopToken<Function>();
			func.Operands.Add( arg );
			PushToken( func );
	    }

	    void PushUserInput()
	    {
		    var commands = new List<Command>();
		    while ( _stack.Count > 0 )
		    {
			    commands.Insert( 0, PopToken<Command>() );
		    }

			PushToken( new UserInput { Commands = commands } );
		}

	    void PushDefineVariableCommand()
	    {
		    PushToken( new DefineVariableCommand
		    {
				Expression = PopToken<Expression>(),
			    Variable = PopToken<Variable>()
		    } );
	    }

	    void PushDeleteVariableCommand()
	    {
		    PushToken( new DeleteVariableCommand { Variable = PopToken<Variable>() } );
	    }

	    void PushExpressionCode()
	    {
		    PushToken( new Expression { Root = PopToken<Operand>() } );
	    }

	    void PushListOfExpressionStart()
	    {
		    PushToken( new OperandList() );
	    }

	    void PushExpressionToList()
	    {
			var arg = PopToken<Operand>();
		    var list = PopToken<OperandList>();
			list.Add( arg );
			PushToken( list );
	    }

        partial void ProductionBegin( ENonTerminal production )
        {
			switch ( production )
	        {
		        case ENonTerminal.TableCloth: ClearStack(); break;
				case ENonTerminal.FunctionBracketsAndArguments: PopVariablePushFunction(); break;
				case ENonTerminal.ListOfExpression: PushListOfExpressionStart(); break;
	        }
        }

        partial void ProductionEnd( ENonTerminal production )
        {
			switch ( production )
            {
				case ENonTerminal.TableCloth: PushUserInput(); break;
				case ENonTerminal.DefineVariableCommand: PushDefineVariableCommand(); break;
				case ENonTerminal.DeleteVariableCommand: PushDeleteVariableCommand(); break;
                case ENonTerminal.ExpressionCode: PushExpressionCode(); break;
                case ENonTerminal.ConstantT: PushTrueConstant(); break;
                case ENonTerminal.ConstantF: PushFalseConstant(); break;
                case ENonTerminal.Identifier: PushVariable( CurrentToken ); break;
            }
        }
    }
}
