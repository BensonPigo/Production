-- =============================================
-- Author:		<Aaron S02109>
-- Create date: <2020/04/15>
-- Description:	�bimp_Order�᭱����A�z�Lweb api�ǰe�����ʹL����Ƶ��t��
-- =============================================
Create PROCEDURE [dbo].[imp_Order_Automation]
AS
BEGIN
	if not exists(select 1 from Production.dbo.System where Automation = 1 )
	begin
		return
	end

	--�ǰeOrder_Qty
	exec SentOrderQtyToAGV
END