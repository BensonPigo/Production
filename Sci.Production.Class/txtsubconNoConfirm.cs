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
    /// <summary>
    /// TxtsubconNoConfirm
    /// </summary>
    public partial class TxtsubconNoConfirm : Win.UI._UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TxtsubconNoConfirm"/> class.
        /// </summary>
        public TxtsubconNoConfirm()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// LocalSupp.Junk
        /// </summary>
        public bool IsIncludeJunk { get; set; }

        /// <summary>
        /// LocalSupp.IsSubcon
        /// </summary>
        public bool IsSubcon { get; set; }

        /// <summary>
        /// LocalSupp.IsShipping
        /// </summary>
        public bool IsShipping { get; set; }

        /// <summary>
        /// LocalSupp.IsMisc
        /// </summary>
        public bool IsMisc { get; set; }

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
            string textValue = this.TextBox1.Text.Trim();
            if (textValue == this.TextBox1.OldValue)
            {
                return;
            }

            if (!string.IsNullOrWhiteSpace(textValue))
            {
                string sql = string.Format("Select Junk from LocalSupp WITH (NOLOCK) where ID = '{0}'", textValue);

                if (this.IsSubcon)
                {
                    sql += " and IsSubcon = 1 ";
                }

                if (this.IsShipping)
                {
                    sql += " and IsShipping = 1 ";
                }

                if (this.IsMisc)
                {
                    sql += " and IsMisc = 1 ";
                }

                if (!MyUtility.Check.Seek(sql, "Production"))
                {
                    this.TextBox1.Text = string.Empty;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("< Supplier: {0} > not found!!!", textValue));
                    return;
                }
                else
                {
                    if (!this.IsIncludeJunk)
                    {
                        string lookupresult = MyUtility.GetValue.Lookup(sql, "Production");
                        if (lookupresult == "True")
                        {
                            this.TextBox1.Text = string.Empty;
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox(string.Format("< Supplier: {0} > not found!!!", textValue));
                            return;
                        }
                    }

                    this.DisplayBox1.Text = MyUtility.GetValue.Lookup("Abb", this.TextBox1.Text.ToString(), "LocalSupp", "ID", "Production");
                }
            }

            this.ValidateControl();
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            this.DisplayBox1.Text = MyUtility.GetValue.Lookup("Abb", this.TextBox1.Text.ToString(), "LocalSupp", "ID", "Production");
        }

        private void TextBox1_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Win.Forms.Base myForm = (Win.Forms.Base)this.FindForm();
            if (myForm.EditMode == false || this.TextBox1.ReadOnly == true)
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
            SelectItem item = new SelectItem(tbSelect, "ID,Abb,Name", "9,13,40", this.Text, false, ",", "ID,Abb,Name");
            item.Size = new System.Drawing.Size(690, 555);
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            this.TextBox1.Text = item.GetSelectedString();
            this.TextBox1.ValidateControl();
            this.DisplayBox1.Text = MyUtility.GetValue.Lookup("Abb", this.TextBox1.Text.ToString(), "LocalSupp", "ID", "Production");
        }

        /// <inheritdoc/>
        public class CellsbuconNoConfirm : DataGridViewGeneratorTextColumnSettings
        {
            /// <summary>
            /// GetGridCell
            /// </summary>
            /// <param name="suppid">LocalSupp.ID</param>
            /// <param name="abbColName">LocalSupp.Abb</param>
            /// <returns>DataGridViewGeneratorTextColumnSettings</returns>
            public static DataGridViewGeneratorTextColumnSettings GetGridCell(string suppid, string abbColName = "")
            {
                CellsbuconNoConfirm ts = new CellsbuconNoConfirm();

                // 右鍵彈出功能
                ts.EditingMouseDown += (s, e) =>
                {
                    if (e.Button == MouseButtons.Right)
                    {
                        DataGridView grid = ((DataGridViewColumn)s).DataGridView;

                        // Parent form 若是非編輯狀態就 return
                        if (!((Win.Forms.Base)grid.FindForm()).EditMode)
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
                    if (!((Win.Forms.Base)grid.FindForm()).EditMode)
                    {
                        return;
                    }

                    DataRow row = grid.GetDataRow<DataRow>(e.RowIndex), sqlRow;
                    string oldValue = row[suppid].ToString();
                    string newValue = e.FormattedValue.ToString(); // user 編輯當下的value , 此值尚未存入DataRow
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
}
