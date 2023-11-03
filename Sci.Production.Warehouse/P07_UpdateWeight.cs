using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P07_UpdateWeight : Win.Subs.Base
    {
        private DataRow dr;
        private DataTable selectDataTable1;
        private string gridAlias;
        private Dictionary<string, string> di_fabrictype = new Dictionary<string, string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="P07_UpdateWeight"/> class.
        /// </summary>
        /// <param name="data">Main DataTable</param>
        /// <param name="data2">OID</param>
        /// <param name="gridAlias">GridAlias</param>
        public P07_UpdateWeight(object data, string data2, string gridAlias)
        {
            this.InitializeComponent();
            this.selectDataTable1 = (DataTable)data;
            this.Text += " - " + data2;
            this.EditMode = true;
            this.gridAlias = gridAlias;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.di_fabrictype.Add("F", "Fabric");
            this.di_fabrictype.Add("A", "Accessory");
            this.listControlBindingSource1.DataSource = this.selectDataTable1;

            // 設定Grid1的顯示欄位
            this.gridUpdateAct.IsEditingReadOnly = false; // 必設定, 否則CheckBox會顯示圖示
            this.gridUpdateAct.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridUpdateAct)
                .Text("poid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true) // 0
                .Text("seq1", header: "Seq1", width: Widths.AnsiChars(6), iseditingreadonly: true) // 1
                .Text("seq2", header: "Seq2", width: Widths.AnsiChars(6), iseditingreadonly: true) // 2
                .ComboBox("fabrictype", header: "Material" + Environment.NewLine + "Type", width: Widths.AnsiChars(10), iseditable: false).Get(out Ict.Win.UI.DataGridViewComboBoxColumn cbb_fabrictype) // 3
                .Text("Roll", header: "Roll#", width: Widths.AnsiChars(9), iseditingreadonly: true) // 4
                .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true) // 5
                .Numeric("shipqty", header: "Ship Qty", width: Widths.AnsiChars(13), decimal_places: 2, integer_places: 10, iseditingreadonly: true) // 6
                .Text("pounit", header: "Purchase" + Environment.NewLine + "Unit", width: Widths.AnsiChars(9), iseditingreadonly: true) // 7
                .Numeric("weight", header: "G.W(kg)", width: Widths.AnsiChars(9), decimal_places: 2, integer_places: 7) // 8
                .Numeric("actualweight", header: "Act.(kg)", width: Widths.AnsiChars(9), decimal_places: 2, integer_places: 7) // 9
                ;

            cbb_fabrictype.DataSource = new BindingSource(this.di_fabrictype, null);
            cbb_fabrictype.ValueMember = "Key";
            cbb_fabrictype.DisplayMember = "Value";

            this.gridUpdateAct.Columns["actualweight"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridUpdateAct.Columns["weight"].DefaultCellStyle.BackColor = Color.Pink;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            this.gridUpdateAct.ValidateControl();
            DualResult result = DBProxy.Current.GetTableSchema(null, this.gridAlias, out ITableSchema schema);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            Exception errMsg = null;
            using (TransactionScope transactionscope = new TransactionScope(TransactionScopeOption.Required, new TimeSpan(0, 15, 0)))
            {
                try
                {
                    foreach (DataRow row in this.selectDataTable1.Rows)
                    {
                        if (!(result = DBProxy.Current.UpdateByChanged(null, schema, row, out bool ischanged)))
                        {
                            throw result.GetException();
                        }
                    }

                    transactionscope.Complete();
                }
                catch (Exception ex)
                {
                    errMsg = ex;
                }
            }

            if (errMsg != null)
            {
                this.ShowErr(errMsg);
                return;
            }

            MyUtility.Msg.InfoBox("Save successful!!");
        }
    }
}
