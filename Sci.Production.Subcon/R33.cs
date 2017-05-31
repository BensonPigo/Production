using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;

namespace Sci.Production.Subcon
{
    public partial class R33 : Sci.Win.Tems.PrintForm
    {
        string Factory, SewingStart, SewingEnd, SP;
        DataTable printData;
        public R33(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.print.Visible = false;
            
            //set ComboFactory
            DataTable dtFactory;
            DBProxy.Current.Select(null, "Select ID from Factory", out dtFactory);
            MyUtility.Tool.SetupCombox(comboFactory, 1, dtFactory);
            comboFactory.Text = Sci.Env.User.Factory;
        }

        protected override bool ValidateInput()
        {
            #region check Sewing Date
            if (dateSewingDate.Value1.Empty() || dateSewingDate.Value2.Empty())
            {
                MyUtility.Msg.InfoBox("Sewinbg Date can't be empty!");
                return false;
            }
            #endregion 
            #region set Data
            Factory = comboFactory.Text;
            SewingStart = ((DateTime)dateSewingDate.Value1).ToString("yyyy-MM-dd");
            SewingEnd = ((DateTime)dateSewingDate.Value2).ToString("yyyy-MM-dd");
            SP = txtSP.Text;
            #endregion 

            return true;
        }

        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            this.ShowWaitMessage("Data Loading...");
            #region SQLParameter
            List<SqlParameter> sqlParameter = new List<SqlParameter>();
            sqlParameter.Add(new SqlParameter("@Factory", Factory));
            sqlParameter.Add(new SqlParameter("@SewingStart", SewingStart));
            sqlParameter.Add(new SqlParameter("@SewingEnd", SewingEnd));
            sqlParameter.Add(new SqlParameter("@SPNO", SP));
            sqlParameter.Add(new SqlParameter("@ToExcelBy", (radioByFactory.Checked) ? 1 : 0));
            #endregion 

            #region SQL cmd
            string strSQL = string.Format(@"
declare @StartDate Date = @SewingStart;
declare @EndDate Date = @SewingEnd;
declare @SP char(20) = @SPNO;
declare @ByFactory Bit = @ToExcelBy;

--取出符合條件的訂單
----日期條件 (1. InLine & OffLine 都在區間內 2. InLine 在區間內 OffLine 在 End 後 3. InLine 在 Start 前 & OffLine 在 End 後)
select	masterID = o.POID
		, orderID = o.ID
		, o.Finished
		, o.FactoryID
		, o.SewLine
into #tsp
from orders o
inner join Factory f on o.FactoryID = f.ID
where	o.Junk != 1
		{0}
		and not ((o.SewOffLine < @StartDate and o.SewInLine < @EndDate) or (o.SewInLine > @StartDate and o.SewInLine > @EndDate))
		and (o.SewInLine is not null or o.SewInLine != '')
		and (o.SewOffLine is not null or o.SewOffLine != '')
order by o.POID, o.ID, o.SewLine

--取出的訂單，執行以下判斷流程
----Step1 尋找符合的訂單
------Step2 組出 SP# 的剪裁 Comb
select	#tsp.masterID
		, #tsp.orderID
		, #cur_artwork.FabricCombo
		, #cur_artwork.Article
		, #cur_artwork.artwork 
		, #tsp.SewLine
		, #tsp.FactoryID
into #cutcomb
from #tsp
inner join (
	select	distinct #tsp.orderID
			, b.Article
			, wo.FabricCombo
			, artwork = stuff ((select '+' + subprocessid
								from (
									select distinct subprocessid
									from Bundle_Detail_Art bda
									where bd.BundleNo = bda.Bundleno
								) k
								for xml path('')
							  ), 1, 1, '')
	from Bundle b
	inner join Bundle_Detail bd on b.id = bd.id
	inner join WorkOrder wo on b.Orderid = wo.OrderID
	inner join #tsp on b.Orderid = #tsp.orderID
)#cur_artwork on #tsp.orderID = #cur_artwork.orderID

----Step3 取出被 Loader 收下的 Bundle Data
----------須排除假日 1. Sunday 2. 有列在 Holiday 的日期
select	b.orderid
		, bd.SizeCode
		, cdate2 = cDate.value
		, Article
		, wo.FabricCombo
		, artwork = ArtWork.value
		, qty = sum(isnull(bd.qty, 0))
into #cur_bdltrack2
from BundleInOut bio
inner join Bundle_Detail bd on bio.BundleNo = bd.BundleNo
inner join Bundle b on b.id = bd.id
inner join WorkOrder wo on b.Orderid = wo.OrderID 
inner join #tsp on b.Orderid = #tsp.orderID
left join Holiday h	on h.HolidayDate = CONVERT(char(10), bio.InComing, 120)
					and h.FactoryID = #tsp.FactoryID
outer apply (
	select value = stuff ((	select '+' + subprocessid
							from (
								select distinct subprocessid
								from Bundle_Detail_Art bda
								where bd.BundleNo = bda.Bundleno
							) k
							for xml path('')
							), 1, 1, '')
) ArtWork
outer apply(
	select value = CONVERT(char(10), bio.InComing, 120)
) cDate
where	bio.SubProcessId = 'loading'	
		{1}
		and h.HolidayDate is null
		and DATEPART(DW, cDate.value) != 1
group by b.orderid, bd.SizeCode, cDate.value, b.Article, wo.FabricCombo, ArtWork.value

----Step4 已收的bundle資料中找出各article/size/部位/artwork/加總數量
------Step5 計算sp#上線日至下線日的產出
--------Step6 依條件日期區間，繞sewing 取得stdqty加總以及抓取備妥的成衣件數
------------依照【日期, SP#, Article, Size, CDomb, Artwork】取最小數量 (因為每個部位都要有，才能成為一件衣服)
select	#cutcomb.FactoryID
		, #cutcomb.orderid
		, #cur_bdltrack2.cdate2
		, #cutcomb.SewLine
		, #cutcomb.Article
		, #cur_bdltrack2.SizeCode
		, #cutcomb.FabricCombo
		, #cutcomb.artwork
		, #cur_bdltrack2.qty
		, AccuLoadingQty = sum(#cur_bdltrack2.qty) over (partition by #cutcomb.orderid, #cutcomb.Article, #cur_bdltrack2.SizeCode, #cutcomb.FabricCombo, #cutcomb.artwork 
														 order by #cur_bdltrack2.cdate2)
into #Min_cut
from #cutcomb
inner join #cur_bdltrack2 on	#cutcomb.artwork = #cur_bdltrack2.artwork 
								and #cutcomb.FabricCombo = #cur_bdltrack2.FabricCombo
								and #cutcomb.orderID = #cur_bdltrack2.orderid
								and #cutcomb.Article = #cur_bdltrack2.Article 
where	#cur_bdltrack2.qty is not null 
		and #cur_bdltrack2.qty != 0
		and #cur_bdltrack2.cdate2 between @StartDate and @EndDate
order by cdate2, orderid, Article, SizeCode, FabricCombo, artwork

select	FactoryID
		, orderID
		, cdate2
		, SewLine
		, Article, SizeCode
		, AccuStdQty = (select sum(s.StdQ)
						from dbo.getDailystdq(#Min_cut.orderid) s
						where s.Date between @StartDate and @EndDate)
		, MinLoadingQty = min(AccuLoadingQty)
into #print
from #Min_cut 
group by FactoryID, orderID, cdate2, SewLine, Article, SizeCode
order by FactoryID, orderID, cdate2, SewLine

----判斷 ToExcel 
IF @ByFactory = 1
	select	FactoryID
			, SewingDate = cdate2
			, AccuStd = sum (isnull (AccuStdQty, 0))
			, AccuLoading = sum (isnull(MinLoadingQty, 0))
			, BCS = iif (sum (isnull (AccuStdQty, 0)) = 0, 100
														 , iif (ROUND (sum (isnull (MinLoadingQty, 0)) / sum (isnull (AccuStdQty, 0)) * 100, 2) >= 100, 100
																																					  , ROUND (sum (isnull (MinLoadingQty, 0)) / sum (isnull (AccuStdQty, 0)) * 100, 2)))
	from #print
	group by FactoryID, cdate2
	order by FactoryID, cdate2
Else 
	select	FactoryID
			, SP = orderID
			, SewingDate = cdate2
			, Line = SewLine
			, AccuStd = sum (isnull (AccuStdQty, 0))
			, AccuLoading = sum (isnull(MinLoadingQty, 0))
			, BCS = iif (sum (isnull (AccuStdQty, 0)) = 0, 100
														 , iif (ROUND (sum (isnull (MinLoadingQty, 0)) / sum (isnull (AccuStdQty, 0)) * 100, 2) >= 100, 100
																																					  , ROUND (sum (isnull (MinLoadingQty, 0)) / sum (isnull (AccuStdQty, 0)) * 100, 2)))
	from #print
	group by FactoryID, orderID, cdate2, SewLine
	order by FactoryID, orderID, cdate2, SewLine

drop table #tsp
drop table #cutcomb
drop table #cur_bdltrack2
drop table #Min_cut
drop table #print"
                , (SP.Empty()) ? "" : "and o.id = @SP"
                , (SP.Empty()) ? "" : "and b.OrderID = @SP");
            #endregion 

            DualResult result = DBProxy.Current.Select(null, strSQL, sqlParameter, out printData);
            if (!result)
            {
                this.HideWaitMessage();
                MyUtility.Msg.ErrorBox(strSQL + "\n\r" + result.Description);
                return result;
            }
            this.HideWaitMessage();
            return Result.True;
        }

        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            if (printData == null || printData.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found.");
                return false;
            }
            this.ShowWaitMessage("Excel Processing...");

            Excel.Application objApp = null;
            Excel.Worksheet worksheet = null;
            if (radioByFactory.Checked)
            {
                #region By Factory
                objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Subcon_R33_ByFactory.xltx");
                MyUtility.Excel.CopyToXls(printData, "", "Subcon_R33_ByFactory.xltx", 3, showExcel: true, excelApp: objApp);
                worksheet = objApp.Sheets[1];
                #endregion 
            }
            else
            {
                #region By SPNO
                objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Subcon_R33_BySPNO.xltx");
                MyUtility.Excel.CopyToXls(printData, "", "Subcon_R33_BySPNO.xltx", 3, showExcel: true, excelApp: objApp);
                worksheet = objApp.Sheets[1];
                #endregion 
            }

            if (objApp != null) Marshal.FinalReleaseComObject(objApp);          //釋放objApp
            if (worksheet != null) Marshal.FinalReleaseComObject(worksheet);    //釋放worksheet
            this.HideWaitMessage();
            return true;
        }
    }
}
