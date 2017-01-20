

CREATE PROCEDURE [dbo].[PPIC_Report02]
	@ID varchar(13)
	,@WithZ bit = 0
AS
BEGIN

declare @POID varchar(13) = (select POID from MNOrder where ID = @ID)

--Page1 第一張只會呈現一次，另外抓就好-------------------------------------------------------------------------------
--##MAKER ##STYLENO ##QTY ##SP
--##S1_Tbl1
--exec Order_Report_SizeSpec @poid, @WithZ, 1


--Page2--------------------------------------------------------------------------------------------------------------
--##S2_Tbl1 ##S2_Tbl2 ##S2_Tbl3
exec Order_Report_QtyBreakdown @ID,0
exec Order_Report_FabColorCombination @ID,0
exec Order_Report_AccColorCombination @ID,0


--##S2_Tbl4 ##S2_Tbl5 ##S2_Tbl6
exec PPIC_Report_Color_MaterialCode @ID


--ShipingMark
declare @newLine varchar(10) = CHAR(13)+CHAR(10)
SELECT shipingMark=iif(MarkFront<>'','(A) '+@newLine+MarkFront,'')
+@newLine+iif(MarkBack<>'','(B) '+@newLine+MarkBack,'')
+@newLine+iif(MarkLeft<>'','(C) '+@newLine+MarkLeft,'')
+@newLine+iif(MarkRight<>'','(D) '+@newLine+MarkRight,'')
FROM MNOrder a where ID = @ID


--##S2PACKING
SELECT Orders.Packing FROM DBO.Orders WHERE ID = @ID
--##S2LH
SELECT Orders.Label FROM DBO.Orders WHERE ID = @ID



END