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
    }

    public class TcWarning : TcMessage
    {
        public TcWarning( ParserErrors.Data tcData ):
            base( tcData )
        {
        }
    }
}
