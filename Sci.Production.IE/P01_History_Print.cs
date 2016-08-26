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
    public partial class P01_History_Print : Sci.Win.Tems.PrintForm
    {
        DataTable printData, artworkType;
        DataRow masterData;
        string status, custCD, machineID;
        int ttlSewingTime, sewer, efficiency;
        public P01_History_Print(DataRow MasterData, string Status, string CustCD, int TtlSewingTime, int Sewer)
        {
            InitializeComponent();
            masterData = MasterData;
            status = Status;
            custCD = CustCD;
            ttlSewingTime = TtlSewingTime;
            sewer = Sewer;
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
           
            return base.ValidateInput();
        }

        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            string sqlCmd = string.Format(@"select t.ID,t.Phase,t.Version,td.Seq,td.OperationID,td.MachineTypeID,td.Mold,td.Frequency,
td.SMV,td.PcsPerHour,td.Sewer,td.Annotation,o.DescEN
from TimeStudyHistory t
inner join TimeStudyHistory_Detail td on t.ID = td.ID
left join Operation o on td.OperationID = o.ID
where t.StyleID = '{0}'
and t.SeasonID = '{1}'
and t.ComboType = '{2}'
and t.BrandID = '{3}'
and LEFT(t.Phase+REPLICATE(' ',10),10)+' VER-'+t.Version = '{4}'
order by td.Seq", MyUtility.Convert.GetString(masterData["StyleID"]), MyUtility.Convert.GetString(masterData["SeasonID"]), MyUtility.Convert.GetString(masterData["ComboType"]), MyUtility.Convert.GetString(masterData["BrandID"]), status);
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }
            if (printData.Rows.Count <= 0)
            {
                return Result.True;
            }

            string id = MyUtility.Convert.GetString(printData.Rows[0]["ID"]);
            sqlCmd = string.Format(@"select isnull(m.ArtworkTypeID,'') as ArtworkTypeID,sum(td.SMV) as ttlSMV
from TimeStudyHistory_Detail td
left join MachineType m on td.MachineTypeID = m.ID
where td.ID = {0}
group by isnull(m.ArtworkTypeID,'')", id);
            result = DBProxy.Current.Select(null, sqlCmd, out artworkType);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            machineID = MyUtility.GetValue.Lookup(string.Format(@"select CONCAT(b.Machine,', ') from (
select MachineTypeID+'*'+CONVERT(varchar,cnt) as Machine from (
select td.MachineTypeID,COUNT(td.MachineTypeID) as cnt from TimeStudyHistory_Detail td where td.ID = {0} and td.MachineTypeID <> '' group by MachineTypeID) a) b
FOR XML PATH('')", id));
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

            string strXltName = Sci.Env.Cfg.XltPathDir + "\\IE_P01_History_Print.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null) return false;
            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];
            worksheet.Cells[1, 1] = string.Format("Factory GSD - {0}, V={1}", MyUtility.Convert.GetString(printData.Rows[0]["Phase"]), MyUtility.Convert.GetString(printData.Rows[0]["Version"]));
            worksheet.Cells[3, 2] = MyUtility.Convert.GetString(masterData["StyleID"]);
            worksheet.Cells[3, 4] = MyUtility.Convert.GetString(masterData["SeasonID"]);
            worksheet.Cells[3, 6] = custCD;
            worksheet.Cells[3, 8] = Convert.ToDateTime(DateTime.Today).ToString("d");
            worksheet.Cells[4, 9] = MyUtility.Convert.GetString(efficiency) + "%";

            //填內容值
            int intRowsStart = 5;
            object[,] objArray = new object[1, 9];
            foreach (DataRow dr in printData.Rows)
            {
                objArray[0, 0] = dr["Seq"];
                objArray[0, 1] = dr["MachineTypeID"];
                objArray[0, 2] = dr["Mold"];
                objArray[0, 3] = dr["OperationID"];
                objArray[0, 4] = dr["DescEN"];
                objArray[0, 5] = dr["SMV"];
                objArray[0, 6] = dr["PcsPerHour"];
                objArray[0, 7] = dr["Sewer"];
                objArray[0, 8] = MyUtility.Math.Round(MyUtility.Convert.GetDecimal(dr["PcsPerHour"]) * (efficiency / 100), 1);

                worksheet.Range[String.Format("A{0}:I{0}", intRowsStart)].Value2 = objArray;
                intRowsStart++;
            }
            intRowsStart++;
            worksheet.Cells[intRowsStart, 1] = "Machine:";
            worksheet.Range[String.Format("B{0}:I{0}", intRowsStart)].Merge(Type.Missing);
            worksheet.Range[String.Format("B{0}:I{0}", intRowsStart)].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
            worksheet.Cells[intRowsStart, 2] = machineID.Length < 3 ? machineID.ToString() : machineID.Substring(0, machineID.Length - 2);
            worksheet.Range[String.Format("A{0}:I{0}", intRowsStart)].Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom).Weight = 3; //1: 虛線, 2:實線, 3:粗體線
            worksheet.Range[String.Format("A{0}:I{0}", intRowsStart)].Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom).LineStyle = 1;
            intRowsStart++;
            worksheet.Range[String.Format("A{0}:B{0}", intRowsStart)].Merge(Type.Missing);
            worksheet.Range[String.Format("A{0}:B{0}", intRowsStart)].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
            worksheet.Cells[intRowsStart, 1] = "Total Sewing Time/Pc:";
            worksheet.Cells[intRowsStart, 3] = MyUtility.Convert.GetString(ttlSewingTime);
            worksheet.Cells[intRowsStart, 4] = "Sec.";
            worksheet.Cells[intRowsStart, 5] = "Prepared by:";
            worksheet.Range[String.Format("E{0}:E{0}", intRowsStart)].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
            worksheet.Range[String.Format("F{0}:I{0}", intRowsStart)].Merge(Type.Missing);

            intRowsStart++;
            worksheet.Range[String.Format("A{0}:D{0}", intRowsStart)].Merge(Type.Missing);
            worksheet.Range[String.Format("A{0}:D{0}", intRowsStart)].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
            StringBuilder artwork = new StringBuilder();
            foreach (DataRow dr in artworkType.Rows)
            {
                artwork.Append(string.Format("{0}: {1} Sec\r\n", Convert.ToString(dr["ArtworkTypeID"]).PadRight(20, ' '), MyUtility.Convert.GetString(MyUtility.Math.Round(MyUtility.Convert.GetDecimal(dr["ttlSMV"]), 0))));
            }
            worksheet.Cells[intRowsStart, 1] = artwork.ToString();

            intRowsStart++;
            worksheet.Range[String.Format("A{0}:B{0}", intRowsStart)].Merge(Type.Missing);
            worksheet.Range[String.Format("A{0}:B{0}", intRowsStart)].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
            worksheet.Cells[intRowsStart, 1] = "Number of Sewer:";
            worksheet.Cells[intRowsStart, 3] = MyUtility.Convert.GetString(sewer);
            worksheet.Cells[intRowsStart, 4] = "Sewer";
            worksheet.Cells[intRowsStart, 5] = "Approved by:";
            worksheet.Range[String.Format("E{0}:E{0}", intRowsStart)].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
            worksheet.Range[String.Format("F{0}:I{0}", intRowsStart)].Merge(Type.Missing);

            intRowsStart++;
            worksheet.Range[String.Format("A{0}:B{0}", intRowsStart)].Merge(Type.Missing);
            worksheet.Range[String.Format("A{0}:B{0}", intRowsStart)].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
            worksheet.Cells[intRowsStart, 1] = "100% Output/Hr:";
            worksheet.Cells[intRowsStart, 3] = MyUtility.Math.Round(3600 / MyUtility.Convert.GetDecimal(ttlSewingTime) * MyUtility.Convert.GetDecimal(sewer), 0);
            worksheet.Cells[intRowsStart, 4] = "Pcs";

            intRowsStart++;
            worksheet.Range[String.Format("A{0}:B{0}", intRowsStart)].Merge(Type.Missing);
            worksheet.Range[String.Format("A{0}:B{0}", intRowsStart)].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
            worksheet.Cells[intRowsStart, 1] = string.Format("{0}%", MyUtility.Convert.GetString(efficiency));
            worksheet.Cells[intRowsStart, 3] = MyUtility.Math.Round(3600 / MyUtility.Convert.GetDecimal(ttlSewingTime) * MyUtility.Convert.GetDecimal(sewer) * efficiency / 100, 0);
            worksheet.Cells[intRowsStart, 4] = "Pcs";

            intRowsStart++;
            worksheet.Range[String.Format("A{0}:B{0}", intRowsStart)].Merge(Type.Missing);
            worksheet.Range[String.Format("A{0}:B{0}", intRowsStart)].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
            worksheet.Cells[intRowsStart, 1] = "PCS/HR/Sewer:";
            worksheet.Cells[intRowsStart, 3] = MyUtility.Math.Round(3600 / MyUtility.Convert.GetDecimal(ttlSewingTime), 2);
            worksheet.Cells[intRowsStart, 4] = "Pcs";
            worksheet.Cells[intRowsStart, 5] = "Noted by:";
            worksheet.Range[String.Format("E{0}:E{0}", intRowsStart)].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
            worksheet.Range[String.Format("F{0}:I{0}", intRowsStart)].Merge(Type.Missing);

            excel.Cells.EntireRow.AutoFit();

            excel.Visible = true;
            return true;
        }
    }
}
