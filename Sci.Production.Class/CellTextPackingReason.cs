using Ict.Win;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Ict;
using Sci.Win.Tools;

namespace Sci.Production.Class
{
    /// <summary>
    /// CellTextPackingReasonList
    /// </summary>
    public class CellTextPackingReason : DataGridViewGeneratorTextColumnSettings
    {
        /// <summary>
        /// GetGridCell
        /// </summary>
        /// <param name="reasonType">Type</param>
        /// <returns>DataGridViewGeneratorTextColumnSettings</returns>
        public static DataGridViewGeneratorTextColumnSettings GetGridCell(string reasonType)
        {
            CellTextPackingReason ts = new CellTextPackingReason();

            // Factory右鍵彈出功能
            ts.EditingMouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    DataGridView grid = ((DataGridViewColumn)s).DataGridView;
                    int colIndex = grid.CurrentCell.ColumnIndex;
                    string currentcolName = grid.Columns[colIndex].Name;

                    // Parent form 若是非編輯狀態就 return
                    if (!((Win.Forms.Base)grid.FindForm()).EditMode)
                    {
                        return;
                    }

                    DataRow row = grid.GetDataRow<DataRow>(e.RowIndex);
                    string colName = string.Empty;
                    switch (reasonType)
                    {
                        case "EG":
                            colName = "PackingReasonIDForTypeEG";
                            break;
                        case "EO":
                            colName = "PackingReasonIDForTypeEO";
                            break;
                        case "ET":
                            colName = "PackingReasonIDForTypeET";
                            break;
                        default:
                            break;
                    }

                    #region SQL CMD
                    string sqlcmd = $@"
select ID,Description 
from PackingReason 
where Junk=0 
and Type='{reasonType}'
";
                    #endregion

                    SelectItem sele = new SelectItem(sqlcmd, "10,40", row[currentcolName].ToString(), false, ",");
                    DialogResult result = sele.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    row[currentcolName] = sele.GetSelecteds()[0]["ID"].ToString() + "-" + sele.GetSelecteds()[0]["Description"].ToString();
                    row[colName] = sele.GetSelectedString();
                    row.EndEdit();
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

                int colIndex = grid.CurrentCell.ColumnIndex;
                string currentcolName = grid.Columns[colIndex].Name;
                string colName = string.Empty;
                switch (reasonType)
                {
                    case "EG":
                        colName = "PackingReasonIDForTypeEG";
                        break;
                    case "EO":
                        colName = "PackingReasonIDForTypeEO";
                        break;
                    case "ET":
                        colName = "PackingReasonIDForTypeET";
                        break;
                    default:
                        break;
                }

                DataRow row = grid.GetDataRow<DataRow>(e.RowIndex);
                string oldValue = row[colName].ToString();
                if (MyUtility.Check.Empty(e.FormattedValue.ToString()))
                {
                    return;
                }

                string[] arry_NewValue = e.FormattedValue.ToString().Split('-'); // user 編輯當下的value , 此值尚未存入DataRow

                if (!MyUtility.Check.Empty(arry_NewValue[0]) && oldValue != arry_NewValue[0])
                {
                    #region SQL CMD
                    string sqlcmd = $@"
select ID
       , Description = rtrim(Description)
from PackingReason WITH (NOLOCK) 
where Type = '{reasonType}' 
and id ='{arry_NewValue[0]}'
and Junk = 0
";
                    #endregion

                    if (!MyUtility.Check.Seek(sqlcmd, out DataRow drSql))
                    {
                        row[colName] = string.Empty;
                        row.EndEdit();
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox(string.Format("< Return TO : {0} > not found!!!", arry_NewValue[0]));
                        return;
                    }
                    else
                    {
                        row[colName] = drSql["ID"];
                        row[currentcolName] = drSql["ID"] + "-" + drSql["Description"];
                        row.EndEdit();
                    }
                }
            };
            return ts;
        }
    }
}
