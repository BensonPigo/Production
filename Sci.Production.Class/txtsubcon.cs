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
    public partial class txtsubcon : Sci.Win.UI._UserControl
    {
        private bool isIncludeJunk;
        private bool IsSubcon;
        private bool IsShipping;
        private bool IsMisc;
        public txtsubcon()
        {
            InitializeComponent();
        }

        #region 篩選條件
        public bool IsIncludeJunk
        {
            set
            {
                this.isIncludeJunk = value;
            }
            get
            {
                return this.isIncludeJunk;
            }
        }

        public bool isSubcon
        {
            set
            {
                this.IsSubcon = value;
            }
            get
            {
                return this.IsSubcon;
            }
        }

        public bool isShipping
        {
            set
            {
                this.IsShipping = value;
            }
            get
            {
                return this.IsShipping;
            }
        }

        public bool isMisc
        {
            set
            {
                this.IsMisc = value;
            }
            get
            {
                return this.IsMisc;
            }
        }
        #endregion
        

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
                string Sql = string.Format(@"
--select Junk from LocalSupp WITH (NOLOCK) where ID = '{0}'

select DISTINCT l.Junk
from dbo.LocalSupp l WITH (NOLOCK) 
left join LocalSupp_Bank lb WITH (NOLOCK)  ON l.id=lb.id 
WHERE l.Junk=0 and lb.Status= 'Confirmed' AND  l.ID = '{0}'
", textValue);

                if (IsSubcon)
                {
                    Sql += " and IsSubcon = 1 ";
                }

                if (IsShipping)
                {
                    Sql += " and IsShipping = 1 ";
                }

                if (IsMisc)
                {
                    Sql += " and IsMisc = 1 ";
                }
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
                    //this.displayBox1.Text = MyUtility.GetValue.Lookup("Abb", this.textBox1.Text.ToString(), "LocalSupp", "ID", "Production");
                    this.displayBox1.Text = MyUtility.GetValue.Lookup($@"
select DISTINCT l.Abb
from dbo.LocalSupp l WITH (NOLOCK) 
left join LocalSupp_Bank lb WITH (NOLOCK)  ON l.id=lb.id 
WHERE l.Junk=0 /*and lb.Status= 'Confirmed'*/ AND  l.ID = '{this.textBox1.Text.ToString()}'
");
                }
            }
            this.ValidateControl();
        }
        
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            this.displayBox1.Text = MyUtility.GetValue.Lookup("Abb", this.textBox1.Text.ToString(), "LocalSupp", "ID", "Production");
        }

        private void textBox1_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Sci.Win.Forms.Base myForm = (Sci.Win.Forms.Base) this.FindForm();
            if (myForm.EditMode == false || textBox1.ReadOnly==true) return;
            string selectCommand;
            selectCommand = @"
--select ID,Abb,Name from LocalSupp WITH (NOLOCK) where 1=1 

select DISTINCT l.ID ,l.Abb ,l.Name
from dbo.LocalSupp l WITH (NOLOCK) 
left join LocalSupp_Bank lb WITH (NOLOCK)  ON l.id=lb.id 
WHERE l.Junk=0 and lb.Status= 'Confirmed'
";
            if (!IsIncludeJunk)
            {
                selectCommand += " and Junk =  0 ";
            }

            if (IsShipping)
            {
                selectCommand += " and IsShipping = 1 ";
            }

            if (IsSubcon)
            {
                selectCommand += " and IsSubcon = 1 ";
            }

            if (IsMisc)
            {
                selectCommand += " and IsMisc = 1 ";
            }

            selectCommand += " Order by ID";
            DataTable tbSelect;
            DBProxy.Current.Select("Production", selectCommand, out tbSelect);
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(tbSelect, "ID,Abb,Name", "9,13,40", this.Text, false, ",", "ID,Abb,Name");
            item.Size = new System.Drawing.Size(690, 555);
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel) { return; }
            this.textBox1.Text = item.GetSelectedString();
            this.textBox1.ValidateControl();
            //this.displayBox1.Text = MyUtility.GetValue.Lookup("Abb", this.textBox1.Text.ToString(), "LocalSupp", "ID", "Production");
            this.displayBox1.Text = MyUtility.GetValue.Lookup($@"

select DISTINCT l.Abb
from dbo.LocalSupp l WITH (NOLOCK) 
left join LocalSupp_Bank lb WITH (NOLOCK)  ON l.id=lb.id 
WHERE l.Junk=0 /*and lb.Status= 'Confirmed'*/ AND  l.ID = '{this.textBox1.Text.ToString()}'
");

        }
    }
    public class cellsbucon : DataGridViewGeneratorTextColumnSettings
    {
        public static DataGridViewGeneratorTextColumnSettings GetGridCell(string suppid)
        {
            cellsbucon ts = new cellsbucon();
            // 右鍵彈出功能

            ts.EditingMouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    DataGridView grid = ((DataGridViewColumn)s).DataGridView;
                    // Parent form 若是非編輯狀態就 return 
                    if (!((Sci.Win.Forms.Base)grid.FindForm()).EditMode) { return; }
                    DataRow row = grid.GetDataRow<DataRow>(e.RowIndex);
                    DataRow row1 = grid.GetDataRow(e.RowIndex);

                    DataTable subTb;
                    string sql = @"
--select ID,Abb,Name from LocalSupp WITH (NOLOCK) where 1=1 

select DISTINCT l.ID ,l.Abb ,l.Name
from dbo.LocalSupp l WITH (NOLOCK) 
left join LocalSupp_Bank lb WITH (NOLOCK)  ON l.id=lb.id 
WHERE l.Junk=0 and lb.Status= 'Confirmed'
order by ID
";
                    DualResult duR =  DBProxy.Current.Select("Production", sql, out subTb);
                    if (duR)
                    {
                        SelectItem sele = new SelectItem(subTb, "ID,Abb,Name", "10,20,30", row[suppid].ToString());
                        DialogResult result = sele.ShowDialog();
                        if (result == DialogResult.Cancel) { return; }
                        //e.EditingControl.Text = sele.GetSelectedString();
                        row1["Suppid"] = sele.GetSelectedString();
                        row["abb"] = sele.GetSelecteds()[0]["abb"].ToString();
                    }

                }


            };
            // 正確性檢查
            ts.CellValidating += (s, e) =>
            {
                DataGridView grid = ((DataGridViewColumn)s).DataGridView;
                // Parent form 若是非編輯狀態就 return 
                if (!((Sci.Win.Forms.Base)grid.FindForm()).EditMode) { return; }
                DataRow row = grid.GetDataRow<DataRow>(e.RowIndex), sqlRow;
                String oldValue = row[suppid].ToString();
                String newValue = e.FormattedValue.ToString(); // user 編輯當下的value , 此值尚未存入DataRow
                string sql = string.Format(@"
--select ID,Abb,Name from LocalSupp WITH (NOLOCK) where  Junk =  0 and ID = '{0}'


select DISTINCT l.ID ,l.Abb ,l.Name
from dbo.LocalSupp l WITH (NOLOCK) 
left join LocalSupp_Bank lb WITH (NOLOCK)  ON l.id=lb.id 
WHERE l.Junk=0 and lb.Status= 'Confirmed' and l.ID = '{0}'
order by ID
", newValue);
                if (!MyUtility.Check.Empty(newValue) && oldValue != newValue)
                {
                    if (!MyUtility.Check.Seek(sql, out sqlRow,"Production"))
                    {
                        row[suppid] = "";
                        row.EndEdit();
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox(string.Format("< Local Supplier > : {0} not found!!!", newValue));
                        return;
                    }
                }




            };
            return ts;
        }

    }
}
