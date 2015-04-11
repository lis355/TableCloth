using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace TableClothKernel
{
	[DebuggerDisplay("{ToExpressionString()}")]
	public abstract class TcToken
	{
		public override string ToString() { return GetType().Name; }
		public virtual string ToDebugString() { return String.Empty; }
		public virtual string ToExpressionString() { return String.Empty; }
	}
	
	public abstract class Operand : TcToken { }
	
	public class Variable : Operand
	{
	    public string Name { set; get; }
	
	    public override string ToDebugString() { return ToExpressionString(); }
		public override string ToExpressionString() { return Name; }
	}
	
	public class ConstantTcToken : Operand
	{
	    public EBooleanConstants Value { set; get; }
	
	    public override string ToDebugString() { return ToExpressionString(); }
		public override string ToExpressionString() { return Value.ToString(); }
	}
	
	public class Function : Operand
	{
	    public string Name { set; get; }
	    public List<Operand> Operands { set; get; }
	
	    public Function():
	        base()
	    {
	        Operands = new List<Operand>();
	    }
	
	    public override string ToDebugString() 
	    {
	        return Name;
	    }

		public override string ToExpressionString() 
	    {
	        return ToDebugString() + "(" 
				+ String.Join( ",", Operands.Select( x => x.ToExpressionString() ) ) + ")";
	    }
	}
	
	public class Operator : Function
	{
	    public EOperator Type { get; set; }
	
	    public override string ToDebugString() 
	    {
	        return Type.ToString();
	    }
	}
}
