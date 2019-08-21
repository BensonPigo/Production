﻿using System;
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
using System.Text.RegularExpressions;
using System.IO;
using Sci.Production.Class;

namespace Sci.Production.Tools
{
    public partial class PasswordByUser : Sci.Win.Tems.Input1
    {
        OpenFileDialog openfiledialog;
        private string file;
        private Sci.Production.Class.PictureSubPage picturePage;
        private DataTable dtPass2 = null;
        private DataTable dtSystem = null;
        private DataTable dtFactory = null;
        private DualResult result = null;
        private string sqlCmd = "";
        private string destination_path;// 放圖檔的路徑

        public PasswordByUser(ToolStripMenuItem menuitem) : base(menuitem)
        {
            InitializeComponent();

            // MyApp.lAdmin = fasle時,
            // 1.則DefaultFilter設為 ID = MyApp.cLogin
            // 2.關閉New/Dele/Move功能
            if (!Sci.Env.User.IsAdmin)
            {
                this.DefaultFilter = $@"ID = '{Sci.Env.User.UserID}' and ISMIS = 0";
                this.IsSupportNew = false;
                this.IsSupportDelete = false;
            } 
            else
            {
                this.DefaultFilter = "ISMIS = 0";
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

            destination_path = Sci.Production.Class.UserESignature.getESignaturePath();
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
                .CheckBox("CanJunk", header: "Junk", width: Widths.AnsiChars(1), trueValue: true, falseValue: false)
                .CheckBox("CanUnJunk", header: "UnJunk", width: Widths.AnsiChars(1), trueValue: true, falseValue: false);

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

            /*
             * 進行電子簽章的存取
             * 必須確認有明確的路徑
             * 否則系統會將圖檔放置執行檔的目錄
             */
            if (this.destination_path != null)
            {
                if (MyUtility.Check.Empty(this.disBoxESignature.Text))
                {
                    // 刪除pic
                    if (System.IO.File.Exists(this.destination_path + MyUtility.Convert.GetString(this.CurrentMaintain["ESignature"])))
                    {
                        try
                        {
                            string deltpath = this.destination_path + MyUtility.Convert.GetString(this.CurrentMaintain["ESignature"]);
                            System.IO.File.Delete(deltpath);
                            this.CurrentMaintain["ESignature"] = string.Empty;
                            this.disBoxESignature.Text = MyUtility.Convert.GetString(this.CurrentMaintain["ESignature"]);
                        }
                        catch (System.IO.IOException exception)
                        {
                            MyUtility.Msg.ErrorBox("Error: Delete file fail. Original error: " + exception.Message);
                        }
                    }
                    else
                    {
                        this.CurrentMaintain["ESignature"] = string.Empty;
                    }
                }
                else
                {
                    if (System.IO.File.Exists(this.destination_path + MyUtility.Convert.GetString(this.CurrentMaintain["ESignature"])) || !MyUtility.Check.Empty(file))
                    {
                        try
                        {
                            string pathFileName = this.destination_path + this.CurrentMaintain["ID"] + Path.GetExtension(this.file);
                            if (!MyUtility.Check.Empty(file))
                            {
                                File.Copy(file, pathFileName, true);
                            }
                        }
                        catch (Exception exception)
                        {
                            MyUtility.Msg.ErrorBox("Error: Save file fail. Original error: " + exception.Message);
                        }
                    }
                }
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

        private void btnSetPic_Click(object sender, EventArgs e)
        {
            if (this.destination_path != null)
            {
                if (openfiledialog == null)
                {
                    openfiledialog = new OpenFileDialog();
                    openfiledialog.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.TIF)|*.BMP;*.JPG;*.GIF|All files (*.*)|*.*";
                }

                if (DialogResult.OK != openfiledialog.ShowDialog()) return;
                file = openfiledialog.FileName;
                this.CurrentMaintain["ESignature"] = CurrentMaintain["ID"] + Path.GetExtension(file);
            }
        }

        private void btnShowImg_Click(object sender, EventArgs e)
        {
            switch (EditMode)
            {
                //  非編輯模式,只能Show照片
                case false:                    
                    Image img = UserESignature.getUserESignature(CurrentMaintain["id"].ToString());
                    if (string.IsNullOrEmpty(this.disBoxESignature.Text)) return;
                    picturePage = new PictureSubPage(img);                  
                    picturePage.ShowDialog(this);
                    break;
                // 編輯模式,只能Clear照片
                case true:
                    if (this.destination_path != null)
                    {
                        DialogResult delResult = MyUtility.Msg.QuestionBox("Clear the E- Signature?", buttons: MessageBoxButtons.YesNo);
                        if (delResult == System.Windows.Forms.DialogResult.Yes)
                        {
                            // 暫時清空pic
                            this.disBoxESignature.Text = string.Empty;
                        }
                    }
                    break;
            }
        }

        protected override void OnEditModeChanged()
        {
            if (MyUtility.Check.Empty(this.btnShowImg))
            {
                return;
            }

            switch (EditMode)
            {
                case true:
                    this.btnShowImg.Text = "Clear";
                    break;
                case false:
                    this.btnShowImg.Text = "Show";
                    break;
            }

            base.OnEditModeChanged();
        }

        private void txtEMailAddr_Validating(object sender, CancelEventArgs e)
        {
            //20190610先不要驗證, 太久了
            //if (!this.EditMode || MyUtility.Check.Empty(this.txtEMailAddr.Text))
            //{
            //    return;
            //}
            //if (!PublicPrg.Prgs.TestMail(this.txtEMailAddr.Text))
            //{
            //    e.Cancel = true;
            //    return;
            //}
        }
    }
}
