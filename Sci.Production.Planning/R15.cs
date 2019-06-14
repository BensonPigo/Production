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
    public partial class R15 : Sci.Win.Tems.PrintForm
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
            this.txtMdivision.Text = Sci.Env.User.Keyword;
            this.txtfactory.Text = Sci.Env.User.Factory;
            MyUtility.Tool.SetupCombox(this.comboOrderBy, 2, 1, "orderid,SPNO,brandid,Brand");
            this.comboOrderBy.SelectedIndex = 0;
            this.dateBuyerDelivery.Select();
            this.dateBuyerDelivery.Value1 = DateTime.Now;
            this.dateBuyerDelivery.Value2 = DateTime.Now.AddDays(30);
            DataTable dt;
            DBProxy.Current.Select(null, "select sby = 'SP#' union all select sby = 'Acticle / Size'", out dt);
            MyUtility.Tool.SetupCombox(this.comboBox1, 1, dt);
            this.comboBox1.SelectedIndex = 0;
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
                (MyUtility.Check.Empty(this.txtSPNoStart.Text) || MyUtility.Check.Empty(this.txtSPNoEnd.Text)))
            {
                MyUtility.Msg.WarningBox("< SCI Delivery > \r\n< Buyer Delivery > \r\n< Sewing Inline > \r\n< Cut Off Date > \r\n< Cust RQS Date > \r\n< Plan Date > \r\n< SP# > \r\ncan't be empty!!");
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
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
            StringBuilder sqlCmd = new StringBuilder();
            if (this.sbyindex == 0)
            {
                sqlCmd = this.SummaryBySP(out cmds);
            }
            else
            {
                sqlCmd = this.SummaryByActicleSize(out cmds);
            }

            DBProxy.Current.DefaultTimeout = 2700;
            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), cmds, out this.printData);
            DBProxy.Current.DefaultTimeout = 300;
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return Result.True;
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
                    Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Planning_R15_WIP.xltx"); // 預先開啟excel app
                    MyUtility.Excel.CopyToXls(this.printData, string.Empty, "Planning_R15_WIP.xltx", 1, false, null, objApp);      // 將datatable copy to excel
                    Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表

                    // 列印動態欄位的表頭
                    for (int i = 0; i < this.dtArtworkType.Rows.Count; i++)
                    {
                        objSheets.Cells[1, 80 + i] = this.dtArtworkType.Rows[i]["id"].ToString();
                    }

                    // 首列資料篩選
                    Microsoft.Office.Interop.Excel.Range firstRow = (Microsoft.Office.Interop.Excel.Range)objSheets.Rows[1];
                    firstRow.AutoFilter(1, Type.Missing, Microsoft.Office.Interop.Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);
                    objApp.Cells.EntireColumn.AutoFit();  // 自動欄寬

                    #region Save & Show Excel
                    string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Planning_R15_WIP");
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
                    Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Planning_R15_WIP.xltx"); // 預先開啟excel app
                    MyUtility.Excel.CopyToXls(this.printData, string.Empty, "Planning_R15_WIP.xltx", 1, false, null, objApp);      // 將datatable copy to excel
                    Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表

                    // 首列資料篩選
                    Microsoft.Office.Interop.Excel.Range firstRow = (Microsoft.Office.Interop.Excel.Range)objSheets.Rows[1];
                    firstRow.AutoFilter(1, Type.Missing, Microsoft.Office.Interop.Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);
                    objApp.Cells.EntireColumn.AutoFit();  // 自動欄寬

                    #region Save & Show Excel
                    string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Planning_R15_WIP");
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
                if (this.isArtwork)
                {
                    Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Planning_R15_WIP_byArticleSize.xltx"); // 預先開啟excel app
                    MyUtility.Excel.CopyToXls(this.printData, string.Empty, "Planning_R15_WIP_byArticleSize.xltx", 1, false, null, objApp);      // 將datatable copy to excel
                    Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表

                    // 列印動態欄位的表頭
                    for (int i = 0; i < this.dtArtworkType.Rows.Count; i++)
                    {
                        objSheets.Cells[1, 80 + i] = this.dtArtworkType.Rows[i]["id"].ToString();
                    }

                    // 首列資料篩選
                    Microsoft.Office.Interop.Excel.Range firstRow = (Microsoft.Office.Interop.Excel.Range)objSheets.Rows[1];
                    firstRow.AutoFilter(1, Type.Missing, Microsoft.Office.Interop.Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);
                    objApp.Cells.EntireColumn.AutoFit();  // 自動欄寬

                    #region Save & Show Excel
                    string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Planning_R15_WIP_byArticleSize");
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
                    Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Planning_R15_WIP_byArticleSize.xltx"); // 預先開啟excel app
                    MyUtility.Excel.CopyToXls(this.printData, string.Empty, "Planning_R15_WIP_byArticleSize.xltx", 1, false, null, objApp);      // 將datatable copy to excel
                    Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表

                    // 首列資料篩選
                    Microsoft.Office.Interop.Excel.Range firstRow = (Microsoft.Office.Interop.Excel.Range)objSheets.Rows[1];
                    firstRow.AutoFilter(1, Type.Missing, Microsoft.Office.Interop.Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);
                    objApp.Cells.EntireColumn.AutoFit();  // 自動欄寬

                    #region Save & Show Excel
                    string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Planning_R15_WIP_byArticleSize");
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

            return true;
        }

        private StringBuilder SummaryBySP(out IList<System.Data.SqlClient.SqlParameter> cmds)
        {
            StringBuilder sqlCmd = new StringBuilder();
            cmds = new List<System.Data.SqlClient.SqlParameter>();

            #region select orders 需要欄位
            sqlCmd.Append(string.Format(@"
                    select o.MDivisionID       , o.FactoryID  , o.SciDelivery     , O.CRDDate           , O.CFMDate       , OrderID = O.ID    
	                       , O.Dest            , O.StyleID    , O.SeasonID        , O.ProjectID         , O.Customize1    , O.BuyMonth
	                       , O.CustPONo        , O.BrandID    , O.CustCDID        , O.ProgramID         , O.CdCodeID      , O.CPU
	                       , O.Qty             , O.FOCQty     , O.PoPrice         , O.CMPPrice          , O.KPILETA       , O.LETA
	                       , O.MTLETA          , O.SewETA     , O.PackETA         , O.MTLComplete       , O.SewInLine     , O.SewOffLine
                           , O.CutInLine       , O.CutOffLine , O.Category        , O.IsForecast        , O.PulloutDate   , O.ActPulloutDate
	                       , O.SMR             , O.MRHandle   , O.MCHandle        , O.OrigBuyerDelivery , O.DoxType       , O.TotalCTN
	                       , O.FtyCTN          , O.ClogCTN    , O.VasShas         , O.TissuePaper       , O.MTLExport     , O.SewLine
	                       , O.ShipModeList    , O.PlanDate   , O.FirstProduction , O.Finished          , O.FtyGroup      , O.OrderTypeID
	                       , O.SpecialMark     , O.GFR        , O.SampleReason    , O.InspDate          , O.MnorderApv    , O.FtyKPI
                           , O.KPIChangeReason , O.StyleUkey  , O.POID            , OrdersBuyerDelivery = o.BuyerDelivery
                           , InspResult = case when o.InspResult = 'P' then 'Pass' when o.InspResult = 'F' then 'Fail' end
                           , InspHandle = o.InspHandle +'-'+ Pass1.Name
                           , O.Junk,DryCTN=isnull(o.DryCTN,0),CFACTN=isnull(o.CFACTN,0)
                    into #cte 
                    from dbo.Orders o WITH (NOLOCK) 
                    left join Pass1 WITH (NOLOCK) on Pass1.ID = O.InspHandle
                    WHERE 1=1"));
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
                sqlCmd.Append(string.Format(@" and o.SewInLine >= '{0}'", Convert.ToDateTime(this.sewingInline1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.sewingInline2))
            {
                sqlCmd.Append(string.Format(@" and o.SewInLine <= '{0}'", Convert.ToDateTime(this.sewingInline2).ToString("d")));
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

            if (!MyUtility.Check.Empty(this.factory))
            {
                sqlCmd.Append(@" and o.factoryid = @factory");
                cmds.Add(new System.Data.SqlClient.SqlParameter("@factory", this.factory));
            }

            sqlCmd.Append($" and o.Category in ({this.category})");
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

            #region SummaryBy SP#
            string[] subprocessIDs = new string[] { "Sorting", "Loading", "Emb", "BO", "PRT", "AT", "PAD-PRT", "SubCONEMB", "HT" };
            string qtyBySetPerSubprocess = this.QtyBySetPerSubprocess(subprocessIDs, "#cte",bySP: true);
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
       , t.SewInLine
       , t.SewOffLine
       , t.BrandID
       , t.OrderID
       , Dest = Country.Alias
       , t.StyleID
       , t.OrderTypeID
       , t.ShipModeList
       , [OrderNo] = t.Customize1
       , t.CustPONo
       , t.CustCDID
       , t.ProgramID
       , t.CdCodeID
       , t.KPILETA
       , t.LETA
       , t.MTLETA
       , t.SewETA
       , t.PackETA
       , t.CPU
       , article_list = (select article + ',' 
                         from (
                              select distinct q.Article  
                              from dbo.Order_Qty q WITH (NOLOCK) 
                              where q.ID = t.OrderID
                         ) t 
                         for xml path('')) 
       , t.Qty
       ,StandardOutput.StandardOutput
       ,Artwork.Artwork
       ,spdX.SubProcessDest
       ,EstCutDate.EstimatedCutDate
       , #cte2.first_cut_date
       , #cte2.cut_qty
       , [RFID Cut Qty] = #SORTING.OutQtyBySet
       , [RFID Loading Qty] = #loading.InQtyBySet
       , [RFID Emb Farm In Qty] = #Emb.InQtyBySet
       , [RFID Emb Farm Out Qty] = #Emb.OutQtyBySet
       , [RFID Bond Farm In Qty] = #BO.InQtyBySet	
       , [RFID Bond Farm Out Qty] = #BO.OutQtyBySet
       , [RFID Print Farm In Qty] = #prt.InQtyBySet
       , [RFID Print Farm Out Qty] = #prt.OutQtyBySet
       , [RFID AT Farm In Qty] = #AT.InQtyBySet
       , [RFID AT Farm Out Qty] = #AT.OutQtyBySet
       , [RFID Pad Print Farm In Qty] = #PADPRT.InQtyBySet
       , [RFID Pad Print Farm Out Qty] = #PADPRT.OutQtyBySet
       , [RFID Emboss Farm In Qty] = #SUBCONEMB.InQtyBySet
       , [RFID Emboss Farm Out Qty] =#SUBCONEMB.OutQtyBySet
       , [RFID HT Farm In Qty] = #HT.InQtyBySet
       , [RFID HT Farm Out Qty] = #HT.OutQtyBySet
        , SubProcessStatus=
			case when t.Junk = 1 then null 
                 when #SORTING.OutQtyBySet is null and #loading.InQtyBySet is null 
                    and #Emb.InQtyBySet is null and #Emb.OutQtyBySet is null
                    and #BO.InQtyBySet is null and #BO.OutQtyBySet  is null 
                    and #prt.InQtyBySet  is null and #prt.OutQtyBySet  is null 
                    and #AT.InQtyBySet  is null and #AT.OutQtyBySet  is null 
                    and #PADPRT.InQtyBySet is null and #PADPRT.OutQtyBySet is null
                    and #SUBCONEMB.InQtyBySet is null and #SUBCONEMB.OutQtyBySet is null
                    and #HT.InQtyBySet is null and #HT.OutQtyBySet is null                
                then null
				 when SORTINGStatus.v = 1 and loadingStatus.v = 1 --判斷有做加工段的數量=訂單qty,則為1,全部為1才為Y
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
       , [pack_rate] = IIF(isnull(t.TotalCTN, 0) = 0, 0
                                                    , round(t.ClogCTN / (t.TotalCTN * 1.0), 4) * 100 ) 
       , t.TotalCTN
       , t.DryCTN
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
left join #Sorting on #Sorting.OrderID = t.OrderID
left join #Loading on #Loading.OrderID = t.OrderID
left join #Emb on #Emb.OrderID = t.OrderID
left join #BO on #BO.OrderID = t.OrderID
left join #PRT on #PRT.OrderID = t.OrderID
left join #AT on #AT.OrderID = t.OrderID
left join #PADPRT on #PADPRT.OrderID = t.OrderID
left join #SUBCONEMB on #SUBCONEMB.OrderID = t.OrderID
left join #HT on #HT.OrderID = t.OrderID
outer apply(select v = case when #SORTING.OutQtyBySet is null or #SORTING.OutQtyBySet >= t.Qty then 1 else 0 end)SORTINGStatus--null即不用判斷此加工段 標記1, 數量=訂單數 標記1
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
outer apply(
	select Artwork =stuff((	
		select concat('+',ArtworkTypeID)
		from(
			select distinct v.ArtworkTypeID
			from dbo.View_Order_Artworks v  WITH (NOLOCK) 
			where v.ID=t.OrderID
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
");

            sqlCmd.Append(string.Format(@" order by {0}", this.orderby));
            sqlCmd.Append(@" ;DROP TABLE #imp_LastSewnDate,#cte,#cte2");
            if (this.isArtwork)
            {
                sqlCmd.Append(@" ;drop table #rawdata_tmscost,#tmscost_pvt");
            }
            #endregion

            return sqlCmd;
        }

        private StringBuilder SummaryByActicleSize(out IList<System.Data.SqlClient.SqlParameter> cmds)
        {
            StringBuilder sqlCmd = new StringBuilder();
            cmds = new List<System.Data.SqlClient.SqlParameter>();

            #region select orders 需要欄位
            sqlCmd.Append(string.Format(@"

select o.MDivisionID       , o.FactoryID  , o.SciDelivery     , O.CRDDate           , O.CFMDate       , OrderID = O.ID    
	   , O.Dest            , O.StyleID    , O.SeasonID        , O.ProjectID         , O.Customize1    , O.BuyMonth
	   , O.CustPONo        , O.BrandID    , O.CustCDID        , O.ProgramID         , O.CdCodeID      , O.CPU
	   , Oq.Qty            , O.FOCQty     , O.PoPrice         , O.CMPPrice          , O.KPILETA       , O.LETA
	   , O.MTLETA          , O.SewETA     , O.PackETA         , O.MTLComplete       , O.SewInLine     , O.SewOffLine
       , O.CutInLine       , O.CutOffLine , O.Category        , O.IsForecast        , O.PulloutDate   , O.ActPulloutDate
	   , O.SMR             , O.MRHandle   , O.MCHandle        , O.OrigBuyerDelivery , O.DoxType       , O.TotalCTN
	   , O.FtyCTN          , O.ClogCTN    , O.VasShas         , O.TissuePaper       , O.MTLExport     , O.SewLine
	   , O.ShipModeList    , O.PlanDate   , O.FirstProduction , O.Finished          , O.FtyGroup      , O.OrderTypeID
	   , O.SpecialMark     , O.GFR        , O.SampleReason    , O.InspDate          , O.MnorderApv    , O.FtyKPI
       , O.KPIChangeReason , O.StyleUkey  , O.POID            , OrdersBuyerDelivery = o.BuyerDelivery
       , InspResult = case when o.InspResult = 'P' then 'Pass' when o.InspResult = 'F' then 'Fail' end
       , InspHandle = o.InspHandle +'-'+ Pass1.Name
       , O.Junk,DryCTN=isnull(o.DryCTN,0),CFACTN=isnull(o.CFACTN,0)
	   , oq.Article,oq.SizeCode
into #cte 
from dbo.Orders o WITH (NOLOCK) 
left join Pass1 WITH (NOLOCK) on Pass1.ID = O.InspHandle
left join Order_Qty oq WITH (NOLOCK) on oq.ID = o.ID
WHERE 1=1 "));
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
                sqlCmd.Append(string.Format(@" and o.SewInLine >= '{0}'", Convert.ToDateTime(this.sewingInline1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.sewingInline2))
            {
                sqlCmd.Append(string.Format(@" and o.SewInLine <= '{0}'", Convert.ToDateTime(this.sewingInline2).ToString("d")));
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

            if (!MyUtility.Check.Empty(this.factory))
            {
                sqlCmd.Append(@" and o.factoryid = @factory");
                cmds.Add(new System.Data.SqlClient.SqlParameter("@factory", this.factory));
            }

            sqlCmd.Append($" and o.Category in ({this.category})");
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
            string[] subprocessIDs = new string[] { "Sorting", "Loading", "Emb", "BO", "PRT", "AT", "PAD-PRT", "SubCONEMB", "HT" };
            string qtyBySetPerSubprocess = this.QtyBySetPerSubprocess(subprocessIDs, "#cte", bySP: false);
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

select sod.OrderID ,Max(so.OutputDate) LastSewnDate
into #imp_LastSewnDate
from SewingOutput so WITH (NOLOCK) 
inner join SewingOutput_Detail sod WITH (NOLOCK) on so.ID = sod.ID
inner join #cte t on sod.OrderID = t.OrderID 
group by sod.OrderID 

{qtyBySetPerSubprocess}

select t.MDivisionID
       , t.FactoryID
       , SewingSchedule.SewingLineID
       , t.OrdersBuyerDelivery
       , SewingSchedule2.Inline
       , SewingSchedule2.Offline
       , t.BrandID
       , t.OrderID
       , Dest = Country.Alias
       , t.StyleID
       , t.OrderTypeID
       , t.ShipModeList
       , [OrderNo] = t.Customize1
       , t.CustPONo
       , t.CustCDID
       , t.ProgramID
       , t.CdCodeID
       , t.KPILETA
       , t.LETA
       , t.MTLETA
       , t.SewETA
       , t.PackETA
       , t.CPU
       , t.Article
	   , t.SizeCode
-----------------------------------------------------------------------------------------------------------------------------------------
       , t.Qty
       ,StandardOutput.StandardOutput
       ,Artwork.Artwork
       ,spdX.SubProcessDest
       ,EstCutDate.EstimatedCutDate
       , #cte2.first_cut_date
       , #cte2.cut_qty
       , [RFID Cut Qty] = SORTING.OutQtyBySet
       , [RFID Loading Qty] = loading.InQtyBySet
       , [RFID Emb Farm In Qty] = Emb.InQtyBySet
       , [RFID Emb Farm Out Qty] = Emb.OutQtyBySet
       , [RFID Bond Farm In Qty] = BO.InQtyBySet	
       , [RFID Bond Farm Out Qty] = BO.OutQtyBySet
       , [RFID Print Farm In Qty] = prt.InQtyBySet
       , [RFID Print Farm Out Qty] = prt.OutQtyBySet
       , [RFID AT Farm In Qty] = AT.InQtyBySet
       , [RFID AT Farm Out Qty] = AT.OutQtyBySet
       , [RFID Pad Print Farm In Qty] = PADPRT.InQtyBySet
       , [RFID Pad Print Farm Out Qty] = PADPRT.OutQtyBySet
       , [RFID Emboss Farm In Qty] = SUBCONEMB.InQtyBySet
       , [RFID Emboss Farm Out Qty] =SUBCONEMB.OutQtyBySet
       , [RFID HT Farm In Qty] = HT.InQtyBySet
       , [RFID HT Farm Out Qty] = HT.OutQtyBySet
        , SubProcessStatus=
			case when t.Junk = 1 then null
                 when SORTING.OutQtyBySet is null and loading.InQtyBySet is null 
                    and Emb.InQtyBySet is null and Emb.OutQtyBySet is null
                    and BO.InQtyBySet is null and BO.OutQtyBySet  is null 
                    and prt.InQtyBySet  is null and prt.OutQtyBySet  is null 
                    and AT.InQtyBySet  is null and AT.OutQtyBySet  is null 
                    and PADPRT.InQtyBySet is null and PADPRT.OutQtyBySet is null
                    and SUBCONEMB.InQtyBySet is null and SUBCONEMB.OutQtyBySet is null
                    and HT.InQtyBySet is null and HT.OutQtyBySet is null                
                then null
				 when SORTINGStatus.v = 1 and loadingStatus.v = 1 --判斷有做加工段的數量=訂單qty,則為1,全部為1才為Y
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
       , [pack_rate] = IIF(isnull(t.TotalCTN, 0) = 0, 0
                                                    , round(t.ClogCTN / (t.TotalCTN * 1.0), 4) * 100 ) 
       , t.TotalCTN               
       , t.DryCTN
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

            sqlCmd.Append(string.Format(@" 
from #cte t 
left join #cte2 on #cte2.OrderID = t.OrderID and #cte2.Article = t.Article and #cte2.SizeCode = t.SizeCode
left join Country with (Nolock) on Country.id= t.Dest
left join #imp_LastSewnDate l on t.OrderID = l.OrderID"));
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
left join #QtyBySetPerSubprocessSorting Sorting on Sorting.OrderID = t.OrderID and Sorting.Article = t.Article and Sorting.Sizecode = t.SizeCode
left join #QtyBySetPerSubprocessLoading Loading on Loading.OrderID = t.OrderID and Loading.Article = t.Article and Loading.Sizecode = t.SizeCode
left join #QtyBySetPerSubprocessEmb Emb on Emb.OrderID = t.OrderID and Emb.Article = t.Article and Emb.Sizecode = t.SizeCode
left join #QtyBySetPerSubprocessBO BO on BO.OrderID = t.OrderID and BO.Article = t.Article and BO.Sizecode = t.SizeCode
left join #QtyBySetPerSubprocessPRT PRT on PRT.OrderID = t.OrderID and PRT.Article = t.Article and PRT.Sizecode = t.SizeCode
left join #QtyBySetPerSubprocessAT AT on AT.OrderID = t.OrderID and AT.Article = t.Article and AT.Sizecode = t.SizeCode
left join #QtyBySetPerSubprocessPADPRT PADPRT on PADPRT.OrderID = t.OrderID and PADPRT.Article = t.Article and PADPRT.Sizecode = t.SizeCode
left join #QtyBySetPerSubprocessSUBCONEMB SUBCONEMB on SUBCONEMB.OrderID = t.OrderID and SUBCONEMB.Article = t.Article and SUBCONEMB.Sizecode = t.SizeCode
left join #QtyBySetPerSubprocessHT HT on HT.OrderID = t.OrderID and HT.Article = t.Article and HT.Sizecode = t.SizeCode
outer apply(select v = case when SORTING.OutQtyBySet is null or SORTING.OutQtyBySet >= t.Qty then 1 else 0 end)SORTINGStatus--null即不用判斷此加工段 標記1, 數量=訂單數 標記1
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
		  select distinct concat(',',ssd.SewingLineID)
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
outer apply(
	select Artwork =stuff((	
		select concat('+',ArtworkTypeID)
		from(
			select distinct v.ArtworkTypeID
			from dbo.View_Order_Artworks v  WITH (NOLOCK) 
			where v.ID=t.OrderID and v.Article = t.Article and v.SizeCode = t.SizeCode
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
");

            sqlCmd.Append(string.Format(@" order by {0}, t.Article, t.SizeCode", this.orderby));
            sqlCmd.Append(@";drop table #imp_LastSewnDate");
            if (this.isArtwork)
            {
                sqlCmd.Append(@";drop table #rawdata_tmscost,#tmscost_pvt");
            }
            #endregion

            return sqlCmd;
        }

        /// <summary>
        /// 此function初始目的Planning R15 效能. 因為R15使用procedure太慢, 若無大量資料計算需求, 請使用procedure的QtyBySetPerSubprocess
        /// 功能同sql table function QtyBySetPerSubprocess最終算出每張[訂單,Article,Size]目前可完成的成衣件數
        /// isMorethenOrderQty
        /// </summary>
        /// <param name="subprocessIDs">字串陣列,需要計算的工段</param>
        /// <param name="tempTable">傳入需有OrderID欄位</param>
        /// <param name="bySP">是否要計算出bySP的Temp table</param>
        /// <param name="isNeedCombinBundleGroup">是否要依照 BundleGroup 算成衣件數 true/false</param>
        /// <param name="isMorethenOrderQty">回傳Qty值是否超過訂單數, (生產有可能超過) </param>
        /// <returns>回傳字串, 提供接下去的Sql指令使用#temp Table</returns>
        private string QtyBySetPerSubprocess(
            string[] subprocessIDs,
            string tempTable = "#cte",
            bool bySP = false,
            bool isNeedCombinBundleGroup = false,
            string isMorethenOrderQty = "0")
        {
            string sqlcmd = $@"
-- 1.	尋找指定訂單 Fabric Combo + Fabric Panel Code
-- 使用資料表 Bundle 去除重複即可得到每張訂單 Fabric Combo + Fabric Panel Code + Article + SizeCode
select	distinct
		bun.Orderid
		, bun.POID
		, bun.PatternPanel
		, bun.FabricPanelCode
		, bun.Article
		, bun.Sizecode
into #AllOrders
from Bundle bun WITH (NOLOCK) 
inner join Orders os  WITH (NOLOCK) on bun.Orderid = os.ID and bun.MDivisionID = os.MDivisionID
inner join {tempTable} t on t.OrderID = bun.Orderid
";

            foreach (string subprocessID in subprocessIDs)
            {
                string subprocessIDt = subprocessID.Replace("-", string.Empty); // 把PAD-PRT為PADPRT, 命名#table名稱用
                string isSpectialReader = string.Empty;
                if (subprocessID.ToLower().EqualString("sorting") || subprocessID.ToLower().EqualString("loading"))
                {
                    isSpectialReader = "1";
                }
                else
                {
                    isSpectialReader = "0";
                }

                // --Step 2. --
                //-- * 2.找出所有 Fabric Combo +Fabric Pancel Code +Article + SizeCode->Cartpart(包含同部位數量)
                //--使用資料表 Bundle_Detail
                // --條件 訂單號碼 + Fabric Combo + Fabric Panel Code +Article + SizeCode
                // --top 1 Bundle Group 當作基準計算每個部位數量
                // --數量分成以下 2 種
                // --a.QtyBySet
                // --  數量直接加總
                // --b.QtyBySubprocess
                // --  部位有須要用 X 外加工計算
                sqlcmd += $@"
select	*
		, num = count (1) over (partition by OrderID, PatternPanel, FabricPanelCode, Article, Sizecode)
into #QtyBySetPerCutpart{subprocessIDt}
from #AllOrders st1
outer apply (
	select	bunD.Patterncode
			, QtyBySet = count (1)
			, QtyBySubprocess = sum (isnull (QtyBySubprocess.v, 0))
	from (
		select	top 1
				bunD.ID
				, bunD.BundleGroup
		from Bundle_Detail bunD WITH (NOLOCK) 
		inner join Bundle bun  WITH (NOLOCK) on bunD.Id = bun.ID
		where bun.Orderid = st1.Orderid
				and bun.PatternPanel = st1.PatternPanel
				and bun.FabricPanelCode = st1.FabricPanelCode
				and bun.Article = st1.Article
				and bun.Sizecode = st1.Sizecode
	) getGroupInfo
	inner join Bundle_Detail bunD on getGroupInfo.Id = bunD.Id and getGroupInfo.BundleGroup = bunD.BundleGroup
	outer apply (
		select v = (select 1
					where exists (select 1								  
									from Bundle_Detail_Art BunDArt WITH (NOLOCK) 
									where BunDArt.Bundleno = bunD.BundleNo
										and BunDArt.SubprocessId = '{subprocessID}'))
	) QtyBySubprocess
	group by bunD.Patterncode
) CutpartCount

-- Step 3. --加總每個訂單各 Fabric Combo 所有捆包的『數量』
select	st2.Orderid
		, st2.Article
		, st2.Sizecode
		, QtyBySet = sum (st2.QtyBySet)
		, QtyBySubprocess = sum (st2.QtyBySubProcess)
into #CutpartBySet{subprocessIDt}
from #QtyBySetPerCutpart{subprocessIDt} st2
group by st2.Orderid, st2.Article, st2.Sizecode

-- Query by Set per Subprocess--
--1.	找出時間區間內指定訂單中裁片的進出資訊
select	st0.Orderid
		, st0.Article
		, st0.SizeCode
		, st0.PatternPanel
		, st0.FabricPanelCode
		, st0.Patterncode
		, InQty = case when {isSpectialReader} = 1 and st0.QtyBySet =0 then 0
					when {isSpectialReader} = 0 and st0.QtyBySubprocess = 0 then 0
					when {isSpectialReader} = 1 then FLOOR(sum(bunD.Qty) / st0.QtyBySet)
					when {isSpectialReader} = 0 then FLOOR(sum(bunD.Qty) / st0.QtyBySubprocess)
					end
		, OutQty = 0
		, bunD.BundleGroup
into #RFID{subprocessIDt}
from #QtyBySetPerCutpart{subprocessIDt} st0					
inner join Order_SizeCode os  WITH (NOLOCK) on st0.POID = os.Id and st0.Sizecode = os.SizeCode
inner join Bundle_Detail bunD  WITH (NOLOCK) on bunD.Patterncode = st0.Patterncode
inner join Bundle bun  WITH (NOLOCK) on bunD.Id = bun.ID and bun.Orderid = st0.Orderid
									and bun.PatternPanel = st0.PatternPanel
									and bun.FabricPanelCode = st0.FabricPanelCode
									and bun.Article = st0.Article
									and bun.Sizecode = st0.Sizecode
inner join BundleInOut bunIO  WITH (NOLOCK) on bunIO.BundleNo = bunD.BundleNo 
where ({isSpectialReader} = 1 or st0.QtyBySubprocess != 0) 
		and bunIO.SubProcessId = '{subprocessID}'
		and bunIO.InComing is not null
		and isnull(bunIO.RFIDProcessLocationID,'') = ''
group by st0.Orderid, st0.SizeCode, st0.PatternPanel, st0.FabricPanelCode, st0.Patterncode, st0.Article,bunD.BundleGroup, st0.QtyBySet, st0.QtyBySubprocess

union all
select	st0.Orderid
		, st0.Article
		, st0.SizeCode
		, st0.PatternPanel
		, st0.FabricPanelCode
		, st0.Patterncode
		, InQty = 0
		, OutQty = case when {isSpectialReader} = 1 and isnull(st0.QtyBySet,0) =0 then 0
					when {isSpectialReader} = 0 and isnull(st0.QtyBySubprocess,0) = 0 then 0
					when {isSpectialReader} = 1 then FLOOR(sum(bunD.Qty) / st0.QtyBySet)
					when {isSpectialReader} = 0 then FLOOR(sum(bunD.Qty) / st0.QtyBySubprocess)
					end
		, bunD.BundleGroup
from #QtyBySetPerCutpart{subprocessIDt} st0					
inner join Order_SizeCode os  WITH (NOLOCK) on st0.POID = os.Id and st0.Sizecode = os.SizeCode
inner join Bundle_Detail bunD  WITH (NOLOCK) on bunD.Patterncode = st0.Patterncode
inner join Bundle bun  WITH (NOLOCK) on bunD.Id = bun.ID and bun.Orderid = st0.Orderid
									and bun.PatternPanel = st0.PatternPanel
									and bun.FabricPanelCode = st0.FabricPanelCode
									and bun.Article = st0.Article
									and bun.Sizecode = st0.Sizecode
inner join BundleInOut bunIO  WITH (NOLOCK) on bunIO.BundleNo = bunD.BundleNo 
where (1 = 1 or st0.QtyBySubprocess != 0) 
		and bunIO.SubProcessId = '{subprocessID}'
		and bunIO.OutGoing is not null
		and isnull(bunIO.RFIDProcessLocationID,'') = ''
group by st0.Orderid, st0.SizeCode, st0.PatternPanel, st0.FabricPanelCode, st0.Patterncode, st0.Article,bunD.BundleGroup, st0.QtyBySet, st0.QtyBySubprocess
--
select	st0.Orderid
		, BundleGroup = r.BundleGroup
		, Size = os.SizeCode
		, st0.Article
		, st0.PatternPanel
		, st0.FabricPanelCode
		, st0.PatternCode
		, InQty = sum (isnull (r.InQty, 0))
		, OutQty = sum (isnull (r.OutQty, 0))
		, OriInQty = sum (isnull (r.InQty, 0))
		, OriOutQty = sum (isnull (r.OutQty, 0))
		, num = count (1) over (partition by st0.Orderid, os.SizeCode, st0.PatternPanel, st0.FabricPanelCode, r.BundleGroup)
into #BundleInOutQty{subprocessIDt}
from #QtyBySetPerCutpart{subprocessIDt} st0
left join Order_SizeCode os on st0.POID = os.Id and st0.Sizecode = os.SizeCode
left join #RFID{subprocessIDt} r on r.OrderID = st0.OrderID 
				and r.Article = st0.Article 
				and r.SizeCode = st0.SizeCode 
				and r.PatternPanel = st0.PatternPanel 
				and r.FabricPanelCode = st0.FabricPanelCode
				and r.PatternCode = st0.PatternCode
where ({isSpectialReader} = 1 or st0.QtyBySubprocess != 0)
group by st0.OrderID, r.BundleGroup, os.SizeCode, st0.PatternPanel, st0.FabricPanelCode, st0.Article, st0.PatternCode, st0.num
";

                if (isNeedCombinBundleGroup)
                {
                    sqlcmd += $@"
--篩選 BundleGroup Step.1 --
update bunInOut
set bunInOut.InQty = 0
	, bunInOut.OutQty = 0
from #BundleInOutQty{subprocessIDt} bunInOut
inner join #QtyBySetPerCutpart{subprocessIDt} bas on bunInOut.OrderID = bas.OrderID
										and bunInOut.PatternPanel = bas.PatternPanel
										and bunInOut.FabricPanelCode = bas.FabricPanelCode
										and bunInOut.Article = bas.Article
										and bunInOut.Size = bas.SizeCode
where bunInOut.num < bas.num

select	OrderID
		, Article
		, Size
		, InQty = min (InQty)
		, OutQty = min (OutQty)
into #FinalQtyBySet{subprocessIDt}
from (
	select	OrderID
			, Size
			, Article
			, PatternPanel
			, InQty = min (InQty)
			, OutQty = min (OutQty)
	from (
		select	OrderID
				, Size
				, Article
				, PatternPanel
				, FabricPanelCode
				, InQty = sum (InQty)
				, OutQty = sum (OutQty)
		from (
			select	OrderID
					, Size
					, Article
					, PatternPanel
					, FabricPanelCode
					, BundleGroup
					, InQty = min (InQty)
					, OutQty = min (OutQty)
			from #BundleInOutQty{subprocessIDt}
			group by OrderID, Size, Article, PatternPanel, FabricPanelCode, BundleGroup
		) minGroupCutpart							
		group by OrderID, Size, Article, PatternPanel, FabricPanelCode
	) sumGroup
	group by OrderID, Size, Article, PatternPanel
) minFabricPanelCode
group by OrderID, Size, Article
";
                }
                else
                {
                    sqlcmd += $@"
select	OrderID
		, Article
		, Size
		, InQty = min (InQty)
		, OutQty = min (OutQty)
into #FinalQtyBySet{subprocessIDt}
from (
	select	OrderID
			, Size
			, Article
			, PatternPanel
			, InQty = min (InQty)
			, OutQty = min (OutQty)
	from (
		select	OrderID
				, Size
				, Article
				, PatternPanel
				, FabricPanelCode
				, InQty = min (InQty)
				, OutQty = min (OutQty)
		from (
			select	OrderID
					, Size
					, PatternPanel
					, FabricPanelCode
					, Article
					, PatternCode
					, InQty = sum (InQty)
					, OutQty = sum (OutQty)
			from #BundleInOutQty{subprocessIDt}
			group by OrderID, Size, Article, PatternPanel, FabricPanelCode, PatternCode
		) sumbas
		group by OrderID, Size, Article, PatternPanel, FabricPanelCode
	) minCutpart
	group by OrderID, Size, Article, PatternPanel
) minFabricPanelCode
group by OrderID, Size, Article
";
                }

                sqlcmd += $@"
-- Result Data --
--	 *	3.	最終算出每張訂單目前可完成的成衣件數
select	OrderID = cbs.OrderID
		, cbs.Article
		, cbs.Sizecode
		, QtyBySet = cbs.QtyBySet
		, QtyBySubprocess = cbs.QtyBySubprocess
		, InQtyBySet = case when {isMorethenOrderQty} = 1 then sub.InQty
						when sub.InQty>oq.qty then oq.qty
						else sub.InQty
						end
		, OutQtyBySet = case when {isMorethenOrderQty} = 1 then sub.OutQty
						when sub.OutQty>oq.qty then oq.qty
						else sub.OutQty
						end
		, InQtyByPcs
		, OutQtyByPcs
into #QtyBySetPerSubprocess{subprocessIDt}
from #CutpartBySet{subprocessIDt} cbs
left join Order_Qty oq  WITH (NOLOCK) on oq.id = cbs.OrderID and oq.SizeCode = cbs.SizeCode and oq.Article = cbs.Article
left join #FinalQtyBySet{subprocessIDt} sub on cbs.Orderid = sub.Orderid and cbs.Sizecode = sub.size and cbs.Article = sub.Article
outer apply (
	select	InQtyByPcs = sum (isnull (bunIO.OriInQty, 0))
			, OutQtyByPcs = sum (isnull (bunIO.OriOutQty, 0))
	from #BundleInOutQty{subprocessIDt} bunIO
	where cbs.OrderID = bunIO.OrderID and cbs.Sizecode = bunIO.Size and cbs.Article = bunIO.Article
) IOQtyPerPcs
";
                if (bySP)
                {
                    sqlcmd += $@"
select OrderID, InQtyBySet = sum (InQty), OutQtyBySet = sum (OutQty)
into #{subprocessIDt}
from(
	select OrderID, SizeCode, InQty = min (InQtyBySet), OutQty = min (OutQtyBySet)
	from #QtyBySetPerSubprocess{subprocessIDt}	minPatternPanel
	group by OrderID, SizeCode
) minArticle
group by OrderID
";
                }
            }

            return sqlcmd;
        }
    }
}
