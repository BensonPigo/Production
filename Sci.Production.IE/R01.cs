using System.ComponentModel;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using System.Runtime.InteropServices;

namespace Sci.Production.IE
{
    /// <summary>
    /// IE_R01
    /// </summary>
    public partial class R01 : Win.Tems.PrintForm
    {
        private string factory;
        private string style;
        private string season;
        private string team;
        private string inline1;
        private string inline2;
        private DataTable printData;

        /// <summary>
        /// R01
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            MyUtility.Tool.SetupCombox(this.comboTeam, 1, 1, ",A,B");
        }

        // Factory
        private void TxtFactory_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sqlCmd = "select distinct FTYGroup from Factory WITH (NOLOCK) where Junk = 0 AND FTYGroup!=''";

            Win.Tools.SelectItem item = new Win.Tools.SelectItem(sqlCmd, "8", this.txtFactory.Text, "Factory");
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            this.txtFactory.Text = item.GetSelectedString();
        }

        // Style
        private void TxtStyle_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sqlCmd = "select distinct ID,BrandID,Description from Style WITH (NOLOCK) where Junk = 0 order by ID";

            Win.Tools.SelectItem item = new Win.Tools.SelectItem(sqlCmd, "16,10,50", this.txtStyle.Text, "Style#,Brand,Description");
            item.Width = 800;
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            this.txtStyle.Text = item.GetSelectedString();
        }

        // Season
        private void TxtSeason_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sqlCmd = "select distinct ID from Season WITH (NOLOCK) where Junk = 0 ORDER BY ID DESC";
            Win.Tools.SelectItem item = new Win.Tools.SelectItem(sqlCmd, "10", this.txtSeason.Text, "Season");
            item.Width = 300;
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            this.txtSeason.Text = item.GetSelectedString();
        }

        /// <summary>
        /// ValidateInput 驗證輸入條件
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ValidateInput()
        {
            this.factory = this.txtFactory.Text;
            this.style = this.txtStyle.Text;
            this.season = this.txtSeason.Text;
            this.team = this.comboTeam.SelectedIndex == -1 || this.comboTeam.SelectedIndex == 0 ? string.Empty : this.comboTeam.SelectedIndex == 1 ? "A" : "B";
            this.inline1 = string.Format("{0:yyyy-MM-dd}", this.dateInlineDate.Value1);
            this.inline2 = string.Format("{0:yyyy-MM-dd}", this.dateInlineDate.Value2);

            return base.ValidateInput();
        }

        /// <summary>
        /// OnAsyncDataLoad 非同步取資料
        /// </summary>
        /// <param name="e">e</param>
        /// <returns>DualResult</returns>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(@"
select lm.FactoryID,
	   lm.StyleID,
	   lm.ComboType,
	   lm.SeasonID,
	   lm.BrandID,
	   lm.Version,
	   lm.SewingLineID,
       lm.Team,
       lm.CurrentOperators,
       lm.StandardOutput,
       lm.TaktTime,
       lm.TotalGSD,
       lm.TotalCycle,
	   IIF(lm.HighestCycle = 0,0,Round(3600.0/lm.HighestCycle,2)) as EOLR,
	   IIF(lm.HighestCycle*lm.CurrentOperators = 0,0,CONVERT(DECIMAL,lm.TotalGSD)/lm.HighestCycle/lm.CurrentOperators) as Eff,
	   IIF(lm.HighestCycle*lm.CurrentOperators = 0,0,CONVERT(DECIMAL,lm.TotalCycle)/lm.HighestCycle/lm.CurrentOperators) as LB,
	   IIF(lm.TaktTime*lm.CurrentOperators = 0,0,CONVERT(DECIMAL,lm.TotalCycle)/lm.TaktTime/lm.CurrentOperators) as LLEF,
	   IIF(lm.HighestCycle * lm.CurrentOperators = 0,0,(ROUND(3600.0/lm.HighestCycle, 2) * (select isnull(CPU,0) from Style WITH (NOLOCK) where lm.BrandID = BrandID and lm.StyleID = ID and lm.SeasonID = SeasonID)/lm.CurrentOperators)) as PPH,
	   isnull((select Name from Pass1 WITH (NOLOCK) where ID = lm.AddName),'') as CreateBy,lm.AddDate,
	   isnull((select Name from Pass1 WITH (NOLOCK) where ID = lm.EditName),'') as EditBy,lm.EditDate
from LineMapping lm WITH (NOLOCK) 
");
            if (!MyUtility.Check.Empty(this.inline1) || !MyUtility.Check.Empty(this.inline2))
            {
                string dateQuery = string.Empty;
                if (!MyUtility.Check.Empty(this.inline1))
                {
                    dateQuery += string.Format("and '{0}' <= convert(varchar(10), Inline, 120) ", this.inline1);
                }

                if (!MyUtility.Check.Empty(this.inline2))
                {
                    dateQuery += string.Format("and convert(varchar(10), Inline, 120) <= '{0}' ", this.inline2);
                }

                sqlCmd.Append(string.Format(
                    @"
inner join(
	select distinct Orders.StyleID
			, Orders.SeasonID
			, Orders.BrandID
	from SewingSchedule
	join Orders on SewingSchedule.OrderID = Orders.ID
	where Orders.Finished = 1 {0}
) s on lm.StyleID = s.StyleID and lm.SeasonID = s.SeasonID and lm.BrandID = s.BrandID
", dateQuery));
            }

            sqlCmd.Append("where 1 = 1");
            if (!MyUtility.Check.Empty(this.factory))
            {
                sqlCmd.Append(string.Format(" and lm.FactoryID = '{0}'", this.factory));
            }

            if (!MyUtility.Check.Empty(this.style))
            {
                sqlCmd.Append(string.Format(" and lm.StyleID = '{0}'", this.style));
            }

            if (!MyUtility.Check.Empty(this.season))
            {
                sqlCmd.Append(string.Format(" and lm.SeasonID = '{0}'", this.season));
            }

            if (!MyUtility.Check.Empty(this.team))
            {
                sqlCmd.Append(string.Format(" and lm.Team = '{0}'", this.team));
            }

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out this.printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return Result.True;
        }

        /// <summary>
        /// OnToExcel 產生Excel
        /// </summary>
        /// <param name="report">report</param>
        /// <returns>bool</returns>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.printData.Rows.Count);

            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            this.ShowWaitMessage("Starting EXCEL...");
            string strXltName = Sci.Env.Cfg.XltPathDir + "\\IE_R01_LineMappingList.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return false;
            }

            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            // 填內容值
            int intRowsStart = 2;
            object[,] objArray = new object[1, 22];
            foreach (DataRow dr in this.printData.Rows)
            {
                objArray[0, 0] = dr["FactoryID"];
                objArray[0, 1] = dr["StyleID"];
                objArray[0, 2] = dr["ComboType"];
                objArray[0, 3] = dr["SeasonID"];
                objArray[0, 4] = dr["BrandID"];
                objArray[0, 5] = dr["Version"];
                objArray[0, 6] = dr["SewingLineID"];
                objArray[0, 7] = dr["Team"];
                objArray[0, 8] = dr["CurrentOperators"];
                objArray[0, 9] = dr["StandardOutput"];
                objArray[0, 10] = dr["TaktTime"];
                objArray[0, 11] = dr["TotalGSD"];
                objArray[0, 12] = dr["TotalCycle"];
                objArray[0, 13] = dr["EOLR"];
                objArray[0, 14] = dr["Eff"];
                objArray[0, 15] = dr["LB"];
                objArray[0, 16] = dr["LLEF"];
                objArray[0, 17] = dr["PPH"];
                objArray[0, 18] = dr["CreateBy"];
                objArray[0, 19] = dr["AddDate"];
                objArray[0, 20] = dr["EditBy"];
                objArray[0, 21] = dr["EditDate"];

                worksheet.Range[string.Format("A{0}:V{0}", intRowsStart)].Value2 = objArray;
                intRowsStart++;
            }

            excel.Cells.EntireColumn.AutoFit();
            excel.Cells.EntireRow.AutoFit();
            this.HideWaitMessage();

            #region Save & Show Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("IE_R01_LineMappingList");
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

        private void TxtFactory_Validating(object sender, CancelEventArgs e)
        {
            DataTable factoryData;
            string fac = string.Empty;
            string sqlCmd = "select distinct FTYGroup from Factory WITH (NOLOCK) where Junk = 0 AND FTYGroup!=''";
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out factoryData);
            foreach (DataRow dr in factoryData.Rows)
            {
                fac = dr["FTYGroup"].ToString();
                if (this.txtFactory.Text == fac)
                {
                    return;
                }
            }

            if (this.txtFactory.Text == string.Empty)
            {
                this.txtFactory.Text = string.Empty;
                return;
            }

            if (this.txtFactory.Text != fac)
            {
                this.txtFactory.Text = string.Empty;
                MyUtility.Msg.WarningBox("This Factory is wrong!");
                return;
            }
        }

        private void TxtStyle_Validating(object sender, CancelEventArgs e)
        {
            DataTable styleData;
            string sty = string.Empty;
            string sqlCmd = "select distinct ID,BrandID,Description from Style WITH (NOLOCK) where Junk = 0 order by ID";
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out styleData);
            foreach (DataRow dr in styleData.Rows)
            {
                sty = dr["ID"].ToString();
                if (this.txtStyle.Text == sty)
                {
                    return;
                }
            }

            if (this.txtStyle.Text == string.Empty)
            {
                this.txtStyle.Text = string.Empty;
                return;
            }

            if (this.txtStyle.Text != sty)
            {
                this.txtStyle.Text = string.Empty;
                MyUtility.Msg.WarningBox("This Style# is wrong!");
                return;
            }
        }

        private void TxtSeason_Validating(object sender, CancelEventArgs e)
        {
            DataTable seasonData;
            string season = string.Empty;
            string sqlCmd = "select distinct ID from Season WITH (NOLOCK) where Junk = 0";
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out seasonData);
            foreach (DataRow dr in seasonData.Rows)
            {
                season = dr["ID"].ToString();
                if (this.txtSeason.Text == season)
                {
                    return;
                }
            }

            if (this.txtSeason.Text == string.Empty)
            {
                this.txtSeason.Text = string.Empty;
                return;
            }

            if (this.txtSeason.Text != season)
            {
                this.txtSeason.Text = string.Empty;
                MyUtility.Msg.WarningBox("This Season is wrong!");
                return;
            }
        }
    }
}
