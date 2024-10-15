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
    public partial class P06_OperatorSummary : Sci.Win.Tems.QueryForm
    {
        /// <inheritdoc/>
        public P06_OperatorSummary(string lmbID = "")
        {
            this.InitializeComponent();
            this.LoadData(lmbID);
        }
        /// <inheritdoc/>
        public void LoadData(string lmbID)
        {
            /* 修改時記得參照P05_Print公式 */
            var sqlcmd = $@"
			DECLARE @LMBID bigint = {lmbID}
			SELECT
            [OperatorID] = lmbd.EmployeeID
            ,[ActualEffi] = iif(lmbd.Cycle = 0,0,ROUND(lmbd.GSD/ lmbd.Cycle,2)*100)
            FROM LineMappingBalancing lmb
            INNER JOIN LineMappingBalancing_Detail lmbd WITH(NOLOCK) ON lmbd.ID = lmb.ID
            left join MachineType_Detail md on md.ID = lmbd.MachineTypeID and md.FactoryID = lmb.FactoryID
            where lmb.ID = @LMBID and ISNULL(md.IsNotShownInP06,0) = 0
            Order by lmbd.No";

            DualResult result = DBProxy.Current.Select(null, sqlcmd, out DataTable dtChart);

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

            this.chart1.Series.Add("Actual Effi%");
            this.chart1.Series["Actual Effi%"].ChartType = SeriesChartType.Column;
            this.chart1.Series["Actual Effi%"].Color = Color.CornflowerBlue;

            // 傳值進去前面設定的Series
            for (int i = 0; i < dtChart.Rows.Count; i++)
            {
                string operarorID = dtChart.Rows[i]["OperatorID"].ToString();
                chartArea.AxisX.CustomLabels.Add(i - 0.5, i + 0.5, operarorID); // X軸的文字

                this.chart1.Series[0].Points.AddXY(i, MyUtility.Convert.GetDecimal(dtChart.Rows[i]["ActualEffi"]));
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
