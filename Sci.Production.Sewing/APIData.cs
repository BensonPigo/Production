using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using System.Windows.Forms;
using Sci.Data;

namespace Sci.Production.Sewing
{
    public class APIData
    {
        public DateTime Date { get; set; }

        public decimal SewTtlManpower { get; set; }

        public decimal SewTtlManhours { get; set; }

        public int Holiday { get; set; }

        public string yyyyMM { get; set; }

        public List<APIData> results;
    }

    public static partial class GetApiData
    {
        public static bool GetAPIData(string i_M, string i_factory, DateTime i_start_date, DateTime i_end_date, out List<APIData> dataMode)
        {
            try
            {
                // 取得web API資料
                using (var client = new WebClient())
                {
                    string apiParemeter = string.Empty;
                    if (!MyUtility.Check.Empty(i_factory))
                    {
                        apiParemeter = $"?M=0&factory={i_factory}&startDate={i_start_date.ToString("yyyy/MM/dd")}&endDate={i_end_date.ToString("yyyy/MM/dd")}";
                    }
                    else
                    {
                        apiParemeter = $"?M={i_M}&factory=0&startDate={i_start_date.ToString("yyyy/MM/dd")}&endDate={i_end_date.ToString("yyyy/MM/dd")}";
                    }

                    XDocument docx = XDocument.Load(Application.ExecutablePath + ".config");
                    string nowConnection = DBProxy.Current.DefaultModuleName;
                    string connections = docx.Descendants("modules").Elements().Where(y => y.FirstAttribute.Value.EqualString(nowConnection)).Descendants("connectionStrings").Elements().Where(x => x.FirstAttribute.Value.Contains("PamsAPIuri")).Select(z => z.LastAttribute.Value).ToList()[0].ToString();

                    Uri uri = new Uri(connections + apiParemeter);
                    var json = client.DownloadString(uri);

                    dataMode = JsonConvert.DeserializeObject<List<APIData>>(json);

                    // dataMode = JsonConvert.DeserializeObject<APIData>(json);
                }
            }
            catch (Exception)
            {
                dataMode = null;
                return false;
            }

            return true;
        }
    }
}
