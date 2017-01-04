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
    public partial class P01_ProductionOutput_CuttingDetail : Sci.Win.Subs.Base
    {
        string workType, id, type, article, sizeCode;
        public P01_ProductionOutput_CuttingDetail(string WorkType, string ID, string Type, string Article,string SizeCode)
        {
            InitializeComponent();
            workType = WorkType;
            id = ID;
            type = Type;
            article = Article;
            sizeCode = SizeCode;
            if (type == "A")
            {
                Text = "Cutting Daily Output - " + id;
            }
            else
            {
                Text = "Cutting Daily Output - " + id + "(" + article + "-" + sizeCode + ")";
            }
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            //設定Grid1的顯示欄位
            this.grid1.IsEditingReadOnly = true;
            this.grid1.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.grid1)
                 .Date("CDate", header: "Date", width: Widths.AnsiChars(12))
                 .Text("CutRef", header: "Ref#", width: Widths.AnsiChars(6))
                 .Text("PatternPanel", header: "Fabric Comb", width: Widths.AnsiChars(2))
                 .Text("LectraCode", header: "Lectra Code", width: Widths.AnsiChars(2))
                 .Text("Cutno", header: "Cut#", width: Widths.AnsiChars(3))
                 .Numeric("CutQty", header: "Q'ty", width: Widths.AnsiChars(6))
                 .Text("Status", header: "Status", width: Widths.AnsiChars(8));
            grid1.Columns[1].Visible = type != "A";
            grid1.Columns[2].Visible = type != "A";
            grid1.Columns[3].Visible = type != "A";
            grid1.Columns[4].Visible = type != "A";
            grid1.Columns[6].Visible = type != "A";

            string sqlCmd;
            if (type == "A")
            {
                sqlCmd = string.Format(@"with AllPattern
as (
select distinct oq.Article,oq.SizeCode,oc.ColorID,oc.PatternPanel
from Orders o
inner join Order_Qty oq on o.ID = oq.ID
inner join Order_ColorCombo oc on o.POID = oc.Id and oq.Article = oc.Article
inner join Order_EachCons oe on oc.Id = oe.Id 
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
from Orders o
left join WorkOrder_Distribute wd on wd.OrderID = o.ID
left join CuttingOutput_Detail cd on cd.WorkOrderUkey = wd.WorkOrderUkey
left join CuttingOutput c on c.ID = cd.ID
left join WorkOrder_PatternPanel wp on wd.WorkOrderUkey = wp.WorkOrderUkey
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
group by cDate", string.Format("o.ID = '{0}'", id));
            }
            else
            {
                sqlCmd = string.Format(@"select c.cDate,cd.CutRef,wp.PatternPanel,w.LectraCode,CD.Cutno,sum(wd.Qty) as CutQty,Status
from Orders o
inner join WorkOrder_Distribute wd on wd.OrderID = o.ID and wd.Article = '{1}' and wd.SizeCode = '{2}'
inner join WorkOrder_PatternPanel wp on wp.WorkOrderUkey = wd.WorkOrderUkey
inner join WorkOrder w on w.ID=wp.ID and w.Ukey=wp.WorkOrderUkey
inner join CuttingOutput_Detail cd on cd.WorkOrderUkey = wd.WorkOrderUkey
inner join CuttingOutput c on c.ID = cd.ID
where {0}
group by c.cDate,cd.CutRef,wp.PatternPanel,w.LectraCode,CD.Cutno,Status"
                    , string.Format("o.ID = '{0}'", id)
                    ,article,sizeCode);
            }
            DataTable gridData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out gridData);
            listControlBindingSource1.DataSource = gridData;
        }
    }
}
