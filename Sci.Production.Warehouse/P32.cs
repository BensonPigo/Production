using Ict;
using Ict.Win;
using Microsoft.Reporting.WinForms;
using Sci.Data;
using Sci.Production.Automation;
using Sci.Production.Automation.LogicLayer;
using Sci.Production.Prg.Entity;
using Sci.Production.PublicPrg;
using Sci.Win;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P32 : Win.Tems.Input6
    {
        private Dictionary<string, string> di_stocktype = new Dictionary<string, string>();
        private ReportViewer viewer;

        /// <inheritdoc/>
        public P32(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.InsertDetailGridOnDoubleClick = false;
            this.viewer = new ReportViewer
            {
                Dock = DockStyle.Fill,
            };
            this.Controls.Add(this.viewer);

            // MDivisionID 是 P32 寫入 => Sci.Env.User.Keyword
            this.DefaultFilter = string.Format("Type='B' and MDivisionID = '{0}'", Env.User.Keyword);
            this.gridicon.Append.Enabled = false;
            this.gridicon.Append.Visible = false;
            this.gridicon.Insert.Enabled = false;
            this.gridicon.Insert.Visible = false;
            this.di_stocktype.Add("B", "Bulk");
            this.di_stocktype.Add("I", "Inventory");
        }

        /// <inheritdoc/>
        public P32(ToolStripMenuItem menuitem, string transID)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = string.Format("Type='B' and id='{0}'", transID);
            this.IsSupportNew = false;
            this.IsSupportEdit = false;
            this.IsSupportDelete = false;
            this.IsSupportConfirm = false;
            this.IsSupportUnconfirm = false;
            this.gridicon.Append.Enabled = false;
            this.gridicon.Append.Visible = false;
            this.gridicon.Insert.Enabled = false;
            this.gridicon.Insert.Visible = false;
            this.di_stocktype.Add("B", "Bulk");
            this.di_stocktype.Add("I", "Inventory");
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["MDivisionID"] = Env.User.Keyword;
            this.CurrentMaintain["FactoryID"] = Env.User.Factory;
            this.CurrentMaintain["Status"] = "New";
            this.CurrentMaintain["Type"] = "B";
            this.CurrentMaintain["IssueDate"] = DateTime.Now;
        }

        /// <inheritdoc/>
        protected override bool ClickDeleteBefore()
        {
            if (this.CurrentMaintain["Status"].EqualString("CONFIRMED"))
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't delete.", "Warning");
                return false;
            }

            return base.ClickDeleteBefore();
        }

        /// <inheritdoc/>
        protected override bool ClickEditBefore()
        {
            if (this.CurrentMaintain["Status"].EqualString("CONFIRMED"))
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't modify.", "Warning");
                return false;
            }

            return base.ClickEditBefore();
        }

        /// <inheritdoc/>
        protected override bool ClickPrint()
        {
            // DataRow dr = grid.GetDataRow<DataRow>(grid.GetSelectedRowIndex());
            if (this.CurrentMaintain["status"].ToString().ToUpper() != "CONFIRMED")
            {
                MyUtility.Msg.WarningBox("Data is not confirmed, can't print.", "Warning");
                return false;
            }

            WH_FromTo_Print p = new WH_FromTo_Print(this.CurrentMaintain, "P32")
            {
                CurrentDataRow = this.CurrentMaintain,
            };

            p.ShowDialog();

            // 代表要列印 RDLC
            if (p.IsPrintRDLC)
            {
                DataRow row = this.CurrentMaintain;
                string id = row["ID"].ToString();
                string remark = row["Remark"].ToString();
                string m = this.CurrentMaintain["MdivisionID"].ToString();
                string issuedate = ((DateTime)MyUtility.Convert.GetDate(row["issuedate"])).ToShortDateString();
                #region -- 撈表頭資料 --
                List<SqlParameter> pars = new List<SqlParameter>
            {
                new SqlParameter("@ID", id),
            };
                string sqlcmd = @"
select b.nameEN 
from dbo.Borrowback  a WITH (NOLOCK) 
inner join dbo.mdivision  b WITH (NOLOCK) 
on b.id = a.mdivisionid
where b.id = a.mdivisionid
and a.id = @ID";
                DualResult result1 = DBProxy.Current.Select(string.Empty, sqlcmd, pars, out DataTable dt1);
                if (result1 == false)
                {
                    this.ShowErr(result1);
                }

                if (dt1 == null || dt1.Rows.Count == 0)
                {
                    MyUtility.Msg.InfoBox("Data not found!!", "DataTable dt1");
                    return false;
                }

                string rptTitle = dt1.Rows[0]["nameEN"].ToString();
                ReportDefinition report = new ReportDefinition();
                report.ReportParameters.Add(new ReportParameter("RptTitle", rptTitle));
                report.ReportParameters.Add(new ReportParameter("ID", id));
                report.ReportParameters.Add(new ReportParameter("Remark", remark));
                report.ReportParameters.Add(new ReportParameter("issuedate", issuedate));
                report.ReportParameters.Add(new ReportParameter("Factory", m));
                pars = new List<SqlParameter>
            {
                new SqlParameter("@ID", id),
            };

                #endregion
                #region -- 撈表身資料 --
                #endregion

                #region -- 撈表身資料 --
                sqlcmd = @"
select  StockSEQ = t.frompoid+' '+(t.fromseq1 + '-' +t.fromseq2) 
        ,ToSP = t.topoid+' '+(t.toseq1  + '-' +t.toseq2) 
        ,[desc] = IIF((p.ID = lag(p.ID,1,'')over (order by p.ID,p.seq1,p.seq2) 
                          AND(p.seq1 = lag(p.seq1,1,'')over (order by p.ID,p.seq1,p.seq2))
                          AND(p.seq2 = lag(p.seq2,1,'')over (order by p.ID,p.seq1,p.seq2))) 
                      ,''
                      ,dbo.getMtlDesc(t.FromPOID,t.FromSeq1,t.FromSeq2,2,0))
        ,FromStock = case t.FromStockType
                        WHEN 'B'THEN 'Bulk'
                        WHEN 'I'THEN 'Inventory'
                        ELSE t.FromStockType end 
        ,ToStock = case t.TostockType
                        WHEN 'B'THEN 'Bulk'
                        WHEN 'I'THEN 'Inventory'
                        ELSE t.FromStockType end 
        ,[Location] = dbo.Getlocation(fi.ukey) 
        ,FI.ContainerCode
        ,p.StockUnit
        ,t.fromroll
        ,t.fromdyelot
        ,t.Qty
        ,[Total]=sum(t.Qty) OVER (PARTITION BY t.frompoid ,t.FromSeq1,t.FromSeq2 )           
from dbo.Borrowback_detail t WITH (NOLOCK) 
left join dbo.PO_Supp_Detail p WITH (NOLOCK) on p.id= t.FromPOID and p.SEQ1 = t.FromSeq1 and p.seq2 = t.FromSeq2 
left join dbo.FtyInventory FI on t.fromPoid = fi.poid and t.fromSeq1 = fi.seq1 and t.fromSeq2 = fi.seq2 and t.fromDyelot = fi.Dyelot
    and t.fromRoll = fi.roll and t.fromStocktype = fi.stocktype
where t.id= @ID";
                result1 = DBProxy.Current.Select(string.Empty, sqlcmd, pars, out DataTable dtDetail);
                if (!result1)
                {
                    this.ShowErr(sqlcmd, result1);
                }

                if (dtDetail == null || dtDetail.Rows.Count == 0)
                {
                    MyUtility.Msg.InfoBox("Data not found!!", "DataTable dtDetail");
                    return false;
                }

                // 傳 list 資料
                List<P32_PrintData> data = dtDetail.AsEnumerable()
                    .Select(row1 => new P32_PrintData()
                    {
                        StockSEQ = row1["StockSEQ"].ToString().Trim(),
                        ToSP = row1["ToSP"].ToString().Trim(),
                        DESC = row1["desc"].ToString().Trim(),
                        FromStock = row1["FromStock"].ToString().Trim(),
                        ToStock = row1["ToStock"].ToString().Trim(),
                        Location = row1["Location"].ToString().Trim() + Environment.NewLine + row1["ContainerCode"].ToString().Trim(),
                        Unit = row1["StockUnit"].ToString().Trim(),
                        Roll = row1["fromroll"].ToString().Trim(),
                        DYELOT = row1["fromdyelot"].ToString().Trim(),
                        QTY = row1["Qty"].ToString().Trim(),
                        TotalQTY = row1["Total"].ToString().Trim(),
                    }).ToList();

                report.ReportDataSource = data;
                #endregion

                // 指定是哪個 RDLC
                // DualResult result;
                Type reportResourceNamespace = typeof(P32_PrintData);
                Assembly reportResourceAssembly = reportResourceNamespace.Assembly;
                string reportResourceName = "P32_Print.rdlc";

                if (!(result1 = ReportResources.ByEmbeddedResource(reportResourceAssembly, reportResourceNamespace, reportResourceName, out IReportResource reportresource)))
                {
                    // this.ShowException(result);
                    return false;
                }

                report.ReportResource = reportresource;

                // 開啟 report view
                var frm = new Win.Subs.ReportView(report)
                {
                    MdiParent = this.MdiParent,
                };
                frm.Show();
            }

            return true;
        }

        private void MySubreportEventHandler(object sender, SubreportProcessingEventArgs e)
        {
            e.DataSources.Add(new ReportDataSource("DataSet1", (DataTable)this.detailgridbs.DataSource));
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            StringBuilder warningmsg = new StringBuilder();

            #region 必輸檢查

            if (MyUtility.Check.Empty(this.CurrentMaintain["IssueDate"]))
            {
                MyUtility.Msg.WarningBox("< Issue Date >  can't be empty!", "Warning");
                this.dateIssueDate.Focus();
                return false;
            }

            if (!MyUtility.Check.Empty(warningmsg.ToString()))
            {
                MyUtility.Msg.WarningBox(warningmsg.ToString());
                return false;
            }

            #endregion 必輸檢查

            foreach (DataRow row in this.DetailDatas)
            {
                if (MyUtility.Check.Empty(row["fromseq1"]) || MyUtility.Check.Empty(row["fromseq2"]))
                {
                    warningmsg.Append($@"SP#: {row["frompoid"]} Seq#: {row["fromseq1"]}-{row["fromseq2"]} can't be empty"
                        + Environment.NewLine);
                }

                if (MyUtility.Check.Empty(row["Qty"]))
                {
                    warningmsg.Append($@"SP#: {row["frompoid"]} Seq#: {row["fromseq1"]}-{row["fromseq2"]} Roll#:{row["fromroll"]} Dyelot:{row["fromdyelot"]} Qty can't be empty" + Environment.NewLine);
                }

                if (row["fabrictype"].ToString().ToUpper() == "F" && (MyUtility.Check.Empty(row["Toroll"]) || MyUtility.Check.Empty(row["Todyelot"])))
                {
                    warningmsg.Append($@"To SP#: {row["topoid"]} To Seq#: {row["toseq1"]}-{row["toseq2"]} To Roll#:{row["toroll"]} To Dyelot:{row["todyelot"]} Roll and Dyelot can't be empty" + Environment.NewLine);
                }

                if (row["fabrictype"].ToString().ToUpper() != "F")
                {
                    row["toroll"] = string.Empty;
                    row["todyelot"] = string.Empty;
                }
            }

            if (!MyUtility.Check.Empty(warningmsg.ToString()))
            {
                MyUtility.Msg.WarningBox(warningmsg.ToString());
                return false;
            }

            if (this.DetailDatas.Count == 0)
            {
                MyUtility.Msg.WarningBox("Detail can't be empty", "Warning");
                return false;
            }

            if (!this.CheckRoll())
            {
                return false;
            }

            // 取單號
            if (this.IsDetailInserting)
            {
                string tmpId = MyUtility.GetValue.GetID(Env.User.Keyword + "RB", "BorrowBack", (DateTime)this.CurrentMaintain["Issuedate"]);
                if (MyUtility.Check.Empty(tmpId))
                {
                    MyUtility.Msg.WarningBox("Get document ID fail!!");
                    return false;
                }

                this.CurrentMaintain["id"] = tmpId;
            }

            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override DualResult OnRenewDataDetailPost(RenewDataPostEventArgs e)
        {
            return base.OnRenewDataDetailPost(e);
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            #region -- Status Label --

            this.label25.Text = this.CurrentMaintain["status"].ToString();

            #endregion Status Label
            string tmp = MyUtility.GetValue.Lookup(string.Format("select estbackdate from borrowback WITH (NOLOCK) where id='{0}'", this.CurrentMaintain["borrowid"]));
            this.dateEstReturnDate.Value = null;
            if (!MyUtility.Check.Empty(tmp))
            {
                this.dateEstReturnDate.Value = DateTime.Parse(tmp);
            }

            // System.Automation=1 和confirmed 且 有P99 Use 權限的人才可以看到此按紐
            if (UtilityAutomation.IsAutomationEnable && (this.CurrentMaintain["Status"].ToString().ToUpper() == "CONFIRMED") &&
                MyUtility.Check.Seek($@"
select * from Pass1
where (FKPass0 in (select distinct FKPass0 from Pass2 where BarPrompt = 'P99. Send to WMS command Status' and Used = 'Y') or IsMIS = 1 or IsAdmin = 1)
and ID = '{Sci.Env.User.UserID}'"))
            {
                this.btnCallP99.Visible = true;
            }
            else
            {
                this.btnCallP99.Visible = false;
            }
        }

        /// <inheritdoc/>
        protected override void OnDetailGridInsert(int index = -1)
        {
            base.OnDetailGridInsert(index);
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            DataGridViewGeneratorTextColumnSettings toLocation = new DataGridViewGeneratorTextColumnSettings();
            toLocation.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);

                    SelectItem2 selectItem2 = Prgs.SelectLocation("B", MyUtility.Convert.GetString(dr["ToLocation"]));

                    selectItem2.ShowDialog();
                    if (selectItem2.DialogResult == DialogResult.OK)
                    {
                        dr["ToLocation"] = selectItem2.GetSelecteds().Select(o => MyUtility.Convert.GetString(o["ID"])).JoinToString(",");
                    }

                    dr.EndEdit();
                }
            };

            toLocation.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    string oldValue = dr["ToLocation"].ToString();
                    string newValue = e.FormattedValue.ToString().Split(',').ToList().Where(o => !MyUtility.Check.Empty(o)).Distinct().JoinToString(",");
                    if (oldValue.Equals(newValue))
                    {
                        return;
                    }

                    string notLocationExistsList = newValue.Split(',').ToList().Where(o => !Prgs.CheckLocationExists("B", o)).JoinToString(",");

                    if (!MyUtility.Check.Empty(notLocationExistsList))
                    {
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox($"ToLocation<{notLocationExistsList}> not Found");
                        return;
                    }
                    else
                    {
                        dr["ToLocation"] = newValue;
                        dr.EndEdit();
                    }
                }
            };

            Ict.Win.UI.DataGridViewTextBoxColumn from_ContainerCode;
            Ict.Win.UI.DataGridViewTextBoxColumn to_ContainerCode;

            #region 欄位設定
            this.Helper.Controls.Grid.Generator(this.detailgrid)
            .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
            .Text("frompoid", header: "From SP#", width: Widths.AnsiChars(13), iseditingreadonly: true) // 0
            .Text("fromseq", header: "From" + Environment.NewLine + "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true) // 1
            .Text("fromroll", header: "From" + Environment.NewLine + "Roll", width: Widths.AnsiChars(6), iseditingreadonly: true) // 2
            .Text("fromdyelot", header: "From" + Environment.NewLine + "Dyelot", width: Widths.AnsiChars(6), iseditingreadonly: true) // 3
            .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true) // 4
            .ComboBox("fromstocktype", header: "From" + Environment.NewLine + "Stock" + Environment.NewLine + "Type", iseditable: false).Get(out Ict.Win.UI.DataGridViewComboBoxColumn cbb_stocktype) // 5
            .Text("Location", header: "From" + Environment.NewLine + "Location", iseditingreadonly: true) // 6
            .Text("FromContainerCode", header: "From" + Environment.NewLine + "Container Code", iseditingreadonly: true).Get(out from_ContainerCode)
            .Text("topoid", header: "To" + Environment.NewLine + "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true) // 7
            .Text("toseq", header: "To" + Environment.NewLine + "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true) // 8
            .Text("toroll", header: "To" + Environment.NewLine + "Roll", width: Widths.AnsiChars(6)) // 9
            .Text("todyelot", header: "To" + Environment.NewLine + "Dyelot", width: Widths.AnsiChars(6)) // 10
            .Numeric("qty", header: "Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10) // 11
            .Text("ToLocation", header: "To Location", width: Widths.AnsiChars(10), settings: toLocation)
            .Text("ToContainerCode", header: "To" + Environment.NewLine + "Container Code", iseditingreadonly: true).Get(out to_ContainerCode)
            .Text("stockunit", header: "Stock" + Environment.NewLine + "Unit", iseditingreadonly: true, width: Widths.AnsiChars(5)) // 12
            .ComboBox("tostocktype", header: "To" + Environment.NewLine + "Stock" + Environment.NewLine + "Type", iseditable: false).Get(out Ict.Win.UI.DataGridViewComboBoxColumn cbb_stocktype2) // 13
            ;
            #endregion 欄位設定

            // 僅有自動化工廠 ( System.Automation = 1 )才需要顯示該欄位 by ISP20220035
            from_ContainerCode.Visible = Automation.UtilityAutomation.IsAutomationEnable;
            to_ContainerCode.Visible = Automation.UtilityAutomation.IsAutomationEnable;
            this.detailgrid.Columns["toroll"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["todyelot"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["qty"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["ToLocation"].DefaultCellStyle.BackColor = Color.Pink;

            cbb_stocktype.DataSource = new BindingSource(this.di_stocktype, null);
            cbb_stocktype.ValueMember = "Key";
            cbb_stocktype.DisplayMember = "Value";

            cbb_stocktype2.DataSource = new BindingSource(this.di_stocktype, null);
            cbb_stocktype2.ValueMember = "Key";
            cbb_stocktype2.DisplayMember = "Value";
        }

        /// <inheritdoc/>
        protected override void ClickConfirm()
        {
            this.RenewData(); // 先重載資料, 避免雙開程式狀況
            base.ClickConfirm();
            if (this.CurrentMaintain == null)
            {
                return;
            }

            // 取得 FtyInventory 資料 (包含PO_Supp_Detail.FabricType)
            DualResult result = Prgs.GetFtyInventoryData((DataTable)this.detailgridbs.DataSource, this.Name, out DataTable dtOriFtyInventory);
            string ids = string.Empty;

            string backdate = MyUtility.GetValue.Lookup(string.Format(
                @"select [backdate] from dbo.borrowback WITH (NOLOCK) where id='{0}' and type='A'",
                this.CurrentMaintain["borrowid"]));
            if (!MyUtility.Check.Empty(backdate))
            {
                MyUtility.Msg.WarningBox(string.Format("This borrow id ({0}) already returned.", this.CurrentMaintain["borrowid"]), "Can't Confirmed");
                return;
            }

            // 檢查 Barcode不可為空
            if (!Prgs.CheckBarCode(dtOriFtyInventory, this.Name))
            {
                return;
            }

            #region 檢查物料Location 是否存在WMS
            if (!PublicPrg.Prgs.Chk_WMS_Location(this.CurrentMaintain["ID"].ToString(), "P32"))
            {
                return;
            }
            #endregion

            #region -- 檢查庫存項lock --
            string sqlcmd = string.Format(
                @"
Select  d.frompoid
        ,d.fromseq1
        ,d.fromseq2
        ,d.fromRoll
        ,d.Qty
        ,balanceQty = isnull(f.InQty,0) - isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - isnull(f.ReturnQty,0)
        ,f.Dyelot
from dbo.BorrowBack_Detail d WITH (NOLOCK) 
inner join FtyInventory f WITH (NOLOCK) on d.FromPOID = f.POID  AND D.FromStockType = F.StockType
    and d.FromRoll = f.Roll and d.FromSeq1 =f.Seq1 and d.FromSeq2 = f.Seq2 and d.fromDyelot = f.Dyelot
where f.lock=1 and d.Id = '{0}'", this.CurrentMaintain["id"]);
            if (!(result = DBProxy.Current.Select(null, sqlcmd, out DataTable datacheck)))
            {
                this.ShowErr(sqlcmd, result);
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    foreach (DataRow tmp in datacheck.Rows)
                    {
                        ids += $"SP#: {tmp["frompoid"]} Seq#: {tmp["fromseq1"]}-{tmp["fromseq2"]} Roll#: {tmp["fromroll"]} Dyelot: {tmp["Dyelot"]} is locked!!" + Environment.NewLine;
                    }

                    MyUtility.Msg.WarningBox("Material Locked!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }
            #endregion

            #region 檢查庫存項WMSLock
            if (!Prgs.ChkWMSLock(this.CurrentMaintain["id"].ToString(), "BorrowBack_Detail_From"))
            {
                return;
            }
            #endregion

            #region 檢查From/To Location是否為空值
            DataTable dtDetail = (DataTable)this.detailgridbs.DataSource;

            if (MyUtility.Check.Seek(@"select * from System where WH_MtlTransChkLocation = 1"))
            {
                // From Location
                DataRow[] dtArry = dtDetail.Select(@"Fromlocation = '' or Fromlocation is null");
                if (dtArry != null && dtArry.Length > 0)
                {
                    DataTable dtFromLocation_Empty = dtArry.CopyToDataTable();

                    // change column name
                    dtFromLocation_Empty.Columns["FromPoId"].ColumnName = "From SP#";
                    dtFromLocation_Empty.Columns["Fromseq"].ColumnName = "From Seq";
                    dtFromLocation_Empty.Columns["FromRoll"].ColumnName = "From Roll";
                    dtFromLocation_Empty.Columns["FromDyelot"].ColumnName = "From Dyelot";
                    dtFromLocation_Empty.Columns["fromstocktype"].ColumnName = "From Stock Type";
                    dtFromLocation_Empty.Columns["topoid"].ColumnName = "To SP#";
                    dtFromLocation_Empty.Columns["toroll"].ColumnName = "To Roll";
                    dtFromLocation_Empty.Columns["todyelot"].ColumnName = "To Dyelot";

                    Prgs.ChkLocationEmpty(dtFromLocation_Empty, "From", @"From SP#,From Seq,From Roll,From Dyelot,From Stock Type,To SP#,To Seq,To Roll,To Dyelot");
                    return;
                }

                // To Location
                dtArry = dtDetail.Select(@"ToLocation = '' or ToLocation is null");
                if (dtArry != null && dtArry.Length > 0)
                {
                    DataTable dtToLocation_Empty = dtArry.CopyToDataTable();

                    // change column name
                    dtToLocation_Empty.Columns["FromPoId"].ColumnName = "From SP#";
                    dtToLocation_Empty.Columns["Fromseq"].ColumnName = "From Seq";
                    dtToLocation_Empty.Columns["FromRoll"].ColumnName = "From Roll";
                    dtToLocation_Empty.Columns["FromDyelot"].ColumnName = "From Dyelot";
                    dtToLocation_Empty.Columns["fromstocktype"].ColumnName = "From Stock Type";
                    dtToLocation_Empty.Columns["topoid"].ColumnName = "To SP#";
                    dtToLocation_Empty.Columns["toroll"].ColumnName = "To Roll";
                    dtToLocation_Empty.Columns["todyelot"].ColumnName = "To Dyelot";

                    Prgs.ChkLocationEmpty(dtToLocation_Empty, "To", @"From SP#,From Seq,From Roll,From Dyelot,From Stock Type,To SP#,To Seq,To Roll,To Dyelot");
                    return;
                }
            }
            #endregion

            #region -- 檢查負數庫存 --

            sqlcmd = string.Format(
                @"
Select  d.frompoid
        ,d.fromseq1
        ,d.fromseq2
        ,d.fromRoll
        ,d.Qty
        ,balanceQty = isnull(f.InQty,0) - isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - isnull(f.ReturnQty,0)
        ,f.Dyelot
from dbo.BorrowBack_Detail d WITH (NOLOCK) 
left join FtyInventory f WITH (NOLOCK) on d.FromPOID = f.POID  AND D.FromStockType = F.StockType
    and d.FromRoll = f.Roll and d.FromSeq1 =f.Seq1 and d.FromSeq2 = f.Seq2 and d.fromDyelot = f.Dyelot
where (isnull(f.InQty,0) - isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - isnull(f.ReturnQty,0) - d.Qty < 0) 
    and d.Id = '{0}'", this.CurrentMaintain["id"]);
            if (!(result = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                this.ShowErr(sqlcmd, result);
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    foreach (DataRow tmp in datacheck.Rows)
                    {
                        ids += $"SP#: {tmp["frompoid"]} Seq#: {tmp["fromseq1"]}-{tmp["fromseq2"]} Roll#: {tmp["fromroll"]}'s balance: {tmp["balanceqty"]} is less than qty: {tmp["qty"]}" + Environment.NewLine;
                    }

                    MyUtility.Msg.WarningBox("Balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }

            #endregion -- 檢查負數庫存 --

            #region -- 檢查目的Roll是否已存在資料 --
            if (!this.CheckRoll())
            {
                return;
            }
            #endregion

            #region -- 更新mdivisionPoDetail 還出數 --
            var bs1 = (from b in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
                       group b by new
                       {
                           poid = b.Field<string>("frompoid").Trim(),
                           seq1 = b.Field<string>("fromseq1").Trim(),
                           seq2 = b.Field<string>("fromseq2").Trim(),
                           stocktype = b.Field<string>("fromstocktype").Trim(),
                       }
                        into m
                       select new
                       {
                           poid = m.First().Field<string>("frompoid"),
                           Seq1 = m.First().Field<string>("fromseq1"),
                           Seq2 = m.First().Field<string>("fromseq2"),
                           Stocktype = m.First().Field<string>("fromstocktype"),
                           Qty = m.Sum(w => w.Field<decimal>("qty")),
                           Location = string.Join(",", m.Select(r => r.Field<string>("Tolocation")).Distinct()),
                       }).ToList();
            var bs1I = (from b in ((DataTable)this.detailgridbs.DataSource).AsEnumerable().Where(w => w.Field<string>("fromstocktype").Trim() == "I")
                        group b by new
                        {
                            poid = b.Field<string>("frompoid").Trim(),
                            seq1 = b.Field<string>("fromseq1").Trim(),
                            seq2 = b.Field<string>("fromseq2").Trim(),
                            stocktype = b.Field<string>("fromstocktype").Trim(),
                        }
                        into m
                        select new Prgs_POSuppDetailData
                        {
                            Poid = m.First().Field<string>("frompoid"),
                            Seq1 = m.First().Field<string>("fromseq1"),
                            Seq2 = m.First().Field<string>("fromseq2"),
                            Stocktype = m.First().Field<string>("fromstocktype"),
                            Qty = m.Sum(w => w.Field<decimal>("qty")),
                            Location = string.Join(",", m.Select(r => r.Field<string>("Tolocation")).Distinct()),
                        }).ToList();
            #endregion
            #region -- 更新mdivisionPoDetail 還回數 --
            var bs2 = (from b in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
                       group b by new
                       {
                           poid = b.Field<string>("topoid").Trim(),
                           seq1 = b.Field<string>("toseq1").Trim(),
                           seq2 = b.Field<string>("toseq2").Trim(),
                           stocktype = b.Field<string>("tostocktype").Trim(),
                       }
                    into m
                       select new Prgs_POSuppDetailData
                       {
                           Poid = m.First().Field<string>("topoid"),
                           Seq1 = m.First().Field<string>("toseq1"),
                           Seq2 = m.First().Field<string>("toseq2"),
                           Stocktype = m.First().Field<string>("tostocktype"),
                           Qty = m.Sum(w => w.Field<decimal>("qty")),
                           Location = string.Join(",", m.Select(r => r.Field<string>("Tolocation")).Distinct()),
                       }).ToList();
            #endregion

            #region -- 更新庫存數量  ftyinventory --
            var bsfio = (from m in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
                         select new
                         {
                             poid = m.Field<string>("frompoid"),
                             seq1 = m.Field<string>("fromseq1"),
                             seq2 = m.Field<string>("fromseq2"),
                             stocktype = m.Field<string>("fromstocktype"),
                             qty = m.Field<decimal>("qty"),
                             location = m.Field<string>("location"),
                             roll = m.Field<string>("fromroll"),
                             dyelot = m.Field<string>("fromdyelot"),
                         }).ToList();
            var bsfioto = (from m in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
                           select new
                           {
                               poid = m.Field<string>("topoid"),
                               seq1 = m.Field<string>("toseq1"),
                               seq2 = m.Field<string>("toseq2"),
                               stocktype = m.Field<string>("tostocktype"),
                               qty = m.Field<decimal>("qty"),
                               location = m.Field<string>("location"),
                               roll = m.Field<string>("toroll"),
                               dyelot = m.Field<string>("todyelot"),
                           }).ToList();
            string sqlupd2_FIO = Prgs.UpdateFtyInventory_IO(4, null, false);
            string sqlupd2_FIO2 = Prgs.UpdateFtyInventory_IO(2, null, false);

            #endregion 更新庫存數量  ftyinventory

            #region 更新庫存位置  ftyinventory_detail.MtlLocationID

            sqlcmd = $@"
Select d.ToPOID ,d.ToSeq1 ,d.ToSeq2 ,d.ToRoll ,d.ToDyelot ,d.ToStockType ,[ToLocation]=d.ToLocation
from dbo.BorrowBack_detail d WITH (NOLOCK) 
where d.Id = '{this.CurrentMaintain["id"]}'";
            DBProxy.Current.Select(null, sqlcmd, out DataTable locationTable);

            var data_Fty_26F = (from b in locationTable.AsEnumerable()
                                select new
                                {
                                    poid = b.Field<string>("ToPOID"),
                                    seq1 = b.Field<string>("toseq1"),
                                    seq2 = b.Field<string>("toseq2"),
                                    stocktype = b.Field<string>("tostocktype"),
                                    toLocation = b.Field<string>("ToLocation"),
                                    roll = b.Field<string>("toroll"),
                                    dyelot = b.Field<string>("todyelot"),
                                }).ToList();

            string upd_Fty_26F = Prgs.UpdateFtyInventory_IO(27, null, false);

            #endregion

            #region -- 更新全數歸還日期 --
            StringBuilder sqlupd2 = new StringBuilder();
            sqlupd2.Append(string.Format(
                @"
;declare @reccount as int;

with acc as(
    select  bd1.ToPoid
            ,bd1.ToSeq1
            ,bd1.ToSeq2
			,bd1.FromFactoryID
            ,qty = sum(bd1.qty) 
    from dbo.BorrowBack b1 WITH (NOLOCK) 
    inner join dbo.BorrowBack_Detail bd1 WITH (NOLOCK) on b1.id = bd1.id 
    where b1.BorrowId='{1}' and b1.Status = 'Confirmed'
    group by bd1.ToPoid,bd1.ToSeq1,bd1.ToSeq2,bd1.FromFactoryID
    )
, borrow as(
    select  bd.FromPoId
            ,bd.FromSeq1
            ,bd.FromSeq2
			,bd.ToFactoryID
            ,borrowedqty = sum(bd.Qty) 
    from dbo.BorrowBack_Detail bd WITH (NOLOCK) 
    left join PO_Supp_Detail p WITH (NOLOCK) on p.id = bd.FromPoId and p.SEQ1 = bd.FromSeq1 and p.SEQ2 = bd.FromSeq2
    where bd.id='{1}'
    group by bd.FromPoId, bd.FromSeq1, bd.FromSeq2,bd.ToFactoryID
    )
select @reccount = count(*)
from borrow 
left join acc on borrow.FromPoId = acc.ToPoid and borrow.FromSeq1 = acc.ToSeq1 and borrow.FromSeq2 = acc.ToSeq2 and borrow.ToFactoryID=acc.FromFactoryID
where borrowedqty > isnull(acc.qty,0.00);

if @reccount = 0 
    begin
        update dbo.BorrowBack 
            set BackDate = '{2}' 
        where id = '{1}'
    end 
else
    begin
        update dbo.BorrowBack 
            set BackDate = DEFAULT 
        where id = '{1}'
    end", this.CurrentMaintain["id"].ToString(),
                this.CurrentMaintain["borrowid"],
                DateTime.Parse(this.CurrentMaintain["issuedate"].ToString()).ToShortDateString()));
            #endregion

            Exception errMsg = null;
            using (TransactionScope transactionscope = new TransactionScope())
            {
                try
                {
                    DataTable resulttb;
                    if (bs1.Count > 0)
                    {
                        string sqlupd2_B = Prgs.UpdateMPoDetail(4, null, true);
                        if (!(result = MyUtility.Tool.ProcessWithObject(bs1, string.Empty, sqlupd2_B, out resulttb, "#TmpSource")))
                        {
                            throw result.GetException();
                        }
                    }

                    if (bs1I.Count > 0)
                    {
                        string sqlupd2_BI = Prgs.UpdateMPoDetail(8, bs1I, true);
                        if (!(result = MyUtility.Tool.ProcessWithObject(bs1I, string.Empty, sqlupd2_BI, out resulttb, "#TmpSource")))
                        {
                            throw result.GetException();
                        }
                    }

                    if (!(result = MyUtility.Tool.ProcessWithObject(bsfio, string.Empty, sqlupd2_FIO, out resulttb, "#TmpSource")))
                    {
                        throw result.GetException();
                    }

                    if (!(result = MyUtility.Tool.ProcessWithObject(bsfioto, string.Empty, sqlupd2_FIO2, out resulttb, "#TmpSource")))
                    {
                        throw result.GetException();
                    }

                    if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_26F, string.Empty, upd_Fty_26F, out resulttb, "#TmpSource")))
                    {
                        throw result.GetException();
                    }

                    if (!(result = DBProxy.Current.Execute(null, $"update BorrowBack set status='Confirmed', editname = '{Env.User.UserID}', editdate = GETDATE() where id = '{this.CurrentMaintain["id"]}'")))
                    {
                        throw result.GetException();
                    }

                    if (!(result = DBProxy.Current.Execute(null, sqlupd2.ToString())))
                    {
                        throw result.GetException();
                    }

                    string sqlupd2_A = Prgs.UpdateMPoDetail(2, bs2, true);
                    if (!(result = MyUtility.Tool.ProcessWithObject(bs2, string.Empty, sqlupd2_A, out resulttb, "#TmpSource")))
                    {
                        throw result.GetException();
                    }

                    // Barcode 需要判斷新的庫存, 在更新 FtyInventory 之後
                    if (!(result = Prgs.UpdateWH_Barcode(true, (DataTable)this.detailgridbs.DataSource, this.Name, out bool fromNewBarcode, dtOriFtyInventory)))
                    {
                        throw result.GetException();
                    }

                    if (!(result = Prgs.UpdateFtyInventoryTone((DataTable)this.detailgridbs.DataSource)))
                    {
                        throw result.GetException();
                    }

                    transactionscope.Complete();
                }
                catch (Exception ex)
                {
                    errMsg = ex;
                }
            }

            if (!MyUtility.Check.Empty(errMsg))
            {
                this.ShowErr(errMsg);
                return;
            }

            // AutoWHFabric WebAPI
            Prgs_WMS.WMSprocess(false, (DataTable)this.detailgridbs.DataSource, this.Name, EnumStatus.New, EnumStatus.Confirm, dtOriFtyInventory);
            MyUtility.Msg.InfoBox("Confirmed successful");
        }

        /// <inheritdoc/>
        protected override void ClickUnconfirm()
        {
            this.RenewData(); // 先重載資料, 避免雙開程式狀況
            base.ClickUnconfirm();
            if (this.CurrentMaintain == null ||
                MyUtility.Msg.QuestionBox("Do you want to unconfirme it?") == DialogResult.No)
            {
                return;
            }

            // 取得 FtyInventory 資料
            DualResult result = Prgs.GetFtyInventoryData((DataTable)this.detailgridbs.DataSource, this.Name, out DataTable dtOriFtyInventory);
            string ids = string.Empty;

            #region -- 檢查庫存項lock --
            string sqlcmd = string.Format(
                @"
Select  d.topoid
        ,d.toseq1
        ,d.toseq2
        ,d.toRoll
        ,d.Qty
        ,balanceQty = isnull(f.InQty,0) - isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - isnull(f.ReturnQty,0)
        ,f.Dyelot
from dbo.BorrowBack_Detail d WITH (NOLOCK) 
inner join FtyInventory f WITH (NOLOCK) 
    on d.toPoId = f.PoId
    and d.toSeq1 = f.Seq1
    and d.toSeq2 = f.seq2
    and d.toStocktype = f.StockType
    and d.toRoll = f.Roll
    and d.toDyelot = f.Dyelot
where f.lock=1 and d.Id = '{0}'", this.CurrentMaintain["id"]);
            if (!(result = DBProxy.Current.Select(null, sqlcmd, out DataTable datacheck)))
            {
                this.ShowErr(sqlcmd, result);
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    foreach (DataRow tmp in datacheck.Rows)
                    {
                        ids += $"SP#: {tmp["topoid"]} Seq#: {tmp["toseq1"]}-{tmp["toseq2"]} Roll#: {tmp["toroll"]} Dyelot: {tmp["Dyelot"]} is locked!!" + Environment.NewLine;
                    }

                    MyUtility.Msg.WarningBox("Material Locked!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }
            #endregion

            #region 檢查庫存項WMSLock
            if (!Prgs.ChkWMSLock(this.CurrentMaintain["id"].ToString(), "BorrowBack_Detail_To"))
            {
                return;
            }
            #endregion

            #region 檢查資料有任一筆WMS已完成, 就不能unConfirmed
            if (!Prgs.ChkWMSCompleteTime((DataTable)this.detailgridbs.DataSource, "BorrowBack_Detail_To"))
            {
                return;
            }
            #endregion

            #region -- 檢查負數庫存 --

            sqlcmd = string.Format(
                @"
Select  d.topoid
        ,d.toseq1
        ,d.toseq2
        ,d.toRoll
        ,d.Qty
        ,balanceQty = isnull(f.InQty,0) - isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - isnull(f.ReturnQty,0)
        ,f.Dyelot
from dbo.BorrowBack_Detail d WITH (NOLOCK) 
left join FtyInventory f WITH (NOLOCK) 
    on d.toPoId = f.PoId
    and d.toSeq1 = f.Seq1
    and d.toSeq2 = f.seq2
    and d.toStocktype = f.StockType
    and d.toRoll = f.Roll
    and d.toDyelot = f.Dyelot
where (isnull(f.InQty,0) - isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - isnull(f.ReturnQty,0) - d.Qty < 0) 
    and d.Id = '{0}'", this.CurrentMaintain["id"]);
            if (!(result = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                this.ShowErr(sqlcmd, result);
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    foreach (DataRow tmp in datacheck.Rows)
                    {
                        ids += $"SP#: {tmp["topoid"]} Seq#: {tmp["toseq1"]}-{tmp["toseq2"]} Roll#: {tmp["toroll"]} Dyelot: {tmp["Dyelot"]}'s balance: {tmp["balanceqty"]} is less than borrowed qty: {tmp["qty"]}" + Environment.NewLine;
                    }

                    MyUtility.Msg.WarningBox("Balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }

            #endregion 檢查負數庫存

            #region -- 更新MdivisionPoDetail 借出數 --
            var bs1 = (from b in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
                       group b by new
                       {
                           poid = b.Field<string>("frompoid").Trim(),
                           seq1 = b.Field<string>("fromseq1").Trim(),
                           seq2 = b.Field<string>("fromseq2").Trim(),
                           stocktype = b.Field<string>("fromstocktype").Trim(),
                       }
                        into m
                       select new
                       {
                           poid = m.First().Field<string>("frompoid"),
                           Seq1 = m.First().Field<string>("fromseq1"),
                           Seq2 = m.First().Field<string>("fromseq2"),
                           Stocktype = m.First().Field<string>("fromstocktype"),
                           Qty = -m.Sum(w => w.Field<decimal>("qty")),
                       }).ToList();
            var bs1I = (from b in ((DataTable)this.detailgridbs.DataSource).AsEnumerable().Where(w => w.Field<string>("fromstocktype") == "I")
                        group b by new
                        {
                            poid = b.Field<string>("frompoid").Trim(),
                            seq1 = b.Field<string>("fromseq1").Trim(),
                            seq2 = b.Field<string>("fromseq2").Trim(),
                            stocktype = b.Field<string>("fromstocktype").Trim(),
                        }
                        into m
                        select new Prgs_POSuppDetailData
                        {
                            Poid = m.First().Field<string>("frompoid"),
                            Seq1 = m.First().Field<string>("fromseq1"),
                            Seq2 = m.First().Field<string>("fromseq2"),
                            Stocktype = m.First().Field<string>("fromstocktype"),
                            Qty = -m.Sum(w => w.Field<decimal>("qty")),
                        }).ToList();
            #endregion
            #region -- 更新MdivisionPoDetail 借入數 --
            var bs2 = (from b in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
                       group b by new
                       {
                           poid = b.Field<string>("topoid").Trim(),
                           seq1 = b.Field<string>("toseq1").Trim(),
                           seq2 = b.Field<string>("toseq2").Trim(),
                           stocktype = b.Field<string>("tostocktype").Trim(),
                       }
                    into m
                       select new Prgs_POSuppDetailData
                       {
                           Poid = m.First().Field<string>("topoid"),
                           Seq1 = m.First().Field<string>("toseq1"),
                           Seq2 = m.First().Field<string>("toseq2"),
                           Stocktype = m.First().Field<string>("tostocktype"),
                           Qty = -m.Sum(w => w.Field<decimal>("qty")),
                       }).ToList();

            string sqlupd2_A = Prgs.UpdateMPoDetail(2, bs2, false);
            #endregion

            #region -- 更新庫存數量  ftyinventory --
            var bsfio = (from m in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
                         select new
                         {
                             poid = m.Field<string>("frompoid"),
                             seq1 = m.Field<string>("fromseq1"),
                             seq2 = m.Field<string>("fromseq2"),
                             stocktype = m.Field<string>("fromstocktype"),
                             qty = -m.Field<decimal>("qty"),
                             location = m.Field<string>("location"),
                             roll = m.Field<string>("fromroll"),
                             dyelot = m.Field<string>("fromdyelot"),
                         }).ToList();
            var bsfioto = (from m in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
                           select new
                           {
                               poid = m.Field<string>("topoid"),
                               seq1 = m.Field<string>("toseq1"),
                               seq2 = m.Field<string>("toseq2"),
                               stocktype = m.Field<string>("tostocktype"),
                               qty = -m.Field<decimal>("qty"),
                               location = m.Field<string>("location"),
                               roll = m.Field<string>("toroll"),
                               dyelot = m.Field<string>("todyelot"),
                           }).ToList();
            string sqlupd2_FIO = Prgs.UpdateFtyInventory_IO(4, null, false);
            string sqlupd2_FIO2 = Prgs.UpdateFtyInventory_IO(2, null, false);
            #endregion 更新庫存數量  ftyinventory

            #region -- 更新全數歸還日期 --
            StringBuilder sqlupd2 = new StringBuilder();
            sqlupd2.Append(string.Format(
                @"
;declare @reccount as int;

with acc as(
    select  bd1.ToPoid
            ,bd1.ToSeq1
            ,bd1.ToSeq2
			,bd1.FromFactoryID
            ,qty = sum(bd1.qty) 
    from dbo.BorrowBack b1 WITH (NOLOCK) 
    inner join dbo.BorrowBack_Detail bd1 WITH (NOLOCK) on b1.id = bd1.id 
    where b1.BorrowId='{1}' and b1.Status = 'Confirmed'
    group by bd1.ToPoid, bd1.ToSeq1, bd1.ToSeq2,bd1.FromFactoryID
    )
, borrow as(
    select  bd.FromPoId
            ,bd.FromSeq1
            ,bd.FromSeq2
			,bd.ToFactoryID
            ,borrowedqty = sum(bd.Qty) 
    from dbo.BorrowBack_Detail bd WITH (NOLOCK) 
    left join PO_Supp_Detail p WITH (NOLOCK) on p.id = bd.FromPoId and p.SEQ1 = bd.FromSeq1 and p.SEQ2 = bd.FromSeq2
    where bd.id='{1}'
    group by bd.FromPoId, bd.FromSeq1, bd.FromSeq2,bd.ToFactoryID
    )
select @reccount = count(*)
from borrow 
left join acc on borrow.FromPoId = acc.ToPoid and borrow.FromSeq1 = acc.ToSeq1 and borrow.FromSeq2 = acc.ToSeq2 and borrow.ToFactoryID=acc.FromFactoryID
where borrowedqty > isnull(acc.qty,0.00);

if @reccount = 0 
    begin
        update dbo.BorrowBack 
            set BackDate = '{2}' 
        where id = '{1}'
    end 
else
    begin
        update dbo.BorrowBack 
            set BackDate = DEFAULT 
        where id = '{1}'
    end", this.CurrentMaintain["id"].ToString(),
                this.CurrentMaintain["borrowid"],
                DateTime.Parse(this.CurrentMaintain["issuedate"].ToString()).ToShortDateString()));
            #endregion

            #region UnConfirmed 廠商能上鎖→PMS更新→廠商更新

            // 先確認 WMS 能否上鎖, 不能直接 return
            if (!Prgs_WMS.WMSLock((DataTable)this.detailgridbs.DataSource, dtOriFtyInventory, this.Name, EnumStatus.Unconfirm))
            {
                return;
            }

            // PMS 的資料更新
            Exception errMsg = null;
            List<AutoRecord> autoRecordList = new List<AutoRecord>();
            using (TransactionScope transactionscope = new TransactionScope())
            {
                try
                {
                    DataTable resulttb;
                    if (bs1.Count > 0)
                    {
                        string sqlupd2_B = Prgs.UpdateMPoDetail(4, null, false);
                        if (!(result = MyUtility.Tool.ProcessWithObject(bs1, string.Empty, sqlupd2_B, out resulttb, "#TmpSource")))
                        {
                            throw result.GetException();
                        }
                    }

                    if (bs1I.Count > 0)
                    {
                        string sqlupd2_BI = Prgs.UpdateMPoDetail(8, bs1I, false);
                        if (!(result = MyUtility.Tool.ProcessWithObject(bs1I, string.Empty, sqlupd2_BI, out resulttb, "#TmpSource")))
                        {
                            throw result.GetException();
                        }
                    }

                    if (!(result = MyUtility.Tool.ProcessWithObject(bs2, string.Empty, sqlupd2_A, out resulttb, "#TmpSource")))
                    {
                        throw result.GetException();
                    }

                    if (!(result = MyUtility.Tool.ProcessWithObject(bsfio, string.Empty, sqlupd2_FIO, out resulttb, "#TmpSource")))
                    {
                        throw result.GetException();
                    }

                    if (!(result = MyUtility.Tool.ProcessWithObject(bsfioto, string.Empty, sqlupd2_FIO2, out resulttb, "#TmpSource")))
                    {
                        throw result.GetException();
                    }

                    if (!(result = DBProxy.Current.Execute(null, $"update BorrowBack set status='New', editname = '{Env.User.UserID}', editdate = GETDATE() where id = '{this.CurrentMaintain["id"]}'")))
                    {
                        throw result.GetException();
                    }

                    if (!(result = DBProxy.Current.Execute(null, sqlupd2.ToString())))
                    {
                        throw result.GetException();
                    }

                    // Barcode 需要判斷新的庫存, 在更新 FtyInventory 之後
                    if (!(result = Prgs.UpdateWH_Barcode(false, (DataTable)this.detailgridbs.DataSource, this.Name, out bool fromNewBarcode, dtOriFtyInventory)))
                    {
                        throw result.GetException();
                    }

                    // transactionscope 內, 準備 WMS 資料 & 將資料寫入 AutomationCreateRecord (Delete, Unconfirm)
                    Prgs_WMS.WMSprocess(false, (DataTable)this.detailgridbs.DataSource, this.Name, EnumStatus.Delete, EnumStatus.Unconfirm, dtOriFtyInventory, typeCreateRecord: 1, autoRecord: autoRecordList);
                    transactionscope.Complete();
                }
                catch (Exception ex)
                {
                    errMsg = ex;
                }
            }

            if (!MyUtility.Check.Empty(errMsg))
            {
                Prgs_WMS.WMSUnLock(false, (DataTable)this.detailgridbs.DataSource, this.Name, EnumStatus.UnLock, EnumStatus.Unconfirm, dtOriFtyInventory);
                this.ShowErr(errMsg);
                return;
            }

            // PMS 更新之後,才執行WMS
            Prgs_WMS.WMSprocess(false, (DataTable)this.detailgridbs.DataSource, this.Name, EnumStatus.Delete, EnumStatus.Unconfirm, dtOriFtyInventory, typeCreateRecord: 2, autoRecord: autoRecordList);
            MyUtility.Msg.InfoBox("UnConfirmed successful");
            #endregion
        }

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();
            this.DetailSelectCommand = string.Format(
                @"
select  [Selected] = 0 
        ,a.id
        ,a.FromFtyinventoryUkey
        ,a.FromPoId
        ,a.FromSeq1
        ,a.FromSeq2
        ,Fromseq = concat(Ltrim(Rtrim(a.FromSeq1)), ' ', a.FromSeq2) 
        ,p1.FabricType
        ,p1.stockunit
        ,[description] = dbo.getmtldesc(a.FromPoId,a.FromSeq1,a.FromSeq2,2,0) 
        ,a.FromRoll
        ,a.FromDyelot
        ,a.FromStocktype
        ,a.Qty
        ,a.ToPoid
        ,a.ToSeq1
        ,a.ToSeq2
        ,toseq = concat(Ltrim(Rtrim(a.ToSeq1)), ' ', a.ToSeq2)
        ,a.ToRoll
        ,a.ToDyelot
        ,a.ToStocktype
        ,a.ukey
        ,location = dbo.Getlocation(fi.ukey)
        ,[FromContainerCode] = fi.ContainerCode
        ,a.ToLocation
        ,a.ToContainerCode
        ,Fromlocation = Fromlocation.listValue
from dbo.BorrowBack_detail a WITH (NOLOCK) 
left join PO_Supp_Detail p1 WITH (NOLOCK) on p1.ID = a.FromPoId and p1.seq1 = a.FromSeq1 and p1.SEQ2 = a.FromSeq2
left join FtyInventory FI on a.fromPoid = fi.poid and a.fromSeq1 = fi.seq1 and a.fromSeq2 = fi.seq2 and a.fromDyelot = fi.Dyelot 
    and a.fromRoll = fi.roll and a.fromStocktype = fi.stocktype
outer apply(
	select listValue = Stuff((
			select concat(',',MtlLocationID)
			from (
					select 	distinct
						fd.MtlLocationID
					from FtyInventory_Detail fd
					left join MtlLocation ml on ml.ID = fd.MtlLocationID
					where fd.Ukey = fi.Ukey
					and ml.Junk = 0 
					and ml.StockType = a.ToStockType
				) s
			for xml path ('')
		) , 1, 1, '')
)Fromlocation
Where a.id = '{0}'", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        private void BtnClearQtyIsEmpty_Click(object sender, EventArgs e)
        {
            this.detailgrid.ValidateControl();

            // detailgridbs.EndEdit();
            ((DataTable)this.detailgridbs.DataSource).Select("qty=0.00 or qty is null").ToList().ForEach(r => r.Delete());
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["borrowid"]))
            {
                MyUtility.Msg.WarningBox("Borrow Id can't be empty!!");
                this.txtBorrowID.Focus();
                return;
            }

            var frm = new P32_Import(this.CurrentMaintain, (DataTable)this.detailgridbs.DataSource);
            frm.ShowDialog(this);
            this.RenewData();
        }

        private void BtnAccumulatedQty_Click(object sender, EventArgs e)
        {
            var frm = new P32_AccumulatedQty(this.CurrentMaintain)
            {
                P32 = this,
            };
            frm.ShowDialog(this);
        }

        private void BtnFind_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.detailgridbs.DataSource))
            {
                return;
            }

            int index = this.detailgridbs.Find("frompoid", this.txtLocateForSP.Text.TrimEnd());
            if (index == -1)
            {
                MyUtility.Msg.WarningBox("Data was not found!!");
            }
            else
            {
                this.detailgridbs.Position = index;
            }
        }

        private void TxtBorrowID_Validating(object sender, CancelEventArgs e)
        {
            if (MyUtility.Check.Empty(this.txtBorrowID.Text))
            {
                this.CurrentMaintain["BorrowId"] = string.Empty;
                return;
            }

            string cmd = $@"
select DISTINCT a.Status,a.BackDate
from BorrowBack a
inner join BorrowBack_Detail b on a.Id = b.ID
inner join Factory f on f.ID = b.ToFactoryID
where a.id='{this.txtBorrowID.Text}' AND  f.MDivisionID = '{Env.User.Keyword}'  and a.type='A'
";

            // BorrowBack MDivisionID 是P31 寫入 => Sci.Env.User.Keyword
            if (!MyUtility.Check.Seek(cmd, out DataRow dr, null))
            {
                e.Cancel = true;
                MyUtility.Msg.WarningBox("Please check borrow id is existed.", "Data not found!!");
                this.txtBorrowID.Text = string.Empty;
                return;
            }
            else
            {
                if (dr["status"].ToString().ToUpper() == "NEW")
                {
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox("Borrow Id is not confirmed!!");
                    return;
                }

                if (!MyUtility.Check.Empty(dr["backdate"]))
                {
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("This borrow# ({0}) already returned.", this.txtBorrowID.Text));
                    return;
                }
            }

            this.CurrentMaintain["BorrowId"] = this.txtBorrowID.Text;
        }

        /// <summary>
        /// 確認 SP# & Seq 是否已經有重複的 Roll
        /// </summary>
        /// <returns>bool</returns>
        private bool CheckRoll()
        {
            // 判斷是否已經收過此種布料SP#,SEQ,Roll不能重複收
            List<string> listMsg = new List<string>();
            foreach (DataRow row in this.DetailDatas)
            {
                // 判斷 物料 是否為 布，布料才需要 Roll & Dyelot
                if (row["fabrictype"].ToString().ToUpper() == "F")
                {
                    #region 判斷 在收料記錄 & FtyInventory 是否存在【同 Roll 不同 Dyelot】
                    string checkSql = $@"
select  1
   from dbo.BorrowBack_Detail BD WITH (NOLOCK) 
	inner join dbo.BorrowBack B WITH (NOLOCK) on BD.ID = B.Id  
	where 
ToPOID = '{row["Topoid"]}' and 
ToSeq1 = '{row["Toseq1"]}' and 
ToSeq2 = '{row["Toseq2"]}' and 
ToRoll = '{row["Toroll"]}' and 
ToDyelot = '{row["Todyelot"]}' and 
B.ID != '{this.CurrentMaintain["id"]}' and B.Status = 'Confirmed'
";
                    if (MyUtility.Check.Seek(checkSql, out DataRow dr, null))
                    {
                        if (Convert.ToInt32(dr[0]) > 0)
                        {
                            listMsg.Add($@"
The Deylot of
<SP#>:{row["Topoid"]}, <Seq>:{row["Toseq1"].ToString() + " " + row["Toseq2"].ToString()}, <Roll>:{row["Toroll"]}, <Deylot>:{row["ToDyelot"].ToString().Trim()} already exists
");
                        }
                    }
                    #endregion
                }
            }

            if (listMsg.Count > 0)
            {
                MyUtility.Msg.WarningBox(listMsg.JoinToString(string.Empty).TrimStart());
                return false;
            }

            return true;
        }

        private void BtnCallP99_Click(object sender, EventArgs e)
        {
            P99_CallForm.CallForm(this.CurrentMaintain["ID"].ToString(), "P32", this);
        }

        private void BtnUpdateLocation_Click(object sender, EventArgs e)
        {
            if (this.DetailDatas == null || this.DetailDatas.Count == 0)
            {
                return;
            }

            List<DataRow> dataRows = this.DetailDatas.Where(x => x["Selected"].EqualDecimal(1)).ToList();
            foreach (DataRow dr in dataRows)
            {
                dr["ToLocation"] = dr["Fromlocation"];
                dr.EndEdit();
            }
        }
    }
}