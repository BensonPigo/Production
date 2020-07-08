using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using Sci.Data;
using System.Data.SqlClient;

namespace Sci.Production.Class
{
    public partial class txttpeuser_canedit : Win.UI._UserControl
    {
        public txttpeuser_canedit()
        {
            this.InitializeComponent();
        }

        public Win.UI.TextBox TextBox1
        {
            get { return this.textBox1; }
        }

        public Win.UI.DisplayBox DisplayBox1
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
                            selectCommand = string.Format("select ID, Name, ExtNo from TPEPass1 WITH (NOLOCK) where Name = '{0}' order by ID", textValue.Trim());
                            DBProxy.Current.Select(null, selectCommand, out selectTable);
                            Win.Tools.SelectItem item = new Win.Tools.SelectItem(selectTable, "ID,Name,ExtNo", "15,30,10", this.textBox1.Text);
                            DialogResult returnResult = item.ShowDialog();
                            if (returnResult == DialogResult.Cancel)
                            {
                                this.textBox1.Text = string.Empty;
                                return;
                            }

                            this.textBox1.Text = item.GetSelectedString();
                        }
                        else
                        {
                            selectCommand = string.Format("select ID, Name, ExtNo from TPEPass1 WITH (NOLOCK) where Ext_No = '{0}' order by ID", textValue.Trim());
                            DBProxy.Current.Select(null, selectCommand, out selectTable);
                            Win.Tools.SelectItem item = new Win.Tools.SelectItem(selectTable, "ID,Name,ExtNo", "15,30,10", this.textBox1.Text);
                            DialogResult returnResult = item.ShowDialog();
                            if (returnResult == DialogResult.Cancel)
                            {
                                this.textBox1.Text = string.Empty;
                                return;
                            }

                            this.textBox1.Text = item.GetSelectedString();
                        }
                    }
                    else
                    {
                        this.textBox1.Text = string.Empty;
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox(string.Format("< User Id: {0} > not found!!!", textValue));
                        return;
                    }
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Win.Forms.Base myForm = (Win.Forms.Base)this.FindForm();
            if (myForm.EditMode == true)
            {
                string selectSql = string.Format("Select Name from TPEPass1 WITH (NOLOCK) where id = '{0}'", this.textBox1.Text.ToString());
                string name = MyUtility.GetValue.Lookup(selectSql);
                selectSql = string.Format("Select ExtNo from TPEPass1 WITH (NOLOCK) where id = '{0}'", this.textBox1.Text.ToString());
                string extNo = MyUtility.GetValue.Lookup(selectSql);
                if (!string.IsNullOrWhiteSpace(name))
                {
                    this.displayBox1.Text = name;
                }
                else
                {
                    this.displayBox1.Text = string.Empty;
                }

                if (!string.IsNullOrWhiteSpace(extNo))
                {
                    this.displayBox1.Text = name + " #" + extNo;
                }
            }
        }

        private void textBox1_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Win.Forms.Base myForm = (Win.Forms.Base)this.FindForm();
            if (myForm.EditMode == false || this.textBox1.ReadOnly == true)
            {
                return;
            }

            Win.Tools.SelectItem item = new Win.Tools.SelectItem("select ID,Name,ExtNo from TPEPass1 WITH (NOLOCK) order by ID", "15,30,10", this.textBox1.Text);
            item.Width = 640;
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            this.textBox1.Text = item.GetSelectedString();
        }

        private void textBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            string sql;
            List<SqlParameter> sqlpar = new List<SqlParameter>();

            sql = @"select 	ID, 
		                    Name, 
		                    Ext= ExtNo, 
		                    Mail = email
                    from Production.dbo.TPEPass1 WITH (NOLOCK) 
                    where id = @id";
            sqlpar.Add(new SqlParameter("@id", this.textBox1.Text.ToString()));

            userData ud = new userData(sql, sqlpar);

            if (ud.errMsg == null)
            {
                ud.ShowDialog();
            }
            else
            {
                MyUtility.Msg.ErrorBox(ud.errMsg);
            }
        }
    }
}
