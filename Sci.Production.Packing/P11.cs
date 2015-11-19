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

namespace Sci.Production.Packing
{
    public partial class P11 : Sci.Win.Tems.Input6
    {
        private string sqlCmd, masterID;
        private DataRow dr, dr1;
        private DialogResult buttonResult;
        private DualResult result;
        Ict.Win.DataGridViewGeneratorTextColumnSettings article = new Ict.Win.DataGridViewGeneratorTextColumnSettings();
        Ict.Win.DataGridViewGeneratorTextColumnSettings size = new Ict.Win.DataGridViewGeneratorTextColumnSettings();
        Ict.Win.DataGridViewGeneratorTextColumnSettings reason = new Ict.Win.DataGridViewGeneratorTextColumnSettings();

        public P11(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.DefaultFilter = "MDivisionID = '" + Sci.Env.User.Keyword + "'";
            detailgrid.AllowUserToOrderColumns = true;
            InsertDetailGridOnDoubleClick = false;
        }

        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            masterID = (e.Master == null) ? "" : e.Master["ID"].ToString();

            this.DetailSelectCommand = string.Format(@"select od.*,pr.Description
from OverrunGMT_Detail od
left join PackingReason pr on pr.Type = 'OG' and pr.ID = od.PackingReasonID
where od.ID = '{0}'", masterID);

            return base.OnDetailSelectCommandPrepare(e);
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();

            //帶出Orders相關欄位
            sqlCmd = string.Format("select StyleID,SeasonID from Orders where ID = '{0}'", CurrentMaintain["ID"].ToString());
            if (MyUtility.Check.Seek(sqlCmd, out dr))
            {
                displayBox1.Value = dr["StyleID"].ToString();
                displayBox2.Value = dr["SeasonID"].ToString();
            }
        }

        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();

            #region Article & SizeCode & Reason ID按右鍵與Validating
            article.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode)
                {
                    if (e.Button == System.Windows.Forms.MouseButtons.Right)
                    {
                        if (e.RowIndex != -1)
                        {
                            dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                            sqlCmd = string.Format("Select Article,SizeCode from Order_Qty where ID = '{0}'", CurrentMaintain["ID"].ToString());
                            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlCmd, "8", dr["Article"].ToString());
                            DialogResult returnResult = item.ShowDialog();
                            if (returnResult == DialogResult.Cancel) { return; }
                            IList<DataRow> ildr = item.GetSelecteds();
                            dr["Article"] = ildr[0]["Article"].ToString();
                            dr["SizeCode"] = ildr[0]["SizeCode"].ToString();
                            dr.EndEdit();
                        }
                    }
                }
            };

            article.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    if (!MyUtility.Check.Empty(e.FormattedValue) && e.FormattedValue.ToString() != dr["Article"].ToString())
                    {
                        //sql參數
                        System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter("@orderid",CurrentMaintain["ID"].ToString());
                        System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter("@article",e.FormattedValue.ToString());

                        IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                        cmds.Add(sp1);
                        cmds.Add(sp2);

                        string sqlCmd = "Select Article from Order_Qty where ID = @orderid and Article = @article";
                        DataTable OrderQtyData;
                        DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out OrderQtyData);
                        if (!result || OrderQtyData.Rows.Count <= 0)
                        {
                            if (!result)
                            {
                                MyUtility.Msg.WarningBox(string.Format("Sql connection fail!!\r\n"+result.ToString()));
                            }
                            else
                            {
                                MyUtility.Msg.WarningBox(string.Format("< Article: {0} > not found!!!", e.FormattedValue.ToString()));
                            }
                            dr["Article"] = "";
                            dr["SizeCode"] = "";
                            dr.EndEdit();
                            e.Cancel = true;
                            return;
                        }
                        else
                        {
                            dr["Article"] = e.FormattedValue.ToString();
                            dr["SizeCode"] = "";
                            dr.EndEdit();
                        }
                    }
                }
            };

            size.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode)
                {
                    if (e.Button == System.Windows.Forms.MouseButtons.Right)
                    {
                        if (e.RowIndex != -1)
                        {
                            dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                            sqlCmd = string.Format("Select distinct SizeCode from Order_Qty where ID = '{0}' and Article = '{1}'", CurrentMaintain["ID"].ToString(), dr["Article"].ToString());
                            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlCmd, "8", dr["SizeCode"].ToString());
                            DialogResult returnResult = item.ShowDialog();
                            if (returnResult == DialogResult.Cancel) { return; }
                            e.EditingControl.Text = item.GetSelectedString();
                        }
                    }
                }
            };

            size.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    if (!MyUtility.Check.Empty(e.FormattedValue) && e.FormattedValue.ToString() != dr["SizeCode"].ToString())
                    {
                        //sql參數
                        System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter("@orderid",CurrentMaintain["ID"].ToString());
                        System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter("@article",dr["Article"].ToString());
                        System.Data.SqlClient.SqlParameter sp3 = new System.Data.SqlClient.SqlParameter("@sizecode",e.FormattedValue.ToString());

                        IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                        cmds.Add(sp1);
                        cmds.Add(sp2);
                        cmds.Add(sp3);

                        string sqlCmd = "Select SizeCode from Order_Qty where ID = @orderid and Article = @article and SizeCode = @sizecode";
                        DataTable OrderQtyData;
                        DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out OrderQtyData);
                        if (!result || OrderQtyData.Rows.Count <= 0)
                        {
                            if (!result)
                            {
                                MyUtility.Msg.WarningBox(string.Format("Sql connection fail!!\r\n"+result.ToString()));
                            }
                            else
                            {
                                MyUtility.Msg.WarningBox(string.Format("< SizeCode: {0} > not found!!!", e.FormattedValue.ToString()));
                            }
                            dr["SizeCode"] = "";
                            e.Cancel = true;
                            return;
                        }
                    }
                }
            };

            reason.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode)
                {
                    if (e.Button == System.Windows.Forms.MouseButtons.Right)
                    {
                        if (e.RowIndex != -1)
                        {
                            dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem("Select ID, Description from PackingReason where Type = 'OG' and Junk = 0", "8,30", dr["PackingReasonID"].ToString());
                            DialogResult returnResult = item.ShowDialog();
                            if (returnResult == DialogResult.Cancel) { return; }
                            e.EditingControl.Text = item.GetSelectedString();
                        }
                    }
                }
            };

            reason.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    if (!MyUtility.Check.Empty(e.FormattedValue) && e.FormattedValue.ToString() != dr["PackingReasonID"].ToString())
                    {
                        if (!MyUtility.Check.Seek(string.Format("Select ID, Description from PackingReason where Type = 'OG' and Junk = 0 and ID = '{0}'", e.FormattedValue.ToString()), out dr1))
                        {
                            MyUtility.Msg.WarningBox(string.Format("< Reason ID: {0} > not found!!!", e.FormattedValue.ToString()));
                            dr["PackingReasonID"] = "";
                            e.Cancel = true;
                            return;
                        }
                        else
                        {
                            dr["PackingReasonID"] = e.FormattedValue.ToString();
                            dr["Description"] = dr1["Description"].ToString();
                            dr.EndEdit();
                        }
                    }
                }
            };
            #endregion

            Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("Article", header: "Color way", width: Widths.AnsiChars(8), settings: article)
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(8), settings: size)
                .Numeric("Qty", header: "Q'ty")
                .Text("PackingReasonID", header: "Reason ID", width: Widths.AnsiChars(5), settings: reason)
                .Text("Description", header: "Reason Description", width: Widths.AnsiChars(40), iseditingreadonly: true);
        }

        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["MDivisionID"] = Sci.Env.User.Keyword;
            CurrentMaintain["Status"] = "New";
            CurrentMaintain["CloseDate"] = DateTime.Today;
        }

        protected override bool ClickEditBefore()
        {
            if (CurrentMaintain["Status"].ToString() == "Confirmed")
            {
                MyUtility.Msg.WarningBox("This record is < Confirmed >, can't be modified!");
                return false;
            }
            return base.ClickEditBefore();
        }

        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.textBox1.ReadOnly = true;
        }

        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(CurrentMaintain["ID"]))
            {
                MyUtility.Msg.WarningBox("SP# can't empty!!");
                textBox1.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["CloseDate"]))
            {
                MyUtility.Msg.WarningBox("Date can't empty!!");
                dateBox1.Focus();
                return false;
            }

            int count = 0;
            foreach (DataRow detail in DetailDatas)
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
                MyUtility.Msg.WarningBox("< Detail > can't be empty!");
                detailgrid.Focus();
                return false;
            }
            return base.ClickSaveBefore();
        }

        protected override bool ClickDeleteBefore()
        {
            if (CurrentMaintain["Status"].ToString() == "Confirmed")
            {
                MyUtility.Msg.WarningBox("This record is < Confirmed >, can't be deleted!");
                return false;
            }

            return base.ClickDeleteBefore();
        }

        //檢查輸入的SP#是否正確
        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            if (EditMode)
            {
                if (textBox1.Text != textBox1.OldValue)
                {
                    if (!MyUtility.Check.Empty(textBox1.Text))
                    {
                        //sql參數
                        System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter("@id",textBox1.Text);
                        System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter("@mdivisionid", Sci.Env.User.Keyword);

                        IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                        cmds.Add(sp1);
                        cmds.Add(sp2);

                        string sqlCmd = "select ID, StyleID, SeasonID, FtyGroup from Orders where ID = @id and MDivisionID = @mdivisionid and GMTClose is not null";
                        DataTable OrdersData;
                        DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out OrdersData);
                        if (!result || OrdersData.Rows.Count <= 0)
                        {
                            if (!result)
                            {
                                MyUtility.Msg.WarningBox("Sql connection fail!\r\n" + result.ToString());
                            }
                            else
                            {
                                MyUtility.Msg.WarningBox("SP# not found!");
                            }
                            //OrderID異動，其他相關欄位要跟著異動
                            textBox1.Text = "";
                            displayBox1.Value = "";
                            displayBox2.Value = "";
                            CurrentMaintain["FactoryID"] = "";
                            e.Cancel = true;
                            return;
                        }
                        else
                        {
                            //OrderID異動，其他相關欄位要跟著異動
                            displayBox1.Value = OrdersData.Rows[0]["StyleID"].ToString();
                            displayBox2.Value = OrdersData.Rows[0]["SeasonID"].ToString();
                            CurrentMaintain["ID"] = textBox1.Text;
                            CurrentMaintain["FactoryID"] = OrdersData.Rows[0]["FtyGroup"].ToString();
                        }
                    }
                }
            }
        }

        //Confirm
        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            sqlCmd = string.Format("update OverrunGMT set Status = 'Confirmed', EditName = '{0}', EditDate = GETDATE() where ID = '{1}'", Sci.Env.User.UserID, CurrentMaintain["ID"].ToString());

            result = DBProxy.Current.Execute(null, sqlCmd);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Confirm failed, Pleaes re-try");
            }

            RenewData();
            OnDetailEntered();
            EnsureToolbarExt();
        }

        //UnConfirm
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();

            //問是否要做Unconfirm，確定才繼續往下做
            buttonResult = MyUtility.Msg.WarningBox("Are you sure you want to < Unconfirm > this data?", "Warning", MessageBoxButtons.YesNo);
            if (buttonResult == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            sqlCmd = string.Format("update OverrunGMT set Status = 'New', EditName = '{0}', EditDate = GETDATE() where ID = '{1}'", Sci.Env.User.UserID, CurrentMaintain["ID"].ToString());

            result = DBProxy.Current.Execute(null, sqlCmd);
            if (!result)
            {
                MyUtility.Msg.WarningBox("UnConfirm failed, Pleaes re-try");
            }

            RenewData();
            OnDetailEntered();
            EnsureToolbarExt();
        }
    }
}
