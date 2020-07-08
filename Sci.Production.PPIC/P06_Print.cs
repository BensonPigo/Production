using System;
using System.Data;
using System.Runtime.InteropServices;
using System.Text;
using Ict;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// P06_Print
    /// </summary>
    public partial class P06_Print : Sci.Win.Tems.PrintForm
    {
        private DataTable gridData;
        private string delDate1;
        private string delDate2;
        private string sciDate1;
        private string sciDate2;
        private string brand;
        private DataRow[] printData;

        /// <summary>
        /// P06_Print
        /// </summary>
        /// <param name="gridData">DataTable GridData</param>
        public P06_Print(DataTable gridData)
        {
            this.InitializeComponent();
            this.gridData = gridData;
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
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
            return Result.True;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            if (this.printData.Length <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            string strXltName = Sci.Env.Cfg.XltPathDir + "\\PPIC_P06_Print.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return false;
            }

            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];
            int intRowsStart = 2;
            int rownum = 0, counter = 0;
            object[,] objArray = new object[1, 39];
            foreach (DataRow dr in this.printData)
            {
                rownum = intRowsStart + counter;
                objArray[0, 0] = dr["FactoryID"];
                objArray[0, 1] = dr["BrandID"];
                objArray[0, 2] = dr["SewLine"];
                objArray[0, 3] = dr["ID"];
                objArray[0, 4] = dr["StyleID"];
                objArray[0, 5] = dr["ColorWay"];
                objArray[0, 6] = dr["OrderType"];
                objArray[0, 7] = dr["Season"];
                objArray[0, 8] = dr["Program"];
                objArray[0, 9] = dr["CustPONo"];
                objArray[0, 10] = dr["CustCDID"];
                objArray[0, 11] = dr["Customize2"];
                objArray[0, 12] = dr["DoxType"];
                objArray[0, 13] = dr["Qty"];
                objArray[0, 14] = dr["Alias"];
                objArray[0, 15] = dr["SewInLine"];
                objArray[0, 16] = dr["SewOffLine"];
                objArray[0, 17] = dr["InspDate"];
                objArray[0, 18] = dr["SDPDate"];
                objArray[0, 19] = dr["EstPulloutDate"];
                objArray[0, 20] = dr["Seq"];
                objArray[0, 21] = dr["ShipmodeID"];
                objArray[0, 22] = dr["BuyerDelivery"];
                objArray[0, 23] = dr["SciDelivery"];
                objArray[0, 24] = dr["CRDDate"];
                objArray[0, 25] = dr["BuyMonth"];
                objArray[0, 26] = dr["Customize1"];
                objArray[0, 27] = dr["ScanAndPack"];
                objArray[0, 28] = dr["RainwearTestPassed"];
                objArray[0, 29] = dr["PackingMethod"];
                objArray[0, 30] = dr["CTNQty"];
                objArray[0, 31] = dr["CLOGQty"];
                objArray[0, 32] = dr["Dimension"];
                objArray[0, 33] = dr["ProdRemark"];
                objArray[0, 34] = dr["ShipRemark"];
                objArray[0, 35] = dr["MtlFormA"];
                objArray[0, 36] = dr["InClogCTN"];
                objArray[0, 37] = dr["CBM"];
                objArray[0, 38] = dr["ClogLocationId"];
                worksheet.Range[string.Format("A{0}:AF{0}", rownum)].Value2 = objArray;
                counter++;
            }

            #region Save & Show Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("PPIC_P06_Print");
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
