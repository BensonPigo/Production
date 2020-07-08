using System;
using System.Drawing;
using System.Windows.Forms;

namespace Sci.Production.IE
{
    /// <summary>
    /// IE_B05
    /// </summary>
    public partial class B05 : Win.Tems.Input1
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

            // this.displayGroupID.Text = this.grid.SelectedRows[0].Cells["MachineGroup"].Value.ToString();
        }

        private void BtnThreadRatio_Click(object sender, EventArgs e)
        {
            B05_ThreadRatio callNextForm = new B05_ThreadRatio(this.CurrentMaintain["ID"].ToString());
            DialogResult result = callNextForm.ShowDialog(this);
        }
    }
}
