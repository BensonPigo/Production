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
isnull(o.CustPONo,'') as CustPONo,isnull(c.Alias,'') as Dest, isnull(o.FactoryID,'') as FactoryID,oq.BuyerDelivery,t.AddDate
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
            sqlCmd.Append(" order by t.TransferDate,t.PackingListID,t.OrderID,t.AddDate");
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

            string MyDocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(Application.StartupPath);
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.RestoreDirectory = true;
            dlg.InitialDirectory = MyDocumentsPath;     //指定"我的文件"路徑
            dlg.Title = "Save as Excel File";
            dlg.Filter = "Excel Files (*.xls)|*.xls";            // Set filter for file extension and default file extension

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK && dlg.FileName != null)
            {
                // Open document
                bool result = MyUtility.Excel.CopyToXls((DataTable)listControlBindingSource1.DataSource, dlg.FileName, xltfile: "Packing_P14.xltx", headerRow: 1);
                if (!result) { MyUtility.Msg.WarningBox(result.ToString(), "Warning"); }
            }
            else
            {
                return;
            }
        }

    }
}
