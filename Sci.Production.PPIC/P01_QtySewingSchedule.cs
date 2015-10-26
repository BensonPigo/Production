using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;

namespace Sci.Production.PPIC
{
    public partial class P01_QtySewingSchedule : Sci.Win.Subs.Base
    {
        private string orderID, styleUkey;
        public P01_QtySewingSchedule(string OrderID, string StyleUkey)
        {
            InitializeComponent();
            orderID = OrderID;
            styleUkey = StyleUkey;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            DataTable GridData,LocationData;
            string sqlCmd = string.Format(@"with SewingData
as
(
select sd.ComboType,sd.Article,sd.SizeCode,isnull(sum(sd.AlloQty),0) as AlloQty,isnull(oq.Qty,0) as Qty,oa.Seq as ArticleSeq,os.Seq as SizeCoseSeq
,(select SewingLineID+',' from SewingSchedule_Detail where OrderID = sd.OrderID and Article = sd.Article and SizeCode = sd.SizeCode for xml path('')) as Line
from SewingSchedule_Detail sd
left join Orders o on sd.OrderID = o.ID
left join Order_Qty oq on sd.OrderID = oq.ID and sd.Article = oq.Article and sd.SizeCode = oq.SizeCode
left join Order_Article oa on sd.OrderID = oa.id and sd.Article = oa.Article
left join Order_SizeCode os on sd.OrderID = os.id and sd.SizeCode = os.SizeCode
where sd.OrderID = '{0}'
group by sd.OrderID,sd.ComboType,sd.Article,sd.SizeCode,oq.Qty,oa.Seq,os.Seq
)
select *
from (select ComboType,Article,SizeCode,Qty,AlloQty,LEFT(Line,LEN(Line)-1) as Line,ArticleSeq,SizeCoseSeq from SewingData) b
pivot (sum(AlloQty) for combotype in ([T],[B],[I],[O])) a
order by ArticleSeq,SizeCoseSeq",orderID);
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out GridData);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query data fail!!"+result.ToString());
            }
            listControlBindingSource1.DataSource = GridData;

            Ict.Win.UI.DataGridViewNumericBoxColumn col_t;
            Ict.Win.UI.DataGridViewNumericBoxColumn col_b;
            Ict.Win.UI.DataGridViewNumericBoxColumn col_i;
            Ict.Win.UI.DataGridViewNumericBoxColumn col_o;

            //設定Grid1的顯示欄位
            this.grid1.IsEditingReadOnly = true;
            this.grid1.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.grid1)
                .Text("Article", header: "Color Way", width: Widths.AnsiChars(8))
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(8))
                .Numeric("Qty", header: "Order Q'ty", width: Widths.AnsiChars(6))
                .Numeric("T", header: "Schedule Q'ty (T)", width: Widths.AnsiChars(6)).Get(out col_t)
                .Numeric("B", header: "Schedule Q'ty (B)", width: Widths.AnsiChars(6)).Get(out col_b)
                .Numeric("I", header: "Schedule Q'ty (I)", width: Widths.AnsiChars(6)).Get(out col_i)
                .Numeric("O", header: "Schedule Q'ty (O)", width: Widths.AnsiChars(6)).Get(out col_o)
                .Text("Line", header: "Sewing Line", width: Widths.AnsiChars(20));

            sqlCmd = string.Format("select Location from Style_Location where StyleUkey = {0}", styleUkey);
            result = DBProxy.Current.Select(null, sqlCmd, out LocationData);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query location fail!!" + result.ToString());
            }

            col_t.Visible = false;
            col_b.Visible = false;
            col_i.Visible = false;
            col_o.Visible = false;
            foreach (DataRow dr in LocationData.Rows)
            {
                switch (dr["Location"].ToString())
                {
                    case "T":
                        col_t.Visible = true;
                        break;

                    case "B":
                        col_b.Visible = true;
                        break;

                    case "I":
                        col_i.Visible = true;
                        break;

                    case "O":
                        col_o.Visible = true;
                        break;

                    default:
                        break;
                }
            }
        }
    }
}
