
/****** 更新時 連同MES.Production 一起更新******/
CREATE FUNCTION [dbo].[GetCheckContinusProduceDays]
(
	@StyleUkey BIGINT,
	@SewingLineID VARCHAR(3),
	@FactoryID VARCHAR(5),
	@Team VARCHAR(3) =  null,
	@SewingDate DATE
)
RETURNS INT
AS
BEGIN
	DECLARE @Day INT;

	WITH tmpStyle AS 
	(
		SELECT ID, BrandID
		FROM Style WITH (NOLOCK)
		WHERE Ukey = @StyleUkey
	), tmpSewingOutputDay AS
	(
		SELECT DISTINCT TOP 30 so.OutputDate
		FROM SewingOutput so WITH (NOLOCK)
		WHERE so.SewingLineID = @SewingLineID
			AND so.FactoryID = @FactoryID
			AND　(@Team = null or so.Team = @Team)
			AND so.OutputDate < @SewingDate
			AND so.Shift <> 'O'
			AND so.Category = 'O'
	), tmpSewingOutputID AS
	(
		SELECT so.ID, so.OutputDate
		FROM SewingOutput so WITH (NOLOCK)
		WHERE so.SewingLineID = @SewingLineID
			AND so.FactoryID = @FactoryID
			AND　(@Team = null or so.Team = @Team)
			AND EXISTS (SELECT 1 FROM tmpSewingOutputDay t WHERE so.OutputDate = t.OutputDate)
			AND so.Shift <> 'O'
			AND so.Category = 'O'
	), tmpSewingOutputStyle AS
	(
		SELECT DISTINCT so.OutputDate, o.StyleID, o.BrandID
		FROM tmpSewingOutputID so
		INNER JOIN SewingOutput_Detail sod WITH (NOLOCK) ON sod.ID = so.ID
		INNER JOIN Orders o WITH (NOLOCK) ON o.ID = sod.OrderID
	), tmpSewingSimlarStyle AS
	(
		SELECT * 
		FROM 
		(
			SELECT OutputDate, StyleID, BrandID FROM tmpSewingOutputStyle
			UNION
			SELECT tso.OutputDate, ss.ChildrenStyleID AS StyleID, ss.ChildrenBrandID AS BrandID
			FROM tmpSewingOutputStyle tso
			INNER JOIN Style_SimilarStyle ss WITH (NOLOCK) ON ss.MasterBrandID = tso.BrandID
															AND ss.MasterStyleID = tso.StyleID
		) a
	), interruptDate AS
	(
		SELECT MAX(OutputDate) AS interruptDate
		FROM tmpSewingOutputDay t
		WHERE NOT EXISTS
		(
			SELECT DISTINCT OutputDate
			FROM tmpSewingSimlarStyle tss
			WHERE EXISTS (SELECT 1 FROM tmpStyle s WHERE s.ID = tss.StyleID AND s.BrandID = tss.BrandID)
			AND t.OutputDate = tss.OutputDate
		)
	), tmpEnd AS
	(
		SELECT 
			CASE 
				WHEN (SELECT interruptDate FROM interruptDate) IS NULL 
					AND EXISTS 
					(
						SELECT 1
						FROM tmpSewingSimlarStyle tss
						WHERE EXISTS 
						(
							SELECT 1 
							FROM tmpStyle s 
							WHERE s.ID = tss.StyleID AND s.BrandID = tss.BrandID
						)
					)
				THEN (SELECT COUNT(1) + 1 FROM tmpSewingOutputDay)
				WHEN (SELECT interruptDate FROM interruptDate) IS NULL THEN 1
				ELSE 
					(SELECT COUNT(1) + 1 
					FROM tmpSewingOutputDay t 
					WHERE EXISTS (SELECT 1 FROM interruptDate i WHERE t.OutputDate > i.interruptDate))
			END AS ContinusDays
	)

	SELECT @Day = ContinusDays FROM tmpEnd;

	RETURN @Day;
END
