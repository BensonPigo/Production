-- =============================================
-- Author:		Jeff
-- Create date: 2023/02/17
-- Description:	From trade 只取需要部份 for < WH P01 Material Compare >
-- =============================================
Create FUNCTION [dbo].[GetECFA_Refno]
(
	@OrderID varchar(13),
	@SuppID Varchar(6),
	@SCIRefno Varchar(30)
)
RETURNS bit
AS
BEGIN
	
	--1. 工廠可走ECFA
	--2. 內銷單(出貨地與進口別為一致時)
	--3. 物料需是台灣廠商
	--4. 物料需設定有apply ECFA
	return iif(exists(
		select 1
		from dbo.Orders with(nolock) 
		inner join Production.dbo.factory with(nolock) on factory.id = orders.FactoryID
		inner join Production.dbo.Supp with(nolock) on Supp.ID = @SuppID
		inner join Production.dbo.Fabric_Supp with(nolock) on Fabric_Supp.suppid = Supp.ID and Fabric_Supp.SCIRefno = @SCIRefno
		where Orders.ID = @OrderID
			and Factory.IsECFA = 1 and orders.Dest = Factory.CountryID
			and Supp.CountryID = 'TW' and Fabric_Supp.isECFA = 1
	), 1 , 0)
END