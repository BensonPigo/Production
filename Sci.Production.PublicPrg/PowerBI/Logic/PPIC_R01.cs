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
    public class PPIC_R01
    {
        /// <inheritdoc/>
        public PPIC_R01()
        {
            DBProxy.Current.DefaultTimeout = 1800;
        }

        /// <inheritdoc/>
        public Base_ViewModel GetSewingLineScheduleData(PPIC_R01_ViewModel model)
        {
            List<SqlParameter> listPar = new List<SqlParameter>
            {
                new SqlParameter("@Inline", SqlDbType.Date) { Value = (object)model.Inline.Value ?? DBNull.Value },
                new SqlParameter("@Offline", SqlDbType.Date) { Value = (object)model.Offline.Value ?? DBNull.Value },
                new SqlParameter("@Line1", SqlDbType.VarChar, 10) { Value = model.Line1 },
                new SqlParameter("@Line2", SqlDbType.VarChar, 10) { Value = model.Line2 },
                new SqlParameter("@MDivisionID", SqlDbType.VarChar, 10) { Value = model.MDivisionID },
                new SqlParameter("@FactoryID", SqlDbType.VarChar, 10) { Value = model.FactoryID },
                new SqlParameter("@BuyerDelivery1", SqlDbType.Date) { Value = (object)model.BuyerDelivery1 ?? DBNull.Value },
                new SqlParameter("@BuyerDelivery2", SqlDbType.Date) { Value = (object)model.BuyerDelivery2 ?? DBNull.Value },
                new SqlParameter("@SciDelivery1", SqlDbType.Date) { Value = (object)model.SciDelivery1 ?? DBNull.Value },
                new SqlParameter("@SciDelivery2", SqlDbType.Date) { Value = (object)model.SciDelivery2 ?? DBNull.Value },
                new SqlParameter("@Brand", SqlDbType.VarChar, 10) { Value = model.Brand },
                new SqlParameter("@Subprocess", SqlDbType.VarChar, 20) { Value = model.Subprocess },
                new SqlParameter("@IsPowerBI", SqlDbType.Bit) { Value = model.IsPowerBI },
            };

            string sql = @"
exec dbo.GetSewingLineScheduleData  @Inline,
                                    @Offline,
                                    @Line1,
                                    @Line2,
                                    @MDivisionID,
                                    @FactoryID,
                                    @BuyerDelivery1,
                                    @BuyerDelivery2,
                                    @SciDelivery1,
                                    @SciDelivery2,
                                    @Brand,
                                    @Subprocess,
                                    @IsPowerBI
";
            Base_ViewModel resultReport = new Base_ViewModel
            {
                Result = DBProxy.Current.Select("Production", sql, listPar, out DataTable[] dataTables),
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
