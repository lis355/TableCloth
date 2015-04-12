namespace TableClothKernel
{
	public class CommandProcessor
	{
		readonly Solution _solution;

		public CommandProcessor( Solution solution )
		{
			_solution = solution;
		}

		public InputProcessor.ProcessorResult.CommandResult Process( Command command )
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

		InputProcessor.ProcessorResult.CommandResult ProcessExpression( Expression expression )
		{
			var result = new InputProcessor.ProcessorResult.CommandResult();

			expression.Simplify();

			// TODO temp
			expression.Calc();
			TcDebug.Log( expression.ToExpressionString() );

			return result;
		}

		InputProcessor.ProcessorResult.CommandResult ProcessDefineVariableCommand( DefineVariableCommand command )
		{
			var result = new InputProcessor.ProcessorResult.CommandResult();

			command.Expression.Simplify();

			_solution.Variables.Set( command.Variable.Name, command.Expression );

			// TODO temp
			TcDebug.Log( command.Expression.ToExpressionString() );

			return result;
		}

		InputProcessor.ProcessorResult.CommandResult ProcessDeleteVariableCommand( DeleteVariableCommand command )
		{
			var result = new InputProcessor.ProcessorResult.CommandResult();

			_solution.Variables.Delete( command.Variable.Name );

			return result;
		}
	}
}
