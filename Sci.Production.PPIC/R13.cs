using Ict;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        /// R13
        /// </summary>
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
WHILE @addDate <= 100 
BEGIN  
	update w set WorkDate = DATEADD(DAY,1,WorkDate)
		from #WorkDate w
		where ReadyDate = @ReadyDateTo and 
			  (exists(select 1 from Holiday h where w.Factory = h.FactoryID and w.WorkDate = h.HolidayDate) or datepart(WEEKDAY,w.WorkDate)  = 1)	
	IF(@@ROWCOUNT = 0 )
	Begin
		break
	End
	set @addDate = @addDate +1
END  

--刪掉星期天
delete #WorkDate where datepart(WEEKDAY,WorkDate) = 1 or datepart(WEEKDAY,ReadyDate) = 1

--刪掉各工廠對應的假日
delete w
from #WorkDate  w
inner join Holiday h on w.Factory = h.FactoryID and (w.WorkDate = h.HolidayDate or w.ReadyDate = h.HolidayDate)";
            }
            else
            {
                sql_Exclude_holiday = $@"
--針對ReadyDate最後一天判斷若是星期天的話就往後延
WHILE @addDate <= 100 
BEGIN  
	update w set WorkDate = DATEADD(DAY,1,WorkDate)
		from #WorkDate w
		where ReadyDate = @ReadyDateTo and datepart(WEEKDAY,w.WorkDate)  = 1
	IF(@@ROWCOUNT = 0 )
	Begin
		break
	End
	set @addDate = @addDate +1
END  

--刪掉星期天
delete #WorkDate where datepart(WEEKDAY,WorkDate) = 1 or datepart(WEEKDAY,ReadyDate) = 1 ";
            }
            #endregion

            #region main sql
            this.tsql = $@"
declare @time time = '{(this.txtTime.Text.Equals(":") ? "00:00:00" : this.txtTime.Text)}'
declare @GAP int = @inputGAP
declare @ReadyDateFrom date = @inputReadyDateFrom
declare @ReadyDateTo date = @inputReadyDateTo
declare @addDate int = 0

SELECT [Factory] = Factory.ID,DATEADD(DAY,number,@ReadyDateFrom) [WorkDate],DATEADD(DAY,number,@ReadyDateFrom) [ReadyDate]
into #WorkDate
FROM master..spt_values s
inner join Factory on 1 = 1 {where2}
WHERE s.type = 'P'
AND DATEADD(DAY,number,@ReadyDateFrom) <= @ReadyDateTo

update #WorkDate set WorkDate = DATEADD(DAY,@GAP,WorkDate)

{sql_Exclude_holiday}

--抓出條件時間內對應的orders資料
--依照指定的Ready Date+條件GAP天數去抓去Orders.SewOffLIne在那天生產結束的訂單，且Buyer Delivery date >= Ready Date的訂單，若該Ready Date星期日、特殊假日(Holiday)不需計算，星期日、特殊假日(Holiday)都要避開，例如Ready date是 7/28 GAP是1，但隔天是週日(7/29)，所以抓取orders.SewOffLIne時間點是7/30。
--依照指定的Ready Date 找到相同的 Buyer Delivery date的訂單且 SewOffLIne > Buyer Delivery date的資料，若該Ready Date星期日、特殊假日(Holiday)不需計算。
select *
into #orders_tmp
from	
(
select o.* ,
[WorkDate] = w.WorkDate,
[WorkTime] = convert(datetime, w.WorkDate) + convert(datetime,@time),
[OriReadyDate] = w.ReadyDate
from Orders o with (nolock)
inner join #WorkDate w on w.Factory = o.FtyGroup and w.WorkDate = o.SewOffLine and o.BuyerDelivery >= w.WorkDate
union
select o.* ,
[WorkDate] = w.WorkDate,
[WorkTime] = convert(datetime, w.WorkDate) + convert(datetime,@time),
[OriReadyDate] = w.ReadyDate
from Orders o with (nolock)
inner join #WorkDate w on w.Factory = o.FtyGroup and w.WorkDate = o.BuyerDelivery 
where o.SewOffLine > o.BuyerDelivery
) as a 
where Category = 'B' and Junk = 0 {where1}
--select ID,FtyGroup,SewOffLine,BuyerDelivery from #orders_tmp order by SewOffLine

--抓取subprocess in
Select DISTINCT
    [Bundleno] = bd.BundleNo,
    [Cut Ref#] = b.CutRef,
    [M] = b.MDivisionid,
    [Factory] = o.FtyGroup,
    [SP] = b.Orderid,
    SubProcessId = s.Id,
	b.article,
    [Size] = bd.SizeCode,
    [Comb] = b.PatternPanel,
	b.FabricPanelCode,
    bd.PatternCode,
    bd.Qty,
    bio.InComing,
    bio.OutGoing,
    o.WorkTime 
into #tmp
from Bundle b WITH (NOLOCK) 
inner join Bundle_Detail bd WITH (NOLOCK) on bd.Id = b.Id
left join Bundle_Detail_Art bda WITH (NOLOCK) on bda.Id = bd.Id and bda.Bundleno = bd.Bundleno
inner join #orders_tmp o WITH (NOLOCK) on o.Id = b.OrderId
inner join SubProcess s WITH (NOLOCK) on (s.IsRFIDDefault = 1 or s.Id = bda.SubprocessId) 
left join BundleInOut bio WITH (NOLOCK) on bio.Bundleno=bd.Bundleno and bio.SubProcessId = s.Id

------
select [M],[Factory],[SP],[Subprocessid],article,[Size],[Comb],FabricPanelCode,PatternCode,accuQty = sum(iif(InComing is null ,0,Qty))
into #tmp2
from #tmp where InComing <= WorkTime
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

select *
into #tmpin2
from 
(select [M],[Factory],[SP],[Subprocessid],accuQty = sum(accuQty)
from #tmp4 
group by [M],[Factory],[SP],[Subprocessid] 
) as a
pivot
	(
	  sum(accuQty)
	  for Subprocessid in ( [AT],[BO],[Emb],[HT],[PAD-PRT],[PRT])
	)as pvt

--抓取subprocess output
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

select *
into #tmpoutFin
from 
(select [M],[Factory],[SP],[Subprocessid],accuQty = sum(accuQty)
from #tmpout4 
group by [M],[Factory],[SP],[Subprocessid] 
) as a
pivot
	(
	  sum(accuQty)
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
        [CuttingStatus] = iif(cuttingQty.value  >= o.Qty,'Y',''),
		o.SewInLine,
		o.SewOffLine,
		o.FtyCTN,
        [SewingLineID] = SewingSchedule.SewingLineID,
		[AT] = isnull(SubProcess.AT,iif(SubProcessOut.AT is not null,0,null)),
		[BONDING] = isnull(SubProcess.BO,iif(SubProcessOut.BO is not null,0,null)),
		[EMBRO] = isnull(SubProcess.Emb,iif(SubProcessOut.Emb is not null,0,null)),
		[HT] = isnull(SubProcess.HT,iif(SubProcessOut.HT is not null,0,null)),
		[PAD-PRT] = isnull(SubProcess.[PAD-PRT],iif(SubProcessOut.[PAD-PRT] is not null,0,null)),
		[PRINTING] = isnull(SubProcess.PRT,iif(SubProcessOut.PRT is not null,0,null)),
		[SubCon] = ls.abb,
		[ReadyDate] = o.OriReadyDate,
		[LoadingStatus] = iif(LoadingQty.value >= o.Qty,'Y',''),
		[Ready] = case when subprocessqty.chksubprocesqty >= inoutcount.ct then 'Y' end
into #detailResult
from #orders_tmp o 
left join Country c with (Nolock) on c.id= o.Dest
left join Order_TmsCost otc with (nolock) on o.id = otc.id and ArtworkTypeID = 'Printing'
left join Localsupp ls with (nolock) on otc.LocalSuppID=ls.id
left join #tmpin2 SubProcess with (Nolock) on SubProcess.SP = o.ID
left join #tmpoutFin SubProcessOut with (Nolock) on SubProcessOut.SP = o.ID
outer apply(
    select SewingLineID = stuff((
          select distinct concat('/', ss.SewingLineID)
          from[SewingSchedule] ss with(nolock)
          where ss.orderid = o.ID 
		  for xml path('')
      ),1,1,'')
) SewingSchedule
outer apply(
    select[value] = sum(accuQty) from #tmpout4 where SP = o.ID
) LoadingQty
outer apply(
    select ct = count(1) * 2
    from #tmpout
	where #tmpout.SP = o.ID 
	and #tmpout.Factory = o.FtyGroup
	and #tmpout.SubProcessId in('Emb','BO','PRT','AT','PAD-PRT','SUBCONEMB','HT')
)inoutcount
outer apply(
    select chksubprocesqty = sum(xxx.Accusubprocesqty)
    from(
        select[Accusubprocesqty] = iif(iif(#tmpin.AccuQty > o.Qty, o.Qty, #tmpin.AccuQty)>=o.Qty,1,0)+
									iif(iif(#tmpout.AccuQty > o.Qty, o.Qty, #tmpout.AccuQty)>=o.Qty,1,0)
		from #tmpin,#tmpout
		where #tmpin.SP = o.ID 
		and #tmpin.Factory = o.FtyGroup
		and #tmpin.SubProcessId in('Emb','BO','PRT','AT','PAD-PRT','SUBCONEMB','HT','Loading','SORTING')
		and #tmpout.SP = o.ID 
	    and #tmpout.Factory = o.FtyGroup
		and #tmpout.SubProcessId = #tmpin.SubProcessId
	)xxx
)subprocessqty
outer apply(
	select value = cutqty
	from #tmpc3 where sp = o.ID
) cuttingQty

select	FtyGroup,
		BuyerDelivery,
	    SciDelivery,
		ID,
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
		[Ready]
from #detailResult

--sheet2 SUMMARY
select MDivisionID, FtyGroup,[SpCnt] = count(*)
from #detailResult
group by MDivisionID, FtyGroup

--sheet3 RATING
--INHOUSE(RFID - SYSTEM BASED)
select[ATcnt] = sum(iif([AT] is not null, 1, 0)),
        [ATclose] = sum(iif([AT] >= Qty, 1, 0)),
        [ATfail] = sum(iif([AT] < Qty and[AT] is not null, 1, 0)),
		[BONDINGcnt] = sum(iif([BONDING] is not null,1,0 )),
		[BONDINGclose] = sum(iif([BONDING] >= Qty,1,0 )),
		[BONDINGfail] = sum(iif([BONDING] < Qty and [BONDING] is not null,1,0 )),
		[HTcnt] = sum(iif([HT] is not null,1,0 )),
		[HTclose] = sum(iif([HT] >= Qty,1,0 )),
		[HTfail] = sum(iif([HT] < Qty and [HT] is not null,1,0 )),
		[PAD-PRTcnt] = sum(iif([PAD-PRT] is not null,1,0 )),
		[PAD-PRTclose] = sum(iif([PAD-PRT] >= Qty,1,0 )),
		[PAD-PRTfail] = sum(iif([PAD-PRT] < Qty and [PAD-PRT] is not null,1,0 ))
from #detailResult 

--SUBCON OUT(RFID -SYSTEM BASED)
SELECT FtyGroup,
       [PrintingCnt] = count(*),
       [PrintingClose] = sum(iif([PRINTING] >= Qty, 1, 0)),
       [Printingfail] = sum(iif([PRINTING] < Qty, 1, 0))
from #detailResult
where[PRINTING] is not null 
group by FtyGroup

SELECT FtyGroup,
	   [EMBROCnt] = count(*),
	   [EMBROClose] = sum(iif([EMBRO] >= Qty,1,0 )),
	   [EMBROfail] = sum(iif([EMBRO] < Qty ,1,0 ))
from #detailResult
where[EMBRO] is not null 
group by FtyGroup

drop table #pOffline,#tmpP,#tmpc2,#tmpc3,#tmpc,#tmpc0
drop table #tmp2,#tmp3,#tmp4,#tmpin2,#tmpin,#tmpoutFin
drop table #tmp,#tmpout1,#tmpout2,#tmpout3,#tmpout4,#tmpout,#WorkDate,#orders_tmp
drop table #detailResult	   
";
            #endregion
            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            return DBProxy.Current.Select(string.Empty, this.tsql, sqlpar, out this.dts);
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(ReportDefinition report)
        {
            DataTable dtDetail = this.dts[0];
            DataTable dtSummary = this.dts[1];
            DataTable dtRATING1 = this.dts[2];
            DataTable dtRATING2 = this.dts[3];
            DataTable dtRATING3 = this.dts[4];
            if (dtDetail.Rows.Count == 0)
            {
                MyUtility.Msg.ErrorBox("Data not found");
                return false;
            }

            this.ShowWaitMessage("Excel Processing...");
            this.SetCount(dtDetail.Rows.Count); // 顯示筆數

            Excel.Application objApp = new Excel.Application();
            Sci.Utility.Report.ExcelCOM com = new Sci.Utility.Report.ExcelCOM(Sci.Env.Cfg.XltPathDir + "\\PPIC_R13.xltx", objApp);
            Excel.Worksheet worksheet;

            // sheet1 data
            com.WriteTable(dtDetail, 2);

            #region sheet2 data
            worksheet = objApp.Sheets[2];

            var distinctM = dtSummary.AsEnumerable().GroupBy(s => new { M = s["MDivisionID"] })
                .Select(s => new { s.Key.M, Total = s.Sum(i => (int)i["SpCnt"]) });

            // 如果M超過1個先複製外框
            if (distinctM.Count() > 1)
            {
                worksheet.get_Range("A3:B6").Copy();
                for (int i = 1; i < distinctM.Count(); i++)
                {
                    Excel.Range to = worksheet.get_Range($"A{(3 + (i * 5)).ToString()}:B{(6 + (i * 5)).ToString()}");
                    to.PasteSpecial(Excel.XlPasteType.xlPasteAll);
                }
            }

            int startRow = 3;
            foreach (var item in distinctM)
            {
                DataRow[] datalist = dtSummary.Select($"MDivisionID = '{item.M}'");

                worksheet.Cells[1][startRow] = item.M;
                worksheet.Cells[2][startRow + 3] = item.Total;
                // 如果超過1筆插入多的row
                for (int i = 0; i < datalist.Length; i++)
                {
                    worksheet.Cells[1][startRow + 2] = datalist[i]["FtyGroup"];
                    worksheet.Cells[2][startRow + 2] = datalist[i]["SpCnt"];

                    if (i < datalist.Length - 1)
                    {
                        worksheet.get_Range($"A{startRow + 3}:B{startRow + 3}").Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                        startRow++;
                    }
                }

                startRow += 5;
            }

            #endregion

            #region sheet3 data
            worksheet = objApp.Sheets[3];

            // INHOUSE (RFID -SYSTEM BASED)
            worksheet.Cells[2][5] = dtRATING1.Rows[0]["ATcnt"];
            worksheet.Cells[3][5] = dtRATING1.Rows[0]["ATclose"];
            worksheet.Cells[4][5] = dtRATING1.Rows[0]["ATfail"];
            worksheet.Cells[2][6] = dtRATING1.Rows[0]["BONDINGcnt"];
            worksheet.Cells[3][6] = dtRATING1.Rows[0]["BONDINGclose"];
            worksheet.Cells[4][6] = dtRATING1.Rows[0]["BONDINGfail"];
            worksheet.Cells[2][7] = dtRATING1.Rows[0]["HTcnt"];
            worksheet.Cells[3][7] = dtRATING1.Rows[0]["HTclose"];
            worksheet.Cells[4][7] = dtRATING1.Rows[0]["HTfail"];
            worksheet.Cells[2][8] = dtRATING1.Rows[0]["PAD-PRTcnt"];
            worksheet.Cells[3][8] = dtRATING1.Rows[0]["PAD-PRTclose"];
            worksheet.Cells[4][8] = dtRATING1.Rows[0]["PAD-PRTfail"];

            // SUBCON OUT  (RFID -SYSTEM BASED)
            int maxRow = dtRATING2.Rows.Count > dtRATING3.Rows.Count ? dtRATING2.Rows.Count : dtRATING3.Rows.Count;

            // 插入多出來的row
            if (maxRow > 1)
            {
                for (int i = 1; i < maxRow; i++)
                {
                    worksheet.get_Range($"A{14 + i}:K{14 + i}").Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown, worksheet.get_Range($"A{14 + i}:K{14 + i}").Copy(Type.Missing));
                }
            }

            com.ExcelApp.ActiveWorkbook.Sheets[3].Select(Type.Missing);
            com.WriteTable(dtRATING2, 15);
            com.WriteTable(dtRATING3, 15, 7);
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
