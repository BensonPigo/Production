using Ict.Win;
using Sci.Production.Class.Command;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Sci.Production.Class.PublicForm
{
    /// <inheritdoc/>
    public partial class EditCalendar : Win.Tems.QueryForm
    {
        private DataTable dtMachineIoT_Calendar_Detail;
        private DataTable dt;

        /// <inheritdoc/>
        public EditCalendar(DataTable dtMachineIoT_Calendar_Detail)
        {
            this.InitializeComponent();
            this.dtMachineIoT_Calendar_Detail = dtMachineIoT_Calendar_Detail;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.GridSetup();
            this.SetDataTable();
        }

        private void GridSetup()
        {
            this.grid1.IsEditingReadOnly = false;

            this.Helper.Controls.Grid.Generator(this.grid1)
                .TimeSpanHHmm("StartTime", header: "Start")
                .TimeSpanHHmm("EndTime", header: "End")
                .CheckBox("IsCrossDate", header: "Cross-day", width: Widths.AnsiChars(5))
                ;

            // 關閉排序功能
            for (int i = 0; i < this.grid1.ColumnCount; i++)
            {
                this.grid1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private void SetDataTable()
        {
            this.dt = new MachineCalendar().GetMachineIoT_Calendar_DetailSchema();
            this.listControlBindingSource1.DataSource = this.dt;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!this.SaveBefore())
            {
                return;
            }

            DataTable dt = this.dt.Copy();
            foreach (DataRow dr in dt.Rows)
            {
                dr["StartTime"] = dr["StartTime"].ToTimeFormat();
                dr["EndTime"] = dr["EndTime"].ToTimeFormat();
            }

            foreach (Control control in this.panel1.Controls)
            {
                if (control is Win.UI.CheckBox checkBox && checkBox.Checked)
                {
                    // 取得 CheckBox 名稱中的數字部分，例如：checkBox1 -> 1
                    int weekDay = int.Parse(checkBox.Name.Replace("checkBox", string.Empty));

                    this.dtMachineIoT_Calendar_Detail.Select($"WeekDay = {weekDay}").ToList().ForEach(row => row.Delete());

                    foreach (DataRow dr in dt.Rows)
                    {
                        dr["WeekDay"] = weekDay;

                        this.dtMachineIoT_Calendar_Detail.ImportRow(dr);
                    }
                }
            }

            this.Close();
        }

        private bool SaveBefore()
        {
            // 移除 StartTime 或 EndTime 為空的資料列
            this.dt.AsEnumerable().Where(row => MyUtility.Check.Empty(row["StartTime"]) || MyUtility.Check.Empty(row["EndTime"])).ToList().ForEach(row => row.Delete());
            this.dt.AcceptChanges();

            if (!this.ValidateStartEndTime())
            {
                return false;
            }

            // 根據 isCrossDate 和 StartTime 排序資料
            DataView dataView = this.dt.DefaultView;
            dataView.Sort = "isCrossDate ASC, StartTime ASC";
            DataTable sortedDt = dataView.ToTable();

            // 驗證每一列，確保時間段沒有重疊或直接連續
            for (int i = 0; i < sortedDt.Rows.Count - 1; i++)
            {
                bool currentIsCrossDate = Convert.ToBoolean(sortedDt.Rows[i]["isCrossDate"]);
                bool nextIsCrossDate = Convert.ToBoolean(sortedDt.Rows[i + 1]["isCrossDate"]);
                TimeSpan currentEndTime = TimeSpan.Parse(sortedDt.Rows[i]["EndTime"].ToString());
                TimeSpan nextStartTime = TimeSpan.Parse(sortedDt.Rows[i + 1]["StartTime"].ToString());

                // 檢查時間重疊的情況
                if (currentIsCrossDate == nextIsCrossDate && currentEndTime >= nextStartTime)
                {
                    MyUtility.Msg.WarningBox("Start time must be greater than the End time of the previous period.");
                    return false;
                }
            }

            return true;
        }

        private bool ValidateStartEndTime()
        {
            foreach (DataRow row in this.dt.Rows)
            {
                if (TimeSpan.TryParse(row["StartTime"].ToTimeFormat(), out TimeSpan startTime) &&
                    TimeSpan.TryParse(row["EndTime"].ToTimeFormat(), out TimeSpan endTime))
                {
                    if (startTime >= endTime)
                    {
                        MyUtility.Msg.WarningBox("The start time cannot be greater than the end time.");
                        return false;
                    }
                }
            }

            return true;
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            this.dt.Clear();
            this.dt.AcceptChanges();
            foreach (Control control in this.panel1.Controls)
            {
                if (control is Win.UI.CheckBox checkBox)
                {
                    checkBox.Checked = false;
                }
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            this.dt.RowsAdd();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (this.grid1.CurrentDataRow == null)
            {
                return;
            }

            this.dt.Rows.Remove(this.grid1.CurrentDataRow);
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
