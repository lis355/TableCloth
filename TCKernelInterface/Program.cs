using System;

namespace TCKernelInterface
{
    class KernelInterface
    {
        static string InputString = "", OutputString = "";

        static void PrintHeader()
        {
            Console.WriteLine();
            Console.WriteLine( TableClothKernel.Information.KernelName + " Interface" );
            Console.WriteLine( "Kernel Version {0}", TableClothKernel.Information.KernelVersion );
            Console.WriteLine( "Kernel Assembly {0}", TableClothKernel.Information.KernelAssembly );
        }

        static void Main( string[] args )
        {
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.White;
            Console.Clear();

            PrintHeader();

            while ( true )
            {
                Console.WriteLine();
                InputString = Console.ReadLine();
                if ( InputString.IndexOf( '$' ) >= 0 )
                {
                    InputString = InputString.Trim().ToLower();

                    switch ( InputString )
                    {
                        case "$exit": Environment.Exit( 0 ); break;
                        case "$clear": Console.Clear(); break;
                        case "$help": PrintHeader(); Console.Write( "System commands:\n\t$ exit -- exit from console\n\t$ clear -- clear console output\n\t$ help -- show list of system commands" );
                            break;
                        default: Console.WriteLine( "Unknown system command" ); break;
                    }
                }
                else// введено математическое выражение, здесь работаем с ядром
                {
                    OutputString = InputString;//TableClothKernel.Calc.CalcExpression(InputString);
                    Console.WriteLine();
                    Console.WriteLine( OutputString );
                }
            }
        }
    }
}
