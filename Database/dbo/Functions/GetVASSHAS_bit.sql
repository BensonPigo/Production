-- =============================================
-- Description:	Get VAS/SHAS bool
-- =============================================
CREATE FUNCTION [dbo].[GetVASSHAS_bit]
(
	@POID varchar(13),
	@Seq1 varchar(3),
	@Seq2 varchar(2)
)
RETURNS Bit
AS
BEGIN

declare @value bit;
	
if exists(
	select top 1 old.*
	from PO_Supp_Detail psd
	inner join Order_BOA ob on psd.SCIRefno = ob.SCIRefno
	and psd.ID = ob.Id
	and psd.SEQ1 = ob.Seq1
	inner join Order_Label_Detail old on ob.Ukey = old.Order_BOAUkey
	where psd.ID = @POID and psd.SEQ1= @Seq1 and psd.SEQ2 = @Seq2
)
	begin
		set @value = 1
	end
else
	begin 
		set @value = 0
	end

	return @value;
END