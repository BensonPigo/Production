-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[GetStockUnitBySPSeq]
(
	@poid varchar(10)
	, @seq1 varchar(3)
	, @seq2 varchar(3)
)
RETURNS varchar(8)
AS
BEGIN
	declare @Return varchar(10) = ''
	select @Return = StockUnit.value
	from PO_Supp_Detail p
	inner join [dbo].[Fabric] ff WITH (NOLOCK) on p.SCIRefno= ff.SCIRefno
	inner join [dbo].[MtlType] mm WITH (NOLOCK) on mm.ID = ff.MtlTypeID
	inner join [dbo].[Unit] uu WITH (NOLOCK) on ff.UsageUnit = uu.ID
	outer apply (
		select value = iif(mm.IsExtensionUnit is null or uu.ExtensionUnit = '', ff.UsageUnit 
																			  , iif(mm.IsExtensionUnit > 0 , iif(uu.ExtensionUnit is null or uu.ExtensionUnit = '', ff.UsageUnit  
																																								  , uu.ExtensionUnit) 
																										   , ff.UsageUnit))  
	) StockUnit
	where	p.id = @poid
			and p.seq1 = @seq1
			and p.seq2 = @seq2
	return @Return
END