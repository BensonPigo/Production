using Sci.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PMS_ProductionKitsConfirm
{
    static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        static void Main(string[] args = null)
        {
            string pStartForm = (args.Length == 0) ? "" : args[0].ToString();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Sci.Env.AppInit();

            DBProxy.Current.DefaultTimeout = 999999999;
            switch (pStartForm)
            {
                case "ProductionKitsConfirm": new Form1("ProductionKitsConfirm"); break;
                case "": Application.Run(new Form1()); break;
            }
            Sci.Env.AppShutdown();
        }

        public class DailyUser : Sci.IUserInfo
        {
            public DailyUser()
            {
            }
            public string AuthorityList { get { return ""; } }
            public string BrandList { get { return ""; } }
            public string Department { get { return ""; } }
            public string Director { get { return ""; } }
            public string Factory { get { return ""; } }
            public string FactoryList { get { return ""; } }
            public bool IsAdmin { get { return true; } }
            public bool IsMIS { get { return true; } }
            public bool IsTS { get { return false; } }
            public string Keyword { get { return ""; } }
            public string MailAddress { get { return ""; } }
            public string MemberList { get { return ""; } }
            public long PositionID { get { return 0; } }
            public string ProxyList { get { return ""; } }
            public string SpecialAuthorityList { get { return ""; } }
            public string UserID { get { return "Daily Schedule"; } }
            public string UserName { get { return "Daily Schedule"; } }
            public string UserPassword { get { return ""; } }

            public bool IsContainsAuthority(string id) { return false; }
            public bool IsContainsFactory(string id) { return false; }
            public bool IsContainsMember(string id) { return false; }
            public bool IsContainsProxy(string id) { return false; }
            public bool IsSameUser(string id) { return false; }
        }
    }
}
