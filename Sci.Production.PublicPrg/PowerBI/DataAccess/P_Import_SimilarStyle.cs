using Ict;
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
        public Base_ViewModel P_SimilarStyle(DateTime? sDate)
        {
            this.DBProxy = new DBProxy()
            {
                DefaultTimeout = 1800,
            };

            Base_ViewModel finalResult = new Base_ViewModel();

            if (!sDate.HasValue)
            {
                sDate = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd"));
            }

            try
            {
                Base_ViewModel resultReport = this.GetSimilarStyleData((DateTime)sDate);
                if (!resultReport.Result)
                {
                    throw resultReport.Result.GetException();
                }

                DataTable detailTable = resultReport.Dt;

                // insert into PowerBI
                finalResult = this.UpdateBIData(detailTable, (DateTime)sDate);
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

        private Base_ViewModel UpdateBIData(DataTable dt, DateTime sdate)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            DualResult result;
            DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);

            List<SqlParameter> lisSqlParameter = new List<SqlParameter>();
            lisSqlParameter.Add(new SqlParameter("@Date", sdate));

            using (sqlConn)
            {
                string sql = $@" 
Update p Set Remark = isnull(t.Remark, ''), 
             RemarkSimilarStyle = isnull(t.RemarkSimilarStyle, ''),
             Type = t.Type
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
                             Type
                            )
Select  OutputDate,
        FactoryID, 
        StyleID, 
        BrandID, 
        isnull(Remark, ''), 
        isnull(RemarkSimilarStyle, ''), 
        Type
From #tmp t
Where not exists ( select 1 
				   from P_SimilarStyle p
				   where p.OutputDate = t.OutputDate 
				   and p.FactoryID = t.FactoryID
				   and p.StyleID = t.StyleID 
				   and p.BrandID = t.BrandID
                )

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
                sql += new Base().SqlBITableInfo("P_SimilarStyle", true);
                result = TransactionClass.ProcessWithDatatableWithTransactionScope(dt, null, sql,  out DataTable dataTable, conn: sqlConn, paramters: lisSqlParameter);
            }

            finalResult.Result = result;

            return finalResult;
        }

        private Base_ViewModel GetSimilarStyleData(DateTime sdate)
        {
            StringBuilder sqlCmd = new StringBuilder();

            #region SQL

            // 基本資料
            sqlCmd.Append($@"
SELECT Distinct
        o.StyleUkey,
        o.StyleID, 
        s.FactoryID,
        o.BrandID,
        s.OutputDate,
        s.Shift,
        s.Team,
        sd.ComboType
INTO #tmp_SewingDate
FROM SewingOutput s with (nolock)
INNER JOIN SewingOutput_Detail sd with (nolock) ON s.ID = sd.ID
INNER JOIN Orders o with (nolock) ON sd.OrderId = o.ID
WHERE s.OutputDate >= @DATE

SELECT  sdate.OutputDate,
        o.StyleID, 
        o.BrandID,
        s.FactoryID,
        MaxOutputDate = max(s.OutputDate)
into #tmp_MaxDates
FROM SewingOutput s with (nolock)
INNER JOIN SewingOutput_Detail sd with (nolock) ON s.ID = sd.ID
INNER JOIN Orders o with (nolock) ON sd.OrderId = o.ID
INNER JOIN #tmp_SewingDate sdate ON sdate.BrandID = o.BrandID and sdate.FactoryID = s.FactoryID and sdate.StyleID = o.StyleID 
WHERE s.OutputDate > DATEADD(month, -3, sdate.OutputDate) 
AND s.OutputDate < sdate.OutputDate
GROUP BY sdate.OutputDate, o.StyleID, s.FactoryID, o.BrandID

SELECT o.StyleID, 
       s.FactoryID,
       o.BrandID,
       md.OutputDate,
       MIN(s.SewingLineID) AS SewingLineID
INTO #tmp_MinSewingID
FROM SewingOutput s with (nolock)
INNER JOIN SewingOutput_Detail sd with (nolock) ON s.ID = sd.ID
INNER JOIN Orders o with (nolock) ON sd.OrderId = o.ID
INNER JOIN #tmp_MaxDates md ON o.StyleID = md.StyleID AND s.FactoryID = md.FactoryID AND s.OutputDate = md.MaxOutputDate AND md.BrandID = o.BrandID
GROUP BY o.StyleID, s.FactoryID, o.BrandID, md.OutputDate


SELECT Distinct sda.OutputDate,
       s.FactoryID,
       o.BrandID,
       m.MasterStyleID,
       m.MasterBrandID,
       MainStyleID = o.StyleID
into #tmp_childstyle
FROM SewingOutput s with (nolock)
INNER JOIN SewingOutput_Detail sd with (nolock) ON s.ID = sd.ID
INNER JOIN Orders o with (nolock) ON sd.OrderId = o.ID
INNER JOIN #tmp_SewingDate sda on sda.StyleID = o.StyleID and sda.BrandID = o.BrandID and sda.FactoryID = s.FactoryID and sda.OutputDate = s.OutputDate 
Outer apply (
                select Distinct MasterBrandID, MasterStyleID 
                from (
                      select MasterBrandID,
                             MasterStyleID 
                      from Style_SimilarStyle s2 WITH (NOLOCK) 
                      where exists( select 1 
                                    from Style s with (nolock) 
                                    where s.Ukey = o.StyleUkey 
                                    and s2.MasterStyleID = s.ID 
                                    and s2.MasterBrandID = s.BrandID)
                       union all
                       select ChildrenBrandID as MasterBrandID, 
                              ChildrenStyleID as MasterStyleID
                       from Style_SimilarStyle s2 WITH (NOLOCK) 
                       where exists( select 1 
                       from Style s with (nolock) 
                       where s.Ukey = o.StyleUkey 
                       and s2.MasterStyleID = s.ID 
                       and s2.ChildrenBrandID = s.BrandID)
                )m
            )m

SELECT  cs.OutputDate,
        cs.MasterStyleID, 
        cs.MasterBrandID,
        s.FactoryID,
        MaxOutputDate = max(s.OutputDate)
INTO #tmp_childMaxDates
FROM SewingOutput s with (nolock)
INNER JOIN SewingOutput_Detail sd with (nolock) ON s.ID = sd.ID
INNER JOIN Orders o with (nolock) ON sd.OrderId = o.ID
INNER JOIN #tmp_childstyle cs  ON cs.MasterBrandID = o.BrandID and cs.FactoryID = s.FactoryID and cs.MasterStyleID = o.StyleID 
WHERE s.OutputDate < cs.OutputDate
GROUP BY cs.MasterStyleID, s.FactoryID, cs.MasterBrandID ,cs.OutputDate 

SELECT  cs.MasterStyleID,  
        s.FactoryID,
        cs.MasterBrandID,
        cs.OutputDate,
        MIN(s.SewingLineID) AS SewingLineID
INTO #tmp_childMinSewingID
FROM SewingOutput s with (nolock)
INNER JOIN SewingOutput_Detail sd with (nolock) ON s.ID = sd.ID
INNER JOIN Orders o with (nolock) ON sd.OrderId = o.ID
INNER JOIN #tmp_childMaxDates cs ON o.StyleID = cs.MasterStyleID AND s.FactoryID = cs.FactoryID AND s.OutputDate = cs.MaxOutputDate And cs.MasterBrandID = o.BrandID
GROUP BY cs.MasterStyleID, s.FactoryID, cs.MasterBrandID, cs.OutputDate

SELECT  s.Outputdate,
        s.FactoryID,
        ISNULL(s.StyleID, '') AS StyleID,
        s.BrandID,
        Remark = MinSewingID.SewingLineID + '(' +  CONVERT(varchar, MaxDates.MaxOutputDate, 111)  + ')',
        RemarkSimilarStyle = RemarkSimilarStyle.Rr,
        [Type] = Case When isnull(RemarkSimilarStyle.Rr,'') != '' or isnull(MaxDates.MaxOutputDate,'') != '' then  'Repeat'
        Else 'New'
        End
FROM #tmp_SewingDate s
LEFT JOIN #tmp_MaxDates MaxDates ON s.StyleID = MaxDates.StyleID 
                                 AND s.FactoryID = MaxDates.FactoryID
                                 AND s.BrandID = MaxDates.BrandID
                                 AND s.OutputDate = MaxDates.OutputDate
                                 AND s.BrandID = MaxDates.BrandID
LEFT JOIN #tmp_MinSewingID MinSewingID ON MaxDates.StyleID = MinSewingID.StyleID 
                                       AND s.FactoryID = MinSewingID.FactoryID
                                       AND s.OutputDate = MinSewingID.OutputDate
                                       AND MinSewingID.BrandID = s.BrandID
LEFT JOIN #tmp_childstyle cs ON cs.OutputDate = s.OutputDate 
                             AND cs.FactoryID = s.FactoryID 
                             AND cs.BrandID = s.BrandID
                             AND cs.MainStyleID = s.StyleID
OUTER APPLY(
            Select Rr = STUFF((
                                SELECT Distinct ',' + cd.MasterStyleID + '→' + csid.SewingLineID + '(' + CONVERT(varchar, cd.MaxOutputDate, 111) + ')'
                                FROM #tmp_childMaxDates cd
                                INNER JOIN #tmp_childMinSewingID csid ON cd.OutputDate = csid.OutputDate
                                                                      AND cd.FactoryID = csid.FactoryID 
                                                                      AND cd.MasterBrandID = csid.MasterBrandID
                                                                      AND cd.MasterStyleID = csid.MasterStyleID 
                                INNER JOIN #tmp_childstyle cs ON cd.OutputDate = cs.OutputDate 
                                                              AND cd.FactoryID = cs.FactoryID 
                                                              AND cd.MasterBrandID = cs.MasterBrandID 
                                                              AND cs.MasterStyleID = cd.MasterStyleID
                                WHERE  cd.FactoryID = s.FactoryID
                                AND cd.MasterBrandID = s.BrandID
								AND cd.MasterStyleID != s.StyleID
                                AND cs.MainStyleID = s.StyleID
                                AND cd.OutputDate = s.OutputDate
                                AND cd.MaxOutputDate Between DATEADD(month, -3, cs.OutputDate) AND cs.OutputDate
            FOR XML PATH ('')), 1, 1, '' )
) RemarkSimilarStyle
GROUP BY s.Outputdate, s.FactoryID, s.StyleID, s.BrandID, MinSewingID.SewingLineID, MaxDates.MaxOutputDate, RemarkSimilarStyle.Rr
ORDER BY s.outputdate DESC
");

            #endregion

            List<SqlParameter> paras = new List<SqlParameter>();
            paras.Add(new SqlParameter("@Date", sdate));

            Base_ViewModel resultReport = new Base_ViewModel
            {
                Result = this.DBProxy.Select("Production", sqlCmd.ToString(), paras, out DataTable dataTables),
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