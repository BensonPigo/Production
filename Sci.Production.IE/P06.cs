using Ict;
using Ict.Win;
using Ict.Win.Tools;
using Microsoft.SqlServer.Management.Smo.Agent;
using Sci.Data;
using Sci.Production.Class;
using Sci.Production.Class.Command;
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
    /// P06
    /// </summary>
    public partial class P06 : Sci.Win.Tems.Input6
    {
        private P06_NotHitTargetReason P06_NotHitTargetReason;
        private DataTable EmployeeData;
        private AutoLineMappingGridSyncScroll lineMappingGrids;
        private AutoLineMappingGridSyncScroll centralizedPPAGrids;

        /// <summary>
        /// P06
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P06(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            MyUtility.Tool.SetupCombox(this.comboPhase, 2, 1, ",,Final,Final");

            this.splitLineMapping.Panel1.Controls.Add(this.detailgrid);
            this.gridCentralizedPPALeft.SupportEditMode = Win.UI.AdvEditModesReadOnly.True;
            this.gridCentralizedPPALeft.DataSource = this.gridCentralizedPPALeftBS;

            this.detailgrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.gridCentralizedPPALeft.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.detailgrid.CellFormatting += this.Detailgrid_CellFormatting;

            this.lineMappingGrids = new AutoLineMappingGridSyncScroll(this.detailgrid, this.gridLineMappingRight, "No", SubGridType.LineMappingBalancing);
            this.centralizedPPAGrids = new AutoLineMappingGridSyncScroll(this.gridCentralizedPPALeft, this.gridCentralizedPPARight, "No", SubGridType.BalancingCentrailizedPPA);

            this.txtSewingline.FactoryobjectName = this.txtfactory;
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
            new P06_Print(this.CurrentMaintain["ID"].ToString()).ShowDialog();
            return base.ClickPrint();
        }

        /// <inheritdoc/>
        protected override void ClickUndo()
        {
            base.ClickUndo();
            this.RefreshLineMappingBalancingSummary();
        }

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();
            this.DetailSelectCommand =
                $@"
select  cast(0 as bit) as Selected,
        ad.*,
        [PPADesc] = isnull(d.Name, ''),
        [OperationDesc] = iif(isnull(op.DescEN, '') = '', ad.OperationID, op.DescEN),
        [SewerDiffPercentageDesc] = round(ad.SewerDiffPercentage * 100, 0),
        [TimeStudyDetailUkeyCnt] = Count(TimeStudyDetailUkey) over (partition by TimeStudyDetailUkey),
        [EmployeeName] = e.Name,
        [EmployeeSkill] = e.Skill
from LineMappingBalancing_Detail ad WITH (NOLOCK)
left join DropDownList d with (nolock) on d.ID = ad.PPA  and d.Type = 'PMS_IEPPA'
left join Operation op with (nolock) on op.ID = ad.OperationID
left join Employee e with (nolock) on e.FactoryID = '{e.Master["FactoryID"]}' and e.ID = ad.EmployeeID
where ad.ID = '{masterID}'
order by iif(ad.No = '', 'ZZ', ad.No), ad.Seq";

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
            this.CurrentMaintain["Phase"] = string.Empty;
            this.CurrentMaintain["AddName"] = Env.User.UserID;
            this.CurrentMaintain["EditName"] = string.Empty;
            this.CurrentMaintain["EditDate"] = DBNull.Value;
            this.CurrentMaintain["ID"] = DBNull.Value;

            return true;
        }

        /// <inheritdoc/>
        protected override void ClickConfirm()
        {
            var listEmptyReason = this.P06_NotHitTargetReason.DataNotHitTargetReason.AsEnumerable().Where(s => MyUtility.Check.Empty(s["IEReasonID"]));

            if (listEmptyReason.Any())
            {
                MyUtility.Msg.WarningBox($"Not Hit Target Reason {listEmptyReason.Select(s => s["No"].ToString()).JoinToString(",")} has not yet input the reason, cannot be confirm.");
                this.P06_NotHitTargetReason.ShowDialog();
                return;
            }

            string sqlGetLBRCondition = $@"
SELECT ALMCS.Condition1 
FROM AutomatedLineMappingConditionSetting ALMCS
WHERE ALMCS.[FactoryID] = '{this.CurrentMaintain["FactoryID"]}'
AND ALMCS.Functions = 'IE_P06'
AND ALMCS.Verify = 'LBRByCycle'
AND ALMCS.Junk = 0
";
            decimal checkLBRCondition = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup(sqlGetLBRCondition));

            if (checkLBRCondition > 0 &&
                MyUtility.Convert.GetDecimal(this.CurrentMaintain["LBRByGSDTime"]) < checkLBRCondition)
            {
                DialogResult dialogResult = MyUtility.Msg.QuestionBox($"[LBR By Cycle Time(%)] should not be lower than {checkLBRCondition}%, please double check and revise it, thanks!");

                if (dialogResult != DialogResult.Yes)
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
            if (MyUtility.Check.Empty(this.CurrentMaintain["Version"]))
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
            DataGridViewGeneratorTextColumnSettings colOperator = new DataGridViewGeneratorTextColumnSettings();

            colCycleTime.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                decimal curCycle = MyUtility.Convert.GetDecimal(e.FormattedValue);

                // 更新同樣TimeStudyDetailUkey的Cycle
                foreach (DataRow updateRow in this.DetailDatas)
                {
                    if (MyUtility.Convert.GetLong(updateRow["TimeStudyDetailUkey"]) == MyUtility.Convert.GetLong(dr["TimeStudyDetailUkey"]))
                    {
                        updateRow["Cycle"] = curCycle;
                    }
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
            };

            colSelected.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                bool isChecked = MyUtility.Convert.GetBool(e.FormattedValue);

                // 第一筆勾選時，將上下各五筆No保留Enable，其餘disable不能勾選
                if (isChecked && !this.DetailDatas.Any(row => (bool)row["Selected"]))
                {
                    var listNo = this.DetailDatas.OrderBy(row => row["No"]).Select(row => row["No"].ToString()).Distinct().ToList();
                    int targetIndex = listNo.IndexOf(dr["No"].ToString());
                    int skipCount = Math.Max(targetIndex - 5, 0); // 計算要跳過的元素個數
                    int takeCount = (targetIndex < 6) ? targetIndex + 6 : 11; // 計算要取出的元素個數

                    var listEnableNo = listNo.Skip(skipCount).Take(takeCount).Where(row => row != dr["No"].ToString());

                    foreach (DataGridViewRow gridRow in this.detailgrid.Rows)
                    {
                        if (gridRow.Cells["No"].Value.ToString() == dr["No"].ToString())
                        {
                            continue;
                        }

                        gridRow.Cells["Selected"].ReadOnly = !listEnableNo.Contains(gridRow.Cells["No"].Value.ToString());
                    }
                }

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

            #region Operator ID No.的按右鍵與Validating
            colOperator.EditingMouseDown += (s, e) =>
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

                if (MyUtility.Check.Empty(this.CurrentMaintain["SewingLineID"]))
                {
                    MyUtility.Msg.WarningBox("Please input the [Sewing Line] before input the [Operator ID]");
                    return;
                }

                DataGridView sourceGrid = ((DataGridViewColumn)s).DataGridView;

                if (e.RowIndex != -1)
                {
                    DataRow dr = sourceGrid.GetDataRow<DataRow>(e.RowIndex);

                    this.GetEmployee(null, this.CurrentMaintain["FactoryID"].ToString(), this.CurrentMaintain["SewingLineID"].ToString());

                    Win.Tools.SelectItem item = new Win.Tools.SelectItem(this.EmployeeData, "ID,Name,Skill,SewingLineID,FactoryID", "10,18,16,2,5", dr["EmployeeID"].ToString(), headercaptions: "ID,Name,Skill,SewingLine,Factory")
                    {
                        Width = 700,
                    };
                    DialogResult returnResult = item.ShowDialog();
                    if (returnResult == DialogResult.Cancel)
                    {
                        return;
                    }

                    IList<DataRow> selectedData = item.GetSelecteds();
                    dr["EmployeeID"] = selectedData[0]["ID"];
                    dr["EmployeeName"] = selectedData[0]["Name"];
                    dr["EmployeeSkill"] = selectedData[0]["Skill"];
                    dr.EndEdit();

                    foreach (DataRow drDetail in this.DetailDatas.Where(row => row["No"].ToString() == dr["No"].ToString()))
                    {
                        drDetail["EmployeeID"] = selectedData[0]["ID"];
                        drDetail["EmployeeName"] = selectedData[0]["Name"];
                        drDetail["EmployeeSkill"] = selectedData[0]["Skill"];
                    }
                }
            };

            colOperator.CellValidating += (s, e) =>
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

                if (MyUtility.Check.Empty(this.CurrentMaintain["SewingLineID"]))
                {
                    MyUtility.Msg.WarningBox("Please input the [Sewing Line] before input the [Operator ID]");
                    this.ReviseEmployeeToEmpty(dr);
                    return;
                }

                if (e.FormattedValue.ToString() != dr["EmployeeID"].ToString())
                {
                    this.GetEmployee(e.FormattedValue.ToString(), this.CurrentMaintain["FactoryID"].ToString(), this.CurrentMaintain["SewingLineID"].ToString());
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
            #endregion

            this.Helper.Controls.Grid.Generator(this.detailgrid)
               .Text("No", header: "No", width: Widths.AnsiChars(4), iseditingreadonly: true)
               .CheckBox("Selected", string.Empty, trueValue: true, falseValue: false, iseditable: true, settings: colSelected)
               .Text("Location", header: "Location", width: Widths.AnsiChars(13), iseditingreadonly: true)
               .Text("PPADesc", header: "PPA", width: Widths.AnsiChars(13), iseditingreadonly: true)
               .CellMachineType("MachineTypeID", "ST/MC type", this, width: Widths.AnsiChars(10))
               .Text("MasterPlusGroup", header: "MC Group", width: Widths.AnsiChars(10), settings: colMachineTypeID)
               .Text("OperationDesc", header: "Operation", width: Widths.AnsiChars(13), iseditingreadonly: true)
               .Text("Annotation", header: "Annotation", width: Widths.AnsiChars(23), iseditingreadonly: true)
               .CellAttachment("Attachment", "Attachment", this, width: Widths.AnsiChars(10))
               .CellPartID("SewingMachineAttachmentID", "Part ID", this, width: Widths.AnsiChars(25))
               .CellTemplate("Template", "Template", this, width: Widths.AnsiChars(10))
               .Numeric("GSD", header: "GSD Time", width: Widths.AnsiChars(5), decimal_places: 2, iseditingreadonly: true)
               .Numeric("Cycle", header: "Cycle Time", width: Widths.AnsiChars(5), decimal_places: 2, settings: colCycleTime)
               .Numeric("SewerDiffPercentageDesc", header: "%", width: Widths.AnsiChars(5), iseditingreadonly: true)
               .Numeric("DivSewer", header: "Div. Sewer", decimal_places: 4, width: Widths.AnsiChars(5), iseditingreadonly: true)
               .Numeric("OriSewer", header: "Ori. Sewer", decimal_places: 4, width: Widths.AnsiChars(5), iseditingreadonly: true)
               .CellThreadComboID("ThreadComboID", "Thread" + Environment.NewLine + "Color", this, width: Widths.AnsiChars(10))
               .Text("Notice", header: "Notice", width: Widths.AnsiChars(10));

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
               .Numeric("Cycle", header: "Cycle Time", width: Widths.AnsiChars(5), decimal_places: 2)
               .Text("Notice", header: "Notice", width: Widths.AnsiChars(10));

            this.Helper.Controls.Grid.Generator(this.gridLineMappingRight)
               .Text("No", header: "No. Of" + Environment.NewLine + "Station", width: Widths.AnsiChars(10), iseditingreadonly: true)
               .Text("TotalCycleTime", header: "Total" + Environment.NewLine + "Cycle Time", width: Widths.AnsiChars(10), iseditingreadonly: true)
               .Text("OperatorLoading", header: "Operator" + Environment.NewLine + "Loading (%)", width: Widths.AnsiChars(10), iseditingreadonly: true)
               .Text("EmployeeID", header: "Operator ID", width: Widths.AnsiChars(10), settings: colOperator)
               .Text("EmployeeName", header: "Operator" + Environment.NewLine + "Name", width: Widths.AnsiChars(10), iseditingreadonly: true)
               .Text("EmployeeSkill", header: "Skill", width: Widths.AnsiChars(10), iseditingreadonly: true)
               .Text("OperatorEffi", header: "Effi (%)", width: Widths.AnsiChars(10), iseditingreadonly: true);

            this.Helper.Controls.Grid.Generator(this.gridCentralizedPPARight)
               .Text("No", header: "No. Of" + Environment.NewLine + "Station", width: Widths.AnsiChars(10), iseditingreadonly: true)
               .Text("TotalCycleTime", header: "Total" + Environment.NewLine + "Cycle Time", width: Widths.AnsiChars(10), iseditingreadonly: true)
               .Text("OperatorLoading", header: "Operator" + Environment.NewLine + "Loading (%)", width: Widths.AnsiChars(10), iseditingreadonly: true)
               .Text("EmployeeID", header: "Operator ID", width: Widths.AnsiChars(10), settings: colOperator)
               .Text("EmployeeName", header: "Operator" + Environment.NewLine + "Name", width: Widths.AnsiChars(10), iseditingreadonly: true)
               .Text("EmployeeSkill", header: "Skill", width: Widths.AnsiChars(10), iseditingreadonly: true)
               .Text("OperatorEffi", header: "Effi (%)", width: Widths.AnsiChars(10), iseditingreadonly: true);

            this.gridLineMappingRight.Columns["EmployeeID"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridCentralizedPPARight.Columns["EmployeeID"].DefaultCellStyle.BackColor = Color.Pink;
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
            this.FilterGrid();
            this.btnEditOperation.Enabled = this.tabDetail.SelectedIndex == 0 && this.EditMode;
        }

        private void FilterGrid()
        {
            if (this.tabDetail.SelectedIndex == 0)
            {
                this.gridCentralizedPPALeftBS.DataSource = null;
                this.detailgridbs.Filter = "PPA <> 'C' and IsNonSewingLine = 0";
                this.lineMappingGrids.RefreshSubData();
            }
            else
            {
                this.gridCentralizedPPALeftBS.DataSource = this.detailgridbs.DataSource;
                this.gridCentralizedPPALeftBS.Filter = "PPA = 'C' and IsNonSewingLine = 0";
                this.centralizedPPAGrids.RefreshSubData();
            }
        }

        private void RefreshLineMappingBalancingSummary()
        {
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

        private void BtnNotHitTargetReason_Click(object sender, EventArgs e)
        {
            this.P06_NotHitTargetReason.ShowDialog();
        }

        private void BtnEditOperation_Click(object sender, EventArgs e)
        {
            if (!this.DetailDatas.Any(s => MyUtility.Convert.GetBool(s["Selected"])))
            {
                MyUtility.Msg.WarningBox("Please tick at least one Operation.");
                return;
            }

            P06_EditOperation p06_EditOperation = new P06_EditOperation((DataTable)this.detailgridbs.DataSource);
            DialogResult dialogResult = p06_EditOperation.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                //this.detailgridbs.DataSource = P06_EditOperation.dtLineMappingBalancing_Detail;
                this.lineMappingGrids.RefreshSubData();
            }
        }

        private void BtnH_Click(object sender, EventArgs e)
        {
            //DataTable dtLineMappingBalancing_DetailAutoDefault = this.dtLineMappingBalancing_Detail.AsEnumerable()
            //    .Where(s => MyUtility.Convert.GetInt(s["SewerManpower"]) == MyUtility.Convert.GetInt(this.CurrentMaintain["SewerManpower"]))
            //    .CopyToDataTable();

            //new P06_Default(dtLineMappingBalancing_DetailAutoDefault).ShowDialog();
        }

        // 撈出Employee資料
        private void GetEmployee(string iD, string factoryID, string sewinglineID)
        {
            string sqlCmd;

            string sqlWhere = string.Empty;

            if (!MyUtility.Check.Empty(iD))
            {
                sqlWhere += $" and ID = '{iD}' ";
            }

            if (!MyUtility.Check.Empty(factoryID))
            {
                sqlWhere += $" and FactoryID = '{factoryID}' ";
            }

            if (!MyUtility.Check.Empty(sewinglineID))
            {
                sqlWhere += $" and SewingLineID = '{sewinglineID}' ";
            }

            sqlCmd = "select ID,Name,Skill,SewingLineID,FactoryID from Employee WITH (NOLOCK) where ResignationDate is null " + sqlWhere;

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
    }
}
