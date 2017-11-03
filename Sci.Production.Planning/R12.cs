using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Win;
using System.Data.SqlClient;
using Sci.Production.Report;
using Sci.Data;
using Msg = Sci.MyUtility.Msg;
using Sci.Production.Report.GSchemas;
using System.Runtime.InteropServices;
using Sci.Utility.Excel;

namespace Sci.Production.Planning
{
    public partial class R12 : Sci.Win.Tems.PrintForm
    {
        DataTable dtPrint = null;
        DataTable dtData, tmpData1, tmpData2, tmpData3, tmpData4, All_tmpData4, tmpStyleDetail, tmpOrderDetail;
        string SqlData1, SqlData2, SqlData3, SqlData4, All_SqlData4, SqlStyleDetail, SqlOrderDetail;
        decimal StandardTms = 0; //int intRowsStart = 2;

        public R12(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            EditMode = true;
            print.Visible = false;
            GetStandardTms();
        }

        //欄位檢核
        protected override bool ValidateInput()
        {
            if (txtBrand.Text.Trim() == "")
            {
                ShowErr("Brand can't be  blank");
                return false;
            }
            if (txtSeason.Text.Trim() == "")
            {
                ShowErr("Season can't be  blank");
                return false;
            }
            return true;
        }

        protected override bool OnToExcel(Win.ReportDefinition report)
        {            
            return true;
        }

        protected override Ict.DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            DualResult result = Result.True;
            if (dtPrint != null) dtPrint.Rows.Clear();
            string where = " ", GroupBy, select;
            SqlConnection con;
            SQL.GetConnection(out con);

            #region tmpData1
            if (txtBrand.Text != "") where += string.Format(" and O.BrandID = '{0}' ", txtBrand.Text);
            if (txtSeason.Text != "") where += string.Format(" and SeasonID =  '{0}' ", txtSeason.Text);
            SqlData1 = string.Format(@"Select O.ID , O.CPU , O.Cpu * {1} / 60 as SMV , O.CPU * {1} as TMS , O.FactoryID , O.BrandAreaCode , 
                                                O.Qty , O.StyleID , F.CountryID
                                        From Orders O WITH (NOLOCK)
                                        Left Join Factory F WITH (NOLOCK) on O.FactoryID = F.ID
                                        Where O.Category in ('B','S') {0}", where, StandardTms);

            BeginInvoke(() => { Sci.MyUtility.Msg.WaitWindows("Wait – Style,Order Data capture, data may be many, please wait (Step 1/5)"); });
            result = DBProxy.Current.SelectByConn(con, SqlData1, out tmpData1);
            BeginInvoke(() => { MyUtility.Msg.WaitClear(); });
            if (!result) return result;
            if (tmpData1 == null || tmpData1.Rows.Count == 0) return new DualResult(false, "Data not found.");

            DataTable dt;
            MyUtility.Tool.ProcessWithDatatable(tmpData1, "", " ", out dt, "#tmpData1", con);
            #endregion

            #region tmpData2
            SqlData2 = @"select tmpData1.ID as OrderID , tmpData1.CPU as CPU, tmpData1.SMV as SMV, tmpData1.TMS as TMS, tmpData1.FactoryID as Factory, 
                                tmpData1.BrandAreaCode as AGCCode, tmpData1.Qty as [Order Qty], tmpData1.StyleID as Style, tmpData1.CountryID as FactoryCountry, 
                                isnull(SewingOutput_Detail.QAQty,0) as ProdQty,isnull(Round(3600 / tmpData1.TMS * Round(SewingOutput.ManPower * SewingOutput_Detail.WorkHour ,1), 0) ,0)  as StardQty 
                            from #tmpData1 tmpData1
                            Left Join SewingOutput_Detail WITH (NOLOCK) on OrderID = tmpData1.ID
                            Left Join SewingOutput WITH (NOLOCK) on SewingOutput.ID = SewingOutput_Detail.ID";

            BeginInvoke(() => { Sci.MyUtility.Msg.WaitWindows("Wait – By Order, Factory Finishing details (Step 2/5)"); });
            result = DBProxy.Current.SelectByConn(con, SqlData2, out tmpData2);
            BeginInvoke(() => { MyUtility.Msg.WaitClear(); });
            if (!result) return result;

            MyUtility.Tool.ProcessWithDatatable(tmpData2, "", " ", out dt, "#tmpData2", con);
            #endregion

            #region tmpData3
            if (radioRegionNo.Checked)
            {
                select = " AGCCode ";
                GroupBy = " AGCCode ";
            }
            else
            {
                select = " Factory as Factory ";
                GroupBy = " Factory ";
            }

            SqlData3 = string.Format(@"
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
", select, GroupBy);

            BeginInvoke(() => { Sci.MyUtility.Msg.WaitWindows("Wait – Group By Style , Country Finishing details (Step 3/5)"); });
            result = DBProxy.Current.SelectByConn(con, SqlData3, out tmpData3);
            BeginInvoke(() => { MyUtility.Msg.WaitClear(); });
            if (!result) return result;

            MyUtility.Tool.ProcessWithDatatable(tmpData3, "", " ", out dt, "#tmpData3", con);
            #endregion

            #region tmpEFFIC
            if (radioRegionNo.Checked)
            {
                select = "AGCCode";
                GroupBy = "AGCCode";
            }
            else
            {
                select = "Factory";
                GroupBy = "Factory";
            }

            SqlData4 = string.Format(@"
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
", select, GroupBy);

            All_SqlData4 = string.Format(@"
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
order by c.data", select);

            BeginInvoke(() => { Sci.MyUtility.Msg.WaitWindows("Wait – Produce tmpEFFIC details (Step 3/5)"); });
            result = DBProxy.Current.SelectByConn(con, SqlData4, out tmpData4);
            result = DBProxy.Current.SelectByConn(con, All_SqlData4, out All_tmpData4);
            BeginInvoke(() => { MyUtility.Msg.WaitClear(); });
            if (!result) return result;
            #endregion

            #region tmpStyleDetail
            if (radioRegionNo.Checked)
                GroupBy = " AGCCode ";
            else
                GroupBy = " Factory ";


            SqlStyleDetail = string.Format(@"select Style,CPU,SMV,{0} as [AGC], StyleProdQty as SewingOutput, StyleStardQty as StdOutput, QtyEFFX, Country from #tmpData3 order by Style", GroupBy);
            BeginInvoke(() => { Sci.MyUtility.Msg.WaitWindows("Wait – Finishing Style Detail Data (Step 4/5)"); });
            result = DBProxy.Current.SelectByConn(con, SqlStyleDetail, out tmpStyleDetail);
            BeginInvoke(() => { MyUtility.Msg.WaitClear(); });
            if (!result) return result;
            #endregion

            #region tmpOrderDetail
            SqlOrderDetail = @"
select  tmpData2.OrderID as SP#, tmpData2.CPU as CPU, tmpData2.SMV as SMV, tmpData2.TMS as TMS, tmpData2.Factory as Factory, 
	    tmpData2.AGCCode  as AreaCode, tmpData2.[Order Qty] as [SP# Q'ty], tmpData2.Style as Style, tmpData2.ProdQty as 實際產量, tmpData2.StardQty as 標準產量 
from #tmpData2 tmpData2
order by tmpData2.OrderID";

            BeginInvoke(() => { Sci.MyUtility.Msg.WaitWindows("Wait – Finishing Order Detail Data (Step 5/5)"); });
            result = DBProxy.Current.SelectByConn(con, SqlOrderDetail, out tmpOrderDetail);
            BeginInvoke(() => { MyUtility.Msg.WaitClear(); });
            if (!result) return result;
            #endregion


            e.Report.ReportDataSource = tmpData1;
            if (tmpData1 != null && tmpData1.Rows.Count > 0)
            {
                //顯示筆數
                SetCount(tmpData1.Rows.Count);
                //transferToExcel();
                return transferData();
            }
            else
            {
                return new DualResult(false, "Data not found.");
            }
        }

        private DualResult transferData()
        {
            string temfile = "", title = "";
            DualResult result = Result.True;

            string strPath = PrivUtils.getPath_XLT(AppDomain.CurrentDomain.BaseDirectory);
            temfile = strPath + @"\Planning_R12.Matrix.xltx";

            SaveXltReportCls sxrc = new SaveXltReportCls(temfile);
            sxrc.BoOpenFile = true;

            tmpData4.Merge(All_tmpData4);

            SaveXltReportCls.XltRptTable xrt1 = new SaveXltReportCls.XltRptTable(tmpData4);
            //SaveXltReportCls.xltRptTable xrt2 = new SaveXltReportCls.xltRptTable(All_tmpData4);
            SaveXltReportCls.XltRptTable xrt3 = new SaveXltReportCls.XltRptTable(tmpStyleDetail);
            SaveXltReportCls.XltRptTable xrt4 = new SaveXltReportCls.XltRptTable(tmpOrderDetail);

            #region 抬頭
            if (radioRegionNo.Checked)
            {
                title = "Season =" + txtSeason.Text + "   , Brand =" + txtBrand.Text + "    , Grouping by Region No";
            }
            else
            {
                title = "Season =" + txtSeason.Text + "   , Brand =" + txtBrand.Text + "    , Grouping by Factory Code";
            }
            #endregion 

            xrt1.ShowHeader = false;
            //xrt2.ShowHeader = false;
            xrt3.ShowHeader = false;
            xrt4.ShowHeader = false;

            xrt1.BoAutoFitColumn = true;
            xrt3.BoAutoFitColumn = true;
            xrt4.BoAutoFitColumn = true;

            sxrc.DicDatas.Add("##detail", xrt1);
            //sxrc.dicDatas.Add("##detailAll", xrt2);
            sxrc.DicDatas.Add("##StyleDetail", xrt3);
            sxrc.DicDatas.Add("##OrderDetail", xrt4);
            sxrc.DicDatas.Add("##title", title);
            sxrc.DicDatas.Add("##Fty Code", txtSeason.Text + "_historical data");
            
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Planning_R12.Matrix");
            sxrc.Save(strExcelName);
            return Result.True;
        }

        private void GetStandardTms()
        {
            DualResult result;
            Helper ghelper = new Helper();
            if (!(result = ghelper.GetProductionSystem(out dtData)))
            {
                ShowErr("Get StandardTms Fail!!");
                return;
            }
            StandardTms = decimal.Parse(dtData.Rows[0]["StdTMS"].ToString());
        }
    }
}
