Create PROCEDURE [dbo].[imp_smnotice_detail]

AS
BEGIN

	Merge Production.dbo.smnotice_detail as t
	Using (select * from Trade_To_Pms.dbo.smnotice_detail a WITH (NOLOCK))as s
	on t.id = s.id and t.type = s.type
	when matched then
		update set 
		   t.[UseFor]			=s.[UseFor]
		  ,t.[PhaseID]			=s.[PhaseID]
		  ,t.[RequireDate]		=s.[RequireDate]
		  ,t.[Apv2SampleTime]	=s.[Apv2SampleTime]
		  ,t.[Apv2SampleHandle]	=s.[Apv2SampleHandle]
		  ,t.[ApvName]			=s.[ApvName]
		  ,t.[ApvDate]			=s.[ApvDate]
		  ,t.[Factory]			=s.[Factory]
		  ,t.[IEConfirmMR]		=s.[IEConfirmMR]
		  ,t.[PendingStatus]	=s.[PendingStatus]
		  ,t.[BasicPattern]		=s.[BasicPattern]
		  ,t.[Remark1]			=s.[Remark1]
		  ,t.[Remark2]			=s.[Remark2]
		  ,t.[AddName]			=s.[AddName]
		  ,t.[AddDate]			=s.[AddDate]
		  ,t.[EditName]			=s.[EditName]
		  ,t.[EditDate]			=s.[EditDate]
	when not matched by target then 	
	insert([ID],[Type],[UseFor],[PhaseID],[RequireDate],[Apv2SampleTime],[Apv2SampleHandle],[ApvName],[ApvDate],[Factory]
	,[IEConfirmMR],[PendingStatus],[BasicPattern],[Remark1],[Remark2],[AddName],[AddDate],[EditName],[EditDate])
	values(s.[ID],s.[Type],s.[UseFor],s.[PhaseID],s.[RequireDate],s.[Apv2SampleTime],s.[Apv2SampleHandle],s.[ApvName],s.[ApvDate]
	,s.[Factory],s.[IEConfirmMR],s.[PendingStatus],s.[BasicPattern],s.[Remark1],s.[Remark2],s.[AddName],s.[AddDate],s.[EditName],s.[EditDate]);
	
END