using Sci.Data;
using System.Configuration;

namespace Sci.Production.Prg
{
    public static class PmsWebAPI
    {
        public static string TradeWebApiUri
        {
            get
            {
                if (DBProxy.Current.DefaultModuleName.Contains("testing") ||
                    DBProxy.Current.DefaultModuleName.Contains("Training") ||
                   DBProxy.Current.DefaultModuleName.Contains("Dummy"))
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
