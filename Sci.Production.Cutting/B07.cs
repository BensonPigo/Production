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
    public partial class B07 : Sci.Win.Tems.Input1
    {
        public B07(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            displayBox1.Text = @"**If perimeter is provided, the formula will be:
Cutting Time = Set up time + Total Cutting Perimeter/Actual Speed+ 
Window time*Window No";
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            string sqlcmd = $@"select * from CuttingTime where MtlTypeID = '{this.CurrentMaintain["ID"]}'";
            DataRow dr;
            if (MyUtility.Check.Seek(sqlcmd,out dr))
            {
                numSetUpT.Value = MyUtility.Convert.GetDecimal(dr["SetUpTime"]);
                numWindowTime.Value = MyUtility.Convert.GetDecimal(dr["WindowTime"]);
                numWindowLength.Value = MyUtility.Convert.GetDecimal(dr["WindowLength"]);
            }
            else
            {
                numSetUpT.Value = 0;
                numWindowTime.Value = 0;
                numWindowLength.Value = 0;
            }
            createby.Text = string.Empty;
            editby.Text = MyUtility.GetValue.Lookup($@"select EditName = concat(EditName,'-'+(select name from Pass1 where id = s.EditName),' '+format(EditDate,'yyyy/MM/dd HH:mm:ss') )
from CuttingTime s where MtlTypeID = '{this.CurrentMaintain["ID"]}'");
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
merge CuttingTime
using #tmp s ON s.MtlType = dbo.CuttingTime.MtlTypeID
when matched then update set
	 [SetUpTime]		={numSetUpT.Value}
	,[WindowTime]	={numWindowTime.Value}
	,[WindowLength]			={numWindowLength.Value}
	,[EditName]				='{Sci.Env.User.UserID}'
	,[EditDate]				=getdate()
when not matched by target then
insert ([MtlTypeID],[SetUpTime],[WindowTime],[WindowLength],[EditName],[EditDate])
values(s.MtlType,{numSetUpT.Value},{numWindowTime.Value},{numWindowLength.Value},'{Sci.Env.User.UserID}',getdate())
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
