using System;
using System.Linq;
using System.Windows.Forms;

namespace hwp2pdf
{
    static class Program
    {
        /// <summary>
        /// 해당 애플리케이션의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static int Main(string[] args)
        {
            if (CliRunner.ShouldRunCli(args))
            {
                return CliRunner.Run(args);
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain());
            return 0;
        }
    }
}
