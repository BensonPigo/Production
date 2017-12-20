-- =============================================
-- Author:		<JEFF S01952>
-- Create date: <2016/11/11>
-- Description:	<import BundleTransfer>
-- =============================================
CREATE PROCEDURE [dbo].[imp_BundleTransfer]
@RFIDServerName varchar(50)='', @RFIDDatabaseName varchar(20)='', @RFIDloginId varchar(20)='', @RFIDLoginPwd varchar(20)='', @RFIDTable varchar(20)=''
AS
BEGIN
--若外部沒傳參數進來,則用system上的欄位資訊
	IF @RFIDServerName = ''
	BEGIN
		select 
			@RFIDServerName = RFIDServerName,
			@RFIDDatabaseName = RFIDDatabaseName,
			@RFIDloginId = RFIDloginId,
			@RFIDLoginPwd = RFIDLoginPwd,
			@RFIDTable = RFIDTable
		from system
	END

	--若不存在則新增連線
	BEGIN
		IF NOT EXISTS (SELECT * FROM sys.servers WHERE name = @RFIDServerName)
		BEGIN
			--新建連線
			EXEC master.dbo.sp_addlinkedserver @server = @RFIDServerName, @srvproduct=N'SQL Server'
			--設定連線登入資訊
			EXEC master.dbo.sp_addlinkedsrvlogin @rmtsrvname = @RFIDServerName, @locallogin = NULL , @useself = N'False', @rmtuser = @RFIDloginId, @rmtpassword = @RFIDLoginPwd
		END
	END

	--Datas處理
	declare @Cmd varchar(max)
	declare @RaisError varchar(max)

	Set @Cmd = N'
	Begin Try
		Begin tran

		select  *  into #tmp from ['+@RFIDServerName+'].'+@RFIDDatabaseName+'.dbo.'+@RFIDTable+'

		declare @MaxSid bigint
		select @MaxSid = max(Sid) from  #tmp

			--add BundleTransfer	
			INSERT INTO BundleTransfer
			SELECT RB.sid, RB.readerid, RR.type, RR.ProcessId, RB.tagid, RB.epcid, RB.transdate, GETDATE()
			FROM  #tmp  RB
			left join RFIDReader RR on RB.readerid=RR.Id
						
			--add update BundleInOut			
			-- RFIDReader.Type=1
			Merge Production.dbo.BundleInOut as t
			Using (select a.EpcId as BundleNo,b.processId as SubProcessId, TransDate = max(a.TransDate),GetDate() as AddDate, b.Type
			FROM #tmp a inner join  RFIDReader b on a.ReaderId = b.Id
			group by a.EpcId,b.processId) as s
			on t.BundleNo = s.BundleNo and t.SubprocessId = s.SubProcessId and s.type=1
			when matched then 
				update set
				t.incoming = s.TransDate,	t.EditDate = s.AddDate
			when not matched by target and s.type=1 then
				insert(BundleNo, SubProcessId, InComing, AddDate)
				values(s.BundleNo, s.SubProcessId, s.TransDate,s.AddDate);

			-- RFIDReader.Type=2
			Merge Production.dbo.BundleInOut as t
			Using (select a.EpcId as BundleNo,b.processId as SubProcessId, TransDate = max(a.TransDate),GetDate() as AddDate, b.Type
			FROM #tmp a inner join  RFIDReader b on a.ReaderId = b.Id
			group by a.EpcId,b.processId) as s
			on t.BundleNo = s.BundleNo and t.SubprocessId = s.SubProcessId and s.type=2 
			when matched then 
				update set
				t.OutGoing = s.TransDate,	t.EditDate = s.AddDat
			when not matched by target and s.type=2 then
				insert(BundleNo, SubProcessId, OutGoing, AddDate)
				values(s.BundleNo, s.SubProcessId, s.TransDate, s.AddDate);

			Commit tran

			IF exists(select 1 from System where RFIDMiddlewareInRFIDServer=0)
				Begin
					DELETE '+@RFIDDatabaseName+'.dbo.'+@RFIDTable+'					
				End
			Else
				Begin	
					DELETE ['+@RFIDServerName+'].'+@RFIDDatabaseName+'.dbo.'+@RFIDTable+'	
					where sid<=@MaxSid
				End

	End Try
	Begin Catch
		RollBack Tran
	End Catch	'

	EXEC(@Cmd)

END
------------------------------------------------------------------------------------------------------------------------