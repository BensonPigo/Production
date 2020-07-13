using System.Windows.Forms;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// B08
    /// </summary>
    public partial class B08 : Win.Tems.Input6
    {
        /// <summary>
        /// B08
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public B08(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .Numeric("Day", header: "Day")
                .Numeric("Efficiency", header: "Efficiency (%)");
        }
    }
}
