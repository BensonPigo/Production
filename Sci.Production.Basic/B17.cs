using System.Windows.Forms;

namespace Sci.Production.Basic
{
    /// <summary>
    /// B17
    /// </summary>
    public partial class B17 : Win.Tems.Input1
    {
        /// <summary>
        /// B17
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public B17(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtID.ReadOnly = true;
        }
    }
}
