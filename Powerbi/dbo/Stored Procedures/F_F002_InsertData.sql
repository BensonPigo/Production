-- =============================================
-- Author:		Victor.Lin
-- Create date: 2019/06/17
-- Description:	Import  System: Trade Function: GarmentExport R35 Detail 
-- =============================================
CREATE PROCEDURE [dbo].[F_F002_InsertData]
AS
BEGIN


	SELECT *  into #F002
	FROM OPENQUERY ([TRADEDB], 
	'	select 
onboard = convert(varchar(7),gi.ETD),
InvoiceNo=gi.ID,
PulloutDate = max(CDate),
ETD=gi.ETD,
Brand=gi.BrandID,
Shipper,
Currency=gi.CurrencyID,
VoucherID=gi.VoucherID,
orgAmount =(sum( (p.Price +p.Surcharge) * p.ShipQty)) ,
Amount = (sum( (p.Price +p.Surcharge) * p.ShipQty)) * trade.dbo.GetRate(''KP'',gi.ETD,gi.CurrencyID,''USD'', default),
VoucherDate=fv.VoucherDate,
VoucherMonth = convert(varchar(7),fv.VoucherDate),
Factory=o.FactoryID,
MDivisionID=Factory.MDivisionID,
DifferenceDays=DATEDIFF ( day , gi.ETD , fv.VoucherDate ) ,
DifferenceDaysGroup =case 
when DATEDIFF ( day , gi.ETD , fv.VoucherDate ) is null then '''' 
when DATEDIFF ( day , gi.ETD , fv.VoucherDate ) <0 then ''<0'' 
when DATEDIFF ( day , gi.ETD , fv.VoucherDate ) <10 then ''<10''
when DATEDIFF ( day , gi.ETD , fv.VoucherDate ) <30 then ''<30'' 
when DATEDIFF ( day , gi.ETD , fv.VoucherDate ) >=30 then ''>30'' 
else '''' end
from trade.dbo.GarmentInvoice gi with(nolock)
left join trade.dbo.Pullout p with(nolock) on p.InvoiceNo = gi.ID
left join Finance.dbo.Voucher fv with(nolock) on fv.ID = gi.VoucherID
left join trade.dbo.Orders o with(nolock) on p.OrderID=o.ID
left join trade.dbo.Factory with(nolock) on o.FactoryID=Factory.ID
where ETD between DATEADD(yy, DATEDIFF(yy,0,getdate()), 0)   and getDate() and gi.datafrom = ''G''
group by gi.ID,gi.ETD,gi.BrandID,Shipper,gi.CurrencyID,PulloutAmount,gi.Surcharge,gi.VoucherID,fv.VoucherDate,o.FactoryID,Factory.MDivisionID
	')

	SELECT *  into #MDivisionID
	FROM OPENQUERY ([TRADEDB], 
		'
		 Select distinct MDivisionID
		 from trade.dbo.Factory
		 where MDivisionID <> ''''
		
		 ')

		TRUNCATE TABLE F_F002
		TRUNCATE TABLE F_F002_MDivision

		Insert into  F_F002
		select *
		from #F002

		Insert into F_F002_MDivision
		select *
		from #MDivisionID


		drop table #F002
		drop table #MDivisionID

END