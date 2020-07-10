using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;
using System.Runtime.InteropServices;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// P02
    /// </summary>
    public partial class P02 : Win.Tems.QueryForm
    {
        private DataGridViewGeneratorNumericColumnSettings oriqty = new DataGridViewGeneratorNumericColumnSettings();
        private DataGridViewGeneratorNumericColumnSettings newqty = new DataGridViewGeneratorNumericColumnSettings();
        private DataTable gridData;

        /// <summary>
        /// P02
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public P02(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            DataTable dtFactory;
            DualResult cbResult;
            if (cbResult = DBProxy.Current.Select(null, string.Format("select ID from Factory WITH (NOLOCK) where MDivisionID = '{0}'", Env.User.Keyword), out dtFactory))
            {
                MyUtility.Tool.SetupCombox(this.comboFactory, 1, dtFactory);
            }

            dtFactory.Rows.Add(new string[] { string.Empty });
            this.comboFactory.SelectedValue = Env.User.Keyword;

            DataRow drOC;
            if (MyUtility.Check.Seek(
                string.Format(
                @"select top 1 UpdateDate 
from OrderComparisonList WITH (NOLOCK) 
where MDivisionID = '{0}' 
and UpdateDate = (select max(UpdateDate) from OrderComparisonList WITH (NOLOCK) where MDivisionID = '{0}')", Env.User.Keyword), out drOC))
            {
                this.dateUpdatedDate.Value = Convert.ToDateTime(drOC["UpdateDate"]);
            }
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            // Grid設定
            this.gridUpdateOrder.IsEditingReadOnly = true;
            this.gridUpdateOrder.RowHeadersVisible = true;
            this.gridUpdateOrder.DataSource = this.listControlBindingSource1;
            this.gridUpdateOrder.Font = new Font("Microsoft Sans Serif", 10F);

            // 當欄位值為0時，顯示空白
            this.oriqty.CellZeroStyle = Ict.Win.UI.DataGridViewNumericBoxZeroStyle.Empty;
            this.newqty.CellZeroStyle = Ict.Win.UI.DataGridViewNumericBoxZeroStyle.Empty;

            this.Helper.Controls.Grid.Generator(this.gridUpdateOrder)
                .Text("FactoryID", header: "Factory", width: Widths.AnsiChars(5))
                .Text("OrderID", header: "SP#", width: Widths.AnsiChars(14))
                .Text("OriginalStyleID", header: "Style", width: Widths.AnsiChars(14))
                .Text("OriginalCustPONo", header: "PO No.", width: Widths.AnsiChars(14))
                .Numeric("OriginalQty", header: "Order\r\nQty", settings: this.oriqty, width: Widths.AnsiChars(5))
                .Text("OriginalBuyerDelivery", header: "Buyer\r\nDel", width: Widths.AnsiChars(5))
                .Text("OriginalSCIDelivery", header: "SCI\r\nDel", width: Widths.AnsiChars(5))
                .Text("OriginalLETA", header: "SCHD L/ETA\r\n(Master SP)")
                .Text("OriginalShipModeList", header: "Ship Mode", width: Widths.AnsiChars(10))
                .Text("KPILETA", header: "KPI\r\nL/ETA", width: Widths.AnsiChars(5))
                .Text(string.Empty, header: string.Empty, width: Widths.AnsiChars(0))
                .Text("TransferToFactory", header: "Transfer to", width: Widths.AnsiChars(8))
                .Text("NewCustPONo", header: "PO No.", width: Widths.AnsiChars(14))
                .Numeric("NewQty", header: "Order\r\nQty", settings: this.newqty, width: Widths.AnsiChars(5))
                .Text("NewBuyerDelivery", header: "Buyer\r\nDel", width: Widths.AnsiChars(5))
                .Text("NewSCIDelivery", header: "SCI\r\nDel", width: Widths.AnsiChars(5))
                .Text("NewLETA", header: "SCHD L/ETA\r\n(Master SP)")
                .Text("NewShipModeList", header: "Ship Mode", width: Widths.AnsiChars(10))
                .Text("NewOrder", header: "New", width: Widths.AnsiChars(1))
                .Text("DeleteOrder", header: "Dele", width: Widths.AnsiChars(1))
                .Text("JunkOrder", header: "Junk", width: Widths.AnsiChars(1))
                .Text("EachConsApv", header: "Each Cons", width: Widths.AnsiChars(1))
                .Text("NewMnorder", header: "M/Notice", width: Widths.AnsiChars(1))
                .Text("NewSMnorder", header: "S/M.Notice", width: Widths.AnsiChars(1))
                .Text("MnorderApv2", header: "VAS/SHAS", width: Widths.AnsiChars(1));

            // 因為資料會有變色，所以按Grid Header不可以做排序
            for (int i = 0; i < this.gridUpdateOrder.ColumnCount; i++)
            {
                this.gridUpdateOrder.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            this.QueryDate();

            for (int i = 0; i < this.gridData.Rows.Count; i++)
            {
                if ((this.gridData.Rows[i]["OriginalQty"].ToString() != this.gridData.Rows[i]["NewQty"].ToString() && this.gridData.Rows[i]["NewQty"].ToString() == "0") ||
                    this.gridData.Rows[i]["JunkOrder"].ToString() == "V" ||
                    this.gridData.Rows[i]["DeleteOrder"].ToString() == "V")
                {
                    this.gridUpdateOrder.Rows[i].DefaultCellStyle.ForeColor = Color.Red;
                    this.gridUpdateOrder.Rows[i].DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold);
                }
            }
        }

        // Query Data
        private void QueryDate()
        {
            string factoryID = (string)this.comboFactory.SelectedValue;
            DateTime? updateDate = (DateTime?)this.dateUpdatedDate.Value;

            string sqlwhere = string.Empty;
            sqlwhere += MyUtility.Check.Empty(factoryID) ? $@" and oc.MDivisionID = '{Env.User.Keyword}'" : $@" and oc.FactoryID = '{factoryID}'";
            sqlwhere += MyUtility.Check.Empty(updateDate) ? @" and UpdateDate is null" : $@" and UpdateDate='{Convert.ToDateTime(updateDate).ToString("d")}'";
            if (!MyUtility.Check.Empty(this.txtbrand1.Text))
            {
                sqlwhere += $@" and oc.BrandID='{this.txtbrand1.Text}'";
            }

            string sqlCmd =
                $@"
select oc.FactoryID
	   , OrderId
	   , OriginalStyleID
	   , o.SeasonID
	   , o.BrandID
       , OriginalCustPONo
	   , OriginalQty
	   , OriginalBuyerDelivery = RIGHT(CONVERT(VARCHAR(20),OriginalBuyerDelivery,111),5)
	   , OriginalSCIDelivery = RIGHT(CONVERT(VARCHAR(20),OriginalSCIDelivery,111),5)
	   , OriginalLETA = RIGHT(CONVERT(VARCHAR(20),OriginalLETA,111),5)
	   , OriginalShipModeList
       , NewShipModeList
	   , KPILETA = RIGHT(CONVERT(VARCHAR(20),oc.KPILETA,111),5)
	   , TransferToFactory
       , NewCustPONo
	   , NewQty
	   , NewBuyerDelivery = RIGHT(CONVERT(VARCHAR(20),NewBuyerDelivery,111),5)
	   , NewSCIDelivery = RIGHT(CONVERT(VARCHAR(20),NewSCIDelivery,111),5)
	   , NewLETA = RIGHT(CONVERT(VARCHAR(20),NewLETA,111),5)
	   , NewOrder = IIF(NewOrder = 1, 'V','')
	   , DeleteOrder = iif(DeleteOrder=1,'V','')
	   , JunkOrder = iif(JunkOrder=1,'V','')
	   , EachConsApv = iif(NewEachConsApv is null,iif(OriginalEachConsApv is null,'','★'),'V')
	   , NewMnorder = iif(NewMnorderApv is null,'','V')
	   , NewSMnorderApv = iif(NewSMnorderApv is null,'','V')
	   , MnorderApv2 = iif(oc.MnorderApv2 is null,'','V')
	   , TransferDate
from OrderComparisonList oc WITH (NOLOCK) 
left join Orders o WITH (NOLOCK)  on oc.OrderId=o.ID
where 1=1
{sqlwhere}     
order by oc.FactoryID,oc.OrderId";

            DualResult result = DBProxy.Current.Select(null, sqlCmd, out this.gridData);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query fail!\r\n" + result.ToString());
            }

            this.listControlBindingSource1.DataSource = this.gridData;
            if (this.gridData != null && this.gridData.Rows.Count == 0)
            {
                this.dateLastDate.Value = null;
            }
            else
            {
                this.dateLastDate.Value = Convert.ToDateTime(this.gridData.Rows[0]["TransferDate"]);
            }
        }

        private void Changcolor()
        {
            for (int i = 0; i < this.gridUpdateOrder.Rows.Count; i++)
            {
                if ((this.gridData.Rows[i]["OriginalQty"].ToString() != this.gridData.Rows[i]["NewQty"].ToString() && this.gridData.Rows[i]["NewQty"].ToString() == "0") ||
                    this.gridData.Rows[i]["JunkOrder"].ToString() == "V" ||
                    this.gridData.Rows[i]["DeleteOrder"].ToString() == "V")
                {
                    this.gridUpdateOrder.Rows[i].DefaultCellStyle.ForeColor = Color.Red;
                    this.gridUpdateOrder.Rows[i].DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold);
                }
            }
        }

        // Close
        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Excel
        private void BtnExcel_Click(object sender, EventArgs e)
        {
            DataTable excelTable;
            try
            {
                MyUtility.Tool.ProcessWithDatatable((DataTable)this.listControlBindingSource1.DataSource, "FactoryID,OrderId,OriginalStyleID,SeasonID,BrandID,OriginalCustPONo,OriginalQty,OriginalBuyerDelivery,OriginalSCIDelivery,OriginalLETA,OriginalShipModeList,KPILETA,TransferToFactory,NewCustPONo,NewQty,NewBuyerDelivery,NewSCIDelivery,NewLETA,NewShipModeList,NewOrder,DeleteOrder,JunkOrder,EachConsApv,NewMnorder,NewSMnorderApv,MnorderApv2", "select * from #tmp", out excelTable);
            }
            catch (Exception ex)
            {
                MyUtility.Msg.ErrorBox("To Excel error.\r\n" + ex.ToString());
                return;
            }

            if (excelTable.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("No data.");
                return;
            }

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\PPIC_P02.xltx");
            MyUtility.Excel.CopyToXls(excelTable, string.Empty, "PPIC_P02.xltx", 3, false, string.Empty, objApp);
            objApp.Cells[2, 3] = "Last Date " + this.dateLastDate.Value.Value.ToShortDateString();
            objApp.Cells[2, 9] = "Update Date " + (this.dateUpdatedDate.Value.HasValue ? this.dateUpdatedDate.Value.Value.ToShortDateString() : null);
            int number = 3;
            for (int j = 0; j < excelTable.Rows.Count; j++)
            {
                number++;
            }

            objApp.get_Range("A" + 4, "A" + number).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].LineStyle
                = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
            objApp.get_Range("A" + 4, "A" + number).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].Weight = 2;

            objApp.get_Range("A" + 4, "A" + number).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
            objApp.get_Range("A" + 4, "A" + number).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].Weight = 2;

            objApp.get_Range("B" + 4, "B" + number).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
            objApp.get_Range("B" + 4, "B" + number).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].Weight = 2;

            objApp.get_Range("L" + 4, "L" + number).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
            objApp.get_Range("L" + 4, "L" + number).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].Weight = 2;

            objApp.get_Range("S" + 4, "S" + number).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
            objApp.get_Range("S" + 4, "S" + number).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].Weight = 2;

            objApp.get_Range("T" + 4, "T" + number).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
            objApp.get_Range("T" + 4, "T" + number).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].Weight = 2;

            objApp.get_Range("U" + 4, "U" + number).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
            objApp.get_Range("U" + 4, "U" + number).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].Weight = 2;

            objApp.get_Range("V" + 4, "V" + number).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
            objApp.get_Range("V" + 4, "V" + number).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].Weight = 2;

            objApp.get_Range("W" + 4, "W" + number).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
            objApp.get_Range("W" + 4, "W" + number).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].Weight = 2;

            objApp.get_Range("X" + 4, "X" + number).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
            objApp.get_Range("X" + 4, "X" + number).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].Weight = 2;

            objApp.get_Range("Y" + 4, "Y" + number).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
            objApp.get_Range("Y" + 4, "Y" + number).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].Weight = 2;

            objApp.get_Range("Z" + 4, "Z" + number).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
            objApp.get_Range("Z" + 4, "Z" + number).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].Weight = 2;

            objApp.get_Range("A" + number, "Z" + number).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
            objApp.get_Range("A" + number, "Z" + number).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].Weight = 2;

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("PPIC_P02");
            Microsoft.Office.Interop.Excel.Workbook workbook = objApp.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);
            Marshal.ReleaseComObject(workbook);

            strExcelName.OpenFile();
            #endregion
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            this.QueryDate();
            this.Changcolor();
        }
    }
}
