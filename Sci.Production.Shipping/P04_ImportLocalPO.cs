using System;
using System.Data;
using System.Text;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// P04_ImportLocalPO
    /// </summary>
    public partial class P04_ImportLocalPO : Win.Subs.Base
    {
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        private DataTable detailData;

        /// <summary>
        /// P04_ImportLocalPO
        /// </summary>
        /// <param name="dt">dt</param>
        public P04_ImportLocalPO(DataTable dt)
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
                .Text("TransactionID", header: "Local Purchase#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("POID", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("Supp", header: "Supplier", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("RefNo", header: "Ref#", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("ThreadColorID", header: "Color Shade", width: Widths.AnsiChars(9), iseditingreadonly: true)
                .Text("MtlTypeID", header: "Material Type", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("UnitId", header: "Unit", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Numeric("Qty", header: "Q'ty", decimal_places: 2)
                .Numeric("NetKg", header: "N.W.(kg)", decimal_places: 2)
                .Numeric("WeightKg", header: "N.W.(kg)", decimal_places: 2);
        }

        // Qurey
        private void BtnQuery_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.txtSPNo.Text) && MyUtility.Check.Empty(this.txtLocalPurchase.Text))
            {
                this.txtSPNo.Focus();
                MyUtility.Msg.WarningBox("< SP# > or < Local Purchase# > can't be empty!");
                return;
            }

            if (MyUtility.Convert.GetString(this.txtSPNo.Text).IndexOf("'") != -1)
            {
                MyUtility.Msg.WarningBox("SP# can not enter the  '  character!!");
                return;
            }

            if (MyUtility.Convert.GetString(this.txtLocalPurchase.Text).IndexOf("'") != -1)
            {
                MyUtility.Msg.WarningBox("Local Purchase# can not enter the  '  character!!");
                return;
            }

            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(@"select lo.*, 1 as Selected, (lo.SuppID+' - '+ls.Abb) as Supp,isnull(li.Description,'') as Description,li.Category as MtlTypeID,
o.BuyerDelivery,isnull(o.BrandID,'') as BrandID,isnull(o.FactoryID,'') as FactoryID,o.SciDelivery,0.0 as NetKg,0.0 as WeightKg,
'' as Seq1,'' as Seq2,'' as Seq,'' as FabricType
from (select l.Id as TransactionID,ld.OrderId as POID,l.LocalSuppID as SuppID,SUBSTRING(ld.Id+ld.ThreadColorID,1,26) as SCIRefno,ld.Refno,ld.ThreadColorID,ld.UnitId,ld.Qty,ld.Price
      from LocalPO l WITH (NOLOCK) , LocalPO_Detail ld WITH (NOLOCK) 
	  where l.Id = ld.Id");
            if (!MyUtility.Check.Empty(this.txtLocalPurchase.Text))
            {
                sqlCmd.Append(string.Format(" and l.id = '{0}'", this.txtLocalPurchase.Text.Trim()));
            }

            if (!MyUtility.Check.Empty(this.txtSPNo.Text))
            {
                sqlCmd.Append(string.Format(" and ld.OrderId = '{0}'", this.txtSPNo.Text.Trim()));
            }

            if (!MyUtility.Check.Empty(this.txtSubconSupplier.TextBox1.Text))
            {
                sqlCmd.Append(string.Format(" and l.LocalSuppID = '{0}'", this.txtSubconSupplier.TextBox1.Text.Trim()));
            }

            sqlCmd.Append(@") lo
left join Orders o on o.ID = lo.POID
left join LocalItem li on li.RefNo = lo.Refno
left join LocalSupp ls on ls.ID = lo.SuppID");
            DataTable selectData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out selectData);
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
                    DataRow[] findrow = this.detailData.Select($@"
TransactionID = '{MyUtility.Convert.GetString(currentRow["TransactionID"])}' 
AND POID = '{MyUtility.Convert.GetString(currentRow["POID"])}' 
AND SCIRefNo = '{MyUtility.Convert.GetString(currentRow["SCIRefNo"])}'
AND RefNo = '{MyUtility.Convert.GetString(currentRow["RefNo"])}'");

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
