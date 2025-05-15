using Ict;
using Sci.Data;
using Sci.Production.Prg;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace Sci.Production.PublicPrg
{
    /// <summary>
    /// Prgs
    /// </summary>
    public static partial class Prgs
    {
        /// <summary>
        /// 檢查 DataTable 重複 Pkey
        /// </summary>
        /// <param name="dataTable">dataTable</param>
        /// <param name="columnsToCheck">檢查欄位組合</param>
        /// <param name="dt">吐回DataTable</param>
        /// <returns>bool</returns>
        public static bool CheckForDuplicateKeys(DataTable dataTable, List<string> columnsToCheck, out DataTable dt)
        {
            dt = dataTable.Clone(); // 先複製結構

            if (dataTable == null || columnsToCheck == null || columnsToCheck.Count == 0)
            {
                dt = null;
                return false; // 如果 DataTable 或欄位清單為空，直接返回 false, 正常要給,不然呼叫幹啥
            }

            var duplicateRows = dataTable.AsEnumerable()
                .Where(row => row.RowState != DataRowState.Deleted)
                .GroupBy(row => string.Join("|", columnsToCheck.Select(col => row[col].ToString())))
                .Where(group => group.Count() > 1)
                .SelectMany(group => group) // 選取所有重複的行
                .ToList();

            if (duplicateRows.Any())
            {
                foreach (var row in duplicateRows)
                {
                    dt.ImportRow(row); // 將重複的Row加入
                }
            }

            return duplicateRows.Count == 0;
        }
    }
}