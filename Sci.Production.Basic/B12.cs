using System.Windows.Forms;
using Ict.Win;

namespace Sci.Production.Basic
{
    /// <summary>
    /// B12
    /// </summary>
    public partial class B12 : Sci.Win.Tems.Input6
    {
        /// <summary>
        /// B12
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public B12(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            this.Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("UnitTo", header: "Conversion unit", width: Widths.AnsiChars(8))
            .Text("Rate", header: "Conversion rate", width: Widths.AnsiChars(15))
            .Text("AddName", header: "Create by", width: Widths.AnsiChars(8))
            .DateTime("AddDate", header: "Create at", width: Widths.AnsiChars(15))
            .Text("EditName", header: "Edit by", width: Widths.AnsiChars(8))
            .DateTime("EditDate", header: "Edit at", width: Widths.AnsiChars(15));
        }
    }
}
