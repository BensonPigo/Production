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
    /// P63
    /// </summary>
    public partial class P63 : Win.Tems.QueryForm
    {
        /// <summary>
        /// P63
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P63(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.EditMode = true;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            DataGridViewGeneratorNumericColumnSettings colBalance = new DataGridViewGeneratorNumericColumnSettings();
            colBalance.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.gridSemiFinishedInventory.GetDataRow<DataRow>(e.RowIndex);
                if (dr == null)
                {
                    return;
                }

                var form = new P63_Transaction(dr);
                form.Show(this);
            };

            this.Helper.Controls.Grid.Generator(this.gridSemiFinishedInventory)
            .Text("POID", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(13))
            .Text("Refno", header: "Refno", iseditingreadonly: true, width: Widths.AnsiChars(16))
            .ExtText("Description", header: "Description", iseditingreadonly: true, width: Widths.AnsiChars(25))
            .Text("Unit", header: "Unit", iseditingreadonly: true, width: Widths.AnsiChars(5))
            .Numeric("InQty", header: "In Qty", iseditingreadonly: true, decimal_places: 2, width: Widths.AnsiChars(4))
            .Numeric("OutQty", header: "Out Qty", iseditingreadonly: true, decimal_places: 2, width: Widths.AnsiChars(4))
            .Numeric("AdjustQty", header: "Adjust Qty", iseditingreadonly: true, decimal_places: 2, width: Widths.AnsiChars(4))
            .Numeric("Balance", header: "Balance", iseditingreadonly: true, decimal_places: 2, width: Widths.AnsiChars(4), settings: colBalance)
            .Text("BulkLocation", header: "Bulk Location", iseditingreadonly: true)
            ;
        }

        private void Query()
        {
            string sqlWhere = string.Empty;

            if (!MyUtility.Check.Empty(this.txtSP.Text))
            {
                sqlWhere += $"and sfi.POID = '{this.txtSP.Text}'";
            }

            string sqlQuery = $@"
select  sfi.POID,
        sfi.Refno,
        sf.Description,
        sf.Unit,
        [InQty] = sum(isnull(sfi.InQty, 0)),
        [OutQty] = sum(isnull(sfi.OutQty, 0)),
        [AdjustQty] = sum(isnull(sfi.AdjustQty, 0)),
        [Balance] = sum(isnull(sfi.InQty, 0)) - sum(isnull(sfi.OutQty, 0)) + sum(isnull(sfi.AdjustQty, 0)),
        [BulkLocation] = BulkLocation.val,
        sfi.StockType
from    SemiFinishedInventory sfi with (nolock) 
inner join  SemiFinished sf with (nolock) on sf.Refno = sfi.Refno
outer apply(SELECT val =  Stuff((select distinct concat( ',',sfl.MtlLocationID)   
                                    from SemiFinishedInventory_Location sfl with (nolock)
                                    where   sfl.POID        = sfi.POID          and
                                            sfl.Refno       = sfi.Refno         and
                                            sfl.StockType   = sfi.StockType
                                FOR XML PATH('')),1,1,'') 
                ) BulkLocation
where   sfi.StockType  = 'B' {sqlWhere}
group by    sfi.POID,
            sfi.Refno,
            sf.Description,
            sf.Unit,
            sfi.StockType,
            BulkLocation.val
";
            DataTable dtResult;
            DualResult result = DBProxy.Current.Select(null, sqlQuery, out dtResult);

            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.gridSemiFinishedInventory.DataSource = dtResult;
        }

        /// <inheritdoc/>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (this.txtSP.Focused)
            {
                switch (keyData)
                {
                    case Keys.Enter:
                        this.Query();
                        break;
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnNewSearch_Click(object sender, EventArgs e)
        {
            this.txtSP.Text = string.Empty;
            this.txtSP.Focus();
        }

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            this.Query();
        }
    }
}
