using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Sci.Data;
using Ict;
using Sci.Win;
using System.Runtime.InteropServices;

namespace Sci.Production.Planning
{
    /// <summary>
    /// R17
    /// </summary>
    public partial class R17 : Sci.Win.Tems.PrintForm
    {
        private DataTable gdtOrderDetail;
        private DataTable gdtPullOut;
        private DataTable gdtFailDetail;
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
SELECT   A = A2.CountryID
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
       , J = Cast(isnull(pd.Qty,0) as int)
	   , K = Cast(isnull(pd.FailQty,0) as int)
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
	   , P= Order_QS.ReasonID
	   , Q = case A1.Category when 'B' then r.Name
		 when 'S' then rs.Name
		 else '' end 
       , R = dbo.getTPEPass1(A1.MRHandle) + vs1.ExtNo
       , S = dbo.getTPEPass1(A1.SMR)+vs2.ExtNo
       , T = dbo.getTPEPass1(A6.POHandle)+vs3.ExtNo
       , U = dbo.getTPEPass1(A6.POSMR)+vs4.ExtNo
FROM ORDERS A1 WITH (NOLOCK) 
inner join OrderType ot with (NoLock) on A1.BrandID = ot.BrandID
                                         and A1.OrderTypeID = ot.ID
LEFT JOIN Pullout_Detail AP WITH (NOLOCK) ON A1.ID =AP.OrderID
LEFT JOIN FACTORY A2 WITH (NOLOCK) ON A1.FACTORYID = A2.ID 
LEFT JOIN COUNTRY A3 WITH (NOLOCK) ON A2.COUNTRYID = A3.ID 
LEFT JOIN Order_QtyShip Order_QS WITH (NOLOCK) ON Order_QS.id=A1.ID
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
outer apply (
	Select 
		Qty = Sum(rA.Qty) - dbo.getInvAdjQtyByDate( A1.ID ,Order_QS.Seq,Order_QS.FtyKPI,'<='),
		FailQty = Sum(rB.Qty)  - dbo.getInvAdjQtyByDate( A1.ID ,Order_QS.Seq,Order_QS.FtyKPI,'>')
	From Pullout_Detail pd
	Outer apply (Select Qty = IIF(pd.PulloutDate <= Order_QS.FtyKPI, pd.shipqty, 0)) rA
	Outer apply (Select Qty = IIF(pd.PulloutDate >  Order_QS.FtyKPI, pd.shipqty, 0)) rB
	where pd.OrderID = A1.ID 
	and pd.OrderShipmodeSeq = Order_QS.Seq
) pd
WHERE ot.IsGMTMaster = 1";
                if (this.dateFactoryKPIDate.Value1 != null)
                {
                    strSQL += string.Format(" AND Order_QS.FtyKPI >= '{0}' ", this.dateFactoryKPIDate.Value1.Value.ToString("yyyy-MM-dd"));
                }

                if (this.dateFactoryKPIDate.Value2 != null)
                {
                    strSQL += string.Format(" AND Order_QS.FtyKPI <= '{0}' ", this.dateFactoryKPIDate.Value2.Value.ToString("yyyy-MM-dd"));
                }

                // 補上 Orders.Cateory and Factory.Type 判斷
                if (this.radioBulk.Checked)
                {
                    if (MyUtility.Check.Empty(this.txtFactory.Text))
                    {
                        strSQL += " AND A1.FACTORYID IN ( select ID from Factory where KPICode!='' and Type='B' and KPICode in (select distinct ID from Factory where KPICode!=''))";
                    }
                    else
                    {
                        strSQL += string.Format(" AND A1.FACTORYID IN (select KPICode from Factory where ID='{0}' and Type='B')", this.txtFactory.Text);
                    }

                    strSQL += " AND A1.Category in ('B', 'G')";
                }
                else if (this.radioSample.Checked)
                {
                    if (MyUtility.Check.Empty(this.txtFactory.Text))
                    {
                        strSQL += " AND A1.FACTORYID IN ( select ID from Factory where KPICode!='' and Type='S' and KPICode in (select distinct ID from Factory where KPICode!=''))";
                    }
                    else
                    {
                        strSQL += string.Format(" AND A1.FACTORYID IN (select KPICode from Factory where ID='{0}' and Type='S')", this.txtFactory.Text);
                    }

                    strSQL += " AND A1.Category='S'";
                }

                strSQL += @" 
GROUP BY A2.CountryID,  A2.KpiCode, A1.FactoryID , A1.ID, A1.BRANDID
, A1.BuyerDelivery, Order_QS.FtyKPI, Order_QS.QTY 
, CASE WHEN A1.GMTComplete   = 'C' OR A1.GMTComplete   = 'S' THEN 'Y' ELSE '' END
, A1.MRHandle, A1.SMR, A6.POHandle, A6.POSMR,t.strData,vs1.ExtNo ,vs2.ExtNo,vs3.ExtNo,vs4.ExtNo 
,Order_QS.ReasonID,A1.Category,r.Name,rs.Name,PD.Qty,PD.FailQty ";
                strSQL += @" 
ORDER BY A1.ID";
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
                strSQL = @" SELECT  '' AS A,  '' AS B, 0 AS C, 0 AS D, 0 AS E, 0.00 AS F FROM ORDERS WHERE 1 = 0 ";
                result = DBProxy.Current.Select(null, strSQL, null, out this.gdtSDP);
                if (!result)
                {
                    return result;
                }

                List<string> lstSDP = new List<string>();
                for (int intIndex = 0; intIndex < this.gdtOrderDetail.Rows.Count; intIndex++)
                {
                    DataRow drData = this.gdtOrderDetail.Rows[intIndex];
                    string handleName = string.Empty;
                    #region Calc SDP Data
                    int intIndex_SDP = lstSDP.IndexOf(drData["B"].ToString() + "___" + drData["A"].ToString());
                    DataRow drSDP;
                    if (intIndex_SDP < 0)
                    {
                        drSDP = this.gdtSDP.NewRow();
                        drSDP["A"] = drData["B"].ToString();
                        drSDP["B"] = MyUtility.GetValue.Lookup("ALIAS", drData["A"].ToString(), "Country", "ID");
                        this.gdtSDP.Rows.Add(drSDP);
                        lstSDP.Add(drData["B"].ToString() + "___" + drData["A"].ToString()); // A
                    }
                    else
                    {
                        drSDP = this.gdtSDP.Rows[intIndex_SDP];
                    }

                    drSDP["C"] = (drSDP["C"].ToString() != string.Empty ? Convert.ToDecimal(drSDP["C"].ToString()) : 0) + (drData["I"].ToString() != string.Empty ? Convert.ToDecimal(drData["I"].ToString()) : 0);
                    drSDP["D"] = (drSDP["D"].ToString() != string.Empty ? Convert.ToDecimal(drSDP["D"].ToString()) : 0) + (drData["J"].ToString() != string.Empty ? Convert.ToDecimal(drData["J"].ToString()) : 0);
                    drSDP["E"] = (drSDP["E"].ToString() != string.Empty ? Convert.ToDecimal(drSDP["E"].ToString()) : 0) + (drData["K"].ToString() != string.Empty ? Convert.ToDecimal(drData["K"].ToString()) : 0);
                    drSDP["F"] = drSDP["C"].ToString() == "0" ? 0 : Convert.ToDecimal(drSDP["D"].ToString()) / Convert.ToDecimal(drSDP["C"].ToString()) * 100;
                    #endregion Calc SDP Data
                }
                #endregion

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
		,J = Sum(A4.ShipQty) - dbo.getInvAdjQtyByDate(A1.ID,null,Order_QS.FtyKPI,'<=')
		,K = ISNULL(Sum(A5.ShipQty),0)  - dbo.getInvAdjQtyByDate( A1.ID , null,Order_QS.FtyKPI,'>') 
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
                if (this.dateFactoryKPIDate.Value1 != null)
                {
                    strSQL += string.Format(" AND Order_QS.FtyKPI >= '{0}' ", this.dateFactoryKPIDate.Value1.Value.ToString("yyyy-MM-dd"));
                }

                if (this.dateFactoryKPIDate.Value2 != null)
                {
                    strSQL += string.Format(" AND Order_QS.FtyKPI <= '{0}' ", this.dateFactoryKPIDate.Value2.Value.ToString("yyyy-MM-dd"));
                }

                // 補上 Orders.Cateory and Factory.Type 判斷
                if (this.radioBulk.Checked)
                {
                    if (MyUtility.Check.Empty(this.txtFactory.Text))
                    {
                        strSQL += " AND A1.FACTORYID IN ( select ID from Factory where KPICode!='' and Type='B' and KPICode in (select distinct ID from Factory where KPICode!=''))";
                    }
                    else
                    {
                        strSQL += string.Format(" AND A1.FACTORYID IN (select KPICode from Factory where ID='{0}' and Type='B')", this.txtFactory.Text);
                    }

                    strSQL += " AND A1.Category='B'";
                }
                else if (this.radioSample.Checked)
                {
                    if (MyUtility.Check.Empty(this.txtFactory.Text))
                    {
                        strSQL += " AND A1.FACTORYID IN ( select ID from Factory where KPICode!='' and Type='S' and KPICode in (select distinct ID from Factory where KPICode!=''))";
                    }
                    else
                    {
                        strSQL += string.Format(" AND A1.FACTORYID IN (select KPICode from Factory where ID='{0}' and Type='S')", this.txtFactory.Text);
                    }

                    strSQL += " AND A1.Category='S'";
                }

                strSQL += @" 
GROUP BY A2.CountryID,  A2.KpiCode, A1.FactoryID , A1.ID, A1.BRANDID,A1.KPIChangeReason
, Order_QS.BuyerDelivery, Order_QS.FtyKPI, Order_QS.QTY 
, CASE WHEN A1.GMTComplete   = 'C' OR A1.GMTComplete   = 'S' THEN 'Y' ELSE '' END
, A1.MRHandle, A1.SMR, A6.POHandle, A6.POSMR,t.strData,vs1.ExtNo ,vs2.ExtNo,vs3.ExtNo,vs4.ExtNo
HAVING Sum(A5.ShipQty) > 0 ";
                strSQL += @" 
ORDER BY A1.ID";
                result = DBProxy.Current.Select(null, strSQL, null, out this.gdtSP);
                if (!result)
                {
                    return result;
                }

                #endregion Fail Order List by SP

                if (this.checkExportDetailData.Checked)
                {
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
OUTER APPLY (select sum(ShipQty)  - dbo.getInvAdjQtyByDate( A1.ID, Order_QS.SEQ,Order_QS.FtyKPI,'<=') as sQty 
             from Pullout_Detail pd 
             where pd.OrderID = A1.ID and pd.OrderShipmodeSeq = Order_QS.Seq and pd.PulloutDate <= Order_QS.FtyKPI ) opd
OUTER APPLY (select top 1 PulloutDate 
			from Pullout_Detail pd 
			where pd.OrderID = A1.ID and pd.OrderShipmodeSeq = Order_QS.Seq 
Order by pulloutDate desc) pd 
WHERE 1= 1 and opd.sQty <>0  ";
                    if (this.dateFactoryKPIDate.Value1 != null)
                    {
                        strSQL += string.Format(" AND Order_QS.FtyKPI >= '{0}' ", this.dateFactoryKPIDate.Value1.Value.ToString("yyyy-MM-dd"));
                    }

                    if (this.dateFactoryKPIDate.Value2 != null)
                    {
                        strSQL += string.Format(" AND Order_QS.FtyKPI <= '{0}' ", this.dateFactoryKPIDate.Value2.Value.ToString("yyyy-MM-dd"));
                    }

                    // 補上 Orders.Cateory and Factory.Type 判斷
                    if (this.radioBulk.Checked)
                    {
                        if (MyUtility.Check.Empty(this.txtFactory.Text))
                        {
                            strSQL += " AND A1.FACTORYID IN ( select ID from Factory where KPICode!='' and Type='B' and KPICode in (select distinct ID from Factory where KPICode!=''))";
                        }
                        else
                        {
                            strSQL += string.Format(" AND A1.FACTORYID IN (select KPICode from Factory where ID='{0}' and Type='B')", this.txtFactory.Text);
                        }

                        strSQL += " AND A1.Category='B'";
                    }
                    else if (this.radioSample.Checked)
                    {
                        if (MyUtility.Check.Empty(this.txtFactory.Text))
                        {
                            strSQL += " AND A1.FACTORYID IN ( select ID from Factory where KPICode!='' and Type='S' and KPICode in (select distinct ID from Factory where KPICode!=''))";
                        }
                        else
                        {
                            strSQL += string.Format(" AND A1.FACTORYID IN (select KPICode from Factory where ID='{0}' and Type='S')", this.txtFactory.Text);
                        }

                        strSQL += " AND A1.Category='S'";
                    }

                    strSQL += @" 
ORDER BY A1.ID";
                    result = DBProxy.Current.Select(null, strSQL, null, out this.gdtPullOut);
                    if (!result)
                    {
                        return result;
                    }

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
OUTER APPLY (select sum(ShipQty)   - dbo.getInvAdjQtyByDate( A1.ID, Order_QS.SEQ,Order_QS.FtyKPI,'>')  as sQty 
             from Pullout_Detail pd 
             where pd.OrderID = A1.ID and pd.OrderShipmodeSeq = Order_QS.Seq and pd.PulloutDate > Order_QS.FtyKPI ) opd
OUTER APPLY (select top 1 PulloutDate 
from Pullout_Detail pd where pd.OrderID = A1.ID and pd.OrderShipmodeSeq = Order_QS.Seq 
Order by pulloutDate desc) pd
WHERE 1= 1
and (opd.sQty > 0 or pd.PulloutDate is null)  
";
                    if (this.dateFactoryKPIDate.Value1 != null)
                    {
                        strSQL += string.Format(" AND Order_QS.FtyKPI >= '{0}' ", this.dateFactoryKPIDate.Value1.Value.ToString("yyyy-MM-dd"));
                    }

                    if (this.dateFactoryKPIDate.Value2 != null)
                    {
                        strSQL += string.Format(" AND Order_QS.FtyKPI <= '{0}' ", this.dateFactoryKPIDate.Value2.Value.ToString("yyyy-MM-dd"));
                    }

                    // 補上 Orders.Cateory and Factory.Type 判斷
                    if (this.radioBulk.Checked)
                    {
                        if (MyUtility.Check.Empty(this.txtFactory.Text))
                        {
                            strSQL += " AND A1.FACTORYID IN ( select ID from Factory where KPICode!='' and Type='B' and KPICode in (select distinct ID from Factory where KPICode!=''))";
                        }
                        else
                        {
                            strSQL += string.Format(" AND A1.FACTORYID IN (select KPICode from Factory where ID='{0}' and Type='B')", this.txtFactory.Text);
                        }

                        strSQL += " AND A1.Category='B'";
                    }
                    else if (this.radioSample.Checked)
                    {
                        if (MyUtility.Check.Empty(this.txtFactory.Text))
                        {
                            strSQL += " AND A1.FACTORYID IN ( select ID from Factory where KPICode!='' and Type='S' and KPICode in (select distinct ID from Factory where KPICode!=''))";
                        }
                        else
                        {
                            strSQL += string.Format(" AND A1.FACTORYID IN (select KPICode from Factory where ID='{0}' and Type='S')", this.txtFactory.Text);
                        }

                        strSQL += " AND A1.Category='S'";
                    }

                    strSQL += @" 
ORDER BY A1.ID";
                    result = DBProxy.Current.Select(null, strSQL, null, out this.gdtFailDetail);
                    if (!result)
                    {
                        return result;
                    }

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
		,J = Sum(A4.ShipQty)   - dbo.getInvAdjQtyByDate(A1.ID, null,Order_QS.FtyKPI,'<=') 
		,K = ISNULL(Sum(A5.ShipQty),0) - dbo.getInvAdjQtyByDate( A1.ID , null,Order_QS.FtyKPI,'>') 
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
                    if (this.dateFactoryKPIDate.Value1 != null)
                    {
                        strSQL += string.Format(" AND Order_QS.FtyKPI >= '{0}' ", this.dateFactoryKPIDate.Value1.Value.ToString("yyyy-MM-dd"));
                    }

                    if (this.dateFactoryKPIDate.Value2 != null)
                    {
                        strSQL += string.Format(" AND Order_QS.FtyKPI <= '{0}' ", this.dateFactoryKPIDate.Value2.Value.ToString("yyyy-MM-dd"));
                    }

                    // 補上　Orders.Category and Factory.Type 判斷
                    if (this.radioBulk.Checked)
                    {
                        if (MyUtility.Check.Empty(this.txtFactory.Text))
                        {
                            strSQL += " AND A1.FACTORYID IN ( select ID from Factory where KPICode!='' and Type='B' and KPICode in (select distinct ID from Factory where KPICode!='') ) ";
                        }
                        else
                        {
                            strSQL += string.Format(" AND A1.FACTORYID IN ( select KPICode from Factory where ID='{0}' and Type='B' ) ", this.txtFactory.Text);
                        }

                        strSQL += " AND A1.Category='B'";
                    }

                    if (this.radioSample.Checked)
                    {
                        if (MyUtility.Check.Empty(this.txtFactory.Text))
                        {
                            strSQL += " AND A1.FACTORYID IN ( select ID from Factory where KPICode!='' and Type='S' and KPICode in (select distinct ID from Factory where KPICode!='') ) ";
                        }
                        else
                        {
                            strSQL += string.Format(" AND A1.FACTORYID IN ( select KPICode from Factory where ID='{0}' and Type='S' ) ", this.txtFactory.Text);
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
                    result = DBProxy.Current.Select(null, strSQL, null, out this.gdtSP);
                    if (!result)
                    {
                        return result;
                    }

                    #endregion Fail Order List by SP
                }

                #region 產生EXCEL
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

                int intRowsCount = this.gdtSDP.Rows.Count;
                int intRowsStart = 2; // 匯入起始位置
                int rownum = intRowsStart; // 每筆資料匯入之位置
                int intColumns = 6; // 匯入欄位數
                string[] aryAlpha = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
                object[,] objArray = new object[1, intColumns]; // 每列匯入欄位區間
                #region 將資料放入陣列並寫入Excel範例檔

                #region 匯出SDP
                for (int i = 0; i < intRowsCount; i += 1)
                {
                    DataRow dr = this.gdtSDP.Rows[i];
                    for (int k = 0; k < intColumns; k++)
                    {
                        objArray[0, k] = string.Empty;
                    }

                    objArray[0, 0] = dr["A"];
                    objArray[0, 1] = dr["B"];
                    objArray[0, 2] = dr["C"];
                    objArray[0, 3] = dr["D"];
                    objArray[0, 4] = dr["E"];
                    objArray[0, 5] = dr["F"];

                    worksheet.Range[string.Format("A{0}:F{0}", rownum + i)].Value2 = objArray;
                    worksheet.Cells[rownum + i, 6].NumberFormatLocal = "0.00";
                }

                if (intRowsCount > 0)
                {
                    worksheet.Range[string.Format("A{0}:A{0}", rownum + intRowsCount)].Value2 = "Total";
                    worksheet.Range[string.Format("C{0}:C{0}", rownum + intRowsCount)].Formula = "=SUM(" + string.Format("C{0}:C{1}", 2, rownum + intRowsCount - 1) + ")";
                    worksheet.Range[string.Format("D{0}:D{0}", rownum + intRowsCount)].Formula = "=SUM(" + string.Format("D{0}:D{1}", 2, rownum + intRowsCount - 1) + ")";
                    worksheet.Range[string.Format("E{0}:E{0}", rownum + intRowsCount)].Formula = "=SUM(" + string.Format("E{0}:E{1}", 2, rownum + intRowsCount - 1) + ")";
                    worksheet.Range[string.Format("F{0}:F{0}", rownum + intRowsCount)].Formula = "=" + string.Format("D{0}/IF(C{0}=0, 1,C{0})*100", rownum + intRowsCount);
                    worksheet.Cells[rownum + intRowsCount, 6].NumberFormatLocal = "0.00";
                }
                #endregion

                #region 匯出 Fail Order List by SP Data
                if ((this.gdtSP != null) && (this.gdtSP.Rows.Count > 0))
                {
                    worksheet = excel.ActiveWorkbook.Worksheets[2];
                    worksheet.Name = "Fail Order List by SP";
                    string[] aryTitles = new string[] { "Country", "KPI Group", "Factory", "SP No", "Brand", "Buyer Delivery", "Factory KPI", "Delivery By Shipmode ", "Order Qty", "On Time Qty", "Fail Qty", "Fail PullOut Date", "ShipMode", "[P]", "Garment Complete", "ReasonID", "Order Reason", "Handle", "SMR", "PO Handle", "PO SMR" };
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
                            objArray_1[0, intIndex_0] = this.gdtSP.Rows[intIndex][aryAlpha[intIndex_0]].ToString();
                        }

                        worksheet.Range[string.Format("A{0}:{1}{0}", intIndex + 2, aryAlpha[aryTitles.Length - 1])].Value2 = objArray_1;
                        worksheet.Range[string.Format("A{0}:{1}{0}", intIndex + 2, aryAlpha[aryTitles.Length - 1])].EntireColumn.AutoFit(); // 自動調整欄寬
                    }

                    worksheet.Cells[rc + 2, 2] = "Total:";
                    worksheet.Cells[rc + 2, 9] = string.Format("=SUM(I2:I{0})", MyUtility.Convert.GetString(rc + 1));
                    worksheet.Cells[rc + 2, 10] = string.Format("=SUM(J2:J{0})", MyUtility.Convert.GetString(rc + 1));
                    worksheet.Cells[rc + 2, 11] = string.Format("=SUM(K2:K{0})", MyUtility.Convert.GetString(rc + 1));

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
                        string[] aryTitles = new string[] { "Country", "KPI Group", "Factory", "SP No", "Brand", "Buyer Delivery", "Factory KPI", "Delivery By Shipmode", "Order Qty", "On Time Qty", "Fail Qty", "PullOut Date", "ShipMode", "[P]", "Garment Complete", "ReasonID", "Order Reason", "Handle  ", "SMR", "PO Handle", "PO SMR" };
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
                                objArray_1[0, intIndex_0] = this.gdtOrderDetail.Rows[intIndex][aryAlpha[intIndex_0]].ToString();
                            }

                            worksheet.Range[string.Format("A{0}:{1}{0}", intIndex + 2, aryAlpha[aryTitles.Length - 1])].Value2 = objArray_1;
                            worksheet.Range[string.Format("A{0}:{1}{0}", intIndex + 2, aryAlpha[aryTitles.Length - 1])].EntireColumn.AutoFit(); // 自動調整欄寬
                            worksheet.Range[string.Format("F{0}:G{0}", intIndex + 2)].NumberFormatLocal = "yyyy/MM/dd";
                        }

                        worksheet.Cells[rc + 2, 2] = "Total:";
                        worksheet.Cells[rc + 2, 9] = string.Format("=SUM(I2:I{0})", MyUtility.Convert.GetString(rc + 1));
                        worksheet.Cells[rc + 2, 10] = string.Format("=SUM(J2:J{0})", MyUtility.Convert.GetString(rc + 1));
                        worksheet.Cells[rc + 2, 11] = string.Format("=SUM(K2:K{0})", MyUtility.Convert.GetString(rc + 1));

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
                        string[] aryTitles = new string[] { "Country", "KPI Group", "Factory", "SP No", "Factory KPI", "Delivery By Shipmode", "Order Qty", "PullOut Qty", "PullOut Date", "ShipMode" };
                        object[,] objArray_1 = new object[1, aryTitles.Length];
                        for (int intIndex = 0; intIndex < aryTitles.Length; intIndex++)
                        {
                            objArray_1[0, intIndex] = aryTitles[intIndex];
                        }

                        worksheet.Range[string.Format("A{0}:{1}{0}", 1, aryAlpha[aryTitles.Length - 1])].Value2 = objArray_1;
                        worksheet.Range[string.Format("A{0}:{1}{0}", 1, aryAlpha[aryTitles.Length - 1])].AutoFilter(1); // 篩選
                        worksheet.Range[string.Format("A{0}:{1}{0}", 1, aryAlpha[aryTitles.Length - 1])].Interior.Color = Color.FromArgb(204, 255, 204);
                        worksheet.Range[string.Format("A{0}:{1}{0}", 1, aryAlpha[aryTitles.Length - 1])].Borders.Color = Color.Black;
                        excel.ActiveWorkbook.Worksheets[4].Columns(5).NumberFormatlocal = "yyyy/MM/dd";
                        excel.ActiveSheet.Columns(9).NumberFormatlocal = "yyyy/MM/dd";

                        int rc = this.gdtPullOut.Rows.Count;
                        for (int intIndex = 0; intIndex < rc; intIndex++)
                        {
                            for (int intIndex_0 = 0; intIndex_0 < aryTitles.Length; intIndex_0++)
                            {
                                objArray_1[0, intIndex_0] = this.gdtPullOut.Rows[intIndex][aryAlpha[intIndex_0]].ToString();
                            }

                            worksheet.Range[string.Format("A{0}:{1}{0}", intIndex + 2, aryAlpha[aryTitles.Length - 1])].Value2 = objArray_1;
                            worksheet.Range[string.Format("A{0}:{1}{0}", intIndex + 2, aryAlpha[aryTitles.Length - 1])].EntireColumn.AutoFit(); // 自動調整欄寬
                        }

                        worksheet.Cells[rc + 2, 2] = "Total:";
                        worksheet.Cells[rc + 2, 7] = string.Format("=SUM(G2:G{0})", MyUtility.Convert.GetString(rc + 1));
                        worksheet.Cells[rc + 2, 8] = string.Format("=SUM(H2:H{0})", MyUtility.Convert.GetString(rc + 1));

                        // 設定分割列數
                        excel.ActiveWindow.SplitRow = 1;

                        // 進行凍結視窗
                        excel.ActiveWindow.FreezePanes = true;
                    }
                    #endregion

                    #region 匯出 Fail Detail
                    if ((this.gdtFailDetail != null) && (this.gdtFailDetail.Rows.Count > 0))
                    {
                        worksheet = excel.ActiveWorkbook.Worksheets[5];
                        worksheet.Name = "Fail Detail";
                        string[] aryTitles = new string[] { "Country", "KPI Group", "Factory", "SP No", "Factory KPI", "Delivery By Shipmode", "Order Qty", "Fail Qty", "PullOut Date", "ShipMode", "ReasonID", "Order Reason" };
                        object[,] objArray_1 = new object[1, aryTitles.Length];
                        for (int intIndex = 0; intIndex < aryTitles.Length; intIndex++)
                        {
                            objArray_1[0, intIndex] = aryTitles[intIndex];
                        }

                        worksheet.get_Range("K:K", Type.Missing).NumberFormatLocal = "@";
                        worksheet.Range[string.Format("A{0}:{1}{0}", 1, aryAlpha[aryTitles.Length - 1])].Value2 = objArray_1;
                        worksheet.Range[string.Format("A{0}:{1}{0}", 1, aryAlpha[aryTitles.Length - 1])].AutoFilter(1); // 篩選
                        worksheet.Range[string.Format("A{0}:{1}{0}", 1, aryAlpha[aryTitles.Length - 1])].Interior.Color = Color.FromArgb(204, 255, 204);
                        worksheet.Range[string.Format("A{0}:{1}{0}", 1, aryAlpha[aryTitles.Length - 1])].Borders.Color = Color.Black;
                        excel.ActiveWorkbook.Worksheets[5].Columns(5).NumberFormatlocal = "yyyy/MM/dd";
                        excel.ActiveSheet.Columns(9).NumberFormatlocal = "yyyy/MM/dd";
                        int rc = this.gdtFailDetail.Rows.Count;
                        for (int intIndex = 0; intIndex < rc; intIndex++)
                        {
                            for (int intIndex_0 = 0; intIndex_0 < aryTitles.Length; intIndex_0++)
                            {
                                objArray_1[0, intIndex_0] = this.gdtFailDetail.Rows[intIndex][aryAlpha[intIndex_0]].ToString();
                            }

                            worksheet.Range[string.Format("A{0}:{1}{0}", intIndex + 2, aryAlpha[aryTitles.Length - 1])].Value2 = objArray_1;
                            worksheet.Range[string.Format("A{0}:{1}{0}", intIndex + 2, aryAlpha[aryTitles.Length - 1])].EntireColumn.AutoFit(); // 自動調整欄寬
                        }

                        worksheet.Cells[rc + 2, 2] = "Total:";
                        worksheet.Cells[rc + 2, 7] = string.Format("=SUM(G2:G{0})", MyUtility.Convert.GetString(rc + 1));

                        worksheet.Cells[rc + 2, 8] = string.Format("=SUM(H2:H{0})", MyUtility.Convert.GetString(rc + 1));

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
