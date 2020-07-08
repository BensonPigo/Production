using System;

namespace Sci.Production.Cutting
{
    public partial class P11_copytocutref : Sci.Win.Subs.Base
    {
        public string copycutref;

        public P11_copytocutref()
        {
            this.InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            this.copycutref = this.txtCutRef.Text;
            if (MyUtility.Check.Empty(this.copycutref))
            {
                MyUtility.Msg.WarningBox("CutRef# can not empty!");
                return;
            }

            this.Close();
        }
    }
}
