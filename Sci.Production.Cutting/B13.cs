using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Class.Command;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Sci.Production.Cutting
{
    /// <inheritdoc/>
    public partial class B13 : Win.Tems.QueryForm
    {
#pragma warning disable SA1502 // Element should not be on a single line
#pragma warning disable SA1516 // Elements should be separated by blank line
        private string Grid1CurrentColumnName { get { return this.grid1.Columns[this.grid1.CurrentCell.ColumnIndex].DataPropertyName; } }
        private DateTime? Grid1CurrentColumnDate { get { return MyUtility.Convert.GetDate(this.Grid1CurrentColumnName); } }
        private bool RightRegionEnable { get { return this.Grid1CurrentColumnName != "MachineID"; } }
        private long CurrentMachineIoTUkey { get { return MyUtility.Convert.GetLong(this.grid1.CurrentDataRow["Ukey"]); } }
#pragma warning restore SA1516 // Elements should be separated by blank line
#pragma warning restore SA1502 // Element should not be on a single line

        private readonly Color colorIsSpecial = Color.FromArgb(215, 215, 215);
        private readonly Color colorIsWorkingHours0 = Color.FromArgb(190, 252, 211);
        private DataTable dtMachineID;
        private DataTable dtSpecial; // 暫存特殊班表表頭, Grid1_CellFormatting 很需要, 一個個去DB會卡很久
        private DataTable dtCalendar;
        private DataTable dtDisplayMain;

        // 暫存畫面上的篩選條件, 部分操作重刷畫面使用
        private string machineIoTType;
        private DateTime workingDate1;
        private DateTime workingDate2;

        /// <inheritdoc/>
        public B13(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.EditMode = true;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.display1.BackColor = this.colorIsSpecial;
            this.display2.BackColor = this.colorIsWorkingHours0;

            this.dateWorkingDate.Value1 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            this.dateWorkingDate.Value2 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
            this.Grid2Setup();
        }

        private void BtnQuery_Click(object sender, System.EventArgs e)
        {
            // 檢查
            if (!this.BeforeQuery())
            {
                return;
            }

            this.machineIoTType = this.comboDropDownList1.SelectedValue.ToString();
            this.workingDate1 = this.dateWorkingDate.Value1.Value;
            this.workingDate2 = this.dateWorkingDate.Value2.Value;

            this.Query();
        }

        private void Query()
        {
            this.grid1.DataSource = null;
            if (this.QueryData())
            {
                this.GridSetup();
                this.SetGrid1DataTableColumns();
                this.GetMachineCalendar();
            }
        }

        private bool BeforeQuery()
        {
            if (!this.dateWorkingDate.HasValue1 || !this.dateWorkingDate.HasValue2)
            {
                MyUtility.Msg.WarningBox("Working Date cannot be empty.");
                return false;
            }

            // 檢查相差天數是否超過 90 天
            TimeSpan dateDifference = this.dateWorkingDate.Value1.Value - this.dateWorkingDate.Value2.Value;
            if (Math.Abs(dateDifference.TotalDays) > 90)
            {
                MyUtility.Msg.WarningBox("Date range limited to up to 90 days at a time.");
                return false;
            }

            return true;
        }

        private bool QueryData()
        {
            string sqlcmd = $@"
SELECT Ukey, MachineID
INTO #tmpMachineIoT
FROM MachineIoT WITH (NOLOCK)
INNER JOIN [SciProduction_SpreadingNo] sn WITH (NOLOCK) ON sn.ID = MachineIoT.MachineID
WHERE MachineIoTType = '{this.machineIoTType}'
AND sn.MDivisionID = '{Sci.Env.User.Keyword}'

--所有機器
SELECT * FROM #tmpMachineIoT

--特殊班表
SELECT  m.Ukey, m.MachineID, m2.SpecialDate
FROM #tmpMachineIoT m
INNER JOIN MachineIoT_SpecialDate m2 ON m2.MachineIoTUkey = m.Ukey
WHERE m2.SpecialDate BETWEEN '{this.workingDate1:yyyy/MM/dd}' AND '{this.workingDate2:yyyy/MM/dd}'

-- 預計要取班表的 機器&開始日期
SELECT m.Ukey, m.MachineID, StartDate = MIN(m2.StartDate)
FROM #tmpMachineIoT m
INNER JOIN MachineIoT_Calendar m2 ON m2.MachineIoTUkey = m.Ukey
WHERE m2.StartDate <= '{this.workingDate2:yyyy/MM/dd}'
GROUP BY m.Ukey, m.MachineID

DROP TABLE #tmpMachineIoT
";
            DualResult result = DBProxy.Current.Select("ManufacturingExecution", sqlcmd, out DataTable[] dts);
            if (!result)
            {
                this.ShowErr(result);
                return false;
            }

            this.dtMachineID = dts[0];
            this.dtSpecial = dts[1];
            this.dtCalendar = dts[2];
            return true;
        }

        private void GridSetup()
        {
            this.grid1.Columns.Clear();

            // 設定 MachineID 欄位
            this.Helper.Controls.Grid.Generator(this.grid1)
                .Text("MachineID", header: "Machine ID", width: Widths.AnsiChars(2), iseditingreadonly: true);

            // 動態添加日期欄位（僅顯示天數）
            for (DateTime date = this.workingDate1; date <= this.workingDate2; date = date.AddDays(1))
            {
                this.Helper.Controls.Grid.Generator(this.grid1)
                    .Numeric(date.ToString("yyyy/MM/dd"), header: date.ToString("MM/dd"), width: Widths.AnsiChars(3), decimal_places: 1);
                if (date.DayOfWeek == DayOfWeek.Sunday)
                {
                    this.grid1.Columns[date.ToString("yyyy/MM/dd")].HeaderCell.Style.Font = new Font(this.grid1.Font, FontStyle.Bold);
                }
            }

            this.grid1.Columns[0].Frozen = true;
            this.grid1.Columns.DisableSortable();
            this.grid1.AutoResizeColumns();
        }

        private void Grid2Setup()
        {
            this.Helper.Controls.Grid.Generator(this.grid2)
                .Text("StartTimeDisplay", header: "Start Time", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("EndTimeDisplay", header: "End Time", width: Widths.AnsiChars(5), iseditingreadonly: true)
                ;
        }

        private void SetGrid1DataTableColumns()
        {
            // 欄位 UKey, MachineID 以及資訊
            this.dtDisplayMain = this.dtMachineID.Copy();

            // 加入動態日期欄位
            for (DateTime date = this.workingDate1; date <= this.workingDate2; date = date.AddDays(1))
            {
                this.dtDisplayMain.Columns.Add(date.ToString("yyyy/MM/dd"), typeof(string));
            }
        }

        private void GetMachineCalendar()
        {
            // 特殊班表
            foreach (DataRow row in this.dtSpecial.Rows)
            {
                new MachineCalendar().QueryDetailAndSetMainWorkingHour((long)row["Ukey"], (DateTime)row["SpecialDate"], this.dtDisplayMain);
            }

            // 週期性班表
            foreach (DataRow row in this.dtCalendar.Rows)
            {
                DateTime startDate = (DateTime)row["StartDate"];
                startDate = startDate >= this.workingDate1 ? startDate : this.workingDate1; // 從最早日期往後每天都撈取
                for (DateTime date = startDate; date <= this.workingDate2; date = date.AddDays(1))
                {
                    // 有 特殊班表 或 假日 跳過不去 DB 撈
                    if (this.dtSpecial.Select($"Ukey = {row["Ukey"]} AND SpecialDate = '{date:yyyy/MM/dd}'").Length > 0
                        || new MachineCalendar().IsHoliday(date, this.machineIoTType))
                    {
                        continue;
                    }

                    new MachineCalendar().QueryDetailAndSetMainWorkingHour((long)row["Ukey"], date, this.dtDisplayMain);
                }
            }

            this.grid1.DataSource = this.dtDisplayMain;
        }

        private void Grid1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                e.CellStyle.BackColor = Color.White;
                return;
            }

            // 不可用 CurrentCell 或 CurrentDataRow 方式取得, 因為 CellFormatting 觸發順序
            DataRow dr = this.grid1.GetDataRow(e.RowIndex);
            string date = this.grid1.Columns[e.ColumnIndex].DataPropertyName;
            if (this.IsSpecialinTemple((long)dr["Ukey"], MyUtility.Convert.GetDate(date).Value))
            {
                e.CellStyle.BackColor = this.colorIsSpecial;
            }
            else if (MyUtility.Convert.GetString(dr[date]) == "0") // 只有 0 才是綠色, 空白不要
            {
                e.CellStyle.BackColor = this.colorIsWorkingHours0;
            }
            else
            {
                e.CellStyle.BackColor = Color.White;
            }
        }

        private void Grid1_SelectionChanged(object sender, EventArgs e)
        {
            if (this.grid1.CurrentCell == null || this.grid1.CurrentCell.RowIndex == -1)
            {
                return;
            }

            this.checkBoxIsSpecialTime.Enabled = this.RightRegionEnable;
            this.displayBoxMachineID.Text = MyUtility.Convert.GetString(this.grid1.CurrentDataRow["MachineID"]);
            this.displayWorkingDate.Value = this.Grid1CurrentColumnDate;
            this.displayBoxWorkingHours.Text = this.RightRegionEnable ? MyUtility.Convert.GetString(this.grid1.CurrentCell.Value) : string.Empty;
            this.checkBoxIsHoliday.Checked = this.RightRegionEnable ? new MachineCalendar().IsHoliday(this.Grid1CurrentColumnDate.Value, this.machineIoTType) : false;
            this.checkBoxIsSpecialTime.Checked = this.RightRegionEnable ? this.IsSpecialinTemple(this.CurrentMachineIoTUkey, this.Grid1CurrentColumnDate.Value) : false;
            this.grid2bs.DataSource = this.RightRegionEnable ? new MachineCalendar().QueryCalendarDetail(this.CurrentMachineIoTUkey, this.Grid1CurrentColumnDate.Value) : null;
        }

        private void CheckBoxIsSpecialTime_CheckedChanged(object sender, EventArgs e)
        {
            this.btnSet.Enabled = this.checkBoxIsSpecialTime.Checked;
        }

        private void CheckBoxIsSpecialTime_Click(object sender, EventArgs e)
        {
            if (this.checkBoxIsSpecialTime.Checked)
            {
                // 新增特殊班表(無表身)
                DualResult result = new MachineCalendar().InsertMachineIoT_SpecialDate(this.CurrentMachineIoTUkey, this.Grid1CurrentColumnDate.Value, out _);
                if (!result)
                {
                    this.ShowErr(result);
                }

                // 加入 dtSpecial, 因為 Grid1_CellFormatting 要使用不能一個個去 DB 撈取
                DataRow newRow = this.dtSpecial.NewRow();
                newRow["Ukey"] = this.CurrentMachineIoTUkey;
                newRow["MachineID"] = this.grid1.CurrentDataRow["MachineID"];
                newRow["SpecialDate"] = this.Grid1CurrentColumnDate;
                this.dtSpecial.Rows.Add(newRow);
            }
            else
            {
                this.DeleteMachineIoT_Special();
            }

            this.CurrentCellRefresh();
        }

        private void DeleteMachineIoT_Special()
        {
            // 驗證
            DateTime date = this.Grid1CurrentColumnDate.Value;
            if (!new MachineCalendar().DeleteMachineIoT_SpecialDate_Before(this.CurrentMachineIoTUkey, date, this.machineIoTType))
            {
                this.checkBoxIsSpecialTime.Checked = true;
                return;
            }

            // 刪除
            DualResult result = new MachineCalendar().DeleteMachineIoT_SpecialDate(this.CurrentMachineIoTUkey, date);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            // 刪除 dtSpecial, 因為 Grid1_CellFormatting  要使用不能一個個去 DB 撈取
            this.dtSpecial.Select($"Ukey = {this.CurrentMachineIoTUkey} AND SpecialDate = '{this.Grid1CurrentColumnName}'").AsEnumerable().ToList().ForEach(r => r.Delete());
        }

        private bool IsSpecialinTemple(long machineIoTUkey, DateTime date)
        {
            return this.dtSpecial.Select($"Ukey = {machineIoTUkey} AND SpecialDate = '{date:yyyy/MM/dd}'").Length > 0;
        }

        private void Grid2_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            DataRow dr = this.grid2.GetDataRow(e.RowIndex);
            if (MyUtility.Convert.GetBool(dr["IsCrossDate"]))
            {
                e.CellStyle.BackColor = Color.FromArgb(255, 153, 153);
            }
            else
            {
                e.CellStyle.BackColor = Color.White;
            }
        }

        private void BtnSet_Click(object sender, EventArgs e)
        {
            if (this.grid1.CurrentCell == null)
            {
                return;
            }

            if (new B13_Set(this.CurrentMachineIoTUkey, this.Grid1CurrentColumnDate.Value).ShowDialog() == DialogResult.OK)
            {
                this.CurrentCellRefresh();
            }
        }

        private void CurrentCellRefresh()
        {
            DataTable dtDetail = new MachineCalendar().QueryCalendarDetail(this.CurrentMachineIoTUkey, this.Grid1CurrentColumnDate.Value);
            this.grid2bs.DataSource = dtDetail;
            double workinghour = new MachineCalendar().CalWorkingHour(dtDetail);
            if (workinghour == 0 && !this.IsSpecialinTemple(this.CurrentMachineIoTUkey, this.Grid1CurrentColumnDate.Value))
            {
                if (this.dtCalendar.Select($"Ukey = {this.CurrentMachineIoTUkey} AND StartDate <= '{this.Grid1CurrentColumnName}'").Length > 0
                    && !new MachineCalendar().IsHoliday(this.Grid1CurrentColumnDate.Value, this.machineIoTType))
                {
                    this.displayBoxWorkingHours.Text = workinghour.ToString();
                    this.grid1.CurrentCell.Value = workinghour;
                }
                else
                {
                    this.displayBoxWorkingHours.Text = string.Empty;
                    this.grid1.CurrentCell.Value = string.Empty;
                }
            }
            else
            {
                this.displayBoxWorkingHours.Text = workinghour.ToString();
                this.grid1.CurrentCell.Value = workinghour;
            }
        }

        private void BtnBatchAssign_Click(object sender, EventArgs e)
        {
            new B13_BatchAssignSpecialTime().ShowDialog();
            this.Query(); // 前一次查詢的條件
        }
    }
}