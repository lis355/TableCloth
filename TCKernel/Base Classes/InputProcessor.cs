using System;
using System.Collections.Generic;

namespace TableClothKernel
{
	public class InputProcessor
	{
		public class ProcessorResult
	    {
			public class CommandResult
			{
				public string Output;

				/// <summary>
				/// Может быть null, это для кеширования
				/// </summary>
				public Expression Expression;
			}

			public bool Success;
		    public TcException Exception;

			public UserInput Input;
			public List<CommandResult> Output;

			public ProcessorResult()
			{
				Success = false;
				Output = new List<CommandResult>();
			}
	    }

		readonly Parser _parser;
		readonly Solution _solution;

		public InputProcessor( Solution solution )
		{
			_solution = solution;

            _parser = new Parser();
            _parser.Errors.Message += MessagesDispatcher;
		}

		public ProcessorResult Process( string input )
		{
			var result = new ProcessorResult();
 
			try
			{
				ProcessWithExceptions( input, result );
				result.Success = true;
			}
			catch ( TcException exception )
			{
				TcDebug.Log( String.Format( "Error in line {0} in column {1} : {2}",
					exception.TcData.Line,
					exception.TcData.Column,
					exception.TcData.Text ) );

				result.Exception = exception;
			}
			catch
			{
			}

			return result;
		}

		void ProcessWithExceptions( string input, ProcessorResult result )
		{
			result.Input = Parse( input );
			
			foreach ( var command in result.Input.Commands )
			{ 
				ValidateCommand( command );
				var commandResult = ProcessCommand( command );

			}
		}

		void ValidateCommand( Command command )
		{
			command.Validate();
		}

		ProcessorResult.CommandResult ProcessCommand( Command cmd )
		{
			return _solution.Commands.Process( cmd );
		}

        void MessagesDispatcher( ParserErrors.Data data )
        {
            if ( data.Type == ParserErrors.EType.Error )
            {
                throw new TcException( data );
            }
        }

        UserInput Parse( string s )
        {
			_parser.Parse( new Scanner( s ) );
			return _parser.ParseResult;
        }
	}
}
