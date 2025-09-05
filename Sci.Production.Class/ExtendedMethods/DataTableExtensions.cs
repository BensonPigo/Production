using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

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

        /// <summary>
        /// 將LINQ查詢結果轉換為DataTable
        /// </summary>
        public static DataTable LinqToDataTable<T>(this IEnumerable<T> linqQuery)
        {
            if (linqQuery.Count() == 0)
            {
                return new DataTable();
            }

            DataTable table = new DataTable();
            PropertyInfo[] properties = linqQuery.First().GetType().GetProperties();
            foreach (var property in properties)
            {
                if (property.PropertyType.IsGenericType &&
                                  property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    table.Columns.Add(property.Name, property.PropertyType.GetGenericArguments()[0]);
                }
                else
                {
                    table.Columns.Add(property.Name, property.PropertyType);
                }
            }

            foreach (var item in linqQuery)
            {
                DataRow newRow = table.NewRow();
                foreach (var property in properties)
                {
                    newRow[property.Name] = property.GetValue(item) == null ? DBNull.Value : property.GetValue(item);
                }

                table.Rows.Add(newRow);
            }

            return table;
        }
    }
}
