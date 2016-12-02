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
    public partial class txtuser : Sci.Win.UI._UserControl
    {
        public txtuser()
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
                if (!MyUtility.Check.Seek(textValue, "Pass1", "ID"))
                {
                    string alltrimData = textValue.Trim();
                    bool isUserName = MyUtility.Check.Seek(alltrimData, "Pass1", "Name" );
                    bool isUserExtNo = MyUtility.Check.Seek(alltrimData, "Pass1", "ExtNo");

                    if (isUserName | isUserExtNo)
                    {
                        string selectCommand;
                        DataTable selectTable;
                        if (isUserName)
                        {
                            selectCommand = string.Format("select ID, Name, ExtNo, Factory from Pass1 where Name = '{0}' order by ID", textValue.Trim());
                            DBProxy.Current.Select(null, selectCommand, out selectTable);
                            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(selectTable,"ID,Name,ExtNo,Factory", "15,30,10,150", this.textBox1.Text);
                            DialogResult returnResult = item.ShowDialog();
                            if (returnResult == DialogResult.Cancel) 
                            {
                                this.textBox1.Text = "";
                                this.DataBindings.Cast<Binding>().ToList().ForEach(binding => binding.WriteValue());
                                return; 
                            }
                            this.textBox1.Text = item.GetSelectedString();
                        }
                        else
                        {
                            selectCommand = string.Format("select ID, Name, ExtNo, Factory from Pass1 where Ext_No = '{0}' order by ID", textValue.Trim());
                            DBProxy.Current.Select(null, selectCommand, out selectTable);
                            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(selectTable, "ID,Name,ExtNo,Factory", "15,30,10,150", this.textBox1.Text);
                            DialogResult returnResult = item.ShowDialog();
                            if (returnResult == DialogResult.Cancel)
                            {
                                this.textBox1.Text = "";
                                this.DataBindings.Cast<Binding>().ToList().ForEach(binding => binding.WriteValue());
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
                        this.DataBindings.Cast<Binding>().ToList().ForEach(binding => binding.WriteValue());
                        return;
                    }
                }
            }

            // 強制把binding的Text寫到DataRow
            this.DataBindings.Cast<Binding>().ToList().ForEach(binding => binding.WriteValue());
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //Sci.Win.Forms.Base myForm = (Sci.Win.Forms.Base)this.FindForm();
            //if (myForm.EditMode == false)
            //{
            string selectSql = string.Format("Select Name from Pass1 where id = '{0}'", this.textBox1.Text.ToString());
            string name = MyUtility.GetValue.Lookup(selectSql);
            selectSql = string.Format("Select ExtNo from Pass1 where id = '{0}'", this.textBox1.Text.ToString());
            string extNo = MyUtility.GetValue.Lookup(selectSql);
            if (!string.IsNullOrWhiteSpace(extNo) || !string.IsNullOrWhiteSpace(name))
            {
                this.displayBox1.Text = name + " #" + extNo;
            }
            else
            {
                this.displayBox1.Text = "";
            }
            if (string.IsNullOrWhiteSpace(name))
            {
                 this.textBox1.Text = "";
            }
           
           
            ////}
        }

        private void textBox1_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Sci.Win.Forms.Base myForm = (Sci.Win.Forms.Base)this.FindForm();
            if (myForm.EditMode == false || textBox1.ReadOnly == true) return;
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem("select ID,Name,ExtNo,Factory from Pass1 where Resign is null order by ID", "15,30,10,150", this.textBox1.Text);
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel) { return; }
            this.textBox1.Text = item.GetSelectedString();
            this.displayBox1.Text = item.GetSelecteds()[0]["EXTNO"].ToString().TrimEnd();
        }
    }
}
