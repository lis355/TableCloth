using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;

/// <summary>
/// Указывает параметры начальной формы-приветствия
/// </summary>
class Start
{
    public const bool IsStartForm = false;
    public const int Delay = 2000;
}

namespace BinaryCalc
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
//             if (Start.IsStartForm)
//             {
//                 StartForm A = new StartForm();
//                 A.Show();
//                 Application.DoEvents();
//                 Thread.Sleep(Start.Delay);
//                 A.Dispose();
//             }
            Application.Run(new MainForm());
        }
    }
}
