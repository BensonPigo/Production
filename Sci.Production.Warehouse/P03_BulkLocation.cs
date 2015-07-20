using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Sci;
using Sci.Data;
using Ict;

namespace Sci.Production.Warehouse
{
    public partial class P03_BulkLocation : Sci.Win.Subs.Base
    {
        DataRow dr;
        string stocktype;
        public P03_BulkLocation(DataRow data, string _stocktype)
        {
            InitializeComponent();
            dr = data;
            stocktype = _stocktype;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            string selectCommand1 = string.Format(@"select * from (SELECT LocationTrans.ID, LocationTrans.issuedate, LocationTrans.Remark
, sum(b.Qty) as qty
, (select cast(FromLocation as nvarchar )+',' 
	from LocationTrans_detail b1
	where id = LocationTrans.id and b1.Poid = b.Poid and b1.Seq1 = b.seq1 and b1.Seq2 = b.Seq2 and b1.tolocation !=''
	for xml path('')) as fromlocation
, (select cast(ToLocation as nvarchar )+',' 
	from LocationTrans_detail b2 
	where id = LocationTrans.id and b2.Poid = b.Poid and b2.Seq1 = b.seq1 and b2.Seq2 = b.Seq2 and b2.tolocation !=''
    for xml path('')) as ToLocation 
, LocationTrans.EditName+'-'+ (select pass1.NAME from pass1 where id = LocationTrans.EditName) editname
, LocationTrans.EditDate
FROM LocationTrans, LocationTrans_detail as b
WHERE LocationTrans.status = 'Confirmed' and LocationTrans.stocktype='B'
AND LocationTrans.ID = b.ID 
AND b.poid = '15060032GB   '
AND b.SEQ1 = '07 ' 
AND b.seq2 = '02'
group by LocationTrans.EditName,LocationTrans.ID, LocationTrans.issuedate, LocationTrans.Remark,LocationTrans.EditDate,b.Poid,b.seq1,b.seq2) tmp
order by EditName"
                , dr["id"].ToString()
                , dr["seq1"].ToString()
                , dr["seq2"].ToString()
                , stocktype);
            DataTable selectDataTable1;
            DualResult selectResult1 = DBProxy.Current.Select(null, selectCommand1, out selectDataTable1);
            if (selectResult1 == false) ShowErr(selectCommand1, selectResult1);
            else
            {
                bindingSource1.DataSource = selectDataTable1;
                selectDataTable1.DefaultView.Sort = "editdate";
            }

            //設定Grid1的顯示欄位
            MyUtility.Tool.SetGridFrozen(this.grid1);
            this.grid1.IsEditingReadOnly = true;
            this.grid1.DataSource = bindingSource1;
            
            Helper.Controls.Grid.Generator(this.grid1)
                .Date("issuedate", header: "Date", width: Widths.AnsiChars(10))
                 .Text("id", header: "Transaction ID", width: Widths.AnsiChars(13))
                 .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                 .Text("FromLocation", header: "Original Loc.", width: Widths.AnsiChars(15))
                 .Text("ToLocation", header: "New Loc.", width: Widths.AnsiChars(15))
                 .Text("Remark", header: "Remark", width: Widths.AnsiChars(10))
                 .Text("editname", header: "Edit name", width: Widths.AnsiChars(15))
                 .DateTime("editdate", header: "Edit Date", width: Widths.AnsiChars(20));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
