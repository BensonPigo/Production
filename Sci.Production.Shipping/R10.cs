using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Shipping
{
    /// <inheritdoc/>
    public partial class R10 : Win.Tems.PrintForm
    {
        private DateTime? date1;
        private DateTime? date2;
        private DateTime? apApvDate1;
        private DateTime? apApvDate2;
        private DateTime? onBoardDate1;
        private DateTime? onBoardDate2;
        private DateTime? voucherDate1;
        private DateTime? voucherDate2;
        private string brand;
        private string custcd;
        private string dest;
        private string shipmode;
        private string forwarder;
        private string rateType;
        private int reportContent;
        private int reportType;
        private DataTable printData;
        private DataTable[] printDataS;
        private DataTable accnoData;

        /// <inheritdoc/>
        public R10(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.radioGarment.Checked = true;
            this.radioExportFeeReport.Checked = true;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            MyUtility.Tool.SetupCombox(this.comboRateType, 2, 1, ",Original currency,FX,Fixed exchange rate,KP,KPI exchange rate");
            this.txtshipmode.SelectedIndex = -1;
        }

        private void RadioGarment_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioGarment.Checked)
            {
                this.labelPulloutDate.Text = "Pullout Date";
                this.txtbrand.Enabled = true;
                this.txtcustcd.Enabled = true;
                this.radioDetailListbySPNo.Text = "Detail List by SP#";
                this.radioDetailListBySPNoByFeeType.Text = "Detail List by SP# by Fee Type";
                this.radioAirPrepaidExpenseReport.Enabled = true;
                this.dateOnBoardDate.Enabled = true;
            }
        }

        private void RadioRowMaterial_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioRowMaterial.Checked)
            {
                this.labelPulloutDate.Text = "Ship Date";
                this.txtbrand.Enabled = false;
                this.txtcustcd.Enabled = false;
                this.radioDetailListbySPNo.Text = "Detail List by WK#";
                this.radioDetailListBySPNoByFeeType.Text = "Detail List by WK# by Fee Type";
                this.radioAirPrepaidExpenseReport.Enabled = false;
                this.dateOnBoardDate.Enabled = false;

                if (this.radioAirPrepaidExpenseReport.Checked)
                {
                    this.radioExportFeeReport.Checked = true;
                }
            }
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.date1 = this.datePulloutDate.Value1;
            this.date2 = this.datePulloutDate.Value2;
            this.apApvDate1 = this.dateAPApvDate.Value1;
            this.apApvDate2 = this.dateAPApvDate.Value2;
            this.onBoardDate1 = this.dateOnBoardDate.Value1;
            this.onBoardDate2 = this.dateOnBoardDate.Value2;
            this.voucherDate1 = this.dateVoucherDate.Value1;
            this.voucherDate2 = this.dateVoucherDate.Value2;
            this.brand = this.txtbrand.Text;
            this.custcd = this.txtcustcd.Text;
            this.dest = this.txtcountryDestination.TextBox1.Text;
            this.shipmode = this.txtshipmode.Text;
            this.forwarder = this.txtsubconForwarder.TextBox1.Text;
            this.rateType = this.comboRateType.SelectedValue.ToString();
            this.reportContent = this.radioGarment.Checked ? 1 : 2;

            if (this.radioExportFeeReport.Checked)
            {
                this.reportType = 1;
            }
            else if (this.radioDetailListbySPNo.Checked)
            {
                this.reportType = 2;
            }
            else if (this.radioDetailListBySPNoByFeeType.Checked)
            {
                this.reportType = 3;
            }
            else if (this.radioAirPrepaidExpenseReport.Checked)
            {
                this.reportType = 4;
            }
            else if (this.radioExportFeeReportMerged.Checked)
            {
                this.reportType = 5;
            }

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            string queryAccount;
            DualResult result;

            if (this.reportType == 5)
            {
                bool b = this.ReportType5();
                if (!b)
                {
                    return new DualResult(false);
                }
                else
                {
                    return Ict.Result.True;
                }
            }

            // Garment
            if (this.reportContent == 1)
            {
                // Export Fee Report or Detail List by SP#
                if (this.reportType == 1 || this.reportType == 2)
                {
                    // Export Fee Report
                    if (this.reportType == 1)
                    {
                        #region 組SQL
                        sqlCmd.Append($@"
with tmpGB 
as (
	select distinct [Type] = 'GARMENT'
		, g.ID
		, [OnBoardDate] = g.ETD 
		, g.Shipper
		, [Foundry] = iif(ISNULL(gm.Foundry,'') = '', '' , 'Y')
		, s.SisFtyAPID
		, g.BrandID
		, [Category] = case when o.Category = 'B' then 'Bulk'
							  when o.Category = 'S' then 'Sample'
							  when o.Category = 'G' then 'Garment'
							  else '' end
		, g.CustCDID
		, g.Dest
		, g.ShipModeID
		, p.PulloutDate
		, [Forwarder] = g.Forwarder+'-'+isnull(ls.Abb,'') 
		, s.BLNo
		, se.CurrencyID
		, p.OrderID
		, [packingID] = p.ID
        , se.AccountID
        , [Amount] = se.Amount * iif('{this.rateType}' = '', 1, dbo.getRate('{this.rateType}', s.CurrencyID,'USD', s.CDate))
    from ShippingAP s WITH (NOLOCK) 
    inner join ShareExpense se WITH (NOLOCK) on se.ShippingAPID = s.ID and se.Junk = 0
    inner join GMTBooking g WITH (NOLOCK) on g.ID = se.InvNo
    inner join PackingList p WITH (NOLOCK) on p.INVNo = g.ID
    inner join (
		select distinct id,OrderID,OrderShipmodeSeq 
		from PackingList_Detail	pd	
	) pd on pd.id = p.id
    inner join Orders o WITH (NOLOCK) on o.ID = pd.OrderID
    inner join LocalSupp ls WITH (NOLOCK) on ls.ID = g.Forwarder 
	outer apply (
		select top 1 Foundry 
		from GMTBooking WITH (NOLOCK) 
		where ISNULL(s.BLNo,'') != '' 
　　	  and (BLNo = s.BLNo or BL2No = s.BLNo) 
　　	  and Foundry = 1
	)gm
    where s.Type = 'EXPORT'
");
                        if (!MyUtility.Check.Empty(this.date1))
                        {
                            sqlCmd.Append(string.Format(" and p.PulloutDate >= '{0}'", Convert.ToDateTime(this.date1).ToString("d")));
                        }

                        if (!MyUtility.Check.Empty(this.date2))
                        {
                            sqlCmd.Append(string.Format(" and p.PulloutDate <= '{0}'", Convert.ToDateTime(this.date2).ToString("d")));
                        }

                        if (!MyUtility.Check.Empty(this.apApvDate1))
                        {
                            sqlCmd.Append(string.Format(" and CONVERT(DATE,s.ApvDate) >= '{0}'", Convert.ToDateTime(this.apApvDate1).ToString("d")));
                        }

                        if (!MyUtility.Check.Empty(this.apApvDate2))
                        {
                            sqlCmd.Append(string.Format(" and CONVERT(DATE,s.ApvDate) <= '{0}'", Convert.ToDateTime(this.apApvDate2).ToString("d")));
                        }

                        if (!MyUtility.Check.Empty(this.onBoardDate1))
                        {
                            sqlCmd.Append(string.Format(" and CONVERT(DATE,g.ETD) >= '{0}'", Convert.ToDateTime(this.onBoardDate1).ToString("d")));
                        }

                        if (!MyUtility.Check.Empty(this.onBoardDate2))
                        {
                            sqlCmd.Append(string.Format(" and CONVERT(DATE,g.ETD) <= '{0}'", Convert.ToDateTime(this.onBoardDate2).ToString("d")));
                        }

                        if (!MyUtility.Check.Empty(this.voucherDate1))
                        {
                            sqlCmd.Append(string.Format(" and s.VoucherDate >= '{0}'", Convert.ToDateTime(this.voucherDate1).ToString("d")));
                        }

                        if (!MyUtility.Check.Empty(this.voucherDate2))
                        {
                            sqlCmd.Append(string.Format(" and s.VoucherDate <= '{0}'", Convert.ToDateTime(this.voucherDate2).ToString("d")));
                        }

                        if (!MyUtility.Check.Empty(this.brand))
                        {
                            sqlCmd.Append(string.Format(" and g.BrandID = '{0}'", this.brand));
                        }

                        if (!MyUtility.Check.Empty(this.custcd))
                        {
                            sqlCmd.Append(string.Format(" and g.CustCDID = '{0}'", this.custcd));
                        }

                        if (!MyUtility.Check.Empty(this.dest))
                        {
                            sqlCmd.Append(string.Format(" and g.Dest = '{0}'", this.dest));
                        }

                        if (!MyUtility.Check.Empty(this.shipmode))
                        {
                            sqlCmd.Append(string.Format(" and g.ShipModeID = '{0}'", this.shipmode));
                        }

                        if (!MyUtility.Check.Empty(this.forwarder))
                        {
                            sqlCmd.Append(string.Format(" and g.Forwarder = '{0}'", this.forwarder));
                        }

                        sqlCmd.Append($@"
),
tmpPL as 
(
	select distinct [Type] = 'GARMENT'
		, p.ID
		, [OnBoardDate] = null
		, [Shipper] = ''  
		, [Foundry] = iif(ISNULL(gm.Foundry,'') = '', '' , 'Y')
		, s.SisFtyAPID
		, o.BrandID
		, [Category] = case when o.Category = 'B' then 'Bulk'
							  when o.Category = 'S' then 'Sample'
							  when o.Category = 'G' then 'Garment'
							  else '' end
		, o.CustCDID
		, o.Dest
		, p.ShipModeID
		, p.PulloutDate
		, [Forwarder] = ''
		, s.BLNo
		, se.CurrencyID
		, p.OrderID
		, [packingID] = p.ID
        , se.AccountID
        , [Amount] = se.Amount * iif('{this.rateType}' = '', 1, dbo.getRate('{this.rateType}', s.CurrencyID,'USD', s.CDate))
    from ShippingAP s WITH (NOLOCK) 
    inner join ShareExpense se WITH (NOLOCK) on se.ShippingAPID = s.ID and se.Junk = 0
    inner join PackingList p WITH (NOLOCK) on p.ID = se.InvNo
    inner join (
		select distinct id,OrderID,OrderShipmodeSeq 
		from PackingList_Detail	pd WITH (NOLOCK) 
	) pd on pd.id = p.id
    inner join Orders o WITH (NOLOCK) on o.ID = pd.OrderID
    inner join SciFMS_AccountNo a WITH (NOLOCK)  on a.ID = se.AccountID
	outer apply (
		select top 1 Foundry 
		from GMTBooking WITH (NOLOCK) 
		where ISNULL(s.BLNo,'') != '' 
　　	  and (BLNo = s.BLNo or BL2No = s.BLNo) 
　　	  and Foundry = 1
	)gm
    where s.Type = 'EXPORT'
");
                        if (!MyUtility.Check.Empty(this.date1))
                        {
                            sqlCmd.Append(string.Format(" and p.PulloutDate >= '{0}'", Convert.ToDateTime(this.date1).ToString("d")));
                        }

                        if (!MyUtility.Check.Empty(this.date2))
                        {
                            sqlCmd.Append(string.Format(" and p.PulloutDate <= '{0}'", Convert.ToDateTime(this.date2).ToString("d")));
                        }

                        if (!MyUtility.Check.Empty(this.apApvDate1))
                        {
                            sqlCmd.Append(string.Format(" and CONVERT(DATE,s.ApvDate) >= '{0}'", Convert.ToDateTime(this.apApvDate1).ToString("d")));
                        }

                        if (!MyUtility.Check.Empty(this.apApvDate2))
                        {
                            sqlCmd.Append(string.Format(" and CONVERT(DATE,s.ApvDate) <= '{0}'", Convert.ToDateTime(this.apApvDate2).ToString("d")));
                        }

                        if (!MyUtility.Check.Empty(this.voucherDate1))
                        {
                            sqlCmd.Append(string.Format(" and s.VoucherDate >= '{0}'", Convert.ToDateTime(this.voucherDate1).ToString("d")));
                        }

                        if (!MyUtility.Check.Empty(this.voucherDate2))
                        {
                            sqlCmd.Append(string.Format(" and s.VoucherDate <= '{0}'", Convert.ToDateTime(this.voucherDate2).ToString("d")));
                        }

                        if (!MyUtility.Check.Empty(this.brand))
                        {
                            sqlCmd.Append(string.Format(" and p.BrandID = '{0}'", this.brand));
                        }

                        if (!MyUtility.Check.Empty(this.custcd))
                        {
                            sqlCmd.Append(string.Format(" and o.CustCDID = '{0}'", this.custcd));
                        }

                        if (!MyUtility.Check.Empty(this.dest))
                        {
                            sqlCmd.Append(string.Format(" and o.Dest = '{0}'", this.dest));
                        }

                        if (!MyUtility.Check.Empty(this.shipmode))
                        {
                            sqlCmd.Append(string.Format(" and p.ShipModeID = '{0}'", this.shipmode));
                        }

                        #endregion
                    }

                    // Detail List by SP#
                    else
                    {
                        #region 組SQL
                        sqlCmd.Append($@"
with tmpGB 
as (
	select distinct [Type] = 'GARMENT'
		, g.ID
		, [OnBoardDate] = g.ETD 
		, g.Shipper
		, g.BrandID
		, [Category] = case when o.Category = 'B' then 'Bulk'
							  when o.Category = 'S' then 'Sample'
							  when o.Category = 'G' then 'Garment'
							  else '' end
		, pd.OrderID
		, oq.BuyerDelivery
		, g.CustCDID
		, g.Dest
		, g.ShipModeID
		, [packingid] = p.ID
		, p.PulloutID
		, p.PulloutDate
		, [Forwarder] = g.Forwarder+'-'+isnull(ls.Abb,'')
		, s.BLNo
		, se.CurrencyID
		, [Rate] = iif('{this.rateType}' = '', 1, dbo.getRate('{this.rateType}', s.CurrencyID,'USD', s.CDate))
		, se.AccountID
		, se.Amount
        , o.FactoryID
		, o.MDivisionID
		, f.KPICode
		, [Foundry] = iif(ISNULL(gm.Foundry,'') = '', '' , 'Y')
		, s.SisFtyAPID
    from ShippingAP s WITH (NOLOCK) 
    inner join ShareExpense se WITH (NOLOCK) on se.ShippingAPID = s.ID and se.Junk = 0
    inner join GMTBooking g WITH (NOLOCK) on g.ID = se.InvNo
    inner join PackingList p WITH (NOLOCK) on p.INVNo = g.ID
    inner join (
		select distinct id,OrderID,OrderShipmodeSeq 
		from PackingList_Detail	pd	
	) pd on pd.id = p.id
    inner join Orders o WITH (NOLOCK) on o.ID = pd.OrderID
    inner join Order_QtyShip oq WITH (NOLOCK) on pd.OrderID=oq.Id and oq.Seq = pd.OrderShipmodeSeq
    inner join LocalSupp ls WITH (NOLOCK) on ls.ID = g.Forwarder
    inner join Factory f with (nolock) on o.FactoryID = f.ID
	outer apply (
		select top 1 Foundry 
		from GMTBooking WITH (NOLOCK) 
		where ISNULL(s.BLNo,'') != '' 
　　	  and (BLNo = s.BLNo or BL2No = s.BLNo) 
　　	  and Foundry = 1
	)gm
    where s.Type = 'EXPORT'
");
                        if (!MyUtility.Check.Empty(this.date1))
                        {
                            sqlCmd.Append(string.Format(" and p.PulloutDate >= '{0}'", Convert.ToDateTime(this.date1).ToString("d")));
                        }

                        if (!MyUtility.Check.Empty(this.date2))
                        {
                            sqlCmd.Append(string.Format(" and p.PulloutDate <= '{0}'", Convert.ToDateTime(this.date2).ToString("d")));
                        }

                        if (!MyUtility.Check.Empty(this.apApvDate1))
                        {
                            sqlCmd.Append(string.Format(" and CONVERT(DATE,s.ApvDate) >= '{0}'", Convert.ToDateTime(this.apApvDate1).ToString("d")));
                        }

                        if (!MyUtility.Check.Empty(this.apApvDate2))
                        {
                            sqlCmd.Append(string.Format(" and CONVERT(DATE,s.ApvDate) <= '{0}'", Convert.ToDateTime(this.apApvDate2).ToString("d")));
                        }

                        if (!MyUtility.Check.Empty(this.onBoardDate1))
                        {
                            sqlCmd.Append(string.Format(" and CONVERT(DATE,g.ETD) >= '{0}'", Convert.ToDateTime(this.onBoardDate1).ToString("d")));
                        }

                        if (!MyUtility.Check.Empty(this.onBoardDate2))
                        {
                            sqlCmd.Append(string.Format(" and CONVERT(DATE,g.ETD) <= '{0}'", Convert.ToDateTime(this.onBoardDate2).ToString("d")));
                        }

                        if (!MyUtility.Check.Empty(this.voucherDate1))
                        {
                            sqlCmd.Append(string.Format(" and s.VoucherDate >= '{0}'", Convert.ToDateTime(this.voucherDate1).ToString("d")));
                        }

                        if (!MyUtility.Check.Empty(this.voucherDate2))
                        {
                            sqlCmd.Append(string.Format(" and s.VoucherDate <= '{0}'", Convert.ToDateTime(this.voucherDate2).ToString("d")));
                        }

                        if (!MyUtility.Check.Empty(this.brand))
                        {
                            sqlCmd.Append(string.Format(" and g.BrandID = '{0}'", this.brand));
                        }

                        if (!MyUtility.Check.Empty(this.custcd))
                        {
                            sqlCmd.Append(string.Format(" and g.CustCDID = '{0}'", this.custcd));
                        }

                        if (!MyUtility.Check.Empty(this.dest))
                        {
                            sqlCmd.Append(string.Format(" and g.Dest = '{0}'", this.dest));
                        }

                        if (!MyUtility.Check.Empty(this.shipmode))
                        {
                            sqlCmd.Append(string.Format(" and g.ShipModeID = '{0}'", this.shipmode));
                        }

                        if (!MyUtility.Check.Empty(this.forwarder))
                        {
                            sqlCmd.Append(string.Format(" and g.Forwarder = '{0}'", this.forwarder));
                        }

                        sqlCmd.Append($@"
),
tmpPL as 
(
	select distinct [Type] = 'GARMENT'
		, p.ID
		, [OnBoardDate] = null
		, [Shipper] = ''
		, o.BrandID
		, [Category] = case when o.Category = 'B' then 'Bulk'
						  when o.Category = 'S' then 'Sample'
						  when o.Category = 'G' then 'Garment'
						  else '' end
		, pd.OrderID
		, oq.BuyerDelivery
		, o.CustCDID
		, o.Dest
		, p.ShipModeID
		, [packingid] = p.ID
		, p.PulloutID
		, p.PulloutDate
		, [Forwarder] = ''
		, s.BLNo
		, se.CurrencyID
		, [Rate] = iif('{this.rateType}' = '', 1, dbo.getRate('{this.rateType}', s.CurrencyID,'USD', s.CDate))
		, se.AccountID
		, se.Amount
        , o.FactoryID
		, o.MDivisionID
		, f.KPICode
		, [Foundry] = iif(ISNULL(gm.Foundry,'') = '', '' , 'Y')
		, s.SisFtyAPID
    from ShippingAP s WITH (NOLOCK) 
    inner join ShareExpense se WITH (NOLOCK) on se.ShippingAPID = s.ID and se.Junk = 0
    inner join PackingList p WITH (NOLOCK) on p.ID = se.InvNo
    inner join (
		select distinct id,OrderID,OrderShipmodeSeq 
		from PackingList_Detail	pd	
	) pd on pd.id = p.id
    inner join Orders o WITH (NOLOCK) on o.ID = pd.OrderID
    inner join Order_QtyShip oq WITH (NOLOCK) on oq.Id = pd.OrderID 
    inner join SciFMS_AccountNo a  WITH (NOLOCK)  on a.ID = se.AccountID
    inner join Factory f with (nolock) on o.FactoryID = f.ID
	outer apply (
		select top 1 Foundry 
		from GMTBooking WITH (NOLOCK) 
		where ISNULL(s.BLNo,'') != '' 
　　	  and (BLNo = s.BLNo or BL2No = s.BLNo) 
　　	  and Foundry = 1
	)gm
    where s.Type = 'EXPORT'
");
                        if (!MyUtility.Check.Empty(this.date1))
                        {
                            sqlCmd.Append(string.Format(" and p.PulloutDate >= '{0}'", Convert.ToDateTime(this.date1).ToString("d")));
                        }

                        if (!MyUtility.Check.Empty(this.date2))
                        {
                            sqlCmd.Append(string.Format(" and p.PulloutDate <= '{0}'", Convert.ToDateTime(this.date2).ToString("d")));
                        }

                        if (!MyUtility.Check.Empty(this.apApvDate1))
                        {
                            sqlCmd.Append(string.Format(" and CONVERT(DATE,s.ApvDate) >= '{0}'", Convert.ToDateTime(this.apApvDate1).ToString("d")));
                        }

                        if (!MyUtility.Check.Empty(this.apApvDate2))
                        {
                            sqlCmd.Append(string.Format(" and CONVERT(DATE,s.ApvDate) <= '{0}'", Convert.ToDateTime(this.apApvDate2).ToString("d")));
                        }

                        if (!MyUtility.Check.Empty(this.voucherDate1))
                        {
                            sqlCmd.Append(string.Format(" and s.VoucherDate >= '{0}'", Convert.ToDateTime(this.voucherDate1).ToString("d")));
                        }

                        if (!MyUtility.Check.Empty(this.voucherDate2))
                        {
                            sqlCmd.Append(string.Format(" and s.VoucherDate <= '{0}'", Convert.ToDateTime(this.voucherDate2).ToString("d")));
                        }

                        if (!MyUtility.Check.Empty(this.brand))
                        {
                            sqlCmd.Append(string.Format(" and p.BrandID = '{0}'", this.brand));
                        }

                        if (!MyUtility.Check.Empty(this.custcd))
                        {
                            sqlCmd.Append(string.Format(" and o.CustCDID = '{0}'", this.custcd));
                        }

                        if (!MyUtility.Check.Empty(this.dest))
                        {
                            sqlCmd.Append(string.Format(" and o.Dest = '{0}'", this.dest));
                        }

                        if (!MyUtility.Check.Empty(this.shipmode))
                        {
                            sqlCmd.Append(string.Format(" and p.ShipModeID = '{0}'", this.shipmode));
                        }
                        #endregion

                    }

                    sqlCmd.Append(@"
),
tmpAllData
as (
select * from tmpGB
union all
select * from tmpPL
)

select * 
into #temp1
from tmpAllData");

                    // Export Fee Report
                    if (this.reportType == 1)
                    {
                        sqlCmd.Append(@"
-----temp2
select distinct [type]
	, id
	, OnBoardDate
	, shipper
	, Foundry = isnull(f.Foundry,'')
	, SisFtyAPID
	, Brandid
	, category
	, custcdid
	, dest
	, shipmodeid
	, pulloutdate
	, forwarder
	, s.BLNo
	, currencyid
	, orderid
	, packingid
	, AccountID
	, Amount
into #temp2
from #temp1 as a
outer apply (
		select [BLNo]= stuff(
			(select CONCAT(',',blno)
		from (
			select distinct blno 
			from #temp1 d
			where d.id=a.id 
			)s
			for xml path('')
		),1,1,'')
	)s
outer apply(select top 1 Foundry from #temp1 d where d.id=a.id and Foundry = 'Y')f


select [type]
	, id
	, OnBoardDate
	, Shipper
	, Foundry
	, SisFtyAPID = SisFtyAPID.value
	, BrandID
	, [Category] = Category.value
	, [OQty]=sum(oqs.OQty)
	, CustCDID
	, Dest
	, ShipModeID
	, PulloutDate
	, [ShipQty]=sum(pt.ShipQty)
	, [ctnqty]=sum(pt.ctnqty)
	, [gw]=sum(pt.gw)
	, [CBM]=sum(pt.CBM)
	, Forwarder
	, BLNo
	, CurrencyID
	, AccountID
	, Amount
into #temp3
from #temp2 a
outer apply(select sum(qty) as OQty from Order_QtyShip  WITH (NOLOCK) where id=a.OrderID) as oqs
outer apply(select sum(shipqty) as shipqty,sum(CTNQty)as CTNQty,sum(GW) as GW,sum(CBM)as CBM from PackingList WITH (NOLOCK)  where id=a.packingID) as pt
outer apply(
	select value = Stuff((
		select concat(',',Category)
		from (
				select 	distinct Category
				from #temp2 d
				where d.id = a.ID
			) s
		for xml path ('')
	) , 1, 1, '')
) Category
outer apply(
	select value = Stuff((
		select concat(',',SisFtyAPID)
		from (
				select 	distinct SisFtyAPID
				from #temp2 d
				where d.id = a.ID
                and d.SisFtyAPID <> ''
			) s
		for xml path ('')
	) , 1, 1, '')
) SisFtyAPID
group by type,id,OnBoardDate,Shipper,BrandID,Category.value,CustCDID,
Dest,ShipModeID,PulloutDate,Forwarder,BLNo,CurrencyID
,AccountID,Amount, Foundry, SisFtyAPID.value

select 
	a.[type]
	, a.id
	, a.OnBoardDate
	, a.Shipper
	, a.Foundry
	, a.SisFtyAPID
	, a.BrandID
	, a.[Category]
	, a.[OQty]
	, a.CustCDID
	, a.Dest
	, a.ShipModeID
	, a.PulloutDate
	, a.[ShipQty]
	, a.[ctnqty]
	, a.[gw]
	, a.[CBM]
	, a.Forwarder
	, a.BLNo
	, a.CurrencyID 
	, a.AccountID
	, a.Amount
into #temp4
from #temp3 a
");
                    }
                    else
                    {
                        sqlCmd.Append(@"
-----temp2 detail List by SP#
select distinct [type]
	,id
	,OnBoardDate
	,shipper
    ,FactoryID
	,MDivisionID
	,KPICode
	, Foundry = isnull(f.Foundry,'')
	,SisFtyAPID
	,Brandid
	,category
	,OrderID
	,BuyerDelivery
	,custcdid
	,dest
	,shipmodeid
	,packingid
	,PulloutID
	,PulloutDate
	,forwarder
	,s.BLNo
	,currencyid
	,Rate
	,AccountID
	,Amount
into #temp2
from #temp1 as a
outer apply (
		select [BLNo]= stuff(
			(select CONCAT(',',blno)
		from (
			select distinct blno 
			from #temp1 d
			where d.id=a.id 
			)s
			for xml path('')
		),1,1,'')
	)s
outer apply(select top 1 Foundry from #temp1 d where d.id=a.id and Foundry = 'Y' and d.OrderID = a.OrderID)f

--temp3 detail List by SP#
select [type]
	,id
	,OnBoardDate
	,Shipper
    ,FactoryID
	,MDivisionID
	,KPICode
	,Foundry
	,SisFtyAPID
	,BrandID
	,Category
	,orderID
	,BuyerDelivery
	,[OQty]=sum(oqs.OQty)
	,CustCDID
	,Dest
	,ShipModeID
	,packingid
	,PulloutID
	,PulloutDate
	,Rate
	,[ShipQty]=sum(pt.ShipQty)
	,[ctnqty]=sum(pt.ctnqty)
	,[gw]=sum(pt.gw)
	,[CBM]=sum(pt.CBM)
	,Forwarder
	,BLNo
	,CurrencyID 
	,AccountID
	,Amount
into #temp3
from #temp2 a
outer apply(select sum(qty) as OQty from Order_QtyShip WITH (NOLOCK)  where id=a.OrderID) as oqs
outer apply(select sum(shipqty) as shipqty,sum(CTNQty)as CTNQty,sum(GW) as GW,sum(CBM)as CBM from PackingList WITH (NOLOCK)  where id=a.packingID) as pt
group by type,id,OnBoardDate,Shipper,FactoryID,MDivisionID,KPICode,BrandID,Category,CustCDID,
Dest,ShipModeID,PulloutDate,Forwarder,BLNo,CurrencyID,orderID ,BuyerDelivery,packingid,PulloutID,Rate,AccountID,Amount,Foundry,SisFtyAPID

-- 取得TOTAL CBM, 用來計算比例
select a.id, sum(a.CBM) TotalCBM
into #tmpTotoalCBM
from 
(
	select distinct a.id, a.packingid, a.CBM
	from #temp3 a
	where exists (select 1 from ShipMode where ID = a.shipmodeID and ID not in ('A/C', 'A/P', 'A/P-C', 'E/C', 'E/P', 'E/P-C'))
)a 
group by a.id

-- 取得TOTAL GW, 用來計算比例
select a.id	,sum(a.gw) TotalGW
into #tmpTotoalGW
from
(
	select distinct a.id, a.packingid, a.gw
	from #temp3 a
	where a.shipmodeID in ('A/C', 'A/P', 'A/P-C', 'E/C', 'E/P', 'E/P-C')
)a
group by a.id

-- group by GB#+SP# 依TotalCBM, Total GW比例來取得對應的Amount值
select * 
into #temp4
from (
	select a.[type]
		,a.id
		,a.OnBoardDate
		,a.Shipper
        ,a.FactoryID
		,a.MDivisionID
		,a.KPICode
		,a.Foundry
		,a.SisFtyAPID
		,a.BrandID
		,a.Category
		,a.OrderID
		,a.BuyerDelivery
		,a.[OQty]
		,a.CustCDID
		,a.Dest
		,a.ShipModeID
		,a.packingid
		,a.PulloutID
		,a.PulloutDate	
		,a.[ShipQty]
		,a.[ctnqty]
		,a.[gw]
		,a.[CBM]
		,a.Forwarder
		,a.BLNo
		,a.CurrencyID 
		,a.AccountID
		,[Amount] = iif(c.TotalCBM is null or c.TotalCBM=0 or a.Rate=0,0, convert(float, round(a.Amount * a.CBM * a.Rate / C.TotalCBM, 2) ))
	from #temp3 a 
	LEFT JOIN #tmpTotoalCBM C ON A.ID=C.ID
union all
	select a.[type]
		,a.id
		,a.OnBoardDate
		,a.Shipper
        ,a.FactoryID
		,a.MDivisionID
		,a.KPICode
		,a.Foundry
		,a.SisFtyAPID
		,a.BrandID
		,a.Category
		,a.OrderID
		,a.BuyerDelivery
		,a.[OQty]
		,a.CustCDID
		,a.Dest
		,a.ShipModeID
		,a.packingid
		,a.PulloutID
		,a.PulloutDate	
		,a.[ShipQty]
		,a.[ctnqty]
		,a.[gw]
		,a.[CBM]
		,a.Forwarder
		,a.BLNo
		,a.CurrencyID
		,a.AccountID
		,[Amount] = iif(c.TotalGW is null or c.TotalGW=0 or a.Rate=0,0, convert(float, round(a.Amount * a.gw * a.Rate / C.TotalGW, 2) ))
	from #temp3 a
	LEFT JOIN #tmpTotoalGW C ON A.ID=C.ID
) a

drop table #tmpTotoalCBM,#tmpTotoalGW
");
                    }

                    queryAccount = string.Format(
                        "{0}{1}",
                        sqlCmd.ToString(),
                        string.Format(@" 
select distinct a.*
into #Accno 
from (
select AccountID as Accno from #temp4 where AccountID not in ('61022001','61022002','61022003','61022004','61022005','59121111')
and AccountID <> ''
) a

select Accno=cast(Accno as nvarchar(100)) ,rn=ROW_NUMBER() over (order by Accno)
into #AccnoNo
from #Accno

if exists(select Accno from #AccnoNo where Accno like '5912%')
begin
insert into #AccnoNo
select '5912-Total',max(rn)+1 from #AccnoNo 
end 

if exists(select Accno from #AccnoNo where Accno like '6105%')
begin
insert into #AccnoNo
select '6105-Total',max(rn)+1 from #AccnoNo 
end 

select Accno,rn
from #AccnoNo
order by SUBSTRING(Accno,1,4),rn
"));
                    result = DBProxy.Current.Select(null, queryAccount, out this.accnoData);
                    if (!result)
                    {
                        DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                        return failResult;
                    }

                    StringBuilder allAccno = new StringBuilder();
                    allAccno.Append("[61022001],[61022002],[61022003],[61022004],[61022005],[59121111]");
                    foreach (DataRow dr in this.accnoData.Rows)
                    {
                        allAccno.Append(string.Format(",[{0}]", MyUtility.Convert.GetString(dr["Accno"])));
                    }

                    sqlCmd.Append(string.Format(
                        @"
select *
from #temp4
PIVOT (SUM(Amount)
FOR AccountID IN ({0})) a
order by id
drop table #temp1,#temp2,#temp3,#temp4
", allAccno.ToString().Substring(0, 1) == "," ? allAccno.ToString().Substring(1, allAccno.Length - 1) : allAccno.ToString()));
                }
                else if (this.reportType == 3)
                {
                    // Detail List by SP# by Fee Type
                    #region 組SQL
                    sqlCmd.Append($@"
with tmpGB 
as (
	select distinct [Type] = 'GARMENT'
		, g.ID
		, [OnBoardDate] = g.ETD
		, g.Shipper
		, g.BrandID
		, [Category] = case when o.Category = 'B' then 'Bulk'
						  when o.Category = 'S' then 'Sample'
						  when o.Category = 'G' then 'Garment'
						  else '' end
		, pd.OrderID
		, oq.BuyerDelivery
		, [OQty] = isnull(oq.Qty,0)
		, g.CustCDID
		, g.Dest
		, g.ShipModeID
		, [PackID] = p.ID
		, p.PulloutID
		, p.PulloutDate
		, p.ShipQty
		, p.CTNQty
		, p.GW
		, p.CBM
		, [Forwarder] = g.Forwarder+'-'+isnull(ls.Abb,'')
		, s.BLNo
		, [FeeType] = se.AccountID+'-'+isnull(a.Name,'')
		, [Amount] = se.Amount * iif('{this.rateType}' = '', 1, dbo.getRate('{this.rateType}', s.CurrencyID,'USD', s.CDate))
		, se.CurrencyID
		, [APID] = s.ID 
		, s.CDate
		, [ApvDate] = CONVERT(DATE,s.ApvDate) 
		, s.VoucherID
		, s.VoucherDate
		, s.SubType
        , o.FactoryID
		, o.MDivisionID
		, f.KPICode
		, [Foundry] = iif(ISNULL(gm.Foundry,'') = '', '' , 'Y')
		, s.SisFtyAPID
    from ShippingAP s WITH (NOLOCK) 
    inner join ShareExpense se WITH (NOLOCK) on se.ShippingAPID = s.ID and se.Junk = 0
    inner join GMTBooking g WITH (NOLOCK) on g.ID = se.InvNo
    inner join PackingList p WITH (NOLOCK) on p.INVNo = g.ID
    inner join (
		select distinct id,OrderID,OrderShipmodeSeq 
		from PackingList_Detail	pd	
	) pd on pd.id = p.id
    left join Orders o WITH (NOLOCK) on o.ID = pd.OrderID
    left join Order_QtyShip oq WITH (NOLOCK) on oq.Id = pd.OrderID and oq.Seq = pd.OrderShipmodeSeq
    left join LocalSupp ls WITH (NOLOCK) on ls.ID = g.Forwarder
    left join SciFMS_AccountNo a WITH (NOLOCK)  on a.ID = se.AccountID
    left join Factory f with (nolock) on o.FactoryID = f.ID    
	outer apply (
		select top 1 Foundry 
		from GMTBooking WITH (NOLOCK) 
		where ISNULL(s.BLNo,'') != '' 
　　	  and (BLNo = s.BLNo or BL2No = s.BLNo) 
　　	  and Foundry = 1
	)gm
    where s.Type = 'EXPORT'
");
                    if (!MyUtility.Check.Empty(this.date1))
                    {
                        sqlCmd.Append(string.Format(" and p.PulloutDate >= '{0}'", Convert.ToDateTime(this.date1).ToString("d")));
                    }

                    if (!MyUtility.Check.Empty(this.date2))
                    {
                        sqlCmd.Append(string.Format(" and p.PulloutDate <= '{0}'", Convert.ToDateTime(this.date2).ToString("d")));
                    }

                    if (!MyUtility.Check.Empty(this.apApvDate1))
                    {
                        sqlCmd.Append(string.Format(" and CONVERT(DATE,s.ApvDate) >= '{0}'", Convert.ToDateTime(this.apApvDate1).ToString("d")));
                    }

                    if (!MyUtility.Check.Empty(this.apApvDate2))
                    {
                        sqlCmd.Append(string.Format(" and CONVERT(DATE,s.ApvDate) <= '{0}'", Convert.ToDateTime(this.apApvDate2).ToString("d")));
                    }

                    if (!MyUtility.Check.Empty(this.onBoardDate1))
                    {
                        sqlCmd.Append(string.Format(" and CONVERT(DATE,g.ETD) >= '{0}'", Convert.ToDateTime(this.onBoardDate1).ToString("d")));
                    }

                    if (!MyUtility.Check.Empty(this.onBoardDate2))
                    {
                        sqlCmd.Append(string.Format(" and CONVERT(DATE,g.ETD) <= '{0}'", Convert.ToDateTime(this.onBoardDate2).ToString("d")));
                    }

                    if (!MyUtility.Check.Empty(this.voucherDate1))
                    {
                        sqlCmd.Append(string.Format(" and s.VoucherDate >= '{0}'", Convert.ToDateTime(this.voucherDate1).ToString("d")));
                    }

                    if (!MyUtility.Check.Empty(this.voucherDate2))
                    {
                        sqlCmd.Append(string.Format(" and s.VoucherDate <= '{0}'", Convert.ToDateTime(this.voucherDate2).ToString("d")));
                    }

                    if (!MyUtility.Check.Empty(this.brand))
                    {
                        sqlCmd.Append(string.Format(" and g.BrandID = '{0}'", this.brand));
                    }

                    if (!MyUtility.Check.Empty(this.custcd))
                    {
                        sqlCmd.Append(string.Format(" and g.CustCDID = '{0}'", this.custcd));
                    }

                    if (!MyUtility.Check.Empty(this.dest))
                    {
                        sqlCmd.Append(string.Format(" and g.Dest = '{0}'", this.dest));
                    }

                    if (!MyUtility.Check.Empty(this.shipmode))
                    {
                        sqlCmd.Append(string.Format(" and g.ShipModeID = '{0}'", this.shipmode));
                    }

                    if (!MyUtility.Check.Empty(this.forwarder))
                    {
                        sqlCmd.Append(string.Format(" and g.Forwarder = '{0}'", this.forwarder));
                    }

                    sqlCmd.Append($@"
),
tmpPL as 
(
	select distinct [Type] = 'GARMENT'
		, p.ID
		, [OnBoardDate] = null
		, [Shipper] = ''
		, o.BrandID
		, [Category] = case when o.Category = 'B' then 'Bulk'
							  when o.Category = 'S' then 'Sample'
							  when o.Category = 'G' then 'Garment'
							  else '' end
		, pd.OrderID
		, oq.BuyerDelivery
		, [OQty] = isnull(oq.Qty,0)
		, o.CustCDID
		, o.Dest
		, p.ShipModeID
		, [PackID] = p.ID
		, p.PulloutID
		, p.PulloutDate
		, p.ShipQty
		, p.CTNQty
		, p.GW
		, p.CBM
		, [Forwarder] = ''
		, s.BLNo
		, [FeeType] = se.AccountID+'-'+isnull(a.Name,'')
		, [Amount] = se.Amount * iif('{this.rateType}' = '', 1, dbo.getRate('{this.rateType}', s.CurrencyID,'USD', s.CDate))
		, se.CurrencyID
		, [APID] = s.ID
		, s.CDate
		, [ApvDate] = CONVERT(DATE,s.ApvDate) 
		, s.VoucherID
		, s.VoucherDate
		, s.SubType
        , o.FactoryID
		, o.MDivisionID
		, f.KPICode
		, [Foundry] = iif(ISNULL(gm.Foundry,'') = '', '' , 'Y')
		, s.SisFtyAPID
    from ShippingAP s WITH (NOLOCK) 
    inner join ShareExpense se WITH (NOLOCK) on se.ShippingAPID = s.ID and se.Junk = 0
    inner join PackingList p WITH (NOLOCK) on p.ID = se.InvNo
    inner join (
		select distinct id,OrderID,OrderShipmodeSeq 
		from PackingList_Detail	pd	
	) pd on pd.id = p.id
    left join Orders o WITH (NOLOCK) on o.ID = pd.OrderID
    left join Order_QtyShip oq WITH (NOLOCK) on oq.Id = pd.OrderID and oq.Seq = pd.OrderShipmodeSeq
    left join SciFMS_AccountNo a  WITH (NOLOCK)  on a.ID = se.AccountID
    left join Factory f with (nolock) on o.FactoryID = f.ID
	outer apply (
		select top 1 Foundry 
		from GMTBooking WITH (NOLOCK) 
		where ISNULL(s.BLNo,'') != '' 
　　	  and (BLNo = s.BLNo or BL2No = s.BLNo) 
　　	  and Foundry = 1
	)gm
    where s.Type = 'EXPORT'
");
                    if (!MyUtility.Check.Empty(this.date1))
                    {
                        sqlCmd.Append(string.Format(" and p.PulloutDate >= '{0}'", Convert.ToDateTime(this.date1).ToString("d")));
                    }

                    if (!MyUtility.Check.Empty(this.date2))
                    {
                        sqlCmd.Append(string.Format(" and p.PulloutDate <= '{0}'", Convert.ToDateTime(this.date2).ToString("d")));
                    }

                    if (!MyUtility.Check.Empty(this.apApvDate1))
                    {
                        sqlCmd.Append(string.Format(" and CONVERT(DATE,s.ApvDate) >= '{0}'", Convert.ToDateTime(this.apApvDate1).ToString("d")));
                    }

                    if (!MyUtility.Check.Empty(this.apApvDate2))
                    {
                        sqlCmd.Append(string.Format(" and CONVERT(DATE,s.ApvDate) <= '{0}'", Convert.ToDateTime(this.apApvDate2).ToString("d")));
                    }

                    if (!MyUtility.Check.Empty(this.voucherDate1))
                    {
                        sqlCmd.Append(string.Format(" and s.VoucherDate >= '{0}'", Convert.ToDateTime(this.voucherDate1).ToString("d")));
                    }

                    if (!MyUtility.Check.Empty(this.voucherDate2))
                    {
                        sqlCmd.Append(string.Format(" and s.VoucherDate <= '{0}'", Convert.ToDateTime(this.voucherDate2).ToString("d")));
                    }

                    if (!MyUtility.Check.Empty(this.brand))
                    {
                        sqlCmd.Append(string.Format(" and p.BrandID = '{0}'", this.brand));
                    }

                    if (!MyUtility.Check.Empty(this.custcd))
                    {
                        sqlCmd.Append(string.Format(" and o.CustCDID = '{0}'", this.custcd));
                    }

                    if (!MyUtility.Check.Empty(this.dest))
                    {
                        sqlCmd.Append(string.Format(" and o.Dest = '{0}'", this.dest));
                    }

                    if (!MyUtility.Check.Empty(this.shipmode))
                    {
                        sqlCmd.Append(string.Format(" and p.ShipModeID = '{0}'", this.shipmode));
                    }

                    sqlCmd.Append(@")
select * from tmpGB
union all
select * from tmpPL
order by ID,OrderID,PackID");
                    #endregion
                }
                else if (this.reportType == 4)
                {
                    // Air Prepaid Expense Report
                    #region 組SQL
                    sqlCmd.Append(@"
select se.ShippingAPID
	, o.FactoryID
	, a.ResponsibleFty
	, a.ResponsibleSubcon
	, a.ResponsibleSCI
	, a.ResponsibleSupp
	, a.ResponsibleBuyer
	, a.RatioFty
	, a.RatioSubcon
	, a.RatioSCI
	, a.RatioSupp
	, a.RatioBuyer
	, s.LocalSuppID
	, l.Abb
	, r.Name
	, [InvNo] = g.ID
	, se.CurrencyID
	, se.AmtFty
	, se.AmtOther
	, s.APPExchageRate
	, se.AirPPID
	, o.ID
	, oqs.Seq
	, o.BrandID
	, [Qty] = oqs.Qty
into #tmp
from ShareExpense_APP se
left join ShippingAP s on se.ShippingAPID = s.ID
left join GMTBooking g on se.InvNo = g.id
left join AirPP a on se.AirPPID = a.ID
left join PackingList p on se.PackingListID = p.ID
left join Orders o on a.OrderID = o.ID
left join Order_QtyShip oqs on a.OrderID = oqs.Id and a.OrderShipmodeSeq = oqs.Seq
left join LocalSupp l on s.LocalSuppID = l.ID
left join Reason r on a.ReasonID = r.ID and r.ReasonTypeID = 'Air_Prepaid_Reason'
where se.Junk = 0");

                    if (!MyUtility.Check.Empty(this.date1))
                    {
                        sqlCmd.Append(string.Format(" and p.PulloutDate >= '{0}'", Convert.ToDateTime(this.date1).ToString("d")));
                    }

                    if (!MyUtility.Check.Empty(this.date2))
                    {
                        sqlCmd.Append(string.Format(" and p.PulloutDate <= '{0}'", Convert.ToDateTime(this.date2).ToString("d")));
                    }

                    if (!MyUtility.Check.Empty(this.apApvDate1))
                    {
                        sqlCmd.Append(string.Format(" and CONVERT(DATE,s.ApvDate) >= '{0}'", Convert.ToDateTime(this.apApvDate1).ToString("d")));
                    }

                    if (!MyUtility.Check.Empty(this.apApvDate2))
                    {
                        sqlCmd.Append(string.Format(" and CONVERT(DATE,s.ApvDate) <= '{0}'", Convert.ToDateTime(this.apApvDate2).ToString("d")));
                    }

                    if (!MyUtility.Check.Empty(this.onBoardDate1))
                    {
                        sqlCmd.Append(string.Format(" and CONVERT(DATE,g.ETD) >= '{0}'", Convert.ToDateTime(this.onBoardDate1).ToString("d")));
                    }

                    if (!MyUtility.Check.Empty(this.onBoardDate2))
                    {
                        sqlCmd.Append(string.Format(" and CONVERT(DATE,g.ETD) <= '{0}'", Convert.ToDateTime(this.onBoardDate2).ToString("d")));
                    }

                    if (!MyUtility.Check.Empty(this.voucherDate1))
                    {
                        sqlCmd.Append(string.Format(" and s.VoucherDate >= '{0}'", Convert.ToDateTime(this.voucherDate1).ToString("d")));
                    }

                    if (!MyUtility.Check.Empty(this.voucherDate2))
                    {
                        sqlCmd.Append(string.Format(" and s.VoucherDate <= '{0}'", Convert.ToDateTime(this.voucherDate2).ToString("d")));
                    }

                    if (!MyUtility.Check.Empty(this.brand))
                    {
                        sqlCmd.Append(string.Format(" and g.BrandID = '{0}'", this.brand));
                    }

                    if (!MyUtility.Check.Empty(this.custcd))
                    {
                        sqlCmd.Append(string.Format(" and g.CustCDID = '{0}'", this.custcd));
                    }

                    if (!MyUtility.Check.Empty(this.dest))
                    {
                        sqlCmd.Append(string.Format(" and g.Dest = '{0}'", this.dest));
                    }

                    if (!MyUtility.Check.Empty(this.shipmode))
                    {
                        sqlCmd.Append(string.Format(" and g.ShipModeID = '{0}'", this.shipmode));
                    }

                    if (!MyUtility.Check.Empty(this.forwarder))
                    {
                        sqlCmd.Append(string.Format(" and g.Forwarder = '{0}'", this.forwarder));
                    }

                    sqlCmd.Append(@"
select  *
into #tmp_unpivot
from #tmp t
UNPIVOT ( A FOR [Responsibility] IN (ResponsibleFty, ResponsibleSubcon, ResponsibleSCI, ResponsibleSupp, ResponsibleBuyer )) UNA
UNPIVOT ( [Responsibility%] FOR B IN (RatioFty, RatioSubcon, RatioSCI, RatioSupp, RatioBuyer )) UNB
where [A] > 0
and REPLACE([Responsibility], 'Responsible', '') = REPLACE(B, 'Ratio', '')


select t.ShippingAPID
	, t.FactoryID
	, [Responsibility] = case REPLACE(t.Responsibility, 'Responsible', '') 
							when 'Fty' then 'Factory'
							when 'Supp' then 'Supplier' 
							else REPLACE(t.Responsibility, 'Responsible', '') 
						end
	, t.[Responsibility%]
	, t.LocalSuppID
	, t.Abb
	, t.Name
	, t.InvNo
	, t.CurrencyID
	, [OriAmount] = sum(t.AmtFty + t.AmtOther)
	, [OriAmtOther] = iif(t.APPExchageRate = 0, 0, sum(t.AmtOther) / t.APPExchageRate)
	, t.APPExchageRate
	, [Amount] = iif(isnull(t.APPExchageRate,0) = 0, 0, sum(t.AmtFty + t.AmtOther) / t.APPExchageRate)
	, t.AirPPID
	, t.ID
	, t.Seq
	, [AppAmtByRes] = iif(t.APPExchageRate = 0, 0, case when REPLACE(t.Responsibility, 'Responsible', '') = 'Factory' 
				then sum(t.AmtFty) / t.APPExchageRate 
				else (sum(t.AmtFty + t.AmtOther) * (t.[Responsibility%] / 100)) / t.APPExchageRate 
				end)
	, t.BrandID
	, t.Qty
	, r_id = ROW_NUMBER() Over(partition by t.ShippingAPID 
							order by case REPLACE(t.Responsibility, 'Responsible', '')
								when 'Fty' then 5
								when 'Subcon' then 1
								when 'SCI' then 2
								when 'Supp' then 3
								when 'Buyer' then 4
							end)
into #tmp_Detail
from #tmp_unpivot t
group by t.ShippingAPID
	, t.FactoryID
	, t.Responsibility
	, t.[Responsibility%]
	, t.LocalSuppID
	, t.Abb
	, t.Name
	, t.InvNo
	, t.CurrencyID
	, t.APPExchageRate
	, t.AirPPID
	, t.ID
	, t.Seq
	, t.BrandID
	, t.Qty


-- Summary
select t.ShippingAPID
	, t.FactoryID
	, [Responsibility] = Responsibility.val
	, t.LocalSuppID
	, t.Abb
	, t.Name
	, t.InvNo
	, t.CurrencyID
	, [OriAmount] = sum(t.AmtFty + t.AmtOther)
	, t.APPExchageRate
	, [Amount] = iif(isnull(t.APPExchageRate,0) = 0, 0, sum(t.AmtFty + t.AmtOther) / t.APPExchageRate)
	, t.AirPPID
	, t.ID
	, t.Seq
	, t.BrandID
	, [Qty] = t.Qty
from #tmp t
outer apply (
	select [val] = STUFF((
              iif(t.ResponsibleFty = 1, '/Factory', '')
			+ iif(t.ResponsibleSubcon = 1, '/Subcon', '')
			+ iif(t.ResponsibleSCI = 1, '/SCI', '')
			+ iif(t.ResponsibleSupp = 1, '/Supplier', '')
			+ iif(t.ResponsibleBuyer = 1, '/Buyer', ''))
       ,1,1,'')
)Responsibility	 
group by t.ShippingAPID
   , t.FactoryID
   , Responsibility.val
   , t.LocalSuppID
   , t.Abb
   , t.Name
   , t.InvNo
   , t.CurrencyID
   , t.APPExchageRate
   , t.AirPPID
   , t.ID
   , t.Seq
   , t.BrandID
   , t.Qty


-- Detail
-- 將多出的扣除到第一筆中
select t.ShippingAPID
	,t.FactoryID
	,t.Responsibility
	,t.[Responsibility%]
	,t.LocalSuppID
	,t.Abb
	,t.Name
	,t.InvNo
	,t.CurrencyID
	,t.OriAmount
	,t.APPExchageRate
	,t.Amount
	,t.AirPPID
	,t.ID
	,t.Seq
	,[AppAmtByRes] = case when t1.diff > 0 and r_id = 1 and t.Responsibility <> 'Factory' then t.AppAmtByRes - t1.diff else t.AppAmtByRes end
	,t.BrandID
	,t.Qty
from #tmp_Detail t
left join (
	select t1.ShippingAPID, t1.AirPPID, [diff] = t1.AppAmtByRes - t1.OriAmtOther
	from (
		select t.ShippingAPID, t.AirPPID, t.OriAmtOther, [AppAmtByRes] = sum(t.AppAmtByRes)
		from #tmp_Detail t
        where t.Responsibility <> 'Factory'
		group by t.ShippingAPID, t.AirPPID, t.OriAmtOther
	)t1
)t1 on t.ShippingAPID = t1.ShippingAPID and t.AirPPID = t1.AirPPID

drop table #tmp, #tmp_unpivot, #tmp_Detail
");
                    #endregion
                }
            }
            else
            {
                // Row Material
                // Export Fee Report or Detail List by SP#
                if (this.reportType == 1 || this.reportType == 2)
                {
                    // Export Fee Report
                    if (this.reportType == 1)
                    {
                        #region 組SQL
                        sqlCmd.Append($@"
with tmpMaterialData
as (
	select [Type] = 'MATERIAL'
		, f.ID
		, [Shipper] = s.MDivisionID
		, [BrandID] = '' 
		, [Category] = '' 
		, [OQty] = 0
		, [CustCDID] = ''
		, [Dest] = f.ImportCountry
		, f.ShipModeID
		, [PulloutDate] = f.PortArrival
		, [ShipQty] = 0
		, [CTNQty] = 0 
		, [GW] = f.WeightKg 
		, [CBM] = f.Cbm
		, [Forwarder] = f.Forwarder+'-'+isnull(ls.Abb,'') 
		, [BLNo] = f.Blno
		, se.CurrencyID
		, [AccountID]= iif(se.AccountID='','Empty',se.AccountID)
		, [Amount] = se.Amount * iif('{this.rateType}' = '', 1, dbo.getRate('{this.rateType}', s.CurrencyID,'USD', s.CDate))
    from ShippingAP s WITH (NOLOCK) 
    inner join ShareExpense se WITH (NOLOCK) on se.ShippingAPID = s.ID and se.Junk = 0
    inner join FtyExport f WITH (NOLOCK) on f.ID = se.InvNo
    left join LocalSupp ls WITH (NOLOCK) on ls.ID = f.Forwarder
    where s.Type = 'EXPORT'
");
                        #endregion
                    }
                    else
                    { // Detail List by SP#
                        #region 組SQL
                        sqlCmd.Append($@"
with tmpMaterialData
as (
	select [Type] = 'MATERIAL'
		, f.ID
		, [Shipper] = s.MDivisionID
		, [BrandID] = ''
		, [Category] = ''
		, [OrderID] = ''
		, [BuyerDelivery] = null
		, [OQty] = 0
		, [CustCDID] = ''
		, [Dest] = f.ImportCountry
		, f.ShipModeID
		, [PackID] = ''
		, [PulloutID] = ''
		, [PulloutDate] = f.PortArrival
		, [ShipQty] = 0
		, [CTNQty] = 0
		, [GW] = f.WeightKg
		, [CBM] = f.Cbm
		, [Forwarder] = f.Forwarder+'-'+isnull(ls.Abb,'')
		, [BLNo] = f.Blno
		, se.CurrencyID
		, [AccountID]= iif(se.AccountID='','Empty',se.AccountID)
		, [Amount] = se.Amount * iif('{this.rateType}' = '', 1, dbo.getRate('{this.rateType}', s.CurrencyID,'USD', s.CDate))
	from ShippingAP s WITH (NOLOCK) 
	inner join ShareExpense se WITH (NOLOCK) on se.ShippingAPID = s.ID and se.Junk = 0
	inner join FtyExport f WITH (NOLOCK) on f.ID = se.InvNo
	left join LocalSupp ls WITH (NOLOCK) on ls.ID = f.Forwarder
	where s.Type = 'EXPORT'
");
                        #endregion
                    }
                    #region 組條件
                    if (!MyUtility.Check.Empty(this.date1))
                    {
                        sqlCmd.Append(string.Format(" and f.PortArrival >= '{0}'", Convert.ToDateTime(this.date1).ToString("d")));
                    }

                    if (!MyUtility.Check.Empty(this.date2))
                    {
                        sqlCmd.Append(string.Format(" and f.PortArrival <= '{0}'", Convert.ToDateTime(this.date2).ToString("d")));
                    }

                    if (!MyUtility.Check.Empty(this.apApvDate1))
                    {
                        sqlCmd.Append(string.Format(" and CONVERT(DATE,s.ApvDate) >= '{0}'", Convert.ToDateTime(this.apApvDate1).ToString("d")));
                    }

                    if (!MyUtility.Check.Empty(this.apApvDate2))
                    {
                        sqlCmd.Append(string.Format(" and CONVERT(DATE,s.ApvDate) <= '{0}'", Convert.ToDateTime(this.apApvDate2).ToString("d")));
                    }

                    if (!MyUtility.Check.Empty(this.voucherDate1))
                    {
                        sqlCmd.Append(string.Format(" and s.VoucherDate >= '{0}'", Convert.ToDateTime(this.voucherDate1).ToString("d")));
                    }

                    if (!MyUtility.Check.Empty(this.voucherDate2))
                    {
                        sqlCmd.Append(string.Format(" and s.VoucherDate <= '{0}'", Convert.ToDateTime(this.voucherDate2).ToString("d")));
                    }

                    if (!MyUtility.Check.Empty(this.dest))
                    {
                        sqlCmd.Append(string.Format(" and f.ImportCountry = '{0}'", this.dest));
                    }

                    if (!MyUtility.Check.Empty(this.shipmode))
                    {
                        sqlCmd.Append(string.Format(" and f.ShipModeID = '{0}'", this.shipmode));
                    }

                    if (!MyUtility.Check.Empty(this.forwarder))
                    {
                        sqlCmd.Append(string.Format(" and f.Forwarder = '{0}'", this.forwarder));
                    }
                    #endregion
                    queryAccount = string.Format(
                        "{0}{1}",
                        sqlCmd.ToString(),
                        string.Format(@"
) 
select distinct a.* 
into #Accno 
from (
select Accountid as Accno from tmpMaterialData where AccountID not in ('61022001','61022002','61022003','61022004','61022005','59121111')
and AccountID <> ''
) a
select Accno=cast(Accno as nvarchar(100)) ,rn=ROW_NUMBER() over (order by Accno)
into #AccnoNo
from #Accno

if exists(select Accno from #AccnoNo where Accno like '5912%')
begin
insert into #AccnoNo
select '5912-Total',max(rn)+1 from #AccnoNo 
end 

if exists(select Accno from #AccnoNo where Accno like '6105%')
begin
insert into #AccnoNo
select '6105-Total',max(rn)+1 from #AccnoNo 
end 

select Accno,rn
from #AccnoNo
order by SUBSTRING(Accno,1,4),rn"));
                    result = DBProxy.Current.Select(null, queryAccount, out this.accnoData);
                    if (!result)
                    {
                        DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                        return failResult;
                    }

                    StringBuilder allAccno = new StringBuilder();
                    allAccno.Append("[61022001],[61022002],[61022003],[61022004],[61022005],[59121111]");
                    foreach (DataRow dr in this.accnoData.Rows)
                    {
                        allAccno.Append(string.Format(",[{0}]", MyUtility.Convert.GetString(dr["Accno"])));
                    }

                    sqlCmd.Append(string.Format(
                        @")
select * from tmpMaterialData
PIVOT (SUM(Amount)
FOR AccountID IN ({0})) a", allAccno.ToString()));
                }
                else
                { // Detail List by SP# by Fee Type
                    #region 組SQL
                    sqlCmd.Append($@"
select [Type] = 'MATERIAL'
	, f.ID
	, [Shipper] = s.MDivisionID
	, [BrandID] = ''
	, [Category] = ''
	, [OrderID] = ''
	, [BuyerDelivery] = null 
	, [OQty] = 0
	, [CustCDID] = ''
	, [Dest] = f.ImportCountry 
	, f.ShipModeID
	, [PackID] = ''
	, [PulloutID] = ''
	, [PulloutDate] = f.PortArrival 
	, [ShipQty] = 0 
	, [CTNQty] = 0
	, [GW] = f.WeightKg
	, [CBM] = f.Cbm
	, [Forwarder] = f.Forwarder+'-'+isnull(ls.Abb,'') 
	, [BLNo] = f.Blno 
	, [FeeType] = se.AccountID+'-'+isnull(a.Name,'')
	, [Amount] = se.Amount * iif('{this.rateType}' = '', 1, dbo.getRate('{this.rateType}', s.CurrencyID,'USD', s.CDate))
	, se.CurrencyID
	, [APID] = s.ID
	, s.CDate
	, [ApvDate] = CONVERT(DATE,s.ApvDate)
	, s.VoucherID
	, s.VoucherDate
	, s.SubType
from ShippingAP s WITH (NOLOCK) 
inner join ShareExpense se WITH (NOLOCK) on se.ShippingAPID = s.ID and se.Junk = 0
inner join FtyExport f WITH (NOLOCK) on f.ID = se.InvNo
left join LocalSupp ls WITH (NOLOCK) on ls.ID = f.Forwarder
left join SciFMS_AccountNo a on a.ID = se.AccountID
where s.Type = 'EXPORT'");
                    if (!MyUtility.Check.Empty(this.date1))
                    {
                        sqlCmd.Append(string.Format(" and f.PortArrival >= '{0}'", Convert.ToDateTime(this.date1).ToString("d")));
                    }

                    if (!MyUtility.Check.Empty(this.date2))
                    {
                        sqlCmd.Append(string.Format(" and f.PortArrival <= '{0}'", Convert.ToDateTime(this.date2).ToString("d")));
                    }

                    if (!MyUtility.Check.Empty(this.apApvDate1))
                    {
                        sqlCmd.Append(string.Format(" and CONVERT(DATE,s.ApvDate) >= '{0}'", Convert.ToDateTime(this.apApvDate1).ToString("d")));
                    }

                    if (!MyUtility.Check.Empty(this.apApvDate2))
                    {
                        sqlCmd.Append(string.Format(" and CONVERT(DATE,s.ApvDate) <= '{0}'", Convert.ToDateTime(this.apApvDate2).ToString("d")));
                    }

                    if (!MyUtility.Check.Empty(this.voucherDate1))
                    {
                        sqlCmd.Append(string.Format(" and s.VoucherDate >= '{0}'", Convert.ToDateTime(this.voucherDate1).ToString("d")));
                    }

                    if (!MyUtility.Check.Empty(this.voucherDate2))
                    {
                        sqlCmd.Append(string.Format(" and s.VoucherDate <= '{0}'", Convert.ToDateTime(this.voucherDate2).ToString("d")));
                    }

                    if (!MyUtility.Check.Empty(this.dest))
                    {
                        sqlCmd.Append(string.Format(" and f.ImportCountry = '{0}'", this.dest));
                    }

                    if (!MyUtility.Check.Empty(this.shipmode))
                    {
                        sqlCmd.Append(string.Format(" and f.ShipModeID = '{0}'", this.shipmode));
                    }

                    if (!MyUtility.Check.Empty(this.forwarder))
                    {
                        sqlCmd.Append(string.Format(" and f.Forwarder = '{0}'", this.forwarder));
                    }

                    sqlCmd.Append(" order by f.ID");
                    #endregion
                }
            }

            if (this.reportType == 4)
            {
                result = DBProxy.Current.Select(null, sqlCmd.ToString(), out this.printDataS);
                this.printData = this.printDataS[0];
            }
            else
            {
                result = DBProxy.Current.Select(null, sqlCmd.ToString(), out this.printData);
            }

            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return Ict.Result.True;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.printData.Rows.Count);

            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            this.ShowWaitMessage("Starting EXCEL...");

            if (this.reportType == 5)
            {
                this.ReportType5ToExcel();
                this.HideWaitMessage();
                return true;
            }

            string strXltName = Env.Cfg.XltPathDir + (this.reportType == 1 ? "\\Shipping_R10_ShareExpenseExportFeeReport.xltx" : this.reportType == 2 ? "\\Shipping_R10_ShareExpenseExportBySP.xltx" : this.reportType == 3 ? "\\Shipping_R10_ShareExpenseExportBySPByFee.xltx" : "\\Shipping_R10_AirPrepaidExpense.xltx");

            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return false;
            }

            // excel.Visible = true;
            DataTable tb_onBoardDate = new DataTable();
            DataTable tb_IncludeFoundry = new DataTable();
            DataTable tb_SisFtyAP = new DataTable();
            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];
            if (this.reportContent == 2)
            {
                if (this.reportType != 1)
                {
                    worksheet.get_Range("C1", "E1").EntireColumn.Delete(Missing.Value);
                }

                worksheet.Cells[1, 2] = "FTY WK#";
                worksheet.Cells[1, 3] = "M";
                if (this.reportType == 1)
                {
                    worksheet.Cells[1, 10] = "Ship Date";
                }
                else
                {
                    worksheet.Cells[1, 14] = "Ship Date";
                }
            }

            // mantis9831 增加On Board Date，因為只針對Garment keep OnBoardDate欄位之後插入
            else
            {
                if (this.reportType != 4)
                {
                    tb_onBoardDate = this.printData.Copy();
                    for (int f = 0; f < tb_onBoardDate.Columns.Count; f++)
                    {
                        if (!tb_onBoardDate.Columns[f].ColumnName.Equals("OnBoardDate"))
                        {
                            tb_onBoardDate.Columns.RemoveAt(f);
                            f--;
                        }
                    }

                    this.printData.Columns.Remove("OnBoardDate");

                    tb_IncludeFoundry = this.printData.Copy();
                    for (int f = 0; f < tb_IncludeFoundry.Columns.Count; f++)
                    {
                        if (!tb_IncludeFoundry.Columns[f].ColumnName.Equals("Foundry"))
                        {
                            tb_IncludeFoundry.Columns.RemoveAt(f);
                            f--;
                        }
                    }

                    this.printData.Columns.Remove("Foundry");

                    tb_SisFtyAP = this.printData.Copy();
                    for (int f = 0; f < tb_SisFtyAP.Columns.Count; f++)
                    {
                        if (!tb_SisFtyAP.Columns[f].ColumnName.Equals("SisFtyAPID"))
                        {
                            tb_SisFtyAP.Columns.RemoveAt(f);
                            f--;
                        }
                    }

                    this.printData.Columns.Remove("SisFtyAPID");
                }
            }

            if (this.reportType == 1 || this.reportType == 2)
            {
                int allColumn = 0;
                if (this.reportType == 1)
                {
                    allColumn = 23;
                }
                else
                {
                    if (this.reportContent == 1)
                    {
                        allColumn = 30;
                    }
                    else
                    {
                        allColumn = 27;
                    }
                }

                #region Setting AccountNo
                int i = 0;
                int counts = 0;
                string accnoL1 = "5912"; // Z欄 5912-2222 Airfreight
                string accnoLnow = string.Empty;
                if (this.reportType != 3)
                {
                    counts = this.accnoData.Rows.Count;
                    foreach (DataRow dr in this.accnoData.Rows)
                    {
                        i++;
                        string sql = string.Format("select concat(SUBSTRING(id,1,4),iif(len(id)>4,'-'+SUBSTRING(id,5,4),''),char(10)+char(13) ,Name) from SciFMS_AccountNo  WITH (NOLOCK)  where ID = '{0}'", MyUtility.Convert.GetString(dr["Accno"]));
                        string accnoColName = MyUtility.GetValue.Lookup(sql);
                        accnoLnow = MyUtility.Convert.GetString(dr["Accno"]).Substring(0, 4);
                        string accnoLnow2 = MyUtility.Convert.GetString(dr["Accno"]).Length > 8 ? MyUtility.Convert.GetString(dr["Accno"]).Substring(4) : MyUtility.Convert.GetString(dr["Accno"]).Length > 4 ? "-" + MyUtility.Convert.GetString(dr["Accno"]).Substring(4) : string.Empty;
                        worksheet.Cells[1, allColumn + i] = MyUtility.Check.Empty(accnoColName) ? accnoLnow + accnoLnow2 : accnoColName;

                        accnoL1 = MyUtility.Convert.GetString(dr["Accno"]).Substring(0, 4);
                    }

                    worksheet.Cells[1, allColumn + i + 1] = "Total Export Fee";
                }

                // 匯率選擇 Fixed, KPI, 各費用欄位名稱加上 (USD)
                if (!MyUtility.Check.Empty(this.comboRateType.SelectedValue))
                {
                    for (int k = allColumn - 5; k <= allColumn + i + 1; k++)
                    {
                        worksheet.Cells[1, k] = worksheet.Cells[1, k].Value + "\r\n(USD)";
                    }
                }

                string excelSumCol = PublicPrg.Prgs.GetExcelEnglishColumnName(allColumn + i);
                string excelColumn = PublicPrg.Prgs.GetExcelEnglishColumnName(allColumn + i + 1);

                var first6105 = this.accnoData.AsEnumerable().Where(w => MyUtility.Convert.GetString(w["Accno"]).Substring(0, 4).EqualString("6105")).GroupBy(t => 1).Select(s => new { rn = s.Min(m => MyUtility.Convert.GetInt(m["rn"])) }).ToList();
                string first6105Column = string.Empty;
                if (first6105.Count > 0)
                {
                    if (this.accnoData.Select("Accno like '5912%'").Count() > 0)
                    {
                        first6105Column = PublicPrg.Prgs.GetExcelEnglishColumnName(allColumn + first6105[0].rn + 1);
                    }
                    else
                    {
                        first6105Column = PublicPrg.Prgs.GetExcelEnglishColumnName(allColumn + first6105[0].rn);
                    }
                }

                string sumCol5912start = string.Empty;
                string sumCol5912 = string.Empty;
                string sumCol6105 = string.Empty;
                string sumCol5912TTL = string.Empty;
                string sumCol6105TTL = string.Empty;
                #endregion

                // 填內容值
                int intRowsStart = 2;
                object[,] objArray = new object[1, allColumn + i + 1];
                foreach (DataRow dr in this.printData.Rows)
                {
                    objArray[0, 0] = dr[0];
                    objArray[0, 1] = dr[1];
                    objArray[0, 2] = dr[2];
                    objArray[0, 3] = dr[3];
                    objArray[0, 4] = dr[4];
                    objArray[0, 5] = dr[5];
                    objArray[0, 6] = dr[6];
                    objArray[0, 7] = dr[7];
                    objArray[0, 8] = dr[8];
                    objArray[0, 9] = dr[9];
                    objArray[0, 10] = dr[10];
                    objArray[0, 11] = dr[11];
                    objArray[0, 12] = dr[12];
                    objArray[0, 13] = dr[13];
                    objArray[0, 14] = dr[14];
                    objArray[0, 15] = dr[15];
                    objArray[0, 16] = dr[16];

                    if (this.reportType == 1)
                    {
                        objArray[0, 17] = MyUtility.Check.Empty(dr[17]) ? 0 : dr[17];
                        objArray[0, 18] = MyUtility.Check.Empty(dr[18]) ? 0 : dr[18];
                        objArray[0, 19] = MyUtility.Check.Empty(dr[19]) ? 0 : dr[19];
                        objArray[0, 20] = MyUtility.Check.Empty(dr[20]) ? 0 : dr[20];
                        objArray[0, 21] = MyUtility.Check.Empty(dr[21]) ? 0 : dr[21];
                        objArray[0, 22] = MyUtility.Check.Empty(dr[22]) ? 0 : dr[22];

                        // 多增加的AccountID, 必須要動態的填入欄位值!
                        if (counts > 0)
                        {
                            for (int t = 1; t <= counts; t++)
                            {
                                if (MyUtility.Convert.GetString(dr.Table.Columns[22 + t].ColumnName).Contains("5912"))
                                {
                                    if (MyUtility.Check.Empty(sumCol5912start))
                                    {
                                        sumCol5912start = PublicPrg.Prgs.GetExcelEnglishColumnName(23 + t);
                                    }
                                }

                                if (MyUtility.Convert.GetString(dr.Table.Columns[22 + t].ColumnName).EqualString("5912-Total"))
                                {
                                    if (MyUtility.Check.Empty(sumCol5912))
                                    {
                                        sumCol5912 = PublicPrg.Prgs.GetExcelEnglishColumnName(22 + t);
                                        sumCol5912TTL = PublicPrg.Prgs.GetExcelEnglishColumnName(23 + t);
                                    }

                                    objArray[0, 22 + t] = $"=W{intRowsStart}+SUM({sumCol5912start}{intRowsStart}:{sumCol5912}{intRowsStart})";
                                }
                                else if (MyUtility.Convert.GetString(dr.Table.Columns[22 + t].ColumnName).EqualString("6105-Total"))
                                {
                                    if (MyUtility.Check.Empty(sumCol6105))
                                    {
                                        sumCol6105 = PublicPrg.Prgs.GetExcelEnglishColumnName(22 + t);
                                        sumCol6105TTL = PublicPrg.Prgs.GetExcelEnglishColumnName(23 + t);
                                    }

                                    objArray[0, 22 + t] = $"=SUM({first6105Column}{intRowsStart}:{sumCol6105}{intRowsStart})";
                                }
                                else
                                {
                                    objArray[0, 22 + t] = MyUtility.Check.Empty(dr[22 + t]) ? 0 : dr[22 + t];
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int f = 17; f < allColumn; f++)
                        {
                            if (f >= allColumn - 6)
                            {
                                objArray[0, f] = MyUtility.Check.Empty(dr[f]) ? 0 : dr[f];
                            }
                            else
                            {
                                objArray[0, f] = dr[f];
                            }
                        }

                        // 多增加的AccountID, 必須要動態的填入欄位值!
                        if (counts > 0)
                        {
                            for (int c = 1; c <= counts; c++)
                            {
                                if (MyUtility.Convert.GetString(dr.Table.Columns[allColumn - 1 + c].ColumnName).Contains("5912"))
                                {
                                    if (MyUtility.Check.Empty(sumCol5912start))
                                    {
                                        sumCol5912start = PublicPrg.Prgs.GetExcelEnglishColumnName(allColumn + c);
                                    }
                                }

                                if (MyUtility.Convert.GetString(dr.Table.Columns[allColumn - 1 + c].ColumnName).EqualString("5912-Total"))
                                {
                                    if (MyUtility.Check.Empty(sumCol5912))
                                    {
                                        sumCol5912 = PublicPrg.Prgs.GetExcelEnglishColumnName(allColumn - 1 + c);
                                        sumCol5912TTL = PublicPrg.Prgs.GetExcelEnglishColumnName(allColumn + c);
                                    }

                                    objArray[0, allColumn - 1 + c] = $"=AA{intRowsStart}+SUM({sumCol5912start}{intRowsStart}:{sumCol5912}{intRowsStart})";
                                }
                                else if (MyUtility.Convert.GetString(dr.Table.Columns[allColumn - 1 + c].ColumnName).EqualString("6105-Total"))
                                {
                                    if (MyUtility.Check.Empty(sumCol6105))
                                    {
                                        sumCol6105 = PublicPrg.Prgs.GetExcelEnglishColumnName(allColumn - 1 + c);
                                        sumCol6105TTL = PublicPrg.Prgs.GetExcelEnglishColumnName(allColumn + c);
                                    }

                                    objArray[0, allColumn - 1 + c] = $"=SUM({first6105Column}{intRowsStart}:{sumCol6105}{intRowsStart})";
                                }
                                else
                                {
                                    objArray[0, allColumn - 1 + c] = MyUtility.Check.Empty(dr[allColumn - 1 + c]) ? 0 : dr[allColumn - 1 + c];
                                }
                            }
                        }
                    }

                    string sc1 = string.Empty;
                    string sc2 = string.Empty;
                    if (!MyUtility.Check.Empty(sumCol5912TTL))
                    {
                        sc1 = $"-{sumCol5912TTL}{intRowsStart}";
                    }

                    if (!MyUtility.Check.Empty(sumCol6105TTL))
                    {
                        sc2 = $"-{sumCol6105TTL}{intRowsStart}";
                    }

                    int totalSumcolumn = allColumn + this.accnoData.Rows.Count;

                    // if (this.reportContent == 2)
                    // {
                    //    totalSumcolumn += 1;
                    // }
                    string sumStartColEng = this.reportType == 1 ? "R" : this.reportContent == 2 ? "V" : "Y";
                    objArray[0, totalSumcolumn] = string.Format("=SUM({2}{0}:{1}{0}) {3} {4}", intRowsStart, excelSumCol, sumStartColEng, sc1, sc2);

                    worksheet.Range[string.Format("A{0}:{1}{0}", intRowsStart, excelColumn)].Value2 = objArray;
                    intRowsStart++;
                }
            }
            else if (this.reportType == 3)
            {
                // 匯率選擇 Fixed, KPI, 各費用欄位名稱加上 (USD)
                if (!MyUtility.Check.Empty(this.comboRateType.SelectedValue))
                {
                    if (this.reportContent == 1)
                    {
                        worksheet.Cells[1, 25] = worksheet.Cells[1, 25].Value + "\r\n(USD)";
                    }
                    else
                    {
                        worksheet.Cells[1, 22] = worksheet.Cells[1, 22].Value + "\r\n(USD)";
                    }
                }

                // 填內容值
                int intRowsStart = 2;
                object[,] objArray = new object[1, 32];
                foreach (DataRow dr in this.printData.Rows)
                {
                    if (this.reportContent == 1)
                    {
                        objArray[0, 0] = dr["Type"];
                        objArray[0, 1] = dr["ID"];
                        objArray[0, 2] = dr["Shipper"];
                        objArray[0, 3] = dr["FactoryID"];
                        objArray[0, 4] = dr["MDivisionID"];
                        objArray[0, 5] = dr["KPICode"];
                        objArray[0, 6] = dr["BrandID"];
                        objArray[0, 7] = dr["Category"];
                        objArray[0, 8] = dr["OrderID"];
                        objArray[0, 9] = dr["BuyerDelivery"];
                        objArray[0, 10] = dr["OQty"];
                        objArray[0, 11] = dr["CustCDID"];
                        objArray[0, 12] = dr["Dest"];
                        objArray[0, 13] = dr["ShipModeID"];
                        objArray[0, 14] = dr["PackID"];
                        objArray[0, 15] = dr["PulloutID"];
                        objArray[0, 16] = dr["PulloutDate"];
                        objArray[0, 17] = dr["ShipQty"];
                        objArray[0, 18] = dr["CTNQty"];
                        objArray[0, 19] = dr["GW"];
                        objArray[0, 20] = dr["CBM"];
                        objArray[0, 21] = dr["Forwarder"];
                        objArray[0, 22] = dr["BLNo"];
                        objArray[0, 23] = dr["FeeType"];
                        objArray[0, 24] = dr["Amount"];
                        objArray[0, 25] = dr["CurrencyID"];
                        objArray[0, 26] = dr["APID"];
                        objArray[0, 27] = dr["CDate"];
                        objArray[0, 28] = dr["ApvDate"];
                        objArray[0, 29] = dr["VoucherID"];
                        objArray[0, 30] = dr["VoucherDate"];
                        objArray[0, 31] = dr["SubType"];

                        worksheet.Range[string.Format("A{0}:AF{0}", intRowsStart)].Value2 = objArray;
                    }
                    else
                    {
                        objArray[0, 0] = dr["Type"];
                        objArray[0, 1] = dr["ID"];
                        objArray[0, 2] = dr["Shipper"];
                        objArray[0, 3] = dr["BrandID"];
                        objArray[0, 4] = dr["Category"];
                        objArray[0, 5] = dr["OrderID"];
                        objArray[0, 6] = dr["BuyerDelivery"];
                        objArray[0, 7] = dr["OQty"];
                        objArray[0, 8] = dr["CustCDID"];
                        objArray[0, 9] = dr["Dest"];
                        objArray[0, 10] = dr["ShipModeID"];
                        objArray[0, 11] = dr["PackID"];
                        objArray[0, 12] = dr["PulloutID"];
                        objArray[0, 13] = dr["PulloutDate"];
                        objArray[0, 14] = dr["ShipQty"];
                        objArray[0, 15] = dr["CTNQty"];
                        objArray[0, 16] = dr["GW"];
                        objArray[0, 17] = dr["CBM"];
                        objArray[0, 18] = dr["Forwarder"];
                        objArray[0, 19] = dr["BLNo"];
                        objArray[0, 20] = dr["FeeType"];
                        objArray[0, 21] = dr["Amount"];
                        objArray[0, 22] = dr["CurrencyID"];
                        objArray[0, 23] = dr["APID"];
                        objArray[0, 24] = dr["CDate"];
                        objArray[0, 25] = dr["ApvDate"];
                        objArray[0, 26] = dr["VoucherID"];
                        objArray[0, 27] = dr["VoucherDate"];
                        objArray[0, 28] = dr["SubType"];

                        worksheet.Range[string.Format("A{0}:AC{0}", intRowsStart)].Value2 = objArray;
                    }

                    intRowsStart++;
                }
            }
            else if (this.reportType == 4)
            {
                MyUtility.Excel.CopyToXls(this.printDataS[0], string.Empty, "Shipping_R10_AirPrepaidExpense.xltx", 1, false, null, excel, wSheet: excel.Sheets[1]);
                MyUtility.Excel.CopyToXls(this.printDataS[1], string.Empty, "Shipping_R10_AirPrepaidExpense.xltx", 1, false, null, excel, wSheet: excel.Sheets[2]);
            }

            // [On Board Date],[Shipper],[Include Foundry]因為只針對Garment所以在excel產生後插入
            if (this.reportContent == 1 && this.reportType != 4)
            {
                Microsoft.Office.Interop.Excel.Range range = worksheet.get_Range("C1", Missing.Value);
                range.EntireColumn.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftToRight, Microsoft.Office.Interop.Excel.XlInsertFormatOrigin.xlFormatFromRightOrBelow);
                worksheet.Cells[1, 3] = "On Board Date";
                range = worksheet.get_Range("C2", "C" + (this.printData.Rows.Count + 1));
                range.EntireColumn.NumberFormat = "yyyy/MM/dd";

                // Sci.Utility.Report.ExcelCOM com = new Sci.Utility.Report.ExcelCOM();
                object[,] arrayValues = tb_onBoardDate.ToArray2D();
                range.Value2 = arrayValues;

                if (this.reportType == 1)
                {
                    range = worksheet.get_Range("E1", Missing.Value);
                    range.EntireColumn.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftToRight, Microsoft.Office.Interop.Excel.XlInsertFormatOrigin.xlFormatFromRightOrBelow);
                    worksheet.Cells[1, 5] = "Include Foundry";
                    range = worksheet.get_Range("E2", "E" + (this.printData.Rows.Count + 1));
                    arrayValues = tb_IncludeFoundry.ToArray2D();
                    range.Value2 = arrayValues;

                    range = worksheet.get_Range("F1", Missing.Value);
                    range.EntireColumn.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftToRight, Microsoft.Office.Interop.Excel.XlInsertFormatOrigin.xlFormatFromRightOrBelow);
                    worksheet.Cells[1, 6] = "Sis. Fty A/P#";
                    range = worksheet.get_Range("F2", "F" + (this.printData.Rows.Count + 1));
                    arrayValues = tb_SisFtyAP.ToArray2D();
                    range.Value2 = arrayValues;
                }

                if (this.reportType == 2 || this.reportType == 3)
                {
                    range = worksheet.get_Range("H1", Missing.Value);
                    range.EntireColumn.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftToRight, Microsoft.Office.Interop.Excel.XlInsertFormatOrigin.xlFormatFromRightOrBelow);
                    worksheet.Cells[1, 8] = "Include Foundry";
                    range = worksheet.get_Range("H2", "H" + (this.printData.Rows.Count + 1));
                    arrayValues = tb_IncludeFoundry.ToArray2D();
                    range.Value2 = arrayValues;

                    range = worksheet.get_Range("I1", Missing.Value);
                    range.EntireColumn.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftToRight, Microsoft.Office.Interop.Excel.XlInsertFormatOrigin.xlFormatFromRightOrBelow);
                    worksheet.Cells[1, 9] = "Sis. Fty A/P#";
                    range = worksheet.get_Range("I2", "I" + (this.printData.Rows.Count + 1));
                    arrayValues = tb_SisFtyAP.ToArray2D();
                    range.Value2 = arrayValues;
                }
            }

            excel.Cells.EntireColumn.AutoFit();
            excel.Cells.EntireRow.AutoFit();
            this.HideWaitMessage();

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName(this.reportType == 1 ? "Shipping_R10_ShareExpenseExportFeeReport" : this.reportType == 2 ? "Shipping_R10_ShareExpenseExportBySP" : this.reportType == 3 ? "Shipping_R10_ShareExpenseExportBySPByFee" : "Shipping_R10_AirPrepaidExpense");
            excel.ActiveWorkbook.SaveAs(strExcelName);
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(worksheet);

            strExcelName.OpenFile();
            #endregion
            return true;
        }

        /// <summary>
        /// Report Type = 5 的報表資料
        /// </summary>
        private bool ReportType5()
        {
            StringBuilder sqlCmd = new StringBuilder();
            DualResult result;

            try
            {
                // Garment
                if (this.reportContent == 1)
                {
                    #region 組SQL
                    sqlCmd.Append($@"
with tmpGB 
as (
	select distinct [Origin] = (SELECT Region FROM System)
		, [RgCode] = (SELECT RgCode FROM System)
		, [Type] = 'GARMENT'
		, g.ID
		, [OnBoardDate] = g.ETD 
		, g.Shipper
		, [Foundry] = iif(ISNULL(gm.Foundry,'') = '', '' , 'Y')
		, s.SisFtyAPID
		, g.BrandID
		, [Category] = case when o.Category = 'B' then 'Bulk'
							  when o.Category = 'S' then 'Sample'
							  when o.Category = 'G' then 'Garment'
							  else '' end
		, g.CustCDID
		, g.Dest
		, g.ShipModeID
		, p.PulloutDate
		, [Forwarder] = g.Forwarder+'-'+isnull(ls.Abb,'') 
		, s.BLNo
		, se.CurrencyID
		, p.OrderID
		, [packingID] = p.ID
        , se.AccountID
        , [Amount] = se.Amount * iif('{this.rateType}' = '', 1, dbo.getRate('{this.rateType}', s.CurrencyID,'USD', s.CDate))
    from ShippingAP s WITH (NOLOCK) 
    inner join ShareExpense se WITH (NOLOCK) on se.ShippingAPID = s.ID and se.Junk = 0
    inner join GMTBooking g WITH (NOLOCK) on g.ID = se.InvNo
    inner join PackingList p WITH (NOLOCK) on p.INVNo = g.ID
    inner join (
		select distinct id,OrderID,OrderShipmodeSeq 
		from PackingList_Detail	pd	
	) pd on pd.id = p.id
    inner join Orders o WITH (NOLOCK) on o.ID = pd.OrderID
    inner join LocalSupp ls WITH (NOLOCK) on ls.ID = g.Forwarder 
	outer apply (
		select top 1 Foundry 
		from GMTBooking WITH (NOLOCK) 
		where ISNULL(s.BLNo,'') != '' 
　　	  and (BLNo = s.BLNo or BL2No = s.BLNo) 
　　	  and Foundry = 1
	)gm
    where s.Type = 'EXPORT'
");
                    if (!MyUtility.Check.Empty(this.date1))
                    {
                        sqlCmd.Append(string.Format(" and p.PulloutDate >= '{0}'", Convert.ToDateTime(this.date1).ToString("d")));
                    }

                    if (!MyUtility.Check.Empty(this.date2))
                    {
                        sqlCmd.Append(string.Format(" and p.PulloutDate <= '{0}'", Convert.ToDateTime(this.date2).ToString("d")));
                    }

                    if (!MyUtility.Check.Empty(this.apApvDate1))
                    {
                        sqlCmd.Append(string.Format(" and CONVERT(DATE,s.ApvDate) >= '{0}'", Convert.ToDateTime(this.apApvDate1).ToString("d")));
                    }

                    if (!MyUtility.Check.Empty(this.apApvDate2))
                    {
                        sqlCmd.Append(string.Format(" and CONVERT(DATE,s.ApvDate) <= '{0}'", Convert.ToDateTime(this.apApvDate2).ToString("d")));
                    }

                    if (!MyUtility.Check.Empty(this.onBoardDate1))
                    {
                        sqlCmd.Append(string.Format(" and CONVERT(DATE,g.ETD) >= '{0}'", Convert.ToDateTime(this.onBoardDate1).ToString("d")));
                    }

                    if (!MyUtility.Check.Empty(this.onBoardDate2))
                    {
                        sqlCmd.Append(string.Format(" and CONVERT(DATE,g.ETD) <= '{0}'", Convert.ToDateTime(this.onBoardDate2).ToString("d")));
                    }

                    if (!MyUtility.Check.Empty(this.voucherDate1))
                    {
                        sqlCmd.Append(string.Format(" and s.VoucherDate >= '{0}'", Convert.ToDateTime(this.voucherDate1).ToString("d")));
                    }

                    if (!MyUtility.Check.Empty(this.voucherDate2))
                    {
                        sqlCmd.Append(string.Format(" and s.VoucherDate <= '{0}'", Convert.ToDateTime(this.voucherDate2).ToString("d")));
                    }

                    if (!MyUtility.Check.Empty(this.brand))
                    {
                        sqlCmd.Append(string.Format(" and g.BrandID = '{0}'", this.brand));
                    }

                    if (!MyUtility.Check.Empty(this.custcd))
                    {
                        sqlCmd.Append(string.Format(" and g.CustCDID = '{0}'", this.custcd));
                    }

                    if (!MyUtility.Check.Empty(this.dest))
                    {
                        sqlCmd.Append(string.Format(" and g.Dest = '{0}'", this.dest));
                    }

                    if (!MyUtility.Check.Empty(this.shipmode))
                    {
                        sqlCmd.Append(string.Format(" and g.ShipModeID = '{0}'", this.shipmode));
                    }

                    if (!MyUtility.Check.Empty(this.forwarder))
                    {
                        sqlCmd.Append(string.Format(" and g.Forwarder = '{0}'", this.forwarder));
                    }

                    sqlCmd.Append($@"
),
tmpPL as 
(
	select distinct [Origin] = (SELECT Region FROM System)
		, [RgCode] = (SELECT RgCode FROM System)
		, [Type] = 'GARMENT'
		, p.ID
		, [OnBoardDate] = null
		, [Shipper] = ''  
		, [Foundry] = iif(ISNULL(gm.Foundry,'') = '', '' , 'Y')
		, s.SisFtyAPID
		, o.BrandID
		, [Category] = case when o.Category = 'B' then 'Bulk'
							  when o.Category = 'S' then 'Sample'
							  when o.Category = 'G' then 'Garment'
							  else '' end
		, o.CustCDID
		, o.Dest
		, p.ShipModeID
		, p.PulloutDate
		, [Forwarder] = ''
		, s.BLNo
		, se.CurrencyID
		, p.OrderID
		, [packingID] = p.ID
        , se.AccountID
        , [Amount] = se.Amount * iif('{this.rateType}' = '', 1, dbo.getRate('{this.rateType}', s.CurrencyID,'USD', s.CDate))
    from ShippingAP s WITH (NOLOCK) 
    inner join ShareExpense se WITH (NOLOCK) on se.ShippingAPID = s.ID and se.Junk = 0
    inner join PackingList p WITH (NOLOCK) on p.ID = se.InvNo
    inner join (
		select distinct id,OrderID,OrderShipmodeSeq 
		from PackingList_Detail	pd WITH (NOLOCK) 
	) pd on pd.id = p.id
    inner join Orders o WITH (NOLOCK) on o.ID = pd.OrderID
    inner join SciFMS_AccountNo a WITH (NOLOCK)  on a.ID = se.AccountID
	outer apply (
		select top 1 Foundry 
		from GMTBooking WITH (NOLOCK) 
		where ISNULL(s.BLNo,'') != '' 
　　	  and (BLNo = s.BLNo or BL2No = s.BLNo) 
　　	  and Foundry = 1
	)gm
    where s.Type = 'EXPORT'
");
                    if (!MyUtility.Check.Empty(this.date1))
                    {
                        sqlCmd.Append(string.Format(" and p.PulloutDate >= '{0}'", Convert.ToDateTime(this.date1).ToString("d")));
                    }

                    if (!MyUtility.Check.Empty(this.date2))
                    {
                        sqlCmd.Append(string.Format(" and p.PulloutDate <= '{0}'", Convert.ToDateTime(this.date2).ToString("d")));
                    }

                    if (!MyUtility.Check.Empty(this.apApvDate1))
                    {
                        sqlCmd.Append(string.Format(" and CONVERT(DATE,s.ApvDate) >= '{0}'", Convert.ToDateTime(this.apApvDate1).ToString("d")));
                    }

                    if (!MyUtility.Check.Empty(this.apApvDate2))
                    {
                        sqlCmd.Append(string.Format(" and CONVERT(DATE,s.ApvDate) <= '{0}'", Convert.ToDateTime(this.apApvDate2).ToString("d")));
                    }

                    if (!MyUtility.Check.Empty(this.voucherDate1))
                    {
                        sqlCmd.Append(string.Format(" and s.VoucherDate >= '{0}'", Convert.ToDateTime(this.voucherDate1).ToString("d")));
                    }

                    if (!MyUtility.Check.Empty(this.voucherDate2))
                    {
                        sqlCmd.Append(string.Format(" and s.VoucherDate <= '{0}'", Convert.ToDateTime(this.voucherDate2).ToString("d")));
                    }

                    if (!MyUtility.Check.Empty(this.brand))
                    {
                        sqlCmd.Append(string.Format(" and p.BrandID = '{0}'", this.brand));
                    }

                    if (!MyUtility.Check.Empty(this.custcd))
                    {
                        sqlCmd.Append(string.Format(" and o.CustCDID = '{0}'", this.custcd));
                    }

                    if (!MyUtility.Check.Empty(this.dest))
                    {
                        sqlCmd.Append(string.Format(" and o.Dest = '{0}'", this.dest));
                    }

                    if (!MyUtility.Check.Empty(this.shipmode))
                    {
                        sqlCmd.Append(string.Format(" and p.ShipModeID = '{0}'", this.shipmode));
                    }

                    #endregion

                    #region SQL
                    sqlCmd.Append(@"
),
tmpAllData
as (
select * from tmpGB
union all
select * from tmpPL
)

select * 
into #temp1
from tmpAllData

-----temp2
select distinct 
	  Origin
	, RgCode
	, [type]
	, id
	, OnBoardDate
	, shipper
	, Foundry = isnull(f.Foundry,'')
	, SisFtyAPID
	, Brandid
	, category
	, custcdid
	, dest
	, shipmodeid
	, pulloutdate
	, forwarder
	, s.BLNo
	, currencyid
	, orderid
	, packingid
	, AccountID
	, Amount
into #temp2
from #temp1 as a
outer apply (
		select [BLNo]= stuff(
			(select CONCAT(',',blno)
		from (
			select distinct blno 
			from #temp1 d
			where d.id=a.id 
			)s
			for xml path('')
		),1,1,'')
	)s
outer apply(select top 1 Foundry from #temp1 d where d.id=a.id and Foundry = 'Y')f

select Origin
	, RgCode
	, [type]
	, id
	, OnBoardDate
	, Shipper
	, Foundry
	, SisFtyAPID
	, BrandID
	, [Category] = Category.value
	, [OQty]=oqs.OQty
	, CustCDID
	, Dest
	, ShipModeID
	, PulloutDate
	, [ShipQty]=sum(pt.ShipQty)
	, [ctnqty]=sum(pt.ctnqty)
	, [gw]=sum(pt.gw)
	, [CBM]=sum(pt.CBM)
	, Forwarder
	, BLNo
	, CurrencyID
	, AccountID
	, Amount
into #temp3
from #temp2 a
outer apply(
	select sum(qty) as OQty 
	from Order_QtyShip  WITH (NOLOCK) 
	where id IN(
		select DISTINCT pd.OrderID
		from PackingList p 
		inner join PackingList_Detail pd ON p.Id = pd.ID
		where p.INVNo = a.ID
	)
) as oqs
outer apply(select sum(shipqty) as shipqty,sum(CTNQty)as CTNQty,sum(GW) as GW,sum(CBM)as CBM from PackingList WITH (NOLOCK)  where id=a.packingID) as pt
outer apply(
	select value = Stuff((
		select concat(',',Category)
		from (
				select 	distinct Category
				from #temp2 d
				where d.id = a.ID
			) s
		for xml path ('')
	) , 1, 1, '')
) Category
group by Origin, RgCode,type,id,OnBoardDate,Shipper,BrandID,Category.value,CustCDID,
Dest,ShipModeID,PulloutDate,Forwarder,BLNo,CurrencyID
,AccountID,Amount, Foundry, SisFtyAPID,oqs.OQty

select Origin
	, RgCode
	, [type]
	, a.id
	, a.OnBoardDate
	, a.Shipper
	, a.Foundry
	, a.SisFtyAPID
	, a.BrandID
	, a.[Category]
	, a.[OQty]
	, a.CustCDID
	, a.Dest
	, a.ShipModeID
	, a.PulloutDate
	, a.[ShipQty]
	, a.[ctnqty]
	, a.[gw]
	, a.[CBM]
	, a.Forwarder
	, a.BLNo
	, a.CurrencyID 
	, [AccountID]='A_'+a.AccountID
	, a.Amount
into #temp4
from #temp3 a


");

                    #endregion

                    string account = this.GetAccount();
                    sqlCmd.Append($@"

select *
from #temp4
PIVOT (SUM(Amount)
FOR AccountID IN (
    {account}
)) a
order by id

drop table #temp1,#temp2,#temp3,#temp4
");
                }
                else
                {
                    #region 組SQL
                    sqlCmd.Append($@"
with tmpMaterialData
as (
	select distinct [Origin] = (SELECT Region FROM System)
		, [RgCode] = (SELECT RgCode FROM System)
		, [Type] = 'MATERIAL'
		, f.ID
		, [OnBoardDate] = f.OnBoard
		, f.Shipper
		, [Foundry] = iif(ISNULL(gm.Foundry,'') = '', '' , 'Y') 
		, s.SisFtyAPID
		, BrandID=''
		, [Category] = ''
		, CustCDID=''
		, Dest = ''
		, f.ShipModeID
		, PulloutDate =  f.PortArrival
		, [Forwarder] = f.Forwarder+'-'+isnull(ls.Abb,'') 
		, s.BLNo
		, se.CurrencyID
		, OrderID=''
		, [packingID] = ''
        , se.AccountID
        , [Amount] = se.Amount * iif('FX' = '', 1, dbo.getRate('FX', s.CurrencyID,'USD', s.CDate))
    from ShippingAP s WITH (NOLOCK) 
    inner join ShareExpense se WITH (NOLOCK) on se.ShippingAPID = s.ID and se.Junk = 0
    inner join FtyExport f WITH (NOLOCK) on f.ID = se.InvNo
    left join LocalSupp ls WITH (NOLOCK) on ls.ID = f.Forwarder
	outer apply (
		select top 1 Foundry 
		from GMTBooking WITH (NOLOCK) 
		where ISNULL(s.BLNo,'') != '' 
　　	  and (BLNo = s.BLNo or BL2No = s.BLNo) 
　　	  and Foundry = 1
	)gm
    where s.Type = 'EXPORT'
");
                    #endregion

                    #region 組條件
                    if (!MyUtility.Check.Empty(this.date1))
                    {
                        sqlCmd.Append(string.Format(" and f.PortArrival >= '{0}'", Convert.ToDateTime(this.date1).ToString("d")));
                    }

                    if (!MyUtility.Check.Empty(this.date2))
                    {
                        sqlCmd.Append(string.Format(" and f.PortArrival <= '{0}'", Convert.ToDateTime(this.date2).ToString("d")));
                    }

                    if (!MyUtility.Check.Empty(this.apApvDate1))
                    {
                        sqlCmd.Append(string.Format(" and CONVERT(DATE,s.ApvDate) >= '{0}'", Convert.ToDateTime(this.apApvDate1).ToString("d")));
                    }

                    if (!MyUtility.Check.Empty(this.apApvDate2))
                    {
                        sqlCmd.Append(string.Format(" and CONVERT(DATE,s.ApvDate) <= '{0}'", Convert.ToDateTime(this.apApvDate2).ToString("d")));
                    }

                    if (!MyUtility.Check.Empty(this.voucherDate1))
                    {
                        sqlCmd.Append(string.Format(" and s.VoucherDate >= '{0}'", Convert.ToDateTime(this.voucherDate1).ToString("d")));
                    }

                    if (!MyUtility.Check.Empty(this.voucherDate2))
                    {
                        sqlCmd.Append(string.Format(" and s.VoucherDate <= '{0}'", Convert.ToDateTime(this.voucherDate2).ToString("d")));
                    }

                    if (!MyUtility.Check.Empty(this.dest))
                    {
                        sqlCmd.Append(string.Format(" and f.ImportCountry = '{0}'", this.dest));
                    }

                    if (!MyUtility.Check.Empty(this.shipmode))
                    {
                        sqlCmd.Append(string.Format(" and f.ShipModeID = '{0}'", this.shipmode));
                    }

                    if (!MyUtility.Check.Empty(this.forwarder))
                    {
                        sqlCmd.Append(string.Format(" and f.Forwarder = '{0}'", this.forwarder));
                    }
                    #endregion

                    sqlCmd.Append($@"
) 
");

                    string account = this.GetAccount();
                    sqlCmd.Append($@"
select * from tmpMaterialData
PIVOT (SUM(Amount)
FOR AccountID IN ({account})) a

");
                }

                result = DBProxy.Current.Select(null, sqlCmd.ToString(), out this.printData);

                List<ReportData> tmpDatas = new List<ReportData>();
                List<ReportData> finalDatas = new List<ReportData>();

                tmpDatas = PublicPrg.DataTableToList.ConvertToClassList<ReportData>(this.printData).ToList();

                foreach (var d in tmpDatas)
                {
                    ReportData a = this.GetValueByAccountNo(d.RgCode, d);
                    finalDatas.Add(a);
                }

                this.printData.Clear();
                this.printData = PublicPrg.ListToDataTable.ToDataTable<ReportData>(finalDatas);

                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                    this.ShowErr(failResult);
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                this.ShowErr(ex);
                return false;
            }

        }

        private void ReportType5ToExcel()
        {
            string strXltName = Env.Cfg.XltPathDir + "\\Shipping_R10_ExportFeeReport(MergerdAcctCode).xltx";

            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            //excel.Visible = true;
            if (excel == null)
            {
                return;
            }

            DataTable tb_onBoardDate = new DataTable();
            DataTable tb_IncludeFoundry = new DataTable();
            DataTable tb_SisFtyAP = new DataTable();
            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            //if (this.rateType == string.Empty)
            //{
            //    for (int i = 23; i <= 52; i++)
            //    {
            //        if (i != 39 && i != 46)
            //        {
            //            worksheet.Cells[1, i] = worksheet.Cells[1, i].Value + "\r\n(USD)";
            //        }
            //    }
            //}

            if (this.reportContent == 2)
            {
                worksheet.Cells[1, 4] = "FTY WK#";
            }

            // 複製儲存格
            //Microsoft.Office.Interop.Excel.Range rngToCopy = worksheet.get_Range("A2:AZ2").EntireRow;
            //for (int i = 1; i <= this.printData.Rows.Count - 1; i++)
            //{
            //    Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range("A2", Type.Missing).EntireRow; // 選擇要被貼上的位置
            //    rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown, rngToCopy.Copy(Type.Missing)); // 貼上
            //}

            MyUtility.Excel.CopyToXls(this.printData, string.Empty, "Shipping_R10_ExportFeeReport(MergerdAcctCode).xltx", 1, showExcel: false, excelApp: excel);

            // 刪除不必要的欄位
            worksheet.get_Range("BA:BQ").EntireColumn.Delete();

            int x = this.printData.Rows.Count + 2;

            // 剩下的底色弄成白色，抓個兩百行不要被User看到就好
            worksheet.get_Range($"A{x}:AZ{x + 200}").Interior.Color = Color.White;

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("Shipping_R10_ExportFeeReport(MergerdAcctCode)");
            excel.ActiveWorkbook.SaveAs(strExcelName);
            excel.Quit();
            Marshal.ReleaseComObject(excel);

            strExcelName.OpenFile();
            #endregion
        }

        /// <summary>
        /// 取得會計科目 for Report type為Export Fee Report(Mergerd Acct Code)的報表
        /// </summary>
        /// <returns>字串</returns>
        private string GetAccount()
        {
            // 所有欄位會用到的會計科目
            List<string> basic = new List<string>()
            {
                "A_61022001", "A_61022002", "A_61022003", "A_61022004", "A_61022005", "A_61022006",
                "A_61092101", "A_61092102", "A_61092103", "A_61092104", "A_61092105", "A_61092106",
                "A_6109", "A_6102", "A_61021005",
                "TotalExportFee", "Blank1",
                "A_59122101", "A_59122102", "A_59122103", "A_59122104", "A_59122106", "A_59121111",
                "Blank2",
                "A_61052101", "A_61052102", "A_61052103", "A_61052104", "A_61052105", "A_61052106",

                "A_61041001", "A_82131001", "A_61051001", "A_61092005", "A_61012001", "A_61012006",
                "A_59122222", "A_5912", "A_59122001", "A_61052001", "A_6105",
                "A_61052006", "A_61012003", "A_61092001", "A_61092006", "A_59129999", "A_61050001",
            };

            string accountList = "[" + basic.JoinToString("],[") + "]";
            return accountList;
        }

        /// <summary>
        /// 傳入Report type為Export Fee Report(Mergerd Acct Code)的報表資料，處理運算每個會計科目的值
        /// </summary>
        /// <param name="factoryID">factoryID</param>
        /// <param name="reportData">reportData</param>
        /// <returns>ReportData</returns>
        private ReportData GetValueByAccountNo(string factoryID, ReportData reportData)
        {
            /*
            規則依據：ISP20210275。
            說明：DB會撈出所有會計科目的資料，根據不同工廠，按照以下步驟，請搭配報表範本看
            1. 以Total Export Fee欄位為分界，這之前的欄位，逐一重新運算/替換
            2. 前面欄位全部重新算完之後，才加總Total Export Fee欄位
            3. 後面繼續運算/替換，所有欄位不需要運算/替換的值，不用管他
            4. 最後將不需要顯示的值改成NULL
            5. 注意NULL的運算，NULL + 1 會等於NULL，因此凡是遭遇運算都必須轉型別
             */

            if (factoryID == "PHI")
            {
                reportData.A_61092102 = MyUtility.Convert.GetDecimal(reportData.A_59122102) + MyUtility.Convert.GetDecimal(reportData.A_61092102);
                reportData.A_61022001 = MyUtility.Convert.GetDecimal(reportData.A_61022001) + MyUtility.Convert.GetDecimal(reportData.A_61012001);
                reportData.A_61022006 = MyUtility.Convert.GetDecimal(reportData.A_61022006) + MyUtility.Convert.GetDecimal(reportData.A_61012006);
                reportData.A_61092101 = MyUtility.Convert.GetDecimal(reportData.A_61092101) + MyUtility.Convert.GetDecimal(reportData.A_61052101) + MyUtility.Convert.GetDecimal(reportData.A_61092001) + MyUtility.Convert.GetDecimal(reportData.A_59122101);
                reportData.A_61092106 = MyUtility.Convert.GetDecimal(reportData.A_61092106) + MyUtility.Convert.GetDecimal(reportData.A_61092006) + MyUtility.Convert.GetDecimal(reportData.A_59122106) + MyUtility.Convert.GetDecimal(reportData.A_59121111) + MyUtility.Convert.GetDecimal(reportData.A_61052106);
            }

            if (factoryID == "PH2")
            {
                reportData.A_61022003 = MyUtility.Convert.GetDecimal(reportData.A_61022003) + MyUtility.Convert.GetDecimal(reportData.A_61012003);
                reportData.A_61092101 = MyUtility.Convert.GetDecimal(reportData.A_61092101) + MyUtility.Convert.GetDecimal(reportData.A_61092001) + MyUtility.Convert.GetDecimal(reportData.A_61092001) + MyUtility.Convert.GetDecimal(reportData.A_59122101);
                reportData.A_61092106 = MyUtility.Convert.GetDecimal(reportData.A_61092106) + MyUtility.Convert.GetDecimal(reportData.A_61092006) + MyUtility.Convert.GetDecimal(reportData.A_59122106) + MyUtility.Convert.GetDecimal(reportData.A_59121111) + MyUtility.Convert.GetDecimal(reportData.A_61052106);
            }

            if (factoryID == "ESP")
            {
                reportData.A_61022001 = MyUtility.Convert.GetDecimal(reportData.A_61022001) + MyUtility.Convert.GetDecimal(reportData.A_61012001);
                reportData.A_61022006 = MyUtility.Convert.GetDecimal(reportData.A_61022006) + MyUtility.Convert.GetDecimal(reportData.A_61012006);
                reportData.A_61092101 = MyUtility.Convert.GetDecimal(reportData.A_61092101) + MyUtility.Convert.GetDecimal(reportData.A_61092001) + MyUtility.Convert.GetDecimal(reportData.A_61092001) + MyUtility.Convert.GetDecimal(reportData.A_59122101);
                reportData.A_61092106 = MyUtility.Convert.GetDecimal(reportData.A_61092106) + MyUtility.Convert.GetDecimal(reportData.A_61092006) + MyUtility.Convert.GetDecimal(reportData.A_59122106) + MyUtility.Convert.GetDecimal(reportData.A_59121111) + MyUtility.Convert.GetDecimal(reportData.A_61052106);
            }

            if (factoryID == "SNP")
            {
                reportData.A_61092101 = MyUtility.Convert.GetDecimal(reportData.A_61092101) + MyUtility.Convert.GetDecimal(reportData.A_61092001) + MyUtility.Convert.GetDecimal(reportData.A_59122101);
                reportData.A_61092106 = MyUtility.Convert.GetDecimal(reportData.A_61092106) + MyUtility.Convert.GetDecimal(reportData.A_59122106) + MyUtility.Convert.GetDecimal(reportData.A_59121111) + MyUtility.Convert.GetDecimal(reportData.A_61052106);
            }

            if (factoryID == "SPT")
            {
                reportData.A_61092101 = MyUtility.Convert.GetDecimal(reportData.A_61092101) + MyUtility.Convert.GetDecimal(reportData.A_61092001) + MyUtility.Convert.GetDecimal(reportData.A_59122101);
                reportData.A_61022001 = MyUtility.Convert.GetDecimal(reportData.A_61022001) + MyUtility.Convert.GetDecimal(reportData.A_61012001);
                reportData.A_61022006 = MyUtility.Convert.GetDecimal(reportData.A_61022006) + MyUtility.Convert.GetDecimal(reportData.A_61012006);
                reportData.A_61092106 = MyUtility.Convert.GetDecimal(reportData.A_61092106) + MyUtility.Convert.GetDecimal(reportData.A_59122106) + MyUtility.Convert.GetDecimal(reportData.A_59121111) + MyUtility.Convert.GetDecimal(reportData.A_61052106);
            }

            if (factoryID == "SPR")
            {
                reportData.A_61092101 = MyUtility.Convert.GetDecimal(reportData.A_61092101) + MyUtility.Convert.GetDecimal(reportData.A_61092001) + MyUtility.Convert.GetDecimal(reportData.A_59122101);
                reportData.A_61092105 = MyUtility.Convert.GetDecimal(reportData.A_61092105) + MyUtility.Convert.GetDecimal(reportData.A_61052105);
                reportData.A_61092106 = MyUtility.Convert.GetDecimal(reportData.A_61092106) + MyUtility.Convert.GetDecimal(reportData.A_59122106) + MyUtility.Convert.GetDecimal(reportData.A_59121111) + MyUtility.Convert.GetDecimal(reportData.A_61052106);
            }

            if (factoryID == "SPS")
            {
                reportData.A_61092101 = MyUtility.Convert.GetDecimal(reportData.A_61092101) + MyUtility.Convert.GetDecimal(reportData.A_61092001) + MyUtility.Convert.GetDecimal(reportData.A_59122101);
                reportData.A_61092105 = MyUtility.Convert.GetDecimal(reportData.A_61092105) + MyUtility.Convert.GetDecimal(reportData.A_61092005) + MyUtility.Convert.GetDecimal(reportData.A_61052105);
                reportData.A_61092106 = MyUtility.Convert.GetDecimal(reportData.A_61092106) + MyUtility.Convert.GetDecimal(reportData.A_59122106) + MyUtility.Convert.GetDecimal(reportData.A_59121111) + MyUtility.Convert.GetDecimal(reportData.A_61052106);
            }

            if (factoryID == "HXG")
            {
                reportData.A_6102 = MyUtility.Convert.GetDecimal(reportData.A_6102) + MyUtility.Convert.GetDecimal(reportData.A_61041001);
            }

            if (factoryID == "HZG")
            {
                // 無異動
            }

            // 共通
            reportData.TotalExportFee = MyUtility.Convert.GetDecimal(reportData.A_61022001)
                + MyUtility.Convert.GetDecimal(reportData.A_61022002)
                + MyUtility.Convert.GetDecimal(reportData.A_61022003)
                + MyUtility.Convert.GetDecimal(reportData.A_61022004)
                + MyUtility.Convert.GetDecimal(reportData.A_61022005)
                + MyUtility.Convert.GetDecimal(reportData.A_61022006)
                + MyUtility.Convert.GetDecimal(reportData.A_61092101)
                + MyUtility.Convert.GetDecimal(reportData.A_61092102)
                + MyUtility.Convert.GetDecimal(reportData.A_61092103)
                + MyUtility.Convert.GetDecimal(reportData.A_61092104)
                + MyUtility.Convert.GetDecimal(reportData.A_61092105)
                + MyUtility.Convert.GetDecimal(reportData.A_61092106)
                + MyUtility.Convert.GetDecimal(reportData.A_6102)
                + MyUtility.Convert.GetDecimal(reportData.A_61021005);

            // TotalExportFee之後不需要顯示得改成NULL
            if (factoryID == "PHI")
            {
                reportData.A_59122102 = null; // MyUtility.Convert.GetDecimal(reportData.A_59122102) + MyUtility.Convert.GetDecimal(reportData.A_59122222);
                reportData.A_59122104 = null; // MyUtility.Convert.GetDecimal(reportData.A_59129999);
                reportData.A_59122106 = null; // MyUtility.Convert.GetDecimal(reportData.A_59122106) + MyUtility.Convert.GetDecimal(reportData.A_5912) + MyUtility.Convert.GetDecimal(reportData.A_59122001);
                reportData.A_61052101 = null; // MyUtility.Convert.GetDecimal(reportData.A_61052101) + MyUtility.Convert.GetDecimal(reportData.A_61052001);
                reportData.A_61052106 = null; //MyUtility.Convert.GetDecimal(reportData.A_61052106) + MyUtility.Convert.GetDecimal(reportData.A_6105) + MyUtility.Convert.GetDecimal(reportData.A_61052006);

                reportData.A_61092103 = null;
                reportData.A_61092104 = null;
                reportData.A_61092105 = null;
                reportData.A_6109 = null;
                reportData.A_6102 = null;
                reportData.A_61021005 = null;
                reportData.A_61052104 = null;
                reportData.A_61052105 = null;
            }

            if (factoryID == "PH2")
            {
                reportData.A_59122106 = null; // MyUtility.Convert.GetDecimal(reportData.A_59122106) + MyUtility.Convert.GetDecimal(reportData.A_59122001);
                reportData.A_61052101 = null; //MyUtility.Convert.GetDecimal(reportData.A_61052101) + MyUtility.Convert.GetDecimal(reportData.A_61052001);

                reportData.A_61092102 = null;
                reportData.A_61092103 = null;
                reportData.A_61092104 = null;
                reportData.A_61092105 = null;
                reportData.A_6109 = null;
                reportData.A_6102 = null;
                reportData.A_61021005 = null;
                reportData.A_59122102 = null;
                reportData.A_59122103 = null;
                reportData.A_59122104 = null;
                reportData.A_61052102 = null;
                reportData.A_61052103 = null;
                reportData.A_61052104 = null;
                reportData.A_61052105 = null;
            }

            if (factoryID == "ESP")
            {
                reportData.A_59122102 = null; // MyUtility.Convert.GetDecimal(reportData.A_59122222);
                reportData.A_59122104 = null; // MyUtility.Convert.GetDecimal(reportData.A_59122104) + MyUtility.Convert.GetDecimal(reportData.A_59129999);
                reportData.A_61052106 = null; // MyUtility.Convert.GetDecimal(reportData.A_61052106) + MyUtility.Convert.GetDecimal(reportData.A_61050001);

                reportData.A_61092102 = null;
                reportData.A_61092103 = null;
                reportData.A_61092104 = null;
                reportData.A_61092105 = null;
                reportData.A_6109 = null;
                reportData.A_6102 = null;
                reportData.A_61021005 = null;
                reportData.A_61052102 = null;
                reportData.A_61052105 = null;
            }

            if (factoryID == "SNP")
            {
                reportData.A_61092102 = null;
                reportData.A_61092103 = null;
                reportData.A_61092104 = null;
                reportData.A_61092105 = null;
                reportData.A_6109 = null;
                reportData.A_6102 = null;
                reportData.A_61021005 = null;
                reportData.A_59122102 = null;
                reportData.A_59122103 = null;
                reportData.A_59122104 = null;
                reportData.A_61052102 = null;
                reportData.A_61052103 = null;
                reportData.A_61052104 = null;
                reportData.A_61052105 = null;
            }

            if (factoryID == "SPT")
            {
                reportData.A_61092102 = null;
                reportData.A_61092103 = null;
                reportData.A_61092104 = null;
                reportData.A_61092105 = null;
                reportData.A_6109 = null;
                reportData.A_6102 = null;
                reportData.A_61021005 = null;
                reportData.A_59122102 = null;
                reportData.A_59122103 = null;
                reportData.A_59122104 = null;
                reportData.A_61052102 = null;
                reportData.A_61052103 = null;
                reportData.A_61052104 = null;
                reportData.A_61052105 = null;
            }

            if (factoryID == "SPR")
            {
                reportData.A_61092102 = null;
                reportData.A_61092103 = null;
                reportData.A_61092104 = null;
                reportData.A_6109 = null;
                reportData.A_6102 = null;
                reportData.A_61021005 = null;
                reportData.A_59122102 = null;
                reportData.A_59122103 = null;
                reportData.A_59122104 = null;
                reportData.A_61052102 = null;
                reportData.A_61052103 = null;
                reportData.A_61052104 = null;
            }

            if (factoryID == "SPS")
            {
                reportData.A_61092102 = null;
                reportData.A_61092103 = null;
                reportData.A_61092104 = null;
                reportData.A_6109 = null;
                reportData.A_6102 = null;
                reportData.A_61021005 = null;
                reportData.A_59122102 = null;
                reportData.A_59122103 = null;
                reportData.A_59122104 = null;
                reportData.A_61052102 = null;
                reportData.A_61052103 = null;
                reportData.A_61052104 = null;
            }

            if (factoryID == "HXG")
            {
                reportData.A_61022006 = null;
                reportData.A_61092101 = null;
                reportData.A_61092102 = null;
                reportData.A_61092103 = null;
                reportData.A_61092104 = null;
                reportData.A_61092105 = null;
                reportData.A_61092106 = null;
                reportData.A_6109 = null;
                reportData.A_61021005 = null;
                reportData.A_59122101 = null;
                reportData.A_59122102 = null;
                reportData.A_59122103 = null;
                reportData.A_59122104 = null;
                reportData.A_59122106 = null;
                reportData.A_61052101 = null;
                reportData.A_61052102 = null;
                reportData.A_61052103 = null;
                reportData.A_61052104 = null;
                reportData.A_61052105 = null;
                reportData.A_61052106 = null;
            }

            if (factoryID == "HZG")
            {
                reportData.A_59122106 = null; //MyUtility.Convert.GetDecimal(reportData.A_59122106) + MyUtility.Convert.GetDecimal(reportData.A_82131001);
                reportData.A_61052101 = null; // MyUtility.Convert.GetDecimal(reportData.A_61052101) + MyUtility.Convert.GetDecimal(reportData.A_61051001);

                reportData.A_61022006 = null;
                reportData.A_61092101 = null;
                reportData.A_61092102 = null;
                reportData.A_61092103 = null;
                reportData.A_61092104 = null;
                reportData.A_61092105 = null;
                reportData.A_61092106 = null;
                reportData.A_6109 = null;
                reportData.A_59122101 = null;
                reportData.A_59122102 = null;
                reportData.A_59122103 = null;
                reportData.A_59122104 = null;
                reportData.A_61052102 = null;
                reportData.A_61052103 = null;
                reportData.A_61052104 = null;
                reportData.A_61052105 = null;
                reportData.A_61052106 = null;
            }

            return reportData;
        }

        /// <summary>
        /// Report Type = 5 的報表資料
        /// </summary>
        private class ReportData
        {
            public string Origin { get; set; }

            public string RgCode { get; set; }

            public string Type { get; set; }

            public string ID { get; set; }

            public DateTime? OnBoardDate { get; set; }

            public string Shipper { get; set; }

            public string Foundry { get; set; }

            public string SisFtyAPID { get; set; }

            public string BrandID { get; set; }

            public string Category { get; set; }

            public int OQty { get; set; }

            public string CustCDID { get; set; }

            public string Dest { get; set; }

            public string ShipModeID { get; set; }

            public DateTime? PullOutDate { get; set; }

            public int ShipQty { get; set; }

            public int CtnQty { get; set; }

            public decimal GW { get; set; }

            public decimal CBM { get; set; }

            public string Forwarder { get; set; }

            public string BLNo { get; set; }

            public string CurrencyID { get; set; }

            public decimal? A_61022001 { get; set; }

            public decimal? A_61022002 { get; set; }

            public decimal? A_61022003 { get; set; }

            public decimal? A_61022004 { get; set; }

            public decimal? A_61022005 { get; set; }

            public decimal? A_61022006 { get; set; }

            public decimal? A_61092101 { get; set; }

            public decimal? A_61092102 { get; set; }

            public decimal? A_61092103 { get; set; }

            public decimal? A_61092104 { get; set; }

            public decimal? A_61092105 { get; set; }

            public decimal? A_61092106 { get; set; }

            public decimal? A_6109 { get; set; }

            public decimal? A_6102 { get; set; }

            public decimal? A_61021005 { get; set; }

            public decimal? TotalExportFee { get; set; }

            public string Blank1 { get; set; }

            public decimal? A_59122101 { get; set; }

            public decimal? A_59122102 { get; set; }

            public decimal? A_59122103 { get; set; }

            public decimal? A_59122104 { get; set; }

            public decimal? A_59122106 { get; set; }

            public decimal? A_59121111 { get; set; }

            public string Blank2 { get; set; }

            public decimal? A_61052101 { get; set; }

            public decimal? A_61052102 { get; set; }

            public decimal? A_61052103 { get; set; }

            public decimal? A_61052104 { get; set; }

            public decimal? A_61052105 { get; set; }

            public decimal? A_61052106 { get; set; }

            public decimal? A_61041001 { get; set; }

            public decimal? A_82131001 { get; set; }

            public decimal? A_61051001 { get; set; }

            public decimal? A_61092005 { get; set; }

            public decimal? A_61012001 { get; set; }

            public decimal? A_61012006 { get; set; }

            public decimal? A_59122222 { get; set; }

            public decimal? A_5912 { get; set; }

            public decimal? A_59122001 { get; set; }

            public decimal? A_61052001 { get; set; }

            public decimal? A_6105 { get; set; }

            public decimal? A_61052006 { get; set; }

            public decimal? A_61012003 { get; set; }

            public decimal? A_61092001 { get; set; }

            public decimal? A_61092006 { get; set; }

            public decimal? A_59129999 { get; set; }

            public decimal? A_61050001 { get; set; }
        }
    }
}
