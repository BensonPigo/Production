using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Ict.Win;
using Sci.Data;
using Ict;

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
        /// <param name="data">DataRow</param>
        public P07_UpdateWeight(DataRow data)
        {
            this.InitializeComponent();
            this.dr = data;
            string selectCommand1 = string.Format(@"select * from receiving_detail  WITH (NOLOCK) where id='{0}'", this.dr["id"].ToString());

            DualResult selectResult1 = DBProxy.Current.Select(null, selectCommand1, out this.selectDataTable1);
            if (selectResult1 == false)
            {
                this.ShowErr(selectCommand1, selectResult1);
            }
            else
            {
                // object inqty = selectDataTable1.Compute("sum(inqty)", null);
                // object outqty = selectDataTable1.Compute("sum(outqty)", null);
                // object adjust = selectDataTable1.Compute("sum(adjust)", null);
                // this.numericBox1.Value = !MyUtility.Check.Empty(inqty) ? decimal.Parse(inqty.ToString()) : 0m;
                // this.numericBox2.Value = !MyUtility.Check.Empty(outqty) ? decimal.Parse(outqty.ToString()) : 0m;
                // this.numericBox3.Value = !MyUtility.Check.Empty(adjust) ? decimal.Parse(adjust.ToString()) : 0m;
            }
        }

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
            Ict.Win.UI.DataGridViewComboBoxColumn cbb_fabrictype;

            // 設定Grid1的顯示欄位
            this.gridUpdateAct.IsEditingReadOnly = false; // 必設定, 否則CheckBox會顯示圖示
            this.gridUpdateAct.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridUpdateAct)
                .Text("poid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true) // 0
                .Text("seq1", header: "Seq1", width: Widths.AnsiChars(6), iseditingreadonly: true) // 1
                .Text("seq2", header: "Seq2", width: Widths.AnsiChars(6), iseditingreadonly: true) // 2
                .ComboBox("fabrictype", header: "Material" + Environment.NewLine + "Type", width: Widths.AnsiChars(10), iseditable: false).Get(out cbb_fabrictype) // 3
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
            bool rtn;
            this.gridUpdateAct.ValidateControl();

            if (!(rtn = MyUtility.Tool.CursorUpdateTable(this.selectDataTable1, "dbo." + this.gridAlias, null)))
            {
                MyUtility.Msg.WarningBox("Save failed!!");
                return;
            }

            MyUtility.Msg.InfoBox("Save successful!!");
        }
    }
}
