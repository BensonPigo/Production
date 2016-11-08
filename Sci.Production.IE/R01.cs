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

namespace Sci.Production.IE
{
    public partial class R01 : Sci.Win.Tems.PrintForm
    {
        string factory, style, season, team;
        DataTable printData;
        public R01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            MyUtility.Tool.SetupCombox(comboBox1, 1, 1, ",A,B");
        }

        //Factory
        private void textBox1_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sqlCmd = "select distinct FTYGroup from Factory where Junk = 0 AND FTYGroup!=''";

            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlCmd, "8", textBox1.Text, "Factory");
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel) { return; }
            textBox1.Text = item.GetSelectedString();
        }

        //Style
        private void textBox2_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sqlCmd = "select distinct ID,BrandID,Description from Style where Junk = 0 order by ID";

            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlCmd, "15,8,50", textBox2.Text, "Style#,Brand,Description");
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel) { return; }
            textBox2.Text = item.GetSelectedString();
        }

        //Season
        private void textBox3_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sqlCmd = "select distinct ID from Season where Junk = 0";

            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlCmd, "10", textBox3.Text, "Season");
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel) { return; }
            textBox3.Text = item.GetSelectedString();
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            factory = textBox1.Text;
            style = textBox2.Text;
            season = textBox3.Text;
            team = comboBox1.SelectedIndex == -1 || comboBox1.SelectedIndex == 0 ? "" : comboBox1.SelectedIndex == 1 ? "A" : "B";

            return base.ValidateInput();
        }

        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(@"select lm.FactoryID,
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
	   IIF(lm.HighestCycle * lm.CurrentOperators = 0,0,(ROUND(3600.0/lm.HighestCycle, 2) * (select isnull(CPU,0) from Style where lm.BrandID = BrandID and lm.StyleID = ID and lm.SeasonID = SeasonID)/lm.CurrentOperators)) as PPH,
	   isnull((select Name from Pass1 where ID = lm.AddName),'') as CreateBy,lm.AddDate,
	   isnull((select Name from Pass1 where ID = lm.EditName),'') as EditBy,lm.EditDate
from LineMapping lm
where 1 = 1
");
            if (!MyUtility.Check.Empty(factory))
            {
                sqlCmd.Append(string.Format(" and lm.FactoryID = '{0}'",factory));
            }
            if (!MyUtility.Check.Empty(style))
            {
                sqlCmd.Append(string.Format(" and lm.StyleID = '{0}'",style));
            }
            if (!MyUtility.Check.Empty(season))
            {
                sqlCmd.Append(string.Format(" and lm.SeasonID = '{0}'",season));
            }
            if (!MyUtility.Check.Empty(team))
            {
                sqlCmd.Append(string.Format(" and lm.Team = '{0}'", team));
            }

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
            string strXltName = Sci.Env.Cfg.XltPathDir + "\\IE_R01_LineMappingList.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null) return false;
            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];
            //填內容值
            int intRowsStart = 2;
            object[,] objArray = new object[1, 22];
            foreach (DataRow dr in printData.Rows)
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

                worksheet.Range[String.Format("A{0}:V{0}", intRowsStart)].Value2 = objArray;
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
