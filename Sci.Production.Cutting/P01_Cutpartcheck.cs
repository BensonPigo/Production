using Ict;
using Ict.Win;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Sci.Data;

namespace Sci.Production.Cutting
{
    public partial class P01_Cutpartcheck : Sci.Win.Subs.Base
    {
        private string cutid;
        public P01_Cutpartcheck(string cID)
        {
            InitializeComponent();

            this.Text = string.Format("Cut Parts Check<SP:{0}>)", cID);
            cutid = cID;
            requery();
            gridSetup();

        }
        private void requery()
        {
            string sqlcmd = String.Format(
            @"with a as (
	            Select d.*,e.Colorid,e.PatternPanel
	            from 
	            (Select b.POID,c.ID,c.Article,c.SizeCode,c.Qty 
	            from (Select id,POID from Orders a where a.cuttingsp = '{0}') as b,
	            order_Qty c where c.id = b.id) d,Order_ColorCombo e 
	            where d.POID=e.id and d.Article = e.Article and e.FabricCode is not null and e.FabricCode !=''
            )
            , b as (
	            Select  b.orderid,b.Article,b.SizeCode,c.PatternPanel,isnull(sum(b.qty),0) as cutqty 
	            from WorkOrder a ,WorkOrder_Distribute b , WorkOrder_PatternPanel c 
	            Where a.id = '{0}' and a.ukey = b.WorkOrderUkey and a.Ukey = c.WorkOrderUkey 
	            group by b.orderid,b.Article,b.SizeCode,c.PatternPanel
            )
            , c as(
            Select a.* ,b.cutqty,a.qty - b.cutqty as Variance 
            from a 
            left join b on a.id = b.orderid and a.Article = b.Article and a.PatternPanel = b.PatternPanel and a.SizeCode = b.SizeCode
            union all 
            Select x.poid,y.ID,y.Article,y.SizeCode,y.qty,'' as Colorid,'=' as Patternpanel,null as cutqty,null as Variance 
            from (Select id,POID from Orders z where z.cuttingsp = '{0}') as x,order_Qty y 
            where y.id = x.id
            )

            select c.*,z.seq
            from c
            inner join Order_SizeCode z on z.id = c.POID and z.SizeCode = c.SizeCode
            order by c.id,article,z.seq,PatternPanel", cutid);
            DataTable gridtb;
            DualResult dr = DBProxy.Current.Select(null, sqlcmd, out gridtb);

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
            grid1.DataSource = gridtb;
        }
        
        private void gridSetup()
        {
            grid1.RowPostPaint += (s, e) =>
            {
                for (int i = 0; i < e.RowIndex; i++)
                {
                    if (grid1.Rows[i].Cells[4].Value.ToString() == "=")
                    {
                        grid1.Rows[i].DefaultCellStyle.BackColor = Color.Pink;
                    }
                }
            };

            Helper.Controls.Grid.Generator(this.grid1)
                .Text("id", header: "SP No", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("Article", header: "Article", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("SizeCode", header: "SizeCode", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("Colorid", header: "Color", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("Patternpanel", header: "Comb", width: Widths.AnsiChars(2), iseditingreadonly: true)
                .Numeric("Qty", header: "Prd Qty", width: Widths.AnsiChars(7), iseditingreadonly: true)
                .Numeric("CutQty", header: "Cut Qty", width: Widths.AnsiChars(7), iseditingreadonly: true)
                .Numeric("Variance", header: "Variance", width: Widths.AnsiChars(7), iseditingreadonly: true);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
