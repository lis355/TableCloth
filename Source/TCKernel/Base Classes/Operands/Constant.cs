namespace TableClothKernel
{
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

		public override string ToExpressionString()
		{
			return ExpressionTranscription.GetConstantTranscription( this );
		}
	}
}
