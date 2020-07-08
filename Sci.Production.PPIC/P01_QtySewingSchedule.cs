using System.Data;
using Ict.Win;
using Ict;
using Sci.Data;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// P01_QtySewingSchedule
    /// </summary>
    public partial class P01_QtySewingSchedule : Win.Subs.Base
    {
        private string orderID;
        private string styleUkey;

        /// <summary>
        /// P01_QtySewingSchedule
        /// </summary>
        /// <param name="orderID">string OrderID</param>
        /// <param name="styleUkey">string StyleUkey</param>
        public P01_QtySewingSchedule(string orderID, string styleUkey)
        {
            this.InitializeComponent();
            this.orderID = orderID;
            this.styleUkey = styleUkey;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            DataTable gridData, locationData;
            string sqlCmd = string.Format(
                @"with SewingData as
(
	select sd.ComboType,sd.Article,sd.SizeCode,isnull(sum(sd.AlloQty),0) as AlloQty,isnull(oq.Qty,0) as Qty,oa.Seq as ArticleSeq,os.Seq as SizeCoseSeq
	,Line = (select DISTINCT SewingLineID + ',' from SewingSchedule_Detail WITH (NOLOCK) where OrderID = sd.OrderID and Article = sd.Article and SizeCode = sd.SizeCode for xml path('')	) 
	from SewingSchedule_Detail sd WITH (NOLOCK) 
	left join Orders o WITH (NOLOCK) on sd.OrderID = o.ID
	left join Order_Qty oq WITH (NOLOCK) on sd.OrderID = oq.ID and sd.Article = oq.Article and sd.SizeCode = oq.SizeCode
	left join Order_Article oa WITH (NOLOCK) on sd.OrderID = oa.id and sd.Article = oa.Article
	left join Order_SizeCode os WITH (NOLOCK) on sd.OrderID = os.id and sd.SizeCode = os.SizeCode
    where sd.OrderID = '{0}'
    group by sd.OrderID,sd.ComboType,sd.Article,sd.SizeCode,oq.Qty,oa.Seq,os.Seq
)
select *
from (select ComboType,Article,SizeCode,Qty,AlloQty,LEFT(Line,LEN(Line)-1) as Line,ArticleSeq,SizeCoseSeq from SewingData) b
pivot (sum(AlloQty) for combotype in ([T],[B],[I],[O])) a
order by ArticleSeq,SizeCoseSeq", this.orderID);
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out gridData);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query data fail!!" + result.ToString());
            }

            this.listControlBindingSource1.DataSource = gridData;

            Ict.Win.UI.DataGridViewNumericBoxColumn col_t;
            Ict.Win.UI.DataGridViewNumericBoxColumn col_b;
            Ict.Win.UI.DataGridViewNumericBoxColumn col_i;
            Ict.Win.UI.DataGridViewNumericBoxColumn col_o;

            // 設定Grid1的顯示欄位
            this.gridSewingSchedule.IsEditingReadOnly = true;
            this.gridSewingSchedule.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridSewingSchedule)
                .Text("Article", header: "Color Way", width: Widths.AnsiChars(8))
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(8))
                .Numeric("Qty", header: "Order Q'ty", width: Widths.AnsiChars(6))
                .Numeric("T", header: "Schedule Q'ty (T)", width: Widths.AnsiChars(6)).Get(out col_t)
                .Numeric("B", header: "Schedule Q'ty (B)", width: Widths.AnsiChars(6)).Get(out col_b)
                .Numeric("I", header: "Schedule Q'ty (I)", width: Widths.AnsiChars(6)).Get(out col_i)
                .Numeric("O", header: "Schedule Q'ty (O)", width: Widths.AnsiChars(6)).Get(out col_o)
                .Text("Line", header: "Sewing Line", width: Widths.AnsiChars(20));

            sqlCmd = string.Format("select Location from Style_Location WITH (NOLOCK) where StyleUkey = {0}", this.styleUkey);
            result = DBProxy.Current.Select(null, sqlCmd, out locationData);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query location fail!!" + result.ToString());
            }

            col_t.Visible = false;
            col_b.Visible = false;
            col_i.Visible = false;
            col_o.Visible = false;
            foreach (DataRow dr in locationData.Rows)
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
