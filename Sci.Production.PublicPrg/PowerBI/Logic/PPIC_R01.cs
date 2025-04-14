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
        private DBProxy DBProxy;

        /// <inheritdoc/>
        public PPIC_R01()
        {
            this.DBProxy = new DBProxy()
            {
                DefaultTimeout = 7200,
            };
        }

        /// <inheritdoc/>
        public Base_ViewModel GetSewingLineScheduleData(PPIC_R01_ViewModel model)
        {
            List<SqlParameter> listPar = new List<SqlParameter>
            {
                new SqlParameter("@Inline", SqlDbType.Date) { Value = (object)model.Inline ?? DBNull.Value },
                new SqlParameter("@Offline", SqlDbType.Date) { Value = (object)model.Offline ?? DBNull.Value },
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
                Result = this.DBProxy.Select("Production", sql, listPar, out DataTable[] dataTables),
            };

            if (!resultReport.Result)
            {
                return resultReport;
            }

            if (!model.IsPowerBI)
            {
                dataTables[0].Columns.Remove("LastDownloadAPSDate");
            }

            resultReport.DtArr = dataTables;
            return resultReport;
        }

        /// <inheritdoc/>
        public Base_ViewModel GetSewingLineScheduleDataBySP(PPIC_R01bySP_ViewModel model)
        {
            List<SqlParameter> listPar = new List<SqlParameter>
            {
                new SqlParameter("@MDivisionID", SqlDbType.VarChar, 8) { Value = model.MDivisionID },
                new SqlParameter("@FactoryID", SqlDbType.VarChar, 8) { Value = model.FactoryID },
                new SqlParameter("@SewingLineIDFrom", SqlDbType.VarChar, 5) { Value = model.SewingLineIDFrom },
                new SqlParameter("@SewingLineIDTo", SqlDbType.VarChar, 5) { Value = model.SewingLineIDTo },
                new SqlParameter("@SewingDateFrom", SqlDbType.Date) { Value = (object)model.SewingDateFrom.Value ?? DBNull.Value },
                new SqlParameter("@SewingDateTo", SqlDbType.Date) { Value = (object)model.SewingDateTo.Value ?? DBNull.Value },
                new SqlParameter("@BuyerDeliveryFrom", SqlDbType.Date) { Value = (object)model.BuyerDeliveryFrom ?? DBNull.Value },
                new SqlParameter("@BuyerDeliveryTo", SqlDbType.Date) { Value = (object)model.BuyerDeliveryTo ?? DBNull.Value },
                new SqlParameter("@SciDeliveryFrom", SqlDbType.Date) { Value = (object)model.SciDeliveryFrom ?? DBNull.Value },
                new SqlParameter("@SciDeliveryTo", SqlDbType.Date) { Value = (object)model.SciDeliveryTo ?? DBNull.Value },
                new SqlParameter("@BrandID", SqlDbType.VarChar, 8) { Value = model.BrandID },
                new SqlParameter("@SubProcess", SqlDbType.VarChar, 20) { Value = model.SubProcess },
                new SqlParameter("@IsPowerBI", SqlDbType.Bit) { Value = model.IsPowerBI },
                new SqlParameter("@sEditDate", SqlDbType.Date) { Value = DateTime.Now.AddDays(-7) },
                new SqlParameter("@eEditDate", SqlDbType.Date) { Value = DateTime.Now.AddDays(1) },
            };

            string sql = @"
exec dbo.PPIC_R01_SewingLineScheduleBySP  @MDivisionID,
                                    @FactoryID,
                                    @SewingLineIDFrom,
                                    @SewingLineIDTo,
                                    @SewingDateFrom,
                                    @SewingDateTo,
                                    @BuyerDeliveryFrom,
                                    @BuyerDeliveryTo,
                                    @SciDeliveryFrom,
                                    @SciDeliveryTo,
                                    @BrandID,
                                    @SubProcess,
                                    @sEditDate,
                                    @eEditDate,
                                    @IsPowerBI";
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
