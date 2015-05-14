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
using Ict;
using Sci.Win.Tools;

namespace Sci.Production.Class
{
    public partial class txtsubcon : UserControl
    {

        private bool isIncludeJunk;

        public txtsubcon()
        {
            InitializeComponent();
        }

        [Category("Custom Properties")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool IsIncludeJunk
        {
            set { this.isIncludeJunk = value; }
            get { return this.isIncludeJunk; }
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
            base.OnValidating(e);
            string textValue = this.textBox1.Text;
            if (!string.IsNullOrWhiteSpace(textValue) && textValue != this.textBox1.OldValue)
            {
                if (!myUtility.Seek(textValue, "LocalSupp", "ID"))
                {
                    MessageBox.Show(string.Format("< Subcon Code: {0} > not found!!!", textValue));
                    this.textBox1.Text = "";
                    e.Cancel = true;
                    return;
                }
                else
                {
                    if (!this.IsIncludeJunk)
                    {
                        string lookupresult = myUtility.Lookup("Junk", this.textBox1.Text.ToString(), "LocalSupp", "ID");
                        if (lookupresult == "True")
                        {
                            MessageBox.Show(string.Format("< Subcon Code: {0} > not found!!!", textValue));
                            this.textBox1.Text = "";
                            e.Cancel = true;
                            return;
                        }
                    }
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            this.displayBox1.Text = myUtility.Lookup("Abb", this.textBox1.Text.ToString(), "LocalSupp", "ID");
        }

        private void textBox1_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string selectCommand;
            selectCommand = "select ID,Abb,Name from LocalSupp order by ID";
            if (!IsIncludeJunk)
            {
                selectCommand = "select ID,Abb,Name from LocalSupp where  Junk =  0 order by ID";
            }
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(selectCommand, "9,13,60", this.Text, false, ",");
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel) { return; }
            this.textBox1.Text = item.GetSelectedString();
        }
    }
}
