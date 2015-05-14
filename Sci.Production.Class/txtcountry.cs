using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sci.Data;
using Sci.Win.UI;

namespace Sci.Production.Class
{
    public partial class txtcountry : UserControl
    {
        public txtcountry()
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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            this.displayBox1.Text = myUtility.Lookup("Alias", this.textBox1.Text.ToString(), "Country", "Id");
        }

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            base.OnValidating(e);
            string textValue = this.textBox1.Text;
            if (!string.IsNullOrWhiteSpace(textValue) && textValue != this.textBox1.OldValue)
            {
                if (!myUtility.Seek(textValue, "Country", "ID"))
                {
                    MessageBox.Show(string.Format("< Country: {0} > not found!!!", textValue));
                    this.textBox1.Text = "";
                    e.Cancel = true;
                    return;
                }
            }
        }

        private void textBox1_PopUp(object sender, TextBoxPopUpEventArgs e)
        {
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem("select ID,Alias from Country where Junk = 0 order by ID", "4,30", this.textBox1.Text);
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel) { return; }
            this.textBox1.Text = item.GetSelectedString();
        }     
    }
}
