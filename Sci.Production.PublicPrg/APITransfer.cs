using Ict;
using Newtonsoft.Json;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
select
    ID,
    Ukey,
    NetKg,
    WeightKg,
    TransferExportReason,
    TransferExportReasonDesc = isnull((select Description from WhseReason where ID = TransferExportReason and type = 'TE'), '')
from TransferExport_Detail
where ID = '{id}'
";
            result = DBProxy.Current.Select(null, sqlcmd, out DataTable dt);
            if (!result)
            {
                return result;
            }

            sqlcmd = $@"
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
	StockQty
from TransferExport_Detail_Carton
where ID = '{id}'
";
            result = DBProxy.Current.Select(null, sqlcmd, out DataTable dt2);
            if (!result)
            {
                return result;
            }

            var obj = new
            {
                Status = "Confirm",
                TransferExportID = id,
                DataSet = new
                {
                    TransferExport_Detail = dt,
                    TransferExport_Detail_Carton = dt2,
                },
            };

            string tradeWebApiUri = PmsWebAPI.TradeWebApiUri;
            string jsonBody = JsonConvert.SerializeObject(obj);
            string requestUri = "api/GetTradeData/ConfirmTK";

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
