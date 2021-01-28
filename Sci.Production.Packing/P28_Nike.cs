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
    public partial class P28_Nike : Sci.Win.Tems.QueryForm
    {
        /// <inheritdoc/>
        public P28_Nike()
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
