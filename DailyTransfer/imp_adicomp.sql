
-- =============================================
-- Author:		<Willy S01910>
-- Create date: <2016/08/21>
-- Description:	<import adicomp >
-- =============================================
Create PROCEDURE [dbo].[imp_Adicomp]
AS
BEGIN
	------------AdidaComplain_History & AdidaComplain_Detail_History------------
	insert into Production.dbo.ADIDASComplain_History(
			ID
			,Version
			,StartDate
			,EndDate
			,AGCCode
			,FactoryName
			,Country
			,AddName
			,AddDate
			,EditName
			,EditDate
			,TPEApvName
			,TPEApvDate
			,FtyApvName
			,FtyApvDate)
		select	
		 isnull(adc.ID, '')
		,(select isnull(max(version),0) + 1 from Production.dbo.ADIDASComplain_History where ID = adc.ID)
		,adc.StartDate
		,adc.EndDate
		,isnull(adc.AGCCode    , '')
		,isnull(adc.FactoryName, '')
		,isnull(adc.Country    , '')
		,isnull(adc.AddName    , '')
		,adc.AddDate
		,isnull(adc.EditName   , '')
		,adc.EditDate
		,isnull(adc.TPEApvName , '')
		,adc.TPEApvDate
		,isnull(adc.FtyApvName , '')
		,adc.FtyApvDate
		from Production.dbo.ADIDASComplain adc with (nolock)
		inner join Trade_To_Pms.dbo.ADIDASComplain adcTrade on adc.ID = adcTrade.ID and adc.TPEApvDate <> adcTrade.TPEApvDate

	insert into Production.dbo.ADIDASComplain_Detail_History(
											UKEY
											,Version
											,ID
											,SalesID
											,SalesName
											,Article
											,ArticleName
											,ProductionDate
											,DefectMainID
											,DefectSubID
											,FOB
											,Qty
											,ValueinUSD
											,ValueINExRate
											,OrderID
											,RuleNo
											,BrandID
											,FactoryID
											,StyleID
											,SuppID
											,Refno
											,CustPONo
											,SeasonId
											,BulkMR
											,SampleMR
											,IsEM
											,Responsibility)
				select
						isnull(adch.UKEY,0)
						,(select isnull(max(version),0) + 1 from Production.dbo.ADIDASComplain_Detail_History where Ukey = adch.Ukey)
						,isnull(adch.ID              ,'')
						,isnull(adch.SalesID         ,'')
						,isnull(adch.SalesName       ,'')
						,isnull(adch.Article         ,'')
						,isnull(adch.ArticleName     ,'')
						,adch.ProductionDate
						,isnull(adch.DefectMainID    ,'')
						,isnull(adch.DefectSubID     ,'')
						,isnull(adch.FOB             ,0)
						,isnull(adch.Qty             ,0)
						,isnull(adch.ValueinUSD      ,0)
						,isnull(adch.ValueINExRate   ,0)
						,isnull(adch.OrderID         ,'')
						,isnull(adch.RuleNo          ,0)
						,isnull(adch.BrandID         ,'')
						,isnull(adch.FactoryID       ,'')
						,isnull(adch.StyleID         ,'')
						,isnull(adch.SuppID,'')
						,ISNULL(adch.Refno,'')
						,ISNULL(adch.CustPONo,'')
						,ISNULL(adch.SeasonId,'')
						,ISNULL(adch.BulkMR,'')
						,ISNULL(adch.SampleMR,'')
						,ISNULL(adch.IsEM,0)
						, adch.Responsibility
				from Production.dbo.ADIDASComplain_Detail adch with (nolock)
				where exists(select 1 from Production.dbo.ADIDASComplain a
								inner join Trade_To_Pms.dbo.ADIDASComplain b on a.ID =b.ID and a.TPEApvDate <> b.TPEApvDate
								 where a.ID = adch.ID)
								 		
	-------------------insert ADIDASComplainDefect---------------

	delete from Production.dbo.ADIDASComplainDefect;

	insert into Production.dbo.ADIDASComplainDefect(id,name,addname,AddDate,EditDate,EditName)
    select ISNULL(a.id, '')
         , ISNULL(a.name, '')
         , ISNULL(a.addname, '')
         , a.AddDate
         , a.EditDate
         , ISNULL(a.EditName, '')
	from Trade_To_Pms.dbo.ADIDASComplainDefect  as a WITH (NOLOCK)
	where not exists(select * from Production.dbo.ADIDASComplainDefect b WITH (NOLOCK) where a.ID=b.ID)
	
	------------------insert ADIDASComplainDefect_Detail---------------

	delete from Production.dbo.ADIDASComplainDefect_Detail;
	insert into Production.dbo.ADIDASComplainDefect_Detail(id,SubID,SubName)
    select ISNULL(a.id, '')
         , ISNULL(a.SubID, '')
         , ISNULL(a.SubName, '')
	from Trade_To_Pms.dbo.ADIDASComplainDefect_Detail  as a WITH (NOLOCK)
	where not exists(select * from Production.dbo.ADIDASComplainDefect_Detail b WITH (NOLOCK) where a.ID=b.ID and a.SubID=b.SubID)
	
	------------------insert ADIDASComplainDefect_FabricType---------------

	delete from Production.dbo.ADIDASComplainDefect_FabricType;
	insert into Production.dbo.ADIDASComplainDefect_FabricType(id,SubID,FabricType,[Responsibility],[MtlTypeID])
    select ISNULL(a.id, '')
         , ISNULL(a.SubID, '')
         , ISNULL(a.FabricType, '')
         , ISNULL(a.Responsibility, '')
         , ISNULL(a.MtlTypeID, '')
	from Trade_To_Pms.dbo.ADIDASComplainDefect_FabricType  as a WITH (NOLOCK)
	where not exists(select * from Production.dbo.ADIDASComplainDefect_FabricType b WITH (NOLOCK) where a.ID=b.ID and a.SubID=b.SubID and a.FabricType=b.FabricType)
	-------------AdidaComplain-----------------------------------

	Merge Production.dbo.ADIDASComplain as t
	Using Trade_To_Pms.dbo.ADIDASComplain as s
	on t.id=s.id
	when matched and t.TPEApvDate <> s.TPEApvDate then 
		update set 
      t.StartDate	      = s.StartDate
      ,t.EndDate	      = s.EndDate
      ,t.AGCCode	      = ISNULL(s.AGCCode, '')
      ,t.FactoryName	  = ISNULL(s.FactoryName, '')
      ,t.Country	      = ISNULL(s.Country, '')
      ,t.AddName	      = ISNULL(s.AddName, '')
      ,t.AddDate	      = s.AddDate
      ,t.EditName	      = ISNULL(s.EditName, '')
      ,t.EditDate	      = s.EditDate
	  ,t.TPEApvName	      = ISNULL(s.TPEApvName, '')
	  ,t.TPEApvDate	      = s.TPEApvDate
	  ,t.FtyApvName	      = ''
	  ,t.FtyApvDate	      = null
	  ,t.Junk			  = ISNULL(s.Junk, 0)
	when not matched by target then
	  insert ( ID,StartDate,EndDate,AGCCode,FactoryName,Country,AddName,AddDate,EditName,EditDate,TPEApvName,TPEApvDate ,Junk)
    values
    (ISNULL(s.ID, '')
   , s.StartDate
   , s.EndDate
   , ISNULL(s.AGCCode, '')
   , ISNULL(s.FactoryName, '')
   , ISNULL(s.Country, '')
   , ISNULL(s.AddName, '')
   , s.AddDate
   , ISNULL(s.EditName, '')
   , s.EditDate
   , ISNULL(s.TPEApvName, '')
   , s.TPEApvDate
   , ISNULL(Junk, 0)
    );

	  ------------AdidasComplain_Detail----------------------------------------
    Merge Production.dbo.ADIDASComplain_Detail as t
    using Trade_To_Pms.dbo.ADIDASComplain_Detail as s
    on t.ukey = s.ukey
    when Matched and exists
                     (
                         select 1
                         from Production.dbo.ADIDASComplain             a
                             inner join Trade_To_Pms.dbo.ADIDASComplain b
                                 on a.ID = b.ID
                                    and a.TPEApvDate <> b.TPEApvDate
                         where a.ID = t.ID
                     ) then
        update set t.id = ISNULL(s.id, '')
                 , t.SalesID = ISNULL(s.SalesID, '')
                 , t.SalesName = ISNULL(s.SalesName, '')
                 , t.Article = ISNULL(s.Article, '')
                 , t.ArticleName = ISNULL(s.ArticleName, '')
                 , t.ProductionDate = s.ProductionDate
                 , t.DefectMainID = ISNULL(s.DefectMainID, '')
                 , t.DefectSubID = ISNULL(s.DefectSubID, '')
                 , t.BrandID = ISNULL(s.BrandID, '')
                 , t.FactoryID = ISNULL(s.FactoryID, '')
                 , t.FOB = ISNULL(s.FOB, 0)
                 , t.Qty = ISNULL(s.Qty, 0)
                 , t.ValueinUSD = ISNULL(s.ValueinUSD, 0)
                 , t.ValueINExRate = ISNULL(s.ValueINExRate, 0)
                 , t.OrderID = ISNULL(s.OrderID, '')
                 , t.RuleNo = ISNULL(s.RuleNo, 0)
                 , t.StyleID = ISNULL(s.StyleID, '')
                 , t.SuppID = ISNULL(s.SuppID, '')
                 , t.SeasonId = ISNULL(s.SeasonId, '')
                 , t.BulkMR = ISNULL(s.BulkMRHandle, '')
                 , t.SampleMR = ISNULL(s.SampleMRHandle, '')
                 , t.Refno = ISNULL(s.Refno, '')
                 , t.CustPONo = ISNULL(s.CustPONo, '')
                 , t.IsEM = ISNULL(s.IsEM, 0)
                 , t.Responsibility = s.Responsibility
    when not matched by Target then
        insert
        (
            ID
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
          , BrandID
          , FactoryID
          , StyleID
          , SuppID
          , SeasonId
          , BulkMR
          , SampleMR
          , Refno
          , CustPONo
          , IsEM
          , Responsibility
        )
        values
        (ISNULL(s.ID, '')
       , ISNULL(s.SalesID, '')
       , ISNULL(s.SalesName, '')
       , ISNULL(s.Article, '')
       , ISNULL(s.ArticleName, '')
       , s.ProductionDate
       , ISNULL(s.DefectMainID, '')
       , ISNULL(s.DefectSubID, '')
       , ISNULL(s.FOB, 0)
       , ISNULL(s.Qty, 0)
       , ISNULL(s.ValueinUSD, 0)
       , ISNULL(s.ValueINExRate, 0)
       , ISNULL(s.OrderID, '')
       , ISNULL(s.RuleNo, 0)
       , ISNULL(s.UKEY, 0)
       , ISNULL(s.BrandID, '')
       , ISNULL(s.FactoryID, '')
       , ISNULL(s.StyleID, '')
       , ISNULL(s.SuppID, '')
       , ISNULL(s.SeasonId, '')
       , ISNULL(s.BulkMRHandle, '')
       , ISNULL(s.SampleMRHandle, '')
       , ISNULL(s.Refno, '')
       , ISNULL(s.CustPONo, '')
       , ISNULL(s.IsEM, 0)
       , ISNULL(s.Responsibility, '')
        );

	  --刪除ADIDASComplain_Detail重複資料
	  delete t
	  from Production.dbo.ADIDASComplain_Detail t
	  where not exists(	select 1 
	  				from Trade_To_Pms.dbo.ADIDASComplain_Detail s
	  				where t.UKEY = s.UKEY) and
			t.ID in (select ID from Trade_To_Pms.dbo.ADIDASComplain)
	------------------ADIDASComplainTarget--------------------------------

	Merge Production.dbo.ADIDASComplainTarget as t
	using Trade_To_Pms.dbo.ADIDASComplainTarget as s
	on t.year=s.year
	when matched then 
		update set
	   t.[Target] = ISNULL(s.[Target], 0)
      ,t.[AddName]= ISNULL(s.[AddName], '')
      ,t.[AddDate]= s.[AddDate]
      ,t.[EditName]= ISNULL(s.[EditName], '')
      ,t.[EditDate] = s.[EditDate]
	when not matched by Target then 
		insert ([Year]
      ,[Target]
      ,[AddName]
      ,[AddDate]
      ,[EditName]
      ,[EditDate])
	  values(
	    ISNULL(s.[Year], '')
      , ISNULL(s.[Target], 0)
      , ISNULL(s.[AddName], '')
      , s.[AddDate]
      , ISNULL(s.[EditName], '')
      , s.[EditDate]
	  )
	  when not matched by source then
	  delete;
	  
	 ------------------insert ADIDASKPITARGET---------------
    Merge Production.dbo.ADIDASKPITARGET as t
    using Trade_To_Pms.dbo.ADIDASKPITARGET as s
    on t.KPIItem = s.KPIItem
    when Matched then
        update set t.XlsColumn = ISNULL(s.XlsColumn, 0)
                 , t.Description = ISNULL(s.Description, '')
                 , t.Target = ISNULL(s.Target, 0)
                 , t.AddName = ISNULL(s.AddName, '')
                 , t.AddDate = s.AddDate
                 , t.EditName = ISNULL(s.EditName, '')
                 , t.EditDate = s.EditDate
    when not matched by Target then
        insert
        (
            KPIItem
          , XlsColumn
          , Description
          , Target
          , AddName
          , AddDate
          , EditName
          , EditDate
        )
        values
          (
            ISNULL(s.KPIItem, 0), 
            ISNULL(s.XlsColumn, 0), 
            ISNULL(s.Description, ''), 
            ISNULL(s.Target, 0), 
            ISNULL(s.AddName, ''), 
            s.AddDate, 
            ISNULL(s.EditName, ''), 
            s.EditDate
          )
    when not matched by source and t.KPIItem in (
                                                    select KPIItem from Trade_To_Pms.dbo.ADIDASKPITARGET WITH (NOLOCK)
                                                ) then
        delete;
END

