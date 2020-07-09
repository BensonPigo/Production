using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Configuration;
using Ict;
using Ict.Win;
using System.Diagnostics;
using System.IO;

namespace Sci.Production
{
#pragma warning disable 1591
    /// <summary>
    /// Main
    /// </summary>
    public partial class Main : Sci.Win.Apps.Base
    {
        /// <summary>
        /// CREATETEMPLATE
        /// </summary>
        /// <param name="menuitem">Tool Strip Menu Item</param>
        /// <returns>Sci.Win.Tems.Base</returns>
        internal delegate Sci.Win.Tems.Base CREATETEMPLATE(ToolStripMenuItem menuitem);

        private static Dictionary<Process, ToolStripMenuItem> proList = new Dictionary<Process, ToolStripMenuItem>();
        private static string strMaxVerDirName = string.Empty;

        /// <summary>
        /// TemplateInfo
        /// </summary>
        internal class TemplateInfo
        {
            /// <summary>
            /// text
            /// </summary>
            public string Text { get; set; }

            /// <summary>
            /// create
            /// </summary>
            public CREATETEMPLATE Create { get; set; }

            /// <summary>
            /// menuitem
            /// </summary>
            public ToolStripMenuItem Menuitem { get; set; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Main"/> class.
        /// </summary>
        public Main()
        {
            Env.App = this;
            this.InitializeComponent();

            // if (Debugger.IsAttached)
            // {
            ToolStripMenuItem winmenu;
            this.menus.Items.Add(winmenu = new ToolStripMenuItem("Window")
            {
                Name = "WINDOW",
                Alignment = ToolStripItemAlignment.Right,
            });
            DirectoryInfo dI = new DirectoryInfo(Env.Cfg.XltPathDir);
            if (File.Exists(dI.Parent.Parent.FullName + "\\Logo.bmp"))
            {
                Image image = Image.FromFile(dI.Parent.Parent.FullName + "\\Logo.bmp");
                this.BackgroundImage = image;
                this.BackgroundImageLayout = ImageLayout.Tile;
            }

            if (File.Exists(dI.Parent.Parent.FullName + "\\Logo.jpg"))
            {
                Image image1 = Image.FromFile(dI.Parent.Parent.FullName + "\\Logo.jpg");
                this.BackgroundImage = image1;
                this.BackgroundImageLayout = ImageLayout.Tile;
            }

            // }
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            if (!Env.DesignTime)
            {
                this.Shown += (s, e) =>
                {
                    if (Env.User == null)
                    {
                        this.OpenLogin();
                    }
                };
            }

            this.OnLineHelpID = "Production";
        }

        /// <inheritdoc/>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (!this.IsGeneralClosing)
            {
                if (!this.OnExit())
                {
                    e.Cancel = true;
                    return;
                }
            }
        }

        private ToolStripMenuItem progmenu = null;
        private ToolStripMenuItem subMenu = null;
        private ToolStripSeparator separator = null;
        private DualResult result = null;
        private DataTable dtDDTable = null;
        private DataTable dtMenu = null;
        private DataTable dtMenuDetail = null;
        private DataRow[] drs = null;
        private Win.Login login;
        private int _isgeneralclosing;
        private IList<TemplateInfo> _templates = new List<TemplateInfo>();

        #region 屬性
        private bool IsGeneralClosing
        {
            get { return this._isgeneralclosing > 0; }
        }

        #endregion
        #region 應用函式

        /// <summary>
        /// DoLogin
        /// </summary>
        /// <param name="user">User Info</param>
        /// <returns>DualResult</returns>
        public DualResult DoLogin(IUserInfo user)
        {
            if (user == null)
            {
                return Ict.Result.F_ArgNull("user");
            }

            if (!this.OnLogout())
            {
                return new DualResult(false, "Cannot logout current user.");
            }

            if (!(this.result = Data.DBProxy.Current.Select(null, "SELECT * FROM Menu ORDER BY MenuNo, IsSubMenu", out this.dtMenu)))
            {
                return new DualResult(false, "Can't get Menu table!");
            }

            if (!(this.result = Data.DBProxy.Current.Select(null, "SELECT * FROM MenuDetail ORDER BY Ukey, BarNo", out this.dtMenuDetail)))
            {
                return new DualResult(false, "Can't get Menu table!");
            }

            // 中英文轉換
            if (Env.Cfg.CodePage == 950)
            {
                if (!(this.result = Data.DBProxy.Current.Select(null, "SELECT * FROM DDTable", out this.dtDDTable)))
                {
                    return new DualResult(false, "Can't get DDTable table!");
                }

                this.DDConvert();
            }

            Env.User = user;

            // 產生Menu
            foreach (DataRow dr in this.dtMenu.Rows)
            {
                if (!MyUtility.Check.Empty(dr["ForMISOnly"]) && !Env.User.IsMIS)
                {
                    continue;
                }

                if (!MyUtility.Check.Empty(dr["IsSubMenu"]))
                {
                    continue;
                }

                if (dr["MenuName"].ToString().Contains("Centralized") && ConfigurationManager.AppSettings["TaipeiServer"] == string.Empty)
                {
                    continue;
                }

                this.progmenu = new ToolStripMenuItem(dr["MenuName"].ToString())
                {
                    Overflow = ToolStripItemOverflow.AsNeeded,
                };
                this.menus.Items.Add(this.progmenu);
                this.progmenu.DropDownItemClicked += this.Progmenu_DropDownItemClicked;

                this.drs = this.dtMenuDetail.Select(string.Format("UKey = '{0}'", dr["PKey"].ToString().Trim()));
                this.GenerateMenu(this.progmenu, this.drs);
            }

            if (!(this.result = this.SetTemplatePerm()))
            {
                this.OnLogout();
                return this.result;
            }

            return Ict.Result.True;
        }

        private void DDConvert()
        {
            foreach (DataRow dr in this.dtMenu.Rows)
            {
                this.drs = this.dtDDTable.Select(string.Format("ID = '{0}'", dr["MenuName"].ToString().Trim()));
                if (this.drs.Length > 0)
                {
                    dr["MenuName"] = this.drs[0]["Name950"].ToString();
                }
            }

            foreach (DataRow dr in this.dtMenuDetail.Rows)
            {
                switch (dr["ObjectCode"].ToString())
                {
                    case "0": // Form
                        this.drs = this.dtDDTable.Select(string.Format("ID = '{0}'", dr["FormName"].ToString().Trim()));
                        if (this.drs.Length > 0)
                        {
                            dr["BarPrompt"] = this.drs[0]["Name950"].ToString();
                        }

                        break;
                    case "1": // SubMenu
                        this.drs = this.dtDDTable.Select(string.Format("ID = '{0}'", dr["BarPrompt"].ToString().Trim()));
                        if (this.drs.Length > 0)
                        {
                            dr["BarPrompt"] = this.drs[0]["Name950"].ToString();
                        }

                        break;
                }
            }
        }

        private void GenerateMenu(ToolStripMenuItem menu, DataRow[] details)
        {
            DataRow[] arrayDrs = null;
            string pKey = string.Empty;
            string dllName = string.Empty;

            foreach (DataRow dr in details)
            {
                switch (dr["ObjectCode"].ToString())
                {
                    case "0": // Form
                        if (MyUtility.Check.Empty(dr["BarPrompt"]) || MyUtility.Check.Empty(dr["FormName"]))
                        {
                            break;
                        }

                        if (!MyUtility.Check.Empty(dr["ForMISOnly"]) && !Env.User.IsMIS)
                        {
                            break;
                        }

                        dllName = dr["FormName"].ToString().Substring(0, dr["FormName"].ToString().LastIndexOf("."));

                        // PublicClass的Dll Name為Sci.Proj
                        if (dllName == "Sci.Win.UI")
                        {
                            dllName = "Sci.Proj";
                        }

                        string typeName = dr["FormName"].ToString() + "," + dllName;
                        Type typeofControl = Type.GetType(typeName);
                        this.AddTemplate(
                            menu,
                            dr["BarPrompt"].ToString(),
                            (menuitem) => (Sci.Win.Tems.Base)this.CreateFormObject(
                                    menuitem,
                                    typeofControl,
                                    dr["FormParameter"].ToString(),
                                    typeName));
                        break;

                    case "1": // SubMenu
                        this.subMenu = this.AddMenu(menu, dr["BarPrompt"].ToString());
                        this.subMenu.DropDownItemClicked += this.Progmenu_DropDownItemClicked;
                        arrayDrs = this.dtMenu.Select(string.Format("MenuName = '{0}'", dr["BarPrompt"].ToString().Trim()));
                        if (arrayDrs.Length > 0)
                        {
                            pKey = arrayDrs[0]["PKey"].ToString();
                            arrayDrs = this.dtMenuDetail.Select(string.Format("UKey = {0}", pKey));
                            this.GenerateMenu(this.subMenu, arrayDrs);
                        }

                        break;

                    case "2": // Seperator
                        menu.DropDownItems.Add(this.separator = new ToolStripSeparator());
                        break;
                }
            }
        }

        private object CreateFormObject(ToolStripMenuItem menuItem, Type typeofControl, string strArg)
        {
            object[] arrArg = null;
            arrArg = string.IsNullOrWhiteSpace(strArg) ? new object[1] { menuItem } : new object[2] { menuItem, strArg };
            object formClass = null;
            try
            {
                formClass = Activator.CreateInstance(typeofControl, arrArg);
                if (formClass is Sci.Win.IForm && !string.IsNullOrWhiteSpace(strArg))
                {
                    Sci.Win.IForm iform = (Sci.Win.IForm)formClass;
                    iform.FormParameter = strArg;
                }
            }
            catch (Exception e)
            {
                this.ShowErr(e);
            }

            return formClass;
        }

        private object CreateFormObject(ToolStripMenuItem menuItem, Type typeofControl, string strArg, string formName)
        {
            bool isSwitchFactory = formName.IndexOf(typeof(Tools.SwitchFactory).FullName, StringComparison.OrdinalIgnoreCase) >= 0;
            if (isSwitchFactory)
            {
                var formObj = this.CreateFormObject(menuItem, typeofControl, strArg);

                var form = (Form)formObj;
                form.FormClosed += (s, e) =>
                {
                    if (form.DialogResult == DialogResult.OK)
                    {
                        Kill_Process();
                    }
                };

                return formObj;
            }

            // else if (!Debugger.IsAttached)
            // {
            //    try
            //    {
            //        string cmd = string.Format("\"userid:{0}\" \"factoryID:{1}\" \"formName:{2}\" \"menuName:{3}\" \"args:{4}\""
            //            , Env.User.UserID
            //            , Env.User.Factory
            //            , formName
            //            , menuItem.Text
            //            , strArg);
            //        Process myProcess = Process.Start(Application.ExecutablePath,cmd);
            //        bool startSuccess = false;
            //        try {
            //            int newPid = myProcess.Id; // 如果process start失敗, 就會抓不到 id, 跳出exception
            //            startSuccess = true;
            //        } catch { }
            //        if (startSuccess)
            //        {
            //            myProcess.EnableRaisingEvents = true;
            //            myProcess.Exited += myProcess_Exited;
            //            proList.Add(myProcess, menuItem);
            //            menuItem.Enabled = false;
            //        }
            //    }
            //    catch
            //    {

            // }
            //    return null;
            // }
            else
            {
                var formObj = this.CreateFormObject(menuItem, typeofControl, strArg);
                return formObj;
            }
        }

        private void MyProcess_Exited(object sender, EventArgs e)
        {
            Process p = (Process)sender;

            if (proList.ContainsKey(p))
            {
                var menuItem = proList[p];
                proList.Remove(p);
                this.Invoke(() => { menuItem.Enabled = true; });

                // Cross-thread operation not valid: Control '' accessed from a thread other than the thread it was created on.
                // this.proList[p].Enabled = true;
            }
        }

        /// <inheritdoc/>
        protected override void OnClosed(EventArgs e)
        {
            Kill_Process();
            base.OnClosed(e);
        }

        /// <summary>
        /// Kill Process
        /// </summary>
        public static void Kill_Process()
        {
            var processArray = proList.Keys.Where(p => !p.HasExited).ToArray();
            foreach (var p in processArray)
            {
                p.Kill();
            }

            proList.Clear();
        }

        /// <summary>
        /// Do Logout
        /// </summary>
        /// <param name="confirm">bool Confirm</param>
        public void DoLogout(bool confirm = true)
        {
            if (confirm)
            {
                if (!MsgHelper.Current.Confirm(this, "Do you want to logout?"))
                {
                    return;
                }
            }

            if (!this.OnLogout())
            {
                return;
            }

            this.OpenLogin();
        }

        private bool OnLogout()
        {
            if (!this.CloseTemplates())
            {
                return false;
            }

            if (this.progmenu != null)
            {
                this.menus.Items.Remove(this.progmenu);
                this.progmenu.Dispose();
                this.progmenu = null;
            }

            this._templates.Clear();

            Env.User = null;
            return true;
        }

        /// <summary>
        /// Do Exit
        /// </summary>
        /// <param name="confirm">Bool Confirm</param>
        public void DoExit(bool confirm = true)
        {
            if (this.IsFormClosing)
            {
                return;
            }

            ++this._isgeneralclosing;
            try
            {
                if (!this.OnExit(confirm: confirm))
                {
                    return;
                }

                this.Close();
            }
            finally
            {
                --this._isgeneralclosing;
            }
        }

        private bool OnExit(bool confirm = true)
        {
            if (confirm)
            {
                if (!MsgHelper.Current.Confirm(this, "Close demo system?"))
                {
                    return false;
                }
            }

            if (!this.CloseTemplates())
            {
                return false;
            }

            return true;
        }

        private void OpenLogin()
        {
            if (this.login != null)
            {
                if (this.login.IsFormClosed)
                {
                    this.login.Dispose();
                    this.login = null;
                }
                else
                {
                    this.login.Activate();
                    return;
                }
            }

            this.login = new Win.Login(this);
            this.login.FormClosed += (s, e) =>
            {
                if (Env.User == null)
                {
                    this.DoExit(confirm: false);
                }
            };
            this.OpenForm(this.login);
        }

        private void OpenForm(Sci.Win.Forms.Base form)
        {
            // Check是否是最新版本
            var isLaterstVersion = CheckAppIsNotLatestVersion();
            if (isLaterstVersion == false)
            {
                form.Text += @"<<< NOT LATEST >>>";
            }

            if (form != null && form is Form)
            {
                form.MdiParent = this;
                form.Show();
                form.Focus();
            }

            if (isLaterstVersion == false)
            {
                this.AlertForNotLatestVersion();
            }
        }

        private bool AlertForNotLatestVersion()
        {
            Application.DoEvents();
            string showMsg = $@"
New version {strMaxVerDirName} is updated!!!

Please re-login system.

Thank you
-----------------------------
注意!!

已經有新版本{strMaxVerDirName}發布

請將手邊事情告一段落後，重新登入系統

謝謝您的配合";
            MessageBox.Show(
                new Form() { TopMost = true },
                showMsg,
                "Warnning",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            return true;
        }

        /// <summary>
        /// Check App Is Not Latest Version
        /// </summary>
        /// <returns>Bool Latest Version</returns>
        internal static bool CheckAppIsNotLatestVersion()
        {
            if (((bool?)AppDomain.CurrentDomain.GetData("NewVerDetectionByLuncher")).GetValueOrDefault(false) == true)
            {
                var hasNewVersion = ((bool?)AppDomain.CurrentDomain.GetData("HasNewVersion")).GetValueOrDefault(false);
                return !hasNewVersion;
            }

            var appDir = AppDomain.CurrentDomain.BaseDirectory;
            var appDI = new DirectoryInfo(appDir); // v0215_1050 (Debug)
            var parentDI = appDI.Parent; // MIS (x86)
            var rePattern = @"Production_[0-9]{12}";

            var m = System.Text.RegularExpressions.Regex.Match(appDI.Name, rePattern);
            if (m.Success == true)
            {
                var maxVerDirName = parentDI.GetDirectories()
                    .Select(childDI => System.Text.RegularExpressions.Regex.Match(childDI.Name, rePattern))
                    .Where(r => r.Success == true)
                    .Select(r => r.Value)
                    .Max();
                if (maxVerDirName != null && maxVerDirName != appDI.Name)
                {
                    strMaxVerDirName = maxVerDirName;
                    return false;
                }
            }

            return true;
        }

        private ToolStripMenuItem AddMenu(ToolStripMenuItem owner, string text, EventHandler onclick = null)
        {
            var menuitem = new ToolStripMenuItem(text);
            if (onclick != null)
            {
                menuitem.Click += onclick;
            }

            owner.DropDownItems.Add(menuitem);
            return menuitem;
        }

        private ToolStripMenuItem AddTemplate(ToolStripMenuItem owner, string text, CREATETEMPLATE create)
        {
            var menuitem = new ToolStripMenuItem(text);

            var templateinfo = new TemplateInfo
            {
                Text = text,
                Create = create,
                Menuitem = menuitem,
            };
            menuitem.Tag = templateinfo;

            owner.DropDownItems.Add(menuitem);

            this._templates.Add(templateinfo);
            return menuitem;
        }

        private DualResult SetTemplatePerm()
        {
            if (this._templates.Count == 0)
            {
                return Ict.Result.True;
            }

            DualResult result;

            var sysuser = Env.User;
            if (sysuser == null)
            {
                foreach (var it in this._templates)
                {
                    it.Menuitem.Enabled = false;
                }

                return Ict.Result.True;
            }
            else if (sysuser.IsAdmin)
            {
                foreach (var it in this._templates)
                {
                    it.Menuitem.Enabled = true;
                }

                return Ict.Result.True;
            }

            IList<string> barprompts;
            if (!(result = Utils.GetAuthorizedMenus(sysuser.UserID, out barprompts)))
            {
                return result;
            }

            foreach (var it in this._templates)
            {
                it.Menuitem.Enabled = barprompts.Contains(it.Text);
            }

            return Ict.Result.True;
        }
        #endregion

        private void Progmenu_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            TemplateInfo templateinfo = e.ClickedItem.Tag as TemplateInfo;
            if (templateinfo != null)
            {
                if (templateinfo.Create != null)
                {
                    var frm = templateinfo.Create((ToolStripMenuItem)e.ClickedItem);
                    if (frm != null)
                    {
                        this.OpenForm(frm);
                    }
                }
            }
        }
    }
}
