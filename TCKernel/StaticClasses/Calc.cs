using System;

namespace TableClothKernel
{
    public static class Calc
    {
        static readonly Parser _parser;

        static Calc()
        {
            _parser = new Parser();
            _parser.Errors.Message += MessagesDispatcher;
        }

        static void MessagesDispatcher( ParserErrors.Data data )
        {
            if ( data.Type == ParserErrors.EType.Error )
            {
                throw new TcException( data );
            }
        }

	    public class CalcResult
	    {
			public bool Success;
			public TcToken Result;
		    public TcException Exception;
	    }

        // Основная функция рассчета строки
        public static CalcResult CalcExpression( string s )
        {
			var result = new CalcResult();

            try
            {
                _parser.Parse( new Scanner( s ) );
                result.Success = true;
				result.Result = _parser.ParseResult;
            }
            catch ( TcException exception )
            {
                TcDebug.Log( String.Format( "Error in {0} in {1} : {2}",
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
    }
}
