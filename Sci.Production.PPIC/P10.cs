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
using Sci.Production.PublicPrg;

namespace Sci.Production.PPIC
{
    public partial class P10 : Sci.Win.Tems.Input6
    {

        public P10(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            DefaultFilter = "FactoryID = '" + Sci.Env.User.Factory + "' and FabricType = 'F'";
            txtuser2.TextBox1.ReadOnly = true;
            txtuser2.TextBox1.IsSupportEditMode = false;
        }

        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["ID"].ToString();
            this.DetailSelectCommand = string.Format(@"select ld.*,(left(ld.Seq1+' ',3)+ld.Seq2) as Seq,isnull(psd.Refno,'') as Refno,isnull(f.Description,'') as Description,
(select max(i.IssueDate) from Issue i, Issue_Detail id where i.Id = id.Id and id.PoId = l.POID and id.Seq1 = ld.Seq1 and id.Seq2 = ld.seq2) as IssueDate,
isnull(psd.InQty,0) as InQty,isnull(psd.OutQty,0) as OutQty,isnull(p.Description,'') as PPICReasonDesc
from Lack l
inner join Lack_Detail ld on l.ID = ld.ID
left join PO_Supp_Detail psd on psd.ID = l.POID and psd.SEQ1 = ld.Seq1 and psd.SEQ2 = ld.Seq2
left join Fabric f on psd.SCIRefno = f.SCIRefno
left join PPICReason p on p.Type = 'FL' and ld.PPICReasonID = p.ID
where l.ID = '{0}'
order by ld.Seq1,ld.Seq2", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            MyUtility.Tool.SetupCombox(comboBox1, 2, 1, "L,Lacking,R,Replacement");
            MyUtility.Tool.SetupCombox(comboBox2, 2, 1, "D,Day,N,Night,O,Subcon-Out");
        }

        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();
            DataGridViewGeneratorTextColumnSettings seq = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings refno = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings reason = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorNumericColumnSettings inqty = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings outqty = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings requestqty = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings issueqty = new DataGridViewGeneratorNumericColumnSettings();
            Dictionary<string, string> processSource = new Dictionary<string, string>() { { "Automation", "Automation" }, { "Bonding", "Bonding" }, { "Cutting", "Cutting" }, { "Embroidery", "Embroidery" }, { "Heat transfer", "Heat transfer" }, { "Printing", "Printing" }, { "Sewing", "Sewing" } };
            DataGridViewGeneratorComboBoxColumnSettings process = new DataGridViewGeneratorComboBoxColumnSettings() { DataSource = new System.Windows.Forms.BindingSource(processSource, null), ValueMember = "Value", DisplayMember = "Value" }; ;

            inqty.CellZeroStyle = Ict.Win.UI.DataGridViewNumericBoxZeroStyle.Empty;
            outqty.CellZeroStyle = Ict.Win.UI.DataGridViewNumericBoxZeroStyle.Empty;
            requestqty.CellZeroStyle = Ict.Win.UI.DataGridViewNumericBoxZeroStyle.Empty;
            issueqty.CellZeroStyle = Ict.Win.UI.DataGridViewNumericBoxZeroStyle.Empty;
            #region Seq按右鍵與Validating
            seq.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode)
                {
                    if (e.Button == System.Windows.Forms.MouseButtons.Right)
                    {
                        if (e.RowIndex != -1)
                        {
                            DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                            Sci.Win.Tools.SelectItem item = Prgs.SelePoItem(CurrentMaintain["POID"].ToString(), dr["Seq"].ToString(), "FabricType = 'F'");
                            DialogResult result = item.ShowDialog();
                            if (result == DialogResult.Cancel) { return; }
                            IList<DataRow> selectData = item.GetSelecteds();
                            dr["Seq"] = item.GetSelectedString();
                            dr["Seq1"] = selectData[0]["Seq1"].ToString();
                            dr["Seq2"] = selectData[0]["Seq2"].ToString();
                            dr["RefNo"] = selectData[0]["RefNo"].ToString();
                            dr["Description"] = selectData[0]["Description"].ToString();
                            dr["InQty"] = Convert.ToDecimal(selectData[0]["InQty"]);
                            dr["OutQty"] = Convert.ToDecimal(selectData[0]["OutQty"]);
                            DateTime? maxIssueDate = MaxIssueDate(selectData[0]["Seq1"].ToString(), selectData[0]["Seq2"].ToString());
                            if (MyUtility.Check.Empty(maxIssueDate))
                            {
                                dr["IssueDate"] = DBNull.Value;
                            }
                            else
                            {
                                dr["IssueDate"] = maxIssueDate;
                            }
                            dr.EndEdit();
                        }
                    }
                }
            };

            seq.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);

                    if (!MyUtility.Check.Empty(e.FormattedValue) && e.FormattedValue.ToString() != dr["Seq"].ToString())
                    {
                        DataRow poData;
                        string sqlCmd = string.Format(@"select left(seq1+' ',3)+seq2 as Seq, Refno,InQty,OutQty,seq1,seq2, 
dbo.getmtldesc(id,seq1,seq2,2,0) as Description 
from dbo.PO_Supp_Detail
where id ='{0}' and seq1 = '{1}' and seq2 = '{2}' and FabricType = 'F'", CurrentMaintain["POID"].ToString(), e.FormattedValue.ToString().Substring(0, 3), e.FormattedValue.ToString().Substring(2, 2));
                        if (MyUtility.Check.Empty(CurrentMaintain["POID"]) || !MyUtility.Check.Seek(sqlCmd, out poData))
                        {
                            if (MyUtility.Check.Empty(CurrentMaintain["POID"]))
                            {
                                MyUtility.Msg.WarningBox("SP# can't empty!!");
                            }
                            else
                            {
                                MyUtility.Msg.WarningBox(string.Format("< Seq: {0} > not found!!!", e.FormattedValue.ToString()));
                            }
                            dr["Seq"] = "";
                            dr["Seq1"] = "";
                            dr["Seq2"] = "";
                            dr["RefNo"] = "";
                            dr["Description"] = "";
                            dr["InQty"] = 0;
                            dr["OutQty"] = 0;
                            dr["IssueDate"] = null;
                            dr.EndEdit();
                            e.Cancel = true;
                            return;
                        }
                        else
                        {
                            dr["Seq"] = poData["Seq"].ToString();
                            dr["Seq1"] = poData["Seq1"].ToString();
                            dr["Seq2"] = poData["Seq2"].ToString();
                            dr["RefNo"] = poData["RefNo"].ToString();
                            dr["Description"] = poData["Description"].ToString();
                            dr["InQty"] = Convert.ToDecimal(poData["InQty"]);
                            dr["OutQty"] = Convert.ToDecimal(poData["OutQty"]);
                            DateTime? maxIssueDate = MaxIssueDate(poData["Seq1"].ToString(), poData["Seq2"].ToString());
                            if (MyUtility.Check.Empty(maxIssueDate))
                            {
                                dr["IssueDate"] = DBNull.Value;
                            }
                            else
                            {
                                dr["IssueDate"] = maxIssueDate;
                            }
                            dr.EndEdit();
                        }
                        
                    }
                }
            };
            #endregion

            #region RefNo的CoubleClick
            refno.EditingMouseDoubleClick += (s, e) =>
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    if (e.RowIndex != -1)
                    {
                        DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                        Sci.Win.Tools.EditMemo callNextForm = new Sci.Win.Tools.EditMemo(dr["Description"].ToString(), "Description", false, null);
                        callNextForm.ShowDialog(this);

                    }
                }
            };
            #endregion

            #region PPICReasonID按右鍵與Validating
            reason.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode)
                {
                    if (e.Button == System.Windows.Forms.MouseButtons.Right)
                    {
                        if (e.RowIndex != -1)
                        {
                            DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(string.Format("select ID,Description from PPICReason where Type = 'FL' and Junk = 0 and TypeForUse = '{0}'", CurrentMaintain["Type"].ToString()), "5,40", dr["PPICReasonID"].ToString());
                            DialogResult returnResult = item.ShowDialog();
                            if (returnResult == DialogResult.Cancel) { return; }
                            IList<DataRow> selectData = item.GetSelecteds();
                            dr["PPICReasonID"] = item.GetSelectedString();
                            dr["PPICReasonDesc"] = selectData[0]["Description"].ToString();
                            dr.EndEdit();
                        }
                    }
                }
            };

            reason.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);

                    if (!MyUtility.Check.Empty(e.FormattedValue) && e.FormattedValue.ToString() != dr["Seq"].ToString())
                    {
                        DataRow reasonData;
                        string sqlCmd = string.Format(@"select ID,Description from PPICReason where Type = 'FL' and Junk = 0 and TypeForUse = '{0}' and ID = '{1}'", CurrentMaintain["Type"].ToString(), e.FormattedValue.ToString());
                        if (!MyUtility.Check.Seek(sqlCmd, out reasonData))
                        {
                            MyUtility.Msg.WarningBox(string.Format("< Reason Id: {0} > not found!!!", e.FormattedValue.ToString()));
                            dr["PPICReasonID"] = "";
                            dr["PPICReasonDesc"] = "";
                            dr.EndEdit();
                            e.Cancel = true;
                            return;
                        }
                        else
                        {
                            dr["PPICReasonID"] = e.FormattedValue.ToString();
                            dr["PPICReasonDesc"] = reasonData["Description"].ToString();
                            dr.EndEdit();
                        }
                    }
                }
            };
            #endregion

            Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("Seq", header: "Seq#", width: Widths.AnsiChars(5), settings: seq)
                .Text("RefNo", header: "Refer#", width: Widths.AnsiChars(15), iseditingreadonly: true, settings: refno)
                .Date("IssueDate", header: "Prod. Last Rcvd Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("InQty", header: "Prod. Accu. Rcvd Qty", width: Widths.AnsiChars(10), decimal_places: 2, iseditingreadonly: true, settings: inqty)
                .Numeric("OutQty", header: "WH Accu. Rcvd Qty", width: Widths.AnsiChars(10), decimal_places: 2, iseditingreadonly: true, settings: outqty)
                .Numeric("RequestQty", header: "Request Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2, maximum: 99999999.99M, minimum: 0, settings: requestqty)
                .Numeric("RejectQty", header: "# of pcs rejected")
                .Numeric("IssueQty", header: "Issue Qty upon request", decimal_places: 2, iseditingreadonly: true, settings: issueqty)
                .ComboBox("Process", header: "Process", width: Widths.AnsiChars(15), settings: process)
                .Text("PPICReasonID", header: "Reason Id", width: Widths.AnsiChars(5), settings: reason)
                .EditText("PPICReasonDesc", header: "Reason", width: Widths.AnsiChars(20), iseditingreadonly: true);
        }

        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["IssueDate"] = DateTime.Today;
            CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;
            CurrentMaintain["FabricType"] = "F";
            CurrentMaintain["ApplyName"] = Sci.Env.User.UserID;
            CurrentMaintain["Status"] = "New";
        }

        protected override bool ClickEditBefore()
        {
            if (CurrentMaintain["Status"].ToString() != "New")
            {
                MyUtility.Msg.WarningBox("This record was approved, can't modify.");
                return false;
            }
            return true;
        }

        protected override bool ClickDeleteBefore()
        {
            if (CurrentMaintain["Status"].ToString() != "New")
            {
                MyUtility.Msg.WarningBox("This record was approved, can't delete.");
                return false;
            }
            return true;
        }

        protected override bool ClickSaveBefore()
        {
            #region 檢查必輸欄位
            if (MyUtility.Check.Empty(CurrentMaintain["Type"]))
            {
                MyUtility.Msg.WarningBox("Type can't empty");
                comboBox1.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["Shift"]))
            {
                MyUtility.Msg.WarningBox("Shift can't empty");
                comboBox2.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["OrderID"]))
            {
                MyUtility.Msg.WarningBox("SP# can't empty");
                textBox2.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["SewingLineID"]))
            {
                MyUtility.Msg.WarningBox("Sewing Line can't empty");
                txtsewingline1.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["ApplyName"]))
            {
                MyUtility.Msg.WarningBox("Handle can't empty");
                txtuser1.TextBox1.Focus();
                return false;
            }
            #endregion
            int i = 0; //計算表身Grid的總筆數
            foreach (DataRow dr in DetailDatas)
            {
                #region 刪除表身Seq為空白的資料
                if (MyUtility.Check.Empty(dr["Seq"]))
                {
                    dr.Delete();
                    continue;
                }
                #endregion
                i++;
                #region 表身的RequestQty不可小於0、Reason不可為空 、Type='R'時RejectQty不可小(等)於0
                if (MyUtility.Check.Empty(dr["RequestQty"]) || Convert.ToDecimal(dr["RequestQty"]) <= 0)
                {
                    MyUtility.Msg.WarningBox("< Request Qty >  can't equal or less 0!");
                    return false;
                }

                if (MyUtility.Check.Empty(dr["PPICReasonID"]))
                {
                    MyUtility.Msg.WarningBox("< Reason Id >  can't empty!");
                    return false;
                }

                if (CurrentMaintain["Type"].ToString() == "R" && (MyUtility.Check.Empty(dr["RejectQty"]) || Convert.ToInt32(dr["RejectQty"]) <= 0))
                {
                    MyUtility.Msg.WarningBox("< # of pcs rejected >  can't equal or less 0!");
                    return false;
                }

                #endregion
            }

            //表身Grid資料不可為空
            if (i == 0)
            {
                MyUtility.Msg.WarningBox("Detail can't empty!!");
                return false;
            }

            //RequestQty不可以超過Warehouse的A倉庫存數量
            DataTable ExceedData;
            try
            {
                MyUtility.Tool.ProcessWithDatatable((DataTable)detailgridbs.DataSource, "Seq,Seq1,Seq2,RequestQty", string.Format(@"select * from (
SELECT l.Seq,l.Seq1,l.Seq2,l.RequestQty,isnull(p.InQty-p.OutQty+p.AdjustQty-p.LInvQty,0) as StockQty
FROM #tmp l
left join PO_Supp_Detail p on p.ID = '{0}' and p.SEQ1 = l.Seq1 and p.SEQ2 = l.Seq2) a
where a.RequestQty > a.StockQty", CurrentMaintain["POID"].ToString()), out ExceedData);
            }
            catch (Exception ex)
            {
                ShowErr("Save error.", ex);
                return false;
            }
            StringBuilder msg = new StringBuilder();
            foreach (DataRow dr in ExceedData.Rows)
            {
                msg.Append(string.Format("Seq#:{0}  < Request Qty >:{1} exceed stock qty:{2}\r\n",dr["Seq"].ToString(),dr["RequestQty"].ToString(),dr["StockQty"].ToString()));
            }
            if (msg.Length != 0)
            {
                MyUtility.Msg.WarningBox(msg.ToString());
                return false;
            }

            //GetID
            if (IsDetailInserting)
            {
                string id = MyUtility.GetValue.GetID(Sci.Env.User.Keyword + "FR", "Lack", DateTime.Today, 2, "Id", null);
                if (MyUtility.Check.Empty(id))
                {
                    MyUtility.Msg.WarningBox("GetID fail, please try again!");
                    return false;
                }
                CurrentMaintain["ID"] = id;
            }
            return base.ClickSaveBefore();
        }

        //撈取最後發料日
        private DateTime? MaxIssueDate(string Seq1, string Seq2)
        {
            DateTime? maxIssueDate = null;
            DataTable issueData;
            string sqlCmd = string.Format("select max(i.IssueDate) as IssueDate from Issue i, Issue_Detail id where i.Id = id.Id and id.PoId = '{0}' and id.Seq1 = '{1}' and id.Seq2 = '{2}'", CurrentMaintain["POID"].ToString(), Seq1, Seq2);
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out issueData);
            if (result)
            {
                maxIssueDate = MyUtility.Check.Empty(issueData.Rows[0]["IssueDate"]) ? (DateTime?)null : Convert.ToDateTime(issueData.Rows[0]["IssueDate"]);
            }

            return maxIssueDate;
        }

        //Type
        private void comboBox1_Validated(object sender, EventArgs e)
        {
            if (EditMode)
            {
                if (comboBox1.OldValue != comboBox1.SelectedValue && detailgridbs.DataSource != null)
                {
                    foreach (DataRow dr in ((DataTable)detailgridbs.DataSource).Rows)
                    {
                        dr["PPICReasonID"] = "";
                        dr["PPICReasonDesc"] = "";
                    }
                }
            }
        }

        //SP#
        private void textBox2_Validating(object sender, CancelEventArgs e)
        {
            if (EditMode)
            {
                if (textBox2.OldValue != textBox2.Text)
                {
                    if (!MyUtility.Check.Empty(textBox2.Text))
                    {
                        DataRow OrderPOID;
                        string sqlCmd = string.Format("select POID from Orders where ID = '{0}' and FtyGroup = '{1}'", textBox2.Text, Sci.Env.User.Factory);
                        if (MyUtility.Check.Seek(sqlCmd, out OrderPOID))
                        {
                            CurrentMaintain["OrderID"] = textBox2.Text;
                            CurrentMaintain["POID"] = OrderPOID["POID"].ToString();
                        }
                        else
                        {
                            MyUtility.Msg.WarningBox("SP# not exist!!");
                            CurrentMaintain["OrderID"] = "";
                            CurrentMaintain["POID"] = "";
                            e.Cancel = true;
                            return;
                        }
                    }
                    else
                    {
                        CurrentMaintain["OrderID"] = "";
                        CurrentMaintain["POID"] = "";
                    }
                }
            }
        }

        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            DualResult result;
            string updateCmd = string.Format("update Lack set Status = 'Confirmed',ApvName = '{0}',ApvDate = GetDate(), EditName = '{0}', EditDate = GetDate() where ID = '{1}'",Sci.Env.User.UserID,CurrentMaintain["ID"].ToString());
            result = DBProxy.Current.Execute(null, updateCmd);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Confirm fail!\r\n"+result.ToString());
                return;
            }
            result = RenewData();
            OnDetailEntered();
            EnsureToolbarExt();
        }

        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            if (!MyUtility.Check.Empty(CurrentMaintain["IssueLackId"]))
            {
                MyUtility.Msg.WarningBox("This order was issued, can't unconfirm!");
                return;
            }
            DialogResult confirmResult;
            confirmResult = MyUtility.Msg.QuestionBox("Are you sure you want to unconfirm this data?", caption: "Confirm", buttons: MessageBoxButtons.YesNo);
            if (confirmResult != System.Windows.Forms.DialogResult.Yes)
            {
                return;
            }
            DualResult result;
            string updateCmd = string.Format("update Lack set Status = 'New',ApvName = '',ApvDate = null, EditName = '{0}', EditDate = GetDate() where ID = '{1}'", Sci.Env.User.UserID, CurrentMaintain["ID"].ToString());
            result = DBProxy.Current.Execute(null, updateCmd);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Unconfirm fail!\r\n" + result.ToString());
                return;
            }
            result = RenewData();
            OnDetailEntered();
            EnsureToolbarExt();
        }

        protected override void ClickReceive()
        {
            base.ClickReceive();
            if (MyUtility.Check.Empty(CurrentMaintain["IssueLackId"]))
            {
                MyUtility.Msg.WarningBox("< Issue No. > can't empty!");
                return;
            }
            DualResult result;
            string updateCmd = string.Format("update Lack set Status = 'Received', EditName = '{0}', EditDate = GetDate() where ID = '{1}'", Sci.Env.User.UserID, CurrentMaintain["ID"].ToString());
            result = DBProxy.Current.Execute(null, updateCmd);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Receive fail!\r\n" + result.ToString());
                return;
            }
            result = RenewData();
            OnDetailEntered();
            EnsureToolbarExt();
        }
    }
}
