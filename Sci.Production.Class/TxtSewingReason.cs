using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Win.Tools;

namespace Sci.Production.Class
{
    /// <summary>
    /// TxtSewingReason
    /// </summary>
    public partial class TxtSewingReason : Win.UI._UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TxtSewingReason"/> class.
        /// </summary>
        public TxtSewingReason()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 填入Reason Type。例如：AQ
        /// </summary>
        [Category("Custom Properties")]
        [Description("填入Reason Type。例如：AQ")]
        public string Type { get; set; }

        public string LinkDB { get; set; } = "Production";

        /// <inheritdoc/>
        public Win.UI.TextBox TextBox1 { get; private set; }

        /// <inheritdoc/>
        public Win.UI.DisplayBox DisplayBox1 { get; private set; }

        /// <inheritdoc/>
        public List<string> ListFixedID { get; set; } = new List<string>();

        private string whereID
        {
            get
            {
                if (this.ListFixedID.Count == 0)
                {
                    return string.Empty;
                }
                else
                {
                    return $" and ID in ({this.ListFixedID.Select(s => $"'{s}'").JoinToString(",")})";
                }
            }
        }

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
            string textValue = this.TextBox1.Text.Trim();
            if (textValue == this.TextBox1.OldValue)
            {
                return;
            }

            if (!string.IsNullOrWhiteSpace(textValue))
            {
                string sql = string.Format("Select 1 from SewingReason WITH (NOLOCK) where ID='{0}' and Type='{1}' {2}", textValue, this.Type, this.whereID);
                if (!MyUtility.Check.Seek(sql, LinkDB))
                {
                    this.TextBox1.Text = string.Empty;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("< Reason: {0} > not found!!!", textValue));
                    return;
                }
                else
                {
                    string sql_cmd = string.Format("Select Description from SewingReason WITH (NOLOCK) where ID='{0}' and Type='{1}'", textValue, this.Type);
                    this.DisplayBox1.Text = MyUtility.GetValue.Lookup(sql_cmd, LinkDB);
                }
            }

            this.ValidateControl();
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            string sql_cmd = string.Format("Select Description from SewingReason WITH (NOLOCK) where ID='{0}' and Type='{1}'", this.TextBox1.Text, this.Type);
            this.DisplayBox1.Text = MyUtility.GetValue.Lookup(sql_cmd, LinkDB);
        }

        private void TextBox1_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            DataTable dtPopResult;

            DualResult result = DBProxy.Current.Select(this.LinkDB, string.Format("Select Id, Description from SewingReason WITH (NOLOCK) where type='{0}' {1} order by id", this.Type, this.whereID), out dtPopResult);

            SelectItem selectItem = new SelectItem(dtPopResult, "ID,Description", null, null);

            DialogResult dialogResult = selectItem.ShowDialog();

            if (dialogResult == DialogResult.Cancel)
            {
                return;
            }

            this.TextBox1.Text = selectItem.GetSelectedString();
            this.Validate();
        }

        /// <summary>
        /// CellSewingReason
        /// </summary>
        public class CellSewingReason : DataGridViewGeneratorTextColumnSettings
        {
            /// <summary>
            /// GetGridCell
            /// </summary>
            /// <param name="ctype">Type</param>
            /// <param name="reasonID">reasonID</param>
            /// <param name="descriptionColName">descriptionColName</param>
            /// <returns>DataGridViewGeneratorTextColumnSettings</returns>
            public static DataGridViewGeneratorTextColumnSettings GetGridCell(string ctype, string reasonID, string descriptionColName)
            {
                CellSewingReason ts = new CellSewingReason();

                // Factory右鍵彈出功能
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
                        SelectItem sele = new SelectItem(string.Format("Select ID, Description from SewingReason WITH (NOLOCK) where type='{0}' order by id", ctype), "10,40", row["ID"].ToString(), false, ",");
                        DialogResult result = sele.ShowDialog();
                        if (result == DialogResult.Cancel)
                        {
                            return;
                        }

                        row[reasonID] = sele.GetSelectedString();
                        if (!MyUtility.Check.Empty(descriptionColName))
                        {
                            row[descriptionColName] = sele.GetSelecteds()[0]["Description"].ToString();
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

                    DataRow row = grid.GetDataRow<DataRow>(e.RowIndex);
                    string oldValue = row[reasonID].ToString();
                    string newValue = e.FormattedValue.ToString(); // user 編輯當下的value , 此值尚未存入DataRow
                    string sql = string.Format("Select ID, Description from SewingReason WITH (NOLOCK) where type='{0}' and id = '{1}' and Junk = 0", ctype, newValue);

                    if (MyUtility.Check.Empty(newValue))
                    {
                        row[reasonID] = string.Empty;
                        row[descriptionColName] = string.Empty;
                        row.EndEdit();
                        return;
                    }

                    if (oldValue != newValue &&
                        !MyUtility.Check.Seek(sql, out DataRow sqlRow, "Production"))
                    {
                        row[reasonID] = string.Empty;
                        row[descriptionColName] = string.Empty;
                        row.EndEdit();
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox(string.Format("< Shipping Reason > : {0} not found!!!", newValue));
                        return;
                    }
                };

                return ts;
            }
        }
    }
}
