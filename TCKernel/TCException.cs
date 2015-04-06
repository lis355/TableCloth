using System;

namespace TableClothKernel
{
    internal class TcException : Exception
    {
        public TcException( string m ) :
            base( m )
        {
        }
    }
}
