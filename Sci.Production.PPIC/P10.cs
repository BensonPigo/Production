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
    /// <summary>
    /// P10
    /// </summary>
    public partial class P10 : Win.Tems.Input6
    {
        /// <summary>
        /// P10
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public P10(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = "MDivisionID = '" + Env.User.Keyword + "' and FabricType = 'F'";
            this.txtuserApprove.TextBox1.ReadOnly = true;
            this.txtuserApprove.TextBox1.IsSupportEditMode = false;
            this.InsertDetailGridOnDoubleClick = false;
            this.displayIssueLackDate.ReadOnly = true;
        }

        /// <summary>
        /// P10
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        /// <param name="id">id</param>
        public P10(ToolStripMenuItem menuitem, string id)
        {
            this.InitializeComponent();
            this.DefaultFilter = "MDivisionID = '" + Env.User.Keyword + "' and FabricType = 'F' and ID = '" + id + "'";
            this.txtuserApprove.TextBox1.ReadOnly = true;
            this.txtuserApprove.TextBox1.IsSupportEditMode = false;
            this.InsertDetailGridOnDoubleClick = false;
            this.displayIssueLackDate.ReadOnly = true;
        }

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : MyUtility.Convert.GetString(e.Master["ID"]);
            this.DetailSelectCommand = $@"
select ld.*
,(left(ld.Seq1+' ',3)+ld.Seq2) as Seq
,[Refno]=isnull(psd.Refno,'') 
,[Description]=dbo.getMtlDesc(l.POID,ld.Seq1,ld.Seq2,1,0) 
,isnull(p.Description,'') as PPICReasonDesc
,[Status]=IIF(LockStatus.LockCount > 0 ,'Locked','Unlocked')
from Lack l WITH (NOLOCK) 
inner join Lack_Detail ld WITH (NOLOCK) on l.ID = ld.ID
left join PO_Supp_Detail psd WITH (NOLOCK) on psd.ID = l.POID and psd.SEQ1 = ld.Seq1 and psd.SEQ2 = ld.Seq2
left join Fabric f WITH (NOLOCK) on psd.SCIRefno = f.SCIRefno
left join PPICReason p WITH (NOLOCK) on p.Type = 'FL' and ld.PPICReasonID = p.ID
OUTER APPLY(
	SELECT [LockCount]=COUNT(UKEY)
	FROM FtyInventory
	WHERE POID=l.OrderID
	AND Seq1=ld.Seq1
	AND Seq2=ld.Seq2
	AND Lock = 1
)LockStatus

where l.ID = '{masterID}'
order by ld.Seq1,ld.Seq2";

            return base.OnDetailSelectCommandPrepare(e);
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

        private void ClearGridData(DataRow dr)
        {
            dr["Seq"] = string.Empty;
            dr["Seq1"] = string.Empty;
            dr["Seq2"] = string.Empty;
            dr["RefNo"] = string.Empty;
            dr["Description"] = string.Empty;
            dr["WhseInQty"] = 0;
            dr["FTYInQty"] = 0;
            dr["FTYLastRecvDate"] = DBNull.Value;
            dr["Remark"] = string.Empty;
            dr.EndEdit();
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["IssueDate"] = DateTime.Today;
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

        /// <inheritdoc/>
        protected override bool ClickPrint()
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["ID"]))
            {
                MyUtility.Msg.WarningBox("No Data!!");
                return false;
            }

            string sqlCmd = string.Format(
                @"select (left(ld.Seq1+' ',3)+'-'+ld.Seq2) as Seq, dbo.getMtlDesc(l.POID,ld.Seq1,ld.Seq2,1,0) as Description,
            ld.FTYLastRecvDate,ld.FTYInQty,ld.WhseInQty,ld.RequestQty,ld.IssueQty
            from Lack l WITH (NOLOCK) 
            left join Lack_Detail ld WITH (NOLOCK) on l.ID = ld.ID
            where l.ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));

            DataTable excelData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out excelData);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Query data fail!!\r\n" + result.ToString());
                return false;
            }

            string strXltName = Env.Cfg.XltPathDir + "\\PPIC_P10.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return false;
            }

            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];
            worksheet.Cells[1, 1] = MyUtility.Convert.GetString(this.CurrentMaintain["MDivisionID"]);
            worksheet.Cells[4, 2] = MyUtility.Convert.GetString(this.CurrentMaintain["ID"]);
            worksheet.Cells[5, 2] = Convert.ToDateTime(this.CurrentMaintain["IssueDate"]).ToString("d");
            worksheet.Cells[6, 2] = MyUtility.Convert.GetString(this.CurrentMaintain["OrderID"]);
            worksheet.Cells[7, 2] = MyUtility.Convert.GetString(this.CurrentMaintain["Remark"]);

            worksheet.Cells[4, 4] = MyUtility.Convert.GetString(this.CurrentMaintain["Type"]) == "R" ? "Replacement" : "Lacking";
            worksheet.Cells[5, 4] = MyUtility.Convert.GetString(this.CurrentMaintain["Shift"]) == "D" ? "Day"
                                    : MyUtility.Convert.GetString(this.CurrentMaintain["Shift"]) == "N" ? "Night" : "Subcon-Out";
            worksheet.Cells[6, 4] = MyUtility.Convert.GetString(this.CurrentMaintain["SewingLineID"]);

            worksheet.Cells[4, 6] = this.txtuserHandle.TextBox1.Text + " " + this.txtuserHandle.DisplayBox1.Text;
            worksheet.Cells[5, 6] = this.txtuserApprove.TextBox1.Text + " " + this.txtuserApprove.DisplayBox1.Text;
            worksheet.Cells[6, 6] = MyUtility.Check.Empty(this.CurrentMaintain["ApvDate"]) ? string.Empty : Convert.ToDateTime(this.CurrentMaintain["ApvDate"]).ToString("d");

            int intRowsStart = 10;
            int dataRowCount = excelData.Rows.Count;
            int rownum = 0;
            object[,] objArray = new object[1, 7];
            for (int i = 0; i < dataRowCount; i++)
            {
                DataRow dr = excelData.Rows[i];
                rownum = intRowsStart + i;
                objArray[0, 0] = dr["Seq"];
                objArray[0, 1] = dr["Description"];
                objArray[0, 2] = dr["FTYLastRecvDate"];
                objArray[0, 3] = dr["FTYInQty"];
                objArray[0, 4] = dr["WhseInQty"];
                objArray[0, 5] = dr["RequestQty"];
                objArray[0, 6] = dr["IssueQty"];

                worksheet.Range[string.Format("A{0}:G{0}", rownum)].Value2 = objArray;
            }

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("PPIC_P10");
            Microsoft.Office.Interop.Excel.Workbook workbook = excel.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(worksheet);
            Marshal.ReleaseComObject(workbook);

            strExcelName.OpenFile();
            #endregion
            return base.ClickPrint();
        }

        // 撈取最後發料日
        private DateTime? MaxIssueDate(string seq1, string seq2)
        {
            DateTime? maxIssueDate = null;
            DataTable issueData;
            string sqlCmd = string.Format("select max(i.IssueDate) as IssueDate from Issue i WITH (NOLOCK) , Issue_Detail id WITH (NOLOCK) where i.Id = id.Id and id.PoId = '{0}' and id.Seq1 = '{1}' and id.Seq2 = '{2}'", MyUtility.Convert.GetString(this.CurrentMaintain["POID"]), seq1, seq2);
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out issueData);
            if (result)
            {
                maxIssueDate = MyUtility.Convert.GetDate(issueData.Rows[0]["IssueDate"]);
            }

            return maxIssueDate;
        }

        // Type
        private void ComboType_Validated(object sender, EventArgs e)
        {
            if (this.EditMode)
            {
                if (this.comboType.OldValue != this.comboType.SelectedValue && this.detailgridbs.DataSource != null)
                {
                    foreach (DataRow dr in ((DataTable)this.detailgridbs.DataSource).Rows)
                    {
                        dr["PPICReasonID"] = string.Empty;
                        dr["PPICReasonDesc"] = string.Empty;
                    }
                }
            }
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
                        // sql參數
                        string poid = MyUtility.GetValue.Lookup("poid", this.txtSP.Text, "orders", "id");
                        System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter("@poid", poid);

                        // 用登入的Factory 抓取對應的FtyGroup
                        DataTable ftyGroupData;
                        DBProxy.Current.Select(null, string.Format("select FTYGroup from Factory where id='{0}' and IsProduceFty = 1", Env.User.Factory), out ftyGroupData);
                        if (ftyGroupData.Rows.Count == 0)
                        {
                            MyUtility.Msg.WarningBox("SP No. not found!!");
                            this.CurrentMaintain["OrderID"] = string.Empty;
                            this.CurrentMaintain["POID"] = string.Empty;
                            this.CurrentMaintain["FactoryID"] = string.Empty;
                            e.Cancel = true;
                            return;
                        }

                        System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter("@factoryid", ftyGroupData.Rows[0]["FTYGroup"].ToString());

                        IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                        cmds.Add(sp1);
                        cmds.Add(sp2);

                        DataTable orderPOID;
                        string sqlCmd = "select POID,FtyGroup from Orders WITH (NOLOCK) where POID = @poid and FtyGroup  = @factoryid";
                        DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out orderPOID);

                        if (result && orderPOID.Rows.Count > 0)
                        {
                            this.CurrentMaintain["OrderID"] = this.txtSP.Text;
                            this.CurrentMaintain["POID"] = orderPOID.Rows[0]["POID"];
                            this.CurrentMaintain["FactoryID"] = orderPOID.Rows[0]["FtyGroup"];
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

        /// <inheritdoc/>
        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            DualResult result;

            string updateCmd = string.Format("update Lack set Status = 'Confirmed',ApvName = '{0}',ApvDate = GetDate(), EditName = '{0}', EditDate = GetDate() where ID = '{1}'", Env.User.UserID, MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));
            result = DBProxy.Current.Execute(null, updateCmd);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Confirm fail!\r\n" + result.ToString());
                return;
            }

            DataTable dt;
            result = DBProxy.Current.Select(null, "SELECT * FROM MailTo WHERE ID = '021'  ", out dt);

            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query Mail Setting fail!\r\n" + result.ToString());
                return;
            }

            if (dt.Rows.Count != 0 && dt.Rows[0]["ToAddress"].ToString() != string.Empty)
            {
                string applyName = MyUtility.GetValue.Lookup($@"
SELECT p.EMail
FROM Lack l
INNER JOIN Pass1 p ON p.ID = l.ApplyName
WHERE l.ID='{this.CurrentMaintain["ID"]}'
");
                string toAddress = dt.Rows[0]["ToAddress"].ToString();
                string ccAddress = dt.Rows[0]["CCAddress"].ToString() + $";{applyName}";
                string subject = dt.Rows[0]["Subject"].ToString();

                // 取得表頭 P10 的單號
                string content = this.CurrentMaintain["ID"].ToString();

                var email = new MailTo(Env.Cfg.MailFrom, toAddress, ccAddress, subject, null, content, true, true);
                email.ShowDialog();

                if (email.SendMailResult)
                {
                    MyUtility.Msg.InfoBox("Send mail successful.");
                }
            }
        }

        /// <inheritdoc/>
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            if (!MyUtility.Check.Empty(this.CurrentMaintain["IssueLackId"]))
            {
                MyUtility.Msg.WarningBox("This order was issued, can't unconfirm!");
                return;
            }

            DialogResult confirmResult;
            confirmResult = MyUtility.Msg.QuestionBox("Are you sure you want to unconfirm this data?", caption: "Confirm", buttons: MessageBoxButtons.YesNo);
            if (confirmResult != DialogResult.Yes)
            {
                return;
            }

            DualResult result;
            string updateCmd = string.Format("update Lack set Status = 'New',ApvName = '',ApvDate = null, EditName = '{0}', EditDate = GetDate() where ID = '{1}'", Env.User.UserID, MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));
            result = DBProxy.Current.Execute(null, updateCmd);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Unconfirm fail!\r\n" + result.ToString());
                return;
            }
        }

        /// <inheritdoc/>
        protected override void ClickReceive()
        {
            base.ClickReceive();
            if (MyUtility.Check.Empty(this.CurrentMaintain["IssueLackId"]))
            {
                MyUtility.Msg.WarningBox("< Issue No. > can't empty!");
                return;
            }

            DualResult result;
            string updateCmd = string.Format("update Lack set Status = 'Received', EditName = '{0}', EditDate = GetDate() where ID = '{1}'", Env.User.UserID, MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));
            result = DBProxy.Current.Execute(null, updateCmd);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Receive fail!\r\n" + result.ToString());
                return;
            }
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.lbStatus.Text = this.CurrentMaintain["status"].ToString().Trim();
            if (!MyUtility.Check.Empty(this.CurrentMaintain["IssueLackDT"]))
            {
                this.displayIssueLackDate.Text = Convert.ToDateTime(this.CurrentMaintain["IssueLackDT"]).ToString("yyyy/MM/dd HH:mm:ss");
            }
            else
            {
                this.displayIssueLackDate.Text = string.Empty;
            }

            if (this.CurrentMaintain["Shift"].Equals("O") && this.EditMode)
            {
                this.txtLocalSupp1.TextBox1.ReadOnly = false;
            }
            else
            {
                this.txtLocalSupp1.TextBox1.ReadOnly = true;
            }

            if (this.CurrentMaintain["Status"].EqualString("Confirmed") && !this.EditMode)
            {
                this.btnAutoOutputQuery.Enabled = true;
            }
            else
            {
                this.btnAutoOutputQuery.Enabled = false;
            }
        }

        private void P10_FormLoaded(object sender, EventArgs e)
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

        private void BtnImport_Click(object sender, EventArgs e)
        {
            var frm = new P10_P11_Import(this.CurrentMaintain, (DataTable)this.detailgridbs.DataSource, "Fabric");
            frm.ShowDialog(this);
            this.RenewData();
        }

        private void ComboShift_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!MyUtility.Check.Empty(this.comboShift.SelectedValue) && this.EditMode)
            {
                if (this.comboShift.SelectedValue.Equals("O"))
                {
                    this.txtLocalSupp1.TextBox1.ReadOnly = false;
                }
                else
                {
                    this.txtLocalSupp1.TextBox1.ReadOnly = true;
                    this.CurrentMaintain["Shift"] = this.comboShift.SelectedValue;
                    this.CurrentMaintain["SubconName"] = string.Empty;
                }
            }
        }

        private void BtnAutoOutputQuery_Click(object sender, EventArgs e)
        {
            if (!MyUtility.GetValue.Lookup($"select status from Lack where id = '{this.CurrentMaintain["ID"]}'").EqualString("Confirmed"))
            {
                MyUtility.Msg.WarningBox("Must confirm!");
                this.Refresh();
                this.OnDetailEntered();
                return;
            }

            DataRow dr;
            if (!MyUtility.Check.Seek($@"select [type],[apvdate],[issuelackid],[Shift],[SubconName] from dbo.lack WITH (NOLOCK) where id='{this.CurrentMaintain["ID"]}' and fabrictype='F' and mdivisionid='{Env.User.Keyword}'", out dr, null))
            {
                MyUtility.Msg.WarningBox("Please check requestid is Fabric.", "Data not found!!");
                return;
            }
            else
            {
                if (MyUtility.Check.Empty(dr["apvdate"]))
                {
                    MyUtility.Msg.WarningBox("Request is not approved!!");
                    return;
                }

                if (!MyUtility.Check.Empty(dr["issuelackid"]))
                {
                    MyUtility.Msg.WarningBox(string.Format("This request# ({0}) already issued by {1}.", this.CurrentMaintain["ID"], dr["issuelackid"]));
                    return;
                }
            }

            string tmpId = MyUtility.GetValue.GetID(Env.User.Keyword + "IL", "IssueLack", DateTime.Now);
            if (MyUtility.Check.Empty(tmpId))
            {
                MyUtility.Msg.WarningBox("Get document ID fail!!");
                return;
            }

            string requestid = MyUtility.Convert.GetString(this.CurrentMaintain["ID"]);
            string type = dr["type"].ToString();

            #region 檢查WH P16是否已經建立過
            string chkex = $@"select id from issuelack where RequestID = '{this.CurrentMaintain["ID"]}'";
            DataRow drow;
            if (MyUtility.Check.Seek(chkex, out drow))
            {
                MyUtility.Msg.WarningBox($"Already exists {drow["id"]}");
                return;
            }
            #endregion

            #region 檢查庫存是否足夠
            string sqlchk = $@"
select s = concat(a.POID,' ', b.seq1,'-', b.seq2)
from dbo.lack a WITH (NOLOCK) 
inner join dbo.Lack_Detail b WITH (NOLOCK) on a.ID = b.ID
inner join dbo.ftyinventory c WITH (NOLOCK) on c.poid = a.POID 
											   and c.seq1 = b.seq1 
											   and c.seq2  = b.seq2 
											   and c.stocktype = 'B'

where a.id = '{this.CurrentMaintain["ID"]}'
and c.lock = 0  and (c.inqty-c.outqty + c.adjustqty) > 0
";
            if (type != "Lacking")
            {
                sqlchk += " and (c.inqty-c.outqty + c.adjustqty) > 0";
            }

            sqlchk += @"
group by a.POID, b.seq1, b.seq2, b.RequestQty
having sum(c.inqty - c.outqty + c.adjustqty) < b.RequestQty
";

            DataTable chkdt;
            DBProxy.Current.Select(null, sqlchk, out chkdt);
            string msg = string.Empty;
            if (chkdt.Rows.Count > 0)
            {
                foreach (DataRow item in chkdt.Rows)
                {
                    msg += MyUtility.Convert.GetString(item[0]) + Environment.NewLine;
                }
            }

            if (!MyUtility.Check.Empty(msg))
            {
                MyUtility.Msg.WarningBox("These items are understocked" + Environment.NewLine + msg);
            }
            #endregion

            string t1 = $@"
select poid = rtrim(a.POID) 
	   , b.seq1
	   , b.seq2
	   , b.RequestQty
from dbo.lack a WITH (NOLOCK) 
inner join dbo.Lack_Detail b WITH (NOLOCK) on a.ID = b.ID
where a.id = '{this.CurrentMaintain["ID"]}';
";
            DualResult result;
            DataTable dt1;
            result = DBProxy.Current.Select(null, t1, out dt1);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            DataTable ldt;
            string sch = $@"select  ID = '{tmpId}', FtyInventoryukey = c.ukey, Qty = b.RequestQty, MDivisionID = '', POID = rtrim(a.POID) , b.Seq1, b.Seq2, Roll, Dyelot, StockType = 'B' 
from dbo.lack a WITH (NOLOCK) 
inner join dbo.Lack_Detail b WITH (NOLOCK) on a.ID = b.ID
inner join dbo.ftyinventory c WITH (NOLOCK) on c.poid = a.POID and c.seq1 = b.seq1 and c.seq2  = b.seq2 and c.stocktype = 'B'
Where 1=0";
            result = DBProxy.Current.Select(null, sch, out ldt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }
            #region 分配qty , 準備表身
            foreach (DataRow dt1row in dt1.Rows)
            {
                string t2 = $@"
select  
        ID = '{tmpId}'
        , FtyInventoryukey = c.ukey
	    , Qty = b.RequestQty
		, MDivisionID = ''
        , POID = rtrim(a.POID) 
	    , b.Seq1
	    , b.Seq2
		, Roll
		, Dyelot
        , StockType = 'B' 
        , Stock = c.inqty - c.outqty + c.adjustqty
from dbo.lack a WITH (NOLOCK) 
inner join dbo.Lack_Detail b WITH (NOLOCK) on a.ID = b.ID
inner join dbo.ftyinventory c WITH (NOLOCK) on c.poid = a.POID 
											   and c.seq1 = b.seq1 
											   and c.seq2  = b.seq2 
											   and c.stocktype = 'B'
Where c.lock = 0  and a.id = '{this.CurrentMaintain["ID"]}' and c.poid = '{dt1row["poid"]}' and c.seq1 = '{dt1row["seq1"]}' and c.seq2 = '{dt1row["seq2"]}'
";

                if (type != "Lacking")
                {
                    t2 += " and (c.inqty-c.outqty + c.adjustqty) > 0";
                }

                DataTable dt2;
                result = DBProxy.Current.Select(null, t2, out dt2);
                if (!result)
                {
                    this.ShowErr(result);
                    return;
                }

                decimal requestQty = MyUtility.Convert.GetDecimal(dt1row["RequestQty"]);
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    if (requestQty > 0)
                    {
                        DataRow lrow = ldt.NewRow();
                        decimal sq = MyUtility.Convert.GetDecimal(dt2.Rows[i]["Stock"]);
                        if (sq < requestQty)
                        {
                            if (i + 1 == dt2.Rows.Count)
                            {
                                lrow["Qty"] = requestQty;
                            }
                            else
                            {
                                lrow["Qty"] = sq;
                                requestQty = requestQty - sq;
                            }
                        }
                        else
                        {
                            lrow["Qty"] = requestQty;
                            requestQty = 0;
                        }

                        lrow["id"] = tmpId;
                        lrow["FtyInventoryukey"] = dt2.Rows[i]["FtyInventoryukey"];
                        lrow["MDivisionID"] = dt2.Rows[i]["MDivisionID"];
                        lrow["POID"] = dt2.Rows[i]["POID"];
                        lrow["Seq1"] = dt2.Rows[i]["Seq1"];
                        lrow["Seq2"] = dt2.Rows[i]["Seq2"];
                        lrow["Roll"] = dt2.Rows[i]["Roll"];
                        lrow["Dyelot"] = dt2.Rows[i]["Dyelot"];
                        lrow["StockType"] = dt2.Rows[i]["StockType"];

                        ldt.Rows.Add(lrow);
                    }
                }
            }
            #endregion
            string sqlinsert = $@"
INSERT INTO [dbo].[IssueLack]([Id],[Type],[MDivisionID],[FactoryID],[IssueDate],[Status],[RequestID],[Remark],[ApvName],[ApvDate],[FabricType],[AddName],[AddDate])
VALUES('{tmpId}','{type}','{Env.User.Keyword}','{Env.User.Factory}',GETDATE(),'NEW','{requestid}','{this.CurrentMaintain["Remark"]}','',null,'F','{Env.User.UserID}',GETDATE())
";
            sqlinsert += $@"insert IssueLack_Detail(ID,FtyInventoryukey,Qty,MDivisionID,POID,Seq1,Seq2, Roll, Dyelot, StockType) select * from #tmp";
            DataTable a;
            using (TransactionScope scope = new TransactionScope())
            {
                DualResult upResult;
                if (!MyUtility.Check.Empty(sqlinsert))
                {
                    if (!(upResult = MyUtility.Tool.ProcessWithDatatable(ldt, string.Empty, sqlinsert, out a)))
                    {
                        this.ShowErr(upResult);
                        return;
                    }
                }

                scope.Complete();
            }

            MyUtility.Msg.InfoBox("Complete!");
        }
    }
}
