using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
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

        public decimal TtlManpower { get; set; }

        public decimal TtlManhours { get; set; }

        public List<APIData> results;
    }

    public static partial class GetApiData
    {
        public static bool GetAPIData(string i_M,string i_factory, DateTime i_start_date, DateTime i_end_date, out List<APIData> dataMode)
        {
            try
            {
                // 取得web API資料
                using (var client = new WebClient())
                {
                    string apiParemeter = string.Empty;
                    if (!MyUtility.Check.Empty(i_factory))
                    {
                        apiParemeter = $"?i_M=0&i_factory={i_factory}&i_start_date={i_start_date.ToString("yyyy/MM/dd")}&i_end_date={i_end_date.ToString("yyyy/MM/dd")}";
                    }
                    else
                    {
                        apiParemeter = $"?i_M={i_M}&i_factory=0&i_start_date={i_start_date.ToString("yyyy/MM/dd")}&i_end_date={i_end_date.ToString("yyyy/MM/dd")}";
                    }

                    XDocument docx = XDocument.Load(Application.ExecutablePath + ".config");
                    string nowConnection = DBProxy.Current.DefaultModuleName;
                    string connections = docx.Descendants("modules").Elements().Where(y => y.FirstAttribute.Value.EqualString(nowConnection)).Descendants("connectionStrings").Elements().Where(x => x.FirstAttribute.Value.Contains("PamsAPIuri")).Select(z => z.LastAttribute.Value).ToList()[0].ToString();

                    Uri uri = new Uri(connections + apiParemeter);
                    var json = client.DownloadString(uri);

                    dataMode = JsonConvert.DeserializeObject<List<APIData>>(json);
                    //dataMode = JsonConvert.DeserializeObject<APIData>(json);
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
