using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TableClothKernel
{
/*
 * Все классы, содержащие методы должны быть помечены атрибутом
 * CalcMethods
 * 
 * Все методы должны быть публичными, статическими и возвращать Operand
 * Метод может либо не иметь аргументов, либо иметь один аргумент, реализующий IEnumerable<Operand>
 * либо список аргументов типа Operand
*/
	public class CalcMethodsAttribute : Attribute { }

	public static class CalcProvider
	{
		class MethodLink
		{
			public enum MethodParameters
			{
				None,
				ListOfOperands,
				EnumerableOfOperands
			}

			public MethodInfo Method { get; private set; }
			public MethodParameters Parameters { get; private set; }
			public int ParametersLength { get; private set; }

			public MethodLink( MethodInfo method, MethodParameters param )
			{
				Method = method;
				Parameters = param;
				ParametersLength = Method.GetParameters().Length;
			}

			public Operand Calc( params Operand[] operands )
			{
				if ( operands == null
					|| operands.Count() != ParametersLength )
					throw new TcException( "Invalid arguments." );

				object[] parameters = null;

				switch ( Parameters )
				{
					case MethodParameters.None:
						break;

					case MethodParameters.ListOfOperands:
						parameters = operands;
						break;

					case MethodParameters.EnumerableOfOperands:
						parameters = new [] { operands };
						break;
				}
				
				return Method.Invoke( null, parameters ) as Operand;
			}
		}

		static readonly Dictionary<string, MethodLink> _methods = new Dictionary<string, MethodLink>();

		static CalcProvider()
		{
			GetMethods( Assembly.GetExecutingAssembly() );
		}

		static void GetMethods( Assembly assembly)
		{
			var types = assembly.GetTypes().Where( 
				x => x.IsClass
					&& x.GetCustomAttributes( typeof( CalcMethodsAttribute ), true ).Any() );

			var param = MethodLink.MethodParameters.None;
			
			foreach ( var method in types.SelectMany( x => x.GetMethods() ) )
			{

				if ( !method.IsPublic
					|| !method.IsStatic
					|| method.ReturnType != typeof( Operand ) 
					|| !IsGoodParameters( method.GetParameters(), ref param ) )
					continue;

				_methods.Add( method.Name, new MethodLink( method, param ) );
			}

			Calc( "T1" );
			Calc( "T2", Constant.True );
			Calc( "T3", Constant.True, Constant.True );
			Calc( "T4", Constant.True );
			Calc( "T5", Constant.True );
		}

		static bool IsGoodParameters( ParameterInfo[] parameters, ref MethodLink.MethodParameters param )
		{
			switch ( parameters.Count() )
			{
				case 0:

					param = MethodLink.MethodParameters.None;

					break;

				case 1:
					
					var parameterType = parameters.First().ParameterType;

					if ( parameterType == typeof( Operand ) )
					{
						param = MethodLink.MethodParameters.ListOfOperands;
					}
					else if ( parameterType.IsAssignableFrom( typeof( IEnumerable<Operand> ) ) )
					{
						param = MethodLink.MethodParameters.EnumerableOfOperands;
					}
					else if ( parameterType.IsAssignableFrom( typeof( Operand[] ) ) )
					{
						param = MethodLink.MethodParameters.EnumerableOfOperands;
					}
					else
					{
						return false;
					}

					break;

				default:

					foreach ( var parameter in parameters )
					{
						if ( parameter.ParameterType != typeof( Operand ) )
						{
							return false;
						}
					}

					param = MethodLink.MethodParameters.ListOfOperands;

					break;
			}

			return true;
		}

		public static Operand Calc( string functionName, params Operand[] operands )
		{
			var method = GetMethod( functionName );
			return method.Calc( operands );
		}

		static MethodLink GetMethod( string functionName )
		{
			MethodLink method;
			if ( !_methods.TryGetValue( functionName, out method ) )
				throw new TcException( "Can't find method " + functionName );

			return method;
		}
	}
}
