using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Data;
using System.Windows.Forms;

namespace Sci.Production.Planning
{
    /// <inheritdoc/>
    public partial class P09 : Win.Tems.QueryForm
    {
        /// <summary>
        /// 展開: By SewingLine, Sewing Date, SP, Factory(已是必輸入條件)
        /// </summary>
        /// <inheritdoc/>
        public P09(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.GridSetup();
        }

        private void GridSetup()
        {
            this.grid1.DataSource = this.grid1bs;
            this.Helper.Controls.Grid.Generator(this.grid1)
                .Text("SewingLineID", header: "SewingLine", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Date("SewingDate")
                ;
        }

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            if (!this.QueryBefore())
            {
                return;
            }

            #region where
            string sqlDECLARE = string.Empty;
            string where = string.Empty;

            // 特殊條件 Sewing Date Range 找出 SewingSchedule.Inline ~ SewingSchedule.Offline 有交集
            if (this.dateSewing.HasValue1)
            {
                sqlDECLARE = $@"
DECLARE @SewingDate1 date = '{this.dateSewing.Text1}' 
DECLARE @SewingDate2 date = '{this.dateSewing.Text2}'
";
                where += $@"
AND (
    -- 狀況 1: @SewingDate1 在 Inline 和 Offline 之間
    (@SewingDate1 BETWEEN ss.Inline AND ss.Offline)
    -- 狀況 2: @SewingDate2 在 Inline 和 Offline 之間
    OR (@SewingDate2 BETWEEN ss.Inline AND ss.Offline)
    -- 狀況 3: Inline 在 @SewingDate1 和 @SewingDate2 之間
    OR (ss.Inline BETWEEN @SewingDate1 AND @SewingDate2)
    -- 狀況 4: Offline 在 @SewingDate1 和 @SewingDate2 之間
    OR (ss.Offline BETWEEN @SewingDate1 AND @SewingDate2)
)";
            }

            if (this.dateSewingInline.HasValue1)
            {
                where += $@"
AND ss.Inline BETWEEN '{this.dateSewingInline.Text1}' AND '{this.dateSewingInline.Text2}'";
            }

            if (this.dateSewingOffline.HasValue1)
            {
                where += $@"
AND ss.Offline BETWEEN '{this.dateSewingOffline.Text1}' AND '{this.dateSewingOffline.Text2}'";
            }

            #endregion

            // 展開: By SewingLine, Sewing Date, SP, Factory(已是必輸入條件)
            string sqlcmd = $@"
{sqlDECLARE}
SELECT
    ss.SewingLineID
   ,ss.OrderID
   ,ss.FactoryID
   ,ss.APSNo
   ,Inline = CAST(ss.Inline AS DATE)
   ,Offline = CAST(ss.Offline AS DATE)
FROM SewingSchedule ss WITH (NOLOCK)
WHERE ss.MDivisionID = '{this.txtMdivision1.Text}'
AND ss.FactoryID = '{this.txtfactory1.Text}'
{where}

-- 將 Inline ~ Offline 列出每天日期展開, 使用遞增數字生成器展開日期範圍
;WITH DateRange AS (
    SELECT Increment = ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) - 1 FROM master.dbo.spt_values
),ExpandedDates AS (
    SELECT
        ss.SewingLineID
       ,ss.OrderID
       ,ss.FactoryID
       ,SewingDate = DATEADD(DAY, dr.Increment, ss.Inline)
    FROM #tmpSewingSchedule ss
    INNER JOIN DateRange dr ON DATEADD(DAY, dr.Increment, ss.Inline) <= ss.Offline --Inline 遞增到與 Offline 同一天
)
SELECT DISTINCT
    SewingLineID
   ,OrderID
   ,FactoryID
   ,SewingDate
INTO #tmpPkeyColumns
FROM ExpandedDates

--by Pkey 準備每日標準數(總和)--很慢
SELECT
    ss.SewingLineID
   ,ss.OrderID
   ,ss.FactoryID
   ,std.Date
   ,StdQty = SUM(std.StdQ)
INTO #tmpSumDailyStdQty
FROM #tmpSewingSchedule ss
OUTER APPLY(SELECT * FROM dbo.getDailystdq (ss.APSNo) std) std
GROUP BY
    ss.SewingLineID
   ,ss.OrderID
   ,ss.FactoryID
   ,std.Date

SELECT
    ss.SewingLineID
   ,ss.OrderID
   ,ss.FactoryID
   ,ss.SewingDate
   ,o.Qty
   ,StdQty = ISNULL(std.StdQty, 0)
   --Cutting Output
   ,sdo.CuttingRemark
FROM #tmpPkeyColumns ss
INNER JOIN Orders o WITH (NOLOCK) ON o.ID = ss.OrderID
LEFT JOIN #tmpSumDailyStdQty std ON std.SewingLineID = ss.SewingLineID AND std.OrderID = ss.OrderID AND std.FactoryID = ss.FactoryID AND std.Date = ss.SewingDate
LEFT JOIN SewingDailyOutputStatusRecord sdo ON sdo.SewingLineID = ss.SewingLineID AND sdo.OrderID = ss.OrderID AND sdo.FactoryID = ss.FactoryID AND sdo.SewingOutputDate = ss.SewingDate
ORDER BY 
    ss.SewingLineID
   ,ss.OrderID
   ,ss.FactoryID

DROP TABLE #tmpSewingSchedule
    ,#tmpPkeyColumns
    ,#tmpSumDailyStdQty
";
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out DataTable dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }
        }

        private bool QueryBefore()
        {
            // MDivision, Factory 不可空
            if (MyUtility.Check.Empty(this.txtMdivision1.Text) || MyUtility.Check.Empty(this.txtfactory1.Text))
            {
                MyUtility.Msg.WarningBox("MDivision and Factory can not be empty!");
                return false;
            }

            // 3 個日期條件至少輸入一個
            if (!this.dateSewing.HasValue1 && !this.dateSewingInline.HasValue1 && !this.dateSewingOffline.HasValue1)
            {
                MyUtility.Msg.WarningBox("Date can not all be empty!");
                return false;
            }

            return true;
        }

        private void BtnDownloadTemplate_Click(object sender, EventArgs e)
        {

        }
    }
}
