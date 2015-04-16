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
    public partial class txtsubcon : UserControl
    {
        public txtsubcon()
        {
            InitializeComponent();
        }

        private bool _IsIncludeJunk;

        [Category("Custom Properties")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public bool IsIncludeJunk
        {
            set { this._IsIncludeJunk = value; }
            get { return this._IsIncludeJunk; }
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
                if (!myUtility.Seek(str, "LocalSupp", "ID"))
                {
                    MessageBox.Show(string.Format("< Subcon Code: {0} > not found!!!", str));
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
                            MessageBox.Show(string.Format("< Subcon Code: {0} > not found!!!", str));
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
            string sqlwhere = "Where 1=1";
            string sqlcmd = string.Empty;

            if (!IsIncludeJunk)
            {
                if (!this.IsIncludeJunk)
                { sqlwhere = sqlwhere + " And Junk =  0 "; }
            };

            sqlcmd = "select ID,Abb,Name from LocalSupp " + sqlwhere + " order by ID";
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem("LocalSupp.Id,Abb,Name", "15,30,100", this.textBox1.Text);
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel) { return; }
            this.textBox1.Text = item.GetSelectedString();
        }
    }
}
