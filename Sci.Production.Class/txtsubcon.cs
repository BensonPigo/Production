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

namespace Sci.Production.Class
{
    public partial class txtsubcon : Sci.Win.UI._UserControl
    {
        private bool isIncludeJunk;
        public txtsubcon()
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
            string textValue = this.textBox1.Text;
            if (!string.IsNullOrWhiteSpace(textValue) && textValue != this.textBox1.OldValue)
            {
                string Sql = string.Format("Select Junk from LocalSupp where ID = '{0}'", textValue);
                if (!MyUtility.Check.Seek(Sql, "Production"))
                {
                    MyUtility.Msg.WarningBox(string.Format("< Subcon Code: {0} > not found!!!", textValue));
                    this.textBox1.Text = "";
                    e.Cancel = true;
                    return;
                }
                else
                {
                    if (!this.IsIncludeJunk)
                    {
                        string lookupresult = MyUtility.GetValue.Lookup(Sql, "Production");
                        if (lookupresult == "True")
                        {
                            MyUtility.Msg.WarningBox(string.Format("< Subcon Code: {0} > not found!!!", textValue));
                            this.textBox1.Text = "";
                            e.Cancel = true;
                            return;
                        }
                    }
                    this.displayBox1.Text = MyUtility.GetValue.Lookup("Abb", this.textBox1.Text.ToString(), "LocalSupp", "ID", "Production");
                }
            }
            this.ValidateControl();
        }
        
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
           
            Sci.Win.Forms.Base myForm = (Sci.Win.Forms.Base) this.FindForm();
            if (myForm.EditMode == false)
            {
                this.displayBox1.Text = MyUtility.GetValue.Lookup("Abb", this.textBox1.Text.ToString(), "LocalSupp", "ID", "Production");
            }
            this.displayBox1.Text = MyUtility.GetValue.Lookup("Abb", this.textBox1.Text.ToString(), "LocalSupp", "ID", "Production");
        }

        private void textBox1_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Sci.Win.Forms.Base myForm = (Sci.Win.Forms.Base) this.FindForm();
            if (myForm.EditMode == false || textBox1.ReadOnly==true) return;
            string selectCommand;
            selectCommand = "select ID,Abb,Name from LocalSupp order by ID";
            if (!IsIncludeJunk)
            {
                selectCommand = "select ID,Abb,Name from LocalSupp where  Junk =  0 order by ID";
            }
            DataTable tbSelect;
            DBProxy.Current.Select("Production", selectCommand, out tbSelect);
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(tbSelect, "ID,Abb,Name", "9,13,40", this.Text, false, ",", "ID,Abb,Name");
            item.Size = new System.Drawing.Size(690, 555);
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel) { return; }
            this.textBox1.Text = item.GetSelectedString();
            this.textBox1.ValidateControl();
            this.displayBox1.Text = MyUtility.GetValue.Lookup("Abb", this.textBox1.Text.ToString(), "LocalSupp", "ID", "Production");
           

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
                    string sql = "select ID,Abb,Name from LocalSupp where  Junk =  0 order by ID";
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
                string sql = string.Format("select ID,Abb,Name from LocalSupp where  Junk =  0 and ID = '{0}'", newValue);
                if (!MyUtility.Check.Empty(newValue) && oldValue != newValue)
                {
                    if (!MyUtility.Check.Seek(sql, out sqlRow,"Production"))
                    {
                        MyUtility.Msg.WarningBox(string.Format("< Local Supplier > : {0} not found!!!", newValue));
                        row[suppid] = "";
                        row.EndEdit();
                        e.Cancel = true;
                        return;
                    }
                }




            };
            return ts;
        }

    }
}
