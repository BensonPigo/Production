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
		Select distinct orderid = wd.OrderID,wd.SizeCode,wd.article,wp.PatternPanel,w.MDivisionID
		into #tmp1
		from WorkOrder w WITH (NOLOCK)
		inner join WorkOrder_Distribute wd WITH (NOLOCK) on wd.WorkOrderUkey = w.Ukey
		inner join WorkOrder_PatternPanel wp  WITH (NOLOCK) on wp.WorkOrderUkey = w.Ukey
		inner join CuttingOutput_Detail cud WITH (NOLOCK) on cud.CuttingID = w.ID
		where cud.id = @ID
		and wd.OrderID <>'EXCESS'
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

		insert into #tmp2_A
		exec CuttingP20calculateCutQty 0, @ID, @Cdate
		
		insert into #tmp2_B
		exec CuttingP20calculateCutQty 1, @ID, @Cdate
		
		insert into #tmp2_WIP_Qty
		exec CuttingP20calculateCutQty 2, @ID, null

		select a.OrderID, a.SizeCode, a.Article, a.PatternPanel, a.MDivisionid,[cutqty] = c.cutQty,pre_cutqty=b.cutqty, WIP_Qty = a.cutqty
		into #tmp2
		from #tmp2_WIP_Qty a
		left join #tmp2_B b on a.Article=b.Article and a.MDivisionid=b.MDivisionid and a.OrderID = b.OrderID and a.PatternPanel = b.PatternPanel and a.SizeCode = b.SizeCode
		left join #tmp2_A c on a.Article=c.Article and a.MDivisionid=c.MDivisionid and a.OrderID = c.OrderID and a.PatternPanel = c.PatternPanel and a.SizeCode = c.SizeCode

        Select o.poid,a.orderid,a.article,a.sizecode,
			order_cpu = o.cpu,
			cutqty = min(isnull(b.cutqty,0)),
			cpu = o.cpu*min(isnull(b.cutqty,0)),
			pre_cutqty = min(isnull(b.pre_cutqty,0)),
			pre_cpu = o.cpu*min(isnull(b.pre_cutqty,0)),
			WIP_Qty = min(isnull(b.WIP_Qty,0))
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
			--UnConfirm �����k0
			update CuttingOutput set ActTTCPU=0,ActGarment =0,PPH=0 where id = @ID;
		END

		--merge CuttingOutput_WIP 
		MERGE CuttingOutput_WIP AS T
			USING #tmp3 AS S
			ON (T.Orderid = S.orderid and T.Article = S.Article and T.Size = S.SizeCode) 
			WHEN NOT MATCHED BY TARGET 
			    THEN INSERT(Orderid,Article,Size ,Qty) VALUES(S.orderid,  S.Article,  S.SizeCode  ,S.WIP_Qty)
			WHEN MATCHED 
			    THEN UPDATE SET T.Qty = S.WIP_Qty;

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