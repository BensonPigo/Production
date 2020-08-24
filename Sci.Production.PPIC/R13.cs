using Ict;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using Sci.Win;
using Sci.Data;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// R13
    /// </summary>
    public partial class R13 : Win.Tems.PrintForm
    {
        private string tsql;
        private List<SqlParameter> sqlpar = new List<SqlParameter>();
        private DataTable[] dts;

        /// <summary>
        /// Initializes a new instance of the <see cref="R13"/> class.
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public R13(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.sqlpar.Clear();
            string where1 = string.Empty;
            string where2 = string.Empty;
            string whereBuyer = string.Empty;
            string sql_Exclude_holiday = string.Empty;
            #region 檢核必輸入條件 sql參數
            if (!this.dateRangeReadyDate.HasValue)
            {
                MyUtility.Msg.WarningBox("Ready Date cannot be empty!");
                return false;
            }
            else
            {
                this.sqlpar.Add(new SqlParameter("@inputReadyDateFrom", this.dateRangeReadyDate.DateBox1.Value));
                this.sqlpar.Add(new SqlParameter("@inputReadyDateTo", this.dateRangeReadyDate.DateBox2.Value));
            }

            if (this.dateRangeBuyerDelivery.HasValue)
            {
                // this.sqlpar.Add(new SqlParameter("@inputReadyDateFrom", this.dateRangeReadyDate.DateBox1.Value));
                // this.sqlpar.Add(new SqlParameter("@inputReadyDateTo", this.dateRangeReadyDate.DateBox2.Value));
                whereBuyer += $@" and O.BuyerDelivery between '{((DateTime)this.dateRangeBuyerDelivery.DateBox1.Value).ToString("yyyy/MM/dd")}' and '{((DateTime)this.dateRangeBuyerDelivery.DateBox2.Value).ToString("yyyy/MM/dd")}'";
            }

            whereBuyer += this.chkIncludeCancelOrder.Checked ? string.Empty : " and o.Junk = 0 ";

            if (!MyUtility.Check.Empty(this.txtMdivision.Text))
            {
                where1 += $" and MDivisionID = '{this.txtMdivision.Text}'";
            }

            if (!MyUtility.Check.Empty(this.txtFactory.Text))
            {
                where1 += $" and FtyGroup = '{this.txtFactory.Text}'";
                where2 = $" and id = '{this.txtFactory.Text}'";
            }

            if (!MyUtility.Check.Empty(this.txtBrand.Text))
            {
                where1 += $" and BrandID = '{this.txtBrand.Text}'";
            }

            this.sqlpar.Add(new SqlParameter("@inputGAP", MyUtility.Check.Empty(this.numericBoxDateGap.Value) ? 0 : this.numericBoxDateGap.Value));

            if (this.checkHoliday.Checked)
            {
                sql_Exclude_holiday = $@"
--針對ReadyDate最後一天判斷若是星期天或是假日的話就往後延
WHILE @GAPfrequency <= @GAP 
BEGIN 
    update #WorkDate set WorkDate = DATEADD(DAY,1,WorkDate)
    WHILE @addDate <= 100 
    BEGIN  
    	update w set WorkDate = DATEADD(DAY,1,WorkDate)
    		from #WorkDate w
    		where (exists(select 1 from Holiday h where w.Factory = h.FactoryID and w.WorkDate = h.HolidayDate) or datepart(WEEKDAY,w.WorkDate)  = 1)	
    	IF(@@ROWCOUNT = 0 )
    	Begin
    		break
    	End
    	set @addDate = @addDate +1
    END  
    set @GAPfrequency = @GAPfrequency +1
END
--刪掉星期天
delete #WorkDate where datepart(WEEKDAY,ReadyDate) = 1

--刪掉各工廠對應的假日
delete w
from #WorkDate  w
inner join Holiday h on w.Factory = h.FactoryID and w.ReadyDate = h.HolidayDate";
            }
            else
            {
                sql_Exclude_holiday = $@"
--針對ReadyDate最後一天判斷若是星期天的話就往後延
WHILE @GAPfrequency <= @GAP 
BEGIN 
    update #WorkDate set WorkDate = DATEADD(DAY,1,WorkDate)
    WHILE @addDate <= 100 
    BEGIN  
    	update w set WorkDate = DATEADD(DAY,1,WorkDate)
    		from #WorkDate w
    		where datepart(WEEKDAY,w.WorkDate)  = 1
    	IF(@@ROWCOUNT = 0 )
    	Begin
    		break
    	End
    	set @addDate = @addDate +1
    END  
    set @GAPfrequency = @GAPfrequency +1
END  
--刪掉星期天
delete #WorkDate where datepart(WEEKDAY,ReadyDate) = 1 ";
            }
            #endregion

            #region main sql

            this.tsql = $@"
declare @time time = '{(this.txtTime.Text.Equals(":") ? "00:00:00" : this.txtTime.Text)}'
declare @GAP int = @inputGAP
declare @GAPfrequency int = 1
declare @ReadyDateFrom date = @inputReadyDateFrom
declare @ReadyDateTo date = @inputReadyDateTo
declare @addDate int = 0

SELECT [Factory] = Factory.ID,DATEADD(DAY,number,@ReadyDateFrom) [WorkDate],DATEADD(DAY,number,@ReadyDateFrom) [ReadyDate]
into #WorkDate
FROM master..spt_values s
inner join Factory on 1 = 1 {where2}
and IsProduceFty = 1
WHERE s.type = 'P'
AND DATEADD(DAY,number,@ReadyDateFrom) <= @ReadyDateTo

{sql_Exclude_holiday}


--抓出條件時間內對應的orders資料
--依照指定的Ready Date+條件GAP天數去抓去Orders.SewOffLIne在那天生產結束的訂單，且Buyer Delivery date >= Ready Date的訂單，若該Ready Date星期日、特殊假日(Holiday)不需計算，星期日、特殊假日(Holiday)都要避開，例如Ready date是 7/28 GAP是1，但隔天是週日(7/29)，所以抓取orders.SewOffLIne時間點是7/30。
--依照指定的Ready Date 找到相同的 Buyer Delivery date的訂單且 SewOffLIne > Buyer Delivery date的資料，若該Ready Date星期日、特殊假日(Holiday)不需計算。
/*
* 先用下線日SewingOfflineDate去撈取訂單資料, 再使用Order_QtyShip.BuyerDelivery 去串ReadyDate,如果有相同訂單則取最小值
*/

select o.* 
,[WorkDate] = w.WorkDate
,[WorkTime] = convert(datetime, w.WorkDate) + convert(datetime,@time)
,[OriReadyDate] = w.ReadyDate
,[ShipModeStatus] = Shipmode.ShipStatus
into #tmpOrderOffLine
from Orders o with (nolock)
inner join #WorkDate w on w.Factory = o.FtyGroup 
and w.WorkDate = o.SewOffLine
outer apply(
    select ShipStatus = 
	stuff((
	        select concat(',', format(os.BuyerDelivery,'yyyy/MM/dd') + ' - ' + convert(varchar(10), sum (os.Qty)))
	        from order_QtyShip os with(nolock)
	        where os.id = o.ID 
            group by os.BuyerDelivery
		    for xml path('')
	),1,1,'')
)Shipmode
where 1=1
{whereBuyer}

select *
into #orders_tmp
from	
(
    select * 
    from #tmpOrderOffLine

    union

    select o.*
            , [WorkDate] = oq.WorkDate
            , [WorkTime] = convert(datetime, oq.WorkDate) + convert(datetime,@time)
            , [OriReadyDate] = oq.ReadyDate
            , [ShipModeStatus] = Shipmode.ShipStatus
    from (
	    select oq.Id
	            , [BuyerDelivery] = min(oq.BuyerDelivery)
	            , w.ReadyDate
                , w.WorkDate
	    from Order_QtyShip oq with (nolock)
	    inner join Orders o with (nolock) on o.ID = oq.Id
	    inner join #WorkDate w on w.Factory = o.FtyGroup
	                              and w.WorkDate = oq.BuyerDelivery
	    where o.id not in (select id from #tmpOrderOffLine)
              {whereBuyer}
	    group by oq.Id, w.ReadyDate, w.WorkDate
    ) oq
    inner join orders o with (nolock) on oq.id=o.id
	outer apply(
        select ShipStatus = 
			stuff((
	            select concat(',', format(os.BuyerDelivery,'yyyy/MM/dd') + ' - ' + convert(varchar(10), sum (os.Qty)))
	            from order_QtyShip os with(nolock)
	            where os.id = o.ID 
                group by os.BuyerDelivery
		        for xml path('')
	    ),1,1,'')
	) Shipmode  
) as a 
where Category = 'B' 
{where1}

select distinct 
    orderid = id
    ,InStartDate = Null
    ,InEndDate = WorkTime
    ,OutStartDate = Null
    ,OutEndDate = WorkTime
into #enn
from #orders_tmp
";

            string[] subprocessIDs = new string[] { "Emb", "BO", "PRT", "AT", "PAD-PRT", "SUBCONEMB", "HT", "Loading", "SORTING" };
            string qtyBySetPerSubprocess = PublicPrg.Prgs.QtyBySetPerSubprocess(subprocessIDs, "#enn", bySP: true, isNeedCombinBundleGroup: true, isMorethenOrderQty: "1");
            this.tsql += qtyBySetPerSubprocess + $@"
--抓取subprocess in
select	[M] = o.MDivisionID,
		[Factory] = o.FtyGroup,
		[SP] = o.ID,
		[SubProcessID] = subporcessQty.SubprocessId,
		[FinishedQtyBySet] = subporcessQty.FinishedQtyBySet
into #tmpInOut
from #orders_tmp o
outer apply ( 
    select SubprocessId='Emb',FinishedQtyBySet from #Emb t where t.OrderID = o.ID
    union all
    select SubprocessId='BO',FinishedQtyBySet from #BO t where t.OrderID = o.ID
    union all
    select SubprocessId='PRT',FinishedQtyBySet from #PRT t where t.OrderID = o.ID
    union all
    select SubprocessId='AT',FinishedQtyBySet from #AT t where t.OrderID = o.ID
    union all
    select SubprocessId='PAD-PRT',FinishedQtyBySet from #PADPRT t where t.OrderID = o.ID
    union all
    select SubprocessId='SUBCONEMB',FinishedQtyBySet from #SUBCONEMB t where t.OrderID = o.ID
    union all
    select SubprocessId='HT',FinishedQtyBySet from #HT t where t.OrderID = o.ID
    union all
    select SubprocessId='Loading',FinishedQtyBySet from #Loading t where t.OrderID = o.ID
) subporcessQty

select *
into #tmpInOutFin
from 
(select [M],[Factory],[SP],[Subprocessid],FinishedQtyBySet
from #tmpInOut 
) as a
pivot
	(
	  sum(FinishedQtyBySet)
	  for Subprocessid in ( [AT],[BO],[Emb],[HT],[PAD-PRT],[PRT])
	)as pvt

-----------cutting完成成衣件數-------------------------------------------------------------------------------------------------------------------------
--先找所有部位
Select distinct sp = o.ID,wd.SizeCode,wd.article,occ.PatternPanel,o.MDivisionID
into #pOffline 
from  #orders_tmp a--------從orderid出發
inner join Orders o WITH (NOLOCK) on o.id = a.ID
inner join WorkOrder_Distribute wd WITH (NOLOCK) on o.id = wd.OrderID
inner join Order_ColorCombo occ WITH (NOLOCK) on o.poid = occ.id and occ.Article = wd.Article
inner join order_Eachcons cons WITH (NOLOCK) on occ.id = cons.id and cons.FabricCombo = occ.PatternPanel and cons.CuttingPiece='0'
where occ.FabricCode !='' and occ.FabricCode is not null 

--以及這趟撈的資料sum(Qty) by SizeCode,Article,PatternPanel,sp
select wd.Qty,wd.SizeCode,wd.Article,wp.PatternPanel,co.MDivisionid,sp = a.id,co.EditDate,a.WorkTime
into #tmpc0
from #orders_tmp a
inner join WorkOrder_Distribute wd WITH (NOLOCK) on wd.orderid = a.ID 
inner join workorder w WITH (NOLOCK) on w.Ukey = wd.WorkOrderUkey
left join WorkOrder_PatternPanel wp WITH (NOLOCK) on wp.WorkOrderUkey = wd.WorkOrderUkey
inner join CuttingOutput_Detail cod WITH (NOLOCK) on cod.CutRef = w.CutRef
inner join CuttingOutput co WITH (NOLOCK) on co.id = cod.id and co.MDivisionid = w.MDivisionId and co.Status <> 'New'

select [Qty] = sum(Qty),SizeCode,Article,PatternPanel,MDivisionid,sp
into #tmpc
from #tmpc0
where EditDate <= WorkTime
group by SizeCode,Article,PatternPanel,MDivisionid,sp

--缺的部位null為0
select a.sp,a.SizeCode,a.Article,a.PatternPanel,a.MDivisionID,Qty=isnull(b.Qty,0)
into #tmpP
from #pOffline a 
left join #tmpc b on a.sp = b.sp and a.Article = b.Article and a.SizeCode = b.SizeCode and a.PatternPanel = b.PatternPanel and a.MDivisionID = b.MDivisionid

select cutqty = min(qty),sp,Article,SizeCode,MDivisionid into #tmpc2 from #tmpP group by sp,Article,SizeCode,MDivisionid--取最小 by size
select cutqty = sum(cutqty),sp into #tmpc3 from #tmpc2 group by sp--總和為此orderid完成數
-----end cutting完成成衣件數

--sheet1 SUBPROCESS READY DATE
select	o.MDivisionID,
		o.FtyGroup,
		o.BuyerDelivery,
	    o.SciDelivery,
		o.ID,
        [CancelOrder] = IIF(o.Junk=1,'Y','N'),
		c.Alias,
		o.StyleID,
		o.CustPONo,
		o.BrandID,
		o.Qty,
		[SewingOutput] = (select MIN(isnull(tt.qaqty,0)) 
                          from dbo.style_location sl WITH (NOLOCK) 
                          left join (
                                SELECT b.ComboType
                                       , qaqty = sum(b.QAQty)  
                                FROM DBO.SewingOutput a WITH (NOLOCK) 
                                inner join dbo.SewingOutput_Detail b WITH (NOLOCK) on b.ID = a.ID
                                where b.OrderId = o.ID and a.OutputDate <= o.WorkDate
                                group by ComboType 
                          ) tt on tt.ComboType = sl.Location
                          where sl.StyleUkey = o.StyleUkey) ,
		[BalanceQty] = '=INDIRECT({"\""}I{"\""} & ROW()) - INDIRECT({"\""}J{"\""} & ROW())',
        [CuttingStatus] = iif(cuttingQty.value >= o.Qty, 'Y', ''),
		o.SewInLine,
		o.SewOffLine,
		o.FtyCTN,
        [SewingLineID] = SewingSchedule.SewingLineID,
		[AT] = SubProcess.AT,
		[BONDING] = SubProcess.BO,
		[EMBRO] = SubProcess.Emb,
		[HT] = SubProcess.HT,
		[PAD-PRT] = SubProcess.[PAD-PRT],
		[PRINTING] = SubProcess.PRT,
		[SubCon] = ls.abb,
		[ReadyDate] = o.OriReadyDate,
		[LoadingStatus] = iif(LoadingQty.value >= o.Qty,'Y',''),
		[Ready] = case when subprocessqty.chksubprocesqty >= inoutcount.ct then 'Y' end,
		[ShipModeStatus]
into #detailResult
from #orders_tmp o 
left join Country c with (Nolock) on c.id= o.Dest
left join Order_TmsCost otc with (nolock) on o.id = otc.id and ArtworkTypeID = 'Printing'
left join Localsupp ls with (nolock) on otc.LocalSuppID= ls.id
left join #tmpInOutFin SubProcess with (Nolock) on SubProcess.SP = o.ID
outer apply(
select SewingLineID = stuff((
select distinct concat('/', ss.SewingLineID)
from[SewingSchedule] ss with(nolock)
where ss.orderid = o.ID 
		  for xml path('')
),1,1,'')
) SewingSchedule
outer apply(
    select[value] = FinishedQtyBySet from #tmpInOut where SP = o.ID and SubProcessID = 'Loading'
) LoadingQty
outer apply(
    select ct = count(1)
    from #tmpInOut
	where SP = o.ID
    and Factory = o.FtyGroup and FinishedQtyBySet is not null
    and SubProcessId in ('Emb','BO','PRT','AT','PAD-PRT','HT','Loading')
)inoutcount
outer apply(
    select chksubprocesqty = sum(xxx.Accusubprocesqty)
    from(
        select[Accusubprocesqty] = iif(FinishedQtyBySet >= o.Qty, 1, 0)
        from #tmpInOut
		where SP = o.ID
        and Factory = o.FtyGroup
        and SubProcessId in ('Emb','BO','PRT','AT','PAD-PRT','HT','Loading')
	)xxx
)subprocessqty
outer apply(
    select value = cutqty
    from #tmpc3 where sp = o.ID
) cuttingQty

select  FtyGroup,
		BuyerDelivery,
	    SciDelivery,
		ID,
        [CancelOrder],
		Alias,
		StyleID,
		CustPONo,
		BrandID,
		Qty,
		[SewingOutput] ,
		[BalanceQty] = Qty - [SewingOutput],
        [CuttingStatus],
		SewInLine,
		SewOffLine,
		FtyCTN,
        [SewingLineID],
		[AT] ,
		[BONDING],
		[EMBRO],
		[HT],
		[PAD-PRT],
		[PRINTING],
		[SubCon],
		[ReadyDate],
		[LoadingStatus],
		[Ready],
		[ShipModeStatus]
        from #detailResult


--sheet2 SUMMARY By Factory
SELECT FtyGroup,
       [ByFtyCnt] = count(*),
       [ByFtyClose] = sum(iif([Ready] = 'Y', 1, 0)),
       [ByFtyfail] = sum(iif([Ready] = 'Y', 0, 1))
from #detailResult
group by FtyGroup
order by FtyGroup

--sheet3 Summary By Factory - SubProcess
select t.Factory
, t.SubProcessID
, [BySubProcessCnt] = count(*)
, [BySubProcessClose] = case when t.SubProcessID = 'Loading' then sum(IIF([LoadingStatus] = 'Y', 1, 0))
	else sum(IIF(t.FinishedQtyBySet >= s.Qty ,1,0)) end
, [BySubProcessfail] = case when t.SubProcessID = 'Loading' then sum(IIF([LoadingStatus] = 'Y', 0, 1))
	else sum(IIF(t.FinishedQtyBySet < s.Qty ,1,0)) end
from #tmpInOut t
inner join #detailResult s on s.ID = t.SP
and t.Factory = s.FtyGroup
where t.SubProcessID in ('Emb','BO','PRT','AT','PAD-PRT','HT','Loading')
group by t.Factory,t.SubProcessID
order by t.Factory,t.SubProcessID

-- Sheet 4 Summary By Printing Subcon  
SELECT FtyGroup
,SubCon
       ,[PrintingCnt] = count(*)
       ,[PrintingClose] = sum(iif([PRINTING] >= Qty, 1, 0))
       ,[Printingfail] = sum(iif([PRINTING] < Qty, 1, 0))
from #detailResult
where [SubCon] is not null
and [PRINTING] is not null 
group by FtyGroup,SubCon
order by FtyGroup,SubCon


drop table #pOffline,#tmpP,#tmpc2,#tmpc3,#tmpc,#tmpc0
drop table #tmpInOut,#tmpInOutFin,#WorkDate,#orders_tmp
drop table #detailResult,#tmpOrderOffLine

";
            #endregion
            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            DBProxy.Current.DefaultTimeout = 2700;
            DualResult result = DBProxy.Current.Select(string.Empty, this.tsql, this.sqlpar, out this.dts);
            DBProxy.Current.DefaultTimeout = 300;
            return result;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(ReportDefinition report)
        {
            DataTable dtDetail = this.dts[0];
            DataTable dtSummary = this.dts[1];
            DataTable dtBySubProcess = this.dts[2];
            DataTable dtByPrinting = this.dts[3];
            if (dtDetail.Rows.Count == 0)
            {
                MyUtility.Msg.ErrorBox("Data not found");
                return false;
            }

            this.ShowWaitMessage("Excel Processing...");
            this.SetCount(dtDetail.Rows.Count); // 顯示筆數

            Excel.Application objApp = new Excel.Application();
            Utility.Report.ExcelCOM com = new Utility.Report.ExcelCOM(Env.Cfg.XltPathDir + "\\PPIC_R13.xltx", objApp);
            Excel.Worksheet worksheet;

#if DEBUG
            objApp.Visible = true;
#endif

            // sheet1 data
            com.WriteTable(dtDetail, 2);

            #region sheet2 data
            worksheet = objApp.Sheets[2];
            worksheet.Select();

            // 如果超過1個先複製外框
            if (dtSummary.Rows.Count > 1)
            {
                worksheet.get_Range("A3:E3").Copy();
                Excel.Range to = worksheet.get_Range($"A3:E{(2 + dtSummary.Rows.Count).ToString()}");
                to.PasteSpecial(Excel.XlPasteType.xlPasteAll);
            }

            com.WriteTable(dtSummary, 3);

            #endregion

            #region sheet3 data
            worksheet = objApp.Sheets[3];
            worksheet.Select();

            // 如果超過1個先複製外框
            if (dtBySubProcess.Rows.Count > 1)
            {
                worksheet.get_Range("A3:F3").Copy();
                Excel.Range to = worksheet.get_Range($"A3:F{(2 + dtBySubProcess.Rows.Count).ToString()}");
                to.PasteSpecial(Excel.XlPasteType.xlPasteAll);
            }

            com.WriteTable(dtBySubProcess, 3);
            #endregion

            #region Sheet4 data
            worksheet = objApp.Sheets[4];
            worksheet.Select();

            // 如果超過1個先複製外框
            if (dtByPrinting.Rows.Count > 1)
            {
                worksheet.get_Range("A3:F3").Copy();
                Excel.Range to = worksheet.get_Range($"A3:F{(2 + dtByPrinting.Rows.Count).ToString()}");
                to.PasteSpecial(Excel.XlPasteType.xlPasteAll);
            }

            com.WriteTable(dtByPrinting, 3);
            #endregion

            com.ExcelApp.ActiveWorkbook.Sheets[1].Select(Type.Missing);
            objApp.Visible = true;
            Marshal.ReleaseComObject(worksheet);
            Marshal.ReleaseComObject(objApp);
            this.HideWaitMessage();
            return true;
        }
    }
}
