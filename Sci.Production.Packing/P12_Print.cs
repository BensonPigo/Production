using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Packing
{
    public partial class P12_Print : Sci.Win.Tems.PrintForm
    {
        private DataTable gridData;
        private string delDate1, delDate2, sciDate1, sciDate2, brand;
        private DataRow[] printData;
        
        public P12_Print(DataTable GridData)
        {
            InitializeComponent();
            gridData = GridData;
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(dateRange1.Value1) && MyUtility.Check.Empty(dateRange1.Value2) && MyUtility.Check.Empty(dateRange2.Value1) && MyUtility.Check.Empty(dateRange2.Value2))
            {
                MyUtility.Msg.WarningBox("Delivery can't empty!!");
                return false;
            }
            delDate1 = MyUtility.Check.Empty(dateRange1.Value1) ? "" : Convert.ToDateTime(dateRange1.Value1).ToString("d");
            delDate2 = MyUtility.Check.Empty(dateRange1.Value2) ? "" : Convert.ToDateTime(dateRange1.Value2).ToString("d");
            sciDate1 = MyUtility.Check.Empty(dateRange2.Value1) ? "" : Convert.ToDateTime(dateRange2.Value1).ToString("d");
            sciDate2 = MyUtility.Check.Empty(dateRange2.Value2) ? "" : Convert.ToDateTime(dateRange2.Value2).ToString("d");
            brand = txtbrand1.Text;
            return base.ValidateInput();
        }

        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder filter = new StringBuilder();
            if (!MyUtility.Check.Empty(delDate1))
            {
                filter.Append(string.Format("BuyerDelivery >= '{0}' and BuyerDelivery <= '{1}' and ", delDate1, delDate2));
            }
            if (!MyUtility.Check.Empty(sciDate1))
            {

                filter.Append(string.Format("SciDelivery >= '{0}' and SciDelivery <= '{1}' and ", sciDate1, sciDate2));
            }
            if (!MyUtility.Check.Empty(brand))
            {

                filter.Append(string.Format("BrandID = '{0}' and ", brand));
            }

            printData = gridData.Select(string.Format("{0}", filter.ToString().Substring(0, filter.ToString().Length-5)));
            return Result.True;
        }

        // 產生Excel
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            if (printData.Length <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            string strXltName = Sci.Env.Cfg.XltPathDir + "\\Packing_P12_Print.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null) return false;
            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];
            int intRowsStart = 2;
            int rownum = 0, counter = 0;
            object[,] objArray = new object[1, 32];
            foreach (DataRow dr in printData)
            {
                rownum = intRowsStart + counter;
                objArray[0, 0] = dr["FactoryID"];
                objArray[0, 1] = dr["BrandID"];
                objArray[0, 2] = dr["SewLine"];
                objArray[0, 3] = dr["ID"];
                objArray[0, 4] = dr["StyleID"];
                objArray[0, 5] = dr["CustPONo"];
                objArray[0, 6] = dr["CustCDID"];
                objArray[0, 7] = dr["Customize2"];
                objArray[0, 8] = dr["DoxType"];
                objArray[0, 9] = dr["Qty"];
                objArray[0, 10] = dr["Alias"];
                objArray[0, 11] = dr["SewOffLine"];
                objArray[0, 12] = dr["InspDate"];
                objArray[0, 13] = dr["SDPDate"];
                objArray[0, 14] = dr["EstPulloutDate"];
                objArray[0, 15] = dr["Seq"];
                objArray[0, 16] = dr["ShipmodeID"];
                objArray[0, 17] = dr["BuyerDelivery"];
                objArray[0, 18] = dr["SciDelivery"];
                objArray[0, 19] = dr["CRDDate"];
                objArray[0, 20] = dr["BuyMonth"];
                objArray[0, 21] = dr["Customize1"];
                objArray[0, 22] = dr["ScanAndPack"];
                objArray[0, 23] = dr["RainwearTestPassed"];
                objArray[0, 24] = dr["CTNQty"];
                objArray[0, 25] = dr["Dimension"];
                objArray[0, 26] = dr["ProdRemark"];
                objArray[0, 27] = dr["ShipRemark"];
                objArray[0, 28] = dr["MtlFormA"];
                objArray[0, 29] = dr["InClogCTN"];
                objArray[0, 30] = dr["CBM"];
                objArray[0, 31] = dr["ClogLocationId"];
                worksheet.Range[String.Format("A{0}:AF{0}", rownum)].Value2 = objArray;
                counter++;
            }
            excel.Visible = true;
            return true;
        }
    }
}
