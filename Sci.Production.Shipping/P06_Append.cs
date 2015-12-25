using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Shipping
{
    public partial class P06_Append : Sci.Win.Subs.Base
    {
        public DateTime pulloutDate;
        public P06_Append()
        {
            InitializeComponent();
        }

        //OK
        private void button1_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(dateBox1.Value))
            {
                MyUtility.Msg.WarningBox("Pull-out Date can't empty!!");
                return;
            }
            pulloutDate = (DateTime)dateBox1.Value;
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
