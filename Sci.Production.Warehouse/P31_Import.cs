using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.PublicPrg;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P31_Import : Win.Subs.Base
    {
        private DataRow dr_master;
        private DataTable dt_detail;
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        private Ict.Win.UI.DataGridViewNumericBoxColumn col_qty;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_roll;

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        protected DataTable dtBorrow;
        private Dictionary<string, string> di_stocktype = new Dictionary<string, string>();

        /// <inheritdoc/>
        public P31_Import(DataRow master, DataTable detail)
        {
            this.InitializeComponent();
            this.dr_master = master;
            this.dt_detail = detail;
            this.di_stocktype.Add("B", "Bulk");
            this.di_stocktype.Add("I", "Inventory");
        }

        // Find Now Button
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        private void BtnFindNow_Click(object sender, EventArgs e)
        {
            StringBuilder strSQLCmd = new StringBuilder();
            string sp = this.txtToSP.Text.TrimEnd();
            string fromSP = this.txtBorrowFromSP.Text.TrimEnd();

            if (string.IsNullOrWhiteSpace(sp) || this.txtSeq.CheckSeq1Empty()
                || this.txtSeq.CheckSeq2Empty() || string.IsNullOrWhiteSpace(fromSP))
            {
                MyUtility.Msg.WarningBox("< To SP# Seq> <From SP#> can't be empty!!");
                this.txtToSP.Focus();
                return;
            }
            else
            {
                #region Get SizeSpec, Refno, Color, Desc
                string strSQL = $@"
select  SizeSpec= isnull(psdsS.SpecValue, '')
        , psd.Refno
        , ColorID = isnull(psdsC.SpecValue, '')
        , f.Description
from PO_Supp_Detail psd
left join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
left join PO_Supp_Detail_Spec psdsS WITH (NOLOCK) on psdsS.ID = psd.id and psdsS.seq1 = psd.seq1 and psdsS.seq2 = psd.seq2 and psdsS.SpecColumnID = 'Size'
left join Fabric f on psd.SCIRefno = f.SCIRefno
LEFT JOIN Orders o ON o.ID=psd.ID
where   psd.id = '{sp}' 
        and psd.seq1 = '{this.txtSeq.Seq1}' 
        and psd.seq2='{this.txtSeq.Seq2}'
		AND o.Category<>'A'
";

                DBProxy.Current.Select(null, strSQL, out DataTable dt);
                if (dt != null && dt.Rows.Count > 0)
                {
                    this.displaySizeSpec.Text = dt.Rows[0]["SizeSpec"].ToString();
                    this.displayRefno.Text = dt.Rows[0]["Refno"].ToString();
                    this.displayColorID.Text = dt.Rows[0]["ColorID"].ToString();
                    this.editDesc.Text = dt.Rows[0]["Description"].ToString();
                }
                else
                {
                    this.displaySizeSpec.Text = string.Empty;
                    this.displayRefno.Text = string.Empty;
                    this.displayColorID.Text = string.Empty;
                    this.editDesc.Text = string.Empty;
                }
                #endregion

                // 建立可以符合回傳的Cursor
                strSQLCmd.Append($@"
select  selected = 0
        ,id = '' 
        ,FromRoll = c.Roll 
        ,FromDyelot = c.Dyelot 
        ,fromftyinventoryukey = c.ukey  
        ,FromPoId = psd.id 
        ,FromSeq1 = psd.Seq1 
        ,FromSeq2 = psd.Seq2 
        ,FromFactoryID = orders.FactoryID
        ,fromseq = concat(Ltrim(Rtrim(psd.seq1)), ' ', psd.Seq2) 
        ,[Description] = dbo.getmtldesc(psd.id,psd.seq1,psd.seq2,2,0) 
        ,psd.stockunit
        ,fromstocktype = c.StockType 
        ,balance = c.InQty - c.OutQty + c.AdjustQty - c.ReturnQty
        ,location = dbo.Getlocation(c.ukey)
        ,toseq = concat(Ltrim(Rtrim(b.seq1)), ' ', b.Seq2) 
        ,toroll = c.Roll 
        ,todyelot = c.Dyelot 
        ,Qty = 0.00 
        ,ToStocktype = 'B' 
        ,topoid = b.id 
        ,toseq1 = b.seq1 
        ,toseq2 = b.seq2 
        ,toFactoryID = (select FactoryID from Orders where b.id = Orders.id)
        ,psd.fabrictype
        ,c.Lock
        ,ToLocation = ''
        ,Fromlocation = Fromlocation.listValue
        ,c.Tone
from dbo.PO_Supp_Detail psd WITH (NOLOCK) 
inner join dbo.ftyinventory c WITH (NOLOCK) on c.poid = psd.id and c.seq1 = psd.seq1 and c.seq2  = psd.seq2 
inner join Orders on c.poid = Orders.id
inner join Factory on Orders.FactoryID = Factory.ID
left join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
left join PO_Supp_Detail_Spec psdsS WITH (NOLOCK) on psdsS.ID = psd.id and psdsS.seq1 = psd.seq1 and psdsS.seq2 = psd.seq2 and psdsS.SpecColumnID = 'Size'
outer apply(
    select psd2.*
    from po_supp_detail psd2 WITH (NOLOCK)
    left join PO_Supp_Detail_Spec psdsC2 WITH (NOLOCK) on psdsC2.ID = psd2.id and psdsC2.seq1 = psd2.seq1 and psdsC2.seq2 = psd2.seq2 and psdsC2.SpecColumnID = 'Color'
    left join PO_Supp_Detail_Spec psdsS2 WITH (NOLOCK) on psdsS2.ID = psd2.id and psdsS2.seq1 = psd2.seq1 and psdsS2.seq2 = psd2.seq2 and psdsS2.SpecColumnID = 'Size'
    where psd2.Refno = psd.Refno
    and psd2.BrandId = psd.BrandId
    and isnull(psdsC2.SpecValue, '') = isnull(psdsC.SpecValue, '')
    and isnull(psdsS2.SpecValue, '') = isnull(psdsS.SpecValue, '')
)b
outer apply(
	select listValue = Stuff((
			select concat(',',MtlLocationID)
			from (
					select 	distinct
						fd.MtlLocationID
					from FtyInventory_Detail fd
					left join MtlLocation ml on ml.ID = fd.MtlLocationID
					where fd.Ukey = c.Ukey
					and ml.Junk = 0 
					and ml.StockType = 'B'
				) s
			for xml path ('')
		) , 1, 1, '')
)Fromlocation
Where psd.id = '{fromSP}' and b.id = '{sp}' and b.seq1 = '{this.txtSeq.Seq1}' 
and b.seq2='{this.txtSeq.Seq2}' and Factory.MDivisionID = '{Env.User.Keyword}'
and c.InQty - c.OutQty + c.AdjustQty - c.ReturnQty > 0 and  c.StockType='B'
AND Orders.Category <> 'A'
");

                this.ShowWaitMessage("Data Loading....");
                DualResult result;
                if (result = DBProxy.Current.Select(null, strSQLCmd.ToString(), out this.dtBorrow))
                {
                    if (this.dtBorrow.Rows.Count == 0)
                    {
                        MyUtility.Msg.WarningBox("Data not found!!");
                        this.txtToSP.Text = string.Empty;
                        this.txtSeq.Seq1 = string.Empty;
                        this.txtSeq.Seq2 = string.Empty;
                        this.txtBorrowFromSP.Text = string.Empty;
                    }

                    this.listControlBindingSource1.DataSource = this.dtBorrow;
                    this.Grid_Filter();
                    if (this.gridImport.Rows.Count == 0)
                    {
                        this.chkNoLock.Checked = false;
                        this.Grid_Filter();
                    }

                    this.dtBorrow.DefaultView.Sort = "fromseq1,fromseq2,location,fromdyelot,balance desc";
                }
                else
                {
                    this.ShowErr(strSQLCmd.ToString(), result);
                }

                this.ChangeColor();
                this.HideWaitMessage();
            }
        }

        private void Sum_checkedqty()
        {
            this.listControlBindingSource1.EndEdit();
            DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
            object localPrice = dt.Compute("Sum(qty)", "selected = 1");
            this.displayTotalQty.Value = localPrice.ToString();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();
            ns.CellValidating += (s, e) =>
                {
                    if (this.EditMode && !MyUtility.Check.Empty(e.FormattedValue))
                    {
                        this.gridImport.GetDataRow(this.gridImport.GetSelectedRowIndex())["qty"] = e.FormattedValue;
                        this.gridImport.GetDataRow(this.gridImport.GetSelectedRowIndex())["selected"] = true;
                        this.Sum_checkedqty();
                    }
                };

            DataGridViewGeneratorTextColumnSettings toLocation = new DataGridViewGeneratorTextColumnSettings();
            toLocation.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    DataRow dr = this.gridImport.GetDataRow<DataRow>(e.RowIndex);

                    SelectItem2 selectItem2 = Prgs.SelectLocation("B", MyUtility.Convert.GetString(dr["ToLocation"]));

                    selectItem2.ShowDialog();
                    if (selectItem2.DialogResult == DialogResult.OK)
                    {
                        dr["ToLocation"] = selectItem2.GetSelecteds().Select(o => MyUtility.Convert.GetString(o["ID"])).JoinToString(",");
                    }

                    dr.EndEdit();
                }
            };

            toLocation.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.gridImport.GetDataRow<DataRow>(e.RowIndex);
                    string oldValue = dr["ToLocation"].ToString();
                    string newValue = e.FormattedValue.ToString().Split(',').ToList().Where(o => !MyUtility.Check.Empty(o)).Distinct().JoinToString(",");
                    if (oldValue.Equals(newValue))
                    {
                        return;
                    }

                    string notLocationExistsList = newValue.Split(',').ToList().Where(o => !Prgs.CheckLocationExists("B", o)).JoinToString(",");

                    if (!MyUtility.Check.Empty(notLocationExistsList))
                    {
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox($"ToLocation<{notLocationExistsList}> not Found");
                        return;
                    }
                    else
                    {
                        dr["ToLocation"] = newValue;
                        dr.EndEdit();
                    }
                }
            };

            this.gridImport.CellValueChanged += (s, e) =>
            {
                if (this.gridImport.Columns[e.ColumnIndex].Name == this.col_chk.Name)
                {
                    DataRow dr = this.gridImport.GetDataRow(e.RowIndex);
                    if (Convert.ToBoolean(dr["selected"]) == true && Convert.ToDecimal(dr["qty"].ToString()) == 0)
                    {
                        dr["qty"] = dr["balance"];
                    }
                    else if (Convert.ToBoolean(dr["selected"]) == false)
                    {
                        dr["qty"] = 0;
                    }

                    dr.EndEdit();
                    this.Sum_checkedqty();
                }
            };

            this.gridImport.IsEditingReadOnly = false; // 必設定, 否則CheckBox會顯示圖示
            this.gridImport.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridImport)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk) // 0
                .Text("fromroll", header: "Roll", iseditingreadonly: true, width: Widths.AnsiChars(6)) // 1
                .Text("fromdyelot", header: "Dyelot", iseditingreadonly: true, width: Widths.AnsiChars(8)) // 2
                .Text("fromseq", header: "From" + Environment.NewLine + "Seq#", iseditingreadonly: true, width: Widths.AnsiChars(6)) // 3
                .EditText("Description", header: "Description", iseditingreadonly: true, width: Widths.AnsiChars(25)) // 4
                .Text("StockUnit", header: "Unit", iseditingreadonly: true) // 5
                .ComboBox("fromstocktype", header: "From" + Environment.NewLine + "Stock" + Environment.NewLine + "Type", iseditable: false).Get(out Ict.Win.UI.DataGridViewComboBoxColumn cbb_stocktype) // 6
                .Numeric("balance", header: "Stock Qty", iseditingreadonly: true, decimal_places: 2, integer_places: 10) // 7
                .Text("location", header: "From Location", iseditingreadonly: true) // 8
                .Text("toseq", header: "To" + Environment.NewLine + "Seq#", iseditingreadonly: true, width: Widths.AnsiChars(6)) // 9
                .Text("Tone", header: "Shade Band" + Environment.NewLine + "Tone/Grp", iseditingreadonly: true, width: Widths.AnsiChars(6)) // 9
                .Text("toroll", header: "To" + Environment.NewLine + "Roll", width: Widths.AnsiChars(6)).Get(out this.col_roll) // 10
                .Numeric("qty", header: "Issue" + Environment.NewLine + "Qty", decimal_places: 2, integer_places: 10, settings: ns).Get(out this.col_qty) // 11
                .Text("ToLocation", header: "To Location", width: Widths.AnsiChars(10), settings: toLocation)
               ;

            this.gridImport.Columns["toroll"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridImport.Columns["qty"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridImport.Columns["ToLocation"].DefaultCellStyle.BackColor = Color.Pink;
            cbb_stocktype.DataSource = new BindingSource(this.di_stocktype, null);
            cbb_stocktype.ValueMember = "Key";
            cbb_stocktype.DisplayMember = "Value";
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Import
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        private void BtnImport_Click(object sender, EventArgs e)
        {
            StringBuilder warningmsg = new StringBuilder();

            this.gridImport.ValidateControl();
            this.gridImport.EndEdit();

            if (MyUtility.Check.Empty(this.dtBorrow) || this.dtBorrow.Rows.Count == 0)
            {
                return;
            }

            DataRow[] dr2 = this.dtBorrow.Select("Selected = 1");
            if (dr2.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select rows first!", "Warnning");
                return;
            }

            dr2 = this.dtBorrow.Select("qty = 0 and Selected = 1");
            if (dr2.Length > 0)
            {
                MyUtility.Msg.WarningBox("Qty of selected row can't be zero!", "Warning");
                return;
            }

            dr2 = this.dtBorrow.Select("qty <> 0 and Selected = 1");
            foreach (DataRow row in dr2)
            {
                if (row["fabrictype"].ToString().ToUpper() == "F" && (MyUtility.Check.Empty(row["toroll"]) || MyUtility.Check.Empty(row["todyelot"])))
                {
                    warningmsg.Append($@"To SP#: {row["topoid"]} To Seq#: {row["toseq1"]}-{row["toseq2"]} To Roll#:{row["toroll"]} To Dyelot:{row["todyelot"]} Roll and Dyelot can't be empty" + Environment.NewLine);
                }

                if (row["fabrictype"].ToString().ToUpper() != "F")
                {
                    row["toroll"] = string.Empty;
                    row["todyelot"] = string.Empty;
                }
            }

            if (!MyUtility.Check.Empty(warningmsg.ToString()))
            {
                MyUtility.Msg.WarningBox(warningmsg.ToString());
                return;
            }

            foreach (DataRow tmp in dr2)
            {
                DataRow[] findrow = this.dt_detail.AsEnumerable().Where(row => row.RowState != DataRowState.Deleted && row["fromftyinventoryukey"].EqualString(tmp["fromftyinventoryukey"])
                                       && row["topoid"].EqualString(tmp["topoid"].ToString()) && row["toseq1"].EqualString(tmp["toseq1"])
                                       && row["toseq2"].EqualString(tmp["toseq2"].ToString()) && row["toroll"].EqualString(tmp["toroll"])
                                       && row["todyelot"].EqualString(tmp["todyelot"]) && row["tostocktype"].EqualString(tmp["tostocktype"])).ToArray();

                if (findrow.Length > 0)
                {
                    findrow[0]["qty"] = tmp["qty"];
                    findrow[0]["ToLocation"] = tmp["ToLocation"];
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

        private void ChkNoLock_CheckedChanged(object sender, EventArgs e)
        {
            this.Grid_Filter();
            this.ChangeColor();
        }

        private void Grid_Filter()
        {
            string filter = string.Empty;
            DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    switch (this.chkNoLock.Checked)
                    {
                        case true:
                            filter = @"lock = 'False'";
                            break;
                        case false:
                            filter = string.Empty;
                            break;
                    }

                    ((DataTable)this.listControlBindingSource1.DataSource).DefaultView.RowFilter = filter;
                }
            }
        }

        private void ChangeColor()
        {
            DataTable tmp_dt = (DataTable)this.listControlBindingSource1.DataSource;
            if (tmp_dt == null || this.gridImport.Rows.Count < 1)
            {
                return;
            }

            for (int i = 0; i < this.gridImport.Rows.Count; i++)
            {
                DataRow dr = this.gridImport.GetDataRow(i);
                if (this.gridImport.Rows.Count <= i || i < 0)
                {
                    return;
                }

                if (dr["Lock"].ToString() == "True")
                {
                    this.gridImport.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(190, 190, 190);
                }
                else
                {
                    this.gridImport.Columns["toroll"].DefaultCellStyle.BackColor = Color.Pink;
                    this.gridImport.Columns["qty"].DefaultCellStyle.BackColor = Color.Pink;
                }

                // grid 欄位可否編輯
                this.gridImport.RowEnter += this.Grid_RowEnter;
            }
        }

        private void Grid_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            var data = ((DataRowView)this.gridImport.Rows[e.RowIndex].DataBoundItem).Row;
            if (data == null)
            {
                return;
            }

            if (MyUtility.Check.Empty(data["Lock"]))
            {
                this.col_chk.IsEditable = true;
                this.col_roll.IsEditingReadOnly = false;
                this.col_qty.IsEditingReadOnly = false;
            }
            else
            {
                this.col_chk.IsEditable = false;
                this.col_roll.IsEditingReadOnly = true;
                this.col_qty.IsEditingReadOnly = true;
            }
        }

        private void GridImport_Validated(object sender, EventArgs e)
        {
            this.ChangeColor();
        }

        private void GridImport_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            this.ChangeColor();
        }
    }
}
