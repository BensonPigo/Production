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
            DBProxy.Current.Select(null, "select '' as ID union all select ID from MDivision", out mDivision);
            MyUtility.Tool.SetupCombox(comboBox1, 1, mDivision);
            comboBox1.Text = Sci.Env.User.Keyword;
        }


        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(dateRange1.Value1) && MyUtility.Check.Empty(dateRange2.Value1))
            {
                MyUtility.Msg.WarningBox("Buyer Delivery or SCI Delivery can't empty!!");
                return false;
            }

            buyerDelivery1 = dateRange1.Value1;
            buyerDelivery2 = dateRange1.Value2;
            sciDelivery1 = dateRange2.Value1;
            sciDelivery2 = dateRange2.Value2;
            mDivision = comboBox1.Text;
            brand = txtbrand1.Text;

            return base.ValidateInput();
        }

        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(@"select o.FactoryID,o.MCHandle,o.SewLine,o.ID,o.CustPONo,o.Customize1,oq.BuyerDelivery,oq.ShipmodeID,oq.Seq,o.SciDelivery,o.TotalCTN,o.ClogCTN,o.PulloutCTNQty,
isnull((select sum(CTNQty) from PackingList_Detail where OrderID = o.ID and OrderShipmodeSeq = oq.Seq),0) as CTNQty,
isnull((select sum(CTNQty) from PackingList_Detail where OrderID = o.ID and OrderShipmodeSeq = oq.Seq and ReceiveDate is not null),0) as ClogQty,
isnull((select sum(pd.CTNQty) from PackingList p, PackingList_Detail pd where p.ID = pd.ID and pd.OrderID = o.ID and pd.OrderShipmodeSeq = oq.Seq and p.PulloutID != ''),0) as PullQty,
isnull((select sum(ShipQty) from PackingList_Detail where OrderID = o.ID),0) as TtlGMTQty,
isnull((select sum(ShipQty) from PackingList_Detail where OrderID = o.ID and ReceiveDate is not null),0) as TtlClogGMTQty,
isnull((select sum(ShipQty) from Pullout p,Pullout_Detail pd where pd.OrderID = o.ID and pd.ID = p.ID and p.Status <> 'New'),0) as TtlPullGMTQty,
isnull((select sum(ShipQty) from PackingList_Detail where OrderID = o.ID and OrderShipmodeSeq = oq.Seq),0) as GMTQty,
isnull((select sum(ShipQty) from PackingList_Detail where OrderID = o.ID and OrderShipmodeSeq = oq.Seq and ReceiveDate is not null),0) as ClogGMTQty,
isnull((select sum(ShipQty) from Pullout p,Pullout_Detail pd where pd.OrderID = o.ID and pd.OrderShipmodeSeq = oq.Seq and pd.ID = p.ID and p.Status <> 'New'),0) as PullGMTQty
from Orders o
inner join Order_QtyShip oq on o.ID = oq.Id
where o.Category = 'B'");

            if (!MyUtility.Check.Empty(buyerDelivery1))
            {
                sqlCmd.Append(string.Format(" and oq.BuyerDelivery between '{0}' and '{1}'", Convert.ToDateTime(buyerDelivery1).ToString("d"), Convert.ToDateTime(buyerDelivery2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(sciDelivery1))
            {
                sqlCmd.Append(string.Format(" and o.SciDelivery between '{0}' and '{1}'", Convert.ToDateTime(sciDelivery1).ToString("d"), Convert.ToDateTime(sciDelivery2).ToString("d")));
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

            MyUtility.Msg.WaitWindows("Starting EXCEL...");
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
            MyUtility.Msg.WaitClear();
            excel.Visible = true;
            return true;
        }
    }
}
