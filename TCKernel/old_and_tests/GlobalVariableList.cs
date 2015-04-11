namespace TableClothKernel
{
    static class GlobalVariableList
    {
        public static VariableList V = new VariableList();

        public static bool GetCalcValue( string name )
        {
            //if ( !IsExist( name ) )
            //    throw new TcException( "no defined variable " + name );

            return V[name].Calc();
        }

        public static bool IsExist( string name )
        {
            return V.IsExist( name );
        }

        public static void New( string name, Expression e )
        {
            V.Set( name, e );
        }

        public static void Clear( string name )
        {
            V.Delete( name );
        }
    }
}
