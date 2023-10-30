﻿using Ict;
using Ict.Win;
using Ict.Win.Defs;
using Sci.Data;
using Sci.Production.Prg;
using Sci.Production.Prg.Entity;
using Sci.Production.PublicPrg;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P48 : Win.Tems.QueryForm
    {
        private DataTable dtInventory;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_ToPoid;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_ToSeq;

        /// <inheritdoc/>
        public P48(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        private void BtnFindNow_Click(object sender, EventArgs e)
        {
            StringBuilder strSQLCmd = new StringBuilder();
            string sp1 = this.txtSPNo1.Text.TrimEnd();
            string sp2 = this.txtSPNo2.Text.TrimEnd();
            string factory = this.txtfactory.Text.TrimEnd();
            string refno = this.txtRef.Text.TrimEnd();
            string location1 = this.txtMtlLocation1.Text.TrimEnd();
            string location2 = this.txtMtlLocation2.Text.TrimEnd();
            string fabrictype = this.txtdropdownlistFabricType.SelectedValue.ToString();
            string strCategory = this.comboCategory.SelectedValue.ToString();
            string brand = this.txtbrand.Text;
            string season = this.txtseason.Text;

            if (string.IsNullOrWhiteSpace(sp1)
                && string.IsNullOrWhiteSpace(sp2)
                && string.IsNullOrWhiteSpace(refno)
                && string.IsNullOrWhiteSpace(location1)
                && string.IsNullOrWhiteSpace(season))
            {
                MyUtility.Msg.WarningBox("< Season > < SP# > < Ref# > < Location > can't be empty!!");
                this.txtSPNo1.Focus();
                return;
            }
            else
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    new SqlParameter("@sp1", sp1),
                    new SqlParameter("@sp2", sp2),
                    new SqlParameter("@factory", factory),
                    new SqlParameter("@refno", refno),
                    new SqlParameter("@location1", location1),
                    new SqlParameter("@location2", location2),
                    new SqlParameter("@fabrictype", fabrictype),
                    new SqlParameter("@strCategory", strCategory),
                    new SqlParameter("@brand", brand),
                    new SqlParameter("@season", season),
                };

                strSQLCmd.Append(string.Format(
                    @"
select  0 as selected 
        , '' id
        , c.PoId
        , a.Seq1
        , a.Seq2
        , concat(Ltrim(Rtrim(a.seq1)), ' ', a.Seq2) as seq
        , dbo.getmtldesc(a.id,a.seq1,a.seq2,2,0) as [Description]
        , [ToPoID] = ''
        , [ToSeq] = ''
        , [ToSeq1] = ''
        , [ToSeq2] = ''
        , c.Roll
        , c.Dyelot
        , c.inqty - c.outqty + c.adjustqty - c.ReturnQty as QtyBefore
        , 0.00 as QtyAfter
        , dbo.Getlocation(c.ukey) as location
        , '' reasonid
        , '' reason_nm
        , a.FabricType
        , a.stockunit
        , c.stockType
        , StockTypeName = case c.StockType 
		        when 'B' then 'Bulk' 
		        when 'I' then 'Inventory' 
		        when 'O' then 'Scrap' 
		        else c.StockType 
		        end
        , c.ukey as ftyinventoryukey
        , [CreateStatus]='' 
        , [FabricTypeName] = (select name from DropDownList where Type='FabricType_Condition' and id=a.fabrictype)		
        , [Category] = case o.Category  when 'B' then 'Bulk'
										when 'M' then 'Material'
										when 'S' then 'Sample'
										when 'T' then 'SMTL' end
		,o.OrderTypeID
		,o.BrandID
        ,[MCHandle] = dbo.getPass1_ExtNo(o.MCHandle)
from dbo.PO_Supp_Detail a WITH (NOLOCK) 
inner join dbo.ftyinventory c WITH (NOLOCK) on c.poid = a.id and c.seq1 = a.seq1 and c.seq2  = a.seq2 and c.stocktype = 'O'
inner join dbo.factory f WITH (NOLOCK) on a.FactoryID=f.id
left join Orders o WITH (NOLOCK) on o.id=a.id
Where   c.lock = 0 
        and c.inqty - c.outqty + c.adjustqty - c.ReturnQty > 0
        and f.mdivisionid = '{0}'        
        ", Env.User.Keyword));

                if (!MyUtility.Check.Empty(sp1))
                {
                    strSQLCmd.Append("and a.id >= @sp1" + Environment.NewLine);
                }

                if (!MyUtility.Check.Empty(sp2))
                {
                    strSQLCmd.Append("and a.id <= @sp2" + Environment.NewLine);
                }

                if (!MyUtility.Check.Empty(refno))
                {
                    strSQLCmd.Append("and a.refno = @refno" + Environment.NewLine);
                }

                if (!MyUtility.Check.Empty(factory))
                {
                    strSQLCmd.Append("and o.FtyGroup = @factory" + Environment.NewLine);
                }

                if (!MyUtility.Check.Empty(brand))
                {
                    strSQLCmd.Append("and o.BrandID = @brand" + Environment.NewLine);
                }

                if (!MyUtility.Check.Empty(season))
                {
                    strSQLCmd.Append("and o.SeasonID = @season" + Environment.NewLine);
                }

                if (!MyUtility.Check.Empty(location1) && !MyUtility.Check.Empty(location2))
                {
                    strSQLCmd.Append(
                        @" 
        and c.ukey in ( select ukey 
                        from dbo.ftyinventory_detail WITH (NOLOCK) 
                        where mtllocationid >= @location1
                        and  mtllocationid <= @location2 ) " + Environment.NewLine);
                }
                else if (!MyUtility.Check.Empty(location1))
                {
                    strSQLCmd.Append(
                        @" 
        and c.ukey in ( select ukey 
                        from dbo.ftyinventory_detail WITH (NOLOCK) 
                        where mtllocationid = @location1) " + Environment.NewLine);
                }
                else if (!MyUtility.Check.Empty(location2))
                {
                    strSQLCmd.Append(
                        @" 
        and c.ukey in ( select ukey 
                        from dbo.ftyinventory_detail WITH (NOLOCK) 
                        where mtllocationid = @location2) " + Environment.NewLine);
                }

                switch (fabrictype)
                {
                    case "ALL":
                        break;
                    case "F":
                        strSQLCmd.Append("And a.fabrictype = 'F'" + Environment.NewLine);
                        break;
                    case "A":
                        strSQLCmd.Append("And a.fabrictype = 'A'" + Environment.NewLine);
                        break;
                }

                strSQLCmd.Append($@" and o.Category in ({strCategory})");

                this.ShowWaitMessage("Data Loading....");
                DualResult result;
                if (result = DBProxy.Current.Select(null, strSQLCmd.ToString(), parameters, out this.dtInventory))
                {
                    if (this.dtInventory.Rows.Count == 0)
                    {
                        MyUtility.Msg.WarningBox("Data not found!!");
                    }
                    else
                    {
                        this.dtInventory.Columns.Add("adjustqty", typeof(decimal));
                        this.dtInventory.Columns["adjustqty"].Expression = "qtybefore-qtyafter";
                        this.dtInventory.DefaultView.Sort = "poid,seq1,seq2,roll,dyelot";
                    }

                    this.listControlBindingSource1.DataSource = this.dtInventory;
                }
                else
                {
                    this.ShowErr(strSQLCmd.ToString(), result);
                }

                this.HideWaitMessage();
            }

            this.btnImport.Enabled = true;
            this.HideWaitMessage();
        }

        /// <inheritdoc/>
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
                        dr["qtyafter"] = 0;
                        return;
                    }
                    else
                    {
                        dr["qtyafter"] = e.FormattedValue;
                        dr["selected"] = true;
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
                    DataRow dr = this.gridImport.GetDataRow(e.RowIndex);
                    if (dr == null)
                    {
                        return;
                    }

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
                    dr["reasonid"] = x[0]["id"];
                    dr["reason_nm"] = x[0]["name"];

                    if (x[0]["id"].Equals("00001") == false)
                    {
                        dr["ToPOID"] = string.Empty;
                        dr["ToSeq"] = string.Empty;
                        dr["ToSeq1"] = string.Empty;
                        dr["ToSeq2"] = string.Empty;
                        this.col_ToPoid.IsEditingReadOnly = true;
                        this.col_ToSeq.IsEditingReadOnly = true;
                    }
                    else
                    {
                        this.col_ToPoid.IsEditingReadOnly = false;
                        this.col_ToSeq.IsEditingReadOnly = false;
                    }

                    dr.EndEdit();
                }
            };
            ts.CellValidating += (s, e) =>
            {
                DataRow dr = this.gridImport.GetDataRow(e.RowIndex);
                if (dr == null)
                {
                    return;
                }

                if (!this.EditMode)
                {
                    return;
                }

                if (string.Compare(e.FormattedValue.ToString(), dr["reasonid"].ToString()) != 0)
                {
                    if (MyUtility.Check.Empty(e.FormattedValue))
                    {
                        dr["reasonid"] = string.Empty;
                        dr["reason_nm"] = string.Empty;
                    }
                    else
                    {
                        DataRow drCheckReason;
                        if (!MyUtility.Check.Seek(
                            string.Format(
                            @"select id, Name from Reason WITH (NOLOCK) where id = '{0}' 
and ReasonTypeID='Stock_Remove' AND junk = 0", e.FormattedValue), out drCheckReason))
                        {
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox("Data not found!", "Reason ID");
                        }
                        else
                        {
                            dr["reasonid"] = e.FormattedValue;
                            dr["reason_nm"] = drCheckReason["name"];
                        }
                    }

                    dr["ToPOID"] = string.Empty;
                    dr["ToSeq"] = string.Empty;
                    dr["ToSeq1"] = string.Empty;
                    dr["ToSeq2"] = string.Empty;
                }

                if (this.gridImport.GetDataRow(this.gridImport.GetSelectedRowIndex())["reasonid"].ToString().Equals("00001") == false)
                {
                    this.col_ToPoid.IsEditingReadOnly = true;
                    this.col_ToSeq.IsEditingReadOnly = true;
                }
                else
                {
                    this.col_ToPoid.IsEditingReadOnly = false;
                    this.col_ToSeq.IsEditingReadOnly = false;
                }

                dr.EndEdit();
            };
            #endregion
            #region ToPoid Seq
            DataGridViewGeneratorTextColumnSettings cs_topoid = new DataGridViewGeneratorTextColumnSettings();
            cs_topoid.CellValidating += (s, e) =>
            {
                DataRow dr = this.gridImport.GetDataRow(e.RowIndex);
                if (!this.EditMode)
                {
                    return; // 非編輯模式
                }

                if (e.RowIndex == -1)
                {
                    return; // 沒東西 return
                }

                if (dr == null)
                {
                    return;
                }

                if (MyUtility.Check.Empty(e.FormattedValue))
                {
                    dr["ToPOID"] = string.Empty;
                    dr["ToSeq1"] = string.Empty;
                    dr["ToSeq2"] = string.Empty;
                    dr["ToSeq"] = string.Empty;
                    dr.EndEdit();
                    return;
                }

                string sqlchk = $@"
select * from View_WH_Orders 
where ID = '{e.FormattedValue}'
and MDivisionID = '{Sci.Env.User.Keyword}'";
                if (!MyUtility.Check.Seek(sqlchk))
                {
                    MyUtility.Msg.WarningBox($"<Bulk SP#>:{e.FormattedValue} not found");
                    e.Cancel = true;
                    return;
                }
                else
                {
                    string oldValue = dr["ToPOID"].ToString();
                    string newValue = e.FormattedValue.ToString();
                    if (oldValue != newValue)
                    {
                        dr["ToSeq1"] = string.Empty;
                        dr["ToSeq2"] = string.Empty;
                        dr["ToSeq"] = string.Empty;
                    }

                    dr["ToPoID"] = e.FormattedValue;
                    dr.EndEdit();
                }
            };

            DataGridViewGeneratorTextColumnSettings cs_toSeq = new DataGridViewGeneratorTextColumnSettings();
            cs_toSeq.EditingMouseDown += (s, e) =>
            {
                DataRow dr = this.gridImport.GetDataRow(e.RowIndex);
                if (dr == null)
                {
                    return;
                }

                if (dr["reasonid"].Equals("00001") == false)
                {
                    return;
                }

                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    string sqlcmd = string.Empty;
                    IList<DataRow> x;
                    if (MyUtility.Check.Empty(dr["ToPoID"]))
                    {
                        MyUtility.Msg.WarningBox("Please fill in 'Bulk SP#' first.");
                        dr["ToSeq"] = string.Empty;
                        dr["ToSeq1"] = string.Empty;
                        dr["ToSeq2"] = string.Empty;
                        dr.EndEdit();
                        return;
                    }
                    else
                    {
                        sqlcmd = $@"
select ID,[Seq] = concat(Seq1,' ',Seq2),seq1,seq2
from Po_Supp_Detail 
where ID = '{dr["ToPoID"]}'
";

                        DBProxy.Current.Select(null, sqlcmd, out DataTable poitems);

                        string columns = "ID,Seq";
                        string headersercap = "POID,Seq,";
                        string columnwidths = "14,10";
                        SelectItem item = new SelectItem(poitems, columns, columnwidths, dr["seq"].ToString(), headersercap)
                        {
                            Width = 500,
                        };
                        DialogResult result = item.ShowDialog();
                        if (result == DialogResult.Cancel)
                        {
                            return;
                        }

                        x = item.GetSelecteds();
                    }

                    dr["ToSeq"] = x[0]["seq"];
                    dr["ToSeq1"] = x[0]["seq1"];
                    dr["ToSeq2"] = x[0]["seq2"];

                    dr.EndEdit();
                }
            };
            cs_toSeq.CellValidating += (s, e) =>
            {
                DataRow dr = this.gridImport.GetDataRow(e.RowIndex);
                if (!this.EditMode)
                {
                    return; // 非編輯模式
                }

                if (e.RowIndex == -1)
                {
                    return; // 沒東西 return
                }

                if (dr == null)
                {
                    return;
                }

                if (MyUtility.Check.Empty(dr["ToPoID"]))
                {
                    MyUtility.Msg.WarningBox("Please fill in 'Bulk SP#' first.");
                    dr["ToSeq"] = string.Empty;
                    dr["ToSeq1"] = string.Empty;
                    dr["ToSeq2"] = string.Empty;
                    dr.EndEdit();
                    return;
                }

                string oldvalue = MyUtility.Convert.GetString(dr["ToSeq"]);
                string newvalue = MyUtility.Convert.GetString(e.FormattedValue);
                if (oldvalue == newvalue)
                {
                    return;
                }

                if (string.Compare(e.FormattedValue.ToString(), dr["ToSeq"].ToString()) != 0)
                {
                    if (MyUtility.Check.Empty(e.FormattedValue))
                    {
                        dr["ToSeq"] = string.Empty;
                        dr["ToSeq1"] = string.Empty;
                        dr["ToSeq2"] = string.Empty;
                    }
                    else
                    {
                        // check Seq Length
                        string[] seq = e.FormattedValue.ToString().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if (seq.Length < 2)
                        {
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox("Data not found!", "Seq");
                            return;
                        }

                        string sqlchk = $@"
select * from Po_Supp_Detail 
where ID = '{dr["ToPoID"]}'
and seq1 = '{seq[0]}'
and seq2 = '{seq[1]}'
";
                        if (!MyUtility.Check.Seek(sqlchk, out DataRow drCheck))
                        {
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox("Data not found!", "Seq");
                            return;
                        }
                        else
                        {
                            dr["Toseq"] = e.FormattedValue;
                            dr["Toseq1"] = seq[0];
                            dr["Toseq2"] = seq[1];
                        }
                    }
                }

                dr.EndEdit();
            };
            #endregion
            this.gridImport.IsEditingReadOnly = false; // 必設定, 否則CheckBox會顯示圖示
            this.gridImport.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridImport)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
                .Text("poid", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(14))
                .Text("seq", header: "Seq#", iseditingreadonly: true, width: Widths.AnsiChars(6))
                .Text("OrderTypeID", header: "Order Type", iseditingreadonly: true, width: Widths.AnsiChars(13))
                .Text("BrandID", header: "Brand", iseditingreadonly: true, width: Widths.AnsiChars(7))
                .Text("roll", header: "Roll", iseditingreadonly: true, width: Widths.AnsiChars(6))
                .Text("dyelot", header: "Dyelot", iseditingreadonly: true, width: Widths.AnsiChars(8))
                .EditText("Description", header: "Description", iseditingreadonly: true, width: Widths.AnsiChars(20))
                .Text("FabricTypeName", header: "Material Type", iseditingreadonly: true, width: Widths.AnsiChars(8))
                .Text("Category", header: "Category", iseditingreadonly: true, width: Widths.AnsiChars(8))
                .Numeric("QtyBefore", header: "Original Qty", iseditable: true, decimal_places: 2, integer_places: 10, width: Widths.AnsiChars(6))
                .Numeric("QtyAfter", header: "Current Qty", decimal_places: 2, integer_places: 10, settings: ns, width: Widths.AnsiChars(6))
                .Numeric("adjustqty", header: "Remove Qty", decimal_places: 2, integer_places: 10, width: Widths.AnsiChars(6))
                .Text("location", header: "Location", iseditingreadonly: true, width: Widths.AnsiChars(6))
                .Text("reasonid", header: "Reason ID", settings: ts, width: Widths.AnsiChars(6))
                .Text("reason_nm", header: "Reason Name", iseditingreadonly: true, width: Widths.AnsiChars(20))
                .Text("MCHandle", header: "MC Handle", iseditingreadonly: true, width: Widths.AnsiChars(20))
                .Text("CreateStatus", header: "Create Status", iseditingreadonly: true, width: Widths.AnsiChars(50))
                .Text("ToPOID", header: "Bulk SP#", width: Widths.AnsiChars(15), iseditingreadonly: true, settings: cs_topoid).Get(out this.col_ToPoid)
                .Text("ToSeq", header: "Bulk Seq", width: Widths.AnsiChars(8), iseditingreadonly: true, settings: cs_toSeq).Get(out this.col_ToSeq)
               ;

            this.gridImport.Columns["QtyAfter"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridImport.Columns["reasonid"].DefaultCellStyle.BackColor = Color.Pink;
            this.comboCategory.SelectedIndex = 4;

            // 設定detailGrid Rows 是否可以編輯
            this.gridImport.RowEnter += this.Detailgrid_RowEnter;
        }

        private void Detailgrid_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || !this.EditMode || this.gridImport.GetSelectedRowIndex() < 0)
            {
                return;
            }

            var data = ((DataRowView)this.gridImport.Rows[e.RowIndex].DataBoundItem).Row;
            if (data == null)
            {
                return;
            }

            if (data["reasonid"].ToString().Equals("00001") == true)
            {
                this.col_ToPoid.IsEditingReadOnly = false;
                this.col_ToSeq.IsEditingReadOnly = false;
            }
            else
            {
                this.col_ToPoid.IsEditingReadOnly = true;
                this.col_ToSeq.IsEditingReadOnly = true;
            }
        }

        // Close
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
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
                if (reasonid.Equals("00001") == false)
                {
                    item["ToPOID"] = string.Empty;
                    item["ToSeq"] = string.Empty;
                    item["ToSeq1"] = string.Empty;
                    item["ToSeq2"] = string.Empty;
                }
            }
        }

        // Create Batch
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
            DataTable warningTB = new DataTable();
            if (dr2.Length > 0)
            {
                warningTB.Columns.Add("SpNo");
                warningTB.Columns.Add("SEQ");
                warningTB.Columns.Add("Roll");
                foreach (DataRow drReason in dr2)
                {
                    DataRow drNoReason = warningTB.NewRow();
                    drNoReason["SpNo"] = drReason["POID"].ToString();
                    drNoReason["SEQ"] = drReason["seq"].ToString();
                    drNoReason["Roll"] = drReason["Roll"].ToString();
                    warningTB.Rows.Add(drNoReason);
                }

                if (warningTB.Rows.Count > 0)
                {
                    var m = MyUtility.Msg.ShowMsgGrid(warningTB, "These SP#'s Reason ID cannot be empty!", "Warning");
                    m.Width = 650;
                    m.grid1.Columns[0].Width = 200;
                    m.grid1.Columns[0].Width = 50;
                    m.grid1.Columns[0].Width = 100;
                    m.text_Find.Width = 150;
                    m.btn_Find.Location = new Point(170, 6);

                    m.btn_Find.Anchor = AnchorStyles.Left | AnchorStyles.Top;
                    return;
                }
            }

            #region Batch Create

            dr2 = dtGridBS1.Select("adjustqty <> 0 and Selected = 1");

            // 建立前檢查重複物料
            var duplicateData = dr2.AsEnumerable()
                .GroupBy(g => new
                {
                    POID = MyUtility.Convert.GetString(g["poid"]),
                    SEQ1 = MyUtility.Convert.GetString(g["seq1"]),
                    SEQ2 = MyUtility.Convert.GetString(g["seq2"]),
                    Roll = MyUtility.Convert.GetString(g["roll"]),
                    Dyelot = MyUtility.Convert.GetString(g["dyelot"]),
                }).Select(s => new
                {
                    s.Key.POID,
                    s.Key.SEQ1,
                    s.Key.SEQ2,
                    s.Key.Roll,
                    s.Key.Dyelot,
                    Count = s.Count(),
                }).Where(w => w.Count > 1).ToList();
            if (duplicateData.Any())
            {
                MyUtility.Msg.ShowMsgGrid_LockScreen(ListToDataTable.ToDataTable(duplicateData), "These data's duplicate!");
                return;
            }

            // *依照POID 批次建立P45 ID
            var listPoid = dr2.Select(row => row["Poid"]).Distinct().ToList();
            var tmpId = MyUtility.GetValue.GetBatchID(Env.User.Keyword + "AM", "Adjust", DateTime.Now, batchNumber: listPoid.Count);
            if (MyUtility.Check.Empty(tmpId))
            {
                MyUtility.Msg.WarningBox("Get document id fail!");
                return;
            }

            this.ShowWaitMessage("Data Creating....");
            #region insert Table

            string insertMaster = @"
insert into Adjust
        (id      , type   , issuedate, mdivisionid, FactoryID
         , status, addname, adddate  , remark)
select   id      , type   , issuedate, mdivisionid, FactoryID
         , status, addname, adddate   , remark
from #tmp";
            string insertDetail = @"
insert into Adjust_Detail
(ID, FtyInventoryUkey ,MDivisionID ,POID ,Seq1 ,Seq2 ,Roll    
,Dyelot ,StockType ,QtyBefore ,QtyAfter ,ReasonID,ToPoid,ToSeq1,ToSeq2 )
select ID, FtyInventoryUkey ,MDivisionID ,POID ,Seq1 ,Seq2 ,Roll    
,Dyelot ,StockType ,QtyBefore ,QtyAfter ,ReasonID,ToPoid,ToSeq1,ToSeq2
from #tmp";

            DataTable dtMaster = new DataTable();
            dtMaster.Columns.Add("ID");
            dtMaster.Columns.Add("MdivisionID");
            dtMaster.Columns.Add("FactoryID");
            dtMaster.Columns.Add("IssueDate");
            dtMaster.Columns.Add("Status");
            dtMaster.Columns.Add("AddName");
            dtMaster.Columns.Add("AddDate");
            dtMaster.Columns.Add("Type");
            dtMaster.Columns.Add("Remark");
            dtMaster.Columns.Add("Poid");

            DataTable dtDetail = new DataTable();
            dtDetail.Columns.Add("ID");
            dtDetail.Columns.Add("FtyInventoryUkey");
            dtDetail.Columns.Add("MDivisionID");
            dtDetail.Columns.Add("POID");
            dtDetail.Columns.Add("Seq1");
            dtDetail.Columns.Add("Seq2");
            dtDetail.Columns.Add("Roll");
            dtDetail.Columns.Add("Dyelot");
            dtDetail.Columns.Add("StockType");
            dtDetail.Columns.Add("QtyBefore");
            dtDetail.Columns.Add("QtyAfter");
            dtDetail.Columns.Add("ReasonID");
            dtDetail.Columns.Add("Location");
            dtDetail.Columns.Add("StockTypeName");
            dtDetail.Columns.Add("ToPOID");
            dtDetail.Columns.Add("ToSeq1");
            dtDetail.Columns.Add("ToSeq2");

            for (int i = 0; i < listPoid.Count; i++)
            {
                DataRow drNewMaster = dtMaster.NewRow();
                drNewMaster["poid"] = listPoid[i].ToString();
                drNewMaster["id"] = tmpId[i].ToString();
                drNewMaster["type"] = "R";
                drNewMaster["issuedate"] = DateTime.Now.ToString("yyyy/MM/dd");
                drNewMaster["mdivisionid"] = Env.User.Keyword;
                drNewMaster["FactoryID"] = Env.User.Factory;
                drNewMaster["status"] = "New";
                drNewMaster["addname"] = Env.User.UserID;
                drNewMaster["adddate"] = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff");
                drNewMaster["remark"] = "Batch create by P48";
                dtMaster.Rows.Add(drNewMaster);
            }

            foreach (DataRow item in dr2)
            {
                DataRow[] drGetID = dtMaster.AsEnumerable().Where(row => row["poid"].EqualString(item["POID"])).ToArray();
                DataRow drNewDetail = dtDetail.NewRow();
                drNewDetail["ID"] = drGetID[0]["ID"];
                drNewDetail["FtyInventoryUkey"] = item["FtyInventoryUkey"];
                drNewDetail["MDivisionID"] = Env.User.Keyword;
                drNewDetail["POID"] = item["POID"];
                drNewDetail["Seq1"] = item["Seq1"];
                drNewDetail["Seq2"] = item["Seq2"];
                drNewDetail["Roll"] = item["Roll"];
                drNewDetail["Dyelot"] = item["Dyelot"];
                drNewDetail["StockType"] = item["StockType"];
                drNewDetail["QtyBefore"] = item["QtyBefore"];
                drNewDetail["QtyAfter"] = item["QtyAfter"];
                drNewDetail["ReasonID"] = item["ReasonID"];
                drNewDetail["Location"] = item["Location"];
                drNewDetail["StockTypeName"] = item["StockTypeName"];
                drNewDetail["ToPOID"] = item["ToPOID"];
                drNewDetail["ToSeq1"] = item["ToSeq1"];
                drNewDetail["ToSeq2"] = item["ToSeq2"];
                dtDetail.Rows.Add(drNewDetail);
            }

            #endregion

            #region 檢查Location是否為空值
            if (MyUtility.Check.Seek(@"select 1 from System where WH_MtlTransChkLocation = 1"))
            {
                DataTable dtLocationEmpty = new DataTable();
                dtLocationEmpty.Columns.Add("POID");
                dtLocationEmpty.Columns.Add("Seq");
                dtLocationEmpty.Columns.Add("Roll");
                dtLocationEmpty.Columns.Add("Dyelot");
                dtLocationEmpty.Columns.Add("StockType");
                foreach (DataRow dr in dtDetail.Rows)
                {
                    if (MyUtility.Check.Empty(dr["Location"]))
                    {
                        DataRow drEmpty = dtLocationEmpty.NewRow();
                        drEmpty["POID"] = dr["POID"];
                        drEmpty["Seq"] = dr["Seq1"] + " " + dr["Seq2"];
                        drEmpty["Roll"] = dr["Roll"];
                        drEmpty["Dyelot"] = dr["Dyelot"];
                        drEmpty["StockType"] = dr["StockTypeName"];
                        dtLocationEmpty.Rows.Add(drEmpty);
                    }
                }

                if (dtLocationEmpty.Rows.Count > 0)
                {
                    // change column name
                    dtLocationEmpty.Columns["PoId"].ColumnName = "SP#";
                    dtLocationEmpty.Columns["seq"].ColumnName = "Seq";
                    dtLocationEmpty.Columns["Roll"].ColumnName = "Roll";
                    dtLocationEmpty.Columns["Dyelot"].ColumnName = "Dyelot";
                    dtLocationEmpty.Columns["StockType"].ColumnName = "Stock Type";

                    Prgs.ChkLocationEmpty(dtLocationEmpty, string.Empty, @"SP#,Seq,Roll,Dyelot,Stock Type");
                    this.HideWaitMessage();
                    return;
                }
            }
            #endregion

            #region TransactionScope
            TransactionScope transactionscope = new TransactionScope();
            DualResult result;
            using (transactionscope)
            {
                try
                {
                    DataTable dtResult;
                    if ((result = MyUtility.Tool.ProcessWithDatatable(dtMaster, null, insertMaster, out dtResult)) == false)
                    {
                        transactionscope.Dispose();
                        MyUtility.Msg.WarningBox(result.ToString(), "Create failed");
                        return;
                    }

                    if ((result = MyUtility.Tool.ProcessWithDatatable(dtDetail, null, insertDetail, out dtResult)) == false)
                    {
                        transactionscope.Dispose();
                        MyUtility.Msg.WarningBox(result.ToString(), "Create failed");
                        return;
                    }

                    transactionscope.Complete();
                }
                catch (Exception ex)
                {
                    transactionscope.Dispose();
                    this.ShowErr("Commit transaction error.", ex);
                    return;
                }
            }
            #endregion

            // Create後Btn失效，需重新Qurey才能再使用。
            this.btnImport.Enabled = false;

            #region Confirmed
            if (tmpId.Count < 1)
            {
                return;
            }

            for (int i = 0; i < listPoid.Count; i++)
            {
                string detailbyIDCmd = $"select * from Adjust_Detail with(nolock) where id = '{tmpId[i]}'";
                result = DBProxy.Current.Select(null, detailbyIDCmd, out DataTable dtDetail_Ukeys);
                if (!result)
                {
                    this.ShowErr(result);
                    return;
                }

                // 取得 FtyInventory 資料 (包含PO_Supp_Detail.FabricType)
                result = Prgs.GetFtyInventoryData(dtDetail_Ukeys, "P45", out DataTable dtOriFtyInventory);
                if (!result)
                {
                    this.ShowErr(result);
                    return;
                }

                // 檢查負數庫存
                if (!(result = Prgs.GetAdjustSumBalance(tmpId[i].ToString(), true, out DataTable datacheck)))
                {
                    this.ShowErr(result);
                    return;
                }

                if (datacheck.Rows.Count > 0)
                {
                    // 將負數庫存資訊顯示在 Grid 上
                    foreach (DataRow drs in datacheck.Rows)
                    {
                        foreach (var item in this.dtInventory.Select($@"poid='{drs["POID"]}' and seq1='{drs["seq1"]}' and seq2='{drs["seq2"]}' and Roll = '{drs["Roll"]}' and Dyelot = '{drs["Dyelot"]}'"))
                        {
                            item["CreateStatus"] = $@"{tmpId[i]}'s balance: {drs["balance"]} is less than Adjust qty: {drs["Adjustqty"]} Balacne Qty is not enough!!";
                        }
                    }
                }
                else
                {
                    if (!(result = Prgs.UpdateScrappAdjustFtyInventory(tmpId[i].ToString(), isConfirm: true)))
                    {
                        foreach (var item in this.dtInventory.Select($"poid='{listPoid[i]}' and selected=1"))
                        {
                            item["CreateStatus"] = $"{tmpId[i]} Confirmed Fail! " + result.ToString();
                        }
                    }

                    if (!(result = DBProxy.Current.Execute(null, $"update Adjust set status = 'Confirmed', editname = '{Env.User.UserID}', editdate = GETDATE() where id = '{tmpId[i]}'")))
                    {
                        foreach (var item in this.dtInventory.Select($"poid='{listPoid[i]}' and selected=1"))
                        {
                            item["CreateStatus"] = $"{tmpId[i]} Confirmed Fail! " + result.ToString();
                        }
                    }

                    // Barcode 需要判斷新的庫存, 在更新 FtyInventory 之後
                    if (!(result = Prgs.UpdateWH_Barcode(true, dtDetail_Ukeys, "P45", out bool fromNewBarcode, dtOriFtyInventory)))
                    {
                        foreach (var item in this.dtInventory.Select($"poid='{listPoid[i]}' and selected=1"))
                        {
                            item["CreateStatus"] = $"{tmpId[i]} Confirmed Fail! " + result.ToString();
                        }
                    }

                    // AutoWHFabric WebAPI
                    Prgs_WMS.WMSprocess(false, dtDetail_Ukeys, "P45", EnumStatus.New, EnumStatus.Confirm, dtOriFtyInventory);
                    foreach (var item in this.dtInventory.Select($"poid='{listPoid[i]}' and selected=1"))
                    {
                        item["CreateStatus"] = $"{tmpId[i]} Create and Confirm Success ";
                    }
                }
            }
            #endregion

            MyUtility.Msg.InfoBox("Create Successful.");

            #endregion
            this.HideWaitMessage();
        }
    }
}
