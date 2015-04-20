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
