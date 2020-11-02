using System;
using System.Data;
using System.Drawing;
using System.Text;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P33_Detail_Detail : Win.Subs.Base
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public Win.Subs.Base P33_Detail;
        private DataRow dr_master;
        private DataTable dt_detail;
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        private decimal _AccuIssued = 0;
        private decimal _RequestQty = 0;
        private DataTable dtFtyinventory;

        /// <inheritdoc/>
        public P33_Detail_Detail(DataRow master, DataTable detail, string accuIssued, string requestQty)
        {
            this.InitializeComponent();
            this.dr_master = master;
            this.dt_detail = detail;
            this._AccuIssued = MyUtility.Check.Empty(accuIssued) ? 0 : Convert.ToDecimal(accuIssued);
            this._RequestQty = MyUtility.Check.Empty(requestQty) ? 0 : Convert.ToDecimal(requestQty);
        }

        private void Sum_checkedqty()
        {
            this.listControlBindingSource1.EndEdit();
            DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;

            this.numIssueQty.Value = MyUtility.Check.Empty(dt.Compute("Sum(Qty)", "selected = 1")) ? 0 : Convert.ToDecimal(dt.Compute("Sum(Qty)", "selected = 1"));
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            DualResult result;

            this.displaySCIRefno.Text = this.dr_master["SCIRefno"].ToString();
            this.displayRefno.Text = this.dr_master["refno"].ToString();
            this.displaySPNo.Text = this.dr_master["poid"].ToString();
            this.displayColorID.Text = this.dr_master["ColorID"].ToString();
            this.displaySuppColor.Text = this.dr_master["SuppColor"].ToString();
            this.editDesc.Text = this.dr_master["DescDetail"].ToString();
            this.numAccuIssue.Value = this._AccuIssued;
            this.numRequestQty.Value = this._RequestQty;

            StringBuilder strSQLCmd = new StringBuilder();

            #region -- sqlcmd query --

            strSQLCmd.Append($@"
SELECT 0 as selected 
       , psd.ID
       , psd.Seq1
       , psd.Seq2
       , [BulkQty] =ISNULL( a.inqty - a.outqty + a.adjustqty,0.00)
	   , [Qty]=0.00
	   , [BulkLocation]= ISNULL( Location.MtlLocationID ,'')
       , a.stocktype
       , [FtyInventoryUkey]=a.Ukey
       , [POID]='{this.dr_master["poid"].ToString()}'
FROM dbo.PO_Supp_Detail psd WITH (NOLOCK) 
LEFT JOIN FtyInventory a on a.POID = psd.id AND a.seq1=psd.seq1 AND a.seq2=psd.seq2
OUTER APPLY(
	SELECT   [MtlLocationID] = STUFF(
	(
		SELECT DISTINCT ',' +fid.MtlLocationID 
		FROM FtyInventory_Detail FID 
		WHERE FID.Ukey= a.Ukey AND  fid.MtlLocationID  <> ''
		FOR XML PATH('')
	), 1, 1, '') 
)Location
WHERE psd.id = '{this.dr_master["poid"]}' 
AND psd.SCIRefno='{this.dr_master["SCIRefno"]}' 
AND psd.ColorID='{this.dr_master["ColorID"]}'
AND (a.stocktype = 'B' OR a.stocktype IS NULL)

");
            #endregion

            this.P33_Detail.ShowWaitMessage("Data Loading....");

            if (result = DBProxy.Current.Select(null, strSQLCmd.ToString(), out this.dtFtyinventory))
            {
                if (this.dtFtyinventory.Rows.Count == 0)
                {
                    MyUtility.Msg.WarningBox("Data not found!!");
                }

                this.listControlBindingSource1.DataSource = this.dtFtyinventory;
            }
            else
            {
                this.ShowErr(strSQLCmd.ToString(), result);
            }

            this.P33_Detail.HideWaitMessage();

            DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();
            ns.CellValidating += (s, e) =>
            {
                if (this.EditMode && !MyUtility.Check.Empty(e.FormattedValue))
                {
                    decimal oldValue = Convert.ToDecimal(this.gridSeq.GetDataRow(this.gridSeq.GetSelectedRowIndex())["Qty"]);
                    if (Convert.ToDecimal(this.gridSeq.GetDataRow(this.gridSeq.GetSelectedRowIndex())["BulkQty"]) < Convert.ToDecimal(e.FormattedValue))
                    {
                        MyUtility.Msg.InfoBox("Can't over [Bulk Qty]!!");
                        this.gridSeq.GetDataRow(this.gridSeq.GetSelectedRowIndex())["Qty"] = oldValue;
                        this.gridSeq.GetDataRow(this.gridSeq.GetSelectedRowIndex())["selected"] = false;

                        return;
                    }

                    this.gridSeq.GetDataRow(this.gridSeq.GetSelectedRowIndex())["Qty"] = e.FormattedValue;
                    this.gridSeq.GetDataRow(this.gridSeq.GetSelectedRowIndex())["selected"] = true;
                    this.Sum_checkedqty();
                }
            };

            this.gridSeq.CellValueChanged += (s, e) =>
            {
                if (this.gridSeq.Columns[e.ColumnIndex].Name == this.col_chk.Name)
                {
                    DataRow dr = this.gridSeq.GetDataRow(e.RowIndex);
                    if (Convert.ToBoolean(dr["selected"]) == true && Convert.ToDecimal(dr["Qty"].ToString()) == 0)
                    {
                        dr["Qty"] = dr["BulkQty"];
                    }
                    else if (Convert.ToBoolean(dr["selected"]) == false)
                    {
                        dr["Qty"] = 0;
                    }

                    dr.EndEdit();
                    this.Sum_checkedqty();
                }
            };

            this.gridSeq.IsEditingReadOnly = false; // 必設定, 否則CheckBox會顯示圖示
            this.gridSeq.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridSeq)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
                .Text("seq1", header: "Seq1", iseditingreadonly: true, width: Widths.AnsiChars(6))
                .Text("seq2", header: "Seq2", iseditingreadonly: true, width: Widths.AnsiChars(6))
                .Numeric("BulkQty", header: "Bulk Qty", iseditingreadonly: true, decimal_places: 2, integer_places: 10)
                .Numeric("Qty", header: "Issue Qty", iseditable: true, decimal_places: 2, integer_places: 10, settings: ns)
                .Text("BulkLocation", header: "Bulk Location", iseditingreadonly: true);

            this.gridSeq.Columns["Qty"].DefaultCellStyle.BackColor = Color.Pink;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            this.gridSeq.ValidateControl();

            DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;

            if (MyUtility.Check.Empty(dt) || dt.Rows.Count == 0)
            {
                return;
            }

            DataRow[] dr2 = dt.Select("Selected = 1");
            if (dr2.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select rows first!", "Warnning");
                return;
            }

            dr2 = dt.Select("Qty = 0 and Selected = 1");
            if (dr2.Length > 0)
            {
                MyUtility.Msg.WarningBox("Issue Qty of selected row can't be zero!", "Warning");
                return;
            }

            dr2 = dt.Select("Qty > BulkQty and Selected = 1");
            if (dr2.Length > 0)
            {
                MyUtility.Msg.WarningBox("Issue Qty of selected row can't be more then Bulk Qty !", "Warning");
                return;
            }

            dr2 = dt.Select("Qty <> 0 and Selected = 1");
            foreach (DataRow tmp in dr2)
            {
                DataRow[] findrow = this.dt_detail.Select($" seq1 = '{tmp["seq1"]}' and seq2 = '{tmp["seq2"]}'");
                if (findrow.Length > 0)
                {
                    findrow[0]["Qty"] = tmp["Qty"];
                }
                else
                {
                    tmp["ID"] = this.dr_master["ID"];  // Issue.ID
                    tmp.AcceptChanges();
                    tmp.SetAdded();
                    this.dt_detail.ImportRow(tmp);
                }
            }

            this.Close();
        }
    }
}
