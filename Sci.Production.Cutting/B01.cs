using Ict;
using Sci.Data;
using Sci.Production.Class;
using System.Data;
using System.Windows.Forms;

namespace Sci.Production.Cutting
{
    /// <inheritdoc/>
    public partial class B01 : Win.Tems.Input1
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="B01"/> class.
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public B01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();

            DualResult result;
            DataTable dt = new DataTable();
            string cmd = "SELECT ID, Name FROM DropDownList WITH (NOLOCK)  WHERE Type='SubProcess_InOutRule'";
            if (result = DBProxy.Current.Select(null, cmd, out dt))
            {
                MyUtility.Tool.SetupCombox(this.combInOutRule, 2, dt);
            }
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();

            this.displayTPECreate.Text = string.Empty;
            this.displayTPEEdit.Text = string.Empty;
            var sqlcmd = @"
select TPECreate = Concat(dbo.getTPEPass1(TPEAddName), '  ', TPEAddDate)
     , TPEEdit = Concat(dbo.getTPEPass1(TPEEditName), '  ', TPEEditDate)
from Subprocess
where Id = @Id";
            var result = DBProxy.Current.SeekEx(sqlcmd, "@Id", this.CurrentMaintain["Id"].ToString());
            if (result)
            {
                var dr = result.ExtendedData;
                this.displayTPECreate.Text = dr["TPECreate"].ToString();
                this.displayTPEEdit.Text = dr["TPEEdit"].ToString();
            }
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(this.txtID.Text.Trim()) || MyUtility.Check.Empty(this.txtShowSeq.Text.Trim()))
            {
                MyUtility.Msg.InfoBox("'ID' and 'Show Seq' can not empty");
                return false;
            }

            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            var canEditkIsDisableMachineNoEntry = this.CurrentMaintain["ID"].EqualString("PRT") && MyUtility.Convert.GetBool(this.CurrentMaintain["IsSubprocessInspection"]);
            this.chkIsDisableMachineNoEntry.ReadOnly = !canEditkIsDisableMachineNoEntry;
        }
    }
}
