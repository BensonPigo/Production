CREATE PROCEDURE [dbo].[PPIC_Report04]
	@ID varchar(13)
	,@WithZ bit = 0
	,@ByType int = 0 --0單張 , 1 By OrderComboID , 2 By PO
AS
BEGIN

declare @POID varchar(13) = (select POID from MNOrder WITH (NOLOCK) where ID = @ID)

--Page1--------------------------------------------------------------------------------------------------------------
--##MAKER ##STYLENO ##QTY ##SP
--##S1_Tbl1
exec PPIC_Report_SizeSpec @ID, @WithZ, 1,@ByType


--Page2--------------------------------------------------------------------------------------------------------------
--##S2_Tbl1 ##S2_Tbl2 ##S2_Tbl3
exec Order_Report_QtyBreakdown @ID,1
exec Order_Report_FabColorCombination @ID,1
exec Order_Report_AccColorCombination @ID,1


--##S2_Tbl4 ##S2_Tbl5 ##S2_Tbl6
exec PPIC_Report_Color_MaterialCode @ID


--##S2PACKING
SELECT Packing FROM MNOrder WITH (NOLOCK) WHERE ID in ( select OrderComboID from MNOrder where ID = @ID)
--##S2LH
SELECT Label FROM MNOrder WITH (NOLOCK) WHERE ID in ( select OrderComboID from MNOrder where ID = @ID)

--Page3--------------------------------------------------------------------------------------------------------------
--##S3_SP ##S3_Style ##S3_QTY ##S3_CUSTCD ##S3_PoNo ##S3_Oeder ##S3_DELIVERY
declare @newLine varchar(10) = CHAR(13)+CHAR(10)
SELECT MAKER=FactoryID,ID,sty=StyleID+'-'+SeasonID,QTY,OrderComboID,CustPONo,Customize1,BuyerDelivery,
Mark=iif(MarkFront<>'','(A) '+@newLine+MarkFront,'')
+@newLine+iif(MarkBack<>'','(B) '+@newLine+MarkBack,'')
+@newLine+iif(MarkLeft<>'','(C) '+@newLine+MarkLeft,'')
+@newLine+iif(MarkRight<>'','(D) '+@newLine+MarkRight,'')
FROM MNOrder a WITH (NOLOCK) where POID = @poid AND OrderComboID = @ID


END