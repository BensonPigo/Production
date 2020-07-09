using System;
using System.ComponentModel;
using System.Windows.Forms;
using Sci.Win.UI;

namespace Sci.Production.Class
{
    /// <summary>
    /// Txtpaytermar
    /// </summary>
    public partial class Txtpaytermar : _UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Txtpaytermar"/> class.
        /// </summary>
        public Txtpaytermar()
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        public Win.UI.TextBox TextBox1 { get; private set; }

        /// <inheritdoc/>
        public DisplayBox DisplayBox1 { get; private set; }

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
            string textValue = this.TextBox1.Text;
            if (!string.IsNullOrWhiteSpace(textValue) && textValue != this.TextBox1.OldValue)
            {
                if (!MyUtility.Check.Seek(textValue, "PayTermAR", "ID"))
                {
                    this.TextBox1.Text = string.Empty;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("< Pay Term: {0} > not found!!!", textValue));
                    return;
                }
            }
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            this.DisplayBox1.Text = MyUtility.GetValue.Lookup("Description", this.TextBox1.Text.ToString(), "PayTermAR", "ID");
        }

        private void TextBox1_PopUp(object sender, TextBoxPopUpEventArgs e)
        {
            Win.Forms.Base myForm = (Win.Forms.Base)this.FindForm();
            if (myForm.EditMode == false || this.TextBox1.ReadOnly == true)
            {
                return;
            }

            Win.Tools.SelectItem item = new Win.Tools.SelectItem("select ID,Description from PayTermAR WITH (NOLOCK) order by ID", "10,100", this.TextBox1.Text);
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            this.TextBox1.Text = item.GetSelectedString();
        }
    }
}
