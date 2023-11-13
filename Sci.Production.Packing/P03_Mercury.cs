using Ict;
using Ict.Win;
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
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Packing
{
    /// <summary>
    /// P03_Mercury
    /// </summary>
    public partial class P03_Mercury : Sci.Win.Tems.QueryForm
    {
        private string packID = string.Empty;

        /// <summary>
        /// P03_Mercury
        /// </summary>
        /// <param name="packID">packID</param>
        public P03_Mercury(string packID)
        {
            this.InitializeComponent();
            this.packID = packID;
            this.gridDownloadStickerQueue.CellFormatting += this.GridDownloadStickerQueue_CellFormatting;
            this.timerRefreshDownloadStickerQueue.Interval = 10000;
        }

        private void GridDownloadStickerQueue_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < this.gridDownloadStickerQueue.Rows.Count)
            {
                DataGridViewRow row = this.gridDownloadStickerQueue.Rows[e.RowIndex];

                DataRow curRow = this.gridDownloadStickerQueue.GetDataRow(e.RowIndex);

                // 在這裡根據Row的內容設置背景色
                if (MyUtility.Convert.GetBool(curRow["Processing"]))
                {
                    row.DefaultCellStyle.BackColor = Color.LightGreen;
                }
                else
                {
                    row.DefaultCellStyle.BackColor = Color.White;
                }
            }
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.Helper.Controls.Grid.Generator(this.gridDownloadStickerQueue)
                .Text("PackingID", header: "Pack ID", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Numeric("CTNQty", header: "Ttl Ctns", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Numeric("ShipQty", header: "Ship Qty", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .EditText("ErrorMsg", header: "Error msg", width: Widths.AnsiChars(25), iseditingreadonly: true)
                .DateTime("AddDate", header: "Add Date", iseditingreadonly: true)
                .DateTime("UpdateDate", header: "Update Date", iseditingreadonly: true);

            this.Query();
            this.timerRefreshDownloadStickerQueue.Enabled = true;
            this.timerRefreshDownloadStickerQueue.Start();
        }

        private void Query()
        {
            string sqlGetDownloadStickerQueue = $@"
select  dq.PackingID,
        p.CTNQty,
        p.ShipQty,
        dq.ErrorMsg,
        dq.AddDate,
        dq.UpdateDate,
        dq.Processing
from DownloadStickerQueue dq with (nolock)
inner join PackingList p with (nolock) on p.ID = dq.PackingID
order by    dq.UpdateDate
";
            DataTable dtResult;
            DualResult result = DBProxy.Current.Select(null, sqlGetDownloadStickerQueue, out dtResult);

            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.gridDownloadStickerQueue.DataSource = dtResult;
        }

        private void BtnUploadPL_Click(object sender, EventArgs e)
        {
            DualResult result;
            string sqlGetOrderInfo = $@"
select  CustPONo, [CustPONo2] = case when   b.Customize1 = 'TRADING PO' then o.Customize1
                                     when   b.Customize2 = 'TRADING PO' then o.Customize2
                                     when   b.Customize3 = 'TRADING PO' then o.Customize3
                                     else '' end
from    PackingGuide p with (nolock)
inner join  Orders o with (nolock) on o.ID = p.OrderID
inner join  Brand b with (nolock) on b.ID = o.BrandID
where p.ID = '{this.packID}'
";

            DataRow drOrderInfo;

            if (!MyUtility.Check.Seek(sqlGetOrderInfo, out drOrderInfo))
            {
                MyUtility.Msg.WarningBox("Order information not found");
                return;
            }

            #region 檢查訂單是否存在Mercury
            ResponseOrdersDataGet.OutputData nikeOrderInfo;

            string[] orderInfo = drOrderInfo["CustPONo"].ToString().Split('-');
            string orderNumber = orderInfo[0];
            string orderNumber2 = drOrderInfo["CustPONo2"].ToString();
            string orderItem = orderInfo.Length < 2 ? string.Empty : orderInfo[1];
            string nikeOrderNumber = string.Empty;

            result = WebServiceNikeMercury.StaticService.OrdersDataGet(orderNumber, out nikeOrderInfo);

            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            nikeOrderNumber = orderNumber;

            if (nikeOrderInfo.DataGetOrderItems.Length == 0)
            {
                result = WebServiceNikeMercury.StaticService.OrdersDataGet(orderNumber2, out nikeOrderInfo);

                if (!result)
                {
                    this.ShowErr(result);
                    return;
                }

                nikeOrderNumber = orderNumber2;
            }

            if (!nikeOrderInfo.DataGetOrderItems.Any(s => s.PO_Item == orderItem))
            {
                MyUtility.Msg.WarningBox("Cannot found order number and item on Mercury, please check with MR.");
                return;
            }
            #endregion

            #region Delete Mercury packing data
            result = WebServiceNikeMercury.StaticService.LabelsPackPlanDelete(nikeOrderNumber, orderItem);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }
            #endregion

            #region 建立Packing到Mercury
            string sqlGetPack = $@"
select  pd.CTNStartNo,
        pd.SizeCode,
        pd.ShipQty,
        l.NikeCartonType
from PackingList_Detail pd with (nolock)
left join LocalItem l with (nolock) on l.Refno = pd.Refno
where   ID = '{this.packID}'
order by pd.Seq
";

            DataTable dtPack;

            result = DBProxy.Current.Select(null, sqlGetPack, out dtPack);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            var groupPack = dtPack.AsEnumerable().GroupBy(s => s["CTNStartNo"].ToString());
            List<RequestLabelsPackPlanCreate.PackPlanOrderSize> listPackSizeData = new List<RequestLabelsPackPlanCreate.PackPlanOrderSize>();
            List<UpdatePackingInfo> listUpdatePackingInfo = new List<UpdatePackingInfo>();

            int totalCtn = groupPack.Count();
            this.lblUploadProgressStatus.Text = $"Uploading Carton 1 of {totalCtn}";
            this.progressBarUploadPL.Value = 1;
            this.progressBarUploadPL.Maximum = totalCtn;
            this.panelUploadProgress.Visible = true;
            Application.DoEvents();

            // 先用 LabelsPackPlanCreate
            List<ResponseLabelsPackPlanCreate.Carton> listMercuryCarton;

            result = WebServiceNikeMercury.StaticService.LabelsPackPlanCreate(this.packID, nikeOrderNumber, orderItem, out listMercuryCarton);
            if (!result)
            {
                this.panelUploadProgress.Visible = false;
                this.ShowErr(result);
                return;
            }

            int iterationCount = 1; // 用來記錄迭代次數的變數

            foreach (var groupItem in groupPack)
            {
                // PackingList_Detail與Mercury回傳資訊比對是否有建立成功
                List<ResponseLabelsPackPlanCreate.CartonContent> sciCartonInfo = groupItem.Select(s => new ResponseLabelsPackPlanCreate.CartonContent
                {
                    OrderSizeDescription = s["SizeCode"].ToString(),
                    PackPlanQty = MyUtility.Convert.GetInt(s["ShipQty"]),
                }).ToList();

                ResponseLabelsPackPlanCreate.Carton findMercuryCarton = listMercuryCarton
                    .Find(s => s.Content.CartonContent.SequenceEqual(sciCartonInfo));

                // 兩邊都有match，將mercury資訊中刪掉對應資料
                if (findMercuryCarton != null)
                {
                    listUpdatePackingInfo.Add(new UpdatePackingInfo
                    {
                        CtnStartNo = groupItem.Key,
                        CustCtn = findMercuryCarton.CartonBarcodeNumber,
                        CustCtn2 = findMercuryCarton.CartonNumber,
                    });
                    listMercuryCarton.Remove(findMercuryCarton);
                    continue;
                }

                // 沒有Match，用LabelsPackPlanAdd手動增加
                ResponseLabelsPackPlanCartonAdd.Carton mercuryCarton;

                List<RequestLabelsPackPlanCartonAdd.AddCartonContentInput> listCartonContentInput = new List<RequestLabelsPackPlanCartonAdd.AddCartonContentInput>();

                listCartonContentInput = groupItem
                    .Select(s => new RequestLabelsPackPlanCartonAdd.AddCartonContentInput()
                    {
                        OrderNumber = nikeOrderNumber,
                        OrderItem = orderItem,
                        OrderSizeDescription = s["SizeCode"].ToString(),
                        PackPlanQty = MyUtility.Convert.GetInt(s["ShipQty"]),
                    }).ToList();

                RequestLabelsPackPlanCartonAdd.Input cartonInfo = new RequestLabelsPackPlanCartonAdd.Input()
                {
                    FactoryCode = WebServiceNikeMercury.StaticService.FactoryCode,
                    CartonTypeCode = groupItem.First()["NikeCartonType"].ToString(),
                    AddCartonContent = new RequestLabelsPackPlanCartonAdd.AddCartonContent() { AddCartonContentInput = listCartonContentInput },
                };

                result = WebServiceNikeMercury.StaticService.LabelsPackPlanAdd(cartonInfo, out mercuryCarton);
                if (!result)
                {
                    this.panelUploadProgress.Visible = false;
                    this.ShowErr(result);
                    return;
                }

                listUpdatePackingInfo.Add(new UpdatePackingInfo
                {
                    CtnStartNo = groupItem.Key,
                    CustCtn = mercuryCarton.CartonBarcodeNumber,
                    CustCtn2 = mercuryCarton.CartonNumber,
                });

                this.lblUploadProgressStatus.Text = $"Uploading Carton {iterationCount} of {totalCtn}";
                this.progressBarUploadPL.Value = iterationCount;
                Application.DoEvents();
                Thread.Sleep(100);
                iterationCount++;
            }

            // 將Mercury系統中沒有match我們Packinglist_Detail的資料刪掉
            foreach (ResponseLabelsPackPlanCreate.Carton mercuryCartonItem in listMercuryCarton)
            {
                result = WebServiceNikeMercury.StaticService.LabelsPackPlanCartonDelete(mercuryCartonItem.CartonNumber);
                if (!result)
                {
                    this.panelUploadProgress.Visible = false;
                    this.ShowErr(result);
                    return;
                }
            }

            // update Packinglist_Detail.CustCTN
            string sqlUpdate = listUpdatePackingInfo
                .Select(s => $"update PackingList_Detail set CustCTN = '{s.CustCtn}', CustCTN2 = '{s.CustCtn2}' where ID = '{this.packID}' and CTNStartNo = '{s.CtnStartNo}'")
                .JoinToString(Environment.NewLine);

            using (TransactionScope transactionScope = new TransactionScope())
            {
                result = DBProxy.Current.Execute(null, sqlUpdate);

                if (!result)
                {
                    this.panelUploadProgress.Visible = false;
                    transactionScope.Dispose();
                    this.ShowErr(result);
                    return;
                }

                transactionScope.Complete();
                this.panelUploadProgress.Visible = false;
            }
            #endregion
        }

        private void P03_Mercury_Resize(object sender, EventArgs e)
        {
            // 計算控制元件的新位置CheckShippingMarkSetting
            int x = (this.ClientSize.Width - this.panelUploadProgress.Width) / 2;
            int y = (this.ClientSize.Height - this.panelUploadProgress.Height) / 2;

            // 設定控制元件的新位置
            this.panelUploadProgress.Location = new Point(x, y);
        }

        private void BtnDownloadStickerFile_Click(object sender, EventArgs e)
        {
            // 檢查packing是否可以download mercury sticker
            string sqlCheck = $@"
--檢查CustCTN是否都有值
if exists (select 1 from Packinglist_Detail with (nolock) where ID = '{this.packID}' and CustCTN2 = '')
begin
    select [Result] = 'Mercury carton number can not empty, please do <Upload PL> again'
    return
end

--CheckShippingMarkSetting
select	p.ID,
		pd.SCICtnNo,
		pd.RefNo,
		p.CustCDID,
		p.BrandID,
		[IsMix] = iif(count(*) > 1, 1, 0)
into #tmpPacking
from PackingList p with (nolock)
INNER JOIN PackingList_Detail pd with (nolock) ON p.ID = pd.ID
where p.id = '{this.packID}' 
group by	p.ID,
			pd.SCICtnNo,
			pd.RefNo,
			p.CustCDID,
			p.BrandID

if exists(  SELECT 1
            from #tmpPacking
            where dbo.CheckShippingMarkSetting(ID, SCICtnNo, RefNo, IsMix, CustCDID, BrandID) = 0)
begin
    select [Result] = 'Sticker basic setting not complete yet.'
    return
end

drop table #tmpPacking

if exists(select 1 from DownloadStickerQueue with (nolock) where PackingID = '{this.packID}')
begin
    select [Result] = 'Pack ID exist in download sticker queue.'
    return
end
";
            DataRow drCheckResult;
            if (MyUtility.Check.Seek(sqlCheck, out drCheckResult))
            {
                MyUtility.Msg.WarningBox(drCheckResult["Result"].ToString());
                return;
            }

            string sqlInsertDownloadStickerQueue = $@"
insert into DownloadStickerQueue(PackingID, AddDate, UpdateDate)
            values('{this.packID}', getdate(), getdate())
";

            DualResult result = DBProxy.Current.Execute(null, sqlInsertDownloadStickerQueue);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.Query();
        }

        private void TimerRefreshDownloadStickerQueue_Tick(object sender, EventArgs e)
        {
            this.Query();
        }

        private class UpdatePackingInfo
        {
            public string CtnStartNo { get; set; }
            public string CustCtn { get; set; }
            public string CustCtn2 { get; set; }
        }
    }
}
