-- =============================================
-- Author:		<Aaron S02109>
-- Create date: <2020/04/20>
-- Description:	在imp_base後面執行，透過web api傳送有異動過的資料給廠商
-- =============================================
Create PROCEDURE [dbo].[imp_base_Automation]
AS
BEGIN
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