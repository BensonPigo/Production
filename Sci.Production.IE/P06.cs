using Ict;
using Ict.Win;
using Ict.Win.Tools;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Smo.Agent;
using Sci.Data;
using Sci.Production.CallPmsAPI;
using Sci.Production.Class;
using Sci.Production.Class.Command;
using Sci.Production.PublicForm;
using Sci.Win.Tools;
using Sci.Win.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static Ict.Win.UI.DataGridView;
using static Sci.Production.IE.AutoLineMappingGridSyncScroll;

namespace Sci.Production.IE
{
    /// <summary>
    /// P06
    /// </summary>
    public partial class P06 : Sci.Win.Tems.Input6
    {
        private P06_NotHitTargetReason P06_NotHitTargetReason;
        private DataTable EmployeeData;
        private AutoLineMappingGridSyncScroll lineMappingGrids;
        private AutoLineMappingGridSyncScroll centralizedPPAGrids;

        private decimal StandardLBR
        {
            get
            {
                if (this.CurrentMaintain == null)
                {
                    return 0;
                }

                string sqlGetLBRCondition = $@"
SELECT ALMCS.Condition1 
FROM AutomatedLineMappingConditionSetting ALMCS
WHERE ALMCS.[FactoryID] = '{this.CurrentMaintain["FactoryID"]}'
AND ALMCS.Functions = 'IE_P06'
AND ALMCS.Verify = 'LBRByCycle'
AND ALMCS.Junk = 0
";
                return MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup(sqlGetLBRCondition));
            }
        }

        /// <summary>
        /// P06
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P06(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            MyUtility.Tool.SetupCombox(this.comboPhase, 2, 1, ",,Final,Final");
            this.gridicon.Append.Visible = false;

            this.splitLineMapping.Panel1.Controls.Add(this.detailgrid);
            this.gridCentralizedPPALeft.SupportEditMode = Win.UI.AdvEditModesReadOnly.True;
            this.gridCentralizedPPALeft.DataSource = this.gridCentralizedPPALeftBS;

            this.detailgrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.gridCentralizedPPALeft.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.detailgrid.CellFormatting += this.Detailgrid_CellFormatting;

            this.lineMappingGrids = new AutoLineMappingGridSyncScroll(this.detailgrid, this.gridLineMappingRight, "No", SubGridType.LineMappingBalancing);
            this.centralizedPPAGrids = new AutoLineMappingGridSyncScroll(this.gridCentralizedPPALeft, this.gridCentralizedPPARight, "No", SubGridType.BalancingCentrailizedPPA);

            this.txtSewingline.FactoryobjectName = this.txtfactory;

            this.numericLBRByCycleTime.ValueChanged += this.NumericLBRByCycleTime_ValueChanged;
            this.masterpanel.Height = this.masterpanel.Controls.Cast<Control>().Max(c => c.Bottom);

            this.gridicon.Location = new System.Drawing.Point(1339, 200);
        }

        /// <summary>
        /// ShowDirectQueryID
        /// </summary>
        /// <param name="id">id</param>
        public void ShowDirectQueryID(string id)
        {
            this.Show();
            this.ReloadDatas();
            this.tabs.SelectedIndex = 0;

            foreach (DataRow dr in this.DataRows)
            {
                if (dr["ID"].ToString() == id)
                {
                    int targetIndex = this.grid.GetRowIndexByDataRow(dr);
                    this.grid.SelectRowTo(targetIndex);
                    this.tabs.SelectedIndex = 1;
                    return;
                }
            }
        }

        private void NumericLBRByCycleTime_ValueChanged(object sender, EventArgs e)
        {
            if (this.numericLBRByCycleTime.Value == null)
            {
                return;
            }

            if (this.numericLBRByCycleTime.Value < this.StandardLBR)
            {
                this.numericLBRByCycleTime.BackColor = Color.PaleVioletRed;
            }
            else
            {
                this.numericLBRByCycleTime.BackColor = this.numericHighestGSDTime.BackColor;
            }
        }

        private void Detailgrid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
            if (dr["OperationID"].ToString() == "PROCIPF00003" ||
                dr["OperationID"].ToString() == "PROCIPF00004")
            {
                this.detailgrid.Rows[e.RowIndex].ReadOnly = true;
                return;
            }

            if (e.ColumnIndex > 1)
            {
                e.CellStyle.BackColor = MyUtility.Convert.GetInt(dr["TimeStudyDetailUkeyCnt"]) > 1 ? Color.FromArgb(255, 255, 153) : this.detailgrid.DefaultCellStyle.BackColor;
            }

            if (this.detailgrid.Columns[e.ColumnIndex].DataPropertyName == "Selected")
            {
                e.CellStyle.BackColor = this.detailgrid.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly ? Color.LightGray : this.detailgrid.DefaultCellStyle.BackColor;
            }
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            MyUtility.Tool.SetupCombox(this.queryfors, 1, 1, "last 2 years modify,All");
            this.queryfors.SelectedIndex = 0;
            if (MyUtility.Check.Empty(this.DefaultWhere) && MyUtility.Check.Empty(this.DefaultFilter))
            {
                this.DefaultWhere = " AddDate >= DATEADD(YY,-2,GETDATE()) OR EditDate >= DATEADD(YY,-2,GETDATE())";
            }
            else
            {
                this.queryfors.SelectedIndex = 1;
            }

            this.queryfors.SelectedIndexChanged += (s, e) =>
            {
                switch (this.queryfors.SelectedIndex)
                {
                    case 0:
                        this.DefaultWhere = " AddDate >= DATEADD(YY,-2,GETDATE()) OR EditDate >= DATEADD(YY,-2,GETDATE())";
                        break;
                    case 1:
                    default:
                        this.DefaultWhere = string.Empty;
                        break;
                }

                // 請參考IE P01註解
                if (this.QBCommand != null && this.QBCommand.Conditions.Count() == 0)
                {
                    this.QueryExpress = string.Empty;
                }

                this.ReloadDatas();

                System.GC.Collect();
            };
        }

        /// <inheritdoc/>
        protected override bool ClickPrint()
        {
            //new P06_Print(this.CurrentMaintain["ID"].ToString());
            new P06_Print(this.CurrentMaintain["ID"].ToString(), this.oriDataTable).ShowDialog();
            return base.ClickPrint();
        }

        /// <inheritdoc/>
        protected override void ClickUndo()
        {
            base.ClickUndo();
            this.RefreshLineMappingBalancingSummary();
            foreach (DataRow dr in this.DetailDatas)
            {
                dr["Selected"] = false;
            }

            this.OnRefreshClick();
        }

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();
            string factoryID = (e.Master == null) ? string.Empty : e.Master["FactoryID"].ToString();
            this.DetailSelectCommand =
                $@"
                SELECT * 
                FROM (
                    SELECT *, [IsRow] = ROW_NUMBER() OVER(PARTITION BY EmployeeID, Ukey ORDER BY Junk ASC)
                    FROM (
                        SELECT 
                            cast(0 as bit) as Selected
                            ,ad.ukey
                            ,ad.[ID]
                            ,[No]
                            ,ad.[Seq]
                            ,[Location]
                            ,[PPA]
                            ,ad.[MachineTypeID]
                            ,ad.[MasterPlusGroup]
                            ,[OperationID]
                            ,ad.[Annotation] 
                            ,ad.[Attachment]
                            ,[SewingMachineAttachmentID]
                            ,[Template]
                            ,[GSD]
                            ,[Cycle]
                            ,[SewerDiffPercentage]
                            ,[DivSewer]  = iif(isAdd = 1, null,[DivSewer])
                            ,[OriSewer]  = iif(isAdd = 1, null,[OriSewer])
                            ,[TimeStudyDetailUkey]
                            ,[ThreadComboID]
                            ,[Notice]
                            ,[EmployeeID]
                            ,ad.[IsNonSewingLine]
                            ,[IsAdd],
                            [PPADesc] = isnull(d.Name, ''),
                            [OperationDesc] = iif(isnull(op.DescEN, '') = '', ad.OperationID, op.DescEN),
                            [SewerDiffPercentageDesc] = round(ad.SewerDiffPercentage * 100, 0),
                            [TimeStudyDetailUkeyCnt] = Count(TimeStudyDetailUkey) over (partition by TimeStudyDetailUkey),
                            [EmployeeName] = e.Name,
                            [EmployeeSkill] = e.Skill,
                            [IsNotShownInP06] = isnull(md.IsNotShownInP06,0),
			                [Junk] = e.junk,
			                [EstCycleTime] = isnull(iif(Effi.Effi_3_year = '' or Effi.Effi_3_year is null ,ad.GSD  / Effi_90_day.Effi_90_day,ad.GSD  / Effi.Effi_3_year) ,0)
                        FROM LineMappingBalancing_Detail ad WITH (NOLOCK)
                        LEFT JOIN DropDownList d WITH (NOLOCK) ON d.ID = ad.PPA AND d.Type = 'PMS_IEPPA'
                        LEFT JOIN Operation op WITH (NOLOCK) ON op.ID = ad.OperationID
                        LEFT JOIN Employee e WITH (NOLOCK) ON e.FactoryID = '{factoryID}' AND e.ID = ad.EmployeeID
                        INNER JOIN LineMappingBalancing lmb ON lmb.ID = ad.ID
                        LEFT JOIN MachineType_Detail md ON md.ID = ad.MachineTypeID AND md.FactoryID = lmb.FactoryID
                        OUTER APPLY
	                    (
		                    select val = stuff((select distinct concat(',',Name)
		                    from OperationRef a
		                    inner JOIN IESELECTCODE b WITH(NOLOCK) on a.CodeID = b.ID and a.CodeType = b.Type
		                    where a.CodeType = '00007' and a.id = ad.OperationID  for xml path('') ),1,1,'')
	                    )Motion
		                OUTER APPLY
		                (
			                SELECT
                            [ST_MC_Type]
                            ,[Motion]
                            ,[Group_Header]
                            ,[Part]
                            ,[Attachment]
                            ,[Effi_3_year] = isnull(FORMAT(AVG(CAST([Effi_3_year] AS DECIMAL(10, 2))), '0.00'),'0.00')
                            From
                            (
                                SELECT 
                                [ST_MC_Type] =lmd.MachineTypeID
                                ,[Motion] = Operation_P03.val
                                ,[Group_Header] = ISNULL(REPLACE(lmd.Location, '--', ''),'')
                                ,[Part] = lmd.SewingMachineAttachmentID
                                ,[Attachment] = lmd.Attachment
                                ,Effi_3_year = FORMAT(CAST(iif(lmd.Cycle = 0,0,ROUND(lmd.GSD/ lmd.Cycle,2)) AS DECIMAL(10, 2)), '0.00')
                                from Employee eo
                                left JOIN LineMappingBalancing_Detail lmd WITH(NOLOCK) on lmd.EmployeeID = eo.ID　
                                left JOIN LineMappingBalancing lm WITH(NOLOCK) on lm.id = lmd.ID
                                OUTER APPLY
                                (
                                select val = stuff((select distinct concat(',',Name)
		                                from OperationRef a
		                                inner JOIN IESELECTCODE b WITH(NOLOCK) on a.CodeID = b.ID and a.CodeType = b.Type
		                                where a.CodeType = '00007' and a.id = lmd.OperationID  for xml path('') ),1,1,'')
                                )Operation_P03
	                            WHERE 
	                            eo.FactoryID = e.FactoryID and eo.ID = ad.EmployeeID AND
			                    lmd.MachineTypeID = ad.MachineTypeID and
			                    Operation_P03.val = Motion.val AND
			                    ISNULL(lmd.Attachment,'') = ISNULL(ad.Attachment,'') AND
			                    ISNULL(lmd.SewingMachineAttachmentID,'') = ISNULL(ad.SewingMachineAttachmentID,'') AND
								ISNULL(REPLACE(lmd.Location, '--', ''),'') = ISNULL(REPLACE(ad.Location, '--', ''),'')  AND
		                        ((lm.EditDate >= DATEADD(DAY, -360, GETDATE()) and lm.EditDate <= GETDATE()) or (lm.AddDate >= DATEADD(DAY, -360, GETDATE()) and lm.AddDate <= GETDATE()))
			                )a
			                GROUP BY [ST_MC_Type],[Motion], [Group_Header], [Part], [Attachment]
		                )Effi
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
		                        ,Effi_90_day = FORMAT(CAST(iif(lmd.Cycle = 0,0,ROUND(lmd.GSD/ lmd.Cycle,2)) AS DECIMAL(10, 2)), '0.00')
		                        from Employee eo
		                        left JOIN LineMappingBalancing_Detail lmd WITH(NOLOCK) on lmd.EmployeeID = eO.ID　
		                        left JOIN LineMappingBalancing lm WITH(NOLOCK) on lm.id = lmd.ID
		                        OUTER APPLY
		                        (
			                        select val = stuff((select distinct concat(',',Name)
			                        from OperationRef a
			                        inner JOIN IESELECTCODE b WITH(NOLOCK) on a.CodeID = b.ID and a.CodeType = b.Type
			                        where a.CodeType = '00007' and a.id = lmd.OperationID  for xml path('') ),1,1,'')
		                        )Operation_P03
		                        WHERE 
		                        eo.FactoryID = e.FactoryID and eo.ID = ad.EmployeeID AND
			                    lmd.MachineTypeID = ad.MachineTypeID and
			                    Operation_P03.val = Motion.val AND
		                        ((lm.EditDate >= DATEADD(day, -90, GETDATE()) and lm.EditDate <= GETDATE()) or (lm.AddDate >= DATEADD(day, -90, GETDATE()) and lm.AddDate <= GETDATE()))
	                        )a
	                        GROUP BY [ST_MC_Type],[Motion]
		                )Effi_90_day
                        WHERE ad.ID = '{masterID}'
                    ) a
                ) b
                WHERE b.IsRow = 1 and b.No != '' and b.IsNotShownInP06 = 'False'
                ORDER BY IIF(No = '', 'ZZ', No), Seq
";
            return base.OnDetailSelectCommandPrepare(e);
        }

        /// <inheritdoc/>
        protected override DualResult OnRenewDataDetailPost(RenewDataPostEventArgs e)
        {
            this.P06_NotHitTargetReason = new P06_NotHitTargetReason(e.Master["ID"].ToString(), e.Master["FactoryID"].ToString(), e.Master["Status"].ToString());
            if (this.P06_NotHitTargetReason.HasNotHitTargetReason)
            {
                this.btnNotHitTargetReason.Font = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold);
                this.btnNotHitTargetReason.ForeColor = Color.Blue;
            }
            else
            {
                this.btnNotHitTargetReason.Font = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Regular);
                this.btnNotHitTargetReason.ForeColor = Color.Black;
            }

            return base.OnRenewDataDetailPost(e);
        }

        private DataTable oriDataTable;

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.FilterGrid();
            this.detailgrid.ColumnHeadersHeight = this.gridLineMappingRight.ColumnHeadersHeight;
            this.gridCentralizedPPALeft.ColumnHeadersHeight = this.gridCentralizedPPARight.ColumnHeadersHeight;
            this.btnEditOperation.Enabled = this.tabDetail.SelectedIndex == 0 && this.EditMode;

            if (this.EditMode)
            {
                foreach (var col in this.gridCentralizedPPALeft.Columns)
                {
                    if (((Ict.Win.UI.IDataGridViewEditMode)col).IsEditingReadOnly == false)
                    {
                        ((DataGridViewColumn)col).DefaultCellStyle.ForeColor = Color.Red;
                    }
                }
            }
            else
            {
                foreach (DataGridViewColumn col in this.gridCentralizedPPALeft.Columns)
                {
                    col.DefaultCellStyle.ForeColor = Color.Black;
                }
            }

            var sumData = this.DetailDatas.AsEnumerable().Where(r => r["EstCycleTime"] != DBNull.Value && r["EstCycleTime"] != null).Sum(r => Convert.ToDecimal(r["EstCycleTime"]));
            var maxData = this.DetailDatas.AsEnumerable()
            .Where(r => r["EstCycleTime"] != DBNull.Value && r["EstCycleTime"] != null)
            .Select(r => Convert.ToDecimal(r["EstCycleTime"]))
            .DefaultIfEmpty()
            .Max();

            decimal? numEstLBRValue = 0;
            if (maxData != 0 && this.numericSewerManpower.Value != 0)
            {
                numEstLBRValue = sumData / maxData / this.numericSewerManpower.Value * 100;
            }

            this.numEstLBR.Value = numEstLBRValue;

            if (!MyUtility.Check.Empty(this.CurrentMaintain["TotalCycleTime"]) &&
                !MyUtility.Check.Empty(this.CurrentMaintain["SewerManpower"]) &&
                !MyUtility.Check.Empty(this.CurrentMaintain["WorkHour"]))
            {
                decimal decTotalGSD = MyUtility.Convert.GetDecimal(this.CurrentMaintain["TotalCycleTime"]);
                decimal decCurrentOperators = MyUtility.Convert.GetDecimal(this.CurrentMaintain["SewerManpower"]);
                decimal decWorkhour = MyUtility.Convert.GetDecimal(this.CurrentMaintain["WorkHour"]);

                decimal decTargetHr = 3600 * decCurrentOperators / decTotalGSD;
                decimal decDailyDemand_Shift = decTargetHr * decWorkhour;
                this.CurrentMaintain["TaktTime"] = Math.Round(3600 * decWorkhour / decDailyDemand_Shift, 2);
            }
            else
            {
                this.CurrentMaintain["TaktTime"] = 0;
            }
        }

        /// <inheritdoc/>
        protected override bool ClickEditBefore()
        {
            if (this.CurrentMaintain["Status"].ToString() == "Confirmed")
            {
                MyUtility.Msg.WarningBox("Confirmed can not edit");
                return false;
            }

            return base.ClickEditBefore();
        }

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtfactory.ReadOnly = this.CurrentMaintain["Version"].ToString() != "0";
            this.comboPhase.ReadOnly = true;
        }

        /// <inheritdoc/>
        protected override bool ClickDeleteBefore()
        {
            if (this.CurrentMaintain["AddName"].ToString() != Env.User.UserID)
            {
                MyUtility.Msg.WarningBox("This line mapping not created by yourself, can't delete this line mapping.");
                return false;
            }

            if (this.CurrentMaintain["Status"].ToString() == "Confirmed")
            {
                MyUtility.Msg.WarningBox("This line mapping already confirmed, can't delete this line mapping.");
                return false;
            }

            return base.ClickDeleteBefore();
        }

        /// <inheritdoc/>
        protected override DualResult ClickDeletePost()
        {
            string sqlDelete = $@"
delete LineMappingBalancing_NotHitTargetReason where ID = '{this.CurrentMaintain["ID"]}'
";
            DualResult result = DBProxy.Current.Execute(null, sqlDelete);

            if (!result)
            {
                return result;
            }

            return base.ClickDeletePost();
        }

        /// <inheritdoc/>
        protected override bool ClickCopyBefore()
        {
            if (this.CurrentMaintain["Status"].ToString() != "Confirmed")
            {
                MyUtility.Msg.WarningBox("This line mapping has not been confirmed and cannot be copied.");
                return false;
            }

            return base.ClickCopyBefore();
        }

        /// <inheritdoc/>
        protected override bool ClickCopy()
        {
            if (!base.ClickCopy())
            {
                return false;
            }

            foreach (DataRow dr in ((DataTable)this.detailgridbs.DataSource).Rows)
            {
                dr["ID"] = DBNull.Value;
                dr["Ukey"] = DBNull.Value;
            }

            this.CurrentMaintain["Status"] = "New";
            this.CurrentMaintain["Version"] = DBNull.Value;
            this.CurrentMaintain["AddName"] = Env.User.UserID;
            this.CurrentMaintain["EditName"] = string.Empty;
            this.CurrentMaintain["EditDate"] = DBNull.Value;
            this.CurrentMaintain["ID"] = DBNull.Value;

            return true;
        }

        /// <inheritdoc/>
        protected override void ClickConfirm()
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["Team"]))
            {
                MyUtility.Msg.WarningBox("[Team] cannot be empty.");
                return;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["SewingLineID"]))
            {
                MyUtility.Msg.WarningBox("[Sewing Line] cannot be empty.");
                return;
            }

            var listEmptyReason = this.P06_NotHitTargetReason.DataNotHitTargetReason.AsEnumerable().Where(s => MyUtility.Check.Empty(s["IEReasonID"]));

            if (listEmptyReason.Any())
            {
                MyUtility.Msg.WarningBox($"Not Hit Target Reason {listEmptyReason.Select(s => s["No"].ToString()).JoinToString(",")} has not yet input the reason, cannot be confirm.");
                this.P06_NotHitTargetReason.ShowDialog();
                return;
            }

            decimal checkLBRCondition = this.StandardLBR;

            if (checkLBRCondition > 0 &&
                MyUtility.Convert.GetDecimal(this.CurrentMaintain["LBRByCycleTime"]) < checkLBRCondition)
            {
                DialogResult dialogResult = CustomQuestionBox.ShowDialog($"[LBR By Cycle Time(%)] should not be lower than {checkLBRCondition}%, please double check and revise it, thanks!", string.Empty, "Confirm", "Close");

                if (dialogResult != DialogResult.OK)
                {
                    return;
                }
            }

            string sqlUpdate = $@"
update  LineMappingBalancing
set Status = 'Confirmed',
    EditName = '{Env.User.UserID}',
    EditDate = getdate(),
    CFMName = '{Env.User.UserID}',
    CFMDate = getdate()
where   ID = '{this.CurrentMaintain["ID"]}'
";
            DualResult result = DBProxy.Current.Execute(null, sqlUpdate);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            base.ClickConfirm();

            MyUtility.Msg.InfoBox("Confirm complete");
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            DataTable dt = this.DetailDatas.CopyToDataTable();
            var list = dt.AsEnumerable()
                .GroupBy(x => x["OperationID"].ToString())
                .Select(g => new
                {
                    No = g.First()["No"],
                    SumSewerDiffPercentageDesc = g.Sum(row => row.Field<decimal>("SewerDiffPercentageDesc")),
                })
                .Where(x => x.SumSewerDiffPercentageDesc < 100)
                .ToList();

            if (list.Count > 0)
            {
                string concatenatedNos = string.Join(", ", list.Select(x => x.No));
                MyUtility.Msg.WarningBox($@"Total % of Operation in No.{concatenatedNos} does not equal to 100%, cannot save!");
                return false;
            }

            int noCount = MyUtility.Convert.GetInt(this.CurrentMaintain["OriNoNumber"]);

            if (noCount + 5 <= this.DetailDatas.AsEnumerable().GroupBy(x => x["No"].ToString()).Count())
            {
                MyUtility.Msg.WarningBox("Please fill in <Reason>!");
                return false;
            }

            if (noCount - 5 > this.DetailDatas.AsEnumerable().GroupBy(x => x["No"].ToString()).Count())
            {
                MyUtility.Msg.WarningBox("Please fill in <Reason>!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["FactoryID"]))
            {
                MyUtility.Msg.WarningBox("[Factory] cannot be empty.");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["Phase"]))
            {
                MyUtility.Msg.WarningBox("[Phase] cannot be empty.");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["Team"]))
            {
                MyUtility.Msg.WarningBox("[Team] cannot be empty.");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["SewingLineID"]))
            {
                MyUtility.Msg.WarningBox("[Sewing Line] cannot be empty.");
                return false;
            }

            this.detailgridbs.RemoveFilter();
            this.gridCentralizedPPALeftBS.RemoveFilter();

            int seqLineMapping = 1;
            int seqCentralizedPPA = 1;

            foreach (var dr in this.DetailDatas.OrderBy(s => s["No"].ToString()))
            {
                if (dr["PPA"].ToString() != "C" && MyUtility.Convert.GetBool(dr["IsNonSewingLine"]) == false)
                {
                    dr["Seq"] = seqLineMapping;
                    seqLineMapping++;
                }
                else if (dr["PPA"].ToString() == "C" && MyUtility.Convert.GetBool(dr["IsNonSewingLine"]) == false)
                {
                    dr["Seq"] = seqCentralizedPPA;
                    seqCentralizedPPA++;
                }
            }

            // 取version
            if (MyUtility.Convert.GetInt(this.CurrentMaintain["Version"]) == 0 ||
                this.CurrentMaintain["SewingLineID", DataRowVersion.Original].ToString() != this.CurrentMaintain["SewingLineID", DataRowVersion.Current].ToString() ||
                this.CurrentMaintain["Team", DataRowVersion.Original].ToString() != this.CurrentMaintain["Team", DataRowVersion.Current].ToString())
            {
                string sqlGetVersion = $@"
select [NewVersion] = isnull(max(Version), 0) + 1
from LineMappingBalancing with (nolock)
where   FactoryID = '{this.CurrentMaintain["FactoryID"]}' and
        StyleID = '{this.CurrentMaintain["StyleID"]}' and
        SeasonID = '{this.CurrentMaintain["SeasonID"]}' and
        BrandID = '{this.CurrentMaintain["BrandID"]}' and
        ComboType = '{this.CurrentMaintain["ComboType"]}' and
        SewingLineID = '{this.CurrentMaintain["SewingLineID"]}' and
        Team = '{this.CurrentMaintain["Team"]}'
";
                this.CurrentMaintain["Version"] = MyUtility.Convert.GetInt(MyUtility.GetValue.Lookup(sqlGetVersion));
            }

            // 有可能因為調整佔位順序造成HighestGSDTime不同，所以這邊要更新
            this.RefreshLineMappingBalancingSummary();

            // 要將PPA = 'C'的資料併回主Detail
            DataTable dtCentralizedPPA = (DataTable)this.gridCentralizedPPALeftBS.DataSource;

            if (dtCentralizedPPA.Rows.Count > 0)
            {
                foreach (DataRow drRemove in ((DataTable)this.detailgridbs.DataSource).AsEnumerable().Where(s => s["PPA"].ToString() == "C").ToList())
                {
                    ((DataTable)this.detailgridbs.DataSource).Rows.Remove(drRemove);
                }

                DataTable dtDetail = (DataTable)this.detailgridbs.DataSource;

                dtCentralizedPPA.MergeTo(ref dtDetail);
            }

            return base.ClickSaveBefore();
        }

        /// <summary>
        /// 把同No下其他項目也勾起來，並將其他有相同TimeStudyDetailUkey的No也勾起來
        /// </summary>
        /// <param name="no">no</param>
        /// <param name="isChecked">isChecked</param>
        private void SelectedSameTimeStudyDetailUkey(string no, bool isChecked)
        {
            var needChecked = this.DetailDatas.Where(row => row["No"].ToString() == no);
            List<string> listNoSyncSelected = new List<string>();

            foreach (var checkItem in needChecked)
            {
                checkItem["Selected"] = isChecked;

                var listNoForSameTimeStudyDetailUkey = this.DetailDatas
                                .Where(row => row["TimeStudyDetailUkey"].ToString() == checkItem["TimeStudyDetailUkey"].ToString() &&
                                              row["No"].ToString() != checkItem["No"].ToString() &&
                                              (bool)row["Selected"] != isChecked)
                                .Select(s => s["No"].ToString())
                                .ToList()
                                .Distinct();

                listNoSyncSelected.AddRange(listNoForSameTimeStudyDetailUkey);
            }

            foreach (string syncSelectedNo in listNoSyncSelected.Distinct())
            {
                this.SelectedSameTimeStudyDetailUkey(syncSelectedNo, isChecked);
            }
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();

            TxtMachineGroup.CelltxtMachineGroup colMachineTypeID = TxtMachineGroup.CelltxtMachineGroup.GetGridCell();
            DataGridViewGeneratorCheckBoxColumnSettings colSelected = new DataGridViewGeneratorCheckBoxColumnSettings();
            DataGridViewGeneratorMaskedTextColumnSettings colPPANo = new DataGridViewGeneratorMaskedTextColumnSettings();
            DataGridViewGeneratorNumericColumnSettings colCycleTime = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings colCycleTimePPA = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorTextColumnSettings colOperator_ID = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings colOperator_Name = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings operation = new DataGridViewGeneratorTextColumnSettings();

            DataGridViewGeneratorNumericColumnSettings percentage = new DataGridViewGeneratorNumericColumnSettings();

            percentage.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);

                DataTable dt = (DataTable)this.gridLineMappingRight.DataSource;

                var dataRow = dt.AsEnumerable()
                    .Select((row, index) => new { Row = row, Index = index })
                    .Where(x => x.Row["No"].ToString() == dr["No"].ToString())
                    .FirstOrDefault();

                if (dataRow != null && dataRow.Index >= 0 && dataRow.Index < this.gridLineMappingRight.Rows.Count)
                {
                    this.gridLineMappingRight.BeginInvoke(new Action(() =>
                    {
                        this.gridLineMappingRight.ClearSelection();
                        this.gridLineMappingRight.Rows[dataRow.Index].Selected = true;
                        var targetCell = this.gridLineMappingRight.Rows[dataRow.Index].Cells[0];
                        if (targetCell.Visible && this.gridLineMappingRight.CurrentCell != targetCell)
                        {
                            this.gridLineMappingRight.FirstDisplayedScrollingRowIndex = dataRow.Index;
                            this.gridLineMappingRight.CurrentCell = targetCell;
                        }
                    }));
                }

                decimal curpercentage = MyUtility.Convert.GetDecimal(e.FormattedValue);

                if (MyUtility.Convert.GetDecimal(dr["SewerDiffPercentageDesc"]) == curpercentage)
                {
                    return;
                }

                dr["SewerDiffPercentageDesc"] = e.FormattedValue;

                this.RefreshLineMappingBalancingSummary(false);
            };
            #region Operation Code
            operation.EditingMouseDown += (s, e) =>
            {
                DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                if (this.EditMode)
                {
                    if (e.Button == MouseButtons.Right && MyUtility.Convert.GetBool(dr["IsAdd"]) == true)
                    {
                        if (e.RowIndex != -1)
                        {
                            P01_SelectOperationCode callNextForm = new P01_SelectOperationCode();
                            DialogResult result = callNextForm.ShowDialog(this);
                            if (result == DialogResult.Cancel)
                            {
                                if (callNextForm.P01SelectOperationCode != null)
                                {
                                    string smv = MyUtility.GetValue.Lookup($@"SELECT ISNULL(SMV,0) from TimeStudy_Detail where OperationID = '{callNextForm.P01SelectOperationCode["ID"].ToString()}'");
                                    dr["GSD"] = MyUtility.Check.Empty(smv) ? 0 : (object)smv;
                                    dr["OperationDesc"] = callNextForm.P01SelectOperationCode["DescEN"].ToString();
                                    dr["MachineTypeID"] = callNextForm.P01SelectOperationCode["MachineTypeID"].ToString();
                                    dr["Template"] = MyUtility.GetValue.Lookup($"select dbo.GetParseOperationMold('{callNextForm.P01SelectOperationCode["MoldID"]}', 'Template')");
                                    dr["Annotation"] = callNextForm.P01SelectOperationCode["Annotation"].ToString();
                                    dr["MasterPlusGroup"] = callNextForm.P01SelectOperationCode["MasterPlusGroup"].ToString();
                                    dr.EndEdit();
                                }
                            }

                            if (result == DialogResult.OK)
                            {
                                string smv = MyUtility.GetValue.Lookup($@"SELECT ISNULL(SMV,0) from TimeStudy_Detail where OperationID = '{callNextForm.P01SelectOperationCode["ID"].ToString()}'");
                                dr["GSD"] = MyUtility.Check.Empty(smv) ? 0 : (object)smv;
                                dr["OperationDesc"] = callNextForm.P01SelectOperationCode["DescEN"].ToString();
                                dr["MachineTypeID"] = callNextForm.P01SelectOperationCode["MachineTypeID"].ToString();
                                dr["Template"] = MyUtility.GetValue.Lookup($"select dbo.GetParseOperationMold('{callNextForm.P01SelectOperationCode["MoldID"]}', 'Template')");
                                dr["Annotation"] = callNextForm.P01SelectOperationCode["Annotation"].ToString();
                                dr["MasterPlusGroup"] = callNextForm.P01SelectOperationCode["MasterPlusGroup"].ToString();
                                dr.EndEdit();
                            }
                        }
                    }
                }
            };
            #endregion

            colCycleTime.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);

                DataTable dt = (DataTable)this.gridLineMappingRight.DataSource;

                var dataRow = dt.AsEnumerable()
                    .Select((row, index) => new { Row = row, Index = index })
                    .Where(x => x.Row["No"].ToString() == dr["No"].ToString())
                    .FirstOrDefault();

                if (dataRow != null && dataRow.Index >= 0 && dataRow.Index < this.gridLineMappingRight.Rows.Count)
                {
                    this.gridLineMappingRight.BeginInvoke(new Action(() =>
                    {
                        this.gridLineMappingRight.ClearSelection();
                        this.gridLineMappingRight.Rows[dataRow.Index].Selected = true;
                        var targetCell = this.gridLineMappingRight.Rows[dataRow.Index].Cells[0];
                        if (targetCell.Visible && this.gridLineMappingRight.CurrentCell != targetCell)
                        {
                            this.gridLineMappingRight.FirstDisplayedScrollingRowIndex = dataRow.Index;
                            this.gridLineMappingRight.CurrentCell = targetCell;
                        }
                    }));
                }

                decimal curCycle = MyUtility.Convert.GetDecimal(e.FormattedValue);

                if (MyUtility.Convert.GetDecimal(dr["Cycle"]) == curCycle)
                {
                    return;
                }

                dr["Cycle"] = e.FormattedValue;

                this.RefreshLineMappingBalancingSummary(false);
            };

            colCycleTimePPA.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                DataRow dr = this.gridCentralizedPPALeft.GetDataRow<DataRow>(e.RowIndex);
                decimal curCycle = MyUtility.Convert.GetDecimal(e.FormattedValue);

                if (MyUtility.Convert.GetDecimal(dr["Cycle"]) == curCycle)
                {
                    return;
                }

                dr["Cycle"] = curCycle;

                if (!MyUtility.Check.Empty(dr["No"]))
                {
                    this.centralizedPPAGrids.RefreshSubData(false);
                }
            };

            colPPANo.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                DataRow dr = this.gridCentralizedPPALeft.GetDataRow<DataRow>(e.RowIndex);
                string ppaNo = e.FormattedValue.ToString();

                if (!MyUtility.Check.Empty(ppaNo))
                {
                    dr["No"] = ppaNo.PadLeft(2, '0');
                }
                else
                {
                    dr["No"] = string.Empty;
                }

                this.centralizedPPAGrids.RefreshSubData();
            };

            colSelected.HeaderAction = DataGridViewGeneratorCheckBoxHeaderAction.None;

            colSelected.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                bool isChecked = MyUtility.Convert.GetBool(e.FormattedValue);

                //// 第一筆勾選時，將上下各五筆No保留Enable，其餘disable不能勾選
                //if (isChecked && !this.DetailDatas.Any(row => (bool)row["Selected"]))
                //{
                //    var listNo = this.DetailDatas.OrderBy(row => row["No"]).Select(row => row["No"].ToString()).Distinct().ToList();
                //    int targetIndex = listNo.IndexOf(dr["No"].ToString());
                //    int skipCount = Math.Max(targetIndex - 5, 0); // 計算要跳過的元素個數
                //    int takeCount = (targetIndex < 6) ? targetIndex + 6 : 11; // 計算要取出的元素個數

                //    var listEnableNo = listNo.Skip(skipCount).Take(takeCount).Where(row => row != dr["No"].ToString());

                //    foreach (DataGridViewRow gridRow in this.detailgrid.Rows)
                //    {
                //        if (gridRow.Cells["No"].Value.ToString() == dr["No"].ToString())
                //        {
                //            continue;
                //        }

                //        gridRow.Cells["Selected"].ReadOnly = !listEnableNo.Contains(gridRow.Cells["No"].Value.ToString());
                //    }
                //}

                //this.SelectedSameTimeStudyDetailUkey(dr["No"].ToString(), isChecked);

                // 如果取消勾選之後，資料中沒有一筆勾選的情況，將每筆資料的Selected read only回復
                if (!isChecked && !this.DetailDatas.Any(row => (bool)row["Selected"]))
                {
                    foreach (DataGridViewRow gridRow in this.detailgrid.Rows)
                    {
                        gridRow.Cells["Selected"].ReadOnly = false;
                    }
                }

                this.detailgrid.Refresh();
            };

            #region Operator ID No. 和 Operator Name的按右鍵與Validating
            colOperator_ID.EditingMouseDown += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                if (e.Button != MouseButtons.Right)
                {
                    return;
                }

                if (MyUtility.Check.Empty(this.CurrentMaintain["FactoryID"]))
                {
                    MyUtility.Msg.WarningBox("Please input the [Factory] before input the [Operator ID]");
                    return;
                }

                DataGridView sourceGrid = ((DataGridViewColumn)s).DataGridView;

                if (e.RowIndex != -1)
                {
                    DataRow dr = sourceGrid.GetDataRow<DataRow>(e.RowIndex);

                    this.GetEmployee(null,null, this.CurrentMaintain["FactoryID"].ToString(), this.CurrentMaintain["SewingLineID"].ToString());
                    P03_Operator callNextForm = new P03_Operator(this.EmployeeData, MyUtility.Convert.GetString(this.CurrentMaintain["SewingLineID"]));
                    DialogResult result = callNextForm.ShowDialog(this);
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    DataTable dt = (DataTable)this.detailgridbs.DataSource;
                    DataRow[] errorDataRow = dt.Select($"EmployeeID = '{MyUtility.Convert.GetString(callNextForm.SelectOperator["ID"])}' and NO <> '{MyUtility.Convert.GetString(dr["No"])}'");
                    if (errorDataRow.Length > 0)
                    {
                        this.ReviseEmployeeToEmpty(dr);
                        MyUtility.Msg.WarningBox($"<{this.EmployeeData.Rows[0]["ID"]} {this.EmployeeData.Rows[0]["Name"]}> already been used in No.{MyUtility.Convert.GetString(errorDataRow[0]["No"])}!!");
                        return;
                    }

                    dr["EmployeeID"] = callNextForm.SelectOperator["ID"];
                    dr["EmployeeName"] = callNextForm.SelectOperator["Name"];
                    dr["EmployeeSkill"] = callNextForm.SelectOperator["Skill"];
                    dr.EndEdit();

                    foreach (DataRow drDetail in this.DetailDatas.Where(row => row["No"].ToString() == dr["No"].ToString()))
                    {
                        drDetail["EmployeeID"] = callNextForm.SelectOperator["ID"];
                        drDetail["EmployeeName"] = callNextForm.SelectOperator["Name"];
                        drDetail["EmployeeSkill"] = callNextForm.SelectOperator["Skill"];
                    }
                }
            };

            colOperator_ID.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                DataGridView sourceGrid = ((DataGridViewColumn)s).DataGridView;
                DataRow dr = sourceGrid.GetDataRow<DataRow>(e.RowIndex);

                if (MyUtility.Check.Empty(e.FormattedValue))
                {
                    this.ReviseEmployeeToEmpty(dr);
                    return;
                }

                if (MyUtility.Check.Empty(this.CurrentMaintain["FactoryID"]))
                {
                    MyUtility.Msg.WarningBox("Please input the [Factory] before input the [Operator ID]");
                    this.ReviseEmployeeToEmpty(dr);
                    return;
                }

                if (e.FormattedValue.ToString() != dr["EmployeeID"].ToString())
                {
                    this.GetEmployee(null, e.FormattedValue.ToString(), this.CurrentMaintain["FactoryID"].ToString(), this.CurrentMaintain["SewingLineID"].ToString());

                    DataTable dt = (DataTable)this.detailgridbs.DataSource;
                    DataRow[] errorDataRow = dt.Select($"EmployeeID = '{MyUtility.Convert.GetString(e.FormattedValue.ToString())}' and NO <> '{MyUtility.Convert.GetString(dr["No"])}'");
                    if (errorDataRow.Length > 0)
                    {
                        this.ReviseEmployeeToEmpty(dr);
                        MyUtility.Msg.WarningBox($"<{e.FormattedValue.ToString()} {this.EmployeeData.Rows[0]["Name"]}> already been used in No.{MyUtility.Convert.GetString(errorDataRow[0]["No"])}!!");
                        return;
                    }

                    if (this.EmployeeData.Rows.Count <= 0)
                    {
                        this.ReviseEmployeeToEmpty(dr);
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox(string.Format("< Employee ID: {0} > not found!!!", e.FormattedValue.ToString()));
                        return;
                    }
                    else
                    {
                        dr["EmployeeID"] = this.EmployeeData.Rows[0]["ID"];
                        dr["EmployeeName"] = this.EmployeeData.Rows[0]["Name"];
                        dr["EmployeeSkill"] = this.EmployeeData.Rows[0]["Skill"];
                        dr.EndEdit();

                        foreach (DataRow drDetail in this.DetailDatas.Where(row => row["No"].ToString() == dr["No"].ToString()))
                        {
                            drDetail["EmployeeID"] = this.EmployeeData.Rows[0]["ID"];
                            drDetail["EmployeeName"] = this.EmployeeData.Rows[0]["Name"];
                            drDetail["EmployeeSkill"] = this.EmployeeData.Rows[0]["Skill"];
                        }
                    }
                }
            };

            colOperator_Name.EditingMouseDown += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                if (e.Button != MouseButtons.Right)
                {
                    return;
                }

                if (MyUtility.Check.Empty(this.CurrentMaintain["FactoryID"]))
                {
                    MyUtility.Msg.WarningBox("Please input the [Factory] before input the [Operator ID]");
                    return;
                }

                DataGridView sourceGrid = ((DataGridViewColumn)s).DataGridView;

                if (e.RowIndex != -1)
                {
                    DataRow dr = sourceGrid.GetDataRow<DataRow>(e.RowIndex);

                    this.GetEmployee(null,null, this.CurrentMaintain["FactoryID"].ToString(), this.CurrentMaintain["SewingLineID"].ToString());
                    P03_Operator callNextForm = new P03_Operator(this.EmployeeData, MyUtility.Convert.GetString(this.CurrentMaintain["SewingLineID"]));
                    DialogResult result = callNextForm.ShowDialog(this);
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    DataTable dt = (DataTable)this.detailgridbs.DataSource;
                    DataRow[] errorDataRow = dt.Select($"EmployeeID = '{MyUtility.Convert.GetString(callNextForm.SelectOperator["ID"])}' and NO <> '{MyUtility.Convert.GetString(dr["No"])}'");
                    if (errorDataRow.Length > 0)
                    {
                        this.ReviseEmployeeToEmpty(dr);
                        MyUtility.Msg.WarningBox($"<{this.EmployeeData.Rows[0]["ID"]} {this.EmployeeData.Rows[0]["Name"]}> already been used in No.{MyUtility.Convert.GetString(errorDataRow[0]["No"])}!!");
                        return;
                    }

                    dr["EmployeeID"] = callNextForm.SelectOperator["ID"];
                    dr["EmployeeName"] = callNextForm.SelectOperator["Name"];
                    dr["EmployeeSkill"] = callNextForm.SelectOperator["Skill"];
                    dr.EndEdit();

                    foreach (DataRow drDetail in this.DetailDatas.Where(row => row["No"].ToString() == dr["No"].ToString()))
                    {
                        drDetail["EmployeeID"] = callNextForm.SelectOperator["ID"];
                        drDetail["EmployeeName"] = callNextForm.SelectOperator["Name"];
                        drDetail["EmployeeSkill"] = callNextForm.SelectOperator["Skill"];
                    }
                }
            };

            colOperator_Name.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                DataGridView sourceGrid = ((DataGridViewColumn)s).DataGridView;
                DataRow dr = sourceGrid.GetDataRow<DataRow>(e.RowIndex);

                if (MyUtility.Check.Empty(e.FormattedValue))
                {
                    this.ReviseEmployeeToEmpty(dr);
                    return;
                }

                if (MyUtility.Check.Empty(this.CurrentMaintain["FactoryID"]))
                {
                    MyUtility.Msg.WarningBox("Please input the [Factory] before input the [Operator ID]");
                    this.ReviseEmployeeToEmpty(dr);
                    return;
                }

                if (!e.FormattedValue.ToString().Contains(","))
                {
                    MyUtility.Msg.WarningBox(string.Format("< Employee Name: {0} > not found!!!", e.FormattedValue.ToString()));
                    this.ReviseEmployeeToEmpty(dr);
                    return;
                }

                if (e.FormattedValue.ToString() != dr["EmployeeName"].ToString())
                {
                    this.GetEmployee(e.FormattedValue.ToString(), null, this.CurrentMaintain["FactoryID"].ToString(), this.CurrentMaintain["SewingLineID"].ToString());
                    DataTable dt = (DataTable)this.detailgridbs.DataSource;
                    DataRow[] errorDataRow = dt.Select($"EmployeeName = '{MyUtility.Convert.GetString(e.FormattedValue.ToString())}' and NO <> '{MyUtility.Convert.GetString(dr["No"])}'");
                    if (errorDataRow.Length > 0)
                    {
                        this.ReviseEmployeeToEmpty(dr);
                        MyUtility.Msg.WarningBox($"<{this.EmployeeData.Rows[0]["ID"]} {this.EmployeeData.Rows[0]["Name"]}> already been used in No.{MyUtility.Convert.GetString(errorDataRow[0]["No"])}!!");
                        return;
                    }

                    if (this.EmployeeData.Rows.Count <= 0)
                    {
                        this.ReviseEmployeeToEmpty(dr);
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox(string.Format("< Employee Name : {0} > not found!!!", e.FormattedValue.ToString()));
                        return;
                    }
                    else
                    {
                        dr["EmployeeID"] = this.EmployeeData.Rows[0]["ID"];
                        dr["EmployeeName"] = this.EmployeeData.Rows[0]["Name"];
                        dr["EmployeeSkill"] = this.EmployeeData.Rows[0]["Skill"];
                        dr.EndEdit();

                        foreach (DataRow drDetail in this.DetailDatas.Where(row => row["No"].ToString() == dr["No"].ToString()))
                        {
                            drDetail["EmployeeID"] = this.EmployeeData.Rows[0]["ID"];
                            drDetail["EmployeeName"] = this.EmployeeData.Rows[0]["Name"];
                            drDetail["EmployeeSkill"] = this.EmployeeData.Rows[0]["Skill"];
                        }
                    }
                }
            };
            #endregion

            this.Helper.Controls.Grid.Generator(this.detailgrid)
               .Text("No", header: "No", width: Widths.AnsiChars(4), iseditingreadonly: true)
               .CheckBox("Selected", string.Empty, trueValue: true, falseValue: false, iseditable: true, settings: colSelected)
               .Text("Location", header: "Location", width: Widths.AnsiChars(13), iseditingreadonly: true)
               .Text("PPADesc", header: "PPA", width: Widths.AnsiChars(5), iseditingreadonly: true)
               .CellMachineType("MachineTypeID", "ST/MC\r\ntype", this, width: Widths.AnsiChars(2))
               .Text("MasterPlusGroup", header: "MC Group", width: Widths.AnsiChars(10), settings: colMachineTypeID)
               .Text("OperationDesc", header: "Operation", width: Widths.AnsiChars(13), iseditingreadonly: true, settings: operation)
               .Text("Annotation", header: "Annotation", width: Widths.AnsiChars(25), iseditingreadonly: true)
               .CellAttachment("Attachment", "Attachment", this, width: Widths.AnsiChars(10))
               .CellPartID("SewingMachineAttachmentID", "Part ID", this, width: Widths.AnsiChars(10))
               .CellTemplate("Template", "Template", this, width: Widths.AnsiChars(10))
               .Numeric("GSD", header: "GSD Time", width: Widths.AnsiChars(5), decimal_places: 2, iseditingreadonly: true)
               .Numeric("EstCycleTime", header: "Est. Cycle Time", width: Widths.AnsiChars(5), decimal_places: 2, iseditingreadonly: true)
               .Numeric("Cycle", header: "Cycle Time", width: Widths.AnsiChars(5), decimal_places: 2, settings: colCycleTime)
               .Numeric("SewerDiffPercentageDesc", header: "%", width: Widths.AnsiChars(5), iseditingreadonly: false, settings: percentage)
               .Numeric("DivSewer", header: "Div. Sewer", decimal_places: 1, width: Widths.AnsiChars(5), iseditingreadonly: true)
               .Numeric("OriSewer", header: "Ori. Sewer", decimal_places: 1, width: Widths.AnsiChars(5), iseditingreadonly: true)
               .CellThreadComboID("ThreadComboID", "Thread" + Environment.NewLine + "Combination", this, width: Widths.AnsiChars(10))
               .EditText("Notice", header: "Notice", width: Widths.AnsiChars(40));

            this.Helper.Controls.Grid.Generator(this.gridCentralizedPPALeft)
               .MaskedText("No", "##", header: "PPA No.", width: Widths.AnsiChars(4), settings: colPPANo)
               .Text("Location", header: "Location", width: Widths.AnsiChars(13), iseditingreadonly: true)
               .CellMachineType("MachineTypeID", "ST/MC type", this, width: Widths.AnsiChars(10))
               .Text("MasterPlusGroup", header: "MC Group", width: Widths.AnsiChars(10), settings: colMachineTypeID)
               .Text("OperationDesc", header: "Operation", width: Widths.AnsiChars(13), iseditingreadonly: true)
               .Text("Annotation", header: "Annotation", width: Widths.AnsiChars(23), iseditingreadonly: true)
               .CellAttachment("Attachment", "Attachment", this, width: Widths.AnsiChars(10))
               .CellPartID("SewingMachineAttachmentID", "Part ID", this, width: Widths.AnsiChars(25))
               .CellTemplate("Template", "Template", this, width: Widths.AnsiChars(10))
               .Numeric("GSD", header: "GSD Time", decimal_places: 2, width: Widths.AnsiChars(5), iseditingreadonly: true)
               .Numeric("Cycle", header: "Cycle Time", width: Widths.AnsiChars(5), decimal_places: 2, settings: colCycleTimePPA)
               .EditText("Notice", header: "Notice", width: Widths.AnsiChars(40));

            this.Helper.Controls.Grid.Generator(this.gridLineMappingRight)
               .Text("No", header: "No. Of" + Environment.NewLine + "Station", width: Widths.AnsiChars(10), iseditingreadonly: true)
               .Text("sumGSDTime", header: "Total" + Environment.NewLine + "Cycle Time", width: Widths.AnsiChars(10), iseditingreadonly: true)
               .Text("TotalCycleTime", header: "Total Cycle " + Environment.NewLine + "Time by (%)", width: Widths.AnsiChars(10), iseditingreadonly: true)
               .Text("OperatorLoading", header: "Operator" + Environment.NewLine + "Loading (%)", width: Widths.AnsiChars(10), iseditingreadonly: true)
               .Text("EmployeeID", header: "Operator ID", width: Widths.AnsiChars(10), settings: colOperator_ID)
               .Text("EmployeeName", header: "Operator" + Environment.NewLine + "Name", width: Widths.AnsiChars(10),settings: colOperator_Name)
               //.Numeric("EstTotalCycleTime", header: "Est. Total Cycle Time", width: Widths.AnsiChars(5), decimal_places: 2, iseditingreadonly: true)
               .Text("EmployeeSkill", header: "Skill", width: Widths.AnsiChars(10), iseditingreadonly: true)
               .Text("OperatorEffi", header: "Effi (%)", width: Widths.AnsiChars(10), iseditingreadonly: true)
               .Numeric("EstOutputHr", header: "Est. Output/Hr", width: Widths.AnsiChars(5), decimal_places: 0, iseditingreadonly: true);

            this.Helper.Controls.Grid.Generator(this.gridCentralizedPPARight)
               .Text("No", header: "No. Of" + Environment.NewLine + "Station", width: Widths.AnsiChars(10), iseditingreadonly: true)
               .Text("TotalCycleTime", header: "Total" + Environment.NewLine + "Cycle Time", width: Widths.AnsiChars(10), iseditingreadonly: true)
               .Text("OperatorLoading", header: "Operator" + Environment.NewLine + "Loading (%)", width: Widths.AnsiChars(10), iseditingreadonly: true)
               .Text("EmployeeID", header: "Operator ID", width: Widths.AnsiChars(10), settings: colOperator_ID)
               .Text("EmployeeName", header: "Operator" + Environment.NewLine + "Name", width: Widths.AnsiChars(10), settings: colOperator_Name)
               .Text("EmployeeSkill", header: "Skill", width: Widths.AnsiChars(10), iseditingreadonly: true)
               .Text("OperatorEffi", header: "Effi (%)", width: Widths.AnsiChars(10), iseditingreadonly: true);

            this.gridLineMappingRight.Columns["EmployeeID"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridLineMappingRight.Columns["EmployeeName"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridCentralizedPPARight.Columns["EmployeeID"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridCentralizedPPARight.Columns["EmployeeName"].DefaultCellStyle.BackColor = Color.Pink;

            this.detailgrid.Columns["No"].Frozen = true;
            this.gridCentralizedPPALeft.Columns["No"].Frozen = true;
            this.gridLineMappingRight.Columns["No"].Frozen = true;
            this.gridCentralizedPPARight.Columns["No"].Frozen = true;
        }

        /// <inheritdoc/>
        protected override DualResult ClickSavePre()
        {
            // 因為這支程式在做某些操作時，Rowstate會亂掉，很難控制，所以直接在存檔前將資料庫的舊資料刪掉，再將detail rowstate都設定成Added來更新detail資料
            // 這樣確保最後存進資料庫的是畫面上的資料
            foreach (DataRow dr in this.DetailDatas)
            {
                dr.AcceptChanges();
                dr.SetAdded();
            }

            DualResult result = DBProxy.Current.Execute(null, $"delete LineMappingBalancing_Detail where id = '{this.CurrentMaintain["ID"]}'");

            if (!result)
            {
                return result;
            }

            return base.ClickSavePre();
        }

        private void TabDetail_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.CurrentMaintain == null)
            {
                return;
            }

            this.FilterGrid(false);
            this.btnEditOperation.Enabled = this.tabDetail.SelectedIndex == 0 && this.EditMode;
        }

        private void FilterGrid(bool needReloadPPA = true)
        {
            if (needReloadPPA || this.gridCentralizedPPALeftBS.DataSource == null)
            {
                this.gridCentralizedPPALeftBS.DataSource = ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
                .Where(s => s["PPA"].ToString() == "C").TryCopyToDataTable((DataTable)this.detailgridbs.DataSource);
            }

            if (this.tabDetail.SelectedIndex == 0)
            {
                this.detailgridbs.Filter = "PPA <> 'C' and IsNonSewingLine = 0";
                this.lineMappingGrids.RefreshSubData();
            }
            else
            {
                this.gridCentralizedPPALeftBS.Filter = "PPA = 'C' and IsNonSewingLine = 0";
                this.centralizedPPAGrids.RefreshSubData();
            }

            this.oriDataTable = (DataTable)this.gridLineMappingRight.DataSource;
            var dt = (DataTable)this.gridLineMappingRight.DataSource;
            foreach (DataRow dr in dt.Rows)
            {
                dr["NoCnt"] = MyUtility.Convert.GetInt(dr["NoCnt"]) - MyUtility.Convert.GetInt(dr["IsNotShownInP06Cnt"]);
            }

            this.gridLineMappingRightBS.DataSource = this.gridLineMappingRight.DataSource;
            this.gridLineMappingRightBS.Filter = "IsNotShownInP06 = 'False'";
            this.detailgridbs.Filter = "IsNotShownInP06 = 'False'"; // ISP20240132 隱藏工段
        }

        private void RefreshLineMappingBalancingSummary(bool isSort = true)
        {
            this.lineMappingGrids.RefreshSubData(isSort: isSort);
            if (this.lineMappingGrids.HighestGSD > 0)
            {
                this.CurrentMaintain["HighestGSDTime"] = this.lineMappingGrids.HighestGSD;
                this.CurrentMaintain["TotalGSDTime"] = this.lineMappingGrids.TotalGSD;
                this.CurrentMaintain["HighestCycleTime"] = this.lineMappingGrids.HighestCycle;
                this.CurrentMaintain["TotalCycleTime"] = this.lineMappingGrids.TotalCycle;
            }

            // LBRByGSDTime
            this.CurrentMaintain["LBRByGSDTime"] = MyUtility.Math.Round(MyUtility.Convert.GetDecimal(this.CurrentMaintain["TotalGSDTime"]) / MyUtility.Convert.GetDecimal(this.CurrentMaintain["HighestGSDTime"]) / MyUtility.Convert.GetDecimal(this.CurrentMaintain["SewerManpower"]) * 100, 0);

            // AvgGSDTime
            this.CurrentMaintain["AvgGSDTime"] = MyUtility.Math.Round(MyUtility.Convert.GetDecimal(this.CurrentMaintain["TotalGSDTime"]) / MyUtility.Convert.GetDecimal(this.CurrentMaintain["SewerManpower"]), 2);

            // LBRByCycleTime
            this.CurrentMaintain["LBRByCycleTime"] = MyUtility.Math.Round(MyUtility.Convert.GetDecimal(this.CurrentMaintain["TotalCycleTime"]) / MyUtility.Convert.GetDecimal(this.CurrentMaintain["HighestCycleTime"]) / MyUtility.Convert.GetDecimal(this.CurrentMaintain["SewerManpower"]) * 100, 0);

            // AvgCycleTime
            this.CurrentMaintain["AvgCycleTime"] = MyUtility.Math.Round(MyUtility.Convert.GetDecimal(this.CurrentMaintain["TotalCycleTime"]) / MyUtility.Convert.GetDecimal(this.CurrentMaintain["SewerManpower"]), 2);

            // TotalTimeDiff
            this.CurrentMaintain["TotalTimeDiff"] = MyUtility.Math.Round((MyUtility.Convert.GetDecimal(this.CurrentMaintain["TotalGSDTime"]) - MyUtility.Convert.GetDecimal(this.CurrentMaintain["TotalCycleTime"])) / MyUtility.Convert.GetDecimal(this.CurrentMaintain["TotalGSDTime"]) * 100, 0);

            // TotalSewingLineOptrs
            this.CurrentMaintain["TotalSewingLineOptrs"] = MyUtility.Convert.GetInt(this.CurrentMaintain["SewerManpower"]) + MyUtility.Convert.GetInt(this.CurrentMaintain["PresserManpower"]) + MyUtility.Convert.GetInt(this.CurrentMaintain["PackerManpower"]);

            // TargetHr
            this.CurrentMaintain["TargetHr"] = MyUtility.Math.Round(3600 * MyUtility.Convert.GetDecimal(this.CurrentMaintain["SewerManpower"]) / MyUtility.Convert.GetDecimal(this.CurrentMaintain["TotalCycleTime"]), 0);

            // DailyDemand
            //this.CurrentMaintain["DailyDemand"] = MyUtility.Math.Round(MyUtility.Convert.GetDecimal(this.CurrentMaintain["TargetHr"]) * MyUtility.Convert.GetDecimal(this.CurrentMaintain["WorkHour"]), 0);

            // this.CurrentMaintain["TaktTime"] = MyUtility.Math.Round(3600 * MyUtility.Convert.GetDecimal(this.CurrentMaintain["WorkHour"]) / MyUtility.Convert.GetDecimal(this.CurrentMaintain["DailyDemand"]), 2);

            decimal decTotalGSD = MyUtility.Convert.GetDecimal(this.CurrentMaintain["TotalCycleTime"]);
            decimal decCurrentOperators = MyUtility.Convert.GetDecimal(this.CurrentMaintain["SewerManpower"]);
            decimal decWorkhour = MyUtility.Convert.GetDecimal(this.CurrentMaintain["WorkHour"]);

            decimal decTargetHr = 3600 * decCurrentOperators / decTotalGSD;
            decimal decDailyDemand_Shift = decTargetHr * decWorkhour;

            this.CurrentMaintain["DailyDemand"] = Math.Round(decDailyDemand_Shift, 2);
            this.CurrentMaintain["TaktTime"] = Math.Round(3600 * decWorkhour / decDailyDemand_Shift, 2);

            // EOLR
            this.CurrentMaintain["EOLR"] = MyUtility.Math.Round(3600 / MyUtility.Convert.GetDecimal(this.CurrentMaintain["HighestCycleTime"]), 2);

            // PPH
            this.CurrentMaintain["PPH"] = MyUtility.Math.Round(MyUtility.Convert.GetDecimal(this.CurrentMaintain["EOLR"]) * MyUtility.Convert.GetDecimal(this.CurrentMaintain["StyleCPU"]) / MyUtility.Convert.GetDecimal(this.CurrentMaintain["SewerManpower"]), 2);

        }

        private void BtnNotHitTargetReason_Click(object sender, EventArgs e)
        {
            if (this.CurrentMaintain == null)
            {
                return;
            }

            this.P06_NotHitTargetReason.ShowDialog();
        }

        private void BtnEditOperation_Click(object sender, EventArgs e)
        {
            if (this.CurrentMaintain == null)
            {
                return;
            }

            if (!this.DetailDatas.Any(s => MyUtility.Convert.GetBool(s["Selected"])))
            {
                MyUtility.Msg.WarningBox("Please tick at least one Operation.");
                return;
            }

            // Employee欄位跟著No要keep住，這邊先保存一版修改前的資料，後面再用此資料復原Employee資訊
            var employeeInfoByNo = this.DetailDatas
                .Where(s => MyUtility.Convert.GetBool(s["Selected"]))
                .GroupBy(s => s["No"].ToString())
                .Select(s => new
                {
                    No = s.Key,
                    EmployeeID = s.First()["EmployeeID"].ToString(),
                    EmployeeName = s.First()["EmployeeName"].ToString(),
                    EmployeeSkill = s.First()["EmployeeSkill"].ToString(),
                })
                .ToList();

            using (P05_EditOperation p06_EditOperation = new P05_EditOperation((DataTable)this.detailgridbs.DataSource, false))
            {
                DialogResult dialogResult = p06_EditOperation.ShowDialog();
                if (dialogResult == DialogResult.OK)
                {
                    foreach (var itemEmployeeInfo in employeeInfoByNo)
                    {
                        foreach (DataRow drDetail in this.DetailDatas.Where(s => s["No"].ToString() == itemEmployeeInfo.No))
                        {
                            drDetail["EmployeeID"] = itemEmployeeInfo.EmployeeID;
                            drDetail["EmployeeName"] = itemEmployeeInfo.EmployeeName;
                            drDetail["EmployeeSkill"] = itemEmployeeInfo.EmployeeSkill;
                        }
                    }

                    this.RefreshLineMappingBalancingSummary();
                }
            }
        }

        private void BtnH_Click(object sender, EventArgs e)
        {
            if (this.CurrentMaintain == null)
            {
                return;
            }

            new P06_Default(this.CurrentMaintain["AutomatedLineMappingID"].ToString()).ShowDialog();
        }

        // 撈出Employee資料
        private void GetEmployee(string name, string iD, string factoryID, string sewinglineID)
        {

            string lastName = string.Empty;
            string firstName = string.Empty;
            if (!MyUtility.Check.Empty(name) && name.Contains(","))
            {
                string[] nameParts = name.Split(',');
                lastName = nameParts[0];
                firstName = nameParts[1];
            }

            string sqlCmd;

            string sqlWhere = string.Empty;

            if (!MyUtility.Check.Empty(iD))
            {
                sqlWhere += $" and e.ID = '{iD}' ";
            }

            if (!MyUtility.Check.Empty(factoryID))
            {
                sqlWhere += $" and e.FactoryID = '{factoryID}' ";
            }

            sqlCmd = $@"select 
                        e.ID
                        ,Name
                        ,FirstName
                        ,LastName
                        ,Section
                        ,Skill
                        ,SewingLineID
                        ,e.FactoryID 
                        from Employee e WITH (NOLOCK)
                        left join EmployeeAllocationSetting eas on e.FactoryID = eas.FactoryID and e.Dept = eas.Dept and e.Position = eas.Position 
                        where ResignationDate is null 
                        and e.FactoryID IN (select ID from Factory where FTYGroup = '{factoryID}')
                        and eas.P06 = 1 and e.Junk = 0"
                + sqlWhere
                + (MyUtility.Check.Empty(lastName) ? string.Empty : $@" and LastName = '{lastName}' ")
                + (MyUtility.Check.Empty(firstName) ? string.Empty : $@" and FirstName = '{firstName}'");

            DualResult result = DBProxy.Current.Select(null, sqlCmd, out this.EmployeeData);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Sql connection fail!!\r\n" + result.ToString());
            }
        }

        private void ReviseEmployeeToEmpty(DataRow dr)
        {
            dr["EmployeeID"] = string.Empty;
            dr["EmployeeName"] = string.Empty;
            dr["EmployeeSkill"] = string.Empty;
            dr.EndEdit();

            foreach (DataRow drDetail in this.DetailDatas.Where(row => row["No"].ToString() == dr["No"].ToString()))
            {
                drDetail["EmployeeID"] = string.Empty;
                drDetail["EmployeeName"] = string.Empty;
                drDetail["EmployeeSkill"] = string.Empty;
            }
        }

        private void TxtSewingline_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (MyUtility.Check.Empty(this.txtfactory.Text))
                {
                    MyUtility.Msg.WarningBox("Please fill in [Factory] first!");
                }
            }
        }

        private void BtnLBRbyCycleTime_Click(object sender, EventArgs e)
        {
            /* 修改時，Print也要一起修改，因為公式一模一樣 */
            new P06_Chart(MyUtility.Convert.GetString(this.CurrentMaintain["ID"])).ShowDialog();
        }

        private void BtnMachineSummary_Click(object sender, EventArgs e)
        {
            /* 修改時，Print也要一起修改，因為公式一模一樣 */
            new P06_MachineSummary(MyUtility.Convert.GetString(this.CurrentMaintain["ID"])).ShowDialog();
        }

        private void BrnOperatorSummary_Click(object sender, EventArgs e)
        {
            new P06_OperatorSummary(MyUtility.Convert.GetString(this.CurrentMaintain["ID"])).ShowDialog();
        }

        private void BtnPrintDetail_Click(object sender, EventArgs e)
        {
            string excelName = "IE_P06";

            this.detailgridbs.Filter = string.Empty;
            DataTable dtSheet1 = (DataTable)this.detailgridbs.DataSource;

            DataTable selectSheet1 = new DataTable();
            selectSheet1.Columns.Add("No", typeof(string));
            selectSheet1.Columns.Add("Location", typeof(string));
            selectSheet1.Columns.Add("PPADesc", typeof(string));
            selectSheet1.Columns.Add("MachineTypeID", typeof(string));
            selectSheet1.Columns.Add("MasterPlusGroup", typeof(string));
            selectSheet1.Columns.Add("OperationDesc", typeof(string));
            selectSheet1.Columns.Add("Annotation", typeof(string));
            selectSheet1.Columns.Add("Attachment", typeof(string));
            selectSheet1.Columns.Add("SewingMachineAttachmentID", typeof(string));
            selectSheet1.Columns.Add("Template", typeof(string));
            selectSheet1.Columns.Add("GSD", typeof(decimal));
            selectSheet1.Columns.Add("EstCycleTime", typeof(string));
            selectSheet1.Columns.Add("Cycle", typeof(decimal));
            selectSheet1.Columns.Add("SewerDiffPercentageDesc", typeof(string));
            selectSheet1.Columns.Add("DivSewer", typeof(string));
            selectSheet1.Columns.Add("OriSewer", typeof(string));
            selectSheet1.Columns.Add("ThreadComboID", typeof(string));
            selectSheet1.Columns.Add("Notice", typeof(string));

            DataTable selectSheet2 = new DataTable();
            selectSheet2.Columns.Add("No", typeof(string));
            selectSheet2.Columns.Add("sumGSDTime", typeof(string));
            selectSheet2.Columns.Add("TotalCycleTime", typeof(string));
            selectSheet2.Columns.Add("OperatorLoading", typeof(string));
            selectSheet2.Columns.Add("EmployeeID", typeof(string));
            selectSheet2.Columns.Add("EmployeeName", typeof(string));
            selectSheet2.Columns.Add("EstTotalCycleTime", typeof(string));
            selectSheet2.Columns.Add("EmployeeSkill", typeof(string));
            selectSheet2.Columns.Add("OperatorEffi", typeof(string));
            selectSheet2.Columns.Add("EstOutputHr", typeof(string));

            // 使用 LINQ 查询选择指定的列，并将结果添加到 DataTable 中
            foreach (var row in dtSheet1.AsEnumerable())
            {
                selectSheet1.Rows.Add(
                    row["No"].ToString(),
                    row["Location"].ToString(),
                    row["PPADesc"].ToString(),
                    row["MachineTypeID"].ToString(),
                    row["MasterPlusGroup"].ToString(),
                    row["OperationDesc"].ToString(),
                    row["Annotation"].ToString(),
                    row["Attachment"].ToString(),
                    row["SewingMachineAttachmentID"].ToString(),
                    row["Template"].ToString(),
                    row["GSD"].ToString(),
                    row["EstCycleTime"].ToString(),
                    row["Cycle"].ToString(),
                    row["SewerDiffPercentageDesc"].ToString(),
                    row["DivSewer"].ToString(),
                    row["OriSewer"].ToString(),
                    row["ThreadComboID"].ToString(),
                    row["Notice"].ToString()
                );
            }

            DataTable dtSheet2 = (DataTable)this.gridLineMappingRight.DataSource;

            foreach (var row in dtSheet2.AsEnumerable())
            {
                selectSheet2.Rows.Add(
                    row["No"].ToString(),
                    row["sumGSDTime"].ToString(),
                    row["TotalCycleTime"].ToString(),
                    row["OperatorLoading"].ToString(),
                    row["EmployeeID"].ToString(),
                    row["EmployeeName"].ToString(),
                    row["EstTotalCycleTime"].ToString(),
                    row["EmployeeSkill"].ToString(),
                    row["OperatorEffi"].ToString(),
                    row["EstOutputHr"].ToString());
            }

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + $"\\{excelName}.xltx"); // 預先開啟excel app
            MyUtility.Excel.CopyToXls(selectSheet1, string.Empty, showExcel: false, xltfile: $"{excelName}.xltx", headerRow: 1, excelApp: objApp, wSheet: objApp.Sheets[1]);
            MyUtility.Excel.CopyToXls(selectSheet2, string.Empty, showExcel: false, xltfile: $"{excelName}.xltx", headerRow: 1, excelApp: objApp, wSheet: objApp.Sheets[2]);
            objApp.Cells.EntireRow.AutoFit();

            Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)objApp.Sheets[1];

            // 設定 A 欄的寬度為自動
            Microsoft.Office.Interop.Excel.Range columnA = worksheet.Columns["A:A"];
            columnA.ColumnWidth = 7;

            objApp.Visible = true;
            Marshal.ReleaseComObject(objApp);
        }

        /// <summary>
        /// DetailGrid Insert、Append
        /// </summary>
        /// <param name="index">index</param>
        protected override void OnDetailGridInsert(int index = 0)
        {
            DataTable copyOriDataTable = ((DataTable)this.detailgridbs.DataSource).Copy();
            DataRow copyDR;
            DataRow seleceted_dataRow = copyOriDataTable.AsEnumerable().FirstOrDefault(x => MyUtility.Convert.GetBool(x["Selected"]) == true);

            int insert_index = 0; // 插入的指標

            if (seleceted_dataRow != null)
            {
                insert_index = copyOriDataTable.Rows.IndexOf(seleceted_dataRow);
                copyDR = seleceted_dataRow;
            }
            else
            {
                insert_index = index;
                copyDR = copyOriDataTable.Rows[index];
            }

            this.detailgrid.AllowUserToAddRows = false;
            base.OnDetailGridInsert(insert_index);

            DataTable oriDt = (DataTable)this.detailgridbs.DataSource;
            DataTable dataTableRight = (DataTable)this.gridLineMappingRight.DataSource;

            if (index >= 0)
            {
                bool isGo = false;

                #region 主表插入
                DataRow nextDataRow = oriDt.Rows[insert_index];
                DataRow dataRow_Location = insert_index == 0 ? oriDt.Rows[insert_index + 1] : oriDt.Rows[insert_index - 1];
                nextDataRow["Selected"] = "False";
                nextDataRow["IsAdd"] = "True";
                nextDataRow["DivSewer"] = DBNull.Value;
                nextDataRow["OriSewer"] = DBNull.Value;
                nextDataRow["No"] = insert_index == 0 ? "01" : oriDt.Rows[insert_index + 1]["No"];
                nextDataRow["IsNotShownInP06"] = false;
                nextDataRow["Location"] = dataRow_Location["Location"];

                List<DataRow> rowsToMainAdd = new List<DataRow>();

                if (MyUtility.Convert.GetBool(copyDR["Selected"]) == true)
                {
                    nextDataRow["No"] = insert_index == 0 ? "01" : oriDt.Rows[insert_index + 1]["No"];
                    foreach (DataRow dataRow in this.DetailDatas)
                    {
                        int indexs = oriDt.Rows.IndexOf(dataRow);
                        string strNo = MyUtility.Convert.GetString(dataRow["No"]);
                        if (MyUtility.Convert.GetString(nextDataRow["No"]) == strNo)
                        {
                            if (indexs != insert_index)
                            {
                                dataRow["No"] = MyUtility.Convert.GetString(MyUtility.Convert.GetInt(dataRow["No"]) + 1).PadLeft(2, '0');
                            }

                            isGo = true;
                            continue;
                        }

                        if (isGo)
                        {
                            dataRow["No"] = MyUtility.Convert.GetString(MyUtility.Convert.GetInt(dataRow["No"]) + 1).PadLeft(2, '0');
                        }
                    }

                    this.detailgridbs.DataSource = oriDt;
                }
                else
                {
                    nextDataRow["No"] = copyDR["No"];
                }

                int indexMain = 0;
                for (int i = 0; i < this.detailgrid.Rows.Count; i++)
                {
                    if (this.detailgrid.Rows[i].Cells["No"].Value.ToString() == (object)nextDataRow["No"])
                    {
                        indexMain = i;
                        break;
                    }
                }

                this.detailgrid.Rows[indexMain].Selected = true;
                this.detailgrid.CurrentCell = this.detailgrid.Rows[indexMain].Cells[0];
                this.detailgrid.FirstDisplayedScrollingRowIndex = indexMain;
                #endregion 主表插入

                #region 插入右邊表
                DataColumn noColumn = dataTableRight.Columns["No"];
                DataColumn[] primaryKeyColumns = dataTableRight.PrimaryKey;

                if (primaryKeyColumns.Length == 1 && primaryKeyColumns[0].ColumnName == "No")
                {
                    dataTableRight.PrimaryKey = null;
                }

                List<DataRow> rowsToAdd = new List<DataRow>();

                if (MyUtility.Convert.GetBool(copyDR["Selected"]))
                {
                    isGo = false;

                    foreach (DataRow dataRow in dataTableRight.Rows)
                    {
                        if ((string)dataRow["No"] == (string)nextDataRow["No"])
                        {
                            DataRow newRow = dataTableRight.NewRow();
                            newRow["No"] = nextDataRow["No"];
                            newRow["NoCnt"] = "1";
                            newRow["IsNotShownInP06Cnt"] = false;
                            newRow["NeedExclude"] = false;
                            newRow["IsNotShownInP06"] = false;

                            rowsToAdd.Add(newRow);

                            isGo = true;
                        }

                        if (isGo)
                        {
                            int iNo = MyUtility.Convert.GetInt(dataRow["No"]);
                            dataRow["No"] = MyUtility.Convert.GetString(iNo + 1).PadLeft(2, '0');
                        }
                    }

                    foreach (DataRow rowToAdd in rowsToAdd)
                    {
                        dataTableRight.Rows.Add(rowToAdd);
                    }
                }
                else
                {
                    DataRow dataRow = dataTableRight.AsEnumerable().FirstOrDefault(x => x["No"].ToString() == copyDR["No"].ToString());
                    if (dataRow != null)
                    {
                        dataRow["NoCnt"] = MyUtility.Convert.GetInt(dataRow["NoCnt"]) + 1;
                    }
                }

                DataView dataView = new DataView(dataTableRight);
                dataView.Sort = "No ASC";
                dataTableRight = dataView.ToTable();

                if (primaryKeyColumns.Length == 1 && primaryKeyColumns[0].ColumnName == "No")
                {
                    DataColumn newNoColumn = dataTableRight.Columns["No"];
                    dataTableRight.PrimaryKey = new DataColumn[] { newNoColumn };
                }

                this.gridLineMappingRight.DataSource = dataTableRight;

                string targetNo = rowsToAdd.Count > 0 ? rowsToAdd[0]["No"].ToString() : copyDR["No"].ToString();
                int indexRight = 0;
                for (int i = 0; i < this.gridLineMappingRight.Rows.Count; i++)
                {
                    if (this.gridLineMappingRight.Rows[i].Cells["No"].Value.ToString() == targetNo)
                    {
                        indexRight = i;
                        break;
                    }
                }

                this.gridLineMappingRight.ClearSelection();
                this.gridLineMappingRight.Rows[indexRight].Selected = true;
                this.gridLineMappingRight.CurrentCell = this.gridLineMappingRight.Rows[indexRight].Cells[0];
                this.gridLineMappingRight.FirstDisplayedScrollingRowIndex = indexRight;
                #endregion 插入右邊表
            }
            else if (index == -1)
            {
                var count = oriDt.AsEnumerable().Where(x => x.RowState != DataRowState.Deleted).Count();
                if (count == 1)
                {
                    this.CurrentDetailData["IsShow"] = 1;
                }
            }
        }

        /// <inheritdoc/>
        protected override void OnDetailGridRemoveClick()
        {
            if (this.CurrentMaintain == null || this.DetailDatas.Count == 0)
            {
                return;
            }

            DataTable dataTable = (DataTable)this.detailgridbs.DataSource;
            DataTable dataTableRight = (DataTable)this.gridLineMappingRight.DataSource;

            string row = string.Empty;

            DataRow seleceted_dataRow = dataTable.AsEnumerable().FirstOrDefault(x => x.RowState != DataRowState.Deleted && MyUtility.Convert.GetBool(x["Selected"]) == true);

            if (seleceted_dataRow != null)
            {
                row = seleceted_dataRow["No"].ToString();
            }
            else
            {
                row = dataTable.AsEnumerable().Where(x => x.RowState != DataRowState.Deleted).CopyToDataTable().Rows[this.detailgridbs.Position]["No"].ToString();
            }

            this.detailgrid.ValidateControl();

            DataRow dr = this.detailgrid.GetDataRow<DataRow>(this.detailgrid.CurrentRow.Index);

            //// Packer / Presser 卡控
            if (dr["OperationID"].ToString() == "PROCIPF00003" || dr["OperationID"].ToString() == "PROCIPF00004")
            {
                string manpowerKey = dr["OperationID"].ToString() == "PROCIPF00003" ? "PackerManpower" : "PresserManpower";
                string roleName = dr["OperationID"].ToString() == "PROCIPF00003" ? "Packer" : "Presser";

                if (!this.ValidateAndDecrementManpower(manpowerKey, roleName))
                {
                    return; // 如果無法刪除，則返回
                }
            }

            // base.OnDetailGridRemoveClick();
            bool isGo = false;
            bool isDes = false;

            int cnt = dataTable.AsEnumerable().Where(x => x.RowState != DataRowState.Deleted && x["No"].ToString() == dr["No"].ToString()).ToList().Count();

            if (MyUtility.Convert.GetBool(dr["Selected"]) == true || cnt == 1)
            {
                isDes = true;
            }

            #region 刪除右邊表
            if (isDes)
            {
                DataColumn noColumn = dataTableRight.Columns["No"];
                DataColumn[] primaryKeyColumns = dataTableRight.PrimaryKey;

                if (primaryKeyColumns.Length == 1 && primaryKeyColumns[0].ColumnName == "No")
                {
                    dataTableRight.PrimaryKey = null;
                }

                List<DataRow> rowsToDelete = new List<DataRow>();
                foreach (DataRow dataRow in dataTableRight.Rows)
                {
                    if (dataRow["No"].ToString() == row)
                    {
                        rowsToDelete.Add(dataRow);
                        isGo = true;
                        continue;
                    }

                    if (isGo && dataRow.RowState != DataRowState.Deleted)
                    {
                        int iNo = MyUtility.Convert.GetInt(dataRow["No"]);
                        dataRow["No"] = iNo == 1 ? "01" : MyUtility.Convert.GetString(iNo - 1).PadLeft(2, '0');
                    }
                }

                foreach (DataRow rowToDelete in rowsToDelete)
                {
                    rowToDelete.Delete();
                }

                // Sort and update DataTable
                DataView dataView = new DataView(dataTableRight);
                dataView.Sort = "No ASC";
                dataTableRight = dataView.ToTable();

                DataColumn newNoColumn = dataTableRight.Columns["No"];
                dataTableRight.PrimaryKey = new DataColumn[] { newNoColumn };
            }
            else
            {
                DataColumn noColumn = dataTableRight.Columns["No"];
                DataColumn[] primaryKeyColumns = dataTableRight.PrimaryKey;

                // 移除主鍵設置
                if (primaryKeyColumns.Length == 1 && primaryKeyColumns[0].ColumnName == "No")
                {
                    dataTableRight.PrimaryKey = null;
                }

                // 查找對應的 DataRow
                DataRow datarow = dataTableRight.AsEnumerable().FirstOrDefault(x => x["No"].ToString() == dr["No"].ToString());

                if (datarow != null)
                {
                    if (MyUtility.Convert.GetInt(datarow["NoCnt"]) > 1)
                    {
                        datarow["NoCnt"] = MyUtility.Convert.GetInt(datarow["NoCnt"]) - 1;
                    }
                    else
                    {
                        datarow.Delete();
                    }

                    // 確認刪除操作已完成
                    dataTableRight.AcceptChanges();
                }

                // 排序並更新 DataTable
                DataView dataView = new DataView(dataTableRight);
                dataView.Sort = "No ASC";
                dataTableRight = dataView.ToTable();

                // 使用新的 DataTable 設置主鍵
                DataColumn newNoColumn = dataTableRight.Columns["No"];
                dataTableRight.PrimaryKey = new DataColumn[] { newNoColumn };
            }
            #endregion 刪除右邊表

            isGo = false;
            #region 刪除主表資料
            List<DataRow> rowsToMainDelete = new List<DataRow>();

            if (isDes)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    if (dataRow.RowState == DataRowState.Deleted)
                    {
                        continue;
                    }

                    if (dataRow["No"].ToString() == row)
                    {
                        rowsToMainDelete.Add(dataRow);
                        isGo = true;
                        continue;
                    }

                    if (isGo)
                    {
                        int iNo = MyUtility.Convert.GetInt(dataRow["No"]);
                        dataRow["No"] = iNo == 1 ? "01" : MyUtility.Convert.GetString(iNo - 1).PadLeft(2, '0');
                    }
                }

                foreach (DataRow rowToDelete in rowsToMainDelete)
                {
                    rowToDelete.Delete();
                }
            }
            else
            {
                dr.Delete();
            }

            #endregion 刪除主表資料

            this.gridLineMappingRight.DataSource = dataTableRight;
            this.detailgridbs.DataSource = dataTable;
        }

        private bool ValidateAndDecrementManpower(string manpowerKey, string roleName)
        {
            int manpower = MyUtility.Convert.GetInt(this.CurrentMaintain[manpowerKey]);

            if ((manpower - 1) < 1)
            {
                MyUtility.Msg.WarningBox($"Cannot delete the last {roleName}");
                return false;
            }

            this.CurrentMaintain[manpowerKey] = manpower - 1;
            return true;
        }

        private void TxtReason_PopUp(object sender, TextBoxPopUpEventArgs e)
        {
            SelectItem sele = new SelectItem("select ID,Description from DropDownList where [Type] = 'PMS_IEReasonType'", "10,40", this.txtReason.Text);
            DialogResult result = sele.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.txtReason.Text = sele.GetSelectedString();
        }
    }
}
