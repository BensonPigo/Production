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
            this.gridReceiveDate.IsEditingReadOnly = true;
            this.gridReceiveDate.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.gridReceiveDate)
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
        private void btnQuery_Click(object sender, EventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(string.Format(@"select cr.ReceiveDate,cr.PackingListID,cr.OrderID,oq.Seq,cr.CTNStartNo,
isnull(o.StyleID,'') as StyleID,isnull(o.BrandID,'') as BrandID,isnull(o.Customize1,'') as Customize1,
isnull(o.CustPONo,'') as CustPONo,isnull(c.Alias,'') as Dest, isnull(o.FactoryID,'') as FactoryID,oq.BuyerDelivery,cr.ClogLocationId,cr.AddDate
from ClogReceive cr WITH (NOLOCK) 
left join Orders o WITH (NOLOCK) on cr.OrderID =  o.ID
left join Country c WITH (NOLOCK) on o.Dest = c.ID
left join PackingList_Detail pd WITH (NOLOCK) on pd.ID = cr.PackingListID and pd.OrderID = cr.OrderID and pd.CTNStartNo = cr.CTNStartNo and pd.CTNQty > 0
left join Order_QtyShip oq WITH (NOLOCK) on oq.Id = pd.OrderID and oq.Seq = pd.OrderShipmodeSeq
where cr.MDivisionID = '{0}'", Sci.Env.User.Keyword));

            if (!MyUtility.Check.Empty(dateReceiveDate.Value1))
            {
                sqlCmd.Append(string.Format(" and cr.ReceiveDate >= '{0}'", Convert.ToDateTime(dateReceiveDate.Value1).ToString("d")));
            }
            if (!MyUtility.Check.Empty(dateReceiveDate.Value2))
            {
                sqlCmd.Append(string.Format(" and cr.ReceiveDate <= '{0}'", Convert.ToDateTime(dateReceiveDate.Value2).ToString("d")));
            }
            if (!MyUtility.Check.Empty(txtPackID.Text))
            {
                sqlCmd.Append(string.Format(" and cr.PackingListID = '{0}'", MyUtility.Convert.GetString(txtPackID.Text)));
            }
            if (!MyUtility.Check.Empty(txtSPNo.Text))
            {
                sqlCmd.Append(string.Format(" and cr.OrderID = '{0}'", MyUtility.Convert.GetString(txtSPNo.Text)));
            }
            sqlCmd.Append(" order by cr.ReceiveDate,cr.PackingListID,cr.OrderID,cr.AddDate");
            DataTable gridData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out gridData);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Query data fail.\r\n"+result.ToString());
            }
            listControlBindingSource1.DataSource = gridData;
            gridReceiveDate.AutoResizeColumns();
        }

        //Close
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //To Excel
        private void btnToExcel_Click(object sender, EventArgs e)
        {
            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Logistic_P05.xltx"); //預先開啟excel app
            MyUtility.Excel.CopyToXls((DataTable)listControlBindingSource1.DataSource, "", "Logistic_P05.xltx", 4, true, null, objApp);// 將datatable copy to excel
            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表

            int r = ((DataTable)listControlBindingSource1.DataSource).Rows.Count;
            objSheets.get_Range(string.Format("A5:N{0}",r+4)).Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;

            objSheets.Cells[2, 2] = Sci.Env.User.Keyword;
            DataRow dr;
            MyUtility.Check.Seek(string.Format(@"select NameEN from Factory where id = '{0}'",Sci.Env.User.Keyword), out dr, null);
            objSheets.Cells[1, 1] = dr["NameEN"].ToString() + "\r\n" + "CARTON RECEIVING REPORT";
            string d1 = "", d2 = "";
            if (!MyUtility.Check.Empty(dateReceiveDate.Value1))
            {
                d1 = Convert.ToDateTime(dateReceiveDate.Value1).ToString("d");
            }
            if (!MyUtility.Check.Empty(dateReceiveDate.Value2))
            {
                d2 = Convert.ToDateTime(dateReceiveDate.Value2).ToString("d");
            }
            string drange = d1 + "~" + d2;
            objSheets.Cells[3, 13] = drange;
            objSheets.get_Range("A1").RowHeight = 45;            
        }
    }
}
