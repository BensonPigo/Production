using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Sci.Data;

namespace Sci.Production.Class
{
    /// <summary>
    /// Txtmulituser
    /// </summary>
    public partial class Txtmulituser : Win.UI._UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Txtmulituser"/> class.
        /// </summary>
        public Txtmulituser()
        {
            this.InitializeComponent();
        }

        private string myUsername = null;
        private string mulitUsername = null;

        /// <inheritdoc/>
        public Win.UI.TextBox TextBox1 { get; private set; }

        /// <inheritdoc/>
        public Win.UI.DisplayBox DisplayBox1 { get; private set; }

        /// <summary>
        /// 多筆User 用[,]分隔
        /// </summary>
        public string Textbox1_text
        {
            set
            {
                this.TextBox1.Text = value;

                this.mulitUsername = string.Empty;
                string[] getUserId = this.TextBox1.Text.Split(',').Distinct().ToArray();
                foreach (var userID in getUserId)
                {
                    Commons.UserPrg.GetName(userID, out this.myUsername, Commons.UserPrg.NameType.NameAndExt);
                    this.mulitUsername += "," + this.myUsername;
                }

                this.DisplayBox1.Text = (this.mulitUsername.Substring(0, 1) == ",") ? this.DisplayBox1.Text = this.mulitUsername.Substring(1) : this.DisplayBox1.Text = this.mulitUsername.Substring(1);
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
                    this.mulitUsername = string.Empty;
                    string[] getUserId = this.TextBox1.Text.Split(',').Distinct().ToArray();
                    foreach (var userID in getUserId)
                    {
                        Commons.UserPrg.GetName(userID, out this.myUsername, Commons.UserPrg.NameType.NameAndExt);
                        this.mulitUsername += "," + this.myUsername;
                    }

                    this.DisplayBox1.Text = (this.mulitUsername.Substring(0, 1) == ",") ? this.DisplayBox1.Text = this.mulitUsername.Substring(1) : this.DisplayBox1.Text = this.mulitUsername.Substring(1);
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
            this.mulitUsername = string.Empty;
            if (MyUtility.Check.Empty(textValue))
            {
                return;
            }

            if (!string.IsNullOrWhiteSpace(textValue) && textValue != this.TextBox1.OldValue)
            {
                string[] getUserId = textValue.Split(',').Distinct().ToArray();
                foreach (var userID in getUserId)
                {
                    if (!MyUtility.Check.Seek(userID, "Pass1", "ID"))
                    {
                        string alltrimData = userID.Trim();
                        bool isUserExtNo = MyUtility.Check.Seek(alltrimData, "Pass1", "ExtNo");
                        DataTable dtName;
                        string selectCommand = string.Format("select ID, Name, ExtNo, REPLACE(Factory,' ','') Factory from Pass1 WITH (NOLOCK) where Name like '%{0}%' order by ID", userID.Trim());
                        var resultName = DBProxy.Current.Select(null, selectCommand, out dtName);

                        // if (isUserName | isUserExtNo)
                        if ((resultName && dtName.Rows.Count > 0) | isUserExtNo)
                        {
                            DataTable selectTable;
                            if (dtName.Rows.Count > 0)
                            {
                                Win.Tools.SelectItem item = new Win.Tools.SelectItem(dtName, "ID,Name,ExtNo,Factory", "10,22,5,40", this.TextBox1.Text);
                                item.Size = new System.Drawing.Size(828, 509);
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
                                selectCommand = string.Format("select ID, Name, ExtNo, REPLACE(Factory,' ','') Factory from Pass1 WITH (NOLOCK) where ExtNo = '{0}' order by ID", userID.Trim());
                                DBProxy.Current.Select(null, selectCommand, out selectTable);
                                Win.Tools.SelectItem item = new Win.Tools.SelectItem(selectTable, "ID,Name,ExtNo,Factory", "10,22,5,40", this.TextBox1.Text);
                                item.Size = new System.Drawing.Size(828, 509);
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
                            MyUtility.Msg.WarningBox(string.Format("< User Id: {0} > not found!!!", userID));
                            this.DataBindings.Cast<Binding>().ToList().ForEach(binding => binding.WriteValue());
                            this.DisplayBox1.Text = string.Empty;
                            return;
                        }
                    }

                    Commons.UserPrg.GetName(userID, out this.myUsername, Commons.UserPrg.NameType.NameAndExt);
                    this.mulitUsername += "," + this.myUsername;
                }
            }

            // 強制把binding的Text寫到DataRow
            this.DataBindings.Cast<Binding>().ToList().ForEach(binding => binding.WriteValue());
            this.DisplayBox1.Text = (this.mulitUsername.Substring(0, 1) == ",") ? this.DisplayBox1.Text = this.mulitUsername.Substring(1) : this.DisplayBox1.Text = this.mulitUsername.Substring(1);
        }

        private void TextBox1_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Win.Forms.Base myForm = (Win.Forms.Base)this.FindForm();
            if (myForm.EditMode == false || this.TextBox1.ReadOnly == true)
            {
                return;
            }

            Win.Tools.SelectItem2 item = new Win.Tools.SelectItem2("select ID, Name, ExtNo, replace(Factory,' ','')factory from Pass1 WITH (NOLOCK) where Resign is null order by ID", string.Empty, this.TextBox1.Text);
            item.Size = new System.Drawing.Size(828, 509);
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            this.TextBox1.Text = item.GetSelectedString();

            this.mulitUsername = string.Empty;
            string[] getUserId = item.GetSelectedString().Split(',').Distinct().ToArray();
            foreach (var userID in getUserId)
            {
                Commons.UserPrg.GetName(userID, out this.myUsername, Commons.UserPrg.NameType.NameAndExt);
                this.mulitUsername += "," + this.myUsername;
            }

            this.DisplayBox1.Text = (this.mulitUsername.Substring(0, 1) == ",") ? this.DisplayBox1.Text = this.mulitUsername.Substring(1) : this.DisplayBox1.Text = this.mulitUsername.Substring(1);
        }
    }
}
