using System.Windows.Forms;
using Ict.Win;
using System.Data;
using Ict;

namespace Sci.Production.Centralized
{
    /// <summary>
    /// IE_B90
    /// </summary>
    public partial class IE_B90 : Win.Tems.Input6
    {
        /// <summary>
        /// IE_B90
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public IE_B90(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string type = (e.Master == null) ? string.Empty : MyUtility.Convert.GetString(e.Master["Type"]);
            string sqlCmd = $@"
Select TypeGroup, Junk
From IEReasonTypeGroup
Where Type = '{type}'";

            this.DetailSelectCommand = sqlCmd;
            return base.OnDetailSelectCommandPrepare(e);
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();
            this.detailgrid.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("TypeGroup", header: "TypeGroup", width: Widths.AnsiChars(10), iseditingreadonly: false)
                .CheckBox("junk", header: "Junk", width: Widths.AnsiChars(5), iseditable: true)
                ;
        }

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtType.Enabled = false;
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            if (this.CurrentMaintain["Type"].Empty())
            {
                MyUtility.Msg.WarningBox("Type cannot be empty!!");
                return false;
            }

            return base.ClickSaveBefore();
        }
    }
}
