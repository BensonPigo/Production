CREATE PROCEDURE [dbo].[PPIC_Report04_New]
	@ID varchar(13)
	,@WithZ bit = 0
	,@ByType int = 0 --0單張 , 1 By CustCDID , 2 By PO
	,@PrintType int = 0 --0:All, 1:SizeSpec/OrderComboSizeAll, 2:page1/Page2
AS
BEGIN

	Declare @POID varchar(13) = (select POID from Orders where ID = @ID)

	IF @PrintType = 0 or @PrintType = 1
	BEGIN
		--SizeSpec-----------------------------------------------------------------------------------------------------------
		--##MAKER ##STYLENO ##QTY ##SP
		--##SSizeSpec_Tbl1
		exec Order_Report_SizeSpec @ID, @WithZ, 1, @ByType

		--OrderComboSize----------------------------------------------------------------------------------------------------
		--##MAKER ##STYLENO ##QTY ##SP
		--##SSizeSpec_OrderCombo_Tbl1
		exec Order_Report_SizeSpec_OrderCombo @ID, @WithZ, 1

		--All---------------------------------------------------------------------------------------------------------------
		--##SAll_Tbl1 ##SAll_Tbl2 ##SAll_Tbl3
		exec Order_Report_QtyBreakdown @ID,2
		exec Order_Report_FabColorCombination @ID,2
		exec Order_Report_AccColorCombination @ID,2

		--##SAll_Tbl4 ##SAll_Tbl5 ##SAll_Tbl6
		exec Order_Report_Color_MaterialCode @ID
	End

	IF @PrintType = 0 or @PrintType = 2
	BEGIN
		--Page1--------------------------------------------------------------------------------------------------------------
		--##S1_Tbl1
		exec Order_Report_QtyBreakdown @ID,1

		--##S1PACKING
		SELECT Orders.Packing FROM DBO.Orders WHERE ID in ( select OrderComboID from Orders where ID = @ID)
		--##S1LH
		SELECT Orders.Label FROM DBO.Orders WHERE ID in ( select OrderComboID from Orders where ID = @ID)
		--##S1VS
		SELECT Orders.VasShas,iif(Orders.VasShas = 1, iif(isnull(MnorderApv2,'') <> '',Orders.Packing2, '**Please wait for 2nd approve！'),'') as Packing2 FROM DBO.Orders WHERE ID = @ID

		--Page2--------------------------------------------------------------------------------------------------------------
		--##S2_SP ##S2_Style ##S2_QTY ##S2_CUSTCD ##S2_PoNo ##S2_Oeder ##S2_DELIVERY
		declare @newLine varchar(10) = CHAR(13)+CHAR(10)
		SELECT MAKER=FactoryID,a.ID,sty=a.StyleID+'-'+a.SeasonID,QTY,OrderComboID,CustCDID,CustPONo,a.Customize1,a.Customize2 AS SI,br.Customize2,BuyerDelivery,
		Mark=iif(MarkFront<>'','(A) '+@newLine+MarkFront,'')
		+@newLine+iif(MarkBack<>'','(B) '+@newLine+MarkBack,'')
		+@newLine+iif(MarkLeft<>'','(C) '+@newLine+MarkLeft,'')
		+@newLine+iif(MarkRight<>'','(D) '+@newLine+MarkRight,'')
		, [Status] = dbo.GetOrderStatus(@ID) 
		FROM dbo.Orders a 
		LEFT JOIN Brand Br
		ON a.BrandID = Br.ID
		where POID = @poid AND OrderComboID = @ID
	End

END