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


namespace Sci.Production.Warehouse
{
    public partial class P24_AccumulatedQty : Sci.Win.Subs.Base
    {
        protected DataRow dr;
        public P24_AccumulatedQty(DataRow data)
        {
            InitializeComponent();
            dr = data;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            StringBuilder selectCommand1 = new StringBuilder();
            selectCommand1.Append(string.Format(@";with 
group_detail
as
(
select d.FromPOID,d.fromseq1,fromseq2,sum(qty) scrap_qty from dbo.SubTransfer_Detail d where d.ID='{0}'
group by d.FromPOID,d.fromseq1,fromseq2
),
cte
as
(
select SubTransfer_Detail.FromPOID,SubTransfer_Detail.FromSeq1,SubTransfer_Detail.FromSeq2,sum(qty) as accu_qty 
from SubTransfer inner join SubTransfer_Detail on SubTransfer.id = SubTransfer_Detail.id
inner join group_detail d1 
on d1.FromPOID = SubTransfer_Detail.FromPOID and d1.FromSeq1 = SubTransfer_Detail.FromSeq1 and d1.FromSeq2 = SubTransfer_Detail.FromSeq2
where SubTransfer.type='E' and SubTransfer.Status='Confirmed' and SubTransfer.id !='{0}'
and SubTransfer_Detail.FromMDivisionID = '{1}'
group by SubTransfer_Detail.FromPOID,SubTransfer_Detail.FromSeq1,SubTransfer_Detail.FromSeq2
)
select FromPOID,fromseq1,fromseq2 
,round(isnull(sum(i.Qty),0.00)*v.rate,2) taipei_scrap
,isnull((select accu_qty from cte where cte.FromPOID = d.FromPOID and cte.FromSeq1 = d.FromSeq1 and cte.FromSeq2 = d.FromSeq2),0.00) accu_qty
,d.scrap_qty
,dbo.getMtlDesc(d.FromPOID,d.FromSeq1,d.FromSeq2,2,0) description
,p.StockUnit
,(round(isnull(sum(i.Qty),0.00)*v.rate,2) - isnull((select accu_qty from cte where cte.FromPOID = d.FromPOID and cte.FromSeq1 = d.FromSeq1 and cte.FromSeq2 = d.FromSeq2),0.00) - d.scrap_qty) as balance_qty
from group_detail d
LEFT join dbo.po_supp_detail p on p.id = d.frompoid and p.seq1 = d.fromseq1 and p.seq2 = d.fromseq2
LEFT join View_unitrate v on v.FROM_U = p.POUnit and v.TO_U = p.StockUnit
INNER join (Invtrans I inner join dbo.Factory on i.FactoryID = factory.ID
)on I.InventoryPOID = d.FromPOID and i.InventorySeq1 = d.FromSeq1 and i.InventorySeq2 = d.FromSeq2 and  i.Type='5'
where factory.MDivisionID='{1}'
group by d.FromPOID,d.fromseq1,d.fromseq2,v.rate,p.StockUnit,d.scrap_qty", dr["id"], Sci.Env.User.Keyword));

            DataTable selectDataTable1;
            MyUtility.Msg.WaitWindows("Data Loading...");
            DualResult selectResult1 = DBProxy.Current.Select(null, selectCommand1.ToString(), out selectDataTable1);
            
            if (selectResult1 == false)
            { ShowErr(selectCommand1.ToString(), selectResult1); }

            MyUtility.Msg.WaitClear();

            bindingSource1.DataSource = selectDataTable1;

            //設定Grid1的顯示欄位
            this.grid1.IsEditingReadOnly = true;
            this.grid1.DataSource = bindingSource1;
            Helper.Controls.Grid.Generator(this.grid1)
                 .Text("frompoid", header: "SP#", width: Widths.AnsiChars(13))
                 .Text("fromseq1", header: "Seq1", width: Widths.AnsiChars(4))
                 .Text("fromseq2", header: "Seq2", width: Widths.AnsiChars(3))
                 .Numeric("taipei_scrap", header: "Taipei" + Environment.NewLine + "Dispose", width: Widths.AnsiChars(12), integer_places: 10, decimal_places: 2)
                 .Numeric("accu_qty", header: "Accu. Scrap", width: Widths.AnsiChars(12), integer_places: 10, decimal_places: 2)
                 .Numeric("scrap_qty", header: "Scrap Qty", width: Widths.AnsiChars(12), integer_places: 10, decimal_places: 2)
                 .Numeric("balance_qty", header: "Balance", width: Widths.AnsiChars(12), integer_places: 10, decimal_places: 2)
                 .Text("stockunit", header: "Stock"+Environment.NewLine+"Unit", width: Widths.AnsiChars(5))
                 .EditText("Description", header: "Description", width: Widths.AnsiChars(40))
                 ;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
