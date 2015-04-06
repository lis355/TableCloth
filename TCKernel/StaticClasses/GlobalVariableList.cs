using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TableClothKernel
{
    static class GlobalVariableList
    {
        public static VariableList V = new VariableList();

        public static bool GetCalcValue( string Name )
        {
            if ( !IsExist( Name ) )
                throw new TCException( "no defined variable " + Name );
            return V[ Name ].Calc();
        }

        public static bool IsExist( string Name ) { return V.IsExist( Name ); }
        public static void New( string Name, Expression E ) { V.Set( Name, E ); }
        public static void Clear( string Name ) { V.Delete( Name ); }
    }
}
