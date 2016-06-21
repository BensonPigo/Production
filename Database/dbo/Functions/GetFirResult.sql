﻿-- =============================================
-- Author:		Mike
-- Create date: 2016/06/01
-- Description:	判斷全部檢驗的狀態，得出最終結果 (pass or faild)
-- =============================================
CREATE FUNCTION [dbo].[GetFirResult]
(
	-- Add the parameters for the function here
	@FirID bigint
)
RETURNS varchar(15)
AS
BEGIN
	DECLARE @Physical AS VARCHAR(5);
	DECLARE @Weight AS VARCHAR(5);
	DECLARE @Continuity AS VARCHAR(5);
	DECLARE @ShadeBond AS VARCHAR(5);

	IF EXISTS(SELECT * FROM DBO.FIR F 
				WHERE F.ID = @FirID AND (F.Nonphysical = 1 AND F.nonWeight = 1 AND F.nonContinuity = 1 AND F.nonShadebond = 1)
									AND (F.PhysicalEncode = 0 AND F.WeightEncode = 0 AND F.ContinuityEncode = 0 AND F.ShadebondEncode = 0))
		RETURN 'Pass';

	IF EXISTS(SELECT * FROM DBO.FIR F 
				WHERE F.ID = @FirID AND ((F.PhysicalEncode = 1 OR F.Nonphysical = 1 )
									AND (F.WeightEncode = 1 OR F.nonWeight = 1 )
									AND (F.ContinuityEncode = 1 OR F.nonContinuity = 1 )
									AND (F.ShadebondEncode = 1 OR F.nonShadebond = 1)))
	BEGIN
	
		SELECT @Physical = F.Physical
				,@Weight = F.Weight
				,@Continuity = F.Continuity
				,@ShadeBond = F.ShadeBond FROM DBO.FIR F WHERE F.ID = @FirID;

				IF @Physical = 'Fail'	return	'Fail';
				IF @Weight = 'Fail'	return	'Fail';
				IF @Continuity = 'Fail' return	'Fail';
				IF @ShadeBond = 'Fail'return	'Fail';

				return 'Pass';
	
	END
	
	RETURN '';

END