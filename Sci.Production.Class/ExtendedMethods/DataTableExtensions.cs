using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Sci.Production.Class.ExtendedMethods
{
    public static class DataTableExtensions
    {
        public static DataTable CopyToDataTableSafe(this IEnumerable<DataRow> source, DataTable template)
        {
            if (source == null || !source.Any())
            {
                return template.Clone(); // 沒資料時，回傳一個空的 DataTable，欄位結構一樣
            }

            return source.CopyToDataTable();
        }
    }
}
