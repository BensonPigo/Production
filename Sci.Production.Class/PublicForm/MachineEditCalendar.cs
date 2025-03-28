using Ict;
using Sci.Data;
using Sci.Production.Class.Command;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Transactions;

namespace Sci.Production.Class.PublicForm
{
    /// <inheritdoc/>
    public partial class MachineEditCalendar : Sci.Win.Tems.QueryForm
    {
        private readonly long machineIoTUkey;
        private readonly string MachineType;
        private readonly MachineCalendar machineCalendar = new MachineCalendar();
        private long machineIoT_CalendarUkey;
        private DataTable dtMachineIoT_Calendar_Detail;

        /// <inheritdoc/>
        public MachineEditCalendar(string machineType, string machineID, DateTime? startTime, long machineIoTUkey, long machineIoT_CalendarUkey, Color colorCrossDay, bool editMode = true)
        {
            this.InitializeComponent();
            this.MachineType = machineType;
            this.displayBoxMachineID.Text = machineID;
            this.dateStart.Value = startTime;
            this.machineIoTUkey = machineIoTUkey;
            this.machineIoT_CalendarUkey = machineIoT_CalendarUkey;
            this.displayCrossday.BackColor = this.calendarGrid1.ColorCrossDay = colorCrossDay;
            this.EditMode = editMode;
            this.dtMachineIoT_Calendar_Detail = this.machineCalendar.QueryMachineIoT_Calendar_Detail(machineIoT_CalendarUkey);
            this.calendarGrid1.SetDataSource(this.dtMachineIoT_Calendar_Detail, true);
            this.calendarGrid1.btnday1.Click += this.Btnday_Click;
            this.calendarGrid1.btnday2.Click += this.Btnday_Click;
            this.calendarGrid1.btnday3.Click += this.Btnday_Click;
            this.calendarGrid1.btnday4.Click += this.Btnday_Click;
            this.calendarGrid1.btnday5.Click += this.Btnday_Click;
            this.calendarGrid1.btnday6.Click += this.Btnday_Click;
            this.calendarGrid1.btnday7.Click += this.Btnday_Click;
        }

        private void Btnday_Click(object sender, EventArgs e)
        {
            Win.UI.Button btn = (Win.UI.Button)sender;
            int weekDay = MyUtility.Convert.GetInt(btn.Name.Replace("btnday", string.Empty));
            new EditCalendar(this.dtMachineIoT_Calendar_Detail, weekDay).ShowDialog();
            this.calendarGrid1.SetDataSource(this.dtMachineIoT_Calendar_Detail, true);

        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.SetComboBoxMachine();
            this.dateStart.ReadOnly = this.machineIoT_CalendarUkey > 0; // 新建才可編輯
        }

        private void SetComboBoxMachine()
        {
            string sqlmcd = $@"
SELECT
    MachineID
FROM MachineIoT WITH (NOLOCK)
WHERE MachineIoTType = '{this.MachineType}'
";
            DualResult result = DBProxy.Current.Select("ManufacturingExecution", sqlmcd, out DataTable dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            MyUtility.Tool.SetupCombox(this.comboBoxImportMachineID, 1, dt);
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.comboBoxImportMachineID.Text) || !this.dateImportStartDate.Value.HasValue)
            {
                MyUtility.Msg.WarningBox("Please input Machine and StartDate!");
                return;
            }

            string sqlcmd = $@"
SELECT MachineIoT_Calendar.Ukey
FROM MachineIoT WITH (NOLOCK)
INNER JOIN MachineIoT_Calendar WITH (NOLOCK) ON MachineIoT.Ukey = MachineIoT_Calendar.MachineIoTUkey
WHERE MachineIoT.MachineIoTType = '{this.MachineType}'
AND MachineIoT.MachineID = '{this.comboBoxImportMachineID.Text}'
AND MachineIoT_Calendar.StartDate = '{this.dateImportStartDate.Text}'";
            if (!MyUtility.Check.Seek(sqlcmd, out DataRow dr, "ManufacturingExecution"))
            {
                MyUtility.Msg.WarningBox("Data not found");
                return;
            }

            this.dtMachineIoT_Calendar_Detail = this.machineCalendar.QueryMachineIoT_Calendar_Detail(MyUtility.Convert.GetLong(dr["Ukey"]));
            this.calendarGrid1.SetDataSource(this.dtMachineIoT_Calendar_Detail, true);
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!this.SaveBefore())
            {
                return;
            }

            Exception errMsg = null;
            using (TransactionScope transactionscope = new TransactionScope())
            {
                try
                {
                    DualResult result = DBProxy.Current.Select("ManufacturingExecution", this.GetSaveSQLMachineIoT_Calendar(), out DataTable dt);
                    if (!result)
                    {
                        throw result.GetException();
                    }

                    if (dt != null && dt.Rows.Count > 0) // 若是新增會有 MachineIoT_Calendar.Ukey
                    {
                        this.machineIoT_CalendarUkey = MyUtility.Convert.GetLong(dt.Rows[0][0]);
                    }

                    if (!(result = DBProxy.Current.OpenConnection("ManufacturingExecution", out SqlConnection conn)))
                    {
                        throw result.GetException();
                    }

                    result = MyUtility.Tool.ProcessWithDatatable(this.dtMachineIoT_Calendar_Detail, string.Empty, this.GetSaveSQLMachineIoT_Calendar_Detail(), out DataTable _, conn: conn);
                    if (!result)
                    {
                        throw result.GetException();
                    }

                    transactionscope.Complete();
                }
                catch (Exception ex)
                {
                    errMsg = ex;
                }
            }

            if (errMsg != null)
            {
                MyUtility.Msg.ErrorBox(errMsg.Message);
                return;
            }

            MyUtility.Msg.InfoBox("Save successfully");
            this.Close();
        }

        private bool SaveBefore()
        {
#pragma warning disable SA1503
            DualResult result;
            try
            {
                // 畫面上檢查
                if (!this.dateStart.Value.HasValue)
                {
                    this.dateStart.Focus();
                    MyUtility.Msg.WarningBox("Please input Start Date");
                    return false;
                }

                // 確認【一週班表】不同天的時間是否有重疊
                if (!this.machineCalendar.ValidateWeekdayOverlap(this.dtMachineIoT_Calendar_Detail)) return false;

                // 以下需要進 DB 檢查
                // 新增時 預防相同 machineIoT 之下的 MachineIoT_Calendar 不能出現兩筆相同 StartDate
                if (!this.machineCalendar.CheckDuplicateStartDate(this.machineIoTUkey, this.dateStart.Value, this.machineIoT_CalendarUkey, this.displayBoxMachineID.Text)) return false;

                // 檢查班表
                if (!(result = this.machineCalendar.ValidateOverlap(this.machineIoTUkey, this.dateStart.Value, this.dtMachineIoT_Calendar_Detail))) throw result.GetException();
            }
            catch (Exception ex)
            {
                if (ex.Message != string.Empty) this.ShowErr(ex);
                return false;
            }

            return true;
#pragma warning restore SA1503
        }

        private string GetSaveSQLMachineIoT_Calendar()
        {
            if (this.machineIoT_CalendarUkey == 0) // 新增
            {
                return $@"
INSERT INTO MachineIoT_Calendar (MachineIoTUkey, StartDate, AddName, AddDate)
OUTPUT INSERTED.Ukey
VALUES ('{this.machineIoTUkey}', '{this.dateStart.Text}', '{Sci.Env.User.UserID}', GETDATE())
";
            }
            else // 更新
            {
                return $@"
UPDATE MachineIoT_Calendar
SET EditName = '{Sci.Env.User.UserID}'
   ,EditDate = GETDATE()
WHERE Ukey = {this.machineIoT_CalendarUkey}
";
            }
        }

        private string GetSaveSQLMachineIoT_Calendar_Detail()
        {
            return $@"
DELETE MachineIoT_Calendar_Detail WHERE MachineIoT_CalendareUkey = {this.machineIoT_CalendarUkey}

INSERT INTO MachineIoT_Calendar_Detail(MachineIoT_CalendareUkey, WeekDay, StartTime, EndTime, IsCrossDate,  AddName, AddDate)
SELECT {this.machineIoT_CalendarUkey}, WeekDay, StartTime, EndTime, IsCrossDate, '{Sci.Env.User.UserID}', GETDATE()
FROM #tmp
";
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
