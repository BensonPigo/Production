using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Packing
{
    /// <inheritdoc/>
    public partial class P28 : Sci.Win.Tems.QueryForm
    {
        /// <inheritdoc/>
        public P28(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        private void BtnNike_Click(object sender, EventArgs e)
        {
            P28_Nike form = new P28_Nike();
            form.ShowDialog();
        }

        private void BtnUA_Click(object sender, EventArgs e)
        {
            P28_UA form = new P28_UA();
            form.ShowDialog();
        }
    }
}
