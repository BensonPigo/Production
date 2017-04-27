

-- =============================================
-- Author:		<Willy S01910>
-- Create date: <2016/08/24>
-- Description:	<import MockupOrder>
-- =============================================
Create PROCEDURE [dbo].[imp_MockupOrder]
AS
BEGIN
	SET NOCOUNT ON;

	declare @oldDate date = (select max(UpdateDate) from Production.dbo.OrderComparisonList WITH (NOLOCK)) --�W���פJ���̫���

	declare @Sayfty table(id varchar(10)) --�u�t�N�X
	insert @Sayfty select id from Production.dbo.Factory

	declare @dToDay date = CONVERT(date, GETDATE()) --���Ѥ��
	declare @Odate_s date  = (select DateStart from Trade_To_Pms.dbo.DateInfo WITH (NOLOCK) where name='MockupOrder')
			

		---------------�s�W Temp MockupOrder ----------------------------
		select a.*,b.MDivisionID, b.FTYGroup 
		into #tempMO
		from Trade_To_Pms.dbo.MockupOrder a WITH (NOLOCK) 
		inner join Production.dbo.factory b on a.FactoryID = b.ID

	------------------MockupOrder--------------------------------------------------------------

		declare @mockT table (ID varchar(13),isInsert bit)

		Merge Production.dbo.MockupOrder as t
		Using #tempMO as s
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
		when not matched by target then  -------go to Merge2
			insert (ID ,MockupID ,Description ,Cpu ,BrandID ,StyleID ,SeasonID ,ProgramID ,FactoryID ,Qty ,CfmDate 
			,SCIDelivery ,MRHandle ,SMR ,Junk ,Remark ,CMPUnit ,CMPPrice ,FTYGroup ,CPUFactor ,MDivisionID ,AddName 
			,AddDate ,EditName ,EditDate )
			values(s.ID ,s.MockupID ,s.Description ,s.Cpu ,s.BrandID ,s.StyleID ,s.SeasonID ,s.ProgramID ,s.FactoryID ,s.Qty ,s.CfmDate
			,s.SCIDelivery,s.MRHandle ,s.SMR ,s.Junk ,s.Remark ,s.CMPUnit ,s.CMPPrice ,FTYGroup ,3 ,s.MDivisionID,s.AddName 
			,s.AddDate ,s.EditName ,s.EditDate )
		output inserted.id, iif(deleted.id='',1,0) into @mockT; --�Ninsert =1 , update =0 ����ܹL��id output
		
		---------Merge2
		Merge Production.dbo.OrderComparisonList as t
		Using (select * from  #tempMO where id in(select id from @mockT where isInsert=1)) as s
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

		------Merge3
		Merge Production.dbo.OrderComparisonList as t
		Using (select a.*,iif(a.qty <> b.qty,1,0) as diffQty,iif(a.styleid <> b.styleid,1,0) as diffStyle, iif(a.SCIDelivery <> b.SCIDelivery,1,0) as diffScid,
			b.Qty as Pqty, b.SCIDelivery as Pscidlv, b.styleid as Pstyle
			from  #tempMO  a
			inner join Production.dbo.MockupOrder b WITH (NOLOCK) on a.id=b.id		
			where (a.qty <> b.qty or a.styleid <> b.styleid or a.SCIDelivery <> b.SCIDelivery)
			and a.id not in (select id from @mockT where isInsert=1)
			) as s
		on orderid=s.id and t.factoryid=s.factoryid and t.updateDate = @dToDay
			when matched then
				update set
				--		Qty ���ۦP
				t.OriginalQty=IIF( s.diffQty=1,s.Pqty,t.OriginalQty),
				t.NewQty =iif(s.diffQty=1, s.qty,t.NewQty),
				--		SCIDelivery ���P
				t.OriginalSCIDelivery =iif(s.diffScid=1, s.Pscidlv,t.OriginalSCIDelivery),
				t.NewSCIDelivery =iif(s.diffScid=1, s.SCIDelivery,t.NewSCIDelivery),
				--		StyleID ���P
				t.OriginalStyleID =iif(s.diffStyle=1, s.Pstyle,t.OriginalStyleID),				
				t.NewStyleID =iif(s.diffStyle=1, s.StyleID,t.NewStyleID)			
			when not matched by Target then
				insert(orderid,UpdateDate,TransferDate,FactoryID,OriginalQty,NewQty,OriginalSCIDelivery,NewSCIDelivery,OriginalStyleID,NewStyleID)
				values(s.id,@dToDay,@oldDate,FactoryID,s.Pqty,s.qty,s.Pscidlv,s.SCIDelivery,s.Pstyle,s.StyleID);

	-----------------MockupOrder again------------------------
	
	Merge Production.dbo.OrderComparisonList as t
	Using (select a.*,b.factoryid as Tfactoryid from Production.dbo.MockupOrder a  WITH (NOLOCK)
		inner join Trade_To_Pms.dbo.MockupOrder b WITH (NOLOCK) on a.id=b.id
		where a.SCIDelivery >=@Odate_s
		and b.factoryID not in (select id from @Sayfty)) as s
	on t.orderid=s.id and t.factoryid=s.factoryid and t.updateDate = @dToDay
		when matched then
			update set
			t.TransferDate=@oldDate,
			t.OriginalQty =s.qty,
			t.OriginalStyleID=s.styleID,
			t.DeleteOrder=1,
			t.NewStyleID=iif(s.factoryid is not null,s.Tfactoryid,t.NewStyleID)

		when not matched by target then
			insert(orderid,UpdateDate,FactoryID,TransferDate,OriginalQty,OriginalStyleID,DeleteOrder,NewStyleID)
			values(s.id,@dToDay,s.FactoryID,@oldDate,s.qty,s.styleID,1,iif(s.factoryid is not null,s.Tfactoryid,''));

	----------------Delete MockOrder ---------------------------------------------------
		
		delete a 
		from Production.dbo.MockupOrder a
		inner join Trade_To_Pms.dbo.MockupOrder b on a.id=b.id and a.FactoryID not in (select id from @Sayfty)
		where a.SCIDelivery >=@Odate_s
	
	drop table #tempMO
END






