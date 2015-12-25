using System;

using TableClothKernel;

namespace TCKernelInterface
{
    internal class KernelInterface
    {
        private Solution _solution;

        public KernelInterface()
        {
            Init();
        }

        private void Init()
        {
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.White;
            Console.Clear();

            TcDebug.LogDelegate = Console.WriteLine;
            TcDebug.Log(Information.KernelName + " " + Information.KernelVersion + " " + Information.KernelAssembly);

            Options.PrettyPrint = true;
            //TcDebug.PrintLog = true;

            _solution = new Solution();
        }

        public void Run()
        {
            while (true)
            {
                Console.Write(">");

                var readLine = Console.ReadLine();
                if (readLine == null)
                    continue;

                var inputString = readLine.Trim();
                if (String.IsNullOrEmpty(inputString))
                    continue;

                if (inputString.IndexOf('$') >= 0)
                {
                    switch (inputString.ToLower())
                    {
                        case "$exit":
                            Environment.Exit(0);
                            break;
                        case "$clear":
                            Console.Clear();
                            break;
                        default:
                            Console.WriteLine("Unknown system command");
                            break;
                    }
                }
                else
                {
                    // введено математическое выражение, здесь работаем с ядром
                    var result = _solution.Input.Process(inputString);
                    foreach (var commandResult in result.Output)
                    {
                        Console.WriteLine(commandResult.Output);
                    }
                }
            }
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
