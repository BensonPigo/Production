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
        t.FirstDyelot = s.FirstDyelot,
        t.Period = ISNULL(s.Period, 0),
        t.BrandRefno = ISNULL(s.BrandRefno, ''),
        t.AWBno = ISNULL(s.AWBno, ''),
        t.AddName = ISNULL(s.AddName, ''),
        t.AddDate = s.AddDate,
        t.ReceivedDate = s.ReceivedDate,
        t.ReceivedRemark = ISNULL(s.ReceivedRemark, '')
	when not matched by target then 	
		insert(TestDocFactoryGroup,  [SuppID],  [ColorID],[FirstDyelot],  SeasonID,   Period,BrandRefno,AWBno,AddName,AddDate,ReceivedDate,ReceivedRemark,DocumentName,BrandID)
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
            ISNULL(s.BrandID, '')
        );

END




