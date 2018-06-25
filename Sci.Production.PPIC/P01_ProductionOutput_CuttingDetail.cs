using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
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
                sqlCmd = string.Format(
@"
select c.cDate, [CutRef] = isnull(cd.CutRef, w.CutRef), wp.PatternPanel, w.FabricPanelCode, w.Cutno,wd.Article,wd.SizeCode
    , isNull( sum(iif(c.Status <> 'New', wd.Qty, 0)) , 0) AS CutQty
into #tmp2
from Orders o WITH (NOLOCK) 
left join WorkOrder_Distribute wd WITH (NOLOCK) on wd.OrderID = o.ID and wd.Article = '{1}' and wd.SizeCode = '{2}'
left join WorkOrder_PatternPanel wp WITH (NOLOCK) on wp.WorkOrderUkey = wd.WorkOrderUkey
left join WorkOrder w WITH (NOLOCK) on w.ID = wp.ID and w.Ukey = wp.WorkOrderUkey
left join CuttingOutput_Detail cd WITH (NOLOCK) on cd.WorkOrderUkey = wd.WorkOrderUkey
left join CuttingOutput c WITH (NOLOCK) on c.ID = cd.ID
where o.ID = '{0}'
group by c.cDate,cd.CutRef,w.CutRef,wp.PatternPanel,w.FabricPanelCode,w.Cutno,Status,wd.Article,wd.SizeCode
order by wp.PatternPanel 

Select distinct d.*,e.Colorid,e.PatternPanel
into #tmp1
from 
(
	Select POID=(select poid from orders WITH (NOLOCK) where id = c.id),c.ID,c.Article,c.SizeCode,c.Qty 
	from order_Qty c WITH (NOLOCK)
	where c.id = '{0}' and c.Article = '{1}' and SizeCode = '{2}'
) d
inner join order_Eachcons cons on d.poid = cons.id
left join Order_ColorCombo e on d.POID=e.id and d.Article = e.Article and cons.id =e.id and cons.FabricCombo = e.PatternPanel
where e.FabricCode is not null and e.FabricCode !=''and cons.CuttingPiece='0' 

select b.cDate,b.CutRef,a.PatternPanel,b.FabricPanelCode,b.Cutno,CutQty = isnull(b.CutQty,0)
from #tmp1 a
left join #tmp2 b on a.Article=b.Article and a.SizeCode = b.SizeCode and a.PatternPanel = b.PatternPanel
order by a.PatternPanel,b.cDate
",
                    this.id,
                    this.article,
                    this.sizeCode);
            }

            DataTable gridData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out gridData);
            this.listControlBindingSource1.DataSource = gridData;
        }
    }
}
