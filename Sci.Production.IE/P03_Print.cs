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
        private DataTable actCycleTime;
        private DataTable operationCode;
        private DataTable noda;
        private DataTable mt;
        private DataTable summt;
        private DataTable nodist;
        private DataTable noppa;
        private DataTable atct;
        private DataTable GCTime;
        private decimal styleCPU;

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
            this.radioU.Checked = true;
            this.radioDescription.Checked = true;
        }

        /// <summary>
        /// ValidateInput 驗證輸入條件
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ValidateInput()
        {
            this.display = this.radioU.Checked ? "U" : "Z";
            this.contentType = this.radioDescription.Checked ? "D" : "A";
            return base.ValidateInput();
        }

        /// <summary>
        /// OnAsyncDataLoad
        /// </summary>
        /// <param name="e">e</param>
        /// <returns>DualResult</returns>
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            string sqlCmd;

            #region 第一頁
            sqlCmd = string.Format(
                @"
select a.*,isnull(o.DescEN,'') as DescEN,rn = ROW_NUMBER() over(order by a.GroupKey)
from (select GroupKey,OperationID,Annotation,max(GSD) as GSD,MachineTypeID
		,AT = case 
		  when Attachment is not null and Template is not null then Attachment+','+Template
		  when Attachment is not null and Template is null then Attachment
		  when Attachment is null and Template is not null then Template 
		  else ''end
	  from LineMapping_Detail WITH (NOLOCK) 
	  where ID = {0} and (IsPPa = 0 or IsPPa is null)
	  group by GroupKey,OperationID,Annotation,MachineTypeID,Attachment,Template) a
left join Operation o WITH (NOLOCK) on o.ID = a.OperationID
order by a.GroupKey", MyUtility.Convert.GetString(this.masterData["ID"]));
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out this.operationCode);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query operation code data fail\r\n" + result.ToString());
                return failResult;
            }
            #endregion

            #region 第二頁
            sqlCmd = string.Format(
                @"select No,CT = COUNT(1)
from LineMapping_Detail ld WITH (NOLOCK) 
where ld.ID = {0} and (IsPPa = 0 or IsPPa is null)
GROUP BY NO", MyUtility.Convert.GetString(this.masterData["ID"]));
            result = DBProxy.Current.Select(null, sqlCmd, out this.nodist);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query print data fail\r\n" + result.ToString());
                return failResult;
            }

            sqlCmd = string.Format(
                @"select ld.No,ld.Cycle,ld.GSD,ld.MachineTypeID,e2.Name,Annotation,o.DescEN
from LineMapping_Detail ld WITH (NOLOCK) 
left join Operation o WITH (NOLOCK) on o.ID = ld.OperationID
outer apply(
	select Name = stuff((
		select distinct concat(',',Name)
		from Employee e WITH (NOLOCK) 
		where e.ID = ld.EmployeeID
		for xml path('')
	),1,1,'')
)e2
where ld.ID = {0} and (IsPPa = 0 or IsPPa is null)
order by ld.No,ld.MachineTypeID,ld.GroupKey", MyUtility.Convert.GetString(this.masterData["ID"]));
            result = DBProxy.Current.Select(null, sqlCmd, out this.noda);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query print data fail\r\n" + result.ToString());
                return failResult;
            }

            sqlCmd = string.Format(
                @"select ld.No,MachineTypeID = rtrim(MachineTypeID+' '+isnull(Attachment,'')+' '+isnull(Template,'')+' '+isnull(ThreadColor,''))
from LineMapping_Detail ld WITH (NOLOCK) 
where ld.ID = {0} and (IsPPa = 0 or IsPPa is null)
GROUP BY ld.No,ld.MachineTypeID,isnull(ThreadColor,''),isnull(Attachment,''),isnull(Template,'')", MyUtility.Convert.GetString(this.masterData["ID"]));
            result = DBProxy.Current.Select(null, sqlCmd, out this.mt);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query print data fail\r\n" + result.ToString());
                return failResult;
            }

            sqlCmd = string.Format(
                @"select MachineTypeID,sumct = sum(ct)
from(
	SELECT No,MachineTypeID,ct=count(1)
	FROM(
		select ld.No,ld.MachineTypeID,ThreadColor=isnull(ThreadColor,''),Attachment=isnull(Attachment,''),Template=isnull(Template,'')
		from LineMapping_Detail ld WITH (NOLOCK) 
		where ld.ID = {0} and (IsPPa = 0 or IsPPa is null)
		GROUP BY ld.No,ld.MachineTypeID,isnull(ThreadColor,''),isnull(Attachment,''),isnull(Template,'')
	)A
	group by No,MachineTypeID
)x
group by MachineTypeID", MyUtility.Convert.GetString(this.masterData["ID"]));
            result = DBProxy.Current.Select(null, sqlCmd, out this.summt);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query print data fail\r\n" + result.ToString());
                return failResult;
            }

            sqlCmd = string.Format(
                @"select a_ct = count(a.no),t_ct = count(t.no)
from(
	select Attachment=isnull(Attachment,''),Template=isnull(Template,'')
	from LineMapping_Detail ld WITH (NOLOCK) 
	where ld.ID = {0} and (IsPPa = 0 or IsPPa is null) and (isnull(Attachment,'') !='' or isnull(Template,'') !='')
	GROUP BY ld.No,ld.MachineTypeID,isnull(ThreadColor,''),isnull(Attachment,''),isnull(Template,'')
)x
outer apply(select * from SplitString(Attachment,','))a
outer apply(select * from SplitString(Template,','))t", MyUtility.Convert.GetString(this.masterData["ID"]));
            result = DBProxy.Current.Select(null, sqlCmd, out this.atct);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query print data fail\r\n" + result.ToString());
                return failResult;
            }

            // Machine Type
            sqlCmd = string.Format(
                @"select ld.OperationID,ld.MachineTypeID,Annotation,DescEN=isnull(o.DescEN,Annotation)
from LineMapping_Detail ld WITH (NOLOCK)
left join Operation o WITH (NOLOCK) on o.ID = ld.OperationID
where ld.ID = {0} and IsPPa = 1
order by ld.No,ld.GroupKey", MyUtility.Convert.GetString(this.masterData["ID"]));
            result = DBProxy.Current.Select(null, sqlCmd, out this.noppa);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query print data fail\r\n" + result.ToString());
                return failResult;
            }

            // 圖1用
            sqlCmd = string.Format(
                @"select distinct No,ActCycle,{1} as TaktTime
from LineMapping_Detail WITH (NOLOCK) 
where ID = {0} and (IsPPa = 0 or IsPPa is null)
order by No",
                MyUtility.Convert.GetString(this.masterData["ID"]),
                MyUtility.Convert.GetString(this.masterData["TaktTime"]));
            result = DBProxy.Current.Select(null, sqlCmd, out this.actCycleTime);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query cycle time data fail\r\n" + result.ToString());
                return failResult;
            }

            // 圖2用
            sqlCmd = string.Format(
                @"select distinct No,TotalGSD,TotalCycle
from LineMapping_Detail WITH (NOLOCK) 
where ID = {0} and (IsPPa = 0 or IsPPa is null)
order by No",
                MyUtility.Convert.GetString(this.masterData["ID"]),
                MyUtility.Convert.GetString(this.masterData["TaktTime"]));
            result = DBProxy.Current.Select(null, sqlCmd, out this.GCTime);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query cycle time data fail\r\n" + result.ToString());
                return failResult;
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
            if (this.noda.Rows.Count <= 0)
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

            #region 第一頁
            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];
            string factory = MyUtility.Convert.GetString(this.masterData["FactoryID"]);
            worksheet.Cells[1, 1] = MyUtility.GetValue.Lookup(string.Format("select NameEN from factory WITH (NOLOCK) where id = '{0}'", factory));
            string style = MyUtility.Convert.GetString(this.masterData["styleID"]);
            string brand = MyUtility.Convert.GetString(this.masterData["brandID"]);
            string season = MyUtility.Convert.GetString(this.masterData["SeasonID"]);
            string combotype = MyUtility.Convert.GetString(this.masterData["combotype"]);
            worksheet.Cells[2, 2] = style + " " + season + " " + brand + " " + combotype;

            // 填Operation
            int intRowsStart = 6;
            object[,] objArray = new object[1, 5];
            foreach (DataRow dr in this.operationCode.Rows)
            {
                objArray[0, 0] = dr["rn"];
                objArray[0, 1] = this.contentType == "A" ? MyUtility.Convert.GetString(dr["Annotation"]).Trim() : MyUtility.Convert.GetString(dr["DescEN"]).Trim();
                objArray[0, 2] = dr["MachineTypeID"];
                objArray[0, 3] = dr["AT"];
                objArray[0, 4] = dr["GSD"];
                worksheet.Range[string.Format("A{0}:E{0}", intRowsStart)].Value2 = objArray;
                intRowsStart++;
            }

            worksheet.Cells[intRowsStart, 1] = string.Format("=MAX($A$2:A{0})+1", intRowsStart - 1);
            worksheet.Cells[intRowsStart, 5] = string.Format("=SUM(E6:E{0})", intRowsStart - 1);
            worksheet.Range[string.Format("A5:E{0}", intRowsStart)].Borders.Weight = 1; // 1: 虛線, 2:實線, 3:粗體線
            worksheet.Range[string.Format("A5:E{0}", intRowsStart)].Borders.LineStyle = 1;

            intRowsStart++;
            worksheet.Cells[intRowsStart, 5] = string.Format("=E{0}/{1}", intRowsStart - 1, this.nodist.Rows.Count);
            worksheet.get_Range("E" + intRowsStart, "E" + intRowsStart).Font.Bold = true;

            intRowsStart++;
            worksheet.Cells[intRowsStart, 2] = "Picture";
            worksheet.get_Range("B" + intRowsStart, "B" + intRowsStart).Font.Bold = true;

            // 插圖 Picture1
            intRowsStart++;
            string destination_path = MyUtility.GetValue.Lookup("select PicPath from System WITH (NOLOCK) ", null);
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

            excel.Visible = true;

            #region 長條圖資料

            // 填act Cycle Time
            worksheet = excel.ActiveWorkbook.Worksheets[3];
            intRowsStart = 2;
            objArray = new object[1, 3];
            foreach (DataRow dr in this.actCycleTime.Rows)
            {
                objArray[0, 0] = dr["No"];
                objArray[0, 1] = dr["ActCycle"];
                objArray[0, 2] = dr["TaktTime"];
                worksheet.Range[string.Format("A{0}:C{0}", intRowsStart)].Value2 = objArray;
                intRowsStart++;
            }

            // 填 TotalGSD
            worksheet = excel.ActiveWorkbook.Worksheets[4];
            intRowsStart = 2;
            objArray = new object[1, 3];
            foreach (DataRow dr in this.GCTime.Rows)
            {
                objArray[0, 0] = dr["No"];
                objArray[0, 1] = dr["TotalGSD"];
                objArray[0, 2] = dr["TotalCycle"];
                worksheet.Range[string.Format("A{0}:C{0}", intRowsStart)].Value2 = objArray;
                intRowsStart++;
            }
            #endregion

            #region 第二頁

            #region 固定資料

            // 左上表頭資料
            worksheet = excel.ActiveWorkbook.Worksheets[2];
            worksheet.Cells[1, 5] = factory;
            worksheet.Cells[5, 5] = MyUtility.Convert.GetString(this.masterData["SewingLineID"]);
            worksheet.Cells[7, 5] = style;
            worksheet.Cells[9, 5] = this.styleCPU;

            // 右下簽名位置
            worksheet.Cells[28, 15] = DateTime.Now.ToString("d");
            worksheet.Cells[31, 15] = Sci.Env.User.UserName;
            #endregion

            #region MACHINE INVENTORY
            if (this.summt.Rows.Count > 3)
            {
                Microsoft.Office.Interop.Excel.Range rng = worksheet.get_Range("A52:A52").EntireRow; // 選取要被複製的資料
                for (int i = 3; i < this.summt.Rows.Count; i++)
                {
                    Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range("A52", Type.Missing).EntireRow; // 選擇要被貼上的位置
                    rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown, rng.Copy(Type.Missing)); // 貼上
                }
            }

            int sumrow = 0;
            foreach (DataRow item in this.summt.Rows)
            {
                worksheet.Cells[51 + sumrow, 1] = item["MachineTypeID"];
                worksheet.Cells[51 + sumrow, 2] = item["sumct"];
                sumrow++;
            }
            #endregion

            #region 新增長條圖 2

            // 新增長條圖
            Microsoft.Office.Interop.Excel.Worksheet chartData2 = excel.ActiveWorkbook.Worksheets[4];
            worksheet = excel.ActiveWorkbook.Worksheets[2];
            Microsoft.Office.Interop.Excel.Range chartRange2;
            object misValue2 = System.Reflection.Missing.Value;
            Microsoft.Office.Interop.Excel.ChartObjects xlsCharts2 = (Microsoft.Office.Interop.Excel.ChartObjects)worksheet.ChartObjects(Type.Missing);
            Microsoft.Office.Interop.Excel.ChartObject myChart2 = (Microsoft.Office.Interop.Excel.ChartObject)xlsCharts2.Add(378, 1082, 700, 350);
            Microsoft.Office.Interop.Excel.Chart chartPage2 = myChart2.Chart;
            chartRange2 = chartData2.get_Range("B1", string.Format("B{0}", MyUtility.Convert.GetString(intRowsStart - 1)));
            chartPage2.SetSourceData(chartRange2, misValue2);

            chartPage2.ChartType = Microsoft.Office.Interop.Excel.XlChartType.xlColumnClustered;

            // 新增折線圖
            Microsoft.Office.Interop.Excel.SeriesCollection seriesCollection2 = chartPage2.SeriesCollection();
            Microsoft.Office.Interop.Excel.Series series2 = seriesCollection2.NewSeries();
            series2.Values = chartData2.get_Range("C2", string.Format("C{0}", MyUtility.Convert.GetString(intRowsStart - 1)));
            series2.XValues = chartData2.get_Range("A2", string.Format("A{0}", MyUtility.Convert.GetString(intRowsStart - 1)));
            series2.Name = "CT time";

            // 折線圖的資料標籤不顯示
            series2.ApplyDataLabels(Microsoft.Office.Interop.Excel.XlDataLabelsType.xlDataLabelsShowNone, false, false);

            // 隱藏Sheet
            chartData2.Visible = Microsoft.Office.Interop.Excel.XlSheetVisibility.xlSheetHidden;
            #endregion

            #region 新增長條圖 1

            // 新增長條圖
            Microsoft.Office.Interop.Excel.Worksheet chartData = excel.ActiveWorkbook.Worksheets[3];
            worksheet = excel.ActiveWorkbook.Worksheets[2];
            Microsoft.Office.Interop.Excel.Range chartRange;
            object misValue = System.Reflection.Missing.Value;
            Microsoft.Office.Interop.Excel.ChartObjects xlsCharts = (Microsoft.Office.Interop.Excel.ChartObjects)worksheet.ChartObjects(Type.Missing);
            Microsoft.Office.Interop.Excel.ChartObject myChart = (Microsoft.Office.Interop.Excel.ChartObject)xlsCharts.Add(378, 718.5, 700, 350);
            Microsoft.Office.Interop.Excel.Chart chartPage = myChart.Chart;
            chartRange = chartData.get_Range("B1", string.Format("B{0}", MyUtility.Convert.GetString(intRowsStart - 1)));
            chartPage.SetSourceData(chartRange, misValue);

            chartPage.ChartType = Microsoft.Office.Interop.Excel.XlChartType.xlColumnClustered;

            // 新增折線圖
            Microsoft.Office.Interop.Excel.SeriesCollection seriesCollection = chartPage.SeriesCollection();
            Microsoft.Office.Interop.Excel.Series series1 = seriesCollection.NewSeries();
            series1.Values = chartData.get_Range("C2", string.Format("C{0}", MyUtility.Convert.GetString(intRowsStart - 1)));
            series1.XValues = chartData.get_Range("A2", string.Format("A{0}", MyUtility.Convert.GetString(intRowsStart - 1)));
            series1.Name = "Takt time";
            series1.ChartType = Microsoft.Office.Interop.Excel.XlChartType.xlLine;

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
            decimal allct = Math.Ceiling((decimal)this.summt.Rows.Count / 3);
            Microsoft.Office.Interop.Excel.Range rngToCopy = worksheet.get_Range("A42:A42").EntireRow; // 選取要被複製的資料
            for (int i = 0; i < allct - 5; i++)
            {
                Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range("A42", Type.Missing).EntireRow; // 選擇要被貼上的位置
                rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown, rngToCopy.Copy(Type.Missing)); // 貼上
            }

            int surow = 0;
            int sucol = 0;
            foreach (DataRow item in this.summt.Rows)
            {
                worksheet.Cells[38 + surow, 4 + sucol] = item["MachineTypeID"];
                worksheet.Cells[38 + surow, 5 + sucol] = item["sumct"];
                surow++;
                if (allct == surow)
                {
                    surow = 0;
                    sucol += 2;
                }
            }

            worksheet.Cells[38, 3] = this.atct.Rows[0]["a_ct"];
            worksheet.Cells[40, 3] = this.atct.Rows[0]["t_ct"];
            #endregion

            #region Machine Type	
            if (this.noppa.Rows.Count > 10)
            {
                rngToCopy = worksheet.get_Range("A25:A25").EntireRow; // 選取要被複製的資料
                for (int i = 10; i < this.noppa.Rows.Count; i++)
                {
                    Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range("A25", Type.Missing).EntireRow; // 選擇要被貼上的位置
                    rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown, rngToCopy.Copy(Type.Missing)); // 貼上
                }
            }

            int idxppa = 0;
            foreach (DataRow item in this.noppa.Rows)
            {
                worksheet.Cells[25 + idxppa, 1] = item["OperationID"];
                worksheet.Cells[25 + idxppa, 4] = this.contentType == "A" ? MyUtility.Convert.GetString(item["Annotation"]).Trim() : MyUtility.Convert.GetString(item["DescEN"]).Trim();
                worksheet.Cells[25 + idxppa, 9] = item["MachineTypeID"];
                idxppa++;
            }
            #endregion

            #region 預設站數為2站，當超過2站就要新增
            decimal no_count = MyUtility.Convert.GetDecimal(this.nodist.Rows.Count);
            int j = 1; // 資料組數，預設為1
            if (no_count > 2)
            {
                rngToCopy = worksheet.get_Range("A17:A21").EntireRow; // 選取要被複製的資料
                for (j = 2; j <= MyUtility.Convert.GetInt(Math.Ceiling(no_count / 2)); j++)
                {
                    Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range("A17", Type.Missing).EntireRow; // 選擇要被貼上的位置
                    rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown, rngToCopy.Copy(Type.Missing)); // 貼上
                }
            }
            #endregion

            int norow = 17 + ((j - 2) * 5); // No格子上的位置Excel Y軸
            int nocolumn = 9;
            #region U字型列印
            if (this.display == "U")
            {
                int maxct = 3;
                int di = this.nodist.Rows.Count;
                int addct = 0;
                bool flag = true;
                decimal dd = Math.Ceiling((decimal)di / 2);
                List<int> max_ct = new List<int>();
                for (int i = 0; i < dd; i++)
                {
                    int a = MyUtility.Convert.GetInt(this.nodist.Rows[i]["ct"]);
                    int d = 0;
                    if (di % 2 == 1 && flag)
                    {
                        flag = false;
                    }
                    else
                    {
                        if (di % 2 == 1)
                        {
                            d = MyUtility.Convert.GetInt(this.nodist.Rows[di - i]["ct"]);
                        }
                        else
                        {
                            d = MyUtility.Convert.GetInt(this.nodist.Rows[di - 1 - i]["ct"]);
                        }
                    }

                    maxct = a > d ? a : d;
                    maxct = maxct > 3 ? maxct : 3;
                    max_ct.Add(maxct);
                    Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(norow + 3)), Type.Missing).EntireRow;
                    for (int k = 3; k < maxct; k++)
                    {
                        rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                        worksheet.get_Range(string.Format("D{0}:H{0}", MyUtility.Convert.GetString(norow + k))).Merge(false); // 合併儲存格
                        worksheet.get_Range(string.Format("I{0}:J{0}", MyUtility.Convert.GetString(norow + k))).Merge(false); // 合併儲存格
                        worksheet.get_Range(string.Format("L{0}:M{0}", MyUtility.Convert.GetString(norow + k))).Merge(false); // 合併儲存格
                        worksheet.get_Range(string.Format("N{0}:P{0}", MyUtility.Convert.GetString(norow + k))).Merge(false); // 合併儲存格
                        if (i > 0)
                        {
                            addct++;
                        }
                    }

                    norow = norow - 5;
                    maxct = 3;
                }

                bool leftDirection = true;
                norow = 17 + ((j - 2) * 5) + addct;
                int m = 0;
                foreach (DataRow nodr in this.nodist.Rows)
                {
                    if (leftDirection)
                    {
                        nocolumn = 9;
                        worksheet.Cells[norow, nocolumn] = MyUtility.Convert.GetString(nodr["No"]);

                        DataRow[] nodrs = this.noda.Select(string.Format("no = '{0}'", MyUtility.Convert.GetString(nodr["No"])));
                        int ridx = 2;
                        foreach (DataRow item in nodrs)
                        {
                            worksheet.Cells[norow + ridx, nocolumn - 7] = item["cycle"];
                            worksheet.Cells[norow + ridx, nocolumn - 6] = item["gsd"];
                            worksheet.Cells[norow + ridx, nocolumn - 5] = this.contentType == "A" ? MyUtility.Convert.GetString(item["Annotation"]).Trim() : MyUtility.Convert.GetString(item["DescEN"]).Trim();
                            worksheet.Cells[norow, nocolumn - 4] = item["name"];

                            ridx++;
                        }

                        DataRow[] mdrs = this.mt.Select(string.Format("no = '{0}'", MyUtility.Convert.GetString(nodr["No"])));
                        ridx = 2;
                        foreach (DataRow item in mdrs)
                        {
                            worksheet.Cells[norow + ridx, nocolumn] = item["machineTypeid"];
                            ridx++;
                        }

                        m++;
                        if (m == dd)
                        {
                            leftDirection = false;
                            m--;
                            continue;
                        }

                        norow = norow - 5 - (max_ct[m] - 3);
                    }
                    else
                    {
                        nocolumn = 12;
                        worksheet.Cells[norow, nocolumn] = MyUtility.Convert.GetString(nodr["No"]);

                        DataRow[] nodrs = this.noda.Select(string.Format("no = '{0}'", MyUtility.Convert.GetString(nodr["No"])));
                        int ridx = 2;
                        foreach (DataRow item in nodrs)
                        {
                            worksheet.Cells[norow + ridx, nocolumn + 6] = item["cycle"];
                            worksheet.Cells[norow + ridx, nocolumn + 5] = item["gsd"];
                            worksheet.Cells[norow + ridx, nocolumn + 2] = this.contentType == "A" ? MyUtility.Convert.GetString(item["Annotation"]).Trim() : MyUtility.Convert.GetString(item["DescEN"]).Trim();
                            worksheet.Cells[norow, nocolumn + 3] = item["name"];

                            ridx++;
                        }

                        DataRow[] mdrs = this.mt.Select(string.Format("no = '{0}'", MyUtility.Convert.GetString(nodr["No"])));
                        ridx = 2;
                        foreach (DataRow item in mdrs)
                        {
                            worksheet.Cells[norow + ridx, nocolumn] = item["machineTypeid"];
                            ridx++;
                        }

                        norow = norow + 5 + (max_ct[m] - 3);
                        m--;
                    }
                }
            }
            #endregion
            #region Z字型列印
            else
            {
                int maxct = 3;
                int ct = 0;
                int addct = 0;
                int indx = 1;
                foreach (DataRow nodr in this.nodist.Rows)
                {
                    maxct = MyUtility.Convert.GetInt(nodr["ct"]) > maxct ? MyUtility.Convert.GetInt(nodr["ct"]) : maxct;
                    ct++;
                    if (ct == 2)
                    {
                        Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(norow + 3)), Type.Missing).EntireRow;
                        for (int i = 3; i < maxct; i++)
                        {
                            rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                            worksheet.get_Range(string.Format("D{0}:H{0}", MyUtility.Convert.GetString(norow + i))).Merge(false); // 合併儲存格
                            worksheet.get_Range(string.Format("I{0}:J{0}", MyUtility.Convert.GetString(norow + i))).Merge(false); // 合併儲存格
                            worksheet.get_Range(string.Format("L{0}:M{0}", MyUtility.Convert.GetString(norow + i))).Merge(false); // 合併儲存格
                            worksheet.get_Range(string.Format("N{0}:P{0}", MyUtility.Convert.GetString(norow + i))).Merge(false); // 合併儲存格
                            if (indx > 2)
                            {
                                addct++;
                            }
                        }

                        norow = norow - 5;
                        ct = 0;
                        maxct = 3;
                    }

                    indx++;
                }

                norow = 17 + ((j - 2) * 5) + addct;
                int leftright_count = 2;
                bool leftDirection = true;
                indx = 2;
                foreach (DataRow nodr in this.nodist.Rows)
                {
                    if (leftDirection)
                    {
                        nocolumn = 9;
                        worksheet.Cells[norow, nocolumn] = MyUtility.Convert.GetString(nodr["No"]);

                        DataRow[] nodrs = this.noda.Select(string.Format("no = '{0}'", MyUtility.Convert.GetString(nodr["No"])));
                        int ridx = 2;
                        foreach (DataRow item in nodrs)
                        {
                            worksheet.Cells[norow + ridx, nocolumn - 7] = item["cycle"];
                            worksheet.Cells[norow + ridx, nocolumn - 6] = item["gsd"];
                            worksheet.Cells[norow + ridx, nocolumn - 5] = this.contentType == "A" ? MyUtility.Convert.GetString(item["Annotation"]).Trim() : MyUtility.Convert.GetString(item["DescEN"]).Trim();
                            worksheet.Cells[norow, nocolumn - 4] = item["name"];

                            ridx++;
                        }

                        DataRow[] mdrs = this.mt.Select(string.Format("no = '{0}'", MyUtility.Convert.GetString(nodr["No"])));
                        ridx = 2;
                        foreach (DataRow item in mdrs)
                        {
                            worksheet.Cells[norow + ridx, nocolumn] = item["machineTypeid"];
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
                        nocolumn = 12;
                        worksheet.Cells[norow, nocolumn] = MyUtility.Convert.GetString(nodr["No"]);

                        DataRow[] nodrs = this.noda.Select(string.Format("no = '{0}'", MyUtility.Convert.GetString(nodr["No"])));
                        int ridx = 2;
                        foreach (DataRow item in nodrs)
                        {
                            worksheet.Cells[norow + ridx, nocolumn + 6] = item["cycle"];
                            worksheet.Cells[norow + ridx, nocolumn + 5] = item["gsd"];
                            worksheet.Cells[norow + ridx, nocolumn + 2] = this.contentType == "A" ? MyUtility.Convert.GetString(item["Annotation"]).Trim() : MyUtility.Convert.GetString(item["DescEN"]).Trim();
                            worksheet.Cells[norow, nocolumn + 3] = item["name"];

                            ridx++;
                        }

                        DataRow[] mdrs = this.mt.Select(string.Format("no = '{0}'", MyUtility.Convert.GetString(nodr["No"])));
                        ridx = 2;
                        foreach (DataRow item in mdrs)
                        {
                            worksheet.Cells[norow + ridx, nocolumn] = item["machineTypeid"];
                            ridx++;
                        }

                        leftright_count++;
                        if (leftright_count > 2)
                        {
                            leftright_count = 1;
                            leftDirection = true;
                        }
                    }

                    if (this.nodist.Rows.Count > indx)
                    {
                        maxct = MyUtility.Convert.GetInt(this.nodist.Rows[indx]["ct"]) > maxct ? MyUtility.Convert.GetInt(this.nodist.Rows[indx]["ct"]) : maxct;
                    }

                    if (leftright_count == 2)
                    {
                        norow = norow - 5 - (maxct - 3);
                        maxct = 3;
                    }

                    indx++;
                }
            }
            #endregion

            #endregion

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
    }
}
