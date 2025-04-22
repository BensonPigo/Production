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
    public class P_Import_ProductionKitsTracking
    {
        /// <inheritdoc/>
        public Base_ViewModel P_ProductionKitsTracking(DateTime? sDate, DateTime? eDate)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            PPIC_R02 biModel = new PPIC_R02();
            if (!sDate.HasValue)
            {
                sDate = DateTime.Parse(DateTime.Now.AddDays(-30).ToString("yyyy/MM/dd"));
            }

            if (!eDate.HasValue)
            {
                eDate = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd"));
            }

            try
            {
                PPIC_R02_ViewModel ppic_R02 = new PPIC_R02_ViewModel()
                {
                    IsPowerBI = true,
                    Date1 = sDate,
                    Date2 = eDate,
                };

                Base_ViewModel resultReport = biModel.GetPPIC_R02(ppic_R02);
                if (!resultReport.Result)
                {
                    throw resultReport.Result.GetException();
                }

                DataTable detailTable = resultReport.Dt;

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
                    new SqlParameter("@StartDate", sDate),
                    new SqlParameter("@EndDate", eDate),
                };
                string sql = @"
UPDATE p
SET p.BrandID = ISNULL(t.BrandID, '')
   ,p.StyleID = ISNULL(t.StyleID, '')
   ,p.SeasonID = ISNULL(t.SeasonID, '')
   ,p.Article = ISNULL(t.Article, '')
   ,p.Mdivision = ISNULL(t.Mdivision, '')
   ,p.FactoryID = ISNULL(t.FactoryID, '')
   ,p.Doc = ISNULL(t.Doc, '')
   ,p.TWSendDate = t.TWSendDate
   ,p.FtyMRRcvDate = t.FtyMRRcvDate
   ,p.FtySendtoQADate = t.FtySendtoQADate
   ,p.QARcvDate = t.QARcvDate
   ,p.UnnecessaryToSend = ISNULL(t.UnnecessaryToSend, '')
   ,p.ProvideDate = t.ProvideDate
   ,p.SPNo = ISNULL(t.SPNo, '')
   ,p.SCIDelivery = t.SCIDelivery
   ,p.BuyerDelivery = t.BuyerDelivery
   ,p.Pullforward = ISNULL(t.Pullforward, '')
   ,p.Handle = ISNULL(t.Handle, '')
   ,p.MRHandle = ISNULL(t.MRHandle, '')
   ,p.SMR = ISNULL(t.SMR, '')
   ,p.POHandle = ISNULL(t.POHandle, '')
   ,p.POSMR = ISNULL(t.POSMR, '')
   ,p.FtyHandle = ISNULL(t.FtyHandle, '')
   ,p.ProductionKitsGroup = ISNULL(t.ProductionKitsGroup, '')
   ,p.AddDate = t.AddDate
   ,p.EditDate = t.EditDate
   ,p.AWBNO = ISNULL(t.AWBNO, '')
   ,p.Reject = ISNULL(t.Reject, '')
FROM P_ProductionKitsTracking p
INNER JOIN #tmp t
    ON  t.FactoryID = p.FactoryID
    AND t.Ukey = p.Ukey

INSERT INTO P_ProductionKitsTracking (
	BrandID
	,StyleID
	,SeasonID
	,Article
	,Mdivision
	,FactoryID
	,Ukey
	,Doc
	,TWSendDate
	,FtyMRRcvDate
	,FtySendtoQADate
	,QARcvDate
	,UnnecessaryToSend
	,ProvideDate
	,SPNo
	,SCIDelivery
	,BuyerDelivery
	,Pullforward
	,Handle
	,MRHandle
	,SMR
	,POHandle
	,POSMR
	,FtyHandle
	,ProductionKitsGroup
	,AddDate
	,EditDate
    ,AWBNO
    ,Reject
)
SELECT
    ISNULL(t.BrandID, '')
   ,ISNULL(t.StyleID, '')
   ,ISNULL(t.SeasonID, '')
   ,ISNULL(t.Article, '')
   ,ISNULL(t.Mdivision, '')
   ,ISNULL(t.FactoryID, '')
   ,ISNULL(t.UKey, 0)
   ,ISNULL(t.Doc, '')
   ,t.TWSendDate
   ,t.FtyMRRcvDate
   ,t.FtySendtoQADate
   ,t.QARcvDate
   ,ISNULL(t.UnnecessaryToSend, '')
   ,t.ProvideDate
   ,ISNULL(t.SPNo, '')
   ,t.SCIDelivery
   ,t.BuyerDelivery
   ,ISNULL(t.Pullforward, '')
   ,ISNULL(t.Handle, '')
   ,ISNULL(t.MRHandle, '')
   ,ISNULL(t.SMR, '')
   ,ISNULL(t.POHandle, '')
   ,ISNULL(t.POSMR, '')
   ,ISNULL(t.FtyHandle, '')
   ,ISNULL(t.ProductionKitsGroup, '')
   ,t.AddDate
   ,t.EditDate
   ,ISNULL(t.AWBNO, '')
   ,ISNULL(t.Reject, '')
FROM #tmp t
WHERE NOT EXISTS (
    SELECT 1
    FROM P_ProductionKitsTracking p
    WHERE t.FactoryID = p.FactoryID
    AND t.UKey = p.UKey
)

DELETE P_ProductionKitsTracking
FROM P_ProductionKitsTracking p WITH(NOLOCK)
WHERE NOT EXISTS (
    SELECT 1
    FROM #tmp t
    WHERE t.FactoryID = p.FactoryID
    AND t.UKey = p.UKey
)
AND ((AddDate >= @StartDate AND AddDate <= @EndDate)
  OR (EditDate >= @StartDate AND EditDate <= @EndDate))
";
                sql += new Base().SqlBITableInfo("P_ProductionKitsTracking", true);
                finalResult = new Base_ViewModel()
                {
                    Result = TransactionClass.ProcessWithDatatableWithTransactionScope(dt, null, sqlcmd: sql, result: out DataTable dataTable, conn: sqlConn, paramters: sqlParameters),
                };
            }

            return finalResult;
        }
    }
}
