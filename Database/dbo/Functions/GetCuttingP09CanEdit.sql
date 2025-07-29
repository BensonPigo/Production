CREATE FUNCTION GetCuttingP09CanEdit
(
    @CutRef varchar(10)
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
        WHERE CutRef = @CutRef AND SpreadingStatus != 'Ready'
    )
        RETURN 0

    IF EXISTS (
        SELECT 1 
        FROM Bundle WITH (NOLOCK)
        WHERE CutRef = @CutRef
    )
        RETURN 0

    IF EXISTS (
        SELECT 1 
        FROM CuttingOutput_Detail WITH (NOLOCK)
        WHERE CutRef = @CutRef
    )
        RETURN 0

    IF EXISTS (
        SELECT 1 
        FROM MarkerReq_Detail_CutRef WITH (NOLOCK)
        WHERE CutRef = @CutRef
    )
        RETURN 0

    RETURN 1
END
GO