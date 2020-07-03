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
    /// <summary>
    /// P03_CopyFromGSD_CIPF
    /// </summary>
    public partial class P03_CopyFromGSD_CIPF : Sci.Win.Forms.Base
    {
        /// <summary>
        /// P03_CopyFromGSD_CIPF
        /// </summary>
        public P03_CopyFromGSD_CIPF()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// No Need
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void Button2_Click(object sender, EventArgs e)
        {
            P03CIPFinfo.Cutting = false;
            P03CIPFinfo.Inspection = false;
            P03CIPFinfo.Pressing = false;
            P03CIPFinfo.Packing = false;
            this.Close();
        }

        /// <summary>
        /// Comfirm
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void Button1_Click(object sender, EventArgs e)
        {
            P03CIPFinfo.Cutting = this.chkCutting.Checked;
            P03CIPFinfo.Inspection = this.chkInspection.Checked;
            P03CIPFinfo.Pressing = this.chkPressing.Checked;
            P03CIPFinfo.Packing = this.chkPacking.Checked;
            this.Close();
        }
    }
}
