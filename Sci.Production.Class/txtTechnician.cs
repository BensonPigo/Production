using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Sci.Data;
using System.Data.SqlClient;

namespace Sci.Production.Class
{
    /// <summary>
    /// TxtTechnician
    /// </summary>
    public partial class TxtTechnician : Win.UI._UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TxtTechnician"/> class.
        /// </summary>
        public TxtTechnician()
        {
            this.InitializeComponent();
        }

        private string myUsername = null;

        /// <summary>
        /// Technician Where
        /// </summary>
        public string CheckColumn { get; set; }

        /// <inheritdoc/>
        public Win.UI.TextBox TextBox1 { get; private set; }

        /// <inheritdoc/>
        public Win.UI.DisplayBox DisplayBox1 { get; private set; }

        private string TechnicianWhere()
        {
            string result = MyUtility.Check.Empty(this.CheckColumn) ? "ID in (select id from Technician WITH (NOLOCK) where junk = 0 )" : $"ID in (select id from Technician WITH (NOLOCK) where junk = 0 and {this.CheckColumn} = 1)";
            return result;
        }

        /// <inheritdoc/>
        public string Textbox1_text
        {
            set
            {
                this.TextBox1.Text = value;
                Commons.UserPrg.GetName(this.TextBox1.Text, out this.myUsername, Commons.UserPrg.NameType.NameAndExt);
                this.DisplayBox1.Text = this.myUsername;
            }
        }

        /// <inheritdoc/>
        [Bindable(true)]
        public string TextBox1Binding
        {
            get
            {
                return this.TextBox1.Text;
            }

            set
            {
                this.TextBox1.Text = value;
                if (!Env.DesignTime)
                {
                    // if (this.textBox1.Text == "" || MyUtility.Check.Empty(this.textBox1.Text))
                    // {
                    //    return;
                    // }
                    Commons.UserPrg.GetName(this.TextBox1.Text, out this.myUsername, Commons.UserPrg.NameType.NameAndExt);
                    this.DisplayBox1.Text = this.myUsername;
                }
            }
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
                // if (!MyUtility.Check.Seek(textValue, "Pass1", "ID"))
                if (!MyUtility.Check.Seek($"select 1 from pass1 where id = '{textValue}' and {this.TechnicianWhere()} "))
                    {
                    string alltrimData = textValue.Trim();
                    bool isUserExtNo = MyUtility.Check.Seek(alltrimData, "Pass1", "ExtNo");
                    DataTable dtName;
                    string selectCommand = string.Format($"select ID, Name, ExtNo, REPLACE(Factory,' ','') Factory from Pass1 WITH (NOLOCK) where Name like '%{0}%' and {this.TechnicianWhere()} order by ID", textValue.Trim());
                    var resultName = DBProxy.Current.Select(null, selectCommand, out dtName);

                    // if (isUserName | isUserExtNo)
                    if ((resultName && dtName.Rows.Count > 0) | isUserExtNo)
                    {
                        DataTable selectTable;
                        if (dtName.Rows.Count > 0)
                        {
                            Win.Tools.SelectItem item = new Win.Tools.SelectItem(dtName, "ID,Name,ExtNo,Factory", "10,22,5,40", this.TextBox1.Text)
                            {
                                Size = new System.Drawing.Size(828, 509),
                            };
                            DialogResult returnResult = item.ShowDialog();
                            if (returnResult == DialogResult.Cancel)
                            {
                                this.TextBox1.Text = string.Empty;
                                this.DataBindings.Cast<Binding>().ToList().ForEach(binding => binding.WriteValue());
                                return;
                            }

                            this.TextBox1.Text = item.GetSelectedString();
                        }
                        else
                        {
                            selectCommand = string.Format("select ID, Name, ExtNo, REPLACE(Factory,' ','') Factory from Pass1 WITH (NOLOCK) where ExtNo = '{0}' order by ID", textValue.Trim());
                            DBProxy.Current.Select(null, selectCommand, out selectTable);
                            Win.Tools.SelectItem item = new Win.Tools.SelectItem(selectTable, "ID,Name,ExtNo,Factory", "10,22,5,40", this.TextBox1.Text)
                            {
                                Size = new System.Drawing.Size(828, 509),
                            };
                            DialogResult returnResult = item.ShowDialog();
                            if (returnResult == DialogResult.Cancel)
                            {
                                this.TextBox1.Text = string.Empty;
                                this.DataBindings.Cast<Binding>().ToList().ForEach(binding => binding.WriteValue());
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
                        this.DataBindings.Cast<Binding>().ToList().ForEach(binding => binding.WriteValue());
                        this.DisplayBox1.Text = string.Empty;
                        return;
                    }
                }
            }

            // 強制把binding的Text寫到DataRow
            this.DataBindings.Cast<Binding>().ToList().ForEach(binding => binding.WriteValue());

            Commons.UserPrg.GetName(this.TextBox1.Text, out this.myUsername, Commons.UserPrg.NameType.NameAndExt);
            this.DisplayBox1.Text = this.myUsername;
        }

        private void TextBox1_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Win.Forms.Base myForm = (Win.Forms.Base)this.FindForm();
            if (myForm.EditMode == false || this.TextBox1.ReadOnly == true)
            {
                return;
            }

            Win.Tools.SelectItem item = new Win.Tools.SelectItem($"select ID, Name, ExtNo, replace(Factory,' ','')factory from Pass1 WITH (NOLOCK) where Resign is null  and {this.TechnicianWhere()}  order by ID", "10,22,5,40", this.TextBox1.Text)
            {
                Size = new System.Drawing.Size(828, 509),
            };
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            this.TextBox1.Text = item.GetSelectedString();

            Commons.UserPrg.GetName(this.TextBox1.Text, out this.myUsername, Commons.UserPrg.NameType.NameAndExt);
            this.DisplayBox1.Text = this.myUsername;
        }

        private void TextBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            string sql;
            List<SqlParameter> sqlpar = new List<SqlParameter>();

            sql = @"select 	ID, 
		                    Name, 
		                    Ext= ExtNo, 
		                    Mail = email
                    from Pass1 WITH (NOLOCK) 
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
