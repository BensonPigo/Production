using System;
using System.Data;
using System.Text;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.Warehouse
{
    public partial class P24_AccumulatedQty : Sci.Win.Subs.Base
    {
        public Sci.Win.Tems.Base P24;
        protected DataRow dr;

        public P24_AccumulatedQty(DataRow data)
        {
            this.InitializeComponent();
            this.dr = data;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            StringBuilder selectCommand1 = new StringBuilder();
            selectCommand1.Append(string.Format(
                @"
;with group_detail as(
    select 	d.FromPOID
           	,d.fromseq1
			,fromseq2
			,scrap_qty = sum(qty) 
    from dbo.SubTransfer_Detail d WITH (NOLOCK) 
    where d.ID='{0}'
    group by d.FromPOID, d.fromseq1, fromseq2
), cte as (
    select	SubTransfer_Detail.FromPOID
    		,SubTransfer_Detail.FromSeq1
    		,SubTransfer_Detail.FromSeq2
    		,accu_qty = sum(qty) 
    from SubTransfer WITH (NOLOCK) 
    inner join SubTransfer_Detail WITH (NOLOCK) on SubTransfer.id = SubTransfer_Detail.id
    inner join group_detail d1 WITH (NOLOCK) on d1.FromPOID = SubTransfer_Detail.FromPOID and d1.FromSeq1 = SubTransfer_Detail.FromSeq1 and d1.FromSeq2 = SubTransfer_Detail.FromSeq2
    where SubTransfer.type='E' and SubTransfer.Status='Confirmed' and SubTransfer.id !='{0}'
    group by SubTransfer_Detail.FromPOID, SubTransfer_Detail.FromSeq1, SubTransfer_Detail.FromSeq2
)
select	FromPOID
		,fromseq1
		,fromseq2 
		,taipei_scrap = round(dbo.GetUnitQty(p.POUnit, p.StockUnit, isnull(sum(i.Qty),0.00)), 2) 
		,accu_qty = isnull((select accu_qty from cte where cte.FromPOID = d.FromPOID and cte.FromSeq1 = d.FromSeq1 and cte.FromSeq2 = d.FromSeq2),0.00) 
		,d.scrap_qty
		,description = dbo.getMtlDesc(d.FromPOID,d.FromSeq1,d.FromSeq2,2,0) 
		,p.StockUnit
		,balance_qty = (round(dbo.GetUnitQty(p.POUnit, p.StockUnit, isnull(sum(i.Qty),0.00)),2) - isnull((select accu_qty from cte where cte.FromPOID = d.FromPOID and cte.FromSeq1 = d.FromSeq1 and cte.FromSeq2 = d.FromSeq2),0.00) - d.scrap_qty)
from group_detail d WITH (NOLOCK) 
LEFT join dbo.po_supp_detail p WITH (NOLOCK) on p.id = d.frompoid and p.seq1 = d.fromseq1 and p.seq2 = d.fromseq2
INNER join (
		    Invtrans I WITH (NOLOCK) 
            inner join dbo.Factory WITH (NOLOCK) on i.FactoryID = factory.ID
	  ) on I.InventoryPOID = d.FromPOID and i.InventorySeq1 = d.FromSeq1 and i.InventorySeq2 = d.FromSeq2 and  i.Type='5'
where factory.MDivisionID='{1}'
group by d.FromPOID,d.fromseq1,d.fromseq2,p.StockUnit,d.scrap_qty, p.POUnit", this.dr["id"], Sci.Env.User.Keyword));

            DataTable selectDataTable1;
            this.P24.ShowWaitMessage("Data Loading...");
            DualResult selectResult1 = DBProxy.Current.Select(null, selectCommand1.ToString(), out selectDataTable1);

            if (selectResult1 == false)
            {
                this.ShowErr(selectCommand1.ToString(), selectResult1);
            }

            this.P24.HideWaitMessage();

            this.bindingSource1.DataSource = selectDataTable1;

            // 設定Grid1的顯示欄位
            this.gridAccumulatedQty.IsEditingReadOnly = true;
            this.gridAccumulatedQty.DataSource = this.bindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridAccumulatedQty)
                 .Text("frompoid", header: "SP#", width: Widths.AnsiChars(13))
                 .Text("fromseq1", header: "Seq1", width: Widths.AnsiChars(4))
                 .Text("fromseq2", header: "Seq2", width: Widths.AnsiChars(3))
                 .Numeric("taipei_scrap", header: "Taipei" + Environment.NewLine + "Dispose", width: Widths.AnsiChars(12), integer_places: 10, decimal_places: 2)
                 .Numeric("accu_qty", header: "Accu. Scrap", width: Widths.AnsiChars(12), integer_places: 10, decimal_places: 2)
                 .Numeric("scrap_qty", header: "Scrap Qty", width: Widths.AnsiChars(12), integer_places: 10, decimal_places: 2)
                 .Numeric("balance_qty", header: "Balance", width: Widths.AnsiChars(12), integer_places: 10, decimal_places: 2)
                 .Text("stockunit", header: "Stock" + Environment.NewLine + "Unit", width: Widths.AnsiChars(5))
                 .EditText("Description", header: "Description", width: Widths.AnsiChars(40))
                 ;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
