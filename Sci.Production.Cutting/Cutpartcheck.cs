using Ict;
using Ict.Win;
using Sci.Production.Prg;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static Sci.Production.Cutting.CuttingWorkOrder;

namespace Sci.Production.Cutting
{
    /// <inheritdoc/>
    public partial class Cutpartcheck : Win.Subs.Base
    {
        CuttingForm form;
        private string _cutid;
        private DataTable dt_curDetailData;
        private DataTable dt_curDistribute;
        private DataTable dt_curPatternPanel;
        private DataTable dt_curSizeRatio;

        /// <summary>
        /// Initializes a new instance of the <see cref="Cutpartcheck"/> class.
        /// </summary>
        /// <param name="cID">Cut ID</param>
        /// <param name="workType">WorkType</param>
        /// <param name="sourceTable">source Table</param>
        /// <param name="curDetailData">currrent DetailData</param>
        /// <param name="curDistribute">currrent Distribute</param>
        /// <param name="curPatternPanel">currrent PatternPanel</param>
        /// <param name="curSizeRatio">P02 size Ratio Grid的當前資料</param>
        /// <param name="form">P02 / P09</param>
        public Cutpartcheck(CuttingForm form, string cID, DataTable curDetailData, DataTable curPatternPanel = null, DataTable curSizeRatio = null, DataTable curDistribute = null)
        {
            this.InitializeComponent();

            this.form = form;
            this.Text = string.Format("Cut Parts Check<SP:{0}>)", cID);
            this._cutid = cID;
            this.dt_curDetailData = curDetailData;
            this.dt_curPatternPanel = curPatternPanel == null ? null : curPatternPanel.AsEnumerable().Where(w => w.RowState != DataRowState.Deleted).TryCopyToDataTable(curPatternPanel);
            this.dt_curSizeRatio = curSizeRatio == null ? null : curSizeRatio.AsEnumerable().Where(w => w.RowState != DataRowState.Deleted).TryCopyToDataTable(curSizeRatio);
            this.dt_curDistribute = curDistribute == null ? null : curDistribute.AsEnumerable().Where(w => w.RowState != DataRowState.Deleted).TryCopyToDataTable(curDistribute);
            this.ReQuery();
            this.GridSetup();
            this.gridCutpartcheck.AutoResizeColumns();
        }

        private void ReQuery()
        {
            #region 合併當前P02,P09畫面資料
            if ((this.dt_curDetailData == null || this.dt_curDetailData.Rows.Count == 0) ||
               (this.dt_curSizeRatio == null || this.dt_curSizeRatio.Rows.Count == 0) ||
                (this.dt_curPatternPanel == null || this.dt_curPatternPanel.Rows.Count == 0))
            {
                return;
            }

            DataTable resutTable = new DataTable();
            resutTable.Columns.Add("OrderID", typeof(string));
            resutTable.Columns.Add("Article", typeof(string));
            resutTable.Columns.Add("SizeCode", typeof(string));
            resutTable.Columns.Add("PatternPanel", typeof(string));
            resutTable.Columns.Add("CutQty", typeof(int));

            string ukeyName = GetWorkOrderUkeyName(this.form);
            var query = from t1 in this.dt_curDetailData.AsEnumerable()
                        join t2 in (this.form == CuttingForm.P02 ? this.dt_curSizeRatio : this.dt_curDistribute).AsEnumerable()
                        on new
                        {
                            ukey = MyUtility.Convert.GetInt(t1["Ukey"]),
                            tmpkey = MyUtility.Convert.GetInt(t1["tmpkey"]),
                        }
                        equals new
                        {
                            ukey = MyUtility.Convert.GetInt(t2[ukeyName]),
                            tmpkey = MyUtility.Convert.GetInt(t2["tmpkey"]),
                        }
                        join t3 in this.dt_curPatternPanel.AsEnumerable()
                        on new
                        {
                            ukey = MyUtility.Convert.GetInt(t1["Ukey"]),
                            tmpkey = MyUtility.Convert.GetInt(t1["tmpkey"]),
                            FabricPanelCode = MyUtility.Convert.GetString(t1["FabricPanelCode"]),
                        }
                        equals new
                        {
                            ukey = MyUtility.Convert.GetInt(t3[ukeyName]),
                            tmpkey = MyUtility.Convert.GetInt(t3["tmpkey"]),
                            FabricPanelCode = MyUtility.Convert.GetString(t3["FabricPanelCode"]),
                        }
                        group new { t1, t2, t3 } by new
                        {
                            OrderID = MyUtility.Convert.GetString(this.form == CuttingForm.P02 ? t1["OrderID"] : t2["OrderID"]),
                            Article = MyUtility.Convert.GetString(this.form == CuttingForm.P02 ? t1["Article"] : t2["Article"]),
                            SizeCode = MyUtility.Convert.GetString(t2["SizeCode"]),
                            PatternPanel = MyUtility.Convert.GetString(t3["PatternPanel"]),
                        }
                        into g
                        select new
                        {
                            g.Key.OrderID,
                            g.Key.Article,
                            g.Key.SizeCode,
                            g.Key.PatternPanel,
                            CutQty = g.Sum(x => MyUtility.Convert.GetInt(x.t2[this.form == CuttingForm.P02 ? "Qty" : "Qty"]) * (this.form == CuttingForm.P02 ? MyUtility.Convert.GetInt(x.t2["Layer"]) : 1)),
                        };

            foreach (var item in query)
            {
                resutTable.Rows.Add(item.OrderID, item.Article, item.SizeCode, item.PatternPanel, item.CutQty);
            }

            #endregion

            #region CUTTING CutPartsCheck  [Prd Qty]數量計算, P02 是 by POID

            string sqlcmd = $@"
SELECT
    o.POID
   ,oq.ID
   ,oq.Article
   ,oq.SizeCode
   ,oq.Qty
   ,occ.ColorID
   ,occ.PatternPanel
   ,[IsCancel] = CAST(IIF(o.Junk = 1 AND o.NeedProduction = 0, 1, 0) AS BIT)
INTO #tmpOrder
FROM Orders o WITH (NOLOCK)
INNER JOIN Order_Qty oq WITH (NOLOCK) ON o.id = oq.id
INNER JOIN Order_ColorCombo occ WITH (NOLOCK)
    ON occ.id = o.POID
        AND occ.Article = oq.Article
        AND occ.FabricCode <> ''
        AND EXISTS (SELECT 1 FROM Order_EachCons WITH (NOLOCK) WHERE ID = '{this._cutid}' AND CuttingPiece = 0 AND FabricPanelCode = occ.FabricPanelCode)  --排除外裁
WHERE o.CuttingSP = '{this._cutid}'
";
            if (this.form == CuttingForm.P02)
            {
                sqlcmd += $@"
SELECT
     o.POID
    ,t.Article
    ,t.SizeCode
    ,t.PatternPanel
    ,CutQty = SUM(t.CutQty)
INTO #tmpWorkPOID
FROM #tmp t
INNER JOIN Orders o WITH (NOLOCK) ON o.ID = t.OrderID
GROUP BY o.POID, t.Article, t.SizeCode, t.PatternPanel

SELECT
     o.POID
    ,o.Article
    ,o.SizeCode
    ,Qty = SUM(o.Qty)
    ,o.ColorID
    ,o.PatternPanel
    ,o.IsCancel
INTO #tmpPOID
FROM #tmpOrder o
GROUP BY o.POID, o.Article, o.SizeCode, o.ColorID, o.PatternPanel, o.IsCancel

-- 存在 WorkOrder 裁剪數 CutQty 從 WorkOrder 取出
SELECT
    ID = o.POID
   ,o.Article
   ,o.SizeCode
   ,o.Qty
   ,o.ColorID
   ,o.PatternPanel
   ,o.IsCancel
   ,w.cutqty
   ,Variance = w.CutQty - o.Qty
INTO #tmpFinal
FROM #tmpPOID o
INNER JOIN #tmpWorkPOID w ON o.POID = w.POID
                         AND o.Article = w.Article
                         AND o.PatternPanel = w.PatternPanel
                         AND o.SizeCode = w.SizeCode
UNION ALL

-- 紅色部分
SELECT DISTINCT
    ID = o.POID
   ,o.Article
   ,o.SizeCode
   ,o.Qty
   ,ColorID = ''
   ,Patternpanel = '='
   ,o.IsCancel
   ,CutQty = NULL
   ,Variance = NULL
FROM #tmpPOID o WITH (NOLOCK)

-- 取 SizeCode 排序
SELECT
    c.*
   ,os.Seq
FROM #tmpFinal c
INNER JOIN Order_SizeCode os WITH (NOLOCK) ON os.id = c.ID AND os.SizeCode = c.SizeCode
ORDER BY c.ID, Article, os.Seq, PatternPanel

DROP TABLE #tmp, #tmpOrder, #tmpFinal
,#tmpWorkPOID,#tmpPOID
";
            }
            else
            {
                sqlcmd += $@"
-- 存在 WorkOrder 裁剪數 CutQty 從 WorkOrder 取出
SELECT
    o.POID
   ,o.ID
   ,o.Article
   ,o.SizeCode
   ,o.Qty
   ,o.ColorID
   ,o.PatternPanel
   ,o.IsCancel
   ,w.cutqty
   ,Variance = w.CutQty - o.Qty
INTO #tmpFinal
FROM #tmpOrder o
INNER JOIN #tmp w ON o.ID = w.OrderID -- 此處 P09 會把 EXCESS 排除
                 AND o.Article = w.Article
                 AND o.PatternPanel = w.PatternPanel
                 AND o.SizeCode = w.SizeCode
UNION ALL

-- 紅色部分
SELECT DISTINCT
    o.POID
   ,o.ID
   ,o.Article
   ,o.SizeCode
   ,o.Qty
   ,ColorID = ''
   ,Patternpanel = '='
   ,o.IsCancel
   ,CutQty = NULL
   ,Variance = NULL
FROM #tmpOrder o WITH (NOLOCK)

-- 取 SizeCode 排序
SELECT
    c.*
   ,os.Seq
FROM #tmpFinal c
INNER JOIN Order_SizeCode os WITH (NOLOCK) ON os.id = c.POID AND os.SizeCode = c.SizeCode
ORDER BY c.ID, Article, os.Seq, PatternPanel

DROP TABLE #tmp, #tmpOrder, #tmpFinal
";
            }

            DualResult result = MyUtility.Tool.ProcessWithDatatable(resutTable, string.Empty, sqlcmd, out DataTable gridtb);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            for (int i = gridtb.Rows.Count - 1; i > 0; i--)
            {
                if (gridtb.Rows[i]["PatternPanel"].ToString() != "=" &&
                    MyUtility.Check.Empty(gridtb.Rows[i]["cutqty"]) &&
                    MyUtility.Check.Empty(gridtb.Rows[i]["Variance"]))
                {
                    if (gridtb.Rows[i][0].ToString() == gridtb.Rows[i - 1][0].ToString() &&
                        gridtb.Rows[i][1].ToString() == gridtb.Rows[i - 1][1].ToString() &&
                        gridtb.Rows[i][2].ToString() == gridtb.Rows[i - 1][2].ToString() &&
                        gridtb.Rows[i][3].ToString() == gridtb.Rows[i - 1][3].ToString() &&
                        gridtb.Rows[i][4].ToString() == gridtb.Rows[i - 1][4].ToString() &&
                        gridtb.Rows[i][5].ToString() == gridtb.Rows[i - 1][5].ToString())
                    {
                        gridtb.Rows[i].Delete();
                    }
                }
            }

            this.gridCutpartcheck.DataSource = gridtb;
            #endregion
        }

        private void GridSetup()
        {
            this.Helper.Controls.Grid.Generator(this.gridCutpartcheck)
                .Text("ID", header: "SP #", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("Article", header: "Article", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("SizeCode", header: "SizeCode", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("Colorid", header: "Color", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("Patternpanel", header: "Comb", width: Widths.AnsiChars(2), iseditingreadonly: true)
                .Numeric("Qty", header: "Prd Qty", width: Widths.AnsiChars(7), iseditingreadonly: true)
                .Numeric("CutQty", header: "Cut Qty", width: Widths.AnsiChars(7), iseditingreadonly: true)
                .Numeric("Variance", header: "Variance", width: Widths.AnsiChars(7), iseditingreadonly: true);

            #region Grid 變色規則
            Color backDefaultColor = this.gridCutpartcheck.DefaultCellStyle.BackColor;

            this.gridCutpartcheck.RowsAdded += (s, e) =>
            {
                if (e.RowIndex < 0)
                {
                    return;
                }

                foreach (DataGridViewRow gridDr in this.gridCutpartcheck.Rows)
                {
                    DataRow dr = this.gridCutpartcheck.GetDataRow(gridDr.Index);

                    gridDr.DefaultCellStyle.BackColor = dr["Patternpanel"].ToString().EqualString("=") ? Color.Pink : backDefaultColor;
                    if (MyUtility.Convert.GetBool(dr["IsCancel"]))
                    {
                        gridDr.DefaultCellStyle.ForeColor = Color.Gray;
                    }
                    else
                    {
                        gridDr.DefaultCellStyle.ForeColor = Color.Black;
                    }
                }
            };
            #endregion
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
