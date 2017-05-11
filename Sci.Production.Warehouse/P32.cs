﻿using Ict;
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
            //MDivisionID 是 P32 寫入 => Sci.Env.User.Keyword
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
            CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;
            CurrentMaintain["Status"] = "New";
            CurrentMaintain["Type"] = "B";
            CurrentMaintain["IssueDate"] = DateTime.Now;
        }

        // delete前檢查
        protected override bool ClickDeleteBefore()
        {
            if (CurrentMaintain["Status"].EqualString("CONFIRMED"))
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't delete.", "Warning");
                return false;
            }
            return base.ClickDeleteBefore();
        }

        // edit前檢查
        protected override bool ClickEditBefore()
        {
            if (CurrentMaintain["Status"].EqualString("CONFIRMED"))
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't modify.", "Warning");
                return false;
            }
            return base.ClickEditBefore();
        }

        //print
        protected override bool ClickPrint()
        {
            //DataRow dr = grid.GetDataRow<DataRow>(grid.GetSelectedRowIndex());
            if (CurrentMaintain["status"].ToString().ToUpper() != "CONFIRMED")
            {
                MyUtility.Msg.WarningBox("Data is not confirmed, can't print.", "Warning");
                return false;
            }
            DataRow row = this.CurrentMaintain;
            string id = row["ID"].ToString();
            string Remark = row["Remark"].ToString();
            string M = CurrentMaintain["MdivisionID"].ToString();
            string issuedate = ((DateTime)MyUtility.Convert.GetDate(row["issuedate"])).ToShortDateString();
            #region -- 撈表頭資料 --
            List<SqlParameter> pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));
            DataTable dt1;
            DualResult result1 = DBProxy.Current.Select("",
            @"select    
            b.name 
            from dbo.Borrowback  a WITH (NOLOCK) 
            inner join dbo.mdivision  b WITH (NOLOCK) 
            on b.id = a.mdivisionid
            where b.id = a.mdivisionid
            and a.id = @ID", pars, out dt1);
            if (!result1) { this.ShowErr(result1); }

            if (dt1 == null || dt1.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found!!", "DataTable dt1");
                return false;
            }

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
            string sqlcmd = @"
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
        ,p.StockUnit
        ,t.fromroll
        ,t.fromdyelot
        ,t.Qty
        ,[Total]=sum(t.Qty) OVER (PARTITION BY t.frompoid ,t.FromSeq1,t.FromSeq2 )           
from dbo.Borrowback_detail t WITH (NOLOCK) 
left join dbo.PO_Supp_Detail p WITH (NOLOCK) on p.id= t.FromPOID and p.SEQ1 = t.FromSeq1 and p.seq2 = t.FromSeq2 
left join dbo.FtyInventory FI on t.fromPoid = fi.poid and t.fromSeq1 = fi.seq1 and t.fromSeq2 = fi.seq2
    and t.fromRoll = fi.roll and t.fromStocktype = fi.stocktype
where t.id= @ID";
            result1 = DBProxy.Current.Select("", sqlcmd, pars, out dtDetail);
            if (!result1) { this.ShowErr(sqlcmd, result1); }

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
                    Location = row1["Location"].ToString().Trim(),
                    Unit = row1["StockUnit"].ToString().Trim(),
                    Roll = row1["fromroll"].ToString().Trim(),
                    DYELOT = row1["fromdyelot"].ToString().Trim(),
                    QTY = row1["Qty"].ToString().Trim(),
                    TotalQTY = row1["Total"].ToString().Trim()
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
                dateIssueDate.Focus();
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

            if (!checkRoll())
                return false;

            //取單號
            if (this.IsDetailInserting)
            {
                string tmpId = Sci.MyUtility.GetValue.GetID(Sci.Env.User.Factory + "RB", "BorrowBack", (DateTime)CurrentMaintain["Issuedate"]);
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
            string tmp = MyUtility.GetValue.Lookup(string.Format("select estbackdate from borrowback WITH (NOLOCK) where id='{0}'", CurrentMaintain["borrowid"]));
            dateEstReturnDate.Value = null;
            if (!MyUtility.Check.Empty(tmp)) dateEstReturnDate.Value =DateTime.Parse(tmp);
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
            .Text("todyelot", header: "To" + Environment.NewLine + "Dyelot", width: Widths.AnsiChars(6))  //10
            .Numeric("qty", header: "Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10)    //11
            .Text("stockunit", header: "Stock" + Environment.NewLine + "Unit", iseditingreadonly: true, width: Widths.AnsiChars(5))    //12
            .ComboBox("tostocktype", header: "To" + Environment.NewLine + "Stock" + Environment.NewLine + "Type", iseditable: false).Get(out cbb_stocktype2)    //13
            ;     //
            #endregion 欄位設定
            this.detailgrid.Columns["toroll"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["todyelot"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["qty"].DefaultCellStyle.BackColor = Color.Pink;

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

            string backdate = MyUtility.GetValue.Lookup(string.Format(@"select [backdate] from dbo.borrowback WITH (NOLOCK) where id='{0}' and type='A'"
                , CurrentMaintain["borrowid"]));
            if (!MyUtility.Check.Empty(backdate))
            {
                MyUtility.Msg.WarningBox(string.Format("This borrow id ({0}) already returned.", CurrentMaintain["borrowid"]), "Can't Confirmed");
                return;
            }

            #region -- 檢查庫存項lock --
            sqlcmd = string.Format(@"
Select  d.frompoid
        ,d.fromseq1
        ,d.fromseq2
        ,d.fromRoll
        ,d.Qty
        ,balanceQty = isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) 
from dbo.BorrowBack_Detail d WITH (NOLOCK) 
inner join FtyInventory f WITH (NOLOCK) on d.FromPOID = f.POID  AND D.FromStockType = F.StockType
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

            sqlcmd = string.Format(@"
Select  d.frompoid
        ,d.fromseq1
        ,d.fromseq2
        ,d.fromRoll
        ,d.Qty
        ,balanceQty = isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0)
from dbo.BorrowBack_Detail d WITH (NOLOCK) 
left join FtyInventory f WITH (NOLOCK) on d.FromPOID = f.POID  AND D.FromStockType = F.StockType
    and d.FromRoll = f.Roll and d.FromSeq1 =f.Seq1 and d.FromSeq2 = f.Seq2
where (isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) - d.Qty < 0) 
    and d.Id = '{0}'", CurrentMaintain["id"]);
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
            if (!checkRoll())
                return;
            #endregion

            #region -- 更新表頭狀態資料 --

            sqlupd3 = string.Format(@"
update BorrowBack 
set status='Confirmed'
    , editname = '{0}' 
    , editdate = GETDATE()
where id = '{1}'", Env.User.UserID, CurrentMaintain["id"]);

            #endregion 更新表頭狀態資料
            #region -- 更新mdivisionPoDetail 還出數 --
            var bs1 = (from b in ((DataTable)detailgridbs.DataSource).AsEnumerable()
                       group b by new
                       {
                           poid = b.Field<string>("frompoid").Trim(),
                           seq1 = b.Field<string>("fromseq1").Trim(),
                           seq2 = b.Field<string>("fromseq2").Trim(),
                           stocktype = b.Field<string>("fromstocktype").Trim()
                       } into m
                       select new
                       {
                           poid = m.First().Field<string>("frompoid"),
                           seq1 = m.First().Field<string>("fromseq1"),
                           seq2 = m.First().Field<string>("fromseq2"),
                           stocktype = m.First().Field<string>("fromstocktype"),
                           qty = m.Sum(w => w.Field<decimal>("qty"))
                       }).ToList();
            var bs1I = (from b in ((DataTable)detailgridbs.DataSource).AsEnumerable().Where(w => w.Field<string>("fromstocktype").Trim() == "I")
                       group b by new
                       {
                           poid = b.Field<string>("frompoid").Trim(),
                           seq1 = b.Field<string>("fromseq1").Trim(),
                           seq2 = b.Field<string>("fromseq2").Trim(),
                           stocktype = b.Field<string>("fromstocktype").Trim()
                       } into m
                        select new Prgs_POSuppDetailData
                       {
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
                       poid = b.Field<string>("topoid").Trim(),
                       seq1 = b.Field<string>("toseq1").Trim(),
                       seq2 = b.Field<string>("toseq2").Trim(),
                       stocktype = b.Field<string>("tostocktype").Trim()
                   } into m
                       select new Prgs_POSuppDetailData
                   {
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
            sqlupd2.Append(string.Format(@"
;declare @reccount as int;

with acc as(
    select  bd1.ToPoid
            ,bd1.ToSeq1
            ,bd1.ToSeq2
            ,qty = sum(bd1.qty) 
    from dbo.BorrowBack b1 WITH (NOLOCK) 
    inner join dbo.BorrowBack_Detail bd1 WITH (NOLOCK) on b1.id = bd1.id 
    where b1.BorrowId='{1}' and b1.Status = 'Confirmed'
    group by bd1.ToPoid,bd1.ToSeq1,bd1.ToSeq2
    )
, borrow as(
    select  bd.FromPoId
            ,bd.FromSeq1
            ,bd.FromSeq2
            ,borrowedqty = sum(bd.Qty) 
    from dbo.BorrowBack_Detail bd WITH (NOLOCK) 
    left join PO_Supp_Detail p WITH (NOLOCK) on p.id = bd.FromPoId and p.SEQ1 = bd.FromSeq1 and p.SEQ2 = bd.FromSeq2
    where bd.id='{1}'
    group by bd.FromPoId, bd.FromSeq1, bd.FromSeq2
    )
select @reccount = count(*)
from borrow 
left join acc on borrow.FromPoId = acc.ToPoid and borrow.FromSeq1 = acc.ToSeq1 and borrow.FromSeq2 = acc.ToSeq2
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
                    _transactionscope.Dispose();
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
            sqlcmd = string.Format(@"
Select  d.topoid
        ,d.toseq1
        ,d.toseq2
        ,d.toRoll
        ,d.Qty
        ,balanceQty = isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) 
from dbo.BorrowBack_Detail d WITH (NOLOCK) 
inner join FtyInventory f WITH (NOLOCK) 
    on d.toPoId = f.PoId
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

            sqlcmd = string.Format(@"
Select  d.topoid
        ,d.toseq1
        ,d.toseq2
        ,d.toRoll
        ,d.Qty
        ,balanceQty = isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) 
from dbo.BorrowBack_Detail d WITH (NOLOCK) 
left join FtyInventory f WITH (NOLOCK) 
    on d.toPoId = f.PoId
    and d.toSeq1 = f.Seq1
    and d.toSeq2 = f.seq2
    and d.toStocktype = f.StockType
    and d.toRoll = f.Roll
where (isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) - d.Qty < 0) 
    and d.Id = '{0}'", CurrentMaintain["id"]);
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

            sqlupd3 = string.Format(@"
update BorrowBack 
    set status='New'
        , editname = '{0}' 
        , editdate = GETDATE()
where id = '{1}'", Env.User.UserID, CurrentMaintain["id"]);

            #endregion 更新表頭狀態資料

            #region -- 更新MdivisionPoDetail 借出數 --
            var bs1 = (from b in ((DataTable)detailgridbs.DataSource).AsEnumerable()
                       group b by new
                       {
                           poid = b.Field<string>("frompoid").Trim(),
                           seq1 = b.Field<string>("fromseq1").Trim(),
                           seq2 = b.Field<string>("fromseq2").Trim(),
                           stocktype = b.Field<string>("fromstocktype").Trim()
                       } into m
                       select new
                       {
                           poid = m.First().Field<string>("frompoid"),
                           seq1 = m.First().Field<string>("fromseq1"),
                           seq2 = m.First().Field<string>("fromseq2"),
                           stocktype = m.First().Field<string>("fromstocktype"),
                           qty = - (m.Sum(w => w.Field<decimal>("qty")))
                       }).ToList();
            var bs1I = (from b in ((DataTable)detailgridbs.DataSource).AsEnumerable().Where(w => w.Field<string>("fromstocktype") == "I")
                       group b by new
                       {
                           poid = b.Field<string>("frompoid").Trim(),
                           seq1 = b.Field<string>("fromseq1").Trim(),
                           seq2 = b.Field<string>("fromseq2").Trim(),
                           stocktype = b.Field<string>("fromstocktype").Trim()
                       } into m
                        select new Prgs_POSuppDetailData
                       {
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
                       poid = b.Field<string>("topoid").Trim(),
                       seq1 = b.Field<string>("toseq1").Trim(),
                       seq2 = b.Field<string>("toseq2").Trim(),
                       stocktype = b.Field<string>("tostocktype").Trim()
                   } into m
                       select new Prgs_POSuppDetailData
                   {
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
            sqlupd2.Append(string.Format(@"
;declare @reccount as int;

with acc as(
    select  bd1.ToPoid
            ,bd1.ToSeq1
            ,bd1.ToSeq2
            ,qty = sum(bd1.qty) 
    from dbo.BorrowBack b1 WITH (NOLOCK) 
    inner join dbo.BorrowBack_Detail bd1 WITH (NOLOCK) on b1.id = bd1.id 
    where b1.BorrowId='{1}' and b1.Status = 'Confirmed'
    group by bd1.ToPoid, bd1.ToSeq1, bd1.ToSeq2
    )
, borrow as(
    select  bd.FromPoId
            ,bd.FromSeq1
            ,bd.FromSeq2
            ,borrowedqty = sum(bd.Qty) 
    from dbo.BorrowBack_Detail bd WITH (NOLOCK) 
    left join PO_Supp_Detail p WITH (NOLOCK) on p.id = bd.FromPoId and p.SEQ1 = bd.FromSeq1 and p.SEQ2 = bd.FromSeq2
    where bd.id='{1}'
    group by bd.FromPoId, bd.FromSeq1, bd.FromSeq2
    )
select @reccount = count(*)
from borrow 
left join acc on borrow.FromPoId = acc.ToPoid and borrow.FromSeq1 = acc.ToSeq1 and borrow.FromSeq2 = acc.ToSeq2
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
                    _transactionscope.Dispose();
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
            
        }

        //寫明細撈出的sql command
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["ID"].ToString();
            this.DetailSelectCommand = string.Format(@"
select  a.id
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
from dbo.BorrowBack_detail a WITH (NOLOCK) 
left join PO_Supp_Detail p1 WITH (NOLOCK) on p1.ID = a.FromPoId and p1.seq1 = a.FromSeq1 and p1.SEQ2 = a.FromSeq2
left join FtyInventory FI on a.fromPoid = fi.poid and a.fromSeq1 = fi.seq1 and a.fromSeq2 = fi.seq2 
    and a.fromRoll = fi.roll and a.fromStocktype = fi.stocktype
Where a.id = '{0}'", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        //delete all
        private void btnClearQtyIsEmpty_Click(object sender, EventArgs e)
        {
            detailgrid.ValidateControl();
            //detailgridbs.EndEdit();
            ((DataTable)detailgridbs.DataSource).Select("qty=0.00 or qty is null").ToList().ForEach(r => r.Delete());

        }

        //Import
        private void btnImport_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(CurrentMaintain["borrowid"]))
            {
                MyUtility.Msg.WarningBox("Borrow Id can't be empty!!");
                txtBorrowID.Focus();
                return;
            }
            var frm = new Sci.Production.Warehouse.P32_Import(CurrentMaintain, (DataTable)detailgridbs.DataSource);
            frm.ShowDialog(this);
            this.RenewData();
        }

        private void btnAccumulatedQty_Click(object sender, EventArgs e)
        {
            var frm = new Sci.Production.Warehouse.P32_AccumulatedQty(CurrentMaintain);
            frm.P32 = this;
            frm.ShowDialog(this);
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(detailgridbs.DataSource)) return;
            int index = detailgridbs.Find("frompoid", txtLocateForSP.Text.TrimEnd());
            if (index == -1)
            { MyUtility.Msg.WarningBox("Data was not found!!"); }
            else
            { detailgridbs.Position = index; }
        }

        //borrow id
        private void txtBorrowID_Validating(object sender, CancelEventArgs e)
        {
            if (MyUtility.Check.Empty(txtBorrowID.Text))
            {
                CurrentMaintain["BorrowId"] = "";
                return;
            }
            DataRow dr;
            //BorrowBack MDivisionID 是P31 寫入 => Sci.Env.User.Keyword
            if (!MyUtility.Check.Seek(string.Format(@"select [status],[backdate] from dbo.borrowback where id='{0}' and type='A' and mdivisionid='{1}'"
                , txtBorrowID.Text, Sci.Env.User.Keyword), out dr, null))
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
                    MyUtility.Msg.WarningBox(string.Format("This borrow# ({0}) already returned.", txtBorrowID.Text));
                    return;
                }

            }
            CurrentMaintain["BorrowId"] = txtBorrowID.Text;
        }

        /// <summary>
        /// 確認 SP# & Seq 是否已經有重複的 Roll
        /// </summary>
        /// <returns>bool</returns>
        private bool checkRoll()
        {
            //判斷是否已經收過此種布料SP#,SEQ,Roll不能重複收
            List<string> listMsg = new List<string>();
            List<string> listDyelot = new List<string>();
            foreach (DataRow row in DetailDatas)
            {
                DataRow dr;
                //判斷 物料 是否為 布，布料才需要 Roll & Dyelot
                if (row["fabrictype"].ToString().ToUpper() == "F")
                {
                    #region 先判斷 FtyInventory 是否有相同的 Roll & Dyelot
                    string checkFtySql = string.Format(@"
select  total = count(*)  
		, Dyelot
From dbo.FtyInventory Fty
where POID = '{0}' and Seq1 = '{1}' and Seq2 = '{2}' and Roll = '{3}' and Dyelot = '{4}'
group by Dyelot
", row["toPoid"], row["toSeq1"], row["toSeq2"], row["toRoll"], row["toDyelot"], CurrentMaintain["id"]);
                    if (MyUtility.Check.Seek(checkFtySql, null))
                    {
                        listDyelot.Add(row["toDyelot"].ToString());
                    }
                    else
                    {
                        #region 判斷 在收料記錄 & FtyInventory 是否存在【同 Roll 不同 Dyelot】
                        string checkSql = string.Format(@"
select 
	total = sum(total)
	, Dyelot = Dyelot
from(
    select  total = COUNT(*) 
            , Dyelot
    from dbo.Receiving_Detail RD WITH (NOLOCK) 
    inner join dbo.Receiving R WITH (NOLOCK) on RD.Id = R.Id  
    where RD.PoId = '{0}' and RD.Seq1 = '{1}' and RD.Seq2 = '{2}' and RD.Roll = '{3}' and Dyelot != '{4}' and R.Status = 'Confirmed'
	group by Dyelot

	Union All
	
	select  total = COUNT(*)  
			, Dyelot = ToDyelot
	from dbo.SubTransfer_Detail SD WITH (NOLOCK) 
	inner join dbo.SubTransfer S WITH (NOLOCK) on SD.ID = S.Id 
	where ToPOID = '{0}' and ToSeq1 = '{1}' and ToSeq2 = '{2}' and ToRoll = '{3}' and ToDyelot != '{4}' and S.Status = 'Confirmed'
	group by ToDyelot
	
	Union All
	
	select  total = COUNT('POID')  
			, Dyelot = ToDyelot
	from dbo.BorrowBack_Detail BD WITH (NOLOCK) 
	inner join dbo.BorrowBack B WITH (NOLOCK) on BD.ID = B.Id  
	where ToPOID = '{0}' and ToSeq1 = '{1}' and ToSeq2 = '{2}' and ToRoll = '{3}' and ToDyelot != '{4}' and B.ID != '{5}' and B.Status = 'Confirmed'
	group by ToDyelot
	
	Union All
	
	select  total = count(*)   
			, Dyelot
	From dbo.TransferIn TI
	inner join dbo.TransferIn_Detail TID on TI.ID = TID.ID
	where POID = '{0}' and Seq1 = '{1}' and Seq2 = '{2}' and Roll = '{3}' and Dyelot != '{4}' and Status = 'Confirmed'
	group by Dyelot
	
	Union All
	
	select  total = count(*)  
			, Dyelot
	From dbo.FtyInventory Fty
	where POID = '{0}' and Seq1 = '{1}' and Seq2 = '{2}' and Roll = '{3}' and Dyelot != '{4}'
	group by Dyelot
) x
group by Dyelot", row["toPoid"], row["toSeq1"], row["toSeq2"], row["toRoll"], row["toDyelot"], CurrentMaintain["id"]);
                        if (MyUtility.Check.Seek(checkSql, out dr, null))
                        {
                            if (Convert.ToInt32(dr[0]) > 0)
                            {
                                listMsg.Add(string.Format(@"
The Deylot of
<SP#>:{0}, <Seq>:{1}, <Roll>:{2}
already exists, system will update the Qty for original Deylot <{3}>
", row["toPoid"], row["toSeq1"].ToString() + " " + row["toSeq2"].ToString(), row["toRoll"], dr["Dyelot"].ToString().Trim()));
                                listDyelot.Add(dr["Dyelot"].ToString().Trim());
                            }
                        }
                        else
                        {
                            listDyelot.Add(row["toDyelot"].ToString().Trim());
                        }
                        #endregion
                    }
                    #endregion
                }
            }

            #region 若上方判斷有 同 Roll 不同 Dyelot
            if (listMsg.Count > 0)
            {
                DialogResult Dr = MyUtility.Msg.QuestionBox(listMsg.JoinToString("").TrimStart(), buttons: MessageBoxButtons.OKCancel);
                switch (Dr.ToString().ToUpper())
                {
                    case "OK":
                        int index = 0;
                        foreach (DataRow row in DetailDatas)
                        {
                            if (row["FabricType"].EqualString("F"))
                            {
                                DualResult result;
                                /**
                                * 如果在編輯模式下，直接改 Grid
                                * 非編輯模式 (Confirm) 必須用 Update 才能顯示正確的資料
                                **/
                                row["toDyelot"] = listDyelot[index++];

                                if (this.EditMode != true)
                                {
                                    result = DBProxy.Current.Execute(null, string.Format(@"
Update BB
set BB.Roll = '{0}'
    , BB.Dyelot = '{1}'
From BorrowBack BB
where BB.Ukey = '{2}'", row["toRoll"], row["toDyelot"], row["Ukey"]));

                                    if (!result)
                                    {
                                        MyUtility.Msg.WarningBox(result.Description);
                                    }
                                }
                            }
                        }
                        break;
                    case "CANCEL":
                        return false;
                }
            }
            #endregion
            return true;
        }
    }
}