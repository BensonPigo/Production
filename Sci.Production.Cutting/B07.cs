using Ict;
using Sci.Data;
using System.Data;
using System.Windows.Forms;

namespace Sci.Production.Cutting
{
    public partial class B07 : Sci.Win.Tems.Input1
    {
        public B07(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.displayBox1.Text = @"**If perimeter is provided, the formula will be:
Cutting Time = Set up time + Total Cutting Perimeter/Actual Speed+ 
Window time*Marker Length/Window Length";
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            string sqlcmd = $@"select * from CuttingTime where WeaveTypeID = '{this.CurrentMaintain["ID"]}'";
            DataRow dr;
            if (MyUtility.Check.Seek(sqlcmd, out dr))
            {
                this.numSetUpT.Value = MyUtility.Convert.GetDecimal(dr["SetUpTime"]);
                this.numWindowTime.Value = MyUtility.Convert.GetDecimal(dr["WindowTime"]);
                this.numWindowLength.Value = MyUtility.Convert.GetDecimal(dr["WindowLength"]);
            }
            else
            {
                this.numSetUpT.Value = 0;
                this.numWindowTime.Value = 0;
                this.numWindowLength.Value = 0;
            }

            this.createby.Text = string.Empty;
            this.editby.Text = MyUtility.GetValue.Lookup($@"select EditName = concat(EditName,'-'+(select name from Pass1 where id = s.EditName),' '+format(EditDate,'yyyy/MM/dd HH:mm:ss') )
from CuttingTime s where WeaveTypeID = '{this.CurrentMaintain["ID"]}'");
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
merge CuttingTime
using #tmp s ON s.WeaveType = dbo.CuttingTime.WeaveTypeID
when matched then update set
	 [SetUpTime]		={this.numSetUpT.Value}
	,[WindowTime]	={this.numWindowTime.Value}
	,[WindowLength]			={this.numWindowLength.Value}
	,[EditName]				='{Sci.Env.User.UserID}'
	,[EditDate]				=getdate()
when not matched by target then
insert ([WeaveTypeID],[SetUpTime],[WindowTime],[WindowLength],[EditName],[EditDate])
values(s.WeaveType,{this.numSetUpT.Value},{this.numWindowTime.Value},{this.numWindowLength.Value},'{Sci.Env.User.UserID}',getdate())
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
