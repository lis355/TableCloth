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

		public virtual void Validate() { }
		public virtual Operand Simplify() { return this; }
	}
	
	public class Variable : Operand
	{
	    public string Name { set; get; }
	}
	
	public class Constant : Operand
	{
	    public EBooleanConstants Value { set; get; }

		public static Constant True = new Constant { Value = EBooleanConstants.True };
		public static Constant False = new Constant { Value = EBooleanConstants.False };

		public static implicit operator Constant( bool value )
	    {
			return ( value ) ? True : False;
	    }

		public static implicit operator bool( Constant value )
	    {
			return value == True;
	    }

		Constant() { }
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
		public abstract void Validate();
	}

	public class DefineVariableCommand : Command
	{
		public Variable Variable { get; set; }
		public Expression Expression { get; set; }

		public override void Validate()
		{
			Variable.Validate();
			Expression.Validate();
		}
	}
	
	public class DeleteVariableCommand : Command
	{
		public Variable Variable { get; set; }

		public override void Validate()
		{
			Variable.Validate();
		}
	}
}
