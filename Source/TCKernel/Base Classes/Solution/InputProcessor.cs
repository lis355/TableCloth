using System;

namespace TableClothKernel
{
	public class InputProcessor
	{
		readonly Parser _parser;
		readonly Solution _solution;

		public InputProcessor( Solution solution )
		{
			_solution = solution;

            _parser = new Parser();
            _parser.Errors.Message += MessagesDispatcher;
		}

		public RequestResult Process( string input )
		{
			var result = new RequestResult();
 
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

		void ProcessWithExceptions( string input, RequestResult result )
		{
			result.Input = Parse( input );
			
			foreach ( var command in result.Input.Commands )
			{ 
				ValidateCommand( command );

				var commandResult = ProcessCommand( command );
                result.Output.Add( commandResult );
			}
		}

		void ValidateCommand( Command command )
		{
			command.Validate();
		}

		RequestResult.CommandResult ProcessCommand( Command cmd )
		{
			return _solution.Commands.Process( cmd );
		}

        void MessagesDispatcher( ParserErrors.Data data )
        {
            if ( data.Type == ParserErrors.EType.SyntaxError )
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
