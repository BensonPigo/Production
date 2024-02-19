using Ict;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sci.Production.Prg;
using Sci.Production.Prg.Entity.NikeMercury;
using Sci.Production.PublicPrgTests1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Sci.Production.Automation;

namespace Sci.Production.Prg.Tests
{
    [TestClass()]
    public class WebServiceNikeMercuryTests
    {
        [TestInitialize]
        public void Initialize()
        {
            TestInitializer.Initialize();
        }

        [TestMethod()]
        public void OrdersDataGetTest()
        {
            //ResponseLabelsPackPlanCartonDelete.Envelope responseResult;

            //DualResult resultDeserialize = WebServiceNikeMercury.StaticService.DeserializeNikeMercuryXml<ResponseLabelsPackPlanCartonDelete.Envelope>(this.testXML, out responseResult);

            RequestLabelsGS1128CartonPrintByCartonRange.Envelope posBody = new RequestLabelsGS1128CartonPrintByCartonRange.Envelope()
            {
                Body = new RequestLabelsGS1128CartonPrintByCartonRange.Body()
                {
                    LabelsGS1128CartonPrintByCartonRange = new RequestLabelsGS1128CartonPrintByCartonRange.LabelsGS1128CartonPrintByCartonRange()
                    {
                        Input = new RequestLabelsGS1128CartonPrintByCartonRange.Input()
                        {
                            FactoryCode = "sss",
                            CartonNumberFrom = "fff",
                            PrintServerName = "ddd",
                        },
                    },
                },
            };

            string soapRequest = WebServiceNikeMercury.StaticService.SerializeNikeMercuryXml(posBody);



            Assert.IsTrue(true);
        }

        private string testXML = @"<s:Envelope xmlns:s=""http://schemas.xmlsoap.org/soap/envelope/"">
    <s:Body>
        <LabelsPackPlanCartonDeleteResponse xmlns=""http://tempuri.org/"">
            <LabelsPackPlanCartonDeleteResult xmlns:a=""http://schemas.datacontract.org/2004/07/OLLIeLabels"" xmlns:i=""http://www.w3.org/2001/XMLSchema-instance"">
                <a:OutputMessage xmlns:b=""http://schemas.datacontract.org/2004/07/OLLIeFramework"">
                    <b:Cover>
                        <b:UuidReference>3488</b:UuidReference>
                        <b:BusinessTransactionID>3488</b:BusinessTransactionID>
                        <b:SenderID>050957364</b:SenderID>
                        <b:ReceiverID>SNY</b:ReceiverID>
                        <b:MessageType>Delete Pack Plan Carton(s)</b:MessageType>
                        <b:ObjectType>LABELS</b:ObjectType>
                    </b:Cover>
                    <b:ReturnCode>0</b:ReturnCode>
                    <b:ReturnDescription>1 carton(s) deleted.</b:ReturnDescription>
                </a:OutputMessage>
            </LabelsPackPlanCartonDeleteResult>
        </LabelsPackPlanCartonDeleteResponse>
    </s:Body>
</s:Envelope>";
    }
}