
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
       VALUES
       (
              isnull(s.[Export_DetailUkey],0),
              s.[InspectionReport] ,
              s.[TestReport] ,
              s.[ContinuityCard],
              isnull(s.awbno,'')
       );


	Merge Production.dbo.FirstDyelot  as t
	Using (select * from Trade_To_Pms.dbo.FirstDyelot )as s
	on t.TestDocFactoryGroup = s.TestDocFactoryGroup and t.Refno = s.Refno and t.SuppID =s. SuppID and t.ColorID =s.ColorID and t.SeasonSCIID = s.SeasonSCIID
	when matched then update set 
		t.TPEFirstDyelot =s.FirstDyelot,
		t.Period  =s.Period 
	when not matched by target then 	
		insert(TestDocFactoryGroup,  [Refno],  [SuppID],  [ColorID],  TPEFirstDyelot ,SeasonSCIID,    Period )
       VALUES
       (
              isnull(s.testdocfactorygroup, ''),
              isnull(s.[Refno],             ''),
              isnull(s.[SuppID],            ''),
              isnull(s.[ColorID],           ''),
              s.[FirstDyelot],
              isnull(s.seasonsciid,         ''),
              isnull(s.period,              0)
       );
END




