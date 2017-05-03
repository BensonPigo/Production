

-- =============================================
-- Author:		<Willy S01910>
-- Create date: <2016/08/24>
-- Description:	<import MockupOrder>
-- =============================================
Create PROCEDURE [dbo].[imp_MockupOrder]
AS
BEGIN
	SET NOCOUNT ON;

	declare @oldDate date = (select top 1 DateEnd from  Trade_To_Pms.dbo.DateInfo where Name='imp_Order_OldDate')
	declare @dToDay date = (select top 1 DateEnd from  Trade_To_Pms.dbo.DateInfo where Name='imp_Order_dToDay')

	--declare @oldDate date = (select max(UpdateDate) from Production.dbo.OrderComparisonList WITH (NOLOCK)) --上次匯入的最後日期
	--declare @dToDay date = CONVERT(date, GETDATE()) --今天日期
	declare @Sayfty table(id varchar(10)) --工廠代碼
	insert @Sayfty select id from Production.dbo.Factory

	
	declare @Odate_s date  = (select DateStart from Trade_To_Pms.dbo.DateInfo WITH (NOLOCK) where name='MockupOrder')
			

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
		when matched and s.qty = t.qty or s.styleid = t.styleid or s.SCIDelivery = t.SCIDelivery then
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
			t.CPUFactor =3 ,
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
			,s.SCIDelivery,s.MRHandle ,s.SMR ,s.Junk ,s.Remark ,s.CMPUnit ,s.CMPPrice ,FTYGroup ,3 ,s.MDivisionID,s.AddName 
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
				t.NewOrder='1'				
			when not matched by target then
				insert(OrderId,UpdateDate,FactoryID,NewQty,NewSCIDelivery,NewStyleID,TransferDate,NewOrder)
				values(s.id,@dToDay,s.FactoryID, s.Qty,s.SCIDelivery,s.StyleID,@oldDate,'1');

		----------Merge3 Update--------------------------------
		Merge Production.dbo.OrderComparisonList as t
		Using (
		select a.*,
		[t_ID]=a.id,[P.ID]=b.id,
		[T_Qty]=a.qty,[P_Qty]=b.qty,
		[T_SCIDelivery]=a.SCIDelivery,[P_SCIDelivery]=b.SCIDelivery,
		[T_StyleID]=a.StyleID,[P_StyleID]=b.StyleID
			from  #temp_MockupOrder_Trade  a
			inner join #temp_MockupOrder_PMS b on a.id=b.id		
			where (a.qty <> b.qty or a.styleid <> b.styleid or a.SCIDelivery <> b.SCIDelivery)			
			) as s
		on orderid=s.id and t.factoryid=s.factoryid and t.updateDate = @dToDay
			when matched then
				update set
				t.OriginalQty=s.[P_Qty],
				t.NewQty =s.[T_Qty],
				t.OriginalSCIDelivery =s.[P_SCIDelivery],
				t.NewSCIDelivery =s.[T_SCIDelivery],				
				t.OriginalStyleID =s.[P_StyleID],				
				t.NewStyleID =s.[T_StyleID]			
			when not matched by Target then
				insert(orderid,UpdateDate ,TransferDate ,FactoryID,OriginalQty ,NewQty   ,OriginalSCIDelivery ,NewSCIDelivery   ,OriginalStyleID,NewStyleID)
				values(s.id   ,@dToDay    ,@oldDate     ,FactoryID,s.[P_Qty]   ,s.[T_Qty],s.[P_SCIDelivery]   ,s.[T_SCIDelivery],s.[P_StyleID]  ,s.[T_StyleID]);

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
			t.NewStyleID=iif(s.factoryid is not null,s.[TRADE_Factory],t.NewStyleID)

		when not matched by target then
			insert(orderid,UpdateDate,FactoryID  ,TransferDate,OriginalQty,OriginalStyleID,DeleteOrder,NewStyleID)
			values(s.id	  ,@dToDay   ,s.FactoryID,@oldDate    ,s.qty      ,s.styleID      ,1          ,iif(s.factoryid is not null,s.[TRADE_Factory],''));

	----------------Delete MockOrder ---------------------------------------------------
		
		delete a 
		from Production.dbo.MockupOrder a
		inner join #MockuporderANGIN b on a.id=b.id 
	
	drop table #temp_MockupOrder_Trade,#MockuporderANGIN,#temp_MockupOrder_PMS
END






