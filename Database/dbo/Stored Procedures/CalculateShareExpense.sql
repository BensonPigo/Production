CREATE PROCEDURE [dbo].[CalculateShareExpense]
(
    @APID as varchar(13) = '',
    @login varchar(20) = ''
)
as 
BEGIN
	SET NOCOUNT ON;

	if len(@APID) = 0
	begin
		return
	end 

	if len(@login) = 0
	begin
		return
	end 

	DECLARE @ShippingAPID VARCHAR(13),
			@Type varchar(25)

	DECLARE  @id VARCHAR(25), -- InvNo, WK 取最高
			@shipmode VARCHAR(10),
			@blno VARCHAR(20),
			@gw NUMERIC(10, 3),
			@cbm NUMERIC(11,4),
			@currency VARCHAR(3),
			@subtype VARCHAR(25)

	DECLARE @ttlgw NUMERIC(10,3),
					@ttlcbm NUMERIC(10,3),
					@ttlcount INT,
					@accno VARCHAR(8), 
					@adddate DATETIME,
					@exact TINYINT,
					@CurrencyID VARCHAR(3)

	DECLARE @amount NUMERIC(15,4), 
					@wkno VARCHAR(13),
					@invno VARCHAR(25), 
					@shipmodeid VARCHAR(10),
					@sharebase VARCHAR(1),
					@count INT,
					@remainamount NUMERIC(15,4),
					@minusamount NUMERIC(15,4),
					@recno INT,
					@ftywk BIT,
					@inputamount NUMERIC(15,2),
					@maxblno VARCHAR(20),
					@maxwkno VARCHAR(13),
					@maxinvno VARCHAR(25),
					@maxdata NUMERIC(9,2),
					@1stsharebase VARCHAR(1)

	
		
	SET @adddate = GETDATE()			 

	DECLARE cursor_ShareExpense CURSOR FOR
	select distinct ShippingAPID, Type
	from ShareExpense
	where ShippingAPID = @APID

	/*
	 * 計算前先更新 ShareExpense 資料
	 */
	OPEN cursor_ShareExpense
	FETCH NEXT FROM cursor_ShareExpense INTO @ShippingAPID, @Type
	WHILE @@FETCH_STATUS = 0
	BEGIN
		IF @Type = 'IMPORT'
		BEGIN
			/*
			 * 將已不存在系統中的 WK Junk
			 */
			update s
			set s.Junk = 1				
				, s.EditName = @login
				, s.EditDate = @adddate
			from ShareExpense s
			where s.ShippingAPID = @ShippingAPID and s.WKNo != '' 
				  and s.WKNo not in (select ID from Export where ID = s.WKNo and ID is not null)
				  and s.WKNo not in (select ID from FtyExport where ID = s.WKNo and ID is not null)
		 
			/*
			 * 更新 WK 基本資料
			 */ 
			DECLARE cursor_allExport CURSOR FOR
				select  e.ID
						, e.ShipModeID
						, e.Blno
						, e.WeightKg
						, e.Cbm
						, s.CurrencyID
						, s.SubType
				from Export e WITH (NOLOCK) , ShareExpense se WITH (NOLOCK) , ShippingAP s WITH (NOLOCK) 
				where e.ID = se.WKNo
				and s.ID = se.ShippingAPID
				and s.ID = @ShippingAPID
			OPEN cursor_allExport
			FETCH NEXT FROM cursor_allExport INTO @id,@shipmode,@blno,@gw,@cbm,@currency,@subtype
			WHILE @@FETCH_STATUS = 0
			BEGIN
				update ShareExpense 
				set ShipModeID = @shipmode
					, BLNo = @blno
					, GW = @gw
					, CBM = @cbm
					, CurrencyID = @currency
					, Type = @subtype
				where ShippingAPID = @ShippingAPID and WKNo = @id

				FETCH NEXT FROM cursor_allExport INTO @id,@shipmode,@blno,@gw,@cbm,@currency,@subtype
			END
			CLOSE cursor_allExport
			DEALLOCATE cursor_allExport
		END
		ELSE
		BEGIN
			/*
			 * 將已不存在系統中的 Inv Junk
			 */
			update s
			set s.Junk = 1				
				, s.EditName = @login
				, s.EditDate = @adddate
			from ShareExpense s
			where s.ShippingAPID = @ShippingAPID 
					and s.InvNo != '' 
					and (
						s.InvNo not in (select ID from GMTBooking where ID = s.InvNo and ID is not null) 
						and s.InvNo not in (select INVNo from PackingList where INVNo = s.InvNo and INVNo is not null) 
						and s.InvNo not in (select ID from FtyExport  where ID = s.InvNo and ID is not null)
						and s.invno not in (select id from Export where id=s.InvNo and id is not null)
					)
			
			/*
			 * 更新 Inv 基本資料
			 */ 
			DECLARE cursor_GB CURSOR FOR
				select g.ID,g.ShipModeID,g.TotalGW,g.TotalCBM,s.CurrencyID,s.SubType, iif(g.BLNo is null or g.BLNo='', isnull (g.BL2No, ''), g.BLNo) as BLNo
				from GMTBooking g WITH (NOLOCK) , ShippingAP s WITH (NOLOCK) , ShareExpense se WITH (NOLOCK) 
				where g.ID = se.InvNo
						and s.id = se.ShippingAPID
						and se.FtyWK = 0
						and s.id = @ShippingAPID

			DECLARE cursor_FtyWK CURSOR FOR
				select f.ID,f.ShipModeID,f.WeightKg,f.Cbm,s.CurrencyID,s.SubType, f.Blno
				from FtyExport f WITH (NOLOCK) , ShippingAP s WITH (NOLOCK) , ShareExpense se WITH (NOLOCK) 
				where f.ID = se.InvNo
						and s.id = se.ShippingAPID
						and se.FtyWK = 1
						and s.id = @ShippingAPID

			DECLARE cursor_PackingList CURSOR FOR
				select p.ID,p.ShipModeID,p.GW,p.CBM,s.CurrencyID,s.SubType, '' as BLNo
				from PackingList p WITH (NOLOCK) , ShippingAP s WITH (NOLOCK) , ShareExpense se WITH (NOLOCK) 
				where p.ID = se.InvNo
						and (p.Type = 'F' or p.Type = 'L')
						and s.id = se.ShippingAPID
						and se.FtyWK = 0
						and s.id = @ShippingAPID

			OPEN cursor_GB
			FETCH NEXT FROM cursor_GB INTO @id,@shipmode,@gw,@cbm,@currency,@subtype,@blno
			WHILE @@FETCH_STATUS = 0
			BEGIN
				update ShareExpense 
				set ShipModeID = @shipmode
					, BLNo = @blno
					, GW = @gw
					, CBM = @cbm
					, CurrencyID = @currency
					, Type = @subtype
				where ShippingAPID = @ShippingAPID and InvNo = @id
				FETCH NEXT FROM cursor_GB INTO @id,@shipmode,@gw,@cbm,@currency,@subtype,@blno
			END
			CLOSE cursor_GB
			DEALLOCATE cursor_GB

			OPEN cursor_FtyWK
			FETCH NEXT FROM cursor_FtyWK INTO @id,@shipmode,@gw,@cbm,@currency,@subtype,@blno
			WHILE @@FETCH_STATUS = 0
			BEGIN
				update ShareExpense 
				set ShipModeID = @shipmode
					, BLNo = @blno
					, GW = @gw
					, CBM = @cbm
					, CurrencyID = @currency
					, Type = @subtype
				where ShippingAPID = @ShippingAPID and InvNo = @id
				FETCH NEXT FROM cursor_FtyWK INTO @id,@shipmode,@gw,@cbm,@currency,@subtype,@blno
			END
			CLOSE cursor_FtyWK
			DEALLOCATE cursor_FtyWK

			OPEN cursor_PackingList
			FETCH NEXT FROM cursor_PackingList INTO @id,@shipmode,@gw,@cbm,@currency,@subtype,@blno
			WHILE @@FETCH_STATUS = 0
			BEGIN
				update ShareExpense 
				set ShipModeID = @shipmode
					, BLNo = @blno
					, GW = @gw
					, CBM = @cbm
					, CurrencyID = @currency
					, Type = @subtype
				where ShippingAPID = @ShippingAPID and InvNo = @id

				FETCH NEXT FROM cursor_PackingList INTO @id,@shipmode,@gw,@cbm,@currency,@subtype,@blno
			END
			CLOSE cursor_PackingList
			DEALLOCATE cursor_PackingList
		END

		/*
		 * 將 ShareExpense_APP 資料全部 Junk
		 */ 
		 update s
		 set s.Junk = 1
			, s.EditName = @login
			, s.EditDate = @adddate
 		 from ShareExpense_APP s
		 where s.ShippingAPID = @ShippingAPID

 
		/* 
		 * 費用分攤 CalculateShareExpense
		 */
		BEGIN
			/*
			 * 設定變數值 
			 */
			set @CurrencyID=(select CurrencyID from ShippingAP where id = @ShippingAPID)
		
			SET @adddate = GETDATE()
			SELECT @ttlgw = isnull(sum(GW),0), @ttlcbm = isnull(sum(CBM),0), @ttlcount = isnull(count(ShippingAPID),0) 
			FROM (SELECT distinct ShippingAPID,BLNo,WKNo,InvNo,GW,CBM FROM ShareExpense WITH (NOLOCK) WHERE ShippingAPID = @ShippingAPID and junk = 0) a

			SELECT @exact = isnull(c.Exact,0) FROM ShippingAP s WITH (NOLOCK) , Currency c WITH (NOLOCK) WHERE s.ID = @ShippingAPID and c.ID = s.CurrencyID

			/*
			 * 找出欲分攤的對象
			 */
			select distinct BLNo,WKNo,InvNo,Type,GW,CBM,ShipModeID,FtyWK
			into #ShareInvWK
			from ShareExpense WITH (NOLOCK) 
			where ShippingAPID = @ShippingAPID and junk = 0

			/*
			 * 撈出須排除的會計科目
			 */
			DECLARE cursor_diffAccNo CURSOR FOR
			(
				-- 費用分攤的會科不存在 AP 可分攤的清單中 --
				select distinct AccountID
				from ShareExpense se WITH (NOLOCK) 
				where ShippingAPID = @ShippingAPID				
					  and dbo.GetAccountNoExpressType(se.AccountID,'Vat') = 0 
					  and dbo.GetAccountNoExpressType(se.AccountID,'SisFty') = 0
					  and not exists (
							select distinct se.AccountID
							from ShippingAP_Detail sd WITH (NOLOCK) 
							left join ShipExpense shipE WITH (NOLOCK) on shipE.ID = sd.ShipExpenseID
							where sd.ID = @ShippingAPID
								  and shipE.AccountID = se.AccountID								  
					  )
			)
			union
			(
				-- AP 不可分攤的會科清單 --
				select distinct se.AccountID
				from ShippingAP_Detail sd WITH (NOLOCK) 
				left join ShipExpense se WITH (NOLOCK) on se.ID = sd.ShipExpenseID
				where sd.ID = @ShippingAPID
						and (
							dbo.GetAccountNoExpressType(se.AccountID,'Vat') = 1 
							or dbo.GetAccountNoExpressType(se.AccountID,'SisFty') = 1
						)
			)

			-- 更新 ShareExpense 應該標註為 Junk 的歷史資料 --
			OPEN cursor_diffAccNo
			FETCH NEXT FROM cursor_diffAccNo INTO @accno
			WHILE @@FETCH_STATUS = 0
			BEGIN
				update ShareExpense 
				set Junk = 1					
					, EditName = @login
					, EditDate = @adddate
				where ShippingAPID = @ShippingAPID and AccountID = @accno
				FETCH NEXT FROM cursor_diffAccNo INTO @accno
			END
			CLOSE cursor_diffAccNo
			DEALLOCATE cursor_diffAccNo
		
			/*
			 * 撈出依會科加總的金額與要分攤的WK or GB
			 */
			DECLARE cursor_ttlAmount CURSOR FOR
				select a.*,isnull(isnull(sr.ShareBase,sr1.ShareBase),'') as ShareBase
				from (
					select a.AccountID
							, a.Amount
							, a.CurrencyID
							, b.BLNo
							, b.WKNo
							, b.InvNo
							, b.Type
							, b.GW
							, b.CBM
							, b.ShipModeID
							, b.FtyWK
					from (
						select isnull(se.AccountID,'') as AccountID, sum(sd.Amount) as Amount, s.CurrencyID
						from ShippingAP_Detail sd WITH (NOLOCK) 
						left join ShipExpense se WITH (NOLOCK) on se.ID = sd.ShipExpenseID
						left join SciFMS_AccountNo a on a.ID = se.AccountID
						left join ShippingAP s WITH (NOLOCK) on s.ID = sd.ID
						where sd.ID = @ShippingAPID
								and not (
									dbo.GetAccountNoExpressType(se.AccountID,'Vat') = 1 
									or dbo.GetAccountNoExpressType(se.AccountID,'SisFty') = 1
								)
						group by se.AccountID, a.Name, s.CurrencyID
					) a
					, ( 
						select BLNo,WKNo,InvNo,Type,GW,CBM,ShipModeID,FtyWK
						from #ShareInvWK
					) b
				) a
				left join ShareRule sr WITH (NOLOCK) on sr.AccountID = a.AccountID 
														and sr.ExpenseReason = a.Type 
														and (
															sr.ShipModeID = '' 
															or sr.ShipModeID like '%'+a.ShipModeID+'%'
														)
				left join ShareRule sr1 WITH (NOLOCK) on sr1.AccountID = left(a.AccountID,4) 
														 and sr1.ExpenseReason = a.Type 
														 and (
															sr1.ShipModeID = '' 
															or sr1.ShipModeID like '%'+a.ShipModeID+'%'
														 )
				order by a.AccountID,GW,CBM

			SET @count = 1
			SET @maxdata = 0
			OPEN cursor_ttlAmount
			FETCH NEXT FROM cursor_ttlAmount INTO @accno,@amount,@currency,@blno,@wkno,@invno,@type,@gw,@cbm,@shipmodeid,@ftywk,@sharebase
			WHILE @@FETCH_STATUS = 0
			BEGIN
				IF @count = 1
					BEGIN
						SET @remainamount = @amount
						SET @maxdata = 0
						SET @maxblno = @blno
						SET @maxwkno = @wkno
						SET @maxinvno = @invno
						SET @1stsharebase = @sharebase
						IF @1stsharebase = 'C'
							BEGIN
								SET @minusamount = iif (@ttlcbm = 0, 0, ROUND(@amount/@ttlcbm,4))
							END
						ELSE
							IF @1stsharebase = 'G'
								BEGIN
									SET @minusamount = iif (@ttlgw = 0, 0, ROUND(@amount/@ttlgw,4))
								END
							ELSE
								BEGIN
									SET @minusamount = iif (@ttlcount = 0, 0, ROUND(@amount/@ttlcount,4))
								END
					END
				ELSE
					BEGIN
						SET @remainamount = @remainamount - @inputamount
					END
	
				IF @1stsharebase = 'C'
					BEGIN
						SET @inputamount = ROUND((@minusamount * @cbm),@exact)
						IF @maxdata < @cbm
							BEGIN
								SET @maxblno = @blno
								SET @maxwkno = @wkno
								SET @maxinvno = @invno
							END
					END
				ELSE
					IF @1stsharebase = 'G'
						BEGIN
							SET @inputamount = ROUND((@minusamount * @gw),@exact)
							IF @maxdata < @gw
							BEGIN
								SET @maxblno = @blno
								SET @maxwkno = @wkno
								SET @maxinvno = @invno
							END
						END
					ELSE
						BEGIN
							SET @inputamount = ROUND(@minusamount,@exact)
						END

				select @recno = isnull(count(ShippingAPID),0) from ShareExpense WITH (NOLOCK) where ShippingAPID = @ShippingAPID and WKNo = @wkno and InvNo = @invno and AccountID = @accno
				IF @recno = 0
					BEGIN
						INSERT INTO ShareExpense
						(ShippingAPID,BLNo,WKNo,InvNo,Type,GW,CBM,CurrencyID,Amount,ShipModeID,ShareBase,FtyWK,AccountID,EditName,EditDate)
						VALUES 
						(@ShippingAPID, @blno, @wkno, @invno, @type, @gw, @cbm, @currency, @inputamount, @shipmodeid, @1stsharebase, @ftywk, @accno, @login, @adddate)
					END
				ELSE
					BEGIN
						UPDATE ShareExpense 
						SET CurrencyID = @currency
							, Amount = @inputamount
							, ShareBase = @1stsharebase
							, EditName = @login
							, EditDate = @adddate
							, Junk = 0
						where ShippingAPID = @ShippingAPID 
							  and WKNo = @wkno 
							  and InvNo = @invno 
							  and AccountID = @accno
					END

	
				IF @count = @ttlcount
					BEGIN	
						SET @count = 1
						SET @remainamount = @remainamount - @inputamount
						IF @remainamount <> 0
							BEGIN
								UPDATE ShareExpense 
								SET CurrencyID = @currency
									, Amount = Amount + @remainamount
									, EditName = @login
									, EditDate = @adddate
									, Junk = 0
								where ShippingAPID = @ShippingAPID 
									  and WKNo = @maxwkno 
									  and InvNo = @maxinvno 
									  and AccountID = @accno
							END
					END
				ELSE
					BEGIN
						SET @count = @count + 1
					END
				FETCH NEXT FROM cursor_ttlAmount INTO @accno,@amount,@currency,@blno,@wkno,@invno,@type,@gw,@cbm,@shipmodeid,@ftywk,@sharebase
			END
			CLOSE cursor_ttlAmount
			DEALLOCATE cursor_ttlAmount

			/*
			 * 分攤 AirPP			 
			 *   以下為Airpp 拆分Factory與Other部分
			 *   只有AirPP的資料需要再往下分攤
			 */
			select se.InvNo,se.AccountID,[Amount] = sum(se.Amount)
			into #InvNoSharedAmt
			from ShareExpense se with (nolock)
			where	se.ShippingAPID = @ShippingAPID 
					and se.Junk = 0 
					and	exists(
						select 1 from GMTBooking gmt with (nolock)
						inner join ShipMode sm with (nolock) on gmt.ShipModeID = sm.ID
						where gmt.ID = se.InvNo and sm.NeedCreateAPP = 1
					)
					and not (
						dbo.GetAccountNoExpressType(se.AccountID,'Vat') = 1 
						or dbo.GetAccountNoExpressType(se.AccountID,'SisFty') = 1
					)
			group by se.InvNo,se.AccountID

			select	t.InvNo,[PackID] = pl.ID,t.AccountID,t.Amount,[PLSharedAmt] = Round(t.Amount / SUM(pl.GW) over(PARTITION BY t.InvNo,t.AccountID) * pl.GW,2)
			into #PLSharedAmtStep1
			from #InvNoSharedAmt t
			inner join PackingList pl with (nolock) on pl.INVNo = t.InvNo

			select * ,[AccuPLSharedAmt] = SUM(PLSharedAmt) over(PARTITION BY InvNo,AccountID order BY InvNo,PackID,AccountID )
			into #PLSharedAmtStep2
			from #PLSharedAmtStep1

			select *,
				  [PLSharedAmtFin] = case	
											when count(1) over(partition by invno,AccountID ) = 1 then Amount
											when ROW_NUMBER() over(partition by invno,AccountID order BY InvNo,PackID,AccountID) < count(1) over(partition by invno,AccountID ) then PLSharedAmt
											else Amount -  LAG(AccuPLSharedAmt) over(partition by invno,AccountID order by invno,PackID,AccountID) 
									 end
			into #PLSharedAmt
			from #PLSharedAmtStep2

			select  t.InvNo,pld.ID,AirPPID=app.ID,t.AccountID,pld.OrderID,pld.OrderShipmodeSeq, t.PLSharedAmtFin
				, [TtlNW] = ROUND(sum(pld.NWPerPcs * pld.ShipQty),3)
				, [OrderSharedAmt] =iif(TtlNW.Value = 0,0,ROUND(t.PLSharedAmtFin / TtlNW.Value * sum(pld.NWPerPcs * pld.ShipQty),2))  
				, [QtyPerCTN] = sum(QtyPerCTN), [RatioFty] = isnull(app.RatioFty,0)		
			into #OrderSharedAmtStep1
			from #PLSharedAmt t
			inner join PackingList_Detail pld with (nolock) on t.PackID = pld.ID
			inner join AirPP app with (nolock) on pld.OrderID = app.OrderID 
												  and pld.OrderShipmodeSeq = app.OrderShipmodeSeq
												  and app.Status <> 'Junked'
			outer apply (
				select [Value] = isnull(sum(NWPerPcs * ShipQty),0) 
				from PackingList_Detail 
				where ID = t.PackID
			) TtlNW
			group by t.InvNo,pld.ID,app.ID,t.AccountID, pld.OrderID, pld.OrderShipmodeSeq, TtlNW.Value, t.PLSharedAmtFin, app.RatioFty

			select * ,[AccuOrderSharedAmt] = SUM(OrderSharedAmt) over(PARTITION BY ID,AccountID order BY AccountID,OrderID,OrderShipmodeSeq )
			into #OrderSharedAmtStep2
			from #OrderSharedAmtStep1

			select	*,
					[OrderSharedAmtFin] =  case	
												when OrderSharedAmt = 0 then 0
												when count(1) over(partition by ID,AccountID ) = 1 then PLSharedAmtFin
												when ROW_NUMBER() over(partition by ID,AccountID order BY AccountID,OrderID,OrderShipmodeSeq) < count(1) over(partition by ID,AccountID ) then OrderSharedAmt
												else PLSharedAmtFin -  LAG(AccuOrderSharedAmt) over(partition by ID,AccountID order by AccountID,OrderID,OrderShipmodeSeq) 
										   end
			into #OrderSharedAmt
			from #OrderSharedAmtStep2

			declare @SharedAmtFactory numeric (12, 2) 
			declare @SharedAmtOther numeric (12, 2) 

			select *,RatioOther=100-RatioFty,
				SharedAmtFactory=ROUND(OrderSharedAmtFin / 100 * RatioFty,2),
				SharedAmtOther=OrderSharedAmtFin - ROUND(OrderSharedAmtFin / 100 * RatioFty,2)
			into #source
			from #OrderSharedAmt

			merge ShareExpense_APP t
			using #source s
			on @ShippingAPID = t.ShippingAPID and s.InvNo=t.InvNo and s.ID = t.PackingListID and s.AirPPID = t.AirPPID and s.AccountID = t.AccountID
			when matched then update set 
				t.[CurrencyID]	  =@CurrencyID
				,t.[NW]			  =s.ttlNw
				,t.[RatioFty]	  =s.[RatioFty]
				,t.[AmtFty]		  =s.SharedAmtFactory
				,t.[RatioOther]	  =s.[RatioOther]
				,t.[AmtOther]	  =s.SharedAmtOther
				,t.[Junk]		  =0
				,t.[EditName]	  =@login
				,t.[EditDate]	  =@adddate
			when not matched by target then
			insert([ShippingAPID],[InvNo],[PackingListID],[AirPPID],[AccountID],[CurrencyID],[NW],[RatioFty],[AmtFty],[RatioOther],[AmtOther],[Junk], [EditName], [EditDate])
			VALUES(@ShippingAPID,s.[InvNo],s.id,s.[AirPPID],s.[AccountID],@CurrencyID,s.ttlNw,s.[RatioFty],s.SharedAmtFactory,s.[RatioOther],s.SharedAmtOther,0, @login, getdate())
			;

			select	@SharedAmtFactory = isnull(sum(ROUND(OrderSharedAmtFin / 100 * RatioFty,2)),0),
					@SharedAmtOther = isnull(sum(OrderSharedAmtFin - ROUND(OrderSharedAmtFin / 100 * RatioFty,2)),0)
			from #OrderSharedAmt

			update ShippingAP 
			set SharedAmtFactory = @SharedAmtFactory
				, SharedAmtOther = @SharedAmtOther
				, EditName = @login
				, EditDate = @adddate 
			where ID = @ShippingAPID
 

			drop table #InvNoSharedAmt,#PLSharedAmtStep1,#PLSharedAmtStep2,#PLSharedAmt,#OrderSharedAmtStep1,#OrderSharedAmtStep2,#OrderSharedAmt,#source
			--以上為Airpp 拆分Factory與Other部分

		END

		drop table #ShareInvWK
		FETCH NEXT FROM cursor_ShareExpense INTO @ShippingAPID, @Type
	END
	CLOSE cursor_ShareExpense
	DEALLOCATE cursor_ShareExpense
END