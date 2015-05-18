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

namespace Sci.Production.Subcon
{
    public partial class B01_History : Sci.Win.Subs.Base
    {
        protected DataRow motherData;
        public B01_History(DataRow data)
        {
            InitializeComponent();
            this.motherData = data;
            this.displayBox1.Text = motherData["refno"].ToString();
            this.displayBox2.Text = motherData["description"].ToString();
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            string selectCommand1 = string.Format("select issuedate,localap.id,qty,unitid,currencyid, price, qty*price amount"+
                ",localsuppid,localsupp.abb from localap, localap_detail,localsupp "+
                "where refno = '{0}' and localap.id = localap_detail.id and localsuppid = localsupp.id", this.motherData["refno"].ToString());
            DataTable selectDataTable1;

            DualResult selectResult1 = DBProxy.Current.Select(null, selectCommand1, out selectDataTable1);
            bindingSource1.DataSource = selectDataTable1;

            //設定Grid1的顯示欄位
            this.grid1.IsEditingReadOnly = true;
            this.grid1.DataSource = bindingSource1;
            Helper.Controls.Grid.Generator(this.grid1)
                 .Text("issuedate", header: "A/P Date", width: Widths.AnsiChars(10))
                 .Text("id", header: "A/P No", width: Widths.AnsiChars(13))
                 .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(6),integer_places:6,decimal_places:2)
                 .Text("Unitid", header: "Unit", width: Widths.AnsiChars(8))
                 .Text("Currencyid", header: "Currency", width: Widths.AnsiChars(3))
                 .Numeric("Price", header: "Price", width: Widths.AnsiChars(6), integer_places: 8, decimal_places: 4)
                 .Numeric("Amount", header: "Amount", width: Widths.AnsiChars(10), integer_places: 14, decimal_places: 4)
                 .Text("localsuppid", header: "Supplier", width: Widths.AnsiChars(8))
                 .Text("abb", header: "Supplier Abb.", width: Widths.AnsiChars(15));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
