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

namespace Sci.Machine.Class
{
    public partial class txtmiscbrand : UserControl
    {
        public txtmiscbrand()
        {
            InitializeComponent();
            this.Width = 78;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Sci.Win.UI.TextBox TextBox1
        {
            get { return this.textBox1; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Sci.Win.UI.DisplayBox DisplayBox1
        {
            get { return this.displayBox1; }
        }

        [Bindable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public string TextBox1Binding
        {
            set { this.textBox1.Text = value; }
            get { return textBox1.Text; }
        }

        [Bindable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public string DisplayBox1Binding
        {
            set { this.displayBox1.Text = value; }
            get { return this.displayBox1.Text; }
        }
        private void textBox1_PopUp(object sender, TextBoxPopUpEventArgs e)
        {
            if(this.textBox1.ReadOnly ==true) return;
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem("Select ID,Name From MiscBrand Where Junk=0", "10,50", this.Text, false, ",");
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel) { return; }
            this.textBox1.Text = item.GetSelectedString();
        }
        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            base.OnValidating(e);
            if (!string.IsNullOrWhiteSpace(this.textBox1.Text))
            {
                string miscBrand = this.textBox1.Text;
                if (!MyUtility.Check.Seek(miscBrand, "MiscBrand", "ID"))
                {
                    this.textBox1.Text = string.Empty;
                    this.displayBox1.Text = string.Empty;
                    MyUtility.Msg.WarningBox(string.Format("< Machine Brand > : {0} not found!!!", miscBrand));
                    e.Cancel = true;
                    return;
                }
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

            string miscBrand = this.textBox1.Text;
            string miscBrandName = MyUtility.GetValue.Lookup("Name", miscBrand, "MiscBrand", "ID");
            this.displayBox1.Text = miscBrandName;
            
        }
    }

    public class cellmiscbrand : DataGridViewGeneratorTextColumnSettings
    {
        public static DataGridViewGeneratorTextColumnSettings GetGridCell()
        {
            cellmiscbrand ts = new cellmiscbrand();
            // Factory右鍵彈出功能
            ts.EditingMouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    DataGridView grid = ((DataGridViewColumn)s).DataGridView;
                    // Parent form 若是非編輯狀態就 return 
                    if (!((Sci.Win.Forms.Base)grid.FindForm()).EditMode) { return; }
                    DataRow row = grid.GetDataRow<DataRow>(e.RowIndex);
                    SelectItem sele = new SelectItem("Select ID,Name From MiscBrand Where Junk=0", "10,50", row["machinebrandid"].ToString(), false, ",");
                    DialogResult result = sele.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    e.EditingControl.Text= sele.GetSelectedString();
                }

            };
            // 正確性檢查
            ts.CellValidating += (s, e) =>
            {
                DataGridView grid = ((DataGridViewColumn)s).DataGridView;
                // Parent form 若是非編輯狀態就 return 
                if (!((Sci.Win.Forms.Base)grid.FindForm()).EditMode) { return; }
                DataRow row = grid.GetDataRow<DataRow>(e.RowIndex);
                String oldValue = row["miscbrandid"].ToString();
                String newValue = e.FormattedValue.ToString(); // user 編輯當下的value , 此值尚未存入DataRow

                if (!MyUtility.Check.Empty(newValue) && oldValue != newValue)
                {
                    if (!MyUtility.Check.Seek(newValue, "MiscBrand", "ID"))
                    {
                        MyUtility.Msg.WarningBox(string.Format("< Miscellaneous Brand > : {0} not found!!!", newValue));
                        row["miscbrandid"] = "";
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