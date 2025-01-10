using System;
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
        /// <param name="timeout">default Time out</param>
        /// <returns>HttpResponseMessage</returns>
        public static HttpResponseMessage GetJsonDataHttpClient(string targetUrl, string parameter, int timeout = 5)
        {
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
            using (HttpClient httpClient = new HttpClient() { Timeout = TimeSpan.FromMinutes(timeout) })
            {
                httpResponseMessage = httpClient.GetAsync(targetUrl + "?" + parameter).Result;
            }

            return httpResponseMessage;
        }
    }
}
