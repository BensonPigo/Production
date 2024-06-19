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
            if (dataTable == null || columnsToCheck == null || columnsToCheck.Count == 0)
            {
                dt = null;
                return false; // 如果 DataTable 或欄位清單為空，直接返回 false, 正常要給,不然呼叫幹啥
            }

            var duplicateKeys = dataTable.AsEnumerable()
                .GroupBy(row => string.Join("|", columnsToCheck.Select(col => row[col].ToString())))
                .Where(group => group.Count() > 1)
                .Select(group => group.Key)
                .ToList();

            dt = duplicateKeys.ToDataTable();
            return duplicateKeys.Count == 0;
        }
    }
}