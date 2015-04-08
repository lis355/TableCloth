using System;

namespace TableClothKernel
{
    /// <summary>
    /// Основной класс для рассчета строки
    /// </summary>
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
            //else if (data.Type == ParserErrors.EType.Warning )
            //{
            //    throw new TcWarning( data );
            //}
        }

        // Основная функция рассчета строки
        public static bool CalcExpression( string s )
        {
            try
            {
                _parser.Parse( new Scanner( s ) );
                return true;
            }
            catch ( TcException exception )
            {
                TcDebug.Log( String.Format( "Error in {0} in {1} : {2}",
                    exception.TcData.Line,
                    exception.TcData.Column,
                    exception.TcData.Text ) );
            }
            catch
            {
            }

            return false;
        }
    }
}
