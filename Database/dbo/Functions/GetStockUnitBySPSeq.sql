-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[GetStockUnitBySPSeq]
(
	@poid varchar(16)
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
		select value = case when mm.IsExtensionUnit = 1 and uu.ExtensionUnit <> '' then uu.ExtensionUnit
							when mm.OutputUnit = 1 then ff.UsageUnit
							when mm.OutputUnit = 2 then p.POUnit 
							else '' end
	) StockUnit
	where	p.id = @poid
			and p.seq1 = @seq1
			and p.seq2 = @seq2
	return @Return
END