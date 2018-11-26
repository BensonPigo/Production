
Create PROCEDURE [dbo].[imp_GASA]

AS
BEGIN
	Merge Production.dbo.SentReport as t
	Using (select * from Trade_To_Pms.dbo.SentReport)as s
	on t.Export_DetailUkey = s.Export_DetailUkey when matched then update set 
		t.TPEInspectionReport =s.InspectionReport,
		t.TPETestReport =s.TestReport,
		t.TPEContinuityCard =s.ContinuityCard
	when not matched by target then 	
		insert(  [Export_DetailUkey],  TPEInspectionReport,  TPETestReport, TPEContinuityCard)
		values(s.[Export_DetailUkey],s.[InspectionReport] ,s.[TestReport] ,s.[ContinuityCard]);


	Merge Production.dbo.FirstDyelot  as t
	Using (select * from Trade_To_Pms.dbo.FirstDyelot )as s
	on t.Consignee = s.Consignee and t.Refno = s.Refno and t.SuppID =s. SuppID and t.ColorID =s.ColorID and t.SeasonSCIID = s.SeasonSCIID
	when matched then update set 
		t.TPEFirstDyelot =s.FirstDyelot,
		t.Period  =s.Period 
	when not matched by target then 	
		insert(Consignee,  [Refno],  [SuppID],  [ColorID],  TPEFirstDyelot ,SeasonSCIID,    Period )
		values(s.Consignee,s.[Refno],s.[SuppID],s.[ColorID],s.[FirstDyelot],s.SeasonSCIID,s.Period );
END




