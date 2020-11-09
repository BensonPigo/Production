

CREATE PROCEDURE [dbo].[imp_BundleTransfer]
@RFIDServerName varchar(50)='', @RFIDDatabaseName varchar(20)='', @RFIDloginId varchar(20)='', @RFIDLoginPwd varchar(20)='', @RFIDTable varchar(20)=''
AS
BEGIN
--若外部沒傳參數進來,則用system上的欄位資訊
	IF @RFIDServerName = ''
	BEGIN
		select 
			@RFIDServerName = RFIDServerName,
			@RFIDDatabaseName = RFIDDatabaseName,
			@RFIDloginId = RFIDloginId,
			@RFIDLoginPwd = RFIDLoginPwd,
			@RFIDTable = RFIDTable
		from system
	END

	--若不存在則新增連線
	BEGIN
		IF NOT EXISTS (SELECT * FROM sys.servers WHERE name = @RFIDServerName)
		BEGIN
			--新建連線
			EXEC master.dbo.sp_addlinkedserver @server = @RFIDServerName, @srvproduct=N'SQL Server'
			--設定連線登入資訊
			EXEC master.dbo.sp_addlinkedsrvlogin @rmtsrvname = @RFIDServerName, @locallogin = NULL , @useself = N'False', @rmtuser = @RFIDloginId, @rmtpassword = @RFIDLoginPwd
		END
	END

	--取得建立ARK server位置
	Declare @ARKServerName varchar(20)
	Declare @ARKDatabaseName varchar(20)
	Declare @ARKLoginId varchar(20)
	Declare @ARKLoginPwd varchar(20)

	select  @ARKServerName = ARKServerName,
			@ARKDatabaseName = ARKDatabaseName,
			@ARKLoginId = ARKLoginId,
			@ARKLoginPwd = ARKLoginPwd
	from system

	IF NOT EXISTS (SELECT 1 FROM sys.servers WHERE name = @ARKServerName)
	BEGIN
		--新建連線
		EXEC master.dbo.sp_addlinkedserver @server = @ARKServerName, @srvproduct=N'SQL Server'
		--設定連線登入資訊
		EXEC master.dbo.sp_addlinkedsrvlogin @rmtsrvname = @ARKServerName, @locallogin = NULL , @useself = N'False', @rmtuser = @ARKLoginId, @rmtpassword = @ARKLoginPwd
	END


	--Datas處理
	declare @Cmd1 varchar(max)
	declare @CmdARK varchar(max)
	declare @CmdARK2 varchar(max)
	declare @Cmd2 varchar(max)
	declare @Cmd3 varchar(max)
	declare @RaisError varchar(max)
	set xact_abort on;
	Set @Cmd1 = N'
	Begin Try
		Begin tran

		select  *  into #tmp from ['+@RFIDServerName+'].'+@RFIDDatabaseName+'.dbo.'+@RFIDTable+'

		declare @MaxSid bigint
		select @MaxSid = max(Sid) from  #tmp
		
			--add BundleTransfer	
			INSERT INTO BundleTransfer(Sid,RFIDReaderId,Type,SubProcessId,TagId,BundleNo,TransferDate,AddDate,LocationID,RFIDProcessLocationID,PanelNo,CutCellID)
			SELECT RB.sid, RB.readerid, RR.type, RRS.ProcessID, RB.tagid, RB.epcid, RB.transdate, GETDATE()
			,[LocationID]=isnull (RB.LocationID, ' + '''''' + '),isnull (RR.RFIDProcessLocationID, '''')
			,PanelNo = isnull(RB.PanelNo,'''')
			,CutCellID=isnull(RRp.CutCellID,'''')
			FROM  #tmp  RB
			left join RFIDReader RR on RB.readerid  collate Chinese_Taiwan_Stroke_CI_AS  =RR.Id
			left join RFIDReader_Panel RRP on RRP.RFIDReaderID = RR.ID 	and isnull(RB.PanelNo,'''')  collate Chinese_Taiwan_Stroke_CI_AS = isnull(RRP.PanelNo,'''')
			left join RFIDReader_SubProcess RRS on RRS.RFIDReaderID = RR.ID
			
			-- 先確認有哪些捆包號 + 工段 + Type，再到紀錄中找最新的資料
			select *
			into #disTmp
			from (
				select distinct
						tmp.EpcId
						, RRS.processId
						, rd.Type
						, rd.RFIDProcessLocationID
				from #tmp tmp				
				inner join RFIDReader rd on tmp.ReaderId collate Chinese_Taiwan_Stroke_CI_AS = rd.Id
				left join RFIDReader_SubProcess RRS on RRS.RFIDReaderID = rd.ID
				where len(tmp.Epcid) <= 10
			) disBundle
			outer apply (
				select top 1
						BundleNo = cast(tmp.EpcId as varchar(10))
						, SubProcessId = RRS.processId
						, TransDate = tmp.TransDate
						, AddDate = GetDate()
						, SewingLineID = isnull(rd.SewingLineID,'''')
						, [LocationID] = isnull (tmp.LocationID, ' + '''''' + ')
						, PanelNo=isnull(tmp.PanelNo,'''')
						, CutCellID=isnull(RRP.CutCellID,'''')
				from #tmp tmp
				inner join RFIDReader rd on tmp.ReaderId collate Chinese_Taiwan_Stroke_CI_AS = rd.Id
				left join RFIDReader_Panel RRP on RRP.RFIDReaderID = rd.ID and isnull(tmp.PanelNo,'''')  collate Chinese_Taiwan_Stroke_CI_AS = isnull(RRP.PanelNo,'''')
				left join RFIDReader_SubProcess RRS on RRS.RFIDReaderID = rd.ID
				where 	disBundle.EpcId = tmp.EpcId
						and disBundle.processId = RRS.processId
						and disBundle.Type = rd.Type
				order by tmp.TransDate Desc
			) getLastTrans 
			where exists(select 1 from Bundle_Detail bd where bd.BundleNo = getLastTrans.BundleNo)
			or  exists(select 1 from BundleReplacement_Detail bd where bd.BundleNo = getLastTrans.BundleNo)
			'


		--add Synchronize MiddlewareARK
		Set @CmdARK = N'
		select 
		distinct
		[Factory] = o.FtyGroup,
		o.BrandID,
		o.StyleID,
		o.SeasonID,
		[ComboType] = sl.Location,
		[WorkLine] = b.Sewinglineid,
		b.Orderid,
		s.StyleName,
		s.Description,
		o.Qty
		into #ToMiddleARK
		from #disTmp d
		inner join Bundle_Detail bd with (nolock) on d.BundleNo = bd.BundleNo
		inner join Bundle b with (nolock) on b.ID = bd.Id
		inner join orders o with (nolock) on b.Orderid = o.ID
		inner join Style_Location sl with (nolock) on o.StyleUkey = sl.StyleUkey
		inner join Style s with (nolock) on s.Ukey = o.StyleUkey
		
		union all
		select 
		distinct
		[Factory] = o.FtyGroup,
		o.BrandID,
		o.StyleID,
		o.SeasonID,
		[ComboType] = sl.Location,
		[WorkLine] = b.Sewinglineid,
		b.Orderid,
		s.StyleName,
		s.Description,
		o.Qty
		from #disTmp d
		inner join BundleReplacement_Detail bd with (nolock) on d.BundleNo = bd.BundleNo
		inner join BundleReplacement b with (nolock) on b.ID = bd.Id
		inner join orders o with (nolock) on b.Orderid = o.ID
		inner join Style_Location sl with (nolock) on o.StyleUkey = sl.StyleUkey
		inner join Style s with (nolock) on s.Ukey = o.StyleUkey
		
		
		--MiddlewareARK.WorkLineRFIDReaderLog
		select distinct 
		Factory,
		WorkLine,
		[Order_No] = Orderid,
		ComboType
		into #WorkLineRFIDReaderLog
		from #ToMiddleARK
		
		update t  set t.SCIUpdate = GetDate(),t.ARKUpdate = null
			from ['+@ARKServerName+'].'+@ARKDatabaseName+'.dbo.WorkLineRFIDReaderLog t
			where exists (select 1 from #WorkLineRFIDReaderLog s where 
											t.Factory = s.Factory collate Chinese_Taiwan_Stroke_CI_AS 
										and t.WorkLine = s.WorkLine collate Chinese_Taiwan_Stroke_CI_AS
										and t.Order_No = s.Order_No collate Chinese_Taiwan_Stroke_CI_AS
										and t.ComboType = s.ComboType collate Chinese_Taiwan_Stroke_CI_AS)
		
		insert into ['+@ARKServerName+'].'+@ARKDatabaseName+'.dbo.WorkLineRFIDReaderLog(Factory, WorkLine, Order_No, ComboType, SCIUpdate, ARKUpdate)
					select s.Factory, s.WorkLine, s.Order_No,s.ComboType, GetDate(), null
					from #WorkLineRFIDReaderLog s
					where not exists(select 1 from ['+@ARKServerName+'].'+@ARKDatabaseName+'.dbo.WorkLineRFIDReaderLog t
												where	t.Factory = s.Factory collate Chinese_Taiwan_Stroke_CI_AS 
														and t.WorkLine = s.WorkLine collate Chinese_Taiwan_Stroke_CI_AS
														and t.Order_No = s.Order_No collate Chinese_Taiwan_Stroke_CI_AS
														and t.ComboType = s.ComboType collate Chinese_Taiwan_Stroke_CI_AS)
		
		
		--MiddlewareARK.Product
		select
		distinct
		[Product_No] = BrandID + ''-'' + StyleID + ''-'' + SeasonID + ''-'' + ComboType,
		[product_name] = StyleName,
		[product_desc] = Description,
		[Brand] = BrandID,
		[Style] = StyleID,
		[Season] = SeasonID,
		ComboType
		into #Product
		from #ToMiddleARK
		
		update t set t.SCIUpdate = GetDate()
		from ['+@ARKServerName+'].'+@ARKDatabaseName+'.dbo.Product t
		where exists(select 1 from #Product s where 
							t.Brand = s.Brand collate Chinese_Taiwan_Stroke_CI_AS 
						and t.Style = s.Style collate Chinese_Taiwan_Stroke_CI_AS
						and t.Season = s.Season collate Chinese_Taiwan_Stroke_CI_AS
						and t.ComboType = s.ComboType collate Chinese_Taiwan_Stroke_CI_AS)
		
		insert into ['+@ARKServerName+'].'+@ARKDatabaseName+'.dbo.Product(product_no, product_name, product_desc, image_url, Brand, Style, Season, ComboType, SCIUpdate)
				select s.product_no, s.product_name, s.product_desc, null, Brand, Style, Season, ComboType, GetDate()
						from #Product s
						where not exists(select 1 from	['+@ARKServerName+'].'+@ARKDatabaseName+'.dbo.Product t
														where	t.Brand = s.Brand collate Chinese_Taiwan_Stroke_CI_AS 
															and t.Style = s.Style collate Chinese_Taiwan_Stroke_CI_AS
															and t.Season = s.Season collate Chinese_Taiwan_Stroke_CI_AS
															and t.ComboType = s.ComboType collate Chinese_Taiwan_Stroke_CI_AS
										)'
		
		Set @CmdARK2 = N'
		--MiddlewareARK.Orders
		select 
		[order_no] = ark.Orderid,
		ark.ComboType,
		[customer_name] = ark.BrandID,
		[product_id] = pd.product_id,
		[order_total] = ark.Qty
		into #ARKOrders
		from #ToMiddleARK ark
		inner join ['+@ARKServerName+'].'+@ARKDatabaseName+'.dbo.Product pd with (nolock) on	pd.Brand = ark.BrandID and 
																			pd.Style = ark.StyleID and 
																			pd.Season = ark.SeasonID and 
																			pd.ComboType = ark.ComboType
		
		update t set t.SCIUpdate = GetDate()
		from ['+@ARKServerName+'].'+@ARKDatabaseName+'.dbo.Orders t
			where exists(select 1 from  #ARKOrders s
									where	t.order_no = s.order_no collate Chinese_Taiwan_Stroke_CI_AS 
										and t.ComboType = s.ComboType collate Chinese_Taiwan_Stroke_CI_AS)
		
		
		insert	into ['+@ARKServerName+'].'+@ARKDatabaseName+'.dbo.Orders(order_no, ComboType, customer_name, product_id, order_total, place_time, delivery_date, unit_price, remarks, SCIUpdate)
				select s.order_no, s.ComboType, s.customer_name, s.product_id, s.order_total, null, null, 0, null, GetDate()
						from #ARKOrders s
						where not exists(select 1 from ['+@ARKServerName+'].'+@ARKDatabaseName+'.dbo.Orders t
														where	t.order_no = s.order_no collate Chinese_Taiwan_Stroke_CI_AS 
															and t.ComboType = s.ComboType collate Chinese_Taiwan_Stroke_CI_AS)

		'

			--add update BundleInOut			
			-- RFIDReader.Type=1
		set @Cmd2 =	N' 
			Declare @BundleNoTB Table(Change VARCHAR(20),BundleNo varchar(10)) -- 紀錄寫入/更新的BundleNo

			Merge Production.dbo.BundleInOut as t
			Using (
				select *
				from #disTmp
			) as s
			on t.BundleNo = s.BundleNo collate Chinese_Taiwan_Stroke_CI_AS 
				and t.SubprocessId = s.SubProcessId collate Chinese_Taiwan_Stroke_CI_AS
				and t.RFIDProcessLocationID = s.RFIDProcessLocationID collate Chinese_Taiwan_Stroke_CI_AS
				and s.type=1
			when matched then 
				update set
				t.incoming = s.TransDate, t.EditDate = s.AddDate, t.SewingLineID=s.SewingLineID, t.LocationID=s.LocationID, t.RFIDProcessLocationID = s.RFIDProcessLocationID,
				t.PanelNo = s.PanelNo,t.CutCellID=s.CutCellID
			when not matched by target and s.type=1 then
				insert(BundleNo, SubProcessId, InComing, AddDate, SewingLineID, LocationID, RFIDProcessLocationID,PanelNo,CutCellID)
				values(s.BundleNo, s.SubProcessId, s.TransDate,s.AddDate, s.SewingLineID, s.LocationID, s.RFIDProcessLocationID, s.PanelNo,s.CutCellID)
			OUTPUT $action,Inserted.BundleNo INTO @BundleNoTB;  

			-- RFIDReader.Type=2
			Merge Production.dbo.BundleInOut as t
			Using (
				select *
				from #disTmp
			) as s
			on t.BundleNo = s.BundleNo collate Chinese_Taiwan_Stroke_CI_AS 
				and t.SubprocessId = s.SubProcessId collate Chinese_Taiwan_Stroke_CI_AS 
				and t.RFIDProcessLocationID = s.RFIDProcessLocationID collate Chinese_Taiwan_Stroke_CI_AS
				and s.type=2 
			when matched then 
				update set
				t.OutGoing = s.TransDate, t.EditDate = s.AddDate, t.SewingLineID=s.SewingLineID, t.LocationID=s.LocationID, t.RFIDProcessLocationID = s.RFIDProcessLocationID,
				t.PanelNo = s.PanelNo,t.CutCellID=s.CutCellID
			when not matched by target and s.type=2 then
				insert(BundleNo, SubProcessId, OutGoing, AddDate, SewingLineID, LocationID, RFIDProcessLocationID,PanelNo,CutCellID)
				values(s.BundleNo, s.SubProcessId, s.TransDate, s.AddDate, s.SewingLineID, s.LocationID, s.RFIDProcessLocationID, s.PanelNo, s.CutCellID)
			OUTPUT $action,Inserted.BundleNo INTO @BundleNoTB;  

			-- RFIDReader.Type=3
			Merge Production.dbo.BundleInOut as t
			Using (
				select *
				from #disTmp
			) as s
			on t.BundleNo = s.BundleNo collate Chinese_Taiwan_Stroke_CI_AS 
				and t.SubprocessId = s.SubProcessId collate Chinese_Taiwan_Stroke_CI_AS 
				and t.RFIDProcessLocationID = s.RFIDProcessLocationID collate Chinese_Taiwan_Stroke_CI_AS
				and s.type=3
			when matched then 
				update set
				t.OutGoing = s.TransDate, t.EditDate = s.AddDate, t.SewingLineID=s.SewingLineID, t.LocationID=s.LocationID, t.RFIDProcessLocationID = s.RFIDProcessLocationID,
				t.PanelNo = s.PanelNo,t.CutCellID = s.CutCellID
			when not matched by target and s.type=3 then
				insert(BundleNo, SubProcessId, InComing, AddDate, SewingLineID, LocationID, RFIDProcessLocationID,PanelNo,CutCellID)
				values(s.BundleNo, s.SubProcessId, s.TransDate, s.AddDate, s.SewingLineID, s.LocationID, s.RFIDProcessLocationID, s.PanelNo, s.CutCellID)
			OUTPUT $action,Inserted.BundleNo INTO @BundleNoTB;  
			
			-- RFIDReader.Type=4
			Merge Production.dbo.BundleInOut as t
			Using (
				select *
				from #disTmp
			) as s
			on t.BundleNo = s.BundleNo collate Chinese_Taiwan_Stroke_CI_AS 
				and t.SubprocessId = s.SubProcessId collate Chinese_Taiwan_Stroke_CI_AS 
				and t.RFIDProcessLocationID = s.RFIDProcessLocationID collate Chinese_Taiwan_Stroke_CI_AS
				and s.type=4
			when matched then 
				update set
				t.OutGoing = s.TransDate, t.EditDate = s.AddDate, t.SewingLineID=s.SewingLineID, t.LocationID=s.LocationID, t.RFIDProcessLocationID = s.RFIDProcessLocationID,
				t.PanelNo = s.PanelNo,t.CutCellID = s.CutCellID
			when not matched by target and s.type=4 then
				insert(BundleNo, SubProcessId, InComing, AddDate, SewingLineID, LocationID, RFIDProcessLocationID,PanelNo,CutCellID)
				values(s.BundleNo, s.SubProcessId, s.TransDate, s.AddDate, s.SewingLineID, s.LocationID, s.RFIDProcessLocationID, s.PanelNo, s.CutCellID)
			OUTPUT $action,Inserted.BundleNo INTO @BundleNoTB; 
			'
	
	set @Cmd3 = '
			--更新ArtworkPO_Detail FarmIn準備資料
			SELECT FarmIn = SUM(LBD.Qty),LB.Orderid,LS.ArtworkTypeId,LBD.Patterncode,LBD.PatternDesc,LBD.SizeCode,LB.Article
			into #FarmIn_tmp
			from Bundle_Detail LBD with (nolock)
			INNER JOIN Bundle LB with (nolock) ON LB.ID = LBD.ID
			INNER JOIN BundleInOut LBIO with (nolock) ON LBIO.BundleNo= LBD.BundleNo
			INNER JOIN SubProcess LS with (nolock) ON LS.ID = LBIO.SubProcessId
			where LB.Orderid in(
				select distinct b.Orderid
				from Bundle_Detail bd with(nolock)
				inner join Bundle b with(nolock) on b.ID = bd.id
				where bd.BundleNo in (select distinct BundleNo from @BundleNoTB)
			)
			AND LBIO.InComing IS NOT NULL
			AND LBIO.RFIDProcessLocationID=''''
			group by LB.Orderid,LS.ArtworkTypeId,LBD.Patterncode,LBD.PatternDesc,LBD.SizeCode,LB.Article
			 
			--更新ArtworkPO_Detail FarmOut準備資料
			SELECT FarmOut = SUM(LBD.Qty),LB.Orderid,LS.ArtworkTypeId,LBD.Patterncode,LBD.PatternDesc,LBD.SizeCode,LB.Article
			into #FarmOut_tmp
			from Bundle_Detail LBD with (nolock)
			INNER JOIN Bundle LB with (nolock) ON LB.ID = LBD.ID
			INNER JOIN BundleInOut LBIO with (nolock) ON LBIO.BundleNo= LBD.BundleNo
			INNER JOIN SubProcess LS with (nolock) ON LS.ID = LBIO.SubProcessId
			where LB.Orderid in(
				select distinct b.Orderid
				from Bundle_Detail bd with(nolock)
				inner join Bundle b with(nolock) on b.ID = bd.id
				where bd.BundleNo in (select distinct BundleNo from @BundleNoTB)
			)
			AND LBIO.OutGoing IS NOT NULL
			AND LBIO.RFIDProcessLocationID=''''
			group by LB.Orderid,LS.ArtworkTypeId,LBD.Patterncode,LBD.PatternDesc,LBD.SizeCode,LB.Article

			select apd.Ukey,FarmIn = sum(t.FarmIn) 
			into #ArtworkPO_DetailFarmIn
			from ArtworkPO_Detail apd with (nolock)
			inner join #FarmIn_tmp t on apd.OrderID = t.Orderid and 
										apd.ArtworkTypeID = t.ArtworkTypeId and 
										apd.PatternCode = t.Patterncode and 
										apd.PatternDesc = t.PatternDesc and 
										(apd.Article = t.Article or apd.Article = '') and 
										(apd.SizeCode = t.SizeCode or apd.SizeCode = '')
			group by apd.Ukey

			select apd.Ukey,FarmOut = sum(t.FarmOut) 
			into #ArtworkPO_DetailFarmOut
			from ArtworkPO_Detail apd with (nolock)
			inner join #FarmOut_tmp t on apd.OrderID = t.Orderid and 
										apd.ArtworkTypeID = t.ArtworkTypeId and 
										apd.PatternCode = t.Patterncode and 
										apd.PatternDesc = t.PatternDesc and 
										(apd.Article = t.Article or apd.Article = '') and 
										(apd.SizeCode = t.SizeCode or apd.SizeCode = '')
			group by apd.Ukey

			update apd set FarmIn = apdo.FarmIn
			from ArtworkPO_Detail apd
			inner join #ArtworkPO_DetailFarmIn apdo on apd.Ukey = apdo.Ukey
			
			update apd set FarmOut = apdo.FarmOut
			from ArtworkPO_Detail apd
			inner join #ArtworkPO_DetailFarmOut apdo on apd.Ukey = apdo.Ukey

			Commit tran

			IF exists(select 1 from System where RFIDMiddlewareInRFIDServer=0)
				Begin
					DELETE '+@RFIDDatabaseName+'.dbo.'+@RFIDTable+'					
				End
			Else
				Begin	
					DELETE ['+@RFIDServerName+'].'+@RFIDDatabaseName+'.dbo.'+@RFIDTable+'	
					where sid <=@MaxSid
				End

	End Try
	Begin Catch
		IF @@TRANCOUNT > 0
		RollBack Tran

		DECLARE  @ErrorMessage  NVARCHAR(4000),  
				 @ErrorSeverity INT,    
				 @ErrorState    INT;
		SELECT     
			@ErrorMessage  = ERROR_MESSAGE(),    
			@ErrorSeverity = ERROR_SEVERITY(),   
			@ErrorState    = ERROR_STATE();

		RAISERROR (@ErrorMessage, -- Message text.    
					 @ErrorSeverity, -- Severity.    
					 @ErrorState -- State.    
				   ); 
	End Catch'

	EXEC(@Cmd1 + @CmdARK + @CmdARK2 + @Cmd2 + @Cmd3)

END
