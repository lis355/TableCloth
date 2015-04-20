using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TableClothKernel
{
	public class CalcMethodsAttribute : Attribute { }

	public static class CalcProvider
	{
		static readonly Dictionary<string, MethodInfo> _methods = new Dictionary<string, MethodInfo>();

		static CalcProvider()
		{
			GetMethods( Assembly.GetExecutingAssembly() );
		}

		static void GetMethods( Assembly assembly)
		{
			var types = assembly.GetTypes().Where( 
				x => x.IsClass
					&& x.GetCustomAttributes( typeof( CalcMethodsAttribute ), true ).Any() );

			var methods = types.SelectMany( 
				x => x.GetMethods().Where(
					m => m.ReturnType == typeof( Operand ) ) );

			foreach ( var method in methods )
			{
				_methods.Add( method.Name, method );
			}
		}

		public static Operand Calc( string functionName, params Operand[] operands )
		{
			var method = GetMethod( functionName );

			var parameters = new [] { operands[0] };

			return method.Invoke( null, parameters ) as Operand;
		}

		static MethodInfo GetMethod( string functionName )
		{
			MethodInfo method;
			if ( !_methods.TryGetValue( functionName, out method ) )
				throw new TcException( "Can't find method " + functionName );

			return method;
		}
	}
}
