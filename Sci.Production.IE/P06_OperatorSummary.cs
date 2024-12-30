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
            -- 查詢和計算績效
                        -- 查詢和計算績效
			SELECT
            *
            into #tmp
            FROM
            (
                SELECT
                    [No] = lmbd.[No]
                    ,[OperatorID] = lmbd.EmployeeID
                    ,[GSD] = lmbd.GSD
					,[Cycle] = lmbd.Cycle * lmbd.SewerDiffPercentage
                    ,[ActualEffi] = IIF(lmbd.Cycle = 0, 0, CAST((lmbd.GSD / lmbd.Cycle * lmbd.SewerDiffPercentage) as NUMERIC(7,4) ) * 100)
					,[OperatorEffi] = ISNULL(
											IIF(
												Effi.Effi_3_year IS NULL, 
												TRY_CAST(Effi_90_day.Effi_90_day AS NUMERIC(7,4)), 
												TRY_CAST(Effi.Effi_3_year AS NUMERIC(7,4))
											), 
											0.00
										)
					   ,[MachineTypeID]= lmbd.MachineTypeID
			           ,[Motion] = Motion.val
					   ,[AAA] = ISNULL(REPLACE(lmbd.[Location], '--', ''),'')
					   ,[PartID] = PartID_lmdb.val
			           ,lmbd.Attachment
                FROM LineMappingBalancing lmb
                INNER JOIN LineMappingBalancing_Detail lmbd WITH(NOLOCK) ON lmbd.ID = lmb.ID
                LEFT JOIN MachineType_Detail md ON md.ID = lmbd.MachineTypeID AND md.FactoryID = lmb.FactoryID
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
                        FROM TimeStudy_Detail td WITH(NOLOCK)
                        WHERE td.OperationID LIKE '-%' AND td.smv = 0
                    ) OperatorIDss 
                    WHERE ID = lmb.ID AND SEQ <= (SELECT TOP 1 Seq FROM TimeStudy_Detail WHERE id = lmb.ID AND OperationID = lmbd.OperationID ORDER BY Seq DESC)
                    ORDER BY SEQ DESC
                ) Group_Header
                OUTER APPLY
                (
                    SELECT val = STUFF((SELECT DISTINCT CONCAT(',', Name)
                        FROM OperationRef a
                        INNER JOIN IESELECTCODE b WITH(NOLOCK) ON a.CodeID = b.ID AND a.CodeType = b.Type
                        WHERE a.CodeType = '00007' AND a.id = lmbd.OperationID FOR XML PATH('')), 1, 1, '')
                ) Motion
				OUTER APPLY
				(
					SELECT val = R.[NAME]
					FROM Operation O WITH(NOLOCK)
					LEFT JOIN Reason R WITH(NOLOCK) ON R.ReasonTypeID = 'IE_Component' AND R.ID = SUBSTRING(O.ID,6,2)
					WHERE O.ID = lmbd.OperationID 
				)PartID_lmdb
                OUTER APPLY
                (
                    SELECT
                    [Effi_90_day] = CAST(SUM(GSD) / SUM(Cycle) AS NUMERIC(7, 4))
                    FROM
                    (
                                              SELECT
						[ST_MC_Type] = ISNULL(lmd.MachineTypeID,'')
						, [Motion] = ISNULL(Operation_P03.val,'')
						, [DiffDays] = DATEDIFF(DAY,lm_Day.EditDate,GETDATE())
						, lmd.GSD 
						, [Cycle] = lmd.Cycle * 1.0
						FROM Employee e WITH(NOLOCK)
						INNER JOIN LineMapping_Detail lmd WITH(NOLOCK) ON lmd.EmployeeID = e.ID
						INNER JOIN LineMapping lm_Day WITH(NOLOCK) ON lm_Day.id = lmd.ID
						OUTER APPLY (
							SELECT val = STUFF((
							SELECT DISTINCT CONCAT(',', Name)
							FROM OperationRef a WITH(NOLOCK)
							INNER JOIN IESELECTCODE b WITH(NOLOCK) ON a.CodeID = b.ID AND a.CodeType = b.Type
							WHERE a.CodeType = '00007' AND a.id = lmd.OperationID  
							FOR XML PATH('')), 1, 1, '')
						) Operation_P03
						WHERE 
						e.FactoryID IN (select ID from Factory where FTYGroup = lmb.FactoryID) AND
						(ISNULL(lmd.MachineTypeID,'') != '') AND 
						e.ID = lmbd.EmployeeID  AND
						lmd.MachineTypeID = lmbd.MachineTypeID AND
						Operation_P03.val = Motion.val AND 
						DATEDIFF(DAY,lm_Day.EditDate,GETDATE()) <= 90
			            UNION
			            SELECT
							[ST_MC_Type] = ISNULL(olmbd.MachineTypeID,'')
						, [Motion] = ISNULL(Operation_P06.val,'')
						, [DiffDays] = DATEDIFF(DAY,lmb_Day.EditDate,GETDATE())
						, olmbd.GSD 
						, [Cycle] = olmbd.Cycle * olmbd.SewerDiffPercentage
						FROM Employee e WITH(NOLOCK)
						INNER JOIN LineMappingBalancing_Detail olmbd WITH(NOLOCK) ON olmbd.EmployeeID = e.ID
						INNER JOIN LineMappingBalancing lmb_Day WITH(NOLOCK) ON lmb_Day.id = olmbd.ID
						OUTER APPLY (
							SELECT val = STUFF((
							SELECT DISTINCT CONCAT(',', Name)
							FROM OperationRef a WITH(NOLOCK)
							INNER JOIN IESELECTCODE b WITH(NOLOCK) ON a.CodeID = b.ID AND a.CodeType = b.Type
							WHERE a.CodeType = '00007' AND a.id = olmbd.OperationID  
							FOR XML PATH('')), 1, 1, '')
						) Operation_P06
						WHERE 
						e.FactoryID IN (select ID from Factory where FTYGroup = lmb.FactoryID) AND 
						(ISNULL(olmbd.MachineTypeID,'') != '') AND 
						e.ID = lmbd.EmployeeID AND
						ISNULL(olmbd.MachineTypeID,'') = lmbd.MachineTypeID AND
						ISNULL(Operation_P06.val,'') = Motion.val  AND 
						DATEDIFF(DAY,lmb_Day.EditDate,GETDATE()) <= 90
                    ) a
                ) Effi_90_day
                OUTER APPLY
                (
                    SELECT
			        [Effi_3_year] =  CAST(SUM(GSD) / SUM(Cycle) AS NUMERIC(7, 4))
			        FROM (
				        SELECT
				       [ST_MC_Type] = ISNULL(lmd.MachineTypeID,''),
				       [Motion] = ISNULL(Operation_P03.val,''),
				       [Group_Header] =  ISNULL(IIF(REPLACE(tsd.[location], '--', '') = '', REPLACE(OP.OperationID, '--', '') ,REPLACE(tsd.[location], '--', '')),''),
				       [Part] = ISNULL(lmd.SewingMachineAttachmentID,''),
				       [Attachment] = ISNULL(lmd.Attachment,'') + ' ' + ISNULL(lmd.Template,'')
				       ,lmd.GSD
				       ,[Cycle] = lmd.Cycle * 1.0
				       FROM Employee e WITH(NOLOCK)
				       INNER JOIN LineMapping_Detail lmd WITH(NOLOCK) ON lmd.EmployeeID = e.ID
				       INNER JOIN LineMapping lm_Day WITH(NOLOCK) ON lm_Day.id = lmd.ID  AND ((lm_Day.EditDate >= DATEADD(YEAR, -3, GETDATE()) AND lm_Day.EditDate <= GETDATE()) OR (lm_Day.AddDate >= DATEADD(YEAR, -3, GETDATE()) AND lm_Day.AddDate <= GETDATE()))
				       OUTER APPLY (
					       SELECT val = STUFF((
						        SELECT DISTINCT CONCAT(',', Name)
						        FROM OperationRef a WITH(NOLOCK)
						        INNER JOIN IESelectCode b WITH(NOLOCK) ON a.CodeID = b.ID AND a.CodeType = b.Type
						        WHERE a.CodeType = '00007' AND a.id = lmd.OperationID  
						        FOR XML PATH('')), 1, 1, '')
				       ) Operation_P03
				       OUTER APPLY(
					       select TOP 1 tsd.Location,tsd.ID
					       from TimeStudy TS
					       inner join TimeStudy_Detail tsd WITH(NOLOCK) ON tsd.id = ts.id
					       where TS.StyleID = lm_Day.StyleID AND TS.SeasonID = lm_Day.SeasonID AND TS.ComboType = lm_Day.ComboType AND TS.BrandID = lm_Day.BrandID
					       and lmd.OperationID = tsd.OperationID
					   
				       )tsd
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
					       WHERE ID =  tsd.ID AND SEQ <= (SELECT TOP 1 Seq FROM TimeStudy_Detail WHERE id = tsd.ID AND OperationID = LMD.OperationID ORDER BY Seq DESC)
					       ORDER BY SEQ DESC
				       )OP
					   OUTER APPLY
						(
							SELECT val = R.[NAME]
							FROM Operation O WITH(NOLOCK)
							LEFT JOIN Reason R WITH(NOLOCK) ON R.ReasonTypeID = 'IE_Component' AND R.ID = SUBSTRING(O.ID,6,2)
							WHERE O.ID = lmd.OperationID 
						)PartID
				       WHERE 
                       e.FactoryID IN (select ID from Factory where FTYGroup = lmb.FactoryID) 
			           AND e.ID = lmbd.EmployeeID
			           AND ISNULL(lmd.MachineTypeID,'') = lmbd.MachineTypeID
			           AND ISNULL(Operation_P03.val,'') = ISNULL(Motion.val,'')
			           AND ISNULL(IIF(REPLACE(tsd.[location], '--', '') = '', REPLACE(OP.OperationID, '--', '') ,REPLACE(tsd.[location], '--', '')),'') =  ISNULL(REPLACE(lmbd.[Location], '--', ''),'')
			           AND ISNULL(PartID.val,'') = ISNULL(PartID_lmdb.val,'')
			           AND (ISNULL(lmd.Attachment,'') + ' ' + ISNULL(lmd.Template,'')) = ISNULL(lmbd.Attachment, '')

                        UNION ALL


						SELECT
						[ST_MC_Type] = ISNULL(olmbd.MachineTypeID,''),
						[Motion] = ISNULL(Operation_P06.val,''),
						[Group_Header] =  ISNULL(IIF(REPLACE(tsd.[location], '--', '') = '', REPLACE(OP.OperationID, '--', '') ,REPLACE(tsd.[location], '--', '')),''),
						[Part] = PartID.val, --ISNULL(lmd.SewingMachineAttachmentID,''),
						[Attachment] = ISNULL(olmbd.Attachment,'') + ' ' + ISNULL(olmbd.Template,'')
						,olmbd.GSD
						,[Cycle] = olmbd.Cycle * olmbd.SewerDiffPercentage

						FROM Employee e WITH(NOLOCK)
						INNER JOIN LineMappingBalancing_Detail olmbd WITH(NOLOCK) ON olmbd.EmployeeID = e.ID
						INNER JOIN LineMappingBalancing lmb_Day WITH(NOLOCK) ON lmb_Day.id = olmbd.ID  AND ((lmb_Day.EditDate >= DATEADD(YEAR, -3, GETDATE()) AND lmb_Day.EditDate <= GETDATE()) OR (lmb_Day.AddDate >= DATEADD(YEAR, -3, GETDATE()) AND lmb_Day.AddDate <= GETDATE()))
						OUTER APPLY (
							SELECT val = STUFF((
								SELECT DISTINCT CONCAT(',', Name)
								FROM OperationRef a WITH(NOLOCK)
								INNER JOIN IESELECTCODE b WITH(NOLOCK) ON a.CodeID = b.ID AND a.CodeType = b.Type
								WHERE a.CodeType = '00007' AND a.id = olmbd.OperationID  
								FOR XML PATH('')), 1, 1, '')
						) Operation_P06
						OUTER APPLY(
							select TOP 1 tsd.Location,tsd.ID
							from TimeStudy TS
							inner join TimeStudy_Detail tsd WITH(NOLOCK) ON tsd.id = ts.id
							where TS.StyleID = lmb_Day.StyleID AND TS.SeasonID = lmb_Day.SeasonID AND TS.ComboType = lmb_Day.ComboType AND TS.BrandID = lmb_Day.BrandID
							and olmbd.OperationID = tsd.OperationID
						)tsd
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
							WHERE ID =  tsd.ID AND SEQ <= (SELECT TOP 1 Seq FROM TimeStudy_Detail WHERE id = tsd.ID AND OperationID = olmbd.OperationID ORDER BY Seq DESC)
							ORDER BY SEQ DESC
						)OP
						OUTER APPLY
						(
							SELECT val = R.[NAME]
							FROM Operation O WITH(NOLOCK)
							LEFT JOIN Reason R WITH(NOLOCK) ON R.ReasonTypeID = 'IE_Component' AND R.ID = SUBSTRING(O.ID,6,2)
							WHERE O.ID = olmbd.OperationID 
						)PartID
				        WHERE 
			            e.FactoryID IN (select ID from Factory where FTYGroup = lmb.FactoryID)  
			            AND e.ID = lmbd.EmployeeID 
			            AND ISNULL(olmbd.MachineTypeID,'') = ISNULL(lmbd.MachineTypeID,'')
			            AND ISNULL(Operation_P06.val,'') = ISNULL(Motion.val,'')
			            AND ISNULL(REPLACE(olmbd.[location], '--', ''),'') = ISNULL(REPLACE(lmbd.[Location], '--', ''),'')
			            AND ISNULL(PartID.val,'') = ISNULL(PartID_lmdb.val,'')
			            AND (ISNULL(lmbd.Attachment,'') + ' ' + ISNULL(lmbd.Template,'')) = ISNULL(olmbd.Attachment, '')
                    ) a
                    GROUP BY [ST_MC_Type], [Motion], [Group_Header], [Part], [Attachment]
                ) Effi
                WHERE lmb.ID = @LMBID AND ISNULL(md.IsNotShownInP06, 0) = 0
            ) a
            ORDER BY [No];

            SELECT 
             [No]
            ,[OperatorID]
            ,[ActualEffi] = CAST(IIF(OperatorID = '', 0, AVG(GSD / NULLIF(CYCLE, 0) * 100)) AS NUMERIC(7,2))
            ,[OperatorEffi] = CAST(IIF(OperatorID = '',0, AVG(OperatorEffi) * 100) AS NUMERIC(7,2))
            FROM #tmp 
            GROUP BY [No],OperatorID
            ORDER BY [No]
			drop table #tmp
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
            this.chart1.Series["Actual Effi%"].IsValueShownAsLabel = true;

            this.chart1.Series.Add("Est Effi%");
            this.chart1.Series["Est Effi%"].ChartType = SeriesChartType.Line;
            this.chart1.Series["Est Effi%"].Color = Color.Brown;
            this.chart1.Series["Est Effi%"].BorderWidth = 3;
            this.chart1.Series["Est Effi%"].IsValueShownAsLabel = true;

            // 傳值進去前面設定的Series
            for (int i = 0; i < dtChart.Rows.Count; i++)
            {
                string operarorID = dtChart.Rows[i]["OperatorID"].ToString();
                chartArea.AxisX.CustomLabels.Add(i - 0.4, i + 0.4, operarorID); // X軸的文字
                this.chart1.Series[0].Points.AddXY(i, MyUtility.Convert.GetDecimal(dtChart.Rows[i]["ActualEffi"]));
                this.chart1.Series[1].Points.AddXY(i, MyUtility.Convert.GetDecimal(dtChart.Rows[i]["OperatorEffi"]));
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
