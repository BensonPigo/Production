using Ict;
using Ict.Win;
using Sci.Production.CallPmsAPI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Sci.Production.IE.AutoLineMappingGridSyncScroll;

namespace Sci.Production.IE
{
    /// <summary>
    /// P05_LBR
    /// </summary>
    public partial class P05_LBR : Sci.Win.Tems.QueryForm
    {
        private DataTable dtAutomatedLineMapping_Detail;
        private DataTable dtAutomatedLineMapping_DetailTemp;
        private DataTable dtAutomatedLineMapping_DetailAuto;
        private DataTable[] dtAutomatedLineMapping_DetailCopys;
        private DataRow drMain;
        private int firstDiaplaySewerManpower;
        private AutoLineMappingGridSyncScroll autoLineMappingGridSyncScroll;
        private int minSewermanpower;

        /// <summary>
        /// P05_LBR
        /// </summary>
        /// <param name="firstDiaplaySewerManpower">firstDiaplaySewerManpower</param>
        /// <param name="drMain">drMain</param>
        /// <param name="dtAutomatedLineMapping_Detail">dtAutomatedLineMapping_Detail</param>
        /// <param name="dtAutomatedLineMapping_DetailTemp">dtAutomatedLineMapping_DetailTemp</param>
        /// <param name="dtAutomatedLineMapping_DetailAuto">dtAutomatedLineMapping_DetailAuto</param>
        public P05_LBR(int firstDiaplaySewerManpower, DataRow drMain, DataTable dtAutomatedLineMapping_Detail, DataTable dtAutomatedLineMapping_DetailTemp, DataTable dtAutomatedLineMapping_DetailAuto)
        {
            this.InitializeComponent();
            this.DialogResult = DialogResult.Cancel;
            this.dtAutomatedLineMapping_Detail = dtAutomatedLineMapping_Detail;
            this.dtAutomatedLineMapping_DetailTemp = dtAutomatedLineMapping_DetailTemp;
            this.dtAutomatedLineMapping_DetailAuto = dtAutomatedLineMapping_DetailAuto;
            this.dtAutomatedLineMapping_DetailCopys = dtAutomatedLineMapping_DetailTemp.AsEnumerable()
                                                     .GroupBy(s => MyUtility.Convert.GetInt(s["SewerManpower"]))
                                                     .OrderBy(s => s.Key)
                                                     .Select(groupItem => groupItem.CopyToDataTable())
                                                     .ToArray();

            this.firstDiaplaySewerManpower = firstDiaplaySewerManpower;
            this.drMain = drMain;
            this.gridMain.DataSource = this.gridMainBs;
            this.gridMainBs.Filter = " PPA <> 'C' and IsNonSewingLine = 0";
            this.minSewermanpower = dtAutomatedLineMapping_DetailTemp.AsEnumerable().Select(s => MyUtility.Convert.GetInt(s["SewerManpower"])).Min();

            this.autoLineMappingGridSyncScroll = new AutoLineMappingGridSyncScroll(this.gridMain, this.gridSub, "No", SubGridType.LineMapping);

            this.tabNoOfOperator.SelectedIndexChanged += this.TabNoOfOperator_SelectedIndexChanged;
            this.gridMain.CellFormatting += this.GridMain_CellFormatting;
        }

        private void GridMain_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            Win.UI.Grid sourceGrid = (Win.UI.Grid)sender;
            DataRow dr = sourceGrid.GetDataRow(e.RowIndex);
            if (e.ColumnIndex > 1)
            {
                e.CellStyle.BackColor = MyUtility.Convert.GetInt(dr["TimeStudyDetailUkeyCnt"]) > 1 ? Color.FromArgb(255, 255, 153) : sourceGrid.DefaultCellStyle.BackColor;
            }
        }

        private void TabNoOfOperator_SelectedIndexChanged(object sender, EventArgs e)
        {
            int displaySewermanpower = this.minSewermanpower + this.tabNoOfOperator.SelectedIndex;
            this.LoadAutomatedLineMapping(displaySewermanpower);
            this.tabNoOfOperator.SelectedTab.Controls.Add(this.splitViewOperator);
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.Helper.Controls.Grid.Generator(this.gridMain)
               .Text("No", header: "No", width: Widths.AnsiChars(4))
               .Text("Location", header: "Location", width: Widths.AnsiChars(13), iseditingreadonly: true)
               .Text("PPADesc", header: "PPA", width: Widths.AnsiChars(13), iseditingreadonly: true)
               .Text("MachineTypeID", header: "ST/MC type", width: Widths.AnsiChars(10), iseditingreadonly: true)
               .Text("MasterPlusGroup", header: "MC Group", width: Widths.AnsiChars(10), iseditingreadonly: true)
               .Text("OperationDesc", header: "Operation", width: Widths.AnsiChars(13), iseditingreadonly: true)
               .Text("Annotation", header: "Annotation", width: Widths.AnsiChars(23), iseditingreadonly: true)
               .Text("Attachment", header: "Attachment", width: Widths.AnsiChars(10))
               .Text("SewingMachineAttachmentID", header: "Part ID", width: Widths.AnsiChars(25))
               .Text("Template", header: "Template", width: Widths.AnsiChars(10))
               .Numeric("GSD", header: "GSD Time", width: Widths.AnsiChars(5), decimal_places: 2, iseditingreadonly: true)
               .Numeric("SewerDiffPercentageDesc", header: "%", width: Widths.AnsiChars(5), iseditingreadonly: true)
               .Numeric("DivSewer", header: "Div. Sewer", decimal_places: 4, width: Widths.AnsiChars(5), iseditingreadonly: true)
               .Numeric("OriSewer", header: "Ori. Sewer", decimal_places: 4, width: Widths.AnsiChars(5), iseditingreadonly: true)
               .Text("ThreadComboID", header: "Thread" + Environment.NewLine + "Combination", width: Widths.AnsiChars(10));

            this.Helper.Controls.Grid.Generator(this.gridSub)
               .Text("No", header: "No. Of" + Environment.NewLine + "Station", width: Widths.AnsiChars(10))
               .Text("TotalGSDTime", header: "Total" + Environment.NewLine + "GSD Time", width: Widths.AnsiChars(10))
               .Text("OperatorLoading", header: "Operator" + Environment.NewLine + "Loading (%)", width: Widths.AnsiChars(10));

            int displayTabIndex = this.firstDiaplaySewerManpower - this.minSewermanpower;
            if (displayTabIndex == this.tabNoOfOperator.SelectedIndex)
            {
                this.LoadAutomatedLineMapping(this.firstDiaplaySewerManpower);
            }
            else
            {
                this.tabNoOfOperator.SelectTab(displayTabIndex);
            }
        }

        private void LoadAutomatedLineMapping(int sewerManpower)
        {
            this.gridMainBs.DataSource = this.dtAutomatedLineMapping_DetailCopys[sewerManpower - this.minSewermanpower];
            this.autoLineMappingGridSyncScroll.RefreshSubData();
            this.numSewerManpower.Value = this.autoLineMappingGridSyncScroll.SewerManpower;
            this.numOPLoading.Value = this.autoLineMappingGridSyncScroll.HighestLoading;
            this.numLBR.Value = this.autoLineMappingGridSyncScroll.LBR;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            try
            {
                this.dtAutomatedLineMapping_DetailCopys[this.tabNoOfOperator.SelectedIndex] =
                    this.dtAutomatedLineMapping_DetailAuto.AsEnumerable()
                        .Where(s => MyUtility.Convert.GetDecimal(s["SewerManpower"]) == this.numSewerManpower.Value).CopyToDataTable();
                this.gridMainBs.DataSource = this.dtAutomatedLineMapping_DetailCopys[this.tabNoOfOperator.SelectedIndex];
                this.autoLineMappingGridSyncScroll.RefreshSubData();
            }
            catch
            {
                MyUtility.Msg.ErrorBox("Reset failed, the data source is irregular, please undo.");
                return;
            }
        }

        private void BtnReload_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MyUtility.Msg.QuestionBox("Are you sure you will reload this data sheet into?", "Reload");

            if (dialogResult != DialogResult.Yes)
            {
                return;
            }

            int loadSewermanpower = MyUtility.Convert.GetInt(this.numSewerManpower.Value);
            int tempIndex = MyUtility.Convert.GetInt(this.numSewerManpower.Value) - this.minSewermanpower;
            int curSewermanpower = MyUtility.Convert.GetInt(this.drMain["SewerManpower"]);

            try
            {
                // 將目前Detail搬到對應Temp
                this.RemoveRowBySewermanpower(this.dtAutomatedLineMapping_DetailTemp, curSewermanpower);

                this.dtAutomatedLineMapping_Detail.MergeTo(ref this.dtAutomatedLineMapping_DetailTemp);
                foreach (DataRow dr in this.dtAutomatedLineMapping_DetailTemp.AsEnumerable().Where(s => MyUtility.Check.Empty(s["SewerManpower"])))
                {
                    dr["SewerManpower"] = curSewermanpower;
                }

                // 因為有可能有按Reset，所以要將curSewermanpower以外的頁面佔存資料回寫AutomatedLineMapping_DetailTemp
                for (int i = this.minSewermanpower; i < (this.dtAutomatedLineMapping_DetailCopys.Length + this.minSewermanpower); i++)
                {
                    if (i == curSewermanpower)
                    {
                        continue;
                    }

                    this.RemoveRowBySewermanpower(this.dtAutomatedLineMapping_DetailTemp, i);

                    int srcIndex = i - this.minSewermanpower;

                    this.dtAutomatedLineMapping_DetailCopys[srcIndex].MergeTo(ref this.dtAutomatedLineMapping_DetailTemp);
                }

                // 將Detail刪除
                foreach (DataRow dr in this.dtAutomatedLineMapping_Detail.ToList())
                {
                    this.dtAutomatedLineMapping_Detail.Rows.Remove(dr);
                }

                // 將目前所選擇資料組塞入Detail
                this.dtAutomatedLineMapping_DetailCopys[tempIndex].MergeTo(ref this.dtAutomatedLineMapping_Detail, false);

                foreach (DataRow dr in this.dtAutomatedLineMapping_Detail.Rows)
                {
                    dr["Selected"] = false;
                }

                // 改表頭的SewerManpower
                this.drMain["SewerManpower"] = loadSewermanpower;
                this.drMain["HighestGSDTime"] = this.autoLineMappingGridSyncScroll.HighestGSD;
            }
            catch
            {
                MyUtility.Msg.ErrorBox("Reload failed, the data source is irregular, please undo.");
                return;
            }

            MyUtility.Msg.InfoBox("Reload complete");
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void RemoveRowBySewermanpower(DataTable dtTarget, int sewermanpower)
        {
            var needDeleteOldData = dtTarget.AsEnumerable().Where(s => MyUtility.Convert.GetInt(s["SewerManpower"]) == sewermanpower);

            foreach (var itemDelete in needDeleteOldData.ToList())
            {
                dtTarget.Rows.Remove(itemDelete);
            }
        }
    }
}
