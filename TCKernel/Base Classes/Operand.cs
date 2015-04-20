namespace TableClothKernel
{
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

		/// <summary>
		/// Валидирование операнда. При некорректности бросается исключение
		/// </summary>
		public virtual void Validate() { }

		/// <summary>
		/// Упростить операнды ( функции не рассчитываются )
		/// </summary>
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
}
