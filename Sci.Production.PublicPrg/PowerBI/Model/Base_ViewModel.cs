using Ict;
using System;
using System.Collections.Generic;
using System.Data;

namespace Sci.Production.Prg.PowerBI.Model
{
    /// <inheritdoc/>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "<暫止>")]
    public class ExecutedList
    {
        /// <summary>
        /// 程式名稱
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 條件 起始日期
        /// </summary>
        public DateTime? SDate { get; set; }

        /// <summary>
        /// 條件 結束日期
        /// </summary>
        public DateTime? EDate { get; set; }

        /// <summary>
        /// 條件 起始日期
        /// </summary>
        public DateTime? SDate2 { get; set; }

        /// <summary>
        /// 條件 結束日期
        /// </summary>
        public DateTime? EDate2 { get; set; }

        /// <summary>
        /// TSQL Store ProcedureName
        /// </summary>
        public string ProcedureName { get; set; }

        /// <summary>
        /// DB Name
        /// </summary>
        public string DBName { get; set; }

        /// <summary>
        /// TSQL
        /// </summary>
        public string TSQL { get; set; }

        /// <summary>
        /// 執行成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 執行時間 起
        /// </summary>
        public DateTime? ExecuteSDate { get; set; }

        /// <summary>
        /// 執行時間 訖
        /// </summary>
        public DateTime? ExecuteEDate { get; set; }

        /// <summary>
        /// 錯誤訊息
        /// </summary>
        public string ErrorMsg { get; set; }

        /// <summary>
        /// 備註
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 只在假日執行
        /// </summary>
        public bool RunOnSunday { get; set; }

        /// <summary>
        /// 群組
        /// </summary>
        public int Group { get; set; }

        /// <summary>
        /// 順序
        /// </summary>
        public int SEQ { get; set; }

        /// <summary>
        /// 執行程式來源
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// BITableInfo.TransferDate
        /// </summary>
        public DateTime? TransferDate { get; set; }

        /// <summary>
        /// 執行清單
        /// </summary>
        public List<ExecutedList> ExecutedLists { get; set; }
    }

    /// <inheritdoc/>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "<暫止>")]
    public class Base_ViewModel
    {
        /// <inheritdoc/>
        public DualResult Result { get; set; }

        /// <inheritdoc/>
        public DataTable Dt { get; set; }

        /// <inheritdoc/>
        public DataTable[] DtArr { get; set; }

        /// <inheritdoc/>
        public int IntValue { get; set; }
    }
}
