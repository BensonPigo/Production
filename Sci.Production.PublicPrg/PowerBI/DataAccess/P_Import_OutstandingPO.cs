using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
update t
SET 
	t.CustPONo =  s.CustPONo,
	t.StyleID =  s.StyleID,
	t.BrandID = s.BrandID,
	t.BuyerDelivery = s.BuyerDelivery,
	t.ShipModeID =  s.ShipModeID,
	t.Category =  s.Category,
	t.PartialShipment =  s.PartialShipment,
	t.BookingSP = s.BookingSP,
	t.Junk =  s.Cancelled,
	t.OrderQty =  s.OrderQty,
	t.PackingCtn =  s.PackingCarton,
	t.PackingQty =  s.PackingQty,
	t.ClogRcvCtn =  s.ClogReceivedCarton,
	t.ClogRcvQty =  s.ClogReceivedQty,
	t.LastCMPOutputDate =  s.LastCMPOutputDate,
	t.CMPQty =  s.CMPQty,
	t.LastDQSOutputDate =  s.LastDQSOutputDate,
	t.DQSQty =  s.DQSQty,
	t.OSTPackingQty =  s.OSTPackingQty,
	t.OSTCMPQty =  s.OSTCMPQty,
	t.OSTDQSQty =  s.OSTDQSQty,
	t.OSTClogQty =  s.OSTClogQty,
	t.OSTClogCtn =  s.OSTClogCtn,
	t.PulloutComplete = s.PulloutComplete,
	t.dest = s.dest,
	t.KPIGroup = s.KPICode,
	t.CancelledButStillNeedProduction = s.CancelledButStillNeedProduction,
	t.CFAInspectionResult = s.CFAInspectionResult,
	t.[3rdPartyInspection] = s.[3rdPartyInspection],
	t.[3rdPartyInspectionResult] = s.[3rdPartyInspectionResult],
	t.LastCartonReceivedDate = s.LastCartonReceivedDate
from P_OustandingPO t
inner join #tmp s  
		ON t.FactoryID=s.FactoryID  
		AND t.orderid=s.id 
		AND t.seq = s.seq 

insert into P_OustandingPO ([FactoryID], [OrderID], [CustPONo], [StyleID], [BrandID], [BuyerDelivery], [Seq], [ShipModeID], [Category]
, [PartialShipment], [Junk], [OrderQty], [PackingCtn], [PackingQty], [ClogRcvCtn], [ClogRcvQty], [LastCMPOutputDate], [CMPQty]
, [LastDQSOutputDate], [DQSQty], [OSTPackingQty], [OSTCMPQty], [OSTDQSQty], [OSTClogQty], [OSTClogCtn], [PulloutComplete], [Dest]
, [KPIGroup], [CancelledButStillNeedProduction], [CFAInspectionResult], [3rdPartyInspection], [3rdPartyInspectionResult], [BookingSP],[LastCartonReceivedDate])
select  s.FactoryID,
		s.id,
		s.CustPONo,
		s.StyleID,
		s.BrandID,
		s.BuyerDelivery,
		s.Seq,
		s.ShipModeID,
		s.Category,
		s.PartialShipment,
		s.Cancelled,
		s.OrderQty,
		s.PackingCarton,
		s.PackingQty,
		s.ClogReceivedCarton,
		s.ClogReceivedQty,
		s.LastCMPOutputDate,
		s.CMPQty,
		s.LastDQSOutputDate,
		s.DQSQty,
		s.OSTPackingQty,
		s.OSTCMPQty,
		s.OSTDQSQty,
		s.OSTClogQty,
		s.OSTClogCtn,
		s.PulloutComplete,
		s.dest,
		s.KPICode,
		s.CancelledButStillNeedProduction,
		s.CFAInspectionResult,
		s.[3rdPartyInspection],
		s.[3rdPartyInspectionResult],
		s.BookingSP,
		s.LastCartonReceivedDate
from #tmp s
where not exists(
	select 1 from P_OustandingPO t 
	where t.FactoryID = s.FactoryID  
	AND t.orderid = s.id 
	AND t.seq = s.seq 
)

delete t
from P_OustandingPO t
left join #tmp s on t.FactoryID = s.FactoryID  
	AND t.orderid = s.id 
	AND t.seq = s.seq 
where t.BuyerDelivery between @StartDate and @EndDate
	and s.ID IS NULL

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
                    Result = MyUtility.Tool.ProcessWithDatatable(dt, null, sqlcmd: sql, result: out DataTable dataTable, conn: sqlConn, paramters: sqlParameters),
                };
            }

            return finalResult;
        }
    }
}
