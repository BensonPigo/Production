using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Win.Tems;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Sci.Production.Subcon
{
    /// <inheritdoc/>
    public partial class B41 : Input1
    {
        /// <inheritdoc/>
        public B41(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.gridIcon1.Insert.Visible = false;
        }

        /// <inheritdoc/>
        protected override void OnEditModeChanged()
        {
            base.OnEditModeChanged();
            if (this.grid1 != null)
            {
                bool edit = this.EditMode && !MyUtility.Convert.GetBool(this.CurrentMaintain["junk"]);
                this.grid1.IsEditingReadOnly = !edit;
            }
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            DataGridViewGeneratorTextColumnSettings t = new DataGridViewGeneratorTextColumnSettings();
            t.MaxLength = 2;
            this.Helper.Controls.Grid.Generator(this.grid1)
                .Text("RFIDProcessTable", header: "Table", width: Widths.AnsiChars(2), settings: t)
                ;
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            if (this.CurrentMaintain != null)
            {
                string sqlcmd = $"select * from RFIDProcessLocation_Detail where id = '{this.CurrentMaintain["ID"]}'";
                DualResult result = DBProxy.Current.Select(null, sqlcmd, out DataTable dt);
                if (!result)
                {
                    this.ShowErr(result);
                }

                this.listControlBindingSource1.DataSource = dt;
            }
        }

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtLocation.ReadOnly = true;
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["ID"]))
            {
                MyUtility.Msg.WarningBox("<Location> can not be empty!");
                return false;
            }

            foreach (DataRow dr in ((DataTable)this.listControlBindingSource1.DataSource).Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    dr["ID"] = this.CurrentMaintain["ID"];
                }
            }

            if (((DataTable)this.listControlBindingSource1.DataSource).AsEnumerable().Where(w => w.RowState != DataRowState.Deleted)
                .GroupBy(g => MyUtility.Convert.GetString(g["RFIDProcessTable"]))
                .Select(s => new { s.Key, ct = s.Count() })
                .Any(a => a.ct > 1))
            {
                MyUtility.Msg.WarningBox("Table can't duplicate!");
                return false;
            }

            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override DualResult ClickSavePost()
        {
            string sqlcmd = $@"
Delete RFIDProcessLocation_Detail where id = '{this.CurrentMaintain["ID"]}'
insert into RFIDProcessLocation_Detail
select ID,RFIDProcessTable from #tmp where isnull(RFIDProcessTable, '') <>''
";
            DualResult result = MyUtility.Tool.ProcessWithDatatable((DataTable)this.listControlBindingSource1.DataSource, string.Empty, sqlcmd, out DataTable dt);
            if (!result)
            {
                return result;
            }

            return base.ClickSavePost();
        }

        private void GridIcon1_AppendClick(object sender, System.EventArgs e)
        {
            DataRow nr = ((DataTable)this.listControlBindingSource1.DataSource).NewRow();
            ((DataTable)this.listControlBindingSource1.DataSource).Rows.Add(nr);
        }

        private void GridIcon1_RemoveClick(object sender, System.EventArgs e)
        {
            if (this.grid1.CurrentDataRow != null)
            {
                this.grid1.CurrentDataRow.Delete();
            }
        }

        private void CheckJunk_CheckedChanged(object sender, System.EventArgs e)
        {
            if (this.CurrentMaintain != null)
            {
                this.CurrentMaintain["junk"] = this.checkJunk.Checked;
                bool edit = this.EditMode && !MyUtility.Convert.GetBool(this.CurrentMaintain["junk"]);
                this.grid1.IsEditingReadOnly = !edit;
            }
        }
    }
}
