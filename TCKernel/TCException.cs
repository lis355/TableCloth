using System;

namespace TableClothKernel
{
    class TCException : Exception
    {
        public TCException( string M ) : base( M ) { }
    }
}
