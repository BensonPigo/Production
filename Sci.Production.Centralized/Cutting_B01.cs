using Ict;
using Sci.Data;
using System.Data;
using System.Windows.Forms;

namespace Sci.Production.Centralized
{
    /// <summary>
    /// Cutting_B01
    /// </summary>
    public partial class Cutting_B01 : Win.Tems.Input1
    {
        /// <summary>
        /// Cutting_B01
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public Cutting_B01(ToolStripMenuItem menuitem)
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
            this.txtID.ReadOnly = true;
        }
    }
}
