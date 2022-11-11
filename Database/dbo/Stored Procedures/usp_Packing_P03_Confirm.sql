-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	PackingList_P03 Confirmed
-- =============================================
CREATE PROCEDURE [dbo].[usp_Packing_P03_Confirm]
	@ID varchar(13)
	,@Factory varchar(8)
	,@User varchar(20)
	,@Confirm bit
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @msg nvarchar(500) ='';
	DECLARE @CTNQty int;
	DECLARE @ShipQty int;
	DECLARE @NW numeric(9,3);
	DECLARE @GW numeric(9,3);
	DECLARE @NNW numeric(9,3);
	DECLARE @CBM numeric(10,4);
	DECLARE @OtherCBM numeric(10,4);
	DECLARE @INVNo varchar(20);
	DECLARE @HasCancelOrder bit;

    -- Insert statements for procedure here
	WITH Packing as (
		SELECT SUM(CTNQty)CTNQty,SUM(ShipQty)ShipQty,SUM(NW)NW,SUM(GW)GW,SUM(NNW)NNW
		,RefNo
		FROM PackingList_Detail WHERE ID=@ID
		GROUP BY RefNo)
	SELECT cbm = a.CTNQty*b.CBM,a.*
	into #CBM_GW_ByRefno
	from Packing a
	inner join LocalItem b on a.RefNo=b.RefNo

	SELECT @HasCancelOrder=IIF(
	EXISTS 
	(
		SELECT pd.OrderID
		FROM PackingList_Detail pd
		INNER JOIN Orders o ON pd.OrderID=o.ID AND  o.Junk = 1
		WHERE pd.ID = @ID
	)  
	,1
	,0);

	select SUM(CTNQty)CTNQty,SUM(ShipQty)ShipQty,SUM(NW)NW,SUM(GW)GW,SUM(NNW)NNW,sum(cbm)cbm 
	into #Chk_CBM_GW
	from #CBM_GW_ByRefno
	
	SET @CTNQty = (select isnull(CTNQty,0) from #Chk_CBM_GW)
	SET @ShipQty = (select isnull(ShipQty,0) from #Chk_CBM_GW)
	SET @NW = (select isnull(NW,0) from #Chk_CBM_GW)
	SET @GW = (select isnull(GW,0) from #Chk_CBM_GW)
	SET @NNW = (select isnull(NNW,0) from #Chk_CBM_GW)
	SET @CBM = (select isnull(CBM,0) from #Chk_CBM_GW)

	----判斷是否存在Order_Qty
	--IF EXISTS(
	--	select a.id, a.Article, a.SizeCode
	--	from order_qty a
	--	inner join 
	--	(
	--		select OrderID
	--		from PackingList_Detail
	--		where id = @ID
	--		group by OrderID
	--	) c on a.ID = c.OrderID
	--	left join PackingList_Detail b on a.ID = b.OrderID and a.Article = b.Article and a.SizeCode = b.SizeCode
	--	where b.ID is null
	--		  and a.Qty > 0
	--)
	--BEGIN
	--	SET @msg += N'There is no [ColorWay] and [Size] in SP#.'			
	--END

	-- 表身重新計算後,再判斷CBM or GW 是不是0
	IF NOT EXISTS(SELECT * FROM #Chk_CBM_GW where cbm >0 and GW > 0)
	BEGIN
		SET @msg += N'Ttl CBM and Ttl GW cannot be empty!! '			
	END
	
	-- 訂單M別與登入系統M別不一致時，不可以Confirm
	IF EXISTS(select *
		from PackingList_Detail pd WITH (NOLOCK) , Orders o WITH (NOLOCK) 
		where pd.ID = @ID and pd.OrderID = o.ID and o.MDivisionID <> (select distinct MDivisionID from factory where id=@Factory))
	BEGIN
		SET @msg += N'SP# M not equal to login system M so cannot confirm! '		
	END

	-- 還沒有Invoice No就不可以做Confirm，有的話取得INVNo
	-- 表身只要存在任何一張 Cancel 訂單，即可跳過『是否已建立 GB』 的驗證步驟。
	IF EXISTS (SELECT * FROM PackingList WHERE ID=@ID AND (INVNo='' OR INVNo IS NULL)) AND @HasCancelOrder=0
	BEGIN 
		SET @msg += N'Shipping is not yet booking so cannot confirm!'
	END
	ELSE
	BEGIN
	   SET @INVNo = (SELECT DISTINCT INVNo FROM PackingList WHERE ID=@ID);
	END

	-- 檢查累計Pullout數不可超過訂單數量
	IF NOT EXISTS(select * 
		from PackingList_Detail pd WITH (NOLOCK) 
	outer apply(
		select isnull(sum(pad.ShipQty),0) as ShipQty
        from PackingList p WITH (NOLOCK), PackingList_Detail pad WITH (NOLOCK)
        where p.PulloutStatus != 'New'
        and p.PulloutID <> ''
        and p.id = pd.ID
        and pad.OrderID = pd.OrderID
        and pad.OrderShipmodeSeq = pd.OrderShipmodeSeq
        and pad.Article = pd.Article
        and pad.Sizecode = pd.Sizecode
	)PulloutQty
	outer apply	(
		select isnull(sum(iaq.DiffQty),0) as DiffQty
		 from InvAdjust ia WITH (NOLOCK) , InvAdjust_Qty iaq WITH (NOLOCK) 
		 where ia.OrderID = pd.OrderID
		 and ia.OrderShipmodeSeq = pd.OrderShipmodeSeq
		 and ia.ID = iaq.ID
		 and iaq.Article = pd.Article
		 and iaq.SizeCode = pd.SizeCode
	)InvadjQty
	outer apply(
		select isnull(oqd.Qty,0) as OrderQty
		,ShipQty = (PulloutQty.ShipQty)+(InvadjQty.DiffQty)
		from Order_QtyShip_Detail oqd WITH (NOLOCK) 
		where oqd.Id =  pd.OrderID
		and oqd.Seq = pd.OrderShipmodeSeq
		and oqd.Article =  pd.Article
		and oqd.SizeCode = pd.SizeCode
	)total
	where pd.ID = @ID
	and (total.OrderQty is not null and total.ShipQty is not null))
	BEGIN
		SET @msg += N'Pullout qty is more than order qty! '		
	END

	-- 檢查Sewing Output Qty是否有超過Packing Qty
	IF EXISTS (	select 
			poid.*
				,PackedData.PackedShipQty
				,PackingData.ShipQty 
				,InvadjQty.DiffQty
				,isnull(PackedData.PackedShipQty,0)+isnull(PackingData.ShipQty,0)+isnull(InvadjQty.DiffQty,0) TotalQty
				,SewingData.QAQty
			from (
			select 
				poid.OrderID
				,pl.ID
				,poid.SizeCode 
				,poid.Article 	
			from PackingList pl
			inner join PackingList_Detail poid on pl.ID = poid.ID 
			where pl.ID= @ID
			group by poid.OrderID,pl.ID,poid.SizeCode ,poid.Article
		)poid
		left join (
			select pld.OrderID,pld.Article,pld.SizeCode,sum(pld.ShipQty) as PackedShipQty
			from PackingList pl WITH (NOLOCK) 
			inner join PackingList_Detail pld WITH (NOLOCK) on pl.ID = pld.ID 
			where pl.Status = 'Confirmed'
			and pld.OrderID in (select distinct OrderID from  PackingList_Detail where ID = @ID)
			group by pld.OrderID,pld.Article,pld.SizeCode
		)PackedData on PackedData.OrderID=poid.OrderID and PackedData.Article=poid.Article and PackedData.SizeCode=poid.SizeCode 
		left join (
			select pld.OrderID,pld.Article,pld.SizeCode,sum(pld.ShipQty) as ShipQty
			from PackingList_Detail pld WITH (NOLOCK) 
			where pld.ID = @ID
			group by pld.OrderID,pld.Article,pld.SizeCode
		)PackingData on PackingData.OrderID=poid.OrderID and PackingData.Article=poid.Article and PackingData.SizeCode=poid.SizeCode  
		left join (
			select ia.OrderID,iaq.Article,iaq.SizeCode,sum(iaq.DiffQty) as DiffQty
			from InvAdjust ia WITH (NOLOCK)
			inner join InvAdjust_Qty iaq WITH (NOLOCK) on ia.ID = iaq.ID
			where ia.OrderID in (select distinct OrderID from  PackingList_Detail where ID = @ID)
			group by ia.OrderID,iaq.Article,iaq.SizeCode
		) InvadjQty on InvadjQty.OrderID=poid.OrderID and InvadjQty.Article=poid.Article and InvadjQty.SizeCode =poid.SizeCode
		inner join (
			select a.OrderID,a.Article,a.SizeCode,MIN(a.QAQty) as QAQty
			from (select poid.OrderID,oq.Article,oq.SizeCode, ol.Location, isnull(sum(sodd.QAQty),0) as QAQty
				 from (select distinct pld.OrderID
							 from PackingList pl WITH (NOLOCK) , PackingList_Detail pld WITH (NOLOCK) 
							 where pl.ID = @ID
							 and pld.ID = pl.ID) poid
			   left join Orders o WITH (NOLOCK) on o.ID = poid.OrderID
			   left join Order_Qty oq WITH (NOLOCK) on oq.ID = o.ID
			   left join Order_Location ol WITH (NOLOCK) on ol.OrderId = o.ID
			   left join SewingOutput_Detail_Detail sodd WITH (NOLOCK) on sodd.OrderId = o.ID and sodd.Article = oq.Article  and sodd.SizeCode = oq.SizeCode and sodd.ComboType = ol.Location
				 group by poid.OrderID,oq.Article,oq.SizeCode, ol.Location) a		
			group by a.OrderID,a.Article,a.SizeCode
		)SewingData on SewingData.OrderID=poid.OrderID and SewingData.Article=poid.Article and SewingData.SizeCode=poid.SizeCode
		where 1=1
		and poid.ID= @ID
		and isnull(PackedData.PackedShipQty,0)+isnull(PackingData.ShipQty,0)+isnull(InvadjQty.DiffQty,0)> isnull(SewingData.QAQty,0))
	BEGIN
		SET @msg += N'Pullout qty cannot exceed sewing qty! ';
	END

	-- 檢查表身的ShipMode與表頭的ShipMode如果不同就不可以SAVE
	IF EXISTS(select 1
		from PackingList_Detail t 
		inner join PackingList t1 on t.ID=t1.ID
		inner join Order_QtyShip o with (nolock) on t.OrderID = o.id and t.OrderShipmodeSeq = o.Seq
		where t1.ShipModeID<>o.ShipModeID
		and t.ID=@ID)
	BEGIN
		SET @msg += N'Packing List Ship Mode does not match Order Q''ty B''down. ';
	END

	-- 檢查表身SP是否為製造單，製造單不能confirm
	IF EXISTS (	select ot.id from OrderType ot 
	where exists (select o.id from orders o 
	where o.id IN (select distinct orderid from PackingList_Detail where id=@ID)
	and	o.BrandID = ot.BrandID and o.OrderTypeID = ot.ID) 
	and ot.IsGMTMaster = 1 )
	BEGIN 
		SET @msg += N'The GMT Master order cannot be confirmed!! ';
	END

	
	IF @msg='' and @Confirm=1
		BEGIN
		BEGIN TRY
		BEGIN TRANSACTION;
			update PackingList set Status = 'Confirmed', EditName = @User, EditDate = Getdate() where ID = @ID;
			update packinglist set
				CTNQty	= @CTNQty,
				ShipQty = @ShipQty,
				NW      = @NW,
				GW      = @GW,
				NNW     = @NNW,
				CBM     = @CBM
			where id = @ID;

			--取得該INVNo其他的CBM，用以加總，回寫至GMTBooking
			SET @OtherCBM =(SELECT [CBM]=ISNULL(SUM(CBM),0) FROM PackingList WITH (NOLOCK) WHERE INVNo = @INVNo AND ID != @ID);

			UPDATE GMTBooking SET 
				   TotalCBM = @CBM+@OtherCBM 
			WHERE ID = @INVNo;

			COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		IF XACT_STATE() <> 0 -- �D0��ܦ����
			ROLLBACK TRANSACTION;
		EXECUTE usp_GetErrorInfo;
	END CATCH;
		END
	ELSE
		select @msg msg;

	DROP TABLE #Chk_CBM_GW,#CBM_GW_ByRefno;
END