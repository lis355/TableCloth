using System;

namespace TableClothKernel
{
	public class Variable : Operand
	{
		public string Name { get; private set; }

		public Variable( string name )
		{
			if ( String.IsNullOrWhiteSpace( name ) )
				throw new ArgumentNullException();

			Name = name;
		}

		public override string ToExpressionString()
		{
			return ExpressionTranscription.GetVariableTranscription( this );
		}

		public override Operand Calc( Calculator calculator )
		{
			return calculator.CalcVariable( this );
		}

		public override bool Equals( object obj )
		{
			var other = obj as Variable;
			if ( other == null )
				return false;

			return Name == other.Name;
		}

		public bool Equals( Variable other )
		{
			return string.Equals( Name, other.Name );
		}

		public override int GetHashCode()
		{
			return ( Name != null ) ? Name.GetHashCode() : 0;
		}
	}
}
