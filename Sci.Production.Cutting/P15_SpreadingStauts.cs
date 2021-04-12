using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Automation;
using Sci.Production.Prg;
using Sci.Production.PublicPrg;
using Sci.Win.Tools;
using Sci.Win.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;
using static Sci.Production.Cutting.BatchCreateData;

namespace Sci.Production.Cutting
{
    /// <inheritdoc/>
    public partial class P15_SpreadingStauts : Win.Tems.QueryForm
    {
        private readonly string sizeRatio;
        private readonly DataTable spDt;
        private DataTable Dt = new DataTable();

        /// <inheritdoc/>
        public P15_SpreadingStauts(DataRow cutrefrow, string sizeRatio, DataTable spDt)
        {
            this.InitializeComponent();
            this.sizeRatio = sizeRatio;
            this.spDt = spDt;
            this.txtCutRef.Text = MyUtility.Convert.GetString(cutrefrow["cutref"]);
            this.txtSPNo.Text = MyUtility.Convert.GetString(cutrefrow["OrderID"]);
            this.txtPOID.Text = MyUtility.Convert.GetString(cutrefrow["poid"]);
            this.dateEstCutDate.Value = MyUtility.Convert.GetDate(cutrefrow["estcutdate"]);
            this.txtFabricCombo.Text = MyUtility.Convert.GetString(cutrefrow["Fabriccombo"]);
            this.txtPatternPanel.Text = MyUtility.Convert.GetString(cutrefrow["FabricPanelCode"]);
            this.numCutNo.Text = MyUtility.Convert.GetString(cutrefrow["Cutno"]);
            this.txtItem.Text = MyUtility.Convert.GetString(cutrefrow["Item"]);
            this.dispFabricKind.Text = MyUtility.Convert.GetString(cutrefrow["FabricKind"]);
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.gridIcon1.Insert.Visible = false;
            this.EditMode = true;
            if (this.spDt.Rows.Count > 1)
            {
                this.txtSPNo.BackColor = Color.Yellow;
            }

            this.GridSetup();
        }

        private void GridSetup()
        {
            this.Dt.Columns.Add("LayerBatch", typeof(int));
            this.Dt.Columns.Add("SizeRatiio", typeof(string));
            this.Dt.Columns.Add("NoofLayer", typeof(int));
            this.Dt.Columns.Add("ToneChar", typeof(string));
            this.gridbs.DataSource = this.Dt;
            DataGridViewGeneratorTextColumnSettings tone = new DataGridViewGeneratorTextColumnSettings() { MaxLength = 1 };
            tone.EditingKeyPress += (s, e) =>
            {
                var regex = new Regex(@"[^A-Z\b\s]");
                if (regex.IsMatch(e.KeyChar.ToString()))
                {
                    e.Handled = true;
                }
            };

            this.grid1.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.grid1)
                .Numeric("LayerBatch", header: "Layer Batch", width: Widths.AnsiChars(1), iseditingreadonly: true)
                .Text("SizeRatiio", header: "Size/Ratiio", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Numeric("NoofLayer", header: "# Of Layer", width: Widths.AnsiChars(1))
                .Text("ToneChar", header: "Tone", width: Widths.AnsiChars(1), settings: tone)
                ;
        }

        private void BtnSPs_Click(object sender, EventArgs e)
        {
            if (this.spDt.AsEnumerable().Any())
            {
                MsgGridForm m = new MsgGridForm(this.spDt, "SP# List") { Width = 650 };
                m.grid1.Columns[0].Width = 140;
                m.text_Find.Width = 140;
                m.btn_Find.Location = new Point(150, 6);
                m.btn_Find.Anchor = AnchorStyles.Left | AnchorStyles.Top;
                this.FormClosing += (s, args) =>
                {
                    if (m.Visible)
                    {
                        m.Close();
                    }
                };
                m.Show(this);
            }
        }

        private void GridIcon1_AppendClick(object sender, EventArgs e)
        {
            DataRow newRow = this.Dt.NewRow();
            newRow["SizeRatiio"] = this.sizeRatio;
            this.Dt.Rows.Add(newRow);
            this.SerialNumber();
        }

        private void GridIcon1_RemoveClick(object sender, EventArgs e)
        {
            if (this.grid1.CurrentDataRow != null)
            {
                this.grid1.CurrentDataRow.Delete();
                this.Dt.AcceptChanges();
                this.SerialNumber();
            }
        }

        private void SerialNumber()
        {
            int i = 1;
            this.Dt.AsEnumerable().ToList().ForEach(f => f["LayerBatch"] = i++);
        }

        private void BtnNext_Click(object sender, EventArgs e)
        {
            if (this.BeforeNext())
            {
                P15.SpreadingDT = this.Dt;
                P15.SpreadingType = this.radioPanel1.Value;
                this.Close();
            }
        }

        private bool BeforeNext()
        {
            if (!this.Dt.AsEnumerable().Any())
            {
                return false;
            }

            if (this.Dt.AsEnumerable().Where(w => MyUtility.Check.Empty(w["NoofLayer"])).Any())
            {
                MyUtility.Msg.WarningBox("<# Of Layer > cannot be empty.");
                return false;
            }

            return true;
        }

        private void P15_SpreadingStauts_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Dispose();
        }
    }
}
