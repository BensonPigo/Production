﻿using System;
using System.Windows.Forms;

namespace Sci.Production.Packing
{
    /// <inheritdoc/>
    public partial class P18_MessageBox : Form
    {
        /// <inheritdoc/>
        public P18_MessageBox()
        {
            this.InitializeComponent();
        }

        private void BtnLacking_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Yes;
        }

        private void BtnContinue_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
        }

        private void P18_MessageBox_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void BtnEndScanning_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
