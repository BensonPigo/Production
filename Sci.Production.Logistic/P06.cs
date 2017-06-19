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
        DataTable gridData;
        string selectDataTable_DefaultView_Sort = "";
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            //Grid設定
            this.gridReturnDate.IsEditingReadOnly = true;
            this.gridReturnDate.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.gridReturnDate)
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
                .DateTime("AddDate", header: "Create Date");

            // 增加CTNStartNo 有中文字的情況之下 按照我們希望的順序排
            int RowIndex = 0;
            int ColumIndex = 0;
            gridReturnDate.CellClick += (s, e) =>
            {
                RowIndex = e.RowIndex;
                ColumIndex = e.ColumnIndex;

            };

            gridReturnDate.Sorted += (s, e) =>
            {

                if ((RowIndex == -1) & (ColumIndex == 3))
                {

                    listControlBindingSource1.DataSource = null;

                    if (selectDataTable_DefaultView_Sort == "DESC")
                    {
                        gridData.DefaultView.Sort = "rn1 DESC";
                        selectDataTable_DefaultView_Sort = "";
                    }
                    else
                    {
                        gridData.DefaultView.Sort = "rn1 ASC";
                        selectDataTable_DefaultView_Sort = "DESC";
                    }
                    listControlBindingSource1.DataSource = gridData;
                    return;
                }


            };


            //
        }

        //Query
        private void btnQuery_Click(object sender, EventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(string.Format(@"
select *,
rn = ROW_NUMBER() over(order by Id,OrderID,(RIGHT(REPLICATE('0', 6) + rtrim(ltrim(CTNStartNo)), 6))),
rn1 = ROW_NUMBER() over(order by TRY_CONVERT(int, CTNStartNo) ,(RIGHT(REPLICATE('0', 6) + rtrim(ltrim(CTNStartNo)), 6)))
from (
select cr.ReturnDate,cr.PackingListID,cr.OrderID,cr.CTNStartNo,pd.Id,
isnull(o.StyleID,'') as StyleID,isnull(o.BrandID,'') as BrandID,isnull(o.Customize1,'') as Customize1,
isnull(o.CustPONo,'') as CustPONo,isnull(c.Alias,'') as Dest, isnull(o.FactoryID,'') as FactoryID,oq.BuyerDelivery,cr.AddDate

from ClogReturn cr WITH (NOLOCK) 
left join Orders o WITH (NOLOCK) on cr.OrderID =  o.ID
left join Country c WITH (NOLOCK) on o.Dest = c.ID
left join PackingList_Detail pd WITH (NOLOCK) on pd.ID = cr.PackingListID and pd.OrderID = cr.OrderID and pd.CTNStartNo = cr.CTNStartNo and pd.CTNQty > 0
left join Order_QtyShip oq WITH (NOLOCK) on oq.Id = pd.OrderID and oq.Seq = pd.OrderShipmodeSeq
where cr.MDivisionID = '{0}'", Sci.Env.User.Keyword));

            if (!MyUtility.Check.Empty(dateReturnDate.Value1))
            {
                sqlCmd.Append(string.Format(" and cr.ReturnDate >= '{0}'", Convert.ToDateTime(dateReturnDate.Value1).ToString("d")));
            }
            if (!MyUtility.Check.Empty(dateReturnDate.Value2))
            {
                sqlCmd.Append(string.Format(" and cr.ReturnDate <= '{0}'", Convert.ToDateTime(dateReturnDate.Value2).ToString("d")));
            }
            if (!MyUtility.Check.Empty(txtPackID.Text))
            {
                sqlCmd.Append(string.Format(" and cr.PackingListID = '{0}'", MyUtility.Convert.GetString(txtPackID.Text)));
            }
            if (!MyUtility.Check.Empty(txtSPNo.Text))
            {
                sqlCmd.Append(string.Format(" and cr.OrderID = '{0}'", MyUtility.Convert.GetString(txtSPNo.Text)));
            }
            sqlCmd.Append(")X order by rn");

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out gridData);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Query data fail.\r\n" + result.ToString());
            }
            listControlBindingSource1.DataSource = gridData;
        }

        //Close
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //To Excel
        private void btnToExcel_Click(object sender, EventArgs e)
        {
            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Logistic_P06.xltx"); //預先開啟excel app
            MyUtility.Excel.CopyToXls((DataTable)listControlBindingSource1.DataSource, "", "Logistic_P06.xltx", 3, true, null, objApp);// 將datatable copy to excel
            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
            objSheets.Cells[2, 2] = Sci.Env.User.Keyword;

            int r = ((DataTable)listControlBindingSource1.DataSource).Rows.Count;
            objSheets.get_Range(string.Format("A4:L{0}", r + 3)).Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;

            DataRow dr;
            MyUtility.Check.Seek(string.Format(@"select NameEN from Factory where id = '{0}'", Sci.Env.User.Keyword), out dr, null);
            objSheets.Cells[1, 1] = dr["NameEN"].ToString() + "\r\n" + "CARTON RETURN REPORT";

            string d1 = "", d2 = "";
            if (!MyUtility.Check.Empty(dateReturnDate.Value1))
            {
                d1 = Convert.ToDateTime(dateReturnDate.Value1).ToString("d");
            }
            if (!MyUtility.Check.Empty(dateReturnDate.Value2))
            {
                d2 = Convert.ToDateTime(dateReturnDate.Value2).ToString("d");
            }
            string drange = d1 + "~" + d2;

            objSheets.Cells[2, 4] = drange;
            objSheets.get_Range("A1").RowHeight = 45;

            ////

            //bool result = MyUtility.Excel.CopyToXls((DataTable)listControlBindingSource1.DataSource, "", xltfile: "Logistic_P06.xltx", headerRow: 1);
            //if (!result) { MyUtility.Msg.WarningBox(result.ToString(), "Warning"); }
        }

    }
}
