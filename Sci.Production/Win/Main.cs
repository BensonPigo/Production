using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sci.Production.Basic;

using Ict;
using Ict.Win;
namespace Sci.Production
{
    public partial class Main : Sci.Win.Apps.Base
    {
        delegate Sci.Win.Tems.Base CREATETEMPLATE(ToolStripMenuItem menuitem);

        class TemplateInfo
        {
            public string text;
            public CREATETEMPLATE create;
            public ToolStripMenuItem menuitem;
        }
        public Main()
        {
            InitializeComponent();

            {
                ToolStripMenuItem winmenu;
                menus.Items.Add(winmenu = new ToolStripMenuItem("Window")
                {
                    Name = "WINDOW",
                    Alignment = ToolStripItemAlignment.Right,
                });
            }
        }
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            if (!Env.DesignTime)
            {
                if (null == Env.User)
                {
                    BeginInvoke(new Action(() =>
                    {
                        OpenLogin();
                    }));
                }
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

        Sci.Production.Win.Login login;

        int _isgeneralclosing;
        IList<TemplateInfo> _templates = new List<TemplateInfo>();

        #region 屬性
        private bool IsGeneralClosing { get { return 0 < _isgeneralclosing; } }
        #endregion
        #region 應用函式
        public DualResult DoLogin(IUserInfo user)
        {
            if (null == user) return Result.F_ArgNull("user");
            if (!OnLogout()) return new DualResult(false, "Cannot logout current user.");


            DualResult result = null;
            DataTable dtDDTable = null;
            DataTable dtMenu = null;
            DataRow[] drs = null;

            if (Sci.Env.Cfg.CodePage == 950)
            {
                if (!(result = Sci.Data.DBProxy.Current.Select(null, "SELECT * FROM DDTable", out dtDDTable)))
                {
                    return new DualResult(false, "Can't get DDTable table!");
                }
            }
            if (!(result = Sci.Data.DBProxy.Current.Select(null, "SELECT * FROM Menu ORDER BY MenuNo, BarNo", out dtMenu)))
            {
                return new DualResult(false, "Can't get Menu table!");
            }

            foreach (DataRow dr in dtMenu.Rows)
            {
                // For MIS專用頁面, 登入者非MIS則略過產生
                if (!myUtility.Empty(dr["FORMISONLY"]) && !Sci.Env.User.IsMIS) continue;

                // Menu及Form語言轉換
                if (Sci.Env.Cfg.CodePage == 950)
                {
                    switch (dr["OBJECTCODE"].ToString())
                    {
                        case "1":
                        case "2":
                            drs = dtDDTable.Select(string.Format("ID = '{0}'", dr["MENUNAME"].ToString().Trim()));
                            if (drs.Length > 0)
                            {
                                dr["MENUNAME"] = drs[0]["NAME950"].ToString();
                            }
                            break;
                        case "4":
                        case "5":
                            drs = dtDDTable.Select(string.Format("ID = '{0}'", dr["FORMNAME"].ToString().Trim()));
                            if (drs.Length > 0)
                            {
                                dr["BARPROMPT"] = drs[0]["NAME950"].ToString();
                            }
                            break;
                    }
                }

                // 產生Menu及Form
                string dllName = "";
                switch (dr["OBJECTCODE"].ToString())
                {
                    case "1":  // Menu
                        menus.Items.Add(progmenu = new ToolStripMenuItem(dr["MENUNAME"].ToString()));
                        progmenu.DropDownItemClicked += progmenu_DropDownItemClicked;
                        break;
                    case "2":  // SubMenu
                        subMenu = AddMenu(progmenu, dr["MENUNAME"].ToString());
                        subMenu.DropDownItemClicked += progmenu_DropDownItemClicked;
                        break;
                    case "3":  // Separtor
                        progmenu.DropDownItems.Add(separator = new ToolStripSeparator());
                        break;
                    case "4":  // Form
                    case "5":  // SubMenu_Form
                        dllName = dr["FORMNAME"].ToString().Substring(0, dr["FORMNAME"].ToString().LastIndexOf("."));
                        AddTemplate((dr["OBJECTCODE"].ToString() == "4" ? progmenu : subMenu), dr["BARPROMPT"].ToString(), (menuitem) => (Sci.Win.Tems.Base)CreateFormObject(menuitem, Type.GetType(dr["FORMNAME"].ToString() + "," + dllName), dr["FORMPARAMETER"].ToString()));
                        break;
                }
            }


            Env.User = user;

            if (!(result = SetTemplatePerm()))
            {
                OnLogout();
                return result;
            }

            return Result.True;
        }

        private Object CreateFormObject(ToolStripMenuItem menuItem, Type typeofControl, String strArg)
        {
            Object[] arrArg = null;
            arrArg = string.IsNullOrWhiteSpace(strArg) ? new Object[1] { menuItem } : new Object[2] { menuItem, strArg };
            Object formClass = Activator.CreateInstance(typeofControl, arrArg);
            return formClass;
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
