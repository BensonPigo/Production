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
using static Sci.Production.Prg.Entity.NikeMercury.ResponseShipmentScannedCartonUploadLSP;
using static Sci.Production.Prg.Entity.NikeMercury.ResponseShipmentShippingDetailsUpdateLSP;

namespace Sci.Production.Automation
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
                    string mercuryUrl = UtilityAutomation.GetSupplierUrl("NIKE", "MERCURY");
                    staticService = new WebServiceNikeMercury(mercuryUrl);
                }

                return staticService;
            }
        }

        private string serviceUrl = string.Empty;
        public string lastRequestXml = string.Empty;
        public string lastResponseXml = string.Empty;
        private string factoryCode = string.Empty;
        private string nikeStickerPrintServer = string.Empty;

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
            DataRow drInitialValues;
            if (MyUtility.Check.Seek("select NikeFactoryCode, NikeStickerPrintServer from system", out drInitialValues))
            {
                this.factoryCode = drInitialValues["NikeFactoryCode"].ToString();
                this.nikeStickerPrintServer = drInitialValues["NikeStickerPrintServer"].ToString();
            }
         }

        /// <summary>
        /// SerializeNikeMercuryXml
        /// </summary>
        /// <param name="xmlObject">xmlObject</param>
        /// <param name="ollNamespaces">ollNamespaces</param>
        /// <returns>string</returns>
        public string SerializeNikeMercuryXml(object xmlObject, string ollNamespaces = "http://schemas.datacontract.org/2004/07/OLLIeLabels")
        {
            XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
            xmlSerializerNamespaces.Add("soapenv", "http://schemas.xmlsoap.org/soap/envelope/");
            xmlSerializerNamespaces.Add("tem", "http://tempuri.org/");
            xmlSerializerNamespaces.Add("oll", ollNamespaces);

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
        [BuildQty] = (pg.ShipQty / pg.QtyPerCTN) * pg.QtyPerCTN,
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
            WebApiBaseResult webApiBaseResult = WebApiTool.WebApiSend(this.serviceUrl, "Labels", soapRequest, HttpMethod.Post, httpContent: httpContent, headers: dicHeader);

            if (!webApiBaseResult.isSuccess)
            {
                soapRequest = this.SerializeNikeMercuryXml(posBody);
                httpContent = new StringContent(soapRequest, Encoding.UTF8, "text/xml");
                webApiBaseResult = WebApiTool.WebApiSend(this.serviceUrl, "Labels", soapRequest, HttpMethod.Post, httpContent: httpContent, headers: dicHeader);
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
            WebApiBaseResult webApiBaseResult = WebApiTool.WebApiSend(this.serviceUrl, "Labels", soapRequest, HttpMethod.Post, httpContent: httpContent, headers: dicHeader);

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
                string addSize = cartonInfo.AddCartonContent.AddCartonContentInput.Select(s => s.OrderSizeDescription).JoinToString(",");
                return new DualResult(false, "Size:" + addSize + Environment.NewLine + labelsPackPlanCartonAddResult.OutputMessage.ReturnDescription);
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
            WebApiBaseResult webApiBaseResult = WebApiTool.WebApiSend(this.serviceUrl, "Labels", soapRequest, HttpMethod.Post, httpContent: httpContent, headers: dicHeader);

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

            ResponseLabelsPackPlanDelete.OutputMessage outputMessage = responseResult.Body.LabelsPackPlanDeleteResponse.LabelsPackPlanDeleteResult.OutputMessage;

            if (outputMessage.ReturnCode < 0 && outputMessage.ReturnDescription != "Item has not been built. The pack plan does not exist.")
            {
                return new DualResult(false, outputMessage.ReturnDescription);
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
            RequestLabelsPackPlanCartonDelete.Envelope posBody = new RequestLabelsPackPlanCartonDelete.Envelope()
            {
                Body = new RequestLabelsPackPlanCartonDelete.Body()
                {
                    LabelsPackPlanCartonDelete = new RequestLabelsPackPlanCartonDelete.LabelsPackPlanCartonDelete()
                    {
                        Input = new RequestLabelsPackPlanCartonDelete.Input()
                        {
                            FactoryCode = this.factoryCode,
                            CartonNumber = cartonNumber,
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
            WebApiBaseResult webApiBaseResult = WebApiTool.WebApiSend(this.serviceUrl, "Labels", soapRequest, HttpMethod.Post, httpContent: httpContent, headers: dicHeader);

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
            WebApiBaseResult webApiBaseResult = WebApiTool.WebApiSend(this.serviceUrl, "Labels", soapRequest, HttpMethod.Post, httpContent: httpContent, headers: dicHeader);

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

            string soapRequest = this.SerializeNikeMercuryXml(posBody, "http://schemas.datacontract.org/2004/07/OLLIeOrders");

            Dictionary<string, string> dicHeader = new Dictionary<string, string>
            {
                { "SOAPAction", "http://tempuri.org/IOrders/OrdersDataGet" },
            };

            HttpContent httpContent = new StringContent(soapRequest, Encoding.UTF8, "text/xml");
            WebApiBaseResult webApiBaseResult = WebApiTool.WebApiSend(this.serviceUrl, "Orders", soapRequest, HttpMethod.Post, httpContent: httpContent, headers: dicHeader);

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

        /// <summary>
        /// LabelsGS1128CartonPrintByCartonRange
        /// </summary>
        /// <param name="cartonNumber">cartonNumber</param>
        /// <returns>DualResult</returns>
        public DualResult LabelsGS1128CartonPrintByCartonRange(string cartonNumber)
        {
            RequestLabelsGS1128CartonPrintByCartonRange.Envelope posBody = new RequestLabelsGS1128CartonPrintByCartonRange.Envelope()
            {
                Body = new RequestLabelsGS1128CartonPrintByCartonRange.Body()
                {
                    LabelsGS1128CartonPrintByCartonRange = new RequestLabelsGS1128CartonPrintByCartonRange.LabelsGS1128CartonPrintByCartonRange()
                    {
                        Input = new RequestLabelsGS1128CartonPrintByCartonRange.Input()
                        {
                            FactoryCode = this.factoryCode,
                            PrintServerName = this.nikeStickerPrintServer,
                            CartonNumberFrom = cartonNumber,
                        },
                    },
                },
            };

            string soapRequest = this.SerializeNikeMercuryXml(posBody);

            Dictionary<string, string> dicHeader = new Dictionary<string, string>
            {
                { "SOAPAction", "http://tempuri.org/ILabels/LabelsGS1128CartonPrintByCartonRange"},
            };

            HttpContent httpContent = new StringContent(soapRequest, Encoding.UTF8, "text/xml");
            WebApiBaseResult webApiBaseResult = WebApiTool.WebApiSend(this.serviceUrl, "Labels", soapRequest, HttpMethod.Post, httpContent: httpContent, headers: dicHeader);

            if (!webApiBaseResult.isSuccess)
            {
                return new DualResult(false, webApiBaseResult.httpStatusCode.ToString() + webApiBaseResult.responseContent);
            }

            ResponseLabelsGS1128CartonPrintByCartonRange.Envelope responseResult;

            DualResult resultDeserialize = this.DeserializeNikeMercuryXml<ResponseLabelsGS1128CartonPrintByCartonRange.Envelope>(webApiBaseResult.responseContent, out responseResult);

            if (!resultDeserialize)
            {
                return resultDeserialize;
            }

            string returnDescription = responseResult.Body.LabelsGS1128CartonPrintByCartonRangeResponse.LabelsGS1128CartonPrintByCartonRangeResult.OutputMessage.ReturnDescription;

            // 有可能回傳成功但只建立0箱的情況，要回傳false
            // 0 Carton Label(s) added to the print queue.
            string createdCartonCnt = returnDescription.Split(' ')[0];

            if (createdCartonCnt == "0")
            {
                return new DualResult(false, new Exception($"CartonNumber:{cartonNumber}" + Environment.NewLine + returnDescription));
            }

            return new DualResult(true);
        }

        /// <summary>
        /// ShipmentScannedCartonUploadLSP
        /// </summary>
        /// <param name="shipmentInfo">shipmentInfo</param>
        /// <param name="shipmentNumber">shipmentNumber</param>
        /// <returns>DualResult</returns>
        public DualResult ShipmentScannedCartonUploadLSP(RequestShipmentScannedCartonUploadLSP.Input shipmentInfo, out string shipmentNumber)
        {
            shipmentNumber = string.Empty;

            if (MyUtility.Check.Empty(this.factoryCode))
            {
                return new DualResult(false, "NikeFactoryCode is empty");
            }

            shipmentInfo.FactoryCode = this.factoryCode;
            RequestShipmentScannedCartonUploadLSP.Envelope posBody = new RequestShipmentScannedCartonUploadLSP.Envelope()
            {
                Body = new RequestShipmentScannedCartonUploadLSP.Body()
                {
                    ShipmentScannedCartonUploadLSP = new RequestShipmentScannedCartonUploadLSP.ShipmentScannedCartonUploadLSP()
                    {
                        Input = shipmentInfo,
                    },
                },
            };

            string soapRequest = this.SerializeNikeMercuryXml(posBody, "http://schemas.datacontract.org/2004/07/OLLIeLogistics");

            Dictionary<string, string> dicHeader = new Dictionary<string, string>
            {
                { "SOAPAction", "http://tempuri.org/ILogistics/ShipmentScannedCartonUploadLSP" },
            };

            HttpContent httpContent = new StringContent(soapRequest, Encoding.UTF8, "text/xml");
            WebApiBaseResult webApiBaseResult = WebApiTool.WebApiSend(this.serviceUrl, "Logistics", soapRequest, HttpMethod.Post, httpContent: httpContent, headers: dicHeader);

            if (!webApiBaseResult.isSuccess)
            {
                return new DualResult(false, webApiBaseResult.httpStatusCode.ToString() + webApiBaseResult.responseContent);
            }

            ResponseShipmentScannedCartonUploadLSP.Envelope responseResult;

            DualResult resultDeserialize = this.DeserializeNikeMercuryXml<ResponseShipmentScannedCartonUploadLSP.Envelope>(webApiBaseResult.responseContent, out responseResult);

            if (!resultDeserialize)
            {
                return resultDeserialize;
            }

            ShipmentScannedCartonUploadLSPResult shipmentScannedCartonUploadLSPResult = responseResult.Body.ShipmentScannedCartonUploadLSPResponse.ShipmentScannedCartonUploadLSPResult;

            if (shipmentScannedCartonUploadLSPResult.OutputMessage.ReturnCode == -1)
            {
                return new DualResult(false, shipmentScannedCartonUploadLSPResult.OutputMessage.ReturnDescription);
            }

            shipmentNumber = shipmentScannedCartonUploadLSPResult.FactoryHubShipmentNumber;

            return new DualResult(true, shipmentScannedCartonUploadLSPResult.OutputMessage.ReturnDescription);
        }

        /// <summary>
        /// ShipmentShippingDetailsUpdateLSP
        /// </summary>
        /// <param name="shipmentInfo">shipmentInfo</param>
        /// <returns>DualResult</returns>
        public DualResult ShipmentShippingDetailsUpdateLSP(RequestShipmentShippingDetailsUpdateLSP.Input shipmentInfo)
        {
            if (MyUtility.Check.Empty(this.factoryCode))
            {
                return new DualResult(false, "NikeFactoryCode is empty");
            }

            shipmentInfo.FactoryCode = this.factoryCode;
            RequestShipmentShippingDetailsUpdateLSP.Envelope posBody = new RequestShipmentShippingDetailsUpdateLSP.Envelope()
            {
                Body = new RequestShipmentShippingDetailsUpdateLSP.Body()
                {
                    ShipmentShippingDetailsUpdateLSP = new RequestShipmentShippingDetailsUpdateLSP.ShipmentShippingDetailsUpdateLSP()
                    {
                        Input = shipmentInfo,
                    },
                },
            };

            string soapRequest = this.SerializeNikeMercuryXml(posBody, "http://schemas.datacontract.org/2004/07/OLLIeLogistics");

            Dictionary<string, string> dicHeader = new Dictionary<string, string>
            {
                { "SOAPAction", "http://tempuri.org/ILogistics/ShipmentShippingDetailsUpdateLSP" },
            };

            HttpContent httpContent = new StringContent(soapRequest, Encoding.UTF8, "text/xml");
            WebApiBaseResult webApiBaseResult = WebApiTool.WebApiSend(this.serviceUrl, "Logistics", soapRequest, HttpMethod.Post, httpContent: httpContent, headers: dicHeader);

            if (!webApiBaseResult.isSuccess)
            {
                return new DualResult(false, soapRequest + webApiBaseResult.httpStatusCode.ToString() + webApiBaseResult.responseContent);
            }

            ResponseShipmentShippingDetailsUpdateLSP.Envelope responseResult;

            DualResult resultDeserialize = this.DeserializeNikeMercuryXml<ResponseShipmentShippingDetailsUpdateLSP.Envelope>(webApiBaseResult.responseContent, out responseResult);

            if (!resultDeserialize)
            {
                return resultDeserialize;
            }

            ShipmentShippingDetailsUpdateLSPResult shipmentShippingDetailsUpdateLSPResult = responseResult.Body.ShipmentShippingDetailsUpdateLSPResponse.ShipmentShippingDetailsUpdateLSPResult;

            if (shipmentShippingDetailsUpdateLSPResult.OutputMessage.ReturnCode == -1)
            {
                return new DualResult(false, shipmentShippingDetailsUpdateLSPResult.OutputMessage.ReturnDescription);
            }

            return new DualResult(true);
        }

        /// <summary>
        /// ShipmentCommercialDocumentsBinaryArrayPDF
        /// </summary>
        /// <param name="docInfo">docInfo</param>
        /// <returns>DualResult</returns>
        public DualResult ShipmentCommercialDocumentsBinaryArrayPDF(RequestShipmentCommercialDocumentsFileLocationPDF.Input docInfo)
        {
            if (MyUtility.Check.Empty(this.factoryCode))
            {
                return new DualResult(false, "NikeFactoryCode is empty");
            }

            docInfo.FactoryCode = this.factoryCode;
            RequestShipmentCommercialDocumentsFileLocationPDF.Envelope posBody = new RequestShipmentCommercialDocumentsFileLocationPDF.Envelope()
            {
                Body = new RequestShipmentCommercialDocumentsFileLocationPDF.Body()
                {
                    ShipmentCommercialDocumentsFileLocationPDF = new RequestShipmentCommercialDocumentsFileLocationPDF.ShipmentCommercialDocumentsFileLocationPDF()
                    {
                        Input = docInfo,
                    },
                },
            };

            string soapRequest = this.SerializeNikeMercuryXml(posBody, "http://schemas.datacontract.org/2004/07/OLLIeLogistics");

            Dictionary<string, string> dicHeader = new Dictionary<string, string>
            {
                { "SOAPAction", "http://tempuri.org/ILogistics/ShipmentShippingDetailsUpdateLSP" },
            };

            HttpContent httpContent = new StringContent(soapRequest, Encoding.UTF8, "text/xml");
            WebApiBaseResult webApiBaseResult = WebApiTool.WebApiSend(this.serviceUrl, "Logistics", soapRequest, HttpMethod.Post, httpContent: httpContent, headers: dicHeader);

            if (!webApiBaseResult.isSuccess)
            {
                return new DualResult(false, webApiBaseResult.httpStatusCode.ToString() + webApiBaseResult.responseContent);
            }

            // 回傳解析內容等SA確認後再進行
            //ResponseShipmentShippingDetailsUpdateLSP.Envelope responseResult;

            //DualResult resultDeserialize = this.DeserializeNikeMercuryXml<ResponseShipmentShippingDetailsUpdateLSP.Envelope>(webApiBaseResult.responseContent, out responseResult);

            //if (!resultDeserialize)
            //{
            //    return resultDeserialize;
            //}

            //ShipmentShippingDetailsUpdateLSPResult shipmentShippingDetailsUpdateLSPResult = responseResult.Body.ShipmentShippingDetailsUpdateLSPResponse.ShipmentShippingDetailsUpdateLSPResult;

            //if (shipmentShippingDetailsUpdateLSPResult.OutputMessage.ReturnCode == -1)
            //{
            //    return new DualResult(false, shipmentShippingDetailsUpdateLSPResult.OutputMessage.ReturnDescription);
            //}

            return new DualResult(true);
        }
    }
}
