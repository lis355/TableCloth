using System;

namespace TableClothKernel
{
	public class InputProcessor
	{
		public class ProcessorResult
	    {
			public bool Success;
		    public TcException Exception;

			public UserInput Input;
			public string Output;

			public ProcessorResult()
			{
				Success = false;
				Output = String.Empty;
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
			ValidateInput( result.Input );
			ProcessInput( result );
		}

		void ValidateInput( UserInput result )
		{
			foreach ( var command in result.Commands )
			{
				ValidateCommand( command );
			}
		}

		void ValidateCommand( Command command )
		{
			command.Validate();
		}

		void ProcessInput( ProcessorResult result )
		{
			foreach ( var command in result.Input.Commands )
			{
				ProcessCommand( command, result );
			}
		}

		void ProcessCommand( Command cmd, ProcessorResult result )
		{
			_solution.Commands.Process( cmd );
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
