using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TableClothKernel
{
	/// <summary>
	/// Базовый токен из парсера
	/// </summary>
	[DebuggerDisplay("{ToDebugString()}")]
	public abstract class TcToken
	{
		public virtual string ToDebugString() { return String.Empty; }
		public virtual string ToExpressionString() { return String.Empty; }
	}

	#region Operands
	
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

	#endregion

	/// <summary>
	/// Весь ввод юзера в одной строке
	/// </summary>
	public class UserInput : TcToken
	{
	    public List<Command> Commands;
	}

	public abstract class Command : TcToken
	{
		public override string ToDebugString() { return GetType().Name; }
	}

	public class DefineVariableCommand : Command
	{
		public Variable Variable { get; set; }
		public Expression Expression { get; set; }
	}
	
	public class DeleteVariableCommand : Command
	{
		public Variable Variable { get; set; }
	}

	public class Expression : Command
	{
		public Operand Root { get; set; }
	}
}
