using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using System.Runtime.InteropServices;
using System.Xml.Linq;
using System.Configuration;
using System.Linq;
using System.Data.SqlClient;

namespace Sci.Production.Centralized
{
    /// <summary>
    /// R07
    /// </summary>
    public partial class R07 : Win.Tems.PrintForm
    {
        private string factory;
        private string mdivision;
        private string cdCode;
        private string shift;
        private DateTime? outputDate1;
        private DateTime? outputDate2;
        private DataTable printData;
        private StringBuilder condition = new StringBuilder();

        /// <summary>
        /// R07
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R07(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            MyUtility.Tool.SetupCombox(this.comboShift, 2, 1, ",,D,Day,N,Night");
            this.comboM.SetDefalutIndex();
            this.comboFactory.SetDefalutIndex(string.Empty);
        }

        /// <summary>
        /// ValidateInput
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(this.dateOutputDate.Value1))
            {
                MyUtility.Msg.WarningBox(" < Output Date > can't be empty!!");
                return false;
            }

            #region -- 擇一必輸的條件 --
            this.outputDate1 = this.dateOutputDate.Value1;
            this.outputDate2 = this.dateOutputDate.Value2;
            #endregion

            this.mdivision = this.comboM.Text;
            this.factory = this.comboFactory.Text;
            this.cdCode = this.txtCDCode.Text;
            this.shift = this.comboShift.SelectedValue.ToString();
            return base.ValidateInput();
        }

        /// <summary>
        /// OnAsyncDataLoad
        /// </summary>
        /// <param name="e">e</param>
        /// <returns>DualResult</returns>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            string sqlCmd = string.Format(
                "exec dbo.GetAdidasEfficiencyReport '{0}', '{1}', '{2}', '{3}', '{4}', '{5}'",
                this.outputDate1.Value.ToString("yyyy/MM/dd"),
                this.outputDate2.Value.ToString("yyyy/MM/dd"),
                this.mdivision,
                this.factory,
                this.cdCode,
                this.shift);

            DBProxy.Current.DefaultTimeout = 1800;  // timeout時間改為30分鐘

            #region --由 appconfig 抓各個連線路徑
            this.SetLoadingText("Load connections... ");
            XDocument docx = XDocument.Load(System.Windows.Forms.Application.ExecutablePath + ".config");
            string[] strSevers = ConfigurationManager.AppSettings["ServerMatchFactory"].Split(new char[] { ';' });
            List<string> connectionStrings = new List<string>(); // ←主要是要重組 List connectionString
            foreach (string ss in strSevers)
            {
                if (ss.IndexOf("testing_PMS", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    continue;
                }

                var connections = docx.Descendants("modules").Elements().Where(y => y.FirstAttribute.Value.Contains(ss.Split(new char[] { ':' })[0].ToString())).Descendants("connectionStrings").Elements().Where(x => x.FirstAttribute.Value.Contains("Production")).Select(z => z.LastAttribute.Value).ToList()[0].ToString();
                connectionStrings.Add(connections);
            }

            if (connectionStrings == null || connectionStrings.Count == 0)
            {
                return new DualResult(false, "no connection loaded.");
            }
            #endregion

            DualResult result = new DualResult(true);

            foreach (string conString in connectionStrings)
            {
                SqlConnection conn;
                using (conn = new SqlConnection(conString))
                {
                    conn.Open();
                    result = DBProxy.Current.SelectByConn(conn, sqlCmd, null, out DataTable dt);
                    if (!result)
                    {
                        DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                        return failResult;
                    }

                    if (this.printData == null)
                    {
                        this.printData = dt;
                    }
                    else
                    {
                        this.printData.Merge(dt);
                    }
                }
            }

            if (this.printData != null && this.printData.Rows.Count > 0)
            {
                sqlCmd = @"
select t.OutputDate
	, t.FactoryID
	, t.SewingLineID
	, t.Shift
	, t.Category
	, t.StyleID
	, t.Manpower
	, t.ManHour
	, t.TotalOutput
	, t.CD
	, t.SeasonID
	, t.BrandID
	, t.Fabrication
	, t.ProductGroup
	, t.ProductFabrication
	, [GSD] = iif(isnull(sl.Rate, 0) = 0 or isnull(sq.TMS, 0) = 0, 0, (sq.TMS / 60) * (sl.Rate / 100))
	, t.Earnedhours
	, t.TotalWorkingHours
	, t.CumulateDaysofDaysinProduction
	, t.EfficiencyLine
	, t.NoofInlineDefects
	, t.NoofEndlineDefectiveGarments
	, t.WFT
from #tmp t
left join Style s on t.StyleID = s.Id and t.BrandID = s.BrandID and t.SeasonID = s.SeasonID
left join Style_Quotation sq on s.Ukey = sq.StyleUkey and sq.ArtworkTypeID = 'SEWING' and sq.Article = ''
left join Style_Location sl on s.Ukey = sl.StyleUkey and RIGHT(t.CD, 1) = sl.Location";

                DBProxy.Current.OpenConnection("Trade", out SqlConnection sqlConnection);
                result = MyUtility.Tool.ProcessWithDatatable(this.printData, null, sqlCmd, out this.printData, conn: sqlConnection);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                    return failResult;
                }
            }

            // timeout時間改回5分鐘
            DBProxy.Current.DefaultTimeout = 300;
            return Ict.Result.True;
        }

        /// <summary>
        /// OnToExcel
        /// </summary>
        /// <param name="report">report</param>
        /// <returns>bool</returns>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            if (this.printData == null || this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.printData.Rows.Count);
            this.ShowWaitMessage("Starting EXCEL...");

            if (this.printData.Columns.Count > 16384)
            {
                MyUtility.Msg.WarningBox("Columns of Data is over 16,384 in excel file, please narrow down range of condition.");
                return false;
            }

            if (this.printData.Rows.Count + 6 > 1048576)
            {
                MyUtility.Msg.WarningBox("Lines of Data is over 1,048,576 in excel file, please narrow down range of condition.");
                return false;
            }

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Planning_R07.xltx"); // 預先開啟excel app
            MyUtility.Excel.CopyToXls(this.printData, string.Empty, "Planning_R07.xltx", 1, false, null, objApp);      // 將datatable copy to excel
            objApp.Visible = false;

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("Planning_R07");
            Microsoft.Office.Interop.Excel.Workbook workbook = objApp.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);
            Marshal.ReleaseComObject(workbook);

            strExcelName.OpenFile();
            #endregion
            this.HideWaitMessage();
            return true;
        }
    }
}
