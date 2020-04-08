using Ict;
using Sci.Data;
using Sci.Production.Class;
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
    public partial class R17 : Sci.Win.Tems.PrintForm
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
            this.txtFactory.Text = Sci.Env.User.Factory;
            this.dateFactoryKPIDate.Select();
        }

        /// <summary>
        /// OnToExcel
        /// </summary>
        /// <param name="report">report</param>
        /// <returns>bool</returns>
        protected override bool OnToExcel(Win.ReportDefinition report)
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
where o.Junk = 0  
and (isnull(ot.IsGMTMaster,0) = 0 or o.OrderTypeID = '') 
and o.LocalOrder <> 1
and o.IsForecast <> 1
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
Outer apply (Select Qty = IIF(pd.pulloutdate <= iif(t.ShipmodeID in ('A/C', 'A/P', 'E/C', 'E/P'), t.{kpiDate}, DATEADD(day, isnull(t.OTDExtension,0), t.{kpiDate})), pdd.shipqty, 0)) rA --On Time
Outer apply (Select Qty = IIF(pd.pulloutdate >  iif(t.ShipmodeID in ('A/C', 'A/P', 'E/C', 'E/P'), t.{kpiDate}, DATEADD(day, isnull(t.OTDExtension,0), t.{kpiDate})), pdd.shipqty, 0)) rB --Fail
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
	 t.OrderID
	,t.Seq
	,ReceiveDate=max(cr.AddDate)
into #tmpReceiveDate1
from #tmp_main t
inner join PackingList_Detail pd on pd.OrderID = t.OrderID and pd.OrderShipmodeSeq = t.Seq
inner join ClogReceive cr on (cr.PackingListID = pd.ID and cr.OrderID = pd.OrderID and cr.CTNStartNo = pd.CTNStartNo and pd.SCICtnNo <>'')
where pd.OrderID = t.OrderID
and pd.OrderShipmodeSeq = t.Seq
and not exists(select 1 
			from PackingList_Detail pdCheck
			where pdCheck.OrderID =t.OrderID
					and pdCheck.OrderShipmodeSeq = t.Seq
					and CTNQty > 0
					and pdCheck.ReceiveDate is null
		)
group by t.OrderID,t.Seq

select
	 t.OrderID
	,t.Seq
	,ReceiveDate=max(cr.AddDate)
into #tmpReceiveDate2
from #tmp_main t
inner join PackingList_Detail pd on pd.OrderID = t.OrderID and pd.OrderShipmodeSeq = t.Seq
inner join ClogReceive cr on cr.SCICtnNo = pd.SCICtnNo and pd.SCICtnNo <>''
where pd.OrderID = t.OrderID
and pd.OrderShipmodeSeq = t.Seq
and not exists(select 1 
			from PackingList_Detail pdCheck
			where pdCheck.OrderID =t.OrderID
					and pdCheck.OrderShipmodeSeq = t.Seq
					and CTNQty > 0
					and pdCheck.ReceiveDate is null
		)
group by t.OrderID,t.Seq

select  t.OrderID,t.Seq,ReceiveDate=max(t.ReceiveDate)
into #maxReceiveDate
from(
	select *from #tmpReceiveDate1
	union all
	select *from #tmpReceiveDate2
)t
group by t.OrderID,t.Seq

SELECT  
	[orderid]= t.OrderID
	,[ordershipmodeseq]= t.Seq
	,[CTNLastReceiveDate]= format(r.ReceiveDate, 'yyyy/MM/dd HH:mm:ss')
into #tmp_ClogReceive
from #tmp_main t
left join #maxReceiveDate r on t.OrderID = r.orderid and t.Seq = r.Seq

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
		,Extension = convert(varchar(10),iif(t.ShipmodeID in ('A/C', 'A/P', 'E/C', 'E/P'), t.{kpiDate}, DATEADD(day, isnull(t.OTDExtension,0), t.{kpiDate})), 111)--I
		,DeliveryByShipmode = t.ShipmodeID
		,t.OrderQty 
        ,OnTimeQty = CASE WHEN t.OnsiteSample = 1 THEN IIF(GetOnsiteSampleFail.isFail = 1 or sew.SewLastDate is null, 0, Cast(t.OrderQty as int))
                          WHEN t.GMTComplete = 'S' and p.PulloutDate is null THEN Cast(0 as int) --[IST20190675] 若為短交且PullOutDate是空的,不算OnTime也不算Fail,直接給0
                          Else iif(isnull(t.isDevSample,0) = 1, iif(pd2.isFail = 1 or pd2.PulloutDate is null, 0, Cast(t.OrderQty as int)), Cast(isnull(pd.Qty,0) as int)) 
                     End
        ,FailQty =  CASE WHEN t.OnsiteSample = 1 THEN IIF(GetOnsiteSampleFail.isFail = 1 or sew.SewLastDate is null, Cast(t.OrderQty as int), 0)
                         WHEN t.GMTComplete = 'S' and p.PulloutDate is null THEN Cast(0 as int)
                         Else iif(isnull(t.isDevSample,0) = 1, iif(pd2.isFail = 1 or pd2.PulloutDate is null, Cast(t.OrderQty as int), 0), Cast(isnull(pd.FailQty,t.OrderQty) as int)) 
             End
        ,pullOutDate = iif(isnull(t.isDevSample,0) = 1, CONVERT(char(10), pd2.PulloutDate, 20), CONVERT(char(10), p.PulloutDate, 111))
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
    Select top 1 iif(pd.PulloutDate > iif(t.ShipmodeID in ('A/C', 'A/P', 'E/C', 'E/P'), t.FtyKPI, DATEADD(day, isnull(t.OTDExtension,0), t.FtyKPI)), 1, 0) isFail, pd.PulloutDate
    From pullout p
	inner join Pullout_Detail pd with (nolock) on p.ID  = pd.id
    where pd.OrderID = t.OrderID
    and pd.OrderShipmodeSeq = t.Seq
    order by pd.PulloutDate ASC
) pd2
----------------End----------------
-----------OnsiteSample=1-----------
outer apply (
    Select isFail = iif(sew.SewLastDate > t.{kpiDate}, 1, 0)
) GetOnsiteSampleFail
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
    , Extension = convert(varchar(10),iif(t.ShipmodeID in ('A/C', 'A/P', 'E/C', 'E/P'), t.{kpiDate}, DATEADD(day, isnull(t.OTDExtension,0), t.{kpiDate})), 111)
    , DeliveryByShipmode = t.ShipmodeID
    , OrderQty = 0
    , OnTimeQty =  0
    , FailQty = Cast(isnull(t.OrderQty - (pd.Qty + pd.FailQty),0) as int) --未出貨Qty
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
                strSQL = @" SELECT  '' AS Country ,  '' AS Factory, 0 AS OrderQty, 0 AS OnTimeQty, 0 AS FailQty, 0.00 AS SDP, '' AS MDivisionID FROM Orders WHERE 1 = 0 ";
                result = DBProxy.Current.Select(null, strSQL, null, out this.gdtSDP);
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
, '' AS pullOutDate
, '' AS Shipmode
, '' AS P
, '' AS GMTComplete
, '' AS ReasonID
, '' AS ReasonName
, '' AS SewLastDate
, '' AS CTNLastReceiveDate
,OutsdReason = ''
,ReasonRemark = ''
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
FROM ORDERS
WHERE 1 = 0 ";
                result = DBProxy.Current.Select(null, strSQL, null, out this.gdtSP);
                if (!result)
                {
                    return result;
                }

                #endregion Fail Order List by SP

                #region get Order_QtyShip Data
                System.Data.DataTable dtOrder_QtyShip;
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

                if (!(result = DBProxy.Current.Select(null, strSQL, null, out dtOrder_QtyShip)))
                {
                    return result;
                }

                IDictionary<string, IList<DataRow>> dictionary_Order_QtyShipIDs = dtOrder_QtyShip.ToDictionaryList((x) => x.Val<string>("ID"));
                #endregion get Order_QtyShip Data

                #region Get TradeHis_Order Data
                System.Data.DataTable dtTradeHis_Order;
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
                if (!(result = DBProxy.Current.Select(null, strSQL, null, out dtTradeHis_Order)))
                {
                    return result;
                }

                var groupQty = this.gdtOrderDetail.AsEnumerable().Select(row => new
                {
                    PoId = row.Field<string>("OrderID"),
                    OrderQty = row.Field<int>("OrderQty"),
                    PullQty = row.Field<int>("OnTimeQty"),
                    FailQty = row.Field<int>("FailQty"),
                    P = row.Field<int>("P")
                }).GroupBy(group => new { group.PoId, group.P }).Select(g => new
                {
                    PoID = g.Key.PoId,
                    sumOrderQty = g.Sum(r => r.OrderQty),
                    sumPullQty = g.Sum(r => r.PullQty),
                    sumFailQty = g.Sum(r => r.FailQty),
                    sumP = g.Key.P // 出貨次數, 在SQL撈取時改為OrderID去計算次數, 不論OrderShipmodeSeq有沒有被撈出來
                }).ToArray();

                IDictionary<string, IList<DataRow>> dictionary_TradeHis_OrderIDs = dtTradeHis_Order.ToDictionaryList((x) => x.Val<string>("ID"));
                #endregion

                List<string> lstSDP = new List<string>();
                List<string> lstSP = new List<string>();
                string poid = string.Empty;
                for (int intIndex = 0; intIndex < this.gdtOrderDetail.Rows.Count; intIndex++)
                {
                    DataRow drData = this.gdtOrderDetail.Rows[intIndex];

                    #region Calc SDP Data
                    int intIndex_SDP = lstSDP.IndexOf(drData["KPICode"].ToString() + "___" + drData["Alias"].ToString()); // A
                    DataRow drSDP;
                    if (intIndex_SDP < 0)
                    {
                        drSDP = this.gdtSDP.NewRow();
                        drSDP["Country"] = drData["KPICode"].ToString();
                        drSDP["Factory"] = drData["Alias"].ToString(); // A
                        drSDP["MDivisionID"] = drData["MDivisionID"].ToString(); // A
                        this.gdtSDP.Rows.Add(drSDP);
                        lstSDP.Add(drData["KPICode"].ToString() + "___" + drData["Alias"].ToString()); // A
                    }
                    else
                    {
                        drSDP = this.gdtSDP.Rows[intIndex_SDP];
                    }

                    drSDP["OrderQty"] = (drSDP["OrderQty"].ToString() != string.Empty ? Convert.ToDecimal(drSDP["OrderQty"].ToString()) : 0) + (drData["OrderQty"].ToString() != string.Empty ? Convert.ToDecimal(drData["OrderQty"].ToString()) : 0);
                    drSDP["OnTimeQty"] = (drSDP["OnTimeQty"].ToString() != string.Empty ? Convert.ToDecimal(drSDP["OnTimeQty"].ToString()) : 0) + (drData["OnTimeQty"].ToString() != string.Empty ? Convert.ToDecimal(drData["OnTimeQty"].ToString()) : 0);
                    drSDP["FailQty"] = (drSDP["FailQty"].ToString() != string.Empty ? Convert.ToDecimal(drSDP["FailQty"].ToString()) : 0) + (drData["FailQty"].ToString() != string.Empty ? Convert.ToDecimal(drData["FailQty"].ToString()) : 0);

                    // SDP(%)
                    // drSDP["SDP"] = drSDP["OrderQty"].ToString() == "0" ? 0 : Convert.ToDecimal(drSDP["OnTimeQty"].ToString()) / Convert.ToDecimal(drSDP["OrderQty"].ToString()) * 100;
                    drSDP["SDP"] = (Convert.ToDecimal(drSDP["OnTimeQty"].ToString()) + Convert.ToDecimal(drSDP["FailQty"].ToString())) == 0 ?
                        0 : Convert.ToDecimal(drSDP["OnTimeQty"].ToString()) / (Convert.ToDecimal(drSDP["OnTimeQty"].ToString()) + Convert.ToDecimal(drSDP["FailQty"].ToString()));

                    #endregion Calc SDP Data

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
            DualResult result = Result.True;
            string temfile = string.Empty;

            if (this.checkExportDetailData.Checked)
            {
                temfile = Sci.Env.Cfg.XltPathDir + "\\Planning_R17_Detail.xltx";
            }
            else
            {
                temfile = Sci.Env.Cfg.XltPathDir + "\\Planning_R17.xltx";
            }

            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(temfile);
            try
            {
                Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

                // order by M
                this.gdtSDP = this.gdtSDP.AsEnumerable().OrderBy(s => s["MDivisionID"]).CopyToDataTable();

                int intRowsCount = this.gdtSDP.Rows.Count;
                int intRowsStart = 2; // 匯入起始位置
                int mdivisionRowsStart = intRowsStart;
                int preRowsStart = intRowsStart;
                int rownum = intRowsStart; // 每筆資料匯入之位置
                int intColumns = 7; // 匯入欄位數
                string[] aryAlpha = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "AA", "AB", "AC", "AD", "AE", "AF", "AG", "AH" };
                object[,] objArray = new object[1, intColumns]; // 每列匯入欄位區間
                #region 將資料放入陣列並寫入Excel範例檔

                Dictionary<string, string> Db_ExcelColumn = new Dictionary<string, string>();
                Dictionary<string, string> OrderDetail_ExcelColumn = new Dictionary<string, string>();
                Dictionary<string, string> Db_ExcelColumn2 = new Dictionary<string, string>();

                Db_ExcelColumn.Add("A", "CountryID");
                Db_ExcelColumn.Add("B", "KPICode");
                Db_ExcelColumn.Add("C", "FactoryID");
                Db_ExcelColumn.Add("D", "OrderID");
                Db_ExcelColumn.Add("E", "StyleID");
                Db_ExcelColumn.Add("F", "Seq");
                Db_ExcelColumn.Add("G", "BrandID");
                Db_ExcelColumn.Add("H", "BuyerDelivery");
                Db_ExcelColumn.Add("I", "FtyKPI");
                Db_ExcelColumn.Add("J", "Extension");
                Db_ExcelColumn.Add("K", "DeliveryByShipmode");
                Db_ExcelColumn.Add("L", "OrderQty");
                Db_ExcelColumn.Add("M", "OnTimeQty");
                Db_ExcelColumn.Add("N", "FailQty");
                Db_ExcelColumn.Add("O", "pullOutDate");
                Db_ExcelColumn.Add("P", "Shipmode");
                Db_ExcelColumn.Add("Q", "P");
                Db_ExcelColumn.Add("R", "GMTComplete");
                Db_ExcelColumn.Add("S", "ReasonID");
                Db_ExcelColumn.Add("T", "ReasonName");
                Db_ExcelColumn.Add("U", "SewLastDate");
                Db_ExcelColumn.Add("V", "CTNLastReceiveDate");
                Db_ExcelColumn.Add("W", "OutsdReason");
                Db_ExcelColumn.Add("X", "ReasonRemark");
                Db_ExcelColumn.Add("Y", "MR");
                Db_ExcelColumn.Add("Z", "SMR");
                Db_ExcelColumn.Add("AA", "POHandle");
                Db_ExcelColumn.Add("AB", "POSMR");
                Db_ExcelColumn.Add("AC", "OrderTypeID");
                Db_ExcelColumn.Add("AD", "isDevSample");
                Db_ExcelColumn.Add("AE", "FOC");

                OrderDetail_ExcelColumn.Add("A", "CountryID");
                OrderDetail_ExcelColumn.Add("B", "KPICode");
                OrderDetail_ExcelColumn.Add("C", "FactoryID");
                OrderDetail_ExcelColumn.Add("D", "OrderID");
                OrderDetail_ExcelColumn.Add("E", "StyleID");
                OrderDetail_ExcelColumn.Add("F", "Seq");
                OrderDetail_ExcelColumn.Add("G", "BrandID");
                OrderDetail_ExcelColumn.Add("H", "BuyerDelivery");
                OrderDetail_ExcelColumn.Add("I", "FtyKPI");
                OrderDetail_ExcelColumn.Add("J", "Extension");
                OrderDetail_ExcelColumn.Add("K", "DeliveryByShipmode");
                OrderDetail_ExcelColumn.Add("L", "OrderQty");
                OrderDetail_ExcelColumn.Add("M", "OnTimeQty");
                OrderDetail_ExcelColumn.Add("N", "FailQty");
                OrderDetail_ExcelColumn.Add("O", "pullOutDate");
                OrderDetail_ExcelColumn.Add("P", "Shipmode");
                OrderDetail_ExcelColumn.Add("Q", "P");
                OrderDetail_ExcelColumn.Add("R", "GMTComplete");
                OrderDetail_ExcelColumn.Add("S", "ReasonID");
                OrderDetail_ExcelColumn.Add("T", "ReasonName");
                OrderDetail_ExcelColumn.Add("U", "MR");
                OrderDetail_ExcelColumn.Add("V", "SMR");
                OrderDetail_ExcelColumn.Add("W", "POHandle");
                OrderDetail_ExcelColumn.Add("X", "POSMR");
                OrderDetail_ExcelColumn.Add("Y", "OrderTypeID");
                OrderDetail_ExcelColumn.Add("Z", "isDevSample");
                OrderDetail_ExcelColumn.Add("AA", "SewouptQty");
                OrderDetail_ExcelColumn.Add("AB", "FOC");
                OrderDetail_ExcelColumn.Add("AC", "SewLastDate");
                OrderDetail_ExcelColumn.Add("AD", "CTNLastReceiveDate");
                OrderDetail_ExcelColumn.Add("AE", "Order_QtyShipCount");
                OrderDetail_ExcelColumn.Add("AF", "Alias");

                Db_ExcelColumn2.Add("A", "Alias");
                Db_ExcelColumn2.Add("B", "KPICode");
                Db_ExcelColumn2.Add("C", "FactoryID");
                Db_ExcelColumn2.Add("D", "OrderID");
                Db_ExcelColumn2.Add("E", "StyleID");
                Db_ExcelColumn2.Add("F", "Seq");
                Db_ExcelColumn2.Add("G", "FtyKPI");
                Db_ExcelColumn2.Add("H", "Extension");
                Db_ExcelColumn2.Add("I", "DeliveryByShipmode");
                Db_ExcelColumn2.Add("J", "OrderQty");
                Db_ExcelColumn2.Add("K", "OnTimeQty");
                Db_ExcelColumn2.Add("L", "pullOutDate");
                Db_ExcelColumn2.Add("M", "Shipmode");
                Db_ExcelColumn2.Add("N", "OrderTypeID");
                Db_ExcelColumn2.Add("O", "isDevSample");
                Db_ExcelColumn2.Add("P", "FOC");
                Db_ExcelColumn2.Add("Q", "SewLastDate");
                Db_ExcelColumn2.Add("R", "CTNLastReceiveDate");

                #region 匯出SDP
                List<string> MSummaryRow = new List<string>();
                for (int i = 0; i < intRowsCount; i += 1)
                {
                    DataRow dr = this.gdtSDP.Rows[i];
                    for (int k = 0; k < intColumns; k++)
                    {
                        objArray[0, k] = string.Empty;
                    }

                    objArray[0, 0] = dr["Factory"];
                    objArray[0, 1] = dr["Country"];
                    objArray[0, 2] = dr["OrderQty"];
                    objArray[0, 3] = dr["OnTimeQty"];
                    objArray[0, 4] = dr["FailQty"];
                    objArray[0, 5] = dr["SDP"];
                    objArray[0, 6] = MyUtility.Convert.GetDouble(dr["SDP"]) >= 0.97 ? "PASS" : "FAIL";

                    worksheet.Range[string.Format("A{0}:G{0}", rownum + i)].Value2 = objArray;

                    // insert by Mdivision Summary data
                    string nextMdisvision = string.Empty;
                    if ((i + 1) < intRowsCount)
                    {
                        nextMdisvision = (string)this.gdtSDP.Rows[i + 1]["MdivisionID"];
                    }

                    // 一個M的資料寫完，開始加總綠色那一列
                    if ((string)dr["MdivisionID"] != nextMdisvision)
                    {
                        objArray[0, 0] = string.Empty;
                        objArray[0, 1] = string.Empty;
                        objArray[0, 2] = $"=SUM(C{mdivisionRowsStart}:C{rownum + i})";
                        objArray[0, 3] = $"=SUM(D{mdivisionRowsStart}:D{rownum + i})";
                        objArray[0, 4] = $"=SUM(E{mdivisionRowsStart}:E{rownum + i})";

                        // 綠色列的位置記下來，最後在黃色列的公式寫入
                        MSummaryRow.Add((rownum + i + 1).ToString());

                        rownum++;

                        // objArray[0, 5] = "=" + string.Format("D{0}/IF(C{0}=0, 1,C{0})*100", rownum + i);
                        objArray[0, 5] = "=" + string.Format("D{0}/IF(D{0}+E{0}=0, 1,D{0}+E{0})", rownum + i);

                        objArray[0, 6] = MyUtility.Convert.GetDouble(dr["SDP"]) >= 0.97 ? "PASS" : "FAIL";
                        worksheet.Range[string.Format("A{0}:G{0}", rownum + i)].Value2 = objArray;
                        worksheet.Range[string.Format("A{0}:G{0}", rownum + i)].Interior.Color = Color.FromArgb(204, 255, 204);
                        worksheet.Range[string.Format("A{0}:G{0}", rownum + i)].Font.Bold = true;
                        worksheet.Range[string.Format("A{0}:G{0}", rownum + i)].Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = 1;
                        mdivisionRowsStart = rownum + i + 1;
                    }
                }

                // 黃色那一列的大總結
                if (intRowsCount > 0)
                {
                    worksheet.Range[string.Format("A{0}:A{0}", rownum + intRowsCount)].Value2 = "G. TTL.";
                    worksheet.Range[string.Format("A{0}:A{0}", rownum + intRowsCount + 2)].Value2 = "* SDP=On time Qty / (On time Qty+Delay Qty)";
                    worksheet.Range[string.Format("A{0}:G{0}", rownum + intRowsCount + 2)].Merge(false);
                    worksheet.Range[string.Format("C{0}:C{0}", rownum + intRowsCount)].Formula = "=SUM(" + string.Format("C{0}:C{1}", 2, rownum + intRowsCount - 1) + ")";
                    worksheet.Range[string.Format("D{0}:D{0}", rownum + intRowsCount)].Formula = "=SUM(" + string.Format("D{0}:D{1}", 2, rownum + intRowsCount - 1) + ")";
                    worksheet.Range[string.Format("E{0}:E{0}", rownum + intRowsCount)].Formula = "=SUM(" + string.Format("E{0}:E{1}", 2, rownum + intRowsCount - 1) + ")";

                    worksheet.Range[string.Format("C{0}:C{0}", rownum + intRowsCount)].Formula = "=C" + MSummaryRow.JoinToString("+C");
                    worksheet.Range[string.Format("D{0}:D{0}", rownum + intRowsCount)].Formula = "=D" + MSummaryRow.JoinToString("+D");
                    worksheet.Range[string.Format("E{0}:E{0}", rownum + intRowsCount)].Formula = "=E" + MSummaryRow.JoinToString("+E");

                    // worksheet.Range[string.Format("F{0}:F{0}", rownum + intRowsCount)].Formula = "=" + string.Format("D{0}/IF(C{0}=0, 1,C{0})*100", rownum + intRowsCount);
                    worksheet.Range[string.Format("F{0}:F{0}", rownum + intRowsCount)].Formula = "=" + string.Format("D{0}/IF(D{0}+E{0}=0, 1,D{0}+E{0})", rownum + intRowsCount);

                    worksheet.Cells[rownum + intRowsCount, 7] = $"=IF(F{rownum + intRowsCount}>=0.97,\"PASS\",\"FAIL\")";
                    worksheet.Range[string.Format("A{0}:G{0}", rownum + intRowsCount)].Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = 1;
                    worksheet.Range[string.Format("A{0}:G{0}", rownum + intRowsCount)].Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = 1;
                    worksheet.Range[string.Format("A{0}:G{0}", rownum + intRowsCount)].Interior.Color = Color.FromArgb(255, 255, 1);
                    worksheet.Range[string.Format("A{0}:G{0}", rownum + intRowsCount)].Font.Bold = true;
                }

                worksheet.Range[string.Format("G1:G{0}", rownum + intRowsCount)].Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = 1;
                worksheet.Range[string.Format("G1:G{0}", rownum + intRowsCount)].Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = 1;
                worksheet.Range[string.Format("G1:G{0}", rownum + intRowsCount - 1)].Interior.Color = Color.FromArgb(254, 255, 146);
                worksheet.Columns.AutoFit();
                #endregion

                #region 匯出 Fail Order List by SP Data
                if ((this.gdtSP != null) && (this.gdtSP.Rows.Count > 0))
                {
                    worksheet = excel.ActiveWorkbook.Worksheets[2];
                    worksheet.Name = "Fail Order List by SP";
                    string[] aryTitles = new string[] { "Country", "KPI Group", "Factory", "SP No", "Style", "Seq", "Brand", "Buyer Delivery", "Factory KPI", "Extension", "Delivery By Shipmode ", "Order Qty", "On Time Qty", "Fail Qty", "Fail PullOut Date", "ShipMode", "[P]", "Garment Complete", "ReasonID", "Order Reason", "Last Sewing Output Date", "Last Carton Received Date", "Outstanding Reason", "Reason Remark", "Handle", "SMR", "PO Handle", "PO SMR", "Order Type", "Dev. Sample", "FOC Qty" };
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
                            string ExcelColumnName = Db_ExcelColumn[aryAlpha[intIndex_0]];
                            objArray_1[0, intIndex_0] = this.gdtSP.Rows[intIndex][ExcelColumnName].ToString();
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
                        string[] aryTitles = new string[] { "Country", "KPI Group", "Factory", "SP No", "Style", "Seq", "Brand", "Buyer Delivery", "Factory KPI", "Extension", "Delivery By Shipmode", "Order Qty", "On Time Qty", "Fail Qty", "PullOut Date", "ShipMode", "[P]", "Garment Complete", "ReasonID", "Order Reason", "Handle  ", "SMR", "PO Handle", "PO SMR", "Order Type", "Dev. Sample", "Sewing Qty", "FOC Qty", "Last sewing output date", "Last Carton Received Date", "Partial shipment" };
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
                                string ExcelColumnName = OrderDetail_ExcelColumn[aryAlpha[intIndex_0]];
                                objArray_1[0, intIndex_0] = this.gdtOrderDetail.Rows[intIndex][ExcelColumnName].ToString();
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
                        string[] aryTitles = new string[] { "Country", "KPI Group", "Factory", "SP No", "Style", "Seq", "Factory KPI", "Extension", "Delivery By Shipmode", "Order Qty", "PullOut Qty", "PullOut Date", "ShipMode", "Order Type", "Dev. Sample", "FOC Qty", "Last Sewing Output Date", "Last Carton Received Date" };
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
                                string ExcelColumnName = Db_ExcelColumn2[aryAlpha[intIndex_0]];
                                objArray_1[0, intIndex_0] = this.gdtPullOut.Rows[intIndex][ExcelColumnName].ToString();
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
                                            CTNLastReceiveDate = data.Field<string>("CTNLastReceiveDate")
                                        };
                    if ((gdtFailDetail != null) && (gdtFailDetail.Count() > 0))
                    {
                        worksheet = excel.ActiveWorkbook.Worksheets[5];
                        worksheet.Name = "Fail Detail";
                        string[] aryTitles = new string[] { "Country", "KPI Group", "Factory", "SP No", "Style", "Seq", "Brand", "Factory KPI", "Extension", "Delivery By Shipmode", "Order Qty", "Fail Qty", "PullOut Date", "ShipMode", "ReasonID", "Order Reason", "Handle", "Remark", "Order Type", "Dev. Sample", "FOC Qty", "Last Sewing Output Date", "Last Carton Received Date" };
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
                string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Planning_R17");
                Microsoft.Office.Interop.Excel.Workbook workbook = excel.ActiveWorkbook;
                workbook.SaveAs(strExcelName);
                workbook.Close();
                excel.Quit();
                Marshal.ReleaseComObject(excel);
                Marshal.ReleaseComObject(worksheet);
                Marshal.ReleaseComObject(workbook);

                strExcelName.OpenFile();
                #endregion
                return Result.True;
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
