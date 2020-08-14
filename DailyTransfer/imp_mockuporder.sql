

-- =============================================
-- Author:		<Willy S01910>
-- Create date: <2016/08/24>
-- Description:	<import MockupOrder>
-- =============================================
Create PROCEDURE [dbo].[imp_MockupOrder]
AS
BEGIN
	SET NOCOUNT ON;
	
------------------------------------------------------------------------------------------------------
--***資料交換的條件限制***
--1. 優先取得Production.dbo.DateInfo
	declare @DateInfoName varchar(30) ='imp_MockupOrder_OldDate';
	declare @oldDate date= (select DateEnd from Production.dbo.DateInfo where name = @DateInfoName);

--2.取得預設值
	if @oldDate is Null
		set @oldDate= (select DateEnd from Trade_To_Pms.dbo.DateInfo WITH (NOLOCK) where name='imp_Order_OldDate')	

--3.更新Pms_To_Trade.dbo.dateInfo
	Delete Pms_To_Trade.dbo.dateInfo Where Name = @DateInfoName 
	Insert into Pms_To_Trade.dbo.dateInfo(Name,DateStart,DateEnd)
	values (@DateInfoName,@oldDate,@oldDate);
------------------------------------------------------------------------------------------------------
--***資料交換的條件限制***
--1. 優先取得Production.dbo.DateInfo
	Set @DateInfoName ='imp_MockupOrder_dToDay';
	declare @dToDay date= (select DateEnd from Production.dbo.DateInfo where name = @DateInfoName);

--2.取得預設值
	if @dToDay is Null
		set @dToDay= (select DateEnd from Trade_To_Pms.dbo.DateInfo WITH (NOLOCK) where name='imp_Order_dToDay')	

--3.更新Pms_To_Trade.dbo.dateInfo
	Delete Pms_To_Trade.dbo.dateInfo Where Name = @DateInfoName 
	Insert into Pms_To_Trade.dbo.dateInfo(Name,DateStart,DateEnd)
	values (@DateInfoName,@dToDay,@dToDay);
------------------------------------------------------------------------------------------------------
--***資料交換的條件限制***
--1. 優先取得Production.dbo.DateInfo
	Set @DateInfoName ='imp_MockupOrder';
	declare @Odate_s date= (select DateStart from Production.dbo.DateInfo where name = @DateInfoName);

--2.取得預設值
	if @Odate_s is Null
		set @Odate_s= (select DateStart from Trade_To_Pms.dbo.DateInfo WITH (NOLOCK) where name = 'MockupOrder')	

--3.更新Pms_To_Trade.dbo.dateInfo
	Delete Pms_To_Trade.dbo.dateInfo Where Name = @DateInfoName 
	Insert into Pms_To_Trade.dbo.dateInfo(Name,DateStart,DateEnd)
	values (@DateInfoName,@Odate_s,@Odate_s);
------------------------------------------------------------------------------------------------------

		---------------From Trade MockupOrder ----------------------------

		select a.*,b.MDivisionID, b.FTYGroup 
		into #temp_MockupOrder_Trade
		from Trade_To_Pms.dbo.MockupOrder a WITH (NOLOCK) 
		inner join Production.dbo.factory b on a.FactoryID = b.ID

		---------------Backup PMS MockupOrder-------------------------
		select * 
		into #temp_MockupOrder_PMS
		from Production.dbo.MockupOrder

	------------------Insert & Update MockupOrder--------------------------------------------------------------	
		Merge Production.dbo.MockupOrder as t
		Using #temp_MockupOrder_Trade as s
		on t.id=s.id
		when matched then
			update set 
			t.MockupID = s.MockupID ,
			t.Description = s.Description ,
			t.Cpu = s.Cpu ,
			t.BrandID = s.BrandID ,
			t.StyleID = s.StyleID ,
			t.SeasonID = s.SeasonID ,
			t.ProgramID = s.ProgramID ,
			t.FactoryID = s.FactoryID ,
			t.Qty = s.Qty ,
			t.CfmDate = s.CfmDate ,
			t.SCIDelivery = s.SCIDelivery ,
			t.MRHandle = s.MRHandle ,
			t.SMR = s.SMR ,
			t.Junk = s.Junk ,
			t.Remark = s.Remark ,
			t.CMPUnit = s.CMPUnit ,
			t.CMPPrice = s.CMPPrice ,
			t.FTYGroup = s.FTYGroup ,
			t.CPUFactor =1 ,
			t.MDivisionID = s.MDivisionID ,
			t.AddName = s.AddName ,
			t.AddDate = s.AddDate ,
			t.EditName = iif(s.EditDate<=t.EditDate,t.EditName,s.EditName) ,
			t.EditDate = iif(s.EditDate<=t.EditDate,t.EditDate,s.EditDate) 
		when not matched by target then 
			insert (ID ,MockupID ,Description ,Cpu ,BrandID ,StyleID ,SeasonID ,ProgramID ,FactoryID ,Qty ,CfmDate 
			,SCIDelivery ,MRHandle ,SMR ,Junk ,Remark ,CMPUnit ,CMPPrice ,FTYGroup ,CPUFactor ,MDivisionID ,AddName 
			,AddDate ,EditName ,EditDate )
			values(s.ID ,s.MockupID ,s.Description ,s.Cpu ,s.BrandID ,s.StyleID ,s.SeasonID ,s.ProgramID ,s.FactoryID ,s.Qty ,s.CfmDate
			,s.SCIDelivery,s.MRHandle ,s.SMR ,s.Junk ,s.Remark ,s.CMPUnit ,s.CMPPrice ,FTYGroup ,1 ,s.MDivisionID,s.AddName 
			,s.AddDate ,s.EditName ,s.EditDate );

		---------Merge2 insert-----------------
		Merge Production.dbo.OrderComparisonList as t
		Using (select * from  #temp_MockupOrder_Trade a 
		where not exists(select 1 from #temp_MockupOrder_PMS b where a.id=b.id)) as s
		on t.orderid=s.id and t.factoryid=s.factoryid and t.updateDate = @dToDay
			when matched then
			update set
				t.NewQty = s.Qty,
				t.NewSCIDelivery=s.SCIDelivery,
				t.NewStyleID=s.StyleID,
				t.TransferDate=@oldDate,
				t.NewOrder='1',		
				t.MDivisionID=s.MDivisionID		
			when not matched by target then
				insert(OrderId,UpdateDate,FactoryID,NewQty,NewSCIDelivery,NewStyleID,TransferDate,NewOrder,MDivisionID)
				values(s.id,@dToDay,s.FactoryID, s.Qty,s.SCIDelivery,s.StyleID,@oldDate,'1',s.MDivisionID);

		----------Merge3 Update---------------1.Qty-----------------(2017/05/10 Qty styleid SCIDelivery 分開CHECK)
		Merge Production.dbo.OrderComparisonList as t
		Using (
		select a.*,
		[t_ID]=a.id,
		[P.ID]=b.id,
		[T_Qty]=a.qty,
		[P_Qty]=b.qty,
		[T_SCIDelivery]=a.SCIDelivery,
		[P_SCIDelivery]=b.SCIDelivery,
		[T_StyleID]=a.StyleID,
		[P_StyleID]=b.StyleID
			from  #temp_MockupOrder_Trade  a
			inner join #temp_MockupOrder_PMS b on a.id=b.id		
			where (a.qty <> b.qty )--or a.styleid <> b.styleid or a.SCIDelivery <> b.SCIDelivery)			
			) as s
		on orderid=s.id and t.factoryid=s.factoryid and t.updateDate = @dToDay
			when matched then
				update set
				t.OriginalQty=s.[P_Qty],
				t.NewQty =s.[T_Qty]
				--t.OriginalSCIDelivery =s.[P_SCIDelivery],
				--t.NewSCIDelivery =s.[T_SCIDelivery],--				
				--t.OriginalStyleID =s.[P_StyleID],	--			
				--t.NewStyleID =s.[T_StyleID],
				--t.MDivisionID=s.MDivisionID			
			when not matched by Target then
			insert(orderid,UpdateDate ,TransferDate ,FactoryID,OriginalQty ,NewQty ,MDivisionID)
			values(s.id   ,@dToDay    ,@oldDate     ,FactoryID,s.[P_Qty]   ,s.[T_Qty],s.MDivisionID);
				--insert(orderid,UpdateDate ,TransferDate ,FactoryID,OriginalQty ,NewQty   ,OriginalSCIDelivery ,NewSCIDelivery   ,OriginalStyleID,NewStyleID,MDivisionID)
				--values(s.id   ,@dToDay    ,@oldDate     ,FactoryID,s.[P_Qty]   ,s.[T_Qty],s.[P_SCIDelivery]   ,s.[T_SCIDelivery],s.[P_StyleID]  ,s.[T_StyleID],s.MDivisionID);
				----------Merge3 Update---------------2.styleid-----------------(2017/05/10 Qty styleid SCIDelivery 分開CHECK)
		Merge Production.dbo.OrderComparisonList as t
		Using (
		select a.*,
		[t_ID]=a.id,
		[P.ID]=b.id,
		[T_Qty]=a.qty,
		[P_Qty]=b.qty,
		[T_SCIDelivery]=a.SCIDelivery,
		[P_SCIDelivery]=b.SCIDelivery,
		[T_StyleID]=a.StyleID,
		[P_StyleID]=b.StyleID
			from  #temp_MockupOrder_Trade  a
			inner join #temp_MockupOrder_PMS b on a.id=b.id		
			where (a.styleid <> b.styleid)			
			) as s
		on orderid=s.id and t.factoryid=s.factoryid and t.updateDate = @dToDay
			when matched then
				update set		
				t.OriginalStyleID =s.[P_StyleID],
				t.NewStyleID =s.[T_StyleID]			
			when not matched by Target then
				insert(orderid,UpdateDate ,TransferDate ,FactoryID,OriginalStyleID,NewStyleID,MDivisionID)
				values(s.id   ,@dToDay    ,@oldDate     ,FactoryID,s.[P_StyleID]  ,s.[T_StyleID],s.MDivisionID);
----------Merge3 Update---------------3.SCIDelivery-----------------(2017/05/10 Qty styleid SCIDelivery 分開CHECK)
		Merge Production.dbo.OrderComparisonList as t
		Using (
		select a.*,
		[t_ID]=a.id,
		[P.ID]=b.id,
		[T_Qty]=a.qty,
		[P_Qty]=b.qty,
		[T_SCIDelivery]=a.SCIDelivery,
		[P_SCIDelivery]=b.SCIDelivery,
		[T_StyleID]=a.StyleID,
		[P_StyleID]=b.StyleID
			from  #temp_MockupOrder_Trade  a
			inner join #temp_MockupOrder_PMS b on a.id=b.id		
			where (a.SCIDelivery <> b.SCIDelivery)			
			) as s
		on orderid=s.id and t.factoryid=s.factoryid and t.updateDate = @dToDay
			when matched then
				update set
				t.OriginalSCIDelivery =s.[P_SCIDelivery],
				t.NewSCIDelivery =s.[T_SCIDelivery]				
			when not matched by Target then
				insert(orderid,UpdateDate ,TransferDate ,FactoryID ,OriginalSCIDelivery ,NewSCIDelivery,MDivisionID)
				values(s.id   ,@dToDay    ,@oldDate     ,FactoryID,s.[P_SCIDelivery]   ,s.[T_SCIDelivery],s.MDivisionID);
				----------Merge3 Update---------------4.單獨JUNK判斷-----------------(2017/05/10 Qty styleid SCIDelivery 分開CHECK)
		Merge Production.dbo.OrderComparisonList as t
		Using (
		select a.*,
		[t_ID]=a.id,
		[P.ID]=b.id,
		[T_Qty]=a.qty,
		[P_Qty]=b.qty,
		[T_SCIDelivery]=a.SCIDelivery,
		[P_SCIDelivery]=b.SCIDelivery,
		[T_StyleID]=a.StyleID,
		[P_StyleID]=b.StyleID
			from  #temp_MockupOrder_Trade  a
			inner join #temp_MockupOrder_PMS b on a.id=b.id		
			where (a.junk <> b.junk)			
			) as s
		on orderid=s.id and t.factoryid=s.factoryid and t.updateDate = @dToDay
			when matched then
				update set
				t.JunkOrder =s.junk;
	-----------------MockupOrder again(Delete)------------------------
	
	--需要刪除的資料-#MockuporderANGIN
	SELECT [TRADE_Factory]=C.FactoryID,a.* 
	into #MockuporderANGIN
	FROM Production.dbo.MockupOrder A
	INNER JOIN Trade_To_Pms.dbo.MockupOrder C ON A.ID=C.ID	
	WHERE NOT exists(select 1 from Trade_To_Pms.dbo.MockupOrder b where a.ID=b.ID)
	or a.FactoryID not in (select id from Production.dbo.Factory)
	and a.SCIDelivery>=@Odate_s
	

	Merge Production.dbo.OrderComparisonList as t
	Using #MockuporderANGIN as s
	on t.orderid=s.id and t.factoryid=s.factoryid and t.updateDate = @dToDay
		when matched then
			update set
			t.TransferDate=@oldDate,
			t.OriginalQty =s.qty,
			t.OriginalStyleID=s.styleID,
			t.DeleteOrder=1,
			t.NewStyleID=iif(s.factoryid is not null,s.[TRADE_Factory],t.NewStyleID),
			t.MDivisionID=s.MDivisionID	
		when not matched by target then
			insert(orderid,UpdateDate,FactoryID  ,TransferDate,OriginalQty,OriginalStyleID,DeleteOrder,NewStyleID,MDivisionID)
			values(s.id	  ,@dToDay   ,s.FactoryID,@oldDate    ,s.qty      ,s.styleID      ,1          ,iif(s.factoryid is not null,s.[TRADE_Factory],''),s.MDivisionID);

	----------------Delete MockOrder ---------------------------------------------------
		
		delete a 
		from Production.dbo.MockupOrder a
		inner join #MockuporderANGIN b on a.id=b.id 
	
	drop table #temp_MockupOrder_Trade,#MockuporderANGIN,#temp_MockupOrder_PMS
END







