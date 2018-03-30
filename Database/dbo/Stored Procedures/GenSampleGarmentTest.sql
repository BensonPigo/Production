-- =============================================
-- Author:		<Aaron>
-- Create date: <2018/3/22>
-- Description:	<Sample Garment Test產生>
-- =============================================
CREATE PROCEDURE [dbo].[GenSampleGarmentTest]
AS
BEGIN
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	--避免跳出:Null value is eliminated by an aggregate or other SET operation
	SET ANSI_WARNINGS OFF;

	Select distinct o.StyleID,o.BrandID,o.SeasonID,oa.Article
	into #tmpSampleGarmentTest
    From Orders o WITH (NOLOCK) 
    Inner join Order_Article oa WITH (NOLOCK) on o.ID = oa.ID
    Where (o.SciDelivery > DATEADD(DAY,-60,CONVERT(date,GETDATE())) or o. BuyerDelivery> DATEADD(DAY,-60,CONVERT(date,GETDATE()))) and
		   o.Junk = 0 and o.IsForecast = 0 and o.Category = 'S';

	--Insert 新資料進入 SampleGarmentTest
	Insert into SampleGarmentTest(BrandID,StyleID,SeasonID,Article)
	select BrandID,StyleID,SeasonID,Article from #tmpSampleGarmentTest t 
	where not exists(select 1 from SampleGarmentTest s where s.StyleID = t.StyleID and s.BrandID = t.BrandID and s.SeasonID = t.SeasonID and s.Article = t.Article);

	--刪除 SampleGarmentTest 不存在#tmpSampleGarmentTest並且沒有detail的檢測資料
	Delete s
	from SampleGarmentTest s where
	not exists (select 1 from #tmpSampleGarmentTest t where s.StyleID = t.StyleID and s.BrandID = t.BrandID and s.SeasonID = t.SeasonID and s.Article = t.Article) and
	not exists (select 1 from SampleGarmentTest_Detail sd where s.ID = sd.ID);

END
