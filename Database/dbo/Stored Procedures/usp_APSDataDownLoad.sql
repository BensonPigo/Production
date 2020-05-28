
CREATE PROCEDURE [dbo].[usp_APSDataDownLoad]
@apsservername varchar(50), @apsdatabasename varchar(25), @factoryid varchar(8), @login varchar(10)
AS
BEGIN
	DECLARE @cmd VARCHAR(MAX)
	DECLARE	@_i int

	--避免Divide by zero error encountered
	SET ARITHABORT ON 
	SET ANSI_WARNINGS ON

	--SewingLine
	BEGIN

	--撈APS上SewingLine資料
	SET @cmd = '
	DECLARE cursor_sewingline CURSOR FOR 
	SELECT Facility.GroupName
		   , SUBSTRING(Facility.NAME,1,2) as Name
		   , Facility.Description
		   , Facility.Workernumber 
		   , Facility.STATE
	FROM ['+ @apsservername + '].'+@apsdatabasename+'.dbo.Factory
	     ,['+ @apsservername + '].'+@apsdatabasename+'.dbo.Facility 
	WHERE Factory.CODE = '''+ @factoryid + ''' 
		  and Facility.FactoryId = Factory.Id'
	
	Begin Try
		execute (@cmd)						
	End Try
	Begin Catch
		EXECUTE usp_GetErrorInfo;
	End Catch
	
	--sent data to GZ WebAPI 會在PPIC P07使用此temp table傳送異動資料
	Create table #tmpSewingLineGZ
	(
		ID varchar(2) null,
		FactoryID varchar(8) null,
		[Action] varchar(5) null
	)

	Create table #tmpSewingScheduleGZ
	(
		ID bigint null,
		[Action] varchar(5) null
	)

	DECLARE @sewingcell varchar(2),
			@sewinglineid varchar(2),
			@description nvarchar(500),
			@sewer int,
			@set numeric(24,10),
			@STATE bit
	OPEN cursor_sewingline
	FETCH NEXT FROM cursor_sewingline 
	INTO @sewingcell,@sewinglineid,@description,@sewer,@STATE
	
	WHILE @@FETCH_STATUS = 0
	BEGIN
		declare @tmpcell varchar(2),
				@tmpdesc nvarchar(500),
				@tmpsewer int,
				@Junk bit
		set @tmpcell = null
		select @tmpcell = SewingCell
			   ,@tmpdesc = Description
			   , @tmpsewer = Sewer
			   , @Junk=Junk 
		from SewingLine 
		where ID = @sewinglineid 
			  and FactoryID = @factoryid
		
		IF @tmpcell is not null
			BEGIN
				--當資料已存在PMS且值有改變就更新
				IF isnull(@tmpcell,'') <> isnull(@sewingcell,'') or isnull(@tmpdesc,'') <> isnull(@description,'') or isnull(@tmpsewer,'') <> isnull(@sewer,'') or isnull(@Junk,0) <> isnull(@STATE,0)
				begin
					update SewingLine 
					set SewingCell = isnull(@sewingcell,'')
						, Description = isnull(@description,'')
						, Sewer = isnull(@sewer,0)
						, EditName = @login
						, EditDate = GETDATE()
						, Junk =@STATE  
					output	INSERTED.ID,
							INSERTED.FactoryID,
							'U' as [Action]
					into #tmpSewingLineGZ
					where ID = @sewinglineid 
						  and FactoryID = @factoryid;
				end
			END
		ELSE
			BEGIN
				--當資料不存在PMS就新增資料
				insert into SewingLine
				(ID,Description,FactoryID,SewingCell,Sewer,AddName,AddDate,Junk) 
				output	INSERTED.ID,
						INSERTED.FactoryID,
						'I' as [Action]
				into #tmpSewingLineGZ
				values 
				(@sewinglineid,isnull(@description,''),@factoryid,isnull(@sewingcell,''),isnull(@sewer,0),@login, GETDATE(),@STATE);

			END
		FETCH NEXT FROM cursor_sewingline 
		INTO @sewingcell,@sewinglineid,@description,@sewer,@STATE
	END
	CLOSE cursor_sewingline
	DEALLOCATE cursor_sewingline

	--刪除PMS多的資料
	CREATE TABLE #tmpSewingLine (ID Varchar(2));
	SET @cmd = '
		insert into #tmpSewingLine 
		SELECT SUBSTRING(Facility.NAME,1,2) as ID 
		FROM ['+ @apsservername + '].'+@apsdatabasename+'.dbo.Factory
			 ,['+ @apsservername + '].'+@apsdatabasename+'.dbo.Facility 
		WHERE Factory.CODE = '''+ @factoryid + ''' 
			  and Facility.FactoryId = Factory.Id'
	execute (@cmd);

	DECLARE cursor_sewingline CURSOR FOR
	select ID 
	from SewingLine 
	where FactoryID = @factoryid
	except
	select ID 
	from #tmpSewingLine;

	OPEN cursor_sewingline
	FETCH NEXT FROM cursor_sewingline 
	INTO @sewinglineid
	
	WHILE @@FETCH_STATUS = 0
	BEGIN
		delete 
		from SewingLine 
		output	deleted.ID,
				deleted.FactoryID,
				'D' as [Action]
		into #tmpSewingLineGZ
		where ID = @sewinglineid 
			  and FactoryID = @factoryid;

		FETCH NEXT FROM cursor_sewingline 
		INTO @sewinglineid
	END
	CLOSE cursor_sewingline
	DEALLOCATE cursor_sewingline
	drop table #tmpSewingLine;

	END

	--Holiday
	BEGIN
	--準備日期範圍
	create table #dateranges ([holidaydate] [date])
	declare @startDate date = DATEADD(DAY,-10,GETDATE()) 
	declare @EndDate date = DATEADD(DAY,160,GETDATE())
	while @startDate <= @EndDate
	begin
		insert into #dateranges 
		values (@startDate)	
		
		set @startDate =  DATEADD(DAY, 1,@startDate)
	end
	set @startDate = DATEADD(DAY,-10,GETDATE()) 

	--APS上的Holiday
	CREATE TABLE #APSHoliday
	([CODE] varchar(8), [Name] nvarchar(20), [FromDate] datetime, [ToDate] datetime)

	set @cmd = '
	insert into #APSHoliday
	SELECT  Factory.CODE
			, Name = convert(nvarchar(20),Holiday.Name)
			, Holiday.FromDate
			, Holiday.ToDate
	FROM ['+ @apsservername + '].'+@apsdatabasename+'.dbo.Factory
		,['+ @apsservername + '].'+@apsdatabasename+'.dbo.Holiday
	WHERE Holiday.FactoryId = Factory.Id
		  and Holiday.FromDate >= DATEADD(DAY,-10,GETDATE())'
	execute (@cmd)

	--拆日期FromDate~ToDate
	select d.holidaydate,h.NAME,h.CODE
	into #Holiday
	from #dateranges d 
	inner join #APSHoliday h on d.holidaydate between FromDate and ToDate

	--更新/新增Table Holiday
	merge Holiday t
	using (select * from #Holiday) s
	on t.HolidayDate = s.HolidayDate 
	   and t.FactoryID = s.Code
	when matched then
		update set
			t.Name = s.Name
			, t.EditName = @login
			, t.EditDate = GETDATE()
	when not matched by target then 
		insert(FactoryID,HolidayDate,Name,AddName,AddDate)
		values (s.Code,s.HolidayDate,s.Name,@login, GETDATE());
	
	--Table WorkHour.holiday 標記為1, 或取消為0
	BEGIN	
	update w set
		w.holiday = 1
	FROM WorkHour w 
	inner join #Holiday h on h.HolidayDate = w.Date 
							 and h.CODE = w.FactoryID
							 and h.HolidayDate between @startDate and @EndDate
							 and w.holiday = 0
	
	update w set
		w.holiday = 0
	FROM WorkHour w 
	left join #Holiday h on h.HolidayDate = w.Date 
							and h.CODE = w.FactoryID
	where h.HolidayDate is null
		  and w.date between @startDate and @EndDate
		  and w.holiday = 1
	END	
	--
	drop table #dateranges,#Holiday

	--刪除PMS多的資料
	CREATE TABLE #tmpHoliday 
	(FromDate datetime, ToDate datetime);

	SET @cmd = '
	insert into #tmpHoliday 
	SELECT Holiday.FromDate
		   , Holiday.ToDate 
	FROM ['+ @apsservername + '].'+@apsdatabasename+'.dbo.Factory
		 , ['+ @apsservername + '].'+@apsdatabasename+'.dbo.Holiday 
	WHERE Factory.CODE = '''+ @factoryid + ''' 
		  and Holiday.FactoryId = Factory.Id 
		  and (Holiday.FromDate >= DATEADD(DAY,-10,GETDATE()) 
			   or Holiday.ToDate >= DATEADD(DAY,-10,GETDATE()))'
	execute (@cmd)

	DECLARE cursor_holiday CURSOR FOR 
	select a.HolidayDate 
	from (
		select h.HolidayDate
			   , IIF(t.FromDate is null,0,1) as Found 
		from Holiday h
		left join  #tmpHoliday t on h.HolidayDate between CONVERT(DATE,t.FromDate) and CONVERT(DATE,t.ToDate)
		where h.FactoryID = @factoryid 
			  and h.HolidayDate >= CONVERT(DATE,DATEADD(DAY,-10,GETDATE()))
	) a
	where a.Found = 0;

	OPEN cursor_holiday
	FETCH NEXT FROM cursor_holiday 
	INTO @startdate

	WHILE @@FETCH_STATUS = 0
	BEGIN
		Begin Try
			Begin Transaction
				delete 
				from Holiday 
				where FactoryID = @factoryid 
					  and HolidayDate = @startdate;
				
				--更新WorkHour
				update WorkHour 
					set Holiday = 0 
				where FactoryID = @factoryid 
					  and Date = @startdate;
			Commit Transaction;
		End Try
		Begin Catch
			RollBack Transaction

			EXECUTE usp_GetErrorInfo;
		End Catch

		FETCH NEXT FROM cursor_holiday INTO @startdate
	END
	CLOSE cursor_holiday
	DEALLOCATE cursor_holiday

	drop table #tmpHoliday;
	END
		
	--WorkHour
	--因為APS系統有3個WorkHour的資料來源，特殊時間(SPECIALCALENDAR)->生產線日曆(WORKCALENDARAPPLY, TARGETTYPE = 1)->工廠日曆(WORKCALENDARAPPLY, TARGETTYPE = 0)，回寫至PMS順序為工廠日曆->生產線日曆->特殊時間
	BEGIN
	--撈工廠日曆資料
	--DECLARE @tmpFtyTemplate table (NAME,TEMPLATEID,ENABLEDATE)
	CREATE TABLE #tmpFtyTemplate 
	(NAME varchar(50),TEMPLATEID int,ENABLEDATE datetime);

	SET @cmd = '
	insert into #tmpFtyTemplate 
	select fa.NAME
		   , w.TEMPLATEID
		   , w.ENABLEDATE
	from ['+ @apsservername + '].'+@apsdatabasename+'.dbo.WorkCalendarApply w
	      , ['+ @apsservername + '].'+@apsdatabasename+'.dbo.Factory f
	      , ['+ @apsservername + '].'+@apsdatabasename+'.dbo.Facility fa
	where f.ID = w.TargetID
		  and w.TargetType = 0
		  and fa.FactoryID = f.ID
		  and f.Code = '''+ @factoryid + '''
		  order by fa.NAME,w.ENABLEDATE'
	execute (@cmd)

	CREATE TABLE #tmpFtyCalendar 
	(TemplateID int,DayOfWeekIndex int,WorkHour numeric(24,10));

	SET @cmd = '
	insert into #tmpFtyCalendar 
	select TemplateID
		   , DayOfWeekIndex
		   , SUM(EndHour - StartHour) as WorkHour
	from ['+ @apsservername + '].'+@apsdatabasename+'.dbo.CalendarTemplateDetail
	group by TemplateID,DayOfWeekIndex
	order by TemplateID,DayOfWeekIndex'
	execute (@cmd)
	
	--取得工廠日曆的詳細資料(詳細到上班時間為幾點到幾點)
	CREATE TABLE #workhour_Detail_Fty
	(SewingLineID varchar(50),FactoryID varchar(8),StartHour Float,EndHour Float ,TemplateID int ,DayOfWeekIndex int);

	SET @cmd = '
		INSERT INTO #workhour_Detail_Fty
		SELECT DISTINCT 
				[SewingLineID]=''''
				,[FactoryID]=f.NAME
				,c.STARTHOUR
				,c.ENDHOUR
				,w.TEMPLATEID
				,c.DAYOFWEEKINDEX
		FROM ['+ @apsservername + '].'+@apsdatabasename+'.dbo.WorkCalendarApply w 
		INNER JOIN ['+ @apsservername + '].'+@apsdatabasename+'.dbo.Factory f ON  f.ID=w.TARGETID
		INNER JOIN ['+ @apsservername + '].'+@apsdatabasename+'.dbo.Facility fa ON fa.FactoryID = f.ID
		INNER JOIN ['+ @apsservername + '].'+@apsdatabasename+'.dbo.CALENDARTEMPLATEDETAIL c ON c.TEMPLATEID=w.TEMPLATEID
		WHERE  w.TargetType = 0 and f.Code =  '''+ @factoryid + '''
		ORDER BY w.TEMPLATEID,c.DAYOFWEEKINDEX
		'
	execute (@cmd)

	DECLARE cursor_sewingline CURSOR FOR 
	select ID 
	from SewingLine 
	where FactoryID = @factoryid 
	order by ID

	declare @workdate date,
			@apsworkhour numeric(24,10),
			@foundworkhour numeric(24,10),
			@templateid int,
			@found_Detail_StartHour numeric(24,10),
			@found_Detail_EndHour numeric(24,10),
			@Holiday int

	OPEN cursor_sewingline
	FETCH NEXT FROM cursor_sewingline INTO @sewinglineid
	WHILE @@FETCH_STATUS = 0
	BEGIN
		set @_i = 0
		SET @workdate = DATEADD(DAY,-1, GETDATE())
		WHILE (@_i < 160)
		BEGIN
			SET @workdate = DATEADD(DAY,1,@workdate)
			set @Holiday = (select COUNT(FactoryID) from Holiday where FactoryID = @factoryid and HolidayDate = @workdate)
			SET @apsworkhour = null
			SET @templateid = null
			--找工廠日曆的工時
			SET @templateid = (select TOP (1) TEMPLATEID 
							   from #tmpFtyTemplate 
							   where Name = @sewinglineid 
									 and EnableDate <= @workdate 
							   order by EnableDate desc)

			IF @templateid is not null
				BEGIN
					select @apsworkhour = WorkHour 
					from #tmpFtyCalendar 
					where TemplateID = @templateid 
						  and DayOfWeekindex = (DATEPART(WEEKDAY, @workdate)-1)

					IF @apsworkhour is not null
						BEGIN
							SET @foundworkhour = null

							select @foundworkhour = Hours 
							from WorkHour 
							where SewingLineID = @sewinglineid 
								  and FactoryID = @factoryid 
								  and Date = @workdate

							--檢查是否有已存在的資料
							SET @found_Detail_StartHour = null
							SET @found_Detail_EndHour = null

							select TOP 1 
								   @found_Detail_StartHour = StartHour 
								   , @found_Detail_EndHour = EndHour 
							from [Workhour_Detail] 
							where SewingLineID = @sewinglineid 
								  and FactoryID = @factoryid 
								  and Date = CAST( @workdate AS DATE)

							--寫入WorkHour
							IF @foundworkhour is null
								insert into WorkHour
								(SewingLineID,FactoryID,Date,Hours,Holiday,AddName,AddDate)
								values 
								(@sewinglineid,@factoryid,@workdate, iif (@Holiday = 0, @apsworkhour, 0), @Holiday, @login,GETDATE());
							ELSE
							BEGIN
								IF @foundworkhour <> @apsworkhour
									update WorkHour 
									set Hours = iif (@Holiday = 0, isnull(@apsworkhour,0), 0)
										, EditName = @login
										, EditDate = GETDATE() 
										, Holiday = @Holiday
									where SewingLineID = @sewinglineid 
										  and FactoryID = @factoryid 
										  and Date = @workdate
							END

							--寫入Workhour_Detail (新增 / 更新都在這裡進行)

							--有的話先刪除
							IF @found_Detail_StartHour IS NOT NULL AND @found_Detail_EndHour IS NOT NULL
							BEGIN
								DELETE 
								FROM [Workhour_Detail] 
								WHERE SewingLineID = @sewinglineid 
									  and FactoryID = @factoryid 
									  and Date = CAST( @workdate AS DATE)
							END

							if @Holiday = 0
							begin
								INSERT INTO [dbo].[Workhour_Detail]
								([SewingLineID],[FactoryID],[Date],[StartHour],[EndHour])
								SELECT   [SewingLineID]=@sewinglineid
										,[FactoryID]=@factoryid
										,[Date]=CAST( @workdate AS DATE) 
										,[StartHour]=StartHour
										,[EndHour]=EndHour
								FROM #workhour_Detail_Fty
								WHERE DayOfWeekindex=(DATEPART(WEEKDAY, @workdate)-1) 
									  AND TemplateID=@templateid 
									  AND FactoryID = @factoryid
							end
							;
						END
					ELSE --檢查PMS是否有這筆資料，若有就把workhour改成0
						BEGIN
							SET @foundworkhour = null
							select @foundworkhour = Hours 
							from WorkHour 
							where FactoryID = @factoryid 
								  and SewingLineID = @sewinglineid 
								  and Date = @workdate 
								  and Hours > 0

							IF @foundworkhour is not null
							BEGIN
								update WorkHour 
								set Hours = 0
									, EditName = @login
									, EditDate = GETDATE() 
									, Holiday = @Holiday
								where SewingLineID = @sewinglineid 
									  and FactoryID = @factoryid 
									  and Date = @workdate
								
								DELETE 
								FROM [Workhour_Detail] 
								WHERE SewingLineID = @sewinglineid 
									  and FactoryID = @factoryid 
									  and Date = CAST( @workdate AS DATE)
							END						
						END
				END
			ELSE  --檢查PMS是否有這筆資料，若有就把workhour改成0
				BEGIN
					SET @foundworkhour = null
					select @foundworkhour = Hours 
					from WorkHour 
					where FactoryID = @factoryid 
						  and SewingLineID = @sewinglineid 
						  and Date = @workdate 
						  and Hours > 0

					IF @foundworkhour is not null
					BEGIN
						update WorkHour 
						set Hours = 0
							, EditName = @login
							, EditDate = GETDATE()
							, Holiday = @holiday 
						where SewingLineID = @sewinglineid 
							  and FactoryID = @factoryid 
							  and Date = @workdate
						
						DELETE 
						FROM [Workhour_Detail] 
						WHERE SewingLineID = @sewinglineid 
							  and FactoryID = @factoryid 
							  and Date = CAST( @workdate AS DATE)
					END
				END
			
			set @_i = @_i+1
		END

		FETCH NEXT FROM cursor_sewingline 
		INTO @sewinglineid
	END
	CLOSE cursor_sewingline
	DEALLOCATE cursor_sewingline

	drop table #tmpFtyTemplate;

	--撈生產線日曆
	CREATE TABLE #tmpProdTemplate
	(Code varchar(50),NAME varchar(50),TEMPLATEID int,ENABLEDATE datetime,DAYOFWEEKINDEX int,WorkHour numeric(4,2))

	SET @cmd = '
	insert into #tmpProdTemplate 
	SELECT E.Code
		   , D.Name
		   , A.TEMPLATEID
		   , A.ENABLEDATE
		   , C.DAYOFWEEKINDEX
		   , WorkHour = Sum(C.ENDHOUR - C.STARTHOUR) 
	FROM ['+ @apsservername + '].'+@apsdatabasename+'.dbo.WORKCALENDARAPPLY A
		,['+ @apsservername + '].'+@apsdatabasename+'.dbo.CALENDARTEMPLATE B
		,['+ @apsservername + '].'+@apsdatabasename+'.dbo.CALENDARTEMPLATEDETAIL C
		,['+ @apsservername + '].'+@apsdatabasename+'.dbo.Facility D
		,['+ @apsservername + '].'+@apsdatabasename+'.dbo.Factory E 
	WHERE A.TEMPLATEID = B.ID 
		  AND B.ID = C.TEMPLATEID 
		  AND D.ID = A.TARGETID 
		  AND E.CODE ='''+ @factoryid + '''
		  and D.FactoryId = E.Id 
		  AND A.TARGETTYPE = 1 
	group by E.Code, D.Name, A.TEMPLATEID, A.ENABLEDATE, C.DAYOFWEEKINDEX
	order by E.Code, D.Name,A.ENABLEDATE desc'
	execute (@cmd)

		--取得生產線日曆的詳細資料(詳細到上班時間為幾點到幾點)
	CREATE TABLE #workhour_Detail_line
	(SewingLineID varchar(50),FactoryID varchar(8),StartHour Float,EndHour Float ,TemplateID int ,DayOfWeekIndex int);

	SET @cmd = '
		INSERT INTO #workhour_Detail_line
		SELECT DISTINCT 
			   [SewingLineID]=fa.NAME
			   ,[FactoryID]=f.NAME
			   ,c.STARTHOUR
			   ,c.ENDHOUR
			   ,w.TEMPLATEID
			   ,c.DAYOFWEEKINDEX
		FROM ['+ @apsservername + '].'+@apsdatabasename+'.dbo.WorkCalendarApply w 
		INNER JOIN ['+ @apsservername + '].'+@apsdatabasename+'.dbo.CALENDARTEMPLATE cc ON cc.ID = w.TEMPLATEID
		INNER JOIN ['+ @apsservername + '].'+@apsdatabasename+'.dbo.CALENDARTEMPLATEDETAIL c ON cc.ID=c.TEMPLATEID
		INNER JOIN ['+ @apsservername + '].'+@apsdatabasename+'.dbo.Facility fa ON fa.ID = w.TARGETID
		INNER JOIN ['+ @apsservername + '].'+@apsdatabasename+'.dbo.Factory f ON  f.ID=fa.FACTORYID
		WHERE  w.TargetType =1 
			   and f.Code = '''+ @factoryid + '''
		ORDER BY fa.NAME,TEMPLATEID,DAYOFWEEKINDEX
		'
	execute (@cmd)

	DECLARE cursor_sewingline CURSOR FOR 
	select distinct 
		   NAME 
	from #tmpProdTemplate

	OPEN cursor_sewingline
	FETCH NEXT FROM cursor_sewingline 
	INTO @sewinglineid

	WHILE @@FETCH_STATUS = 0
	BEGIN
		set @_i = 0
		SET @workdate = DATEADD(DAY,-1, GETDATE())
		WHILE (@_i < 160)
		BEGIN
			SET @workdate = DATEADD(DAY,1,@workdate)			
			set @Holiday = (select COUNT(FactoryID) from Holiday where FactoryID = @factoryid and HolidayDate = @workdate)
			SET @apsworkhour = null

			--找產線日曆的工時
			select TOP(1) 
				   @apsworkhour = WorkHour  
				   , @templateid=TEMPLATEID
			from #tmpProdTemplate 
			where Name = @sewinglineid 
				  and EnableDate <= @workdate 
				  and DayOfWeekindex = (DATEPART(WEEKDAY, @workdate)-1) 
			order by EnableDate desc

			IF @apsworkhour is not null
				BEGIN
					SET @foundworkhour = null
					select @foundworkhour = Hours 
					from WorkHour 
					where SewingLineID = @sewinglineid 
						  and FactoryID = @factoryid 
						  and Date = @workdate

					--檢查是否有已存在的資料
					SET @found_Detail_StartHour = null
					SET @found_Detail_EndHour = null

					select @found_Detail_StartHour = StartHour 
						   , @found_Detail_EndHour = EndHour 
					from [Workhour_Detail] 
					where SewingLineID = @sewinglineid 
						  and FactoryID = @factoryid 
						  and Date  =CAST( @workdate AS DATE)

					--寫入WorkHour
					IF @foundworkhour is null
						insert into WorkHour
						(SewingLineID,FactoryID,Date,Hours,Holiday,AddName,AddDate)
						values 
						(@sewinglineid,@factoryid,@workdate, iif (@Holiday = 0, @apsworkhour, 0), @Holiday,@login,GETDATE());
					ELSE
					BEGIN
						IF @foundworkhour <> @apsworkhour
							update WorkHour 
							set Hours = iif (@Holiday = 0, isnull(@apsworkhour,0), 0)
								, EditName = @login
								, EditDate = GETDATE() 
								, Holiday = @Holiday
							where SewingLineID = @sewinglineid 
								  and FactoryID = @factoryid 
								  and Date = @workdate
					END

					--寫入Workhour_Detail (新增 / 更新都在這裡進行)

					--有的話先刪除
					IF @found_Detail_StartHour IS NOT NULL AND @found_Detail_EndHour IS NOT NULL
					BEGIN
						DELETE 
						FROM [Workhour_Detail]  
						WHERE SewingLineID = @sewinglineid 
							  AND Date = CAST( @workdate AS DATE) 
							  AND FactoryID = @factoryid
					END

					if @Holiday = 0 
					begin
						INSERT INTO [dbo].[Workhour_Detail]
						([SewingLineID],[FactoryID],[Date],[StartHour],[EndHour])
						SELECT [SewingLineID]=SewingLineID 
							   ,[FactoryID]=@factoryid 
							   ,[Date]=CAST( @workdate AS DATE) 
							   ,[StartHour] 
							   ,[EndHour]
						FROM #workhour_Detail_line
						WHERE SewingLineID=@sewinglineid 
							  AND TemplateID=@templateid 
							  AND DayOfWeekIndex=(DATEPART(WEEKDAY, @workdate)-1) 
							  AND FactoryID = @factoryid
					end
					;

				END
			ELSE --檢查PMS是否有這筆資料，若有就把workhour改成0
				BEGIN
					SET @foundworkhour = null

					select @foundworkhour = Hours 
					from WorkHour 
					where FactoryID = @factoryid 
						  and SewingLineID = @sewinglineid 
						  and Date = @workdate 
						  and Hours > 0

					IF @foundworkhour is not null
					BEGIN
						update WorkHour 
						set Hours = 0
							, EditName = @login
							, EditDate = GETDATE() 
							, Holiday = @Holiday
						where SewingLineID = @sewinglineid 
							  and FactoryID = @factoryid 
							  and Date = @workdate
						
						DELETE 
						FROM [Workhour_Detail]  
						WHERE SewingLineID = @sewinglineid 
							  AND Date =  CAST( @workdate AS DATE) 
							  AND FactoryID = @factoryid
					END
				END

			set @_i = @_i+1
		END

		FETCH NEXT FROM cursor_sewingline 
		INTO @sewinglineid
	END
	CLOSE cursor_sewingline
	DEALLOCATE cursor_sewingline

	drop table #tmpFtyCalendar;
	drop table #tmpProdTemplate;

	--撈特殊時間
	SET @cmd = '
	DECLARE cursor_apsspecialtime CURSOR FOR 
	select f.Name
		   , s.SpecialDate
		   , SUM(s.EndHour - s.StartHour) as WorkHour
	from ['+ @apsservername + '].'+@apsdatabasename+'.dbo.SpecialCalendar s
		 , ['+ @apsservername + '].'+@apsdatabasename+'.dbo.Facility f
		 , ['+ @apsservername + '].'+@apsdatabasename+'.dbo.Factory fa
	where fa.Code = '''+ @factoryid + '''
	  	  and f.FactoryID = fa.ID
		  and s.facilityID = f.ID
		  and s.SpecialDate >= CONVERT(DATE,GETDATE())
	group by f.Name, s.SpecialDate'
	execute (@cmd)
		
	--取得特殊班表時間的詳細資料(詳細到上班時間為幾點到幾點)
	CREATE TABLE #workhour_Detail_special
	(SewingLineID varchar(50),FactoryID varchar(8) ,SpecialDate Date,StartHour Float,EndHour Float);
	
	SET @cmd = '
	INSERT INTO #workhour_Detail_special
	SELECT   [SewingLineID]=f.NAME
			, [FactoryID]=fa.CODE
			, s.SpecialDate 
			, s.STARTHOUR
			, s.ENDHOUR   
	FROM ['+ @apsservername + '].'+@apsdatabasename+'.dbo.SpecialCalendar s
	INNER JOIN ['+ @apsservername + '].'+@apsdatabasename+'.dbo.Facility f ON s.facilityID = f.ID
	INNER JOIN ['+ @apsservername + '].'+@apsdatabasename+'.dbo.Factory fa ON f.FactoryID = fa.ID
	WHERE fa.Code ='''+ @factoryid + '''
		  and s.SpecialDate >= CONVERT(DATE,GETDATE())
'
	execute (@cmd)

	OPEN cursor_apsspecialtime
	FETCH NEXT FROM cursor_apsspecialtime 
	INTO @sewinglineid,@workdate,@apsworkhour

	WHILE @@FETCH_STATUS = 0
	BEGIN
		SET @foundworkhour = null
		select @foundworkhour = Hours 
		from WorkHour 
		where FactoryID = @factoryid 
			  and SewingLineID = @sewinglineid 
			  and Date = @workdate

		--檢查是否有已存在的資料
		SET @found_Detail_StartHour = null
		SET @found_Detail_EndHour = null

		select @found_Detail_StartHour = StartHour 
			   , @found_Detail_EndHour = EndHour 
		from [Workhour_Detail] 
		where SewingLineID = @sewinglineid 
			  and FactoryID = @factoryid 
			  and Date = CAST( @workdate AS DATE) 

		--寫入WorkHour
		-- 如果特殊時間加總為零，代表當天為休息日。
		IF @foundworkhour is null
			BEGIN
				insert into WorkHour
				(SewingLineID,FactoryID,Date,Hours,Holiday,AddName,AddDate)
				values 
				(@sewinglineid,@factoryid,@workdate,@apsworkhour,iif (@apsworkhour = 0, 1, 0),@login,GETDATE());			
			END
		ELSE
			BEGIN
				update WorkHour 
				set Hours = isnull(@apsworkhour,0)
					, EditName = @login
					, EditDate = GETDATE() 
					, Holiday = iif (@apsworkhour = 0, 1, 0)
				where SewingLineID = @sewinglineid 
						and FactoryID = @factoryid 
						and Date = @workdate
			END

		--寫入Workhour_Detail (新增 / 更新都在這裡進行)
		IF @found_Detail_StartHour IS NOT NULL AND @found_Detail_EndHour IS NOT NULL
		BEGIN
			DELETE 
			FROM [Workhour_Detail]  
			WHERE SewingLineID = @sewinglineid 
				  AND Date =  CAST( @workdate AS DATE) 
				  AND FactoryID = @factoryid
		END
		
		IF @apsworkhour > 0
		begin
			INSERT INTO [dbo].[Workhour_Detail]([SewingLineID],[FactoryID],[Date],[StartHour],[EndHour])
			SELECT [SewingLineID]=SewingLineID 
					,[FactoryID]=@factoryid
					,[Date]= CAST( @workdate AS DATE) 
					,[StartHour]=STARTHOUR 
					,[EndHour]=ENDHOUR
			FROM #workhour_Detail_special
			WHERE SewingLineID = @sewinglineid 
				  AND SpecialDate =  CAST( @workdate AS DATE) 
				  AND FactoryID = @factoryid;
		end

		FETCH NEXT FROM cursor_apsspecialtime 
		INTO @sewinglineid,@workdate,@apsworkhour
	END
	CLOSE cursor_apsspecialtime
	DEALLOCATE cursor_apsspecialtime
	END

	--[LearnCurve][LearnCurve_Detail]
	BEGIN
	--撈APS上[LNCURVETEMPLATE][LNCURVEDETAIL]來異動[LearnCurve][LearnCurve_Detail]
	SET @cmd = 'BEGIN TRAN;
				MERGE LearnCurve AS T
				USING ['+ @apsservername + '].'+@apsdatabasename+'.dbo.LNCURVETEMPLATE AS S	ON (T.ID = S.ID)
				WHEN NOT MATCHED BY TARGET THEN 
					INSERT 
					(ID, Name, Description, AddName, AddDate) 
					VALUES
					(S.ID, S.NAME, S.DESCRIPTION, '''+ @login + ''', getdate())
				WHEN MATCHED THEN 
					UPDATE 
					SET T.Name = S.NAME
						, T.Description = S.DESCRIPTION
						, T.EditName = '''+ @login + '''
						, T.EditDate = getdate()
				WHEN NOT MATCHED BY SOURCE THEN 
					DELETE;
				COMMIT TRAN;'
	Begin Try
		execute (@cmd)						
	End Try
	Begin Catch
		EXECUTE usp_GetErrorInfo;
	End Catch

	SET @cmd = 'BEGIN TRAN;
				MERGE LearnCurve_Detail AS T
				USING ['+ @apsservername + '].'+@apsdatabasename+'.dbo.LNCURVEDETAIL AS S ON (T.ID = S.TEMPLATEID AND T.Day = S.SERIALNUMBER)
				WHEN NOT MATCHED BY TARGET THEN 
					INSERT
					(ID, Day, Efficiency) 
					VALUES
					(S.TEMPLATEID, S.SERIALNUMBER, S.SNVALUE)
				WHEN MATCHED THEN 
					UPDATE 
					SET T.Efficiency = S.SNVALUE
				WHEN NOT MATCHED BY SOURCE THEN 
					DELETE;
				COMMIT TRAN;'
	Begin Try
		execute (@cmd)						
	End Try
	Begin Catch
		EXECUTE usp_GetErrorInfo;
	End Catch

	END


	--SewingSchedule
	BEGIN
	--撈APS的Sewing Schedule Detail
	CREATE TABLE #tmpAPSSchedule 
	(ID int,POID int,SALESORDERNO varchar(20),REFNO varchar(100),NAME varchar(50),STARTTIME datetime,ENDTIME datetime,UPDATEDATE datetime,POCOLOR char(60),POSIZE varchar(30),PLANAMOUNT numeric(24,10),DURATION numeric(24,10),CAPACITY numeric(24,10),EFFICIENCY numeric(24,10), Sewer int ,LNCSERIALNumber  int, SwitchTime int);
	
	SET @cmd = '
	insert into #tmpAPSSchedule
	SELECT p.ID
		   , pd.POID
		   , po.SALESORDERNO
		   , po.REFNO
		   , fa.NAME
		   , p.STARTTIME
		   , p.ENDTIME
		   , p.UPDATEDATE
		   , pd.POCOLOR
		   , pd.POSIZE
		   , pd.PLANAMOUNT
		   , p.DURATION
		   , po.CAPACITY
		   , pd.EFFICIENCY
		   , sewer = p.WorkerNumber 
		   , p.LNCSERIALNumber 
		   , SwitchTime = case 
							  when p.SwitchType is null or p.SwitchType < 0 then 0
							  else isnull(p.SwitchTime,0)
						  end
	from ['+ @apsservername + '].'+@apsdatabasename+'.dbo.PRODUCTIONEVENT p
		,['+ @apsservername + '].'+@apsdatabasename+'.dbo.Factory f
		,['+ @apsservername + '].'+@apsdatabasename+'.dbo.Facility fa
		,['+ @apsservername + '].'+@apsdatabasename+'.dbo.PO po
		,['+ @apsservername + '].'+@apsdatabasename+'.dbo.PRODUCTIONEVENTDETAIL pd
	where fa.FACTORYID = f.ID 
		and p.FACILITYID = fa.ID 
		and po.ID = pd.POID 
		and pd.PRODUCTIONEVENTID = p.ID 
		and EXISTS (
				SELECT 1 
				from ['+ @apsservername + '].'+@apsdatabasename+'.dbo.PRODUCTIONEVENT aa
					 , ['+ @apsservername + '].'+@apsdatabasename+'.dbo.PRODUCTIONEVENTDETAIL bb 
				where aa.ID=bb.PRODUCTIONEVENTID 
					  and (CONVERT(DATE,aa.ENDTIME) >= CONVERT(DATE,DATEADD(DAY,-90,GETDATE())) 
						   OR CONVERT(DATE,aa.UPDATEDATE) >= CONVERT(DATE,DATEADD(DAY,-90,GETDATE()))) 
		and bb.POID=po.ID)
		and f.Code = '''+ @factoryid + ''''
	execute (@cmd)

	--DECLARE cursor_sewingschedule CURSOR FOR
	select SALESORDERNO as OrderID
		   , ComboType = IIF(REFNO is null or REFNO = '', isnull((select TOP (1) sl.Location from Orders o, Style_Location sl where o.ID = SALESORDERNO and o.StyleUkey = sl.StyleUkey),''),REFNO)
		   ,ID as APSNo 
	into #tmpCheckData 
	from #tmpAPSSchedule
	
	--刪除PMS存在但APS不存在的Schedule
	DECLARE cursor_needtodeletesewingschedule CURSOR FOR
	select APSNo
		   , OrderID
		   , ComboType 
	from SewingSchedule 
	where FactoryID = @factoryid 
		  and CONVERT(DATE,Offline) >= CONVERT(DATE,DATEADD(DAY,-90,GETDATE()))
	except
	select APSNo
		   , OrderID
		   , ComboType 
	from #tmpCheckData

	declare @apsno int,
			@orderid varchar(13),
			@combotype varchar(1)

	OPEN cursor_needtodeletesewingschedule
	FETCH NEXT FROM cursor_needtodeletesewingschedule 
	INTO @apsno,@orderid,@combotype

	WHILE @@FETCH_STATUS = 0
	BEGIN
		Begin Try
			Begin Transaction
				delete 
				from SewingSchedule_Detail 
				where ID in (
						select ID 
						from SewingSchedule 
						where FactoryID = @factoryid 
							  and APSNo = @apsno 
							  and OrderID = @orderid 
							  and ComboType = @combotype);
				
				delete 
				from SewingSchedule 
				output	deleted.ID,
						'D' as [Action]
				into #tmpSewingScheduleGZ
				where FactoryID = @factoryid 
					  and APSNo = @apsno 
					  and OrderID = @orderid 
					  and ComboType = @combotype;
			Commit Transaction;
		End Try
		Begin Catch
			RollBack Transaction

			EXECUTE usp_GetErrorInfo;
		End Catch
		FETCH NEXT FROM cursor_needtodeletesewingschedule 
		INTO @apsno,@orderid,@combotype
	END
	CLOSE cursor_needtodeletesewingschedule
	DEALLOCATE cursor_needtodeletesewingschedule

	--將APS的Sewing Schedule更新進PMS
	DECLARE cursor_sewingschedule CURSOR FOR
	select SALESORDERNO
		   , REFNO
		   , NAME
		   , ID
		   , STARTTIME
		   , ENDTIME
		   , ROUND(CAPACITY*60,0) as GSD
		   , DURATION
		   , UPDATEDATE
		   , SUM(PLANAMOUNT) as AlloQty
		   , POID
		   , OriEFF = Max(EFFICIENCY)
		   , sewer 
		   , LNCSERIALNumber
		   , SwitchTime=isnull(SwitchTime,0)
	from #tmpAPSSchedule
	group by SALESORDERNO,REFNO,NAME,ID,STARTTIME,ENDTIME,ROUND(CAPACITY*60,0),DURATION,UPDATEDATE,POID,Sewer,LNCSERIALNumber, SwitchTime
	order by SALESORDERNO
	/*
		APS的生產計劃資料結構（ProductionEvent、ProductionEventDetail）
		一張生產計畫=一筆ProductionEvent
		一張生產計劃包含多個項目(ProductionEventDetail)，細分到OrderID、ComboType、SewingLineID、Article、SizeCode
		因此 一個ProductionEvent 對 多ProductionEventDetail

		但，PMS的生產計劃資料結構(SewingSchedule、SewingSchedule_Detail)
		第一層SewingSchedule，就已經細分到OrderID、ComboType、SewingLineID		
		而SewingSchedule_Detail，則是進一部細分到Article、SizeCode
		
		同樣是一對多，但是資料分組的方式不一樣

		因此若要寫入正確的SewingSchedule.OriEff，必須根據OrderID、ComboType、SewingLineID	做分組

		P.S ** 根據上述原因，可知得出SewingSchedule.MaxEff的計算方式是錯的
	*/
	
	declare @inline datetime,
			@offline datetime,
			@gsd numeric(24,10),
			@duration numeric(24,10),
			@editdate datetime,
			@alloqty int,
			@sewingscheduleid bigint,
			@pmsassdate datetime,
			@pmseditdate datetime,
			@apspoid int,
			@maxeff numeric(24,10),
			@OriEff numeric(24,10),     -- OriEff 原本的定義是『計畫效率 * 產線效率』，現在重新調整定義OriEff = 計畫效率
			@apseff numeric(24,10),
			@SewLineEff numeric(24,10), -- 生產線效率
			@article varchar(8),
			@sizecode varchar(8),
			@detailalloqty int,
			@LNCSERIALNumber int,       -- 定義為該計畫生產的最後一天對應的是學習曲線的第幾天
			@LnCurveTemplateID int,
			@SwitchTime int -- 換款時間
			;


	CREATE TABLE #ProdEff (Efficiency numeric(24,10))
	CREATE TABLE #LnEff (lnEff numeric(24,10))
	CREATE TABLE #LnCurveTemplate (LnCurveTemplateID int)
	CREATE TABLE #DynamicEff (BeginDate datetime,Eff numeric(24,10))

	OPEN cursor_sewingschedule
	FETCH NEXT FROM cursor_sewingschedule 
	INTO @orderid,@combotype,@sewinglineid,@apsno,@inline,@offline,@gsd,@duration,@editdate,@alloqty,@apspoid ,@OriEff, @sewer ,@LNCSERIALNumber,@SwitchTime
	
	WHILE @@FETCH_STATUS = 0
	BEGIN
		IF @combotype is null or @combotype = ''
			SET @combotype = isnull((select TOP (1) sl.Location from Orders o, Style_Location sl where o.ID = @orderid and o.StyleUkey = sl.StyleUkey),'')

		SET @sewingscheduleid = null
		select @sewingscheduleid = ID
			   , @pmsassdate = AddDate
			   , @pmseditdate = EditDate 
		from SewingSchedule 
		where FactoryID = @factoryid 
			  and OrderID = @orderid 
			  and ComboType = @combotype 
			  and APSNo = @apsno
		
		IF @sewingscheduleid is null
			BEGIN
				--找生產單效率
				SET @cmd = '
				insert into #ProdEff 
				Select Efficiency 
				From ['+ @apsservername + '].'+@apsdatabasename+'.dbo.ProductionEventCapacity 
				where ProdEventID = '+CONVERT(varchar(max),@apsno) + ' 
					  And POID = '+CONVERT(varchar(max),@apspoid)
				execute (@cmd)

				select @apseff = Efficiency 
				from #ProdEff
				
				IF @apseff is null
				begin
					declare @c numeric(15,6)
					
					select @c = COUNT(ID)
					from #tmpAPSSchedule 
					where ID = @apsno

					select @maxeff = isnull(IIF(@c = 0,0,SUM(EFFICIENCY)/@c),0) 
					from #tmpAPSSchedule 
					where ID = @apsno
				end
				ELSE
					SET @maxeff = @apseff

				delete from #ProdEff 

				SET @apseff = null
				--是否有設定LearningCurve，並取得學習曲線效率
				SET @cmd = '
				insert into #LnEff 
				Select Max(ld.Snvalue) as lnEff 
				From ['+ @apsservername + '].'+@apsdatabasename+'.dbo.LnCurveApply l
					,['+ @apsservername + '].'+@apsdatabasename+'.dbo.LnCurveApplyDetail la
					,['+ @apsservername + '].'+@apsdatabasename+'.dbo.LnCurveDetail ld 
				where la.ProductionEventID = '+CONVERT(varchar(max),@apsno) + ' 
					  and l.ID = la.ApplyID 
					  and l.LnCurveTemplateID = ld.TemplateID'
				execute (@cmd)

				select @apseff = isnull((lnEff/100),0) 
				from #LnEff

				--取得生產計劃(ProductionEvent) 是設定哪一個學習曲線樣板(LnCurveTemplateID)--
				
				SET @cmd = '
				INSERT INTO #LnCurveTemplate
				SELECT TOP 1  L.LNCURVETEMPLATEID
				FROM ['+ @apsservername + '].'+@apsdatabasename+'.dbo.LnCurveApply L
				INNER JOIN ['+ @apsservername + '].'+@apsdatabasename+'.dbo.LnCurveApplyDetail LD ON L.ID=LD.ApplyID
				WHERE LD.ProductionEventID='+CONVERT(varchar(max),@apsno) + '
				'
				execute (@cmd)
				set @LnCurveTemplateID = (select LNCURVETEMPLATEID from #LnCurveTemplate)

				IF @apseff is not null and @apseff <> 0
					SET @maxeff = @maxeff*@apseff

				delete from #LnEff 
				delete from #LnCurveTemplate

				SET @apseff = null
				--是否有設定動態效率(動態效率=生產線效率)
				SET @cmd = '
				insert into #DynamicEff 
				Select TOP(1) 
					   fe.BeginDate
					   , isnull((fe.Efficiency/100),0) as Eff 
				From ['+ @apsservername + '].'+@apsdatabasename+'.dbo.FacilityEfficiency fe
					,['+ @apsservername + '].'+@apsdatabasename+'.dbo.Facility f
					,['+ @apsservername + '].'+@apsdatabasename+'.dbo.Factory fa 
				where fe.FacilityID = f.ID 
					  And f.FactoryID = fa.ID 
					  And fa.Code = '''+@factoryid + ''' 
					  and f.Name = ''' + @sewinglineid + ''' 
					  and fe.BeginDate <= ''' + CONVERT(char(19),@inline,120) + ''' 
					  and f.DYNAMICEFFICIENCY = 1
				Order by fe.BeginDate Desc'
				execute (@cmd)
				
				select @apseff = Eff 
				from #DynamicEff

				SET @SewLineEff=@apseff

				IF @apseff is not null and @apseff <> 0
				BEGIN	
					SET @maxeff = @maxeff*@apseff
				END

				SET @sewer = iif (isnull(@sewer, 0) = 0, isnull ((select Sewer from SewingLine where FactoryID = @factoryid and ID = @sewinglineid), 0), @sewer)

				delete from #DynamicEff
				IF @gsd = 0
					set @set = 0
				ELSE
					set @set = (3600/@gsd)*@sewer*@maxeff

				Begin Try
					Begin Transaction
						insert into SewingSchedule
						(OrderID
						,ComboType
						,SewingLineID
						,AlloQty
						,Inline
						,Offline
						,MDivisionID
						,FactoryID
						,Sewer
						,TotalSewingTime
						,MaxEff
						,StandardOutput
						,WorkDay
						,WorkHour
						,APSNo
						,OrderFinished
						,AddName
						,AddDate
						,LearnCurveID
						,OriEff
						,SewLineEff
						,LNCSERIALNumber
						,SwitchTime
						)
						output	inserted.ID,
								'I' as [Action]
						into #tmpSewingScheduleGZ
						values (
						@orderid
						,@combotype
						,@sewinglineid
						,@alloqty
						,@inline
						,@offline,
						(select MDivisionID from Factory where ID = @factoryid)
						,@factoryid
						,@sewer
						,@gsd
						,CONVERT(numeric(5,2),isnull(@maxeff*100,0))
						,CONVERT(int,ROUND(@set,0))--StandardOutput
						,isnull((select COUNT(*) from WorkHour where SewingLineID = @sewinglineid and FactoryID = @factoryid and Date >= CONVERT(DATE,@inline) and Date <= CONVERT(DATE,@offline) and Hours > 0),0)--WorkDay
						,CONVERT(numeric(8,3),@duration)
						,@apsno
						,(select Finished from Orders where ID = @orderid)
						,@login
						,@editdate
						,@LnCurveTemplateID
						,CONVERT(numeric(5,2),isnull(@OriEff*100,100))
						,CONVERT(numeric(5,2),isnull(@SewLineEff*100,100))
						,@LNCSERIALNumber
						,isnull(@SwitchTime,0)
						);

						--取最新的ID
						select @sewingscheduleid = ID from SewingSchedule where FactoryID = @factoryid and OrderID = @orderid and ComboType = @combotype and APSNo = @apsno

						DECLARE cursor_apsscheduledetail CURSOR FOR
						SELECT POCOLOR
							   , POSIZE
							   , PLANAMOUNT = SUM(PLANAMOUNT) 
						from #tmpAPSSchedule 
						where ID = @apsno 
							  and SALESORDERNO = @orderid 
							  and POID = @apspoid 
						GROUP BY POCOLOR, POSIZE

						OPEN cursor_apsscheduledetail
						FETCH NEXT FROM cursor_apsscheduledetail 
						INTO @article,@sizecode,@detailalloqty

						WHILE @@FETCH_STATUS = 0
						BEGIN
							insert into SewingSchedule_Detail
							(ID, OrderID, ComboType,SewingLineID, Article,SizeCode, AlloQty)
							values 
							(@sewingscheduleid,@orderid,@combotype,@sewinglineid,@article,@sizecode,@detailalloqty);

							FETCH NEXT FROM cursor_apsscheduledetail 
							INTO @article,@sizecode,@detailalloqty
						END
						CLOSE cursor_apsscheduledetail
						DEALLOCATE cursor_apsscheduledetail
					Commit Transaction;
				End Try
				Begin Catch
					RollBack Transaction

					EXECUTE usp_GetErrorInfo;
				End Catch
			END
		ELSE
			BEGIN
				IF (@pmseditdate is null and @pmsassdate <> @editdate) or (@pmseditdate is not null and @pmseditdate <> @editdate)
					BEGIN
						SET @apseff = null
						--找生產單效率
						SET @cmd = '
						insert into #ProdEff 
						Select Efficiency
						From ['+ @apsservername + '].'+@apsdatabasename+'.dbo.ProductionEventCapacity 
						where ProdEventID = '+CONVERT(varchar(max),@apsno) + ' 
							  And POID = '+CONVERT(varchar(max),@apspoid)
						execute (@cmd)

						select @apseff = Efficiency 
						from #ProdEff
						
						IF @apseff is null
						begin
							declare @c1 numeric(15,6)

							select @c1 = COUNT(ID)
							from #tmpAPSSchedule 
							where ID = @apsno

							select @maxeff = iif(@c1 = 0,0,SUM(EFFICIENCY)/@c1)
							from #tmpAPSSchedule 
							where ID = @apsno
						end
						ELSE
							SET @maxeff = @apseff

						delete from #ProdEff 

						SET @apseff = null
						
						--是否有設定LearningCurve，並取得學習曲線效率
						SET @cmd = '
						insert into #LnEff 
						Select CONVERT(numeric(24,10),Max(ld.Snvalue)) as lnEff 
						From ['+ @apsservername + '].'+@apsdatabasename+'.dbo.LnCurveApply l
							,['+ @apsservername + '].'+@apsdatabasename+'.dbo.LnCurveApplyDetail la
							,['+ @apsservername + '].'+@apsdatabasename+'.dbo.LnCurveDetail ld 
						where la.ProductionEventID = '+CONVERT(varchar(max),@apsno) + ' 
							  and l.ID = la.ApplyID 
							  and l.LnCurveTemplateID = ld.TemplateID'
						execute (@cmd)

						select @apseff = isnull((lnEff/100),0) 
						from #LnEff
						
						IF @apseff is not null and @apseff <> 0
							SET @maxeff = @maxeff*@apseff

						delete from #LnEff

						SET @apseff = null
						--是否有設定動態效率(動態效率=生產線效率)
						SET @cmd = '
						insert into #DynamicEff 
						Select TOP(1) 
							   fe.BeginDate
							   , isnull((fe.Efficiency/100),0) as Eff 
						From ['+ @apsservername + '].'+@apsdatabasename+'.dbo.FacilityEfficiency fe
							,['+ @apsservername + '].'+@apsdatabasename+'.dbo.Facility f
							,['+ @apsservername + '].'+@apsdatabasename+'.dbo.Factory fa 
						where fe.FacilityID = f.ID 
							  And f.FactoryID = fa.ID 
							  And fa.Code = '''+@factoryid + ''' 
							  and f.Name = ''' + @sewinglineid + ''' 
							  and fe.BeginDate <= ''' + CONVERT(char(19),@inline,120) + ''' 
							  and f.DYNAMICEFFICIENCY = 1
						Order by fe.BeginDate Desc'
						execute (@cmd)

						select @apseff = Eff 
						from #DynamicEff

						SET @SewLineEff=@apseff
						
						--取得生產計劃(ProductionEvent) 是設定哪一個學習曲線樣板(LnCurveTemplateID)--
						SET @LnCurveTemplateID = null
						SET @cmd = '
						INSERT INTO #LnCurveTemplate
						SELECT TOP 1  L.LNCURVETEMPLATEID
						FROM ['+ @apsservername + '].'+@apsdatabasename+'.dbo.LnCurveApply L
						INNER JOIN ['+ @apsservername + '].'+@apsdatabasename+'.dbo.LnCurveApplyDetail LD ON L.ID = LD.ApplyID
						WHERE LD.ProductionEventID='+CONVERT(varchar(max),@apsno) + '
						'
						execute (@cmd)

						select @LnCurveTemplateID = LNCURVETEMPLATEID 
						from #LnCurveTemplate

						IF @apseff is not null and @apseff <>0
						BEGIN	
							SET @maxeff = @maxeff*@apseff
						END

						delete from #DynamicEff
						delete from #LnCurveTemplate
						
						SET @sewer = iif (isnull(@sewer, 0) = 0, isnull ((select Sewer from SewingLine where FactoryID = @factoryid and ID = @sewinglineid), 0), @sewer)

						IF @gsd = 0
							set @set = 0
						ELSE
							set @set = (3600/@gsd)*@sewer*@maxeff
						Begin Try
							Begin Transaction
								
								update SewingSchedule 
								set SewingLineID = @sewinglineid
									, AlloQty = isnull(@alloqty,0)
									, Inline = @inline
									, Offline = @offline
									, Sewer = isnull(@sewer,0)
									, TotalSewingTime = isnull(@gsd,0)
									, MaxEff = CONVERT(numeric(5,2),isnull(@maxeff*100,0))
									, StandardOutput = IIF(@gsd is null or @gsd = 0 ,0,CONVERT(int,ROUND(@set,0)))
									, WorkDay = isnull((select COUNT(*) from WorkHour where SewingLineID = @sewinglineid and FactoryID = @factoryid and Date >= CONVERT(DATE,@inline) and Date <= CONVERT(DATE,@offline) and Hours > 0),0)
									, WorkHour = CONVERT(numeric(8,3),isnull(@duration,0))
									, EditName = @login
									, EditDate = @editdate
									, LearnCurveID = @LnCurveTemplateID
									, OriEff = CONVERT(numeric(5,2),isnull( @OriEff*100,100))
									, SewLineEff = CONVERT(numeric(5,2),isnull( @SewLineEff*100,100))
									, LNCSERIALNumber = @LNCSERIALNumber
									, SwitchTime = isnull(@SwitchTime,0)
								output	inserted.ID,
										'U' as [Action]
								into #tmpSewingScheduleGZ
								where ID = @sewingscheduleid;

								--更新SewingSchedule_Detail
								--刪除資料
								DECLARE cursor_deletescheduledetail CURSOR FOR
								select Article
									   , SizeCode 
								from SewingSchedule_Detail 
								where ID = @sewingscheduleid
								except
								select RTRIM(POCOLOR) as Article
									   , POSIZE as SizeCode 
								from #tmpAPSSchedule 
								where ID = @apsno 
									  and SALESORDERNO = @orderid 
									  and POID = @apspoid

								OPEN cursor_deletescheduledetail
								FETCH NEXT FROM cursor_deletescheduledetail 
								INTO @article,@sizecode

								WHILE @@FETCH_STATUS = 0
								BEGIN
									delete 
									from SewingSchedule_Detail 
									where ID = @sewingscheduleid 
										  and OrderID = @orderid 
										  and ComboType = @combotype 
										  and Article = @article 
										  and SizeCode = @sizecode;

									FETCH NEXT FROM cursor_deletescheduledetail 
									INTO @article,@sizecode
								END
								CLOSE cursor_deletescheduledetail
								DEALLOCATE cursor_deletescheduledetail

								--更新與新增資料
								DECLARE cursor_apsscheduledetail CURSOR FOR
								SELECT POCOLOR
									   , POSIZE
									   , PLANAMOUNT 
								from #tmpAPSSchedule 
								where ID = @apsno 
									  and SALESORDERNO = @orderid 
									  and POID = @apspoid

								OPEN cursor_apsscheduledetail
								FETCH NEXT FROM cursor_apsscheduledetail 
								INTO @article,@sizecode,@detailalloqty
								
								WHILE @@FETCH_STATUS = 0
								BEGIN
									IF EXISTS(select 1 from SewingSchedule_Detail where ID = @sewingscheduleid and OrderID = @orderid and ComboType = @combotype and Article = @article and SizeCode = @sizecode)
										update SewingSchedule_Detail 
										set SewingLineID = @sewinglineid
											, AlloQty = isnull(@detailalloqty,0) 
										where ID = @sewingscheduleid 
											  and OrderID = @orderid 
											  and ComboType = @combotype 
											  and Article = @article 
											  and SizeCode = @sizecode;
									ELSE
										insert into SewingSchedule_Detail 
										(ID, OrderID, ComboType,SewingLineID, Article,SizeCode, AlloQty)
										values 
										(@sewingscheduleid,@orderid,@combotype,@sewinglineid,@article,@sizecode,@detailalloqty);

									FETCH NEXT FROM cursor_apsscheduledetail 
									INTO @article,@sizecode,@detailalloqty
								END
								CLOSE cursor_apsscheduledetail
								DEALLOCATE cursor_apsscheduledetail
							Commit Transaction;
						End Try
						Begin Catch
							RollBack Transaction

							EXECUTE usp_GetErrorInfo;
						End Catch
					END
			END

		FETCH NEXT FROM cursor_sewingschedule 
		INTO @orderid,@combotype,@sewinglineid,@apsno,@inline,@offline,@gsd,@duration,@editdate,@alloqty,@apspoid ,@OriEff, @Sewer ,@LNCSERIALNumber,@SwitchTime

	END
	CLOSE cursor_sewingschedule
	DEALLOCATE cursor_sewingschedule
	drop table #tmpAPSSchedule;
	END
	

	--更新Orders的SewInline, SewOffline, SewLine
	BEGIN
	DECLARE cursor_orders CURSOR FOR
	select o.ID
		   , o.SewInLine
		   , o.SewOffLine
		   , o.SewLine
		   , CONVERT(date,(select MIN(Inline) from SewingSchedule where OrderID = o.ID)) as SewingInline
		   , CONVERT(date,(select MAX(Offline) from SewingSchedule where OrderID = o.ID)) as SewingOffline
		   , SewingLine = (select CONCAT(a.SewingLineID,'/') from (select distinct SewingLineID from SewingSchedule where OrderID = o.ID) a order by SewingLineID for xml path(''))
	from Orders o
	where o.FtyGroup = @factoryid
		  and o.Finished = 0

	DECLARE @orderinline date,
			@orderoffline date,
			@ordersewline varchar(60),
			@sewinginline date,
			@sewingoffline date,
			@sewline varchar(60) 

	OPEN cursor_orders
	FETCH NEXT FROM cursor_orders 
	INTO @orderid,@orderinline,@orderoffline,@ordersewline,@sewinginline,@sewingoffline,@sewline
	
	WHILE @@FETCH_STATUS = 0
	BEGIN
		Begin Try
			Begin Transaction
			IF isnull(@orderinline,'') <> isnull(@sewinginline,'') or isnull(@orderoffline,'') <> isnull(@sewingoffline,'') or isnull(@ordersewline,'') <> isnull(@sewline,'')
				update Orders 
				set SewInLine = @sewinginline
					, SewOffLine = @sewingoffline
					, SewLine = isnull(@sewline,'') 
				where ID = @orderid

			IF isnull(@orderinline,'') <> isnull(@sewinginline,'')
				insert into Order_History 
				(ID,HisType,OldValue,NewValue,Remark,AddName,AddDate)
				values 
				(@orderid,'SewInOffLine',IIF(@orderinline is null,'',CONVERT(char(8), @orderinline, 112)),IIF(@sewinginline is null,'',CONVERT(char(8), @sewinginline, 112)),'Sewing Inline Update',@login,GETDATE())
			
			IF isnull(@orderoffline,'') <> isnull(@sewingoffline,'')
				insert into Order_History 
				(ID,HisType,OldValue,NewValue,Remark,AddName,AddDate)
				values 
				(@orderid,'SewInOffLine',IIF(@orderoffline is null,'',CONVERT(char(8), @orderoffline, 112)),IIF(@sewingoffline is null,'',CONVERT(char(8), @sewingoffline, 112)),'Sewing Offline Update',@login,GETDATE())

			Commit Transaction;
		End Try
		Begin Catch
			RollBack Transaction

			EXECUTE usp_GetErrorInfo;
		End Catch
		FETCH NEXT FROM cursor_orders 
		INTO @orderid,@orderinline,@orderoffline,@ordersewline,@sewinginline,@sewingoffline,@sewline
	END
	CLOSE cursor_orders
	DEALLOCATE cursor_orders
	END

	--7708加上
	select id
	into #tmpID--要刪除的ID
	from SewingSchedule s1
		, (
			select s.FactoryID
				   , s.OrderID
				   , s.ComboType
				   , s.APSNo--,maxEditDate = max(s.EditDate)
			from SewingSchedule s
			group by s.FactoryID,s.OrderID,s.ComboType,s.APSNo 
			having count(s.OrderID)>1
		  ) s2
	where s1.FactoryID = s2.FactoryID 
		  and s1.OrderID = s2.OrderID 
		  and s1.ComboType = s2.ComboType 
		  and s1.APSNo = s2.APSNo
	except
	select id--重複中,要留下的id,EditDate有值則要留max,若重複則取一
	from(
		select id = max(s1.id)
		from SewingSchedule s1
			 , (
				select s.FactoryID
					   , s.OrderID
					   , s.ComboType
					   , s.APSNo
					   , maxEditDate = max(s.EditDate)
				from SewingSchedule s
				group by s.FactoryID,s.OrderID,s.ComboType,s.APSNo 
				having count(s.OrderID)>1
			   ) s2
		where s1.FactoryID = s2.FactoryID 
			  and s1.OrderID = s2.OrderID 
			  and s1.ComboType = s2.ComboType 
			  and s1.APSNo = s2.APSNo
			  and s1.EditDate = s2.maxEditDate
		group by s1.FactoryID,s1.OrderID,s1.ComboType,s1.APSNo
		union
		select id = max(s1.id)
		from SewingSchedule s1
			 , (
				select s.FactoryID
					   , s.OrderID
					   , s.ComboType
					   , s.APSNo
					   , maxEditDate = max(s.EditDate)
				from SewingSchedule s
				group by s.FactoryID,s.OrderID,s.ComboType,s.APSNo 
				having count(s.OrderID)>1 and max(s.EditDate) is null
			   ) s2
		where s1.FactoryID = s2.FactoryID 
			  and s1.OrderID = s2.OrderID 
			  and s1.ComboType = s2.ComboType 
			  and s1.APSNo = s2.APSNo
		group by s1.FactoryID,s1.OrderID,s1.ComboType,s1.APSNo
	)a

	delete SewingSchedule 
	output	deleted.ID,
			'D' as [Action]
	into #tmpSewingScheduleGZ
	where id in (select id from #tmpID)

	delete SewingSchedule_Detail 
	where id in (select id from #tmpID)

	select  s.ID,s.Junk from dbo.SewingLine s with (nolock) 
        where   exists (select 1 from #tmpSewingLineGZ t where t.Action in ('I','U') and s.ID = t.ID and s.FactoryID = t.FactoryID)

	select  s.ID,
	        s.OrderID,
	        s.SewingLineID,
	        s.Inline,
	        s.Offline,
	        [StdOutput] = iif(isnull(s.TotalSewingTime,0) = 0, 0, (3600*s.Sewer)/s.TotalSewingTime) 
	from    SewingSchedule s with (nolock)
	where   exists (select 1 from #tmpSewingScheduleGZ t where t.Action in ('I','U') and s.ID = t.ID)

	select ID
	from #tmpSewingScheduleGZ where Action = 'D'
END