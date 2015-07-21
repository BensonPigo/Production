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
            this.label1.Text = lableName;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.dateBox1.Text))
            {
                MyUtility.Msg.WarningBox("Date can't be empty!");
                this.dateBox1.Focus();
                return;
            }

            returnDate = Convert.ToDateTime(this.dateBox1.Text);
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
