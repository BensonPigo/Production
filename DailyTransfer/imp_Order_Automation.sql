﻿-- =============================================
-- Author:		<Aaron S02109>
-- Create date: <2020/04/15>
-- Description:	在imp_Order後面執行，透過web api傳送有異動過的資料給廠商
-- =============================================
Create PROCEDURE [dbo].[imp_Order_Automation]
AS
BEGIN
	if not exists(select 1 from Production.dbo.System where Automation = 1 )
	begin
		return
	end

	Declare @Url varchar(100)
	--傳送Order_Qty
	select @Url = [dbo].[GetWebApiURL]('3A0197', 'AGV') 
	if(isnull(@Url, '') <> '')
	begin
		exec dbo.SentOrderQtyToAGV
		exec dbo.SentOrderPatternPanelToAGV
	end
	
	--傳送Order資訊給Sunrise
------------------------------------------------------------------------------------------------------
--***資料交換的條件限制***
--1. 優先取得Production.dbo.DateInfo
	declare @DateInfoName varchar(30) ='imp_Order_Automation';
	declare @DateStart date= (select DateStart from Production.dbo.DateInfo where name = @DateInfoName);
	declare @DateEnd date  = (select DateEnd   from Production.dbo.DateInfo where name = @DateInfoName);
	declare @Remark nvarchar(max) = (select Remark from Production.dbo.DateInfo where name = @DateInfoName);

--2.取得預設值
	if @DateStart is Null
		set @DateStart= CONVERT(DATE,DATEADD(day,-7,GETDATE()))
	if @DateEnd is Null
		set @DateEnd = CONVERT(DATE,DATEADD(day,1,GETDATE()))

--3.更新Pms_To_Trade.dbo.dateInfo
if exists(select 1 from Pms_To_Trade.dbo.dateInfo where Name = @DateInfoName )
	update Pms_To_Trade.dbo.dateInfo  set DateStart = @DateStart,DateEnd = @DateEnd, Remark=@Remark where Name = @DateInfoName 
else
	Insert into Pms_To_Trade.dbo.dateInfo(Name,DateStart,DateEnd,Remark)
	values (@DateInfoName,@DateStart,@DateEnd,@Remark);
------------------------------------------------------------------------------------------------------

	declare @orderList nvarchar(max);

	-- Sunrise FinishingProcesses
	select @Url = [dbo].[GetWebApiURL]('3A0134', 'FinishingProcesses') 
	--需刪除的訂單
	--M 單
	--Cancel 訂單並且是不需要在生產的訂單
	--SNP 請其他間工廠代工的訂單（g. N2E）
	--訂單已轉到其他工廠（此部分就是目前使用 OrderComparisonList 的部分）
	if(isnull(@Url, '') <> '')
	begin
		select a.OrderID into #tmpDeleteOrder
		from (select [OrderID] = a.ID
				from orders a with (nolock)
				inner join  Factory f on a.FactoryID = f.ID
				where	((a.AddDate >= @DateStart and a.AddDate < @DateEnd) or
						(a.EditDate >= @DateStart and a.EditDate < @DateEnd) or
						(a.PulloutCmplDate >= @DateStart and a.PulloutCmplDate < @DateEnd)) 
						and
						(a.Category in ('M', '', 'T') or
						(a.junk = 1 and a.NeedProduction = 0) or
						(f.IsProduceFty = 0))
				UNION
				select [OrderID] = ID
				from Orders with (nolock)
				where GMTClose >= @DateStart and Finished = 1
				union
				select distinct ocl.OrderID 
				from OrderComparisonList ocl with (nolock)
				where ocl.UpdateDate >= @DateStart and
					  ocl.DeleteOrder = 1 and
					  not exists(
						select 1
						from orders o with (nolock)
						where ocl.OrderId =  o.ID)) a

		SELECT @orderList =  Stuff((select concat( ',',ID) 
								from Production.dbo.Orders with (nolock)
								where	((AddDate >= @DateStart and AddDate < @DateEnd) or
										(EditDate >= @DateStart and EditDate < @DateEnd) or
										(PulloutCmplDate >= @DateStart and PulloutCmplDate < @DateEnd)) 
										and 
										not exists (select 1 from #tmpDeleteOrder t where t.OrderID = Orders.ID)
								FOR XML PATH('')),1,1,'') 

								
		----必須更新的訂單
		if(@orderList is not null and @orderList <> '')
		begin
			exec dbo.SentOrdersToFinishingProcesses @orderList,'Orders,Order_QtyShip,Order_SizeCode,Order_Qty'
		end
		

		SELECT @orderList =  Stuff((select concat( ',',OrderId) from #tmpDeleteOrder FOR XML PATH('')),1,1,'') 
		----必須刪除的訂單
		if(@orderList is not null and @orderList <> '')
		begin
			----若OrderID不存在資料表，才能使用Order_Delete
			exec dbo.SentOrdersToFinishingProcesses @orderList,'Order_Delete'
		end

		drop table #tmpDeleteOrder
	end

	
	-- Gensong FinishingProcesses
	select @Url = [dbo].[GetWebApiURL]('3A0174', 'FinishingProcesses') 
	
	--需刪除的訂單
	--M 單
	--Cancel 訂單並且是不需要在生產的訂單
	--SNP 請其他間工廠代工的訂單（g. N2E）
	--訂單已轉到其他工廠（此部分就是目前使用 OrderComparisonList 的部分）
	if(isnull(@Url, '') <> '')
	begin
		select a.OrderID into #tmpDeleteOrder_Gensong
		from (select [OrderID] = a.ID
				from orders a with (nolock)
				inner join  Factory f on a.FactoryID = f.ID
				where	((a.AddDate >= @DateStart and a.AddDate < @DateEnd) or
						(a.EditDate >= @DateStart and a.EditDate < @DateEnd) or
						(a.PulloutCmplDate >= @DateStart and a.PulloutCmplDate < @DateEnd)) 
						and
						(a.Category in ('M', '', 'T') or
						(a.junk = 1 and a.NeedProduction = 0) or
						(f.IsProduceFty = 0))
				UNION
				select [OrderID] = ID
				from Orders with (nolock)
				where GMTClose >= @DateStart and Finished = 1
				union
				select distinct ocl.OrderID 
				from OrderComparisonList ocl with (nolock)
				where ocl.UpdateDate >= @DateStart and
					  ocl.DeleteOrder = 1 and
					  not exists(
						select 1
						from orders o with (nolock)
						where ocl.OrderId =  o.ID)) a

		SELECT @orderList =  Stuff((select concat( ',',ID) 
								from Production.dbo.Orders with (nolock)
								where	((AddDate >= @DateStart and AddDate < @DateEnd) or
										(EditDate >= @DateStart and EditDate < @DateEnd) or
										(PulloutCmplDate >= @DateStart and PulloutCmplDate < @DateEnd)) 
										and 
										not exists (select 1 from #tmpDeleteOrder_Gensong t where t.OrderID = Orders.ID)
								FOR XML PATH('')),1,1,'') 

		----必須更新的訂單
		if(@orderList is not null and @orderList <> '')
		begin
			exec dbo.SentOrdersToFinishingProcesses_Gensong @orderList,'Orders,Order_QtyShip'
		end
		

		SELECT @orderList =  Stuff((select concat( ',',OrderId) from #tmpDeleteOrder_Gensong FOR XML PATH('')),1,1,'') 
		----必須刪除的訂單
		if(@orderList is not null and @orderList <> '')
		begin
			----若OrderID不存在資料表，才能使用Order_Delete
			----exec dbo.SentOrdersToFinishingProcesses_Gensong @orderList,'Order_Delete'

			exec dbo.SentOrdersToFinishingProcesses_Gensong @orderList,'Orders,Order_QtyShip'
		end

		drop table #tmpDeleteOrder_Gensong
	end
END