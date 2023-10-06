using Ict;
using Sci.Data;
using Sci.Production.Prg;
using Sci.Production.Prg.Entity.NikeMercury;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Class
{
    /// <summary>
    /// NikeMercuryWebServiceTest
    /// </summary>
    public partial class P03_NikeMercuryWebServiceTest : Sci.Win.Tems.QueryForm
    {
        private string packID;
        private string orderNumber;
        private string orderNumber2;
        private string orderItem;
        public P03_NikeMercuryWebServiceTest(string packID)
        {
            this.InitializeComponent();
            this.EditMode = true;
            this.packID = packID;

            string sqlGetData = $@"
select  CustPONo, Customize1
from    PackingGuide p with (nolock)
inner join  Orders o with (nolock) on o.ID = p.OrderID
where p.ID = '{this.packID}'
";
            DataTable dtResult;
            DualResult result = DBProxy.Current.Select(null, sqlGetData, out dtResult);
            this.txtPackID.Text = packID;
            if (!result)
            {
                this.ShowErr(result);
            }
            else
            {
                DataRow drOrderInfo = dtResult.Rows[0];
                string[] orderInfo = drOrderInfo["CustPONo"].ToString().Split('-');
                this.orderNumber = orderInfo[0];
                this.orderNumber2 = drOrderInfo["Customize1"].ToString();
                this.orderItem = orderInfo.Length < 2 ? string.Empty : orderInfo[1];
                this.txtOrderInfo.Text = $"{this.orderNumber} {this.orderNumber2} - {this.orderItem}";
            }
        }

        private void BtnLabelsPackPlanCreate_Click(object sender, EventArgs e)
        {
            DualResult result = WebServiceNikeMercury.StaticService.LabelsPackPlanCreate(this.packID);

            if (!MyUtility.Check.Empty(result.Description))
            {
                MyUtility.Msg.InfoBox("Description:" + result.Description);
                return;
            }

            if (!result)
            {
                this.ShowErr(result);
            }

            this.editBoxRequestXml.Text = WebServiceNikeMercury.StaticService.lastRequestXml;
            this.editBoxResponseXml.Text = WebServiceNikeMercury.StaticService.lastResponseXml;
        }

        private void BtnLabelsPackPlanDelete_Click(object sender, EventArgs e)
        {
            DualResult result = WebServiceNikeMercury.StaticService.LabelsPackPlanDelete("SNY", this.orderNumber2, this.orderItem);

            if (!MyUtility.Check.Empty(result.Description))
            {
                MyUtility.Msg.InfoBox("Description:" + result.Description);
                return;
            }

            if (!result)
            {
                this.ShowErr(result);
            }

            this.editBoxRequestXml.Text = WebServiceNikeMercury.StaticService.lastRequestXml;
            this.editBoxResponseXml.Text = WebServiceNikeMercury.StaticService.lastResponseXml;
        }

        private void BtnTestDeserialize_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.editBoxResponseXml.Text))
            {
                return;
            }

            ResponseLabelsPackPlanCartonAdd.Envelope responseResult;

            DualResult resultDeserialize = WebServiceNikeMercury.StaticService.DeserializeNikeMercuryXml<ResponseLabelsPackPlanCartonAdd.Envelope>(this.editBoxResponseXml.Text, out responseResult);

        }

        private void BtnLabelsPackPlanCartonAdd_Click(object sender, EventArgs e)
        {
            RequestLabelsPackPlanCartonAdd.Input packPlanCartonAdd = new RequestLabelsPackPlanCartonAdd.Input()
            {
                FactoryCode = "SNY",
                CartonTypeCode = "A4",
                AddCartonContent = new RequestLabelsPackPlanCartonAdd.AddCartonContent()
                {
                    AddCartonContentInput = new List<RequestLabelsPackPlanCartonAdd.AddCartonContentInput>()
                    {
                        new RequestLabelsPackPlanCartonAdd.AddCartonContentInput()
                        {
                            OrderNumber = this.orderNumber2,
                            OrderItem = this.orderItem,
                            OrderSizeDescription = "M",
                            PackPlanQty = 10,
                        },
                        new RequestLabelsPackPlanCartonAdd.AddCartonContentInput()
                        {
                            OrderNumber = this.orderNumber2,
                            OrderItem = this.orderItem,
                            OrderSizeDescription = "L",
                            PackPlanQty = 12,
                        },
                    },
                },
            };

            DualResult result = WebServiceNikeMercury.StaticService.LabelsPackPlanAdd(packPlanCartonAdd);

            if (!MyUtility.Check.Empty(result.Description))
            {
                MyUtility.Msg.InfoBox("Description:" + result.Description);
                return;
            }

            if (!result)
            {
                this.ShowErr(result);
            }

            this.editBoxRequestXml.Text = WebServiceNikeMercury.StaticService.lastRequestXml;
            this.editBoxResponseXml.Text = WebServiceNikeMercury.StaticService.lastResponseXml;
        }

        private void BtnLabelsPackPlanCartonUpdate_Click(object sender, EventArgs e)
        {
            RequestLabelsPackPlanCartonUpdate.Input packPlanCartonUpdate = new RequestLabelsPackPlanCartonUpdate.Input()
            {
                FactoryCode = "SNY",
                CartonNumber = this.txtCartonNumber.Text,
                CartonContent = new List<RequestLabelsPackPlanCartonUpdate.CartonContent>()
                {
                    new RequestLabelsPackPlanCartonUpdate.CartonContent(){
                        OrderNumber = this.orderNumber2,
                        OrderItem = this.orderItem,
                        OrderSizeDescription = "L",
                        PackPlanQty = 22,
                    },
                    new RequestLabelsPackPlanCartonUpdate.CartonContent(){
                        OrderNumber = this.orderNumber2,
                        OrderItem = this.orderItem,
                        OrderSizeDescription = "M",
                        PackPlanQty = 25,
                    },
                },
            };

            DualResult result = WebServiceNikeMercury.StaticService.LabelsPackPlanUpdate(packPlanCartonUpdate);

            if (!MyUtility.Check.Empty(result.Description))
            {
                MyUtility.Msg.InfoBox("Description:" + result.Description);
                return;
            }

            if (!result)
            {
                this.ShowErr(result);
            }

            this.editBoxRequestXml.Text = WebServiceNikeMercury.StaticService.lastRequestXml;
            this.editBoxResponseXml.Text = WebServiceNikeMercury.StaticService.lastResponseXml;
        }
    }
}
