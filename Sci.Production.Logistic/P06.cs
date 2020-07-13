#pragma warning disable SA1652 // Enable XML documentation output
using System;
#pragma warning restore SA1652 // Enable XML documentation output
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;
using System.Runtime.InteropServices;

namespace Sci.Production.Logistic
{
    /// <summary>
    /// Logistic_P06
    /// </summary>
    public partial class P06 : Win.Tems.QueryForm
    {
        /// <summary>
        /// P06
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public P06(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        private DataTable gridData;
        private string selectDataTable_DefaultView_Sort = string.Empty;
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;

        /// <summary>
        /// OnFormLoaded()
        /// </summary>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            // Grid設定
            this.gridReturnDate.IsEditingReadOnly = false;
            this.gridReturnDate.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridReturnDate)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
                .Date("ReturnDate", header: "Return Date", iseditable: false)
                .Text("PackingListID", header: "Pack ID", width: Widths.AnsiChars(15), iseditable: false)
                .Text("OrderID", header: "SP#", width: Widths.AnsiChars(15), iseditable: false)
                .Text("CTNStartNo", header: "CTN#", width: Widths.AnsiChars(6), iseditable: false)
                .Text("StyleID", header: "Style#", width: Widths.AnsiChars(15), iseditable: false)
                .Text("BrandID", header: "Brand", width: Widths.AnsiChars(10), iseditable: false)
                .Text("Customize1", header: "Order#", width: Widths.AnsiChars(15), iseditable: false)
                .Text("CustPONo", header: "PO No.", width: Widths.AnsiChars(15), iseditable: false)
                .Text("Dest", header: "Destination", width: Widths.AnsiChars(20), iseditable: false)
                .Text("FactoryID", header: "Factory", width: Widths.AnsiChars(5), iseditable: false)
                .Date("BuyerDelivery", header: "Buyer Delivery", iseditable: false)
                .DateTime("AddDate", header: "Create Date", iseditable: false)
                .Text("AddName", header: "AddName", width: Widths.AnsiChars(15), iseditable: false)
                .Text("RepackPackID", header: "Repack To Pack ID", width: Widths.AnsiChars(15), iseditable: false)
                .Text("RepackOrderID", header: "Repack To SP #", width: Widths.AnsiChars(15), iseditable: false)
                .Text("RepackCtnStartNo", header: "Repack To CTN #", width: Widths.AnsiChars(6), iseditable: false);

            // 增加CTNStartNo 有中文字的情況之下 按照我們希望的順序排
            int rowIndex = 0;
            int columIndex = 0;
            this.gridReturnDate.CellClick += (s, e) =>
            {
                rowIndex = e.RowIndex;
                columIndex = e.ColumnIndex;
            };

            this.gridReturnDate.Sorted += (s, e) =>
            {
                #region 如果準備排序的欄位 = "CTNStartNo" 則用以下方法排序
                if ((rowIndex == -1) && this.gridData.Columns[columIndex].ColumnName.ToString().EqualString("CTNStartNo"))
                {
                    this.listControlBindingSource1.DataSource = null;

                    if (this.selectDataTable_DefaultView_Sort == "DESC")
                    {
                        this.gridData.DefaultView.Sort = "rn DESC";
                        this.selectDataTable_DefaultView_Sort = string.Empty;
                    }
                    else
                    {
                        this.gridData.DefaultView.Sort = "rn ASC";
                        this.selectDataTable_DefaultView_Sort = "DESC";
                    }

                    this.listControlBindingSource1.DataSource = this.gridData;
                    return;
                }
                #endregion
            };
        }

        // Query
        private void BtnQuery_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.dateReturnDate.Value1) && MyUtility.Check.Empty(this.dateReturnDate.Value2))
            {
                MyUtility.Msg.WarningBox("Please input <Return Date> first!");
                return;
            }

            this.ShowWaitMessage("Data Loading...");
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(string.Format(
                @"
select  1 as selected
        , *
        , rn = ROW_NUMBER() over (order by TRY_CONVERT (int, CTNStartNo), (RIGHT (REPLICATE ('0', 6) + rtrim (ltrim (CTNStartNo)), 6)))
from (
    select  cr.ReturnDate
            , [PackingListID] = cr.PackingListID  --iif(pd.OrigID = '',cr.PackingListID, pd.OrigID)
            , [OrderID] =cr.OrderID   --iif(pd.OrigOrderID = '',pd.OrderID, pd.OrigOrderID)
            , [CTNStartNo] = cr.CTNStartNo   --iif(pd.OrigCTNStartNo = '',pd.CTNStartNo, pd.OrigCTNStartNo)
            , isnull (o.StyleID, '') as StyleID
            , isnull (o.BrandID, '') as BrandID
            , isnull (o.Customize1, '') as Customize1
            , isnull (o.CustPONo, '') as CustPONo
            , isnull (c.Alias, '') as Dest
            , isnull (o.FactoryID, '') as FactoryID
            , oq.BuyerDelivery
            , cr.AddDate
			, AddName = (select concat(id,'-',Name) from pass1 where id = cr.AddName)

            , [RepackPackID] = iif(pd.OrigID != '',pd.ID, pd.OrigID)
            , [RepackOrderID] = iif(pd.OrigOrderID != '',pd.OrderID, pd.OrigOrderID)
            , [RepackCtnStartNo] = iif(pd.OrigCTNStartNo != '',pd.CTNStartNo, pd.OrigCTNStartNo)

    from  ClogReturn cr WITH (NOLOCK) 
    left join  PackingList_Detail pd WITH (NOLOCK) on pd.ID = cr.PackingListID and pd.CTNStartNo = cr.CTNStartNo 
    left join Orders o WITH (NOLOCK) on cr.OrderID =  o.ID
    left join Country c WITH (NOLOCK) on o.Dest = c.ID
    left join Order_QtyShip oq WITH (NOLOCK) on oq.Id = pd.OrderID and oq.Seq = pd.OrderShipmodeSeq

    where cr.MDivisionID = '{0}'
    --and pd.ReturnDate is not null 
    ", Env.User.Keyword));

            if (!MyUtility.Check.Empty(this.dateReturnDate.Value1))
            {
                sqlCmd.Append(string.Format(
                    @" 
            and cr.ReturnDate >= '{0}'", Convert.ToDateTime(this.dateReturnDate.Value1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.dateReturnDate.Value2))
            {
                sqlCmd.Append(string.Format(
                    @" 
            and cr.ReturnDate <= '{0}'", Convert.ToDateTime(this.dateReturnDate.Value2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.txtPackID.Text))
            {
                sqlCmd.Append(string.Format(
                    @" 
            and (cr.PackingListID = '{0}' or  pd.OrigID = '{0}')", MyUtility.Convert.GetString(this.txtPackID.Text)));
            }

            if (!MyUtility.Check.Empty(this.txtSPNo.Text))
            {
                sqlCmd.Append(string.Format(
                    @" 
            and (cr.OrderID = '{0}' or pd.OrigOrderID = '{0}')", MyUtility.Convert.GetString(this.txtSPNo.Text)));
            }

            sqlCmd.Append(@"
) X 
order by PackingListID, OrderID, rn");

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out this.gridData);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Query data fail.\r\n" + result.ToString());
            }

            this.listControlBindingSource1.DataSource = this.gridData;
            this.HideWaitMessage();
        }

        // Close
        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // To Excel
        private void BtnToExcel_Click(object sender, EventArgs e)
        {
            DataTable excelTable = (DataTable)this.listControlBindingSource1.DataSource;
            DataTable printDT = excelTable.Clone();

            if (excelTable == null || excelTable.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("No data!!");
                return;
            }

            // 如果沒勾選資料,會跳訊息
            foreach (DataRow dr1 in excelTable.Rows)
            {
                if (dr1["Selected"].EqualString("1"))
                {
                    printDT.ImportRow(dr1);
                }
            }

            if (printDT.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Checked item first before click ToExcel");
                return;
            }

            this.ShowWaitMessage("Excel Processing...");

            /*
             * 輸出的資料中
             * 1. Selected，此欄位是為了判斷是否需要列印
             * 2. rn，此欄位是為了 SQL 排序
             */
            printDT.Columns.Remove("Selected");
            printDT.Columns.Remove("rn");

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Logistic_P06.xltx"); // 預先開啟excel app
            MyUtility.Excel.CopyToXls(printDT, string.Empty, "Logistic_P06.xltx", 3, false, null, objApp); // 將datatable copy to excel
            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
            objSheets.Cells[2, 2] = Env.User.Keyword;

            int r = printDT.Rows.Count;
            objSheets.get_Range(string.Format("A4:M{0}", r + 3)).Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;

            DataRow dr;
            MyUtility.Check.Seek(string.Format(@"select NameEN from Factory where id = '{0}'", Env.User.Factory), out dr, null);
            objSheets.Cells[1, 1] = dr["NameEN"].ToString() + "\r\n" + "CARTON RETURN REPORT";

            string d1 = string.Empty, d2 = string.Empty;
            if (!MyUtility.Check.Empty(this.dateReturnDate.Value1))
            {
                d1 = Convert.ToDateTime(this.dateReturnDate.Value1).ToString("yyyy/MM/dd");
            }

            if (!MyUtility.Check.Empty(this.dateReturnDate.Value2))
            {
                d2 = Convert.ToDateTime(this.dateReturnDate.Value2).ToString("yyyy/MM/dd");
            }

            string drange = d1 + "~" + d2;

            objSheets.Cells[2, 4] = drange;
            objSheets.get_Range("A1").RowHeight = 45;

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("Logistic_P06");
            Microsoft.Office.Interop.Excel.Workbook workbook = objApp.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);
            Marshal.ReleaseComObject(objSheets);
            Marshal.ReleaseComObject(workbook);

            strExcelName.OpenFile();
            #endregion
            this.HideWaitMessage();
        }
    }
}
