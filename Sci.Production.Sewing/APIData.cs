﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using System.Windows.Forms;
using Sci.Data;

namespace Sci.Production.Sewing
{
    /// <inheritdoc/>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleType", Justification = "Reviewed.")]
    public static partial class GetApiData
    {
        /// <inheritdoc/>
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
                    MyWebClient mwc = new MyWebClient();
                    var json = mwc.DownloadString(uri);

                    dataMode = JsonConvert.DeserializeObject<List<APIData>>(json);
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

    public class MyWebClient : WebClient
    {
        protected override WebRequest GetWebRequest(Uri uri)
        {
            WebRequest wr = base.GetWebRequest(uri);
            wr.Timeout = 5 * 60 * 1000; // 5分鐘
            return wr;
        }
    }

    /// <inheritdoc/>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
    public class APIData
    {
        /// <inheritdoc/>
        public DateTime Date { get; set; }

        /// <inheritdoc/>
        public string DateYYYYMMDD { get { return this.Date.ToString("yyyyMMdd"); } }

        /// <inheritdoc/>
        public decimal SewTtlManpower { get; set; }

        /// <inheritdoc/>
        public decimal SewTtlManhours { get; set; }

        /// <inheritdoc/>
        public decimal TransManpowerIn { get; set; }

        /// <inheritdoc/>
        public decimal TransManpowerOut { get; set; }

        /// <inheritdoc/>
        public decimal TransManhoursIn { get; set; }

        /// <inheritdoc/>
        public decimal TransManhoursOut { get; set; }

        /// <inheritdoc/>
        public int Holiday { get; set; }

        /// <inheritdoc/>
        public string YyyyMM { get; set; }

        /// <inheritdoc/>
        public List<APIData> results;
    }
}
