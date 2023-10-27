CREATE FUNCTION [dbo].GetPo3Spec_Null
(
	@POID varchar(13)
	, @Seq1 varchar(3)
	, @Seq2 varchar(2)
)
RETURNS TABLE AS RETURN
(
	select *
	from 
	(
		select BomTypeID = BomType.ID, SpecValue = po3s.SpecValue
		from Production.dbo.BomType with (nolock)
		left join PO_Supp_Detail_Spec po3s with (nolock) on po3s.ID = @POID and po3s.Seq1 = @Seq1 and po3s.Seq2 = @Seq2 and BomType.ID = po3s.SpecColumnID
	)tmp
	pivot
	(
		MAX(SpecValue) for BomTypeID in
		(Color, Size, SizeUnit, ZipperInsert, Article, COO, Gender, CustomerSize, DecLabelSize, BrandFactoryCode, Style, StyleLocation, Season, CareCode, CustomerPO, BuyMonth, BuyerDlvMonth)
	) as p
)