using System.Collections.Generic;
using System.Diagnostics;

namespace TableClothKernel
{
	[DebuggerDisplay("{ToDebugString()}")]
	public abstract class TcToken
	{
		public abstract string ToDebugString();
		public abstract string ToExpressionString();
	}

	public abstract class Operand : TcToken
	{
		public override string ToDebugString()
		{
			return DebugExpressionTranscription.GetOperandCurrentTranscription( this );
		}

		public override string ToExpressionString()
		{
			return ExpressionTranscription.GetOperandCurrentTranscription( this );
		}
	}
	
	public class Variable : Operand
	{
	    public string Name { set; get; }
	}
	
	public class Constant : Operand
	{
	    public EBooleanConstants Value { set; get; }
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
	}
	
	public class Operator : Function
	{
	    public EOperator Type { get; set; }
	}
}
