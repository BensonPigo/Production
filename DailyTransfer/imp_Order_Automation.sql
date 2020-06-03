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
	end
	
	--�ǰeOrder��T��Sunrise
	declare @startDate date = getdate() - 7;
	declare @endDate date = getdate() + 1;
	declare @orderList nvarchar(max);

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
				where	((a.AddDate >= @startDate and a.AddDate < @endDate) or
						(a.EditDate >= @startDate and a.EditDate < @endDate) or
						(a.PulloutCmplDate >= @startDate and a.PulloutCmplDate < @endDate)) 
						and
						(a.Category = 'M' or
						(a.junk = 1 and a.NeedProduction = 0) or
						(f.IsProduceFty = 0))
				union
				select distinct ocl.OrderID 
				from OrderComparisonList ocl with (nolock)
				where ocl.UpdateDate >= @startDate and
					  ocl.DeleteOrder = 1 and
					  not exists(
						select 1
						from orders o with (nolock)
						where ocl.OrderId =  o.ID)) a

		SELECT @orderList =  Stuff((select concat( ',',ID) 
								from Production.dbo.Orders with (nolock)
								where	((AddDate >= @startDate and AddDate < @endDate) or
										(EditDate >= @startDate and EditDate < @endDate) or
										(PulloutCmplDate >= @startDate and PulloutCmplDate < @endDate)) 
										and 
										not exists (select 1 from #tmpDeleteOrder t where t.OrderID = Orders.ID)
								FOR XML PATH('')),1,1,'') 
		if(@orderList is not null and @orderList <> '')
		begin
			exec dbo.SentOrdersToFinishingProcesses @orderList,'Orders,Order_QtyShip,Order_SizeCode'
		end
		

		SELECT @orderList =  Stuff((select concat( ',',OrderId) from #tmpDeleteOrder FOR XML PATH('')),1,1,'') 

		if(@orderList is not null and @orderList <> '')
		begin
			exec dbo.SentOrdersToFinishingProcesses @orderList,'Order_Delete'
		end

		drop table #tmpDeleteOrder
	end

END