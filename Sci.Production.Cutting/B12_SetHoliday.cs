using Ict;
using Sci.Data;
using Sci.Production.Class.Command;
using System;
using System.Data;

namespace Sci.Production.Cutting
{
    /// <summary>
    /// B12_SetHoliday
    /// </summary>
    public partial class B12_SetHoliday : Win.Subs.Base
    {
        private readonly string machineIoTType;
        private readonly DateTime reviseDate;
        private int newRecord;

        /// <summary>
        /// B12_SetHoliday
        /// </summary>
        /// <inheritdoc/>
        public B12_SetHoliday(DateTime date, string machineIoTType)
        {
            this.InitializeComponent();
            this.txtDate.Text = date.ToString("yyyy/MM/dd");
            this.reviseDate = date;
            this.machineIoTType = machineIoTType;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            // PMS 多 by FactoryID 條件, MES 不 by FactoryID 條件
            string sqlcmd = $@"
select Name
from MachineIoTHoliday WITH (NOLOCK)
where HolidayDate='{this.txtDate.Text}'
AND MachineIoTType = '{this.machineIoTType}'
AND FactoryID = '{Sci.Env.User.Factory}'
";
            string holidayName = MyUtility.GetValue.Lookup(sqlcmd, "ManufacturingExecution");

            if (MyUtility.Check.Empty(holidayName))
            {
                this.Text += " - New";
                this.newRecord = 1;
            }
            else
            {
                this.txtDescription.Text = holidayName;
                this.Text += " - Revise";
                this.newRecord = 0;
            }
        }

        private void BtnAccept_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.txtDescription.Text))
            {
                if (this.newRecord == 0)
                {
                    // 驗證
                    if (!this.ValidateAllMachine())
                    {
                        return;
                    }

                    // PMS 多 by FactoryID 條件, MES 不 by FactoryID 條件
                    string deleteCmd = $@"
delete MachineIoTHoliday
where HolidayDate = '{this.reviseDate:yyyy/MM/dd}'
AND MachineIoTType = '{this.machineIoTType}'
AND FactoryID = '{Sci.Env.User.Factory}'
";
                    DualResult result = DBProxy.Current.Execute("ManufacturingExecution", deleteCmd);
                    if (!result)
                    {
                        MyUtility.Msg.WarningBox("Delete data fail!!\r\n" + result.ToString());
                        return;
                    }
                }
            }
            else
            {
                if (this.newRecord == 0)
                {
                    // PMS 多 by FactoryID 條件, MES 不 by FactoryID 條件
                    string updateCmd = $@"
update MachineIoTHoliday
set Name = '{this.txtDescription.Text}'
where HolidayDate = '{this.reviseDate:yyyy/MM/dd}'
AND MachineIoTType = '{this.machineIoTType}'
AND FactoryID = '{Sci.Env.User.Factory}'
";
                    DualResult result = DBProxy.Current.Execute("ManufacturingExecution", updateCmd);
                    if (!result)
                    {
                        MyUtility.Msg.WarningBox("update data fail!!\r\n" + result.ToString());
                        return;
                    }
                }
                else
                {
                    // PMS 多 by FactoryID 條件, MES 不 by FactoryID 條件
                    string insertCmd = $@"
insert into MachineIoTHoliday(HolidayDate, MachineIoTType, FactoryID, Name, AddName, AddDate)
values ('{this.reviseDate:yyyy/MM/dd}','{this.machineIoTType}','{Sci.Env.User.Factory}','{this.txtDescription.Text}','{Env.User.UserID}',GETDATE());
";
                    DualResult result = DBProxy.Current.Execute("ManufacturingExecution", insertCmd);
                    if (!result)
                    {
                        MyUtility.Msg.WarningBox("Insert data fail!!\r\n" + result.ToString());
                        return;
                    }
                }
            }
        }

        private bool ValidateAllMachine()
        {
            // PMS 多 by FactoryID 條件, MES 不 by FactoryID 條件
            string sqlcmd = $@"
SELECT Ukey, MachineID
FROM MachineIoT WITH (NOLOCK)
INNER JOIN [SciProduction_SpreadingNo] sn WITH (NOLOCK) ON sn.ID = MachineIoT.MachineID
WHERE MachineIoTType = '{this.machineIoTType}'
AND FactoryID = '{Sci.Env.User.Factory}'
";
            DualResult result = DBProxy.Current.Select("ManufacturingExecution", sqlcmd, out DataTable dtMachine);
            if (!result)
            {
                this.ShowErr(result);
                return false;
            }

            DateTime date = this.reviseDate;
            foreach (DataRow dr in dtMachine.Rows)
            {
                long machineIoTUkey = (long)dr["Ukey"];
                if (!new MachineCalendar().DeleteHoliday_Before(machineIoTUkey, date))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
