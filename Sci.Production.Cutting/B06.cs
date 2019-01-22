using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Cutting
{
    public partial class B06 : Sci.Win.Tems.Input1
    {
        public B06(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            displayBox1.Text = @"*Spreading Time = Preparation Time *Marker Length + Changeover time *
No. of Roll + Set up time + (Machine Spreading Time*Marker Length*Layer) + 
(No. of Separator * Dye lot -1) + Forward Time";
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            string sqlcmd = $@"select * from SpreadingTime where MtlTypeID = '{this.CurrentMaintain["ID"]}'";
            DataRow dr;
            if (MyUtility.Check.Seek(sqlcmd,out dr))
            {
                numPreparationTime.Value = MyUtility.Convert.GetDecimal(dr["PreparationTime"]);
                numChangeOverRollTime.Value = MyUtility.Convert.GetDecimal(dr["ChangeOverRollTime"]);
                numChangeOverUnRollTime.Value = MyUtility.Convert.GetDecimal(dr["ChangeOverUnRollTime"]);
                numSetupTime.Value = MyUtility.Convert.GetDecimal(dr["SetupTime"]);
                numSeparatorTime.Value = MyUtility.Convert.GetDecimal(dr["SeparatorTime"]);
                numSpreadingTime.Value = MyUtility.Convert.GetDecimal(dr["SpreadingTime"]);
                numForwardTime.Value = MyUtility.Convert.GetDecimal(dr["ForwardTime"]);
            }
            else
            {
                numPreparationTime.Value = 0;
                numChangeOverRollTime.Value = 0;
                numChangeOverUnRollTime.Value = 0;
                numSetupTime.Value = 0;
                numSeparatorTime.Value = 0;
                numSpreadingTime.Value = 0;
                numForwardTime.Value = 0;
            }
            createby.Text = MyUtility.GetValue.Lookup($@"select AddName = concat(AddName,'-'+(select name from Pass1 where id = s.AddName),' '+format(AddDate,'yyyy/MM/dd HH:mm:ss') )
from SpreadingTime s where MtlTypeID = '{this.CurrentMaintain["ID"]}'");
            editby.Text = MyUtility.GetValue.Lookup($@"select EditName = concat(EditName,'-'+(select name from Pass1 where id = s.EditName),' '+format(EditDate,'yyyy/MM/dd HH:mm:ss') )
from SpreadingTime s where MtlTypeID = '{this.CurrentMaintain["ID"]}'");
        }

        protected override DualResult ClickSave()
        {
            // 修改表身資料,不寫入表頭EditName and EditDate
            ITableSchema dtSchema;
            var ok = DBProxy.Current.GetTableSchema("Production", "style", out dtSchema);
            dtSchema.IsSupportEditDate = false;
            dtSchema.IsSupportEditName = false;

            string insertupdate = $@"
select MtlType = '{this.CurrentMaintain["ID"]}' into #tmp
merge SpreadingTime
using #tmp s ON s.MtlType = dbo.SpreadingTime.MtlTypeID
when matched then update set
	 [PreparationTime]		={numPreparationTime.Value}
	,[ChangeOverRollTime]	={numChangeOverRollTime.Value}
	,[ChangeOverUnRollTime]	={numChangeOverUnRollTime.Value}
	,[SetupTime]			={numSetupTime.Value}
	,[SeparatorTime]		={numSeparatorTime.Value}
	,[SpreadingTime]		={numSpreadingTime.Value}
	,[ForwardTime]			={numForwardTime.Value}
	,[EditName]				='{Sci.Env.User.UserID}'
	,[EditDate]				=getdate()
when not matched by target then
insert ([MtlTypeID],[PreparationTime],[ChangeOverRollTime],[ChangeOverUnRollTime],[SetupTime],[SeparatorTime],[SpreadingTime],[ForwardTime],[AddName],[AddDate])
values(s.MtlType,{numPreparationTime.Value},{numChangeOverRollTime.Value},{numChangeOverUnRollTime.Value},{numSetupTime.Value},
{numSeparatorTime.Value},{numSpreadingTime.Value},{numForwardTime.Value},'{Sci.Env.User.UserID}',getdate())
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
