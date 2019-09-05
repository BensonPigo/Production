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
    public partial class R44 : Sci.Win.Tems.PrintForm
    {
        string Factory, SewingStart, SewingEnd, SP;
        DataTable printData;
        public R44(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.print.Visible = false;

            //set ComboFactory
            DataTable dtFactory;
            DBProxy.Current.Select(null, @"
select ID = ''

union all
Select ID 
from Factory WITH (NOLOCK)
where Junk != 1", out dtFactory);
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
            #region SQLParameter
            List<SqlParameter> sqlParameter = new List<SqlParameter>();
            sqlParameter.Add(new SqlParameter("@Factory", Factory));
            sqlParameter.Add(new SqlParameter("@SewingStart", SewingStart));
            sqlParameter.Add(new SqlParameter("@SewingEnd", SewingEnd));
            sqlParameter.Add(new SqlParameter("@SPNO", SP));
            sqlParameter.Add(new SqlParameter("@ToExcelBy", (radioByFactory.Checked) ? 1 : 0));
            #endregion

            #region SQL cmd
            DBProxy.Current.DefaultTimeout = 1800;  // timeout時間加長為30分鐘
            string strSQL = string.Format(@"
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
into #tsp
from orders o WITH (NOLOCK)
inner join Factory f WITH (NOLOCK) on o.FactoryID = f.ID
where	o.Junk != 1
		-- {0} 篩選 OrderID
		{0}
		and not ((o.SewOffLine < @StartDate and o.SewInLine < @EndDate) or (o.SewInLine > @StartDate and o.SewInLine > @EndDate))
		and (o.SewInLine is not null or o.SewInLine != '')
		and (o.SewOffLine is not null or o.SewOffLine != '')	
		-- {1} 篩選 FactoryID	
        {1}
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



/*
*	準備好要印的資料
*/

select
t.FactoryID,
[SP] = t.orderID,
t.StyleID,
[SewingDate] =  cb.SewDate,
[Line] = t.SewLine,
[AccuLoad] = AccuLoad.val
into #print0
from #tsp t
cross join #CBDate cb
outer apply(SELECT val = isnull(sum(FinishedQtyBySet),0) FROM DBO.QtyBySetPerSubprocess(t.orderID,'LOADING',DEFAULT ,cb.SewDate,DEFAULT,DEFAULT,1,1)) as AccuLoad


select FactoryID
		, SP
		, StyleID
		, SewingDate 
		, Line
		, AccuStd = iif(isnull(x4.StdQ,0) > isnull(Upperlimit.qty,0),isnull(Upperlimit.qty,0), isnull(x4.StdQ,0))
		, AccuLoad 
into #print
from #print0 p
outer apply(
	select top 1 x3.StdQ,x3.Date
	from(        
		select Date,StdQ=sum(x2.StdQ) over(order by x2.Date)
        from(
            select Date,StdQ=sum(StdQ) 
            from dbo.[getDailystdq](p.SP)
            group by Date
        )x2

	)x3
	where x3.Date <= p.SewingDate
	order by Date desc
)x4
outer apply(	
	SELECT qty = sum(qty)
			FROM Order_QtyShip 
			WHERE ID =p.SP
)Upperlimit

/*
*	判斷 ToExcel & 算出 BCS = Round(loading / std * 100, 2)
*	分母不能 = 0
*/
IF @ByFactory = 1
	select  FactoryID
			, SewingDate
			, Std = sum(AccuStd)
			, Loading = sum(AccuLoad) 
			, BCS = iif(ROUND(sum(AccuLoad) / iif(sum(AccuStd) = 0, 1, sum(AccuStd)) * 100, 2) >= 100, 100
																									 , ROUND(sum(AccuLoad) / iif(sum(AccuStd) = 0, 1, sum(AccuStd)) * 100, 2))
	from #print
	where SewingDate between @StartDate and @EndDate
	group by FactoryID, SewingDate
	having not (sum(AccuStd) = 0 and sum(AccuLoad) =0 )
	order by FactoryID, SewingDate
Else  

select	p.FactoryID,p.SP,p.StyleID,p.SewingDate,p.Line,p.AccuStd,p.AccuLoad
		, BCS = iif(BCS.value >= 100, 100, BCS.value)
	from #print p
    outer apply (
    	select value = ROUND(p.AccuLoad / iif(AccuStd = 0, 1, AccuStd) * 100, 2)
    ) BCS
where SewingDate between @StartDate and @EndDate 
and not (p.AccuLoad = 0 and p.AccuStd =0 )
order by p.FactoryID,p.SP,p.SewingDate,p.Line

drop table #tsp
drop table #print
drop table #CBDate
DROP TABLE #print0
"
                , (SP.Empty()) ? "" : "and o.id = @SP"
                , (Factory.Empty()) ? "" : "and f.ID = @FactoryID");
            #endregion

            DualResult result = DBProxy.Current.Select(null, strSQL, sqlParameter, out printData);
            if (!result)
            {
                return result;
            }
            DBProxy.Current.DefaultTimeout = 300;  // timeout時間調回為5分鐘
            return Result.True;
        }

        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            if (printData == null || printData.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found.");
                return false;
            }
            this.SetCount(printData.Rows.Count);
            this.ShowWaitMessage("Excel Processing...");

            Excel.Application objApp = null;
            Excel.Worksheet worksheet = null;
            if (radioByFactory.Checked)
            {
                #region By Factory
                objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Subcon_R44_ByFactory.xltx");
                worksheet = objApp.Sheets[1];
                worksheet.Name = "cutting bcs base on std" + (DateTime.Now).ToString("yyyyMMdd");
                #region set CheckDate & Factory
                worksheet.Cells[2, 2] = SewingStart + " - " + SewingEnd;
                worksheet.Cells[2, 5] = Factory;
                #endregion
                MyUtility.Excel.CopyToXls(printData, "", "Subcon_R44_ByFactory.xltx", 3, showExcel: true, excelApp: objApp);                
                #endregion
            }
            else
            {
                #region By SPNO
                objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Subcon_R44_BySPNO.xltx");
                worksheet = objApp.Sheets[1];
                worksheet.Name = "cutting bcs base on std" + (DateTime.Now).ToString("yyyyMMdd");
                #region set CheckDate & Factory
                worksheet.Cells[2, 3] = SewingStart + " - " + SewingEnd;
                worksheet.Cells[2, 6] = Factory;
                #endregion
                MyUtility.Excel.CopyToXls(printData, "", "Subcon_R44_BySPNO.xltx", 3, showExcel: true, excelApp: objApp);                
                #endregion
                
            }

            Marshal.ReleaseComObject(worksheet);
            this.HideWaitMessage();
            return true;
        }
    }
}
