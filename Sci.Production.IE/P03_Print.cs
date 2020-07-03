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
using System.Runtime.InteropServices;
using System.IO;
using System.Linq;

namespace Sci.Production.IE
{
    /// <summary>
    /// IE_P03_Print
    /// </summary>
    public partial class P03_Print : Sci.Win.Tems.PrintForm
    {
        private DataRow masterData;
        private string display;
        private string contentType;
        private string strLanguage;
        private DataTable operationCode;
        private DataTable nodist;
        private DataTable noppa;
        private decimal styleCPU;
        private decimal takt1;
        private decimal takt2;
        private decimal changp;
        private decimal count1;
        private decimal count2;
        private decimal standardOutput1;
        private decimal standardOutput2;
        private decimal dailyDemand1;
        private decimal dailyDemand2;
        private decimal totalCycle1;
        private decimal totalCycle2;
        private decimal totalGSD1;
        private decimal totalGSD2;
        private bool change = false;
        private DataTable nodist2;

        /// <summary>
        /// P03_Print
        /// </summary>
        /// <param name="masterData">MasterData</param>
        /// <param name="styleCPU">StyleCPU</param>
        public P03_Print(DataRow masterData, decimal styleCPU)
        {
            this.InitializeComponent();
            this.masterData = masterData;
            this.styleCPU = styleCPU;
            this.radioU_Left.Checked = true;
            this.radioDescription.Checked = true;
            MyUtility.Tool.SetupCombox(this.comboLanguage, 2, 1, "en,English,cn,Chinese,vn,Vietnam,kh,Cambodia");
            this.comboLanguage.SelectedIndex = 0;
        }

        /// <summary>
        /// ValidateInput 驗證輸入條件
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ValidateInput()
        {
            if (this.radioU_Left.Checked)
            {
                this.display = "U_Left";
            }
            else if (this.radioU_Right.Checked)
            {
                this.display = "U_Right";
            }
            else
            {
                this.display = "Z";
            }

            this.contentType = this.radioDescription.Checked ? "D" : "A";
            this.changp = MyUtility.Convert.GetDecimal(this.numpage.Value);
            this.strLanguage = this.comboLanguage.SelectedValue.ToString();
            return base.ValidateInput();
        }

        /// <summary>
        /// OnAsyncDataLoad
        /// </summary>
        /// <param name="e">e</param>
        /// <returns>DualResult</returns>
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            #region 切分頁計算 takt
            decimal ttlnocount = MyUtility.Convert.GetInt(this.masterData["CurrentOperators"]);
            if (this.chkpage.Checked && ttlnocount > this.changp)
            {
                string sqlp1 = string.Format(
                    @"
select no = count(distinct no)
from LineMapping_Detail ld WITH (NOLOCK)
where ld.ID = {0} and (IsPPa = 0 or IsPPa is null) and no<={1}
",
                    MyUtility.Convert.GetString(this.masterData["ID"]),
                    this.changp);
                string sqlp2 = string.Format(
                    @"
select no = count(distinct no)
from LineMapping_Detail ld WITH (NOLOCK)
where ld.ID = {0} and (IsPPa = 0 or IsPPa is null) and no>{1}
",
                    MyUtility.Convert.GetString(this.masterData["ID"]),
                    this.changp);
                this.count1 = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup(sqlp1));
                this.count2 = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup(sqlp2));
                sqlp1 = string.Format(
                    @"
select sumCycle = sum(Cycle)
from LineMapping_Detail ld WITH (NOLOCK)
where ld.ID = {0} and (IsPPa = 0 or IsPPa is null)  and no<={1}
",
                    MyUtility.Convert.GetString(this.masterData["ID"]),
                    this.changp);
                sqlp2 = string.Format(
                    @"
select sumCycle = sum(Cycle)
from LineMapping_Detail ld WITH (NOLOCK)
where ld.ID = {0} and (IsPPa = 0 or IsPPa is null) and no>{1}
",
                    MyUtility.Convert.GetString(this.masterData["ID"]),
                    this.changp);

                this.totalCycle1 = Math.Round(MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup(sqlp1)), 0);
                this.totalCycle2 = Math.Round(MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup(sqlp2)), 0);
                decimal currentOperators1 = this.count1;
                decimal currentOperators2 = this.count2;
                this.standardOutput1 = this.totalCycle1 == 0 ? 0 : MyUtility.Math.Round(3600 * currentOperators1 / this.totalCycle1, 0);
                this.standardOutput2 = this.totalCycle2 == 0 ? 0 : MyUtility.Math.Round(3600 * currentOperators2 / this.totalCycle2, 0);
                this.dailyDemand1 = MyUtility.Math.Round(MyUtility.Convert.GetDecimal(this.masterData["Workhour"]) * this.standardOutput1, 0);
                this.dailyDemand2 = MyUtility.Math.Round(MyUtility.Convert.GetDecimal(this.masterData["Workhour"]) * this.standardOutput2, 0);
                this.takt1 = this.dailyDemand1 == 0 ? 0 : MyUtility.Math.Round(MyUtility.Convert.GetDecimal(this.masterData["NetTime"]) / this.dailyDemand1, 0);
                this.takt2 = this.dailyDemand2 == 0 ? 0 : MyUtility.Math.Round(MyUtility.Convert.GetDecimal(this.masterData["NetTime"]) / this.dailyDemand2, 0);

                sqlp1 = string.Format(
                   @"
select sumTotalGSD = sum(GSD)
from LineMapping_Detail ld WITH (NOLOCK)
where ld.ID = {0} and (IsPPa = 0 or IsPPa is null)  and no<={1}
",
                   MyUtility.Convert.GetString(this.masterData["ID"]),
                   this.changp);
                sqlp2 = string.Format(
                    @"
select sumTotalGSD = sum(GSD)
from LineMapping_Detail ld WITH (NOLOCK)
where ld.ID = {0} and (IsPPa = 0 or IsPPa is null) and no>{1}
",
                    MyUtility.Convert.GetString(this.masterData["ID"]),
                    this.changp);
                this.totalGSD1 = Math.Round(MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup(sqlp1)), 0);
                this.totalGSD2 = Math.Round(MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup(sqlp2)), 0);
                this.change = true;
            }
            else
            {
                this.change = false;
            }
            #endregion

            string sqlCmd;

            #region 第一頁
            sqlCmd = string.Format(
                @"
select   a.GroupKey
        ,a.OperationID
        ,a.Annotation
        ,a.GSD
        ,a.MachineTypeID--MachineTypeID = iif(m.MachineGroupID = '','',a.MachineTypeID)
		,a.MasterPlusGroup
        ,a.Attachment
        ,a.Template
        ,a.ThreadColor
        ,DescEN = case when '{1}' = 'cn' then isnull(od.DescCHS,o.DescEN)
                       when '{1}' = 'vn' then isnull(od.DescVI,o.DescEN)
                       when '{1}' = 'kh' then isnull(od.DescKH,o.DescEN)
            else o.DescEN end
        ,rn = ROW_NUMBER() over(order by iif(IsPPa=1,1,0) ,a.No)
        ,a.Cycle
        ,a.ActCycle
        ,IsPPa
        ,a.No
from LineMapping_Detail a 
left join Operation o WITH (NOLOCK) on o.ID = a.OperationID
left join OperationDesc od on o.ID = od.ID
left join MachineType m WITH (NOLOCK) on m.id =  a.MachineTypeID
where a.ID = {0}
order by iif(IsPPa=1,1,0) ,a.No
,a.MachineTypeID--iif(m.MachineGroupID = '','',a.MachineTypeID)
,a.Attachment,a.Template,a.ThreadColor", MyUtility.Convert.GetString(this.masterData["ID"]), this.strLanguage);
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out this.operationCode);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query operation code data fail\r\n" + result.ToString());
                return failResult;
            }
            #endregion
            #region Machine Type IsPPa
            sqlCmd = string.Format(
                @"
select   ld.OperationID
        ,ld.MachineTypeID--MachineTypeID = iif(m.MachineGroupID = '','',ld.MachineTypeID)
        ,ld.Annotation
        ,DescEN = case when '{1}' = 'cn' then isnull(od.DescCHS,o.DescEN)
                       when '{1}' = 'vn' then isnull(od.DescVI,o.DescEN)
                       when '{1}' = 'kh' then isnull(od.DescKH,o.DescEN)
            else o.DescEN end
from LineMapping_Detail ld WITH (NOLOCK)
left join MachineType m WITH (NOLOCK) on m.id =  MachineTypeID
left join Operation o WITH (NOLOCK) on o.ID = ld.OperationID
left join OperationDesc od on o.ID = od.ID
where ld.ID = {0} and IsPPa = 1 
order by ld.No,ld.GroupKey", MyUtility.Convert.GetString(this.masterData["ID"]), this.strLanguage);
            result = DBProxy.Current.Select(null, sqlCmd, out this.noppa);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query print data fail\r\n" + result.ToString());
                return failResult;
            }
            #endregion

            #region 第二頁
            if (!this.change)
            {
                sqlCmd = string.Format(
                    @"select No 
,CT = COUNT(1)
,[ActCycle] = Max(ld.ActCycle)
,[ActCycleTime(average)]=ActCycle.Value
from LineMapping_Detail ld WITH (NOLOCK) 
OUTER APPLY(
	SELECT [Value]=SUM(ActCycle)/COUNT(NO) FROM 
	(
		SELECT DISTINCT No, ActCycle, TotalGSD, TotalCycle
		FROM 
		(
			select  ld.*
					, Description = case when '{1}' = 'cn' then isnull(od.DescCHS,o.DescEN)
                                         when '{1}' = 'vn' then isnull(od.DescVI,o.DescEN)
                                         when '{1}' = 'kh' then isnull(od.DescKH,o.DescEN)
                                         else o.DescEN end
					, e.Name as EmployeeName
					, e.Skill as EmployeeSkill
					, iif(ld.Cycle = 0,0,ROUND(ld.GSD/ld.Cycle,2)*100) as Efficiency
			from LineMapping_Detail ld WITH (NOLOCK) 
			left join Employee e WITH (NOLOCK) on ld.EmployeeID = e.ID
			left join Operation o WITH (NOLOCK) on ld.OperationID = o.ID
            left join OperationDesc od on o.ID = od.ID
			where ld.ID = {0} AND No <> ''
		)a
	)b
)ActCycle
where ld.ID = {0} and (IsPPa = 0 or IsPPa is null)
GROUP BY NO ,ActCycle.Value
order by no", MyUtility.Convert.GetString(this.masterData["ID"]), this.strLanguage);
                result = DBProxy.Current.Select(null, sqlCmd, out this.nodist);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Query print data fail\r\n" + result.ToString());
                    return failResult;
                }
            }
            #endregion
            #region 第二頁 有分頁
            else
            {
                #region
                sqlCmd = string.Format(
                    @"
select id, minno = min(no), maxno = max(no)
into #tmp
from(
	select distinct ld.ID,no
	from LineMapping_Detail ld WITH (NOLOCK)
	where ld.ID = {0} and (IsPPa = 0 or IsPPa is null)
	and no <= {1}
)x
group by ID

select No
,CT = COUNT(1)
,[ActCycle] = Max(ld.ActCycle)
,[ActCycleTime(average)]=ActCycle.Value
from LineMapping_Detail ld WITH (NOLOCK) 
inner join #tmp t on ld.ID = t.ID
OUTER APPLY(
	SELECT [Value]=SUM(ActCycle)/COUNT(NO) FROM 
	(
		SELECT DISTINCT No, ActCycle, TotalGSD, TotalCycle
		FROM 
		(
			select  ld.*
					, Description = case when '{2}' = 'cn' then isnull(od.DescCHS,o.DescEN)
                                         when '{2}' = 'vn' then isnull(od.DescVI,o.DescEN)
                                         when '{2}' = 'kh' then isnull(od.DescKH,o.DescEN)
                                         else o.DescEN end
					, e.Name as EmployeeName
					, e.Skill as EmployeeSkill
					, iif(ld.Cycle = 0,0,ROUND(ld.GSD/ld.Cycle,2)*100) as Efficiency
			from LineMapping_Detail ld WITH (NOLOCK) 
			left join Employee e WITH (NOLOCK) on ld.EmployeeID = e.ID
			left join Operation o WITH (NOLOCK) on ld.OperationID = o.ID
            left join OperationDesc od on o.ID = od.ID
			where ld.ID = {0} AND No <> ''
		)a
	)b
)ActCycle
where  (IsPPa = 0 or IsPPa is null)  and no between t.minno and t.maxno
GROUP BY NO ,ActCycle.Value
order by no

",
                    MyUtility.Convert.GetString(this.masterData["ID"]),
                    this.changp
                    , strLanguage);
                result = DBProxy.Current.Select(null, sqlCmd, out this.nodist);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Query print data fail\r\n" + result.ToString());
                    return failResult;
                }

                sqlCmd = string.Format(
                    @"
select id, minno = min(no), maxno = max(no)
into #tmp
from(
	select distinct ld.ID,no
	from LineMapping_Detail ld WITH (NOLOCK)
	where ld.ID = {0} and (IsPPa = 0 or IsPPa is null) 
	and no > {1}
)x
group by ID

select No,CT = COUNT(1),[ActCycle] = Max(ld.ActCycle)
,[ActCycleTime(average)]=ActCycle.Value
from LineMapping_Detail ld WITH (NOLOCK) 
inner join #tmp t on ld.ID = t.ID

OUTER APPLY(
	SELECT [Value]=SUM(ActCycle)/COUNT(NO) FROM 
	(
		SELECT DISTINCT No, ActCycle, TotalGSD, TotalCycle
		FROM 
		(
			select  ld.*
					, Description = case when '{2}' = 'cn' then isnull(od.DescCHS,o.DescEN)
                                         when '{2}' = 'vn' then isnull(od.DescVI,o.DescEN)
                                         when '{2}' = 'kh' then isnull(od.DescKH,o.DescEN)
                                         else o.DescEN end
					, e.Name as EmployeeName
					, e.Skill as EmployeeSkill
					, iif(ld.Cycle = 0,0,ROUND(ld.GSD/ld.Cycle,2)*100) as Efficiency
			from LineMapping_Detail ld WITH (NOLOCK) 
			left join Employee e WITH (NOLOCK) on ld.EmployeeID = e.ID
			left join Operation o WITH (NOLOCK) on ld.OperationID = o.ID
            left join OperationDesc od on o.ID = od.ID
			where ld.ID = {0} AND No <> ''
		)a
	)b
)ActCycle
where  (IsPPa = 0 or IsPPa is null) and no between t.minno and t.maxno
GROUP BY NO ,ActCycle.Value
order by no

",
                    MyUtility.Convert.GetString(this.masterData["ID"]),
                    this.changp,
                    this.strLanguage);
                result = DBProxy.Current.Select(null, sqlCmd, out this.nodist2);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Query print data fail\r\n" + result.ToString());
                    return failResult;
                }
                #endregion
            }
            #endregion

            return Result.True;
        }

        /// <summary>
        /// 產生Excel
        /// </summary>
        /// <param name="report">report</param>
        /// <returns>bool</returns>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            if (this.operationCode.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            // string strXltName = Sci.Env.Cfg.XltPathDir + (this.display == "U" ? "\\IE_P03_Print_U.xltx" : "\\IE_P03_Print_Z.xltx");
            string strXltName = Sci.Env.Cfg.XltPathDir + "\\IE_P03_Print.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return false;
            }

            //excel.Visible = true;
            #region 第一頁
            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];
            string factory = MyUtility.Convert.GetString(this.masterData["FactoryID"]);
            worksheet.Cells[1, 1] = MyUtility.GetValue.Lookup(string.Format("select NameEN from factory WITH (NOLOCK) where id = '{0}'", factory));
            string style = MyUtility.Convert.GetString(this.masterData["styleID"]);
            string brand = MyUtility.Convert.GetString(this.masterData["brandID"]);
            string season = MyUtility.Convert.GetString(this.masterData["SeasonID"]);
            string combotype = MyUtility.Convert.GetString(this.masterData["combotype"]);
            worksheet.Cells[2, 2] = style + " " + season + " " + brand + " " + combotype;

            // excel 範圍別名宣告 公式使用
            excel.ActiveWorkbook.Names.Add("Operation", worksheet.Range["A6", "K" + this.operationCode.Rows.Count + 5]);

            // 填Operation
            int intRowsStart = 6;
            object[,] objArray = new object[1, 11];
            foreach (DataRow dr in this.operationCode.Rows)
            {
                objArray[0, 0] = dr["rn"];
                objArray[0, 1] = this.contentType == "A" ? MyUtility.Convert.GetString(dr["Annotation"]).Trim() : MyUtility.Convert.GetString(dr["DescEN"]).Trim();
                objArray[0, 2] = dr["MachineTypeID"];
                objArray[0, 3] = dr["MasterPlusGroup"];
                objArray[0, 4] = dr["Attachment"];
                objArray[0, 5] = dr["Template"];
                objArray[0, 6] = dr["GSD"];
                objArray[0, 7] = dr["Cycle"];
                objArray[0, 8] = dr["ThreadColor"];
                objArray[0, 9] = dr["OperationID"];
                objArray[0, 10] = $"=CONCATENATE(C{intRowsStart},\" \",E{intRowsStart},\" \",F{intRowsStart},\" \",I{intRowsStart})";
                worksheet.Range[string.Format("A{0}:K{0}", intRowsStart)].Value2 = objArray;
                intRowsStart++;
            }

            worksheet.Cells[intRowsStart, 1] = string.Format("=MAX($A$2:A{0})+1", intRowsStart - 1);

            // time
            worksheet.Cells[intRowsStart, 7] = string.Format("=SUM(G6:G{0})", intRowsStart - 1);
            worksheet.Range[string.Format("A5:J{0}", intRowsStart)].Borders.Weight = 1; // 1: 虛線, 2:實線, 3:粗體線
            worksheet.Range[string.Format("A5:J{0}", intRowsStart)].Borders.LineStyle = 1;

            intRowsStart++;
            worksheet.Cells[intRowsStart, 7] = string.Format("=F{0}/{1}", intRowsStart - 1, MyUtility.Convert.GetInt(this.masterData["CurrentOperators"]));
            worksheet.get_Range("F" + intRowsStart, "F" + intRowsStart).Font.Bold = true;

            intRowsStart++;
            worksheet.Cells[intRowsStart, 2] = "Picture";
            worksheet.get_Range("B" + intRowsStart, "B" + intRowsStart).Font.Bold = true;

            // 插圖 Picture1
            intRowsStart++;
            string destination_path = MyUtility.GetValue.Lookup("select StyleSketch from System WITH (NOLOCK) ", null);
            string picture12 = string.Format("select Picture1, Picture2 from Style where id = '{0}' and BrandID = '{1}' and SeasonID = '{2}'", style, brand, season);
            DataRow pdr;
            MyUtility.Check.Seek(picture12, out pdr);
            string filepath;
            Image img = null;
            double xltPixelRate;
            dynamic left;
            Microsoft.Office.Interop.Excel.Range cell = worksheet.Cells[intRowsStart, 2];
            left = cell.Left;
            if (!MyUtility.Check.Empty(pdr["Picture1"]))
            {
                filepath = destination_path + MyUtility.Convert.GetString(pdr["Picture1"]);
                if (File.Exists(filepath))
                {
                    img = Image.FromFile(filepath);
                    xltPixelRate = img.Width / 180 > 1 ? img.Width / 180 : 1;
                    worksheet.Shapes.AddPicture(filepath, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, left, cell.Top, (float)(img.Width / xltPixelRate), (float)(img.Height / xltPixelRate));
                    left += 220;
                }
            }

            // Picture2
            if (!MyUtility.Check.Empty(pdr["Picture2"]))
            {
                filepath = destination_path + MyUtility.Convert.GetString(pdr["Picture2"]);
                if (File.Exists(filepath))
                {
                    img = Image.FromFile(filepath);
                    xltPixelRate = img.Width / 180 > 1 ? img.Width / 180 : 1;
                    worksheet.Shapes.AddPicture(filepath, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, left, cell.Top, (float)(img.Width / xltPixelRate), (float)(img.Height / xltPixelRate));
                }
            }
            #endregion

            if (this.change)
            {
                this.ExcelMainData(excel.ActiveWorkbook.Worksheets[2], excel.ActiveWorkbook.Worksheets[3], excel.ActiveWorkbook.Worksheets[4], factory, style, this.nodist, this.count1);
                this.ExcelMainData(excel.ActiveWorkbook.Worksheets[5], excel.ActiveWorkbook.Worksheets[6], excel.ActiveWorkbook.Worksheets[7], factory, style, this.nodist2, this.count2);
            }
            else
            {
                decimal currentOperators = this.masterData["CurrentOperators"] == null ? 0 : Convert.ToDecimal(this.masterData["CurrentOperators"]);
                this.ExcelMainData(excel.ActiveWorkbook.Worksheets[2], excel.ActiveWorkbook.Worksheets[3], excel.ActiveWorkbook.Worksheets[4], factory, style, this.nodist, currentOperators);
                excel.ActiveWorkbook.Worksheets[5].Visible = Microsoft.Office.Interop.Excel.XlSheetVisibility.xlSheetHidden;
                excel.ActiveWorkbook.Worksheets[6].Visible = Microsoft.Office.Interop.Excel.XlSheetVisibility.xlSheetHidden;
                excel.ActiveWorkbook.Worksheets[7].Visible = Microsoft.Office.Interop.Excel.XlSheetVisibility.xlSheetHidden;
            }

            // 寫此行目的是要將Excel畫面上顯示Copy給取消
            excel.CutCopyMode = Microsoft.Office.Interop.Excel.XlCutCopyMode.xlCopy;

            #region Save & Show Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("IE_P03_Print");
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

        private void AddLineMappingFormula(Microsoft.Office.Interop.Excel.Worksheet worksheet, int rownum)
        {
            // Operation
            worksheet.Cells[rownum, 5] = $"=IF(ISNA(VLOOKUP(D{rownum},Operation,2,0)),\"\",VLOOKUP(D{rownum},Operation,2,0))";
            worksheet.Cells[rownum, 20] = $"=IF(ISNA(VLOOKUP(S{rownum},Operation,2,0)),\"\",VLOOKUP(S{rownum},Operation,2,0))";

            // GSD
            worksheet.Cells[rownum, 3] = $"=IF(ISNA(VLOOKUP(D{rownum},Operation,7,0)),\"\",VLOOKUP(D{rownum},Operation,7,0))";
            worksheet.Cells[rownum, 23] = $"=IF(ISNA(VLOOKUP(S{rownum},Operation,7,0)),\"\",VLOOKUP(S{rownum},Operation,7,0))";

            // TMS
            worksheet.Cells[rownum, 2] = $"=IF(ISNA(VLOOKUP(D{rownum},Operation,8,0)),\"\",VLOOKUP(D{rownum},Operation,8,0))";
            worksheet.Cells[rownum, 24] = $"=IF(ISNA(VLOOKUP(S{rownum},Operation,8,0)),\"\",VLOOKUP(S{rownum},Operation,8,0))";

            // ST/MC type
            worksheet.Cells[rownum, 10] = $"=IF(ISNA(VLOOKUP(D{rownum},Operation,11,0)),\"\",IF(VLOOKUP(D{rownum},Operation,11,0)=IF(ISNA(VLOOKUP(D{rownum - 1},Operation,11,0)),\"\",VLOOKUP(D{rownum - 1},Operation,11,0)),\"\",VLOOKUP(D{rownum},Operation,11,0)))";
            worksheet.Cells[rownum, 17] = $"=IF(ISNA(VLOOKUP(S{rownum},Operation,11,0)),\"\",IF(VLOOKUP(S{rownum},Operation,11,0)=IF(ISNA(VLOOKUP(S{rownum - 1},Operation,11,0)),\"\",VLOOKUP(S{rownum - 1},Operation,11,0)),\"\",VLOOKUP(S{rownum},Operation,11,0)))";

            // Machine Group
            worksheet.Cells[rownum, 12] = $"=IF(ISNA(VLOOKUP(D{rownum},Operation,4,0)),\"\",IF(VLOOKUP(D{rownum},Operation,4,0)=IF(ISNA(VLOOKUP(D{rownum - 1},Operation,4,0)),\"\",VLOOKUP(D{rownum - 1},Operation,4,0)),\"\",VLOOKUP(D{rownum},Operation,4,0)))";
            worksheet.Cells[rownum, 16] = $"=IF(ISNA(VLOOKUP(S{rownum},Operation,4,0)),\"\",IF(VLOOKUP(S{rownum},Operation,4,0)=IF(ISNA(VLOOKUP(S{rownum - 1},Operation,4,0)),\"\",VLOOKUP(S{rownum - 1},Operation,4,0)),\"\",VLOOKUP(S{rownum},Operation,4,0)))";

            // Attachment
            worksheet.Cells[rownum, 30] = $"=IF(OR(ISNA(VLOOKUP(D{rownum},Operation,5,0)),J{rownum}=\"\"),\"\",IF(VLOOKUP(D{rownum},Operation,5,0)=\"\",\"\",\"Attachment\"))";
            worksheet.Cells[rownum, 21] = $"=IF(OR(ISNA(VLOOKUP(S{rownum},Operation,5,0)),Q{rownum}=\"\"),\"\",IF(VLOOKUP(S{rownum},Operation,5,0)=\"\",\"\",\"Attachment\"))";

            // Template
            worksheet.Cells[rownum, 31] = $"=IF(OR(ISNA(VLOOKUP(D{rownum},Operation,6,0)),J{rownum}=\"\"),\"\",IF(VLOOKUP(D{rownum},Operation,6,0)=\"\",\"\",\"Template\"))";
            worksheet.Cells[rownum, 22] = $"=IF(OR(ISNA(VLOOKUP(S{rownum},Operation,6,0)),Q{rownum}=\"\"),\"\",IF(VLOOKUP(S{rownum},Operation,6,0)=\"\",\"\",\"Template\"))";

            // only Machine Type
            worksheet.Cells[rownum, 27] = $"=IF(ISNA(VLOOKUP(D{rownum},Operation,3,0)),\"\",IF(VLOOKUP(D{rownum},Operation,3,0)=IF(ISNA(VLOOKUP(D{rownum - 1},Operation,3,0)),\"\",VLOOKUP(D{rownum - 1},Operation,3,0)),\"\",VLOOKUP(D{rownum},Operation,3,0)))";
            worksheet.Cells[rownum, 28] = $"=IF(ISNA(VLOOKUP(S{rownum},Operation,3,0)),\"\",IF(VLOOKUP(S{rownum},Operation,3,0)=IF(ISNA(VLOOKUP(S{rownum - 1},Operation,3,0)),\"\",VLOOKUP(S{rownum - 1},Operation,3,0)),\"\",VLOOKUP(S{rownum},Operation,3,0)))";

        }

        private void ExcelMainData(Microsoft.Office.Interop.Excel.Worksheet worksheet, Microsoft.Office.Interop.Excel.Worksheet cycleTimeSheet, Microsoft.Office.Interop.Excel.Worksheet gcTimeSheet, string factory, string style, DataTable nodist, decimal currentOperators)
        {
            #region 第二頁

            #region 固定資料

            // 左上表頭資料
            worksheet.Cells[1, 6] = factory;
            worksheet.Cells[5, 6] = MyUtility.Convert.GetString(this.masterData["SewingLineID"]);
            worksheet.Cells[7, 6] = style;
            worksheet.Cells[9, 6] = this.styleCPU;

            // 右下簽名位置
            worksheet.Cells[29, 20] = DateTime.Now.ToString("d");
            worksheet.Cells[32, 20] = Sci.Env.User.UserName;

            // 左下表頭資料
            worksheet.Cells[56, 4] = this.masterData["Version"];
            worksheet.Cells[58, 4] = this.masterData["Workhour"];
            worksheet.Cells[60, 4] = currentOperators;
            #endregion
            string[] allMachine = this.operationCode.AsEnumerable().Where(s => !MyUtility.Check.Empty(s["MachineTypeID"])).Select(s => s["MachineTypeID"].ToString()).Distinct().ToArray();
            #region MACHINE INVENTORY
            if (allMachine.Length > 3)
            {
                Microsoft.Office.Interop.Excel.Range rng = worksheet.get_Range("A52:A52").EntireRow; // 選取要被複製的資料
                for (int i = 3; i < allMachine.Length; i++)
                {
                    Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range("A52", Type.Missing).EntireRow; // 選擇要被貼上的位置
                    rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown, rng.Copy(Type.Missing)); // 貼上
                }
            }

            int sumrow = 0;
            foreach (string item in allMachine)
            {
                worksheet.Cells[51 + sumrow, 1] = item;
                sumrow++;
            }
            #endregion

            decimal chartDataEndRow = currentOperators + 1;
            #region 新增長條圖 2 GCtime chart

            // 新增長條圖
            Microsoft.Office.Interop.Excel.Worksheet chartData2 = gcTimeSheet;
            Microsoft.Office.Interop.Excel.Range chartRange2;
            object misValue2 = System.Reflection.Missing.Value;
            Microsoft.Office.Interop.Excel.ChartObjects xlsCharts2 = (Microsoft.Office.Interop.Excel.ChartObjects)worksheet.ChartObjects(Type.Missing);
            Microsoft.Office.Interop.Excel.ChartObject myChart2 = (Microsoft.Office.Interop.Excel.ChartObject)xlsCharts2.Add(378, 1082, 1000, 350);
            Microsoft.Office.Interop.Excel.Chart chartPage2 = myChart2.Chart;
            chartRange2 = chartData2.get_Range("B1", string.Format("B{0}", MyUtility.Convert.GetString(chartDataEndRow)));
            chartPage2.SetSourceData(chartRange2, misValue2);

            chartPage2.ChartType = Microsoft.Office.Interop.Excel.XlChartType.xlColumnClustered;

            // 新增折線圖
            Microsoft.Office.Interop.Excel.SeriesCollection seriesCollection2 = chartPage2.SeriesCollection();
            Microsoft.Office.Interop.Excel.Series series2 = seriesCollection2.NewSeries();
            series2.Values = chartData2.get_Range("C2", string.Format("C{0}", MyUtility.Convert.GetString(chartDataEndRow)));
            series2.XValues = chartData2.get_Range("A2", string.Format("A{0}", MyUtility.Convert.GetString(chartDataEndRow)));
            series2.Name = "Total Cycle Time";

            // 折線圖的資料標籤不顯示
            series2.ApplyDataLabels(Microsoft.Office.Interop.Excel.XlDataLabelsType.xlDataLabelsShowNone, false, false);

            // 隱藏Sheet
            chartData2.Visible = Microsoft.Office.Interop.Excel.XlSheetVisibility.xlSheetHidden;
            #endregion

            #region 新增長條圖 1 CycleTime chart

            // 新增長條圖
            Microsoft.Office.Interop.Excel.Worksheet chartData = cycleTimeSheet;
            Microsoft.Office.Interop.Excel.Range chartRange;
            object misValue = System.Reflection.Missing.Value;
            Microsoft.Office.Interop.Excel.ChartObjects xlsCharts = (Microsoft.Office.Interop.Excel.ChartObjects)worksheet.ChartObjects(Type.Missing);
            Microsoft.Office.Interop.Excel.ChartObject myChart = (Microsoft.Office.Interop.Excel.ChartObject)xlsCharts.Add(378, 718.5, 1000, 350);
            Microsoft.Office.Interop.Excel.Chart chartPage = myChart.Chart;
            chartRange = chartData.get_Range("B1", string.Format("B{0}", MyUtility.Convert.GetString(chartDataEndRow)));
            chartPage.SetSourceData(chartRange, misValue);

            chartPage.ChartType = Microsoft.Office.Interop.Excel.XlChartType.xlColumnClustered;

            // 新增折線圖
            Microsoft.Office.Interop.Excel.SeriesCollection seriesCollection = chartPage.SeriesCollection();
            Microsoft.Office.Interop.Excel.Series series1 = seriesCollection.NewSeries();
            series1.Values = chartData.get_Range("C2", string.Format("C{0}", MyUtility.Convert.GetString(chartDataEndRow)));
            series1.XValues = chartData.get_Range("A2", string.Format("A{0}", MyUtility.Convert.GetString(chartDataEndRow)));
            series1.Name = "Takt time";
            series1.ChartType = Microsoft.Office.Interop.Excel.XlChartType.xlLine;

            // 新增折線圖
            Microsoft.Office.Interop.Excel.SeriesCollection seriesCollection_actTime = chartPage.SeriesCollection();
            Microsoft.Office.Interop.Excel.Series series1_actTime = seriesCollection_actTime.NewSeries();
            series1_actTime.Values = chartData.get_Range("D2", string.Format("D{0}", MyUtility.Convert.GetString(chartDataEndRow)));
            series1_actTime.XValues = chartData.get_Range("A2", string.Format("A{0}", MyUtility.Convert.GetString(chartDataEndRow)));
            series1_actTime.Name = "Act Cycle Time(average)";
            series1_actTime.ChartType = Microsoft.Office.Interop.Excel.XlChartType.xlLine;

            // 更改圖表版面配置 && 填入圖表標題 & 座標軸標題
            chartPage.ApplyLayout(9);
            chartPage.ChartTitle.Select();
            chartPage.ChartTitle.Text = "Line Balancing Graph";
            Microsoft.Office.Interop.Excel.Axis z = (Microsoft.Office.Interop.Excel.Axis)chartPage.Axes(Microsoft.Office.Interop.Excel.XlAxisType.xlValue, Microsoft.Office.Interop.Excel.XlAxisGroup.xlPrimary);
            z.AxisTitle.Text = "Act Cycle Time (in secs)";
            z = (Microsoft.Office.Interop.Excel.Axis)chartPage.Axes(Microsoft.Office.Interop.Excel.XlAxisType.xlCategory, Microsoft.Office.Interop.Excel.XlAxisGroup.xlPrimary);
            z.AxisTitle.Text = "Operator No.";

            // 新增資料標籤
            // chartPage.ApplyDataLabels(Microsoft.Office.Interop.Excel.XlDataLabelsType.xlDataLabelsShowValue, false, true);

            // 折線圖的資料標籤不顯示
            series1.ApplyDataLabels(Microsoft.Office.Interop.Excel.XlDataLabelsType.xlDataLabelsShowNone, false, false);

            // 隱藏Sheet
            chartData.Visible = Microsoft.Office.Interop.Excel.XlSheetVisibility.xlSheetHidden;
            #endregion

            #region MACHINE
            decimal allct = Math.Ceiling((decimal)allMachine.Length / 3);
            Microsoft.Office.Interop.Excel.Range rngToCopy = worksheet.get_Range("A42:A42").EntireRow; // 選取要被複製的資料
            for (int i = 0; i < allct - 5; i++)
            {
                Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range("A42", Type.Missing).EntireRow; // 選擇要被貼上的位置
                rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown, rngToCopy.Copy(Type.Missing)); // 貼上
            }

            int surow = 0;
            int sucol = 0;
            foreach (string item in allMachine)
            {
                worksheet.Cells[38 + surow, 5 + sucol] = item;
                surow++;
                if (allct == surow)
                {
                    surow = 0;
                    sucol += 2;
                }
            }
            #endregion

            #region Machine Type
            string[] noPPA = this.operationCode.AsEnumerable().Where(s => s["ISPPA"].Equals(true)).Select(s => s["rn"].ToString()).ToArray();
            if (noPPA.Length > 10)
            {
                rngToCopy = worksheet.get_Range("A25:A25").EntireRow; // 選取要被複製的資料
                for (int i = 10; i < noPPA.Length; i++)
                {
                    Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range("A25", Type.Missing).EntireRow; // 選擇要被貼上的位置
                    rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown, rngToCopy.Copy(Type.Missing)); // 貼上
                }
            }

            int idxppa = 0;
            foreach (string item in noPPA)
            {
                worksheet.Cells[25 + idxppa, 1] = $"=IF(ISNA(VLOOKUP(D{25 + idxppa},Operation,10,0)),\"\",VLOOKUP(D{25 + idxppa},Operation,10,0))";
                worksheet.Cells[25 + idxppa, 5] = $"=IF(ISNA(VLOOKUP(D{25 + idxppa},Operation,2,0)),\"\",VLOOKUP(D{25 + idxppa},Operation,2,0))";
                worksheet.Cells[25 + idxppa, 10] = $"=IF(ISNA(VLOOKUP(D{25 + idxppa},Operation,3,0)),\"\",VLOOKUP(D{25 + idxppa},Operation,3,0))";
                worksheet.Cells[25 + idxppa, 4] = item;
                idxppa++;
            }
            #endregion

            #region 預設站數為2站，當超過2站就要新增
            decimal no_count = MyUtility.Convert.GetDecimal(nodist.Rows.Count);
            int ttlLineRowCnt = MyUtility.Convert.GetInt(Math.Ceiling(no_count / 2));
            if (no_count > 2)
            {
                rngToCopy = worksheet.get_Range("A17:A21").EntireRow; // 選取要被複製的資料
                for (int j = 1; j < ttlLineRowCnt; j++)
                {
                    Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range("A17", Type.Missing).EntireRow; // 選擇要被貼上的位置
                    rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown, rngToCopy.Copy(Type.Missing)); // 貼上
                }
            }
            #endregion

            int norow = 17 + (ttlLineRowCnt * 5) - 5; // No格子上的位置Excel Y軸
            int nocolumn = 9;

            // 計錄各站點公式放入GCtime sheet資料來源
            List<GCTimeChartData> list_GCTimeChartData = new List<GCTimeChartData>();
            List<CycleTimeChart> list_CycleTimeChart = new List<CycleTimeChart>();

            #region U_Left字型列印
            if (this.display == "U_Left")
            {
                int maxct = 3;
                int di = nodist.Rows.Count;
                int addct = 0;
                bool flag = true;
                decimal dd = Math.Ceiling((decimal)di / 2);
                List<int> listMax_ct = new List<int>();
                for (int i = 0; i < dd; i++)
                {
                    int a = MyUtility.Convert.GetInt(nodist.Rows[i]["ct"]);
                    int d = 0;
                    if (di % 2 == 1 && flag)
                    {
                        flag = false;
                    }
                    else
                    {
                        if (di % 2 == 1)
                        {
                            d = MyUtility.Convert.GetInt(nodist.Rows[di - i]["ct"]);
                        }
                        else
                        {
                            d = MyUtility.Convert.GetInt(nodist.Rows[di - 1 - i]["ct"]);
                        }
                    }

                    maxct = a > d ? a : d;
                    maxct = maxct > 3 ? maxct : 3;
                    listMax_ct.Add(maxct);
                    Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(norow + 3)), Type.Missing).EntireRow;
                    for (int k = 3; k < maxct; k++)
                    {
                        rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                        worksheet.get_Range(string.Format("E{0}:I{0}", MyUtility.Convert.GetString(norow + k))).Merge(false); // 合併儲存格
                        worksheet.get_Range(string.Format("T{0}:V{0}", MyUtility.Convert.GetString(norow + k))).Merge(false); // 合併儲存格

                        addct++;
                    }

                    // 將公式填入對應的格子中
                    for (int q = 1; q <= maxct; q++)
                    {
                        this.AddLineMappingFormula(worksheet, norow + q + 1);
                    }

                    norow = norow - 5;
                    maxct = 3;
                }

                bool leftDirection = true;

                // excel 範圍別名宣告 公式使用 for MACHINE INVENTORY計算用
                string endRow = (16 + (ttlLineRowCnt * 5) + addct).ToString();
                worksheet.Names.Add("MachineINV1", worksheet.Range["AA17", "AA" + endRow], Type.Missing);
                worksheet.Names.Add("MachineINV2", worksheet.Range["AB17", "AB" + endRow], Type.Missing);
                worksheet.Names.Add("MachineAttachmentTemplateL", worksheet.Range["AC19", "AD" + endRow], Type.Missing);
                worksheet.Names.Add("MachineAttachmentTemplateR", worksheet.Range["U17", "V" + endRow], Type.Missing);
                worksheet.Names.Add("TtlTMS", $"=ROUND((SUM('{worksheet.Name}'!$B$17:$B${endRow})+SUM('{worksheet.Name}'!$X$17:$X${endRow}))/2,0)", Type.Missing);
                worksheet.Names.Add("TtlGSD", $"=ROUND((SUM('{worksheet.Name}'!$C$17:$C${endRow})+SUM('{worksheet.Name}'!$W$17:$W${endRow}))/2,0)", Type.Missing);
                worksheet.Names.Add("MaxTMS", $"=Max('{worksheet.Name}'!$B$17:$B${endRow},'{worksheet.Name}'!$X$17:$X${endRow})", Type.Missing);
                int m = 0;

                norow = MyUtility.Convert.GetInt(endRow) - (listMax_ct[0] + 1);
                foreach (DataRow nodr in nodist.Rows)
                {
                    list_GCTimeChartData.Add(new GCTimeChartData()
                    {
                        OperatorNo = MyUtility.Convert.GetString(nodr["No"]),
                        TotalGSDFormula = $"='{worksheet.Name}'!{(leftDirection ? "C" : "W")}{norow}",
                        TotalCycleFormula = $"='{worksheet.Name}'!{(leftDirection ? "B" : "X")}{norow}"
                    });

                    list_CycleTimeChart.Add(new CycleTimeChart()
                    {
                        OperatorNo = MyUtility.Convert.GetString(nodr["No"]),
                        ActCycleFormula = $"='{worksheet.Name}'!{(leftDirection ? "K" : "R")}{norow}",
                        ActCycleTime = MyUtility.Convert.GetString(nodr["ActCycleTime(average)"]),
                        TaktFormula = $"=E1"
                    });

                    if (leftDirection)
                    {
                        nocolumn = 10;
                        worksheet.Cells[norow, nocolumn] = MyUtility.Convert.GetString(nodr["No"]);
                        worksheet.Cells[norow, nocolumn + 1] = MyUtility.Convert.GetString(nodr["ActCycle"]);
                        DataRow[] nodrs = this.operationCode.Select(string.Format("no = '{0}'", MyUtility.Convert.GetString(nodr["No"])));
                        int ridx = 2;
                        string machinetype = string.Empty;
                        string machinetypeL = string.Empty;
                        int row = norow + ridx;
                        foreach (DataRow item in nodrs)
                        {
                            worksheet.Cells[norow + ridx, nocolumn - 6] = item["rn"].ToString();

                            ridx++;
                        }

                        m++;
                        if (m == dd)
                        {
                            leftDirection = false;
                            m--;
                            continue;
                        }

                        norow = norow - 5 - (listMax_ct[m] - 3);
                    }
                    else
                    {
                        nocolumn = 17;
                        worksheet.Cells[norow, nocolumn] = MyUtility.Convert.GetString(nodr["No"]);
                        worksheet.Cells[norow, nocolumn + 1] = MyUtility.Convert.GetString(nodr["ActCycle"]);
                        DataRow[] nodrs = this.operationCode.Select(string.Format("no = '{0}'", MyUtility.Convert.GetString(nodr["No"])));
                        int ridx = 2;
                        string machinetype = string.Empty;
                        string machinetypeL = string.Empty;
                        int row = norow + ridx;
                        foreach (DataRow item in nodrs)
                        {
                            worksheet.Cells[norow + ridx, nocolumn + 2] = item["rn"].ToString();

                            ridx++;
                        }

                        norow = norow + 5 + (listMax_ct[m] - 3);
                        m--;
                    }
                }
            }
            #endregion

            #region U_Right字型列印
            if (this.display == "U_Right")
            {
                int maxct = 3;
                int di = nodist.Rows.Count;
                int addct = 0;
                bool flag = true;
                decimal dd = Math.Ceiling((decimal)di / 2);
                List<int> listMax_ct = new List<int>();
                for (int i = 0; i < dd; i++)
                {
                    int a = MyUtility.Convert.GetInt(nodist.Rows[i]["ct"]);
                    int d = 0;
                    if (di % 2 == 1 && flag)
                    {
                        flag = false;
                    }
                    else
                    {
                        if (di % 2 == 1)
                        {
                            d = MyUtility.Convert.GetInt(nodist.Rows[di - i]["ct"]);
                        }
                        else
                        {
                            d = MyUtility.Convert.GetInt(nodist.Rows[di - 1 - i]["ct"]);
                        }
                    }

                    maxct = a > d ? a : d;
                    maxct = maxct > 3 ? maxct : 3;
                    listMax_ct.Add(maxct);
                    Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(norow + 3)), Type.Missing).EntireRow;
                    for (int k = 3; k < maxct; k++)
                    {
                        rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                        worksheet.get_Range(string.Format("E{0}:I{0}", MyUtility.Convert.GetString(norow + k))).Merge(false); // 合併儲存格
                        worksheet.get_Range(string.Format("T{0}:V{0}", MyUtility.Convert.GetString(norow + k))).Merge(false); // 合併儲存格
                        addct++;
                    }

                    // 將公式填入對應的格子中
                    for (int q = 1; q <= maxct; q++)
                    {
                        this.AddLineMappingFormula(worksheet, norow + q + 1);
                    }

                    norow = norow - 5;
                    maxct = 3;
                }

                bool rightDirection = true;

                // excel 範圍別名宣告 公式使用 for MACHINE INVENTORY計算用
                string endRow = (16 + (ttlLineRowCnt * 5) + addct).ToString();
                worksheet.Names.Add("MachineINV1", worksheet.Range["AA17", "AA" + endRow], Type.Missing);
                worksheet.Names.Add("MachineINV2", worksheet.Range["AB17", "AB" + endRow], Type.Missing);
                worksheet.Names.Add("MachineAttachmentTemplateL", worksheet.Range["AC19", "AD" + endRow], Type.Missing);
                worksheet.Names.Add("MachineAttachmentTemplateR", worksheet.Range["U17", "V" + endRow], Type.Missing);
                worksheet.Names.Add("TtlTMS", $"=ROUND((SUM('{worksheet.Name}'!$B$17:$B${endRow})+SUM('{worksheet.Name}'!$X$17:$X${endRow}))/2,0)", Type.Missing);
                worksheet.Names.Add("TtlGSD", $"=ROUND((SUM('{worksheet.Name}'!$C$17:$C${endRow})+SUM('{worksheet.Name}'!$W$17:$W${endRow}))/2,0)", Type.Missing);
                worksheet.Names.Add("MaxTMS", $"=Max('{worksheet.Name}'!$B$17:$B${endRow},'{worksheet.Name}'!$X$17:$X${endRow})", Type.Missing);
                int m = 0;

                norow = MyUtility.Convert.GetInt(endRow) - (listMax_ct[0] + 1);
                foreach (DataRow nodr in nodist.Rows)
                {
                    list_GCTimeChartData.Add(new GCTimeChartData()
                    {
                        OperatorNo = MyUtility.Convert.GetString(nodr["No"]),
                        TotalGSDFormula = $"='{worksheet.Name}'!{(rightDirection ? "W" : "C")}{norow}",
                        TotalCycleFormula = $"='{worksheet.Name}'!{(rightDirection ? "X" : "B")}{norow}"
                    });

                    list_CycleTimeChart.Add(new CycleTimeChart()
                    {
                        OperatorNo = MyUtility.Convert.GetString(nodr["No"]),
                        ActCycleFormula = $"='{worksheet.Name}'!{(rightDirection ? "R" : "K")}{norow}",
                        ActCycleTime = MyUtility.Convert.GetString(nodr["ActCycleTime(average)"]),
                        TaktFormula = $"=E1"
                    });

                    if (rightDirection)
                    {
                        nocolumn = 17;
                        worksheet.Cells[norow, nocolumn] = MyUtility.Convert.GetString(nodr["No"]);
                        worksheet.Cells[norow, nocolumn + 1] = MyUtility.Convert.GetString(nodr["ActCycle"]);
                        DataRow[] nodrs = this.operationCode.Select(string.Format("no = '{0}'", MyUtility.Convert.GetString(nodr["No"])));
                        int ridx = 2;
                        string machinetype = string.Empty;
                        string machinetypeL = string.Empty;
                        int row = norow + ridx;
                        foreach (DataRow item in nodrs)
                        {
                            worksheet.Cells[norow + ridx, nocolumn + 2] = item["rn"].ToString();

                            ridx++;
                        }

                        m++;
                        if (m == dd)
                        {
                            rightDirection = false;
                            m--;
                            continue;
                        }

                        norow = norow - 5 - (listMax_ct[m] - 3);
                    }
                    else
                    {
                        nocolumn = 10;
                        worksheet.Cells[norow, nocolumn] = MyUtility.Convert.GetString(nodr["No"]);
                        worksheet.Cells[norow, nocolumn + 1] = MyUtility.Convert.GetString(nodr["ActCycle"]);
                        DataRow[] nodrs = this.operationCode.Select(string.Format("no = '{0}'", MyUtility.Convert.GetString(nodr["No"])));
                        int ridx = 2;
                        string machinetype = string.Empty;
                        string machinetypeL = string.Empty;
                        int row = norow + ridx;
                        foreach (DataRow item in nodrs)
                        {
                            worksheet.Cells[norow + ridx, nocolumn - 6] = item["rn"].ToString();

                            ridx++;
                        }

                        norow = norow + 5 + (listMax_ct[m] - 3);
                        m--;
                    }
                }
            }
            #endregion
            #region Z字型列印
            if (this.display == "Z")
            {
                int maxct = 3;
                int ct = 0;
                int addct = 0;
                int indx = 1;
                List<int> listMax_ct = new List<int>();
                for (int l = 0; l < nodist.Rows.Count + 1; l++)
                {
                    if (l < nodist.Rows.Count)
                    {
                        maxct = MyUtility.Convert.GetInt(nodist.Rows[l]["ct"]) > maxct ? MyUtility.Convert.GetInt(nodist.Rows[l]["ct"]) : maxct;
                    }

                    ct++;
                    if (ct == 2)
                    {
                        listMax_ct.Add(maxct);
                        Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(norow + 3)), Type.Missing).EntireRow;
                        for (int i = 3; i < maxct; i++)
                        {
                            rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                            worksheet.get_Range(string.Format("E{0}:I{0}", MyUtility.Convert.GetString(norow + i))).Merge(false); // 合併儲存格
                            worksheet.get_Range(string.Format("T{0}:V{0}", MyUtility.Convert.GetString(norow + i))).Merge(false); // 合併儲存格

                            addct++;
                        }

                        // 將公式填入對應的格子中
                        for (int q = 1; q <= maxct; q++)
                        {
                            this.AddLineMappingFormula(worksheet, norow + q + 1);
                        }

                        norow = norow - 5;
                        ct = 0;
                        maxct = 3;
                    }
                }

                // excel 範圍別名宣告 公式使用 for MACHINE INVENTORY計算用
                string endRow = (16 + (ttlLineRowCnt * 5) + addct).ToString();
                worksheet.Names.Add("MachineINV1", worksheet.Range["AA17", "AA" + endRow], Type.Missing);
                worksheet.Names.Add("MachineINV2", worksheet.Range["AB17", "AB" + endRow], Type.Missing);
                worksheet.Names.Add("MachineAttachmentTemplateL", worksheet.Range["AC19", "AD" + endRow], Type.Missing);
                worksheet.Names.Add("MachineAttachmentTemplateR", worksheet.Range["U17", "V" + endRow], Type.Missing);
                worksheet.Names.Add("TtlTMS", $"=ROUND((SUM('{worksheet.Name}'!$B$17:$B${endRow})+SUM('{worksheet.Name}'!$X$17:$X${endRow}))/2,0)", Type.Missing);
                worksheet.Names.Add("TtlGSD", $"=ROUND((SUM('{worksheet.Name}'!$C$17:$C${endRow})+SUM('{worksheet.Name}'!$W$17:$W${endRow}))/2,0)", Type.Missing);
                worksheet.Names.Add("MaxTMS", $"=Max('{worksheet.Name}'!$B$17:$B${endRow},'{worksheet.Name}'!$X$17:$X${endRow})", Type.Missing);

                int leftright_count = 2;
                bool leftDirection = true;
                indx = 0;
                norow = MyUtility.Convert.GetInt(endRow) + 1;
                foreach (DataRow nodr in nodist.Rows)
                {
                    if (leftright_count == 2)
                    {
                        norow = norow - (listMax_ct[indx] + 2);
                        indx++;
                    }

                    list_GCTimeChartData.Add(new GCTimeChartData()
                    {
                        OperatorNo = MyUtility.Convert.GetString(nodr["No"]),
                        TotalGSDFormula = $"='{worksheet.Name}'!{(leftDirection ? "C" : "S")}{norow}",
                        TotalCycleFormula = $"='{worksheet.Name}'!{(leftDirection ? "B" : "X")}{norow}"
                    });

                    list_CycleTimeChart.Add(new CycleTimeChart()
                    {
                        OperatorNo = MyUtility.Convert.GetString(nodr["No"]),
                        ActCycleFormula = $"='{worksheet.Name}'!{(leftDirection ? "K" : "R")}{norow}",
                        ActCycleTime = MyUtility.Convert.GetString(nodr["ActCycleTime(average)"]),
                        TaktFormula = $"=E1"
                    });

                    if (leftDirection)
                    {
                        nocolumn = 10;
                        worksheet.Cells[norow, nocolumn] = MyUtility.Convert.GetString(nodr["No"]);
                        worksheet.Cells[norow, nocolumn + 1] = MyUtility.Convert.GetString(nodr["ActCycle"]);
                        DataRow[] nodrs = this.operationCode.Select(string.Format("no = '{0}'", MyUtility.Convert.GetString(nodr["No"])));
                        int ridx = 2;
                        string machinetype = string.Empty;
                        string machinetypeL = string.Empty;
                        int row = norow + ridx;
                        foreach (DataRow item in nodrs)
                        {
                            worksheet.Cells[norow + ridx, nocolumn - 6] = item["rn"].ToString();

                            ridx++;
                        }

                        leftright_count++;
                        if (leftright_count > 2)
                        {
                            leftright_count = 1;
                            leftDirection = false;
                        }
                    }
                    else
                    {
                        nocolumn = 17;
                        worksheet.Cells[norow, nocolumn] = MyUtility.Convert.GetString(nodr["No"]);
                        worksheet.Cells[norow, nocolumn + 1] = MyUtility.Convert.GetString(nodr["ActCycle"]);
                        DataRow[] nodrs = this.operationCode.Select(string.Format("no = '{0}'", MyUtility.Convert.GetString(nodr["No"])));
                        int ridx = 2;
                        string machinetype = string.Empty;
                        string machinetypeL = string.Empty;
                        int row = norow + ridx;
                        foreach (DataRow item in nodrs)
                        {
                            worksheet.Cells[norow + ridx, nocolumn + 2] = item["rn"].ToString();

                            ridx++;
                        }

                        leftright_count++;
                        if (leftright_count > 2)
                        {
                            leftright_count = 1;
                            leftDirection = true;
                        }
                    }

                   
                }
            }
            #endregion

            #region 長條圖資料

            // 填act Cycle Time
            worksheet = cycleTimeSheet;
            int intRowsStart = 2;
            object[,] objArray = new object[1, 4];
            foreach (CycleTimeChart dr in list_CycleTimeChart)
            {
                objArray[0, 0] = dr.OperatorNo;
                objArray[0, 1] = dr.ActCycleFormula;
                objArray[0, 2] = dr.TaktFormula;
                objArray[0, 3] = dr.ActCycleTime;
                worksheet.Range[string.Format("A{0}:D{0}", intRowsStart)].Value2 = objArray;
                intRowsStart++;
            }

            // 填 TotalGSD
            worksheet = gcTimeSheet;
            intRowsStart = 2;
            objArray = new object[1, 3];
            foreach (GCTimeChartData dr in list_GCTimeChartData)
            {
                objArray[0, 0] = dr.OperatorNo;
                objArray[0, 1] = dr.TotalGSDFormula;
                objArray[0, 2] = dr.TotalCycleFormula;
                worksheet.Range[string.Format("A{0}:C{0}", intRowsStart)].Value2 = objArray;
                intRowsStart++;
            }

            #endregion

            #endregion
        }

        private class GCTimeChartData
        {
            private string operatorNo;
            private string totalGSDFormula;
            private string totalCycleFormula;

            public string OperatorNo
            {
                get
                {
                    return this.operatorNo;
                }

                set
                {
                    this.operatorNo = value;
                }
            }

            public string TotalGSDFormula
            {
                get
                {
                    return this.totalGSDFormula;
                }

                set
                {
                    this.totalGSDFormula = value;
                }
            }

            public string TotalCycleFormula
            {
                get
                {
                    return this.totalCycleFormula;
                }

                set
                {
                    this.totalCycleFormula = value;
                }
            }
        }

        private class CycleTimeChart
        {
            private string operatorNo;
            private string actCycleFormula;
            private string actCycleTime;
            private string taktFormula;

            public string OperatorNo
            {
                get
                {
                    return this.operatorNo;
                }

                set
                {
                    this.operatorNo = value;
                }
            }

            public string ActCycleFormula
            {
                get
                {
                    return this.actCycleFormula;
                }

                set
                {
                    this.actCycleFormula = value;
                }
            }

            public string ActCycleTime
            {
                get
                {
                    return this.actCycleTime;
                }

                set
                {
                    this.actCycleTime = value;
                }
            }

            public string TaktFormula
            {
                get
                {
                    return this.taktFormula;
                }

                set
                {
                    this.taktFormula = value;
                }
            }
        }
    }
}
