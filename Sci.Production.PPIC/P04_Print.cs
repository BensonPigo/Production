using System;
using System.Data;
using System.Drawing;
using Ict;
using Sci.Data;
using System.Runtime.InteropServices;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// P04_Print
    /// </summary>
    public partial class P04_Print : Win.Tems.PrintForm
    {
        private string style1;
        private string style2;
        private string brand;
        private string season;
        private string localMR;
        private DataTable printData;
        private DataTable subprocessColumnName;
        private DataTable styleArtworkData;

        /// <summary>
        /// P04_Print
        /// </summary>
        public P04_Print()
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.style1 = this.txtstyleStart.Text;
            this.style2 = this.txtstyleEnd.Text;
            this.brand = this.txtbrand.Text;
            this.season = this.txtseason.Text;
            this.localMR = this.txtuserLocalMR.TextBox1.Text;

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            string sqlCmd = string.Format(
                @"select s.ID,s.SeasonID,s.BrandID,s.ProgramID,s.Model,s.Description,s.StyleName,
s.CdCodeID,s.SizePage,s.GMTLT,s.CareCode,s.SizeUnit,s.CPU,s.Gender,s.CTNQty,
IIF(s.Phase = '1','Sample',IIF(s.Phase = '2','Bulk','')) as Phase,
isnull((select ID + '-' + Name from TPEPass1 WITH (NOLOCK) where ID = IIF(s.Phase = '1',s.SampleSMR,IIF(s.Phase = '2',s.BulkSMR,''))),'') as SMR,
isnull((select ID + '-' + Name from TPEPass1 WITH (NOLOCK) where ID = IIF(s.Phase = '1',s.SampleMRHandle,IIF(s.Phase = '2',s.BulkMRHandle,''))),'') as Handle,
isnull((select ID + '-' + Name from Pass1 WITH (NOLOCK) where ID = s.LocalMR),'') as LocalMR,s.Processes,s.UKey
from Style s WITH (NOLOCK) 
where 1 = 1 {0} {1} {2} {3} {4}",
                MyUtility.Check.Empty(this.style1) ? string.Empty : "and s.ID >= '" + this.style1 + "'",
                MyUtility.Check.Empty(this.style2) ? string.Empty : "and s.ID <= '" + this.style2 + "'",
                MyUtility.Check.Empty(this.brand) ? string.Empty : "and s.BrandID = '" + this.brand + "'",
                MyUtility.Check.Empty(this.season) ? string.Empty : "and s.SeasonID = '" + this.season + "'",
                MyUtility.Check.Empty(this.localMR) ? string.Empty : "and s.LocalMR = '" + this.localMR + "'");

            DualResult result = DBProxy.Current.Select(null, sqlCmd, out this.printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            // 組Subprocess欄位名稱
            sqlCmd = @"select *,(ROW_NUMBER() OVER (ORDER BY a.Seq, a.ColumnSeq))+20 as rno from (
SELECT ID,Seq,ArtworkUnit,SystemType,Seq+'U' as FakeID,RTRIM(ID)+' ('+ArtworkUnit+')' as ColumnN, '1' as ColumnSeq FROM ArtworkType WITH (NOLOCK) 
WHERE ArtworkUnit <> ''
union all
SELECT ID,Seq,ArtworkUnit,SystemType,Seq+'TMS' as FakeID,RTRIM(ID)+' (TMS)' as ColumnN, '2' as ColumnSeq FROM ArtworkType WITH (NOLOCK) 
WHERE IsTMS = 1
union all 
SELECT ID,Seq,ArtworkUnit,SystemType,Seq+'Pri' as FakeID,RTRIM(ID)+' (Price)' as ColumnN, '3' as ColumnSeq FROM ArtworkType WITH (NOLOCK) 
WHERE IsPrice = 1) a";
            result = DBProxy.Current.Select(null, sqlCmd, out this.subprocessColumnName);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query artworktype fail\r\n" + result.ToString());
                return failResult;
            }

            this.subprocessColumnName.PrimaryKey = new DataColumn[] { this.subprocessColumnName.Columns["FakeID"] };

            // 撈Style Subprocess資料
            sqlCmd = @"with ATData
as
(
select *,(ROW_NUMBER() OVER (ORDER BY a.Seq, a.ColumnSeq))+20 as rno from (
SELECT ID,Seq,ArtworkUnit,SystemType,Seq+'U' as FakeID,RTRIM(ID)+' ('+ArtworkUnit+')' as ColumnN, '1' as ColumnSeq FROM ArtworkType WITH (NOLOCK) 
WHERE ArtworkUnit <> ''
union all
SELECT ID,Seq,ArtworkUnit,SystemType,Seq+'TMS' as FakeID,RTRIM(ID)+' (TMS)' as ColumnN, '2' as ColumnSeq FROM ArtworkType WITH (NOLOCK) 
WHERE IsTMS = 1
union all
SELECT ID,Seq,ArtworkUnit,SystemType,Seq+'Pri' as FakeID,RTRIM(ID)+' (Price)' as ColumnN, '3' as ColumnSeq FROM ArtworkType WITH (NOLOCK) 
WHERE IsPrice = 1) a
)
select st.StyleUkey,st.Qty,st.TMS,st.Price,a.rno as UnitRno, a1.rno as TMSRno, a2.rno as PriRno
from Style_TmsCost st WITH (NOLOCK) 
left join ATData a on a.FakeID = st.Seq+'U' 
left join ATData a1 on a1.FakeID = st.Seq+'TMS'
left join ATData a2 on a2.FakeID = st.Seq+'Pri'";
            result = DBProxy.Current.Select(null, sqlCmd, out this.styleArtworkData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query style tms & cost fail\r\n" + result.ToString());
                return failResult;
            }

            return Result.True;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.printData.Rows.Count);

            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            string strXltName = Sci.Env.Cfg.XltPathDir + "\\PPIC_P04_Print.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return false;
            }

            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            // 填Subprocess欄位名稱
            int lastCol = 1;
            foreach (DataRow dr in this.subprocessColumnName.Rows)
            {
                worksheet.Cells[2, MyUtility.Convert.GetInt(dr["rno"])] = MyUtility.Convert.GetString(dr["ColumnN"]);
                lastCol = MyUtility.Convert.GetInt(dr["rno"]);
            }

            // 算出Excel的Column的英文位置
            string excelColEng = PublicPrg.Prgs.GetExcelEnglishColumnName(lastCol);

            // 合併儲存格,文字置中
            worksheet.Range[string.Format("A1:{0}1", excelColEng)].Merge(Type.Missing);
            worksheet.Range[string.Format("A1:{0}1", excelColEng)].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

            // 填內容值
            int intRowsStart = 3;
            object[,] objArray = new object[1, lastCol];
            foreach (DataRow dr in this.printData.Rows)
            {
                objArray[0, 0] = dr["ID"];
                objArray[0, 1] = dr["SeasonID"];
                objArray[0, 2] = dr["BrandID"];
                objArray[0, 3] = dr["ProgramID"];
                objArray[0, 4] = dr["Model"];
                objArray[0, 5] = dr["Description"];
                objArray[0, 6] = dr["StyleName"];
                objArray[0, 7] = dr["CdCodeID"];
                objArray[0, 8] = dr["SizePage"];
                objArray[0, 9] = dr["GMTLT"];
                objArray[0, 10] = dr["CareCode"];
                objArray[0, 11] = dr["SizeUnit"];
                objArray[0, 12] = dr["CPU"];
                objArray[0, 13] = dr["Gender"];
                objArray[0, 14] = dr["CTNQty"];
                objArray[0, 15] = dr["Phase"];
                objArray[0, 16] = dr["SMR"];
                objArray[0, 17] = dr["Handle"];
                objArray[0, 18] = dr["LocalMR"];
                objArray[0, 19] = dr["Processes"];

                // 先清空Subprocess值
                for (int i = this.printData.Columns.Count; i < lastCol; i++)
                {
                    objArray[0, i] = string.Empty;
                }

                DataRow[] finRow = this.styleArtworkData.Select(string.Format("StyleUkey = {0}", MyUtility.Convert.GetString(dr["UKey"])));
                if (finRow.Length > 0)
                {
                    foreach (DataRow sdr in finRow)
                    {
                        if (!MyUtility.Check.Empty(sdr["UnitRno"]))
                        {
                            if (!MyUtility.Check.Empty(sdr["Qty"]))
                            {
                                objArray[0, MyUtility.Convert.GetInt(sdr["UnitRno"]) - 1] = sdr["Qty"];
                            }
                        }

                        if (!MyUtility.Check.Empty(sdr["TMSRno"]))
                        {
                            if (!MyUtility.Check.Empty(sdr["TMS"]))
                            {
                                objArray[0, MyUtility.Convert.GetInt(sdr["TMSRno"]) - 1] = sdr["TMS"];
                            }
                        }

                        if (!MyUtility.Check.Empty(sdr["PriRno"]))
                        {
                            if (!MyUtility.Check.Empty(sdr["Price"]))
                            {
                                objArray[0, MyUtility.Convert.GetInt(sdr["PriRno"]) - 1] = sdr["Price"];
                            }
                        }
                    }
                }

                worksheet.Range[string.Format("A{0}:{1}{0}", 1, excelColEng)].Interior.Color = Color.FromArgb(204, 255, 204); // 第一列底色
                worksheet.Range[string.Format("A{0}:{1}{0}", 2, excelColEng)].Interior.Color = Color.FromArgb(204, 255, 204); // 第二列底色
                worksheet.Range[string.Format("A{0}:{1}{0}", 2, excelColEng)].AutoFilter(1); // 篩選
                worksheet.Range[string.Format("A{0}:{1}{0}", intRowsStart, excelColEng)].Value2 = objArray;

                intRowsStart++;
            }

            excel.Cells.EntireColumn.AutoFit();
            excel.Cells.EntireRow.AutoFit();

            #region Save & Show Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("PPIC_P04_Print");
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
