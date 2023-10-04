using Ict;
using Newtonsoft.Json;
using PmsWebApiUtility45;
using Sci.Data;
using Sci.Production.Prg.Entity;
using Sci.Production.Prg.Entity.NikeMercury;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using static PmsWebApiUtility20.WebApiTool;

namespace Sci.Production.Prg
{
    /// <summary>
    /// Nike Mercury webservice相關method
    /// </summary>
    public class WebServiceNikeMercury
    {
        private static WebServiceNikeMercury staticService;

        /// <summary>
        /// StaticService
        /// </summary>
        public static WebServiceNikeMercury StaticService
        {
            get
            {
                if (staticService == null)
                {
                    staticService = new WebServiceNikeMercury("http://localhost/OLLIe/OLLIe.svc/Labels");
                }

                return staticService;
            }
        }

        private string serviceUrl = string.Empty;

        /// <summary>
        /// WebServiceNikeMercury
        /// </summary>
        /// <param name="serviceUrl">serviceUrl</param>
        public WebServiceNikeMercury(string serviceUrl)
        {
            this.serviceUrl = serviceUrl;
        }

        private string SerializeNikeMercuryXml(object xmlObject)
        {
            XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
            xmlSerializerNamespaces.Add("soapenv", "http://schemas.xmlsoap.org/soap/envelope/");
            xmlSerializerNamespaces.Add("tem", "http://tempuri.org/");
            xmlSerializerNamespaces.Add("oll", "http://schemas.datacontract.org/2004/07/OLLIeLabels");

            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true, // 移除 XML 声明行
                Indent = true,             // 格式化输出
                Encoding = new UTF8Encoding(false), // 设置输出编码为 UTF-8，且不包含 BOM
            };

            var serializer = new XmlSerializer(xmlObject.GetType());
            string xmlString = string.Empty;

            // 使用 StringWriter 创建 XmlWriter
            using (StringWriter stringWriter = new StringWriter())
            using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, xmlWriterSettings))
            {
                serializer.Serialize(xmlWriter, xmlObject, xmlSerializerNamespaces);
                xmlString = stringWriter.ToString();
            }

            return xmlString;
        }

        private DualResult DeserializeNikeMercuryXml<T>(string xmlString, out T result)
        {
            result = null;

            // 使用 XmlSerializer 將 XML 字符串轉換為結構變數
            XElement xmlElement = XElement.Parse(xmlString);

            if(xmlElement.Descendants("faultcode").Any())

        }

        /// <summary>
        /// LabelsPackPlanCreate
        /// </summary>
        /// <param name="packID">packID</param>
        /// <returns>DualResult</returns>
        public DualResult LabelsPackPlanCreate(string packID)
        {
            string sqlGetData = $@"
select  CustPONo, Customize1
from    PackingGuide p with (nolock)
inner join  Orders o with (nolock) on o.ID = p.OrderID
where p.ID = '{packID}'

select  SizeCode,
        QtyPerCTN,
        ShipQty,
        [BuildQty] = ShipQty / QtyPerCTN * QtyPerCTN,
        [BalQty] = ShipQty - (ShipQty / QtyPerCTN * QtyPerCTN)
from PackingGuide_Detail pg with (nolock)
where   pg.ID = '{packID}'

";
            DataTable[] dtResults;
            DualResult result = DBProxy.Current.Select(null, sqlGetData, out dtResults);
            if (!result)
            {
                return result;
            }

            DataRow drOrderInfo = dtResults[0].Rows[0];
            DataTable dtPackPlanOrderSize = dtResults[1];

            if (dtPackPlanOrderSize.Rows.Count == 0)
            {
                return new DualResult(false, "Packing Guide not found");
            }

            string[] orderInfo = drOrderInfo["CustPONo"].ToString().Split('-');
            string orderNumber = orderInfo[0];
            string orderItem = orderInfo.Length < 2 ? string.Empty : orderInfo[1];

            RequestLabelsPackPlanCreate.Envelope posBody = new RequestLabelsPackPlanCreate.Envelope()
            {
                Body = new RequestLabelsPackPlanCreate.Body()
                {
                    LabelsPackPlanCreate = new RequestLabelsPackPlanCreate.LabelsPackPlanCreate()
                    {
                        input = new RequestLabelsPackPlanCreate.Input()
                        {
                            FactoryCode = "SNY",
                            OrderNumber = orderNumber,
                            OrderItem = orderItem,
                            SingleSizePerCartonFlag = 0,
                            SizeData = new RequestLabelsPackPlanCreate.SizeData()
                            {
                                PackPlanOrderSize = dtPackPlanOrderSize
                                .AsEnumerable()
                                .Select(s =>
                                    new RequestLabelsPackPlanCreate.PackPlanOrderSize()
                                    {
                                        Description = s["SizeCode"].ToString(),
                                        BuildQty = MyUtility.Convert.GetInt(s["BuildQty"]),
                                        CartonPackFactor = MyUtility.Convert.GetInt(s["QtyPerCTN"]),
                                        CartonTypeCode = "A4",
                                    })
                                .ToArray(),
                            },
                        },
                    },
                },
            };

            string soapRequest = this.SerializeNikeMercuryXml(posBody);

            Dictionary<string, string> dicHeader = new Dictionary<string, string>
            {
                { "Content-Type", "text/xml; charset=utf-8" },
                { "SOAPAction", "http://tempuri.org/ILabels/LabelsPackPlanCreate" },
            };

            return new DualResult(true);

            WebApiBaseResult webApiBaseResult = WebApiTool.WebApiPost(this.serviceUrl, string.Empty, soapRequest, headers: dicHeader);

            if (!webApiBaseResult.isSuccess)
            {
                return new DualResult(false, webApiBaseResult.responseContent);
            }

            return new DualResult(true, webApiBaseResult.responseContent);
        }
    }
}
