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
using System.Linq;

namespace Sci.Production.Warehouse
{
    public partial class P31_Import : Sci.Win.Subs.Base
    {
        DataRow dr_master;
        DataTable dt_detail;
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        Ict.Win.UI.DataGridViewNumericBoxColumn col_qty;
        Ict.Win.UI.DataGridViewTextBoxColumn col_roll;

        protected DataTable dtBorrow;
        private Dictionary<string, string> di_stocktype = new Dictionary<string, string>();

        public P31_Import(DataRow master, DataTable detail)
        {
            this.InitializeComponent();
            this.dr_master = master;
            this.dt_detail = detail;
            this.di_stocktype.Add("B", "Bulk");
            this.di_stocktype.Add("I", "Inventory");
        }

        // Find Now Button
        private void btnFindNow_Click(object sender, EventArgs e)
        {
            StringBuilder strSQLCmd = new StringBuilder();
            string sp = this.txtToSP.Text.TrimEnd();
            string fromSP = this.txtBorrowFromSP.Text.TrimEnd();
            int intNoLock = this.chkNoLock.Checked ? 1 : 0;

            if (string.IsNullOrWhiteSpace(sp) || this.txtSeq.checkSeq1Empty()
                || this.txtSeq.checkSeq2Empty() || string.IsNullOrWhiteSpace(fromSP))
            {
                MyUtility.Msg.WarningBox("< To SP# Seq> <From SP#> can't be empty!!");
                this.txtToSP.Focus();
                return;
            }
            else
            {
                #region Get SizeSpec, Refno, Color, Desc
                DataTable dt;
                string strSQL = string.Format(
                    @"
select  po.SizeSpec
        , po.Refno
        , po.ColorID
        , f.Description
from PO_Supp_Detail po
left join Fabric f on po.SCIRefno = f.SCIRefno
LEFT JOIN Orders o ON o.ID=po.ID
where   po.id = '{0}' 
        and po.seq1 = '{1}' 
        and po.seq2='{2}'
		AND o.Category<>'A'
", sp, this.txtSeq.seq1, this.txtSeq.seq2);

                DBProxy.Current.Select(null, strSQL, out dt);
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
        ,FromPoId = a.id 
        ,FromSeq1 = a.Seq1 
        ,FromSeq2 = a.Seq2 
        ,FromFactoryID = orders.FactoryID
        ,fromseq = concat(Ltrim(Rtrim(a.seq1)), ' ', a.Seq2) 
        ,[Description] = dbo.getmtldesc(a.id,a.seq1,a.seq2,2,0) 
        ,a.stockunit
        ,fromstocktype = c.StockType 
        ,balance = c.InQty - c.OutQty + c.AdjustQty 
        ,location = dbo.Getlocation(c.ukey)
        ,toseq = concat(Ltrim(Rtrim(b.seq1)), ' ', b.Seq2) 
        ,toroll = iif(toSP.Roll is not null, toSP.Roll, c.Roll)
        ,todyelot = iif(toSP.Roll is not null, toSP.Dyelot, c.Dyelot)
        ,Qty = 0.00 
        ,ToStocktype = 'B' 
        ,topoid = b.id 
        ,toseq1 = b.seq1 
        ,toseq2 = b.seq2 
        ,toFactoryID = (select FactoryID from Orders where b.id = Orders.id)
        ,a.fabrictype
        ,c.Lock
from dbo.PO_Supp_Detail a WITH (NOLOCK) 
inner join dbo.ftyinventory c WITH (NOLOCK) on c.poid = a.id and c.seq1 = a.seq1 and c.seq2  = a.seq2 
inner join Orders on c.poid = Orders.id
inner join Factory on Orders.FactoryID = Factory.ID
left join dbo.po_supp_detail b WITH (NOLOCK) on b.Refno = a.Refno and b.SizeSpec = a.SizeSpec and b.ColorID = a.ColorID and b.BrandId = a.BrandId
outer apply(
    select	Top 1 Roll
		    , Dyelot
    from FtyInventory
    where POID = b.id
	    and Seq1 = b.seq1
	    and Seq2 = b.seq2
        and Roll = c.Roll
) as toSP
Where a.id = '{fromSP}' and b.id = '{sp}' and b.seq1 = '{this.txtSeq.seq1}' 
and b.seq2='{this.txtSeq.seq2}' and Factory.MDivisionID = '{Sci.Env.User.Keyword}'
and c.inqty-c.outqty + c.adjustqty > 0 and  c.StockType='B'
AND Orders.Category <> 'A' ");

                this.ShowWaitMessage("Data Loading....");
                Ict.DualResult result;
                if (result = DBProxy.Current.Select(null, strSQLCmd.ToString(), out this.dtBorrow))
                {
                    if (this.dtBorrow.Rows.Count == 0)
                    {
                        MyUtility.Msg.WarningBox("Data not found!!");
                        this.txtToSP.Text = string.Empty;
                        this.txtSeq.seq1 = string.Empty;
                        this.txtSeq.seq2 = string.Empty;
                        this.txtBorrowFromSP.Text = string.Empty;
                    }

                    this.listControlBindingSource1.DataSource = this.dtBorrow;
                    this.grid_Filter();
                    if (this.gridImport.Rows.Count == 0)
                    {
                        this.chkNoLock.Checked = false;
                        this.grid_Filter();
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

        private void sum_checkedqty()
        {
            this.listControlBindingSource1.EndEdit();
            DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
            object localPrice = dt.Compute("Sum(qty)", "selected = 1");
            this.displayTotalQty.Value = localPrice.ToString();
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            Ict.Win.DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();
            ns.CellValidating += (s, e) =>
                {
                    if (this.EditMode && !MyUtility.Check.Empty(e.FormattedValue))
                    {
                        this.gridImport.GetDataRow(this.gridImport.GetSelectedRowIndex())["qty"] = e.FormattedValue;
                        this.gridImport.GetDataRow(this.gridImport.GetSelectedRowIndex())["selected"] = true;
                        this.sum_checkedqty();
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
                    this.sum_checkedqty();
                }
            };

            Ict.Win.UI.DataGridViewComboBoxColumn cbb_stocktype;

            this.gridImport.IsEditingReadOnly = false; // 必設定, 否則CheckBox會顯示圖示
            this.gridImport.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridImport)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk) // 0
                .Text("fromroll", header: "Roll", iseditingreadonly: true, width: Widths.AnsiChars(6)) // 1
                .Text("fromdyelot", header: "Dyelot", iseditingreadonly: true, width: Widths.AnsiChars(8)) // 2
                .Text("fromseq", header: "From" + Environment.NewLine + "Seq#", iseditingreadonly: true, width: Widths.AnsiChars(6)) // 3
                .EditText("Description", header: "Description", iseditingreadonly: true, width: Widths.AnsiChars(25)) // 4
                .Text("StockUnit", header: "Unit", iseditingreadonly: true) // 5
                .ComboBox("fromstocktype", header: "From" + Environment.NewLine + "Stock" + Environment.NewLine + "Type", iseditable: false).Get(out cbb_stocktype) // 6
                .Numeric("balance", header: "Stock Qty", iseditingreadonly: true, decimal_places: 2, integer_places: 10) // 7
                .Text("location", header: "From Location", iseditingreadonly: true) // 8
                .Text("toseq", header: "To" + Environment.NewLine + "Seq#", iseditingreadonly: true, width: Widths.AnsiChars(6)) // 9
                .Text("toroll", header: "To" + Environment.NewLine + "Roll", width: Widths.AnsiChars(6)).Get(out this.col_roll) // 10
                .Numeric("qty", header: "Issue" + Environment.NewLine + "Qty", decimal_places: 2, integer_places: 10, settings: ns).Get(out this.col_qty) // 11
               ;

            cbb_stocktype.DataSource = new BindingSource(this.di_stocktype, null);
            cbb_stocktype.ValueMember = "Key";
            cbb_stocktype.DisplayMember = "Value";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Import
        private void btnImport_Click(object sender, EventArgs e)
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
                    warningmsg.Append(string.Format(
                        @"To SP#: {0} To Seq#: {1}-{2} To Roll#:{3} To Dyelot:{4} Roll and Dyelot can't be empty",
                        row["topoid"], row["toseq1"], row["toseq2"], row["toroll"], row["todyelot"]) + Environment.NewLine);
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

        // To SP# Valid
        private void txtToSP_Validating(object sender, CancelEventArgs e)
        {
// string sp = textBox1.Text.TrimEnd();

// DataRow tmp;

// if (MyUtility.Check.Empty(sp)) return;

// if (txtSeq1.checkEmpty(showErrMsg: false))
//            {
//                if (!MyUtility.Check.Seek(string.Format("select 1 where exists(select * from po_supp_detail WITH (NOLOCK) where id ='{0}')"
//                    , sp), null))
//                {
//                    MyUtility.Msg.WarningBox("SP# is not found!!");
//                    e.Cancel = true;
//                    return;
//                }
//            }
//            else
//            {
//                if (!MyUtility.Check.Seek(string.Format(@"select sizespec,refno,colorid,dbo.getmtldesc(id,seq1,seq2,2,0) as [description]
//                        from po_supp_detail WITH (NOLOCK) where id ='{0}' and seq1 = '{1}' and seq2 = '{2}'", sp, txtSeq1.seq1, txtSeq1.seq2), out tmp, null))
//                {
//                    MyUtility.Msg.WarningBox("SP#-Seq is not found!!");
//                    e.Cancel = true;
//                    return;
//                }
//                else
//                {
//                    this.displayBox2.Value = tmp["sizespec"];
//                    this.displayBox3.Value = tmp["refno"];
//                    this.displayBox4.Value = tmp["colorid"];
//                    this.editBox1.Text = tmp["description"].ToString();
//                }
//            }
        }

        private void chkNoLock_CheckedChanged(object sender, EventArgs e)
        {
            this.grid_Filter();
            this.ChangeColor();
        }

        private void grid_Filter()
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

        private void gridImport_Validated(object sender, EventArgs e)
        {
            this.ChangeColor();
        }

        private void gridImport_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            this.ChangeColor();
        }
    }
}
