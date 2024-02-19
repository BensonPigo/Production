using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.MercuryDownloadStickerQueue
{
    internal static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Sci.Env.AppInit();
            Sci.Env.User = new Sci.UserInfo() { UserName = "SCIMIS" };

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MercuryDownloadStickerQueue());
        }
    }
}
