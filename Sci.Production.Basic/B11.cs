using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Basic
{
    public partial class B11 : Sci.Win.Tems.Input1
    {
        public B11(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            //帶出Group/Quota的全名
            this.displayGroupQuota2.Text = MyUtility.GetValue.Lookup(string.Format("select Name from Reason WITH (NOLOCK) where ReasonTypeID = 'QuotaRegion' and ID = '{0}'", CurrentMaintain["QuotaArea"].ToString()));

            //在編輯模式下，下列這些欄位都不可以被修改
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

            //按鈕Shipping Mark變色
            if (!String.IsNullOrWhiteSpace(CurrentMaintain["MarkFront"].ToString()) ||
                !String.IsNullOrWhiteSpace(CurrentMaintain["MarkBack"].ToString()) ||
                !String.IsNullOrWhiteSpace(CurrentMaintain["MarkLeft"].ToString()) ||
                !String.IsNullOrWhiteSpace(CurrentMaintain["MarkRight"].ToString()))
            { 
                this.btnShippingMark.ForeColor = Color.Blue;
            }
            else
            {
                this.btnShippingMark.ForeColor = Color.Black;
            }
        }

        private void btnShippingMark_Click(object sender, EventArgs e)
        {
            Sci.Production.Basic.B11_ShippingMark callNextForm = new Sci.Production.Basic.B11_ShippingMark(false, CurrentMaintain);
            callNextForm.ShowDialog(this);
        }
    }
}
