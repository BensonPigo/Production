using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.Packing
{
    public partial class P01_OverrunGMTRecord : Sci.Win.Subs.Base
    {
        private string orderID;
        public P01_OverrunGMTRecord(string OrderID)
        {
            InitializeComponent();
            orderID = OrderID;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            DataTable OverrunData;
            string sqlCmd = string.Format(@"select od.*,pr.Description
from OverrunGMT_Detail od WITH (NOLOCK) 
left join Orders o WITH (NOLOCK) on od.ID = o.ID
left join Order_Article oa WITH (NOLOCK) on oa.id = od.ID and oa.Article = od.Article
left join Order_SizeCode os WITH (NOLOCK) on o.POID = os.Id and od.SizeCode = os.SizeCode
left join PackingReason pr WITH (NOLOCK) on pr.Type = 'OG' and pr.ID = od.PackingReasonID
where od.ID = '{0}'
order by oa.Seq,os.Seq", orderID);
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out OverrunData);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query overrun data fail!!"+result.ToString());
            }

            listControlBindingSource1.DataSource = OverrunData;
            //設定Grid的顯示欄位
            this.grid1.IsEditingReadOnly = true;
            this.grid1.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.grid1)
                .Text("Article", header: "Article", width: Widths.AnsiChars(8))
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(8))
                .Numeric("Qty", header: "Q'ty", width: Widths.AnsiChars(6))
                .Text("PackingReasonID", header: "Reason Id", width: Widths.AnsiChars(5))
                .EditText("Description", header: "Reason Description", width: Widths.AnsiChars(45));
        }
    }
}
