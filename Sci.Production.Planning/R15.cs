using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;
using System.Runtime.InteropServices;

namespace Sci.Production.Planning
{
    public partial class R15 : Sci.Win.Tems.PrintForm
    {
        int selectindex = 0;
        string factory, mdivision, orderby, spno1, spno2, custcd, brandid;
        DateTime? sciDelivery1, sciDelivery2, CustRqsDate1, CustRqsDate2, BuyerDelivery1, BuyerDelivery2
            , CutOffDate1, CutOffDate2, planDate1, planDate2;
        DataTable printData, dtArtworkType;
        StringBuilder artworktypes = new StringBuilder();
        bool isArtwork;
        public R15(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            txtMdivision.Text = Sci.Env.User.Keyword;
            txtfactory.Text = Sci.Env.User.Factory;
            comboCategory.SelectedIndex = 1;  //Bulk
            MyUtility.Tool.SetupCombox(comboOrderBy, 2, 1, "orderid,SPNO,brandid,Brand");
            comboOrderBy.SelectedIndex = 0;
            dateBuyerDelivery.Select();
            dateBuyerDelivery.Value1 = DateTime.Now;
            dateBuyerDelivery.Value2 = DateTime.Now.AddDays(30);
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(dateSCIDelivery.Value1) &&
                MyUtility.Check.Empty(dateCustRQSDate.Value1) &&
                MyUtility.Check.Empty(dateBuyerDelivery.Value1) &&
                MyUtility.Check.Empty(dateCutOffDate.Value1) &&
                MyUtility.Check.Empty(datePlanDate.Value1) &&
                (MyUtility.Check.Empty(txtSPNoStart.Text) || MyUtility.Check.Empty(txtSPNoEnd.Text)))
            {
                MyUtility.Msg.WarningBox("< Buyer Delivery > & < SCI Delivery > & < Cust RQS Date > & < Cut Off Date > & < Plan Date > & < SP# > can't be empty!!");
                return false;
            }

            #region -- 擇一必輸的條件 --
            sciDelivery1 = dateSCIDelivery.Value1;
            sciDelivery2 = dateSCIDelivery.Value2;
            CustRqsDate1 = dateCustRQSDate.Value1;
            CustRqsDate2 = dateCustRQSDate.Value2;
            BuyerDelivery1 = dateBuyerDelivery.Value1;
            BuyerDelivery2 = dateBuyerDelivery.Value2;
            CutOffDate1 = dateCutOffDate.Value1;
            CutOffDate2 = dateCutOffDate.Value2;
            planDate1 = datePlanDate.Value1;
            planDate2 = datePlanDate.Value2;
            spno1 = txtSPNoStart.Text;
            spno2 = txtSPNoEnd.Text;
            #endregion
            brandid = txtbrand.Text;
            custcd = txtCustCD.Text;
            mdivision = txtMdivision.Text;
            factory = txtfactory.Text;
            selectindex = comboCategory.SelectedIndex;
            orderby = comboOrderBy.SelectedValue.ToString();
            isArtwork = checkIncludeArtowkData.Checked;
            if (isArtwork)
            {
                DualResult result;
                if (!(result = DBProxy.Current.Select("", "select id from dbo.artworktype WITH (NOLOCK) where istms=1 or isprice= 1 order by seq", out dtArtworkType)))
                {
                    MyUtility.Msg.WarningBox(result.ToString());
                    return false;
                }

                if (dtArtworkType.Rows.Count == 0)
                {
                    MyUtility.Msg.WarningBox("Artwork Type data not found, Please inform MIS to check !");
                    return false;
                }

                artworktypes.Clear();
                for (int i = 0; i < dtArtworkType.Rows.Count; i++)
                {
                    artworktypes.Append(string.Format(@"[{0}],", dtArtworkType.Rows[i]["id"].ToString()));
                }
            }

            return base.ValidateInput();
        }

        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
            StringBuilder sqlCmd = new StringBuilder();

            sqlCmd.Append(string.Format(@"
                select o.MDivisionID,o.FactoryID,S.BuyerDelivery,o.SciDelivery,O.CRDDate,O.CFMDate
                ,O.ID OrderID,O.Dest,O.StyleID,O.SeasonID,O.ProjectID,O.Customize1,O.BuyMonth
                ,O.CustPONo,O.BrandID,O.CustCDID,O.ProgramID,O.CdCodeID,O.CPU,O.Qty,S.Qty [Qty_byShip]
                ,O.FOCQty,O.PoPrice,O.CMPPrice,O.KPILETA,O.LETA,O.MTLETA,O.SewETA
                ,O.PackETA,O.MTLComplete,O.SewInLine,O.SewOffLine,O.CutInLine,O.CutOffLine
                ,O.Category,O.IsForecast,O.PulloutDate,O.ActPulloutDate,O.SMR,O.MRHandle
                ,O.MCHandle,O.OrigBuyerDelivery,O.DoxType
                ,O.TotalCTN,O.FtyCTN,O.ClogCTN
                ,O.VasShas,O.TissuePaper,O.MTLExport,O.SewLine,O.ShipModeList,s.ShipmodeID,O.PlanDate
                ,O.FirstProduction,O.Finished,O.FtyGroup,O.OrderTypeID,O.SpecialMark,O.GFR
                ,O.SampleReason,O.InspDate,O.MnorderApv,O.FtyKPI,O.KPIChangeReason
                ,O.StyleUkey
                ,O.POID
				,InspResult = IIF(o.InspResult='P','Pass',IIF(o.InspResult='F','Fail',''))
				,InspHandle = (o.InspHandle +'-'+ I.Name)
                into #cte 
                from dbo.Orders o WITH (NOLOCK) inner join Order_QtyShip s WITH (NOLOCK) on s.id = o.ID
				OUTER APPLY(
					SELECT  Name 
					FROM Pass1 WITH (NOLOCK) 
					WHERE Pass1.ID = O.InspHandle
				)I
                WHERE 1=1 
                "));

            #region --- 條件組合  ---
            if (!MyUtility.Check.Empty(sciDelivery1))
            {
                sqlCmd.Append(string.Format(@" and o.SciDelivery >= '{0}'", Convert.ToDateTime(sciDelivery1).ToString("d")));
            }
            if (!MyUtility.Check.Empty(sciDelivery2))
            {
                sqlCmd.Append(string.Format(@" and o.SciDelivery <= '{0}'", Convert.ToDateTime(sciDelivery2).ToString("d")));
            }
            if (!MyUtility.Check.Empty(BuyerDelivery1))
            {
                sqlCmd.Append(string.Format(@" and s.BuyerDelivery >= '{0}'", Convert.ToDateTime(BuyerDelivery1).ToString("d")));
            }
            if (!MyUtility.Check.Empty(BuyerDelivery2))
            {
                sqlCmd.Append(string.Format(@" and s.BuyerDelivery <= '{0}'", Convert.ToDateTime(BuyerDelivery2).ToString("d")));
            }
            if (!MyUtility.Check.Empty(CustRqsDate1))
            {
                sqlCmd.Append(string.Format(@" and o.CRDDate >= '{0}'", Convert.ToDateTime(CustRqsDate1).ToString("d")));
            }
            if (!MyUtility.Check.Empty(CustRqsDate2))
            {
                sqlCmd.Append(string.Format(@" and o.CRDDate <= '{0}'", Convert.ToDateTime(CustRqsDate2).ToString("d")));
            }
            if (!MyUtility.Check.Empty(CutOffDate1))
            {
                sqlCmd.Append(string.Format(@" and o.SDPDate >= '{0}'", Convert.ToDateTime(CutOffDate1).ToString("d")));
            }
            if (!MyUtility.Check.Empty(CutOffDate2))
            {
                sqlCmd.Append(string.Format(@" and o.SDPDate <= '{0}'", Convert.ToDateTime(CutOffDate2).ToString("d")));
            }
            if (!MyUtility.Check.Empty(planDate1))
            {
                sqlCmd.Append(string.Format(@" and o.PlanDate >= '{0}'", Convert.ToDateTime(planDate1).ToString("d")));
            }
            if (!MyUtility.Check.Empty(planDate2))
            {
                sqlCmd.Append(string.Format(@" and o.PlanDate <= '{0}'", Convert.ToDateTime(planDate2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(spno1))
            {
                sqlCmd.Append(" and o.id >= @spno1 ");
                cmds.Add(new System.Data.SqlClient.SqlParameter("@spno1", spno1));
            }
            if (!MyUtility.Check.Empty(spno2))
            {
                sqlCmd.Append(" and o.id <= @spno2 ");
                cmds.Add(new System.Data.SqlClient.SqlParameter("@spno2", spno2));
            }

            if (!MyUtility.Check.Empty(brandid))
            {
                sqlCmd.Append(string.Format(@" and o.brandid = @brandid"));
                cmds.Add(new System.Data.SqlClient.SqlParameter("@brandid", brandid));
            }

            if (!MyUtility.Check.Empty(custcd))
            {
                sqlCmd.Append(string.Format(@" and o.CustCDID = @custcd"));
                cmds.Add(new System.Data.SqlClient.SqlParameter("@custcd", custcd));
            }

            if (!MyUtility.Check.Empty(mdivision))
            {
                sqlCmd.Append(" and o.mdivisionid = @MDivision");
                cmds.Add(new System.Data.SqlClient.SqlParameter("@MDivision", mdivision));
            }

            if (!MyUtility.Check.Empty(factory))
            {
                sqlCmd.Append(" and o.factoryid = @factory");
                cmds.Add(new System.Data.SqlClient.SqlParameter("@factory", factory));
            }

            switch (selectindex)
            {
                case 0:
                    sqlCmd.Append(@" and (o.Category = 'B' or o.Category = 'S')");
                    break;
                case 1:
                    sqlCmd.Append(@" and o.Category = 'B' ");
                    break;
                case 2:
                    sqlCmd.Append(@" and (o.Category = 'S')");
                    break;
            }

            #endregion

            #region -- 有列印Artwork --
            if (isArtwork)
            {
                sqlCmd.Append(@"
                --依取得的訂單資料取得訂單的 TMS Cost
                select aa.orderid,bb.ArtworkTypeID,iif(cc.IsTMS=1,bb.tms,bb.price) price_tms 
                into #rawdata_tmscost
                from #cte aa 
                inner join dbo.Order_TmsCost bb on bb.id = aa.orderid
                inner join dbo.ArtworkType cc on cc.id = bb.ArtworkTypeID
                where IsTMS =1 or IsPrice = 1
                ");

                sqlCmd.Append(string.Format(@"--將取得Tms Cost做成樞紐表
                select * 
                into #tmscost_pvt
                from #rawdata_tmscost
                pivot
                (
                    sum(price_tms)
                    for artworktypeid in ( {0})
                )as pvt ", artworktypes.ToString().Substring(0, artworktypes.ToString().Length - 1)));
            }
            #endregion

            #region -- 產生RFID Loading Qty --
            sqlCmd.Append(@"/*Cur_bdltrack2*/

declare @EndDate date = CONVERT(date, GETDATE());

/*
Step1
取得基礎訂單
1.排除掉SewinLine and SewinOffLine 空值的訂單
*/
select masterID = o.POID
		, o.orderID 
		, StyleID
		, o.Finished
		, o.FactoryID
		, o.SewLine
		, o.SewInLine
		, o.SewOffLine
into  #tsp
from #cte o
inner join Factory f on o.FactoryID = f.ID
where	1=1--o.Junk != 1
and (o.SewInLine is not null or o.SewInLine != '')
		and (o.SewOffLine is not null or o.SewOffLine != '')	

----------------------------------------------------------------------------------------------------------
/*
	依照 Poid，取得製作一件衣服所有需要的 【FabricCode & PatternCode】
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
------------------------------------------------------------------------------------------------------------------
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

------------------------------------------------------------------------------------------------------------------
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
Step2
 取出被 Loader 收下的 Bundle Data
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
		-- 篩選 OrderID		
group by b.orderid, bd.SizeCode, cDate.value, b.Article, wo.FabricPanelCode, ArtWork.value, bd.Patterncode, bd.BundleGroup;

------------------------------------------------------------------------------------------------------------------

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
-----------------------------------------------------

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
		select distinct value = SizeCode
		from #cur_bdltrack2 
		where	#cutcomb.orderID = #cur_bdltrack2.Orderid
	) SizeCode
	outer apply (
		select distinct value = BundleGroup
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

-----------------------------------------------------------

/*
*	準備好要印的資料
*/
select	FactoryID
		, SP
		, StyleID
		, SewingDate = DateAdd(day, 1 ,SewingDate)
		, Line
		, AccuLoad = AccuLoading
into #print
from (
	select	FactoryID
			, SP = orderID
			, StyleID
			, SewingDate = cdate2
			, Line = SewLine
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
	group by FactoryID, orderID, StyleID, cdate2, SewLine
) a
");
            #endregion

            sqlCmd.Append(string.Format(@"
                -- 依撈出來的order資料(cte)去找各製程的WIP
                select 
                 t.OrderID
                 ,(SELECT SUM(CWIP.Qty) FROM DBO.CuttingOutput_WIP CWIP WITH (NOLOCK) WHERE CWIP.OrderID = T.OrderID) cut_qty
                 ,(SELECT MIN(a.cDate) from dbo.CuttingOutput a WITH (NOLOCK) 
	                 inner join dbo.CuttingOutput_Detail b WITH (NOLOCK) on b.id = a.id 
	                 inner join dbo.WorkOrder_Distribute c WITH (NOLOCK) on c.WorkOrderUkey = b.WorkOrderUkey
	                 where c.OrderID = t.OrderID) first_cut_date
                ,(select MIN(isnull(tt.qaqty,0)) from dbo.style_location sl WITH (NOLOCK) left join 
	                (SELECT b.ComboType,sum(b.QAQty) qaqty FROM DBO.SewingOutput a WITH (NOLOCK) 
	                inner join dbo.SewingOutput_Detail b WITH (NOLOCK) on b.ID = a.ID
	                where b.OrderId = t.OrderID
	                group by ComboType ) tt on tt.ComboType = sl.Location
	                where sl.StyleUkey = t.StyleUkey) sewing_output

                ,t.StyleUkey
                ,(select min(qty) qty from (
                select sum(b.Qty) qty ,c.PatternCode,c.ArtworkID 
                from dbo.farmin a WITH (NOLOCK) inner join dbo.FarmIn_Detail b WITH (NOLOCK) on b.id = a.id 
                right join (select distinct v.ArtworkTypeID, v.Article,v.ArtworkID,v.PatternCode 
                from dbo.View_Order_Artworks v 
                where v.ID=t.OrderID) c on c.ArtworkTypeID = a.ArtworkTypeId 
                and c.PatternCode = b.PatternCode 
                and c.ArtworkID = b.ArtworkID
                where a.ArtworkTypeId='EMBROIDERY' 
	                and b.Orderid = t.OrderID
	                group by c.PatternCode,c.ArtworkID) x) EMBROIDERY_qty

                ,(select min(qty) qty from (
                select sum(b.Qty) qty ,c.PatternCode,c.ArtworkID 
                from dbo.farmin a WITH (NOLOCK) inner join dbo.FarmIn_Detail b WITH (NOLOCK) on b.id = a.id 
                right join (select distinct v.ArtworkTypeID,v.ArtworkID,v.PatternCode 
                from dbo.View_Order_Artworks v where v.ID=t.OrderID) c 
                on c.ArtworkTypeID = a.ArtworkTypeId 
                and c.PatternCode = b.PatternCode 
                and c.ArtworkID = b.ArtworkID
                where a.ArtworkTypeId='BONDING' 
	                and b.Orderid = t.OrderID
	                group by c.PatternCode,c.ArtworkID) x) BONDING_qty

                ,(select min(qty) qty from (
                select sum(b.Qty) qty ,c.PatternCode,c.ArtworkID 
                from dbo.farmin a WITH (NOLOCK) inner join dbo.FarmIn_Detail b WITH (NOLOCK) on b.id = a.id 
                right join (select distinct v.ArtworkTypeID,v.ArtworkID,v.PatternCode 
                from dbo.View_Order_Artworks v 
                where v.ID=t.OrderID) c on c.ArtworkTypeID = a.ArtworkTypeId 
                and c.PatternCode = b.PatternCode 
                and c.ArtworkID = b.ArtworkID
                where a.ArtworkTypeId='PRINTING' 
	                and b.Orderid = t.OrderID
	                group by c.PatternCode,c.ArtworkID) x) PRINTING_qty
                ,SEWOUTPUT.*
                into #cte2
                from #cte t
                outer apply (SELECT min(X.OutputDate) firstSewingDate, max(X.OutputDate) lastestSewingDate
                ,sum(X.QAQty) QAQTY ,AVG(X.QAQTY) AVG_QAQTY
                from (SELECT a.OutputDate,sum(a.QAQty) QAQty FROM DBO.SewingOutput a WITH (NOLOCK) 
                inner join dbo.SewingOutput_Detail b WITH (NOLOCK) on b.ID = a.ID
                where b.OrderId = t.OrderID group by a.OutputDate ) X) SEWOUTPUT

                select t.MDivisionID,t.FactoryID,t.SewLine,t.BuyerDelivery,t.SewInLine,t.SewOffLine
                ,t.BrandID,t.OrderID
                ,Dest = Country.Alias
				,t.StyleID,t.OrderTypeID
                ,t.ShipmodeID
				,[OrderNo]=t.Customize1
				,t.CustPONo,t.CustCDID,t.ProgramID,t.CdCodeID,t.KPILETA
                ,t.LETA,t.MTLETA,t.SewETA,t.PackETA,t.CPU
                ,t.Qty_byShip,#cte2.first_cut_date
				,#cte2.cut_qty
                ,[RFID Cut Qty]= CutQty.[RFID Cut Qty]
			    ,[RFID Loading Qty]= isnull(loading.AccuLoad,0)				
				,[RFID Emb Farm In Qty] =Embin.[RFID Emb Farm In Qty]
				,[RFID Emb Farm Out Qty] = Embout.[RFID Emb Farm Out Qty]
				,[RFID Bond Farm In Qty] =Bondin.[RFID Bond Farm In Qty]
				,[RFID Bond Farm Out Qty] = Bondout.[RFID Bond Farm Out Qty]
				,[RFID Print Farm In Qty] =Printin.[RFID Print Farm In Qty]
				,[RFID Print Farm Out Qty] = Printout.[RFID Print Farm Out Qty]
				,[RFID AT Farm In Qty] =ATin.[RFID AT Farm In Qty]
				,[RFID AT Farm Out Qty] = ATout.[RFID AT Farm Out Qty]
				,[RFID Pad Print Farm In Qty] =PadPrintin.[RFID Pad Print Farm In Qty]
				,[RFID Pad Print Farm Out Qty] = PadPrintout.[RFID Pad Print Farm Out Qty]
				,[RFID Emboss Farm In Qty] =Embossin.[RFID Emboss Farm In Qty]
				,[RFID Emboss Farm Out Qty] = Embossout.[RFID Emboss Farm Out Qty]
				,[RFID HT Farm In Qty] =htin.[RFID HT Farm In Qty]
				,[RFID HT Farm Out Qty] = htout.[RFID HT Farm Out Qty]
                ,#cte2.EMBROIDERY_qty,#cte2.BONDING_qty
                ,#cte2.PRINTING_qty,#cte2.sewing_output,t.qty+t.FOCQty - #cte2.sewing_output [Balance]
                ,#cte2.firstSewingDate				
				,[Last Sewn Date] = (select Max(so.OutputDate) 
                         from SewingOutput so WITH (NOLOCK) 
                         inner join SewingOutput_Detail sod WITH (NOLOCK) on so.ID = sod.ID
                         where sod.OrderID = t.OrderID)
				,#cte2.AVG_QAQTY
                ,DATEADD(DAY,iif(isnull(#cte2.AVG_QAQTY,0) = 0,0,ceiling((t.qty+t.FOCQty - #cte2.sewing_output)/(#cte2.AVG_QAQTY*1.0))),#cte2.firstSewingDate) [Est_offline]
                ,IIF(isnull(t.TotalCTN,0)=0, 0, round(t.ClogCTN / (t.TotalCTN*1.0),4) * 100 ) [pack_rate]
                ,t.TotalCTN                
                ,t.TotalCTN-t.FtyCTN as FtyCtn
                , t.ClogCTN, t.InspDate
				, InspResult
				, [CFA Name] = InspHandle
				, t.ActPulloutDate,t.FtyKPI                
                ,KPIChangeReason.KPIChangeReason  KPIChangeReason
                ,t.PlanDate, dbo.getTPEPass1(t.SMR) [SMR], dbo.getTPEPass1(T.MRHandle) [Handle]
                ,(select dbo.getTPEPass1(p.POSMR) from dbo.PO p WITH (NOLOCK) where p.ID =t.POID) [PO SMR]
                ,(select dbo.getTPEPass1(p.POHandle) from dbo.PO p WITH (NOLOCK) where p.ID =t.POID) [PO Handle]             
                ,dbo.getTPEPass1(t.McHandle) [MC Handle]
                ,t.DoxType
                ,(select article+',' from (select distinct q.Article  from dbo.Order_Qty q WITH (NOLOCK) where q.ID = t.OrderID) t for xml path('')) article_list                
                , (select Name from Reason WITH (NOLOCK) where ReasonTypeID = 'Style_SpecialMark' and ID = t.SpecialMark) [SpecMark]
                , t.GFR, t.SampleReason
                ,(select s.StdTms * t.CPU from System s WITH (NOLOCK) ) [TMS]
"));
            if (isArtwork) 
                sqlCmd.Append(string.Format(@",{0} ",artworktypes.ToString().Substring(0, artworktypes.ToString().Length - 1)));
            sqlCmd.Append(string.Format(@" from #cte t inner join #cte2 on #cte2.OrderID = t.OrderID  
left join Country with (Nolock) on Country.id= t.Dest"));
            if (isArtwork) 
                sqlCmd.Append(string.Format(@" left join #tmscost_pvt on #tmscost_pvt.orderid = t.orderid "));

            //KPIChangeReason
            sqlCmd.Append(@"  
outer apply ( select ID + '-' + Name KPIChangeReason  from Reason 
where ReasonTypeID = 'Order_BuyerDelivery' and ID = t.KPIChangeReason 
and t.KPIChangeReason !='' and t.KPIChangeReason is not null 
) KPIChangeReason 
outer apply (
	select 
	[RFID Cut Qty] = isnull(sum(BD.Qty), 0)   
	from Bundle B
	left join Bundle_Detail BD on BD.Id=B.ID
	left join BundleInOut BIO on BIO.BundleNo=BD.BundleNo 
	where Orderid=t.OrderID and BIO.SubProcessId='SORTING'
) CutQty
outer apply 
(
	select [AccuLoad] = AccuLoad from 
	(select distinct FactoryID,SP,StyleID,Line,AccuLoad,SewingDate from #print ) a
	where SP=t.OrderID and  StyleID=t.StyleID
	and FactoryID=t.FactoryID
	and line=t.SewLine
    and SewingDate= CONVERT(date, GETDATE())
)loading

outer apply(
	select [RFID Emb Farm In Qty] =isnull(sum(BD.Qty), 0) 
	from Bundle B
	left join Bundle_Detail BD on BD.Id=B.ID
	left join BundleInOut BIO on BIO.BundleNo=BD.BundleNo 
	where Orderid=t.OrderID and BIO.SubProcessId='Emb' and BIO.InComing is not null
)Embin
outer apply(
	select [RFID Emb Farm Out Qty] = isnull(sum(BD.Qty), 0) 
	from Bundle B
	left join Bundle_Detail BD on BD.Id=B.ID
	left join BundleInOut BIO on BIO.BundleNo=BD.BundleNo 
	where Orderid=t.OrderID and BIO.SubProcessId='Emb' and BIO.OutGoing is not null
)Embout
outer apply(
	select [RFID Bond Farm In Qty] =isnull(sum(BD.Qty), 0) 
	from Bundle B
	left join Bundle_Detail BD on BD.Id=B.ID
	left join BundleInOut BIO on BIO.BundleNo=BD.BundleNo 
	where Orderid=t.OrderID and BIO.SubProcessId='BO' and BIO.InComing is not null
)Bondin
outer apply(
	select [RFID Bond Farm Out Qty] = isnull(sum(BD.Qty), 0) 
	from Bundle B
	left join Bundle_Detail BD on BD.Id=B.ID
	left join BundleInOut BIO on BIO.BundleNo=BD.BundleNo 
	where Orderid=t.OrderID and BIO.SubProcessId='BO' and BIO.OutGoing is not null
)Bondout
outer apply(
	select [RFID Print Farm In Qty] =isnull(sum(BD.Qty), 0) 
	from Bundle B
	left join Bundle_Detail BD on BD.Id=B.ID
	left join BundleInOut BIO on BIO.BundleNo=BD.BundleNo 
	where Orderid=t.OrderID and BIO.SubProcessId='PRT' and BIO.InComing is not null
)Printin
outer apply(
	select [RFID Print Farm Out Qty] = isnull(sum(BD.Qty), 0) 
	from Bundle B
	left join Bundle_Detail BD on BD.Id=B.ID
	left join BundleInOut BIO on BIO.BundleNo=BD.BundleNo 
	where Orderid=t.OrderID and BIO.SubProcessId='PRT' and BIO.OutGoing is not null
)Printout
outer apply(
	select [RFID AT Farm In Qty] =isnull(sum(BD.Qty), 0) 
	from Bundle B
	left join Bundle_Detail BD on BD.Id=B.ID
	left join BundleInOut BIO on BIO.BundleNo=BD.BundleNo 
	where Orderid=t.OrderID and BIO.SubProcessId='AT' and BIO.InComing is not null
)ATin
outer apply(
	select [RFID AT Farm Out Qty] = isnull(sum(BD.Qty), 0) 
	from Bundle B
	left join Bundle_Detail BD on BD.Id=B.ID
	left join BundleInOut BIO on BIO.BundleNo=BD.BundleNo 
	where Orderid=t.OrderID and BIO.SubProcessId='AT' and BIO.OutGoing is not null
)ATout
outer apply(
	select [RFID Pad Print Farm In Qty] =isnull(sum(BD.Qty), 0) 
	from Bundle B
	left join Bundle_Detail BD on BD.Id=B.ID
	left join BundleInOut BIO on BIO.BundleNo=BD.BundleNo 
	where Orderid=t.OrderID and BIO.SubProcessId='PAD-PRT' and BIO.InComing is not null
)PadPrintin
outer apply(
	select [RFID Pad Print Farm Out Qty] = isnull(sum(BD.Qty), 0) 
	from Bundle B
	left join Bundle_Detail BD on BD.Id=B.ID
	left join BundleInOut BIO on BIO.BundleNo=BD.BundleNo 
	where Orderid=t.OrderID and BIO.SubProcessId='PAD-PRT' and BIO.OutGoing is not null
)PadPrintout
outer apply(
	select [RFID Emboss Farm In Qty] =isnull(sum(BD.Qty), 0) 
	from Bundle B
	left join Bundle_Detail BD on BD.Id=B.ID
	left join BundleInOut BIO on BIO.BundleNo=BD.BundleNo 
	where Orderid=t.OrderID and BIO.SubProcessId='SUBCONEMB' and BIO.InComing is not null
)Embossin
outer apply(
	select [RFID Emboss Farm Out Qty] = isnull(sum(BD.Qty), 0) 
	from Bundle B
	left join Bundle_Detail BD on BD.Id=B.ID
	left join BundleInOut BIO on BIO.BundleNo=BD.BundleNo 
	where Orderid=t.OrderID and BIO.SubProcessId='SUBCONEMB' and BIO.OutGoing is not null
)Embossout
outer apply(
	select [RFID HT Farm In Qty] =isnull(sum(BD.Qty), 0) 
	from Bundle B
	left join Bundle_Detail BD on BD.Id=B.ID
	left join BundleInOut BIO on BIO.BundleNo=BD.BundleNo 
	where Orderid=t.OrderID and BIO.SubProcessId='HT' and BIO.InComing is not null
)htin
outer apply(
	select [RFID HT Farm Out Qty] = isnull(sum(BD.Qty), 0) 
	from Bundle B
	left join Bundle_Detail BD on BD.Id=B.ID
	left join BundleInOut BIO on BIO.BundleNo=BD.BundleNo 
	where Orderid=t.OrderID and BIO.SubProcessId='HT' and BIO.OutGoing is not null
)htout

");

            sqlCmd.Append(string.Format(@" order by {0}", orderby));
            sqlCmd.Append(@" 
DROP TABLE #cte2,#cte
drop table #tsp
drop table #cutcomb
drop table #cur_bdltrack2
drop table #Min_cut
drop table #print
drop table #TablePatternUkey;
drop table #TablePatternCode
drop table #CBDate
");
            if (isArtwork)
            {
                sqlCmd.Append(@" drop table #rawdata_tmscost,#tmscost_pvt");
            }


            DBProxy.Current.DefaultTimeout = 1800;
            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), cmds, out printData);
            DBProxy.Current.DefaultTimeout = 0;
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }
            return Result.True;
        }

        // 產生Excel
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            SetCount(printData.Rows.Count);

            if (printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            if (printData.Columns.Count > 16384)
            {
                MyUtility.Msg.WarningBox("Columns of Data is over 16,384 in excel file, please narrow down range of condition.");
                return false;
            }

            if (printData.Rows.Count + 1 > 1048576)
            {
                MyUtility.Msg.WarningBox("Lines of Data is over 1,048,576 in excel file, please narrow down range of condition.");
                return false;
            }

            if (isArtwork)
            {
                Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Planning_R15_WIP.xltx"); //預先開啟excel app
                MyUtility.Excel.CopyToXls(printData, "", "Planning_R15_WIP.xltx", 1, false, null, objApp);      // 將datatable copy to excel
                Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表

                for (int i = 0; i < dtArtworkType.Rows.Count; i++)  //列印動態欄位的表頭
                {
                    objSheets.Cells[1, 55 + i] = dtArtworkType.Rows[i]["id"].ToString();
                }

                //首列資料篩選
                Microsoft.Office.Interop.Excel.Range firstRow = (Microsoft.Office.Interop.Excel.Range)objSheets.Rows[1];
                firstRow.AutoFilter(1, Type.Missing, Microsoft.Office.Interop.Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);
                objApp.Cells.EntireColumn.AutoFit();  //自動欄寬

                #region Save & Show Excel
                string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Planning_R15_WIP");
                Microsoft.Office.Interop.Excel.Workbook workbook = objApp.ActiveWorkbook;
                workbook.SaveAs(strExcelName);
                workbook.Close();
                objApp.Quit();
                Marshal.ReleaseComObject(objApp);
                Marshal.ReleaseComObject(objSheets);
                Marshal.ReleaseComObject(firstRow);
                Marshal.ReleaseComObject(workbook);

                strExcelName.OpenFile();
                #endregion
            }
            else
            {
                Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Planning_R15_WIP.xltx"); //預先開啟excel app
                MyUtility.Excel.CopyToXls(printData, "", "Planning_R15_WIP.xltx", 1, false, null, objApp);      // 將datatable copy to excel
                Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表

                //首列資料篩選
                Microsoft.Office.Interop.Excel.Range firstRow = (Microsoft.Office.Interop.Excel.Range)objSheets.Rows[1];
                firstRow.AutoFilter(1, Type.Missing, Microsoft.Office.Interop.Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);
                objApp.Cells.EntireColumn.AutoFit();  //自動欄寬

                #region Save & Show Excel
                string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Planning_R15_WIP");
                Microsoft.Office.Interop.Excel.Workbook workbook = objApp.ActiveWorkbook;
                workbook.SaveAs(strExcelName);
                workbook.Close();
                objApp.Quit();
                Marshal.ReleaseComObject(objApp);
                Marshal.ReleaseComObject(objSheets);
                Marshal.ReleaseComObject(firstRow);
                Marshal.ReleaseComObject(workbook);

                strExcelName.OpenFile();
                #endregion
            }
            
            return true;
        }
    }
}
