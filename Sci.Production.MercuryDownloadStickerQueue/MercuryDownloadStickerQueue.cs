using Bytescout.BarCodeReader;
using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Automation;
using Sci.Production.Prg;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;
using XsPDF.Pdf;

namespace Sci.Production.MercuryDownloadStickerQueue
{
    public partial class MercuryDownloadStickerQueue : Sci.Win.Tems.QueryForm
    {
        private DataTable dtDownloadStickerQueue;
        private string BarcodeReader_RegistrationName = MyUtility.Convert.GetString(ConfigurationManager.AppSettings["BarcodeReader_RegistrationName"]);
        private string BarcodeReader_RegistrationKey = MyUtility.Convert.GetString(ConfigurationManager.AppSettings["BarcodeReader_RegistrationKey"]);
        private TaskScheduler mainTaskScheduler;

        public MercuryDownloadStickerQueue()
        {
            this.InitializeComponent();
            this.gridDownloadStickerQueue.CellFormatting += this.GridDownloadStickerQueue_CellFormatting;
            this.mainTaskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
            this.EditMode = false;
            this.gridDownloadStickerQueue.SupportEditMode = Win.UI.AdvEditModesReadOnly.True;
            this.checkCartonBarcode.IsSupportEditMode = false;
            this.btnRun.EditMode = Win.UI.AdvEditModes.DisableOnEdit;
            this.timerRefresh.Interval = 30000;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.Helper.Controls.Grid.Generator(this.gridDownloadStickerQueue)
                .Text("PackingID", header: "Pack ID", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Numeric("CTNQty", header: "Ttl Ctns", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Numeric("ShipQty", header: "Ship Qty", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .EditText("ErrorMsg", header: "Error msg", width: Widths.AnsiChars(25), iseditingreadonly: true)
                .DateTime("AddDate", header: "Add Date", iseditingreadonly: true)
                .DateTime("UpdateDate", header: "Update Date", iseditingreadonly: true);

            this.timerRefresh.Enabled = true;
            this.timerRefresh.Start();
            this.DoDownloadStickerQueue();
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

        private void Query()
        {
            string sqlGetData = @"
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

            DualResult result = DBProxy.Current.Select("Production", sqlGetData, out this.dtDownloadStickerQueue);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }
            this.gridDownloadStickerQueue.DataSource = this.dtDownloadStickerQueue;
        }

        private void backgroundDownloadSticker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                string runPackingID = this.dtDownloadStickerQueue.AsEnumerable().First()["PackingID"].ToString();

                #region 前置作業 1.抓取Packing資訊 2.清空Mercury pdf
                string sqlGetPaking = $@"
Declare @IsSuccess bit = 0
if exists(select 1 from Packinglist_Detail with (nolock) where ID = '{runPackingID}' and CustCTN = '')
begin
    set @IsSuccess = 0
    update DownloadStickerQueue set Processing = 0, UpdateDate = GetDate(), ErrorMsg = 'Packing List Cust CTN# cannot be empty.'
            where PackingID = '{runPackingID}'
    select [Result] = @IsSuccess
    return
end


update DownloadStickerQueue set Processing = 1, ErrorMsg = '' where PackingID = '{runPackingID}'

set @IsSuccess = 1
select  [Result] = @IsSuccess,
        NikeStickerPrintFileFolder
from dbo.system

--CheckShippingMarkSetting
select	distinct
        pd.SCICtnNo,
		pd.CustCTN,
        pd.CustCTN2
from PackingList_Detail pd with (nolock)
where pd.id = '{runPackingID}'
";
                DataTable[] dtResults;
                DualResult result = DBProxy.Current.Select("Production", sqlGetPaking, out dtResults);
                string returnMessage = string.Empty;

                if (!result)
                {
                    returnMessage = this.UpdateDownloadStickerQueueErr(runPackingID, result);
                    this.backgroundDownloadSticker.ReportProgress(0, returnMessage);
                    return;
                }

                bool isSuccess = MyUtility.Convert.GetBool(dtResults[0].Rows[0]["Result"]);
                if (!isSuccess)
                {
                    return;
                }

                this.backgroundDownloadSticker.ReportProgress(0, string.Empty);

                DataTable dtDownloadSticker = dtResults[1];
                string nikeStickerPrintFileFolder = dtResults[0].Rows[0]["NikeStickerPrintFileFolder"].ToString();

                this.progressBarProcessing.Maximum = dtDownloadSticker.Rows.Count;

                // 先刪除暫存pdf檔案夾下所有pdf
                if (!Directory.Exists(nikeStickerPrintFileFolder))
                {
                    returnMessage = this.UpdateDownloadStickerQueueErr(runPackingID, new DualResult(false, $"Sticker Print File Folder({nikeStickerPrintFileFolder}) not exists"));
                    this.backgroundDownloadSticker.ReportProgress(0, returnMessage);
                    return;
                }

                result = this.ClearNikeStickerPrintFileFolder(nikeStickerPrintFileFolder);
                if (!result)
                {
                    returnMessage = this.UpdateDownloadStickerQueueErr(runPackingID, result);
                    this.backgroundDownloadSticker.ReportProgress(0, returnMessage);
                    return;
                }
                #endregion

                #region 建立共用 SqlConnection 準備temp table資料
                SqlConnection sqlConnectionPMSFile;

                result = DBProxy._OpenConnection("PMSFile", out sqlConnectionPMSFile);

                if (!result)
                {
                    returnMessage = this.UpdateDownloadStickerQueueErr(runPackingID, result);
                    this.backgroundDownloadSticker.ReportProgress(0, returnMessage);
                    return;
                }

                using (sqlConnectionPMSFile)
                {
                    // 先準備temp table資料，後面再將圖片update
                    string sqlCreateTemp = $@"
select	p.ID,
		pd.SCICtnNo,
		pd.RefNo,
		p.CustCDID,
		p.BrandID,
		[IsMixPack] = iif(count(*) > 1, 1, 0)
into #tmpPacking
from PackingList p with (nolock)
INNER JOIN PackingList_Detail pd with (nolock) ON p.ID = pd.ID
where p.id = '{runPackingID}' 
group by	p.ID,
			pd.SCICtnNo,
			pd.RefNo,
			p.CustCDID,
			p.BrandID

SELECT	[PackingListID] = t.ID,
		t.SCICtnNo,
		t.RefNo,
		t.BrandID,
		t.IsMixPack,
		[StickerCombinationUkey] = case when t.IsMixPack = 0 and c.StickerCombinationUkey > 0 then c.StickerCombinationUkey
										when t.IsMixPack = 1 and c.StickerCombinationUkey_MixPack > 0 then c.StickerCombinationUkey_MixPack
										else smc.Ukey end
into #tmp_Combination
from #tmpPacking t
left join ShippingMarkCombination smc with (nolock) on smc.BrandID = t.BrandID and  Category='PIC'  AND IsDefault = 1 AND smc.IsMixPack = t.IsMixPack 
INNER JOIN CustCD c ON t.BrandID = c.BrandID AND t.CustCDID = c.ID


SELECT DISTINCT	t.SCICtnNo
				,[ShippingMarkCombinationUkey] = comb.Ukey
				,combD.ShippingMarkTypeUkey
				,pictD.Side
				,pictD.Seq
				,pictD.Is2Side
				,pictD.IsHorizontal
				,pictD.IsSSCC
				,pictD.FromRight
				,pictD.FromBottom
				,s.Width
				,s.Length
				,pict.CtnHeight
				,pictD.IsOverCtnHt
				,pictD.NotAutomate
FROM #tmp_Combination t
INNER JOIN ShippingMarkCombination comb with (nolock) ON comb.Ukey = t.StickerCombinationUkey
INNER JOIN ShippingMarkCombination_Detail combD with (nolock) ON comb.Ukey = combD.ShippingMarkCombinationUkey
INNER JOIN ShippingMarkPicture pict with (nolock) ON pict.BrandID = t.BrandID AND pict.ShippingMarkCombinationUkey = comb.Ukey AND pict.CTNRefno = t.RefNo AND pict.Category='PIC'
INNER JOIN ShippingMarkPicture_Detail pictD with (nolock) ON pict.Ukey = pictD.ShippingMarkPictureUkey AND pictD.ShippingMarkTypeUkey = combD.ShippingMarkTypeUkey
INNER JOIN StickerSize s with (nolock) ON s.ID = pictD.StickerSizeID
where exists (select 1 from ShippingMarkType st with (nolock) where st.Ukey = combD.ShippingMarkTypeUkey)


drop table #tmpPacking, #tmp_Combination
";
                    DataTable dtTempPMSFile;
                    result = DBProxy.Current.Select("Production", sqlCreateTemp, out dtTempPMSFile);
                    if (!result)
                    {
                        returnMessage = this.UpdateDownloadStickerQueueErr(runPackingID, result);
                        this.backgroundDownloadSticker.ReportProgress(0, returnMessage);
                        return;
                    }

                    DataTable dtEmpty;
                    string sqlCreatePMSFileTemp = @"
alter table #tmpShippingMarkPic_DetailPMSFile alter column SCICtnNo varchar(16)
alter table #tmpShippingMarkPic_DetailPMSFile alter column ShippingMarkTypeUkey bigint
alter table #tmpShippingMarkPic_DetailPMSFile add [Image] varbinary(max) null
";

                    // 將tmpShippingMarkPic_Detail建立在pmsfile中，之後產生圖片在update進去
                    result = MyUtility.Tool.ProcessWithDatatable(dtTempPMSFile, "SCICtnNo,ShippingMarkTypeUkey", sqlCreatePMSFileTemp, out dtEmpty, temptablename: "#tmpShippingMarkPic_DetailPMSFile", conn: sqlConnectionPMSFile);
                    if (!result)
                    {
                        returnMessage = this.UpdateDownloadStickerQueueErr(runPackingID, result);
                        this.backgroundDownloadSticker.ReportProgress(0, returnMessage);
                        return;
                    }
                    #endregion
                    int progressRate = 0;
                    foreach (DataRow itemDownloadSticker in dtDownloadSticker.Rows)
                    {
                        string mercuryCartonNo = itemDownloadSticker["CustCTN2"].ToString();
                        string mercuryBarcodeCartonNo = itemDownloadSticker["CustCTN"].ToString();
                        string sciCtnNo = itemDownloadSticker["SCICtnNo"].ToString();

                        // call Mercury wbe service產生carton sticker pdf
                        result = WebServiceNikeMercury.StaticService.LabelsGS1128CartonPrintByCartonRange(mercuryCartonNo);
                        if (!result)
                        {
                            returnMessage = this.UpdateDownloadStickerQueueErr(runPackingID, result);
                            this.backgroundDownloadSticker.ReportProgress(0, returnMessage);
                            return;
                        }

                        // 等待檔案產生後轉檔成圖片
                        byte[] pdfImage;
                        result = this.CreateTempShippingMarkPic(mercuryBarcodeCartonNo, nikeStickerPrintFileFolder, this.checkCartonBarcode.Checked, out pdfImage);
                        if (!result)
                        {
                            returnMessage = this.UpdateDownloadStickerQueueErr(runPackingID, result);
                            this.backgroundDownloadSticker.ReportProgress(0, returnMessage);
                            return;
                        }

                        // 將圖片update回對應箱子暫存檔中
                        List<SqlParameter> listPar = new List<SqlParameter>() { new SqlParameter("@pdfImage", pdfImage) };
                        result = DBProxy.Current.ExecuteByConn(sqlConnectionPMSFile, $" update #tmpShippingMarkPic_DetailPMSFile set Image = @pdfImage where SCICtnNo = '{sciCtnNo}'", listPar);
                        if (!result)
                        {
                            returnMessage = this.UpdateDownloadStickerQueueErr(runPackingID, result);
                            this.backgroundDownloadSticker.ReportProgress(0, returnMessage);
                            return;
                        }

                        // 更新進度條
                        progressRate++;
                        this.backgroundDownloadSticker.ReportProgress(progressRate, string.Empty);
                    }

                    // 寫入ShippingMarkPic相關table並傳資料給Sunrise, Gensong
                    string sqlCreateShippingMarkPic = $@"
Declare @ShippingMarkPicUkey bigint

if exists(select 1 from ShippingMarkPic with (nolock) where PackingListID = '{runPackingID}')
begin
    update ShippingMarkPic set EditName = 'SCIMIS', EditDate = GetDate()
    where PackingListID = '{runPackingID}'
end
else
begin
    insert into ShippingMarkPic(PackingListID, AddDate, AddName) values('{runPackingID}', GetDate(), 'SCIMIS')
end

select  @ShippingMarkPicUkey = Ukey from ShippingMarkPic with (nolock) where PackingListID = '{runPackingID}'

delete ShippingMarkPic_Detail where ShippingMarkPicUkey = @ShippingMarkPicUkey

insert into ShippingMarkPic_Detail( ShippingMarkPicUkey,
                                    SCICtnNo,
                                    FileName,
                                    ShippingMarkCombinationUkey,
                                    ShippingMarkTypeUkey,
		                            Side,
                                    Seq,
                                    Is2Side,
                                    IsHorizontal,
                                    IsSSCC,
                                    FromRight,
                                    FromBottom,
                                    Width,
                                    Length,
                                    CtnHeight,
                                    IsOverCtnHt,
                                    NotAutomate)
select  @ShippingMarkPicUkey
        ,SCICtnNo
        ,[FileName] = ''
		,ShippingMarkCombinationUkey
		,ShippingMarkTypeUkey
		,Side
		,Seq
		,Is2Side
		,IsHorizontal
		,IsSSCC
		,FromRight
		,FromBottom
		,Width
		,Length
		,CtnHeight
		,IsOverCtnHt
		,NotAutomate
from #tmpShippingMarkPic_Detail

select [ShippingMarkPicUkey] = @ShippingMarkPicUkey
";

                    string sqlCreateShippingMarkPicPMSFile = $@"
delete ShippingMarkPic_Detail where ShippingMarkPicUkey = @ShippingMarkPicUkey

insert into ShippingMarkPic_Detail(ShippingMarkPicUkey, SCICtnNo, ShippingMarkTypeUkey, Image)
        select  @ShippingMarkPicUkey,
                SCICtnNo,
                ShippingMarkTypeUkey,
                Image
        from    #tmpShippingMarkPic_DetailPMSFile
";
                    using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required,
                                                          new TransactionOptions
                                                          {
                                                              Timeout = TimeSpan.FromMinutes(5),
                                                              IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted
                                                          }))
                    {
                        DataTable dtShippingMarkPicUkey;
                        result = MyUtility.Tool.ProcessWithDatatable(dtTempPMSFile, null, sqlCreateShippingMarkPic, out dtShippingMarkPicUkey, temptablename: "#tmpShippingMarkPic_Detail");
                        if (!result)
                        {
                            transaction.Dispose();
                            returnMessage = this.UpdateDownloadStickerQueueErr(runPackingID, result);
                            this.backgroundDownloadSticker.ReportProgress(0, returnMessage);
                            return;
                        }

                        result = DBProxy.Current.Execute("Production", $"delete DownloadStickerQueue where PackingID = '{runPackingID}'");
                        if (!result)
                        {
                            transaction.Dispose();
                            returnMessage = this.UpdateDownloadStickerQueueErr(runPackingID, result);
                            this.backgroundDownloadSticker.ReportProgress(0, returnMessage);
                            return;
                        }

                        long shippingMarkPicUkey = MyUtility.Convert.GetLong(dtShippingMarkPicUkey.Rows[0]["ShippingMarkPicUkey"]);
                        List<SqlParameter> listParPMSFile = new List<SqlParameter>() { new SqlParameter("@ShippingMarkPicUkey", shippingMarkPicUkey) };
                        result = DBProxy.Current.ExecuteByConn(sqlConnectionPMSFile, sqlCreateShippingMarkPicPMSFile, listParPMSFile);
                        if (!result)
                        {
                            transaction.Dispose();
                            returnMessage = this.UpdateDownloadStickerQueueErr(runPackingID, result);
                            this.backgroundDownloadSticker.ReportProgress(0, returnMessage);
                            return;
                        }

                        transaction.Complete();
                    }
                }

                #region 資料交換 - Sunrise
                Task.Run(() => new Sunrise_FinishingProcesses().SentPackingToFinishingProcesses(runPackingID, string.Empty))
                        .ContinueWith(UtilityAutomation.AutomationExceptionHandler, System.Threading.CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, this.mainTaskScheduler);
                #endregion

                #region 資料交換 - Gensong
                Task.Run(() => new Gensong_FinishingProcesses().SentPackingListToFinishingProcesses(runPackingID, string.Empty))
                .ContinueWith(UtilityAutomation.AutomationExceptionHandler, System.Threading.CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, this.mainTaskScheduler);
                #endregion

                this.backgroundDownloadSticker.ReportProgress(0, string.Empty);
            }
            catch (Exception ex)
            {
                this.backgroundDownloadSticker.ReportProgress(0, ex.ToString());
                throw;
            }
        }

        public DualResult CreateTempShippingMarkPic(string mercuryBarcodeCartonNo, string nikeStickerPrintFileFolder, bool checkCartonBarcode, out byte[] pdfImage)
        {
            pdfImage = null;
            string pdfFullFileName = string.Empty;
            // 等待pdf產生timeout以秒為單位
            int waitTimeoutForPDF = 300;
            int runningTime = 0;
            while (runningTime < waitTimeoutForPDF)
            {
                string[] pdfFiles = Directory.GetFiles(nikeStickerPrintFileFolder, "*.pdf");

                if (pdfFiles.Length > 0)
                {
                    pdfFullFileName = pdfFiles[0];
                    break;
                }

                Thread.Sleep(1000);
                runningTime++;
            }

            if (MyUtility.Check.Empty(pdfFullFileName))
            {
                return new DualResult(false, $"Mercury sticker pdf create timeout({waitTimeoutForPDF} second)");
            }

            DualResult result = this.CheckFileLock(pdfFullFileName);
            if (!result)
            {
                return result;
            }

            // 檢查pdf carton barcode是否相符
            if (checkCartonBarcode)
            {
                // BarcodeReader
                Reader reader = new Reader(this.BarcodeReader_RegistrationName, this.BarcodeReader_RegistrationKey);
                reader.BarcodeTypesToFind.Code128 = true;

                reader.ReadFrom(pdfFullFileName);

                foreach (FoundBarcode barcode in reader.FoundBarcodes)
                {
                    string strBarcode = barcode.Value.Replace("<FNC1>", string.Empty);

                    if (strBarcode.Length < 20)
                    {
                        continue;
                    }

                    if (strBarcode != mercuryBarcodeCartonNo)
                    {
                        return new DualResult(false, "Mercury sticker pdf barcode did not match PMS");
                    }
                }
            }

            // 將pdf轉成圖片
            try
            {
                // XsPDF套件
                PdfImageConverter pdfConverter = new PdfImageConverter(pdfFullFileName);
                pdfConverter.DPI = 300;
                pdfConverter.GrayscaleOutput = false;

                // 直接取用Image轉成byte[]，不需要再轉Bitmap
                Image bmp = pdfConverter.PageToImage(0);

                // 準備要寫入DB的資料
                using (var stream = new MemoryStream())
                {
                    bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                    pdfImage = stream.ToArray();
                }

                bmp.Dispose();
                pdfConverter.Dispose();

                result = this.CheckFileLock(pdfFullFileName);
                if (!result)
                {
                    return result;
                }

                // 刪除PDF檔案
                File.Delete(pdfFullFileName);
            }
            catch (Exception ex)
            {
                return new DualResult(false, ex);
            }

            return new DualResult(true);
        }

        private DualResult CheckFileLock(string fullFilePath, int checkTimeout = 30)
        {
            int timeout = checkTimeout < 1 ? 2 : checkTimeout;
            // 確認Mercury是否已經沒有咬住pdf(timeout 30秒)
            for (int i = 0; i < timeout; i++)
            {
                try
                {
                    using (FileStream fs = new FileStream(fullFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        // 如果上面的程式碼成功運行，表示檔案沒有被其他程序占用
                        fs.Dispose();
                        break;
                    }
                }
                catch (IOException iex)
                {
                    // 如果這裡捕捉到 IOException，表示檔案正在被其他程序占用
                    if (i == (timeout - 1))
                    {
                        return new DualResult(false, iex);
                    }
                    Thread.Sleep(1000);
                }
                catch (Exception ex)
                {
                    return new DualResult(false, ex);
                }
            }
            return new DualResult(true);
        }

        private DualResult ClearNikeStickerPrintFileFolder(string nikeStickerPrintFileFolder)
        {
            try
            {
                string[] pdfFiles = Directory.GetFiles(nikeStickerPrintFileFolder, "*.pdf");

                foreach (string pdfFile in pdfFiles)
                {
                    // 刪除PDF檔案
                    File.Delete(pdfFile);
                }

                return new DualResult(true);
            }
            catch (Exception ex)
            {
                return new DualResult(false, ex);
            }

        }

        private string UpdateDownloadStickerQueueErr(string packID, DualResult errResult)
        {
            string errMsg = errResult.GetException() != null ? errResult.GetException().ToString() : errResult.Description;
            string sqlUpdate = $@" update DownloadStickerQueue set Processing = 0, UpdateDate = GetDate(), ErrorMsg = LEFT(@ErrorMsg, 1000) where PackingID = '{packID}'";
            List<SqlParameter> listPar = new List<SqlParameter>() { new SqlParameter("@ErrorMsg", errMsg) };
            DualResult result = DBProxy.Current.Execute("Production", sqlUpdate, listPar);
            return result ? string.Empty : result.GetException().ToString();
        }

        private void backgroundDownloadSticker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.progressBarProcessing.Value = e.ProgressPercentage;

            if (!MyUtility.Check.Empty(e.UserState))
            {
                this.editErrorMsg.Text = $"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")}" + Environment.NewLine + e.UserState.ToString();
            }

            if (e.ProgressPercentage == 0)
            {
                this.Query();
            }
        }

        private void DoDownloadStickerQueue()
        {
            if (!this.backgroundDownloadSticker.IsBusy)
            {
                this.Query();

                if (this.dtDownloadStickerQueue == null || this.dtDownloadStickerQueue.Rows.Count == 0)
                {
                    return;
                }

                this.backgroundDownloadSticker.RunWorkerAsync();
            }
        }

        private void timerRefresh_Tick(object sender, EventArgs e)
        {
            this.DoDownloadStickerQueue();
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            this.DoDownloadStickerQueue();
        }
    }
}
