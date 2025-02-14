using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;
using System.Linq;
using Sci.Production.PublicPrg;
using System.Text;
using System;
using System.Windows.Forms.VisualStyles;

namespace Sci.Production.Subcon
{
    /// <inheritdoc/>
    public partial class P11 : Win.Tems.Input6
    {
        private bool FirstCheckOutputQty = true;

        /// <inheritdoc/>
        public P11(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = $"MDivisionID = '{Env.User.Keyword}'";
        }

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string subConOutFty = (e.Master == null) ? string.Empty : e.Master["SubConOutFty"].ToString();
            string contractNumber = (e.Master == null) ? string.Empty : e.Master["ContractNumber"].ToString();
            string cmd = $@"
select 
    sd.SubConOutFty,
    sd.ContractNumber,
    sd.OrderId,
    o.StyleID,
    sd.ComboType,
    sd.Article,
    [OrderQty] = (select isnull(sum(Qty),0) from Order_Qty with (nolock) where ID = sd.OrderID and Article = sd.Article),
    sd.OutputQty,
    [AccuOutputQty] = (
        select isnull(sum(sod.QAQty),0) 
        from SewingOutput s with (nolock)
        inner join SewingOutput_Detail sod with (nolock) on s.ID = sod.ID
        where   s.SubConOutContractNumber = sd.ContractNumber and
                s.SubconOutFty = sd.SubConOutFty  and
                sod.OrderID = sd.OrderID and
                sod.Article = sd.Article and
                sod.Combotype  = sd.Combotype
        ),
    UnitPrice = sd.UnitPrice,
    UnitPriceByComboType = sd.UnitPrice * r.rate,
    SewingCPU = tms.SewingCPU*r.rate,
    CuttingCPU = tms.CuttingCPU*r.rate,
    InspectionCPU = tms.InspectionCPU*r.rate,
    OtherCPU = tms.OtherCPU*r.rate,
    OtherAmt = tms.OtherAmt*r.rate,
    EMBAmt = tms.EMBAmt*r.rate,
    PrintingAmt = tms.PrintingAmt*r.rate,
    OtherPrice = tms.OtherPrice*r.rate,
    EMBPrice = tms.EMBPrice*r.rate,
    PrintingPrice = tms.PrintingPrice*r.rate,
    LocalCurrencyID = LocalCurrencyID,
    LocalUnitPrice = isnull(LocalUnitPrice,0),
    Vat = isnull(Vat,0),

    UPIncludeVAT = isnull(LocalUnitPrice,0)+isnull(Vat,0),
    KpiRate = isnull(KpiRate,0),
    [Addrow] = '',
    [old_OrderId] = ''
from dbo.SubconOutContract_Detail sd with (nolock)
left join Orders o with (nolock) on sd.Orderid = o.ID
OUTER apply (
    select  
    [SewingCPU] = sum(iif(ArtworkTypeID = 'SEWING',TMS/1400,0)),
    [CuttingCPU]= sum(iif(ArtworkTypeID = 'CUTTING',TMS/1400,0)),
    [InspectionCPU]= sum(iif(ArtworkTypeID = 'INSPECTION',TMS/1400,0)),
    [OtherCPU]= sum(iif(ArtworkTypeID in ('INSPECTION','CUTTING','SEWING'),0,TMS/1400)),
    [OtherAmt]= sum(iif(ArtworkTypeID in ('PRINTING','EMBROIDERY'),0,Price)) * sd.OutputQty,
    [EMBAmt] = sum(iif(ArtworkTypeID = 'EMBROIDERY',Price,0)) * sd.OutputQty,
    [PrintingAmt] = sum(iif(ArtworkTypeID = 'PRINTING',Price,0)) * sd.OutputQty,
    [OtherPrice]= sum(iif(ArtworkTypeID in ('PRINTING','EMBROIDERY'),0,Price)),
    [EMBPrice] = sum(iif(ArtworkTypeID = 'EMBROIDERY',Price,0)),
    [PrintingPrice] = sum(iif(ArtworkTypeID = 'PRINTING',Price,0))
    from Order_TmsCost with (nolock)
    where ID = sd.OrderID
) as tms
outer apply(select rate = isnull(dbo.GetOrderLocation_Rate(o.ID,sd.ComboType)
,(select rate = rate from Style_Location sl with (nolock) where sl.StyleUkey = o.StyleUkey and sl.Location = sd.ComboType))/100)r
where sd.SubConOutFty = '{subConOutFty}' and sd.ContractNumber = '{contractNumber}'
";
            this.DetailSelectCommand = cmd;
            return base.OnDetailSelectCommandPrepare(e);
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            #region 檢查必輸欄位
            if (this.DetailDatas.Count() == 0)
            {
                MyUtility.Msg.WarningBox("Please add detail data!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["SubConOutFty"]))
            {
                MyUtility.Msg.WarningBox("SubCon-Out-Fty can't empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["ContractNumber"]))
            {
                MyUtility.Msg.WarningBox("Contract Number can't empty!!");
                return false;
            }

            var detailEmptyCells = this.DetailDatas
                .Where(s =>
                      MyUtility.Check.Empty(s["OrderID"]) ||
                      MyUtility.Check.Empty(s["ComboType"]) ||
                      MyUtility.Check.Empty(s["Article"]) ||
                      MyUtility.Check.Empty(s["OutputQty"]) ||
                      MyUtility.Check.Empty(s["UnitPrice"]));
            if (detailEmptyCells.Count() > 0)
            {
                MyUtility.Msg.WarningBox("Detail data SP#、ComboType、Article、Output Qty、Price(Unit) can't empty or 0!!");
                return false;
            }

            DualResult resultCheckLocalSupp_BankStatus = Prgs.CheckLocalSupp_BankStatus(this.CurrentMaintain["SubConOutFty"].ToString(), Prgs.CallFormAction.Save);
            if (!resultCheckLocalSupp_BankStatus)
            {
                return false;
            }
            #endregion

            #region 檢查資料重複
            string cmd = @"select distinct OrderID,Article,Combotype from #tmp";
            DualResult checkDuplicate = MyUtility.Tool.ProcessWithDatatable(this.DetailDatas.CopyToDataTable(), "OrderID,Article,Combotype", cmd, out DataTable dtDuplicate);

            if (!checkDuplicate)
            {
                this.ShowErr(checkDuplicate);
                return false;
            }

            if (this.DetailDatas.Count() != dtDuplicate.Rows.Count)
            {
                this.ShowErr("'SP#,ComboType,Article' detail key cannot duplicate.");
                return false;
            }
            #endregion

            #region 檢查Subcon Out Qty是否大於Accu. Subcon Out Qty
            string sqlCheckSubconOutQty = @"
alter table #tmp alter column ContractNumber varchar(50)
alter table #tmp alter column SubConOutFty varchar(8)
alter table #tmp alter column OrderID varchar(13)
alter table #tmp alter column Article varchar(8)
alter table #tmp alter column Combotype varchar(1)

select  sd.OrderID,
        sd.Article,
        sd.Combotype,
        [Subcon Out Qty] = sd.OutputQty,
        [Accu. Subcon Out Qty] = AccuOutputQty.val
from    #tmp sd
outer   apply( select [val] = isnull(sum(sod.QAQty),0) 
               from SewingOutput s with (nolock)
               inner join SewingOutput_Detail sod with (nolock) on s.ID = sod.ID
               where   s.SubConOutContractNumber = sd.ContractNumber and
                       s.SubconOutFty = sd.SubConOutFty  and
                       sod.OrderID = sd.OrderID and
                       sod.Article = sd.Article and
                       sod.Combotype  = sd.Combotype) AccuOutputQty
where   AccuOutputQty.val > sd.OutputQty
";
            DataTable dtCheckSubconOutQtyResult;
            DualResult checkSubconOutQtyResult = MyUtility.Tool.ProcessWithDatatable(this.DetailDatas.CopyToDataTable(), "ContractNumber,SubConOutFty,OrderID,Article,Combotype,OutputQty", sqlCheckSubconOutQty, out dtCheckSubconOutQtyResult);

            if (!checkSubconOutQtyResult)
            {
                this.ShowErr(checkSubconOutQtyResult);
                return false;
            }

            if (dtCheckSubconOutQtyResult.Rows.Count > 0)
            {
                MyUtility.Msg.ShowMsgGrid(dtCheckSubconOutQtyResult, "Subcon Out Qty can not less than Accu. Subcon Out Qty.", "Warning");

                foreach (DataRow dr in dtCheckSubconOutQtyResult.Rows)
                {
                    DataRow drRefreshAccuSubconQty = this.DetailDatas.Where(s =>
                    s["OrderID"].ToString() == dr["OrderID"].ToString() &&
                    s["Article"].ToString() == dr["Article"].ToString() &&
                    s["Combotype"].ToString() == dr["Combotype"].ToString())
                    .First();

                    drRefreshAccuSubconQty["AccuOutputQty"] = dr["Accu. Subcon Out Qty"];
                }

                return false;
            }
            #endregion

            StringBuilder errorMsg = new StringBuilder("<Subcon Qty> over than <Order Qty> SP# List:");
            int errorcnt = 0;
            foreach (DataRow dr in this.DetailDatas)
            {
                if (MyUtility.Convert.GetDecimal(dr["OutputQty"]) > MyUtility.Convert.GetDecimal(dr["OrderQty"]))
                {
                    errorMsg.Append(Environment.NewLine + dr["OrderID"]);
                    errorcnt += 1;
                }
            }

            if (errorcnt > 0)
            {
                MyUtility.Msg.WarningBox(errorMsg.ToString());
            }

            #region 檢查OutputQty數量統計是否小於Order_Qty.Qty

            if (this.FirstCheckOutputQty)
            {
                sqlCheckSubconOutQty = @"
alter table #tmp alter column OrderID varchar(13)
select  sd.OrderID,
        sd.Article,
        sd.StyleID,
        [Subcon Out Qty] = TotalOutputQty.OutputQty + sd.OutputQty,
        [Order_Qty Qty] = Order_Qty.Qty
from    #tmp sd
outer   apply( select Qty = isnull(sum(oq.Qty),0) from Order_Qty oq 
               where oq.ID = sd.OrderID 
                and oq.Article = sd.Article
) Order_Qty
outer   apply( select OutputQty = isnull(sum(sd2.OutputQty),0) from SubconOutContract_Detail sd2 
               where sd2.OrderID = sd.OrderID 
                and sd2.Article = sd.Article
                and not exists (select 1 from #tmp t where t.OrderId = sd2.OrderId 
													and t.Article = sd2.Article 
													and t.SubConOutFty = sd2.SubConOutFty 
													and t.ContractNumber = sd2.ContractNumber 
													and t.ComboType = sd2.ComboType)
) TotalOutputQty
where   Order_Qty.Qty < TotalOutputQty.OutputQty + sd.OutputQty
";
                checkSubconOutQtyResult = MyUtility.Tool.ProcessWithDatatable(this.DetailDatas.CopyToDataTable(), "StyleID,SubConOutFty,ContractNumber,ComboType,OrderID,Article,OutputQty", sqlCheckSubconOutQty, out dtCheckSubconOutQtyResult);

                if (!checkSubconOutQtyResult)
                {
                    this.ShowErr(checkSubconOutQtyResult);
                    return false;
                }

                if (dtCheckSubconOutQtyResult.Rows.Count > 0)
                {
                    var formMsg = MyUtility.Msg.ShowMsgGrid(dtCheckSubconOutQtyResult, "SP# apply qty over Order's qty, please check again and press save button if confirm.", "Warning");
                    formMsg.Width = 900;
                    formMsg.grid1.Columns[0].Width = 120;
                    formMsg.grid1.Columns[2].Width = 100;
                    formMsg.Visible = false;
                    formMsg.ShowDialog();
                    this.FirstCheckOutputQty = false;
                    return false;
                }
            }
            #endregion

            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.txtSubConOutFty.TextBox1.ReadOnly = false;
            this.txtContractnumber.ReadOnly = false;
            this.CurrentMaintain["MDivisionID"] = Env.User.Keyword;
            this.CurrentMaintain["factoryid"] = Env.User.Factory;
            this.CurrentMaintain["Status"] = "New";
            this.btnBatchImport.Enabled = true;
            this.gridicon.Append.Enabled = true;
            this.gridicon.Insert.Enabled = true;
            this.gridicon.Remove.Enabled = true;
        }

        /// <inheritdoc/>
        protected override bool ClickEditBefore()
        {
            base.ClickEditBefore();

            if (this.CurrentMaintain["Status"].ToString().ToUpper().EqualString("CLOSED"))
            {
                MyUtility.Msg.WarningBox("This record already Close, cannot modify this record!!");
                return false;
            }

            return true;
        }

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtSubConOutFty.TextBox1.ReadOnly = true;
            this.txtContractnumber.ReadOnly = true;
            if (this.CurrentMaintain["Status"].Equals("Confirmed"))
            {
                this.dateIssuedate.ReadOnly = true;
                this.btn_JunkSP.Enabled = false;

                // Confirmed下表身不能編輯
                this.gridicon.Append.Enabled = false;
                this.gridicon.Insert.Enabled = false;
                this.gridicon.Remove.Enabled = false;
            }
            else
            {
                this.gridicon.Append.Enabled = true;
                this.gridicon.Insert.Enabled = true;
                this.gridicon.Remove.Enabled = true;
            }
        }

        /// <inheritdoc/>
        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            if (MyUtility.Check.Empty(this.CurrentMaintain["Issuedate"]))
            {
                MyUtility.Msg.WarningBox("Issue Date cannot be empty!");
                return;
            }

            DualResult resultCheckLocalSupp_BankStatus = Prgs.CheckLocalSupp_BankStatus(this.CurrentMaintain["SubConOutFty"].ToString(), Prgs.CallFormAction.Confirm);
            if (!resultCheckLocalSupp_BankStatus)
            {
                return;
            }

            string updConfirm = $"update dbo.SubconOutContract set Status = 'Confirmed', ApvName = '{Env.User.UserID}' ,ApvDate = getdate() where SubConOutFty = '{this.CurrentMaintain["SubConOutFty"]}' and ContractNumber = '{this.CurrentMaintain["ContractNumber"]}'";
            DualResult result = DBProxy.Current.Execute(null, updConfirm);
            if (!result)
            {
                this.ShowErr(result);
            }
        }

        /// <inheritdoc/>
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            this.refresh.PerformClick();

            foreach (DataRow dr in this.DetailDatas)
            {
                this.UpdateAccuOutputQty(dr);
            }

            var chkCanUnconfirm = this.DetailDatas.Where(s => (int)s["AccuOutputQty"] > 0);
            if (chkCanUnconfirm.Count() > 0)
            {
                MyUtility.Msg.WarningBox("Detail data Accu. Output Qty more then 0, can't Unconfirm");
                return;
            }

            string updConfirm = $"update dbo.SubconOutContract set Status = 'New', ApvName = '' ,ApvDate = null where SubConOutFty = '{this.CurrentMaintain["SubConOutFty"]}' and ContractNumber = '{this.CurrentMaintain["ContractNumber"]}'";
            DualResult result = DBProxy.Current.Execute(null, updConfirm);
            if (!result)
            {
                this.ShowErr(result);
            }
        }

        /// <inheritdoc/>
        protected override void ClickClose()
        {
            base.ClickClose();
            string sqlcmd;
            sqlcmd = $@"
update SubconOutContract 
set status='Closed', editname='{Env.User.UserID}', editdate=GETDATE() 
where SubConOutFty = '{this.CurrentMaintain["SubConOutFty"]}' 
and ContractNumber = '{this.CurrentMaintain["ContractNumber"]}'";

            DualResult result;
            if (!(result = DBProxy.Current.Execute(null, sqlcmd)))
            {
                this.ShowErr(sqlcmd, result);
                return;
            }
        }

        private void UpdateAccuOutputQty(DataRow dr)
        {
            string sqlCmd = $@" select isnull(sum(sod.QAQty),0) 
    from SewingOutput s with (nolock)
    inner join SewingOutput_Detail sod with (nolock) on s.ID = sod.ID
    where   s.SubConOutContractNumber = '{this.CurrentMaintain["Contractnumber"]}' and
            s.SubconOutFty = '{this.CurrentMaintain["SubConOutFty"]}'  and
            sod.OrderID = '{dr["OrderID"]}' and
            sod.Article = '{dr["Article"]}' and
            sod.Combotype  = '{dr["ComboType"]}'";

            int accuOutputQty = MyUtility.Convert.GetInt(MyUtility.GetValue.Lookup(sqlCmd));
            dr["AccuOutputQty"] = accuOutputQty;
        }

        /// <inheritdoc/>
        protected override bool ClickDeleteBefore()
        {
            if (this.CurrentMaintain["Status"].Equals("Confirmed"))
            {
                MyUtility.Msg.WarningBox("This Data already confirm, can't delete");
                return false;
            }

            return base.ClickDeleteBefore();
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.txtuser1.TextBox1.ReadOnly = true;
            this.label10.Text = this.CurrentMaintain["Status"].ToString();
            this.FirstCheckOutputQty = true;

            this.btnBatchImport.Enabled = this.EditMode == true && this.CurrentMaintain["Status"].ToString().ToUpper().EqualString("NEW");
            this.btnSplitSP.Enabled = this.EditMode == true && this.CurrentMaintain["Status"].ToString().ToUpper().EqualString("CONFIRMED");
            this.btn_JunkHis.Enabled = !this.EditMode;

            if (this.CurrentMaintain["Status"].Equals("Confirmed"))
            {
                this.col_OrderId.IsEditingReadOnly = true;
                this.col_ComboType.IsEditingReadOnly = true;
                this.col_Article.IsEditingReadOnly = true;
                this.col_UnitPrice.IsEditingReadOnly = true;
                this.col_LocalCurrencyID.IsEditingReadOnly = true;
                this.col_LocalUnitPrice.IsEditingReadOnly = true;
                this.col_Vat.IsEditingReadOnly = true;
                this.col_KpiRate.IsEditingReadOnly = true;
                this.btn_JunkSP.Enabled = true;
            }
            else
            {
                this.col_OrderId.IsEditingReadOnly = false;
                this.col_ComboType.IsEditingReadOnly = false;
                this.col_Article.IsEditingReadOnly = false;
                this.col_UnitPrice.IsEditingReadOnly = false;
                this.col_LocalCurrencyID.IsEditingReadOnly = false;
                this.col_LocalUnitPrice.IsEditingReadOnly = false;
                this.col_Vat.IsEditingReadOnly = false;
                this.col_KpiRate.IsEditingReadOnly = false;
                this.btn_JunkSP.Enabled = false;
            }

            if (this.EditMode && this.CurrentMaintain["Status"].Equals("Confirmed"))
            {
                foreach (Ict.Win.UI.DataGridViewTextBoxBaseColumn item in this.detailgrid.Columns)
                {
                    if (item.IsEditingReadOnly)
                    {
                        item.DefaultCellStyle.ForeColor = Color.Black;
                    }
                }

                this.gridicon.Remove.Enabled = false;
            }
        }

        private Ict.Win.UI.DataGridViewTextBoxColumn col_OrderId;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_ComboType;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_Article;
        private Ict.Win.UI.DataGridViewNumericBoxColumn col_UnitPrice;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_LocalCurrencyID;
        private Ict.Win.UI.DataGridViewNumericBoxColumn col_LocalUnitPrice;
        private Ict.Win.UI.DataGridViewNumericBoxColumn col_Vat;
        private Ict.Win.UI.DataGridViewNumericBoxColumn col_KpiRate;

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            DataGridViewGeneratorTextColumnSettings orderIdSet = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings comboTypeSet = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings articleSet = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorNumericColumnSettings outputQtySet = new DataGridViewGeneratorNumericColumnSettings();

            #region SP# event
            orderIdSet.CellValidating += (s, e) =>
            {
                if (e.FormattedValue.Equals(this.CurrentDetailData["OrderID"]))
                {
                    return;
                }

                this.CurrentDetailData["OrderID"] = e.FormattedValue;
                this.CurrentDetailData["StyleID"] = string.Empty;
                this.CurrentDetailData["ComboType"] = string.Empty;
                this.CurrentDetailData["Article"] = string.Empty;
                this.CurrentDetailData["OrderQty"] = 0;
                this.CurrentDetailData["OutputQty"] = 0;
                this.CurrentDetailData["AccuOutputQty"] = 0;
                this.CurrentDetailData["UnitPrice"] = 0;
                this.CurrentDetailData["SewingCPU"] = 0;
                this.CurrentDetailData["CuttingCPU"] = 0;
                this.CurrentDetailData["InspectionCPU"] = 0;
                this.CurrentDetailData["OtherCPU"] = 0;
                this.CurrentDetailData["OtherAmt"] = 0;
                this.CurrentDetailData["EMBAmt"] = 0;
                this.CurrentDetailData["PrintingAmt"] = 0;
                this.CurrentDetailData["OtherPrice"] = 0;
                this.CurrentDetailData["EMBPrice"] = 0;
                this.CurrentDetailData["PrintingPrice"] = 0;

                if (MyUtility.Check.Empty(e.FormattedValue))
                {
                    return;
                }

                string chkQrderSql = $@"
select 1
from  Orders o WITH (NOLOCK) 
inner join Factory f on o.FactoryID = f.ID
where   o.MDivisionID = '{this.CurrentMaintain["MDivisionID"]}'
        and o.ID = '{e.FormattedValue}'
		and o.Category NOT IN ( 'G' ,'A')
        and f.IsProduceFty = 1
";
                bool chkQrder = MyUtility.Check.Seek(chkQrderSql);
                if (chkQrder == false)
                {
                    MyUtility.Msg.WarningBox($"SP#<{e.FormattedValue}> not found");
                    e.Cancel = true;
                    return;
                }

                string ChkSubconOut = $@"
select 1
from  Orders o WITH (NOLOCK) 
inner join Factory f on o.FactoryID = f.ID
where   o.MDivisionID = '{this.CurrentMaintain["MDivisionID"]}'
        and o.ID = '{e.FormattedValue}'
        and f.isSubcon = 0
";
                if (MyUtility.Check.Seek(ChkSubconOut))
                {
                    MyUtility.Msg.WarningBox("Wrong FTY code for subcon out. Please ask TPE Planning team to update FTY code to subcon out fty.");
                    this.CurrentDetailData["OrderID"] = string.Empty;
                    e.Cancel = true;
                    return;
                }

                DualResult result;
                result = this.GetComboType();
                if (result == false)
                {
                    e.Cancel = true;
                    return;
                }

                result = this.GetTmsData(e.FormattedValue.ToString(), this.CurrentDetailData["ComboType"].ToString());
                if (result == false)
                {
                    e.Cancel = true;
                    return;
                }

                this.GetAccuOutputQty();
            };

            // 右鍵開窗
            orderIdSet.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    this.GetOrderID();
                }
            };

            orderIdSet.EditingMouseDoubleClick += (s, e) =>
            {
                if (this.EditMode)
                {
                    this.GetOrderID();
                }
            };
            #endregion

            #region ComboType event
            comboTypeSet.CellValidating += (s, e) =>
            {
                if (MyUtility.Check.Empty(e.FormattedValue))
                {
                    return;
                }

                string comboTypeCheck = $@"select 1 
                    from orders o with (nolock) 
inner join dbo.Style_Location sl with (nolock) on o.styleUkey = sl.styleUkey
where o.id = '{this.CurrentDetailData["OrderID"]}' and sl.Location = '{e.FormattedValue}'";

                if (MyUtility.Check.Seek(comboTypeCheck) == false)
                {
                    MyUtility.Msg.WarningBox("ComboType not exists");
                    e.Cancel = true;
                    return;
                }

                this.CurrentDetailData["ComboType"] = e.FormattedValue;
                this.GetAccuOutputQty();
            };

            // 右鍵開窗
            comboTypeSet.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    this.GetComboType();
                }
            };

            comboTypeSet.EditingMouseDoubleClick += (s, e) =>
            {
                if (this.EditMode)
                {
                    this.GetComboType();
                }
            };
            #endregion

            #region OutputQty event
            outputQtySet.CellValidating += (s, e) =>
            {
                decimal output = decimal.Parse(e.FormattedValue.ToString());
                if (output > (int)this.CurrentDetailData["OrderQty"])
                {
                    MyUtility.Msg.WarningBox("Subcon Out Qty can't more than Order Qty");
                    e.Cancel = true;
                    return;
                }

                this.UpdateAccuOutputQty(this.CurrentDetailData);
                if (this.CurrentMaintain["Status"].Equals("Confirmed") && output < (int)this.CurrentDetailData["AccuOutputQty"])
                {
                    MyUtility.Msg.WarningBox("Subcon Out Qty can't small than Accu.Output Qty");
                    e.Cancel = true;
                    return;
                }

                this.CurrentDetailData["OutputQty"] = output;
                this.CurrentDetailData["OtherAmt"] = (decimal)this.CurrentDetailData["OtherPrice"] * output;
                this.CurrentDetailData["EMBAmt"] = (decimal)this.CurrentDetailData["EMBPrice"] * output;
                this.CurrentDetailData["PrintingAmt"] = (decimal)this.CurrentDetailData["PrintingPrice"] * output;
            };

            #endregion

            #region Article
            articleSet.CellValidating += (s, e) =>
            {
                if (e.FormattedValue.Equals(this.CurrentDetailData["Article"]))
                {
                    return;
                }

                this.CurrentDetailData["Article"] = e.FormattedValue;

                if (MyUtility.Check.Empty(e.FormattedValue))
                {
                    this.CurrentDetailData["OrderQty"] = 0;
                    this.CurrentDetailData["OutputQty"] = 0;
                    return;
                }

                string checkArticle = $@"select isnull(Sum(Qty),0) from Order_Qty with (nolock) where id = '{this.CurrentDetailData["OrderId"]}' and Article = '{e.FormattedValue}'";
                string articleResult = MyUtility.GetValue.Lookup(checkArticle);

                if (articleResult == "0")
                {
                    MyUtility.Msg.WarningBox($"Article<{e.FormattedValue}> not found");
                    e.Cancel = true;
                    return;
                }
                else
                {
                    this.CurrentDetailData["OrderQty"] = articleResult;
                    this.CurrentDetailData["OutputQty"] = 0;
                    this.GetAccuOutputQty();
                }
            };

            // 右鍵開窗
            articleSet.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    this.GetArticle();
                }
            };

            articleSet.EditingMouseDoubleClick += (s, e) =>
            {
                this.GetArticle();
            };
            #endregion

            #region
            DataGridViewGeneratorNumericColumnSettings localUnitPrice = new DataGridViewGeneratorNumericColumnSettings();
            localUnitPrice.CellValidating += (s, e) =>
            {
                this.CurrentDetailData["LocalUnitPrice"] = e.FormattedValue;
                this.CurrentDetailData["UPIncludeVAT"] = MyUtility.Convert.GetDecimal(this.CurrentDetailData["LocalUnitPrice"]) + MyUtility.Convert.GetDecimal(this.CurrentDetailData["Vat"]);
                this.CurrentDetailData.EndEdit();
            };

            DataGridViewGeneratorNumericColumnSettings vat = new DataGridViewGeneratorNumericColumnSettings();
            vat.CellValidating += (s, e) =>
            {
                this.CurrentDetailData["Vat"] = e.FormattedValue;
                this.CurrentDetailData["UPIncludeVAT"] = MyUtility.Convert.GetDecimal(this.CurrentDetailData["LocalUnitPrice"]) + MyUtility.Convert.GetDecimal(this.CurrentDetailData["Vat"]);
                this.CurrentDetailData.EndEdit();
            };
            #endregion

            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("OrderId", header: "SP#", width: Widths.AnsiChars(15), settings: orderIdSet).Get(out this.col_OrderId)
                .Text("StyleID", header: "Style", width: Widths.AnsiChars(18), iseditingreadonly: true)
                .Text("ComboType", header: "ComboType", width: Widths.AnsiChars(5), settings: comboTypeSet).Get(out this.col_ComboType)
                .Text("Article", header: "Article", width: Widths.AnsiChars(8), settings: articleSet).Get(out this.col_Article)
                .Numeric("OrderQty", header: "Order Qty", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("OutputQty", header: "Subcon Out Qty", width: Widths.AnsiChars(10), iseditingreadonly: false, settings: outputQtySet)
                .Numeric("AccuOutputQty", header: "Accu. Subcon Out Qty", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("UnitPrice", header: "Price(Unit)", width: Widths.AnsiChars(10), integer_places: 12, decimal_places: 4).Get(out this.col_UnitPrice)
                .Numeric("UnitPriceByComboType", header: "Price(Unit) by ComboType", width: Widths.AnsiChars(10), integer_places: 12, decimal_places: 4, iseditingreadonly: true)
                .Numeric("SewingCPU", header: "Sewing CPU", width: Widths.AnsiChars(10), decimal_places: 4, iseditingreadonly: true)
                .Numeric("CuttingCPU", header: "Cutting CPU", width: Widths.AnsiChars(10), decimal_places: 4, iseditingreadonly: true)
                .Numeric("InspectionCPU", header: "Inspection CPU", width: Widths.AnsiChars(10), decimal_places: 4, iseditingreadonly: true)
                .Numeric("OtherCPU", header: "Other CPU", width: Widths.AnsiChars(10), decimal_places: 4, iseditingreadonly: true)
                .Numeric("OtherAmt", header: "Other Amt", width: Widths.AnsiChars(10), decimal_places: 4, iseditingreadonly: true)
                .Numeric("EMBAmt", header: "EMB Amt", width: Widths.AnsiChars(10), decimal_places: 4, iseditingreadonly: true)
                .Numeric("PrintingAmt", header: "Printing Amt", width: Widths.AnsiChars(10), decimal_places: 4, iseditingreadonly: true)

                .Text("LocalCurrencyID", header: "Currency", width: Widths.AnsiChars(3)).Get(out this.col_LocalCurrencyID)
                .Numeric("LocalUnitPrice", header: "U/P Exclude VAT(Local currency)", width: Widths.AnsiChars(12), decimal_places: 4, settings: localUnitPrice).Get(out this.col_LocalUnitPrice)
                .Numeric("Vat", header: "VAT (Local currency)", width: Widths.AnsiChars(10), decimal_places: 2, settings: vat).Get(out this.col_Vat)
                .Numeric("UPIncludeVAT", header: "U/P Include VAT(Local currency)", width: Widths.AnsiChars(10), decimal_places: 4, iseditingreadonly: true)
                .Numeric("KpiRate", header: "Kpi Rate", width: Widths.AnsiChars(3), maximum: 9, decimal_places: 2).Get(out this.col_KpiRate)
                ;

            base.OnDetailGridSetup();
        }

        private DualResult GetComboType()
        {
            DataTable checkLocation;
            DualResult result = new DualResult(true);

            string checkLocationSql = $@"
select sl.Location,sl.Rate 
from dbo.Style_Location sl with (nolock) 
inner join orders o with (nolock) on sl.StyleUkey  = o.StyleUkey
where o.id = '{this.CurrentDetailData["OrderId"]}'";
            result = DBProxy.Current.Select(null, checkLocationSql, out checkLocation);
            if (result == false)
            {
                this.ShowErr(result);
                return result;
            }

            if (checkLocation.Rows.Count == 1)
            {
                this.CurrentDetailData["ComboType"] = checkLocation.Rows[0]["Location"];
            }
            else if (checkLocation.Rows.Count > 1)
            {
                Win.Tools.SelectItem item = new Win.Tools.SelectItem(checkLocation, "Location,Rate", "10,15", null, null);
                DialogResult dialogResult = item.ShowDialog();
                if (dialogResult == DialogResult.Cancel)
                {
                    return result;
                }

                this.CurrentDetailData["ComboType"] = item.GetSelecteds()[0][0];
            }

            return result;
        }

        private void GetOrderID()
        {
            string orderChkSql = $@"
select o.ID,o.BrandID,o.StyleID,o.SeasonID
from  Orders o WITH (NOLOCK) 
inner join Factory f on o.FactoryID = f.ID
where   o.MDivisionID = '{this.CurrentMaintain["MDivisionID"]}'
		and o.Category NOT IN ( 'G' ,'A')
        and f.IsProduceFty = 1 and o.Junk = 0 order by o.AddDate desc
";
            Win.Tools.SelectItem item = new Win.Tools.SelectItem(orderChkSql, "18,15,12,8", null, null);
            DialogResult dialogResult = item.ShowDialog();
            if (dialogResult != DialogResult.Cancel)
            {
                if (this.CurrentDetailData["OrderId"].Equals(item.GetSelecteds()[0][0]))
                {
                    return;
                }

                this.CurrentDetailData["OrderId"] = item.GetSelecteds()[0][0];
                this.CurrentDetailData["StyleID"] = string.Empty;
                this.CurrentDetailData["ComboType"] = string.Empty;
                this.CurrentDetailData["Article"] = string.Empty;
                this.CurrentDetailData["OrderQty"] = 0;
                this.CurrentDetailData["OutputQty"] = 0;
                this.CurrentDetailData["AccuOutputQty"] = 0;
                this.CurrentDetailData["UnitPrice"] = 0;
                this.CurrentDetailData["SewingCPU"] = 0;
                this.CurrentDetailData["CuttingCPU"] = 0;
                this.CurrentDetailData["InspectionCPU"] = 0;
                this.CurrentDetailData["OtherCPU"] = 0;
                this.CurrentDetailData["OtherAmt"] = 0;
                this.CurrentDetailData["EMBAmt"] = 0;
                this.CurrentDetailData["PrintingAmt"] = 0;
                this.CurrentDetailData["OtherPrice"] = 0;
                this.CurrentDetailData["EMBPrice"] = 0;
                this.CurrentDetailData["PrintingPrice"] = 0;

                DualResult result;
                result = this.GetComboType();
                if (result == false)
                {
                    return;
                }

                result = this.GetTmsData(this.CurrentDetailData["OrderId"].ToString(), this.CurrentDetailData["ComboType"].ToString());
                if (result == false)
                {
                    return;
                }

                this.GetAccuOutputQty();
            }
        }

        private DualResult GetTmsData(string orderID, string comboType)
        {
            DualResult result = new DualResult(true);
            DataRow resultDr;
            string chkQrderSql = $@"
select
o.StyleID,
o.StyleUkey,
SewingCPU = tms.SewingCPU*r.rate,
CuttingCPU = tms.CuttingCPU*r.rate,
InspectionCPU = tms.InspectionCPU*r.rate,
OtherCPU = tms.OtherCPU*r.rate,
OtherPrice = tms.OtherPrice*r.rate,
EMBPrice = tms.EMBPrice*r.rate,
PrintingPrice = tms.PrintingPrice*r.rate
from  Orders o WITH (NOLOCK) 
inner join Factory f on o.FactoryID = f.ID
outer apply (
    select  
    [SewingCPU] = sum(iif(ArtworkTypeID = 'SEWING',TMS/1400,0)),
    [CuttingCPU]= sum(iif(ArtworkTypeID = 'CUTTING',TMS/1400,0)),
    [InspectionCPU]= sum(iif(ArtworkTypeID = 'INSPECTION',TMS/1400,0)),
    [OtherCPU]= sum(iif(ArtworkTypeID in ('INSPECTION','CUTTING','SEWING'),0,TMS/1400)),
    [OtherPrice]= sum(iif(ArtworkTypeID in ('PRINTING','EMBROIDERY'),0,Price)),
    [EMBPrice] = sum(iif(ArtworkTypeID = 'EMBROIDERY',Price,0)),
    [PrintingPrice] = sum(iif(ArtworkTypeID = 'PRINTING',Price,0))
    from Order_TmsCost with (nolock)
    where ID = o.ID
) as tms
outer apply(select rate = isnull(dbo.GetOrderLocation_Rate(o.ID,'{comboType}')
,(select rate = rate from Style_Location sl with (nolock) where sl.StyleUkey = o.StyleUkey and sl.Location = '{comboType}'))/100)r
where   o.MDivisionID = '{this.CurrentMaintain["MDivisionID"]}'
        and o.ID = '{orderID}'
		and o.Category NOT IN ( 'G' ,'A')
        and f.IsProduceFty = 1
";
            bool chkQrder = MyUtility.Check.Seek(chkQrderSql, out resultDr);
            if (chkQrder == false)
            {
                MyUtility.Msg.WarningBox($"SP#<{orderID}> not found");
                return new DualResult(false);
            }
            else
            {
                this.CurrentDetailData["StyleID"] = resultDr["StyleID"];
                this.CurrentDetailData["SewingCPU"] = resultDr["SewingCPU"];
                this.CurrentDetailData["CuttingCPU"] = resultDr["CuttingCPU"];
                this.CurrentDetailData["InspectionCPU"] = resultDr["InspectionCPU"];
                this.CurrentDetailData["OtherCPU"] = resultDr["OtherCPU"];
                this.CurrentDetailData["OtherPrice"] = resultDr["OtherPrice"];
                this.CurrentDetailData["EMBPrice"] = resultDr["EMBPrice"];
                this.CurrentDetailData["PrintingPrice"] = resultDr["PrintingPrice"];

                return result;
            }
        }

        private DualResult GetArticle()
        {
            DualResult result = new DualResult(true);

            if (MyUtility.Check.Empty(this.CurrentDetailData["OrderID"]))
            {
                return result;
            }

            string chkArticleSql = $@"select Article,[Order Qty] = isnull(sum(Qty),0) from Order_Qty with (nolock) where id = '{this.CurrentDetailData["OrderID"]}' group by Article";
            Win.Tools.SelectItem item = new Win.Tools.SelectItem(chkArticleSql, "10,15", null, null);
            DialogResult dialogResult = item.ShowDialog();
            if (dialogResult == DialogResult.Cancel)
            {
                return result;
            }

            if (this.CurrentDetailData["Article"].Equals(item.GetSelecteds()[0][0]))
            {
                return result;
            }

            this.CurrentDetailData["Article"] = item.GetSelecteds()[0][0];
            this.CurrentDetailData["OrderQty"] = item.GetSelecteds()[0][1];
            this.GetAccuOutputQty();
            return result;
        }

        private void GetAccuOutputQty()
        {
            string getAccuOutputQty = $@"
    select isnull(sum(sod.QAQty),0) 
    from SewingOutput s with (nolock)
    inner join SewingOutput_Detail sod with (nolock) on s.ID = sod.ID
    where   s.SubConOutContractNumber = '{this.CurrentMaintain["Contractnumber"]}' and
            s.SubconOutFty = '{this.CurrentMaintain["SubConOutFty"]}'  and
            sod.OrderID = '{this.CurrentDetailData["OrderID"]}' and
            sod.Article = '{this.CurrentDetailData["Article"]}' and
            sod.Combotype  = '{this.CurrentDetailData["ComboType"]}'";

            this.CurrentDetailData["AccuOutputQty"] = MyUtility.GetValue.Lookup(getAccuOutputQty);
        }

        private void BtnBatchImport_Click(object sender, System.EventArgs e)
        {
            if (this.EditMode == true && this.CurrentMaintain["Status"].ToString().ToUpper().EqualString("NEW"))
            {
                this.detailgrid.EndEdit();
                var frm = new P11_BatchImport((DataTable)this.detailgridbs.DataSource);
                frm.ShowDialog(this);
                int i = 0;
                foreach (DataRow dr in ((DataTable)this.detailgridbs.DataSource).Rows)
                {
                    if (dr["Addrow"].ToString() == "Y")
                    {
                        this.detailgridbs.Position = i;
                        DualResult result = this.GetTmsData(this.CurrentDetailData["OrderId"].ToString(), this.CurrentDetailData["ComboType"].ToString());
                        if (result == false)
                        {
                            return;
                        }

                        this.GetAccuOutputQty();

                        decimal output = MyUtility.Convert.GetDecimal(this.CurrentDetailData["OutputQty"]);
                        this.CurrentDetailData["OtherAmt"] = (decimal)this.CurrentDetailData["OtherPrice"] * output;
                        this.CurrentDetailData["EMBAmt"] = (decimal)this.CurrentDetailData["EMBPrice"] * output;
                        this.CurrentDetailData["PrintingAmt"] = (decimal)this.CurrentDetailData["PrintingPrice"] * output;
                        this.CurrentDetailData.EndEdit();
                    }

                    i++;
                }

                this.RenewData();
            }
        }

        private void BtnSplitSP_Click(object sender, System.EventArgs e)
        {
            this.detailgrid.EndEdit();
            var frm = new P11_SplitSP((DataTable)this.detailgridbs.DataSource);
            frm.ShowDialog(this);
            foreach (DataRow dr in ((DataTable)this.detailgridbs.DataSource).Rows)
            {
                DataRow[] findrow = ((DataTable)this.detailgridbs.DataSource).AsEnumerable().Where(
                  row => row.RowState != DataRowState.Deleted &&
                  row["OrderID"].EqualString(dr["old_OrderID"].ToString()) &&
                  row["ComboType"].EqualString(dr["ComboType"].ToString()) &&
                  row["Article"].EqualString(dr["Article"].ToString())
                  ).ToArray();

                if (dr["Addrow"].ToString() == "Y" && findrow.Length > 0)
                {
                    dr["StyleID"] = findrow[0]["StyleID"];
                    dr["SewingCPU"] = findrow[0]["SewingCPU"];
                    dr["CuttingCPU"] = findrow[0]["CuttingCPU"];
                    dr["InspectionCPU"] = findrow[0]["InspectionCPU"];
                    dr["OtherCPU"] = findrow[0]["OtherCPU"];
                    dr["OtherPrice"] = findrow[0]["OtherPrice"];
                    dr["EMBPrice"] = findrow[0]["EMBPrice"];
                    dr["PrintingPrice"] = findrow[0]["PrintingPrice"];
                    dr["AccuOutputQty"] = findrow[0]["AccuOutputQty"];
                    dr["UnitPrice"] = findrow[0]["UnitPrice"];
                    dr["OtherAmt"] = findrow[0]["OtherAmt"];
                    dr["EMBAmt"] = findrow[0]["EMBAmt"];
                }
            }

            this.RenewData();
        }

        /// <inheritdoc/>
        protected override void OnDetailGridAppendClick()
        {
            base.OnDetailGridAppendClick();
            this.FirstCheckOutputQty = true;
        }

        /// <inheritdoc/>
        protected override void OnDetailGridInsertClick()
        {
            base.OnDetailGridInsertClick();
            this.FirstCheckOutputQty = true;
        }

        /// <inheritdoc/>
        protected override void OnDetailGridRemoveClick()
        {
            base.OnDetailGridRemoveClick();
            this.FirstCheckOutputQty = true;
        }

        private void Btn_JunkSP_Click(object sender, EventArgs e)
        {
            DataTable dtDetail = this.CurrentDetailData.Table;
            if (dtDetail.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox(" Contract detail is empty.");
                return;
            }

            DataTable table = dtDetail.Copy();
            var form = new P11_JunkSP(table);
            form.ShowDialog();
            this.OnDetailEntered();
        }

        private void Btn_JunkHis_Click(object sender, EventArgs e)
        {
            var form = new P11_History(this.CurrentMaintain);
            form.ShowDialog();
        }
    }
}