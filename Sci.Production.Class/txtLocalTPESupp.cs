using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Class
{
    public partial class txtLocalTPESupp : Sci.Win.UI._UserControl
    {
        public txtLocalTPESupp()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }


        public Sci.Win.UI.TextBox TextBox1
        {
            get { return this.textBox1; }
            set { this.textBox1 = value; }
        }

        public Sci.Win.UI.DisplayBox DisplayBox1
        {
            get { return this.displayBox1; }
            set { this.displayBox1 = value; }
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
           // base.OnValidating(e);
            string textValue = this.textBox1.Text;

            if (!string.IsNullOrWhiteSpace(textValue) && textValue != this.textBox1.OldValue)
            {
                if (!MyUtility.Check.Seek($"select 1 from LocalSupp WITH (NOLOCK) where id = '{textValue}' union all select 1 from Supp WITH (NOLOCK) where id = '{textValue}'"))
                {
                    this.textBox1.Text = "";
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("< LocalSupplier Code: {0} > not found!!!", textValue));
                    return;
                }
            }
           
        }

      

        private void textBox1_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Sci.Win.Forms.Base myForm = (Sci.Win.Forms.Base)this.FindForm();
            if (myForm.EditMode == false || textBox1.ReadOnly == true) return;
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem("select ID,Name,Abb from LocalSupp WITH (NOLOCK) union all select ID,[Name] = NameEN,[Abb] = AbbEN from Supp WITH (NOLOCK) order by ID ", "8,30,20", this.textBox1.Text);
            item.Width = 650;
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel) { return; }
            this.textBox1.Text = item.GetSelectedString();
            this.displayBox1.Text = item.GetSelecteds()[0]["Name"].ToString().TrimEnd();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            this.displayBox1.Text = MyUtility.GetValue.Lookup($"select top 1 Abb from ( select Abb from LocalSupp WITH (NOLOCK) where id ='{this.textBox1.Text.ToString()}' union all select [Abb] = AbbEN from Supp WITH (NOLOCK) where id = '{this.textBox1.Text.ToString()}') a ");
        }
    }
}
