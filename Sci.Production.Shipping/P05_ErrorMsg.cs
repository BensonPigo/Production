using System;

namespace Sci.Production.Shipping
{
    public partial class P05_ErrorMsg : Win.Subs.Base
    {
        /// <summary>
        /// P05_ErrorMsg
        /// </summary>
        /// <param name="errMsg">errMsg</param>
        public P05_ErrorMsg(string errMsg)
        {
            this.InitializeComponent();
            this.labelErrMsg.Text = errMsg;
            this.labelErrMsg.Height = this.labelErrMsg.Height + 20;
            this.Width = this.labelErrMsg.Width + 20;
            this.Height = this.labelErrMsg.Height + this.buttonOK.Height + 70;
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
