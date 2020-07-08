using System;
using System.Windows.Forms;

namespace Sci.Production.Packing
{
    public partial class P18_MessageBox : Form
    {
        public P18_MessageBox()
        {
            this.InitializeComponent();
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
