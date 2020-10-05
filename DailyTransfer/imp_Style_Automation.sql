-- =============================================
-- Author:		<Aaron S02109>
-- Create date: <2020/04/20>
-- Description:	在imp_base後面執行，透過web api傳送有異動過的資料給廠商
-- =============================================
Create PROCEDURE [dbo].[imp_Style_Automation]
AS
BEGIN
	if not exists(select 1 from Production.dbo.System where Automation = 1 )
	begin
		return
	end

	Declare @Url varchar(100)
	--傳送SubProcess
	select @Url = [dbo].[GetWebApiURL]('3A0134', 'FinishingProcesses') 
	if(isnull(@Url, '') <> '')
	begin

		declare @inputStyleKey varchar(max)
		SELECT @inputStyleKey =  Stuff((select concat( ',',ID + '`' + SeasonID + '`' + BrandID)   from AutomationStyle FOR XML PATH('')),1,1,'') 

		if @inputStyleKey is null
		set @inputStyleKey = ''
		
		exec SentStyleFPSSettingToFinishingProcesses @inputStyleKey
	end
	
END