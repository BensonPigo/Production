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
        public Base_ViewModel P_ProductionKitsTracking(ExecutedList item)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            PPIC_R02 biModel = new PPIC_R02();
            if (!item.SDate.HasValue)
            {
                item.SDate = DateTime.Parse(DateTime.Now.AddDays(-30).ToString("yyyy/MM/dd"));
            }

            if (!item.EDate.HasValue)
            {
                item.EDate = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd"));
            }

            try
            {
                PPIC_R02_ViewModel ppic_R02 = new PPIC_R02_ViewModel()
                {
                    IsPowerBI = true,
                    Date1 = item.SDate,
                    Date2 = item.EDate,
                };

                Base_ViewModel resultReport = biModel.GetPPIC_R02(ppic_R02);
                if (!resultReport.Result)
                {
                    throw resultReport.Result.GetException();
                }

                DataTable detailTable = resultReport.Dt;

                // insert into PowerBI
                finalResult = this.UpdateBIData(detailTable, item);
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
            Base_ViewModel finalResult;

            string where = @" 
NOT EXISTS (
SELECT 1
FROM #tmp t
WHERE t.Article = p.Article
AND t.FactoryID = p.FactoryID
AND t.Doc = p.Doc
AND t.SPNo = p.SPNo
AND t.ProductionKitsGroup = p.ProductionKitsGroup
)
AND ((AddDate >= @StartDate AND AddDate <= @EndDate)
OR (EditDate >= @StartDate AND EditDate <= @EndDate))
";
            string tmp = new Base().SqlBITableHistory("P_ProductionKitsTracking", "P_ProductionKitsTracking_History", "#tmp", where, false, false);
            DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);
            using (sqlConn)
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>()
                {
                    new SqlParameter("@StartDate", item.SDate),
                    new SqlParameter("@EndDate", item.EDate),
                    new SqlParameter("@BIFactoryID", item.RgCode),
                    new SqlParameter("@IsTrans", item.IsTrans),
                };
                string sql = $@"
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
   ,p.BIFactoryID = @BIFactoryID
   ,p.BIInsertDate = GetDate()
FROM P_ProductionKitsTracking p
INNER JOIN #tmp t
    ON  t.FactoryID = p.FactoryID
    AND t.Ukey = p.Style_ProductionKitsUkey

INSERT INTO P_ProductionKitsTracking (
	BrandID
	,StyleID
	,SeasonID
	,Article
	,Mdivision
	,FactoryID
	,Style_ProductionKitsUkey
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
    ,BIFactoryID 
    ,BIInsertDate
)
SELECT
    ISNULL(t.BrandID, '')
   ,ISNULL(t.StyleID, '')
   ,ISNULL(t.SeasonID, '')
   ,ISNULL(t.Article, '')
   ,ISNULL(t.Mdivision, '')
   ,ISNULL(t.FactoryID, '')
   ,Style_ProductionKitsUkey = ISNULL(t.UKey, 0)
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
   ,@BIFactoryID
   ,GetDate()
FROM #tmp t
WHERE NOT EXISTS (
    SELECT 1
    FROM P_ProductionKitsTracking p
    WHERE t.FactoryID = p.FactoryID
    AND t.UKey = p.Style_ProductionKitsUkey
)

{tmp}

DELETE p
FROM P_ProductionKitsTracking p WITH(NOLOCK)
WHERE NOT EXISTS (
    SELECT 1
    FROM #tmp t
    WHERE t.FactoryID = p.FactoryID
    AND t.UKey = p.Style_ProductionKitsUkey
)
AND ((AddDate >= @StartDate AND AddDate <= @EndDate)
  OR (EditDate >= @StartDate AND EditDate <= @EndDate))
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
