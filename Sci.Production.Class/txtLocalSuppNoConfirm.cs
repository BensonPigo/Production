using Ict;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Sci.Production.Class
{
    /// <summary>
    /// TxtLocalSuppNoConfirm
    /// </summary>
    public partial class TxtLocalSuppNoConfirm : Win.UI._UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TxtLocalSuppNoConfirm"/> class.
        /// </summary>
        public TxtLocalSuppNoConfirm()
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

        /// <inheritdoc/>
        public Win.UI.TextBox TextBox1 { get; private set; }

        /// <inheritdoc/>
        public Win.UI.DisplayBox DisplayBox1 { get; set; }

        /// <inheritdoc/>
        [Bindable(true)]
        public string TextBox1Binding
        {
            get { return this.TextBox1.Text; }
            set { this.TextBox1.Text = value;  }
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
                if (!MyUtility.Check.Seek(textValue, "LocalSupp", "ID"))
                {
                    this.TextBox1.Text = string.Empty;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("< LocalSupplier Code: {0} > not found!!!", textValue));
                    return;
                }
            }

            this.ValidateControl();
        }

        private void TextBox1_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Win.Forms.Base myForm = (Win.Forms.Base)this.FindForm();
            if (myForm.EditMode == false || this.TextBox1.ReadOnly == true)
            {
                return;
            }

            Win.Tools.SelectItem item = new Win.Tools.SelectItem("select ID,Name,Abb from LocalSupp WITH (NOLOCK) order by ID", "8,30,20", this.TextBox1.Text)
            {
                Width = 650,
            };
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            this.TextBox1.Text = item.GetSelectedString();
            this.ValidateControl();
            this.DisplayBox1.Text = item.GetSelecteds()[0]["Name"].ToString().TrimEnd();
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            this.DisplayBox1.Text = MyUtility.GetValue.Lookup("Abb", this.TextBox1.Text.ToString(), "LocalSupp", "ID");
        }
    }
}
