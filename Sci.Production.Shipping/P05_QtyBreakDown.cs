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

namespace Sci.Production.Shipping
{
    public partial class P05_QtyBreakDown : Sci.Win.Subs.Input6A
    {
        public P05_QtyBreakDown(DataRow MasterData)
        {
            InitializeComponent();
            displayBox1.Text = MasterData["ID"].ToString();
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            
            grid1.IsEditingReadOnly = true;
            Helper.Controls.Grid.Generator(this.grid1)
                .Text("OrderID", header: "SP No.", width: Widths.AnsiChars(13))
                .Text("Article", header: "Color Way", width: Widths.AnsiChars(8))
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(5))
                .Numeric("OrderQty", header: "Order Q'ty")
                .Numeric("ShipQty", header: "Q'ty")
                .Numeric("POPrice", header: "FOB", decimal_places: 2)
                .Numeric("Surcharge", header: "Surcharge", decimal_places: 4);
        }

        protected override void OnAttached(DataRow data)
        {
            base.OnAttached(data);
            string sqlCmd = string.Format(@"select a.OrderID,a.OrderShipmodeSeq,a.Article,a.SizeCode,oqd.Qty as OrderQty,a.ShipQty,isnull(ou2.POPrice,ou1.POPrice) as POPrice,
(select round(sum(iif(os.PriceType = '1',(os.Price/o.Qty),os.Price)),4)
from Order_Surcharge os, Orders o
where os.Id = o.ID
and o.ID = a.OrderID) as Surcharge
from (select pd.OrderID,pd.OrderShipmodeSeq,pd.Article,pd.SizeCode,sum(pd.ShipQty) as ShipQty
      from PackingList_Detail pd
      where pd.ID = '{0}'
      group by pd.OrderID,pd.OrderShipmodeSeq,pd.Article,pd.SizeCode) a
left join Order_UnitPrice ou1 on ou1.Id = a.OrderID and ou1.Article = '----' and ou1.SizeCode = '----'
left join Order_UnitPrice ou2 on ou2.Id = a.OrderID and ou2.Article = a.Article and ou2.SizeCode = a.SizeCode
left join Order_QtyShip_Detail oqd on oqd.Id = a.OrderID and oqd.Seq = a.OrderShipmodeSeq and oqd.Article = a.Article and oqd.SizeCode = a.SizeCode
left join Orders o on o.ID = a.OrderID
left join Order_Article oa on oa.ID = o.POID and oa.Article = a.Article
left join Order_SizeCode os on os.ID = o.POID and os.SizeCode = a.SizeCode
order by a.OrderID,a.OrderShipmodeSeq,oa.Seq,os.Seq", displayBox2.Text);
            DataTable gridData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out gridData);
            if (!result)
            {
                MyUtility.Msg.ErrorBox(result.ToString());
            }

            listControlBindingSource1.DataSource = gridData;
        }
    }
}
