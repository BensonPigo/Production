using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sci.Win.UI;
using Sci.Data;
using Sci.Win.Tools;
using Ict.Win;
using Ict;
using Sci.Win;
using Ict.Data;
using Sci;

namespace Sci.Production.Class
{
    public partial class txtShippingReason : Sci.Win.UI._UserControl
    {
        public txtShippingReason()
        {
            InitializeComponent();
        }

        [Category("Custom Properties")]
        [Description("填入Reason Type。例如：AQ")]
        public string Type { set; get ; }


        public Sci.Win.UI.TextBox TextBox1
        {
            get { return this.textBox1; }
        }

         public Sci.Win.UI.DisplayBox DisplayBox1
        {
            get { return this.displayBox1; }
        }

        [Bindable(true)]
        public string TextBox1Binding
        {
            set { this.textBox1.Text = value; }
            get { return textBox1.Text; }
        }

        [Bindable(true)]
        public string DisplayBox1Binding
        {
            set { this.displayBox1.Text = value; }
            get { return this.displayBox1.Text; }
        }

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            //  base.OnValidating(e);
            string textValue = this.textBox1.Text.Trim();
            if (textValue == this.textBox1.OldValue)
            {
                return;
            }
            if (!string.IsNullOrWhiteSpace(textValue))
            {
                string Sql = string.Format("Select 1 from ShippingReason WITH (NOLOCK) where ID='{0}' and Type='{1}'", textValue, Type);
                if (!MyUtility.Check.Seek(Sql, "Production"))
                {
                    this.textBox1.Text = "";
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("< Reason: {0} > not found!!!", textValue));
                    return;
                }
                else
                {
                    string sql_cmd = string.Format("Select Description from ShippingReason WITH (NOLOCK) where ID='{0}' and Type='{1}'", textValue, Type);
                    this.displayBox1.Text = MyUtility.GetValue.Lookup(sql_cmd, "Production");
                }
            }
            this.ValidateControl();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string sql_cmd = string.Format("Select Description from ShippingReason WITH (NOLOCK) where ID='{0}' and Type='{1}'", this.textBox1.Text, Type);
            this.displayBox1.Text = MyUtility.GetValue.Lookup(sql_cmd, "Production");
        }

        private void textBox1_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem
                (string.Format("Select Id, Description from ShippingReason WITH (NOLOCK) where type='{0}' order by id", Type), "10,40", this.textBox1.Text);
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel) { return; }
            this.textBox1.Text = item.GetSelectedString();
            this.Validate();
        }
    }
}
