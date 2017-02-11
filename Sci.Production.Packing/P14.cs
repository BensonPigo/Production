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


namespace Sci.Production.Packing
{
    public partial class P14 : Sci.Win.Tems.QueryForm
    {
        public P14(ToolStripMenuItem menuitem)
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
                .Date("TransferDate", header: "Transfer Date")
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
            sqlCmd.Append(string.Format(@"select t.TransferDate,t.PackingListID,t.OrderID,t.CTNStartNo,
isnull(o.StyleID,'') as StyleID,isnull(o.BrandID,'') as BrandID,isnull(o.Customize1,'') as Customize1,
isnull(o.CustPONo,'') as CustPONo,isnull(c.Alias,'') as Dest, isnull(o.FactoryID,'') as FactoryID, convert(varchar, oq.BuyerDelivery, 111) as BuyerDelivery,t.AddDate
from TransferToClog t
left join Orders o on t.OrderID =  o.ID
left join Country c on o.Dest = c.ID
left join PackingList_Detail pd on pd.ID = t.PackingListID and pd.OrderID = t.OrderID and pd.CTNStartNo = t.CTNStartNo and pd.CTNQty > 0
left join Order_QtyShip oq on oq.Id = pd.OrderID and oq.Seq = pd.OrderShipmodeSeq
where t.MDivisionID = '{0}'", Sci.Env.User.Keyword));

            if (!MyUtility.Check.Empty(dateRange1.Value1))
            {
                sqlCmd.Append(string.Format(" and t.TransferDate >= '{0}'", Convert.ToDateTime(dateRange1.Value1).ToString("d")));
            }
            if (!MyUtility.Check.Empty(dateRange1.Value2))
            {
                sqlCmd.Append(string.Format(" and t.TransferDate <= '{0}'", Convert.ToDateTime(dateRange1.Value2).ToString("d")));
            }
            if (!MyUtility.Check.Empty(textBox1.Text))
            {
                sqlCmd.Append(string.Format(" and t.PackingListID = '{0}'", MyUtility.Convert.GetString(textBox1.Text)));
            }
            if (!MyUtility.Check.Empty(textBox2.Text))
            {
                sqlCmd.Append(string.Format(" and t.OrderID = '{0}'", MyUtility.Convert.GetString(textBox2.Text)));
            }
            sqlCmd.Append(@" order by t.TransferDate,t.PackingListID,t.OrderID,
                                CAST(left(t.CTNStartNo, patindex('%[^0-9]%', t.CTNStartNo + ' ') -1 ) AS INT),
                                right(t.CTNStartNo, len(t.CTNStartNo) - patindex('%[^0-9]%', t.CTNStartNo + ' ') +1),
                                t.AddDate");
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
            button1_Click(null, null);
            DataTable ExcelTable = (DataTable)listControlBindingSource1.DataSource;


            if (ExcelTable == null || ExcelTable.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("No data!!");
                return;
            }
            //
            
            //bool result = MyUtility.Excel.CopyToXls(k, "", xltfile: "Packing_P14_TransferSlip.xltx", headerRow: 3);
            ////bool result = MyUtility.Excel.CopyToXls(ExcelTable, "", xltfile: "Packing_P14.xltx", headerRow: 1);
            //if (!result) { MyUtility.Msg.WarningBox(result.ToString(), "Warning"); }
            ////P14_Print_OrderList frm = new P14_Print_OrderList();
            ////frm.ShowDialog();
            string date1, date2, packID, SPNo;
            date1 = (!MyUtility.Check.Empty(dateRange1.Value1)) ? dateRange1.Text1 : null;
            date2 = (!MyUtility.Check.Empty(dateRange1.Value2)) ? dateRange1.Text1 : null;
            packID = (!MyUtility.Check.Empty(textBox1.Text)) ? textBox1.Text : null;
            SPNo = (!MyUtility.Check.Empty(textBox2.Text)) ? textBox2.Text : null;
            P14_Print_OrderList frm = new P14_Print_OrderList(ExcelTable, date1, date2, packID, SPNo);
            frm.ShowDialog();
        }

    }
}
