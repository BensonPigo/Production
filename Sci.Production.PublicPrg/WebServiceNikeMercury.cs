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
using static Sci.Production.Prg.Entity.NikeMercury.ResponseLabelsPackPlanCartonAdd;
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
        private string factoryCode = string.Empty;

        /// <summary>
        /// FactoryCode
        /// </summary>
        public string FactoryCode
        {
            get { return this.factoryCode; }
        }

        /// <summary>
        /// WebServiceNikeMercury
        /// </summary>
        /// <param name="serviceUrl">serviceUrl</param>
        public WebServiceNikeMercury(string serviceUrl)
        {
            this.serviceUrl = serviceUrl;
            this.factoryCode = MyUtility.GetValue.Lookup("select NikeFactoryCode from system", "Production");
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
        /// <param name="orderNumber">orderNumber</param>
        /// <param name="orderItem">orderItem</param>
        /// <param name="listMercuryCarton">listMercuryCarton</param>
        /// <returns>DualResult</returns>
        public DualResult LabelsPackPlanCreate(string packID, string orderNumber, string orderItem, out List<ResponseLabelsPackPlanCreate.Carton> listMercuryCarton)
        {
            listMercuryCarton = new List<ResponseLabelsPackPlanCreate.Carton>();
            string sqlGetData = $@"
select  pg.SizeCode,
        pg.QtyPerCTN,
        pg.ShipQty,
        [BuildQty] = (pg.ShipQty / pg.QtyPerCTN) * pg.QtyPerCTN
        l.NikeCartonType
from PackingGuide_Detail pg with (nolock)
left join LocalItem l with (nolock) on l.Refno = pg.Refno
where   pg.ID = '{packID}'

";
            DataTable dtPackPlanOrderSize;
            DualResult result = DBProxy.Current.Select(null, sqlGetData, out dtPackPlanOrderSize);
            if (!result)
            {
                return result;
            }

            if (dtPackPlanOrderSize.Rows.Count == 0)
            {
                return new DualResult(false, "Packing Guide not found");
            }

            RequestLabelsPackPlanCreate.Envelope posBody = new RequestLabelsPackPlanCreate.Envelope()
            {
                Body = new RequestLabelsPackPlanCreate.Body()
                {
                    LabelsPackPlanCreate = new RequestLabelsPackPlanCreate.LabelsPackPlanCreate()
                    {
                        input = new RequestLabelsPackPlanCreate.Input()
                        {
                            FactoryCode = this.factoryCode,
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
                                        CartonTypeCode = s["NikeCartonType"].ToString(),
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

            if (labelsPackPlanCreateResult.OutputMessage.ReturnCode == -1)
            {
                return new DualResult(false, labelsPackPlanCreateResult.OutputMessage.ReturnDescription);
            }

            listMercuryCarton = labelsPackPlanCreateResult.OutputData.Cartons;

            return new DualResult(true);
        }

        /// <summary>
        /// LabelsPackPlanAdd
        /// </summary>
        /// <param name="cartonInfo">cartonInfo</param>
        /// <param name="mercuryCarton">mercuryCarton</param>
        /// <returns>DualResult</returns>
        public DualResult LabelsPackPlanAdd(RequestLabelsPackPlanCartonAdd.Input cartonInfo, out ResponseLabelsPackPlanCartonAdd.Carton mercuryCarton)
        {
            mercuryCarton = new ResponseLabelsPackPlanCartonAdd.Carton();

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

            LabelsPackPlanCartonAddResult labelsPackPlanCartonAddResult = responseResult.Body.LabelsPackPlanCartonAddResponse.LabelsPackPlanCartonAddResult;

            if (labelsPackPlanCartonAddResult.OutputMessage.ReturnCode == -1)
            {
                return new DualResult(false, labelsPackPlanCartonAddResult.OutputMessage.ReturnDescription);
            }

            mercuryCarton = labelsPackPlanCartonAddResult.OutputData.Carton;

            return new DualResult(true);
        }

        /// <summary>
        /// LabelsPackPlanDelete
        /// </summary>
        /// <param name="orderNumber">orderNumber</param>
        /// <param name="orderItem">orderItem</param>
        /// <returns>DualResult</returns>
        public DualResult LabelsPackPlanDelete(string orderNumber, string orderItem)
        {
            RequestLabelsPackPlanDelete.Envelope posBody = new RequestLabelsPackPlanDelete.Envelope()
            {
                Body = new RequestLabelsPackPlanDelete.Body()
                {
                    LabelsPackPlanDelete = new RequestLabelsPackPlanDelete.LabelsPackPlanDelete()
                    {
                        Input = new RequestLabelsPackPlanDelete.Input()
                        {
                            FactoryCode = this.factoryCode,
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

            return new DualResult(true);
        }

        /// <summary>
        /// LabelsPackPlanCartonDelete
        /// </summary>
        /// <param name="cartonNumber">cartonNumber</param>
        /// <returns>DualResult</returns>
        public DualResult LabelsPackPlanCartonDelete(string cartonNumber)
        {
            RequestLabelsPackPlanDelete.Envelope posBody = new RequestLabelsPackPlanDelete.Envelope()
            {
                Body = new RequestLabelsPackPlanDelete.Body()
                {
                    LabelsPackPlanDelete = new RequestLabelsPackPlanDelete.LabelsPackPlanDelete()
                    {
                        Input = new RequestLabelsPackPlanDelete.Input()
                        {
                            FactoryCode = this.factoryCode,
                            OrderNumber = cartonNumber,
                        },
                    },
                },
            };

            string soapRequest = this.SerializeNikeMercuryXml(posBody);

            Dictionary<string, string> dicHeader = new Dictionary<string, string>
            {
                { "SOAPAction", "http://tempuri.org/ILabels/LabelsPackPlanCartonDelete"},
            };

            HttpContent httpContent = new StringContent(soapRequest, Encoding.UTF8, "text/xml");
            WebApiBaseResult webApiBaseResult = WebApiTool.WebApiSend(this.serviceUrl, string.Empty, soapRequest, HttpMethod.Post, httpContent: httpContent, headers: dicHeader);

            if (!webApiBaseResult.isSuccess)
            {
                return new DualResult(false, webApiBaseResult.httpStatusCode.ToString() + webApiBaseResult.responseContent);
            }

            ResponseLabelsPackPlanCartonDelete.Envelope responseResult;

            DualResult resultDeserialize = this.DeserializeNikeMercuryXml<ResponseLabelsPackPlanCartonDelete.Envelope>(webApiBaseResult.responseContent, out responseResult);

            if (!resultDeserialize)
            {
                return resultDeserialize;
            }

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

        /// <summary>
        /// OrdersDataGet
        /// </summary>
        /// <param name="orderNumber">orderNumber</param>
        /// <param name="outputData">outputData</param>
        /// <returns>DualResult</returns>
        public DualResult OrdersDataGet(string orderNumber, out ResponseOrdersDataGet.OutputData outputData)
        {
            outputData = new ResponseOrdersDataGet.OutputData();

            RequestOrdersDataGet.Envelope posBody = new RequestOrdersDataGet.Envelope()
            {
                Body = new RequestOrdersDataGet.Body()
                {
                    OrdersDataGet = new RequestOrdersDataGet.OrdersDataGet()
                    {
                        Input = new RequestOrdersDataGet.Input()
                        {
                            FactoryCode = this.factoryCode,
                            OrderNumber = orderNumber,
                        },
                    },
                },
            };

            string soapRequest = this.SerializeNikeMercuryXml(posBody);

            Dictionary<string, string> dicHeader = new Dictionary<string, string>
            {
                { "SOAPAction", "http://tempuri.org/IOrders/OrdersDataGet" },
            };

            HttpContent httpContent = new StringContent(soapRequest, Encoding.UTF8, "text/xml");
            WebApiBaseResult webApiBaseResult = WebApiTool.WebApiSend(this.serviceUrl, string.Empty, soapRequest, HttpMethod.Post, httpContent: httpContent, headers: dicHeader);

            if (!webApiBaseResult.isSuccess)
            {
                return new DualResult(false, webApiBaseResult.httpStatusCode.ToString() + webApiBaseResult.responseContent);
            }

            ResponseOrdersDataGet.Envelope responseResult;

            DualResult resultDeserialize = this.DeserializeNikeMercuryXml<ResponseOrdersDataGet.Envelope>(webApiBaseResult.responseContent, out responseResult);

            if (!resultDeserialize)
            {
                return resultDeserialize;
            }

            outputData = responseResult.Body.OrdersDataGetResponse.OrdersDataGetResult.OutputData;

            return new DualResult(true);
        }
    }
}
