using System;

namespace TableClothKernel
{
	static class DebugExpressionTranscription
	{
		public static string GetOperandCurrentTranscription( Operand operand )
		{
			if ( operand == null )
				throw new NullReferenceException();

			if ( operand is Constant )
			{
				return ExpressionTranscription.GetConstantTranscription( operand as Constant );
			}
			else if ( operand is Variable )
			{
				return ExpressionTranscription.GetVariableTranscription( operand as Variable );
			}
			else if ( operand is Operator )
			{
				return ExpressionTranscription.GetSimplyOperatorName( operand as Operator, EStringOperatorType.Function );
			}
			else if ( operand is Function )
			{
				return ExpressionTranscription.GetFunctionName( operand as Function ) +
					"(" + ( operand as Function ).Operands.Count + ")";
			}

			throw new TcException( "Unknown operand" );
		}
	}
}
