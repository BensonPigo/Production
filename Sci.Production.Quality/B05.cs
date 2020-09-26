using System;
using System.Windows.Forms;

namespace Sci.Production.Quality
{
    public partial class B05 : Win.Tems.Input1
    {
        private readonly ToolTip toolTip1 = new ToolTip();

        public B05(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        private void NumWeight_TextChanged(object sender, EventArgs e)
        {
            this.numWeight.MaxLength = 2;
        }
    }
}
