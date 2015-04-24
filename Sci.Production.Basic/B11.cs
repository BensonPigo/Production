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
            this.displayBox5.Text = myUtility.Lookup("Name", "QuotaRegion                                       " + CurrentMaintain["QuotaArea"].ToString(), "Reason", "ReasonTypeID+ID");

            //在編輯模式下，下列這些欄位都不可以被修改
            if (this.EditMode)
            {
                this.txtcountry1.TextBox1.ReadOnly = true;
                this.editBox1.ReadOnly = true;
                this.txtpaytermar1.TextBox1.ReadOnly = true;
                this.txtpaytermar2.TextBox1.ReadOnly = true;
                this.checkBox1.ReadOnly = true;
                this.checkBox2.ReadOnly = true;
                this.checkBox3.ReadOnly = true;
                this.checkBox4.ReadOnly = true;
            }

            //按鈕Shipping Mark變色
            if (!String.IsNullOrWhiteSpace(CurrentMaintain["MarkFront"].ToString()) ||
                !String.IsNullOrWhiteSpace(CurrentMaintain["MarkBack"].ToString()) ||
                !String.IsNullOrWhiteSpace(CurrentMaintain["MarkLeft"].ToString()) ||
                !String.IsNullOrWhiteSpace(CurrentMaintain["MarkRight"].ToString()))
            { 
                this.button1.ForeColor = Color.Blue;
            }
            else
            {
                this.button1.ForeColor = Color.Black;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Sci.Production.Basic.B11_ShippingMark callNextForm = new Sci.Production.Basic.B11_ShippingMark(false, CurrentMaintain);
            callNextForm.ShowDialog(this);
        }
    }
}
