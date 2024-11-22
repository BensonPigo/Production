using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
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
        private bool isGroup;
        private List<string> listID;
        private DataRow[] drArray;

        /// <inheritdoc/>
        public P10_AssignReleaser(string id, bool isGroup = false, DataTable dataTable = null)
        {
            this.InitializeComponent();
            this.isGroup = isGroup;

            if (!isGroup)
            {
                this.id = id;
                this.listID = id.Split(new string[] { "','" }, StringSplitOptions.None).ToList();
                if (dataTable != null)
                {
                    this.drArray = dataTable.AsEnumerable().Where(x => x["ID"].ToString() == id).ToArray();
                }
            }
            else
            {
                this.id = id;
                this.listID = id.Split(new string[] { "','" }, StringSplitOptions.None).ToList();
                this.drArray = dataTable.AsEnumerable()
                    .Where(row => this.listID.Contains(row["ID"].ToString()))
                    .ToArray();
            }
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.GridSetup();
            this.Query();
            this.ControlButton();

            if (this.isGroup)
            {
                DataTable dd = (DataTable)this.listControlBindingSource1.DataSource;

                for (int i = dd.Rows.Count - 1; i >= 0; i--)
                {
                    if (this.grid1.CurrentDataRow != null)
                    {
                        if (!this.CheckReleaser(dd.Rows[i]))
                        {
                            dd.Rows[i].AcceptChanges();
                            dd.Rows[i].Delete();
                        }
                    }
                }

                this.listControlBindingSource1.DataSource = dd;
            }
            else
            {
                if (this.drArray == null)
                {
                    this.drArray = new DataRow[0];
                }
                DataTable dd = (DataTable)this.listControlBindingSource1.DataSource;
                // 將 drArray 轉換為 List<DataRow>，以方便新增操作
                List<DataRow> drList = drArray.ToList();

                // 假設你有一個新的 DataRow
                DataRow newRow = dd.NewRow(); // 創建新的 DataRow（你可以從現有 DataTable 中生成）

                newRow["ID"] = this.id;
                drList.Add(newRow);

                // 將 List 轉換回陣列
                drArray = drList.ToArray();
            }
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

                    string sqlcmd = "select ID,Name from Pass1 where Resign is null order by ID";
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

                string sqlcmd = $@"select ID,Name from Pass1 where Resign is null and ID = '{e.FormattedValue}'";
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
            select
            im.*
            ,ReleaserName = (select Name from [ExtendServer].ManufacturingExecution.dbo.Pass1 where id = im.Releaser)
            ,AddNameDisplay = Concat(AddName, '-' + (select Name from pass1 where id = im.AddName))
            from Issue_MIND im where ID IN ('{this.id}')";
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
                if (dr.RowState == DataRowState.Deleted)
                {
                    continue;
                }

                if (!this.CheckReleaser(dr))
                {
                    dr.AcceptChanges();
                    dr.Delete();
                }
            }
        }

        private bool CheckReleaser(DataRow dr)
        {
            string sqlcmd = $@"select 1 from Issue_Detail where ID in('{this.id}') and MINDReleaser = '{dr["Releaser"]}'";
            return MyUtility.Check.Seek(sqlcmd);
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            string sqlcmd = "select ID,Name from Pass1 where Resign is null order by ID";
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
            DataTable copyDT = ((DataTable)this.listControlBindingSource1.DataSource).Copy();
            if (this.EditMode)
            {
                foreach (DataRow dataRow in this.drArray)
                {
                    string strReleaser = string.Empty;
                    foreach (DataRow dr in ((DataTable)this.listControlBindingSource1.DataSource).AsEnumerable().Where(w => w.RowState != DataRowState.Deleted).OrderBy(w => w.Field<string>("Releaser")))
                    {
                        if (MyUtility.Check.Empty(dr["Releaser"]))
                        {
                            dr.AcceptChanges();
                            dr.Delete();
                        }
                        else
                        {
                            strReleaser += MyUtility.Convert.GetString(dr["Releaser"]) + ",";

                            if (this.isGroup)
                            {
                                if (MyUtility.Convert.GetString(dataRow["ID"]) == MyUtility.Convert.GetString(this.drArray[0]["ID"]))
                                {
                                    dr["ID"] = dataRow["ID"].ToString();
                                }
                                else
                                {
                                    DataRow newRow = ((DataTable)this.listControlBindingSource1.DataSource).NewRow();
                                    newRow["ID"] = dataRow["ID"].ToString();
                                    newRow["Releaser"] = dr == null ? string.Empty : dr["Releaser"];
                                    newRow["ReleaserName"] = dr == null ? string.Empty : dr["ReleaserName"];
                                    newRow["AddName"] = Sci.Env.User.UserID;
                                    newRow["AddNameDisplay"] = Sci.Env.User.UserID + "-" + Sci.Env.User.UserName;
                                    newRow["AddDate"] = DBNull.Value;
                                    ((DataTable)this.listControlBindingSource1.DataSource).Rows.Add(newRow);
                                }
                            }
                        }
                    }

                    if (!MyUtility.Check.Empty(strReleaser))
                    {
                        dataRow["Releaser"] = strReleaser.Substring(0, strReleaser.Length - 1);
                    }
                    else
                    {
                        dataRow["Releaser"] = string.Empty;
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

                if (!this.isGroup)
                {
                    this.Query();
                }
                else
                {
                    var filteredRows = copyDT.AsEnumerable()
                    .Where(row => row.RowState != DataRowState.Deleted && !MyUtility.Check.Empty(row["Releaser"]));

                    DataTable dt;
                    if (filteredRows.Any())
                    {
                        dt = filteredRows.CopyToDataTable();
                    }
                    else
                    {
                        dt = copyDT.Clone(); // 返回結構相同但不包含數據的空 DataTable
                    }

                    this.listControlBindingSource1.DataSource = dt;
                }
            }

            this.EditMode = !this.EditMode;
            this.ControlButton();

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
