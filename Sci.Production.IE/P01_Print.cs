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
    public partial class P01_Print : Sci.Win.Tems.PrintForm
    {
        DataRow masterData;
        string artworktype, language,custcdID,machineID;
        decimal efficiency;
        DataTable printData, artworkType;
        public P01_Print(DataRow MasterData)
        {
            InitializeComponent();
            masterData = MasterData;
            label4.Text = "Language\r\n(For description)";
            DataTable artworkType;
            string sqlCmd = string.Format("Select distinct ArtworkTypeID from MachineType, TimeStudy_Detail where MachineType.ID = TimeStudy_Detail.MachineTypeID and TimeStudy_Detail.ID = {0}", MyUtility.Convert.GetString(masterData["ID"]));
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out artworkType);
            MyUtility.Tool.SetupCombox(comboBox1, 1,artworkType);
            MyUtility.Tool.SetupCombox(comboBox2, 1, 1, "English,Chinese,Cambodia,Vietnam");
            comboBox1.SelectedIndex = -1;
            comboBox2.Text = "English";
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(numericBox1.Value))
            {
                MyUtility.Msg.WarningBox("Efficiency setting can't empty!!");
                return false;
            }

            efficiency = MyUtility.Convert.GetInt(numericBox1.Value);
            artworktype = comboBox1.Text;
            language = comboBox2.Text;

            return base.ValidateInput();
        }

        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            custcdID = MyUtility.GetValue.Lookup(string.Format("select CdCodeID from Style where ID = '{0}' and SeasonID = '{1}' and BrandID = '{2}'", MyUtility.Convert.GetString(masterData["StyleID"]), MyUtility.Convert.GetString(masterData["SeasonID"]), MyUtility.Convert.GetString(masterData["BrandID"])));
            machineID = MyUtility.GetValue.Lookup(string.Format(@"select CONCAT(b.Machine,', ') from (
select MachineTypeID+'*'+CONVERT(varchar,cnt) as Machine from (
select td.MachineTypeID,COUNT(td.MachineTypeID) as cnt from TimeStudy_Detail td left join MachineType m on td.MachineTypeID = m.ID where td.ID = {0} and td.MachineTypeID <> ''{1} group by MachineTypeID) a) b
FOR XML PATH('')", MyUtility.Convert.GetString(masterData["ID"]), (MyUtility.Check.Empty(artworktype) ? "" : " and m.ArtworkTypeID = '"+artworktype+"'")));
            string sqlCmd = string.Format(@"select td.Seq,td.OperationID,td.MachineTypeID,td.Mold,td.Frequency,td.SMV,td.PcsPerHour,td.Sewer,
td.Annotation,o.DescEN,od.DescCHS,od.DescKH,od.DescVI 
from TimeStudy_Detail td
left join Operation o on td.OperationID = o.ID
left join OperationDesc od on o.ID = od.ID
left join MachineType m on td.MachineTypeID = m.ID
where td.ID = {0}{1}", MyUtility.Convert.GetString(masterData["ID"]), MyUtility.Check.Empty(artworktype) ? "" : string.Format(" and m.ArtworkTypeID = '{0}'", artworktype));
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            sqlCmd = string.Format(@"select isnull(m.ArtworkTypeID,'') as ArtworkTypeID,sum(td.SMV) as ttlSMV
from TimeStudy_Detail td
left join MachineType m on td.MachineTypeID = m.ID
where td.ID = {0}{1}
group by isnull(m.ArtworkTypeID,'')", MyUtility.Convert.GetString(masterData["ID"]), MyUtility.Check.Empty(artworktype) ? "" : string.Format(" and m.ArtworkTypeID = '{0}'", artworktype));
            result = DBProxy.Current.Select(null, sqlCmd, out artworkType);
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

            string strXltName = Sci.Env.Cfg.XltPathDir + "\\IE_P01_Print.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null) return false;
            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];
            worksheet.Cells[1, 1] = string.Format("Factory GSD - {0}", MyUtility.Convert.GetString(masterData["Phase"]));
            worksheet.Cells[3, 2] = MyUtility.Convert.GetString(masterData["StyleID"]);
            worksheet.Cells[3, 5] = MyUtility.Convert.GetString(masterData["SeasonID"]);
            worksheet.Cells[3, 7] = custcdID;
            worksheet.Cells[3, 11] = Convert.ToDateTime(DateTime.Today).ToString("d");
            worksheet.Cells[4, 12] = MyUtility.Convert.GetString(efficiency) + "%";
            
            //填內容值
            int intRowsStart = 5;
            object[,] objArray = new object[1, 12];
            foreach (DataRow dr in printData.Rows)
            {
                objArray[0, 0] = intRowsStart-4;
                objArray[0, 1] = dr["Seq"];
                objArray[0, 2] = dr["OperationID"];
                objArray[0, 3] = dr["MachineTypeID"];
                objArray[0, 4] = dr["Mold"];
                objArray[0, 5] = language == "English" ? dr["DescEN"] : language == "Chinese" ? dr["DescCHS"] : language == "Cambodia" ? dr["DescKH"] : language == "Vietnam" ? dr["DescVI"] : "";
                objArray[0, 6] = dr["Annotation"];
                objArray[0, 7] = dr["Frequency"];
                objArray[0, 8] = dr["SMV"];
                objArray[0, 9] = dr["PcsPerHour"];
                objArray[0, 10] = dr["Sewer"];
                objArray[0, 11] = MyUtility.Math.Round(MyUtility.Convert.GetDecimal(dr["PcsPerHour"]) * (efficiency / 100), 1);
                
                worksheet.Range[String.Format("A{0}:L{0}", intRowsStart)].Value2 = objArray;
                intRowsStart++;
            }
            intRowsStart++;
            worksheet.Range[String.Format("A{0}:B{0}", intRowsStart)].Merge(Type.Missing);
            worksheet.Range[String.Format("A{0}:B{0}", intRowsStart)].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
            worksheet.Cells[intRowsStart, 1] = "Machine:";
            worksheet.Range[String.Format("C{0}:L{0}", intRowsStart)].Merge(Type.Missing);
            worksheet.Range[String.Format("C{0}:L{0}", intRowsStart)].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;

            //避免machineID為空所產生的錯誤
            if (machineID.Length > 2) worksheet.Cells[intRowsStart, 3] = machineID.Substring(0, machineID.Length - 2);

            worksheet.Range[String.Format("A{0}:L{0}", intRowsStart)].Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom).Weight = 3; //1: 虛線, 2:實線, 3:粗體線
            worksheet.Range[String.Format("A{0}:L{0}", intRowsStart)].Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom).LineStyle = 1;
            intRowsStart++;
            worksheet.Range[String.Format("A{0}:C{0}", intRowsStart)].Merge(Type.Missing);
            worksheet.Range[String.Format("A{0}:C{0}", intRowsStart)].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
            worksheet.Cells[intRowsStart, 1] = "Total Sewing Time/Pc:";
            worksheet.Cells[intRowsStart, 4] = MyUtility.Convert.GetString(masterData["TotalSewingTime"]);
            worksheet.Cells[intRowsStart, 5] = "Sec.";
            worksheet.Cells[intRowsStart, 7] = "Prepared by:";
            worksheet.Range[String.Format("H{0}:L{0}", intRowsStart)].Merge(Type.Missing);

            intRowsStart++;
            worksheet.Range[String.Format("A{0}:F{0}", intRowsStart)].Merge(Type.Missing);
            worksheet.Range[String.Format("A{0}:F{0}", intRowsStart)].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
            StringBuilder artwork = new StringBuilder();
            foreach (DataRow dr in artworkType.Rows)
            {
                artwork.Append(string.Format("{0}: {1} Sec\r\n", Convert.ToString(dr["ArtworkTypeID"]).PadRight(20, ' '), MyUtility.Convert.GetString(MyUtility.Math.Round(MyUtility.Convert.GetDecimal(dr["ttlSMV"]), 0))));
            }
            worksheet.Cells[intRowsStart, 1] = artwork.ToString();

            intRowsStart++;
            worksheet.Range[String.Format("A{0}:C{0}", intRowsStart)].Merge(Type.Missing);
            worksheet.Range[String.Format("A{0}:C{0}", intRowsStart)].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
            worksheet.Cells[intRowsStart, 1] = "Number of Sewer:";
            worksheet.Cells[intRowsStart, 4] = MyUtility.Convert.GetString(masterData["NumberSewer"]);
            worksheet.Cells[intRowsStart, 5] = "Sewer";
            worksheet.Cells[intRowsStart, 7] = "Approved by:";
            worksheet.Range[String.Format("H{0}:L{0}", intRowsStart)].Merge(Type.Missing);

            intRowsStart++;
            worksheet.Range[String.Format("A{0}:C{0}", intRowsStart)].Merge(Type.Missing);
            worksheet.Range[String.Format("A{0}:C{0}", intRowsStart)].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
            worksheet.Cells[intRowsStart, 1] = "100% Output/Hr:";

            //避免分母為0的錯誤
            if (MyUtility.Convert.GetDecimal(masterData["TotalSewingTime"]) > 0 && MyUtility.Convert.GetDecimal(masterData["NumberSewer"]) > 0)
            {
                worksheet.Cells[intRowsStart, 4] = MyUtility.Math.Round(3600 / MyUtility.Convert.GetDecimal(masterData["TotalSewingTime"]) * MyUtility.Convert.GetDecimal(masterData["NumberSewer"]), 0);
            }
            
            worksheet.Cells[intRowsStart, 5] = "Pcs";

            intRowsStart++;
            worksheet.Range[String.Format("A{0}:C{0}", intRowsStart)].Merge(Type.Missing);
            worksheet.Range[String.Format("A{0}:C{0}", intRowsStart)].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
            worksheet.Cells[intRowsStart, 1] = string.Format("{0}%", MyUtility.Convert.GetString(efficiency));

            //避免分母為0的錯誤
            if (MyUtility.Convert.GetDecimal(masterData["TotalSewingTime"]) > 0 && MyUtility.Convert.GetDecimal(masterData["NumberSewer"]) > 0)
            {
                worksheet.Cells[intRowsStart, 4] = MyUtility.Math.Round(3600 / MyUtility.Convert.GetDecimal(masterData["TotalSewingTime"]) * MyUtility.Convert.GetDecimal(masterData["NumberSewer"]) * efficiency / 100, 0);
            }

            worksheet.Cells[intRowsStart, 5] = "Pcs";

            intRowsStart++;
            worksheet.Range[String.Format("A{0}:C{0}", intRowsStart)].Merge(Type.Missing);
            worksheet.Range[String.Format("A{0}:C{0}", intRowsStart)].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
            worksheet.Cells[intRowsStart, 1] = "PCS/HR/Sewer:";

            //避免分母為0的錯誤
            if (MyUtility.Convert.GetDecimal(masterData["TotalSewingTime"]) > 0 && MyUtility.Convert.GetDecimal(masterData["NumberSewer"]) > 0)
            {
                worksheet.Cells[intRowsStart, 4] = MyUtility.Math.Round(3600 / MyUtility.Convert.GetDecimal(masterData["TotalSewingTime"]), 2);
            }

            worksheet.Cells[intRowsStart, 5] = "Pcs";
            worksheet.Cells[intRowsStart, 7] = "Noted by:";
            worksheet.Range[String.Format("H{0}:L{0}", intRowsStart)].Merge(Type.Missing);
            
            excel.Cells.EntireRow.AutoFit();

            excel.Visible = true;
            return true;
        }
    }
}
