using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.PublicPrg;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Windows.Forms;
using System.Reflection;
using Microsoft.Reporting.WinForms;
using Sci.Win;
using System.Data.SqlClient;

namespace Sci.Production.Warehouse
{
    public partial class P32 : Sci.Win.Tems.Input6
    {
        private Dictionary<string, string> di_fabrictype = new Dictionary<string, string>();
        private Dictionary<string, string> di_stocktype = new Dictionary<string, string>();
        protected ReportViewer viewer;
        public P32(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            InsertDetailGridOnDoubleClick = false;
            viewer = new ReportViewer();
            viewer.Dock = DockStyle.Fill;
            Controls.Add(viewer);

            this.DefaultFilter = string.Format("Type='B' and MDivisionID = '{0}'", Sci.Env.User.Keyword);
            gridicon.Append.Enabled = false;
            gridicon.Append.Visible = false;
            gridicon.Insert.Enabled = false;
            gridicon.Insert.Visible = false;
            di_stocktype.Add("B", "Bulk");
            di_stocktype.Add("I", "Inventory");
            //
        }

        public P32(ToolStripMenuItem menuitem, string transID)
            : base(menuitem)
        {
            InitializeComponent();
            this.DefaultFilter = string.Format("Type='B' and id='{0}'", transID);
            this.IsSupportNew = false;
            this.IsSupportEdit = false;
            this.IsSupportDelete = false;
            this.IsSupportConfirm = false;
            this.IsSupportUnconfirm = false;
            gridicon.Append.Enabled = false;
            gridicon.Append.Visible = false;
            gridicon.Insert.Enabled = false;
            gridicon.Insert.Visible = false;
            di_stocktype.Add("B", "Bulk");
            di_stocktype.Add("I", "Inventory");

        }

        // 新增時預設資料
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["MDivisionID"] = Sci.Env.User.Keyword;
            CurrentMaintain["Status"] = "New";
            CurrentMaintain["Type"] = "B";
            CurrentMaintain["IssueDate"] = DateTime.Now;
        }

        // delete前檢查
        protected override bool ClickDeleteBefore()
        {
            DataRow dr = grid.GetDataRow<DataRow>(grid.GetSelectedRowIndex());
            if (dr["status"].ToString().ToUpper() == "CONFIRMED")
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't delete.", "Warning");
                return false;
            }
            return base.ClickDeleteBefore();
        }

        // edit前檢查
        protected override bool ClickEditBefore()
        {
            DataRow dr = grid.GetDataRow<DataRow>(grid.GetSelectedRowIndex());
            if (dr["status"].ToString().ToUpper() == "CONFIRMED")
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't modify.", "Warning");
                return false;
            }
            return base.ClickEditBefore();
        }

        //print
        protected override bool ClickPrint()
        {
            DataRow dr = grid.GetDataRow<DataRow>(grid.GetSelectedRowIndex());
            if (dr["status"].ToString().ToUpper() != "CONFIRMED")
            {
                MyUtility.Msg.WarningBox("Data is not confirmed, can't print.", "Warning");
                return false;
            }
            DataRow row = this.CurrentDataRow;
            string id = row["ID"].ToString();
            string Remark = row["Remark"].ToString();
            string M = row["MdivisionID"].ToString();
            string issuedate = ((DateTime)MyUtility.Convert.GetDate(row["issuedate"])).ToShortDateString();
            #region -- 撈表頭資料 --
            List<SqlParameter> pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));
            DataTable dt1;
            DualResult result1 = DBProxy.Current.Select("",
            @"select    
            b.name 
            from dbo.Borrowback  a 
            inner join dbo.mdivision  b 
            on b.id = a.mdivisionid
            where b.id = a.mdivisionid
            and a.id = @ID", pars, out dt1);
            if (!result1) { this.ShowErr(result1); }
            string RptTitle = dt1.Rows[0]["name"].ToString();
            ReportDefinition report = new ReportDefinition();
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("RptTitle", RptTitle));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("ID", id));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Remark", Remark));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("issuedate", issuedate));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Factory", M));
            pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));
            #endregion

            #region -- 撈表身資料 --
            DataTable dtDetail;
            string sqlcmd = @"select  t.frompoid+' '+(t.fromseq1 + '-' +t.fromseq2) as StockSEQ,t.topoid+' '+(t.toseq1  + '-' +t.toseq2) as ToSP
			,dbo.getMtlDesc(t.FromPOID,t.FromSeq1,t.FromSeq2,2,iif(p.scirefno = lag(p.scirefno,1,'') over (order by p.refno,p.seq1,p.seq2),1,0)) [desc]
            ,case t.FromStockType
			WHEN 'B'THEN 'Bulk'
			WHEN 'I'THEN 'Inventory'
			ELSE t.FromStockType
			end FromStock
			,case t.TostockType
			WHEN 'B'THEN 'Bulk'
			WHEN 'I'THEN 'Inventory'
			ELSE t.FromStockType
			end ToStock
			,dbo.Getlocation(t.FromFtyInventoryUkey) [Location]
			,p.StockUnit,t.fromroll,t.fromdyelot
            ,t.Qty,[Total]=sum(t.Qty) OVER (PARTITION BY t.frompoid ,t.FromSeq1,t.FromSeq2 )           
            from dbo.Borrowback_detail t 
            left join dbo.PO_Supp_Detail p 
            on 
            p.id= t.FromPOID and p.SEQ1 = t.FromSeq1 and p.seq2 = t.FromSeq2 
            where t.id= @ID";
            result1 = DBProxy.Current.Select("", sqlcmd, pars, out dtDetail);
            if (!result1) { this.ShowErr(sqlcmd, result1); }



            // 傳 list 資料            
            List<P32_PrintData> data = dtDetail.AsEnumerable()
                .Select(row1 => new P32_PrintData()
                {

                    StockSEQ = row1["StockSEQ"].ToString(),
                    ToSP = row1["ToSP"].ToString(),
                    DESC = row1["desc"].ToString(),
                    FromStock = row1["FromStock"].ToString(),
                    ToStock = row1["ToStock"].ToString(),
                    Location = row1["Location"].ToString(),
                    Unit = row1["StockUnit"].ToString(),
                    Roll = row1["fromroll"].ToString(),
                    DYELOT = row1["fromdyelot"].ToString(),
                    QTY = row1["Qty"].ToString(),
                    TotalQTY = row1["Total"].ToString()
                }).ToList();

            report.ReportDataSource = data;
            #endregion
            // 指定是哪個 RDLC
            //DualResult result;
            Type ReportResourceNamespace = typeof(P32_PrintData);
            Assembly ReportResourceAssembly = ReportResourceNamespace.Assembly;
            string ReportResourceName = "P32_Print.rdlc";
         

            IReportResource reportresource;
            if (!(result1 = ReportResources.ByEmbeddedResource(ReportResourceAssembly, ReportResourceNamespace, ReportResourceName, out reportresource)))
            {
                //this.ShowException(result);
                return false;
            }
            report.ReportResource = reportresource;

            // 開啟 report view
            var frm = new Sci.Win.Subs.ReportView(report);
            frm.MdiParent = MdiParent;
            frm.Show();
            return true;
        }

        private void MySubreportEventHandler(object sender, SubreportProcessingEventArgs e)
        {
            e.DataSources.Add(new ReportDataSource("DataSet1", (DataTable)this.detailgridbs.DataSource));
        }

        // save前檢查 & 取id
        protected override bool ClickSaveBefore()
        {
            StringBuilder warningmsg = new StringBuilder();

            #region 必輸檢查

            if (MyUtility.Check.Empty(CurrentMaintain["IssueDate"]))
            {
                MyUtility.Msg.WarningBox("< Issue Date >  can't be empty!", "Warning");
                dateBox3.Focus();
                return false;
            }

            if (!MyUtility.Check.Empty(warningmsg.ToString()))
            {
                MyUtility.Msg.WarningBox(warningmsg.ToString());
                return false;
            }

            #endregion 必輸檢查

            foreach (DataRow row in DetailDatas)
            {
                if (MyUtility.Check.Empty(row["fromseq1"]) || MyUtility.Check.Empty(row["fromseq2"]))
                {
                    warningmsg.Append(string.Format(@"SP#: {0} Seq#: {1}-{2} can't be empty"
                        , row["frompoid"], row["fromseq1"], row["fromseq2"])
                        + Environment.NewLine);
                }

                if (MyUtility.Check.Empty(row["Qty"]))
                {
                    warningmsg.Append(string.Format(@"SP#: {0} Seq#: {1}-{2} Roll#:{3} Dyelot:{4} Qty can't be empty"
                        , row["frompoid"], row["fromseq1"], row["fromseq2"], row["fromroll"], row["fromdyelot"]) + Environment.NewLine);
                }

                if (row["fabrictype"].ToString().ToUpper() == "F" && (MyUtility.Check.Empty(row["Toroll"]) || MyUtility.Check.Empty(row["Todyelot"])))
                {
                    warningmsg.Append(string.Format(@"To SP#: {0} To Seq#: {1}-{2} To Roll#:{3} To Dyelot:{4} Roll and Dyelot can't be empty"
                        , row["topoid"], row["toseq1"], row["toseq2"], row["toroll"], row["todyelot"]) + Environment.NewLine);
                }

                if (row["fabrictype"].ToString().ToUpper() != "F")
                {
                    row["toroll"] = "";
                    row["todyelot"] = "";
                }
            }
            if (!MyUtility.Check.Empty(warningmsg.ToString()))
            {
                MyUtility.Msg.WarningBox(warningmsg.ToString());
                return false;
            }

            if (DetailDatas.Count == 0)
            {
                MyUtility.Msg.WarningBox("Detail can't be empty", "Warning");
                return false;
            }

            //取單號
            if (this.IsDetailInserting)
            {
                string tmpId = Sci.MyUtility.GetValue.GetID(Sci.Env.User.Keyword + "RB", "BorrowBack", (DateTime)CurrentMaintain["Issuedate"]);
                if (MyUtility.Check.Empty(tmpId))
                {
                    MyUtility.Msg.WarningBox("Get document ID fail!!");
                    return false;
                }
                CurrentMaintain["id"] = tmpId;
            }

            return base.ClickSaveBefore();
        }

        // grid 加工填值
        protected override DualResult OnRenewDataDetailPost(RenewDataPostEventArgs e)
        {
            return base.OnRenewDataDetailPost(e);
        }

        //refresh
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            #region -- Status Label --

            label25.Text = CurrentMaintain["status"].ToString();

            #endregion Status Label
            string tmp = MyUtility.GetValue.Lookup(string.Format("select estbackdate from borrowback where id='{0}'", CurrentMaintain["borrowid"]));
            dateBox2.Value = null;
            if (!MyUtility.Check.Empty(tmp)) dateBox2.Value =DateTime.Parse(tmp);
        }

        // detail 新增時設定預設值
        protected override void OnDetailGridInsert(int index = -1)
        {
            base.OnDetailGridInsert(index);
        }

        // Detail Grid 設定
        protected override void OnDetailGridSetup()
        {
            Ict.Win.UI.DataGridViewComboBoxColumn cbb_stocktype;
            Ict.Win.UI.DataGridViewComboBoxColumn cbb_stocktype2;

            #region 欄位設定
            Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("frompoid", header: "From SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)  //0
            .Text("fromseq", header: "From" + Environment.NewLine + "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true)  //1
            .Text("fromroll", header: "From" + Environment.NewLine + "Roll", width: Widths.AnsiChars(6), iseditingreadonly: true)  //2
            .Text("fromdyelot", header: "From" + Environment.NewLine + "Dyelot", width: Widths.AnsiChars(6), iseditingreadonly: true)  //3
            .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true) //4
            .ComboBox("fromstocktype", header: "From" + Environment.NewLine + "Stock" + Environment.NewLine + "Type", iseditable: false).Get(out cbb_stocktype)    //5
            .Text("Location", header: "From" + Environment.NewLine + "Location", iseditingreadonly: true)    //6
            .Text("topoid", header: "To" + Environment.NewLine + "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)  //7
            .Text("toseq", header: "To" + Environment.NewLine + "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true)  //8
            .Text("toroll", header: "To" + Environment.NewLine + "Roll", width: Widths.AnsiChars(6))  //9
            .Text("todyelot", header: "To" + Environment.NewLine + "Dyelot", width: Widths.AnsiChars(6), iseditingreadonly: true)  //10
            .Numeric("qty", header: "Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10)    //11
            .Text("stockunit", header: "Stock" + Environment.NewLine + "Unit", iseditingreadonly: true, width: Widths.AnsiChars(5))    //12
            .ComboBox("tostocktype", header: "To" + Environment.NewLine + "Stock" + Environment.NewLine + "Type", iseditable: false).Get(out cbb_stocktype2)    //13
            ;     //
            #endregion 欄位設定
            this.detailgrid.Columns[9].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns[11].DefaultCellStyle.BackColor = Color.Pink;

            cbb_stocktype.DataSource = new BindingSource(di_stocktype, null);
            cbb_stocktype.ValueMember = "Key";
            cbb_stocktype.DisplayMember = "Value";

            cbb_stocktype2.DataSource = new BindingSource(di_stocktype, null);
            cbb_stocktype2.ValueMember = "Key";
            cbb_stocktype2.DisplayMember = "Value";
        }

        //Confirm
        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            var dr = this.CurrentMaintain;
            if (null == dr) return;

            string sqlupd2_A = "";
            string sqlupd2_B = "";
            string sqlupd2_BI = "";
            String sqlupd2_FIO = "";
            String sqlupd2_FIO2 = "";

            StringBuilder sqlupd2 = new StringBuilder();
            String sqlcmd = "", sqlupd3 = "", ids = "";
            DualResult result, result2;
            DataTable datacheck;

            string backdate = MyUtility.GetValue.Lookup(string.Format(@"select [backdate] from dbo.borrowback where id='{0}' and type='A' and mdivisionid='{1}'"
                , CurrentMaintain["borrowid"], Sci.Env.User.Keyword));
            if (!MyUtility.Check.Empty(backdate))
            {
                MyUtility.Msg.WarningBox(string.Format("This borrow id ({0}) already returned.", CurrentMaintain["borrowid"]), "Can't Confirmed");
                return;
            }

            #region -- 檢查庫存項lock --
            sqlcmd = string.Format(@"Select d.frompoid,d.fromseq1,d.fromseq2,d.fromRoll,d.Qty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty
from dbo.BorrowBack_Detail d inner join FtyInventory f
on d.FromMDivisionID = f.MDivisionID and d.FromPOID = f.POID  AND D.FromStockType = F.StockType
and d.FromRoll = f.Roll and d.FromSeq1 =f.Seq1 and d.FromSeq2 = f.Seq2
where f.lock=1 and d.Id = '{0}'", CurrentMaintain["id"]);
            if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                ShowErr(sqlcmd, result2);
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    foreach (DataRow tmp in datacheck.Rows)
                    {
                        ids += string.Format("SP#: {0} Seq#: {1}-{2} Roll#: {3} is locked!!" + Environment.NewLine
                            , tmp["frompoid"], tmp["fromseq1"], tmp["fromseq2"], tmp["fromroll"]);
                    }
                    MyUtility.Msg.WarningBox("Material Locked!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }
            #endregion

            #region -- 檢查負數庫存 --

            sqlcmd = string.Format(@"Select d.frompoid,d.fromseq1,d.fromseq2,d.fromRoll,d.Qty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty
from dbo.BorrowBack_Detail d left join FtyInventory f
on d.FromMDivisionID = f.MDivisionID and d.FromPOID = f.POID  AND D.FromStockType = F.StockType
and d.FromRoll = f.Roll and d.FromSeq1 =f.Seq1 and d.FromSeq2 = f.Seq2
where (isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) - d.Qty < 0) and d.Id = '{0}'", CurrentMaintain["id"]);
            if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                ShowErr(sqlcmd, result2);
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    foreach (DataRow tmp in datacheck.Rows)
                    {
                        ids += string.Format("SP#: {0} Seq#: {1}-{2} Roll#: {3}'s balance: {4} is less than qty: {5}" + Environment.NewLine
                            , tmp["frompoid"], tmp["fromseq1"], tmp["fromseq2"], tmp["fromroll"], tmp["balanceqty"], tmp["qty"]);
                    }
                    MyUtility.Msg.WarningBox("Balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }

            #endregion -- 檢查負數庫存 --

            #region -- 檢查目的Roll是否已存在資料 --

            sqlcmd = string.Format(@"Select d.ToPoid,d.ToSeq1,d.toseq2,d.ToRoll,d.ToDyelot,d.Qty
,f.InQty
from dbo.BorrowBack_Detail d inner join FtyInventory f
on d.ToMdivisionid = f.mdivisionid
and d.ToPoid = f.PoId
and d.ToSeq1 = f.Seq1
and d.toseq2 = f.seq2
and d.ToStocktype = f.StockType
and d.ToRoll = f.Roll
and d.ToDyelot != f.dyelot
where f.InQty > 0 and toroll !='' and toroll is not null and d.Id = '{0}'", CurrentMaintain["id"]);
            if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                ShowErr(sqlcmd, result2);
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    foreach (DataRow tmp in datacheck.Rows)
                    {
                        ids += string.Format("Seq#: {1}-{2} Roll#: {3} exist in SP#: {0} but dyelot is not {4}" + Environment.NewLine
                            , tmp["topoid"], tmp["toseq1"], tmp["toseq2"], tmp["toroll"],tmp["todyelot"]);
                    }
                    MyUtility.Msg.WarningBox(ids + Environment.NewLine + "Please change roll# !!", "Warning");
                    return;
                }
            }

            #endregion

            #region -- 更新表頭狀態資料 --

            sqlupd3 = string.Format(@"update BorrowBack set status='Confirmed', editname = '{0}' , editdate = GETDATE()
                                where id = '{1}'", Env.User.UserID, CurrentMaintain["id"]);

            #endregion 更新表頭狀態資料
            #region -- 更新mdivisionPoDetail 還出數 --
            var bs1 = (from b in ((DataTable)detailgridbs.DataSource).AsEnumerable()
                       group b by new
                       {
                           mdivisionid = b.Field<string>("frommdivisionid").Trim(),
                           poid = b.Field<string>("frompoid").Trim(),
                           seq1 = b.Field<string>("fromseq1").Trim(),
                           seq2 = b.Field<string>("fromseq2").Trim(),
                           stocktype = b.Field<string>("fromstocktype").Trim()
                       } into m
                       select new
                       {
                           mdivisionid = m.First().Field<string>("frommdivisionid"),
                           poid = m.First().Field<string>("frompoid"),
                           seq1 = m.First().Field<string>("fromseq1"),
                           seq2 = m.First().Field<string>("fromseq2"),
                           stocktype = m.First().Field<string>("fromstocktype"),
                           qty = m.Sum(w => w.Field<decimal>("qty"))
                       }).ToList();
            var bs1I = (from b in ((DataTable)detailgridbs.DataSource).AsEnumerable().Where(w => w.Field<string>("fromstocktype").Trim() == "I")
                       group b by new
                       {
                           mdivisionid = b.Field<string>("frommdivisionid").Trim(),
                           poid = b.Field<string>("frompoid").Trim(),
                           seq1 = b.Field<string>("fromseq1").Trim(),
                           seq2 = b.Field<string>("fromseq2").Trim(),
                           stocktype = b.Field<string>("fromstocktype").Trim()
                       } into m
                        select new Prgs_POSuppDetailData
                       {
                           mdivisionid = m.First().Field<string>("frommdivisionid"),
                           poid = m.First().Field<string>("frompoid"),
                           seq1 = m.First().Field<string>("fromseq1"),
                           seq2 = m.First().Field<string>("fromseq2"),
                           stocktype = m.First().Field<string>("fromstocktype"),
                           qty = m.Sum(w => w.Field<decimal>("qty"))
                       }).ToList();

            if (bs1.Count > 0)
                sqlupd2_B = Prgs.UpdateMPoDetail(4, null, true);
            if (bs1I.Count > 0)
                sqlupd2_BI = Prgs.UpdateMPoDetail(8, bs1I, true);
            #endregion
            #region -- 更新mdivisionPoDetail 還回數 --
            var bs2 = (from b in ((DataTable)detailgridbs.DataSource).AsEnumerable()
                   group b by new
                   {
                       mdivisionid = b.Field<string>("tomdivisionid").Trim(),
                       poid = b.Field<string>("topoid").Trim(),
                       seq1 = b.Field<string>("toseq1").Trim(),
                       seq2 = b.Field<string>("toseq2").Trim(),
                       stocktype = b.Field<string>("tostocktype").Trim()
                   } into m
                       select new Prgs_POSuppDetailData
                   {
                       mdivisionid = m.First().Field<string>("tomdivisionid"),
                       poid = m.First().Field<string>("topoid"),
                       seq1 = m.First().Field<string>("toseq1"),
                       seq2 = m.First().Field<string>("toseq2"),
                       stocktype = m.First().Field<string>("tostocktype"),
                       qty = m.Sum(w => w.Field<decimal>("qty"))
                   }).ToList();
            sqlupd2_A = Prgs.UpdateMPoDetail(2, bs2, true);
            #endregion 

            #region -- 更新庫存數量  ftyinventory --
            var bsfio = (from m in ((DataTable)detailgridbs.DataSource).AsEnumerable()
                         select new
                         {
                             mdivisionid = m.Field<string>("fromMdivisionid"),
                             poid = m.Field<string>("frompoid"),
                             seq1 = m.Field<string>("fromseq1"),
                             seq2 = m.Field<string>("fromseq2"),
                             stocktype = m.Field<string>("fromstocktype"),
                             qty = m.Field<decimal>("qty"),
                             location = m.Field<string>("location"),
                             roll = m.Field<string>("fromroll"),
                             dyelot = m.Field<string>("fromdyelot"),
                         }).ToList();
            var bsfioto = (from m in ((DataTable)detailgridbs.DataSource).AsEnumerable()
                           select new
                           {
                               mdivisionid = m.Field<string>("toMdivisionid"),
                               poid = m.Field<string>("topoid"),
                               seq1 = m.Field<string>("toseq1"),
                               seq2 = m.Field<string>("toseq2"),
                               stocktype = m.Field<string>("tostocktype"),
                               qty = m.Field<decimal>("qty"),
                               location = m.Field<string>("location"),
                               roll = m.Field<string>("toroll"),
                               dyelot = m.Field<string>("todyelot"),
                           }).ToList();
            sqlupd2_FIO = Prgs.UpdateFtyInventory_IO(4, null, false);
            sqlupd2_FIO2 = Prgs.UpdateFtyInventory_IO(2, null, false);
            #endregion 更新庫存數量  ftyinventory

            #region -- 更新全數歸還日期 --
            sqlupd2.Append(string.Format(@";declare @reccount as int;
with acc
as
(
select bd1.ToPoid,bd1.ToSeq1,bd1.ToSeq2,sum(bd1.qty) qty
from dbo.BorrowBack b1 inner join dbo.BorrowBack_Detail bd1 on b1.id = bd1.id 
where b1.BorrowId='{1}' and b1.Status = 'Confirmed'
group by bd1.ToPoid,bd1.ToSeq1,bd1.ToSeq2
)
, borrow
as
(
select bd.FromPoId,bd.FromSeq1,bd.FromSeq2,sum(bd.Qty) borrowedqty
from dbo.BorrowBack_Detail bd 
left join PO_Supp_Detail p on p.id = bd.FromPoId and p.SEQ1 = bd.FromSeq1 and p.SEQ2 = bd.FromSeq2
where bd.id='{1}'
group by bd.FromPoId,bd.FromSeq1,bd.FromSeq2
)
select @reccount = count(*)
from borrow left join acc on borrow.FromPoId = acc.ToPoid and borrow.FromSeq1 = acc.ToSeq1 and borrow.FromSeq2 = acc.ToSeq2
where borrowedqty > isnull(acc.qty,0.00);
if @reccount = 0 
begin
	update dbo.BorrowBack set BackDate = '{2}' where id = '{1}'
end 
else
begin
	update dbo.BorrowBack set BackDate = DEFAULT where id = '{1}'
end", CurrentMaintain["id"].ToString(), CurrentMaintain["borrowid"], DateTime.Parse(CurrentMaintain["issuedate"].ToString()).ToShortDateString()));
            #endregion

            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    DataTable resulttb;
                    if (bs1.Count > 0)
                    {
                        if (!(result = MyUtility.Tool.ProcessWithObject(bs1, "", sqlupd2_B, out resulttb, "#TmpSource")))
                        {
                            _transactionscope.Dispose();
                            ShowErr(result);
                            return;
                        }
                    }
                    if (bs1I.Count > 0)
                    {
                        if (!(result = MyUtility.Tool.ProcessWithObject(bs1I, "", sqlupd2_BI, out resulttb, "#TmpSource")))
                        {
                            _transactionscope.Dispose();
                            ShowErr(result);
                            return;
                        }
                    }
                    if (!(result = MyUtility.Tool.ProcessWithObject(bs2, "", sqlupd2_A, out resulttb, "#TmpSource")))
                    {
                        _transactionscope.Dispose();
                        ShowErr(result);
                        return;
                    }
                    if (!(result = MyUtility.Tool.ProcessWithObject(bsfio, "", sqlupd2_FIO, out resulttb, "#TmpSource")))
                    {
                        _transactionscope.Dispose();
                        ShowErr(result);
                        return;
                    }
                    if (!(result = MyUtility.Tool.ProcessWithObject(bsfioto, "", sqlupd2_FIO2, out resulttb, "#TmpSource")))
                    {
                        _transactionscope.Dispose();
                        ShowErr(result);
                        return;
                    }
                    if (!(result = DBProxy.Current.Execute(null, sqlupd3)))
                    {
                        _transactionscope.Dispose();
                        ShowErr(sqlupd3, result);
                        return;
                    }
                    if (!(result2 = DBProxy.Current.Execute(null, sqlupd2.ToString())))
                    {
                        _transactionscope.Dispose();
                        ShowErr(sqlupd2.ToString(), result2);
                        return;
                    }

                    _transactionscope.Complete();
                    MyUtility.Msg.InfoBox("Confirmed successful");
                }
                catch (Exception ex)
                {
                    _transactionscope.Dispose();
                    ShowErr("Commit transaction error.", ex);
                    return;
                }
            }
            _transactionscope.Dispose();
            _transactionscope = null;
            this.RenewData();
            this.OnDetailEntered();
            this.EnsureToolbarExt();
        }

        //Unconfirm
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            DataTable datacheck;
            DataTable dt = (DataTable)detailgridbs.DataSource;

            string sqlupd2_B = "";
            string sqlupd2_BI = "";
            string sqlupd2_A = "";
            String sqlupd2_FIO = "";
            String sqlupd2_FIO2 = "";

            DialogResult dResult = MyUtility.Msg.QuestionBox("Do you want to unconfirme it?");
            if (dResult.ToString().ToUpper() == "NO") return;
            var dr = this.CurrentMaintain; if (null == dr) return;
            StringBuilder sqlupd2 = new StringBuilder();
            string sqlcmd = "", sqlupd3 = "", ids = "";
            DualResult result, result2;

            #region -- 檢查庫存項lock --
            sqlcmd = string.Format(@"Select d.topoid,d.toseq1,d.toseq2,d.toRoll,d.Qty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty
from dbo.BorrowBack_Detail d inner join FtyInventory f
on d.toMdivisionid = f.Mdivisionid
and d.toPoId = f.PoId
and d.toSeq1 = f.Seq1
and d.toSeq2 = f.seq2
and d.toStocktype = f.StockType
and d.toRoll = f.Roll
where f.lock=1 and d.Id = '{0}'", CurrentMaintain["id"]);
            if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                ShowErr(sqlcmd, result2);
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    foreach (DataRow tmp in datacheck.Rows)
                    {
                        ids += string.Format("SP#: {0} Seq#: {1}-{2} Roll#: {3} is locked!!" + Environment.NewLine
                            , tmp["topoid"], tmp["toseq1"], tmp["toseq2"], tmp["toroll"]);
                    }
                    MyUtility.Msg.WarningBox("Material Locked!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }
            #endregion

            #region -- 檢查負數庫存 --

            sqlcmd = string.Format(@"Select d.topoid,d.toseq1,d.toseq2,d.toRoll,d.Qty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty
from dbo.BorrowBack_Detail d left join FtyInventory f
on d.toMdivisionid = f.Mdivisionid
and d.toPoId = f.PoId
and d.toSeq1 = f.Seq1
and d.toSeq2 = f.seq2
and d.toStocktype = f.StockType
and d.toRoll = f.Roll
where (isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) - d.Qty < 0) and d.Id = '{0}'", CurrentMaintain["id"]);
            if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                ShowErr(sqlcmd, result2);
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    foreach (DataRow tmp in datacheck.Rows)
                    {
                        ids += string.Format("SP#: {0} Seq#: {1}-{2} Roll#: {3}'s balance: {4} is less than borrowed qty: {5}" + Environment.NewLine
                            , tmp["topoid"], tmp["toseq1"], tmp["toseq2"], tmp["toroll"], tmp["balanceqty"], tmp["qty"]);
                    }
                    MyUtility.Msg.WarningBox("Balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }

            #endregion 檢查負數庫存

            #region -- 更新表頭狀態資料 --

            sqlupd3 = string.Format(@"update BorrowBack set status='New', editname = '{0}' , editdate = GETDATE()
                                where id = '{1}'", Env.User.UserID, CurrentMaintain["id"]);

            #endregion 更新表頭狀態資料

            #region -- 更新MdivisionPoDetail 借出數 --
            var bs1 = (from b in ((DataTable)detailgridbs.DataSource).AsEnumerable()
                       group b by new
                       {
                           mdivisionid = b.Field<string>("frommdivisionid").Trim(),
                           poid = b.Field<string>("frompoid").Trim(),
                           seq1 = b.Field<string>("fromseq1").Trim(),
                           seq2 = b.Field<string>("fromseq2").Trim(),
                           stocktype = b.Field<string>("fromstocktype").Trim()
                       } into m
                       select new
                       {
                           mdivisionid = m.First().Field<string>("frommdivisionid"),
                           poid = m.First().Field<string>("frompoid"),
                           seq1 = m.First().Field<string>("fromseq1"),
                           seq2 = m.First().Field<string>("fromseq2"),
                           stocktype = m.First().Field<string>("fromstocktype"),
                           qty = - (m.Sum(w => w.Field<decimal>("qty")))
                       }).ToList();
            var bs1I = (from b in ((DataTable)detailgridbs.DataSource).AsEnumerable().Where(w => w.Field<string>("fromstocktype") == "I")
                       group b by new
                       {
                           mdivisionid = b.Field<string>("frommdivisionid").Trim(),
                           poid = b.Field<string>("frompoid").Trim(),
                           seq1 = b.Field<string>("fromseq1").Trim(),
                           seq2 = b.Field<string>("fromseq2").Trim(),
                           stocktype = b.Field<string>("fromstocktype").Trim()
                       } into m
                        select new Prgs_POSuppDetailData
                       {
                           mdivisionid = m.First().Field<string>("frommdivisionid"),
                           poid = m.First().Field<string>("frompoid"),
                           seq1 = m.First().Field<string>("fromseq1"),
                           seq2 = m.First().Field<string>("fromseq2"),
                           stocktype = m.First().Field<string>("fromstocktype"),
                           qty = - (m.Sum(w => w.Field<decimal>("qty")))
                       }).ToList();
            if (bs1.Count > 0)
                sqlupd2_B = Prgs.UpdateMPoDetail(4, null, false);
            if (bs1I.Count > 0)
                sqlupd2_BI = Prgs.UpdateMPoDetail(8, bs1I, false);
            #endregion
            #region -- 更新MdivisionPoDetail 借入數 --
            var bs2 = (from b in ((DataTable)detailgridbs.DataSource).AsEnumerable()
                   group b by new
                   {
                       mdivisionid = b.Field<string>("tomdivisionid").Trim(),
                       poid = b.Field<string>("topoid").Trim(),
                       seq1 = b.Field<string>("toseq1").Trim(),
                       seq2 = b.Field<string>("toseq2").Trim(),
                       stocktype = b.Field<string>("tostocktype").Trim()
                   } into m
                       select new Prgs_POSuppDetailData
                   {
                       mdivisionid = m.First().Field<string>("tomdivisionid"),
                       poid = m.First().Field<string>("topoid"),
                       seq1 = m.First().Field<string>("toseq1"),
                       seq2 = m.First().Field<string>("toseq2"),
                       stocktype = m.First().Field<string>("tostocktype"),
                       qty = - (m.Sum(w => w.Field<decimal>("qty")))
                   }).ToList();

            sqlupd2_A = Prgs.UpdateMPoDetail(2, bs2, false);
            #endregion

            #region -- 更新庫存數量  ftyinventory --
            var bsfio = (from m in ((DataTable)detailgridbs.DataSource).AsEnumerable()
                         select new
                         {
                             mdivisionid = m.Field<string>("fromMdivisionid"),
                             poid = m.Field<string>("frompoid"),
                             seq1 = m.Field<string>("fromseq1"),
                             seq2 = m.Field<string>("fromseq2"),
                             stocktype = m.Field<string>("fromstocktype"),
                             qty = - (m.Field<decimal>("qty")),
                             location = m.Field<string>("location"),
                             roll = m.Field<string>("fromroll"),
                             dyelot = m.Field<string>("fromdyelot"),
                         }).ToList();
            var bsfioto = (from m in ((DataTable)detailgridbs.DataSource).AsEnumerable()
                           select new
                           {
                               mdivisionid = m.Field<string>("toMdivisionid"),
                               poid = m.Field<string>("topoid"),
                               seq1 = m.Field<string>("toseq1"),
                               seq2 = m.Field<string>("toseq2"),
                               stocktype = m.Field<string>("tostocktype"),
                               qty = - (m.Field<decimal>("qty")),
                               location = m.Field<string>("location"),
                               roll = m.Field<string>("toroll"),
                               dyelot = m.Field<string>("todyelot"),
                           }).ToList();
            sqlupd2_FIO = Prgs.UpdateFtyInventory_IO(4, null, false);
            sqlupd2_FIO2 = Prgs.UpdateFtyInventory_IO(2, null, false);    
            #endregion 更新庫存數量  ftyinventory

            #region -- 更新全數歸還日期 --
            sqlupd2.Append(string.Format(@";declare @reccount as int;
with acc
as
(
select bd1.ToPoid,bd1.ToSeq1,bd1.ToSeq2,sum(bd1.qty) qty
from dbo.BorrowBack b1 inner join dbo.BorrowBack_Detail bd1 on b1.id = bd1.id 
where b1.BorrowId='{1}' and b1.Status = 'Confirmed'
group by bd1.ToPoid,bd1.ToSeq1,bd1.ToSeq2
)
, borrow
as
(
select bd.FromPoId,bd.FromSeq1,bd.FromSeq2,sum(bd.Qty) borrowedqty
from dbo.BorrowBack_Detail bd 
left join PO_Supp_Detail p on p.id = bd.FromPoId and p.SEQ1 = bd.FromSeq1 and p.SEQ2 = bd.FromSeq2
where bd.id='{1}'
group by bd.FromPoId,bd.FromSeq1,bd.FromSeq2
)
select @reccount = count(*)
from borrow left join acc on borrow.FromPoId = acc.ToPoid and borrow.FromSeq1 = acc.ToSeq1 and borrow.FromSeq2 = acc.ToSeq2
where borrowedqty > isnull(acc.qty,0.00);
if @reccount = 0 
begin
	update dbo.BorrowBack set BackDate = '{2}' where id = '{1}'
end 
else
begin
	update dbo.BorrowBack set BackDate = DEFAULT where id = '{1}'
end", CurrentMaintain["id"].ToString(),CurrentMaintain["borrowid"],DateTime.Parse(CurrentMaintain["issuedate"].ToString()).ToShortDateString()));
            #endregion

            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    DataTable resulttb;
                    if (bs1.Count > 0)
                    {
                        if (!(result = MyUtility.Tool.ProcessWithObject(bs1, "", sqlupd2_B, out resulttb, "#TmpSource")))
                        {
                            _transactionscope.Dispose();
                            ShowErr(result);
                            return;
                        }
                    }
                    if (bs1I.Count > 0)
                    {
                        if (!(result = MyUtility.Tool.ProcessWithObject(bs1I, "", sqlupd2_BI, out resulttb, "#TmpSource")))
                        {
                            _transactionscope.Dispose();
                            ShowErr(result);
                            return;
                        }
                    }
                    if (!(result = MyUtility.Tool.ProcessWithObject(bs2, "", sqlupd2_A, out resulttb, "#TmpSource")))
                    {
                        _transactionscope.Dispose();
                        ShowErr(result);
                        return;
                    }
                    if (!(result = MyUtility.Tool.ProcessWithObject(bsfio, "", sqlupd2_FIO, out resulttb, "#TmpSource")))
                    {
                        _transactionscope.Dispose();
                        ShowErr(result);
                        return;
                    }
                    if (!(result = MyUtility.Tool.ProcessWithObject(bsfioto, "", sqlupd2_FIO2, out resulttb, "#TmpSource")))
                    {
                        _transactionscope.Dispose();
                        ShowErr(result);
                        return;
                    }
                    if (!(result = DBProxy.Current.Execute(null, sqlupd3)))
                    {
                        _transactionscope.Dispose();
                        ShowErr(sqlupd3, result);
                        return;
                    }

                    if (!(result2 = DBProxy.Current.Execute(null, sqlupd2.ToString())))
                    {
                        _transactionscope.Dispose();
                        ShowErr(sqlupd2.ToString(), result2);
                        return;
                    }

                    _transactionscope.Complete();
                    MyUtility.Msg.InfoBox("UnConfirmed successful");
                }
                catch (Exception ex)
                {
                    _transactionscope.Dispose();
                    ShowErr("Commit transaction error.", ex);
                    return;
                }
            }
            _transactionscope.Dispose();
            _transactionscope = null;
            this.RenewData();
            this.OnDetailEntered();
            this.EnsureToolbarExt();
        }

        //寫明細撈出的sql command
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["ID"].ToString();
            this.DetailSelectCommand = string.Format(@"select a.id,a.FromFtyinventoryUkey,a.FromMdivisionid,a.FromPoId,a.FromSeq1,a.FromSeq2
,left(a.FromSeq1+' ',3)+a.FromSeq2 as Fromseq
,p1.FabricType
,p1.stockunit
,dbo.getmtldesc(a.FromPoId,a.FromSeq1,a.FromSeq2,2,0) as [description]
,a.FromRoll
,a.FromDyelot
,a.FromStocktype
,a.Qty
,a.ToMdivisionId
,a.ToPoid,a.ToSeq1,a.ToSeq2,left(a.ToSeq1+' ',3)+a.ToSeq2 as toseq
,a.ToRoll
,a.ToDyelot
,a.ToStocktype
,a.ukey
,stuff((select ',' + mtllocationid from (select mtllocationid from dbo.ftyinventory_detail fd where ukey= a.fromftyinventoryukey)t for xml path('')), 1, 1, '') location
from dbo.BorrowBack_detail a left join PO_Supp_Detail p1 on p1.ID = a.FromPoId and p1.seq1 = a.FromSeq1 and p1.SEQ2 = a.FromSeq2
Where a.id = '{0}'", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        //delete all
        private void button9_Click(object sender, EventArgs e)
        {
            detailgrid.ValidateControl();
            //detailgridbs.EndEdit();
            ((DataTable)detailgridbs.DataSource).Select("qty=0.00 or qty is null").ToList().ForEach(r => r.Delete());

        }

        //Import
        private void button5_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(CurrentMaintain["borrowid"]))
            {
                MyUtility.Msg.WarningBox("Borrow Id can't be empty!!");
                textBox2.Focus();
                return;
            }
            var frm = new Sci.Production.Warehouse.P32_Import(CurrentMaintain, (DataTable)detailgridbs.DataSource);
            frm.ShowDialog(this);
            this.RenewData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var frm = new Sci.Production.Warehouse.P32_AccumulatedQty(CurrentMaintain);
            frm.P32 = this;
            frm.ShowDialog(this);
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(detailgridbs.DataSource)) return;
            int index = detailgridbs.Find("frompoid", textBox1.Text.TrimEnd());
            if (index == -1)
            { MyUtility.Msg.WarningBox("Data was not found!!"); }
            else
            { detailgridbs.Position = index; }
        }

        //borrow id
        private void textBox2_Validating(object sender, CancelEventArgs e)
        {
            DataRow dr;
            if (!MyUtility.Check.Seek(string.Format(@"select [status],[backdate] from dbo.borrowback where id='{0}' and type='A' and mdivisionid='{1}'"
                , textBox2.Text, Sci.Env.User.Keyword), out dr, null))
            {
                e.Cancel = true;
                MyUtility.Msg.WarningBox("Please check borrow id is existed.", "Data not found!!");
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
                    MyUtility.Msg.WarningBox(string.Format("This borrow# ({0}) already returned.", textBox2.Text));
                    return;
                }

            }
            CurrentMaintain["BorrowId"] = textBox2.Text;
        }
    }
}