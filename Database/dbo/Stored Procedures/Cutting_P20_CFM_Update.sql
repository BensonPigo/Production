-- =============================================
-- Author:		Aaron
-- Create date: 2018/02/21
-- Description: Cutting P20 confirm,unconfirm時針對CuttingOutput.ActGarment/ PPH/ ActTTCPU ,CuttingOutput_WIP等相關table作計算與更新
-- =============================================
CREATE PROCEDURE [dbo].[Cutting_P20_CFM_Update]
	-- Add the parameters for the stored procedure here
	@ID varchar(13)='',
	@Cdate date ='',
	@ManPower  numeric(3,0) = '',
	@ManHours  numeric(5,1) = '',
	@Run_type  varchar(10) = ''
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRY
		Select distinct d.*,e.Colorid,e.PatternPanel
            into #tmp1
            from 
            (Select b.POID,c.ID,c.Article,c.SizeCode,c.Qty from (Select distinct a.id,POID ,w.article
            from Orders a WITH (NOLOCK) ,CuttingOutput_Detail cu WITH (NOLOCK) ,WorkOrder_Distribute w WITH (NOLOCK)  where a.id = w.OrderID and cu.id= @ID
            and w.WorkOrderUkey = cu.WorkOrderUkey) as b,
            order_Qty c where c.id = b.id and b.Article = c.Article) d,Order_ColorCombo e,order_Eachcons cons
            where d.POID=e.id and d.Article = e.Article and e.FabricCode is not null and e.FabricCode !='' and cons.id =e.id and
			      d.poid = cons.id and cons.CuttingPiece='0' and  cons.FabricCombo = e.PatternPanel

            Select  b.orderid,b.Article,b.SizeCode,c.PatternPanel,isnull(sum(isnull(b.qty,0)),0) as cutqty ,isnull(sum(isnull(iif(ma.cdate <  @Cdate , b.qty,0),0)),0) as pre_cutqty 
            into #tmp2
            from CuttingOutput ma WITH (NOLOCK) ,CuttingOutput_Detail a WITH (NOLOCK) ,WorkOrder_Distribute b WITH (NOLOCK) , WorkOrder_PatternPanel c WITH (NOLOCK) , Orders O WITH (NOLOCK) 
            Where ma.cdate <= @Cdate and ma.ID = a.id and ma.Status!='New' 
            and a.WorkOrderUkey = b.WorkOrderUkey and a.WorkOrderUkey = c.WorkOrderUkey   and O.ID=b.OrderID
            and O.POID in (select CuttingID from CuttingOutput_Detail WITH (NOLOCK) where CuttingOutput_Detail.ID = @ID)
            group by b.orderid,b.Article,b.SizeCode,c.PatternPanel

            Select a.poid,a.id,a.article,a.sizecode,order_cpu = o.cpu,min(isnull(b.cutqty,0)) as cutqty ,cpu = o.cpu*min(isnull(b.cutqty,0)),min(isnull(b.pre_cutqty,0)) as pre_cutqty ,day_cpu = o.cpu*min(isnull(b.pre_cutqty,0))
			into #tmp3
            from #tmp1 a 
            left join #tmp2 b on a.id = b.orderid and a.Article = b.Article and a.PatternPanel = b.PatternPanel and a.SizeCode = b.SizeCode
            left join orders o WITH (NOLOCK) on o.id = a.id
            group by a.poid,a.id,a.article,a.sizecode ,o.cpu


		--update CuttingOutput.ActGarment/ PPH/ ActTTCPU
		IF(@Run_type = 'Confirm')
		BEGIN
			Declare @ActTTCPU numeric(10,3),@PPH numeric(8,2),@ActGarment int,@ncpu numeric

			select @ncpu = sum((a.cutqty - isnull(cw.Qty,0)) * a.order_cpu),@ActTTCPU =sum(a.cpu) - sum(a.day_cpu),@ActGarment = sum(a.cutqty)  - sum(a.pre_cutqty) 
			from #tmp3 a
			left join CuttingOutput_WIP cw WITH (NOLOCK) on cw.Orderid = a.id and cw.Article = a.Article and cw.Size = a.SizeCode


			IF(datalength(@ManPower) = 0 OR datalength(@ManHours) = 0)
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
		IF(@Run_type = 'Confirm')
		BEGIN
			MERGE CuttingOutput_WIP AS T
			USING #tmp3 AS S
			ON (T.Orderid = S.id and T.Article = S.Article and T.Size = S.SizeCode) 
			WHEN NOT MATCHED BY TARGET 
			    THEN INSERT(Orderid,Article,Size ,Qty) VALUES(S.id,  S.Article,  S.SizeCode  ,S.cutqty)
			WHEN MATCHED 
			    THEN UPDATE SET T.Qty = S.cutqty;
		END
		ELSE
		BEGIN
			--UnConfirm 更新不包含該單當天的資料
			MERGE CuttingOutput_WIP AS T
			USING #tmp3 AS S
			ON (T.Orderid = S.id and T.Article = S.Article and T.Size = S.SizeCode) 
			WHEN NOT MATCHED BY TARGET 
			    THEN INSERT(Orderid,Article,Size ,Qty) VALUES(S.id,  S.Article,  S.SizeCode  ,S.pre_cutqty)
			WHEN MATCHED 
			    THEN UPDATE SET T.Qty = S.cutqty;
		END
		


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
		WHERE CO.Status='Confirmed' and COD.CuttingID IN (SELECT CuttingID from CuttingOutput_Detail where id = @ID)
		group by COD.CuttingID
		)a,Cutting c
		where c.ID =a.CuttingID

	END TRY
	BEGIN CATCH
		EXEC usp_GetErrorInfo;
	END CATCH
END