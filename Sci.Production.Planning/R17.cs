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

            return base.ValidateInput();
        }

        /// <summary>
        /// OnAsyncDataLoad
        /// </summary>
        /// <param name="e">e</param>
        /// <returns>DualResult</returns>
        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            //string[] aryAlpha = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "AA", "AB", "AC", "AD" };
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
into #tmp_main
FROM Orders o WITH (NOLOCK)
LEFT JOIN OrderType ot on o.OrderTypeID = ot.ID and o.BrandID = ot.BrandID and o.BrandID = ot.BrandID
LEFT JOIN Factory f ON o.FACTORYID = f.ID --AND  o.FactoryID = f.KPICode
LEFT JOIN Country c ON F.COUNTRYID = c.ID 
inner JOIN Order_QtyShip Order_QS on Order_QS.id = o.id
LEFT JOIN PO ON o.POID = PO.ID
LEFT JOIN Reason r on r.id = Order_QS.ReasonID and r.ReasonTypeID = 'Order_BuyerDelivery'          
LEFT JOIN Reason rs on rs.id = Order_QS.ReasonID and rs.ReasonTypeID = 'Order_BuyerDelivery_sample'
LEFT JOIN Brand b on o.BrandID = b.ID
where o.Junk = 0  
and (isnull(ot.IsGMTMaster,0) = 0 or o.OrderTypeID = '') ";

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

                if (this.dateFactoryKPIDate.Value1 != null)
                {
                    strSQL += string.Format(" AND Order_QS.FtyKPI >= '{0}' ", this.dateFactoryKPIDate.Value1.Value.ToString("yyyy-MM-dd"));
                }

                if (this.dateFactoryKPIDate.Value2 != null)
                {
                    strSQL += string.Format(" AND Order_QS.FtyKPI <= '{0}' ", this.dateFactoryKPIDate.Value2.Value.ToString("yyyy-MM-dd"));
                }

                if (this.txtFactory.Text != string.Empty)
                {
                    strSQL += string.Format(" AND f.KPICode = '{0}' ", this.txtFactory.Text);
                }

                strSQL += @"
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
From Pullout_Detail pd
inner join #tmp_main t on pd.OrderID = t.OrderID and pd.OrderShipmodeSeq = t.Seq
Outer apply (Select Qty = IIF(pd.pulloutdate <= iif(t.ShipmodeID in ('A/C', 'A/P', 'E/C', 'E/P'), t.FtyKPI, DATEADD(day, isnull(t.OTDExtension,0), t.FtyKPI)), pd.shipqty, 0)) rA --On Time
Outer apply (Select Qty = IIF(pd.pulloutdate >  iif(t.ShipmodeID in ('A/C', 'A/P', 'E/C', 'E/P'), t.FtyKPI, DATEADD(day, isnull(t.OTDExtension,0), t.FtyKPI)), pd.shipqty, 0)) rB --Fail
group by pd.OrderID, pd.OrderShipmodeSeq

select max(p.PulloutDate)PulloutDate ,pd.OrderID,pd.OrderShipmodeSeq
into #tmp_Pullout_Detail_p
from Pullout_Detail pd 
inner join #tmp_main t on pd.OrderID = t.OrderID and pd.OrderShipmodeSeq = t.Seq
INNER JOIN Pullout p ON p.Id=pd.id AND p.PulloutDate=pd.PulloutDate 
where pd.OrderID = t.OrderID and pd.OrderShipmodeSeq =  t.Seq  
group by pd.OrderID,pd.OrderShipmodeSeq

select *
into #tmp_Pullout_Detail_pd2
from 
(
	Select iif(pd.PulloutDate > iif(t.ShipmodeID in ('A/C', 'A/P', 'E/C', 'E/P'), t.FtyKPI, DATEADD(day, isnull(t.OTDExtension,0), t.FtyKPI)), 1, 0) isFail
		,pd.PulloutDate
		,pd.OrderID,pd.OrderShipmodeSeq
		,ROW_NUMBER() over(PARTITION BY pd.OrderID,pd.OrderShipmodeSeq order by pd.pulloutdate ASC) r_id
	From Pullout_Detail pd
	inner join #tmp_main t on pd.OrderID = t.OrderID and pd.OrderShipmodeSeq = t.Seq
)a
where a.r_id = 1

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

SELECT [CTNLastReceiveDate]= format(Max(rcv.minreceivedate),'yyyy/MM/dd') , rcv.orderid,rcv.ordershipmodeseq 
into #tmp_ClogReceive
FROM (
	SELECT [MinReceiveDate]=Min(CR.receivedate), cr.orderid, PD.ordershipmodeseq 
	FROM Production.dbo.ClogReceive cr 
	inner join #tmp_main t2 on t2.OrderID = cr.orderid and t2.localorder = 0 
	INNER JOIN Production.dbo.PackingList_Detail PD ON PD.id = CR.PackingListID AND PD.ctnstartno = CR.ctnstartno  and t2.Seq=PD.ordershipmodeseq
	GROUP BY cr.PackingListID, cr.ctnstartno, cr.orderid, PD.ordershipmodeseq
) rcv 
GROUP BY rcv.orderid,rcv.ordershipmodeseq 


SELECT  * 
INTO #tmp FROM 
( 
SELECT
		 t.CountryID
		,t.KPICode
		,t.FactoryID
		,t.OrderID 
		,t.seq
		,t.BrandID
		,BuyerDelivery = convert(varchar(10),t.BuyerDelivery,111)--G
		,FtyKPI= convert(varchar(10),t.FtyKPI,111)
		,Extension = convert(varchar(10),iif(t.ShipmodeID in ('A/C', 'A/P', 'E/C', 'E/P'), t.FtyKPI, DATEADD(day, isnull(t.OTDExtension,0), t.FtyKPI)), 111)--I
		,DeliveryByShipmode = t.ShipmodeID
		,t.OrderQty 
		,OnTimeQty = iif(t.isDevSample = 1, iif(pd2.isFail = 1 or pd2.PulloutDate is null, 0, Cast(t.OrderQty as int)), Cast(isnull(pd.Qty,0) as int))
		,FailQty = CASE  WHEN t.GMTComplete='S' AND pd2.PulloutDate IS NULL  THEN 0
								ELSE iif(t.isDevSample = 1, iif(pd2.isFail = 1 or pd2.PulloutDate is null, Cast(t.OrderQty as int), 0), Cast(isnull(pd.FailQty,t.OrderQty) as int)) END
		,pullOutDate = iif(isnull(t.isDevSample,0) = 1, CONVERT(char(10), pd2.PulloutDate, 20), CONVERT(char(10), p.PulloutDate, 20))
		,Shipmode = t.ShipmodeID
		,P = Cast(isnull(op.PulloutDate,0) as int)  --未出貨,出貨次數=0
		,t.GMTComplete 
		,t.ReasonID
		,t.ReasonName   
		,MR = dbo.getTPEPass1_ExtNo(t.MRHandle)
		,SMR = dbo.getTPEPass1_ExtNo(t.SMR)
		,POHandle = dbo.getTPEPass1_ExtNo(t.POHandle)
		,POSMR = dbo.getTPEPass1_ExtNo(t.POSMR)
		,OrderTypeID = t.OrderTypeID
		,isDevSample = iif(t.isDevSample = 1, 'Y', '')
		,sew.SewouptQty
		,sew.SewLastDate
		,ctnr.CTNLastReceiveDate
		,Order_QtyShipCount=iif(ps.ct>1,'Y','')
		,t.Alias 
		,t.MDivisionID
from #tmp_main t
--出貨次數--
left join #tmp_Pullout_Detail op on op.OrderID = t.OrderID and op.OrderShipmodeSeq = t.Seq 
------------
-----isDevSample=0-----
left join  #tmp_Pullout_Detail_pd pd on pd.OrderID = t.OrderID and pd.OrderShipmodeSeq = t.Seq 
left join  #tmp_Pullout_Detail_p p on p.OrderID = t.OrderID and p.OrderShipmodeSeq = t.Seq 
---------End-------
-------isDevSample=1-----
left join #tmp_Pullout_Detail_pd2 pd2 on pd2.OrderID = t.OrderID and pd2.OrderShipmodeSeq = t.Seq 
-------sew
left join #tmp_SewingOutput sew on sew.OrderId = t.OrderID
----[CTNLastReceiveDate]
left join #tmp_ClogReceive ctnr on ctnr.orderid = t.OrderID and ctnr.ordershipmodeseq = t.Seq
outer apply(
	select ct=count(distinct seq)
	from Order_QtyShip oq
	where oq.id = t.OrderID
)ps
where t.OrderQty > 0 
-----End-------

UNION ALL ----------------------------------------------------
SELECT 
	CountryID = t.CountryID
    ,KPICode = t.KPICode
    ,FactoryID = t.FactoryID
    ,t.OrderID  
    ,t.Seq 
    ,t.BrandID  
    , BuyerDelivery = convert(varchar(10),t.BuyerDelivery,111)
    , FtyKPI = convert(varchar(10),t.FtyKPI,111)
    , Extension = convert(varchar(10),iif(t.ShipmodeID in ('A/C', 'A/P', 'E/C', 'E/P'), t.FtyKPI, DATEADD(day, isnull(t.OTDExtension,0), t.FtyKPI)), 111)
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
    ,MR = dbo.getTPEPass1_ExtNo(t.MRHandle)
    ,SMR = dbo.getTPEPass1_ExtNo(t.SMR)
    ,POHandle = dbo.getTPEPass1_ExtNo(t.POHandle)
    ,POSMR = dbo.getTPEPass1_ExtNo(t.POSMR)
    ,t.OrderTypeID  
    ,isDevSample = iif(t.isDevSample = 1, 'Y', '')
    ,SewouptQty=sew.SewouptQty
    ,sew.SewLastDate
    ,ctnr.CTNLastReceiveDate
    ,Order_QtyShipCount=iif(ps.ct>1,'Y','')
    ,t.Alias
    ,t.MDivisionID
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
outer apply(
	select ct=count(distinct seq)
	from Order_QtyShip oq
	where oq.id = t.OrderID
)ps
-------End-------
where t.GMTComplete != 'S' 
and t.OrderQty - (pd.Qty + pd.FailQty) > 0 
and isnull(t.isDevSample,0) = 0 --isDevSample = 0 才需要看這邊的規則是否Fail
)a


SELECT t.*
FROM #tmp t
INNER JOIN Factory f ON t.KPICode=f.id

drop table #tmp_Pullout_Detail_p,#tmp_Pullout_Detail_pd,#tmp_Pullout_Detail_pd2,#tmp_Pullout_Detail,#tmp_SewingOutput,#tmp_ClogReceive,#tmp,#tmp_main
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
, '' AS MR
, '' AS SMR
, '' AS POHandle 
, '' AS POSMR
, '' AS OrderTypeID
, '' AS isDevSample
, '' AS SewouptQty
, '' AS SewLastDate
, '' AS CTNLastReceiveDate
, '' AS Order_QtyShipCount
, '' AS Alias
, '' AS MDivisionID
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
                    strSQL += " AND o.Category = 'S' AND f.Type = 'S'";
                }
                else
                {
                    strSQL += " AND o.Category = 'G'";
                }

                if (this.dateFactoryKPIDate.Value1 != null)
                {
                    strSQL += string.Format(" AND Order_QS.FtyKPI >= '{0}' ", this.dateFactoryKPIDate.Value1.Value.ToString("yyyy-MM-dd"));
                }

                if (this.dateFactoryKPIDate.Value2 != null)
                {
                    strSQL += string.Format(" AND Order_QS.FtyKPI <= '{0}' ", this.dateFactoryKPIDate.Value2.Value.ToString("yyyy-MM-dd"));
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

                #region Get pullout Data
                System.Data.DataTable dtPullout_Detail;
                strSQL = @"
Select o.ID 
, convert(varchar(10),pd.PulloutDate,111)  as strData 
From Pullout_Detail pd, Orders o 
Left Join OrderType ot on o.OrderTypeID = ot.ID and o.BrandID = ot.BrandID
Left Join Order_QtyShip Order_QS on o.ID = Order_QS.ID
Left Join Factory f ON o.FACTORYID = f.ID 
Where pd.OrderID = o.ID and (ot.IsGMTMaster = 0 or o.OrderTypeID = '')
and pd.ShipQty> 0  and (o.Junk is null or o.Junk = 0) 
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

                if (this.dateFactoryKPIDate.Value1 != null)
                {
                    strSQL += string.Format(" AND Order_QS.FtyKPI >= '{0}' ", this.dateFactoryKPIDate.Value1.Value.ToString("yyyy-MM-dd"));
                }

                if (this.dateFactoryKPIDate.Value2 != null)
                {
                    strSQL += string.Format(" AND Order_QS.FtyKPI <= '{0}' ", this.dateFactoryKPIDate.Value2.Value.ToString("yyyy-MM-dd"));
                }

                if (this.txtFactory.Text != string.Empty)
                {
                    strSQL += string.Format(" AND f.KPICode = '{0}' ", this.txtFactory.Text);
                }

                if (!(result = DBProxy.Current.Select(null, strSQL, null, out dtPullout_Detail)))
                {
                    return result;
                }

                IDictionary<string, IList<DataRow>> dictionary_Pullout_DetailIDs = dtPullout_Detail.ToDictionaryList((x) => x.Val<string>("ID"));
                #endregion  Get pullout Data

                #region Get Pullout Data
                System.Data.DataTable dtPullout;
                strSQL = @"Select o.ID , COUNT(*) as strData 
From Pullout_Detail p, ORDERS o 
Left Join OrderType ot on o.OrderTypeID = ot.ID and o.BrandID = ot.BrandID
Left Join Order_QtyShip Order_QS on o.ID = Order_QS.ID
Left Join Factory f ON o.FactoryID = f.ID 
Where p.OrderID = o.ID and (ot.IsGMTMaster = 0 or o.OrderTypeID = '')
and p.ShipQty> 0  and (o.Junk is null or o.Junk = 0)
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

                if (this.dateFactoryKPIDate.Value1 != null)
                {
                    strSQL += string.Format(" AND Order_QS.FtyKPI >= '{0}' ", this.dateFactoryKPIDate.Value1.Value.ToString("yyyy-MM-dd"));
                }

                if (this.dateFactoryKPIDate.Value2 != null)
                {
                    strSQL += string.Format(" AND Order_QS.FtyKPI <= '{0}' ", this.dateFactoryKPIDate.Value2.Value.ToString("yyyy-MM-dd"));
                }

                if (this.txtFactory.Text != string.Empty)
                {
                    strSQL += string.Format(" AND f.KPICode = '{0}' ", this.txtFactory.Text);
                }

                strSQL += " GROUP BY o.ID ";
                if (!(result = DBProxy.Current.Select(null, strSQL, null, out dtPullout)))
                {
                    return result;
                }

                IDictionary<string, DataRow> dictionary_PulloutIDs = dtPullout.ToDictionary((x) => x.Val<string>("ID"));
                #endregion  Get Pullout Data

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
                    strSQL += " AND o.Category = 'S' AND f.Type = 'S'";
                }
                else
                {
                    strSQL += " AND o.Category = 'G'";
                }

                if (this.dateFactoryKPIDate.Value1 != null)
                {
                    strSQL += string.Format(" AND Order_QS.FtyKPI >= '{0}' ", this.dateFactoryKPIDate.Value1.Value.ToString("yyyy-MM-dd"));
                }

                if (this.dateFactoryKPIDate.Value2 != null)
                {
                    strSQL += string.Format(" AND Order_QS.FtyKPI <= '{0}' ", this.dateFactoryKPIDate.Value2.Value.ToString("yyyy-MM-dd"));
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
                }).GroupBy(group => group.PoId).Select(g => new
                {
                    PoID = g.Key,
                    sumOrderQty = g.Sum(r => r.OrderQty),
                    sumPullQty = g.Sum(r => r.PullQty),
                    sumFailQty = g.Sum(r => r.FailQty),
                    sumP = g.Sum(r => r.P)
                }).ToArray();

                IDictionary<string, IList<DataRow>> dictionary_TradeHis_OrderIDs = dtTradeHis_Order.ToDictionaryList((x) => x.Val<string>("ID"));
                #endregion  Get pullout Data

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
                        0 : Convert.ToDecimal(drSDP["OnTimeQty"].ToString()) / (Convert.ToDecimal(drSDP["OnTimeQty"].ToString()) + Convert.ToDecimal(drSDP["FailQty"].ToString())) * 100;

                    #endregion Calc SDP Data

                    #region Calc Fail Order List by SP Data

                    // By SP# 明細, group by SPNO 顯示
                    if ((drData["FailQty"].ToString() != string.Empty) && (Convert.ToDecimal(drData["FailQty"].ToString()) > 0) && poid != drData["OrderID"].ToString())
                    {
                        DataRow drSP = this.gdtSP.NewRow();
                        drSP.ItemArray = drData.ItemArray;
                        poid = drData["OrderID"].ToString();
                        foreach (var i in groupQty)
                        {
                            if (i.sumFailQty > 0)
                            {
                                if (i.PoID == drData["OrderID"].ToString())
                                {
                                    drSP["OrderQty"] = i.sumOrderQty;
                                    drSP["OnTimeQty"] = i.sumPullQty;
                                    drSP["FailQty"] = i.sumFailQty;
                                    drSP["P"] = i.sumP;
                                }
                            }
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
                    string where = string.Empty;
                    if (this.radioBulk.Checked)
                    {
                        where += " AND o.Category = 'B' AND f.Type = 'B'";
                    }
                    else if (this.radioSample.Checked)
                    {
                        where += " AND o.Category = 'S' AND f.Type = 'S'";
                    }
                    else
                    {
                        where += " AND o.Category = 'G'";
                    }

                    if (this.dateFactoryKPIDate.Value1 != null)
                    {
                        where += string.Format(" AND Order_QS.FtyKPI >= '{0}' ", this.dateFactoryKPIDate.Value1.Value.ToString("yyyy-MM-dd"));
                    }

                    if (this.dateFactoryKPIDate.Value2 != null)
                    {
                        where += string.Format(" AND Order_QS.FtyKPI <= '{0}' ", this.dateFactoryKPIDate.Value2.Value.ToString("yyyy-MM-dd"));
                    }

                    if (this.txtFactory.Text != string.Empty)
                    {
                        where += string.Format(" AND f.KPICode = '{0}' ", this.txtFactory.Text);
                    }

                    strSQL = $@" 
SELECT Alias = c.alias 
     , KPICode =  f.KPICode
     , FactoryID = o.FactoryID
     , OrderID = o.ID
     , Seq = Order_QS.Seq
     , FtyKPI = convert(varchar(10),Order_QS.FtyKPI ,111)     
     , Extension = convert(varchar(10),iif(Order_QS.ShipmodeID in ('A/C', 'A/P', 'E/C', 'E/P'), Order_QS.FtyKPI, DATEADD(day, isnull(b.OTDExtension,0), Order_QS.FtyKPI)), 111)
     , DeliveryByShipmode = Order_QS.ShipmodeID
     , OrderQty = Order_QS.QTY
     , OnTimeQty = CASE o.GMTComplete WHEN 'S' THEN Order_QS.QTY
	                    ELSE iif(ot.isDevSample = 1, Order_QS.QTY, isnull(opd.sQty,0)) END
     , PulloutDate = iif(ot.isDevSample = 1, convert(varchar(10),opd2.PulloutDate,111),convert(varchar(10),pd.PulloutDate,111))
     , ShipmodeID = Order_QS.ShipmodeID
     , OrderTypeID = o.OrderTypeID      
     , isDevSample = iif(ot.isDevSample = 1, 'Y', '')                                                
FROM ORDERS o WITH (NOLOCK)
LEFT JOIN OrderType ot on o.OrderTypeID = ot.ID and o.BrandID = ot.BrandID
LEFT JOIN FACTORY f ON o.FACTORYID = f.ID 
LEFT JOIN COUNTRY c ON f.COUNTRYID = c.ID 
INNER JOIN Order_QtyShip Order_QS on o.ID = Order_QS.ID
LEFT JOIN Brand b on o.BrandID = b.ID
-----isDevSample=0-----
OUTER APPLY (select sum(ShipQty) as sQty 
             from Pullout_Detail pd              
             where pd.OrderID = o.ID and pd.OrderShipmodeSeq = Order_QS.Seq and pd.pulloutdate <= iif(Order_QS.ShipmodeID in ('A/C', 'A/P', 'E/C', 'E/P'), Order_QS.FtyKPI, DATEADD(day, isnull(b.OTDExtension,0), Order_QS.FtyKPI))) opd
OUTER APPLY (select top 1 PulloutDate 
             from Pullout_Detail pd 
             where pd.OrderID = o.ID and pd.OrderShipmodeSeq = Order_QS.Seq 
             and pd.ShipQty> 0
             Order by PulloutDate desc) pd 
-------End-------
-----isDevSample=1-----
outer apply (
	Select top 1 iif(pd.PulloutDate > iif(Order_QS.ShipmodeID in ('A/C', 'A/P', 'E/C', 'E/P'), Order_QS.FtyKPI, DATEADD(day, isnull(b.OTDExtension,0), Order_QS.FtyKPI)), 1, 0) isFail, pd.PulloutDate
	From Pullout_Detail pd
	where pd.OrderID = o.ID 
	and pd.OrderShipmodeSeq = Order_QS.Seq
	order by pd.PulloutDate ASC
) opd2
-------End-------
where Order_QS.Qty > 0 and  (opd.sQty > 0 or o.GMTComplete = 'S') and (ot.IsGMTMaster = 0 or o.OrderTypeID = '')  and (o.Junk is null or o.Junk = 0) 
{where}
";
                    result = DBProxy.Current.Select(null, strSQL, null, out this.gdtPullOut);
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
                return new DualResult(false, "data loading error.", ex);
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
                string[] aryAlpha = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "AA", "AB", "AC", "AD" };
                object[,] objArray = new object[1, intColumns]; // 每列匯入欄位區間
                #region 將資料放入陣列並寫入Excel範例檔

                Dictionary<string, string> Db_ExcelColumn = new Dictionary<string, string>();
                Dictionary<string, string> Db_ExcelColumn2 = new Dictionary<string, string>();
                

                Db_ExcelColumn.Add("A", "CountryID");
                Db_ExcelColumn.Add("B", "KPICode");
                Db_ExcelColumn.Add("C", "FactoryID");
                Db_ExcelColumn.Add("D", "OrderID");
                Db_ExcelColumn.Add("E", "Seq");
                Db_ExcelColumn.Add("F", "BrandID");
                Db_ExcelColumn.Add("G", "BuyerDelivery");
                Db_ExcelColumn.Add("H", "FtyKPI");
                Db_ExcelColumn.Add("I", "Extension");
                Db_ExcelColumn.Add("J", "DeliveryByShipmode");
                Db_ExcelColumn.Add("K", "OrderQty");
                Db_ExcelColumn.Add("L", "OnTimeQty");
                Db_ExcelColumn.Add("M", "FailQty");
                Db_ExcelColumn.Add("N", "pullOutDate");
                Db_ExcelColumn.Add("O", "Shipmode");
                Db_ExcelColumn.Add("P", "P");
                Db_ExcelColumn.Add("Q", "GMTComplete");
                Db_ExcelColumn.Add("R", "ReasonID");
                Db_ExcelColumn.Add("S", "ReasonName");
                Db_ExcelColumn.Add("T", "MR");
                Db_ExcelColumn.Add("U", "SMR");
                Db_ExcelColumn.Add("V", "POHandle");
                Db_ExcelColumn.Add("W", "POSMR");
                Db_ExcelColumn.Add("X", "OrderTypeID");
                Db_ExcelColumn.Add("Y", "isDevSample");
                Db_ExcelColumn.Add("Z", "SewouptQty");
                Db_ExcelColumn.Add("AA", "SewLastDate");
                Db_ExcelColumn.Add("AB", "CTNLastReceiveDate");
                Db_ExcelColumn.Add("AC", "Order_QtyShipCount");
                Db_ExcelColumn.Add("AD", "Alias");



                Db_ExcelColumn2.Add("A", "Alias");
                Db_ExcelColumn2.Add("B", "KPICode");
                Db_ExcelColumn2.Add("C", "FactoryID");
                Db_ExcelColumn2.Add("D", "OrderID");
                Db_ExcelColumn2.Add("E", "Seq");
                Db_ExcelColumn2.Add("F", "FtyKPI");
                Db_ExcelColumn2.Add("G", "Extension");
                Db_ExcelColumn2.Add("H", "DeliveryByShipmode");
                Db_ExcelColumn2.Add("I", "OrderQty");
                Db_ExcelColumn2.Add("J", "OnTimeQty");
                Db_ExcelColumn2.Add("K", "pullOutDate");
                Db_ExcelColumn2.Add("L", "ShipmodeID");
                Db_ExcelColumn2.Add("M", "OrderTypeID");
                Db_ExcelColumn2.Add("N", "isDevSample");


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
                    objArray[0, 6] = (decimal)dr["SDP"] >= 97 ? "PASS" : "FAIL";

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
                        objArray[0, 5] = "=" + string.Format("D{0}/IF(D{0}+E{0}=0, 1,D{0}+E{0})*100", rownum + i);

                        objArray[0, 6] = (decimal)dr["SDP"] >= 97 ? "PASS" : "FAIL";
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
                    worksheet.Range[string.Format("C{0}:C{0}", rownum + intRowsCount)].Formula = "=SUM(" + string.Format("C{0}:C{1}", 2, rownum + intRowsCount - 1) + ")";
                    worksheet.Range[string.Format("D{0}:D{0}", rownum + intRowsCount)].Formula = "=SUM(" + string.Format("D{0}:D{1}", 2, rownum + intRowsCount - 1) + ")";
                    worksheet.Range[string.Format("E{0}:E{0}", rownum + intRowsCount)].Formula = "=SUM(" + string.Format("E{0}:E{1}", 2, rownum + intRowsCount - 1) + ")";

                    worksheet.Range[string.Format("C{0}:C{0}", rownum + intRowsCount)].Formula = "=C" + MSummaryRow.JoinToString("+C");
                    worksheet.Range[string.Format("D{0}:D{0}", rownum + intRowsCount)].Formula = "=D" + MSummaryRow.JoinToString("+D");
                    worksheet.Range[string.Format("E{0}:E{0}", rownum + intRowsCount)].Formula = "=E" + MSummaryRow.JoinToString("+E");

                    // worksheet.Range[string.Format("F{0}:F{0}", rownum + intRowsCount)].Formula = "=" + string.Format("D{0}/IF(C{0}=0, 1,C{0})*100", rownum + intRowsCount);
                    worksheet.Range[string.Format("F{0}:F{0}", rownum + intRowsCount)].Formula = "=" + string.Format("D{0}/IF(D{0}+E{0}=0, 1,D{0}+E{0})*100", rownum + intRowsCount);

                    worksheet.Cells[rownum + intRowsCount, 7] = $"=IF(F{rownum + intRowsCount}>=97,\"PASS\",\"FAIL\")";
                    worksheet.Range[string.Format("A{0}:G{0}", rownum + intRowsCount)].Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = 1;
                    worksheet.Range[string.Format("A{0}:G{0}", rownum + intRowsCount)].Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = 1;
                    worksheet.Range[string.Format("A{0}:G{0}", rownum + intRowsCount)].Interior.Color = Color.FromArgb(255, 255, 1);
                    worksheet.Range[string.Format("A{0}:G{0}", rownum + intRowsCount)].Font.Bold = true;
                }

                worksheet.Range[string.Format("G1:G{0}", rownum + intRowsCount)].Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = 1;
                worksheet.Range[string.Format("G1:G{0}", rownum + intRowsCount)].Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = 1;
                worksheet.Range[string.Format("G1:G{0}", rownum + intRowsCount)].Interior.Color = Color.FromArgb(254, 255, 146);
                worksheet.Columns.AutoFit();
                #endregion

                #region 匯出 Fail Order List by SP Data
                if ((this.gdtSP != null) && (this.gdtSP.Rows.Count > 0))
                {
                    worksheet = excel.ActiveWorkbook.Worksheets[2];
                    worksheet.Name = "Fail Order List by SP";
                    string[] aryTitles = new string[] { "Country", "KPI Group", "Factory", "SP No", "Seq", "Brand", "Buyer Delivery", "Factory KPI", "Extension", "Delivery By Shipmode ", "Order Qty", "On Time Qty", "Fail Qty", "Fail PullOut Date", "ShipMode", "[P]", "Garment Complete", "ReasonID", "Order Reason", "Handle", "SMR", "PO Handle", "PO SMR", "Order Type", "Dev. Sample" };
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
                    worksheet.Cells[rc + 2, 11] = string.Format("=SUM(K2:K{0})", MyUtility.Convert.GetString(rc + 1));
                    worksheet.Cells[rc + 2, 12] = string.Format("=SUM(L2:L{0})", MyUtility.Convert.GetString(rc + 1));
                    worksheet.Cells[rc + 2, 13] = string.Format("=SUM(M2:M{0})", MyUtility.Convert.GetString(rc + 1));

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
                        string[] aryTitles = new string[] { "Country", "KPI Group", "Factory", "SP No", "Seq", "Brand", "Buyer Delivery", "Factory KPI", "Extension", "Delivery By Shipmode", "Order Qty", "On Time Qty", "Fail Qty", "PullOut Date", "ShipMode", "[P]", "Garment Complete", "ReasonID", "Order Reason", "Handle  ", "SMR", "PO Handle", "PO SMR", "Order Type", "Dev. Sample", "Sewing Qty", "Last sewing output date", "Last carton received date", "Partial shipment" };
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
                                string ExcelColumnName = Db_ExcelColumn[aryAlpha[intIndex_0]];
                                objArray_1[0, intIndex_0] = this.gdtOrderDetail.Rows[intIndex][ExcelColumnName].ToString();
                            }

                            worksheet.Range[string.Format("A{0}:{1}{0}", intIndex + 2, aryAlpha[aryTitles.Length - 1])].Value2 = objArray_1;

                            worksheet.Range[string.Format("G{0}:H{0}", intIndex + 2)].NumberFormatLocal = "yyyy/MM/dd";
                        }

                        worksheet.Columns.AutoFit();
                        worksheet.Cells[rc + 2, 2] = "Total:";
                        worksheet.Cells[rc + 2, 11] = string.Format("=SUM(K2:K{0})", MyUtility.Convert.GetString(rc + 1));
                        worksheet.Cells[rc + 2, 12] = string.Format("=SUM(L2:L{0})", MyUtility.Convert.GetString(rc + 1));
                        worksheet.Cells[rc + 2, 13] = string.Format("=SUM(M2:M{0})", MyUtility.Convert.GetString(rc + 1));

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
                        string[] aryTitles = new string[] { "Country", "KPI Group", "Factory", "SP No", "Seq", "Factory KPI", "Extension", "Delivery By Shipmode", "Order Qty", "PullOut Qty", "PullOut Date", "ShipMode", "Order Type", "Dev. Sample" };
                        object[,] objArray_1 = new object[1, aryTitles.Length];
                        for (int intIndex = 0; intIndex < aryTitles.Length; intIndex++)
                        {
                            objArray_1[0, intIndex] = aryTitles[intIndex];
                        }

                        worksheet.Range[string.Format("A{0}:{1}{0}", 1, aryAlpha[aryTitles.Length - 1])].Value2 = objArray_1;
                        worksheet.Range[string.Format("A{0}:{1}{0}", 1, aryAlpha[aryTitles.Length - 1])].AutoFilter(1); // 篩選
                        worksheet.Range[string.Format("A{0}:{1}{0}", 1, aryAlpha[aryTitles.Length - 1])].Interior.Color = Color.FromArgb(204, 255, 204);
                        worksheet.Range[string.Format("A{0}:{1}{0}", 1, aryAlpha[aryTitles.Length - 1])].Borders.Color = Color.Black;
                        excel.ActiveWorkbook.Worksheets[4].Columns(6).NumberFormatlocal = "yyyy/MM/dd";
                        excel.ActiveSheet.Columns(10).NumberFormatlocal = "yyyy/MM/dd";

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
                        worksheet.Cells[rc + 2, 9] = string.Format("=SUM(I2:I{0})", MyUtility.Convert.GetString(rc + 1));
                        worksheet.Cells[rc + 2, 10] = string.Format("=SUM(J2:J{0})", MyUtility.Convert.GetString(rc + 1));

                        // 設定分割列數
                        excel.ActiveWindow.SplitRow = 1;

                        // 進行凍結視窗
                        excel.ActiveWindow.FreezePanes = true;
                    }
                    #endregion

                    #region 匯出 Fail Detail
                    var gdtFailDetail = from data in this.gdtOrderDetail.AsEnumerable()
                                        where data.Field<int>("FailQty") > 0
                                        select new
                                        {
                                            CountryID = data.Field<string>("CountryID"),
                                            KPICode = data.Field<string>("KPICode"),
                                            FactoryID = data.Field<string>("FactoryID"),
                                            OrderID = data.Field<string>("OrderID"),
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
                                            OrderTypeID = data.Field<string>("OrderTypeID"),
                                            isDevSample = data.Field<string>("isDevSample")
                                        };
                    if ((gdtFailDetail != null) && (gdtFailDetail.Count() > 0))
                    {
                        worksheet = excel.ActiveWorkbook.Worksheets[5];
                        worksheet.Name = "Fail Detail";
                        string[] aryTitles = new string[] { "Country", "KPI Group", "Factory", "SP No", "Seq", "Brand", "Factory KPI", "Extension", "Delivery By Shipmode", "Order Qty", "Fail Qty", "PullOut Date", "ShipMode", "ReasonID", "Order Reason", "Handle", "Order Type", "Dev. Sample" };
                        object[,] objArray_1 = new object[1, aryTitles.Length];
                        for (int intIndex = 0; intIndex < aryTitles.Length; intIndex++)
                        {
                            objArray_1[0, intIndex] = aryTitles[intIndex];
                        }

                        worksheet.get_Range("L:L", Type.Missing).NumberFormatLocal = "@";
                        worksheet.Range[string.Format("A{0}:{1}{0}", 1, aryAlpha[aryTitles.Length - 1])].Value2 = objArray_1;
                        worksheet.Range[string.Format("A{0}:{1}{0}", 1, aryAlpha[aryTitles.Length - 1])].AutoFilter(1); // 篩選
                        worksheet.Range[string.Format("A{0}:{1}{0}", 1, aryAlpha[aryTitles.Length - 1])].Interior.Color = Color.FromArgb(204, 255, 204);
                        worksheet.Range[string.Format("A{0}:{1}{0}", 1, aryAlpha[aryTitles.Length - 1])].Borders.Color = Color.Black;
                        excel.ActiveWorkbook.Worksheets[5].Columns(6).NumberFormatlocal = "yyyy/MM/dd";
                        excel.ActiveSheet.Columns(10).NumberFormatlocal = "yyyy/MM/dd";
                        int rc = gdtFailDetail.Count();
                        int i = 1;
                        foreach (var dr in gdtFailDetail)
                        {
                            i++;
                            objArray_1[0, 0] = dr.CountryID;
                            objArray_1[0, 1] = dr.KPICode;
                            objArray_1[0, 2] = dr.FactoryID;
                            objArray_1[0, 3] = dr.OrderID;
                            objArray_1[0, 4] = dr.Seq;
                            objArray_1[0, 5] = dr.BrandID;
                            objArray_1[0, 6] = dr.FtyKPI;
                            objArray_1[0, 7] = dr.Extension;
                            objArray_1[0, 8] = dr.DeliveryByShipmode;
                            objArray_1[0, 9] = dr.OrderQty;
                            objArray_1[0, 10] = dr.FailQty;
                            objArray_1[0, 11] = dr.pullOutDate;
                            objArray_1[0, 12] = dr.Shipmode;
                            objArray_1[0, 13] = dr.ReasonID;
                            objArray_1[0, 14] = dr.ReasonName;
                            objArray_1[0, 15] = dr.MR;
                            objArray_1[0, 16] = dr.OrderTypeID;
                            objArray_1[0, 17] = dr.isDevSample;
                            worksheet.Range[string.Format("A{0}:{1}{0}", i, aryAlpha[aryTitles.Length - 1])].Value2 = objArray_1;
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

                return new DualResult(false, "Export excel error.", ex);
            }
        }
    }


}
