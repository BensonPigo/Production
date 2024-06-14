using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Ict;
using Ict.Win;

namespace Sci.Production.Cutting
{
    /// <inheritdoc/>
    public partial class Cutpartcheck : Win.Subs.Base
    {
        private string _cutid;
        private string _WorkType;
        private string _sourceTable;
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
        public Cutpartcheck(string cID, string workType, string sourceTable, DataTable curDetailData, DataTable curDistribute = null, DataTable curPatternPanel = null, DataTable curSizeRatio = null)
        {
            this.InitializeComponent();

            this.Text = string.Format("Cut Parts Check<SP:{0}>)", cID);
            this._cutid = cID;
            this._WorkType = workType;
            this._sourceTable = sourceTable;
            this.dt_curDetailData = curDetailData;
            this.dt_curDistribute = curDistribute;
            this.dt_curPatternPanel = curPatternPanel;
            this.dt_curSizeRatio = curSizeRatio;
            this.ReQuery();
            this.GridSetup();
            this.gridCutpartcheck.AutoResizeColumns();
        }

        private void ReQuery()
        {
            #region 合併當前P02,P09畫面資料
            DataTable resutTable = new DataTable();

            // 資料來源是WorkOrderForOutput Cutting_P09
            if (string.Compare(this._sourceTable, "WorkOrderForOutput", true) == 0)
            {
                if ((this.dt_curDetailData == null || this.dt_curDetailData.Rows.Count == 0) ||
                    (this.dt_curDistribute == null || this.dt_curDistribute.Rows.Count == 0) ||
                    (this.dt_curPatternPanel == null || this.dt_curPatternPanel.Rows.Count == 0))
                {
                    return;
                }

                var query = from t1 in this.dt_curDetailData.AsEnumerable()
                            join t2 in this.dt_curDistribute.AsEnumerable()
                            on t1.Field<int>("Ukey") equals t2.Field<int>("WorkOrderForOutputUkey")
                            join t3 in this.dt_curPatternPanel.AsEnumerable()
                            on new { ukey = t1.Field<int>("Ukey"), FabricPanelCode = t1.Field<string>("FabricPanelCode") }
                            equals new { ukey = t3.Field<int>("WorkOrderForOutputUkey"), FabricPanelCode = t3.Field<string>("FabricPanelCode") }
                            group new { t1, t2, t3 } by new
                            {
                                OrderID = t2.Field<string>("OrderID"),
                                Article = t2.Field<string>("Article"),
                                SizeCode = t2.Field<string>("SizeCode"),
                                PatternPanel = t3.Field<string>("PatternPanel"),
                            } into g
                            select new
                            {
                                OrderID = g.Key.OrderID,
                                Article = g.Key.Article,
                                SizeCode = g.Key.SizeCode,
                                PatternPanel = g.Key.PatternPanel,
                                CutQty = g.Sum(x => x.t2.Field<int>("qty")),
                            };

                resutTable.Columns.Add("OrderID", typeof(string));
                resutTable.Columns.Add("Article", typeof(string));
                resutTable.Columns.Add("SizeCode", typeof(string));
                resutTable.Columns.Add("PatternPanel", typeof(string));
                resutTable.Columns.Add("CutQty", typeof(int));

                foreach (var item in query)
                {
                    resutTable.Rows.Add(item.OrderID, item.Article, item.SizeCode, item.PatternPanel, item.CutQty);
                }
            }
            else
            {
                // 資料來源是WorkOrderForPlanning Cutting_P02
                if ((this.dt_curDetailData == null || this.dt_curDetailData.Rows.Count == 0) ||
                   (this.dt_curSizeRatio == null || this.dt_curSizeRatio.Rows.Count == 0) ||
                    (this.dt_curPatternPanel == null || this.dt_curPatternPanel.Rows.Count == 0))
                {
                    return;
                }

                var query = from t1 in this.dt_curDetailData.AsEnumerable()
                            join t2 in this.dt_curSizeRatio.AsEnumerable()
                            on t1.Field<int>("Ukey") equals t2.Field<int>("WorkOrderForPlanningUkey")
                            join t3 in this.dt_curPatternPanel.AsEnumerable()
                            on new { ukey = t1.Field<int>("Ukey"), FabricPanelCode = t1.Field<string>("FabricPanelCode") }
                            equals new { ukey = t3.Field<int>("WorkOrderForPlanningUkey"), FabricPanelCode = t3.Field<string>("FabricPanelCode") }
                            group new { t1, t2, t3 } by new
                            {
                                OrderID = t1.Field<string>("OrderID"),
                                Article = t1.Field<string>("Article"),
                                SizeCode = t2.Field<string>("SizeCode"),
                                PatternPanel = t3.Field<string>("PatternPanel"),
                            } into g
                            select new
                            {
                                OrderID = g.Key.OrderID,
                                Article = g.Key.Article,
                                SizeCode = g.Key.SizeCode,
                                PatternPanel = g.Key.PatternPanel,
                                CutQty = g.Sum(x => x.t2.Field<int>("TtlQty")), // 取自 P02 Grid SizeRatio Tlt.Qty
                            };

                resutTable.Columns.Add("OrderID", typeof(string));
                resutTable.Columns.Add("Article", typeof(string));
                resutTable.Columns.Add("SizeCode", typeof(string));
                resutTable.Columns.Add("PatternPanel", typeof(string));
                resutTable.Columns.Add("CutQty", typeof(int));

                foreach (var item in query)
                {
                    resutTable.Rows.Add(item.OrderID, item.Article, item.SizeCode, item.PatternPanel, item.CutQty);
                }
            }

            #endregion

            #region CUTTING_CutPartsCheck  [Prd Qty]數量計算
            string sqlcmd = string.Empty;
            if (this._WorkType == "1" || this._WorkType == "2")
            {
                sqlcmd = $@"
select  a.CuttingSP
		,a.POID
        ,b.Article
        ,b.SizeCode
        ,( select sum(OQ.qty)
		    from order_Qty OQ WITH (NOLOCK) 
		    where OQ.ID=b.ID and OQ.Article=b.Article and OQ.SizeCode=b.SizeCode ) Qty
        ,c.ColorID
        ,c.PatternPanel
        ,[IsCancel] = Cast(iif(a.Junk = 1 and a.NeedProduction = 0, 1, 0) as bit)
into #tmpOrder
from Orders a WITH (NOLOCK) 
inner join order_Qty b WITH (NOLOCK) on a.id = b.id
inner join Order_ColorCombo c WITH (NOLOCK) on c.id = a.POID and c.Article = b.Article 
    and c.FabricCode is not null and c.FabricCode != ''
    and c.FabricPanelCode in (select distinct FabricPanelCode from Order_EachCons WITH (NOLOCK) WHERE ID='{this._cutid}' and CuttingPiece = 0)  --排除外裁
where a.cuttingsp = '{this._cutid}'

Select o.POID,a.Article,a.SizeCode,a.PatternPanel
,cutqty = isnull(sum(a.CutQty),0)
into #tmpWorkOrder
from #tmp a WITH (NOLOCK) 
inner join orders o WITH (NOLOCK) on o.id = a.OrderID
group by o.POID,a.Article,a.SizeCode,a.PatternPanel

select * 
into #tmpC
from (
    Select a.POID,a.Article,a.SizeCode,a.Qty,a.ColorID,a.PatternPanel, a.IsCancel,b.cutqty, Variance = b.cutqty - a.qty
    from #tmpOrder a
    inner join #tmpWorkOrder b on a.POID = b.POID
    and a.Article = b.Article 
    and a.PatternPanel = b.PatternPanel 
    and a.SizeCode = b.SizeCode

union all 
    
Select x.poid
,y.Article
,y.SizeCode
,QTY= ( select sum(OQ.qty)
		from order_Qty OQ WITH (NOLOCK) 
		where OQ.ID=y.ID and OQ.Article=y.Article and OQ.SizeCode=y.SizeCode ) 
,Colorid = ''
,Patternpanel = '=' 
,[IsCancel] = Cast(iif(x.Junk = 1 and x.NeedProduction = 0, 1, 0) as bit)
,null as cutqty
,null as Variance 
from Orders x with (nolock)
inner join order_Qty y with (nolock) on  y.id = x.id
where x.cuttingsp = '{this._cutid}'

) a

select c.*,z.seq
from #tmpC c
inner join Order_SizeCode z WITH (NOLOCK) on z.id = c.POID and z.SizeCode = c.SizeCode
order by c.POID,article,z.seq,PatternPanel

drop table #tmpOrder,#tmpWorkOrder,#tmpC
                
                ";
                DualResult result = MyUtility.Tool.ProcessWithDatatable(resutTable, string.Empty, sqlcmd, out DataTable gridtb, "#tmp");

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
            }
            #endregion
        }

        private void GridSetup()
        {
            this.Helper.Controls.Grid.Generator(this.gridCutpartcheck)
                .Text("id", header: "SP #", width: Widths.AnsiChars(13), iseditingreadonly: true)
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
