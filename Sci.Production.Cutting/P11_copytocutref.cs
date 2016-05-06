using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Cutting
{
    public partial class P11_copytocutref : Sci.Win.Subs.Base
    {
        public string copycutref;
        public P11_copytocutref()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Copy_Click(object sender, EventArgs e)
        {
            copycutref = textBox1.Text;
            if (MyUtility.Check.Empty(copycutref))
            {
                MyUtility.Msg.WarningBox("CutRef# can not empty!");
                return;
            }
            this.Close();
        }
    }
}
