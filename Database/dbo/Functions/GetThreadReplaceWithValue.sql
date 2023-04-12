-- =============================================
-- Author:		Jeff
-- Create date: 2023/02/16
-- Description:	From trade 只取需要部份 for < WH P01 Material Compare >
-- =============================================
Create FUNCTION [dbo].[GetThreadReplaceWithValue] 
(
	@SuppID			varchar(6)
	,@Style_ThreadColorCombo_History_Detail_Ukey		bigint
	,@CfmDate		date
	,@Category		varchar(1)
)
RETURNS TABLE 
AS
RETURN 
(
	select nCol.SciRefno, nCol.ColorID, nCol.SuppColor, nCol.IsForOtherBrand, nCol.ToBrandColorID
	from Style_ThreadColorCombo_History_Detail std
	outer apply (
		select * from Production.dbo.GetThreadReplace(@SuppID, std.SCIRefNo, std.SuppColor, @CfmDate)
		where @Category in ('B', 'M')
	) gtr
	outer apply (select SciRefno = isnull(gtr.ToSCIRefno, std.SCIRefNo)
						,ColorID = std.ColorID --此處不替換Color還是保留原本的，因為要庫存使用
						,SuppColor = isnull(gtr.ToBrandSuppColor, std.SuppColor)
						,ToBrandColorID = isnull(gtr.ToBrandColorID, std.ColorID) --應該用不到
						,IsForOtherBrand = iif(gtr.ToSCIRefno is null, 0, 1)) nCol --1代表有替換 0代表沒有
	where std.Ukey = @Style_ThreadColorCombo_History_Detail_Ukey
)