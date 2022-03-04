using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Data;
using System.Windows.Forms;

namespace Sci.Production.Logistic
{
    /// <inheritdoc/>
    public partial class P16 : Win.Tems.QueryForm
    {
        /// <inheritdoc/>
        public P16(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.EditMode = true;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.grid1.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.grid1)
                .CheckBox("selected", header: string.Empty, width: Widths.AnsiChars(3), trueValue: 1, falseValue: 0, iseditable: true)
                .Text("ID", header: "SP#", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("CustPONo", header: "PO#", width: Widths.AnsiChars(14), iseditingreadonly: true)
                .Text("StyleID", header: "Style", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("BrandID", header: "Brand", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Date("BuyerDelivery", header: "Buyer Dlv.", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Date("SCIDelivery", header: "SCI Dlv.", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .CheckBox("BrokenNeedles", header: "Broken needles", width: Widths.AnsiChars(3), trueValue: 1, falseValue: 0, iseditable: true)
                .Button("Detail", null, header: "Detail", width: Widths.AnsiChars(6), onclick: this.BtnDetail_Click)
            ;
        }

        private void BtnDetail_Click(object sender, EventArgs e)
        {
            DataRow dr = this.grid1.GetDataRow(this.listControlBindingSource1.Position);

            P16_BrokenNeedlesRecord callForm = new P16_BrokenNeedlesRecord(this.IsSupportEdit, dr);
            callForm.ShowDialog(this);
        }

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.txtSP.Text) &&
                MyUtility.Check.Empty(this.txtPONo.Text) &&
                MyUtility.Check.Empty(this.dateBuyerDelivery.Value1) &&
                MyUtility.Check.Empty(this.dateSCIDelivery.Value1))
            {
                MyUtility.Msg.WarningBox("Msg : SP#, PO#, Buyer Delivery and SCI Delivery cannot all be empty.");
                return;
            }

            string where = string.Empty;
            if (!MyUtility.Check.Empty(this.txtSP.Text))
            {
                where += $"\r\nand o.id = '{this.txtSP.Text}'";
            }

            if (!MyUtility.Check.Empty(this.txtPONo.Text))
            {
                where += $"\r\nand o.CustPONo = '{this.txtPONo.Text}'";
            }

            if (!MyUtility.Check.Empty(this.txtbrand.Text))
            {
                where += $"\r\nand o.BrandID = '{this.txtbrand.Text}'";
            }

            if (!MyUtility.Check.Empty(this.comboDropDownList1.Text))
            {
                where += $"\r\nand o.Category in ({this.comboDropDownList1.SelectedValue})";
            }

            if (!MyUtility.Check.Empty(this.dateBuyerDelivery.Value1))
            {
                where += $"\r\nand o.BuyerDelivery between '{((DateTime)this.dateBuyerDelivery.Value1).ToString("yyyy/MM/dd")}' and '{((DateTime)this.dateBuyerDelivery.Value2).ToString("yyyy/MM/dd")}'";
            }

            if (!MyUtility.Check.Empty(this.dateSCIDelivery.Value1))
            {
                where += $"\r\nand o.SciDelivery between '{((DateTime)this.dateSCIDelivery.Value1).ToString("yyyy/MM/dd")}' and '{((DateTime)this.dateSCIDelivery.Value2).ToString("yyyy/MM/dd")}'";
            }

            string sqlcmd = $@"
select selected = cast(0 as bit), o.ID, o.CustPONo, o.StyleID, o.BrandID, o.BuyerDelivery, o.SciDelivery, o.BrokenNeedles
from Orders o
where 1=1
and o.MDivisionID = '{Sci.Env.User.Keyword}'
{where}
";
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out DataTable dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.listControlBindingSource1.DataSource = dt;
            if (dt.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
            if (dt == null)
            {
                return;
            }

            if (dt.Select("selected = 1").Length == 0)
            {
                MyUtility.Msg.WarningBox("Msg : Please select data first.");
                return;
            }

            DataTable dt2 = dt.Select("selected = 1").CopyToDataTable();
            string sqlcmd = $@"
update o
set o.BrokenNeedles = t.BrokenNeedles
from orders o
inner join #tmp t on t.id = o.id
";
            DualResult result = MyUtility.Tool.ProcessWithDatatable(dt2, "id,BrokenNeedles", sqlcmd, out DataTable dt3);
            if (!result)
            {
                this.ShowErr(result);
            }

            MyUtility.Msg.InfoBox("Success");
        }
    }
}
