using Ict;
using Ict.Win;

using Sci.Data;
using Sci.Production.CallPmsAPI;
using Sci.Production.Class;
using Sci.Production.Prg;
using Sci.Win.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static Sci.Production.IE.AutoLineMappingGridSyncScroll;

namespace Sci.Production.IE
{
    /// <summary>
    /// P05
    /// </summary>
    public partial class P05 : Sci.Win.Tems.Input6
    {
        private DataTable dtAutomatedLineMapping_DetailTemp = new DataTable();
        private DataTable dtAutomatedLineMapping_DetailAuto = new DataTable();
        private P05_NotHitTargetReason p05_NotHitTargetReason;

        private AutoLineMappingGridSyncScroll lineMappingGrids;
        private AutoLineMappingGridSyncScroll centralizedPPAGrids;

        private int iBarchart = 0;
        private Win.UI.Button[] chartLBRButtons = new Win.UI.Button[]
        {
                new Win.UI.Button(),
                new Win.UI.Button(),
                new Win.UI.Button(),
                new Win.UI.Button(),
                new Win.UI.Button(),
        };

        public string sqlGetAutomatedLineMapping_DetailAuto = @"
select  ad.*,
        [PPADesc] = isnull(d.Name, ''),
        [OperationDesc] = iif(isnull(op.DescEN, '') = '', ad.OperationID, op.DescEN),
        [SewerDiffPercentageDesc] = round(ad.SewerDiffPercentage * 100, 0),
        [TimeStudyDetailUkeyCnt] = Count(ad.TimeStudyDetailUkey) over (partition by ad.TimeStudyDetailUkey, ad.SewerManpower),
        [IsNotShownInP05] = isnull(md.IsNotShownInP05,0) 
from    AutomatedLineMapping_DetailAuto ad with (nolock) 
left join AutomatedLineMapping alm on alm.ID = ad.ID
left join MachineType_Detail md on md.ID = ad.MachineTypeID  and md.FactoryID = alm.FactoryID
left join DropDownList d with (nolock) on d.ID = ad.PPA  and d.Type = 'PMS_IEPPA'
left join Operation op with (nolock) on op.ID = ad.OperationID
where {0} and isnull(md.IsNotShownInP06,0) = 0
order by ad.SewerManpower, ad.No, ad.Seq
";

        public string sqlGetAutomatedLineMapping_DetailTemp = @"
select  ad.*,
        [PPADesc] = isnull(d.Name, ''),
        [OperationDesc] = iif(isnull(op.DescEN, '') = '', ad.OperationID, op.DescEN),
        [SewerDiffPercentageDesc] = round(ad.SewerDiffPercentage * 100, 0),
        [TimeStudyDetailUkeyCnt] = Count(ad.TimeStudyDetailUkey) over (partition by ad.TimeStudyDetailUkey, ad.SewerManpower),
        [IsNotShownInP05] = isnull(md.IsNotShownInP05,0) 
from    AutomatedLineMapping_DetailTemp ad with (nolock) 
left join AutomatedLineMapping alm on alm.ID = ad.ID
left join DropDownList d with (nolock) on d.ID = ad.PPA  and d.Type = 'PMS_IEPPA'
left join Operation op with (nolock) on op.ID = ad.OperationID
left join MachineType_Detail md on md.ID = ad.MachineTypeID  and md.FactoryID = alm.FactoryID

where {0}
order by ad.SewerManpower, ad.No, ad.Seq
";

        public string sqlGetAutomatedLineMapping_Detail = @"
select  cast(0 as bit) as Selected,
        ad.*,
        [PPADesc] = isnull(d.Name, ''),
        [OperationDesc] = iif(isnull(op.DescEN, '') = '', ad.OperationID, op.DescEN),
        [SewerDiffPercentageDesc] = round(ad.SewerDiffPercentage * 100, 0),
        [TimeStudyDetailUkeyCnt] = Count(TimeStudyDetailUkey) over (partition by TimeStudyDetailUkey),
		[IsNotShownInP05] = isnull(md.IsNotShownInP05,0) 
from AutomatedLineMapping_Detail ad WITH (NOLOCK)
left join DropDownList d with (nolock) on d.ID = ad.PPA  and d.Type = 'PMS_IEPPA'
left join Operation op with (nolock) on op.ID = ad.OperationID
inner join AutomatedLineMapping alm on alm.id = ad.ID
LEFT join MachineType_Detail md on md.ID = ad.MachineTypeID and md.FactoryID = alm.FactoryID
where {0} --and isnull(md.IsNotShownInP05,0) = 0
order by iif(ad.No = '', 'ZZ', ad.No), ad.Seq";

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
AND ALMCS.Functions = 'IE_P05'
AND ALMCS.Verify = 'LBRByGSD'
AND ALMCS.Junk = 0
";
                return MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup(sqlGetLBRCondition));
            }
        }

        /// <summary>
        /// P05
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P05(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            MyUtility.Tool.SetupCombox(this.comboPhase, 2, 1, ",,Initial,Initial,Prelim,Prelim");

            this.splitLineMapping.Panel1.Controls.Add(this.detailgrid);
            this.gridCentralizedPPALeft.SupportEditMode = Win.UI.AdvEditModesReadOnly.True;
            this.gridCentralizedPPALeft.DataSource = this.gridCentralizedPPALeftBS;

            this.gridLineMappingRight.DataSource = this.gridLineMappingRightBS.DataSource;

            this.detailgrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.gridCentralizedPPALeft.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;

            this.detailgrid.CellFormatting += this.Detailgrid_CellFormatting;

            this.lineMappingGrids = new AutoLineMappingGridSyncScroll(this.detailgrid, this.gridLineMappingRight, "No", SubGridType.LineMapping);
            this.centralizedPPAGrids = new AutoLineMappingGridSyncScroll(this.gridCentralizedPPALeft, this.gridCentralizedPPARight, "No", SubGridType.CentrailizedPPA);

            this.chartLBR.Controls.AddRange(this.chartLBRButtons);

            foreach (Win.UI.Button chartBtn in this.chartLBRButtons)
            {
                chartBtn.Size = new Size(27, 24);
                chartBtn.FlatStyle = FlatStyle.Flat;
                chartBtn.Font = new Font(FontFamily.GenericSerif, 7);
                chartBtn.Padding = new Padding(0, 0, 0, 0);
                chartBtn.BackColor = this.BackColor;
                chartBtn.Click += this.ChartBtn_Click;
            }

            // dtAutomatedLineMapping_DetailTemp, dtAutomatedLineMapping_DetailAuto給結構
            DualResult result = DBProxy.Current.Select(null, string.Format(this.sqlGetAutomatedLineMapping_DetailAuto, "1 = 0"), out this.dtAutomatedLineMapping_DetailAuto);
            if (!result)
            {
                this.ShowErr(result);
            }

            result = DBProxy.Current.Select(null, string.Format(this.sqlGetAutomatedLineMapping_DetailTemp, "1 = 0"), out this.dtAutomatedLineMapping_DetailTemp);
            if (!result)
            {
                this.ShowErr(result);
            }

            this.chartLBR.Paint += this.ChartLBR_Paint;

            this.numericLBRByGSDTime.ValueChanged += this.NumericLBRByGSDTime_ValueChanged;
            // this.masterpanel.Height = this.masterpanel.Controls.Cast<Control>().Max(c => c.Bottom);
        }

        private void NumericLBRByGSDTime_ValueChanged(object sender, EventArgs e)
        {
            if (this.numericLBRByGSDTime.Value == null)
            {
                return;
            }

            if (this.numericLBRByGSDTime.Value < this.StandardLBR)
            {
                this.numericLBRByGSDTime.BackColor = Color.PaleVioletRed;
            }
            else
            {
                this.numericLBRByGSDTime.BackColor = this.numericHighestGSDTime.BackColor;
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
            new P05_Print(this.CurrentMaintain["ID"].ToString()).ShowDialog();
            return base.ClickPrint();
        }

        /// <inheritdoc/>
        protected override void ClickUndo()
        {
            base.ClickUndo();

            if (this.CurrentMaintain == null)
            {
                this.chartLBR.Series.Clear();
                this.chartLBR.ChartAreas.Clear();
                this.chartLBR.BackColor = this.BackColor;
                this.lineMappingGrids.SubData.Clear();
                this.centralizedPPAGrids.SubData.Clear();

                return;
            }

            this.RefreshAutomatedLineMappingSummary();
            this.ShowLBRChart(this.CurrentMaintain);
            foreach (DataRow dr in this.DetailDatas)
            {
                dr["Selected"] = false;
            }

            this.OnRefreshClick();
        }

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            this.dtAutomatedLineMapping_DetailTemp.Clear();
            this.dtAutomatedLineMapping_DetailAuto.Clear();
            string masterID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();
            this.DetailSelectCommand = string.Format(this.sqlGetAutomatedLineMapping_Detail, $" ad.ID = '{masterID}'");
            return base.OnDetailSelectCommandPrepare(e);
        }

        /// <inheritdoc/>
        protected override DualResult OnRenewDataDetailPost(RenewDataPostEventArgs e)
        {
            // dtAutomatedLineMapping_DetailTemp, dtAutomatedLineMapping_DetailAuto取資料
            DualResult result = DBProxy.Current.Select(null, string.Format(this.sqlGetAutomatedLineMapping_DetailAuto, $" ad.ID = '{e.Master["ID"]}'"), out this.dtAutomatedLineMapping_DetailAuto);
            if (!result)
            {
                this.ShowErr(result);
            }

            result = DBProxy.Current.Select(null, string.Format(this.sqlGetAutomatedLineMapping_DetailTemp, $" ad.ID = '{e.Master["ID"]}'"), out this.dtAutomatedLineMapping_DetailTemp);
            if (!result)
            {
                this.ShowErr(result);
            }

            this.ShowLBRChart(e.Master);
            this.p05_NotHitTargetReason = new P05_NotHitTargetReason(e.Master["ID"].ToString(), e.Master["FactoryID"].ToString(), e.Master["Status"].ToString());
            if (this.p05_NotHitTargetReason.HasNotHitTargetReason)
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

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.FilterGrid();
            this.detailgrid.ColumnHeadersHeight = this.gridLineMappingRight.ColumnHeadersHeight;
            this.gridCentralizedPPALeft.ColumnHeadersHeight = this.gridCentralizedPPARight.ColumnHeadersHeight;
            this.btnEditOperation.Enabled = this.tabDetail.SelectedIndex == 0 && this.EditMode;
            this.btnTransferToP06.Enabled = this.CurrentMaintain["Status"].ToString() == "Confirmed";

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

            if (!MyUtility.Check.Empty(this.CurrentMaintain["TotalGSDTime"]) &&
                !MyUtility.Check.Empty(this.CurrentMaintain["SewerManpower"]) &&
                !MyUtility.Check.Empty(this.CurrentMaintain["WorkHour"]))
            {
                decimal decTotalGSD = MyUtility.Convert.GetDecimal(this.CurrentMaintain["TotalGSDTime"]);
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
        protected override void ClickNewAfter()
        {
            this.btnMachineSummary.Enabled = false;
            P05_CreateNewLineMapping p05_CreateNewLineMapping = new P05_CreateNewLineMapping(this, this.CurrentMaintain, (DataTable)this.detailgridbs.DataSource, this.dtAutomatedLineMapping_DetailTemp, this.dtAutomatedLineMapping_DetailAuto);
            p05_CreateNewLineMapping.ShowDialog();
            if (this.DetailDatas.Count == 0)
            {
                base.ClickNewAfter();
                this.DoDetailUndo(false);
                this.btnMachineSummary.Enabled = true;
                return;
            }

            this.FilterGrid();
            this.RefreshAutomatedLineMappingSummary();
            this.ShowLBRChart(this.CurrentMaintain);
            base.ClickNewAfter();
            this.txtfactory.ReadOnly = true;
            this.comboPhase.ReadOnly = true;
            this.btnMachineSummary.Enabled = true;

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
            this.txtfactory.ReadOnly = true;
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
delete AutomatedLineMapping_DetailTemp where ID = '{this.CurrentMaintain["ID"]}'
delete AutomatedLineMapping_DetailAuto where ID = '{this.CurrentMaintain["ID"]}'
delete AutomatedLineMapping_NotHitTargetReason where ID = '{this.CurrentMaintain["ID"]}'
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

            DualResult result;

            result = DBProxy.Current.Select(null, string.Format(this.sqlGetAutomatedLineMapping_DetailAuto, $" ad.ID = '{this.CurrentMaintain["ID"]}'"), out this.dtAutomatedLineMapping_DetailAuto);
            if (!result)
            {
                this.ShowErr(result);
                return false;
            }

            result = DBProxy.Current.Select(null, string.Format(this.sqlGetAutomatedLineMapping_DetailTemp, $" ad.ID = '{this.CurrentMaintain["ID"]}'"), out this.dtAutomatedLineMapping_DetailTemp);
            if (!result)
            {
                this.ShowErr(result);
                return false;
            }

            this.CurrentMaintain["Status"] = "New";
            this.CurrentMaintain["Version"] = DBNull.Value;
            this.CurrentMaintain["Phase"] = string.Empty;
            this.CurrentMaintain["AddName"] = Env.User.UserID;
            this.CurrentMaintain["EditName"] = string.Empty;
            this.CurrentMaintain["EditDate"] = DBNull.Value;
            this.CurrentMaintain["ID"] = DBNull.Value;
            this.btnTransferToP06.Enabled = this.CurrentMaintain["Status"].ToString() == "Confirmed";
            return true;
        }

        /// <inheritdoc/>
        protected override void ClickConfirm()
        {

            if (MyUtility.Msg.InfoBox("Are you sure you want to Confirm?", buttons: MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }

            decimal checkLBRCondition = this.StandardLBR;

            if (checkLBRCondition > 0 &&
                MyUtility.Convert.GetDecimal(this.CurrentMaintain["LBRByGSDTime"]) < checkLBRCondition)
            {
                MyUtility.Msg.WarningBox($"[LBR By GSD Time(%)] should not be lower than {checkLBRCondition}%, if you have any concern, please contact TPE-MPC Team directly.");
                return;
            }

            var listEmptyReason = this.p05_NotHitTargetReason.DataNotHitTargetReason.AsEnumerable().Where(s => MyUtility.Check.Empty(s["IEReasonID"]));

            if (listEmptyReason.Any())
            {
                MyUtility.Msg.WarningBox($"Not Hit Target Reason {listEmptyReason.Select(s => s["No"].ToString()).JoinToString(",")} has not yet input the reason, cannot be confirm.");
                this.p05_NotHitTargetReason.ShowDialog();
                return;
            }

            string sqlUpdate = $@"
update  AutomatedLineMapping
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

            this.detailgridbs.RemoveFilter();
            this.gridCentralizedPPALeftBS.RemoveFilter();

            int seqLineMapping = 1;
            int seqCentralizedPPA = 1;
            int seqIsNonSewingLine = 1;

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
                else
                {
                    dr["Seq"] = seqIsNonSewingLine;
                    seqIsNonSewingLine++;
                }
            }

            // 取version
            if (MyUtility.Convert.GetInt(this.CurrentMaintain["Version"]) == 0)
            {
                string sqlGetVersion = $@"
select [NewVersion] = isnull(max(Version), 0) + 1
from AutomatedLineMapping with (nolock)
where   FactoryID = '{this.CurrentMaintain["FactoryID"]}' and
        StyleID = '{this.CurrentMaintain["StyleID"]}' and
        SeasonID = '{this.CurrentMaintain["SeasonID"]}' and
        BrandID = '{this.CurrentMaintain["BrandID"]}' and
        ComboType = '{this.CurrentMaintain["ComboType"]}' and 
        Phase = '{this.CurrentMaintain["Phase"]}'
";
                this.CurrentMaintain["Version"] = MyUtility.Convert.GetInt(MyUtility.GetValue.Lookup(sqlGetVersion));
            }

            // 有可能因為調整佔位順序造成HighestGSDTime不同，所以這邊要更新
            this.RefreshAutomatedLineMappingSummary();

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


                // 第一筆勾選時，將上下各五筆No保留Enable，其餘disable不能勾選
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

                this.SelectedSameTimeStudyDetailUkey(dr["No"].ToString(), isChecked);

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

            this.Helper.Controls.Grid.Generator(this.detailgrid)
               .Text("No", header: "No", width: Widths.AnsiChars(4), iseditingreadonly: true)
               .CheckBox("Selected", string.Empty, trueValue: true, falseValue: false, iseditable: true, settings: colSelected)
               .Text("Location", header: "Location", width: Widths.AnsiChars(13), iseditingreadonly: true)
               .Text("PPADesc", header: "PPA", width: Widths.AnsiChars(5), iseditingreadonly: true)
               .CellMachineType("MachineTypeID", "ST/MC type", this, width: Widths.AnsiChars(10))
               .Text("MasterPlusGroup", header: "MC Group", width: Widths.AnsiChars(10), settings: colMachineTypeID)
               .Text("OperationDesc", header: "Operation", width: Widths.AnsiChars(13), iseditingreadonly: true)
               .Text("Annotation", header: "Annotation", width: Widths.AnsiChars(23), iseditingreadonly: true)
               .CellAttachment("Attachment", "Attachment", this, width: Widths.AnsiChars(10))
               .CellPartID("SewingMachineAttachmentID", "Part ID", this, width: Widths.AnsiChars(10))
               .CellTemplate("Template", "Template", this, width: Widths.AnsiChars(10))
               .Numeric("GSD", header: "GSD\r\nTime", width: Widths.AnsiChars(4), decimal_places: 2, iseditingreadonly: true)
               .Numeric("SewerDiffPercentageDesc", header: "%", width: Widths.AnsiChars(3), iseditingreadonly: true)
               .Numeric("DivSewer", header: "Div.\r\nSewer", decimal_places: 2, width: Widths.AnsiChars(5), iseditingreadonly: true)
               .Numeric("OriSewer", header: "Ori.\r\nSewer", decimal_places: 2, width: Widths.AnsiChars(5), iseditingreadonly: true)
               .CellThreadComboID("ThreadComboID", "Thread" + Environment.NewLine + "Combination", this, width: Widths.AnsiChars(10))
               .EditText("Notice", header: "Notice", width: Widths.AnsiChars(13));

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
               .EditText("Notice", header: "Notice", width: Widths.AnsiChars(20));

            this.Helper.Controls.Grid.Generator(this.gridLineMappingRight)
               .Text("No", header: "No. Of" + Environment.NewLine + "Station", width: Widths.AnsiChars(10))
               .Text("sumGSDTime", header: "Total" + Environment.NewLine + "GSD Time", width: Widths.AnsiChars(10))
               .Text("TotalGSDTime", header: "Total GSD" + Environment.NewLine + "Time by (%)", width: Widths.AnsiChars(10))
               .Text("OperatorLoading", header: "Operator" + Environment.NewLine + "Loading (%)", width: Widths.AnsiChars(10));

            this.Helper.Controls.Grid.Generator(this.gridCentralizedPPARight)
               .Text("No", header: "PPA" + Environment.NewLine + "No.", width: Widths.AnsiChars(10))
               .Text("TotalGSDTime", header: "Total" + Environment.NewLine + "GSD Time", width: Widths.AnsiChars(10))
               .Text("OperatorLoading", header: "Operator" + Environment.NewLine + "Loading (%)", width: Widths.AnsiChars(10));

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

            DualResult result = DBProxy.Current.Execute(null, $"delete AutomatedLineMapping_Detail where id = '{this.CurrentMaintain["ID"]}'");

            if (!result)
            {
                return result;
            }

            return base.ClickSavePre();
        }

        /// <inheritdoc/>
        protected override DualResult ClickSavePost()
        {
            #region 更新DetailAuto, DetailTemp
            bool isNewData = !MyUtility.Check.Seek($"select 1 from AutomatedLineMapping_DetailAuto with (nolock) where ID = '{this.CurrentMaintain["ID"]}'");
            DualResult result;
            DataTable dtEmpty;

            // AutomatedLineMapping_DetailAuto只保留最一開始產生的資料
            if (isNewData)
            {
                string insertAutomatedLineMapping_DetailAuto = $@"
insert into AutomatedLineMapping_DetailAuto(ID
                                            ,No
                                            ,SewerManpower
                                            ,Seq
                                            ,Location
                                            ,PPA
                                            ,MachineTypeID
                                            ,MasterPlusGroup
                                            ,OperationID
                                            ,Annotation
                                            ,Attachment
                                            ,SewingMachineAttachmentID
                                            ,Template
                                            ,GSD
                                            ,SewerDiffPercentage
                                            ,DivSewer
                                            ,OriSewer
                                            ,TimeStudyDetailUkey
                                            ,ThreadComboID
                                            ,IsNonSewingLine)
select  [ID] = '{this.CurrentMaintain["ID"]}'
        ,No
        ,SewerManpower
        ,Seq
        ,Location
        ,isnull(PPA, '')
        ,isnull(MachineTypeID, '')
        ,isnull(MasterPlusGroup, '')
        ,isnull(OperationID, '')
        ,isnull(Annotation, '')
        ,isnull(Attachment, '')
        ,isnull(SewingMachineAttachmentID, '')
        ,isnull(Template, '')
        ,isnull(GSD, 0)
        ,isnull(SewerDiffPercentage, 0)
        ,isnull(DivSewer, 0)
        ,isnull(OriSewer, 0)
        ,TimeStudyDetailUkey
        ,isnull(ThreadComboID, '')
        ,IsNonSewingLine
from #tmp
";
                result = MyUtility.Tool.ProcessWithDatatable(this.dtAutomatedLineMapping_DetailAuto, string.Empty, insertAutomatedLineMapping_DetailAuto, out dtEmpty);
                if (!result)
                {
                    return result;
                }
            }

            string updateAutomatedLineMapping_DetailTemp = $@"
delete AutomatedLineMapping_DetailTemp where ID = '{this.CurrentMaintain["ID"]}'

insert into AutomatedLineMapping_DetailTemp(ID
                                            ,No
                                            ,SewerManpower
                                            ,Seq
                                            ,Quota
                                            ,Location
                                            ,PPA
                                            ,MachineTypeID
                                            ,MasterPlusGroup
                                            ,OperationID
                                            ,Annotation
                                            ,SewingMachineAttachmentID
                                            ,Attachment
                                            ,Template
                                            ,GSD
                                            ,SewerDiffPercentage
                                            ,DivSewer
                                            ,OriSewer
                                            ,TimeStudyDetailUkey
                                            ,ThreadComboID
                                            ,Notice
                                            ,IsNonSewingLine
                                            )
select  '{this.CurrentMaintain["ID"]}'
        ,No
        ,SewerManpower
        ,Seq
        ,isnull(Quota, 0)
        ,Location
        ,isnull(PPA, '')
        ,isnull(MachineTypeID, '')
        ,isnull(MasterPlusGroup, '')
        ,isnull(OperationID, '')
        ,isnull(Annotation, '')
        ,isnull(SewingMachineAttachmentID, '')
        ,isnull(Attachment, '')
        ,isnull(Template, '')
        ,isnull(GSD, 0)
        ,isnull(SewerDiffPercentage, 0)
        ,isnull(DivSewer, 0)
        ,isnull(OriSewer, 0)
        ,TimeStudyDetailUkey
        ,isnull(ThreadComboID, '')
        ,isnull(Notice, '')
        ,IsNonSewingLine
from #tmp
";

            result = MyUtility.Tool.ProcessWithDatatable(this.dtAutomatedLineMapping_DetailTemp, null, updateAutomatedLineMapping_DetailTemp, out dtEmpty);
            if (!result)
            {
                return result;
            }
            #endregion
            return base.ClickSavePost();
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
                this.detailgridbs.Filter = "PPA <> 'C' and IsNonSewingLine = 0 and IsNotShownInP05 = 0";
                this.lineMappingGrids.RefreshSubData();
            }
            else
            {
                this.gridCentralizedPPALeftBS.Filter = "PPA = 'C' and IsNonSewingLine = 0 and IsNotShownInP05 = 0";
                this.centralizedPPAGrids.RefreshSubData();
            }

            this.gridLineMappingRightBS.DataSource = this.gridLineMappingRight.DataSource;
            this.gridLineMappingRightBS.Filter = "IsNotShownInP05 = 'False' and No <> '' and No is not null";
            this.detailgridbs.Filter = "IsNotShownInP05 = 'False' and No <> '' and No is not null"; // ISP20240132 隱藏工段
        }

        private void RefreshAutomatedLineMappingSummary()
        {
            this.lineMappingGrids.RefreshSubData();
            if (this.lineMappingGrids.HighestGSD > 0)
            {
                this.CurrentMaintain["HighestGSDTime"] = this.lineMappingGrids.HighestGSD;
                this.CurrentMaintain["TotalGSDTime"] = this.lineMappingGrids.TotalGSD;
            }

            decimal.TryParse(MyUtility.Convert.GetString(this.CurrentMaintain["TotalGSDTime"]), out decimal s1);
            decimal.TryParse(MyUtility.Convert.GetString(this.CurrentMaintain["HighestGSDTime"]), out decimal s2);
            decimal.TryParse(MyUtility.Convert.GetString(this.CurrentMaintain["SewerManpower"]), out decimal s3);

            // LBRByGSDTime
            if (s1 != 0 && s2 != 0 && s3 != 0)
            {
                this.CurrentMaintain["LBRByGSDTime"] = MyUtility.Math.Round(MyUtility.Convert.GetDecimal(this.CurrentMaintain["TotalGSDTime"]) / MyUtility.Convert.GetDecimal(this.CurrentMaintain["HighestGSDTime"]) / MyUtility.Convert.GetDecimal(this.CurrentMaintain["SewerManpower"]) * 100, 2);
            }
            else
            {
                this.CurrentMaintain["LBRByGSDTime"] = 0;
            }

            // AvgGSDTime
            if (s1 != 0 && s3 != 0)
            {
                this.CurrentMaintain["AvgGSDTime"] = MyUtility.Math.Round(MyUtility.Convert.GetDecimal(this.CurrentMaintain["TotalGSDTime"]) / MyUtility.Convert.GetDecimal(this.CurrentMaintain["SewerManpower"]), 2);
            }
            else
            {
                this.CurrentMaintain["AvgGSDTime"] = 0;
            }

            // TotalSewingLineOptrs
            this.CurrentMaintain["TotalSewingLineOptrs"] = MyUtility.Convert.GetInt(this.CurrentMaintain["SewerManpower"]) + MyUtility.Convert.GetInt(this.CurrentMaintain["PresserManpower"]) + MyUtility.Convert.GetInt(this.CurrentMaintain["PackerManpower"]);

            // TargetHr
            if (s1 != 0 && s3 != 0)
            {
                this.CurrentMaintain["TargetHr"] = MyUtility.Math.Round(3600 * MyUtility.Convert.GetDecimal(this.CurrentMaintain["SewerManpower"]) / MyUtility.Convert.GetDecimal(this.CurrentMaintain["TotalGSDTime"]), 0);
            }
            else
            {
                this.CurrentMaintain["TargetHr"] = 0;
            }

            // DailyDemand
            this.CurrentMaintain["DailyDemand"] = MyUtility.Math.Round(MyUtility.Convert.GetDecimal(this.CurrentMaintain["TargetHr"]) * MyUtility.Convert.GetDecimal(this.CurrentMaintain["WorkHour"]), 0);
            decimal.TryParse(MyUtility.Convert.GetString(this.CurrentMaintain["DailyDemand"]), out decimal s4);
            decimal.TryParse(MyUtility.Convert.GetString(this.CurrentMaintain["WorkHour"]), out decimal s5);

            // TaktTime
            if (s4 != 0 && s5 != 0)
            {
                decimal decTotalGSD = MyUtility.Convert.GetDecimal(this.CurrentMaintain["TotalGSDTime"]);
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

            // EOLR
            if (s2 != 0)
            {
                this.CurrentMaintain["EOLR"] = MyUtility.Math.Round(3600 / MyUtility.Convert.GetDecimal(this.CurrentMaintain["HighestGSDTime"]), 2);
            }
            else
            {
                this.CurrentMaintain["EOLR"] = 0;
            }

            decimal.TryParse(MyUtility.Convert.GetString(this.CurrentMaintain["WorkHour"]), out decimal s6);
            decimal.TryParse(MyUtility.Convert.GetString(this.CurrentMaintain["StyleCPU"]), out decimal s7);

            // PPH
            if (s3 != 0 && s6 != 0 && s7 != 0)
            {
                this.CurrentMaintain["PPH"] = MyUtility.Math.Round(MyUtility.Convert.GetDecimal(this.CurrentMaintain["EOLR"]) * MyUtility.Convert.GetDecimal(this.CurrentMaintain["StyleCPU"]) / MyUtility.Convert.GetDecimal(this.CurrentMaintain["SewerManpower"]), 2);
            }
            else
            {
                this.CurrentMaintain["PPH"] = 0;
            }
        }

        private void ShowLBRChart(DataRow drMain)
        {
            // 清除任何現有的系列和點
            this.chartLBR.Series.Clear();
            this.chartLBR.ChartAreas.Clear();
            this.chartLBR.BackColor = this.BackColor;

            // 建立新的圖表區域
            ChartArea chartArea = new ChartArea();
            chartArea.AxisX.MajorGrid.Enabled = false;
            chartArea.AxisY.MajorGrid.Enabled = false;
            chartArea.AxisY.Maximum = 90;
            chartArea.AxisY.Minimum = 50;
            chartArea.AxisY.LabelStyle.Format = "0'%'";
            chartArea.BackColor = this.BackColor;

            this.chartLBR.ChartAreas.Add(chartArea);

            // 建立新的系列
            Series series = new Series();
            series.ChartType = SeriesChartType.Column;
            series.IsVisibleInLegend = false;

            // 添加數據點
            var groupLBR = this.dtAutomatedLineMapping_DetailTemp.AsEnumerable()
                                .Where(s => s["OperationID"].ToString() != "PROCIPF00004" &&
                                            s["OperationID"].ToString() != "PROCIPF00003" &&
                                            s["PPA"].ToString() != "C" &&
                                            MyUtility.Convert.GetBool(s["IsNonSewingLine"]) == false)
                                .GroupBy(s => new { SewerManpower = MyUtility.Convert.GetInt(s["SewerManpower"]) })
                                .Select(groupItem =>
                                {
                                    decimal highestGSDTime = groupItem.GroupBy(s => s["No"].ToString()).Max(groupSubItem => groupSubItem.Sum(s => (decimal)s["GSD"] * (MyUtility.Check.Empty(s["SewerDiffPercentage"]) ? 0 : (decimal)s["SewerDiffPercentage"])));
                                    return new
                                    {
                                        highestGSDTime,
                                        TotalGSD = MyUtility.Math.Round(groupItem.Sum(s => (decimal)s["GSD"] * (MyUtility.Check.Empty(s["SewerDiffPercentage"]) ? 0 : (decimal)s["SewerDiffPercentage"])), 2),
                                        groupItem.Key.SewerManpower,
                                        LBR = MyUtility.Math.Round(MyUtility.Math.Round(groupItem.Sum(s => (decimal)s["GSD"] * (MyUtility.Check.Empty(s["SewerDiffPercentage"]) ? 0 : (decimal)s["SewerDiffPercentage"])), 2) / highestGSDTime / groupItem.Key.SewerManpower * 100, 0),
                                    };
                                }).OrderBy(s => s.SewerManpower);

            foreach (var itemLBR in groupLBR)
            {
                if (itemLBR.SewerManpower.ToString() == drMain["SewerManpower"].ToString())
                {
                    series.Points.Add(new DataPoint(itemLBR.SewerManpower, MyUtility.Convert.GetDouble(drMain["LBRByGSDTime"])));
                }
                else
                {
                    series.Points.Add(new DataPoint(itemLBR.SewerManpower, MyUtility.Convert.GetDouble(itemLBR.LBR)));
                }
            }

            // 將系列添加到圖表控制項
            this.chartLBR.Series.Add(series);
            this.chartLBR.ChartAreas[0].Position.Width = 95; // 例如設定為 100 單位
            this.chartLBR.ChartAreas[0].Position.Height = 85; // 例如設定為 100 單位
            this.chartLBR.ChartAreas[0].Position.Y = 5;

            // 將原本X軸的label字體顏色改為跟背景一樣
            this.chartLBR.ChartAreas[0].AxisX.LabelStyle.ForeColor = this.BackColor;

            foreach (var point in this.chartLBR.Series[0].Points)
            {
                if (point.XValue.ToString() == drMain["SewerManpower"].ToString())
                {
                    point.Color = Color.FromArgb(81, 130, 189);
                }
                else
                {
                    point.Color = Color.FromArgb(191, 191, 191);
                }
            }
        }

        private void ChartLBR_Paint(object sender, PaintEventArgs e)
        {
            if (this.chartLBR.ChartAreas.Count == 0 || this.CurrentMaintain == null)
            {
                return;
            }

            int i = 0;
            int btnLocationY = MyUtility.Convert.GetInt(this.chartLBR.ChartAreas[0].AxisY.ValueToPixelPosition(50) + 5);

            foreach (var point in this.chartLBR.Series[0].Points)
            {
                // 取得 Bar 的數值
                double value = point.YValues[0];
                double limitValueY = value;
                if (limitValueY <= 50)
                {
                    limitValueY = 50;
                }
                else if (limitValueY >= 90)
                {
                    limitValueY = 90;
                }

                // 建立自訂標籤文字
                string label = value.ToString("0'%'");

                // 取得 Bar 的座標
                PointF position = PointF.Empty;
                position.X = (float)this.chartLBR.ChartAreas[0].AxisX.ValueToPixelPosition(point.XValue - 0.5); // 調整 X 座標位置
                position.Y = (float)this.chartLBR.ChartAreas[0].AxisY.ValueToPixelPosition(limitValueY) - 15; // 調整 Y 座標位置

                this.chartLBRButtons[i].Text = point.XValue.ToString();
                this.chartLBRButtons[i].Location = new Point(MyUtility.Convert.GetInt(position.X) + 5, btnLocationY);

                if (point.XValue.ToString() == this.CurrentMaintain["SewerManpower"].ToString())
                {
                    this.chartLBRButtons[i].FlatAppearance.BorderColor = this.BackColor;
                    this.iBarchart = MyUtility.Convert.GetInt(point.XValue.ToString());
                }
                else
                {
                    this.chartLBRButtons[i].FlatAppearance.BorderColor = Color.Black;
                }

                e.Graphics.DrawString(label, this.chartLBR.Font, Brushes.Black, position);

                i++;
            }
        }

        private void BtnNotHitTargetReason_Click(object sender, EventArgs e)
        {
            if (this.CurrentMaintain == null)
            {
                return;
            }

            this.p05_NotHitTargetReason.ShowDialog();
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

            using (P05_EditOperation p05_EditOperation = new P05_EditOperation(this.detailgrid.GetTable()))
            {
                DialogResult dialogResult = p05_EditOperation.ShowDialog();
                if (dialogResult == DialogResult.OK)
                {
                    this.RefreshAutomatedLineMappingSummary();
                    this.ShowLBRChart(this.CurrentMaintain);
                }
            }

            this.detailgrid.Sort(this.detailgrid.Columns["No"], System.ComponentModel.ListSortDirection.Ascending);
        }

        private void ChartBtn_Click(object sender, EventArgs e)
        {
            if (this.CurrentMaintain == null)
            {
                return;
            }

            int firstDisplaySewermanpower = MyUtility.Convert.GetInt(((Win.UI.Button)sender).Text);
            new P05_Chart(firstDisplaySewermanpower, MyUtility.Convert.GetString(this.CurrentMaintain["ID"])).ShowDialog();
            //int firstDisplaySewermanpower = MyUtility.Convert.GetInt(((Win.UI.Button)sender).Text);
            //if (this.EditMode)
            //{
            //    P05_LBR p05_LBR = new P05_LBR(firstDisplaySewermanpower, this.CurrentMaintain, (DataTable)this.detailgridbs.DataSource, this.dtAutomatedLineMapping_DetailTemp, this.dtAutomatedLineMapping_DetailAuto);
            //    DialogResult dialogResult = p05_LBR.ShowDialog();
            //    this.FilterGrid(dialogResult == DialogResult.OK);
            //    this.RefreshAutomatedLineMappingSummary();
            //    this.ShowLBRChart(this.CurrentMaintain);
            //}
            //else
            //{
            //    P05_Compare p05_Compare = new P05_Compare(firstDisplaySewermanpower, this.CurrentMaintain, (DataTable)this.detailgridbs.DataSource, this.dtAutomatedLineMapping_DetailTemp, this.dtAutomatedLineMapping_DetailAuto);
            //    p05_Compare.ShowDialog();
            //}
        }

        private void BtnH_Click(object sender, EventArgs e)
        {
            if (this.CurrentMaintain == null)
            {
                return;
            }

            DataTable dtAutomatedLineMapping_DetailAutoDefault = this.dtAutomatedLineMapping_DetailAuto.AsEnumerable()
                .Where(s => MyUtility.Convert.GetInt(s["SewerManpower"]) == MyUtility.Convert.GetInt(this.CurrentMaintain["SewerManpower"]))
                .CopyToDataTable();

            new P05_Default(dtAutomatedLineMapping_DetailAutoDefault).ShowDialog();
        }

        private void BtnTransferToP06_Click(object sender, EventArgs e)
        {
            if (this.CurrentMaintain == null)
            {
                return;
            }

            string sqlInsertLineMappingBalancing = $@"
            INSERT INTO LineMappingBalancing 
            (
	            AutomatedLineMappingID
	            ,StyleUKey
	            ,Phase
	            ,Version
	            ,FactoryID
	            ,StyleID
	            ,SeasonID
	            ,BrandID
	            ,ComboType
	            ,StyleCPU
	            ,SewerManpower
	            ,PackerManpower
	            ,PresserManpower
	            ,TotalGSDTime
                ,TotalCycleTime
	            ,HighestGSDTime
                ,HighestCycleTime
	            ,TimeStudyID
	            ,TimeStudyStatus
	            ,TimeStudyVersion
	            ,WorkHour
	            ,Status
	            ,AddName
	            ,AddDate
                ,OriNoNumber
                ,Reason
                ,OriTotalGSDTime
            )
            SELECT
            alm.ID
            ,alm.StyleUKey
            ,'Final'
            ,0
            ,alm.FactoryID
            ,alm.StyleID
            ,alm.SeasonID
            ,alm.BrandID
            ,alm.ComboType
            ,alm.StyleCPU
            ,alm.SewerManpower
            ,alm.PackerManpower
            ,alm.PresserManpower
            ,alm.TotalGSDTime
            ,alm.TotalGSDTime
            ,alm.HighestGSDTime
            ,alm.HighestGSDTime
            ,alm.TimeStudyID
            ,alm.TimeStudyStatus
            ,alm.TimeStudyVersion
            ,alm.WorkHour
            ,'New'
            ,'{Env.User.UserID}'
            ,GETDATE()
            ,{this.DetailDatas.AsEnumerable().GroupBy(x => x["No"].ToString()).Count()}
            ,''
            ,alm.TotalGSDTime
            FROM AutomatedLineMapping alm
            WHERE alm.ID = '{this.CurrentMaintain["ID"]}';

            DECLARE @ID INT = @@identity

            INSERT INTO LineMappingBalancing_Detail 
            (
                ID
                ,No
                ,Seq
                ,Location
                ,PPA
                ,MachineTypeID
                ,MasterPlusGroup
                ,OperationID
                ,Annotation
                ,Attachment
                ,SewingMachineAttachmentID
                ,Template
                ,GSD
                ,Cycle
                ,SewerDiffPercentage
                ,DivSewer
                ,OriSewer
                ,TimeStudyDetailUkey
                ,ThreadComboID
                ,Notice
                ,IsNonSewingLine
                ,GroupNo
            )
            SELECT
            @ID
            ,almd.No
            ,almd.Seq
            ,almd.Location
            ,almd.PPA
            ,almd.MachineTypeID
            ,almd.MasterPlusGroup
            ,almd.OperationID
            ,almd.Annotation
            ,almd.Attachment
            ,almd.SewingMachineAttachmentID
            ,almd.Template
            ,almd.GSD
            ,almd.GSD
            ,almd.SewerDiffPercentage
            ,almd.DivSewer
            ,almd.OriSewer
            ,almd.TimeStudyDetailUkey
            ,almd.ThreadComboID
            ,''
            ,almd.IsNonSewingLine
            ,[GroupNo] = 1
            FROM AutomatedLineMapping_Detail almd
            WHERE almd.ID = '{this.CurrentMaintain["ID"]}'

            select [ID] = @ID
";
            DataTable dtOutID;
            DualResult result = DBProxy.Current.Select(null, sqlInsertLineMappingBalancing, out dtOutID);

            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            string p06Title = MyUtility.GetValue.Lookup("select BarPrompt from MenuDetail with (nolock) where Formname = 'Sci.Production.IE.P06'");

            bool hasP06Authority = PublicPrg.Prgs.GetAuthority(Env.User.UserID, p06Title, "CanEdit");

            if (!hasP06Authority)
            {
                MyUtility.Msg.InfoBox("Transfer to P06 complete");
                return;
            }

            string id = dtOutID.Rows[0][0].ToString();

            DialogResult dialogResult = MyUtility.Msg.QuestionBox("Line Mapping transfer is successful, do you want to open IE_P06 directly?");
            if (dialogResult == DialogResult.Yes)
            {
                foreach (Form form in Application.OpenForms)
                {
                    if (form is P06)
                    {
                        form.Activate();
                        P06 activateForm = (P06)form;
                        activateForm.ShowDirectQueryID(id);
                        return;
                    }
                }

                ToolStripMenuItem p06MenuItem = null;
                foreach (ToolStripMenuItem toolMenuItem in Env.App.MainMenuStrip.Items)
                {
                    if (toolMenuItem.Text.EqualString("IE"))
                    {
                        foreach (var subMenuItem in toolMenuItem.DropDown.Items)
                        {
                            if (subMenuItem.GetType().Equals(typeof(ToolStripMenuItem)))
                            {
                                if (((ToolStripMenuItem)subMenuItem).Text.EqualString(p06Title))
                                {
                                    p06MenuItem = (ToolStripMenuItem)subMenuItem;
                                    break;
                                }
                            }
                        }
                    }
                }

                if (p06MenuItem == null)
                {
                    MyUtility.Msg.WarningBox("P06. Line Mapping & Balancing menu setting not found");
                    return;
                }

                P06 callP06 = new P06(p06MenuItem);
                callP06.MdiParent = this.MdiParent;
                callP06.ShowDirectQueryID(id);
            }
        }

        private void BtnLineMappingComparison_Click(object sender, EventArgs e)
        {
            P05_Compare p05_Compare = new P05_Compare(this.iBarchart, this.CurrentMaintain, (DataTable)this.detailgridbs.DataSource, this.dtAutomatedLineMapping_DetailTemp, this.dtAutomatedLineMapping_DetailAuto);
            p05_Compare.ShowDialog();
        }

        private void BtnViewOperator_Click(object sender, EventArgs e)
        {
            P05_LBR p05_LBR = new P05_LBR(this.iBarchart, this.CurrentMaintain, (DataTable)this.detailgridbs.DataSource, this.dtAutomatedLineMapping_DetailTemp, this.dtAutomatedLineMapping_DetailAuto);
            DialogResult dialogResult = p05_LBR.ShowDialog();
            this.FilterGrid(dialogResult == DialogResult.OK);
            this.RefreshAutomatedLineMappingSummary();
            this.ShowLBRChart(this.CurrentMaintain);
        }

        private void BtnMachineSummary_Click(object sender, EventArgs e)
        {
            new P05_MachineSummary(MyUtility.Convert.GetString(this.CurrentMaintain["ID"])).ShowDialog();
        }
    }
}
