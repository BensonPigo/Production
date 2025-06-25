using Sci.Data;
using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Sci.Production.Prg.PowerBI.DataAccess
{
    /// <inheritdoc/>
    public class P_Import_SimilarStyle
    {
        private DBProxy DBProxy;

        /// <inheritdoc/>
        public Base_ViewModel P_SimilarStyle(ExecutedList item)
        {
            this.DBProxy = new DBProxy()
            {
                DefaultTimeout = 1800,
            };

            Base_ViewModel finalResult = new Base_ViewModel();

            if (!item.SDate.HasValue)
            {
                item.SDate = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd"));
            }

            try
            {
                finalResult = this.GetSimilarStyleData(item);
                if (!finalResult.Result)
                {
                    throw finalResult.Result.GetException();
                }

                DataTable detailTable = finalResult.Dt;

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
            Base_ViewModel finalResult = new Base_ViewModel();
            DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);

            string where = @"  Not exists ( select 1 
				   from #tmp t
			       where p.OutputDate = t.OutputDate 
				   and p.FactoryID = t.FactoryID
				   and p.StyleID = t.StyleID 
				   and p.BrandID = t.BrandID 
                )
And p.OutputDate >= @Date";

            string tmp = new Base().SqlBITableHistory("P_SimilarStyle", "P_SimilarStyle_History", "#tmp", where, false, false);

            List<SqlParameter> lisSqlParameter = new List<SqlParameter>
            {
                new SqlParameter("@Date", item.SDate),
            };

            using (sqlConn)
            {
                string sql = $@" 
Update p Set Remark = isnull(t.Remark, ''), 
             RemarkSimilarStyle = isnull(t.RemarkSimilarStyle, ''),
             Type = t.Type,
             BIFactoryID = t.BIFactoryID,
             BIInsertDate = t.BIInsertDate
From P_SimilarStyle p
inner join #tmp t on p.OutputDate = t.OutputDate 
                 and p.FactoryID = t.FactoryID 
                 and p.StyleID = t.StyleID
                 and p.BrandID = t.BrandID
                 and (isnull(p.Remark,'') != isnull(t.Remark,'') 
                   or isnull(p.RemarkSimilarStyle,'') != isnull(t.RemarkSimilarStyle,'') 
                   or p.Type != t.type)


Insert into P_SimilarStyle ( OutputDate,
                             FactoryID, 
                             StyleID, 
                             BrandID, 
                             Remark, 
                             RemarkSimilarStyle, 
                             Type,
                             BIFactoryID,
                             BIInsertDate
                            )
Select  OutputDate,
        FactoryID, 
        StyleID, 
        BrandID, 
        isnull(Remark, ''), 
        isnull(RemarkSimilarStyle, ''), 
        Type,
        BIFactoryID,
        BIInsertDate
From #tmp t
Where not exists ( select 1 
				   from P_SimilarStyle p
				   where p.OutputDate = t.OutputDate 
				   and p.FactoryID = t.FactoryID
				   and p.StyleID = t.StyleID 
				   and p.BrandID = t.BrandID
                )

{tmp}

Delete P_SimilarStyle 
Where Not exists ( select 1 
				   from #tmp t
			       where P_SimilarStyle.OutputDate = t.OutputDate 
				   and P_SimilarStyle.FactoryID = t.FactoryID
				   and P_SimilarStyle.StyleID = t.StyleID 
				   and P_SimilarStyle.BrandID = t.BrandID 
                )
And P_SimilarStyle.OutputDate >= @Date
";

                finalResult.Result = TransactionClass.ProcessWithDatatableWithTransactionScope(dt, null, sql,  out DataTable dataTable, conn: sqlConn, paramters: lisSqlParameter);
            }

            return finalResult;
        }

        private Base_ViewModel GetSimilarStyleData(ExecutedList item)
        {
            StringBuilder sqlCmd = new StringBuilder();

            #region SQL

            // 基本資料
            sqlCmd.Append($@"
SELECT DISTINCT
    o.StyleUkey,
    o.StyleID, 
    s.FactoryID,
    o.BrandID,
    s.OutputDate,
    s.Shift,
    s.Team,
    sd.ComboType
INTO #tmp_SewingDate
FROM SewingOutput s WITH (NOLOCK)
JOIN SewingOutput_Detail sd WITH (NOLOCK) ON s.ID = sd.ID
JOIN Orders o WITH (NOLOCK) ON sd.OrderId = o.ID
WHERE s.OutputDate >= @DATE

SELECT  
    sdate.OutputDate,
    o.StyleID, 
    o.BrandID,
    s.FactoryID,
    MaxOutputDate = MAX(s.OutputDate)
INTO #tmp_MaxDates
FROM SewingOutput s WITH (NOLOCK)
JOIN SewingOutput_Detail sd WITH (NOLOCK) ON s.ID = sd.ID
JOIN Orders o WITH (NOLOCK) ON sd.OrderId = o.ID
JOIN #tmp_SewingDate sdate 
    ON sdate.BrandID = o.BrandID 
   AND sdate.FactoryID = s.FactoryID 
   AND sdate.StyleID = o.StyleID 
WHERE s.OutputDate BETWEEN DATEADD(MONTH, -3, sdate.OutputDate) AND sdate.OutputDate
  AND s.OutputDate < sdate.OutputDate
GROUP BY sdate.OutputDate, o.StyleID, s.FactoryID, o.BrandID


SELECT 
    o.StyleID, 
    s.FactoryID,
    o.BrandID,
    md.OutputDate,
    SewingLineID = MIN(s.SewingLineID)
INTO #tmp_MinSewingID
FROM SewingOutput s WITH (NOLOCK)
JOIN SewingOutput_Detail sd WITH (NOLOCK) ON s.ID = sd.ID
JOIN Orders o WITH (NOLOCK) ON sd.OrderId = o.ID
JOIN #tmp_MaxDates md ON o.StyleID = md.StyleID 
                       AND s.FactoryID = md.FactoryID 
                       AND s.OutputDate = md.MaxOutputDate 
                       AND o.BrandID = md.BrandID
GROUP BY o.StyleID, s.FactoryID, o.BrandID, md.OutputDate


SELECT DISTINCT 
    sda.OutputDate,
    s.FactoryID,
    o.BrandID,
    MasterStyleID = m.MasterStyleID,
    MasterBrandID = m.MasterBrandID,
    MainStyleID = o.StyleID
INTO #tmp_childstyle
FROM SewingOutput s WITH (NOLOCK)
JOIN SewingOutput_Detail sd WITH (NOLOCK) ON s.ID = sd.ID
JOIN Orders o WITH (NOLOCK) ON sd.OrderId = o.ID
JOIN #tmp_SewingDate sda ON sda.StyleID = o.StyleID 
                       AND sda.BrandID = o.BrandID 
                       AND sda.FactoryID = s.FactoryID 
                       AND sda.OutputDate = s.OutputDate
OUTER APPLY (
    SELECT DISTINCT MasterBrandID, MasterStyleID
    FROM (
        SELECT MasterBrandID, MasterStyleID
        FROM Style_SimilarStyle s2 WITH (NOLOCK)
        WHERE EXISTS (
            SELECT 1 FROM Style s WITH (NOLOCK)
            WHERE s.Ukey = o.StyleUkey 
              AND s2.MasterStyleID = s.ID 
              AND s2.MasterBrandID = s.BrandID
        )
        UNION ALL
        SELECT ChildrenBrandID AS MasterBrandID, ChildrenStyleID AS MasterStyleID
        FROM Style_SimilarStyle s2 WITH (NOLOCK)
        WHERE EXISTS (
            SELECT 1 FROM Style s WITH (NOLOCK)
            WHERE s.Ukey = o.StyleUkey 
              AND s2.MasterStyleID = s.ID 
              AND s2.ChildrenBrandID = s.BrandID
        )
    ) m
) m

SELECT  
    cs.OutputDate,
    cs.MasterStyleID, 
    cs.MasterBrandID,
    s.FactoryID,
    MaxOutputDate = MAX(s.OutputDate)
INTO #tmp_childMaxDates
FROM SewingOutput s WITH (NOLOCK)
JOIN SewingOutput_Detail sd WITH (NOLOCK) ON s.ID = sd.ID
JOIN Orders o WITH (NOLOCK) ON sd.OrderId = o.ID
JOIN #tmp_childstyle cs ON cs.MasterBrandID = o.BrandID 
                       AND cs.FactoryID = s.FactoryID 
                       AND cs.MasterStyleID = o.StyleID
WHERE s.OutputDate < cs.OutputDate
GROUP BY cs.MasterStyleID, s.FactoryID, cs.MasterBrandID, cs.OutputDate


SELECT 
    cs.MasterStyleID,  
    s.FactoryID,
    cs.MasterBrandID,
    cs.OutputDate,
    SewingLineID = MIN(s.SewingLineID)
INTO #tmp_childMinSewingID
FROM SewingOutput s WITH (NOLOCK)
JOIN SewingOutput_Detail sd WITH (NOLOCK) ON s.ID = sd.ID
JOIN Orders o WITH (NOLOCK) ON sd.OrderId = o.ID
JOIN #tmp_childMaxDates cs ON o.StyleID = cs.MasterStyleID 
                           AND s.FactoryID = cs.FactoryID 
                           AND s.OutputDate = cs.MaxOutputDate 
                           AND o.BrandID = cs.MasterBrandID
GROUP BY cs.MasterStyleID, s.FactoryID, cs.MasterBrandID, cs.OutputDate

SELECT 
    s.OutputDate,
    s.FactoryID,
    ISNULL(s.StyleID, '') AS StyleID,
    s.BrandID,
    Remark = MinSewingID.SewingLineID + '(' + CONVERT(VARCHAR, MaxDates.MaxOutputDate, 111) + ')',
    RemarkSimilarStyle = RemarkSimilarStyle.Rr,
    [Type] = CASE 
                WHEN ISNULL(RemarkSimilarStyle.Rr, '') != '' 
                  OR ISNULL(MaxDates.MaxOutputDate, '') != '' 
                     THEN 'Repeat'
                ELSE 'New'
             END,
    BIFactoryID = @BIFactoryID,
    BIInsertDate = GETDATE()
FROM #tmp_SewingDate s
LEFT JOIN #tmp_MaxDates MaxDates ON s.StyleID = MaxDates.StyleID 
                                  AND s.FactoryID = MaxDates.FactoryID
                                  AND s.BrandID = MaxDates.BrandID
                                  AND s.OutputDate = MaxDates.OutputDate
LEFT JOIN #tmp_MinSewingID MinSewingID ON MaxDates.StyleID = MinSewingID.StyleID 
                                      AND s.FactoryID = MinSewingID.FactoryID
                                      AND s.OutputDate = MinSewingID.OutputDate
                                      AND MinSewingID.BrandID = s.BrandID
LEFT JOIN #tmp_childstyle cs ON cs.OutputDate = s.OutputDate 
                             AND cs.FactoryID = s.FactoryID 
                             AND cs.BrandID = s.BrandID 
                             AND cs.MainStyleID = s.StyleID
OUTER APPLY (
    SELECT Rr = STUFF((
        SELECT DISTINCT 
            ',' + cd.MasterStyleID + '→' + csid.SewingLineID + '(' + CONVERT(VARCHAR, cd.MaxOutputDate, 111) + ')'
        FROM #tmp_childMaxDates cd
        JOIN #tmp_childMinSewingID csid ON cd.OutputDate = csid.OutputDate
                                        AND cd.FactoryID = csid.FactoryID 
                                        AND cd.MasterBrandID = csid.MasterBrandID
                                        AND cd.MasterStyleID = csid.MasterStyleID 
        JOIN #tmp_childstyle cs ON cd.OutputDate = cs.OutputDate 
                                AND cd.FactoryID = cs.FactoryID 
                                AND cd.MasterBrandID = cs.MasterBrandID 
                                AND cs.MasterStyleID = cd.MasterStyleID
        WHERE cd.FactoryID = s.FactoryID
          AND cd.MasterBrandID = s.BrandID
          AND cd.MasterStyleID != s.StyleID
          AND cs.MainStyleID = s.StyleID
          AND cd.OutputDate = s.OutputDate
          AND cd.MaxOutputDate BETWEEN DATEADD(MONTH, -3, cs.OutputDate) AND cs.OutputDate
        FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 1, '')
) RemarkSimilarStyle
GROUP BY 
    s.OutputDate, s.FactoryID, s.StyleID, s.BrandID, 
    MinSewingID.SewingLineID, MaxDates.MaxOutputDate, RemarkSimilarStyle.Rr
ORDER BY s.OutputDate DESC

");

            #endregion

            List<SqlParameter> paras = new List<SqlParameter>
            {
                new SqlParameter("@Date", item.SDate),
                new SqlParameter("@BIFactoryID", item.RgCode),
            };

            Base_ViewModel resultReport = new Base_ViewModel
            {
                Result = this.DBProxy.Select("Production", sqlCmd.ToString(), paras, out DataTable dt),
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