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
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain());
        }
    }
}
