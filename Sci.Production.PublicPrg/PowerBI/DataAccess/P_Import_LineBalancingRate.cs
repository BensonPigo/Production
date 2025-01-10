using Ict;
using Sci.Data;
using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;
using System;

namespace Sci.Production.Prg.PowerBI.DataAccess
{
    /// <inheritdoc/>
    public class P_Import_LineBalancingRate
    {
        /// <summary>
        /// 寫入 P_LineBalancingRate
        /// </summary>
        /// <param name="sDate">Start Date</param>
        /// <param name="eDate">End Data</param>
        /// <returns>Base_ViewModel</returns>
        public Base_ViewModel P_LineBalancingRate(DateTime? sDate, DateTime? eDate)
        {
            Base_ViewModel finalResult = new Base_ViewModel();

            if (!sDate.HasValue)
            {
                sDate = DateTime.Parse(DateTime.Now.AddDays(-1).ToString("yyyy/MM/dd"));
            }

            if (!eDate.HasValue)
            {
                eDate = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd"));
            }

            try
            {
                finalResult.Result = this.ImportLineBalancingRate(sDate.Value, eDate.Value);
            }
            catch (Exception ex)
            {
                finalResult.Result = new DualResult(false, ex);
            }

            return finalResult;
        }

        private DualResult ImportLineBalancingRate(DateTime sDate, DateTime eDate)
        {
            DualResult finalResult = new DualResult(true);
            try
            {
                for (DateTime date = sDate; date <= eDate; date = date.AddDays(1))
                {
                    string tsql = $"exec P_Import_LineBalancingRate '{date.ToString("yyyy/MM/dd")}'";
                    finalResult = TransactionClass.ExecuteTransactionScope("PowerBI", tsql);
                    if (!finalResult)
                    {
                        throw finalResult.GetException();
                    }
                }
            }
            catch (Exception ex)
            {
                finalResult = new DualResult(false, ex);
            }

            return finalResult;
        }
    }
}
