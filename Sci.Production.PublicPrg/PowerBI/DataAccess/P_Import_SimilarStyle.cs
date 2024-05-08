using Ict;
using Sci.Data;
using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Sci.Production.Prg.PowerBI.DataAccess
{
    /// <inheritdoc/>
    public class P_Import_SimilarStyle
    {
        /// <inheritdoc/>
        public Base_ViewModel P_SimilarStyle(DateTime? sDate)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            SimilarStyle biModel = new SimilarStyle();

            if (!sDate.HasValue)
            {
                sDate = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd"));
            }

            try
            {
                Base_ViewModel resultReport = biModel.GetSimilarStyleData((DateTime)sDate);
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
Delete 
p 
From P_SimilarStyle p
inner join #tmp t on p.OutputDate = t.OutputDate 
and p.FactoryID = t.FactoryID 
and p.StyleID = t.StyleID
and p.BrandID = t.BrandID

Insert into P_SimilarStyle
select t.* from #tmp t
where not exists(	select 1 
					from P_SimilarStyle p
					where p.OutputDate = t.OutputDate 
					and p.FactoryID = t.FactoryID
					and p.StyleID = t.StyleID 
					and p.BrandID = t.BrandID
)

";

                result = MyUtility.Tool.ProcessWithDatatable(dt, null, sql, result: out DataTable dataTable, temptablename: "#tmp", conn: sqlConn, paramters: null);
            }

            finalResult.Result = new DualResult(true);

            return finalResult;
        }
    }
}
