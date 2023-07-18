using Ict;
using Ict.Win;
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
    /// P06_NotHitTargetReason
    /// </summary>
    public partial class P06_NotHitTargetReason : Sci.Win.Tems.QueryForm
    {
        private string id;
        private string factoryID;
        private string status;
        private DataTable dtLineMappingBalancing_NotHitTargetReason;

        /// <summary>
        /// HasNotHitTargetReason
        /// </summary>
        public bool HasNotHitTargetReason
        {
            get
            {
                this.QueryNotHitTarget();
                return this.dtLineMappingBalancing_NotHitTargetReason == null ? false : this.dtLineMappingBalancing_NotHitTargetReason.Rows.Count > 0;
            }
        }

        /// <summary>
        /// dtNotHitTargetReason
        /// </summary>
        public DataTable DataNotHitTargetReason
        {
            get
            {
                return this.dtLineMappingBalancing_NotHitTargetReason;
            }
        }

        /// <summary>
        /// P06_NotHitTargetReason
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="factoryID">factoryID</param>
        /// <param name="status">status</param>
        public P06_NotHitTargetReason(string id, string factoryID, string status)
        {
            this.InitializeComponent();
            this.id = id;
            this.factoryID = factoryID;
            this.status = status;
            this.EditMode = status != "Confirmed";
            this.Shown += this.P06_NotHitTargetReason_Shown;
        }

        private void P06_NotHitTargetReason_Shown(object sender, EventArgs e)
        {
            this.gridNotHitTargetReason.Columns["IEReasonID"].DefaultCellStyle.BackColor = this.status != "Confirmed" ? Color.Pink : Color.White;
            this.btnSave.Enabled = this.status != "Confirmed";
            this.QueryNotHitTarget();

            string sqlUpdate = $@"
alter table #tmp alter column No varchar(2)

delete  an
from LineMappingBalancing_NotHitTargetReason an
where   ID = '{this.id}' and
        No not in (select No from #tmp where isnull(IEReasonID, '') <> '')

";

            DualResult result = MyUtility.Tool.ProcessWithDatatable(this.dtLineMappingBalancing_NotHitTargetReason, null, sqlUpdate, out DataTable dtEmpty);

            if (!result)
            {
                this.ShowErr(result);
                return;
            }
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.Helper.Controls.Grid.Generator(this.gridNotHitTargetReason)
               .Text("No", header: "No.", width: Widths.AnsiChars(4), iseditingreadonly: true)
               .Numeric("TotalCycleTimeAuto", header: "Total Cycle Time" + Environment.NewLine + "(Auto)", width: Widths.AnsiChars(8), iseditingreadonly: true)
               .Numeric("TotalCycleTimeFinal", header: "Total Cycle Time" + Environment.NewLine + "(Final)", width: Widths.AnsiChars(8), iseditingreadonly: true)
               .Numeric("SewerLoadingAuto", header: "Operator Loading" + Environment.NewLine + "(Auto) (%)", width: Widths.AnsiChars(8), iseditingreadonly: true)
               .Numeric("SewerLoadingFinal", header: "Operator Loading" + Environment.NewLine + "(Final) (%)", width: Widths.AnsiChars(8), iseditingreadonly: true)
               .CellIEReason("IEReasonID", "Reason ID", this, "AS", width: Widths.AnsiChars(6))
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
            if (this.dtLineMappingBalancing_NotHitTargetReason.Rows.Count == 0)
            {
                return;
            }

            string sqlUpdate = $@"
alter table #tmp alter column No varchar(2)

delete  an
from LineMappingBalancing_NotHitTargetReason an
where   ID = '{this.id}' and
        No not in (select No from #tmp where isnull(IEReasonID, '') <> '')

update  an set  an.IEReasonID = t.IEReasonID,
                an.EditName = '{Env.User.UserID}',
                an.EditDate = getdate()
from LineMappingBalancing_NotHitTargetReason an
inner join #tmp t on t.No = an.No and isnull(t.IEReasonID, '') <> '' and t.IEReasonID <> an.IEReasonID
where an.ID = '{this.id}'

insert into LineMappingBalancing_NotHitTargetReason(
ID
,No
,TotalCycleTimeAuto
,TotalCycleTimeFinal
,SewerLoadingAuto
,SewerLoadingFinal
,IEReasonID
)
select  '{this.id}',
        t.No,
        t.TotalCycleTimeAuto,
        t.TotalCycleTimeFinal,
        t.SewerLoadingAuto,
        t.SewerLoadingFinal,
        t.IEReasonID
from    #tmp t
where   isnull(t.IEReasonID, '') <> '' and
        not exists(
            select  1 from LineMappingBalancing_NotHitTargetReason an with (nolock)
            where   ID = '{this.id}' and t.No = an.No
        )

";

            DualResult result = MyUtility.Tool.ProcessWithDatatable(this.dtLineMappingBalancing_NotHitTargetReason, null, sqlUpdate, out DataTable dtEmpty);

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
        Functions = 'IE_P06' and
        Verify = 'TargetReason' and
        Junk = 0";

            if (!MyUtility.Check.Seek(sqlGetHitCondition, out drHitCondition))
            {
                this.ShowErr("AutomatedLineMappingConditionSetting not exists");
                return;
            }

            List<SqlParameter> listPar = new List<SqlParameter>()
            {
                new SqlParameter("@Condition1", drHitCondition["Condition1"]),
                new SqlParameter("@Condition2", drHitCondition["Condition2"]),
                new SqlParameter("@Condition3", drHitCondition["Condition3"]),
            };

            string sqlCheck = $@"
select  amd.No,
        [TotalGSD] = sum(amd.GSD * amd.SewerDiffPercentage),
        [OperatorLoadingGSD] = Round(sum(amd.GSD * amd.SewerDiffPercentage) / (am.TotalGSDTime / am.SewerManpower) * 100, 0),
        [TotalCycle] = sum(amd.Cycle * amd.SewerDiffPercentage),
        [OperatorLoadingCycle] = Round(sum(amd.Cycle * amd.SewerDiffPercentage) / (am.TotalCycleTime / am.SewerManpower) * 100, 0)
into    #tmpCheckHit
from    LineMappingBalancing am with (nolock)
inner   join  LineMappingBalancing_Detail amd with (nolock) on amd.ID = am.ID
where   am.ID = '{this.id}' and
        amd.OperationID not in ('PROCIPF00004', 'PROCIPF00003') and
        amd.PPA <> 'C' and
        amd.IsNonSewingLine = 0
group by amd.No, am.TotalGSDTime, am.SewerManpower, am.TotalCycleTime

select  tf.No,
        [TotalCycleTimeAuto] = tf.TotalGSD,
        [TotalCycleTimeFinal] = tf.TotalCycle,
        [SewerLoadingAuto] = tf.OperatorLoadingGSD,
        [SewerLoadingFinal] = tf.OperatorLoadingCycle,
        [IEReasonID] = isnull(anh.IEReasonID, ''),
        [Description] = isnull(i.Description, ''),
        anh.EditName,
        anh.EditDate
from    #tmpCheckHit tf
left    join    LineMappingBalancing_NotHitTargetReason anh with (nolock) on anh.ID = '{this.id}' and anh.No = tf.No
left    join    IEReason i with (nolock) on i.ID = anh.IEReasonID and i.Type = 'AS'
where   tf.OperatorLoadingCycle > @Condition1 or
        tf.OperatorLoadingGSD > @Condition2 or
        (tf.OperatorLoadingCycle > tf.OperatorLoadingGSD and tf.OperatorLoadingCycle > @Condition3)
order by tf.No
";

            DualResult result = DBProxy.Current.Select(null, sqlCheck, listPar, out this.dtLineMappingBalancing_NotHitTargetReason);

            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.gridNotHitTargetReason.DataSource = this.dtLineMappingBalancing_NotHitTargetReason;
        }
    }
}
