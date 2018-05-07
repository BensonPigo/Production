using System;
using System.Collections.Generic;
using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sci.Data;
using Sci.Win.UI;
using Ict;
using Sci.Win.Tools;
using Sci.Win;
using Ict.Win;
using System.Data.SqlClient;

namespace Sci.Production.Class
{
    public partial class txtLocalTPESupp : Sci.Win.UI._UserControl
    {
        private bool isIncludeJunk;
        public txtLocalTPESupp()
        {
            InitializeComponent();
        }

        [Category("Custom Properties")]
        public bool IsIncludeJunk
        {
            set { this.isIncludeJunk = value; }
            get { return this.isIncludeJunk; }
        }

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
            // base.OnValidating(e);
            string textValue = this.textBox1.Text.Trim();
            if (textValue == this.textBox1.OldValue)
            {
                return;
            }
            if (!string.IsNullOrWhiteSpace(textValue))
            {
                string Sql = string.Format("Select Junk from LocalSupp WITH (NOLOCK) where ID = '{0}'  union all select Junk from Supp WITH (NOLOCK) where ID = '{0}' ", textValue);
                if (!MyUtility.Check.Seek(Sql, "Production"))
                {
                    this.textBox1.Text = "";
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("< Supplier: {0} > not found!!!", textValue));
                    return;
                }
                else
                {
                    if (!this.IsIncludeJunk)
                    {
                        string lookupresult = MyUtility.GetValue.Lookup(Sql, "Production");
                        if (lookupresult == "True")
                        {
                            this.textBox1.Text = "";
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox(string.Format("< Supplier: {0} > not found!!!", textValue));
                            return;
                        }
                    }
                    string sql_cmd = $"select Abb from LocalSupp WITH (NOLOCK) where  Junk =  0  and ID = '{this.textBox1.Text.ToString()}'   union all select [Abb] = AbbEN from Supp WITH (NOLOCK) where  Junk =  0 and ID = '{this.textBox1.Text.ToString()}' ";
                    this.displayBox1.Text = MyUtility.GetValue.Lookup(sql_cmd, "Production");
                }
            }
            this.ValidateControl();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string sql_cmd = $"select Abb from LocalSupp WITH (NOLOCK) where  Junk =  0  and ID = '{this.textBox1.Text.ToString()}'   union all select [Abb] = AbbEN from Supp WITH (NOLOCK) where  Junk =  0 and ID = '{this.textBox1.Text.ToString()}' ";
            this.displayBox1.Text = MyUtility.GetValue.Lookup(sql_cmd, "Production");
        }

        private void textBox1_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Sci.Win.Forms.Base myForm = (Sci.Win.Forms.Base)this.FindForm();
            if (myForm.EditMode == false || textBox1.ReadOnly == true) return;
            string selectCommand;
            selectCommand = "select ID,Abb,Name from LocalSupp WITH (NOLOCK)  union all select ID,[Name] = NameEN,[Abb] = AbbEN from Supp WITH (NOLOCK) order by ID";
            if (!IsIncludeJunk)
            {
                selectCommand = "select ID,Abb,Name from LocalSupp WITH (NOLOCK) where  Junk =  0  union all select ID,[Name] = NameEN,[Abb] = AbbEN from Supp WITH (NOLOCK) where  Junk =  0  order by ID";
            }
            DataTable tbSelect;
            DBProxy.Current.Select("Production", selectCommand, out tbSelect);
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(tbSelect, "ID,Abb,Name", "9,13,40", this.Text, false, ",", "ID,Abb,Name");
            item.Size = new System.Drawing.Size(690, 555);
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel) { return; }
            this.textBox1.Text = item.GetSelectedString();
            this.textBox1.ValidateControl();
            string sql_cmd = $"select Abb from LocalSupp WITH (NOLOCK) where  Junk =  0  and ID = '{this.textBox1.Text.ToString()}'   union all select [Abb] = AbbEN from Supp WITH (NOLOCK) where  Junk =  0 and ID = '{this.textBox1.Text.ToString()}' ";
            this.displayBox1.Text = MyUtility.GetValue.Lookup(sql_cmd, "Production");


        }
    }
   
    
}
