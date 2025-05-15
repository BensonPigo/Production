﻿using System.Data;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// P01_ProductionOutput_CuttingDetail
    /// </summary>
    public partial class P01_ProductionOutput_CuttingDetail : Win.Subs.Base
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
                 .Text("CDate", header: "Date", width: Widths.AnsiChars(10))
                 .Text("CutRef", header: "Ref#", width: Widths.AnsiChars(8))
                 .Text("PatternPanel", header: "Fabric Comb", width: Widths.AnsiChars(2))
                 .Text("FabricPanelCode", header: "Lectra Code", width: Widths.AnsiChars(2))
                 .Text("Cutno", header: "Cut#", width: Widths.AnsiChars(3))
                 .Text("CutQty", header: "Q'ty", width: Widths.AnsiChars(6), alignment: System.Windows.Forms.DataGridViewContentAlignment.MiddleRight);
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
left join WorkOrderForOutput_Distribute wd WITH (NOLOCK) on wd.OrderID = o.ID
left join CuttingOutput_Detail cd WITH (NOLOCK) on cd.WorkOrderForOutputUkey = wd.WorkOrderForOutputUkey
left join CuttingOutput c WITH (NOLOCK) on c.ID = cd.ID
left join WorkOrderForOutput_PatternPanel wp WITH (NOLOCK) on wd.WorkOrderForOutputUkey = wp.WorkOrderForOutputUkey
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
	select wd.OrderID,[cDate] = max(co.cDate),wd.SizeCode,wd.Article,wp.PatternPanel,wd.WorkOrderForOutputUkey,
		cutqty= iif(isnull(sum(cod.Layer*ws.Qty), 0)>wd.Qty,wd.Qty,isnull(sum(cod.Layer*ws.Qty), 0)),
		co.MDivisionid,
		TotalCutQty=isnull(sum(cod.Layer*ws.Qty),0),cod.CutRef,wo.FabricPanelCode,wo.Cutno
	into #CutQtytmp1
	from WorkOrderForOutput_Distribute wd WITH (NOLOCK)
	inner join WorkOrderForOutput_PatternPanel wp WITH (NOLOCK) on wp.WorkOrderForOutputUkey = wd.WorkOrderForOutputUkey
	inner join WorkOrderForOutput_SizeRatio ws WITH (NOLOCK) on ws.WorkOrderForOutputUkey = wd.WorkOrderForOutputUkey and ws.SizeCode = wd.SizeCode
	inner join WorkOrderForOutput wo WITH (NOLOCK) on wo.Ukey = wd.WorkOrderForOutputUkey
	left join CuttingOutput_Detail cod on cod.WorkOrderForOutputUkey = wd.WorkOrderForOutputUkey
	left join CuttingOutput co WITH (NOLOCK) on co.id = cod.id and co.Status <> 'New'
	inner join orders o WITH (NOLOCK) on o.id = wd.OrderID
	where o.poid=(select poid from orders o with(nolock) where id = '{this.id}')
	group by wd.OrderID,wd.SizeCode,wd.Article,wp.PatternPanel,co.MDivisionid,wd.Qty,wd.WorkOrderForOutputUkey,cod.CutRef,wo.FabricPanelCode,wo.Cutno
	------------------
	select * ,AccuCutQty=sum(cutqty) over(partition by WorkOrderForOutputUkey,patternpanel,sizecode order by WorkOrderForOutputUkey,orderid)
		,Rowid=ROW_NUMBER() over(partition by WorkOrderForOutputUkey,patternpanel,sizecode order by WorkOrderForOutputUkey,orderid)
	into #CutQtytmp2
	from #CutQtytmp1
	------------------
	select *,Lagaccu= LAG(AccuCutQty,1,AccuCutQty) over(partition by WorkOrderForOutputUkey,patternpanel,sizecode order by WorkOrderForOutputUkey,orderid)
	into #Lagtmp
	from #CutQtytmp2 
	------------------
	select *,cQty=iif(TotalCutQty < AccuCutQty and TotalCutQty > Lagaccu,TotalCutQty-Lagaccu,cutqty)
	into #tmp2_1
	from #Lagtmp 
	------------------mp2_A
	select  [OrderID] = isnull(OrderID, '----'),
            [cDate] = iif(isCuttingOutputComplete.val = 0, '',isnull(Format(cDate, 'yyyy/MM/dd') , '----')),
            [CutRef] = isnull(CutRef, '----'),
            SizeCode,
            Article,
            PatternPanel,
            MDivisionid,
	        [cutqty] = case when isCuttingOutputComplete.val = 0 then '0'
                            when Cutno is null then '----'
                            else  cast(sum(cQty) as varchar) end,
            FabricPanelCode,[Cutno] = isnull(cast(Cutno as varchar), '----')
    from #tmp2_1
    outer apply (select val = iif(TotalCutQty>= AccuCutQty or (TotalCutQty < AccuCutQty and TotalCutQty > Lagaccu), 1, 0)) isCuttingOutputComplete
	where orderid = '{this.id}' and SizeCode = '{this.sizeCode}' and Article ='{this.article}'
	group by OrderID,CutRef,SizeCode,Article,PatternPanel,MDivisionid,cDate,FabricPanelCode,Cutno,isCuttingOutputComplete.val
    order by PatternPanel,FabricPanelCode,cDate,CutRef,Cutno

	drop table #CutQtytmp1,#CutQtytmp2,#Lagtmp,#tmp2_1
";
            }

            DataTable gridData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out gridData);
            this.listControlBindingSource1.DataSource = gridData;
        }
    }
}
