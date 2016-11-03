using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sci.Win.UI;
using Sci.Data;
using Sci.Win.Tools;
using Ict.Win;
using Ict;
using Sci.Win;
using Ict.Data;
using Sci;

namespace Sci.Production.Class
{
    public partial class txtcutReason : UserControl
    {
        public txtcutReason()
        {
            InitializeComponent();
        }

        [Category("Custom Properties")]
        [Description("填入Reason Type。例如：RC")]
        public string Type { set; get ; }


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
          //  base.OnValidating(e);
            string str = this.textBox1.Text;
            if (!string.IsNullOrWhiteSpace(str) && str != this.textBox1.OldValue)
            {
                if (!MyUtility.Check.Seek(Type + str, "CutReason", "type+ID"))
                {
                    MyUtility.Msg.WarningBox(string.Format("< Reason: {0} > not found!!!", str));
                    this.textBox1.Text = "";
                    e.Cancel = true;
                    return;
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            this.displayBox1.Text = MyUtility.GetValue.Lookup("Description", Type + this.textBox1.Text.ToString(), "CutReason", "Type+ID");
        }

        private void textBox1_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem
                (string.Format("Select Id, Description from CutReason where type='{0}' order by id", Type), "10,100", this.textBox1.Text);
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel) { return; }
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
                    if (!((Sci.Win.Forms.Base)grid.FindForm()).EditMode) { return; }
                    DataRow row = grid.GetDataRow<DataRow>(e.RowIndex);
                    SelectItem sele = new SelectItem(string.Format("Select ID,description From CutReason Where Junk=0 and type = '{0}'", ctype), "10,50", row["cutreasonid"].ToString(), false, ",");
                    DialogResult result = sele.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    e.EditingControl.Text = sele.GetSelectedString();
                }

            };
            // 正確性檢查
            ts.CellValidating += (s, e) =>
            {
                DataGridView grid = ((DataGridViewColumn)s).DataGridView;
                // Parent form 若是非編輯狀態就 return 
                if (!((Sci.Win.Forms.Base)grid.FindForm()).EditMode) { return; }
                DataRow row = grid.GetDataRow<DataRow>(e.RowIndex);
                String oldValue = row["cutreasonid"].ToString();
                String newValue = e.FormattedValue.ToString(); // user 編輯當下的value , 此值尚未存入DataRow

                if (!MyUtility.Check.Empty(newValue) && oldValue != newValue)
                {
                    if (!MyUtility.Check.Seek(ctype+newValue, "cutreason", "Type+ID"))
                    {
                        MyUtility.Msg.WarningBox(string.Format("< Cut Reason > : {0} not found!!!", newValue));
                        row["cutreasonid"] = "";
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
