using System;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using Sci.Data;
using Ict;

namespace Sci.Production.Class
{
    /// <summary>
    /// TxtLocalTPESuppNoConfirm
    /// </summary>
    public partial class TxtLocalTPESuppNoConfirm : Win.UI._UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TxtLocalTPESuppNoConfirm"/> class.
        /// </summary>
        public TxtLocalTPESuppNoConfirm()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Is Include Junk
        /// </summary>
        [Category("Custom Properties")]
        public bool IsIncludeJunk { get; set; }

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
            string textValue = this.TextBox1.Text.Trim();
            if (textValue == this.TextBox1.OldValue)
            {
                return;
            }

            if (!string.IsNullOrWhiteSpace(textValue))
            {
                string sql = string.Format("Select Junk from LocalSupp WITH (NOLOCK) where ID = '{0}'  union all select Junk from Supp WITH (NOLOCK) where ID = '{0}' ", textValue);
                if (!MyUtility.Check.Seek(sql, "Production"))
                {
                    this.TextBox1.Text = string.Empty;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("< Supplier: {0} > not found!!!", textValue));
                    return;
                }
                else
                {
                    if (!this.IsIncludeJunk)
                    {
                        string lookupresult = MyUtility.GetValue.Lookup(sql, "Production");
                        if (lookupresult == "True")
                        {
                            this.TextBox1.Text = string.Empty;
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox(string.Format("< Supplier: {0} > not found!!!", textValue));
                            return;
                        }
                    }

                    string sql_cmd = $"select Abb from LocalSupp WITH (NOLOCK) where  Junk =  0  and ID = '{this.TextBox1.Text.ToString()}'   union all select [Abb] = AbbEN from Supp WITH (NOLOCK) where  Junk =  0 and ID = '{this.TextBox1.Text.ToString()}' ";
                    this.DisplayBox1.Text = MyUtility.GetValue.Lookup(sql_cmd, "Production");
                }
            }

            this.ValidateControl();
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            string sql_cmd = $"select Abb from LocalSupp WITH (NOLOCK) where  Junk =  0  and ID = '{this.TextBox1.Text.ToString()}'   union all select [Abb] = AbbEN from Supp WITH (NOLOCK) where  Junk =  0 and ID = '{this.TextBox1.Text.ToString()}' ";
            this.DisplayBox1.Text = MyUtility.GetValue.Lookup(sql_cmd, "Production");
        }

        private void TextBox1_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Win.Forms.Base myForm = (Win.Forms.Base)this.FindForm();
            if (myForm.EditMode == false || this.TextBox1.ReadOnly == true)
            {
                return;
            }

            string selectCommand;
            selectCommand = "select ID,Abb,Name from LocalSupp WITH (NOLOCK)  union all select ID,[Name] = NameEN,[Abb] = AbbEN from Supp WITH (NOLOCK) order by ID";
            if (!this.IsIncludeJunk)
            {
                selectCommand = "select ID,Abb,Name from LocalSupp WITH (NOLOCK) where  Junk =  0  union all select ID,[Name] = NameEN,[Abb] = AbbEN from Supp WITH (NOLOCK) where  Junk =  0  order by ID";
            }

            DataTable tbSelect;
            DBProxy.Current.Select("Production", selectCommand, out tbSelect);
            Win.Tools.SelectItem item = new Win.Tools.SelectItem(tbSelect, "ID,Abb,Name", "9,13,40", this.Text, false, ",", "ID,Abb,Name");
            item.Size = new System.Drawing.Size(690, 555);
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            this.TextBox1.Text = item.GetSelectedString();
            this.TextBox1.ValidateControl();
            string sql_cmd = $"select Abb from LocalSupp WITH (NOLOCK) where  Junk =  0  and ID = '{this.TextBox1.Text.ToString()}'   union all select [Abb] = AbbEN from Supp WITH (NOLOCK) where  Junk =  0 and ID = '{this.TextBox1.Text.ToString()}' ";
            this.DisplayBox1.Text = MyUtility.GetValue.Lookup(sql_cmd, "Production");
        }
    }
}
