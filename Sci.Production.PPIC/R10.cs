using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using Sci.Data;
using Ict;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// R10
    /// </summary>
    public partial class R10 : Win.Tems.PrintForm
    {
        private string strSPStart;
        private string strSPEnd;
        private string strM;
        private string strFactory;
        private string dateBuyerDeliveryStart;
        private string dateBuyerDeliveryEnd;
        private string dateSewingOutput;
        private DataTable resultDt;

        /// <summary>
        /// R10
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public R10(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.txtfactory.Text = Env.User.Factory;
            this.txtMdivision.Text = Env.User.Keyword;
            this.dateBoxSewingOutput.Value = DateTime.Today.AddDays(-1);
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            #region get input data
            this.strSPStart = this.textSPStart.Text;
            this.strSPEnd = this.textSPEnd.Text;
            this.strM = this.txtMdivision.Text;
            this.strFactory = this.txtfactory.Text;
            this.dateBuyerDeliveryStart = (this.dateRangeBuyerDelivery.Value1 == null) ? string.Empty : ((DateTime)this.dateRangeBuyerDelivery.Value1).ToString("yyyy/MM/dd");
            this.dateBuyerDeliveryEnd = (this.dateRangeBuyerDelivery.Value2 == null) ? string.Empty : ((DateTime)this.dateRangeBuyerDelivery.Value2).ToString("yyyy/MM/dd");
            this.dateSewingOutput = (this.dateBoxSewingOutput.Value == null) ? string.Empty : ((DateTime)this.dateBoxSewingOutput.Value).ToString("yyyy/MM/dd");
            #endregion
            #region check input data
            if (this.dateBuyerDeliveryStart.Empty() && this.dateBuyerDeliveryEnd.Empty() && this.strSPStart.Empty() && this.strSPEnd.Empty())
            {
                MyUtility.Msg.WarningBox("Buyer Delivery Date and SP# can't all be empty.");
                return false;
            }

            if (this.dateSewingOutput.Empty())
            {
                MyUtility.Msg.WarningBox("Sewing Output Date can't be empty.");
                return false;
            }
            #endregion
            return true;
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            #region SQL Parameter
            List<SqlParameter> listSqlParameter = new List<SqlParameter>();
            listSqlParameter.Add(new SqlParameter("@SPStart", this.strSPStart));
            listSqlParameter.Add(new SqlParameter("@SPEnd", this.strSPEnd));
            listSqlParameter.Add(new SqlParameter("@MDivision", this.strM));
            listSqlParameter.Add(new SqlParameter("@Factory", this.strFactory));
            listSqlParameter.Add(new SqlParameter("@BDDateStart", this.dateBuyerDeliveryStart));
            listSqlParameter.Add(new SqlParameter("@BDDateEnd", this.dateBuyerDeliveryEnd));
            listSqlParameter.Add(new SqlParameter("@SODate", this.dateSewingOutput));
            #endregion
            #region SQL where filte
            /*
             * Buyer Delivery
             */
            string str_BuyerDelivery = string.Empty;
            if (!this.dateBuyerDeliveryStart.Empty() && !this.dateBuyerDeliveryEnd.Empty())
            {
                str_BuyerDelivery = "and O.BuyerDelivery between @BDDateStart and @BDDateEnd";
            }
            else if (!this.dateBuyerDeliveryStart.Empty() && this.dateBuyerDeliveryEnd.Empty())
            {
                str_BuyerDelivery = "and @BDDateStart <= O.BuyerDelivery";
            }
            else if (this.dateBuyerDeliveryStart.Empty() && !this.dateBuyerDeliveryEnd.Empty())
            {
                str_BuyerDelivery = "and O.BuyerDelivery <= @BDDateStart";
            }

            /*
             * SP#
             */
            string str_SPnum = string.Empty;
            if (!this.strSPStart.Empty() && !this.strSPEnd.Empty())
            {
                str_SPnum = "and O.ID between @SPStart and @SPEnd";
            }
            else if (!this.strSPStart.Empty() && this.strSPEnd.Empty())
            {
                str_SPnum = "and @SPStart <= O.ID";
            }
            else if (this.strSPStart.Empty() && !this.strSPEnd.Empty())
            {
                str_SPnum = "and O.ID <= @SPEnd";
            }

            Dictionary<string, string> sqlFilte = new Dictionary<string, string>();
            sqlFilte.Add("DaysSinceInline_Factory", this.strFactory.Empty() ? string.Empty : "and _WH.FactoryID = @Factory");
            sqlFilte.Add("DaysToDelivery_Factory", this.strFactory.Empty() ? string.Empty : "and _H.FactoryID = @Factory");
            sqlFilte.Add("_BuyerDelivery", str_BuyerDelivery);
            sqlFilte.Add("_SPnum", str_SPnum);
            sqlFilte.Add("_M", this.strM.Empty() ? string.Empty : "and O.MDivisionID = @MDivision");
            sqlFilte.Add("_Factory", this.strFactory.Empty() ? string.Empty : "and O.FactoryID = @Factory");
            #endregion
            #region SQL Command
            string whereIncludeCancelOrder = this.chkIncludeCancelOrder.Checked ? string.Empty : " and (o.Junk=0 or (o.Junk=1 and o.NeedProduction=1)) ";
            string sqlCmd = string.Format(
                @"
select	O.ID
        ,[Cancel] = IIF(o.Junk=1,'Y','N')
		, Category = case O.Category 
						when 'B' then 'Bulk'
						when 'S' then 'Sample' 
						else O.Category 
					 end
		, EType = EType.value
		, O.FactoryID
		, O.BrandID
		, O.StyleID
		, O.SeasonID
		, O.CdCodeID
	    , sty.CDCodeNew
	    , sty.ProductType
	    , sty.FabricType
	    , sty.Lining
	    , sty.Gender
	    , sty.Construction
		, O.CPU
		, CPU_EType = IIF (O.StyleUnit = 'sets', Convert(varchar, (O.CPU * isnull (SL.Rate, 0) / 100)), '0')
		, O.Qty
		, O.SciDelivery
		, O.BuyerDelivery
		, Multi_Delivery = (select IIF (count (*) > 1, 'Y', 'N')
							from Order_QtyShip
							where ID = O.ID)
		, QtyDelivery = stuff ((select CHAR(10) + tmp.value
								from (
									select value = CONCAT (_OQ.Qty, ', ', _OQ.ShipmodeID, ', ', _OQ.BuyerDelivery)
									from Order_QtyShip _OQ
									where ID = O.ID
								) tmp
								for xml path('')), 1, 1, '')
		, O.SewInLine
		, O.SewOffLine
		, LineNum = stuff ((select CHAR (10) + tmp.value
                            from (
                                select value = CONCAT(_SS.SewingLineID, '  (', CONVERT(varchar, _SS.Inline, 111), ' ~ ' , CONVERT(varchar, _SS.Offline, 111), ')')
                                from SewingSchedule _SS
                                where _SS.OrderID = O.ID
                            ) tmp
                            for xml path('')), 1, 1, '')
		, SewingComplete = SewingComplete.value
		, PdnDays = IIF (SewingComplete.value = 'Y', '0', Convert(varchar, PdnDays.value))
		-- StdOutput C# 計算
		, StdOutput = '' 
		, DaysSinceInline = IIF (SewingComplete.value = 'Y', '0' , Convert(varchar, DaysSinceInline.value))
		, AccuStdOutput = AccuStdOutput.value
		, AccuActOutput = AccuActOutput.value
		-- VarianceQty = AccuActOutput - AccuStdOutput
		, VarianceQty = IIF (SewingComplete.value = 'Y', 0, AccuActOutput.value - AccuStdOutput.value)
		-- VarianceDays = (AccuActOutput - AccuStdOutput) / StdOutput
		, VarianceDays = '' 
		, DaysToDelivery = IIF (@SODate <= O.BuyerDelivery, DaysToDelivery.value, (0 - DaysToDelivery.value))
		-- DaysNeedForProd C# 計算，需要 nTtlOutPut & nTtlSewDays 做計算
		, DaysNeedForProd = ''
		, nTtlOutPut = nTtlOutPut.value
		, nTtlSewDays = nTtlSewDays.value
		, PostSewingDays = (select isnull (Max (_AT.PostSewingDays), 0)
							from Order_TmsCost _OT
							left join Artworktype _AT on _OT.Seq = _AT.Seq
							where	_OT.ID = O.ID
									and _OT.Price != 0)
		-- Variance = DaysToDelivery - DaysNeedForProd - PostSewingDays
		, Variance = ''
		-- PotentialDelayRisk = '' 則需要在 C# 做額外判斷
		, PotentialDelayRisk = IIF (O.BuyerDelivery < @SODate, '-', '')
		, SMR = dbo.getPass1_ExtNo (O.SMR)
		, MRHandle = dbo.getPass1_ExtNo (O.MRHandle)
from Orders O
left join Style_Location SL on SL.StyleUkey=O.StyleUkey
outer apply (
	select value = SL.Location 
) EType
outer apply (
	select value = IIF (SUM (_SOD.QAQty) >= O.Qty, 'Y', '')
	from SewingOutput_Detail _SOD
	left join SewingOutput _SO on _SOD.ID = _SO.ID
	where	_SOD.OrderId = O.ID
			and _SOD.ComboType = EType.value
) SewingComplete
outer apply (
	select value = SUM (_SS.WorkDay)
	from SewingSchedule _SS
	where	_SS.OrderID = O.ID
			and _SS.ComboType = EType.value
) PdnDays
outer apply (
		select value = count(*)
		from SewingSchedule _SS
		where	_SS.OrderID = O.ID
				and _SS.ComboType = EType.value
) dataCount
outer apply (
    select  Inline = _SS.Inline
            , Offline = _SS.Offline
            , SewingLineID = _SS.SewingLineID
            , APSNo
    from SewingSchedule _SS
    where   _SS.OrderID = O.ID
            and _SS.ComboType = EType.value
            and dataCount.value = 1
) SewingScheduleData
outer apply (
	select value = IIF (dataCount.value > 1, 0
                                           , IIF (O.SewInline <= @SODate, count(*)
                                                                        , 0 - count(*)
                                                 )
                       )
	from WorkHour _WH
	where	_WH.SewingLineID = SewingScheduleData.SewingLineID
			--Factory
			{0}--and _WH.FactoryID = @Factory
			and _WH.Date >= SewingScheduleData.Inline
			--SewingOutputDate
			and _WH.Date <= @SODate
			and _WH.Holiday = 0
) DaysSinceInline
outer apply (
	select value = IIF (SewingComplete.value = 'Y', 0 
												  , (select	IIF (SUM (StdQ) >= O.Qty, O.Qty, SUM (StdQ))
                                                     from dbo.getDailystdq (SewingScheduleData.APSNo) _getStdq
												 	 where _getStdq.Date <= @SODate))
) AccuStdOutput
outer apply (
	select value = isnull (SUM (_SOD.QAQty), 0)
	from SewingOutput _SO
	inner join SewingOutput_Detail _SOD on _SO.ID = _SOD.ID
	where	_SOD.OrderId = O.ID
			and _SOD.ComboType = 'T'
			--SewingOutputDate
			and _SO.OutputDate <= @SODate
) AccuActOutput
outer apply (
	select value = count (*) 
	from Holiday _H
	where	--Holiday 必須排除星期日，星期日另外算
            DATEPART (WEEKDAY, _H.HolidayDate) != 7
			--SewingOutputDate
			and _H.HolidayDate between O.BuyerDelivery and @SODate
			--Factory
			{1}--and _H.FactoryID = @Factory
) Holidays
outer apply (
    select value = DateDiff (Day, O.BuyerDelivery, @SODate) - Holidays.value - dbo.getDateRangeSundayCount (O.BuyerDelivery, @SODate)
) DaysToDelivery
outer apply (
	select value = count (distinct _SO.OutputDate)
	from SewingOutput _SO
	inner join SewingOutput_Detail _SOD on _SO.ID = _SOD.ID
	where	_SOD.OrderId = O.ID
			and _SOD.ComboType = EType.value
) nTtlSewDays 
outer apply (
	select value = ISNULL (SUM (_SOD.QAQty), 0)
	from SewingOutput _SO
	inner join SewingOutput_Detail _SOD on _SO.ID = _SOD.ID
	where	_SOD.OrderId = O.ID
			and _SOD.ComboType = EType.value
) nTtlOutPut
Outer apply (
	SELECT ProductType = r2.Name
		, FabricType = r1.Name
		, Lining
		, Gender
		, Construction = d1.Name
        , s.CDCodeNew
	FROM Style s WITH(NOLOCK)
	left join DropDownList d1 WITH(NOLOCK) on d1.type= 'StyleConstruction' and d1.ID = s.Construction
	left join Reason r1 WITH(NOLOCK) on r1.ReasonTypeID= 'Fabric_Kind' and r1.ID = s.FabricType
	left join Reason r2 WITH(NOLOCK) on r2.ReasonTypeID= 'Style_Apparel_Type' and r2.ID = s.ApparelType
	where s.Ukey = o.StyleUkey
)sty
where	O.Category in ('B','S')
		--Buyer Delivery Date
		{2}--and O.BuyerDelivery between @BDDateStart and @BDDateEnd  
		--SP#
		{3}--and O.ID between @SPStart and @SPEnd  
		--M
		{4}--and O.MDivisionID = @MDivision  
		--Factory
		{5}--and O.FactoryID = @Factory  
        --Include Cancel Order
        {6}
ORDER BY O.ID", sqlFilte["DaysSinceInline_Factory"],
                sqlFilte["DaysToDelivery_Factory"],
                sqlFilte["_BuyerDelivery"],
                sqlFilte["_SPnum"],
                sqlFilte["_M"],
                sqlFilte["_Factory"],
                whereIncludeCancelOrder);
            #endregion
            #region SQL get DataTable
            DualResult result;
            result = DBProxy.Current.Select(null, sqlCmd, listSqlParameter, out this.resultDt);
            if (!result)
            {
                return result;
            }
            #endregion
            return new DualResult(true);
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            #region check resultDT
            if (this.resultDt == null || this.resultDt.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not Found.");
                return false;
            }
            #endregion

            this.ShowWaitMessage("Data computing...");
            #region compute 【StdOutput, VarianceDays ,DaysNeedForProd, Variance, PotentialDelayRisk】
            foreach (DataRow resultDr in this.resultDt.Rows)
            {
                #region VarianceDays ,DaysNeedForProd
                if (resultDr["SewingComplete"].ToString().EqualString("Y"))
                {
                    resultDr["StdOutput"] = 0;
                    resultDr["VarianceDays"] = 0;
                    resultDr["DaysNeedForProd"] = 0;
                }
                else
                {
                    #region StdOutput
                    decimal dec_StdOutput = PublicPrg.Prgs.GetStdQ(resultDr["ID"].ToString());
                    resultDr["StdOutput"] = dec_StdOutput;
                    #endregion
                    #region VarianceDays
                    decimal dec_VarianceQty, dec_VarianceDay;
                    decimal.TryParse(resultDr["VarianceQty"].ToString(), out dec_VarianceQty);
                    dec_VarianceDay = (dec_StdOutput == 0) ? 0 : dec_VarianceQty / dec_StdOutput;
                    resultDr["VarianceDays"] = (dec_VarianceDay > 0) ? Math.Ceiling(dec_VarianceDay) : Math.Floor(dec_VarianceDay);
                    #endregion
                    #region DaysNeedForProd
                    decimal dec_nTtlSewDays, dec_nTtlOutPut, dec_nAvgActOut, dec_Qty, dec_AccuActOutput;
                    decimal.TryParse(resultDr["nTtlSewDays"].ToString(), out dec_nTtlSewDays);
                    decimal.TryParse(resultDr["nTtlOutPut"].ToString(), out dec_nTtlOutPut);
                    decimal.TryParse(resultDr["Qty"].ToString(), out dec_Qty);
                    decimal.TryParse(resultDr["AccuActOutput"].ToString(), out dec_AccuActOutput);
                    dec_nAvgActOut = (dec_nTtlSewDays > 0) ? (dec_nTtlOutPut / dec_nTtlSewDays) : dec_StdOutput;
                    resultDr["DaysNeedForProd"] = (dec_nAvgActOut > 0) ? Math.Ceiling((dec_Qty - dec_AccuActOutput) / dec_nAvgActOut) : resultDr["PdnDays"];
                    #endregion
                }
                #endregion
                decimal dec_Variance;
                #region Variance
                decimal dec_DaysToDelivery, dec_DaysNeedForProd, dec_PostSewingDays;
                decimal.TryParse(resultDr["DaysToDelivery"].ToString(), out dec_DaysToDelivery);
                decimal.TryParse(resultDr["DaysNeedForProd"].ToString(), out dec_DaysNeedForProd);
                decimal.TryParse(resultDr["PostSewingDays"].ToString(), out dec_PostSewingDays);
                dec_Variance = dec_DaysToDelivery - dec_DaysNeedForProd - dec_PostSewingDays;
                resultDr["Variance"] = dec_Variance;
                #endregion
                #region PotentialDelayRisk
                if (resultDr["PotentialDelayRisk"].ToString().Empty())
                {
                    resultDr["PotentialDelayRisk"] = (dec_Variance < 0) ? "Y" : "N";
                }
                #endregion
            }
            #endregion
            #region remove columns 【nTtlOutPut, nTtlSewDays】
            this.resultDt.Columns.Remove("nTtlOutPut");
            this.resultDt.Columns.Remove("nTtlSewDays");
            #endregion

            this.ShowWaitMessage("Excel Processing...");
            #region Excel Process
            Excel.Application objApp = null;
            Excel.Worksheet worksheet = null;
            objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\PPIC_R10.xltx");
            MyUtility.Excel.CopyToXls(this.resultDt, string.Empty, "PPIC_R10.xltx", 3, showExcel: false, excelApp: objApp);
            worksheet = objApp.Sheets[1];

            /*
             * Set Title
             */
            worksheet.Cells[2, 2] = this.dateBuyerDeliveryStart + " ~ " + this.dateBuyerDeliveryEnd;
            worksheet.Cells[2, 7] = this.strSPStart + " ~ " + this.strSPEnd;
            worksheet.Cells[2, 11] = this.strFactory;
            worksheet.Cells[2, 14] = this.dateSewingOutput;

            worksheet.Rows.AutoFit();

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("PPIC_R10");
            Excel.Workbook workbook = objApp.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);
            Marshal.ReleaseComObject(worksheet);
            Marshal.ReleaseComObject(workbook);

            strExcelName.OpenFile();
            #endregion
            #endregion
            this.HideWaitMessage();
            return true;
        }
    }
}
