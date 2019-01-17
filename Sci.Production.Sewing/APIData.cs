using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Sci.Production.Sewing
{
    public class APIData
    {
        public DateTime Date { get; set; }

        public string SewTtlManpower { get; set; }

        public string SewTtlManhours { get; set; }

        public string TtlManpower { get; set; }

        public string TtlManhours { get; set; }

        public List<APIData> results;
    }

    public static partial class GetApiData
    {
        public static bool GetAPIData(string i_factory, DateTime i_start_date, DateTime i_end_date, out APIData dataMode)
        {
            try
            {
                // 取得web API資料
                using (var client = new WebClient())
                {
                    string apiParemeter = $"?i_factory={i_factory}&i_start_date={i_start_date.ToString("yyyy/MM/dd")}&i_end_date={i_end_date.ToString("yyyy/MM/dd")}";
                    client.Encoding = Encoding.UTF8;
                    Uri uri = new Uri("http://localhost:43281/api/Sample/GetPAMS");
                    var json = client.DownloadString(uri);
                    //List<aaa> dataModel = JsonConvert.DeserializeObject<List<aaa>>(json);
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
