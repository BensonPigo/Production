using System;
using System.Windows.Forms;

namespace Sci.Production.Shipping
{
    public partial class R40_SelectFileDialog : Win.Forms.Base
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="R40_SelectFileDialog"/> class.
        /// </summary>
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
