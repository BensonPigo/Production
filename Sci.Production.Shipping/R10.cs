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
    public partial class R10 : Sci.Win.Tems.PrintForm
    {
        DateTime? date1, date2, apApvDate1, apApvDate2;
        string brand, custcd, dest, shipmode, forwarder;
        int reportContent, reportType;
        DataTable printData, accnoData;
        public R10(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            radioButton1.Checked = true;
            radioButton3.Checked = true;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            txtshipmode1.SelectedIndex = -1;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                label2.Text = "Pullout Date";
                txtbrand1.Enabled = true;
                txtcustcd1.Enabled = true;
                radioButton4.Text = "Detail List by SP#";
                radioButton5.Text = "Detail List by SP# by Fee Type";
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                label2.Text = "Ship Date";
                txtbrand1.Enabled = false;
                txtcustcd1.Enabled = false;
                radioButton4.Text = "Detail List by WK#";
                radioButton5.Text = "Detail List by WK# by Fee Type";
            }
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            date1 = dateRange1.Value1;
            date2 = dateRange1.Value2;
            apApvDate1 = dateRange2.Value1;
            apApvDate2 = dateRange2.Value2;
            brand = txtbrand1.Text;
            custcd = txtcustcd1.Text;
            dest = txtcountry1.TextBox1.Text;
            shipmode = txtshipmode1.Text;
            forwarder = txtsubcon1.TextBox1.Text;
            reportContent = radioButton1.Checked ? 1 : 2;
            reportType = radioButton3.Checked ? 1 : radioButton4.Checked ? 2 : 3;
            return base.ValidateInput();
        }

        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            string queryAccount;
            DualResult result;
            if (reportContent == 1) //Garment
            {
                if (reportType == 1 || reportType == 2)    //Export Fee Report or Detail List by SP#
                {
                    if (reportType == 1)    //Export Fee Report
                    {
                        #region 組SQL
                        sqlCmd.Append(@"with tmpGB 
as (
select distinct 'GARMENT' as Type,g.ID,g.Shipper,g.BrandID,IIF(o.Category = 'B','Bulk',IIF(o.Category = 'S','Sample','')) as Category,
isnull(oq.Qty,0) as OQty,g.CustCDID,g.Dest,g.ShipModeID,p.PulloutDate,p.ShipQty,p.CTNQty,
p.GW,p.CBM,g.Forwarder+'-'+isnull(ls.Abb,'') as Forwarder,s.BLNo,se.CurrencyID,se.AccountID,se.Amount
from ShippingAP s
inner join ShareExpense se on se.ShippingAPID = s.ID
inner join GMTBooking g on g.ID = se.InvNo
inner join PackingList p on p.INVNo = g.ID
inner join PackingList_Detail pd on pd.ID = p.ID
left join Orders o on o.ID = pd.OrderID
left join Order_QtyShip oq on oq.Id = pd.OrderID and oq.Seq = pd.OrderShipmodeSeq
left join LocalSupp ls on ls.ID = g.Forwarder
where s.Type = 'EXPORT'");
                        if (!MyUtility.Check.Empty(date1))
                        {
                            sqlCmd.Append(string.Format(" and p.PulloutDate >= '{0}'", Convert.ToDateTime(date1).ToString("d")));
                        }
                        if (!MyUtility.Check.Empty(date2))
                        {
                            sqlCmd.Append(string.Format(" and p.PulloutDate <= '{0}'", Convert.ToDateTime(date2).ToString("d")));
                        }
                        if (!MyUtility.Check.Empty(apApvDate1))
                        {
                            sqlCmd.Append(string.Format(" and s.ApvDate >= '{0}'", Convert.ToDateTime(apApvDate1).ToString("d")));
                        }
                        if (!MyUtility.Check.Empty(apApvDate2))
                        {
                            sqlCmd.Append(string.Format(" and s.ApvDate <= '{0}'", Convert.ToDateTime(apApvDate2).ToString("d")));
                        }
                        if (!MyUtility.Check.Empty(brand))
                        {
                            sqlCmd.Append(string.Format(" and g.BrandID = '{0}'", brand));
                        }
                        if (!MyUtility.Check.Empty(custcd))
                        {
                            sqlCmd.Append(string.Format(" and g.CustCDID = '{0}'", custcd));
                        }
                        if (!MyUtility.Check.Empty(dest))
                        {
                            sqlCmd.Append(string.Format(" and g.Dest = '{0}'", dest));
                        }
                        if (!MyUtility.Check.Empty(shipmode))
                        {
                            sqlCmd.Append(string.Format(" and g.ShipModeID = '{0}'", shipmode));
                        }
                        if (!MyUtility.Check.Empty(forwarder))
                        {
                            sqlCmd.Append(string.Format(" and g.Forwarder = '{0}'", forwarder));
                        }
                        sqlCmd.Append(@"),tmpPL
as (
select distinct 'GARMENT' as Type,p.ID,'' as Shipper,o.BrandID,IIF(o.Category = 'B','Bulk',IIF(o.Category = 'S','Sample','')) as Category,
isnull(oq.Qty,0) as OQty,o.CustCDID,o.Dest,p.ShipModeID,p.PulloutDate,p.ShipQty,p.CTNQty,
p.GW,p.CBM,'' as Forwarder,s.BLNo,se.CurrencyID,se.AccountID,se.Amount
from ShippingAP s
inner join ShareExpense se on se.ShippingAPID = s.ID
inner join PackingList p on p.ID = se.InvNo
inner join PackingList_Detail pd on pd.ID = p.ID
left join Orders o on o.ID = pd.OrderID
left join Order_QtyShip oq on oq.Id = pd.OrderID and oq.Seq = pd.OrderShipmodeSeq
left join [Finance].dbo.AccountNo a on a.ID = se.AccountID
where s.Type = 'EXPORT'");
                        if (!MyUtility.Check.Empty(date1))
                        {
                            sqlCmd.Append(string.Format(" and p.PulloutDate >= '{0}'", Convert.ToDateTime(date1).ToString("d")));
                        }
                        if (!MyUtility.Check.Empty(date2))
                        {
                            sqlCmd.Append(string.Format(" and p.PulloutDate <= '{0}'", Convert.ToDateTime(date2).ToString("d")));
                        }
                        if (!MyUtility.Check.Empty(apApvDate1))
                        {
                            sqlCmd.Append(string.Format(" and s.ApvDate >= '{0}'", Convert.ToDateTime(apApvDate1).ToString("d")));
                        }
                        if (!MyUtility.Check.Empty(apApvDate2))
                        {
                            sqlCmd.Append(string.Format(" and s.ApvDate <= '{0}'", Convert.ToDateTime(apApvDate2).ToString("d")));
                        }
                        if (!MyUtility.Check.Empty(brand))
                        {
                            sqlCmd.Append(string.Format(" and p.BrandID = '{0}'", brand));
                        }
                        if (!MyUtility.Check.Empty(custcd))
                        {
                            sqlCmd.Append(string.Format(" and o.CustCDID = '{0}'", custcd));
                        }
                        if (!MyUtility.Check.Empty(dest))
                        {
                            sqlCmd.Append(string.Format(" and o.Dest = '{0}'", dest));
                        }
                        if (!MyUtility.Check.Empty(shipmode))
                        {
                            sqlCmd.Append(string.Format(" and p.ShipModeID = '{0}'", shipmode));
                        }

                        #endregion
                    }
                    else
                    {   //Detail List by SP#
                        #region 組SQL
                        sqlCmd.Append(@"with tmpGB 
as (
select distinct 'GARMENT' as Type,g.ID,g.Shipper,g.BrandID,IIF(o.Category = 'B','Bulk',IIF(o.Category = 'S','Sample','')) as Category,
pd.OrderID,oq.BuyerDelivery,isnull(oq.Qty,0) as OQty,g.CustCDID,g.Dest,g.ShipModeID,p.ID as PackID, p.PulloutID,p.PulloutDate,p.ShipQty,p.CTNQty,
p.GW,p.CBM,g.Forwarder+'-'+isnull(ls.Abb,'') as Forwarder,s.BLNo,se.CurrencyID,se.AccountID,se.Amount
from ShippingAP s
inner join ShareExpense se on se.ShippingAPID = s.ID
inner join GMTBooking g on g.ID = se.InvNo
inner join PackingList p on p.INVNo = g.ID
inner join PackingList_Detail pd on pd.ID = p.ID
left join Orders o on o.ID = pd.OrderID
left join Order_QtyShip oq on oq.Id = pd.OrderID and oq.Seq = pd.OrderShipmodeSeq
left join LocalSupp ls on ls.ID = g.Forwarder
where s.Type = 'EXPORT'");
                        if (!MyUtility.Check.Empty(date1))
                        {
                            sqlCmd.Append(string.Format(" and p.PulloutDate >= '{0}'", Convert.ToDateTime(date1).ToString("d")));
                        }
                        if (!MyUtility.Check.Empty(date2))
                        {
                            sqlCmd.Append(string.Format(" and p.PulloutDate <= '{0}'", Convert.ToDateTime(date2).ToString("d")));
                        }
                        if (!MyUtility.Check.Empty(apApvDate1))
                        {
                            sqlCmd.Append(string.Format(" and s.ApvDate >= '{0}'", Convert.ToDateTime(apApvDate1).ToString("d")));
                        }
                        if (!MyUtility.Check.Empty(apApvDate2))
                        {
                            sqlCmd.Append(string.Format(" and s.ApvDate <= '{0}'", Convert.ToDateTime(apApvDate2).ToString("d")));
                        }
                        if (!MyUtility.Check.Empty(brand))
                        {
                            sqlCmd.Append(string.Format(" and g.BrandID = '{0}'", brand));
                        }
                        if (!MyUtility.Check.Empty(custcd))
                        {
                            sqlCmd.Append(string.Format(" and g.CustCDID = '{0}'", custcd));
                        }
                        if (!MyUtility.Check.Empty(dest))
                        {
                            sqlCmd.Append(string.Format(" and g.Dest = '{0}'", dest));
                        }
                        if (!MyUtility.Check.Empty(shipmode))
                        {
                            sqlCmd.Append(string.Format(" and g.ShipModeID = '{0}'", shipmode));
                        }
                        if (!MyUtility.Check.Empty(forwarder))
                        {
                            sqlCmd.Append(string.Format(" and g.Forwarder = '{0}'", forwarder));
                        }
                        sqlCmd.Append(@"),tmpPL
as (
select distinct 'GARMENT' as Type,p.ID,'' as Shipper,o.BrandID,IIF(o.Category = 'B','Bulk',IIF(o.Category = 'S','Sample','')) as Category,
pd.OrderID,oq.BuyerDelivery,isnull(oq.Qty,0) as OQty,o.CustCDID,o.Dest,p.ShipModeID,p.ID as PackID, p.PulloutID,p.PulloutDate,p.ShipQty,p.CTNQty,
p.GW,p.CBM,'' as Forwarder,s.BLNo,se.CurrencyID,se.AccountID,se.Amount
from ShippingAP s
inner join ShareExpense se on se.ShippingAPID = s.ID
inner join PackingList p on p.ID = se.InvNo
inner join PackingList_Detail pd on pd.ID = p.ID
left join Orders o on o.ID = pd.OrderID
left join Order_QtyShip oq on oq.Id = pd.OrderID and oq.Seq = pd.OrderShipmodeSeq
left join [Finance].dbo.AccountNo a on a.ID = se.AccountID
where s.Type = 'EXPORT'");
                        if (!MyUtility.Check.Empty(date1))
                        {
                            sqlCmd.Append(string.Format(" and p.PulloutDate >= '{0}'", Convert.ToDateTime(date1).ToString("d")));
                        }
                        if (!MyUtility.Check.Empty(date2))
                        {
                            sqlCmd.Append(string.Format(" and p.PulloutDate <= '{0}'", Convert.ToDateTime(date2).ToString("d")));
                        }
                        if (!MyUtility.Check.Empty(apApvDate1))
                        {
                            sqlCmd.Append(string.Format(" and s.ApvDate >= '{0}'", Convert.ToDateTime(apApvDate1).ToString("d")));
                        }
                        if (!MyUtility.Check.Empty(apApvDate2))
                        {
                            sqlCmd.Append(string.Format(" and s.ApvDate <= '{0}'", Convert.ToDateTime(apApvDate2).ToString("d")));
                        }
                        if (!MyUtility.Check.Empty(brand))
                        {
                            sqlCmd.Append(string.Format(" and p.BrandID = '{0}'", brand));
                        }
                        if (!MyUtility.Check.Empty(custcd))
                        {
                            sqlCmd.Append(string.Format(" and o.CustCDID = '{0}'", custcd));
                        }
                        if (!MyUtility.Check.Empty(dest))
                        {
                            sqlCmd.Append(string.Format(" and o.Dest = '{0}'", dest));
                        }
                        if (!MyUtility.Check.Empty(shipmode))
                        {
                            sqlCmd.Append(string.Format(" and p.ShipModeID = '{0}'", shipmode));
                        }
                        #endregion

                    }
                    queryAccount = string.Format("{0}{1}", sqlCmd.ToString(), @") 
select distinct a.* from (
select AccountID as Accno from tmpGB where AccountID not in ('61022001','61022002','61022003','61022004','61022005','59121111')
union
select AccountID as Accno from tmpPL where AccountID not in ('61022001','61022002','61022003','61022004','61022005','59121111')
) a
order by Accno");
                    result = DBProxy.Current.Select(null, queryAccount, out accnoData);
                    if (!result)
                    {
                        DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                        return failResult;
                    }
                    StringBuilder allAccno = new StringBuilder();
                    allAccno.Append("[61022001],[61022002],[61022003],[61022004],[61022005],[59121111]");
                    foreach (DataRow dr in accnoData.Rows)
                    {
                        allAccno.Append(string.Format(",[{0}]", MyUtility.Convert.GetString(dr["Accno"])));
                    }
                    sqlCmd.Append(string.Format(@"),
tmpAllData
as (
select * from tmpGB
union all
select * from tmpPL)

select * from tmpAllData
PIVOT (SUM(Amount)
FOR AccountID IN ({0})) a", allAccno.ToString()));
                }

                else
                {   //Detail List by SP# by Fee Type
                    #region 組SQL
                    sqlCmd.Append(@"with tmpGB 
as (
select distinct 'GARMENT' as Type,g.ID,g.Shipper,g.BrandID,IIF(o.Category = 'B','Bulk',IIF(o.Category = 'S','Sample','')) as Category,
pd.OrderID,oq.BuyerDelivery,isnull(oq.Qty,0) as OQty,g.CustCDID,g.Dest,g.ShipModeID,p.ID as PackID, p.PulloutID,p.PulloutDate,p.ShipQty,p.CTNQty,
p.GW,p.CBM,g.Forwarder+'-'+isnull(ls.Abb,'') as Forwarder,s.BLNo,se.AccountID+'-'+isnull(a.Name,'') as FeeType,se.Amount,se.CurrencyID,
s.ID as APID,s.CDate,s.ApvDate,s.VoucherID,s.SubType
from ShippingAP s
inner join ShareExpense se on se.ShippingAPID = s.ID
inner join GMTBooking g on g.ID = se.InvNo
inner join PackingList p on p.INVNo = g.ID
inner join PackingList_Detail pd on pd.ID = p.ID
left join Orders o on o.ID = pd.OrderID
left join Order_QtyShip oq on oq.Id = pd.OrderID and oq.Seq = pd.OrderShipmodeSeq
left join LocalSupp ls on ls.ID = g.Forwarder
left join [Finance].dbo.AccountNo a on a.ID = se.AccountID
where s.Type = 'EXPORT'");
                    if (!MyUtility.Check.Empty(date1))
                    {
                        sqlCmd.Append(string.Format(" and p.PulloutDate >= '{0}'", Convert.ToDateTime(date1).ToString("d")));
                    }
                    if (!MyUtility.Check.Empty(date2))
                    {
                        sqlCmd.Append(string.Format(" and p.PulloutDate <= '{0}'", Convert.ToDateTime(date2).ToString("d")));
                    }
                    if (!MyUtility.Check.Empty(apApvDate1))
                    {
                        sqlCmd.Append(string.Format(" and s.ApvDate >= '{0}'", Convert.ToDateTime(apApvDate1).ToString("d")));
                    }
                    if (!MyUtility.Check.Empty(apApvDate2))
                    {
                        sqlCmd.Append(string.Format(" and s.ApvDate <= '{0}'", Convert.ToDateTime(apApvDate2).ToString("d")));
                    }
                    if (!MyUtility.Check.Empty(brand))
                    {
                        sqlCmd.Append(string.Format(" and g.BrandID = '{0}'", brand));
                    }
                    if (!MyUtility.Check.Empty(custcd))
                    {
                        sqlCmd.Append(string.Format(" and g.CustCDID = '{0}'", custcd));
                    }
                    if (!MyUtility.Check.Empty(dest))
                    {
                        sqlCmd.Append(string.Format(" and g.Dest = '{0}'", dest));
                    }
                    if (!MyUtility.Check.Empty(shipmode))
                    {
                        sqlCmd.Append(string.Format(" and g.ShipModeID = '{0}'", shipmode));
                    }
                    if (!MyUtility.Check.Empty(forwarder))
                    {
                        sqlCmd.Append(string.Format(" and g.Forwarder = '{0}'", forwarder));
                    }
                    sqlCmd.Append(@"),
tmpPL
as (
select distinct 'GARMENT' as Type,p.ID,'' as Shipper,o.BrandID,IIF(o.Category = 'B','Bulk',IIF(o.Category = 'S','Sample','')) as Category,
pd.OrderID,oq.BuyerDelivery,isnull(oq.Qty,0) as OQty,o.CustCDID,o.Dest,p.ShipModeID,p.ID as PackID, p.PulloutID,p.PulloutDate,p.ShipQty,p.CTNQty,
p.GW,p.CBM,'' as Forwarder,s.BLNo,se.AccountID+'-'+isnull(a.Name,'') as FeeType,se.Amount,se.CurrencyID,
s.ID as APID,s.CDate,s.ApvDate,s.VoucherID,s.SubType
from ShippingAP s
inner join ShareExpense se on se.ShippingAPID = s.ID
inner join PackingList p on p.ID = se.InvNo
inner join PackingList_Detail pd on pd.ID = p.ID
left join Orders o on o.ID = pd.OrderID
left join Order_QtyShip oq on oq.Id = pd.OrderID and oq.Seq = pd.OrderShipmodeSeq
left join [Finance].dbo.AccountNo a on a.ID = se.AccountID
where s.Type = 'EXPORT'");
                    if (!MyUtility.Check.Empty(date1))
                    {
                        sqlCmd.Append(string.Format(" and p.PulloutDate >= '{0}'", Convert.ToDateTime(date1).ToString("d")));
                    }
                    if (!MyUtility.Check.Empty(date2))
                    {
                        sqlCmd.Append(string.Format(" and p.PulloutDate <= '{0}'", Convert.ToDateTime(date2).ToString("d")));
                    }
                    if (!MyUtility.Check.Empty(apApvDate1))
                    {
                        sqlCmd.Append(string.Format(" and s.ApvDate >= '{0}'", Convert.ToDateTime(apApvDate1).ToString("d")));
                    }
                    if (!MyUtility.Check.Empty(apApvDate2))
                    {
                        sqlCmd.Append(string.Format(" and s.ApvDate <= '{0}'", Convert.ToDateTime(apApvDate2).ToString("d")));
                    }
                    if (!MyUtility.Check.Empty(brand))
                    {
                        sqlCmd.Append(string.Format(" and p.BrandID = '{0}'", brand));
                    }
                    if (!MyUtility.Check.Empty(custcd))
                    {
                        sqlCmd.Append(string.Format(" and o.CustCDID = '{0}'", custcd));
                    }
                    if (!MyUtility.Check.Empty(dest))
                    {
                        sqlCmd.Append(string.Format(" and o.Dest = '{0}'", dest));
                    }
                    if (!MyUtility.Check.Empty(shipmode))
                    {
                        sqlCmd.Append(string.Format(" and p.ShipModeID = '{0}'", shipmode));
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
            {   //Row Material
                if (reportType == 1 || reportType == 2)    //Export Fee Report or Detail List by SP#
                {
                    if (reportType == 1)    //Export Fee Report
                    {
                        #region 組SQL
                        sqlCmd.Append(@"with tmpMaterialData
as (
select 'MATERIAL' as Type, f.ID,s.MDivisionID as Shipper,'' as BrandID,'' as Category,
0 as OQty,'' as CustCDID,f.ImportCountry as Dest,f.ShipModeID,f.PortArrival as PulloutDate,0 as ShipQty,
0 as CTNQty,f.WeightKg as GW,f.Cbm as CBM,f.Forwarder+'-'+isnull(ls.Abb,'') as Forwarder,f.Blno as BLNo,se.CurrencyID,se.AccountID,
se.Amount
from ShippingAP s
inner join ShareExpense se on se.ShippingAPID = s.ID
inner join FtyExport f on f.ID = se.InvNo
left join LocalSupp ls on ls.ID = f.Forwarder
where s.Type = 'EXPORT'");
                        #endregion
                    }
                    else
                    {   //Detail List by SP#
                        #region 組SQL
                        sqlCmd.Append(@"with tmpMaterialData
as (
select 'MATERIAL' as Type, f.ID,s.MDivisionID as Shipper,'' as BrandID,'' as Category,'' as OrderID, null as BuyerDelivery,
0 as OQty,'' as CustCDID,f.ImportCountry as Dest,f.ShipModeID,'' as PackID,'' as PulloutID,f.PortArrival as PulloutDate,
0 as ShipQty,0 as CTNQty,f.WeightKg as GW,f.Cbm as CBM,f.Forwarder+'-'+isnull(ls.Abb,'') as Forwarder,f.Blno as BLNo,
se.CurrencyID,se.AccountID,se.Amount
from ShippingAP s
inner join ShareExpense se on se.ShippingAPID = s.ID
inner join FtyExport f on f.ID = se.InvNo
left join LocalSupp ls on ls.ID = f.Forwarder
where s.Type = 'EXPORT'");
                        #endregion
                    }
                    #region 組條件
                    if (!MyUtility.Check.Empty(date1))
                    {
                        sqlCmd.Append(string.Format(" and f.PortArrival >= '{0}'", Convert.ToDateTime(date1).ToString("d")));
                    }
                    if (!MyUtility.Check.Empty(date2))
                    {
                        sqlCmd.Append(string.Format(" and f.PortArrival <= '{0}'", Convert.ToDateTime(date2).ToString("d")));
                    }
                    if (!MyUtility.Check.Empty(apApvDate1))
                    {
                        sqlCmd.Append(string.Format(" and s.ApvDate >= '{0}'", Convert.ToDateTime(apApvDate1).ToString("d")));
                    }
                    if (!MyUtility.Check.Empty(apApvDate2))
                    {
                        sqlCmd.Append(string.Format(" and s.ApvDate <= '{0}'", Convert.ToDateTime(apApvDate2).ToString("d")));
                    }
                    if (!MyUtility.Check.Empty(dest))
                    {
                        sqlCmd.Append(string.Format(" and f.ImportCountry = '{0}'", dest));
                    }
                    if (!MyUtility.Check.Empty(shipmode))
                    {
                        sqlCmd.Append(string.Format(" and f.ShipModeID = '{0}'", shipmode));
                    }
                    if (!MyUtility.Check.Empty(forwarder))
                    {
                        sqlCmd.Append(string.Format(" and f.Forwarder = '{0}'", forwarder));
                    }
                    #endregion
                    queryAccount = string.Format("{0}{1}", sqlCmd.ToString(), @") 
select distinct a.* from (
select Accountid as Accno from tmpMaterialData where AccountID not in ('61012001','61012002','61012003','61012004','61012005','59121111')) a
order by Accno");
                    result = DBProxy.Current.Select(null, queryAccount, out accnoData);
                    if (!result)
                    {
                        DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                        return failResult;
                    }
                    StringBuilder allAccno = new StringBuilder();
                    allAccno.Append("[61012001],[61012002],[61012003],[61012004],[61012005],[59121111]");
                    foreach (DataRow dr in accnoData.Rows)
                    {
                        allAccno.Append(string.Format(",[{0}]", MyUtility.Convert.GetString(dr["Accno"])));
                    }
                    sqlCmd.Append(string.Format(@")
select * from tmpMaterialData
PIVOT (SUM(Amount)
FOR AccountID IN ({0})) a", allAccno.ToString()));
                }
                else
                {   //Detail List by SP# by Fee Type
                    #region 組SQL
                    sqlCmd.Append(@"select 'MATERIAL' as Type, f.ID,s.MDivisionID as Shipper,'' as BrandID,'' as Category,'' as OrderID, null as BuyerDelivery,
0 as OQty,'' as CustCDID,f.ImportCountry as Dest,f.ShipModeID,'' as PackID,'' as PulloutID,f.PortArrival as PulloutDate,
0 as ShipQty,0 as CTNQty,f.WeightKg as GW,f.Cbm as CBM,f.Forwarder+'-'+isnull(ls.Abb,'') as Forwarder,f.Blno as BLNo,
se.AccountID+'-'+isnull(a.Name,'') as FeeType,se.Amount,se.CurrencyID,s.ID as APID,s.CDate,s.ApvDate,s.VoucherID,s.SubType
from ShippingAP s
inner join ShareExpense se on se.ShippingAPID = s.ID
inner join FtyExport f on f.ID = se.InvNo
left join LocalSupp ls on ls.ID = f.Forwarder
left join [Finance].dbo.AccountNo a on a.ID = se.AccountID
where s.Type = 'EXPORT'");
                    if (!MyUtility.Check.Empty(date1))
                    {
                        sqlCmd.Append(string.Format(" and f.PortArrival >= '{0}'", Convert.ToDateTime(date1).ToString("d")));
                    }
                    if (!MyUtility.Check.Empty(date2))
                    {
                        sqlCmd.Append(string.Format(" and f.PortArrival <= '{0}'", Convert.ToDateTime(date2).ToString("d")));
                    }
                    if (!MyUtility.Check.Empty(apApvDate1))
                    {
                        sqlCmd.Append(string.Format(" and s.ApvDate >= '{0}'", Convert.ToDateTime(apApvDate1).ToString("d")));
                    }
                    if (!MyUtility.Check.Empty(apApvDate2))
                    {
                        sqlCmd.Append(string.Format(" and s.ApvDate <= '{0}'", Convert.ToDateTime(apApvDate2).ToString("d")));
                    }
                    if (!MyUtility.Check.Empty(dest))
                    {
                        sqlCmd.Append(string.Format(" and f.ImportCountry = '{0}'", dest));
                    }
                    if (!MyUtility.Check.Empty(shipmode))
                    {
                        sqlCmd.Append(string.Format(" and f.ShipModeID = '{0}'", shipmode));
                    }
                    if (!MyUtility.Check.Empty(forwarder))
                    {
                        sqlCmd.Append(string.Format(" and f.Forwarder = '{0}'", forwarder));
                    }
                    sqlCmd.Append(" order by f.ID");
                    #endregion
                }
            }
            result = DBProxy.Current.Select(null, sqlCmd.ToString(), out printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return Result.True;
        }

        // 產生Excel
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            SetCount(printData.Rows.Count);

            if (printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            MyUtility.Msg.WaitWindows("Starting EXCEL...");
            string strXltName = Sci.Env.Cfg.XltPathDir + (reportType == 1 ? "\\Shipping_R10_ShareExpenseExportFeeReport.xltx" : reportType == 2 ? "\\Shipping_R10_ShareExpenseExportBySP.xltx" : "\\Shipping_R10_ShareExpenseExportBySPByFee.xltx");
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null) return false;
            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];
            if (reportContent == 2)
            {
                worksheet.Cells[1, 2] = "FTY WK#";
                worksheet.Cells[1, 3] = "M";
                if (reportType == 1)
                {
                    worksheet.Cells[1, 10] = "Ship Date";
                }
                else
                {
                    worksheet.Cells[1, 14] = "Ship Date";
                }
            }

            int allColumn = reportType == 1 ? 23 : 27;
            int i = 0;
            if (reportType != 3)
            {
                foreach (DataRow dr in accnoData.Rows)
                {
                    i++;
                    worksheet.Cells[1, allColumn + i] = MyUtility.GetValue.Lookup(string.Format("select Name from [Finance].dbo.AccountNo where ID = '{0}'", MyUtility.Convert.GetString(dr["Accno"])));
                }
                worksheet.Cells[1, allColumn + i + 1] = "Total Export Fee";
            }
            string excelSumCol = PublicPrg.Prgs.GetExcelEnglishColumnName(allColumn + i);
            string excelColumn = PublicPrg.Prgs.GetExcelEnglishColumnName(allColumn + i + 1);
            if (reportType == 1 || reportType == 2)
            {
                //填內容值
                int intRowsStart = 2;
                object[,] objArray = new object[1, allColumn + i + 1];
                foreach (DataRow dr in printData.Rows)
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
                    if (reportType == 1)
                    {
                        objArray[0, 17] = MyUtility.Check.Empty(dr[17]) ? 0 : dr[17];
                        objArray[0, 18] = MyUtility.Check.Empty(dr[18]) ? 0 : dr[18];
                        objArray[0, 19] = MyUtility.Check.Empty(dr[19]) ? 0 : dr[19];
                        objArray[0, 20] = MyUtility.Check.Empty(dr[20]) ? 0 : dr[20];
                        objArray[0, 21] = MyUtility.Check.Empty(dr[21]) ? 0 : dr[21];
                        objArray[0, 22] = MyUtility.Check.Empty(dr[22]) ? 0 : dr[22];
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
                    }
                    i = 0;
                    foreach (DataRow ddr in accnoData.Rows)
                    {
                        i++;
                        objArray[0, allColumn - 1 + i] = MyUtility.Check.Empty(dr[allColumn - 1 + i]) ? 0 : dr[allColumn - 1 + i];
                    }
                    objArray[0, allColumn + i] = string.Format("=SUM({2}{0}:{1}{0})", intRowsStart, excelSumCol, reportType == 1 ? "R" : "V");
                    worksheet.Range[String.Format("A{0}:{1}{0}", intRowsStart, excelColumn)].Value2 = objArray;
                    intRowsStart++;
                }
            }
            else
            {
                //填內容值
                int intRowsStart = 2;
                object[,] objArray = new object[1, 28];
                foreach (DataRow dr in printData.Rows)
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

                    worksheet.Range[String.Format("A{0}:AB{0}", intRowsStart)].Value2 = objArray;
                    intRowsStart++;
                }
            }

            excel.Cells.EntireColumn.AutoFit();
            excel.Cells.EntireRow.AutoFit();
            MyUtility.Msg.WaitClear();
            excel.Visible = true;
            return true;
        }
    }
}
