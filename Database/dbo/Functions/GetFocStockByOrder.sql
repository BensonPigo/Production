USE [Production]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetFocStockByOrder]
(
	@OrderID varchar(20)
)
RETURNS int
AS
BEGIN
	-- Declare the return variable here
	DECLARE @FocStockQty as int;	
	DECLARE @FocQty_InOrder as int;
	DECLARE @TotalShipQty as int;
	DECLARE @TotalDiffQty as int;

	
	DECLARE @Tmp1 TABLE (
	Article varchar(20),
	SizeCode varchar(20),
	Price decimal(12,4)
	)
		
	DECLARE @Tmp2 TABLE (
	Article varchar(20),
	SizeCode varchar(20),
	ShipQty int
	)

	DECLARE @Tmp3 TABLE (
	Article varchar(20),
	SizeCode varchar(20),
	DiffQty int
	)

	--�p��p��FOC �w�s�H
	--FOC �w�s�ƶq = [�q�� FOC �`�ƶq] - [Size/Article �� FOC ���`�X�f��] + [Size/Article �� FOC ���x�_�]�Ƚվ㪺�ƶq]

	--1. �q�� FOC �ƶq
	SELECT @FocQty_InOrder=FOCQty
	FROM Orders 
	WHERE ID = @OrderID

	--2. Size/Article �� FOC ���w�q�A����z�XSize/Article ������A�p�G����O 0 �Y�OFOC�A��o�Ǭ� 0 ��Size/Article�O���U��
	INSERT @Tmp1
	SELECT   [Article]=oq.Article
			,[SizeCode]=oq.SizeCode 
			,[Price]=ISNULL(ou1.POPrice, ISNULL(ou2.POPrice,-1))
	FROM Order_Qty oq
	LEFT JOIN Order_UnitPrice ou1 ON ou1.Id=oq.Id AND ou1.Article=oq.Article AND ou1.SizeCode=oq.SizeCode 
	LEFT JOIN Order_UnitPrice ou2 ON ou2.Id=oq.Id AND ou2.Article='----' AND ou2.SizeCode='----' 
	WHERE oq.ID = @OrderID
	ORDER BY  oq.Article,oq.SizeCode 


	--3. �A��^Packing�A�n��X���欰0���`�X�f�ơAStatus <> New�N��w�g�X�f�F�A�����X�f�ѤU�N�O�w�s
	INSERT @Tmp2
    select pd.Article,pd.SizeCode,[ShipQty]=SUM(pd.ShipQty)
    from PackingList p
    inner join PackingList_Detail pd on p.ID = pd.ID
    where p.PulloutStatus <> 'New'
    and p.PulloutID <> ''
    and pd.OrderID = @OrderID
    group by pd.Article,pd.SizeCode
    order by pd.Article,pd.SizeCode


	--4. �x�_�]�Ƚվ㪺�ƶq
	INSERT @Tmp3
	SELECt iq.Article,iq.SizeCode,[DiffQty]= SUM(iq.DiffQty)
	FROm InvAdjust i
	INNER JOIN InvAdjust_Qty iq ON i.ID = iq.ID
	WHERE i.OrderID = @OrderID
	GROUP BY iq.Article,iq.SizeCode

	----P.S  3�B4���������ھ�Article�BSizeCode�����ե[�`�A�̫�[�`�קK�o��

	--P.S �p�G�H��n�W�[�UArticle/SizeCode��FOC�X�f�A�q�o��
	SELECT --t1.Article,t1.SizeCode
			 @TotalShipQty=SUM(ISNULL(t2.ShipQty,0))
			,@TotalDiffQty=SUM(ISNULL(t3.DiffQty,0))
	FROM @tmp1 t1
	LEFT JOIN @tmp2 t2 ON t1.Article=t2.Article AND t1.SizeCode=t2.SizeCode
	LEFT JOIN @tmp3 t3 ON t1.Article=t3.Article AND t1.SizeCode=t3.SizeCode
	WHERE t1.Price=0
	--GROUP BY t1.Article,t1.SizeCode

	--�x�_�վ�A�վ㪺�O�X�f�ƶq�A�]�����ઽ���ήw�s�ƥh��
	SET @FocStockQty = @FocQty_InOrder - ( ISNULL(@TotalShipQty,0) +  ISNULL(@TotalDiffQty,0) )

	-- Return the result of the function
	RETURN @FocStockQty

END
GO


