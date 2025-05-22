
CREATE FUNCTION [dbo].[GetCuttingP02CanEdit]
(
    @Ukey BIGINT,
    @CutPlanID VARCHAR(13),
    @CutRef VARCHAR(10)
)
RETURNS bit
AS
BEGIN

    IF @CutRef IS NULL OR @CutRef = ''
        RETURN 1
		
	-- 若有找到, 設置為 0 = 不可編輯, 就不繼續往後找
	
    IF EXISTS (
        SELECT 1 
        FROM WorkOrderForOutput WITH (NOLOCK)
        WHERE WorkOrderForPlanningUkey = @Ukey
    )
        RETURN 0

    IF EXISTS (
        SELECT 1 
        FROM SpreadingSchedule_Detail WITH (NOLOCK)
        WHERE CutRef = @CutRef
    )
        RETURN 0

    RETURN 1
END