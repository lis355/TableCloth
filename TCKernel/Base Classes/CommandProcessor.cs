namespace TableClothKernel
{
	public class CommandProcessor
	{
		readonly Solution _solution;

		public CommandProcessor( Solution solution )
		{
			_solution = solution;
		}

		public void Process( Command command )
		{
			if ( command is Expression )
			{
				ProcessExpression( command as Expression );
			}
			else if ( command is DefineVariableCommand )
			{
				ProcessDefineVariableCommand( command as DefineVariableCommand );
			}
			else if ( command is DeleteVariableCommand )
			{
				ProcessDeleteVariableCommand( command as DeleteVariableCommand );
			}
			else
			{
				throw new TcException( "Unknown command" );
			}
		}

		void ProcessExpression( Expression expression )
		{
			expression.Simplify();

			// TODO temp
			expression.Calc();
			TcDebug.Log( expression.ToExpressionString() );
		}

		void ProcessDefineVariableCommand( DefineVariableCommand command )
		{
			command.Expression.Simplify();

			_solution.Variables.Set( command.Variable.Name, command.Expression );

			// TODO temp
			TcDebug.Log( command.Expression.ToExpressionString() );
		}

		void ProcessDeleteVariableCommand( DeleteVariableCommand command )
		{
			_solution.Variables.Delete( command.Variable.Name );
		}
	}
}
