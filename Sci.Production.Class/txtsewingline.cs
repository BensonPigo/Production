using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Win.UI;

namespace Sci.Production.Class
{
    /// <summary>
    /// Txtsewingline
    /// </summary>
    public partial class Txtsewingline : Win.UI.TextBox
    {
        private string fty = string.Empty;

        /// <summary>
        /// SewingLine.FactoryID
        /// </summary>
        [Category("Custom Properties")]
        public Control FactoryobjectName { get; set; } = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="Txtsewingline"/> class.
        /// </summary>
        public Txtsewingline()
        {
            this.Width = 60;
        }

        /// <inheritdoc/>
        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            base.OnPopUp(e);

            if (this.FactoryobjectName == null || MyUtility.Check.Empty(this.FactoryobjectName.Text))
            {
                this.fty = string.Empty;
            }
            else
            {
                this.fty = this.FactoryobjectName.Text;
            }

            string ftyWhere = string.Empty;
            if (!this.fty.Empty())
            {
                ftyWhere = string.Format("Where FactoryId = '{0}'", this.fty);
            }

            string sql = string.Format("Select ID,FactoryID,Description From Production.dbo.SewingLine WITH (NOLOCK) {0} ", ftyWhere);
            Win.Tools.SelectItem item = new Win.Tools.SelectItem(sql, "2,6,16", this.Text, false, ",");
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.Text = item.GetSelectedString();
        }

        /// <inheritdoc/>
        protected override void OnValidating(CancelEventArgs e)
        {
            base.OnValidating(e);
            string str = this.Text;
            if (!string.IsNullOrWhiteSpace(str) && str != this.OldValue)
            {
                if (this.FactoryobjectName == null || MyUtility.Check.Empty(this.FactoryobjectName.Text))
                {
                    string tmp = MyUtility.GetValue.Lookup("ID", str, "SewingLine", "id", "Production");
                    if (string.IsNullOrWhiteSpace(tmp))
                    {
                        e.Cancel = true;
                        this.Text = string.Empty;
                        MyUtility.Msg.WarningBox(string.Format("< Sewing Line> : {0} not found!!!", str));
                        return;
                    }
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace((string)this.FactoryobjectName.Text))
                    {
                        string selectCommand = string.Format("select ID from Production.dbo.SewingLine WITH (NOLOCK) where FactoryID = '{0}' and ID = '{1}'", (string)this.FactoryobjectName.Text, this.Text.ToString());
                        if (!MyUtility.Check.Seek(selectCommand, null))
                        {
                            e.Cancel = true;
                            this.Text = string.Empty;
                            MyUtility.Msg.WarningBox(string.Format("< Sewing Line: {0} > not found!!!", str));
                            return;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// CellCellSewingLine
        /// </summary>
        public class CellCellSewingLine : DataGridViewGeneratorTextColumnSettings
        {
            /// <inheritdoc/>
            public static DataGridViewGeneratorTextColumnSettings GetGridCell(string factory)
            {
                // pur 為ture 表示需判斷PurchaseFrom
                CellCellSewingLine ts = new CellCellSewingLine();
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
                        string sql = $@"select ID,Description,MDivisionID from ClogLocation WITH (NOLOCK) where MDivisionID = '{factory.ToString().Trim()}' and junk=0 order by ID ";
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

                    sql = $@"select ID from ClogLocation WITH (NOLOCK) where MDivisionID = '{factory.ToString().Trim()}' and ID = '{newValue}' and junk=0 ";
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