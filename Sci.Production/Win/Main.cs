using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

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
            DualResult result;

            {
                //menus.Items.Add(progmenu = new ToolStripMenuItem("固定資產管理"));
                //progmenu.DropDownItemClicked += progmenu_DropDownItemClicked;
                //AddTemplate(progmenu, "B01.資產取得代碼", (menuitem) => new Sci.Win.FAB01(menuitem));
                //AddTemplate(progmenu, "B02.財產目錄設定", (menuitem) => new Sci.Win.FAB02(menuitem));
                //AddTemplate(progmenu, "B03.資產類別設定", (menuitem) => new Sci.Win.FAB03(menuitem));
                //AddTemplate(progmenu, "B04.帳務類別設定", (menuitem) => new Sci.FA.FAB04(menuitem));
                //AddTemplate(progmenu, "P01.固定資產取得作業", (menuitem) => new Sci.Win.FAP01(menuitem));
                //AddTemplate(progmenu, "P99.固定資產取得作業(Test)", (menuitem) => new Sci.Win.FAP01(menuitem));
                //AddTemplate(progmenu, "範本6", (menuitem) => new Sci.Win.FORM6(menuitem));
                //AddTemplate(progmenu, "範本7", (menuitem) => new Sci.Win.FORM7(menuitem));
                //AddTemplate(progmenu, "範本Q", (menuitem) => new Sci.Win.QUERY(menuitem));
                //AddTemplate(progmenu, "範本R", (menuitem) => new Sci.Win.REPORT(menuitem));

                ////Parent Menu
                //menus.Items.Add(progmenu = new ToolStripMenuItem("PMS測試"));
                //progmenu.DropDownItemClicked += progmenu_DropDownItemClicked;
                //AddTemplate(progmenu, "SCI元件測試", (menuitem) => new Sci.PMS.Test1(menuitem));
                //AddTemplate(progmenu, "SCI Test", (menuitem) => new Sci.PMS.PBDB998(menuitem));
                //AddTemplate(progmenu, "B02.System Parameter", (menuitem) => new Sci.PMS.PBDB020(menuitem));
                //AddTemplate(progmenu, "B05.Supplier/Sub Con(local)", (menuitem) => new Sci.PMS.PBDB050(menuitem));

                //// Sub Menu
                //subMenu = AddMenu(progmenu, "SubMenu");
                //subMenu.DropDownItemClicked += progmenu_DropDownItemClicked;
                //AddTemplate(subMenu, "SCI元件測試(2)", (menuitem) => new Sci.PMS.Test1(menuitem));
                //AddTemplate(subMenu, "B02.System Parameter(2)", (menuitem) => new Sci.PMS.PBDB020(menuitem));
                //AddTemplate(subMenu, "B05.Supplier/Sub Con(Loacl)(2)", (menuitem) => new Sci.PMS.PBDB050(menuitem));

                //AddTemplate(progmenu, "B80.CD Index", (menuitem) => new Sci.PMS.PBDB800(menuitem));
                //AddTemplate(progmenu, "B83.Cust CD", (menuitem) => new Sci.PMS.PBDB830(menuitem));
                //AddTemplate(progmenu, "P02.International Express", (menuitem) => new Sci.PMS.PGEP020(menuitem));
                //AddTemplate(progmenu, "P21.Replacement Report(Fabric)", (menuitem) => new Sci.PMS.PPCP210(menuitem));
                //AddTemplate(progmenu, "Input2/6", (menuitem) => new Sci.PMS.Test7(menuitem));
            }

            Env.User = user;

            if (!(result = SetTemplatePerm()))
            {
                OnLogout();
                return result;
            }

            return Result.True;
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
