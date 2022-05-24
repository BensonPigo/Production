﻿using System;
using System.Data;
using Sci.Data;
using Ict;
using PostJobLog;
using Sci.Production.Prg;

namespace Sci.Production.PublicPrg
{
    /// <summary>
    /// Prgs
    /// </summary>
    public static partial class Prgs
    {
        #region GetExcelEnglishColumnName

        /// <summary>
        /// GetExcelEnglishColumnName(int)
        /// </summary>
        /// <param name="column">column</param>
        /// <returns>string</returns>
        public static string GetExcelEnglishColumnName(int column)
        {
            string strReturn = string.Empty;

            int iQuotient = column / 26; // 商數
            int iRemainder = column % 26; // 餘數

            if (iRemainder == 0)
            {
                iQuotient--;  // 剛好整除的時候，商數要減一
            }

            if (iQuotient > 0)
            {
                strReturn = Convert.ToChar(64 + iQuotient).ToString(); // A 65 利用ASCII做轉換
            }

            if (iRemainder == 0)
            {
                strReturn += "Z";
            }
            else
            {
                strReturn += Convert.ToChar(64 + iRemainder).ToString();    // A 65 利用ASCII做轉換
            }

            return strReturn;
        }
        #endregion;

        /// <summary>
        /// Post Replacement Report To Trade
        /// </summary>
        /// <param name="replacementID">replacementID</param>
        /// <returns>DualResult</returns>
        public static DualResult PostReplacementReportToTrade(string replacementID, out string errorKind)
        {
            errorKind = string.Empty;
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
,Junk
,FinalNeedQty
,ReplacementUnit
from ReplacementReport_Detail where ID = '{replacementID}'
";

            DataTable dtReplacementReport;
            DataTable dtReplacementReportDetail;
            DualResult result;
            result = DBProxy.Current.Select(null, sqlReplacementReport, out dtReplacementReport);
            if (!result)
            {
                errorKind = "Generate data from Replacement report failed.";
                return result;
            }

            result = DBProxy.Current.Select(null, sqlReplacementReportDetail, out dtReplacementReportDetail);
            if (!result)
            {
                errorKind = "Generate data from Replacement report failed.";
                return result;
            }

            var postBody = new { ReplacementReport = dtReplacementReport, ReplacementReport_Detail = dtReplacementReportDetail };
            string tradeWebApiUri = PmsWebAPI.TradeWebApiUri;

            CallTPEWebAPI callTPEWebAPI = new CallTPEWebAPI(tradeWebApiUri);

            result = callTPEWebAPI.CallWebApiPost("/api/ReplacementReport/UpdateReplacement", postBody);
            if (!result)
            {
                errorKind = "Networking problem.";
            }

            return result;
        }

        /// <summary>
        /// Post Order Change
        /// </summary>
        /// <param name="iD">_ID</param>
        /// <param name="status">_status</param>
        /// <param name="ftyComments">ftyComments</param>
        /// <returns>DualResult</returns>
        public static DualResult PostOrderChange(string iD, string status, string ftyComments)
        {
            OrderChangeModel orderChangeModel = new OrderChangeModel()
            {
                ID = iD,
                Status = status,
                EditName = Env.User.UserID,
                FTYComments = ftyComments,
            };

            DualResult result;
            string tradeWebApiUri = PmsWebAPI.TradeWebApiUri;
            CallTPEWebAPI callTPEWebAPI = new CallTPEWebAPI(tradeWebApiUri);

            result = callTPEWebAPI.CallWebApiPost("api/OrderChange/Receive", orderChangeModel);
            return result;
        }

        /// <summary>
        /// Check Order Change Confirmed
        /// </summary>
        /// <param name="orderid">orderid</param>
        /// <param name="seq">seq</param>
        /// <returns>bool</returns>
        public static bool CheckOrderChangeConfirmed(string orderid, string seq)
        {
            string sqlcmd = $@"
select 1
from OrderChangeApplication o with(nolock)
inner join OrderChangeApplication_Detail od with(nolock) on o.ID = od.id
where OrderID = '{orderid}' and od.Seq = '{seq}' and status != 'Confirmed' and status != 'Closed' 
";
            return !MyUtility.Check.Seek(sqlcmd);
        }

        public static bool CheckSPNoCompleted(string POID)
        {
            // 同一個POID底下, 只要有任一筆沒Completed 就可以Save by ISP20220649 
            string sqlcmd = $@"
select 
[Status1] = IIF(Orders.Category != 'M' and Orders.GMTComplete in ('C','S') ,'completed','')
,[Status2] = IIF( Orders.Category = 'M' and (DateAdd(Day, -60, Convert(Date, GetDate())) >= Orders.BuyerDelivery or DateAdd(Day, -60, Convert(Date, GetDate())) >= Orders.SCIDelivery),'completed','')
into #tmp
from Orders
Where PoID = '{POID}'

select * from #tmp
where status1 = '' and Status2 =''

drop table #tmp
";
            if (!MyUtility.Check.Seek(sqlcmd))
            {
                MyUtility.Msg.WarningBox($"Save failed, <SP#> already completed.");
                return false;
            }

            return true;
        }
    }

    /// <summary>
    /// OrderChangeModel
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleType", Justification = "Reviewed.")]
    public class OrderChangeModel
    {
        /// <summary>
        /// ID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// EditName
        /// </summary>
        public string EditName { get; set; }

        /// <summary>
        /// FTYComments
        /// </summary>
        public string FTYComments { get; set; }
    }
}