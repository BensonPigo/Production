﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Linq;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// R10
    /// </summary>
    public partial class R10 : Sci.Win.Tems.PrintForm
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
        private DataTable accnoData;

        /// <summary>
        /// R10
        /// </summary>
        /// <param name="menuitem">menuitem</param>
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
                this.dateOnBoardDate.Enabled = false;
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
            this.reportType = this.radioExportFeeReport.Checked ? 1 : this.radioDetailListbySPNo.Checked ? 2 : 3;
            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            string queryAccount;
            DualResult result;

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
    inner join PackingList_Detail pd WITH (NOLOCK) on pd.ID = p.ID
    inner join Orders o WITH (NOLOCK) on o.ID = pd.OrderID
    inner join LocalSupp ls WITH (NOLOCK) on ls.ID = g.Forwarder 
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
    inner join PackingList_Detail pd WITH (NOLOCK) on pd.ID = p.ID
    inner join Orders o WITH (NOLOCK) on o.ID = pd.OrderID
    inner join SciFMS_AccountNo a  WITH (NOLOCK)  on a.ID = se.AccountID
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
    from ShippingAP s WITH (NOLOCK) 
    inner join ShareExpense se WITH (NOLOCK) on se.ShippingAPID = s.ID and se.Junk = 0
    inner join GMTBooking g WITH (NOLOCK) on g.ID = se.InvNo
    inner join PackingList p WITH (NOLOCK) on p.INVNo = g.ID
    inner join PackingList_Detail pd WITH (NOLOCK) on pd.ID = p.ID
    inner join Orders o WITH (NOLOCK) on o.ID = pd.OrderID
    inner join Order_QtyShip oq WITH (NOLOCK) on pd.OrderID=oq.Id and oq.Seq = pd.OrderShipmodeSeq
    inner join LocalSupp ls WITH (NOLOCK) on ls.ID = g.Forwarder
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
    from ShippingAP s WITH (NOLOCK) 
    inner join ShareExpense se WITH (NOLOCK) on se.ShippingAPID = s.ID and se.Junk = 0
    inner join PackingList p WITH (NOLOCK) on p.ID = se.InvNo
    inner join PackingList_Detail pd WITH (NOLOCK) on pd.ID = p.ID
    inner join Orders o WITH (NOLOCK) on o.ID = pd.OrderID
    inner join Order_QtyShip oq WITH (NOLOCK) on oq.Id = pd.OrderID 
    inner join SciFMS_AccountNo a  WITH (NOLOCK)  on a.ID = se.AccountID
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


select [type]
	, id
	, OnBoardDate
	, Shipper
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
group by type,id,OnBoardDate,Shipper,BrandID,Category.value,CustCDID,
Dest,ShipModeID,PulloutDate,Forwarder,BLNo,CurrencyID
,AccountID,Amount

select 
	a.[type]
	, a.id
	, a.OnBoardDate
	, a.Shipper
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

--temp3 detail List by SP#
select [type]
	,id
	,OnBoardDate
	,Shipper
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
group by type,id,OnBoardDate,Shipper,BrandID,Category,CustCDID,
Dest,ShipModeID,PulloutDate,Forwarder,BLNo,CurrencyID,orderID ,BuyerDelivery,packingid,PulloutID,Rate,AccountID,Amount

-- 取得TOTAL CBM, 用來計算比例
select a.id	,sum(a.CBM) TotalCBM
into #tmpTotoalCBM
from #temp3 a
where a.shipmodeID in ('SEA','S-A/P','S-A/C')
group by a.id

-- 取得TOTAL GW, 用來計算比例
select a.id	,sum(a.gw) TotalGW
into #tmpTotoalGW
from #temp3 a
where a.shipmodeID in ('A/C', 'A/P', 'A/P-C', 'E/C', 'E/P', 'E/P-C')
group by a.id

-- group by GB#+SP# 依TotalCBM, Total GW比例來取得對應的Amount值
select * 
into #temp4
from (
	select a.[type]
		,a.id
		,a.OnBoardDate
		,a.Shipper
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
                else
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
    from ShippingAP s WITH (NOLOCK) 
    inner join ShareExpense se WITH (NOLOCK) on se.ShippingAPID = s.ID and se.Junk = 0
    inner join GMTBooking g WITH (NOLOCK) on g.ID = se.InvNo
    inner join PackingList p WITH (NOLOCK) on p.INVNo = g.ID
    inner join PackingList_Detail pd WITH (NOLOCK) on pd.ID = p.ID
    left join Orders o WITH (NOLOCK) on o.ID = pd.OrderID
    left join Order_QtyShip oq WITH (NOLOCK) on oq.Id = pd.OrderID and oq.Seq = pd.OrderShipmodeSeq
    left join LocalSupp ls WITH (NOLOCK) on ls.ID = g.Forwarder
    left join SciFMS_AccountNo a WITH (NOLOCK)  on a.ID = se.AccountID
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
    from ShippingAP s WITH (NOLOCK) 
    inner join ShareExpense se WITH (NOLOCK) on se.ShippingAPID = s.ID and se.Junk = 0
    inner join PackingList p WITH (NOLOCK) on p.ID = se.InvNo
    inner join PackingList_Detail pd WITH (NOLOCK) on pd.ID = p.ID
    left join Orders o WITH (NOLOCK) on o.ID = pd.OrderID
    left join Order_QtyShip oq WITH (NOLOCK) on oq.Id = pd.OrderID and oq.Seq = pd.OrderShipmodeSeq
    left join SciFMS_AccountNo a  WITH (NOLOCK)  on a.ID = se.AccountID
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

            result = DBProxy.Current.Select(null, sqlCmd.ToString(), out this.printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return Result.True;
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
            string strXltName = Sci.Env.Cfg.XltPathDir + (this.reportType == 1 ? "\\Shipping_R10_ShareExpenseExportFeeReport.xltx" : this.reportType == 2 ? "\\Shipping_R10_ShareExpenseExportBySP.xltx" : "\\Shipping_R10_ShareExpenseExportBySPByFee.xltx");
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return false;
            }

            DataTable tb_onBoardDate = new DataTable();
            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];
            if (this.reportContent == 2)
            {
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
                tb_onBoardDate = this.printData.Copy();
                for (int f = 0; f < tb_onBoardDate.Columns.Count; f++)
                {
                    if (!tb_onBoardDate.Columns[f].ColumnName.Equals("OnBoardDate"))
                    {
                        tb_onBoardDate.Columns.RemoveAt(f);
                        f--;
                    }
                }

                this.printData.Columns.RemoveAt(2);
            }

            if (this.reportType == 1 || this.reportType == 2)
            {
                int allColumn = this.reportType == 1 ? 23 : 27;

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
                        // objArray[0, 19] = MyUtility.Check.Empty(dr[19]) ? 0 : dr[19];
                        // objArray[0, 20] = $"=S{intRowsStart}+T{intRowsStart}";
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
                        objArray[0, 16] = dr[16];
                        objArray[0, 17] = dr[17];
                        objArray[0, 18] = dr[18];
                        objArray[0, 19] = dr[19];
                        objArray[0, 20] = dr[20];
                        objArray[0, 21] = MyUtility.Check.Empty(dr[21]) ? 0 : dr[21];
                        objArray[0, 22] = MyUtility.Check.Empty(dr[22]) ? 0 : dr[22];
                        objArray[0, 23] = MyUtility.Check.Empty(dr[23]) ? 0 : dr[23];
                        objArray[0, 24] = MyUtility.Check.Empty(dr[24]) ? 0 : dr[24];
                        objArray[0, 25] = MyUtility.Check.Empty(dr[25]) ? 0 : dr[25];
                        objArray[0, 26] = MyUtility.Check.Empty(dr[26]) ? 0 : dr[26];

                        // 多增加的AccountID, 必須要動態的填入欄位值!
                        if (counts > 0)
                        {
                            for (int c = 1; c <= counts; c++)
                            {
                                if (MyUtility.Convert.GetString(dr.Table.Columns[26 + c].ColumnName).Contains("5912"))
                                {
                                    if (MyUtility.Check.Empty(sumCol5912start))
                                    {
                                        sumCol5912start = PublicPrg.Prgs.GetExcelEnglishColumnName(27 + c);
                                    }
                                }

                                if (MyUtility.Convert.GetString(dr.Table.Columns[26 + c].ColumnName).EqualString("5912-Total"))
                                {
                                    if (MyUtility.Check.Empty(sumCol5912))
                                    {
                                        sumCol5912 = PublicPrg.Prgs.GetExcelEnglishColumnName(26 + c);
                                        sumCol5912TTL = PublicPrg.Prgs.GetExcelEnglishColumnName(27 + c);
                                    }

                                    objArray[0, 26 + c] = $"=AA{intRowsStart}+SUM({sumCol5912start}{intRowsStart}:{sumCol5912}{intRowsStart})";
                                }
                                else if (MyUtility.Convert.GetString(dr.Table.Columns[26 + c].ColumnName).EqualString("6105-Total"))
                                {
                                    if (MyUtility.Check.Empty(sumCol6105))
                                    {
                                        sumCol6105 = PublicPrg.Prgs.GetExcelEnglishColumnName(26 + c);
                                        sumCol6105TTL = PublicPrg.Prgs.GetExcelEnglishColumnName(27 + c);
                                    }

                                    objArray[0, 26 + c] = $"=SUM({first6105Column}{intRowsStart}:{sumCol6105}{intRowsStart})";
                                }
                                else
                                {
                                    objArray[0, 26 + c] = MyUtility.Check.Empty(dr[26 + c]) ? 0 : dr[26 + c];
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

                    objArray[0, allColumn + this.accnoData.Rows.Count] = string.Format("=SUM({2}{0}:{1}{0}) {3} {4}", intRowsStart, excelSumCol, this.reportType == 1 ? "R" : "V", sc1, sc2);
                    worksheet.Range[string.Format("A{0}:{1}{0}", intRowsStart, excelColumn)].Value2 = objArray;
                    intRowsStart++;
                }
            }
            else
            {
                // 匯率選擇 Fixed, KPI, 各費用欄位名稱加上 (USD)
                if (!MyUtility.Check.Empty(this.comboRateType.SelectedValue))
                {
                    worksheet.Cells[1, 22] = worksheet.Cells[1, 22].Value + "\r\n(USD)";
                }


                // 填內容值
                int intRowsStart = 2;
                object[,] objArray = new object[1, 29];
                foreach (DataRow dr in this.printData.Rows)
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

                    worksheet.Range[string.Format("A{0}:AB{0}", intRowsStart)].Value2 = objArray;
                    intRowsStart++;
                }
            }

            // mantis9831 增加On Board Date，因為只針對Garment所以在excel產生後插入
            if (this.reportContent == 1)
            {
                Microsoft.Office.Interop.Excel.Range range = (Microsoft.Office.Interop.Excel.Range)worksheet.get_Range("C1", Missing.Value);
                range.EntireColumn.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftToRight, Microsoft.Office.Interop.Excel.XlInsertFormatOrigin.xlFormatFromRightOrBelow);
                worksheet.Cells[1, 3] = "On Board Date";
                range = (Microsoft.Office.Interop.Excel.Range)worksheet.get_Range("C2", "C" + (this.printData.Rows.Count + 1));
                range.EntireColumn.NumberFormat = "yyyy/MM/dd";
                Sci.Utility.Report.ExcelCOM com = new Sci.Utility.Report.ExcelCOM();
                object[,] arrayValues = tb_onBoardDate.ToArray2D();
                range.Value2 = arrayValues;
            }

            excel.Cells.EntireColumn.AutoFit();
            excel.Cells.EntireRow.AutoFit();
            this.HideWaitMessage();

            #region Save & Show Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName(this.reportType == 1 ? "Shipping_R10_ShareExpenseExportFeeReport" : this.reportType == 2 ? "Shipping_R10_ShareExpenseExportBySP" : "Shipping_R10_ShareExpenseExportBySPByFee");
            excel.ActiveWorkbook.SaveAs(strExcelName);
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(worksheet);

            strExcelName.OpenFile();
            #endregion
            return true;
        }
    }
}
