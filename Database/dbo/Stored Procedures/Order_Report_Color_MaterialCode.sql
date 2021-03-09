CREATE PROCEDURE [dbo].[Order_Report_Color_MaterialCode]
	@ID varchar(13)
AS
BEGIN
	declare @POID varchar(13) = (select POID from Orders where ID = @ID)

	select ' ' = c.ID+'-->'+c.Name from Orders a
	inner join Order_ColorCombo b on a.ID = b.Id
	inner join Color c on b.ColorID = c.ID AND a.BrandID = c.BrandId
	where a.ID in ( select ID from dbo.Orders where poid = @poid )
	group by c.ID,c.Name


	SELECT 'Fabric:'=FabricCode + '-->' + b.Refno + ';' + c.[Description] FROM Orders a
	inner join dbo.Order_BOF b on a.ID = b.Id 
	inner join dbo.Fabric c on b.SCIRefno = c.SCIRefno and a.BrandID = c.BrandID
	where a.ID in ( select ID from dbo.Orders where poid = @poid )


	select 'Accessories:'=d.tt from Orders a
	inner join dbo.Order_BOA	b on a.ID = b.Id
	inner join dbo.Fabric c on b.SCIRefno = c.SCIRefno and a.BrandID = c.BrandID
	outer apply (select b.Seq1+
					case when FabricPanelCode <> '' 
					then '-'+FabricPanelCode else '' end 
					+ iif(b.SizeItem like 'Z%' or b.SizeItem like 'W%' or b.SizeItem like 'S%', '-' + b.SizeItem, '') 
					+ iif(b.SizeItem_Elastic like 'Z%' or b.SizeItem_Elastic like 'W%', '/' + b.SizeItem_Elastic, '')
					+ '-->' + b.Refno + ';' + c.[Description] as tt) d
	where a.ID in ( select ID from dbo.Orders where poid = @poid )
	order by d.tt

END
