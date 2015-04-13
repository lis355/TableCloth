namespace TableClothKernel
{
	public class CommandProcessor
	{
		readonly Solution _solution;

		public CommandProcessor( Solution solution )
		{
			_solution = solution;
		}

		public RequestResult.CommandResult Process( Command command )
		{
			if ( command is Expression )
			{
				return ProcessExpression( command as Expression );
			}
			else if ( command is DefineVariableCommand )
			{
				return ProcessDefineVariableCommand( command as DefineVariableCommand );
			}
			else if ( command is DeleteVariableCommand )
			{
				return ProcessDeleteVariableCommand( command as DeleteVariableCommand );
			}
			else
			{
				throw new TcException( "Unknown command" );
			}
		}

		RequestResult.CommandResult ProcessExpression( Expression expression )
		{
			var result = new RequestResult.CommandResult();

			expression.Simplify();

            result.Expression = expression.Calc();
			result.Output = expression.ToExpressionString();

			return result;
		}

		RequestResult.CommandResult ProcessDefineVariableCommand( DefineVariableCommand command )
		{
			var result = new RequestResult.CommandResult();

			command.Expression.Simplify();

			_solution.Variables.Set( command.Variable.Name, command.Expression );

            result.Expression = command.Expression;
            result.Output = command.Expression.ToExpressionString();

			return result;
		}

		RequestResult.CommandResult ProcessDeleteVariableCommand( DeleteVariableCommand command )
		{
			var result = new RequestResult.CommandResult();

			_solution.Variables.Delete( command.Variable.Name );

			return result;
		}
	}
}
