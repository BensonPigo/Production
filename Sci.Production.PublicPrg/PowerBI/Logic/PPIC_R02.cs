using Sci.Data;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Sci.Production.Prg.PowerBI.Logic
{
    /// <summary>
    /// 有調整到需要一併更新至BI
    /// </summary>
    public class PPIC_R02
    {
        /// <inheritdoc/>
        public PPIC_R02()
        {
            DBProxy.Current.DefaultTimeout = 7200;
        }

        /// <inheritdoc/>
        public Base_ViewModel GetPPIC_R02(PPIC_R02_ViewModel model)
        {
            List<SqlParameter> listPar;
            if (model.IsPowerBI)
            {
                listPar = new List<SqlParameter>
                {
                    new SqlParameter("@Date1", SqlDbType.Date) { Value = (object)model.Date1 ?? DBNull.Value },
                    new SqlParameter("@Date2", SqlDbType.Date) { Value = (object)model.Date2 ?? DBNull.Value },
                };
            }
            else
            {
                listPar = new List<SqlParameter>
                {
                    new SqlParameter("@SciDelivery1", SqlDbType.Date) { Value = (object)model.SciDelivery1 ?? DBNull.Value },
                    new SqlParameter("@SciDelivery2", SqlDbType.Date) { Value = (object)model.SciDelivery2 ?? DBNull.Value },
                    new SqlParameter("@ProvideDate1", SqlDbType.Date) { Value = (object)model.ProvideDate1 ?? DBNull.Value },
                    new SqlParameter("@ProvideDate2", SqlDbType.Date) { Value = (object)model.ProvideDate2 ?? DBNull.Value },
                    new SqlParameter("@ReceiveDate1", SqlDbType.Date) { Value = (object)model.ReceiveDate1 ?? DBNull.Value },
                    new SqlParameter("@ReceiveDate2", SqlDbType.Date) { Value = (object)model.ReceiveDate2 ?? DBNull.Value },
                    new SqlParameter("@BrandID", SqlDbType.VarChar, 8) { Value = model.BrandID },
                    new SqlParameter("@StyleID", SqlDbType.VarChar, 15) { Value = model.StyleID },
                    new SqlParameter("@SeasonID", SqlDbType.VarChar, 10) { Value = model.SeasonID },
                    new SqlParameter("@MDivisionID", SqlDbType.VarChar, 8) { Value = model.MDivisionID },
                    new SqlParameter("@MRHandle", SqlDbType.VarChar, 10) { Value = model.MRHandle },
                    new SqlParameter("@SMR", SqlDbType.VarChar, 10) { Value = model.SMR },
                    new SqlParameter("@PoHandle", SqlDbType.VarChar, 10) { Value = model.PoHandle },
                    new SqlParameter("@POSMR", SqlDbType.VarChar, 10) { Value = model.POSMR },
                };
            }

            #region where
            string where = string.Empty;
            if (!MyUtility.Check.Empty(model.Date1))
            {
                where += "AND (sp.AddDate >= @Date1 OR sp.EditDate >= @Date1)\r\n";
            }

            if (!MyUtility.Check.Empty(model.Date2))
            {
                where += "AND (sp.AddDate <= @Date2 OR sp.EditDate <= @Date2)\r\n";
            }

            if (!MyUtility.Check.Empty(model.SciDelivery1))
            {
                where += "AND sp.SCIDelivery >= @SCIDelivery1\r\n";
            }

            if (!MyUtility.Check.Empty(model.SciDelivery2))
            {
                where += "AND sp.SCIDelivery <= @SCIDelivery2\r\n";
            }

            if (!MyUtility.Check.Empty(model.ProvideDate1))
            {
                where += "AND sp.ProvideDate >= @ProvideDate1\r\n";
            }

            if (!MyUtility.Check.Empty(model.ProvideDate2))
            {
                where += "AND sp.ProvideDate <= @ProvideDate2\r\n";
            }

            if (!MyUtility.Check.Empty(model.ReceiveDate1))
            {
                where += "AND sp.ReceiveDate >= @ReceiveDate1\r\n";
            }

            if (!MyUtility.Check.Empty(model.ReceiveDate2))
            {
                where += "AND sp.ReceiveDate <= @ReceiveDate2\r\n";
            }

            if (!MyUtility.Check.Empty(model.BrandID))
            {
                where += "AND s.BrandID = @BrandID\r\n";
            }

            if (!MyUtility.Check.Empty(model.StyleID))
            {
                where += "AND s.ID = @StyleID\r\n";
            }

            if (!MyUtility.Check.Empty(model.SeasonID))
            {
                where += "AND s.SeasonID = @SeasonID\r\n";
            }

            if (!MyUtility.Check.Empty(model.MDivisionID))
            {
                where += "AND sp.ProductionKitsGroup = @MDivisionID\r\n";
            }

            if (!MyUtility.Check.Empty(model.MRHandle))
            {
                where += "AND sp.MRHandle = @MRHandle\r\n";
            }

            if (!MyUtility.Check.Empty(model.SMR))
            {
                where += "AND sp.SMR = @SMR\r\n";
            }

            if (!MyUtility.Check.Empty(model.PoHandle))
            {
                where += "AND sp.PoHandle = @PoHandle\r\n";
            }

            if (!MyUtility.Check.Empty(model.POSMR))
            {
                where += "AND sp.POSMR = @POSMR\r\n";
            }

            switch (model.PrintType)
            {
                case 1:
                    where += "AND sp.SendDate is null\r\n";
                    break;
                case 2:
                    where += "AND sp.SendDate is not null\r\n";
                    break;
                case 3:
                    where += "AND sp.ReceiveDate is not null\r\n";
                    break;
            }
            #endregion

            string biColumn = string.Empty;
            if (model.IsPowerBI)
            {
                biColumn = @"
    ,sp.Ukey
    ,sp.Article
    ,sp.AddDate
    ,sp.EditDate
    ,[BIFactoryID] = (select top 1 IIF(RgCode = 'PHI', 'PH1', RgCode) from Production.dbo.[System])
    ,[BIInsertDate] = GETDATE()
";
            }

            string sql = $@"
SELECT
    s.BrandID
    ,StyleID = s.ID
    ,s.SeasonID
    ,sp.ProductionKitsGroup
    ,[Mdivision] = sp.MDivisionID
    ,sp.FactoryID
    ,Doc = CONCAT(sp.DOC, '-', r.Name)
    ,[TWSendDate] = sp.SendDate
    ,sp.AWBNO
    ,[FtyMRRcvDate] = sp.ReceiveDate
    ,Reject = IIF(sp.Reject = 1, 'Y', 'N')
    ,[FtySendtoQADate] = sp.SendToQA
    ,[QARcvDate] = sp.QAReceived
    ,[UnnecessaryToSend] = IIF(LEN(ISNULL(sp.ReasonID, '')) = 0, 'N', 'Y')
    ,sp.ProvideDate
    ,[SPNo] = sp.OrderId
    ,sp.SCIDelivery
    ,sp.BuyerDelivery
    ,PullForward = IIF(sp.IsPF = 1, 'Y', 'N')
    ,Handle = CONCAT(sp.SendName, '-', (SELECT Name FROM TPEPass1 WITH (NOLOCK) WHERE ID = sp.SendName))
    ,MRHandle = CONCAT(sp.MRHandle, '-', (SELECT Name FROM TPEPass1 WITH (NOLOCK) WHERE ID = sp.MRHandle))
    ,SMR = CONCAT(sp.SMR, '-', (SELECT Name FROM TPEPass1 WITH (NOLOCK) WHERE ID = sp.SMR))
    ,POHandle = CONCAT(sp.PoHandle, '-', (SELECT Name FROM TPEPass1 WITH (NOLOCK) WHERE ID = sp.PoHandle))
    ,POSMR = CONCAT(sp.POSMR, '-', (SELECT Name FROM TPEPass1 WITH (NOLOCK) WHERE ID = sp.POSMR))
    ,FtyHandle = CONCAT(sp.FtyHandle, '-', (SELECT Name FROM Pass1 WITH (NOLOCK) WHERE ID = sp.FtyHandle))
    {biColumn}
FROM Style_ProductionKits sp WITH (NOLOCK)
INNER JOIN Style s WITH (NOLOCK) ON s.Ukey = sp.StyleUkey
LEFT JOIN Reason r WITH (NOLOCK) ON r.ReasonTypeID = 'ProductionKits' AND r.ID = sp.DOC
WHERE 1 = 1
{where}
";
            Base_ViewModel resultReport = new Base_ViewModel
            {
                Result = DBProxy.Current.Select("Production", sql, listPar, out DataTable dataTables),
            };

            if (!resultReport.Result)
            {
                return resultReport;
            }

            resultReport.Dt = dataTables;
            return resultReport;
        }
    }
}
