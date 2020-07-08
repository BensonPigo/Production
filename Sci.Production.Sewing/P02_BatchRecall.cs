using System;
using System.Data;
using Ict.Win;
using Ict;
using Sci.Data;
using System.Transactions;

namespace Sci.Production.Sewing
{
    /// <summary>
    /// P01_BatchRecall
    /// </summary>
    public partial class P02_BatchRecall : Win.Tems.QueryForm
    {
        /// <inheritdoc/>
        public P02_BatchRecall()
        {
            this.InitializeComponent();
        }

        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk = new Ict.Win.UI.DataGridViewCheckBoxColumn();

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.grid1.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.grid1)
            .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: true, falseValue: false).Get(out this.col_chk)
            .Date("OutputDate", header: "Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("FactoryID", header: "Factory", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Text("SewingLineID", header: "Line#", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Text("Team", header: "Team", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Text("Shift", header: "Shift", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Numeric("QAQty", header: "QAQty", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Text("request", header: "request", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("Reason", header: "Reason ", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("remark", header: "Remark", width: Widths.AnsiChars(8), iseditingreadonly: true)
            ;
            this.Query();
        }

        private void Query()
        {
            string dateOutput1 = string.Empty;
            string dateOutput2 = string.Empty;
            string line = string.Empty;
            string where = string.Empty;
            if (!MyUtility.Check.Empty(this.dateOutput.Value1))
            {
                dateOutput1 = ((DateTime)this.dateOutput.Value1).ToString("yyyy/MM/dd");
                where += $"and so.OutputDate >= '{dateOutput1}' ";
            }

            if (!MyUtility.Check.Empty(this.dateOutput.Value2))
            {
                dateOutput2 = ((DateTime)this.dateOutput.Value2).ToString("yyyy/MM/dd");
                where += $"and so.OutputDate <= '{dateOutput2}' ";
            }

            if (!MyUtility.Check.Empty(this.txtsewinglineLine.Text))
            {
                line = this.txtsewinglineLine.Text;
                where += $"and so.SewingLineID = '{line}' ";
            }

            DataTable dt;
            string sqlcmd = $@"
SELECT selected = cast(0 as bit)
    , so.OutputDate
    , so.FactoryID
    , so.SewingLineID
    , so.Team
    , so.Shift
    , so.QAQty
    , request= concat((select p1.name from pass1 p1 where p1.id=sod.requestname) ,'-',sod.requestdate)
    , reason=sod.reasonid + '-' + (select name from Reason where ReasonTypeID='Sewing_RVS' and id=sod.reasonid)
    , sod.remark
    , so.id
    , sod.ukey
FROM SewingOutput so
INNER JOIN SewingOutput_DailyUnlock sod on so.id = sod.SewingOutputID
OUTER APPLY (
	SELECT max(a.Ukey) Ukey, a.SewingOutputID 
	FROM SewingOutput_DailyUnlock a 
	WHERE a.UnLockDate is null 
	AND so.id = a.SewingOutputID
	GROUP BY a.SewingOutputID
)sodd
WHERE 1=1
    AND so.Status = 'Sent' 
    AND so.FactoryID = '{Sci.Env.User.Factory}'
    AND sod.Ukey = sodd.Ukey
    {where}
ORDER BY sod.ukey desc,so.LockDate,so.SewingLineID,so.Team,so.Shift
";
            DualResult result = DBProxy.Current.Select("Production", sqlcmd, out dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.listControlBindingSource1.DataSource = dt;
        }

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            this.listControlBindingSource1.DataSource = null;
            this.Query();
            if (this.listControlBindingSource1.DataSource != null && ((DataTable)this.listControlBindingSource1.DataSource).Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found!");
            }
        }

        private void Btnsave_Click(object sender, EventArgs e)
        {
            this.grid1.ValidateControl();
            var query = ((DataTable)this.listControlBindingSource1.DataSource).Select("selected = 1");
            if (query.Length <= 0)
            {
                MyUtility.Msg.InfoBox("Please select item first!");
                return;
            }

            DataTable dt = query.CopyToDataTable();
            string sqlcmd = $@"
insert into SewingOutput_History (ID,HisType,OldValue,NewValue,ReasonID,Remark,AddName,AddDate)
select t.id ,'Status' ,'Sent' ,'New' ,sod.ReasonID ,sod.Remark ,'{Sci.Env.User.UserID}' ,GETDATE()
from #tmp t
inner join SewingOutput_DailyUnlock  sod 
on t.id = sod.SewingOutputID AND sod.UnLockDate is null

UPDATE sod SET
UnLockDate = getdate() ,UnLockName= '{Sci.Env.User.UserID}'
FROM SewingOutput_DailyUnlock sod
WHERE sod.SewingOutputID IN (select id from #tmp) AND sod.UnLockDate IS NULL

UPDATE s SET Status='New', LockDate = null 
, s.editname='{Sci.Env.User.UserID}' 
, s.editdate=getdate()
FROM SewingOutput s WHERE ID IN (SELECT ID FROM #tmp)
";
            DataTable dt2;
            using (TransactionScope scope = new TransactionScope())
            {
                DualResult upResult;
                if (!MyUtility.Check.Empty(sqlcmd))
                {
                    if (!(upResult = MyUtility.Tool.ProcessWithDatatable(dt, string.Empty, sqlcmd, out dt2)))
                    {
                        this.ShowErr(upResult);
                        return;
                    }
                }

                scope.Complete();
            }

            MyUtility.Msg.InfoBox("Successfully");

            this.listControlBindingSource1.DataSource = null;
            this.Query();
            if (this.listControlBindingSource1.DataSource != null && ((DataTable)this.listControlBindingSource1.DataSource).Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found!");
            }
        }

        private void Btnclose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
