using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public bool Calc() { return Root.CalcExpressionOnThisVertex(); }

        public int VariableCount { get { return VariableNames.Count; } }

        public void AddVariable( string Name )
        {
            if ( !VariableNames.Contains( Name ) )
                VariableNames.Add( Name );
        }
    }
}
