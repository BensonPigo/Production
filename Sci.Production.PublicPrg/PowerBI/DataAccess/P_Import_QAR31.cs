using System;
using System.Collections.Generic;
using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;
using System.Data;
using System.Data.SqlClient;
using Sci.Data;

namespace Sci.Production.Prg.PowerBI.DataAccess
{
    /// <inheritdoc/>
    public class P_Import_QAR31
    {
        /// <inheritdoc/>
        public Base_ViewModel P_QAR31(DateTime? sDate, DateTime? eDate)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            QA_R31 biModel = new QA_R31();
            if (!sDate.HasValue)
            {
                sDate = DateTime.Parse(DateTime.Now.AddDays(-60).ToString("yyyy/MM/dd"));
            }

            if (!eDate.HasValue)
            {
                eDate = DateTime.Parse(DateTime.Now.AddDays(30).ToString("yyyy/MM/dd"));
            }

            try
            {
                QA_R31_ViewModel qa_R31 = new QA_R31_ViewModel()
                {
                    BuyerDelivery1 = sDate,
                    BuyerDelivery2 = eDate,
                    SP1 = string.Empty,
                    SP2 = string.Empty,
                    MDivisionID = string.Empty,
                    FactoryID = string.Empty,
                    Brand = string.Empty,
                    Category = string.Empty,
                    Exclude_Sister_Transfer_Out = false,
                    Outstanding = false,
                    InspStaged = string.Empty,
                    IsPowerBI = true,
                };

                Base_ViewModel resultReport = biModel.GetQA_R31Data(qa_R31);
                if (!resultReport.Result)
                {
                    throw resultReport.Result.GetException();
                }

                DataTable detailTable = resultReport.Dt;
                if (!resultReport.Result)
                {
                    throw resultReport.Result.GetException();
                }

                // insert into PowerBI
                finalResult = this.UpdateBIData(detailTable, sDate.Value, eDate.Value);
                if (!finalResult.Result)
                {
                    throw finalResult.Result.GetException();
                }
            }
            catch (Exception ex)
            {
                finalResult.Result = new Ict.DualResult(false, ex);
            }

            return finalResult;
        }

        private Base_ViewModel UpdateBIData(DataTable dt, DateTime sDate, DateTime eDate)
        {
            Base_ViewModel finalResult;
            DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);
            using (sqlConn)
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>()
                {
                    new SqlParameter("@SDate", sDate),
                    new SqlParameter("@EDate", eDate),
                };
                string sql = @"	
UPDATE t
SET 
		t.Stage =  s.Stage,
		t.InspResult =  s.InspResult,
		t.NotYetInspCtn# =  s.NotYetInspCtn#,
		t.NotYetInspCtn =  s.NotYetInspCtn,
		t.NotYetInspQty =  s.NotYetInspQty,
		t.FailCtn# =  s.FailCtn#,
		t.FailCtn =  s.FailCtn,
		t.FailQty =  s.FailQty,
		t.MDivisionID =  s.MDivisionID,
		t.FactoryID =  s.FactoryID,
		t.BuyerDelivery =  s.BuyerDelivery,
		t.BrandID =  s.BrandID,
		t.OrderID =  s.ID,
		t.Category =  s.Category,
		t.OrderTypeID =  s.OrderTypeID,
		t.CustPoNo =  s.CustPoNo,
		t.StyleID =  s.StyleID,
		t.StyleName =  s.StyleName,
		t.SeasonID =  s.SeasonID,
		t.Dest =  s.Dest,
		t.Customize1 =  s.Customize1,
		t.CustCDID =  s.CustCDID,
		t.Seq =  s.Seq,
		t.ShipModeID =  s.ShipModeID,
		t.ColorWay =  s.ColorWay,
		t.SewLine =  s.SewLine,
		t.TtlCtn =  s.TtlCtn,
		t.StaggeredCtn =  s.StaggeredCtn,
		t.ClogCtn =  s.ClogCtn,
		t.[ClogCtn%] =  s.[ClogCtn%],
		t.LastCartonReceivedDate =  s.LastCartonReceivedDate,
		t.CFAFinalInspectDate =  s.CFAFinalInspectDate,
		t.CFA3rdInspectDate =  s.CFA3rdInspectDate,
		t.CFARemark =  s.CFARemark
from P_QA_R31 t 
inner join #tmp s on t.OrderID = s.ID 
AND t.Stage = s.Stage 
AND t.StyleName = s.StyleName 
AND t.Seq = s.Seq


insert into P_QA_R31 (
    Stage,InspResult,NotYetInspCtn#,NotYetInspCtn,NotYetInspQty,FailCtn#,FailCtn,FailQty,MDivisionID,FactoryID,BuyerDelivery,BrandID,OrderID,Category
,OrderTypeID,CustPoNo,StyleID,StyleName,SeasonID,Dest,Customize1,CustCDID,Seq,ShipModeID,ColorWay,SewLine,TtlCtn,StaggeredCtn,ClogCtn,[ClogCtn%],LastCartonReceivedDate
,CFAFinalInspectDate,CFA3rdInspectDate,CFARemark
)
select 	s.Stage,s.InspResult,s.NotYetInspCtn#,s.NotYetInspCtn,s.NotYetInspQty,s.FailCtn#,s.FailCtn,s.FailQty,s.MDivisionID,s.FactoryID,s.BuyerDelivery,s.BrandID,s.ID,s.Category,
s.OrderTypeID,s.CustPoNo,s.StyleID,s.StyleName,s.SeasonID,s.Dest,s.Customize1,s.CustCDID,s.Seq,s.ShipModeID,s.ColorWay,s.SewLine,s.TtlCtn,s.StaggeredCtn,s.ClogCtn,s.[ClogCtn%],
s.LastCartonReceivedDate,s.CFAFinalInspectDate,s.CFA3rdInspectDate,s.CFARemark
from #tmp s
where not exists (
    select 1 from P_QA_R31 t 
    where t.OrderID = s.ID 
    AND t.Stage = s.Stage 
    AND t.StyleName = s.StyleName 
    AND t.Seq = s.Seq
)

Insert into P_QA_R31_History
Select t.FactoryID, t.BIFactoryID, [BIInsertDate]=GetDate()
From P_QA_R31 t
where not exists (
    select 1 from #tmp s 
    where t.OrderID = s.ID 
    AND t.Stage = s.Stage 
    AND t.StyleName = s.StyleName 
    AND t.Seq = s.Seq
)
and t.BuyerDelivery between @SDate and @EDate
and exists(
	select * from Production.dbo.Factory  as f
	where f.ProduceM = f.MDivisionID
	and f.id = t.FactoryID
)


delete t 
from dbo.P_QA_R31 t
where not exists (
    select 1 from #tmp s 
    where t.OrderID = s.ID 
    AND t.Stage = s.Stage 
    AND t.StyleName = s.StyleName 
    AND t.Seq = s.Seq
)
and t.BuyerDelivery between @SDate and @EDate
and exists(
	select * from Production.dbo.Factory  as f
	where f.ProduceM = f.MDivisionID
	and f.id = t.FactoryID
)
";
                sql += new Base().SqlBITableInfo("P_QA_R31", true);
                finalResult = new Base_ViewModel()
                {
                    Result = TransactionClass.ProcessWithDatatableWithTransactionScope(dt, null, sqlcmd: sql, result: out DataTable dataTable, conn: sqlConn, paramters: sqlParameters),
                };
            }

            return finalResult;
        }
    }
}
