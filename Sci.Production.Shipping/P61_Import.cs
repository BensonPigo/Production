using Ict;
using Ict.Win;
using Sci.Production.Prg;
using Sci.Win.Tools;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Sci.Production.Shipping
{
    /// <inheritdoc/>
    public partial class P61_Import : Sci.Win.Subs.Base
    {
        private DataTable dt_detail;
        private DataTable dt_p61_detail;

        /// <inheritdoc/>
        public P61_Import(DataTable detail, DataTable deleteDT, DataTable p61_detail)
        {
            this.InitializeComponent();
            this.dt_p61_detail = p61_detail;

            if (deleteDT != null)
            {
                detail.Merge(deleteDT);
            }

            // 取得要排除的 KHCustomsItemUkey 列表
            List<string> p61Ids = p61_detail.AsEnumerable()
                .Where(row => row.RowState != DataRowState.Deleted)
                .Select(row => row["KHCustomsItemUkey"].ToString()).ToList();

            // 使用 LINQ 排除這些 KHCustomsItemUkey
            var filteredRows = detail.AsEnumerable().Where(row => !p61Ids.Contains(row["KHCustomsItemUkey"].ToString()));

            if (filteredRows.Any())
            {
                this.dt_detail = filteredRows.CopyToDataTable();
            }
            else
            {
                this.dt_detail = detail;
            }

            this.btnImport.Enabled = false;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.grid1.DataSource = this.listControlBindingSource1;
            this.grid1.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.grid1)
               .CheckBox("selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
               .Text("CustomsType", header: "Customs Type", iseditingreadonly: true, width: Widths.AnsiChars(8))
               .Text("vk_Refno", header: "RefNo", width: Widths.AnsiChars(15), iseditingreadonly: true)
               .Text("RefNo", header: "SCI RefNo", iseditingreadonly: true, width: Widths.AnsiChars(20))
               .Text("Description", header: "Description", iseditingreadonly: true, width: Widths.AnsiChars(40))
               .Text("Qty", header: "Q'ty", iseditingreadonly: true, width: Widths.AnsiChars(13))
               .Text("UnitID", header: "Unit", width: Widths.AnsiChars(5), iseditingreadonly: true)
               ;
        }

        /// <inheritdoc/>
        private void TxtCustomsType_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            var distinctRows = this.dt_detail.AsEnumerable()
                            .GroupBy(row => row["CustomsType"])
                            .Select(group => group.First());
            DataTable distinctTable = distinctRows.Any() ? distinctRows.CopyToDataTable() : null;
            SelectItem2 sele;
            sele = new SelectItem2(distinctTable, "CustomsType", "CustomsType", "23", null);
            DialogResult result = sele.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.txtCustomsType.Text = sele.GetSelectedString().Replace(",", ",");
        }

        /// <inheritdoc/>
        private void BtnQuery_Click(object sender, System.EventArgs e)
        {
            List<string> filterValues = this.txtCustomsType.Text.Split(',').Select(x => x.Trim()).ToList();
            var filteredRows = this.dt_detail.AsEnumerable().Where(row => filterValues.Contains(row["CustomsType"].ToString()));

            if (filteredRows.Any())
            {
                DataTable filteredTable = filteredRows.CopyToDataTable();

                // 更新 "Selected" 欄位的值為 1
                filteredTable.AsEnumerable().ToList().ForEach(row => row["Selected"] = 1);

                this.listControlBindingSource1.DataSource = filteredTable;
                this.btnImport.Enabled = true;
            }
            else
            {
                this.listControlBindingSource1.DataSource = null;
                this.btnImport.Enabled = false;
            }
        }

        private void BtnCancel_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void BtnImport_Click(object sender, System.EventArgs e)
        {
            this.grid1.ValidateControl();

            DataTable dtGridBS1 = (DataTable)this.listControlBindingSource1.DataSource;
            if (MyUtility.Check.Empty(dtGridBS1) || dtGridBS1.Rows.Count == 0)
            {
                return;
            }

            DataRow[] dr2 = dtGridBS1.Select("Selected = 1");
            if (dr2.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please query <Customs Type> and select RefNo!", "Warnning");
                return;
            }

            foreach (DataRow tmp in dr2)
            {
                DataRow[] findrow = this.dt_p61_detail.AsEnumerable()
                    .Where(row => row.RowState != DataRowState.Deleted
                    && row["KHCustomsItemUkey"].EqualString(tmp["KHCustomsItemUkey"].ToString())
                    && row["ExportID"].EqualString(tmp["ExportID"].ToString())
                    ).ToArray();
                if (findrow.Length == 0)
                {
                     this.dt_p61_detail.ImportRowAdded(tmp);
                }
            }

            this.Close();
        }
    }
}
