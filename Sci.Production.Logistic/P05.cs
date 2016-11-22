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
    public partial class P05 : Sci.Win.Tems.QueryForm
    {
        public P05(ToolStripMenuItem menuitem)
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
                .Date("ReceiveDate", header: "Receive Date")
                .Text("PackingListID", header: "Pack ID", width: Widths.Auto())
                .Text("OrderID", header: "SP#", width: Widths.Auto())
                .Text("seq", header: "SEQ", width: Widths.Auto())
                .Text("CTNStartNo", header: "CTN#", width: Widths.Auto())
                .Text("StyleID", header: "Style#", width: Widths.Auto())
                .Text("BrandID", header: "Brand", width: Widths.Auto())
                .Text("Customize1", header: "Order#", width: Widths.Auto())
                .Text("CustPONo", header: "PO No.", width: Widths.Auto())
                .Text("Dest", header: "Destination", width: Widths.Auto())
                .Text("FactoryID", header: "Factory", width: Widths.Auto())
                .Date("BuyerDelivery", header: "Buyer Delivery", width: Widths.Auto())
                .CellClogLocation("ClogLocationId", header: "Location No", width: Widths.Auto())
                .DateTime("AddDate", header: "Create Date", width: Widths.Auto());
        }

        //Query
        private void button1_Click(object sender, EventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(string.Format(@"select cr.ReceiveDate,cr.PackingListID,cr.OrderID,oq.Seq,cr.CTNStartNo,
isnull(o.StyleID,'') as StyleID,isnull(o.BrandID,'') as BrandID,isnull(o.Customize1,'') as Customize1,
isnull(o.CustPONo,'') as CustPONo,isnull(c.Alias,'') as Dest, isnull(o.FactoryID,'') as FactoryID,oq.BuyerDelivery,cr.ClogLocationId,cr.AddDate
from ClogReceive cr
left join Orders o on cr.OrderID =  o.ID
left join Country c on o.Dest = c.ID
left join PackingList_Detail pd on pd.ID = cr.PackingListID and pd.OrderID = cr.OrderID and pd.CTNStartNo = cr.CTNStartNo and pd.CTNQty > 0
left join Order_QtyShip oq on oq.Id = pd.OrderID and oq.Seq = pd.OrderShipmodeSeq
where cr.MDivisionID = '{0}'", Sci.Env.User.Keyword));

            if (!MyUtility.Check.Empty(dateRange1.Value1))
            {
                sqlCmd.Append(string.Format(" and cr.ReceiveDate >= '{0}'", Convert.ToDateTime(dateRange1.Value1).ToString("d")));
            }
            if (!MyUtility.Check.Empty(dateRange1.Value2))
            {
                sqlCmd.Append(string.Format(" and cr.ReceiveDate <= '{0}'", Convert.ToDateTime(dateRange1.Value2).ToString("d")));
            }
            if (!MyUtility.Check.Empty(textBox1.Text))
            {
                sqlCmd.Append(string.Format(" and cr.PackingListID = '{0}'", MyUtility.Convert.GetString(textBox1.Text)));
            }
            if (!MyUtility.Check.Empty(textBox2.Text))
            {
                sqlCmd.Append(string.Format(" and cr.OrderID = '{0}'", MyUtility.Convert.GetString(textBox2.Text)));
            }
            sqlCmd.Append(" order by cr.ReceiveDate,cr.PackingListID,cr.OrderID,cr.AddDate");
            DataTable gridData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out gridData);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Query data fail.\r\n"+result.ToString());
            }
            listControlBindingSource1.DataSource = gridData;
            grid1.AutoResizeColumns();
        }

        //Close
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //To Excel
        private void button3_Click(object sender, EventArgs e)
        {
            bool result = MyUtility.Excel.CopyToXls((DataTable)listControlBindingSource1.DataSource, "", xltfile: "Logistic_P05.xltx", headerRow: 1);
            if (!result) { MyUtility.Msg.WarningBox(result.ToString(), "Warning"); }
        }

    }
}
