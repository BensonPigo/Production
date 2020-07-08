using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Sci.Data;
using System.Data.SqlClient;

namespace Sci.Production.Class
{
    public partial class txtuser : Sci.Win.UI._UserControl
    {
        public txtuser()
        {
            this.InitializeComponent();
        }

        private string myUsername = null;

        public Sci.Win.UI.TextBox TextBox1
        {
            get { return this.textBox1; }
        }

        public Sci.Win.UI.DisplayBox DisplayBox1
        {
            get { return this.displayBox1; }
        }

        public string textbox1_text
        {
            set
            {
                this.textBox1.Text = value;
                Sci.Production.Class.Commons.UserPrg.GetName(this.TextBox1.Text, out this.myUsername, Sci.Production.Class.Commons.UserPrg.NameType.nameAndExt);
                this.DisplayBox1.Text = this.myUsername;
            }
        }

        [Bindable(true)]
        public string TextBox1Binding
        {
            get { return this.textBox1.Text; }

            set
            {
                this.textBox1.Text = value;
                if (!Env.DesignTime)
                {
                    // if (this.textBox1.Text == "" || MyUtility.Check.Empty(this.textBox1.Text))
                    // {
                    //    return;
                    // }
                    Sci.Production.Class.Commons.UserPrg.GetName(this.TextBox1.Text, out this.myUsername, Sci.Production.Class.Commons.UserPrg.NameType.nameAndExt);
                    this.DisplayBox1.Text = this.myUsername;
                }
            }
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
                List<SqlParameter> sqlpara = new List<SqlParameter>();
                sqlpara.Add(new SqlParameter("@ID", textValue));

                if (!MyUtility.Check.Seek(@"select 1 from Pass1 where ID = @ID", sqlpara))
                {
                    string alltrimData = textValue.Trim();
                    bool isUserExtNo = MyUtility.Check.Seek(alltrimData, "Pass1", "ExtNo");
                    DataTable dtName;
                    string selectCommand = string.Format("select ID, Name, ExtNo, REPLACE(Factory,' ','') Factory from Pass1 WITH (NOLOCK) where Name like '%{0}%' order by ID", textValue.Trim());
                    var resultName = DBProxy.Current.Select(null, selectCommand, out dtName);

                    // if (isUserName | isUserExtNo)
                    if ((resultName && dtName.Rows.Count > 0) | isUserExtNo)
                    {
                        DataTable selectTable;
                        if (dtName.Rows.Count > 0)
                        {
                            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(dtName, "ID,Name,ExtNo,Factory", "10,22,5,40", this.textBox1.Text);
                            item.Size = new System.Drawing.Size(828, 509);
                            DialogResult returnResult = item.ShowDialog();
                            if (returnResult == DialogResult.Cancel)
                            {
                                this.textBox1.Text = string.Empty;
                                this.DataBindings.Cast<Binding>().ToList().ForEach(binding => binding.WriteValue());
                                return;
                            }

                            this.textBox1.Text = item.GetSelectedString();
                        }
                        else
                        {
                            selectCommand = string.Format("select ID, Name, ExtNo, REPLACE(Factory,' ','') Factory from Pass1 WITH (NOLOCK) where ExtNo = '{0}' order by ID", textValue.Trim());
                            DBProxy.Current.Select(null, selectCommand, out selectTable);
                            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(selectTable, "ID,Name,ExtNo,Factory", "10,22,5,40", this.textBox1.Text);
                            item.Size = new System.Drawing.Size(828, 509);
                            DialogResult returnResult = item.ShowDialog();
                            if (returnResult == DialogResult.Cancel)
                            {
                                this.textBox1.Text = string.Empty;
                                this.DataBindings.Cast<Binding>().ToList().ForEach(binding => binding.WriteValue());
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
                        this.DataBindings.Cast<Binding>().ToList().ForEach(binding => binding.WriteValue());
                        this.DisplayBox1.Text = string.Empty;
                        return;
                    }
                }
            }

            // 強制把binding的Text寫到DataRow
            this.DataBindings.Cast<Binding>().ToList().ForEach(binding => binding.WriteValue());

            Sci.Production.Class.Commons.UserPrg.GetName(this.TextBox1.Text, out this.myUsername, Sci.Production.Class.Commons.UserPrg.NameType.nameAndExt);
            this.DisplayBox1.Text = this.myUsername;
        }

        private void textBox1_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Sci.Win.Forms.Base myForm = (Sci.Win.Forms.Base)this.FindForm();
            if (myForm.EditMode == false || this.textBox1.ReadOnly == true)
            {
                return;
            }

            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem("select ID, Name, ExtNo, replace(Factory,' ','')factory from Pass1 WITH (NOLOCK) where Resign is null order by ID", "10,22,5,40", this.textBox1.Text);
            item.Size = new System.Drawing.Size(828, 509);
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            this.textBox1.Text = item.GetSelectedString();

            Sci.Production.Class.Commons.UserPrg.GetName(this.TextBox1.Text, out this.myUsername, Sci.Production.Class.Commons.UserPrg.NameType.nameAndExt);
            this.DisplayBox1.Text = this.myUsername;
        }

        private void textBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            string sql;
            List<SqlParameter> sqlpar = new List<SqlParameter>();

            sql = @"select 	ID, 
		                    Name, 
		                    Ext= ExtNo, 
		                    Mail = email
                    from Pass1 WITH (NOLOCK) 
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
