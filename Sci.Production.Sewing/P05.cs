using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;
using System.Linq;
using Sci.Production.PublicPrg;

namespace Sci.Production.Sewing
{
    /// <inheritdoc/>
    public partial class P05 : Win.Tems.Input6
    {
        /// <inheritdoc/>
        public P05(ToolStripMenuItem menuitem)
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
[QrderQty] = (select isnull(sum(Qty),0) from Order_Qty with (nolock) where ID = sd.OrderID and Article = sd.Article),
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
KpiRate = isnull(KpiRate,0)

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
        }

        private Ict.Win.UI.DataGridViewTextBoxColumn col_OrderId;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_ComboType;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_Article;
        private Ict.Win.UI.DataGridViewNumericBoxColumn col_UnitPrice;

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
                this.CurrentDetailData["QrderQty"] = 0;
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
                if (output > (int)this.CurrentDetailData["QrderQty"])
                {
                    MyUtility.Msg.WarningBox("Output Qty can't more than Order Qty");
                    e.Cancel = true;
                    return;
                }

                this.UpdateAccuOutputQty(this.CurrentDetailData);
                if (this.CurrentMaintain["Status"].Equals("Confirmed") && output < (int)this.CurrentDetailData["AccuOutputQty"])
                {
                    MyUtility.Msg.WarningBox("Output Qty can't small than Accu.Output Qty");
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
                    this.CurrentDetailData["QrderQty"] = 0;
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
                    this.CurrentDetailData["QrderQty"] = articleResult;
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
                .Numeric("QrderQty", header: "Order Qty", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("OutputQty", header: "Output Qty", width: Widths.AnsiChars(10), settings: outputQtySet)
                .Numeric("AccuOutputQty", header: "Accu. Output Qty", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("UnitPrice", header: "Price(Unit)", width: Widths.AnsiChars(10), integer_places: 12, decimal_places: 4).Get(out this.col_UnitPrice)
                .Numeric("SewingCPU", header: "Sewing CPU", width: Widths.AnsiChars(10), decimal_places: 4, iseditingreadonly: true)
                .Numeric("CuttingCPU", header: "Cutting CPU", width: Widths.AnsiChars(10), decimal_places: 4, iseditingreadonly: true)
                .Numeric("InspectionCPU", header: "Inspection CPU", width: Widths.AnsiChars(10), decimal_places: 4, iseditingreadonly: true)
                .Numeric("OtherCPU", header: "Other CPU", width: Widths.AnsiChars(10), decimal_places: 4, iseditingreadonly: true)
                .Numeric("OtherAmt", header: "Other Amt", width: Widths.AnsiChars(10), decimal_places: 4, iseditingreadonly: true)
                .Numeric("EMBAmt", header: "EMB Amt", width: Widths.AnsiChars(10), decimal_places: 4, iseditingreadonly: true)
                .Numeric("PrintingAmt", header: "Printing Amt", width: Widths.AnsiChars(10), decimal_places: 4, iseditingreadonly: true)

                .Text("LocalCurrencyID", header: "Currency", width: Widths.AnsiChars(3))
                .Numeric("LocalUnitPrice", header: "U/P Exclude VAT(Local currency)", width: Widths.AnsiChars(12), decimal_places: 4, settings: localUnitPrice)
                .Numeric("Vat", header: "VAT (Local currency)", width: Widths.AnsiChars(10), decimal_places: 2, settings: vat)
                .Numeric("UPIncludeVAT", header: "U/P Include VAT(Local currency)", width: Widths.AnsiChars(10), decimal_places: 4, iseditingreadonly: true)
                .Numeric("KpiRate", header: "Kpi Rate", width: Widths.AnsiChars(3), maximum: 9, decimal_places: 2)
                ;
            this.detailgrid.RowSelecting += (s, e) =>
            {
                DataRow curDr = ((DataTable)this.detailgridbs.DataSource).Rows[e.RowIndex];
                if (this.EditMode && this.CurrentMaintain["Status"].Equals("Confirmed"))
                {
                    foreach (DataGridViewColumn item in this.detailgrid.Columns)
                    {
                        item.DefaultCellStyle.ForeColor = Color.Black;
                    }

                    if (curDr.RowState == DataRowState.Modified || curDr.RowState == DataRowState.Unchanged)
                    {
                        this.detailgrid.Rows[e.RowIndex].Cells["OutputQty"].ReadOnly = false;
                        this.detailgrid.Rows[e.RowIndex].Cells["OutputQty"].Style.ForeColor = Color.Red;
                        if ((int)curDr["AccuOutputQty"] > 0)
                        {
                            this.gridicon.Remove.Enabled = false;
                        }
                        else
                        {
                            this.gridicon.Remove.Enabled = true;
                        }
                    }
                    else
                    {
                        this.detailgrid.Rows[e.RowIndex].Cells["OrderId"].Style.ForeColor = Color.Red;
                        this.detailgrid.Rows[e.RowIndex].Cells["ComboType"].Style.ForeColor = Color.Red;
                        this.detailgrid.Rows[e.RowIndex].Cells["Article"].Style.ForeColor = Color.Red;
                        this.detailgrid.Rows[e.RowIndex].Cells["OutputQty"].Style.ForeColor = Color.Red;
                        this.detailgrid.Rows[e.RowIndex].Cells["UnitPrice"].Style.ForeColor = Color.Red;
                    }
                }
                else if (this.EditMode)
                {
                    this.detailgrid.Rows[e.RowIndex].Cells["OrderId"].Style.ForeColor = Color.Red;
                    this.detailgrid.Rows[e.RowIndex].Cells["ComboType"].Style.ForeColor = Color.Red;
                    this.detailgrid.Rows[e.RowIndex].Cells["Article"].Style.ForeColor = Color.Red;
                    this.detailgrid.Rows[e.RowIndex].Cells["OutputQty"].Style.ForeColor = Color.Red;
                    this.detailgrid.Rows[e.RowIndex].Cells["UnitPrice"].Style.ForeColor = Color.Red;
                }
            };

            // 設定detailGrid Rows 是否可以編輯
            this.detailgrid.RowEnter += this.Detailgrid_RowEnter;

            base.OnDetailGridSetup();
        }

        private void Detailgrid_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || !this.EditMode)
            {
                return;
            }

            DataRow curDr = ((DataTable)this.detailgridbs.DataSource).Rows[e.RowIndex];
            if (this.CurrentMaintain["Status"].Equals("Confirmed"))
            {
                if (curDr.RowState == DataRowState.Modified || curDr.RowState == DataRowState.Unchanged)
                {
                    this.col_OrderId.IsEditingReadOnly = true;
                    this.col_ComboType.IsEditingReadOnly = true;
                    this.col_Article.IsEditingReadOnly = true;
                    this.col_UnitPrice.IsEditingReadOnly = true;

                    if ((int)curDr["AccuOutputQty"] > 0)
                    {
                        this.gridicon.Remove.Enabled = false;
                    }
                    else
                    {
                        this.gridicon.Remove.Enabled = true;
                    }
                }
                else
                {
                    this.col_OrderId.IsEditingReadOnly = false;
                    this.col_ComboType.IsEditingReadOnly = false;
                    this.col_Article.IsEditingReadOnly = false;
                    this.col_UnitPrice.IsEditingReadOnly = false;
                }
            }
            else
            {
                this.col_OrderId.IsEditingReadOnly = false;
                this.col_ComboType.IsEditingReadOnly = false;
                this.col_Article.IsEditingReadOnly = false;
                this.col_UnitPrice.IsEditingReadOnly = false;
            }
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
                this.CurrentDetailData["QrderQty"] = 0;
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
            this.CurrentDetailData["QrderQty"] = item.GetSelecteds()[0][1];
            this.GetAccuOutputQty();
            return result;
        }

        private void GetAccuOutputQty()
        {
            string getAccuOutputQty = $@"select isnull(sum(sod.QAQty),0) 
    from SewingOutput s with (nolock)
    inner join SewingOutput_Detail sod with (nolock) on s.ID = sod.ID
    where   s.SubConOutContractNumber = '{this.CurrentMaintain["Contractnumber"]}' and
            s.SubconOutFty = '{this.CurrentMaintain["SubConOutFty"]}'  and
            sod.OrderID = '{this.CurrentDetailData["OrderID"]}' and
            sod.Article = '{this.CurrentDetailData["Article"]}' and
            sod.Combotype  = '{this.CurrentDetailData["ComboType"]}'";

            this.CurrentDetailData["AccuOutputQty"] = MyUtility.GetValue.Lookup(getAccuOutputQty);
        }
    }
}