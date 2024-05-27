using Ict;
using Sci.Data;
using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Sci.Production.Prg.PowerBI.DataAccess
{
    /// <inheritdoc/>
    public class P_Import_SimilarStyle
    {
        /// <inheritdoc/>
        public Base_ViewModel P_SimilarStyle(DateTime? sDate)
        {
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

                DataTable detailTable = resultReport.DtArr[0];
                if (!resultReport.Result)
                {
                    throw resultReport.Result.GetException();
                }

                // insert into PowerBI
                finalResult = this.UpdateBIData(detailTable);
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

        private Base_ViewModel UpdateBIData(DataTable dt)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            DualResult result;
            DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);
            using (sqlConn)
            {
                string sql = @"	
    Update p Set Remark = t.Remark, RemarkSimilarStyle = t.RemarkSimilarStyle, Type = t.Type
      From P_SimilarStyle p
inner join #tmp t on p.OutputDate = t.OutputDate 
                 and p.FactoryID = t.FactoryID 
                 and p.StyleID = t.StyleID
                 and p.BrandID = t.BrandID
                 and (p.Remark != t.Remark or p.RemarkSimilarStyle != t.RemarkSimilarStyle or p.Type != t.type)


    Insert into P_SimilarStyle ( OutputDate,
                                 FactoryID, 
                                 StyleID, 
                                 BrandID, 
                                 Remark, 
                                 RemarkSimilarStyle, 
                                 Type
                                )
                         select  OutputDate,
                                 FactoryID, 
                                 StyleID, 
                                 BrandID, 
                                 Remark, 
                                 RemarkSimilarStyle, 
                                 Type
                           from #tmp t
                          where not exists ( select 1 
					                           from P_SimilarStyle p
					                          where p.OutputDate = t.OutputDate 
					                            and p.FactoryID = t.FactoryID
					                            and p.StyleID = t.StyleID 
					                            and p.BrandID = t.BrandID
                                           )

    Delete P_SimilarStyle 
     where Not exists ( select 1 
				          from #tmp t
			     	     where P_SimilarStyle.OutputDate = t.OutputDate 
					       and P_SimilarStyle.FactoryID = t.FactoryID
					       and P_SimilarStyle.StyleID = t.StyleID 
					       and P_SimilarStyle.BrandID = t.BrandID 
                       )
";

                result = MyUtility.Tool.ProcessWithDatatable(dt, null, sql, result: out DataTable dataTable, temptablename: "#tmp", conn: sqlConn, paramters: null);
            }

            finalResult.Result = new DualResult(true);

            return finalResult;
        }

        private Base_ViewModel GetSimilarStyleData(DateTime sdate)
        {
            StringBuilder sqlCmd = new StringBuilder();
            #region Where

            string where = $@" and s.OutputDate >= '{sdate.ToString("yyyy-MM-dd")}'";

            #endregion

            #region SQL

            // 基本資料
            sqlCmd.Append($@"
WITH 
SewingDate AS
(
        SELECT o.StyleUkey,
               o.StyleID, 
               s.FactoryID,
	           o.BrandID,
               s.OutputDate,
		       s.Shift,
		       s.Team,
	           sd.ComboType
          FROM SewingOutput s
    INNER JOIN SewingOutput_Detail sd ON s.ID = sd.ID
    INNER JOIN Orders o ON sd.OrderId = o.ID
         WHERE 1=1
    {where}
      GROUP BY s.ID, s.OutputDate, o.StyleID, s.FactoryID, o.BrandID, o.StyleUkey,s.Shift,s.Team,sd.ComboType
),
MaxDates as(
            SELECT distinct  sdate.OutputDate,
	                o.StyleID, 
		            o.BrandID,
                    s.FactoryID,
		            MaxOutputDate = max(s.OutputDate)
                FROM SewingOutput s
        INNER JOIN SewingOutput_Detail sd ON s.ID = sd.ID
        INNER JOIN Orders o ON sd.OrderId = o.ID
	    INNER JOIN SewingDate sdate ON sdate.BrandID = o.BrandID and sdate.FactoryID = s.FactoryID and sdate.StyleID = o.StyleID 
	            WHERE  s.OutputDate < sdate.OutputDate
            GROUP BY sdate.OutputDate, o.StyleID, s.FactoryID, o.BrandID
),
MinSewingID AS (
                SELECT Distinct
                        o.StyleID, 
                        s.FactoryID,
		                o.BrandID,
                        md.OutputDate,
                        MIN(s.SewingLineID) AS SewingLineID
                    FROM SewingOutput s
              INNER JOIN SewingOutput_Detail sd ON s.ID = sd.ID
              INNER JOIN Orders o ON sd.OrderId = o.ID
	          INNER JOIN MaxDates md ON o.StyleID = md.StyleID 
                                    AND s.FactoryID = md.FactoryID
                                    AND s.OutputDate = md.MaxOutputDate
						            AND md.BrandID = o.BrandID
                GROUP BY o.StyleID, s.FactoryID, o.BrandID, md.OutputDate
) ,
childstyle as(
              SELECT Distinct
		             sda.OutputDate,
		             s.FactoryID,
	                 o.BrandID,
	                 m.MasterStyleID,
	                 m.MasterBrandID,
		             MainStyleID = o.StyleID
                FROM SewingOutput s
          INNER JOIN SewingOutput_Detail sd ON s.ID = sd.ID
          INNER JOIN Orders o ON sd.OrderId = o.ID
	      INNER JOIN SewingDate sda on sda.StyleID = o.StyleID and sda.BrandID = o.BrandID and sda.FactoryID = s.FactoryID and sda.OutputDate = s.OutputDate 
	     Outer apply (
		                select distinct MasterBrandID, MasterStyleID 
		                from (
			                select MasterBrandID,
                                   MasterStyleID 
			                  from Style_SimilarStyle s2 WITH (NOLOCK) 
			                 where exists( select 1 
                                             from Style s with (nolock) 
							                where s.Ukey = o.StyleUkey 
                                              and s2.MasterStyleID = s.ID 
                                              and s2.MasterBrandID = s.BrandID
                                          )
			                union all
			                select ChildrenBrandID as MasterBrandID, 
                                   ChildrenStyleID as MasterStyleID
			                  from Style_SimilarStyle s2 WITH (NOLOCK) 
			                 where exists( select 1 
                                             from Style s with (nolock) 
							                where s.Ukey = o.StyleUkey 
                                              and s2.MasterStyleID = s.ID 
                                              and s2.ChildrenBrandID = s.BrandID
                                          )
		                        )m
	                      )m
) ,
childMaxDates as(
                  SELECT cs.OutputDate,
	                       cs.MasterStyleID, 
		                   cs.MasterBrandID,
                           s.FactoryID,
		                   MaxOutputDate = max(s.OutputDate)
                    FROM SewingOutput s
              INNER JOIN SewingOutput_Detail sd ON s.ID = sd.ID
              INNER JOIN Orders o ON sd.OrderId = o.ID
	          INNER JOIN childstyle cs ON cs.MasterBrandID = o.BrandID 
                                      and cs.FactoryID = s.FactoryID 
                                      and cs.MasterStyleID = o.StyleID 
	               WHERE s.OutputDate < cs.OutputDate
                GROUP BY cs.MasterStyleID, s.FactoryID, cs.MasterBrandID ,cs.OutputDate 
) ,
childMinSewingID AS (
                    SELECT  cs.MasterStyleID,  
                            s.FactoryID,
		                    cs.MasterBrandID,
                            cs.OutputDate,
                            MIN(s.SewingLineID) AS SewingLineID
                      FROM SewingOutput s
                INNER JOIN SewingOutput_Detail sd ON s.ID = sd.ID
                INNER JOIN Orders o ON sd.OrderId = o.ID
	            INNER JOIN childMaxDates cs ON o.StyleID = cs.MasterStyleID 
                                           AND s.FactoryID = cs.FactoryID
                                           AND s.OutputDate = cs.MaxOutputDate
						                   And cs.MasterBrandID = o.BrandID
                  GROUP BY cs.MasterStyleID, s.FactoryID, cs.MasterBrandID, cs.OutputDate
)

   SELECT DISTINCT s.Outputdate,
                   s.FactoryID,
                   ISNULL(s.StyleID, '') AS StyleID,
                   s.BrandID,
                   Remark = MinSewingID.SewingLineID + '(' +  CONVERT(varchar, MaxDates.MaxOutputDate, 111)  + ')',
	               RmarkSimilarStyle = RmarkSimilarStyle.Rr,
	               [Type] = Case When MaxDates.MaxOutputDate between DateADD(MONTH, -3 ,s.OutputDate) and  s.OutputDate then 'Repeat'
	                             When isnull(RmarkSimilarStyle.Rr,'') != '' then  'Repeat'
			                     Else 'New'
			                     End
     FROM SewingDate s
Left JOIN MaxDates ON s.StyleID = MaxDates.StyleID 
				   AND s.FactoryID = MaxDates.FactoryID
				   AND s.BrandID = MaxDates.BrandID
				   AND s.OutputDate = MaxDates.OutputDate
				   AND s.BrandID = MaxDates.BrandID
Left JOIN MinSewingID ON MaxDates.StyleID = MinSewingID.StyleID 
                          AND s.FactoryID = MinSewingID.FactoryID
                          AND s.OutputDate = MinSewingID.OutputDate
						  AND MinSewingID.BrandID = s.BrandID
Left JOIN childstyle cs ON cs.OutputDate = s.OutputDate and cs.FactoryID = s.FactoryID and cs.BrandID = s.BrandID and cs.MainStyleID = s.StyleID
Outer Apply(
            Select Rr = STUFF((
		                        SELECT distinct ',' + cd.MasterStyleID + '→' + csid.SewingLineID + '(' + CONVERT(varchar, cd.MaxOutputDate, 111) + ')'
		                          FROM childMaxDates cd
                            INNER JOIN childMinSewingID csid ON cd.MasterBrandID = csid.MasterBrandID 
                                                            AND cd.FactoryID = csid.FactoryID 
                                                            AND cd.MasterStyleID = csid.MasterStyleID 
                                                            AND cd.OutputDate = csid.OutputDate
                            INNER JOIN childstyle cs ON cd.OutputDate = cs.OutputDate 
                                                    AND cd.FactoryID = cs.FactoryID 
                                                    AND cd.MasterBrandID = cs.MasterBrandID 
                                                    AND cs.MainStyleID = s.StyleID
                                 WHERE  cd.FactoryID = s.FactoryID
                                   AND cd.MasterBrandID = s.BrandID
                                   AND cs.MainStyleID = s.StyleID
                                   AND cd.OutputDate = s.OutputDate
		                           AND cd.MaxOutputDate Between DATEADD(month, -3, cs.OutputDate) and cs.OutputDate
		            for xml path ('')), 1, 1, '' )
) RmarkSimilarStyle
");

            #endregion

            Base_ViewModel resultReport = new Base_ViewModel
            {
                Result = DBProxy.Current.Select("Production", sqlCmd.ToString(), out DataTable[] dataTables),
            };

            if (!resultReport.Result)
            {
                return resultReport;
            }

            resultReport.DtArr = dataTables;
            return resultReport;
        }
    }
}