using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci;
using Sci.Data;

namespace Sci.Production.Warehouse
{
    public partial class P33_Detail_Detail : Sci.Win.Subs.Base
    {
        public Sci.Win.Subs.Base P33_Detail;
        DataRow dr_master;
        DataTable dt_detail;
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        public decimal _AccuIssued = 0;
        public decimal _RequestQty = 0;
        protected DataTable dtFtyinventory;

        public P33_Detail_Detail(DataRow master, DataTable detail ,string AccuIssued ,string RequestQty)
        {
            InitializeComponent();
            dr_master = master;
            dt_detail = detail;
            _AccuIssued = MyUtility.Check.Empty(AccuIssued) ? 0 : Convert.ToDecimal(AccuIssued);
            _RequestQty = MyUtility.Check.Empty(RequestQty) ? 0 : Convert.ToDecimal(RequestQty);
        }

        private void sum_checkedqty()
        {
            listControlBindingSource1.EndEdit();
            DataTable dt = (DataTable)listControlBindingSource1.DataSource;

            this.numIssueQty.Value = MyUtility.Check.Empty(dt.Compute("Sum(Qty)", "selected = 1")) ? 0 : Convert.ToDecimal(dt.Compute("Sum(Qty)", "selected = 1"));        
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            Ict.DualResult result;

            this.displayRefno.Text = dr_master["refno"].ToString();
            this.displaySPNo.Text = dr_master["poid"].ToString();
            this.displayColorID.Text = dr_master["SuppColor"].ToString();
            this.editDesc.Text = dr_master["DescDetail"].ToString();
            this.numAccuIssue.Value = _AccuIssued;
            this.numRequestQty.Value = _RequestQty;

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
       , [POID]='{ dr_master["poid"].ToString()}'
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
WHERE psd.id = '{dr_master["poid"]}' 
AND psd.SCIRefno='{dr_master["SCIRefno"]}' 
AND psd.SuppColor='{dr_master["SuppColor"]}'
AND (a.stocktype = 'B' OR a.stocktype IS NULL)

");
            #endregion

            P33_Detail.ShowWaitMessage("Data Loading....");

            if (result = DBProxy.Current.Select(null, strSQLCmd.ToString(), out dtFtyinventory))
            {
                if (dtFtyinventory.Rows.Count == 0)
                { MyUtility.Msg.WarningBox("Data not found!!"); }
                listControlBindingSource1.DataSource = dtFtyinventory;
            }
            else
            {
                ShowErr(strSQLCmd.ToString(), result);
            }

            P33_Detail.HideWaitMessage();

            Ict.Win.DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();
            ns.CellValidating += (s, e) =>
            {
                if (this.EditMode && !MyUtility.Check.Empty(e.FormattedValue))
                {
                    decimal oldValue = Convert.ToDecimal(gridSeq.GetDataRow(gridSeq.GetSelectedRowIndex())["Qty"]);
                    if (Convert.ToDecimal(gridSeq.GetDataRow(gridSeq.GetSelectedRowIndex())["BulkQty"]) < Convert.ToDecimal(e.FormattedValue))
                    {
                        MyUtility.Msg.InfoBox("Can't over [Bulk Qty]!!");
                        gridSeq.GetDataRow(gridSeq.GetSelectedRowIndex())["Qty"] = oldValue;
                        gridSeq.GetDataRow(gridSeq.GetSelectedRowIndex())["selected"] = false;

                        return;
                    }

                    gridSeq.GetDataRow(gridSeq.GetSelectedRowIndex())["Qty"] = e.FormattedValue;
                    gridSeq.GetDataRow(gridSeq.GetSelectedRowIndex())["selected"] = true;
                    this.sum_checkedqty();
                }
            };
            
            this.gridSeq.CellValueChanged += (s, e) =>
            {
                if (gridSeq.Columns[e.ColumnIndex].Name == col_chk.Name)
                {
                    DataRow dr = gridSeq.GetDataRow(e.RowIndex);
                    if (Convert.ToBoolean(dr["selected"]) == true && Convert.ToDecimal(dr["Qty"].ToString()) == 0)
                    {
                        dr["Qty"] = dr["BulkQty"];
                    }
                    else if (Convert.ToBoolean(dr["selected"]) == false)
                    {
                        dr["Qty"] = 0;
                    }
                    dr.EndEdit();
                    this.sum_checkedqty();
                }
            };
            
            this.gridSeq.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            this.gridSeq.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.gridSeq)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)
                .Text("seq1", header: "Seq1", iseditingreadonly: true, width: Widths.AnsiChars(6))
                .Text("seq2", header: "Seq2", iseditingreadonly: true, width: Widths.AnsiChars(6))
                .Numeric("BulkQty", header: "Bulk Qty", iseditingreadonly: true, decimal_places: 2, integer_places: 10) 
                .Numeric("Qty", header: "Issue Qty", iseditable: true, decimal_places: 2, integer_places: 10, settings: ns)  
                .Text("BulkLocation", header: "Bulk Location", iseditingreadonly: true) 
                ;

            this.gridSeq.Columns["Qty"].DefaultCellStyle.BackColor = Color.Pink;

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnImport_Click(object sender, EventArgs e)
        {

            gridSeq.ValidateControl();

            DataTable dt = (DataTable)listControlBindingSource1.DataSource;

            if (MyUtility.Check.Empty(dt) || dt.Rows.Count == 0) return;

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
                DataRow[] findrow = dt_detail.Select($" seq1 = '{tmp["seq1"]}' and seq2 = '{tmp["seq2"]}'");
                if (findrow.Length > 0)
                {
                    findrow[0]["Qty"] = tmp["Qty"];
                }
                else
                {
                    tmp["ID"] = dr_master["ID"];  // Issue.ID
                    tmp.AcceptChanges();
                    tmp.SetAdded();
                    dt_detail.ImportRow(tmp);
                }
            }

            this.Close();
        }
    }
}
