﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace TableClothKernel
{
	public static class ExpressionTranscription
    {
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
			return _kLeftRoundBracket 
				+ Space()
				+ String.Join( _kComma + Space(), function.Operands.Select( x => x.ToExpressionString() ) )
				+ Space()
				+ _kRightRoundBracket;
		}

		public static string GetOperatorTranscription( Operator op )
		{
			if ( Options.OperatorOutType == EStringOperatorType.Function )
			{
				return GetSimplyOperatorName( op, Options.OperatorOutType )
					+ GetFunctionArgumentsTranscription( op );
			}
			else
			{
				if ( SimplyOperatorIsNot( op ) )
				{
					return PrintNotOperator( op as NotOperator );
				}
				else
				{
					return PrintDefaultBinaryOperator( op as BinaryOperator );
				}
			}
		}

		public static string GetSimplyOperatorName( Operator op, EStringOperatorType type )
		{
			return _operatorsTranscription[op.Type][type];
		}

		public static bool SimplyOperatorIsNot( Operator op )
		{
			return op.Type == EOperator.Not;
		}

		static string PrintNotOperator( NotOperator op )
		{
			return GetSimplyOperatorName( op, Options.OperatorOutType )
				+ op.Operand.ToExpressionString();
		}

		static string PrintDefaultBinaryOperator( BinaryOperator op )
		{
			return _kLeftRoundBracket
				+ Space()
				+ op.FirstOperand.ToExpressionString()
				+ Space()
				+ GetSimplyOperatorName( op, Options.OperatorOutType )
				+ Space()
				+ op.SecondOperand.ToExpressionString()
				+ Space() 
				+ _kRightRoundBracket;
		}

		static public string GetOperandListTranscription( OperandList list )
		{
			return _kLeftCurlyBracket
				+ Space()
				+ String.Join( _kComma + Space(), list.Select( x => x.ToExpressionString() ) )
				+ Space()
				+ _kRightCurlyBracket;
		}

		static string Space()
		{
			return ( Options.PrettyPrint ) ? _kSpace : string.Empty;
		}

		public static bool IsValidName( string name )
		{
			return !String.IsNullOrWhiteSpace( name );
		}

		public static EOperator? GetEOperatorFromName( string name )
		{
			EOperator result;
			return ( Enum.TryParse( name, false, out result ) ) ?
				new EOperator?( result ) : null;
		}

		const string _kLeftRoundBracket = "(";
		const string _kRightRoundBracket = ")";
		const string _kLeftCurlyBracket = "{";
		const string _kRightCurlyBracket = "}";
		const string _kComma = ",";
		const string _kSpace = " ";

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
