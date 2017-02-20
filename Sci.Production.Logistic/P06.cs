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

namespace Sci.Production.Logistic
{
    public partial class P06 : Sci.Win.Tems.QueryForm
    {
        public P06(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            //Grid設定
            this.grid1.IsEditingReadOnly = true;
            this.grid1.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.grid1)
                .Date("ReturnDate", header: "Return Date")
                .Text("PackingListID", header: "Pack ID", width: Widths.AnsiChars(15))
                .Text("OrderID", header: "SP#", width: Widths.AnsiChars(15))
                .Text("CTNStartNo", header: "CTN#", width: Widths.AnsiChars(6))
                .Text("StyleID", header: "Style#", width: Widths.AnsiChars(15))
                .Text("BrandID", header: "Brand", width: Widths.AnsiChars(10))
                .Text("Customize1", header: "Order#", width: Widths.AnsiChars(15))
                .Text("CustPONo", header: "PO No.", width: Widths.AnsiChars(15))
                .Text("Dest", header: "Destination", width: Widths.AnsiChars(20))
                .Text("FactoryID", header: "Factory", width: Widths.AnsiChars(5))
                .Date("BuyerDelivery", header: "Buyer Delivery")
                .DateTime("AddDate",header: "Create Date");
        }

        //Query
        private void button1_Click(object sender, EventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(string.Format(@"select cr.ReturnDate,cr.PackingListID,cr.OrderID,cr.CTNStartNo,
isnull(o.StyleID,'') as StyleID,isnull(o.BrandID,'') as BrandID,isnull(o.Customize1,'') as Customize1,
isnull(o.CustPONo,'') as CustPONo,isnull(c.Alias,'') as Dest, isnull(o.FactoryID,'') as FactoryID,oq.BuyerDelivery,cr.AddDate
from ClogReturn cr WITH (NOLOCK) 
left join Orders o WITH (NOLOCK) on cr.OrderID =  o.ID
left join Country c WITH (NOLOCK) on o.Dest = c.ID
left join PackingList_Detail pd WITH (NOLOCK) on pd.ID = cr.PackingListID and pd.OrderID = cr.OrderID and pd.CTNStartNo = cr.CTNStartNo and pd.CTNQty > 0
left join Order_QtyShip oq WITH (NOLOCK) on oq.Id = pd.OrderID and oq.Seq = pd.OrderShipmodeSeq
where cr.MDivisionID = '{0}'", Sci.Env.User.Keyword));

            if (!MyUtility.Check.Empty(dateRange1.Value1))
            {
                sqlCmd.Append(string.Format(" and cr.ReturnDate >= '{0}'", Convert.ToDateTime(dateRange1.Value1).ToString("d")));
            }
            if (!MyUtility.Check.Empty(dateRange1.Value2))
            {
                sqlCmd.Append(string.Format(" and cr.ReturnDate <= '{0}'", Convert.ToDateTime(dateRange1.Value2).ToString("d")));
            }
            if (!MyUtility.Check.Empty(textBox1.Text))
            {
                sqlCmd.Append(string.Format(" and cr.PackingListID = '{0}'", MyUtility.Convert.GetString(textBox1.Text)));
            }
            if (!MyUtility.Check.Empty(textBox2.Text))
            {
                sqlCmd.Append(string.Format(" and cr.OrderID = '{0}'", MyUtility.Convert.GetString(textBox2.Text)));
            }
            sqlCmd.Append(" order by cr.ReturnDate,cr.PackingListID,cr.OrderID,cr.AddDate");
            DataTable gridData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out gridData);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Query data fail.\r\n"+result.ToString());
            }
            listControlBindingSource1.DataSource = gridData;
        }

        //Close
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //To Excel
        private void button3_Click(object sender, EventArgs e)
        {
            bool result = MyUtility.Excel.CopyToXls((DataTable)listControlBindingSource1.DataSource, "", xltfile: "Logistic_P06.xltx", headerRow: 1);
            if (!result) { MyUtility.Msg.WarningBox(result.ToString(), "Warning"); }
        }

    }
}
