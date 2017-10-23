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
select o.MDivisionID
       , o.FactoryID
       , o.SciDelivery
       , O.CRDDate
       , O.CFMDate
       , OrderID = O.ID 
       , O.Dest
       , O.StyleID
       , O.SeasonID
       , O.ProjectID
       , O.Customize1
       , O.BuyMonth
       , O.CustPONo
       , O.BrandID
       , O.CustCDID
       , O.ProgramID
       , O.CdCodeID
       , O.CPU
       , O.Qty
       , O.FOCQty
       , O.PoPrice
       , O.CMPPrice
       , O.KPILETA
       , O.LETA
       , O.MTLETA
       , O.SewETA
       , O.PackETA
       , O.MTLComplete
       , O.SewInLine
       , O.SewOffLine
       , O.CutInLine
       , O.CutOffLine
       , O.Category
       , O.IsForecast
       , O.PulloutDate
       , O.ActPulloutDate
       , O.SMR
       , O.MRHandle
       , O.MCHandle
       , O.OrigBuyerDelivery
       , O.DoxType
       , O.TotalCTN
       , O.FtyCTN
       , O.ClogCTN
       , O.VasShas
       , O.TissuePaper
       , O.MTLExport
       , O.SewLine
       , O.ShipModeList
       , O.PlanDate
       , O.FirstProduction
       , O.Finished
       , O.FtyGroup
       , O.OrderTypeID
       , O.SpecialMark
       , O.GFR
       , O.SampleReason
       , O.InspDate
       , O.MnorderApv
       , O.FtyKPI
       , O.KPIChangeReason
       , O.StyleUkey
       , O.POID
       , InspResult = IIF(o.InspResult='P', 'Pass'
                                          , IIF(o.InspResult = 'F', 'Fail'
                                                                  , ''))
       , InspHandle = (o.InspHandle +'-'+ I.Name)
       , OrdersBuyerDelivery = o.BuyerDelivery
into #cte 
from dbo.Orders o WITH (NOLOCK) 
OUTER APPLY(
      SELECT  Name 
      FROM Pass1 WITH (NOLOCK) 
      WHERE Pass1.ID = O.InspHandle
)I
WHERE 1=1"));

            #region --- 條件組合  ---
            if (!MyUtility.Check.Empty(sciDelivery1))
            {
                sqlCmd.Append(string.Format(@" 
      and o.SciDelivery >= '{0}'", Convert.ToDateTime(sciDelivery1).ToString("d")));
            }
            if (!MyUtility.Check.Empty(sciDelivery2))
            {
                sqlCmd.Append(string.Format(@" 
      and o.SciDelivery <= '{0}'", Convert.ToDateTime(sciDelivery2).ToString("d")));
            }

            if (MyUtility.Check.Empty(BuyerDelivery1) == false && MyUtility.Check.Empty(BuyerDelivery2) == false)
            {
                sqlCmd.Append(string.Format(@" 
      and exists (
            select 1 
            from Order_QtyShip s WITH (NOLOCK) 
            where s.id = o.ID
                  and s.BuyerDelivery between '{0}' and '{1}'
      )", Convert.ToDateTime(BuyerDelivery1).ToString("d")
                                                              , Convert.ToDateTime(BuyerDelivery2).ToString("d")));
            }
            else if (!MyUtility.Check.Empty(BuyerDelivery1))
            {
                sqlCmd.Append(string.Format(@" 
      and exists (
            select 1 
            from Order_QtyShip s WITH (NOLOCK) 
            where s.id = o.ID
                  and s.BuyerDelivery >= '{0}'
      )", Convert.ToDateTime(BuyerDelivery1).ToString("d")));
            }
            else if (!MyUtility.Check.Empty(BuyerDelivery2))
            {
                sqlCmd.Append(string.Format(@" 
      and exists (
            select 1 
            from Order_QtyShip s WITH (NOLOCK) 
            where s.id = o.ID
                  and s.BuyerDelivery <= '{0}'
      )", Convert.ToDateTime(BuyerDelivery2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(CustRqsDate1))
            {
                sqlCmd.Append(string.Format(@" 
      and o.CRDDate >= '{0}'", Convert.ToDateTime(CustRqsDate1).ToString("d")));
            }
            if (!MyUtility.Check.Empty(CustRqsDate2))
            {
                sqlCmd.Append(string.Format(@" 
      and o.CRDDate <= '{0}'", Convert.ToDateTime(CustRqsDate2).ToString("d")));
            }
            if (!MyUtility.Check.Empty(CutOffDate1))
            {
                sqlCmd.Append(string.Format(@" 
      and o.SDPDate >= '{0}'", Convert.ToDateTime(CutOffDate1).ToString("d")));
            }
            if (!MyUtility.Check.Empty(CutOffDate2))
            {
                sqlCmd.Append(string.Format(@" 
      and o.SDPDate <= '{0}'", Convert.ToDateTime(CutOffDate2).ToString("d")));
            }
            if (!MyUtility.Check.Empty(planDate1))
            {
                sqlCmd.Append(string.Format(@" 
      and o.PlanDate >= '{0}'", Convert.ToDateTime(planDate1).ToString("d")));
            }
            if (!MyUtility.Check.Empty(planDate2))
            {
                sqlCmd.Append(string.Format(@" 
      and o.PlanDate <= '{0}'", Convert.ToDateTime(planDate2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(spno1))
            {
                sqlCmd.Append(@" 
      and o.id >= @spno1 ");
                cmds.Add(new System.Data.SqlClient.SqlParameter("@spno1", spno1));
            }
            if (!MyUtility.Check.Empty(spno2))
            {
                sqlCmd.Append(@" 
      and o.id <= @spno2 ");
                cmds.Add(new System.Data.SqlClient.SqlParameter("@spno2", spno2));
            }

            if (!MyUtility.Check.Empty(brandid))
            {
                sqlCmd.Append(string.Format(@"
      and o.brandid = @brandid"));
                cmds.Add(new System.Data.SqlClient.SqlParameter("@brandid", brandid));
            }

            if (!MyUtility.Check.Empty(custcd))
            {
                sqlCmd.Append(string.Format(@" 
      and o.CustCDID = @custcd"));
                cmds.Add(new System.Data.SqlClient.SqlParameter("@custcd", custcd));
            }

            if (!MyUtility.Check.Empty(mdivision))
            {
                sqlCmd.Append(@" 
      and o.mdivisionid = @MDivision");
                cmds.Add(new System.Data.SqlClient.SqlParameter("@MDivision", mdivision));
            }

            if (!MyUtility.Check.Empty(factory))
            {
                sqlCmd.Append(@" 
      and o.factoryid = @factory");
                cmds.Add(new System.Data.SqlClient.SqlParameter("@factory", factory));
            }

            switch (selectindex)
            {
                case 0:
                    sqlCmd.Append(@" 
      and (o.Category = 'B' or o.Category = 'S')");
                    break;
                case 1:
                    sqlCmd.Append(@" 
      and o.Category = 'B' ");
                    break;
                case 2:
                    sqlCmd.Append(@" 
      and (o.Category = 'S')");
                    break;
            }

            #endregion

            #region -- 有列印Artwork --
            if (isArtwork)
            {
                sqlCmd.Append(@"
--依取得的訂單資料取得訂單的 TMS Cost
select aa.orderid
       , bb.ArtworkTypeID
       , price_tms = iif(cc.IsTMS=1,bb.tms,bb.price)  
into #rawdata_tmscost
from #cte aa 
inner join dbo.Order_TmsCost bb on bb.id = aa.orderid
inner join dbo.ArtworkType cc on cc.id = bb.ArtworkTypeID
where IsTMS =1 or IsPrice = 1
                ");

                sqlCmd.Append(string.Format(@"
--將取得Tms Cost做成樞紐表
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

            #region -- 產生所有 SubProcessID Qty --
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
*   
*   ※除了 SORTING, LOADING 
*   ※其他 SubProcess 需依照 Pattern_GL.Annotation
*   ※計算各個部位如何才能算一件成衣
*/
Select	distinct tpu.Poid
		, PatternCode = case
							when pg.Annotation = '' or pg.Annotation is null then 'ALLPARTS'
							else PatternCode.value
						end
		, F_CODE = a.FabricPanelCode
        , SubProcessID = SubProcessID.Data
into #TablePatternCode
from #TablePatternUkey tpu
inner join Pattern_GL pg WITH (NOLOCK) on tpu.UKey = pg.PatternUKEY
inner join Pattern_GL_LectraCode a WITH (NOLOCK) on	 a.PatternUkey = tpu.Ukey
														and a.FabricCode != ''
														and pg.SEQ = a.SEQ
outer apply (
	select value = iif(a.SEQ = '0001', SubString(a.PatternCode, 11, Len(a.PatternCode)), a.PatternCode)
) PatternCode
outer apply (
	select Data = splitAnnotation.Data
	from dbo.SplitString(pg.Annotation, '+') splitAnnotation
	inner join SubProcess sp on splitAnnotation.Data = sp.Id

	union all 
	select Data = 'SORTING'

	union all
	select Data = 'LOADING'
) SubProcessID

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
        , #cur_artwork.SubProcessID
        , isInComing = isInComing.value
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
            , tpc.SubProcessID
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
outer apply (
    select value = 'T'
    union all
    select value = 'F'
) isInComing

/*
* Step3
* 取出被各個部門收下的 Bundle Data
*		排除
*			1. Order_BOF.Kind = 0 (表Other) 
*			2. Order_BOF.Kind = 3 (表Polyfill)的資料
*			3. 外裁項 【Bundle_Detail 不會出現外裁項的資料】
*		BundleGroup 必須是同 Group 組合才能算一件衣服，因為不同 Group 可能會有色差
*/
select	b.orderid
		, bd.SizeCode
		, b.Article
		, bd.BundleGroup
		, FabricCombo = wo.FabricPanelCode
		, PatternCode = bd.Patterncode
		, artwork = isnull(ArtWork.value, '')
		, qty = isnull (bd.qty, 0)
        , bio.SubProcessId
        , haveOutGoing = case 
                            when bio.OutGoing is not null then 'T'
                            else 'F'
                         end
into #tmpBundleInOutQty
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
where	ob.Kind not in ('0','3')

/*
 * 分成兩個群組 
 * 1. InComing
 * 2. OutGoing
 */ 
select *
into #cur_bdltrack2
from (
    select 	orderid
		    , SizeCode
		    , Article
		    , BundleGroup
		    , FabricCombo
		    , PatternCode
		    , artwork
		    , qty = sum (isnull (qty, 0))
            , SubProcessId
            , isInComing = 'T'
    from #tmpBundleInOutQty
    group by orderid, SizeCode, Article, BundleGroup, FabricCombo, PatternCode
             , artwork, SubProcessId

    union all
    select  orderid
		    , SizeCode
		    , Article
		    , BundleGroup
		    , FabricCombo
		    , PatternCode
		    , artwork
		    , qty = sum (isnull (qty, 0))
            , SubProcessId
            , isInComing = 'F'
    from #tmpBundleInOutQty
    where haveOutGoing = 'T'
    group by orderid, SizeCode, Article, BundleGroup, FabricCombo, PatternCode
             , artwork, SubProcessId
) x;

------------------------------------------------------------------------------------------------------------------

/*	
*	Step4 已收的 bundle 資料中找出 
*			article 
*			size
*			部位
*			artwork
*		  各個部門的加總數量
*	Step5 計算 sp# 的產出
*/
select	NewCutComb.FactoryID
		, NewCutComb.orderid
		, NewCutComb.StyleID
		, NewCutComb.SewLine
		, NewCutComb.Article
		, NewCutComb.SizeCode
		, NewCutComb.BundleGroup
		, NewCutComb.FabricCombo
		, NewCutComb.PatternCode
		, NewCutComb.artwork
		, AccuInComingQty = sum(isnull(#cur_bdltrack2.qty, 0))
        , SubProcessId = NewCutComb.SubProcessId
        , isInComing = NewCutComb.isInComing
into #Min_cut
from (
/*
*	組出，同 OrderID, Artwork, Article, SizeCode 需要的 FabricCombe & PatternCode
*/
	select	distinct #cutcomb.*
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
) NewCutComb 
left join #cur_bdltrack2 on	NewCutComb.artwork = #cur_bdltrack2.artwork 
							and NewCutComb.FabricCombo = #cur_bdltrack2.FabricCombo
							and NewCutComb.PatternCode = #cur_bdltrack2.PatternCode
							and NewCutComb.orderID = #cur_bdltrack2.orderid
							and NewCutComb.Article = #cur_bdltrack2.Article 
							and NewCutComb.SizeCode = #cur_bdltrack2.SizeCode
							and NewCutComb.BundleGroup = #cur_bdltrack2.BundleGroup
                            and NewCutComb.SubProcessId = #cur_bdltrack2.SubProcessId
                            and NewCutComb.isInComing = #cur_bdltrack2.isInComing
group by NewCutComb.FactoryID, NewCutComb.orderid, NewCutComb.StyleID, NewCutComb.SewLine, NewCutComb.Article
         , NewCutComb.SizeCode, NewCutComb.BundleGroup, NewCutComb.FabricCombo, NewCutComb.PatternCode
         , NewCutComb.artwork, NewCutComb.SubProcessId, NewCutComb.isInComing;
--order by orderid, Article, SizeCode, FabricCombo, PatternCode, artwork

-----------------------------------------------------------

/*
*	準備 InComing & OutGoing 資料
*/
select	FactoryID
		, SP
		, StyleID
		, Line
        , SubProcessId
		, AccuQty = AccuInComing
        , isInComing
into #AccuInComeData
from (
	select	FactoryID
			, SP = orderID
			, StyleID
			, Line = SewLine
            , SubProcessId
			, AccuInComing = sum (isnull(MinInComingQty, 0))
            , isInComing
	from (
/*
*		依照【SP#, Article, Size, Comb, Artwork】取最小數量 (因為每個部位都要有，才能成為一件衣服)
*		判斷 BundleGroup
*			第一次群組判斷最小數量需要加上 BundleGroup 
*                 (BundleGroup 必須是同 Group 組合才能算一件衣服，因為不同 Group 可能會有色差)
*			第二次群組加總所有成衣數量		
*/
		select FactoryID
				, orderID
				, StyleID
				, SewLine
				, Article
                , SizeCode
                , SubProcessId
				, MinInComingQty = sum (MinInComingQty)
                , isInComing
		from (
			select	FactoryID
					, orderID
					, StyleID
					, SewLine
					, Article
                    , SizeCode
                    , SubProcessId
					, MinInComingQty = min(AccuInComingQty)
                    , isInComing
			from #Min_cut 
			group by FactoryID, orderID	,StyleID, SewLine, Article, SizeCode, BundleGroup, SubProcessId, isInComing
		)w
		group by FactoryID, orderID	,StyleID, SewLine, Article, SizeCode, SubProcessId, isInComing
	)x 
	group by FactoryID, orderID, StyleID, SewLine, SubProcessId, isInComing
) a
");
            #endregion

            sqlCmd.Append(string.Format(@"
                -- 依撈出來的order資料(cte)去找各製程的WIP
select t.OrderID
       , cut_qty = (SELECT SUM(CWIP.Qty) 
                    FROM DBO.CuttingOutput_WIP CWIP WITH (NOLOCK) 
                    WHERE CWIP.OrderID = T.OrderID)
       , first_cut_date = (SELECT MIN(a.cDate) 
                           from dbo.CuttingOutput a WITH (NOLOCK) 
                           inner join dbo.CuttingOutput_Detail b WITH (NOLOCK) on b.id = a.id 
                           inner join dbo.WorkOrder_Distribute c WITH (NOLOCK) on c.WorkOrderUkey = b.WorkOrderUkey
                           where c.OrderID = t.OrderID) 
       , sewing_output = (select MIN(isnull(tt.qaqty,0)) 
                          from dbo.style_location sl WITH (NOLOCK) 
                          left join (
                                SELECT b.ComboType
                                       , qaqty = sum(b.QAQty)  
                                FROM DBO.SewingOutput a WITH (NOLOCK) 
                                inner join dbo.SewingOutput_Detail b WITH (NOLOCK) on b.ID = a.ID
                                where b.OrderId = t.OrderID
                                group by ComboType 
                          ) tt on tt.ComboType = sl.Location
                          where sl.StyleUkey = t.StyleUkey) 
       , t.StyleUkey
       , EMBROIDERY_qty = (select qty = min(qty)  
                           from (
                                select qty = sum(b.Qty) 
                                       , c.PatternCode
                                       , c.ArtworkID 
                                from dbo.farmin a WITH (NOLOCK) 
                                inner join dbo.FarmIn_Detail b WITH (NOLOCK) on b.id = a.id 
                                right join (
                                    select distinct v.ArtworkTypeID
                                           , v.Article
                                           , v.ArtworkID
                                           , v.PatternCode 
                                   from dbo.View_Order_Artworks v 
                                   where v.ID=t.OrderID
                                ) c on c.ArtworkTypeID = a.ArtworkTypeId 
                                       and c.PatternCode = b.PatternCode 
                                       and c.ArtworkID = b.ArtworkID
                                where a.ArtworkTypeId='EMBROIDERY' 
                                      and b.Orderid = t.OrderID
                                group by c.PatternCode,c.ArtworkID
                          ) x) 
       , BONDING_qty = (select qty = min(qty)  
                        from (
                           select qty = sum(b.Qty)  
                                  , c.PatternCode
                                  , c.ArtworkID 
                           from dbo.farmin a WITH (NOLOCK) 
                           inner join dbo.FarmIn_Detail b WITH (NOLOCK) on b.id = a.id 
                           right join (
                                select distinct v.ArtworkTypeID
                                       , v.ArtworkID
                                       , v.PatternCode 
                                from dbo.View_Order_Artworks v 
                                where v.ID = t.OrderID
                           ) c on c.ArtworkTypeID = a.ArtworkTypeId 
                                  and c.PatternCode = b.PatternCode 
                                  and c.ArtworkID = b.ArtworkID
                           where a.ArtworkTypeId='BONDING' 
                                 and b.Orderid = t.OrderID
                           group by c.PatternCode, c.ArtworkID
                       ) x) 
       , PRINTING_qty = (select qty = min(qty) 
                         from (
                           select qty = sum(b.Qty) 
                                  , c.PatternCode
                                  , c.ArtworkID 
                           from dbo.farmin a WITH (NOLOCK) 
                           inner join dbo.FarmIn_Detail b WITH (NOLOCK) on b.id = a.id 
                           right join (
                                select distinct v.ArtworkTypeID
                                       , v.ArtworkID
                                       , v.PatternCode 
                                from dbo.View_Order_Artworks v 
                                where v.ID=t.OrderID
                           ) c on c.ArtworkTypeID = a.ArtworkTypeId 
                                  and c.PatternCode = b.PatternCode 
                                  and c.ArtworkID = b.ArtworkID
                           where a.ArtworkTypeId = 'PRINTING' 
                                 and b.Orderid = t.OrderID
                           group by c.PatternCode, c.ArtworkID
                         ) x) 
       , SEWOUTPUT.*
into #cte2
from #cte t
outer apply (
    SELECT firstSewingDate = min(X.OutputDate) 
           , lastestSewingDate = max(X.OutputDate) 
           , QAQTY = sum(X.QAQty) 
           , AVG_QAQTY = AVG(X.QAQTY)
    from (
        SELECT a.OutputDate
               , QAQty = sum(a.QAQty) 
        FROM DBO.SewingOutput a WITH (NOLOCK) 
        inner join dbo.SewingOutput_Detail b WITH (NOLOCK) on b.ID = a.ID
        where b.OrderId = t.OrderID 
        group by a.OutputDate 
    ) X
) SEWOUTPUT

select t.MDivisionID
       , t.FactoryID
       , t.SewLine
       , t.OrdersBuyerDelivery
       , t.SewInLine
       , t.SewOffLine
       , t.BrandID
       , t.OrderID
       , Dest = Country.Alias
       , t.StyleID
       , t.OrderTypeID
       , t.ShipModeList
       , [OrderNo] = t.Customize1
       , t.CustPONo
       , t.CustCDID
       , t.ProgramID
       , t.CdCodeID
       , t.KPILETA
       , t.LETA
       , t.MTLETA
       , t.SewETA
       , t.PackETA
       , t.CPU
       , t.Qty
       , #cte2.first_cut_date
       , #cte2.cut_qty
       , [RFID Cut Qty] = isnull (CutQty.AccuInCome, 0)
       , [RFID Loading Qty] = isnull (loading.AccuInCome,0)             
       , [RFID Emb Farm In Qty] = isnull (Embin.AccuInCome, 0)
       , [RFID Emb Farm Out Qty] = isnull (Embout.AccuOutGo, 0)
       , [RFID Bond Farm In Qty] = isnull (Bondin.AccuInCome, 0)
       , [RFID Bond Farm Out Qty] = isnull (Bondout.AccuOutGo, 0)
       , [RFID Print Farm In Qty] = isnull (Printin.AccuInCome, 0)
       , [RFID Print Farm Out Qty] = isnull (Printout.AccuOutGo, 0)
       , [RFID AT Farm In Qty] = isnull (ATin.AccuInCome, 0)
       , [RFID AT Farm Out Qty] = isnull (ATout.AccuOutGo, 0)
       , [RFID Pad Print Farm In Qty] = isnull (PadPrintin.AccuInCome, 0)
       , [RFID Pad Print Farm Out Qty] = isnull (PadPrintout.AccuOutGo, 0)
       , [RFID Emboss Farm In Qty] = isnull (Embossin.AccuInCome, 0)
       , [RFID Emboss Farm Out Qty] = isnull (Embossout.AccuOutGo, 0)
       , [RFID HT Farm In Qty] = isnull (htin.AccuInCome, 0)
       , [RFID HT Farm Out Qty] = isnull (htout.AccuOutGo, 0)
       , #cte2.EMBROIDERY_qty
       , #cte2.BONDING_qty
       , #cte2.PRINTING_qty
       , #cte2.sewing_output
       , [Balance] = t.qty + t.FOCQty - #cte2.sewing_output 
       , #cte2.firstSewingDate              
       , [Last Sewn Date] = (select Max(so.OutputDate) 
                             from SewingOutput so WITH (NOLOCK) 
                             inner join SewingOutput_Detail sod WITH (NOLOCK) on so.ID = sod.ID
                             where sod.OrderID = t.OrderID)
       , #cte2.AVG_QAQTY
       , [Est_offline] = DATEADD(DAY
                                 , iif(isnull(#cte2.AVG_QAQTY, 0) = 0, 0
                                                                     , ceiling((t.qty+t.FOCQty - #cte2.sewing_output) / (#cte2.AVG_QAQTY*1.0)))
                                 , #cte2.firstSewingDate) 
       , [pack_rate] = IIF(isnull(t.TotalCTN, 0) = 0, 0
                                                    , round(t.ClogCTN / (t.TotalCTN * 1.0), 4) * 100 ) 
       , t.TotalCTN                
       , FtyCtn = t.TotalCTN - t.FtyCTN
       , t.ClogCTN
       , t.InspDate
       , InspResult
       , [CFA Name] = InspHandle
       , t.ActPulloutDate
       , t.FtyKPI                
       , KPIChangeReason = KPIChangeReason.KPIChangeReason  
       , t.PlanDate
       , dbo.getTPEPass1(t.SMR) [SMR]
       , dbo.getTPEPass1(T.MRHandle) [Handle]
       , [PO SMR] = (select dbo.getTPEPass1(p.POSMR) 
                     from dbo.PO p WITH (NOLOCK) 
                     where p.ID = t.POID) 
       , [PO Handle] = (select dbo.getTPEPass1(p.POHandle) 
                        from dbo.PO p WITH (NOLOCK) 
                        where p.ID = t.POID)   
       , [MC Handle] = dbo.getTPEPass1(t.McHandle) 
       , t.DoxType
       , article_list = (select article + ',' 
                         from (
                              select distinct q.Article  
                              from dbo.Order_Qty q WITH (NOLOCK) 
                              where q.ID = t.OrderID
                         ) t 
                         for xml path('')) 
       , [SpecMark] = (select Name 
                       from Reason WITH (NOLOCK) 
                       where ReasonTypeID = 'Style_SpecialMark' 
                             and ID = t.SpecialMark) 
       , t.GFR
       , t.SampleReason
       , [TMS] = (select s.StdTms * t.CPU 
                  from System s WITH (NOLOCK)) 
"));
            if (isArtwork) 
                sqlCmd.Append(string.Format(@",{0} ",artworktypes.ToString().Substring(0, artworktypes.ToString().Length - 1)));
            sqlCmd.Append(string.Format(@" 
from #cte t 
inner join #cte2 on #cte2.OrderID = t.OrderID 
left join Country with (Nolock) on Country.id= t.Dest"));
            if (isArtwork) 
                sqlCmd.Append(string.Format(@" 
left join #tmscost_pvt on #tmscost_pvt.orderid = t.orderid "));

            //KPIChangeReason
            sqlCmd.Append(@"  
outer apply ( 
    select KPIChangeReason = ID + '-' + Name   
    from Reason 
    where ReasonTypeID = 'Order_BuyerDelivery' 
          and ID = t.KPIChangeReason 
          and t.KPIChangeReason != '' 
          and t.KPIChangeReason is not null 
) KPIChangeReason 
outer apply (
	select [AccuInCome] = iif (AccuQty > t.Qty, t.Qty, AccuQty)
    from #AccuInComeData 
	where SP = t.OrderID 
          and StyleID = t.StyleID
	      and FactoryID = t.FactoryID
	      and line = t.SewLine
          and SubProcessId = 'SORTING'
          and isInComing = 'T'
) CutQty
outer apply (
	select [AccuInCome] = iif (AccuQty > t.Qty, t.Qty, AccuQty) 
    from #AccuInComeData 
	where SP = t.OrderID 
          and StyleID = t.StyleID
	      and FactoryID = t.FactoryID
	      and line = t.SewLine
          and SubProcessId = 'loading'
          and isInComing = 'T'
) loading
outer apply(
	select [AccuInCome] = iif (AccuQty > t.Qty, t.Qty, AccuQty) 
    from #AccuInComeData 
	where SP = t.OrderID 
          and StyleID = t.StyleID
	      and FactoryID = t.FactoryID
	      and line = t.SewLine
          and SubProcessId = 'Emb'
          and isInComing = 'T'
) Embin
outer apply(
	select [AccuOutGo] = iif (AccuQty > t.Qty, t.Qty, AccuQty) 
    from #AccuInComeData 
	where SP = t.OrderID 
          and StyleID = t.StyleID
	      and FactoryID = t.FactoryID
	      and line = t.SewLine
          and SubProcessId = 'Emb'
          and isInComing = 'F'
) Embout
outer apply(
	select [AccuInCome] = iif (AccuQty > t.Qty, t.Qty, AccuQty) 
    from #AccuInComeData 
	where SP = t.OrderID 
          and StyleID = t.StyleID
	      and FactoryID = t.FactoryID
	      and line = t.SewLine
          and SubProcessId = 'BO'
          and isInComing = 'T'
) Bondin
outer apply(
	select [AccuOutGo] = iif (AccuQty > t.Qty, t.Qty, AccuQty) 
    from #AccuInComeData 
	where SP = t.OrderID 
          and StyleID = t.StyleID
	      and FactoryID = t.FactoryID
	      and line = t.SewLine
          and SubProcessId = 'BO'
          and isInComing = 'F'
) Bondout
outer apply(
	select [AccuInCome] = iif (AccuQty > t.Qty, t.Qty, AccuQty) 
    from #AccuInComeData 
	where SP = t.OrderID 
          and StyleID = t.StyleID
	      and FactoryID = t.FactoryID
	      and line = t.SewLine
          and SubProcessId = 'PRT'
          and isInComing = 'T'
) Printin
outer apply(
	select [AccuOutGo] = iif (AccuQty > t.Qty, t.Qty, AccuQty) 
    from #AccuInComeData 
	where SP = t.OrderID 
          and StyleID = t.StyleID
	      and FactoryID = t.FactoryID
	      and line = t.SewLine
          and SubProcessId = 'PRT'
          and isInComing = 'F'
) Printout
outer apply(
	select [AccuInCome] = iif (AccuQty > t.Qty, t.Qty, AccuQty) 
    from #AccuInComeData 
	where SP = t.OrderID 
          and StyleID = t.StyleID
	      and FactoryID = t.FactoryID
	      and line = t.SewLine
          and SubProcessId = 'AT'
          and isInComing = 'T'
) ATin
outer apply(
	select [AccuOutGo] = iif (AccuQty > t.Qty, t.Qty, AccuQty) 
    from #AccuInComeData 
	where SP = t.OrderID 
          and StyleID = t.StyleID
	      and FactoryID = t.FactoryID
	      and line = t.SewLine
          and SubProcessId = 'AT'
          and isInComing = 'F'
) ATout
outer apply(
	select [AccuInCome] = iif (AccuQty > t.Qty, t.Qty, AccuQty) 
    from #AccuInComeData 
	where SP = t.OrderID 
          and StyleID = t.StyleID
	      and FactoryID = t.FactoryID
	      and line = t.SewLine
          and SubProcessId = 'PAD-PRT'
          and isInComing = 'T'
) PadPrintin
outer apply(
	select [AccuOutGo] = iif (AccuQty > t.Qty, t.Qty, AccuQty) 
    from #AccuInComeData 
	where SP = t.OrderID 
          and StyleID = t.StyleID
	      and FactoryID = t.FactoryID
	      and line = t.SewLine
          and SubProcessId = 'PAD-PRT'
          and isInComing = 'F'
) PadPrintout
outer apply(
	select [AccuInCome] = iif (AccuQty > t.Qty, t.Qty, AccuQty) 
    from #AccuInComeData 
	where SP = t.OrderID 
          and StyleID = t.StyleID
	      and FactoryID = t.FactoryID
	      and line = t.SewLine
          and SubProcessId = 'SUBCONEMB'
          and isInComing = 'T'
) Embossin
outer apply(
	select [AccuOutGo] = iif (AccuQty > t.Qty, t.Qty, AccuQty) 
    from #AccuInComeData 
	where SP = t.OrderID 
          and StyleID = t.StyleID
	      and FactoryID = t.FactoryID
	      and line = t.SewLine
          and SubProcessId = 'SUBCONEMB'
          and isInComing = 'F'
) Embossout
outer apply(
	select [AccuInCome] = iif (AccuQty > t.Qty, t.Qty, AccuQty) 
    from #AccuInComeData 
	where SP = t.OrderID 
          and StyleID = t.StyleID
	      and FactoryID = t.FactoryID
	      and line = t.SewLine
          and SubProcessId = 'HT'
          and isInComing = 'T'
) htin
outer apply(
	select [AccuOutGo] = iif (AccuQty > t.Qty, t.Qty, AccuQty) 
    from #AccuInComeData 
	where SP = t.OrderID 
          and StyleID = t.StyleID
	      and FactoryID = t.FactoryID
	      and line = t.SewLine
          and SubProcessId = 'HT'
          and isInComing = 'F'
) htout
");

            sqlCmd.Append(string.Format(@" order by {0}", orderby));
            sqlCmd.Append(@" 
DROP TABLE #cte2, #cteㄝ, #tsp, #cutcomb, #tmpBundleInOutQty, #cur_bdltrack2, #Min_cut
           , #AccuInComeData, #TablePatternUkey, #TablePatternCode;
");
            if (isArtwork)
            {
                sqlCmd.Append(@" drop table #rawdata_tmscost,#tmscost_pvt");
            }


            DBProxy.Current.DefaultTimeout = 2700;
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
                    objSheets.Cells[1, 74 + i] = dtArtworkType.Rows[i]["id"].ToString();
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
