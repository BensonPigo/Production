using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.IE
{
    public partial class B05 : Sci.Win.Tems.Input1
    {
        public B05(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            string sql = String.Format("select * from [MachineType_ThreadRatio] where ID='{0}'",CurrentMaintain["ID"].ToString());
            if (MyUtility.Check.Seek(sql, null))
            {
                btnThreadRatio.ForeColor = Color.Blue;
            }
            else
            {
                btnThreadRatio.ForeColor = Control.DefaultForeColor;
            }
        }

        private void btnThreadRatio_Click(object sender, EventArgs e)
        {
            Sci.Production.IE.B05_ThreadRatio callNextForm = new Sci.Production.IE.B05_ThreadRatio(CurrentMaintain["ID"].ToString());
            DialogResult result = callNextForm.ShowDialog(this);
        }
    }
}
