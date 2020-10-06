using Ict;
using Ict.Win;
using Sci.Data;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Sci.Production.Subcon
{
    /// <inheritdoc/>
    public partial class P42_SubProcessStatus : Win.Tems.QueryForm
    {
        private readonly DataRow DataRow;
        private readonly int SummarType;

        /// <inheritdoc/>
        public P42_SubProcessStatus(DataRow dataRow, int summarType)
        {
            this.InitializeComponent();

            this.DataRow = dataRow;
            this.SummarType = summarType;

            if (summarType == 0)
            {
                this.Text = $"Sub Process Status（SP# : {dataRow["Orderid"]}）";
            }
            else
            {
                this.Text = $"Sub Process Status（SP# : {dataRow["Orderid"]}, Article : {dataRow["Article"]}, Size : {dataRow["SizeCode"]}）";
            }

            this.GridSetup();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.Query();
        }

        private void GridSetup()
        {
            this.Helper.Controls.Grid.Generator(this.grid1)
            .Text("SubprocessId", header: "Subprocess", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Numeric("FinishedQtyBySet", header: "Finish Qty", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("Preparation", header: "Preparation%", width: Widths.AnsiChars(8), iseditingreadonly: true)
            ;

            this.Helper.Controls.Grid.Generator(this.grid2)
            .Text("ID", header: "SP#", width: Widths.AnsiChars(14), iseditingreadonly: true)
            .Text("Article", header: "Article", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("SizeCode", header: "SizeCode", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("ColorID", header: "Color", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("FabricCombo", header: "Comb", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Numeric("Qty", header: "Prd Qty", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Numeric("OutQty", header: "Cut Qty", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Numeric("variance", header: "Variance", width: Widths.AnsiChars(10), iseditingreadonly: true)
            ;
            Color backDefaultColor = this.grid2.DefaultCellStyle.BackColor;
            this.grid2.RowsAdded += (s, e) =>
            {
                if (e.RowIndex < 0)
                {
                    return;
                }

                int index = e.RowIndex;
                for (int i = 0; i < e.RowCount; i++)
                {
                    DataGridViewRow dr = this.grid2.Rows[index];
                    dr.DefaultCellStyle.BackColor = dr.Cells[4].Value.ToString().EqualString("=") ? Color.Pink : backDefaultColor;
                    index++;
                }
            };
        }

        private void Query()
        {
            this.Query1();
            this.Query2();
            this.Query3();
        }

        private void Query1()
        {
            string where;
            string where2;
            if (this.SummarType == 0)
            {
                where = $@"where oq.id = '{this.DataRow["OrderID"]}'";
                where2 = $@"where o.ID = '{this.DataRow["OrderID"]}'";
            }
            else
            {
                where = $@"where oq.id = '{this.DataRow["OrderID"]}' and oq.Article ='{this.DataRow["Article"]}' and oq.SizeCode ='{this.DataRow["SizeCode"]}' ";
                where2 = $@"where o.ID = '{this.DataRow["OrderID"]}' and oq.Article ='{this.DataRow["Article"]}' and oq.SizeCode ='{this.DataRow["SizeCode"]}'";
            }

            string sqlcmd = $@"
select Orderid = oq.ID,oq.Article,oq.SizeCode,oq.Qty, InStartDate = Null,InEndDate = Null,OutStartDate = Null,OutEndDate = Null
into #enn
from Order_Qty oq  with(nolock)
{where}
";

            string[] subprocessIDs = new string[] { "SewingLine", "Loading" };
            string qtyBySetPerSubprocess = PublicPrg.Prgs.QtyBySetPerSubprocess(subprocessIDs, "#enn", bySP: true, isNeedCombinBundleGroup: true, isMorethenOrderQty: "1");

            sqlcmd += qtyBySetPerSubprocess + $@"
select oq.Orderid,oq.Article,oq.SizeCode,
	oq.Qty,
	[Accu. Ready Qty]=a.InQtyBySet,
	[Ready - to Load Qty]=a.InQtyBySet-a.OutQtyBySet,
	[Loading Follow-up Qty]=oq.Qty-a.InQtyBySet,
	a.OutQtyBySet,
    wbin = s.InQtyBySet
into #tmp
from #enn oq
left join #QtyBySetPerSubprocessLoading a on oq.Orderid = a.OrderID and oq.Article = a.Article and oq.SizeCode = a.SizeCode
left join #QtyBySetPerSubprocessSewingLine s on oq.Orderid = s.OrderID and oq.Article = s.Article and oq.SizeCode = s.SizeCode

; with SewQty as (
	select	oq.Article
			, oq.SizeCode
			, oq.Qty
			, ComboType = sl.Location
			, QAQty = isnull(sum(sdd.QAQty),0)
	from Orders o WITH (NOLOCK) 
    inner join factory f WITH (NOLOCK) on o.FactoryID= f.id and f.IsProduceFty=1
	inner join Style_Location sl WITH (NOLOCK) on o.StyleUkey = sl.StyleUkey
	inner join Order_Qty oq WITH (NOLOCK) on oq.ID = o.ID
	left join SewingOutput_Detail_Detail sdd WITH (NOLOCK) on sdd.OrderId = o.ID 
															  and sdd.Article = oq.Article 
															  and sdd.SizeCode = oq.SizeCode 
															  and sdd.ComboType = sl.Location
	{where2}
	group by oq.Article,oq.SizeCode,oq.Qty,sl.Location
)
select Article, SizeCode, QAQty = MIN(QAQty)
into #tmp2
from SewQty
group by Article,SizeCode
";

            if (this.SummarType == 0)
            {
                sqlcmd += $@"
select 
	Qty=sum(Qty),
	[Accu. Ready Qty]=sum([Accu. Ready Qty]),
	[Accu. Output Qty]=sum(b.QAQty),
	[Current WIP]=sum(a.wbin)-sum(b.QAQty),
	[Ready - to Load Qty]=sum([Ready - to Load Qty]),
	[Loading Follow-up Qty]=sum([Loading Follow-up Qty])
from #tmp a
left join #tmp2 b on a.Article= b.Article and a.SizeCode = b.SizeCode

drop table #tmp,#tmp2
";
            }
            else
            {
                sqlcmd += $@"
select a.*,[Accu. Output Qty]=b.QAQty,[Current WIP]=a.wbin-b.QAQty
from #tmp a
left join #tmp2 b on a.Article= b.Article and a.SizeCode = b.SizeCode

drop table #tmp,#tmp2
";
            }

            DualResult result = DBProxy.Current.Select(null, sqlcmd, out DataTable dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            if (dt.Rows.Count > 0)
            {
                this.numericBox2.Value = MyUtility.Convert.GetDecimal(dt.Rows[0]["Qty"]);
                this.numericBox1.Value = MyUtility.Convert.GetDecimal(dt.Rows[0]["Accu. Ready Qty"]);
                this.numericBox4.Value = MyUtility.Convert.GetDecimal(dt.Rows[0]["Accu. Output Qty"]);
                this.numericBox3.Value = MyUtility.Convert.GetDecimal(dt.Rows[0]["Current WIP"]);
                this.numericBox6.Value = MyUtility.Convert.GetDecimal(dt.Rows[0]["Ready - to Load Qty"]);
                this.numericBox5.Value = MyUtility.Convert.GetDecimal(dt.Rows[0]["Loading Follow-up Qty"]);
            }
        }

        private void Query2()
        {
            string sqlcmd = $@"
select bda.SubProcessID
from Bundle b with(nolock)
inner join orders o with(nolock) on b.Orderid = o.ID and  b.MDivisionID = o.MDivisionID
inner join factory f WITH (NOLOCK) on o.FactoryID= f.id and f.IsProduceFty=1
inner join Bundle_Detail bd WITH (NOLOCK) on b.id = bd.Id
INNER JOIN Bundle_Detail_Order BDO WITH (NOLOCK) on BDO.BundleNo = BD.BundleNo
inner join Bundle_Detail_art bda WITH (NOLOCK) on bda.bundleno = bd.bundleno
where BDO.OrderID ='{this.DataRow["OrderID"]}'
union
select ID from SubProcess s where s.IsRFIDProcess=1 and s.IsRFIDDefault=1
";

            DualResult result = DBProxy.Current.Select(null, sqlcmd, out DataTable dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            sqlcmd = $@"
select OrderID = '{this.DataRow["OrderID"]}', InStartDate = Null,InEndDate = Null,OutStartDate = Null,OutEndDate = Null into #enn
";
            string[] subprocessIDs = dt.AsEnumerable().Select(s => MyUtility.Convert.GetString(s["SubProcessID"])).ToArray();
            string qtyBySetPerSubprocess = PublicPrg.Prgs.QtyBySetPerSubprocess(subprocessIDs, "#enn", bySP: true, isNeedCombinBundleGroup: true, isMorethenOrderQty: "1");
            sqlcmd += qtyBySetPerSubprocess;

            List<string> sqlJ = new List<string>();
            foreach (string subprocessID in subprocessIDs)
            {
                string subprocessIDtmp = subprocessID.Replace("-", string.Empty); // 把PAD-PRT為PADPRT, #table名稱用
                sqlJ.Add($@"
    select SubprocessId='{subprocessID}',FinishedQtyBySet from #QtyBySetPerSubprocess{subprocessIDtmp} a
    where oq.id = a.OrderID and oq.Article = a.Article and oq.SizeCode = a.SizeCode
");
            }

            if (this.SummarType == 0)
            {
                sqlcmd += $@"
select a.SubprocessId,
	FinishedQtyBySet=sum(a.FinishedQtyBySet),
	Preparation =FORMAT(iif(sum(oq.Qty)=0,0,cast(sum(a.FinishedQtyBySet) as decimal)/sum(oq.Qty)),'P')
from Order_Qty oq with(nolock)
cross apply ( 
{string.Join("union all", sqlJ)}
) a
inner join SubProcess s with(nolock)on s.Id = a.SubprocessId
where oq.id = '{this.DataRow["OrderID"]}'
group by a.SubprocessId, s.ShowSeq
order by s.ShowSeq,a.SubprocessId

drop table #tmpSubProcessID
";
            }
            else
            {
                sqlcmd += $@"
select a.SubprocessId,
	a.FinishedQtyBySet,
	Preparation =FORMAT(iif(oq.Qty=0,0,cast(a.FinishedQtyBySet as decimal)/oq.Qty),'P')
from Order_Qty oq  with(nolock)
cross apply ( 
{string.Join("union all", sqlJ)}
) a
inner join SubProcess s with(nolock)on s.Id = a.SubprocessId
where oq.id = '{this.DataRow["OrderID"]}' and oq.Article ='{this.DataRow["Article"]}' and oq.SizeCode ='{this.DataRow["SizeCode"]}'
order by s.ShowSeq,s.Id
drop table #tmpSubProcessID
";
            }

            result = DBProxy.Current.Select(null, sqlcmd, out dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.listControlBindingSource1.DataSource = dt;
        }

        private void Query3()
        {
            string where;
            if (this.SummarType == 0)
            {
                where = $@"where oq.id = '{this.DataRow["OrderID"]}'";
            }
            else
            {
                where = $@"where oq.id = '{this.DataRow["OrderID"]}' and oq.Article ='{this.DataRow["Article"]}' and oq.SizeCode ='{this.DataRow["SizeCode"]}' ";
            }

            string sqlcmd = $@"
select Orderid = oq.ID,oq.Article,oq.SizeCode,oq.Qty, InStartDate = Null,InEndDate = Null,OutStartDate = Null,OutEndDate = Null
into #enn
from Order_Qty oq  with(nolock)
{where}
";
            string[] subprocessIDs = new string[] { "Sorting", };
            string qtyBySetPerSubprocess = PublicPrg.Prgs.QtyBySetPerSubprocess(subprocessIDs, "#enn", bySP: true, isNeedCombinBundleGroup: true, isMorethenOrderQty: "1");

            sqlcmd += qtyBySetPerSubprocess + $@"
select oq.ID,oq.Article,oq.SizeCode,Colorid='',FabricCombo='=',oq.Qty,OutQty=cast(null as int),variance=cast(null as int)
into #tmp
from Order_Qty oq with(nolock)
{where}

union all
select oq.ID,oq.Article,oq.SizeCode,w.Colorid,w.FabricCombo,oq.Qty,OutQty=isnull(a.OutQtyBySet,0),variance=oq.Qty-isnull(a.OutQtyBySet,0)
from Order_Qty oq with(nolock)
outer apply(
	select distinct w.Colorid,w.FabricCombo
	from WorkOrder_Distribute wd with(nolock) 
	left join WorkOrder w with(nolock) on w.Ukey = wd.WorkOrderUkey
	where wd.OrderID = oq.ID and wd.Article = oq.Article and wd.SizeCode = oq.SizeCode
)w
left join #QtyBySetPerSubprocess_PatternPanelSorting a on oq.id = a.OrderID and oq.Article = a.Article and oq.SizeCode = a.SizeCode and w.FabricCombo = a.PatternPanel
{where}


order by oq.ID,oq.Article,oq.SizeCode,Colorid

select c.*
from #tmp c
inner join Orders o  WITH (NOLOCK) on o.ID = c.ID
inner join factory f WITH (NOLOCK) on o.FactoryID= f.id and f.IsProduceFty=1
left join Order_SizeCode z WITH (NOLOCK) on z.id = o.POID and z.SizeCode = c.SizeCode
order by c.ID,c.Article,z.Seq,c.FabricCombo

drop table #tmp
";
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out DataTable dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.listControlBindingSource2.DataSource = dt;
        }
    }
}
