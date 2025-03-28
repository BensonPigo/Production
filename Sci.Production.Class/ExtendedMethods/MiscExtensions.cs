using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Class.Command
{
    public static class MiscExtensions
    {
        /// <summary>
        /// 抓取資料庫的時間
        /// </summary>
        /// <inheritdoc/>
        public static DateTime? GetDBTime(this IDBProxy proxy)
        {
            return proxy.LookupEx<DateTime>("Select GetDate();").ExtendedData;
        }

        /// <inheritdoc/>
        public static T FindForm<T>(this Control ctl)
            where T : Form
        {
            return (T)ctl.FindForm();
        }

        /// <summary>
        /// 嘗試呼叫每一個(通常也只有一個)DataBind物件進行ReadValue動作
        /// </summary>
        /// <inheritdoc/>
        public static void TryBindingRead(this Control ctl)
        {
            ctl.DataBindings.Cast<Binding>().ToList().ForEach(bd => bd.ReadValue());
        }

        /// <summary>
        /// 嘗試呼叫每一個(通常也只有一個)DataBind物件進行WriteValue動作
        /// </summary>
        /// <inheritdoc/>
        public static void TryBindingWrite(this Control ctl)
        {
            ctl.DataBindings.Cast<Binding>().ToList().ForEach(bd => bd.WriteValue());
        }

        /// <summary>
        /// 比較目標文字，可以自己決定要不要IgnorCase
        /// </summary>
        /// <inheritdoc/>
        public static int CompareTo(this string source, string strB, bool ignorCase)
        {
            return string.Compare(source, strB, ignorCase);
        }

        /// <summary>
        /// 轉呼叫string.IsNullOrWhiteSpace(source)
        /// </summary>
        /// <inheritdoc/>
        public static bool IsNullOrWhiteSpace(this string source)
        {
            return string.IsNullOrWhiteSpace(source);
        }

        /// <summary>
        /// 依指定數量切割datatable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="chunkSize"></param>
        /// <returns></returns>
        public static IEnumerable<IEnumerable<T>> ToChunks<T>(this IEnumerable<T> enumerable,
                                               int chunkSize)
        {
            int itemsReturned = 0;
            var list = enumerable.ToList(); // Prevent multiple execution of IEnumerable.
            int count = list.Count;
            while (itemsReturned < count)
            {
                int currentChunkSize = Math.Min(chunkSize, count - itemsReturned);
                yield return list.GetRange(itemsReturned, currentChunkSize);
                itemsReturned += currentChunkSize;
            }
        }

        /// <summary>
        /// 當依照指定的方式做檢查，如果檢查結果是有資料，就把按鈕變藍色且粗體字，否則為正常黑色字體
        /// </summary>
        /// <param name="btn">btn</param>
        /// <param name="checker">用於檢查有沒有資料的方法</param>
        /// <inheritdoc/>
        public static void ChangeStyleByChecker(this Ict.Win.UI.Button btn, Func<bool> checker = null)
        {
            if (checker == null || checker())
            {
                btn.Font = new Font(btn.Font, FontStyle.Bold);
                btn.ForeColor = System.Drawing.Color.FromArgb((int)((byte)0), (int)((byte)0), (int)((byte)255));
            }
            else
            {
                btn.Font = new Font(btn.Font, FontStyle.Regular);
                btn.ForeColor = System.Drawing.Color.Black;
            }
        }

        /// <summary>
        /// 把整列變成鵝黃色(Pending Remark Default color)
        /// </summary>
        /// <inheritdoc/>
        public static void ChangePendingRemarkStyle(this DataGridViewRow gridRow)
        {
            gridRow.Cells.SetStyleBackColor(VFPColor.Yellow_255_255_128);
        }

        /// <summary>
        /// 把GridView裡面一整列的BackColor設定為新值，不給Color或是給null表示清空回預設
        /// </summary>
        /// <inheritdoc/>
        public static void SetStyleBackColor(this DataGridViewCellCollection cells, Color? color = null)
        {
            cells.Cast<DataGridViewCell>().ToList().ForEach(cell => cell.Style.BackColor = color.HasValue ? color.Value : Color.Empty);
        }

        /// <summary>
        /// 把GridView裡面一整列的ForeColor設定為新值，不給Color或是給null表示清空回預設
        /// </summary>
        /// <inheritdoc/>
        public static void SetStyleForeColor(this DataGridViewCellCollection cells, Color? color = null)
        {
            cells.Cast<DataGridViewCell>().ToList().ForEach(cell => cell.Style.ForeColor = color.HasValue ? color.Value : Color.Empty);
        }

        /// <summary>
        /// 根據傳入的檢查方法來決定是否要把整列變成鵝黃色(Pending Remark Default color)
        /// </summary>
        /// <inheritdoc/>
        public static void ChangePendingRemarkStyleByChecker(this DataGridViewRow gridRow, Func<bool> checker)
        {
            if (checker != null && checker())
            {
                gridRow.Cells.Cast<DataGridViewCell>().ToList().ForEach(cell => cell.Style.BackColor = VFPColor.Yellow_255_255_128);
            }
            else
            {
                gridRow.Cells.Cast<DataGridViewCell>().ToList().ForEach(cell => cell.Style.BackColor = SystemColors.Control);
            }
        }

        /// <summary>
        /// <para>繼承使用Field(T)，先把object轉T1，之後再轉T2，會需要做這個的原因是，隱含轉換子，不允許從object轉為其他任何類別</para>
        /// <para>ex: newRow.Field&lt;string, myClass&gt;("MR")</para>
        /// </summary>
        /// <inheritdoc/>
        public static T2 Field<T1, T2>(this DataRow row, string columnName)
        {
            return (T2)(dynamic)(T1)row[columnName];
        }

        /// <summary>
        /// 將資料表的欄位與值拿來當作Dictionary的Key轉換成一個與資料表脫勾的字典物件供後續使用，初始目的是為了讓資料列裡面的值可以不會因為資料表被dispose而無法取用(multi-threading)
        /// </summary>
        /// <inheritdoc/>
        public static Dictionary<string, object> ToDict(this DataRow row)
        {
            return row.Table.Columns.Cast<DataColumn>().ToDictionary(col => col.ColumnName, col => row.IsNull(col.ColumnName) ? null : row[col.ColumnName]);
        }

        /// <summary>
        /// 有些時候，為了偵錯，要看一筆DataRow的值，結果要馬不知道哪個欄位是哪個值，要馬看完整Table又不知道剛剛這筆關鍵的DataRow在哪邊，所以寫這個方法
        /// </summary>
        /// <inheritdoc/>
        public static DataTable ViewInTable(this DataRow row)
        {
            try
            {
                var dt = row.Table.Clone();
                dt.LoadDataRow(row.ItemArray, true);
                return dt;
            }
            catch
            {
                return null; // 因為是Debug用，如果沒辦法clone就算了(通常是因為Table中有使用到Parent 或是child這種DataRelation的ExpressColumn
            }
        }

        /// <summary>
        /// 當發生String or binary data would be truncated錯誤時候，可用此方法觀察每個欄位的最長值是多少
        /// </summary>
        /// <inheritdoc/>
        public static DataTable FIndingStringTruncatedErrorInfo(this DataTable dt)
        {
            var columns = dt.Columns.Cast<DataColumn>().Select(col => new
            {
                Name = col.ColumnName,
                DataType = col.DataType,
            })
            .ToList();

            var result = new DataTable();
            result.ColumnsStringAdd("ColumnName");
            result.ColumnsStringAdd("DataTypeName");
            result.ColumnsIntAdd("MaxLength");
            columns
                .Select(colItem => new
                {
                    Name = colItem.Name,
                    DataType = colItem.DataType,
                    MaxLength = colItem.DataType == typeof(string) ?
                        dt.AsEnumerable().Select(row => row[colItem.Name].ToString().Length).Max() :
                        (object)DBNull.Value,
                })
                .ToList()
                .ForEach(item =>
                {
                    var newRow = result.NewRow();
                    newRow["ColumnName"] = item.Name;
                    newRow["DataTypeName"] = item.DataType.Name;
                    newRow["MaxLength"] = item.MaxLength;
                    result.Rows.Add(newRow);
                });
            return result;
        }

        /// <summary>
        /// 檢查當前物件，是否存在於檢查物件
        /// </summary>
        /// <inheritdoc/>
        public static bool IsOneOfThe<T>(this T lookupValue, params T[] lookupTargets)
        {
            if (lookupValue is string)
            {
                return IsOneOfThe((string)(dynamic)lookupValue, false, (string[])(dynamic)lookupTargets);
            }
            else
            {
                return lookupTargets.Contains(lookupValue);
            }
        }

        /// <summary>
        /// 檢查當前文字，是否存在於檢查目標(區分大小寫比較)
        /// </summary>
        /// <inheritdoc/>
        public static bool IsOneOfThe(this string lookupValue, params string[] lookupTargets)
        {
            return IsOneOfThe(lookupValue, false, lookupTargets);
        }

        /// <summary>
        /// 檢查當前文字，是否存在於檢查目標(指定是否區分大小寫來比較)
        /// </summary>
        /// <inheritdoc/>
        public static bool IsOneOfThe(this string lookupValue, bool ignoreCase, params string[] lookupTargets)
        {
            if (lookupValue == null)
            {
                return false;
            }
            else if (lookupTargets == null)
            {
                return false;
            }
            else if (lookupTargets.Any() == false)
            {
                return false;
            }
            else
            {
                return lookupTargets.Any(item => string.Compare(item, lookupValue, ignoreCase) == 0);
            }
        }

        /// <summary>
        /// 是否全部的項目都有被包含在來源清單內
        /// </summary>
        /// <inheritdoc/>
        public static bool ContainsAll<T>(this IEnumerable<T> source, params T[] itemToCheck)
        {
            return itemToCheck.All(item => source.Contains(item));
        }

        /// <summary>
        /// 將控制項的Text屬性回傳作為SqlParameter.SqlValue使用，如果是空字串或是null，就用newValue代替回傳，如果newValue是null就用DBNull.Value代替回傳
        /// </summary>
        /// <inheritdoc/>
        public static object GetDBParameterValue(this System.Windows.Forms.TextBoxBase txt, object newValue = null, bool trimBeforeCompare = true)
        {
            if (txt == null)
            {
                return newValue ?? DBNull.Value;
            }

            if (trimBeforeCompare)
            {
                if (string.IsNullOrWhiteSpace(txt.Text))
                {
                    return newValue ?? DBNull.Value;
                }
                else
                {
                    return txt.Text.Trim();
                }
            }
            else
            {
                if (string.IsNullOrEmpty(txt.Text))
                {
                    return newValue ?? DBNull.Value;
                }
                else
                {
                    return txt.Text.Trim();
                }
            }
        }

        /// <summary>
        /// 將控制項的Text屬性回傳作為SqlParameter.SqlValue使用，如果是空字串或是null，就用newValue代替回傳，如果newValue是null就用DBNull.Value代替回傳
        /// </summary>
        /// <inheritdoc/>
        public static object GetDBParameterValue_Text(this System.Windows.Forms.ComboBox cbx, object newValue = null, bool trimBeforeCompare = true)
        {
            if (cbx == null)
            {
                return newValue ?? DBNull.Value;
            }

            if (trimBeforeCompare)
            {
                if (string.IsNullOrWhiteSpace(cbx.Text))
                {
                    return newValue ?? DBNull.Value;
                }
                else
                {
                    return cbx.Text.Trim();
                }
            }
            else
            {
                if (string.IsNullOrEmpty(cbx.Text))
                {
                    return newValue ?? DBNull.Value;
                }
                else
                {
                    return cbx.Text.Trim();
                }
            }
        }

        /// <summary>
        /// 將控制項的SelectedValue2屬性回傳作為SqlParameter.SqlValue使用，如果是空字串或是null，就用newValue代替回傳，如果newValue是null就用DBNull.Value代替回傳
        /// </summary>
        /// <inheritdoc/>
        public static object GetDBParameterValue_SelectedValue2(this Ict.Win.UI.ComboBox cbx, object newValue = null, bool trimBeforeCompare = true)
        {
            if (cbx == null)
            {
                return newValue ?? DBNull.Value;
            }

            if (cbx.SelectedValue2 == null)
            {
                return newValue ?? DBNull.Value;
            }

            return newValue ?? DBNull.Value;
        }

        /// <summary>
        /// 使用原本的DataTable.AsEnumerable方法，但是加入可以用RowState來篩選的能力，也可以是反向篩選(主要拿來篩選非Delete的DataRow，因為實在是太常用了)
        /// </summary>
        /// <inheritdoc/>
        public static EnumerableRowCollection<DataRow> AsEnumerable(this DataTable source, DataRowState filter, bool reverse = false)
        {
            return source.AsEnumerable()
                .Where(row =>
                {
                    var result = (row.RowState & filter) == row.RowState;
                    if (reverse)
                    {
                        return !result;
                    }
                    else
                    {
                        return result;
                    }
                });
        }

        /// <summary>
        /// 將資料列完整複製，主要是因為在多執行續當中，有機會出現DataTable被主執行續(或其他執行續給Dispose了，而導致別的執行續無法順利取值
        /// </summary>
        /// <inheritdoc/>
        public static DataRow GetDetachCopy(this DataRow row)
        {
            if (row == null)
            {
                return null;
            }
            else
            {
                lock (row.Table)
                {
                    var newRow = row.Table.NewRow();
                    newRow.ItemArray = row.ItemArray;
                    return newRow;
                }
            }
        }

        /// <summary>
        /// Set all column's SortMode to DataGridViewColumnSortMode.NotSortable
        /// </summary>
        /// <inheritdoc/>
        public static DataGridViewColumnCollection DisableSortable(this DataGridViewColumnCollection cols)
        {
            cols.Cast<DataGridViewColumn>().ToList().ForEach(col => col.SortMode = DataGridViewColumnSortMode.NotSortable);
            return cols;
        }

        /// <inheritdoc/>
        public static DataGridViewColumnCollection ReadOnly(this DataGridViewColumnCollection cols)
        {
            cols.Cast<DataGridViewColumn>().ToList().ForEach(col => col.ReadOnly = true);
            return cols;
        }

        /// <summary>
        /// 更新目前Grid內正在編輯的那一格(請記得先ValidateControl)
        /// </summary>
        /// <inheritdoc/>
        public static Ict.Win.UI.DataGridView InvalidCurrentRow(this Ict.Win.UI.DataGridView grid)
        {
            if (grid.CurrentCell == null)
            {
                return grid;
            }

            grid.InvalidateCell(grid.CurrentCell.ColumnIndex, grid.CurrentCell.RowIndex);
            return grid;
        }

        /// <inheritdoc/>
        public static DataRow GetCurrentDataRow(this Ict.Win.UI.DataGridView grid)
        {
            if (grid.SelectedRows.Count == 0)
            {
                return null;
            }
            else
            {
                return grid.GetDataRow(grid.SelectedRows[0].Index);
            }
        }

        /// <summary>
        /// 用指定的分隔符號，透過string.Join方法將各項目串連起來做回傳
        /// </summary>
        /// <inheritdoc/>
        public static string JoinToString(this IEnumerable<string> items, string delimiter)
        {
            return string.Join(delimiter, items.ToArray());
        }

        /// <summary>
        /// 合併DateTime.ToString()的功能
        /// </summary>
        /// <inheritdoc/>
        public static string ToStringEx(this DateTime? date, string format)
        {
            if (date.HasValue)
            {
                return date.Value.ToString(format);
            }
            else
            {
                return null;
            }
        }

        /// <inheritdoc/>
        public static IEnumerable<DataRow> ExtNotDeletedRows(this DataTable table)
        {
            return table
                .AsEnumerable()
                .Where(row => row.RowState != DataRowState.Deleted);
        }

        /// <summary>
        /// 常常用到：還沒有刪除的資料列，逐筆執行任務
        /// </summary>
        /// <inheritdoc/>
        public static void ExtNotDeletedRowsForeach(this DataTable table, Action<DataRow> job)
        {
            ExtNotDeletedRows(table)
                .ToList()
                .ForEach(job);
        }

        /// <summary>
        /// 常常用到：還沒有刪除的資料列，逐筆執行任務(帶有序號)
        /// </summary>
        /// <inheritdoc/>
        public static void ExtNotDeletedRowsForeach(this DataTable table, Action<DataRow, int> job)
        {
            ExtNotDeletedRows(table)
                .Select((row, idx) =>
                {
                    job(row, idx);
                    return true;
                }).ToList();
        }

        /// <summary>
        /// 常常用到：還沒有刪除的資料列，逐筆轉換
        /// </summary>
        /// <inheritdoc/>
        public static IEnumerable<T> ExtNotDeletedRowsSelect<T>(this DataTable table, Func<DataRow, T> job)
        {
            return ExtNotDeletedRows(table)
                .Select(job);
        }

        /// <summary>
        /// 常常用到：還沒有刪除的資料列，逐筆轉換(帶有序號)
        /// </summary>
        /// <inheritdoc/>
        public static IEnumerable<T> ExtNotDeletedRowsSelect<T>(this DataTable table, Func<DataRow, int, T> job)
        {
            return ExtNotDeletedRows(table)
                .Select(job);
        }

        /// <summary>
        /// 常常用到：還沒有刪除的資料列，逐筆轉換
        /// </summary>
        /// <inheritdoc/>
        public static List<T> ExtNotDeletedRowsToList<T>(this DataTable table, Func<DataRow, T> job)
        {
            return ExtNotDeletedRowsSelect(table, job).ToList();
        }

        /// <summary>
        /// 常常用到：還沒有刪除的資料列，逐筆轉換(帶有序號)
        /// </summary>
        /// <inheritdoc/>
        public static List<T> ExtNotDeletedRowsToList<T>(this DataTable table, Func<DataRow, int, T> job)
        {
            return ExtNotDeletedRowsSelect(table, job).ToList();
        }

        /// <summary>
        /// 把所有指定類型的控制項全部一一列出(巢狀尋找
        /// </summary>
        /// <typeparam name="T">指定的控制項類別</typeparam>
        /// <param name="root">最上層控制項容器</param>
        /// <returns>列舉結果</returns>
        public static IEnumerable<T> GetControlsOfType<T>(this Control root)
            where T : Control
        {
            var t = root as T;
            if (t != null)
            {
                yield return t;
            }

            if (root is TabControl)
            {
                foreach (TabPage c in ((TabControl)root).TabPages)
                {
                    foreach (var i in GetControlsOfType<T>(c))
                    {
                        yield return i;
                    }
                }
            }
            else
            {
                foreach (Control c in root.Controls)
                {
                    foreach (var i in GetControlsOfType<T>(c))
                    {
                        yield return i;
                    }
                }
            }
        }

        /// <summary>
        /// 把所有指定類型的控制項全部一一列出(巢狀尋找
        /// </summary>
        /// <typeparam name="T">指定的控制項類別</typeparam>
        /// <param name="root">最上層控制項容器</param>
        /// <returns>列舉結果</returns>
        public static IEnumerable<T> GetControlsOfType<T>(this Form root)
            where T : Control
        {
            return ((Control)root).GetControlsOfType<T>();
        }

        /// <summary>
        /// 取得所有繼承的Class
        /// </summary>
        /// <param name="myType"> My Type </param>
        /// <param name="addMyType"> 是否加入自己Type </param>
        /// <returns></returns>
        public static IEnumerable<Type> GetInheritedClasses(Type myType, bool addMyType = true)
        {
            var lisType = System.Reflection.Assembly.GetAssembly(myType).GetTypes()
                .Where(theType => theType.IsClass && !theType.IsAbstract && theType.IsSubclassOf(myType))
                .ToList();

            if (addMyType)
            {
                lisType.Add(myType);
            }

            return lisType;
        }

        /// <summary>
        /// 如果來源日期是null，則回傳null，否則回傳yyyy-MM-dd
        /// </summary>
        /// <inheritdoc/>
        public static string ToYYYYMMDD(this DateTime? source)
        {
            if (source == null)
            {
                return null;
            }
            else
            {
                return ((DateTime)source).ToString("yyyy-MM-dd");
            }
        }

        /// <summary>
        /// 新增一Row
        /// </summary>
        /// <inheritdoc/>
        public static void RowsAdd(this DataTable dt)
        {
            DataRow row = dt.NewRow();
            dt.Rows.Add(row);
        }

        public static DataTable ToDataTable<T>(this IEnumerable<T> data)
        {
            var properties = typeof(T).GetProperties();
            DataTable table = new DataTable();

            foreach (var prop in properties)
            {
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            foreach (var item in data)
            {
                var values = properties.Select(p => p.GetValue(item, null)).ToArray();
                table.Rows.Add(values);
            }

            return table;
        }
    }
}
