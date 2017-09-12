using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Ict;

namespace RFIDmiddleware
{
    class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Sci.Env.AppInit();
            DBProxy.Current.DefaultTimeout = 300;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new RFIDmiddleware());
        }
    }
}
