using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.PPIC
{
    public partial class P04_Print : Sci.Win.Tems.PrintForm
    {
        private string style1, style2, brand, season, localMR;
        DataTable printData,subprocessColumnName,styleArtworkData;
        public P04_Print()
        {
            InitializeComponent();
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            style1 = txtstyle1.Text;
            style2 = txtstyle2.Text;
            brand = txtbrand1.Text;
            season = txtseason1.Text;
            localMR = txtuser1.TextBox1.Text;

            return base.ValidateInput();
        }

        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            string sqlCmd = string.Format(@"select s.ID,s.SeasonID,s.BrandID,s.ProgramID,s.Model,s.Description,s.StyleName,
s.CdCodeID,s.SizePage,s.GMTLT,s.CareCode,s.SizeUnit,s.CPU,s.Gender,s.CTNQty,
IIF(s.Phase = '1','Sample',IIF(s.Phase = '2','Bulk','')) as Phase,
isnull((select ID + '-' + Name from TPEPass1 where ID = IIF(s.Phase = '1',s.SampleSMR,IIF(s.Phase = '2',s.BulkSMR,''))),'') as SMR,
isnull((select ID + '-' + Name from TPEPass1 where ID = IIF(s.Phase = '1',s.SampleMRHandle,IIF(s.Phase = '2',s.BulkMRHandle,''))),'') as Handle,
isnull((select ID + '-' + Name from Pass1 where ID = s.LocalMR),'') as LocalMR,s.Processes,s.UKey
from Style s
where 1 = 1 {0} {1} {2} {3} {4}", MyUtility.Check.Empty(style1) ? "" : "and s.ID >= '" + style1 + "'", MyUtility.Check.Empty(style2) ? "" : "and s.ID <= '" + style2 + "'"
                            , MyUtility.Check.Empty(brand) ? "" : "and s.BrandID = '" + brand + "'", MyUtility.Check.Empty(season) ? "" : "and s.SeasonID = '" + season + "'", MyUtility.Check.Empty(localMR) ? "" : "and s.LocalMR = '" + localMR + "'");
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            //組Subprocess欄位名稱
            sqlCmd = @"select *,(ROW_NUMBER() OVER (ORDER BY a.Seq, a.ColumnSeq))+20 as rno from (
SELECT ID,Seq,ArtworkUnit,SystemType,Seq+'U' as FakeID,RTRIM(ID)+' ('+ArtworkUnit+')' as ColumnN, '1' as ColumnSeq FROM ArtworkType
WHERE ArtworkUnit <> ''
union all
SELECT ID,Seq,ArtworkUnit,SystemType,Seq+'TMS' as FakeID,RTRIM(ID)+' (TMS)' as ColumnN, '2' as ColumnSeq FROM ArtworkType
WHERE IsTMS = 1
union all
SELECT ID,Seq,ArtworkUnit,SystemType,Seq+'Pri' as FakeID,RTRIM(ID)+' (Price)' as ColumnN, '3' as ColumnSeq FROM ArtworkType
WHERE IsPrice = 1) a";
            result = DBProxy.Current.Select(null, sqlCmd, out subprocessColumnName);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query artworktype fail\r\n" + result.ToString());
                return failResult;
            }
            subprocessColumnName.PrimaryKey = new DataColumn[] { subprocessColumnName.Columns["FakeID"] };

            //撈Style Subprocess資料
            sqlCmd = @"with ATData
as
(
select *,(ROW_NUMBER() OVER (ORDER BY a.Seq, a.ColumnSeq))+20 as rno from (
SELECT ID,Seq,ArtworkUnit,SystemType,Seq+'U' as FakeID,RTRIM(ID)+' ('+ArtworkUnit+')' as ColumnN, '1' as ColumnSeq FROM ArtworkType
WHERE ArtworkUnit <> ''
union all
SELECT ID,Seq,ArtworkUnit,SystemType,Seq+'TMS' as FakeID,RTRIM(ID)+' (TMS)' as ColumnN, '2' as ColumnSeq FROM ArtworkType
WHERE IsTMS = 1
union all
SELECT ID,Seq,ArtworkUnit,SystemType,Seq+'Pri' as FakeID,RTRIM(ID)+' (Price)' as ColumnN, '3' as ColumnSeq FROM ArtworkType
WHERE IsPrice = 1) a
)
select st.StyleUkey,st.Qty,st.TMS,st.Price,a.rno as UnitRno, a1.rno as TMSRno, a2.rno as PriRno
from Style_TmsCost st
left join ATData a on a.FakeID = st.Seq+'U' 
left join ATData a1 on a1.FakeID = st.Seq+'TMS'
left join ATData a2 on a2.FakeID = st.Seq+'Pri'";
            result = DBProxy.Current.Select(null, sqlCmd, out styleArtworkData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query style tms & cost fail\r\n" + result.ToString());
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

            string strXltName = Sci.Env.Cfg.XltPathDir + "PPIC_P04_Print.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null) return false;
            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];
            //填Subprocess欄位名稱
            int lastCol=1;
            foreach(DataRow dr in subprocessColumnName.Rows)
            {
                worksheet.Cells[2, MyUtility.Convert.GetInt(dr["rno"])] = MyUtility.Convert.GetString(dr["ColumnN"]);
                lastCol = MyUtility.Convert.GetInt(dr["rno"]);
            }

            //算出Excel的Column的英文位置
            string excelColEng;
            if (lastCol <= 26)
            {
                excelColEng = MyUtility.Convert.GetString(Convert.ToChar(lastCol + 64));
            }
            else
            {
                if ((lastCol + 64) % 26 == 0)
                {
                    excelColEng = MyUtility.Convert.GetString(Convert.ToChar((int)((lastCol - 1) / 26) + 64)) + 'Z';
                }
                else
                {
                    excelColEng = MyUtility.Convert.GetString(Convert.ToChar((int)((lastCol) / 26) + 64)) + MyUtility.Convert.GetString(Convert.ToChar(lastCol - ((int)(lastCol / 26) * 26) + 64));
                }
            }

            //合併儲存格,文字置中
            worksheet.Range[String.Format("A1:{0}1", excelColEng)].Merge(Type.Missing);
            worksheet.Range[String.Format("A1:{0}1", excelColEng)].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

            //填內容值
            int intRowsStart = 3;
            object[,] objArray = new object[1, 64];
            foreach (DataRow dr in printData.Rows)
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
                //先清空Subprocess值
                for (int i = 20; i < lastCol; i++)
                {
                    objArray[0, i] = "";
                }
                
                DataRow[] finRow = styleArtworkData.Select(string.Format("StyleUkey = {0}", MyUtility.Convert.GetString(dr["UKey"])));
                if (finRow.Length > 0)
                {
                    foreach (DataRow sdr in finRow)
                    {
                        if (!MyUtility.Check.Empty(sdr["UnitRno"]))
                        {
                            if (!MyUtility.Check.Empty(sdr["Qty"]))
                            {
                                objArray[0, MyUtility.Convert.GetInt(sdr["UnitRno"])-1] = sdr["Qty"];
                            }
                        }
                        if (!MyUtility.Check.Empty(sdr["TMSRno"]))
                        {
                            if (!MyUtility.Check.Empty(sdr["TMS"]))
                            {
                                objArray[0, MyUtility.Convert.GetInt(sdr["TMSRno"])-1] = sdr["TMS"];
                            }
                        }

                        if (!MyUtility.Check.Empty(sdr["PriRno"]))
                        {
                            if (!MyUtility.Check.Empty(sdr["Price"]))
                            {
                                objArray[0, MyUtility.Convert.GetInt(sdr["PriRno"])-1] = sdr["Price"];
                            }
                        }
                    }
                }

                worksheet.Range[String.Format("A{0}:{1}{0}", intRowsStart, excelColEng)].Value2 = objArray;
                intRowsStart++;
            }
            excel.Cells.EntireRow.AutoFit();
            excel.Cells.EntireColumn.AutoFit();

            excel.Visible = true;
            return true;
        }
    }
}
