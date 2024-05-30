CREATE FUNCTION [dbo].[IsSustainableMaterial]
(
	@OrderID varchar(13)
	, @SCIRefno varchar(30)
)
RETURNS bit
AS
BEGIN
	RETURN Case When
			exists (
				Select 1
				From Orders o with (nolock)
				Where ID = @OrderID
				and exists (
					Select 1 From Order_BOF bof with (nolock)
					Left join dbo.Fabric f with (nolock) on bof.SCIRefno = f.SCIRefno
					Left join dbo.Fabric_Content fc with (nolock) on f.SCIRefno = fc.SCIRefno
					Left join dbo.MtlType_Brand mtlb with (nolock) on mtlb.ID = fc.MtltypeId and mtlb.BrandID = o.BrandID
					Where bof.Id = o.POID
					and mtlb.IsSustainableMaterial = 1
					and ((o.BrandID = 'LLL' and bof.Kind = '1')
						or o.BrandID != 'LLL')
					and (@SCIRefno = '' or bof.SCIRefno = @SCIRefno)
					)
				)
			Then 1 Else 0 End
END
GO