using Ict;
using Ict.Win;
using Sci.Production.Class;
using Sci.Production.Class.Command;
using System;
using System.Data;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Cutting
{
    /// <inheritdoc/>
    public partial class B13_Set : Win.Tems.QueryForm
    {
        private long machineIoTUkey;
        private DateTime date;
        private DataTable dt;

        /// <inheritdoc/>
        public B13_Set(long machineIoTUkey, DateTime date)
        {
            this.InitializeComponent();
            this.date = date;
            this.machineIoTUkey = machineIoTUkey;
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
                .CheckBox("IsCrossDate", header: "Cross-day", width: Widths.AnsiChars(5), trueValue: true, falseValue: false)
                ;

            // 關閉排序功能
            this.grid1.Columns.DisableSortable();
        }

        private void SetDataTable()
        {
            this.dt = new MachineCalendar().GetMachineIoT_Calendar_DetailSchema();
            this.listControlBindingSource1.DataSource = this.dt;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
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
                    long machineIoT_SpecialUkey = new MachineCalendar().GetMachineIoT_SpecialUkey(this.machineIoTUkey, this.date);
                    DualResult result = new MachineCalendar().UpdateMachineIoT_SpecialDate(machineIoT_SpecialUkey);
                    if (!result)
                    {
                        throw result.GetException();
                    }

                    if (!(result = new MachineCalendar().SaveMachineIoT_SpecialDate_Detail(dtFormatTime, machineIoT_SpecialUkey)))
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
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private bool SaveBefore(out DataTable dtFormatTime)
        {
            if (!new MachineCalendar().ValidateDataTableTime(this.dt, out dtFormatTime))
            {
                return false;
            }

            // 檢查 1 檢查週期班表前一天是否有時間重疊 2 檢查週期班表後一天是否有時間重疊
            if (!new MachineCalendar().ValidSpecialDate(dtFormatTime, this.date, this.machineIoTUkey))
            {
                return false;
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
    }
}
