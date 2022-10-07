using Ict.Win;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Quality
{
    /// <summary>
    /// B32
    /// </summary>
    public partial class B32 : Sci.Win.Tems.Input6
    {
        /// <summary>
        /// B32
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public B32(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();

            MyUtility.Tool.SetupCombox(this.comboShift, 2, 1, ",,D,Day,N,Night");
        }

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtfactory.ReadOnly = true;
            this.comboShift.ReadOnly = true;
            this.dateStartDate.ReadOnly = true;
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();

            DataGridViewGeneratorTextColumnSettings settingSubProMachineID = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorNumericColumnSettings settingTarget = new DataGridViewGeneratorNumericColumnSettings();

            settingSubProMachineID.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                if (MyUtility.Check.Empty(this.CurrentMaintain["SubprocessID"]) || MyUtility.Check.Empty(this.CurrentMaintain["FactoryID"]))
                {
                    MyUtility.Msg.WarningBox("Please input <Subprcess>, <Factory> first.");
                    e.FormattedValue = string.Empty;
                    return;
                }

                if (MyUtility.Check.Empty(e.FormattedValue))
                {
                    return;
                }

                string subProMachineID = e.FormattedValue.ToString();

                bool isExistsSubProMachine = MyUtility.Check.Seek($"select 1 from SubProMachine where FactoryID = '{this.CurrentMaintain["FactoryID"]}' and SubProcessID = '{this.CurrentMaintain["Subprocess"]}' and ID = '{subProMachineID}'");
                if (!isExistsSubProMachine)
                {
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox($"There is no <Subprocess Machine>: {subProMachineID}");
                    return;
                }
            };

            settingSubProMachineID.EditingMouseDown += (s, e) =>
            {
                if (e.Button != MouseButtons.Right)
                {
                    return;
                }

                if (MyUtility.Check.Empty(this.CurrentMaintain["SubprocessID"]) || MyUtility.Check.Empty(this.CurrentMaintain["FactoryID"]))
                {
                    MyUtility.Msg.WarningBox("Please input <Subprcess>, <Factory> first.");
                    return;
                }

                string sqlPopSubProMachine = $"select ID, Description from SubProMachine where FactoryID = '{this.CurrentMaintain["FactoryID"]}' and SubProcessID = '{this.CurrentMaintain["SubprocessID"]}'";

                SelectItem selectItem = new SelectItem(sqlPopSubProMachine, "15,40", null);

                DialogResult dialogResult = selectItem.ShowDialog();

                if (dialogResult != DialogResult.OK)
                {
                    return;
                }

                this.CurrentDetailData["SubProMachineID"] = selectItem.GetSelecteds()[0]["ID"];
                this.CurrentDetailData.EndEdit();
            };

            settingTarget.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                int curTarget = MyUtility.Convert.GetInt(e.FormattedValue);

                if (curTarget < 1)
                {
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox("Target only enter greater than 0");
                    return;
                }
            };

            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("SubProMachineID", header: "Cutting Machine", width: Widths.AnsiChars(25), iseditingreadonly: true, settings: settingSubProMachineID)
                .Numeric("Target", header: "Target", width: Widths.AnsiChars(10), iseditingreadonly: false, settings: settingTarget);
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["FactoryID"]) ||
                MyUtility.Check.Empty(this.CurrentMaintain["Shift"]) ||
                MyUtility.Check.Empty(this.CurrentMaintain["SubprocessID"]) ||
                MyUtility.Check.Empty(this.CurrentMaintain["StartDate"]) ||
                MyUtility.Check.Empty(this.CurrentMaintain["BeginTime"]) ||
                MyUtility.Check.Empty(this.CurrentMaintain["EndTime"]))
            {
                MyUtility.Msg.WarningBox("<Factory>, <Subprocess>, <Shift>, < Start Date>, <Begin Time> and <End Time> cannot empty.");

                return false;
            }

            if (this.CurrentMaintain["BeginTime"].ToString() == this.CurrentMaintain["EndTime"].ToString())
            {
                MyUtility.Msg.WarningBox("<Begin Time> and <End Time> cannot be same.");
                return false;
            }

            bool isDetailNotComplete = this.DetailDatas.Any(s => MyUtility.Check.Empty(s["SubProMachineID"]) || MyUtility.Check.Empty(s["Target"]));

            if (isDetailNotComplete)
            {
                MyUtility.Msg.WarningBox("Please maintain <Target> for each <Cutting Machine>");
                return false;
            }

            return base.ClickSaveBefore();
        }
    }
}
