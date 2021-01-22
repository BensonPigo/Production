using System;
using System.Data;
using System.Drawing;
using System.Text;
using Ict;
using Ict.Win;
using Sci.Data;
using System.Linq;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P13_Import : Win.Subs.Base
    {
        private DataRow dr_master;
        private DataTable dt_detail;
        private bool IsReason06 = false;
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;

        // bool flag;
        // string poType;
        private DataTable dtArtwork;

        /// <inheritdoc/>
        public P13_Import(DataRow master, DataTable detail)
        {
            this.InitializeComponent();
            this.dr_master = master;
            this.dt_detail = detail;

            if (this.dr_master["WhseReasonID"].ToString() == "00006")
            {
                this.IsReason06 = true;
            }
        }

        // Find Now Button
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        private void BtnFindNow_Click(object sender, EventArgs e)
        {
            StringBuilder strSQLCmd = new StringBuilder();
            string sp = this.txtSPNo.Text.TrimEnd();

            if (string.IsNullOrWhiteSpace(sp))
            {
                MyUtility.Msg.WarningBox("< SP# > can't be empty!!");
                this.txtSPNo.Focus();
                return;
            }
            else
            {
                // 建立可以符合回傳的Cursor
                strSQLCmd.Append(string.Format(
                    @"
select  selected = 0  
        , Orders.FtyGroup
        , id = '' 
        , PoId = a.id 
        , a.Seq1
        , a.Seq2
        , seq = concat(Ltrim(Rtrim(a.seq1)), ' ', a.Seq2) 
        , a.FabricType
        , a.stockunit
        , [Description] = dbo.getmtldesc(a.id,a.seq1,a.seq2,2,0) 
        , Roll = Rtrim(Ltrim(c.Roll))
        , Dyelot = Rtrim(Ltrim(c.Dyelot))
        , Qty = 0.00 
        , StockType = 'B' 
        , ftyinventoryukey = c.ukey  
        , location = dbo.Getlocation(c.ukey)
        , balance = c.inqty - c.outqty + c.adjustqty - c.ReturnQty
		,a.NetQty
		,a.LossQty
        , [FabricTypeName] = (select name from DropDownList where Type='FabricType_Condition' and a.FabricType = id)
        , [Article] = case  when a.Seq1 like 'T%' then Stuff((Select distinct concat( ',',tcd.Article) 
			                                                         From dbo.Orders as o 
			                                                         Inner Join dbo.Style as s On s.Ukey = o.StyleUkey
			                                                         Inner Join dbo.Style_ThreadColorCombo as tc On tc.StyleUkey = s.Ukey
			                                                         Inner Join dbo.Style_ThreadColorCombo_Detail as tcd On tcd.Style_ThreadColorComboUkey = tc.Ukey 
			                                                         where	o.POID = a.ID and
			                                                         		tcd.SuppId = p.SuppId and
			                                                         		tcd.SCIRefNo   = a.SCIRefNo	and
			                                                         		tcd.ColorID	   = a.ColorID
			                                                         FOR XML PATH('')),1,1,'') 
                            else '' end
from dbo.PO_Supp_Detail a WITH (NOLOCK) 
inner join dbo.PO_Supp p with (nolock) on a.ID = p.ID and a.Seq1 = p.Seq1
inner join dbo.ftyinventory c WITH (NOLOCK) on c.poid = a.id and c.seq1 = a.seq1 and c.seq2  = a.seq2 and c.stocktype = 'B'
inner join dbo.Orders on c.poid = orders.id
inner join dbo.Factory on orders.FactoryID = factory.ID
INNER JOIN Fabric f on a.SCIRefNo=f.SCIRefNo
Where a.id = '{0}' and c.lock = 0 and c.inqty - c.outqty + c.adjustqty - c.ReturnQty > 0 AND Orders.category!='A'
    and factory.MDivisionID = '{1}'
", sp, Env.User.Keyword));
                if (!this.txtSeq1.CheckSeq1Empty() && this.txtSeq1.CheckSeq2Empty())
                {
                    strSQLCmd.Append(string.Format(
                        @" 
    and a.seq1 = '{0}' ", this.txtSeq1.Seq1));
                }
                else if (!this.txtSeq1.CheckEmpty(showErrMsg: false))
                {
                    strSQLCmd.Append(string.Format(
                        @" 
    and a.seq1 = '{0}' and a.seq2='{1}'", this.txtSeq1.Seq1, this.txtSeq1.Seq2));
                }

                if (this.IsReason06)
                {
                    strSQLCmd.Append(" and f.MtlTypeID in ('EMB THREAD','SP THREAD','THREAD')  " + Environment.NewLine);
                }
                else
                {
                    strSQLCmd.Append(" and f.MtlTypeID not in ('EMB THREAD','SP THREAD','THREAD')  " + Environment.NewLine);
                }

                if (string.Compare(this.comboFabricType.SelectedValue.ToString(), "ALL") != 0)
                {
                    strSQLCmd.Append($@" and a.FabricType='{this.comboFabricType.SelectedValue}'");
                }

                this.ShowWaitMessage("Data Loading....");
                DualResult result;
                if (result = DBProxy.Current.Select(null, strSQLCmd.ToString(), out this.dtArtwork))
                {
                    if (this.dtArtwork.Rows.Count == 0)
                    {
                        MyUtility.Msg.WarningBox("Data not found!!");
                    }

                    this.listControlBindingSource1.DataSource = this.dtArtwork;
                    this.dtArtwork.DefaultView.Sort = "seq1,seq2,location,dyelot,balance desc";
                }
                else
                {
                    this.ShowErr(strSQLCmd.ToString(), result);
                }

                this.HideWaitMessage();
            }
        }

        private void Sum_checkedqty()
        {
            this.listControlBindingSource1.EndEdit();
            DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
            object localPrice = dt.Compute("Sum(qty)", "selected = 1");
            this.displayTotal.Value = localPrice.ToString();
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
                        this.grid1.GetDataRow(this.grid1.GetSelectedRowIndex())["qty"] = e.FormattedValue;
                        this.grid1.GetDataRow(this.grid1.GetSelectedRowIndex())["selected"] = true;
                        this.Sum_checkedqty();
                    }
                };

            this.grid1.CellValueChanged += (s, e) =>
            {
                if (this.grid1.Columns[e.ColumnIndex].Name == this.col_chk.Name)
                {
                    DataRow dr = this.grid1.GetDataRow(e.RowIndex);

                    if (Convert.ToBoolean(dr["selected"]) == true && Convert.ToDecimal(dr["qty"].ToString()) == 0)
                    {
                        if (this.IsReason06)
                        {
                            // a.如Stock Qty >= PO_Supp_Detail.NetQty,則Issue Qty帶入PO_Supp_Detail.NetQty
                            // b.如Stock Qty >= PO_Supp_Detail.LossQty,則Issue Qty帶入PO_Supp_Detail.LossQty //PO_Supp_Detail.LossQty = 0時跳過此步驟
                            // c.Issue Qty帶入Stock Qty
                            if (Convert.ToInt32(dr["balance"]) >= Convert.ToInt32(dr["NetQty"]))
                            {
                                dr["qty"] = dr["NetQty"];
                            }
                            else if (Convert.ToInt32(dr["balance"]) >= Convert.ToInt32(dr["LossQty"]) && Convert.ToInt32(dr["LossQty"]) != 0)
                            {
                                dr["qty"] = dr["LossQty"];
                            }
                            else
                            {
                                dr["qty"] = dr["balance"];
                            }
                        }
                        else
                        {
                            dr["qty"] = dr["balance"];
                        }
                    }
                    else if (Convert.ToBoolean(dr["selected"]) == false)
                    {
                        dr["qty"] = 0;
                    }

                    dr.EndEdit();
                    this.Sum_checkedqty();
                }
            };

            this.grid1.IsEditingReadOnly = false; // 必設定, 否則CheckBox會顯示圖示
            this.grid1.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.grid1)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk) // 0
                .Text("seq", header: "Seq#", iseditingreadonly: true, width: Widths.AnsiChars(6)) // 1
                .Text("location", header: "Bulk Location", iseditingreadonly: true) // 2
                .Text("FabricTypeName", header: "Fabric Type", iseditingreadonly: true) // 3
                .Text("dyelot", header: "Dyelot", iseditingreadonly: true, width: Widths.AnsiChars(8)) // 3
                .Text("roll", header: "Roll", iseditingreadonly: true, width: Widths.AnsiChars(6)) // 4
                .Text("StockUnit", header: "Unit", iseditingreadonly: true) // 5
                .EditText("Article", header: "Article", iseditingreadonly: true, width: Widths.AnsiChars(15)) // 8
                .Numeric("NetQty", header: "Used Qty", iseditingreadonly: true, decimal_places: 2, integer_places: 10)
                .Numeric("LossQty", header: "Loss Qty", iseditingreadonly: true, decimal_places: 2, integer_places: 10)
                .Numeric("balance", header: "Stock Qty", iseditingreadonly: true, decimal_places: 2, integer_places: 10) // 6
                .Numeric("qty", header: "Issue Qty", decimal_places: 2, integer_places: 10, settings: ns) // 7
               .EditText("Description", header: "Description", iseditingreadonly: true, width: Widths.AnsiChars(25)); // 8

            this.grid1.Columns["qty"].DefaultCellStyle.BackColor = Color.Pink;

            if (this.dr_master["whseReasonID"].ToString() == "00006")
            {
                this.grid1.Columns["NetQty"].Visible = true;
                this.grid1.Columns["LossQty"].Visible = true;
                this.grid1.Columns["Article"].Visible = true;
            }
            else
            {
                this.grid1.Columns["NetQty"].Visible = false;
                this.grid1.Columns["LossQty"].Visible = false;
                this.grid1.Columns["Article"].Visible = false;
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            // listControlBindingSource1.EndEdit();
            this.grid1.ValidateControl();
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

            dr2 = dtGridBS1.Select("qty = 0 and Selected = 1");
            if (dr2.Length > 0)
            {
                MyUtility.Msg.WarningBox("Qty of selected row can't be zero!", "Warning");
                return;
            }

            dr2 = dtGridBS1.Select("qty <> 0 and Selected = 1");
            foreach (DataRow tmp in dr2)
            {
                DataRow[] findrow = this.dt_detail.AsEnumerable().Where(row => row.RowState != DataRowState.Deleted
                                                                          && row["poid"].EqualString(tmp["poid"].ToString()) && row["seq1"].EqualString(tmp["seq1"])
                                                                          && row["seq2"].EqualString(tmp["seq2"].ToString()) && row["roll"].EqualString(tmp["roll"])
                                                                          && row["dyelot"].EqualString(tmp["dyelot"])).ToArray();

                // .Select(string.Format("poid = '{0}' and seq1 = '{1}' and seq2 = '{2}' and roll ='{3}'and dyelot='{4}'"
                    // , tmp["poid"].ToString(), tmp["seq1"].ToString(), tmp["seq2"].ToString(), tmp["roll"].ToString(), tmp["dyelot"].ToString()));
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
    }
}
