using Ict;
using Ict.Win;
using Ict.Win.Tools;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.IE
{
    /// <summary>
    /// P05_NotHitTargetReason
    /// </summary>
    public partial class P05_NotHitTargetReason : Sci.Win.Tems.QueryForm
    {
        private string id;
        private string factoryID;
        private string status;
        private DataTable dtAutomatedLineMapping_NotHitTargetReason = new DataTable();

        /// <summary>
        /// HasNotHitTargetReason
        /// </summary>
        public bool HasNotHitTargetReason
        {
            get
            {
                this.QueryNotHitTarget();
                return this.dtAutomatedLineMapping_NotHitTargetReason == null ? false : this.dtAutomatedLineMapping_NotHitTargetReason.Rows.Count > 0;
            }
        }

        /// <summary>
        /// dtNotHitTargetReason
        /// </summary>
        public DataTable DataNotHitTargetReason
        {
            get
            {
                return this.dtAutomatedLineMapping_NotHitTargetReason;
            }
        }

        /// <summary>
        /// P05_NotHitTargetReason
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="factoryID">factoryID</param>
        /// <param name="status">status</param>
        public P05_NotHitTargetReason(string id, string factoryID, string status)
        {
            this.InitializeComponent();
            this.id = id;
            this.factoryID = factoryID;
            this.status = status;
            this.EditMode = status != "Confirmed";
            this.Shown += this.P05_NotHitTargetReason_Shown;
        }

        private void P05_NotHitTargetReason_Shown(object sender, EventArgs e)
        {
            this.gridNotHitTargetReason.Columns["IEReasonID"].DefaultCellStyle.BackColor = this.status != "Confirmed" ? Color.Pink : Color.White;
            this.btnSave.Enabled = this.status != "Confirmed";
            this.QueryNotHitTarget();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.Helper.Controls.Grid.Generator(this.gridNotHitTargetReason)
               .Text("No", header: "No.", width: Widths.AnsiChars(4), iseditingreadonly: true)
               .Numeric("TotalGSDTimeAuto", header: "Total GSD Time" + Environment.NewLine + "(Auto)", width: Widths.AnsiChars(8), iseditingreadonly: true)
               .Numeric("TotalGSDTimeFinal", header: "Total GSD Time" + Environment.NewLine + "(Final)", width: Widths.AnsiChars(8), iseditingreadonly: true)
               .Numeric("SewerLoadingAuto", header: "Operator Loading" + Environment.NewLine + "(Auto) (%)", width: Widths.AnsiChars(8), iseditingreadonly: true)
               .Numeric("SewerLoadingFinal", header: "Operator Loading" + Environment.NewLine + "(Final) (%)", width: Widths.AnsiChars(8), iseditingreadonly: true)
               .CellIEReason("IEReasonID", "Reason ID", this, "AL", width: Widths.AnsiChars(6))
               .Text("Description", header: "Reason Name", width: Widths.AnsiChars(25), iseditingreadonly: true)
               .Text("EditName", header: "Edit By", width: Widths.AnsiChars(15), iseditingreadonly: true)
               .DateTime("EditDate", header: "Edit Date", width: Widths.AnsiChars(25), iseditingreadonly: true)
               ;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (this.dtAutomatedLineMapping_NotHitTargetReason.Rows.Count == 0)
            {
                return;
            }

            string sqlUpdate = $@"
alter table #tmp alter column No varchar(2)

delete  an
from AutomatedLineMapping_NotHitTargetReason an
where   ID = '{this.id}' and
        No not in (select No from #tmp where isnull(IEReasonID, '') <> '')

update  an set  an.IEReasonID = t.IEReasonID,
                an.EditName = '{Env.User.UserID}',
                an.EditDate = getdate()
from AutomatedLineMapping_NotHitTargetReason an
inner join #tmp t on t.No = an.No and isnull(t.IEReasonID, '') <> '' and t.IEReasonID <> an.IEReasonID
where an.ID = '{this.id}'

insert into AutomatedLineMapping_NotHitTargetReason(
ID
,No
,TotalGSDTimeAuto
,TotalGSDTimeFinal
,SewerLoadingAuto
,SewerLoadingFinal
,IEReasonID
)
select  '{this.id}',
        t.No,
        t.TotalGSDTimeAuto,
        t.TotalGSDTimeFinal,
        t.SewerLoadingAuto,
        t.SewerLoadingFinal,
        t.IEReasonID
from    #tmp t
where   isnull(t.IEReasonID, '') <> '' and
        not exists(
            select  1 from AutomatedLineMapping_NotHitTargetReason an with (nolock)
            where   ID = '{this.id}' and t.No = an.No
        )

";

            DualResult result = MyUtility.Tool.ProcessWithDatatable(this.dtAutomatedLineMapping_NotHitTargetReason, null, sqlUpdate, out DataTable dtEmpty);

            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.QueryNotHitTarget();
            MyUtility.Msg.InfoBox("Save successful");
        }

        /// <summary>
        /// QueryNotHitTarget
        /// </summary>
        public void QueryNotHitTarget()
        {
            DataRow drHitCondition;
            string sqlGetHitCondition = $@"
select Condition1, Condition2, Condition3
from AutomatedLineMappingConditionSetting with (nolock)
where   FactoryID = '{this.factoryID}' and
        Functions = 'IE_P05' and
        Verify = 'TargetReason' and
        Junk = 0";

            if (!MyUtility.Check.Seek(sqlGetHitCondition, out drHitCondition))
            {
                return;
            }

            List<SqlParameter> listPar = new List<SqlParameter>()
            {
                new SqlParameter("@Condition1", drHitCondition["Condition1"]),
                new SqlParameter("@Condition2", drHitCondition["Condition2"]),
                new SqlParameter("@Condition3", drHitCondition["Condition3"]),
                new SqlParameter("@Status", this.status),
            };

            string sqlCheck = $@"
select  amd.No,
        [TotalGSD] = sum(amd.GSD * amd.SewerDiffPercentage),
        [OperatorLoading] = Round(sum(amd.GSD * amd.SewerDiffPercentage) / (am.TotalGSDTime / am.SewerManpower) * 100, 0)
into    #tmpCheckHitFinal
from    AutomatedLineMapping am with (nolock)
inner   join  AutomatedLineMapping_Detail amd with (nolock) on amd.ID = am.ID
where   am.ID = '{this.id}' and
        amd.OperationID not in ('PROCIPF00004', 'PROCIPF00003') and
        amd.PPA <> 'C' and
        amd.IsNonSewingLine = 0
group by amd.No, am.TotalGSDTime, am.SewerManpower

select  amd.No,
        [TotalGSD] = sum(amd.GSD * amd.SewerDiffPercentage),
        [OperatorLoading] = Round(sum(amd.GSD * amd.SewerDiffPercentage) / (am.TotalGSDTime / am.SewerManpower) * 100, 0)
into    #tmpCheckHitAuto
from    AutomatedLineMapping am with (nolock)
inner   join  AutomatedLineMapping_DetailAuto amd with (nolock) on amd.ID = am.ID and amd.SewerManpower = am.SewerManpower
where   am.ID = '{this.id}' and
        amd.OperationID not in ('PROCIPF00004', 'PROCIPF00003') and
        amd.PPA <> 'C' and
        amd.IsNonSewingLine = 0
group by amd.No, am.TotalGSDTime, am.SewerManpower

select  tf.No,
        [TotalGSDTimeAuto] = ta.TotalGSD,
        [TotalGSDTimeFinal] = tf.TotalGSD,
        [SewerLoadingAuto] = ta.OperatorLoading,
        [SewerLoadingFinal] = tf.OperatorLoading,
        [IEReasonID] = isnull(anh.IEReasonID, ''),
        [Description] = isnull(i.Description, ''),
        anh.EditName,
        anh.EditDate
into    #tmpCheckHitFinalResult
from    #tmpCheckHitFinal tf
inner   join    #tmpCheckHitAuto ta on tf.No = ta.No
left    join    AutomatedLineMapping_NotHitTargetReason anh with (nolock) on anh.ID = '{this.id}' and anh.No = tf.No
left    join    IEReason i with (nolock) on i.ID = anh.IEReasonID and i.Type = 'AL'
where   tf.OperatorLoading > @Condition1 or
        ta.OperatorLoading > @Condition2 or
        (tf.OperatorLoading > ta.OperatorLoading and tf.OperatorLoading > @Condition3)
order by tf.No

--在單子confirm前如果因AutomatedLineMappingConditionSetting條件有變更，就將不符合的資料刪除
if(@Status <> 'Confirmed')
begin
    delete  an
    from AutomatedLineMapping_NotHitTargetReason an
    where   ID = '{this.id}' and
            No not in (select No from #tmpCheckHitFinalResult where isnull(IEReasonID, '') <> '')

    select  No,
            TotalGSDTimeAuto,
            TotalGSDTimeFinal,
            SewerLoadingAuto,
            SewerLoadingFinal,
            IEReasonID,
            Description,
            EditName,
            EditDate  
    from #tmpCheckHitFinalResult
    order by No
end
else
begin
    select  anh.No,
            anh.TotalGSDTimeAuto,
            anh.TotalGSDTimeFinal,
            anh.SewerLoadingAuto,
            anh.SewerLoadingFinal,
            anh.IEReasonID,
            i.Description,
            anh.EditName,
            anh.EditDate
    from AutomatedLineMapping_NotHitTargetReason anh with (nolock)
    left    join    IEReason i with (nolock) on i.ID = anh.IEReasonID and i.Type = 'AL'
    where   anh.ID = '{this.id}'
    order by anh.No
end

drop table #tmpCheckHitFinal, #tmpCheckHitFinalResult

";

            DualResult result = DBProxy.Current.Select(null, sqlCheck, listPar, out this.dtAutomatedLineMapping_NotHitTargetReason);

            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.gridNotHitTargetReason.DataSource = this.dtAutomatedLineMapping_NotHitTargetReason;
        }
    }
}
