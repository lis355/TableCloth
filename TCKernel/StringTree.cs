using System.Collections.Generic;

namespace TableClothKernel
{
    public abstract class Node
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

    public class RootNode : Node
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

    public class ExpressionCommandNode : Node { }
    public class CreateNewVariableCommandNode : Node { }
    public class DeleteVariableCommandNode : Node { }
    public class GetExpressionTypeCommandNode : Node { }

    public class ExpressionNode : Node { public ExpressionNode( string e ) { Label = e; } }

    public class PirseNode : Node { }
    public class ShefferNode : Node { }
    public class EquivalenceNode : Node { }
    public class ImplicationNode : Node { }
    public class XorNode : Node { }
    public class OrNode : Node { }
    public class AndNode : Node { }
    public class NotNode : Node { }

    public class ConstantNode : Node { }
    public class TrueNode : Node { }
    public class FalseNode : Node { }
}
