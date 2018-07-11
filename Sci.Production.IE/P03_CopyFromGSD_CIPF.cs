using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Sci.Production.IE;

namespace Sci.Production.IE
{
    public partial class P03_CopyFromGSD_CIPF : Sci.Win.Forms.Base
    {
        public P03_CopyFromGSD_CIPF()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            P03CIPFinfo.cutting = false;
            P03CIPFinfo.inspection = false;
            P03CIPFinfo.pressing = false;
            P03CIPFinfo.packing = false;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            P03CIPFinfo.cutting = chkCutting.Checked;
            P03CIPFinfo.inspection = chkInspection.Checked;
            P03CIPFinfo.pressing = chkPressing.Checked;
            P03CIPFinfo.packing = chkPacking.Checked;
            this.Close();
        }
    }
}
