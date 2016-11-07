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
            Select a.* ,b.cutqty,a.qty - b.cutqty as Variance 
            from a 
            left join b on a.id = b.orderid and a.Article = b.Article and a.PatternPanel = b.PatternPanel and a.SizeCode = b.SizeCode  
            union all 
            Select x.poid,y.ID,y.Article,y.SizeCode,y.qty,'' as Colorid,'=' as Patternpanel,null as cutqty,null as Variance 
            from (Select id,POID from Orders z where z.cuttingsp = '{0}') as x,order_Qty y 
            where y.id = x.id
            order by id,article ,sizecode,PatternPanel", cutid);
            DataTable gridtb;
            DualResult dr = DBProxy.Current.Select(null, sqlcmd, out gridtb);
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
