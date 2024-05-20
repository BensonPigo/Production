using Sci.Data;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sci.Production.Prg.PowerBI.Logic
{
    /// <inheritdoc/>
    public class QA_R01
    {
        /// <inheritdoc/>
        public QA_R01()
        {
            DBProxy.Current.DefaultTimeout = 1800;
        }

        /// <inheritdoc/>
        public Base_ViewModel Get_QA_R01(QA_R01_ViewModel model)
        {

            List<SqlParameter> listPar = new List<SqlParameter>
            {
                new SqlParameter("@StartInstPhysicalInspDate", SqlDbType.Date) { Value = (object)model.StartInstPhysicalInspDate.Value ?? DBNull.Value },
                new SqlParameter("@EndInstPhysicalInspDate", SqlDbType.Date) { Value = (object)model.EndInstPhysicalInspDate.Value ?? DBNull.Value },

                new SqlParameter("@StartArriveWHDate", SqlDbType.Date) { Value = (object)model.StartArriveWHDate.Value ?? DBNull.Value },
                new SqlParameter("@EndArriveWHDate", SqlDbType.Date) { Value = (object)model.EndArriveWHDate.Value ?? DBNull.Value },

                new SqlParameter("@StartSciDelivery", SqlDbType.Date) { Value = (object)model.StartSciDelivery.Value ?? DBNull.Value },
                new SqlParameter("@EndSciDelivery", SqlDbType.Date) { Value = (object)model.EndSciDelivery.Value ?? DBNull.Value },

                new SqlParameter("@StartSewingInLineDate", SqlDbType.Date) { Value = (object)model.StartSewingInLineDate.Value ?? DBNull.Value },
                new SqlParameter("@EndSewingInLineDate", SqlDbType.Date) { Value = (object)model.EndSewingInLineDate.Value ?? DBNull.Value },

                new SqlParameter("@StratEstCuttingDate", SqlDbType.Date) { Value = (object)model.StratEstCuttingDate.Value ?? DBNull.Value },
                new SqlParameter("@EndEstCuttingDate", SqlDbType.Date) { Value = (object)model.EndEstCuttingDate.Value ?? DBNull.Value },

                new SqlParameter("@StratWK", SqlDbType.Date) { Value = (object)model.StratWK ?? DBNull.Value },
                new SqlParameter("@EndWK", SqlDbType.Date) { Value = (object)model.EndWK ?? DBNull.Value },

                new SqlParameter("@StartSP", SqlDbType.VarChar, 15) { Value = (object)model.StartSP ?? DBNull.Value },
                new SqlParameter("@EndSP", SqlDbType.VarChar, 15) { Value = (object)model.EndSP ?? DBNull.Value },

                new SqlParameter("@Season", SqlDbType.VarChar, 15) { Value = (object)model.Season ?? DBNull.Value },

                new SqlParameter("@Brand", SqlDbType.VarChar, 15) { Value = (object)model.Brand ?? DBNull.Value },

                new SqlParameter("@Refno", SqlDbType.VarChar, 15) { Value = (object)model.Refno ?? DBNull.Value },

                new SqlParameter("@Category", SqlDbType.VarChar, 15) { Value = (object)model.Category ?? DBNull.Value },

                new SqlParameter("@Supplier", SqlDbType.VarChar, 15) { Value = (object)model.Supplier ?? DBNull.Value },

                new SqlParameter("@OverallResultStatus", SqlDbType.VarChar, 15) { Value = (object)model.OverallResultStatus ?? DBNull.Value },
            };

            string sqlcmd = $@"
            select 
            [BalanceQty]=sum(fit.inqty - fit.outqty + fit.adjustqty - fit.ReturnQty) 
            ,rd.poid
            ,rd.seq1
            ,rd.seq2
            ,RD.ID
            INTO #balanceTmp
            from dbo.View_AllReceivingDetail rd
            inner join FIR f on f.POID=rd.poid AND  f.ReceivingID = rd.id AND f.seq1 = rd.seq1 and f.seq2 = rd.Seq2
            inner join FtyInventory fit on fit.poid = rd.PoId and fit.seq1 = rd.seq1 and fit.seq2 = rd.Seq2 AND fit.StockType=rd.StockType and fit.Roll=rd.Roll and fit.Dyelot=rd.Dyelot
            where 1=1 
                {rWhere.Replace("where", "AND")}
                {sqlWhere.Replace("where", "AND").Replace("SP.", "f.").Replace("P.", "f.")}
                GROUP BY rd.poid,rd.seq1,rd.seq2,RD.ID
            ";

            DBProxy.Current.OpenConnection("Production", out SqlConnection sqlConn);
            using (sqlConn)
            {
                Base_ViewModel resultReport = new Base_ViewModel
                {
                    Result = DBProxy.Current.Select("Production", sqlcmd, listPar, out DataTable[] dataTables),
                };
                resultReport.DtArr = dataTables;
                return resultReport;
            }
        }

        public Tuple<string,string> GetWhere(QA_R01_ViewModel model)
        {
            string sqlWhere1 = string.Empty;
            string sqlwhere2 = string.Empty;

            return Tuple.Create(sqlWhere1, sqlwhere2);
        }

    }
}
