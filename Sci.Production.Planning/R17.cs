using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Sci.Data;
using Ict;
using Sci.Win;
using System.Runtime.InteropServices;
using Sci.Production.Class;
using System.Linq;
using Sci.Production.Class.Commons;
using System.Linq;
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
            string[] aryAlpha = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            DualResult result = new DualResult(true);
            try
            {
                #region Order Detail
                string strSQL = @" 
SELECT
 A = F.CountryID
,B = F.KpiCode
,C = o.FactoryID
,D = o.ID
,E = Order_QS.seq
,F = o.BRANDID
,G = convert(varchar(10),Order_QS.BuyerDelivery,111)
,H = convert(varchar(10),Order_QS.FtyKPI,111)
,I = convert(varchar(10),iif(Order_QS.ShipmodeID in ('A/C', 'A/P', 'E/C', 'E/P'), Order_QS.FtyKPI, DATEADD(day, isnull(b.OTDExtension,0), Order_QS.FtyKPI)), 111)
,J = Order_QS.ShipmodeID
,K = Cast(Order_QS.QTY as int)
,L = CASE o.GMTComplete WHEN 'S' THEN Cast(isnull(Order_QS.QTY,0) as int)
                        ELSE iif(Ot.isDevSample = 1, iif(pd2.isFail = 1 or pd2.PulloutDate is null, 0, Cast(Order_QS.QTY as int)), Cast(isnull(pd.Qty,0) as int)) END
,M = CASE o.GMTComplete WHEN 'S' THEN 0
                        ELSE iif(ot.isDevSample = 1, iif(pd2.isFail = 1 or pd2.PulloutDate is null, Cast(Order_QS.QTY as int), 0), Cast(isnull(pd.FailQty,Order_QS.QTY) as int)) END
,N = iif(ot.isDevSample = 1, CONVERT(char(10), pd2.PulloutDate, 20), CONVERT(char(10), p.PulloutDate, 20))
,O = Order_QS.ShipmodeID
,P = Cast(isnull(op.PulloutDate,0) as int)
,Q = CASE o.GMTComplete WHEN 'C' THEN 'Y' 
                        WHEN 'S' THEN 'S' ELSE '' END
,R = Order_QS.ReasonID
,S = case o.Category when 'B' then r.Name
  when 'S' then rs.Name
  else '' end
,T = o.MRHandle
,U = o.SMR
,V = PO.POHandle
,W = PO.POSMR
,X = o.OrderTypeID
,Y = iif(ot.isDevSample = 1, 'Y', '')
,Z = c.alias
,o.MDivisionID
into #tmp
FROM Orders o WITH (NOLOCK)
LEFT JOIN OrderType ot on o.OrderTypeID = ot.ID and o.BrandID = ot.BrandID and o.BrandID = ot.BrandID
LEFT JOIN Factory f ON o.FACTORYID = f.ID 
LEFT JOIN Country c ON F.COUNTRYID = c.ID 
inner JOIN Order_QtyShip Order_QS on Order_QS.id = o.id
LEFT JOIN PO ON o.POID = PO.ID
LEFT JOIN Reason r on r.id = Order_QS.ReasonID and r.ReasonTypeID = 'Order_BuyerDelivery'          
LEFT JOIN Reason rs on rs.id = Order_QS.ReasonID and rs.ReasonTypeID = 'Order_BuyerDelivery_sample'
LEFT JOIN Brand b on o.BrandID = b.ID
--出貨次數--
outer apply (
	select COUNT(op.pulloutdate) PulloutDate 
	from Pullout_Detail op 
	where op.OrderID = o.id 
	and op.OrderShipmodeSeq = Order_QS.Seq
) op 
------------
-----isDevSample=0-----
outer apply (
	Select 
		Qty = Sum(rA.Qty) - dbo.getInvAdjQtyByDate( o.ID ,Order_QS.Seq,DATEADD(day, isnull(b.OTDExtension,0), Order_QS.FtyKPI),'<='),
		FailQty = Sum(rB.Qty)  - dbo.getInvAdjQtyByDate( o.ID ,Order_QS.Seq,DATEADD(day, isnull(b.OTDExtension,0), Order_QS.FtyKPI),'>')
	From Pullout_Detail pd
    Outer apply (Select Qty = IIF(pd.pulloutdate <= iif(Order_QS.ShipmodeID in ('A/C', 'A/P', 'E/C', 'E/P'), Order_QS.FtyKPI, DATEADD(day, isnull(b.OTDExtension,0), Order_QS.FtyKPI)), pd.shipqty, 0)) rA --On Time
	Outer apply (Select Qty = IIF(pd.pulloutdate >  iif(Order_QS.ShipmodeID in ('A/C', 'A/P', 'E/C', 'E/P'), Order_QS.FtyKPI, DATEADD(day, isnull(b.OTDExtension,0), Order_QS.FtyKPI)), pd.shipqty, 0)) rB --Fail
	where pd.OrderID = o.ID 
	and pd.OrderShipmodeSeq = Order_QS.Seq
) pd
outer apply (
    select top 1 PulloutDate
    from Pullout_Detail pd 
    where pd.OrderID = o.ID and pd.OrderShipmodeSeq =  Order_QS.Seq 
    Order by PulloutDate desc
) p
-------End-------
-----isDevSample=1-----
outer apply (
	Select top 1 iif(pd.PulloutDate > iif(Order_QS.ShipmodeID in ('A/C', 'A/P', 'E/C', 'E/P'), Order_QS.FtyKPI, DATEADD(day, isnull(b.OTDExtension,0), Order_QS.FtyKPI)), 1, 0) isFail, pd.PulloutDate
	From Pullout_Detail pd
	where pd.OrderID = o.ID 
	and pd.OrderShipmodeSeq = Order_QS.Seq
	order by pd.pulloutdate ASC
) pd2
-------End-------
where Order_QS.Qty > 0 and (ot.IsGMTMaster = 0 or o.OrderTypeID = '')  and (o.Junk is null or o.Junk = 0) ";

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
                    strSQL += string.Format(" AND f.KpiCode = '{0}' ", this.txtFactory.Text);
                }

                strSQL += @"select * from (
select * from #tmp 
union all
select A, B, C, D, E, F, G, H, I, J, K = 0, L = 0, M = K - (L +M), N = '', O, P = 0, Q, R, S, T, U, V, W, X, Y, Z, MDivisionID
from #tmp where (L +M) < K and I < G) as result
order by D,E,K desc
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
                strSQL = @" SELECT  '' AS A,  '' AS B, 0 AS C, 0 AS D, 0 AS E, 0.00 AS F, '' AS MDivisionID FROM ORDERS WHERE 1 = 0 ";
                result = DBProxy.Current.Select(null, strSQL, null, out this.gdtSDP);
                if (!result)
                {
                    return result;
                }

                #endregion

                #region Fail Order List by SP
                strSQL = @" 
SELECT  '' AS A
, '' AS B
, '' AS C
, '' AS D
, '' AS E
, '' AS F
, '' AS G
, '' AS H
,  '' AS I
,  '' AS J
,  0 AS K
,  0 AS L
,  0 AS M
, '' AS N
, '' AS O
, '' AS P
, '' AS Q
, '' AS R
, '' AS S
, '' AS T
, '' AS U
, '' AS V 
, '' AS W
, '' AS X
, '' AS Y
, '' AS Z
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
                    strSQL += string.Format(" AND f.KpiCode = '{0}' ", this.txtFactory.Text);
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
                    strSQL += string.Format(" AND f.KpiCode = '{0}' ", this.txtFactory.Text);
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
                    strSQL += string.Format(" AND f.KpiCode = '{0}' ", this.txtFactory.Text);
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
                    strSQL += string.Format(" AND f.KpiCode = '{0}' ", this.txtFactory.Text);
                }

                strSQL += "  order by TH_Order.AddDate desc ";
                if (!(result = DBProxy.Current.Select(null, strSQL, null, out dtTradeHis_Order)))
                {
                    return result;
                }

                var groupQty = this.gdtOrderDetail.AsEnumerable().Select(row => new
                {
                    PoId = row.Field<string>("D"),
                    OrderQty = row.Field<int>("K"),
                    PullQty = row.Field<int>("L"),
                    FailQty = row.Field<int>("M"),
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
                    string handleName = string.Empty;
                    if (UserPrg.GetName(drData["T"].ToString().Trim(), out handleName, UserPrg.NameType.idAndNameAndExt))
                    {
                        drData["T"] = handleName;
                    }

                    if (UserPrg.GetName(drData["U"].ToString().Trim(), out handleName, UserPrg.NameType.idAndNameAndExt))
                    {
                        drData["U"] = handleName;
                    }

                    if (UserPrg.GetName(drData["V"].ToString().Trim(), out handleName, UserPrg.NameType.idAndNameAndExt))
                    {
                        drData["V"] = handleName;
                    }

                    if (UserPrg.GetName(drData["W"].ToString().Trim(), out handleName, UserPrg.NameType.idAndNameAndExt))
                    {
                        drData["W"] = handleName;
                    }

                    #region Calc SDP Data
                    int intIndex_SDP = lstSDP.IndexOf(drData["B"].ToString() + "___" + drData["Z"].ToString()); // A
                    DataRow drSDP;
                    if (intIndex_SDP < 0)
                    {
                        drSDP = this.gdtSDP.NewRow();
                        drSDP["A"] = drData["B"].ToString();
                        drSDP["B"] = drData["Z"].ToString(); // A
                        drSDP["MDivisionID"] = drData["MDivisionID"].ToString(); // A
                        this.gdtSDP.Rows.Add(drSDP);
                        lstSDP.Add(drData["B"].ToString() + "___" + drData["Z"].ToString()); // A
                    }
                    else
                    {
                        drSDP = this.gdtSDP.Rows[intIndex_SDP];
                    }

                    drSDP["C"] = (drSDP["C"].ToString() != string.Empty ? Convert.ToDecimal(drSDP["C"].ToString()) : 0) + (drData["K"].ToString() != string.Empty ? Convert.ToDecimal(drData["K"].ToString()) : 0);
                    drSDP["D"] = (drSDP["D"].ToString() != string.Empty ? Convert.ToDecimal(drSDP["D"].ToString()) : 0) + (drData["L"].ToString() != string.Empty ? Convert.ToDecimal(drData["L"].ToString()) : 0);
                    drSDP["E"] = (drSDP["E"].ToString() != string.Empty ? Convert.ToDecimal(drSDP["E"].ToString()) : 0) + (drData["M"].ToString() != string.Empty ? Convert.ToDecimal(drData["M"].ToString()) : 0);
                    drSDP["F"] = drSDP["C"].ToString() == "0" ? 0 : Convert.ToDecimal(drSDP["D"].ToString()) / Convert.ToDecimal(drSDP["C"].ToString()) * 100;
                    #endregion Calc SDP Data

                    #region Calc Fail Order List by SP Data

                    // By SP# 明細, group by SPNO 顯示
                    if ((drData["M"].ToString() != string.Empty) && (Convert.ToDecimal(drData["M"].ToString()) > 0) && poid != drData["D"].ToString())
                    {
                        DataRow drSP = this.gdtSP.NewRow();
                        drSP.ItemArray = drData.ItemArray;
                        poid = drData["D"].ToString();
                        foreach (var i in groupQty)
                        {
                            if (i.sumFailQty > 0)
                            {
                                if (i.PoID == drData["D"].ToString())
                                {
                                    drSP["K"] = i.sumOrderQty;
                                    drSP["L"] = i.sumPullQty;
                                    drSP["M"] = i.sumFailQty;
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
                        if (dictionary_Order_QtyShipIDs.TryGetValue(dtData["D"].ToString(), out lstDataRows))
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

                                dtData["J"] = strTemp;
                                dtData["O"] = strShipmodeID;
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
                        where += string.Format(" AND f.KpiCode = '{0}' ", this.txtFactory.Text);
                    }

                    strSQL = $@" 
SELECT A = c.alias 
     , B =  f.KpiCode
     , C = o.FactoryID
     , D = o.ID
     , E = Order_QS.Seq
     , F = convert(varchar(10),Order_QS.FtyKPI ,111)     
     , G = convert(varchar(10),iif(Order_QS.ShipmodeID in ('A/C', 'A/P', 'E/C', 'E/P'), Order_QS.FtyKPI, DATEADD(day, isnull(b.OTDExtension,0), Order_QS.FtyKPI)), 111)
     , H = Order_QS.ShipmodeID
     , I = Order_QS.QTY
     , J = CASE o.GMTComplete WHEN 'S' THEN Order_QS.QTY
	                    ELSE iif(ot.isDevSample = 1, Order_QS.QTY, isnull(opd.sQty,0)) END
     , K = iif(ot.isDevSample = 1, convert(varchar(10),opd2.PulloutDate,111),convert(varchar(10),pd.PulloutDate,111))
     , L = Order_QS.ShipmodeID
     , M = o.OrderTypeID      
     , N = iif(ot.isDevSample = 1, 'Y', '')                                                
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

                    objArray[0, 0] = dr["B"];
                    objArray[0, 1] = dr["A"];
                    objArray[0, 2] = dr["C"];
                    objArray[0, 3] = dr["D"];
                    objArray[0, 4] = dr["E"];
                    objArray[0, 5] = dr["F"];
                    objArray[0, 6] = (decimal)dr["F"] >= 97 ? "PASS" : "FAIL";

                    worksheet.Range[string.Format("A{0}:G{0}", rownum + i)].Value2 = objArray;

                    // insert by Mdivision Summary data
                    string nextMdisvision = string.Empty;
                    if ((i + 1) < intRowsCount)
                    {
                        nextMdisvision = (string)this.gdtSDP.Rows[i+1]["MdivisionID"];
                    }

                    if ((string)dr["MdivisionID"] != nextMdisvision)
                    {
                        objArray[0, 0] = string.Empty;
                        objArray[0, 1] = string.Empty;
                        objArray[0, 2] = $"=SUM(C{mdivisionRowsStart}:C{rownum + i})";
                        objArray[0, 3] = $"=SUM(D{mdivisionRowsStart}:D{rownum + i})";
                        objArray[0, 4] = $"=SUM(E{mdivisionRowsStart}:E{rownum + i})";
                        rownum++;
                        objArray[0, 5] = "=" + string.Format("D{0}/IF(C{0}=0, 1,C{0})*100", rownum + i);
                        objArray[0, 6] = (decimal)dr["F"] >= 97 ? "PASS" : "FAIL";
                        worksheet.Range[string.Format("A{0}:G{0}", rownum + i)].Value2 = objArray;
                        worksheet.Range[string.Format("A{0}:G{0}", rownum + i)].Interior.Color = Color.FromArgb(204, 255, 204);
                        worksheet.Range[string.Format("A{0}:G{0}", rownum + i)].Font.Bold = true;
                        worksheet.Range[string.Format("A{0}:G{0}", rownum + i)].Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = 1;
                        mdivisionRowsStart = rownum + i + 1;
                    }
                }

                if (intRowsCount > 0)
                {
                    worksheet.Range[string.Format("A{0}:A{0}", rownum + intRowsCount)].Value2 = "G. TTL.";
                    worksheet.Range[string.Format("C{0}:C{0}", rownum + intRowsCount)].Formula = "=SUM(" + string.Format("C{0}:C{1}", 2, rownum + intRowsCount - 1) + ")";
                    worksheet.Range[string.Format("D{0}:D{0}", rownum + intRowsCount)].Formula = "=SUM(" + string.Format("D{0}:D{1}", 2, rownum + intRowsCount - 1) + ")";
                    worksheet.Range[string.Format("E{0}:E{0}", rownum + intRowsCount)].Formula = "=SUM(" + string.Format("E{0}:E{1}", 2, rownum + intRowsCount - 1) + ")";
                    worksheet.Range[string.Format("F{0}:F{0}", rownum + intRowsCount)].Formula = "=" + string.Format("D{0}/IF(C{0}=0, 1,C{0})*100", rownum + intRowsCount);
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
                            objArray_1[0, intIndex_0] = this.gdtSP.Rows[intIndex][aryAlpha[intIndex_0]].ToString();
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
                        string[] aryTitles = new string[] { "Country", "KPI Group", "Factory", "SP No", "Seq", "Brand", "Buyer Delivery", "Factory KPI", "Extension", "Delivery By Shipmode", "Order Qty", "On Time Qty", "Fail Qty", "PullOut Date", "ShipMode", "[P]", "Garment Complete", "ReasonID", "Order Reason", "Handle  ", "SMR", "PO Handle", "PO SMR", "Order Type", "Dev. Sample" };
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
                                objArray_1[0, intIndex_0] = this.gdtPullOut.Rows[intIndex][aryAlpha[intIndex_0]].ToString();
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
                                     where data.Field<int>("M") > 0
                                     select new
                                    {
                                        A = data.Field<string>("A"),
                                        B= data.Field<string>("B"),
                                        C= data.Field<string>("C"),
                                        D= data.Field<string>("D"),
                                        E= data.Field<string>("E"),
                                        F= data.Field<string>("H"),
                                        G = data.Field<string>("I"),
                                        H = data.Field<string>("J"),
                                        I= data.Field<int>("K").ToString(),
                                        J= data.Field<int>("M").ToString(),
                                        K= data.Field<string>("N"),
                                        L= data.Field<string>("O"),
                                        M= data.Field<string>("R"),
                                        N= data.Field<string>("S"),
                                        O = data.Field<string>("X"),
                                        P = data.Field<string>("Y")
                                     };
                    if ((gdtFailDetail != null) && (gdtFailDetail.Count() > 0))
                    {
                        worksheet = excel.ActiveWorkbook.Worksheets[5];
                        worksheet.Name = "Fail Detail";
                        string[] aryTitles = new string[] { "Country", "KPI Group", "Factory", "SP No", "Seq", "Factory KPI", "Extension", "Delivery By Shipmode", "Order Qty", "Fail Qty", "PullOut Date", "ShipMode", "ReasonID", "Order Reason", "Order Type", "Dev. Sample" };
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
                        excel.ActiveWorkbook.Worksheets[5].Columns(6).NumberFormatlocal = "yyyy/MM/dd";
                        excel.ActiveSheet.Columns(10).NumberFormatlocal = "yyyy/MM/dd";
                        int rc = gdtFailDetail.Count();
                        int i = 1;
                        foreach (var dr in gdtFailDetail)
                        {
                            i++;
                            objArray_1[0, 0] = dr.A;
                            objArray_1[0, 1] = dr.B;
                            objArray_1[0, 2] = dr.C;
                            objArray_1[0, 3] = dr.D;
                            objArray_1[0, 4] = dr.E;
                            objArray_1[0, 5] = dr.F;
                            objArray_1[0, 6] = dr.G;
                            objArray_1[0, 7] = dr.H;
                            objArray_1[0, 8] = dr.I;
                            objArray_1[0, 9] = dr.J;
                            objArray_1[0, 10] = dr.K;
                            objArray_1[0, 11] = dr.L;
                            objArray_1[0, 12] = dr.M;
                            objArray_1[0, 13] = dr.N;
                            objArray_1[0, 14] = dr.O;
                            objArray_1[0, 15] = dr.P;
                            worksheet.Range[string.Format("A{0}:{1}{0}", i, aryAlpha[aryTitles.Length - 1])].Value2 = objArray_1;
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
