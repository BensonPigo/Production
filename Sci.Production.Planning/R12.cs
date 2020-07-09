using System;
using System.Data;
using System.Windows.Forms;
using Ict;
using Sci.Win;
using System.Data.SqlClient;
using Sci.Production.Report;
using Sci.Data;
using Sci.Production.Report.GSchemas;
using Sci.Utility.Excel;

namespace Sci.Production.Planning
{
    /// <summary>
    /// R12
    /// </summary>
    public partial class R12 : Win.Tems.PrintForm
    {
        private DataTable dtPrint = null;
        private DataTable dtData;
        private DataTable tmpData1;
        private DataTable tmpData2;
        private DataTable tmpData3;
        private DataTable tmpData4;
        private DataTable All_tmpData4;
        private DataTable tmpStyleDetail;
        private DataTable tmpOrderDetail;
        private string SqlData1;
        private string SqlData2;
        private string SqlData3;
        private string SqlData4;
        private string All_SqlData4;
        private string SqlStyleDetail;
        private string SqlOrderDetail;
        private decimal StandardTms = 0;

        /// <summary>
        /// R12
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R12(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.EditMode = true;
            this.print.Visible = false;
            this.GetStandardTms();
        }

        /// <summary>
        /// ValidateInput
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ValidateInput()
        {
            if (this.txtBrand.Text.Trim() == string.Empty)
            {
                this.ShowErr("Brand can't be  blank");
                return false;
            }

            if (this.txtSeason.Text.Trim() == string.Empty)
            {
                this.ShowErr("Season can't be  blank");
                return false;
            }

            return true;
        }

        /// <summary>
        /// OnToExcel
        /// </summary>
        /// <param name="report">report</param>
        /// <returns>bool</returns>
        protected override bool OnToExcel(ReportDefinition report)
        {
            return true;
        }

        /// <summary>
        /// OnAsyncDataLoad
        /// </summary>
        /// <param name="e">e</param>
        /// <returns>DualResult</returns>
        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            DualResult result = Ict.Result.True;
            if (this.dtPrint != null)
            {
                this.dtPrint.Rows.Clear();
            }

            string where = " ", groupBy, select;
            SqlConnection con;
            SQL.GetConnection(out con);

            #region tmpData1
            if (this.txtBrand.Text != string.Empty)
            {
                where += string.Format(" and O.BrandID = '{0}' ", this.txtBrand.Text);
            }

            if (this.txtSeason.Text != string.Empty)
            {
                where += string.Format(" and SeasonID =  '{0}' ", this.txtSeason.Text);
            }

            this.SqlData1 = string.Format(
                @"
Select O.ID , O.CPU , O.Cpu * {1} / 60 as SMV , O.CPU * {1} as TMS , O.FactoryID , O.BrandAreaCode , O.Qty , O.StyleID , F.CountryID
From Orders O WITH (NOLOCK)
Left Join Factory F WITH (NOLOCK) on O.FactoryID = F.ID
Where 1 = 1 
	  and o.LocalOrder = 0 
	  and f.IsProduceFty = 1 
      and O.Category in ('B','S') 
      {0}", where,
                this.StandardTms);

            this.BeginInvoke(() => { MyUtility.Msg.WaitWindows("Wait – Style,Order Data capture, data may be many, please wait (Step 1/5)"); });
            result = DBProxy.Current.SelectByConn(con, this.SqlData1, out this.tmpData1);
            this.BeginInvoke(() => { MyUtility.Msg.WaitClear(); });
            if (!result)
            {
                return result;
            }

            if (this.tmpData1 == null || this.tmpData1.Rows.Count == 0)
            {
                return new DualResult(false, "Data not found.");
            }

            DataTable dt;
            MyUtility.Tool.ProcessWithDatatable(this.tmpData1, string.Empty, " ", out dt, "#tmpData1", con);
            #endregion

            #region tmpData2
            this.SqlData2 = @"
select tmpData1.ID as OrderID , tmpData1.CPU as CPU, tmpData1.SMV as SMV, tmpData1.TMS as TMS, tmpData1.FactoryID as Factory, 
    tmpData1.BrandAreaCode as AGCCode, tmpData1.Qty as [Order Qty], tmpData1.StyleID as Style, tmpData1.CountryID as FactoryCountry, 
    isnull(SewingOutput_Detail.QAQty,0) as ProdQty,isnull(Round(3600 / tmpData1.TMS * Round(SewingOutput.ManPower * SewingOutput_Detail.WorkHour ,1), 0) ,0)  as StardQty 
from #tmpData1 tmpData1
Left Join SewingOutput_Detail WITH (NOLOCK) on OrderID = tmpData1.ID AND SewingOutput_Detail.WorkHour > 0
Left Join SewingOutput WITH (NOLOCK) on SewingOutput.ID = SewingOutput_Detail.ID";

            this.BeginInvoke(() => { MyUtility.Msg.WaitWindows("Wait – By Order, Factory Finishing details (Step 2/5)"); });
            result = DBProxy.Current.SelectByConn(con, this.SqlData2, out this.tmpData2);
            this.BeginInvoke(() => { MyUtility.Msg.WaitClear(); });
            if (!result)
            {
                return result;
            }

            MyUtility.Tool.ProcessWithDatatable(this.tmpData2, string.Empty, " ", out dt, "#tmpData2", con);
            #endregion

            #region tmpData3
            if (this.radioRegionNo.Checked)
            {
                select = " AGCCode ";
                groupBy = " AGCCode ";
            }
            else
            {
                select = " Factory as Factory ";
                groupBy = " Factory ";
            }

            this.SqlData3 = string.Format(
                @"
select * 
    ,CASE WHEN SMV < 40 THEN 'A' WHEN SMV >= 40 and SMV < 50  THEN 'B' WHEN SMV >= 50 and SMV < 60  THEN 'C' WHEN SMV >= 60 and SMV < 70  THEN 'D'
    WHEN SMV >= 70 and SMV < 80  THEN 'E' WHEN SMV >= 80 and SMV < 90  THEN 'F'	WHEN SMV >= 90 and SMV < 100 THEN 'G'  ELSE 'H' END as SMVEFFX
    ,CASE WHEN StyleProdQty < 3001 THEN 'A' WHEN StyleProdQty >= 3001 and StyleProdQty < 5001  THEN 'B' WHEN StyleProdQty >= 5001 and StyleProdQty < 7001  THEN 'C' WHEN StyleProdQty >= 7001 and StyleProdQty < 9001  THEN 'D'
    WHEN StyleProdQty >= 9001 and StyleProdQty < 10001  THEN 'E' WHEN StyleProdQty >= 10001 THEN 'F' END as QtyEFFX 
from (
	select Style ,FactoryCountry as Country ,{0}
	,sum(CPU) as CPU ,sum(SMV) as SMV , sum([Order Qty]) as [Order Qty]
	,sum(ProdQty) as StyleProdQty ,sum(StardQty) as StyleStardQty 
	from #tmpData2
	group by Style,FactoryCountry ,{1}
) c
",
                select,
                groupBy);

            this.BeginInvoke(() => { MyUtility.Msg.WaitWindows("Wait – Group By Style , Country Finishing details (Step 3/5)"); });
            result = DBProxy.Current.SelectByConn(con, this.SqlData3, out this.tmpData3);
            this.BeginInvoke(() => { MyUtility.Msg.WaitClear(); });
            if (!result)
            {
                return result;
            }

            MyUtility.Tool.ProcessWithDatatable(this.tmpData3, string.Empty, " ", out dt, "#tmpData3", con);
            #endregion

            #region tmpEFFIC
            if (this.radioRegionNo.Checked)
            {
                select = "AGCCode";
                groupBy = "AGCCode";
            }
            else
            {
                select = "Factory";
                groupBy = "Factory";
            }

            this.SqlData4 = string.Format(
                @"
;with final as (
	select {0},Country ,sum(SMV / sSMV) as [%] ,SMVEFFX 
	,max(v1) as v1 ,max(v2) as v2 ,max(v3) as v3
	,max(v4) as v4 ,max(v5) as v5 ,max(v6) as v6
	from #tmpData3 td3
	OUTER APPLY (select sum(SMV) as sSMV from #tmpData3 tmp3 where td3.Country = tmp3.Country and td3.{0} = tmp3.{0}) ss
	outer apply (select iif( sum(StyleStardQty) = 0 , 0 , sum(StyleProdQty) / sum(StyleStardQty) ) as v1 from #tmpData3 tmp where tmp.Country = td3.Country and tmp.{0} = td3.{0} and tmp.SMVEFFX = td3.SMVEFFX and tmp.QtyEFFX = 'A') v1
	outer apply (select iif( sum(StyleStardQty) = 0 , 0 , sum(StyleProdQty) / sum(StyleStardQty) ) as v2 from #tmpData3 tmp where tmp.Country = td3.Country and tmp.{0} = td3.{0} and tmp.SMVEFFX = td3.SMVEFFX and tmp.QtyEFFX = 'B') v2
	outer apply (select iif( sum(StyleStardQty) = 0 , 0 , sum(StyleProdQty) / sum(StyleStardQty) ) as v3 from #tmpData3 tmp where tmp.Country = td3.Country and tmp.{0} = td3.{0} and tmp.SMVEFFX = td3.SMVEFFX and tmp.QtyEFFX = 'C') v3
	outer apply (select iif( sum(StyleStardQty) = 0 , 0 , sum(StyleProdQty) / sum(StyleStardQty) ) as v4 from #tmpData3 tmp where tmp.Country = td3.Country and tmp.{0} = td3.{0} and tmp.SMVEFFX = td3.SMVEFFX and tmp.QtyEFFX = 'D') v4
	outer apply (select iif( sum(StyleStardQty) = 0 , 0 , sum(StyleProdQty) / sum(StyleStardQty) ) as v5 from #tmpData3 tmp where tmp.Country = td3.Country and tmp.{0} = td3.{0} and tmp.SMVEFFX = td3.SMVEFFX and tmp.QtyEFFX = 'E') v5
	outer apply (select iif( sum(StyleStardQty) = 0 , 0 , sum(StyleProdQty) / sum(StyleStardQty) ) as v6 from #tmpData3 tmp where tmp.Country = td3.Country and tmp.{0} = td3.{0} and tmp.SMVEFFX = td3.SMVEFFX and tmp.QtyEFFX = 'F') v6
	group by {0} , Country , SMVEFFX
) 
select {0} = iif(final.{0} is not null, final.{0}, c.{0})
,Country = iif(final.Country is not null, final.Country, c.Country)
,isnull(final.[%] ,0) [%]
,SMVEFF = case c.data when 'A' then '40 & below' when 'B' then '40-49' when 'C' then '50-59' when 'D' then '60-69' when 'E' then '70-79' when 'F' then '80-89' when 'G' then '90-99' else '100 & above' end
,isnull(final.v1 ,0) v1 ,isnull(final.v2 ,0) v2 ,isnull(final.v3 ,0) v3
,isnull(final.v4 ,0) v4 ,isnull(final.v5 ,0) v5 ,isnull(final.v6 ,0) v6
from final 
full join (
	select * from (select {0},Country from final group by {0},Country) a
	inner join ( select data from dbo.SplitString('A,B,C,D,E,F,G,H',',') ) b on 1=1
) c on c.Country = final.Country and c.Data = final.SMVEFFX and c.{0} = final.{0}
order by {0} , Country , c.data
",
                select,
                groupBy);

            this.All_SqlData4 = string.Format(
                @"
;with final as (
	select sum(SMV / sSMV) as [%] ,SMVEFFX 
	,max(v1) as v1 ,max(v2) as v2 ,max(v3) as v3
	,max(v4) as v4 ,max(v5) as v5 ,max(v6) as v6
	from #tmpData3 td3
	OUTER APPLY (select sum(SMV) as sSMV from #tmpData3 tmp3) ss --Sum 某一單位Factory, Region
	outer apply (select iif( sum(StyleStardQty) = 0 , 0 , sum(StyleProdQty) / sum(StyleStardQty) ) as v1 from #tmpData3 tmp where tmp.SMVEFFX = td3.SMVEFFX and tmp.QtyEFFX = 'A') v1
	outer apply (select iif( sum(StyleStardQty) = 0 , 0 , sum(StyleProdQty) / sum(StyleStardQty) ) as v2 from #tmpData3 tmp where tmp.SMVEFFX = td3.SMVEFFX and tmp.QtyEFFX = 'B') v2
	outer apply (select iif( sum(StyleStardQty) = 0 , 0 , sum(StyleProdQty) / sum(StyleStardQty) ) as v3 from #tmpData3 tmp where tmp.SMVEFFX = td3.SMVEFFX and tmp.QtyEFFX = 'C') v3
	outer apply (select iif( sum(StyleStardQty) = 0 , 0 , sum(StyleProdQty) / sum(StyleStardQty) ) as v4 from #tmpData3 tmp where tmp.SMVEFFX = td3.SMVEFFX and tmp.QtyEFFX = 'D') v4
	outer apply (select iif( sum(StyleStardQty) = 0 , 0 , sum(StyleProdQty) / sum(StyleStardQty) ) as v5 from #tmpData3 tmp where tmp.SMVEFFX = td3.SMVEFFX and tmp.QtyEFFX = 'E') v5
	outer apply (select iif( sum(StyleStardQty) = 0 , 0 , sum(StyleProdQty) / sum(StyleStardQty) ) as v6 from #tmpData3 tmp where tmp.SMVEFFX = td3.SMVEFFX and tmp.QtyEFFX = 'F') v6
	group by SMVEFFX
) 
select 'All' as {0}, ''
,isnull(final.[%] ,0) [%]
,SMVEFF = case c.data when 'A' then '40 & below' when 'B' then '40-49' when 'C' then '50-59' when 'D' then '60-69' when 'E' then '70-79' when 'F' then '80-89' when 'G' then '90-99' else '100 & above' end
,isnull(final.v1 ,0) v1 ,isnull(final.v2 ,0) v2 ,isnull(final.v3 ,0) v3
,isnull(final.v4 ,0) v4 ,isnull(final.v5 ,0) v5 ,isnull(final.v6 ,0) v6
from final 
full join (
	 select data from dbo.SplitString('A,B,C,D,E,F,G,H',',')
) c on c.Data = final.SMVEFFX
order by c.data",
                select);

            this.BeginInvoke(() => { MyUtility.Msg.WaitWindows("Wait – Produce tmpEFFIC details (Step 3/5)"); });
            result = DBProxy.Current.SelectByConn(con, this.SqlData4, out this.tmpData4);
            result = DBProxy.Current.SelectByConn(con, this.All_SqlData4, out this.All_tmpData4);
            this.BeginInvoke(() => { MyUtility.Msg.WaitClear(); });
            if (!result)
            {
                return result;
            }
            #endregion

            #region tmpStyleDetail
            if (this.radioRegionNo.Checked)
            {
                groupBy = " AGCCode ";
            }
            else
            {
                groupBy = " Factory ";
            }

            this.SqlStyleDetail = string.Format(@"select Style,CPU,SMV,{0} as [AGC], StyleProdQty as SewingOutput, StyleStardQty as StdOutput, QtyEFFX, Country from #tmpData3 order by Style", groupBy);
            this.BeginInvoke(() => { MyUtility.Msg.WaitWindows("Wait – Finishing Style Detail Data (Step 4/5)"); });
            result = DBProxy.Current.SelectByConn(con, this.SqlStyleDetail, out this.tmpStyleDetail);
            this.BeginInvoke(() => { MyUtility.Msg.WaitClear(); });
            if (!result)
            {
                return result;
            }
            #endregion

            #region tmpOrderDetail
            this.SqlOrderDetail = @"
select  tmpData2.OrderID as SP#, tmpData2.CPU as CPU, tmpData2.SMV as SMV, tmpData2.TMS as TMS, tmpData2.Factory as Factory, 
	    tmpData2.AGCCode  as AreaCode, tmpData2.[Order Qty] as [SP# Q'ty], tmpData2.Style as Style, tmpData2.ProdQty as 實際產量, tmpData2.StardQty as 標準產量 
from #tmpData2 tmpData2
order by tmpData2.OrderID";

            this.BeginInvoke(() => { MyUtility.Msg.WaitWindows("Wait – Finishing Order Detail Data (Step 5/5)"); });
            result = DBProxy.Current.SelectByConn(con, this.SqlOrderDetail, out this.tmpOrderDetail);
            this.BeginInvoke(() => { MyUtility.Msg.WaitClear(); });
            if (!result)
            {
                return result;
            }
            #endregion

            e.Report.ReportDataSource = this.tmpData1;
            if (this.tmpData1 != null && this.tmpData1.Rows.Count > 0)
            {
                this.SetCount(this.tmpData1.Rows.Count);
                return this.TransferData();
            }
            else
            {
                return new DualResult(false, "Data not found.");
            }
        }

        private DualResult TransferData()
        {
            string temfile = string.Empty, title = string.Empty;
            DualResult result = Ict.Result.True;

            string strPath = PrivUtils.GetPath_XLT(AppDomain.CurrentDomain.BaseDirectory);
            temfile = strPath + @"\Planning_R12.Matrix.xltx";

            SaveXltReportCls sxrc = new SaveXltReportCls(temfile);
            sxrc.BoOpenFile = true;

            this.tmpData4.Merge(this.All_tmpData4);

            SaveXltReportCls.XltRptTable xrt1 = new SaveXltReportCls.XltRptTable(this.tmpData4);
            SaveXltReportCls.XltRptTable xrt3 = new SaveXltReportCls.XltRptTable(this.tmpStyleDetail);
            SaveXltReportCls.XltRptTable xrt4 = new SaveXltReportCls.XltRptTable(this.tmpOrderDetail);

            #region 抬頭
            if (this.radioRegionNo.Checked)
            {
                title = "Season =" + this.txtSeason.Text + "   , Brand =" + this.txtBrand.Text + "    , Grouping by Region No";
            }
            else
            {
                title = "Season =" + this.txtSeason.Text + "   , Brand =" + this.txtBrand.Text + "    , Grouping by Factory Code";
            }
            #endregion

            xrt1.ShowHeader = false;
            xrt3.ShowHeader = false;
            xrt4.ShowHeader = false;

            xrt1.BoAutoFitColumn = true;
            xrt3.BoAutoFitColumn = true;
            xrt4.BoAutoFitColumn = true;

            sxrc.DicDatas.Add("##detail", xrt1);
            sxrc.DicDatas.Add("##StyleDetail", xrt3);
            sxrc.DicDatas.Add("##OrderDetail", xrt4);
            sxrc.DicDatas.Add("##title", title);
            sxrc.DicDatas.Add("##Fty Code", this.txtSeason.Text + "_historical data");

            string strExcelName = Class.MicrosoftFile.GetName("Planning_R12.Matrix");
            sxrc.Save(strExcelName);
            return Ict.Result.True;
        }

        private void GetStandardTms()
        {
            DualResult result;
            Helper ghelper = new Helper();
            if (!(result = ghelper.GetProductionSystem(out this.dtData)))
            {
                this.ShowErr("Get StandardTms Fail!!");
                return;
            }

            this.StandardTms = decimal.Parse(this.dtData.Rows[0]["StdTMS"].ToString());
        }
    }
}
