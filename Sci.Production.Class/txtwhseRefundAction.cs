using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Class
{
    public partial class txtwhseRefundAction : Sci.Win.UI._UserControl
    {
        public txtwhseRefundAction()
        {
            InitializeComponent();
        }

        private txtwhseReason whseReason;

        [Category("Custom Properties")]
        [Description("選擇畫面上的txtwhseReason自訂控制項名稱。例如：txtWhseReason1")]
        public txtwhseReason WhseReason
        {
            set { whseReason = value; }
            get { return whseReason; }
        }

        public Sci.Win.UI.TextBox TextBox1
        {
            get { return this.textBox1; }
        }

        public Sci.Win.UI.DisplayBox DisplayBox1
        {
            get { return this.displayBox1; }
        }

        [Bindable(true)]
        public string TextBox1Binding
        {
            set { this.textBox1.Text = value; }
            get { return textBox1.Text; }
        }

        [Bindable(true)]
        public string DisplayBox1Binding
        {
            set { this.displayBox1.Text = value; }
            get { return this.displayBox1.Text; }
        }

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
          //  base.OnValidating(e);
            string str = this.textBox1.Text;
            if (!string.IsNullOrWhiteSpace(str) && str != this.textBox1.OldValue)
            {
                if (!MyUtility.Check.Seek(str, "WhseReason", "ID"))
                {
                    MyUtility.Msg.WarningBox(string.Format("< Refund Action: {0} > not found!!!", str));
                    this.textBox1.Text = "";
                    e.Cancel = true;
                    return;
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Sci.Win.Forms.Base myForm = (Sci.Win.Forms.Base)this.FindForm();
            if (myForm.EditMode == false)
            {
                this.displayBox1.Text = MyUtility.GetValue.Lookup("Description", "RA" + this.textBox1.Text.ToString(), "WhseReason", "Type+ID");
            }
        }

        private void textBox1_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            String actionCode = MyUtility.GetValue.Lookup("actioncode", "RR"+whseReason.TextBox1.Text, "WhseReason", "Type+ID");
            Sci.Win.Tools.SelectItem item = 
                new Sci.Win.Tools.SelectItem("select Id, Description from WhseReason where type ='RA' "+
                string.Format(" and id in ({0})", actionCode), "10,100", this.textBox1.Text);
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel) { return; }
            this.textBox1.Text = item.GetSelectedString();
            this.Validate();
        }
    }
}
