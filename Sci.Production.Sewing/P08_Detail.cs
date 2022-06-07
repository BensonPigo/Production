using Ict;
using Ict.Win;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using Sci.Data;

namespace Sci.Production.Sewing
{
    /// <inheritdoc/>
    public partial class P08_Detail : Sci.Win.Tems.QueryForm
    {
        private DataRow drData;

        /// <inheritdoc/>
        public int ttlDiscrepancy;

        /// <inheritdoc/>
        public DataTable dtDetail;

        /// <inheritdoc/>
        public P08_Detail(DataRow dr)
        {
            this.EditMode = true;
            this.InitializeComponent();
            this.drData = dr;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            string sqlcmd = "select MDScanUKey,PackingReasonID,Qty,Remarks = '' from MDScan_Detail where 1=0";

            DataTable dtDBSource;
            DualResult result = DBProxy.Current.Select(string.Empty, sqlcmd, out dtDBSource);
            if (result)
            {
                this.listControlBindingSource1.DataSource = dtDBSource;
            }

            this.grid.IsEditingReadOnly = false;
            this.grid.DataSource = this.listControlBindingSource1;
            DataGridViewGeneratorTextColumnSettings col_Remark = new DataGridViewGeneratorTextColumnSettings();
            col_Remark.EditingMouseDown += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                if (e.Button == MouseButtons.Right)
                {
                    DataRow dr = this.grid.GetDataRow(e.RowIndex);
                    string item_cmd = "select ID,Description from PackingReason WHERE Type='MD' AND Junk=0";
                    SelectItem item = new SelectItem(item_cmd, "10,25", dr["Remarks"].ToString());
                    DialogResult dresult = item.ShowDialog();
                    if (dresult == DialogResult.Cancel)
                    {
                        return;
                    }

                    dr["Remarks"] = item.GetSelecteds()[0]["Description"].ToString();
                    dr["PackingReasonID"] = item.GetSelecteds()[0]["ID"].ToString();
                    dr.EndEdit();
                }
            };

            DataGridViewGeneratorNumericColumnSettings col_Discrepancy = new DataGridViewGeneratorNumericColumnSettings();
            col_Discrepancy.CellValidating += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.grid.GetDataRow(e.RowIndex);
                decimal ttlQtyPerCTN = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup($@"
select QtyPerCTN = sum(QtyPerCTN) 
from PackingList_Detail 
where ID = '{this.drData["ID"]}' and CTNStartNo = '{this.drData["CTNStartNo"]}' 
and SCICtnNo = '{this.drData["SCICtnNo"]}' and OrderID = '{this.drData["OrderID"]}'
"));
                dr.EndEdit();
                decimal ttlDiscrepancy = ((DataTable)this.listControlBindingSource1.DataSource).AsEnumerable().Where(w => w.RowState != DataRowState.Deleted).Sum(r => MyUtility.Convert.GetDecimal(r["Qty"]));

                ttlDiscrepancy = ttlDiscrepancy - MyUtility.Convert.GetDecimal(dr["Qty"]) + MyUtility.Convert.GetDecimal(e.FormattedValue);

                if (ttlQtyPerCTN < ttlDiscrepancy)
                {
                    MyUtility.Msg.WarningBox("total Discrepancy cannot more than total Qty Per CTN");
                    e.Cancel = true;
                    return;
                }
            };

            this.Helper.Controls.Grid.Generator(this.grid)
                .Numeric("Qty", header: "Discrepancy", width: Widths.AnsiChars(15), decimal_places: 0, iseditingreadonly: false, settings: col_Discrepancy)
                .Text("Remarks", header: "Remarks", width: Widths.AnsiChars(25), iseditingreadonly: true, settings: col_Remark)
                ;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            // 檢查是否有勾選資料
            this.grid.ValidateControl();
            this.listControlBindingSource1.EndEdit();

            DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
            if (MyUtility.Check.Empty(dt))
            {
                return;
            }

            decimal ttlDiscrepancy = ((DataTable)this.listControlBindingSource1.DataSource).AsEnumerable().Where(w => w.RowState != DataRowState.Deleted).Sum(s => MyUtility.Convert.GetDecimal(s["Qty"]));

            MyUtility.Msg.InfoBox("Save successful!!");
            this.ttlDiscrepancy = MyUtility.Convert.GetInt(ttlDiscrepancy);
            this.dtDetail = dt;
            this.Close();
        }

        private void GridIcon1_AppendClick(object sender, EventArgs e)
        {
            this.grid.ValidateControl();
            this.listControlBindingSource1.EndEdit();

            DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
            if (MyUtility.Check.Empty(dt))
            {
                return;
            }

            DataRow drData = dt.NewRow();
            drData["Qty"] = 0;
            drData["Remarks"] = string.Empty;
            dt.Rows.InsertAt(drData, 0);
            dt.AcceptChanges();
        }

        private void GridIcon1_RemoveClick(object sender, EventArgs e)
        {
            this.grid.ValidateControl();
            this.listControlBindingSource1.EndEdit();
            DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
            if (MyUtility.Check.Empty(dt))
            {
                return;
            }

            DataRow drSelect = this.grid.GetDataRow(this.listControlBindingSource1.Position);
            if (drSelect != null)
            {
                drSelect.Delete();
                dt.AcceptChanges();
            }
        }

        private void GridIcon1_InsertClick(object sender, EventArgs e)
        {
            this.grid.ValidateControl();
            this.listControlBindingSource1.EndEdit();
            DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
            if (MyUtility.Check.Empty(dt))
            {
                return;
            }

            // 處理背後的DataTable
            DataTable gridData = (DataTable)this.listControlBindingSource1.DataSource;
            DataRow nRow = gridData.NewRow();

            // 取得控制項反白的Row，的Index
            DataRow currentGridRow = this.grid.GetDataRow(this.grid.GetSelectedRowIndex());

            // 把被選取的Row，從控制項抓下來丟給背後的DataTable
            nRow = currentGridRow;
            gridData.ImportRow(nRow);
        }
    }
}
