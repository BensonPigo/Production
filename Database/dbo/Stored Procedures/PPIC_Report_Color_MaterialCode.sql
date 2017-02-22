

CREATE PROCEDURE [dbo].[PPIC_Report_Color_MaterialCode]
	@ID varchar(13)
AS
BEGIN
	declare @POID varchar(13) = (select POID from MNOrder WITH (NOLOCK) where ID = @ID)

	select ' ' = c.ID+'-->'+c.Name from MNOrder a WITH (NOLOCK)
	inner join MNOrder_ColorCombo b WITH (NOLOCK) on a.ID = b.Id
	inner join Color c WITH (NOLOCK) on b.ColorID = c.ID AND a.BrandID = c.BrandId
	where a.ID in (select ID from dbo.MNOrder WITH (NOLOCK) where poid = @poid and CustCDID = (select CustCDID from MNOrder WITH (NOLOCK) where Id = @ID))
	group by c.ID,c.Name


	SELECT 'Fabric:'=FabricCode + '-->' + b.Refno + ';' + c.[Description] FROM MNOrder a WITH (NOLOCK)
	inner join dbo.MNOrder_BOF b WITH (NOLOCK) on a.ID = b.Id 
	inner join dbo.Fabric c WITH (NOLOCK) on b.SCIRefno = c.SCIRefno and a.BrandID = c.BrandID
	where a.ID in (select ID from dbo.MNOrder WITH (NOLOCK) where poid = @poid and CustCDID = (select CustCDID from MNOrder WITH (NOLOCK) where Id = @ID))


	select 'Accessories:'=d.tt from MNOrder a WITH (NOLOCK)
	inner join dbo.MNOrder_BOA	b WITH (NOLOCK) on a.ID = b.Id
	inner join dbo.Fabric c WITH (NOLOCK) on b.SCIRefno = c.SCIRefno and a.BrandID = c.BrandID
	outer apply (select b.Seq+case when PatternPanel <> '' then '-'+PatternPanel else '' end + '-->' + b.Refno + ';' + c.[Description] as tt) d
	where a.ID in (select ID from dbo.MNOrder WITH (NOLOCK) where poid = @poid and CustCDID = (select CustCDID from MNOrder WITH (NOLOCK) where Id = @ID))
	order by d.tt

END