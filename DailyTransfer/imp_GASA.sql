
Create PROCEDURE [dbo].[imp_GASA]

AS
BEGIN
	Merge Production.dbo.SentReport as t
	Using (select * from Trade_To_Pms.dbo.SentReport)as s
	on t.Export_DetailUkey = s.Export_DetailUkey when matched then update set 
		t.Export_DetailUkey=s. Export_DetailUkey,
		t.TEPInspectionReport =s. InspectionReport,
		t.TEPTestReport =s.TestReport,
		t.TEPContinuityCard =s.ContinuityCard
	when not matched by target then 	
		insert([Export_DetailUkey],TEPInspectionReport,TEPTestReport,TEPContinuityCard,[EditName],[EditDate])
		values(s.[Export_DetailUkey],s.[InspectionReport],s.[TestReport],s.[ContinuityCard],s.[EditName],s.[EditDate]);


	Merge Production.dbo.FirstDyelot  as t
	Using (select * from Trade_To_Pms.dbo.FirstDyelot )as s
	on t.SCIRefno = s.SCIRefno when matched then update set 
		t. SCIRefno =s. SCIRefno,
		t. SuppID =s. SuppID,
		t. ColorID =s. ColorID ,
		t.TPEFirstDyelot =s.FirstDyelot
	when not matched by target then 	
		insert([SCIRefno],[SuppID],[ColorID],TPEFirstDyelot,[EditName],[EditDate])
		values(s.[SCIRefno],s.[SuppID],s.[ColorID],s.[FirstDyelot],s.[EditName],s.[EditDate]);
END




