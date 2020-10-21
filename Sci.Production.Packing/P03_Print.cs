using System;
using System.Data;
using Ict;
using Sci.Data;
using System.Runtime.InteropServices;
using System.Linq;
using Word = Microsoft.Office.Interop.Word;

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
            this.reportType = this.radioPackingListReportFormA.Checked ? "1" :
                this.radioPackingListReportFormB.Checked ? "2" :
                this.radioPackingGuideReport.Checked ? "3" :
                this.rdbtnShippingMark.Checked ? "5" :
                this.rdbtnShippingMarkToChina.Checked ? "6" :
                this.rdbtnShippingMarkToUsaInd.Checked ? "7" :
                this.radioMDform.Checked ? "8" :
                this.radioWeighingform.Checked ? "9" :
                "4";
            this.ctn1 = this.txtCTNStart.Text;
            this.ctn2 = this.txtCTNEnd.Text;
            this.ReportResourceName = "P03_BarcodePrint.rdlc";
            this.country = this.checkBoxCountry.Checked;
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
            else if (this.radioQRcodePrint.Checked)
            {
                result = new PackingPrintBarcode().PrintQRcode(this.masterData["ID"].ToString(), this.ctn1, this.ctn2);
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

            this.HideWaitMessage();
            return true;
        }

        /// <inheritdoc/>
        public DualResult PrintShippingmark()
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
	    select SizeCode=stuff((select ('/'+isnull(x.SizeSpec,z.SizeSpec)) 
	    from PackingList_Detail pd2 
	    outer apply(select SizeSpec from Order_SizeSpec os where os.SizeCode = pd2.SizeCode and os.id = o.poid and os.SizeItem = 'S01')x
		outer apply(select SizeSpec from Order_SizeSpec_OrderCombo oso where oso.SizeCode = pd2.SizeCode and oso.id = o.poid and oso.OrderComboID = o.OrderComboID and SizeItem = 'S01')z
	    where pd2.id = pd.id and pd2.CTNStartNo = pd.CTNStartNo for xml path('')),1,1,'')
    )a
    outer apply (select ct = count(SizeCode) from PackingList_Detail pd2 where pd2.id = pd.id and pd2.CTNStartNo = pd.CTNStartNo)b1
    outer apply (
	    select qty=stuff((select concat('/',isnull(x.SizeSpec,z.SizeSpec)+'-',ShipQty) 
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
		  and pd.CTNQty > 0
)a
order by RIGHT(REPLICATE('0', 8) + CTNStartno, 8)
";
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out this.printData);

            if (this.printData == null || this.printData.Rows.Count == 0)
            {
                return new DualResult(false, "Data not found.");
            }

            Word._Application winword = new Word.Application();
            winword.FileValidation = Microsoft.Office.Core.MsoFileValidationMode.msoFileValidationSkip;
            winword.Visible = false;
            object printFile;
            Word._Document document;
            Word.Table tables = null;

            printFile = Env.Cfg.XltPathDir + "\\Packing_P03_Shipping mark.dotx";
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
	    select SizeCode=stuff((select ('/'+isnull(x.SizeSpec,z.SizeSpec)) 
	    from PackingList_Detail pd2 
	    outer apply(select SizeSpec from Order_SizeSpec os where os.SizeCode = pd2.SizeCode and os.id = o.poid and os.SizeItem = 'S01')x
		outer apply(select SizeSpec from Order_SizeSpec_OrderCombo oso where oso.SizeCode = pd2.SizeCode and oso.id = o.poid and oso.OrderComboID = o.OrderComboID and SizeItem = 'S01')z
	    where pd2.id = pd.id and pd2.CTNStartNo = pd.CTNStartNo for xml path('')),1,1,'')
    )a
    outer apply (select ct = count(SizeCode) from PackingList_Detail pd2 where pd2.id = pd.id and pd2.CTNStartNo = pd.CTNStartNo)b1
    outer apply (
	    select qty=stuff((select concat('/',isnull(x.SizeSpec,z.SizeSpec)+'-',ShipQty) 
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

            Word._Application winword = new Word.Application();
            winword.FileValidation = Microsoft.Office.Core.MsoFileValidationMode.msoFileValidationSkip;
            winword.Visible = false;
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
	    select SizeCode=stuff((select ('/'+isnull(x.SizeSpec,z.SizeSpec)) 
	    from PackingList_Detail pd2 
	    outer apply(select SizeSpec from Order_SizeSpec os where os.SizeCode = pd2.SizeCode and os.id = o.poid and os.SizeItem = 'S01')x
		outer apply(select SizeSpec from Order_SizeSpec_OrderCombo oso where oso.SizeCode = pd2.SizeCode and oso.id = o.poid and oso.OrderComboID = o.OrderComboID and SizeItem = 'S01')z
	    where pd2.id = pd.id and pd2.CTNStartNo = pd.CTNStartNo for xml path('')),1,1,'')
    )a
    outer apply (select ct = count(SizeCode) from PackingList_Detail pd2 where pd2.id = pd.id and pd2.CTNStartNo = pd.CTNStartNo)b1
    outer apply (
	    select qty=stuff((select concat('/',isnull(x.SizeSpec,z.SizeSpec)+'-',ShipQty) 
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

            Word._Application winword = new Word.Application();
            winword.FileValidation = Microsoft.Office.Core.MsoFileValidationMode.msoFileValidationSkip;
            winword.Visible = false;
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
    }
}
