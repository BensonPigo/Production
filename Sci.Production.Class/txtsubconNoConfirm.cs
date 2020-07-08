using System;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using Sci.Data;
using Ict;
using Sci.Win.Tools;
using Ict.Win;

namespace Sci.Production.Class
{
    public partial class txtsubconNoConfirm : Sci.Win.UI._UserControl
    {
        private bool isIncludeJunk;
        private bool IsSubcon;
        private bool IsShipping;
        private bool IsMisc;

        public txtsubconNoConfirm()
        {
            this.InitializeComponent();
        }

        #region 篩選條件
        public bool IsIncludeJunk
        {
            get
            {
                return this.isIncludeJunk;
            }

            set
            {
                this.isIncludeJunk = value;
            }
        }

        public bool isSubcon
        {
            get
            {
                return this.IsSubcon;
            }

            set
            {
                this.IsSubcon = value;
            }
        }

        public bool isShipping
        {
            get
            {
                return this.IsShipping;
            }

            set
            {
                this.IsShipping = value;
            }
        }

        public bool isMisc
        {
            get
            {
                return this.IsMisc;
            }

            set
            {
                this.IsMisc = value;
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
                string Sql = string.Format("Select Junk from LocalSupp WITH (NOLOCK) where ID = '{0}'", textValue);

                if (this.IsSubcon)
                {
                    Sql += " and IsSubcon = 1 ";
                }

                if (this.IsShipping)
                {
                    Sql += " and IsShipping = 1 ";
                }

                if (this.IsMisc)
                {
                    Sql += " and IsMisc = 1 ";
                }

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

                    this.displayBox1.Text = MyUtility.GetValue.Lookup("Abb", this.textBox1.Text.ToString(), "LocalSupp", "ID", "Production");
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
            Sci.Win.Forms.Base myForm = (Sci.Win.Forms.Base)this.FindForm();
            if (myForm.EditMode == false || this.textBox1.ReadOnly == true)
            {
                return;
            }

            string selectCommand;
            selectCommand = "select ID,Abb,Name from LocalSupp WITH (NOLOCK) where 1=1 ";
            if (!this.IsIncludeJunk)
            {
                selectCommand += " and Junk =  0 ";
            }

            if (this.IsShipping)
            {
                selectCommand += " and IsShipping = 1 ";
            }

            if (this.IsSubcon)
            {
                selectCommand += " and IsSubcon = 1 ";
            }

            if (this.IsMisc)
            {
                selectCommand += " and IsMisc = 1 ";
            }

            selectCommand += " Order by ID";
            DataTable tbSelect;
            DBProxy.Current.Select("Production", selectCommand, out tbSelect);
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(tbSelect, "ID,Abb,Name", "9,13,40", this.Text, false, ",", "ID,Abb,Name");
            item.Size = new System.Drawing.Size(690, 555);
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            this.textBox1.Text = item.GetSelectedString();
            this.textBox1.ValidateControl();
            this.displayBox1.Text = MyUtility.GetValue.Lookup("Abb", this.textBox1.Text.ToString(), "LocalSupp", "ID", "Production");
        }
    }

    public class cellsbuconNoConfirm : DataGridViewGeneratorTextColumnSettings
    {
        public static DataGridViewGeneratorTextColumnSettings GetGridCell(string suppid, string abbColName = "")
        {
            cellsbuconNoConfirm ts = new cellsbuconNoConfirm();

            // 右鍵彈出功能
            ts.EditingMouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    DataGridView grid = ((DataGridViewColumn)s).DataGridView;

                    // Parent form 若是非編輯狀態就 return
                    if (!((Sci.Win.Forms.Base)grid.FindForm()).EditMode)
                    {
                        return;
                    }

                    DataRow row = grid.GetDataRow<DataRow>(e.RowIndex);
                    DataRow row1 = grid.GetDataRow(e.RowIndex);

                    DataTable subTb;
                    string sql = "select ID,Abb,Name from LocalSupp WITH (NOLOCK) where  Junk =  0 order by ID";
                    DualResult duR = DBProxy.Current.Select("Production", sql, out subTb);
                    if (duR)
                    {
                        SelectItem sele = new SelectItem(subTb, "ID,Abb,Name", "10,20,30", row[suppid].ToString());
                        DialogResult result = sele.ShowDialog();
                        if (result == DialogResult.Cancel)
                        {
                            return;
                        }

                        // e.EditingControl.Text = sele.GetSelectedString();
                        row1[suppid] = sele.GetSelectedString();
                        if (!MyUtility.Check.Empty(abbColName))
                        {
                            row[abbColName] = sele.GetSelecteds()[0]["abb"].ToString();
                        }
                    }
                }
            };

            // 正確性檢查
            ts.CellValidating += (s, e) =>
            {
                DataGridView grid = ((DataGridViewColumn)s).DataGridView;

                // Parent form 若是非編輯狀態就 return
                if (!((Sci.Win.Forms.Base)grid.FindForm()).EditMode)
                {
                    return;
                }

                DataRow row = grid.GetDataRow<DataRow>(e.RowIndex), sqlRow;
                String oldValue = row[suppid].ToString();
                String newValue = e.FormattedValue.ToString(); // user 編輯當下的value , 此值尚未存入DataRow
                string sql = string.Format("select ID,Abb,Name from LocalSupp WITH (NOLOCK) where  Junk =  0 and ID = '{0}'", newValue);
                if (!MyUtility.Check.Empty(newValue) && oldValue != newValue)
                {
                    if (!MyUtility.Check.Seek(sql, out sqlRow, "Production"))
                    {
                        row[suppid] = string.Empty;
                        row.EndEdit();
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox(string.Format("< Local Supplier > : {0} not found!!!", newValue));
                        return;
                    }
                    else
                    {
                        if (!MyUtility.Check.Empty(abbColName))
                        {
                            row[suppid] = newValue;
                            row[abbColName] = sqlRow["abb"].ToString();
                        }
                    }
                }
            };
            return ts;
        }
    }
}
