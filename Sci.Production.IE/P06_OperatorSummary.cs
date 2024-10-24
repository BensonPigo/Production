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
			--SELECT
            --[OperatorID] = lmbd.EmployeeID
            --,[ActualEffi] = iif(lmbd.Cycle = 0,0,ROUND(lmbd.GSD/ lmbd.Cycle,2)*100)
            --FROM LineMappingBalancing lmb
            --INNER JOIN LineMappingBalancing_Detail lmbd WITH(NOLOCK) ON lmbd.ID = lmb.ID
            --left join MachineType_Detail md on md.ID = lmbd.MachineTypeID and md.FactoryID = lmb.FactoryID
            --where lmb.ID = @LMBID and ISNULL(md.IsNotShownInP06,0) = 0
            --Order by lmbd.No

            SELECT
			*
			,[EstCycleTime] = iif(a.OperatorEffi = '0.00','0.00',a.GSD / a.OperatorEffi * 100)
			from
			(
				SELECT
				 [No]  = lmbd.[No]
				,[OperatorID] = lmbd.EmployeeID
				,[GSD] = lmbd.GSD
				,[ActualEffi] = iif(lmbd.Cycle = 0,0,ROUND(lmbd.GSD/ lmbd.Cycle,2)*100)
				, [OperatorEffi] = isnull(iif(Effi.Effi_3_year = '' or Effi.Effi_3_year is null ,Effi_90_day.Effi_90_day,Effi.Effi_3_year) ,'0.00')
				FROM LineMappingBalancing lmb
				INNER JOIN LineMappingBalancing_Detail lmbd WITH(NOLOCK) ON lmbd.ID = lmb.ID
				left join MachineType_Detail md on md.ID = lmbd.MachineTypeID and md.FactoryID = lmb.FactoryID
				OUTER APPLY
				(
					SELECT TOP 1
					val = OperatorIDss.OperationID
					FROM
					(
						SELECT 
						td.id
						,td.Seq
						,td.OperationID
						from TimeStudy_Detail td WITH(NOLOCK)
						where  td.OperationID LIKE '-%' and td.smv = 0
					)
					OperatorIDss 
					WHERE ID =  lmb.ID AND SEQ <= (SELECT TOP 1 Seq FROM TimeStudy_Detail WHERE id = lmb.ID AND OperationID = lmbd.OperationID ORDER BY Seq DESC)
					ORDER BY SEQ DESC
				)Group_Header
				OUTER APPLY
				(
					select val = stuff((select distinct concat(',',Name)
					from OperationRef a
					inner JOIN IESELECTCODE b WITH(NOLOCK) on a.CodeID = b.ID and a.CodeType = b.Type
					where a.CodeType = '00007' and a.id = lmbd.OperationID  for xml path('') ),1,1,'')
				)Motion
				OUTER APPLY
				(
					SELECT
					[ST_MC_Type]
					,[Motion]
					,[Effi_90_day] =isnull(FORMAT(AVG(CAST([Effi_90_day] AS DECIMAL(10, 2))), '0.00'),'0')
					From
					(
						SELECT 
						[ST_MC_Type] =lmd.MachineTypeID
						,[Motion] = Operation_P03.val
						,Effi_90_day = FORMAT(CAST(iif(LM.ID is null or LMD.Cycle = 0,0,ROUND(lmd.GSD/ lmd.Cycle * 100,2)) AS DECIMAL(10, 2)), '0.00')
						from Employee eo
						left JOIN LineMapping_Detail lmd WITH(NOLOCK) on lmd.EmployeeID = eo.ID　
						left JOIN LineMapping lm WITH(NOLOCK) on lm.id = lmd.ID
						OUTER APPLY
						(
							select val = stuff((select distinct concat(',',Name)
							from OperationRef a
							inner JOIN IESELECTCODE b WITH(NOLOCK) on a.CodeID = b.ID and a.CodeType = b.Type
							where a.CodeType = '00007' and a.id = lmd.OperationID  for xml path('') ),1,1,'')
						)Operation_P03
						WHERE 
						eo.FactoryID = lmb.FactoryID and eo.ID = lmbd.EmployeeID AND
						lmd.MachineTypeID = lmbd.MachineTypeID and
						Operation_P03.val = Motion.val AND
						((lm.EditDate >= DATEADD(day, -90, GETDATE()) and lm.EditDate <= GETDATE()) or (lm.AddDate >= DATEADD(day, -90, GETDATE()) and lm.AddDate <= GETDATE()))
					)a
					GROUP BY [ST_MC_Type],[Motion]
				)Effi_90_day
				 OUTER APPLY
				(
					SELECT
					[ST_MC_Type]
					,[Motion]
					,[Group_Header]
					,[Part]
					,[Attachment]
					,[Effi_3_year] =isnull(FORMAT(AVG(CAST([Effi_3_year] AS DECIMAL(10, 2))), '0.00'),'0.00')
					From
					(
						SELECT 
						[ST_MC_Type] =lmd.MachineTypeID
						,[Motion] = Operation_P03.val
						,[Group_Header] = ISNULL(IIF(REPLACE(tsd.[location], '--', '') = '', REPLACE(OP.OperationID, '--', '') ,REPLACE(tsd.[location], '--', '')),'')
						,[Part] = lmd.SewingMachineAttachmentID
						,[Attachment] = lmd.Attachment
						,Effi_3_year = FORMAT(CAST(iif(lmd.Cycle = 0,0,ROUND(lmd.GSD/ lmd.Cycle * 100,2)) AS DECIMAL(10, 2)), '0.00')
						from Employee eo
						left JOIN LineMapping_Detail lmd WITH(NOLOCK) on lmd.EmployeeID = eo.ID　
						left JOIN LineMapping lm WITH(NOLOCK) on lm.id = lmd.ID
						left JOIN TimeStudy_Detail tsd WITH(NOLOCK) on lmd.OperationID = tsd.OperationID
						OUTER APPLY
						(
						select val = stuff((select distinct concat(',',Name)
								from OperationRef a
								inner JOIN IESELECTCODE b WITH(NOLOCK) on a.CodeID = b.ID and a.CodeType = b.Type
								where a.CodeType = '00007' and a.id = lmd.OperationID  for xml path('') ),1,1,'')
						)Operation_P03
						OUTER APPLY
						(
							SELECT TOP 1
							OperatorIDss.OperationID
							FROM
							(
								SELECT 
								td.id
								,td.Seq
								,td.OperationID
								from TimeStudy_Detail td WITH(NOLOCK)
								where  td.OperationID LIKE '-%' and td.smv = 0
							)
							OperatorIDss 
							WHERE ID =  lmb.ID AND SEQ <= (SELECT TOP 1 Seq FROM TimeStudy_Detail WHERE id = lmb.ID AND OperationID = LMD.OperationID ORDER BY Seq DESC)
							ORDER BY SEQ DESC
						)OP
						WHERE 
						eo.FactoryID = lmb.FactoryID and eo.ID = lmbd.EmployeeID AND
						lmd.MachineTypeID = lmbd.MachineTypeID and
						Operation_P03.val = Motion.val AND
						ISNULL(lmd.Attachment,'') = ISNULL(lmbd.Attachment,'') AND
						lmd.SewingMachineAttachmentID = lmbd.SewingMachineAttachmentID AND
						ISNULL(IIF(REPLACE(tsd.[location], '--', '') = '', REPLACE(OP.OperationID, '--', '') ,REPLACE(tsd.[location], '--', '')),'') =  ISNULL(REPLACE(Group_Header.val, '--', ''),'') AND
						((lm.EditDate >= DATEADD(DAY, -360, GETDATE()) and lm.EditDate <= GETDATE()) or (lm.AddDate >= DATEADD(DAY, -360, GETDATE()) and lm.AddDate <= GETDATE()))
					)a
					GROUP BY [ST_MC_Type],[Motion], [Group_Header], [Part], [Attachment]
				)Effi
				where lmb.ID = @LMBID and ISNULL(md.IsNotShownInP06,0) = 0
			)a
			Order by [No]

            ";

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

            this.chart1.Series.Add("Est Effi%");
            this.chart1.Series["Est Effi%"].ChartType = SeriesChartType.Line;
            this.chart1.Series["Est Effi%"].Color = Color.Brown;
            this.chart1.Series["Est Effi%"].BorderWidth = 3;

            // 傳值進去前面設定的Series
            for (int i = 0; i < dtChart.Rows.Count; i++)
            {
                string operarorID = dtChart.Rows[i]["OperatorID"].ToString();
                chartArea.AxisX.CustomLabels.Add(i - 0.5, i + 0.5, operarorID); // X軸的文字

                this.chart1.Series[0].Points.AddXY(i, MyUtility.Convert.GetDecimal(dtChart.Rows[i]["ActualEffi"]));
                this.chart1.Series[1].Points.AddXY(i, MyUtility.Convert.GetDecimal(dtChart.Rows[i]["EstCycleTime"]));
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
