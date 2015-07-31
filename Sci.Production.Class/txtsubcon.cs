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
using Sci.Win;

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
        public bool IsIncludeJunk
        {
            set { this.isIncludeJunk = value; }
            get { return this.isIncludeJunk; }
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
            base.OnValidating(e);
            string textValue = this.textBox1.Text;
            if (!string.IsNullOrWhiteSpace(textValue) && textValue != this.textBox1.OldValue)
            {
                string Sql = string.Format("Select Junk from LocalSupp where ID = '{0}'", textValue);
                if (!MyUtility.Check.Seek(Sql, "Production"))
                {
                    MyUtility.Msg.WarningBox(string.Format("< Subcon Code: {0} > not found!!!", textValue));
                    this.textBox1.Text = "";
                    e.Cancel = true;
                    return;
                }
                else
                {
                    if (!this.IsIncludeJunk)
                    {
                        string lookupresult = MyUtility.GetValue.Lookup(Sql, "Production");
                        if (lookupresult == "True")
                        {
                            MyUtility.Msg.WarningBox(string.Format("< Subcon Code: {0} > not found!!!", textValue));
                            this.textBox1.Text = "";
                            e.Cancel = true;
                            return;
                        }
                    }
                    this.displayBox1.Text = MyUtility.GetValue.Lookup("Abb", this.textBox1.Text.ToString(), "LocalSupp", "ID", "Production");
                }
            }
            this.ValidateControl();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
           
            Sci.Win.Forms.Base myForm = (Sci.Win.Forms.Base) this.FindForm();
            if (myForm.EditMode == false)
            {
                this.displayBox1.Text = MyUtility.GetValue.Lookup("Abb", this.textBox1.Text.ToString(), "LocalSupp", "ID", "Production");
            }
        }

        private void textBox1_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Sci.Win.Forms.Base myForm = (Sci.Win.Forms.Base) this.FindForm();
            if (myForm.EditMode == false || textBox1.ReadOnly==true) return;
            string selectCommand;
            selectCommand = "select ID,Abb,Name from LocalSupp order by ID";
            if (!IsIncludeJunk)
            {
                selectCommand = "select ID,Abb,Name from LocalSupp where  Junk =  0 order by ID";
            }
            DataTable tbSelect;
            DBProxy.Current.Select("Production", selectCommand, out tbSelect);
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(tbSelect, "ID,Abb,Name", "9,13,60", this.Text, false, ",", "ID,Abb,Name");
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel) { return; }
            this.textBox1.Text = item.GetSelectedString();
            this.textBox1.ValidateControl();
            this.displayBox1.Text = MyUtility.GetValue.Lookup("Abb", this.textBox1.Text.ToString(), "LocalSupp", "ID", "Production");
        }
    }
}
