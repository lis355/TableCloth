using System;

namespace TableClothKernel
{
    public class TcMessage : Exception
    {
        public ParserErrors.Data TcData { get; private set; }

        public TcMessage( ParserErrors.Data tcData ):
            base()
        {
            TcData = tcData;
        }
    }

    public class TcException : TcMessage
    {
        public TcException( ParserErrors.Data tcData ):
            base( tcData )
        {
        }

		public TcException( string message ):
            base( new ParserErrors.Data { Text = message, Type =  ParserErrors.EType.Error } )
        {
        }
    }

    public class TcWarning : TcMessage
    {
        public TcWarning( ParserErrors.Data tcData ):
            base( tcData )
        {
        }
    }
}
