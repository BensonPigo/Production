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
			Merge Production.dbo.BundleTransfer as t
			Using (select a.Sid,a.ReaderId,b.Type,b.processId as SubProcessId,a.TagId,a.EpcId as BundleNo, a.TransDate,GetDate() as AddDate
				   FROM '+@RFIDServerName+'.'+@RFIDDatabaseName+'.dbo.'+@RFIDTable+' a left join  RFIDReader b on a.ReaderId = b.Id) as s
			on t.Sid = s.Sid
			when matched and (t.AddDate <> s.AddDate) then
				update set
				t.Sid  =s.SId,
				t.RFIDReaderId = s.ReaderId,
				t.SubprocessId = s.SubProcessId,
				t.TagId = s.TagId,
				t.BundleNo = s.BundleNo,
				t.TransferDate = s.TransDate,
				t.AddDate = s.AddDate
			when not matched by target then  
				insert(Sid,RFIDReaderId,SubprocessId,TagId,BundleNo,TransferDate,AddDate)
				values(s.Sid,s.ReaderId,s.SubProcessId,s.TagId,s.BundleNo,s.TransDate,s.AddDate);
			
			Merge Production.dbo.BundleInOut as t
			Using (select a.Sid,a.ReaderId,b.Type,b.processId as SubProcessId,a.TagId,a.EpcId as BundleNo, a.TransDate,GetDate() as AddDate
				   FROM '+@RFIDServerName+'.'+@RFIDDatabaseName+'.dbo.'+@RFIDTable+' a left join  RFIDReader b on a.ReaderId = b.Id) as s
			on t.BundleNo = s.BundleNo and t.SubprocessId = s.SubProcessId
			when matched and (t.AddDate <> s.AddDate) then
				update set
				t.BundleNo = s.BundleNo,
				t.SubProcessId = s.SubProcessId,
				t.InComing = s.TransDate,
				t.AddDate = s.AddDate
			when not matched by target then
				insert(OutGoing,EditDate)
				values(s.TransDate,s.AddDate);

			Delete Middleware.dbo.Rfid_Bundle

			Commit tran
	End Try
	Begin Catch
		RollBack Tran
	End Catch
	'
	EXEC(@Cmd)
END
------------------------------------------------------------------------------------------------------------------------