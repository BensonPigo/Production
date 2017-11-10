using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.IE
{
    /// <summary>
    /// IE_B05
    /// </summary>
    public partial class B05 : Sci.Win.Tems.Input1
    {
        /// <summary>
        /// B05
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public B05(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// OnDetailEntered()
        /// </summary>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            string sql = string.Format("select * from [MachineType_ThreadRatio] where ID='{0}'", this.CurrentMaintain["ID"].ToString());
            if (MyUtility.Check.Seek(sql, null))
            {
                this.btnThreadRatio.ForeColor = Color.Blue;
            }
            else
            {
                this.btnThreadRatio.ForeColor = Control.DefaultForeColor;
            }
        }

        private void BtnThreadRatio_Click(object sender, EventArgs e)
        {
            Sci.Production.IE.B05_ThreadRatio callNextForm = new Sci.Production.IE.B05_ThreadRatio(this.CurrentMaintain["ID"].ToString());
            DialogResult result = callNextForm.ShowDialog(this);
        }
    }
}
