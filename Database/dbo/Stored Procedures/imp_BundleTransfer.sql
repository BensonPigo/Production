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
			,[LocationID]=isnull (RB.LocationID, ' + '''''' + ')
			FROM  #tmp  RB
			left join RFIDReader RR on RB.readerid  collate Chinese_Taiwan_Stroke_CI_AS  =RR.Id
			
			-- 先確認有哪些捆包號 + 工段 + Type，再到紀錄中找最新的資料
			select *
			into #disTmp
			from (
				select distinct
						tmp.EpcId
						, rd.processId
						, rd.Type
				from #tmp tmp				
				inner join RFIDReader rd on tmp.ReaderId collate Chinese_Taiwan_Stroke_CI_AS = rd.Id
			) disBundle
			outer apply (
				select top 1
						BundleNo = tmp.EpcId
						, SubProcessId = rd.processId
						, TransDate = tmp.TransDate
						, AddDate = GetDate()
						, rd.SewingLineID
						, [LocationID] = isnull (tmp.LocationID, ' + '''''' + ')
				from #tmp tmp
				inner join RFIDReader rd on tmp.ReaderId collate Chinese_Taiwan_Stroke_CI_AS = rd.Id
				where 	disBundle.EpcId = tmp.EpcId
						and disBundle.processId = rd.processId
						and disBundle.Type = rd.Type
				order by tmp.TransDate Desc
			) getLastTrans

			--add update BundleInOut			
			-- RFIDReader.Type=1
			Merge Production.dbo.BundleInOut as t
			Using (
				select *
				from #disTmp
			) as s
			on t.BundleNo = s.BundleNo collate Chinese_Taiwan_Stroke_CI_AS 
				and t.SubprocessId = s.SubProcessId collate Chinese_Taiwan_Stroke_CI_AS
				and s.type=1
			when matched then 
				update set
				t.incoming = s.TransDate, t.EditDate = s.AddDate, t.SewingLineID=s.SewingLineID, t.LocationID=s.LocationID
			when not matched by target and s.type=1 then
				insert(BundleNo, SubProcessId, InComing, AddDate, SewingLineID, LocationID)
				values(s.BundleNo, s.SubProcessId, s.TransDate,s.AddDate, s.SewingLineID, s.LocationID);

			-- RFIDReader.Type=2
			Merge Production.dbo.BundleInOut as t
			Using (
				select *
				from #disTmp
			) as s
			on t.BundleNo = s.BundleNo collate Chinese_Taiwan_Stroke_CI_AS 
				and t.SubprocessId = s.SubProcessId collate Chinese_Taiwan_Stroke_CI_AS 
				and s.type=2 
			when matched then 
				update set
				t.OutGoing = s.TransDate, t.EditDate = s.AddDate, t.SewingLineID=s.SewingLineID, t.LocationID=s.LocationID
			when not matched by target and s.type=2 then
				insert(BundleNo, SubProcessId, OutGoing, AddDate, SewingLineID, LocationID)
				values(s.BundleNo, s.SubProcessId, s.TransDate, s.AddDate, s.SewingLineID, s.LocationID);

			-- RFIDReader.Type=3
			Merge Production.dbo.BundleInOut as t
			Using (
				select *
				from #disTmp
			) as s
			on t.BundleNo = s.BundleNo collate Chinese_Taiwan_Stroke_CI_AS 
				and t.SubprocessId = s.SubProcessId collate Chinese_Taiwan_Stroke_CI_AS 
				and s.type=3
			when matched then 
				update set
				t.OutGoing = s.TransDate, t.EditDate = s.AddDate, t.SewingLineID=s.SewingLineID, t.LocationID=s.LocationID
			when not matched by target and s.type=3 then
				insert(BundleNo, SubProcessId, InComing, AddDate, SewingLineID, LocationID)
				values(s.BundleNo, s.SubProcessId, s.TransDate, s.AddDate, s.SewingLineID, s.LocationID);


			Commit tran

			IF exists(select 1 from System where RFIDMiddlewareInRFIDServer=0)
				Begin
					DELETE '+@RFIDDatabaseName+'.dbo.'+@RFIDTable+'					
				End
			Else
				Begin	
					DELETE ['+@RFIDServerName+'].'+@RFIDDatabaseName+'.dbo.'+@RFIDTable+'	
					where sid <=@MaxSid
				End

	End Try
	Begin Catch
		SELECT ERROR_NUMBER(), ERROR_MESSAGE();
		RollBack Tran
	End Catch	'

	EXEC(@Cmd)

END
------------------------------------------------------------------------------------------------------------------------