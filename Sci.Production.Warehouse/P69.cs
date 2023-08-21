using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P69 : Win.Tems.QueryForm
    {

        /// <inheritdoc/>
        public P69(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.EditMode = true;
        }

        /// <inheritdoc/>
        public P69(string p01_SP, ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.EditMode = true;
            this.txtSP.Text = p01_SP;
            this.Event_Query();
        }

        /// <inheritdoc/>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (this.txtSP.Focused)
            {
                switch (keyData)
                {
                    case Keys.Enter:
                        this.Event_Query();
                        break;
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            DataGridViewGeneratorNumericColumnSettings colBalance = new DataGridViewGeneratorNumericColumnSettings();
            colBalance.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.grid.GetDataRow<DataRow>(e.RowIndex);
                if (dr == null)
                {
                    return;
                }

                var form = new P69_Transaction(dr);
                form.ShowDialog(this);
                this.Event_Query();
            };

            this.Helper.Controls.Grid.Generator(this.grid)
            .Text("POID", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(13))
            .Text("Seq", header: "Seq", iseditingreadonly: true, width: Widths.AnsiChars(6))
            .ExtText("Description", header: "Description", iseditingreadonly: true, width: Widths.AnsiChars(25))
            .Text("Color", header: "Color", iseditingreadonly: true, width: Widths.AnsiChars(10))
            .Text("Unit", header: "Unit", iseditingreadonly: true, width: Widths.AnsiChars(5))
            .Numeric("InQty", header: "In Qty", iseditingreadonly: true, decimal_places: 2, width: Widths.AnsiChars(4))
            .Numeric("OutQty", header: "Out Qty", iseditingreadonly: true, decimal_places: 2, width: Widths.AnsiChars(4))
            .Numeric("AdjustQty", header: "Adjust Qty", iseditingreadonly: true, decimal_places: 2, width: Widths.AnsiChars(4))
            .Numeric("Balance", header: "Balance", iseditingreadonly: true, decimal_places: 2, width: Widths.AnsiChars(4), settings: colBalance)
            .Text("BulkLocation", header: "Bulk Location", iseditingreadonly: true)
            ;
        }

        /// <inheritdoc/>
        public void Event_Query()
        {
            string sqlWhere = string.Empty;
            List<SqlParameter> sqlParameters = new List<SqlParameter>();

            if (MyUtility.Check.Empty(this.txtSP.Text))
            {
                MyUtility.Msg.WarningBox("SP# can't be empty. Please fill SP# first!");
                this.txtSP.Focus();
                this.grid.DataSource = null;
                return;
            }

            sqlParameters.Add(new SqlParameter("@POID", this.txtSP.Text));
            sqlParameters.Add(new SqlParameter("@POID1", this.txtSP.Text + "%"));

            if (!MyUtility.Check.Seek($@"SELECT * FROM Orders o WITH(NOLOCK) WHERE o.LocalOrder = 1 AND o.ID = @POID", sqlParameters))
            {
                MyUtility.Msg.WarningBox($"SP# :{this.txtSP.Text} cannot be found!");
                return;
            }

            string sqlQuery = $@"
            SELECT 
            [POID] = lom.POID,
            [Seq] = CONCAT(lom.Seq1,'-',lom.Seq2),
            [Seq1] = lom.Seq1,
            [Seq2] = lom.Seq2,
            [FabricType] = lom.FabricType,
            [Refno] = lom.Refno,
            [Description] = lom.[Desc],
            [Color] = lom.Color,
            [Unit] = lom.Unit,
            [InQty] = countValue.InQty,
            [OutQty] = countValue.OutQty,
            [AdjustQty] = countValue.AdjustQty,
            [Balance] = countValue.Balance,
            [BulkLocation] = BulkLocation.val,
            [WeaveType] = lom.WeaveType,
            [MtlType] = lom.MtlType,
            [Size] = lom.SizeCode
            FROM LocalOrderMaterial lom WITH(NOLOCK)
            OUTER APPLY
            (
	            SELECT 
	            [InQty] = SUM(ISNULL(loi.InQty,0)),
	            [OutQty] = SUM(ISNULL(loi.OutQty,0)),
	            [AdjustQty] = SUM(ISNULL(loi.AdjustQty,0)),
	            [Balance] = SUM(ISNULL(loi.InQty,0))-SUM(ISNULL(loi.OutQty,0))+SUM(ISNULL(loi.AdjustQty,0))
	            FROM LocalOrderInventory loi WITH(NOLOCK)
	            WHERE loi.POID = lom.POID AND loi.Seq1 = lom.Seq1 AND loi.Seq2 = lom.Seq2
            )countValue
            OUTER APPLY
            (
	            select val = stuff((
	            select concat(',',tmp.MtlLocationID) from
	            (
		            SELECT distinct  lol.MtlLocationID
		            FROM LocalOrderMaterial a WITH(NOLOCK)
		            INNER JOIN LocalOrderInventory loi  ON loi.POID = a.POID AND loi.Seq1 = a.Seq1 AND loi.Seq2 = a.Seq2
		            INNER JOIN LocalOrderInventory_Location lol ON lol.LocalOrderInventoryUkey = loi.Ukey
		            WHERE a.POID = lom.POID AND a.Seq1 = lom.Seq1 AND a.Seq2 = lom.Seq2
		            and lol.MtlLocationID != ''
		            and lol.MtlLocationID is not null
	            ) tmp for xml path('')),1,1,'')
            )BulkLocation
            WHERE lom.POID LIKE @POID1
            ";
            DataTable dtResult;
            DualResult result = DBProxy.Current.Select(null, sqlQuery, sqlParameters, out dtResult);

            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            if (dtResult.Rows.Count == 0)
            {
                this.grid.DataSource = null;
                MyUtility.Msg.WarningBox($"Data not found.");
                return;
            }

            this.grid.DataSource = dtResult;
        }

        /// <inheritdoc/>
        private void BtnNewSearch_Click(object sender, EventArgs e)
        {
            this.txtSP.Text = string.Empty;
            this.txtSP.Focus();
        }

        private void BtnCreateSeq_Click(object sender, EventArgs e)
        {
            var windows = new P69_CreateSeq(true);
            windows.ShowDialog(this);
            if (windows.GetBoolImport())
            {
                this.txtSP.Text = windows.GetPOID();
                this.Event_Query();
            }
        }

        private void Edit_Seq_Click(object sender, EventArgs e)
        {
            if (this.grid == null || this.grid.Rows.Count == 0)
            {
                return;
            }

            int rowIndex = this.grid.CurrentCell.RowIndex;
            DataRow dr = this.grid.GetDataRow(rowIndex);
            if (dr == null)
            {
                return;
            }

            var windows = new P69_CreateSeq(false, dr);
            windows.ShowDialog();
            if (windows.GetBoolImport())
            {
                this.Event_Query();
            }
        }

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            this.Event_Query();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
