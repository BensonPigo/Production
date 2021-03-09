using Ict;
using Ict.Win;
using Microsoft.Reporting.WinForms;
using Sci.Data;
using Sci.Production.Automation;
using Sci.Production.PublicPrg;
using Sci.Win;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P31 : Win.Tems.Input6
    {
        private Dictionary<string, string> di_stocktype = new Dictionary<string, string>();
        private ReportViewer viewer;

        /// <inheritdoc/>
        public P31(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.InsertDetailGridOnDoubleClick = false;
            this.viewer = new ReportViewer
            {
                Dock = DockStyle.Fill,
            };
            this.Controls.Add(this.viewer);

            // MDivisionID 是 P31 寫入 => Sci.Env.User.Keyword
            this.DefaultFilter = string.Format("Type='A' and MDivisionID = '{0}'", Env.User.Keyword);
            this.gridicon.Append.Enabled = false;
            this.gridicon.Append.Visible = false;
            this.gridicon.Insert.Enabled = false;
            this.gridicon.Insert.Visible = false;
            this.di_stocktype.Add("B", "Bulk");
            this.di_stocktype.Add("I", "Inventory");
        }

        /// <inheritdoc/>
        public P31(ToolStripMenuItem menuitem, string transID)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = string.Format("Type='A' and id='{0}'", transID);
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
            this.CurrentMaintain["Type"] = "A";
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

        // print

        /// <inheritdoc/>
        protected override bool ClickPrint()
        {
            // DataRow dr = grid.GetDataRow<DataRow>(grid.GetSelectedRowIndex());
            if (this.CurrentMaintain["status"].ToString().ToUpper() != "CONFIRMED")
            {
                MyUtility.Msg.WarningBox("Data is not confirmed, can't print.", "Warning");
                return false;
            }

            DataRow row = this.CurrentMaintain;
            string id = row["ID"].ToString();
            string estbackdate = ((DateTime)MyUtility.Convert.GetDate(row["Estbackdate"])).ToShortDateString();
            string remark = row["Remark"].ToString();
            string mdivisionid = this.CurrentMaintain["mdivisionid"].ToString();
            string issuedate = ((DateTime)MyUtility.Convert.GetDate(row["issuedate"])).ToShortDateString();

            #region  抓表頭資料
            List<SqlParameter> pars = new List<SqlParameter>
            {
                new SqlParameter("@ID", id),
            };

            string sqlcmd = @"
select b.nameEN 
from dbo.Borrowback a WITH (NOLOCK) 
inner join dbo.mdivision  b WITH (NOLOCK) on b.id = a.mdivisionid
where   b.id = a.mdivisionid
and a.id = @ID";
            DualResult result1 = DBProxy.Current.Select(string.Empty, sqlcmd, pars, out DataTable dtt);
            if (!result1)
            {
                this.ShowErr(result1);
            }

            if (dtt == null || dtt.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found!!", "DataTable dtt");
                return false;
            }

            string rptTitle = dtt.Rows[0]["nameEN"].ToString();
            ReportDefinition report = new ReportDefinition();
            report.ReportParameters.Add(new ReportParameter("RptTitle", rptTitle));
            report.ReportParameters.Add(new ReportParameter("ID", id));
            report.ReportParameters.Add(new ReportParameter("Estbackdate", estbackdate));
            report.ReportParameters.Add(new ReportParameter("Remark", remark));
            report.ReportParameters.Add(new ReportParameter("mdivisionid", mdivisionid));
            report.ReportParameters.Add(new ReportParameter("issuedate", issuedate));
            #endregion

            #region  抓表身資料
            pars = new List<SqlParameter>
            {
                new SqlParameter("@ID", id),
            };
            sqlcmd = @"
select  FromSP = a.FromPOID + '(' + a.FromSeq1 + '-' + a.Fromseq2 + ')' 
        ,TOSP = a.Topoid + '(' + a.ToSeq1 + '-' + a.ToSeq2 + ')' 
        ,[DESC] = IIF((b.ID = lag(b.ID,1,'')over (order by b.ID,b.seq1,b.seq2) 
                        AND(b.seq1 = lag(b.seq1,1,'')over (order by b.ID,b.seq1,b.seq2))
                        AND(b.seq2 = lag(b.seq2,1,'')over (order by b.ID,b.seq1,b.seq2))) 
                      ,''
                      ,dbo.getMtlDesc(a.FromPOID,a.FromSeq1,a.Fromseq2,2,0))
        ,StockType = case a.FromStockType
                        when 'B' then 'Bulk'
                        when 'I' then 'Inventory'
                        else a.FromStockType end 
        ,[Location] = dbo.Getlocation(fi.ukey)
        ,unit = b.StockUnit
        ,a.FromRoll
        ,a.FromDyelot
        ,a.Qty
        ,[Total] = sum(a.Qty) OVER (PARTITION BY a.FromPOID ,a.FromSeq1,a.FromSeq2 )
from dbo.Borrowback_detail a WITH (NOLOCK) 
left join dbo.PO_Supp_Detail b WITH (NOLOCK) on b.id=a.FromPOID and b.SEQ1=a.FromSeq1 and b.SEQ2=a.FromSeq2
left join dbo.FtyInventory FI on a.fromPoid = fi.poid and a.fromSeq1 = fi.seq1 and a.fromSeq2 = fi.seq2 and a.fromDyelot = fi.Dyelot
    and a.fromRoll = fi.roll and a.fromStocktype = fi.stocktype
left join dbo.SubTransfer_Detail c on c.id=a.id
where a.id= @ID";
            result1 = DBProxy.Current.Select(string.Empty, sqlcmd, pars, out DataTable dd);
            if (!result1)
            {
                this.ShowErr(result1);
            }

            if (dd == null || dd.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found!!", "DataTable dd");
                return false;
            }

            // 傳 list 資料
            List<P31_PrintData> data = dd.AsEnumerable()
                .Select(row1 => new P31_PrintData()
                {
                    FromSP = row1["FromSP"].ToString().Trim(),
                    TOSP = row1["TOSP"].ToString().Trim(),
                    DESC = row1["DESC"].ToString().Trim(),
                    StockType = row1["StockType"].ToString().Trim(),
                    Location = row1["Location"].ToString().Trim(),
                    Unit = row1["unit"].ToString().Trim(),
                    FromRoll = row1["FromRoll"].ToString().Trim(),
                    FromDyelot = row1["FromDyelot"].ToString().Trim(),
                    QTY = MyUtility.Convert.GetDecimal(row1["QTY"]),
                    Total = MyUtility.Convert.GetDecimal(row1["Total"]),
                }).ToList();

            report.ReportDataSource = data;
            #endregion

            // 指定是哪個 RDLC
            #region  指定是哪個 RDLC

            // DualResult result;
            Type reportResourceNamespace = typeof(P31_PrintData);
            Assembly reportResourceAssembly = reportResourceNamespace.Assembly;
            string reportResourceName = "P31_Print.rdlc";

            if (!(result1 = ReportResources.ByEmbeddedResource(reportResourceAssembly, reportResourceNamespace, reportResourceName, out IReportResource reportresource1)))
            {
                // this.ShowException(result);
                return false;
            }

            report.ReportResource = reportresource1;
            #endregion

            // 開啟 report view
            var frm1 = new Win.Subs.ReportView(report)
            {
                MdiParent = this.MdiParent,
            };
            frm1.Show();

            return true;

            // Sci.Win.ReportDefinition rd = new Sci.Win.ReportDefinition();
            ////
            // DualResult result;

            // IReportResource reportresource;

            // DataTable dt = (DataTable)gridbs.DataSource;
            // DataTable dtmaster = new DataTable();

            // if (!(result = ReportResources.ByEmbeddedResource(Assembly.GetAssembly(GetType()), GetType(), "P13Detail.rdlc", out reportresource)))
            // {
            //    ShowErr(result);
            // }
            // else
            // {
            //    rd.ReportResource = reportresource;
            //    rd.ReportDataSources.Add(new System.Collections.Generic.KeyValuePair<string, object>("DataSet1", dtmaster));
            //    // Assign subreport datasource, 如果不是 master-detail report 則以下的指令不必指定.
            //    rd.SubreportDataSource("RepDetail", "DetailData", (DataTable)this.detailgridbs.DataSource);
            //    using (var frm = new Sci.Win.Subs.ReportView(rd))
            //    {
            //        frm.ShowDialog(this);
            //    }
            // }

            // return base.ClickPrint();
        }

        private void MySubreportEventHandler(object sender, SubreportProcessingEventArgs e)
        {
            e.DataSources.Add(new ReportDataSource("DataSet1", (DataTable)this.detailgridbs.DataSource));
        }

        // save前檢查 & 取id

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
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

            if (MyUtility.Check.Empty(this.CurrentMaintain["estbackdate"]))
            {
                MyUtility.Msg.WarningBox("< Est. Return Date >  can't be empty!", "Warning");
                this.dateEstReturnDate.Focus();
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
                    warningmsg.Append($@"SP#: {row["frompoid"]} Seq#: {row["fromseq1"]}-{row["fromseq2"]} Roll#:{row["fromroll"]} Dyelot:{row["fromdyelot"]} Issue Qty can't be empty" + Environment.NewLine);
                }

                if (row["fabrictype"].ToString().ToUpper() == "F" && (MyUtility.Check.Empty(row["toroll"]) || MyUtility.Check.Empty(row["todyelot"])))
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
                string tmpId = MyUtility.GetValue.GetID(Env.User.Keyword + "MB", "BorrowBack", (DateTime)this.CurrentMaintain["Issuedate"]);
                if (MyUtility.Check.Empty(tmpId))
                {
                    MyUtility.Msg.WarningBox("Get document ID fail!!");
                    return false;
                }

                this.CurrentMaintain["id"] = tmpId;
            }

            return base.ClickSaveBefore();
        }

        // grid 加工填值

        /// <inheritdoc/>
        protected override DualResult OnRenewDataDetailPost(RenewDataPostEventArgs e)
        {
            return base.OnRenewDataDetailPost(e);
        }

        // refresh

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            #region Status Label

            this.label25.Text = this.CurrentMaintain["status"].ToString();

            #endregion Status Label

            if (Vstrong_AutoWHAccessory.IsVstrong_AutoWHAccessoryEnable && (this.CurrentMaintain["Status"].ToString().ToUpper() == "CONFIRMED"))
            {
                this.btnCallP99.Visible = true;
            }
            else
            {
                this.btnCallP99.Visible = false;
            }
        }

        // Detail Grid 設定

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

            #region 欄位設定
            this.Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("frompoid", header: "From SP#", width: Widths.AnsiChars(13), iseditingreadonly: true) // 0
            .Text("fromseq", header: "From" + Environment.NewLine + "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true) // 1
            .Text("fromroll", header: "From" + Environment.NewLine + "Roll", width: Widths.AnsiChars(6), iseditingreadonly: true) // 2
            .Text("fromdyelot", header: "From" + Environment.NewLine + "Dyelot", width: Widths.AnsiChars(6), iseditingreadonly: true) // 3
            .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true) // 4
            .ComboBox("fromstocktype", header: "From" + Environment.NewLine + "Stock" + Environment.NewLine + "Type", iseditable: false).Get(out Ict.Win.UI.DataGridViewComboBoxColumn cbb_stocktype) // 5
            .Text("Location", header: "From" + Environment.NewLine + "Location", iseditingreadonly: true) // 6
            .Text("topoid", header: "To" + Environment.NewLine + "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true) // 7
            .Text("toseq", header: "To" + Environment.NewLine + "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true) // 8
            .Text("toroll", header: "To" + Environment.NewLine + "Roll", width: Widths.AnsiChars(6)) // 9
            .Text("todyelot", header: "To" + Environment.NewLine + "Dyelot", width: Widths.AnsiChars(6)) // 10
            .Numeric("qty", header: "Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10) // 11
            .Text("ToLocation", header: "To Location", width: Widths.AnsiChars(10), settings: toLocation)
            .Text("stockunit", header: "Stock" + Environment.NewLine + "Unit", iseditingreadonly: true)
            ;
            #endregion 欄位設定
            this.detailgrid.Columns["toroll"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["todyelot"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["qty"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["ToLocation"].DefaultCellStyle.BackColor = Color.Pink;

            cbb_stocktype.DataSource = new BindingSource(this.di_stocktype, null);
            cbb_stocktype.ValueMember = "Key";
            cbb_stocktype.DisplayMember = "Value";
        }

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            var dr = this.CurrentMaintain;
            if (dr == null)
            {
                return;
            }

            string upd_MD_2T = string.Empty;
            string upd_MD_4T = string.Empty;
            string upd_MD_8T = string.Empty;
            string upd_Fty_4T = string.Empty;
            string upd_Fty_2T = string.Empty;

            StringBuilder sqlupd2 = new StringBuilder();
            string sqlcmd = string.Empty, sqlupd3 = string.Empty, ids = string.Empty;
            DualResult result, result2;

            #region -- 檢查庫存項lock --
            sqlcmd = string.Format(
                @"
Select  d.frompoid
        ,d.fromseq1
        ,d.fromseq2
        ,d.fromRoll,d.Qty
        ,balanceQty = isnull(f.InQty,0) - isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - isnull(f.ReturnQty,0)
        ,f.Dyelot
from dbo.BorrowBack_Detail d WITH (NOLOCK) 
inner join FtyInventory f WITH (NOLOCK) on d.FromPOID = f.POID  
  AND D.FromStockType = F.StockType and d.FromRoll = f.Roll and d.FromSeq1 =f.Seq1 and d.FromSeq2 = f.Seq2 and d.fromDyelot = f.Dyelot
where f.lock = 1 and d.Id = '{0}'", this.CurrentMaintain["id"]);
            if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out DataTable datacheck)))
            {
                this.ShowErr(sqlcmd, result2);
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
left join FtyInventory f WITH (NOLOCK) on d.FromPOID = f.POID AND D.FromStockType = F.StockType 
    and d.FromRoll = f.Roll and d.FromSeq1 =f.Seq1 and d.FromSeq2 = f.Seq2 and d.fromDyelot = f.Dyelot
where (isnull(f.InQty,0) - isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - isnull(f.ReturnQty,0) - d.Qty < 0) and d.Id = '{0}'", this.CurrentMaintain["id"]);
            if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                this.ShowErr(sqlcmd, result2);
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    foreach (DataRow tmp in datacheck.Rows)
                    {
                        ids += $"SP#: {tmp["frompoid"]} Seq#: {tmp["fromseq1"]}-{tmp["fromseq2"]} Roll#: {tmp["fromroll"]} Dyelot: {tmp["Dyelot"]}'s balance: {tmp["balanceqty"]} is less than qty: {tmp["qty"]}" + Environment.NewLine;
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

            #region -- 更新表頭狀態資料 --

            sqlupd3 = $@"update BorrowBack set status='Confirmed', editname = '{Env.User.UserID}' , editdate = GETDATE()
where id = '{this.CurrentMaintain["id"]}'";

            #endregion 更新表頭狀態資料
            #region -- 更新 mdivisionpodetail 借出數 --
            var data_MD_4T = (from b in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
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
            var data_MD_8T = (from b in ((DataTable)this.detailgridbs.DataSource).AsEnumerable().Where(w => w.Field<string>("fromstocktype").Trim() == "I")
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
                                  Location = string.Join(",", m.Select(r => r.Field<string>("Tolocation")).Distinct()),
                              }).ToList();

            #endregion
            #region -- 更新mdivisionPoDetail 借入數 A倉數 --
            var data_MD_2T = (from b in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
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
            #region -- 更新庫存數量 ftyinventory --
            var data_Fty_4T = (from m in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
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

            var data_Fty_2T = (from b in this.DetailDatas
                               select new
                               {
                                   poid = b.Field<string>("topoid"),
                                   seq1 = b.Field<string>("toseq1"),
                                   seq2 = b.Field<string>("toseq2"),
                                   stocktype = b.Field<string>("tostocktype"),
                                   qty = b.Field<decimal>("qty"),
                                   location = b.Field<string>("Tolocation"),
                                   roll = b.Field<string>("toroll"),
                                   dyelot = b.Field<string>("todyelot"),
                               }).ToList();
            upd_Fty_4T = Prgs.UpdateFtyInventory_IO(4, null, true);
            upd_Fty_2T = Prgs.UpdateFtyInventory_IO(2, null, true);

            #endregion 更新庫存數量 ftyinventory
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

            TransactionScope transactionscope = new TransactionScope();
            DBProxy.Current.OpenConnection(null, out SqlConnection sqlConn);

            using (transactionscope)
            using (sqlConn)
            {
                try
                {
                    /*
                     * 先更新 FtyInventory 後更新 MDivisionPoDetail
                     * 所有 MDivisionPoDetail 資料都在 Transaction 中更新，
                     * 因為要在同一 SqlConnection 之下執行
                     */
                    #region FtyInventory
                    if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_4T, string.Empty, upd_Fty_4T, out DataTable resulttb, "#TmpSource", conn: sqlConn)))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(result);
                        return;
                    }

                    if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_2T, string.Empty, upd_Fty_2T, out resulttb, "#TmpSource", conn: sqlConn)))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(result);
                        return;
                    }

                    if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_26F, string.Empty, upd_Fty_26F, out resulttb, "#TmpSource")))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(result);
                        return;
                    }
                    #endregion

                    #region MDivisionPoDetail
                    if (data_MD_4T.Count > 0)
                    {
                        upd_MD_4T = Prgs.UpdateMPoDetail(4, null, true, sqlConn: sqlConn);
                    }

                    if (data_MD_8T.Count > 0)
                    {
                        upd_MD_8T = Prgs.UpdateMPoDetail(8, data_MD_8T, true, sqlConn: sqlConn);
                    }

                    upd_MD_2T = Prgs.UpdateMPoDetail(2, data_MD_2T, true, sqlConn: sqlConn);

                    if (data_MD_4T.Count > 0)
                    {
                        if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_4T, string.Empty, upd_MD_4T, out resulttb, "#TmpSource", conn: sqlConn)))
                        {
                            transactionscope.Dispose();
                            this.ShowErr(result);
                            return;
                        }
                    }

                    if (data_MD_8T.Count > 0)
                    {
                        if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_8T, string.Empty, upd_MD_8T, out resulttb, "#TmpSource", conn: sqlConn)))
                        {
                            transactionscope.Dispose();
                            this.ShowErr(result);
                            return;
                        }
                    }

                    if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_2T, string.Empty, upd_MD_2T, out resulttb, "#TmpSource", conn: sqlConn)))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(result);
                        return;
                    }
                    #endregion

                    if (!(result = DBProxy.Current.Execute(null, sqlupd3)))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(sqlupd3, result);
                        return;
                    }

                    transactionscope.Complete();
                    transactionscope.Dispose();
                    this.FtyBarcodeData(true);
                    DataTable dt = this.CurrentMaintain.Table.AsEnumerable().Where(s => s["ID"] == this.CurrentMaintain["ID"]).CopyToDataTable();

                    // AutoWHACC WebAPI for Vstrong
                    if (Vstrong_AutoWHAccessory.IsVstrong_AutoWHAccessoryEnable)
                    {
                        Task.Run(() => new Vstrong_AutoWHAccessory().SentBorrowBack_Detail_New(dt, "New"))
                        .ContinueWith(UtilityAutomation.AutomationExceptionHandler, TaskContinuationOptions.OnlyOnFaulted);
                    }

                    // AutoWH Fabric WebAPI for Gensong
                    if (Gensong_AutoWHFabric.IsGensong_AutoWHFabricEnable)
                    {
                        Task.Run(() => new Gensong_AutoWHFabric().SentBorrowBack_Detail_New(dt, "New"))
                        .ContinueWith(UtilityAutomation.AutomationExceptionHandler, TaskContinuationOptions.OnlyOnFaulted);
                    }

                    MyUtility.Msg.InfoBox("Confirmed successful");
                }
                catch (Exception ex)
                {
                    transactionscope.Dispose();
                    this.ShowErr("Commit transaction error.", ex);
                    return;
                }
            }

            transactionscope.Dispose();
            transactionscope = null;

            // this.RenewData();
            // this.OnDetailEntered();
            // this.EnsureToolbarExt();
        }

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            DataTable dt = (DataTable)this.detailgridbs.DataSource;

            string upd_MD_4F = string.Empty;
            string upd_MD_8F = string.Empty;

            string upd_MD_2F = string.Empty;
            string upd_Fty_4F = string.Empty;
            string upd_Fty_2F = string.Empty;

            DialogResult dResult = MyUtility.Msg.QuestionBox("Do you want to unconfirme it?");
            if (dResult.ToString().ToUpper() == "NO")
            {
                return;
            }

            var dr = this.CurrentMaintain;
            if (dr == null)
            {
                return;
            }

            StringBuilder sqlupd2 = new StringBuilder();
            string sqlcmd = string.Empty, sqlupd3 = string.Empty, ids = string.Empty;
            DualResult result, result2;

            // 564: WAREHOUSE_P31_Material Borrow，若已有Act. Return date則不能unconfirm
            if (!MyUtility.Check.Empty(dr["backdate"]))
            {
                MyUtility.Msg.WarningBox("[Act. Return Date] already has value, can't unconfirm !!", "Warning");
                return;
            }

            #region -- 檢查庫存項lock --
            sqlcmd = string.Format(
                @"
Select  d.topoid
        ,d.toseq1
        ,d.toseq2
        ,d.toRoll
        ,d.Qty
        ,balanceQty = isnull(f.InQty,0) - isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - isnull(f.ReturnQty,0)
        ,f.Dyelot
from dbo.BorrowBack_Detail d WITH (NOLOCK) inner join FtyInventory f WITH (NOLOCK) 
  on d.toPoId = f.PoId
  and d.toSeq1 = f.Seq1
  and d.toSeq2 = f.seq2
  and d.toStocktype = f.StockType
  and d.toRoll = f.Roll
  and d.toDyelot = f.Dyelot
where f.lock=1 and d.Id = '{0}'", this.CurrentMaintain["id"]);
            if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out DataTable datacheck)))
            {
                this.ShowErr(sqlcmd, result2);
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
            if (!Prgs.ChkWMSCompleteTime(dt, "BorrowBack_Detail_To"))
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
from dbo.BorrowBack_Detail d WITH (NOLOCK) left join FtyInventory f WITH (NOLOCK) 
  on d.toPoId = f.PoId
  and d.toSeq1 = f.Seq1
  and d.toSeq2 = f.seq2
  and d.toStocktype = f.StockType
  and d.toRoll = f.Roll
  and d.toDyelot = f.Dyelot
where (isnull(f.InQty,0) - isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - isnull(f.ReturnQty,0) - d.Qty < 0) and d.Id = '{0}'", this.CurrentMaintain["id"]);
            if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                this.ShowErr(sqlcmd, result2);
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

            #region UnConfirmed 先檢查WMS是否傳送成功

            DataTable dtDetail = this.CurrentMaintain.Table.AsEnumerable().Where(s => s["ID"] == this.CurrentMaintain["ID"]).CopyToDataTable();
            if (Vstrong_AutoWHAccessory.IsVstrong_AutoWHAccessoryEnable)
            {
                if (!Vstrong_AutoWHAccessory.SentBorrowBack_Detail_delete(dtDetail, "UnConfirmed"))
                {
                    return;
                }
            }

            // AutoWHFabric WebAPI for Gensong
            if (Gensong_AutoWHFabric.IsGensong_AutoWHFabricEnable)
            {
                if (!Gensong_AutoWHFabric.SentBorrowBack_Detail_Delete(dtDetail, "UnConfirmed"))
                {
                    return;
                }
            }
            #endregion

            #region -- 更新表頭狀態資料 --

            sqlupd3 = $@"update BorrowBack set status='New', editname = '{Env.User.UserID}' , editdate = GETDATE()
where id = '{this.CurrentMaintain["id"]}'";

            #endregion 更新表頭狀態資料

            #region -- 更新MdivisionPoDetail 借出數 --
            var data_MD_4F = (from b in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
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
            var data_MD_8F = (from b in ((DataTable)this.detailgridbs.DataSource).AsEnumerable().Where(w => w.Field<string>("fromstocktype").Trim() == "I")
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
                              }).ToList();

            #endregion
            #region -- 更新MdivisionPoDetail 借入數 --
            var data_MD_2F = (from b in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
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

            #endregion
            #region -- 更新庫存數量  ftyinventory --
            var data_Fty_4F = (from m in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
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
            var data_Fty_2F = (from m in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
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
            upd_Fty_4F = Prgs.UpdateFtyInventory_IO(4, null, false);
            upd_Fty_2F = Prgs.UpdateFtyInventory_IO(2, null, false);
            #endregion 更新庫存數量  ftyinventory

            TransactionScope transactionscope = new TransactionScope();
            DBProxy.Current.OpenConnection(null, out SqlConnection sqlConn);

            using (transactionscope)
            using (sqlConn)
            {
                try
                {
                    /*
                     * 先更新 FtyInventory 後更新 MDivisionPoDetail
                     * 所有 MDivisionPoDetail 資料都在 Transaction 中更新，
                     * 因為要在同一 SqlConnection 之下執行
                     */
                    #region FtyInventory
                    if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_4F, string.Empty, upd_Fty_4F, out DataTable resulttb, "#TmpSource", conn: sqlConn)))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(result);
                        return;
                    }

                    if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_2F, string.Empty, upd_Fty_2F, out resulttb, "#TmpSource", conn: sqlConn)))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(result);
                        return;
                    }
                    #endregion

                    #region MDivisionPoDetail
                    if (data_MD_4F.Count > 0)
                    {
                        upd_MD_4F = Prgs.UpdateMPoDetail(4, null, false, sqlConn: sqlConn);
                    }

                    if (data_MD_8F.Count > 0)
                    {
                        upd_MD_8F = Prgs.UpdateMPoDetail(8, data_MD_8F, false, sqlConn: sqlConn);
                    }

                    upd_MD_2F = Prgs.UpdateMPoDetail(2, data_MD_2F, false, sqlConn: sqlConn);

                    if (data_MD_4F.Count > 0)
                    {
                        if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_4F, string.Empty, upd_MD_4F, out resulttb, "#TmpSource", conn: sqlConn)))
                        {
                            transactionscope.Dispose();
                            this.ShowErr(result);
                            return;
                        }
                    }

                    if (data_MD_8F.Count > 0)
                    {
                        if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_8F, string.Empty, upd_MD_8F, out resulttb, "#TmpSource", conn: sqlConn)))
                        {
                            transactionscope.Dispose();
                            this.ShowErr(result);
                            return;
                        }
                    }

                    if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_2F, string.Empty, upd_MD_2F, out resulttb, "#TmpSource", conn: sqlConn)))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(result);
                        return;
                    }
                    #endregion

                    if (!(result = DBProxy.Current.Execute(null, sqlupd3)))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(sqlupd3, result);
                        return;
                    }

                    transactionscope.Complete();
                    transactionscope.Dispose();
                    this.FtyBarcodeData(false);
                    MyUtility.Msg.InfoBox("UnConfirmed successful");
                }
                catch (Exception ex)
                {
                    transactionscope.Dispose();
                    this.ShowErr("Commit transaction error.", ex);
                    return;
                }
            }

            transactionscope.Dispose();
            transactionscope = null;
        }

        private void FtyBarcodeData(bool isConfirmed)
        {
            DualResult result;
            DataTable dt = new DataTable();
            string sqlcmd = string.Empty;
            string upd_Fty_Barcode_V1 = string.Empty;
            string upd_Fty_Barcode_V2 = string.Empty;
            #region From
            sqlcmd = $@"
select i2.ID
,[Barcode1] = f.Barcode
,[OriBarcode] = fbOri.Barcode
,[balanceQty] = f.InQty - f.OutQty + f.AdjustQty - f.ReturnQty
,[NewBarcode] = iif(f.Barcode ='',fbOri.Barcode,f.Barcode)
,[Poid] = i2.FromPOID
,[Seq1] = i2.FromSeq1
,[Seq2] = i2.FromSeq2
,[Roll] = i2.FromRoll
,[Dyelot] = i2.FromDyelot
,[StockType] = i2.FromStockType
from Production.dbo.BorrowBack_Detail i2
inner join Production.dbo.BorrowBack i on i2.Id=i.Id 
inner join FtyInventory f on f.POID = i2.FromPOID
    and f.Seq1 = i2.FromSeq1 and f.Seq2 = i2.FromSeq2
    and f.Roll = i2.FromRoll and f.Dyelot = i2.FromDyelot
    and f.StockType = i2.FromStockType
outer apply(
	select *
	from FtyInventory_Barcode t
	where t.Ukey = f.Ukey
	and t.TransactionID = '{this.CurrentMaintain["ID"]}'
)fbOri
where 1=1
and exists(
	select 1 from Production.dbo.PO_Supp_Detail 
	where id = i2.FromPoid and seq1=i2.FromSeq1 and seq2=i2.FromSeq2 
	and FabricType='F'
)
and i2.id ='{this.CurrentMaintain["ID"]}'
";
            DBProxy.Current.Select(string.Empty, sqlcmd, out dt);
            var data_From_FtyBarcode = (from m in dt.AsEnumerable().Where(s => s["NewBarcode"].ToString() != string.Empty)
                                        select new
                                        {
                                            TransactionID = this.CurrentMaintain["ID"].ToString(),
                                            poid = m.Field<string>("poid"),
                                            seq1 = m.Field<string>("seq1"),
                                            seq2 = m.Field<string>("seq2"),
                                            stocktype = m.Field<string>("stocktype"),
                                            roll = m.Field<string>("roll"),
                                            dyelot = m.Field<string>("dyelot"),
                                            Barcode = m.Field<string>("NewBarcode"),
                                        }).ToList();

            // confirmed 要刪除Barcode, 反之則從Ftyinventory_Barcode補回
            upd_Fty_Barcode_V1 = isConfirmed ? Prgs.UpdateFtyInventory_IO(70, null, !isConfirmed) : Prgs.UpdateFtyInventory_IO(72, null, true);
            upd_Fty_Barcode_V2 = Prgs.UpdateFtyInventory_IO(71, null, isConfirmed);
            DataTable resultFrom;
            if (data_From_FtyBarcode.Count >= 1)
            {
                // 需先更新upd_Fty_Barcode_V1, 才能更新upd_Fty_Barcode_V2, 順序不能變
                if (!(result = MyUtility.Tool.ProcessWithObject(data_From_FtyBarcode, string.Empty, upd_Fty_Barcode_V1, out resultFrom, "#TmpSource")))
                {
                    this.ShowErr(result);
                    return;
                }

                if (!(result = MyUtility.Tool.ProcessWithObject(data_From_FtyBarcode, string.Empty, upd_Fty_Barcode_V2, out resultFrom, "#TmpSource")))
                {
                    this.ShowErr(result);
                    return;
                }
            }
            #endregion
            #region To

            sqlcmd = $@"
select f.Ukey
,[ToBarcode] = isnull(f.Barcode,'')
,[ToBarcode2] = isnull(Tofb.Barcode,'')
,[FromBarcode] = isnull(fromBarcode.Barcode,'')
,[FromBarcode2] = isnull(Fromfb.Barcode,'')
,[ToBalanceQty] = f.InQty - f.OutQty + f.AdjustQty - f.ReturnQty
,[FromBalanceQty] = fromBarcode.BalanceQty
,[NewBarcode] = ''
,[Poid] = i2.ToPOID
,[Seq1] = i2.ToSeq1
,[Seq2] = i2.ToSeq2
,[Roll] = i2.ToRoll
,[Dyelot] = i2.ToDyelot
,[StockType] = i2.ToStockType
from Production.dbo.BorrowBack_Detail i2
inner join Production.dbo.BorrowBack i on i2.Id=i.Id 
left join FtyInventory f on f.POID = i2.ToPOID
    and f.Seq1 = i2.ToSeq1 and f.Seq2 = i2.ToSeq2
    and f.Roll = i2.ToRoll and f.Dyelot = i2.ToDyelot
    and f.StockType = i2.ToStockType

outer apply(
	select Barcode = MAX(Barcode)
	from FtyInventory_Barcode t
	where t.Ukey = f.Ukey
)Tofb
outer apply(
	select f2.Barcode 
	    ,BalanceQty = f2.InQty - f2.OutQty + f2.AdjustQty - f2.ReturnQty
	    ,f2.Ukey
	from FtyInventory f2	
	where f2.POID = i2.FromPOID
	and f2.Seq1 = i2.FromSeq1 and f2.Seq2 = i2.FromSeq2
	and f2.Roll = i2.FromRoll and f2.Dyelot = i2.FromDyelot
	and f2.StockType = i2.FromStockType
)fromBarcode
outer apply(
	select Barcode = MAX(Barcode)
	from FtyInventory_Barcode t
	where t.Ukey = fromBarcode.Ukey
)Fromfb
where 1=1
and exists(
	select 1 from Production.dbo.PO_Supp_Detail 
	where id = i2.ToPoid and seq1=i2.ToSeq1 and seq2=i2.ToSeq2 
	and FabricType='F'
)
and i2.id ='{this.CurrentMaintain["ID"]}'

";
            DBProxy.Current.Select(string.Empty, sqlcmd, out dt);

            foreach (DataRow dr in dt.Rows)
            {
                string strBarcode = string.Empty;

                // 目標有自己的Barcode, 則Ftyinventory跟記錄都是用自己的
                if (!MyUtility.Check.Empty(dr["ToBarcode"]) || !MyUtility.Check.Empty(dr["ToBarcode2"]))
                {
                    strBarcode = MyUtility.Check.Empty(dr["ToBarcode2"]) ? dr["ToBarcode"].ToString() : dr["ToBarcode2"].ToString();
                    dr["NewBarcode"] = strBarcode;
                }
                else
                {
                    // 目標沒Barcode, 則 來源有餘額(部分轉)就用來源Barocde_01++, 如果全轉就用來源Barocde
                    strBarcode = MyUtility.Check.Empty(dr["FromBarcode2"]) ? dr["FromBarcode"].ToString() : dr["FromBarcode2"].ToString();

                    // InQty-Out+Adj != 0 代表非整卷, 要在Barcode後+上-01,-02....
                    if (!MyUtility.Check.Empty(dr["FromBalanceQty"]))
                    {
                        if (strBarcode.Contains("-"))
                        {
                            dr["NewBarcode"] = strBarcode.Substring(0, 13) + "-" + Prgs.GetNextValue(strBarcode.Substring(14, 2), 1);
                        }
                        else
                        {
                            dr["NewBarcode"] = MyUtility.Check.Empty(strBarcode) ? string.Empty : strBarcode + "-01";
                        }
                    }
                    else
                    {
                        // 如果InQty-Out+Adj = 0 代表整卷發出就使用原本Barcode
                        dr["NewBarcode"] = strBarcode;
                    }
                }
            }

            var data_To_FtyBarcode = (from m in dt.AsEnumerable().Where(s => s["NewBarcode"].ToString() != string.Empty)
                                      select new
                                      {
                                          TransactionID = this.CurrentMaintain["ID"].ToString(),
                                          poid = m.Field<string>("poid"),
                                          seq1 = m.Field<string>("seq1"),
                                          seq2 = m.Field<string>("seq2"),
                                          stocktype = m.Field<string>("stocktype"),
                                          roll = m.Field<string>("roll"),
                                          dyelot = m.Field<string>("dyelot"),
                                          Barcode = m.Field<string>("NewBarcode"),
                                      }).ToList();

            // confirmed 要刪除Barcode, 反之則從Ftyinventory_Barcode補回
            upd_Fty_Barcode_V1 = Prgs.UpdateFtyInventory_IO(70, null, isConfirmed);
            upd_Fty_Barcode_V2 = Prgs.UpdateFtyInventory_IO(71, null, isConfirmed);
            DataTable resultTo;
            if (data_To_FtyBarcode.Count >= 1)
            {
                // 需先更新upd_Fty_Barcode_V1, 才能更新upd_Fty_Barcode_V2, 順序不能變
                if (!(result = MyUtility.Tool.ProcessWithObject(data_To_FtyBarcode, string.Empty, upd_Fty_Barcode_V1, out resultTo, "#TmpSource")))
                {
                    this.ShowErr(result);
                    return;
                }

                if (!(result = MyUtility.Tool.ProcessWithObject(data_To_FtyBarcode, string.Empty, upd_Fty_Barcode_V2, out resultTo, "#TmpSource")))
                {
                    this.ShowErr(result);
                    return;
                }
            }
            #endregion
        }

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();
            this.DetailSelectCommand = string.Format(
                @"
select  a.id
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
        ,a.fromftyinventoryukey
        ,a.ukey
        ,location = stuff((select ',' + mtllocationid 
                           from (select mtllocationid 
                                 from dbo.ftyinventory_detail fd WITH (NOLOCK) 
                                 where ukey= FI.ukey)t 
                           for xml path(''))
                          , 1, 1, '') 
        ,a.ToLocation
from dbo.BorrowBack_detail a WITH (NOLOCK) 
left join PO_Supp_Detail p1 WITH (NOLOCK) on p1.ID = a.FromPoId and p1.seq1 = a.FromSeq1 and p1.SEQ2 = a.FromSeq2
left join FtyInventory FI on a.FromPoid = Fi.Poid and a.FromSeq1 = Fi.Seq1 and a.FromSeq2 = Fi.Seq2 
    and a.FromRoll = Fi.Roll and a.FromDyelot = Fi.Dyelot and a.FromStockType = StockType
Where a.id = '{0}'", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        // delete all
        private void BtnClearQtyIsEmpty_Click(object sender, EventArgs e)
        {
            this.detailgrid.ValidateControl();

            // detailgridbs.EndEdit();
            ((DataTable)this.detailgridbs.DataSource).Select("qty=0.00 or qty is null").ToList().ForEach(r => r.Delete());
        }

        // Import
        private void BtnImport_Click(object sender, EventArgs e)
        {
            var frm = new P31_Import(this.CurrentMaintain, (DataTable)this.detailgridbs.DataSource);
            frm.ShowDialog(this);
            this.RenewData();
        }

        private void BtnAccumulatedQty_Click(object sender, EventArgs e)
        {
            var frm = new P31_AccumulatedQty(this.CurrentMaintain)
            {
                P31 = this,
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

        /// <summary>
        /// 確認是否已經有重複的Roll
        /// </summary>
        /// <returns>bool</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
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
ToPOID = '{row["topoid"]}' and 
ToSeq1 = '{row["toseq1"]}' and 
ToSeq2 = '{row["toseq2"]}' and 
ToRoll = '{row["toroll"]}' and 
ToDyelot = '{row["todyelot"]}' and 
B.ID != '{this.CurrentMaintain["id"]}' and 
B.Status = 'Confirmed'
";
                    if (MyUtility.Check.Seek(checkSql, out DataRow dr, null))
                    {
                        if (Convert.ToInt32(dr[0]) > 0)
                        {
                            listMsg.Add($@"
The Deylot of
<SP#>:{row["topoid"]}, <Seq>:{row["toseq1"].ToString() + " " + row["toseq2"].ToString()}, <Roll>:{row["toroll"]}, <Deylot>:{row["toDyelot"].ToString().Trim()} already exists
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
            P99_CallForm.CallForm(this.CurrentMaintain["ID"].ToString(), "P31", this);
        }
    }
}