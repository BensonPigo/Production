using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Class;
using Sci.Production.Class.Command;
using System;
using System.Data;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Cutting
{
    /// <inheritdoc/>
    public partial class B13_BatchAssignSpecialTime : Win.Tems.QueryForm
    {
        private DataTable dt;

        /// <inheritdoc/>
        public B13_BatchAssignSpecialTime()
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.GridSetup();
            this.SetDataTable();
            this.SetGridMachineData();
        }

        private void GridSetup()
        {
            this.gridMachine.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.gridMachine)
                .CheckBox("Selected", header: "Sel", trueValue: true, falseValue: false)
                .Text("MachineID", "Machine ID", iseditingreadonly: true)
                ;

            // 關閉排序功能
            this.gridMachine.Columns.DisableSortable();

            this.Helper.Controls.Grid.Generator(this.grid1)
                .TimeSpanHHmm("StartTime", header: "Start")
                .TimeSpanHHmm("EndTime", header: "End")
                .CheckBox("IsCrossDate", header: "Cross-day", width: Widths.AnsiChars(5), trueValue: true, falseValue: false)
                ;

            // 關閉排序功能
            this.grid1.Columns.DisableSortable();
        }

        private void SetDataTable()
        {
            this.dt = new MachineCalendar().GetMachineIoT_Calendar_DetailSchema();
            this.grid1bs.DataSource = this.dt;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            this.gridMachine.ValidateControl();
            this.grid1.ValidateControl();
            if (!this.SaveBefore(out DataTable dtFormatTime))
            {
                return;
            }

            Exception errMsg = null;
            using (TransactionScope transactionscope = new TransactionScope())
            {
                try
                {
                    // 檢查所有勾選的 Machine. 逐一寫入
                    foreach (DataRow row in ((DataTable)this.gridMachinebs.DataSource).Select($"Selected = 1"))
                    {
                        long machineIoTUkey = MyUtility.Convert.GetLong(row["Ukey"]);
                        if (this.checkBoxIsSpecialTime.Checked)
                        {
                            DualResult result = new MachineCalendar().SaveMachineIoT_SpecialDate(machineIoTUkey, this.dateWorking.Value.Value, out long machineIoT_SpecialUkey);
                            if (!result)
                            {
                                throw result.GetException();
                            }

                            if (!(result = new MachineCalendar().SaveMachineIoT_SpecialDate_Detail(dtFormatTime, machineIoT_SpecialUkey)))
                            {
                                throw result.GetException();
                            }
                        }
                        else
                        {
                            // 驗證
                            if (!new MachineCalendar().DeleteMachineIoT_SpecialDate_Before(machineIoTUkey, this.dateWorking.Value.Value, this.comboDropDownList1.SelectedValue.ToString()))
                            {
                                return;
                            }

                            DualResult result = new MachineCalendar().DeleteMachineIoT_SpecialDate(machineIoTUkey, this.dateWorking.Value.Value);
                            if (!result)
                            {
                                throw result.GetException();
                            }
                        }
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
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private bool SaveBefore(out DataTable dtFormatTime)
        {
            dtFormatTime = null;
            if (!this.dateWorking.Value.HasValue)
            {
                MyUtility.Msg.WarningBox("Workng Date can not empty!");
                return false;
            }

            if (!new MachineCalendar().ValidateDataTableTime(this.dt, out dtFormatTime))
            {
                return false;
            }

            // 檢查所有勾選的 Machine. 1 檢查週期班表前一天是否有時間重疊 2 檢查週期班表後一天是否有時間重疊
            DateTime date = this.dateWorking.Value.Value;
            foreach (DataRow row in ((DataTable)this.gridMachinebs.DataSource).Select($"Selected = 1"))
            {
                long machineIoTUkey = MyUtility.Convert.GetLong(row["Ukey"]);
                if (!new MachineCalendar().ValidSpecialDate(dtFormatTime, date, machineIoTUkey))
                {
                    return false;
                }
            }

            return true;
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            this.dt.Clear();
            this.dt.AcceptChanges();
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

        private void ComboDropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.SetGridMachineData();
        }

        private void SetGridMachineData()
        {
            string sqlmcd = $@"
SELECT Selected = CAST(0 AS BIT), MachineID, Ukey
FROM MachineIoT WITH (NOLOCK)
INNER JOIN [SciProduction_SpreadingNo] sn WITH (NOLOCK) ON sn.ID = MachineIoT.MachineID
WHERE MachineIoTType = '{this.comboDropDownList1.SelectedValue}'
AND sn.MDivisionID = '{Sci.Env.User.Keyword}'
ORDER BY MachineID
";
            DualResult result = DBProxy.Current.Select("ManufacturingExecution", sqlmcd, out DataTable dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.gridMachinebs.DataSource = dt;
        }

        private void CheckBoxIsSpecialTime_CheckedChanged(object sender, EventArgs e)
        {
            this.grid1.IsEditingReadOnly = !this.checkBoxIsSpecialTime.Checked;
            this.btnAdd.Enabled = this.checkBoxIsSpecialTime.Checked;
            this.btnDelete.Enabled = this.checkBoxIsSpecialTime.Checked;

            if (!this.checkBoxIsSpecialTime.Checked)
            {
                this.dt.Clear();
                this.dt.AcceptChanges();
            }
        }
    }
}
