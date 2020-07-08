using Ict;
using Sci.Data;
using System.Data;
using System.Windows.Forms;

namespace Sci.Production.Cutting
{
    public partial class B06 : Win.Tems.Input1
    {
        public B06(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.displayBox1.Text = @"*Spreading Time = Preparation Time *Marker Length + Changeover time *
No. of Roll + Set up time + (Machine Spreading Time*Marker Length*Layer) + 
Separator Time*(Dye Lot-1)  + Forward Time";
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            string sqlcmd = $@"select * from SpreadingTime where WeaveTypeID = '{this.CurrentMaintain["ID"]}'";
            DataRow dr;
            if (MyUtility.Check.Seek(sqlcmd, out dr))
            {
                this.numPreparationTime.Value = MyUtility.Convert.GetDecimal(dr["PreparationTime"]);
                this.numChangeOverRollTime.Value = MyUtility.Convert.GetDecimal(dr["ChangeOverRollTime"]);
                this.numChangeOverUnRollTime.Value = MyUtility.Convert.GetDecimal(dr["ChangeOverUnRollTime"]);
                this.numSetupTime.Value = MyUtility.Convert.GetDecimal(dr["SetupTime"]);
                this.numSeparatorTime.Value = MyUtility.Convert.GetDecimal(dr["SeparatorTime"]);
                this.numSpreadingTime.Value = MyUtility.Convert.GetDecimal(dr["SpreadingTime"]);
                this.numForwardTime.Value = MyUtility.Convert.GetDecimal(dr["ForwardTime"]);
            }
            else
            {
                this.numPreparationTime.Value = 0;
                this.numChangeOverRollTime.Value = 0;
                this.numChangeOverUnRollTime.Value = 0;
                this.numSetupTime.Value = 0;
                this.numSeparatorTime.Value = 0;
                this.numSpreadingTime.Value = 0;
                this.numForwardTime.Value = 0;
            }

            this.createby.Text = string.Empty;
            this.editby.Text = MyUtility.GetValue.Lookup($@"select EditName = concat(EditName,'-'+(select name from Pass1 where id = s.EditName),' '+format(EditDate,'yyyy/MM/dd HH:mm:ss') )
from SpreadingTime s where WeaveTypeID = '{this.CurrentMaintain["ID"]}'");
        }

        protected override DualResult ClickSave()
        {
            // 修改表身資料,不寫入表頭EditName and EditDate
            ITableSchema dtSchema;
            var ok = DBProxy.Current.GetTableSchema("Production", "style", out dtSchema);
            dtSchema.IsSupportEditDate = false;
            dtSchema.IsSupportEditName = false;

            string insertupdate = $@"
select WeaveType = '{this.CurrentMaintain["ID"]}' into #tmp
merge SpreadingTime
using #tmp s ON s.WeaveType = dbo.SpreadingTime.WeaveTypeID
when matched then update set
	 [PreparationTime]		={this.numPreparationTime.Value}
	,[ChangeOverRollTime]	={this.numChangeOverRollTime.Value}
	,[ChangeOverUnRollTime]	={this.numChangeOverUnRollTime.Value}
	,[SetupTime]			={this.numSetupTime.Value}
	,[SeparatorTime]		={this.numSeparatorTime.Value}
	,[SpreadingTime]		={this.numSpreadingTime.Value}
	,[ForwardTime]			={this.numForwardTime.Value}
	,[EditName]				='{Sci.Env.User.UserID}'
	,[EditDate]				=getdate()
when not matched by target then
insert ([WeaveTypeID],[PreparationTime],[ChangeOverRollTime],[ChangeOverUnRollTime],[SetupTime],[SeparatorTime],[SpreadingTime],[ForwardTime],[EditName],[EditDate])
values(s.WeaveType,{this.numPreparationTime.Value},{this.numChangeOverRollTime.Value},{this.numChangeOverUnRollTime.Value},{this.numSetupTime.Value},
{this.numSeparatorTime.Value},{this.numSpreadingTime.Value},{this.numForwardTime.Value},'{Sci.Env.User.UserID}',getdate())
;
drop table #tmp
";
            DualResult result = DBProxy.Current.Execute(null, insertupdate);
            if (!result)
            {
                return Result.F(result.ToString());
            }

            return Result.True;
        }
    }
}
