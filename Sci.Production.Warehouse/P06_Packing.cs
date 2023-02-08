using Ict;
using Ict.Win;
using Ict.Win.Defs;
using Sci.Data;
using Sci.Production.Prg;
using Sci.Win.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    /// <summary>
    /// P06_Packing
    /// </summary>
    public partial class P06_Packing : Sci.Win.Tems.QueryForm
    {
        private string transferExportID;
        private DataTable dtCarton = new DataTable();
        /// <summary>
        /// P06_Packing
        /// </summary>
        /// <param name="transferExportID">transferExportID</param>
        public P06_Packing(string transferExportID)
        {
            this.InitializeComponent();
            this.EditMode = false;
            this.transferExportID = transferExportID;
            this.btnAssignCarton.Visible = this.EditMode;
            this.btnCancelAssingCarton.Visible = this.EditMode;

            this.dtCarton.Columns.Add("Carton", typeof(string));
            this.dtCarton.Columns.Add("NetKg", typeof(decimal));
            this.dtCarton.Columns.Add("WeightKg", typeof(decimal));
            this.dtCarton.Columns.Add("CBM", typeof(decimal));

            this.gridNotAssignCarton.DataSource = this.bindingSourceNotAssignCarton;
            this.gridAssignedCarton.DataSource = this.bindingSourceAssignedCarton;
            this.gridCartonList.DataSource = this.bindingSourceCartonList;
            this.bindingSourceAssignedCarton.Filter = "StockQty > 0";
            this.bindingSourceNotAssignCarton.Filter = "StockQty > 0";
            this.bindingSourceCartonList.DataSource = this.dtCarton;
        }

        /// <inheritdoc/>
        protected override void OnEditModeChanged()
        {
            base.OnEditModeChanged();
            if (this.btnEditSave == null)
            {
                return;
            }

            this.btnEditSave.Text = this.EditMode ? "Save" : "Edit";
            this.btnAssignCarton.Visible = this.EditMode;
            this.btnCancelAssingCarton.Visible = this.EditMode;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            DataGridViewGeneratorNumericColumnSettings colAssignQty = new DataGridViewGeneratorNumericColumnSettings();
            colAssignQty.CellValidating += (s, e) =>
            {
                decimal assignQty = MyUtility.Convert.GetDecimal(e.FormattedValue);
                decimal stockQty = MyUtility.Convert.GetDecimal(this.gridNotAssignCarton.GetDataRow(e.RowIndex)["StockQty"]);

                if (assignQty > stockQty)
                {
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox("[Assign Export Q'ty] cannot be bigger than [Export Q'ty].");
                    return;
                }

                if (assignQty == 0)
                {
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(" [Assign Export Q'ty] cannot be 0.");
                    return;
                }
            };

            this.Helper.Controls.Grid.Generator(this.gridNotAssignCarton)
                .CheckBox("select", trueValue: true, falseValue: false, iseditable: true)
                .Text("InventoryPOID", header: "From SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("FromSEQ", header: "From SEQ", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Text("POID", header: "To SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("ToSEQ", header: "To SEQ", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Text("Roll", header: "Roll", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("StockQty", header: "Export Qty", width: Widths.AnsiChars(13), decimal_places: 2, iseditingreadonly: true)
                .Text("Carton", header: "Assign Carton No", width: Widths.AnsiChars(6), iseditingreadonly: false)
                .Numeric("AssignQty", header: "Assign Export Q'ty", width: Widths.AnsiChars(13), decimal_places: 2, iseditingreadonly: false, settings: colAssignQty);

            this.Helper.Controls.Grid.Generator(this.gridAssignedCarton)
                .CheckBox("select", trueValue: true, falseValue: false, iseditable: true)
                .Text("InventoryPOID", header: "From SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("FromSEQ", header: "From SEQ", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Text("POID", header: "To SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("ToSEQ", header: "To SEQ", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Text("Roll", header: "Roll", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Carton", header: "Assign Carton No", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Numeric("StockQty", header: "Export Qty", width: Widths.AnsiChars(13), decimal_places: 2, iseditingreadonly: true);

            this.Helper.Controls.Grid.Generator(this.gridCartonList)
                .Text("Carton", header: "Assign Carton No", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Numeric("NetKg", header: "N.W. (kg)", width: Widths.AnsiChars(12), decimal_places: 2, iseditingreadonly: false)
                .Numeric("WeightKg", header: "G.W. (kg)", width: Widths.AnsiChars(12), decimal_places: 2, iseditingreadonly: false)
                .Numeric("CBM", header: "CBM", width: Widths.AnsiChars(12), decimal_places: 5, iseditingreadonly: false);

            // 設定grid cell color
            this.gridNotAssignCarton.Columns["Carton"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridNotAssignCarton.Columns["AssignQty"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridCartonList.Columns["NetKg"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridCartonList.Columns["WeightKg"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridCartonList.Columns["CBM"].DefaultCellStyle.BackColor = Color.Pink;

            this.Query();
        }

        private void BtnEditSave_Click(object sender, EventArgs e)
        {
            if (this.EditMode)
            {

            }

            this.EditMode = !this.EditMode;
        }

        private void Query()
        {
            string sqlQuery = $@"
select	[select] = cast(0 as bit),
		ted.InventoryPOID,
		[FromSeq] = Concat(ted.InventorySeq1, ' ', ted.InventorySeq2),
		ted.PoID,
		[ToSEQ] = Concat(ted.Seq1, ' ', ted.Seq2),
		ted.Seq1,
		ted.Seq2,
		tedc.Roll,
		[Dyelot] = tedc.LotNo,
		tedc.StockQty,
		tedc.Carton,
		[AssignQty] = tedc.StockQty,
		tedc.TransferExport_DetailUkey,
        ted.FabricType
from TransferExport_Detail ted with (nolock)
inner join TransferExport_Detail_Carton tedc with (nolock) on tedc.TransferExport_DetailUkey = ted.Ukey
where ted.ID = '{this.transferExportID}'
";

            DataTable dtResult;

            DualResult result = DBProxy.Current.Select(null, sqlQuery, out dtResult);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            if (dtResult.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("No Data Found");
                return;
            }

            DataTable dtNotAssignCarton = dtResult.AsEnumerable().Where(s => MyUtility.Check.Empty(s["Carton"])).TryCopyToDataTable(dtResult);
            DataTable dtAssignCarton = dtResult.AsEnumerable().Where(s => !MyUtility.Check.Empty(s["Carton"])).TryCopyToDataTable(dtResult);

            if (dtAssignCarton.Rows.Count > 0)
            {
                var listCarton = dtAssignCarton.AsEnumerable()
                            .Where(s => !MyUtility.Check.Empty(s["NetKg"]) || !MyUtility.Check.Empty(s["WeightKg"]) || !MyUtility.Check.Empty(s["CBM"]))
                            .GroupBy(s => s["Carton"].ToString());

                foreach (var item in listCarton)
                {
                    DataRow newRow = this.dtCarton.NewRow();
                    newRow["Carton"] = item.Key;
                    newRow["NetKg"] = item.First()["NetKg"];
                    newRow["WeightKg"] = item.First()["WeightKg"];
                    newRow["CBM"] = item.First()["CBM"];

                    this.dtCarton.Rows.Add(newRow);
                }
            }

            this.bindingSourceNotAssignCarton.DataSource = dtNotAssignCarton;
            this.bindingSourceAssignedCarton.DataSource = dtAssignCarton;

            this.GridRowStatusChange();
        }

        private void GridRowStatusChange()
        {
            foreach (DataGridViewRow dr in this.gridNotAssignCarton.Rows)
            {
                if (MyUtility.Check.Empty(dr.Cells["StockQty"].Value))
                {
                    dr.ReadOnly = true;
                }

                if (this.gridNotAssignCarton.GetDataRow(dr.Index)["FabricType"].ToString() == "F")
                {
                    dr.Cells["AssignQty"].ReadOnly = true;
                }
            }
        }

        private DualResult Save()
        {
            return new DualResult(true);
        }

        private void BtnCancelAssingCarton_Click(object sender, EventArgs e)
        {
            var selectRow = ((DataTable)this.bindingSourceAssignedCarton.DataSource).AsEnumerable().Where(s => MyUtility.Convert.GetBool(s["select"]));

            if (!selectRow.Any())
            {
                MyUtility.Msg.WarningBox("Please tick one data row on below grid first .");
                return;
            }

            DataTable dtNotAssignedCarton = (DataTable)this.bindingSourceNotAssignCarton.DataSource;

            foreach (DataRow dr in selectRow)
            {
                string filterExpression = $@"
InventoryPOID = '{dr["InventoryPOID"]}' and
FromSEQ = '{dr["FromSEQ"]}' and
POID = '{dr["POID"]}' and
ToSEQ = '{dr["ToSEQ"]}' and
Roll = '{dr["Roll"]}' and
Dyelot = '{dr["Dyelot"]}' ";
                DataRow[] drResult = dtNotAssignedCarton.Select(filterExpression);

                if (drResult.Length == 0)
                {
                    DataRow newRow = dtNotAssignedCarton.NewRow();

                    newRow["select"] = false;
                    newRow["InventoryPOID"] = dr["InventoryPOID"];
                    newRow["FromSeq"] = dr["FromSeq"];
                    newRow["PoID"] = dr["PoID"];
                    newRow["ToSEQ"] = dr["ToSEQ"];
                    newRow["Seq1"] = dr["Seq1"];
                    newRow["Seq2"] = dr["Seq2"];
                    newRow["Roll"] = dr["Roll"];
                    newRow["Dyelot"] = dr["Dyelot"];
                    newRow["StockQty"] = dr["StockQty"];
                    newRow["Carton"] = string.Empty;
                    newRow["AssignQty"] = dr["StockQty"];
                    newRow["TransferExport_DetailUkey"] = dr["TransferExport_DetailUkey"];
                    newRow["FabricType"] = dr["FabricType"];

                    dtNotAssignedCarton.Rows.Add(newRow);
                }
                else
                {
                    drResult[0]["StockQty"] = (decimal)dr["StockQty"] + (decimal)drResult[0]["StockQty"];
                }

                dr["StockQty"] = 0;
                dr["select"] = false;
            }

            this.CartonListRefresh();
        }

        private void BtnAssignCarton_Click(object sender, EventArgs e)
        {
            var selectRow = ((DataTable)this.bindingSourceNotAssignCarton.DataSource).AsEnumerable().Where(s => MyUtility.Convert.GetBool(s["select"]));

            if (!selectRow.Any())
            {
                MyUtility.Msg.WarningBox("Please tick one data row on top grid first .");
                return;
            }

            if (selectRow.Any(s => MyUtility.Check.Empty(s["Carton"])))
            {
                MyUtility.Msg.WarningBox("[Assign Carton No] cannot be empty.");
                return;
            }

            if (selectRow.Any(s => MyUtility.Check.Empty(s["AssignQty"])))
            {
                MyUtility.Msg.WarningBox("[Assign Export Qty] cannot be 0.");
                return;
            }

            DataTable dtAssignedCarton = (DataTable)this.bindingSourceAssignedCarton.DataSource;

            foreach (DataRow dr in selectRow)
            {
                string filterExpression = $@"
InventoryPOID = '{dr["InventoryPOID"]}' and
FromSEQ = '{dr["FromSEQ"]}' and
POID = '{dr["POID"]}' and
ToSEQ = '{dr["ToSEQ"]}' and
Roll = '{dr["Roll"]}' and
Dyelot = '{dr["Dyelot"]}' ";
                DataRow[] drResult = dtAssignedCarton.Select(filterExpression);

                if (drResult.Length == 0)
                {
                    DataRow newRow = dtAssignedCarton.NewRow();

                    newRow["select"] = false;
                    newRow["InventoryPOID"] = dr["InventoryPOID"];
                    newRow["FromSeq"] = dr["FromSeq"];
                    newRow["PoID"] = dr["PoID"];
                    newRow["ToSEQ"] = dr["ToSEQ"];
                    newRow["Seq1"] = dr["Seq1"];
                    newRow["Seq2"] = dr["Seq2"];
                    newRow["Roll"] = dr["Roll"];
                    newRow["Dyelot"] = dr["Dyelot"];
                    newRow["StockQty"] = dr["StockQty"];
                    newRow["Carton"] = dr["Carton"];
                    newRow["AssignQty"] = dr["StockQty"];
                    newRow["TransferExport_DetailUkey"] = dr["TransferExport_DetailUkey"];
                    newRow["FabricType"] = dr["FabricType"];

                    dtAssignedCarton.Rows.Add(newRow);
                }
                else
                {
                    drResult[0]["StockQty"] = (decimal)dr["StockQty"] + (decimal)drResult[0]["StockQty"];
                }

                dr["StockQty"] = 0;
                dr["select"] = false;
            }

            this.CartonListRefresh();
        }

        private void CartonListRefresh()
        {
            var allCarton = ((DataTable)this.bindingSourceAssignedCarton.DataSource).AsEnumerable()
                .Where(s => MyUtility.Convert.GetDecimal(s["StockQty"]) > 0)
                .Select(s => s["Carton"].ToString()).Distinct();

            var listCarton = this.dtCarton.AsEnumerable();

            foreach (string newCarton in allCarton)
            {
                if (!listCarton.Any(s => s["Carton"].ToString() == newCarton))
                {
                    DataRow newRow = this.dtCarton.NewRow();
                    newRow["Carton"] = newCarton;
                    this.dtCarton.Rows.Add(newRow);
                }
            }

            for (int i = this.dtCarton.Rows.Count - 1; i >= 0; i--)
            {
                if (!allCarton.Any(s => s == this.dtCarton.Rows[i]["Carton"].ToString()))
                {
                    this.dtCarton.Rows.RemoveAt(i);
                }
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
