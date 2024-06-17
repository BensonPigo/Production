using Ict.Win;
using System.Data;
using System.Windows.Forms;
using Ict;
using Sci.Win.Tools;
using Sci.Data;
using System.Linq;

namespace Sci.Production.Class
{
    /// <inheritdoc/>
    public class CellTextFabricPanelCode : DataGridViewGeneratorTextColumnSettings
    {
        /// <inheritdoc/>
        public DataGridViewGeneratorTextColumnSettings GetGridCell(string id, int type)
        {
            CellTextFabricPanelCode ts = new CellTextFabricPanelCode();
            ts.EditingMouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    DataGridView grid = ((DataGridViewColumn)s).DataGridView;

                    // Parent form 若是非編輯狀態就 return
                    if (!((Win.Forms.Base)grid.FindForm()).EditMode)
                    {
                        return;
                    }

                    DataTable dt = this.GetData(id);
                    DataRow row = grid.GetDataRow<DataRow>(e.RowIndex);
                    SelectItem sele = new SelectItem(dt, "PatternPanel,FabricPanelCode", "10,10", row["PatternPanel"].ToString(), false, ",");
                    DialogResult result = sele.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    row["PatternPanel"] = sele.GetSelectedString();
                    row["FabricPanelCode"] = sele.GetSelecteds()[0]["FabricPanelCode"].ToString();
                }
            };

            if (type == 0)
            {
                ts.CellValidating += (s, e) =>
                {
                    DataGridView grid = ((DataGridViewColumn)s).DataGridView;

                    // Parent form 若是非編輯狀態就 return
                    if (!((Win.Forms.Base)grid.FindForm()).EditMode)
                    {
                        return;
                    }

                    DataRow row = grid.GetDataRow<DataRow>(e.RowIndex);
                    string oldValue = row["PatternPanel"].ToString();
                    string newValue = e.FormattedValue.ToString();
                    if (!MyUtility.Check.Empty(newValue) && oldValue != newValue)
                    {
                        DataTable dt = this.GetData(id);
                        if (dt.Select($"PatternPanel = '{newValue}'").Length == 0)
                        {
                            MyUtility.Msg.WarningBox($"< PatternPanel : {newValue} > not found!!!");
                            row["PatternPanel"] = string.Empty;
                            e.Cancel = true;
                        }
                        else
                        {
                            row["PatternPanel"] = newValue;
                        }

                        row.EndEdit();
                    }
                };
            }
            else
            {
                ts.CellValidating += (s, e) =>
                {
                    DataGridView grid = ((DataGridViewColumn)s).DataGridView;

                    // Parent form 若是非編輯狀態就 return
                    if (!((Win.Forms.Base)grid.FindForm()).EditMode)
                    {
                        return;
                    }

                    DataRow row = grid.GetDataRow<DataRow>(e.RowIndex);
                    string oldValue = row["FabricPanelCode"].ToString();
                    string newValue = e.FormattedValue.ToString();
                    if (!MyUtility.Check.Empty(newValue) && oldValue != newValue)
                    {
                        DataTable dt = this.GetData(id);
                        if (dt.Select($"FabricPanelCode = '{newValue}'").Length == 0)
                        {
                            MyUtility.Msg.WarningBox($"< FabricPanelCode : {newValue} > not found!!!");
                            row["FabricPanelCode"] = string.Empty;
                            e.Cancel = true;
                        }
                        else
                        {
                            row["FabricPanelCode"] = newValue;
                        }

                        row.EndEdit();
                    }
                };
            }

            return ts;
        }

        private DataTable GetData(string id)
        {
            string sqlcmd = $@"
SELECT
    PatternPanel
   ,FabricPanelCode
FROM Order_FabricCode
WHERE ID = '{id}'
GROUP BY PatternPanel, FabricPanelCode
ORDER BY PatternPanel, FabricPanelCode
";
            DualResult result = DBProxy.Current.Select(string.Empty, sqlcmd, out DataTable dt);
            if (!result)
            {
                MyUtility.Msg.ErrorBox(result.ToString());
                return null;
            }

            return dt;
        }
    }
}
