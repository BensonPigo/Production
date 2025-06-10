CREATE FUNCTION [dbo].[GetNextInspectionSeq]
(
    @FIRID BIGINT,
    @Roll NVARCHAR(50),
    @Dyelot NVARCHAR(50)
)
RETURNS INT
AS
BEGIN
    DECLARE @InspSeq INT;

    -- Find the maximum InspSeq for the given parameters
    SELECT @InspSeq = MAX(InspSeq)
    FROM [dbo].[FIR_Physical_His]
    WHERE ID = @FIRID
      AND Roll = @Roll
      AND Dyelot = @Dyelot;

    -- If no records are found, set @InspSeq to 0
    IF @InspSeq IS NULL
        SET @InspSeq = 0;

    -- Return the next inspection sequence number
    RETURN @InspSeq + 1;
END;
GO
