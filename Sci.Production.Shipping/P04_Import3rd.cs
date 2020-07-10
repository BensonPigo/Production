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

            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
            cmds.Add(sp1);
            cmds.Add(sp2);

            string sqlCmd = @"
select 1 as Selected,psd.ID as POID,psd.SEQ1,psd.SEQ2, (left(psd.SEQ1+' ',3)+'-'+psd.SEQ2) as Seq,ps.SuppID,
(ps.SuppID+'-'+(select AbbEN from Supp WITH (NOLOCK) where ID = ps.SuppID)) as Supp,psd.Refno,psd.SCIRefno,
isnull(f.DescDetail,'') as Description, psd.FabricType, (case when psd.FabricType = 'F' then 'Fabric' when psd.FabricType = 'A' then 'Accessory' else '' end) as Type, 
isnull(f.MtlTypeID,'') as MtlTypeID,psd.POUnit as UnitId,psd.ShipQty,psd.ShipFOC
,(psd.ShipQty+psd.ShipFOC) as Qty,0.0 as NetKg,0.0 as WeightKg,
o.BuyerDelivery,isnull(o.BrandID,'') as BrandID,isnull(o.FactoryID,'') as FactoryID,o.SciDelivery
from PO_Supp ps WITH (NOLOCK) 
left join PO_Supp_Detail psd WITH (NOLOCK) on ps.ID = psd.ID and ps.SEQ1 = psd.SEQ1
left join Fabric f WITH (NOLOCK) on f.SCIRefno = psd.SCIRefno
left join Orders o WITH (NOLOCK) on o.ID = ps.ID
where ps.ID = @poid
and psd.ShipQty+psd.ShipFOC <>0
and ps.SuppID = @suppid";
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
