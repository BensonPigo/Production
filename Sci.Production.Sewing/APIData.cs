using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Sci.Production.Sewing
{
    public class APIData
    {
        public DateTime Date { get; set; }

        public decimal SewTtlManpower { get; set; }

        public decimal SewTtlManhours { get; set; }

        public decimal TtlManpower { get; set; }

        public decimal TtlManhours { get; set; }

        public List<APIData> results;
    }

    public static partial class GetApiData
    {
        public static bool GetAPIData(string i_M,string i_factory, DateTime i_start_date, DateTime i_end_date, out APIData dataMode)
        {
            try
            {
                // 取得web API資料
                using (var client = new WebClient())
                {
                    string apiParemeter = string.Empty;
                    if (!MyUtility.Check.Empty(i_factory))
                    {
                        apiParemeter = $"?i_M=&i_factory={i_factory}&i_start_date={i_start_date.ToString("yyyy/MM/dd")}&i_end_date={i_end_date.ToString("yyyy/MM/dd")}";
                    }
                    else
                    {
                        apiParemeter = $"?i_M={i_M}&i_factory=&i_start_date={i_start_date.ToString("yyyy/MM/dd")}&i_end_date={i_end_date.ToString("yyyy/MM/dd")}";
                    }

                    client.Encoding = Encoding.UTF8;
                    Uri uri = new Uri(ConfigurationManager.AppSettings["PamsAPIuri"].ToString() + apiParemeter);
                    var json = client.DownloadString(uri);

                    // List<aaa> dataModel = JsonConvert.DeserializeObject<List<aaa>>(json);
                    dataMode = JsonConvert.DeserializeObject<APIData>(json);
                }
            }
            catch (Exception ex)
            {
                dataMode = null;
                return false;
            }

            return true;
        }
    }
}
