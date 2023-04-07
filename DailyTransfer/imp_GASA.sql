
Create PROCEDURE [dbo].[imp_GASA]

AS
BEGIN
	Merge Production.dbo.SentReport as t
	Using (select * from Trade_To_Pms.dbo.SentReport)as s
	on t.Export_DetailUkey = s.Export_DetailUkey when matched then update set 
		t.TPEInspectionReport =s.InspectionReport,
		t.TPETestReport =s.TestReport,
		t.TPEContinuityCard =s.ContinuityCard,
		t.AWBNo  = isnull(s.AWBNo,'') 
	when not matched by target then 	
		insert(  [Export_DetailUkey],  TPEInspectionReport,  TPETestReport, TPEContinuityCard,		   AWBNo )
		values(s.[Export_DetailUkey],s.[InspectionReport] ,s.[TestReport] ,s.[ContinuityCard],isnull(s.AWBNo,'') );


	Merge Production.dbo.FirstDyelot  as t
	Using (select * from Trade_To_Pms.dbo.FirstDyelot )as s
	on t.TestDocFactoryGroup = s.TestDocFactoryGroup 
	and t.Refno = s.Refno 
	and t.SuppID =s. SuppID 
	and t.ColorID =s.ColorID 
	and t.SeasonSCIID = s.SeasonSCIID
	when matched then update set 
		t.TPEFirstDyelot =s.FirstDyelot,
		t.Period  =s.Period ,
		t.BrandRefno¡@=s.BrandRefno,
		t.AWBno = s.AWBno,
		t.AddName = s.AddName,
		t.AddDate = s.AddDate,
		t.ReceivedDate = s.ReceivedDate,
		t.ReceivedRemark = s.ReceivedRemark,
		t.DocumentName = s.DocumentName,
		t.BrandID = s.BrandID
	when not matched by target then 	
		insert(TestDocFactoryGroup,  [Refno],  [SuppID],  [ColorID],  TPEFirstDyelot ,SeasonSCIID,    Period,BrandRefno,AWBno,AddName,AddDate,ReceivedDate,ReceivedRemark,DocumentName,BrandID )
		values(s.TestDocFactoryGroup,s.[Refno],s.[SuppID],s.[ColorID],s.[FirstDyelot],s.SeasonSCIID,s.Period ,s.BrandRefno,s.AWBno,s.AddName,s.AddDate,s.ReceivedDate,s.ReceivedRemark,s.DocumentName,s.BrandID);
END




