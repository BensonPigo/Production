﻿using System;
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
    public partial class txtShippingReason : Sci.Win.UI._UserControl
    {
        public txtShippingReason()
        {
            InitializeComponent();
        }

        [Category("Custom Properties")]
        [Description("填入Reason Type。例如：AQ")]
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
                if (!MyUtility.Check.Seek(Type + str, "ShippingReason", "type+ID"))
                {
                    this.textBox1.Text = "";
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("< Reason: {0} > not found!!!", str));
                    return;
                }
                DataRow temp;
                if (MyUtility.Check.Seek(string.Format("Select Description from ShippingReason WITH (NOLOCK) where ID='{0}' and Type='{1}'", str, Type), out temp))
                    this.DisplayBox1.Text = temp[0].ToString();

                this.DataBindings.Cast<Binding>().ToList().ForEach(binding => binding.WriteValue());
            }
        }

        //private void textBox1_TextChanged(object sender, EventArgs e)
        //{
        //    this.displayBox1.Text = MyUtility.GetValue.Lookup("Description", Type + this.textBox1.Text.ToString(), "ShippingReason", "Type+ID");
        //}

        private void textBox1_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem
                (string.Format("Select Id, Description from ShippingReason WITH (NOLOCK) where type='{0}' order by id", Type), "10,40", this.textBox1.Text);
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel) { return; }
            this.textBox1.Text = item.GetSelectedString();
            this.Validate();
        }
    }
    public class cellShippingReason : DataGridViewGeneratorTextColumnSettings
    {
        public static DataGridViewGeneratorTextColumnSettings GetGridCell(string ctype)
        {
            cellShippingReason ts = new cellShippingReason();
            // Factory右鍵彈出功能
            ts.EditingMouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    DataGridView grid = ((DataGridViewColumn)s).DataGridView;
                    // Parent form 若是非編輯狀態就 return 
                    if (!((Sci.Win.Forms.Base)grid.FindForm()).EditMode) { return; }
                    DataRow row = grid.GetDataRow<DataRow>(e.RowIndex);
                    SelectItem sele = new SelectItem(string.Format("Select ID,description From ShippingReason WITH (NOLOCK) Where Junk=0 and type = '{0}'", ctype), "10,40", row["ShippingReasonid"].ToString(), false, ",");
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
                String oldValue = row["ShippingReasonid"].ToString();
                String newValue = e.FormattedValue.ToString(); // user 編輯當下的value , 此值尚未存入DataRow

                if (!MyUtility.Check.Empty(newValue) && oldValue != newValue)
                {
                    if (!MyUtility.Check.Seek(ctype+newValue, "ShippingReason", "Type+ID"))
                    {
                        row["ShippingReasonid"] = "";
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
