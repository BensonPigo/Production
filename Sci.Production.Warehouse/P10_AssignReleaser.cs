using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Win.Tools;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P10_AssignReleaser : Win.Tems.QueryForm
    {
        private string id;

        /// <inheritdoc/>
        public P10_AssignReleaser(string id)
        {
            this.InitializeComponent();
            this.id = id;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.GridSetup();
            this.Query();
            this.ControlButton();
        }

        private void GridSetup()
        {
            DataGridViewGeneratorTextColumnSettings releaser = new DataGridViewGeneratorTextColumnSettings();
            releaser.EditingMouseDown += (s, e) =>
            {
                if (!this.EditMode || e.RowIndex == -1)
                {
                    return;
                }

                if (e.Button == MouseButtons.Right)
                {
                    DataRow dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);

                    string sqlcmd = "select ID,Name from Pass1 where Resign is not null order by ID";
                    DualResult result = DBProxy.Current.Select("ManufacturingExecution", sqlcmd, out DataTable dt);
                    if (!result)
                    {
                        this.ShowErr(result);
                        return;
                    }

                    SelectItem item = new SelectItem(dt, "ID,Name", "15,25", dr["Releaser"].ToString());
                    if (item.ShowDialog() == DialogResult.Cancel)
                    {
                        return;
                    }

                    dr["Releaser"] = item.GetSelectedString();
                    dr["ReleaserName"] = item.GetSelecteds()[0]["Name"];
                    dr.EndEdit();
                }
            };
            releaser.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                DataRow dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                if (MyUtility.Check.Empty(e.FormattedValue))
                {
                    dr["Releaser"] = string.Empty;
                    dr["ReleaserName"] = string.Empty;
                    dr.EndEdit();
                    return;
                }

                string sqlcmd = $@"select ID,Name from Pass1 where Resign is not null and ID = '{e.FormattedValue}'";
                if (!MyUtility.Check.Seek(sqlcmd, out DataRow row, "ManufacturingExecution"))
                {
                    dr["Releaser"] = string.Empty;
                    dr["ReleaserName"] = string.Empty;
                    MyUtility.Msg.WarningBox("Releaser not found!");
                }
                else
                {
                    dr["Releaser"] = e.FormattedValue;
                    dr["ReleaserName"] = MyUtility.Convert.GetString(row["Name"]);
                }

                dr.EndEdit();
            };

            this.grid1.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.grid1)
                .Text("Releaser", header: "Releaser", width: Widths.AnsiChars(10), settings: releaser)
                .Text("ReleaserName", header: "Releaser", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("AddNameDisplay", header: "Add Name", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .DateTime("AddDate", header: "Add Date", width: Widths.AnsiChars(20), iseditingreadonly: true)
                ;

            this.grid1.Columns["Releaser"].DefaultCellStyle.BackColor = Color.Pink;
        }

        private void Query()
        {
            string sqlcmd = $@"
select im.*,
    ReleaserName = (select Name from [ExtendServer].ManufacturingExecution.dbo.Pass1 where id = im.Releaser),
    AddNameDisplay = Concat(AddName, '-' + (select Name from pass1 where id = im.AddName))
from Issue_MIND im where id = '{this.id}'";
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out DataTable dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.listControlBindingSource1.DataSource = dt;
        }

        private void BtnAppend_Click(object sender, EventArgs e)
        {
            this.AppendNewRow();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (this.grid1.CurrentDataRow != null)
            {
                if (!this.CheckReleaser(this.grid1.CurrentDataRow))
                {
                    this.grid1.CurrentDataRow.AcceptChanges();
                    this.grid1.CurrentDataRow.Delete();
                }
                else
                {
                    MyUtility.Msg.WarningBox("Releaser already exists.");
                }
            }
        }

        private void BtnDeleteAll_Click(object sender, EventArgs e)
        {
            foreach (DataRow dr in ((DataTable)this.listControlBindingSource1.DataSource).Rows)
            {
                if (!this.CheckReleaser(dr))
                {
                    dr.AcceptChanges();
                    dr.Delete();
                }
            }
        }

        private bool CheckReleaser(DataRow dr)
        {
            string sqlcmd = $@"select 1 from Issue_Detail where ID = '{this.id}' and MINDReleaser = '{dr["Releaser"]}'";
            return MyUtility.Check.Seek(sqlcmd);
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            string sqlcmd = "select ID,Name from Pass1 where Resign is not null order by ID";
            DualResult result = DBProxy.Current.Select("ManufacturingExecution", sqlcmd, out DataTable dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            var oriReleaserlist = ((DataTable)this.listControlBindingSource1.DataSource).AsEnumerable()
                .Where(w => w.RowState != DataRowState.Deleted)
                .Select(s => MyUtility.Convert.GetString(s["Releaser"])).ToList();
            string releasers = oriReleaserlist.JoinToString(",");
            SelectItem2 item = new SelectItem2(dt, "ID,Name", string.Empty, "15,25", releasers);
            item.Width = 600;
            if (item.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            foreach (var dr in item.GetSelecteds())
            {
                if (!oriReleaserlist.Contains(MyUtility.Convert.GetString(dr["ID"])))
                {
                    this.AppendNewRow(dr);
                }
            }

            foreach (DataRow dr in ((DataTable)this.listControlBindingSource1.DataSource).AsEnumerable().Where(w => w.RowState != DataRowState.Deleted))
            {
                if (!item.GetSelectedList().Contains(MyUtility.Convert.GetString(dr["Releaser"])))
                {
                    dr.AcceptChanges();
                    dr.Delete();
                }
            }
        }

        private void AppendNewRow(DataRow dr = null)
        {
            DataRow newRow = ((DataTable)this.listControlBindingSource1.DataSource).NewRow();
            newRow["ID"] = this.id;
            newRow["Releaser"] = dr == null ? string.Empty : dr["ID"];
            newRow["ReleaserName"] = dr == null ? string.Empty : dr["Name"];
            newRow["AddName"] = Sci.Env.User.UserID;
            newRow["AddNameDisplay"] = Sci.Env.User.UserID + "-" + Sci.Env.User.UserName;
            newRow["AddDate"] = DBNull.Value;
            ((DataTable)this.listControlBindingSource1.DataSource).Rows.Add(newRow);
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (this.EditMode)
            {
                foreach (DataRow dr in ((DataTable)this.listControlBindingSource1.DataSource).Rows)
                {
                    if (MyUtility.Check.Empty(dr["Releaser"]))
                    {
                        dr.AcceptChanges();
                        dr.Delete();
                    }
                }

                DualResult result = DBProxy.Current.GetTableSchema(null, "Issue_MIND", out ITableSchema tableSchema);
                if (!result)
                {
                    this.ShowErr(result);
                    return;
                }

                using (TransactionScope scope = new TransactionScope())
                {
                    DateTime datenow = DateTime.Now;
                    foreach (DataRow dr in ((DataTable)this.listControlBindingSource1.DataSource).Rows)
                    {
                        switch (dr.RowState)
                        {
                            case DataRowState.Added:
                                dr["AddDate"] = datenow;
                                result = DBProxy.Current.Insert(null, tableSchema, dr);
                                break;
                            case DataRowState.Deleted:
                                result = DBProxy.Current.Delete(null, tableSchema, dr);
                                break;
                            case DataRowState.Modified:
                                result = DBProxy.Current.UpdateByChanged(null, tableSchema, dr, out bool ischanged);
                                break;
                        }

                        if (!result)
                        {
                            scope.Dispose();
                            this.ShowErr(result);
                            return;
                        }
                    }

                    scope.Complete();
                }
            }

            this.EditMode = !this.EditMode;
            this.ControlButton();
            this.Query();
        }

        private void ControlButton()
        {
            if (this.EditMode)
            {
                this.btnSave.Text = "Save";
                this.btnUndo.Text = "Undo";
            }
            else
            {
                this.btnSave.Text = "Edit";
                this.btnUndo.Text = "Close";
            }
        }

        private void BtnUndo_Click(object sender, EventArgs e)
        {
            if (!this.EditMode)
            {
                this.Close();
            }
            else
            {
                DialogResult buttonResult = MyUtility.Msg.WarningBox("Discard changes?", "Warning", MessageBoxButtons.YesNo);
                if (buttonResult == DialogResult.No)
                {
                    return;
                }

                this.EditMode = false;
                this.ControlButton();
                this.Query();
            }
        }
    }
}
