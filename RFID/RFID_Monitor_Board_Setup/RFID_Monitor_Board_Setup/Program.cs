using Sci.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RFID_Monitor_Board_Setup
{
    static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        static void Main(string[] args = null)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Sci.Env.AppInit();
            Sci.Env.User = new MonitorUser();
            DBProxy.Current.DefaultTimeout = 300;  //加長時間為5分鐘，避免timeout
            Application.Run(new RFID_Monitor_Board_Setup());
            Sci.Env.AppShutdown();
        }


    }
}
