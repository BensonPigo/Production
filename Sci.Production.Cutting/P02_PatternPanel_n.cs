using Ict;
using Ict.Win;
using Sci.Production.Prg;
using System;
using System.Data;
using System.Linq;

namespace Sci.Production.Cutting
{
    /// <inheritdoc/>
    public partial class P02_PatternPanel_n : Sci.Win.Tems.QueryForm
    {
        private readonly string ID;
        private readonly long UKey;
        private readonly long NewKey;
        private DataTable PatternPanelTb;
        private DataTable PatternPanelTbTmp;

        /// <inheritdoc/>
        public P02_PatternPanel_n(DataTable patternPanelTb, DataRow currentDataRow)
        {
            this.InitializeComponent();
            this.EditMode = true;
            this.ID = MyUtility.Convert.GetString(currentDataRow["ID"]);
            this.UKey = MyUtility.Convert.GetLong(currentDataRow["UKey"]);
            this.NewKey = MyUtility.Convert.GetLong(currentDataRow["NewKey"]);
            this.PatternPanelTb = patternPanelTb;
            this.PatternPanelTbTmp = patternPanelTb.Select($@"Workorderukey = {this.UKey} and newkey = {this.NewKey}").TryCopyToDataTable(patternPanelTb);
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.GridSetup();
        }

        private void GridSetup()
        {
            this.grid1.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.grid1)
                .Text("PatternPanel", header: "Pattern Panel", width: Widths.AnsiChars(2))
                .Text("FabricPanelCode", header: "Fab_Panel Code", width: Widths.AnsiChars(2));
            this.listControlBindingSource1.DataSource = this.PatternPanelTbTmp;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            this.grid1.ValidateControl();
            this.PatternPanelTb.Select($@"Workorderukey = {this.UKey} and newkey = {this.NewKey}").ToList().ForEach(row => row.Delete());
            this.PatternPanelTbTmp.ToList().Where(w => w.RowState != DataRowState.Deleted).ToList().ForEach(row => this.PatternPanelTb.ImportRowAdded(row));
            this.Close();
        }

        private void BtnAppend_Click(object sender, EventArgs e)
        {
            DataRow newRow = this.PatternPanelTbTmp.NewRow();
            newRow["ID"] = this.ID;
            newRow["Workorderukey"] = this.UKey;
            newRow["newkey"] = this.NewKey;
            newRow["PatternPanel"] = string.Empty;
            newRow["FabricPanelCode"] = string.Empty;
            this.PatternPanelTbTmp.Rows.Add(newRow);
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (this.grid1.SelectedRows.Count > 0)
            {
                this.grid1.Rows.Remove(this.grid1.SelectedRows[0]);
            }
        }

        private void BtnUndo_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
