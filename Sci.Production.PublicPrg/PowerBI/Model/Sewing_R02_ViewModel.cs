using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Data;

namespace Sci.Production.Prg.PowerBI.Model
{
    /// <inheritdoc/>
    public class Sewing_R02_ViewModel
    {
        /// <inheritdoc/>
        public DateTime? StartOutputDate { get; set; }

        /// <inheritdoc/>
        public DateTime? EndOutputDate { get; set; }

        /// <inheritdoc/>
        public string Factory { get; set; }

        /// <inheritdoc/>
        public string M { get; set; }

        /// <inheritdoc/>
        public int ReportType { get; set; }

        /// <inheritdoc/>
        public string StartSewingLine { get; set; }

        /// <inheritdoc/>
        public string EndSewingLine { get; set; }

        /// <inheritdoc/>
        public int OrderBy { get; set; }

        /// <inheritdoc/>
        public bool ExcludeNonRevenue { get; set; }

        /// <inheritdoc/>
        public bool ExcludeSampleFactory { get; set; }

        /// <inheritdoc/>
        public bool ExcludeOfMockUp { get; set; }

        /// <inheritdoc/>
        public bool IsCN { get; set; }

        /// <inheritdoc/>
        public DateTime StartDate { get; set; }

        /// <inheritdoc/>
        public DateTime EndDate { get; set; }

        /// <inheritdoc/>
        public bool IsContainShare { get; set; }

        /// <inheritdoc/>
        public bool IsLocal { get; set; }
    }

    /// <inheritdoc/>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Reviewed")]
    public class AttendanceSummary_APICondition
    {
        /// <summary>
        /// 工廠
        /// </summary>
        public string FactoryID;

        /// <summary>
        /// 開始日期
        /// </summary>
        public string StartDate;

        /// <summary>
        /// 結束日期
        /// </summary>
        public string EndDate;

        /// <summary>
        /// 是否要產Sharing的資料
        /// </summary>
        public bool IsContainShare = false;

        /// <summary>
        /// 是否只有本地
        /// CN地區沒有Local條件，請傳false
        /// </summary>
        public bool IsLocal = false;
    }

    /// <inheritdoc/>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Reviewed")]
    public class AttendanceSummaryResult
    {
        /// <summary>
        /// 工廠
        /// </summary>
        public string FactoryID;

        /// <summary>
        /// 查詢的結果
        /// </summary>
        public DataSet QueryResult;

        /// <summary>
        /// 失敗訊息
        /// </summary>
        public string Exception = string.Empty;

        /// <summary>
        /// 失敗訊息，但會執行完畢
        /// </summary>
        public string ExceptionKeepRun = string.Empty;
    }

    /// <inheritdoc/>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Reviewed")]
    public class P_MonthlySewingOutputSummary_ViewModel
    {
        /// <inheritdoc/>
        public string Fty { get; set; }

        /// <inheritdoc/>
        public string Period { get; set; }

        /// <inheritdoc/>
        public string LastDatePerMonth { get; set; }

        /// <inheritdoc/>
        public int TtlQtyExclSubconOut { get; set; }

        /// <inheritdoc/>
        public decimal TtlCPUInclSubconIn { get; set; }

        /// <inheritdoc/>
        public decimal SubconInTtlCPU { get; set; }

        /// <inheritdoc/>
        public decimal SubconOutTtlCPU { get; set; }

        /// <inheritdoc/>
        public decimal PPH { get; set; }

        /// <inheritdoc/>
        public decimal AvgWorkHr { get; set; }

        /// <inheritdoc/>
        public int TtlManpower { get; set; }

        /// <inheritdoc/>
        public decimal TtlManhours { get; set; }

        /// <inheritdoc/>
        public decimal Eff { get; set; }

        /// <inheritdoc/>
        public decimal AvgWorkHrPAMS { get; set; }

        /// <inheritdoc/>
        public decimal TtlManpowerPAMS { get; set; }

        /// <inheritdoc/>
        public decimal TtlManhoursPAMS { get; set; }

        /// <inheritdoc/>
        public decimal EffPAMS { get; set; }

        /// <inheritdoc/>
        public int TransferManpowerPAMS { get; set; }

        /// <inheritdoc/>
        public decimal TransferManhoursPAMS { get; set; }

        /// <inheritdoc/>
        public decimal TtlRevenue { get; set; }

        /// <inheritdoc/>
        public int TtlWorkDay { get; set; }

        /// <inheritdoc/>
        public List<P_MonthlySewingOutputSummary_ViewModel> DataList;
    }

    /// <summary>
    /// BI table P_CMPByDate
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Reviewed")]
    public class P_CMPByDate
    {
        /// <summary>
        /// factory id
        /// </summary>
        public string FactoryID { get; set; }

        /// <summary>
        /// output date
        /// </summary>
        public DateTime OutputDate { get; set; }

        /// <summary>
        /// gph cpu
        /// </summary>
        public decimal GPHCPU { get; set; }

        /// <summary>
        /// sph cpu
        /// </summary>
        public decimal SPHCPU { get; set; }

        /// <summary>
        /// vph cpu
        /// </summary>
        public decimal VPHCPU { get; set; }

        /// <summary>
        /// gph manhours
        /// </summary>
        public decimal GPHManhours { get; set; }

        /// <summary>
        /// sph manhours
        /// </summary>
        public decimal SPHManhours { get; set; }

        /// <summary>
        /// vph manhours
        /// </summary>
        public decimal VPHManhours { get; set; }

        /// <summary>
        /// Garment per hour
        /// </summary>
        public decimal GPH
        {
            get
            {
                return this.GPHManhours == 0 ? 0 : this.GPHCPU / this.GPHManhours;
            }
        }

        /// <summary>
        /// Subprocess per hour
        /// </summary>
        public decimal SPH
        {
            get
            {
                return this.SPHManhours == 0 ? 0 : this.SPHCPU / this.SPHManhours;
            }
        }

        /// <summary>
        /// Value per hour
        /// </summary>
        public decimal VPH
        {
            get
            {
                return this.VPHManhours == 0 ? 0 : this.VPHCPU / this.VPHManhours;
            }
        }

        /// <summary>
        /// 工時比率
        /// </summary>
        public decimal ManhoursRatio { get; set; }

        /// <summary>
        /// 總人力
        /// </summary>
        public decimal TotalActiveHeadcount { get; set; }

        /// <summary>
        /// 收入部門人頭數
        /// </summary>
        public decimal RevenumDeptHeadcount { get; set; }

        /// <summary>
        /// 人力比率
        /// </summary>
        public decimal ManpowerRatio { get; set; }
    }
}
