-- =============================================
-- Author:		<Aaron S02109>
-- Create date: <2020/04/20>
-- Description:	�bimp_base�᭱����A�z�Lweb api�ǰe�����ʹL����Ƶ��t��
-- =============================================
Create PROCEDURE [dbo].[imp_base_Automation]
AS
BEGIN
	if not exists(select 1 from Production.dbo.System where Automation = 1 )
	begin
		return
	end

	Declare @Url varchar(100)
	--�ǰeSubProcess
	select @Url = [dbo].[GetWebApiURL]('3A0197', 'AGV') 
	if(isnull(@Url, '') <> '')
	begin
		exec SentSubprocessToAGV
	end
	
END