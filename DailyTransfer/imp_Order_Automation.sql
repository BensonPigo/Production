-- =============================================
-- Author:		<Aaron S02109>
-- Create date: <2020/04/15>
-- Description:	�bimp_Order�᭱����A�z�Lweb api�ǰe�����ʹL����Ƶ��t��
-- =============================================
Create PROCEDURE [dbo].[imp_Order_Automation]
AS
BEGIN
	if not exists(select 1 from Production.dbo.System where Automation = 1 )
	begin
		return
	end

	Declare @Url varchar(100)
	--�ǰeOrder_Qty
	select @Url = [dbo].[GetWebApiURL]('3A0197', 'AGV') 
	if(isnull(@Url, '') <> '')
	begin
		exec dbo.SentOrderQtyToAGV
		exec dbo.SentOrderPatternPanelToAGV
	end
	
	--�ǰeOrder��T��Sunrise
------------------------------------------------------------------------------------------------------
--***��ƥ洫�����󭭨�***
--1. �u�����oProduction.dbo.DateInfo
	declare @DateInfoName varchar(30) ='imp_Order_Automation';
	declare @DateStart date= (select DateStart from Production.dbo.DateInfo where name = @DateInfoName);
	declare @DateEnd date  = (select DateEnd   from Production.dbo.DateInfo where name = @DateInfoName);
	declare @Remark nvarchar(max) = (select Remark from Production.dbo.DateInfo where name = @DateInfoName);

--2.���o�w�]��
	if @DateStart is Null
		set @DateStart= CONVERT(DATE,DATEADD(day,-7,GETDATE()))
	if @DateEnd is Null
		set @DateEnd = CONVERT(DATE,DATEADD(day,1,GETDATE()))

--3.��sPms_To_Trade.dbo.dateInfo
if exists(select 1 from Pms_To_Trade.dbo.dateInfo where Name = @DateInfoName )
	update Pms_To_Trade.dbo.dateInfo  set DateStart = @DateStart,DateEnd = @DateEnd, Remark=@Remark where Name = @DateInfoName 
else
	Insert into Pms_To_Trade.dbo.dateInfo(Name,DateStart,DateEnd,Remark)
	values (@DateInfoName,@DateStart,@DateEnd,@Remark);
------------------------------------------------------------------------------------------------------

	declare @orderList nvarchar(max);

	-- Sunrise FinishingProcesses
	select @Url = [dbo].[GetWebApiURL]('3A0134', 'FinishingProcesses') 
	--�ݧR�����q��
	--M ��
	--Cancel �q��åB�O���ݭn�b�Ͳ����q��
	--SNP �Ш�L���u�t�N�u���q��]g. N2E�^
	--�q��w����L�u�t�]�������N�O�ثe�ϥ� OrderComparisonList �������^
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
						(a.Category = 'M' or
						(a.junk = 1 and a.NeedProduction = 0) or
						(f.IsProduceFty = 0))
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

								
		----������s���q��
		if(@orderList is not null and @orderList <> '')
		begin
			exec dbo.SentOrdersToFinishingProcesses @orderList,'Orders,Order_QtyShip,Order_SizeCode,Order_Qty'
		end
		

		SELECT @orderList =  Stuff((select concat( ',',OrderId) from #tmpDeleteOrder FOR XML PATH('')),1,1,'') 
		----�����R�����q��
		if(@orderList is not null and @orderList <> '')
		begin
			----�YOrderID���s�b��ƪ�A�~��ϥ�Order_Delete
			----exec dbo.SentOrdersToFinishingProcesses @orderList,'Order_Delete'

			exec dbo.SentOrdersToFinishingProcesses @orderList,'Orders,Order_QtyShip,Order_SizeCode,Order_Qty'
		end

		drop table #tmpDeleteOrder
	end

	
	-- Gensong FinishingProcesses
	select @Url = [dbo].[GetWebApiURL]('3A0174', 'FinishingProcesses') 
	
	--�ݧR�����q��
	--M ��
	--Cancel �q��åB�O���ݭn�b�Ͳ����q��
	--SNP �Ш�L���u�t�N�u���q��]g. N2E�^
	--�q��w����L�u�t�]�������N�O�ثe�ϥ� OrderComparisonList �������^
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
						(a.Category = 'M' or
						(a.junk = 1 and a.NeedProduction = 0) or
						(f.IsProduceFty = 0))
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

		----������s���q��
		if(@orderList is not null and @orderList <> '')
		begin
			exec dbo.SentOrdersToFinishingProcesses_Gensong @orderList,'Orders,Order_QtyShip'
		end
		

		SELECT @orderList =  Stuff((select concat( ',',OrderId) from #tmpDeleteOrder_Gensong FOR XML PATH('')),1,1,'') 
		----�����R�����q��
		if(@orderList is not null and @orderList <> '')
		begin
			----�YOrderID���s�b��ƪ�A�~��ϥ�Order_Delete
			----exec dbo.SentOrdersToFinishingProcesses_Gensong @orderList,'Order_Delete'

			exec dbo.SentOrdersToFinishingProcesses_Gensong @orderList,'Orders,Order_QtyShip'
		end

		drop table #tmpDeleteOrder_Gensong
	end
END