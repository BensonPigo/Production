﻿using System;
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
                MyUtility.Check.Empty(this.dateBuyerDelivery.Value1) &&
                MyUtility.Check.Empty(this.dateCutOffDate.Value1) &&
                MyUtility.Check.Empty(this.datePlanDate.Value1) &&
                (MyUtility.Check.Empty(this.txtSPNoStart.Text) || MyUtility.Check.Empty(this.txtSPNoEnd.Text)))
            {
                MyUtility.Msg.WarningBox("< Buyer Delivery > & < SCI Delivery > & < Cust RQS Date > & < Cut Off Date > & < Plan Date > & < SP# > can't be empty!!");
                return false;
            }

            #region -- 擇一必輸的條件 --
            this.sciDelivery1 = this.dateSCIDelivery.Value1;
            this.sciDelivery2 = this.dateSCIDelivery.Value2;
            this.CustRqsDate1 = this.dateCustRQSDate.Value1;
            this.CustRqsDate2 = this.dateCustRQSDate.Value2;
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
inner join dbo.Order_TmsCost bb on bb.id = aa.orderid
inner join dbo.ArtworkType cc on cc.id = bb.ArtworkTypeID
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
            sqlCmd.Append(string.Format(@"

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
                                   from dbo.View_Order_Artworks v 
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
                                from dbo.View_Order_Artworks v 
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
                                from dbo.View_Order_Artworks v 
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
-----------------------------------------------------------------------------------------------------------------------------------------------------------
------------↓計算累計成衣件數
-----準備兩個累積
Select DISTINCT 
    [Cut Ref#] = b.CutRef,
    [M] = b.MDivisionid,
    [Factory] = o.FtyGroup,
    [SP] = b.Orderid, 
	b.article, 
    [Comb] = b.PatternPanel,
	b.FabricPanelCode
	,b.ID 
into #tmp_Orders
from Bundle b WITH (NOLOCK)  
inner join orders o WITH (NOLOCK) on o.Id = b.OrderId 
inner join Order_EachCons oe WITH (NOLOCK) on oe.id = o.poid and oe.FabricPanelCode = b.FabricPanelCode
inner join Order_BOF bof WITH (NOLOCK) on bof.Id = oe.Id and bof.FabricCode = oe.FabricCode
where 1=1
and o.ID in (select distinct orderid from #cte) 
and bof.kind != 0 

Select DISTINCT
    [Bundleno] = bd.BundleNo,
    b.[Cut Ref#] ,
    b.[M], 
	b.[Factory],
    b.[SP],
    SubProcessId = s.Id,
	b.article,
    [Size] = bd.SizeCode,
    b.[Comb] ,
	b.FabricPanelCode,
    bd.PatternCode,
    bd.Qty,
    bio.InComing,
    bio.OutGoing 
into #tmp
from #tmp_Orders b WITH (NOLOCK)  
inner join Bundle_Detail bd WITH (NOLOCK) on bd.Id = b.Id
left join Bundle_Detail_Art bda WITH (NOLOCK) on bda.Id = bd.Id and bda.Bundleno = bd.Bundleno
inner join SubProcess s WITH (NOLOCK) on (s.IsRFIDDefault = 1 or s.Id = bda.SubprocessId) 
left join BundleInOut bio WITH (NOLOCK) on bio.Bundleno=bd.Bundleno and bio.SubProcessId = s.Id
where 1=1

drop table #tmp_Orders
-----------------------------------------------------------------------------------------------------------------------------------------------------------
select [M],[Factory],[SP],[Subprocessid],article,[Size],[Comb],FabricPanelCode,PatternCode,accuQty = sum(iif(InComing is null ,0,Qty))
into #tmp2
from #tmp
group by [M],[Factory],[SP],[Subprocessid],article,[Size],[Comb],FabricPanelCode,PatternCode

select [M],[Factory],[SP],[Subprocessid],article,[Size],[Comb],FabricPanelCode,accuQty = min(accuQty)
into #tmp3
from #tmp2
group by [M],[Factory],[SP],[Subprocessid],article,[Size],[Comb],FabricPanelCode


select [M],[Factory],[SP],[Subprocessid],article,[Size],accuQty = min(accuQty)
into #tmp4
from #tmp3
group by [M],[Factory],[SP],[Subprocessid],article,[Size]

select [M],[Factory],[SP],[Subprocessid],accuQty = sum(accuQty)
into #tmpin
from #tmp4
group by [M],[Factory],[SP],[Subprocessid]
------
select [M],[Factory],[SP],[Subprocessid],article,[Size],[Comb],FabricPanelCode,PatternCode,Qty = iif(OutGoing is null ,0,Qty)
into #tmpout1
from #tmp

select [M],[Factory],[SP],[Subprocessid],article,[Size],[Comb],FabricPanelCode,PatternCode,accuQty = sum(Qty)
into #tmpout2
from #tmpout1
group by [M],[Factory],[SP],[Subprocessid],article,[Size],[Comb],FabricPanelCode,PatternCode

select [M],[Factory],[SP],[Subprocessid],article,[Size],[Comb],FabricPanelCode,accuQty = min(accuQty)
into #tmpout3
from #tmpout2
group by [M],[Factory],[SP],[Subprocessid],article,[Size],[Comb],FabricPanelCode

select [M],[Factory],[SP],[Subprocessid],article,[Size],accuQty = min(accuQty)
into #tmpout4
from #tmpout3
group by [M],[Factory],[SP],[Subprocessid],article,[Size]

select [M],[Factory],[SP],[Subprocessid],accuQty = sum(accuQty)
into #tmpout
from #tmpout4
group by [M],[Factory],[SP],[Subprocessid]
-----------------------------------------------------------------------------------------------------------------------------------------------------------

select sod.OrderID ,Max(so.OutputDate) LastSewnDate
into #imp_LastSewnDate
from SewingOutput so WITH (NOLOCK) 
inner join SewingOutput_Detail sod WITH (NOLOCK) on so.ID = sod.ID
inner join #cte t on sod.OrderID = t.OrderID 
group by sod.OrderID 

select o.SubProcessId, o.SP, o.Factory, [AccuOutGo] = iif (AccuQty > t.Qty, t.Qty, AccuQty)  
into #tmp_Out
from #tmpout o
inner join #cte t on o.SP = t.OrderID and o.Factory = t.FactoryID

select i.SubProcessId, i.SP, i.Factory, [AccuInCome] = iif (AccuQty > t.Qty, t.Qty, AccuQty) 
into #tmp_In
from #tmpin i
inner join #cte t on i.SP = t.OrderID and i.Factory = t.FactoryID

select SP,Factory,ct = sum(xxx.Accusubprocesqty)
into #tmp_inoutcount
from(
	select #tmpout.SP, #tmpout.Factory,[Accusubprocesqty] = iif(#tmpin.AccuQty > 0, 1, 0)+
								iif(#tmpout.AccuQty > 0, 1, 0)
	from #tmpin,#tmpout
	inner join #cte t on #tmpout.SP = t.OrderID and #tmpout.Factory = t.FactoryID
	where #tmpin.SP = t.OrderID 
	and #tmpin.Factory = t.FactoryID
	and #tmpin.SubProcessId in('Emb','BO','PRT','AT','PAD-PRT','SUBCONEMB','HT','Loading','SORTING')
	and #tmpout.SP = t.OrderID 
	and #tmpout.Factory = t.FactoryID
	and #tmpout.SubProcessId = #tmpin.SubProcessId
)xxx
group by SP,Factory


select SP,Factory,chksubprocesqty = sum(xxx.Accusubprocesqty)
into #tmp_subprocessqty
from(
	select #tmpout.SP, #tmpout.Factory,[Accusubprocesqty] = iif(iif(#tmpin.AccuQty > t.Qty, t.Qty, #tmpin.AccuQty)>=t.Qty,1,0)+
								iif(iif(#tmpout.AccuQty > t.Qty, t.Qty, #tmpout.AccuQty)>=t.Qty,1,0)
	from #tmpin,#tmpout
	inner join #cte t on #tmpout.SP = t.OrderID and #tmpout.Factory = t.FactoryID
	where #tmpin.SP = t.OrderID 
	and #tmpin.Factory = t.FactoryID
	and #tmpin.SubProcessId in('Emb','BO','PRT','AT','PAD-PRT','SUBCONEMB','HT','Loading','SORTING')
	and #tmpout.SP = t.OrderID 
	and #tmpout.Factory = t.FactoryID
	and #tmpout.SubProcessId = #tmpin.SubProcessId
)xxx
group by SP,Factory
-----------------------------------------------------------------------------------------------------------------------------------------------------------
----------↑計算累計成衣件數
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
       , [RFID Cut Qty] = isnull (CutQty.AccuOutGo, 0)
       , [RFID Loading Qty] = isnull (loading.AccuInCome,0)             
       , [RFID Emb Farm In Qty] = isnull (Embin.AccuInCome, 0)
       , [RFID Emb Farm Out Qty] = isnull (Embout.AccuOutGo, 0)
       , [RFID Bond Farm In Qty] = isnull (Bondin.AccuInCome, 0)
       , [RFID Bond Farm Out Qty] = isnull (Bondout.AccuOutGo, 0)
       , [RFID Print Farm In Qty] = isnull (Printin.AccuInCome, 0)
       , [RFID Print Farm Out Qty] = isnull (Printout.AccuOutGo, 0)
       , [RFID AT Farm In Qty] = isnull (ATin.AccuInCome, 0)
       , [RFID AT Farm Out Qty] = isnull (ATout.AccuOutGo, 0)
       , [RFID Pad Print Farm In Qty] = isnull (PadPrintin.AccuInCome, 0)
       , [RFID Pad Print Farm Out Qty] = isnull (PadPrintout.AccuOutGo, 0)
       , [RFID Emboss Farm In Qty] = isnull (Embossin.AccuInCome, 0)
       , [RFID Emboss Farm Out Qty] = isnull (Embossout.AccuOutGo, 0)
       , [RFID HT Farm In Qty] = isnull (htin.AccuInCome, 0)
       , [RFID HT Farm Out Qty] = isnull (htout.AccuOutGo, 0)
        , SubProcessStatus=
			case when t.Junk = 1 then null
				 when subprocessqty.chksubprocesqty = inoutcount.ct and inoutcount.ct >0 then 'Y'
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
"));
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
    from Reason 
    where ReasonTypeID = 'Order_BuyerDelivery' 
          and ID = t.KPIChangeReason 
          and t.KPIChangeReason != '' 
          and t.KPIChangeReason is not null 
) KPIChangeReason 
left join #tmp_Out CutQty on CutQty.SP = t.OrderID and CutQty.Factory = t.FactoryID and CutQty.SubProcessId = 'SORTING'
left join #tmp_In loading on loading.SP = t.OrderID and loading.Factory = t.FactoryID and loading.SubProcessId = 'loading'
left join #tmp_In Embin on Embin.SP = t.OrderID and Embin.Factory = t.FactoryID and Embin.SubProcessId = 'Emb'
left join #tmp_Out Embout on Embout.SP = t.OrderID and Embout.Factory = t.FactoryID and Embout.SubProcessId = 'Emb'
left join #tmp_In Bondin on Bondin.SP = t.OrderID and Bondin.Factory = t.FactoryID and Bondin.SubProcessId = 'BO'
left join #tmp_Out Bondout on Bondout.SP = t.OrderID and Bondout.Factory = t.FactoryID and Bondout.SubProcessId = 'BO'
left join #tmp_In Printin on Printin.SP = t.OrderID and Printin.Factory = t.FactoryID and Printin.SubProcessId = 'PRT'
left join #tmp_Out Printout on Printout.SP = t.OrderID and Printout.Factory = t.FactoryID and Printout.SubProcessId = 'PRT'
left join #tmp_In ATin on ATin.SP = t.OrderID and ATin.Factory = t.FactoryID and ATin.SubProcessId = 'AT'
left join #tmp_Out ATout on ATout.SP = t.OrderID and ATout.Factory = t.FactoryID and ATout.SubProcessId = 'AT'
left join #tmp_In PadPrintin on PadPrintin.SP = t.OrderID and PadPrintin.Factory = t.FactoryID and PadPrintin.SubProcessId = 'PAD-PRT'
left join #tmp_Out PadPrintout on PadPrintout.SP = t.OrderID and PadPrintout.Factory = t.FactoryID and PadPrintout.SubProcessId = 'PAD-PRT'
left join #tmp_In Embossin on Embossin.SP = t.OrderID and Embossin.Factory = t.FactoryID and Embossin.SubProcessId = 'SUBCONEMB'
left join #tmp_Out Embossout on Embossout.SP = t.OrderID and Embossout.Factory = t.FactoryID and Embossout.SubProcessId = 'SUBCONEMB'
left join #tmp_In htin on htin.SP = t.OrderID and htin.Factory = t.FactoryID and htin.SubProcessId = 'HT'
left join #tmp_Out htout on htout.SP = t.OrderID and htout.Factory = t.FactoryID and htout.SubProcessId = 'HT' 
left join #tmp_inoutcount inoutcount on inoutcount.SP = t.Orderid and inoutcount.Factory = t.FactoryID
left join #tmp_subprocessqty subprocessqty on subprocessqty.SP = t.Orderid and subprocessqty.Factory = t.FactoryID
outer apply(
	select StandardOutput =stuff((
		  select distinct concat(',',ComboType,':',StandardOutput)
		  from [SewingSchedule]
		  where orderid = t.OrderID 
		  for xml path('')
	  ),1,1,'')
)StandardOutput
outer apply(
	select Artwork =stuff((	
		select concat('+',ArtworkTypeID)
		from(
			select distinct v.ArtworkTypeID
			from dbo.View_Order_Artworks v 
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
            sqlCmd.Append(@" ;DROP TABLE #imp_LastSewnDate, #tmp_Out, #tmp_In, #tmp_inoutcount, #tmp_subprocessqty, #cte2, #cte,#tmp,#tmp2,#tmp3,#tmp4,#tmpout1,#tmpout2,#tmpout3,#tmpout4,#tmpin,#tmpout");
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
inner join dbo.Order_TmsCost bb on bb.id = aa.orderid
inner join dbo.ArtworkType cc on cc.id = bb.ArtworkTypeID
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

            #region SummaryBy Acticle/Size
            sqlCmd.Append(@"
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
                                   from dbo.View_Order_Artworks v 
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
                                from dbo.View_Order_Artworks v 
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
                                from dbo.View_Order_Artworks v 
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
-----------------------------------------------------------------------------------------------------------------------------------------------------------
----------↓計算累計成衣件數
---準備兩個累積 
Select DISTINCT 
    [Cut Ref#] = b.CutRef,
    [M] = b.MDivisionid,
    [Factory] = o.FtyGroup,
    [SP] = b.Orderid, 
	b.article, 
    [Comb] = b.PatternPanel,
	b.FabricPanelCode
	,b.ID 
into #tmp_Orders
from Bundle b WITH (NOLOCK)  
inner join orders o WITH (NOLOCK) on o.Id = b.OrderId 
inner join Order_EachCons oe WITH (NOLOCK) on oe.id = o.poid and oe.FabricPanelCode = b.FabricPanelCode
inner join Order_BOF bof WITH (NOLOCK) on bof.Id = oe.Id and bof.FabricCode = oe.FabricCode
where 1=1
and o.ID in (select distinct orderid from #cte) 
and bof.kind != 0 

Select DISTINCT
    [Bundleno] = bd.BundleNo,
    b.[Cut Ref#] ,
    b.[M], 
	b.[Factory],
    b.[SP],
    SubProcessId = s.Id,
	b.article,
    [Size] = bd.SizeCode,
    b.[Comb] ,
	b.FabricPanelCode,
    bd.PatternCode,
    bd.Qty,
    bio.InComing,
    bio.OutGoing 
into #tmp
from #tmp_Orders b WITH (NOLOCK)  
inner join Bundle_Detail bd WITH (NOLOCK) on bd.Id = b.Id
left join Bundle_Detail_Art bda WITH (NOLOCK) on bda.Id = bd.Id and bda.Bundleno = bd.Bundleno
inner join SubProcess s WITH (NOLOCK) on (s.IsRFIDDefault = 1 or s.Id = bda.SubprocessId) 
left join BundleInOut bio WITH (NOLOCK) on bio.Bundleno=bd.Bundleno and bio.SubProcessId = s.Id
where 1=1

drop table #tmp_Orders
-----------------------------------------------------------------------------------------------------------------------------------------------------------

select [M],[Factory],[SP],[Subprocessid],article,[Size],[Comb],FabricPanelCode,PatternCode,accuQty = sum(iif(InComing is null ,0,Qty))
into #tmp2
from #tmp
group by [M],[Factory],[SP],[Subprocessid],article,[Size],[Comb],FabricPanelCode,PatternCode

select [M],[Factory],[SP],[Subprocessid],article,[Size],[Comb],FabricPanelCode,accuQty = min(accuQty)
into #tmp3
from #tmp2
group by [M],[Factory],[SP],[Subprocessid],article,[Size],[Comb],FabricPanelCode


select [M],[Factory],[SP],[Subprocessid],article,[Size],accuQty = min(accuQty)
into #tmp4
from #tmp3
group by [M],[Factory],[SP],[Subprocessid],article,[Size]

select [M],[Factory],[SP],[Subprocessid],article,[Size],accuQty = sum(accuQty)
into #tmpin
from #tmp4
group by [M],[Factory],[SP],[Subprocessid],article,[Size]
------
select [M],[Factory],[SP],[Subprocessid],article,[Size],[Comb],FabricPanelCode,PatternCode,Qty = iif(OutGoing is null ,0,Qty)
into #tmpout1
from #tmp

select [M],[Factory],[SP],[Subprocessid],article,[Size],[Comb],FabricPanelCode,PatternCode,accuQty = sum(Qty)
into #tmpout2
from #tmpout1
group by [M],[Factory],[SP],[Subprocessid],article,[Size],[Comb],FabricPanelCode,PatternCode

select [M],[Factory],[SP],[Subprocessid],article,[Size],[Comb],FabricPanelCode,accuQty = min(accuQty)
into #tmpout3
from #tmpout2
group by [M],[Factory],[SP],[Subprocessid],article,[Size],[Comb],FabricPanelCode


select [M],[Factory],[SP],[Subprocessid],article,[Size],accuQty = min(accuQty)
into #tmpout4
from #tmpout3
group by [M],[Factory],[SP],[Subprocessid],article,[Size]

select [M],[Factory],[SP],[Subprocessid],article,[Size],accuQty = sum(accuQty)
into #tmpout
from #tmpout4
group by [M],[Factory],[SP],[Subprocessid],article,[Size]
-----------------------------------------------------------------------------------------------------------------------------------------------------------

select sod.OrderID ,Max(so.OutputDate) LastSewnDate
into #imp_LastSewnDate
from SewingOutput so WITH (NOLOCK) 
inner join SewingOutput_Detail sod WITH (NOLOCK) on so.ID = sod.ID
inner join #cte t on sod.OrderID = t.OrderID 
group by sod.OrderID 

select o.SubProcessId, o.SP, o.Factory, o.Article, o.Size, [AccuOutGo] = iif (AccuQty > t.Qty, t.Qty, AccuQty)  
into #tmp_Out
from #tmpout o
inner join #cte t on o.SP = t.OrderID and o.Factory = t.FactoryID and o.Article = t.Article and o.Size = t.SizeCode

select i.SubProcessId, i.SP, i.Factory, i.Article, i.Size, [AccuInCome] = iif (AccuQty > t.Qty, t.Qty, AccuQty) 
into #tmp_In
from #tmpin i
inner join #cte t on i.SP = t.OrderID and i.Factory = t.FactoryID and i.Article = t.Article and i.Size = t.SizeCode

select SP,Factory,Article,Size,ct = sum(xxx.Accusubprocesqty)
into #tmp_inoutcount
from(
	select #tmpout.SP, #tmpout.Factory,#tmpout.Article,#tmpout.Size
			,[Accusubprocesqty] = iif(#tmpin.AccuQty > 0, 1, 0)+
									iif(#tmpout.AccuQty > 0, 1, 0)
	from #tmpin,#tmpout
	inner join #cte t on #tmpout.SP = t.OrderID and #tmpout.Factory = t.FactoryID and #tmpout.Article = t.Article and #tmpout.Size = t.SizeCode
	where #tmpin.SP = t.OrderID 
	and #tmpin.Factory = t.FactoryID
	and #tmpin.SubProcessId in('Emb','BO','PRT','AT','PAD-PRT','SUBCONEMB','HT','Loading','SORTING')
	and #tmpout.SP = t.OrderID 
	and #tmpout.Factory = t.FactoryID
	and #tmpout.SubProcessId = #tmpin.SubProcessId
	and #tmpin.Article = t.Article and #tmpin.Size = t.SizeCode
	and #tmpout.Article = t.Article and #tmpout.Size = t.SizeCode
)xxx
group by SP,Factory,Article,Size
	 
select SP,Factory,Article,Size,chksubprocesqty = sum(xxx.Accusubprocesqty)
into #tmp_subprocessqty
from(
	select #tmpout.SP, #tmpout.Factory,#tmpout.Article,#tmpout.Size
		,[Accusubprocesqty] = iif(iif(#tmpin.AccuQty > t.Qty, t.Qty, #tmpin.AccuQty)>=t.Qty,1,0)+
								iif(iif(#tmpout.AccuQty > t.Qty, t.Qty, #tmpout.AccuQty)>=t.Qty,1,0)
	from #tmpin,#tmpout
	inner join #cte t on #tmpout.SP = t.OrderID and #tmpout.Factory = t.FactoryID and #tmpout.Article = t.Article and #tmpout.Size = t.SizeCode
	where #tmpin.SP = t.OrderID 
	and #tmpin.Factory = t.FactoryID
	and #tmpin.SubProcessId in('Emb','BO','PRT','AT','PAD-PRT','SUBCONEMB','HT','Loading','SORTING')
	and #tmpout.SP = t.OrderID 
	and #tmpout.Factory = t.FactoryID
	and #tmpout.SubProcessId = #tmpin.SubProcessId
	and #tmpin.Article = t.Article and #tmpin.Size = t.SizeCode
	and #tmpout.Article = t.Article and #tmpout.Size = t.SizeCode
)xxx
group by SP,Factory,Article,Size
-----------------------------------------------------------------------------------------------------------------------------------------------------------
----------↑計算累計成衣件數
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
       , [RFID Cut Qty] = isnull (CutQty.AccuOutGo, 0)
       , [RFID Loading Qty] = isnull (loading.AccuInCome,0)
       , [RFID Emb Farm In Qty] = isnull (Embin.AccuInCome, 0)
       , [RFID Emb Farm Out Qty] = isnull (Embout.AccuOutGo, 0)
       , [RFID Bond Farm In Qty] = isnull (Bondin.AccuInCome, 0)		
       , [RFID Bond Farm Out Qty] = isnull (Bondout.AccuOutGo, 0)
       , [RFID Print Farm In Qty] = isnull (Printin.AccuInCome, 0)
       , [RFID Print Farm Out Qty] = isnull (Printout.AccuOutGo, 0)
       , [RFID AT Farm In Qty] = isnull (ATin.AccuInCome, 0)
       , [RFID AT Farm Out Qty] = isnull (ATout.AccuOutGo, 0)
       , [RFID Pad Print Farm In Qty] = isnull (PadPrintin.AccuInCome, 0)
       , [RFID Pad Print Farm Out Qty] = isnull (PadPrintout.AccuOutGo, 0)
       , [RFID Emboss Farm In Qty] = isnull (Embossin.AccuInCome, 0)
       , [RFID Emboss Farm Out Qty] = isnull (Embossout.AccuOutGo, 0)
       , [RFID HT Farm In Qty] = isnull (htin.AccuInCome, 0)
       , [RFID HT Farm Out Qty] = isnull (htout.AccuOutGo, 0)
        , SubProcessStatus=
			case when t.Junk = 1 then null
				 when subprocessqty.chksubprocesqty = inoutcount.ct and inoutcount.ct >0 then 'Y'
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
    from Reason 
    where ReasonTypeID = 'Order_BuyerDelivery' 
          and ID = t.KPIChangeReason 
          and t.KPIChangeReason != '' 
          and t.KPIChangeReason is not null 
) KPIChangeReason 
left join #tmp_Out CutQty on CutQty.SP = t.OrderID and CutQty.Factory = t.FactoryID and CutQty.Article = t.Article and CutQty.Size = t.SizeCode and CutQty.SubProcessId = 'SORTING'
left join #tmp_In loading on loading.SP = t.OrderID and loading.Factory = t.FactoryID and loading.Article = t.Article and loading.Size = t.SizeCode and loading.SubProcessId = 'loading'
left join #tmp_In Embin on Embin.SP = t.OrderID and Embin.Factory = t.FactoryID and Embin.Article = t.Article and Embin.Size = t.SizeCode and Embin.SubProcessId = 'Emb'
left join #tmp_Out Embout on Embout.SP = t.OrderID and Embout.Factory = t.FactoryID and Embout.Article = t.Article and Embout.Size = t.SizeCode and Embout.SubProcessId = 'Emb'
left join #tmp_In Bondin on Bondin.SP = t.OrderID and Bondin.Factory = t.FactoryID and Bondin.Article = t.Article and Bondin.Size = t.SizeCode and Bondin.SubProcessId = 'BO'
left join #tmp_Out Bondout on Bondout.SP = t.OrderID and Bondout.Factory = t.FactoryID and Bondout.Article = t.Article and Bondout.Size = t.SizeCode and Bondout.SubProcessId = 'BO'
left join #tmp_In Printin on Printin.SP = t.OrderID and Printin.Factory = t.FactoryID and Printin.Article = t.Article and Printin.Size = t.SizeCode and Printin.SubProcessId = 'PRT'
left join #tmp_Out Printout on Printout.SP = t.OrderID and Printout.Factory = t.FactoryID and Printout.Article = t.Article and Printout.Size = t.SizeCode and Printout.SubProcessId = 'PRT'
left join #tmp_In ATin on ATin.SP = t.OrderID and ATin.Factory = t.FactoryID and ATin.Article = t.Article and ATin.Size = t.SizeCode and ATin.SubProcessId = 'AT'
left join #tmp_Out ATout on ATout.SP = t.OrderID and ATout.Factory = t.FactoryID and ATout.Article = t.Article and ATout.Size = t.SizeCode and ATout.SubProcessId = 'AT'
left join #tmp_In PadPrintin on PadPrintin.SP = t.OrderID and PadPrintin.Factory = t.FactoryID and PadPrintin.Article = t.Article and PadPrintin.Size = t.SizeCode and PadPrintin.SubProcessId = 'PAD-PRT'
left join #tmp_Out PadPrintout on PadPrintout.SP = t.OrderID and PadPrintout.Factory = t.FactoryID and PadPrintout.Article = t.Article and PadPrintout.Size = t.SizeCode and PadPrintout.SubProcessId = 'PAD-PRT'
left join #tmp_In Embossin on Embossin.SP = t.OrderID and Embossin.Factory = t.FactoryID and Embossin.Article = t.Article and Embossin.Size = t.SizeCode and Embossin.SubProcessId = 'SUBCONEMB'
left join #tmp_Out Embossout on Embossout.SP = t.OrderID and Embossout.Factory = t.FactoryID and Embossout.Article = t.Article and Embossout.Size = t.SizeCode and Embossout.SubProcessId = 'SUBCONEMB'
left join #tmp_In htin on htin.SP = t.OrderID and htin.Factory = t.FactoryID and htin.Article = t.Article and htin.Size = t.SizeCode and htin.SubProcessId = 'HT'
left join #tmp_Out htout on htout.SP = t.OrderID and htout.Factory = t.FactoryID and htout.Article = t.Article and htout.Size = t.SizeCode and htout.SubProcessId = 'HT'
left join #tmp_inoutcount inoutcount on inoutcount.SP = t.OrderID and inoutcount.Factory = t.FactoryID and inoutcount.Article = t.Article and inoutcount.Size = t.SizeCode
left join #tmp_subprocessqty subprocessqty on subprocessqty.SP = t.OrderID and subprocessqty.Factory = t.FactoryID and subprocessqty.Article = t.Article and subprocessqty.Size = t.SizeCode 
outer apply(
	select SewingLineID =stuff((
		  select distinct concat(',',ssd.SewingLineID)
		  from [SewingSchedule] ss
		  inner join SewingSchedule_Detail ssd on ssd.id = ss.id
		  where ssd.orderid = t.OrderID and ssd.Article = t.Article and ssd.SizeCode = t.SizeCode
		  for xml path('')
	  ),1,1,'')
)SewingSchedule
outer apply(
	select Inline = MIN(ss.Inline),Offline = max(SS.Offline)
	from [SewingSchedule] ss
	inner join SewingSchedule_Detail ssd on ssd.id = ss.id
	where ssd.orderid = t.OrderID and ssd.Article = t.Article and ssd.SizeCode = t.SizeCode
)SewingSchedule2
outer apply(
	select StandardOutput =stuff((
		  select distinct concat(',',ssd.ComboType,':',StandardOutput)
		  from [SewingSchedule] ss
		  inner join SewingSchedule_Detail ssd on ssd.id = ss.id
		  where ssd.orderid = t.OrderID and ssd.Article = t.Article and ssd.SizeCode = t.SizeCode
		  for xml path('')
	  ),1,1,'')
)StandardOutput
outer apply(
	select Artwork =stuff((	
		select concat('+',ArtworkTypeID)
		from(
			select distinct v.ArtworkTypeID
			from dbo.View_Order_Artworks v 
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

            sqlCmd.Append(string.Format(@" order by {0}", this.orderby));
            sqlCmd.Append(@";drop table #imp_LastSewnDate, #tmp_Out, #tmp_In, #tmp_inoutcount, #tmp_subprocessqty ,#cte2,#cte,#tmp,#tmp2,#tmp3,#tmp4,#tmpout1,#tmpout2,#tmpout3,#tmpout4,#tmpin,#tmpout");
            if (this.isArtwork)
            {
                sqlCmd.Append(@";drop table #rawdata_tmscost,#tmscost_pvt");
            }
            #endregion

            return sqlCmd;
        }
    }
}
