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
    public partial class txttpeuser_canedit : Sci.Win.UI._UserControl
    {
        public txttpeuser_canedit()
        {
            InitializeComponent();
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
           // base.OnValidating(e);
            string textValue = this.textBox1.Text;
            if (!string.IsNullOrWhiteSpace(textValue) && textValue != this.textBox1.OldValue)
            {
                if (!MyUtility.Check.Seek(textValue, "TPEPass1", "ID"))
                {
                    string alltrimData = textValue.Trim();
                    bool isUserName = MyUtility.Check.Seek(alltrimData, "TPEPass1", "Name");
                    bool isUserExtNo = MyUtility.Check.Seek(alltrimData, "TPEPass1", "ExtNo");

                    if (isUserName | isUserExtNo)
                    {
                        string selectCommand;
                        DataTable selectTable;
                        if (isUserName)
                        {
                            selectCommand = string.Format("select ID, Name, ExtNo from TPEPass1 where Name = '{0}' order by ID", textValue.Trim());
                            DBProxy.Current.Select(null, selectCommand, out selectTable);
                            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(selectTable,"ID,Name,ExtNo", "15,30,10", this.textBox1.Text);
                            DialogResult returnResult = item.ShowDialog();
                            if (returnResult == DialogResult.Cancel) 
                            {
                                this.textBox1.Text = "";
                                return; 
                            }
                            this.textBox1.Text = item.GetSelectedString();
                        }
                        else
                        {
                            selectCommand = string.Format("select ID, Name, ExtNo from TPEPass1 where Ext_No = '{0}' order by ID", textValue.Trim());
                            DBProxy.Current.Select(null, selectCommand, out selectTable);
                            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(selectTable, "ID,Name,ExtNo", "15,30,10", this.textBox1.Text);
                            DialogResult returnResult = item.ShowDialog();
                            if (returnResult == DialogResult.Cancel)
                            {
                                this.textBox1.Text = "";
                                return;
                            }
                            this.textBox1.Text = item.GetSelectedString();
                        }
                    }
                    else
                    {
                        MyUtility.Msg.WarningBox(string.Format("< User Id: {0} > not found!!!", textValue));
                        this.textBox1.Text = "";
                        e.Cancel = true;
                        return;
                    }
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Sci.Win.Forms.Base myForm = (Sci.Win.Forms.Base)this.FindForm();
            if (myForm.EditMode == false)
            {
                string selectSql = string.Format("Select Name from TPEPass1 where id = '{0}'", this.textBox1.Text.ToString());
                string name = MyUtility.GetValue.Lookup(selectSql);
                selectSql = string.Format("Select ExtNo from TPEPass1 where id = '{0}'", this.textBox1.Text.ToString());
                string extNo = MyUtility.GetValue.Lookup(selectSql);
                if (!string.IsNullOrWhiteSpace(name)) { this.displayBox1.Text = name; }
                else this.displayBox1.Text = "";
                if (!string.IsNullOrWhiteSpace(extNo)) { this.displayBox1.Text = name + " #" + extNo; }
            }
        }

        private void textBox1_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Sci.Win.Forms.Base myForm = (Sci.Win.Forms.Base)this.FindForm();
            if (myForm.EditMode == false || textBox1.ReadOnly == true) return;
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem("select ID,Name,ExtNo from TPEPass1 order by ID", "15,30,10", this.textBox1.Text);
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel) { return; }
            this.textBox1.Text = item.GetSelectedString();
        }
    }
}
