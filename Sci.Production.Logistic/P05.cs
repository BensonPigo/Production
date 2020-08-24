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
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Logistic
{
    /// <summary>
    /// Logistic_P05
    /// </summary>
    public partial class P05 : Win.Tems.QueryForm
    {
        /// <summary>
        /// P05
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public P05(ToolStripMenuItem menuitem)
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
            this.gridReceiveDate.IsEditingReadOnly = false;
            this.gridReceiveDate.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridReceiveDate)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
                .Date("ReceiveDate", header: "Receive Date", iseditable: false)
                .Text("PackingListID", header: "Pack ID", width: Widths.Auto(), iseditable: false)
                .Text("OrderID", header: "SP#", width: Widths.Auto(), iseditable: false)
                .Text("seq", header: "SEQ", width: Widths.Auto(), iseditable: false)
                .Text("CTNStartNo", header: "CTN#", width: Widths.Auto(), iseditable: false)
                .Text("StyleID", header: "Style#", width: Widths.Auto(), iseditable: false)
                .Text("BrandID", header: "Brand", width: Widths.Auto(), iseditable: false)
                .Text("Customize1", header: "Order#", width: Widths.Auto(), iseditable: false)
                .Text("CustPONo", header: "PO No.", width: Widths.Auto(), iseditable: false)
                .Text("Dest", header: "Destination", width: Widths.Auto(), iseditable: false)
                .Text("FactoryID", header: "Factory", width: Widths.Auto(), iseditable: false)
                .Date("BuyerDelivery", header: "Buyer Delivery", width: Widths.Auto(), iseditable: false)
                .CellClogLocation("ClogLocationId", header: "Location No", width: Widths.Auto(), iseditable: false)
                .DateTime("AddDate", header: "Create Date", width: Widths.Auto(), iseditable: false)
                .Text("AddName", header: "AddName", width: Widths.Auto(), iseditable: false)
                .Text("RepackPackID", header: "Repack To Pack ID", width: Widths.AnsiChars(15), iseditable: false)
                .Text("RepackOrderID", header: "Repack To SP #", width: Widths.AnsiChars(15), iseditable: false)
                .Text("RepackCtnStartNo", header: "Repack To CTN #", width: Widths.AnsiChars(6), iseditable: false);

            // 增加CTNStartNo 有中文字的情況之下 按照我們希望的順序排
            int rowIndex = 0;
            int columIndex = 0;
            this.gridReceiveDate.CellClick += (s, e) =>
            {
                rowIndex = e.RowIndex;
                columIndex = e.ColumnIndex;
            };

            this.gridReceiveDate.Sorted += (s, e) =>
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
            if (MyUtility.Check.Empty(this.dateReceiveDate.Value1) && MyUtility.Check.Empty(this.dateReceiveDate.Value2))
            {
                MyUtility.Msg.WarningBox("Please input <Receive Date> first!");
                return;
            }

            this.ShowWaitMessage("Data Loading...");
            StringBuilder sqlCmd = new StringBuilder();
            StringBuilder sqlCmdWhere = new StringBuilder();
            StringBuilder sqlCmdWhere_Ori = new StringBuilder();

            if (!MyUtility.Check.Empty(this.txtPackID.Text))
            {
                sqlCmdWhere.Append($"and pd.PackingListID = '{this.txtPackID.Text}'");
                sqlCmdWhere_Ori.Append($"and pd.OrigID =  '{this.txtPackID.Text}' ");
            }

            if (!MyUtility.Check.Empty(this.txtSPNo.Text))
            {
                sqlCmdWhere.Append($"and pd.OrderID = '{this.txtSPNo.Text}'");
                sqlCmdWhere_Ori.Append($"and pd.OrigOrderID =  '{this.txtSPNo.Text}' ");
            }

            sqlCmd.Append($@"
SELECT *
INTO   #tmp_NoRepeat
FROM
(
	SELECT DISTINCT ID,CTNStartNo ,OrderID,PackingListID,MDivisionID,ReceiveDate,AddDate,AddName,ClogLocationId 
	FROM ClogReceive pd
	WHERE 1=1
	{sqlCmdWhere}

)a

SELECT DISTINCT
[PackID]= cr.PackingListID
, pd.OrderShipmodeSeq 
, pd.OrigID
, pd.OrigOrderID 
, pd.OrigCTNStartNo 
, cr.ID
, cr.CTNStartNo 
, cr.OrderID
, cr.PackingListID
, cr.MDivisionID
, cr.ReceiveDate
, cr.AddDate
, cr.AddName
, cr.ClogLocationId
INTO #MainTable
from  #tmp_NoRepeat cr WITH (NOLOCK) 
LEFT JOIN PackingList_Detail pd  on pd.ID = cr.PackingListID and pd.CTNStartNo = cr.CTNStartNo 
WHERE       cr.MDivisionID = '{Env.User.Keyword}'
");

            if (!MyUtility.Check.Empty(this.dateReceiveDate.Value1))
            {
                sqlCmd.Append(string.Format(
                    @" 
            and cr.ReceiveDate >= '{0}'", Convert.ToDateTime(this.dateReceiveDate.Value1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.dateReceiveDate.Value2))
            {
                sqlCmd.Append(string.Format(
                    @" 
            and cr.ReceiveDate <= '{0}'", Convert.ToDateTime(this.dateReceiveDate.Value2).ToString("d")));
            }

            sqlCmd.Append(
                @"
select  1 as selected
        , ReceiveDate
        , PackingListID
        , OrderID
        , Seq
        , CTNStartNo
        , StyleID
        , BrandID
        , Customize1
        , CustPONo
        , Dest
        , FactoryID
        , BuyerDelivery
        , [SCI Delivery]
        , Qty
        , [TTQty] = isnull (TTQty, 0)
        , [Rec Qty]
        , [Ret Qty]
        , [Bal Qty] = CASE 
                          WHEN ([TTQty] - [Rec Qty] + [Ret Qty]) is null then 0 
                          else ([TTQty] - [Rec Qty] + [Ret Qty]) 
                      END
        , [%] = CASE 
                    WHEN TTQty = 0 then 0 
                    WHEN ([TTQty] - [Rec Qty] + [Ret Qty]) is null then 0 
                    ELSE round(1- (([TTQty] - [Rec Qty] + [Ret Qty]) / TTQty),2) * 100 
                END
        , ClogLocationId
        , AddDate
        , rn = ROW_NUMBER() over(order by TRY_CONVERT(int, CTNStartNo) ,(RIGHT(REPLICATE('0', 6) + rtrim(ltrim(CTNStartNo)), 6)))
        , AddName
        , RepackPackID
        , RepackOrderID
        , RepackCtnStartNo
from (
     select  t.ReceiveDate
            , [PackingListID] = iif(t.OrigID = '' OR t.OrigID IS NULL,t.PackID, t.OrigID)
            , [OrderID] = iif(t.OrigOrderID = '' OR t.OrigOrderID IS NULL,t.OrderID, t.OrigOrderID)
            , oq.Seq
            , [CTNStartNo] = iif(t.OrigCTNStartNo = '' OR t.OrigCTNStartNo IS NULL,t.CTNStartNo, t.OrigCTNStartNo)
            , isnull(o.StyleID,'') as StyleID
            , isnull(o.BrandID,'') as BrandID
            , isnull(o.Customize1,'') as Customize1
            , isnull(o.CustPONo,'') as CustPONo
            , isnull(c.Alias,'') as Dest
            , isnull(o.FactoryID,'') as FactoryID
            , o.BuyerDelivery
            , [SCI Delivery] = o.SciDelivery
            , oq.Qty
            , [TTQty] = (select sum (isnull(pld.CTNQty, 0)) 
                         from PackingList_Detail pld
                         where  pld.ID = t.PackingListID 
                                and pld.OrderID = t.OrderID)
            , [Rec Qty] = ( select count(*) 
                            from ClogReceive CReceive
                            where   CReceive.PackingListID = t.PackingListID 
                                    and CReceive.OrderID = t.OrderID)
            , [Ret Qty] = ( select count(*) 
                            from ClogReturn CReturn
                            where   CReturn.PackingListID = t.PackingListID 
                                    and CReturn.OrderID = t.OrderID)
            , t.ClogLocationId
            , t.AddDate
            , t.Id
			, AddName = (select concat(id,'-',Name) from pass1 where id = t.AddName)
            , [RepackPackID] = iif(t.OrigID != '',t.ID, t.OrigID)
            , [RepackOrderID] = iif(t.OrigOrderID != '',t.OrderID, t.OrigOrderID)
            , [RepackCtnStartNo] = iif(t.OrigCTNStartNo != '',t.CTNStartNo, t.OrigCTNStartNo)
	FROM #MainTable t
    left join Orders o WITH (NOLOCK) on t.OrderID =  o.ID
    left join Country c WITH (NOLOCK) on o.Dest = c.ID
    left join Order_QtyShip oq WITH (NOLOCK) on oq.Id = t.OrderID and oq.Seq = t.OrderShipmodeSeq
) X 
order by PackingListID, OrderID, rn

DROP TABLE  #tmp_NoRepeat,#MainTable
    ");

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out this.gridData);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Query data fail.\r\n" + result.ToString());
            }

            this.listControlBindingSource1.DataSource = this.gridData;
            this.gridReceiveDate.AutoResizeColumns();
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

            // 判斷是否有資料
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

            Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Logistic_P05.xltx"); // 預先開啟excel app
            MyUtility.Excel.CopyToXls(printDT, string.Empty, "Logistic_P05.xltx", 4, false, null, objApp); // 將datatable copy to excel
            Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表

            int r = printDT.Rows.Count;
            objSheets.get_Range(string.Format("A5:V{0}", r + 4)).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

            objSheets.Cells[2, 2] = Env.User.Keyword;
            MyUtility.Check.Seek(string.Format(@"select NameEN from Factory where id = '{0}'", Env.User.Factory), out DataRow dr, null);

            if (dr != null)
            {
                objSheets.Cells[1, 1] = dr["NameEN"].ToString() + "\r\n" + "CARTON RECEIVING REPORT";
            }
            else
            {
                objSheets.Cells[1, 1] = "CARTON RECEIVING REPORT";
            }

            string d1 = string.Empty, d2 = string.Empty;
            if (!MyUtility.Check.Empty(this.dateReceiveDate.Value1))
            {
                d1 = Convert.ToDateTime(this.dateReceiveDate.Value1).ToString("yyyy/MM/dd");
            }

            if (!MyUtility.Check.Empty(this.dateReceiveDate.Value2))
            {
                d2 = Convert.ToDateTime(this.dateReceiveDate.Value2).ToString("yyyy/MM/dd");
            }

            string drange = d1 + "~" + d2;
            objSheets.Cells[3, 13] = drange;
            objSheets.get_Range("A1").RowHeight = 45;

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("Logistic_P05");
            Excel.Workbook workbook = objApp.ActiveWorkbook;
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

        private void BtnToDRExcel_Click(object sender, EventArgs e)
        {
            this.gridReceiveDate.ValidateControl();
            this.listControlBindingSource1.EndEdit();
            DataTable excelTable = (DataTable)this.listControlBindingSource1.DataSource;

            // 判斷是否有資料
            if (excelTable == null || excelTable.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("No data!!");
                return;
            }

            DataRow[] selectData = excelTable.Select("Selected = 1");
            if (selectData.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select data first!");
                return;
            }

            string sqlCmd = $@"
select a.orderid,o.CustPONo 
,[Qty] = sum(Packing.Qty)
,[TtlCtns] = sum(TTlCtns.CTNQty)
from #tmp a
left join Orders o on a.orderid	= o.ID
outer apply(
	select sum(ShipQty)  Qty
	from PackingList_Detail p 
	where p.OrderID=a.OrderID and p.CTNStartNo=a.CTNStartNo
)Packing
outer apply(
	select p.CTNQty
	from PackingList_Detail p 
	where p.OrderID=a.OrderID and p.CTNStartNo=a.CTNStartNo
)TTlCtns
where a.Selected = 1
group by a.orderid,o.CustPONo";
            MyUtility.Tool.ProcessWithDatatable(excelTable, string.Empty, sqlCmd, out DataTable dtPrint);

            if (dtPrint == null || dtPrint.Rows.Count == 0)
            {
                MyUtility.Msg.ErrorBox("Data not found!");
                return;
            }

            Excel.Application objApp = new Excel.Application();
            Utility.Report.ExcelCOM com = new Utility.Report.ExcelCOM(Env.Cfg.XltPathDir + "\\Logistic_P05_ToDR.xltx", objApp);
            Excel.Worksheet worksheet = objApp.Sheets[1];
            com.WriteTable(dtPrint, 2);
            worksheet.get_Range($"A2:D{MyUtility.Convert.GetString(1 + dtPrint.Rows.Count)}").Borders.LineStyle = Excel.XlLineStyle.xlContinuous; // 畫線
            com.ExcelApp.ActiveWorkbook.Sheets[1].Select(Type.Missing);
            objApp.Visible = true;
            Marshal.ReleaseComObject(worksheet);
            Marshal.ReleaseComObject(objApp);
        }
    }
}
