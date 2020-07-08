using System;
using System.Windows.Forms;

namespace Sci.Production.Shipping
{
    public partial class R40_SelectFileDialog : Win.Forms.Base
    {
        public R40_SelectFileDialog()
        {
            this.InitializeComponent();
        }

        private void BtnSelectFile_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        }
    }
}
