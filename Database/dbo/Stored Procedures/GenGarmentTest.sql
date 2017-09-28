
-- =============================================
-- Author:		<Alger Song>
-- Create date: <2016/08/26>
-- Description:	<Garment Test產生>
-- =============================================
CREATE PROCEDURE [dbo].[GenGarmentTest]

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	--避免跳出:Null value is eliminated by an aggregate or other SET operation
	SET ANSI_WARNINGS OFF

    --先撈出應該出現的資料
	select distinct o.StyleID,o.SeasonID,o.BrandID,oq.Article,o.MDivisionID into #tmpData
	from Orders o WITH (NOLOCK)
	inner join Order_Qty oq WITH (NOLOCK) on o.ID = oq.ID
	where o.Category = 'B' 
	and o.SciDelivery >= DATEADD(DAY,-30,CONVERT(date,GETDATE()))
	and o.Junk = 0
	and o.IsForecast = 0
	
	--撈出不存在GarmentTest的資料
	select *
	into #tmpLackData
	from (
	select StyleID,SeasonID,BrandID,Article,MDivisionID from #tmpData
	except
	select StyleID,SeasonID,BrandID,Article,MDivisionID from GarmentTest WITH (NOLOCK)) t

	--撈出需要更新GarmentTest的資料
	select * into #tmpUpdateData from (
	select StyleID,SeasonID,BrandID,Article,MDivisionID from #tmpData
	except
	select StyleID,SeasonID,BrandID,Article,MDivisionID from #tmpLackData) T

	--將資料新增至GarmentTest
	Insert into GarmentTest (MDivisionid,BrandID,StyleID,SeasonID,Article,OrderID,DeadLine,SewingInline,SewingOffline)
	select t.MDivisionID,t.BrandID,t.StyleID,t.SeasonID,t.Article, 
	isnull((select top(1) a.OrderID from (
	select sd.OrderID,s.OutputDate from SewingOutput_Detail sd WITH (NOLOCK)
	left join SewingOutput s WITH (NOLOCK) on sd.ID = s.ID
	where sd.OrderId in (select o.ID from Orders o WITH (NOLOCK)
						 where o.BrandID = t.BrandID 
						 and o.StyleID = t.StyleID 
						 and o.SeasonID = t.SeasonID 
						 and o.MDivisionID = t.MDivisionID
						 and o.Category = 'B')
	and sd.Article = t.Article
	and s.Status <> 'New') a
	where a.OutputDate is not null
	order by a.OutputDate),'') OrderID , 
	(select min(o.SCIDelivery) from Orders o WITH (NOLOCK)
	where o.BrandID = t.BrandID 
	and o.StyleID = t.StyleID 
	and o.SeasonID = t.SeasonID 
	and o.MDivisionID = t.MDivisionID
	and o.Category = 'B') DeadLine ,
	(select min(o.SewInLine) from Orders o WITH (NOLOCK)
	where o.BrandID = t.BrandID 
	and o.StyleID = t.StyleID 
	and o.SeasonID = t.SeasonID 
	and o.MDivisionID = t.MDivisionID
	and o.Category = 'B') SewingInline ,
	(select min(o.SewOffLine) from Orders o WITH (NOLOCK)
	where o.BrandID = t.BrandID 
	and o.StyleID = t.StyleID 
	and o.SeasonID = t.SeasonID 
	and o.MDivisionID = t.MDivisionID
	and o.Category = 'B') SewingOffline  from #tmpLackData t

	--更新GarmentTest資料
	declare @StyleID VARCHAR(20)
	declare @SeasonID VARCHAR(20)
	declare @BrandID VARCHAR(20)
	declare @Article VARCHAR(20)
	declare @MDivisionID VARCHAR(20)
	DECLARE @OrderID nvarchar(20)
	DECLARE @DeadLine datetime
	DECLARE @SewingInline datetime
	DECLARE @SewingOffline datetime

	DECLARE CursortmpUpdateData CURSOR FOR    --建立Cursor
	SELECT StyleID,SeasonID,BrandID,Article,MDivisionID FROM #tmpUpdateData    --取得要塞入定義參數的資料

	OPEN CursortmpUpdateData    --開啟Cursor
	FETCH NEXT FROM CursortmpUpdateData INTO @StyleID, @SeasonID, @BrandID, @Article, @MDivisionID    --下移Cursor塞入參數資料

	WHILE @@FETCH_STATUS = 0    --判斷是否成功取得資料
　　	  BEGIN

			--OrderID
			SELECT @OrderID = isnull((select top(1) a.OrderID from (
			select sd.OrderID,s.OutputDate from SewingOutput_Detail sd WITH (NOLOCK)
			left join SewingOutput s WITH (NOLOCK) on sd.ID = s.ID
			where sd.OrderId in (select o.ID from Orders o WITH (NOLOCK)
			where o.BrandID = @BrandID 
			and o.StyleID = @StyleID 
			and o.SeasonID = @SeasonID 
			and o.MDivisionID = @MDivisionID
			and o.Category = 'B')
			and sd.Article = @Article
			and s.Status <> 'New') a
			where a.OutputDate is not null
			order by a.OutputDate),'') 

			--DeadLine
			select @DeadLine = min(o.SCIDelivery) from Orders o WITH (NOLOCK)
			where o.BrandID = @BrandID  
			and o.StyleID = @StyleID
			and o.SeasonID = @SeasonID 
			and o.MDivisionID = @MDivisionID
			and o.Category = 'B'

			--SewingInline
			select @SewingInline = min(o.SewInLine) from Orders o WITH (NOLOCK)
			where o.BrandID = @BrandID
			and o.StyleID = @StyleID
			and o.SeasonID = @SeasonID
			and o.MDivisionID = @MDivisionID
			and o.Category = 'B'

			--SewingOffline
			select @SewingOffline = min(o.SewOffLine) from Orders o WITH (NOLOCK)
			where o.BrandID = @BrandID
			and o.StyleID = @StyleID
			and o.SeasonID = @SeasonID
			and o.MDivisionID = @MDivisionID
			and o.Category = 'B' 
　　
			--更新GarmentTest資料
			update GarmentTest Set OrderID = @OrderID , DeadLine = @DeadLine , SewingInline = @SewingInline , SewingOffline = @SewingOffline
			where StyleID = @StyleID
			and SeasonID = @SeasonID
			and BrandID = @BrandID
			and Article = @Article
			and MDivisionID = @MDivisionID

　　	  FETCH NEXT FROM CursortmpUpdateData INTO @StyleID, @SeasonID, @BrandID, @Article, @MDivisionID    --下移Cursor 塞入下一筆參數資料
	END

	CLOSE CursortmpUpdateData    --關閉Cursor
	DEALLOCATE CursortmpUpdateData


	--刪除GarmentTest不存在Orders的資料
	delete GarmentTest
	where SewingInline > Convert(Date,GETDATE())
	and not exists (select 1 from Orders o WITH (NOLOCK), Order_Qty oq WITH (NOLOCK)
					where o.ID = oq.ID
					and o.MDivisionID = GarmentTest.MDivisionid 
					and o.BrandID = GarmentTest.BrandID 
					and o.StyleID = GarmentTest.StyleID 
					and o.SeasonID = GarmentTest.SeasonID
					and oq.Article = GarmentTest.Article
					and o.Junk = 0
					and o.Category = 'B')

	--clean up
	DROP TABLE #tmpData
	DROP TABLE #tmpLackData
	DROP TABLE #tmpUpdateData

END