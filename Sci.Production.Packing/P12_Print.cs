using System;
using System.Data;
using System.Runtime.InteropServices;
using System.Text;
using Ict;

namespace Sci.Production.Packing
{
    /// <summary>
    /// Packing_P12_Print
    /// </summary>
    public partial class P12_Print : Win.Tems.PrintForm
    {
        private DataTable gridData;
        private string delDate1;
        private string delDate2;
        private string sciDate1;
        private string sciDate2;
        private string brand;
        private DataRow[] printData;

        /// <summary>
        /// P12_Print
        /// </summary>
        /// <param name="gridData">GridData</param>
        public P12_Print(DataTable gridData)
        {
            this.InitializeComponent();
            this.gridData = gridData;
        }

        /// <summary>
        /// ValidateInput驗證輸入條件
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(this.dateDelivery.Value1) && MyUtility.Check.Empty(this.dateDelivery.Value2) && MyUtility.Check.Empty(this.dateSCIDelivery.Value1) && MyUtility.Check.Empty(this.dateSCIDelivery.Value2))
            {
                MyUtility.Msg.WarningBox("Delivery can't empty!!");
                return false;
            }

            this.delDate1 = MyUtility.Check.Empty(this.dateDelivery.Value1) ? string.Empty : Convert.ToDateTime(this.dateDelivery.Value1).ToString("d");
            this.delDate2 = MyUtility.Check.Empty(this.dateDelivery.Value2) ? string.Empty : Convert.ToDateTime(this.dateDelivery.Value2).ToString("d");
            this.sciDate1 = MyUtility.Check.Empty(this.dateSCIDelivery.Value1) ? string.Empty : Convert.ToDateTime(this.dateSCIDelivery.Value1).ToString("d");
            this.sciDate2 = MyUtility.Check.Empty(this.dateSCIDelivery.Value2) ? string.Empty : Convert.ToDateTime(this.dateSCIDelivery.Value2).ToString("d");
            this.brand = this.txtbrand.Text;
            return base.ValidateInput();
        }

        /// <summary>
        /// OnAsyncDataLoad非同步取資料
        /// </summary>
        /// <param name="e">e</param>
        /// <returns>DualResult</returns>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder filter = new StringBuilder();
            if (!MyUtility.Check.Empty(this.delDate1))
            {
                filter.Append(string.Format("BuyerDelivery >= '{0}' and ", Convert.ToDateTime(this.delDate1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.delDate2))
            {
                filter.Append(string.Format("BuyerDelivery <= '{0}' and ", Convert.ToDateTime(this.delDate2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.sciDate1))
            {
                filter.Append(string.Format("SciDelivery >= '{0}' and ", Convert.ToDateTime(this.sciDate1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.sciDate2))
            {
                filter.Append(string.Format("SciDelivery <= '{0}' and ", Convert.ToDateTime(this.sciDate2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.brand))
            {
                filter.Append(string.Format("BrandID = '{0}' and ", this.brand));
            }

            this.printData = this.gridData.Select(string.Format("{0}", filter.ToString().Substring(0, filter.ToString().Length - 5)));
            return Ict.Result.True;
        }

        /// <summary>
        /// OnToExcel產生Excel
        /// </summary>
        /// <param name="report">report</param>
        /// <returns>bool</returns>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            if (this.printData.Length <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            string strXltName = Env.Cfg.XltPathDir + "\\Packing_P12_Print.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return false;
            }

            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];
            int intRowsStart = 2;
            int rownum = 0, counter = 0;
            object[,] objArray = new object[1, 32];
            foreach (DataRow dr in this.printData)
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
                worksheet.Range[string.Format("A{0}:AF{0}", rownum)].Value2 = objArray;
                counter++;
            }
            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("Packing_P12_Print");
            Microsoft.Office.Interop.Excel.Workbook workbook = excel.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(worksheet);
            Marshal.ReleaseComObject(workbook);

            strExcelName.OpenFile();
            #endregion
            return true;
        }
    }
}
