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
    public partial class txtLocalTPESupp : Sci.Win.UI.TextBox
    {
        public txtLocalTPESupp()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
           // base.OnValidating(e);
            string textValue = this.Text;

            if (!string.IsNullOrWhiteSpace(textValue) && textValue != this.OldValue)
            {
                if (!MyUtility.Check.Seek($"select 1 from LocalSupp WITH (NOLOCK) where id = '{textValue}' union all select 1 from Supp WITH (NOLOCK) where id = '{textValue}'"))
                {
                    this.Text = "";
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("< LocalSupplier Code: {0} > not found!!!", textValue));
                    return;
                }
            }
           
        }

      

        private void textBox1_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Sci.Win.Forms.Base myForm = (Sci.Win.Forms.Base)this.FindForm();
            if (myForm.EditMode == false || this.ReadOnly == true) return;
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem("select ID,Name,Abb from LocalSupp WITH (NOLOCK) union all select ID,[Name] = NameEN,[Abb] = AbbEN from Supp WITH (NOLOCK) order by ID ", "8,30,20", this.Text);
            item.Width = 650;
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel) { return; }
            this.Text = item.GetSelectedString();
        }

    }
}
