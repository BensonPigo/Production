using System.Net.Http;

namespace Sci.Production.Prg.PowerBI.Logic
{
    /// <summary>
    /// This class is used to get data from a web service
    /// </summary>
    public class HttpHelpers
    {
        /// <summary>
        /// This method is used to get data from a web service using HttpClient
        /// </summary>
        /// <param name="targetUrl">Url</param>
        /// <param name="parameter">Para</param>
        /// <returns>HttpResponseMessage</returns>
        public static HttpResponseMessage GetJsonDataHttpClient(string targetUrl, string parameter)
        {
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
            using (HttpClient httpClient = new HttpClient())
            {
                httpResponseMessage = httpClient.GetAsync(targetUrl + "?" + parameter).Result;
            }

            return httpResponseMessage;
        }
    }
}
