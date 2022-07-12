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

        private bool QtyCanEdit;
        private Ict.Win.UI.DataGridViewNumericBoxColumn col_Qty;

        /// <inheritdoc/>
        public P08_Detail(DataRow dr, DataTable dtHistory)
        {
            this.EditMode = true;
            this.InitializeComponent();
            this.drData = dr;
            this.dtDetail = dtHistory;
            this.QtyCanEdit = dtHistory.Rows.Count > 0 ? false : true;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            if (this.dtDetail == null || this.dtDetail.Columns.Count == 0)
            {
                string sqlcmd = $@"    select MDScanUKey		,PackingReasonID		,Qty		,Remarks =  (select Description from PackingReason WHERE Type='MD' AND Junk=0 and id = md.PackingReasonID)               ,CanEdit = 0    from MDScan_Detail md     where md.MDScanUKey = (        select top 1 m.Ukey        from MDScan m        where exists(			select 1 from PackingList_Detail pd 			where (pd.ID = '{this.drData["ID"]}'             and pd.CTNStartNo = '{this.drData["CTNStartNo"]}')			and  pd.ID = m.PackingListID and pd.OrderID = m.OrderID  and pd.CTNStartNo = m.CTNStartNo and pd.MDStatus <> 'Pass')        order by m.AddDate desc    )";
                DualResult result = DBProxy.Current.Select(string.Empty, sqlcmd, out DataTable dtDBSource);
                if (result)
                {
                    this.listControlBindingSource1.DataSource = dtDBSource;
                    this.QtyCanEdit = dtDBSource.Rows.Count > 0 ? false : true;
                }
                else
                {
                    string sqlcmd2 = $@"        select MDScanUKey,PackingReasonID,Qty,Remarks =  '',CanEdit = 1        from MDScan_Detail        where 1=0    )";
                    DualResult result2 = DBProxy.Current.Select(string.Empty, sqlcmd2, out DataTable dtDBSource2);
                    if (result2)
                    {
                        this.listControlBindingSource1.DataSource = dtDBSource2;
                    }
                }
            }
            else
            {
                this.listControlBindingSource1.DataSource = this.dtDetail.Copy();
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
                    string item_cmd = $"select ID,Description from PackingReason WHERE Type='MD' AND Junk=0 ";
                    SelectItem item = new SelectItem(item_cmd, "10,25", dr["Remarks"].ToString());
                    DialogResult dresult = item.ShowDialog();
                    if (dresult == DialogResult.Cancel)
                    {
                        return;
                    }

                    dr["Remarks"] = item.GetSelecteds()[0]["Description"].ToString();
                    dr["PackingReasonID"] = item.GetSelecteds()[0]["ID"].ToString();

                    DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
                    if (dt.AsEnumerable().Where(w => w["Remarks"].ToString() == dr["Remarks"].ToString()).Count() > 1)
                    {
                        MyUtility.Msg.WarningBox("[Remarks] cannot be repeat!!");
                        dr["Remarks"] = string.Empty;
                        dr["PackingReasonID"] = string.Empty;
                    }

                    dr.EndEdit();
                }
            };

            col_Remark.CellValidating += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.grid.GetDataRow(e.RowIndex);
                DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
                if (dt.AsEnumerable().Where(w => w["Remarks"].ToString() != dr["Remarks"].ToString() && w["Remarks"].ToString() == e.FormattedValue.ToString()).Any())
                {
                    MyUtility.Msg.WarningBox("[Remarks] cannot be repeat!!");
                    e.Cancel = true;
                    return;
                }

                dr.EndEdit();

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
                .Numeric("Qty", header: "Discrepancy", width: Widths.AnsiChars(15), decimal_places: 0, iseditingreadonly: false, settings: col_Discrepancy).Get(out this.col_Qty)
                .Text("Remarks", header: "Remarks", width: Widths.AnsiChars(25), iseditingreadonly: true, settings: col_Remark)
                ;

            this.grid.RowEnter += this.Grid_RowEnter;
        }

        private void Grid_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            var data = ((DataRowView)this.grid.Rows[e.RowIndex].DataBoundItem).Row;
            if (data == null)
            {
                return;
            }

            if (this.QtyCanEdit == false && !MyUtility.Check.Empty(data["Qty"]) && MyUtility.Check.Empty(data["CanEdit"]))
            {
                this.col_Qty.IsEditingReadOnly = true;
            }
            else
            {
                this.col_Qty.IsEditingReadOnly = false;
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CloseFunc()
        {
            if (this.dtDetail == null || this.dtDetail.Columns.Count == 0)
            {
                // 按下Close 代表return 原本資料回上一層
                string sqlcmd = $@"    select MDScanUKey		,PackingReasonID		,Qty		,Remarks =  (select Description from PackingReason WHERE Type='MD' AND Junk=0 and id = md.PackingReasonID)               ,CanEdit = 0    from MDScan_Detail md     where md.MDScanUKey = (        select top 1 m.Ukey        from MDScan m        where exists(			select 1 from PackingList_Detail pd 			where (pd.ID = '{this.drData["ID"]}'             and pd.CTNStartNo = '{this.drData["CTNStartNo"]}')			and  pd.ID = m.PackingListID and pd.OrderID = m.OrderID  and pd.CTNStartNo = m.CTNStartNo and pd.MDStatus <> 'Pass')        order by m.AddDate desc    )";
                DualResult result = DBProxy.Current.Select(string.Empty, sqlcmd, out DataTable dtDBSource);
                if (result)
                {
                    this.dtDetail = dtDBSource;
                }
            }

            if (this.dtDetail.Rows.Count == 0)
            {
                this.ttlDiscrepancy = 0;
                return;
            }
            else
            {
                decimal ttlDiscrepancy = this.dtDetail.AsEnumerable().Where(w => w.RowState != DataRowState.Deleted).Sum(s => MyUtility.Convert.GetDecimal(s["Qty"]));
                this.ttlDiscrepancy = MyUtility.Convert.GetInt(ttlDiscrepancy);
                return;
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            this.grid.ValidateControl();
            this.listControlBindingSource1.EndEdit();

            DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
            if (MyUtility.Check.Empty(dt))
            {
                return;
            }

            if (dt.AsEnumerable().Where(w => MyUtility.Check.Empty(w["Remarks"])).Any())
            {
                MyUtility.Msg.WarningBox("Remarks cannot be empty!");
                return;
            }

            foreach (DataRow drChk in dt.Rows)
            {
                if (dt.AsEnumerable().Where(w => w["PackingReasonID"] == drChk["PackingReasonID"]).Count() > 1)
                {
                    MyUtility.Msg.WarningBox("[Remarks] cannot be repeat!!");
                    return;
                }
            }

            decimal ttlDiscrepancy = ((DataTable)this.listControlBindingSource1.DataSource).AsEnumerable().Where(w => w.RowState != DataRowState.Deleted).Sum(s => MyUtility.Convert.GetDecimal(s["Qty"]));

            MyUtility.Msg.InfoBox("Save successful!!");
            this.ttlDiscrepancy = MyUtility.Convert.GetInt(ttlDiscrepancy);
            foreach (DataRow dr in dt.Rows)
            {
                dr["CanEdit"] = 0;
            }

            this.dtDetail = dt.Copy();
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
            drData["CanEdit"] = 1;
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

            if (this.grid.CurrentDataRow != null)
            {
                this.grid.CurrentDataRow.Delete();
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

        private void P08_Detail_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.CloseFunc();
        }
    }
}
