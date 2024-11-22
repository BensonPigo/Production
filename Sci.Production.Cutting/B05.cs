using Ict;
using Ict.Win;
using Sci.Andy.ExtensionMethods;
using Sci.Data;
using Sci.Production.Class.Command;
using Sci.Production.Class.PublicForm;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Sci.Production.Cutting
{
    /// <inheritdoc/>
    public partial class B05 : Win.Tems.Input1
    {
        private readonly string machineIoTType = "Spreading";
        private readonly Color colorCrossDay = Color.Pink;
        private long machineIoTUkey;

        /// <inheritdoc/>
        public B05(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = $" MDivisionid = '{Env.User.Keyword}'";
            this.txtCell1.MDivisionID = Env.User.Keyword;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.displayCrossday.BackColor = this.calendarGrid1.ColorCrossDay = this.colorCrossDay;
            this.GridSetup();
        }

        private void GridSetup()
        {
            this.Helper.Controls.Grid.Generator(this.grid1)
                .Text("StartDate", header: "StartDate", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("CreateBy", header: "Create By", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("EditBy", header: "EditBy", width: Widths.AnsiChars(20), iseditingreadonly: true)
                ;

            // 關閉排序功能
            this.grid1.Columns.DisableSortable();
        }

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtID.ReadOnly = true;
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["MDivisionid"] = Env.User.Keyword;
            this.CurrentMaintain["FactoryID"] = Env.User.Factory;
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            if (this.CurrentMaintain["ID"].IsEmpty())
            {
                MyUtility.Msg.WarningBox("<Spreading No> cannot be empty.");
                return false;
            }

            if (this.CurrentMaintain["FactoryID"].IsEmpty())
            {
                MyUtility.Msg.WarningBox("Factory cannot be empty.");
                return false;
            }

            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override Ict.DualResult ClickSavePost()
        {
            string sqlcmd = $@"select 1 from MachineIoT where MachineIoTType = '{this.machineIoTType}' and MachineID = '{this.CurrentMaintain["id"]}'";
            if (MyUtility.Check.Seek(sqlcmd, "ManufacturingExecution"))
            {
                string sqlcmdUpdate = $@"
UPDATE MachineIoT
SET EditName = '{Sci.Env.User.UserID}'
   ,EditDate = GETDATE()
WHERE MachineIoTType = '{this.machineIoTType}' AND MachineID = '{this.CurrentMaintain["id"]}'";
                DualResult result = DBProxy.Current.Execute("ManufacturingExecution", sqlcmdUpdate);
                if (!result)
                {
                    return result;
                }
            }
            else
            {
                string sqlcmdInsert = $@"
INSERT INTO MachineIoT (MachineIoTType, MachineID, AddName, AddDate)
VALUES ('{this.machineIoTType}', '{this.CurrentMaintain["id"]}', '{Sci.Env.User.UserID}', GETDATE())
";
                DualResult result = DBProxy.Current.Execute("ManufacturingExecution", sqlcmdInsert);
                if (!result)
                {
                    return result;
                }
            }

            return base.ClickSavePost();
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.calendarGrid1.SetDataSource(0);
            this.Query();
        }

        private void Query()
        {
            this.machineIoTUkey = MyUtility.Convert.GetLong(MyUtility.GetValue.Lookup($@"select Ukey from MachineIoT where MachineIoTType = '{this.machineIoTType}' and MachineID = '{this.CurrentMaintain["id"]}'", "ManufacturingExecution"));

            string sqlcmd = $@"
SELECT *
    ,CreateBy = CONCAT((SELECT Name FROM Pass1 WHERE ID = MachineIoT_Calendar.AddName), ' ', FORMAT(MachineIoT_Calendar.AddDate, 'yyyy/MM/dd HH:mm:ss'))
    ,EditBy = CONCAT((SELECT Name FROM Pass1 WHERE ID = MachineIoT_Calendar.EditName), ' ', FORMAT(MachineIoT_Calendar.EditDate, 'yyyy/MM/dd HH:mm:ss'))
FROM MachineIoT_Calendar WITH(NOLOCK)
WHERE EXISTS (SELECT 1 FROM MachineIoT WHERE Ukey = MachineIoT_Calendar.MachineIoTUkey AND MachineID = '{this.CurrentMaintain["ID"]}' AND MachineIoTType = '{this.machineIoTType}')
ORDER BY StartDate DESC
";
            DualResult result = DBProxy.Current.Select("ManufacturingExecution", sqlcmd, out DataTable dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.listControlBindingSource1.DataSource = dt;
        }

        private void Grid1_SelectionChanged(object sender, System.EventArgs e)
        {
            if (this.grid1.CurrentDataRow == null)
            {
                return;
            }

            this.dateStart.Value = MyUtility.Convert.GetDate(this.grid1.CurrentDataRow["StartDate"]);
            this.calendarGrid1.SetDataSource(MyUtility.Convert.GetLong(this.grid1.CurrentDataRow["Ukey"]));
        }

        private void BtnCreateNewCalendar_Click(object sender, System.EventArgs e)
        {
            new MachineEditCalendar(this.machineIoTType, this.CurrentMaintain["ID"].ToString(), null, this.machineIoTUkey, 0, this.colorCrossDay).ShowDialog();
            this.OnDetailEntered();
        }

        private void BtnEditCalendar_Click(object sender, System.EventArgs e)
        {
            if (this.grid1.CurrentDataRow == null)
            {
                return;
            }

            var startDate = MyUtility.Convert.GetDate(this.grid1.CurrentDataRow["StartDate"]);
            var machineIoT_CalendarUkey = MyUtility.Convert.GetLong(this.grid1.CurrentDataRow["Ukey"]);
            new MachineEditCalendar(this.machineIoTType, this.CurrentMaintain["ID"].ToString(), startDate, this.machineIoTUkey, machineIoT_CalendarUkey, this.colorCrossDay).ShowDialog();
            this.OnDetailEntered();
        }

        private void BtnRemoveCalendar_Click(object sender, System.EventArgs e)
        {
            if (this.grid1.CurrentDataRow == null)
            {
                return;
            }

            if (MyUtility.Msg.QuestionBox("Do you want to remote the data ?") == DialogResult.No)
            {
                return;
            }

            long machineIoT_CalendarUkey = MyUtility.Convert.GetLong(this.grid1.CurrentDataRow["Ukey"]);
            DateTime startDate = (DateTime)this.grid1.CurrentDataRow["StartDate"];
            new MachineCalendar().DeleteMachineIoT_Calendar(this.machineIoTUkey, machineIoT_CalendarUkey, startDate);
            this.OnDetailEntered();
        }
    }
}
