﻿using System.Collections.Generic;

namespace TableClothKernel
{
    public abstract class Operand
    {
        public virtual Operand Calc()
        {
            return this;
        }
    }

    public class Constant : Operand
    {
        public static Constant True = new Constant();
        public static Constant False = new Constant();
    }

    public class Variable : Operand
    {
        public string Name { set; get; }

        public Variable( string name )
        {
            Name = name;
        }
    }

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
