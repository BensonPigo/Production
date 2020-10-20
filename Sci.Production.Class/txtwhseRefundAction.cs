using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Sci.Production.Class
{
    /// <summary>
    /// TxtwhseRefundAction
    /// </summary>
    public partial class TxtwhseRefundAction : Win.UI._UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TxtwhseRefundAction"/> class.
        /// </summary>
        public TxtwhseRefundAction()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 是否要顯示 Junk 的資料
        /// </summary>
        [Description("是否要顯示 Junk 的資料")]
        public bool IsSupportJunk { get; set; } = true;

        /// <summary>
        /// 選擇畫面上的txtwhseReason自訂控制項名稱。例如：txtWhseReason1
        /// </summary>
        [Category("Custom Properties")]
        [Description("選擇畫面上的txtwhseReason自訂控制項名稱。例如：txtWhseReason1")]
        public TxtwhseReason WhseReason { get; set; }

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
            string str = this.TextBox1.Text;
            string sqlFilter = string.Empty;
            if (!this.IsSupportJunk)
            {
                sqlFilter = " and Junk = 0";
            }

            if (!string.IsNullOrWhiteSpace(str) && str != this.TextBox1.OldValue)
            {
                string actionCode = MyUtility.GetValue.Lookup("actioncode", "RR" + this.WhseReason.TextBox1.Text, "WhseReason", "Type+ID");
                if (actionCode == string.Empty)
                {
                    MyUtility.Msg.WarningBox("cannot found data!", "Warning");
                    return;
                }

                string sqlcmd = $@"select Id, Description from WhseReason WITH (NOLOCK) where type ='RA'and id in ({actionCode}) and id in ('{str}') {sqlFilter}";

                // if (!MyUtility.Check.Seek(str, "WhseReason", "ID"))
                if (!MyUtility.Check.Seek(sqlcmd))
                {
                    this.DisplayBox1.Text = string.Empty;
                    this.TextBox1.Text = string.Empty;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("< Refund Action: {0} > not found!!!", str));
                    this.DataBindings.Cast<Binding>().ToList().ForEach(binding => binding.WriteValue());
                    return;
                }

                DataRow temp;
                if (MyUtility.Check.Seek($@"Select Description from WhseReason WITH (NOLOCK) where ID='{str}' and Type='RA' {sqlFilter}", out temp))
                {
                    this.DisplayBox1.Text = temp[0].ToString();
                }

                this.DataBindings.Cast<Binding>().ToList().ForEach(binding => binding.WriteValue());
            }
        }

        // private void textBox1_TextChanged(object sender, EventArgs e)
        // {
        //    Sci.Win.Forms.Base myForm = (Sci.Win.Forms.Base)this.FindForm();
        //    if (myForm.EditMode == false)
        //    {
        //        this.displayBox1.Text = MyUtility.GetValue.Lookup("Description", "RA" + this.textBox1.Text.ToString(), "WhseReason", "Type+ID");
        //    }
        // }
        private void TextBox1_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sqlFilter = string.Empty;
            if (!this.IsSupportJunk)
            {
                sqlFilter = " and Junk = 0";
            }

            string actionCode = MyUtility.GetValue.Lookup("actioncode", "RR" + this.WhseReason.TextBox1.Text, "WhseReason", "Type+ID");
            if (actionCode == string.Empty)
            {
                MyUtility.Msg.WarningBox("cannot found data!", "Warning");
                return;
            }

            Win.Tools.SelectItem item = new Win.Tools.SelectItem(
                $@"
select Id, Description from WhseReason WITH (NOLOCK) 
where type ='RA' {$" and id in ({actionCode})"}
{sqlFilter}
", "10,30",
                this.TextBox1.Text);
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.TextBox1.Text = item.GetSelectedString();
            this.DisplayBox1.Text = item.GetSelecteds()[0][1].ToString();
            this.Validate();
            this.DataBindings.Cast<Binding>().ToList().ForEach(binding => binding.WriteValue());
        }
    }
}
