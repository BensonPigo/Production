using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;

namespace Sci.Production.Packing
{
    /// <summary>
    /// Packing_P11
    /// </summary>
    public partial class P11 : Win.Tems.Input6
    {
        private string sqlCmd;
        private string masterID;
        private DataRow dr;
        private DataRow dr1;
        private DialogResult buttonResult;
        private DualResult result;
        private DataGridViewGeneratorTextColumnSettings article = new DataGridViewGeneratorTextColumnSettings();
        private DataGridViewGeneratorTextColumnSettings size = new DataGridViewGeneratorTextColumnSettings();
        private DataGridViewGeneratorTextColumnSettings reason = new DataGridViewGeneratorTextColumnSettings();

        /// <summary>
        /// P11
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P11(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = "MDivisionID = '" + Sci.Env.User.Keyword + "'";
            this.detailgrid.AllowUserToOrderColumns = true;
            this.InsertDetailGridOnDoubleClick = false;
        }

        /// <summary>
        /// OnDetailSelectCommandPrepare
        /// </summary>
        /// <param name="e">e</param>
        /// <returns>DualResult</returns>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            this.masterID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();

            this.DetailSelectCommand = string.Format(
                @"select od.*,pr.Description
from OverrunGMT_Detail od WITH (NOLOCK) 
left join PackingReason pr WITH (NOLOCK) on pr.Type = 'OG' and pr.ID = od.PackingReasonID
where od.ID = '{0}'", this.masterID);

            return base.OnDetailSelectCommandPrepare(e);
        }

        /// <summary>
        /// OnDetailEntered
        /// </summary>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();

            this.labelConfirmed.Visible = MyUtility.Check.Empty(this.CurrentMaintain["ID"]) ? false : true;

            // 帶出Orders相關欄位
            this.sqlCmd = string.Format("select StyleID,SeasonID from Orders WITH (NOLOCK) where ID = '{0}'", this.CurrentMaintain["ID"].ToString());
            if (MyUtility.Check.Seek(this.sqlCmd, out this.dr))
            {
                this.displayStyle.Value = this.dr["StyleID"].ToString();
                this.displaySeason.Value = this.dr["SeasonID"].ToString();
            }
            else
            {
                this.displayStyle.Text = string.Empty;
                this.displaySeason.Text = string.Empty;
            }

            DataRow dr1;
            string sqlStatus = string.Format(@"select status from OverrunGMT WITH (NOLOCK) where id='{0}'", this.CurrentMaintain["ID"].ToString());
            if (MyUtility.Check.Seek(sqlStatus, out dr1))
            {
                this.labelConfirmed.Text = dr1["Status"].ToString();
            }
        }

        /// <summary>
        /// OnDetailGridSetup
        /// </summary>
        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();

            #region Article & SizeCode & Reason ID按右鍵與Validating
            this.article.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode)
                {
                    if (e.Button == System.Windows.Forms.MouseButtons.Right)
                    {
                        if (e.RowIndex != -1)
                        {
                            this.dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                            this.sqlCmd = string.Format("Select Article,SizeCode from Order_Qty WITH (NOLOCK) where ID = '{0}'", this.CurrentMaintain["ID"].ToString());
                            Win.Tools.SelectItem item = new Win.Tools.SelectItem(this.sqlCmd, "8", this.dr["Article"].ToString());
                            DialogResult returnResult = item.ShowDialog();
                            if (returnResult == DialogResult.Cancel)
                            {
                                return;
                            }

                            IList<DataRow> ildr = item.GetSelecteds();
                            this.dr["Article"] = ildr[0]["Article"].ToString();
                            this.dr["SizeCode"] = ildr[0]["SizeCode"].ToString();
                            this.dr.EndEdit();
                        }
                    }
                }
            };

            this.article.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    this.dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    if (!MyUtility.Check.Empty(e.FormattedValue) && e.FormattedValue.ToString() != this.dr["Article"].ToString())
                    {
                        // sql參數
                        System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter("@orderid", this.CurrentMaintain["ID"].ToString());
                        System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter("@article", e.FormattedValue.ToString());

                        IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                        cmds.Add(sp1);
                        cmds.Add(sp2);

                        string sqlCmd = "Select Article from Order_Qty WITH (NOLOCK) where ID = @orderid and Article = @article";
                        DataTable orderQtyData;
                        DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out orderQtyData);
                        if (!result || orderQtyData.Rows.Count <= 0)
                        {
                            if (!result)
                            {
                                MyUtility.Msg.WarningBox(string.Format("Sql connection fail!!\r\n" + result.ToString()));
                            }
                            else
                            {
                                MyUtility.Msg.WarningBox(string.Format("< Article: {0} > not found!!!", e.FormattedValue.ToString()));
                            }

                            this.dr["Article"] = string.Empty;
                            this.dr["SizeCode"] = string.Empty;
                            this.dr.EndEdit();
                            e.Cancel = true;
                            return;
                        }
                        else
                        {
                            this.dr["Article"] = e.FormattedValue.ToString();
                            this.dr["SizeCode"] = string.Empty;
                            this.dr.EndEdit();
                        }
                    }
                }
            };

            this.size.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode)
                {
                    if (e.Button == System.Windows.Forms.MouseButtons.Right)
                    {
                        if (e.RowIndex != -1)
                        {
                            this.dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                            this.sqlCmd = string.Format("Select distinct SizeCode from Order_Qty WITH (NOLOCK) where ID = '{0}' and Article = '{1}'", this.CurrentMaintain["ID"].ToString(), this.dr["Article"].ToString());
                            Win.Tools.SelectItem item = new Win.Tools.SelectItem(this.sqlCmd, "8", this.dr["SizeCode"].ToString());
                            DialogResult returnResult = item.ShowDialog();
                            if (returnResult == DialogResult.Cancel)
                            {
                                return;
                            }

                            e.EditingControl.Text = item.GetSelectedString();
                        }
                    }
                }
            };

            this.size.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    this.dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    if (!MyUtility.Check.Empty(e.FormattedValue) && e.FormattedValue.ToString() != this.dr["SizeCode"].ToString())
                    {
                        // sql參數
                        System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter("@orderid", this.CurrentMaintain["ID"].ToString());
                        System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter("@article", this.dr["Article"].ToString());
                        System.Data.SqlClient.SqlParameter sp3 = new System.Data.SqlClient.SqlParameter("@sizecode", e.FormattedValue.ToString());

                        IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                        cmds.Add(sp1);
                        cmds.Add(sp2);
                        cmds.Add(sp3);

                        string sqlCmd = "Select SizeCode from Order_Qty WITH (NOLOCK) where ID = @orderid and Article = @article and SizeCode = @sizecode";
                        DataTable orderQtyData1;
                        DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out orderQtyData1);
                        if (!result || orderQtyData1.Rows.Count <= 0)
                        {
                            if (!result)
                            {
                                MyUtility.Msg.WarningBox(string.Format("Sql connection fail!!\r\n" + result.ToString()));
                            }
                            else
                            {
                                MyUtility.Msg.WarningBox(string.Format("< SizeCode: {0} > not found!!!", e.FormattedValue.ToString()));
                            }

                            this.dr["SizeCode"] = string.Empty;
                            e.Cancel = true;
                            return;
                        }
                    }
                }
            };

            this.reason.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode)
                {
                    if (e.Button == System.Windows.Forms.MouseButtons.Right)
                    {
                        if (e.RowIndex != -1)
                        {
                            this.dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                            Win.Tools.SelectItem item = new Win.Tools.SelectItem("Select ID, Description from PackingReason WITH (NOLOCK) where Type = 'OG' and Junk = 0", "8,30", this.dr["PackingReasonID"].ToString());
                            DialogResult returnResult = item.ShowDialog();
                            if (returnResult == DialogResult.Cancel)
                            {
                                return;
                            }

                            e.EditingControl.Text = item.GetSelectedString();
                        }
                    }
                }
            };

            this.reason.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    this.dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    if (!MyUtility.Check.Empty(e.FormattedValue) && e.FormattedValue.ToString() != this.dr["PackingReasonID"].ToString())
                    {
                        if (!MyUtility.Check.Seek(string.Format("Select ID, Description from PackingReason WITH (NOLOCK) where Type = 'OG' and Junk = 0 and ID = '{0}'", e.FormattedValue.ToString()), out this.dr1))
                        {
                            this.dr["PackingReasonID"] = string.Empty;
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox(string.Format("< Reason ID: {0} > not found!!!", e.FormattedValue.ToString()));
                            return;
                        }
                        else
                        {
                            this.dr["PackingReasonID"] = e.FormattedValue.ToString();
                            this.dr["Description"] = this.dr1["Description"].ToString();
                            this.dr.EndEdit();
                        }
                    }
                }
            };
            #endregion

            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("Article", header: "Color way", width: Widths.AnsiChars(8), settings: this.article)
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(8), settings: this.size)
                .Numeric("Qty", header: "Q'ty")
                .Text("PackingReasonID", header: "Reason ID", width: Widths.AnsiChars(5), settings: this.reason)
                .Text("Description", header: "Reason Description", width: Widths.AnsiChars(40), iseditingreadonly: true);
        }

        /// <summary>
        /// ClickNewAfter
        /// </summary>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["MDivisionID"] = Sci.Env.User.Keyword;
            this.CurrentMaintain["Status"] = "New";
            this.CurrentMaintain["CloseDate"] = DateTime.Today;
        }

        /// <summary>
        /// ClickEditBefore
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ClickEditBefore()
        {
            if (this.CurrentMaintain["Status"].ToString() == "Confirmed")
            {
                MyUtility.Msg.WarningBox("This record is < Confirmed >, can't be modified!");
                return false;
            }

            return base.ClickEditBefore();
        }

        /// <summary>
        /// ClickEditAfter
        /// </summary>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtSP.ReadOnly = true;
        }

        /// <summary>
        /// ClickSaveBefore
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["ID"]))
            {
                this.txtSP.Focus();
                MyUtility.Msg.WarningBox("SP# can't empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["CloseDate"]))
            {
                this.dateDate.Focus();
                MyUtility.Msg.WarningBox("Date can't empty!!");
                return false;
            }

            int count = 0;
            foreach (DataRow detail in this.DetailDatas)
            {
                if (MyUtility.Check.Empty(detail["Article"]) || MyUtility.Check.Empty(detail["SizeCode"]) || MyUtility.Check.Empty(detail["Qty"]))
                {
                    detail.Delete();
                    continue;
                }

                if (MyUtility.Check.Empty(detail["PackingReasonID"]))
                {
                    MyUtility.Msg.WarningBox("Reason ID can't be empty!");
                    return false;
                }

                count = count + 1;
            }

            if (count == 0)
            {
                this.detailgrid.Focus();
                MyUtility.Msg.WarningBox("< Detail > can't be empty!");
                return false;
            }

            return base.ClickSaveBefore();
        }

        /// <summary>
        /// ClickDeleteBefore
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ClickDeleteBefore()
        {
            if (this.CurrentMaintain["Status"].ToString() == "Confirmed")
            {
                MyUtility.Msg.WarningBox("This record is < Confirmed >, can't be deleted!");
                return false;
            }

            return base.ClickDeleteBefore();
        }

        // 檢查輸入的SP#是否正確
        private void TxtSP_Validating(object sender, CancelEventArgs e)
        {
            if (this.EditMode)
            {
                if (this.txtSP.Text != this.txtSP.OldValue)
                {
                    if (!MyUtility.Check.Empty(this.txtSP.Text))
                    {
                        // sql參數
                        System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter("@id", this.txtSP.Text);
                        System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter("@mdivisionid", Sci.Env.User.Keyword);

                        IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                        cmds.Add(sp1);
                        cmds.Add(sp2);

                        string sqlCmd = "select ID, StyleID, SeasonID, FtyGroup from Orders WITH (NOLOCK) where ID = @id and MDivisionID = @mdivisionid and GMTClose is not null";
                        DataTable ordersData;
                        DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out ordersData);
                        if (!result || ordersData.Rows.Count <= 0)
                        {
                            if (!result)
                            {
                                MyUtility.Msg.WarningBox("Sql connection fail!\r\n" + result.ToString());
                            }
                            else
                            {
                                MyUtility.Msg.WarningBox("SP# not found!");
                            }

                            // OrderID異動，其他相關欄位要跟著異動
                            this.txtSP.Text = string.Empty;
                            this.displayStyle.Value = string.Empty;
                            this.displaySeason.Value = string.Empty;
                            this.CurrentMaintain["FactoryID"] = string.Empty;
                            e.Cancel = true;
                            return;
                        }
                        else
                        {
                            // OrderID異動，其他相關欄位要跟著異動
                            this.displayStyle.Value = ordersData.Rows[0]["StyleID"].ToString();
                            this.displaySeason.Value = ordersData.Rows[0]["SeasonID"].ToString();
                            this.CurrentMaintain["ID"] = this.txtSP.Text;
                            this.CurrentMaintain["FactoryID"] = ordersData.Rows[0]["FtyGroup"].ToString();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// ClickConfirm
        /// </summary>
        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            this.sqlCmd = string.Format("update OverrunGMT set Status = 'Confirmed', EditName = '{0}', EditDate = GETDATE() where ID = '{1}'", Sci.Env.User.UserID, this.CurrentMaintain["ID"].ToString());

            this.result = DBProxy.Current.Execute(null, this.sqlCmd);
            if (!this.result)
            {
                MyUtility.Msg.WarningBox("Confirm failed, Please re-try");
            }
        }

        /// <summary>
        /// ClickUnconfirm
        /// </summary>
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();

            // 問是否要做Unconfirm，確定才繼續往下做
            this.buttonResult = MyUtility.Msg.WarningBox("Are you sure you want to < Unconfirm > this data?", "Warning", MessageBoxButtons.YesNo);
            if (this.buttonResult == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            this.sqlCmd = string.Format("update OverrunGMT set Status = 'New', EditName = '{0}', EditDate = GETDATE() where ID = '{1}'", Sci.Env.User.UserID, this.CurrentMaintain["ID"].ToString());

            this.result = DBProxy.Current.Execute(null, this.sqlCmd);
            if (!this.result)
            {
                MyUtility.Msg.WarningBox("UnConfirm failed, Please re-try");
            }
        }
    }
}
