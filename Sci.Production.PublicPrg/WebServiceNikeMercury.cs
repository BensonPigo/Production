using Ict;
using Newtonsoft.Json;
using PmsWebApiUtility45;
using Sci.Data;
using Sci.Production.Prg.Entity.NikeMercury;
using Sci.Win.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using static PmsWebApiUtility20.WebApiTool;
using static Sci.Production.Prg.Entity.NikeMercury.ResponseLabelsPackPlanCreate;

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
        public string lastRequestXml = string.Empty;
        public string lastResponseXml = string.Empty;

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

            this.lastRequestXml = xmlString;

            return xmlString;
        }

        public DualResult DeserializeNikeMercuryXml<T>(string xmlString, out T result)
            where T : class
        {
            result = null;
            this.lastResponseXml = xmlString;
            try
            {
                // 使用 XmlSerializer 將 XML 字符串轉換為結構變數
                //XElement xmlElement = XElement.Parse(xmlString);

                //if (xmlElement.Descendants("faultcode").Any())
                //{
                //    string errMsg = xmlElement.Descendants("faultstring").First().Value;
                //    return new DualResult(false, errMsg);
                //}

                XmlSerializer serializer = new XmlSerializer(typeof(T));
                using (StringReader stringReader = new StringReader(xmlString))
                {
                    result = (T)serializer.Deserialize(stringReader);
                }

                return new DualResult(true);
            }
            catch (Exception ex)
            {
                return new DualResult(false, ex);
            }
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
            string orderNumber2 = drOrderInfo["Customize1"].ToString();

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
                { "SOAPAction", "http://tempuri.org/ILabels/LabelsPackPlanCreate" },
            };
            HttpContent httpContent = new StringContent(soapRequest, Encoding.UTF8, "text/xml");
            WebApiBaseResult webApiBaseResult = WebApiTool.WebApiSend(this.serviceUrl, string.Empty, soapRequest, HttpMethod.Post, httpContent: httpContent, headers: dicHeader);

            if (!webApiBaseResult.isSuccess)
            {
                posBody.Body.LabelsPackPlanCreate.input.OrderNumber = orderNumber2;
                soapRequest = this.SerializeNikeMercuryXml(posBody);
                httpContent = new StringContent(soapRequest, Encoding.UTF8, "text/xml");
                webApiBaseResult = WebApiTool.WebApiSend(this.serviceUrl, string.Empty, soapRequest, HttpMethod.Post, httpContent: httpContent, headers: dicHeader);
            }

            if (!webApiBaseResult.isSuccess)
            {
                return new DualResult(false, webApiBaseResult.httpStatusCode.ToString() + webApiBaseResult.responseContent);
            }

            ResponseLabelsPackPlanCreate.Envelope responseResult;

            DualResult resultDeserialize = this.DeserializeNikeMercuryXml<ResponseLabelsPackPlanCreate.Envelope>(webApiBaseResult.responseContent, out responseResult);

            if (!resultDeserialize)
            {
                return resultDeserialize;
            }

            ResponseLabelsPackPlanCreate.LabelsPackPlanCreateResult labelsPackPlanCreateResult = responseResult.Body.LabelsPackPlanCreateResponse.LabelsPackPlanCreateResult;

            if (labelsPackPlanCreateResult.OutputMessage.ReturnCode != -1)
            {
                MyUtility.Msg.ShowMsgGrid(labelsPackPlanCreateResult.OutputData.Cartons.ToDataTable());
                //string resultJson = JsonConvert.SerializeObject(labelsPackPlanCreateResult.OutputData.Cartons);
                //MyUtility.Msg.InfoBox(resultJson);
                return new DualResult(true);
            }

            MyUtility.Msg.InfoBox(labelsPackPlanCreateResult.OutputMessage.ReturnDescription);

            return new DualResult(true);
        }

        /// <summary>
        /// LabelsPackPlanAdd
        /// </summary>
        /// <param name="cartonInfo">cartonInfo</param>
        /// <returns>DualResult</returns>
        public DualResult LabelsPackPlanAdd(RequestLabelsPackPlanCartonAdd.Input cartonInfo)
        {
            RequestLabelsPackPlanCartonAdd.Envelope posBody = new RequestLabelsPackPlanCartonAdd.Envelope()
            {
                Body = new RequestLabelsPackPlanCartonAdd.Body()
                {
                    LabelsPackPlanCartonAdd = new RequestLabelsPackPlanCartonAdd.LabelsPackPlanCartonAdd()
                    {
                        Input = cartonInfo,
                    },
                },
            };

            string soapRequest = this.SerializeNikeMercuryXml(posBody);

            Dictionary<string, string> dicHeader = new Dictionary<string, string>
            {
                { "SOAPAction", "http://tempuri.org/ILabels/LabelsPackPlanCartonAdd" },
            };

            HttpContent httpContent = new StringContent(soapRequest, Encoding.UTF8, "text/xml");
            WebApiBaseResult webApiBaseResult = WebApiTool.WebApiSend(this.serviceUrl, string.Empty, soapRequest, HttpMethod.Post, httpContent: httpContent, headers: dicHeader);

            if (!webApiBaseResult.isSuccess)
            {
                return new DualResult(false, webApiBaseResult.httpStatusCode.ToString() + webApiBaseResult.responseContent);
            }

            ResponseLabelsPackPlanCartonAdd.Envelope responseResult;

            DualResult resultDeserialize = this.DeserializeNikeMercuryXml<ResponseLabelsPackPlanCartonAdd.Envelope>(webApiBaseResult.responseContent, out responseResult);

            if (!resultDeserialize)
            {
                return resultDeserialize;
            }

            MyUtility.Msg.InfoBox(responseResult.Body.LabelsPackPlanCartonAddResponse.LabelsPackPlanCartonAddResult.OutputMessage.ReturnDescription);

            return new DualResult(true);
        }

        /// <summary>
        /// LabelsPackPlanDelete
        /// </summary>
        /// <param name="factoryCode">factoryCode</param>
        /// <param name="orderNumber">orderNumber</param>
        /// <param name="orderItem">orderItem</param>
        /// <returns>DualResult</returns>
        public DualResult LabelsPackPlanDelete(string factoryCode, string orderNumber, string orderItem)
        {
            RequestLabelsPackPlanDelete.Envelope posBody = new RequestLabelsPackPlanDelete.Envelope()
            {
                Body = new RequestLabelsPackPlanDelete.Body()
                {
                    LabelsPackPlanDelete = new RequestLabelsPackPlanDelete.LabelsPackPlanDelete()
                    {
                        Input = new RequestLabelsPackPlanDelete.Input() {
                            FactoryCode = factoryCode,
                            OrderNumber = orderNumber,
                            OrderItem = orderItem,
                        },
                    },
                },
            };

            string soapRequest = this.SerializeNikeMercuryXml(posBody);

            Dictionary<string, string> dicHeader = new Dictionary<string, string>
            {
                { "SOAPAction", "http://tempuri.org/ILabels/LabelsPackPlanDelete"},
            };

            HttpContent httpContent = new StringContent(soapRequest, Encoding.UTF8, "text/xml");
            WebApiBaseResult webApiBaseResult = WebApiTool.WebApiSend(this.serviceUrl, string.Empty, soapRequest, HttpMethod.Post, httpContent: httpContent, headers: dicHeader);

            if (!webApiBaseResult.isSuccess)
            {
                return new DualResult(false, webApiBaseResult.httpStatusCode.ToString() + webApiBaseResult.responseContent);
            }

            ResponseLabelsPackPlanDelete.Envelope responseResult;

            DualResult resultDeserialize = this.DeserializeNikeMercuryXml<ResponseLabelsPackPlanDelete.Envelope>(webApiBaseResult.responseContent, out responseResult);

            if (!resultDeserialize)
            {
                return resultDeserialize;
            }

            MyUtility.Msg.InfoBox(responseResult.Body.LabelsPackPlanDeleteResponse.LabelsPackPlanDeleteResult.OutputMessage.ReturnDescription);

            return new DualResult(true);
        }

        /// <summary>
        /// LabelsPackPlanUpdate
        /// </summary>
        /// <param name="cartonInfo">cartonInfo</param>
        /// <returns>DualResult</returns>
        public DualResult LabelsPackPlanUpdate(RequestLabelsPackPlanCartonUpdate.Input cartonInfo)
        {
            RequestLabelsPackPlanCartonUpdate.Envelope posBody = new RequestLabelsPackPlanCartonUpdate.Envelope()
            {
                Body = new RequestLabelsPackPlanCartonUpdate.Body()
                {
                    LabelsPackPlanCartonUpdate = new RequestLabelsPackPlanCartonUpdate.LabelsPackPlanCartonUpdate()
                    {
                        Input = cartonInfo,
                    },
                },
            };

            string soapRequest = this.SerializeNikeMercuryXml(posBody);

            Dictionary<string, string> dicHeader = new Dictionary<string, string>
            {
                { "SOAPAction", "http://tempuri.org/ILabels/LabelsPackPlanCartonUpdate" },
            };

            HttpContent httpContent = new StringContent(soapRequest, Encoding.UTF8, "text/xml");
            WebApiBaseResult webApiBaseResult = WebApiTool.WebApiSend(this.serviceUrl, string.Empty, soapRequest, HttpMethod.Post, httpContent: httpContent, headers: dicHeader);

            if (!webApiBaseResult.isSuccess)
            {
                return new DualResult(false, webApiBaseResult.httpStatusCode.ToString() + webApiBaseResult.responseContent);
            }

            ResponseLabelsPackPlanCartonUpdate.Envelope responseResult;

            DualResult resultDeserialize = this.DeserializeNikeMercuryXml<ResponseLabelsPackPlanCartonUpdate.Envelope>(webApiBaseResult.responseContent, out responseResult);

            if (!resultDeserialize)
            {
                return resultDeserialize;
            }

            MyUtility.Msg.InfoBox(responseResult.Body.LabelsPackPlanCartonUpdateResponse.LabelsPackPlanCartonUpdateResult.OutputMessage.ReturnDescription);

            return new DualResult(true);
        }
    }
}
