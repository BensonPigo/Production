﻿using Ict;
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
*	依照 Poid，取得製作一件衣服所有需要的 【FabricCode & PatternCode】
*/
select	Poid = o.POID
		, pt.UKey
into #TablePatternUkey
from #tsp p
inner join Orders o WITH (NOLOCK) on p.orderID = o.ID
inner join Pattern pt WITH (NOLOCK) on	pt.StyleUkey = o.StyleUkey
							and pt.Status = 'Completed' 
							and pt.EDITdATE = (	SELECT MAX(EditDate) 
												from pattern WITH (NOLOCK) 
												where styleukey = o.StyleUkey
														and Status = 'Completed') 

/*														
*	取得所有 PatternPanel
*	※ C# 中 SubStrin (10) 代表第 11 個字元，SQL 中 SubString (11) 代表第 11 個字元
*
*	FabricPanelCode 只需要取 FabricCode 不為空白
*	Distinct 原因是會有多筆 Article Group
*	PatternCode 只要 Annotation = 空值，則 PatterCode = 'ALLPARTS'	
*/
Select	distinct tpu.Poid
		, PatternCode = case
							when pg.Annotation = '' or pg.Annotation is null then 'ALLPARTS'
							else PatternCode.value
						end
		, F_CODE = a.FabricPanelCode
into #TablePatternCode
from #TablePatternUkey tpu
inner join Pattern_GL pg WITH (NOLOCK) on tpu.UKey = pg.PatternUKEY
inner join Pattern_GL_LectraCode a WITH (NOLOCK) on	 a.PatternUkey = tpu.Ukey
														and a.FabricCode != ''
														and pg.SEQ = a.SEQ
outer apply (
	select value = iif(a.SEQ = '0001', SubString(a.PatternCode, 11, Len(a.PatternCode)), a.PatternCode)
) PatternCode


/*
*	取出的訂單，執行以下判斷流程 (#CutComb 儲存 整件衣服 應該裁剪的部位【須展開至 PatterCode】)
*	Step1 找出同 Article、Comb 的 bundel card 有幾個不同的 artwork bundle
*	Step2 組出 SP# 的剪裁 Comb
*		排除
*			1. Order_BOF.Kind = 0 (表Other) 
*			2. Order_BOF.Kind = 3 (表Polyfill)的資料
*			3. 外裁項 (Order_EachCons.CuttingPiece == 0) 【0 代表非外裁項】
*/
select	distinct #tsp.orderID
		, b.Article
		, FabricCombo = tpc.F_CODE
		, artwork = isnull (stuff ((select '+' + subprocessid
									from (
										select distinct bda.subprocessid
										from Bundle_Detail_Art bda WITH (NOLOCK)
										where bd.BundleNo = bda.Bundleno
									) k
									for xml path('')
								  ), 1, 1, '')
						  , '')
		, PatternCode = tpc.PatternCode
into #tmp_TablePatternCode
from #TablePatternCode tpc
left join Bundle b WITH (NOLOCK) on b.POID = tpc.Poid
left join Bundle_Detail bd WITH (NOLOCK) on b.id = bd.id
							  and tpc.PatternCode = bd.Patterncode
left join #tsp on b.Orderid = #tsp.orderID and #tsp.MDivisionID  = b.MDivisionID 
left join WorkOrder wo WITH (NOLOCK) on b.POID = wo.id
						   and b.CutRef = wo.CutRef
left join Order_BOF ob WITH (NOLOCK) on ob.Id = wo.Id 
						  and ob.FabricCode = wo.FabricCode
left join Order_EachCons oec WITH (NOLOCK) on oec.ID = ob.ID 
							    and oec.MarkerName = wo.Markername 
								and oec.FabricCombo = wo.FabricCombo 
								and oec.FabricPanelCode = wo.FabricPanelCode 
								and oec.FabricCode = wo.FabricCode

where ob.Kind not in ('0','3')
      and oec.CuttingPiece = 0

select	#tsp.masterID
		, #tsp.orderID
		, #tsp.StyleID
		, #cur_artwork.FabricCombo
		, #cur_artwork.PatternCode
		, #cur_artwork.Article
		, #cur_artwork.artwork 
		, #tsp.SewLine
		, #tsp.FactoryID
into #cutcomb 
from #tsp
inner join #tmp_TablePatternCode as #cur_artwork on #tsp.orderID = #cur_artwork.orderID

/*
*	Step3 取出被 Loader 收下的 Bundle Data
*		條件 SubProcessId = 'Loading'
*		排除
*			1. Order_BOF.Kind = 0 (表Other) 
*			2. Order_BOF.Kind = 3 (表Polyfill)的資料
*			3. 外裁項 【Bundle_Detail 不會出現外裁項的資料】
*		BundleGroup 必須是同 Group 組合才能算一件衣服，因為不同 Group 可能會有色差
*/
select	b.orderid
		, bd.SizeCode
		, cdate2 = cDate.value
		, b.Article
		, bd.BundleGroup
		, FabricCombo = wo.FabricPanelCode
		, PatternCode = bd.Patterncode
		, artwork = isnull(ArtWork.value, '')
		, qty = sum(isnull(bd.qty, 0))
into #cur_bdltrack2
from BundleInOut bio WITH (NOLOCK)
inner join Bundle_Detail bd WITH (NOLOCK) on bio.BundleNo = bd.BundleNo
inner join Bundle b WITH (NOLOCK) on b.id = bd.id
inner join WorkOrder wo WITH (NOLOCK) on b.POID = wo.id
						   and b.CutRef = wo.CutRef
left join Order_BOF ob WITH (NOLOCK) on ob.Id = wo.Id 
						  and ob.FabricCode = wo.FabricCode
inner join #tsp on b.Orderid = #tsp.orderID and #tsp.MDivisionID  = b.MDivisionID 
outer apply (
	select value = stuff ((	select '+' + subprocessid
							from (
								select distinct bda.subprocessid
								from Bundle_Detail_Art bda WITH (NOLOCK)
								where bd.BundleNo = bda.Bundleno
							) k
							for xml path('')
							), 1, 1, '')
) ArtWork
outer apply(
	select value = CONVERT(char(10), bio.InComing, 120)
) cDate
where	bio.SubProcessId = 'loading'
        and isnull(bio.RFIDProcessLocationID,'') = ''
		and ob.Kind not in ('0','3')		
		--{2} 篩選 OrderID
		{2}
group by b.orderid, bd.SizeCode, cDate.value, b.Article, wo.FabricPanelCode, ArtWork.value, bd.Patterncode, bd.BundleGroup;

/*
*	根據每一個 OrderID 取得 SewInDate  日期展開至 EndDate
*	這邊會影響到最後計算成衣件數
*/
create table #CBDate (
	OrderID varchar(13)
	, SewDate date
);

Declare rs Cursor For Select orderID, SewInLine from #tsp
Declare @OrderID varchar(20);
Declare @StartInLine date;
Declare @SewInLine date;
Declare @count int = 0;

/*
*	指向 #tsp 第一筆資料
*	@@FETCH_STATUS = 0，代表資料指向成功
*/
Open rs
Fetch Next From rs Into @OrderID, @SewInLine
While @@FETCH_STATUS = 0
Begin	
	select top 1 @StartInLine = iif(cdate2 is null or cdate2 = '', @SewInLine, cdate2)
	from #cur_bdltrack2 
	where #cur_bdltrack2.Orderid = @OrderID
	order by cdate2
	
	while @StartInLine <= @EndDate
	begin
		insert into #CBDate
		select @OrderID, @StartInLine

		set @StartInLine =  DATEADD(day, 1, @StartInLine) 
	end 

/*
*	Fetch 指向 #tsp 下一筆資料
*/
	Fetch Next From rs Into @OrderID, @SewInLine
End;
Close rs;
Deallocate rs; 

/*	
*	Step4 已收的bundle資料中找出各 
*			article 
*			size
*			部位
*			artwork
*			加總數量
*	Step5 計算 sp# 上線日至下線日的產出
*	Step6 依條件日期區間，至 sewing 取得 stdqty 加總以及抓取備妥的成衣件數
*/
select	NewCutComb.FactoryID
		, NewCutComb.orderid
		, NewCutComb.StyleID
		, NewCutComb.cdate2
		, NewCutComb.SewLine
		, NewCutComb.Article
		, NewCutComb.SizeCode
		, NewCutComb.BundleGroup
		, NewCutComb.FabricCombo
		, NewCutComb.PatternCode
		, NewCutComb.artwork
		, qty = isnull(#cur_bdltrack2.qty, 0)
		, AccuLoadingQty = sum(isnull(#cur_bdltrack2.qty, 0)) over (partition by NewCutComb.FactoryID, NewCutComb.orderid,  NewCutComb.Article, NewCutComb.SizeCode, NewCutComb.BundleGroup, NewCutComb.FabricCombo, NewCutComb.PatternCode, NewCutComb.artwork
																    order by NewCutComb.cdate2)
into #Min_cut
from (
/*
*	組出每一天，同 OrderID, Artwork, Article, SizeCode 需要的 FabricCombe & PatternCode
*	這邊會在 CutComb 成衣組合中，加入需計算的日期	
*/
	select	distinct #cutcomb.*
			, cdate2 = #CBDate.SewDate
			, SizeCode = SizeCode.value
			, BundleGroup = BundleGroup.value
	from #cutcomb
	outer apply (
		select top 1 value = SizeCode
		from #cur_bdltrack2 
		where	#cutcomb.orderID = #cur_bdltrack2.Orderid
	) SizeCode
	outer apply (
		select top 1 value = BundleGroup
		from #cur_bdltrack2 
		where	#cutcomb.orderID = #cur_bdltrack2.Orderid
	) BundleGroup
	inner join #CBDate on #cutcomb.orderID = #CBDate.OrderID
) NewCutComb 
left join #cur_bdltrack2 on	NewCutComb.artwork = #cur_bdltrack2.artwork 
							and NewCutComb.FabricCombo = #cur_bdltrack2.FabricCombo
							and NewCutComb.PatternCode = #cur_bdltrack2.PatternCode
							and NewCutComb.orderID = #cur_bdltrack2.orderid
							and NewCutComb.Article = #cur_bdltrack2.Article 
							and NewCutComb.SizeCode = #cur_bdltrack2.SizeCode
                            and NewCutComb.Cdate2 = #cur_bdltrack2.Cdate2	
							and NewCutComb.BundleGroup = #cur_bdltrack2.BundleGroup;						
--order by cdate2, orderid, Article, SizeCode, FabricCombo, PatternCode, artwork

/*
*	準備好要印的資料
*/
select	FactoryID
		, SP
		, StyleID
		, SewingDate = DateAdd(day, 1 ,SewingDate)
		, Line
		, AccuStd
		, AccuLoad = AccuLoading
into #print
from (
	select	FactoryID
			, SP = orderID
			, StyleID
			, SewingDate = cdate2
			, Line = SewLine
			, AccuStd = isnull(std.value, 0)
			, AccuLoading = sum (isnull(MinLoadingQty, 0))
	from (
/*
*		依照【日期, SP#, Article, Size, Comb, Artwork】取最小數量 (因為每個部位都要有，才能成為一件衣服)
*		判斷 BundleGroup
*			第一次群組判斷最小數量需要加上 BundleGroup
*			第二次群組加總所有成衣數量		
*/
		select FactoryID
				, orderID
				, StyleID
				, cdate2
				, SewLine
				, Article, SizeCode
				, MinLoadingQty = sum (MinLoadingQty)
		from (
			select	FactoryID
					, orderID
					, StyleID
					, cdate2
					, SewLine
					, Article, SizeCode
					, MinLoadingQty = min(AccuLoadingQty)
			from #Min_cut 
			group by FactoryID, orderID	,StyleID, cdate2, SewLine, Article, SizeCode, BundleGroup
		)w
		group by FactoryID, orderID	,StyleID, cdate2, SewLine, Article, SizeCode
	)x 
	outer apply(
		select	value = isnull(( iif((select sum(s.StdQ)
									from dbo.getDailystdq(x.orderid) s
									where s.Date between @StartDate and @EndDate
									)=0,0,(select sum(s.StdQ)
									from dbo.getDailystdq(x.orderid) s
									where s.Date <= @EndDate
									)))
								, 0)
	) std
	group by FactoryID, orderID, StyleID, cdate2, SewLine, std.value
) a

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
	order by FactoryID, SewingDate
Else  
with step1 as ( --InComing有日期同PatternCode加總，算出每個Pattern總數
		Select 
				o.FactoryID,
				b.Orderid,
				b.Article,
				bd.SizeCode,
				b.PatternPanel,
				bd.PatternCode,
				QtySum =SUM(iif(isnull(bio.InComing,'') = '',0,bd.Qty))
			from Bundle b WITH (NOLOCK) 
			inner join Bundle_Detail bd WITH (NOLOCK) on bd.Id = b.Id
			left join Bundle_Detail_Art bda WITH (NOLOCK) on bda.Id = bd.Id and bda.Bundleno = bd.Bundleno
			inner join orders o WITH (NOLOCK) on o.Id = b.OrderId and o.MDivisionID  = b.MDivisionID 
			inner join SubProcess s WITH (NOLOCK) on (s.IsRFIDDefault = 1 or s.Id = bda.SubprocessId) 
			left join BundleInOut bio WITH (NOLOCK) on bio.Bundleno=bd.Bundleno and bio.SubProcessId = s.Id
			outer apply(
					select sub= stuff((
						Select distinct concat('+', bda.SubprocessId)
						from Bundle_Detail_Art bda WITH (NOLOCK) 
						where bda.Id = bd.Id and bda.Bundleno = bd.Bundleno
						for xml path('')
					),1,1,'')
			) as sub
			where s.Id = 'LOADING' and b.Orderid in (select distinct sp from #print)
            and isnull(bio.RFIDProcessLocationID,'') = ''
			and b.PatternPanel in (
				Select distinct oe.PatternPanel
				From dbo.Order_EachCons a WITH (NOLOCK) 
				Left Join dbo.Orders b WITH (NOLOCK) On a.ID = b.ID  
				left join dbo.Order_BOF bof WITH (NOLOCK) on bof.Id = a.Id and bof.FabricCode = a.FabricCode
				left join Order_EachCons_PatternPanel oe WITH (NOLOCK) on oe.Order_EachConsUkey = a.Ukey
				Where a.ID = o.POID and bof.Kind not in ('0','3') )
			  group by o.FactoryID,
				 b.Orderid,
				 b.Article,
				bd.SizeCode,
				 b.PatternPanel,
                bd.PatternCode)
,	step2 as( --同一個Pattern Combo下抓出量最小的pattern的數量，作沒該Pattern Combo的數量
		select	FactoryID,
				Orderid,
				Article,
				SizeCode,
				PatternPanel,
				QtySum = MIN(QtySum)
		from	step1
		group by	FactoryID,
					Orderid,
					Article,
					SizeCode,
					PatternPanel
	)
,	step3 as(--抓出sp下Pattern Combo中數量最小的值，代表成衣數量
		select	FactoryID,
				Orderid,
				Article,
				SizeCode,
				QtySum = min(QtySum)
		from	step2
		group by	FactoryID,
					Orderid,
					Article,
					SizeCode
				)
select	p.FactoryID,p.SP,p.StyleID,p.SewingDate,p.Line,p.AccuStd,acc.QtyAll
		, BCS = iif(BCS.value >= 100, 100, BCS.value)
	from #print p
	outer apply(select FactoryID,[SP#] = Orderid,QtyAll=sum(QtySum) 
				from step3 where Orderid= p.SP and FactoryID = p.FactoryID
				group by FactoryID,Orderid
				) acc 
    outer apply (
    	select value = ROUND(acc.QtyAll / iif(AccuStd = 0, 1, AccuStd) * 100, 2)
    ) BCS
where SewingDate between @StartDate and @EndDate and acc.QtyAll > 0
and p.AccuStd !=0
order by p.FactoryID,p.SP,p.SewingDate,p.Line

drop table #tsp
drop table #cutcomb
drop table #cur_bdltrack2
drop table #Min_cut
drop table #print
drop table #TablePatternUkey;
drop table #TablePatternCode,#tmp_TablePatternCode;
drop table #CBDate
"
                , (SP.Empty()) ? "" : "and o.id = @SP"
                , (Factory.Empty()) ? "" : "and f.ID = @FactoryID"
                , (SP.Empty()) ? "" : "and b.OrderID = @SP");
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
