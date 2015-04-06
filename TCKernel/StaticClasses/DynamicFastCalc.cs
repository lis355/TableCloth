using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Reflection.Emit;

namespace TableClothKernel
{
    class DynamicFastCalc
    {
        static readonly DynamicFastCalc _instance = new DynamicFastCalc();
        public static DynamicFastCalc Instance { get { return _instance; } }

        readonly ModuleBuilder _moduleBuilder;

        DynamicFastCalc()
        {
            var assemblyBuilder = Thread.GetDomain().DefineDynamicAssembly(
                new AssemblyName( "FastCalc" ),
                AssemblyBuilderAccess.RunAndSave );

            _moduleBuilder = assemblyBuilder.DefineDynamicModule( "FastCalcModule" );
        }

        public interface IFastCalcMethod
        {
            bool Calc( params object[] args );
        }

        class FastCalcMethod : IFastCalcMethod
        {
            public object Owner;
            public MethodInfo Info;

            public bool Calc( params object[] args )
            {
                return ( bool )Info.Invoke(
                    Owner,
                    args );
            }
        }

        public IFastCalcMethod CreateCalcMethod( string name, int argCount )
        {
            var typeBuilder = _moduleBuilder.DefineType(
                "FastCalcFunctions_" + name,
                TypeAttributes.Public );

            var argType = typeof( bool );

            var parametersArray = Enumerable.Repeat( argType, argCount ).ToArray();

            MethodBuilder methodSum = typeBuilder.DefineMethod(
                name,
                MethodAttributes.Public,
                argType,
                parametersArray );

            EmitInstructions( methodSum.GetILGenerator() );

            var thistype = typeBuilder.CreateType();

            MethodInfo info = thistype.GetMethod(
                name,
                BindingFlags.Instance
                | BindingFlags.Public );

            FastCalcMethod result = new FastCalcMethod
            {
                Owner = Activator.CreateInstance( thistype ),
                Info = info
            };

            return result;
        }

        void EmitInstructions( ILGenerator generator )
        {
            generator.Emit( OpCodes.Ldarg_1 );
            generator.Emit( OpCodes.Ldarg_2 );
            generator.Emit( OpCodes.Xor );
            generator.Emit( OpCodes.Ret );
        }
    }
}
