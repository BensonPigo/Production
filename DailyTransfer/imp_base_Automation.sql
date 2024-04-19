-- =============================================
-- Author:		<Aaron S02109>
-- Create date: <2020/04/20>
-- Description:	在imp_base後面執行，透過web api傳送有異動過的資料給廠商
-- =============================================
Create PROCEDURE [dbo].[imp_base_Automation]
AS
BEGIN
	---- Check TransferDate before everything
	IF NOT  EXISTS(
		select 1 from Trade_To_PMS..DateInfo 
		where Name = 'TransferDate'
		AND DateStart in (CAST(DATEADD(DAY,-1,GETDATE()) AS date), CAST(GETDATE() AS DATE))
	)
	BEGIN
		-- 拋出錯誤
		RAISERROR ('The DB transferdate is wrong. Trade_To_PMS..DateInfo  中不存在符合條件的 TransferDate 記錄。', 16, 1); -- 16是錯誤的嚴重程度，1是錯誤狀態	
		RETURN; 
	END

	if not exists(select 1 from Production.dbo.System where Automation = 1 )
	begin
		return
	end

	Declare @Url varchar(100)
	--傳送SubProcess
	select @Url = [dbo].[GetWebApiURL]('3A0197', 'AGV') 
	if(isnull(@Url, '') <> '')
	begin
		exec SentSubprocessToAGV
	end
	
END