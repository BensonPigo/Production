Create PROCEDURE [dbo].[imp_GASA]
AS
BEGIN
SET NOCOUNT ON;
	
	Merge Production.dbo.FirstDyelot  as t
	Using (select * from Trade_To_Pms.dbo.FirstDyelot )as s
	on t.TestDocFactoryGroup = s.TestDocFactoryGroup 
	and t.BrandRefno = s.BrandRefno
	and t.SuppID =s. SuppID 
	and t.ColorID =s.ColorID 
	and t.SeasonID = s.SeasonID
	and t.DocumentName = s.DocumentName
	and t.BrandID = s.BrandID
	when matched then update set 
		t.FirstDyelot =s.FirstDyelot,
		t.Period  =s.Period ,
		t.BrandRefno =s.BrandRefno,
		t.AWBno = s.AWBno,
		t.AddName = s.AddName,
		t.AddDate = s.AddDate,
		t.ReceivedDate = s.ReceivedDate,
		t.ReceivedRemark = s.ReceivedRemark
	when not matched by target then 	
		insert(TestDocFactoryGroup,  [SuppID],  [ColorID],[FirstDyelot],  SeasonID,   Period,BrandRefno,AWBno,AddName,AddDate,ReceivedDate,ReceivedRemark,DocumentName,BrandID)
		values(s.TestDocFactoryGroup,s.[SuppID],s.[ColorID],s.[FirstDyelot],s.SeasonID,s.Period ,s.BrandRefno,s.AWBno,s.AddName,s.AddDate,s.ReceivedDate,s.ReceivedRemark,s.DocumentName,s.BrandID);

	--GASAClip import update to exclude the data created by pms
	Delete t
	from Production.dbo.GASAClip as t 
	left join Trade_To_Pms.dbo.GASAClip s on t.Pkey = s.Pkey	 and  t.UniqueKey = s.UniqueKey 
	where s.UniqueKey is null
	and exists(
		select 1 from Production.dbo.GASAClip s 
		where s.PKey = t.PKey and s.UniqueKey = t.UniqueKey
		and not exists(
			select 1 from Production.dbo.Pass1 p
			where p.ID = s.AddName
		)
	)

	update t
	 set	 t.[TableName]	=s.[TableName]
			,t.[SourceFile]	=s.[SourceFile]
			,t.[Description]=s.[Description]
			,t.[AddName]	=s.[AddName]
			,t.[AddDate]	=s.[AddDate]
	from Production.dbo.GASAClip t
	inner join Trade_To_Pms.dbo.GASAClip s ON t.Pkey = s.Pkey AND  t.UniqueKey = s.UniqueKey 
	where not exists(
		select 1 from Production.dbo.Pass1 p
		where p.ID = t.AddName
	)
	;

	INSERT INTO Production.dbo.GASAClip ([PKey],[TableName],[UniqueKey],[SourceFile],[Description],[AddName],[AddDate]	)
	SELECT [PKey],[TableName],[UniqueKey],[SourceFile],[Description],[AddName],[AddDate]	
	FROM Trade_To_Pms.dbo.GASAClip s
	WHERE NOT EXISTS ( 
		select 1 from Production.dbo.GASAClip t 
		WHERE t.Pkey = s.Pkey AND  t.UniqueKey = s.UniqueKey 
	)
	and not exists(
		select 1 from Production.dbo.Pass1 p
		where p.ID = s.AddName
	)
END




