using System;
using System.Data;
using System.Drawing;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P12_Import : Win.Subs.Base
    {
        private DataRow dr_master;
        private DataTable dt_detail;
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;

        // bool flag;
        // string poType;
        private DataTable dtArtwork;

        /// <inheritdoc/>
        public P12_Import(DataRow master, DataTable detail)
        {
            this.InitializeComponent();
            this.dr_master = master;
            this.dt_detail = detail;
        }

        // Find Now Button
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        private void BtnFindNow_Click(object sender, EventArgs e)
        {
            string sp_b = this.txtSPNo.Text;

            if (string.IsNullOrWhiteSpace(sp_b))
            {
                MyUtility.Msg.WarningBox("< SP# > can't be empty!!");
                this.txtSPNo.Focus();
                return;
            }
            else
            {
                // 建立可以符合回傳的Cursor
                string strSQLCmd = string.Format(
                    @"
select  selected = 0
        , id = '' 
        , PoId = a.id 
        , a.Seq1
        , a.Seq2
        , seq = concat(a.seq1, ' ', a.Seq2)
        , a.FabricType
        , a.stockunit
        , [Description] = dbo.getmtldesc(a.id,a.seq1,a.seq2,2,0) 
        , Roll = '' 
        , Dyelot = '' 
        , Qty = 0.00 
        , StockType = 'B' 
        , ftyinventoryukey = c.ukey 
        , location = dbo.Getlocation(c.ukey) 
        , balance = c.inqty-c.outqty + c.adjustqty 
from dbo.PO_Supp_Detail a WITH (NOLOCK) 
inner join dbo.ftyinventory c WITH (NOLOCK) on c.poid = a.id and c.seq1 = a.seq1 and c.seq2  = a.seq2 and c.stocktype = 'B'
inner join Orders on c.poid = orders.id
inner join factory on orders.factoryID = factory.id
inner join fabric WITH (NOLOCK) on fabric.scirefno = a.scirefno
inner join mtltype WITH (NOLOCK) on mtltype.id = fabric.mtltypeid
Where a.id = '{0}' 
      and c.lock = 0 
      and upper(dbo.mtltype.Issuetype) = 'PACKING' 
      and factory.MDivisionID = '{1}'
      and Orders.category != 'A'
", sp_b, Env.User.Keyword);

                DualResult result;
                if (result = DBProxy.Current.Select(null, strSQLCmd, out this.dtArtwork))
                {
                    if (this.dtArtwork.Rows.Count == 0)
                    {
                        MyUtility.Msg.WarningBox("Data not found!!");
                    }

                    this.txtSPNo.Text = string.Empty;
                    this.listControlBindingSource1.DataSource = this.dtArtwork;
                }
                else
                {
                    this.ShowErr(strSQLCmd, result);
                }
            }
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();
            ns.CellValidating = (s, e) =>
            {
                if (this.EditMode && !MyUtility.Check.Empty(e.FormattedValue))
                {
                    DataRow temp = this.dtArtwork.Rows[e.RowIndex];
                    temp["qty"] = e.FormattedValue;
                    if (Convert.ToDecimal(e.FormattedValue) > 0)
                    {
                        temp["selected"] = true;
                    }
                }
            };

            this.gridImport.CellValueChanged += (s, e) =>
            {
                if (this.gridImport.Columns[e.ColumnIndex].Name == this.col_chk.Name)
                {
                    DataRow dr = this.gridImport.GetDataRow(e.RowIndex);
                    if (Convert.ToBoolean(dr["Selected"]) == true && Convert.ToDecimal(dr["qty"].ToString()) == 0)
                    {
                        dr["qty"] = dr["balance"];
                    }
                    else if (Convert.ToBoolean(dr["selected"]) == false)
                    {
                        dr["qty"] = 0;
                    }

                    dr.EndEdit();
                }
            };

            this.gridImport.IsEditingReadOnly = false; // 必設定, 否則CheckBox會顯示圖示
            this.gridImport.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridImport)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk) // 0
                .Text("seq", header: "Seq#", iseditingreadonly: true, width: Widths.AnsiChars(6)) // 1
                .Text("location", header: "Bulk Location", iseditingreadonly: true) // 2
                .Text("StockUnit", header: "Unit", iseditingreadonly: true) // 3
                .Numeric("balance", header: "Stock Qty", iseditable: true, decimal_places: 2, integer_places: 10) // 4
                .Numeric("qty", header: "Issue Qty", decimal_places: 2, integer_places: 10, settings: ns) // 5
               .EditText("Description", header: "Description", iseditingreadonly: true, width: Widths.AnsiChars(40)); // 6

            this.gridImport.Columns["qty"].DefaultCellStyle.BackColor = Color.Pink;  // PCS/Stitch
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        private void BtnImport_Click(object sender, EventArgs e)
        {
            // listControlBindingSource1.EndEdit();
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

            dr2 = dtGridBS1.Select("qty = 0 and Selected = 1");
            if (dr2.Length > 0)
            {
                MyUtility.Msg.WarningBox("Qty of selected row can't be zero!", "Warning");
                return;
            }

            dr2 = dtGridBS1.Select("qty > balance");
            if (dr2.Length > 0)
            {
                MyUtility.Msg.WarningBox("Qty of selected row can't more than Stock Qty!", "Warning");
                return;
            }

            dr2 = dtGridBS1.Select("qty <> 0 and Selected = 1");
            foreach (DataRow tmp in dr2)
            {
                DataRow[] findrow = this.dt_detail.Select(string.Format(
                    "poid = '{0}' and seq1 = '{1}' and seq2 = '{2}'",
                    tmp["poid"].ToString(), tmp["seq1"].ToString(), tmp["seq2"].ToString()));

                if (findrow.Length > 0)
                {
                    // findrow[0]["unitprice"] = tmp["unitprice"];
                    // findrow[0]["Price"] = tmp["Price"];
                    // findrow[0]["amount"] = tmp["amount"];
                    // findrow[0]["poqty"] = tmp["poqty"];
                    // findrow[0]["qtygarment"] = 1;
                    findrow[0]["Qty"] = tmp["Qty"];
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
