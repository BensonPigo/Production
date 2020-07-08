using System;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using Sci.Win.Tools;
using Ict.Win;
using Ict;

namespace Sci.Production.Class
{
    public partial class txtcutReason : Sci.Win.UI._UserControl
    {
        public txtcutReason()
        {
            this.InitializeComponent();
        }

        [Category("Custom Properties")]
        [Description("填入Reason Type。例如：RC")]
        public string Type { get; set; }

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
            string str = this.textBox1.Text;
            if (!string.IsNullOrWhiteSpace(str) && str != this.textBox1.OldValue)
            {
                if (!MyUtility.Check.Seek(this.Type + str, "CutReason", "type+ID"))
                {
                    this.textBox1.Text = string.Empty;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("< Reason: {0} > not found!!!", str));
                    return;
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            this.displayBox1.Text = MyUtility.GetValue.Lookup("Description", this.Type + this.textBox1.Text.ToString(), "CutReason", "Type+ID");
        }

        private void textBox1_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(
                string.Format("Select Id, Description from CutReason WITH (NOLOCK) where type='{0}' order by id", this.Type), "10,40", this.textBox1.Text);
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.textBox1.Text = item.GetSelectedString();
            this.Validate();
        }
    }

    public class cellcutreason : DataGridViewGeneratorTextColumnSettings
    {
        public static DataGridViewGeneratorTextColumnSettings GetGridCell(string ctype)
        {
            cellcutreason ts = new cellcutreason();

            // Factory右鍵彈出功能
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
                if (!((Sci.Win.Forms.Base)grid.FindForm()).EditMode)
                {
                    return;
                }

                DataRow row = grid.GetDataRow<DataRow>(e.RowIndex);
                String oldValue = row["cutreasonid"].ToString();
                String newValue = e.FormattedValue.ToString(); // user 編輯當下的value , 此值尚未存入DataRow

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
