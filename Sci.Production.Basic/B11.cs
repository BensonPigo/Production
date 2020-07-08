using System;
using System.Drawing;
using System.Windows.Forms;

namespace Sci.Production.Basic
{
    /// <summary>
    /// B11
    /// </summary>
    public partial class B11 : Sci.Win.Tems.Input1
    {
        /// <summary>
        /// B11
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public B11(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();

            // 帶出Group/Quota的全名
            this.displayGroupQuota2.Text = MyUtility.GetValue.Lookup(string.Format("select Name from Reason WITH (NOLOCK) where ReasonTypeID = 'QuotaRegion' and ID = '{0}'", this.CurrentMaintain["QuotaArea"].ToString()));

            // 在編輯模式下，下列這些欄位都不可以被修改
            if (this.EditMode)
            {
                this.txtCountry.TextBox1.ReadOnly = true;
                this.editLabel.ReadOnly = true;
                this.txtPaytermarBulk.TextBox1.ReadOnly = true;
                this.txtPaytermarSample.TextBox1.ReadOnly = true;
                this.checkJunk.ReadOnly = true;
                this.checkScanPack.ReadOnly = true;
                this.checkVASSHAS.ReadOnly = true;
                this.checkSpecialCustomer.ReadOnly = true;
            }

            // 按鈕Shipping Mark變色
            if (!string.IsNullOrWhiteSpace(this.CurrentMaintain["MarkFront"].ToString()) ||
                !string.IsNullOrWhiteSpace(this.CurrentMaintain["MarkBack"].ToString()) ||
                !string.IsNullOrWhiteSpace(this.CurrentMaintain["MarkLeft"].ToString()) ||
                !string.IsNullOrWhiteSpace(this.CurrentMaintain["MarkRight"].ToString()))
            {
                this.btnShippingMark.ForeColor = Color.Blue;
            }
            else
            {
                this.btnShippingMark.ForeColor = Color.Black;
            }
        }

        private void BtnShippingMark_Click(object sender, EventArgs e)
        {
            Sci.Production.Basic.B11_ShippingMark callNextForm = new Sci.Production.Basic.B11_ShippingMark(false, this.CurrentMaintain);
            callNextForm.ShowDialog(this);
        }
    }
}
