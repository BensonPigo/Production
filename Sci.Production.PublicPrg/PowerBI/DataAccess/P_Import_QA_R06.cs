using Ict;
using Sci.Data;
using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Sci.Production.Prg.PowerBI.DataAccess
{
    /// <summary>
    /// 此BI報表與 PMS/QA/R06已脫鉤 待討論
    /// </summary>
    public class P_Import_QA_R06
    {
        private DBProxy DBProxy;

        /// <inheritdoc/>
        public Base_ViewModel P_QA_R06(DateTime? sDate, DateTime? eDate)
        {
            this.DBProxy = new DBProxy()
            {
                DefaultTimeout = 1800,
            };

            Base_ViewModel finalResult = new Base_ViewModel();

            if (!sDate.HasValue)
            {
                var today = DateTime.Now;
                var firstDayOfCurrentMonth = new DateTime(today.Year, today.Month, 1);
                sDate = firstDayOfCurrentMonth.AddMonths(-6);
            }

            if (!eDate.HasValue)
            {
                var today = DateTime.Now;
                var firstDayOfNextMonth = new DateTime(today.Year, today.Month, 1).AddMonths(1);
                eDate = firstDayOfNextMonth.AddDays(-1);
            }

            try
            {
                finalResult = this.Get_QA_R06_Data((DateTime)sDate, (DateTime)eDate);
                if (!finalResult.Result)
                {
                    throw finalResult.Result.GetException();
                }

                DataTable dataTable = finalResult.Dt;

                finalResult.Result = new Ict.DualResult(true);
            }
            catch (Exception ex)
            {
                finalResult.Result = new Ict.DualResult(false, ex);
            }

            return finalResult;
        }

        private Base_ViewModel Get_QA_R06_Data(DateTime sdate, DateTime edate)
        {
            StringBuilder sqlcmdSP = new StringBuilder();

            sqlcmdSP.Append("exec dbo.P_Import_QA_R06");
            sqlcmdSP.Append($"'{sdate.ToString("yyyy/MM/dd")}',"); // sDate
            sqlcmdSP.Append($"'{edate.ToString("yyyy/MM/dd")}'"); // eDate

            Base_ViewModel resultReport = new Base_ViewModel
            {
                Result = this.DBProxy.Select("PowerBI", sqlcmdSP.ToString(), out DataTable dataTables),
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
