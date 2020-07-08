using Ict;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Sci.Production.Class
{
    public partial class txtLocalSuppNoConfirm : Sci.Win.UI._UserControl
    {
        public txtLocalSuppNoConfirm()
        {
            this.InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

        public Sci.Win.UI.TextBox TextBox1
        {
            get { return this.textBox1; }
        }

        public Sci.Win.UI.DisplayBox DisplayBox1
        {
            get { return this.displayBox1; }
            set { this.displayBox1 = value; }
        }

        [Bindable(true)]
        public string TextBox1Binding
        {
            get { return this.textBox1.Text; }
            set { this.textBox1.Text = value;  }
        }

        [Bindable(true)]
        public string DisplayBox1Binding
        {
            get { return this.displayBox1.Text; }
            set { this.displayBox1.Text = value; }
        }

        public virtual void textBox1_Validating(object sender, CancelEventArgs e)
        {
           // base.OnValidating(e);
            string textValue = this.textBox1.Text;

            if (!string.IsNullOrWhiteSpace(textValue) && textValue != this.textBox1.OldValue)
            {
                if (!MyUtility.Check.Seek(textValue, "LocalSupp", "ID"))
                {
                    this.textBox1.Text = string.Empty;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("< LocalSupplier Code: {0} > not found!!!", textValue));
                    return;
                }
            }

            this.ValidateControl();
        }

        private void textBox1_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Sci.Win.Forms.Base myForm = (Sci.Win.Forms.Base)this.FindForm();
            if (myForm.EditMode == false || this.textBox1.ReadOnly == true)
            {
                return;
            }

            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem("select ID,Name,Abb from LocalSupp WITH (NOLOCK) order by ID", "8,30,20", this.textBox1.Text);
            item.Width = 650;
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            this.textBox1.Text = item.GetSelectedString();
            this.ValidateControl();
            this.displayBox1.Text = item.GetSelecteds()[0]["Name"].ToString().TrimEnd();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            this.displayBox1.Text = MyUtility.GetValue.Lookup("Abb", this.textBox1.Text.ToString(), "LocalSupp", "ID");
        }
    }
}
