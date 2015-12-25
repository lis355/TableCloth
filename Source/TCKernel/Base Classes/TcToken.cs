using System;
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
}
