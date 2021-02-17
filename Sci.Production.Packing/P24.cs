using Ict.Win;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using Sci.Win.Tems;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Data.SqlClient;
using Sci.Win.Tools;
using System.Transactions;
using Sci.Production.Automation;

namespace Sci.Production.Packing
{
    /// <inheritdoc/>
    public partial class P24 : Sci.Win.Tems.Input6
    {
        private string destination_path; // 放的路徑
        private string oldPackingListID = string.Empty;
        private string oldStickerComb = string.Empty;
        private string oldStickerType = string.Empty;
        private List<ShippingMarkPic_Detail> readToSave;
        private List<ShippingMarkPic_Detail> readToDelete;

        /// <inheritdoc/>
        public P24(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.destination_path = MyUtility.GetValue.Lookup("select ShippingMarkPath from System WITH (NOLOCK) ", null);
            this.gridicon.Visible = false;
        }

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : MyUtility.Convert.GetString(e.Master["Ukey"]);
            string packingListID = (e.Master == null) ? string.Empty : MyUtility.Convert.GetString(e.Master["PackingListID"]);
            this.DetailSelectCommand = $@"
WITH PackingListDetail as(
	SELECT DISTINCT pd.ID
		,pd.OrderShipmodeSeq
		,pd.OrderID
		,o.CustPONO
		,pd.CTNStartNo
		,pd.CustCtn
		,pd.RefNo
		,pd.SCICtnNo
        ,pd.Seq
	FROM PackingList_Detail pd
	INNER JOIN ShippingMarkPic a  ON pd.ID = a.PackingListID
	INNER JOIN ShippingMarkPic_Detail b ON a.Ukey = b.ShippingMarkPicUkey  AND b.SCICtnNo = pd.SCICtnNo
	INNER JOIN Orders o ON o.ID = pd.OrderID
	WHERE pd.ID='{packingListID}'
)
SELECT pd.OrderID
    ,o.CustPONO
    ,pd.CTNStartNo
    ,pd.CustCtn
    ,pd.RefNo
    ,[ColorWay]=ColorWay.Val
    ,[Color]=Color.Val
    ,[SizeCode]=SizeCode.Val
    ,[ShippingMarkCombination]=comb.ID
    ,[ShippingMarkType]=st.ID
    , b.ShippingMarkPicUkey
    , b.SCICtnNo
    , b.FileName
    , [HTMLFile] = IIF(b.FileName = '' ,0 , 1)
    , [ShippingMark]=IIF(b.Image IS NULL , 0 , 1 )
    , b.ShippingMarkCombinationUkey
    , b.ShippingMarkTypeUkey
    , b.Side
    , b.Seq
    , b.Is2Side
    , b.IsHorizontal
    , b.IsSSCC
    , b.FromRight
    , b.FromBottom
    , b.Width
    , b.Length
    ,local_file_type=''
    ,FileSourcePath=''
    ,FileAction=''
FROm ShippingMarkPic a
INNER JOIN ShippingMarkPic_Detail b ON a.Ukey = b.ShippingMarkPicUkey
INNER JOIN PackingListDetail pd ON pd.ID = a.PackingListID AND b.SCICtnNo = pd.SCICtnNo
INNER JOIN Orders o ON o.ID = pd.OrderID
INNER JOIN ShippingMarkCombination comb ON comb.Ukey = b.ShippingMarkCombinationUkey
INNER JOIN ShippingMarkType st ON st.Ukey = b.ShippingMarkTypeUkey
OUTER APPLY(
	SELECT [Val]=STUFF((
		SELECT DISTINCT ',' + pd2.Article
		FROM PackingList_Detail pd2
		WHERE pd2.ID = pd.ID AND pd2.OrderID = pd.OrderID AND pd2.OrderShipmodeSeq = pd.OrderShipmodeSeq AND pd2.CTNStartNo = pd.CTNStartNo
		FOR XML PATH('')
	),1,1,'')
)ColorWay
OUTER APPLY(
	SELECT [Val]=STUFF((
		SELECT DISTINCT ',' + pd2.Color
		FROM PackingList_Detail pd2
		WHERE pd2.ID = pd.ID AND pd2.OrderID = pd.OrderID AND pd2.OrderShipmodeSeq = pd.OrderShipmodeSeq AND pd2.CTNStartNo = pd.CTNStartNo
		FOR XML PATH('')
	),1,1,'')
)Color
OUTER APPLY(
	SELECT [Val]=STUFF((
		SELECT DISTINCT ',' + pd2.SizeCode
		FROM PackingList_Detail pd2
		WHERE pd2.ID = pd.ID AND pd2.OrderID = pd.OrderID AND pd2.OrderShipmodeSeq = pd.OrderShipmodeSeq AND pd2.CTNStartNo = pd.CTNStartNo
		FOR XML PATH('')
	),1,1,'')
)SizeCode
WHERE a.Ukey = '{masterID}'
ORDER BY pd.Seq ASC
";
            return base.OnDetailSelectCommandPrepare(e);
        }

        /// <inheritdoc/>
        protected override bool OnGridSetup()
        {
            this.detailgrid.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("OrderID", header: "SP No.", width: Widths.AnsiChars(16), iseditingreadonly: true)
            .Text("CustPONO", header: "P.O. No.", width: Widths.AnsiChars(16), iseditingreadonly: true)
            .Text("CTNStartNo", header: "CTN#", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("SCICtnNo", header: "SCI Ctn No.", width: Widths.AnsiChars(16), iseditingreadonly: true)
            .Text("CustCTN", header: "Cust #", width: Widths.AnsiChars(25), iseditingreadonly: true)
            .Text("RefNo", header: "Ref No.", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("ColorWay", header: "ColorWay", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("Color", header: "Color", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("SizeCode", header: "Size", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("ShippingMarkCombination", header: "Sticker Combination", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("ShippingMarkType", header: "Sticker Type", width: Widths.AnsiChars(20), iseditingreadonly: true)
            .CheckBox("IsSSCC", header: "Is SSCC", width: Widths.AnsiChars(3), iseditable: false, trueValue: 1, falseValue: 0)
            .Text("Side", header: "Side", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Text("Seq", header: "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .CheckBox("Is2Side", header: "Is 2 Side", width: Widths.AnsiChars(3), iseditable: false, trueValue: 1, falseValue: 0)
            .CheckBox("IsHorizontal", header: "Is Horizontal", width: Widths.AnsiChars(3), iseditable: false, trueValue: 1, falseValue: 0)
            .Numeric("FromRight", header: "From Right (mm)", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Numeric("FromBottom", header: "From Bottom (mm)", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Numeric("Width", header: "Width", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Numeric("Length", header: "Length", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .CheckBox("ShippingMark", header: "Shipping Mark", width: Widths.AnsiChars(3), iseditable: false, trueValue: 1, falseValue: 0)
            .CheckBox("HTMLFile", header: "HTML File", width: Widths.AnsiChars(3), iseditable: false, trueValue: 1, falseValue: 0)
            .Button("Upload", null, header: string.Empty, width: Widths.AnsiChars(5), onclick: this.BtnUpload)
            .Button("Delete", null, header: string.Empty, width: Widths.AnsiChars(5), onclick: this.BtnDelete)
            ;
            this.detailgrid.CellPainting += this.Detailgrid_CellPainting;
            #region 關閉排序功能
            for (int i = 0; i < this.detailgrid.ColumnCount; i++)
            {
                this.detailgrid.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            #endregion
            return base.OnGridSetup();
        }

        private void Detailgrid_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            // if (e.RowIndex < 0 || e.ColumnIndex < 8)
            // {
            //    return;
            // }

            // if (e.RowIndex > 0)
            // {
            //    e.AdvancedBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
            // }

            // if (this.IsTheSameCellValue(e.RowIndex))
            // {
            //    e.AdvancedBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.None;
            // }
            // else
            // {
            //    e.AdvancedBorderStyle.Bottom = this.detailgrid.AdvancedCellBorderStyle.Bottom;
            // }
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.ChangCell();
            this.SetComboSeqAndSide();
            this.mtbs.ResetBindings(false);

            if (this.EditMode)
            {
                this.btnDownload.Enabled = false;
                this.btnImport.Enabled = false;
                this.readToSave = new List<ShippingMarkPic_Detail>();
                this.readToDelete = new List<ShippingMarkPic_Detail>();
            }
            else
            {
                this.btnDownload.Enabled = true;
                this.btnImport.Enabled = true;
            }

            this.oldPackingListID = MyUtility.Convert.GetString(this.CurrentMaintain["PackingListID"]);
        }

        private void ChangCell()
        {
            // foreach (DataGridViewRow dr in this.detailgrid.Rows)
            // {
            //    if (this.IsTheSameCellValueBefore(dr.Index))
            //    {
            //        //dr.Cells[8] = new DataGridViewTextBoxCell();
            //        dr.Cells[9] = new DataGridViewTextBoxCell();
            //        dr.Cells[10] = new DataGridViewTextBoxCell();
            //        dr.Cells[11] = new DataGridViewTextBoxCell();

            // //dr.Cells[8].Style.ForeColor = Color.White;
            //        dr.Cells[9].Style.ForeColor = Color.White;
            //        dr.Cells[10].Style.ForeColor = Color.White;
            //        dr.Cells[11].Style.ForeColor = Color.White;

            // //dr.Cells[8].Style.SelectionForeColor = Color.White;
            //        dr.Cells[9].Style.SelectionForeColor = Color.White;
            //        dr.Cells[10].Style.SelectionForeColor = Color.White;
            //        dr.Cells[11].Style.SelectionForeColor = Color.White;

            // //dr.Cells[8].Style.SelectionBackColor = Color.White;
            //        dr.Cells[9].Style.SelectionBackColor = Color.White;
            //        dr.Cells[10].Style.SelectionBackColor = Color.White;
            //        dr.Cells[11].Style.SelectionBackColor = Color.White;

            // //dr.Cells[8].ReadOnly = true;
            //        //dr.Cells[8].ReadOnly = true;
            //        dr.Cells[9].ReadOnly = true;
            //        dr.Cells[10].ReadOnly = true;
            //        dr.Cells[11].ReadOnly = true;
            //    }
            // }
        }

        private bool IsTheSameCellValue(int row)
        {
            if (row == this.detailgrid.Rows.Count - 1)
            {
                return false;
            }

            DataGridViewCell cell1 = this.detailgrid["SCICtnNo", row];
            DataGridViewCell cell2 = this.detailgrid["SCICtnNo", row + 1];
            if (cell1.Value == null || cell2.Value == null)
            {
                return false;
            }

            return cell1.Value.ToString() == cell2.Value.ToString();
        }

        private bool IsTheSameCellValueBefore(int row)
        {
            if (row > this.detailgrid.Rows.Count || row == 0)
            {
                return false;
            }

            DataGridViewCell cell1 = this.detailgrid["SCICtnNo", row - 1];
            DataGridViewCell cell2 = this.detailgrid["SCICtnNo", row];
            if (cell1.Value == null || cell2.Value == null)
            {
                return false;
            }

            return cell1.Value.ToString() == cell2.Value.ToString();
        }

        private void BtnUpload(object sender, EventArgs e)
        {
            if (!this.EditMode)
            {
                return;
            }

            // 呼叫File 選擇視窗
            OpenFileDialog file = new OpenFileDialog
            {
                InitialDirectory = "c:\\", // 預設路徑
                Filter = "Image Files(*.BMP;)|*.BMP", // 使用檔名
                FilterIndex = 1,
                RestoreDirectory = true,
            };
            if (file.ShowDialog() == DialogResult.OK)
            {
                string local_path_file = file.FileName;

                byte[] image = this.ConvertFileToByteArray(local_path_file);

                long shippingMarkPicUkey = MyUtility.Convert.GetLong(this.CurrentDetailData["ShippingMarkPicUkey"]);
                string sCICtnNo = MyUtility.Convert.GetString(this.CurrentDetailData["SCICtnNo"]);
                long shippingMarkTypeUkey = MyUtility.Convert.GetLong(this.CurrentDetailData["ShippingMarkTypeUkey"]);
                string fileName = file.SafeFileName.Split('.')[0];

                ShippingMarkPic_Detail body = new ShippingMarkPic_Detail()
                {
                    SCICtnNo = sCICtnNo,
                    ShippingMarkPicUkey = shippingMarkPicUkey,
                    ShippingMarkTypeUkey = shippingMarkTypeUkey,
                    Image = image,
                    FileName = fileName,
                };

                // 存入集合，若按下Save才寫入DB
                this.readToSave.Add(body);
                this.CurrentDetailData["ShippingMark"] = true;
            }
        }

        private void BtnDelete(object sender, EventArgs e)
        {
            if ((!this.EditMode && !MyUtility.Convert.GetBool(this.CurrentDetailData["ShippingMark"])) || MyUtility.Convert.GetLong(this.CurrentDetailData["ShippingMarkPicUkey"]) == 0)
            {
                // MyUtility.Convert.GetLong(this.CurrentDetailData["ShippingMarkPicUkey"]) = 0，表示為新增
                if (this.readToSave.Any(x => x.SCICtnNo == MyUtility.Convert.GetString(this.CurrentDetailData["SCICtnNo"])
                                            && x.ShippingMarkTypeUkey == MyUtility.Convert.GetLong(this.CurrentDetailData["ShippingMarkTypeUkey"])
                                            && x.ShippingMarkPicUkey == 0))
                {
                    this.readToSave.RemoveAll(x => x.SCICtnNo == MyUtility.Convert.GetString(this.CurrentDetailData["SCICtnNo"])
                                            && x.ShippingMarkTypeUkey == MyUtility.Convert.GetLong(this.CurrentDetailData["ShippingMarkTypeUkey"])
                                            && x.ShippingMarkPicUkey == 0);
                    this.CurrentDetailData["ShippingMark"] = false;
                }

                return;
            }

            DialogResult diaR = MyUtility.Msg.QuestionBox("Sure to delete ?");

            if (diaR == DialogResult.No)
            {
                return;
            }

            long shippingMarkPicUkey = MyUtility.Convert.GetLong(this.CurrentDetailData["ShippingMarkPicUkey"]);
            string sCICtnNo = MyUtility.Convert.GetString(this.CurrentDetailData["SCICtnNo"]);
            long shippingMarkTypeUkey = MyUtility.Convert.GetLong(this.CurrentDetailData["ShippingMarkTypeUkey"]);

            ShippingMarkPic_Detail body = new ShippingMarkPic_Detail()
            {
                SCICtnNo = sCICtnNo,
                ShippingMarkPicUkey = shippingMarkPicUkey,
                ShippingMarkTypeUkey = shippingMarkTypeUkey,
            };

            // 存入集合，若按下Save才寫入DB
            this.readToDelete.Add(body);

            this.CurrentDetailData["ShippingMark"] = false;
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["EditName"] = string.Empty;
        }

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtPackingListID.ReadOnly = true;
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            if (this.DetailDatas.Count() == 0)
            {
                MyUtility.Msg.WarningBox("Detail can not be empty!");
                return false;
            }

            if (this.DetailDatas.Where(o => MyUtility.Check.Empty(o["Side"])).Any())
            {
                string brandID = MyUtility.GetValue.Lookup($@"SELECT BrandID FROM PackingList WHERE ID = '{this.CurrentMaintain["PackingListID"]}' ");
                var cartonList = this.DetailDatas.Where(o => MyUtility.Check.Empty(o["Side"])).Select(o => o["Refno"].ToString()).Distinct().ToList();

                MyUtility.Msg.WarningBox($"Barnd {brandID} Carton {cartonList.JoinToString(",")} not yet finish Shipping Mark Setting in Packing B03.");
                return false;
            }

            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override DualResult ClickSavePost()
        {/*
            List<string> updateCmds = new List<string>();
            List<string> deleteCmds = new List<string>();
            List<SqlParameter> paras = new List<SqlParameter>();
            List<SqlParameter> paras_D = new List<SqlParameter>();

            int idx = 0;
            foreach (ShippingMarkPic_Detail body in this.readToSave)
            {
                paras.Add(new SqlParameter($"@Image{idx}", body.Image));
                paras.Add(new SqlParameter($"@FileName{idx}", body.FileName));

                string updateCmd = $@"
UPDATE ShippingMarkPic_Detail WITH(NOLOCK)
SET Image = @Image{idx} , FileName = @FileName{idx}
WHERE SCICtnNo='{body.SCICtnNo}' AND ShippingMarkPicUkey='{body.ShippingMarkPicUkey}' AND ShippingMarkTypeUkey='{body.ShippingMarkTypeUkey}'
";
                updateCmds.Add(updateCmd);

                idx++;
            }

            foreach (ShippingMarkPic_Detail body in this.readToDelete)
            {
                string deleteCmd = $@"
UPDATE ShippingMarkPic_Detail WITH(NOLOCK)
SET Image = NULL , FileName = ''
WHERE SCICtnNo='{body.SCICtnNo}' AND ShippingMarkPicUkey='{body.ShippingMarkPicUkey}' AND ShippingMarkTypeUkey='{body.ShippingMarkTypeUkey}'
";
                deleteCmds.Add(deleteCmd);

                idx++;
            }

            using (TransactionScope transactionscope = new TransactionScope())
            {
                try
                {
                    DualResult r;

                    if (updateCmds.Count > 0)
                    {
                        r = DBProxy.Current.Execute(null, updateCmds.JoinToString(Environment.NewLine), paras);

                        if (!r)
                        {
                            transactionscope.Dispose();
                            return r;
                        }
                    }

                    if (deleteCmds.Count > 0)
                    {
                        r = DBProxy.Current.Execute(null, deleteCmds.JoinToString(Environment.NewLine));

                        if (!r)
                        {
                            transactionscope.Dispose();
                            return r;
                        }
                    }

                    transactionscope.Complete();
                }
                catch (Exception ex)
                {
                    transactionscope.Dispose();
                    this.ShowErr("Commit transaction error.", ex);
                    return new DualResult(false);
                }
            }
            */
            return base.ClickSavePost();
        }

        /// <inheritdoc/>
        protected override void ClickSaveAfter()
        {
            base.ClickSaveAfter();

            List<string> updateCmds = new List<string>();
            List<string> deleteCmds = new List<string>();
            List<SqlParameter> paras = new List<SqlParameter>();
            List<SqlParameter> paras_D = new List<SqlParameter>();

            int idx = 0;
            foreach (ShippingMarkPic_Detail body in this.readToSave)
            {
                paras.Add(new SqlParameter($"@Image{idx}", body.Image));
                paras.Add(new SqlParameter($"@FileName{idx}", body.FileName));

                string updateCmd = $@"
UPDATE ShippingMarkPic_Detail
SET Image = @Image{idx} , FileName = @FileName{idx}
WHERE SCICtnNo='{body.SCICtnNo}' /*AND ShippingMarkPicUkey='{body.ShippingMarkPicUkey}'*/ AND ShippingMarkTypeUkey='{body.ShippingMarkTypeUkey}'
";
                updateCmds.Add(updateCmd);

                idx++;
            }

            foreach (ShippingMarkPic_Detail body in this.readToDelete)
            {
                string deleteCmd = $@"
UPDATE ShippingMarkPic_Detail
SET Image = NULL , FileName = ''
WHERE SCICtnNo='{body.SCICtnNo}' /*AND ShippingMarkPicUkey='{body.ShippingMarkPicUkey}'*/ AND ShippingMarkTypeUkey='{body.ShippingMarkTypeUkey}'
";
                deleteCmds.Add(deleteCmd);

                idx++;
            }

            using (TransactionScope transactionscope = new TransactionScope())
            {
                try
                {
                    DualResult r;

                    if (updateCmds.Count > 0)
                    {
                        r = DBProxy.Current.Execute(null, updateCmds.JoinToString(Environment.NewLine), paras);

                        if (!r)
                        {
                            transactionscope.Dispose();
                            this.ShowErr(r);
                        }
                    }

                    if (deleteCmds.Count > 0)
                    {
                        r = DBProxy.Current.Execute(null, deleteCmds.JoinToString(Environment.NewLine));

                        if (!r)
                        {
                            transactionscope.Dispose();
                            this.ShowErr(r);
                        }
                    }

                    transactionscope.Complete();
                }
                catch (Exception ex)
                {
                    transactionscope.Dispose();
                    this.ShowErr("Commit transaction error.", ex);

                    // return new DualResult(false);
                }
            }

            #region ISP20201607 資料交換 - Gensong
            if (Gensong_FinishingProcesses.IsGensong_FinishingProcessesEnable)
            {
                // 不透過Call API的方式，自己組合，傳送API
                Task.Run(() => new Gensong_FinishingProcesses().SentPackingListToFinishingProcesses(this.CurrentMaintain["PackingListID"].ToString(), string.Empty))
                    .ContinueWith(UtilityAutomation.AutomationExceptionHandler, System.Threading.CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());
            }
            #endregion

            this.RenewData();
            this.ChangCell();

            // 資料交換 - Sunrise
            Task.Run(() => new Sunrise_FinishingProcesses().SentPackingToFinishingProcesses(this.CurrentMaintain["PackingListID"].ToString(), string.Empty))
                .ContinueWith(UtilityAutomation.AutomationExceptionHandler, System.Threading.CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());
        }

        /// <inheritdoc/>
        protected override DualResult ClickDelete()
        {
            string sqldelete = $@"
delete ShippingMarkPic where ukey = {this.CurrentMaintain["Ukey"]}
delete ShippingMarkPic_Detail where ShippingMarkPicUkey = {this.CurrentMaintain["Ukey"]}
";

            foreach (DataRow dr in this.DetailDatas)
            {
                string destination_fileName = MyUtility.Convert.GetString(dr["FileName"]);
                try
                {
                    string destination = Path.Combine(this.destination_path, destination_fileName);
                    if (System.IO.File.Exists(destination))
                    {
                        if (System.IO.File.Exists(destination))
                        {
                            System.IO.File.Delete(destination);
                        }
                    }
                }
                catch (System.IO.IOException exception)
                {
                    MyUtility.Msg.ErrorBox("Error: update file fail. Original error: " + exception.Message);
                }
            }

            return DBProxy.Current.Execute(null, sqldelete);
        }

        /// <inheritdoc/>
        protected override void ClickDeleteAfter()
        {
            #region ISP20201607 資料交換 - Gensong
            if (Gensong_FinishingProcesses.IsGensong_FinishingProcessesEnable)
            {
                // 不透過Call API的方式，自己組合，傳送API
                Task.Run(() => new Gensong_FinishingProcesses().SentPackingListToFinishingProcesses(this.CurrentMaintain["PackingListID"].ToString(), string.Empty))
                    .ContinueWith(UtilityAutomation.AutomationExceptionHandler, System.Threading.CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());
            }
            #endregion
            base.ClickDeleteAfter();
            #region ISP20200757 資料交換 - Sunrise
            Task.Run(() => new Sunrise_FinishingProcesses().SentPackingToFinishingProcesses(this.CurrentMaintain["PackingListID"].ToString(), string.Empty))
                .ContinueWith(UtilityAutomation.AutomationExceptionHandler, System.Threading.CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());
            #endregion
            this.ReloadDatas();
        }

        private void TxtPackingListID_Validating(object sender, CancelEventArgs e)
        {
            string newPackingListID = this.txtPackingListID.Text;

            if (this.oldPackingListID != newPackingListID)
            {
                // 一旦清空 / 重選，移除『表頭、表身』所有的資料
                this.ClearDetailtBody();

                this.oldPackingListID = newPackingListID;

                if (MyUtility.Check.Empty(newPackingListID))
                {
                    this.CurrentMaintain["PackingListID"] = DBNull.Value;
                    return;
                }

                this.CheckShippingMarkCombination(newPackingListID);

                // 先區分出是混尺碼/非混尺碼箱號
                List<string> notMixed_CTNStartNo = this.GetCTNStartNo_IsMixed(false, newPackingListID);
                List<string> mixed_CTNStartNo = this.GetCTNStartNo_IsMixed(true, newPackingListID);

                DataTable custCD_Default = this.GetCustCD_Default(newPackingListID);

                // 找出缺少的B03設定
                string cmd = $@"
-----Step 2. 找出箱子種類、並串上第一步驟的ShippingMarkCombination 資料
SELECT  p.BrandID ,[ShippingMarkCombination]=comb.ID ,[ShippingMarkType]=st.ID ,pd.RefNo 
FROM PackingList p
INNER JOIN PackingList_Detail pd ON p.id = pd.ID
INNER JOIN ShippingMarkCombination comb ON comb.BrandID = p.BrandID
INNER JOIN ShippingMarkCombination_Detail combD ON comb.Ukey=combD.ShippingMarkCombinationUkey
INNER JOIN ShippingMarkType st ON st.Ukey = combD.ShippingMarkTypeUkey
WHERE p.ID = @ID AND pd.CTNStartNo IN ('{notMixed_CTNStartNo.JoinToString("','")}')   -- IN 非混尺碼箱號
AND comb.Ukey IN (
    -----Step 1. 先根據找出Cust CD預設的 ShippingMarkCombination (混尺碼分開做)
	SELECT DISTINCT 
	[Sticker Combination]= ISNULL(s.Ukey,(
		SELECT Ukey 
		FROM ShippingMarkCombination
		WHERE BrandID = p.BrandID AND Category='PIC'  AND IsDefault = 1 AND IsMixPack=0
		))
	--, [Is Mix Pack] = 'N' 
	FROM PackingList p
	INNER JOIN PackingList_Detail pd ON p.id = pd.ID
	INNER JOIN CustCD c ON c.BrandID = p.BrandID AND c.ID = p.CustCDID
	LEFT JOIN ShippingMarkCombination s ON s.Ukey = c.StickerCombinationUkey
	where p.ID = @ID
)
-----Step 3. 看看Packing B03 (ShippingMarkPicture)，有沒有缺少步驟二的資料，有的話撈出來
AND NOT EXISTS(
	SELECT 1
	FROM ShippingMarkPicture a
	INNER JOIN ShippingMarkPicture_Detail b ON a.Ukey = b.ShippingMarkPictureUkey
	WHERE Category='PIC'
	AND a.BrandID = p.BrandID 
	AND a.ShippingMarkCombinationUkey=comb.Ukey 
	AND a.CTNRefno =  pd.RefNo 
	AND b.ShippingMarkTypeUkey =  combD.ShippingMarkTypeUkey
)
UNION ALL--------------------------------------------------------------------------------------------
SELECT  p.BrandID ,[ShippingMarkCombination]=comb.ID ,[ShippingMarkType]=st.ID ,pd.RefNo 
FROM PackingList p
INNER JOIN PackingList_Detail pd ON p.id = pd.ID
INNER JOIN ShippingMarkCombination comb ON comb.BrandID = p.BrandID
INNER JOIN ShippingMarkCombination_Detail combD ON comb.Ukey=combD.ShippingMarkCombinationUkey
INNER JOIN ShippingMarkType st ON st.Ukey = combD.ShippingMarkTypeUkey
WHERE p.ID = @ID AND pd.CTNStartNo IN ('{mixed_CTNStartNo.JoinToString("','")}')   -- IN 混尺碼箱號
AND comb.Ukey IN (
	SELECT DISTINCT
	[StickerCombinationUkey]= ISNULL(s.Ukey,(
		SELECT Ukey 
		FROM ShippingMarkCombination
		WHERE BrandID = p.BrandID AND Category='PIC'  AND IsDefault = 1 AND IsMixPack=1
		))
	--, [Is Mix Pack] = 'Y' 
	FROM PackingList p
	INNER JOIN PackingList_Detail pd ON p.id = pd.ID
	INNER JOIN CustCD c ON c.BrandID = p.BrandID AND c.ID = p.CustCDID
	LEFT JOIN ShippingMarkCombination s ON s.Ukey = c.StickerCombinationUkey_MixPack
	where p.ID = @ID
)
AND NOT EXISTS(
	SELECT 1
	FROM ShippingMarkPicture a
	INNER JOIN ShippingMarkPicture_Detail b ON a.Ukey = b.ShippingMarkPictureUkey
	WHERE Category='PIC'
	AND a.BrandID = p.BrandID 
	AND a.ShippingMarkCombinationUkey=comb.Ukey 
	AND a.CTNRefno =  pd.RefNo 
	AND b.ShippingMarkTypeUkey =  combD.ShippingMarkTypeUkey
)

";
                DataTable ddt;

                List<SqlParameter> paras = new List<SqlParameter>();
                paras.Add(new SqlParameter("@ID", newPackingListID));
                DualResult result = DBProxy.Current.Select(null, cmd, paras, out ddt);
                if (!result)
                {
                    this.ShowErr(result);
                    return;
                }

                if (ddt.Rows.Count > 0)
                {
                    string msg = "No Set Packing_B03" + Environment.NewLine;
                    var sum = ddt.AsEnumerable().Select(o => MyUtility.Convert.GetString(o["BrandID"])
                            + " - " + MyUtility.Convert.GetString(o["ShippingMarkCombination"])
                            + " - " + MyUtility.Convert.GetString(o["ShippingMarkType"])
                            + " - " + MyUtility.Convert.GetString(o["RefNo"])).Distinct().ToList();
                    msg += sum.JoinToString(Environment.NewLine);

                    this.CurrentMaintain["PackingListID"] = DBNull.Value;
                    this.oldPackingListID = string.Empty;
                    this.ClearDetailtBody();
                    MyUtility.Msg.WarningBox(msg);
                    return;
                }
                else
                {
                    this.GetDetailBody(newPackingListID, notMixed_CTNStartNo, mixed_CTNStartNo);

                    this.CurrentMaintain["PackingListID"] = newPackingListID;
                }
            }
        }

        private void BtnDownload_Click(object sender, EventArgs e)
        {
            if (this.detailgridbs.DataSource == null)
            {
                return;
            }

            if (((DataTable)this.detailgridbs.DataSource).Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found");
                return;
            }

            var detailList = ((DataTable)this.detailgridbs.DataSource).AsEnumerable().Select(s => new
            {
                SCICtnNo = MyUtility.Convert.GetString(s["SCICtnNo"]),
                OrderID = MyUtility.Convert.GetString(s["OrderID"]),
                CustPONO = MyUtility.Convert.GetString(s["CustPONO"]),
                CTNStartNo = MyUtility.Convert.GetString(s["CTNStartNo"]),
                CustCTN = MyUtility.Convert.GetString(s["CustCTN"]),
                ColorWay = MyUtility.Convert.GetString(s["ColorWay"]),
                Color = MyUtility.Convert.GetString(s["Color"]),
                SizeCode = MyUtility.Convert.GetString(s["SizeCode"]),
                ShippingMarkTypeUkey = MyUtility.Convert.GetLong(s["ShippingMarkTypeUkey"]),
                ShippingMarkType = MyUtility.Convert.GetString(s["ShippingMarkType"]),
            }).ToList();

            #region To Excel
            string excelName = "Packing_P24_Download";
            Excel.Application excelApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + $"\\{excelName}.xltx");
            Excel.Worksheet worksheet = excelApp.ActiveWorkbook.Worksheets[1];

            int rownum = 2;
            foreach (var dr in detailList)
            {
                worksheet.Cells[rownum, 1] = dr.OrderID;
                worksheet.Cells[rownum, 2] = dr.CustPONO;
                worksheet.Cells[rownum, 3] = this.CurrentMaintain["PackingListID"];
                worksheet.Cells[rownum, 4] = dr.CTNStartNo;
                worksheet.Cells[rownum, 5] = dr.CustCTN;
                worksheet.Cells[rownum, 6] = dr.SCICtnNo;
                worksheet.Cells[rownum, 7] = dr.ColorWay;
                worksheet.Cells[rownum, 8] = dr.Color;
                worksheet.Cells[rownum, 9] = dr.SizeCode;
                worksheet.Cells[rownum, 10] = dr.ShippingMarkTypeUkey;
                worksheet.Cells[rownum, 11] = dr.ShippingMarkType;
                rownum++;
            }

            worksheet.get_Range((Excel.Range)worksheet.Cells[2, 12], (Excel.Range)worksheet.Cells[rownum - 1, 12]).Interior.Color = Color.FromArgb(255, 199, 206);

            #endregion
            #region 釋放上面開啟過excel物件
            string strExcelName = Class.MicrosoftFile.GetName(excelName);
            Excel.Workbook workbook = excelApp.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            excelApp.Quit();

            if (worksheet != null)
            {
                Marshal.FinalReleaseComObject(worksheet);
            }

            if (excelApp != null)
            {
                Marshal.FinalReleaseComObject(excelApp);
            }
            #endregion
            strExcelName.OpenFile();
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            DataTable dtexcel;
            if (this.EditMode)
            {
                return;
            }

            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                Filter = "Excel files (*.xlsx;*.xls;*.xlt)|*.xlsx;*.xls;*.xlt",
            };

            // 開窗且有選擇檔案
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string msg;

                dtexcel = this.GetExcel(openFileDialog1.FileName, out msg);
                if (!MyUtility.Check.Empty(msg))
                {
                    MyUtility.Msg.ErrorBox(msg);
                    return;
                }

                if (MyUtility.Check.Empty(dtexcel.Columns["Pack ID"]) || MyUtility.Check.Empty(dtexcel.Columns["SCI Ctn No."]) ||
                    MyUtility.Check.Empty(dtexcel.Columns["Shipping Mark Pic File Path"]) || MyUtility.Check.Empty(dtexcel.Columns["Sticker Type"])
                     || MyUtility.Check.Empty(dtexcel.Columns["Sticker Type Ukey"]))
                {
                    MyUtility.Msg.WarningBox("excel file format error !!");
                    return;
                }

                for (int i = dtexcel.Rows.Count - 1; i > 0; i--)
                {
                    if (MyUtility.Check.Empty(dtexcel.Rows[i]["Pack ID"]))
                    {
                        dtexcel.Rows[i].Delete();
                    }
                }

                #region 檢查PackingListID是否與表頭相同
                foreach (DataRow dr in dtexcel.Rows)
                {
                    if (dr.RowState != DataRowState.Deleted &&
                        !MyUtility.Convert.GetString(dr["Pack ID"]).EqualString(MyUtility.Convert.GetString(this.CurrentMaintain["PackingListID"])))
                    {
                        MyUtility.Msg.WarningBox("Pack ID not the same !!");
                        return;
                    }
                }
                #endregion

                var excelist = dtexcel.AsEnumerable().Select(s => new
                {
                    PackingListID = MyUtility.Convert.GetString(s["Pack ID"]),
                    SCICtnNo = MyUtility.Convert.GetString(s["SCI Ctn No."]),
                    ShippingMarkTypeUkey= MyUtility.Convert.GetLong(s["Sticker Type Ukey"]),
                    ShippingMarkType = MyUtility.Convert.GetString(s["Sticker Type"]),
                    FileName = MyUtility.Convert.GetString(s["Shipping Mark Pic File Path"]),
                });

                #region 寫入DB前檢查
                if (!excelist.Where(o => !MyUtility.Check.Empty(o.FileName)).Any())
                {
                    MyUtility.Msg.WarningBox("<Shipping Mark Pic File Path> not exists !!");
                    return;
                }

                foreach (var item in excelist.Where(o => !MyUtility.Check.Empty(o.FileName)))
                {
                    if (!File.Exists(item.FileName))
                    {
                        MyUtility.Msg.WarningBox($"File : {item.FileName} not exists !!");
                        return;
                    }

                    if (Path.GetExtension(item.FileName).ToUpper() != ".BMP")
                    {
                        MyUtility.Msg.WarningBox($"File : {item.FileName} is not .BMP type !!");
                        return;
                    }

                    string cmd = $@"
SELECT 1
FROm ShippingMarkPic a
INNER JOIN ShippingMarkPic_Detail b ON a.Ukey = b.ShippingMarkPicUkey
--INNER JOIN ShippingMarktype s On b.ShippingMarkTypeUkey = s.Ukey
WHERE a.PackingListID='{item.PackingListID}'
AND b.SCICtnNo='{item.SCICtnNo}'
AND b.ShippingMarkTypeUkey='{item.ShippingMarkTypeUkey}'
";

                    if (!MyUtility.Check.Seek(cmd))
                    {
                        MyUtility.Msg.WarningBox($"Pack ID : {item.PackingListID} , SCI Ctn No. : {item.SCICtnNo}, Sticker Type : {item.ShippingMarkType} not exists !!");
                        return;
                    }
                }
                #endregion

                this.ShowWaitMessage("Processing...");

                #region 開始寫入DB
                int idx = 0;
                List<string> updateCmds = new List<string>();
                List<SqlParameter> paras = new List<SqlParameter>();
                foreach (var item in excelist.Where(o => !MyUtility.Check.Empty(o.FileName)))
                {
                    byte[] image = this.ConvertFileToByteArray(item.FileName);

                    string fileName = Path.GetFileNameWithoutExtension(item.FileName);
                    paras.Add(new SqlParameter($"@Image{idx}", image));
                    paras.Add(new SqlParameter($"@FileName{idx}", fileName));

                    string updateCmd = $@"
UPDATE b
SET Image = @Image{idx} , FileName = @FileName{idx}
FROM ShippingMarkPic a
INNER JOIN ShippingMarkPic_Detail b ON a.Ukey = b.ShippingMarkPicUkey
WHERE a.PackingListID='{item.PackingListID}'
AND b.SCICtnNo='{item.SCICtnNo}'
AND b.ShippingMarkTypeUkey='{item.ShippingMarkTypeUkey}'";

                    updateCmds.Add(updateCmd);
                    idx++;
                }

                using (TransactionScope transactionscope = new TransactionScope())
                {
                    try
                    {
                        DualResult r = DBProxy.Current.Execute(null, updateCmds.JoinToString(Environment.NewLine), paras);

                        if (!r)
                        {
                            transactionscope.Dispose();
                            this.ShowErr(r);
                            return;
                        }

                        transactionscope.Complete();
                    }
                    catch (Exception ex)
                    {
                        transactionscope.Dispose();
                        this.ShowErr("Commit transaction error.", ex);
                        return;
                    }
                }
                #endregion

                this.RenewData();

                this.HideWaitMessage();

                MyUtility.Msg.InfoBox("Import Success !!");
            }
        }

        /// <summary>
        /// GetExcel
        /// </summary>
        /// <param name="strPath">strPath</param>
        /// <param name="strMsg">strMsg</param>
        /// <returns>DataTable</returns>
        public DataTable GetExcel(string strPath, out string strMsg)
        {
            try
            {
                Microsoft.Office.Interop.Excel.Application xlsApp = new Microsoft.Office.Interop.Excel.Application
                {
                    Visible = false,
                };
                Microsoft.Office.Interop.Excel.Workbook xlsBook = xlsApp.Workbooks.Open(strPath);
                Microsoft.Office.Interop.Excel.Worksheet xlsSheet = xlsBook.ActiveSheet;
                Microsoft.Office.Interop.Excel.Range xlsRangeFirstCell = xlsSheet.get_Range("A1");
                Microsoft.Office.Interop.Excel.Range xlsRangeLastCell = xlsSheet.Cells.SpecialCells(Microsoft.Office.Interop.Excel.XlCellType.xlCellTypeLastCell);
                Microsoft.Office.Interop.Excel.Range xlsRange = xlsSheet.get_Range(xlsRangeFirstCell, xlsRangeLastCell);
                object[,] objValue = xlsRange.Value2 as object[,];

                // Array[][] to DataTable
                long lngColumnCount = 12;
                long lngRowCount = objValue.GetLongLength(0);
                DataTable dtExcel = new DataTable();
                for (int j = 1; j <= lngColumnCount; j++)
                {
                    dtExcel.Columns.Add(objValue[1, j].ToString());
                }

                for (int i = 2; i <= lngRowCount; i++)
                {
                    DataRow drRow = dtExcel.NewRow();
                    for (int j = 1; j <= lngColumnCount; j++)
                    {
                        drRow[j - 1] = MyUtility.Check.Empty(objValue[i, j]) ? string.Empty : objValue[i, j].ToString();
                    }

                    dtExcel.Rows.Add(drRow);
                }

                xlsBook.Close();
                xlsApp.Quit();
                strMsg = string.Empty;
                return dtExcel;
            }
            catch (Exception ex)
            {
                strMsg = ex.Message;
                return null;
            }
        }

        private DataTable ToDataTable<T>(List<T> items)
        {
            var tb = new DataTable(typeof(T).Name);

            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo prop in props)
            {
                Type t = GetCoreType(prop.PropertyType);
                tb.Columns.Add(prop.Name, t);
            }

            foreach (T item in items)
            {
                var values = new object[props.Length];

                for (int i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }

                tb.Rows.Add(values);
            }

            return tb;
        }

        /// <summary>
        /// Determine of specified type is nullable
        /// </summary>
        /// <param name="t">t</param>
        /// <returns>bool</returns>
        public static bool IsNullable(Type t)
        {
            return !t.IsValueType || (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>));
        }

        /// <summary>
        /// Return underlying type if type is Nullable otherwise return the type
        /// </summary>
        /// <param name="t">t</param>
        /// <returns>Type</returns>
        public static Type GetCoreType(Type t)
        {
            if (t != null && IsNullable(t))
            {
                if (!t.IsValueType)
                {
                    return t;
                }
                else
                {
                    return Nullable.GetUnderlyingType(t);
                }
            }
            else
            {
                return t;
            }
        }

        private void SetComboSeqAndSide()
        {
            if (this.detailgrid == null)
            {
                return;
            }

            string firstDetailSP = string.Empty;
            DataRow drOrder = null;
            if (this.detailgrid.Rows.Count > 0)
            {
                firstDetailSP = this.detailgrid.Rows[0].Cells["OrderID"].Value.ToString();
                MyUtility.Check.Seek($"select BrandID from orders with (nolock) where ID = '{firstDetailSP}'", out drOrder);
            }

            if (drOrder == null)
            {
                return;
            }
        }

        /// <summary>
        /// 刪除表身
        /// </summary>
        private void ClearDetailtBody()
        {
            // 刪除表身
            foreach (DataRow del in this.DetailDatas)
            {
                del.Delete();
            }
        }

        /// <summary>
        /// 根據PackingListID，取得混尺碼或非混尺碼箱號 (CTNStarNo)
        /// </summary>
        /// <param name="isMixPack">要找混尺碼或非混尺碼</param>
        /// <param name="packingListID">packingListID</param>
        /// <returns>CTNStarNo清單</returns>
        private List<string> GetCTNStartNo_IsMixed(bool isMixPack, string packingListID)
        {
            string cmd = string.Empty;
            List<string> ctnStarNoList = new List<string>();
            DataTable dt;

            if (isMixPack)
            {
                // 取得混尺碼的箱號
                cmd += $@"
SELECT pd.ID,CTNStartNo,COUNT(DISTINCT CTNStartNo),COUNT( CTNStartNo)
FROM PackingList p
INNER JOIN PackingList_Detail pd ON p.ID = pd.ID
WHERE p.ID = '{packingListID}'
GROUP BY pd.ID,CTNStartNo
 HAVING COUNT(DISTINCT CTNStartNo) <> COUNT(CTNStartNo)
";
            }
            else
            {
                // 取得非混尺碼的箱號
                cmd += $@"
SELECT pd.ID,CTNStartNo,COUNT(DISTINCT CTNStartNo),COUNT( CTNStartNo)
FROM PackingList p
INNER JOIN PackingList_Detail pd ON p.ID = pd.ID
WHERE p.ID = '{packingListID}'
GROUP BY pd.ID,CTNStartNo
 HAVING COUNT(DISTINCT CTNStartNo) = COUNT(CTNStartNo)
";
            }

            DBProxy.Current.Select(null, cmd, out dt);
            ctnStarNoList = dt.AsEnumerable().Select(r => MyUtility.Convert.GetString(r["CTNStartNo"])).ToList();

            return ctnStarNoList;
        }

        /// <summary>
        /// 根據PackingListID + BrandID，找出 CustCD 貼標組合的設定
        /// </summary>
        /// <param name="packingListID">packingListID</param>
        /// <returns>CTNStarNo清單</returns>
        private DataTable GetCustCD_Default(string packingListID)
        {
            string cmd = string.Empty;
            List<string> ctnStarNoList = new List<string>();
            DataTable dt;

            cmd = $@"
SELECT DISTINCT 
[StickerCombinationUkey]= ISNULL(s.Ukey,(
	SELECT Ukey 
	FROM ShippingMarkCombination
	WHERE BrandID = p.BrandID AND Category='PIC'  AND IsDefault = 1 AND IsMixPack=0
	))
, [Is Mix Pack] = 'N' 
FROM PackingList p
INNER JOIN PackingList_Detail pd ON p.id = pd.ID
INNER JOIN CustCD c ON c.BrandID = p.BrandID AND c.ID = p.CustCDID
LEFT JOIN ShippingMarkCombination s ON s.Ukey = c.StickerCombinationUkey
where p.ID ='{packingListID}'
UNION

SELECT DISTINCT
[StickerCombinationUkey]= ISNULL(s.Ukey,(
	SELECT Ukey 
	FROM ShippingMarkCombination
	WHERE BrandID = p.BrandID AND Category='PIC'  AND IsDefault = 1 AND IsMixPack=1
	))
, [Is Mix Pack] = 'Y' 
FROM PackingList p
INNER JOIN PackingList_Detail pd ON p.id = pd.ID
INNER JOIN CustCD c ON c.BrandID = p.BrandID AND c.ID = p.CustCDID
LEFT JOIN ShippingMarkCombination s ON s.Ukey = c.StickerCombinationUkey_MixPack
where p.ID ='{packingListID}'

";

            DBProxy.Current.Select(null, cmd, out dt);

            return dt;
        }

        /// <summary>
        /// 透過packingListID 取得P24表身
        /// </summary>
        /// <param name="packingListID">packingListID</param>
        /// <param name="notMixCTNStartNo">非混尺碼箱號</param>
        /// <param name="mixCTNStartNo">混尺碼箱號</param>
        private void GetDetailBody(string packingListID, List<string> notMixCTNStartNo, List<string> mixCTNStartNo)
        {
            string cmd = string.Empty;
            cmd = $@"

SELECT DISTINCT
	 pd.OrderID
	,o.CustPONo
	,pd.CTNStartNo
	,pd.SCICtnNo
	,pd.CustCTN
	,pd.RefNo
	,[ColorWay]=ColorWay.Val
	,[Color]=Color.Val
	,[SizeCode]=SizeCode.Val
    ,[ShippingMarkPicUkey] = 0
	,[ShippingMarkCombination]=comb.ID
	,a.ShippingMarkCombinationUkey
	,[ShippingMarkType]=st.ID
	,b.ShippingMarkTypeUkey
    ,[HTMLFile] = 0----畫面上根據有無FileName欄位打勾的欄位，新增時一定是空的，所以給0
    ,[ShippingMark]=0 ----畫面上根據有無Image欄位打勾的欄位，新增時一定是空的，所以給0
	,b.IsSSCC
	,b.Side
	,b.Seq
	,b.Is2Side
	,b.IsHorizontal
	,b.FromRight
	,b.FromBottom
	,c.Width
	,c.Length
	,[Image]=NULL
	,[FileName]=''
    ,local_file_type=''
    ,FileSourcePath=''
    ,FileAction=''
FROM PackingList p
INNER JOIN PackingList_Detail pd ON p.id = pd.ID
INNER JOIN Orders o ON pd.OrderID = o.ID
INNER JOIN ShippingMarkPicture a ON a.BrandID = p.BrandID  AND a.CTNRefno =  pd.RefNo AND a.Category = 'PIC'
INNER JOIN ShippingMarkPicture_Detail b ON a.Ukey = b.ShippingMarkPictureUkey
INNER JOIN ShippingMarkCombination comb ON comb.BrandID = p.BrandID AND a.ShippingMarkCombinationUkey=comb.Ukey
INNER JOIN ShippingMarkCombination_Detail combD ON comb.Ukey=a.ShippingMarkCombinationUkey 
INNER JOIN ShippingMarkType st ON st.Ukey = b.ShippingMarkTypeUkey
LEFT JOIN StickerSize c ON b.StickerSizeID = c.ID
OUTER APPLY(
	SELECT [Val]=STUFF((
		SELECT DISTINCT ',' + pd2.Article
		FROM PackingList_Detail pd2
		WHERE pd2.ID = p.ID AND pd2.OrderID = pd.OrderID AND pd2.OrderShipmodeSeq = pd.OrderShipmodeSeq AND pd2.CTNStartNo = pd.CTNStartNo
		FOR XML PATH('')
	),1,1,'')
)ColorWay
OUTER APPLY(
	SELECT [Val]=STUFF((
		SELECT DISTINCT ',' + pd2.Color
		FROM PackingList_Detail pd2
		WHERE pd2.ID = p.ID AND pd2.OrderID = pd.OrderID AND pd2.OrderShipmodeSeq = pd.OrderShipmodeSeq AND pd2.CTNStartNo = pd.CTNStartNo
		FOR XML PATH('')
	),1,1,'')
)Color
OUTER APPLY(
	SELECT [Val]=STUFF((
		SELECT DISTINCT ',' + pd2.SizeCode
		FROM PackingList_Detail pd2
		WHERE pd2.ID = p.ID AND pd2.OrderID = pd.OrderID AND pd2.OrderShipmodeSeq = pd.OrderShipmodeSeq AND pd2.CTNStartNo = pd.CTNStartNo
		FOR XML PATH('')
	),1,1,'')
)SizeCode
WHERE p.ID ='{packingListID}'
AND pd.CTNStartNo IN ('{notMixCTNStartNo.JoinToString("','")}')
AND comb.IsMixPack= 0 
AND Comb.Ukey IN (	
	SELECT DISTINCT 
	[Sticker Combination]= ISNULL(s.Ukey,(
		SELECT Ukey 
		FROM ShippingMarkCombination
		WHERE BrandID = p.BrandID AND Category='PIC'  AND IsDefault = 1 AND IsMixPack=0
		))
	--, [Is Mix Pack] = 'N' 
	FROM PackingList p
	INNER JOIN PackingList_Detail pd ON p.id = pd.ID
	INNER JOIN CustCD c ON c.BrandID = p.BrandID AND c.ID = p.CustCDID
	LEFT JOIN ShippingMarkCombination s ON s.Ukey = c.StickerCombinationUkey
	where p.ID = '{packingListID}'
)
UNION ALL

SELECT DISTINCT
	 pd.OrderID
	,o.CustPONo
	,pd.CTNStartNo
	,pd.SCICtnNo
	,pd.CustCTN
	,pd.RefNo
	,[ColorWay]=ColorWay.Val
	,[Color]=Color.Val
	,[SizeCode]=SizeCode.Val
    ,[ShippingMarkPicUkey] = 0
	,[ShippingMarkCombination]=comb.ID
	,a.ShippingMarkCombinationUkey
	,[ShippingMarkType]=st.ID
	,b.ShippingMarkTypeUkey
    ,[HTMLFile]=0 ----畫面上根據有無FileName欄位打勾的欄位，新增時一定是空的，所以給0
    ,[ShippingMark]=0 ----畫面上根據有無Image欄位打勾的欄位，新增時一定是空的，所以給0
	,b.IsSSCC
	,b.Side
	,b.Seq
	,b.Is2Side
	,b.IsHorizontal
	,b.FromRight
	,b.FromBottom
	,c.Width
	,c.Length
	,[Image]=NULL
	,[FileName]=''
    ,local_file_type=''
    ,FileSourcePath=''
    ,FileAction=''
FROM PackingList p
INNER JOIN PackingList_Detail pd ON p.id = pd.ID
INNER JOIN Orders o ON pd.OrderID = o.ID
INNER JOIN ShippingMarkPicture a ON a.BrandID = p.BrandID  AND a.CTNRefno =  pd.RefNo AND a.Category = 'PIC'
INNER JOIN ShippingMarkPicture_Detail b ON a.Ukey = b.ShippingMarkPictureUkey
INNER JOIN ShippingMarkCombination comb ON comb.BrandID = p.BrandID AND a.ShippingMarkCombinationUkey=comb.Ukey
INNER JOIN ShippingMarkCombination_Detail combD ON comb.Ukey=a.ShippingMarkCombinationUkey 
INNER JOIN ShippingMarkType st ON st.Ukey = b.ShippingMarkTypeUkey
LEFT JOIN StickerSize c ON b.StickerSizeID = c.ID
OUTER APPLY(
	SELECT [Val]=STUFF((
		SELECT DISTINCT ',' + pd2.Article
		FROM PackingList_Detail pd2
		WHERE pd2.ID = p.ID AND pd2.OrderID = pd.OrderID AND pd2.OrderShipmodeSeq = pd.OrderShipmodeSeq AND pd2.CTNStartNo = pd.CTNStartNo
		FOR XML PATH('')
	),1,1,'')
)ColorWay
OUTER APPLY(
	SELECT [Val]=STUFF((
		SELECT DISTINCT ',' + pd2.Color
		FROM PackingList_Detail pd2
		WHERE pd2.ID = p.ID AND pd2.OrderID = pd.OrderID AND pd2.OrderShipmodeSeq = pd.OrderShipmodeSeq AND pd2.CTNStartNo = pd.CTNStartNo
		FOR XML PATH('')
	),1,1,'')
)Color
OUTER APPLY(
	SELECT [Val]=STUFF((
		SELECT DISTINCT ',' + pd2.SizeCode
		FROM PackingList_Detail pd2
		WHERE pd2.ID = p.ID AND pd2.OrderID = pd.OrderID AND pd2.OrderShipmodeSeq = pd.OrderShipmodeSeq AND pd2.CTNStartNo = pd.CTNStartNo
		FOR XML PATH('')
	),1,1,'')
)SizeCode
WHERE p.ID ='{packingListID}'
AND pd.CTNStartNo IN ('{mixCTNStartNo.JoinToString("','")}')
AND comb.IsMixPack= 1
AND Comb.Ukey IN (	
	SELECT DISTINCT
	[StickerCombinationUkey]= ISNULL(s.Ukey,(
		SELECT Ukey 
		FROM ShippingMarkCombination
		WHERE BrandID = p.BrandID AND Category='PIC'  AND IsDefault = 1 AND IsMixPack=1
		))
	--, [Is Mix Pack] = 'Y' 
	FROM PackingList p
	INNER JOIN PackingList_Detail pd ON p.id = pd.ID
	INNER JOIN CustCD c ON c.BrandID = p.BrandID AND c.ID = p.CustCDID
	LEFT JOIN ShippingMarkCombination s ON s.Ukey = c.StickerCombinationUkey_MixPack
	where p.ID = '{packingListID}'
)
";

            DataTable ddt;
            DualResult result = DBProxy.Current.Select(null, cmd, out ddt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.detailgridbs.DataSource = ddt;
        }

        /// <summary>
        /// 檢查所有箱子對應 Sticker Combination 的紙箱是否皆已經設定 Packing B03
        /// </summary>
        /// <returns>true = 通過檢查 / false = 不符合</returns>
        private bool CheckCTNRefnoInB03()
        {
            string packingListID = MyUtility.Convert.GetString(this.CurrentMaintain["PackingListID"]);
            long shippingMarkCombinationUkey = MyUtility.Convert.GetLong(this.CurrentMaintain["ShippingMarkCombinationUkey"]);
            long shippingMarkTypeUkey = MyUtility.Convert.GetLong(this.CurrentMaintain["ShippingMarkTypeUkey"]);
            DataTable dt;

            string cmd = $@"
---- Step 2. 找出該PL底下，不存在「已設定箱號」的資料，有的話用於提示訊息
SELECT DISTINCT RefNo 
FROM PackingList pl
INNER JOIN PackingList_Detail pld ON pl.ID = pld.ID
WHERE pl.ID = '{packingListID}'
AND NOT EXISTS(
	---- Step 1. 找出該PL底下已設定的箱號
	SELECT 1
	FROM ShippingMarkPicture s
	INNER JOIN ShippingMarkPicture_Detail sd ON s.Ukey = sd.ShippingMarkPictureUkey
	WHERE EXISTS(
		SELECT 1
		FROM PackingList p 
		INNER JOIN PackingList_Detail pd ON p.ID = pd.ID
		WHERE p.ID = '{packingListID}' AND p.BrandID = s.BrandID AND s.CTNRefno = pd.RefNo
	)
	AND s.ShippingMarkCombinationUkey = {shippingMarkCombinationUkey}
	AND sd.ShippingMarkTypeUkey = {shippingMarkTypeUkey}
	AND s.CTNRefno = pld.RefNo
)
";
            DualResult r = DBProxy.Current.Select(null, cmd, out dt);

            if (!r)
            {
                this.ShowErr(r);
                return false;
            }

            if (dt.Rows.Count == 0)
            {
                // 每種箱子都有設定
                return true;
            }
            else
            {
                // 有箱子沒設定
                string ctnRefnos = dt.AsEnumerable().Select(o => MyUtility.Convert.GetString(o["Refno"])).ToList().JoinToString(",");
                MyUtility.Msg.WarningBox($"CTN Refno {ctnRefnos} not setting Packing B03 yet.");
                return false;
            }
        }

        private bool CheckShippingMarkCombination(string packingListID)
        {
            string cmd = $@"
SELECT * FROM 
(
	SELECT DISTINCT 
	[Sticker Combination]= ISNULL(s.Ukey,(
		SELECT Ukey 
		FROM ShippingMarkCombination
		WHERE BrandID = p.BrandID AND Category='PIC'  AND IsDefault = 1 AND IsMixPack=0
		))
	--, [Is Mix Pack] = 'N' 
	FROM PackingList p
	INNER JOIN PackingList_Detail pd ON p.id = pd.ID
	INNER JOIN CustCD c ON c.BrandID = p.BrandID AND c.ID = p.CustCDID
	LEFT JOIN ShippingMarkCombination s ON s.Ukey = c.StickerCombinationUkey
	where p.ID = '{packingListID}'
	UNION ALL
	SELECT DISTINCT 
	[Sticker Combination]= ISNULL(s.Ukey,(
		SELECT Ukey 
		FROM ShippingMarkCombination
		WHERE BrandID = p.BrandID AND Category='PIC'  AND IsDefault = 1 AND IsMixPack=0
		))
	--, [Is Mix Pack] = 'N' 
	FROM PackingList p
	INNER JOIN PackingList_Detail pd ON p.id = pd.ID
	INNER JOIN CustCD c ON c.BrandID = p.BrandID AND c.ID = p.CustCDID
	LEFT JOIN ShippingMarkCombination s ON s.Ukey = c.StickerCombinationUkey
	where p.ID = '{packingListID}'
) a
WHERE [Sticker Combination] IS NULL

";
            if (MyUtility.Check.Seek(cmd))
            {
                MyUtility.Msg.WarningBox("No Set Packing_B06 by brand!!");
                return false;
            }
            else
            {
                return true;
            }
        }

        private byte[] ConvertFileToByteArray(string filePath)
        {
            byte[] file;

            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = new BinaryReader(stream))
                {
                    file = reader.ReadBytes((int)stream.Length);
                }
            }

            return file;
        }

        private void BtnGenerate_Click(object sender, EventArgs e)
        {
            P24_Generate form = new P24_Generate();
            form.ShowDialog();
            this.Reload();
        }

        /// <inheritdoc/>
        public void Reload()
        {
            if (this.CurrentDataRow != null)
            {
                string idIndex = string.Empty;
                if (!MyUtility.Check.Empty(this.CurrentDataRow))
                {
                    if (!MyUtility.Check.Empty(this.CurrentDataRow["PackingListID"]))
                    {
                        idIndex = MyUtility.Convert.GetString(this.CurrentDataRow["PackingListID"]);
                    }
                }

                this.ReloadDatas();
                this.RenewData();
                if (!MyUtility.Check.Empty(idIndex))
                {
                    this.gridbs.Position = this.gridbs.Find("PackingListID", idIndex);
                }
            }
        }
    }

    /// <inheritdoc/>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleType", Justification = "Reviewed.")]
    public class ShippingMarkPic_Detail
    {
        /// <inheritdoc/>
        public string SCICtnNo { get; set; }

        /// <inheritdoc/>
        public long ShippingMarkTypeUkey { get; set; }

        /// <inheritdoc/>
        public long ShippingMarkPicUkey { get; set; }

        /// <inheritdoc/>
        public byte[] Image { get; set; }

        /// <inheritdoc/>
        public string FileName { get; set; }
    }
}
