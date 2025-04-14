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
	and t.LOT = s.LOT
	when matched then update set 
        t.FirstDyelot = s.FirstDyelot,
        t.Period = ISNULL(s.Period, 0),
        t.BrandRefno = ISNULL(s.BrandRefno, ''),
        t.AWBno = ISNULL(s.AWBno, ''),
        t.AddName = ISNULL(s.AddName, ''),
        t.AddDate = s.AddDate,
        t.ReceivedDate = s.ReceivedDate,
        t.ReceivedRemark = ISNULL(s.ReceivedRemark, '')
	when not matched by target then 	
		insert(TestDocFactoryGroup,  [SuppID],  [ColorID],[FirstDyelot],  SeasonID,   Period,BrandRefno,AWBno,AddName,AddDate,ReceivedDate,ReceivedRemark,DocumentName,BrandID,LOT)
        VALUES (
            ISNULL(s.TestDocFactoryGroup, ''),
            ISNULL(s.[SuppID], ''),
            ISNULL(s.[ColorID], ''),
            s.[FirstDyelot],
            ISNULL(s.SeasonID, ''),
            ISNULL(s.Period, 0),
            ISNULL(s.BrandRefno, ''),
            ISNULL(s.AWBno, ''),
            ISNULL(s.AddName, ''),
            s.AddDate,
            s.ReceivedDate,
            ISNULL(s.ReceivedRemark, ''),
            ISNULL(s.DocumentName, ''),
            ISNULL(s.BrandID, ''),
			ISNULL(s.LOT, '')
        );

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




