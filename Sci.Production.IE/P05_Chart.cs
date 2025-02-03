using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.IE
{
    /// <inheritdoc/>
    public partial class P05_Chart : Sci.Win.Tems.QueryForm
    {
        private string almID = string.Empty;
        private int sewermanpower = 0;
        private DataTable dtChart;

        /// <inheritdoc/>
        public P05_Chart(int sewermanpower = 0, string almID = "")
        {
            this.InitializeComponent();
            this.almID = almID == string.Empty ? "0" : almID;
            this.sewermanpower = sewermanpower;
            this.LoadData();
        }

        /// <inheritdoc/>
        public void LoadData()
        {
            var sqlcmd = $@"
			DECLARE @ALMID int = {this.almID}
            DECLARE @Sewermanpower int = {this.sewermanpower}
            select
            AD.No
            ,AD.SewerManpower
            , ActGSDTime = Round(Sum(AD.GSD * AD.SewerDiffPercentage), 2)
            , TaktTime = Round(3600 * alm.WorkHour / Round(Round(3600 * AD.SewerManpower / alm.TotalGSDTime, 0) * alm.WorkHour, 0), 2)
            , ActGSDTime_Avg = Round(alm.TotalGSDTime / AD.SewerManpower, 2)
            , ct = count(1)
            from  AutomatedLineMapping_DetailTemp ad with (nolock) 
            LEFT JOIN AutomatedLineMapping alm WITH(NOLOCK) ON  ALM.ID = AD.ID
            left join DropDownList d with (nolock) on d.ID = ad.PPA  and d.Type = 'PMS_IEPPA'
            left join Operation op with (nolock) on op.ID = ad.OperationID
            left join MachineType_Detail md on md.ID = AD.MachineTypeID  and md.FactoryID = alm.FactoryID
            where ad.id = @ALMID and ad.SewerManpower = @Sewermanpower and no != ''
            group by AD.No, alm.WorkHour, AD.SewerManpower, alm.TotalGSDTime,alm.SewerManpower
            order by AD.NO,alm.SewerManpower";

            DualResult result = DBProxy.Current.Select(null, sqlcmd, out this.dtChart);

            if (!result)
            {
                MyUtility.Msg.WarningBox("Sql connection fail!!\r\n" + result.ToString());
                return;
            }

            // 建立新的圖表區域
            ChartArea chartArea = new ChartArea();
            chartArea.AxisX.MajorGrid.Enabled = false; // 設定X軸線是否開啟
            chartArea.AxisY.MajorGrid.Enabled = true;   // 設定Y軸線是否開啟

            chartArea.AxisX.Interval = 1;   // X軸從1開始
            chartArea.AxisX.LabelStyle.Angle = 45;  // X軸 文字樣式 45度

            // chartArea.AxisY.Interval = 10;  // Y軸10度為一個區間
            chartArea.AxisY.Minimum = 0;  // Y軸最小為 0;

            this.chart1.ChartAreas.Add(chartArea);

            this.chart1.Series.Add("GSD Time");
            this.chart1.Series["GSD Time"].ChartType = SeriesChartType.Column;
            this.chart1.Series["GSD Time"].Color = Color.CornflowerBlue;

            this.chart1.Series.Add("Takt time");
            this.chart1.Series["Takt time"].ChartType = SeriesChartType.Line;
            this.chart1.Series["Takt time"].Color = Color.Brown;
            this.chart1.Series["Takt time"].BorderWidth = 3;

            this.chart1.Series.Add("Avg. GSD Time");
            this.chart1.Series["Avg. GSD Time"].ChartType = SeriesChartType.Line;
            this.chart1.Series["Avg. GSD Time"].Color = Color.YellowGreen;
            this.chart1.Series["Avg. GSD Time"].BorderWidth = 3;

            // 傳值進去前面設定的Series
            for (int i = 0; i < this.dtChart.Rows.Count; i++)
            {
                string label = this.dtChart.Rows[i]["No"].ToString();
                chartArea.AxisX.CustomLabels.Add(i - 0.5, i + 0.5, label); // X軸的文字

                this.chart1.Series[0].Points.AddXY(i, MyUtility.Convert.GetDecimal(this.dtChart.Rows[i]["ActGSDTime"]));
                this.chart1.Series[1].Points.AddXY(i, MyUtility.Convert.GetDecimal(this.dtChart.Rows[i]["TaktTime"]));
                this.chart1.Series[2].Points.AddXY(i, MyUtility.Convert.GetDecimal(this.dtChart.Rows[i]["ActGSDTime_Avg"]));
            }
        }
    }
}
