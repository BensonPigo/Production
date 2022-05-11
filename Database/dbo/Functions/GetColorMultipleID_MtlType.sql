
Create Function [dbo].[GetColorMultipleID_MtlType]
	(
	  @BrandID		VarChar(8)
	 ,@ColorID		VarChar(6)
	 ,@MtlTypeID	VarChar(20) = ''　-- Fabric.MtlTypeID
	 ,@SuppColor	VarChar(500) = '' -- PO_Supp_Detail.SuppColor
	)
Returns VarChar(500)
As
Begin
	declare @Colors varchar(500)
	if @MtlTypeID in ('EMB THREAD','SP THREAD','THREAD') and @SuppColor <> ''
		set @Colors =  @SuppColor
	else
		set @Colors = dbo.GetColorMultipleID(@BrandID, @ColorID)

	Return @Colors
End