using Ict.Win;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Ict;
using Sci.Win.Tools;

namespace Sci.Production.Class
{
    /// <summary>
    /// CellTextDropDownList
    /// </summary>
    public class CellTextDropDownList : DataGridViewGeneratorTextColumnSettings
    {
        /// <summary>
        /// GetGridCell
        /// </summary>
        /// <param name="ctype">Type</param>
        /// <returns>DataGridViewGeneratorTextColumnSettings</returns>
        public static DataGridViewGeneratorTextColumnSettings GetGridCell(string ctype)
        {
            CellTextDropDownList ts = new CellTextDropDownList();

            // Factory右鍵彈出功能
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

                    DataRow row = grid.GetDataRow<DataRow>(e.RowIndex);

                    string shiftcolname = grid.Columns.Cast<DataGridViewColumn>().Where(o => o.Name.Contains("Shift")).FirstOrDefault().Name;
                    string newShiftcolname = grid.Columns.Cast<DataGridViewColumn>().Where(o => o.Name.Contains("NewShift")).FirstOrDefault() == null ?
                                            string.Empty : grid.Columns.Cast<DataGridViewColumn>().Where(o => o.Name.Contains("NewShift")).FirstOrDefault().Name;

                    string colName = string.IsNullOrEmpty(newShiftcolname) ? shiftcolname : newShiftcolname;

                    #region SQL CMD
                    string sqlcmd = $@"
select ID
       , Name = rtrim(Name)
from DropDownList WITH (NOLOCK) 
where Type = '{ctype}' 
order by Seq";
                    #endregion

                    SelectItem sele = new SelectItem(sqlcmd, "10,40", row[colName].ToString(), false, ",");
                    DialogResult result = sele.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    e.EditingControl.Text = sele.GetSelectedString();
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

                DataRow row = grid.GetDataRow<DataRow>(e.RowIndex);

                // Cutting P03 使用時，預設值是NewShift，但其他地方沒有NewShift這個欄位名稱，因此動態抓不能寫死
                string shiftcolname1 = grid.Columns.Cast<DataGridViewColumn>().Where(o => o.Name.Contains("Shift")).FirstOrDefault().Name;
                string newShiftcolname1 = grid.Columns.Cast<DataGridViewColumn>().Where(o => o.Name.Contains("NewShift")).FirstOrDefault() == null ?
                                            string.Empty : grid.Columns.Cast<DataGridViewColumn>().Where(o => o.Name.Contains("NewShift")).FirstOrDefault().Name;

                string colName = string.IsNullOrEmpty(newShiftcolname1) ? shiftcolname1 : newShiftcolname1;

                string oldValue = row[colName].ToString();
                string newValue = e.FormattedValue.ToString(); // user 編輯當下的value , 此值尚未存入DataRow

                if (!MyUtility.Check.Empty(newValue) && oldValue != newValue)
                {
                    #region SQL CMD
                    string sqlcmd = $@"
select ID
       , Name = rtrim(Name)
from DropDownList WITH (NOLOCK) 
where Type = '{ctype}' 
and id ='{newValue}'
order by Seq";
                    #endregion

                    if (!MyUtility.Check.Seek(sqlcmd))
                    {
                        row[colName] = string.Empty;
                        row.EndEdit();
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox(string.Format("< Return TO : {0} > not found!!!", newValue));
                        return;
                    }
                }
            };
            return ts;
        }
    }
}
