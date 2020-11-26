using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Automation;
using Sci.Production.PublicPrg;
using static Sci.Production.PublicPrg.Prgs;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// P10_DeleteGarmentBookingHistory
    /// </summary>
    public partial class P10_DeleteGarmentBookingHistory : Win.Subs.Base
    {
        private DataTable masterDataTable_init = new DataTable();
        private DataTable masterDataTable;

        /// <summary>
        /// Initializes a new instance of the <see cref="P10_DeleteGarmentBookingHistory"/> class.
        /// </summary>
        /// <param name="masterData">DataTable</param>
        public P10_DeleteGarmentBookingHistory(DataTable masterData)
        {
            this.InitializeComponent();
            this.masterDataTable_init = masterData.Copy();
            this.masterDataTable = masterData;
            this.EditMode = false;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            DataGridViewGeneratorTextColumnSettings ressonID = Class.TxtShippingReason.CellShippingReason.GetGridCell("DG", "ReasonID", "Description");
            DataGridViewGeneratorDateColumnSettings dateSettings = new DataGridViewGeneratorDateColumnSettings();
            DataGridViewGeneratorTextColumnSettings textSettings = new DataGridViewGeneratorTextColumnSettings();

            ressonID.CellEditable += (s, e) =>
            {
                if (this.EditMode)
                {
                    var dr = this.gridShipPlanDeleteGBHistory.GetDataRow<DataRow>(e.RowIndex);
                    if (dr == null || dr["Type"].EqualString("T"))
                    {
                        e.IsEditable = false;
                    }
                }
            };

            dateSettings.CellEditable += (s, e) =>
            {
                if (this.EditMode)
                {
                    var dr = this.gridShipPlanDeleteGBHistory.GetDataRow<DataRow>(e.RowIndex);
                    if (dr == null || dr["Type"].EqualString("T"))
                    {
                        e.IsEditable = false;
                    }
                }
            };

            textSettings.CellEditable += (s, e) =>
            {
                if (this.EditMode)
                {
                    var dr = this.gridShipPlanDeleteGBHistory.GetDataRow<DataRow>(e.RowIndex);
                    if (dr == null || dr["Type"].EqualString("T"))
                    {
                        e.IsEditable = false;
                    }
                }
            };

            this.listControlBindingSource1.DataSource = this.masterDataTable;
            this.gridShipPlanDeleteGBHistory.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridShipPlanDeleteGBHistory)
                .Text("GMTBookingID", header: "GB#", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("ReasonID", header: "Reason", width: Widths.AnsiChars(20), settings: ressonID, iseditable: true)
                .Text("Description", header: "Reason Desc", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Date("BackDate", header: "Back Date", settings: dateSettings, iseditable: true)
                .Text("NewShipModeID", header: "New Ship Mode", width: Widths.AnsiChars(15), settings: textSettings, iseditingreadonly: false)
                .Date("NewPulloutDate", header: "New Pullout Date", settings: dateSettings, iseditable: true)
                .EditText("NewDestination", header: "New Destination", settings: textSettings, iseditingreadonly: false)
                .EditText("Remark", header: "Remark", settings: textSettings, iseditingreadonly: false)
                .Text("AddName", header: "Delete By", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Date("AddDate", header: "Delete Date", iseditingreadonly: true);

            this.gridShipPlanDeleteGBHistory.Sorted += (s, e) =>
            {
                this.GridChangeColor();
            };

            // change status
            var dataRows = this.masterDataTable.AsEnumerable().Where(x => x.Field<string>("Type").EqualString("N"));

            if (!dataRows.Any())
            {
                this.btnSave.Visible = false;
            }
        }

        private void Init()
        {
            this.masterDataTable.Clear();
            this.masterDataTable.Merge(this.masterDataTable_init);
            this.listControlBindingSource1.DataSource = this.masterDataTable;
            this.GridChangeColor();
        }

        /// <inheritdoc/>
        protected override void OnEditModeChanged()
        {
            base.OnEditModeChanged();
            this.btnSave.Text = this.EditMode ? "Save" : "Edit";
            this.btnClose.Text = this.EditMode ? "Undo" : "Close";
            this.GridChangeColor();
        }

        private void GridChangeColor()
        {
            for (int i = 0; i <= this.gridShipPlanDeleteGBHistory.Rows.Count - 1; i++)
            {
                DataGridViewRow dataGridViewRow = this.gridShipPlanDeleteGBHistory.Rows[i];
                if (this.EditMode && this.gridShipPlanDeleteGBHistory.GetDataRow(i)["Type"].EqualString("N"))
                {
                    dataGridViewRow.Cells["ReasonID"].Style.BackColor = Color.Pink;
                    dataGridViewRow.Cells["BackDate"].Style.BackColor = Color.Pink;
                    dataGridViewRow.Cells["NewShipModeID"].Style.BackColor = Color.Pink;
                    dataGridViewRow.Cells["NewPulloutDate"].Style.BackColor = Color.Pink;
                    dataGridViewRow.Cells["NewDestination"].Style.BackColor = Color.Pink;
                    dataGridViewRow.Cells["Remark"].Style.BackColor = Color.Pink;
                }
                else
                {
                    dataGridViewRow.Cells["ReasonID"].Style.BackColor = Color.White;
                    dataGridViewRow.Cells["BackDate"].Style.BackColor = Color.White;
                    dataGridViewRow.Cells["NewShipModeID"].Style.BackColor = Color.White;
                    dataGridViewRow.Cells["NewPulloutDate"].Style.BackColor = Color.White;
                    dataGridViewRow.Cells["NewDestination"].Style.BackColor = Color.White;
                    dataGridViewRow.Cells["Remark"].Style.BackColor = Color.White;
                }
            }
        }

        // Save
        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (this.EditMode)
            {
                string msg = string.Empty;
                var dataRows = this.masterDataTable.AsEnumerable().Where(x => x.Field<string>("Type").EqualString("N")).ToList();
                foreach (var dataRow in dataRows)
                {
                    if (dataRow["ReasonID"].Empty())
                    {
                        msg += string.Format("{0} Reason cannot be empty!", dataRow["GMTBookingID"]) + Environment.NewLine;
                    }

                    if (dataRow["ReasonID"].EqualString("DG001") && dataRow["BackDate"].Empty())
                    {
                        msg += string.Format("{0} Back Date cannot be empty!", dataRow["GMTBookingID"]) + Environment.NewLine;
                    }

                    if (dataRow["ReasonID"].EqualString("DG002") && dataRow["NewShipModeID"].Empty())
                    {
                        msg += string.Format("{0} New Ship Mode cannot be empty!", dataRow["GMTBookingID"]) + Environment.NewLine;
                    }

                    if (dataRow["ReasonID"].EqualString("DG003") && dataRow["NewPulloutDate"].Empty())
                    {
                        msg += string.Format("{0} New Pullout Date cannot be empty!", dataRow["GMTBookingID"]) + Environment.NewLine;
                    }

                    if (dataRow["ReasonID"].EqualString("DG004") && dataRow["NewDestination"].Empty())
                    {
                        msg += string.Format("{0} New Destination cannot be empty!", dataRow["GMTBookingID"]) + Environment.NewLine;
                    }

                    if (dataRow["ReasonID"].EqualString("DG007") && dataRow["Remark"].Empty())
                    {
                        msg += string.Format("{0} Remark cannot be empty!!", dataRow["GMTBookingID"]) + Environment.NewLine;
                    }
                }

                if (!msg.Empty())
                {
                    MyUtility.Msg.ErrorBox(msg);
                    return;
                }

                // Save
                this.EditMode = false;
            }
            else
            {
                // change status
                var dataRows = this.masterDataTable.AsEnumerable().Where(x => x.Field<string>("Type").EqualString("N"));

                if (dataRows.Any())
                {
                    this.EditMode = true;
                }
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            if (this.btnClose.Text == "Close")
            {
                this.Close();
            }
            else
            {
                this.EditMode = !this.EditMode;

                // 回到檢視模式，並且重新取得資料
                this.Init();
            }
        }
    }
}
