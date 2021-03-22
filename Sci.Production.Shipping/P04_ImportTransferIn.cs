using System;
using System.Data;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// P04_ImportTransferIn
    /// </summary>
    public partial class P04_ImportTransferIn : Win.Subs.Base
    {
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        private DataTable detailData;

        /// <summary>
        /// P04_ImportTransferIn
        /// </summary>
        /// <param name="dt">dt</param>
        public P04_ImportTransferIn(DataTable dt)
        {
            this.InitializeComponent();
            this.detailData = dt;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.gridImport.IsEditingReadOnly = false;
            this.gridImport.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridImport)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
                .Text("TransactionID", header: "Transfer In No.", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("POID", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("Seq", header: "SEQ", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("Supp", header: "Supplier", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("RefNo", header: "Ref#", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("FabricType", header: "Type", width: Widths.AnsiChars(9), iseditingreadonly: true)
                .Text("MtlTypeID", header: "Material Type", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("UnitId", header: "Unit", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Numeric("Qty", header: "Q'ty", decimal_places: 2)
                .Numeric("NetKg", header: "N.W.(kg)", decimal_places: 2)
                .Numeric("WeightKg", header: "N.W.(kg)", decimal_places: 2);
        }

        // Qurey
        private void BtnQuery_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.txtTransferInNo.Text))
            {
                this.txtTransferInNo.Focus();
                MyUtility.Msg.WarningBox("< Transfer In No. > can't be empty!");
                return;
            }

            string sqlCmd = $@"
select Selected = cast(1 as bit)
    , [TransactionID] = td.ID
    , td.Poid
    , td.Seq1
    , td.Seq2
    , Seq = (left(td.Seq1 + ' ', 3) + '-' + td.Seq2)
    , SuppID = isnull(ps.SuppID, '')
    , Supp = (isnull(ps.SuppID, '') + '-' + isnull(s.AbbEN, ''))
    , RefNo = isnull(psd.Refno, '')
    , SCIRefNo = isnull(psd.SCIRefno, '')
    , Description = isnull(f.DescDetail, '') 
    , FabricType = isnull(psd.FabricType, '')
    , Type = case 
        when psd.FabricType = 'F' then 'Fabric' 
        when psd.FabricType = 'A' then 'Accessory' 
        else '' 
        end
    , MtlTypeID = isnull(f.MtlTypeID, '') 
    , UnitId = isnull(psd.StockUnit, '') 
    , td.Qty
    , NetKg = 0.0
    , WeightKg = 0.0
    , o.BuyerDelivery
    , BrandID = isnull(o.BrandID, '')
    , FactoryID = isnull(o.FactoryID, '')
    , o.SciDelivery
from TransferIn_Detail td WITH (NOLOCK) 
left join PO_Supp ps WITH (NOLOCK) on ps.ID = td.Poid and ps.SEQ1 = td.Seq1
left join PO_Supp_Detail psd WITH (NOLOCK) on psd.ID = td.Poid and psd.SEQ1= td.Seq1 and psd.SEQ2 = td.Seq2
left join Supp s WITH (NOLOCK) on s.ID = ps.SuppID
left join Fabric f WITH (NOLOCK) on f.SCIRefno = psd.SCIRefno
left join Orders o WITH (NOLOCK) on o.ID = td.Poid
where td.ID = '{this.txtTransferInNo.Text}'
";
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out DataTable selectData);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            if (selectData.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
            }

            this.listControlBindingSource1.DataSource = selectData;
        }

        // Import
        private void BtnImport_Click(object sender, EventArgs e)
        {
            this.gridImport.ValidateControl();
            this.listControlBindingSource1.EndEdit();
            DataTable gridData = (DataTable)this.listControlBindingSource1.DataSource;
            if (MyUtility.Check.Empty(gridData) || gridData.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("No data!");
                return;
            }

            DataRow[] dr = gridData.Select("Selected = 1");
            if (dr.Length > 0)
            {
                foreach (DataRow currentRow in dr)
                {
                    DataRow[] findrow = this.detailData.Select($@"
TransactionID = '{MyUtility.Convert.GetString(currentRow["TransactionID"])}' 
AND POID = '{MyUtility.Convert.GetString(currentRow["POID"])}' 
AND Seq1 = '{MyUtility.Convert.GetString(currentRow["Seq1"])}'
AND Seq2 = '{MyUtility.Convert.GetString(currentRow["Seq2"])}'");

                    if (findrow.Length == 0)
                    {
                        currentRow.AcceptChanges();
                        currentRow.SetAdded();
                        this.detailData.ImportRow(currentRow);
                    }
                    else
                    {
                        findrow[0]["Qty"] = currentRow["Qty"];
                        findrow[0]["NetKg"] = currentRow["NetKg"];
                        findrow[0]["WeightKg"] = currentRow["WeightKg"];
                    }
                }
            }

            MyUtility.Msg.InfoBox("Import completed!");
        }
    }
}
