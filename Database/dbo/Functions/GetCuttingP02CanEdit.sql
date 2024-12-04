
CREATE FUNCTION [dbo].[GetCuttingP02CanEdit]
(
    @Ukey BIGINT,
    @CutPlanID VARCHAR(13),
    @CutRef VARCHAR(10)
)
RETURNS bit
AS
BEGIN
	-- Declare the return variable here
	DECLARE @canEdit bit = 1
    
    -- 若有找到, 設置為 0 = 不可編輯, 就不繼續往後找
    set @canEdit = CAST(IIF(@CutPlanID <> '', 0, 1) AS BIT)
    if @canEdit = 0 RETURN @canEdit
    
    set @canEdit = CAST(IIF(EXISTS(SELECT 1 FROM WorkOrderForOutput WITH (NOLOCK) WHERE WorkOrderForPlanningUkey = @Ukey), 0, 1) AS BIT)
    if @canEdit = 0 RETURN @canEdit

    set @canEdit = CAST(IIF(EXISTS(SELECT 1 FROM SpreadingSchedule_Detail WITH (NOLOCK) WHERE CutRef = @CutRef AND CutRef <> ''), 0, 1) AS BIT)
	RETURN @canEdit
END