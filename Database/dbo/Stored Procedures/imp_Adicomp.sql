
-- =============================================
-- Author:		<Willy S01910>
-- Create date: <2016/08/21>
-- Description:	<import adicomp >
-- =============================================
CREATE PROCEDURE [dbo].[imp_Adicomp]
AS
BEGIN

	--------	-- 刪除Production.ADIDASComplain and ADIDASComplain_Detail 在Trade_To_PMS 裡沒有的資料 (根據StartDate判斷轉出月份Range) 

	declare @DetPAD table (id varchar(13))

	delete PAD2
	output deleted.id into @DetPAD
	from Production.dbo.ADIDASComplain_Detail as PAD2 
	left join (
		select PAD1.* from Trade_To_Pms.dbo.ADIDASComplain as TAD1
		inner join Production.dbo.ADIDASComplain as PAD1 on PAD1.ID=TAD1.ID
		where TAD1.StartDate is not null 
		and PAD1.StartDate>=TAD1.StartDate) as PAD1 on PAD2.ID=PAD1.ID
	where PAD1.ID is null

	-- delete 所有ADIDASComplain_Detail剛剛刪除的資料
	delete from Production.dbo.ADIDASComplain  where id in (select ID from @DetPAD)
	

	-------------AdidaComplain-----------------------------------

	Merge Production.dbo.ADIDASComplain as t
	Using Trade_To_Pms.dbo.ADIDASComplain as s
	on t.id=s.id
	when matched then
		update set 
       t.ID	 =s.ID
      ,t.StartDate	      =s.StartDate
      ,t.EndDate	      =s.EndDate
      ,t.AGCCode	      =s.AGCCode
      ,t.FactoryName	      =s.FactoryName
      ,t.Country	      =s.Country
      ,t.AddName	      =s.AddName
      ,t.AddDate	      =s.AddDate
      ,t.EditName	      =s.EditName
      ,t.EditDate	      =s.EditDate
	when not matched by target then
	  insert ( ID,StartDate,EndDate,AGCCode,FactoryName,Country,AddName,AddDate,EditName,EditDate)
	  values ( s.ID,s.StartDate,s.EndDate,s.AGCCode,s.FactoryName,s.Country,s.AddName,s.AddDate,s.EditName,s.EditDate);

	  ------------AdidasComplain_Detail----------------------------------------

	  Merge  Production.dbo.ADIDASComplain_Detail as t
	  using Trade_To_Pms.dbo.ADIDASComplain_Detail as s
	  on  t.ukey=s.ukey
	  when Matched then 
	  update set
      t.SalesID=	      s.SalesID 
      ,t.SalesName=	      s.SalesName 
      ,t.Article=	      s.Article 
      ,t.ArticleName=	      s.ArticleName 
      ,t.ProductionDate=	      s.ProductionDate 
      ,t.DefectMainID=	      s.DefectMainID 
      ,t.DefectSubID=	      s.DefectSubID 
	  ,t.BrandID= s.BrandID
	  ,t.FactoryID= s.FactoryID
      ,t.FOB=	      s.FOB 
      ,t.Qty=	      s.Qty 
      ,t.ValueinUSD=	      s.ValueinUSD 
      ,t.ValueINExRate=	      s.ValueINExRate 
      ,t.OrderID=	      s.OrderID  
      ,t.RuleNo=	      s.RuleNo 
      ,t.UKEY=	      s.UKEY 
	  when not matched by Target then 
	  insert( ID 
      , SalesID 
      , SalesName 
      , Article 
      , ArticleName 
      , ProductionDate 
      , DefectMainID 
      , DefectSubID 
      , FOB 
      , Qty 
      , ValueinUSD 
      , ValueINExRate 
      , OrderID 
      , RuleNo 
      , UKEY
	  ,BrandID
	  ,FactoryID
 
		)
		values( s.ID 
      ,s.SalesID 
      ,s.SalesName 
      ,s.Article 
      ,s.ArticleName 
      ,s.ProductionDate 
      ,s.DefectMainID 
      ,s.DefectSubID 
      ,s.FOB 
      ,s.Qty 
      ,s.ValueinUSD 
      ,s.ValueINExRate 
      ,s.OrderID 
      ,s.RuleNo 
      ,s.UKEY
	  ,s.BrandID
	  ,s.FactoryID 
)
	when not matched by source and t.id in (select id from Trade_To_Pms.dbo.ADIDASComplain WITH (NOLOCK))then
	delete;

	-------------------insert ADIDASComplainDefect---------------

	insert into Production.dbo.ADIDASComplainDefect(id,name,addname,AddDate,EditDate,EditName)
	select id,name,addname,AddDate,EditDate,EditName
	from Trade_To_Pms.dbo.ADIDASComplainDefect  as a WITH (NOLOCK)
	where not exists(select * from Production.dbo.ADIDASComplainDefect b WITH (NOLOCK) where a.ID=b.ID)
	
	------------------insert ADIDASComplainDefect_Detail---------------

	insert into Production.dbo.ADIDASComplainDefect_Detail(id,SubID,SubName)
	select id,SubID,SubName
	from Trade_To_Pms.dbo.ADIDASComplainDefect_Detail  as a WITH (NOLOCK)
	where not exists(select * from Production.dbo.ADIDASComplainDefect_Detail b WITH (NOLOCK) where a.ID=b.ID and a.SubID=b.SubID)

	------------------ADIDASComplainTarget--------------------------------

	Merge Production.dbo.ADIDASComplainTarget as t
	using Trade_To_Pms.dbo.ADIDASComplainTarget as s
	on t.year=s.year
	when matched then 
		update set
	   t.[Target] = s.[Target]
      ,t.[AddName]= s.[AddName]
      ,t.[AddDate]= s.[AddDate]
      ,t.[EditName]= s.[EditName]
      ,t.[EditDate] = s.[EditDate]
	when not matched by Target then 
		insert ([Year]
      ,[Target]
      ,[AddName]
      ,[AddDate]
      ,[EditName]
      ,[EditDate])
	  values(
	  s.[Year]
      ,s.[Target]
      ,s.[AddName]
      ,s.[AddDate]
      ,s.[EditName]
      ,s.[EditDate]
	  )
	  when not matched by source then
	  delete;
	  
	  -----------------------MonthlyQty-----------------------

	  Merge Production.dbo.ADIDASComplain_MonthlyQty as t
	  using  Trade_To_Pms.dbo.ADIDASComplain_MonthlyQty as s
	  on t.YearMonth=s.YearMonth and t.FactoryID=s.FactoryID and t.Brandid=s.brandid
		when not matched by target then 
		insert(yearmonth,factoryid,brandid,qty,addname,adddate)
		values(s.yearmonth,s.factoryid,s.brandid,s.qty,s.addname,s.adddate);
	 ------------------insert ADIDASKPITARGET---------------

	 Merge  Production.dbo.ADIDASKPITARGET as t
	  using Trade_To_Pms.dbo.ADIDASKPITARGET as s
	  on  t.KPIItem=s.KPIItem
	  when Matched then 
	  update set
       t.XlsColumn   = s.XlsColumn   
	  ,t.Description = s.Description	
	  ,t.Target		 = s.Target		
	  ,t.AddName	 = s.AddName		
	  ,t.AddDate	 = s.AddDate		
	  ,t.EditName	 = s.EditName	
	  ,t.EditDate	 = s.EditDate	
	  when not matched by Target then 
	  insert( KPIItem,XlsColumn,Description,Target,AddName,AddDate,EditName,EditDate)
		values( s.KPIItem,s.XlsColumn,s.Description,s.Target,s.AddName,s.AddDate,s.EditName,s.EditDate)
	when not matched by source and t.KPIItem in (select KPIItem from Trade_To_Pms.dbo.ADIDASKPITARGET WITH (NOLOCK))then
	delete;
END