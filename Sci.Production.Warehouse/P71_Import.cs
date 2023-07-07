using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    /// <summary>
    /// P71_Import
    /// </summary>
    public partial class P71_Import : Win.Tems.QueryForm
    {
        private DataTable mainDetail;

        /// <summary>
        /// P71_Import
        /// </summary>
        /// <param name="mainDetail">mainDetail</param>
        public P71_Import(DataTable mainDetail)
        {
            this.InitializeComponent();
            this.EditMode = true;
            this.mainDetail = mainDetail;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.Helper.Controls.Grid.Generator(this.gridImport)
            .CheckBox("selected", trueValue: 1, falseValue: 0, iseditable: true)
            .Text("POID", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("Seq", header: "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Text("MaterialType", header: "MaterialType", width: Widths.AnsiChars(9), iseditingreadonly: true)
            .EditText("Desc", header: "Description", width: Widths.AnsiChars(25), iseditingreadonly: true)
            .Text("Color", header: "Color", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Text("Roll", header: "Roll", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("Tone", header: "Tone/Grp", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Numeric("BalanceQty", header: "Balance Qty", decimal_places: 2, width: Widths.AnsiChars(8),iseditingreadonly: true)
            .Numeric("Qty", header: "Issue Qty", decimal_places: 2, width: Widths.AnsiChars(8))
            .Text("Unit", header: "Unit", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("Location", header: "Location", width: Widths.AnsiChars(15), iseditingreadonly: true)
            ;
        }

        private void Query()
        {
            if (MyUtility.Check.Empty(this.txtSP.Text))
            {
                MyUtility.Msg.WarningBox("SP# can not be empty");
                return;
            }

            string strwhere = string.Empty;
            if (!MyUtility.Check.Empty(this.txtSeq1.Text))
            {
                strwhere += $" and loi.Seq1 = '{this.txtSeq1.Text}'";
            }

            if (!MyUtility.Check.Empty(this.txtseq2.Text))
            {
                strwhere += $" and loi.Seq2 = '{this.txtseq2.Text}'";
            }

            if (!MyUtility.Check.Empty(this.txtLocation.Text))
            {
                strwhere += $" and Location.val like '%{this.txtLocation.Text}%' ";
            }

            string sqlQuery = $@"
select  [selected] = 0,
        loi.POID,
        [Seq] = CONCAT(loi.Seq1,' ',loi.Seq2),
		[MaterialType] = CASE lom.FabricType 
					WHEN 'F' then CONCAT('Fabric-',lom.MtlType)
					WHEN 'A' THEN CONCAT('Accessory-',lom.MtlType) ELSE '' END,
        lom.FabricType,
        loi.Seq1,loi.Seq2,
		lom.[Desc],
		lom.Color,        
        loi.Roll,
        loi.Dyelot,
        loi.Tone,        
        lom.Unit,
        [BalanceQty] = loi.InQty - loi.OutQty + loi.AdjustQty,
        [Qty] = 0,
        [Location] = Location.val
from    LocalOrderInventory loi with (nolock)
left join  LocalOrderMaterial lom with (nolock) on loi.Poid = lom.Poid and loi.Seq1 = lom.Seq1 and loi.Seq2 = lom.Seq2
outer apply (
	SELECT val =  Stuff((select distinct concat( ',',MtlLocationID)   
	from LocalOrderInventory_Location loil with (nolock)
	WHERE loil.LocalOrderInventoryUkey	 = loi.Ukey
	FOR XML PATH('')),1,1,'')  
) Location
where   (loi.InQty - loi.OutQty + loi.AdjustQty) > 0 
and loi.StockType = 'B' 
and loi.POID = '{this.txtSP.Text}'
{strwhere}
";
            DataTable dtResult;
            DualResult result = DBProxy.Current.Select(null, sqlQuery, out dtResult);

            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.gridImport.DataSource = dtResult;
        }

        private void BtnFind_Click(object sender, EventArgs e)
        {
            this.Query();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            var selectedItems = ((DataTable)this.gridImport.DataSource).AsEnumerable().Where(s => (int)s["selected"] == 1);

            if (!selectedItems.Any())
            {
                MyUtility.Msg.WarningBox("Please select rows first!", "Warnning");
                return;
            }

            var chkQtyItems = ((DataTable)this.gridImport.DataSource).AsEnumerable().Where(s => (int)s["selected"] == 1 && MyUtility.Check.Empty(s["Qty"]));
            if (chkQtyItems.Any())
            {
                MyUtility.Msg.WarningBox("Please input issue qty before click import.", "Warnning");
                return;
            }

            var checkMainDetail = this.mainDetail.AsEnumerable();

            foreach (DataRow drImportSource in selectedItems)
            {
                DataRow[] findrow = this.mainDetail.AsEnumerable().Where(row => row.RowState != DataRowState.Deleted
                                                                        && row["poid"].EqualString(drImportSource["poid"].ToString()) 
                                                                        && row["seq1"].EqualString(drImportSource["seq1"])
                                                                        && row["seq2"].EqualString(drImportSource["seq2"].ToString()) 
                                                                        && row["roll"].EqualString(drImportSource["roll"])
                                                                        && row["dyelot"].EqualString(drImportSource["dyelot"])).ToArray();
                if (findrow.Length > 0)
                {
                    findrow[0]["qty"] = drImportSource["qty"];
                }
                else
                {
                    drImportSource.AcceptChanges();
                    drImportSource.SetAdded();
                    this.mainDetail.ImportRow(drImportSource);
                }
            }

            MyUtility.Msg.InfoBox("Import complete!!");
            this.Close();
        }
    }
}
