using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Sci.Win.UI;

namespace Sci.Production.Class
{
    /// <summary>
    /// Txtcountry
    /// </summary>
    public partial class Txtcountry : _UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Txtcountry"/> class.
        /// </summary>
        public Txtcountry()
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

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            this.DisplayBox1.Text = MyUtility.GetValue.Lookup("Alias", this.TextBox1.Text.ToString(), "Country", "Id");

            // this.DataBindings.Cast<Binding>().ToList().ForEach(binding => binding.WriteValue());
        }

        private void TextBox1_Validating(object sender, CancelEventArgs e)
        {
          // base.OnValidating(e);
            string textValue = this.TextBox1.Text;
            if (!string.IsNullOrWhiteSpace(textValue) && textValue != this.TextBox1.OldValue)
            {
                if (!MyUtility.Check.Seek(textValue, "Country", "ID"))
                {
                    this.TextBox1.Text = string.Empty;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("< Country: {0} > not found!!!", textValue));
                }
            }

            this.DataBindings.Cast<Binding>().ToList().ForEach(binding => binding.WriteValue());
        }

        private void TextBox1_PopUp(object sender, TextBoxPopUpEventArgs e)
        {
            Win.Tools.SelectItem item = new Win.Tools.SelectItem("select ID,Alias from Country WITH (NOLOCK) where Junk = 0 order by ID", "4,30", this.TextBox1.Text);
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            this.TextBox1.Text = item.GetSelectedString();
            this.DataBindings.Cast<Binding>().ToList().ForEach(binding => binding.WriteValue());
        }
    }
}
