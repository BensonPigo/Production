

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
	SET @cmd = 'DECLARE cursor_sewingline CURSOR FOR SELECT Facility.GroupName, SUBSTRING(Facility.NAME,1,2) as Name, Facility.Description, Facility.Workernumber 
	FROM ['+ @apsservername + '].'+@apsdatabasename+'.dbo.Factory,['+ @apsservername + '].'+@apsdatabasename+'.dbo.Facility WHERE Factory.CODE = '''+ @factoryid + ''' and Facility.FactoryId = Factory.Id'
	
	Begin Try
		execute (@cmd)						
	End Try
	Begin Catch
		EXECUTE usp_GetErrorInfo;
	End Catch
	
	DECLARE @sewingcell varchar(2),
			@sewinglineid varchar(2),
			@description nvarchar(500),
			@sewer numeric(24,10),
			@set numeric(24,10)
	OPEN cursor_sewingline
	FETCH NEXT FROM cursor_sewingline INTO @sewingcell,@sewinglineid,@description,@sewer
	WHILE @@FETCH_STATUS = 0
	BEGIN
		declare @tmpcell varchar(2),
				@tmpdesc nvarchar(500),
				@tmpsewer int
		select @tmpcell = SewingCell,@tmpdesc = Description, @tmpsewer = Sewer from SewingLine where ID = @sewinglineid and FactoryID = @factoryid
		IF @tmpcell is not null
			BEGIN
				--當資料已存在PMS且值有改變就更新
				IF isnull(@tmpcell,'') <> isnull(@sewingcell,'') or isnull(@tmpdesc,'') <> isnull(@description,'') or isnull(@tmpsewer,'') <> isnull(@sewer,'')
					update SewingLine set SewingCell = isnull(@sewingcell,''), Description = isnull(@description,''), Sewer = isnull(@sewer,0), EditName = @login, EditDate = GETDATE() where ID = @sewinglineid and FactoryID = @factoryid;
			END
		ELSE
			BEGIN
				--當資料不存在PMS就新增資料
				insert into SewingLine(ID,Description,FactoryID,SewingCell,Sewer,AddName,AddDate) values (@sewinglineid,isnull(@description,''),@factoryid,isnull(@sewingcell,''),isnull(@sewer,0),@login, GETDATE());
			END
		FETCH NEXT FROM cursor_sewingline INTO @sewingcell,@sewinglineid,@description,@sewer
	END
	CLOSE cursor_sewingline
	DEALLOCATE cursor_sewingline

	--刪除PMS多的資料
	CREATE TABLE #tmpSewingLine (ID Varchar(2));
	SET @cmd = 'insert into #tmpSewingLine SELECT SUBSTRING(Facility.NAME,1,2) as ID FROM ['+ @apsservername + '].'+@apsdatabasename+'.dbo.Factory,['+ @apsservername + '].'+@apsdatabasename+'.dbo.Facility WHERE Factory.CODE = '''+ @factoryid + ''' and Facility.FactoryId = Factory.Id'
	execute (@cmd);

	DECLARE cursor_sewingline CURSOR FOR
	select ID from SewingLine where FactoryID = @factoryid
	except
	select ID from #tmpSewingLine;

	OPEN cursor_sewingline
	FETCH NEXT FROM cursor_sewingline INTO @sewinglineid
	WHILE @@FETCH_STATUS = 0
	BEGIN
		delete from SewingLine where ID = @sewinglineid and FactoryID = @factoryid;

		FETCH NEXT FROM cursor_sewingline INTO @sewinglineid
	END
	CLOSE cursor_sewingline
	DEALLOCATE cursor_sewingline
	drop table #tmpSewingLine;

	END

	--Holiday
	BEGIN
	--撈APS上Hoiday資料
	SET @cmd = 'DECLARE cursor_holiday CURSOR FOR SELECT Holiday.Name, Holiday.FromDate, DATEDIFF(DAY,Holiday.FromDate,Holiday.ToDate)+1 as DayDiff FROM ['+ @apsservername + '].'+@apsdatabasename+'.dbo.Factory,['+ @apsservername + '].'+@apsdatabasename+'.dbo.Holiday WHERE Factory.CODE = '''+ @factoryid + ''' and Holiday.FactoryId = Factory.Id and (Holiday.FromDate >= DATEADD(DAY,-10,GETDATE()) or Holiday.ToDate >= DATEADD(DAY,-10,GETDATE()))'
	execute (@cmd)
	DECLARE @holidayname nvarchar(20),
			@startdate date,
			@daydiff int

	OPEN cursor_holiday
	FETCH NEXT FROM cursor_holiday INTO @holidayname,@startdate,@daydiff
	WHILE @@FETCH_STATUS = 0
	BEGIN
		set @_i = 0
		WHILE (@_i < @daydiff)
		BEGIN
			declare @tmpname nvarchar(20)
			SET @tmpname = null   --初始化
			select @tmpname = Name from Holiday where FactoryID = @factoryid and HolidayDate = DATEADD(DAY,@_i,@startdate)
			IF @tmpname is not null
				BEGIN
					--當資料已存在PMS且值有改變就更新
					IF @tmpname <> @holidayname
						BEGIN
							Begin Try
								Begin Transaction
									--update Holiday set @tmpname = isnull(@holidayname,''), EditName = @login, EditDate = GETDATE() where FactoryID = @factoryid and HolidayDate = DATEADD(DAY,@_i,@startdate);
									update Holiday set Name = isnull(@holidayname,''), EditName = @login, EditDate = GETDATE() where FactoryID = @factoryid and HolidayDate = DATEADD(DAY,@_i,@startdate);

									--更新WorkHour
									--update WorkHour set Holiday = 1 where FactoryID = @factoryid and Date = DATEADD(DAY,@_i,@startdate);
									update WorkHour set Holiday = 0 where FactoryID = @factoryid and Date = DATEADD(DAY,@_i,@startdate);
								Commit Transaction;
							End Try
							Begin Catch
								RollBack Transaction

								EXECUTE usp_GetErrorInfo;
							End Catch
						END
				END
			ELSE
				BEGIN
					Begin Try
						Begin Transaction
							--當資料不存在PMS就新增資料
							insert into Holiday(FactoryID,HolidayDate,Name,AddName,AddDate) values (@factoryid,DATEADD(DAY,@_i,@startdate),@holidayname,@login, GETDATE());
							--更新WorkHour
							update WorkHour set Holiday = 1 where FactoryID = @factoryid and Date = DATEADD(DAY,@_i,@startdate);
						Commit Transaction;
					End Try
					Begin Catch
						RollBack Transaction

						EXECUTE usp_GetErrorInfo;
					End Catch
				END
			SET @_i = @_i + 1
		END
		FETCH NEXT FROM cursor_holiday INTO @holidayname,@startdate,@daydiff
	END
	CLOSE cursor_holiday
	DEALLOCATE cursor_holiday

	--刪除PMS多的資料
	CREATE TABLE #tmpHoliday (FromDate datetime, ToDate datetime);
	SET @cmd = 'insert into #tmpHoliday SELECT Holiday.FromDate,Holiday.ToDate FROM ['+ @apsservername + '].'+@apsdatabasename+'.dbo.Factory,['+ @apsservername + '].'+@apsdatabasename+'.dbo.Holiday WHERE Factory.CODE = '''+ @factoryid + ''' and Holiday.FactoryId = Factory.Id and (Holiday.FromDate >= DATEADD(DAY,-10,GETDATE()) or Holiday.ToDate >= DATEADD(DAY,-10,GETDATE()))'
	execute (@cmd)

	DECLARE cursor_holiday CURSOR FOR 
	select a.HolidayDate 
	from (select h.HolidayDate,IIF(t.FromDate is null,0,1) as Found from Holiday h
		  left join  #tmpHoliday t on h.HolidayDate between CONVERT(DATE,t.FromDate) and CONVERT(DATE,t.ToDate)
		  where h.FactoryID = @factoryid and h.HolidayDate >= CONVERT(DATE,DATEADD(DAY,-10,GETDATE()))) a
	where a.Found = 0;

	OPEN cursor_holiday
	FETCH NEXT FROM cursor_holiday INTO @startdate
	WHILE @@FETCH_STATUS = 0
	BEGIN
		Begin Try
			Begin Transaction
				delete from Holiday where FactoryID = @factoryid and HolidayDate = @startdate;
				--更新WorkHour
				update WorkHour set Holiday = 0 where FactoryID = @factoryid and Date = @startdate;
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
	CREATE TABLE #tmpFtyTemplate (NAME varchar(50),TEMPLATEID int,ENABLEDATE datetime);
	SET @cmd = 'insert into #tmpFtyTemplate select fa.NAME,w.TEMPLATEID,w.ENABLEDATE
	from ['+ @apsservername + '].'+@apsdatabasename+'.dbo.WorkCalendarApply w, ['+ @apsservername + '].'+@apsdatabasename+'.dbo.Factory f, ['+ @apsservername + '].'+@apsdatabasename+'.dbo.Facility fa
	where f.ID = w.TargetID
	and w.TargetType = 0
	and fa.FactoryID = f.ID
	and f.Code = '''+ @factoryid + '''
	order by fa.NAME,w.ENABLEDATE'
	execute (@cmd)

	CREATE TABLE #tmpFtyCalendar (TemplateID int,DayOfWeekIndex int,WorkHour numeric(24,10));
	SET @cmd = 'insert into #tmpFtyCalendar select TemplateID,DayOfWeekIndex,SUM(EndHour - StartHour) as WorkHour
	from ['+ @apsservername + '].'+@apsdatabasename+'.dbo.CalendarTemplateDetail
	group by TemplateID,DayOfWeekIndex
	order by TemplateID,DayOfWeekIndex'
	execute (@cmd)

	DECLARE cursor_sewingline CURSOR FOR select ID from SewingLine where FactoryID = @factoryid order by ID

	declare @workdate date,
			@apsworkhour numeric(24,10),
			@foundworkhour numeric(24,10),
			@templateid int

	OPEN cursor_sewingline
	FETCH NEXT FROM cursor_sewingline INTO @sewinglineid
	WHILE @@FETCH_STATUS = 0
	BEGIN
		set @_i = 0
		SET @workdate = GETDATE()
		WHILE (@_i < 160)
		BEGIN
			SET @workdate = DATEADD(DAY,1,@workdate)
			SET @apsworkhour = null
			SET @templateid = null
			--找工廠日曆的工時
			SET @templateid = (select TOP (1) TEMPLATEID from #tmpFtyTemplate where Name = @sewinglineid and EnableDate <= @workdate order by EnableDate desc)
			IF @templateid is not null
				BEGIN
					select @apsworkhour = WorkHour from #tmpFtyCalendar where TemplateID = @templateid and DayOfWeekindex = (DATEPART(WEEKDAY, @workdate)-1)
					IF @apsworkhour is not null
						BEGIN
							SET @foundworkhour = null
							select @foundworkhour = Hours from WorkHour where SewingLineID = @sewinglineid and FactoryID = @factoryid and Date = @workdate
							IF @foundworkhour is null
								insert into WorkHour(SewingLineID,FactoryID,Date,Hours,Holiday,AddName,AddDate)
								values (@sewinglineid,@factoryid,@workdate,@apsworkhour,(select COUNT(FactoryID) from Holiday where FactoryID = @factoryid and HolidayDate = @workdate),@login,GETDATE());
							ELSE
								BEGIN
									IF @foundworkhour <> @apsworkhour
										update WorkHour set Hours = isnull(@apsworkhour,0), EditName = @login, EditDate = GETDATE() where SewingLineID = @sewinglineid and FactoryID = @factoryid and Date = @workdate
								END
						END
					ELSE --檢查PMS是否有這筆資料，若有就把workhour改成0
						BEGIN
							SET @foundworkhour = null
							select @foundworkhour = Hours from WorkHour where FactoryID = @factoryid and SewingLineID = @sewinglineid and Date = @workdate and Hours > 0
							IF @foundworkhour is not null
								update WorkHour set Hours = 0, EditName = @login, EditDate = GETDATE() where SewingLineID = @sewinglineid and FactoryID = @factoryid and Date = @workdate
						END
				END
			ELSE  --檢查PMS是否有這筆資料，若有就把workhour改成0
				BEGIN
					SET @foundworkhour = null
					select @foundworkhour = Hours from WorkHour where FactoryID = @factoryid and SewingLineID = @sewinglineid and Date = @workdate and Hours > 0
					IF @foundworkhour is not null
						update WorkHour set Hours = 0, EditName = @login, EditDate = GETDATE() where SewingLineID = @sewinglineid and FactoryID = @factoryid and Date = @workdate
				END
			set @_i = @_i+1
		END

		FETCH NEXT FROM cursor_sewingline INTO @sewinglineid
	END
	CLOSE cursor_sewingline
	DEALLOCATE cursor_sewingline

	drop table #tmpFtyTemplate;

	--撈生產線日曆
	CREATE TABLE #tmpProdTemplate(Code varchar(50),NAME varchar(50),TEMPLATEID int,ENABLEDATE datetime,DAYOFWEEKINDEX int,WorkHour numeric(4,2))
	SET @cmd = 'insert into #tmpProdTemplate 
SELECT
E.Code, D.Name, A.TEMPLATEID, A.ENABLEDATE, C.DAYOFWEEKINDEX, WorkHour =Sum(C.ENDHOUR - C.STARTHOUR) 
FROM ['+ @apsservername + '].'+@apsdatabasename+'.dbo.WORKCALENDARAPPLY A
	,['+ @apsservername + '].'+@apsdatabasename+'.dbo.CALENDARTEMPLATE B
	,['+ @apsservername + '].'+@apsdatabasename+'.dbo.CALENDARTEMPLATEDETAIL C
	,['+ @apsservername + '].'+@apsdatabasename+'.dbo.Facility D
	,['+ @apsservername + '].'+@apsdatabasename+'.dbo.Factory E 
WHERE A.TEMPLATEID = B.ID AND B.ID = C.TEMPLATEID AND D.ID = A.TARGETID 
AND E.CODE ='''+ @factoryid + '''and D.FactoryId = E.Id AND A.TARGETTYPE = 1 
group by E.Code, D.Name, A.TEMPLATEID, A.ENABLEDATE, C.DAYOFWEEKINDEX
order by E.Code, D.Name,A.ENABLEDATE desc'
	execute (@cmd)

	DECLARE cursor_sewingline CURSOR FOR select distinct NAME from #tmpProdTemplate

	OPEN cursor_sewingline
	FETCH NEXT FROM cursor_sewingline INTO @sewinglineid
	WHILE @@FETCH_STATUS = 0
	BEGIN
		set @_i = 0
		SET @workdate = GETDATE()
		WHILE (@_i < 160)
		BEGIN
			SET @workdate = DATEADD(DAY,1,@workdate)
			SET @apsworkhour = null
			--找工廠日曆的工時
			select TOP(1) @apsworkhour = WorkHour from #tmpProdTemplate 
			where Name = @sewinglineid and EnableDate <= @workdate and DayOfWeekindex = (DATEPART(WEEKDAY, @workdate)-1) order by EnableDate desc
				IF @apsworkhour is not null
					BEGIN
						SET @foundworkhour = null
						select @foundworkhour = Hours from WorkHour where SewingLineID = @sewinglineid and FactoryID = @factoryid and Date = @workdate
						IF @foundworkhour is null
							insert into WorkHour(SewingLineID,FactoryID,Date,Hours,Holiday,AddName,AddDate)
							values (@sewinglineid,@factoryid,@workdate,@apsworkhour,(select COUNT(FactoryID) from Holiday where FactoryID = @factoryid and HolidayDate = @workdate),@login,GETDATE());
						ELSE
							BEGIN
								IF @foundworkhour <> @apsworkhour
									update WorkHour set Hours = isnull(@apsworkhour,0), EditName = @login, EditDate = GETDATE() where SewingLineID = @sewinglineid and FactoryID = @factoryid and Date = @workdate
							END
					END
				ELSE --檢查PMS是否有這筆資料，若有就把workhour改成0
					BEGIN
						SET @foundworkhour = null
						select @foundworkhour = Hours from WorkHour where FactoryID = @factoryid and SewingLineID = @sewinglineid and Date = @workdate and Hours > 0
						IF @foundworkhour is not null
							update WorkHour set Hours = 0, EditName = @login, EditDate = GETDATE() where SewingLineID = @sewinglineid and FactoryID = @factoryid and Date = @workdate
					END
			set @_i = @_i+1
		END

		FETCH NEXT FROM cursor_sewingline INTO @sewinglineid
	END
	CLOSE cursor_sewingline
	DEALLOCATE cursor_sewingline

	drop table #tmpFtyCalendar;
	drop table #tmpProdTemplate;

	--撈特殊時間
	SET @cmd = 'DECLARE cursor_apsspecialtime CURSOR FOR 
	select f.Name,s.SpecialType, s.SpecialDate, SUM(s.EndHour - s.StartHour) as WorkHour
	from ['+ @apsservername + '].'+@apsdatabasename+'.dbo.SpecialCalendar s, ['+ @apsservername + '].'+@apsdatabasename+'.dbo.Facility f, ['+ @apsservername + '].'+@apsdatabasename+'.dbo.Factory fa
	where fa.Code = '''+ @factoryid + '''
	and f.FactoryID = fa.ID
	and s.facilityID = f.ID
	and s.SpecialDate >= CONVERT(DATE,GETDATE())
	group by f.Name,s.SpecialType, s.SpecialDate'
	execute (@cmd)

	declare @specialtype int

	OPEN cursor_apsspecialtime
	FETCH NEXT FROM cursor_apsspecialtime INTO @sewinglineid,@specialtype,@workdate,@apsworkhour
	WHILE @@FETCH_STATUS = 0
	BEGIN
		SET @foundworkhour = null
		select @foundworkhour = Hours from WorkHour where FactoryID = @factoryid and SewingLineID = @sewinglineid and Date = @workdate

		IF @foundworkhour is null
			BEGIN
				IF @specialtype = 1
					insert into WorkHour(SewingLineID,FactoryID,Date,Hours,Holiday,AddName,AddDate)
					values (@sewinglineid,@factoryid,@workdate,@apsworkhour,(select COUNT(FactoryID) from Holiday where FactoryID = @factoryid and HolidayDate = @workdate),@login,GETDATE());
			END
		ELSE
			BEGIN
				IF @specialtype = 1
					update WorkHour set Hours = isnull(@apsworkhour,0), EditName = @login, EditDate = GETDATE() where SewingLineID = @sewinglineid and FactoryID = @factoryid and Date = @workdate
				ELSE
					BEGIN
						IF @foundworkhour > 0
							update WorkHour set Hours = 0, EditName = @login, EditDate = GETDATE() where SewingLineID = @sewinglineid and FactoryID = @factoryid and Date = @workdate
					END
			END
		FETCH NEXT FROM cursor_apsspecialtime INTO @sewinglineid,@specialtype,@workdate,@apsworkhour
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
				WHEN NOT MATCHED BY TARGET
					THEN INSERT(ID, Name, Description, AddName, AddDate) VALUES(S.ID, S.NAME, S.DESCRIPTION, '''+ @login + ''', getdate())
				WHEN MATCHED 
					THEN UPDATE SET T.Name = S.NAME,  T.Description = S.DESCRIPTION,  T.EditName = '''+ @login + ''', T.EditDate = getdate()
				WHEN NOT MATCHED BY SOURCE
					THEN DELETE;
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
				WHEN NOT MATCHED BY TARGET
					THEN INSERT(ID, Day, Efficiency) VALUES(S.TEMPLATEID, S.SERIALNUMBER, S.SNVALUE)
				WHEN MATCHED 
					THEN UPDATE SET T.Efficiency = S.SNVALUE
				WHEN NOT MATCHED BY SOURCE
					THEN DELETE;
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
	CREATE TABLE #tmpAPSSchedule (ID int,POID int,SALESORDERNO varchar(20),REFNO varchar(100),NAME varchar(50),STARTTIME datetime,ENDTIME datetime,UPDATEDATE datetime,POCOLOR char(60),POSIZE varchar(30),PLANAMOUNT numeric(24,10),DURATION numeric(24,10),CAPACITY numeric(24,10),EFFICIENCY numeric(24,10));
	SET @cmd = 'insert into #tmpAPSSchedule SELECT p.ID, pd.POID, po.SALESORDERNO, po.REFNO, fa.NAME, p.STARTTIME, p.ENDTIME, p.UPDATEDATE, pd.POCOLOR, pd.POSIZE, pd.PLANAMOUNT, p.DURATION, po.CAPACITY, pd.EFFICIENCY
	from ['+ @apsservername + '].'+@apsdatabasename+'.dbo.PRODUCTIONEVENT p, ['+ @apsservername + '].'+@apsdatabasename+'.dbo.Factory f, ['+ @apsservername + '].'+@apsdatabasename+'.dbo.Facility fa, ['+ @apsservername + '].'+@apsdatabasename+'.dbo.PO po, ['+ @apsservername + '].'+@apsdatabasename+'.dbo.PRODUCTIONEVENTDETAIL pd
	where fa.FACTORYID = f.ID 
	and p.FACILITYID = fa.ID 
	and po.ID = pd.POID 
	and pd.PRODUCTIONEVENTID = p.ID 
	and EXISTS (SELECT 1 from ['+ @apsservername + '].'+@apsdatabasename+'.dbo.PRODUCTIONEVENT aa, ['+ @apsservername + '].'+@apsdatabasename+'.dbo.PRODUCTIONEVENTDETAIL bb where aa.ID=bb.PRODUCTIONEVENTID and (CONVERT(DATE,aa.ENDTIME) >= CONVERT(DATE,DATEADD(DAY,-20,GETDATE())) OR CONVERT(DATE,aa.UPDATEDATE) >= CONVERT(DATE,DATEADD(DAY,-20,GETDATE()))) and bb.POID=po.ID)
	and f.Code = '''+ @factoryid + ''''
	execute (@cmd)

	--DECLARE cursor_sewingschedule CURSOR FOR
	select SALESORDERNO as OrderID,IIF(REFNO is null or REFNO = '', isnull((select TOP (1) sl.Location from Orders o, Style_Location sl where o.ID = SALESORDERNO and o.StyleUkey = sl.StyleUkey),''),REFNO) as ComboType,ID as APSNo into #tmpCheckData from #tmpAPSSchedule
	--刪除PMS存在但APS不存在的Schedule
	DECLARE cursor_needtodeletesewingschedule CURSOR FOR
	select APSNo,OrderID,ComboType from SewingSchedule where FactoryID = @factoryid and CONVERT(DATE,Offline) >= CONVERT(DATE,DATEADD(DAY,-20,GETDATE()))
	except
	select APSNo,OrderID,ComboType from #tmpCheckData

	declare @apsno int,
			@orderid varchar(13),
			@combotype varchar(1)

	OPEN cursor_needtodeletesewingschedule
	FETCH NEXT FROM cursor_needtodeletesewingschedule INTO @apsno,@orderid,@combotype
	WHILE @@FETCH_STATUS = 0
	BEGIN
		Begin Try
			Begin Transaction
				delete from SewingSchedule_Detail where ID in (select ID from SewingSchedule where FactoryID = @factoryid and APSNo = @apsno and OrderID = @orderid and ComboType = @combotype);
				delete from SewingSchedule where FactoryID = @factoryid and APSNo = @apsno and OrderID = @orderid and ComboType = @combotype;
			Commit Transaction;
		End Try
		Begin Catch
			RollBack Transaction

			EXECUTE usp_GetErrorInfo;
		End Catch
		FETCH NEXT FROM cursor_needtodeletesewingschedule INTO @apsno,@orderid,@combotype
	END
	CLOSE cursor_needtodeletesewingschedule
	DEALLOCATE cursor_needtodeletesewingschedule

	--將APS的Sewing Schedule更新進PMS
	DECLARE cursor_sewingschedule CURSOR FOR
	select SALESORDERNO,REFNO,NAME,ID,STARTTIME,ENDTIME,ROUND(CAPACITY*60,0) as GSD,DURATION,UPDATEDATE,SUM(PLANAMOUNT) as AlloQty,POID 
	from #tmpAPSSchedule
	group by SALESORDERNO,REFNO,NAME,ID,STARTTIME,ENDTIME,ROUND(CAPACITY*60,0),DURATION,UPDATEDATE,POID
	order by SALESORDERNO
	
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
			@apseff numeric(24,10),
			@article varchar(8),
			@sizecode varchar(8),
			@detailalloqty int


	CREATE TABLE #ProdEff (Efficiency numeric(24,10))
	CREATE TABLE #LnEff (lnEff numeric(24,10))
	CREATE TABLE #DynamicEff (BeginDate datetime,Eff numeric(24,10))

	OPEN cursor_sewingschedule
	FETCH NEXT FROM cursor_sewingschedule INTO @orderid,@combotype,@sewinglineid,@apsno,@inline,@offline,@gsd,@duration,@editdate,@alloqty,@apspoid
	WHILE @@FETCH_STATUS = 0
	BEGIN
		IF @combotype is null or @combotype = ''
			SET @combotype = isnull((select TOP (1) sl.Location from Orders o, Style_Location sl where o.ID = @orderid and o.StyleUkey = sl.StyleUkey),'')

		SET @sewingscheduleid = null
		select @sewingscheduleid = ID, @pmsassdate = AddDate, @pmseditdate = EditDate 
		from SewingSchedule where FactoryID = @factoryid and OrderID = @orderid and ComboType = @combotype and APSNo = @apsno
		IF @sewingscheduleid is null
			BEGIN
				--找生產單效率
				SET @cmd = 'insert into #ProdEff Select Efficiency From ['+ @apsservername + '].'+@apsdatabasename+'.dbo.ProductionEventCapacity where ProdEventID = '+CONVERT(varchar(max),@apsno) + ' And POID = '+CONVERT(varchar(max),@apspoid)
				execute (@cmd)

				select @apseff = Efficiency from #ProdEff
				
				IF @apseff is null
				begin
					declare @c numeric(15,6)
					select @c = COUNT(ID)from #tmpAPSSchedule where ID = @apsno

					select @maxeff = isnull(IIF(@c = 0,0,SUM(EFFICIENCY)/@c),0) 
					from #tmpAPSSchedule where ID = @apsno
				end
				ELSE
					SET @maxeff = @apseff

				delete from #ProdEff 

				SET @apseff = null
				--否有設定LearningCurve
				SET @cmd = 'insert into #LnEff Select Max(ld.Snvalue) as lnEff From ['+ @apsservername + '].'+@apsdatabasename+'.dbo.LnCurveApply l,['+ @apsservername + '].'+@apsdatabasename+'.dbo.LnCurveApplyDetail la,['+ @apsservername + '].'+@apsdatabasename+'.dbo.LnCurveDetail ld where la.ProductionEventID = '+CONVERT(varchar(max),@apsno) + ' and l.ID = la.ApplyID and l.LnCurveTemplateID = ld.TemplateID'
				execute (@cmd)
				select @apseff = isnull((lnEff/100),0) from #LnEff

				IF @apseff is not null and @apseff <> 0
					SET @maxeff = @maxeff*@apseff

				delete from #LnEff 

				SET @apseff = null
				--是否有設定動態效率
				SET @cmd = 'insert into #DynamicEff Select TOP(1) fe.BeginDate, isnull((fe.Efficiency/100),0) as Eff From ['+ @apsservername + '].'+@apsdatabasename+'.dbo.FacilityEfficiency fe,['+ @apsservername + '].'+@apsdatabasename+'.dbo.LnCurveApplyDetail la,['+ @apsservername + '].'+@apsdatabasename+'.dbo.Facility f,['+ @apsservername + '].'+@apsdatabasename+'.dbo.Factory fa where fe.FacilityID = f.ID And f.FactoryID = fa.ID And fa.Code = '''+@factoryid + ''' and f.Name = ''' + @sewinglineid + ''' and fe.BeginDate <= ''' + CONVERT(char(19),@inline,120) + ''' Order by fe.BeginDate Desc'
				execute (@cmd)
				select @apseff = Eff from #DynamicEff
				
				IF @apseff is not null and @apseff <> 0
					SET @maxeff = @maxeff*@apseff
				
				SET @sewer = isnull((select Sewer from SewingLine where FactoryID = @factoryid and ID = @sewinglineid),0)

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
						,AddDate)
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
						,@editdate);

						--取最新的ID
						select @sewingscheduleid = ID from SewingSchedule where FactoryID = @factoryid and OrderID = @orderid and ComboType = @combotype and APSNo = @apsno

						DECLARE cursor_apsscheduledetail CURSOR FOR
						SELECT POCOLOR, POSIZE, PLANAMOUNT from #tmpAPSSchedule where ID = @apsno and SALESORDERNO = @orderid and POID = @apspoid

						OPEN cursor_apsscheduledetail
						FETCH NEXT FROM cursor_apsscheduledetail INTO @article,@sizecode,@detailalloqty
						WHILE @@FETCH_STATUS = 0
						BEGIN
							insert into SewingSchedule_Detail (ID, OrderID, ComboType,SewingLineID, Article,SizeCode, AlloQty)
							values (@sewingscheduleid,@orderid,@combotype,@sewinglineid,@article,@sizecode,@detailalloqty);

							FETCH NEXT FROM cursor_apsscheduledetail INTO @article,@sizecode,@detailalloqty
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
						SET @cmd = 'insert into #ProdEff Select Efficiency From ['+ @apsservername + '].'+@apsdatabasename+'.dbo.ProductionEventCapacity where ProdEventID = '+CONVERT(varchar(max),@apsno) + ' And POID = '+CONVERT(varchar(max),@apspoid)
						execute (@cmd)

						select @apseff = Efficiency from #ProdEff
						
						IF @apseff is null
						begin
							declare @c1 numeric(15,6)
							select @c1 = COUNT(ID)from #tmpAPSSchedule where ID = @apsno

							select @maxeff = iif(@c1 = 0,0,SUM(EFFICIENCY)/@c1)
							from #tmpAPSSchedule where ID = @apsno
						end
						ELSE
							SET @maxeff = @apseff

						delete from #ProdEff 

						SET @apseff = null
						--否有設定LearningCurve
						SET @cmd = 'insert into #LnEff Select CONVERT(numeric(24,10),Max(ld.Snvalue)) as lnEff From ['+ @apsservername + '].'+@apsdatabasename+'.dbo.LnCurveApply l,['+ @apsservername + '].'+@apsdatabasename+'.dbo.LnCurveApplyDetail la,['+ @apsservername + '].'+@apsdatabasename+'.dbo.LnCurveDetail ld where la.ProductionEventID = '+CONVERT(varchar(max),@apsno) + ' and l.ID = la.ApplyID and l.LnCurveTemplateID = ld.TemplateID'
						execute (@cmd)
						select @apseff = isnull((lnEff/100),0) from #LnEff
						
						IF @apseff is not null and @apseff <>0
							SET @maxeff = @maxeff*@apseff

						delete from #LnEff

						SET @apseff = null
						--是否有設定動態效率
						SET @cmd = 'insert into #DynamicEff Select TOP(1) fe.BeginDate, isnull((fe.Efficiency/100),0) as Eff From ['+ @apsservername + '].'+@apsdatabasename+'.dbo.FacilityEfficiency fe,['+ @apsservername + '].'+@apsdatabasename+'.dbo.LnCurveApplyDetail la,['+ @apsservername + '].'+@apsdatabasename+'.dbo.Facility f,['+ @apsservername + '].'+@apsdatabasename+'.dbo.Factory fa where fe.FacilityID = f.ID And f.FactoryID = fa.ID And fa.Code = '''+@factoryid + ''' and f.Name = ''' + @sewinglineid + ''' and fe.BeginDate <= ''' + CONVERT(char(19),@inline,120) + ''' Order by fe.BeginDate Desc'
						execute (@cmd)
						select @apseff = Eff from #DynamicEff
						
						IF @apseff is not null and @apseff <>0
							SET @maxeff = @maxeff*@apseff

						delete from #DynamicEff
						
						IF @gsd = 0
						set @set = 0
						ELSE
						set @set = (3600/@gsd)*@sewer*@maxeff
						Begin Try
							Begin Transaction
								SET @sewer = isnull((select Sewer from SewingLine where FactoryID = @factoryid and ID = @sewinglineid),0)
								update SewingSchedule 
								set SewingLineID = @sewinglineid, AlloQty = isnull(@alloqty,0), Inline = @inline, Offline = @offline, Sewer = isnull(@sewer,0), 
									TotalSewingTime = isnull(@gsd,0), MaxEff = CONVERT(numeric(5,2),isnull(@maxeff*100,0)), StandardOutput = IIF(@gsd is null or @gsd = 0 ,0,CONVERT(int,ROUND(@set,0))), WorkDay = isnull((select COUNT(*) from WorkHour where SewingLineID = @sewinglineid and FactoryID = @factoryid and Date >= CONVERT(DATE,@inline) and Date <= CONVERT(DATE,@offline) and Hours > 0),0), 
									WorkHour = CONVERT(numeric(8,3),isnull(@duration,0)), EditName = @login, EditDate = @editdate
								where ID = @sewingscheduleid;

								--更新SewingSchedule_Detail
								--刪除資料
								DECLARE cursor_deletescheduledetail CURSOR FOR
								select Article,SizeCode from SewingSchedule_Detail where ID = @sewingscheduleid
								except
								select RTRIM(POCOLOR) as Article,POSIZE as SizeCode from #tmpAPSSchedule where ID = @apsno and SALESORDERNO = @orderid and POID = @apspoid

								OPEN cursor_deletescheduledetail
								FETCH NEXT FROM cursor_deletescheduledetail INTO @article,@sizecode
								WHILE @@FETCH_STATUS = 0
								BEGIN
									delete from SewingSchedule_Detail where ID = @sewingscheduleid and OrderID = @orderid and ComboType = @combotype and Article = @article and SizeCode = @sizecode;
									FETCH NEXT FROM cursor_deletescheduledetail INTO @article,@sizecode
								END
								CLOSE cursor_deletescheduledetail
								DEALLOCATE cursor_deletescheduledetail

								--更新與新增資料
								DECLARE cursor_apsscheduledetail CURSOR FOR
								SELECT POCOLOR, POSIZE, PLANAMOUNT from #tmpAPSSchedule where ID = @apsno and SALESORDERNO = @orderid and POID = @apspoid

								OPEN cursor_apsscheduledetail
								FETCH NEXT FROM cursor_apsscheduledetail INTO @article,@sizecode,@detailalloqty
								WHILE @@FETCH_STATUS = 0
								BEGIN
									IF EXISTS(select 1 from SewingSchedule_Detail where ID = @sewingscheduleid and OrderID = @orderid and ComboType = @combotype and Article = @article and SizeCode = @sizecode)
										update SewingSchedule_Detail set SewingLineID = @sewinglineid, AlloQty = isnull(@detailalloqty,0) where ID = @sewingscheduleid and OrderID = @orderid and ComboType = @combotype and Article = @article and SizeCode = @sizecode;
									ELSE
										insert into SewingSchedule_Detail (ID, OrderID, ComboType,SewingLineID, Article,SizeCode, AlloQty)
										values (@sewingscheduleid,@orderid,@combotype,@sewinglineid,@article,@sizecode,@detailalloqty);

									FETCH NEXT FROM cursor_apsscheduledetail INTO @article,@sizecode,@detailalloqty
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

		FETCH NEXT FROM cursor_sewingschedule INTO @orderid,@combotype,@sewinglineid,@apsno,@inline,@offline,@gsd,@duration,@editdate,@alloqty,@apspoid

	END
	CLOSE cursor_sewingschedule
	DEALLOCATE cursor_sewingschedule
	drop table #tmpAPSSchedule;
	END
	

	--更新Orders的SewInline, SewOffline, SewLine
	BEGIN
	DECLARE cursor_orders CURSOR FOR
	select o.ID,o.SewInLine,o.SewOffLine,o.SewLine,
	CONVERT(date,(select MIN(Inline) from SewingSchedule where OrderID = o.ID)) as SewingInline,
	CONVERT(date,(select MAX(Offline) from SewingSchedule where OrderID = o.ID)) as SewingOffline,
	(select CONCAT(a.SewingLineID,'/') from (select distinct SewingLineID from SewingSchedule where OrderID = o.ID) a order by SewingLineID for xml path('')) as SewingLine
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
	FETCH NEXT FROM cursor_orders INTO @orderid,@orderinline,@orderoffline,@ordersewline,@sewinginline,@sewingoffline,@sewline
	WHILE @@FETCH_STATUS = 0
	BEGIN
		Begin Try
			Begin Transaction
			IF isnull(@orderinline,'') <> isnull(@sewinginline,'') or isnull(@orderoffline,'') <> isnull(@sewingoffline,'') or isnull(@ordersewline,'') <> isnull(@sewline,'')
				update Orders set SewInLine = @sewinginline, SewOffLine = @sewingoffline, SewLine = isnull(@sewline,'') where ID = @orderid

			IF isnull(@orderinline,'') <> isnull(@sewinginline,'')
				insert into Order_History (ID,HisType,OldValue,NewValue,Remark,AddName,AddDate)
				values (@orderid,'SewInOffLine',IIF(@orderinline is null,'',CONVERT(char(8), @orderinline, 112)),IIF(@sewinginline is null,'',CONVERT(char(8), @sewinginline, 112)),'Sewing Inline Update',@login,GETDATE())
			
			IF isnull(@orderoffline,'') <> isnull(@sewingoffline,'')
				insert into Order_History (ID,HisType,OldValue,NewValue,Remark,AddName,AddDate)
				values (@orderid,'SewInOffLine',IIF(@orderoffline is null,'',CONVERT(char(8), @orderoffline, 112)),IIF(@sewingoffline is null,'',CONVERT(char(8), @sewingoffline, 112)),'Sewing Offline Update',@login,GETDATE())

			Commit Transaction;
		End Try
		Begin Catch
			RollBack Transaction

			EXECUTE usp_GetErrorInfo;
		End Catch
		FETCH NEXT FROM cursor_orders INTO @orderid,@orderinline,@orderoffline,@ordersewline,@sewinginline,@sewingoffline,@sewline
	END
	CLOSE cursor_orders
	DEALLOCATE cursor_orders
	END

	--7708加上
	select id
	into #tmpID--要刪除的ID
	from SewingSchedule s1,(
		select s.FactoryID,s.OrderID,s.ComboType,s.APSNo--,maxEditDate = max(s.EditDate)
		from SewingSchedule s
		group by s.FactoryID,s.OrderID,s.ComboType,s.APSNo having count(s.OrderID)>1
	)s2
	where s1.FactoryID = s2.FactoryID and s1.OrderID = s2.OrderID and s1.ComboType = s2.ComboType and s1.APSNo = s2.APSNo
	except
	select id--重複中,要留下的id,EditDate有值則要留max,若重複則取一
	from(
		select id = max(s1.id)
		from SewingSchedule s1,(
			select s.FactoryID,s.OrderID,s.ComboType,s.APSNo,maxEditDate = max(s.EditDate)
			from SewingSchedule s
			group by s.FactoryID,s.OrderID,s.ComboType,s.APSNo having count(s.OrderID)>1
		)s2
		where s1.FactoryID = s2.FactoryID and s1.OrderID = s2.OrderID and s1.ComboType = s2.ComboType and s1.APSNo = s2.APSNo
		and s1.EditDate = s2.maxEditDate
		group by s1.FactoryID,s1.OrderID,s1.ComboType,s1.APSNo
		union
		select id = max(s1.id)
		from SewingSchedule s1,(
			select s.FactoryID,s.OrderID,s.ComboType,s.APSNo,maxEditDate = max(s.EditDate)
			from SewingSchedule s
			group by s.FactoryID,s.OrderID,s.ComboType,s.APSNo having count(s.OrderID)>1 and max(s.EditDate) is null
		)s2
		where s1.FactoryID = s2.FactoryID and s1.OrderID = s2.OrderID and s1.ComboType = s2.ComboType and s1.APSNo = s2.APSNo
		group by s1.FactoryID,s1.OrderID,s1.ComboType,s1.APSNo
	)a

	delete SewingSchedule where id in(select id from #tmpID)
	delete SewingSchedule_Detail where id in(select id from #tmpID)
END