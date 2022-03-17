using Ict;
using Sci.Data;
using Sci.Production.Class;
using Sci.Utility.Excel;
using Sci.Win;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Planning
{
    /// <summary>
    /// R17
    /// </summary>
    public partial class R17 : Win.Tems.PrintForm
    {
        private DataTable gdtOrderDetail;
        private DataTable gdtPullOut;
        private DataTable gdtSP;
        private DataTable gdtSDP;
        private string KpiDate;

        /// <summary>
        /// R17
        /// </summary>
        public R17()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// R17
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R17(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.EditMode = true;
            this.print.Visible = false;
            this.txtFactory.Text = Env.User.Factory;
            this.dateFactoryKPIDate.Select();
        }

        /// <summary>
        /// OnToExcel
        /// </summary>
        /// <param name="report">report</param>
        /// <returns>bool</returns>
        protected override bool OnToExcel(ReportDefinition report)
        {
            return true;
        }

        /// <summary>
        /// ValidateInput
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(this.dateFactoryKPIDate.Value1))
            {
                MyUtility.Msg.ErrorBox("Begin Factory KPI Date can not empty!");
                return false;
            }

            if (MyUtility.Check.Empty(this.dateFactoryKPIDate.Value2))
            {
                MyUtility.Msg.ErrorBox("End Factory KPI Date can not empty!");
                return false;
            }

            this.KpiDate = MyUtility.Convert.GetString(this.comboDropDownList1.SelectedValue);
            return base.ValidateInput();
        }

        /// <summary>
        /// OnAsyncDataLoad
        /// </summary>
        /// <param name="e">e</param>
        /// <returns>DualResult</returns>
        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            DualResult result = new DualResult(true);
            try
            {
                #region Order Detail
                string strSQL = @"
SELECT
	 CountryID = F.CountryID
	,KPICode = F.KPICode
	,FactoryID = o.FactoryID
	,OrderID = o.ID
	,o.StyleID
	,Seq = Order_QS.seq
	,BrandID = o.BrandID
	,Order_QS.BuyerDelivery
	,Order_QS.FtyKPI 
	,Order_QS.ShipmodeID
	,b.OTDExtension 
	,DeliveryByShipmode = Order_QS.ShipmodeID
	,OrderQty = Cast(Order_QS.QTY as int)										
	,Shipmode = Order_QS.ShipmodeID
	,GMTComplete = CASE o.GMTComplete WHEN 'C' THEN 'Y' 
							WHEN 'S' THEN 'S' ELSE '' END
	,Order_QS.ReasonID
	,ReasonName = case o.Category when 'B' then r.Name
	  when 'S' then rs.Name
	  else '' end
	,o.MRHandle
	,o.SMR
	,PO.POHandle
	,PO.POSMR
	,o.OrderTypeID
	,ot.isDevSample				
	,c.alias
	,o.MDivisionID 
	,o.localorder
    , OutsdReason = rd.Name
    , ReasonRemark = o.OutstandingRemark
    ,o.OnsiteSample
	,[CFAFinalInspectDate]=format(Order_QS.CFAFinalInspectDate, 'yyyy/MM/dd')
	,Order_QS.CFAFinalInspectResult
	,[CFA3rdInspectDate]=format(Order_QS.CFA3rdInspectDate, 'yyyy/MM/dd')
	,Order_QS.CFA3rdInspectResult
    ,[IDDReason] = cr.Description
into #tmp_main
FROM Orders o WITH (NOLOCK)
LEFT JOIN OrderType ot on o.OrderTypeID = ot.ID and o.BrandID = ot.BrandID and o.BrandID = ot.BrandID
LEFT JOIN Factory f ON o.FACTORYID = f.ID --AND  o.FactoryID = f.KPICode
LEFT JOIN Country c ON F.COUNTRYID = c.ID 
inner JOIN Order_QtyShip Order_QS on Order_QS.id = o.id
LEFT JOIN PO ON o.POID = PO.ID
LEFT JOIN Reason r on r.id = Order_QS.ReasonID and r.ReasonTypeID = 'Order_BuyerDelivery'          
LEFT JOIN Reason rs on rs.id = Order_QS.ReasonID and rs.ReasonTypeID = 'Order_BuyerDelivery_sample'
Left join Reason rd on rd.id = o.OutstandingReason and rd.ReasonTypeID = 'Delivery_OutStand'
LEFT JOIN Brand b on o.BrandID = b.ID
LEFT JOIN ClogReason cr ON cr.ID = Order_QS.ClogReasonID  AND cr.Type='ID'
where o.Junk = 0  
and (isnull(ot.IsGMTMaster,0) = 0 or o.OrderTypeID = '') 
and o.LocalOrder <> 1
and o.IsForecast <> 1
and exists (select 1 from Factory where o.FactoryId = id and IsProduceFty = 1)
";

                if (this.radioBulk.Checked)
                {
                    strSQL += " AND o.Category = 'B' AND f.Type = 'B'";
                }
                else if (this.radioSample.Checked)
                {
                    strSQL += " AND o.Category = 'S' AND f.Type = 'S'";
                }
                else
                {
                    strSQL += " AND o.Category = 'G'";
                }

                string kpiDate = string.Empty;
                if (this.KpiDate == "1")
                {
                    kpiDate = "FtyKPI";
                }
                else
                {
                    kpiDate = "BuyerDelivery";
                }

                if (this.dateFactoryKPIDate.Value1 != null)
                {
                    strSQL += $" AND Order_QS.{kpiDate} >= '{this.dateFactoryKPIDate.Value1.Value.ToString("yyyy-MM-dd")}'";
                }

                if (this.dateFactoryKPIDate.Value2 != null)
                {
                    strSQL += $" AND Order_QS.{kpiDate} <= '{this.dateFactoryKPIDate.Value2.Value.ToString("yyyy-MM-dd")}'";
                }

                if (this.txtFactory.Text != string.Empty)
                {
                    strSQL += string.Format(" AND f.KPICode = '{0}' ", this.txtFactory.Text);
                }

                strSQL += $@"

select COUNT(op.pulloutdate) PulloutDate ,op.OrderID,op.OrderShipmodeSeq
into #tmp_Pullout_Detail
from Pullout_Detail op 
inner join #tmp_main t on  op.OrderID = t.OrderID and op.OrderShipmodeSeq = t.Seq 
group by op.OrderID,op.OrderShipmodeSeq
	
Select 
	Qty = Sum(rA.Qty),
	FailQty = Sum(rB.Qty),
	pd.OrderID,
	pd.OrderShipmodeSeq
into #tmp_Pullout_Detail_pd
From Pullout_Detail pd  with (nolock)
inner join #tmp_main t on pd.OrderID = t.OrderID and pd.OrderShipmodeSeq = t.Seq
inner join Pullout_Detail_Detail pdd with (nolock) on pd.Ukey = pdd.Pullout_DetailUKey
Outer apply (Select Qty = IIF(pd.pulloutdate <= iif(t.ShipmodeID in ('E/C', 'E/P'), t.{kpiDate}, DATEADD(day, isnull(t.OTDExtension,0), t.{kpiDate})), pdd.shipqty, 0)) rA --On Time
Outer apply (Select Qty = IIF(pd.pulloutdate >  iif(t.ShipmodeID in ('E/C', 'E/P'), t.{kpiDate}, DATEADD(day, isnull(t.OTDExtension,0), t.{kpiDate})), pdd.shipqty, 0)) rB --Fail
group by pd.OrderID, pd.OrderShipmodeSeq

select max(p.PulloutDate)PulloutDate ,pd.OrderID,pd.OrderShipmodeSeq
into #tmp_Pullout_Detail_p
from Pullout_Detail pd with (nolock)
inner join #tmp_main t on pd.OrderID = t.OrderID and pd.OrderShipmodeSeq = t.Seq
INNER JOIN Pullout p ON p.Id=pd.id AND p.PulloutDate=pd.PulloutDate 
inner join Pullout_Detail_Detail pdd with (nolock) on pd.Ukey = pdd.Pullout_DetailUKey
where pd.OrderID = t.OrderID and pd.OrderShipmodeSeq =  t.Seq 
group by pd.OrderID,pd.OrderShipmodeSeq


select SewouptQty=sum(x.QaQty),SewLastDate=format(max(SewLastDate),'yyyy/MM/dd'), OrderID
into #tmp_SewingOutput
from(
	Select OrderID, Article, SizeCode, 
		Min(QaQty) as QaQty,
		Min(SewLastDate) as SewLastDate
	From (
		Select ComboType, t.OrderID, Article, SizeCode, QaQty = Sum(SewingOutput_Detail_Detail.QaQty), Max(OutputDate) as SewLastDate
		From SewingOutput_Detail_Detail
		inner join (select distinct OrderID from #tmp_main) t on SewingOutput_Detail_Detail.OrderID= t.OrderID
		inner join SewingOutput on SewingOutput_Detail_Detail.ID = SewingOutput.ID 
		Group by ComboType, t.OrderID, Article, SizeCode
	) as sdd
	Group by OrderID, Article, SizeCode
)x
group by OrderID

select
	ID,
	CTNStartNo,
	OrderID,
	OrigID,
	OrigOrderID,
	OrigCTNStartNo,
	OrderShipmodeSeq
into #PackingList_Detail
from PackingList_Detail  pld
where exists(select 1 from #tmp_main where orderid = pld.OrderID)
and CTNQty > 0

select OrderID, OrderShipmodeSeq, AddDate = MAX(AddDate)
into #CReceive
from (
	select pd.OrderID, OrderShipmodeSeq, c.AddDate
	from #PackingList_Detail pd 
	inner join ClogReceive c WITH (NOLOCK) on pd.ID = c.PackingListID 
												and pd.OrderID = c.OrderID 
												and pd.CTNStartNo = c.CTNStartNo
	where c.PackingListID != ''
		    and c.OrderID != ''
		    and c.CTNStartNo != ''

	union all -- 找拆箱
	select OrderID = pd.OrigOrderID, OrderShipmodeSeq, c.AddDate
	from #PackingList_Detail pd 
	inner join ClogReceive c WITH (NOLOCK) on pd.OrigID = c.PackingListID
												and pd.OrigOrderID = c.OrderID
												and pd.OrigCTNStartNo = c.CTNStartNo
	where c.PackingListID != ''
		    and c.OrderID != ''
		    and c.CTNStartNo != ''
) t
where not exists (
	-- 每個紙箱必須放在 Clog（ReceiveDate 有日期）
	select 1 
	from Production.dbo.PackingList_Detail pdCheck
	where t.OrderID = pdCheck.OrderID 
			and t.OrderShipmodeSeq = pdCheck.OrderShipmodeSeq
			and pdCheck.ReceiveDate is null)
group by OrderID, OrderShipmodeSeq

SELECT  
	[orderid]= t.OrderID
	,[ordershipmodeseq]= t.Seq
	,[CTNLastReceiveDate]= format(c.AddDate, 'yyyy/MM/dd HH:mm:ss')
into #tmp_ClogReceive
from #tmp_main t
left join #CReceive c on c.OrderID = t.OrderID and c.OrderShipmodeSeq = t.Seq

Select  oqsD.ID,oqsD.Seq
        ,sum (case when dbo.GetPoPriceByArticleSize(oqsd.id,oqsD.Article,oqsD.SizeCode) > 0 then oqsD.Qty else 0 end) as FOB
	    ,sum (case when dbo.GetPoPriceByArticleSize(oqsd.id,oqsD.Article,oqsD.SizeCode) = 0 then oqsD.Qty else 0 end) as FOC
    into #getQtyBySeq
	From Order_QtyShip_Detail oqsD
	where exists(select 1 from #tmp_main where OrderID = oqsD.ID and  Seq =  oqsD.Seq)
    group by oqsD.ID,oqsD.Seq

SELECT  * 
INTO #tmp FROM 
( 
SELECT
		 t.CountryID
		,t.KPICode
		,t.FactoryID
		,t.OrderID 
		,t.StyleID
		,t.seq
		,t.BrandID
		,BuyerDelivery = convert(varchar(10),t.BuyerDelivery,111)--G
		,FtyKPI= convert(varchar(10),t.FtyKPI,111)
		,Extension = convert(varchar(10),iif(t.ShipmodeID in ('E/C', 'E/P'), t.{kpiDate}, DATEADD(day, isnull(t.OTDExtension,0), t.{kpiDate})), 111)--I
		,DeliveryByShipmode = t.ShipmodeID
		,t.OrderQty 
        ,OnTimeQty = CASE WHEN t.OnsiteSample = 1 THEN IIF(GetOnsiteSampleFail.isFail = 1 or sew.SewLastDate is null, 0, Cast(t.OrderQty as int))
                          WHEN t.GMTComplete = 'S' and isnull(p.PulloutDate, packPulloutDate.val) is null THEN Cast(0 as int) --[IST20190675] 若為短交且PullOutDate是空的,不算OnTime也不算Fail,直接給0
                          WHEN isnull(t.isDevSample,0) = 1 then iif(pd2.isFail = 1 or isnull(pd2.PulloutDate, packPulloutDate.val) is null, 0, Cast(t.OrderQty as int))
                          Else Cast(isnull(pd.Qty, isnull(packOnTimeQty.val, 0)) as int)
                     End
        ,FailQty =  CASE WHEN t.OnsiteSample = 1 THEN IIF(GetOnsiteSampleFail.isFail = 1 or sew.SewLastDate is null, Cast(t.OrderQty as int), 0)
                         WHEN t.GMTComplete = 'S' and isnull(p.PulloutDate, packPulloutDate.val) is null THEN Cast(0 as int)
                         WHEN isnull(t.isDevSample,0) = 1 then iif(pd2.isFail = 1 or isnull(pd2.PulloutDate, packPulloutDate.val) is null, Cast(t.OrderQty as int), 0)
                         --當pullout與packing都沒又抓到fail qty時，就當作全部fail
                         WHEN pd.FailQty is null and packFailQty.val is null and packOnTimeQty.val is null then Cast(t.OrderQty as int)
                         Else Cast(isnull(pd.FailQty, isnull(packFailQty.val, 0)) as int)
             End
        , CTNOnTimeQty = Case When t.GMTComplete = 'S' and ctnr.CTNLastReceiveDate is null then Cast(0 as int)
                              When GetCTNFail.isFail = 0 Then Cast(t.OrderQty as int)
                              Else Cast(0 as int)
                         End

        , CTNFailQty = Case When t.GMTComplete = 'S' and ctnr.CTNLastReceiveDate is null then Cast(0 as int)
                              When GetCTNFail.isFail = 1 Then Cast(t.OrderQty as int)
                              Else Cast(0 as int)
                         End
        ,pullOutDate = isnull(iif(isnull(t.isDevSample,0) = 1, CONVERT(char(10), pd2.PulloutDate, 20), CONVERT(char(10), p.PulloutDate, 111)), packPulloutDate.val)
		,Shipmode = t.ShipmodeID
		,P = (select count(1)from(select distinct ID,OrderID,OrderShipmodeSeq from Pullout_Detail p2 where p2.OrderID = t.OrderID and p2.ShipQty > 0)x )  --未出貨,出貨次數=0 --不論這OrderID的OrderShipmodeSeq有沒有被撈出來, 都要計算
		,t.GMTComplete 
		,t.ReasonID
		,t.ReasonName   
        ,SewLastDate = convert(varchar(10),sew.SewLastDate,111)
		,ctnr.CTNLastReceiveDate
        ,t.OutsdReason
        ,t.ReasonRemark
		,MR = dbo.getTPEPass1_ExtNo(t.MRHandle)
		,SMR = dbo.getTPEPass1_ExtNo(t.SMR)
		,POHandle = dbo.getTPEPass1_ExtNo(t.POHandle)
		,POSMR = dbo.getTPEPass1_ExtNo(t.POSMR)
		,OrderTypeID = t.OrderTypeID
		,isDevSample = iif(t.isDevSample = 1, 'Y', '')
		,sew.SewouptQty
		, getQtyBySeq.FOC
		,Order_QtyShipCount=iif(ps.ct>1,'Y','')
		,t.Alias 
		,t.MDivisionID
        ,Remark = '' -- trade Fail Detail 582行
		,t.CFAFinalInspectDate
		,t.CFAFinalInspectResult
		,t.CFA3rdInspectDate
		,t.CFA3rdInspectResult
        ,t.IDDReason
from #tmp_main t
--出貨次數--
left join #tmp_Pullout_Detail op on op.OrderID = t.OrderID and op.OrderShipmodeSeq = t.Seq 
------------
-----isDevSample=0-----
left join  #tmp_Pullout_Detail_pd pd on pd.OrderID = t.OrderID and pd.OrderShipmodeSeq = t.Seq  
left join  #tmp_Pullout_Detail_p p on p.OrderID = t.OrderID and p.OrderShipmodeSeq = t.Seq 
---------End-------
-------sew
left join #tmp_SewingOutput sew on sew.OrderId = t.OrderID
----[CTNLastReceiveDate]
left join #tmp_ClogReceive ctnr on ctnr.orderid = t.OrderID and ctnr.ordershipmodeseq = t.Seq
left join #getQtyBySeq getQtyBySeq on getQtyBySeq.ID = t.OrderID and getQtyBySeq.Seq = t.Seq
outer apply(
	select ct=count(distinct seq)
	from Order_QtyShip oq
	where oq.id = t.OrderID
)ps
-----------isDevSample=1-----------
outer apply (
    Select top 1 iif(pd.PulloutDate > iif(t.ShipmodeID in ('E/C', 'E/P'), t.FtyKPI, DATEADD(day, isnull(t.OTDExtension,0), t.FtyKPI)), 1, 0) isFail, pd.PulloutDate
    From pullout p
	inner join Pullout_Detail pd with (nolock) on p.ID  = pd.id
    where pd.OrderID = t.OrderID
    and pd.OrderShipmodeSeq = t.Seq
    order by pd.PulloutDate ASC
) pd2
----------A2B from packing-------------
outer apply (
    select  [val] = sum(pld.shipqty)
    from PackingList pl with (nolock)
    inner join PackingList_Detail pld with (nolock) on pl.id = pld.id
    where   pd.OrderID is null
            and pld.OrderID = t.OrderID
            and pld.OrderShipmodeSeq = t.Seq
            and pl.PulloutID is not null
            and pl.pulloutdate <= iif(t.ShipmodeID in ('E/C', 'E/P'), t.{kpiDate}, DATEADD(day, isnull(t.OTDExtension,0), t.{kpiDate}))
) packOnTimeQty
outer apply (
    select  [val] = sum(pld.shipqty)
    from PackingList pl with (nolock)
    inner join PackingList_Detail pld with (nolock) on pl.id = pld.id
    where   pd.OrderID is null
            and pld.OrderID = t.OrderID
            and pld.OrderShipmodeSeq = t.Seq
            and pl.PulloutID is not null
            and pl.pulloutdate > iif(t.ShipmodeID in ('E/C', 'E/P'), t.{kpiDate}, DATEADD(day, isnull(t.OTDExtension,0), t.{kpiDate}))
) packFailQty
outer apply (
    select  [val] = min(pl.PulloutDate)
    from PackingList pl with (nolock)
    inner join PackingList_Detail pld with (nolock) on pl.id = pld.id
    where   pd.OrderID is null
            and pld.OrderID = t.OrderID
            and pld.OrderShipmodeSeq = t.Seq
            and pl.PulloutID is not null
) packPulloutDate
----------------End----------------
-----------OnsiteSample=1-----------
outer apply (
    Select isFail = iif(sew.SewLastDate > t.{kpiDate}, 1, 0)
) GetOnsiteSampleFail
-----------------------------------
outer apply(
    Select isFail = iif(ctnr.CTNLastReceiveDate > t.{kpiDate}, 1, 0)
) as GetCTNFail
----------------End----------------
where t.OrderQty > 0 
-----End-------



--部分未出貨Fail的自成一行,且ShipQty給0,避免在Excel整欄加總重覆計算
UNION ALL ----------------------------------------------------
SELECT 
	CountryID = t.CountryID
    ,KPICode = t.KPICode
    ,FactoryID = t.FactoryID
    ,t.OrderID  
	,t.StyleID
    ,t.Seq 
    ,t.BrandID  
    , BuyerDelivery = convert(varchar(10),t.BuyerDelivery,111)
    , FtyKPI = convert(varchar(10),t.FtyKPI,111)
    , Extension = convert(varchar(10),iif(t.ShipmodeID in ('E/C', 'E/P'), t.{kpiDate}, DATEADD(day, isnull(t.OTDExtension,0), t.{kpiDate})), 111)
    , DeliveryByShipmode = t.ShipmodeID
    , OrderQty = 0
    , OnTimeQty =  0
    , FailQty = Cast(isnull(t.OrderQty - (pd.Qty + pd.FailQty),0) as int) --未出貨Qty
    , CTNOnTimeQty = 0
    , CTNFailQty = 0
    , pullOutDate = null
    , Shipmode = t.ShipModeID
    ,P =0 --未出貨,出貨次數=0
    ,t.GMTComplete 
    ,t.ReasonID 
    ,t.ReasonName 
    , SewLastDate = convert(varchar(10),sew.SewLastDate,111)
    ,ctnr.CTNLastReceiveDate
    ,t.OutsdReason
    ,t.ReasonRemark
    ,MR = dbo.getTPEPass1_ExtNo(t.MRHandle)
    ,SMR = dbo.getTPEPass1_ExtNo(t.SMR)
    ,POHandle = dbo.getTPEPass1_ExtNo(t.POHandle)
    ,POSMR = dbo.getTPEPass1_ExtNo(t.POSMR)
    ,t.OrderTypeID  
    ,isDevSample = iif(t.isDevSample = 1, 'Y', '')
    ,SewouptQty=sew.SewouptQty
    , getQtyBySeq.FOC
    ,Order_QtyShipCount=iif(ps.ct>1,'Y','')
    ,t.Alias
    ,t.MDivisionID
    ,Remark = '' -- trade Fail Detail 582行
	,t.CFAFinalInspectDate
	,t.CFAFinalInspectResult
	,t.CFA3rdInspectDate
	,t.CFA3rdInspectResult
    ,t.IDDReason
From #tmp_main t 
--是否有分批出貨
outer apply (
	select isPartial = iif (count(distinct oqs.Seq) > 1 ,'Y','')
	from Order_QtyShip oqs
	where oqs.Id = t.OrderID
) getPartial

-----isDevSample=0-----
left join  #tmp_Pullout_Detail_pd pd on pd.OrderID = t.OrderID and pd.OrderShipmodeSeq = t.Seq 
left join  #tmp_Pullout_Detail_p p on p.OrderID = t.OrderID and p.OrderShipmodeSeq = t.Seq 
---------End------- 
-------sew
left join #tmp_SewingOutput sew on sew.OrderId = t.OrderID
----[CTNLastReceiveDate]
left join #tmp_ClogReceive ctnr on ctnr.orderid = t.OrderID and ctnr.ordershipmodeseq = t.Seq
left join #getQtyBySeq getQtyBySeq on getQtyBySeq.ID = t.OrderID and getQtyBySeq.Seq = t.Seq
outer apply(
	select ct=count(distinct seq)
	from Order_QtyShip oq
	where oq.id = t.OrderID
)ps
-------End-------
where t.GMTComplete != 'S' 
and t.OnsiteSample = 0 -- 2020/03/27 [IST20200536]ISP20200567 onSiteSample = 0 才需要看這邊規則是否Fail
and t.OrderQty - (pd.Qty + pd.FailQty) > 0
and isnull(t.isDevSample,0) = 0 --isDevSample = 0 才需要看這邊的規則是否Fail
)a


SELECT t.*
FROM #tmp t
INNER JOIN Factory f ON t.KPICode=f.id
ORDER BY  t.OrderID, t.seq, t.KpiCode

drop table #tmp_Pullout_Detail_p,#tmp_Pullout_Detail_pd,#tmp_Pullout_Detail,#tmp_SewingOutput,#tmp_ClogReceive,#tmp,#tmp_main,#getQtyBySeq,#maxReceiveDate,#tmpReceiveDate1,#tmpReceiveDate2
";

                result = DBProxy.Current.Select(null, strSQL, null, out this.gdtOrderDetail);
                if (!result)
                {
                    return result;
                }

                if ((this.gdtOrderDetail == null) || (this.gdtOrderDetail.Rows.Count == 0))
                {
                    return new DualResult(false, "Data not found!");
                }

                #endregion Order Detail

                #region SDP
                strSQL = @" 
Select 
 [Country] = cc.CountryID
, [MDivisionID] = cc.MDivisionID
, [Factory] = cc.FactoryID
, [OrderQty] = cc.OrderQty
, cc.OnTimeQty
, cc.FailQty
, [SDP] = cast(OnTimeQty as float) / (cast(OnTimeQty as float) + cast(FailQty as float)) --[IST20190675] 調整SDP計算,OnTimeQty / (OnTimeQty + FailQty)
, isPass = iif(cast(OnTimeQty as float) / (cast(OnTimeQty as float) + cast(FailQty as float)) > 0.970, 'PASS', 'FAIL') 
, [SDP% (Clog Rec)] = cast(CTNOnTimeQty as float) / (cast(CTNOnTimeQty as float) + cast(CTNFailQty as float))
, isPassClogRec = iif(cast(CTNOnTimeQty as float) / (cast(CTNOnTimeQty as float) + cast(CTNFailQty as float)) > 0.970, 'PASS', 'FAIL') 
From (
    Select tmpSDP.CountryID
    , tmpSDP.MDivisionID
    , tmpSDP.FactoryID
    , OrderQty = sum(tmpSDP.OrderQty) 
    , OnTimeQty = sum(tmpSDP.OnTimeQty)
    , FailQty = sum(tmpSDP.FailQty)
    , CTNOnTimeQty = sum(tmpSDP.CTNOnTimeQty)
    , CTNFailQty = sum(tmpSDP.CTNFailQty)
    From #tmp tmpSDP
    Group by Rollup(tmpSDP.CountryID,tmpSDP.MDivisionID, tmpSDP.FactoryID)
) cc
where (cc.MDivisionID is not null) 
or (cc.CountryID is null and cc.FactoryID is null and cc.MDivisionID is null)
";
                result = MyUtility.Tool.ProcessWithDatatable(this.gdtOrderDetail, string.Empty, strSQL, out this.gdtSDP);
                if (!result)
                {
                    return result;
                }

                #endregion

                #region Fail Order List by SP
                strSQL = @" 
SELECT  '' AS CountryID
, '' AS KPICode
, '' AS FactoryID
, '' AS OrderID
, '' AS StyleID
, '' AS Seq
, '' AS BrandID
, '' AS BuyerDelivery
, '' AS FtyKPI
,  '' AS Extension
,  '' AS DeliveryByShipmode
,  0 AS OrderQty
,  0 AS OnTimeQty
,  0 AS FailQty
,  CTNOnTimeQty = 0
,  CTNFailQty = 0
, '' AS pullOutDate
, '' AS Shipmode
, '' AS P
, '' AS GMTComplete
, '' AS ReasonID
, '' AS ReasonName
, '' AS SewLastDate
, '' AS CTNLastReceiveDate
, OutsdReason = ''
, ReasonRemark = ''
, '' AS MR
, '' AS SMR
, '' AS POHandle 
, '' AS POSMR
, '' AS OrderTypeID
, '' AS isDevSample
, '' AS SewouptQty
, 0  AS FOC
, '' AS Order_QtyShipCount
, '' AS Alias
, '' AS MDivisionID
, '' AS Remark
, '' AS CFAFinalInspectDate
, '' AS CFAFinalInspectResult
, '' AS CFA3rdInspectDate
, '' AS CFA3rdInspectResult
, IDDReason = ''
FROM ORDERS
WHERE 1 = 0 ";
                result = DBProxy.Current.Select(null, strSQL, null, out this.gdtSP);
                if (!result)
                {
                    return result;
                }

                #endregion Fail Order List by SP

                #region get Order_QtyShip Data
                strSQL = @"
Select Order_QS.ID
, Convert(varchar, Order_QS.ShipmodeID) + '-' + Convert(varchar, Order_QS.Qty) + '(' +  convert(varchar(10),Order_QS.BuyerDelivery,111) + ')' as strData
,Order_QS.ShipmodeID 
From Order_QtyShip Order_QS, Orders o
Left Join OrderType ot on o.OrderTypeID = ot.ID and o.BrandID = ot.BrandID
Left Join Factory f On o.FactoryID = f.ID 
Where Order_QS.ID = o.ID and (ot.IsGMTMaster = 0 or o.OrderTypeID = '')  and (o.Junk is null or o.Junk = 0) 
";

                if (this.radioBulk.Checked)
                {
                    strSQL += " AND o.Category = 'B' AND f.Type = 'B'";
                }
                else if (this.radioSample.Checked)
                {
                    strSQL += " AND o.Category = 'S' AND f.Type = 'S' and isnull(o.OnSiteSample,0) <> 1";
                }
                else
                {
                    strSQL += " AND o.Category = 'G'";
                }

                if (this.dateFactoryKPIDate.Value1 != null)
                {
                    strSQL += $" AND Order_QS.{kpiDate} >= '{this.dateFactoryKPIDate.Value1.Value.ToString("yyyy-MM-dd")}' ";
                }

                if (this.dateFactoryKPIDate.Value2 != null)
                {
                    strSQL += $" AND Order_QS.{kpiDate} <= '{this.dateFactoryKPIDate.Value2.Value.ToString("yyyy-MM-dd")}' ";
                }

                if (this.txtFactory.Text != string.Empty)
                {
                    strSQL += string.Format(" AND f.KPICode = '{0}' ", this.txtFactory.Text);
                }

                if (!(result = DBProxy.Current.Select(null, strSQL, null, out DataTable dtOrder_QtyShip)))
                {
                    return result;
                }

                IDictionary<string, IList<DataRow>> dictionary_Order_QtyShipIDs = dtOrder_QtyShip.ToDictionaryList((x) => x.Val<string>("ID"));
                #endregion get Order_QtyShip Data

                #region Get TradeHis_Order Data
                strSQL = @"Select o.ID
, TH_Order.ReasonID 
, r.Name
, TH_Order.Remark 
from TradeHis_Order TH_Order, Reason r, ORDERS o 
Left Join OrderType ot on o.OrderTypeID = ot.ID and o.BrandID = ot.BrandID
Left Join Order_QtyShip Order_QS on o.ID = Order_QS.ID
Left Join Factory f ON o.FACTORYID = f.ID 
Where TH_Order.SourceID = o.ID 
AND TH_Order.HisType = 'Delivery' 
AND r.ReasonTypeID = TH_Order.ReasonTypeID 
AND r.ID = TH_Order.ReasonID and (ot.IsGMTMaster = 0 or o.OrderTypeID = '')  and (o.Junk is null or o.Junk = 0) ";
                if (this.radioBulk.Checked)
                {
                    strSQL += " AND o.Category = 'B' AND f.Type = 'B'";
                }
                else if (this.radioSample.Checked)
                {
                    strSQL += " AND o.Category = 'S' AND f.Type = 'S' and o.OnSiteSample <> 1";
                }
                else
                {
                    strSQL += " AND o.Category = 'G'";
                }

                if (this.dateFactoryKPIDate.Value1 != null)
                {
                    strSQL += $" AND Order_QS.{kpiDate} >= '{this.dateFactoryKPIDate.Value1.Value.ToString("yyyy-MM-dd")}' ";
                }

                if (this.dateFactoryKPIDate.Value2 != null)
                {
                    strSQL += $" AND Order_QS.{kpiDate} <= '{this.dateFactoryKPIDate.Value2.Value.ToString("yyyy-MM-dd")}' ";
                }

                if (this.txtFactory.Text != string.Empty)
                {
                    strSQL += string.Format(" AND f.KPICode = '{0}' ", this.txtFactory.Text);
                }

                strSQL += "  order by TH_Order.AddDate desc ";
                if (!(result = DBProxy.Current.Select(null, strSQL, null, out DataTable dtTradeHis_Order)))
                {
                    return result;
                }

                var groupQty = this.gdtOrderDetail.AsEnumerable().Select(row => new
                {
                    PoId = row.Field<string>("OrderID"),
                    OrderQty = row.Field<int>("OrderQty"),
                    PullQty = row.Field<int>("OnTimeQty"),
                    FailQty = row.Field<int>("FailQty"),
                    P = row.Field<int>("P"),
                }).GroupBy(group => new { group.PoId, group.P }).Select(g => new
                {
                    PoID = g.Key.PoId,
                    sumOrderQty = g.Sum(r => r.OrderQty),
                    sumPullQty = g.Sum(r => r.PullQty),
                    sumFailQty = g.Sum(r => r.FailQty),
                    sumP = g.Key.P, // 出貨次數, 在SQL撈取時改為OrderID去計算次數, 不論OrderShipmodeSeq有沒有被撈出來
                }).ToArray();

                IDictionary<string, IList<DataRow>> dictionary_TradeHis_OrderIDs = dtTradeHis_Order.ToDictionaryList((x) => x.Val<string>("ID"));
                #endregion

                List<string> lstSDP = new List<string>();
                List<string> lstSP = new List<string>();
                string poid = string.Empty;
                for (int intIndex = 0; intIndex < this.gdtOrderDetail.Rows.Count; intIndex++)
                {
                    DataRow drData = this.gdtOrderDetail.Rows[intIndex];
                    #region Calc Fail Order List by SP Data

                    // By SP# 明細, group by SPNO 顯示
                    if ((drData["FailQty"].ToString() != string.Empty) && (Convert.ToDecimal(drData["FailQty"].ToString()) > 0) && poid != drData["OrderID"].ToString())
                    {
                        DataRow drSP = this.gdtSP.NewRow();
                        drSP.ItemArray = drData.ItemArray;
                        poid = drData["OrderID"].ToString();
                        foreach (var i in groupQty.AsEnumerable().Where(x => x.sumFailQty > 0 && x.PoID == poid))
                        {
                            drSP["OrderQty"] = i.sumOrderQty;
                            drSP["OnTimeQty"] = i.sumPullQty;
                            drSP["FailQty"] = i.sumFailQty;
                            drSP["P"] = i.sumP;
                        }

                        this.gdtSP.Rows.Add(drSP);
                    }
                    #endregion
                }

                // shipmode
                if (this.gdtSP != null)
                {
                    IList<DataRow> lstDataRows;
                    for (int index = 0; index < this.gdtSP.Rows.Count; index++)
                    {
                        DataRow dtData = this.gdtSP.Rows[index];
                        lstDataRows = null;
                        if (dictionary_Order_QtyShipIDs.TryGetValue(dtData["OrderID"].ToString(), out lstDataRows))
                        {
                            if (lstDataRows != null)
                            {
                                string strTemp = string.Empty, strShipmodeID = string.Empty;
                                for (int index_1 = 0; index_1 < lstDataRows.Count; index_1++)
                                {
                                    strTemp += strTemp == string.Empty ? lstDataRows[index_1]["strData"].ToString() : ", " + lstDataRows[index_1]["strData"].ToString();
                                    if (strShipmodeID.IndexOf(lstDataRows[index_1]["ShipmodeID"].ToString()) < 0)
                                    {
                                        strShipmodeID += strShipmodeID == string.Empty ? lstDataRows[index_1]["ShipmodeID"].ToString() : ", " + lstDataRows[index_1]["ShipmodeID"].ToString();
                                    }
                                }

                                dtData["DeliveryByShipmode"] = strTemp;
                                dtData["Shipmode"] = strShipmodeID;
                            }
                        }
                    }
                }

                if (this.checkExportDetailData.Checked)
                {
                    #region On time Order List by PullOut
                    strSQL = @"Select tmp.Alias
                    , tmp.KpiCode
                    , tmp.FactoryID
                    , tmp.OrderID
                    , tmp.StyleID
                    , tmp.Seq
                    , tmp.FtyKPI
                    , tmp.Extension
                    , tmp.DeliveryByShipmode
                    , tmp.OrderQty
                    , tmp.OnTimeQty
                    , tmp.PullOutDate
                    , tmp.Shipmode
                    , tmp.OrderTypeID
                    , tmp.isDevSample
                    , tmp.FOC
                    , tmp.SewLastDate
                    , tmp.CTNLastReceiveDate
                    , tmp.CFAFinalInspectDate
                    , tmp.CFAFinalInspectResult
                    , tmp.CFA3rdInspectDate
                    , tmp.CFA3rdInspectResult
                    , tmp.IDDReason
                    From #tmp tmp
                    Where tmp.OnTimeQty <> 0
                    Order by tmp.OrderID, tmp.Seq";

                    result = MyUtility.Tool.ProcessWithDatatable(this.gdtOrderDetail, string.Empty, strSQL, out this.gdtPullOut);
                    if (!result)
                    {
                        return result;
                    }
                    #endregion On time Order List by PullOut
                }

                #region 顯示筆數

                this.SetCount(this.gdtOrderDetail.Rows.Count);

                if (!(result = this.TransferToExcel()))
                {
                    return result;
                }
                #endregion
            }
            catch (Exception ex)
            {
                return new DualResult(false, "data loading error.", ex.ToString());
            }

            return result;
        }

        /// <summary>
        /// transferToExcel
        /// </summary>
        /// <returns>DualResult</returns>
        private DualResult TransferToExcel()
        {
            DualResult result = Ict.Result.True;
            string temfile = string.Empty;

            if (this.checkExportDetailData.Checked)
            {
                temfile = Env.Cfg.XltPathDir + "\\Planning_R17_Detail.xltx";
            }
            else
            {
                temfile = Env.Cfg.XltPathDir + "\\Planning_R17.xltx";
            }

            Excel.Application excel = MyUtility.Excel.ConnectExcel(temfile);
            try
            {
                Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];
                excel.DisplayAlerts = false; // 關閉Excel的警告視窗是否彈出

                int intRowsCount = this.gdtSDP.Rows.Count;
                int intRowsStart = 2; // 匯入起始位置
                int mdivisionRowsStart = intRowsStart;
                int preRowsStart = intRowsStart;
                int rownum = intRowsStart; // 每筆資料匯入之位置
                string[] aryAlpha = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "AA", "AB", "AC", "AD", "AE", "AF", "AG", "AH", "AI", "AJ", "AK", "AL", "AM" };

                #region 將資料放入陣列並寫入Excel範例檔

                Dictionary<string, string> db_ExcelColumn = new Dictionary<string, string>();
                Dictionary<string, string> orderDetail_ExcelColumn = new Dictionary<string, string>();
                Dictionary<string, string> db_ExcelColumn2 = new Dictionary<string, string>();

                db_ExcelColumn.Add("A", "CountryID");
                db_ExcelColumn.Add("B", "KPICode");
                db_ExcelColumn.Add("C", "FactoryID");
                db_ExcelColumn.Add("D", "OrderID");
                db_ExcelColumn.Add("E", "StyleID");
                db_ExcelColumn.Add("F", "Seq");
                db_ExcelColumn.Add("G", "BrandID");
                db_ExcelColumn.Add("H", "BuyerDelivery");
                db_ExcelColumn.Add("I", "FtyKPI");
                db_ExcelColumn.Add("J", "Extension");
                db_ExcelColumn.Add("K", "DeliveryByShipmode");
                db_ExcelColumn.Add("L", "OrderQty");
                db_ExcelColumn.Add("M", "OnTimeQty");
                db_ExcelColumn.Add("N", "FailQty");
                db_ExcelColumn.Add("O", "CTNOnTimeQty");
                db_ExcelColumn.Add("P", "CTNFailQty");
                db_ExcelColumn.Add("Q", "pullOutDate");
                db_ExcelColumn.Add("R", "Shipmode");
                db_ExcelColumn.Add("S", "P");
                db_ExcelColumn.Add("T", "GMTComplete");
                db_ExcelColumn.Add("U", "ReasonID");
                db_ExcelColumn.Add("V", "ReasonName");
                db_ExcelColumn.Add("W", "SewLastDate");
                db_ExcelColumn.Add("X", "CTNLastReceiveDate");
                db_ExcelColumn.Add("Y", "IDDReason");
                db_ExcelColumn.Add("Z", "OutsdReason");
                db_ExcelColumn.Add("AA", "ReasonRemark");
                db_ExcelColumn.Add("AB", "MR");
                db_ExcelColumn.Add("AC", "SMR");
                db_ExcelColumn.Add("AD", "POHandle");
                db_ExcelColumn.Add("AE", "POSMR");
                db_ExcelColumn.Add("AF", "OrderTypeID");
                db_ExcelColumn.Add("AG", "isDevSample");
                db_ExcelColumn.Add("AH", "FOC");
                db_ExcelColumn.Add("AI", "CFAFinalInspectDate");
                db_ExcelColumn.Add("AJ", "CFAFinalInspectResult");
                db_ExcelColumn.Add("AK", "CFA3rdInspectDate");
                db_ExcelColumn.Add("AL", "CFA3rdInspectResult");

                orderDetail_ExcelColumn.Add("A", "CountryID");
                orderDetail_ExcelColumn.Add("B", "KPICode");
                orderDetail_ExcelColumn.Add("C", "FactoryID");
                orderDetail_ExcelColumn.Add("D", "OrderID");
                orderDetail_ExcelColumn.Add("E", "StyleID");
                orderDetail_ExcelColumn.Add("F", "Seq");
                orderDetail_ExcelColumn.Add("G", "BrandID");
                orderDetail_ExcelColumn.Add("H", "BuyerDelivery");
                orderDetail_ExcelColumn.Add("I", "FtyKPI");
                orderDetail_ExcelColumn.Add("J", "Extension");
                orderDetail_ExcelColumn.Add("K", "DeliveryByShipmode");
                orderDetail_ExcelColumn.Add("L", "OrderQty");
                orderDetail_ExcelColumn.Add("M", "OnTimeQty");
                orderDetail_ExcelColumn.Add("N", "FailQty");
                orderDetail_ExcelColumn.Add("O", "pullOutDate");
                orderDetail_ExcelColumn.Add("P", "Shipmode");
                orderDetail_ExcelColumn.Add("Q", "P");
                orderDetail_ExcelColumn.Add("R", "GMTComplete");
                orderDetail_ExcelColumn.Add("S", "ReasonID");
                orderDetail_ExcelColumn.Add("T", "ReasonName");
                orderDetail_ExcelColumn.Add("U", "MR");
                orderDetail_ExcelColumn.Add("V", "SMR");
                orderDetail_ExcelColumn.Add("W", "POHandle");
                orderDetail_ExcelColumn.Add("X", "POSMR");
                orderDetail_ExcelColumn.Add("Y", "OrderTypeID");
                orderDetail_ExcelColumn.Add("Z", "isDevSample");
                orderDetail_ExcelColumn.Add("AA", "SewouptQty");
                orderDetail_ExcelColumn.Add("AB", "FOC");
                orderDetail_ExcelColumn.Add("AC", "SewLastDate");
                orderDetail_ExcelColumn.Add("AD", "CTNLastReceiveDate");
                orderDetail_ExcelColumn.Add("AE", "IDDReason");
                orderDetail_ExcelColumn.Add("AF", "Order_QtyShipCount");
                orderDetail_ExcelColumn.Add("AG", "Alias");
                orderDetail_ExcelColumn.Add("AH", "CFAFinalInspectDate");
                orderDetail_ExcelColumn.Add("AI", "CFAFinalInspectResult");
                orderDetail_ExcelColumn.Add("AJ", "CFA3rdInspectDate");
                orderDetail_ExcelColumn.Add("AK", "CFA3rdInspectResult");

                db_ExcelColumn2.Add("A", "Alias");
                db_ExcelColumn2.Add("B", "KPICode");
                db_ExcelColumn2.Add("C", "FactoryID");
                db_ExcelColumn2.Add("D", "OrderID");
                db_ExcelColumn2.Add("E", "StyleID");
                db_ExcelColumn2.Add("F", "Seq");
                db_ExcelColumn2.Add("G", "FtyKPI");
                db_ExcelColumn2.Add("H", "Extension");
                db_ExcelColumn2.Add("I", "DeliveryByShipmode");
                db_ExcelColumn2.Add("J", "OrderQty");
                db_ExcelColumn2.Add("K", "OnTimeQty");
                db_ExcelColumn2.Add("L", "pullOutDate");
                db_ExcelColumn2.Add("M", "Shipmode");
                db_ExcelColumn2.Add("N", "OrderTypeID");
                db_ExcelColumn2.Add("O", "isDevSample");
                db_ExcelColumn2.Add("P", "FOC");
                db_ExcelColumn2.Add("Q", "SewLastDate");
                db_ExcelColumn2.Add("R", "CTNLastReceiveDate");
                db_ExcelColumn2.Add("S", "IDDReason");
                db_ExcelColumn2.Add("T", "CFAFinalInspectDate");
                db_ExcelColumn2.Add("U", "CFAFinalInspectResult");
                db_ExcelColumn2.Add("V", "CFA3rdInspectDate");
                db_ExcelColumn2.Add("W", "CFA3rdInspectResult");

                #region 匯出SDP
                List<string> mSummaryRow = new List<string>();
                for (int i = 0; i < intRowsCount; i += 1)
                {
                    DataRow dr = this.gdtSDP.Rows[i];
                    worksheet.Range[$"A{i + 2}:A{i + 2}"].Value = dr["Country"];
                    worksheet.Range[$"B{i + 2}:B{i + 2}"].Value = dr["Factory"];
                    worksheet.Range[$"C{i + 2}:C{i + 2}"].Value = dr["OrderQty"];
                    worksheet.Range[$"D{i + 2}:D{i + 2}"].Value = dr["OnTimeQty"];
                    worksheet.Range[$"E{i + 2}:E{i + 2}"].Value = dr["FailQty"];
                    worksheet.Range[$"F{i + 2}:F{i + 2}"].Value = dr["SDP"];
                    worksheet.Range[$"G{i + 2}:G{i + 2}"].Value = dr["isPass"];
                    worksheet.Range[$"H{i + 2}:H{i + 2}"].Value = dr["SDP% (Clog Rec)"];
                    worksheet.Range[$"I{i + 2}:I{i + 2}"].Value = dr["isPassClogRec"];

                    if (worksheet.Cells[i + 2, 6].Value < 0.97)
                    {
                        worksheet.Cells[i + 2, 7].Font.Color = Color.Red;
                    }

                    if (worksheet.Cells[i + 2, 8].Value < 0.97)
                    {
                        worksheet.Cells[i + 2, 9].Font.Color = Color.Red;
                    }
                }

                int cntColumn = this.gdtSDP.Columns.Count;
                int cntRow = this.gdtSDP.Rows.Count;
                string colName = string.Empty;

                worksheet.Range[$"G1:G{cntRow}"].Interior.Color = Color.FromArgb(254, 255, 146); // 底色
                worksheet.Range[$"G1:G{cntRow + 1}"].Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlContinuous;
                worksheet.Range[$"I1:I{cntRow}"].Interior.Color = Color.FromArgb(254, 255, 146); // 底色
                worksheet.Range[$"I1:I{cntRow + 1}"].Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlContinuous;
                worksheet.Range["F:F"].ColumnWidth = 9;
                worksheet.Range["H:H"].ColumnWidth = 9;
                for (int index = 2; index <= cntRow + 1; index++)
                {
                    if (MyUtility.Check.Empty((string)worksheet.Cells[index, 2].Value))
                    {
                        worksheet.Cells[index, 1].Value = string.Empty;
                        colName = MyExcelPrg.GetExcelColumnName(index);
                        worksheet.Range[$"A{index}:F{index}"].Interior.Color = Color.FromArgb(204, 255, 204); // 底色
                        worksheet.Range[$"H{index}:H{index}"].Interior.Color = Color.FromArgb(204, 255, 204); // 底色
                        worksheet.Range[$"A{index}:I{index}"].Font.Bold = true;
                        worksheet.Range[$"A{index}:I{index}"].Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
                    }
                }

                worksheet.Cells[cntRow + 1, 1].Value = "G. TTL.";
                worksheet.Range[$"A{cntRow + 1}:I{cntRow + 1}"].Interior.Color = Color.FromArgb(255, 255, 1); // 黃色底色
                worksheet.Range[$"A1:I{cntRow + 1}"].Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;
                worksheet.Range[$"A1:I{cntRow + 1}"].Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
                worksheet.Range[$"A1:I{cntRow + 1}"].Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlContinuous;
                worksheet.Range[$"A1:I{cntRow + 1}"].Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;

                // 調整後Qty與OnTimeQty + FailQty會有差異,差異的部分即是短交數
                worksheet.Cells[cntRow + 3, 1].Value = "* SDP=On time Qty / (On time Qty+Delay Qty)";
                worksheet.Range[$"A{cntRow + 3}:G{cntRow + 3}"].Merge();

                worksheet.Columns.AutoFit();
                #endregion

                #region 匯出 Fail Order List by SP Data
                if ((this.gdtSP != null) && (this.gdtSP.Rows.Count > 0))
                {
                    worksheet = excel.ActiveWorkbook.Worksheets[2];
                    worksheet.Name = "Fail Order List by SP";
                    string[] aryTitles = new string[] { "Country", "KPI Group", "Factory", "SP No", "Style", "Seq", "Brand", "Buyer Delivery", "Factory KPI", "Extension", "Delivery By Shipmode ", "Order Qty", "On Time Qty", "Fail Qty", "On Time Qty(Clog rec)", "Fail Qty(Clog rec)", "Fail PullOut Date", "ShipMode", "[P]", "Garment Complete", "ReasonID", "Order Reason", "Last Sewing Output Date", "Last Carton Received Date", "IDD Reason", "Outstanding Reason", "Reason Remark", "Handle", "SMR", "PO Handle", "PO SMR", "Order Type", "Dev. Sample", "FOC Qty", "CFA Inspection Date", "CFA final inspection result", "3rd party Inspection date", "3rd party insp. Result" };
                    object[,] objArray_1 = new object[1, aryTitles.Length];
                    for (int intIndex = 0; intIndex < aryTitles.Length; intIndex++)
                    {
                        objArray_1[0, intIndex] = aryTitles[intIndex];
                    }

                    worksheet.Range[string.Format("A{0}:{1}{0}", 1, aryAlpha[aryTitles.Length - 1])].Value2 = objArray_1;
                    worksheet.Range[string.Format("A{0}:{1}{0}", 1, aryAlpha[aryTitles.Length - 1])].AutoFilter(1);
                    worksheet.Range[string.Format("A{0}:{1}{0}", 1, aryAlpha[aryTitles.Length - 1])].Interior.Color = Color.FromArgb(204, 255, 204);
                    worksheet.Range[string.Format("A{0}:{1}{0}", 1, aryAlpha[aryTitles.Length - 1])].Borders.Color = Color.Black;

                    int rc = this.gdtSP.Rows.Count;
                    for (int intIndex = 0; intIndex < rc; intIndex++)
                    {
                        for (int intIndex_0 = 0; intIndex_0 < aryTitles.Length; intIndex_0++)
                        {
                            string excelColumnName = db_ExcelColumn[aryAlpha[intIndex_0]];
                            objArray_1[0, intIndex_0] = this.gdtSP.Rows[intIndex][excelColumnName].ToString();
                        }

                        worksheet.Range[string.Format("A{0}:{1}{0}", intIndex + 2, aryAlpha[aryTitles.Length - 1])].Value2 = objArray_1;
                    }

                    worksheet.Columns.AutoFit();
                    worksheet.Cells[rc + 2, 2] = "Total:";
                    worksheet.Cells[rc + 2, 12] = string.Format("=SUM(L2:L{0})", MyUtility.Convert.GetString(rc + 1));
                    worksheet.Cells[rc + 2, 13] = string.Format("=SUM(M2:M{0})", MyUtility.Convert.GetString(rc + 1));
                    worksheet.Cells[rc + 2, 14] = string.Format("=SUM(N2:N{0})", MyUtility.Convert.GetString(rc + 1));

                    // 設定分割列數
                    excel.ActiveWindow.SplitRow = 1;

                    // 進行凍結視窗
                    excel.ActiveWindow.FreezePanes = true;
                }
                #endregion
                if (this.checkExportDetailData.Checked)
                {
                    #region 匯出 Order Detail
                    if ((this.gdtOrderDetail != null) && (this.gdtOrderDetail.Rows.Count > 0))
                    {
                        worksheet = excel.ActiveWorkbook.Worksheets[3];
                        worksheet.Name = "Order Detail";
                        string[] aryTitles = new string[] { "Country", "KPI Group", "Factory", "SP No", "Style", "Seq", "Brand", "Buyer Delivery", "Factory KPI", "Extension", "Delivery By Shipmode", "Order Qty", "On Time Qty", "Fail Qty", "PullOut Date", "ShipMode", "[P]", "Garment Complete", "ReasonID", "Order Reason", "Handle  ", "SMR", "PO Handle", "PO SMR", "Order Type", "Dev. Sample", "Sewing Qty", "FOC Qty", "Last sewing output date", "Last Carton Received Date", "IDD Reason", "Partial shipment", "Alias", "CFA Inspection Date", "CFA final inspection result", "3rd party Inspection date", "3rd party insp. Result" };
                        object[,] objArray_1 = new object[1, aryTitles.Length];
                        for (int intIndex = 0; intIndex < aryTitles.Length; intIndex++)
                        {
                            objArray_1[0, intIndex] = aryTitles[intIndex];
                        }

                        worksheet.Range[string.Format("A{0}:{1}{0}", 1, aryAlpha[aryTitles.Length - 1])].Value2 = objArray_1;
                        worksheet.Range[string.Format("A{0}:{1}{0}", 1, aryAlpha[aryTitles.Length - 1])].AutoFilter(1); // 篩選
                        worksheet.Range[string.Format("A{0}:{1}{0}", 1, aryAlpha[aryTitles.Length - 1])].Interior.Color = Color.FromArgb(204, 255, 204);
                        worksheet.Range[string.Format("A{0}:{1}{0}", 1, aryAlpha[aryTitles.Length - 1])].Borders.Color = Color.Black;
                        int rc = this.gdtOrderDetail.Rows.Count;
                        for (int intIndex = 0; intIndex < rc; intIndex++)
                        {
                            for (int intIndex_0 = 0; intIndex_0 < aryTitles.Length; intIndex_0++)
                            {
                                string excelColumnName = orderDetail_ExcelColumn[aryAlpha[intIndex_0]];
                                objArray_1[0, intIndex_0] = this.gdtOrderDetail.Rows[intIndex][excelColumnName].ToString();
                            }

                            worksheet.Range[string.Format("A{0}:{1}{0}", intIndex + 2, aryAlpha[aryTitles.Length - 1])].Value2 = objArray_1;

                            worksheet.Range[string.Format("G{0}:H{0}", intIndex + 2)].NumberFormatLocal = "yyyy/MM/dd";
                        }

                        worksheet.Columns.AutoFit();
                        worksheet.Cells[rc + 2, 2] = "Total:";
                        worksheet.Cells[rc + 2, 12] = string.Format("=SUM(L2:L{0})", MyUtility.Convert.GetString(rc + 1));
                        worksheet.Cells[rc + 2, 13] = string.Format("=SUM(M2:M{0})", MyUtility.Convert.GetString(rc + 1));
                        worksheet.Cells[rc + 2, 14] = string.Format("=SUM(N2:N{0})", MyUtility.Convert.GetString(rc + 1));

                        // 設定分割列數
                        excel.ActiveWindow.SplitRow = 1;

                        // 進行凍結視窗
                        excel.ActiveWindow.FreezePanes = true;
                    }
                    #endregion

                    #region 匯出 On time Order List by PullOut
                    if ((this.gdtPullOut != null) && (this.gdtPullOut.Rows.Count > 0))
                    {
                        worksheet = excel.ActiveWorkbook.Worksheets[4];
                        worksheet.Name = "On time Order List by PullOut";
                        string[] aryTitles = new string[] { "Country", "KPI Group", "Factory", "SP No", "Style", "Seq", "Factory KPI", "Extension", "Delivery By Shipmode", "Order Qty", "PullOut Qty", "PullOut Date", "ShipMode", "Order Type", "Dev. Sample", "FOC Qty", "Last Sewing Output Date", "Last Carton Received Date", "IDD Reason", "CFA Inspection Date", "CFA final inspection result", "3rd party Inspection date", "3rd party insp. Result" };
                        object[,] objArray_1 = new object[1, aryTitles.Length];
                        for (int intIndex = 0; intIndex < aryTitles.Length; intIndex++)
                        {
                            objArray_1[0, intIndex] = aryTitles[intIndex];
                        }

                        worksheet.Range[string.Format("A{0}:{1}{0}", 1, aryAlpha[aryTitles.Length - 1])].Value2 = objArray_1;
                        worksheet.Range[string.Format("A{0}:{1}{0}", 1, aryAlpha[aryTitles.Length - 1])].AutoFilter(1); // 篩選
                        worksheet.Range[string.Format("A{0}:{1}{0}", 1, aryAlpha[aryTitles.Length - 1])].Interior.Color = Color.FromArgb(204, 255, 204);
                        worksheet.Range[string.Format("A{0}:{1}{0}", 1, aryAlpha[aryTitles.Length - 1])].Borders.Color = Color.Black;

                        int rc = this.gdtPullOut.Rows.Count;
                        for (int intIndex = 0; intIndex < rc; intIndex++)
                        {
                            for (int intIndex_0 = 0; intIndex_0 < aryTitles.Length; intIndex_0++)
                            {
                                string excelColumnName = db_ExcelColumn2[aryAlpha[intIndex_0]];
                                objArray_1[0, intIndex_0] = this.gdtPullOut.Rows[intIndex][excelColumnName].ToString();
                            }

                            worksheet.Range[string.Format("A{0}:{1}{0}", intIndex + 2, aryAlpha[aryTitles.Length - 1])].Value2 = objArray_1;
                        }

                        worksheet.Columns.AutoFit();
                        worksheet.Cells[rc + 2, 2] = "Total:";
                        worksheet.Cells[rc + 2, 10] = string.Format("=SUM(J2:J{0})", MyUtility.Convert.GetString(rc + 1));
                        worksheet.Cells[rc + 2, 11] = string.Format("=SUM(K2:K{0})", MyUtility.Convert.GetString(rc + 1));

                        // 設定分割列數
                        excel.ActiveWindow.SplitRow = 1;

                        // 進行凍結視窗
                        excel.ActiveWindow.FreezePanes = true;
                    }
                    #endregion

                    #region 匯出 Fail Detail
                    var gdtFailDetail = from data in this.gdtOrderDetail.AsEnumerable()
                                        where data.Field<int>("FailQty") > 0 && !(data.Field<string>("GMTComplete") == "S" && data["PullOutDate"] == DBNull.Value)
                                        select new
                                        {
                                            CountryID = data.Field<string>("CountryID"),
                                            KPICode = data.Field<string>("KPICode"),
                                            FactoryID = data.Field<string>("FactoryID"),
                                            OrderID = data.Field<string>("OrderID"),
                                            Style = data.Field<string>("StyleID"),
                                            Seq = data.Field<string>("Seq"),
                                            BrandID = data.Field<string>("BrandID"),
                                            FtyKPI = data.Field<string>("FtyKPI"),
                                            Extension = data.Field<string>("Extension"),
                                            DeliveryByShipmode = data.Field<string>("DeliveryByShipmode"),
                                            OrderQty = data.Field<int>("OrderQty").ToString(),
                                            FailQty = data.Field<int>("FailQty").ToString(),
                                            pullOutDate = data.Field<string>("pullOutDate"),
                                            Shipmode = data.Field<string>("Shipmode"),
                                            ReasonID = data.Field<string>("ReasonID"),
                                            ReasonName = data.Field<string>("ReasonName"),
                                            MR = data.Field<string>("MR"),
                                            Remark = data.Field<string>("Remark"),
                                            OrderTypeID = data.Field<string>("OrderTypeID"),
                                            isDevSample = data.Field<string>("isDevSample"),
                                            FOC = data.Field<int>("FOC"),
                                            SewLastDate = data.Field<string>("SewLastDate"),
                                            CTNLastReceiveDate = data.Field<string>("CTNLastReceiveDate"),
                                            IDDReason = data.Field<string>("IDDReason"),
                                            CFAFinalInspectDate = data.Field<string>("CFAFinalInspectDate"),
                                            CFAFinalInspectResult = data.Field<string>("CFAFinalInspectResult"),
                                            CFA3rdInspectDate = data.Field<string>("CFA3rdInspectDate"),
                                            CFA3rdInspectResult = data.Field<string>("CFA3rdInspectResult"),
                                        };
                    if ((gdtFailDetail != null) && (gdtFailDetail.Count() > 0))
                    {
                        worksheet = excel.ActiveWorkbook.Worksheets[5];
                        worksheet.Name = "Fail Detail";
                        string[] aryTitles = new string[] { "Country", "KPI Group", "Factory", "SP No", "Style", "Seq", "Brand", "Factory KPI", "Extension", "Delivery By Shipmode", "Order Qty", "Fail Qty", "PullOut Date", "ShipMode", "ReasonID", "Order Reason", "Handle", "Remark", "Order Type", "Dev. Sample", "FOC Qty", "Last Sewing Output Date", "Last Carton Received Date", "IDD Reason", "CFA Inspection Date", "CFA final inspection result", "3rd party Inspection date", "3rd party insp. Result" };
                        object[,] objArray_1 = new object[1, aryTitles.Length];
                        for (int intIndex = 0; intIndex < aryTitles.Length; intIndex++)
                        {
                            objArray_1[0, intIndex] = aryTitles[intIndex];
                        }

                        worksheet.Range[string.Format("A{0}:{1}{0}", 1, aryAlpha[aryTitles.Length - 1])].Value2 = objArray_1;
                        worksheet.Range[string.Format("A{0}:{1}{0}", 1, aryAlpha[aryTitles.Length - 1])].AutoFilter(1); // 篩選
                        worksheet.Range[string.Format("A{0}:{1}{0}", 1, aryAlpha[aryTitles.Length - 1])].Interior.Color = Color.FromArgb(204, 255, 204);
                        worksheet.Range[string.Format("A{0}:{1}{0}", 1, aryAlpha[aryTitles.Length - 1])].Borders.Color = Color.Black;

                        int rc = gdtFailDetail.Count();
                        int i = 1;
                        foreach (var dr in gdtFailDetail)
                        {
                            i++;
                            objArray_1[0, 0] = dr.CountryID;
                            objArray_1[0, 1] = dr.KPICode;
                            objArray_1[0, 2] = dr.FactoryID;
                            objArray_1[0, 3] = dr.OrderID;
                            objArray_1[0, 4] = dr.Style;
                            objArray_1[0, 5] = dr.Seq;
                            objArray_1[0, 6] = dr.BrandID;
                            objArray_1[0, 7] = dr.FtyKPI;
                            objArray_1[0, 8] = dr.Extension;
                            objArray_1[0, 9] = dr.DeliveryByShipmode;
                            objArray_1[0, 10] = dr.OrderQty;
                            objArray_1[0, 11] = dr.FailQty;
                            objArray_1[0, 12] = dr.pullOutDate;
                            objArray_1[0, 13] = dr.Shipmode;
                            objArray_1[0, 14] = dr.ReasonID;
                            objArray_1[0, 15] = dr.ReasonName;
                            objArray_1[0, 16] = dr.MR;
                            objArray_1[0, 17] = dr.Remark;
                            objArray_1[0, 18] = dr.OrderTypeID;
                            objArray_1[0, 19] = dr.isDevSample;
                            objArray_1[0, 20] = dr.FOC;
                            objArray_1[0, 21] = dr.SewLastDate;
                            objArray_1[0, 22] = dr.CTNLastReceiveDate;
                            objArray_1[0, 23] = dr.IDDReason;
                            objArray_1[0, 24] = dr.CFAFinalInspectDate;
                            objArray_1[0, 25] = dr.CFAFinalInspectResult;
                            objArray_1[0, 26] = dr.CFA3rdInspectDate;
                            objArray_1[0, 27] = dr.CFA3rdInspectResult;
                            worksheet.Range[string.Format("A{0}:{1}{0}", i, aryAlpha[aryTitles.Length - 1])].Value2 = objArray_1;
                        }

                        worksheet.Columns.AutoFit();
                        worksheet.Cells[rc + 2, 2] = "Total:";
                        worksheet.Cells[rc + 2, 11] = string.Format("=SUM(K2:K{0})", MyUtility.Convert.GetString(rc + 1));

                        worksheet.Cells[rc + 2, 12] = string.Format("=SUM(L2:L{0})", MyUtility.Convert.GetString(rc + 1));

                        // 設定分割列數
                        excel.ActiveWindow.SplitRow = 1;

                        // 進行凍結視窗
                        excel.ActiveWindow.FreezePanes = true;
                    }
                    #endregion
                }
                #endregion

                #region Save & Show Excel
                string strExcelName = MicrosoftFile.GetName("Planning_R17");
                Excel.Workbook workbook = excel.ActiveWorkbook;
                workbook.SaveAs(strExcelName);
                workbook.Close();
                excel.Quit();
                Marshal.ReleaseComObject(excel);
                Marshal.ReleaseComObject(worksheet);
                Marshal.ReleaseComObject(workbook);

                strExcelName.OpenFile();
                #endregion
                return Ict.Result.True;
            }
            catch (Exception ex)
            {
                if (excel != null)
                {
                    excel.Quit();
                }

                return new DualResult(false, "Export excel error.", ex.ToString());
            }
        }
    }
}