

CREATE PROCEDURE [dbo].[PPIC_Report_Color_MaterialCode]
	@ID varchar(13)
AS
BEGIN
	declare @OrderComboID varchar(13) = (select OrderComboID from MNOrder where ID = @ID)

	select ' ' = c.ID+'-->'+c.Name from MNOrder a
	inner join MNOrder_ColorCombo b on a.ID = b.Id
	inner join Color c on b.ColorID = c.ID AND a.BrandID = c.BrandId
	where a.ID in ( select ID from dbo.MNOrder where OrderComboID = @OrderComboID )
	group by c.ID,c.Name


	SELECT 'Fabric:'=FabricCode + '-->' + b.Refno + ';' + c.[Description] FROM MNOrder a
	inner join dbo.MNOrder_BOF b on a.ID = b.Id 
	inner join dbo.Fabric c on b.SCIRefno = c.SCIRefno and a.BrandID = c.BrandID
	where a.ID in ( select ID from dbo.MNOrder where OrderComboID = @OrderComboID )


	select 'Accessories:'=d.tt from MNOrder a
	inner join dbo.MNOrder_BOA	b on a.ID = b.Id
	inner join dbo.Fabric c on b.SCIRefno = c.SCIRefno and a.BrandID = c.BrandID
	outer apply (select b.Seq+case when PatternPanel <> '' then '-'+PatternPanel else '' end + '-->' + b.Refno + ';' + c.[Description] as tt) d
	where a.ID in ( select ID from dbo.MNOrder where OrderComboID = @OrderComboID )
	order by d.tt

END