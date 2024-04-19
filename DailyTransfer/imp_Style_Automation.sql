-- =============================================
-- Author:		<Aaron S02109>
-- Create date: <2020/04/20>
-- Description:	�bimp_base�᭱����A�z�Lweb api�ǰe�����ʹL����Ƶ��t��
-- =============================================
Create PROCEDURE [dbo].[imp_Style_Automation]
AS
BEGIN

	---- Check TransferDate before everything
	IF NOT  EXISTS(
		select 1 from Trade_To_PMS..DateInfo 
		where Name = 'TransferDate'
		AND DateStart in (CAST(DATEADD(DAY,-1,GETDATE()) AS date), CAST(GETDATE() AS DATE))
	)
	BEGIN
		-- �ߥX���~
		RAISERROR ('The DB transferdate is wrong. Trade_To_PMS..DateInfo  �����s�b�ŦX���� TransferDate �O���C', 16, 1); -- 16�O���~���Y���{�סA1�O���~���A	
		RETURN; 
	END
	
	if not exists(select 1 from Production.dbo.System where Automation = 1 )
	begin
		return
	end

	Declare @Url varchar(100)
	--�ǰeSubProcess
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