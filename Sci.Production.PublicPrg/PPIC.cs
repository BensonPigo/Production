using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Sci.Data;
using Sci;
using Ict;
using Ict.Win;
using System.Configuration;
using PostJobLog;

namespace Sci.Production.PublicPrg
{

    public static partial class Prgs
    {
        #region GetExcelEnglishColumnName
        /// <summary>
        /// GetExcelEnglishColumnName(int)
        /// </summary>
        /// <param name="column"></param>
        /// <returns>string</returns>
        public static string GetExcelEnglishColumnName(int column)
        {
            string strReturn = "";

            int iQuotient = column / 26;//商數
            int iRemainder = column % 26;//餘數

            if (iRemainder == 0)
                iQuotient--;  // 剛好整除的時候，商數要減一

            if (iQuotient > 0)
                strReturn = Convert.ToChar(64 + iQuotient).ToString();//A 65 利用ASCII做轉換

            if (iRemainder == 0)
                strReturn += "Z";
            else
                strReturn += Convert.ToChar(64 + iRemainder).ToString();    //A 65 利用ASCII做轉換

            return strReturn;
        }
        #endregion;

        public static DualResult PostReplacementReportToTrade(string replacementID)
        {
            string sqlReplacementReport = $@"
select ID
,CDate
,POID
,Type
,MDivisionID
,FactoryID
,ApplyName
,ApplyDate
,ApvName
,[ApvDate] = GETDATE()
,TPECFMName
,TPECFMDate
,[Status] = 'Approved'
,ExportToTPE
,AddName
,AddDate
,[EditName] = '{Env.User.UserID}'
,[EditDate] = GETDATE()
,TPEEditName
,TPEEditDate
,Responsibility from ReplacementReport where ID = '{replacementID}'
";

            string sqlReplacementReportDetail = $@"
select
ID
,Seq1
,Seq2
,Refno
,SCIRefno
,INVNo
,ETA
,ColorID
,EstInQty
,ActInQty
,AGradeDefect
,AGradeRequest
,BGradeDefect
,BGradeRequest
,NarrowWidth
,NarrowRequest
,Other
,OtherReason
,OtherRequest
,TotalRequest
,AfterCutting
,AfterCuttingReason
,AfterCuttingRequest
,DamageSendDate
,AWBNo
,ReplacementETA
,Responsibility
,ResponsibilityReason
,Suggested
,OccurCost
,UKey
,OldFabricUkey
,OldFabricVer
,Junk from ReplacementReport_Detail where ID = '{replacementID}'
";

            DataTable dtReplacementReport;
            DataTable dtReplacementReportDetail;
            DualResult result;
            result = DBProxy.Current.Select(null, sqlReplacementReport,out dtReplacementReport);
            if (!result)
            {
                return result;
            }
            result = DBProxy.Current.Select(null, sqlReplacementReportDetail, out dtReplacementReportDetail);
            if (!result)
            {
                return result;
            }

            var postBody = new { ReplacementReport = dtReplacementReport, ReplacementReport_Detail = dtReplacementReportDetail };
            string tradeWebApiUri = ConfigurationManager.AppSettings["TradeWebAPI"];

            CallTPEWebAPI callTPEWebAPI = new CallTPEWebAPI(tradeWebApiUri);

            result = callTPEWebAPI.CallWebApiPost("/api/ReplacementReport/UpdateReplacement", postBody);
            return result;
        }
    }
}