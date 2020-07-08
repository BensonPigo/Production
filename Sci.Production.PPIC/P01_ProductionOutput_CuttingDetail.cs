using System.Data;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// P01_ProductionOutput_CuttingDetail
    /// </summary>
    public partial class P01_ProductionOutput_CuttingDetail : Sci.Win.Subs.Base
    {
        private string workType;
        private string id;
        private string type;
        private string article;
        private string sizeCode;

        /// <summary>
        /// P01_ProductionOutput_CuttingDetail
        /// </summary>
        /// <param name="workType">string WorkType</param>
        /// <param name="id">string ID</param>
        /// <param name="type">string Type</param>
        /// <param name="article">string Article</param>
        /// <param name="sizeCode">string SizeCode</param>
        public P01_ProductionOutput_CuttingDetail(string workType, string id, string type, string article, string sizeCode)
        {
            this.InitializeComponent();
            this.workType = workType;
            this.id = id;
            this.type = type;
            this.article = article;
            this.sizeCode = sizeCode;
            if (this.type == "A")
            {
                this.Text = "Cutting Daily Output - " + this.id;
            }
            else
            {
                this.Text = "Cutting Daily Output - " + this.id + "(" + this.article + "-" + this.sizeCode + ")";
            }
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            // 設定Grid1的顯示欄位
            this.gridCuttingDailyOutput.IsEditingReadOnly = true;
            this.gridCuttingDailyOutput.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridCuttingDailyOutput)
                 .Date("CDate", header: "Date", width: Widths.AnsiChars(10))
                 .Text("CutRef", header: "Ref#", width: Widths.AnsiChars(8))
                 .Text("PatternPanel", header: "Fabric Comb", width: Widths.AnsiChars(2))
                 .Text("FabricPanelCode", header: "Lectra Code", width: Widths.AnsiChars(2))
                 .Text("Cutno", header: "Cut#", width: Widths.AnsiChars(3))
                 .Numeric("CutQty", header: "Q'ty", width: Widths.AnsiChars(6));
            this.gridCuttingDailyOutput.Columns[1].Visible = this.type != "A";
            this.gridCuttingDailyOutput.Columns[2].Visible = this.type != "A";
            this.gridCuttingDailyOutput.Columns[3].Visible = this.type != "A";
            this.gridCuttingDailyOutput.Columns[4].Visible = this.type != "A";

            string sqlCmd;
            if (this.type == "A")
            {
                sqlCmd = string.Format(
                    @"with AllPattern
as (
select distinct oq.Article,oq.SizeCode,oc.ColorID,oc.PatternPanel
from Orders o WITH (NOLOCK) 
inner join Order_Qty oq WITH (NOLOCK) on o.ID = oq.ID
inner join Order_ColorCombo oc WITH (NOLOCK) on o.POID = oc.Id and oq.Article = oc.Article
inner join Order_EachCons oe WITH (NOLOCK) on oc.Id = oe.Id 
where {0}
and oc.FabricCode <> ''
and oe.CuttingPiece = '0'
and oc.Id = oe.Id 
and oc.PatternPanel = oe.FabricCombo
),
CutQty
as (
select c.cDate,isnull(wd.Article,'') as Article,isnull(wd.SizeCode,'') as SizeCode,
isnull(wp.PatternPanel,'') as PatternPanel,isnull(SUM(wd.Qty),0) as CutQty
from Orders o WITH (NOLOCK) 
left join WorkOrder_Distribute wd WITH (NOLOCK) on wd.OrderID = o.ID
left join CuttingOutput_Detail cd WITH (NOLOCK) on cd.WorkOrderUkey = wd.WorkOrderUkey
left join CuttingOutput c WITH (NOLOCK) on c.ID = cd.ID
left join WorkOrder_PatternPanel wp WITH (NOLOCK) on wd.WorkOrderUkey = wp.WorkOrderUkey
where {0}
and c.Status <> 'New'
group by c.cDate,wd.Article,wd.SizeCode,wp.PatternPanel
),
tmpCutput
as (
select ap.Article,ap.SizeCode,ap.PatternPanel,cq.cDate,isnull(cq.CutQty,0) as CutQty
from AllPattern ap 
left join CutQty cq on ap.Article = cq.Article and ap.SizeCode = cq.SizeCode and ap.PatternPanel = cq.PatternPanel
)
select cDate, sum(CutQty) as CutQty
from (select Article,SizeCode,cDate,MIN(CutQty) as CutQty
          from tmpCutput
          group by Article,SizeCode,cDate) a
where cDate != ''
group by cDate", string.Format("o.ID = '{0}'", this.id));
            }
            else
            {
                sqlCmd = $@"
	select wd.OrderID,co.cDate,wd.SizeCode,wd.Article,wp.PatternPanel,wd.WorkOrderUkey,
		cutqty= iif(sum(cod.Layer*ws.Qty)>wd.Qty,wd.Qty,sum(cod.Layer*ws.Qty)),
		co.MDivisionid,
		TotalCutQty=sum(cod.Layer*ws.qty),cod.CutRef,wo.FabricPanelCode,wo.Cutno
	into #CutQtytmp1
	from WorkOrder_Distribute wd WITH (NOLOCK)
	inner join WorkOrder_PatternPanel wp WITH (NOLOCK) on wp.WorkOrderUkey = wd.WorkOrderUkey
	inner join WorkOrder_SizeRatio ws WITH (NOLOCK) on ws.WorkOrderUkey = wd.WorkOrderUkey and ws.SizeCode = wd.SizeCode
	inner join WorkOrder wo WITH (NOLOCK) on wo.Ukey = wd.WorkOrderUkey
	inner join CuttingOutput_Detail cod on cod.WorkOrderUkey = wd.WorkOrderUkey
	inner join CuttingOutput co WITH (NOLOCK) on co.id = cod.id and co.Status <> 'New'
	inner join orders o WITH (NOLOCK) on o.id = wd.OrderID
	where o.poid=(select poid from orders o with(nolock) where id = '{this.id}')
	group by wd.OrderID,wd.SizeCode,wd.Article,wp.PatternPanel,co.MDivisionid,wd.Qty,wd.WorkOrderUkey,cod.CutRef,co.cDate,wo.FabricPanelCode,wo.Cutno
	------------------
	select * ,AccuCutQty=sum(cutqty) over(partition by WorkOrderUkey,patternpanel,sizecode order by WorkOrderUkey,orderid)
		,Rowid=ROW_NUMBER() over(partition by WorkOrderUkey,patternpanel,sizecode order by WorkOrderUkey,orderid)
	into #CutQtytmp2
	from #CutQtytmp1
	------------------
	select *,Lagaccu= LAG(AccuCutQty,1,AccuCutQty) over(partition by WorkOrderUkey,patternpanel,sizecode order by WorkOrderUkey,orderid)
	into #Lagtmp
	from #CutQtytmp2 
	------------------
	select *,cQty=iif(TotalCutQty < AccuCutQty and TotalCutQty > Lagaccu,TotalCutQty-Lagaccu,cutqty)
	into #tmp2_1
	from #Lagtmp where TotalCutQty>= AccuCutQty or (TotalCutQty < AccuCutQty and TotalCutQty > Lagaccu)
	------------------mp2_A
	select OrderID,cDate,CutRef,SizeCode,Article,PatternPanel,MDivisionid,[cutqty] = sum(cQty),FabricPanelCode,Cutno
	from #tmp2_1
	where orderid = '{this.id}' and SizeCode = '{this.sizeCode}' and Article ='{this.article}'
	group by OrderID,CutRef,SizeCode,Article,PatternPanel,MDivisionid,cDate,FabricPanelCode,Cutno
    order by cDate,CutRef,PatternPanel,FabricPanelCode,Cutno

	drop table #CutQtytmp1,#CutQtytmp2,#Lagtmp,#tmp2_1
";
            }

            DataTable gridData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out gridData);
            this.listControlBindingSource1.DataSource = gridData;
        }
    }
}
