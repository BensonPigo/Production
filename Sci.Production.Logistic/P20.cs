#pragma warning disable SA1652 // Enable XML documentation output
using System;
#pragma warning restore SA1652 // Enable XML documentation output
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;
using Sci.Win.Tools;
using System.Linq;
using System.Data.SqlClient;
using System.Collections.Generic;
using Sci.Production.CallPmsAPI;
using System.ComponentModel;
using System.IO;
using Sci.Production.Packing;
using Sci.Win.Tems;
using System.Net.Mail;
using System.Net;
using System.Transactions;

namespace Sci.Production.Logistic
{
    /// <summary>
    /// Logistic_P20
    /// </summary>
    public partial class P20 : QueryForm
    {
        private DataTable dtPackErrTransfer;
        private string specialErrorID = "00006";
        private string mainPackQuerySql = $@"
set ARITHABORT on
select
    [selected] = 0,
    PackingListID = pd.ID,
    pd.CTNStartNo,
    [OrderID] = Stuff((select distinct concat( '/',OrderID)   
        from PackingList_Detail where ID = pd.ID 
        and CTNStartNo = pd.CTNStartNo and DisposeFromClog= 0  FOR XML PATH('')),1,1,''),
    o.CustPONo,
    o.StyleID,
    o.SeasonID,
    o.BrandID,
    c.Alias,
    o.BuyerDelivery,
    pd.Remark,
    ce.PackingErrorDate,
    pd.DRYReceiveDate,
    pd.ClogPackingErrorDate,
    pu.Status,
    [MainSP] = pd.OrderID,
    [ErrorID]=ce.PackingErrorID,
    [ErrorType] = ce.PackingErrorID + '-'+pe.Description,
    pd.SCICtnNo,
    ShipQty=(select sum(ShipQty) from PackingList_Detail pd2 with(nolock) where pd2.id=pd.id and pd2.ctnstartno=pd.ctnstartno),
    ErrQty = ISNULL(ce.ErrQty, 0) ,
    ClogPackingErrorErrQty = ISNULL(ce.ErrQty, 0) ,
    o.FtyGroup,
    pd.SizeCode,
    o.SewLine,
    pd.Seq,
    ClogPackingErrorID = ce.ID,
    ce.CFMDate
from PackingList_Detail pd with (nolock)
inner join PackingList p with (nolock) on pd.ID = p.ID
left join Orders o with (nolock) on o.ID = pd.OrderID
left join Country c with (nolock) on c.ID = o.Dest
left join Pullout pu with (nolock) on pu.ID = p.PulloutID
outer apply(
    ----一個紙箱，可以做多次的ClogPackingError檢驗紀錄，但一定要Completed之後才能再新增新的，因此需要判斷CFMDate IS NULL
	select top 1 *
	from ClogPackingError cpe
	where cpe.CFMDate IS NULL
	AND pd.ID=cpe.PackingListID AND pd.OrderID = cpe.OrderID AND pd.CTNStartNo=cpe.CTNStartNo AND pd.SCICtnNo=cpe.SCICtnNo
	order by cpe.AddDate desc
)ce
left join PackingErrorTypeReason pe with (nolock)  on ce.PackingErrorID = pe.ID
 ";

        /// <summary>
        /// P19
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P20(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.EditMode = true;
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            #region 設定Comobx TypeError

            DataTable dtTypeError;
            DualResult resulterror;
            string strSqlCmd = $@"
select '' as Error,'' as id
union all
select id+'-'+Description as Error,id from PackingErrorTypeReason
where Type='TP' and Junk=0";
            if (resulterror = DBProxy.Current.Select(null, strSqlCmd, out dtTypeError))
            {
                this.comboErrorType.DataSource = dtTypeError;
                this.comboErrorType.DisplayMember = "Error";
                this.comboErrorType.ValueMember = "id";
            }
            else
            {
                this.ShowErr(resulterror);
            }

            #endregion

            this.Helper.Controls.Grid.Generator(this.gridPackErrTransfer)
           .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(2), iseditable: true, trueValue: 1, falseValue: 0)
           .Text("PackingListID", header: "Pack ID", width: Widths.AnsiChars(16), iseditingreadonly: true)
           .Text("CTNStartNo", header: "CTN#", width: Widths.AnsiChars(8), iseditingreadonly: true)
           .Numeric("ShipQty", header: "Pack Qty", iseditingreadonly: true)
           .Numeric("ErrQty", header: "ErrQty")
           .Text("OrderID", header: "SP#", width: Widths.AnsiChars(16), iseditingreadonly: true)
           .Text("CustPoNo", header: "PO#", width: Widths.AnsiChars(16), iseditingreadonly: true)
           .Text("StyleID", header: "Style", width: Widths.AnsiChars(16), iseditingreadonly: true)
           .Text("SeasonID", header: "Season", width: Widths.AnsiChars(12), iseditingreadonly: true)
           .Text("BrandID", header: "Brand", width: Widths.AnsiChars(8), iseditingreadonly: true)
           .Text("ErrorType", header: "ErrorType", width: Widths.AnsiChars(20), iseditingreadonly: true)
           .Text("Alias", header: "Destination", width: Widths.AnsiChars(20), iseditingreadonly: true)
           .Date("BuyerDelivery", header: "Buyer Delivery", iseditingreadonly: true)
           .Date("CFMDate", header: "CFMDate", iseditingreadonly: true)
           .Text("Remark", header: "Remark", width: Widths.AnsiChars(20), iseditingreadonly: true)
           .Button("Detail", null, header: "Detail", width: Widths.AnsiChars(8), onclick: this.BtnDetail_Click)
           .Button("Compeleted", null, header: string.Empty, width: Widths.AnsiChars(13), onclick: this.BtCompleted_Click)
           ;

            string emptySql = this.mainPackQuerySql + " where 1 = 0";
            DualResult result = DBProxy.Current.Select(null, emptySql, out this.dtPackErrTransfer);
            this.listControlBindingSource1.DataSource = this.dtPackErrTransfer;

            // Column的Visible設定為false，但Index要照算
            this.gridPackErrTransfer.Columns["CFMDate"].Visible = false;
        }

        private void BtnDetail_Click(object sender, EventArgs e)
        {
            this.gridPackErrTransfer.EndEdit();
            this.gridPackErrTransfer.ValidateControl();
            DataRow drSelect = this.gridPackErrTransfer.GetDataRow(this.listControlBindingSource1.Position);
            // 當按鈕被隱藏起來就不要開窗顯示
            if (!ChkButtonHide(15))// detail index
            {
                return;
            }

            // 只有特定的ErrorID需要跳出Detail
            if (MyUtility.Convert.GetString(drSelect["ErrorID"]) != specialErrorID)
            {
                return;
            }

            DataTable dt = ((DataTable)this.listControlBindingSource1.DataSource).Copy();

            // 更新欄位名稱
            dt.Columns["PackingListID"].ColumnName = "PackID";
            dt.Columns["CTNStartNo"].ColumnName = "CTN";
            DataRow[] dr = dt.Select($@"PackID = '{drSelect["PackingListID"]}' and CTN = '{drSelect["CTNStartNo"]}'");
            P20_ClogErrorRecord callForm = new P20_ClogErrorRecord(true, drSelect["ClogPackingErrorID"].ToString(), null, null, dr[0]);
            callForm.ShowDialog(this);
        }

        private void BtCompleted_Click(object sender, EventArgs e)
        {
            this.gridPackErrTransfer.EndEdit();
            this.gridPackErrTransfer.ValidateControl();
            DataRow drSelect = this.gridPackErrTransfer.GetDataRow(this.listControlBindingSource1.Position);
            // 當按鈕被隱藏起來就不要開窗顯示
            if (!ChkButtonHide(16))// complete index
            {
                return;
            }

            // 有ClogPackingError記錄才可以Completed
            if (MyUtility.Check.Empty(drSelect["ErrorID"]))
            {
                return;
            }

            string packingID = MyUtility.Convert.GetString(drSelect["PackingListID"]);
            string cTNStartNo = MyUtility.Convert.GetString(drSelect["CTNStartNo"]);
            string clogPackingErrorID = MyUtility.Convert.GetString(drSelect["ClogPackingErrorID"]);

            string sql = $@"
update PackingList_Detail
 SET ClogPackingErrorQty = 0
    ,ClogPackingErrorDate = null
    ,ClogPackingErrorID =''
where ID = '{packingID}' AND CTNStartNo = '{cTNStartNo}'

update ClogPackingError
SET CFMDate = GETDATE()
    ,EditName = '{Sci.Env.User.UserID}'
    ,EditDate = GETDATE()
where ID = '{clogPackingErrorID}'
";

            using (TransactionScope scope = new TransactionScope())
            {
                DualResult result;
                try
                {
                    result = DBProxy.Current.Execute(null, sql);
                    if (!result)
                    {
                        scope.Dispose();
                        this.ShowErr(result);
                    }
                    else
                    {
                        scope.Complete();
                        scope.Dispose();
                        MyUtility.Msg.InfoBox("Success!!");
                    }
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    this.ShowErr(ex);
                }
            }

            this.QueryData();
        }

        private bool ChkButtonHide(int cellNB)
        {
            DataGridViewCellStyle btnCompleteStyle = this.gridPackErrTransfer.Rows[this.listControlBindingSource1.Position].Cells[cellNB].Style;
            DataGridViewCellStyle cellStyleDelete = new DataGridViewCellStyle();
            cellStyleDelete.Padding = new Padding(0, 0, 1000, 0);

            // 當按鈕被隱藏起來就不要開窗顯示
            if (btnCompleteStyle.Padding == cellStyleDelete.Padding)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void BtnFind_Click(object sender, EventArgs e)
        {
            this.QueryData();

            if (this.dtPackErrTransfer == null || this.dtPackErrTransfer.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("No Data Found!");
                return;
            }
        }

        private void TxtScanBarcode_Validating(object sender, CancelEventArgs e)
        {
            string scannedBarcode = this.txtScanBarcode.Text;
            this.txtScanBarcode.Text = string.Empty;

            if (MyUtility.Check.Empty(scannedBarcode))
            {
                return;
            }

            if (scannedBarcode.Length < 14)
            {
                MyUtility.Msg.WarningBox($"<Barcode>{scannedBarcode} format wrong");
                e.Cancel = true;
                return;
            }

            string packID = scannedBarcode.Substring(0, 13);
            string cartonStartNo = scannedBarcode.Substring(13).TrimStart('^');

            foreach (DataGridViewRow dr in this.gridPackErrTransfer.Rows)
            {
                if (dr.Cells["PackingListID"].Value.Equals(packID) && dr.Cells["CTNStartNo"].Value.Equals(cartonStartNo))
                {
                    this.gridPackErrTransfer.SelectRowTo(dr.Index);
                    MyUtility.Msg.WarningBox($"<Barcode>{scannedBarcode} already scanned");
                    e.Cancel = true;
                    return;
                }
            }

            CheckPackResult checkPackResult = this.CheckPackID(packID, cartonStartNo);

            if (checkPackResult.IsOK)
            {
                this.dtPackErrTransfer.ImportRow(checkPackResult.DrResult);
            }
            else
            {
                MyUtility.Msg.WarningBox(checkPackResult.ErrMsg);
            }

            e.Cancel = true;
        }

        private void BtnImportBarcode_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "txt files (*.txt)|*.txt",
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string importFileName = openFileDialog.FileName;
                using (StreamReader reader = new StreamReader(importFileName, Encoding.UTF8))
                {
                    this.ShowWaitMessage("Processing....");
                    string line;
                    try
                    {
                        string forsizesplit = string.Empty;
                        while ((line = reader.ReadLine()) != null)
                        {
                            string[] splitResult = line.Split('\t');
                            if (splitResult.Length != 5)
                            {
                                MyUtility.Msg.WarningBox("Format is not correct!");
                                this.HideWaitMessage();
                                return;
                            }

                            string checkCode = splitResult[0].TrimEnd();

                            if (checkCode != "2")
                            {
                                MyUtility.Msg.WarningBox("Format is not correct!");
                                this.HideWaitMessage();
                                return;
                            }

                            string packID = splitResult[2].Substring(0, 13).TrimEnd();
                            string cartonStartNo = splitResult[2].Substring(13).TrimEnd().TrimStart('^');

                            CheckPackResult checkPackResult = this.CheckPackID(packID, cartonStartNo, false);

                            if (checkPackResult.IsOK)
                            {
                                checkPackResult.DrResult["selected"] = 1;
                                this.dtPackErrTransfer.ImportRow(checkPackResult.DrResult);
                                continue;
                            }

                            checkPackResult = this.CheckPackID(packID, cartonStartNo, true);

                            if (checkPackResult.IsOK)
                            {
                                checkPackResult.DrResult["selected"] = 1;
                                this.dtPackErrTransfer.ImportRow(checkPackResult.DrResult);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MyUtility.Msg.WarningBox("Error Import File:" + Environment.NewLine + ex.Message);
                    }

                    this.HideWaitMessage();
                }
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            this.ShowWaitMessage("Save Processing....");

            // 檢查資料
            var drSelected = this.dtPackErrTransfer.AsEnumerable().Where(s => (int)s["selected"] == 1).ToList();
            StringBuilder warningmsg = new StringBuilder();

            foreach (DataRow dr in drSelected)
            {
                CheckPackResult checkPackResult = this.CheckPackID(dr["PackingListID"].ToString(), dr["CTNStartNo"].ToString());
                if (!checkPackResult.IsOK)
                {
                    MyUtility.Msg.WarningBox(checkPackResult.ErrMsg);
                    this.HideWaitMessage();
                    return;
                }

                if (MyUtility.Check.Empty(dr["ErrQty"]) || MyUtility.Convert.GetString(dr["ErrQty"]) == "0")
                {
                    warningmsg.Append($@"Packing ID:{dr["PackingListID"]}, SP#: {dr["MainSP"]}
, CTN#: {dr["CTNStartNo"]} " + Environment.NewLine);
                }

                if (MyUtility.Check.Empty(dr["ErrorID"]))
                {
                    warningmsg.Append($@"Packing ID:{dr["PackingListID"]}, SP#: {dr["MainSP"]}
, CTN#: {dr["CTNStartNo"]} " + Environment.NewLine);
                }
            }

            if (!MyUtility.Check.Empty(warningmsg.ToString()))
            {
                MyUtility.Msg.WarningBox("Below records needs input Error Type and Qty!" + Environment.NewLine + warningmsg.ToString());
                this.HideWaitMessage();
                return;
            }

            string saveSql = string.Empty;
            DualResult updateResult;
            using (TransactionScope updateScope = new TransactionScope())
            {
                try
                {
                    foreach (DataRow dr in drSelected)
                    {
                        if (MyUtility.Convert.GetString(dr["ErrorID"]) == specialErrorID)
                        {
                            if (MyUtility.Convert.GetInt(dr["ErrQty"]) == 0)
                            {
                                updateScope.Dispose();
                                this.HideWaitMessage();
                                MyUtility.Msg.WarningBox("Error qty cannot be 0 when Error Type is 00006.");
                                return;
                            }

                            if (MyUtility.Convert.GetInt(dr["ErrQty"]) > MyUtility.Convert.GetInt(dr["ShipQty"]))
                            {
                                updateScope.Dispose();
                                this.HideWaitMessage();
                                MyUtility.Msg.WarningBox("Error Qty cannot more than Pack Qty when Error Type is 00006.");
                                return;
                            }
                        }

                        saveSql = $@"
update PackingList_Detail 
set ClogPackingErrorDate = GETDATE() ,ClogPackingErrorQty = {dr["ErrQty"]},ClogPackingErrorID ='{dr["ErrorID"]}'
where ID = '{dr["PackingListID"]}' and CTNStartNo = '{dr["CTNStartNo"]}' and DisposeFromClog= 0;

insert into ClogPackingError(PackingErrorDate,MDivisionID,OrderID,PackingListID,CTNStartNo,AddName,AddDate,PackingErrorID,SCICtnNo,ErrQty)
values(GETDATE(),'{Env.User.Keyword}','{dr["MainSP"]}','{dr["PackingListID"]}','{dr["CTNStartNo"]}','{Env.User.UserID}',GETDATE(),'{dr["ErrorID"]}','{dr["SCICtnNo"]}',{dr["ErrQty"]})
";
                        updateResult = DBProxy.Current.Execute(null, saveSql);
                        if (!updateResult)
                        {
                            updateScope.Dispose();
                            this.ShowErr(updateResult);
                            this.HideWaitMessage();
                            return;
                        }

                        string[] listOrderID = dr["OrderID"].ToString().Split('/');
                        foreach (string orderID in listOrderID)
                        {
                            bool isOkUpdateOrdersCTN = PublicPrg.Prgs.UpdateOrdersCTN(orderID);
                            if (!isOkUpdateOrdersCTN)
                            {
                                updateScope.Dispose();
                                MyUtility.Msg.WarningBox("Update OrdersCTN Fail..");
                                this.HideWaitMessage();
                                return;
                            }
                        }
                    }

                    updateScope.Complete();
                    updateScope.Dispose();

                    MyUtility.Msg.InfoBox("Save successfully");
                    this.HideWaitMessage();
                }
                catch (Exception ex)
                {
                    updateScope.Dispose();
                    this.ShowErr("Commit transaction error.", ex);
                    this.HideWaitMessage();
                    return;
                }
            }

            // 存檔完成後，重新載入資料，讓Detail按鈕自動判斷要不要出現
            this.QueryData();

            var mdFaillist = drSelected.Where(w => MyUtility.Convert.GetString(w["ErrorID"]) == specialErrorID).ToList();
            if (mdFaillist.Any())
            {
                DataTable dtFaillist = drSelected.Where(w => MyUtility.Convert.GetString(w["ErrorID"]) == specialErrorID).CopyToDataTable();
                string sqlcmdfail = @"
select t.*, 
    EG_Description = (select pr.Description from PackingReason pr where pr.type = 'EG' and id = ced.PackingReasonIDForTypeEG),
    EO_Description = (select pr.Description from PackingReason pr where pr.type = 'EO' and id = ced.PackingReasonIDForTypeEO),
    ET_Description = (select pr.Description from PackingReason pr where pr.type = 'ET' and id = ced.PackingReasonIDForTypeET)
from #tmp t
left join ClogPackingError_Detail ced on t.ClogPackingErrorID = ced.ClogPackingErrorID
order by t.PackingListID,t.OrderID, Cast(t.CTNStartNo as int )

drop table #tmp
";
                DualResult result = MyUtility.Tool.ProcessWithDatatable(dtFaillist, null, sqlcmdfail, out DataTable dtFail, "#tmp");
                if (!result)
                {
                    return;
                }

                var ftyGroupList = dtFail.AsEnumerable().Select(s => MyUtility.Convert.GetString(s["ftyGroup"])).Distinct().ToList();
                foreach (var ftyGroup in ftyGroupList)
                {
                    var byFtyGroup = dtFail.AsEnumerable().Where(w => MyUtility.Convert.GetString(w["ftyGroup"]) == ftyGroup).ToList();

                    string sqlcmd = $"select * from MailGroup where Code  = '103' and FactoryID = '{ftyGroup}'";
                    if (MyUtility.Check.Seek(sqlcmd, out DataRow drm) && !MyUtility.Check.Empty(drm["ToAddress"]))
                    {
                        sqlcmd = $"select * from MailTo  where ID  = '103'";
                        MyUtility.Check.Seek(sqlcmd, out DataRow drMailTo);
                        string mailTotoAddress = MyUtility.Convert.GetString(drMailTo["ToAddress"]);
                        string mailToCcAddress = MyUtility.Convert.GetString(drMailTo["CcAddress"]);

                        string toAddress = MyUtility.Convert.GetString(drm["ToAddress"]) + ";" + mailTotoAddress;
                        string ccAddress = MyUtility.Convert.GetString(drm["CcAddress"]) + ";" + mailToCcAddress;
                        string subject = MyUtility.Convert.GetString(drMailTo["Subject"]);
                        string content =
                            @"
<style type='text/css'>
    .tg {
        border-collapse:collapse;
        border-spacing:0;
    }
    .tg td {
        font-family:Arial, sans-serif;
        font-size:14px;
        padding:10px 5px;
        border-style:solid;
        border-width:1px;
        overflow:hidden;
        word-break:normal;
        border-color:black;
    }
    .tg th {
        font-family:Arial, sans-serif;
        font-size:14px;
        font-weight:normal;
        padding:10px 5px;
        border-style:solid;
        border-width:1px;
        overflow:hidden;
        word-break:normal;
        border-color:black;
    }
    .tg .tg-2cz9 {
        font-weight:bold;
        background-color:#ccffcc;
        text-align:center;
        vertical-align:middle
    }
    .tg .tg-nrix {
        text-align:center;
        vertical-align:middle
    }
</style>";

                        content += $@"
<table class='tg' align='left' >
    <thead>
        <tr>
          <th class='tg-2cz9'>Pack ID</th>
          <th class='tg-2cz9'>CTN#</th>
          <th class='tg-2cz9'>Size</th>
          <th class='tg-2cz9'>SP#</th>
          <th class='tg-2cz9'>PO No.</th>
          <th class='tg-2cz9'>Style</th>
          <th class='tg-2cz9'>Season</th>
          <th class='tg-2cz9'>Brand</th>
          <th class='tg-2cz9'>Sewing Line</th>
          <th class='tg-2cz9'>MD Fail Qty </th>
          <th class='tg-2cz9'>Reason for Garment Sound</th>
          <th class='tg-2cz9'>Area/Operation</th>
          <th class='tg-2cz9'>Action Taken</th>
        </tr>
    </thead>
    <tbody>";
                        foreach (var item in byFtyGroup)
                        {
                            content += $@"
        <tr>
            <td>{item["PackingListID"]}</td>
            <td>{item["ctnStartNo"]}</td>
            <td>{item["SizeCode"]}</td>
            <td>{item["MainSP"]}</td>
            <td>{item["custPONo"]}</td>
            <td>{item["StyleID"]}</td>
            <td>{item["SeasonID"]}</td>
            <td>{item["BrandID"]}</td>
            <td>{item["SewLine"]}</td>
            <td>{item["ErrQty"]}</td>
            <td>{item["EG_Description"]}</td>
            <td>{item["EO_Description"]}</td>
            <td>{item["ET_Description"]}</td>
        </tr>";
                        }

                        content += @"
    </tbody>
</table>
<br/><br/><br/>
<br/><br/><br/>
<br/><br/><br/>
";

                        try
                        {
                            MailMessage message = new MailMessage();
                            message.Subject = subject;

                            foreach (var to in toAddress.Split(';'))
                            {
                                if (!MyUtility.Check.Empty(to))
                                {
                                    message.To.Add(to);
                                }
                            }

                            foreach (var cc in ccAddress.Split(';'))
                            {
                                if (!MyUtility.Check.Empty(cc))
                                {
                                    message.CC.Add(cc);
                                }
                            }

                            if (MyUtility.Check.Empty(Env.Cfg.MailFrom))
                            {
                                MyUtility.Msg.WarningBox("Please set <Send From> in Basic B02.");
                                return;
                            }

                            message.From = new MailAddress(Env.Cfg.MailFrom);
                            message.Body = content;
                            message.IsBodyHtml = true;

                            // mail Smtp
                            SmtpClient client = new SmtpClient(Env.Cfg.MailServerIP);

                            // 寄件者 帳密
                            client.Credentials = new NetworkCredential(Env.Cfg.MailServerAccount, Env.Cfg.MailServerPassword);
                            client.DeliveryMethod = SmtpDeliveryMethod.Network;

                            client.Send(message);
                        }
                        catch (Exception ex)
                        {
                            this.ShowErr(ex);
                            return;
                        }
                    }
                }
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void QueryData()
        {
            #region select 條件
            List<SqlParameter> listPar = new List<SqlParameter>();
            string sqlWhere = string.Empty;

            if (!MyUtility.Check.Empty(this.txtSP.Text))
            {
                sqlWhere += "and pd.OrderID = @orderID  " + Environment.NewLine;
                listPar.Add(new SqlParameter("@orderID", this.txtSP.Text));
            }

            if (!MyUtility.Check.Empty(this.txtPOID.Text))
            {
                sqlWhere += "and o.CustPONo = @CustPONo  " + Environment.NewLine;
                listPar.Add(new SqlParameter("@CustPONo", this.txtPOID.Text));
            }

            if (!MyUtility.Check.Empty(this.txtPackID.Text))
            {
                sqlWhere += "and pd.ID = @PackID  " + Environment.NewLine;
                listPar.Add(new SqlParameter("@PackID", this.txtPackID.Text));
            }

            #endregion

            string querySQL = $@"
{this.mainPackQuerySql}
where	pd.CTNStartNo <> '' 
		and p.MDivisionID = '{Env.User.Keyword}' 
		and p.Type in ('B','L') 
        and pd.DisposeFromClog= 0		
		and (pu.Status = 'New' or pu.Status is null) 
        and pd.CTNQty = 1
		--AND ce.CFMDate IS NULL
        {sqlWhere}
order by pd.ID,pd.Seq
";

            DualResult result = DBProxy.Current.Select(null, querySQL, listPar, out this.dtPackErrTransfer);
            this.listControlBindingSource1.DataSource = this.dtPackErrTransfer;
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.HideButton();
        }

        private CheckPackResult CheckPackID(string packID, string cartonStartNo, bool fromCustCTN = false)
        {
            CheckPackResult checkPackResult = new CheckPackResult() { IsOK = true };
            DataRow drPackResult;
            string keyWhere = string.Empty;
            List<SqlParameter> listPar;

            if (fromCustCTN == true)
            {
                keyWhere = $"   and pd.CustCTN = @CustCTN";
                listPar = new List<SqlParameter>()
                                            {
                                                new SqlParameter("@CustCTN", packID + cartonStartNo),
                                            };
            }
            else
            {
                keyWhere = @"   and pd.ID = @ID
                                and pd.CTNStartNo = @CTNStartNo";
                listPar = new List<SqlParameter>()
                                            {
                                                new SqlParameter("@ID", packID),
                                                new SqlParameter("@CTNStartNo", cartonStartNo),
                                            };
            }

            string checkPackSql = $@"
{this.mainPackQuerySql}
where	pd.CTNStartNo <> '' 
		and p.MDivisionID = '{Env.User.Keyword}' 
		and p.Type in ('B','L') 
        and pd.CTNQty = 1
        and pd.DisposeFromClog= 0
		--AND ce.CFMDate IS NULL
        {keyWhere}
";
            bool result = MyUtility.Check.Seek(checkPackSql, listPar, out drPackResult);

            if (!result)
            {
                checkPackResult.ErrMsg = $"<CTN#:{packID + cartonStartNo}> does not exist!";
                checkPackResult.IsOK = false;
                return checkPackResult;
            }

            if (drPackResult["PackingErrorDate"] != DBNull.Value)
            {
                checkPackResult.ErrMsg = $"<CTN#:{packID + cartonStartNo}> has been transferred to Clog!";
                checkPackResult.IsOK = false;
                return checkPackResult;
            }

            if (drPackResult["ClogPackingErrorDate"] != DBNull.Value)
            {
                checkPackResult.ErrMsg = $"<CTN#:{packID + cartonStartNo}> This CTN# Packing Error has been transferred.";
                checkPackResult.IsOK = false;
                return checkPackResult;
            }

            if (drPackResult["Status"].Equals("Confirmed") || drPackResult["Status"].Equals("Locked"))
            {
                checkPackResult.ErrMsg = $"<CTN#:{packID + cartonStartNo}> Already pullout!";
                checkPackResult.IsOK = false;
                return checkPackResult;
            }

            drPackResult["selected"] = 1;
            checkPackResult.DrResult = drPackResult;

            return checkPackResult;
        }

        private class CheckPackResult
        {
            private bool isOK;
            private DataRow drResult;
            private string errMsg;

            public bool IsOK
            {
                get
                {
                    return this.isOK;
                }

                set
                {
                    this.isOK = value;
                }
            }

            public DataRow DrResult
            {
                get
                {
                    return this.drResult;
                }

                set
                {
                    this.drResult = value;
                }
            }

            public string ErrMsg
            {
                get
                {
                    return this.errMsg;
                }

                set
                {
                    this.errMsg = value;
                }
            }
        }

        private void PicUpdate_Click(object sender, EventArgs e)
        {
            this.gridPackErrTransfer.EndEdit();
            this.gridPackErrTransfer.ValidateControl();
            DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
            if (dt == null || dt.Rows.Count == 0)
            {
                return;
            }

            DataRow[] dtfound = dt.Select("Selected = 1");
            foreach (var item in dtfound)
            {
                item["ErrorID"] = this.comboErrorType.SelectedValue;
                item["ErrorType"] = this.comboErrorType.Text;
            }

            MyUtility.Msg.InfoBox("Successful!");
            //this.gridPackErrTransfer.AutoResizeColumns();
        }

        private void BtnToExcel_Click(object sender, EventArgs e)
        {
            DataTable dtExcel = (DataTable)this.listControlBindingSource1.DataSource;
            if (dtExcel == null || dtExcel.Rows.Count <= 0)
            {
                return;
            }

            string sqlcmd = @"
select 
t.PackingListID
,t.CTNStartNo
,t.ShipQty
,t.ErrQty
,t.OrderID
,t.CustPONo
,t.StyleID
,t.SeasonID
,t.BrandID
,t.ErrorType
,t.Alias
,t.BuyerDelivery
    ,EG_Description = (select pr.Description from PackingReason pr where pr.type = 'EG' and id = ced.PackingReasonIDForTypeEG)
    ,EO_Description = (select pr.Description from PackingReason pr where pr.type = 'EO' and id = ced.PackingReasonIDForTypeEO)
    ,ET_Description = (select pr.Description from PackingReason pr where pr.type = 'ET' and id = ced.PackingReasonIDForTypeET)
,t.Remark
from #tmp t
left join ClogPackingError_Detail ced on t.ClogPackingErrorID = ced.ClogPackingErrorID
order by t.PackingListID,t.OrderID, Cast(t.CTNStartNo as int )

drop table #tmp
";
            DualResult result = MyUtility.Tool.ProcessWithDatatable(dtExcel, null, sqlcmd, out DataTable dt, "#tmp");
            if (!result)
            {
                return;
            }

            if (dt == null || dt.Rows.Count == 0)
            {
                MyUtility.Msg.ErrorBox("Data not found");
                return;
            }

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Logistic_P20.xltx"); // 預先開啟excel app
            MyUtility.Excel.CopyToXls(dt, string.Empty, "Logistic_P20.xltx", 2, true, null, objApp);
        }

        /// <summary>
        /// /重新排序，也要做隱藏紐
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridPackErrTransfer_Sorted(object sender, EventArgs e)
        {
            this.HideButton();
        }

        private void HideButton()
        {
            // DataGridView，把Button Cell隱藏
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            dataGridViewCellStyle2.Padding = new Padding(0, 0, 1000, 0);
            foreach (DataGridViewRow row in this.gridPackErrTransfer.Rows)
            {
                // Cells[10] = ErrorType、Cells[13] = CFNDate、Cells[15] = Detail按鈕、Cells[16] = Completed按鈕
                if (!row.Cells[10].Value.StrStartsWith(specialErrorID))
                {
                    // Detail按鈕
                    row.Cells[15].Style = dataGridViewCellStyle2;
                }

                if (MyUtility.Check.Empty(row.Cells[13].Value) && MyUtility.Check.Empty(row.Cells[10].Value))
                {
                    // Completed按鈕
                    row.Cells[16].Style = dataGridViewCellStyle2;
                }
            }
        }
    }
}
