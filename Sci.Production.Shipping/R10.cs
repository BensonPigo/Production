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
using System.Runtime.InteropServices;

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
        private string brand;
        private string custcd;
        private string dest;
        private string shipmode;
        private string forwarder;
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
            }
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.date1 = this.datePulloutDate.Value1;
            this.date2 = this.datePulloutDate.Value2;
            this.apApvDate1 = this.dateAPApvDate.Value1;
            this.apApvDate2 = this.dateAPApvDate.Value2;
            this.brand = this.txtbrand.Text;
            this.custcd = this.txtcustcd.Text;
            this.dest = this.txtcountryDestination.TextBox1.Text;
            this.shipmode = this.txtshipmode.Text;
            this.forwarder = this.txtsubconForwarder.TextBox1.Text;
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
                        sqlCmd.Append(@"with tmpGB 
as (
select distinct 'GARMENT' as Type,
g.ID,g.Shipper,g.BrandID,
IIF(o.Category = 'B','Bulk',IIF(o.Category = 'S','Sample','')) as Category,
g.CustCDID,g.Dest,g.ShipModeID,p.PulloutDate,g.Forwarder+'-'+isnull(ls.Abb,'') as Forwarder,
s.BLNo,
se.CurrencyID,
p.OrderID,p.ID as packingID
from ShippingAP s WITH (NOLOCK) 
inner join ShareExpense se WITH (NOLOCK) on se.ShippingAPID = s.ID
inner join GMTBooking g WITH (NOLOCK) on g.ID = se.InvNo
inner join PackingList p WITH (NOLOCK) on p.INVNo = g.ID
inner join PackingList_Detail pd WITH (NOLOCK) on pd.ID = p.ID
inner join Orders o WITH (NOLOCK) on o.ID = pd.OrderID
inner join LocalSupp ls WITH (NOLOCK) on ls.ID = g.Forwarder 
where s.Type = 'EXPORT'");
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

                        sqlCmd.Append(@"),tmpPL
as (
select distinct 'GARMENT' as Type,
p.ID,'' as Shipper,o.BrandID,
IIF(o.Category = 'B','Bulk',IIF(o.Category = 'S','Sample','')) as Category,
o.CustCDID,o.Dest,p.ShipModeID,p.PulloutDate,
'' as Forwarder,
s.BLNo,
se.CurrencyID,
p.OrderID,p.ID as packingID
from ShippingAP s WITH (NOLOCK) 
inner join ShareExpense se WITH (NOLOCK) on se.ShippingAPID = s.ID
inner join PackingList p WITH (NOLOCK) on p.ID = se.InvNo
inner join PackingList_Detail pd WITH (NOLOCK) on pd.ID = p.ID
inner join Orders o WITH (NOLOCK) on o.ID = pd.OrderID
inner join [FinanceEN].dbo.AccountNo a on a.ID = se.AccountID
where s.Type = 'EXPORT'");
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
                        sqlCmd.Append(@"
with tmpGB 
as (
select distinct 'GARMENT' as Type,
g.ID,g.Shipper,g.BrandID,
IIF(o.Category = 'B','Bulk',IIF(o.Category = 'S','Sample','')) as Category,
pd.OrderID,oq.BuyerDelivery,
g.CustCDID,g.Dest,g.ShipModeID,p.ID as packingid, p.PulloutID,p.PulloutDate,
g.Forwarder+'-'+isnull(ls.Abb,'') as Forwarder,
s.BLNo,se.CurrencyID
from ShippingAP s WITH (NOLOCK) 
inner join ShareExpense se WITH (NOLOCK) on se.ShippingAPID = s.ID
inner join GMTBooking g WITH (NOLOCK) on g.ID = se.InvNo
inner join PackingList p WITH (NOLOCK) on p.INVNo = g.ID
inner join PackingList_Detail pd WITH (NOLOCK) on pd.ID = p.ID
inner join Orders o WITH (NOLOCK) on o.ID = pd.OrderID
inner join Order_QtyShip oq WITH (NOLOCK) on p.OrderID=oq.Id
inner join LocalSupp ls WITH (NOLOCK) on ls.ID = g.Forwarder
where s.Type = 'EXPORT'");
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

                        sqlCmd.Append(@"),
tmpPL
as (
select distinct 'GARMENT' as Type,
p.ID,'' as Shipper,o.BrandID,
IIF(o.Category = 'B','Bulk',IIF(o.Category = 'S','Sample','')) as Category,
pd.OrderID,oq.BuyerDelivery,
o.CustCDID,o.Dest,p.ShipModeID,p.ID as packingid, p.PulloutID,p.PulloutDate,
'' as Forwarder,
s.BLNo,se.CurrencyID
from ShippingAP s WITH (NOLOCK) 
inner join ShareExpense se WITH (NOLOCK) on se.ShippingAPID = s.ID
inner join PackingList p WITH (NOLOCK) on p.ID = se.InvNo
inner join PackingList_Detail pd WITH (NOLOCK) on pd.ID = p.ID
inner join Orders o WITH (NOLOCK) on o.ID = pd.OrderID
inner join Order_QtyShip oq WITH (NOLOCK) on oq.Id = pd.OrderID 
inner join [FinanceEN].dbo.AccountNo a on a.ID = se.AccountID
where s.Type = 'EXPORT'");
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

                    sqlCmd.Append(@"),
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
select distinct type,id,shipper,Brandid,category,custcdid,dest,shipmodeid,pulloutdate,
forwarder,s.BLNo,currencyid,orderid,packingid
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


select type,id,Shipper,BrandID,Category,[OQty]=sum(oqs.OQty),CustCDID,
Dest,ShipModeID,PulloutDate,
[ShipQty]=sum(pt.ShipQty),[ctnqty]=sum(pt.ctnqty),[gw]=sum(pt.gw),[CBM]=sum(pt.CBM),Forwarder,BLNo,CurrencyID 
into #temp3
from #temp2 a
outer apply(select sum(qty) as OQty from Order_QtyShip where id=a.OrderID) as oqs
outer apply(select sum(shipqty) as shipqty,sum(CTNQty)as CTNQty,sum(GW) as GW,sum(CBM)as CBM from PackingList where id=a.packingID) as pt
group by type,id,Shipper,BrandID,Category,CustCDID,
Dest,ShipModeID,PulloutDate,Forwarder,BLNo,CurrencyID 

select a.*,b.AccountID,b.Amount 
into #temp4
from #temp3 a
inner join ShareExpense b on a.ID=b.InvNo ");
                    }
                    else
                    {
                        sqlCmd.Append(@"
-----temp2 detail List by SP#
select distinct type,id,shipper,Brandid,category,OrderID,BuyerDelivery,custcdid,dest,shipmodeid,packingid,PulloutID,PulloutDate,
forwarder,s.BLNo,currencyid
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
select type,id,Shipper,BrandID,Category,orderID,BuyerDelivery,[OQty]=sum(oqs.OQty),CustCDID,
Dest,ShipModeID,packingid,PulloutID,PulloutDate,
[ShipQty]=sum(pt.ShipQty),[ctnqty]=sum(pt.ctnqty),[gw]=sum(pt.gw),[CBM]=sum(pt.CBM),Forwarder,BLNo,CurrencyID 
into #temp3
from #temp2 a
outer apply(select sum(qty) as OQty from Order_QtyShip where id=a.OrderID) as oqs
outer apply(select sum(shipqty) as shipqty,sum(CTNQty)as CTNQty,sum(GW) as GW,sum(CBM)as CBM from PackingList where id=a.packingID) as pt
group by type,id,Shipper,BrandID,Category,CustCDID,
Dest,ShipModeID,PulloutDate,Forwarder,BLNo,CurrencyID,orderID ,BuyerDelivery,packingid,PulloutID

select a.*,b.AccountID,b.Amount 
into #temp4
from #temp3 a
inner join ShareExpense b on a.ID=b.InvNo ");
                    }

                     queryAccount = string.Format(
                         "{0}{1}",
                         sqlCmd.ToString(),
                         string.Format(@" 
select distinct a.* from (
select AccountID as Accno from #temp4 where AccountID not in ('61022001','61022002','61022003','61022004','61022005','59121111')
and AccountID <> ''
) a
order by Accno"));
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
                    sqlCmd.Append(@"with tmpGB 
as (
select distinct 'GARMENT' as Type,g.ID,g.Shipper,g.BrandID,IIF(o.Category = 'B','Bulk',IIF(o.Category = 'S','Sample','')) as Category,
pd.OrderID,oq.BuyerDelivery,isnull(oq.Qty,0) as OQty,g.CustCDID,g.Dest,g.ShipModeID,p.ID as PackID, p.PulloutID,p.PulloutDate,p.ShipQty,p.CTNQty,
p.GW,p.CBM,g.Forwarder+'-'+isnull(ls.Abb,'') as Forwarder,s.BLNo,se.AccountID+'-'+isnull(a.Name,'') as FeeType,se.Amount,se.CurrencyID,
s.ID as APID,s.CDate,CONVERT(DATE,s.ApvDate) as ApvDate,s.VoucherID,s.SubType
from ShippingAP s WITH (NOLOCK) 
inner join ShareExpense se WITH (NOLOCK) on se.ShippingAPID = s.ID
inner join GMTBooking g WITH (NOLOCK) on g.ID = se.InvNo
inner join PackingList p WITH (NOLOCK) on p.INVNo = g.ID
inner join PackingList_Detail pd WITH (NOLOCK) on pd.ID = p.ID
left join Orders o WITH (NOLOCK) on o.ID = pd.OrderID
left join Order_QtyShip oq WITH (NOLOCK) on oq.Id = pd.OrderID and oq.Seq = pd.OrderShipmodeSeq
left join LocalSupp ls WITH (NOLOCK) on ls.ID = g.Forwarder
left join [FinanceEN].dbo.AccountNo a on a.ID = se.AccountID
where s.Type = 'EXPORT'");
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

                    sqlCmd.Append(@"),
tmpPL
as (
select distinct 'GARMENT' as Type,p.ID,'' as Shipper,o.BrandID,IIF(o.Category = 'B','Bulk',IIF(o.Category = 'S','Sample','')) as Category,
pd.OrderID,oq.BuyerDelivery,isnull(oq.Qty,0) as OQty,o.CustCDID,o.Dest,p.ShipModeID,p.ID as PackID, p.PulloutID,p.PulloutDate,p.ShipQty,p.CTNQty,
p.GW,p.CBM,'' as Forwarder,s.BLNo,se.AccountID+'-'+isnull(a.Name,'') as FeeType,se.Amount,se.CurrencyID,
s.ID as APID,s.CDate,CONVERT(DATE,s.ApvDate) as ApvDate,s.VoucherID,s.SubType
from ShippingAP s WITH (NOLOCK) 
inner join ShareExpense se WITH (NOLOCK) on se.ShippingAPID = s.ID
inner join PackingList p WITH (NOLOCK) on p.ID = se.InvNo
inner join PackingList_Detail pd WITH (NOLOCK) on pd.ID = p.ID
left join Orders o WITH (NOLOCK) on o.ID = pd.OrderID
left join Order_QtyShip oq WITH (NOLOCK) on oq.Id = pd.OrderID and oq.Seq = pd.OrderShipmodeSeq
left join [FinanceEN].dbo.AccountNo a on a.ID = se.AccountID
where s.Type = 'EXPORT'");
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
                        sqlCmd.Append(@"with tmpMaterialData
as (
select 'MATERIAL' as Type, f.ID,s.MDivisionID as Shipper,'' as BrandID,'' as Category,
0 as OQty,'' as CustCDID,f.ImportCountry as Dest,f.ShipModeID,f.PortArrival as PulloutDate,0 as ShipQty,
0 as CTNQty,f.WeightKg as GW,f.Cbm as CBM,f.Forwarder+'-'+isnull(ls.Abb,'') as Forwarder,f.Blno as BLNo,se.CurrencyID,[AccountID]= iif(se.AccountID='','Empty',se.AccountID),
se.Amount
from ShippingAP s WITH (NOLOCK) 
inner join ShareExpense se WITH (NOLOCK) on se.ShippingAPID = s.ID
inner join FtyExport f WITH (NOLOCK) on f.ID = se.InvNo
left join LocalSupp ls WITH (NOLOCK) on ls.ID = f.Forwarder
where s.Type = 'EXPORT'");
                        #endregion
                    }
                    else
                    { // Detail List by SP#
                        #region 組SQL
                        sqlCmd.Append(@"with tmpMaterialData
as (
select 'MATERIAL' as Type, f.ID,s.MDivisionID as Shipper,'' as BrandID,'' as Category,'' as OrderID, null as BuyerDelivery,
0 as OQty,'' as CustCDID,f.ImportCountry as Dest,f.ShipModeID,'' as PackID,'' as PulloutID,f.PortArrival as PulloutDate,
0 as ShipQty,0 as CTNQty,f.WeightKg as GW,f.Cbm as CBM,f.Forwarder+'-'+isnull(ls.Abb,'') as Forwarder,f.Blno as BLNo,
se.CurrencyID,[AccountID]= iif(se.AccountID='','Empty',se.AccountID),se.Amount
from ShippingAP s WITH (NOLOCK) 
inner join ShareExpense se WITH (NOLOCK) on se.ShippingAPID = s.ID
inner join FtyExport f WITH (NOLOCK) on f.ID = se.InvNo
left join LocalSupp ls WITH (NOLOCK) on ls.ID = f.Forwarder
where s.Type = 'EXPORT'");
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
                        string.Format(@") 
select distinct a.* from (
select Accountid as Accno from tmpMaterialData where AccountID not in ('61012001','61012002','61012003','61012004','61012005','59121111')
and AccountID <> ''
) a
order by Accno"));
                    result = DBProxy.Current.Select(null, queryAccount, out this.accnoData);
                    if (!result)
                    {
                        DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                        return failResult;
                    }

                    StringBuilder allAccno = new StringBuilder();
                    allAccno.Append("[61012001],[61012002],[61012003],[61012004],[61012005],[59121111]");
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
                    sqlCmd.Append(@"select 'MATERIAL' as Type, f.ID,s.MDivisionID as Shipper,'' as BrandID,'' as Category,'' as OrderID, null as BuyerDelivery,
0 as OQty,'' as CustCDID,f.ImportCountry as Dest,f.ShipModeID,'' as PackID,'' as PulloutID,f.PortArrival as PulloutDate,
0 as ShipQty,0 as CTNQty,f.WeightKg as GW,f.Cbm as CBM,f.Forwarder+'-'+isnull(ls.Abb,'') as Forwarder,f.Blno as BLNo,
se.AccountID+'-'+isnull(a.Name,'') as FeeType,se.Amount,se.CurrencyID,s.ID as APID,s.CDate,CONVERT(DATE,s.ApvDate) as ApvDate,s.VoucherID,s.SubType
from ShippingAP s WITH (NOLOCK) 
inner join ShareExpense se WITH (NOLOCK) on se.ShippingAPID = s.ID
inner join FtyExport f WITH (NOLOCK) on f.ID = se.InvNo
left join LocalSupp ls WITH (NOLOCK) on ls.ID = f.Forwarder
left join [FinanceEN].dbo.AccountNo a on a.ID = se.AccountID
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

            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];
            excel.Visible = true;
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

            int allColumn = this.reportType == 1 ? 23 : 27;
            int counts = this.accnoData.Rows.Count;
            int i = 0;
            if (this.reportType != 3)
            {
                foreach (DataRow dr in this.accnoData.Rows)
                {
                    i++;
                    worksheet.Cells[1, allColumn + i] = MyUtility.GetValue.Lookup(string.Format("select Name from [FinanceEN].dbo.AccountNo where ID = '{0}'", MyUtility.Convert.GetString(dr["Accno"])));
                }

                worksheet.Cells[1, allColumn + i + 1] = "Total Export Fee";
            }

            string excelSumCol = PublicPrg.Prgs.GetExcelEnglishColumnName(allColumn + i);
            string excelColumn = PublicPrg.Prgs.GetExcelEnglishColumnName(allColumn + i + 1);
            if (this.reportType == 1 || this.reportType == 2)
            {
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
                        if (counts > 0)
                        {
                            for (int t = 1; t <= counts; t++)
                            {
                                objArray[0, 22 + t] = MyUtility.Check.Empty(dr[22 + t]) ? 0 : dr[22 + t];
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
                        objArray[0, 27] = MyUtility.Check.Empty(dr[27]) ? 0 : dr[27];

                        // if (counts > 0)
                        // {
                        //    for (int t = 1; t <= counts; t++)
                        //    {
                        //        objArray[0, 27 + t] = MyUtility.Check.Empty(dr[27 + t]) ? 0 : dr[27 + t];
                        //    }
                        // }
                    }

                    i = 0;
                    foreach (DataRow ddr in this.accnoData.Rows)
                    {
                        i++;
                        objArray[0, allColumn - (1 + i)] = MyUtility.Check.Empty(dr[allColumn - (1 + i)]) ? 0 : dr[allColumn - (1 + i)];
                    }

                    objArray[0, allColumn + i] = string.Format("=SUM({2}{0}:{1}{0})", intRowsStart, excelSumCol, this.reportType == 1 ? "R" : "V");
                    worksheet.Range[string.Format("A{0}:{1}{0}", intRowsStart, excelColumn)].Value2 = objArray;
                    intRowsStart++;
                }
            }
            else
            {
                // 填內容值
                int intRowsStart = 2;
                object[,] objArray = new object[1, 28];
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
                    objArray[0, 27] = dr["SubType"];

                    worksheet.Range[string.Format("A{0}:AB{0}", intRowsStart)].Value2 = objArray;
                    intRowsStart++;
                }
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
