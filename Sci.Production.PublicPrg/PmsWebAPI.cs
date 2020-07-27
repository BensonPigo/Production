using Sci.Data;
using System.Configuration;

namespace Sci.Production.Prg
{
    public static class PmsWebAPI
    {
        /// <summary>
        /// IsDummy
        /// </summary>
        public static bool IsDummy
        {
            get
            {
                return DBProxy.Current.DefaultModuleName.Contains("testing") ||
                        DBProxy.Current.DefaultModuleName.Contains("Training") ||
                        DBProxy.Current.DefaultModuleName.Contains("Dummy");
            }
        }

        /// <summary>
        /// TradeWebApiUri
        /// </summary>
        public static string TradeWebApiUri
        {
            get
            {
                if (IsDummy)
                {
                    return ConfigurationManager.AppSettings["TradeWebAPI_Test"];
                }
                else
                {
                    return ConfigurationManager.AppSettings["TradeWebAPI"];
                }
            }
        }
    }
}
