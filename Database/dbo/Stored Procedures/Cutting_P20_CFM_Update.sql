-- =============================================
-- Author:		Aaron
-- Create date: 2018/02/21
-- Description: Cutting P20 confirm,unconfirm時針對CuttingOutput.ActGarment/ PPH/ ActTTCPU ,CuttingOutput_WIP等相關table作計算與更新
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
		Select distinct orderid = o.ID,wd.SizeCode,wd.article,occ.PatternPanel,o.MDivisionID
		into #tmp1
		from Orders o WITH (NOLOCK)
		inner join WorkOrder_Distribute wd WITH (NOLOCK) on o.id = wd.OrderID
		inner join Order_ColorCombo occ on o.poid = occ.id and occ.Article = wd.Article
		inner join order_Eachcons cons on occ.id = cons.id and cons.FabricCombo = occ.PatternPanel and cons.CuttingPiece='0'
		inner join CuttingOutput_Detail cud WITH (NOLOCK) on cud.WorkOrderUkey = wd.WorkOrderUkey
		where occ.FabricCode !='' and occ.FabricCode is not null 
		and cud.id = @ID
		--
		select wd.OrderID,wd.SizeCode,wd.Article,wp.PatternPanel,
			cutqty= iif(sum(cod.Layer*ws.Qty)>wd.Qty,wd.Qty,sum(cod.Layer*ws.Qty)),
			pre_cutqty= iif(sum(iif(co.cdate < @Cdate,cod.Layer*ws.Qty,0)) >wd.Qty,-- 此單之前的單,裁的數量,排除當前單的數量
							wd.Qty,
							sum(iif(co.cdate < @Cdate,cod.Layer*ws.Qty,0))),
			co.MDivisionid
		into #tmp2
		from WorkOrder_Distribute wd WITH (NOLOCK)
		inner join WorkOrder_PatternPanel wp WITH (NOLOCK) on wp.WorkOrderUkey = wd.WorkOrderUkey
		inner join WorkOrder_SizeRatio ws WITH (NOLOCK) on ws.WorkOrderUkey = wd.WorkOrderUkey and ws.SizeCode = wd.SizeCode
		inner join CuttingOutput_Detail cod on cod.WorkOrderUkey = wd.WorkOrderUkey
		inner join CuttingOutput co WITH (NOLOCK) on co.id = cod.id and co.Status <> 'New'
		inner join orders o WITH (NOLOCK) on o.id = wd.OrderID
		where co.cdate <= @Cdate
		and O.POID in (select CuttingID from CuttingOutput_Detail WITH (NOLOCK) where CuttingOutput_Detail.ID = @ID)
		group by wd.OrderID,wd.SizeCode,wd.Article,wp.PatternPanel,co.MDivisionid,wd.Qty

        Select o.poid,a.orderid,a.article,a.sizecode,
			order_cpu = o.cpu,
			cutqty = min(isnull(b.cutqty,0)),
			cpu = o.cpu*min(isnull(b.cutqty,0)),
			pre_cutqty = min(isnull(b.pre_cutqty,0)),
			pre_cpu = o.cpu*min(isnull(b.pre_cutqty,0))
		into #tmp3
        from #tmp1 a 
        left join #tmp2 b on a.orderid = b.orderid and a.Article = b.Article and a.PatternPanel = b.PatternPanel and a.SizeCode = b.SizeCode
        left join orders o WITH (NOLOCK) on o.id = a.orderid
        group by o.poid,a.orderid,a.article,a.sizecode ,o.cpu

		--update CuttingOutput.ActGarment/ PPH/ ActTTCPU
		IF(@Run_type = 'Confirm')
		BEGIN
			Declare @ActTTCPU numeric(10,3),@PPH numeric(8,2),@ActGarment int,@ncpu numeric

			select @ncpu = sum((a.cutqty - isnull(cw.Qty,0)) * a.order_cpu),@ActTTCPU =sum(a.cpu) - sum(a.pre_cpu),@ActGarment = sum(a.cutqty)  - sum(a.pre_cutqty) 
			from #tmp3 a
			left join CuttingOutput_WIP cw WITH (NOLOCK) on cw.Orderid = a.orderid and cw.Article = a.Article and cw.Size = a.SizeCode


			IF(@ManPower = 0 OR @ManHours = 0 )
			BEGIN
				SET @PPH = 0
			END
			ELSE
			BEGIN
				IF (@ManHours > 0)
			    BEGIN
			       SET @PPH = Round(@ncpu / @ManPower / @ManHours, 2);
			    END
			    ELSE
				BEGIN
			        SET @PPH = 0
			    END
			END
		
			update CuttingOutput set ActTTCPU=@ActTTCPU,ActGarment =@ActGarment,PPH=@PPH where id = @ID;
		END
		ELSE
		BEGIN
			--UnConfirm 直接歸0
			update CuttingOutput set ActTTCPU=0,ActGarment =0,PPH=0 where id = @ID;
		END

		--merge CuttingOutput_WIP 
		MERGE CuttingOutput_WIP AS T
			USING #tmp3 AS S
			ON (T.Orderid = S.orderid and T.Article = S.Article and T.Size = S.SizeCode) 
			WHEN NOT MATCHED BY TARGET 
			    THEN INSERT(Orderid,Article,Size ,Qty) VALUES(S.orderid,  S.Article,  S.SizeCode  ,S.cutqty)
			WHEN MATCHED 
			    THEN UPDATE SET T.Qty = S.cutqty;

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