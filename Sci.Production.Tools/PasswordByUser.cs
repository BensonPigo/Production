using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;
using Sci.Win;


namespace Sci.Production.Tools
{
    public partial class PasswordByUser : Sci.Win.Tems.Input1
    {
        private DataTable dtPass2 = null;
        private DataTable dtSystem = null;
        private DataTable dtFactory = null;
        private DualResult result = null;
        private string sqlCmd = "";

        public PasswordByUser(ToolStripMenuItem menuitem) : base(menuitem)
        {
            InitializeComponent();

            // MyApp.lAdmin = fasle時,
            // 1.則DefaultFilter設為 ID = MyApp.cLogin
            // 2.關閉New/Dele/Move功能
            if (!Sci.Env.User.IsAdmin)
            {
                this.DefaultFilter = "ID = '" + Sci.Env.User.UserID + "'";
                this.IsSupportNew = false;
                this.IsSupportDelete = false;
            }
            else
            {
                 this.editFactory.PopUp += (s, e) => 
                {
                    DBProxy.Current.Select(null, "SELECT DISTINCT FtyGroup FROM Factory WHERE FtyGroup != '' and Junk = 0 ORDER BY FtyGroup", out dtFactory);
                    Sci.Win.Tools.SelectItem2 seleItem2 = new Sci.Win.Tools.SelectItem2(dtFactory, "FtyGroup", "Factory", "15", (this.editFactory.Text).Replace(" ", ""));
                    if (seleItem2.ShowDialog(this) == DialogResult.OK)
                    {
                        CurrentMaintain["Factory"] = "";
                        IList<DataRow> returnValue = seleItem2.GetSelecteds();
                        foreach (DataRow dr in returnValue)
                        {
                            CurrentMaintain["Factory"] = CurrentMaintain["Factory"] + dr["FtyGroup"].ToString().PadRight(8) + ",";
                        }
                    }
                };

                this.txtPosition.PopUp += (s, e) =>
                {
                    Sci.Win.Tools.SelectItem seleItem = new Sci.Win.Tools.SelectItem("SELECT ID, Description, PKey From Pass0 ORDER BY ID", "15,25", this.txtPosition.Text, "Position,Description");
                    IList<DataRow> listSelect = null;
                    if (seleItem.ShowDialog(this) == DialogResult.OK)
                    {
                        listSelect = seleItem.GetSelecteds();
                        CurrentMaintain["FKPass0"] = (Int64)listSelect[0]["PKey"];
                        CurrentMaintain["Position"] = listSelect[0]["ID"].ToString();
                    }
                };
            }

            Dictionary<string, string> codePageSource = new Dictionary<string, string>();
            codePageSource.Add("950", "繁體中文");
            codePageSource.Add("0", "English");
            comboLanguage.DataSource = new System.Windows.Forms.BindingSource(codePageSource, null);
            comboLanguage.ValueMember = "Key";
            comboLanguage.DisplayMember = "Value";
        }

        protected override void SearchGridColumns()
        {
            if (locatefor.Text.Trim() == "")
            {
                return;
            }
            DataRow[] sdr = ((DataTable)gridbs.DataSource).Select(string.Format("Name like '%{0}%' or ID like '%{0}%'", locatefor.Text));
            DataTable dt;

            if (sdr.Length == 1)
            {
                base.SearchGridColumns();
            }
            else if (sdr.Length > 1)
            {
                dt = ((DataTable)gridbs.DataSource).Clone();
                foreach (DataRow dr in sdr)
                {
                    dt.ImportRow(dr);
                }
                dt.Columns.Add("lastTime");
                foreach (DataRow dr in dt.Rows)
                {
                    dr["lastTime"] = MyUtility.Check.Empty(dr["LastLoginTime"])?"":((DateTime)MyUtility.Convert.GetDate(dr["LastLoginTime"])).ToString("yyyy/MM/dd HH:mm:ss");
                }
                Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(dt, "ID,Name,lastTime", "15,20,20", this.Text, "ID,Name,Last Login Time");
                DialogResult result = item.ShowDialog();
                if (result == DialogResult.Cancel) return;
                locatefor.Text = item.GetSelectedString();
                base.SearchGridColumns();
            }
            else
            {
                base.SearchGridColumns();

            }
        }

        //protected override bool OnGridSetup()
        //{
        //    DataGridViewGeneratorTextColumnSettings ts = new DataGridViewGeneratorTextColumnSettings();
        //    ts.UseSystemPasswordChar = true;  // 預設為*
        //    //ts.PasswordChar = "*";

        //    Helper.Controls.Grid.Generator(this.grid)
        //        .Text("ID", header: "User ID", width: Widths.AnsiChars(10))
        //        .Text("NAME", header: "Name", width: Widths.AnsiChars(20))
        //        .Text("PASSWORD", header: "Password", width: Widths.AnsiChars(10), settings: ts)
        //        .CheckBox("ISADMIN", header: "Administrator", width: Widths.AnsiChars(1))
        //        .DateTime("LastLoginTime", header: "Last Login Time", width: Widths.AnsiChars(20));
        //    return true;
        //}

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            Helper.Controls.Grid.Generator(this.grid1)
                .Text("MenuName", "Menu", width: Widths.AnsiChars(12))
                .Text("BarPrompt", "Function", width: Widths.AnsiChars(30))
                .Text("Used", "Used", width: Widths.AnsiChars(1), alignment:DataGridViewContentAlignment.MiddleCenter)
                .CheckBox("CanNew", header: "New", width: Widths.AnsiChars(1), trueValue: true, falseValue: false)
                .CheckBox("CanEdit", header: "Edit", width: Widths.AnsiChars(1), trueValue: true, falseValue: false)
                .CheckBox("CanDelete", header: "Delete", width: Widths.AnsiChars(1), trueValue: true, falseValue: false)
                .CheckBox("CanPrint", header: "Print", width: Widths.AnsiChars(1), trueValue: true, falseValue: false)
                .CheckBox("CanConfirm", header: "Confirm", width: Widths.AnsiChars(1), trueValue: true, falseValue: false)
                .CheckBox("CanUnConfirm", header: "UnConfirm", width: Widths.AnsiChars(1), trueValue: true, falseValue: false)
                .CheckBox("CanSend", header: "Send", width: Widths.AnsiChars(1), trueValue: true, falseValue: false)
                .CheckBox("CanRecall", header: "Recall", width: Widths.AnsiChars(1), trueValue: true, falseValue: false)
                .CheckBox("CanCheck", header: "Check", width: Widths.AnsiChars(1), trueValue: true, falseValue: false)
                .CheckBox("CanUnCheck", header: "UnCheck", width: Widths.AnsiChars(1), trueValue: true, falseValue: false)
                .CheckBox("CanClose", header: "Close", width: Widths.AnsiChars(1), trueValue: true, falseValue: false)
                .CheckBox("CanUnClose", header: "UnClose", width: Widths.AnsiChars(1), trueValue: true, falseValue: false)
                .CheckBox("CanReceive", header: "UnReceive", width: Widths.AnsiChars(1), trueValue: true, falseValue: false)
                .CheckBox("CanReturn", header: "Return", width: Widths.AnsiChars(1), trueValue: true, falseValue: false)
                .CheckBox("CanJunk", header: "Junk", width: Widths.AnsiChars(1), trueValue: true, falseValue: false);

            this.listControlBindingSource1.DataSource = dtPass2;
            this.grid1.DataSource = this.listControlBindingSource1;
        }

        protected override void ClickNewAfter()
        {
            CurrentMaintain["CodePage"] = "0";
            base.ClickNewAfter();
        }

        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(CurrentMaintain["Position"]))
            {
                MyUtility.Msg.ErrorBox("< Position > can not be empty!");
                return false;
            }

            if (this.IsDetailInserting)
            {
                if (MyUtility.Check.Empty(CurrentMaintain["ID"]))
                {
                    MyUtility.Msg.ErrorBox("< ID > can not be empty!");
                    return false;
                }
                if (CurrentMaintain["ID"].ToString().Trim().Length > 9)
                {
                    MyUtility.Msg.ErrorBox("< ID > length can not be greate than 9 characters!!");
                    return false;
                }

                //bug fix:475: Tools-Password by user，存檔出現錯誤訊息
                //if (!(result = DBProxy.Current.Select(null, "SELECT acctkeywd  FROM System", out dtSystem)))
                if (!(result = DBProxy.Current.Select(null, "SELECT AccountKeyword  FROM System", out dtSystem)))
                {
                    MyUtility.Msg.ErrorBox(result.ToString());
                    return false;
                }
                CurrentMaintain["ID"] = dtSystem.Rows[0]["AccountKeyword"].ToString() + CurrentMaintain["ID"].ToString().Trim();
            }

            return base.ClickSaveBefore();
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();

            this.txtIDStart.ReadOnly = !(this.EditMode && this.IsDetailInserting);
            this.txtIDEnd.ReadOnly = !(this.EditMode && Sci.Env.User.IsAdmin);
            this.txtPassword.ReadOnly = !(this.EditMode && (Sci.Env.User.IsAdmin || this.txtIDStart.Text == Sci.Env.User.UserID));
            this.txtPassword.UseSystemPasswordChar = !this.EditMode;
            this.txtExtNo.ReadOnly = !(this.EditMode && Sci.Env.User.IsAdmin);
            this.editFactory.ReadOnly = true;  // !(this.EditMode && Sci.Env.User.IsAdmin);

            this.txtUserManager.TextBox1.Enabled = (this.EditMode && Sci.Env.User.IsAdmin);
            this.txtUserSupervisor.TextBox1.Enabled = (this.EditMode && Sci.Env.User.IsAdmin);
            this.txtUserDeputy.TextBox1.Enabled = (this.EditMode && Sci.Env.User.IsAdmin);
            this.txtEMailAddr.ReadOnly = !(this.EditMode && Sci.Env.User.IsAdmin);
            this.txtPosition.ReadOnly = true;//!(this.EditMode && Sci.Env.User.IsAdmin);
            if (this.EditMode && Sci.Env.User.IsAdmin)
            {
                this.txtPosition.ForeColor = Color.Red;
                this.txtPosition.BackColor = Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            }
            else
            {
                this.txtPosition.ForeColor = Color.Blue;
                this.txtPosition.BackColor = Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));                
            }

            this.dateDateHired.ReadOnly = !(this.EditMode && Sci.Env.User.IsAdmin);
            this.dateResign.ReadOnly = !(this.EditMode && Sci.Env.User.IsAdmin);
            this.editRemark.ReadOnly = !(this.EditMode && Sci.Env.User.IsAdmin);
            this.checkAdmin.ReadOnly = !(this.EditMode && Sci.Env.User.IsAdmin);

            sqlCmd = string.Format(@"SELECT A.*, B.MenuNo, B.BarNo 
                                                FROM Pass2 as A
                                                LEFT JOIN (SELECT Menu.MenuNo, MenuDetail.BarNo, MenuDetail.PKey
                                                                FROM Menu, MenuDetail 
                                                                WHERE Menu.PKey = MenuDetail.UKey) AS B 
                                                                ON A.FKMenu = B.PKey
                                                WHERE A.FKPASS0 = '{0}'
                                                ORDER BY MenuNo, BarNo", CurrentMaintain["FKPass0"].ToString());

            if (!(result = DBProxy.Current.Select(null, sqlCmd, out dtPass2)))
            {
                MyUtility.Msg.ErrorBox(result.ToString());
                return;
            }
            this.listControlBindingSource1.DataSource = dtPass2;
        }
    }
}
