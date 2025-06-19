using Sci.Data;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;

namespace Sci.Production.Prg.PowerBI.Logic
{
    /// <inheritdoc/>
    public class Warehouse_R16
    {
        private DBProxy DBProxy;

        /// <inheritdoc/>
        public Warehouse_R16()
        {
            this.DBProxy = new DBProxy()
            {
                DefaultTimeout = 7200,
            };
        }

        /// <inheritdoc/>
        public Base_ViewModel GetIssueFabricByCuttingTransactionList(Warehouse_R16_ViewModel model)
        {
            List<SqlParameter> listPar = new List<SqlParameter>
            {
                new SqlParameter("@IssueDateFrom", SqlDbType.Date) { Value = (object)model.IssueDateFrom ?? DBNull.Value },
                new SqlParameter("@IssueDateTo", SqlDbType.Date) { Value = (object)model.IssueDateTo ?? DBNull.Value },
                new SqlParameter("@MDivisionID", SqlDbType.VarChar) { Value = model.MDivisionID },
                new SqlParameter("@FactoryID", SqlDbType.VarChar) { Value = model.FactoryID },
                new SqlParameter("@CutplanIDFrom", SqlDbType.VarChar) { Value = model.CutplanIDFrom },
                new SqlParameter("@CutplanIDTo", SqlDbType.VarChar) { Value = model.CutplanIDTo },
                new SqlParameter("@SPFrom", SqlDbType.VarChar) { Value = model.SPFrom },
                new SqlParameter("@SPTo", SqlDbType.VarChar) { Value = model.SPTo },
                new SqlParameter("@EditDateFrom", SqlDbType.Date) { Value = (object)model.EditDateFrom ?? DBNull.Value },
                new SqlParameter("@EditDateTo", SqlDbType.Date) { Value = (object)model.EditDateTo ?? DBNull.Value },
            };

            string sql = @"
exec dbo.Warehouse_Report_R16  @IssueDateFrom,
                               @IssueDateTo,
                               @MDivisionID,
                               @FactoryID,
                               @CutplanIDFrom,
                               @CutplanIDTo,
                               @SPFrom,   
                               @SPTo,
                               @EditDateFrom,
                               @EditDateTo
";
            Base_ViewModel resultReport = new Base_ViewModel
            {
                Result = this.DBProxy.Select("Production", sql, listPar, out DataTable dataTable),
            };

            if (!resultReport.Result)
            {
                return resultReport;
            }

            resultReport.Dt = dataTable;
            return resultReport;
        }
    }
}
