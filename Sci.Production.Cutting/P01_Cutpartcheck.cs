using Ict;
using Ict.Win;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Sci.Data;

namespace Sci.Production.Cutting
{
    /// <inheritdoc/>
    public partial class P01_Cutpartcheck : Win.Subs.Base
    {
        private string _cutid;
        private string _WorkType;

        /// <summary>
        /// Initializes a new instance of the <see cref="P01_Cutpartcheck"/> class.
        /// </summary>
        /// <param name="cID">Cut ID</param>
        /// <param name="workType">WorkType</param>
        public P01_Cutpartcheck(string cID, string workType)
        {
            this.InitializeComponent();

            this.Text = string.Format("Cut Parts Check<SP:{0}>)", cID);
            this._cutid = cID;
            this._WorkType = workType;
            this.ReQuery();
            this.GridSetup();
            this.gridCutpartcheck.AutoResizeColumns();
        }

        private void ReQuery()
        {
            #region CUTTING_P01_CutPartsCheck  [Prd Qty]數量計算
            string sql = string.Empty, sql2 = string.Empty;
            if (this._WorkType == "1" || this._WorkType == "2")
            {
                sql = string.Format(
                    @";with a as (
	                    select  a.CuttingSP
                                ,b.ID
                                ,b.Article
                                ,b.SizeCode
                                ,( select sum(OQ.qty)
		                            from order_Qty OQ WITH (NOLOCK) 
		                            where OQ.ID=b.ID and OQ.Article=b.Article and OQ.SizeCode=b.SizeCode ) Qty
                                ,c.ColorID
                                ,c.PatternPanel
                                ,[IsCancel] = Cast(iif(a.Junk = 1 and a.NeedProduction = 0, 1, 0) as bit)
	                    from Orders a WITH (NOLOCK) 
	                    inner join order_Qty b WITH (NOLOCK) on a.id = b.id
	                    inner join Order_ColorCombo c WITH (NOLOCK) on c.id = a.POID and c.Article = b.Article 
                            and c.FabricCode is not null and c.FabricCode != ''
                            and c.FabricPanelCode in (select distinct FabricPanelCode from Order_EachCons WITH (NOLOCK) WHERE ID='{0}' and CuttingPiece = 0)  --排除外裁
	                    where a.cuttingsp = '{0}'
                    ) ", this._cutid);
                sql2 = string.Format(
                    @"Select    x.poid
                                ,y.ID
                                ,y.Article
                                ,y.SizeCode
                                ,( select sum(OQ.qty)
		                             from order_Qty OQ WITH (NOLOCK) 
		                             where OQ.ID=y.ID and OQ.Article=y.Article and OQ.SizeCode=y.SizeCode ) QTY
                                ,'' as Colorid
                                ,'=' as Patternpanel
                                ,[IsCancel] = Cast(iif(x.Junk = 1 and x.NeedProduction = 0, 1, 0) as bit)
                                ,null as cutqty
                                ,null as Variance 
	                  from Orders x with (nolock)
                      inner join order_Qty y with (nolock) on  y.id = x.id
                      where x.cuttingsp = '{0}'  ", this._cutid);
            }
            #endregion
            if (sql != string.Empty && sql2 != string.Empty)
            {
                string sqlcmd = sql + string.Format(
            @"
        , b as (
	        Select  b.orderid,b.Article,b.SizeCode,c.PatternPanel
	        ,isnull(sum(b.qty),0) as cutqty
	        from WorkOrder a WITH (NOLOCK) ,WorkOrder_Distribute b WITH (NOLOCK) , WorkOrder_PatternPanel c WITH (NOLOCK) 
	        Where a.id = '{0}' 
	        and a.ukey = b.WorkOrderUkey
	        and a.Ukey = c.WorkOrderUkey and a.FabricPanelCode=c.FabricPanelCode
	        group by b.orderid,b.Article,b.SizeCode,c.PatternPanel
        )
        , c as(
	        Select a.* ,b.cutqty, b.cutqty - a.qty as Variance 
	        from a 
	        inner join b on a.id = b.orderid 
	        and a.Article = b.Article 
	        and a.PatternPanel = b.PatternPanel 
	        and a.SizeCode = b.SizeCode
	        union all 
	        {1}
        )

        select c.*,z.seq
        from c
        inner join Order_SizeCode z WITH (NOLOCK) on z.id = c.CuttingSP and z.SizeCode = c.SizeCode
        order by c.id,article,z.seq,PatternPanel",
            this._cutid,
            sql2);

                DualResult result = DBProxy.Current.Select(null, sqlcmd, out DataTable gridtb);

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
