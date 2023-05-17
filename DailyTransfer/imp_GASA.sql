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
	when matched then update set 
		t.FirstDyelot =s.FirstDyelot,
		t.Period  =s.Period ,
		t.BrandRefno =s.BrandRefno,
		t.AWBno = s.AWBno,
		t.AddName = s.AddName,
		t.AddDate = s.AddDate,
		t.ReceivedDate = s.ReceivedDate,
		t.ReceivedRemark = s.ReceivedRemark,
		t.DocumentName = s.DocumentName,
		t.BrandID = s.BrandID
	when not matched by target then 	
		insert(TestDocFactoryGroup,  [SuppID],  [ColorID],[FirstDyelot],  SeasonID,   Period,BrandRefno,AWBno,AddName,AddDate,ReceivedDate,ReceivedRemark,DocumentName,BrandID)
		values(s.TestDocFactoryGroup,s.[SuppID],s.[ColorID],s.[FirstDyelot],s.SeasonID,s.Period ,s.BrandRefno,s.AWBno,s.AddName,s.AddDate,s.ReceivedDate,s.ReceivedRemark,s.DocumentName,s.BrandID);
END




