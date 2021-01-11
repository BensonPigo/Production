
Create Procedure [dbo].[PowerBI_InventoryAnalysis]
	(  @_ExchangeType VarChar(2) = 'FX'
	 , @_ToCurrencyID VarChar(3) = 'USD'
	)
As
Begin
	Set NoCount On;
		
	If Exists(Select * From PBIReportData.sys.tables Where Name = 'InventoryAnalysis')
	Begin
		Drop Table PBIReportData.dbo.InventoryAnalysis;
	End;
	Create Table PBIReportData.dbo.InventoryAnalysis
		(RowNo				BigInt,
		 MrTeam				VarChar(5),
		 Brand				VarChar(8),
		 Stlye				VarChar(15),
		 Season				VarChar(10),
		 SeasonYear			VarChar(7),
		 POID				VarChar(13),
		 Seq1				VarChar(3),
		 Seq2				VarChar(2),
		 InvReason			VarChar(5),
		 InvReasonCNM		NVarChar(Max),
		 InvReasonENM		NVarChar(Max),
		 Program			VarChar(12),
		 Project			VarChar(30),
		 Price				Numeric(16, 4),
		 IsFoc				VarChar(15),
		 Qty				Numeric(10, 2),
		 ChargeStatus		VarChar(1),
		 ChargeStatusNM		VarChar(20),
		 Amount				Numeric(16, 2),
		 Amount_WithouFOC	Numeric(16, 2),
		 FOBCharge			Numeric(16, 2),
		 SuppGroup			VarChar(60),
		 Supplier			VarChar(60),
		 Currency			VarChar(3),
		 ToCurrency			VarChar(3),
		 MtlDesc			NVarChar(Max),
		 ETD				Date,
		 RefNo				VarChar(20),
		 SCIRefNo			VarChar(30),
		 FabricType			VarChar(20),
		 MaterialType		VarChar(20),
		 Color				NVarChar(Max),
		 SizeSpec			VarChar(15),
		 SizeUnit			VarChar(8),
		 Deadline			VarChar(10),
		 OrderSMR			NVarChar(70),
		 OrderHandle		NVarChar(70),
		 POSMR				NVarChar(70),
		 POHandle			NVarChar(70),
		 POSMRNM			NVarChar(50),
		 OrderSMRNM			NVarChar(50)
		)
	;

	Declare @ExchangeType VarChar(2) = @_ExchangeType;
	Declare @ToCurrencyID VarChar(3) = @_ToCurrencyID;
	
	Declare @ExecSQL NVarChar(MAX);
	Set @ExecSQL =
		'
		Insert into PBIReportData.dbo.InventoryAnalysis
			Select * From OpenQuery([TradeDB], 
			  ''Select RowNo			= Row_Number() Over(Partition by i.POID, i.Seq1, i.Seq2 Order by i.POID, i.Seq1, i.Seq2)
					 , MrTeam			= b.MrTeam
					 , Brand			= o.BrandID
					 , Stlye			= o.StyleID
					 , Season			= ss.SeasonSCIID
					 , SeasonYear		= ss.[Month]
					 , POID				= i.POID
					 , Seq1				= i.Seq1
					 , Seq2				= i.Seq2
					 , InvReason		= i.ReasonID		
					 , InvReasonCNM		= ir.ReasonCH
					 , InvReasonENM		= ir.ReasonEN
					 , Program			= o.ProgramID
					 , Project			= IIF(	o.ProjectID != ''''''''
											  , o.ProjectID
											  , Case o.Category 
													When ''''B'''' Then ''''Bulk''''
													When ''''S'''' Then ''''Sample''''
													When ''''M'''' Then ''''Material''''
													When ''''O'''' Then ''''Other''''
													When ''''T'''' Then ''''Sample Material''''
												Else
													o.Category
												End
											 )
					 , Price			= po3.Price
					 , IsFoc			= i.IsFOC
					 , Qty				= i.Qty
					 , ChargeStatus		= i.ChargeStatus
					 , ChargeStatusNM	= i.ChargeStatusNM
					 , Amount			= i.Amount
					 , Amount_WithouFOC	= i.Amount_WithouFOC
					 , FOBCharge		= 0
					 , SuppGroup		= sg.ID + ''''-'''' + sg.AbbEN
					 , Supplier			= s.ID + ''''-'''' + s.AbbEN
					 , Currency			= s.CurrencyID
					 , ToCurrency		= '''''+@ToCurrencyID+'''''
					 , MtlDesc			= mtlDesc.DescDetail
					 , ETD				= ship.ETD
					 , RefNo			= po3.RefNo
					 , SCIRefNo			= po3.SCIRefNo
					 , FabricType		= ft.Name
					 , MaterialType		= f.MtltypeId
					 , Color			= Concat(po3.ColorID,'''' '''', c.Name)
					 , SizeSpec			= po3.SizeSpec
					 , SizeUnit			= po3.SizeUnit
					 , Deadline			= Convert(VarChar(10), i.Deadline, 111)
					 , OrderSMR			= OrderSMR.IdAndNameAndExt
					 , OrderHandle		= OrderHandle.IdAndNameAndExt
					 , POSMR			= POSMR.IdAndNameAndExt
					 , POHandle			= POHandle.IdAndNameAndExt
					 , POSMRNM			= POSMR.NameOnly
					 , OrderSMRNM		= OrderSMR.NameOnly
				  From (Select i.POID, i.Seq1, i.Seq2
							 , i.ReasonID
							 , IsFoc = IIF(IsNull(ir.IsFOC, 0) = 1, ''''FOC'''', ''''Normal'''')
							 , Qty = Sum(i.Qty)
							 , ChargeStatus = IIF(i.Payable = ''''Y'''', ''''1'''', ''''2'''')
							 , ChargeStatusNM = IIF(i.Payable = ''''Y'''', ''''Chargeable'''', ''''Non Chargeable'''')
							 , Amount = Sum(amt.Amount)
							 , Amount_WithouFOC = Sum(IIF(IsNull(ir.IsFOC, 0) = 0, amt.Amount, 0))
							 , Deadline = Min(i.Deadline)
						  From Trade.dbo.Inventory as i
						 Inner Join Trade.dbo.InvtransReason as ir
							On ir.ID = i.ReasonID
						 Inner Join Trade.dbo.Orders as o
							On o.ID = i.POID
						 Inner Join Trade.dbo.PO_Supp as po2
							On po2.ID = i.POID And po2.Seq1 = i.Seq1
						 Inner Join Trade.dbo.PO_Supp_Detail as po3
							On po3.ID = i.POID And po3.Seq1 = i.Seq1 And po3.Seq2 = i.Seq2
						  Left Join Trade.dbo.Supp as s On s.ID = po2.SuppID
						  Left join Trade.dbo.Unit as u On u.ID = i.UnitID
						 Outer Apply (	Select Price = IIF(IsNull(ir.IsFOC, 0) = 1, 0, po3.Price)) as p
						 Outer Apply (	Select * From Trade.dbo.GetCurrencyRate('''''+@ExchangeType+''''', s.CurrencyID, '''''+@ToCurrencyID+''''', o.cfmDate) as currency) as cur
						 Outer Apply (	Select Amount = Round(IsNull(Trade.dbo.GetAmount((p.Price * cur.Rate), i.Qty, u.PriceRate, 1), 0), cur.Exact)) as amt
						 Group by i.POID, i.Seq1, i.Seq2, i.ReasonID, IsNull(ir.IsFOC, 0), IIF(i.Payable = ''''Y'''', ''''1'''', ''''2''''), IIF(i.Payable = ''''Y'''', ''''Chargeable'''', ''''Non Chargeable'''')
						Union All
						Select ds.InventoryPOID, ds.inventoryseq1, ds.inventoryseq2
							 , ReasonID = ''''''''
							 , IsFoc = ''''''''
							 , Qty = 0
							 , ChargeStatus = ''''3''''
							 , ChargeStatusNM = ''''Charged''''
							 , Amount = Sum(ds.debitamt * r.Rate)
							 , Amount_WithouFOC = Sum(ds.debitamt * r.Rate)
							 , Deadline = Null
						  From Trade.dbo.Debit_Stock as ds
						 Inner Join Trade.dbo.Debit as d On d.ID = ds.ID
						 Inner Join Trade.dbo.Orders as do on ds.OrderID = do.ID
						 Outer Apply (Select * From Trade.dbo.GetCurrencyRate('''''+@ExchangeType+''''', d.CurrencyID, '''''+@ToCurrencyID+''''', do.cfmdate)) as r
						 Where d.Status = ''''CONFIRMED''''
						   And Exists (Select 1 From Trade.dbo.Inventory as i Where i.POID = ds.InventoryPOID And i.Seq1 = ds.InventorySeq1 And i.Seq2 = ds.InventorySeq2 Having Sum(i.Qty) > 0)
						 Group by ds.InventoryPOID, ds.inventoryseq1, ds.inventoryseq2
					   ) as i
				 Inner Join Trade.dbo.Orders as o
					On o.ID = i.POID
				 Inner Join Trade.dbo.Brand as b
					On b.ID = o.BrandID
				 Inner Join Trade.dbo.Season as ss
					On ss.BrandID = o.BrandID And ss.ID = o.SeasonID
				 Inner Join Trade.dbo.SeasonSCI as sss
					On sss.ID = ss.SeasonSCIID
				 Inner Join Trade.dbo.PO as po
					On po.ID = i.POID
				 Inner Join Trade.dbo.PO_Supp as po2
					On po2.ID = i.POID And po2.Seq1 = i.Seq1
				 Inner Join Trade.dbo.PO_Supp_Detail as po3
					On po3.ID = i.POID And po3.Seq1 = i.Seq1 And po3.Seq2 = i.Seq2
				  Left Join Trade.dbo.Supp as s On s.ID = po2.SuppID
				  Left Join Trade.dbo.Supp as sg On sg.ID = s.SuppGroupFabric
				  Left Join Trade.dbo.Color as c On c.BrandId = o.BrandID And c.ID = po3.ColorID
				  Left Join Trade.dbo.Fabric as f On f.SCIRefno = po3.SCIRefNo
				  Left Join Trade.dbo.GetName as OrderSMR On OrderSMR.ID = o.SMR
				  Left Join Trade.dbo.GetName as OrderHandle On OrderHandle.ID = o.MRHandle
				  Left Join Trade.dbo.GetName as POSMR On POSMR.ID = po.POSMR
				  Left Join Trade.dbo.GetName as POHandle On POHandle.ID = po.POHandle
				  Left Join Trade.dbo.InvtransReason as ir On ir.ID = i.ReasonID
				  Left Join Trade.dbo.DropDownList as ft On ft.Type = ''''FabricType'''' And ft.ID = po3.FabricType
				 Outer Apply (Select ETD = Convert(VarChar(10), Ship.ETD, 111) From Trade.dbo.GetShip_Detail_MinETA(i.POID, i.seq1, i.seq2, ''''G'''') as ship) as ship
				 Outer Apply (Select DescDetail From Trade.dbo.GetDescMtl_ByPoDetail(i.POID, i.seq1, i.seq2) as mtlDesc) as mtlDesc
				 Order by b.MrTeam, o.BrandID, ss.SeasonSCIID, i.POID, i.SEQ1, i.SEQ2
			  '');
			';
	Exec (@ExecSQL);

End