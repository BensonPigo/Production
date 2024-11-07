using Newtonsoft.Json;
using Sci.Production.Prg.PowerBI.Model;
using System.Data;
using static Sci.Production.CallPmsAPI.PackingA2BWebAPI_Model;

namespace Sci.Production.Prg
{
    /// <inheritdoc/>
    public static class CallWebAPI
    {
        /// <inheritdoc/>
        public static DataTable ToTable<T>(string strJson)
        {
            JsonModel<T> m = JsonConvert.DeserializeObject<JsonModel<T>>(strJson);
            return ToDataTable<T>(m.ResultDt);
        }
    }
}
