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
    /// P10_PulloutDateReason
    /// </summary>
    public partial class P10_PulloutDateReason : Win.Subs.Base
    {
        private DataTable masterDataTable;
        private DataTable masterDataTable_init;

        /// <summary>
        /// Initializes a new instance of the <see cref="P10_PulloutDateReason"/> class.
        /// </summary>
        /// <param name="masterData">DataTable</param>
        public P10_PulloutDateReason(DataTable masterData)
        {
            this.InitializeComponent();
            this.masterDataTable = masterData.Select("CutOffdate < PulloutDate").CopyToDataTable();
            this.masterDataTable_init = masterData;
            this.EditMode = true;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            DataGridViewGeneratorTextColumnSettings ressonID = Class.TxtShippingReason.CellShippingReason.GetGridCell("CO", "ShippingReasonIDForTypeCO", "Description");

            ressonID.CellEditable += (s, e) =>
            {
                if (this.EditMode)
                {
                    var dr = this.grid.GetDataRow<DataRow>(e.RowIndex);
                    if (dr == null)
                    {
                        e.IsEditable = false;
                    }
                }
            };

            this.listControlBindingSource1.DataSource = this.masterDataTable;
            this.grid.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.grid)
                .Text("INVNO", header: "GB#", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("ID", header: "Packing No.", width: Widths.AnsiChars(14), iseditingreadonly: true)
                .Text("OrderID", header: "SP#", width: Widths.AnsiChars(14), iseditingreadonly: true)
                .Text("OrderShipmodeSeq", header: "Seq", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .DateTime("CutOffdate", header: "Cut-off Date/Time", iseditingreadonly: true)
                .Date("PulloutDate", header: "Pullout Date", iseditingreadonly: true)
                .Text("ShippingReasonIDForTypeCO", header: "Reason ID", width: Widths.AnsiChars(8), settings: ressonID, iseditable: true)
                .Text("Description", header: "Reason Desc.", width: Widths.AnsiChars(90), iseditingreadonly: true);

            this.grid.Sorted += (s, e) =>
            {
                this.GridChangeColor();
            };

            this.GridChangeColor();

            if (this.masterDataTable != null && this.masterDataTable.Rows.Count > 0)
            {
                this.grid.Rows[0].Selected = false;
            }
        }

        /// <inheritdoc/>
        protected override void OnEditModeChanged()
        {
            base.OnEditModeChanged();
        }

        private void GridChangeColor()
        {
            for (int i = 0; i <= this.grid.Rows.Count - 1; i++)
            {
                DataGridViewRow dataGridViewRow = this.grid.Rows[i];
                if (this.EditMode)
                {
                    dataGridViewRow.Cells["ShippingReasonIDForTypeCO"].Style.BackColor = Color.Pink;
                }
                else
                {
                    dataGridViewRow.Cells["ShippingReasonIDForTypeCO"].Style.BackColor = Color.White;
                }
            }
        }

        // Save
        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (this.EditMode)
            {
                string msg = string.Empty;
                foreach (DataRow dataRow in this.masterDataTable.Rows)
                {
                    if (dataRow["ShippingReasonIDForTypeCO"].Empty())
                    {
                        msg += string.Format("{0} Reason cannot be empty!", dataRow["INVNO"]) + Environment.NewLine;
                        DataRow dr = this.masterDataTable_init.Select($"ID = '{dataRow["ID"]}'").FirstOrDefault();
                        dr["ShippingReasonIDForTypeCO"] = string.Empty;
                        dr["Description"] = string.Empty;
                    }
                    else
                    {
                        DataRow dr = this.masterDataTable_init.Select($"ID = '{dataRow["ID"]}'").FirstOrDefault();
                        dr["ShippingReasonIDForTypeCO"] = dataRow["ShippingReasonIDForTypeCO"];
                        dr["Description"] = dataRow["Description"];
                    }
                }

                if (!msg.Empty())
                {
                    MyUtility.Msg.ErrorBox(msg);
                    return;
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
