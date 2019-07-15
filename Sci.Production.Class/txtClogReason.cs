using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ict;

namespace Sci.Production.Class
{
    public partial class txtClogReason : Sci.Win.UI._UserControl
    {
        public txtClogReason()
        {
            InitializeComponent();
        }

        [Category("Custom Properties")]
        [Description("填入Reason Type。例如：RR")]
        public string Type { set; get ; }


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
           // base.OnValidating(e);
            string str = this.textBox1.Text;
            //if (!string.IsNullOrWhiteSpace(str) && str != this.textBox1.OldValue)
            if (!string.IsNullOrWhiteSpace(str) )
            {
                if (!MyUtility.Check.Seek(Type + str, "ClogReason", "type+ID"))
                {
                    this.DisplayBox1.Text = "";
                    this.textBox1.Text = "";
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("< Reason: {0} > not found!!!", str));
                    this.DataBindings.Cast<Binding>().ToList().ForEach(binding => binding.WriteValue());
                    return;
                }

                DataRow temp;
                if (MyUtility.Check.Seek(string.Format("Select Description from ClogReason WITH (NOLOCK) where ID='{0}' and Type='{1}'", str, Type), out temp))
                    this.DisplayBox1.Text = temp[0].ToString();

                this.DataBindings.Cast<Binding>().ToList().ForEach(binding => binding.WriteValue());
            }

            if (string.IsNullOrWhiteSpace(str))
            {
                this.DisplayBox1.Text = "";
                return;
            }
            if (e.Cancel)
            { return; }
              
            this.ValidateControl();
        }

        private void textBox1_Validated(object sender, EventArgs e)
        {
            this.OnValidated(e);
        }

        private void textBox1_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem
                (string.Format("Select Id, Description from ClogReason WITH (NOLOCK) where type='{0}' order by id", Type), "10,30", this.textBox1.Text);
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel) { return; }
            this.textBox1.Text = item.GetSelectedString();
            this.DisplayBox1.Text = item.GetSelecteds()[0][1].ToString();
            this.Validate();
            this.DataBindings.Cast<Binding>().ToList().ForEach(binding => binding.WriteValue());
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            string str = this.textBox1.Text;
            if (!string.IsNullOrWhiteSpace(str) )
            {
                if (!MyUtility.Check.Seek(Type + str, "ClogReason", "type+ID"))
                {
                    this.DisplayBox1.Text = "";
                    this.textBox1.Text = "";
                    textBox1.Focus();
                    MyUtility.Msg.WarningBox(string.Format("< Reason: {0} > not found!!!", str));
                    this.DataBindings.Cast<Binding>().ToList().ForEach(binding => binding.WriteValue());
                    return;
                }
                DataRow temp;
                if (MyUtility.Check.Seek(string.Format("Select Description from ClogReason WITH (NOLOCK) where ID='{0}' and Type='{1}'", str, Type), out temp))
                    this.DisplayBox1.Text = temp[0].ToString();

                this.DataBindings.Cast<Binding>().ToList().ForEach(binding => binding.WriteValue());
            }
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            this.displayBox1.Text = MyUtility.GetValue.Lookup($@"
Select Description from ClogReason WITH (NOLOCK) where ID='{this.textBox1.Text}' and Type='{Type}'");

        }
    }
}
