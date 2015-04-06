using System.Collections.Generic;

namespace TableClothKernel
{
    class Expression
    {
        public ExpressionVertex Root;

        public List<string> VariableNames;

        public Expression()
        {
            VariableNames = new List<string>( 3 );
        }

        public bool Calc()
        {
            return Root.CalcExpressionOnThisVertex();
        }

        public int VariableCount { get { return VariableNames.Count; } }

        public void AddVariable( string name )
        {
            if ( !VariableNames.Contains( name ) )
                VariableNames.Add( name );
        }
    }
}
