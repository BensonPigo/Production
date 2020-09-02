using System;
using System.Data;
using System.Text;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.Warehouse
{
    public partial class P31_AccumulatedQty : Win.Subs.Base
    {
        public Win.Tems.Base P31;
        protected DataRow dr;

        public P31_AccumulatedQty(DataRow data)
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
select  A.FromPoId
        ,A.FromSeq1
        ,A.FromSeq2
        ,Qty = sum(a.Qty)  
        ,pd.Refno
        ,color_name = c.Name 
        ,stocktype = case when a.FromStocktype ='B' then 'Bulk' 
                          when a.FromStocktype='I' then 'Inventory' 
                          else a.fromstocktype end  
        ,[Description] = dbo.getmtldesc(a.FromPoId,a.FromSeq1,a.FromSeq2,2,0)  
from dbo.BorrowBack_Detail a WITH (NOLOCK) 
left join (PO_Supp_Detail pd WITH (NOLOCK) 
            left join Orders o WITH (NOLOCK) on o.id=pd.ID
            inner join color c WITH (NOLOCK) on c.id = pd.ColorID AND C.BrandId = o.BrandId
          ) on a.FromPoId = pd.ID and a.FromSeq1= pd.seq1 and a.FromSeq2 =  pd.seq2
where a.Id = '{0}'
GROUP BY A.FromPoId,A.FromSeq1,A.FromSeq2,a.FromStocktype,pd.Refno,c.Name", this.dr["id"].ToString()));

            DataTable selectDataTable1;
            this.P31.ShowWaitMessage("Data Loading....");
            DualResult selectResult1 = DBProxy.Current.Select(null, selectCommand1.ToString(), out selectDataTable1);

            if (selectResult1 == false)
            {
                this.ShowErr(selectCommand1.ToString(), selectResult1);
            }

            this.P31.HideWaitMessage();

            this.bindingSource1.DataSource = selectDataTable1;

            // 設定Grid1的顯示欄位
            this.gridAccumulatedQty.IsEditingReadOnly = true;
            this.gridAccumulatedQty.DataSource = this.bindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridAccumulatedQty)
                 .Text("frompoid", header: "From SP#", width: Widths.AnsiChars(13))
                 .Text("fromseq1", header: "From Seq1", width: Widths.AnsiChars(4))
                 .Text("fromseq2", header: "From Seq2", width: Widths.AnsiChars(3))
                 .Numeric("qty", header: "Borrowing Qty", width: Widths.AnsiChars(8), integer_places: 10, decimal_places: 2)
                 .Text("refno", header: "Refno", width: Widths.AnsiChars(23))
                 .EditText("color_name", header: "Color Name", width: Widths.AnsiChars(20))
                 .Text("stocktype", header: "Stock Type", width: Widths.AnsiChars(10))
                 .EditText("Description", header: "Description", width: Widths.AnsiChars(20))
                 ;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
