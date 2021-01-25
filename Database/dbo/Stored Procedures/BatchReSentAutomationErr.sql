CREATE PROCEDURE [dbo].[BatchReSentAutomationErr]
AS
begin
	if not exists(select 1 from Production.dbo.System where Automation = 1 and AutomationAutoRunTime > 0)
	begin
		return
	end

	declare @RetryTimeFlag datetime = DATEADD( MINUTE, -11, GETDATE())

	select	Ukey
			,SuppID
			,ModuleName
			,APIThread
			,SuppAPIThread
			,JSON
			,AddName
			,AddDate
			,EditName
			,EditDate
	into #tmpRetryAutomation
	from AutomationCreateRecord with (nolock)
	where AddDate < @RetryTimeFlag

	insert into AutomationErrMsg(SuppID
								,ModuleName
								,APIThread
								,SuppAPIThread
								,ErrorCode
								,ErrorMsg
								,JSON
								,ReSented
								,AddName
								,AddDate
								)
					select	SuppID
							,ModuleName
							,APIThread
							,SuppAPIThread
							,'995'
							,'Created from AutomationCreateRecord'
							,JSON
							,0
							,AddName
							,AddDate
					from #tmpRetryAutomation

	delete AutomationCreateRecord 
	where Ukey in (select Ukey from #tmpRetryAutomation)

	DECLARE cur_APIThread CURSOR FOR 
     Select distinct APIThread	from dbo.AutomationErrMsg 
								where ReSented = 0 and 
									  JSON <> '' and
									  SuppAPIThread in ('api/SunriseFinishingProcesses/SentDataByApiTag', 'api/GensongFinishingProcesses/SentDataByApiTag');
	declare @APIThread   varchar(50)
	
	
	declare @Ukey   bigint
	
	Declare @isSuccess nvarchar(max)

	OPEN cur_APIThread --開始run cursor                   
	FETCH NEXT FROM cur_APIThread INTO @APIThread --將第一筆資料填入變數
	WHILE @@FETCH_STATUS = 0 --檢查是否有讀取到資料; WHILE用來處理迴圈，當為true時則進入迴圈執行
	BEGIN
		print @APIThread

		DECLARE cur_ResentUkey CURSOR FOR 
	     Select Ukey from dbo.AutomationErrMsg 
		 where	APIThread = @APIThread and 
				ReSented = 0 and 
				JSON <> '' and
				SuppAPIThread in ('api/SunriseFinishingProcesses/SentDataByApiTag', 'api/GensongFinishingProcesses/SentDataByApiTag')
		 order by AddDate

		open cur_ResentUkey;
		fetch next from cur_ResentUkey into @Ukey
		while @@FETCH_STATUS = 0
		begin
			print @Ukey
			exec dbo.SentJsonFromAutomationErrMsg @Ukey,'SCIMIS', @isSuccess OUTPUT, 1200

			if(isnull(@isSuccess, '') <> '')
			begin
				break
			end
		fetch next from cur_ResentUkey into @Ukey
		end
		CLOSE cur_ResentUkey
		DEALLOCATE cur_ResentUkey 

	FETCH NEXT FROM cur_APIThread INTO @APIThread 
	END
	--關閉cursor與參數的關聯
	CLOSE cur_APIThread
	DEALLOCATE cur_APIThread 
	
end
go