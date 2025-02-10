using Ict;
using Sci.Data;
using Sci.Win.Tools;
using System;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Word = Microsoft.Office.Interop.Word;
using Excel = Microsoft.Office.Interop.Excel;
using Sci.Production.Prg;
using Sci.Production.Class.Command;

namespace Sci.Production.Packing
{
    /// <summary>
    /// Pcking_P03_Print
    /// </summary>
    public partial class P03_Print : Win.Tems.PrintForm
    {
        private DataRow masterData;
        private int orderQty;
        private bool SP_Multiple = false;
        private string reportType;
        private string ctn1;
        private string ctn2;
        private string specialInstruction;
        private bool country;
        private string orderID;
        private DataTable printData;
        private DataTable ctnDim;
        private DataTable qtyCtn;
        private DataTable articleSizeTtlShipQty;
        private DataTable printGroupData;
        private DataTable clipData;
        private DataTable qtyBDown;
        private DataTable[] printDataA;

        /// <summary>
        /// OrderQty
        /// </summary>
        public int OrderQty
        {
            get
            {
                return this.orderQty;
            }

            set
            {
                this.orderQty = value;
            }
        }

        /// <summary>
        /// P03_Print
        /// </summary>
        /// <param name="masterData">masterData</param>
        /// <param name="orderQty">OrderQty</param>
        public P03_Print(DataRow masterData, int orderQty)
        {
            this.InitializeComponent();

            // 如果是多訂單一起裝箱就不列印
            this.SP_Multiple = MyUtility.Convert.GetInt(MyUtility.GetValue.Lookup(string.Format("select COUNT(distinct OrderID+OrderShipmodeSeq) from PackingList_Detail WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(masterData["ID"])))) > 1;
            if (this.SP_Multiple)
            {
                this.radioPackingGuideReport.Visible = false;
            }

            this.masterData = masterData;
            this.OrderQty = orderQty;
            this.radioPackingListReportFormA.Checked = true;
            this.ControlPrintFunction(false);
            this.chkCartonNo.ForeColor = System.Drawing.Color.Blue;
            MyUtility.Tool.SetupCombox(this.comboType, 2, 1, $"0,2\" * 1\",1,9cm*3 cm");
            this.comboType.SelectedIndex = 0;
        }

        private void RadioBarcodePrint_CheckedChanged(object sender, EventArgs e)
        {
            this.ControlPrintFunction(((Win.UI.RadioButton)sender).Checked);
            this.checkBoxCountry.Enabled = this.radioNewBarcodePrint.Checked;
            this.checkBoxCountry.Checked = this.radioNewBarcodePrint.Checked;
        }

        // 控制元件是否可使用
        private void ControlPrintFunction(bool isSupport)
        {
            this.IsSupportToPrint = isSupport;
            this.IsSupportToExcel = !isSupport;
            this.txtCTNStart.Enabled = isSupport;
            this.txtCTNEnd.Enabled = isSupport;
            if (!isSupport)
            {
                this.txtCTNStart.Text = string.Empty;
                this.txtCTNEnd.Text = string.Empty;
            }
        }

        /// <summary>
        /// ValidateInput驗證輸入條件
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ValidateInput()
        {
            if (this.radioHandheldMetalDetectionReport.Checked)
            {
                if (MyUtility.Check.Empty(this.txtSPNo.Text))
                {
                    MyUtility.Msg.WarningBox("Please fill in <SPNo>!");
                    return false;
                }

                string sqlcmd = $@"SELECT 1 FROM Orders WITH (NOLOCK) WHERE ID = '{this.txtSPNo.Text}'";
                if (!MyUtility.Check.Seek(sqlcmd))
                {
                    MyUtility.Msg.WarningBox($"This SPNo<{this.txtSPNo.Text}> does not exists!");
                    this.txtSPNo.Text = string.Empty;
                    return false;
                }

                sqlcmd = $@"SELECT 1 FROM PackingList_Detail WITH (NOLOCK) WHERE ID = '{this.masterData["ID"]}' AND OrderID = '{this.txtSPNo.Text}'";
                if (!MyUtility.Check.Seek(sqlcmd))
                {
                    MyUtility.Msg.WarningBox($"SPNo<{this.txtSPNo.Text}> does not belong to this PackingID<{this.masterData["ID"]}>!");
                    this.txtSPNo.Text = string.Empty;
                    return false;
                }
            }

            this.reportType = this.radioPackingListReportFormA.Checked ? "1" :
                this.radioPackingListReportFormB.Checked ? "2" :
                this.radioPackingGuideReport.Checked ? "3" :
                this.rdbtnShippingMark.Checked ? "5" :
                this.rdbtnShippingMarkToChina.Checked ? "6" :
                this.rdbtnShippingMarkToUsaInd.Checked ? "7" :
                this.radioMDform.Checked ? "8" :
                this.radioWeighingform.Checked ? "9" :
                this.rdbtnShippingMarkLLL.Checked ? "10" :
                this.radioHandheldMetalDetectionReport.Checked ? "11" :
                "4";
            this.ctn1 = this.txtCTNStart.Text;
            this.ctn2 = this.txtCTNEnd.Text;
            this.ReportResourceName = "P03_BarcodePrint.rdlc";
            this.country = this.checkBoxCountry.Checked;
            this.orderID = this.txtSPNo.Text;
            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            if (this.reportType == "1" || this.reportType == "2")
            {
                DualResult result = PublicPrg.Prgs.QueryPackingListReportData(MyUtility.Convert.GetString(this.masterData["ID"]), this.reportType, out this.printData, out this.ctnDim, out this.qtyBDown);
                return result;
            }
            else if (this.reportType == "3")
            {
                DualResult result = PublicPrg.Prgs.QueryPackingGuideReportData(MyUtility.Convert.GetString(this.masterData["ID"]), out this.printData, out this.ctnDim, out this.qtyCtn, out this.articleSizeTtlShipQty, out this.printGroupData, out this.clipData, out this.specialInstruction);
                return result;
            }

            if (this.reportType == "8")
            {
                DualResult result = PublicPrg.Prgs.QueryPackingMDform(MyUtility.Convert.GetString(this.masterData["ID"]), out this.printDataA);
                return result;
            }

            if (this.reportType == "9")
            {
                DualResult result = PublicPrg.Prgs.QueryPackingCartonWeighingForm(MyUtility.Convert.GetString(this.masterData["ID"]), out this.printDataA);
                return result;
            }

            if (this.reportType == "10")
            {
                return this.ShippingmarkLLL(out this.printDataA);
            }

            if (this.reportType == "11")
            {
                return this.HandheldMetalDetection(out this.printData);
            }

            return Ict.Result.True;
        }

        /// <summary>
        /// ToPrint
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ToPrint()
        {
            this.ValidateInput();

            this.ShowWaitMessage("Data Loading ...");
            DualResult result;
            if (this.radioNewBarcodePrint.Checked)
            {
                result = new PackingPrintBarcode().PrintBarcode(this.masterData["ID"].ToString(), this.ctn1, this.ctn2, "New", this.country);
            }
            else if (this.radioBarcodePrint.Checked)
            {
                result = new PackingPrintBarcode().PrintBarcode(this.masterData["ID"].ToString(), this.ctn1, this.ctn2);
            }
            else if (this.radioBarcodePrintOther.Checked)
            {
                result = new PackingPrintBarcode().PrintBarcodeOtherSize(this.masterData["ID"].ToString(), this.ctn1, this.ctn2);
            }
            else if (this.radioQRcodePrint.Checked)
            {
                result = new PackingPrintBarcode().PrintQRcode(this.masterData["ID"].ToString(), this.ctn1, this.ctn2, selectType: this.comboType.SelectedIndex);
            }
            else if (this.rdbtnShippingMarkToChina.Checked)
            {
                result = this.PrintShippingmark_ToChina();
            }
            else if (this.rdbtnShippingMarkToUsaInd.Checked)
            {
                result = this.PrintShippingmark_ToUsaInd();
            }
            else if (this.radioCustCTN.Checked)
            {
                result = new PackingPrintBarcode().PrintCustCTN(this.masterData["ID"].ToString(), this.ctn1, this.ctn2);
            }
            else
            {
                result = this.PrintShippingmark();
            }

            if (result == false)
            {
                MyUtility.Msg.WarningBox(result.ToString());
                return false;
            }

            this.HideWaitMessage();
            return true;
        }

        /// <summary>
        /// OnToExcel產生Excel
        /// </summary>
        /// <param name="report">report</param>
        /// <returns>bool</returns>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            this.ShowWaitMessage("Data Loading....");
            if (this.reportType == "1" || this.reportType == "2")
            {
                DataTable dt = this.masterData.Table.AsEnumerable().Where(row => row["ID"].EqualString(this.masterData["id"])).CopyToDataTable();
                DataTable dtTop1 = dt.AsEnumerable().Take(1).CopyToDataTable();
                DataSet dsPrintdata = new DataSet();
                DataSet dsctnDim = new DataSet();
                DataSet dsqtyBDown = new DataSet();

                this.printData.TableName = this.masterData["ID"].ToString();
                dsPrintdata.Tables.Add(this.printData);

                this.ctnDim.TableName = this.masterData["ID"].ToString();
                dsctnDim.Tables.Add(this.ctnDim);

                this.qtyBDown.TableName = this.masterData["ID"].ToString();
                dsqtyBDown.Tables.Add(this.qtyBDown);
                if (this.SP_Multiple)
                {
                    PublicPrg.Prgs.PackingListToExcel_PackingListReport("\\Packing_P03_PackingListReport_Multiple.xltx", dtTop1, this.reportType, dsPrintdata, dsctnDim, dsqtyBDown);
                }
                else
                {
                    PublicPrg.Prgs.PackingListToExcel_PackingListReport("\\Packing_P03_PackingListReport.xltx", dtTop1, this.reportType, dsPrintdata, dsctnDim, dsqtyBDown);
                }
            }
            else if (this.reportType == "3")
            {
                PublicPrg.Prgs.PackingListToExcel_PackingGuideReport("\\Packing_P03_PackingGuideReport.xltx", this.printData, this.ctnDim, this.qtyCtn, this.articleSizeTtlShipQty, this.printGroupData, this.clipData, this.masterData, this.OrderQty, this.specialInstruction, false);
            }

            if (this.reportType == "8")
            {
                PublicPrg.Prgs.PackingListToExcel_PackingMDFormReport("\\Packing_P03_PackingMDFormReport.xltx", this.masterData, this.printDataA);
            }

            if (this.reportType == "9")
            {
                PublicPrg.Prgs.PackingListToExcel_PackingCartonWeighingReport("\\Packing_P03_PackingCartonWeighingForm.xltx", this.masterData, this.printDataA);
            }

            if (this.reportType == "10")
            {
                this.ShippingmarkLLLReport();
            }

            if (this.reportType == "11")
            {
                this.HandheldMetalDetectionReport();
            }

            this.HideWaitMessage();
            return true;
        }

        private DualResult ShippingmarkLLL(out DataTable[] dt)
        {
            string sqlcmd = $@"
select * from(
    select pd.CTNStartno,
		o.Customize1,
		SizeCode = IIF(a.SizeCode like '%,%', a.SizeCode, SUBSTRING(a.SizeCode,1,PATINDEX('%-%',a.SizeCode) - 1)),
		Article = concat(pd.Article, '/' + sa.ArticleName),
		CTNStartNostring = concat(pd.CTNStartNo, ' OF ', p.CTNQty)
    from PackingList_Detail pd
    inner join PackingList p on p.id = pd.id
    inner join orders o on o.id = pd.orderid
	left join Style_Article sa WITH (NOLOCK) on sa.StyleUkey = o.StyleUkey and sa.Article = pd.Article
    outer apply (
	    select SizeCode=stuff((
			select concat(',', isnull(z.SizeSpec,x.SizeSpec), '-', pd2.QtyPerCTN) 
			from PackingList_Detail pd2 
			outer apply(select SizeSpec from Order_SizeSpec os where os.SizeCode = pd2.SizeCode and os.id = o.poid and os.SizeItem = 'S01')x
			outer apply(select SizeSpec from Order_SizeSpec_OrderCombo oso where oso.SizeCode = pd2.SizeCode and oso.id = o.poid and oso.OrderComboID = o.OrderComboID and SizeItem = 'S01')z
			where pd2.id = pd.id and pd2.CTNStartNo = pd.CTNStartNo
			for xml path('')
		),1,1,'')
    )a
    where pd.id = '{this.masterData["ID"]}'
		  and pd.CTNQty > 0
)a
order by RIGHT(REPLICATE('0', 8) + CTNStartno, 8)
";
            return DBProxy.Current.Select(null, sqlcmd, out dt);
        }

        private void ShippingmarkLLLReport()
        {
            string strXltName = Env.Cfg.XltPathDir + "Packing_P03_Shipping Marks LLL.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return;
            }

            DataTable dt = this.printDataA[0];
            if (dt.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found");
                return;
            }

            double pageCT = Math.Ceiling(dt.Rows.Count / 8.0);

            if (pageCT > 1)
            {
                for (int i = 0; i < pageCT; i++)
                {
                    if (i > 0)
                    {
                        Microsoft.Office.Interop.Excel.Worksheet worksheet1 = (Microsoft.Office.Interop.Excel.Worksheet)excel.ActiveWorkbook.Worksheets[1];
                        Microsoft.Office.Interop.Excel.Worksheet worksheetn = (Microsoft.Office.Interop.Excel.Worksheet)excel.ActiveWorkbook.Worksheets[i + 1];
                        worksheet1.Copy(worksheetn);
                    }
                }
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                int page = (i / 8) + 1;
                Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[page];

                int col = (int)(i % 2.0) + 1; // 左或右
                int rowCT = (((i / 2) % 4) * 5) + 1;

                worksheet.Cells[rowCT + 1, col] = dr["Customize1"];
                worksheet.Cells[rowCT + 2, col] = dr["SizeCode"];
                worksheet.Cells[rowCT + 3, col] = dr["Article"];
                worksheet.Cells[rowCT + 4, col] = dr["CTNStartNostring"];
            }

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("Packing_P03_Shipping Marks LLL");
            Microsoft.Office.Interop.Excel.Workbook workbook = excel.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(workbook);

            strExcelName.OpenFile();
            #endregion
        }

        private DualResult HandheldMetalDetection(out DataTable dt)
        {
            string sqlcmd = $@"
SELECT
     No = pld.CTNStartNo
    ,ScanEditDate = FORMAT(pld.ScanEditDate, 'yyyy/MM/dd')
    ,pld.CTNStartNo
    ,pld.Article
    ,pld.Color
    ,pld.SizeCode
    ,GarmentQty = pld.ShipQty
    ,PassedQty = pld.ShipQty
    ,FailedQty = 0
FROM PackingList_Detail pld WITH (NOLOCK)
WHERE ID = '{this.masterData["ID"]}'
AND OrderID = '{this.orderID}'
";
            return DBProxy.Current.Select(null, sqlcmd, out dt);
        }

        private void HandheldMetalDetectionReport()
        {
            // 此報表是給廠商稽核用
            this.SetCount(this.printData.Rows.Count);
            if (this.printData.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found");
                return;
            }

            var listOrderby = this.printData.AsEnumerable().OrderBy(row => MyUtility.Convert.GetString(row["No"]).Trim().PadLeft(4, '0')).ToList();
            string previousNo = null;
            foreach (var row in listOrderby)
            {
                string currentNo = MyUtility.Convert.GetString(row["No"]).Trim();

                // 若當前的 No 和上一筆相同，則將當前 No 設為空白
                if (currentNo == previousNo)
                {
                    row["No"] = string.Empty;  // 或使用 ""，依需求而定
                }

                // 更新上一筆的 No
                previousNo = currentNo;
            }

            DataTable dtOrderby = listOrderby.CopyToDataTable();
            string fileName = "Packing_P03_Handheld Metal Detection Report";
            string fileNamexltx = fileName + ".xltx";
            Excel.Application excelApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + fileNamexltx);
            if (excelApp == null)
            {
                return;
            }

            Excel.Worksheet worksheet = (Excel.Worksheet)excelApp.ActiveSheet;

            // 表頭資訊
            string sqlcmd = $@"SELECT StyleID, Customize1 FROM Orders WHERE ID = '{this.orderID}'";
            MyUtility.Check.Seek(sqlcmd, out DataRow dr);
            worksheet.Cells[3, 2] = dr["StyleID"];
            worksheet.Cells[4, 2] = dr["Customize1"];
            worksheet.Cells[5, 2] = this.orderID;
            worksheet.Cells[3, 8] = listOrderby.Select(row => MyUtility.Convert.GetString(row["CTNStartNo"])).Distinct().Count(); // Ttl Ctns
            worksheet.Cells[4, 8] = listOrderby.Sum(row => MyUtility.Convert.GetInt(row["GarmentQty"])); // Ttl Garments
            worksheet.Cells[5, 8] = "1.0"; // Sensitivity Used

            // 表身
            int headerRow = 7;
            int insertRowIndex = headerRow + 2;

            // 先增加需要幾 Row , 範本只有1列
            for (int i = 0; i < listOrderby.Count() - 1; i++)
            {
                Excel.Range insertRow = worksheet.Rows[insertRowIndex];
                insertRow.Insert(Excel.XlInsertShiftDirection.xlShiftDown, Excel.XlInsertFormatOrigin.xlFormatFromLeftOrAbove);
            }

            int ttlRow = headerRow + listOrderby.Count() + 1;
            worksheet.Cells[ttlRow, 7].Formula = $"=SUM(G{headerRow + 1}:G{ttlRow - 1})";
            worksheet.Cells[ttlRow, 8].Formula = $"=SUM(H{headerRow + 1}:H{ttlRow - 1})";

            MyUtility.Excel.CopyToXls(dtOrderby, string.Empty, fileNamexltx, headerRow, false, null, excelApp);
            worksheet.Columns[2].ColumnWidth = 20;

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName(fileName);
            Excel.Workbook workbook = excelApp.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            excelApp.Quit();
            strExcelName.OpenFile();
            #endregion
        }

        /// <inheritdoc/>
        private DualResult PrintShippingmark()
        {
            #region.
            string sqlcmd = $@"
select * from(
    select pd.CTNStartno,o.Customize1,o.CustPOno,pd.Article,a.SizeCode,
		qty=iif(b1.ct = 1,convert(nvarchar, pd.shipqty),b.qty)+' PCS',
		CountryName,
		GW=pd.GW,
		NW=pd.NW
    from PackingList_Detail pd
    inner join orders o on o.id = pd.orderid
    outer apply (
	    select SizeCode=stuff((select ('/'+isnull(z.SizeSpec,x.SizeSpec)) 
	    from PackingList_Detail pd2 
	    outer apply(select SizeSpec from Order_SizeSpec os where os.SizeCode = pd2.SizeCode and os.id = o.poid and os.SizeItem = 'S01')x
		outer apply(select SizeSpec from Order_SizeSpec_OrderCombo oso where oso.SizeCode = pd2.SizeCode and oso.id = o.poid and oso.OrderComboID = o.OrderComboID and SizeItem = 'S01')z
	    where pd2.id = pd.id and pd2.CTNStartNo = pd.CTNStartNo for xml path('')),1,1,'')
    )a
    outer apply (select ct = count(SizeCode) from PackingList_Detail pd2 where pd2.id = pd.id and pd2.CTNStartNo = pd.CTNStartNo)b1
    outer apply (
	    select qty=stuff((select concat('/',isnull(z.SizeSpec,x.SizeSpec)+'-',ShipQty) 
	    from PackingList_Detail pd2 
	    outer apply(select SizeSpec from Order_SizeSpec os where os.SizeCode = pd2.SizeCode and os.id = o.poid and os.SizeItem = 'S01')x
		outer apply(select SizeSpec from Order_SizeSpec_OrderCombo oso where oso.SizeCode = pd2.SizeCode and oso.id = o.poid and oso.OrderComboID = o.OrderComboID and SizeItem = 'S01')z
	    where pd2.id = pd.id and pd2.CTNStartNo = pd.CTNStartNo for xml path('')),1,1,'')
    )b
    outer apply	(
		SELECT [CountryName]=c.NameEN FROM Factory f
		INNER JOIN Country c On f.CountryID=c.ID
		WHERE f.ID='{Env.User.Factory}'
	)c
    where pd.id = '{this.masterData["ID"]}'
      {(this.txtCTNStart.Text.IsNullOrWhiteSpace() ? string.Empty : string.Format(" and pd.CTNStartNo >= {0}", this.txtCTNStart.Text))}
      {(this.txtCTNEnd.Text.IsNullOrWhiteSpace() ? string.Empty : string.Format(" and pd.CTNStartNo <= {0}", this.txtCTNEnd.Text))}
		  and pd.CTNQty > 0
)a
order by RIGHT(REPLICATE('0', 8) + CTNStartno, 8)
";

            DualResult result = DBProxy.Current.Select(null, sqlcmd, out this.printData);

            if (this.printData == null || this.printData.Rows.Count == 0)
            {
                return new DualResult(false, "Data not found.");
            }

            Word._Application winword = new Word.Application
            {
                FileValidation = Microsoft.Office.Core.MsoFileValidationMode.msoFileValidationSkip,
                Visible = false,
            };
            object printFile;
            Word._Document document;
            Word.Table tables = null;

            if (this.rdbtnShippingMarkKHAdidas.Checked)
            {
                printFile = Env.Cfg.XltPathDir + "\\Packing_P03_Shipping Mark_ForKHAdidas.dotx";
            }
            else
            {
                printFile = Env.Cfg.XltPathDir + "\\Packing_P03_Shipping mark.dotx";
            }

            document = winword.Documents.Add(ref printFile);
            try
            {
                document.Activate();
                Word.Tables table = document.Tables;

                #region 計算頁數
                winword.Selection.Tables[1].Select();
                winword.Selection.Copy();
                int page = this.printData.Rows.Count;
                for (int i = 1; i < page; i++)
                {
                    winword.Selection.MoveDown();
                    if (page > 1)
                    {
                        winword.Selection.InsertNewPage();
                    }

                    winword.Selection.Paste();
                }
                #endregion
                #region 填入資料
                for (int i = 0; i < page; i++)
                {
                    tables = table[i + 1];

                    #region
                    string customize1 = this.printData.Rows[i]["Customize1"].ToString();
                    string custPOno = this.printData.Rows[i]["CustPOno"].ToString();
                    string article = this.printData.Rows[i]["Article"].ToString();
                    string sizeCode = this.printData.Rows[i]["SizeCode"].ToString();
                    string qty = this.printData.Rows[i]["qty"].ToString();
                    string country = this.printData.Rows[i]["CountryName"].ToString();
                    string gw = this.printData.Rows[i]["gw"].ToString();
                    string nw = this.printData.Rows[i]["nw"].ToString();
                    string cTNStartno = this.printData.Rows[i]["CTNStartno"].ToString();
                    #endregion

                    if (this.rdbtnShippingMarkKHAdidas.Checked)
                    {
                        tables.Cell(1, 2).Range.Text = custPOno;
                        tables.Cell(1, 3).Range.Text = cTNStartno;
                        tables.Cell(2, 2).Range.Text = qty.Replace("PCS", string.Empty);
                        tables.Cell(3, 2).Range.Text = country;
                        tables.Cell(4, 1).Range.Text = "GROSS WEIGHT:" + gw + "K.G.";
                        tables.Cell(5, 1).Range.Text = "NET WEIGHT:" + nw + "K.G.";
                    }
                    else
                    {
                        tables.Cell(1, 2).Range.Text = customize1;
                        tables.Cell(1, 3).Range.Text = cTNStartno;
                        tables.Cell(2, 2).Range.Text = custPOno;
                        tables.Cell(3, 2).Range.Text = article;
                        tables.Cell(4, 2).Range.Text = sizeCode;
                        tables.Cell(5, 2).Range.Text = qty;
                        tables.Cell(6, 2).Range.Text = country;
                        tables.Cell(7, 1).Range.Text = "GROSS WEIGHT:" + gw;
                        tables.Cell(8, 1).Range.Text = "NET WEIGHT:" + nw;
                    }
                }
                #endregion

                // 關閉word保護模式
                // winword.ActiveDocument.Protect(Word.WdProtectionType.wdAllowOnlyComments, Password: "ScImIs");
                #region Save & Show Word
                winword.Visible = true;
                Marshal.ReleaseComObject(winword);
                Marshal.ReleaseComObject(document);
                Marshal.ReleaseComObject(table);
                #endregion
            }
            catch (Exception ex)
            {
                if (winword != null)
                {
                    winword.Quit();
                }

                return new DualResult(false, "Export word error.", ex);
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

            return new DualResult(true);
            #endregion
        }

        /// <inheritdoc/>
        public DualResult PrintShippingmark_ToChina()
        {
            #region PrintShippingmark_ToChina
            string sqlcmd = $@"
select * from(
    select distinct 
	[CTNQty]='   /'+Cast(c.CTNQty as varchar),
	[Carton_CTNQty]=pd.CTNStartno+'/'+Cast(c.CTNQty as varchar),
	pd.CTNStartno,--只用於排序
	o.CustPOno,
	pd.Article,
	a.SizeCode,
	qty=iif(b1.ct = 1,convert(nvarchar, pd.shipqty),b.qty)+' PCS',
	d.CountryName,
	[MEASUREMENT]=Cast(Cast(round(li.CtnLength,0) AS int)AS varchar)+'*'+Cast(Cast(round(li.CtnWidth,0) AS int)AS varchar)+'*'+Cast(Cast(round(li.CtnHeight,0) AS int)AS varchar)+' '+ li.CtnUnit,
    [Weight]=Cast(Cast(round(GW.GW,2) AS numeric(17,2))AS varchar)+'/'+Cast(Cast(round(li.CtnWeight,2) AS numeric(17,2))AS varchar)+' KG'

    from PackingList_Detail pd
    inner join orders o on o.id = pd.orderid
	INNER JOIN LocalItem li ON li.RefNo=pd.RefNo
	INNER JOIN PackingList p ON p.ID=pd.ID
    outer apply (
	    select SizeCode=stuff((select ('/'+isnull(z.SizeSpec, x.SizeSpec)) 
	    from PackingList_Detail pd2 
	    outer apply(select SizeSpec from Order_SizeSpec os where os.SizeCode = pd2.SizeCode and os.id = o.poid and os.SizeItem = 'S01')x
		outer apply(select SizeSpec from Order_SizeSpec_OrderCombo oso where oso.SizeCode = pd2.SizeCode and oso.id = o.poid and oso.OrderComboID = o.OrderComboID and SizeItem = 'S01')z
	    where pd2.id = pd.id and pd2.CTNStartNo = pd.CTNStartNo for xml path('')),1,1,'')
    )a
    outer apply (select ct = count(SizeCode) from PackingList_Detail pd2 where pd2.id = pd.id and pd2.CTNStartNo = pd.CTNStartNo)b1
    outer apply (
	    select qty=stuff((select concat('/',isnull(z.SizeSpec,x.SizeSpec)+'-',ShipQty) 
	    from PackingList_Detail pd2 
	    outer apply(select SizeSpec from Order_SizeSpec os where os.SizeCode = pd2.SizeCode and os.id = o.poid and os.SizeItem = 'S01')x
		outer apply(select SizeSpec from Order_SizeSpec_OrderCombo oso where oso.SizeCode = pd2.SizeCode and oso.id = o.poid and oso.OrderComboID = o.OrderComboID and SizeItem = 'S01')z
	    where pd2.id = pd.id and pd2.CTNStartNo = pd.CTNStartNo for xml path('')),1,1,'')
    )b
	outer apply	(

		SELECT DISTINCT [CTNQty]= pl.CTNQty FROM PackingList pl
		INNER JOIn PackingList_Detail pld on pl.ID=pd.ID AND pl.ID=pd.ID
	)c
	outer apply	(
	SELECT [CountryName]=c.NameEN FROM Factory f
	INNER JOIN Country c On f.CountryID=c.ID
	WHERE f.ID='{Env.User.Factory}'
	)d
	OUTER APPLY(
		SELECT DISTINCT GW
		FROM  PackingList_Detail
		WHERE ID=pd.ID AND CTNStartNo = pd.CTNStartNo AND GW!=0
	)GW	
    where pd.id = '{this.masterData["ID"]}'
)a
order by RIGHT(REPLICATE('0', 8) + CTNStartno, 8)
";
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out this.printData);

            if (this.printData == null || this.printData.Rows.Count == 0)
            {
                return new DualResult(false, "Data not found.");
            }

            Word._Application winword = new Word.Application
            {
                FileValidation = Microsoft.Office.Core.MsoFileValidationMode.msoFileValidationSkip,
                Visible = false,
            };
            object printFile;
            Word._Document document;
            Word.Table tables = null;

            printFile = Env.Cfg.XltPathDir + "\\Packing_P03_Shipping mark To China.dotx";
            document = winword.Documents.Add(ref printFile);
            try
            {
                document.Activate();
                Word.Tables table = document.Tables;

                #region 計算頁數
                winword.Selection.Tables[1].Select();
                winword.Selection.Copy();
                int page = this.printData.Rows.Count;
                for (int i = 1; i < page; i++)
                {
                    winword.Selection.MoveDown();
                    if (page > 1)
                    {
                        winword.Selection.InsertNewPage();
                    }

                    winword.Selection.Paste();
                }
                #endregion
                #region 填入資料
                for (int i = 0; i < page; i++)
                {
                    tables = table[i + 1];

                    #region 對應SQL選取的欄位
                    string cARTON = this.chkCartonNo.Checked ? this.printData.Rows[i]["Carton_CTNQty"].ToString() : this.printData.Rows[i]["CTNQty"].ToString();
                    string custPOno = this.printData.Rows[i]["CustPOno"].ToString();
                    string article = this.printData.Rows[i]["Article"].ToString();
                    string sizeCode = this.printData.Rows[i]["SizeCode"].ToString();
                    string qty = this.printData.Rows[i]["qty"].ToString();
                    string country = this.printData.Rows[i]["CountryName"].ToString();

                    string measurement = this.printData.Rows[i]["Measurement"].ToString();
                    string weight = this.printData.Rows[i]["Weight"].ToString();
                    #endregion

                    tables.Cell(1, 2).Range.Text = cARTON;
                    tables.Cell(2, 2).Range.Text = custPOno;
                    tables.Cell(3, 2).Range.Text = article;
                    tables.Cell(4, 2).Range.Text = sizeCode;
                    tables.Cell(5, 2).Range.Text = qty;
                    tables.Cell(6, 2).Range.Text = country;
                    tables.Cell(7, 2).Range.Text = measurement;
                    tables.Cell(8, 2).Range.Text = weight;
                }
                #endregion

                // 關閉word保護模式
                // winword.ActiveDocument.Protect(Word.WdProtectionType.wdAllowOnlyComments, Password: "ScImIs");
                #region Save & Show Word
                winword.Visible = true;
                Marshal.ReleaseComObject(winword);
                Marshal.ReleaseComObject(document);
                Marshal.ReleaseComObject(table);
                #endregion
            }
            catch (Exception ex)
            {
                if (winword != null)
                {
                    winword.Quit();
                }

                return new DualResult(false, "Export word error.", ex);
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

            return new DualResult(true);
            #endregion
        }

        /// <inheritdoc/>
        public DualResult PrintShippingmark_ToUsaInd()
        {
            #region PrintShippingmark_ToUsaInd
            string sqlcmd = $@"
select * from(
    select distinct 
	pd.CTNStartno,--只用於排序
	o.CustPOno,
	a.SizeCode,
	qty=iif(b1.ct = 1,convert(nvarchar, pd.shipqty),b.qty)+' PCS',
	d.CountryName,
	[Measurement]=Cast(Cast(round(li.CtnLength,0) AS int)AS varchar)+'*'+Cast(Cast(round(li.CtnWidth,0) AS int)AS varchar)+'*'+Cast(Cast(round(li.CtnHeight,0) AS int)AS varchar)+' '+ li.CtnUnit,

	[Weight]=Cast(Cast(round(GW.GW,3) AS numeric(17,3))AS varchar)+' KG' 

	--[CtnWeight]=Cast(round(li.CtnWeight,2) AS numeric(17,2))
    from PackingList_Detail pd
    inner join orders o on o.id = pd.orderid
	INNER JOIN LocalItem li ON li.RefNo=pd.RefNo
	INNER JOIN PackingList p ON p.ID=pd.ID
    outer apply (
	    select SizeCode=stuff((select ('/'+isnull(z.SizeSpec,x.SizeSpec)) 
	    from PackingList_Detail pd2 
	    outer apply(select SizeSpec from Order_SizeSpec os where os.SizeCode = pd2.SizeCode and os.id = o.poid and os.SizeItem = 'S01')x
		outer apply(select SizeSpec from Order_SizeSpec_OrderCombo oso where oso.SizeCode = pd2.SizeCode and oso.id = o.poid and oso.OrderComboID = o.OrderComboID and SizeItem = 'S01')z
	    where pd2.id = pd.id and pd2.CTNStartNo = pd.CTNStartNo for xml path('')),1,1,'')
    )a
    outer apply (select ct = count(SizeCode) from PackingList_Detail pd2 where pd2.id = pd.id and pd2.CTNStartNo = pd.CTNStartNo)b1
    outer apply (
	    select qty=stuff((select concat('/',isnull(z.SizeSpec,x.SizeSpec)+'-',ShipQty) 
	    from PackingList_Detail pd2 
	    outer apply(select SizeSpec from Order_SizeSpec os where os.SizeCode = pd2.SizeCode and os.id = o.poid and os.SizeItem = 'S01')x
		outer apply(select SizeSpec from Order_SizeSpec_OrderCombo oso where oso.SizeCode = pd2.SizeCode and oso.id = o.poid and oso.OrderComboID = o.OrderComboID and SizeItem = 'S01')z
	    where pd2.id = pd.id and pd2.CTNStartNo = pd.CTNStartNo for xml path('')),1,1,'')
    )b	
    outer apply	(
	    SELECT [CountryName]=c.NameEN FROM Factory f
	    INNER JOIN Country c On f.CountryID=c.ID
	    WHERE f.ID='{Env.User.Factory}'
	)d
	OUTER APPLY(
		SELECT DISTINCT GW
		FROM  PackingList_Detail
		WHERE ID=pd.ID AND CTNStartNo = pd.CTNStartNo AND GW!=0
	)GW	
    where pd.id = '{this.masterData["ID"]}'
)a
order by RIGHT(REPLICATE('0', 8) + CTNStartno, 8)
";
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out this.printData);

            if (this.printData == null || this.printData.Rows.Count == 0)
            {
                return new DualResult(false, "Data not found.");
            }

            Word._Application winword = new Word.Application
            {
                FileValidation = Microsoft.Office.Core.MsoFileValidationMode.msoFileValidationSkip,
                Visible = false,
            };
            object printFile;
            Word._Document document;
            Word.Table tables = null;

            printFile = Env.Cfg.XltPathDir + "\\Packing_P03_Shipping mark To Usa Ind.dotx";
            document = winword.Documents.Add(ref printFile);
            try
            {
                document.Activate();
                Word.Tables table = document.Tables;

                #region 計算頁數
                winword.Selection.Tables[1].Select();
                winword.Selection.Copy();
                int page = this.printData.Rows.Count;
                for (int i = 1; i < page; i++)
                {
                    winword.Selection.MoveDown();
                    if (page > 1)
                    {
                        winword.Selection.InsertNewPage();
                    }

                    winword.Selection.Paste();
                }
                #endregion
                #region 填入資料
                for (int i = 0; i < page; i++)
                {
                    tables = table[i + 1];

                    #region
                    string custPOno = this.printData.Rows[i]["CustPOno"].ToString();
                    string sizeCode = this.printData.Rows[i]["SizeCode"].ToString();
                    string qty = this.printData.Rows[i]["qty"].ToString();
                    string country = this.printData.Rows[i]["CountryName"].ToString();

                    string weight = this.printData.Rows[i]["Weight"].ToString();
                    #endregion

                    tables.Cell(1, 2).Range.Text = country;
                    tables.Cell(2, 2).Range.Text = weight;
                    tables.Cell(3, 2).Range.Text = custPOno;
                    tables.Cell(4, 2).Range.Text = qty;
                    tables.Cell(5, 2).Range.Text = sizeCode;
                }
                #endregion

                // 關閉word保護模式
                // winword.ActiveDocument.Protect(Word.WdProtectionType.wdAllowOnlyComments, Password: "ScImIs");
                #region Save & Show Word
                winword.Visible = true;
                Marshal.ReleaseComObject(winword);
                Marshal.ReleaseComObject(document);
                Marshal.ReleaseComObject(table);
                #endregion
            }
            catch (Exception ex)
            {
                if (winword != null)
                {
                    winword.Quit();
                }

                return new DualResult(false, "Export word error.", ex);
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

            return new DualResult(true);
            #endregion
        }

        private void RdbtnShippingMarkToChina_CheckedChanged(object sender, EventArgs e)
        {
            this.ControlPrintFunction(((Win.UI.RadioButton)sender).Checked);
            this.checkBoxCountry.Enabled = this.radioNewBarcodePrint.Checked;
            this.checkBoxCountry.Checked = this.radioNewBarcodePrint.Checked;
        }

        private void RdbtnShippingMarkToUsaInd_CheckedChanged(object sender, EventArgs e)
        {
            this.ControlPrintFunction(((Win.UI.RadioButton)sender).Checked);
            this.checkBoxCountry.Enabled = this.radioNewBarcodePrint.Checked;
            this.checkBoxCountry.Checked = this.radioNewBarcodePrint.Checked;
        }

        private void RadioMDform_CheckedChanged(object sender, EventArgs e)
        {
            this.ControlPrintFunction(!((Win.UI.RadioButton)sender).Checked);
            this.checkBoxCountry.Enabled = !((Win.UI.RadioButton)sender).Checked;
            this.checkBoxCountry.Checked = !((Win.UI.RadioButton)sender).Checked;
        }

        private void RadioQRcodePrint_CheckedChanged(object sender, EventArgs e)
        {
            this.ControlPrintFunction(((Win.UI.RadioButton)sender).Checked);
            this.checkBoxCountry.Enabled = this.radioNewBarcodePrint.Checked;
            this.checkBoxCountry.Checked = this.radioNewBarcodePrint.Checked;
        }

        private void RadioCustCTN_CheckedChanged(object sender, EventArgs e)
        {
            this.ControlPrintFunction(((Win.UI.RadioButton)sender).Checked);
            this.checkBoxCountry.Enabled = !this.radioCustCTN.Checked;
            this.checkBoxCountry.Checked = !this.radioCustCTN.Checked;
        }

        private void RadioBarcodePrintOther_CheckedChanged(object sender, EventArgs e)
        {
            this.ControlPrintFunction(((Win.UI.RadioButton)sender).Checked);
            this.checkBoxCountry.Enabled = this.radioBarcodePrintOther.Checked;
            this.checkBoxCountry.Checked = this.radioBarcodePrintOther.Checked;
        }

        private void RdbtnShippingMarkLLL_CheckedChanged(object sender, EventArgs e)
        {
            this.ControlPrintFunction(!((Win.UI.RadioButton)sender).Checked);
            this.checkBoxCountry.Enabled = this.radioNewBarcodePrint.Checked;
            this.checkBoxCountry.Checked = this.radioNewBarcodePrint.Checked;
        }

        private void RadioHandheldMetalDetectionReport_CheckedChanged(object sender, EventArgs e)
        {
            this.ControlPrintFunction(!((Win.UI.RadioButton)sender).Checked);
            this.txtSPNo.ReadOnly = !((Win.UI.RadioButton)sender).Checked;
            if (!((Win.UI.RadioButton)sender).Checked)
            {
                this.txtSPNo.Text = string.Empty;
            }
            else
            {
                string sqlcmd = $@"SELECT DISTINCT OrderID FROM PackingList_Detail WITH (NOLOCK) WHERE ID = '{this.masterData["ID"]}' ORDER BY OrderID";
                DBProxy.Current.Select(null, sqlcmd, out DataTable dt);

                // 只有一個 OrderID 才自動帶入
                if (dt.Rows.Count == 1)
                {
                    this.txtSPNo.Text = MyUtility.Convert.GetString(dt.Rows[0]["OrderID"]);
                }
            }
        }

        private void TxtSPNo_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sqlcmd = $@"SELECT DISTINCT OrderID FROM PackingList_Detail WITH (NOLOCK) WHERE ID = '{this.masterData["ID"]}' ORDER BY OrderID";
            SelectItem item = new SelectItem(sqlcmd, "20", this.txtSPNo.Text);
            if (item.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            this.txtSPNo.Text = item.GetSelectedString();
        }
    }
}
