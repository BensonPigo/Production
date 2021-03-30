using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using System.Runtime.InteropServices;

namespace Sci.Production.Planning
{
    /// <summary>
    /// R15
    /// </summary>
    public partial class R15 : Win.Tems.PrintForm
    {
        private int sbyindex;
        private string category;
        private string factory;
        private string mdivision;
        private string orderby;
        private string spno1;
        private string spno2;
        private string custcd;
        private string brandid;
        private string styleId;
        private DateTime? sciDelivery1;
        private DateTime? sciDelivery2;
        private DateTime? CustRqsDate1;
        private DateTime? CustRqsDate2;
        private DateTime? BuyerDelivery1;
        private DateTime? BuyerDelivery2;
        private DateTime? CutOffDate1;
        private DateTime? CutOffDate2;
        private DateTime? planDate1;
        private DateTime? planDate2;
        private DateTime? sewingInline1;
        private DateTime? sewingInline2;
        private DataTable printData;
        private DataTable dtArtworkType;
        private StringBuilder artworktypes = new StringBuilder();
        private bool isArtwork;

        /// <summary>
        /// R15
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R15(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.txtMdivision.Text = Env.User.Keyword;
            this.txtfactory.Text = Env.User.Factory;
            MyUtility.Tool.SetupCombox(this.comboOrderBy, 2, 1, "orderid,SPNO,brandid,Brand");
            this.comboOrderBy.SelectedIndex = 0;
            this.dateBuyerDelivery.Select();
            this.dateBuyerDelivery.Value1 = DateTime.Now;
            this.dateBuyerDelivery.Value2 = DateTime.Now.AddDays(30);
            DataTable dt;
            DBProxy.Current.Select(null, "select sby = 'SP#' union all select sby = 'Acticle / Size' union all select sby = 'By SP# , Line'", out dt);
            MyUtility.Tool.SetupCombox(this.comboBox1, 1, dt);
            this.comboBox1.SelectedIndex = 0;
            this.ReportType = "SP#";
        }

        /// <summary>
        /// ValidateInput
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(this.dateSCIDelivery.Value1) &&
                MyUtility.Check.Empty(this.dateCustRQSDate.Value1) &&
                MyUtility.Check.Empty(this.dateSewingInline.Value1) &&
                MyUtility.Check.Empty(this.dateBuyerDelivery.Value1) &&
                MyUtility.Check.Empty(this.dateCutOffDate.Value1) &&
                MyUtility.Check.Empty(this.datePlanDate.Value1) &&
                !this.dateLastSewDate.HasValue &&
                (MyUtility.Check.Empty(this.txtSPNoStart.Text) || MyUtility.Check.Empty(this.txtSPNoEnd.Text)))
            {
                MyUtility.Msg.WarningBox("< SCI Delivery > \r\n< Buyer Delivery > \r\n< Sewing Inline > \r\n< Cut Off Date > \r\n< Cust RQS Date > \r\n< Plan Date > \r\n< SP# > \r\n< Last Sew. Date > \r\ncan't be empty!!");
                return false;
            }

            #region -- 擇一必輸的條件 --
            this.sciDelivery1 = this.dateSCIDelivery.Value1;
            this.sciDelivery2 = this.dateSCIDelivery.Value2;
            this.CustRqsDate1 = this.dateCustRQSDate.Value1;
            this.CustRqsDate2 = this.dateCustRQSDate.Value2;
            this.sewingInline1 = this.dateSewingInline.Value1;
            this.sewingInline2 = this.dateSewingInline.Value2;
            this.BuyerDelivery1 = this.dateBuyerDelivery.Value1;
            this.BuyerDelivery2 = this.dateBuyerDelivery.Value2;
            this.CutOffDate1 = this.dateCutOffDate.Value1;
            this.CutOffDate2 = this.dateCutOffDate.Value2;
            this.planDate1 = this.datePlanDate.Value1;
            this.planDate2 = this.datePlanDate.Value2;
            this.spno1 = this.txtSPNoStart.Text;
            this.spno2 = this.txtSPNoEnd.Text;
            #endregion
            this.brandid = this.txtbrand.Text;
            this.styleId = this.txtStyle.Text;
            this.custcd = this.txtCustCD.Text;
            this.mdivision = this.txtMdivision.Text;
            this.factory = this.txtfactory.Text;
            this.category = this.comboCategory.SelectedValue.ToString();
            this.orderby = this.comboOrderBy.SelectedValue.ToString();
            this.isArtwork = this.checkIncludeArtowkData.Checked;
            this.sbyindex = this.comboBox1.SelectedIndex;
            if (this.isArtwork)
            {
                DualResult result;
                if (!(result = DBProxy.Current.Select(string.Empty, "select id from dbo.artworktype WITH (NOLOCK) where istms=1 or isprice= 1 order by seq", out this.dtArtworkType)))
                {
                    MyUtility.Msg.WarningBox(result.ToString());
                    return false;
                }

                if (this.dtArtworkType.Rows.Count == 0)
                {
                    MyUtility.Msg.WarningBox("Artwork Type data not found, Please inform MIS to check !");
                    return false;
                }

                this.artworktypes.Clear();
                for (int i = 0; i < this.dtArtworkType.Rows.Count; i++)
                {
                    this.artworktypes.Append(string.Format(@"[{0}],", this.dtArtworkType.Rows[i]["id"].ToString()));
                }
            }

            return base.ValidateInput();
        }

        /// <summary>
        /// OnAsyncDataLoad
        /// </summary>
        /// <param name="e">e</param>
        /// <returns>DualResult</returns>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
            StringBuilder sqlCmd = new StringBuilder();
            if (this.sbyindex == 0)
            {
                sqlCmd = this.SummaryBySP(out cmds);
            }
            else
            {
                if (this.sbyindex == 1)
                {
                    sqlCmd = this.SummaryByActicleSize(false, out cmds);
                }
                else
                {
                    sqlCmd = this.SummaryByActicleSize(true, out cmds);
                    sqlCmd.Append(this.SummaryBySPLine());
                }
            }

            DBProxy.Current.DefaultTimeout = 2700;
            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), cmds, out this.printData);
            DBProxy.Current.DefaultTimeout = 300;
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return Ict.Result.True;
        }

        /// <summary>
        /// OnToExcel
        /// </summary>
        /// <param name="report">report</param>
        /// <returns>bool</returns>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.printData.Rows.Count);

            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            if (this.printData.Columns.Count > 16384)
            {
                MyUtility.Msg.WarningBox("Columns of Data is over 16,384 in excel file, please narrow down range of condition.");
                return false;
            }

            if (this.printData.Rows.Count + 1 > 1048576)
            {
                MyUtility.Msg.WarningBox("Lines of Data is over 1,048,576 in excel file, please narrow down range of condition.");
                return false;
            }

            if (this.sbyindex == 0)
            {
                if (this.isArtwork)
                {
                    Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Planning_R15_WIP.xltx"); // 預先開啟excel app
                    MyUtility.Excel.CopyToXls(this.printData, string.Empty, "Planning_R15_WIP.xltx", 1, false, null, objApp);      // 將datatable copy to excel
                    Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表

                    // 列印動態欄位的表頭
                    for (int i = 0; i < this.dtArtworkType.Rows.Count; i++)
                    {
                        objSheets.Cells[1, this.printData.Columns.Count - this.dtArtworkType.Rows.Count + i + 1] = this.dtArtworkType.Rows[i]["id"].ToString();
                    }

                    // 首列資料篩選
                    Microsoft.Office.Interop.Excel.Range firstRow = (Microsoft.Office.Interop.Excel.Range)objSheets.Rows[1];
                    firstRow.AutoFilter(1, Type.Missing, Microsoft.Office.Interop.Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);

                    objApp.Cells.EntireColumn.AutoFit();  // 自動欄寬

                    // 客製化欄位，記得設定this.IsSupportCopy = true
                    this.CreateCustomizedExcel(ref objSheets);

                    #region Save & Show Excel
                    string strExcelName = Class.MicrosoftFile.GetName("Planning_R15_WIP");
                    Microsoft.Office.Interop.Excel.Workbook workbook = objApp.ActiveWorkbook;
                    workbook.SaveAs(strExcelName);
                    workbook.Close();
                    objApp.Quit();
                    Marshal.ReleaseComObject(objApp);
                    Marshal.ReleaseComObject(objSheets);
                    Marshal.ReleaseComObject(firstRow);
                    Marshal.ReleaseComObject(workbook);

                    strExcelName.OpenFile();
                    #endregion
                }
                else
                {
                    Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Planning_R15_WIP.xltx"); // 預先開啟excel app
                    MyUtility.Excel.CopyToXls(this.printData, string.Empty, "Planning_R15_WIP.xltx", 1, false, null, objApp);      // 將datatable copy to excel
                    Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表

                    // 首列資料篩選
                    Microsoft.Office.Interop.Excel.Range firstRow = (Microsoft.Office.Interop.Excel.Range)objSheets.Rows[1];
                    firstRow.AutoFilter(1, Type.Missing, Microsoft.Office.Interop.Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);

                    objApp.Cells.EntireColumn.AutoFit();  // 自動欄寬

                    // 客製化欄位，記得設定this.IsSupportCopy = true
                    this.CreateCustomizedExcel(ref objSheets);

                    #region Save & Show Excel
                    string strExcelName = Class.MicrosoftFile.GetName("Planning_R15_WIP");
                    Microsoft.Office.Interop.Excel.Workbook workbook = objApp.ActiveWorkbook;
                    workbook.SaveAs(strExcelName);
                    workbook.Close();
                    objApp.Quit();
                    Marshal.ReleaseComObject(objApp);
                    Marshal.ReleaseComObject(objSheets);
                    Marshal.ReleaseComObject(firstRow);
                    Marshal.ReleaseComObject(workbook);

                    strExcelName.OpenFile();
                    #endregion
                }
            }
            else if (this.sbyindex == 1)
            {
                if (this.isArtwork)
                {
                    Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Planning_R15_WIP_byArticleSize.xltx"); // 預先開啟excel app
                    MyUtility.Excel.CopyToXls(this.printData, string.Empty, "Planning_R15_WIP_byArticleSize.xltx", 1, false, null, objApp);      // 將datatable copy to excel
                    Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表

                    // 列印動態欄位的表頭
                    for (int i = 0; i < this.dtArtworkType.Rows.Count; i++)
                    {
                        objSheets.Cells[1, this.printData.Columns.Count - this.dtArtworkType.Rows.Count + i + 1] = this.dtArtworkType.Rows[i]["id"].ToString();
                    }

                    // 首列資料篩選
                    Microsoft.Office.Interop.Excel.Range firstRow = (Microsoft.Office.Interop.Excel.Range)objSheets.Rows[1];
                    firstRow.AutoFilter(1, Type.Missing, Microsoft.Office.Interop.Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);
                    objApp.Cells.EntireColumn.AutoFit();  // 自動欄寬

                    // 客製化欄位，記得設定this.IsSupportCopy = true
                    this.CreateCustomizedExcel(ref objSheets);

                    #region Save & Show Excel
                    string strExcelName = Class.MicrosoftFile.GetName("Planning_R15_WIP_byArticleSize");
                    Microsoft.Office.Interop.Excel.Workbook workbook = objApp.ActiveWorkbook;
                    workbook.SaveAs(strExcelName);
                    workbook.Close();
                    objApp.Quit();
                    Marshal.ReleaseComObject(objApp);
                    Marshal.ReleaseComObject(objSheets);
                    Marshal.ReleaseComObject(firstRow);
                    Marshal.ReleaseComObject(workbook);

                    strExcelName.OpenFile();
                    #endregion
                }
                else
                {
                    Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Planning_R15_WIP_byArticleSize.xltx"); // 預先開啟excel app
                    MyUtility.Excel.CopyToXls(this.printData, string.Empty, "Planning_R15_WIP_byArticleSize.xltx", 1, false, null, objApp);      // 將datatable copy to excel
                    Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表

                    // 首列資料篩選
                    Microsoft.Office.Interop.Excel.Range firstRow = (Microsoft.Office.Interop.Excel.Range)objSheets.Rows[1];
                    firstRow.AutoFilter(1, Type.Missing, Microsoft.Office.Interop.Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);
                    objApp.Cells.EntireColumn.AutoFit();  // 自動欄寬

                    // 客製化欄位，記得設定this.IsSupportCopy = true
                    this.CreateCustomizedExcel(ref objSheets);

                    #region Save & Show Excel
                    string strExcelName = Class.MicrosoftFile.GetName("Planning_R15_WIP_byArticleSize");
                    Microsoft.Office.Interop.Excel.Workbook workbook = objApp.ActiveWorkbook;
                    workbook.SaveAs(strExcelName);
                    workbook.Close();
                    objApp.Quit();
                    Marshal.ReleaseComObject(objApp);
                    Marshal.ReleaseComObject(objSheets);
                    Marshal.ReleaseComObject(firstRow);
                    Marshal.ReleaseComObject(workbook);

                    strExcelName.OpenFile();
                    #endregion
                }
            }
            else
            {
                string filename = "Planning_R15_WIP_bySPLine";
                Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + $"{filename}.xltx");
                MyUtility.Excel.CopyToXls(this.printData, string.Empty, $"{filename}.xltx", 1, false, null, objApp);
                Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
                if (this.isArtwork)
                {
                    // 列印動態欄位的表頭
                    for (int i = 0; i < this.dtArtworkType.Rows.Count; i++)
                    {
                        objSheets.Cells[1, this.printData.Columns.Count - this.dtArtworkType.Rows.Count + i + 1] = this.dtArtworkType.Rows[i]["id"].ToString();
                    }
                }

                Microsoft.Office.Interop.Excel.Range firstRow = (Microsoft.Office.Interop.Excel.Range)objSheets.Rows[1];
                firstRow.AutoFilter(1, Type.Missing, Microsoft.Office.Interop.Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);
                objApp.Cells.EntireColumn.AutoFit();  // 自動欄寬
                // 客製化欄位，記得設定this.IsSupportCopy = true
                this.CreateCustomizedExcel(ref objSheets);
                #region Save & Show Excel
                string strExcelName = Class.MicrosoftFile.GetName(filename);
                Microsoft.Office.Interop.Excel.Workbook workbook = objApp.ActiveWorkbook;
                workbook.SaveAs(strExcelName);
                workbook.Close();
                objApp.Quit();
                Marshal.ReleaseComObject(objApp);
                Marshal.ReleaseComObject(objSheets);
                Marshal.ReleaseComObject(firstRow);
                Marshal.ReleaseComObject(workbook);

                strExcelName.OpenFile();
                #endregion
            }
            return true;
        }

        private StringBuilder SummaryBySP(out IList<System.Data.SqlClient.SqlParameter> cmds)
        {
            StringBuilder sqlCmd = new StringBuilder();
            cmds = new List<System.Data.SqlClient.SqlParameter>();
            string whereIncludeCancelOrder = this.chkIncludeCancelOrder.Checked ? string.Empty : " and o.Junk = 0 ";
            #region select orders 需要欄位
            sqlCmd.Append(string.Format($@"
                    select o.MDivisionID       , o.FactoryID  , o.SciDelivery     , O.CRDDate           , O.CFMDate       , OrderID = O.ID    
	                       , O.Dest            , O.StyleID    , O.SeasonID        , O.ProjectID         , O.Customize1    , O.BuyMonth
	                       , O.CustPONo        , O.BrandID    , O.CustCDID        , O.ProgramID         , O.CdCodeID      , O.CPU
	                       , O.Qty             , O.FOCQty     , O.PoPrice         , O.CMPPrice          , O.KPILETA       , O.LETA
	                       , O.MTLETA          , O.SewETA     , O.PackETA         , O.MTLComplete       , O.SewInLine     , O.SewOffLine
                           , O.CutInLine       , O.CutOffLine , O.Category        , O.IsForecast        , O.PulloutDate   , O.ActPulloutDate
	                       , O.SMR             , O.MRHandle   , O.MCHandle        , O.OrigBuyerDelivery , O.DoxType       , O.TotalCTN
	                       , O.FtyCTN          , O.ClogCTN    , O.VasShas         , O.TissuePaper       , O.MTLExport     , O.SewLine
	                       , O.ShipModeList    , O.PlanDate   , O.FirstProduction , O.Finished          , O.FtyGroup      , O.OrderTypeID
	                       , O.SpecialMark     , O.GFR        , O.SampleReason    , InspDate = QtyShip_InspectDate.Val     
		                   , O.MnorderApv      , O.FtyKPI	  , O.KPIChangeReason , O.StyleUkey		    , O.POID          , OrdersBuyerDelivery = o.BuyerDelivery
                           , InspResult = QtyShip_Result.Val
                           , InspHandle = QtyShip_Handle.Val
                           , O.Junk,CFACTN=isnull(o.CFACTN,0)
                           , InStartDate = Null, InEndDate = Null, OutStartDate = Null, OutEndDate = Null
                           , s.CDCodeNew
                           , sty.ProductType
                           , sty.FabricType
                           , sty.Lining
                           , sty.Gender
                           , sty.Construction
                    into #cte 
                    from dbo.Orders o WITH (NOLOCK) 
                    inner join factory f WITH (NOLOCK) on o.FactoryID= f.id and f.IsProduceFty=1
                    left join Style s on s.Ukey = o.StyleUkey
                    left join Pass1 WITH (NOLOCK) on Pass1.ID = O.InspHandle
                    OUTER APPLY(
	                    SELECT [Val]=STUFF((
		                    SELECT  DISTINCT ','+ Cast(CFAFinalInspectDate as varchar)
		                    from Order_QtyShip oqs
		                    WHERE ID = o.id
		                    FOR XML PATH('')
	                    ),1,1,'')
                    )QtyShip_InspectDate
                    OUTER APPLY(
	                    SELECT [Val]=STUFF((
		                    SELECT  DISTINCT ','+ CFAFinalInspectResult 
		                    from Order_QtyShip oqs
		                    WHERE ID = o.id AND CFAFinalInspectResult <> '' AND CFAFinalInspectResult IS NOT NULL
		                    FOR XML PATH('')
	                    ),1,1,'')
                    )QtyShip_Result
                    OUTER APPLY(
	                    SELECT [Val]=STUFF((
		                    SELECT  DISTINCT ','+ CFAFinalInspectHandle +'-'+ p.Name
		                    from Order_QtyShip oqs
		                    LEFT JOIN Pass1 p WITH (NOLOCK) ON oqs.CFAFinalInspectHandle = p.ID
		                    WHERE oqs.ID = o.id AND CFAFinalInspectHandle <> '' AND CFAFinalInspectHandle IS NOT NULL
		                    FOR XML PATH('')
	                    ),1,1,'')
                    )QtyShip_Handle
                    Outer apply (
	                    SELECT ProductType = r2.Name
		                    , FabricType = r1.Name
		                    , Lining
		                    , Gender
		                    , Construction = d1.Name
	                    FROM Style s WITH(NOLOCK)
	                    left join DropDownList d1 WITH(NOLOCK) on d1.type= 'StyleConstruction' and d1.ID = s.Construction
	                    left join Reason r1 WITH(NOLOCK) on r1.ReasonTypeID= 'Fabric_Kind' and r1.ID = s.FabricType
	                    left join Reason r2 WITH(NOLOCK) on r2.ReasonTypeID= 'Style_Apparel_Type' and r2.ID = s.ApparelType
	                    where s.Ukey = o.StyleUkey
                    )sty
                    WHERE 1=1 {whereIncludeCancelOrder} "));
            #endregion

            #region --- 條件組合  ---
            if (!MyUtility.Check.Empty(this.styleId))
            {
                sqlCmd.Append($@" and o.StyleID = '{this.styleId}' ");
            }

            if (!MyUtility.Check.Empty(this.sciDelivery1))
            {
                sqlCmd.Append(string.Format(@" and o.SciDelivery >= '{0}'", Convert.ToDateTime(this.sciDelivery1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.sciDelivery2))
            {
                sqlCmd.Append(string.Format(@" and o.SciDelivery <= '{0}'", Convert.ToDateTime(this.sciDelivery2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.sewingInline1))
            {
                // sqlCmd.Append(string.Format(@" and o.SewInLine >= '{0}'", Convert.ToDateTime(this.sewingInline1).ToString("d")));
                sqlCmd.Append($@"and ( o.SewInLine >= '{this.sewingInline1.Value.ToString("d")}' OR '{this.sewingInline1.Value.ToString("d")}' BETWEEN o.SewInLine AND o.SewOffLine )");
            }

            if (!MyUtility.Check.Empty(this.sewingInline2))
            {
                // sqlCmd.Append(string.Format(@" and o.SewInLine <= '{0}'", Convert.ToDateTime(this.sewingInline2).ToString("d")));
                sqlCmd.Append($@"and ( o.SewInLine <= '{this.sewingInline2.Value.ToString("d")}' OR '{this.sewingInline2.Value.ToString("d")}' BETWEEN o.SewInLine AND o.SewOffLine )");
            }

            if (!MyUtility.Check.Empty(this.BuyerDelivery1) && !MyUtility.Check.Empty(this.BuyerDelivery2))
            {
                sqlCmd.Append(string.Format(
                    @" 
      and exists (
            select 1 
            from Order_QtyShip s WITH (NOLOCK) 
            where s.id = o.ID
            and s.BuyerDelivery between '{0}' and '{1}'
      )",
                    Convert.ToDateTime(this.BuyerDelivery1).ToString("d"),
                    Convert.ToDateTime(this.BuyerDelivery2).ToString("d")));
            }
            else if (!MyUtility.Check.Empty(this.BuyerDelivery1))
            {
                sqlCmd.Append(string.Format(
                    @" 
      and exists (
            select 1 
            from Order_QtyShip s WITH (NOLOCK) 
            where s.id = o.ID
                  and s.BuyerDelivery >= '{0}'
      )",
                    Convert.ToDateTime(this.BuyerDelivery1).ToString("d")));
            }
            else if (!MyUtility.Check.Empty(this.BuyerDelivery2))
            {
                sqlCmd.Append(string.Format(
                    @" 
      and exists (
            select 1 
            from Order_QtyShip s WITH (NOLOCK) 
            where s.id = o.ID
                  and s.BuyerDelivery <= '{0}'
      )",
                    Convert.ToDateTime(this.BuyerDelivery2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.CustRqsDate1))
            {
                sqlCmd.Append(string.Format(@" and o.CRDDate >= '{0}'", Convert.ToDateTime(this.CustRqsDate1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.CustRqsDate2))
            {
                sqlCmd.Append(string.Format(@" and o.CRDDate <= '{0}'", Convert.ToDateTime(this.CustRqsDate2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.CutOffDate1))
            {
                sqlCmd.Append(string.Format(@" and o.SDPDate >= '{0}'", Convert.ToDateTime(this.CutOffDate1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.CutOffDate2))
            {
                sqlCmd.Append(string.Format(@" and o.SDPDate <= '{0}'", Convert.ToDateTime(this.CutOffDate2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.planDate1))
            {
                sqlCmd.Append(string.Format(@" and o.PlanDate >= '{0}'", Convert.ToDateTime(this.planDate1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.planDate2))
            {
                sqlCmd.Append(string.Format(@" and o.PlanDate <= '{0}'", Convert.ToDateTime(this.planDate2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.spno1))
            {
                sqlCmd.Append(@" and o.id >= @spno1 ");
                cmds.Add(new System.Data.SqlClient.SqlParameter("@spno1", this.spno1));
            }

            if (!MyUtility.Check.Empty(this.spno2))
            {
                sqlCmd.Append(@" and o.id <= @spno2 ");
                cmds.Add(new System.Data.SqlClient.SqlParameter("@spno2", this.spno2));
            }

            if (this.dateLastSewDate.HasValue)
            {
                sqlCmd.Append($@" and exists(select 1 from View_SewingInfoSP vsis with (nolock) where vsis.OrderID = o.ID and vsis.LastSewDate between @LastSewDateFrom and @LastSewDateTo) ");
                cmds.Add(new System.Data.SqlClient.SqlParameter("@LastSewDateFrom", this.dateLastSewDate.DateBox1.Value));
                cmds.Add(new System.Data.SqlClient.SqlParameter("@LastSewDateTo", this.dateLastSewDate.DateBox2.Value));
            }

            if (!MyUtility.Check.Empty(this.brandid))
            {
                sqlCmd.Append(string.Format(@" and o.brandid = @brandid"));
                cmds.Add(new System.Data.SqlClient.SqlParameter("@brandid", this.brandid));
            }

            if (!MyUtility.Check.Empty(this.custcd))
            {
                sqlCmd.Append(string.Format(@" and o.CustCDID = @custcd"));
                cmds.Add(new System.Data.SqlClient.SqlParameter("@custcd", this.custcd));
            }

            if (!MyUtility.Check.Empty(this.mdivision))
            {
                sqlCmd.Append(@" and o.mdivisionid = @MDivision");
                cmds.Add(new System.Data.SqlClient.SqlParameter("@MDivision", this.mdivision));
            }

            if (!MyUtility.Check.Empty(this.factory))
            {
                sqlCmd.Append(@" and o.FtyGroup = @factory");
                cmds.Add(new System.Data.SqlClient.SqlParameter("@factory", this.factory));
            }

            sqlCmd.Append($" and o.Category in ({this.category})");
            sqlCmd.Append($" and exists (select 1 from Factory where o.FactoryId = id and IsProduceFty = 1)");
            #endregion

            #region -- 有列印Artwork --
            if (this.isArtwork)
            {
                sqlCmd.Append(@"
--依取得的訂單資料取得訂單的 TMS Cost
select aa.orderid
       , bb.ArtworkTypeID
       , price_tms = iif(cc.IsTMS=1,bb.tms,bb.price)  
into #rawdata_tmscost
from #cte aa 
inner join dbo.Order_TmsCost bb WITH (NOLOCK) on bb.id = aa.orderid
inner join dbo.ArtworkType cc WITH (NOLOCK)  on cc.id = bb.ArtworkTypeID
where IsTMS =1 or IsPrice = 1
                ");

                sqlCmd.Append(string.Format(
@"
  --將取得Tms Cost做成樞紐表
  select * 
  into #tmscost_pvt
  from #rawdata_tmscost
  pivot
  (
      sum(price_tms)
      for artworktypeid in ( {0})
  )as pvt ",
this.artworktypes.ToString().Substring(0, this.artworktypes.ToString().Length - 1)));
            }
            #endregion

            string[] subprocessIDs = new string[] { "Sorting", "Loading", "Emb", "BO", "PRT", "AT", "PAD-PRT", "SubCONEMB", "HT", "SewingLine" };
            string qtyBySetPerSubprocess = PublicPrg.Prgs.QtyBySetPerSubprocess(subprocessIDs, "#cte", bySP: true, isNeedCombinBundleGroup: true, isMorethenOrderQty: "1");

            #region SummaryBy SP#
            sqlCmd.Append($@"

-- 依撈出來的order資料(cte)去找各製程的WIP
SELECT X.OrderId
	   , firstSewingDate = min(X.OutputDate) 
       , lastestSewingDate = max(X.OutputDate) 
       , QAQTY = sum(X.QAQty) 
       , AVG_QAQTY = AVG(X.QAQTY)
into #tmp_SEWOUTPUT
from (
    SELECT b.OrderId, a.OutputDate
           , QAQty = sum(a.QAQty) 
    FROM DBO.SewingOutput a WITH (NOLOCK) 
    inner join dbo.SewingOutput_Detail b WITH (NOLOCK) on b.ID = a.ID
	inner join (select distinct OrderID from #cte) t on b.OrderId = t.OrderID 
    group by b.OrderId,a.OutputDate 
) X
group by X.OrderId

SELECT c.OrderID, MIN(a.cDate) first_cut_date
into #tmp_first_cut_date
from dbo.CuttingOutput a WITH (NOLOCK) 
inner join dbo.CuttingOutput_Detail b WITH (NOLOCK) on b.id = a.id 
inner join dbo.WorkOrder_Distribute c WITH (NOLOCK) on c.WorkOrderUkey = b.WorkOrderUkey
inner join (select distinct OrderID from #cte) t on c.OrderID = t.OrderID
group by c.OrderID

select pd.OrderId, pd.ScanQty
into #tmp_PackingList_Detail
from PackingList_Detail pd
inner join #cte t on pd.OrderID = t.OrderID

select t.OrderID
       , cut_qty = (SELECT SUM(CWIP.Qty) 
                    FROM DBO.CuttingOutput_WIP CWIP WITH (NOLOCK) 
                    WHERE CWIP.OrderID = T.OrderID)
	   ,f.first_cut_date
       , sewing_output = (select MIN(isnull(tt.qaqty,0)) 
                          from dbo.style_location sl WITH (NOLOCK) 
                          left join (
                                SELECT b.ComboType
                                       , qaqty = sum(b.QAQty)  
                                FROM DBO.SewingOutput a WITH (NOLOCK) 
                                inner join dbo.SewingOutput_Detail b WITH (NOLOCK) on b.ID = a.ID
                                where b.OrderId = t.OrderID
                                group by ComboType 
                          ) tt on tt.ComboType = sl.Location
                          where sl.StyleUkey = t.StyleUkey) 
       , t.StyleUkey
       , EMBROIDERY_qty = (select qty = min(qty)  
                           from (
                                select qty = sum(b.Qty) 
                                       , c.PatternCode
                                       , c.ArtworkID 
                                from dbo.farmin a WITH (NOLOCK) 
                                inner join dbo.FarmIn_Detail b WITH (NOLOCK) on b.id = a.id 
                                right join (
                                    select distinct v.ArtworkTypeID
                                           , v.Article
                                           , v.ArtworkID
                                           , v.PatternCode 
                                   from dbo.View_Order_Artworks v  WITH (NOLOCK) 
                                   where v.ID=t.OrderID
                                ) c on c.ArtworkTypeID = a.ArtworkTypeId 
                                       and c.PatternCode = b.PatternCode 
                                       and c.ArtworkID = b.ArtworkID
                                where a.ArtworkTypeId='EMBROIDERY' 
                                      and b.Orderid = t.OrderID
                                group by c.PatternCode,c.ArtworkID
                          ) x) 
       , BONDING_qty = (select qty = min(qty)  
                        from (
                           select qty = sum(b.Qty)  
                                  , c.PatternCode
                                  , c.ArtworkID 
                           from dbo.farmin a WITH (NOLOCK) 
                           inner join dbo.FarmIn_Detail b WITH (NOLOCK) on b.id = a.id 
                           right join (
                                select distinct v.ArtworkTypeID
                                       , v.ArtworkID
                                       , v.PatternCode 
                                from dbo.View_Order_Artworks v  WITH (NOLOCK) 
                                where v.ID = t.OrderID
                           ) c on c.ArtworkTypeID = a.ArtworkTypeId 
                                  and c.PatternCode = b.PatternCode 
                                  and c.ArtworkID = b.ArtworkID
                           where a.ArtworkTypeId='BONDING' 
                                 and b.Orderid = t.OrderID
                           group by c.PatternCode, c.ArtworkID
                       ) x) 
       , PRINTING_qty = (select qty = min(qty) 
                         from (
                           select qty = sum(b.Qty) 
                                  , c.PatternCode
                                  , c.ArtworkID 
                           from dbo.farmin a WITH (NOLOCK) 
                           inner join dbo.FarmIn_Detail b WITH (NOLOCK) on b.id = a.id 
                           right join (
                                select distinct v.ArtworkTypeID
                                       , v.ArtworkID
                                       , v.PatternCode 
                                from dbo.View_Order_Artworks v  WITH (NOLOCK) 
                                where v.ID=t.OrderID
                           ) c on c.ArtworkTypeID = a.ArtworkTypeId 
                                  and c.PatternCode = b.PatternCode 
                                  and c.ArtworkID = b.ArtworkID
                           where a.ArtworkTypeId = 'PRINTING' 
                                 and b.Orderid = t.OrderID
                           group by c.PatternCode, c.ArtworkID
                         ) x) 
       , s.firstSewingDate
	   , s.lastestSewingDate
	   , s.QAQTY
	   , s.AVG_QAQTY
into #cte2 
from #cte t
left join #tmp_first_cut_date f on t.OrderID = f.OrderID
left join #tmp_SEWOUTPUT s on t.OrderID = s.OrderID 

drop table #tmp_first_cut_date,#tmp_SEWOUTPUT

select sod.OrderID ,Max(so.OutputDate) LastSewnDate
into #imp_LastSewnDate
from SewingOutput so WITH (NOLOCK) 
inner join SewingOutput_Detail sod WITH (NOLOCK) on so.ID = sod.ID
inner join #cte t on sod.OrderID = t.OrderID 
group by sod.OrderID 

{qtyBySetPerSubprocess}

select t.MDivisionID
       , t.FactoryID
       , t.SewLine
       , t.OrdersBuyerDelivery
       , t.SciDelivery
       , t.SewInLine
       , t.SewOffLine
       , IDD.val
       , t.BrandID
       , t.OrderID
       , t.POID
	   , [Cancelled] = IIF(t.Junk=1 ,'Y' ,'')  ------------------
       , Dest = Country.Alias
       , t.StyleID
       , t.OrderTypeID
       , t.ShipModeList
	   , [PartialShipping]=IIF( (SELECT COUNT(ID) FROM Order_QtyShip WHERE ID = t.OrderID) >=2 ,'Y' ,'')
       , [OrderNo] = t.Customize1
       , t.CustPONo
       , t.CustCDID
       , t.ProgramID
       , t.CdCodeID
	   , t.CDCodeNew
	   , t.ProductType
	   , t.FabricType
	   , t.Lining
	   , t.Gender
	   , t.Construction
       , t.KPILETA
       , t.LETA
       , t.MTLETA
       , t.SewETA
       , t.PackETA
       , t.CPU
	   , [TTL CPU] = t.CPU * t.Qty
	   , [CPU Closed]= t.CPU * #cte2.sewing_output
	   , [CPU bal]= t.CPU * (t.qty + t.FOCQty - #cte2.sewing_output )
       , article_list = (select article + ',' 
                         from (
                              select distinct q.Article  
                              from dbo.Order_Qty q WITH (NOLOCK) 
                              where q.ID = t.OrderID
                         ) t 
                         for xml path('')) 
       , t.Qty
       ,StandardOutput.StandardOutput
	   ,oriArtwork = ann.Artwork
	   ,AddedArtwork = EXa.Artwork
       ,Artwork.Artwork
       ,spdX.SubProcessDest
       ,EstCutDate.EstimatedCutDate
       , #cte2.first_cut_date
       , #cte2.cut_qty
       , [RFID Cut Qty] = #SORTING.OutQtyBySet
       , [RFID SewingLine In Qty] = #SewingLine.InQtyBySet
       , [RFID Loading Qty] = #loading.InQtyBySet
       , [RFID Emb Farm In Qty] = IIF(EXISTS (SELECT 1 FROM [SplitString]( Artwork.Artwork , '+')  WHERE Data IN ('EMBROIDERY')),ISNULL( #Emb.InQtyBySet ,0) ,#Emb.InQtyBySet)
       , [RFID Emb Farm Out Qty] = IIF(EXISTS (SELECT 1 FROM [SplitString]( Artwork.Artwork , '+')  WHERE Data IN ('EMBROIDERY')),ISNULL( #Emb.OutQtyBySet ,0) ,#Emb.OutQtyBySet)
       , [RFID Bond Farm In Qty] = IIF(EXISTS (SELECT 1 FROM [SplitString]( Artwork.Artwork , '+')  WHERE Data IN ('BO')),ISNULL( #BO.InQtyBySet ,0) ,#BO.InQtyBySet)	
       , [RFID Bond Farm Out Qty] = IIF(EXISTS (SELECT 1 FROM [SplitString]( Artwork.Artwork , '+')  WHERE Data IN ('BO')),ISNULL( #BO.OutQtyBySet ,0) ,#BO.OutQtyBySet)
       , [RFID Print Farm In Qty] = IIF(EXISTS (SELECT 1 FROM [SplitString]( Artwork.Artwork , '+')  WHERE Data IN ('PRINTING')),ISNULL( #prt.InQtyBySet,0) ,#prt.InQtyBySet)
       , [RFID Print Farm Out Qty] = IIF(EXISTS (SELECT 1 FROM [SplitString]( Artwork.Artwork , '+')  WHERE Data IN ('PRINTING')),ISNULL( #prt.OutQtyBySet,0) ,#prt.OutQtyBySet)
       , [RFID AT Farm In Qty] =IIF(EXISTS (SELECT 1 FROM [SplitString]( Artwork.Artwork , '+')  WHERE Data IN ('AT')),ISNULL(#AT.InQtyBySet,0) ,#AT.InQtyBySet)
       , [RFID AT Farm Out Qty] = IIF(EXISTS (SELECT 1 FROM [SplitString]( Artwork.Artwork , '+')  WHERE Data IN ('AT')),ISNULL(#AT.OutQtyBySet,0) ,#AT.OutQtyBySet)
       , [RFID Pad Print Farm In Qty] = IIF(EXISTS (SELECT 1 FROM [SplitString]( Artwork.Artwork , '+')  WHERE Data IN ('PAD PRINTING')),ISNULL(#PADPRT.InQtyBySet ,0) ,#PADPRT.InQtyBySet)
       , [RFID Pad Print Farm Out Qty] = IIF(EXISTS (SELECT 1 FROM [SplitString]( Artwork.Artwork , '+')  WHERE Data IN ('PAD PRINTING')),ISNULL( #PADPRT.OutQtyBySet,0) ,#PADPRT.OutQtyBySet)
       , [RFID Emboss Farm In Qty] = IIF(EXISTS (SELECT 1 FROM [SplitString]( Artwork.Artwork , '+')  WHERE Data IN ('EMBOSS/DEBOSS')),ISNULL( #SUBCONEMB.InQtyBySet ,0) ,#SUBCONEMB.InQtyBySet)
       , [RFID Emboss Farm Out Qty] = IIF(EXISTS (SELECT 1 FROM [SplitString]( Artwork.Artwork , '+')  WHERE Data IN ('EMBOSS/DEBOSS')),ISNULL( #SUBCONEMB.OutQtyBySet ,0) ,#SUBCONEMB.OutQtyBySet)
       , [RFID HT Farm In Qty] = IIF(EXISTS (SELECT 1 FROM [SplitString]( Artwork.Artwork , '+')  WHERE Data IN ('HEAT TRANSFER')),ISNULL( #HT.InQtyBySet ,0) ,#HT.InQtyBySet)
       , [RFID HT Farm Out Qty] = IIF(EXISTS (SELECT 1 FROM [SplitString]( Artwork.Artwork , '+')  WHERE Data IN ('HEAT TRANSFER')),ISNULL( #HT.OutQtyBySet ,0) ,#HT.OutQtyBySet)
        , SubProcessStatus=
			case when t.Junk = 1 then null 
                 when #SORTING.OutQtyBySet is null and #loading.InQtyBySet is null 
                    and #SewingLine.InQtyBySet is null
                    and #Emb.InQtyBySet is null and #Emb.OutQtyBySet is null
                    and #BO.InQtyBySet is null and #BO.OutQtyBySet  is null 
                    and #prt.InQtyBySet  is null and #prt.OutQtyBySet  is null 
                    and #AT.InQtyBySet  is null and #AT.OutQtyBySet  is null 
                    and #PADPRT.InQtyBySet is null and #PADPRT.OutQtyBySet is null
                    and #SUBCONEMB.InQtyBySet is null and #SUBCONEMB.OutQtyBySet is null
                    and #HT.InQtyBySet is null and #HT.OutQtyBySet is null                
                then null
				 when SORTINGStatus.v = 1 and loadingStatus.v = 1 --判斷有做加工段的數量=訂單qty,則為1,全部為1才為Y
					and SewingLineStatus.v = 1
					and Emb_i.v = 1 and Emb_o.v = 1
					and BO_i.v = 1 and BO_o.v = 1
					and prt_i.v = 1 and prt_o.v = 1
					and AT_i.v = 1 and AT_o.v = 1
					and PADPRT_i.v = 1 and PADPRT_o.v = 1
					and SUBCONEMB_i.v = 1 and SUBCONEMB_o.v = 1
					and HT_i.v = 1 and HT_o.v = 1
				 then 'Y'
			end
       , #cte2.EMBROIDERY_qty
       , #cte2.BONDING_qty
       , #cte2.PRINTING_qty
       , #cte2.sewing_output
       , [Balance] = t.qty + t.FOCQty - #cte2.sewing_output 
       , #cte2.firstSewingDate
	   , [Last Sewn Date] = l.LastSewnDate
       , #cte2.AVG_QAQTY
       , [Est_offline] = DATEADD(DAY
                                 , iif(isnull(#cte2.AVG_QAQTY, 0) = 0, 0
                                                                     , ceiling((t.qty+t.FOCQty - #cte2.sewing_output) / (#cte2.AVG_QAQTY*1.0)))
                                 , #cte2.firstSewingDate) 
       , [Scanned_Qty] = PackDetail.ScanQty
       , [pack_rate] = IIF(isnull(t.TotalCTN, 0) = 0, 0
                                                    , round(t.ClogCTN / (t.TotalCTN * 1.0), 4) * 100 ) 
       , t.TotalCTN
       , FtyCtn = t.TotalCTN - t.FtyCTN
       , t.ClogCTN
       , t.CFACTN
       , t.InspDate
       , InspResult
       , [CFA Name] = InspHandle
       , t.ActPulloutDate
       , t.FtyKPI                
       , KPIChangeReason = KPIChangeReason.KPIChangeReason  
       , t.PlanDate
       , dbo.getTPEPass1(t.SMR) [SMR]
       , dbo.getTPEPass1(T.MRHandle) [Handle]
       , [PO SMR] = (select dbo.getTPEPass1(p.POSMR) 
                     from dbo.PO p WITH (NOLOCK) 
                     where p.ID = t.POID) 
       , [PO Handle] = (select dbo.getTPEPass1(p.POHandle) 
                        from dbo.PO p WITH (NOLOCK) 
                        where p.ID = t.POID)   
       , [MC Handle] = dbo.getTPEPass1(t.McHandle) 
       , t.DoxType
       , [SpecMark] = (select Name 
                       from Reason WITH (NOLOCK) 
                       where ReasonTypeID = 'Style_SpecialMark' 
                             and ID = t.SpecialMark) 
       , t.GFR
       , t.SampleReason
       , [TMS] = (select s.StdTms * t.CPU 
                  from System s WITH (NOLOCK)) 
");
            if (this.isArtwork)
            {
                sqlCmd.Append(string.Format(@",{0} ", this.artworktypes.ToString().Substring(0, this.artworktypes.ToString().Length - 1)));
            }

            sqlCmd.Append(string.Format(@" 
from #cte t 
inner join #cte2 on #cte2.OrderID = t.OrderID 
left join Country with (Nolock) on Country.id= t.Dest
left join #imp_LastSewnDate l on t.OrderID = l.OrderID"));
            if (this.isArtwork)
            {
                sqlCmd.Append(string.Format(@"  left join #tmscost_pvt on #tmscost_pvt.orderid = t.orderid "));
            }

            // KPIChangeReason
            sqlCmd.Append(@"
outer apply ( 
    select KPIChangeReason = ID + '-' + Name   
    from Reason  WITH (NOLOCK) 
    where ReasonTypeID = 'Order_BuyerDelivery' 
          and ID = t.KPIChangeReason 
          and t.KPIChangeReason != '' 
          and t.KPIChangeReason is not null 
) KPIChangeReason 
outer apply (SELECT val =  Stuff((select concat( ',',Format(oqs.IDD, 'yyyy/MM/dd'))   from Order_QtyShip oqs with (nolock) where oqs.ID = t.OrderID and oqs.IDD is not null FOR XML PATH('')),1,1,'') 
  ) IDD
left join #Sorting on #Sorting.OrderID = t.OrderID
left join #SewingLine on #SewingLine.OrderID = t.OrderID
left join #Loading on #Loading.OrderID = t.OrderID
left join #Emb on #Emb.OrderID = t.OrderID
left join #BO on #BO.OrderID = t.OrderID
left join #PRT on #PRT.OrderID = t.OrderID
left join #AT on #AT.OrderID = t.OrderID
left join #PADPRT on #PADPRT.OrderID = t.OrderID
left join #SUBCONEMB on #SUBCONEMB.OrderID = t.OrderID
left join #HT on #HT.OrderID = t.OrderID
outer apply(select v = case when #SORTING.OutQtyBySet is null or #SORTING.OutQtyBySet >= t.Qty then 1 else 0 end)SORTINGStatus--null即不用判斷此加工段 標記1, 數量=訂單數 標記1
outer apply(select v = case when #SewingLine.InQtyBySet is null or #SewingLine.InQtyBySet >= t.Qty then 1 else 0 end)SewingLineStatus
outer apply(select v = case when #loading.InQtyBySet is null or #loading.InQtyBySet >= t.Qty then 1 else 0 end)loadingStatus
outer apply(select v = case when #Emb.InQtyBySet is null or #Emb.InQtyBySet >= t.Qty then 1 else 0 end)Emb_i
outer apply(select v = case when #Emb.OutQtyBySet is null or #Emb.OutQtyBySet >= t.Qty then 1 else 0 end)Emb_o
outer apply(select v = case when #BO.InQtyBySet is null or #BO.InQtyBySet >= t.Qty then 1 else 0 end)BO_i
outer apply(select v = case when #BO.OutQtyBySet is null or #BO.OutQtyBySet >= t.Qty then 1 else 0 end)BO_o
outer apply(select v = case when #prt.InQtyBySet is null or #prt.InQtyBySet >= t.Qty then 1 else 0 end)prt_i
outer apply(select v = case when #prt.OutQtyBySet is null or #prt.OutQtyBySet >= t.Qty then 1 else 0 end)prt_o
outer apply(select v = case when #AT.InQtyBySet is null or #AT.InQtyBySet >= t.Qty then 1 else 0 end)AT_i
outer apply(select v = case when #AT.OutQtyBySet is null or #AT.OutQtyBySet >= t.Qty then 1 else 0 end)AT_o
outer apply(select v = case when #PADPRT.InQtyBySet is null or #PADPRT.InQtyBySet >= t.Qty then 1 else 0 end)PADPRT_i
outer apply(select v = case when #PADPRT.OutQtyBySet is null or #PADPRT.OutQtyBySet >= t.Qty then 1 else 0 end)PADPRT_o
outer apply(select v = case when #SUBCONEMB.InQtyBySet is null or #SUBCONEMB.InQtyBySet >= t.Qty then 1 else 0 end)SUBCONEMB_i
outer apply(select v = case when #SUBCONEMB.OutQtyBySet is null or #SUBCONEMB.OutQtyBySet >= t.Qty then 1 else 0 end)SUBCONEMB_o
outer apply(select v = case when #HT.InQtyBySet is null or #HT.InQtyBySet >= t.Qty then 1 else 0 end)HT_i
outer apply(select v = case when #HT.OutQtyBySet is null or #HT.OutQtyBySet >= t.Qty then 1 else 0 end)HT_o
outer apply(
	select StandardOutput =stuff((
		  select distinct concat(',',ComboType,':',StandardOutput)
		  from [SewingSchedule] WITH (NOLOCK) 
		  where orderid = t.OrderID 
		  for xml path('')
	  ),1,1,'')
)StandardOutput
outer apply(select PatternUkey from dbo.GetPatternUkey(t.POID,'','',t.StyleUkey,''))gp
outer apply(
	select Artwork = STUFF((
		select CONCAT('+', ArtworkTypeId)
		from(
			select distinct [ArtworkTypeId]=IIF(s1.ArtworkTypeId='',s1.ID,s1.ArtworkTypeId)
			from(
				SELECT bda1.SubprocessId
				FROM Bundle_Detail_Order bd1 WITH (NOLOCK)
				INNER JOIN Bundle_Detail_Art bda1 WITH (NOLOCK) ON bd1.BundleNo = bda1.Bundleno
				WHERE bd1.Orderid=t.OrderID
	
				EXCEPT
				select s.Data
				from(
					Select distinct Annotation = dbo.[RemoveNumericCharacters](a.Annotation)
					from Pattern_GL a WITH (NOLOCK) 
					Where a.PatternUkey = gp.PatternUkey
					and a.Annotation <> ''
				)x
				outer apply(select * from SplitString(x.Annotation ,'+'))s
			)x
			INNER JOIN Subprocess s1 WITH (NOLOCK) ON s1.ID = x.SubprocessId
		)x
		order by ArtworkTypeID
		for xml path('')
	),1,1,'')
)EXa
outer apply(
	select Artwork = stuff((
		select CONCAT('+',ArtworkTypeID)
		from(
			select distinct [ArtworkTypeId]=IIF(s1.ArtworkTypeId='',s1.ID,s1.ArtworkTypeId)
			from(
				Select distinct Annotation = dbo.[RemoveNumericCharacters](a.Annotation)
				from Pattern_GL a WITH (NOLOCK) 
				Where a.PatternUkey = gp.PatternUkey
				and a.Annotation <> ''
			)x
			outer apply(select * from SplitString(x.Annotation ,'+'))s
			INNER JOIN Subprocess s1 WITH (NOLOCK) ON s1.ID = s.Data
		)x
		order by ArtworkTypeID
		for xml path('')
	),1,1,'')
)ann
outer apply(
	select Artwork =stuff((	
		select concat('+',ArtworkTypeID)
		from(
			SELECT DISTINCT [ArtworkTypeId]=IIF(s1.ArtworkTypeId='',s1.ID,s1.ArtworkTypeId)
			FROM Bundle_Detail_Order bd1 WITH (NOLOCK)
			INNER JOIN Bundle_Detail_Art bda1 WITH (NOLOCK) ON bd1.BundleNo = bda1.Bundleno
			INNER JOIN Subprocess s1 WITH (NOLOCK) ON s1.ID=bda1.SubprocessId
			WHERE bd1.Orderid=t.OrderID
		)tmpartwork
		for xml path('')
	),1,1,'')
)Artwork
outer apply(
	select SubProcessDest = concat('Inhouse:'+stuff((
		select concat(',',ot.ArtworkTypeID)
		from order_tmscost ot WITH (NOLOCK)
		inner join artworktype WITH (NOLOCK) on ot.artworktypeid = artworktype.id 
		where ot.id = t.OrderID and ot.InhouseOSP = 'I' 
		and artworktype.isSubprocess = 1
		for xml path('')
	),1,1,'')
	,'; '+(
	select opsc=stuff((
		select concat('; ',ospA.abb+':'+ospB.spdO)
		from
		(
			select distinct abb = isnull(l.abb,'')
			from order_tmscost ot WITH (NOLOCK)
			inner join artworktype WITH (NOLOCK) on ot.artworktypeid = artworktype.id 
			left join localsupp l WITH (NOLOCK) on l.id = ot.LocalSuppID
			where ot.id = t.OrderID and ot.InhouseOSP = 'o'
			and artworktype.isSubprocess = 1
		)ospA
		outer apply(
			select spdO = stuff((
				select concat(',',ot.ArtworkTypeID) 
				from order_tmscost ot WITH (NOLOCK)
				inner join artworktype WITH (NOLOCK) on ot.artworktypeid = artworktype.id 
				left join localsupp l WITH (NOLOCK) on l.id = ot.LocalSuppID
				where ot.id = t.OrderID and ot.InhouseOSP = 'o'and isnull(l.Abb,'') = ospA.abb
			    and artworktype.isSubprocess = 1
				for xml path('')
			),1,1,'')
		)ospB
		for xml path('')
	),1,1,'')))
)spdX
outer apply(select EstimatedCutDate = min(EstCutDate) from WorkOrder wo WITH (NOLOCK) where t.POID = wo.id)EstCutDate
outer apply(
    select ScanQty = sum(pd.ScanQty)
    from #tmp_PackingList_Detail pd
    where pd.OrderId = t.OrderID
)PackDetail
");

            sqlCmd.Append(string.Format(@" order by {0}" + Environment.NewLine, this.orderby));
            sqlCmd.Append(" drop table #cte, #cte2, #tmp_PackingList_Detail, #imp_LastSewnDate;" + Environment.NewLine);
            foreach (string subprocess in subprocessIDs)
            {
                string whereSubprocess = subprocess;
                if (subprocess.Equals("PAD-PRT"))
                {
                    whereSubprocess = "PADPRT";
                }

                sqlCmd.Append(string.Format(@" drop table #QtyBySetPerSubprocess{0}, #{0};" + Environment.NewLine, whereSubprocess));
            }

            #endregion

            return sqlCmd;
        }

        private StringBuilder SummaryByActicleSize(bool byline, out IList<System.Data.SqlClient.SqlParameter> cmds)
        {
            StringBuilder sqlCmd = new StringBuilder();
            cmds = new List<System.Data.SqlClient.SqlParameter>();
            string whereIncludeCancelOrder = this.chkIncludeCancelOrder.Checked ? string.Empty : " and o.Junk = 0 ";
            #region select orders 需要欄位
            sqlCmd.Append(string.Format($@"
select o.MDivisionID       , o.FactoryID  , o.SciDelivery     , O.CRDDate           , O.CFMDate       , OrderID = O.ID    
	   , O.Dest            , O.StyleID    , O.SeasonID        , O.ProjectID         , O.Customize1    , O.BuyMonth
	   , O.CustPONo        , O.BrandID    , O.CustCDID        , O.ProgramID         , O.CdCodeID      , O.CPU
	   , Oq.Qty            , O.FOCQty     , O.PoPrice         , O.CMPPrice          , O.KPILETA       , O.LETA
	   , O.MTLETA          , O.SewETA     , O.PackETA         , O.MTLComplete       , O.SewInLine     , O.SewOffLine
       , O.CutInLine       , O.CutOffLine , O.Category        , O.IsForecast        , O.PulloutDate   , O.ActPulloutDate
	   , O.SMR             , O.MRHandle   , O.MCHandle        , O.OrigBuyerDelivery , O.DoxType       , O.TotalCTN
	   , O.FtyCTN          , O.ClogCTN    , O.VasShas         , O.TissuePaper       , O.MTLExport     , O.SewLine
	   , O.ShipModeList    , O.PlanDate   , O.FirstProduction , O.Finished          , O.FtyGroup      , O.OrderTypeID
	   , O.SpecialMark     , O.GFR        , O.SampleReason    , InspDate = QtyShip_InspectDate.Val     
       , O.MnorderApv      , O.FtyKPI	   , O.KPIChangeReason , O.StyleUkey		 , O.POID          , OrdersBuyerDelivery = o.BuyerDelivery
       , InspResult = QtyShip_Result.Val
       , InspHandle = QtyShip_Handle.Val
       , O.Junk,CFACTN=isnull(o.CFACTN,0)
	   , oq.Article,oq.SizeCode
       , InStartDate = Null, InEndDate = Null, OutStartDate = Null, OutEndDate = Null
       , s.CDCodeNew
       , sty.ProductType
       , sty.FabricType
       , sty.Lining
       , sty.Gender
       , sty.Construction
into #cte 
from dbo.Orders o WITH (NOLOCK) 
inner join factory f WITH (NOLOCK) on o.FactoryID= f.id and f.IsProduceFty=1
left join Style s on s.Ukey = o.StyleUkey
left join Order_Qty oq WITH (NOLOCK) on oq.ID = o.ID
OUTER APPLY(
	SELECT [Val]=STUFF((
		SELECT  DISTINCT ','+ Cast(CFAFinalInspectDate as varchar)
		from Order_QtyShip oqs
		WHERE ID = o.id
		FOR XML PATH('')
	),1,1,'')
)QtyShip_InspectDate
OUTER APPLY(
	SELECT [Val]=STUFF((
		SELECT  DISTINCT ','+ CFAFinalInspectResult 
		from Order_QtyShip oqs
		WHERE ID = o.id AND CFAFinalInspectResult <> '' AND CFAFinalInspectResult IS NOT NULL
		FOR XML PATH('')
	),1,1,'')
)QtyShip_Result
OUTER APPLY(
	SELECT [Val]=STUFF((
		SELECT  DISTINCT ','+ CFAFinalInspectHandle +'-'+ p.Name
		from Order_QtyShip oqs
		LEFT JOIN Pass1 p WITH (NOLOCK) ON oqs.CFAFinalInspectHandle = p.ID
		WHERE oqs.ID = o.id AND CFAFinalInspectHandle <> '' AND CFAFinalInspectHandle IS NOT NULL
		FOR XML PATH('')
	),1,1,'')
)QtyShip_Handle
OUTER APPLY(
    SELECT ProductType = r2.Name
        , FabricType = r1.Name
        , Lining
        , Gender
        , Construction = d1.Name
    FROM Style s WITH(NOLOCK)
    left join DropDownList d1 WITH(NOLOCK) on d1.type= 'StyleConstruction' and d1.ID = s.Construction
    left join Reason r1 WITH(NOLOCK) on r1.ReasonTypeID= 'Fabric_Kind' and r1.ID = s.FabricType
    left join Reason r2 WITH(NOLOCK) on r2.ReasonTypeID= 'Style_Apparel_Type' and r2.ID = s.ApparelType
    where s.Ukey = o.StyleUkey
)sty
WHERE 1=1 {whereIncludeCancelOrder} "));
            #endregion

            #region --- 條件組合  ---
            if (!MyUtility.Check.Empty(this.sciDelivery1))
            {
                sqlCmd.Append(string.Format(@" and o.SciDelivery >= '{0}'", Convert.ToDateTime(this.sciDelivery1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.sciDelivery2))
            {
                sqlCmd.Append(string.Format(@" and o.SciDelivery <= '{0}'", Convert.ToDateTime(this.sciDelivery2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.sewingInline1))
            {
                // sqlCmd.Append(string.Format(@" and o.SewInLine >= '{0}'", Convert.ToDateTime(this.sewingInline1).ToString("d")));
                sqlCmd.Append($@"and ( o.SewInLine >= '{this.sewingInline1.Value.ToString("d")}' OR '{this.sewingInline1.Value.ToString("d")}' BETWEEN o.SewInLine AND o.SewOffLine )");
            }

            if (!MyUtility.Check.Empty(this.sewingInline2))
            {
                // sqlCmd.Append(string.Format(@" and o.SewInLine <= '{0}'", Convert.ToDateTime(this.sewingInline2).ToString("d")));
                sqlCmd.Append($@"and ( o.SewInLine <= '{this.sewingInline2.Value.ToString("d")}' OR '{this.sewingInline2.Value.ToString("d")}' BETWEEN o.SewInLine AND o.SewOffLine )");
            }

            if (!MyUtility.Check.Empty(this.BuyerDelivery1) && !MyUtility.Check.Empty(this.BuyerDelivery2))
            {
                sqlCmd.Append(string.Format(
                    @" 
      and exists (
            select 1 
            from Order_QtyShip s WITH (NOLOCK) 
            where s.id = o.ID
            and s.BuyerDelivery between '{0}' and '{1}'
      )",
                    Convert.ToDateTime(this.BuyerDelivery1).ToString("d"),
                    Convert.ToDateTime(this.BuyerDelivery2).ToString("d")));
            }
            else if (!MyUtility.Check.Empty(this.BuyerDelivery1))
            {
                sqlCmd.Append(string.Format(
                    @" 
      and exists (
            select 1 
            from Order_QtyShip s WITH (NOLOCK) 
            where s.id = o.ID
                  and s.BuyerDelivery >= '{0}'
      )",
                    Convert.ToDateTime(this.BuyerDelivery1).ToString("d")));
            }
            else if (!MyUtility.Check.Empty(this.BuyerDelivery2))
            {
                sqlCmd.Append(string.Format(
                    @" 
      and exists (
            select 1 
            from Order_QtyShip s WITH (NOLOCK) 
            where s.id = o.ID
                  and s.BuyerDelivery <= '{0}'
      )",
                    Convert.ToDateTime(this.BuyerDelivery2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.CustRqsDate1))
            {
                sqlCmd.Append(string.Format(@" and o.CRDDate >= '{0}'", Convert.ToDateTime(this.CustRqsDate1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.CustRqsDate2))
            {
                sqlCmd.Append(string.Format(@" and o.CRDDate <= '{0}'", Convert.ToDateTime(this.CustRqsDate2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.CutOffDate1))
            {
                sqlCmd.Append(string.Format(@" and o.SDPDate >= '{0}'", Convert.ToDateTime(this.CutOffDate1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.CutOffDate2))
            {
                sqlCmd.Append(string.Format(@" and o.SDPDate <= '{0}'", Convert.ToDateTime(this.CutOffDate2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.planDate1))
            {
                sqlCmd.Append(string.Format(@" and o.PlanDate >= '{0}'", Convert.ToDateTime(this.planDate1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.planDate2))
            {
                sqlCmd.Append(string.Format(@" and o.PlanDate <= '{0}'", Convert.ToDateTime(this.planDate2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.spno1))
            {
                sqlCmd.Append(@" and o.id >= @spno1 ");
                cmds.Add(new System.Data.SqlClient.SqlParameter("@spno1", this.spno1));
            }

            if (!MyUtility.Check.Empty(this.spno2))
            {
                sqlCmd.Append(@" and o.id <= @spno2 ");
                cmds.Add(new System.Data.SqlClient.SqlParameter("@spno2", this.spno2));
            }

            if (!MyUtility.Check.Empty(this.brandid))
            {
                sqlCmd.Append(string.Format(@" and o.brandid = @brandid"));
                cmds.Add(new System.Data.SqlClient.SqlParameter("@brandid", this.brandid));
            }

            if (!MyUtility.Check.Empty(this.custcd))
            {
                sqlCmd.Append(string.Format(@" and o.CustCDID = @custcd"));
                cmds.Add(new System.Data.SqlClient.SqlParameter("@custcd", this.custcd));
            }

            if (!MyUtility.Check.Empty(this.mdivision))
            {
                sqlCmd.Append(@" and o.mdivisionid = @MDivision");
                cmds.Add(new System.Data.SqlClient.SqlParameter("@MDivision", this.mdivision));
            }

            if (this.dateLastSewDate.HasValue)
            {
                sqlCmd.Append($@" and exists(select 1 from View_SewingInfoSP vsis with (nolock) where vsis.OrderID = o.ID and vsis.LastSewDate between @LastSewDateFrom and @LastSewDateTo) ");
                cmds.Add(new System.Data.SqlClient.SqlParameter("@LastSewDateFrom", this.dateLastSewDate.DateBox1.Value));
                cmds.Add(new System.Data.SqlClient.SqlParameter("@LastSewDateTo", this.dateLastSewDate.DateBox2.Value));
            }

            if (!MyUtility.Check.Empty(this.factory))
            {
                sqlCmd.Append(@" and o.FtyGroup = @factory");
                cmds.Add(new System.Data.SqlClient.SqlParameter("@factory", this.factory));
            }

            sqlCmd.Append($" and o.Category in ({this.category})");
            sqlCmd.Append($" and exists (select 1 from Factory where o.FactoryId = id and IsProduceFty = 1)");
            #endregion

            #region -- 有列印Artwork --
            if (this.isArtwork)
            {
                sqlCmd.Append(@"
--依取得的訂單資料取得訂單的 TMS Cost
select aa.orderid
       , bb.ArtworkTypeID
       , price_tms = iif(cc.IsTMS=1,bb.tms,bb.price)  
into #rawdata_tmscost
from #cte aa 
inner join dbo.Order_TmsCost bb  WITH (NOLOCK) on bb.id = aa.orderid
inner join dbo.ArtworkType cc  WITH (NOLOCK) on cc.id = bb.ArtworkTypeID
where IsTMS =1 or IsPrice = 1
                ");

                sqlCmd.Append(string.Format(
                    @"
--將取得Tms Cost做成樞紐表
select * 
into #tmscost_pvt
from #rawdata_tmscost
pivot
(
    sum(price_tms)
    for artworktypeid in ( {0})
)as pvt ",
                    this.artworktypes.ToString().Substring(0, this.artworktypes.ToString().Length - 1)));
            }
            #endregion
            string[] subprocessIDs = new string[] { "Sorting", "Loading", "Emb", "BO", "PRT", "AT", "PAD-PRT", "SubCONEMB", "HT", "SewingLine" };
            string qtyBySetPerSubprocess = PublicPrg.Prgs.QtyBySetPerSubprocess(subprocessIDs, "#cte", bySP: false, isNeedCombinBundleGroup: true, isMorethenOrderQty: "1");
            #region SummaryBy Acticle/Size
            sqlCmd.Append($@"
-- 依撈出來的order資料(cte)去找各製程的WIP
SELECT OrderId,Article,SizeCode
		, firstSewingDate = min(X.OutputDate) 
        , lastestSewingDate = max(X.OutputDate) 
        , QAQTY = sum(X.QAQty) 
        , AVG_QAQTY = AVG(X.QAQTY)
into #tmp_SEWOUTPUT
from (
     SELECT b.OrderId,a.OutputDate,c.Article,c.SizeCode
            , QAQty = sum(c.QAQty)
     FROM DBO.SewingOutput a WITH (NOLOCK) 
     inner join dbo.SewingOutput_Detail b WITH (NOLOCK) on b.ID = a.ID
	 inner join SewingOutput_Detail_Detail c WITH (NOLOCK) on c.SewingOutput_DetailUKey = b.UKey
	 inner join (select distinct OrderID,Article,SizeCode from #cte) t on b.OrderId = t.OrderID and c.Article = t.Article and c.SizeCode = t.SizeCode 
     group by b.OrderId,a.OutputDate ,c.Article,c.SizeCode
) X 
group by OrderId,Article,SizeCode

select c.OrderID
	,c.Article
	,c.SizeCode
	,first_cut_date = MIN(a.cDate)
into #tmp_first_cut_date
from dbo.CuttingOutput a WITH (NOLOCK) 
inner join dbo.CuttingOutput_Detail b WITH (NOLOCK) on b.id = a.id 
inner join dbo.WorkOrder_Distribute c WITH (NOLOCK) on c.WorkOrderUkey = b.WorkOrderUkey
inner join (select distinct OrderID,Article,SizeCode from #cte) t on c.OrderID = t.OrderID and c.Article = t.Article and c.SizeCode = t.SizeCode
group by c.OrderID,c.Article,c.SizeCode  

select pd.OrderId,  pd.Article, pd.SizeCode, pd.ScanQty
into #tmp_PackingList_Detail
from PackingList_Detail pd
inner join #cte t on pd.OrderId = t.OrderID and pd.Article = t.Article and pd.SizeCode = t.SizeCode

select t.OrderID,t.Article,t.SizeCode
       , cut_qty = (SELECT SUM(CWIP.Qty) 
                    FROM DBO.CuttingOutput_WIP CWIP WITH (NOLOCK) 
                    WHERE CWIP.OrderID = T.OrderID and CWIP.Article = t.Article and CWIP.Size = t.SizeCode)
	   , f.first_cut_date
       , sewing_output = (select MIN(isnull(tt.qaqty,0)) 
                          from dbo.style_location sl WITH (NOLOCK) 
                          left join (
                                SELECT c.ComboType
                                       , qaqty = sum(c.QAQty)  
                                FROM DBO.SewingOutput a WITH (NOLOCK) 
                                inner join dbo.SewingOutput_Detail b WITH (NOLOCK) on b.ID = a.ID
								inner join SewingOutput_Detail_Detail c WITH (NOLOCK) on c.SewingOutput_DetailUKey = b.UKey
								where b.OrderId = t.OrderID and c.Article = t.Article and c.SizeCode = t.SizeCode
                                group by c.ComboType 
                          ) tt on tt.ComboType = sl.Location
                          where sl.StyleUkey = t.StyleUkey) 
       , t.StyleUkey
       , EMBROIDERY_qty = (select qty = min(qty)  
                           from (
                                select qty = sum(b.Qty) 
                                       , c.PatternCode
                                       , c.ArtworkID ,c.Article,c.SizeCode
                                from dbo.farmin a WITH (NOLOCK) 
                                inner join dbo.FarmIn_Detail b WITH (NOLOCK) on b.id = a.id 
                                right join (
                                    select distinct v.ArtworkTypeID
                                           , v.Article
										   , v.SizeCode
                                           , v.ArtworkID
                                           , v.PatternCode 
                                   from dbo.View_Order_Artworks v  WITH (NOLOCK) 
                                   where v.ID=t.OrderID and v.Article = t.Article and v.SizeCode = t.SizeCode
                                ) c on c.ArtworkTypeID = a.ArtworkTypeId 
                                       and c.PatternCode = b.PatternCode 
                                       and c.ArtworkID = b.ArtworkID
                                where a.ArtworkTypeId='EMBROIDERY'
                                      and b.Orderid = t.OrderID
                                group by c.PatternCode,c.ArtworkID,c.Article,c.SizeCode
                          ) x) 
       , BONDING_qty = (select qty = min(qty)  
                        from (
                           select qty = sum(b.Qty)  
                                  , c.PatternCode
                                  , c.ArtworkID ,c.Article,c.SizeCode
                           from dbo.farmin a WITH (NOLOCK) 
                           inner join dbo.FarmIn_Detail b WITH (NOLOCK) on b.id = a.id 
                           right join (
                                select distinct v.ArtworkTypeID, v.Article, v.SizeCode
                                       , v.ArtworkID
                                       , v.PatternCode 
                                from dbo.View_Order_Artworks v  WITH (NOLOCK) 
                                where v.ID=t.OrderID and v.Article = t.Article and v.SizeCode = t.SizeCode
                           ) c on c.ArtworkTypeID = a.ArtworkTypeId 
                                  and c.PatternCode = b.PatternCode 
                                  and c.ArtworkID = b.ArtworkID
                           where a.ArtworkTypeId='BONDING' 
                                 and b.Orderid = t.OrderID
                           group by c.PatternCode,c.ArtworkID,c.Article,c.SizeCode
                       ) x) 
       , PRINTING_qty = (select qty = min(qty) 
                         from (
                           select qty = sum(b.Qty) 
                                  , c.PatternCode
                                  , c.ArtworkID ,c.Article,c.SizeCode
                           from dbo.farmin a WITH (NOLOCK) 
                           inner join dbo.FarmIn_Detail b WITH (NOLOCK) on b.id = a.id 
                           right join (
                                select distinct v.ArtworkTypeID, v.Article, v.SizeCode
                                       , v.ArtworkID
                                       , v.PatternCode 
                                from dbo.View_Order_Artworks v  WITH (NOLOCK) 
                                where v.ID=t.OrderID and v.Article = t.Article and v.SizeCode = t.SizeCode
                           ) c on c.ArtworkTypeID = a.ArtworkTypeId 
                                  and c.PatternCode = b.PatternCode 
                                  and c.ArtworkID = b.ArtworkID
                           where a.ArtworkTypeId = 'PRINTING' 
                                 and b.Orderid = t.OrderID
                           group by c.PatternCode,c.ArtworkID,c.Article,c.SizeCode
                         ) x)
       , s.firstSewingDate
	   , s.lastestSewingDate
	   , s.QAQTY
	   , s.AVG_QAQTY
into #cte2 
from #cte t
left join #tmp_first_cut_date f on f.OrderId = t.OrderID and f.Article = t.Article and f.SizeCode = t.SizeCode 
left join #tmp_SEWOUTPUT s on s.OrderId = t.OrderID and s.Article = t.Article and s.SizeCode = t.SizeCode 

drop table #tmp_first_cut_date, #tmp_SEWOUTPUT

{qtyBySetPerSubprocess}

select t.MDivisionID
       , t.FactoryID
       , SewingSchedule.SewingLineID
       , t.OrdersBuyerDelivery
       , t.SciDelivery
       , SewingSchedule2.Inline
       , SewingSchedule2.Offline
       , IDD.val
       , t.BrandID
       , t.OrderID
       , t.POID
	   , [Cancelled] = IIF(t.Junk=1 ,'Y' ,'')  ------------------
       , Dest = Country.Alias
       , t.StyleID
       , t.OrderTypeID
       , t.ShipModeList
	   , [PartialShipping]=IIF( (SELECT COUNT(ID) FROM Order_QtyShip WHERE ID = t.OrderID) >=2 ,'Y' ,'')
       , [OrderNo] = t.Customize1
       , t.CustPONo
       , t.CustCDID
       , t.ProgramID
       , t.CdCodeID
	   , t.CDCodeNew
	   , t.ProductType
	   , t.FabricType
	   , t.Lining
	   , t.Gender
	   , t.Construction
       , t.KPILETA
       , t.LETA
       , t.MTLETA
       , t.SewETA
       , t.PackETA
       , t.CPU
	   , [TTL CPU] = t.CPU * t.Qty
	   , [CPU Closed]= t.CPU * #cte2.sewing_output
	   , [CPU bal]= t.CPU * (t.qty + t.FOCQty - #cte2.sewing_output)
       , t.Article
	   , t.SizeCode
-----------------------------------------------------------------------------------------------------------------------------------------
       , t.Qty
       ,StandardOutput.StandardOutput
	   ,oriArtwork = ann.Artwork
	   ,AddedArtwork = EXa.Artwork
       ,Artwork.Artwork
       ,spdX.SubProcessDest
       ,EstCutDate.EstimatedCutDate
       , #cte2.first_cut_date
       , #cte2.cut_qty
       , [RFID Cut Qty] = SORTING.OutQtyBySet
       , [RFID SewingLine In Qty] = SewingLine.InQtyBySet
       , [RFID Loading Qty] = loading.InQtyBySet
       , [RFID Emb Farm In Qty] = IIF(EXISTS (SELECT 1 FROM [SplitString]( Artwork.Artwork , '+')  WHERE Data IN ('EMBROIDERY')),ISNULL( Emb.InQtyBySet ,0) ,Emb.InQtyBySet)
       , [RFID Emb Farm Out Qty] = IIF(EXISTS (SELECT 1 FROM [SplitString]( Artwork.Artwork , '+')  WHERE Data IN ('EMBROIDERY')),ISNULL( Emb.OutQtyBySet ,0) ,Emb.OutQtyBySet)
       , [RFID Bond Farm In Qty] = IIF(EXISTS (SELECT 1 FROM [SplitString]( Artwork.Artwork , '+')  WHERE Data IN ('BO')),ISNULL( BO.InQtyBySet ,0) ,BO.InQtyBySet)	
       , [RFID Bond Farm Out Qty] = IIF(EXISTS (SELECT 1 FROM [SplitString]( Artwork.Artwork , '+')  WHERE Data IN ('BO')),ISNULL( BO.OutQtyBySet ,0) ,BO.OutQtyBySet)
       , [RFID Print Farm In Qty] = IIF(EXISTS (SELECT 1 FROM [SplitString]( Artwork.Artwork , '+')  WHERE Data IN ('PRINTING')),ISNULL( prt.InQtyBySet,0) ,prt.InQtyBySet)
       , [RFID Print Farm Out Qty] = IIF(EXISTS (SELECT 1 FROM [SplitString]( Artwork.Artwork , '+')  WHERE Data IN ('PRINTING')),ISNULL( prt.OutQtyBySet,0) ,prt.OutQtyBySet)
       , [RFID AT Farm In Qty] =IIF(EXISTS (SELECT 1 FROM [SplitString]( Artwork.Artwork , '+')  WHERE Data IN ('AT')),ISNULL(AT.InQtyBySet,0) ,AT.InQtyBySet)
       , [RFID AT Farm Out Qty] = IIF(EXISTS (SELECT 1 FROM [SplitString]( Artwork.Artwork , '+')  WHERE Data IN ('AT')),ISNULL(AT.OutQtyBySet,0) ,AT.OutQtyBySet)
       , [RFID Pad Print Farm In Qty] = IIF(EXISTS (SELECT 1 FROM [SplitString]( Artwork.Artwork , '+')  WHERE Data IN ('PAD PRINTING')),ISNULL(PADPRT.InQtyBySet ,0) ,PADPRT.InQtyBySet)
       , [RFID Pad Print Farm Out Qty] = IIF(EXISTS (SELECT 1 FROM [SplitString]( Artwork.Artwork , '+')  WHERE Data IN ('PAD PRINTING')),ISNULL( PADPRT.OutQtyBySet,0) ,PADPRT.OutQtyBySet)
       , [RFID Emboss Farm In Qty] = IIF(EXISTS (SELECT 1 FROM [SplitString]( Artwork.Artwork , '+')  WHERE Data IN ('EMBOSS/DEBOSS')),ISNULL( SUBCONEMB.InQtyBySet ,0) ,SUBCONEMB.InQtyBySet)
       , [RFID Emboss Farm Out Qty] = IIF(EXISTS (SELECT 1 FROM [SplitString]( Artwork.Artwork , '+')  WHERE Data IN ('EMBOSS/DEBOSS')),ISNULL( SUBCONEMB.OutQtyBySet ,0) ,SUBCONEMB.OutQtyBySet)
       , [RFID HT Farm In Qty] = IIF(EXISTS (SELECT 1 FROM [SplitString]( Artwork.Artwork , '+')  WHERE Data IN ('HEAT TRANSFER')),ISNULL( HT.InQtyBySet ,0) ,HT.InQtyBySet)
       , [RFID HT Farm Out Qty] = IIF(EXISTS (SELECT 1 FROM [SplitString]( Artwork.Artwork , '+')  WHERE Data IN ('HEAT TRANSFER')),ISNULL( HT.OutQtyBySet ,0) ,HT.OutQtyBySet)

        , SubProcessStatus=
			case when t.Junk = 1 then null
                 when SORTING.OutQtyBySet is null and loading.InQtyBySet is null 
                    and SewingLine.InQtyBySet is null                    
                    and Emb.InQtyBySet is null and Emb.OutQtyBySet is null
                    and BO.InQtyBySet is null and BO.OutQtyBySet  is null 
                    and prt.InQtyBySet  is null and prt.OutQtyBySet  is null 
                    and AT.InQtyBySet  is null and AT.OutQtyBySet  is null 
                    and PADPRT.InQtyBySet is null and PADPRT.OutQtyBySet is null
                    and SUBCONEMB.InQtyBySet is null and SUBCONEMB.OutQtyBySet is null
                    and HT.InQtyBySet is null and HT.OutQtyBySet is null                
                then null
				 when SORTINGStatus.v = 1 and loadingStatus.v = 1 --判斷有做加工段的數量=訂單qty,則為1,全部為1才為Y
                    and SewingLineStatus.v = 1 
					and Emb_i.v = 1 and Emb_o.v = 1
					and BO_i.v = 1 and BO_o.v = 1
					and prt_i.v = 1 and prt_o.v = 1
					and AT_i.v = 1 and AT_o.v = 1
					and PADPRT_i.v = 1 and PADPRT_o.v = 1
					and SUBCONEMB_i.v = 1 and SUBCONEMB_o.v = 1
					and HT_i.v = 1 and HT_o.v = 1
				 then 'Y'
			end
       , #cte2.EMBROIDERY_qty
       , #cte2.BONDING_qty
       , #cte2.PRINTING_qty
       , #cte2.sewing_output
       , [Balance] = t.qty + t.FOCQty - #cte2.sewing_output 
       , #cte2.firstSewingDate              
	   , [Last Sewn Date] = vsis.LastSewDate
       , #cte2.AVG_QAQTY
       , [Est_offline] = DATEADD(DAY
                                 , iif(isnull(#cte2.AVG_QAQTY, 0) = 0, 0
                                                                     , ceiling((t.qty+t.FOCQty - #cte2.sewing_output) / (#cte2.AVG_QAQTY*1.0)))
                                 , #cte2.firstSewingDate) 
       , [Scanned_Qty] = PackDetail.ScanQty
       , [pack_rate] = IIF(isnull(t.TotalCTN, 0) = 0, 0
                                                    , round(t.ClogCTN / (t.TotalCTN * 1.0), 4) * 100 ) 
       , t.TotalCTN
       , FtyCtn = t.TotalCTN - t.FtyCTN
       , t.ClogCTN
       , t.CFACTN
-----------------------------------------------------------------------------------------------------------------------------------------
       , t.InspDate
       , InspResult
       , [CFA Name] = InspHandle
       , t.ActPulloutDate
       , t.FtyKPI                
       , KPIChangeReason = KPIChangeReason.KPIChangeReason  
       , t.PlanDate
       , dbo.getTPEPass1(t.SMR) [SMR]
       , dbo.getTPEPass1(T.MRHandle) [Handle]
       , [PO SMR] = (select dbo.getTPEPass1(p.POSMR) 
                     from dbo.PO p WITH (NOLOCK) 
                     where p.ID = t.POID) 
       , [PO Handle] = (select dbo.getTPEPass1(p.POHandle) 
                        from dbo.PO p WITH (NOLOCK) 
                        where p.ID = t.POID)   
       , [MC Handle] = dbo.getTPEPass1(t.McHandle) 
       , t.DoxType
       , [SpecMark] = (select Name 
                       from Reason WITH (NOLOCK) 
                       where ReasonTypeID = 'Style_SpecialMark' 
                             and ID = t.SpecialMark) 
       , t.GFR
       , t.SampleReason
       , [TMS] = (select s.StdTms * t.CPU 
                  from System s WITH (NOLOCK)) 
");
            if (this.isArtwork)
            {
                sqlCmd.Append(string.Format(@",{0} ", this.artworktypes.ToString().Substring(0, this.artworktypes.ToString().Length - 1)));
            }

            string tmpbyline = byline ? @"
    ,t.FOCQty
    ,AlloQty =(select sum(sdd.AlloQty) from SewingSchedule_Detail sdd where sdd.OrderID  = t.OrderID and sdd.Article =t.Article and sdd.SizeCode = t.SizeCode)
into #lasttmp" : string.Empty;
            sqlCmd.Append($@"
{tmpbyline}
from #cte t 
left join #cte2 on #cte2.OrderID = t.OrderID and #cte2.Article = t.Article and #cte2.SizeCode = t.SizeCode
left join Country with (Nolock) on Country.id= t.Dest
left join View_SewingInfoArticleSize vsis on t.OrderID = vsis.OrderID and t.Article = vsis.Article and t.SizeCode = vsis.SizeCode");
            if (this.isArtwork)
            {
                sqlCmd.Append(string.Format(@"  left join #tmscost_pvt on #tmscost_pvt.orderid = t.orderid "));
            }

            sqlCmd.Append(@"
outer apply ( 
    select KPIChangeReason = ID + '-' + Name   
    from Reason  WITH (NOLOCK) 
    where ReasonTypeID = 'Order_BuyerDelivery' 
          and ID = t.KPIChangeReason 
          and t.KPIChangeReason != '' 
          and t.KPIChangeReason is not null 
) KPIChangeReason 
outer apply (SELECT val =  Stuff((select concat( ',',Format(oqs.IDD, 'yyyy/MM/dd'))   from Order_QtyShip oqs with (nolock) where oqs.ID = t.OrderID and oqs.IDD is not null FOR XML PATH('')),1,1,'') 
  ) IDD
left join #QtyBySetPerSubprocessSorting Sorting on Sorting.OrderID = t.OrderID and Sorting.Article = t.Article and Sorting.Sizecode = t.SizeCode
left join #QtyBySetPerSubprocessSewingLine SewingLine on SewingLine.OrderID = t.OrderID and SewingLine.Article = t.Article and SewingLine.Sizecode = t.SizeCode
left join #QtyBySetPerSubprocessLoading Loading on Loading.OrderID = t.OrderID and Loading.Article = t.Article and Loading.Sizecode = t.SizeCode
left join #QtyBySetPerSubprocessEmb Emb on Emb.OrderID = t.OrderID and Emb.Article = t.Article and Emb.Sizecode = t.SizeCode
left join #QtyBySetPerSubprocessBO BO on BO.OrderID = t.OrderID and BO.Article = t.Article and BO.Sizecode = t.SizeCode
left join #QtyBySetPerSubprocessPRT PRT on PRT.OrderID = t.OrderID and PRT.Article = t.Article and PRT.Sizecode = t.SizeCode
left join #QtyBySetPerSubprocessAT AT on AT.OrderID = t.OrderID and AT.Article = t.Article and AT.Sizecode = t.SizeCode
left join #QtyBySetPerSubprocessPADPRT PADPRT on PADPRT.OrderID = t.OrderID and PADPRT.Article = t.Article and PADPRT.Sizecode = t.SizeCode
left join #QtyBySetPerSubprocessSUBCONEMB SUBCONEMB on SUBCONEMB.OrderID = t.OrderID and SUBCONEMB.Article = t.Article and SUBCONEMB.Sizecode = t.SizeCode
left join #QtyBySetPerSubprocessHT HT on HT.OrderID = t.OrderID and HT.Article = t.Article and HT.Sizecode = t.SizeCode
outer apply(select v = case when SORTING.OutQtyBySet is null or SORTING.OutQtyBySet >= t.Qty then 1 else 0 end)SORTINGStatus--null即不用判斷此加工段 標記1, 數量=訂單數 標記1
outer apply(select v = case when SewingLine.InQtyBySet is null or SewingLine.InQtyBySet >= t.Qty then 1 else 0 end)SewingLineStatus
outer apply(select v = case when loading.InQtyBySet is null or loading.InQtyBySet >= t.Qty then 1 else 0 end)loadingStatus
outer apply(select v = case when Emb.InQtyBySet is null or Emb.InQtyBySet >= t.Qty then 1 else 0 end)Emb_i
outer apply(select v = case when Emb.OutQtyBySet is null or Emb.OutQtyBySet >= t.Qty then 1 else 0 end)Emb_o
outer apply(select v = case when BO.InQtyBySet is null or BO.InQtyBySet >= t.Qty then 1 else 0 end)BO_i
outer apply(select v = case when BO.OutQtyBySet is null or BO.OutQtyBySet >= t.Qty then 1 else 0 end)BO_o
outer apply(select v = case when prt.InQtyBySet is null or prt.InQtyBySet >= t.Qty then 1 else 0 end)prt_i
outer apply(select v = case when prt.OutQtyBySet is null or prt.OutQtyBySet >= t.Qty then 1 else 0 end)prt_o
outer apply(select v = case when AT.InQtyBySet is null or AT.InQtyBySet >= t.Qty then 1 else 0 end)AT_i
outer apply(select v = case when AT.OutQtyBySet is null or AT.OutQtyBySet >= t.Qty then 1 else 0 end)AT_o
outer apply(select v = case when PADPRT.InQtyBySet is null or PADPRT.InQtyBySet >= t.Qty then 1 else 0 end)PADPRT_i
outer apply(select v = case when PADPRT.OutQtyBySet is null or PADPRT.OutQtyBySet >= t.Qty then 1 else 0 end)PADPRT_o
outer apply(select v = case when SUBCONEMB.InQtyBySet is null or SUBCONEMB.InQtyBySet >= t.Qty then 1 else 0 end)SUBCONEMB_i
outer apply(select v = case when SUBCONEMB.OutQtyBySet is null or SUBCONEMB.OutQtyBySet >= t.Qty then 1 else 0 end)SUBCONEMB_o
outer apply(select v = case when HT.InQtyBySet is null or HT.InQtyBySet >= t.Qty then 1 else 0 end)HT_i
outer apply(select v = case when HT.OutQtyBySet is null or HT.OutQtyBySet >= t.Qty then 1 else 0 end)HT_o
outer apply(
	select SewingLineID =stuff((
		  select distinct concat('/',ssd.SewingLineID)
		  from [SewingSchedule] ss WITH (NOLOCK) 
		  inner join SewingSchedule_Detail ssd  WITH (NOLOCK) on ssd.id = ss.id
		  where ssd.orderid = t.OrderID and ssd.Article = t.Article and ssd.SizeCode = t.SizeCode
		  for xml path('')
	  ),1,1,'')
)SewingSchedule
outer apply(
	select Inline = MIN(ss.Inline),Offline = max(SS.Offline)
	from [SewingSchedule] ss WITH (NOLOCK) 
	inner join SewingSchedule_Detail ssd  WITH (NOLOCK) on ssd.id = ss.id
	where ssd.orderid = t.OrderID and ssd.Article = t.Article and ssd.SizeCode = t.SizeCode
)SewingSchedule2
outer apply(
	select StandardOutput =stuff((
		  select distinct concat(',',ssd.ComboType,':',StandardOutput)
		  from [SewingSchedule] ss WITH (NOLOCK) 
		  inner join SewingSchedule_Detail ssd  WITH (NOLOCK) on ssd.id = ss.id
		  where ssd.orderid = t.OrderID and ssd.Article = t.Article and ssd.SizeCode = t.SizeCode
		  for xml path('')
	  ),1,1,'')
)StandardOutput
outer apply(select PatternUkey from dbo.GetPatternUkey(t.POID,'','',t.StyleUkey,t.SizeCode))gp
outer apply(
	select Artwork = STUFF((
		select CONCAT('+', ArtworkTypeId)
		from(
			select distinct [ArtworkTypeId]=IIF(s1.ArtworkTypeId='',s1.ID,s1.ArtworkTypeId)
			from(
				SELECT bda1.SubprocessId
				FROM Bundle b1
				INNER JOIN Bundle_Detail_Order bd1 WITH (NOLOCK) ON b1.ID = bd1.iD
				INNER JOIN Bundle_Detail_Art bda1 WITH (NOLOCK) ON bd1.BundleNo = bda1.Bundleno
				WHERE bd1.Orderid=t.OrderID AND b1.Article=t.Article AND b1.SizeCode=t.SizeCode
	
				EXCEPT
				select s.Data
				from(
					Select distinct Annotation = dbo.[RemoveNumericCharacters](a.Annotation)
					from Pattern_GL a WITH (NOLOCK) 
					Where a.PatternUkey = gp.PatternUkey
					and a.Annotation <> ''
				)x
				outer apply(select * from SplitString(x.Annotation ,'+'))s
			)x
			INNER JOIN Subprocess s1 WITH (NOLOCK) ON s1.ID = x.SubprocessId
		)x
		order by ArtworkTypeID
		for xml path('')
	),1,1,'')
)EXa
outer apply(
	select Artwork = stuff((
		select CONCAT('+',ArtworkTypeID)
		from(
			select distinct [ArtworkTypeId]=IIF(s1.ArtworkTypeId='',s1.ID,s1.ArtworkTypeId)
			from(
				Select distinct Annotation = dbo.[RemoveNumericCharacters](a.Annotation)
				from Pattern_GL a WITH (NOLOCK) 
				Where a.PatternUkey = gp.PatternUkey
				and a.Annotation <> ''
			)x
			outer apply(select * from SplitString(x.Annotation ,'+'))s
			INNER JOIN Subprocess s1 WITH (NOLOCK) ON s1.ID = s.Data
		)x
		order by ArtworkTypeID
		for xml path('')
	),1,1,'')
)ann
outer apply(
	select Artwork =stuff((	
		select concat('+',ArtworkTypeID)
		from(
			SELECT DISTINCT [ArtworkTypeId]=IIF(s1.ArtworkTypeId='',s1.ID,s1.ArtworkTypeId)
			FROM Bundle b1
			INNER JOIN Bundle_Detail_Order bd1 WITH (NOLOCK) ON b1.ID = bd1.iD
			INNER JOIN Bundle_Detail_Art bda1 WITH (NOLOCK) ON bd1.BundleNo = bda1.Bundleno
			INNER JOIN Subprocess s1 WITH (NOLOCK) ON s1.ID=bda1.SubprocessId
			WHERE bd1.Orderid=t.OrderID AND b1.Article=t.Article AND b1.SizeCode=t.SizeCode
		)tmpartwork
		for xml path('')
	),1,1,'')
)Artwork
outer apply(
	select SubProcessDest = concat('Inhouse:'+stuff((
		select concat(',',ot.ArtworkTypeID)
		from order_tmscost ot WITH (NOLOCK)
		inner join artworktype WITH (NOLOCK) on ot.artworktypeid = artworktype.id 
		where ot.id = t.OrderID and ot.InhouseOSP = 'I' 
		and artworktype.isSubprocess = 1
		for xml path('')
	),1,1,'')
	,'; '+(
	select opsc=stuff((
		select concat('; ',ospA.abb+':'+ospB.spdO)
		from
		(
			select distinct abb = isnull(l.abb,'')
			from order_tmscost ot WITH (NOLOCK)
			inner join artworktype WITH (NOLOCK) on ot.artworktypeid = artworktype.id 
			left join localsupp l WITH (NOLOCK) on l.id = ot.LocalSuppID
			where ot.id = t.OrderID and ot.InhouseOSP = 'o'
			and artworktype.isSubprocess = 1
		)ospA
		outer apply(
			select spdO = stuff((
				select concat(',',ot.ArtworkTypeID) 
				from order_tmscost ot WITH (NOLOCK)
				inner join artworktype WITH (NOLOCK) on ot.artworktypeid = artworktype.id 
				left join localsupp l WITH (NOLOCK) on l.id = ot.LocalSuppID
				where ot.id = t.OrderID and ot.InhouseOSP = 'o'and isnull(l.Abb,'') = ospA.abb
			    and artworktype.isSubprocess = 1
				for xml path('')
			),1,1,'')
		)ospB
		for xml path('')
	),1,1,'')))
)spdX
outer apply(select EstimatedCutDate = min(EstCutDate) from WorkOrder wo WITH (NOLOCK) where t.POID = wo.id)EstCutDate
outer apply(
    select ScanQty = sum(pd.ScanQty)
    from #tmp_PackingList_Detail pd
    where pd.OrderId = t.OrderID
    and pd.Article = t.Article
	and pd.SizeCode = t.SizeCode
)PackDetail
");

            sqlCmd.Append(string.Format(@" order by {0}, t.Article, t.SizeCode" + Environment.NewLine, this.orderby));
            sqlCmd.Append(" drop table #cte, #cte2, #tmp_PackingList_Detail;" + Environment.NewLine);
            foreach (string subprocess in subprocessIDs)
            {
                string whereSubprocess = subprocess;
                if (subprocess.Equals("PAD-PRT"))
                {
                    whereSubprocess = "PADPRT";
                }

                sqlCmd.Append(string.Format(@" drop table #QtyBySetPerSubprocess{0};" + Environment.NewLine, whereSubprocess));
            }
            #endregion

            return sqlCmd;
        }

        private StringBuilder SummaryBySPLine()
        {
            StringBuilder ars = new StringBuilder();
            if (this.isArtwork)
            {
                foreach (DataRow dr in this.dtArtworkType.Rows)
                {
                    ars.Append($",[{dr["id"]}]=sum([{dr["id"]}])");
                }
            }

            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append($@"

select
    t.MDivisionID
    , t.FactoryID
    , t.SewingLineID
    , t.OrdersBuyerDelivery
    , t.SciDelivery
    , Inline = min(t.Inline)
    , Offline = max(t.Offline)
    , t.val
    , t.BrandID
    , t.OrderID
    , t.POID
    , t.Cancelled
    , t.Dest
    , t.StyleID
    , t.OrderTypeID
    , t.ShipModeList
    , t.[PartialShipping]
    , t.[OrderNo]
    , t.CustPONo
    , t.CustCDID
    , t.ProgramID
    , t.CdCodeID
	, t.CDCodeNew
	, t.ProductType
	, t.FabricType
	, t.Lining
	, t.Gender
	, t.Construction
    , t.KPILETA
    , t.LETA
    , t.MTLETA
    , t.SewETA
    , t.PackETA
    , t.CPU
    , [TTL CPU] = t.CPU * SUM(t.Qty)
    , [CPU Closed] = t.CPU  * sum(t.sewing_output)
    , [CPU bal] = t.CPU * (SUM(t.Qty) + t.FOCQty - sum(t.sewing_output) )
    , article_list = al2.article_list
    , Qty = SUM(t.Qty)
    , AlloQty = sum(t.AlloQty)
    , st2.StandardOutput
    , oriArtwork = oann.oriArtwork
    , AddedArtwork = aann.AddedArtwork
    , Artwork.Artwork
    , t.SubProcessDest
    , EstimatedCutDate = MIN(t.EstimatedCutDate)
    , first_cut_date = MIN(t.first_cut_date)
    , cut_qty = SUM(cut_qty)
    , [RFID Cut Qty] = SUM([RFID Cut Qty])
    , [RFID SewingLine In Qty] = SUM([RFID SewingLine In Qty])
    , [RFID Loading Qty] = SUM([RFID Loading Qty])
    , [RFID Emb Farm In Qty] = SUM([RFID Emb Farm In Qty])
    , [RFID Emb Farm Out Qty] = SUM([RFID Emb Farm Out Qty])
    , [RFID Bond Farm In Qty] = SUM([RFID Bond Farm In Qty])
    , [RFID Bond Farm Out Qty] = SUM([RFID Bond Farm Out Qty])
    , [RFID Print Farm In Qty] = SUM([RFID Print Farm In Qty])
    , [RFID Print Farm Out Qty] = SUM([RFID Print Farm Out Qty])
    , [RFID AT Farm In Qty] = SUM([RFID AT Farm In Qty])
    , [RFID AT Farm Out Qty] = SUM([RFID AT Farm Out Qty])
    , [RFID Pad Print Farm In Qty] = SUM([RFID Pad Print Farm In Qty])
    , [RFID Pad Print Farm Out Qty] = SUM([RFID Pad Print Farm Out Qty])
    , [RFID Emboss Farm In Qty] = SUM([RFID Emboss Farm In Qty])
    , [RFID Emboss Farm Out Qty] = SUM([RFID Emboss Farm Out Qty])
    , [RFID HT Farm In Qty] = SUM([RFID HT Farm In Qty])
    , [RFID HT Farm Out Qty] = SUM([RFID HT Farm Out Qty])
    , ss.SubProcessStatus
    , EMBROIDERY_qty = SUM(t.EMBROIDERY_qty)
    , BONDING_qty = SUM(t.BONDING_qty)
    , PRINTING_qty = SUM(t.PRINTING_qty)
    , sewing_output = SUM(t.sewing_output)
    , [Balance] = SUM(t.Qty) + t.FOCQty - sum(t.sewing_output) 
    , firstSewingDate = MIN(firstSewingDate)
    , [Last Sewn Date] = MAX([Last Sewn Date])
    , AVG_QAQTY = AVG(AVG_QAQTY)
    , [Est_offline] = DATEADD(DAY
                                , iif(isnull(AVG(AVG_QAQTY), 0) = 0, 0
                                                                    , ceiling(SUM(t.Qty) + t.FOCQty - sum(t.sewing_output)  / (AVG(AVG_QAQTY)*1.0)))
                                , MIN(firstSewingDate)) 
    , [Scanned_Qty] = SUM(t.[Scanned_Qty])
    -- 以下來源 即by OrderID
    , t.[pack_rate]
    , t.TotalCTN
    , t.FtyCtn
    , t.ClogCTN
    , t.CFACTN
    , InspDate
    , InspResult
    , [CFA Name]
    , ActPulloutDate
    , FtyKPI
    , KPIChangeReason
    , PlanDate
    , SMR
    , Handle
    , [PO SMR]
    , [PO Handle]
    , [MC Handle]
    , DoxType
    , [SpecMark]
    , GFR
    , SampleReason
    , [TMS]
    {ars}
from #lasttmp t
outer apply(
	select article_list = stuff((
		select concat(',' , article)
		from (
			select distinct t2.Article 
			from #lasttmp t2 
			where t2.OrderID = t.OrderID and t2.SewingLineID = t.SewingLineID
		) t 
		for xml path('')
	),1,1,'')
)al2
outer apply(
	select StandardOutput = stuff((
		select concat(',' , Data)
		from (
			select distinct s.Data
			from #lasttmp t2 
			outer apply(select * from SplitString(t2.StandardOutput ,','))s
			where t2.OrderID = t.OrderID and t2.SewingLineID = t.SewingLineID
		) t 
		for xml path('')
	),1,1,'')
)st2
outer apply(
	select oriArtwork = stuff((
			select concat('+' , Data)
			from (
				select distinct s.Data
				from #lasttmp t2 
				outer apply(select * from SplitString(t2.oriArtwork ,'+'))s
				where t2.OrderID = t.OrderID and t2.SewingLineID = t.SewingLineID
			) t 
			for xml path('')
		),1,1,'')
)oann
outer apply(
	select AddedArtwork = stuff((
			select concat('+' , Data)
			from (
				select distinct s.Data
				from #lasttmp t2 
				outer apply(select * from SplitString(t2.AddedArtwork ,'+'))s
				where t2.OrderID = t.OrderID and t2.SewingLineID = t.SewingLineID
			) t 
			for xml path('')
		),1,1,'')
)aann
outer apply(
	select Artwork = stuff((
			select concat('+' , Data)
			from (
				select distinct s.Data
				from #lasttmp t2 
				outer apply(select * from SplitString(t2.Artwork ,'+'))s
				where t2.OrderID = t.OrderID and t2.SewingLineID = t.SewingLineID
			) t 
			for xml path('')
		),1,1,'')
)Artwork
outer apply(
	select SubProcessStatus = IIF(
		exists(
			select 1
			from #lasttmp t2 
			where t2.OrderID = t.OrderID and t2.SewingLineID = t.SewingLineID
			and SubProcessStatus is null
		)
		or not exists(
			select 1
			from #lasttmp t2 
			where t2.OrderID = t.OrderID and t2.SewingLineID = t.SewingLineID
		),
		null , 'Y')
)ss

group by 
t.MDivisionID
, t.FactoryID
, t.SewingLineID
, t.OrdersBuyerDelivery
, t.SciDelivery
, t.val
, t.BrandID
, t.OrderID
, t.POID
, t.Cancelled
, t.Dest
, t.StyleID
, t.OrderTypeID
, t.ShipModeList
, t.[PartialShipping]
, t.[OrderNo]
, t.CustPONo
, t.CustCDID
, t.ProgramID
, t.CdCodeID
, t.CDCodeNew
, t.ProductType
, t.FabricType
, t.Lining
, t.Gender
, t.Construction
, t.KPILETA
, t.LETA
, t.MTLETA
, t.SewETA
, t.PackETA
, t.CPU
, t.FOCQty 
, al2.article_list
, st2.StandardOutput
, oann.oriArtwork
, aann.AddedArtwork
, Artwork.Artwork
, t.SubProcessDest
, ss.SubProcessStatus
, t.[pack_rate]
, t.TotalCTN
, t.FtyCtn
, t.ClogCTN
, t.CFACTN
, t.InspDate
, InspResult
, [CFA Name]
, ActPulloutDate
, FtyKPI
, KPIChangeReason
, PlanDate
, SMR
, Handle
, [PO SMR]
, [PO Handle]
, [MC Handle]
, DoxType
, [SpecMark]
, GFR
, SampleReason
, [TMS]
");
            return sqlCmd;
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (this.comboBox1.SelectedIndex)
            {
                case 0:
                         this.ReportType = "SP#";
                         break;
                case 1:
                         this.ReportType = "Acticle / Size";
                         break;
                default:
                         break;
            }
        }
    }
}
