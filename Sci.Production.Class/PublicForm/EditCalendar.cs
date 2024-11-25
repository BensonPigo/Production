using Ict;
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
        public EditCalendar(DataTable dtMachineIoT_Calendar_Detail, int weekday)
        {
            this.InitializeComponent();
            this.dtMachineIoT_Calendar_Detail = dtMachineIoT_Calendar_Detail;
            this.SetWeekDayCheckBox(weekday);
            this.SetDataTable(weekday);
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.GridSetup();
        }

        private void GridSetup()
        {
            this.grid1.IsEditingReadOnly = false;

            this.Helper.Controls.Grid.Generator(this.grid1)
                .TimeSpanHHmm("StartTime", header: "Start")
                .TimeSpanHHmm("EndTime", header: "End")
                .CheckBox("IsCrossDate", header: "Cross-day", width: Widths.AnsiChars(5), trueValue: true, falseValue: false)
                ;

            // 關閉排序功能
            this.grid1.Columns.DisableSortable();
        }

        private void SetDataTable(int weekday)
        {
            this.dt = new MachineCalendar().GetMachineIoT_Calendar_DetailSchema();
            foreach (DataRow dr in this.dtMachineIoT_Calendar_Detail.Select($"WeekDay = {weekday}"))
            {
                this.dt.ImportRow(dr);
            }

            this.listControlBindingSource1.DataSource = this.dt;
        }

        private void SetWeekDayCheckBox(int weekday)
        {
            // 動態尋找對應的 CheckBox
            if (this.panel1.Controls.Find($"checkBox{weekday}", true).FirstOrDefault() is CheckBox checkBox)
            {
                checkBox.Checked = true;
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            this.grid1.ValidateControl();
            if (!new MachineCalendar().ValidateDataTableTime(this.dt, out DataTable dtFormatTime))
            {
                return;
            }

            // 依據有勾選星期幾展開資料, 返回前一畫面顯示
            foreach (Control control in this.panel1.Controls)
            {
                if (control is Win.UI.CheckBox checkBox && checkBox.Checked)
                {
                    // 取得 CheckBox 名稱中的數字部分，例如：checkBox1 -> 1
                    int weekDay = int.Parse(checkBox.Name.Replace("checkBox", string.Empty));

                    this.dtMachineIoT_Calendar_Detail.Select($"WeekDay = {weekDay}").ToList().ForEach(row => row.Delete());

                    foreach (DataRow dr in dtFormatTime.Rows)
                    {
                        dr["WeekDay"] = weekDay;

                        this.dtMachineIoT_Calendar_Detail.ImportRow(dr);
                    }
                }
            }

            this.Close();
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
            this.dt.AcceptChanges();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
