using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TableClothKernel
{
    abstract class Node
    {
        public string Type() { return GetType().Name.ToString(); }
        public string Label = "";
        public List<Node> Childs = new List<Node>();

        public string PrintDot( ref int GlobalNum )
        {
            int myNum = GlobalNum, cNum;
            string res = "\tv" + ( GlobalNum++ ).ToString() + " [ label = \"" + Type() + ( ( Label != "" ) ? " : " + Label : "" ) + "\" ];\n";
            foreach ( Node c in Childs )
            {
                cNum = GlobalNum;
                res += "\t" + c.PrintDot( ref GlobalNum ) + "v" + myNum.ToString() + " -- v" + cNum.ToString() + ";\n";
            }
            return res;
        }
    }

    class RootNode : Node
    {
        public void ToDot()
        {
            string res = "graph\n{\n";
            int level = 0;
            res += base.PrintDot( ref level );
            res += "}\n";
            System.IO.File.WriteAllText( "t.dot", res );
            System.Diagnostics.Process.Start( "dot.exe", "t.dot -Tpng -ot.png" );
        }
    }
    class ExpressionCommandNode : Node { }
    class CreateNewVariableCommandNode : Node { }
    class DeleteVariableCommandNode : Node { }
    class GetExpressionTypeCommandNode : Node { }

    class ExpressionNode : Node { public ExpressionNode( string e ) { Label = e; } }

    class PirseNode : Node { }
    class ShefferNode : Node { }
    class EquivalenceNode : Node { }
    class ImplicationNode : Node { }
    class XorNode : Node { }
    class OrNode : Node { }
    class AndNode : Node { }
    class NotNode : Node { }

    class ConstantNode : Node { }
    class TrueNode : Node { }
    class FalseNode : Node { }
}
