-- =============================================
-- Author:		Aaron
-- Create date: 2018/02/21
-- Description: Cutting P20 confirm,unconfirm�ɰw��CuttingOutput.ActGarment/ PPH/ ActTTCPU ,CuttingOutput_WIP������table�@�p��P��s
-- =============================================
CREATE PROCEDURE [dbo].[Cutting_P20_CFM_Update]
	-- Add the parameters for the stored procedure here
	@ID varchar(13)='',
	@Cdate date ='',
	@ManPower  numeric(3,0) = 0,
	@ManHours  numeric(5,1) = 0,
	@Run_type  varchar(10) = ''
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRY
		--find all PatternPanel
/*
ISP20241140
   準備基準資料,回 P02 WorkOrderForPlanning 找到所有部位 PatternPanel
   EX: WorkOrderForPlanning, 有FA, FB, FC, FD,FE , 但 WorkOrderForOutput 只有 FA
*/
        SELECT DISTINCT wd.OrderID, wd.SizeCode, wd.Article, wopp.PatternPanel, w.MDivisionID
		INTO #tmp1
        FROM CuttingOutput_Detail cud WITH (NOLOCK)
        INNER JOIN WorkOrderForOutput w WITH (NOLOCK) ON cud.CuttingID = w.ID
        INNER JOIN WorkOrderForOutput_Distribute wd WITH (NOLOCK) ON w.Ukey = wd.WorkOrderForOutputUkey
        INNER JOIN WorkOrderForPlanning wop WITH (NOLOCK) ON w.ID = wop.ID
        INNER JOIN WorkOrderForPlanning_PatternPanel wopp WITH (NOLOCK) ON wop.Ukey = wopp.WorkOrderForPlanningUkey
        WHERE cud.id = @ID
        AND wd.OrderID <>'EXCESS'
		--
		If Object_ID('tempdb..#tmp2_A') Is Null
		Begin
			Create Table #tmp2_A
			(  
				 Orderid Varchar(16)
				, SizeCode VarChar(8)
				, Article VarChar(8)
				, PatternPanel VarChar(8)
				, MDivisionid VarChar(8)
				, cutQty int
			);
		End;	
	
		If Object_ID('tempdb..#tmp2_B') Is Null
		Begin
			Create Table #tmp2_B
			(  
				 Orderid Varchar(16)
				, SizeCode VarChar(8)
				, Article VarChar(8)
				, PatternPanel VarChar(8)
				, MDivisionid VarChar(8)
				, cutQty int
			);
		End;

		If Object_ID('tempdb..#tmp2_WIP_Qty') Is Null
		Begin
			Create Table #tmp2_WIP_Qty
			(  
				 Orderid Varchar(16)
				, SizeCode VarChar(8)
				, Article VarChar(8)
				, PatternPanel VarChar(8)
				, MDivisionid VarChar(8)
				, cutQty int
			);
		End;

        --從 P09 計算裁剪數量
        --CuttingOutput.cdate <= @Cdate 此 @ID 裁剪日<以>前已裁剪的數量
		insert into #tmp2_A
		exec CuttingP20calculateCutQty 0, @ID, @Cdate
		
        --CuttingOutput.cdate < @Cdate  此 @ID 裁剪日<之>前已裁剪的數量
		insert into #tmp2_B
		exec CuttingP20calculateCutQty 1, @ID, @Cdate
		
        --此 @ID 有的 POID 已完成的裁剪數量
		insert into #tmp2_WIP_Qty
		exec CuttingP20calculateCutQty 2, @ID, null

		select a.OrderID, a.SizeCode, a.Article, a.PatternPanel, a.MDivisionid,[cutqty] = c.cutQty,pre_cutqty=b.cutqty, WIP_Qty = a.cutqty
		into #tmp2
		from #tmp2_WIP_Qty a
		left join #tmp2_B b on a.Article=b.Article and a.MDivisionid=b.MDivisionid and a.OrderID = b.OrderID and a.PatternPanel = b.PatternPanel and a.SizeCode = b.SizeCode
		left join #tmp2_A c on a.Article=c.Article and a.MDivisionid=c.MDivisionid and a.OrderID = c.OrderID and a.PatternPanel = c.PatternPanel and a.SizeCode = c.SizeCode

        Select o.poid,a.orderid,a.article,a.sizecode,
			order_cpu = isnull(ot.Price, 0),
			cutqty = min(isnull(b.cutqty,0)),
			cpu = isnull(ot.Price, 0) * min(isnull(b.cutqty,0)),
			pre_cutqty = min(isnull(b.pre_cutqty,0)),
			pre_cpu = isnull(ot.Price, 0) * min(isnull(b.pre_cutqty,0)),
			WIP_Qty = min(isnull(b.WIP_Qty,0))
		into #tmp3
        from #tmp1 a -- #tmp1 是 P02 有的資訊
        left join #tmp2 b on a.orderid = b.orderid and a.Article = b.Article and a.PatternPanel = b.PatternPanel and a.SizeCode = b.SizeCode -- #tmp2 是 P09 有的資訊
        left join orders o WITH (NOLOCK) on o.id = a.orderid
		left join Order_TmsCost ot with(nolock) on ot.id = o.id and ot.ArtworkTypeID='Cutting'
        group by o.poid,a.orderid,a.article,a.sizecode, o.cpu,isnull(ot.Price, 0) 

		--update CuttingOutput.ActGarment/ PPH/ ActTTCPU
		IF(@Run_type = 'Confirm')
		BEGIN
			Declare @ActTTCPU numeric(10,3)
            Declare @PPH numeric(8,2)
			Declare @ActGarment int

			select
				@ActGarment = sum(a.cutqty) - sum(a.pre_cutqty) 
			from #tmp3 a
			
			--以下計算@ActTTCPU
			--找出相同WorkOrder.ID, Article, SizeCode (會有不在 CuttingOutput_Detail內的資料)
			select w.ID, w.Ukey, wd.Article, wd.SizeCode, w.FabricCombo,
				w.ConsPC
			into #tmpAllworkorder
			from WorkOrderForOutput w WITH (NOLOCK)
			inner join WorkOrderForOutput_Distribute wd WITH (NOLOCK) on wd.WorkOrderForOutputUkey = w.Ukey and wd.OrderID <> 'EXCESS' 
			where exists(
				--先找到 CuttingOutput_Detail 對應的 WorkOrder Article,SizeCode
				select 1
				from  CuttingOutput_Detail cod WITH (NOLOCK) 
				inner join WorkOrderForOutput_Distribute wd2 WITH (NOLOCK) on wd2.WorkOrderForOutputUkey = cod.WorkOrderForOutputUkey and wd.OrderID <> 'EXCESS'
				inner join WorkOrderForOutput w2 WITH (NOLOCK) on w2.Ukey = cod.WorkOrderForOutputUkey
				where cod.id = @ID
				and w2.id = w.id and wd2.Article = wd.Article and wd2.SizeCode = wd.SizeCode
			)

			--準備相同 t.FabricCombo, t.Article, t.SizeCode 的 ConsPC
			--取min是因相同 t.FabricCombo, t.Article, t.SizeCode 的 ConsPC, 有時會有0.0001的差距
			select t.id, t.FabricCombo, t.Article, t.SizeCode, ConsPC = min(t.ConsPC)
			into #tmpASF
			from #tmpAllworkorder t
			group by t.id, t.FabricCombo, t.Article, t.SizeCode

			--by ID,FabricCombo,Article,SizeCode 計算 ConsPC 佔比率
			select a.ID,a.FabricCombo,a.Article,a.SizeCode,
				 ConsRate = iif(isnull(x.TTLCons, 0) = 0, 0, (a.ConsPC / x.TTLCons))
			into #tmpConsRate
			from #tmpASF a
			inner join (
				select t.id, t.Article, t.SizeCode, TTLCons = sum(ConsPC)
				from #tmpASF t
				group by t.id, t.Article, t.SizeCode
			)x on x.ID = a.ID and x.Article = a.Article and x.SizeCode = a.SizeCode

			select
				--w.ID,--cod.CutRef,	
				--OutputQty = cod.Layer * ws.SizeRatio, -- 此筆實際裁剪數
				--w.ConsPC, -- 用 P09 的
				--ot.price, -- CPU/PC
				@ActTTCPU = ROUND(sum( (cod.Layer * ws.SizeRatio) * t.ConsRate * ot.price), 3) -- TotalCPU
			from  CuttingOutput_Detail cod WITH (NOLOCK) 
			inner join WorkOrderForOutput w WITH (NOLOCK) on w.Ukey = cod.WorkOrderForOutputUkey
			inner join WorkOrderForOutput_Distribute wd WITH (NOLOCK) on wd.WorkOrderForOutputUkey = cod.WorkOrderForOutputUkey and wd.OrderID <> 'EXCESS'
			inner join #tmpConsRate t on t.id = w.ID and t.FabricCombo = w.FabricCombo and t.Article = wd.Article and t.SizeCode = wd.SizeCode
			outer apply(select SizeRatio = sum(Qty) from WorkOrderForOutput_SizeRatio ws  WITH (NOLOCK) where ws.WorkOrderForOutputUkey = cod.WorkOrderForOutputUkey and ws.SizeCode = t.SizeCode)ws
			inner join Order_TmsCost ot WITH (NOLOCK) on ot.artworktypeid = 'CUTTING' and ot.id = w.ID-- 抓母單
			where cod.id = @ID

			drop table #tmpASF,#tmpAllworkorder,#tmpConsRate

			IF(@ManPower = 0 OR @ManHours = 0 )
			BEGIN
				SET @PPH = 0
			END
			ELSE
			BEGIN
			    SET @PPH = Round(@ActTTCPU / @ManPower / @ManHours, 2);
			END
		
			update CuttingOutput set ActTTCPU=@ActTTCPU, PPH=@PPH, ActGarment = @ActGarment where id = @ID;
		END
		ELSE
		BEGIN
			--UnConfirm CuttingOutput
			update CuttingOutput set ActTTCPU=0,ActGarment =0,PPH=0 where id = @ID;
		END

		--merge CuttingOutput_WIP 
		MERGE CuttingOutput_WIP AS T
			USING #tmp3 AS S
			ON (T.Orderid = S.orderid and T.Article = S.Article and T.Size = S.SizeCode) 
			WHEN NOT MATCHED BY TARGET 
			    THEN INSERT(Orderid,Article,Size ,Qty ,EditDate) VALUES(S.orderid,  S.Article,  S.SizeCode  ,S.WIP_Qty ,GetDate())
			WHEN MATCHED 
			    THEN UPDATE SET T.Qty = S.WIP_Qty, EditDate = GetDate();

		--update Cutting.FirstCutDate/ LastCutDate 
		update c
		set c.FirstCutDate = a.FirstCutDate
			,c.LastCutDate = a.LastCutDate
		from
		(
		select
		FirstCutDate = min(CO.cDate), LastCutDate = max(CO.cDate) ,COD.CuttingID
		FROM CuttingOutput_Detail COD
		LEFT JOIN CuttingOutput CO on CO.ID=COD.ID
		WHERE CO.Status <> 'New' and COD.CuttingID IN (SELECT CuttingID from CuttingOutput_Detail where id = @ID)
		group by COD.CuttingID
		)a,Cutting c
		where c.ID =a.CuttingID

	END TRY
	BEGIN CATCH
		EXEC usp_GetErrorInfo;
	END CATCH
END