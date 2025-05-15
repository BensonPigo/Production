CREATE PROCEDURE [dbo].[GetNextSequenceCutRef]
    @CutRef VARCHAR(10),
    @NextValue VARCHAR(10) OUTPUT
AS
BEGIN
    SET NOCOUNT ON; 

    DECLARE @NewCutRef VARCHAR(10) = ''
    DECLARE @OriCutSeq INT = 0, @NewCutSeq INT
    SET @NextValue = ''

    -- Return empty if CutRef is null or empty
    IF ISNULL(@CutRef, '') = ''
    BEGIN
        RETURN
    END

    -- Extract NewCutRef
    IF CHARINDEX('-', @CutRef) > 0
    BEGIN
        SET @NewCutRef = SUBSTRING(@CutRef, 1, CHARINDEX('-', @CutRef) - 1)
    END
    ELSE
    BEGIN
        SET @NewCutRef = @CutRef
    END

    -- Get the current sequence number
    SET @OriCutSeq = ISNULL((SELECT Seq FROM CutRefIndex with(nolock) WHERE CutRef = @NewCutRef), 0)
    SET @NewCutSeq = @OriCutSeq + 1

    -- Update or insert the new sequence number
    IF @OriCutSeq > 0
    BEGIN
        UPDATE CutRefIndex
        SET Seq = @NewCutSeq
        WHERE CutRef = @NewCutRef
    END
    ELSE 
    BEGIN
        INSERT INTO CutRefIndex(CutRef, Seq)
        VALUES(@NewCutRef, @NewCutSeq)
    END

    -- Set the output value
    SET @NextValue = CONCAT(@NewCutRef, '-', @NewCutSeq)
END
GO



--DECLARE @NextValue VARCHAR(10);
--EXEC dbo.GetNextSequenceCutRef @CutRef = '0002D6', @NextValue = @NextValue OUTPUT;
--select @NextValue