using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Sci.Data;

namespace Sci.Production
{
    static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            
            Sci.Env.AppInit();
            DBProxy.Current.DefaultTimeout = 300;  //加長時間為5分鐘，避免timeout
            Application.Run(new Main());
            Sci.Env.AppShutdown();
        }
    }
}
