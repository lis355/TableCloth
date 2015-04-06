using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Threading;

namespace TableClothGUI
{
    public class NoteLinkKernel
    {
        ProcessStartInfo KernelProcInfo;
        Process KernelProc;

        string lastCommand, lastResult;
        Stopwatch lastCommandTime;

        bool isRunning;

        public long CommandTime { get { return lastCommandTime.ElapsedMilliseconds; } }
        public string Result { get { return lastResult; } }

        public NoteLinkKernel( string path, string args )
        {
            if ( !File.Exists( path ) ) throw new System.Exception( "File " + path + " not found" );

            lastCommandTime = new Stopwatch();

            KernelProcInfo = new ProcessStartInfo();
            KernelProcInfo.FileName = path;
            KernelProcInfo.Arguments = args;
            KernelProcInfo.UseShellExecute = false;
            KernelProcInfo.CreateNoWindow = true;
            KernelProcInfo.RedirectStandardInput = true;
            KernelProcInfo.RedirectStandardOutput = true;
            KernelProcInfo.StandardOutputEncoding = System.Text.Encoding.GetEncoding( 866 );

            KernelProc = Process.Start( KernelProcInfo );
            KernelProc.OutputDataReceived += new DataReceivedEventHandler( KernelOutputHandler );
            KernelProc.BeginOutputReadLine();

            /**/
            Thread.Sleep( 1000 );
            KernelProc.StandardInput.WriteLine( "1+1;" );
        }

        public void KillKernel()
        {
            KernelProc.Kill();
        }

        public void ExecuteCommand( string command )
        {
            lastCommand = command;
            lastCommandTime.Reset();
            lastCommandTime.Start();
            lastResult = "";
            isRunning = true;
            KernelProc.StandardInput.WriteLine( lastCommand + ";555+444;" );
            while ( isRunning ) ;
            //KernelProc.StandardOutput.BaseStream.Flush();
            //lastResult = KernelProc.StandardOutput.ReadLine();
            lastCommandTime.Stop();
            //MessageBox.Show(lastCommandTime.ElapsedMilliseconds.ToString());
        }

        private void KernelOutputHandler( object sendingProcess, DataReceivedEventArgs outLine )
        {
            if ( !String.IsNullOrEmpty( outLine.Data ) )
            {
                // MessageBox.Show(outLine.Data);
                if ( outLine.Data == "999" )
                {
                    isRunning = false;
                }
                else
                {
                    lastResult += outLine.Data + "; ";
                }
                //  ParentNote.CurrentFuncInputSpace(lastResult);
            }
        }
    }
}
