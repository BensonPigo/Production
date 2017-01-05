
create FUNCTION [dbo].[GetColorSizeQty]
(
	@OrderID VARCHAR(13)
)
RETURNS @tbl table(
	Article varchar(8)
	,SizeCode varchar(8)
	,ColorID varchar(6)
	,ColorDesc nvarchar(90)
	,FabricCode varchar(3)
	,LectraCode varchar(2)
	,PatternPanel varchar(2) 
	,Zipper_RL varchar(5)
	,CustCD varchar(16)
	,SizeDesc varchar(15)
	,Seq varchar(2)
	,Qty decimal(8,0)
	,SizeItem varchar(3)
	,OrderList nvarchar(90)
)
AS
BEGIN
	
	declare @tmp table(ID varchar(13) ,Article varchar(8) ,SizeCode varchar(8) ,ColorID varchar(6) ,ColorDesc nvarchar(90) 
	,FabricCode varchar(3) ,LectraCode varchar(2) ,PatternPanel varchar(2) ,Zipper_RL varchar(5)
	,CustCD varchar(16) ,SizeDesc varchar(15) ,Seq varchar(2) ,Qty decimal(8,0), SizeItem varchar(3))

	--抓取ID為POID
	select @OrderID=POID FROM dbo.Orders where ID = @OrderID

	--主料
	insert into @tmp
	select Orders.ID,Order_Qty.Article
	,SizeCode=case when Order_SizeSpec.SizeSpec is null then Order_Qty.SizeCode else Order_SizeSpec.SizeSpec end
	,Order_ColorCombo.ColorID,ColorDesc=Color.Name
	,Order_ColorCombo.FabricCode
	,Order_ColorCombo.LectraCode
	,Order_ColorCombo.PatternPanel
	,Zipper_RL=ZipperInsert
	,CustCD=Orders.CustCDID
	,SizeDesc=''
	,Order_SizeCode.Seq
	,Order_Qty.Qty
	,'' as SizeItem
	from Orders 
	inner join Order_Qty on Order_Qty.ID = Orders.ID
	inner join Order_ColorCombo on Order_ColorCombo.Id = @OrderID and Order_ColorCombo.Article = Order_Qty.Article and Order_ColorCombo.FabricCode != ''
	--ColorDesc
	inner join Color on color.ID = Order_ColorCombo.ColorID and Color.BrandId = Orders.BrandID
	--S00 SizeCode
	left join Order_SizeSpec on Order_SizeSpec.Id = Orders.ID and Order_SizeSpec.SizeCode = Order_Qty.SizeCode and SizeItem = 'S00'
	--Zipper_RL
	inner join CustCD on CustCD.ID = Orders.CustCDID and CustCD.BrandID = Orders.BrandID
	--SizeCode Seq
	left join Order_SizeCode on Order_SizeCode.Id = @OrderID and Order_SizeCode.SizeCode = Order_Qty.SizeCode
	where Orders.poId = @OrderID


	--副料
	insert into @tmp
	select Orders.ID,Order_Qty.Article
	,SizeCode=case when Order_SizeSpec.SizeSpec is null then Order_Qty.SizeCode else Order_SizeSpec.SizeSpec end
	,Order_ColorCombo.ColorID,ColorDesc=Color.Name
	,Order_ColorCombo.FabricCode
	,Order_ColorCombo.LectraCode
	,Order_ColorCombo.PatternPanel
	,Zipper_RL=ZipperInsert
	,CustCD=Orders.CustCDID
	,SizeDesc=isnull(m.SizeSpec,'')
	,Order_SizeCode.Seq
	,Order_Qty.Qty
	,isnull(s.SizeItem,'') as SizeItem
	from Orders 
	inner join Order_Qty on Order_Qty.ID = Orders.ID
	inner join Order_ColorCombo on Order_ColorCombo.Id = @OrderID and Order_ColorCombo.Article = Order_Qty.Article and Order_ColorCombo.FabricCode = ''
	--ColorDesc
	inner join Color on color.ID = Order_ColorCombo.ColorID and Color.BrandId = Orders.BrandID
	--S00 SizeCode
	left join Order_SizeSpec on Order_SizeSpec.Id = Orders.ID and Order_SizeSpec.SizeCode = Order_Qty.SizeCode and Order_SizeSpec.SizeItem = 'S00'
	--取得SizeDesc
	outer apply(select SizeItem=case when SizeItem_Elastic <> '' then SizeItem_Elastic else SizeItem end from Order_BOA
				where id = @OrderID and Order_BOA.PatternPanel <> '' and Order_BOA.PatternPanel = Order_ColorCombo.PatternPanel 
				and (SizeItem_Elastic <> '' or SizeItem <> '' )) s
	outer apply(select SizeSpec from Order_SizeSpec aa where aa.Id = @OrderID and aa.SizeItem = s.SizeItem and aa.SizeCode = Order_Qty.SizeCode) m
	--Zipper_RL
	inner join CustCD on CustCD.ID = Orders.CustCDID and CustCD.BrandID = Orders.BrandID
	--SizeCode Seq
	left join Order_SizeCode on Order_SizeCode.Id = @OrderID and Order_SizeCode.SizeCode = Order_Qty.SizeCode
	where Orders.poId = @OrderID

	insert into @tbl
	select Article,SizeCode,ColorID,ColorDesc,FabricCode,LectraCode,PatternPanel,Zipper_RL,CustCD,SizeDesc,Seq,Qty=sum(Qty),SizeItem,d.OrderList+'/'
	from @tmp aa
	--Order List
	outer apply (SELECT STUFF((SELECT '/'+tmp.ID FROM @tmp tmp where 
	tmp.Article = aa.Article and tmp.SizeCode = aa.SizeCode and tmp.ColorID = aa.ColorID and tmp.ColorDesc = aa.ColorDesc
	and tmp.FabricCode = aa.FabricCode and tmp.LectraCode = aa.LectraCode and tmp.PatternPanel = aa.PatternPanel
	and tmp.Zipper_RL = aa.Zipper_RL and tmp.CustCD = aa.CustCD and tmp.SizeDesc = aa.SizeDesc
	and tmp.Seq = aa.Seq order by tmp.ID FOR XML PATH(''), TYPE ).value('.', 'NVARCHAR(MAX)'),1,1,'') as OrderList) d
	group by Article,SizeCode,ColorID,ColorDesc,FabricCode,LectraCode,PatternPanel,Zipper_RL,CustCD,SizeDesc,Seq,d.OrderList,SizeItem
	

	/*
	--主料
	Select * from Order_ColorCombo where Id = '15122845GM' and fabriccode != '' order by LectraCode, Fabriccode
	select Id,FabricCode,* from Order_BOF where Id = '15122845GM'


	--副料
	Select Id,PatternPanel,ColorID,Article,* from Order_ColorCombo where Id = '15122845GM' and fabriccode = ''
	select Id,PatternPanel,SizeItem,SizeItem_Elastic,* from Order_BOA where Id = '15122845GM'


	--取代S00
	select * from Order_SizeSpec where id = '15122845GM' and SizeItem = 'S00'
	select article, sizecode, Sum(Order_Qty.qty) from Order_Qty inner join orders on orders.id = order_qty.id where orders.poId = '15122845GM' group by Article, sizecode


	select * from Order_SizeSpec where id = '15122845GM' and SizeItem = 'S46'
	*/


	--Article	Order_Article
	--SizeCode	Order_SizeCode
	--ColorId
	--ColorDesc Color.Name
	--**Zipper_RL  With mZipper_RL,;
	--**CustCD With cCustCd,;
	--**SizeDesc With mSizeDesc,;
	--Seq		Order_SizeCode
	--OrderList		Allt(Orderlist)+Allt(cOrderSP)+"/"
		


	return

END