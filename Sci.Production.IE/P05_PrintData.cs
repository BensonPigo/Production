using Ict;
using Sci.Data;
using Sci.Utility.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.IE
{
    /// <summary>
    /// P05_Report
    /// </summary>
    public class P05_PrintData
    {
        private enum OperationType
        {
            Sewing,
            PPA,
            NonSewing,
        }

        private string almID;
        private string display;
        private string content;
        private string language;

        private DataTable dtHeadInfo;
        private DataTable dtSewing;
        private DataTable dtPPA;
        private DataTable dtNonSewing;
        private DataTable[] dtLineMapping_MachineArea;
        private DataTable[] dtPPA_MachineArea;
        private DataTable dtChart;
        private DataTable dtPPASort;

        private int SewingRowTotalGSDTime = 0;
        private int PPARowTotalGSDTime = 0;

        /// <summary>
        /// P05_Report
        /// </summary>
        /// <param name="almID">AutomatedLineMapping.ID</param>
        /// <param name="display">導出方式</param>
        /// <param name="content">工段資訊要導出[Operation] or [Annotation]的模式</param>
        /// <param name="language">工段顯示語系</param>
        public P05_PrintData(string almID = "", string display = "", string content = "", string language = "")
        {
            this.almID = almID;
            this.display = display;
            this.content = content;
            this.language = language;
        }

        /// <summary>
        /// SetCondition
        /// </summary>
        /// <param name="almID">AutomatedLineMapping.ID</param>
        /// <param name="display">導出方式</param>
        /// <param name="content">工段資訊要導出[Operation] or [Annotation]的模式</param>
        /// <param name="language">工段顯示語系</param>
        public void SetCondition(string almID, string display, string content, string language)
        {
            this.almID = almID;
            this.display = display;
            this.content = content;
            this.language = language;
        }

        /// <summary>
        /// LoadData
        /// </summary>
        /// <returns>bool</returns>
        public bool LoadData()
        {
            var testVar = $@"
DECLARE @ALMID bigint = {this.almID}
DECLARE @Content varchar(1) = '{this.content}'
DECLARE @Language varchar(2) = '{this.language}'";

            var sql = $@"
/*{testVar}
*/

-- [Sewing Operation], [Centralized PPA Operation], [Non Sewing Line Operation] 表頭
;with PPA as (
	SELECT almd.GSD, almd.No
	FROM AutomatedLineMapping_Detail almd
	WHERE almd.ID = @ALMID
	and almd.PPA = 'C'
	and almd.IsNonSewingLine = 0
)
SELECT alm.StyleUKey
, Style = CONCAT(alm.StyleID, ' - ', alm.ComboType)
, Season = alm.SeasonID
, Brand = alm.BrandID
, Content = ddl.Name
, alm.FactoryID
, alm.StyleCPU
, alm.Version
, alm.WorkHour
, EOLR = ROUND(3600 / alm.HighestGSDTime, 2)
, LBRByGSDTime = ROUND(alm.TotalGSDTime / alm.HighestGSDTime / alm.SewerManpower, 4)
, alm.SewerManpower
, alm.TotalGSDTime
, AvgGSDTime = ROUND(alm.TotalGSDTime / alm.SewerManpower, 2)
, PPASewer = getPPA.Sewer
, TotalGSDTime_PPA = ISNULL(ROUND(getPPA.TotalGSDTime, 2), 0)
, AvgGSDTime_PPA = ISNULL(ROUND(getPPA.TotalGSDTime / getPPA.Sewer, 2), 0)
, ConfirmedBy = CONCAT(alm.CFMName, '-', cfm.Name)
, PrintBy = CONCAT(printBy.ID, '-', printBy.Name)
FROM AutomatedLineMapping alm
LEFT JOIN DropDownList ddl on ddl.Type = 'Pms_LMContent' and ddl.ID = @Content
LEFT JOIN Pass1 cfm on cfm.ID = alm.CFMName
LEFT JOIN Pass1 printBy on printBy.ID = @UserID
OUTER APPLY (
	SELECT TotalGSDTime = (Select SUM(GSD) from PPA)
	, Sewer = (Select count(distinct No) from PPA)
) getPPA
WHERE alm.ID = @ALMID

-- [Sewing Operation] 表身
SELECT almd.No
, almd.Seq
, almd.Location
, Operation = IIF(@Content = 'O',
				CASE @Language
					WHEN 'en' THEN IIF(o.DescEN = '', o.ID, o.DescEN)
					WHEN 'km' THEN IIF(o.DescKH = '', o.ID, o.DescKH)
					WHEN 'vi' THEN IIF(o.DescVN = '', o.ID, o.DescVN)
					WHEN 'zh' THEN IIF(o.DescCH = '', o.ID, o.DescCH)
					END,
				almd.Annotation)
, almd.MachineTypeID
, almd.MasterPlusGroup
, almd.Attachment
, almd.SewingMachineAttachmentID
, almd.Template
, almd.GSD
, almd.SewerDiffPercentage
, almd.ThreadComboID
, almd.OperationID
FROM AutomatedLineMapping_Detail almd
LEFT JOIN Operation o on almd.OperationID = o.ID
WHERE almd.ID = @ALMID
AND almd.PPA != 'C'
AND almd.IsNonSewingLine = 0
ORDER BY almd.Seq ASC

-- Excel [Centralized PPA Operation] 表身
SELECT almd.No
, almd.Seq
, almd.Location
, Operation = IIF(@Content = 'O',
				CASE @Language
					WHEN 'en' THEN IIF(o.DescEN = '', o.ID, o.DescEN)
					WHEN 'km' THEN IIF(o.DescKH = '', o.ID, o.DescKH)
					WHEN 'vi' THEN IIF(o.DescVN = '', o.ID, o.DescVN)
					WHEN 'zh' THEN IIF(o.DescCH = '', o.ID, o.DescCH)
					END,
				almd.Annotation)
, almd.MachineTypeID
, almd.MasterPlusGroup
, almd.Attachment
, almd.SewingMachineAttachmentID
, almd.Template
, almd.GSD
, almd.SewerDiffPercentage
, almd.ThreadComboID
, almd.OperationID
FROM AutomatedLineMapping_Detail almd
LEFT JOIN Operation o on almd.OperationID = o.ID
WHERE almd.ID = @ALMID
AND almd.PPA = 'C'
AND almd.IsNonSewingLine = 0
ORDER BY almd.Seq ASC

-- Excel [Centralized PPA Operation] 表身
SELECT almd.Seq
, almd.Location
, Operation = IIF(@Content = 'O',
				CASE @Language
					WHEN 'en' THEN IIF(o.DescEN = '', o.ID, o.DescEN)
					WHEN 'km' THEN IIF(o.DescKH = '', o.ID, o.DescKH)
					WHEN 'vi' THEN IIF(o.DescVN = '', o.ID, o.DescVN)
					WHEN 'zh' THEN IIF(o.DescCH = '', o.ID, o.DescCH)
					END,
				almd.Annotation)
, almd.MachineTypeID
, almd.MasterPlusGroup
, almd.Attachment
, almd.SewingMachineAttachmentID
, almd.Template
, almd.GSD
, almd.SewerDiffPercentage
, almd.ThreadComboID
, almd.OperationID
FROM AutomatedLineMapping_Detail almd
LEFT JOIN Operation o on almd.OperationID = o.ID
WHERE almd.ID = @ALMID
AND almd.IsNonSewingLine = 1
ORDER BY almd.Seq ASC

-- Excel [Line Mapping] Machine 區塊共用資料
select MachineTypeID, MasterPlusGroup, No, Attachment, SewingMachineAttachmentID, Template, concatString.Value
into #main
from AutomatedLineMapping_Detail
-- 將Attachment, SewingMachineAttachmentID, Template用逗號拆分後，重新組成字串
outer apply (
	select Value = isnull((
		select ltrim(rtrim(Data))
		from
		(
			select Data from dbo.SplitString(Attachment, ',')
			union all
			select Data from dbo.SplitString(SewingMachineAttachmentID, ',')
			union all
			select Data from dbo.SplitString(Template, ',')
		) tmp
		order by tmp.Data FOR XML PATH('')
	), '')
) concatString
where ID = @ALMID
and IsNonSewingLine = 0
and PPA != 'C'
and MachineTypeID not like 'MM%'

-- Excel [Line Mapping] Machine 表格
select item.MachineTypeID, item.MasterPlusGroup, getCount.Count
from (
	select MachineTypeID, MasterPlusGroup
	from #main
	group by MachineTypeID, MasterPlusGroup
) item
-- 計數 (No + Value相同視為1個)
outer apply (
	select [Count] = COUNT(distinct concat(main.No, main.Value))
	from #main main
	where main.MachineTypeID = item.MachineTypeID
	and main.MasterPlusGroup = item.MasterPlusGroup
) getCount
order by item.MachineTypeID, item.MasterPlusGroup

-- Excel [Line Mapping] Attachment Count / Template Count
select AttachmentCount = (
	select count(*) c from (
		select MachineTypeID, MasterPlusGroup, No, Value
		from #main
		where Attachment != ''
		group by MachineTypeID, MasterPlusGroup, No, Value
	) tmp)
, TemplateCount = (
	select count(*) c from (
		select MachineTypeID, MasterPlusGroup, No, Value
		from #main
		where Template != ''
		group by MachineTypeID, MasterPlusGroup, No, Value
	) tmp)

-- Excel [Line Mapping] Item 表格
Select * from
(
	select Item = Attachment, No, Detail = SewingMachineAttachmentID
	from #main where Attachment != ''
	union all
	select Item = Template, No, Detail = SewingMachineAttachmentID
	from #main where Template != ''
) ItemArea

drop table #main

-- Excel [Line Mapping] Line Balancing Graph圖表資料
select No
, ActGSDTime = Round(Sum(almd.GSD * almd.SewerDiffPercentage), 2)
, TaktTime = Round(3600 * alm.WorkHour / Round(Round(3600 * alm.SewerManpower / alm.TotalGSDTime, 0) * alm.WorkHour, 0), 2)
, ActGSDTime_Avg = Round(alm.TotalGSDTime / alm.SewerManpower, 2)
, ct = count(1)
from AutomatedLineMapping alm
left join AutomatedLineMapping_Detail almd on alm.ID = almd.ID
where alm.ID  = @ALMID
and almd.IsNonSewingLine = 0
and almd.PPA != 'C'
group by almd.No, alm.WorkHour, alm.SewerManpower, alm.TotalGSDTime

-- Excel [Centralized PPA] Machine區塊資料
select MachineTypeID, MasterPlusGroup, No, Attachment, SewingMachineAttachmentID, Template, concatString.Value
into #main_PPA
from AutomatedLineMapping_Detail
-- 將Attachment, SewingMachineAttachmentID, Template用逗號拆分後，重新組成字串
outer apply (
	select Value = isnull((
		select ltrim(rtrim(Data))
		from
		(
			select Data from dbo.SplitString(Attachment, ',')
			union all
			select Data from dbo.SplitString(SewingMachineAttachmentID, ',')
			union all
			select Data from dbo.SplitString(Template, ',')
		) tmp
		order by tmp.Data FOR XML PATH('')
	), '')
) concatString
where ID = @ALMID
and IsNonSewingLine = 0
and PPA = 'C'
and MachineTypeID not like 'MM%'

-- Excel [Centralized PPA] Machine 表格
select item.MachineTypeID, item.MasterPlusGroup, getCount.Count
from (
	select MachineTypeID, MasterPlusGroup
	from #main_PPA
	group by MachineTypeID, MasterPlusGroup
) item
-- 計數 (No + Value相同視為1個)
outer apply (
	select [Count] = COUNT(distinct concat(main.No, main.Value))
	from #main_PPA main
	where main.MachineTypeID = item.MachineTypeID
	and main.MasterPlusGroup = item.MasterPlusGroup
) getCount
order by item.MachineTypeID, item.MasterPlusGroup

-- Excel [Centralized PPA] Attachment Count / Template Count
select AttachmentCount = (
	select count(*) c from (
		select MachineTypeID, MasterPlusGroup, No, Value
		from #main_PPA
		where Attachment != ''
		group by MachineTypeID, MasterPlusGroup, No, Value
	) tmp)
, TemplateCount = (
	select count(*) c from (
		select MachineTypeID, MasterPlusGroup, No, Value
		from #main_PPA
		where Template != ''
		group by MachineTypeID, MasterPlusGroup, No, Value
	) tmp)

-- Excel [Centralized PPA] Item 表格
Select * from
(
	select Item = Attachment, No, Detail = SewingMachineAttachmentID
	from #main_PPA where Attachment != ''
	union all
	select Item = Template, No, Detail = SewingMachineAttachmentID
	from #main_PPA where Template != ''
) ItemArea

drop table #main_PPA

-- Excel [Centralized PPA] 排序資料
select No
, ct = count(1)
from AutomatedLineMapping alm
left join AutomatedLineMapping_Detail almd on alm.ID = almd.ID
where alm.ID  = @ALMID
and almd.IsNonSewingLine = 0
and almd.PPA = 'C'
group by almd.No
";

            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@ALMID", this.almID),
                new SqlParameter("@Content", this.content),
                new SqlParameter("@Language", this.language),
                new SqlParameter("@UserID", Env.User.UserID),
            };

            DataTable[] dtResults;
            DualResult result = DBProxy.Current.Select(null, sql, paras, out dtResults);

            if (!result)
            {
                MyUtility.Msg.WarningBox("Sql connection fail!!\r\n" + result.ToString());
                return false;
            }

            if (dtResults.Count() == 0 || dtResults[0].Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            this.dtHeadInfo = dtResults[0];
            this.dtSewing = dtResults[1];
            this.dtPPA = dtResults[2];
            this.dtNonSewing = dtResults[3];
            this.dtLineMapping_MachineArea = new DataTable[] { dtResults[4], dtResults[5], dtResults[6] };
            this.dtChart = dtResults[7];
            this.dtPPA_MachineArea = new DataTable[] { dtResults[8], dtResults[9], dtResults[10] };
            this.dtPPASort = dtResults[11];

            return true;
        }

        /// <summary>
        /// ToExcel
        /// </summary>
        /// <returns>bool</returns>
        public bool ToExcel()
        {
            string strXltName = Env.Cfg.XltPathDir + "\\IE_P05_Print.xltx";
            Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return false;
            }

#if DEBUG
            excel.Visible = true;
#endif

            // Sheet - Sewing Operation
            this.SetOperationSheet(excel.ActiveWorkbook.Worksheets[4], this.dtSewing, OperationType.Sewing);

            // excel 範圍別名宣告 公式使用
            excel.ActiveWorkbook.Names.Add("Operation", excel.ActiveWorkbook.Worksheets[4].Range["A6", "L" + this.dtSewing.Rows.Count + 5]);

            // Sheet - Centralized PPA Operation
            this.SetOperationSheet(excel.ActiveWorkbook.Worksheets[5], this.dtPPA, OperationType.PPA);

            // excel 範圍別名宣告 公式使用
            excel.ActiveWorkbook.Names.Add("OperationPPA", excel.ActiveWorkbook.Worksheets[5].Range["A6", "L" + this.dtPPA.Rows.Count + 5]);

            // Sheet - Non Sewing Line Operation
            this.SetOperationSheet(excel.ActiveWorkbook.Worksheets[6], this.dtNonSewing, OperationType.NonSewing);

            // Sheet - Line Mapping
            this.SetLineMappingSheet(excel.ActiveWorkbook.Worksheets[1], this.dtLineMapping_MachineArea, OperationType.Sewing, excel.ActiveWorkbook.Worksheets[2]);

            // Sheet - Centralized PPA
            this.SetLineMappingSheet(excel.ActiveWorkbook.Worksheets[3], this.dtPPA_MachineArea, OperationType.PPA);

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("IE_P05_Print");
            Excel.Workbook workbook = excel.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(workbook);

            strExcelName.OpenFile();
            #endregion

            return true;
        }

        private void SetOperationSheet(Excel.Worksheet sheet, DataTable dtOperation, OperationType operationType)
        {
            sheet.Cells[2, 2] = MyUtility.Convert.GetString(this.dtHeadInfo.Rows[0]["Style"]); // #Style
            sheet.Cells[3, 2] = MyUtility.Convert.GetString(this.dtHeadInfo.Rows[0]["Season"]); // #Season
            sheet.Cells[4, 2] = MyUtility.Convert.GetString(this.dtHeadInfo.Rows[0]["Brand"]); // #Brand
            sheet.Cells[5, 3] = MyUtility.Convert.GetString(this.dtHeadInfo.Rows[0]["Content"]); // #Content

            // 填Operation資料
            int intRowsStart = 6;
            int intRowTotalGSDTime = 0;
            object[,] objArray = new object[1, 13];
            foreach (DataRow dr in dtOperation.Rows)
            {
                objArray[0, 0] = dr["Seq"];
                objArray[0, 1] = dr["Location"];
                objArray[0, 2] = dr["Operation"];
                objArray[0, 3] = dr["MachineTypeID"];
                objArray[0, 4] = dr["MasterPlusGroup"];
                objArray[0, 5] = dr["Attachment"];
                objArray[0, 6] = dr["SewingMachineAttachmentID"];
                objArray[0, 7] = dr["Template"];
                objArray[0, 8] = dr["GSD"];
                objArray[0, 9] = dr["SewerDiffPercentage"];
                objArray[0, 10] = dr["ThreadComboID"];
                objArray[0, 11] = dr["OperationID"];
                sheet.Range[string.Format("A{0}:M{0}", intRowsStart)].Value2 = objArray;

                if (operationType == OperationType.Sewing)
                {
                    // 遇到特定OperationID時，前一筆設定下框線
                    if (intRowTotalGSDTime == 0 && dr["OperationID"].ToString().IsOneOfThe("PROCIPF00003", "PROCIPF00004"))
                    {
                        intRowTotalGSDTime = intRowsStart - 1;
                        Excel.Borders borders = sheet.Range[string.Format("A{0}:N{0}", intRowTotalGSDTime)].Borders;
                        borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlDouble;
                    }
                }

                intRowsStart++;
            }

            // [Total GSD Time] & [Avg. GSD Time]
            if (operationType.IsOneOfThe(OperationType.Sewing, OperationType.PPA)
                && dtOperation.Rows.Count > 0)
            {
                // 沒有特定OperationID時，就在最後一筆設定下框線
                if (intRowTotalGSDTime == 0)
                {
                    intRowTotalGSDTime = intRowsStart - 1;
                    Excel.Borders borders = sheet.Range[string.Format("M{0}:N{0}", intRowTotalGSDTime)].Borders;
                    borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlDouble;
                }

                if (operationType == OperationType.Sewing)
                {
                    this.SewingRowTotalGSDTime = intRowTotalGSDTime;
                }
                else
                {
                    this.PPARowTotalGSDTime = intRowTotalGSDTime;
                }

                if (dtOperation.Rows.Count > 2)
                {
                    int idx = intRowTotalGSDTime - 2;
                    sheet.get_Range($"M{idx}:M{intRowTotalGSDTime - 1}").Merge();
                    sheet.Cells[idx, 13] = "Total" + Environment.NewLine + "GSD time";
                    sheet.Cells[idx, 13].HorizontalAlignment = Excel.Constants.xlRight; // 設定靠右對齊
                    sheet.Cells[idx, 13].Font.Bold = true; // 設定粗體字

                    sheet.get_Range($"N{idx}:N{intRowTotalGSDTime - 1}").Merge();
                    sheet.Cells[idx, 14] = "Avg." + Environment.NewLine + "GSD time";
                    sheet.Cells[idx, 14].HorizontalAlignment = Excel.Constants.xlRight; // 設定靠右對齊
                    sheet.Cells[idx, 14].Font.Bold = true; // 設定粗體字
                }
                else
                {
                    sheet.Cells[5, 13] = "Total" + Environment.NewLine + "GSD time";
                    sheet.Cells[5, 14] = "Avg." + Environment.NewLine + "GSD time";

                    sheet.get_Range($"M5:N5").HorizontalAlignment = Excel.Constants.xlRight; // 設定靠右對齊
                    sheet.get_Range($"M5:N5").Font.Bold = true; // 設定粗體字
                }

                // Total GSD Time
                sheet.Cells[intRowTotalGSDTime, 13] = $"=ROUND(SUMPRODUCT($I$6:$I${intRowTotalGSDTime},$J$6:$J${intRowTotalGSDTime}),2)";

                int sewer = MyUtility.Convert.GetInt(this.dtHeadInfo.Rows[0]["SewerManpower"]);
                if (operationType == OperationType.PPA)
                {
                    sewer = MyUtility.Convert.GetInt(this.dtHeadInfo.Rows[0]["PPASewer"]);
                }

                // Avg. GSD Time
                sheet.Cells[intRowTotalGSDTime, 14] = $"=ROUND($M${intRowTotalGSDTime}/{sewer},2)";
            }

            sheet.Range[string.Format("A5:L{0}", intRowsStart - 1)].Borders.Weight = 1; // 1: 虛線, 2:實線, 3:粗體線
            sheet.Range[string.Format("A5:L{0}", intRowsStart - 1)].Borders.LineStyle = 1;

            intRowsStart++;
            sheet.Cells[intRowsStart, 2] = "Picture";
            intRowsStart += 2;

            // 圖片範圍
            Excel.Range range = sheet.Range[string.Format("B{0}:C{1}", intRowsStart, intRowsStart + 18)];

            // 設定外框
            range.Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;
            range.Borders[Excel.XlBordersIndex.xlEdgeTop].Weight = Excel.XlBorderWeight.xlMedium;
            range.Borders[Excel.XlBordersIndex.xlEdgeTop].Color = Color.Red;
            range.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
            range.Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlMedium;
            range.Borders[Excel.XlBordersIndex.xlEdgeBottom].Color = Color.Red;
            range.Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlContinuous;
            range.Borders[Excel.XlBordersIndex.xlEdgeLeft].Weight = Excel.XlBorderWeight.xlMedium;
            range.Borders[Excel.XlBordersIndex.xlEdgeLeft].Color = Color.Red;
            range.Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
            range.Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = Excel.XlBorderWeight.xlMedium;
            range.Borders[Excel.XlBordersIndex.xlEdgeRight].Color = Color.Red;

            string destination_path;
            if (DBProxy.Current.DefaultModuleName.Contains("PMSDB") || DBProxy.Current.DefaultModuleName.Contains("testing"))
            {
                // 台北開啟系統
                destination_path = MyUtility.GetValue.Lookup("select PicturePath from TradeSystem WITH (NOLOCK)", "Trade");
            }
            else
            {
                // 工廠開啟系統
                destination_path = MyUtility.GetValue.Lookup("select StyleSketch from System WITH (NOLOCK)", null);
            }

            string picture12 = string.Format("select Picture1, Picture2 from Style where ukey = {0}", this.dtHeadInfo.Rows[0]["StyleUKey"]);
            DataRow pdr;
            MyUtility.Check.Seek(picture12, out pdr);
            float left = (float)range.Left + 10;
            float top = (float)range.Top + 10;

            // Picture1
            if (!MyUtility.Check.Empty(pdr["Picture1"]))
            {
                string filepath = destination_path + MyUtility.Convert.GetString(pdr["Picture1"]);
                if (File.Exists(filepath))
                {
                    this.SetPicture(filepath, sheet, range, left, top);

                    left += (float)range.Width / 2;
                }
            }

            // Picture2
            if (!MyUtility.Check.Empty(pdr["Picture2"]))
            {
                string filepath = destination_path + MyUtility.Convert.GetString(pdr["Picture2"]);
                if (File.Exists(filepath))
                {
                    this.SetPicture(filepath, sheet, range, left, top);
                }
            }
        }

        private void SetPicture(string filepath, Excel.Worksheet sheet, Excel.Range range, float left, float top)
        {
            Image img = Image.FromFile(filepath);

            float pictureAreaWidth = ((float)range.Width / 2) - 20;
            float pictureAreaHeight = (float)range.Height - 20;
            float scale = pictureAreaWidth / img.Width;
            float scaleHeight = pictureAreaHeight / img.Height;

            // 取最小的倍率
            if (scale > scaleHeight)
            {
                scale = scaleHeight;
            }

            var imageWidth = (float)(img.Width * scale);
            var imageHeight = (float)(img.Height * scale);

            // 置中距離計算
            float leftDiff = (pictureAreaWidth - imageWidth) / 2;
            float topDiff = (pictureAreaHeight - imageHeight) / 2;

            sheet.Shapes.AddPicture(filepath, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, left + leftDiff, top + topDiff, imageWidth, imageHeight);
        }

        private void SetLineMappingSheet(Excel.Worksheet sheet, DataTable[] dtMachineArea, OperationType operationType, Excel.Worksheet chartData = null)
        {
            #region 固定資料

            // 左上表頭資料
            sheet.Cells[1, 4] = MyUtility.Convert.GetString(this.dtHeadInfo.Rows[0]["FactoryID"]); // Factory
            sheet.Cells[7, 4] = MyUtility.Convert.GetString(this.dtHeadInfo.Rows[0]["Style"]);     // Style
            sheet.Cells[9, 4] = MyUtility.Convert.GetString(this.dtHeadInfo.Rows[0]["StyleCPU"]);  // CPU/pc
            sheet.Cells[12, 6] = MyUtility.Convert.GetString(this.dtHeadInfo.Rows[0]["Content"]);  // #Content
            sheet.Cells[12, 23] = MyUtility.Convert.GetString(this.dtHeadInfo.Rows[0]["Content"]);  // #Content

            if (operationType == OperationType.Sewing)
            {
                // 右下簽名位置
                sheet.Cells[30, 4] = DateTime.Now.ToString("yyyy/MM/dd");                                  // Print Date
                sheet.Cells[33, 4] = MyUtility.Convert.GetString(this.dtHeadInfo.Rows[0]["PrintBy"]);      // Print By
                sheet.Cells[36, 4] = MyUtility.Convert.GetString(this.dtHeadInfo.Rows[0]["ConfirmedBy"]);  // Confirm By

                // 左下欄位資料
                sheet.Cells[48, 3] = MyUtility.Convert.GetString(this.dtHeadInfo.Rows[0]["Version"]);          // Version
                sheet.Cells[50, 3] = MyUtility.Convert.GetString(this.dtHeadInfo.Rows[0]["WorkHour"]);         // No. of Hours
                sheet.Cells[52, 3] = MyUtility.Convert.GetString(this.dtHeadInfo.Rows[0]["SewerManpower"]);    // No. of Sewer
                sheet.Cells[54, 3] = "=ROUND(3600*C52/C64,0)";                                                 // Target / Hr. (100%)
                sheet.Cells[56, 3] = "=ROUND(C54*C50,0)";                                                      // Daily Demand / Shift
                sheet.Cells[58, 3] = "=ROUND(3600*C50/C56,2)";                                                 // Takt Time
                sheet.Cells[60, 3] = MyUtility.Convert.GetString(this.dtHeadInfo.Rows[0]["EOLR"]);             // EOLR
                sheet.Cells[62, 3] = "=ROUND(C60*D9/C52,2)";                                                   // PPH
                sheet.Cells[64, 3] = $"='Sewing Operation'!M{this.SewingRowTotalGSDTime}";                     // Total GSD Time
                sheet.Cells[66, 3] = MyUtility.Convert.GetString(this.dtHeadInfo.Rows[0]["LBRByGSDTime"]);     // LBR by GSD Time
            }

            #endregion

            #region Machine 區塊
            int machineRowIndex = 31;
            if (dtMachineArea[0].Rows.Count > 0)
            {
                object[,] objArray = new object[1, 4];
                foreach (DataRow dr in dtMachineArea[0].Rows)
                {
                    // 合併儲存格
                    Excel.Range rangeToMerge = sheet.Range[$"L{machineRowIndex}:M{machineRowIndex}"];
                    rangeToMerge.Merge();

                    rangeToMerge = sheet.Range[$"N{machineRowIndex}:P{machineRowIndex}"];
                    rangeToMerge.Merge();

                    // 填資料
                    objArray[0, 0] = dr["MachineTypeID"];
                    objArray[0, 1] = dr["MasterPlusGroup"];
                    objArray[0, 2] = null;
                    objArray[0, 3] = dr["Count"];
                    sheet.Range[$"K{machineRowIndex}:N{machineRowIndex}"].Value2 = objArray;
                    machineRowIndex++;
                }

                sheet.Range["N28"].Value2 = $"=SUM(N31:N{machineRowIndex - 1})";
                sheet.Range[$"K31:P{machineRowIndex - 1}"].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter; // 水平置中
                sheet.Range[$"K31:P{machineRowIndex - 1}"].Borders.Weight = 2; // 1: 虛線, 2:實線, 3:粗體線
                sheet.Range[$"K31:P{machineRowIndex - 1}"].Borders.LineStyle = 1;
            }

            #endregion

            #region Attachment / Tempate 區塊
            sheet.Range["T28"].Value2 = dtMachineArea[1].Rows[0]["AttachmentCount"];
            sheet.Range["T29"].Value2 = dtMachineArea[1].Rows[0]["TemplateCount"];

            int attachmentRowIndex = 31;
            if (dtMachineArea[2].Rows.Count > 0)
            {
                object[,] objArray = new object[1, 5];
                foreach (DataRow dr in dtMachineArea[2].Rows)
                {
                    // 合併儲存格
                    Excel.Range rangeToMerge = sheet.Range[$"R{attachmentRowIndex}:S{attachmentRowIndex}"];
                    rangeToMerge.Merge();

                    rangeToMerge = sheet.Range[$"T{attachmentRowIndex}:U{attachmentRowIndex}"];
                    rangeToMerge.Merge();

                    rangeToMerge = sheet.Range[$"V{attachmentRowIndex}:X{attachmentRowIndex}"];
                    rangeToMerge.Merge();

                    // 填資料
                    objArray[0, 0] = dr["Item"];
                    objArray[0, 1] = null;
                    objArray[0, 2] = dr["No"];
                    objArray[0, 3] = null;
                    objArray[0, 4] = dr["Detail"];
                    sheet.Range[$"R{attachmentRowIndex}:V{attachmentRowIndex}"].Value2 = objArray;
                    attachmentRowIndex++;
                }

                sheet.Range[$"R31:X{attachmentRowIndex - 1}"].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter; // 水平置中
                sheet.Range[$"R31:X{attachmentRowIndex - 1}"].Borders.Weight = 2; // 1: 虛線, 2:實線, 3:粗體線
                sheet.Range[$"R31:X{attachmentRowIndex - 1}"].Borders.LineStyle = 1;
            }

            #endregion

            if (operationType == OperationType.Sewing)
            {
                #region Line Balancing Graph 圖表

                // 填資料
                int intRowsStart = 2;
                object[,] objArray = new object[1, 4];
                foreach (DataRow dr in this.dtChart.Rows)
                {
                    objArray[0, 0] = dr["No"];
                    objArray[0, 1] = dr["ActGSDTime"];
                    objArray[0, 2] = dr["TaktTime"];
                    objArray[0, 3] = dr["ActGSDTime_Avg"];
                    chartData.Range[$"A{intRowsStart}:D{intRowsStart}"].Value2 = objArray;
                    intRowsStart++;
                }

                int chartDataEndRow = this.dtChart.Rows.Count + 1;

                // 新增長條圖
                Excel.Range chartRange;
                object misValue = System.Reflection.Missing.Value;
                Excel.ChartObjects xlsCharts = (Excel.ChartObjects)sheet.ChartObjects(Type.Missing);
                var rowIndex = (machineRowIndex > attachmentRowIndex ? machineRowIndex : attachmentRowIndex) + 3;
                Excel.Range chartPositionCell = sheet.Range[$"K{rowIndex}"]; // 定位
                Excel.ChartObject myChart = xlsCharts.Add(chartPositionCell.Left, chartPositionCell.Top, 28.06 * 28.35, 8.41 * 28.35); // 寬 & 高 (point = cm * 28.35)
                Excel.Chart chartPage = myChart.Chart;
                chartRange = chartData.get_Range("B1", $"B{chartDataEndRow}");
                chartPage.SetSourceData(chartRange, misValue);
                chartPage.ChartType = Excel.XlChartType.xlColumnClustered;

                // 新增折線圖
                Excel.SeriesCollection seriesCollection = chartPage.SeriesCollection();
                Excel.Series series1 = seriesCollection.NewSeries();
                series1.Values = chartData.get_Range("C2", $"C{chartDataEndRow}");
                series1.XValues = chartData.get_Range("A2", $"A{chartDataEndRow}");
                series1.Name = "Takt time";
                series1.ChartType = Excel.XlChartType.xlLine;

                // 新增折線圖
                Excel.SeriesCollection seriesCollection_actTime = chartPage.SeriesCollection();
                Excel.Series series1_actTime = seriesCollection_actTime.NewSeries();
                series1_actTime.Values = chartData.get_Range("D2", $"D{chartDataEndRow}");
                series1_actTime.XValues = chartData.get_Range("A2", $"A{chartDataEndRow}");
                series1_actTime.Name = "Act Cycle Time(average)";
                series1_actTime.ChartType = Excel.XlChartType.xlLine;

                // 更改圖表版面配置 && 填入圖表標題 & 座標軸標題
                chartPage.ApplyLayout(9);
                chartPage.ChartTitle.Select();
                chartPage.ChartTitle.Text = "Line Balancing Graph";
                Excel.Axis z = (Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlValue, Excel.XlAxisGroup.xlPrimary);
                z.AxisTitle.Text = "Act Cycle Time (in secs)";
                z = (Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlCategory, Excel.XlAxisGroup.xlPrimary);
                z.AxisTitle.Text = "Operator No.";

                // 折線圖的資料標籤不顯示
                series1.ApplyDataLabels(Excel.XlDataLabelsType.xlDataLabelsShowNone, false, false);

                // 隱藏Sheet
                chartData.Visible = Excel.XlSheetVisibility.xlSheetHidden;
                #endregion
            }

            DataTable dtSort = operationType == OperationType.Sewing ? this.dtChart : this.dtPPASort;
            DataTable dtSewingData = operationType == OperationType.Sewing ? this.dtSewing : this.dtPPA;
            string alias = operationType == OperationType.Sewing ? "Operation" : "OperationPPA";

            #region 預設站數為2站，當超過2站就要新增
            decimal no_count = MyUtility.Convert.GetDecimal(dtSort.Rows.Count);
            int ttlLineRowCnt = MyUtility.Convert.GetInt(Math.Ceiling(no_count / 2));
            if (no_count > 2)
            {
                Excel.Range rngToCopy = sheet.get_Range("A17:A21").EntireRow; // 選取要被複製的資料
                for (int j = 1; j < ttlLineRowCnt; j++)
                {
                    Excel.Range rngToInsert = sheet.get_Range("A17", Type.Missing).EntireRow; // 選擇要被貼上的位置
                    rngToInsert.Insert(Excel.XlInsertShiftDirection.xlShiftDown, rngToCopy.Copy(Type.Missing)); // 貼上
                }
            }
            #endregion

            int norow = 17 + (ttlLineRowCnt * 5) - 5; // No格子上的位置Excel Y軸

            string sheetName = operationType == OperationType.Sewing ? "Sewing Operation" : "Centralized PPA Operation";
            int rowTotalGSDTime = operationType == OperationType.Sewing ? this.SewingRowTotalGSDTime : this.PPARowTotalGSDTime;

            #region U字型列印
            if (this.display.StartsWith("U"))
            {
                int maxct = 3;
                int di = dtSort.Rows.Count;
                int addct = 0;
                bool flag = true;
                decimal dd = Math.Ceiling((decimal)di / 2);
                List<int> listMax_ct = new List<int>();
                for (int i = 0; i < dd; i++)
                {
                    int a = MyUtility.Convert.GetInt(dtSort.Rows[i]["ct"]);
                    int d = 0;
                    if (di % 2 == 1 && flag)
                    {
                        flag = false;
                    }
                    else
                    {
                        if (di % 2 == 1)
                        {
                            d = MyUtility.Convert.GetInt(dtSort.Rows[di - i]["ct"]);
                        }
                        else
                        {
                            d = MyUtility.Convert.GetInt(dtSort.Rows[di - 1 - i]["ct"]);
                        }
                    }

                    maxct = a > d ? a : d;
                    maxct = maxct > 3 ? maxct : 3;
                    listMax_ct.Add(maxct);
                    Excel.Range rngToInsert = sheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(norow + 3)), Type.Missing).EntireRow;
                    for (int k = 3; k < maxct; k++)
                    {
                        rngToInsert.Insert(Excel.XlInsertShiftDirection.xlShiftDown);
                        sheet.get_Range(string.Format("F{0}:J{0}", MyUtility.Convert.GetString(norow + k))).Merge(false); // 合併儲存格

                        addct++;
                    }

                    // 將公式填入對應的格子中
                    for (int q = 1; q <= maxct; q++)
                    {
                        this.AddLineMappingFormula(sheet, norow + q + 1, alias);
                    }

                    norow -= 5;
                    maxct = 3;
                }

                bool reverse = false;

                string endRow = (16 + (ttlLineRowCnt * 5) + addct).ToString();
                int m = 0;

                if (listMax_ct.Count == 0)
                {
                    listMax_ct.Add(0);
                }

                norow = MyUtility.Convert.GetInt(endRow) - (listMax_ct[0] + 1);

                foreach (DataRow nodr in dtSort.Rows)
                {
                    if (!reverse)
                    {
                        int loadingPctcolumn = this.display.EndsWith("L") ? 1 : 28;
                        int loadingTimecolumn = this.display.EndsWith("L") ? 4 : 25;
                        int seqcolumn = this.display.EndsWith("L") ? 5 : 22;
                        int nocolumn = this.display.EndsWith("L") ? 14 : 18;

                        // Operator loading (%)
                        sheet.Cells[norow, loadingPctcolumn] = $"={MyExcelPrg.GetExcelColumnName(loadingTimecolumn)}{norow}/'{sheetName}'!N{rowTotalGSDTime}";

                        // Station No.
                        sheet.Cells[norow, nocolumn] = MyUtility.Convert.GetString(nodr["No"]);

                        DataRow[] nodrs = dtSewingData.Select($@"No = '{MyUtility.Convert.GetString(nodr["No"])}'").OrderBy(x => MyUtility.Convert.GetInt(x["Seq"])).ToArray();
                        int ridx = 2;
                        foreach (DataRow item in nodrs)
                        {
                            // Seq.
                            sheet.Cells[norow + ridx, seqcolumn] = item["Seq"].ToString();

                            ridx++;
                        }

                        // Loading Time (第一行)
                        sheet.Cells[norow, loadingTimecolumn] = string.Format("=SUM({0}{1}:{0}{2})", MyExcelPrg.GetExcelColumnName(loadingTimecolumn), norow + 2, norow + ridx - 1);

                        m++;
                        if (m == dd)
                        {
                            reverse = true;
                            m--;
                            continue;
                        }

                        norow = norow - 5 - (listMax_ct[m] - 3);
                    }
                    else
                    {
                        int loadingPctcolumn = this.display.EndsWith("L") ? 28 : 1;
                        int loadingTimecolumn = this.display.EndsWith("L") ? 25 : 4;
                        int seqcolumn = this.display.EndsWith("L") ? 22 : 5;
                        int nocolumn = this.display.EndsWith("L") ? 18 : 14;

                        // Operator loading (%)
                        sheet.Cells[norow, loadingPctcolumn] = $"={MyExcelPrg.GetExcelColumnName(loadingTimecolumn)}{norow}/'{sheetName}'!N{rowTotalGSDTime}";

                        // Station No.
                        sheet.Cells[norow, nocolumn] = MyUtility.Convert.GetString(nodr["No"]);

                        DataRow[] nodrs = dtSewingData.Select($@"No = '{MyUtility.Convert.GetString(nodr["No"])}'").OrderBy(x => MyUtility.Convert.GetInt(x["Seq"])).ToArray();
                        int ridx = 2;
                        foreach (DataRow item in nodrs)
                        {
                            // Seq.
                            sheet.Cells[norow + ridx, seqcolumn] = item["Seq"].ToString();

                            ridx++;
                        }

                        // Loading Time (第一行)
                        sheet.Cells[norow, loadingTimecolumn] = string.Format("=SUM({0}{1}:{0}{2})", MyExcelPrg.GetExcelColumnName(loadingTimecolumn), norow + 2, norow + ridx - 1);

                        norow = norow + 5 + (listMax_ct[m] - 3);
                        m--;
                    }
                }
            }
            #endregion

            #region S / Z字型列印
            if (this.display.StartsWith("S") || this.display.StartsWith("Z"))
            {
                int maxct = 3;
                int ct = 0;
                int addct = 0;
                int indx = 1;
                List<int> listMax_ct = new List<int>();
                for (int l = 0; l < dtSort.Rows.Count + 1; l++)
                {
                    if (l < dtSort.Rows.Count)
                    {
                        maxct = MyUtility.Convert.GetInt(dtSort.Rows[l]["ct"]) > maxct ? MyUtility.Convert.GetInt(dtSort.Rows[l]["ct"]) : maxct;
                    }

                    ct++;
                    if (ct == 2)
                    {
                        listMax_ct.Add(maxct);
                        Excel.Range rngToInsert = sheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(norow + 3)), Type.Missing).EntireRow;
                        for (int i = 3; i < maxct; i++)
                        {
                            rngToInsert.Insert(Excel.XlInsertShiftDirection.xlShiftDown);
                            sheet.get_Range(string.Format("F{0}:J{0}", MyUtility.Convert.GetString(norow + i))).Merge(false); // 合併儲存格

                            addct++;
                        }

                        // 將公式填入對應的格子中
                        for (int q = 1; q <= maxct; q++)
                        {
                            this.AddLineMappingFormula(sheet, norow + q + 1, alias);
                        }

                        norow -= 5;
                        ct = 0;
                        maxct = 3;
                    }
                }

                string endRow = (16 + (ttlLineRowCnt * 5) + addct).ToString();

                int leftright_count = 2; // S字型列印用
                bool leftDirection = this.display.EndsWith("L");
                indx = 0;
                norow = MyUtility.Convert.GetInt(endRow) + 1;
                foreach (DataRow nodr in dtSort.Rows)
                {
                    if (dtSort.Rows.IndexOf(nodr) % 2 == 0)
                    {
                        norow -= listMax_ct[indx] + 2;
                        indx++;
                    }

                    if (leftDirection)
                    {
                        // Operator loading (%)
                        sheet.Cells[norow, 1] = $"=D{norow}/'{sheetName}'!N{rowTotalGSDTime}";

                        // Station No.
                        sheet.Cells[norow, 14] = MyUtility.Convert.GetString(nodr["No"]);

                        DataRow[] nodrs = dtSewingData.Select($@"No = '{MyUtility.Convert.GetString(nodr["No"])}'").OrderBy(x => MyUtility.Convert.GetInt(x["Seq"])).ToArray();
                        int ridx = 2;
                        string machinetype = string.Empty;
                        string machinetypeL = string.Empty;
                        foreach (DataRow item in nodrs)
                        {
                            // Seq.
                            sheet.Cells[norow + ridx, 5] = item["Seq"].ToString();

                            ridx++;
                        }

                        // Loading Time (第一行)
                        sheet.Cells[norow, 4] = string.Format("=SUM(D{0}:D{1})", norow + 2, norow + ridx - 1);

                        // S字型單測累計兩次要換邊 (LRRLLRRLLR)
                        if (this.display.StartsWith("S"))
                        {
                            leftright_count++;
                            if (leftright_count > 2)
                            {
                                leftright_count = 1;
                                leftDirection = false;
                            }
                        }

                        // Z字型左右換邊 (LRLRLRLRLR)
                        else
                        {
                            leftDirection = false;
                        }
                    }
                    else
                    {
                        // Operator loading (%)
                        sheet.Cells[norow, 28] = $"=Y{norow}/'{sheetName}'!N{rowTotalGSDTime}";

                        // Station No.
                        sheet.Cells[norow, 18] = MyUtility.Convert.GetString(nodr["No"]);

                        DataRow[] nodrs = dtSewingData.Select($@"No = '{MyUtility.Convert.GetString(nodr["No"])}'").OrderBy(x => MyUtility.Convert.GetInt(x["Seq"])).ToArray();
                        int ridx = 2;
                        string machinetype = string.Empty;
                        string machinetypeL = string.Empty;
                        foreach (DataRow item in nodrs)
                        {
                            // Seq.
                            sheet.Cells[norow + ridx, 22] = item["Seq"].ToString();

                            ridx++;
                        }

                        // Loading Time (第一行)
                        sheet.Cells[norow, 25] = string.Format("=SUM(Y{0}:Y{1})", norow + 2, norow + ridx - 1);

                        // S字型單測累計兩次要換邊 (LRRLLRRLLR)
                        if (this.display.StartsWith("S"))
                        {
                            leftright_count++;
                            if (leftright_count > 2)
                            {
                                leftright_count = 1;
                                leftDirection = true;
                            }
                        }

                        // Z字型左右換邊 (LRLRLRLRLR)
                        else
                        {
                            leftDirection = true;
                        }
                    }
                }
            }
            #endregion
        }

        private void AddLineMappingFormula(Excel.Worksheet worksheet, int rownum, string alias)
        {
            // GSD Time
            worksheet.Cells[rownum, 2] = $"=IF(ISNA(VLOOKUP(E{rownum},{alias},9,0)),\"\",VLOOKUP(E{rownum},{alias},9,0))";
            worksheet.Cells[rownum, 27] = $"=IF(ISNA(VLOOKUP(V{rownum},{alias},9,0)),\"\",VLOOKUP(V{rownum},{alias},9,0))";

            // Loading (%)
            worksheet.Cells[rownum, 3] = $"=IF(ISNA(VLOOKUP(E{rownum},{alias},10,0)),\"\",VLOOKUP(E{rownum},{alias},10,0))";
            worksheet.Cells[rownum, 26] = $"=IF(ISNA(VLOOKUP(V{rownum},{alias},10,0)),\"\",VLOOKUP(V{rownum},{alias},10,0))";

            // Loading Time
            worksheet.Cells[rownum, 4] = $"=IF(ISNA(VLOOKUP(E{rownum},{alias},9,0)*VLOOKUP(E{rownum},{alias},10,0)),\"\",VLOOKUP(E{rownum},{alias},9,0)*VLOOKUP(E{rownum},{alias},10,0))";
            worksheet.Cells[rownum, 25] = $"=IF(ISNA(VLOOKUP(V{rownum},{alias},9,0)*VLOOKUP(V{rownum},{alias},10,0)),\"\",VLOOKUP(V{rownum},{alias},9,0)*VLOOKUP(V{rownum},{alias},10,0))";

            // Operation
            worksheet.Cells[rownum, 6] = $"=IF(ISNA(VLOOKUP(E{rownum},{alias},3,0)),\"\",VLOOKUP(E{rownum},{alias},3,0))";
            worksheet.Cells[rownum, 23] = $"=IF(ISNA(VLOOKUP(V{rownum},{alias},3,0)),\"\",VLOOKUP(V{rownum},{alias},3,0))";

            // Attachment / Template
            worksheet.Cells[rownum, 11] = $"=IF(ISNA(VLOOKUP(E{rownum},{alias},6,0) & VLOOKUP(E{rownum},{alias},8,0)),\"\",VLOOKUP(E{rownum},{alias},6,0) & VLOOKUP(E{rownum},{alias},8,0))";
            worksheet.Cells[rownum, 21] = $"=IF(ISNA(VLOOKUP(V{rownum},{alias},6,0) & VLOOKUP(V{rownum},{alias},8,0)),\"\",VLOOKUP(V{rownum},{alias},6,0) & VLOOKUP(V{rownum},{alias},8,0))";

            // MC Group
            worksheet.Cells[rownum, 12] = $"=IF(ISNA(VLOOKUP(E{rownum},{alias},5,0)),\"\",VLOOKUP(E{rownum},{alias},5,0))";
            worksheet.Cells[rownum, 20] = $"=IF(ISNA(VLOOKUP(V{rownum},{alias},5,0)),\"\",VLOOKUP(V{rownum},{alias},5,0))";

            // ST/MC
            worksheet.Cells[rownum, 13] = $"=IF(ISNA(VLOOKUP(E{rownum},{alias},4,0)),\"\",VLOOKUP(E{rownum},{alias},4,0))";
            worksheet.Cells[rownum, 19] = $"=IF(ISNA(VLOOKUP(V{rownum},{alias},4,0)),\"\",VLOOKUP(V{rownum},{alias},4,0))";
        }
    }
}
