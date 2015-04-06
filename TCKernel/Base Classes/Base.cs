using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TableClothKernel
{
    public abstract class Operand
    {
        public abstract Constant Calc();
    }

    public class Constant : Operand
    {
        public static Constant True = new Constant { value = true };
        public static Constant False = new Constant { value = false };

        bool value;

        public override Constant Calc()
        {
            return ( value ) ? True : False;
        }
    }

    /*public class Variable : Operand
    {
        public string Name { set; get; }

        public Variable( string name )
        {
            Name = name;
        }
    }*/

    public abstract class Operator : Operand
    {
    }

    public abstract class MonoOperator : Operator
    {
        public Operand Operand;
    }

    public abstract class BinaryOperator : Operator
    {
        public Operand L, R;
    }

    public abstract class CommonOperator : Operator
    {
        public List<Operand> Operands;
    }

    /*public class NotOperator : MonoOperator
    {
        public NotOperator( Operand operand )
        {
            Operand = operand;
        }

        //public override Constant Calc()
    }

    public class OrOperator : BinaryOperator
    {
        public OrOperator( Operand l, Operand r )
        {
            L = l;
            R = r;
        }
    }*/

    public class Calc2
    {
        public Calc2()
        {
        }
    }
}
