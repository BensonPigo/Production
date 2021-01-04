USE Production

CREATE FUNCTION [dbo].[Get_ShippingMarkTemplate_DefineColumn]
(
	@PackingListID varchar(15),
	@OrderID varchar(13),
	@CTNStartNo varchar(6),
	@RefNo varchar(21)
)
RETURNS @returntable TABLE
(
	ID varchar(60) ,
	FromPMS bit  ,
	ChkEmpty bit  ,
	[Desc] varchar(200) ,
	[Value] varchar(100) 
)
AS
BEGIN

	declare @IsMixPack varchar(10) = 'MIXED'
			, @hcf int;

	/*
		確認是否為混尺碼裝箱
	*/
	if ((select count(1)
		 from (
			SELECT pld.Article
					, pld.SizeCode
			FROm Production.dbo.PackingList_Detail pld
			WHERE pld.ID = @PackingLIstID
					AND pld.CTNStartNo= @CTNStartNo
			group by pld.Article, pld.SizeCode
		 ) CheckMix) > 1)
	begin
		--print '混尺碼裝箱'
		insert into @returntable
		SELECT   col.ID
				,col.FromPMS
				,col.ChkEmpty
				,col.[Desc]
				,[Value] =	CASE col.ID 
								WHEN 'CABCode' THEN OrderInfo.CAB
								WHEN 'Customer_PO' THEN OrderInfo.Customer_PO
								WHEN 'FinalDestination' THEN OrderInfo.FinalDest
								WHEN 'AFS_STOCK_CATEGORY' THEN OrderInfo.AFS_STOCK_CATEGORY
								When 'CartonType' then LocalItem.CartonType
								When 'StyleArticle' then Concat (OrderInfo.StyleID, '-', PackInfo.Article)
								When 'SZQty' then Concat ('SZ/QTY: ', @IsMixPack, '/', PackInfo.TtlShipQty)
								When 'PrepackContentsLable' then 'Prepack Contents'
								When 'PrepackContentsSZQtyLable' then Concat ('SZ:', char(13), 'QTY:')
								When 'PrepackContentsSZQtyValue' then Concat (PrepackContents.Size, char(13), PrepackContents.Qty)
								When 'NoofPrepacks' then Concat ('# of Prepacks', char(13)
									, (IIF(PackInfo.TtlPrepackQty = 0 , 0 , PackInfo.TtlShipQty / PackInfo.TtlPrepackQty))
									, ' of '
									,  PackInfo.TtlPrepackQty
								)
								When 'FTYPONo' then OrderInfo.CustPONo
							END
		FROM ShippingMarkTemplate_DefineColumn col
		OUTER APPLY(
			SELECT CAB
					, Customer_PO 
					, FinalDest
					, AFS_STOCK_CATEGORY 
					, StyleID
					, CustPONo
			FROM Orders o
			WHERE o.ID = @OrderID 
		) OrderInfo
		outer apply (
			select	Article = Max (pld.Article)
					, TtlShipQty = sum (pld.ShipQty)
					, TtlPrepackQty = sum (pld.PrepackQty)
			FROm Production.dbo.PackingList_Detail pld
			WHERE pld.ID = @PackingLIstID
					AND pld.CTNStartNo= @CTNStartNo
		) PackInfo
		outer apply (
			select Size =	Stuff ((
							SELECT CONCAT (char(9), pld.SizeCode)			
							From Production.dbo.PackingList_Detail pld
							left join orders o on pld.OrderID = o.ID
							left join Order_SizeCode osc on o.POID = osc.Id
															and pld.SizeCode = osc.SizeCode
							WHERE pld.ID = @PackingLIstID
									and pld.CTNStartNo= @CTNStartNo
							order by osc.Seq
							for xml path('')
							), 1, 1, '')
					, Qty =	Stuff ((			
							SELECT CONCAT (char(9),  pld.PrepackQty)
							From Production.dbo.PackingList_Detail pld
							left join orders o on pld.OrderID = o.ID
							left join Order_SizeCode osc on o.POID = osc.Id
															and pld.SizeCode = osc.SizeCode
							WHERE pld.ID = @PackingLIstID
									and pld.CTNStartNo= @CTNStartNo
							order by osc.Seq
							for xml path('')
							), 1, 1, '')
		) PrepackContents
		OUTER APPLY(
			SELECT CartonType
			FROM Production.dbo.LocalItem 
			WHERE RefNo = @RefNo 		
		)LocalItem
	end
	else
	begin	
		--print '單尺碼裝箱'
		insert into @returntable
		SELECT   col.ID
				,col.FromPMS
				,col.ChkEmpty
				,col.[Desc]
				,[Value] =	CASE col.ID 
								WHEN 'CABCode' THEN OrderInfo.CAB
								WHEN 'Customer_PO' THEN OrderInfo.Customer_PO
								WHEN 'FinalDestination' THEN OrderInfo.FinalDest
								WHEN 'AFS_STOCK_CATEGORY' THEN OrderInfo.AFS_STOCK_CATEGORY
								When 'CartonType' then LocalItem.CartonType
								When 'StyleArticle' then Concat (OrderInfo.StyleID, '-', PackInfo.Article)
								When 'SZQty' then Concat ('SZ: ', PackInfo.SizeCode, char(13), 'QTY: ', PackInfo.ShipQty)
								When 'PrepackContentsLable' then ''
								When 'PrepackContentsSZQtyLable' then ''
								When 'PrepackContentsSZQtyValue' then ''
								When 'NoofPrepacks' then ''
								When 'FTYPONo' then OrderInfo.CustPONo
							END
		FROM ShippingMarkTemplate_DefineColumn col
		OUTER APPLY(
			SELECT CAB
					, Customer_PO 
					, FinalDest
					, AFS_STOCK_CATEGORY 
					, StyleID
					, CustPONo
			FROM Orders o
			WHERE o.ID = @OrderID 
		) OrderInfo
		outer apply (		
			SELECT pld.Article
					, pld.SizeCode
					, ShipQty
			FROm Production.dbo.PackingList_Detail pld
			WHERE pld.ID = @PackingLIstID
					AND pld.CTNStartNo= @CTNStartNo
		) PackInfo
		OUTER APPLY(
			SELECT CartonType
			FROM Production.dbo.LocalItem 
			WHERE RefNo = @RefNo 		
		)LocalItem
	end
	RETURN
END

GO