using static PmsWebApiUtility20.WebApiTool;

namespace Sci.Production.CallPmsAPI.Model
{
    /// <inheritdoc/>
    public class ResultInfo
    {
        /// <inheritdoc/>
        public WebApiBaseResult Result { get; set; }

        /// <inheritdoc/>
        public string ErrCode { get; set; }

        /// <inheritdoc/>
        public string ResultDT { get; set; }
    }
}
