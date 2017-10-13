using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Reflection;
using Microsoft.Office.Interop.Excel;

using Sci.Data;
using Ict;
using Ict.Win;
using Sci.Win;
using Sci.Production.Report;
using System.Runtime.InteropServices;


namespace Sci.Production.Planning
{
    public partial class R17 : Sci.Win.Tems.PrintForm
    {
        System.Data.DataTable gdtDatas, gdtOrderDetail, gdtPullOut, gdtFailDetail, gdtSP;

        public R17()
        {
            InitializeComponent();
        }

        public R17(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            EditMode = true;
            print.Visible = false;
            txtFactory.Text = Sci.Env.User.Factory;
            dateFactoryKPIDate.Select();
        }

        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            return true;
        }

        protected override bool ValidateInput()
        {
            return base.ValidateInput();
        }

        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            DualResult result = new DualResult(true);
            try
            {
                string strSQL = "";
                StringBuilder sqlcmd = new StringBuilder();
                sqlcmd.Append(@" 
select A = A1.FACTORYID,B = A3.ALIAS, A1.ID, Order_QS.FtyKPI ,C = SUM(A1.QTY)OVER (PARTITION BY A1.FACTORYID,A3.ALIAS)
into #tmp
FROM ORDERS A1 WITH (NOLOCK) 
Left Join Order_QtyShip Order_QS on A1.ID = Order_QS.ID
LEFT JOIN FACTORY A2 WITH (NOLOCK) ON A1.FACTORYID = A2.ID 
LEFT JOIN COUNTRY A3 WITH (NOLOCK) ON A2.COUNTRYID = A3.ID 
WHERE 1= 1  ");
                if (dateFactoryKPIDate.Value1 != null)
                    sqlcmd.Append(string.Format(" AND Order_QS.FtyKPI >= '{0}' ", dateFactoryKPIDate.Value1.Value.ToString("yyyy-MM-dd")));
                if (dateFactoryKPIDate.Value2 != null)
                    sqlcmd.Append(string.Format(" AND Order_QS.FtyKPI <= '{0}' ", dateFactoryKPIDate.Value2.Value.ToString("yyyy-MM-dd")));
                if (radioBulk.Checked)
                {
                    if (MyUtility.Check.Empty(txtFactory.Text)) //factory沒值
                    {
                        sqlcmd.Append(" AND A1.FACTORYID IN ( select ID from Factory where KPICode!='' and Type='B' and KPICode in (select distinct ID from Factory where KPICode!=''))");
                    }
                    else  //factory有值
                    {
                        sqlcmd.Append(string.Format(" AND A1.FACTORYID IN (select KPICode from Factory where ID='{0}' and Type='B')", txtFactory.Text));
                    }
                    sqlcmd.Append(" AND A1.Category='B'");
                }
                else if (radioSample.Checked)
                {
                    if (MyUtility.Check.Empty(txtFactory.Text)) //factory沒值
                    {
                        sqlcmd.Append(" AND A1.FACTORYID IN ( select ID from Factory where KPICode!='' and Type='S' and KPICode in (select distinct ID from Factory where KPICode!=''))");
                    }
                    else  //factory有值
                    {
                        sqlcmd.Append(string.Format(" AND A1.FACTORYID IN (select KPICode from Factory where ID='{0}' and Type='S')", txtFactory.Text));
                    }
                    sqlcmd.Append(" AND A1.Category='S'");
                }
                  
                //
               
                sqlcmd.Append(@" 

SELECT a, 
       b, 
       c, 
       Iif(c - d - e <> 0, d + ( c - d - e ), d) AS d, 
       --註解: if Order_Qty <> (Ontime_Qty+Delay_qty), 就將差額加回去去OntimeQty 
       e, 
       f 
FROM   (SELECT a, 
               b, 
               c, 
               Sum(d + variance)d, 
               Sum(e)           e, 
               F=Concat(Iif(c - Sum(d + variance) - Sum(e) <> 0, 
                                 Round(CONVERT(FLOAT, Sum(d + variance) + ( 
                                                      c - Sum(d + variance) 
                                                      - Sum( 
                                                      e) )) / 
                                       CONVERT 
                                             (FLOAT, c) * 100, 2), Round( 
                                 CONVERT(FLOAT, Sum(d + variance 
                                                )) / CONVERT(FLOAT, c) * 100, 2) 
                        ), '%') 
        FROM   (SELECT DISTINCT a, 
                                b, 
                                c, 
                                a1.id, 
                                Isnull(pass.qty, 0)        AS d, 
                                ( Isnull(Variance.qty, 0) )AS Variance, 
                                Isnull(delay.qty, 0)       AS e 
                FROM   #tmp A1 
                       LEFT JOIN pullout_detail F WITH (nolock) 
                              ON A1.id = F.orderid 
                       OUTER apply(SELECT Sum(Isnull(orderqty, 0) - Isnull( 
                                              shipqty, 0)) 
                                          qty 
                                   FROM   pullout_detail 
                                   WHERE  orderid = A1.id 
                                          AND ( status = 'S' )) Variance 
                       OUTER apply(SELECT Sum(shipqty) qty 
                                   FROM   pullout_detail 
                                   WHERE  orderid = A1.id 
                                          AND pulloutdate <= ftykpi) pass 
                       OUTER apply(SELECT Sum(shipqty) qty 
                                   FROM   pullout_detail 
                                   WHERE  orderid = A1.id 
                                          AND pulloutdate > ftykpi
                                UNION 
                    SELECT  SUM(Qty) qty 
                    FROM Orders WITH (NOLOCK)                     
                    WHERE A1.ID = ID AND KPIChangeReason=0005
                    AND NOT EXISTS (SELECT 1 FROM Pullout_Detail WHERE OrderID=A1.ID)
                    
) delay) aa 
        GROUP  BY a, 
                  b, 
                  c) aa 

      drop table #tmp");
                result = DBProxy.Current.Select(null, sqlcmd.ToString(), null, out gdtDatas);
                if (!result) return result;
                if ((gdtDatas == null) || (gdtDatas.Rows.Count == 0))
                    return new DualResult(false, "Datas not found!");

                #region Fail Order List by SP
                strSQL = @"
SELECT   A= A2.CountryID 
		,B = A2.KpiCode
		,C = A1.FactoryID 
		,D = A1.ID 
		,E = A1.BRANDID
		,F = Convert(varchar,Order_QS.BuyerDelivery)  
		,G = Convert(varchar,cast(Order_QS.FtyKPI as date)) 
		,H = (SELECT strData+',' FROM (SELECT Convert(varchar, Order_QtyShip.ShipmodeID) + '-' + Convert(varchar, Order_QtyShip.Qty) + '(' + REPLACE(Convert(varchar, Order_QtyShip.BuyerDelivery),'-','/') + ')' as strData FROM Order_QtyShip WITH (NOLOCK) where id = A1.ID) t for xml path('')) 
		,I = Order_QS.QTY  
		,J = Sum(A4.ShipQty) 
		,K = ISNULL(Sum(A5.ShipQty),0) 
		,L = (select strData+',' from (Select REPLACE(convert(varchar,PulloutDate),'-','/') as strData from Pullout_Detail WITH (NOLOCK) where OrderID = A1.ID)t for xml path('')) 
		,M = t.strData 
		,N = (Select Count(id) as CountPullOut from Pullout_Detail WITH (NOLOCK) where OrderID = A1.ID) 
		,O = CASE WHEN A1.GMTComplete   = 'C' OR A1.GMTComplete   = 'S' THEN 'Y' ELSE '' END 
		,P = A1.KPIChangeReason 
		,Q = (select TOP 1 Name from Reason WITH (NOLOCK) where ReasonTypeID = 'Order_BuyerDelivery' and ID = A1.KPIChangeReason ) 
		,R = dbo.getTPEPass1(A1.MRHandle)+vs1.ExtNo  
		,S = dbo.getTPEPass1(A1.SMR)+vs2.ExtNo  
		,T = dbo.getTPEPass1(A6.POHandle)+vs3.ExtNo  
		,U = dbo.getTPEPass1(A6.POSMR)+vs4.ExtNo  
FROM ORDERS A1 WITH (NOLOCK) 
LEFT JOIN Order_QtyShip Order_QS WITH (NOLOCK) ON Order_QS.id=A1.ID
LEFT JOIN Pullout_Detail AP WITH (NOLOCK) ON A1.ID =AP.OrderID
LEFT JOIN FACTORY A2 WITH (NOLOCK) ON A1.FACTORYID = A2.ID 
LEFT JOIN COUNTRY A3 WITH (NOLOCK) ON A2.COUNTRYID = A3.ID 
LEFT JOIN PullOut_Detail A4 WITH (NOLOCK) ON A1.ID = A4.ORDERID AND A4.PullOutDate <= Order_QS.FtyKPI AND A4.UKey=AP.UKey
LEFT JOIN PO A6 WITH (NOLOCK) ON A1.POID = A6.ID
OUTER APPLY(
SELECT  ShipQty FROM PullOut_Detail WITH (NOLOCK) 
WHERE A1.ID = ORDERID AND PullOutDate > Order_QS.FtyKPI
AND UKey=AP.UKey
UNION 
SELECT  Qty AS ShipQty FROM Orders WITH (NOLOCK) 
WHERE A1.ID = ID AND KPIChangeReason=0005
AND NOT EXISTS(SELECT 1 FROM   pullout_detail 
          WHERE  orderid = A1.id)
) A5
OUTER APPLY(
  select strData =stuff(( 
    Select DISTINCT concat(',',ShipmodeID)
    from Order_QtyShip 
    WITH (NOLOCK) where id = A1.ID  Group by ShipModeID
    for xml path('')
  ),1,1,'')
) t
outer apply (SELECT ' #'+ExtNo AS ExtNo from dbo.TPEPASS1 a WITH (NOLOCK) where a.ID= A1.MRHandle ) vs1
outer apply (SELECT ' #'+ExtNo AS ExtNo from dbo.TPEPASS1 a WITH (NOLOCK) where a.ID= A1.SMR ) vs2
outer apply (SELECT ' #'+ExtNo AS ExtNo from dbo.TPEPASS1 a WITH (NOLOCK) where a.ID= A6.POHandle ) vs3
outer apply (SELECT ' #'+ExtNo AS ExtNo from dbo.TPEPASS1 a WITH (NOLOCK) where a.ID= A6.POSMR ) vs4
 WHERE 1= 1  ";
                if (dateFactoryKPIDate.Value1 != null)
                    strSQL += string.Format(" AND Order_QS.FtyKPI >= '{0}' ", dateFactoryKPIDate.Value1.Value.ToString("yyyy-MM-dd"));
                if (dateFactoryKPIDate.Value2 != null)
                    strSQL += string.Format(" AND Order_QS.FtyKPI <= '{0}' ", dateFactoryKPIDate.Value2.Value.ToString("yyyy-MM-dd"));
                //補上 Orders.Cateory and Factory.Type 判斷
                if (radioBulk.Checked)
                {
                    if (MyUtility.Check.Empty(txtFactory.Text)) //factory沒值
                    {
                        strSQL +=(" AND A1.FACTORYID IN ( select ID from Factory where KPICode!='' and Type='B' and KPICode in (select distinct ID from Factory where KPICode!=''))");
                    }
                    else  //factory有值
                    {
                        strSQL +=(string.Format(" AND A1.FACTORYID IN (select KPICode from Factory where ID='{0}' and Type='B')", txtFactory.Text));
                    }
                    strSQL +=(" AND A1.Category='B'");
                }
                else if (radioSample.Checked)
                {
                    if (MyUtility.Check.Empty(txtFactory.Text)) //factory沒值
                    {
                        strSQL +=(" AND A1.FACTORYID IN ( select ID from Factory where KPICode!='' and Type='S' and KPICode in (select distinct ID from Factory where KPICode!=''))");
                    }
                    else  //factory有值
                    {
                        strSQL +=(string.Format(" AND A1.FACTORYID IN (select KPICode from Factory where ID='{0}' and Type='S')", txtFactory.Text));
                    }
                    strSQL +=(" AND A1.Category='S'");
                }

                strSQL += @" 
GROUP BY A2.CountryID,  A2.KpiCode, A1.FactoryID , A1.ID, A1.BRANDID,A1.KPIChangeReason
, Order_QS.BuyerDelivery, Order_QS.FtyKPI, Order_QS.QTY 
, CASE WHEN A1.GMTComplete   = 'C' OR A1.GMTComplete   = 'S' THEN 'Y' ELSE '' END
, A1.MRHandle, A1.SMR, A6.POHandle, A6.POSMR,t.strData,vs1.ExtNo ,vs2.ExtNo,vs3.ExtNo,vs4.ExtNo
HAVING Sum(A5.ShipQty) > 0 ";
                strSQL += @" 
ORDER BY A1.ID";
                result = DBProxy.Current.Select(null, strSQL, null, out gdtSP);
                if (!result) return result;

                #endregion Fail Order List by SP

                //有勾選
                if (checkExportDetailData.Checked)
                {
                    #region Order Detail
                    strSQL = @" 
SELECT A = A2.CountryID
       , B = A2.KpiCode
       , C = A1.FactoryID 
       , D = A1.ID
       , E = A1.BRANDID
       , F = Convert(varchar,A1.BuyerDelivery )
       , G = Convert(varchar,cast( Order_QS.FtyKPI as date)) 
       , H = (SELECT strData + ',' 
              FROM (
                SELECT strData = Convert(varchar, Order_QtyShip.ShipmodeID) 
                                 + '-' 
                                 + Convert(varchar, Order_QtyShip.Qty) 
                                 + '(' 
                                 +  REPLACE(Convert(varchar, Order_QtyShip.BuyerDelivery), '-', '/') 
                                 + ')'
                FROM Order_QtyShip WITH (NOLOCK) 
                where id = A1.ID
              ) t for xml path('')) 
       , I = Order_QS.QTY
       , J = isnull(Sum(onTimePD.ShipQty), 0)
       , K = ISNULL(Sum(A5.ShipQty), 0)
       , L = (select strData + ',' 
              from (
                Select strData = REPLACE(convert(varchar,PulloutDate), '-', '/') 
                from Pullout_Detail WITH (NOLOCK) 
                where OrderID = A1.ID
              ) t for xml path(''))       
       , M = t.strData
       , N = (Select CountPullOut = Count(id) 
              from Pullout_Detail WITH (NOLOCK) 
              where OrderID = A1.ID)
      , O = CASE 
                WHEN A1.GMTComplete = 'C' OR A1.GMTComplete = 'S' THEN 'Y' 
                ELSE '' 
             END
       --, P = (SELECT TOP 1 A1.ReasonID  
       --       from Order_History A1 WITH (NOLOCK) 
       --       Where A1.OldValue =  A1.ID  
       --             And A1.HisType = 'Delivery')
       --, Q = (Select TOP 1 A2.Name  
       --       from Order_History A1 WITH (NOLOCK) 
       --       LEFT JOIN Reason A2 WITH (NOLOCK) ON A2.ID = A1.ReasonID 
       --       Where A1.OldValue = A1.ID 
       --             And A1.HisType = 'Delivery')
	   , P= Order_QS.ReasonID
	   , Q = case A1.Category when 'B' then r.Name
		 when 'S' then rs.Name
		 else '' end 
       , R = dbo.getTPEPass1(A1.MRHandle) + vs1.ExtNo
       , S = dbo.getTPEPass1(A1.SMR)+vs2.ExtNo
       , T = dbo.getTPEPass1(A6.POHandle)+vs3.ExtNo
       , U = dbo.getTPEPass1(A6.POSMR)+vs4.ExtNo
FROM ORDERS A1 WITH (NOLOCK) 
LEFT JOIN Pullout_Detail AP WITH (NOLOCK) ON A1.ID =AP.OrderID
LEFT JOIN FACTORY A2 WITH (NOLOCK) ON A1.FACTORYID = A2.ID 
LEFT JOIN COUNTRY A3 WITH (NOLOCK) ON A2.COUNTRYID = A3.ID 
LEFT JOIN Order_QtyShip Order_QS WITH (NOLOCK) ON Order_QS.id=A1.ID
LEFT JOIN PullOut_Detail onTimePD WITH (NOLOCK) ON A1.ID = onTimePD.ORDERID 
                                                   AND onTimePD.PullOutDate <= Order_QS.FtyKPI 
                                                   AND onTimePD.UKey = AP.UKey
left join PullOut onTimeP With (Nolock) on onTimePD.ID = onTimeP.ID
                                           and onTimeP.Status in ('Locked', 'Confirmed')
LEFT JOIN PO A6 WITH (NOLOCK) ON A1.POID = A6.ID
outer apply(
	select * from Reason
	where id=Order_QS.ReasonID and ReasonTypeID='Order_BuyerDelivery'
)r
outer apply(
	select * from Reason
	where id=Order_QS.ReasonID and ReasonTypeID='Order_BuyerDelivery_sample'
)rs
OUTER APPLY(
  SELECT ShipQty 
  FROM PullOut_Detail WITH (NOLOCK) 
  WHERE A1.ID = ORDERID 
        AND PullOutDate > Order_QS.FtyKPI
        AND UKey=AP.UKey

  UNION 
  SELECT ShipQty = Qty 
  FROM Orders WITH (NOLOCK) 
  WHERE A1.ID = ID 
        AND KPIChangeReason = 0005
        AND NOT EXISTS (SELECT 1 
                        FROM pullout_detail 
                        WHERE orderid = A1.id)
) A5
OUTER APPLY(
   select strData = stuff ((Select DISTINCT concat(',', ShipmodeID)
                            from Order_QtyShip WITH (NOLOCK) 
                            where id = A1.ID  
                            Group by ShipModeID
                            for xml path('')
                            )
                           , 1, 1, '')
) t
outer apply (
  SELECT ExtNo = ' #' + ExtNo 
  from dbo.TPEPASS1 a WITH (NOLOCK) 
  where a.ID = A1.MRHandle 
) vs1
outer apply (
  SELECT ExtNo = ' #' + ExtNo 
  from dbo.TPEPASS1 a WITH (NOLOCK) 
  where a.ID = A1.SMR 
) vs2
outer apply (
  SELECT ExtNo = ' #' + ExtNo 
  from dbo.TPEPASS1 a WITH (NOLOCK) 
  where a.ID = A6.POHandle 
) vs3
outer apply (
  SELECT ExtNo = ' #' + ExtNo 
  from dbo.TPEPASS1 a WITH (NOLOCK) 
  where a.ID = A6.POSMR 
) vs4
WHERE 1 = 1 
      and a1.Qty <> 0 ";
                    if (dateFactoryKPIDate.Value1 != null)
                        strSQL += string.Format(" AND Order_QS.FtyKPI >= '{0}' ", dateFactoryKPIDate.Value1.Value.ToString("yyyy-MM-dd"));
                    if (dateFactoryKPIDate.Value2 != null)
                        strSQL += string.Format(" AND Order_QS.FtyKPI <= '{0}' ", dateFactoryKPIDate.Value2.Value.ToString("yyyy-MM-dd"));
                    //補上 Orders.Cateory and Factory.Type 判斷
                    if (radioBulk.Checked)
                    {
                        if (MyUtility.Check.Empty(txtFactory.Text)) //factory沒值
                        {
                            strSQL +=(" AND A1.FACTORYID IN ( select ID from Factory where KPICode!='' and Type='B' and KPICode in (select distinct ID from Factory where KPICode!=''))");
                        }
                        else  //factory有值
                        {
                            strSQL +=(string.Format(" AND A1.FACTORYID IN (select KPICode from Factory where ID='{0}' and Type='B')", txtFactory.Text));
                        }
                        strSQL +=(" AND A1.Category='B'");
                    }
                    else if (radioSample.Checked)
                    {
                        if (MyUtility.Check.Empty(txtFactory.Text)) //factory沒值
                        {
                            strSQL +=(" AND A1.FACTORYID IN ( select ID from Factory where KPICode!='' and Type='S' and KPICode in (select distinct ID from Factory where KPICode!=''))");
                        }
                        else  //factory有值
                        {
                            strSQL +=(string.Format(" AND A1.FACTORYID IN (select KPICode from Factory where ID='{0}' and Type='S')", txtFactory.Text));
                        }
                        strSQL +=(" AND A1.Category='S'");
                    }

                    strSQL += @" 
GROUP BY A2.CountryID,  A2.KpiCode, A1.FactoryID , A1.ID, A1.BRANDID
                                                        , A1.BuyerDelivery, Order_QS.FtyKPI, Order_QS.QTY 
                                                        , CASE WHEN A1.GMTComplete   = 'C' OR A1.GMTComplete   = 'S' THEN 'Y' ELSE '' END
                                                        , A1.MRHandle, A1.SMR, A6.POHandle, A6.POSMR,t.strData,vs1.ExtNo ,vs2.ExtNo,vs3.ExtNo,vs4.ExtNo 
                        ,Order_QS.ReasonID,A1.Category,r.Name,rs.Name ";
                    strSQL += @" 
ORDER BY A1.ID";
                    result = DBProxy.Current.Select(null, strSQL, null, out gdtOrderDetail);
                    if (!result) return result;

                    #endregion Order Detail

                    #region On time Order List by PullOut
                    strSQL = @" 
SELECT   A = A2.CountryID 
		,B = A2.KpiCode 
		,C = A1.FactoryID   
		,D = A1.ID 
		,E = Convert(varchar,cast(Order_QS.FtyKPI as date))
		,F = (SELECT strData+',' FROM (SELECT Convert(varchar, Order_QtyShip.ShipmodeID) + '-' + Convert(varchar, Order_QtyShip.Qty) + '(' +  REPLACE(Convert(varchar, Order_QtyShip.BuyerDelivery),'-','/') + ')' as strData FROM Order_QtyShip WITH (NOLOCK) where id = A1.ID) t for xml path(''))  
		,G = Order_QS.QTY  
		,H = isnull(opd.sQty,0)
		,I = Convert(varchar,pd.PulloutDate)
		,J = (SELECT distinct oq.ShipmodeID+','
from Order_QtyShip oq WITH (NOLOCK) 
where oq.id=a1.id 
for xml path(''))                                               
FROM ORDERS A1 WITH (NOLOCK) 
LEFT JOIN Order_QtyShip Order_QS WITH (NOLOCK) ON Order_QS.id=A1.ID
LEFT JOIN FACTORY A2 WITH (NOLOCK) ON A1.FACTORYID = A2.ID 
LEFT JOIN COUNTRY A3 WITH (NOLOCK) ON A2.COUNTRYID = A3.ID 
OUTER APPLY (select sum(ShipQty) as sQty 
             from Pullout_Detail pd 
             where pd.OrderID = A1.ID and pd.OrderShipmodeSeq = Order_QS.Seq and pd.PulloutDate <= Order_QS.FtyKPI ) opd
OUTER APPLY (select top 1 PulloutDate 
			from Pullout_Detail pd 
			where pd.OrderID = A1.ID and pd.OrderShipmodeSeq = Order_QS.Seq 
Order by pulloutDate desc) pd 
WHERE 1= 1 and opd.sQty <>0  ";
                    if (dateFactoryKPIDate.Value1 != null)
                        strSQL += string.Format(" AND Order_QS.FtyKPI >= '{0}' ", dateFactoryKPIDate.Value1.Value.ToString("yyyy-MM-dd"));
                    if (dateFactoryKPIDate.Value2 != null)
                        strSQL += string.Format(" AND Order_QS.FtyKPI <= '{0}' ", dateFactoryKPIDate.Value2.Value.ToString("yyyy-MM-dd"));
                    //補上 Orders.Cateory and Factory.Type 判斷
                    if (radioBulk.Checked)
                    {
                        if (MyUtility.Check.Empty(txtFactory.Text)) //factory沒值
                        {
                            strSQL +=(" AND A1.FACTORYID IN ( select ID from Factory where KPICode!='' and Type='B' and KPICode in (select distinct ID from Factory where KPICode!=''))");
                        }
                        else  //factory有值
                        {
                            strSQL +=(string.Format(" AND A1.FACTORYID IN (select KPICode from Factory where ID='{0}' and Type='B')", txtFactory.Text));
                        }
                        strSQL +=(" AND A1.Category='B'");
                    }
                    else if (radioSample.Checked)
                    {
                        if (MyUtility.Check.Empty(txtFactory.Text)) //factory沒值
                        {
                            strSQL +=(" AND A1.FACTORYID IN ( select ID from Factory where KPICode!='' and Type='S' and KPICode in (select distinct ID from Factory where KPICode!=''))");
                        }
                        else  //factory有值
                        {
                            strSQL +=(string.Format(" AND A1.FACTORYID IN (select KPICode from Factory where ID='{0}' and Type='S')", txtFactory.Text));
                        }
                        strSQL +=(" AND A1.Category='S'");
                    }
                    strSQL += @" 
ORDER BY A1.ID";
                    result = DBProxy.Current.Select(null, strSQL, null, out gdtPullOut);
                    if (!result) return result;

                    #endregion On time Order List by PullOut

                    #region Fail Detail
                    strSQL = @" 
SELECT distinct A =A2.CountryID 
,B = A2.KpiCode 
,C = A1.FactoryID  
,D = A1.ID 
,E = Convert(varchar,cast(Order_QS.FtyKPI as date))  
,F = (SELECT strData+',' FROM (SELECT Convert(varchar, Order_QtyShip.ShipmodeID) + '-' + Convert(varchar, Order_QtyShip.Qty) + '(' + REPLACE(Convert(varchar, Order_QtyShip.BuyerDelivery),'-','/') + ')' as strData FROM Order_QtyShip WITH (NOLOCK) where id = A1.ID) t for xml path('')) 
,G = Order_QS.QTY 
,H = isnull(opd.sQty,0) 
,I = Convert(varchar,pd.PulloutDate ) 
,J = (SELECT distinct oq.ShipmodeID+','
    from Order_QtyShip oq WITH (NOLOCK) 
    where oq.id=a1.id 
    for xml path('')) 
,K = concat(Order_QS.ReasonID,'') 
,L = case a1.Category when 'B' then r.Name
           when 'S' then rs.Name
           else '' end                                               
FROM ORDERS A1 WITH (NOLOCK) 
LEFT JOIN FACTORY A2 WITH (NOLOCK) ON A1.FACTORYID = A2.ID 
LEFT JOIN COUNTRY A3 WITH (NOLOCK) ON A2.COUNTRYID = A3.ID 
LEFT JOIN PullOut_Detail A4 WITH (NOLOCK) ON A1.ID = A4.ORDERID 
LEFT JOIN Order_QtyShip Order_QS WITH (NOLOCK) ON Order_QS.id=A1.ID
LEFT JOIN Reason r on r.id = Order_QS.ReasonID and r.ReasonTypeID = 'Order_BuyerDelivery'          
LEFT JOIN Reason rs on rs.id = Order_QS.ReasonID and r.ReasonTypeID = 'Order_BuyerDelivery_sample'
OUTER APPLY (select sum(ShipQty) as sQty 
             from Pullout_Detail pd 
             where pd.OrderID = A1.ID and pd.OrderShipmodeSeq = Order_QS.Seq and pd.PulloutDate > Order_QS.FtyKPI ) opd
OUTER APPLY (select top 1 PulloutDate 
from Pullout_Detail pd where pd.OrderID = A1.ID and pd.OrderShipmodeSeq = Order_QS.Seq 
Order by pulloutDate desc) pd
WHERE 1= 1
and (opd.sQty > 0 or pd.PulloutDate is null)  
";
                    if (dateFactoryKPIDate.Value1 != null)
                        strSQL += string.Format(" AND Order_QS.FtyKPI >= '{0}' ", dateFactoryKPIDate.Value1.Value.ToString("yyyy-MM-dd"));
                    if (dateFactoryKPIDate.Value2 != null)
                        strSQL += string.Format(" AND Order_QS.FtyKPI <= '{0}' ", dateFactoryKPIDate.Value2.Value.ToString("yyyy-MM-dd"));
                    //補上 Orders.Cateory and Factory.Type 判斷
                    if (radioBulk.Checked)
                    {
                        if (MyUtility.Check.Empty(txtFactory.Text)) //factory沒值
                        {
                            strSQL += (" AND A1.FACTORYID IN ( select ID from Factory where KPICode!='' and Type='B' and KPICode in (select distinct ID from Factory where KPICode!=''))");
                        }
                        else  //factory有值
                        {
                            strSQL +=(string.Format(" AND A1.FACTORYID IN (select KPICode from Factory where ID='{0}' and Type='B')", txtFactory.Text));
                        }
                        strSQL +=(" AND A1.Category='B'");
                    }
                    else if (radioSample.Checked)
                    {
                        if (MyUtility.Check.Empty(txtFactory.Text)) //factory沒值
                        {
                            strSQL +=(" AND A1.FACTORYID IN ( select ID from Factory where KPICode!='' and Type='S' and KPICode in (select distinct ID from Factory where KPICode!=''))");
                        }
                        else  //factory有值
                        {
                            strSQL +=(string.Format(" AND A1.FACTORYID IN (select KPICode from Factory where ID='{0}' and Type='S')", txtFactory.Text));
                        }
                        strSQL +=(" AND A1.Category='S'");
                    }
                    strSQL += @" 
ORDER BY A1.ID";
                    result = DBProxy.Current.Select(null, strSQL, null, out gdtFailDetail);
                    if (!result) return result;

                    #endregion Fail Detail

                    #region Fail Order List by SP
                    strSQL = @" 
SELECT   A= A2.CountryID 
		,B = A2.KpiCode
		,C = A1.FactoryID 
		,D = A1.ID 
		,E = A1.BRANDID
		,F = Convert(varchar,Order_QS.BuyerDelivery)  
		,G = Convert(varchar,cast(Order_QS.FtyKPI as date)) 
		,H = (SELECT strData+',' FROM (SELECT Convert(varchar, Order_QtyShip.ShipmodeID) + '-' + Convert(varchar, Order_QtyShip.Qty) + '(' + REPLACE(Convert(varchar, Order_QtyShip.BuyerDelivery),'-','/') + ')' as strData FROM Order_QtyShip WITH (NOLOCK) where id = A1.ID) t for xml path('')) 
		,I = Order_QS.QTY  
		,J = Sum(A4.ShipQty) 
		,K = ISNULL(Sum(A5.ShipQty),0) 
		,L = (select strData+',' from (Select REPLACE(convert(varchar,PulloutDate),'-','/') as strData from Pullout_Detail WITH (NOLOCK) where OrderID = A1.ID)t for xml path('')) 
		,M = t.strData 
		,N = (Select Count(id) as CountPullOut from Pullout_Detail WITH (NOLOCK) where OrderID = A1.ID) 
		,O = CASE WHEN A1.GMTComplete   = 'C' OR A1.GMTComplete   = 'S' THEN 'Y' ELSE '' END 
		,P = A1.KPIChangeReason 
		,Q = (select TOP 1 Name from Reason WITH (NOLOCK) where ReasonTypeID = 'Order_BuyerDelivery' and ID = A1.KPIChangeReason ) 
		,R = dbo.getTPEPass1(A1.MRHandle)+vs1.ExtNo  
		,S = dbo.getTPEPass1(A1.SMR)+vs2.ExtNo  
		,T = dbo.getTPEPass1(A6.POHandle)+vs3.ExtNo  
		,U = dbo.getTPEPass1(A6.POSMR)+vs4.ExtNo  
FROM ORDERS A1 WITH (NOLOCK) 
LEFT JOIN Order_QtyShip Order_QS WITH (NOLOCK) ON Order_QS.id=A1.ID
LEFT JOIN Pullout_Detail AP WITH (NOLOCK) ON A1.ID =AP.OrderID
LEFT JOIN FACTORY A2 WITH (NOLOCK) ON A1.FACTORYID = A2.ID 
LEFT JOIN COUNTRY A3 WITH (NOLOCK) ON A2.COUNTRYID = A3.ID 
LEFT JOIN PullOut_Detail A4 WITH (NOLOCK) ON A1.ID = A4.ORDERID AND A4.PullOutDate <= Order_QS.FtyKPI AND A4.UKey=AP.UKey
LEFT JOIN PO A6 WITH (NOLOCK) ON A1.POID = A6.ID
OUTER APPLY(
SELECT  ShipQty FROM PullOut_Detail WITH (NOLOCK) 
WHERE A1.ID = ORDERID AND PullOutDate > Order_QS.FtyKPI
AND UKey=AP.UKey
UNION 
SELECT  Qty AS ShipQty FROM Orders WITH (NOLOCK) 
WHERE A1.ID = ID AND KPIChangeReason=0005
AND NOT EXISTS(SELECT 1 FROM   pullout_detail 
          WHERE  orderid = A1.id)
) A5
OUTER APPLY(
  select strData =stuff(( 
    Select DISTINCT concat(',',ShipmodeID)
    from Order_QtyShip 
    WITH (NOLOCK) where id = A1.ID  Group by ShipModeID
    for xml path('')
  ),1,1,'')
) t
outer apply (SELECT ' #'+ExtNo AS ExtNo from dbo.TPEPASS1 a WITH (NOLOCK) where a.ID= A1.MRHandle ) vs1
outer apply (SELECT ' #'+ExtNo AS ExtNo from dbo.TPEPASS1 a WITH (NOLOCK) where a.ID= A1.SMR ) vs2
outer apply (SELECT ' #'+ExtNo AS ExtNo from dbo.TPEPASS1 a WITH (NOLOCK) where a.ID= A6.POHandle ) vs3
outer apply (SELECT ' #'+ExtNo AS ExtNo from dbo.TPEPASS1 a WITH (NOLOCK) where a.ID= A6.POSMR ) vs4
 WHERE 1= 1  ";
                    if (dateFactoryKPIDate.Value1 != null)
                        strSQL += string.Format(" AND Order_QS.FtyKPI >= '{0}' ", dateFactoryKPIDate.Value1.Value.ToString("yyyy-MM-dd"));
                    if (dateFactoryKPIDate.Value2 != null)
                        strSQL += string.Format(" AND Order_QS.FtyKPI <= '{0}' ", dateFactoryKPIDate.Value2.Value.ToString("yyyy-MM-dd"));
                    //補上　Orders.Category and Factory.Type 判斷
                    if (radioBulk.Checked)
                    {                        
                        if (MyUtility.Check.Empty(txtFactory.Text))//factory沒值
                        {                            
                            strSQL += " AND A1.FACTORYID IN ( select ID from Factory where KPICode!='' and Type='B' and KPICode in (select distinct ID from Factory where KPICode!='') ) ";
                        }
                        else
                        {                            
                            strSQL += string.Format(" AND A1.FACTORYID IN ( select KPICode from Factory where ID='{0}' and Type='B' ) ", txtFactory.Text);
                        }
                        strSQL += " AND A1.Category='B'";
                    }
                    if (radioSample.Checked)
                    {
                        if (MyUtility.Check.Empty(txtFactory.Text))//factory沒值
                        {
                            strSQL += " AND A1.FACTORYID IN ( select ID from Factory where KPICode!='' and Type='S' and KPICode in (select distinct ID from Factory where KPICode!='') ) ";
                        }
                        else
                        {
                            strSQL += string.Format(" AND A1.FACTORYID IN ( select KPICode from Factory where ID='{0}' and Type='S' ) ", txtFactory.Text);
                        }
                        strSQL += " AND A1.Category='S'";
                    }                 

                    strSQL += @" GROUP BY A2.CountryID,  A2.KpiCode, A1.FactoryID , A1.ID, A1.BRANDID,A1.KPIChangeReason
, Order_QS.BuyerDelivery, Order_QS.FtyKPI, Order_QS.QTY 
, CASE WHEN A1.GMTComplete   = 'C' OR A1.GMTComplete   = 'S' THEN 'Y' ELSE '' END
, A1.MRHandle, A1.SMR, A6.POHandle, A6.POSMR,t.strData,vs1.ExtNo ,vs2.ExtNo,vs3.ExtNo,vs4.ExtNo
                                                        HAVING Sum(A5.ShipQty) > 0 ";
                    strSQL += @" 
ORDER BY A1.ID";
                    result = DBProxy.Current.Select(null, strSQL, null, out gdtSP);
                    if (!result) return result;

                    #endregion Fail Order List by SP
                }

                #region 產生EXCEL
                if (!(result = transferToExcel()))
                    return result;
                #endregion 

            }
            catch (Exception ex)
            {
                return new DualResult(false, "data loading error.", ex);
            }
            return result;
        }


        private DualResult transferToExcel()
        {
            DualResult result = Result.True;
            //string strPath = PrivUtils.getPath_XLT(System.Windows.Forms.Application.StartupPath);
            string temfile = "";

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

                int intRowsCount = gdtDatas.Rows.Count;
                int intRowsStart = 2;//匯入起始位置
                int rownum = intRowsStart; //每筆資料匯入之位置 
                int intColumns = 6;//匯入欄位數
                int intWorkIndex = 2;
                int row = 0;
                string[] aryAlpha = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
                object[,] objArray = new object[1, intColumns];//每列匯入欄位區間
                #region 將資料放入陣列並寫入Excel範例檔
                for (int i = 0; i < intRowsCount; i += 1)
                {
                    //this.progressBar1.Value = i + 1;
                    DataRow dr = gdtDatas.Rows[i];
                    //string Ukey = dr["Ukey"].ToString();
                    //關聯參考資料 Key1
                    //string sKey = dr["ID"].ToString();
                    //end
                    for (int k = 0; k < intColumns; k++)
                    {
                        objArray[0, k] = "";
                    }

                    objArray[0, 0] = dr["A"];
                    objArray[0, 1] = dr["B"];
                    objArray[0, 2] = dr["C"];
                    objArray[0, 3] = dr["D"];
                    objArray[0, 4] = dr["E"];
                    objArray[0, 5] = dr["F"];

                    worksheet.Range[String.Format("A{0}:F{0}", rownum + i)].Value2 = objArray;
                    row++;
                }
                worksheet.Cells[row + 2, 1] = "Total:";
                worksheet.Cells[row + 2, 2] = " ";
                worksheet.Cells[row + 2, 3] = string.Format("=SUM(C2:C{0})", MyUtility.Convert.GetString(row + 1));
                worksheet.Cells[row + 2, 4] = string.Format("=SUM(D2:D{0})", MyUtility.Convert.GetString(row + 1));
                worksheet.Cells[row + 2, 5] = string.Format("=SUM(E2:E{0})", MyUtility.Convert.GetString(row + 1));
                worksheet.Cells[row + 2, 6] = string.Format("=(D{0}/C{0})", MyUtility.Convert.GetString(row + 1));
                worksheet.Range[String.Format("A{0}:F{1}", 2, row + 1)].Borders.Color = Color.Black;                
                //worksheet.Range["E2:E3", "E2:E3"].NumberFormatLocal= "#,##0_ ";
                excel.Worksheets[1].Columns(5).NumberFormatlocal = "#,##0_ ";
                if ((gdtSP != null) && (gdtSP.Rows.Count > 0))
                {
                    //if (excel.ActiveWorkbook.Worksheets.Count >= intWorkIndex)
                    //    worksheet = excel.ActiveWorkbook.Worksheets[intWorkIndex];
                    //else
                    //    worksheet = excel.ActiveWorkbook.Worksheets.Add(Type.Missing, (worksheet == null ? Type.Missing : worksheet), Type.Missing, XlSheetType.xlWorksheet);
                    //intWorkIndex++;
                    worksheet= excel.ActiveWorkbook.Worksheets[2];
                    worksheet.Name = "Fail Order List by SP";
                    string[] aryTitles = new string[] { "Country", "KPI Group", "Factory", "SP No", "Brand", "Buyer Delivery", "Factory KPI", "Delivery By Shipmode ", "Order Qty", "On Time Qty", "Fail Qty", "Fail PullOut Date", "ShipMode", "[P]", "Garment Complete", "ReasonID", "Order Reason", "Handle", "SMR", "PO Handle", "PO SMR" };
                    object[,] objArray_1 = new object[1, aryTitles.Length];
                    for (int intIndex = 0; intIndex < aryTitles.Length; intIndex++)
                    {
                        objArray_1[0, intIndex] = aryTitles[intIndex];
                    }
                    worksheet.Range[String.Format("A{0}:{1}{0}", 1, aryAlpha[aryTitles.Length - 1])].Value2 = objArray_1;
                    worksheet.Range[String.Format("A{0}:{1}{0}", 1, aryAlpha[aryTitles.Length - 1])].AutoFilter(1); //篩選
                    worksheet.Range[String.Format("A{0}:{1}{0}", 1, aryAlpha[aryTitles.Length - 1])].Interior.Color = Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(204)))));
                    worksheet.Range[String.Format("A{0}:{1}{0}", 1, aryAlpha[aryTitles.Length - 1])].Borders.Color = Color.Black;

                    //  excelRange.EntireColumn.AutoFit();
                    int rc = gdtSP.Rows.Count;
                    for (int intIndex = 0; intIndex < rc; intIndex++)
                    {

                        for (int intIndex_0 = 0; intIndex_0 < aryTitles.Length; intIndex_0++)
                        {
                            objArray_1[0, intIndex_0] = gdtSP.Rows[intIndex][aryAlpha[intIndex_0]].ToString();
                        }
                        worksheet.Range[String.Format("A{0}:{1}{0}", intIndex + 2, aryAlpha[aryTitles.Length - 1])].Value2 = objArray_1;
                        worksheet.Range[String.Format("A{0}:{1}{0}", intIndex + 2, aryAlpha[aryTitles.Length - 1])].EntireColumn.AutoFit(); //自動調整欄寬
                    }

                    worksheet.Cells[rc + 2, 2] = "Total:";
                    worksheet.Cells[rc + 2, 9] = string.Format("=SUM(I2:I{0})", MyUtility.Convert.GetString(rc + 1));
                    worksheet.Cells[rc + 2, 10] = string.Format("=SUM(J2:J{0})", MyUtility.Convert.GetString(rc + 1));
                    worksheet.Cells[rc + 2, 11] = string.Format("=SUM(K2:K{0})", MyUtility.Convert.GetString(rc + 1));
                    //設定分割列數
                    excel.ActiveWindow.SplitRow = 1;
                    // 進行凍結視窗
                    excel.ActiveWindow.FreezePanes = true;
                }

                if (checkExportDetailData.Checked)
                {
                    if ((gdtOrderDetail != null) && (gdtOrderDetail.Rows.Count > 0))
                    {
                        //if (excel.ActiveWorkbook.Worksheets.Count >= intWorkIndex)
                        //    worksheet = excel.ActiveWorkbook.Worksheets[intWorkIndex];
                        //else
                        //    worksheet = excel.ActiveWorkbook.Worksheets.Add(Type.Missing, (worksheet == null ? Type.Missing : worksheet), Type.Missing, XlSheetType.xlWorksheet);
                        //intWorkIndex++;
                        worksheet = excel.ActiveWorkbook.Worksheets[3];
                        worksheet.Name = "Order Detail";
                        string[] aryTitles = new string[] { "Country", "KPI Group", "Factory", "SP No", "Brand", "Buyer Delivery", "Factory KPI", "Delivery By Shipmode", "Order Qty", "On Time Qty", "Fail Qty", "PullOut Date", "ShipMode", "[P]", "Garment Complete", "ReasonID", "Order Reason", "Handle  ", "SMR", "PO Handle", "PO SMR" };
                        object[,] objArray_1 = new object[1, aryTitles.Length];
                        for (int intIndex = 0; intIndex < aryTitles.Length; intIndex++)
                        {
                            objArray_1[0, intIndex] = aryTitles[intIndex];
                        }
                        worksheet.Range[String.Format("A{0}:{1}{0}", 1, aryAlpha[aryTitles.Length - 1])].Value2 = objArray_1;
                        worksheet.Range[String.Format("A{0}:{1}{0}", 1, aryAlpha[aryTitles.Length - 1])].AutoFilter(1); //篩選
                        worksheet.Range[String.Format("A{0}:{1}{0}", 1, aryAlpha[aryTitles.Length - 1])].Interior.Color = Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(204)))));
                        worksheet.Range[String.Format("A{0}:{1}{0}", 1, aryAlpha[aryTitles.Length - 1])].Borders.Color = Color.Black;
                        int rc = gdtOrderDetail.Rows.Count;
                        //  excel.ActiveSheet.Columns(6).NumberFormatlocal = "yyyy/MM/dd";
                        //  excel.ActiveSheet.Columns(7).NumberFormatlocal = "yyyy/MM/dd"; 
                        for (int intIndex = 0; intIndex < rc; intIndex++)
                        {
                            for (int intIndex_0 = 0; intIndex_0 < aryTitles.Length; intIndex_0++)
                            {
                                objArray_1[0, intIndex_0] = gdtOrderDetail.Rows[intIndex][aryAlpha[intIndex_0]].ToString();
                            }
                            worksheet.Range[String.Format("A{0}:{1}{0}", intIndex + 2, aryAlpha[aryTitles.Length - 1])].Value2 = objArray_1;
                            worksheet.Range[String.Format("A{0}:{1}{0}", intIndex + 2, aryAlpha[aryTitles.Length - 1])].EntireColumn.AutoFit();//自動調整欄寬
                            worksheet.Range[String.Format("F{0}:G{0}", intIndex + 2)].NumberFormatLocal = "yyyy/MM/dd";
                        }

                        worksheet.Cells[rc + 2, 2] = "Total:";
                        worksheet.Cells[rc + 2, 9] = string.Format("=SUM(I2:I{0})", MyUtility.Convert.GetString(rc + 1));
                        worksheet.Cells[rc + 2, 10] = string.Format("=SUM(J2:J{0})", MyUtility.Convert.GetString(rc + 1));
                        worksheet.Cells[rc + 2, 11] = string.Format("=SUM(K2:K{0})", MyUtility.Convert.GetString(rc + 1));
                        //設定分割列數
                        excel.ActiveWindow.SplitRow = 1;
                        // 進行凍結視窗
                        excel.ActiveWindow.FreezePanes = true;
                    }
                    if ((gdtPullOut != null) && (gdtPullOut.Rows.Count > 0))
                    {
                        //if (excel.ActiveWorkbook.Worksheets.Count >= intWorkIndex)
                        //    worksheet = excel.ActiveWorkbook.Worksheets[intWorkIndex];
                        //else
                        //    worksheet = excel.ActiveWorkbook.Worksheets.Add(Type.Missing, (worksheet == null ? Type.Missing : worksheet), Type.Missing, XlSheetType.xlWorksheet);
                        //intWorkIndex++;
                        worksheet = excel.ActiveWorkbook.Worksheets[4];
                        worksheet.Name = "On time Order List by PullOut";
                        string[] aryTitles = new string[] { "Country", "KPI Group", "Factory", "SP No", "Factory KPI", "Delivery By Shipmode", "Order Qty", "PullOut Qty", "PullOut Date", "ShipMode" };
                        object[,] objArray_1 = new object[1, aryTitles.Length];
                        for (int intIndex = 0; intIndex < aryTitles.Length; intIndex++)
                        {
                            objArray_1[0, intIndex] = aryTitles[intIndex];
                        }
                        worksheet.Range[String.Format("A{0}:{1}{0}", 1, aryAlpha[aryTitles.Length - 1])].Value2 = objArray_1;
                        worksheet.Range[String.Format("A{0}:{1}{0}", 1, aryAlpha[aryTitles.Length - 1])].AutoFilter(1); //篩選
                        worksheet.Range[String.Format("A{0}:{1}{0}", 1, aryAlpha[aryTitles.Length - 1])].Interior.Color = Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(204)))));
                        worksheet.Range[String.Format("A{0}:{1}{0}", 1, aryAlpha[aryTitles.Length - 1])].Borders.Color = Color.Black;
                        excel.ActiveWorkbook.Worksheets[4].Columns(5).NumberFormatlocal = "yyyy/MM/dd";
                        excel.ActiveSheet.Columns(9).NumberFormatlocal = "yyyy/MM/dd";

                        int rc = gdtPullOut.Rows.Count;
                        for (int intIndex = 0; intIndex < rc; intIndex++)
                        {
                            for (int intIndex_0 = 0; intIndex_0 < aryTitles.Length; intIndex_0++)
                            {
                                objArray_1[0, intIndex_0] = gdtPullOut.Rows[intIndex][aryAlpha[intIndex_0]].ToString();
                            }
                            worksheet.Range[String.Format("A{0}:{1}{0}", intIndex + 2, aryAlpha[aryTitles.Length - 1])].Value2 = objArray_1;
                            worksheet.Range[String.Format("A{0}:{1}{0}", intIndex + 2, aryAlpha[aryTitles.Length - 1])].EntireColumn.AutoFit(); //自動調整欄寬
                        }

                        worksheet.Cells[rc + 2, 2] = "Total:";
                        worksheet.Cells[rc + 2, 7] = string.Format("=SUM(G2:G{0})", MyUtility.Convert.GetString(rc + 1));
                        worksheet.Cells[rc + 2, 8] = string.Format("=SUM(H2:H{0})", MyUtility.Convert.GetString(rc + 1));
                        //設定分割列數
                        excel.ActiveWindow.SplitRow = 1;
                        // 進行凍結視窗
                        excel.ActiveWindow.FreezePanes = true;
                    }
                    if ((gdtFailDetail != null) && (gdtFailDetail.Rows.Count > 0))
                    {
                        //if (excel.ActiveWorkbook.Worksheets.Count >= intWorkIndex)
                        //    worksheet = excel.ActiveWorkbook.Worksheets[intWorkIndex];
                        //else
                        //    worksheet = excel.ActiveWorkbook.Worksheets.Add(Type.Missing, (worksheet == null ? Type.Missing : worksheet), Type.Missing, XlSheetType.xlWorksheet);
                        //intWorkIndex++;
                        worksheet = excel.ActiveWorkbook.Worksheets[5];
                        worksheet.Name = "Fail Detail";
                        string[] aryTitles = new string[] { "Country", "KPI Group", "Factory", "SP No", "Factory KPI", "Delivery By Shipmode", "Order Qty", "Fail Qty", "PullOut Date", "ShipMode", "ReasonID", "Order Reason" };
                        object[,] objArray_1 = new object[1, aryTitles.Length];
                        for (int intIndex = 0; intIndex < aryTitles.Length; intIndex++)
                        {
                            objArray_1[0, intIndex] = aryTitles[intIndex];
                        }
                        worksheet.get_Range("K:K", Type.Missing).NumberFormatLocal = "@";
                        worksheet.Range[String.Format("A{0}:{1}{0}", 1, aryAlpha[aryTitles.Length - 1])].Value2 = objArray_1;
                        worksheet.Range[String.Format("A{0}:{1}{0}", 1, aryAlpha[aryTitles.Length - 1])].AutoFilter(1); //篩選
                        worksheet.Range[String.Format("A{0}:{1}{0}", 1, aryAlpha[aryTitles.Length - 1])].Interior.Color = Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(204)))));
                        worksheet.Range[String.Format("A{0}:{1}{0}", 1, aryAlpha[aryTitles.Length - 1])].Borders.Color = Color.Black;
                        excel.ActiveWorkbook.Worksheets[5].Columns(5).NumberFormatlocal = "yyyy/MM/dd";
                        excel.ActiveSheet.Columns(9).NumberFormatlocal = "yyyy/MM/dd";
                        int rc = gdtFailDetail.Rows.Count;
                        for (int intIndex = 0; intIndex < rc; intIndex++)
                        {

                            for (int intIndex_0 = 0; intIndex_0 < aryTitles.Length; intIndex_0++)
                            {
                                objArray_1[0, intIndex_0] = gdtFailDetail.Rows[intIndex][aryAlpha[intIndex_0]].ToString();
                            }
                            worksheet.Range[String.Format("A{0}:{1}{0}", intIndex + 2, aryAlpha[aryTitles.Length - 1])].Value2 = objArray_1;
                            worksheet.Range[String.Format("A{0}:{1}{0}", intIndex + 2, aryAlpha[aryTitles.Length - 1])].EntireColumn.AutoFit(); //自動調整欄寬
                        }

                        worksheet.Cells[rc + 2, 2] = "Total:";
                        worksheet.Cells[rc + 2, 7] = string.Format("=SUM(G2:G{0})", MyUtility.Convert.GetString(rc + 1));
                        worksheet.Cells[rc + 2, 8] = string.Format("=SUM(H2:H{0})", MyUtility.Convert.GetString(rc + 1));
                        //設定分割列數
                        excel.ActiveWindow.SplitRow = 1;
                        // 進行凍結視窗
                        excel.ActiveWindow.FreezePanes = true;
                    }
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
                if (null != excel) excel.Quit();
                return new DualResult(false, "Export excel error.", ex);
            }
        }
    }
}
