using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;
using Sci.Production.Prg;

namespace Sci.Production.Centralized
{
    /// <summary>
    /// R04
    /// </summary>
    public partial class R04 : Win.Tems.PrintForm
    {
        private DataTable printData;
        private DataTable dtAllData;
        private DateTime? date1;
        private DateTime? date2;
        private string category;
        private string mDivision;
        private string factory;
        private string brand;
        private string cdcode;
        private string shift;
        private string productType;
        private string fabricType;
        private string lining;
        private string gender;
        private string construction;
        private bool show_Accumulate_output;
        private bool exclude_NonRevenue;

        /// <summary>
        /// R04
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R04(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.comboM.Text = Env.User.Keyword;
            this.comboProductType1.SetDataSource();
            this.comboFabricType1.SetDataSource();
            this.comboLining1.SetDataSource();
            this.comboGender1.SetDataSource();
            this.comboConstruction1.SetDataSource();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            #region load combobox Location M 預設顯示登入的M
            this.comboM.SetDefalutIndex();
            this.comboM.Text = Env.User.Keyword;
            #endregion

            #region load combobox Factory 預設顯示空白
            this.comboFactory.SetDefalutIndex(string.Empty);
            #endregion

            MyUtility.Tool.SetupCombox(this.comboShift, 1, 1, ",Day+Night,Subcon-In,Subcon-Out");
            this.comboCategory.SelectedIndex = 0;
            base.OnFormLoaded();
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.date1 = this.dateOoutputDate.Value1;
            this.date2 = this.dateOoutputDate.Value2;
            this.category = this.comboCategory.Text;
            this.mDivision = this.comboM.Text;
            this.factory = this.comboFactory.Text;
            this.brand = this.txtbrand.Text;
            this.cdcode = this.txtCDCode.Text;
            this.shift = this.comboShift.Text;
            this.show_Accumulate_output = this.chk_Accumulate_output.Checked;
            this.exclude_NonRevenue = this.chkExcludeNonRevenue.Checked;
            this.productType = this.comboProductType1.Text;
            this.fabricType = this.comboFabricType1.Text;
            this.lining = this.comboLining1.Text;
            this.gender = this.comboGender1.Text;
            this.construction = this.comboConstruction1.Text;

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1513:ClosingBraceMustBeFollowedByBlankLine", Justification = "Reviewed.")]
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            DBProxy.Current.DefaultTimeout = 1800;  // timeout時間改為30分鐘
            this.dtAllData = null;

            List<SqlParameter> listPar = new List<SqlParameter>()
            {
                new SqlParameter("@M", this.mDivision),
                new SqlParameter("@Factory", this.factory),
                new SqlParameter("@Category", this.category),
                new SqlParameter("@Brand", this.brand),
                new SqlParameter("@CDCode", this.cdcode),
                new SqlParameter("@ProductType", this.productType),
                new SqlParameter("@FabricType", this.fabricType),
                new SqlParameter("@Lining", this.lining),
                new SqlParameter("@Construction", this.construction),
                new SqlParameter("@Gender", this.gender),
                new SqlParameter("@Shift", this.shift),
                new SqlParameter("@Include_Artwork", this.chk_Include_Artwork.Checked),
                new SqlParameter("@ShowAccumulate_output", this.show_Accumulate_output),
                new SqlParameter("@ExcludeSampleFty", this.chkExcludeSampleFty.Checked),
                new SqlParameter("@OnlyCancelOrder", this.chkOnlyCancelOrder.Checked),
                new SqlParameter("@ExcludeNonRevenue", this.exclude_NonRevenue),
                new SqlParameter("@SubconOut", this.chkSubconOut.Checked),
            };

            if (this.date1 == null)
            {
                listPar.Add(new SqlParameter("@StartDate", DBNull.Value));
            }
            else
            {
                listPar.Add(new SqlParameter("@StartDate", this.date1));
            }

            if (this.date2 == null)
            {
                listPar.Add(new SqlParameter("@EndDate", DBNull.Value));
            }
            else
            {
                listPar.Add(new SqlParameter("@EndDate", this.date2));
            }

            string sqlGetSewingDailyOutputList = @"
exec GetSewingDailyOutputList   @M						
	                            ,@Factory	
	                            ,@StartDate    
	                            ,@EndDate      
	                            ,@Category	
	                            ,@Brand		   
	                            ,@CDCode	
	                            ,@ProductType
	                            ,@FabricType
	                            ,@Lining	
	                            ,@Construction
	                            ,@Gender	
	                            ,@Shift		
	                            ,@Include_Artwork			
	                            ,@ShowAccumulate_output	
	                            ,@ExcludeSampleFty		
	                            ,@OnlyCancelOrder		
	                            ,@ExcludeNonRevenue		
	                            ,@SubconOut				
";

            #region --由 appconfig 抓各個連線路徑
            this.SetLoadingText("Load connections... ");
            List<string> connectionString = CentralizedClass.AllFactoryConnectionString();

            if (connectionString == null || connectionString.Count == 0)
            {
                return new DualResult(false, "no connection loaded.");
            }
            #endregion

            DualResult result = new DualResult(true);

            foreach (string conString in connectionString)
            {
                SqlConnection conn;
                using (conn = new SqlConnection(conString))
                {
                    conn.Open();
                    result = DBProxy.Current.SelectByConn(conn, sqlGetSewingDailyOutputList, listPar, out this.printData);
                    if (!result)
                    {
                        DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                        return failResult;
                    }

                    if (this.printData != null && this.printData.Rows.Count > 0)
                    {
                        if (this.dtAllData == null)
                        {
                            this.dtAllData = this.printData;
                        }
                        else
                        {
                            this.dtAllData.Merge(this.printData);
                        }
                    }
                }
            }

            DBProxy.Current.DefaultTimeout = 300;  // timeout時間改回5分鐘
            return Ict.Result.True;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            int start_column;
            if (this.dtAllData == null || this.dtAllData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            this.SetCount(this.dtAllData.Rows.Count);

            this.ShowWaitMessage("Starting EXCEL...");
            string excelFile = "Sewing_R04_SewingDailyOutputList.xltx";
            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + excelFile); // 開excelapp
            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
            if (this.show_Accumulate_output == true)
            {
                start_column = 49;
            }
            else
            {
                start_column = 47;
                objSheets.get_Range("AS:AT").EntireColumn.Delete();
            }

            for (int i = start_column; i < this.dtAllData.Columns.Count; i++)
            {
                objSheets.Cells[1, i + 1] = this.dtAllData.Columns[i].ColumnName;
            }

            string r = MyUtility.Excel.ConvertNumericToExcelColumn(this.dtAllData.Columns.Count);
            objSheets.get_Range("A1", r + "1").Cells.Interior.Color = Color.LightGreen;
            objSheets.get_Range("A1", r + "1").AutoFilter(1);
            bool result = MyUtility.Excel.CopyToXls(this.dtAllData, string.Empty, xltfile: excelFile, headerRow: 1, excelApp: objApp);

            if (!result)
            {
                MyUtility.Msg.WarningBox(result.ToString(), "Warning");
            }

            this.HideWaitMessage();
            return true;
        }
    }
}
