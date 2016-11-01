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
    public partial class P31_AccumulatedQty : Sci.Win.Subs.Base
    {
        protected DataRow dr;
        public P31_AccumulatedQty(DataRow data)
        {
            InitializeComponent();
            dr = data;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            StringBuilder selectCommand1 = new StringBuilder();
            selectCommand1.Append(string.Format(@"select A.FromPoId,A.FromSeq1,A.FromSeq2
,sum(a.Qty) as Qty
,pd.Refno
,c.Name as color_name
,case when a.FromStocktype ='B' then 'Bulk' when a.FromStocktype='I' then 'Inventory' else a.fromstocktype end as stocktype
,dbo.getmtldesc(a.FromPoId,a.FromSeq1,a.FromSeq2,2,0) as [Description]
from dbo.BorrowBack_Detail a left join 
        (PO_Supp_Detail pd 
left join Orders o on o.id=pd.ID
inner join color c on c.id = pd.ColorID AND C.BrandId = o.BrandId) on a.FromPoId = pd.ID and a.FromSeq1= pd.seq1 and a.FromSeq2 =  pd.seq2
where a.Id = '{0}'
GROUP BY A.FromPoId,A.FromSeq1,A.FromSeq2,a.FromStocktype,pd.Refno,c.Name", dr["id"].ToString()));

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

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
