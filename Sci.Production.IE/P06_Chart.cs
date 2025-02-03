using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Sci.Production.IE
{
    /// <inheritdoc/>
    public partial class P06_Chart : Sci.Win.Tems.QueryForm
    {
        private string almID;
        private DataTable[] dataTables;

        /// <inheritdoc/>
        public P06_Chart(string almID = "")
        {
            this.InitializeComponent();
            this.almID = almID;
            this.LoadData();
        }

        /// <inheritdoc/>
        public void LoadData()
        {
            /* 修改時記得參照P05_Print公式 */
            var sqlcmd = $@"
			DECLARE @ALMID bigint = {this.almID}
			-- Excel [Line Mapping] Line Balancing Graph圖表資料
            select No
            , ActGSDTime = Round(Sum(almd.Cycle * almd.SewerDiffPercentage), 2)
            , TaktTime = IIF(alm.TotalCycleTime = 0, 0, Round(3600 * alm.WorkHour / Round(Round(3600 * alm.SewerManpower / alm.TotalCycleTime, 0) * alm.WorkHour, 0), 2))
            , ActCycleTime_Avg = Round(alm.TotalCycleTime / alm.SewerManpower, 2)
            , ct = count(1)
            from LineMappingBalancing alm
            left join LineMappingBalancing_Detail almd on alm.ID = almd.ID
            left join MachineType_Detail md on md.ID = almd.MachineTypeID  and md.FactoryID = alm.FactoryID
            where alm.ID  = @ALMID
            and almd.IsNonSewingLine = 0
            and isnull(md.IsNotShownInP06,0) = 0
            and almd.PPA != 'C'
            group by almd.No, alm.WorkHour, alm.SewerManpower, alm.TotalCycleTime

            -- Excel [Line Mapping] Total GSD Time / Total Cycle Time Graph圖表資料
            select No
            , TotalGSDTime = Round(Sum(almd.GSD * almd.SewerDiffPercentage),2)
            , TotalCycleTime = Round(Sum(almd.Cycle * almd.SewerDiffPercentage),2)
            from LineMappingBalancing_Detail almd
            left join LineMappingBalancing alm on alm.ID = almd.ID
            left join MachineType_Detail md on md.ID = almd.MachineTypeID  and md.FactoryID = alm.FactoryID
            where almd.ID = @ALMID
            and almd.IsNonSewingLine = 0
            and isnull(md.IsNotShownInP06,0) = 0
            and almd.PPA != 'C'
            group by almd.No
			";

            DualResult result = DBProxy.Current.Select(null, sqlcmd, out this.dataTables);

            if (!result)
            {
                MyUtility.Msg.WarningBox("Sql connection fail!!\r\n" + result.ToString());
                return;
            }

            #region Line Balancing Graph 圖

            // 建立新的圖表區域
            ChartArea chartArea = new ChartArea();
            chartArea.AxisX.MajorGrid.Enabled = false; // 設定X軸線是否開啟
            chartArea.AxisY.MajorGrid.Enabled = true;   // 設定Y軸線是否開啟

            chartArea.AxisX.Interval = 1;   // X軸從1開始
            chartArea.AxisX.LabelStyle.Angle = 45;  // X軸 文字樣式 45度

            // chartArea.AxisY.Interval = 10;  // Y軸10度為一個區間
            chartArea.AxisY.Minimum = 0;  // Y軸最小為 0;

            this.chart1.ChartAreas.Add(chartArea);

            this.chart1.Series.Add("Act Cycle Time");
            this.chart1.Series["Act Cycle Time"].ChartType = SeriesChartType.Column;
            this.chart1.Series["Act Cycle Time"].Color = Color.CornflowerBlue;

            this.chart1.Series.Add("Takt time");
            this.chart1.Series["Takt time"].ChartType = SeriesChartType.Line;
            this.chart1.Series["Takt time"].Color = Color.Red;
            this.chart1.Series["Takt time"].BorderWidth = 3;

            this.chart1.Series.Add("Avg. Cycle Time");
            this.chart1.Series["Avg. Cycle Time"].ChartType = SeriesChartType.Line;
            this.chart1.Series["Avg. Cycle Time"].Color = Color.YellowGreen;
            this.chart1.Series["Avg. Cycle Time"].BorderWidth = 3;

            // 傳值進去前面設定的Series
            for (int i = 0; i < this.dataTables[0].Rows.Count; i++)
            {
                string label = this.dataTables[0].Rows[i]["No"].ToString();
                chartArea.AxisX.CustomLabels.Add(i - 0.5, i + 0.5, label); // X軸的文字

                this.chart1.Series[0].Points.AddXY(i, MyUtility.Convert.GetDecimal(this.dataTables[0].Rows[i]["ActGSDTime"]));
                this.chart1.Series[1].Points.AddXY(i, MyUtility.Convert.GetDecimal(this.dataTables[0].Rows[i]["TaktTime"]));
                this.chart1.Series[2].Points.AddXY(i, MyUtility.Convert.GetDecimal(this.dataTables[0].Rows[i]["ActCycleTime_Avg"]));
            }
            #endregion Line Balancing Graph 圖

            #region Total Cycle Time Graph圖表資料

            // 建立新的圖表區域
            ChartArea chartArea1 = new ChartArea();
            chartArea1.AxisX.MajorGrid.Enabled = false; // 設定X軸線是否開啟
            chartArea1.AxisY.MajorGrid.Enabled = true;   // 設定Y軸線是否開啟

            chartArea1.AxisX.Interval = 1;   // X軸從1開始
            chartArea1.AxisX.LabelStyle.Angle = 45;  // X軸 文字樣式 45度

            // chartArea1.AxisY.Interval = 10;  // Y軸10度為一個區間
            chartArea1.AxisY.Minimum = 0;  // Y軸最小為 0;

            this.chart2.ChartAreas.Add(chartArea1);

            this.chart2.Series.Add("Total GSD Time");
            this.chart2.Series["Total GSD Time"].ChartType = SeriesChartType.Column;
            this.chart2.Series["Total GSD Time"].Color = Color.CornflowerBlue;

            this.chart2.Series.Add("Total Cycle Time");
            this.chart2.Series["Total Cycle Time"].ChartType = SeriesChartType.Column;
            this.chart2.Series["Total Cycle Time"].Color = Color.IndianRed;

            // 傳值進去前面設定的Series
            for (int i = 0; i < this.dataTables[1].Rows.Count; i++)
            {
                string label = this.dataTables[1].Rows[i]["No"].ToString();
                chartArea1.AxisX.CustomLabels.Add(i - 0.5, i + 0.5, label); // X軸的文字

                this.chart2.Series[0].Points.AddXY(i, MyUtility.Convert.GetDecimal(this.dataTables[1].Rows[i]["TotalGSDTime"]));
                this.chart2.Series[1].Points.AddXY(i, MyUtility.Convert.GetDecimal(this.dataTables[1].Rows[i]["TotalCycleTime"]));
            }
            #endregion Total Cycle Time Graph圖表資料
        }
    }
}
