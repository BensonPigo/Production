using Ict;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    public partial class P22_FabricSticker : Sci.Win.Subs.Base
    {
        private object strSubTransferID;

        public P22_FabricSticker(object strSubTransferID)
        {
            InitializeComponent();
            this.strSubTransferID = strSubTransferID;
            this.grid1.IsEditingReadOnly = false;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            DualResult result;
            DataTable dtPrint = (DataTable)this.listControlBindingSource.DataSource;
        }
    }
}
