using Ict;
using Ict.Win;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Sci.Data;
using System.Linq;

namespace Sci.Production.Cutting
{
    /// <inheritdoc/>
    public partial class Cutpartchecksummary : Win.Subs.Base
    {
        private string cutid;
        private string _sourceTable;
        private DataTable dt_curDetailData;
        private DataTable dt_curDistribute;
        private DataTable dt_curSizeRatio;

        private DataTable fabcodetb; // PatternPanel Table

        /// <summary>
        /// Initializes a new instance of the <see cref="Cutpartchecksummary"/> class.
        /// </summary>
        /// <param name="cID">Cut ID</param>
        /// <param name="sourceTable">source Table</param>
        /// <param name="curDetailData">currrent DetailData</param>
        /// <param name="curDistribute">P09 當前Distribute資料</param>
        /// <param name="curSizeRatio">P02 size Ratio Grid的當前資料</param>
        public Cutpartchecksummary(string cID, string sourceTable, DataTable curDetailData, DataTable curDistribute = null, DataTable curSizeRatio = null)
        {
            this.InitializeComponent();
            this.Text = string.Format("Cut Parts Check Summary<SP:{0}>)", cID);
            this.cutid = cID;
            this._sourceTable = sourceTable;
            this.dt_curDetailData = curDetailData;
            this.dt_curDistribute = curDistribute;
            this.dt_curSizeRatio = curSizeRatio;
            this.ReQuery();
            this.GridSetup();
            this.gridCutpartchecksummary.AutoResizeColumns();
        }

        private void ReQuery()
        {
            #region 找出有哪些部位
            string fabcodesql = string.Format(
                @"
            Select distinct a.PatternPanel
            from Order_FabricCode a WITH (NOLOCK) ,Order_EachCons b WITH (NOLOCK) 
            where a.id = '{0}' and a.FabricCode is not null and a.FabricCode !='' 
            and b.id = '{0}' and a.id = b.id and b.cuttingpiece='0' and  b.FabricCombo = a.PatternPanel
            order by patternpanel", this.cutid);
            DualResult fabresult = DBProxy.Current.Select("Production", fabcodesql, out this.fabcodetb);
            if (!fabresult)
            {
                this.ShowErr(fabresult);
                return;
            }
            #endregion

            #region 建立Grid
            string settbsql = @"
Select  [ID] = b.POID
        ,a.article
        ,a.sizecode
        ,a.qty,'' as complete
"; // 寫SQL建立Table

            // 組動態欄位
            foreach (DataRow dr in this.fabcodetb.Rows)
            {
                settbsql += ", 0 as " + dr["PatternPanel"];
            }

            settbsql += string.Format(
                @"  
                    ,[IsCancel] = Cast(iif(b.Junk = 1 and b.NeedProduction = 0, 1, 0) as bit)
                    From Order_Qty a WITH (NOLOCK) 
                    inner join orders b WITH (NOLOCK)  on a.id = b.id 
                    inner join Order_SizeCode c WITH (NOLOCK) on c.id = b.poid and c.SizeCode = a.SizeCode
                    Where b.cuttingsp ='{0}' 
                    order by b.POID, a.article, c.Seq",
                this.cutid);
            DualResult gridResult = DBProxy.Current.Select(null, settbsql, out DataTable gridtb);
            if (!gridResult)
            {
                this.ShowErr(gridResult);
                return;
            }
            #endregion

            #region 寫入部位數量

            // 合併當前P02,P09畫面資料
            DataTable resutTable = new DataTable();

            // 資料來源是WorkOrderForOutput Cutting_P09
            if (string.Compare(this._sourceTable, "WorkOrderForOutput", true) == 0)
            {
                if ((this.dt_curDetailData == null || this.dt_curDetailData.Rows.Count == 0) ||
                    (this.dt_curDistribute == null || this.dt_curDistribute.Rows.Count == 0))
                {
                    return;
                }

                var query = from t1 in this.dt_curDetailData.AsEnumerable()
                            join t2 in this.dt_curDistribute.AsEnumerable()
                            on t1.Field<int>("Ukey") equals t2.Field<int>("WorkOrderForOutputUkey")
                            select new
                            {
                               ID = t1.Field<string>("ID"),
                               FabricPanelCode = t1.Field<string>("FabricPanelCode"),
                               OrderID = t2.Field<string>("orderid"),
                               Article = t2.Field<string>("Article"),
                               SizeCode = t2.Field<string>("sizecode"),
                               Qty = t2.Field<int>("Qty"),
                            };

                resutTable.Columns.Add("ID", typeof(string));
                resutTable.Columns.Add("FabricPanelCode", typeof(string));
                resutTable.Columns.Add("OrderID", typeof(string));
                resutTable.Columns.Add("Article", typeof(string));
                resutTable.Columns.Add("SizeCode", typeof(string));
                resutTable.Columns.Add("Qty", typeof(int));

                foreach (var item in query)
                {
                    resutTable.Rows.Add(item.ID, item.FabricPanelCode, item.OrderID, item.Article, item.SizeCode, item.Qty);
                }
            }
            else
            {
                // 資料來源是WorkOrderForPlanning Cutting_P02
                if ((this.dt_curDetailData == null || this.dt_curDetailData.Rows.Count == 0) ||
                   (this.dt_curSizeRatio == null || this.dt_curSizeRatio.Rows.Count == 0))
                {
                    return;
                }

                var query = from t1 in this.dt_curDetailData.AsEnumerable()
                            join t2 in this.dt_curSizeRatio.AsEnumerable()
                            on t1.Field<int>("Ukey") equals t2.Field<int>("WorkOrderForPlanningUkey")
                            select new
                            {
                                ID = t1.Field<string>("ID"),
                                FabricPanelCode = t1.Field<string>("FabricPanelCode"),
                                OrderID = t1.Field<string>("orderid"),
                                Article = t1.Field<string>("Article"),
                                SizeCode = t2.Field<string>("sizecode"),
                                Qty = t2.Field<int>("TtlQty"), // 取自 P02 Grid SizeRatio Tlt.Qty
                            };

                resutTable.Columns.Add("ID", typeof(string));
                resutTable.Columns.Add("FabricPanelCode", typeof(string));
                resutTable.Columns.Add("OrderID", typeof(string));
                resutTable.Columns.Add("Article", typeof(string));
                resutTable.Columns.Add("SizeCode", typeof(string));
                resutTable.Columns.Add("Qty", typeof(int));

                foreach (var item in query)
                {
                    resutTable.Rows.Add(item.ID, item.FabricPanelCode, item.OrderID, item.Article, item.SizeCode, item.Qty);
                }
            }

            string getqtysql =
                $@"
Select a.article,a.sizecode,a.qty,c.PatternPanel,a.ID 
from #tmp a WITH (NOLOCK) 
inner join Order_fabriccode c WITH (NOLOCK) on a.id=c.id and a.FabricPanelCode = c.FabricPanelCode 
Where a.id = '{this.cutid}' 
and a.article !=''
";
            gridResult = DBProxy.Current.Select(null, getqtysql, out DataTable getqtytb);
            if (!gridResult)
            {
                this.ShowErr(gridResult);
                return;
            }

            foreach (DataRow dr in getqtytb.Rows)
            {
                DataRow[] gridselect = gridtb.Select($"id = '{dr["ID"]}' and article = '{dr["article"]}' and sizecode = '{dr["sizecode"]}'");
                if (gridselect.Length != 0)
                {
                    gridselect[0][dr["PatternPanel"].ToString()] = MyUtility.Convert.GetDecimal(gridselect[0][dr["PatternPanel"].ToString()]) + MyUtility.Convert.GetDecimal(dr["Qty"]);
                }
            }
            #endregion

            #region 判斷是否Complete
            bool complete = true;
            fabcodesql = string.Format(
                @"
select distinct occ.Article,occ.Patternpanel
from Orders o WITH (NOLOCK)
inner join Order_Qty oq WITH (NOLOCK) on oq.id = o.id
inner join Order_ColorCombo occ WITH (NOLOCK) on occ.id = o.POID and occ.FabricCode is not null and occ.FabricCode !='' and occ.Article = oq.Article
inner join Order_EachCons oe WITH (NOLOCK) on oe.id = occ.id and oe.FabricCombo = occ.PatternPanel and oe.CuttingPiece = 0
inner join Order_EachCons_Color oec WITH (NOLOCK) on oec.Order_EachConsUkey = oe.Ukey and oec.ColorID = occ.ColorID
inner join Order_EachCons_Color_Article oeca WITH (NOLOCK) on oeca.Order_EachCons_ColorUkey = oec.Ukey and oeca.Article = oq.Article --and oeca.ColorID = oec.ColorID
where o.CuttingSP = '{0}'
and o.junk = 0
", this.cutid);
            gridResult = DBProxy.Current.Select(null, fabcodesql, out DataTable panneltb);
            if (!gridResult)
            {
                this.ShowErr(gridResult);
                return;
            }

            foreach (DataRow dr in gridtb.Rows)
            {
                complete = true;
                DataRow[] sel = panneltb.Select(string.Format("Article = '{0}'", dr["Article"]));
                foreach (DataRow pdr in sel)
                {
                    if (MyUtility.Convert.GetDecimal(dr["Qty"]) > MyUtility.Convert.GetDecimal(dr[pdr["Patternpanel"].ToString()]))
                    {
                        complete = false;
                    }
                }

                if (complete)
                {
                    dr["Complete"] = "Y";
                }
            }

            #endregion
            this.gridCutpartchecksummary.DataSource = gridtb;
        }

        private void GridSetup()
        {
            this.Helper.Controls.Grid.Generator(this.gridCutpartchecksummary)
                .Text("id", header: "SP #", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("Article", header: "Article", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("SizeCode", header: "SizeCode", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Numeric("Qty", header: "Order Qty", width: Widths.AnsiChars(7), iseditingreadonly: true)
                .Text("Complete", header: "Complete", width: Widths.AnsiChars(1), iseditingreadonly: true);

            #region Grid 變色規則
            Color backDefaultColor = this.gridCutpartchecksummary.DefaultCellStyle.BackColor;

            this.gridCutpartchecksummary.RowsAdded += (s, e) =>
            {
                int index = 0;
                string art = string.Empty;
                foreach (DataGridViewRow dr in this.gridCutpartchecksummary.Rows)
                {
                    DataRow sourceDr = this.gridCutpartchecksummary.GetDataRow(dr.Index);
                    if (MyUtility.Convert.GetBool(sourceDr["IsCancel"]))
                    {
                        dr.DefaultCellStyle.ForeColor = Color.Gray;
                    }
                    else
                    {
                        dr.DefaultCellStyle.ForeColor = Color.Black;
                    }

                    if (index == 0)
                    {
                        art = dr.Cells[1].Value.ToString();
                        index++;
                        continue;
                    }

                    if (dr.Cells[1].Value.ToString() != art)
                    {
                        this.gridCutpartchecksummary.Rows[index - 1].DefaultCellStyle.BackColor = Color.Pink;
                        art = dr.Cells[1].Value.ToString();
                    }
                    else
                    {
                        this.gridCutpartchecksummary.Rows[index - 1].DefaultCellStyle.BackColor = backDefaultColor;
                    }

                    index++;
                }
            };
            #endregion

            for (int i = 0; i < this.fabcodetb.Rows.Count; i++)
            {
                this.Helper.Controls.Grid.Generator(this.gridCutpartchecksummary)
                    .Numeric(this.fabcodetb.Rows[i]["PatternPanel"].ToString().Trim(), header: this.fabcodetb.Rows[i]["PatternPanel"].ToString(), width: Widths.AnsiChars(7)).Get(out Ict.Win.UI.DataGridViewNumericBoxColumn col_color);

                col_color.CellFormatting += (s, e) =>
                {
                    if (e.RowIndex == -1)
                    {
                        return;
                    }

                    DataRow dr = this.gridCutpartchecksummary.GetDataRow(e.RowIndex);

                    if (MyUtility.Convert.GetDecimal(dr[e.ColumnIndex]) < MyUtility.Convert.GetDecimal(dr["Qty"]) &&
                        !MyUtility.Convert.GetBool(dr["IsCancel"]))
                    {
                        e.CellStyle.ForeColor = Color.Red;
                        if (MyUtility.Convert.GetDecimal(dr[e.ColumnIndex]) > 0 && dr["Complete"].ToString() != string.Empty)
                        {
                            dr["Complete"] = string.Empty;
                        }
                    }
                };
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
