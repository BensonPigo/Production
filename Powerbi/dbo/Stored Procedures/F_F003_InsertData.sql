-- =============================================
-- Author:		Victor.Lin
-- Create date: 2019/06/18
-- Description:	Import  System: Trade Function: GarmentExport R34 Detail 
-- =============================================
CREATE PROCEDURE [dbo].[F_F003_InsertData]

AS
BEGIN

	SELECT *  into #F003
	FROM OPENQUERY ([TRADEDB], 
	'	
	    select PulloutMonth=Convert(Varchar(7),CDate),
            CDate,
            InvoiceNo= pullout.InvoiceNo,
            ETD,
            OrdereID= orders.ID, 
            CurrencyID= GarmentInvoice.CurrencyID,
            Rate=trade.dbo.GetRate(''KP'', ETD, GarmentInvoice.CurrencyID,''USD'', default) ,
            orginAmount=(price+pullout.Surcharge) * ShipQty ,
            Amount=(price+pullout.Surcharge) * ShipQty * trade.dbo.GetRate(''KP'', ETD, GarmentInvoice.CurrencyID,''USD'', default),
            OnboardMonth=isnull(Convert(Varchar(7),GarmentInvoice.etd),null) ,
			Factory = orders.FactoryID ,
            MDivision=Factory.MDivisionID ,
			DifferenceDays=DATEDIFF ( day , Pullout.CDate , ETD ) ,
			DifferenceDaysGroup =case 
			when DATEDIFF ( day ,CDate , ETD ) is null then '''' 
			when DATEDIFF ( day ,CDate , ETD) <0 then ''<0'' 
			when DATEDIFF ( day ,CDate , ETD ) <10 then ''<10''
			when DATEDIFF ( day ,CDate , ETD ) <30 then ''<30'' 
			when DATEDIFF ( day ,CDate , ETD ) >=30 then ''>30'' 
			else '''' end,
			Brand=GarmentInvoice.BrandID                  
    from trade.dbo.Pullout 
    left join trade.dbo.orders on Pullout.OrderID=orders.ID 
    left join trade.dbo.Factory on orders.FactoryID=Factory.ID
    left join trade.dbo.GarmentInvoice on GarmentInvoice.ID=pullout.InvoiceNo 
    where GarmentInvoice.datafrom = ''G'' and
    Pullout.CDate between  DATEADD(yy, DATEDIFF(yy,0,getdate()), 0)   and getDate()
	')

	SELECT *  into #MDivisionID
	FROM OPENQUERY ([TRADEDB], 
		'
		 Select distinct MDivisionID
		 from trade.dbo.Factory
		 where MDivisionID <> ''''
		
		 ')

		TRUNCATE TABLE F_F003
		TRUNCATE TABLE F_F003_MDivision

		Insert into  F_F003
		select *
		from #F003


		Insert into F_F003_MDivision
		select *
		from #MDivisionID


		drop table #F003
		drop table #MDivisionID
END