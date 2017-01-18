

CREATE PROCEDURE [dbo].[PPIC_Report_Color_MaterialCode]
	@ID varchar(13)
AS
BEGIN
	declare @POID varchar(13) = (select POID from Orders where ID = @ID)

	select ' ' = c.ID+'-->'+c.Name from Orders a
	inner join Order_ColorCombo b on a.ID = b.Id
	inner join Color c on b.ColorID = c.ID AND a.BrandID = c.BrandId
	where a.ID in (select ID from dbo.Orders where poid = @poid and CustCDID = (select CustCDID from Orders where Id = @ID))
	group by c.ID,c.Name


	SELECT 'Fabric:'=FabricCode + '-->' + b.Refno + ';' + c.[Description] FROM Orders a
	inner join dbo.Order_BOF b on a.ID = b.Id 
	inner join dbo.Fabric c on b.SCIRefno = c.SCIRefno and a.BrandID = c.BrandID
	where a.ID in (select ID from dbo.Orders where poid = @poid and CustCDID = (select CustCDID from Orders where Id = @ID))


	select 'Accessories:'=d.tt from Orders a
	inner join dbo.Order_BOA	b on a.ID = b.Id
	inner join dbo.Fabric c on b.SCIRefno = c.SCIRefno and a.BrandID = c.BrandID
	outer apply (select b.Seq+case when PatternPanel <> '' then '-'+PatternPanel else '' end + '-->' + b.Refno + ';' + c.[Description] as tt) d
	where a.ID in (select ID from dbo.Orders where poid = @poid and CustCDID = (select CustCDID from Orders where Id = @ID))
	order by d.tt

END