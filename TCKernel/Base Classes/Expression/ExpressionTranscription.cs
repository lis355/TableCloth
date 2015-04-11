using System;
using System.Collections.Generic;
using System.Linq;

namespace TableClothKernel
{
	public static class ExpressionTranscription
    {
		/// <summary>
		/// Возвращает текущее написание оператора в зависимости от
		/// Options.ConstantOutType
		/// Options.CommandOutType
		/// </summary>
		public static string GetOperandCurrentTranscription( Operand operand )
		{
			if ( operand == null )
				throw new NullReferenceException();

			if ( operand is Constant )
			{
				return GetConstantTranscription( operand as Constant );
			}
			else if ( operand is Variable )
			{
				return GetVariableTranscription( operand as Variable );
			}
			else if ( operand is Operator )
			{
				return GetOperatorTranscription( operand as Operator );
			}
			else if ( operand is Function )
			{
				return GetFunctionTranscription( operand as Function );
			}

			throw new TcException( "Unknown operand" );
		}

		public static string GetConstantTranscription( Constant constant )
		{
			return _constantTranscription[constant.Value][Options.ConstantOutType];
		}

		public static string GetVariableTranscription( Variable variable )
		{
			return variable.Name;
		}

		public static string GetFunctionTranscription( Function function )
		{
			return GetFunctionName( function ) + GetFunctionArgumentsTranscription( function );
		}

		public static string GetFunctionName( Function function )
		{
			return function.Name;
		}

		static string GetFunctionArgumentsTranscription( Function function )
		{
			return "(" + String.Join( ",", function.Operands.Select( x => x.ToExpressionString() ) ) + ")";
		}

		public static string GetOperatorTranscription( Operator op )
		{
			if ( Options.OperatorOutType == EStringOperatorType.Function )
			{
				return GetSimplyOperatorName( op ) + GetFunctionArgumentsTranscription( op );
			}
			else
			{
				if ( SimplyOperatorIsMono( op ) )
				{
					return PrintDefaultMonoOperator( op );
				}
				else
				{
					return PrintDefaultBinaryOperator( op );
				}
			}
		}

		public static string GetSimplyOperatorName( Operator op )
		{
			return _operatorsTranscription[op.Type][Options.OperatorOutType];
		}

		static bool SimplyOperatorIsMono( Operator op )
		{
			return op.Type == EOperator.Not
				|| op.Type == EOperator.Sheffer
				|| op.Type == EOperator.Pirse;
		}

		static string PrintDefaultMonoOperator( Operator op )
		{
			return GetSimplyOperatorName( op ) + op.Operands[0].ToExpressionString();
		}

		static string PrintDefaultBinaryOperator( Operator op )
		{
			return "(" + op.Operands[0].ToExpressionString()
				+ GetSimplyOperatorName( op ) + op.Operands[1].ToExpressionString() + ")";
		}

		static readonly Dictionary<EBooleanConstants, Dictionary<EStringConstantType, string>> _constantTranscription;
		static readonly Dictionary<EOperator, Dictionary<EStringOperatorType, string>> _operatorsTranscription;

		static ExpressionTranscription()
		{
			_constantTranscription = new Dictionary<EBooleanConstants, Dictionary<EStringConstantType, string>>();

			AddConstantTranscription( EBooleanConstants.True,	"1",		"True"		);
			AddConstantTranscription( EBooleanConstants.False,	"0",		"False"		);

			_operatorsTranscription = new Dictionary<EOperator, Dictionary<EStringOperatorType, string>>();

			AddOperatorsTranscription( EOperator.Not,			"!",		"[not]"		);
			AddOperatorsTranscription( EOperator.And,			"&&",		"[and]"		);
			AddOperatorsTranscription( EOperator.Or,			"||",		"[or]"		);
			AddOperatorsTranscription( EOperator.Xor,			"^",		"[xor]"		);
			AddOperatorsTranscription( EOperator.Equivalence,	"==",		"[equ]"		);
			AddOperatorsTranscription( EOperator.Implication,	"=>",		"[impl]"	);
			AddOperatorsTranscription( EOperator.Sheffer,		"[shef]",	"[shef]"	);
			AddOperatorsTranscription( EOperator.Pirse,			"[pirse]",	"[pirse]"	);
		}

		static void AddConstantTranscription( EBooleanConstants type, string number, string world )
		{
			_constantTranscription.Add( type, new Dictionary<EStringConstantType, string>() );
			_constantTranscription[type][EStringConstantType.Number] = number;
			_constantTranscription[type][EStringConstantType.Word] = world;
		}

		static void AddOperatorsTranscription( EOperator type, string symbol, string world )
		{
			_operatorsTranscription.Add( type, new Dictionary<EStringOperatorType, string>() );
			_operatorsTranscription[type][EStringOperatorType.Symbol] = symbol;
			_operatorsTranscription[type][EStringOperatorType.Word] = world;
			_operatorsTranscription[type][EStringOperatorType.Function] = type.ToString();
		}
    }
}
