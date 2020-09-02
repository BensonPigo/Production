using System;
using System.Data;
using Ict.Win;
using Sci.Data;
using Ict;

namespace Sci.Production.Warehouse
{
    public partial class P03_BulkLocation : Win.Subs.Base
    {
        private DataRow dr;
        private string stocktype;

        public P03_BulkLocation(DataRow data, string _stocktype)
        {
            this.InitializeComponent();
            this.dr = data;
            this.stocktype = _stocktype;

            switch (this.stocktype)
            {
                case "B":
                    this.Text = "P03_BulkLocation";
                    break;
                case "I":
                    this.Text = "P03_StockLocation";
                    break;
                case "O":
                    this.Text = "P03_CrapLocation";
                    break;
            }

            this.Text += string.Format(" ({0}-{1}- {2})", this.dr["id"].ToString(),
this.dr["seq1"].ToString(),
this.dr["seq2"].ToString());
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            string selectCommand1 = string.Format(
                @";with tmp as
(SELECT a.ID, a.issuedate, a.Remark
, sum(b.Qty) qty
, b.FromLocation
, b.ToLocation
, a.EditName
, a.EditDate
FROM LocationTrans a WITH (NOLOCK) inner join  LocationTrans_detail as b WITH (NOLOCK) on a.ID = b.ID 
WHERE a.status = 'Confirmed' and b.stocktype='{3}'
AND B.Poid='{0}' and b.Seq1='{1}' and b.Seq2='{2}'
group by 
a.ID, a.issuedate, a.Remark, b.FromLocation, b.ToLocation, a.EditName, a.EditDate
)
select issuedate,id,sum(qty) qty
,(select FromLocation+',' from (select distinct tmp_b.FromLocation from tmp tmp_b where tmp_b.id=a.ID) t for xml path('') ) fromlocation
,(select tolocation+',' from (select distinct tmp_c.ToLocation from tmp tmp_c where tmp_c.id=a.ID) t for xml path('') ) tolocation
,remark
,EditName+'-'+ isnull((select pass1.NAME from pass1 WITH (NOLOCK) where id = a.EditName),'') editname
,EditDate from  tmp a
group by EditName,ID,issuedate, Remark,EditDate
order by EditName,ID",
                this.dr["id"].ToString(),
                this.dr["seq1"].ToString(),
                this.dr["seq2"].ToString(),
                this.stocktype);
            DataTable selectDataTable1;
            DualResult selectResult1 = DBProxy.Current.Select(null, selectCommand1, out selectDataTable1);
            if (selectResult1 == false)
            {
                this.ShowErr(selectCommand1, selectResult1);
            }
            else
            {
                this.bindingSource1.DataSource = selectDataTable1;
                selectDataTable1.DefaultView.Sort = "editdate";
            }

            // 設定Grid1的顯示欄位
            this.gridBulkLocationTransaction.IsEditingReadOnly = true;
            this.gridBulkLocationTransaction.DataSource = this.bindingSource1;

            this.Helper.Controls.Grid.Generator(this.gridBulkLocationTransaction)
                .Date("issuedate", header: "Date", width: Widths.AnsiChars(10))
                 .Text("id", header: "Transaction ID", width: Widths.AnsiChars(14))
                 .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                 .Text("FromLocation", header: "Original Loc.", width: Widths.AnsiChars(15))
                 .Text("ToLocation", header: "New Loc.", width: Widths.AnsiChars(15))
                 .Text("Remark", header: "Remark", width: Widths.AnsiChars(10))
                 .Text("editname", header: "Edit name", width: Widths.AnsiChars(15))
                 .DateTime("editdate", header: "Edit Date", width: Widths.AnsiChars(20));
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
