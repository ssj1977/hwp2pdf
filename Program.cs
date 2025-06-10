using System;
using System.Collections.Generic;
using System.Linq;
//using System.Threading.Tasks; //For .NET 4 or above
using System.Windows.Forms;

namespace hwp2pdf
{
    static class Program
    {
        /// <summary>
        /// 해당 애플리케이션의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main(string[] args) // Modified to accept args
        {
            if (args.Length > 0)
            {
                // Call command-line handler (to be implemented)
                // For now, just a placeholder to ensure Program.cs compiles
                // after CommandLineRunner.cs is added in the next step.
                // We'll fully implement CommandLineRunner.Run later.
                // Console.WriteLine("Command-line mode activated with args: " + string.Join(" ", args));
                // This line will be replaced by CommandLineRunner.Run(args) once that class is more developed.
                // To avoid premature execution logic here, we'll make the subtask for CommandLineRunner
                // create the class and a basic Run method.
                // For now, let's assume CommandLineRunner.Run will exist.
                 CommandLineRunner.Run(args);
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FormMain());
            }
        }
    }
}
