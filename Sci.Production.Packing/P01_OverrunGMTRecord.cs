using System.Data;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.Packing
{
    /// <summary>
    /// Packing_P01_OverrunGMTRecord
    /// </summary>
    public partial class P01_OverrunGMTRecord : Win.Subs.Base
    {
        private string orderID;

        /// <summary>
        /// P01_OverrunGMTRecord
        /// </summary>
        /// <param name="orderID">OrderID</param>
        public P01_OverrunGMTRecord(string orderID)
        {
            this.InitializeComponent();
            this.orderID = orderID;
        }

        /// <summary>
        /// OnFormLoaded
        /// </summary>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            DataTable overrunData;
            string sqlCmd = string.Format(
                @"select od.*,pr.Description
from OverrunGMT_Detail od WITH (NOLOCK) 
left join Orders o WITH (NOLOCK) on od.ID = o.ID
left join Order_Article oa WITH (NOLOCK) on oa.id = od.ID and oa.Article = od.Article
left join Order_SizeCode os WITH (NOLOCK) on o.POID = os.Id and od.SizeCode = os.SizeCode
left join PackingReason pr WITH (NOLOCK) on pr.Type = 'OG' and pr.ID = od.PackingReasonID
where od.ID = '{0}'
order by oa.Seq,os.Seq", this.orderID);
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out overrunData);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query overrun data fail!!" + result.ToString());
            }

            this.listControlBindingSource1.DataSource = overrunData;

            // 設定Grid的顯示欄位
            this.gridOverrunGarmentRecord.IsEditingReadOnly = true;
            this.gridOverrunGarmentRecord.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridOverrunGarmentRecord)
                .Text("Article", header: "Article", width: Widths.AnsiChars(8))
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(8))
                .Numeric("Qty", header: "Q'ty", width: Widths.AnsiChars(6))
                .Text("PackingReasonID", header: "Reason Id", width: Widths.AnsiChars(5))
                .EditText("Description", header: "Reason Description", width: Widths.AnsiChars(45));
        }
    }
}
