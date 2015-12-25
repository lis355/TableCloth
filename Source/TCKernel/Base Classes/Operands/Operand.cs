namespace TableClothKernel
{
	public abstract class Operand : TcToken
	{
		public override string ToDebugString()
		{
			return DebugExpressionTranscription.GetOperandCurrentTranscription( this );
		}

		/// <summary>
		/// Валидирование операнда. При некорректности бросается исключение
		/// </summary>
		public virtual void Validate() { }

		/// <summary>
		/// Упростить операнды ( функции не рассчитываются )
		/// </summary>
		public virtual Operand Simplify() { return this; }

		/// <summary>
		/// Рассчитать выражение. Информацию предоставляет calculator.
		/// </summary>
		public virtual Operand Calc( Calculator calculator ) { return this; }
	}
}
