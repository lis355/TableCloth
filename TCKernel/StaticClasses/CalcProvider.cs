using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TableClothKernel
{
	public class CalcMethodsAttribute : Attribute { }

	public static class CalcProvider
	{
		static readonly List<MethodInfo> _methods = new List<MethodInfo>();

		static CalcProvider()
		{
			GetMethods( Assembly.GetExecutingAssembly() );
		}

		static void GetMethods( Assembly assembly)
		{
			foreach( var type in assembly.GetTypes() )
			{
				if ( !type.IsClass
					|| !type.GetCustomAttributes( typeof( CalcMethodsAttribute ), true ).Any() )
					continue;

				foreach ( var method in type.GetMethods() )
				{
					if ( method.ReturnType == typeof( Operand ) )
					{
						_methods.Add( method );
					}
				}
			}
		}

		public static Operand Calc( string functionName, params Operand[] operands )
		{
			var method = _methods.FirstOrDefault( x => x.Name == functionName );
			if ( method == null )
				throw new TcException( "Can't find method" );

			return method.Invoke( null, new [] { operands[0] } ) as Operand;
		}
	}
}
