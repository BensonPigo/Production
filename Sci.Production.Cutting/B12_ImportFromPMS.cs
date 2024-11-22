using Ict;
using Sci.Data;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Sci.Production.Cutting
{
    /// <inheritdoc/>
    public partial class B12_ImportFromPMS : Win.Tems.QueryForm
    {
        private readonly string machineIoTType;

        /// <inheritdoc/>
        public B12_ImportFromPMS(string machineIoTType)
        {
            this.InitializeComponent();
            this.machineIoTType = machineIoTType;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.comboFactory1.SetDataSource();
            this.comboFactory1.Text = Sci.Env.User.Factory;
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            // 檢查輸入
            if (!this.dateRange1.HasValue1 || !this.dateRange1.HasValue2)
            {
                MyUtility.Msg.WarningBox("Date cannot be empty.");
                return;
            }

            // 從 PMS 查詢資料
            string sqlcmdPms = $@"
SELECT HolidayDate, Name
FROM Holiday WITH(NOLOCK)
WHERE HolidayDate BETWEEN '{this.dateRange1.Value1:yyyy/MM/dd}' AND '{this.dateRange1.Value2:yyyy/MM/dd}'
AND FactoryID = '{this.comboFactory1.SelectedValue}'
";
            DualResult resultPms = DBProxy.Current.Select("Production", sqlcmdPms, out DataTable dtPms);
            if (!resultPms)
            {
                this.ShowErr(resultPms);
                return;
            }

            // 只做更新與新增假日, 因為刪除假日必須驗證那天所有 Machine 的班表
            // PMS 多 by FactoryID 條件, MES 不 by FactoryID 條件
            string sqlcmd = $@"
UPDATE m
SET m.Name = t.Name
    ,m.EditName = '{Sci.Env.User.UserID}'
    ,m.EditDate = GETDATE()
FROM MachineIoTHoliday m
INNER JOIN #tmp t ON t.HolidayDate = m.HolidayDate
WHERE m.MachineIoTType = '{this.machineIoTType}'
AND m.FactoryID = '{Sci.Env.User.Factory}'

INSERT INTO MachineIoTHoliday(HolidayDate, MachineIoTType, FactoryID, Name, AddName, AddDate)
SELECT t.HolidayDate,'{this.machineIoTType}','{Sci.Env.User.Factory}', t.Name, '{Sci.Env.User.UserID}', GETDATE()
FROM #tmp t
LEFT JOIN MachineIoTHoliday m ON t.HolidayDate = m.HolidayDate
                             AND m.MachineIoTType = '{this.machineIoTType}'
                             AND m.FactoryID = '{Sci.Env.User.Factory}'
WHERE m.HolidayDate IS NULL
";
            DualResult result = DBProxy.Current.OpenConnection("ManufacturingExecution", out SqlConnection conn);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            result = MyUtility.Tool.ProcessWithDatatable(dtPms, string.Empty, sqlcmd, out DataTable _, conn: conn);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.Close();
        }
    }
}
