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
    public partial class P18_MessageBox : Form
    {
        public P18_MessageBox()
        {
            InitializeComponent();
        }

        private void btnLacking_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Yes;
        }

        private void btnContinue_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
        }

        private void P18_MessageBox_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
    }
}
