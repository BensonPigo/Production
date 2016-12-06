using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Sci;
using Ict.Win;
using Ict;
using Sci.Data;
using System.Linq;
using System.Transactions;
using Sci.Production.PublicPrg;
using System.Data.SqlClient;
using Sci.Win;
using System.Reflection;

namespace Sci.Production.Warehouse
{
    public partial class P10 : Sci.Win.Tems.Input8
    {

        public P10(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.DefaultFilter = string.Format("Type='A' and MDivisionID = '{0}'", Sci.Env.User.Keyword);

            WorkAlias = "Issue";                        // PK: ID
            GridAlias = "Issue_summary";           // PK: ID+UKey
            SubGridAlias = "Issue_detail";          // PK: ID+Issue_SummaryUkey+FtyInventoryUkey

            KeyField1 = "ID"; //Issue PK
            KeyField2 = "ID"; // Summary FK

            //SubKeyField1 = "Ukey";    // 將第2層的PK欄位傳給第3層的FK。
            SubKeyField1 = "ID";    // 將第2層的PK欄位傳給第3層的FK。
            SubKeyField2 = "Ukey";  // 將第2層的PK欄位傳給第3層的FK。

            SubDetailKeyField1 = "id,Ukey";    // second PK
            SubDetailKeyField2 = "id,Issue_SummaryUkey"; // third FK
            //SubDetailKeyField1 = "Ukey";    // second PK
            //SubDetailKeyField2 = "Issue_SummaryUkey"; // third FK

            DoSubForm = new P10_Detail();
            this.ChangeDetailColor();
        }

        public P10(ToolStripMenuItem menuitem, string transID)
            : base(menuitem)
        {

            this.DefaultFilter = string.Format("Type='A' and id='{0}'", transID);
            this.IsSupportNew = false;
            this.IsSupportEdit = false;
            this.IsSupportDelete = false;
            this.IsSupportConfirm = false;
            this.IsSupportUnconfirm = false;
            WorkAlias = "Issue";                        // PK: ID
            GridAlias = "Issue_summary";           // PK: ID+UKey
            SubGridAlias = "Issue_detail";          // PK: ID+Issue_SummaryUkey+FtyInventoryUkey

            KeyField1 = "ID"; //master PK
            KeyField2 = "ID"; // second FK

            SubDetailKeyField1 = "ID,Ukey";    // second PK
            SubDetailKeyField2 = "ID,Issue_SummaryUkey"; // third FK

            //SubDetailKeyField1 = "Ukey";    // second PK
            //SubDetailKeyField2 = "Issue_SummaryUkey"; // third FK

            SubKeyField1 = "ID";    // 將第2層的PK欄位傳給第3層的FK。
            SubKeyField2 = "Ukey";  // 將第2層的PK欄位傳給第3層的FK。

            DoSubForm = new P10_Detail();
        }

        private void ChangeDetailColor()
        {
            detailgrid.RowPostPaint += (s, e) =>
            {
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                if (detailgrid.Rows.Count <= e.RowIndex || e.RowIndex < 0) return;

                int i = e.RowIndex;
                decimal qty, accu_issue, netqty;
                decimal.TryParse(dr["qty"].ToString(), out qty);
                decimal.TryParse(dr["accu_issue"].ToString(), out accu_issue);
                decimal.TryParse(dr["netqty"].ToString(), out netqty);
                if (qty + accu_issue > netqty)
                {
                    detailgrid.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(255, 128, 192);
                }

            };
        }

        // Detail Grid 設定
        protected override void OnDetailGridSetup()
        {
            #region qty 開窗
            Ict.Win.DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();
            ns.CellMouseDoubleClick += (s, e) =>
            {
                DoSubForm.IsSupportUpdate = false;
                OpenSubDetailPage();
            };
            #endregion

            // 使用虛擬欄位顯示 "bal_qty"及"var_qty"
            this.detailgrid.VirtualMode = true;
            this.detailgrid.CellValueNeeded += (s, e) =>
            {
                string STRrequestqty = this.detailgrid.Rows[e.RowIndex].Cells["requestqty"].Value.ToString();
                string STRaccu_issue = this.detailgrid.Rows[e.RowIndex].Cells["accu_issue"].Value.ToString();
                string STRqty = this.detailgrid.Rows[e.RowIndex].Cells["qty"].Value.ToString();
                decimal DECrequestqty;
                 decimal DECaccu_issue;
                 decimal DECqty;
                if (!decimal.TryParse(STRrequestqty, out DECrequestqty))
                { DECrequestqty= 0;}
                 if (!decimal.TryParse(STRaccu_issue, out DECaccu_issue))
                { DECaccu_issue= 0;}
                 if (!decimal.TryParse(STRqty, out DECqty))
                 { DECqty = 0; }
                if (e.ColumnIndex == this.detailgrid.Columns["bal_qty"].Index && !MyUtility.Check.Empty(this.detailgrid.Rows[e.RowIndex].Cells["requestqty"].Value))
                   // e.Value = Decimal.Parse(this.detailgrid.Rows[e.RowIndex].Cells["requestqty"].Value.ToString()) - Decimal.Parse(this.detailgrid.Rows[e.RowIndex].Cells["accu_issue"].Value.ToString());
                    e.Value = DECrequestqty-DECaccu_issue;
                if (e.ColumnIndex == this.detailgrid.Columns["var_qty"].Index && !MyUtility.Check.Empty(this.detailgrid.Rows[e.RowIndex].Cells["requestqty"].Value))
                    e.Value = DECrequestqty - DECaccu_issue - DECqty;
                   // e.Value = (Decimal.Parse(this.detailgrid.Rows[e.RowIndex].Cells["requestqty"].Value.ToString()) - Decimal.Parse(this.detailgrid.Rows[e.RowIndex].Cells["accu_issue"].Value.ToString())) - Decimal.Parse(this.detailgrid.Rows[e.RowIndex].Cells["qty"].Value.ToString());
            };


            #region 欄位設定
            Helper.Controls.Grid.Generator(this.detailgrid)
            .CellPOIDWithSeqRollDyelot("poid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)  //0
            .Text("SCIRefno", header: "SCIRefno", width: Widths.AnsiChars(20), iseditingreadonly: true)  //1
            .Text("Colorid", header: "Color", width: Widths.AnsiChars(10), iseditingreadonly: true)  //2
           // .Text("SizeSpec", header: "SizeSpec", width: Widths.AnsiChars(10), iseditingreadonly: true)  //3
            .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true) //4
            .Text("unit", header: "unit", width: Widths.AnsiChars(4))  //add
            .Numeric("requestqty", name: "requestqty", header: "Request", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true)    //5
            .Numeric("accu_issue", name: "accu_issue", header: "Accu. Issued", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true)    //6
            .Numeric("", name: "bal_qty", header: "Bal. Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true)    //7
            .Numeric("qty", name: "qty", header: "Issue Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, settings: ns, iseditingreadonly: true)    //8
            .Numeric("", name: "var_qty", header: "Var Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true)    //9
            .Numeric("arqty", name: "arqty", header: "Accu Req. Qty by Material", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true)    //9
            .Numeric("aiqqty", name: "aiqqty", header: "Accu Issue Qty by Material", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true)    //9
            .Numeric("avqty", name: "avqty", header: "Accu Var by Material", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true)    //9
            .Numeric("netqty", name: "netqty", header: "Net Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, settings: ns, iseditingreadonly: true)    //10
            ;     //
            #endregion 欄位設定
        }

        //寫明細撈出的sql command
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["ID"].ToString();
            string cutplanID = (e.Master == null) ? "" : e.Master["cutplanID"].ToString();
            #region 20161117 舊寫法
//            this.DetailSelectCommand = string.Format(@"select
//a.Id
//,a.Poid
//,a.SCIRefno
//,a.Qty
//,a.Colorid
//,a.SizeSpec
//,(select DescDetail from fabric where scirefno= a.scirefno) [description]
//,isnull((select sum(cons) from dbo.Cutplan_Detail_Cons c inner join dbo.PO_Supp_Detail p on p.ID=c.Poid and p.SEQ1 = c.Seq1 and p.SEQ2 = c.Seq2
//where  c.id='{1}' and p.scirefno = a.scirefno and c.poid = a.poid 
//--and p.colorid = a.colorid and p.SizeSpec = a.SizeSpec
//),0.00) as requestqty
//,isnull((select sum(qty) from issue a1 inner join issue_summary b1 on b1.id = a1.id where a1.cutplanid = '{1}' 
//and b1.poid = a.poid and b1.scirefno = a.scirefno 
//--and b1.colorid = a.colorid  --20161006 by willy,20161116 LEO調整
//and a1.id != '{0}' and a1.status='Confirmed'),0.00) as accu_issue
//,a.qty
//,a.Ukey,t.netqty
//--,(select sum(NETQty) from dbo.PO_Supp_Detail 
//--where 1=1
//--AND SCIRefno = a.SCIRefno 
//--and Colorid = a.Colorid and SizeSpec = a.SizeSpec  --20161006 by willy,這2欄位是空的,先mark做測試
//--and id = a.Poid and seq1 = (select min(seq1) from dbo.PO_Supp_Detail where id = a.Poid
//--and SCIRefno = a.SCIRefno --and Colorid = a.Colorid and SizeSpec = a.SizeSpec 
//--)) netqty
//from dbo.Issue_Summary a
//left join dbo.PO_Supp_Detail t on t.id=a.Poid and t.seq1=a.seq1 and t.seq2=a.Seq2
//Where a.id = '{0}'", masterID, cutplanID);
            #endregion
            this.DetailSelectCommand = string.Format(@"
                    ;with main as
                    (
	                    select
	                    a.Id
	                    ,a.Poid
	                    ,a.SCIRefno
	                    ,a.Qty
	                    ,a.Colorid
	                    ,a.SizeSpec
	                    ,[arqty] = isnull((select sum(cons) from dbo.cutplan_detail_cons c where c.id='{1}' and c.poid = a.poid),0.00)
	                    ,[description] = (select DescDetail from fabric where scirefno = a.scirefno)
	                    ,[requestqty] = isnull((select sum(cons) 
		                    from dbo.Cutplan_Detail_Cons c 
		                    inner join dbo.PO_Supp_Detail p on p.ID=c.Poid and p.SEQ1 = c.Seq1 and p.SEQ2 = c.Seq2
		                    where  c.id='{1}' and p.seq1 = a.seq1 and p.seq2 = a.seq2 and c.poid = a.poid), 0.00)
	                    ,[accu_issue] = isnull((select sum(qty) 
		                    from issue a1 
		                    inner join issue_summary b1 on b1.id = a1.id 
		                    where a1.cutplanid = '{1}' and b1.poid = a.poid and b1.seq1 = a.seq1 and b1.seq2 = a.seq2
		                    and a1.id != '{0}' and a1.status='Confirmed'), 0.00)
	                    ,a.Ukey
                        ,a.seq1
                        ,a.seq2
	                    from dbo.Issue_Summary a
	                    Where a.id = '{0}'
                    ),
                    NetQty as
                    (
	                    select a.NETQty,a.ID,a.SEQ1,a.SEQ2,a.SCIRefno,a.ColorID
	                    ,[Unit] = [dbo].[getStockUnit](a.SCIRefno,c.SuppID)
                        ,[aiqqty] = ISNULL(SUM(a.OutputQty)	,0.00)
	                    from PO_Supp_Detail a
	                    left join Issue_Summary b on  a.ID=b.Poid and a.seq1 = b.seq1 and a.seq2 = b.seq2
	                    left join PO_Supp c on c.id = a.ID and c.SEQ1 = a.SEQ1
	                    where 1=1
	                    and a.seq1 =(select min(seq1) from dbo.PO_Supp_Detail where id=b.Poid and seq1 = b.seq1 and seq2 = b.seq2)
	                    and b.Id='{0}'
                        group by a.NETQty,a.ID,a.SEQ1,a.SEQ2,a.SCIRefno,a.ColorID,[dbo].[getStockUnit](a.SCIRefno,c.SuppID)
                    )
                    select a.*
                    ,b.ColorID
                    ,[Unit] = isnull(b.Unit,0)
                    ,[NETQty] = isnull(b.NETQty,0)
                    ,[aiqqty] = isnull(b.aiqqty,0)
                    ,[avqty] =isnull([accu_issue],0)-isnull([aiqqty],0)
                    from main a                    
                    left join NetQty b on a.Poid = b.ID and a.seq1 = b.seq1 and a.seq2 = b.seq2"

                , masterID, cutplanID);

            return base.OnDetailSelectCommandPrepare(e);
        }

        // 新增時預設資料
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["MDivisionID"] = Sci.Env.User.Keyword;
            CurrentMaintain["Status"] = "New";
            CurrentMaintain["Type"] = "A";
            CurrentMaintain["issuedate"] = DateTime.Now;
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
            //!EMPTY(APVName) OR !EMPTY(Closed)，只能編輯remark欄。
            DataRow dr = grid.GetDataRow<DataRow>(grid.GetSelectedRowIndex());
            if (dr["status"].ToString().ToUpper() == "CONFIRMED")
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't modify.", "Warning");
                return false;
            }
            return base.ClickEditBefore();
        }

        // save前檢查 & 取id
        protected override bool ClickSaveBefore()
        {
            StringBuilder warningmsg = new StringBuilder();

            #region 必輸檢查

            if (MyUtility.Check.Empty(CurrentMaintain["cutplanId"]))
            {
                MyUtility.Msg.WarningBox("< Request# >  can't be empty!", "Warning");
                txtRequest.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["IssueDate"]))
            {
                MyUtility.Msg.WarningBox("< Issue Date >  can't be empty!", "Warning");
                dateBox1.Focus();
                return false;
            }

            #endregion 必輸檢查

            if (DetailDatas.Count == 0)
            {
                MyUtility.Msg.WarningBox("Detail can't be empty", "Warning");
                return false;
            }

            //取單號
            if (this.IsDetailInserting)
            {
                string tmpId = Sci.MyUtility.GetValue.GetID(Sci.Env.User.Keyword + "IC", "Issue", (DateTime)CurrentMaintain["IssueDate"]);
                if (MyUtility.Check.Empty(tmpId))
                {
                    MyUtility.Msg.WarningBox("Get document ID fail!!");
                    return false;
                }
                CurrentMaintain["id"] = tmpId;
            }

            return base.ClickSaveBefore();
        }

        protected override DualResult ConvertSubDetailDatasFromDoSubForm(SubDetailConvertFromEventArgs e)
        {
            DataTable dt;
            foreach (DataRow dr in DetailDatas)
            {
                if (GetSubDetailDatas(dr, out dt))
                {
                    sum_subDetail(dr, dt);
                }
            }
            return base.ConvertSubDetailDatasFromDoSubForm(e);
        }

        static void sum_subDetail(DataRow target, DataTable source)
        {
            target["qty"] = (source.Rows.Count == 0) ? 0m : source.AsEnumerable().Where(r => r.RowState != DataRowState.Deleted)
                .Sum(r => r.Field<decimal>("qty"));
        }

        private void btnAutoPick_Click(object sender, EventArgs e)
        {
            DataTable subDT;
            foreach (DataRow dr in DetailDatas)
            {
                var issued = PublicPrg.Prgs.autopick(dr);
                if (issued == null)
                {
                    break;
                }
                if (GetSubDetailDatas(dr, out subDT))
                {
                    subDT.Clear();
                    foreach (DataRow dr2 in issued)
                    {
                        dr2.AcceptChanges();
                        dr2.SetAdded();
                        subDT.ImportRow(dr2);
                    }
                    sum_subDetail(dr, subDT);
                }

            }
        }

        private void txtRequest_Validating(object sender, CancelEventArgs e)
        {
            DataTable dt;
            string sqlcmd;
            if (!MyUtility.Check.Empty(txtRequest.Text) && txtRequest.Text != txtRequest.OldValue)
            {
                //foreach (DataRow dr in ((DataTable)detailgridbs.DataSource).Rows)
                //{
                //    dr.Delete();
                //}
                dt = (DataTable)this.detailgridbs.DataSource;
                for (int i = dt.Rows.Count-1; i >= 0; i--)
                {
                    dt.Rows[i].Delete();
                }
                CurrentMaintain["cutplanid"] = txtRequest.Text;
                if (!MyUtility.Check.Seek(string.Format("select id from dbo.cutplan where id='{0}' and mdivisionid = '{1}'", txtRequest.Text, Sci.Env.User.Keyword), null))
                {
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox("Request not existe");
                    return;
                }
                else
                {
                    sqlcmd = string.Format(@"
with 
main as(
    select 
        poid,
        t.SCIRefno,
        t.ColorID,
        t.SizeSpec,
        sum(cons)requestqty,0.00 as qty,
        --isnull((select sum(qty) from Issue_Summary a inner join Issue b on a.Id=b.Id inner join Cutplan d on b.CutplanID=d.ID where d.ID='{0}' and b.status='Confirmed'), 0.00) as accu_issue,
        isnull((
            select 
	            sum(qty) 
            from Issue_Summary a 
	            inner join Issue b on a.Id=b.Id 
	            inner join Cutplan d on b.CutplanID=d.ID 
            where 
	            d.ID='{0}' and 
	            t.SEQ1 = a.seq1 and
				t.seq2 = a.seq2 and
	            b.status='Confirmed')
            , 0.00) as accu_issue,

        '{1}' as id,
        --'' [Description], 
        (select DescDetail from fabric where scirefno= t.scirefno)as [description], 
        t.SEQ1,
        t.SEQ2
    from dbo.Cutplan_Detail_Cons c 
        inner join dbo.PO_Supp_Detail t on t.id=c.Poid and t.seq1=c.seq1 and t.seq2=c.Seq2
    where 
        c.ID='{0}'
    group by poid,t.SCIRefno,t.ColorID,t.SizeSpec,t.SEQ1,t.SEQ2
),
NetQty as(
    select DISTINCT  
        a.NETQty,
        a.ID,
        a.SEQ1,
        a.SEQ2,
        a.SCIRefno,
        a.ColorID 
    from PO_Supp_Detail a,Issue_Summary b
    where 
        a.seq1 = b.seq1 
        and a.seq2 = b.seq2 
        and a.ID=b.Poid 
        and not a.SEQ1 >= '7'
        and a.SEQ1 = (select min(seq1) from dbo.PO_Supp_Detail where id=a.id and seq1 = a.SEQ1 and seq2 = a.seq2)
)
select a.*, isnull(b.NETQty,0) as NETQty from main a 
left join NetQty b on a.Poid = b.ID and a.seq1 = b.seq1 and a.seq2 = b.seq2
", txtRequest.Text, CurrentMaintain["id"]);
                    DBProxy.Current.Select(null, sqlcmd, out dt);
                    if (MyUtility.Check.Empty(dt) || MyUtility.Check.Empty(dt.Rows.Count))
                    {
                        MyUtility.Msg.WarningBox("Cutplan Cons Data not found!!");
                        return;
                    }
                    List<DataRow> rows = dt.AsEnumerable().ToList();
                    DataTable gridTable = ((DataTable)detailgridbs.DataSource);
                    var sameFields = gridTable.GetSameFields(dt);
                    foreach (DataRow item in dt.Rows)
                    {
                        var newRow = gridTable.NewRow();
                        item.CopyTo(newRow, sameFields);
                        gridTable.Rows.Add(newRow);
                    }
                }
            }
        }

        //refresh
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            DataTable dt;
            if (!(CurrentMaintain == null))
            {
                disCutCell.Text = MyUtility.GetValue.Lookup(string.Format("select CutCellID from dbo.cutplan  where id='{0}'", CurrentMaintain["cutplanid"]));
                disLine.Text = MyUtility.GetValue.Lookup(string.Format(@"select t.SewLine+','  from (select distinct o.SewLine 
from dbo.Issue_Summary a inner join dbo.orders o on a.Poid = o.POID where a.id='{0}' and o.sewline !='') t for xml path('')", CurrentMaintain["id"]));

                DBProxy.Current.Select(null, string.Format(@";with cte as
(Select WorkOrder.FabricCombo,Cutplan_Detail.CutNo from Cutplan_Detail inner join dbo.workorder on WorkOrder.Ukey = Cutplan_Detail.WorkorderUkey 
where Cutplan_Detail.ID='{0}' )
select distinct FabricCombo ,(select convert(varchar,CutNo)+',' 
from (select CutNo from cte where cte.FabricCombo = a.FabricCombo )t order by CutNo for xml path('')) cutnos from cte a
", CurrentMaintain["cutplanid"]), out dt);
                ebCut.Text = String.Join(" / ", dt.AsEnumerable().Select(row => row["FabricCombo"].ToString() + "-" + row["cutnos"].ToString())
       );

            }

            #region Status Label

            label25.Text = CurrentMaintain["status"].ToString();

            #endregion Status Label
        }
        //以下設定ISS Detail
//        protected override DualResult OnSubDetailSelectCommandPrepare(PrepareSubDetailSelectCommandEventArgs e)
//        {
//            string masterID = (e.Detail == null) ? "" : e.Detail["ID"].ToString();
//            string ukey = (e.Detail == null || MyUtility.Check.Empty(e.Detail["ukey"])) ? "0" : e.Detail["ukey"].ToString();
//            this.SubDetailSelectCommand = string.Format(@"select *,a.seq1+a.seq2 as seq
//,(Select t.mtllocationid+',' from (select mtllocationid from ftyinventory_detail where ukey = a.ftyinventoryukey) t for xml path('')) [location]
//,ftyinventory.inqty
//,ftyinventory.outqty
//,ftyinventory.adjustqty
//,ftyinventory.inqty-ftyinventory.outqty+ftyinventory.adjustqty as balanceQty
//from dbo.Issue_detail a inner join dbo.ftyinventory on ftyinventory.ukey = a.ftyinventoryukey
//Where a.id = '{0}' and a.issue_summaryukey = {1}", masterID, ukey);
//            return base.OnSubDetailSelectCommandPrepare(e);
//        }

        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            var dr = this.CurrentMaintain;
            if (null == dr) return;

            StringBuilder sqlupd2 = new StringBuilder();
            String sqlcmd = "", sqlupd3 = "", ids = "";
            DualResult result, result2;
            DataTable datacheck;

            #region 檢查庫存項lock
            sqlcmd = string.Format(@"Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty
from dbo.Issue_Detail d inner join FtyInventory f
on d.ftyinventoryukey = f.ukey
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
                            , tmp["poid"], tmp["seq1"], tmp["seq2"], tmp["roll"]);
                    }
                    MyUtility.Msg.WarningBox("Material Locked!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }
            #endregion

            #region 檢查負數庫存

            sqlcmd = string.Format(@"Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty
from dbo.Issue_Detail d left join FtyInventory f
on d.ftyinventoryukey = f.ukey
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
                        ids += string.Format("SP#: {0} Seq#: {1}-{2} Roll#: {3}'s balance: {4} is less than issue qty: {5}" + Environment.NewLine
                            , tmp["poid"], tmp["seq1"], tmp["seq2"], tmp["roll"], tmp["balanceqty"], tmp["qty"]);
                    }
                    MyUtility.Msg.WarningBox("Balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }

            #endregion 檢查負數庫存

            #region 更新表頭狀態資料

            sqlupd3 = string.Format(@"update Issue set status='Confirmed', editname = '{0}' , editdate = GETDATE()
                                where id = '{1}'", Env.User.UserID, CurrentMaintain["id"]);

            #endregion 更新表頭狀態資料

            #region 更新庫存數量  ftyinventory
            sqlcmd = string.Format(@"select * from issue_detail where id='{0}'", CurrentMaintain["id"]);
            if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                ShowErr(sqlcmd, result2);
                return;
            }

            foreach (DataRow item in datacheck.Rows)
            {
                sqlupd2.Append(Prgs.UpdateFtyInventory(4, item["mdivisionid"].ToString(), item["poid"].ToString(), item["seq1"].ToString(), item["seq2"].ToString()
                    , (decimal)item["qty"]
                    , item["roll"].ToString(), item["dyelot"].ToString(), item["stocktype"].ToString(), true));
            }

            var bs1 = (from b in datacheck.AsEnumerable()
                       group b by new
                       {
                           mdivisionid = b.Field<string>("mdivisionid"),
                           poid = b.Field<string>("poid"),
                           seq1 = b.Field<string>("seq1"),
                           seq2 = b.Field<string>("seq2"),
                           stocktype = b.Field<string>("stocktype")
                       } into m
                       select new
                       {
                           mdivisionid = m.First().Field<string>("mdivisionid"),
                           poid = m.First().Field<string>("poid"),
                           seq1 = m.First().Field<string>("seq1"),
                           seq2 = m.First().Field<string>("seq2"),
                           stocktype = m.First().Field<string>("stocktype"),
                           qty = m.Sum(w => w.Field<decimal>("qty"))
                       }).ToList();

            foreach (var item in bs1)
            {
                sqlupd2.Append(Prgs.UpdateMPoDetail(4, item.poid, item.seq1, item.seq2, item.qty, true, item.stocktype, item.mdivisionid));
            }

            #endregion 更新庫存數量  ftyinventory

            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    if (!MyUtility.Check.Empty(sqlupd2.ToString()))
                    {
                        if (!(result2 = DBProxy.Current.Execute(null, sqlupd2.ToString())))
                        {
                            _transactionscope.Dispose();
                            ShowErr(sqlupd2.ToString(), result2);
                            return;
                        }
                    }

                    if (!(result = DBProxy.Current.Execute(null, sqlupd3)))
                    {
                        _transactionscope.Dispose();
                        ShowErr(sqlupd3, result);
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

            DialogResult dResult = MyUtility.Msg.QuestionBox("Do you want to unconfirme it?");
            if (dResult == DialogResult.No) return;
            var dr = this.CurrentMaintain; if (null == dr) return;
            StringBuilder sqlupd2 = new StringBuilder();
            string sqlcmd = "", sqlupd3 = "", ids = "";
            DualResult result, result2;

            #region 檢查庫存項lock
            sqlcmd = string.Format(@"Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty
from dbo.Issue_Detail d inner join FtyInventory f
on d.ftyinventoryukey = f.ukey
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
                            , tmp["poid"], tmp["seq1"], tmp["seq2"], tmp["roll"]);
                    }
                    MyUtility.Msg.WarningBox("Material Locked!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }
            #endregion

            #region 檢查負數庫存

            sqlcmd = string.Format(@"Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty
from dbo.Issue_Detail d left join FtyInventory f
on d.ftyinventoryukey = f.ukey
where (isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) + d.Qty < 0) and d.Id = '{0}'", CurrentMaintain["id"]);
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
                        ids += string.Format("SP#: {0} Seq#: {1}-{2} Roll#: {3}'s balance: {4} is less than issue qty: {5}" + Environment.NewLine
                            , tmp["poid"], tmp["seq1"], tmp["seq2"], tmp["roll"], tmp["balanceqty"], tmp["qty"]);
                    }
                    MyUtility.Msg.WarningBox("Balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }

            #endregion 檢查負數庫存

            #region 更新表頭狀態資料

            sqlupd3 = string.Format(@"update Issue set status='New', editname = '{0}' , editdate = GETDATE()
                                where id = '{1}'", Env.User.UserID, CurrentMaintain["id"]);

            #endregion 更新表頭狀態資料

            #region 更新庫存數量  ftyinventory
            sqlcmd = string.Format(@"select * from issue_detail where id='{0}'", CurrentMaintain["id"]);
            if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                ShowErr(sqlcmd, result2);
                return;
            }

            foreach (DataRow item in datacheck.Rows)
            {
                sqlupd2.Append(Prgs.UpdateFtyInventory(4, item["mdivisionid"].ToString(), item["poid"].ToString(), item["seq1"].ToString(), item["seq2"].ToString(), (decimal)item["qty"]
                    , item["roll"].ToString(), item["dyelot"].ToString(), item["stocktype"].ToString(), false));
            }

            var bs1 = (from b in datacheck.AsEnumerable()
                       group b by new
                       {
                           mdivisionid = b.Field<string>("mdivisionid"),
                           poid = b.Field<string>("poid"),
                           seq1 = b.Field<string>("seq1"),
                           seq2 = b.Field<string>("seq2"),
                           stocktype = b.Field<string>("stocktype")
                       } into m
                       select new
                       {
                           mdivisionid = m.First().Field<string>("mdivisionid"),
                           poid = m.First().Field<string>("poid"),
                           seq1 = m.First().Field<string>("seq1"),
                           seq2 = m.First().Field<string>("seq2"),
                           stocktype = m.First().Field<string>("stocktype"),
                           qty = m.Sum(w => w.Field<decimal>("qty"))
                       }).ToList();

            foreach (var item in bs1)
            {
                sqlupd2.Append(Prgs.UpdateMPoDetail(4, item.poid, item.seq1, item.seq2, item.qty, false, item.stocktype, item.mdivisionid));
            }

            #endregion 更新庫存數量  ftyinventory

            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    if (!MyUtility.Check.Empty(sqlupd2.ToString()))
                    {
                        if (!(result2 = DBProxy.Current.Execute(null, sqlupd2.ToString())))
                        {
                            _transactionscope.Dispose();
                            ShowErr(sqlupd2.ToString(), result2);
                            return;
                        }
                    }

                    if (!(result = DBProxy.Current.Execute(null, sqlupd3)))
                    {
                        _transactionscope.Dispose();
                        ShowErr(sqlupd3, result);
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

        private void btCutRef_Click(object sender, EventArgs e)
        {
            var frm = new Sci.Production.Warehouse.P10_CutRef(CurrentMaintain);
            frm.ShowDialog(this);
        }

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
            string cutplanID = row["cutplanID"].ToString();
            string issuedate = ((DateTime)MyUtility.Convert.GetDate(row["issuedate"])).ToShortDateString();
            string cutno = this.ebCut.Text;

            #region  抓表頭資料
            List<SqlParameter> pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));
            DataTable dt;
            DualResult result = DBProxy.Current.Select("",
            @"select  b.name 
            from dbo.Issue a 
            inner join dbo.mdivision  b 
            on b.id = a.mdivisionid
            where b.id = a.mdivisionid
            and a.id = @ID", pars, out dt);
            if (!result) { this.ShowErr(result); }
            string RptTitle = dt.Rows[0]["name"].ToString();
            ReportDefinition report = new ReportDefinition();
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("RptTitle", RptTitle));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("ID", id));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Remark", Remark));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("cutplanID", cutplanID));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("issuedate", issuedate));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("cutno", cutno));
            pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));
            DataTable aa;
            string cCellNo;
            result = DBProxy.Current.Select("",
            @"select  b.CutCellID 
            from dbo.Issue as a 
	        inner join dbo.cutplan as b
            on b.id = a.cutplanid
            where b.id = a.cutplanid
            and a.id = @ID", pars, out aa);
            if (!result) { this.ShowErr(result); }
            if (aa.Rows.Count == 0)
                cCellNo = "";
            else
                cCellNo = aa.Rows[0]["CutCellID"].ToString();
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("cCellNo", cCellNo));

            pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));
            DataTable cc;
            string cLineNo;
            result = DBProxy.Current.Select("",
            @"select o.sewline 
            from dbo.Orders o 
            where id in (select distinct poid from issue_detail where id = @ID)", pars, out cc);
            if (!result) { this.ShowErr(result); }
            if (cc.Rows.Count == 0)
                cLineNo = "";
            else
                cLineNo = cc.Rows[0]["sewline"].ToString();
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("cCellNo", cLineNo));
            #endregion

            #region  抓表身資料
            pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));
            DataTable bb;
            string sqlcmd = @"select t.poid
                                    ,t.seq1+ '-' +t.seq2 as SEQ
                                    ,[desc]=dbo.getMtlDesc(t.poid,t.seq1,t.seq2,2,0) 
                                    ,t.Roll
                                    ,t.Dyelot
                                    ,t.Qty
                                    ,p.StockUnit
                                    ,[location]=dbo.Getlocation(b.ukey)      
                                    ,[Total]=sum(t.Qty) OVER (PARTITION BY t.POID ,t.Seq1,t.Seq2 )       
                            from dbo.Issue_Detail t 
                            left join dbo.PO_Supp_Detail p   
                                on p.id= t.poid and p.SEQ1 = t.Seq1 and p.seq2 = t.Seq2
                            left join FtyInventory b
                                on b.poid = t.poid and b.seq1 =t.seq1 and b.seq2=t.seq2 and b.Roll =t.Roll and b.Dyelot =t.Dyelot and b.StockType = t.StockType
                            where t.id= @ID";
            result = DBProxy.Current.Select("", sqlcmd, pars, out bb);
            if (!result) { this.ShowErr(sqlcmd, result); }

            // 傳 list 資料            
            List<P10_PrintData> data = bb.AsEnumerable()
                .Select(row1 => new P10_PrintData()
                {
                    Poid = row1["poid"].ToString(),
                    Seq =row1["SEQ"].ToString(),
                    Desc =row1["desc"].ToString(),
                    Location =row1["Location"].ToString(),
                    Unit =row1["StockUnit"].ToString(),
                    Roll =row1["Roll"].ToString(),
                    Dyelot=row1["Dyelot"].ToString(),
                    Qty  =row1["Qty"].ToString(),
                    Total  =row1["Total"].ToString()
                }).ToList();

            report.ReportDataSource = data;
            #endregion

            // 指定是哪個 RDLC
            #region  指定是哪個 RDLC
            //DualResult result;
            Type ReportResourceNamespace = typeof(P10_PrintData);
            Assembly ReportResourceAssembly = ReportResourceNamespace.Assembly;
            string ReportResourceName = "P10_Print.rdlc";

            IReportResource reportresource;
            if (!(result = ReportResources.ByEmbeddedResource(ReportResourceAssembly, ReportResourceNamespace, ReportResourceName, out reportresource)))
            {
                //this.ShowException(result);
                return false;
            }
            
            report.ReportResource = reportresource;
            #endregion

            // 開啟 report view
            var frm = new Sci.Win.Subs.ReportView(report);
            frm.MdiParent = MdiParent;
            frm.Show();
  
            
            
            return true;



        }

    }
}
