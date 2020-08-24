using Ict;
using Sci.Data;
using System.Data;
using System.Windows.Forms;

namespace Sci.Production.Cutting
{
    public partial class B01 : Win.Tems.Input1
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="B01"/> class.
        /// </summary>
        /// <param name="menuitem"></param>
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
        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(this.txtID.Text.Trim()) || MyUtility.Check.Empty(this.txtShowSeq.Text.Trim()))
            {
                MyUtility.Msg.InfoBox("'ID' and 'Show Seq' can not empty");
                return false;
            }

            return base.ClickSaveBefore();
        }
    }
}
