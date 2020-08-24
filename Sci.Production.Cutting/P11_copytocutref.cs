using System;

namespace Sci.Production.Cutting
{
    /// <inheritdoc/>
    public partial class P11_copytocutref : Win.Subs.Base
    {
        /// <inheritdoc/>
        public string Copycutref { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="P11_copytocutref"/> class.
        /// </summary>
        public P11_copytocutref()
        {
            this.InitializeComponent();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnCopy_Click(object sender, EventArgs e)
        {
            this.Copycutref = this.txtCutRef.Text;
            if (MyUtility.Check.Empty(this.Copycutref))
            {
                MyUtility.Msg.WarningBox("CutRef# can not empty!");
                return;
            }

            this.Close();
        }
    }
}
