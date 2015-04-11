using System;

using TableClothKernel;

namespace TCKernelInterface
{
    class KernelInterface
    {
		Solution _solution;

	    void Init()
	    {
		    Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.White;
            Console.Clear();

			TcDebug.LogDelegate = Console.WriteLine;
			TcDebug.Log( Information.KernelName + " " + Information.KernelVersion + " " + Information.KernelAssembly );

			Options.PrettyPrint = true;
			//TcDebug.PrintLog = true;

			_solution = new Solution();
	    }

	    public void Run()
        {
			Init();
            
			string inputString;

            do
            {
                Console.WriteLine();

                inputString = Console.ReadLine();
				inputString = inputString.Trim();

                if ( inputString.IndexOf( '$' ) >= 0 )
                {
                    switch ( inputString.ToLower() )
                    {
                        case "$exit": Environment.Exit( 0 ); break;
                        case "$clear": Console.Clear(); break;
                        default: Console.WriteLine( "Unknown system command" ); break;
                    }
                }
                else
                {
					// введено математическое выражение, здесь работаем с ядром
                    var result = _solution.Input.Process( inputString );
                }
            }
			while ( !String.IsNullOrEmpty( inputString ) );
        }
    }

	static class Program
	{
		static void Main( string[] args )
		{
			var kernelInterface = new KernelInterface();
			kernelInterface.Run();
		}
	}
}
