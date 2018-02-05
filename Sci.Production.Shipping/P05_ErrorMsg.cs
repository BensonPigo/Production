using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Shipping
{
    public partial class P05_ErrorMsg : Sci.Win.Subs.Base
    {

        /// <summary>
        /// P05_ErrorMsg
        /// </summary>
        /// <param name="errMsg">errMsg</param>
        public P05_ErrorMsg(string errMsg)
        {
            this.InitializeComponent();
            this.labelErrMsg.Text = errMsg;
            this.Width = this.labelErrMsg.Width + 20;
            this.Height = this.labelErrMsg.Height + this.buttonOK.Height + 80;
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
