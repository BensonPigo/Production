using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Shipping
{
    public partial class P12 : Sci.Win.Tems.QueryForm
    {
        public P12(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.EditMode = true;
        }

        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.grid1.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.grid1)
                .CheckBox("selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
                .Text("FactoryID", header: "Factory", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("BrandID", header: "Brand", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Date("BuyerDelivery", header: "Buyer Delivery", iseditingreadonly: true)
                .Text("OrderID", header: "SP#", width: Widths.AnsiChars(16), iseditingreadonly: true)
                .Text("CustPONo", header: "PO#", width: Widths.AnsiChars(16), iseditingreadonly: true)
                .Text("StyleID", header: "Style#", width: Widths.AnsiChars(16), iseditingreadonly: true)
                .Text("SeasonID", header: "Season", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Numeric("Qty", header: "Order Qty", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Numeric("ChargeableQty", header: "Chargeable Qty", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Numeric("FOCQty", header: "FOC Qty", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Numeric("ChargeablePulloutQty", header: "Chargeable Pullout Qty", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Numeric("FOCPulloutQty", header: "FOC Pullout Qty", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Numeric("FinishedFOCStockinQty", header: "Finished FOC Stock-in Qty", width: Widths.AnsiChars(5), iseditingreadonly: true)
                ;
        }

        private void Find()
        {
            #region 必輸入條件
            if (MyUtility.Check.Empty(this.dateBuyerDelivery.Value1))
            {
                MyUtility.Msg.WarningBox("Please input <Buyer Delivery>!");
                return;
            }
            #endregion
            this.listControlBindingSource1.DataSource = null;
            #region where
            string where = string.Empty;
            if (!MyUtility.Check.Empty(this.txtSP1.Text))
            {
                where += $@" and o.id >= '{this.txtSP1.Text}' ";
            }

            if (!MyUtility.Check.Empty(this.txtSP2.Text))
            {
                where += $@" and o.id <= '{this.txtSP2.Text}' ";
            }

            if (!MyUtility.Check.Empty(this.txtbrand1.Text))
            {
                where += $@" and o.brandid = '{this.txtbrand1.Text}' ";
            }

            if (!MyUtility.Check.Empty(this.dateBuyerDelivery.Value1))
            {
                where += $@" and o.BuyerDelivery between '{((DateTime)this.dateBuyerDelivery.Value1).ToString("d")}' and '{((DateTime)this.dateBuyerDelivery.Value2).ToString("d")}' ";
            }

            if (!MyUtility.Check.Empty(this.txtfactory.Text))
            {
                where += $@" and o.FtyGroup ='{this.txtfactory.Text}'";
            }

            #endregion
            string sqlcmd = $@"
select
	selected = cast(0 as bit),
	o.FactoryID,
	o.BrandID,
	o.BuyerDelivery,
	OrderID=o.ID,
	o.CustPONo,
	o.StyleID,
	o.SeasonID,
	o.Qty,
	ChargeableQty =o.Qty-o.FOCQty,
	o.FOCQty,
	ChargeablePulloutQty = isnull(c.value,0),
	FOCPulloutQty = isnull(c.value2,0),
	FinishedFOCStockinQty =o.FOCQty - isnull(c.value2,0)
from orders o with(nolock)
inner join Factory f with(nolock) on f.id = o.FactoryID and f.IsProduceFty = 1
outer apply(
	select sum(value) as value , sum(value2) as value2 
	from
	(	
		select 
		value = iif(pl.Type <> 'F',sum(pod.ShipQty),0),
		value2=iif( pl.Type='F',sum(pod.ShipQty),0)
		from Pullout_Detail pod with(nolock)
		inner join PackingList pl with(nolock) on pl.ID = pod.PackingListID
		where pod.OrderID = o.ID
		group by pl.Type
	) a
)c
where o.Junk = 0
and not exists(select 1 from Order_Finish ox where ox.id = o.ID)
and o.FOCQty > isnull(c.value2,0)
and exists (
	select 1
	from Order_QtyShip_Detail oqd WITH (NOLOCK) 
	left join Order_UnitPrice ou1 WITH (NOLOCK) on ou1.Id = oqd.Id and ou1.Article = '----' and ou1.SizeCode = '----' 
	left join Order_UnitPrice ou2 WITH (NOLOCK) on ou2.Id = oqd.Id and ou2.Article = oqd.Article and ou2.SizeCode = oqd.SizeCode 
	where oqd.Id = o.id
	and isnull(ou2.POPrice,isnull(ou1.POPrice,-1)) = 0
)--有一筆Price為0表示此Orderid有Foc
and o.MDivisionID = '{Sci.Env.User.Keyword}'
{where}
order by o.ID
";
            DataTable dt;
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            if (dt.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return;
            }

            this.listControlBindingSource1.DataSource = dt;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (((DataTable)this.listControlBindingSource1.DataSource).Select("selected = 1").Count() == 0)
            {
                return;
            }

            DataTable dt = ((DataTable)this.listControlBindingSource1.DataSource).Select("selected = 1").CopyToDataTable();
            DataTable dt2 = dt.Copy();
            DataTable odt;
            DualResult result;

            string sqlchk = $@"
select t.OrderID
from #tmp t
inner join Order_Finish ox with(nolock) on ox.id = t.OrderID
";
            result = MyUtility.Tool.ProcessWithDatatable(dt, string.Empty, sqlchk, out odt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            if (odt.Rows.Count > 0)
            {
                var idList = odt.AsEnumerable().Select(s => MyUtility.Convert.GetString(s["OrderID"])).ToList();
                string msg = $@"SP# already extsis Finished FOC
SP# : {string.Join(",", idList)}";
                MyUtility.Msg.WarningBox(msg);
                if (dt.AsEnumerable().Where(w => !idList.Contains(MyUtility.Convert.GetString(w["OrderId"]))).Count() > 0)
                {
                    dt2 = dt.AsEnumerable().Where(w => !idList.Contains(MyUtility.Convert.GetString(w["OrderId"]))).CopyToDataTable();
                }

                if (dt.AsEnumerable().Where(w => !idList.Contains(MyUtility.Convert.GetString(w["OrderId"]))).Count() == 0)
                {
                    dt2.Clear();
                }
            }

            if (dt2.Rows.Count > 0)
            {
                string insertOrderFinished = $@"
insert Order_Finish(ID,FOCQty,CurrentFOCQty,AddName,AddDate)
select OrderID,FinishedFOCStockinQty,(FinishedFOCStockinQty -FOCPulloutQty) ,'{Sci.Env.User.UserID}',getdate()
from #tmp
";
                result = MyUtility.Tool.ProcessWithDatatable(dt2, string.Empty, insertOrderFinished, out odt);
                if (!result)
                {
                    this.ShowErr(result);
                    return;
                }
            }

            this.Find();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            this.Find();
        }
    }
}
