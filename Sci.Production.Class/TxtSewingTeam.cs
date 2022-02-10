using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using Sci.Win.UI;
using Sci.Data;
using Ict.Win;
using Ict;
using System.Collections.Generic;

namespace Sci.Production.Class
{
    /// <summary>
    /// TxtSewingTeam
    /// </summary>
    public partial class TxtSewingTeam : Win.UI.TextBox
    {
        /// <inheritdoc/>
        [Category("IssupportJunk")]
        public bool IssupportJunk { get; set; } = false;

        /// <inheritdoc/>
        [Category("IssupportEmptyitem")]
        public bool IssupportEmptyitem { get; set; } = false;

        /// <inheritdoc/>
        public TxtSewingTeam()
        {
            this.Size = new System.Drawing.Size(88, 23);
            this.IsSupportSytsemContextMenu = false;
        }

        /// <inheritdoc/>
        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            base.OnPopUp(e);
            if (e.IsHandled)
            {
                return;
            }

            Win.Tems.Base myform = (Win.Tems.Base)this.FindForm();
            if (myform.EditMode)
            {
                List<string> listFilte = new List<string>();
                if (!this.IssupportJunk)
                {
                    listFilte.Add("Junk = 0");
                }

                string where = string.Empty;
                if (listFilte.Count > 0)
                {
                    where = "where " + listFilte.JoinToString("\n\rand ");
                }

                string sqlEmptyitem = $"select ID = '' union all ";
                string sqlcmd = $@"select ID from SewingTeam {where}";
                if (this.IssupportEmptyitem)
                {
                    sqlcmd = sqlEmptyitem + sqlcmd;
                }

                DBProxy.Current.Select("Production", sqlcmd, out DataTable tbSewingTeam);
                Win.Tools.SelectItem item = new Win.Tools.SelectItem(tbSewingTeam, "ID", "10", this.Text, "ID");
                DialogResult result = item.ShowDialog();
                if (result == DialogResult.Cancel)
                {
                    return;
                }

                this.Text = item.GetSelectedString();
                e.IsHandled = true;
            }
        }

        /// <inheritdoc/>
        protected override void OnValidating(CancelEventArgs e)
        {
            base.OnValidating(e);

            string str = this.Text;
            if (!MyUtility.Check.Empty(str))
            {
                if (str != this.OldValue)
                {
                    string where = string.Empty;
                    if (!this.IssupportJunk)
                    {
                        where += " and Junk = 0";
                    }

                    string sqlcmd = $@"select ID from SewingTeam where id = '{str}' {where}";
                    if (!MyUtility.Check.Seek(sqlcmd, "Production"))
                    {
                        this.Text = string.Empty;
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox(string.Format("< SewingTeam : {0} > not found!!!", str));
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// CellSewingTeam
        /// </summary>
        public class CellSewingTeam : DataGridViewGeneratorTextColumnSettings
        {
            /// <inheritdoc/>
            public static DataGridViewGeneratorTextColumnSettings GetGridCell(bool issupportJunk, string columnName)
            {
                CellSewingTeam ts = new CellSewingTeam();
                ts.EditingMouseDown += (s, e) =>
                {
                    // 右鍵彈出功能
                    if (e.Button == MouseButtons.Right)
                    {
                        DataGridView grid = ((DataGridViewColumn)s).DataGridView;

                        // Parent form 若是非編輯狀態就 return
                        if (!((Win.Forms.Base)grid.FindForm()).EditMode)
                        {
                            return;
                        }

                        if (MyUtility.Check.Empty(e.FormattedValue))
                        {
                            return;
                        }

                        DataRow row = grid.GetDataRow<DataRow>(e.RowIndex);
                        string where = string.Empty;
                        if (!issupportJunk)
                        {
                            where += " and Junk = 0";
                        }

                        string sqlcmd = $@"select ID from SewingTeam where 1=1 {where}";
                        DBProxy.Current.Select("Production", sqlcmd, out DataTable tbSewingTeam);
                        Win.Tools.SelectItem item = new Win.Tools.SelectItem(tbSewingTeam, "ID", "10", row[columnName].ToString(), "ID");
                        DialogResult result = item.ShowDialog();
                        if (result == DialogResult.Cancel)
                        {
                            return;
                        }

                        var sellist = item.GetSelecteds();
                        e.EditingControl.Text = item.GetSelectedString();
                    }
                };

                // 正確性檢查
                ts.CellValidating += (s, e) =>
                {
                    DataGridView grid = ((DataGridViewColumn)s).DataGridView;

                    // Parent form 若是非編輯狀態就 return
                    if (!((Win.Forms.Base)grid.FindForm()).EditMode)
                    {
                        return;
                    }

                    if (MyUtility.Check.Empty(e.FormattedValue))
                    {
                        return;
                    }

                    // 右鍵彈出功能
                    DataRow row = grid.GetDataRow<DataRow>(e.RowIndex);
                    string oldValue = row[columnName].ToString();
                    string newValue = e.FormattedValue.ToString(); // user 編輯當下的value , 此值尚未存入DataRow
                    string sql = $@"select ID from SewingTeam where ID = '{newValue}' and junk=0 ";
                    DBProxy.Current.Select("Production", sql, out DataTable tbSewingTeam);
                    if (tbSewingTeam.Rows.Count == 0)
                    {
                        row[columnName] = string.Empty;
                        row.EndEdit();
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox($"< SewingTeam : {newValue}> not found.");
                        return;
                    }

                    if (oldValue != newValue)
                    {
                        row[columnName] = newValue;
                        row.EndEdit();
                    }
                };
                return ts;
            }
        }
    }
}
