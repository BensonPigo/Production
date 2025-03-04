

-- =============================================
-- Author:		<Alger Song>
-- Create date: <2017/01/24>
-- Description:	<ChangeOver>
-- =============================================
CREATE PROCEDURE [dbo].[ChangeOver]

--此為初始值。設定參數控制 0 = 排程執行、1 = 手動執行
@UpdateRecentData bit = 0

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	--取得30分鐘前的時間
	Declare @UpdateRecentDate Datetime = dateadd(minute, -30, getdate())

   --刪除不存在Sewing Schedule的資料
	select distinct s.APSNo,s.FactoryID,s.SewingLineID,s.OrderID,s.ComboType
	into #checker
	from SewingSchedule s WITH (NOLOCK)

	create index chkExists on #checker(APSNo,FactoryID,SewingLineID,OrderID,ComboType)

	delete from ChgOver 
	where not exists (select 1 
						from #checker s, Factory f 
						where s.APSNo = ChgOver.APSNo and s.SewingLineID = ChgOver.SewingLineID and s.OrderID = ChgOver.OrderID and s.ComboType = ChgOver.ComboType and s.FactoryID = f.ID and f.IsSampleRoom = 0);

	--更新現有資料
	update ChgOver set Inline = s.Inline,AlloQty = s.AlloQty,StandardOutput = s.StandardOutput,TotalSewingTime = s.TotalSewingTime
	from SewingSchedule s where s.APSNo = ChgOver.APSNo and s.OrderID = ChgOver.OrderID and (@UpdateRecentData = 0 or (@UpdateRecentData = 1 and (s.AddDate >= @UpdateRecentDate or s.EditDate >= @UpdateRecentDate)));
	update ChgOver set StyleID = o.StyleID,CDCodeID = o.CdCodeID, SeasonID = o.SeasonID
	from Orders o where o.ID = ChgOver.OrderID;

	--產生ChgOver資料
	Declare cursor_tmpSewing Cursor for
	select s.FactoryID,s.SewingLineID,s.Inline,s.OrderID,s.APSNo,s.ComboType,s.AlloQty,s.TotalSewingTime,s.StandardOutput,
	isnull(o.StyleID,'') as StyleID,isnull(o.SeasonID,'') as SeasonID,o.CdCodeID,
	LAG(isnull(o.StyleID,'')+s.ComboType,1,'') OVER (Partition by s.FactoryID,s.SewingLineID Order by s.FactoryID,s.SewingLineID,s.Inline,s.OrderID) as Compare,f.MDivisionID 
	from SewingSchedule s WITH (NOLOCK)
	left join Orders o WITH (NOLOCK) on s.OrderID = o.ID
	left join Factory f WITH (NOLOCK) on s.FactoryID = f.ID
	where s.Inline is not null 
	and s.Offline > DATEADD(MONTH,-1,GETDATE()) 
	and f.IsSampleRoom = 0
	and @UpdateRecentData = 0 or (@UpdateRecentData = 1 and (s.AddDate >= @UpdateRecentDate or s.EditDate >= @UpdateRecentDate))
	order by s.FactoryID,s.SewingLineID,s.Inline,s.OrderID;


	--宣告變數: 記錄程式中的資料
	DECLARE @factoryid VARCHAR(8), --工廠別
			@sewinglineid VARCHAR(5), --Sewing Line ID
			@inline DATETIME, --上線日
			@apsno INT, --APS系統Sewing Schedule的ID
			@combotype VARCHAR(1), --組合型態
			@alloqty INT, --生產數量
			@ttlsewingtime INT, --總秒數
			@stdoutput INT, --Standard Output 
			@styleid VARCHAR(15), --款式
			@seasonid VARCHAR(10), --季節
			@cdcodeid VARCHAR(6), --CD Code
			@orderid VARCHAR(13), --訂單編號
			@compare VARCHAR(26), --紀錄上一筆的StyleID+ComboType
			@type VARCHAR(1), --New/Repeat
			@chgoverid INT, --紀錄ChgOver.ID
			@chgoverinline DATETIME, --紀錄ChgOver.Inline
			@MDivisionID as varchar(20) 

	--開始run cursor
	OPEN cursor_tmpSewing
	--將第一筆資料填入變數
	FETCH NEXT FROM cursor_tmpSewing INTO @factoryid,@sewinglineid,@inline,@orderid,@apsno,@combotype,@alloqty,@ttlsewingtime,@stdoutput,@styleid,@seasonid,@cdcodeid,@compare,@MDivisionID
	WHILE @@FETCH_STATUS = 0
	BEGIN
		IF @compare <> ''
			BEGIN
				IF @styleid+@combotype <> @compare --換款式
					BEGIN
						IF EXISTS(select 1 from ChgOver WITH (NOLOCK) where APSNo = @apsno and FactoryID = @factoryid and SewingLineID = @sewinglineid and OrderID = @orderid and ComboType = @combotype)
							BEGIN
								select @chgoverid = ID,@chgoverinline = Inline
								from ChgOver WITH (NOLOCK)
								where FactoryID = @factoryid and SewingLineID = @sewinglineid and StyleID = @styleid and ComboType = @combotype;
								IF @chgoverinline < @inline
									SET @type = 'R'
								ELSE
									BEGIN
										SET @type = 'N'
										update ChgOver set Type = 'R' where ID = @chgoverid
									END
								select @chgoverid = ID from ChgOver WITH (NOLOCK) where APSNo = @apsno and FactoryID = @factoryid and SewingLineID = @sewinglineid and OrderID = @orderid and ComboType = @combotype
								update ChgOver set Type = @type, Inline = @inline where ID = @chgoverid
							END
						ELSE
							BEGIN
								select @chgoverid = ID,@chgoverinline = Inline
								from ChgOver WITH (NOLOCK)
								where FactoryID = @factoryid and SewingLineID = @sewinglineid and StyleID = @styleid and ComboType = @combotype;
								IF @chgoverinline < @inline
									SET @type = 'R'
								ELSE
									BEGIN
										SET @type = 'N'
										update ChgOver set Type = 'R' where ID = @chgoverid
									END
								insert into ChgOver (OrderID,ComboType,FactoryID,APSNo,StyleID,SeasonID,SewingLineID,CDCodeID,Inline,TotalSewingTime,AlloQty,StandardOutput,Type,Status,AddDate,MDivisionID)
								values (@orderid,@combotype,@factoryid,@apsno,@styleid,@seasonid,@sewinglineid,@cdcodeid,@inline,@ttlsewingtime,@alloqty,@stdoutput,@type,'NEW',GETDATE(),@MDivisionID)
							END
					END
				ELSE
					BEGIN
						IF EXISTS(select 1 from ChgOver WITH (NOLOCK) where APSNo = @apsno and FactoryID = @factoryid and SewingLineID = @sewinglineid and OrderID = @orderid and ComboType = @combotype)
							BEGIN
								delete ChgOver
								where APSNo = @apsno and FactoryID = @factoryid and SewingLineID = @sewinglineid and OrderID = @orderid and ComboType = @combotype
							END
					END
			END
		FETCH NEXT FROM cursor_tmpSewing INTO @factoryid,@sewinglineid,@inline,@orderid,@apsno,@combotype,@alloqty,@ttlsewingtime,@stdoutput,@styleid,@seasonid,@cdcodeid,@compare,@MDivisionID
	END
	CLOSE cursor_tmpSewing
	DEALLOCATE cursor_tmpSewing

	--刪除連續的StyleID+ComboType
	select *
	from (select ID,StyleID,ComboType,FactoryID,SewingLineID,StyleID+ComboType as CurrentRec,LAG(StyleID+ComboType,1,'') OVER (Partition by FactoryID,SewingLineID Order by FactoryID,SewingLineID,Inline,ID) as LastRec
		  from ChgOver WITH (NOLOCK)) a
	where a.LastRec <> ''
	and a.CurrentRec = a.LastRec;

	--刪除已不存在ChgOver的ChgOver_Check & ChgOver_Problem
	delete from ChgOver_Check where not exists (select 1 from ChgOver where ChgOver.ID = ChgOver_Check.ID);
	delete from ChgOver_Problem where not exists (select 1 from ChgOver where ChgOver.ID = ChgOver_Problem.ID);

	--當Update Type = R時, 若[ChgOver_Check].[ActualDate]全為空 (條件[ChgOver_Check].ID=[ChgOver].ID)則將此筆 [ChgOver_Check] 資料刪除
	--對應ChgOver_Check下所有資料的ActualDate都是空的才刪，若有一筆ActualDate有值則不刪
	delete from ChgOver_Check where ID in (select c.ID
									from ChgOver c
									inner join ChgOver_Check cc on c.ID = cc.ID
									where c.Type = 'R'
									group by c.ID
									having sum(iif(cc.ActualDate is null,0,1)) = 0
									);

	--填Category欄位值
	update ChgOver set Category = c.Category
	from (
		select
			ID,
			Category = iif(b.LastProdType <> '' and b.ProductionType <> '', 
				(
				case when b.ProductionType <> b.LastProdType and b.FabricType <> b.LastFabType then 'A'
					when b.ProductionType <> b.LastProdType and b.FabricType = b.LastFabType then 'B'
					when b.ProductionType = b.LastProdType and b.FabricType <> b.LastFabType then 'C'
					when b.ProductionType = b.LastProdType and b.FabricType = b.LastFabType then 'D'
					else ''
					end),'') 
		from (
			select ID,ProductionType,FabricType,LAG(ProductionType,1,'') OVER (Partition by a.FactoryID,a.SewingLineID order by a.FactoryID,a.SewingLineID,a.Inline,a.ID) as LastProdType,
			LAG(FabricType,1,'') OVER (Partition by a.FactoryID,a.SewingLineID order by a.FactoryID,a.SewingLineID,a.Inline,a.ID) as LastFabType
			from (
				select co.ID,co.FactoryID,co.SewingLineID,co.StyleID,co.ComboType,co.Inline,
				ProductionType = case when s.StyleUnit = 'PCS'
									  then (select r.Name from Reason r where ReasonTypeID = 'Style_Apparel_Type' and r.ID = s.ApparelType)
									  else (select r.Name from Style_Location sl inner join Reason r on r.ID = sl.ApparelType where ReasonTypeID = 'Style_Apparel_Type' and sl.StyleUkey = s.Ukey and sl.Location =co.ComboType)
									  end,
				FabricType = case when s.StyleUnit = 'PCS' then s.FabricType
									  else (select sl.FabricType from Style_Location sl where sl.StyleUkey = s.Ukey and sl.Location =co.ComboType)
									  end
			from ChgOver co
			inner join orders o on o.id = co.OrderID
			inner join Style s on s.Ukey = o.StyleUkey
			) a
		) b
	) c
	where c.ID = ChgOver.ID

	----------------ChgOver_Check-------------------------------

	SELECT *
	INTO #tmp
	FROM ChgOver co WITH (nolock)
	WHERE co.Inline >= DATEADD(year, -1, GETDATE())
	AND co.Inline >= '2025-01-01'

	DELETE CC
	FROM ChgOver_Check CC
	INNER JOIN #tmp t WITH(NOLOCK) on t.ID = CC.ID
    Where Not exists ( 
                       Select 1
				       From  ChgOverCheckList ckl 
			           Inner join ChgOverCheckList_Detail ckd with (nolock) on ckl.ID = ckd.ID
				       Where cc.ChgOverCheckListID = ckl.ID 
					   and  ckl.Category = T.Category AND ckl.StyleType = t.Type and ckl.FactoryID = T.FactoryID
                       and cc.No = ckd.ChgOverCheckListBaseID 
                     )
	
	INSERT INTO ChgOver_Check
	(
		[ID]
		,[DayBe4Inline]
		,[BaseOn]
		,[ChgOverCheckListID]
		,[DeadLine]
		,[CompletionDate]
		,[Remark]
		,[No]
		,[Checked]
		,[LeadTime]
		,[EditName]
		,[EditDate]
		,[ResponseDep]
	)
	SELECT
	[ID] = T.ID
	,[DayBe4Inline] = 0
	,[BaseOn] = 0
	,[ChgOverCheckListID] = ISNULL(CC.ID,0) 
	,[DeadLine] = dbo.CalculateWorkDate(T.Inline, -CCD.LeadTime, T.FactoryID)
	,[CompletionDate] = NULL
	,[Remaek] = ''
	,[No] = cb.[NO]
	,[Checked] = 0
	,[Leadtime] = ISNULL(CCD.LeadTime,0)
	,[EditName] = ''
	,[EditDate] = NULL
	,[ResponseDep] = ISNULL(CCD.ResponseDep,'')
	From #tmp t
    INNER JOIN ChgOverCheckList CC ON CC.Category = T.Category AND CC.StyleType = t.Type and CC.FactoryID = T.FactoryID
	INNER JOIN ChgOverCheckList_Detail CCD ON CCD.ID = CC.ID
	INNER JOIN ChgOverCheckListBase cb with (nolock) on CCD.ChgOverCheckListBaseID = cb.ID
	Where Not exists ( 
						Select 1
						From ChgOver_Check cc
						Where cc.id = t.ID
						and cc.ChgOverCheckListID = CCD.ID
						AND cc.No = CCD.ChgOverCheckListBaseID
					 )

	Update cck Set Deadline = dbo.CalculateWorkDate(t.Inline, -CCD.LeadTime, t.FactoryID),
	LeadTime = CCD.LeadTime, 
	ResponseDep = CCD.ResponseDep
	From ChgOver_Check cck
	INNER JOIN #tmp t on t.ID = cck.ID
    INNER JOIN ChgOverCheckList CC ON CC.Category = T.Category AND CC.StyleType = t.Type and CC.FactoryID = T.FactoryID
	INNER JOIN ChgOverCheckList_Detail CCD with (nolock) on CC.ID = CCD.ID and cck.No = CCD.ChgOverCheckListBaseID

	DROP TABLE #tmp
END