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
from Factory
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
into #tsp
from orders o
inner join Factory f on o.FactoryID = f.ID
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
inner join Orders o on p.orderID = o.ID
inner join Pattern pt on	pt.StyleUkey = o.StyleUkey
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
inner join (
	select	distinct #tsp.orderID
			, b.Article
			, FabricCombo = tpc.F_CODE
			, artwork = isnull (stuff ((select '+' + subprocessid
										from (
											select distinct bda.subprocessid
											from Bundle_Detail_Art bda
											where bd.BundleNo = bda.Bundleno
										) k
										for xml path('')
									  ), 1, 1, '')
							  , '')
			, PatternCode = tpc.PatternCode
	from #TablePatternCode tpc
	left join Bundle b on b.POID = tpc.Poid
	left join Bundle_Detail bd on b.id = bd.id
								  and tpc.PatternCode = bd.Patterncode
	left join #tsp on b.Orderid = #tsp.orderID
	left join WorkOrder wo on b.POID = wo.id
							   and b.CutRef = wo.CutRef
	left join Order_BOF ob on ob.Id = wo.Id 
							  and ob.FabricCode = wo.FabricCode
	left join Order_EachCons oec on oec.ID = ob.ID 
								    and oec.MarkerName = wo.Markername 
									and oec.FabricCombo = wo.FabricCombo 
									and oec.FabricPanelCode = wo.FabricPanelCode 
									and oec.FabricCode = wo.FabricCode

	where ob.Kind not in ('0','3')
	      and oec.CuttingPiece = 0
)#cur_artwork on #tsp.orderID = #cur_artwork.orderID

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
from BundleInOut bio
inner join Bundle_Detail bd on bio.BundleNo = bd.BundleNo
inner join Bundle b on b.id = bd.id
inner join WorkOrder wo on b.POID = wo.id
						   and b.CutRef = wo.CutRef
left join Order_BOF ob on ob.Id = wo.Id 
						  and ob.FabricCode = wo.FabricCode
inner join #tsp on b.Orderid = #tsp.orderID
outer apply (
	select value = stuff ((	select '+' + subprocessid
							from (
								select distinct bda.subprocessid
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
	
	;with qa as (
		select	OrderID = @OrderID
				, qdate = @StartInLine

		union all
		select	OrderID = @OrderID
				, DATEADD(day, 1, qdate)
		from qa 
		where	qdate < @EndDate
	) 
	insert into #CBDate
	select OrderID, qdate
	from qa
	Option (maxrecursion 365);

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
select	p.FactoryID,p.SP,p.StyleID,p.SewingDate,p.Line,p.AccuStd,acc.QtyAll
		, BCS = iif(BCS.value >= 100, 100, BCS.value)
from #print p
---0006111以上全部都不動，直接組新的Table計算欄位(acc.QtyAll)Accu. Loading Qty of Garment
---再依[SP#]= p.SP and FactoryID = p.FactoryID做outer apply
outer apply(
	select FactoryID,[SP#],[Article],QtyAll=sum(QtySM)
	from
	(
		select FactoryID,[SP#],[Article],SizeCode,[Comb],QtySM = min(QtySum)
		from(
			Select DISTINCT
				 o.FactoryID,
				[SP#] = b.Orderid,
				[Article] = b.Article,
				bd.SizeCode,
				[Comb] = b.PatternPanel,
				[Artwork] = sub.sub,
				[Pattern] = bd.PatternCode,
				--[Qty] = bd.Qty,
				[QtySum] = sum(bd.Qty) over(partition by o.FactoryID,b.Orderid,b.Article,bd.SizeCode,b.PatternPanel,sub.sub,bd.PatternCode)
			from Bundle b WITH (NOLOCK) 
			inner join Bundle_Detail bd WITH (NOLOCK) on bd.Id = b.Id
			left join Bundle_Detail_Art bda WITH (NOLOCK) on bda.Id = bd.Id and bda.Bundleno = bd.Bundleno
			inner join orders o WITH (NOLOCK) on o.Id = b.OrderId
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
			where 1=1
			and s.Id = 'LOADING'
			and b.Orderid = p.SP
			and bio.InComing is not null and bio.InComing !='' and bio.InComing< p.SewingDate
			and b.PatternPanel in(
				Select distinct oe.PatternPanel
				From dbo.Order_EachCons a WITH (NOLOCK) 
				Left Join dbo.Orders b WITH (NOLOCK) On a.ID = b.ID  
				left join dbo.Order_BOF bof WITH (NOLOCK) on bof.Id = a.Id and bof.FabricCode = a.FabricCode
				left join Order_EachCons_PatternPanel oe WITH (NOLOCK) on oe.Order_EachConsUkey = a.Ukey
				Where a.ID = o.POID and bof.kind !=0
			)
		)aa
		group by FactoryID,[SP#],[Article],SizeCode,[Comb]
	)bb
	where [SP#]= p.SP and FactoryID = p.FactoryID
	group by FactoryID,[SP#],[Article]
)acc 
outer apply (
	select value = ROUND(acc.QtyAll / iif(AccuStd = 0, 1, AccuStd) * 100, 2)
) BCS
where SewingDate between @StartDate and @EndDate
and p.AccuStd !=0
order by p.FactoryID,p.SP,p.SewingDate,p.Line

drop table #tsp
drop table #cutcomb
drop table #cur_bdltrack2
drop table #Min_cut
drop table #print
drop table #TablePatternUkey;
drop table #TablePatternCode
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
