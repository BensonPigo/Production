using System;
using System.Data;
using System.Text;
using Ict;
using Sci.Data;
using System.Runtime.InteropServices;

namespace Sci.Production.IE
{
    /// <summary>
    /// IE_P01_Print
    /// </summary>
    public partial class P01_Print : Win.Tems.PrintForm
    {
        private DataRow masterData;
        private string artworktype;
        private string strLanguage;
        private string custcdID;
        private string machineID;
        private decimal efficiency;
        private DataTable printData;
        private DataTable artworkType;

        /// <summary>
        /// P01_Print
        /// </summary>
        /// <param name="masterData">masterData</param>
        public P01_Print(DataRow masterData)
        {
            this.InitializeComponent();
            this.EditMode = true;
            this.masterData = masterData;
            DataTable artworkType;
            string sqlCmd = string.Format(
                @"
Select distinct ATD.ArtworkTypeID 
from MachineType MT WITH (NOLOCK) , TimeStudy_Detail WITH (NOLOCK) ,Artworktype_Detail ATD WITH (NOLOCK)
where MT.ID = TimeStudy_Detail.MachineTypeID 
and MT.ID=ATD.MachineTypeID
and TimeStudy_Detail.ID = {0}",
                MyUtility.Convert.GetString(this.masterData["ID"]));
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out artworkType);
            MyUtility.Tool.SetupCombox(this.comboArtworkType, 1, artworkType);
            this.comboArtworkType.SelectedIndex = -1;

            MyUtility.Tool.SetupCombox(this.comboLanguage, 2, 1, "en,English,cn,Chinese,vn,Vietnam,kh,Cambodia");
            this.comboLanguage.SelectedIndex = 0;
        }

        /// <summary>
        /// 驗證輸入條件
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(this.numEfficiencySetting.Value))
            {
                MyUtility.Msg.WarningBox("Efficiency setting can't empty!!");
                return false;
            }

            this.efficiency = MyUtility.Convert.GetInt(this.numEfficiencySetting.Value);
            this.artworktype = this.comboArtworkType.Text;
            this.strLanguage = this.comboLanguage.SelectedValue.ToString();

            return base.ValidateInput();
        }

        /// <summary>
        /// 非同步取資料
        /// </summary>
        /// <param name="e">e</param>
        /// <returns>DualResult</returns>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)

        {
            this.custcdID = MyUtility.GetValue.Lookup(string.Format("select CdCodeID from Style WITH (NOLOCK) where ID = '{0}' and SeasonID = '{1}' and BrandID = '{2}'", MyUtility.Convert.GetString(this.masterData["StyleID"]), MyUtility.Convert.GetString(this.masterData["SeasonID"]), MyUtility.Convert.GetString(this.masterData["BrandID"])));
            this.machineID = MyUtility.GetValue.Lookup(string.Format(
                @"select CONCAT(b.Machine,', ') from (
select MachineTypeID+'*'+CONVERT(varchar,cnt) as Machine from (
select td.MachineTypeID,COUNT(td.MachineTypeID) as cnt from TimeStudy_Detail td WITH (NOLOCK) left join MachineType m WITH (NOLOCK) on td.MachineTypeID = m.ID where td.ID = {0} and td.MachineTypeID <> ''{1} group by MachineTypeID) a) b
FOR XML PATH('')", MyUtility.Convert.GetString(this.masterData["ID"]),
                MyUtility.Check.Empty(this.artworktype) ? string.Empty : " and m.ArtworkTypeID = '" + this.artworktype + "'"));

            string ietmsUKEY = MyUtility.GetValue.Lookup($@"select i.Ukey from IETMS i WITH (NOLOCK) where  i.ID = '{this.masterData["IETMSID"]}' and i.Version='{this.masterData["IETMSversion"]}'");

            string sqlCmd = string.Empty;
            sqlCmd = $@"
select 
    seq = '0',
	OperationID = '--CUTTING',	
	MachineTypeID = null,
	Mold = null,
	Frequency = round(ProTMS, 4),
	SMV = round(ProTMS, 4),	
	PcsPerHour =IIF(ProTMS=0
                    ,0
                    ,round(3600/ProTMS, 1)
                ),
	Sewer=0,
	Annotation = null,	
	DescEN = null
	,[MasterPlusGroup]=''
    ,[Template] = ''
from[IETMS_Summary] where location = '' and[IETMSUkey] = '{ietmsUKEY}' and ArtworkTypeID = 'Cutting'
union all
select 
    seq = '0',
	OperationID = 'PROCIPF00001',	
	MachineTypeID = 'CUT',
	Mold = null,
	Frequency = round(ProTMS, 4),
	SMV = round(ProTMS, 4),	
	PcsPerHour =IIF(ProTMS=0
                    ,0
                    ,round(3600/ProTMS, 1)
                ),
	Sewer=0,
	Annotation = 	null,
	DescEN = '**Cutting'
	,[MasterPlusGroup]=''
    ,[Template] = ''
from[IETMS_Summary] where location = '' and[IETMSUkey] = '{ietmsUKEY}' and ArtworkTypeID = 'Cutting'
union all
";
            sqlCmd += string.Format(
                @"
select td.Seq
    , td.OperationID
    , td.MachineTypeID
    , td.Mold
    , td.Frequency
    , td.SMV
    , td.PcsPerHour
    , td.Sewer
    , td.Annotation
    , [DescEN] = case when '{3}' = 'cn' then isnull(od.DescCHS,o.DescEN)
                   when '{3}' = 'vn' then isnull(od.DescVI,o.DescEN)
                   when '{3}' = 'kh' then isnull(od.DescKH,o.DescEN)
     else o.DescEN end
    , o.MasterPlusGroup
    , td.Template
from TimeStudy_Detail td WITH (NOLOCK) 
left join Operation o WITH (NOLOCK) on td.OperationID = o.ID
left join OperationDesc od on o.ID = od.ID
{0}
where td.ID = {1}{2}
",
                MyUtility.Check.Empty(this.artworktype) ? string.Empty : "LEFT JOIN MachineType m WITH (NOLOCK) on td.MachineTypeID = m.ID " + Environment.NewLine + "LEFT JOIN Artworktype_Detail ATD WITH(NOLOCK) ON m.ID = ATD.MachineTypeID",
                MyUtility.Convert.GetString(this.masterData["ID"]),
                MyUtility.Check.Empty(this.artworktype) ? string.Empty : string.Format(" and ATD.ArtworkTypeID = '{0}'", this.artworktype),
                this.strLanguage);
            sqlCmd += $@"
union all
select 
    seq = '9960',
	OperationID = '--IPF',	
	MachineTypeID = null,
	Mold = null,
	Frequency = sum(round(ProTMS, 4)),
	SMV = sum(round(ProTMS, 4)),	
	PcsPerHour = sum(
	                IIF(ProTMS=0
	                ,0
	                ,round(3600/ProTMS, 1))	
	            ),
	Sewer=0,
	Annotation = null,	
	DescEN = null
	,[MasterPlusGroup]=''
    ,[Template] = ''
from [IETMS_Summary] where location = '' and [IETMSUkey] = '{ietmsUKEY}' and ArtworkTypeID <> 'Cutting'
union all
select
    seq = '9970',
	OperationID = 'PROCIPF00002',	
	MachineTypeID = 'M',
	Mold = null,
	Frequency = round(ProTMS, 4),
	SMV = round(ProTMS, 4),	
	PcsPerHour = IIF(ProTMS=0
                    ,0
                    ,round(3600/ProTMS, 1)
                ),
	Sewer=0,
	Annotation = null,
	DescEN = '**Inspection'
	,[MasterPlusGroup]=''
    ,[Template] = ''
from [IETMS_Summary] where location = '' and [IETMSUkey] = '{ietmsUKEY}' and ArtworkTypeID = 'Inspection'
union all
select 
    seq = '9980',
	OperationID = 'PROCIPF00004',	
	MachineTypeID = 'MM2',
	Mold = null,
	Frequency = round(ProTMS, 4),
	SMV = round(ProTMS, 4),	
	PcsPerHour = IIF(ProTMS=0
                    ,0
                    ,round(3600/ProTMS, 1)
                ),
	Sewer=0,
	Annotation = null,
	DescEN = '**Pressing'
	,[MasterPlusGroup]=''
    ,[Template] = ''
from [IETMS_Summary] where location = '' and [IETMSUkey] = '{ietmsUKEY}' and ArtworkTypeID = 'Pressing'
union all
select 	
    seq = '9990',
	OperationID = 'PROCIPF00003',	
	MachineTypeID = 'MM2',
	Mold = null,
	Frequency = round(ProTMS, 4),
	SMV = round(ProTMS, 4),	
	PcsPerHour = IIF(ProTMS=0
                    ,0
                    ,round(3600/ProTMS, 1)
                ),
	Sewer=0,
	Annotation = null,
	DescEN =  '**Packing'
	,[MasterPlusGroup]=''
    ,[Template] = ''
from [IETMS_Summary] where location = '' and [IETMSUkey] = '{ietmsUKEY}' and ArtworkTypeID = 'Packing'
order by seq
";
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out this.printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            sqlCmd = string.Format(
                @"select isnull(m.ArtworkTypeID,'') as ArtworkTypeID,sum(td.SMV) as ttlSMV
from TimeStudy_Detail td WITH (NOLOCK) 
left join MachineType m WITH (NOLOCK) on td.MachineTypeID = m.ID
LEFT JOIN Artworktype_Detail ATD WITH (NOLOCK) ON m.ID=ATD.MachineTypeID
where td.ID = {0}{1}
group by isnull(m.ArtworkTypeID,'')", MyUtility.Convert.GetString(this.masterData["ID"]),
                MyUtility.Check.Empty(this.artworktype) ? string.Empty : string.Format(" and ATD.ArtworkTypeID = '{0}'", this.artworktype));
            result = DBProxy.Current.Select(null, sqlCmd, out this.artworkType);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return Ict.Result.True;
        }
        /// <summary>
        /// 產生Excel
        /// </summary>
        /// <param name="report">report</param>
        /// <returns>bool</returns>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            string chkdata = string.Empty;
            if (this.chkCutting.Checked || this.chkInspection.Checked || this.chkPacking.Checked || this.chkPressing.Checked)
            {
                if (this.chkCutting.Checked)
                {
                    if (this.printData.Select("OperationID = 'PROCIPF00001'").Length == 0)
                    {
                        chkdata += "Cutting,";
                    }
                }

                if (this.chkInspection.Checked)
                {
                    if (this.printData.Select("OperationID = 'PROCIPF00002'").Length == 0)
                    {
                        chkdata += "Inspection,";
                    }
                }

                if (this.chkPacking.Checked)
                {
                    if (this.printData.Select("OperationID = 'PROCIPF00003'").Length == 0)
                    {
                        chkdata += "Packing,";
                    }
                }

                if (this.chkPressing.Checked)
                {
                    if (this.printData.Select("OperationID = 'PROCIPF00004'").Length == 0)
                    {
                        chkdata += "Pressing,";
                    }
                }
            }

            if (chkdata.Length > 0)
            {
                MyUtility.Msg.WarningBox($"CIPF no have {chkdata} data");
                return false;
            }

            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.printData.Rows.Count);

            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            string strXltName = Env.Cfg.XltPathDir + "\\IE_P01_Print.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return false;
            }

            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];
            worksheet.Cells[1, 1] = string.Format("Factory GSD - {0}", MyUtility.Convert.GetString(this.masterData["Phase"]));
            worksheet.Cells[3, 2] = MyUtility.Convert.GetString(this.masterData["StyleID"]);
            worksheet.Cells[3, 5] = MyUtility.Convert.GetString(this.masterData["SeasonID"]);
            worksheet.Cells[3, 7] = this.custcdID;
            worksheet.Cells[3, 12] = Convert.ToDateTime(DateTime.Today).ToString("d");
            worksheet.Cells[4, 14] = MyUtility.Convert.GetString(this.efficiency) + "%";
            worksheet.Columns[3].ColumnWidth = 18.4;

            // 填內容值
            int intRowsStart = 5;
            object[,] objArray = new object[1, 14];
            foreach (DataRow dr in this.printData.Rows)
            {
                objArray[0, 0] = intRowsStart - 4;
                objArray[0, 1] = dr["Seq"];
                objArray[0, 2] = dr["OperationID"];
                objArray[0, 3] = dr["MachineTypeID"];
                objArray[0, 4] = dr["MasterPlusGroup"];
                objArray[0, 5] = dr["Mold"];
                objArray[0, 6] = dr["Template"];
                objArray[0, 7] = dr["DescEN"];
                objArray[0, 8] = dr["Annotation"];
                objArray[0, 9] = dr["Frequency"];
                objArray[0, 10] = dr["SMV"];
                objArray[0, 11] = dr["PcsPerHour"];
                objArray[0, 12] = dr["Sewer"];
                objArray[0, 13] = MyUtility.Math.Round(MyUtility.Convert.GetDecimal(dr["PcsPerHour"]) * (this.efficiency / 100), 1);

                worksheet.Range[string.Format("A{0}:N{0}", intRowsStart)].Value2 = objArray;
                intRowsStart++;
            }

            intRowsStart++;
            worksheet.Range[string.Format("A{0}:B{0}", intRowsStart)].Merge(Type.Missing);
            worksheet.Range[string.Format("A{0}:B{0}", intRowsStart)].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
            worksheet.Cells[intRowsStart, 1] = "Machine:";
            worksheet.Range[string.Format("C{0}:N{0}", intRowsStart)].Merge(Type.Missing);
            worksheet.Range[string.Format("C{0}:N{0}", intRowsStart)].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;

            // 避免machineID為空所產生的錯誤
            if (this.machineID.Length > 2)
            {
                worksheet.Cells[intRowsStart, 3] = this.machineID.Substring(0, this.machineID.Length - 2);
            }

            worksheet.Range[string.Format("A{0}:N{0}", intRowsStart)].Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom).Weight = 3; // 1: 虛線, 2:實線, 3:粗體線
            worksheet.Range[string.Format("A{0}:N{0}", intRowsStart)].Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom).LineStyle = 1;

            intRowsStart++;
            worksheet.Range[string.Format("A{0}:C{0}", intRowsStart)].Merge(Type.Missing);
            worksheet.Range[string.Format("A{0}:C{0}", intRowsStart)].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
            worksheet.Cells[intRowsStart, 1] = "Total Sewing Time/Pc:";
            worksheet.Cells[intRowsStart, 4] = MyUtility.Convert.GetString(this.masterData["TotalSewingTime"]);
            worksheet.Cells[intRowsStart, 5] = "Sec.";
            worksheet.Cells[intRowsStart, 7] = "Prepared by:";
            worksheet.Range[string.Format("H{0}:N{0}", intRowsStart)].Merge(Type.Missing);
            string cipfrow = string.Empty;
            if (this.chkCutting.Checked || this.chkInspection.Checked || this.chkPacking.Checked || this.chkPressing.Checked)
            {
                intRowsStart++;
                worksheet.Range[string.Format("A{0}:C{0}", intRowsStart)].Merge(Type.Missing);
                worksheet.Range[string.Format("A{0}:C{0}", intRowsStart)].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                string chk = string.Empty;
                decimal ttl = 0;
                if (this.chkCutting.Checked)
                {
                    if (this.printData.Select("OperationID = 'PROCIPF00001'").Length > 0)
                    {
                        chk += "Cutting,";
                        ttl += MyUtility.Convert.GetDecimal(this.printData.Select("OperationID = 'PROCIPF00001'")[0]["SMV"]);
                    }
                }

                if (this.chkInspection.Checked)
                {
                    if (this.printData.Select("OperationID = 'PROCIPF00002'").Length > 0)
                    {
                        chk += "Inspection,";
                        ttl += MyUtility.Convert.GetDecimal(this.printData.Select("OperationID = 'PROCIPF00002'")[0]["SMV"]);
                    }
                }

                if (this.chkPacking.Checked)
                {
                    if (this.printData.Select("OperationID = 'PROCIPF00003'").Length > 0)
                    {
                        chk += "Packing,";
                        ttl += MyUtility.Convert.GetDecimal(this.printData.Select("OperationID = 'PROCIPF00003'")[0]["SMV"]);
                    }
                }

                if (this.chkPressing.Checked)
                {
                    if (this.printData.Select("OperationID = 'PROCIPF00004'").Length > 0)
                    {
                        chk += "Pressing";
                        ttl += MyUtility.Convert.GetDecimal(this.printData.Select("OperationID = 'PROCIPF00004'")[0]["SMV"]);
                    }
                }

                worksheet.Cells[intRowsStart, 1] = $"Total Time/Pc(Include {chk}):";
                worksheet.Cells[intRowsStart, 4] = MyUtility.Convert.GetString(MyUtility.Convert.GetDecimal(this.masterData["TotalSewingTime"]) + ttl);
                worksheet.Cells[intRowsStart, 5] = "Sec.";
                worksheet.Range[string.Format("H{0}:N{0}", intRowsStart)].Merge(Type.Missing);
                cipfrow = $"A{intRowsStart}";
            }

            intRowsStart++;
            worksheet.Range[string.Format("A{0}:G{0}", intRowsStart)].Merge(Type.Missing);
            worksheet.Range[string.Format("A{0}:G{0}", intRowsStart)].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
            StringBuilder artwork = new StringBuilder();
            foreach (DataRow dr in this.artworkType.Rows)
            {
                artwork.Append(string.Format("{0}: {1} Sec\r\n", Convert.ToString(dr["ArtworkTypeID"]).PadRight(20, ' '), MyUtility.Convert.GetString(MyUtility.Math.Round(MyUtility.Convert.GetDecimal(dr["ttlSMV"]), 0))));
            }

            worksheet.Cells[intRowsStart, 1] = artwork.ToString();

            intRowsStart++;
            worksheet.Range[string.Format("A{0}:C{0}", intRowsStart)].Merge(Type.Missing);
            worksheet.Range[string.Format("A{0}:C{0}", intRowsStart)].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
            worksheet.Cells[intRowsStart, 1] = "Number of Sewer:";
            worksheet.Cells[intRowsStart, 4] = MyUtility.Convert.GetString(this.masterData["NumberSewer"]);
            worksheet.Cells[intRowsStart, 5] = "Sewer";
            worksheet.Cells[intRowsStart, 7] = "Approved by:";
            worksheet.Range[string.Format("H{0}:N{0}", intRowsStart)].Merge(Type.Missing);

            intRowsStart++;
            worksheet.Range[string.Format("A{0}:C{0}", intRowsStart)].Merge(Type.Missing);
            worksheet.Range[string.Format("A{0}:C{0}", intRowsStart)].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
            worksheet.Cells[intRowsStart, 1] = "100% Output/Hr:";

            // 避免分母為0的錯誤
            if (MyUtility.Convert.GetDecimal(this.masterData["TotalSewingTime"]) > 0 && MyUtility.Convert.GetDecimal(this.masterData["NumberSewer"]) > 0)
            {
                worksheet.Cells[intRowsStart, 4] = MyUtility.Math.Round(3600 / MyUtility.Convert.GetDecimal(this.masterData["TotalSewingTime"]) * MyUtility.Convert.GetDecimal(this.masterData["NumberSewer"]), 0);
            }

            worksheet.Cells[intRowsStart, 5] = "Pcs";

            intRowsStart++;
            worksheet.Range[string.Format("A{0}:C{0}", intRowsStart)].Merge(Type.Missing);
            worksheet.Range[string.Format("A{0}:C{0}", intRowsStart)].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
            worksheet.Cells[intRowsStart, 1] = string.Format("{0}%", MyUtility.Convert.GetString(this.efficiency));

            // 避免分母為0的錯誤
            if (MyUtility.Convert.GetDecimal(this.masterData["TotalSewingTime"]) > 0 && MyUtility.Convert.GetDecimal(this.masterData["NumberSewer"]) > 0)
            {
                worksheet.Cells[intRowsStart, 4] = MyUtility.Math.Round(3600 / MyUtility.Convert.GetDecimal(this.masterData["TotalSewingTime"]) * MyUtility.Convert.GetDecimal(this.masterData["NumberSewer"]) * this.efficiency / 100, 0);
            }

            worksheet.Cells[intRowsStart, 5] = "Pcs";

            intRowsStart++;
            worksheet.Range[string.Format("A{0}:C{0}", intRowsStart)].Merge(Type.Missing);
            worksheet.Range[string.Format("A{0}:C{0}", intRowsStart)].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
            worksheet.Cells[intRowsStart, 1] = "PCS/HR/Sewer:";

            // 避免分母為0的錯誤
            if (MyUtility.Convert.GetDecimal(this.masterData["TotalSewingTime"]) > 0 && MyUtility.Convert.GetDecimal(this.masterData["NumberSewer"]) > 0)
            {
                worksheet.Cells[intRowsStart, 4] = MyUtility.Math.Round(3600 / MyUtility.Convert.GetDecimal(this.masterData["TotalSewingTime"]), 2);
            }

            worksheet.Cells[intRowsStart, 5] = "Pcs";
            worksheet.Cells[intRowsStart, 7] = "Noted by:";
            worksheet.Range[string.Format("H{0}:N{0}", intRowsStart)].Merge(Type.Missing);

            excel.Cells.EntireRow.AutoFit();
            if (!MyUtility.Check.Empty(cipfrow))
            {
                worksheet.get_Range(cipfrow).RowHeight = 33;
            }

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("IE_P01_Print");
            Microsoft.Office.Interop.Excel.Workbook workbook = excel.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(workbook);
            Marshal.ReleaseComObject(worksheet);

            strExcelName.OpenFile();
            #endregion
            return true;
        }
    }
}
