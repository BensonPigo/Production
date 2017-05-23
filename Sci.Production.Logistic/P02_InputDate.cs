using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Logistic
{
    public partial class P02_InputDate : Sci.Win.Subs.Base
    {
        public DateTime returnDate;
        public P02_InputDate(string captionName, string lableName)
        {
            InitializeComponent();
            this.Text = captionName;
            this.labelReceiveDate.Text = lableName;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.dateReceiveDate.Text))
            {
                this.dateReceiveDate.Focus();
                MyUtility.Msg.WarningBox("Date can't be empty!");
                return;
            }

            returnDate = Convert.ToDateTime(this.dateReceiveDate.Text);
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
