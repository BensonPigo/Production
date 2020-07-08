using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using System.Runtime.InteropServices;

namespace Sci.Production.Planning
{
    /// <summary>
    /// R03
    /// </summary>
    public partial class R03 : Win.Tems.PrintForm
    {
        private DataTable printData;
        private DateTime? sciDate1;
        private DateTime? sciDate2;
        private string style;
        private string brand;
        private string season;
        private string smr;
        private string subcon;

        /// <summary>
        /// R03
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R03(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// ValidateInput
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ValidateInput()
        {
            this.style = this.txtstyle.Text;
            this.brand = this.txtbrand.Text;
            this.season = this.txtseason.Text;
            this.smr = this.txttpeuser_caneditSMR.TextBox1.Text;
            this.subcon = this.txtsubconSupplier.TextBox1.Text;
            this.sciDate1 = this.dateSCIDelivery.Value1;
            this.sciDate2 = this.dateSCIDelivery.Value2;

            return base.ValidateInput();
        }

        /// <summary>
        /// OnAsyncDataLoad
        /// </summary>
        /// <param name="e">e</param>
        /// <returns>DualResult</returns>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(@"select distinct s.ID,s.BrandID,s.SeasonID,sa.ArtworkTypeID,sa.Article,sa.ArtworkID,sa.ArtworkName,
sa.PatternCode,sa.PatternDesc,sa.TMS,sa.Qty,isnull(a.ArtworkUnit,'') as ArtworkUnit,sa.Cost,
saq.LocalSuppId+'-'+isnull(ls.Abb,'') as LocalSupp,saq.CurrencyId,saq.Price,saq.Oven,saq.Wash,saq.Mockup,saq.PriceApv
from Orders o WITH (NOLOCK) 
inner join Style s  WITH (NOLOCK) on o.StyleUkey = s.Ukey
inner join Style_Artwork sa WITH (NOLOCK) on sa.StyleUkey = o.StyleUkey and sa.StyleUkey = s.Ukey
inner join Style_Artwork_Quot saq WITH (NOLOCK) on sa.Ukey = saq.Ukey
left join LocalSupp ls WITH (NOLOCK) on ls.ID = saq.LocalSuppId
left join ArtworkType a WITH (NOLOCK) on a.ID = sa.ArtworkTypeID
where 1 = 1");

            if (!MyUtility.Check.Empty(this.style))
            {
                sqlCmd.Append(string.Format(@" and s.ID = '{0}' and o.StyleID = '{0}'", this.style));
            }

            if (!MyUtility.Check.Empty(this.brand))
            {
                sqlCmd.Append(string.Format(@" and s.BrandID = '{0}' and o.BrandID = '{0}'", this.brand));
            }

            if (!MyUtility.Check.Empty(this.season))
            {
                sqlCmd.Append(string.Format(@" and s.SeasonID = '{0}' and o.SeasonID = '{0}'", this.season));
            }

            if (!MyUtility.Check.Empty(this.smr))
            {
                sqlCmd.Append(string.Format(@" and ((s.Phase = 'Sample' and s.SampleSMR = '{0}') or (s.Phase ='Bulk' and s.BulkSMR = '{0}'))", this.smr));
            }

            if (!MyUtility.Check.Empty(this.subcon))
            {
                sqlCmd.Append(string.Format(" and saq.LocalSuppId = '{0}'", this.subcon));
            }

            if (!MyUtility.Check.Empty(this.sciDate1))
            {
                sqlCmd.Append(string.Format(" and o.SciDelivery >= '{0}'", Convert.ToDateTime(this.sciDate1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.sciDate2))
            {
                sqlCmd.Append(string.Format(" and o.SciDelivery <= '{0}'", Convert.ToDateTime(this.sciDate2).ToString("d")));
            }

            sqlCmd.Append(" order by s.ID,s.BrandID,s.SeasonID,sa.ArtworkTypeID");

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out this.printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return Result.True;
        }

        /// <summary>
        /// OnToExcel
        /// </summary>
        /// <param name="report">report</param>
        /// <returns>bool</returns>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            this.SetCount(this.printData.Rows.Count);

            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            this.ShowWaitMessage("Starting EXCEL...");
            string strXltName = Sci.Env.Cfg.XltPathDir + "\\Planning_R03_LocalQuotationList.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return false;
            }

            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            int intRowsStart = 2;
            object[,] objArray = new object[1, 20];
            foreach (DataRow dr in this.printData.Rows)
            {
                objArray[0, 0] = dr["ID"];
                objArray[0, 1] = dr["BrandID"];
                objArray[0, 2] = dr["SeasonID"];
                objArray[0, 3] = dr["ArtworkTypeID"];
                objArray[0, 4] = dr["Article"];
                objArray[0, 5] = dr["ArtworkID"];
                objArray[0, 6] = dr["ArtworkName"];
                objArray[0, 7] = dr["PatternCode"];
                objArray[0, 8] = dr["PatternDesc"];
                objArray[0, 9] = dr["TMS"];
                objArray[0, 10] = dr["Qty"];
                objArray[0, 11] = dr["ArtworkUnit"];
                objArray[0, 12] = dr["Cost"];
                objArray[0, 13] = dr["LocalSupp"];
                objArray[0, 14] = dr["CurrencyId"];
                objArray[0, 15] = dr["Price"];
                objArray[0, 16] = dr["Oven"];
                objArray[0, 17] = dr["Wash"];
                objArray[0, 18] = dr["Mockup"];
                objArray[0, 19] = dr["PriceApv"];

                worksheet.Range[string.Format("A{0}:T{0}", intRowsStart)].Value2 = objArray;
                intRowsStart++;
            }

            excel.Cells.EntireColumn.AutoFit();
            excel.Cells.EntireRow.AutoFit();

            #region Save & Show Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Planning_R03_LocalQuotationList");
            Microsoft.Office.Interop.Excel.Workbook workbook = excel.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(worksheet);
            Marshal.ReleaseComObject(workbook);

            strExcelName.OpenFile();
            #endregion
            this.HideWaitMessage();
            return true;
        }
    }
}
