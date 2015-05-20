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

namespace Sci.Production.Subcon
{
    public partial class P01_Ap : Sci.Win.Subs.Base
    {
        DataRow dr;
        public P01_Ap(DataRow data)
        {
            InitializeComponent();
            dr = data;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            string selectCommand1 = string.Format("SELECT B.ID,A.issuedate,B.ApQty,B.PRICE,A.Handle,A.apvdate FROM ArtworkAP A, ArtworkAP_Detail B " +
	                                "WHERE A.ID = B.ID  AND B.artworkPOID = '{0}' and b.artworkpo_detailukey = {1}", dr["id"].ToString(), dr["ukey"].ToString());
            DataTable selectDataTable1;
            DualResult selectResult1 = DBProxy.Current.Select(null, selectCommand1, out selectDataTable1);
            if (selectResult1 == false) ShowErr(selectCommand1, selectResult1);

            bindingSource1.DataSource = selectDataTable1;

            //設定Grid1的顯示欄位
            this.grid1.IsEditingReadOnly = true;
            this.grid1.DataSource = bindingSource1;
            Helper.Controls.Grid.Generator(this.grid1)
                 .Text("id", header: "A/P#", width: Widths.AnsiChars(13))
                 .Date("issuedate", header: "Date", width: Widths.AnsiChars(13))
                 .Numeric("ApQty", header: "Qty", width: Widths.AnsiChars(6), integer_places: 6, decimal_places: 0)
                 .Numeric("Price", header: "Price", width: Widths.AnsiChars(6), integer_places: 13, decimal_places: 4)
                 .Text("Handle", header: "Handle", width: Widths.AnsiChars(8))
                 .DateTime("apvdate", header: "Approve Date", width: Widths.AnsiChars(20));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
