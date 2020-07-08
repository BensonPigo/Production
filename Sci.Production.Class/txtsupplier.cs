using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Sci.Production.Class
{
    public partial class txtsupplier : Sci.Win.UI._UserControl
    {
        public txtsupplier()
        {
            this.InitializeComponent();
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
            string textValue = this.textBox1.Text;
            if (!string.IsNullOrWhiteSpace(textValue) && textValue != this.textBox1.OldValue)
            {
                if (!MyUtility.Check.Seek(textValue, "Production.dbo.Supp", "ID"))
                {
                    this.textBox1.Text = string.Empty;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("< Supplier Code: {0} > not found!!!", textValue));
                    return;
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            this.displayBox1.Text = MyUtility.GetValue.Lookup("AbbEN", this.textBox1.Text.ToString(), "Production.dbo.Supp", "ID");
        }

        private void textBox1_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Sci.Win.Forms.Base myForm = (Sci.Win.Forms.Base)this.FindForm();
            if (myForm.EditMode == false || this.textBox1.ReadOnly == true)
            {
                return;
            }

            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem("select ID,AbbCH,AbbEN from Production.dbo.Supp WITH (NOLOCK) order by ID", "8,30,30", this.textBox1.Text);
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            this.textBox1.Text = item.GetSelectedString();
        }
    }
}
