using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Sci.Production.Class
{
    public partial class txtwhseRefundAction : Sci.Win.UI._UserControl
    {
        public txtwhseRefundAction()
        {
            this.InitializeComponent();
        }

        private txtwhseReason whseReason;

        [Category("Custom Properties")]
        [Description("選擇畫面上的txtwhseReason自訂控制項名稱。例如：txtWhseReason1")]
        public txtwhseReason WhseReason
        {
            get { return this.whseReason; }
            set { this.whseReason = value; }
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
            get { return this.textBox1.Text; }
            set { this.textBox1.Text = value; }
        }

        [Bindable(true)]
        public string DisplayBox1Binding
        {
            get { return this.displayBox1.Text; }
            set { this.displayBox1.Text = value; }
        }

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
          // base.OnValidating(e);
            string str = this.textBox1.Text;
            if (!string.IsNullOrWhiteSpace(str) && str != this.textBox1.OldValue)
            {
                String actionCode = MyUtility.GetValue.Lookup("actioncode", "RR" + this.whseReason.TextBox1.Text, "WhseReason", "Type+ID");
                string sqlcmd = string.Format(@"select Id, Description from WhseReason WITH (NOLOCK) where type ='RA'and id in ({0}) and id in ('{1}')", actionCode, str);

                // if (!MyUtility.Check.Seek(str, "WhseReason", "ID"))
                if (!MyUtility.Check.Seek(sqlcmd))
                {
                    this.DisplayBox1.Text = string.Empty;
                    this.textBox1.Text = string.Empty;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("< Refund Action: {0} > not found!!!", str));
                    this.DataBindings.Cast<Binding>().ToList().ForEach(binding => binding.WriteValue());
                    return;
                }

                DataRow temp;
                if (MyUtility.Check.Seek(string.Format("Select Description from WhseReason WITH (NOLOCK) where ID='{0}' and Type='RA'", str), out temp))
                {
                    this.DisplayBox1.Text = temp[0].ToString();
                }

                this.DataBindings.Cast<Binding>().ToList().ForEach(binding => binding.WriteValue());
            }
        }

        // private void textBox1_TextChanged(object sender, EventArgs e)
        // {
        //    Sci.Win.Forms.Base myForm = (Sci.Win.Forms.Base)this.FindForm();
        //    if (myForm.EditMode == false)
        //    {
        //        this.displayBox1.Text = MyUtility.GetValue.Lookup("Description", "RA" + this.textBox1.Text.ToString(), "WhseReason", "Type+ID");
        //    }
        // }
        private void textBox1_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            String actionCode = MyUtility.GetValue.Lookup("actioncode", "RR" + this.whseReason.TextBox1.Text, "WhseReason", "Type+ID");
            if (actionCode == string.Empty)
            {
                MyUtility.Msg.WarningBox("can't fount data!", "Warning");
                return;
            }

            Sci.Win.Tools.SelectItem item =
                new Sci.Win.Tools.SelectItem(
                    "select Id, Description from WhseReason WITH (NOLOCK) where type ='RA' " +
                string.Format(" and id in ({0})", actionCode), "10,30", this.textBox1.Text);
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.textBox1.Text = item.GetSelectedString();
            this.DisplayBox1.Text = item.GetSelecteds()[0][1].ToString();
            this.Validate();
            this.DataBindings.Cast<Binding>().ToList().ForEach(binding => binding.WriteValue());
        }
    }
}
