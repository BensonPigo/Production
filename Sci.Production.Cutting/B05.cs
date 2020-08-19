using System.Windows.Forms;

namespace Sci.Production.Cutting
{
    public partial class B05 : Win.Tems.Input1
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="B05"/> class.
        /// </summary>
        /// <param name="menuitem"></param>
        public B05(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = $" MDivisionid = '{Env.User.Keyword}' ";
            this.txtCell1.MDivisionID = Env.User.Keyword;
        }

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtID.ReadOnly = true;
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["MDivisionid"] = Env.User.Keyword;
        }
    }
}
