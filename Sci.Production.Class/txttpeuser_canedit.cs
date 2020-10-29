using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using Sci.Data;
using System.Data.SqlClient;

namespace Sci.Production.Class
{
    /// <summary>
    /// Txttpeuser_canedit
    /// </summary>
    public partial class Txttpeuser_canedit : Win.UI._UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Txttpeuser_canedit"/> class.
        /// </summary>
        public Txttpeuser_canedit()
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        public Win.UI.TextBox TextBox1 { get; private set; }

        /// <inheritdoc/>
        public Win.UI.DisplayBox DisplayBox1 { get; private set; }

        /// <inheritdoc/>
        [Bindable(true)]
        public string TextBox1Binding
        {
            get { return this.TextBox1.Text; }
            set { this.TextBox1.Text = value; }
        }

        /// <inheritdoc/>
        [Bindable(true)]
        public string DisplayBox1Binding
        {
            get { return this.DisplayBox1.Text; }
            set { this.DisplayBox1.Text = value; }
        }

        private void TextBox1_Validating(object sender, CancelEventArgs e)
        {
           // base.OnValidating(e);
            string textValue = this.TextBox1.Text;
            if (!string.IsNullOrWhiteSpace(textValue) && textValue != this.TextBox1.OldValue)
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
                            Win.Tools.SelectItem item = new Win.Tools.SelectItem(selectTable, "ID,Name,ExtNo", "15,30,10", this.TextBox1.Text);
                            DialogResult returnResult = item.ShowDialog();
                            if (returnResult == DialogResult.Cancel)
                            {
                                this.TextBox1.Text = string.Empty;
                                return;
                            }

                            this.TextBox1.Text = item.GetSelectedString();
                        }
                        else
                        {
                            selectCommand = string.Format("select ID, Name, ExtNo from TPEPass1 WITH (NOLOCK) where Ext_No = '{0}' order by ID", textValue.Trim());
                            DBProxy.Current.Select(null, selectCommand, out selectTable);
                            Win.Tools.SelectItem item = new Win.Tools.SelectItem(selectTable, "ID,Name,ExtNo", "15,30,10", this.TextBox1.Text);
                            DialogResult returnResult = item.ShowDialog();
                            if (returnResult == DialogResult.Cancel)
                            {
                                this.TextBox1.Text = string.Empty;
                                return;
                            }

                            this.TextBox1.Text = item.GetSelectedString();
                        }
                    }
                    else
                    {
                        this.TextBox1.Text = string.Empty;
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox(string.Format("< User Id: {0} > not found!!!", textValue));
                        return;
                    }
                }
            }
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            Win.Forms.Base myForm = (Win.Forms.Base)this.FindForm();
            if (myForm.EditMode == true)
            {
                string selectSql = string.Format("Select Name from TPEPass1 WITH (NOLOCK) where id = '{0}'", this.TextBox1.Text.ToString());
                string name = MyUtility.GetValue.Lookup(selectSql);
                selectSql = string.Format("Select ExtNo from TPEPass1 WITH (NOLOCK) where id = '{0}'", this.TextBox1.Text.ToString());
                string extNo = MyUtility.GetValue.Lookup(selectSql);
                if (!string.IsNullOrWhiteSpace(name))
                {
                    this.DisplayBox1.Text = name;
                }
                else
                {
                    this.DisplayBox1.Text = string.Empty;
                }

                if (!string.IsNullOrWhiteSpace(extNo))
                {
                    this.DisplayBox1.Text = name + " #" + extNo;
                }
            }
        }

        private void TextBox1_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Win.Forms.Base myForm = (Win.Forms.Base)this.FindForm();
            if (myForm.EditMode == false || this.TextBox1.ReadOnly == true)
            {
                return;
            }

            Win.Tools.SelectItem item = new Win.Tools.SelectItem("select ID,Name,ExtNo from TPEPass1 WITH (NOLOCK) order by ID", "15,30,10", this.TextBox1.Text)
            {
                Width = 640,
            };
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            this.TextBox1.Text = item.GetSelectedString();
        }

        private void TextBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            string sql;
            List<SqlParameter> sqlpar = new List<SqlParameter>();

            sql = @"select 	ID, 
		                    Name, 
		                    Ext= ExtNo, 
		                    Mail = email
                    from Production.dbo.TPEPass1 WITH (NOLOCK) 
                    where id = @id";
            sqlpar.Add(new SqlParameter("@id", this.TextBox1.Text.ToString()));

            UserData ud = new UserData(sql, sqlpar);

            if (ud.ErrMsg == null)
            {
                ud.ShowDialog();
            }
            else
            {
                MyUtility.Msg.ErrorBox(ud.ErrMsg);
            }
        }
    }
}
