using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;
using Sci.Production.PublicPrg;
using System.Runtime.InteropServices;
using System.Transactions;
using Sci.Win.Tools;

namespace Sci.Production.PPIC
{
    /// <inheritdoc/>
    public partial class P22 : Win.Tems.Input6
    {
        /// <inheritdoc/>
        public P22(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = "MDivisionID = '" + Env.User.Keyword + "' and FabricType = 'F'";
            this.txtuserApprove.TextBox1.ReadOnly = true;
            this.txtuserApprove.TextBox1.IsSupportEditMode = false;
            this.InsertDetailGridOnDoubleClick = false;
        }

        /// <inheritdoc/>
        public P22(ToolStripMenuItem menuitem, string id)
        {
            this.InitializeComponent();
            this.DefaultFilter = "MDivisionID = '" + Env.User.Keyword + "' and FabricType = 'F' and ID = '" + id + "'";
            this.txtuserApprove.TextBox1.ReadOnly = true;
            this.txtuserApprove.TextBox1.IsSupportEditMode = false;
            this.InsertDetailGridOnDoubleClick = false;
        }

        /// <inheritdoc/>

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.lbStatus.Text = this.CurrentMaintain["status"].ToString().Trim();

            if (this.CurrentMaintain["Shift"].Equals("O") && this.EditMode)
            {
                this.txtLocalSupp1.TextBox1.ReadOnly = false;
            }
            else
            {
                this.txtLocalSupp1.TextBox1.ReadOnly = true;
            }
        }

        /// <inheritdoc/>
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
            DataGridViewGeneratorComboBoxColumnSettings process = new DataGridViewGeneratorComboBoxColumnSettings();
            DataTable processSourceDt = new DataTable();
            Dictionary<string, string> processSourcedata = new Dictionary<string, string>();
            string sqlprocess = $@"Select FullName from Production.dbo.subprocess where IsLackingAndReplacement=1";
            DualResult dualResult = DBProxy.Current.Select(null, sqlprocess, out processSourceDt);
            foreach (DataRow item in processSourceDt.Rows)
            {
                string fullname = MyUtility.Convert.GetString(item["FullName"]);
                processSourcedata.Add(fullname, fullname);
            }

            process.DataSource = new BindingSource(processSourcedata, null);
            process.ValueMember = "Key";
            process.DisplayMember = "Value";

            inqty.CellZeroStyle = Ict.Win.UI.DataGridViewNumericBoxZeroStyle.Empty;
            outqty.CellZeroStyle = Ict.Win.UI.DataGridViewNumericBoxZeroStyle.Empty;
            requestqty.CellZeroStyle = Ict.Win.UI.DataGridViewNumericBoxZeroStyle.Empty;
            issueqty.CellZeroStyle = Ict.Win.UI.DataGridViewNumericBoxZeroStyle.Empty;
            #region Seq按右鍵與Validating
            seq.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode)
                {
                    if (e.Button == MouseButtons.Right)
                    {
                        if (e.RowIndex != -1)
                        {
                            DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                            SelectItem item = Prgs.SelePoItem(MyUtility.Convert.GetString(this.CurrentMaintain["POID"]), MyUtility.Convert.GetString(dr["Seq"]), "FabricType = 'F'");
                            DialogResult result = item.ShowDialog();
                            if (result == DialogResult.Cancel)
                            {
                                return;
                            }

                            IList<DataRow> selectData = item.GetSelecteds();
                            dr["Seq"] = item.GetSelectedString();
                            dr["Seq1"] = selectData[0]["Seq1"];
                            dr["Seq2"] = selectData[0]["Seq2"];
                            dr["RefNo"] = selectData[0]["RefNo"];
                            dr["Description"] = selectData[0]["Description"];

                            DataTable wHdata;
                            DualResult whdr = DBProxy.Current.Select(null, string.Format("SELECT m.InQty,m.OutQty FROM MDivisionPoDetail m WITH (NOLOCK) inner join Orders o WITH (NOLOCK) on m.POID=o.ID inner join Factory f WITH (NOLOCK) on f.ID=o.FtyGroup WHERE m.POID = '{0}' AND m.Seq1 = '{1}' AND m.Seq2 = '{2}' AND f.MDivisionID = '{3}'", MyUtility.Convert.GetString(this.CurrentMaintain["POID"]), MyUtility.Convert.GetString(dr["Seq1"]), MyUtility.Convert.GetString(dr["Seq2"]), Env.User.Keyword), out wHdata);
                            if (whdr)
                            {
                                if (wHdata.Rows.Count > 0)
                                {
                                    dr["WhseInQty"] = MyUtility.Convert.GetDecimal(wHdata.Rows[0]["InQty"]);
                                    dr["FTYInQty"] = MyUtility.Convert.GetDecimal(wHdata.Rows[0]["OutQty"]);
                                }
                            }

                            DateTime? maxIssueDate = this.MaxIssueDate(MyUtility.Convert.GetString(selectData[0]["Seq1"]), MyUtility.Convert.GetString(selectData[0]["Seq2"]));
                            if (MyUtility.Check.Empty(maxIssueDate))
                            {
                                dr["FTYLastRecvDate"] = DBNull.Value;
                            }
                            else
                            {
                                dr["FTYLastRecvDate"] = maxIssueDate;
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

                    if (!MyUtility.Check.Empty(e.FormattedValue) && MyUtility.Convert.GetString(e.FormattedValue) != MyUtility.Convert.GetString(dr["Seq"]))
                    {
                        if (MyUtility.Check.Empty(this.CurrentMaintain["POID"]))
                        {
                            this.ClearGridData(dr);
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox("SP# can't empty!!");
                            return;
                        }

                        if (MyUtility.Convert.GetString(e.FormattedValue).IndexOf("'") != -1)
                        {
                            this.ClearGridData(dr);
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox("Can not enter the  '  character!!");
                            return;
                        }

                        DataRow poData;

                        // bug fix:直接輸入seq會有錯誤訊息
                        // string sqlCmd = string.Format(@"select left(seq1+' ',3)+seq2 as Seq, Refno,InQty,OutQty,seq1,seq2, dbo.getmtldesc(id,seq1,seq2,2,0) as Description
                        //                                from dbo.PO_Supp_Detail
                        //                                where id ='{0}' and seq1 = '{1}' and seq2 = '{2}' and FabricType = 'F'", MyUtility.Convert.GetString(CurrentMaintain["POID"]), MyUtility.Convert.GetString(e.FormattedValue).Substring(0, 3), MyUtility.Convert.GetString(e.FormattedValue).Substring(2, 2));
                        string seq12 = MyUtility.Convert.GetString(e.FormattedValue);
                        char[] ch1 = new char[] { ' ' };
                        string[] inputString = seq12.Split(ch1, StringSplitOptions.RemoveEmptyEntries);
                        if (inputString.Length < 2 || inputString.Length > 3)
                        {
                            MyUtility.Msg.WarningBox("Please input legal seq#!! example:01 03");
                            this.ClearGridData(dr);
                            e.Cancel = true;
                            return;
                        }

                        string sqlCmd = string.Format(
                            @"select 
left(psd.seq1+' ',3)+psd.seq2 as Seq
, psd.Refno,isnull(m.InQty,0) as InQty
,isnull(m.OutQty,0) as OutQty
,psd.seq1
,psd.seq2
, dbo.getmtldesc(psd.id,psd.seq1,psd.seq2,2,0) as Description 
,[Status]=IIF(LockStatus.LockCount > 0 ,'Locked','Unlocked')
from dbo.PO_Supp_Detail psd WITH (NOLOCK) 
left join MDivisionPoDetail m WITH (NOLOCK) on m.POID = psd.ID and m.Seq1 = psd.SEQ1 and m.Seq2 = psd.SEQ2
inner join dbo.Factory F WITH (NOLOCK) on F.id=psd.factoryid and F.MDivisionID='{3}'
OUTER APPLY(
	SELECT [LockCount]=COUNT(UKEY)
	FROM FtyInventory
	WHERE POID='{0}'
	AND Seq1=psd.Seq1
	AND Seq2=psd.Seq2
	AND Lock = 1
)LockStatus
                        where psd.id ='{0}' and psd.seq1 = '{1}' and psd.seq2 = '{2}' and psd.FabricType = 'F'",
                            MyUtility.Convert.GetString(this.CurrentMaintain["POID"]),
                            inputString[0],
                            inputString[1],
                            Env.User.Keyword);

                        if (!MyUtility.Check.Seek(sqlCmd, out poData))
                        {
                            this.ClearGridData(dr);
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox(string.Format("< Seq: {0} > not found!!!", MyUtility.Convert.GetString(e.FormattedValue)));
                            return;
                        }
                        else
                        {
                            dr["Seq"] = poData["Seq"];
                            dr["Seq1"] = poData["Seq1"];
                            dr["Seq2"] = poData["Seq2"];
                            dr["RefNo"] = poData["RefNo"];
                            dr["Description"] = poData["Description"];
                            dr["WhseInQty"] = MyUtility.Convert.GetDecimal(poData["InQty"]);
                            dr["FTYInQty"] = MyUtility.Convert.GetDecimal(poData["OutQty"]);
                            dr["Status"] = MyUtility.Convert.GetString(poData["Status"]);
                            DateTime? maxIssueDate = this.MaxIssueDate(MyUtility.Convert.GetString(poData["Seq1"]), MyUtility.Convert.GetString(poData["Seq2"]));
                            if (MyUtility.Check.Empty(maxIssueDate))
                            {
                                dr["FTYLastRecvDate"] = DBNull.Value;
                            }
                            else
                            {
                                dr["FTYLastRecvDate"] = maxIssueDate;
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
                if (e.Button == MouseButtons.Left)
                {
                    if (e.RowIndex != -1)
                    {
                        DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                        EditMemo callNextForm = new EditMemo(MyUtility.Convert.GetString(dr["Description"]), "Description", false, null);
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
                    if (e.Button == MouseButtons.Right)
                    {
                        if (e.RowIndex != -1)
                        {
                            DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                            SelectItem item = new SelectItem(string.Format("select ID,Description from PPICReason WITH (NOLOCK) where Type = 'FL' and Junk = 0 and TypeForUse = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["Type"])), "5,40", MyUtility.Convert.GetString(dr["PPICReasonID"]));
                            DialogResult returnResult = item.ShowDialog();
                            if (returnResult == DialogResult.Cancel)
                            {
                                return;
                            }

                            IList<DataRow> selectData = item.GetSelecteds();
                            dr["PPICReasonID"] = item.GetSelectedString();
                            dr["PPICReasonDesc"] = selectData[0]["Description"];
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

                    if (!MyUtility.Check.Empty(e.FormattedValue) && MyUtility.Convert.GetString(e.FormattedValue) != MyUtility.Convert.GetString(dr["Seq"]))
                    {
                        if (MyUtility.Convert.GetString(e.FormattedValue).IndexOf("'") != -1)
                        {
                            dr["PPICReasonID"] = string.Empty;
                            dr["PPICReasonDesc"] = string.Empty;
                            dr.EndEdit();
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox("Can not enter the  '  character!!");
                            return;
                        }

                        DataRow reasonData;
                        string sqlCmd = string.Format(@"select ID,Description from PPICReason WITH (NOLOCK) where Type = 'FL' and Junk = 0 and TypeForUse = '{0}' and ID = '{1}'", MyUtility.Convert.GetString(this.CurrentMaintain["Type"]), MyUtility.Convert.GetString(e.FormattedValue));
                        if (!MyUtility.Check.Seek(sqlCmd, out reasonData))
                        {
                            dr["PPICReasonID"] = string.Empty;
                            dr["PPICReasonDesc"] = string.Empty;
                            dr.EndEdit();
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox(string.Format("< Reason Id: {0} > not found!!!", MyUtility.Convert.GetString(e.FormattedValue)));
                            return;
                        }
                        else
                        {
                            dr["PPICReasonID"] = MyUtility.Convert.GetString(e.FormattedValue);
                            dr["PPICReasonDesc"] = reasonData["Description"];
                            dr.EndEdit();
                        }
                    }
                }
            };
            #endregion

            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("Seq", header: "Seq#", width: Widths.AnsiChars(5), settings: seq)
                .Text("RefNo", header: "Refer#", width: Widths.AnsiChars(15), iseditingreadonly: true, settings: refno)
                .Date("FTYLastRecvDate", header: "Prod. Last Rcvd Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("FTYInQty", header: "Prod. Accu. Rcvd Qty", width: Widths.AnsiChars(10), decimal_places: 2, iseditingreadonly: true, settings: outqty)
                .Numeric("WhseInQty", header: "WH Accu. Rcvd Qty", width: Widths.AnsiChars(10), decimal_places: 2, iseditingreadonly: true, settings: inqty)
                .Numeric("RequestQty", header: "Request Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2, maximum: 99999999.99M, minimum: 0, settings: requestqty)
                .Numeric("RejectQty", header: "# of pcs rejected")
                .Numeric("IssueQty", header: "Issue Qty upon request", decimal_places: 2, iseditingreadonly: true, settings: issueqty)
                .ComboBox("Process", header: "Process", width: Widths.AnsiChars(15), settings: process)
                .Text("PPICReasonID", header: "Reason Id", width: Widths.AnsiChars(5), settings: reason)
                .EditText("PPICReasonDesc", header: "Reason", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("Status", header: "Status", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("Remark", header: "Remark", width: Widths.AnsiChars(20), iseditingreadonly: false);
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            MyUtility.Tool.SetupCombox(this.comboType, 2, 1, "L,Lacking,R,Replacement");
            MyUtility.Tool.SetupCombox(this.comboShift, 2, 1, "D,Day,N,Night,O,Subcon-Out");
            this.queryfors.SelectedIndexChanged += (s, e) =>
            {
                switch (this.queryfors.SelectedIndex)
                {
                    case 0:
                        this.DefaultWhere = string.Empty;
                        break;
                    default:
                        this.DefaultWhere = string.Format("FactoryID = '{0}'", this.queryfors.SelectedValue);
                        break;
                }

                this.ReloadDatas();
            };
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["MDivisionID"] = Env.User.Keyword;
            this.CurrentMaintain["FabricType"] = "F";
            this.CurrentMaintain["ApplyName"] = Env.User.UserID;
            this.CurrentMaintain["Status"] = "New";
        }

        /// <inheritdoc/>
        protected override bool ClickEditBefore()
        {
            if (MyUtility.Convert.GetString(this.CurrentMaintain["Status"]) != "New")
            {
                MyUtility.Msg.WarningBox("This record was approved, can't modify.");
                return false;
            }

            return true;
        }

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            if (this.CurrentMaintain["Status"].ToString() != "New")
            {
                this.txtDept.ReadOnly = true;
            }
        }

        /// <inheritdoc/>
        protected override bool ClickDeleteBefore()
        {
            if (MyUtility.Convert.GetString(this.CurrentMaintain["Status"]) != "New")
            {
                MyUtility.Msg.WarningBox("This record was approved, can't delete.");
                return false;
            }

            return true;
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            #region 檢查必輸欄位
            if (MyUtility.Check.Empty(this.CurrentMaintain["Type"]))
            {
                MyUtility.Msg.WarningBox("Type can't empty");
                this.comboType.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["Shift"]))
            {
                MyUtility.Msg.WarningBox("Shift can't empty");
                this.comboShift.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["OrderID"]))
            {
                MyUtility.Msg.WarningBox("SP# can't empty");
                this.txtSP.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["ApplyName"]))
            {
                MyUtility.Msg.WarningBox("Handle can't empty");
                this.txtuserHandle.TextBox1.Focus();
                return false;
            }
            #endregion
            int i = 0; // 計算表身Grid的總筆數
            foreach (DataRow dr in this.DetailDatas)
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
                if (MyUtility.Check.Empty(dr["RequestQty"]) || MyUtility.Convert.GetDecimal(dr["RequestQty"]) <= 0)
                {
                    MyUtility.Msg.WarningBox("< Request Qty >  can't equal or less 0!");
                    return false;
                }

                if (MyUtility.Check.Empty(dr["PPICReasonID"]))
                {
                    MyUtility.Msg.WarningBox("< Reason Id >  can't empty!");
                    return false;
                }

                if (MyUtility.Convert.GetString(this.CurrentMaintain["Type"]) == "R" && (MyUtility.Check.Empty(dr["RejectQty"]) || MyUtility.Convert.GetInt(dr["RejectQty"]) <= 0))
                {
                    MyUtility.Msg.WarningBox("< # of pcs rejected >  can't equal or less 0!");
                    return false;
                }

                #endregion
            }

            // 表身Grid資料不可為空
            if (i == 0)
            {
                MyUtility.Msg.WarningBox("Detail can't empty!!");
                return false;
            }

            // RequestQty不可以超過Warehouse的A倉庫存數量
            DataTable exceedData;
            try
            {
                string strSQLSelect = string.Format(
                    @"
select * from (
SELECT l.Seq,l.Seq1,l.Seq2,l.RequestQty,isnull(mpd.InQty-mpd.OutQty+mpd.AdjustQty-mpd.LInvQty,0) as StockQty
FROM #tmp l
left join MDivisionPoDetail mpd WITH (NOLOCK) on mpd.POID = '{0}' and mpd.SEQ1 = l.Seq1 and mpd.SEQ2 = l.Seq2) a
where a.RequestQty > a.StockQty",
                    MyUtility.Convert.GetString(this.CurrentMaintain["POID"]));

                MyUtility.Tool.ProcessWithDatatable(
                    (DataTable)this.detailgridbs.DataSource,
                    "Seq,Seq1,Seq2,RequestQty",
                    strSQLSelect,
                    out exceedData);
            }
            catch (Exception ex)
            {
                this.ShowErr("Save error.", ex);
                return false;
            }

            StringBuilder msg = new StringBuilder();
            if (this.comboType.Text != "Lacking")
            {
                foreach (DataRow dr in exceedData.Rows)
                {
                    msg.Append(string.Format("Seq#:{0}  < Request Qty >:{1} exceed stock qty:{2}\r\n", MyUtility.Convert.GetString(dr["Seq"]), MyUtility.Convert.GetString(dr["RequestQty"]), MyUtility.Convert.GetString(dr["StockQty"])));
                }
            }

            if (msg.Length != 0)
            {
                MyUtility.Msg.WarningBox(msg.ToString());
                return false;
            }

            // GetID
            if (this.IsDetailInserting)
            {
                string id = MyUtility.GetValue.GetID(Env.User.Factory + "FR", "Lack", DateTime.Today, 2, "Id", null);
                if (MyUtility.Check.Empty(id))
                {
                    MyUtility.Msg.WarningBox("GetID fail, please try again!");
                    return false;
                }

                this.CurrentMaintain["ID"] = id;
            }

            return base.ClickSaveBefore();
        }

        // SP#
        private void TxtSP_Validating(object sender, CancelEventArgs e)
        {
            if (this.EditMode)
            {
                if (this.txtSP.OldValue != this.txtSP.Text)
                {
                    if (!MyUtility.Check.Empty(this.txtSP.Text))
                    {
                        // 根據 this.txtSP.Text 取得POID
                        string orderid = this.txtSP.Text;
                        string poid = MyUtility.GetValue.Lookup("poid", orderid, "orders", "id");
                        string sqlCmd = string.Empty;
                        IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();

                        #region 驗證1：使用者登入 Factory 必須是 Factory.IsProduceFty =1
                        DataTable ftyGroupData;

                        sqlCmd = $@"select FTYGroup from Factory where id='{Env.User.Factory}' and IsProduceFty = 1";
                        DBProxy.Current.Select(null, sqlCmd, out ftyGroupData);
                        if (ftyGroupData.Rows.Count == 0)
                        {
                            MyUtility.Msg.WarningBox("SP No. not found!!");
                            this.CurrentMaintain["OrderID"] = string.Empty;
                            this.CurrentMaintain["POID"] = string.Empty;
                            this.CurrentMaintain["FactoryID"] = string.Empty;
                            e.Cancel = true;
                            return;
                        }

                        // Get FTYGroup
                        string ftyGroup = ftyGroupData.Rows[0]["FTYGroup"].ToString();
                        #endregion

                        #region 驗證2：Orders.FtyGroup 必須是 與登入者的FTYGroup相同
                        cmds.Add(new System.Data.SqlClient.SqlParameter("@OrderID", orderid));

                        DataTable orders;
                        sqlCmd = "select FtyGroup from Orders WITH (NOLOCK) where ID = @OrderID";
                        DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out orders);

                        if (result && orders.Rows.Count > 0)
                        {
                            if (MyUtility.Convert.GetString(orders.Rows[0]["FtyGroup "]) != ftyGroup)
                            {
                                MyUtility.Msg.WarningBox($"Current login factory is {ftyGroup} , it is different factory group with SP# factory {MyUtility.Convert.GetString(orders.Rows[0]["FtyGroup "])}");
                                this.CurrentMaintain["OrderID"] = string.Empty;
                                this.CurrentMaintain["POID"] = string.Empty;
                                this.CurrentMaintain["FactoryID"] = string.Empty;
                                e.Cancel = true;
                                return;
                            }

                            // 通過驗證
                            this.CurrentMaintain["OrderID"] = this.txtSP.Text;
                            this.CurrentMaintain["POID"] = poid;
                            this.CurrentMaintain["FactoryID"] = ftyGroup;
                        }
                        else
                        {
                            this.CurrentMaintain["OrderID"] = string.Empty;
                            this.CurrentMaintain["POID"] = string.Empty;
                            this.CurrentMaintain["FactoryID"] = string.Empty;
                            this.DeleteAllGridData();
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox("SP# not exist!!");
                            return;
                        }
                        #endregion
                    }
                    else
                    {
                        this.CurrentMaintain["OrderID"] = string.Empty;
                        this.CurrentMaintain["POID"] = string.Empty;
                        this.CurrentMaintain["FactoryID"] = string.Empty;
                    }

                    this.DeleteAllGridData();
                }
            }
        }

        // 刪除表身Grid資料
        private void DeleteAllGridData()
        {
            foreach (DataRow dr in this.DetailDatas)
            {
                dr.Delete();
            }
        }

        private void P22_FormLoaded(object sender, EventArgs e)
        {
            DataTable queryDT;
            string querySql = string.Format(
                @"
select '' FTYGroup

union 
select distinct FTYGroup 
from Factory 
where MDivisionID = '{0}'", Env.User.Keyword);
            DBProxy.Current.Select(null, querySql, out queryDT);
            MyUtility.Tool.SetupCombox(this.queryfors, 1, queryDT);
            this.queryfors.SelectedIndex = 0;
        }
    }
}
