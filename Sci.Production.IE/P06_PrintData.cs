using Ict;
using Sci.Data;
using Sci.Production.Prg;
using Sci.Utility.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// P06_Report
    /// </summary>
    public class P06_PrintData
    {
        private enum OperationType
        {
            [Description("Sewing Line Operation")]
            Sewing,

            [Description("Centralized PPA Operation")]
            PPA,

            [Description("Non Sewing Line Operation")]
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
        private DataTable dtChart2;
        private DataTable dtPPASort;
        private DataTable rightDataTable;

        private int SewingRowTotalCycle = 0;
        private int PPARowTotalCycle = 0;

        private string driection;

        /// <summary>
        /// P05_Report
        /// </summary>
        /// <param name="almID">LineMappingBalancing.ID</param>
        /// <param name="display">導出方式</param>
        /// <param name="content">工段資訊要導出[Operation] or [Annotation]的模式</param>
        /// <param name="language">工段顯示語系</param>
        /// <param name="direction">排列方向</param>
        public P06_PrintData(string almID = "", string display = "", string content = "", string language = "", string direction = "")
        {
            this.almID = almID;
            this.display = display;
            this.content = content;
            this.language = language;
            this.driection = direction;
        }

        /// <summary>
        /// SetCondition
        /// </summary>
        /// <param name="almID">AutomatedLineMapping.ID</param>
        /// <param name="display">導出方式</param>
        /// <param name="content">工段資訊要導出[Operation] or [Annotation]的模式</param>
        /// <param name="language">工段顯示語系</param>
        /// <param name="direction">排列方向</param>
        /// <param name="rightDataTable">排列方向</param>
        public void SetCondition(string almID, string display, string content, string language, string direction = "", DataTable rightDataTable = null)
        {
            this.almID = almID;
            this.display = display;
            this.content = content;
            this.language = language;
            this.driection = direction;
            this.rightDataTable = rightDataTable;
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
	FROM LineMappingBalancing_Detail almd
	WHERE almd.ID = @ALMID
	and almd.PPA = 'C'
	and almd.IsNonSewingLine = 0
)
SELECT alm.StyleUKey
, Style = CONCAT(alm.StyleID, ' - ', alm.ComboType)
, Season = alm.SeasonID
, Brand = alm.BrandID
, Content = ddl.Name
, alm.SewingLineID
, alm.FactoryID
, alm.StyleCPU
, alm.Version
, alm.WorkHour
, EOLR = IIF(alm.HighestCycleTime = 0, 0, ROUND(3600 / alm.HighestCycleTime, 2))
, LBRByCycleTime = IIF(alm.HighestCycleTime = 0, 0, ROUND(alm.TotalCycleTime / alm.HighestCycleTime / alm.SewerManpower, 4))
, alm.SewerManpower
, alm.TotalGSDTime
, AvgGSDTime = ROUND(alm.TotalGSDTime / alm.SewerManpower, 2)
, PPASewer = getPPA.Sewer
, TotalGSDTime_PPA = ISNULL(ROUND(getPPA.TotalGSDTime, 2), 0)
, AvgGSDTime_PPA = ISNULL(ROUND(getPPA.TotalGSDTime / getPPA.Sewer, 2), 0)
, ConfirmedBy = CONCAT(alm.CFMName, '-', cfm.Name)
, PrintBy = CONCAT(printBy.ID, '-', printBy.Name)
, [FactoryNameEN] = f.NameEN
FROM LineMappingBalancing alm
LEFT JOIN DropDownList ddl on ddl.Type = 'Pms_LMContent' and ddl.ID = @Content
LEFT JOIN Pass1 cfm on cfm.ID = alm.CFMName
LEFT JOIN Pass1 printBy on printBy.ID = @UserID
LEFT JOIN Factory f with (nolock) on f.ID = alm.FactoryID
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
, Attachment = REPLACE(almd.Attachment, ',', CHAR(13) + CHAR(10))
, SewingMachineAttachmentID = REPLACE(almd.SewingMachineAttachmentID, ',', CHAR(13) + CHAR(10))
, Template = REPLACE(almd.Template, ',', CHAR(13) + CHAR(10))
, almd.GSD
, almd.Cycle
, almd.SewerDiffPercentage
, almd.ThreadComboID
, almd.OperationID
FROM LineMappingBalancing_Detail almd
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
, Attachment = REPLACE(almd.Attachment, ',', CHAR(13) + CHAR(10))
, SewingMachineAttachmentID = REPLACE(almd.SewingMachineAttachmentID, ',', CHAR(13) + CHAR(10))
, Template = REPLACE(almd.Template, ',', CHAR(13) + CHAR(10))
, almd.GSD
, almd.Cycle
, almd.SewerDiffPercentage
, almd.ThreadComboID
, almd.OperationID
FROM LineMappingBalancing_Detail almd
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
, Attachment = REPLACE(almd.Attachment, ',', CHAR(13) + CHAR(10))
, SewingMachineAttachmentID = REPLACE(almd.SewingMachineAttachmentID, ',', CHAR(13) + CHAR(10))
, Template = REPLACE(almd.Template, ',', CHAR(13) + CHAR(10))
, almd.GSD
, almd.Cycle
, almd.SewerDiffPercentage
, almd.ThreadComboID
, almd.OperationID
FROM LineMappingBalancing_Detail almd
LEFT JOIN Operation o on almd.OperationID = o.ID
WHERE almd.ID = @ALMID
AND almd.IsNonSewingLine = 1
ORDER BY almd.Seq ASC

-- Excel [Line Mapping] Machine區塊資料
select MachineTypeID, MasterPlusGroup, No, Seq, Attachment, SewingMachineAttachmentID, Template, concatString.Value
into #main
from LineMappingBalancing_Detail
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
            union all
			select [Data] = ThreadComboID
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
select Item = List.Data, main.No, Detail = REPLACE(getDetail.Value, ',', CHAR(13) + CHAR(10))
from #main main
outer apply (select tmpString = CONCAT(Attachment, iif(Template = '', '', ','), Template)) combine
outer apply (select * from dbo.SplitString(combine.tmpString, ',')) List
outer apply (
	select Value = isnull(STUFF((
		select ',' + ID
		from SewingMachineAttachment
		where ID in (select Data From dbo.SplitString(SewingMachineAttachmentID, ','))
		and MoldID = List.Data
		FOR XML PATH('')
	),1,1,''), '')
) getDetail 
where combine.tmpString != '' and List.Data != ''
Order by main.No, Seq, List.no

drop table #main

-- Excel [Line Mapping] Line Balancing Graph圖表資料
select No
, ActGSDTime = Round(Sum(almd.Cycle * almd.SewerDiffPercentage), 2)
, TaktTime = IIF(alm.TotalCycleTime = 0, 0, Round(3600 * alm.WorkHour / Round(Round(3600 * alm.SewerManpower / alm.TotalCycleTime, 0) * alm.WorkHour, 0), 2))
, ActGSDTime_Avg = Round(alm.TotalCycleTime / alm.SewerManpower, 2)
, ct = count(1)
from LineMappingBalancing alm
left join LineMappingBalancing_Detail almd on alm.ID = almd.ID
where alm.ID  = @ALMID
and almd.IsNonSewingLine = 0
and almd.PPA != 'C'
group by almd.No, alm.WorkHour, alm.SewerManpower, alm.TotalCycleTime

-- Excel [Line Mapping] Total GSD Time / Total Cycle Time Graph圖表資料
select No
, TotalGSDTime = Round(Sum(almd.GSD * almd.SewerDiffPercentage),2)
, TotalCycleTime = Round(Sum(almd.Cycle * almd.SewerDiffPercentage),2)
from LineMappingBalancing_Detail almd
where almd.ID = @ALMID
and almd.IsNonSewingLine = 0
and almd.PPA != 'C'
group by almd.No

-- Excel [Centralized PPA] Machine區塊資料
select MachineTypeID, MasterPlusGroup, No, Seq, Attachment, SewingMachineAttachmentID, Template, concatString.Value
into #main_PPA
from LineMappingBalancing_Detail
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
select Item = List.Data, main.No, Detail = REPLACE(getDetail.Value, ',', CHAR(13) + CHAR(10))
from #main_PPA main
outer apply (select tmpString = CONCAT(Attachment, iif(Template = '', '', ','), Template)) combine
outer apply (select * from dbo.SplitString(combine.tmpString, ',')) List
outer apply (
	select Value = isnull(STUFF((
		select ',' + ID
		from SewingMachineAttachment
		where ID in (select Data From dbo.SplitString(SewingMachineAttachmentID, ','))
		and MoldID = List.Data
		FOR XML PATH('')
	),1,1,''), '')
) getDetail 
where combine.tmpString != ''
Order by main.No, Seq, List.no

drop table #main_PPA

-- Excel [Centralized PPA] 排序資料
select No
, ct = count(1)
from LineMappingBalancing alm
left join LineMappingBalancing_Detail almd on alm.ID = almd.ID
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
            this.dtChart2 = dtResults[8];
            this.dtPPA_MachineArea = new DataTable[] { dtResults[9], dtResults[10], dtResults[11] };
            this.dtPPASort = dtResults[12];

            return true;
        }

        public bool ToExcel()
        {
            string strXltName = Env.Cfg.XltPathDir + "\\IE_P06_Print.xltx";
            Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            // excel.Visible = true;
            if (excel == null)
            {
                return false;
            }

            // Sheet - Sewing Operation
            this.SetOperationSheet(excel.ActiveWorkbook.Worksheets[5], this.dtSewing, OperationType.Sewing);

            // excel 範圍別名宣告 公式使用
            excel.ActiveWorkbook.Names.Add("Operation", excel.ActiveWorkbook.Worksheets[5].Range["A6", "L" + this.dtSewing.Rows.Count + 5]);

            // Sheet - Centralized PPA Operation
            this.SetOperationSheet(excel.ActiveWorkbook.Worksheets[6], this.dtPPA, OperationType.PPA);

            // excel 範圍別名宣告 公式使用
            excel.ActiveWorkbook.Names.Add("OperationPPA", excel.ActiveWorkbook.Worksheets[6].Range["A6", "L" + this.dtPPA.Rows.Count + 5]);

            // Sheet - Non Sewing Line Operation
            this.SetOperationSheet(excel.ActiveWorkbook.Worksheets[7], this.dtNonSewing, OperationType.NonSewing);

            // Sheet - Line Mapping
            this.SetLineMappingSheet(excel.ActiveWorkbook.Worksheets[1], this.dtLineMapping_MachineArea, OperationType.Sewing, excel.ActiveWorkbook.Worksheets[2], excel.ActiveWorkbook.Worksheets[3]);

            // Sheet - Centralized PPA
            this.SetLineMappingSheet(excel.ActiveWorkbook.Worksheets[4], this.dtPPA_MachineArea, OperationType.PPA);

            for (int i = 1; i < 4; i++)
            {
                Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[i];

                // 設置列印設置
                worksheet.PageSetup.PaperSize = Excel.XlPaperSize.xlPaperA4; // 設置紙張大小為 A4
                worksheet.PageSetup.Orientation = Excel.XlPageOrientation.xlPortrait; // 設置為縱向列印
                worksheet.PageSetup.Zoom = false; // 禁用默認縮放比例
                worksheet.PageSetup.FitToPagesWide = 1; // 將內容縮放以適合一頁寬
                worksheet.PageSetup.FitToPagesTall = false;

                // 設置頁邊距，使得頁邊距最小
                worksheet.PageSetup.LeftMargin = excel.InchesToPoints(0.25); // 左邊距 0.25 英寸
                worksheet.PageSetup.RightMargin = excel.InchesToPoints(0.25); // 右邊距 0.25 英寸
                worksheet.PageSetup.TopMargin = excel.InchesToPoints(0.25); // 上邊距 0.25 英寸
                worksheet.PageSetup.BottomMargin = excel.InchesToPoints(0.25); // 下邊距 0.25 英寸
            }


            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("IE_P06_Print");
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
            sheet.Cells[1, 1] = MyUtility.Convert.GetString(this.dtHeadInfo.Rows[0]["FactoryNameEN"]) + Environment.NewLine + operationType.GetDescription(); // #FactoryNameEN
            sheet.Cells[2, 2] = MyUtility.Convert.GetString(this.dtHeadInfo.Rows[0]["Style"]); // #Style
            sheet.Cells[3, 2] = MyUtility.Convert.GetString(this.dtHeadInfo.Rows[0]["Season"]); // #Season
            sheet.Cells[4, 2] = MyUtility.Convert.GetString(this.dtHeadInfo.Rows[0]["Brand"]); // #Brand
            sheet.Cells[5, 3] = MyUtility.Convert.GetString(this.dtHeadInfo.Rows[0]["Content"]); // #Content

            // 填Operation資料
            int intRowsStart = 6;
            int intRowTotalCycle = 0;
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
                objArray[0, 9] = dr["Cycle"];
                objArray[0, 10] = dr["SewerDiffPercentage"];
                objArray[0, 11] = dr["ThreadComboID"];
                objArray[0, 12] = dr["OperationID"];
                sheet.Range[string.Format("A{0}:M{0}", intRowsStart)].Value2 = objArray;

                if (operationType == OperationType.Sewing)
                {
                    // 遇到特定OperationID時，前一筆設定下框線
                    if (intRowTotalCycle == 0 && dr["OperationID"].ToString().IsOneOfThe("PROCIPF00003", "PROCIPF00004"))
                    {
                        intRowTotalCycle = intRowsStart - 1;
                        Excel.Borders borders = sheet.Range[string.Format("A{0}:O{0}", intRowTotalCycle)].Borders;
                        borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlDouble;
                    }
                }

                intRowsStart++;
            }

            // [Total Cycle] & [Avg. Cycle]
            if (operationType.IsOneOfThe(OperationType.Sewing, OperationType.PPA)
                && dtOperation.Rows.Count > 0)
            {
                // 沒有特定OperationID時，就在最後一筆設定下框線
                if (intRowTotalCycle == 0)
                {
                    intRowTotalCycle = intRowsStart - 1;
                    Excel.Borders borders = sheet.Range[string.Format("N{0}:O{0}", intRowTotalCycle)].Borders;
                    borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlDouble;
                }

                if (operationType == OperationType.Sewing)
                {
                    this.SewingRowTotalCycle = intRowTotalCycle;
                }
                else
                {
                    this.PPARowTotalCycle = intRowTotalCycle;
                }

                if (dtOperation.Rows.Count > 2)
                {
                    int idx = intRowTotalCycle - 2;
                    sheet.get_Range($"N{idx}:N{intRowTotalCycle - 1}").Merge();
                    sheet.Cells[idx, 14] = "Total" + Environment.NewLine + "Cycle Time";
                    sheet.Cells[idx, 14].HorizontalAlignment = Excel.Constants.xlRight; // 設定靠右對齊
                    sheet.Cells[idx, 14].Font.Bold = true; // 設定粗體字

                    sheet.get_Range($"O{idx}:O{intRowTotalCycle - 1}").Merge();
                    sheet.Cells[idx, 15] = "Avg." + Environment.NewLine + "Cycle Time";
                    sheet.Cells[idx, 15].HorizontalAlignment = Excel.Constants.xlRight; // 設定靠右對齊
                    sheet.Cells[idx, 15].Font.Bold = true; // 設定粗體字
                }
                else
                {
                    sheet.Cells[5, 14] = "Total" + Environment.NewLine + "Cycle Time";
                    sheet.Cells[5, 15] = "Avg." + Environment.NewLine + "Cycle Time";

                    sheet.get_Range($"N5:O5").HorizontalAlignment = Excel.Constants.xlRight; // 設定靠右對齊
                    sheet.get_Range($"N5:O5").Font.Bold = true; // 設定粗體字
                }

                // Total Cycle
                sheet.Cells[intRowTotalCycle, 14] = $"=ROUND(SUMPRODUCT($J$6:$J${intRowTotalCycle},$K$6:$K${intRowTotalCycle}),2)";

                int sewer = MyUtility.Convert.GetInt(this.dtHeadInfo.Rows[0]["SewerManpower"]);
                if (operationType == OperationType.PPA)
                {
                    sewer = MyUtility.Convert.GetInt(this.dtHeadInfo.Rows[0]["PPASewer"]);
                }

                // Avg. Cycle
                sheet.Cells[intRowTotalCycle, 15] = $"=ROUND($N${intRowTotalCycle}/{sewer},2)";
            }

            sheet.Range[string.Format("A5:M{0}", intRowsStart - 1)].Borders.Weight = 1; // 1: 虛線, 2:實線, 3:粗體線
            sheet.Range[string.Format("A5:M{0}", intRowsStart - 1)].Borders.LineStyle = 1;

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

            sheet.Rows.AutoFit();
            ((Excel.Range)sheet.Cells[1, 1]).RowHeight = 48;
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

        private void SetLineMappingSheet(Excel.Worksheet sheet, DataTable[] dtMachineArea, OperationType operationType, Excel.Worksheet chartData = null, Excel.Worksheet chart2Data = null)
        {
            string sheetName = operationType == OperationType.Sewing ? "Sewing Operation" : "Centralized PPA Operation";

            #region 固定資料

            // 左上表頭資料
            sheet.Cells[1, 5] = MyUtility.Convert.GetString(this.dtHeadInfo.Rows[0]["FactoryID"]);      // Factory
            sheet.Cells[5, 5] = MyUtility.Convert.GetString(this.dtHeadInfo.Rows[0]["SewingLineID"]);   // SewingLineID
            sheet.Cells[7, 5] = MyUtility.Convert.GetString(this.dtHeadInfo.Rows[0]["Style"]);          // Style
            sheet.Cells[9, 5] = MyUtility.Convert.GetString(this.dtHeadInfo.Rows[0]["StyleCPU"]);       // CPU/pc
            sheet.Cells[12, 7] = MyUtility.Convert.GetString(this.dtHeadInfo.Rows[0]["Content"]);       // #Content
            sheet.Cells[12, 24] = MyUtility.Convert.GetString(this.dtHeadInfo.Rows[0]["Content"]);      // #Content

            if (operationType == OperationType.Sewing)
            {
                // 右下簽名位置
                sheet.Cells[30, 5] = DateTime.Now.ToString("yyyy/MM/dd");                                  // Print Date
                sheet.Cells[33, 5] = MyUtility.Convert.GetString(this.dtHeadInfo.Rows[0]["PrintBy"]);      // Print By
                sheet.Cells[36, 5] = MyUtility.Convert.GetString(this.dtHeadInfo.Rows[0]["ConfirmedBy"]);  // Confirm By

                // 左下欄位資料
                sheet.Cells[48, 4] = MyUtility.Convert.GetString(this.dtHeadInfo.Rows[0]["Version"]);       // Version
                sheet.Cells[50, 4] = MyUtility.Convert.GetString(this.dtHeadInfo.Rows[0]["WorkHour"]);      // No. of Hours
                sheet.Cells[52, 4] = MyUtility.Convert.GetString(this.dtHeadInfo.Rows[0]["SewerManpower"]); // No. of Sewer
                sheet.Cells[54, 4] = "=ROUND(3600*D52/D66,0)";                                              // Target / Hr. (100%)
                sheet.Cells[56, 4] = "=ROUND(D54*D50,0)";                                                   // Daily Demand / Shift
                sheet.Cells[58, 4] = "=ROUND(3600*D50/D56,2)";                                              // Takt Time
                sheet.Cells[60, 4] = MyUtility.Convert.GetString(this.dtHeadInfo.Rows[0]["EOLR"]);          // EOLR
                sheet.Cells[62, 4] = "=ROUND(D60*E9/D52,2)";                                                // PPH

                int i = this.SewingRowTotalCycle;
                sheet.Cells[64, 4] = $"='{sheetName}'!N{i}";                                                // Total Cycle Time
                sheet.Cells[66, 4] = $"=ROUND(SUMPRODUCT('{sheetName}'!I6:I{i},'{sheetName}'!K6:K{i}),2)";  // Total GSD Time
                sheet.Cells[68, 4] = "=(D66-D64)/D66";                                                      // Total % Time Diff
                sheet.Cells[70, 4] = MyUtility.Convert.GetString(this.dtHeadInfo.Rows[0]["LBRByCycleTime"]); // LBR by Cycle Time
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
                    Excel.Range rangeToMerge = sheet.Range[$"M{machineRowIndex}:N{machineRowIndex}"];
                    rangeToMerge.Merge();

                    rangeToMerge = sheet.Range[$"O{machineRowIndex}:Q{machineRowIndex}"];
                    rangeToMerge.Merge();

                    // 填資料
                    objArray[0, 0] = dr["MachineTypeID"];
                    objArray[0, 1] = dr["MasterPlusGroup"];
                    objArray[0, 2] = null;
                    objArray[0, 3] = dr["Count"];
                    sheet.Range[$"L{machineRowIndex}:O{machineRowIndex}"].Value2 = objArray;
                    machineRowIndex++;
                }

                sheet.Range["O28"].Value2 = $"=SUM(O31:O{machineRowIndex - 1})";
                sheet.Range[$"L31:N{machineRowIndex - 1}"].HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft; // 靠左對齊
                sheet.Range[$"L31:N{machineRowIndex - 1}"].NumberFormat = "@"; // 文字格式
                sheet.Range[$"O31:Q{machineRowIndex - 1}"].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight; // Count靠右對齊
                sheet.Range[$"O31:Q{machineRowIndex - 1}"].NumberFormat = 0; // 數字格式(正整數)
                sheet.Range[$"L31:Q{machineRowIndex - 1}"].Borders.Weight = 2; // 1: 虛線, 2:實線, 3:粗體線
                sheet.Range[$"L31:Q{machineRowIndex - 1}"].Borders.LineStyle = 1;
            }

            #endregion

            #region Attachment / Tempate 區塊
            sheet.Range["U28"].Value2 = dtMachineArea[1].Rows[0]["AttachmentCount"];
            sheet.Range["U29"].Value2 = dtMachineArea[1].Rows[0]["TemplateCount"];

            int attachmentRowIndex = 31;
            sheet.Range[$"S31:Y{attachmentRowIndex + dtMachineArea[2].Rows.Count - 1}"].NumberFormat = "@"; // 文字格式

            if (dtMachineArea[2].Rows.Count > 0)
            {
                object[,] objArray = new object[1, 5];
                foreach (DataRow dr in dtMachineArea[2].Rows)
                {
                    // 合併儲存格
                    Excel.Range rangeToMerge = sheet.Range[$"S{attachmentRowIndex}:T{attachmentRowIndex}"];
                    rangeToMerge.Merge();

                    rangeToMerge = sheet.Range[$"U{attachmentRowIndex}:V{attachmentRowIndex}"];
                    rangeToMerge.Merge();

                    rangeToMerge = sheet.Range[$"W{attachmentRowIndex}:Y{attachmentRowIndex}"];
                    rangeToMerge.Merge();

                    // 填資料
                    objArray[0, 0] = dr["Item"];
                    objArray[0, 1] = null;
                    objArray[0, 2] = dr["No"];
                    objArray[0, 3] = null;
                    objArray[0, 4] = dr["Detail"];
                    sheet.Range[$"S{attachmentRowIndex}:W{attachmentRowIndex}"].Value2 = objArray;

                    // 合併儲存格在自動調整行高時只會看第一欄，在此依據Detail換行的次數將第一欄填入對應數量的換行符號
                    var lineCount = dr["Detail"].ToString().Split(new string[] { Environment.NewLine }, StringSplitOptions.None).Length;
                    if (lineCount > 1)
                    {
                        sheet.Cells[attachmentRowIndex, 1] = string.Join(Environment.NewLine, new string[lineCount]);
                    }

                    attachmentRowIndex++;
                }

                sheet.Range[$"S31:Y{attachmentRowIndex - 1}"].HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft; // 靠左對齊
                sheet.Range[$"S31:Y{attachmentRowIndex - 1}"].Borders.Weight = 2; // 1: 虛線, 2:實線, 3:粗體線
                sheet.Range[$"S31:Y{attachmentRowIndex - 1}"].Borders.LineStyle = 1;
            }

            #endregion
            if (operationType == OperationType.Sewing)
            {
                #region Line Balancing Graph

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
                Excel.Range chartPositionCell = sheet.Range[$"L{rowIndex}"]; // 定位
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
                series1_actTime.Name = "Avg. Cycle Time";
                series1_actTime.ChartType = Excel.XlChartType.xlLine;

                // 更改圖表版面配置 && 填入圖表標題 & 座標軸標題
                chartPage.ApplyLayout(9);
                chartPage.ChartTitle.Select();
                chartPage.ChartTitle.Text = "Line Balancing Graph";
                Excel.Axis z = (Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlValue, Excel.XlAxisGroup.xlPrimary);
                z.AxisTitle.Text = "Avg. Cycle Time";
                z = (Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlCategory, Excel.XlAxisGroup.xlPrimary);
                z.AxisTitle.Text = "Operator No.";

                // 折線圖的資料標籤不顯示
                series1.ApplyDataLabels(Excel.XlDataLabelsType.xlDataLabelsShowNone, false, false);

                // 隱藏Sheet
                chartData.Visible = Excel.XlSheetVisibility.xlSheetHidden;
                #endregion

                #region Total GSD Time / Total Cycle Time Graph

                // 填資料
                intRowsStart = 2;
                objArray = new object[1, 3];
                foreach (DataRow dr in this.dtChart2.Rows)
                {
                    objArray[0, 0] = dr["No"];
                    objArray[0, 1] = dr["TotalGSDTime"];
                    objArray[0, 2] = dr["TotalCycleTime"];
                    chart2Data.Range[$"A{intRowsStart}:C{intRowsStart}"].Value2 = objArray;
                    intRowsStart++;
                }

                chartDataEndRow = this.dtChart2.Rows.Count + 1;

                // 新增長條圖
                misValue = System.Reflection.Missing.Value;
                xlsCharts = (Excel.ChartObjects)sheet.ChartObjects(Type.Missing);

                // 取得圖表1的位置和高度
                double top = myChart.Top;
                double height = myChart.Height;

                // 設定圖表2的位置
                double newTop = top + height + 20; // 在下方距離 20 point 的位置
                double newLeft = myChart.Left; // 與圖表 A 左邊對齊
                Excel.ChartObject myChart2 = xlsCharts.Add(newLeft, newTop, 28.06 * 28.35, 8.41 * 28.35); // 寬 & 高 (point = cm * 28.35)
                chartPage = myChart2.Chart;
                chartRange = chart2Data.get_Range("B1", $"B{chartDataEndRow}");
                chartPage.SetSourceData(chartRange, misValue);
                chartPage.ChartType = Excel.XlChartType.xlColumnClustered;

                // 新增折線圖
                Excel.SeriesCollection seriesCollection2 = chartPage.SeriesCollection();
                Excel.Series series2 = seriesCollection2.NewSeries();
                series2.Values = chart2Data.get_Range("C2", string.Format("C{0}", MyUtility.Convert.GetString(chartDataEndRow)));
                series2.XValues = chart2Data.get_Range("A2", string.Format("A{0}", MyUtility.Convert.GetString(chartDataEndRow)));
                series2.Name = "Total Cycle Time";

                // 折線圖的資料標籤不顯示
                series2.ApplyDataLabels(Excel.XlDataLabelsType.xlDataLabelsShowNone, false, false);

                // 隱藏Sheet
                chart2Data.Visible = Excel.XlSheetVisibility.xlSheetHidden;
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

            // Direction排序
            int norow = this.driection == "F" ? 17 + (ttlLineRowCnt * 5) - 5 : 17; // No格子上的位置Excel Y軸

            int rowTotalGSDTime = operationType == OperationType.Sewing ? this.SewingRowTotalCycle : this.PPARowTotalCycle;

            bool leftDirection = this.display.EndsWith("L");

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
                        sheet.get_Range(string.Format("G{0}:K{0}", MyUtility.Convert.GetString(norow + k))).Merge(false); // 合併儲存格

                        addct++;
                    }

                    // 將公式填入對應的格子中
                    for (int q = 1; q <= maxct; q++)
                    {
                        this.AddLineMappingFormula(sheet, norow + q + 1, alias);
                    }

                    if (this.driection == "F")
                    {
                        norow -= 5;
                    }
                    else
                    {
                        norow += 5;
                    }

                    maxct = 3;
                }

                bool reverse = false;

                string endRow = (16 + (ttlLineRowCnt * 5) + addct).ToString();
                int m = 0;

                if (listMax_ct.Count == 0)
                {
                    listMax_ct.Add(0);
                }

                // 寫入Operator區域的值進去
                norow = this.driection == "F" ? MyUtility.Convert.GetInt(endRow) - (listMax_ct[0] + 1) : 17;

                foreach (DataRow nodr in dtSort.Rows)
                {
                    int loadingPctcolumn = leftDirection ? 1 : 30;
                    int cycleTimecolumn = leftDirection ? 2 : 28;
                    int loadingTimecolumn = leftDirection ? 5 : 26;
                    int seqcolumn = leftDirection ? 6 : 23;
                    int nocolumn = leftDirection ? 15 : 19;
                    int stmcColumn = leftDirection ? 14 : 22;
                    int operatorColumn = leftDirection ? 10 : 20;

                    int MG = leftDirection ? 13 : 21;
                    int tc = leftDirection ? 12 : 20;



                    if (reverse)
                    {
                        loadingPctcolumn = leftDirection ? 30 : 1;
                        cycleTimecolumn = leftDirection ? 28 : 2;
                        loadingTimecolumn = leftDirection ? 26 : 5;
                        seqcolumn = leftDirection ? 23 : 6;
                        nocolumn = leftDirection ? 19 : 15;
                        stmcColumn = leftDirection ? 20 : 14;
                        operatorColumn = leftDirection ? 20 : 10;

                        MG = leftDirection ? 21 : 13;
                        tc = leftDirection ? 20 : 12;
                    }

                    // Operator loading (%)
                    sheet.Cells[norow, loadingPctcolumn] = $"={MyExcelPrg.GetExcelColumnName(loadingTimecolumn)}{norow}/'{sheetName}'!O{rowTotalGSDTime}";

                    // Station No.
                    sheet.Cells[norow, nocolumn] = MyUtility.Convert.GetString(nodr["No"]);

                    DataRow[] nodrs = dtSewingData.Select($@"No = '{MyUtility.Convert.GetString(nodr["No"])}'").OrderBy(x => MyUtility.Convert.GetInt(x["Seq"])).ToArray();
                    int ridx = 2;

                    for (int x = 0; x < nodrs.Length; x++)
                    {
                        // Seq.
                        sheet.Cells[norow + ridx, seqcolumn] = nodrs[x]["Seq"].ToString();

                        // 相同MachineTypeID Attachment MasterPlusGroup ThreadComboId Template的值，Excel：MC Group、ST/MC、Thread Combination 欄位清空
                        if (x > 0)
                        {
                            if (nodrs[x]["MachineTypeID"].ToString() == nodrs[x - 1]["MachineTypeID"].ToString() &&
                                nodrs[x]["Attachment"].ToString() == nodrs[x - 1]["Attachment"].ToString() &&
                                nodrs[x]["MasterPlusGroup"].ToString() == nodrs[x - 1]["MasterPlusGroup"].ToString() &&
                                nodrs[x]["ThreadComboId"].ToString() == nodrs[x - 1]["ThreadComboId"].ToString() &&
                                nodrs[x]["Template"].ToString() == nodrs[x - 1]["Template"].ToString())
                            {

                                sheet.Cells[norow + ridx, tc] = string.Empty;
                                sheet.Cells[norow + ridx, MG] = string.Empty;
                                sheet.Cells[norow + ridx, stmcColumn] = string.Empty;

                            }
                        }

                        /******* ISP20240132：字形大小切換 *******/
                        Excel.Range cell = sheet.Cells[norow + ridx, stmcColumn];
                        var list = this.dtSewing.AsEnumerable().Where(row => row.Field<short>("Seq") == MyUtility.Convert.GetInt(nodrs[x]["Seq"])).ToList();
                        int i = list.AsEnumerable().Where(row =>
                                                        (row["MachineTypeID"].ToString() != string.Empty ? 1 : 0) +
                                                        (row["Attachment"].ToString() != string.Empty ? 1 : 0) +
                                                        (row["Template"].ToString() != string.Empty ? 1 : 0) >= 2)
                                                    .ToList().Count;

                        cell.Font.Size = i > 0 ? 16 : 18;
                        /****************************************/
                        ridx++;
                    }

                    int idx_s = norow + 2;
                    int idx_e = norow + ridx - 1;

                    // Cycle Time GSD Time (第一行)
                    string cycleTimeFormula;
                    if (leftDirection)
                    {
                        cycleTimeFormula = "=(SUMPRODUCT(C{0}:C{1},D{0}:D{1})-SUMPRODUCT(B{0}:B{1},D{0}:D{1}))/SUMPRODUCT(C{0}:C{1},D{0}:D{1})";
                    }
                    else
                    {
                        cycleTimeFormula = "=(SUMPRODUCT(AB{0}:AB{1},AA{0}:AA{1})-SUMPRODUCT(AC{0}:AC{1},AA{0}:AA{1}))/SUMPRODUCT(AB{0}:AB{1},AA{0}:AA{1})";
                    }

                    sheet.Cells[norow, cycleTimecolumn] = string.Format(cycleTimeFormula, idx_s, idx_e);

                    // Loading Time (第一行)
                    sheet.Cells[norow, loadingTimecolumn] = string.Format("=SUM({0}{1}:{0}{2})", MyExcelPrg.GetExcelColumnName(loadingTimecolumn), idx_s, idx_e);

                    var operatorList = this.rightDataTable.AsEnumerable().Where(row => row["No"].ToString() == MyUtility.Convert.GetString(nodr["No"])).ToList();
                    sheet.Cells[norow, operatorColumn] = operatorList[0]["EmployeeID"].ToString() + "　" + operatorList[0]["EmployeeName"].ToString();
                    if (this.driection == "F")
                    {
                        if (!reverse)
                        {
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
                            norow = norow + 5 + (listMax_ct[m] - 3);
                            m--;
                        }
                    }
                    else
                    {
                        if (!reverse)
                        {
                            if (di % 2 == 0)
                            {
                                if (m == dd - 1)
                                {
                                    reverse = true;
                                    continue;
                                }
                            }
                            else
                            {
                                if (m == dd - 1)
                                {
                                    reverse = true;
                                    m--;
                                    continue;
                                }
                            }

                            norow = norow + 5 + (listMax_ct[m] - 3);
                            m++;
                        }
                        else
                        {
                            norow = norow - 5 - (listMax_ct[m] - 3);
                            m--;
                        }
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
                            sheet.get_Range(string.Format("G{0}:K{0}", MyUtility.Convert.GetString(norow + i))).Merge(false); // 合併儲存格

                            addct++;
                        }

                        // 將公式填入對應的格子中
                        for (int q = 1; q <= maxct; q++)
                        {
                            this.AddLineMappingFormula(sheet, norow + q + 1, alias);
                        }

                        if (this.driection == "F")
                        {
                            norow -= 5;
                        }
                        else
                        {
                            norow += 5;
                        }

                        ct = 0;
                        maxct = 3;
                    }
                }

                string endRow = (16 + (ttlLineRowCnt * 5) + addct).ToString();

                int leftright_count = 2; // S字型列印用
                indx = 0;
                norow = this.driection == "F" ? MyUtility.Convert.GetInt(endRow) + 1 : 17;
                foreach (DataRow nodr in dtSort.Rows)
                {
                    if (dtSort.Rows.IndexOf(nodr) % 2 == 0)
                    {
                        if (this.driection == "F")
                        {
                            norow -= listMax_ct[indx] + 2;
                        }
                        else
                        {
                            if (indx == 0)
                            {
                                norow = 17;
                            }
                            else
                            {
                                norow += listMax_ct[indx - 1] + 2;
                            }
                        }
                        indx++;
                    }

                    if (leftDirection)
                    {
                        // Operator loading (%)
                        sheet.Cells[norow, 1] = $"=E{norow}/'{sheetName}'!O{rowTotalGSDTime}";

                        // Station No.
                        sheet.Cells[norow, 15] = MyUtility.Convert.GetString(nodr["No"]);

                        DataRow[] nodrs = dtSewingData.Select($@"No = '{MyUtility.Convert.GetString(nodr["No"])}'").OrderBy(x => MyUtility.Convert.GetInt(x["Seq"])).ToArray();
                        int ridx = 2;
                        string machinetype = string.Empty;
                        string machinetypeL = string.Empty;
                        for (int i = 0; i < nodrs.Length; i++)
                        {
                            // Seq.
                            sheet.Cells[norow + ridx, 6] = nodrs[i]["Seq"].ToString();

                            // 相同MachineTypeID Attachment MasterPlusGroup ThreadComboId Template的值，Excel：MC Group、ST/MC、Thread Combination 欄位清空
                            if (i > 0)
                            {
                                if (nodrs[i]["MachineTypeID"].ToString() == nodrs[i - 1]["MachineTypeID"].ToString() &&
                                    nodrs[i]["Attachment"].ToString() == nodrs[i - 1]["Attachment"].ToString() &&
                                    nodrs[i]["MasterPlusGroup"].ToString() == nodrs[i - 1]["MasterPlusGroup"].ToString() &&
                                    nodrs[i]["ThreadComboId"].ToString() == nodrs[i - 1]["ThreadComboId"].ToString() &&
                                    nodrs[i]["Template"].ToString() == nodrs[i - 1]["Template"].ToString())
                                {
                                    sheet.Cells[norow + ridx, 12] = string.Empty;
                                    sheet.Cells[norow + ridx, 13] = string.Empty;
                                    sheet.Cells[norow + ridx, 14] = string.Empty;
                                }
                            }

                            /******* ISP20240132：字形大小切換 *******/
                            Excel.Range cell = sheet.Cells[norow + ridx, 14];
                            var list = this.dtSewing.AsEnumerable().Where(row => row.Field<short>("Seq") == MyUtility.Convert.GetInt(nodrs[i]["Seq"])).ToList();
                            list = list.AsEnumerable().Where(row =>
                                                            (row["MachineTypeID"].ToString() != string.Empty ? 1 : 0) +
                                                            (row["Attachment"].ToString() != string.Empty ? 1 : 0) +
                                                            (row["Template"].ToString() != string.Empty ? 1 : 0) >= 2)
                                                        .ToList();

                            cell.Font.Size = list.Count > 0 ? 16 : 18;
                            /****************************************/
                            ridx++;
                        }

                        int idx_s = norow + 2;
                        int idx_e = norow + ridx - 1;

                        // Cycle Time GSD Time (第一行)
                        sheet.Cells[norow, 2] = string.Format("=(SUMPRODUCT(C{0}:C{1},D{0}:D{1})-SUMPRODUCT(B{0}:B{1},D{0}:D{1}))/SUMPRODUCT(C{0}:C{1},D{0}:D{1})", idx_s, idx_e);

                        // Loading Time (第一行)
                        sheet.Cells[norow, 5] = string.Format("=SUM(E{0}:E{1})", idx_s, idx_e);

                        var operatorList = this.rightDataTable.AsEnumerable().Where(row => row["No"].ToString() == MyUtility.Convert.GetString(nodr["No"])).ToList();

                        if (operatorList.Count > 0)
                        {
                            sheet.Cells[norow, 10] = MyUtility.Convert.GetString(operatorList[0]["EmployeeID"]) + "　" + MyUtility.Convert.GetString(operatorList[0]["EmployeeName"]);
                        }
                        else
                        {
                            sheet.Cells[norow, 10] = string.Empty;
                        }

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
                        sheet.Cells[norow, 30] = $"=Z{norow}/'{sheetName}'!O{rowTotalGSDTime}";

                        // Station No.
                        sheet.Cells[norow, 19] = MyUtility.Convert.GetString(nodr["No"]);

                        DataRow[] nodrs = dtSewingData.Select($@"No = '{MyUtility.Convert.GetString(nodr["No"])}'").OrderBy(x => MyUtility.Convert.GetInt(x["Seq"])).ToArray();
                        int ridx = 2;
                        string machinetype = string.Empty;
                        string machinetypeL = string.Empty;
                        for (int i = 0; i < nodrs.Length; i++)
                        {
                            // Seq.
                            sheet.Cells[norow + ridx, 23] = nodrs[i]["Seq"].ToString();

                            // 相同MachineTypeID Attachment MasterPlusGroup ThreadComboId Template的值，Excel：MC Group、ST/MC、Thread Combination 欄位清空
                            if (i > 0)
                            {
                                if (nodrs[i]["MachineTypeID"].ToString() == nodrs[i - 1]["MachineTypeID"].ToString() &&
                                    nodrs[i]["Attachment"].ToString() == nodrs[i - 1]["Attachment"].ToString() &&
                                    nodrs[i]["MasterPlusGroup"].ToString() == nodrs[i - 1]["MasterPlusGroup"].ToString() &&
                                    nodrs[i]["ThreadComboId"].ToString() == nodrs[i - 1]["ThreadComboId"].ToString() &&
                                    nodrs[i]["Template"].ToString() == nodrs[i - 1]["Template"].ToString())
                                {
                                    sheet.Cells[norow + ridx, 20] = string.Empty;
                                    sheet.Cells[norow + ridx, 21] = string.Empty;
                                    sheet.Cells[norow + ridx, 22] = string.Empty;
                                }
                            }

                            /******* ISP20240132：字形大小切換 *******/
                            Excel.Range cell = sheet.Cells[norow + ridx, 20];
                            var list = this.dtSewing.AsEnumerable().Where(row => row.Field<short>("Seq") == MyUtility.Convert.GetInt(nodrs[i]["Seq"])).ToList();
                            list = list.AsEnumerable().Where(row =>
                                                            (row["MachineTypeID"].ToString() != string.Empty ? 1 : 0) +
                                                            (row["Attachment"].ToString() != string.Empty ? 1 : 0) +
                                                            (row["Template"].ToString() != string.Empty ? 1 : 0) >= 2)
                                                        .ToList();

                            cell.Font.Size = list.Count > 0 ? 16 : 18;
                            /****************************************/
                            ridx++;
                        }

                        int idx_s = norow + 2;
                        int idx_e = norow + ridx - 1;

                        // Cycle Time GSD Time (第一行)
                        sheet.Cells[norow, 28] = string.Format("=(SUMPRODUCT(AB{0}:AB{1},AA{0}:AA{1})-SUMPRODUCT(AC{0}:AC{1},AA{0}:AA{1}))/SUMPRODUCT(AB{0}:AB{1},AA{0}:AA{1})", idx_s, idx_e);

                        // Loading Time (第一行)
                        sheet.Cells[norow, 26] = string.Format("=SUM(Z{0}:Z{1})", idx_s, idx_e);

                        var operatorList = this.rightDataTable.AsEnumerable().Where(row => row["No"].ToString() == MyUtility.Convert.GetString(nodr["No"])).ToList();
                        if (operatorList.Count > 0)
                        {
                            sheet.Cells[norow, 20] = operatorList[0]["EmployeeID"].ToString() + "　" + operatorList[0]["EmployeeName"].ToString();
                        }

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

            // sheet.Rows.AutoFit();
        }

        private void AddLineMappingFormula(Excel.Worksheet worksheet, int rownum, string alias)
        {
            // Cycle Time
            worksheet.Cells[rownum, 2] = $"=IF(ISNA(VLOOKUP(F{rownum},{alias},10,0)),\"\",VLOOKUP(F{rownum},{alias},10,0))";
            worksheet.Cells[rownum, 29] = $"=IF(ISNA(VLOOKUP(W{rownum},{alias},10,0)),\"\",VLOOKUP(W{rownum},{alias},10,0))";

            // GSD Time
            worksheet.Cells[rownum, 3] = $"=IF(ISNA(VLOOKUP(F{rownum},{alias},9,0)),\"\",VLOOKUP(F{rownum},{alias},9,0))";
            worksheet.Cells[rownum, 28] = $"=IF(ISNA(VLOOKUP(W{rownum},{alias},9,0)),\"\",VLOOKUP(W{rownum},{alias},9,0))";

            // Loading (%)
            worksheet.Cells[rownum, 4] = $"=IF(ISNA(VLOOKUP(F{rownum},{alias},11,0)),\"\",VLOOKUP(F{rownum},{alias},11,0))";
            worksheet.Cells[rownum, 27] = $"=IF(ISNA(VLOOKUP(W{rownum},{alias},11,0)),\"\",VLOOKUP(W{rownum},{alias},11,0))";

            // Loading Time
            worksheet.Cells[rownum, 5] = $"=IF(ISNA(VLOOKUP(F{rownum},{alias},10,0)*VLOOKUP(F{rownum},{alias},11,0)),\"\",VLOOKUP(F{rownum},{alias},10,0)*VLOOKUP(F{rownum},{alias},11,0))";
            worksheet.Cells[rownum, 26] = $"=IF(ISNA(VLOOKUP(W{rownum},{alias},10,0)*VLOOKUP(W{rownum},{alias},11,0)),\"\",VLOOKUP(W{rownum},{alias},10,0)*VLOOKUP(W{rownum},{alias},11,0))";

            // Operation
            worksheet.Cells[rownum, 7] = $"=IF(ISNA(VLOOKUP(F{rownum},{alias},3,0)),\"\",VLOOKUP(F{rownum},{alias},3,0))";
            worksheet.Cells[rownum, 24] = $"=IF(ISNA(VLOOKUP(W{rownum},{alias},3,0)),\"\",VLOOKUP(W{rownum},{alias},3,0))";

            // Attachment / Template (兩者串起後換行)
            worksheet.Cells[rownum, 12] = $"=IF(ISNA(VLOOKUP(F{rownum},{alias},12,0)),\"\",VLOOKUP(F{rownum},{alias},12,0))";
            worksheet.Cells[rownum, 12].WrapText = true;
            worksheet.Cells[rownum, 22] = $"=IF(ISNA(VLOOKUP(W{rownum},{alias},12,0)),\"\",VLOOKUP(W{rownum},{alias},12,0))";
            worksheet.Cells[rownum, 22].WrapText = true;

            // MC Group
            worksheet.Cells[rownum, 13] = $"=IF(ISNA(VLOOKUP(F{rownum},{alias},5,0)),\"\",IF(ISBLANK(VLOOKUP(F{rownum},{alias},5,0)),\"\",VLOOKUP(F{rownum},{alias},5,0)))";
            worksheet.Cells[rownum, 21] = $"=IF(ISNA(VLOOKUP(W{rownum},{alias},5,0)),\"\",IF(ISBLANK(VLOOKUP(W{rownum},{alias},5,0)),\"\",VLOOKUP(W{rownum},{alias},5,0)))";

            // ST/MC
            worksheet.Cells[rownum, 14] = $"=IF(ISNA(VLOOKUP(F{rownum},{alias},4,0)),\"\",VLOOKUP(F{rownum},{alias},4,0) & IF(VLOOKUP(F{rownum},{alias},6,0) =\"\", \"\", CHAR(10) & VLOOKUP(F{rownum},{alias},6,0)) & IF(VLOOKUP(F{rownum},{alias},8,0) =\"\" , \"\", CHAR(10) & VLOOKUP(F{rownum},{alias},8,0)))";
            worksheet.Cells[rownum, 20] = $"=IF(ISNA(VLOOKUP(W{rownum},{alias},4,0)),\"\",VLOOKUP(W{rownum},{alias},4,0) & IF(VLOOKUP(W{rownum},{alias},6,0) =\"\", \"\", CHAR(10) & VLOOKUP(W{rownum},{alias},6,0)) & IF(VLOOKUP(W{rownum},{alias},8,0) =\"\" , \"\", CHAR(10) & VLOOKUP(W{rownum},{alias},8,0)))";
        }
    }
}
