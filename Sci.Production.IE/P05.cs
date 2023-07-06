using Ict;
using Ict.Win;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Smo.Agent;
using Sci.Data;
using Sci.Production.Class;
using Sci.Production.Prg;
using Sci.Win.Tools;
using Sci.Win.UI;
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
using static Ict.Win.WinAPI;

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

        private Win.UI.Button[] chartLBRButtons = new Win.UI.Button[]
        {
                new Win.UI.Button(),
                new Win.UI.Button(),
                new Win.UI.Button(),
                new Win.UI.Button(),
                new Win.UI.Button(),
        };

        private string sqlGetAutomatedLineMapping_DetailAuto = @"
select  ad.*,
        [PPADesc] = isnull(d.Name, ''),
        [OperationDesc] = iif(isnull(op.DescEN, '') = '', ad.OperationID, op.DescEN),
        [SewerDiffPercentageDesc] = round(ad.SewerDiffPercentage * 100, 0),
        [TimeStudyDetailUkeyCnt] = Count(ad.TimeStudyDetailUkey) over (partition by ad.TimeStudyDetailUkey, ad.SewerManpower)
from    AutomatedLineMapping_DetailAuto ad with (nolock) 
left join DropDownList d with (nolock) on d.ID = ad.PPA  and d.Type = 'PMS_IEPPA'
left join Operation op with (nolock) on op.ID = ad.OperationID
where {0}
order by ad.SewerManpower, ad.No, ad.Seq
";

        private string sqlGetAutomatedLineMapping_DetailTemp = @"
select  ad.*,
        [PPADesc] = isnull(d.Name, ''),
        [OperationDesc] = iif(isnull(op.DescEN, '') = '', ad.OperationID, op.DescEN),
        [SewerDiffPercentageDesc] = round(ad.SewerDiffPercentage * 100, 0),
        [TimeStudyDetailUkeyCnt] = Count(ad.TimeStudyDetailUkey) over (partition by ad.TimeStudyDetailUkey, ad.SewerManpower)
from    AutomatedLineMapping_DetailTemp ad with (nolock) 
left join DropDownList d with (nolock) on d.ID = ad.PPA  and d.Type = 'PMS_IEPPA'
left join Operation op with (nolock) on op.ID = ad.OperationID
where {0}
order by ad.SewerManpower, ad.No, ad.Seq
";

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
            this.detailgridbs.DataSourceChanged += this.Detailgridbs_DataSourceChanged;
            this.gridCentralizedPPALeft.DataSource = this.gridCentralizedPPALeftBS;

            this.detailgrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.gridCentralizedPPALeft.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.detailgrid.CellFormatting += this.Detailgrid_CellFormatting;

            this.lineMappingGrids = new AutoLineMappingGridSyncScroll(this.detailgrid, this.gridLineMappingRight, "No");
            this.centralizedPPAGrids = new AutoLineMappingGridSyncScroll(this.gridCentralizedPPALeft, this.gridCentralizedPPARight, "No");

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
        }

        private void Detailgrid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
            if (dr["OperationID"].ToString() == "PROCIPF00003" ||
                dr["OperationID"].ToString() == "PROCIPF00004")
            {
                this.detailgrid.Rows[e.RowIndex].Cells["MachineTypeID"].ReadOnly = true;
                this.detailgrid.Rows[e.RowIndex].Cells["MasterPlusGroup"].ReadOnly = true;
            }

            if (e.ColumnIndex > 1)
            {
                e.CellStyle.BackColor = MyUtility.Convert.GetInt(dr["TimeStudyDetailUkeyCnt"]) > 1 ? Color.FromArgb(255, 255, 153) : this.detailgrid.DefaultCellStyle.BackColor;
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
        protected override void ClickUndo()
        {
            base.ClickUndo();
            this.RefreshAutomatedLineMappingSummary();
            this.ShowLBRChart(this.CurrentMaintain);
        }

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            this.dtAutomatedLineMapping_DetailTemp.Clear();
            this.dtAutomatedLineMapping_DetailAuto.Clear();
            string masterID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();
            this.DetailSelectCommand =
                $@"
select  cast(0 as bit) as Selected,
        ad.*,
        [PPADesc] = isnull(d.Name, ''),
        [OperationDesc] = iif(isnull(op.DescEN, '') = '', ad.OperationID, op.DescEN),
        [SewerDiffPercentageDesc] = round(ad.SewerDiffPercentage * 100, 0),
        [TimeStudyDetailUkeyCnt] = Count(TimeStudyDetailUkey) over (partition by TimeStudyDetailUkey)
from AutomatedLineMapping_Detail ad WITH (NOLOCK)
left join DropDownList d with (nolock) on d.ID = ad.PPA  and d.Type = 'PMS_IEPPA'
left join Operation op with (nolock) on op.ID = ad.OperationID
where ad.ID = '{masterID}'
order by ad.Seq";

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
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            P05_CreateNewLineMapping p05_CreateNewLineMapping = new P05_CreateNewLineMapping(this.CurrentMaintain, (DataTable)this.detailgridbs.DataSource, this.dtAutomatedLineMapping_DetailTemp, this.dtAutomatedLineMapping_DetailAuto);
            p05_CreateNewLineMapping.ShowDialog();
            if (this.DetailDatas.Count == 0)
            {
                return;
            }

            this.RefreshAutomatedLineMappingSummary();
            this.FilterGrid();
            this.ShowLBRChart(this.CurrentMaintain);
            base.ClickNewAfter();
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

            this.CurrentMaintain["Status"] = "New";
            this.CurrentMaintain["Version"] = DBNull.Value;
            this.CurrentMaintain["Phase"] = string.Empty;
            this.CurrentMaintain["AddName"] = Env.User.UserID;
            this.CurrentMaintain["EditName"] = string.Empty;
            this.CurrentMaintain["EditDate"] = DBNull.Value;

            DualResult result;

            result = DBProxy.Current.Select(null, string.Format(this.sqlGetAutomatedLineMapping_DetailAuto, $" ad.ID = '{this.CurrentMaintain["ID"]}'"), out this.dtAutomatedLineMapping_DetailAuto);
            if (!result)
            {
                this.ShowErr(result);
                return false;
            }

            string sqlGetAutomatedLineMapping_DetailTemp = $@"
select  ad.*,
        [PPADesc] = isnull(d.Name, ''),
        [OperationDesc] = iif(isnull(op.DescEN, '') = '', ad.OperationID, op.DescEN),
        [SewerDiffPercentageDesc] = round(ad.SewerDiffPercentage * 100, 0),
        [TimeStudyDetailUkeyCnt] = Count(TimeStudyDetailUkey) over (partition by TimeStudyDetailUkey)
from    AutomatedLineMapping_DetailTemp ad with (nolock) 
left join DropDownList d with (nolock) on d.ID = ad.PPA  and d.Type = 'PMS_IEPPA'
left join Operation op with (nolock) on op.ID = ad.OperationID
where ad.ID = '{this.CurrentMaintain["ID"]}'
order by ad.SewerManpower, ad.Seq
";
            result = DBProxy.Current.Select(null, string.Format(this.sqlGetAutomatedLineMapping_DetailTemp, $" ad,ID = '{this.CurrentMaintain["ID"]}'"), out this.dtAutomatedLineMapping_DetailTemp);
            if (!result)
            {
                this.ShowErr(result);
                return false;
            }

            return true;
        }

        /// <inheritdoc/>
        protected override void ClickConfirm()
        {
            string sqlGetLBRCondition = $@"
SELECT ALMCS.Condition1 
FROM AutomatedLineMappingConditionSetting ALMCS
WHERE ALMCS.[FactoryID] = '{this.CurrentMaintain["FactoryID"]}'
AND ALMCS.[Function] = 'IE_P05'
AND ALMCS.Verify = 'LBRByGSD'
AND ALMCS.Junk = 0
";
            decimal checkLBRCondition = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup(sqlGetLBRCondition));

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

            foreach (var dr in this.DetailDatas)
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

            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();

            TxtMachineGroup.CelltxtMachineGroup colMachineTypeID = TxtMachineGroup.CelltxtMachineGroup.GetGridCell();
            DataGridViewGeneratorTextColumnSettings colThreadComboID = new DataGridViewGeneratorTextColumnSettings();

            #region colThreadComboID
            colThreadComboID.EditingMouseDown += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                if (e.Button == MouseButtons.Right)
                {
                    return;
                }

                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                string sqlCmd = $@"
select　Thread_ComboID
from Style_ThreadColorCombo st with (nolock)
where	st.StyleUkey = '{this.CurrentMaintain["StyleUkey"]}' and
		st.MachineTypeID = '{dr["MachineTypeID"]}' and
		exists(select 1 from Style_ThreadColorCombo_Operation sto with (nolock) 
                        where   sto.Style_ThreadColorComboUkey = st.Ukey and 
                                sto.OperationID = '{dr["MachineTypeID"]}')
";
                SelectItem item = new Win.Tools.SelectItem(sqlCmd, "12", dr["ThreadComboID"].ToString());
                DialogResult returnResult = item.ShowDialog();
                if (returnResult == DialogResult.Cancel)
                {
                    return;
                }

                e.EditingControl.Text = item.GetSelectedString();

            };

            colThreadComboID.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                if (MyUtility.Check.Empty(e.FormattedValue) || e.FormattedValue.ToString() == dr["ThreadComboID"].ToString())
                {
                    return;
                }

                string sqlCmd = $@"
select　1
from Style_ThreadColorCombo st with (nolock)
where	st.StyleUkey = '{this.CurrentMaintain["StyleUkey"]}' and
		st.MachineTypeID = '{dr["MachineTypeID"]}' and
        st.Thread_ComboID = '{e.FormattedValue.ToString()}'
		exists(select 1 from Style_ThreadColorCombo_Operation sto with (nolock) 
                        where   sto.Style_ThreadColorComboUkey = st.Ukey and 
                                sto.OperationID = '{dr["MachineTypeID"]}')
";
                DataTable machineData;
                DualResult result = DBProxy.Current.Select(null, sqlCmd, out machineData);
                if (!result)
                {
                    dr["ThreadComboID"] = string.Empty;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox("Sql connection fail!!\r\n" + result.ToString());
                    return;
                }

                if (machineData.Rows.Count <= 0)
                {
                    dr["ThreadComboID"] = string.Empty;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("< ST/MC type: {0} > not found!!!", e.FormattedValue.ToString()));
                    return;
                }
            };
            #endregion

            this.Helper.Controls.Grid.Generator(this.detailgrid)
               .Text("No", header: "No", width: Widths.AnsiChars(4), iseditingreadonly: true)
               .CheckBox("Selected", string.Empty, trueValue: true, falseValue: false, iseditable: true)
               .Text("Location", header: "Location", width: Widths.AnsiChars(13), iseditingreadonly: true)
               .Text("PPADesc", header: "PPA", width: Widths.AnsiChars(13), iseditingreadonly: true)
               .CellMachineType("MachineTypeID", "ST/MC type", this, width: Widths.AnsiChars(10))
               .Text("MasterPlusGroup", header: "MC Group", width: Widths.AnsiChars(10), settings: colMachineTypeID)
               .Text("OperationDesc", header: "Operation", width: Widths.AnsiChars(13), iseditingreadonly: true)
               .Text("Annotation", header: "Annotation", width: Widths.AnsiChars(23), iseditingreadonly: true)
               .CellAttachment("Attachment", "Attachment", this, width: Widths.AnsiChars(10))
               .CellPartID("SewingMachineAttachmentID", "Part ID", this, width: Widths.AnsiChars(25))
               .CellTemplate("Template", "Template", this, width: Widths.AnsiChars(10))
               .Numeric("GSD", header: "GSD Time", width: Widths.AnsiChars(5), iseditingreadonly: true)
               .Numeric("SewerDiffPercentageDesc", header: "%", width: Widths.AnsiChars(5), iseditingreadonly: true)
               .Numeric("DivSewer", header: "Div. Sewer", decimal_places: 4, width: Widths.AnsiChars(5), iseditingreadonly: true)
               .Numeric("OriSewer", header: "Ori. Sewer", decimal_places: 4, width: Widths.AnsiChars(5), iseditingreadonly: true)
               .Text("ThreadComboID", header: "Thread" + Environment.NewLine + "Color", width: Widths.AnsiChars(10), settings: colThreadComboID)
               .Text("Notice", header: "Notice", width: Widths.AnsiChars(10));

            this.Helper.Controls.Grid.Generator(this.gridCentralizedPPALeft)
               .Text("No", header: "PPA No.", width: Widths.AnsiChars(4))
               .Text("Location", header: "Location", width: Widths.AnsiChars(13), iseditingreadonly: true)
               .CellMachineType("MachineTypeID", "ST/MC type", this, width: Widths.AnsiChars(10))
               .Text("MasterPlusGroup", header: "MC Group", width: Widths.AnsiChars(10), settings: colMachineTypeID)
               .Text("OperationDesc", header: "Operation", width: Widths.AnsiChars(13), iseditingreadonly: true)
               .Text("Annotation", header: "Annotation", width: Widths.AnsiChars(23), iseditingreadonly: true)
               .CellAttachment("Attachment", "Attachment", this, width: Widths.AnsiChars(10))
               .CellPartID("SewingMachineAttachmentID", "Part ID", this, width: Widths.AnsiChars(25))
               .CellTemplate("Template", "Template", this, width: Widths.AnsiChars(10))
               .Numeric("GSD", header: "GSD Time", width: Widths.AnsiChars(5), iseditingreadonly: true)
               .Text("Notice", header: "Notice", width: Widths.AnsiChars(10));

            this.Helper.Controls.Grid.Generator(this.gridLineMappingRight)
               .Text("No", header: "No. Of" + Environment.NewLine + "Station", width: Widths.AnsiChars(10))
               .Text("TotalGSDTime", header: "Total" + Environment.NewLine + "GSD Time", width: Widths.AnsiChars(10))
               .Text("OperatorLoading", header: "Operator" + Environment.NewLine + "Loading (%)", width: Widths.AnsiChars(10));

            this.Helper.Controls.Grid.Generator(this.gridCentralizedPPARight)
               .Text("No", header: "PPA" + Environment.NewLine + "No.", width: Widths.AnsiChars(10))
               .Text("TotalGSDTime", header: "Total" + Environment.NewLine + "GSD Time", width: Widths.AnsiChars(10))
               .Text("OperatorLoading", header: "Operator" + Environment.NewLine + "Loading (%)", width: Widths.AnsiChars(10));
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
            this.FilterGrid();
            this.btnEditOperation.Enabled = this.tabDetail.SelectedIndex == 0 && this.EditMode;
        }

        private void FilterGrid()
        {
            if (this.tabDetail.SelectedIndex == 0)
            {
                this.detailgridbs.Filter = "PPA <> 'C' and IsNonSewingLine = 0";
                this.lineMappingGrids.RefreshSubData(AutoLineMappingGridSyncScroll.SubGridType.LineMapping);
            }
            else
            {
                this.gridCentralizedPPALeftBS.Filter = "PPA = 'C' and IsNonSewingLine = 0";
                this.centralizedPPAGrids.RefreshSubData(AutoLineMappingGridSyncScroll.SubGridType.CentrailizedPPA);
            }
        }

        private void Detailgridbs_DataSourceChanged(object sender, EventArgs e)
        {
            this.gridCentralizedPPALeftBS.DataSource = this.detailgridbs.DataSource;
        }

        private void RefreshAutomatedLineMappingSummary()
        {
            // LBRByGSDTime
            this.CurrentMaintain["LBRByGSDTime"] = MyUtility.Math.Round(MyUtility.Convert.GetDecimal(this.CurrentMaintain["TotalGSDTime"]) / MyUtility.Convert.GetDecimal(this.CurrentMaintain["HighestGSDTime"]) / MyUtility.Convert.GetDecimal(this.CurrentMaintain["SewerManpower"]) * 100, 0);

            // AvgGSDTime
            this.CurrentMaintain["AvgGSDTime"] = MyUtility.Math.Round(MyUtility.Convert.GetDecimal(this.CurrentMaintain["TotalGSDTime"]) / MyUtility.Convert.GetDecimal(this.CurrentMaintain["SewerManpower"]), 2);

            // TotalSewingLineOptrs
            this.CurrentMaintain["TotalSewingLineOptrs"] = MyUtility.Convert.GetInt(this.CurrentMaintain["SewerManpower"]) + MyUtility.Convert.GetInt(this.CurrentMaintain["PresserManpower"]) + MyUtility.Convert.GetInt(this.CurrentMaintain["PackerManpower"]);

            // TargetHr
            this.CurrentMaintain["TargetHr"] = MyUtility.Math.Round(3600 * MyUtility.Convert.GetDecimal(this.CurrentMaintain["SewerManpower"]) / MyUtility.Convert.GetDecimal(this.CurrentMaintain["TotalGSDTime"]), 0);

            // DailyDemand
            this.CurrentMaintain["DailyDemand"] = MyUtility.Math.Round(MyUtility.Convert.GetDecimal(this.CurrentMaintain["TargetHr"]) * MyUtility.Convert.GetDecimal(this.CurrentMaintain["WorkHour"]), 0);

            // TaktTime
            this.CurrentMaintain["TaktTime"] = MyUtility.Math.Round(3600 * MyUtility.Convert.GetDecimal(this.CurrentMaintain["WorkHour"]) / MyUtility.Convert.GetDecimal(this.CurrentMaintain["DailyDemand"]), 2);

            // EOLR
            this.CurrentMaintain["EOLR"] = MyUtility.Math.Round(3600 / MyUtility.Convert.GetDecimal(this.CurrentMaintain["HighestGSDTime"]), 2);

            // PPH
            this.CurrentMaintain["PPH"] = MyUtility.Math.Round(MyUtility.Convert.GetDecimal(this.CurrentMaintain["EOLR"]) * MyUtility.Convert.GetDecimal(this.CurrentMaintain["StyleCPU"]) / MyUtility.Convert.GetDecimal(this.CurrentMaintain["SewerManpower"]), 2);
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
                                    decimal highestGSDTime = groupItem.GroupBy(s => s["No"].ToString()).Max(groupSubItem => groupSubItem.Sum(s => (decimal)s["GSD"] * (decimal)s["SewerDiffPercentage"]));
                                    return new
                                    {
                                        highestGSDTime,
                                        TotalGSD = MyUtility.Math.Round(groupItem.Sum(s => (decimal)s["GSD"] * (decimal)s["SewerDiffPercentage"]), 2),
                                        groupItem.Key.SewerManpower,
                                        LBR = MyUtility.Math.Round(MyUtility.Math.Round(groupItem.Sum(s => (decimal)s["GSD"] * (decimal)s["SewerDiffPercentage"]), 2) / highestGSDTime / groupItem.Key.SewerManpower * 100, 0),
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
            this.p05_NotHitTargetReason.ShowDialog();
        }

        private void BtnEditOperation_Click(object sender, EventArgs e)
        {
            if (!this.DetailDatas.Any(s => MyUtility.Convert.GetBool(s["Selected"])))
            {
                MyUtility.Msg.WarningBox("Please tick at least one Operation.");
                return;
            }

            P05_EditOperation p05_EditOperation = new P05_EditOperation((DataTable)this.detailgridbs.DataSource);
            DialogResult dialogResult = p05_EditOperation.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                this.detailgridbs.DataSource = p05_EditOperation.dtAutomatedLineMapping_Detail;
            }
        }

        private void ChartBtn_Click(object sender, EventArgs e)
        {
            int firstDisplaySewermanpower = MyUtility.Convert.GetInt(((Win.UI.Button)sender).Text);
            if (this.EditMode)
            {
                P05_LBR p05_LBR = new P05_LBR(firstDisplaySewermanpower, this.CurrentMaintain, (DataTable)this.detailgridbs.DataSource, this.dtAutomatedLineMapping_DetailTemp, this.dtAutomatedLineMapping_DetailAuto);
                p05_LBR.ShowDialog();
                this.RefreshAutomatedLineMappingSummary();
                this.FilterGrid();
                this.ShowLBRChart(this.CurrentMaintain);
            }
            else
            {
                P05_Compare p05_Compare = new P05_Compare(firstDisplaySewermanpower, this.CurrentMaintain, (DataTable)this.detailgridbs.DataSource, this.dtAutomatedLineMapping_DetailTemp, this.dtAutomatedLineMapping_DetailAuto);
                p05_Compare.ShowDialog();
            }
        }
    }
}
