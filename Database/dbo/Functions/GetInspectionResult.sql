
-- =============================================
-- Author:		Mike
-- Create date: 2016/07/28
-- Description:	取得FIR或AIR的所有檢驗結果，以逗點分隔顯示。
-- =============================================
CREATE FUNCTION [dbo].[GetInspectionResult]
(
	-- Add the parameters for the function here
	@poid varchar(13),
	@seq1 varchar(3),
	@seq2 varchar(2)
)
RETURNS varchar(200)
AS
BEGIN
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
	DECLARE @result AS VARCHAR(200);

	set @result ='';

	select @result = (select x.Result+',' 
	from (Select CASE 
        when a.Nonphysical = 1 and a.nonContinuity=1 and nonShadebond=1 and a.nonWeight=1 then 'N/A'
        else a.result
        END as [Result] 
        from dbo.FIR a 
        where a.POID = @poid 
		and a.SEQ1 = @seq1 
		and a.SEQ2 = @seq2
        and (a.ContinuityEncode = 1 or a.PhysicalEncode = 1 or a.ShadebondEncode =1 or a.WeightEncode = 1 
				or (a.Nonphysical = 1 and a.nonContinuity=1 and nonShadebond=1 and a.nonWeight=1)
			)
        UNION ALL
        Select a.result
        from dbo.AIR a 
        where a.POID = @poid 
		and a.SEQ1 = @seq1 
		and a.SEQ2 = @seq2
        and a.Result !=''
        ) x for xml path(''))


	return @result;

END