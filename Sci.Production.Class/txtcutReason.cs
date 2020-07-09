using System;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using Sci.Win.Tools;
using Ict.Win;
using Ict;

namespace Sci.Production.Class
{
    /// <summary>
    /// TxtcutReason
    /// </summary>
    public partial class TxtcutReason : Win.UI._UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TxtcutReason"/> class.
        /// </summary>
        public TxtcutReason()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 填入Reason Type。例如：RC
        /// </summary>
        [Category("Custom Properties")]
        [Description("填入Reason Type。例如：RC")]
        public string Type { get; set; }

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
            string str = this.TextBox1.Text;
            if (!string.IsNullOrWhiteSpace(str) && str != this.TextBox1.OldValue)
            {
                if (!MyUtility.Check.Seek(this.Type + str, "CutReason", "type+ID"))
                {
                    this.TextBox1.Text = string.Empty;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("< Reason: {0} > not found!!!", str));
                    return;
                }
            }
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            this.DisplayBox1.Text = MyUtility.GetValue.Lookup("Description", this.Type + this.TextBox1.Text.ToString(), "CutReason", "Type+ID");
        }

        private void TextBox1_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            SelectItem item = new SelectItem(
                string.Format("Select Id, Description from CutReason WITH (NOLOCK) where type='{0}' order by id", this.Type), "10,40", this.TextBox1.Text);
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.TextBox1.Text = item.GetSelectedString();
            this.Validate();
        }

        /// <summary>
        /// Cellcutreason
        /// </summary>
        public class Cellcutreason : DataGridViewGeneratorTextColumnSettings
        {
            /// <summary>
            /// GetGridCell
            /// </summary>
            /// <param name="ctype">Type</param>
            /// <returns>DataGridViewGeneratorTextColumnSettings</returns>
            public static DataGridViewGeneratorTextColumnSettings GetGridCell(string ctype)
            {
                Cellcutreason ts = new Cellcutreason();

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
                        SelectItem sele = new SelectItem(string.Format("Select ID,description From CutReason WITH (NOLOCK) Where Junk=0 and type = '{0}'", ctype), "10,40", row["cutreasonid"].ToString(), false, ",");
                        DialogResult result = sele.ShowDialog();
                        if (result == DialogResult.Cancel)
                        {
                            return;
                        }

                        e.EditingControl.Text = sele.GetSelectedString();
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
                    string oldValue = row["cutreasonid"].ToString();
                    string newValue = e.FormattedValue.ToString(); // user 編輯當下的value , 此值尚未存入DataRow

                    if (!MyUtility.Check.Empty(newValue) && oldValue != newValue)
                    {
                        if (!MyUtility.Check.Seek(ctype + newValue, "cutreason", "Type+ID"))
                        {
                            row["cutreasonid"] = string.Empty;
                            row.EndEdit();
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox(string.Format("< Cut Reason > : {0} not found!!!", newValue));
                            return;
                        }
                    }
                };
                return ts;
            }
        }
    }
}
