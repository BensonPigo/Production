using Ict;
using Sci.Production.PublicPrg;
using System;
using System.Data;
using System.Linq;
using System.Transactions;
using System.Windows.Forms;
using Ict.Win;
using Sci.Data;
using System.IO;
using Sci.Production.Automation;
using System.Threading.Tasks;

namespace Sci.Production.Logistic
{
    /// <summary>
    /// P11
    /// </summary>
    public partial class P11 : Win.Tems.Input6
    {
        /// <summary>
        /// P11
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P11(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = $"MDivisionID = '{Env.User.Keyword}'";
            this.gridicon.Append.Visible = false;
            this.gridicon.Insert.Visible = false;
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["Status"] = "New";
            this.CurrentMaintain["DisposeDate"] = DateTime.Now;
            this.CurrentMaintain["MDivisionID"] = Env.User.Keyword;

            this.OnDetailGridRemoveClick();
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.lblStatus.Text = this.CurrentMaintain["status"].ToString();

            if (this.EditMode)
            {
                this.btnImport.Enabled = true;
                this.btnExcelImport.Enabled = true;
            }
            else
            {
                this.btnImport.Enabled = false;
                this.btnExcelImport.Enabled = false;
            }
        }

        /// <inheritdoc/>
        protected override bool ClickDeleteBefore()
        {
            if (this.CurrentMaintain["Status"].Equals("Confirmed"))
            {
                MyUtility.Msg.WarningBox("Confirmed data can not delete!!");
                return false;
            }

            return base.ClickDeleteBefore();
        }

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();
            this.DetailSelectCommand = $@"  
select  
        cdd.ID,
        cdd.PackingListID,
        cdd.CTNStartNO,
        pd.OrderID,
        o.CustPoNo,
        o.StyleID,
        pd.Article,
        pd.Color,
        [Size] =  (SELECT Stuff((select  concat( '/',SizeCode)
						from (select distinct pda.SizeCode,osca.Seq
								from PackingList_Detail pda with (nolock)
								inner join Orders o2 with (nolock) on pda.OrderID = o2.ID
						        inner join Order_SizeCode osca with (nolock) on o2.POID = osca.ID and pda.SizeCode = osca.SizeCode
								where pda.ID = cdd.PackingListID and pda.CTNStartNO = cdd.CTNStartNO ) a  order by Seq
					FOR XML PATH('')),1,1,'')) ,
        [QtyPerCTN] = (SELECT Stuff((select  concat( '/',QtyPerCTN)
						from (select [QtyPerCTN] = sum(pda.QtyPerCTN),osca.Seq
								from PackingList_Detail pda with (nolock)
								inner join Orders o2 with (nolock) on pda.OrderID = o2.ID
						        inner join Order_SizeCode osca with (nolock) on o2.POID = osca.ID and pda.SizeCode = osca.SizeCode
								where pda.ID = cdd.PackingListID and pda.CTNStartNO = cdd.CTNStartNO group by  pda.SizeCode,osca.Seq ) a  order by Seq
							FOR XML PATH('')),1,1,'')) ,
        pd.ClogLocationID
from ClogGarmentDispose_Detail cdd with (nolock)
left join PackingList_Detail pd with (nolock) on  pd.ID = cdd.PackingListID and pd.CTNStartNO = cdd.CTNStartNO and CTNQty = 1
left join Orders o with (nolock) on o.ID = pd.OrderID
where cdd.ID = '{masterID}'
";
            return base.OnDetailSelectCommandPrepare(e);
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            #region -- 欄位設定 --
            this.Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("PackingListID", header: "Pack ID", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("CTNStartNO", header: "CTN#", width: Widths.AnsiChars(5), iseditingreadonly: true)
            .Text("OrderID", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("CustPoNo", header: "PO#", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("StyleID", header: "Style", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("Article", header: "ColorWay", width: Widths.AnsiChars(9), iseditingreadonly: true)
            .Text("Color", header: "Color", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("Size", header: "Size", width: Widths.AnsiChars(5), iseditingreadonly: true)
            .Text("QtyPerCTN", header: "Qty", width: Widths.AnsiChars(5), iseditingreadonly: true)
            .CellClogLocation("ClogLocationID", header: "Location", width: Widths.AnsiChars(8), iseditingreadonly: true);

            #endregion 欄位設定
        }

        /// <inheritdoc/>
        protected override bool ClickEditBefore()
        {
            if (!this.CurrentMaintain["Status"].Equals("New"))
            {
                MyUtility.Msg.WarningBox("The record status is not new, can't modify !!");
                return false;
            }

            return base.ClickEditBefore();
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            bool isNewSave = MyUtility.Check.Empty(this.CurrentMaintain["ID"]);
            if (MyUtility.Check.Empty(this.CurrentMaintain["ClogReasonID"]))
            {
                MyUtility.Msg.WarningBox("< Reason > can't be empty!");
                this.txtClogReason.TextBox1.Focus();
                return false;
            }

            if (isNewSave)
            {
                this.CurrentMaintain["ID"] = MyUtility.GetValue.GetID(Env.User.Keyword + "GD", "ClogGarmentDispose", DateTime.Now);
            }

            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override void ClickConfirm()
        {
            base.ClickConfirm();

            DualResult result;

            var listDistinctPKID = this.DetailDatas.Select(s => s["PackingListID"].ToString()).Distinct();
            bool isPackingAlreadyPullout = false;
            foreach (string packingID in listDistinctPKID)
            {
                isPackingAlreadyPullout = MyUtility.Check.Seek($"select 1 from PackingList where ID = '{packingID}' AND PulloutID <> '' AND PulloutID IS NOT NULL ");
                if (isPackingAlreadyPullout)
                {
                    MyUtility.Msg.WarningBox($"<{packingID}> already pullout, cannot confirm!!");
                    return;
                }
            }

            #region 檢查detail資料是否已被confirm過
            string sqlCheckConfirmed = $@"
select cd.ID,cd.PackingListID,cd.CTNStartNO
from ClogGarmentDispose c
inner join ClogGarmentDispose_Detail cd with (nolock) on c.ID = cd.ID
inner join ClogGarmentDispose_Detail cd1 on cd1.ID = '{this.CurrentMaintain["ID"]}' and cd.PackingListID = cd1.PackingListID and cd.CTNStartNO = cd1.CTNStartNO
where c.Status = 'Confirmed' and c.ID <> '{this.CurrentMaintain["ID"]}'";

            result = DBProxy.Current.Select(null, sqlCheckConfirmed, out DataTable dtDisposeConfirmed);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            bool isDetailAlreadyConfirmed = dtDisposeConfirmed.Rows.Count > 0;
            if (isDetailAlreadyConfirmed)
            {
                MyUtility.Msg.ShowMsgGrid_LockScreen(dtDisposeConfirmed, "Detail data are already confirmed by other ID, please check the following data");
                return;
            }
            #endregion

            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    string updateCMD = $@"
update ClogGarmentDispose set Status = 'Confirmed' , EditName = '{Env.User.UserID}', EditDate = GETDATE() where ID = '{this.CurrentMaintain["ID"]}'

update pd set pd.DisposeFromClog = 1
, pd.DisposeDate = Getdate()
from PackingList_Detail pd
where exists (select 1 from ClogGarmentDispose_Detail t where t.ID = '{this.CurrentMaintain["ID"]}' and t.PackingListID = pd.ID and t.CTNStartNO = pd.CTNStartNO)
";

                    result = DBProxy.Current.Execute(null, updateCMD);
                    if (!result)
                    {
                        transactionScope.Dispose();
                        this.ShowErr(result);
                        return;
                    }

                    DataColumn dtDetailCol = new DataColumn("OrderID", typeof(string));
                    DataTable dtDetail = new DataTable();
                    dtDetail.Columns.Add(dtDetailCol);
                    DataTable dtOrderID = this.DetailDatas.GroupBy(s => s["OrderID"]).Select(s =>
                    {
                        DataRow dr = dtDetail.NewRow();
                        dr["OrderID"] = s.Key;
                        return dr;
                    }).CopyToDataTable();

                    result = Prgs.UpdateOrdersCTN(dtOrderID);
                    if (!result)
                    {
                        transactionScope.Dispose();
                        this.ShowErr(result);
                        return;
                    }

                    transactionScope.Complete();
                }
                catch (Exception ex)
                {
                    transactionScope.Dispose();
                    this.ShowErr(ex);
                    return;
                }
            }

            MyUtility.Msg.InfoBox("Confirmed successful");
            #region ISP20201607 資料交換 - Gensong
            if (Gensong_FinishingProcesses.IsGensong_FinishingProcessesEnable)
            {
                // 不透過Call API的方式，自己組合，傳送API
                Task.Run(() => new Gensong_FinishingProcesses().SentClogGarmentDisposeToFinishingProcesses(this.CurrentMaintain["ID"].ToString(), string.Empty))
                    .ContinueWith(UtilityAutomation.AutomationExceptionHandler, TaskContinuationOptions.OnlyOnFaulted);
            }
            #endregion
        }

        /// <inheritdoc/>
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    string updateCMD = $@"
update ClogGarmentDispose set Status = 'New' , EditName = '{Env.User.UserID}', EditDate = GETDATE() where ID = '{this.CurrentMaintain["ID"]}'

update pd set pd.DisposeFromClog = 0
,pd.DisposeDate = null
from PackingList_Detail pd
where exists (select 1 from ClogGarmentDispose_Detail t where t.ID = '{this.CurrentMaintain["ID"]}' and t.PackingListID = pd.ID and t.CTNStartNO = pd.CTNStartNO)
";
                    DualResult result = DBProxy.Current.Execute(null, updateCMD);
                    if (!result)
                    {
                        transactionScope.Dispose();
                        this.ShowErr(result);
                        return;
                    }

                    DataColumn dtDetailCol = new DataColumn("OrderID", typeof(string));
                    DataTable dtDetail = new DataTable();
                    dtDetail.Columns.Add(dtDetailCol);
                    DataTable dtOrderID = this.DetailDatas.GroupBy(s => s["OrderID"]).Select(s =>
                    {
                        DataRow dr = dtDetail.NewRow();
                        dr["OrderID"] = s.Key;
                        return dr;
                    }).CopyToDataTable();

                    result = Prgs.UpdateOrdersCTN(dtOrderID);
                    if (!result)
                    {
                        transactionScope.Dispose();
                        this.ShowErr(result);
                        return;
                    }

                    transactionScope.Complete();
                }
                catch (Exception ex)
                {
                    transactionScope.Dispose();
                    this.ShowErr(ex);
                    return;
                }
            }

            MyUtility.Msg.InfoBox("UnConfirmed successful");

            #region ISP20201607 資料交換 - Gensong
            if (Gensong_FinishingProcesses.IsGensong_FinishingProcessesEnable)
            {
                // 不透過Call API的方式，自己組合，傳送API
                Task.Run(() => new Gensong_FinishingProcesses().SentClogGarmentDisposeToFinishingProcesses(this.CurrentMaintain["ID"].ToString(), string.Empty))
                    .ContinueWith(UtilityAutomation.AutomationExceptionHandler, TaskContinuationOptions.OnlyOnFaulted);
            }
            #endregion
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            if (!this.EditMode)
            {
                return;
            }

            P11_Import p11_Import = new P11_Import(this.CurrentMaintain["ID"].ToString(), (DataTable)this.detailgridbs.DataSource);
            p11_Import.ShowDialog();
        }

        private void BtnExcelImport_Click(object sender, EventArgs e)
        {
            P11_ExcelImport callNextForm = new P11_ExcelImport(this.CurrentMaintain, (DataTable)this.detailgridbs.DataSource);
            callNextForm.ShowDialog(this);
        }

        private void BtnDownloadExcel_Click(object sender, EventArgs e)
        {
            // 呼叫執行檔絕對路徑
            DirectoryInfo dir = new DirectoryInfo(Application.StartupPath);

            string strXltName = Env.Cfg.XltPathDir + "\\ClogP11_ExcelImportTemplete.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return;
            }

            excel.Visible = true;
        }
    }
}
