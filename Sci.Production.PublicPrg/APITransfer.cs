using Ict;
using Newtonsoft.Json;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZXing;
using static PmsWebApiUtility20.WebApiTool;

namespace Sci.Production.Prg
{
    /// <inheritdoc/>
    public static class APITransfer
    {
        private class TradeAPIResult
        {
            /// <summary>
            /// Result
            /// </summary>
            public bool Result { get; set; }

            /// <summary>
            /// ErrorMsg
            /// </summary>
            public string ErrorMsg { get; set; }
        }

        /// <inheritdoc/>
        public static DualResult SendTransferExport(string id)
        {
            DualResult result;
            string sqlcmd = $@"
select  Remark_Factory
from TransferExport with (nolock)
where ID = '{id}'

select
    ID,
    Ukey,
    TransferExportReason,
    TransferExportReasonDesc = isnull((select Description from WhseReason where ID = TransferExportReason and type = 'TE'), '')
from TransferExport_Detail with (nolock)
where ID = '{id}'

select
	TransferExport_DetailUkey,
	ID,
	POID,
	Seq1,
	Seq2,
	Carton,
	LotNo,
	Qty,
	FOC,
	EditName,
	EditDate,
	StockUnitID,
	StockQty,
    Tone,
    MINDQRCode,
    Roll,
    NetKg,
    WeightKg,
    CBM
from TransferExport_Detail_Carton with (nolock)
where ID = '{id}'
";
            DataTable[] dtResults;
            result = DBProxy.Current.Select(null, sqlcmd, out dtResults);
            if (!result)
            {
                return result;
            }

            DataTable dtTransferExport = dtResults[0];
            DataTable dtTransferExport_Detail = dtResults[1];
            DataTable dtTransferExport_Detail_Carton = dtResults[2];

            var jsonBody = new
            {
                Status = "Confirm",
                TransferExportID = id,
                DataSet = new
                {
                    TransferExport = dtTransferExport,
                    TransferExport_Detail = dtTransferExport_Detail,
                    TransferExport_Detail_Carton = dtTransferExport_Detail_Carton,
                },
            };

            return CallTradeTKAPI(jsonBody);
        }

        /// <summary>
        /// SendShippingSeparateConfirm
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>DualResult</returns>
        public static DualResult SendShippingSeparateConfirm(string id)
        {
            DualResult result;
            string sqlcmd = $@"
select  WHSpearateConfirmDate,
        ShippingSeparateConfirmDate
from TransferExport with (nolock)
where ID = '{id}'
";
            DataTable dtResult;
            result = DBProxy.Current.Select(null, sqlcmd, out dtResult);

            if (!result)
            {
                return result;
            }

            var jsonBody = new
            {
                Status = "Shipping Separate Confirm",
                TransferExportID = id,
                DataSet = new
                {
                    TransferExport = dtResult,
                },
            };

            return CallTradeTKAPI(jsonBody);
        }

        /// <summary>
        /// SendRequestSeparate
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>DualResult</returns>
        public static DualResult SendRequestSeparate(string id)
        {
            DualResult result;
            string sqlcmd = $@"
select  Remark_Factory,
        FtyRequestSeparateDate
from TransferExport with (nolock)
where ID = '{id}'

select  Ukey,
        TransferExportReason,
        TransferExportReasonDesc
from TransferExport_Detail with (nolock)
where ID = '{id}'

select  TransferExport_DetailUkey,
        ID,
        POID,
        Seq1,
        Seq2,
        Carton,
        LotNo,
        Qty,
        FOC,
        EditName,
        EditDate,
        StockUnitID,
        SotckQty,
        Tone,
        MINDQRCode,
        Roll,
        NetKg,
        WeightKg,
        CBM,
        GroupID
from    TransferExport_Detail_Carton with (nolock)
where ID = '{id}'

";
            DataTable[] dtResults;
            result = DBProxy.Current.Select(null, sqlcmd, out dtResults);

            if (!result)
            {
                return result;
            }

            DataTable dtTransferExport = dtResults[0];
            DataTable dtTransferExport_Detail = dtResults[1];
            DataTable dtTransferExport_Detail_Carton = dtResults[2];

            var jsonBody = new
            {
                Status = "Request Separate",
                TransferExportID = id,
                DataSet = new
                {
                    TransferExport = dtTransferExport,
                    TransferExport_Detail = dtTransferExport_Detail,
                    TransferExport_Detail_Carton = dtTransferExport_Detail_Carton,
                },
            };

            return CallTradeTKAPI(jsonBody);
        }

        /// <summary>
        /// CallTradeTKAPI
        /// </summary>
        /// <param name="jsonBody">jsonBody</param>
        /// <returns>DualResult</returns>
        public static DualResult CallTradeTKAPI(object jsonBody)
        {
            DualResult result = new DualResult(true);

            string tradeWebApiUri = PmsWebAPI.TradeWebApiUri;

            string requestUri = "api/GetTradeData/TransferTK";

            WebApiBaseResult webApiBaseResult = PmsWebApiUtility45.WebApiTool.WebApiPost(tradeWebApiUri, requestUri, jsonBody, 600);

            switch (webApiBaseResult.webApiResponseStatus)
            {
                case WebApiResponseStatus.Success:
                    TradeAPIResult tradeAPIResult = JsonConvert.DeserializeObject<TradeAPIResult>(webApiBaseResult.responseContent);
                    if (tradeAPIResult.Result)
                    {
                        result = Result.True;
                    }
                    else
                    {
                        result = new DualResult(false, new Exception("tradeAPI Error: " + tradeAPIResult.ErrorMsg));
                    }

                    break;
                case WebApiResponseStatus.WebApiReturnFail:
                    result = new DualResult(false, new Exception(webApiBaseResult.responseContent));
                    break;
                case WebApiResponseStatus.OtherException:
                    result = new DualResult(false, webApiBaseResult.exception);
                    break;
                case WebApiResponseStatus.ApiTimeout:
                    break;
                default:
                    result = Result.True;
                    break;
            }

            return result;
        }
    }
}
