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
            txtMdivision1.Text = Sci.Env.User.Keyword;
            txtFactory1.Text = Sci.Env.User.Factory;
            dateRange1.Select();
        }

        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            return true;
        }

        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(dateRange1.Value1))
            {
                MyUtility.Msg.WarningBox(" < Factory KPI Date > can't be empty!!");
                return false;
            }
            return base.ValidateInput();
        }

        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {

            string[] aryAlpha = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            DualResult result = new DualResult(true);
            try
            {
                string strSQL = @" SELECT A1.FACTORYID AS A, A3.ALIAS AS B , A1.QTY AS C, SUM(A4.ShipQty) AS D,  SUM(A5.ShipQty) AS E, SUM(A4.ShipQty)  /  case when A1.QTY = 0 then 1 else a1.qty end * 100 AS F
                                                FROM ORDERS A1
                                                LEFT JOIN FACTORY A2 ON A1.FACTORYID = A2.ID 
                                                LEFT JOIN COUNTRY A3 ON A2.COUNTRYID = A3.ID 
                                                LEFT JOIN PullOut_Detail A4 ON A1.ID = A4.ORDERID AND A4.PullOutDate <= A1.FtyKPI 
                                                LEFT JOIN PullOut_Detail A5 ON A1.ID = A5.ORDERID AND A5.PullOutDate > A1.FtyKPI 
                                                WHERE 1= 1 ";
                if (dateRange1.Value1 != null)
                    strSQL += string.Format(" AND A1.FtyKPI >= '{0}' ", dateRange1.Value1.Value.ToString("yyyy-MM-dd"));
                if (dateRange1.Value2 != null)
                    strSQL += string.Format(" AND A1.FtyKPI <= '{0}' ", dateRange1.Value2.Value.ToString("yyyy-MM-dd"));
                if (txtFactory1.Text != "")
                    strSQL += string.Format(" AND A1.FACTORYID = '{0}' ", txtFactory1.Text);
                if (txtCountry1.TextBox1.Text != "")
                    strSQL += string.Format(" AND A2.COUNTRYID = '{0}' ", txtCountry1.TextBox1.Text);
                strSQL += " GROUP BY A1.FACTORYID, A3.ALIAS, A1.QTY  ORDER BY  A1.FACTORYID, A3.ALIAS ";
                result = DBProxy.Current.Select(null, strSQL, null, out gdtDatas);
                if (!result) return result;
                if ((gdtDatas == null) || (gdtDatas.Rows.Count == 0))
                    return new DualResult(false, "查不到任何資料，請重新查詢！");
                if (checkBox1.Checked)
                {
                    #region Order Detail
                    strSQL = @" SELECT  A2.CountryID AS A,  A2.KpiCode AS B, A1.FactoryID AS C , A1.ID AS D, A1.BRANDID AS E
                                                        , Convert(varchar,A1.BuyerDelivery ) AS F
                                                        , Convert(varchar,cast( A1.FtyKPI as date))  AS G 
                                                        ,(SELECT strData+',' FROM (SELECT Convert(varchar, Order_QtyShip.ShipmodeID) + '-' + Convert(varchar, Order_QtyShip.Qty) + '(' + Convert(varchar, Order_QtyShip.BuyerDelivery) + ')' as strData FROM Order_QtyShip where id = A1.ID) t for xml path('')) AS H 
                                                        , A1.QTY AS I 
                                                        , Sum(A4.ShipQty) AS J
                                                        , Sum(A5.ShipQty) AS K
                                                        , (select strData+',' from (Select convert(varchar,PulloutDate) as strData from Pullout_Detail where OrderID = A1.ID)t for xml path('')) AS L
                                                        , (select strData+',' from (Select ShipmodeID  as strData from Order_QtyShip  where id = A1.ID  Group by ShipModeID) t for xml path('')) AS M
                                                        , (Select Count(id) as CountPullOut from Pullout_Detail where OrderID = A1.ID) AS N
                                                        , CASE WHEN A1.GMTComplete   = 'C' OR A1.GMTComplete   = 'S' THEN 'Y' ELSE '' END AS O
                                                        , (SELECT TOP 1 A1.ReasonID  from Order_History A1 Where A1.OldValue =  A1.ID  And A1.HisType = 'Delivery' ) AS P
                                                        , (Select TOP 1 A2.Name  from Order_History A1
                                                                          LEFT JOIN Reason A2  ON A2.ID = A1.ReasonID 
                                                                          Where A1.OldValue =  A1.ID  And A1.HisType = 'Delivery') AS Q
                                                        , dbo.getTPEPass1(A1.MRHandle)  AS R
                                                        , dbo.getTPEPass1(A1.SMR)  AS S
                                                        , dbo.getTPEPass1(A6.POHandle)  AS T
                                                        , dbo.getTPEPass1(A6.POSMR)  AS U
                                                FROM ORDERS A1
                                                LEFT JOIN FACTORY A2 ON A1.FACTORYID = A2.ID 
                                                LEFT JOIN COUNTRY A3 ON A2.COUNTRYID = A3.ID 
                                                LEFT JOIN PullOut_Detail A4 ON A1.ID = A4.ORDERID AND A4.PullOutDate <= A1.FtyKPI 
                                                LEFT JOIN PullOut_Detail A5 ON A1.ID = A5.ORDERID AND A5.PullOutDate > A1.FtyKPI 
                                                LEFT JOIN PO A6 ON A1.POID = A6.ID
                                                WHERE 1= 1 ";
                    if (dateRange1.Value1 != null)
                        strSQL += string.Format(" AND A1.FtyKPI >= '{0}' ", dateRange1.Value1.Value.ToString("yyyy-MM-dd"));
                    if (dateRange1.Value2 != null)
                        strSQL += string.Format(" AND A1.FtyKPI <= '{0}' ", dateRange1.Value2.Value.ToString("yyyy-MM-dd"));
                    if (txtMdivision1.Text != "")
                        strSQL += string.Format(" AND A1.MDivisionID = '{0}' ", txtMdivision1.Text); 
                    if (txtFactory1.Text != "")
                        strSQL += string.Format(" AND A1.FACTORYID = '{0}' ", txtFactory1.Text);
                    if (txtCountry1.TextBox1.Text != "")
                        strSQL += string.Format(" AND A2.COUNTRYID = '{0}' ", txtCountry1.TextBox1.Text);
                    strSQL += @" GROUP BY A2.CountryID,  A2.KpiCode, A1.FactoryID , A1.ID, A1.BRANDID
                                                        , A1.BuyerDelivery, A1.FtyKPI, A1.QTY 
                                                        , CASE WHEN A1.GMTComplete   = 'C' OR A1.GMTComplete   = 'S' THEN 'Y' ELSE '' END
                                                        , A1.MRHandle, A1.SMR, A6.POHandle, A6.POSMR ";
                    result = DBProxy.Current.Select(null, strSQL, null, out gdtOrderDetail);
                    if (!result) return result;
//                    if (gdtOrderDetail != null)
//                    {
//                        System.Data.DataTable dtTemp;
//                        for (int intIndex = 0; intIndex < gdtOrderDetail.Rows.Count; intIndex++)
//                        {
//                            dtTemp = null;
//                            #region Set H Column Data
//                            strSQL = string.Format(@"SELECT Convert(varchar, Order_QtyShip.ShipmodeID) + '-' + Convert(varchar, Order_QtyShip.Qty) + '(' + Convert(varchar, Order_QtyShip.BuyerDelivery) + ')' as strData FROM Order_QtyShip where id = '{0}' ", gdtOrderDetail.Rows[intIndex]["D"].ToString());
//                            result = DBProxy.Current.Select(null, strSQL, null, out dtTemp);
//                            if (!result) return result;
//                            if (dtTemp != null)
//                            {
//                                string strTemp = "";
//                                for (int intIndex_1 = 0; intIndex_1 < dtTemp.Rows.Count; intIndex_1++)
//                                {
//                                    strTemp += (strTemp == "" ? dtTemp.Rows[intIndex_1]["strData"].ToString() : ", " + dtTemp.Rows[intIndex_1]["strData"].ToString());
//                                }
//                                gdtOrderDetail.Rows[intIndex]["H"] = strTemp;
//                            }
//                            #endregion Set H Column Data

//                            #region Set L Column Data
//                            strSQL = string.Format(@"Select PulloutDate as strData from Pullout_Detail where OrderID = '{0}' Order by PullOutDate ", gdtOrderDetail.Rows[intIndex]["D"].ToString());
//                            result = DBProxy.Current.Select(null, strSQL, null, out dtTemp);
//                            if (!result) return result;
//                            if (dtTemp != null)
//                            {
//                                string strTemp = "";
//                                for (int intIndex_1 = 0; intIndex_1 < dtTemp.Rows.Count; intIndex_1++)
//                                {
//                                    strTemp += (strTemp == "" ? dtTemp.Rows[intIndex_1]["strData"].ToString() : ", " + dtTemp.Rows[intIndex_1]["strData"].ToString());
//                                }
//                                gdtOrderDetail.Rows[intIndex]["L"] = strTemp;
//                            }
//                            #endregion Set L Column Data

//                            #region Set M Column Data
//                            strSQL = string.Format(@"Select ShipmodeID  as strData from Order_QtyShip  where id = '{0}'  Group by ShipModeID ", gdtOrderDetail.Rows[intIndex]["D"].ToString());
//                            result = DBProxy.Current.Select(null, strSQL, null, out dtTemp);
//                            if (!result) return result;
//                            if (dtTemp != null)
//                            {
//                                string strTemp = "";
//                                for (int intIndex_1 = 0; intIndex_1 < dtTemp.Rows.Count; intIndex_1++)
//                                {
//                                    strTemp += (strTemp == "" ? dtTemp.Rows[intIndex_1]["strData"].ToString() : ", " + dtTemp.Rows[intIndex_1]["strData"].ToString());
//                                }
//                                gdtOrderDetail.Rows[intIndex]["M"] = strTemp;
//                            }
//                            #endregion Set M Column Data

//                            #region Set N Column Data
//                            strSQL = string.Format(@"Select Count(id) as CountPullOut from Pullout_Detail where OrderID = '{0}'  ", gdtOrderDetail.Rows[intIndex]["D"].ToString());
//                            result = DBProxy.Current.Select(null, strSQL, null, out dtTemp);
//                            if (!result) return result;
//                            if (dtTemp != null)
//                            {
//                                gdtOrderDetail.Rows[intIndex]["N"] = (dtTemp.Rows[0]["CountPullOut"].ToString().CompareTo("1") <= 0 ? "0" : dtTemp.Rows[0]["CountPullOut"].ToString());
//                            }
//                            #endregion Set N Column Data

//                            #region Set P、Q Column Data
//                            strSQL = string.Format(@"Select A1.ReasonID , A2.Name  from Order_History A1
//                                                                          LEFT JOIN Reason A2  ON A2.ID = A1.ReasonID 
//                                                                          Where A1.OldValue =  '{0}'  And A1.HisType = 'Delivery' order by A1.AddDate desc ", gdtOrderDetail.Rows[intIndex]["D"].ToString());
//                            result = DBProxy.Current.Select(null, strSQL, null, out dtTemp);
//                            if (!result) return result;
//                            if ((dtTemp != null) && (dtTemp.Rows.Count > 0))
//                            {
//                                gdtOrderDetail.Rows[intIndex]["P"] = dtTemp.Rows[0]["ReasonID"].ToString();
//                                gdtOrderDetail.Rows[intIndex]["Q"] = dtTemp.Rows[0]["Name"].ToString();
//                            }
//                            #endregion Set P、Q Column Data
//                            string Handle_NAME = "";
//                            //if (UserPrg.GetName(gdtOrderDetail.Rows[intIndex]["R"].ToString().Trim(), out Handle_NAME, UserPrg.NameType.nameAndExt))
//                                //gdtOrderDetail.Rows[intIndex]["R"] = Handle_NAME;
//                            //if (UserPrg.GetName(gdtOrderDetail.Rows[intIndex]["S"].ToString().Trim(), out Handle_NAME, UserPrg.NameType.nameAndExt))
//                                gdtOrderDetail.Rows[intIndex]["S"] = Handle_NAME;
//                            //if (UserPrg.GetName(gdtOrderDetail.Rows[intIndex]["T"].ToString().Trim(), out Handle_NAME, UserPrg.NameType.nameAndExt))
//                                gdtOrderDetail.Rows[intIndex]["T"] = Handle_NAME;
//                            //if (UserPrg.GetName(gdtOrderDetail.Rows[intIndex]["U"].ToString().Trim(), out Handle_NAME, UserPrg.NameType.nameAndExt))
//                                //gdtOrderDetail.Rows[intIndex]["U"] = Handle_NAME;
//                        }
//                    }

                    #endregion Order Detail
                    #region On time Order List by PullOut
                    strSQL = @" SELECT  A2.CountryID AS A,  A2.KpiCode AS B, A1.FactoryID AS C , A1.ID AS D
                                                        , Convert(varchar,cast(A1.FtyKPI as date)) AS E
                                                        , (SELECT strData+',' FROM (SELECT Convert(varchar, Order_QtyShip.ShipmodeID) + '-' + Convert(varchar, Order_QtyShip.Qty) + '(' + Convert(varchar, Order_QtyShip.BuyerDelivery) + ')' as strData FROM Order_QtyShip where id = A1.ID) t for xml path('')) AS F
                                                        , A1.QTY AS G
                                                        , A4.ShipQty AS H
                                                        ,Convert(varchar,A4.PulloutDate) AS I   
                                                        ,J.strData AS J
                                                      --  , (Select ShipmodeID  as strData from Order_QtyShip  where id = A1.ID  Group by ShipModeID) AS J                                                     
                                                FROM ORDERS A1
                                                LEFT JOIN FACTORY A2 ON A1.FACTORYID = A2.ID 
                                                LEFT JOIN COUNTRY A3 ON A2.COUNTRYID = A3.ID 
                                                LEFT JOIN PullOut_Detail A4 ON A1.ID = A4.ORDERID AND A4.PullOutDate <= A1.FtyKPI 
                                                OUTER APPLY(Select ShipmodeID  as strData from Order_QtyShip  where id = A1.ID  Group by ShipModeID)J
                                                WHERE 1= 1 ";
                    if (dateRange1.Value1 != null)
                        strSQL += string.Format(" AND A1.FtyKPI >= '{0}' ", dateRange1.Value1.Value.ToString("yyyy-MM-dd"));
                    if (dateRange1.Value2 != null)
                        strSQL += string.Format(" AND A1.FtyKPI <= '{0}' ", dateRange1.Value2.Value.ToString("yyyy-MM-dd"));
                    if (txtMdivision1.Text != "")
                        strSQL += string.Format(" AND A1.MDivisionID = '{0}' ", txtMdivision1.Text);
                    if (txtFactory1.Text != "")
                        strSQL += string.Format(" AND A1.FACTORYID = '{0}' ", txtFactory1.Text);
                    if (txtCountry1.TextBox1.Text != "")
                        strSQL += string.Format(" AND A2.COUNTRYID = '{0}' ", txtCountry1.TextBox1.Text);
                    result = DBProxy.Current.Select(null, strSQL, null, out gdtPullOut);
                    if (!result) return result;
                    //if (gdtPullOut != null)
                    //{
                    //    System.Data.DataTable dtTemp;
                    //    for (int intIndex = 0; intIndex < gdtPullOut.Rows.Count; intIndex++)
                    //    {
                    //        dtTemp = null;
                    //        #region Set F Column Data
                    //        strSQL = string.Format(@"SELECT Convert(varchar, Order_QtyShip.ShipmodeID) + '-' + Convert(varchar, Order_QtyShip.Qty) + '(' + Convert(varchar, Order_QtyShip.BuyerDelivery) + ')' as strData FROM Order_QtyShip where id = '{0}' ", gdtPullOut.Rows[intIndex]["D"].ToString());
                    //        result = DBProxy.Current.Select(null, strSQL, null, out dtTemp);
                    //        if (!result) return result;
                    //        if (dtTemp != null)
                    //        {
                    //            string strTemp = "";
                    //            for (int intIndex_1 = 0; intIndex_1 < dtTemp.Rows.Count; intIndex_1++)
                    //            {
                    //                strTemp += (strTemp == "" ? dtTemp.Rows[intIndex_1]["strData"].ToString() : ", " + dtTemp.Rows[intIndex_1]["strData"].ToString());
                    //            }
                    //            gdtPullOut.Rows[intIndex]["F"] = strTemp;
                    //        }
                    //        #endregion Set F Column Data

                    //        #region Set J Column Data
                    //        strSQL = string.Format(@"Select ShipmodeID  as strData from Order_QtyShip  where id = '{0}'  Group by ShipModeID ", gdtPullOut.Rows[intIndex]["D"].ToString());
                    //        result = DBProxy.Current.Select(null, strSQL, null, out dtTemp);
                    //        if (!result) return result;
                    //        if (dtTemp != null)
                    //        {
                    //            string strTemp = "";
                    //            for (int intIndex_1 = 0; intIndex_1 < dtTemp.Rows.Count; intIndex_1++)
                    //            {
                    //                strTemp += (strTemp == "" ? dtTemp.Rows[intIndex_1]["strData"].ToString() : ", " + dtTemp.Rows[intIndex_1]["strData"].ToString());
                    //            }
                    //            gdtPullOut.Rows[intIndex]["J"] = strTemp;
                    //        }
                    //        #endregion Set J Column Data
                    //    }
                    //}

                    #endregion On time Order List by PullOut
                    #region Fail Detail
                    strSQL = @" SELECT  A2.CountryID AS A,  A2.KpiCode AS B, A1.FactoryID AS C , A1.ID AS D
                                                        , Convert(varchar,cast(A1.FtyKPI as date))  AS E
                                                        , (SELECT strData+',' FROM (SELECT Convert(varchar, Order_QtyShip.ShipmodeID) + '-' + Convert(varchar, Order_QtyShip.Qty) + '(' + Convert(varchar, Order_QtyShip.BuyerDelivery) + ')' as strData FROM Order_QtyShip where id = A1.ID) t for xml path('')) AS F
                                                        , A1.QTY AS G
                                                        , A4.ShipQty AS H
                                                        , Convert(varchar,A4.PulloutDate ) AS I   
                                                        ,J.strData AS J 
                                                       -- , (Select ShipmodeID  as strData from Order_QtyShip  where id = A1.ID  Group by ShipModeID) AS J                                                     
                                                FROM ORDERS A1
                                                LEFT JOIN FACTORY A2 ON A1.FACTORYID = A2.ID 
                                                LEFT JOIN COUNTRY A3 ON A2.COUNTRYID = A3.ID 
                                                LEFT JOIN PullOut_Detail A4 ON A1.ID = A4.ORDERID AND A4.PullOutDate > A1.FtyKPI 
                                                OUTER APPLY(Select ShipmodeID  as strData from Order_QtyShip  where id = A1.ID  Group by ShipModeID)J
                                                WHERE 1= 1 ";
                    if (dateRange1.Value1 != null)
                        strSQL += string.Format(" AND A1.FtyKPI >= '{0}' ", dateRange1.Value1.Value.ToString("yyyy-MM-dd"));
                    if (dateRange1.Value2 != null)
                        strSQL += string.Format(" AND A1.FtyKPI <= '{0}' ", dateRange1.Value2.Value.ToString("yyyy-MM-dd"));
                    if (txtMdivision1.Text != "")
                        strSQL += string.Format(" AND A1.MDivisionID = '{0}' ", txtMdivision1.Text);
                    if (txtFactory1.Text != "")
                        strSQL += string.Format(" AND A1.FACTORYID = '{0}' ", txtFactory1.Text);
                    if (txtCountry1.TextBox1.Text != "")
                        strSQL += string.Format(" AND A2.COUNTRYID = '{0}' ", txtCountry1.TextBox1.Text);
                    result = DBProxy.Current.Select(null, strSQL, null, out gdtFailDetail);
                    if (!result) return result;
                    //if (gdtFailDetail != null)
                    //{
                    //    System.Data.DataTable dtTemp;
                    //    for (int intIndex = 0; intIndex < gdtFailDetail.Rows.Count; intIndex++)
                    //    {
                    //        dtTemp = null;
                    //        #region Set F Column Data
                    //        strSQL = string.Format(@"SELECT Convert(varchar, Order_QtyShip.ShipmodeID) + '-' + Convert(varchar, Order_QtyShip.Qty) + '(' + Convert(varchar, Order_QtyShip.BuyerDelivery) + ')' as strData FROM Order_QtyShip where id = '{0}' ", gdtFailDetail.Rows[intIndex]["D"].ToString());
                    //        result = DBProxy.Current.Select(null, strSQL, null, out dtTemp);
                    //        if (!result) return result;
                    //        if (dtTemp != null)
                    //        {
                    //            string strTemp = "";
                    //            for (int intIndex_1 = 0; intIndex_1 < dtTemp.Rows.Count; intIndex_1++)
                    //            {
                    //                strTemp += (strTemp == "" ? dtTemp.Rows[intIndex_1]["strData"].ToString() : ", " + dtTemp.Rows[intIndex_1]["strData"].ToString());
                    //            }
                    //            gdtFailDetail.Rows[intIndex]["F"] = strTemp;
                    //        }
                    //        #endregion Set F Column Data

                    //        #region Set J Column Data
                    //        strSQL = string.Format(@"Select ShipmodeID  as strData from Order_QtyShip  where id = '{0}'  Group by ShipModeID ", gdtFailDetail.Rows[intIndex]["D"].ToString());
                    //        result = DBProxy.Current.Select(null, strSQL, null, out dtTemp);
                    //        if (!result) return result;
                    //        if (dtTemp != null)
                    //        {
                    //            string strTemp = "";
                    //            for (int intIndex_1 = 0; intIndex_1 < dtTemp.Rows.Count; intIndex_1++)
                    //            {
                    //                strTemp += (strTemp == "" ? dtTemp.Rows[intIndex_1]["strData"].ToString() : ", " + dtTemp.Rows[intIndex_1]["strData"].ToString());
                    //            }
                    //            gdtFailDetail.Rows[intIndex]["J"] = strTemp;
                    //        }
                    //        #endregion Set J Column Data
                    //    }
                    //}
                    #endregion Fail Detail
                    #region Fail Order List by SP
                    strSQL = @" SELECT  A2.CountryID AS A,  A2.KpiCode AS B, A1.FactoryID AS C , A1.ID AS D, A1.BRANDID AS E
                                                        , Convert(varchar,A1.BuyerDelivery)  AS F
                                                        , Convert(varchar,cast(A1.FtyKPI as date)) AS G 
                                                        , (SELECT strData+',' FROM (SELECT Convert(varchar, Order_QtyShip.ShipmodeID) + '-' + Convert(varchar, Order_QtyShip.Qty) + '(' + Convert(varchar, Order_QtyShip.BuyerDelivery) + ')' as strData FROM Order_QtyShip where id = A1.ID) t for xml path('')) AS H 
                                                        , A1.QTY AS I 
                                                        , Sum(A4.ShipQty) AS J
                                                        , Sum(A5.ShipQty) AS K
                                                        , (select strData+',' from (Select convert(varchar,PulloutDate) as strData from Pullout_Detail where OrderID = A1.ID)t for xml path('')) AS L
                                                        , (select strData+',' from (Select ShipmodeID  as strData from Order_QtyShip  where id = A1.ID  Group by ShipModeID) t for xml path('')) AS M
                                                        , (Select Count(id) as CountPullOut from Pullout_Detail where OrderID = A1.ID) AS N
                                                        , CASE WHEN A1.GMTComplete   = 'C' OR A1.GMTComplete   = 'S' THEN 'Y' ELSE '' END AS O
                                                        , (SELECT TOP 1 A1.ReasonID  from Order_History A1 Where A1.OldValue =  A1.ID  And A1.HisType = 'Delivery' ) AS P
                                                        , (Select TOP 1 A2.Name  from Order_History A1
                                                                          LEFT JOIN Reason A2  ON A2.ID = A1.ReasonID 
                                                                          Where A1.OldValue =  A1.ID  And A1.HisType = 'Delivery') AS Q
                                                        , dbo.getTPEPass1(A1.MRHandle)  AS R
                                                        , dbo.getTPEPass1(A1.SMR)  AS S
                                                        , dbo.getTPEPass1(A6.POHandle)  AS T
                                                        , dbo.getTPEPass1(A6.POSMR)  AS U
                                                FROM ORDERS A1
                                                LEFT JOIN FACTORY A2 ON A1.FACTORYID = A2.ID 
                                                LEFT JOIN COUNTRY A3 ON A2.COUNTRYID = A3.ID 
                                                LEFT JOIN PullOut_Detail A4 ON A1.ID = A4.ORDERID AND A4.PullOutDate <= A1.FtyKPI 
                                                LEFT JOIN PullOut_Detail A5 ON A1.ID = A5.ORDERID AND A5.PullOutDate > A1.FtyKPI 
                                                LEFT JOIN PO A6 ON A1.POID = A6.ID
                                                WHERE 1= 1 ";
                    if (dateRange1.Value1 != null)
                        strSQL += string.Format(" AND A1.FtyKPI >= '{0}' ", dateRange1.Value1.Value.ToString("yyyy-MM-dd"));
                    if (dateRange1.Value2 != null)
                        strSQL += string.Format(" AND A1.FtyKPI <= '{0}' ", dateRange1.Value2.Value.ToString("yyyy-MM-dd"));
                    if (txtMdivision1.Text != "")
                        strSQL += string.Format(" AND A1.MDivisionID = '{0}' ", txtMdivision1.Text);
                    if (txtFactory1.Text != "")
                        strSQL += string.Format(" AND A1.FACTORYID = '{0}' ", txtFactory1.Text);
                    if (txtCountry1.TextBox1.Text != "")
                        strSQL += string.Format(" AND A2.COUNTRYID = '{0}' ", txtCountry1.TextBox1.Text);
                    strSQL += @" GROUP BY A2.CountryID,  A2.KpiCode, A1.FactoryID , A1.ID, A1.BRANDID
                                                        , A1.BuyerDelivery, A1.FtyKPI, A1.QTY 
                                                        , CASE WHEN A1.GMTComplete   = 'C' OR A1.GMTComplete   = 'S' THEN 'Y' ELSE '' END
                                                        , A1.MRHandle, A1.SMR, A6.POHandle, A6.POSMR
                                                        HAVING Sum(A5.ShipQty) > 0 ";
                    result = DBProxy.Current.Select(null, strSQL, null, out gdtSP);
                    if (!result) return result;
//                    if (gdtSP != null)
//                    {
//                        System.Data.DataTable dtTemp;
//                        for (int intIndex = 0; intIndex < gdtSP.Rows.Count; intIndex++)
//                        {
//                            dtTemp = null;
//                            #region Set H Column Data
//                            strSQL = string.Format(@"SELECT Convert(varchar, Order_QtyShip.ShipmodeID) + '-' + Convert(varchar, Order_QtyShip.Qty) + '(' + Convert(varchar, Order_QtyShip.BuyerDelivery) + ')' as strData FROM Order_QtyShip where id = '{0}' ", gdtSP.Rows[intIndex]["D"].ToString());
//                            result = DBProxy.Current.Select(null, strSQL, null, out dtTemp);
//                            if (!result) return result;
//                            if (dtTemp != null)
//                            {
//                                string strTemp = "";
//                                for (int intIndex_1 = 0; intIndex_1 < dtTemp.Rows.Count; intIndex_1++)
//                                {
//                                    strTemp += (strTemp == "" ? dtTemp.Rows[intIndex_1]["strData"].ToString() : ", " + dtTemp.Rows[intIndex_1]["strData"].ToString());
//                                }
//                                gdtSP.Rows[intIndex]["H"] = strTemp;
//                            }
//                            #endregion Set H Column Data

//                            #region Set L Column Data
//                            strSQL = string.Format(@"Select PulloutDate as strData from Pullout_Detail where OrderID = '{0}' Order by PullOutDate ", gdtSP.Rows[intIndex]["D"].ToString());
//                            result = DBProxy.Current.Select(null, strSQL, null, out dtTemp);
//                            if (!result) return result;
//                            if (dtTemp != null)
//                            {
//                                string strTemp = "";
//                                for (int intIndex_1 = 0; intIndex_1 < dtTemp.Rows.Count; intIndex_1++)
//                                {
//                                    strTemp += (strTemp == "" ? dtTemp.Rows[intIndex_1]["strData"].ToString() : ", " + dtTemp.Rows[intIndex_1]["strData"].ToString());
//                                }
//                                gdtSP.Rows[intIndex]["L"] = strTemp;
//                            }
//                            #endregion Set L Column Data

//                            #region Set M Column Data
//                            strSQL = string.Format(@"Select ShipmodeID  as strData from Order_QtyShip  where id = '{0}'  Group by ShipModeID ", gdtSP.Rows[intIndex]["D"].ToString());
//                            result = DBProxy.Current.Select(null, strSQL, null, out dtTemp);
//                            if (!result) return result;
//                            if (dtTemp != null)
//                            {
//                                string strTemp = "";
//                                for (int intIndex_1 = 0; intIndex_1 < dtTemp.Rows.Count; intIndex_1++)
//                                {
//                                    strTemp += (strTemp == "" ? dtTemp.Rows[intIndex_1]["strData"].ToString() : ", " + dtTemp.Rows[intIndex_1]["strData"].ToString());
//                                }
//                                gdtSP.Rows[intIndex]["M"] = strTemp;
//                            }
//                            #endregion Set M Column Data

//                            #region Set N Column Data
//                            strSQL = string.Format(@"Select Count(id) as CountPullOut from Pullout_Detail where OrderID = '{0}'  ", gdtSP.Rows[intIndex]["D"].ToString());
//                            result = DBProxy.Current.Select(null, strSQL, null, out dtTemp);
//                            if (!result) return result;
//                            if (dtTemp != null)
//                            {
//                                gdtSP.Rows[intIndex]["N"] = (dtTemp.Rows[0]["CountPullOut"].ToString().CompareTo("1") <= 0 ? "0" : dtTemp.Rows[0]["CountPullOut"].ToString());
//                            }
//                            #endregion Set N Column Data

//                            #region Set P、Q Column Data
//                            strSQL = string.Format(@"Select A1.ReasonID , A2.Name  from TradeHis_Order A1
//                                                                          LEFT JOIN Reason A2  ON A2.ReasonTypeID = A1.ReasonTypeID AND A2.ID = A1.ReasonID 
//                                                                          Where A1.SourceID =  '{0}'  And A1.HisType = 'Delivery' order by A1.AddDate desc ", gdtSP.Rows[intIndex]["D"].ToString());
//                            result = DBProxy.Current.Select(null, strSQL, null, out dtTemp);
//                            if (!result) return result;
//                            if ((dtTemp != null) && (dtTemp.Rows.Count > 0))
//                            {
//                                gdtSP.Rows[intIndex]["P"] = dtTemp.Rows[0]["ReasonID"].ToString();
//                                gdtSP.Rows[intIndex]["Q"] = dtTemp.Rows[0]["Name"].ToString();
//                            }
//                            #endregion Set P、Q Column Data
//                            string Handle_NAME = "";
//                            //if (UserPrg.GetName(gdtSP.Rows[intIndex]["R"].ToString().Trim(), out Handle_NAME, UserPrg.NameType.nameAndExt))
//                            //    gdtSP.Rows[intIndex]["R"] = Handle_NAME;
//                            //if (UserPrg.GetName(gdtSP.Rows[intIndex]["S"].ToString().Trim(), out Handle_NAME, UserPrg.NameType.nameAndExt))
//                            //    gdtSP.Rows[intIndex]["S"] = Handle_NAME;
//                            //if (UserPrg.GetName(gdtSP.Rows[intIndex]["T"].ToString().Trim(), out Handle_NAME, UserPrg.NameType.nameAndExt))
//                            //    gdtSP.Rows[intIndex]["T"] = Handle_NAME;
//                            //if (UserPrg.GetName(gdtSP.Rows[intIndex]["U"].ToString().Trim(), out Handle_NAME, UserPrg.NameType.nameAndExt))
//                            //    gdtSP.Rows[intIndex]["U"] = Handle_NAME;
//                        }
//                    }

                    #endregion Fail Order List by SP
                }
                #region
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
            string temfile = Sci.Env.Cfg.XltPathDir + "\\Planning_R17.xltx";

            //Microsoft.Office.Interop.Excel.Application excel = null;
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Planning_R17.xltx");
            try
            {
                //if (!(result = PrivUtils.Excels.CreateExcel(temfile, out excel))) return result;
                Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];
                
                int intRowsCount = gdtDatas.Rows.Count;
                int intRowsStart = 2;//匯入起始位置
                int rownum = intRowsStart; //每筆資料匯入之位置 
                int intColumns = 6;//匯入欄位數

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
                }
                if (checkBox1.Checked)
                {
                    int intWorkIndex = 2;
                    string[] aryAlpha = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
                    if ((gdtOrderDetail != null) && (gdtOrderDetail.Rows.Count > 0))
                    {
                        if (excel.ActiveWorkbook.Worksheets.Count >= intWorkIndex)
                            worksheet = excel.ActiveWorkbook.Worksheets[intWorkIndex];
                        else
                            worksheet = excel.ActiveWorkbook.Worksheets.Add(Type.Missing, (worksheet == null ? Type.Missing : worksheet), Type.Missing, XlSheetType.xlWorksheet);
                        intWorkIndex++;
                        worksheet.Name = "Order Detail";
                        string[] aryTitles = new string[] {"Country", "KPI Group", "Factory", "SP No", "Brand", "Buyer Delivery", "Factory KPI", "Delivery By Shipmode", "Order Qty", "On Time Qty", "Fail Qty", "PullOut Date", "ShipMode", "[P]", "Garment Complete", "ReasonID", "Order Reason", "Handle  ", "SMR", "PO Handle", "PO SMR" };
                        object[,] objArray_1 = new object[1, aryTitles.Length];
                        for (int intIndex = 0; intIndex < aryTitles.Length; intIndex++)
                        {
                            objArray_1[0, intIndex] = aryTitles[intIndex];
                        }
                        worksheet.Range[String.Format("A{0}:{1}{0}", 1, aryAlpha[aryTitles.Length - 1])].Value2 = objArray_1;
                        worksheet.Range[String.Format("A{0}:{1}{0}", 1, aryAlpha[aryTitles.Length - 1])].AutoFilter(1, true); //篩選
                        worksheet.Range[String.Format("A{0}:{1}{0}", 1, aryAlpha[aryTitles.Length - 1])].Interior.Color = Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(204)))));
                        worksheet.Range[String.Format("A{0}:{1}{0}", 1, aryAlpha[aryTitles.Length - 1])].Borders.Color = Color.Black;
                        excel.ActiveSheet.Columns(6).NumberFormatlocal = "yyyy/MM/dd";
                        excel.ActiveSheet.Columns(7).NumberFormatlocal = "yyyy/MM/dd"; 
                        for (int intIndex = 0; intIndex < gdtOrderDetail.Rows.Count; intIndex++)
                        {
                            for (int intIndex_0 = 0; intIndex_0 < aryTitles.Length; intIndex_0++)
                            {                 
                                objArray_1[0, intIndex_0] = gdtOrderDetail.Rows[intIndex][aryAlpha[intIndex_0]].ToString();
                            }
                            worksheet.Range[String.Format("A{0}:{1}{0}", intIndex + 2, aryAlpha[aryTitles.Length - 1])].Value2 = objArray_1;
                            worksheet.Range[String.Format("A{0}:{1}{0}", intIndex + 2, aryAlpha[aryTitles.Length - 1])].EntireColumn.AutoFit();//自動調整欄寬
                  
                        }
                         //設定分割列數
                        excel.ActiveWindow.SplitRow = 1; 
                        // 進行凍結視窗
                        excel.ActiveWindow.FreezePanes = true;
                    }
                    if ((gdtPullOut != null) && (gdtPullOut.Rows.Count > 0))
                    {
                        if (excel.ActiveWorkbook.Worksheets.Count >= intWorkIndex)
                            worksheet = excel.ActiveWorkbook.Worksheets[intWorkIndex];
                        else
                            worksheet = excel.ActiveWorkbook.Worksheets.Add(Type.Missing, (worksheet == null ? Type.Missing : worksheet), Type.Missing, XlSheetType.xlWorksheet);
                        intWorkIndex++;
                        worksheet.Name = "On time Order List by PullOut";
                        string[] aryTitles = new string[] { "Country", "KPI Group", "Factory", "SP No", "Factory KPI", "Delivery By Shipmode", "Order Qty", "PullOut Qty", "PullOut Date", "ShipMode" };
                        object[,] objArray_1 = new object[1, aryTitles.Length];
                        for (int intIndex = 0; intIndex < aryTitles.Length; intIndex++)
                        {
                            objArray_1[0, intIndex] = aryTitles[intIndex];
                        }
                        worksheet.Range[String.Format("A{0}:{1}{0}", 1, aryAlpha[aryTitles.Length - 1])].Value2 = objArray_1;
                        worksheet.Range[String.Format("A{0}:{1}{0}", 1, aryAlpha[aryTitles.Length - 1])].AutoFilter(1, true); //篩選
                        worksheet.Range[String.Format("A{0}:{1}{0}", 1, aryAlpha[aryTitles.Length - 1])].Interior.Color = Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(204)))));
                        worksheet.Range[String.Format("A{0}:{1}{0}", 1, aryAlpha[aryTitles.Length - 1])].Borders.Color = Color.Black;
                        excel.ActiveSheet.Columns(5).NumberFormatlocal = "yyyy/MM/dd";
                        excel.ActiveSheet.Columns(9).NumberFormatlocal = "yyyy/MM/dd"; 


                        for (int intIndex = 0; intIndex < gdtPullOut.Rows.Count; intIndex++)
                        {              
                            for (int intIndex_0 = 0; intIndex_0 < aryTitles.Length; intIndex_0++)
                            {
                                objArray_1[0, intIndex_0] = gdtPullOut.Rows[intIndex][aryAlpha[intIndex_0]].ToString();
                            }
                            worksheet.Range[String.Format("A{0}:{1}{0}", intIndex + 2, aryAlpha[aryTitles.Length - 1])].Value2 = objArray_1;
                            worksheet.Range[String.Format("A{0}:{1}{0}", intIndex + 2, aryAlpha[aryTitles.Length - 1])].EntireColumn.AutoFit(); //自動調整欄寬
                        }
                        //設定分割列數
                        excel.ActiveWindow.SplitRow = 1;
                        // 進行凍結視窗
                        excel.ActiveWindow.FreezePanes = true;
                    }
                    if ((gdtFailDetail != null) && (gdtFailDetail.Rows.Count > 0))
                    {
                        if (excel.ActiveWorkbook.Worksheets.Count >= intWorkIndex)
                            worksheet = excel.ActiveWorkbook.Worksheets[intWorkIndex];
                        else
                            worksheet = excel.ActiveWorkbook.Worksheets.Add(Type.Missing, (worksheet == null ? Type.Missing : worksheet), Type.Missing, XlSheetType.xlWorksheet);
                        intWorkIndex++;
                        worksheet.Name = "Fai lDetail";
                        string[] aryTitles = new string[] { "Country", "KPI Group", "Factory", "SP No", "Factory KPI", "Delivery By Shipmode", "Order Qty", "PullOut Qty", "PullOut Date", "ShipMode" };
                        object[,] objArray_1 = new object[1, aryTitles.Length];
                        for (int intIndex = 0; intIndex < aryTitles.Length; intIndex++)
                        {
                            objArray_1[0, intIndex] = aryTitles[intIndex];
                        }
                        worksheet.Range[String.Format("A{0}:{1}{0}", 1, aryAlpha[aryTitles.Length - 1])].Value2 = objArray_1;
                        worksheet.Range[String.Format("A{0}:{1}{0}", 1, aryAlpha[aryTitles.Length - 1])].AutoFilter(1,true); //篩選
                        worksheet.Range[String.Format("A{0}:{1}{0}", 1, aryAlpha[aryTitles.Length - 1])].Interior.Color = Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(204)))));
                        worksheet.Range[String.Format("A{0}:{1}{0}", 1, aryAlpha[aryTitles.Length - 1])].Borders.Color = Color.Black;
                        excel.ActiveSheet.Columns(5).NumberFormatlocal = "yyyy/MM/dd"; 
                        excel.ActiveSheet.Columns(9).NumberFormatlocal = "yyyy/MM/dd"; 
                        for (int intIndex = 0; intIndex < gdtFailDetail.Rows.Count; intIndex++)
                        {
                            
                            for (int intIndex_0 = 0; intIndex_0 < aryTitles.Length; intIndex_0++)
                            {
                                objArray_1[0, intIndex_0] = gdtFailDetail.Rows[intIndex][aryAlpha[intIndex_0]].ToString();
                            }
                            worksheet.Range[String.Format("A{0}:{1}{0}", intIndex + 2, aryAlpha[aryTitles.Length - 1])].Value2 = objArray_1;
                            worksheet.Range[String.Format("A{0}:{1}{0}", intIndex + 2, aryAlpha[aryTitles.Length - 1])].EntireColumn.AutoFit(); //自動調整欄寬
                        }
                        //設定分割列數
                        excel.ActiveWindow.SplitRow = 1;
                        // 進行凍結視窗
                        excel.ActiveWindow.FreezePanes = true;
                    }
                    if ((gdtSP != null) && (gdtSP.Rows.Count > 0))
                    {
                        if (excel.ActiveWorkbook.Worksheets.Count >= intWorkIndex)
                            worksheet = excel.ActiveWorkbook.Worksheets[intWorkIndex];
                        else
                            worksheet = excel.ActiveWorkbook.Worksheets.Add(Type.Missing, (worksheet == null ? Type.Missing : worksheet), Type.Missing, XlSheetType.xlWorksheet);
                        intWorkIndex++;
                        worksheet.Name = "Fail Order List by SP";
                        string[] aryTitles = new string[] { "Country", "KPI Group", "Factory", "SP No", "Brand", "Buyer Delivery", "Factory KPI", "Delivery By Shipmode ", "Order Qty", "On Time Qty", "Fail Qty", "Fail PullOut Date", "ShipMode", "[P]", "Garment Complete", "ReasonID", "Order Reason", "Handle", "SMR", "PO Handle", "PO SMR" };
                        object[,] objArray_1 = new object[1, aryTitles.Length];
                        for (int intIndex = 0; intIndex < aryTitles.Length; intIndex++)
                        {
                            objArray_1[0, intIndex] = aryTitles[intIndex];
                        }
                        worksheet.Range[String.Format("A{0}:{1}{0}", 1, aryAlpha[aryTitles.Length - 1])].Value2 = objArray_1;
                        worksheet.Range[String.Format("A{0}:{1}{0}", 1, aryAlpha[aryTitles.Length - 1])].AutoFilter(1,true); //篩選
                        worksheet.Range[String.Format("A{0}:{1}{0}", 1, aryAlpha[aryTitles.Length - 1])].Interior.Color = Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(204)))));
                        worksheet.Range[String.Format("A{0}:{1}{0}", 1, aryAlpha[aryTitles.Length - 1])].Borders.Color = Color.Black;
                        excel.ActiveSheet.Columns(6).NumberFormatlocal = "yyyy/MM/dd";
                        excel.ActiveSheet.Columns(7).NumberFormatlocal = "yyyy/MM/dd";
                        
                      //  excelRange.EntireColumn.AutoFit();
                        
                        for (int intIndex = 0; intIndex < gdtSP.Rows.Count; intIndex++)
                        {
                           
                            for (int intIndex_0 = 0; intIndex_0 < aryTitles.Length; intIndex_0++)
                            {
                                objArray_1[0, intIndex_0] = gdtSP.Rows[intIndex][aryAlpha[intIndex_0]].ToString();
                            }
                            worksheet.Range[String.Format("A{0}:{1}{0}", intIndex + 2, aryAlpha[aryTitles.Length - 1])].Value2 = objArray_1;
                            worksheet.Range[String.Format("A{0}:{1}{0}", intIndex + 2, aryAlpha[aryTitles.Length - 1])].EntireColumn.AutoFit(); //自動調整欄寬
                        }
                        //設定分割列數
                        excel.ActiveWindow.SplitRow = 1;
                        // 進行凍結視窗
                        excel.ActiveWindow.FreezePanes = true;
                    }
                }
                #endregion
                excel.Visible = true;
                //自動開啟Excel存檔畫面
                //if (!(result = PrivUtils.Excels.SaveExcel(temfile.Substring(0, temfile.Length - 4), excel))) return result;
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
