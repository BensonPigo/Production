using System;
using System.Collections.Generic;
using System.Data;
using Ict.Win;
using Ict;
using Sci.Data;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// P11_BatchImport
    /// </summary>
    public partial class P11_BatchApprove : Win.Subs.Base
    {
        private Action reloadParant;
        private DataTable dt;
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;

        /// <summary>
        /// P11_BatchImport
        /// </summary>
        /// <param name="reloadParant">Action</param>
        public P11_BatchApprove(Action reloadParant)
        {
            this.InitializeComponent();
            this.reloadParant = reloadParant;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.grid1.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.grid1)
             .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
            .Text("InvSerial", header: "Invoice Serial", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Numeric("KGS", header: "TTL GRS WEIGHT (KGS)", width: Widths.AnsiChars(15), decimal_places: 2, iseditingreadonly: true)
            .Numeric("qty", header: "TTL QTY", width: Widths.AnsiChars(8), decimal_places: 2, iseditingreadonly: true)
            .Text("NameEN", header: "COUNTRY OF DESTINATION", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("BIRShipTo", header: "Ship To", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Numeric("fob", header: "FOB Value", width: Widths.AnsiChars(15), decimal_places: 2, iseditingreadonly: true)
            .Numeric("material", header: "Material Value", width: Widths.AnsiChars(15), decimal_places: 2, iseditingreadonly: true)
            .Numeric("cmp", header: "TTL CMP VALUE", width: Widths.AnsiChars(10), decimal_places: 2, iseditingreadonly: true)
            ;
            this.Query();
        }

        private void Query()
        {
            string sqlcmd = $@"
select 
	b.id,
	p.INVNo,
	KGS=p.gw,
	F=sum(pd.ShipQty),
	c.NameEN,
	I=sum(pd.ShipQty)*round(o.PoPrice,2),
	M=sum(pd.ShipQty)*Round((((isnull(o.CPU,0) + isnull(s1.Price,0)) * isnull(isnull(f1.CpuCost,f.CpuCost),0)) + isnull(s2.Price,0) + isnull(s3.Price,0)), 3),
	BIRShipTo=SUBSTRING(ccd.BIRShipTo,0,CHARINDEX(char(13),ccd.BIRShipTo))
into #tmp
from orders o with(nolock)
inner join PackingList_Detail pd with(nolock) on pd.OrderID = o.id
inner join PackingList p with(nolock) on p.id = pd.id
inner join GMTBooking g with(nolock) on p.INVNo =g.id
inner join BIRInvoice b with(nolock)on g.BIRID = b.id
left join Style s with(nolock) on s.Ukey = o.StyleUkey
left join country c with(nolock) on c.id = o.Dest
left join CustCD ccd with(nolock) on ccd.BrandID = o.BrandID and ccd.id = o.CustCDID
outer apply(
	select fd.CpuCost
	from FtyShipper_Detail fsd WITH (NOLOCK) , FSRCpuCost_Detail fd WITH (NOLOCK) 
	where fsd.BrandID = o.BrandID
	and fsd.FactoryID = o.FactoryID
	and o.OrigBuyerDelivery between fsd.BeginDate and fsd.EndDate
	and fsd.ShipperID = fd.ShipperID
	and o.OrigBuyerDelivery between fd.BeginDate and fd.EndDate
	and o.OrigBuyerDelivery is not null
    and fsd.seasonID = o.seasonID
)f1
outer apply(
	select fd.CpuCost
	from FtyShipper_Detail fsd WITH (NOLOCK) , FSRCpuCost_Detail fd WITH (NOLOCK) 
	where fsd.BrandID = o.BrandID
	and fsd.FactoryID = o.FactoryID
	and o.OrigBuyerDelivery between fsd.BeginDate and fsd.EndDate
	and fsd.ShipperID = fd.ShipperID
	and o.OrigBuyerDelivery between fd.BeginDate and fd.EndDate
	and o.OrigBuyerDelivery is not null
    and fsd.seasonID = ''
)f
outer apply(
	select Price = sum(ot.Price)
	from Order_TmsCost ot WITH (NOLOCK) 
	left join ArtworkType a WITH (NOLOCK) on ot.ArtworkTypeID = a.ID
	where ot.ID = o.id and a.Classify = 'I'and a.IsTtlTMS <> 1
)s1
outer apply(
	select Price = sum(ot.Price)
	from Order_TmsCost ot WITH (NOLOCK) 
	left join ArtworkType a WITH (NOLOCK) on ot.ArtworkTypeID = a.ID
	where ot.ID = o.id and a.Classify = 'A'
)s2
outer apply(
	select dbo.GetLocalPurchaseStdCost(o.id) price
)s3
where b.Status = 'New'
group by b.id,p.INVNo,p.gw,c.NameEN,o.CPU,s1.Price,s2.Price,s3.price,isnull(isnull(f1.CpuCost,f.CpuCost),0),ccd.BIRShipTo,o.PoPrice


select selected = 0,b.id,b.InvSerial,KGS=round(sum(t.KGS),2),qty=round(sum(t.F),2),t.NameEN,BIRShipTo,fob=round(sum(t.I),2),material=round(sum(t.I),2)-round(sum(t.M),2),cmp=round(sum(t.M),2),b.brandid
from BIRInvoice b with(nolock)
inner join GMTBooking g with(nolock)on g.BIRID = b.id
inner join #tmp t on t.id = b.id
where b.Status = 'New'
group by b.id,b.InvSerial,t.NameEN,b.brandid,BIRShipTo
order by b.id
drop table #tmp
";
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out this.dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.listControlBindingSource1.DataSource = this.dt;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            this.Query();
        }

        private void Btnclose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnApprove_Click(object sender, EventArgs e)
        {
            this.grid1.ValidateControl();
            DataRow[] selectDrs = this.dt.Select("Selected=1");
            if (selectDrs.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select record first! ");
                return;
            }

            List<string> ids = new List<string>();
            foreach (DataRow dr in selectDrs)
            {
                ids.Add("'" + dr["id"] + "'");
            }

            string sqlupdate = $@"
update BIRInvoice set Status='Approved', Approve='{Env.User.UserID}', ApproveDate=getdate(), EditName='{Env.User.UserID}', EditDate=getdate()
where id in({string.Join(",", ids)})
";
            DualResult result = DBProxy.Current.Execute(null, sqlupdate);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.Query();
            this.reloadParant();
        }

        private void BtnExcel_Click(object sender, EventArgs e)
        {
            this.grid1.ValidateControl();
            DataTable dtex = this.dt.Select("Selected=1").CopyToDataTable();
            dtex.Columns.Remove("id");
            dtex.Columns.Remove("Selected");
            dtex.Columns.Remove("BrandID");
            bool result;
            result = MyUtility.Excel.CopyToXls(dtex, string.Empty, xltfile: "Shipping_P02_BatchApprove.xltx", headerRow: 1, showSaveMsg: false);
            if (!result)
            {
                MyUtility.Msg.WarningBox(result.ToString(), "Warning");
            }
        }
    }
}