using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;


namespace Sci.Production.Prg.PowerBI.DataAccess
{
    /// <inheritdoc/>
    public class P_Import_OutstandingPO
    {
        /// <inheritdoc/>
        public P_Import_OutstandingPO()
        {
            Data.DBProxy.Current.DefaultTimeout = 7200;
        }

        /// <inheritdoc/>
        public Base_ViewModel P_OutstandingPO(DateTime? sDate, DateTime? eDate)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            PPIC_R16 biModel = new PPIC_R16();
            if (!sDate.HasValue)
            {
                sDate = DateTime.Parse(DateTime.Now.AddDays(-150).ToString("yyyy/MM/dd"));
            }

            if (!eDate.HasValue)
            {
                eDate = DateTime.Parse(DateTime.Now.AddDays(30).ToString("yyyy/MM/dd"));
            }

            try
            {
                PPIC_R16_ViewModel model = new PPIC_R16_ViewModel()
                {
                    BuyerDeliveryFrom = sDate.Value,
                    BuyerDeliveryTo = eDate.Value,
                    MDivisionID = string.Empty,
                    FactoryID = string.Empty,
                    BrandID = string.Empty,
                    Is7DayEdit = true,
                    IsBookingOrder = false,
                    IsExcludeCancelShortage = false,
                    IsExcludeSister = true,
                    IsJunk = true,
                    IsOutstanding = false,
                };

                Base_ViewModel resultReport = biModel.GetOustandingPO(model);
                if (!resultReport.Result)
                {
                    throw resultReport.Result.GetException();
                }

                // insert into PowerBI
                finalResult = this.UpdateBIData(resultReport.Dt, sDate.Value, eDate.Value);
                if (!finalResult.Result)
                {
                    throw finalResult.Result.GetException();
                }

                finalResult.Result = new Ict.DualResult(true);
            }
            catch (Exception ex)
            {
                finalResult.Result = new Ict.DualResult(false, ex);
            }

            return finalResult;
        }

        private Base_ViewModel UpdateBIData(DataTable dt, DateTime? sDate, DateTime? eDate)
        {
            Base_ViewModel finalResult;
            Data.DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);
            using (sqlConn)
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>()
                {
                    new SqlParameter("@StartDate", sDate),
                    new SqlParameter("@EndDate", eDate),
                };

                string sql = @"
alter table #tmp alter column FactoryID varchar(8)
alter table #tmp alter column ID varchar(13)
alter table #tmp alter column Seq varchar(2)

                update t
                SET 
	            t.CustPONo							= ISNULL(s.CustPONo,''),
	            t.StyleID							= ISNULL(s.StyleID,	''),
	            t.BrandID							= ISNULL(s.BrandID,	''),
	            t.BuyerDelivery						= s.BuyerDelivery,
	            t.ShipModeID						= ISNULL(s.ShipModeID,''),
	            t.Category							= ISNULL(s.Category,''),
	            t.PartialShipment					= ISNULL(s.PartialShipment,''),
	            t.BookingSP							= ISNULL(s.BookingSP,''),
	            t.Junk								= ISNULL(s.Cancelled,''),
	            t.OrderQty							= ISNULL(s.OrderQty,0),
	            t.PackingCtn						= ISNULL(s.PackingCarton,0),
	            t.PackingQty						= ISNULL(s.PackingQty,0),
	            t.ClogRcvCtn                        = ISNULL(s.ClogReceivedCarton,0),
	            t.ClogRcvQty                        = ISNULL(s.ClogReceivedQty,0),
	            t.LastCMPOutputDate					= s.LastCMPOutputDate,
	            t.CMPQty							= ISNULL(s.CMPQty,0),
	            t.LastDQSOutputDate					= s.LastDQSOutputDate,
	            t.DQSQty							= ISNULL(s.DQSQty,''),
	            t.OSTPackingQty						= ISNULL(s.OSTPackingQty,''),
	            t.OSTCMPQty							= ISNULL(s.OSTCMPQty,''),
	            t.OSTDQSQty							= ISNULL(s.OSTDQSQty,''),
	            t.OSTClogQty						= ISNULL(s.OSTClogQty,''),
	            t.OSTClogCtn						= ISNULL(s.OSTClogCtn,0),
	            t.PulloutComplete					= ISNULL(s.PulloutComplete,''),
	            t.dest								= ISNULL(s.dest,''),
	            t.KPIGroup							= ISNULL(s.KPICode,''),
	            t.CancelledButStillNeedProduction	= ISNULL(s.CancelledButStillNeedProduction,''),
	            t.CFAInspectionResult				= ISNULL(s.CFAInspectionResult,''),
	            t.[3rdPartyInspection]				= ISNULL(s.[3rdPartyInspection],''),
	            t.[3rdPartyInspectionResult]		= ISNULL(s.[3rdPartyInspectionResult],''),
	            t.LastCartonReceivedDate			= s.LastCartonReceivedDate,
                t.BIFactoryID                       = s.BIFactoryID,
                t.BIInsertDate                      = s.BIInsertDate
                from P_OustandingPO t
                inner join #tmp s  
		                ON t.FactoryID = s.FactoryID
		                AND t.OrderID = s.ID
		                AND t.Seq = s.Seq

                insert into P_OustandingPO ([FactoryID], [OrderID], [CustPONo], [StyleID], [BrandID], [BuyerDelivery], [Seq], [ShipModeID], [Category]
                , [PartialShipment], [Junk], [OrderQty], [PackingCtn], [PackingQty], [ClogRcvCtn], [ClogRcvQty], [LastCMPOutputDate], [CMPQty]
                , [LastDQSOutputDate], [DQSQty], [OSTPackingQty], [OSTCMPQty], [OSTDQSQty], [OSTClogQty], [OSTClogCtn], [PulloutComplete], [Dest]
                , [KPIGroup], [CancelledButStillNeedProduction], [CFAInspectionResult], [3rdPartyInspection], [3rdPartyInspectionResult], [BookingSP],[LastCartonReceivedDate], [BIFactoryID], [BIInsertDate])
                select  
                [FactoryID]								= ISNULL(s.FactoryID,''),
		        [OrderID]								= ISNULL(s.id,''),
		        [CustPONo]								= ISNULL(s.CustPONo,''),
		        [StyleID]								= ISNULL(s.StyleID,''),
		        [BrandID]								= ISNULL(s.BrandID,''),
		        [BuyerDelivery]							= s.BuyerDelivery,
		        [Seq]									= ISNULL(s.Seq,''),
		        [ShipModeID]							= ISNULL(s.ShipModeID,''),
		        [Category]								= ISNULL(s.Category,''),
		        [PartialShipment]						= ISNULL(s.PartialShipment,''),
		        [Junk]									= ISNULL(s.Cancelled,''),
		        [OrderQty]								= ISNULL(s.OrderQty,0),
		        [PackingCtn]							= ISNULL(s.PackingCarton,0),
		        [PackingQty]							= ISNULL(s.PackingQty,0),
		        [ClogRcvCtn]							= ISNULL(s.ClogReceivedCarton,0),
		        [ClogRcvQty]							= ISNULL(s.ClogReceivedQty,0),
		        [LastCMPOutputDate]						= s.LastCMPOutputDate,
		        [CMPQty]								= ISNULL(s.CMPQty,0),
		        [LastDQSOutputDate]						= s.LastDQSOutputDate,
		        [DQSQty]								= ISNULL(s.DQSQty,''),
		        [OSTPackingQty]							= ISNULL(s.OSTPackingQty,''),
		        [OSTCMPQty]								= ISNULL(s.OSTCMPQty,''),
		        [OSTDQSQty]								= ISNULL(s.OSTDQSQty,''),
		        [OSTClogQty]							= ISNULL(s.OSTClogQty,''),
		        [OSTClogCtn]							= ISNULL(s.OSTClogCtn,0),
		        [PulloutComplete]						= ISNULL(s.PulloutComplete,''),
		        [Dest]									= ISNULL(s.dest,''),
		        [KPIGroup]								= ISNULL(s.KPICode,''),
		        [CancelledButStillNeedProduction]		= ISNULL(s.CancelledButStillNeedProduction,''),
		        [CFAInspectionResult]					= ISNULL(s.CFAInspectionResult,''),
		        [3rdPartyInspection]					= ISNULL(s.[3rdPartyInspection],''),
		        [3rdPartyInspectionResult]				= ISNULL(s.[3rdPartyInspectionResult],''),
		        [BookingSP]								= ISNULL(s.BookingSP,''),
		        [LastCartonReceivedDate]				= s.LastCartonReceivedDate,
                [BIFactoryID]                           = s.BIFactoryID,
                [BIInsertDate]                          = s.BIInsertDate
                from #tmp s
                where not exists(
	                select 1 from P_OustandingPO t 
	                where t.FactoryID = s.FactoryID
	                AND t.OrderID = s.ID
	                AND t.Seq = s.Seq
                )

                insert into P_OustandingPO_History ([FactoryID],[OrderID],[Seq],BIFactoryID,BIInsertDate)
                Select t.FactoryID, t.OrderID, t.Seq, t.BIFactoryID, GetDate()
                from dbo.P_OustandingPO t
                left join #tmp s on t.FactoryID = s.FactoryID AND t.OrderID = s.ID AND t.Seq = s.Seq
                where t.BuyerDelivery between @StartDate and @EndDate
	                and s.ID IS NULL

                delete t
                from P_OustandingPO t
                left join #tmp s on t.FactoryID = s.FactoryID
	                AND t.OrderID = s.ID
	                AND t.Seq = s.Seq
                where t.BuyerDelivery between @StartDate and @EndDate
	                and s.ID IS NULL

                insert into P_OustandingPO_History ([FactoryID],[OrderID],[Seq],BIFactoryID,BIInsertDate)
                Select t.FactoryID, t.OrderID, t.Seq, t.BIFactoryID, GetDate()
                from dbo.P_OustandingPO t
                where exists (select 1 from MainServer.Production.dbo.Order_QtyShip oq where t.OrderID = oq.Id)
                and t.Seq = ''

                delete t
                from P_OustandingPO t
                where exists (select 1 from MainServer.Production.dbo.Order_QtyShip oq where t.OrderID = oq.Id)
                and t.Seq = ''

                insert into P_OustandingPO_History ([FactoryID],[OrderID],[Seq],BIFactoryID,BIInsertDate)
                Select t.FactoryID, t.OrderID, t.Seq, t.BIFactoryID, GetDate()
                from dbo.P_OustandingPO t
                where BuyerDelivery > @EndDate

                delete P_OustandingPO
                where BuyerDelivery > @EndDate

                update b
                    set b.TransferDate = getdate()
		                , b.IS_Trans = 1
                from BITableInfo b
                where b.id = 'P_OustandingPO'
                ";

                finalResult = new Base_ViewModel()
                {
                    Result = TransactionClass.ProcessWithDatatableWithTransactionScope(dt, null, sqlcmd: sql, result: out DataTable dataTable, conn: sqlConn, paramters: sqlParameters),
                };
            }

            return finalResult;
        }
    }
}
