using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using Sci.Win.UI;
using Sci.Data;
using Ict.Win;
using Ict;

namespace Sci.Production.Class
{
    /// <summary>
    /// TxtCFALocation
    /// </summary>
    public partial class TxtCFALocation : Win.UI.TextBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TxtCFALocation"/> class.
        /// </summary>
        public TxtCFALocation()
        {
            this.Size = new System.Drawing.Size(80, 23);
            this.IsSupportSytsemContextMenu = false;
        }

        /// <summary>
        /// MDivision ID
        /// </summary>
        [Category("Custom Properties")]
        public Control MDivisionObjectName { get; set; }

        /// <summary>
        /// MDivision ID
        /// </summary>
        [Category("Custom Properties")]
        public string M { get; set; }

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
                string sql = "select ID,Description from CFALocation WITH (NOLOCK) order by ID";
                if (this.MDivisionObjectName != null && !string.IsNullOrWhiteSpace((string)this.MDivisionObjectName.Text))
                {
                    sql = string.Format("select ID,Description from CFALocation WITH (NOLOCK) where MDivisionID = '{0}' and junk=0 order by ID", this.MDivisionObjectName.Text);
                }

                if (!MyUtility.Check.Empty(this.M))
                {
                    sql = string.Format("select ID,Description from CFALocation WITH (NOLOCK) where MDivisionID = '{0}' and junk=0 order by ID", this.M);
                }

                DataTable tbCFALocation;
                DBProxy.Current.Select("Production", sql, out tbCFALocation);
                Win.Tools.SelectItem item = new Win.Tools.SelectItem(tbCFALocation, "ID,Description", "10,40,10", this.Text, "ID,Description");

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
                    if (MyUtility.Check.Seek(str, "CFALocation", "id") == false)
                    {
                        this.Text = string.Empty;
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox(string.Format("< CFALocation : {0} > not found!!!", str));
                        return;
                    }
                }
                else
                {
                    if (this.MDivisionObjectName != null)
                    {
                        if (!string.IsNullOrWhiteSpace((string)this.MDivisionObjectName.Text))
                        {
                            string selectCommand = string.Format("select ID from CFALocation WITH (NOLOCK) where MDivisionID = '{0}' and ID = '{1}'", (string)this.MDivisionObjectName.Text, str);
                            if (!MyUtility.Check.Seek(selectCommand, null))
                            {
                                this.Text = string.Empty;
                                e.Cancel = true;
                                MyUtility.Msg.WarningBox(string.Format("< CFALocation : {0} > not found!!!", str));
                                return;
                            }
                        }
                    }

                    if (!MyUtility.Check.Empty(this.M))
                    {
                        if (!string.IsNullOrWhiteSpace((string)this.MDivisionObjectName.Text))
                        {
                            string selectCommand = string.Format("select ID from CFALocation WITH (NOLOCK) where MDivisionID = '{0}' and ID = '{1}'", this.M, str);
                            if (!MyUtility.Check.Seek(selectCommand, null))
                            {
                                this.Text = string.Empty;
                                e.Cancel = true;
                                MyUtility.Msg.WarningBox(string.Format("< CFALocation : {0} > not found!!!", str));
                                return;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// CellCFALocation
        /// </summary>
        public class CellCFALocation : DataGridViewGeneratorTextColumnSettings
        {
            /// <summary>
            /// GetGridCell
            /// </summary>
            /// <param name="mdivisionID">Mdivision</param>
            /// <returns>DataGridViewGeneratorTextColumnSettings</returns>
            public static DataGridViewGeneratorTextColumnSettings GetGridCell(string mdivisionID)
            {
                // pur 為ture 表示需判斷PurchaseFrom
                CellCFALocation ts = new CellCFALocation();
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

                        DataRow row = grid.GetDataRow<DataRow>(e.RowIndex);
                        DataTable tbCFALocation;
                        string sql = $@"select ID,Description from CFALocation WITH (NOLOCK) where MDivisionID = '{mdivisionID.ToString().Trim()}' and junk=0 order by ID ";
                        DBProxy.Current.Select("Production", sql, out tbCFALocation);
                        Win.Tools.SelectItem item = new Win.Tools.SelectItem(tbCFALocation, "ID,Description", "10,40", row["CFALocationID"].ToString(), "ID,Description,M");
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

                    // 右鍵彈出功能
                    DataRow row = grid.GetDataRow<DataRow>(e.RowIndex);
                    string oldValue = row["CFALocationID"].ToString();
                    string newValue = e.FormattedValue.ToString(); // user 編輯當下的value , 此值尚未存入DataRow
                    string sql;

                    sql = $@"select ID from CFALocation WITH (NOLOCK) where MDivisionID = '{mdivisionID.ToString().Trim()}' and ID = '{newValue}' and junk=0 ";
                    if (!MyUtility.Check.Empty(newValue) && oldValue != newValue)
                    {
                        if (!MyUtility.Check.Seek(sql))
                        {
                            row["CFALocationID"] = string.Empty;
                            row.EndEdit();
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox($"< CFALocation : {newValue}> not found.");
                            return;
                        }
                    }
                };
                return ts;
            }
        }
    }
}
