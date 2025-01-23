using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Data;
using System.Windows.Forms;

namespace Sci.Production.IE
{
    /// <inheritdoc/>
    public partial class P04 : Sci.Win.Tems.QueryForm
    {
        /// <inheritdoc/>
        public P04(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.EditMode = true;
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.Helper.Controls.Grid.Generator(this.gridLineMappingStatus)
            .Text("FtyGroup", header: "Factory", iseditingreadonly: true, width: Widths.AnsiChars(7))
            .Text("SeasonID", header: "Season", iseditingreadonly: true, width: Widths.AnsiChars(7))
            .Text("StyleID", header: "Style#", iseditingreadonly: true, width: Widths.AnsiChars(14))
            .Text("BrandID", header: "Brand", iseditingreadonly: true, width: Widths.AnsiChars(10))
            .Text("CPU", header: "CPU/pc", iseditingreadonly: true, width: Widths.AnsiChars(7))
            .Date("SewInLine", header: "Sewing Inline", iseditingreadonly: true, width: Widths.AnsiChars(11))
            .Text("LineMapping", header: "Line Mapping", iseditingreadonly: true, width: Widths.AnsiChars(7))
            .Text("Version", header: "Version", iseditingreadonly: true, width: Widths.AnsiChars(7))
            .Text("Phase", header: "Phase", iseditingreadonly: true, width: Widths.AnsiChars(7))
            .Text("SewingLineID", header: "Sewing Line", iseditingreadonly: true, width: Widths.AnsiChars(7))
            .Text("Team", header: "Team", iseditingreadonly: true, width: Widths.AnsiChars(5))
            .Numeric("ttlTimeDiff", header: "Total % time diff", decimal_places: 2, integer_places: 8, width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Numeric("LBR", header: "LBR", width: Widths.AnsiChars(5), iseditingreadonly: true)
            .Text("Status", header: "Status", iseditingreadonly: true, width: Widths.AnsiChars(9))
            .Text("AddName", header: "Add Name", iseditingreadonly: true, width: Widths.AnsiChars(23))
            .Text("EditName", header: "Edit Name", iseditingreadonly: true, width: Widths.AnsiChars(23))
            .DateTime("AddDate", header: "Add Date", iseditingreadonly: true)
            .DateTime("EditDate", header: "Edit Date", iseditingreadonly: true)
            ;
        }

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.dateInlineDate.Value1) || MyUtility.Check.Empty(this.dateInlineDate.Value2))
            {
                MyUtility.Msg.WarningBox("Please fill < Sewing Inline >.");
                this.dateInlineDate.Focus();
                return;
            }

            this.Query();
        }

        private void Query()
        {
            /*
            說明:
            1.依據欄位 FtyGroup, Style, Season, Brand, Sewing Line, Team, Line mapping (Line mapping是P03/P05/P06)
              取最新 Phase 最大 version
            2.最新 Phase 優先度: Final > Prelim > Initial
            3.如果資料還沒到 P03/P05/P06 仍要顯示 (P01 ? Orders ?) 的資訊 [Line Mapping]、[Version] 欄位呈現空白
            */
            this.listControlBindingSource1.DataSource = null;
            string where = string.Empty;
            if (!MyUtility.Check.Empty(this.dateInlineDate.Value1) && !MyUtility.Check.Empty(this.dateInlineDate.Value2))
            {
                where += $"AND o.SewInLine BETWEEN '{this.dateInlineDate.Value1.Value:yyyy-MM-dd}' and '{this.dateInlineDate.Value2.Value:yyyy-MM-dd}'" + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(this.txtfty.Text))
            {
                where += $@"AND o.FtyGroup = '{this.txtfty.Text}'";
            }

            string sqlcmd = $@"
--先取 Orders 基本資訊
SELECT
     o.FtyGroup
    ,o.FactoryID
    ,o.SewInLine
    ,o.StyleUKey
    ,o.StyleID
    ,o.SeasonID
    ,o.BrandID
    ,o.ID
    ,s.CPU
INTO #tmpOrders
FROM Orders o WITH (NOLOCK)
INNER JOIN Factory f WITH (NOLOCK) ON f.ID = o.FactoryID
LEFT JOIN Style s WITH (NOLOCK) ON s.Ukey = o.StyleUkey
WHERE o.Category = 'B'
AND f.IsProduceFty = 1
{where}

--去重複, 會有不同 OrderID
SELECT DISTINCT
     FtyGroup
    ,SewInLine
    ,StyleUKey
    ,StyleID
    ,SeasonID
    ,BrandID
    ,CPU
INTO #tmpDistinctOrders
FROM #tmpOrders

--P03
SELECT
     o.* -- FtyGroup, SewInLine, StyleUKey, StyleID, SeasonID, BrandID, CPU
    ,MaxVersion = ROW_NUMBER() OVER (
            PARTITION BY o.FtyGroup, o.StyleUKey, l.SewingLineID, l.Team
            ORDER BY
                --先以 Phase 排序, 優先度: Final > Prelim > Initial
                CASE 
                    WHEN l.Phase = 'Final' THEN 1 
                    WHEN l.Phase = 'Prelim' THEN 2 
                    WHEN l.Phase = 'Initial' THEN 3 
                    ELSE 4 -- 若還有其它值, 但正常資訊不應該有
                END,
                 --再排序 Version: 目的在同一 Phase 中取最大 Version
                l.Version DESC
        )
    ,[LineMapping] = 'IE P03'
    ,Team
    ,Phase
    ,Version
    ,SewingLineID
    ,Status
    ,AddDate
    ,EditDate
    ,[ttlTimeDiff] = ISNULL(Round(((ISNULL(TotalGSD, 0) - ISNULL(TotalCycle, 0)) / NULLIF(1.0 * TotalGSD, 0)) * 100.0, 2), 0)
    ,[LBR] = ISNULL(Round(TotalCycle / NULLIF(HighestCycle, 0) / NULLIF(CurrentOperators, 0) * 100.0, 0), 0)
    ,AddName = (SELECT TOP 1 IdAndNameAndExt FROM dbo.GetPassName(AddName))
    ,EditName = (SELECT TOP 1 IdAndNameAndExt FROM dbo.GetPassName(EditName))
INTO #tmpP0356
FROM #tmpDistinctOrders o
INNER JOIN LineMapping l WITH (NOLOCK) ON l.StyleUKey = o.StyleUKey

--P05
UNION ALL
SELECT
     o.* -- FtyGroup, SewInLine, StyleUKey, StyleID, SeasonID, BrandID, CPU
    ,MaxVersion = ROW_NUMBER() OVER (
            PARTITION BY o.FtyGroup, o.StyleUKey
            ORDER BY
                --先以 Phase 排序, 優先度: Final > Prelim > Initial
                CASE 
                    WHEN l.Phase = 'Final' THEN 1 
                    WHEN l.Phase = 'Prelim' THEN 2 
                    WHEN l.Phase = 'Initial' THEN 3 
                    ELSE 4 -- 若還有其它值, 但正常資訊不應該有
                END,
                 --再排序 Version: 目的在同一 Phase 中取最大 Version
                l.Version DESC
        )
    ,[LineMapping] = 'IE P05'
    ,Team = ''
    ,Phase
    ,Version
    ,SewingLineID = ''
    ,Status
    ,AddDate
    ,EditDate
    ,[ttlTimeDiff] = 0.0
    ,[LBR] =  Round(TotalGSDTime / NULLIF(HighestGSDTime, 0) / NULLIF(1.0 * SewerManpower, 0) * 100, 0) 
    ,AddName = (SELECT TOP 1 IdAndNameAndExt FROM dbo.GetPassName(AddName))
    ,EditName = (SELECT TOP 1 IdAndNameAndExt FROM dbo.GetPassName(EditName))
FROM #tmpDistinctOrders o
INNER JOIN AutomatedLineMapping l WITH (NOLOCK) ON l.StyleUKey = o.StyleUKey

--P06
UNION ALL
SELECT
     o.* -- FtyGroup, SewInLine, StyleUKey, StyleID, SeasonID, BrandID, CPU
    ,MaxVersion = ROW_NUMBER() OVER (
            PARTITION BY o.FtyGroup, o.StyleUKey, l.SewingLineID, l.Team
            ORDER BY
                --先以 Phase 排序, 優先度: Final > Prelim > Initial
                CASE 
                    WHEN l.Phase = 'Final' THEN 1 
                    WHEN l.Phase = 'Prelim' THEN 2 
                    WHEN l.Phase = 'Initial' THEN 3 
                    ELSE 4 -- 若還有其它值, 但正常資訊不應該有
                END,
                 --再排序 Version: 目的在同一 Phase 中取最大 Version
                l.Version DESC
        )
    ,[LineMapping] = 'IE P06'
    ,Team
    ,Phase
    ,Version
    ,SewingLineID
    ,Status
    ,AddDate
    ,EditDate
    ,[ttlTimeDiff] = ISNULL(Round(((ISNULL(TotalGSDTime, 0) - ISNULL(TotalCycleTime, 0)) / NULLIF(1.0 * TotalGSDTime,0)) * 100.0, 2), 0)
    ,[LBR] = ISNULL(Round(TotalCycleTime / NULLIF(HighestCycleTime, 0) / NULLIF(1.0 * SewerManpower, 0) * 100, 0) , 0)
    ,AddName = (SELECT TOP 1 IdAndNameAndExt FROM dbo.GetPassName(AddName))
    ,EditName = (SELECT TOP 1 IdAndNameAndExt FROM dbo.GetPassName(EditName))
FROM #tmpDistinctOrders o
INNER JOIN LineMappingBalancing l WITH (NOLOCK) ON l.StyleUKey = o.StyleUKey

SELECT *
FROM #tmpP0356

-- 不存在 P03 / P05 / P06, 但存在 SewingSchedule
SELECT DISTINCT
     FtyGroup, SewInLine, StyleUKey, StyleID, SeasonID, BrandID
    ,MaxVersion = 1 -- 勾選 Last Version 固定顯示
FROM #tmpOrders o
WHERE NOT EXISTS (SELECT 1 FROM #tmpP0356 WHERE FtyGroup = o.FtyGroup AND SewInLine = o.SewInLine AND StyleUKey = o.StyleUKey)
AND EXISTS (SELECT 1 FROM SewingSchedule WITH (NOLOCK) WHERE OrderID = o.ID AND FactoryID = o.FactoryID)

DROP TABLE #tmpOrders, #tmpDistinctOrders, #tmpP0356
";

            this.ShowWaitMessage("Data Loading....");
            this.listControlBindingSource1.DataSource = null;
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out DataTable[] dts);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            // 將不存在 P03 / P05 / P06 的資訊加入
            DataTable dt = dts[0];
            foreach (DataRow dr in dts[1].Rows)
            {
                dt.ImportRow(dr);
            }

            DataView dv = dt.DefaultView;
            dv.Sort = "FtyGroup ASC, StyleID ASC, SeasonID ASC, BrandID ASC";
            this.listControlBindingSource1.DataSource = dv.ToTable();
            this.HideWaitMessage();
            if (dt.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found!!");
            }

            this.Grid_Filter();
        }

        private void ChkLaster_CheckedChanged(object sender, EventArgs e)
        {
            this.Grid_Filter();
        }

        /// <inheritdoc/>
        private void Grid_Filter()
        {
            if (this.gridLineMappingStatus.RowCount > 0)
            {
                this.listControlBindingSource1.Filter = this.chkLaster.Checked ? "MaxVersion = 1" : string.Empty;
            }
        }
    }
}
