USE [Production]
GO

/****** Object:  UserDefinedFunction [dbo].[GetGarmentSizeByOrderIDSeq]    Script Date: 2019/06/05 ¤W¤È 11:12:02 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE FUNCTION [dbo].[GetGarmentSizeByOrderIDSeq]
(
    @OrderID as  varchar(10),
    @Seq1 as  varchar(10),
    @Seq2 as  varchar(10)
)
RETURNS varchar(100)
AS
BEGIN
    Declare @returnGarmentSize varchar(100) = '';
	
	IF( (@OrderID='' OR @OrderID IS NULL) OR (@Seq1='' OR @Seq1 IS NULL) OR (@Seq2='' OR @Seq2 IS NULL))
	BEGIN
		RETURN @returnGarmentSize;
	END

	SELECT @returnGarmentSize= OSS.GarmentSize
	FROM PO_Supp_Detail p WITH (NOLOCK)
	OUTER APPLY(
		SELECT top 1 oBOA.SizeItem 
		FROM Order_BOA oBOA WITH (NOLOCK) 
		WHERE p.ID = oBOA.ID AND p.SEQ1 = oBOA.Seq1 AND p.SCIRefno = oBOA.SCIRefno 
	) LIST
	OUTER APPLY(
		select [GarmentSize] = Stuff((select concat( ',',LOSS.SizeCode) 
		from Order_SizeSpec LOSS WITH (NOLOCK)
		LEFT JOIN Order_SizeCode LOSC WITH (NOLOCK) ON LOSC.Id=LOSS.ID AND LOSC.SizeCode = LOSS.SizeCode
		where LOSS.Id = p.ID AND LOSS.SizeItem = LIST.SizeItem AND LOSS.SizeSpec = p.SizeSpec ORDER BY LOSC.Seq ASC FOR XML PATH('')),1,1,'') 
	) OSS
	WHERE p.ID=@OrderID AND p.Seq1=@Seq1 AND p.Seq2=@Seq2 

    RETURN @returnGarmentSize;
END



GO


