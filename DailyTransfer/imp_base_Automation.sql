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

	--�ǰeSubProcess
	exec SentSubprocessToAGV
END