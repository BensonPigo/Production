using Sci.Data;
using System;
using System.Windows.Forms;
using System.Configuration;

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
            // Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Sci.Env.AppInit();
            DBProxy.Current.DefaultTimeout = 300;  // 加長時間為5分鐘，避免timeout

            bool clearTaipeiServer = true;
            if (args != null && args.Length != 0)
            {
                foreach (string arg in args)
                {
                    if (arg.Contains("TaipeiServer:'true'"))
                    {
                        clearTaipeiServer = false;

                        // 若是傳送參數則排除,Tradedb_TSR防止使用者修改到資料
                        ConfigurationManager.AppSettings["PMSDBServer"] = ConfigurationManager.AppSettings["PMSDBServer"].ToString().Replace(",PMSDB_TSR", string.Empty);
                        ConfigurationManager.AppSettings["TestingServer"] = ConfigurationManager.AppSettings["TestingServer"].ToString().Replace(",testing_TSR", string.Empty);
                        ConfigurationManager.AppSettings["TaipeiServer"] = ConfigurationManager.AppSettings["PMSDBServer"];
                    }
                }
            }

            if (clearTaipeiServer && DBProxy.Current.DefaultModuleName != "bin" && DBProxy.Current.DefaultModuleName != "x86")
            {
                ConfigurationManager.AppSettings["TaipeiServer"] = string.Empty;
            }

            // Logs.UI.LogInfo(string.Format("args = [{0}]", "         -----------xxx"));
            // args = new string[] { "userid:'MIS'", "formName:'Sci.Trade.Basic.B03,Sci.Trade.Basic'", "menuName:'B03. Country'", "args:''" };
            // if (args != null && args.Length != 0)
            // {
            //    DirectOpenForm(args);
            // }
            // else
            // {
            Application.Run(new Main());

            // }
            Sci.Env.AppShutdown();
        }

        // public static void DirectOpenForm(string[] args)
        // {

        // //args = new string[] { "userid:C6001305", "factoryID:FA2", "formName:Sci.Sample.Basic.B10,Sci.Sample.Basic", "menuName:B09. Supplier (Taiwan)", "args:" };

        // string userid = "";
        //    string factoryID = "";
        //    string formName = "";
        //    string menuName = "";
        //    string arguments = "";

        // foreach (string arg in args)
        //    {
        //        var strArg = arg.Split(new char[] { ':' }, 2);
        //        if (strArg[0].EqualString("userid"))
        //        {
        //            userid = strArg[1];
        //        }
        //        else if (strArg[0].EqualString("factoryID"))
        //        {
        //            factoryID = strArg[1];
        //        }
        //        else if (strArg[0].EqualString("formName"))
        //        {
        //            formName = strArg[1];
        //        }
        //        else if (strArg[0].EqualString("menuName"))
        //        {
        //            menuName = strArg[1];
        //        }
        //        else if (strArg[0].EqualString("args"))
        //        {
        //            arguments = strArg[1];
        //        }
        //    }

        // List<System.Data.SqlClient.SqlParameter> sqlPars = new List<System.Data.SqlClient.SqlParameter>();
        //    sqlPars.Add(new System.Data.SqlClient.SqlParameter("@UserID", userid));
        //    System.Data.DataTable data;
        //    var result1 = Sci.Data.DBProxy.Current.Select("", "select id,password from dbo.Pass1 where id = @UserID", sqlPars, out data);
        //    UserInfo userInfo = new UserInfo();
        //    var result = Sci.Production.Win.Login.UserLogin(userid, data.Rows[0]["Password"].ToString(), factoryID, userInfo);
        //    Env.User = userInfo;
        //    var menuItem = new ToolStripMenuItem(menuName);
        //    Type typeofControl = Type.GetType(formName);
        //    var arrArg = string.IsNullOrWhiteSpace(arguments) ? new Object[1] { menuItem } : new Object[2] { menuItem, arguments };
        //    Form formClass = (Form)Activator.CreateInstance(typeofControl, arrArg);

        // Application.Run(formClass); //load Trade
        //    Sci.Env.AppShutdown();
        //    return;

        // }
    }

    // static class Program
    // {
    //    public static List<Process> innerProcess = new List<Process>();
    //    /// <summary>
    //    /// 應用程式的主要進入點。
    //    /// </summary>
    //    [STAThread]
    //    static void Main(string[] args = null)
    //    {
    //        Application.SetCompatibleTextRenderingDefault(false);
    //        Sci.Env.AppInit();
    //        Sci.Win.Apps.Base.SetFormName_OnFormLoaded = SetFormName;
    //        Sci.Data.DBProxy.Current.DefaultTimeout = 900;
    //        if (args.Length != 0)
    //        {
    //            try
    //            {
    //                RemoteStartup.Deserialized(args[0]).Start();
    //                return;
    //            }
    //            catch (Exception ex)
    //            {
    //                //嘗試用參數去解析要開啟的內容，失敗也不回應，資安考量，不能因為隨便誰給了參數，就跳訊息說自動登入失敗，簡直此地無銀三百兩
    //            }
    //        }

    // Application.Run(new Main()); //load Trade
    //        Sci.Env.AppShutdown();

    // }

    // public static void SetFormName(Form form) {

    // //if (!DBProxy.Current.DefaultModuleName.ToUpper().Contains("FORMAL"))
    //        //    form.Text = "(" + DBProxy.Current.DefaultModuleName + ") " + form.Text;
    //        if (!MyUtility.Check.Empty(Sci.Env.User))
    //            form.Text = "(" + Sci.Env.User.Factory + ") - (" + DBProxy.Current.DefaultModuleName + ")" + form.Text + " - (" + Sci.Env.User.UserID + " - " + Sci.Env.User.UserName + ")";
    //    }
    // }

    // public class RemoteStartup
    // {
    //    public string Username { get; set; }
    //    public string Password { get; set; }
    //    public ToolStripMenuItem MenuItem { get; set; }
    //    public Type TypeofControl { get; set; }
    //    public string StrArg { get; set; }
    //    public string FormName { get; set; }
    //    public string FormTextSufix { get; set; }
    //    public string Factory { get; set; }

    // [Serializable]
    //    private class SerializeBag
    //    {
    //        public string Username;
    //        public string Password;
    //        public string MenuItemText;
    //        public string ParentMenuItemText;
    //        public string TypeName;
    //        public string StrArg;
    //        public string FormName;
    //        public string FormTextSufix;
    //        public string Factory;
    //    }
    //    public string Serialized()
    //    {
    //        var se = new System.Runtime.Serialization.DataContractSerializer(typeof(SerializeBag));
    //        using (var ms = new System.IO.MemoryStream())
    //        {
    //            var para = new SerializeBag()
    //            {
    //                Username = this.Username,
    //                Password = this.Password,
    //                MenuItemText = this.MenuItem.Text,
    //                ParentMenuItemText = this.MenuItem.OwnerItem.Text,
    //                TypeName = this.TypeofControl.AssemblyQualifiedName,
    //                StrArg = this.StrArg,
    //                FormName = this.FormName,
    //                FormTextSufix = this.FormTextSufix,
    //                Factory = this.Factory,
    //            };
    //            se.WriteObject(ms, para);
    //            return Convert.ToBase64String(ms.ToArray());
    //        }
    //    }

    // public static RemoteStartup Deserialized(string serializedText)
    //    {
    //        RemoteStartup ins;
    //        var se = new System.Runtime.Serialization.DataContractSerializer(typeof(SerializeBag));
    //        using (var ms = new System.IO.MemoryStream())
    //        {
    //            var bs = Convert.FromBase64String(serializedText);
    //            ms.Write(bs, 0, bs.Length);
    //            ms.Position = 0;
    //            var data = (SerializeBag)se.ReadObject(ms);
    //            var menu1 = new ToolStripMenuItem(data.ParentMenuItemText);
    //            var menu2 = new ToolStripMenuItem(data.MenuItemText);
    //            menu1.DropDownItems.Add(menu2);
    //            ins = new RemoteStartup()
    //            {
    //                Username = data.Username,
    //                Password = data.Password,
    //                MenuItem = menu2,
    //                TypeofControl = System.Type.GetType(data.TypeName),
    //                StrArg = data.StrArg,
    //                FormName = data.FormName,
    //                FormTextSufix = data.FormTextSufix,
    //                Factory = data.Factory,
    //            };
    //            return ins;
    //        }
    //    }

    // public object Start()
    //    {
    //        Object[] arrArg = null;

    // //這時候Env.User是Null表示是從Program進來的
    //        if (Env.User == null)
    //        {
    //            var mainForm = new Sci.Production.Main();
    //            var u = new UserInfo();
    //            Sci.Production.Win.Login.UserLogin(this.Username, this.Password, this.Factory, u);
    //            mainForm.DoLogin(u);
    //            arrArg = string.IsNullOrWhiteSpace(StrArg) ?
    //                new Object[1] { this.MenuItem } :
    //                new Object[2] { this.MenuItem, StrArg };

    // Form frm;
    //            if (TypeofControl.GetConstructor(new Type[] { typeof(ToolStripMenuItem), typeof(string) }) != null)
    //                frm = ((Sci.Win.Tems.Base)Activator.CreateInstance(TypeofControl, arrArg));
    //            else
    //                frm = ((Sci.Win.Tems.Base)Activator.CreateInstance(TypeofControl, this.MenuItem));
    //            frm.Text += " " + this.FormTextSufix;
    //            return frm.ShowDialog();
    //        }
    //        else
    //        {
    //            //到這邊來的話表示是Menu項目被點選這時候有可能是VS執行，有可能是EXE執行，如果是EXE執行，就透過Process去呼叫獨立執行單元啟動
    //            //var IndependantProcess = this.StrArg.Split(",".ToArray(), StringSplitOptions.RemoveEmptyEntries).Any(item => item == "Inde");
    //            var IndependantProcess = true; //Roger說，使用者會因為之前的程式掛掉或卡住，那如果一律全部出去，就可以讓使用者有機會正常的關閉所有開啟的視窗
    //            if (Debugger.IsAttached == false && IndependantProcess)
    //            {
    //                new System.Threading.Thread(new System.Threading.ParameterizedThreadStart((arg) =>
    //                {
    //                    var objArray = arg as object[];
    //                    var menuItemInput = objArray[0] as ToolStripMenuItem;
    //                    var processParameter = objArray[1] as string;
    //                    object parent = this.MenuItem.OwnerItem;
    //                    Form frm;
    //                    while (((ToolStripMenuItem)parent).OwnerItem != null)
    //                    {
    //                        parent = ((ToolStripMenuItem)parent).OwnerItem;
    //                    }
    //                    frm = ((ToolStripMenuItem)parent).Owner.FindForm();
    //                    frm.Invoke((Action<ToolStripMenuItem>)((m) => m.Enabled = false), menuItemInput);

    // Process p = null;
    //                    do
    //                    {
    //                        try
    //                        {
    //                            p = Process.Start(Application.ExecutablePath, processParameter);
    //                        }
    //                        catch (Exception ex)
    //                        {
    //                            MyUtility.Msg.ErrorBox(ex.Message);
    //                            if (MyUtility.Msg.QuestionBox("failed to open form : " + ex.Message + "\r\n\r\nRetry?") == DialogResult.No)
    //                                return;
    //                        }
    //                    } while (p == null);

    // while (true)
    //                    {
    //                        if (p.WaitForExit(500) == false)
    //                        {
    //                            if (frm.IsDisposed)
    //                            {
    //                                p.Kill();
    //                                p.Dispose();
    //                                p = null;
    //                                break;
    //                            }
    //                        }
    //                        else
    //                        {
    //                            frm.Invoke((Action<ToolStripMenuItem>)((m) => m.Enabled = true), menuItemInput);
    //                            break;
    //                        }
    //                    }

    // })).Start(new object[] { this.MenuItem, this.Serialized() });
    //                return null;
    //            }
    //            else
    //            {
    //                arrArg = string.IsNullOrWhiteSpace(StrArg) ?
    //                    new Object[1] { this.MenuItem } :
    //                    new Object[2] { this.MenuItem, StrArg };
    //                Form frm;
    //                if (TypeofControl.GetConstructor(new Type[] { typeof(ToolStripMenuItem), typeof(string) }) != null)
    //                    frm = ((Sci.Win.Tems.Base)Activator.CreateInstance(TypeofControl, arrArg));
    //                else
    //                    frm = ((Sci.Win.Tems.Base)Activator.CreateInstance(TypeofControl, this.MenuItem));
    //                frm.Text += " " + this.FormTextSufix;
    //                return frm;
    //            }
    //        }
    //    }

    // }
}
