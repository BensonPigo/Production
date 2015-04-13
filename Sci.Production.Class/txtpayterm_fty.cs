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
    public partial class txtpayterm_fty : UserControl
    {
        public txtpayterm_fty()
        {
            InitializeComponent();
        }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Sci.Win.UI.TextBox TextBox1
        {
            get { return this.textBox1; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Sci.Win.UI.DisplayBox DisplayBox1
        {
            get { return this.displayBox1; }
        }

        [Bindable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public string TextBox1Binding
        {
            set { this.textBox1.Text = value; }
            get { return textBox1.Text; }
        }

        [Bindable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public string DisplayBox1Binding
        {
            set { this.displayBox1.Text = value; }
            get { return this.displayBox1.Text; }
        }

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            string str = this.textBox1.Text;
            if (!string.IsNullOrWhiteSpace(str) && str != this.textBox1.OldValue)
            {
                if (!myUtility.Seek(str, "PayTerm", "ID"))
                {
                    MessageBox.Show(string.Format("< Pay Term: {0} > not found!!!", str));
                    this.textBox1.Text = "";
                    e.Cancel = true;
                    return;
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            this.displayBox1.Text = myUtility.Lookup("Name", this.textBox1.Text.ToString(), "PayTerm", "ID");
        }

        private void textBox1_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem("PayTerm.Id,Name", "10,100", this.textBox1.Text);
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel) { return; }
            this.textBox1.Text = item.GetSelectedString();
        }
    }
}
