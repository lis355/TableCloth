﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace TableClothKernel
{
    public class TcDebug
    {
        static void Main()
        {
            Log( Information.KernelName + " " + Information.KernelVersion + " " + Information.KernelAssembly );

            var tests = new Tests();
            tests.Run();

            Console.ReadLine();
        }

        static readonly List<string> _logStrings = new List<string>();

        public static void Log( string s )
        {
            s = String.Format( "[{0}] : {1}", DateTime.Now.ToUniversalTime(), s );
            _logStrings.Add( s );
            Console.WriteLine( s );
        }

        public static void SaveLog()
        {
            File.WriteAllLines( "log.txt", _logStrings, Encoding.UTF8 );
        }
    }

}
