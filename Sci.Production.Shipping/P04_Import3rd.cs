using System;
using System.Collections.Generic;
using System.Data;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// P04_Import3rd
    /// </summary>
    public partial class P04_Import3rd : Win.Subs.Base
    {
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        private DataTable detailData;

        /// <summary>
        /// P04_Import3rd
        /// </summary>
        /// <param name="dt">dt</param>
        public P04_Import3rd(DataTable dt)
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
                .Numeric("WeightKg", header: "G.W.(kg)", decimal_places: 2);
        }

        // Qurey
        private void BtnQuery_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.txtSPNo.Text))
            {
                this.txtSPNo.Focus();
                MyUtility.Msg.WarningBox("SP# can't be empty!");
                return;
            }

            if (MyUtility.Check.Empty(this.txtsupplier.TextBox1.Text))
            {
                this.txtsupplier.TextBox1.Focus();
                MyUtility.Msg.WarningBox("Supplier can't be empty!");
                return;
            }

            // sql參數
            System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter("@poid", this.txtSPNo.Text.Trim());
            System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter("@suppid", this.txtsupplier.TextBox1.Text.Trim());
            System.Data.SqlClient.SqlParameter sp3 = new System.Data.SqlClient.SqlParameter("@OrderCompanyID", P04.orderCompanyID);

            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>
            {
                sp1,
                sp2,
                sp3
            };

            string sqlCmd = @"
SELECT 
    1 AS Selected,
    psd.ID AS POID,
    psd.SEQ1,
    psd.SEQ2,
    (LEFT(psd.SEQ1 + ' ', 3) + '-' + psd.SEQ2) AS Seq,
    ps.SuppID,
    (ps.SuppID + '-' + 
        (SELECT AbbEN 
         FROM Supp WITH (NOLOCK) 
         WHERE ID = ps.SuppID)) AS Supp,
    psd.Refno,
    psd.SCIRefno,
    ISNULL(f.DescDetail, '') AS Description,
    psd.FabricType,
    (CASE 
        WHEN psd.FabricType = 'F' THEN 'Fabric' 
        WHEN psd.FabricType = 'A' THEN 'Accessory' 
        ELSE '' 
     END) AS Type, 
    ISNULL(f.MtlTypeID, '') AS MtlTypeID,
    psd.POUnit AS UnitId,
    psd.ShipQty,
    psd.ShipFOC,
    (psd.ShipQty + psd.ShipFOC) AS Qty,
    0.0 AS NetKg,
    0.0 AS WeightKg,
    o.BuyerDelivery,
    ISNULL(o.BrandID, '') AS BrandID,
    ISNULL(o.FactoryID, '') AS FactoryID,
    o.SciDelivery
FROM 
    PO_Supp ps WITH (NOLOCK)
LEFT JOIN 
    PO_Supp_Detail psd WITH (NOLOCK) ON ps.ID = psd.ID AND ps.SEQ1 = psd.SEQ1
LEFT JOIN 
    Fabric f WITH (NOLOCK) ON f.SCIRefno = psd.SCIRefno
LEFT JOIN 
    Orders o WITH (NOLOCK) ON o.ID = ps.ID
WHERE 
    ps.ID = @poid
    AND (psd.ShipQty + psd.ShipFOC) <> 0
    AND ps.SuppID = @suppid
    AND o.OrderCompanyID = @OrderCompanyID
;

";
            DataTable selectData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out selectData);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query error." + result.ToString());
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
                    DataRow[] findrow = this.detailData.Select(string.Format("POID = '{0}' and Seq1 = '{1}' and Seq2 = '{2}'", MyUtility.Convert.GetString(currentRow["POID"]), MyUtility.Convert.GetString(currentRow["Seq1"]), MyUtility.Convert.GetString(currentRow["Seq2"])));
                    if (findrow.Length == 0)
                    {
                        currentRow.AcceptChanges();
                        currentRow.SetAdded();
                        this.detailData.ImportRow(currentRow);
                    }
                    else
                    {
                        findrow[0]["Qty"] = MyUtility.Convert.GetDouble(currentRow["Qty"]);
                        findrow[0]["NetKg"] = MyUtility.Convert.GetDouble(currentRow["NetKg"]);
                        findrow[0]["WeightKg"] = MyUtility.Convert.GetDouble(currentRow["WeightKg"]);
                    }
                }
            }

            MyUtility.Msg.InfoBox("Import completed!");
        }
    }
}
