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

namespace Sci.Production.Logistic
{
    public partial class R01 : Sci.Win.Tems.PrintForm
    {
        DateTime? buyerDelivery1, buyerDelivery2, sciDelivery1, sciDelivery2;
        string mDivision, brand;
        DataTable printData;
        public R01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            DataTable mDivision;
            DBProxy.Current.Select(null, "select '' as ID union all select ID from MDivision WITH (NOLOCK) ", out mDivision);
            MyUtility.Tool.SetupCombox(comboM, 1, mDivision);
            comboM.Text = Sci.Env.User.Keyword;
        }


        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(dateBuyerDelivery.Value1) && MyUtility.Check.Empty(dateSCIDelivery.Value1))
            {
                MyUtility.Msg.WarningBox("Buyer Delivery or SCI Delivery can't be empty!!");
                return false;
            }

            buyerDelivery1 = dateBuyerDelivery.Value1;
            buyerDelivery2 = dateBuyerDelivery.Value2;
            sciDelivery1 = dateSCIDelivery.Value1;
            sciDelivery2 = dateSCIDelivery.Value2;
            mDivision = comboM.Text;
            brand = txtbrand.Text;

            return base.ValidateInput();
        }

        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(@"
select  o.FactoryID
        , o.MCHandle
        , o.SewLine
        , o.ID
        , o.CustPONo
        , o.Customize1
        , oq.BuyerDelivery
        , oq.ShipmodeID
        , oq.Seq
        , o.SciDelivery
        , o.TotalCTN
        , o.ClogCTN
        , o.PulloutCTNQty
        , CTNQty = isnull ((select sum (CTNQty) 
                            from PackingList_Detail WITH (NOLOCK) 
                            where   OrderID = o.ID 
                                    and OrderShipmodeSeq = oq.Seq) 
                           , 0)
        , ClogQty = isnull ((select sum (CTNQty) 
                             from PackingList_Detail WITH (NOLOCK) 
                             where  OrderID = o.ID 
                                    and OrderShipmodeSeq = oq.Seq 
                                    and ReceiveDate is not null)
                           , 0)
        , PullQty = isnull ((select sum (pd.CTNQty) 
                             from PackingList p WITH (NOLOCK) 
                                  , PackingList_Detail pd WITH (NOLOCK) 
                             where  p.ID = pd.ID 
                                    and pd.OrderID = o.ID 
                                    and pd.OrderShipmodeSeq = oq.Seq 
                                    and p.PulloutID != '')
                           , 0)
        , TtlGMTQty = isnull ((select sum(ShipQty) 
                               from PackingList_Detail WITH (NOLOCK) 
                               where OrderID = o.ID)
                             , 0)
        , TtlClogGMTQty = isnull ((select sum(ShipQty) 
                                   from PackingList_Detail WITH (NOLOCK) 
                                   where    OrderID = o.ID 
                                            and ReceiveDate is not null)
                                 , 0)
        , TtlPullGMTQty = isnull ((select sum(ShipQty) 
                                   from Pullout p WITH (NOLOCK) 
                                        , Pullout_Detail pd WITH (NOLOCK) 
                                   where    pd.OrderID = o.ID 
                                            and pd.ID = p.ID 
                                            and p.Status <> 'New')
                                 , 0)
        , GMTQty = isnull ((select sum(ShipQty) 
                            from PackingList_Detail WITH (NOLOCK) 
                            where   OrderID = o.ID 
                                    and OrderShipmodeSeq = oq.Seq)
                          , 0)
        , ClogGMTQty = isnull ((select sum(ShipQty) 
                                from PackingList_Detail WITH (NOLOCK) 
                                where   OrderID = o.ID 
                                        and OrderShipmodeSeq = oq.Seq 
                                        and ReceiveDate is not null)
                              , 0)
        , PullGMTQty = isnull ((select sum(ShipQty) 
                                from Pullout p
                                     , Pullout_Detail pd WITH (NOLOCK) 
                                where   pd.OrderID = o.ID 
                                        and pd.OrderShipmodeSeq = oq.Seq 
                                        and pd.ID = p.ID 
                                        and p.Status <> 'New'),0)
from Orders o WITH (NOLOCK) 
inner join Order_QtyShip oq WITH (NOLOCK) on o.ID = oq.Id
where o.Category = 'B'");

            if (!MyUtility.Check.Empty(buyerDelivery1))
            {
                sqlCmd.Append(string.Format(" and oq.BuyerDelivery >= '{0}'", Convert.ToDateTime(buyerDelivery1).ToString("d")));
            }
            if (!MyUtility.Check.Empty(buyerDelivery2))
            {
                sqlCmd.Append(string.Format(" and oq.BuyerDelivery <= '{0}'", Convert.ToDateTime(buyerDelivery2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(sciDelivery1))
            {
                sqlCmd.Append(string.Format(" and o.SciDelivery >= '{0}'", Convert.ToDateTime(sciDelivery1).ToString("d")));
            }
            if (!MyUtility.Check.Empty(sciDelivery2))
            {
                sqlCmd.Append(string.Format(" and o.SciDelivery <= '{0}'", Convert.ToDateTime(sciDelivery2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(brand))
            {
                sqlCmd.Append(string.Format(" and o.BrandID = '{0}'", brand));
            }

            if (!MyUtility.Check.Empty(mDivision))
            {
                sqlCmd.Append(string.Format(" and o.MDivisionID = '{0}'", mDivision));
            }
            sqlCmd.Append(" order by o.FtyGroup,o.ID,oq.BuyerDelivery");

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out printData);
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

            this.ShowWaitMessage("Starting EXCEL...");
            string strXltName = Sci.Env.Cfg.XltPathDir + "\\Logistic_R01_CartonStatusReport.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null) return false;
            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            worksheet.Cells[2, 1] = string.Format("Buyer Delivery: {0} ~ {1}             SCI Delivery: {2} ~ {3}             M: {4}             Brand: {5}",
                MyUtility.Check.Empty(buyerDelivery1) ? "" : Convert.ToDateTime(buyerDelivery1).ToString("d"),
                MyUtility.Check.Empty(buyerDelivery2) ? "" : Convert.ToDateTime(buyerDelivery2).ToString("d"),
                MyUtility.Check.Empty(sciDelivery1) ? "" : Convert.ToDateTime(sciDelivery1).ToString("d"),
                MyUtility.Check.Empty(sciDelivery2) ? "" : Convert.ToDateTime(sciDelivery2).ToString("d"),
                mDivision, brand);

            //填內容值
            int intRowsStart = 4;
            object[,] objArray = new object[1, 29];
            foreach (DataRow dr in printData.Rows)
            {
                objArray[0, 0] = dr["FactoryID"];
                objArray[0, 1] = dr["MCHandle"];
                objArray[0, 2] = dr["SewLine"];
                objArray[0, 3] = dr["ID"];
                objArray[0, 4] = dr["CustPONo"];
                objArray[0, 5] = dr["Customize1"];
                objArray[0, 6] = dr["SciDelivery"];
                objArray[0, 7] = dr["BuyerDelivery"];
                objArray[0, 8] = dr["ShipmodeID"];
                objArray[0, 9] = dr["TotalCTN"];
                objArray[0, 10] = dr["ClogCTN"];
                objArray[0, 11] = string.Format("=J{0}-K{0}", MyUtility.Convert.GetString(intRowsStart));
                objArray[0, 12] = string.Format("=IF(J{0}=0,0,ROUND(K{0}/J{0},2)*100)", MyUtility.Convert.GetString(intRowsStart));
                objArray[0, 13] = dr["PulloutCTNQty"];
                objArray[0, 14] = dr["TtlGMTQty"];
                objArray[0, 15] = dr["TtlClogGMTQty"];
                objArray[0, 16] = string.Format("=O{0}-P{0}", MyUtility.Convert.GetString(intRowsStart));
                objArray[0, 17] = string.Format("=IF(O{0}=0,0,ROUND(P{0}/O{0},2)*100)", MyUtility.Convert.GetString(intRowsStart));
                objArray[0, 18] = dr["TtlPullGMTQty"];
                objArray[0, 19] = dr["CTNQty"];
                objArray[0, 20] = dr["ClogQty"];
                objArray[0, 21] = string.Format("=T{0}-U{0}", MyUtility.Convert.GetString(intRowsStart));
                objArray[0, 22] = string.Format("=IF(T{0}=0,0,ROUND(U{0}/T{0},2)*100)", MyUtility.Convert.GetString(intRowsStart));
                objArray[0, 23] = dr["PullQty"];
                objArray[0, 24] = dr["GMTQty"];
                objArray[0, 25] = dr["ClogGMTQty"];
                objArray[0, 26] = string.Format("=Y{0}-Z{0}", MyUtility.Convert.GetString(intRowsStart));
                objArray[0, 27] = string.Format("=IF(Y{0}=0,0,ROUND(Z{0}/Y{0},2)*100)", MyUtility.Convert.GetString(intRowsStart));
                objArray[0, 28] = dr["PullGMTQty"];
                worksheet.Range[String.Format("A{0}:AC{0}", intRowsStart)].Value2 = objArray;
                intRowsStart++;
            }

            excel.Cells.EntireColumn.AutoFit();
            excel.Cells.EntireRow.AutoFit();
            this.HideWaitMessage();
            excel.Visible = true;
            return true;
        }
    }
}
