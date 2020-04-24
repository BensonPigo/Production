CREATE PROCEDURE [dbo].[BatchReSentJsonToAGV]
AS
begin
	if not exists(select 1 from Production.dbo.System where Automation = 1 )
	begin
		return
	end

	DECLARE cur_APIThread CURSOR FOR 
     Select distinct APIThread from dbo.AutomationErrMsg where ReSented = 0 and JSON <> '';
	declare @APIThread   varchar(30)
	
	DECLARE cur_ResentUkey CURSOR FOR 
	     Select Ukey from dbo.AutomationErrMsg where APIThread = @APIThread and ReSented = 0 and JSON <> '' order by AddDate
	declare @Ukey   bigint
	
	Declare @isSuccess bit

	OPEN cur_APIThread --開始run cursor                   
	FETCH NEXT FROM cur_APIThread INTO @APIThread --將第一筆資料填入變數
	WHILE @@FETCH_STATUS = 0 --檢查是否有讀取到資料; WHILE用來處理迴圈，當為true時則進入迴圈執行
	BEGIN
		print @APIThread
		open cur_ResentUkey;
		fetch next from cur_ResentUkey into @Ukey
		while @@FETCH_STATUS = 0
		begin
			print @Ukey
			exec dbo.SentJsonToAGV @Ukey,'SCIMIS', @isSuccess OUTPUT

			if(@isSuccess = 0)
			begin
				break
			end
		fetch next from cur_ResentUkey into @Ukey
		end
		CLOSE cur_ResentUkey

	FETCH NEXT FROM cur_APIThread INTO @APIThread 
	END
	--關閉cursor與參數的關聯
	CLOSE cur_APIThread
	DEALLOCATE cur_APIThread 
	DEALLOCATE cur_ResentUkey 
end
