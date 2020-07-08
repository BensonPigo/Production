using System;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using Sci.Data;
using Ict;

namespace Sci.Production.Class
{
    public partial class txtLocalTPESupp : Win.UI._UserControl
    {
        private bool isIncludeJunk;

        public txtLocalTPESupp()
        {
            this.InitializeComponent();
        }

        [Category("Custom Properties")]
        public bool IsIncludeJunk
        {
            get { return this.isIncludeJunk; }
            set { this.isIncludeJunk = value; }
        }

        public Win.UI.TextBox TextBox1
        {
            get { return this.textBox1; }
        }

        public Win.UI.DisplayBox DisplayBox1
        {
            get { return this.displayBox1; }
        }

        [Bindable(true)]
        public string TextBox1Binding
        {
            get { return this.textBox1.Text; }
            set { this.textBox1.Text = value; }
        }

        [Bindable(true)]
        public string DisplayBox1Binding
        {
            get { return this.displayBox1.Text; }
            set { this.displayBox1.Text = value; }
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
                string Sql = string.Format(
                    @"
select l.Junk
from LocalSupp l WITH (NOLOCK) 
WHERE l.Junk=0  AND l.ID = '{0}'  
union all 
select Junk from Supp WITH (NOLOCK) where ID = '{0}'
", textValue);
                if (!MyUtility.Check.Seek(Sql, "Production"))
                {
                    this.textBox1.Text = string.Empty;
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
                            this.textBox1.Text = string.Empty;
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox(string.Format("< Supplier: {0} > not found!!!", textValue));
                            return;
                        }
                    }

                    string sql_cmd = $@"
select l.Abb
from LocalSupp l WITH (NOLOCK) 
WHERE l.Junk=0 AND l.ID = '{this.textBox1.Text.ToString()}'   
union all 
select [Abb] = AbbEN from Supp WITH (NOLOCK) where  Junk =  0 and ID = '{this.textBox1.Text.ToString()}' ";
                    this.displayBox1.Text = MyUtility.GetValue.Lookup(sql_cmd, "Production");
                }
            }

            this.ValidateControl();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string sql_cmd = $@"
select l.Abb
from LocalSupp l WITH (NOLOCK) 
WHERE l.ID = '{this.textBox1.Text.ToString()}'   
union all 
select [Abb] = AbbEN from Supp WITH (NOLOCK) where  Junk =  0 and ID = '{this.textBox1.Text.ToString()}' 
";
            this.displayBox1.Text = MyUtility.GetValue.Lookup(sql_cmd, "Production");
        }

        private void textBox1_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Win.Forms.Base myForm = (Win.Forms.Base)this.FindForm();
            if (myForm.EditMode == false || this.textBox1.ReadOnly == true)
            {
                return;
            }

            string selectCommand;
            selectCommand = $@"
select l.ID , l.Abb , l.Name
from LocalSupp l WITH (NOLOCK) 
WHERE l.Junk=0
union all 
select ID,[Name] = NameEN,[Abb] = AbbEN from Supp WITH (NOLOCK) order by ID";
            if (!this.IsIncludeJunk)
            {
                selectCommand = @"
select l.ID , l.Abb , l.Name
from LocalSupp l WITH (NOLOCK) 
WHERE l.Junk=0 
union all 
select ID,[Name] = NameEN,[Abb] = AbbEN from Supp WITH (NOLOCK) where  Junk =  0  order by ID";
            }

            DataTable tbSelect;
            DBProxy.Current.Select("Production", selectCommand, out tbSelect);
            Win.Tools.SelectItem item = new Win.Tools.SelectItem(tbSelect, "ID,Abb,Name", "9,13,40", this.Text, false, ",", "ID,Abb,Name");
            item.Size = new System.Drawing.Size(690, 555);
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            this.textBox1.Text = item.GetSelectedString();
            this.textBox1.ValidateControl();
            string sql_cmd = $@"
select l.Abb
from LocalSupp l WITH (NOLOCK) 
WHERE l.Junk=0 AND l.ID = '{this.textBox1.Text.ToString()}'   
union all 
select [Abb] = AbbEN from Supp WITH (NOLOCK) where  Junk =  0 and ID = '{this.textBox1.Text.ToString()}' ";
            this.displayBox1.Text = MyUtility.GetValue.Lookup(sql_cmd, "Production");
        }
    }
}
