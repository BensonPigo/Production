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
        static void Main(string[] args = null)
        {
            //Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            Sci.Env.AppInit();
            DBProxy.Current.DefaultTimeout = 300;  //加長時間為5分鐘，避免timeout

            //args = new string[] { "userid:'MIS'", "formName:'Sci.Trade.Basic.B03,Sci.Trade.Basic'", "menuName:'B03. Country'", "args:''" };
            if (args != null && args.Length != 0) {
                DirectOpenForm(args);
            }else{
                
                Application.Run(new Main());    
            }

            Sci.Env.AppShutdown();
        }

        public static void DirectOpenForm(string[] args){

                //args = new string[] { "userid:'MIS'", "formName:'Sci.Sample.Basic.B10,Sci.Sample.Basic'", "menuName:''", "args:''" };
                //  "userid:'{0}' formName:'{1}' menuName:'{2}' args:'{3}'"

            string userid = "";
            string factoryID = "";
            string formName = "";
            string menuName = "";
            string arguments = "";

            foreach (string arg in args) {
                var strArg = arg.Split(new char[] { ':' }, 2);
                if ( strArg[0].EqualString("userid")){
                    userid = strArg[1].Length <= 2 && !strArg[1].Contains("'")
                        ? strArg[1]
                        : strArg[1].Replace("'", "");

                }
                else if (strArg[0].EqualString("factoryID"))
                {
                    factoryID = strArg[1].Length <= 2 && !strArg[1].Contains("'")
                        ? strArg[1]
                        : strArg[1].Replace("'", "");
                }
                else if (strArg[0].EqualString("formName"))
                {
                    formName = strArg[1].Length <= 2 && !strArg[1].Contains("'")
                        ? strArg[1]
                        : strArg[1].Replace("'", "");
                }
                else if (strArg[0].EqualString("menuName"))
                {
                    menuName = strArg[1].Length <= 2 && !strArg[1].Contains("'")
                        ? strArg[1]
                        : strArg[1].Replace("'", "");
                }
                else if (strArg[0].EqualString("args"))
                {
                    arguments = strArg[1].Length <= 2 && !strArg[1].Contains("'")
                        ? strArg[1]
                        : strArg[1].Replace("'","");
                }
            }


            List<System.Data.SqlClient.SqlParameter> sqlPars = new List<System.Data.SqlClient.SqlParameter>();
            sqlPars.Add(new System.Data.SqlClient.SqlParameter("@UserID",userid));
            System.Data.DataTable data;
            var result1 = Sci.Data.DBProxy.Current.Select("","select id,password from dbo.Pass1 where id = @UserID",sqlPars,out data);
            UserInfo userInfo = new UserInfo();
            var result = Sci.Production.Win.Login.UserLogin(userid, data.Rows[0]["Password"].ToString(),factoryID, userInfo);
            Env.User = userInfo;
            var menuItem = new ToolStripMenuItem(menuName);
            Type typeofControl = Type.GetType(formName);
            var arrArg = string.IsNullOrWhiteSpace(arguments) ? new Object[1] { menuItem } : new Object[2] { menuItem, arguments };
            Form formClass = (Form)Activator.CreateInstance(typeofControl, arrArg);
            
            Application.Run(formClass); //load Trade 
            Sci.Env.AppShutdown();
            return;
            
        }
    }
}
