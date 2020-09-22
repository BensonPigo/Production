using System;
using System.Data;
using System.Text;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.Warehouse
{
    public partial class P32_AccumulatedQty : Win.Subs.Base
    {
        public Win.Tems.Base P32;
        protected DataRow dr;

        public P32_AccumulatedQty(DataRow data)
        {
            this.InitializeComponent();
            this.dr = data;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            StringBuilder selectCommand1 = new StringBuilder();
            selectCommand1.Append(string.Format(
                @"
with cc as(
  select  d.ToPoid
          ,d.ToSeq1
          ,d.ToSeq2
          ,current_return = sum(d.Qty)  
  from dbo.BorrowBack_Detail d WITH (NOLOCK) 
  WHERE d.ID='{0}'
  group by d.ToPoid,d.ToSeq1,d.ToSeq2
  )
, acc as(
  select  bd1.ToPoid
          ,bd1.ToSeq1
          ,bd1.ToSeq2
          ,qty = sum(bd1.qty) 
  from dbo.BorrowBack b1 WITH (NOLOCK) 
  inner join dbo.BorrowBack_Detail bd1 WITH (NOLOCK) on b1.id = bd1.id 
  where b1.BorrowId='{1}' and b1.Status = 'Confirmed' and b1.id!='{0}'
  group by bd1.ToPoid,bd1.ToSeq1,bd1.ToSeq2
  )
, borrow as (
  select  bd.FromPoId
          ,bd.FromSeq1
          ,bd.FromSeq2
          ,borrowedqty = sum(bd.Qty) 
  from dbo.BorrowBack_Detail bd WITH (NOLOCK) 
  left join PO_Supp_Detail p WITH (NOLOCK) on p.id = bd.FromPoId and p.SEQ1 = bd.FromSeq1 and p.SEQ2 = bd.FromSeq2
  where bd.id='{1}'
  group by bd.FromPoId,bd.FromSeq1,bd.FromSeq2
  )
select  FromPoId
        ,FromSeq1
        ,FromSeq2
        ,borrowedqty
        ,qty = isnull(acc.qty,0.00) 
        ,current_return = isnull(cc.current_return,0.00)  
        ,balance = isnull(borrowedqty,0.00) - isnull(acc.qty,0.00) - isnull(cc.current_return,0.00)  
        ,p.Refno
        ,color_name = (select  color.name 
                       from color WITH (NOLOCK) 
                       where color.id = p.ColorID AND color.BrandId = o.BrandId)  
        --,color.name as color_name
        ,[description] = dbo.getMtlDesc(FromPoId,FromSeq1,FromSeq2,2,0)
from borrow 
left join acc on borrow.FromPoId = acc.ToPoid and borrow.FromSeq1 = acc.ToSeq1 and borrow.FromSeq2 = acc.ToSeq2
left join cc on borrow.FromPoId = cc.ToPoid and borrow.FromSeq1 = cc.ToSeq1 and borrow.FromSeq2 = cc.ToSeq2
left join dbo.PO_Supp_Detail p WITH (NOLOCK) on  borrow.FromPoId = p.id and borrow.FromSeq1 = p.SEQ1 and borrow.FromSeq2 = p.SEQ2
left join Orders o  WITH (NOLOCK) on  p.id = o.id
--inner join color color on color.id=p.ColorID",
                this.dr["id"].ToString(), this.dr["borrowid"].ToString()));

            DataTable selectDataTable1;
            this.P32.ShowWaitMessage("Data Loading...");
            DualResult selectResult1 = DBProxy.Current.Select(null, selectCommand1.ToString(), out selectDataTable1);

            if (selectResult1 == false)
            {
                this.ShowErr(selectCommand1.ToString(), selectResult1);
            }

            this.P32.HideWaitMessage();

            this.bindingSource1.DataSource = selectDataTable1;

            // 設定Grid1的顯示欄位
            this.gridAccumulatedQty.IsEditingReadOnly = true;
            this.gridAccumulatedQty.DataSource = this.bindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridAccumulatedQty)
                 .Text("frompoid", header: "Borrow SP#", width: Widths.AnsiChars(13))
                 .Text("fromseq1", header: "Borrow" + Environment.NewLine + "Seq1", width: Widths.AnsiChars(4))
                 .Text("fromseq2", header: "Borrow" + Environment.NewLine + "Seq2", width: Widths.AnsiChars(3))
                 .Numeric("borrowedqty", header: "Borrowing" + Environment.NewLine + "Qty", width: Widths.AnsiChars(8), integer_places: 10, decimal_places: 2)
                 .Numeric("qty", header: "Accu." + Environment.NewLine + "Return Qty", width: Widths.AnsiChars(8), integer_places: 10, decimal_places: 2)
                 .Numeric("current_return", header: "Return" + Environment.NewLine + "Qty", width: Widths.AnsiChars(8), integer_places: 10, decimal_places: 2)
                 .Numeric("balance", header: "Variance", width: Widths.AnsiChars(8), integer_places: 10, decimal_places: 2)
                 .Text("refno", header: "Refno", width: Widths.AnsiChars(15))
                 .EditText("color_name", header: "Color Name", width: Widths.AnsiChars(20))
                 .EditText("Description", header: "Description", width: Widths.AnsiChars(20))
                 ;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
