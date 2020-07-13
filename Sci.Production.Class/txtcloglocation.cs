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
    /// Txtcloglocation
    /// </summary>
    public partial class Txtcloglocation : Win.UI.TextBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Txtcloglocation"/> class.
        /// </summary>
        public Txtcloglocation()
        {
            this.Size = new System.Drawing.Size(80, 23);
            this.IsSupportSytsemContextMenu = false;
        }

        /// <summary>
        /// MDivision
        /// </summary>
        [Category("Custom Properties")]
        public Control MDivisionObjectName { get; set; }

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
                string sql = "select ID,Description,MDivisionID from ClogLocation WITH (NOLOCK) order by ID";
                if (this.MDivisionObjectName != null && !string.IsNullOrWhiteSpace((string)this.MDivisionObjectName.Text))
                {
                    sql = string.Format("select ID,Description,MDivisionID from ClogLocation WITH (NOLOCK) where MDivisionID = '{0}' and junk=0 order by ID", this.MDivisionObjectName.Text);
                }

                DataTable tbClogLocation;
                DBProxy.Current.Select("Production", sql, out tbClogLocation);
                Win.Tools.SelectItem item = new Win.Tools.SelectItem(tbClogLocation, "ID,Description,MDivisionID", "10,40,10", this.Text, "ID,Description,M");

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
                    if (MyUtility.Check.Seek(str, "ClogLocation", "id") == false)
                    {
                        this.Text = string.Empty;
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox(string.Format("< ClogLocation : {0} > not found!!!", str));
                        return;
                    }
                }
                else
                {
                    if (this.MDivisionObjectName != null)
                    {
                        if (!string.IsNullOrWhiteSpace((string)this.MDivisionObjectName.Text))
                        {
                            string selectCommand = string.Format("select ID from ClogLocation WITH (NOLOCK) where MDivisionID = '{0}' and ID = '{1}'", (string)this.MDivisionObjectName.Text, str);
                            if (!MyUtility.Check.Seek(selectCommand, null))
                            {
                                this.Text = string.Empty;
                                e.Cancel = true;
                                MyUtility.Msg.WarningBox(string.Format("< ClogLocation : {0} > not found!!!", str));
                                return;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// CellClogLocation
        /// </summary>
        public class CellClogLocation : DataGridViewGeneratorTextColumnSettings
        {
            /// <summary>
            /// GetGridCell
            /// </summary>
            /// <param name="mdivisionID">mdivision</param>
            /// <returns>DataGridViewGeneratorTextColumnSettings</returns>
            public static DataGridViewGeneratorTextColumnSettings GetGridCell(string mdivisionID)
            {
                // pur 為ture 表示需判斷PurchaseFrom
                CellClogLocation ts = new CellClogLocation();
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
                        DataTable tbClogLocation;
                        string sql = $@"select ID,Description,MDivisionID from ClogLocation WITH (NOLOCK) where MDivisionID = '{mdivisionID.ToString().Trim()}' and junk=0 order by ID ";
                        DBProxy.Current.Select("Production", sql, out tbClogLocation);
                        Win.Tools.SelectItem item = new Win.Tools.SelectItem(tbClogLocation, "ID,Description,MDivisionID", "10,40,10", row["ClogLocationID"].ToString(), "ID,Description,M");
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
                    string oldValue = row["ClogLocationID"].ToString();
                    string newValue = e.FormattedValue.ToString(); // user 編輯當下的value , 此值尚未存入DataRow
                    string sql;

                    sql = $@"select ID from ClogLocation WITH (NOLOCK) where MDivisionID = '{mdivisionID.ToString().Trim()}' and ID = '{newValue}' and junk=0 ";
                    if (!MyUtility.Check.Empty(newValue) && oldValue != newValue)
                    {
                        if (!MyUtility.Check.Seek(sql))
                        {
                            row["ClogLocationID"] = string.Empty;
                            row.EndEdit();
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox($"< ClogLocation : {newValue}> not found.");
                            return;
                        }
                    }
                };
                return ts;
            }
        }
    }
}
