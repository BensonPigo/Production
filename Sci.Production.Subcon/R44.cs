using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;

namespace Sci.Production.Subcon
{
    public partial class R44 : Win.Tems.PrintForm
    {
        string Factory;
        string SewingStart;
        string SewingEnd;
        string SP;
        string Category;
        DataTable printData;

        public R44(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.print.Visible = false;

            // set ComboFactory
            DataTable dtFactory;
            DBProxy.Current.Select(null, @"
select ID = ''

union all
Select ID 
from Factory WITH (NOLOCK)
where Junk != 1", out dtFactory);
            MyUtility.Tool.SetupCombox(this.comboFactory, 1, dtFactory);
            this.comboFactory.Text = Sci.Env.User.Factory;

            this.comboDropDownList1.SelectedIndex = 2;
        }

        protected override bool ValidateInput()
        {
            #region check Sewing Date
            if (this.dateSewingDate.Value1.Empty() || this.dateSewingDate.Value2.Empty())
            {
                MyUtility.Msg.InfoBox("Sewinbg Date can't be empty!");
                return false;
            }
            #endregion
            #region set Data
            this.Factory = this.comboFactory.Text;
            this.SewingStart = ((DateTime)this.dateSewingDate.Value1).ToString("yyyy-MM-dd");
            this.SewingEnd = ((DateTime)this.dateSewingDate.Value2).ToString("yyyy-MM-dd");
            this.SP = this.txtSP.Text;
            this.Category = MyUtility.Convert.GetString(this.comboDropDownList1.SelectedValue);
            #endregion
            return true;
        }

        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            #region SQLParameter
            List<SqlParameter> sqlParameter = new List<SqlParameter>();
            sqlParameter.Add(new SqlParameter("@Factory", this.Factory));
            sqlParameter.Add(new SqlParameter("@SewingStart", this.SewingStart));
            sqlParameter.Add(new SqlParameter("@SewingEnd", this.SewingEnd));
            sqlParameter.Add(new SqlParameter("@SPNO", this.SP));
            sqlParameter.Add(new SqlParameter("@ToExcelBy", this.radioByFactory.Checked ? 1 : 0));
            #endregion

            #region SQL cmd
            string whereIncludeCancelOrder = this.chkIncludeCancelOrder.Checked ? string.Empty : " and o.Junk = 0 ";
            DBProxy.Current.DefaultTimeout = 1800;  // timeout時間加長為30分鐘
            string strSQL = string.Format(
                @"
declare @StartDate Date = @SewingStart;
declare @EndDate Date = @SewingEnd;
declare @SP char(20) = @SPNO;
declare @ByFactory Bit = @ToExcelBy;
declare @FactoryID char(10) = @Factory;


/*
*	日期條件 
*		1. InLine & OffLine 都在區間內 
*		2. InLine 在區間內 OffLine 在 End 後 
*		3. InLine 在 Start 前 & OffLine 在 End 後
*/
select	masterID = o.POID
		, orderID = o.ID
		, StyleID
		, o.Finished
		, o.FactoryID
		, o.SewLine
		, o.SewInLine
		, o.SewOffLine
        , o.MDivisionID 
        , OrderQty = Qty
into #tsp
from orders o WITH (NOLOCK)
inner join Factory f WITH (NOLOCK) on o.FactoryID = f.ID
where	o.Category in ({2})
		-- {0} 篩選 OrderID
		{0}
		and not ((o.SewOffLine < @StartDate and o.SewInLine < @EndDate) or (o.SewInLine > @StartDate and o.SewInLine > @EndDate))
		and (o.SewInLine is not null or o.SewInLine != '')
		and (o.SewOffLine is not null or o.SewOffLine != '')	
		-- {1} 篩選 FactoryID	
        {1}
        {3}
--order by o.POID, o.ID, o.SewLine


/*
*	根據每一個 OrderID 取得 SewInDate  日期展開至 EndDate
*	這邊會影響到最後計算成衣件數
*/
create table #CBDate (
	SewDate date
);


Declare @StartInLine date = @StartDate;
Declare @SewInLine date;

/*
*	指向 #tsp 第一筆資料
*	@@FETCH_STATUS = 0，代表資料指向成功
*/

while @StartInLine <= @EndDate
begin
	insert into #CBDate
	select @StartInLine

	set @StartInLine =  DATEADD(day, 1, @StartInLine) 
end 
",
                this.SP.Empty() ? string.Empty : "and o.id = @SP",
                this.Factory.Empty() ? string.Empty : "and f.ID = @FactoryID",
                this.Category,
                whereIncludeCancelOrder);

            strSQL += $@"
select distinct t.orderID,
    InStartDate = Null,
    InEndDate = cast(cb.SewDate as datetime)+cast('23:59:59' as datetime),
    OutStartDate = Null,
    OutEndDate = Null
into #enn
from #tsp t
cross join #CBDate cb
";

            string[] subprocessIDs = new string[] { "Loading", };
            string qtyBySetPerSubprocess = PublicPrg.Prgs.QtyBySetPerSubprocess(subprocessIDs, "#enn", bySP: true, isNeedCombinBundleGroup: true, isMorethenOrderQty: "1");

            strSQL += qtyBySetPerSubprocess + @"

            /*
            *	準備好要印的資料
            */

select
    t.FactoryID,
    [SP] = t.orderID,
    t.StyleID,
    [SewingDate] = cb.SewDate,
    [Line] = t.SewLine,
    [AccuLoad] = sub.FinishedQtyBySet,
    OrderQty
into #print0
from #tsp t
cross join #CBDate cb
left join #Loading sub on sub.orderID = t.orderID and sub.InEndDate = cast(cb.SewDate as datetime)+cast('23:59:59' as datetime) -- 此處有相同orderID不同InEndDate

select FactoryID
		, SP
		, StyleID
		, SewingDate 
		, Line
		, AccuStd = iif(isnull(x4.AccStdQ,0) > isnull(Upperlimit.qty,0),isnull(Upperlimit.qty,0), isnull(x4.AccStdQ,0))
		, AccuLoad 
        , OrderQty
into #print
from #print0 p
outer apply(
	select AccStdQ=max(AccStdQ)
	from(
		select stdQty.*
		from SewingSchedule	s
		outer apply(select Date,AccStdQ=sum(StdQ)over(Partition by APSNo Order by Date)
		from dbo.[getDailystdq](s.APSNo) ) stdQty
		where s.OrderID=P.SP
	)x3
	where p.SewingDate = Date
)x4
outer apply(	
	SELECT qty = sum(qty)
	FROM Order_QtyShip 
	WHERE ID =p.SP
)Upperlimit
where x4.AccStdQ > 0


/*
*	判斷 ToExcel & 算出 BCS = Round(loading / std * 100, 2)
*	分母不能 = 0
*/
IF @ByFactory = 1
	select  FactoryID
			, SewingDate
            , OrderQty=sum(OrderQty)
			, Std = sum(AccuStd)
			, Loading = sum(AccuLoad) 
			, BCS = iif(ROUND(cast(sum(AccuLoad)as decimal) / iif(sum(AccuStd) = 0, 1, sum(AccuStd)) * 100, 2) >= 100, 100
					  , ROUND(cast(sum(AccuLoad)as decimal)  / iif(sum(AccuStd) = 0, 1, sum(AccuStd)) * 100, 2))
			, Followup = iif(ROUND(cast(sum(AccuLoad)as decimal)  / iif(sum(AccuStd) = 0, 1, sum(AccuStd)) * 100, 2) >= 100, 0,sum(AccuStd)-sum(AccuLoad)) 
	from #print
	where SewingDate between @StartDate and @EndDate
	group by FactoryID, SewingDate 
	having not (sum(AccuStd) = 0 and sum(AccuLoad) =0 )
	order by FactoryID, SewingDate
Else  

select	p.FactoryID,p.SP,p.StyleID,p.SewingDate,p.Line
        , p.OrderQty
        , p.AccuStd,p.AccuLoad
		, BCS = iif(BCS.value >= 100, 100, BCS.value)
        , Followup = iif(BCS.value >= 100, 0, p.AccuStd-p.AccuLoad) 
	from #print p
    outer apply (
    	select value = ROUND(cast(p.AccuLoad as decimal) / iif(AccuStd = 0, 1, AccuStd) * 100, 2)
    ) BCS
where SewingDate between @StartDate and @EndDate 
and not (p.AccuLoad = 0 and p.AccuStd =0 )
order by p.FactoryID,p.SP,p.SewingDate,p.Line

drop table #tsp
drop table #print
drop table #CBDate
DROP TABLE #print0
";
            #endregion

            DualResult result = DBProxy.Current.Select(null, strSQL, sqlParameter, out this.printData);
            if (!result)
            {
                return result;
            }

            DBProxy.Current.DefaultTimeout = 300;  // timeout時間調回為5分鐘
            return Result.True;
        }

        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            if (this.printData == null || this.printData.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found.");
                return false;
            }

            this.SetCount(this.printData.Rows.Count);
            this.ShowWaitMessage("Excel Processing...");

            Excel.Application objApp = null;
            Excel.Worksheet worksheet = null;
            if (this.radioByFactory.Checked)
            {
                #region By Factory
                objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Subcon_R44_ByFactory.xltx");
                worksheet = objApp.Sheets[1];
                worksheet.Name = "cutting bcs base on std" + DateTime.Now.ToString("yyyyMMdd");
                #region set CheckDate & Factory
                worksheet.Cells[2, 2] = this.SewingStart + " - " + this.SewingEnd;
                worksheet.Cells[2, 5] = this.Factory;
                #endregion
                MyUtility.Excel.CopyToXls(this.printData, string.Empty, "Subcon_R44_ByFactory.xltx", 3, showExcel: true, excelApp: objApp);
                #endregion
            }
            else
            {
                #region By SPNO
                objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Subcon_R44_BySPNO.xltx");
                worksheet = objApp.Sheets[1];
                worksheet.Name = "cutting bcs base on std" + DateTime.Now.ToString("yyyyMMdd");
                #region set CheckDate & Factory
                worksheet.Cells[2, 3] = this.SewingStart + " - " + this.SewingEnd;
                worksheet.Cells[2, 6] = this.Factory;
                #endregion
                MyUtility.Excel.CopyToXls(this.printData, string.Empty, "Subcon_R44_BySPNO.xltx", 3, showExcel: true, excelApp: objApp);
                #endregion

            }

            Marshal.ReleaseComObject(worksheet);
            this.HideWaitMessage();
            return true;
        }
    }
}
