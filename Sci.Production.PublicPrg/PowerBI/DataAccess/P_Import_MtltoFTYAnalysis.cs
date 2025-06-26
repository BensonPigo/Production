using Sci.Data;
using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Sci.Production.Prg.PowerBI.DataAccess
{
    /// <inheritdoc/>
    public class P_Import_MtltoFTYAnalysis
    {
        private DBProxy DBProxy;

        /// <inheritdoc/>
        public Base_ViewModel P_IMtltoFTYAnalysis(ExecutedList item)
        {
            this.DBProxy = new DBProxy()
            {
                DefaultTimeout = 1800,
            };

            Base_ViewModel finalResult = new Base_ViewModel();

            if (!item.SDate.HasValue)
            {
                item.SDate = DateTime.Now.AddDays(-150);
            }

            try
            {
                Base_ViewModel resultReport = this.GetIMtltoFTYAnalysis_Data(item);
                if (!resultReport.Result)
                {
                    throw resultReport.Result.GetException();
                }

                DataTable dataTable = resultReport.Dt;

                // insert into PowerBI
                finalResult = this.UpdateBIData(dataTable, item);
                if (!finalResult.Result)
                {
                    throw finalResult.Result.GetException();
                }

                finalResult = new Base().UpdateBIData(item);
            }
            catch (Exception ex)
            {
                finalResult.Result = new Ict.DualResult(false, ex);
            }

            return finalResult;
        }

        private Base_ViewModel UpdateBIData(DataTable dt, ExecutedList item)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);

            List<SqlParameter> lisSqlParameter = new List<SqlParameter>
            {
                new SqlParameter("@SDate", item.SDate),
            };

            using (sqlConn)
            {
                string sql = $@"
				update p
					set p.Factory				= t.Factory
						, p.Country				= t.Country
						, p.Brand				= t.Brand
						, p.WeaveType			= t.WeaveType
						, p.ETD					= t.ETD
						, p.ETA					= t.ETA
						, p.CloseDate			= t.CloseDate
						, p.ActDate				= t.ActDate
						, p.Category			= t.Category
						, p.OrderCfmDate		= t.OrderCfmDate
						, p.SciDelivery			= t.SciDelivery
						, p.Refno				= t.Refno
						, p.SCIRefno			= t.SCIRefno
						, p.SuppID				= t.SuppID
						, p.SuppName			= t.SuppName
						, p.CurrencyID			= t.CurrencyID
						, p.CurrencyRate		= t.CurrencyRate
						, p.Price				= t.Price
						, p.[Price(TWD)]		= t.[PriceTWD]
						, p.Unit				= t.Unit
						, p.PoQty				= t.PoQty
						, p.PoFoc				= t.PoFoc
						, p.ShipQty				= t.ShipQty
						, p.ShipFoc				= t.ShipFoc
						, p.TTShipQty			= t.TTShipQty
						, p.[ShipAmt(TWD)]		= t.[ShipAmtTWD]
						, p.FabricJunk			= t.FabricJunk
						, p.ShipmentTerm		= t.ShipmentTerm
						, p.FabricType			= t.FabricType
						, p.PINO				= t.PINO
						, p.PIDATE				= t.PIDate
						, p.Color				= t.Color
						, p.ColorName			= t.ColorName
						, p.Season				= t.Season
						, p.PCHandle			= t.PCHandle
						, p.POHandle			= t.POHandle
						, p.POSMR				= t.POSMR
						, p.Style				= t.Style
						, p.OrderType			= t.OrderType
						, p.ShipModeID			= t.ShipModeID
						, p.Supp1stCfmDate		= t.Supp1stCfmDate
						, p.BrandSuppCode		= t.BrandSuppCode
						, p.BrandSuppName		= t.BrandSuppName
						, p.CountryofLoading	= t.CountryofLoading
						, p.SupdelRvsd			= t.SupdelRvsd
						, p.ProdItem			= t.ProdItem
						, p.KPILETA				= t.KPILETA
						, p.MaterialConfirm		= t.MaterialConfirm
						, p.SupplierGroup		= t.SupplierGroup
						, p.TransferBIDate		= t.TransferBIDate
						, p.BIFactoryID			= t.BIFactoryID
						, p.BIInsertDate		= t.BIInsertDate
				FROM P_MtltoFTYAnalysis p
				inner join #Final t on p.WKID = t.WKID and p.OrderID = t.OrderID and p.Seq1 = t.Seq1 and p.Seq2 = t.Seq2

				insert into P_MtltoFTYAnalysis([Factory], [Country], [Brand], [WeaveType], [ETD], [ETA], [CloseDate], [ActDate], [Category], [OrderID], [Seq1], [Seq2], [OrderCfmDate], [SciDelivery], [Refno], [SCIRefno], [SuppID], [SuppName], [CurrencyID], [CurrencyRate], [Price], [Price(TWD)], [Unit], [PoQty], [PoFoc], [ShipQty], [ShipFoc], [TTShipQty], [ShipAmt(TWD)], [FabricJunk], [WKID], [ShipmentTerm], [FabricType], [PINO], [PIDATE], [Color], [ColorName], [Season], [PCHandle], [POHandle], [POSMR], [Style], [OrderType], [ShipModeID], [Supp1stCfmDate], [BrandSuppCode], [BrandSuppName], [CountryofLoading], [SupdelRvsd], [ProdItem], [KPILETA], [MaterialConfirm], [SupplierGroup], [TransferBIDate],[BIFactoryID],[BIInsertDate])
				select
				[Factory]
				, [Country]
				, [Brand]
				, [WeaveType]
				, [ETD]
				, [ETA]
				, [CloseDate]
				, [ActDate]
				, [Category]
				, [OrderID]
				, [Seq1]
				, [Seq2]
				, [OrderCfmDate]
				, [SciDelivery]
				, [Refno]
				, [SCIRefno]
				, [SuppID]
				, [SuppName]
				, [CurrencyID]
				, [CurrencyRate]
				, [Price]
				, [PriceTWD]
				, [Unit]
				, [PoQty]
				, [PoFoc]
				, [ShipQty]
				, [ShipFoc]
				, [TTShipQty]
				, [ShipAmtTWD]
				, [FabricJunk]
				, [WKID]
				, [ShipmentTerm]
				, [FabricType]
				, [PINO]
				, [PIDATE]
				, [Color]
				, [ColorName]
				, [Season]
				, [PCHandle]
				, [POHandle]
				, [POSMR]
				, [Style]
				, [OrderType]
				, [ShipModeID]
				, [Supp1stCfmDate]
				, [BrandSuppCode]
				, [BrandSuppName]
				, [CountryofLoading]
				, [SupdelRvsd]
				, [ProdItem]
				, [KPILETA]
				, [MaterialConfirm]
				, [SupplierGroup]
				, [TransferBIDate]
				, [BIFactoryID]
				, [BIInsertDate]
				from #Final t
				where not exists (select 1 from P_MtltoFTYAnalysis p where p.WKID = t.WKID and p.OrderID = t.OrderID and p.Seq1 = t.Seq1 and p.Seq2 = t.Seq2)
                ";
                finalResult.Result = TransactionClass.ProcessWithDatatableWithTransactionScope(dt, null, sql, out DataTable dataTable, conn: sqlConn, paramters: lisSqlParameter, temptablename: "#Final");
            }

            return finalResult;
        }

        private Base_ViewModel GetIMtltoFTYAnalysis_Data(ExecutedList item)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("@SDate", item.SDate),
                new SqlParameter("@BIFactoryID", item.RgCode),
            };

            string sqlcmd = $@"

			Select [Factory] = ISNULL(main.FactoryID, '')
				, [Country] = ISNULL(Factory.CountryID, '')
				, [Brand] = ISNULL(main.BrandID, '')
				, [WeaveType] = ISNULL(main.WeaveTypeID, '')
				, [ETD] = main.ETD
				, [ETA] = main.ETA
				, [CloseDate] = main.CloseDate
				, [ActDate] = main.ActDate
				, [Category] = ISNULL(ddl.Name, '')
				, [OrderID] = ISNULL(main.POID, '')
				, [Seq1] = ISNULL(main.Seq1, '')
				, [Seq2] = ISNULL(main.Seq2, '')
				, [OrderCfmDate] = main.CFMDate
				, [SciDelivery] = main.SCIDelivery
				, [Refno] = ISNULL(main.Refno, '')
				, [SCIRefno] = ISNULL(main.SCIRefno, '')
				, [SuppID] = ISNULL(Supp.ID, '')
				, [SuppName] = ISNULL(Supp.AbbCh, '')
				, [CurrencyID] = ISNULL(Supp.CurrencyID, '')
				, [CurrencyRate] = ISNULL(curTWD.Rate, 0)
				, [Price] = ISNULL(main.Price, 0)
				, [PriceTWD] = ISNULL(main.Price * curTWD.Rate, 0)
				, [Unit] = ISNULL(main.POUnit, '')
				, [PoQty] = ISNULL(main.PoQty, 0)
				, [PoFoc] = ISNULL(main.PoFoc, 0)
				, [ShipQty] = ISNULL(main.ShipQty, 0)
				, [ShipFoc] = ISNULL(main.ShipFoc, 0)
				, [TTShipQty] = ISNULL(main.TTShipQty, 0)
				, [ShipAmtTWD] = ISNULL(dbo.GetAmount((main.Price * curTWD.Rate),main.ShipQty, 1, 2), 0) 
				, [FabricJunk] = ISNULL(main.FabricJunk, '')
				, [WKID] = ISNULL(main.ID, '')
				, [ShipmentTerm] = ISNULL(main.ShipmentTerm, '')
				, [FabricType] = ISNULL(main.FabricType, '')
				, [PINO] = ISNULL(main.PINO, '')
				, [PIDATE] = main.PIDate
				, [Color] = ISNULL(main.ColorID, '')
				, [ColorName] = ISNULL(Color.Name, '')
				, [Season] = ISNULL(main.SeasonID, '')
				, [PCHandle] = ISNULL(pcUser.IdAndNameAndExt, '')
				, [POHandle] = ISNULL(poUser.IdAndNameAndExt, '')
				, [POSMR] = ISNULL(posmrUser.IdAndNameAndExt, '')
				, [Style] = ISNULL(main.StyleID, '')
				, [OrderType] = ISNULL(main.OrderTypeID, '')
				, [ShipModeID] = ISNULL(main.ShipModeID, '')
				, [Supp1stCfmDate] = IsNull(stockPO3.CfmETD, stockPO3.SystemETD)
				, [BrandSuppCode]= ISNULL(Supp_BrandSuppCode.SuppCode, '')
				, [BrandSuppName] = ISNULL(Supp_BrandSuppCode.SuppName, '')
				, [CountryofLoading] = ISNULL(Country.Alias, '')
				, [SupdelRvsd] = stockPO3.RevisedETD
				, [ProdItem] = ISNULL(MtlType.ProductionType, '')
				, [KPILETA] = main.EstLETA
				, [MaterialConfirm] = iif(main.Confirm = 1, 'Y', 'N')
				, [SupplierGroup] = ISNULL(Supp.SuppGroupFabric, '')
				, [TransferBIDate] = GETDATE()
				, [BIFactoryID] = @BIFactoryID
				, [BIInsertDate] = GetDate()
			From (
				SELECT Orders.FactoryID
					, Orders.BrandID
					, e.ETD
					, e.ETA
					, e.CloseDate
					, e.WhseArrival as ActDate
					, ed.POID
					, ed.Seq1
					, ed.Seq2
					, Orders.CFMDate
					, Orders.SCIDelivery
					, ed.Refno
					, ed.SCIRefno
					, ed.Price
					, po3.POUnit
					, po3.Qty as PoQty
					, po3.Foc as PoFoc
					, ed.Qty as ShipQty
					, ed.Foc as ShipFoc
					, ed.Qty + ed.Foc as TTShipQty
					, e.ID
					, e.ShipmentTerm
					, po3.PINO
					, po3.PIDate
					, ColorID = po3Spec.Color
					, Orders.SeasonID
					, Orders.StyleID
					, Orders.OrderTypeID
					, e.ShipModeID
					, Orders.Category
					, PO.PCHandle
					, PO.POHandle
					, PO.POSMR
					, ed.SuppID
					, po3.StockPOID
					, po3.StockSeq1
					, po3.StockSeq2
					, Fabric.WeaveTypeID
					, iif(cfu.Junk = 1, 'Y', '') as FabricJunk
					, iif(Fabric.Type = 'F', 'Fabric', 'Accessory') as FabricType
					, [Confirm] = po3.Complete
					, GetSci.EstLETA
				FROM [Production].dbo.Export e WITH (NOLOCK)
				Left join [Production].dbo.Export_Detail ed WITH (NOLOCK) on e.ID = ed.ID
				Left join [Production].dbo.PO_Supp_Detail po3 WITH (NOLOCK) on po3.ID = ed.POID and po3.Seq1 = ed.Seq1 and po3.Seq2 = ed.Seq2
				Outer apply [Production].dbo.GetPo3Spec(po3.ID, po3.Seq1, po3.Seq2) po3Spec
				Left join [Production].dbo.PO WITH (NOLOCK) on PO.ID = po3.ID
				Left join [Production].dbo.Orders WITH (NOLOCK) on Orders.ID = po3.ID
				Left join [Production].dbo.Fabric WITH (NOLOCK) on ed.SciRefno = Fabric.SciRefno
				Outer apply (Select * From [Production].dbo.CheckFabricUseful(ed.SCIRefno, Orders.SeasonID, ed.SuppID)) cfu
				Outer apply [Production].dbo.GetSCI(Orders.POID, Orders.Category) as GetSci
				Where e.CloseDate >= @SDate AND ed.PoType = 'G' 

			) main
			inner join [Production].dbo.Factory WITH (NOLOCK) on main.FactoryID = Factory.ID and Factory.IsProduceFty = 1
			Left join [Production].dbo.Supp WITH (NOLOCK) on main.SuppID = Supp.ID
			Left join [Production].dbo.Country WITH (NOLOCK) On Country.ID = Supp.CountryID
			Left join [Production].dbo.Fabric WITH (NOLOCK) on main.SciRefno = Fabric.SciRefno
			Left Join [Production].dbo.MtlType WITH (NOLOCK) On Fabric.MtltypeId = MtlType.ID
			Left join [Production].dbo.Color WITH (NOLOCK) on Color.ID = main.ColorID And Color.BrandId = main.BrandID
			Outer Apply (select [IdAndNameAndExt] = [Production].dbo.getTPEPass1_ExtNo(main.PCHandle)) as pcUser
			Outer Apply (select [IdAndNameAndExt] = [Production].dbo.getTPEPass1_ExtNo(main.POHandle)) as poUser
			Outer Apply (select [IdAndNameAndExt] = [Production].dbo.getTPEPass1_ExtNo(main.POSMR)) as posmrUser 
			Left Join [Production].dbo.DropDownList ddl WITH (NOLOCK) on ddl.Type = 'Category' and ddl.ID = main.Category
			Left join [Production].dbo.Supp_BrandSuppCode WITH (NOLOCK) On Supp_BrandSuppCode.ID = Supp.ID and Supp_BrandSuppCode.BrandID = main.BrandID
			Outer apply [Production].dbo.GetCurrencyRate('20', Supp.CurrencyID, 'TWD' ,main.CFMDate) as curTWD
			Outer Apply (
				Select CfmETD, SystemETD, RevisedETD
				from [Production].dbo.PO_Supp_Detail tmpPO3 WITH (NOLOCK)
				inner join [Production].dbo.Po_Supp tmpPO2 WITH (NOLOCK) on tmpPO3.ID = tmpPO2.ID and tmpPO3.Seq1 = tmpPO2.Seq1
				inner join [Production].dbo.Supp stockSupp WITH (NOLOCK) on tmpPO2.SuppID = stockSupp.ID
				inner join [Production].dbo.Orders stockOrders WITH (NOLOCK) on stockOrders.ID = tmpPO3.ID
				where tmpPO3.ID = IIF(IsNull(main.StockPOID, '') = '' , main.POID, main.StockPOID)
					and tmpPO3.Seq1 = IIF(IsNull(main.StockPOID, '') = '' , main.Seq1, main.StockSeq1)
					and tmpPO3.Seq2 = IIF(IsNull(main.StockPOID, '') = '' , main.Seq2, main.StockSeq2)
			) stockPO3
			Order by main.ID, main.POID, main.Seq1, main.Seq2
			";

            Base_ViewModel resultReport = new Base_ViewModel
            {
                Result = this.DBProxy.Select("Production", sqlcmd, sqlParameters, out DataTable dt),
            };

            if (!resultReport.Result)
            {
                return resultReport;
            }

            resultReport.Dt = dt;

            return resultReport;
        }
    }
}
