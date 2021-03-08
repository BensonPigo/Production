using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P45_Import : Win.Subs.Base
    {
        private DataRow dr_master;
        private DataTable dt_detail;
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        private DataTable dtInventory;

        /// <inheritdoc/>
        public P45_Import(DataRow master, DataTable detail)
        {
            this.InitializeComponent();
            this.dr_master = master;
            this.dt_detail = detail;
        }

        // Find Now Button
        private void BtnFindNow_Click(object sender, EventArgs e)
        {
            StringBuilder strSQLCmd = new StringBuilder();
            string sp = this.txtSPNo.Text.TrimEnd();
            string refno = this.txtRef.Text.TrimEnd();
            string location = this.txtLocation.Text.TrimEnd();
            string fabrictype = this.txtdropdownlistFabricType.SelectedValue.ToString();

            if (string.IsNullOrWhiteSpace(sp) && string.IsNullOrWhiteSpace(refno) && string.IsNullOrWhiteSpace(location))
            {
                MyUtility.Msg.WarningBox("< SP# > < Ref# > < Location > can't be empty!!");
                this.txtSPNo.Focus();
                return;
            }
            else
            {
                strSQLCmd.Append(string.Format(
                    @"
select  0 as selected 
        , '' id
        , c.PoId
        , a.Seq1
        , a.Seq2
        , concat(Ltrim(Rtrim(a.seq1)), ' ', a.Seq2) as seq
        , dbo.getmtldesc(a.id,a.seq1,a.seq2,2,0) as [Description]
        , c.Roll
        , c.Dyelot
        , c.inqty - c.outqty + c.adjustqty - c.ReturnQty as QtyBefore
        , 0.00 as QtyAfter
        , c.inqty - c.outqty + c.adjustqty - c.ReturnQty as adjustqty
        , dbo.Getlocation(c.ukey) as location
        , '' reasonid
        , '' reason_nm
        , a.FabricType
        , a.stockunit
        , c.stockType
        , c.ukey as ftyinventoryukey
        , ColorID =dbo.GetColorMultipleID(a.BrandId, a.ColorID)
from dbo.PO_Supp_Detail a WITH (NOLOCK) 
inner join dbo.ftyinventory c WITH (NOLOCK) on c.poid = a.id and c.seq1 = a.seq1 and c.seq2  = a.seq2 and c.stocktype = 'O'
inner join dbo.factory f WITH (NOLOCK) on a.FactoryID=f.id
Where   c.lock = 0 
        and c.inqty - c.outqty + c.adjustqty - c.ReturnQty <> 0
        and f.mdivisionid = '{0}'", Env.User.Keyword));

                if (!MyUtility.Check.Empty(sp))
                {
                    strSQLCmd.Append(string.Format(
                        @" 
        and a.id = '{0}' ", sp));
                }

                if (!MyUtility.Check.Empty(refno))
                {
                    strSQLCmd.Append(string.Format(
                        @" 
        and a.refno = '{0}' ", refno));
                }

                if (!MyUtility.Check.Empty(location))
                {
                    strSQLCmd.Append(string.Format(
                        @" 
        and c.ukey in ( select ukey 
                        from dbo.ftyinventory_detail WITH (NOLOCK) 
                        where mtllocationid = '{0}') ", location));
                }

                if (!this.txtSeq.CheckSeq1Empty())
                {
                    strSQLCmd.Append(string.Format(
                        @"
        and a.seq1 = '{0}'", this.txtSeq.Seq1));
                }

                if (!this.txtSeq.CheckSeq2Empty())
                {
                    strSQLCmd.Append(string.Format(
                        @" 
        and a.seq2 = '{0}'", this.txtSeq.Seq2));
                }

                switch (fabrictype)
                {
                    case "ALL":
                        break;
                    case "F":
                        strSQLCmd.Append(@" 
        And a.fabrictype = 'F'");
                        break;
                    case "A":
                        strSQLCmd.Append(@" 
        And a.fabrictype = 'A'");
                        break;
                }

                this.ShowWaitMessage("Data Loading....");
                DualResult result;
                if (result = DBProxy.Current.Select(null, strSQLCmd.ToString(), out this.dtInventory))
                {
                    if (this.dtInventory.Rows.Count == 0)
                    {
                        MyUtility.Msg.WarningBox("Data not found!!");
                    }
                    else
                    {
                        this.dtInventory.DefaultView.Sort = "seq1,seq2,location,dyelot";
                    }

                    this.listControlBindingSource1.DataSource = this.dtInventory;
                }
                else
                {
                    this.ShowErr(strSQLCmd.ToString(), result);
                }

                this.HideWaitMessage();
            }
        }

        // Form Load

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            #region -- Reason Combox --
            string selectCommand = @"select Name idname,id from Reason WITH (NOLOCK) where ReasonTypeID='Stock_Remove' AND junk = 0";
            DualResult returnResult;
            DataTable dropDownListTable = new DataTable();
            if (returnResult = DBProxy.Current.Select(null, selectCommand, out dropDownListTable))
            {
                this.comboReason.DataSource = dropDownListTable;
                this.comboReason.DisplayMember = "IDName";
                this.comboReason.ValueMember = "ID";
            }
            #endregion
            #region -- Current Qty Valid --
            DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();
            ns.CellValidating += (s, e) =>
            {
                if (this.EditMode && !MyUtility.Check.Empty(e.FormattedValue))
                {
                    DataRow dr = this.gridImport.GetDataRow(e.RowIndex);

                    if (MyUtility.Convert.GetDecimal(dr["QtyBefore"]) - MyUtility.Convert.GetDecimal(e.FormattedValue) <= 0)
                    {
                        dr["QtyAfter"] = MyUtility.Convert.GetDecimal(dr["QtyBefore"]) - MyUtility.Convert.GetDecimal(dr["AdjustQty"]);
                        return;
                    }
                    else
                    {
                        dr["QtyAfter"] = e.FormattedValue;
                        dr["AdjustQty"] = MyUtility.Convert.GetDecimal(dr["QtyBefore"]) - MyUtility.Convert.GetDecimal(dr["QtyAfter"]);
                        dr["selected"] = true;
                        dr.EndEdit();
                    }
                }
            };
            #endregion
            #region -- Remove Qty Valid --
            DataGridViewGeneratorNumericColumnSettings adjustqty = new DataGridViewGeneratorNumericColumnSettings();
            adjustqty.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.gridImport.GetDataRow(e.RowIndex);
                    if (MyUtility.Convert.GetDecimal(dr["QtyBefore"]) - MyUtility.Convert.GetDecimal(e.FormattedValue) < 0 ||
                        MyUtility.Check.Empty(e.FormattedValue))
                    {
                        dr["AdjustQty"] = MyUtility.Convert.GetDecimal(dr["QtyBefore"]) - MyUtility.Convert.GetDecimal(dr["QtyAfter"]);
                        return;
                    }
                    else
                    {
                        dr["AdjustQty"] = e.FormattedValue;
                        dr["QtyAfter"] = MyUtility.Convert.GetDecimal(dr["QtyBefore"]) - MyUtility.Convert.GetDecimal(dr["AdjustQty"]);
                        dr["selected"] = true;
                        dr.EndEdit();
                    }
                }
            };
            #endregion
            #region -- Reason ID 右鍵開窗 --
            DataGridViewGeneratorTextColumnSettings ts = new DataGridViewGeneratorTextColumnSettings();
            ts.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    DataTable poitems;
                    string sqlcmd = string.Empty;
                    IList<DataRow> x;

                    sqlcmd = @"select id, Name from Reason WITH (NOLOCK) where ReasonTypeID='Stock_Remove' AND junk = 0";
                    DualResult result2 = DBProxy.Current.Select(null, sqlcmd, out poitems);
                    if (!result2)
                    {
                        this.ShowErr(sqlcmd, result2);
                        return;
                    }

                    Win.Tools.SelectItem item = new Win.Tools.SelectItem(
                        poitems,
                        "ID,Name",
                        "5,150",
                        this.gridImport.GetDataRow(this.gridImport.GetSelectedRowIndex())["reasonid"].ToString(),
                        "ID,Name");
                    item.Width = 600;
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    x = item.GetSelecteds();

                    this.gridImport.GetDataRow(this.gridImport.GetSelectedRowIndex())["reasonid"] = x[0]["id"];
                    this.gridImport.GetDataRow(this.gridImport.GetSelectedRowIndex())["reason_nm"] = x[0]["name"];
                }
            };
            ts.CellValidating += (s, e) =>
            {
                DataRow dr;
                if (!this.EditMode)
                {
                    return;
                }

                if (string.Compare(e.FormattedValue.ToString(), this.gridImport.GetDataRow(this.gridImport.GetSelectedRowIndex())["reasonid"].ToString()) != 0)
                {
                    if (MyUtility.Check.Empty(e.FormattedValue))
                    {
                        this.gridImport.GetDataRow(this.gridImport.GetSelectedRowIndex())["reasonid"] = string.Empty;
                        this.gridImport.GetDataRow(this.gridImport.GetSelectedRowIndex())["reason_nm"] = string.Empty;
                    }
                    else
                    {
                        if (!MyUtility.Check.Seek(
                            string.Format(
                            @"select id, Name from Reason WITH (NOLOCK) where id = '{0}' 
and ReasonTypeID='Stock_Remove' AND junk = 0", e.FormattedValue), out dr, null))
                        {
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox("Data not found!", "Reason ID");
                            return;
                        }
                        else
                        {
                            this.gridImport.GetDataRow(this.gridImport.GetSelectedRowIndex())["reasonid"] = e.FormattedValue;
                            this.gridImport.GetDataRow(this.gridImport.GetSelectedRowIndex())["reason_nm"] = dr["name"];
                        }
                    }
                }
            };
            #endregion

            this.gridImport.IsEditingReadOnly = false; // 必設定, 否則CheckBox會顯示圖示
            this.gridImport.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridImport)
            .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk) // 0
            .Text("poid", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(14)) // 1
            .Text("seq", header: "Seq#", iseditingreadonly: true, width: Widths.AnsiChars(6)) // 2
            .Text("roll", header: "Roll", iseditingreadonly: true, width: Widths.AnsiChars(6)) // 4
            .Text("dyelot", header: "Dyelot", iseditingreadonly: true, width: Widths.AnsiChars(8)) // 5
            .Text("ColorID", header: "Color", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .EditText("Description", header: "Description", iseditingreadonly: true, width: Widths.AnsiChars(20)) // 3
            .Numeric("QtyBefore", header: "Original Qty", iseditable: true, decimal_places: 2, integer_places: 10, width: Widths.AnsiChars(6)) // 6
            .Numeric("QtyAfter", header: "Current Qty", decimal_places: 2, integer_places: 10, settings: ns, width: Widths.AnsiChars(6)) // 7
            .Numeric("AdjustQty", header: "Remove Qty", decimal_places: 2, integer_places: 10, settings: adjustqty, width: Widths.AnsiChars(6)) // 8
            .Text("location", header: "Location", iseditingreadonly: true, width: Widths.AnsiChars(6)) // 9
            .Text("reasonid", header: "Reason ID", settings: ts, width: Widths.AnsiChars(6)) // 10
            .Text("reason_nm", header: "Reason Name", iseditingreadonly: true, width: Widths.AnsiChars(20)) // 11
            ;

            this.gridImport.Columns["QtyAfter"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridImport.Columns["reasonid"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridImport.Columns["AdjustQty"].DefaultCellStyle.BackColor = Color.Pink;
        }

        // Close
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Import
        private void BtnImport_Click(object sender, EventArgs e)
        {
            this.gridImport.ValidateControl();
            DataTable dtGridBS1 = (DataTable)this.listControlBindingSource1.DataSource;
            if (MyUtility.Check.Empty(dtGridBS1) || dtGridBS1.Rows.Count == 0)
            {
                return;
            }

            DataRow[] dr2 = dtGridBS1.Select("Selected = 1");
            if (dr2.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select rows first!", "Warnning");
                return;
            }

            dr2 = dtGridBS1.Select("adjustqty = 0 and Selected = 1");
            if (dr2.Length > 0)
            {
                MyUtility.Msg.WarningBox("Adjust Qty of selected row can't be zero!", "Warning");
                return;
            }

            dr2 = dtGridBS1.Select("reasonid = '' and Selected = 1");
            if (dr2.Length > 0)
            {
                MyUtility.Msg.WarningBox("Reason ID of selected row can't be empty!", "Warning");
                return;
            }

            dr2 = dtGridBS1.Select("adjustqty <> 0 and Selected = 1");
            foreach (DataRow tmp in dr2)
            {
                DataRow[] findrow = this.dt_detail.Select(string.Format("ftyinventoryukey = {0}", tmp["ftyinventoryukey"]));

                if (findrow.Length > 0)
                {
                    findrow[0]["qtybefore"] = tmp["qtybefore"];
                    findrow[0]["qtyafter"] = tmp["qtyafter"];
                    findrow[0]["adjustqty"] = tmp["adjustqty"];
                }
                else
                {
                    tmp["id"] = this.dr_master["id"];
                    tmp.AcceptChanges();
                    tmp.SetAdded();
                    this.dt_detail.ImportRow(tmp);
                }
            }

            this.Close();
        }

        // SP# Valid
        private void TxtSPNo_Validating(object sender, CancelEventArgs e)
        {
// string sp = textBox1.Text.TrimEnd();

// if (MyUtility.Check.Empty(sp)) return;

// if (txtSeq1.checkEmpty(showErrMsg: false))
//            {
//                if (!MyUtility.Check.Seek(string.Format("select 1 where exists(select * from MdivisionPoDetail WITH (NOLOCK) where poid ='{0}')",sp), null))
//                {
//                    MyUtility.Msg.WarningBox("SP# is not found!!");
//                    e.Cancel = true;
//                    return;
//                }
//            }
//            else
//            {
//                if (!MyUtility.Check.Seek(string.Format(@"select 1 where exists(select * from MdivisionPoDetail WITH (NOLOCK) where poid ='{0}'
//                        and seq1 = '{1}' and seq2 = '{2}')", sp, txtSeq1.seq1, txtSeq1.seq2), null))
//                {
//                    MyUtility.Msg.WarningBox("SP#-Seq is not found!!");
//                    e.Cancel = true;
//                    return;
//                }
//            }
        }

        // Update All
        private void BtnUpdateAll_Click(object sender, EventArgs e)
        {
            string reasonid = this.comboReason.SelectedValue.ToString();
            this.gridImport.ValidateControl();

            if (this.dtInventory == null || this.dtInventory.Rows.Count == 0)
            {
                return;
            }

            DataRow[] drfound = this.dtInventory.Select("selected = 1");

            foreach (var item in drfound)
            {
                item["reasonid"] = reasonid;
                item["reason_nm"] = this.comboReason.Text;
            }
        }

        private void TxtLocation_Validating(object sender, CancelEventArgs e)
        {
            if (this.txtLocation.Text.ToString() == string.Empty)
            {
                return;
            }

            if (!MyUtility.Check.Seek(
                string.Format(
                @"
select 1 
where exists(
    select * 
    from    dbo.MtlLocation WITH (NOLOCK) 
    where   StockType='O' 
            and id = '{0}'
            and junk != '1'
)", this.txtLocation.Text), null))
            {
                e.Cancel = true;
                MyUtility.Msg.WarningBox("Location is not exist!!", "Data not found");
            }
        }

        // Location  右鍵
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        private void TxtLocation_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            if (!this.EditMode)
            {
                return;
            }

            Win.Tools.SelectItem item = new Win.Tools.SelectItem(
                string.Format(@"
select  id
        , [Description] 
from    dbo.MtlLocation WITH (NOLOCK) 
where   StockType='O'
        and junk != '1'"), "10,40", this.txtLocation.Text, "ID,Desc");
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.txtLocation.Text = item.GetSelectedString();
        }
    }
}
