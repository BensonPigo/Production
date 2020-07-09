using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Sci.Data;

namespace Sci.Production.Class
{
    /// <summary>
    /// Txtpayterm_fty
    /// </summary>
    public partial class Txtpayterm_fty : Win.UI._UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Txtpayterm_fty"/> class.
        /// </summary>
        public Txtpayterm_fty()
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
            string textValue = this.TextBox1.Text;
            if (!string.IsNullOrWhiteSpace(textValue) && textValue != this.TextBox1.OldValue)
            {
                if (!MyUtility.Check.Seek(textValue, "PayTerm", "ID", "Production"))
                {
                    this.TextBox1.Text = string.Empty;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("< Pay Term: {0} > not found!!!", textValue));
                }
            }

            this.DataBindings.Cast<Binding>().ToList().ForEach(binding => binding.WriteValue());
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            this.DisplayBox1.Text = MyUtility.GetValue.Lookup("Name", this.TextBox1.Text.ToString(), "PayTerm", "ID", "Production");
        }

        private void TextBox1_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Win.Forms.Base myForm = (Win.Forms.Base)this.FindForm();
            if (myForm.EditMode == false || this.TextBox1.ReadOnly == true)
            {
                return;
            }

            string selItem = "select ID,Name from PayTerm WITH (NOLOCK) where Junk = 0 order by ID";
            DataTable itemDt;
            DBProxy.Current.Select("Production", selItem, out itemDt);
            Win.Tools.SelectItem item = new Win.Tools.SelectItem(itemDt, "ID,Name", "6,40", this.TextBox1.Text);
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            this.TextBox1.Text = item.GetSelectedString();
        }
    }
}
