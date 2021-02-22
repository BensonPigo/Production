CREATE PROCEDURE [dbo].[usp_ReTransferMtlToScrap]
	@FtyInventoryUkey bigint
AS
begin
	select	fi.POID,
			fi.Ukey,
			fi.Seq1,
			fi.Seq2,
			fi.Roll,
			fi.Dyelot,
			[Factory] = (select FactoryID from Orders with (nolock) where ID=fi.POID),
			[Qty] = fi.InQty - fi.OutQty + fi.AdjustQty - fi.ReturnQty,
			[SubTransfer_DetailUkey] = sd.Ukey
	into #updSubTransfer_Detail
	from dbo.FtyInventory fi WITH (NOLOCK)
	left join SubTransfer_Detail sd with (nolock) on	fi.POID = sd.ID and 
														fi.Seq1 = sd.FromSeq1 and
														fi.Seq2 = sd.FromSeq2 and
														fi.Roll = sd.FromRoll and
														fi.Dyelot = sd.FromDyelot
	where fi.Ukey = @FtyInventoryUkey


	if exists (select 1 from #updSubTransfer_Detail where SubTransfer_DetailUkey is not null)
	begin
		update sd set sd.Qty = sd.Qty + usd.Qty
		from SubTransfer_Detail sd
		inner join #updSubTransfer_Detail usd on sd.ukey = usd.SubTransfer_DetailUkey
	end
	else
	begin
		INSERT INTO [dbo].[SubTransfer_Detail]
           ([ID]				,[FromFtyInventoryUkey]           ,[FromMDivisionID]           ,[FromFactoryID]
		   ,[FromPOID]
           ,[FromSeq1]           ,[FromSeq2]						,[FromRoll]					,[FromStockType]
           ,[FromDyelot]           
		   ,[ToMDivisionID]    ,[ToFactoryID]						,[ToPOID]					,[ToSeq1]
           ,[ToSeq2]           ,[ToRoll]							,[ToStockType]				,[ToDyelot]
           ,[Qty]			   ,[ToLocation])
		 SELECT [POID]
			,Ukey
			,'' [FromMDivisionID] 
			,Factory
			,[POID]
			,[Seq1]
			,[Seq2]
			,[Roll]
			,'B' [FromStock]
			,[Dyelot]
			,'' [ToMDivisionID]
			,Factory
			,[POID]
			,[Seq1]
			,[Seq2]
			,[Roll]
			,'O'	[ToStock]
			,[Dyelot]
			,Qty
			,[ToLocation]=( 
							SELECT TOP 1 ID 
							FROM MtlLocation 
							WHERE ID = dbo.Getlocation(SubTransfer_DetailUkey) AND (StockType='B'  OR StockType='O' ) AND Junk=0
							GROUP BY ID 
							HAVING Count(StockType)=2
						  )
			FROM #updSubTransfer_Detail
	end

end
