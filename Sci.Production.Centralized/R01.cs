using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Ict;
using Sci.Win;
using System.Data.SqlClient;
using Sci.Data;
using Sci.Utility.Excel;
using Sci.Production.Prg;
using System.Xml.Linq;
using System.Linq;
using System.Configuration;

namespace Sci.Production.Centralized
{
    /// <summary>
    /// R01
    /// </summary>
    public partial class R01 : Win.Tems.PrintForm
    {
        private DataTable dtPrint = null;

        private DataTable tmpData1;

        private DataTable tmpData2;

        private DataTable Final_Data1;

        private DataTable FinalData4;

        private DataTable FinalAllData4;

        private DataTable FinalStyleDetail;

        private DataTable FinalOrderDetail;

        private DataTable FinalALLData;

        private DataTable FinalDetailData;
        private string SqlData1;
        private string SqlData2;
        private string SqlData3;
        private string SqlData4;
        private string All_SqlData4;
        private string SqlStyleDetail;
        private string SqlOrderDetail;

        /// <summary>
        /// R01
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.EditMode = true;
            this.print.Visible = false;
        }

        // 欄位檢核

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        protected override bool OnToExcel(ReportDefinition report)
        {
            return true;
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            DualResult result = Result.True;
            if (this.dtPrint != null)
            {
                this.dtPrint.Rows.Clear();
            }

            string where = " ", groupBy, select;
            #region --由Factory.PmsPath抓各個連線路徑
            this.SetLoadingText("Load connections... ");
            XDocument docx = XDocument.Load(Application.ExecutablePath + ".config");
            string[] strSevers = ConfigurationManager.AppSettings["ServerMatchFactory"].Split(new char[] { ';' });
            List<string> connectionString = new List<string>(); // ←以R13為例 主要是要重組 List connectionString
            foreach (string ss in strSevers)
            {
                var connections = docx.Descendants("modules").Elements().Where(y => y.FirstAttribute.Value.Contains(ss.Split(new char[] { ':' })[0].ToString())).Descendants("connectionStrings").Elements().Where(x => x.FirstAttribute.Value.Contains("Production")).Select(z => z.LastAttribute.Value).ToList()[0].ToString();
                connectionString.Add(connections);
            }

            if (connectionString == null || connectionString.Count == 0)
            {
                return new DualResult(false, "no connection loaded.");
            }
            #endregion

            // 將所有工廠的資料合併起來
            for (int i = 0; i < connectionString.Count; i++)
            {
                 string conString = connectionString[i];
                 this.SetLoadingText(
                    string.Format(
                        "Load data from connection {0}/{1} ",
                        i + 1,
                        connectionString.Count));

                // 跨資料庫連線，將所需資料存到TempTable，再給不同資料庫使用
                 SqlConnection con;
                 using (con = new SqlConnection(conString))
                {
                    con.Open();
                    DataTable tmpData3, tmpData4, all_tmpData4, tmpStyleDetail, tmpOrderDetail;

                    #region tmpData1
                    where = " ";
                    if (this.txtBrand.Text != string.Empty)
                    {
                        where += string.Format(" and O.BrandID = '{0}' ", this.txtBrand.Text);
                    }

                    if (this.txtCountry.TextBox1.Text != string.Empty)
                    {
                        where += string.Format(" and F.CountryID = '{0}'  ", this.txtCountry.TextBox1.Text);
                    }

                    if (this.txtSeason.Text != string.Empty)
                    {
                        where += string.Format(" and SeasonID =  '{0}' ", this.txtSeason.Text);
                    }

                    this.SqlData1 = string.Format(
                        @"
Select O.ID 
	   , O.CPU 
	   , SMV = O.Cpu * ts.StdTMS / 60 
	   , TMS = O.CPU * ts.StdTMS
	   , O.FactoryID 
	   , O.BrandAreaCode 
	   , O.Qty 
	   , O.StyleID 
	   , F.CountryID
From Orders O WITH (NOLOCK)
Left Join Factory F WITH (NOLOCK) on O.FactoryID = F.ID
inner join Production.dbo.System ts on 1 = 1
Where 1 = 1 
	  and o.LocalOrder = 0 
	  and f.IsProduceFty = 1 
      and O.Category in ('B','S') 
	  {0}", where);

                    this.BeginInvoke(() => { Sci.MyUtility.Msg.WaitWindows("Wait – Style, Order 資料抓取中, 資料可能很多, 請等待 (Step 1/5)"); });
                    result = DBProxy.Current.SelectByConn(con, this.SqlData1, out this.tmpData1);
                    this.BeginInvoke(() => { MyUtility.Msg.WaitClear(); });
                    if (!result)
                    {
                        return result;
                    }

                    DataTable dt;
                    MyUtility.Tool.ProcessWithDatatable(this.tmpData1, string.Empty, " ", out dt, "#tmpData1", con);
                    #endregion
                    #region tmpData2
                    this.SqlData2 = @"
select OrderID = tmpData1.ID
	   , CPU = tmpData1.CPU
	   , SMV = tmpData1.SMV 
	   , TMS = tmpData1.TMS
	   , Factory = tmpData1.FactoryID
	   , AGCCode = tmpData1.BrandAreaCode
	   , [Order Qty] = tmpData1.Qty
	   , Style = tmpData1.StyleID 
	   , FactoryCountry = tmpData1.CountryID
	   , ProdQty = isnull(SewingOutput_Detail.QAQty, 0)
	   , [StardQty]= isnull(Round(3600 / tmpData1.TMS * Round(SewingOutput.ManPower * SewingOutput_Detail.WorkHour ,1), 0) ,0)  
from #tmpData1 tmpData1
Left Join SewingOutput_Detail WITH (NOLOCK) on SewingOutput_Detail.OrderId = tmpData1.ID AND SewingOutput_Detail.WorkHour > 0
Left Join SewingOutput WITH (NOLOCK) on SewingOutput.ID = SewingOutput_Detail.ID";

                    this.BeginInvoke(() => { Sci.MyUtility.Msg.WaitWindows("Wait – By Order, Factory 整理明細 (Step 2/5)"); });
                    result = DBProxy.Current.SelectByConn(con, this.SqlData2, out this.tmpData2);
                    this.BeginInvoke(() => { MyUtility.Msg.WaitClear(); });
                    if (!result)
                    {
                        return result;
                    }

                    MyUtility.Tool.ProcessWithDatatable(this.tmpData2, string.Empty, " ", out dt, "#tmpData2", con);
                    #endregion

                    #region tmpData3
                    if (this.rbRegionNo.Checked)
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

                    this.BeginInvoke(() => { Sci.MyUtility.Msg.WaitWindows("Wait – Group By Style , Country 整理明細 (Step 3/5)"); });
                    result = DBProxy.Current.SelectByConn(con, this.SqlData3, out tmpData3);
                    this.BeginInvoke(() => { MyUtility.Msg.WaitClear(); });
                    if (!result)
                    {
                        return result;
                    }

                    MyUtility.Tool.ProcessWithDatatable(tmpData3, string.Empty, " ", out dt, "#tmpData3", con);
                    #endregion

                    #region tmpEFFIC
                    if (this.rbRegionNo.Checked)
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
) c on c.Country = final.Country and c.data = final.SMVEFFX and c.{0} = final.{0}
order by {0} , Country , data
",
                        select,
                        groupBy);

                    this.All_SqlData4 = @"
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
select 'All',''
,isnull(final.[%] ,0) [%]
,SMVEFF = case c.data when 'A' then '40 & below' when 'B' then '40-49' when 'C' then '50-59' when 'D' then '60-69' when 'E' then '70-79' when 'F' then '80-89' when 'G' then '90-99' else '100 & above' end
,isnull(final.v1 ,0) v1 ,isnull(final.v2 ,0) v2 ,isnull(final.v3 ,0) v3
,isnull(final.v4 ,0) v4 ,isnull(final.v5 ,0) v5 ,isnull(final.v6 ,0) v6
from final 
full join (
	 select data from dbo.SplitString('A,B,C,D,E,F,G,H',',')
) c on c.Data = final.SMVEFFX
order by data";

                    this.BeginInvoke(() => { Sci.MyUtility.Msg.WaitWindows("Wait – 產生tmpEFFIC明細 (Step 3/5)"); });
                    result = DBProxy.Current.SelectByConn(con, this.SqlData4, out tmpData4);
                    result = DBProxy.Current.SelectByConn(con, this.All_SqlData4, out all_tmpData4);
                    this.BeginInvoke(() => { MyUtility.Msg.WaitClear(); });
                    if (!result)
                    {
                        return result;
                    }
                    #endregion

                    #region tmpStyleDetail
                    if (this.rbRegionNo.Checked)
                    {
                        groupBy = " AGCCode ";
                    }
                    else
                    {
                        groupBy = " Factory ";
                    }

                    this.SqlStyleDetail = string.Format(@"select Style,CPU,SMV,{0} as [grouping], StyleProdQty as 實際產量, StyleStardQty as 標準產量, QtyEFFX as [橫向區間#], Country from #tmpData3 order by Style", groupBy);

                    this.BeginInvoke(() => { Sci.MyUtility.Msg.WaitWindows("Wait – 整理 Style Detail 資料 (Step 4/5)"); });
                    result = DBProxy.Current.SelectByConn(con, this.SqlStyleDetail, out tmpStyleDetail);
                    this.BeginInvoke(() => { MyUtility.Msg.WaitClear(); });
                    if (!result)
                    {
                        return result;
                    }
                    #endregion

                    #region tmpOrderDetail
                    this.SqlOrderDetail = @"select tmpData2.OrderID as SP#, tmpData2.CPU as CPU, tmpData2.SMV as SMV, tmpData2.TMS as TMS, tmpData2.Factory as Factory, 
	                                            tmpData2.AGCCode  as AreaCode, tmpData2.[Order Qty] as [SP# Q'ty], tmpData2.Style as Style, tmpData2.ProdQty as 實際產量, tmpData2.StardQty as 標準產量 
                                            from #tmpData2 tmpData2 order by tmpData2.OrderID";

                    this.BeginInvoke(() => { Sci.MyUtility.Msg.WaitWindows("Wait – 整理 Order Detail 資料 (Step 5/5)"); });
                    result = DBProxy.Current.SelectByConn(con, this.SqlOrderDetail, out tmpOrderDetail);
                    this.BeginInvoke(() => { MyUtility.Msg.WaitClear(); });
                    if (!result)
                    {
                        return result;
                    }
                    #endregion
                    /*
                     將資料合併起來
                     */

                    // 用於計算筆數
                    if (this.Final_Data1 == null || this.Final_Data1.Rows.Count == 0)
                    {
                        this.Final_Data1 = this.tmpData1;
                    }
                    else
                    {
                        this.Final_Data1.Merge(this.tmpData1);
                    }

                    if (this.FinalData4 == null || this.FinalData4.Rows.Count == 0)
                    {
                        this.FinalData4 = tmpData4;
                    }
                    else
                    {
                        this.FinalData4.Merge(tmpData4);
                    }

                    if (this.FinalAllData4 == null || this.FinalAllData4.Rows.Count == 0)
                    {
                        this.FinalAllData4 = tmpData3;
                    }
                    else
                    {
                        this.FinalAllData4.Merge(tmpData3);
                    }

                    if (this.FinalStyleDetail == null || this.FinalStyleDetail.Rows.Count == 0)
                    {
                        this.FinalStyleDetail = tmpStyleDetail;
                    }
                    else
                    {
                        this.FinalStyleDetail.Merge(tmpStyleDetail);
                    }

                    if (this.FinalOrderDetail == null || this.FinalOrderDetail.Rows.Count == 0)
                    {
                        this.FinalOrderDetail = tmpOrderDetail;
                    }
                    else
                    {
                        this.FinalOrderDetail.Merge(tmpOrderDetail);
                    }
                }
            }

            SqlConnection sqlConn = null;
            DBProxy.Current.OpenConnection(null, out sqlConn);
            using (sqlConn)
            {
                DataTable dtAll;
                MyUtility.Tool.ProcessWithDatatable(this.FinalAllData4, string.Empty, " ", out dtAll, "#tmpALL", sqlConn);
                if (this.rbRegionNo.Checked)
                {
                    select = "AGCCode";
                    groupBy = "AGCCode";
                }
                else
                {
                    select = "Factory";
                    groupBy = "Factory";
                }

                result = DBProxy.Current.SelectByConn(
                    sqlConn,
                    string.Format(
                    @"
;with final as (
	select {0},Country ,sum(SMV / sSMV) as [%] ,SMVEFFX 
	,max(v1) as v1 ,max(v2) as v2 ,max(v3) as v3
	,max(v4) as v4 ,max(v5) as v5 ,max(v6) as v6
	from #tmpALL td3
	OUTER APPLY (select sum(SMV) as sSMV from #tmpALL tmp3 where td3.Country = tmp3.Country and td3.{0} = tmp3.{0}) ss
	outer apply (select iif( sum(StyleStardQty) = 0 , 0 , sum(StyleProdQty) / sum(StyleStardQty) ) as v1 from #tmpALL tmp where tmp.Country = td3.Country and tmp.{0} = td3.{0} and tmp.SMVEFFX = td3.SMVEFFX and tmp.QtyEFFX = 'A') v1                                          
	outer apply (select iif( sum(StyleStardQty) = 0 , 0 , sum(StyleProdQty) / sum(StyleStardQty) ) as v2 from #tmpALL tmp where tmp.Country = td3.Country and tmp.{0} = td3.{0} and tmp.SMVEFFX = td3.SMVEFFX and tmp.QtyEFFX = 'B') v2                                          
	outer apply (select iif( sum(StyleStardQty) = 0 , 0 , sum(StyleProdQty) / sum(StyleStardQty) ) as v3 from #tmpALL tmp where tmp.Country = td3.Country and tmp.{0} = td3.{0} and tmp.SMVEFFX = td3.SMVEFFX and tmp.QtyEFFX = 'C') v3                                        
	outer apply (select iif( sum(StyleStardQty) = 0 , 0 , sum(StyleProdQty) / sum(StyleStardQty) ) as v4 from #tmpALL tmp where tmp.Country = td3.Country and tmp.{0} = td3.{0} and tmp.SMVEFFX = td3.SMVEFFX and tmp.QtyEFFX = 'D') v4                                         
	outer apply (select iif( sum(StyleStardQty) = 0 , 0 , sum(StyleProdQty) / sum(StyleStardQty) ) as v5 from #tmpALL tmp where tmp.Country = td3.Country and tmp.{0} = td3.{0} and tmp.SMVEFFX = td3.SMVEFFX and tmp.QtyEFFX = 'E') v5                                          
	outer apply (select iif( sum(StyleStardQty) = 0 , 0 , sum(StyleProdQty) / sum(StyleStardQty) ) as v6 from #tmpALL tmp where tmp.Country = td3.Country and tmp.{0} = td3.{0} and tmp.SMVEFFX = td3.SMVEFFX and tmp.QtyEFFX = 'F') v6
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
) c on c.Country = final.Country and c.data = final.SMVEFFX and c.{0} = final.{0}
order by {0} , Country , data", select,
                    groupBy),
                    out this.FinalDetailData);

#pragma warning disable SA1118 // Parameter must not span multiple lines
                result = DBProxy.Current.SelectByConn(
                    sqlConn,
                    @";with final as (
	select sum(SMV / sSMV) as [%] ,SMVEFFX 
	,max(v1) as v1 ,max(v2) as v2 ,max(v3) as v3
	,max(v4) as v4 ,max(v5) as v5 ,max(v6) as v6
	from #tmpALL td3
	OUTER APPLY (select sum(SMV) as sSMV from #tmpALL tmp3) ss --Sum 某一單位Factory, Region
	outer apply (select iif( sum(StyleStardQty) = 0 , 0 , sum(StyleProdQty) / sum(StyleStardQty) ) as v1 from #tmpALL tmp where tmp.SMVEFFX = td3.SMVEFFX and tmp.QtyEFFX = 'A') v1
	outer apply (select iif( sum(StyleStardQty) = 0 , 0 , sum(StyleProdQty) / sum(StyleStardQty) ) as v2 from #tmpALL tmp where tmp.SMVEFFX = td3.SMVEFFX and tmp.QtyEFFX = 'B') v2
	outer apply (select iif( sum(StyleStardQty) = 0 , 0 , sum(StyleProdQty) / sum(StyleStardQty) ) as v3 from #tmpALL tmp where tmp.SMVEFFX = td3.SMVEFFX and tmp.QtyEFFX = 'C') v3
	outer apply (select iif( sum(StyleStardQty) = 0 , 0 , sum(StyleProdQty) / sum(StyleStardQty) ) as v4 from #tmpALL tmp where tmp.SMVEFFX = td3.SMVEFFX and tmp.QtyEFFX = 'D') v4
	outer apply (select iif( sum(StyleStardQty) = 0 , 0 , sum(StyleProdQty) / sum(StyleStardQty) ) as v5 from #tmpALL tmp where tmp.SMVEFFX = td3.SMVEFFX and tmp.QtyEFFX = 'E') v5
	outer apply (select iif( sum(StyleStardQty) = 0 , 0 , sum(StyleProdQty) / sum(StyleStardQty) ) as v6 from #tmpALL tmp where tmp.SMVEFFX = td3.SMVEFFX and tmp.QtyEFFX = 'F') v6
	group by SMVEFFX
) 
select 'All',''
,isnull(final.[%] ,0) [%]
,SMVEFF = case c.data when 'A' then '40 & below' when 'B' then '40-49' when 'C' then '50-59' when 'D' then '60-69' when 'E' then '70-79' when 'F' then '80-89' when 'G' then '90-99' else '100 & above' end
,isnull(final.v1 ,0) v1 ,isnull(final.v2 ,0) v2 ,isnull(final.v3 ,0) v3
,isnull(final.v4 ,0) v4 ,isnull(final.v5 ,0) v5 ,isnull(final.v6 ,0) v6
from final 
full join (
	 select data from dbo.SplitString('A,B,C,D,E,F,G,H',',')
) c on c.Data = final.SMVEFFX
order by data",
                    out this.FinalALLData);

#pragma warning restore SA1118 // Parameter must not span multiple lines
                sqlConn.Close();
            }

            e.Report.ReportDataSource = this.Final_Data1;
            if (this.Final_Data1 != null && this.Final_Data1.Rows.Count > 0)
            {
                // 顯示筆數
                this.SetCount(this.Final_Data1.Rows.Count);
                return this.TransferData();
            }
            else
            {
                return new DualResult(false, "Data not found.");
            }
        }

        private DualResult TransferData()
        {
            string temfile = string.Empty;
            DualResult result = Result.True;

            string strPath = PrivUtils.getPath_XLT(AppDomain.CurrentDomain.BaseDirectory);
            temfile = strPath + @"\Centralized_R01.Matrix.xltx";

            SaveXltReportCls sxrc = new SaveXltReportCls(temfile);
            sxrc.BoOpenFile = true;

            SaveXltReportCls.XltRptTable xrt1 = new SaveXltReportCls.XltRptTable(this.FinalDetailData);
            SaveXltReportCls.XltRptTable xrt2 = new SaveXltReportCls.XltRptTable(this.FinalALLData);
            SaveXltReportCls.XltRptTable xrt3 = new SaveXltReportCls.XltRptTable(this.FinalStyleDetail);
            SaveXltReportCls.XltRptTable xrt4 = new SaveXltReportCls.XltRptTable(this.FinalOrderDetail);

            xrt1.ShowHeader = false;
            xrt2.ShowHeader = false;
            xrt3.ShowHeader = false;
            xrt4.ShowHeader = false;

            sxrc.DicDatas.Add("##detail", xrt1);
            sxrc.DicDatas.Add("##detailAll", xrt2);
            sxrc.DicDatas.Add("##StyleDetail", xrt3);
            sxrc.DicDatas.Add("##OrderDetail", xrt4);
            sxrc.DicDatas.Add("##SeasonCode", this.txtSeason.Text + "_historical data");

            sxrc.Save(Sci.Production.Class.MicrosoftFile.GetName("Centralized_R01.Matrix"));
            this.Final_Data1.Clear();
            this.FinalData4.Clear();
            this.FinalDetailData.Clear();
            this.FinalAllData4.Clear();
            this.FinalALLData.Clear();
            this.FinalStyleDetail.Clear();
            this.FinalOrderDetail.Clear();
            return Result.True;
        }
    }
}
