using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.PPIC
{
    public partial class P24_WriteDesc : Sci.Win.Forms.Base
    {
        public string strDesc;
        public P24_WriteDesc()
        {
            InitializeComponent();
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.editDesc.Text = string.Empty;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.strDesc = this.editDesc.Text;
            this.Close();
        }
    }
}
