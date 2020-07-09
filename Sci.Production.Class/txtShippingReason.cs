using System;
using System.ComponentModel;
using System.Windows.Forms;
using Ict;

namespace Sci.Production.Class
{
    /// <summary>
    /// TxtShippingReason
    /// </summary>
    public partial class TxtShippingReason : Win.UI._UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TxtShippingReason"/> class.
        /// </summary>
        public TxtShippingReason()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 填入Reason Type。例如：AQ
        /// </summary>
        [Category("Custom Properties")]
        [Description("填入Reason Type。例如：AQ")]
        public string Type { get; set; }

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
                string sql = string.Format("Select 1 from ShippingReason WITH (NOLOCK) where ID='{0}' and Type='{1}'", textValue, this.Type);
                if (!MyUtility.Check.Seek(sql, "Production"))
                {
                    this.TextBox1.Text = string.Empty;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("< Reason: {0} > not found!!!", textValue));
                    return;
                }
                else
                {
                    string sql_cmd = string.Format("Select Description from ShippingReason WITH (NOLOCK) where ID='{0}' and Type='{1}'", textValue, this.Type);
                    this.DisplayBox1.Text = MyUtility.GetValue.Lookup(sql_cmd, "Production");
                }
            }

            this.ValidateControl();
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            string sql_cmd = string.Format("Select Description from ShippingReason WITH (NOLOCK) where ID='{0}' and Type='{1}'", this.TextBox1.Text, this.Type);
            this.DisplayBox1.Text = MyUtility.GetValue.Lookup(sql_cmd, "Production");
        }

        private void TextBox1_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Win.Tools.SelectItem item = new Win.Tools.SelectItem(
                string.Format("Select Id, Description from ShippingReason WITH (NOLOCK) where type='{0}' order by id", this.Type), "10,40", this.TextBox1.Text);
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.TextBox1.Text = item.GetSelectedString();
            this.Validate();
        }
    }
}
