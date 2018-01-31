using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sci.Production.Basic;
using System.Configuration;
using Ict;
using Ict.Win;
using System.Diagnostics;
using System.IO;

namespace Sci.Production
{
#pragma warning disable 1591
    public partial class Main : Sci.Win.Apps.Base
    {
        delegate Sci.Win.Tems.Base CREATETEMPLATE(ToolStripMenuItem menuitem);
        static Dictionary<Process, ToolStripMenuItem> proList = new Dictionary<Process, ToolStripMenuItem>();

        class TemplateInfo
        {
            public string text;
            public CREATETEMPLATE create;
            public ToolStripMenuItem menuitem;
        }
        public Main()
        {
            Sci.Env.App = this;
            InitializeComponent();
            //if (Debugger.IsAttached)
            //{
                ToolStripMenuItem winmenu;
                menus.Items.Add(winmenu = new ToolStripMenuItem("Window")
                {
                    Name = "WINDOW",
                    Alignment = ToolStripItemAlignment.Right,
                });
                DirectoryInfo DI = new DirectoryInfo(Sci.Env.Cfg.XltPathDir);
                if (System.IO.File.Exists(DI.Parent.Parent.FullName + "\\Logo.bmp"))
                {
                    Image Image = Image.FromFile(DI.Parent.Parent.FullName + "\\Logo.bmp");
                    this.BackgroundImage = Image;
                    this.BackgroundImageLayout = ImageLayout.Tile;
                }
                if (System.IO.File.Exists(DI.Parent.Parent.FullName + "\\Logo.jpg"))
                {
                    Image Image = Image.FromFile(DI.Parent.Parent.FullName + "\\Logo.jpg");
                    this.BackgroundImage = Image;
                    this.BackgroundImageLayout = ImageLayout.Tile;
                }
                
            //}
        }
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            if (!Env.DesignTime)
            {
                this.Shown+=(s,e)=>{
                    if (null == Env.User)
                    {
                        OpenLogin();
                    }
                };

                
            }
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (!IsGeneralClosing)
            {
                if (!OnExit())
                {
                    e.Cancel = true;
                    return;
                }
            }
        }


        ToolStripMenuItem progmenu = null;
        ToolStripMenuItem subMenu = null;
        ToolStripSeparator separator = null;
        DualResult result = null;
        DataTable dtDDTable = null;
        DataTable dtMenu = null;
        DataTable dtMenuDetail = null;
        DataRow[] drs = null;

        Sci.Production.Win.Login login;

        int _isgeneralclosing;
        IList<TemplateInfo> _templates = new List<TemplateInfo>();
        public static string FormTextSufix = "";

        #region 屬性
        private bool IsGeneralClosing { get { return 0 < _isgeneralclosing; } }
        #endregion
        #region 應用函式
        public DualResult DoLogin(IUserInfo user)
        {
            if (null == user) return Result.F_ArgNull("user");
            if (!OnLogout()) return new DualResult(false, "Cannot logout current user.");

            if (!(result = Sci.Data.DBProxy.Current.Select(null, "SELECT * FROM Menu ORDER BY MenuNo, IsSubMenu", out dtMenu)))
            {
                return new DualResult(false, "Can't get Menu table!");
            }
            if (!(result = Sci.Data.DBProxy.Current.Select(null, "SELECT * FROM MenuDetail ORDER BY Ukey, BarNo", out dtMenuDetail)))
            {
                return new DualResult(false, "Can't get Menu table!");
            }

            // 中英文轉換
            if (Sci.Env.Cfg.CodePage == 950)
            {
                if (!(result = Sci.Data.DBProxy.Current.Select(null, "SELECT * FROM DDTable", out dtDDTable)))
                {
                    return new DualResult(false, "Can't get DDTable table!"); ;
                }
                DDConvert();
            }

            Env.User = user;

            // 產生Menu
            foreach (DataRow dr in dtMenu.Rows)
            {               
                if (!MyUtility.Check.Empty(dr["ForMISOnly"]) && !Sci.Env.User.IsMIS) continue;
                if (!MyUtility.Check.Empty(dr["IsSubMenu"])) continue;
                if (dr["MenuName"].ToString().Contains("Centralized") && ConfigurationManager.AppSettings["TaipeiServer"] == "") continue;

                progmenu = new ToolStripMenuItem(dr["MenuName"].ToString());
                progmenu.Overflow = ToolStripItemOverflow.AsNeeded;
                menus.Items.Add(progmenu);
                progmenu.DropDownItemClicked += progmenu_DropDownItemClicked;

                drs = dtMenuDetail.Select(string.Format("UKey = '{0}'", dr["PKey"].ToString().Trim()));
                GenerateMenu(progmenu, drs);
            }

            if (!(result = SetTemplatePerm()))
            {
                OnLogout();
                return result;
            }

            return Result.True;
        }

        private void DDConvert()
        {
            foreach (DataRow dr in dtMenu.Rows)
            {
                drs = dtDDTable.Select(string.Format("ID = '{0}'", dr["MenuName"].ToString().Trim()));
                if (drs.Length > 0)
                {
                    dr["MenuName"] = drs[0]["Name950"].ToString();
                }
            }

            foreach (DataRow dr in dtMenuDetail.Rows)
            {
                switch (dr["ObjectCode"].ToString())
                {
                    case "0": // Form
                        drs = dtDDTable.Select(string.Format("ID = '{0}'", dr["FormName"].ToString().Trim()));
                        if (drs.Length > 0)
                        {
                            dr["BarPrompt"] = drs[0]["Name950"].ToString();
                        }
                        break;
                    case "1": // SubMenu
                        drs = dtDDTable.Select(string.Format("ID = '{0}'", dr["BarPrompt"].ToString().Trim()));
                        if (drs.Length > 0)
                        {
                            dr["BarPrompt"] = drs[0]["Name950"].ToString();
                        }
                        break;
                }
            }
        }

        private void GenerateMenu(ToolStripMenuItem menu, DataRow[] details)
        {
            DataRow[] arrayDrs = null;
            string pKey = "";
            string dllName = "";

            foreach (DataRow dr in details)
            {
                switch (dr["ObjectCode"].ToString())
                {
                    case "0": // Form
                        if (MyUtility.Check.Empty(dr["BarPrompt"]) || MyUtility.Check.Empty(dr["FormName"])) break;
                        if (!MyUtility.Check.Empty(dr["ForMISOnly"]) && !Sci.Env.User.IsMIS) break;

                        dllName = dr["FormName"].ToString().Substring(0, dr["FormName"].ToString().LastIndexOf("."));
                        // PublicClass的Dll Name為Sci.Proj
                        if (dllName == "Sci.Win.UI") dllName = "Sci.Proj";

                        string typeName = dr["FormName"].ToString() + "," + dllName;
                        Type typeofControl = Type.GetType(typeName);
                        AddTemplate(menu
                            , dr["BarPrompt"].ToString()
                            , (menuitem) => (Sci.Win.Tems.Base)CreateFormObject(
                                    menuitem
                                    , typeofControl
                                    , dr["FormParameter"].ToString()
                                    , typeName)
                             );
                        break;

                    case "1": //SubMenu
                        subMenu = AddMenu(menu, dr["BarPrompt"].ToString());
                        subMenu.DropDownItemClicked += progmenu_DropDownItemClicked;
                        arrayDrs = dtMenu.Select(string.Format("MenuName = '{0}'", dr["BarPrompt"].ToString().Trim()));
                        if (arrayDrs.Length > 0)
                        {
                            pKey = arrayDrs[0]["PKey"].ToString();
                            arrayDrs = dtMenuDetail.Select(string.Format("UKey = {0}", pKey));
                            GenerateMenu(subMenu, arrayDrs);
                        }
                        break;

                    case "2":  //Seperator
                        menu.DropDownItems.Add(separator = new ToolStripSeparator());
                        break;
                }
            }
        }

        private Object CreateFormObject(ToolStripMenuItem menuItem, Type typeofControl, String strArg)
        {
            Object[] arrArg = null;
            arrArg = string.IsNullOrWhiteSpace(strArg) ? new Object[1] { menuItem } : new Object[2] { menuItem, strArg };
            Object formClass = null;
            try
            {
                formClass = Activator.CreateInstance(typeofControl, arrArg);
            }
            catch (Exception e)
            {
                this.ShowErr(e);                
            }
            return formClass;
        }

        private Object CreateFormObject(ToolStripMenuItem menuItem, Type typeofControl, String strArg, string formName)
        {
            bool isSwitchFactory = formName.IndexOf(typeof(Sci.Production.Tools.SwitchFactory).FullName,StringComparison.OrdinalIgnoreCase)>=0;
            if (isSwitchFactory) { 
                var formObj = CreateFormObject( menuItem,  typeofControl,  strArg);

                var form = (Form)formObj;
                form.FormClosed += (s, e) =>
                {
                    if (form.DialogResult == DialogResult.OK)
                    {
                        Sci.Production.Main.Kill_Process();
                    }
                };

                return formObj;
            }
            //else if (!Debugger.IsAttached)
            //{
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

            //    }
            //    return null;
            //}
            else
            {
                var formObj = CreateFormObject(menuItem, typeofControl, strArg);
                return formObj;
            }

        }


        void myProcess_Exited(object sender, EventArgs e)
        {
            Process p = (Process)sender;

            if (proList.ContainsKey(p))
            {
                var menuItem = proList[p];
                proList.Remove(p);
                this.Invoke(() => { menuItem.Enabled = true; });

                //Cross-thread operation not valid: Control '' accessed from a thread other than the thread it was created on.
                //this.proList[p].Enabled = true;
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            Kill_Process();
            base.OnClosed(e);

        }
        
        public static void Kill_Process()
        {
            var processArray = proList.Keys.Where(p => !p.HasExited).ToArray();
            foreach (var p in processArray)
            {
                p.Kill();

            }
            proList.Clear();
        }

        public void DoLogout(bool confirm = true)
        {
            if (confirm)
            {
                if (!MsgHelper.Current.Confirm(this, "Do you want to logout?"))
                {
                    return;
                }
            }

            if (!OnLogout()) return;

            OpenLogin();
        }
        private bool OnLogout()
        {
            if (!CloseTemplates()) return false;

            if (null != progmenu)
            {
                menus.Items.Remove(progmenu);
                progmenu.Dispose(); progmenu = null;
            }
            _templates.Clear();

            Env.User = null;
            return true;
        }
        public void DoExit(bool confirm = true)
        {
            if (IsFormClosing) return;

            ++_isgeneralclosing;
            try
            {
                if (!OnExit(confirm: confirm)) return;

                Close();
            }
            finally { --_isgeneralclosing; }
        }
        private bool OnExit(bool confirm = true)
        {
            if (confirm)
            {
                if (!MsgHelper.Current.Confirm(this, "Close demo system?")) return false;
            }

            if (!CloseTemplates()) return false;

            return true;
        }

        private void OpenLogin()
        {
            if (null != login)
            {
                if (login.IsFormClosed)
                {
                    login.Dispose();
                    login = null;
                }
                else
                {
                    login.Activate();
                    return;
                }
            }

            login = new Win.Login(this);
            login.FormClosed += (s, e) =>
            {
                if (null == Env.User) DoExit(confirm: false);
            };
            OpenForm(login);
        }
        private void OpenForm(Sci.Win.Forms.Base form)
        {
            form.MdiParent = this;
            form.Show();
            form.Focus();
        }
        private ToolStripMenuItem AddMenu(ToolStripMenuItem owner, string text, EventHandler onclick = null)
        {
            var menuitem = new ToolStripMenuItem(text);
            if (null != onclick) menuitem.Click += onclick;

            owner.DropDownItems.Add(menuitem);
            return menuitem;
        }
        private ToolStripMenuItem AddTemplate(ToolStripMenuItem owner, string text, CREATETEMPLATE create)
        {
            var menuitem = new ToolStripMenuItem(text);

            var templateinfo = new TemplateInfo();
            templateinfo.text = text;
            templateinfo.create = create;
            templateinfo.menuitem = menuitem;
            menuitem.Tag = templateinfo;

            owner.DropDownItems.Add(menuitem);

            _templates.Add(templateinfo);
            return menuitem;
        }
        private DualResult SetTemplatePerm()
        {
            if (0 == _templates.Count) return Result.True;
            DualResult result;

            var sysuser = Env.User;
            if (null == sysuser)
            {
                foreach (var it in _templates) it.menuitem.Enabled = false;
                return Result.True;
            }
            else if (sysuser.IsAdmin)
            {
                foreach (var it in _templates) it.menuitem.Enabled = true;
                return Result.True;
            }

            IList<string> barprompts;
            if (!(result = Utils.GetAuthorizedMenus(sysuser.UserID, out barprompts))) return result;

            foreach (var it in _templates)
            {
                it.menuitem.Enabled = barprompts.Contains(it.text);
            }
            return Result.True;
        }
        #endregion

        void progmenu_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            var templateinfo = e.ClickedItem.Tag as TemplateInfo;
            if (null != templateinfo)
            {
                if (null != templateinfo.create)
                {
                    var frm = templateinfo.create((ToolStripMenuItem)e.ClickedItem);
                    if (null != frm) OpenForm(frm);
                }
            }
        }
    }
}
