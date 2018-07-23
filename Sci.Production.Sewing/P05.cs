using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Win.Tems;
using Ict.Win;
using Sci.Data;
using System.Linq;

namespace Sci.Production.Sewing
{
    public partial class P05 : Sci.Win.Tems.Input6
    {
        public P05(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = $"Factoryid = '{Env.User.Factory}'";
        }

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
sd.UnitPrice,
tms.SewingCPU,
tms.CuttingCPU,
tms.InspectionCPU,
tms.OtherCPU,
tms.OtherAmt,
tms.EMBAmt,
tms.PrintingAmt,
tms.OtherPrice,
tms.EMBPrice,
tms.PrintingPrice
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
where sd.SubConOutFty = '{subConOutFty}' and sd.ContractNumber = '{contractNumber}'
";
            this.DetailSelectCommand = cmd;
            return base.OnDetailSelectCommandPrepare(e);
        }

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
            #endregion
            return base.ClickSaveBefore();
        }

        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["factoryid"] = Env.User.Factory;
            this.CurrentMaintain["Status"] = "New";
        }

        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            if (this.CurrentMaintain["Status"].Equals("Confirm"))
            {
                this.txtSubConOutFty.SetReadOnly(true);
                this.txtContractnumber.ReadOnly = true;
                this.dateIssuedate.ReadOnly = true;
            }
        }

        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            string updConfirm = $"update dbo.SubconOutContract set Status = 'Confirm', ApvName = '{Env.User.UserID}' ,ApvDate = getdate() where SubConOutFty = '{this.CurrentMaintain["SubConOutFty"]}' and ContractNumber = '{this.CurrentMaintain["ContractNumber"]}'";
            DualResult result = DBProxy.Current.Execute(null, updConfirm);
            if (!result)
            {
                this.ShowErr(result);
            }
        }

        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            this.refresh.PerformClick();
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

        protected override bool ClickDeleteBefore()
        {
            if (this.CurrentMaintain["Status"].Equals("Confirm"))
            {
                MyUtility.Msg.WarningBox("This Data already confirm, can't delete");
                return false;
            }
            return base.ClickDeleteBefore();
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.txtuser1.SetReadOnly(true);
            this.label10.Text = this.CurrentMaintain["Status"].ToString();
        }

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
where   o.FtyGroup = '{this.CurrentMaintain["factoryid"]}'
        and o.ID = '{e.FormattedValue}'
		and o.Category != 'G'
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
                result = this.GetTmsData(e.FormattedValue.ToString());
                if (result == false)
                {
                    e.Cancel = true;
                    return;
                }

                result = this.GetComboType();
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

            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("OrderId", header: "SP#", width: Widths.AnsiChars(15), settings: orderIdSet)
                .Text("StyleID", header: "Style", width: Widths.AnsiChars(18), iseditingreadonly: true)
                .Text("ComboType", header: "ComboType", width: Widths.AnsiChars(5), settings: comboTypeSet)
                .Text("Article", header: "Article", width: Widths.AnsiChars(8), settings: articleSet)
                .Numeric("QrderQty", header: "Order Qty", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("OutputQty", header: "Output Qty", width: Widths.AnsiChars(10), settings: outputQtySet)
                .Numeric("AccuOutputQty", header: "Accu. Output Qty", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("UnitPrice", header: "Price(Unit)", width: Widths.AnsiChars(10),integer_places: 12, decimal_places: 4)
                .Numeric("SewingCPU", header: "Sewing CPU", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("CuttingCPU", header: "Cutting CPU", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("InspectionCPU", header: "Inspection CPU", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("OtherCPU", header: "Other CPU", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("OtherAmt", header: "Other Amt", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("EMBAmt", header: "EMB Amt", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("PrintingAmt", header: "Printing Amt", width: Widths.AnsiChars(10), iseditingreadonly: true);

            this.detailgrid.AllowUserToDeleteRows = false;
            this.detailgrid.RowSelecting += (s, e) =>
            {
                DataRow curDr = ((DataTable)this.detailgridbs.DataSource).Rows[e.RowIndex];
                if (this.EditMode && this.CurrentMaintain["Status"].Equals("Confirm"))
                {
                    foreach (DataGridViewColumn item in this.detailgrid.Columns)
                    {
                        item.ReadOnly = true;
                    }

                    if (curDr.RowState == DataRowState.Modified || curDr.RowState == DataRowState.Unchanged)
                    {
                        this.detailgrid.Columns["OutputQty"].ReadOnly = false;
                        if ((int)curDr["AccuOutputQty"] > 0)
                        {
                            this.detailgrid.AllowUserToDeleteRows = false;
                        }
                        else
                        {
                            this.detailgrid.AllowUserToDeleteRows = true;
                        }
                    }
                    else
                    {
                        this.detailgrid.Columns["OrderId"].ReadOnly = false;
                        this.detailgrid.Columns["ComboType"].ReadOnly = false;
                        this.detailgrid.Columns["Article"].ReadOnly = false;
                        this.detailgrid.Columns["OutputQty"].ReadOnly = false;
                        this.detailgrid.Columns["UnitPrice"].ReadOnly = false;
                    }
                }
                else
                {
                    this.detailgrid.Columns["OrderId"].ReadOnly = false;
                    this.detailgrid.Columns["ComboType"].ReadOnly = false;
                    this.detailgrid.Columns["Article"].ReadOnly = false;
                    this.detailgrid.Columns["OutputQty"].ReadOnly = false;
                    this.detailgrid.Columns["UnitPrice"].ReadOnly = false;
                }
            };

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
                Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(checkLocation, "Location,Rate", "10,15", null, null);
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
where   o.FtyGroup = '{this.CurrentMaintain["factoryid"]}'
		and o.Category != 'G'
        and f.IsProduceFty = 1 and o.Junk = 0 order by o.AddDate desc
";
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(orderChkSql, "18,15,12,8", null, null);
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
                result = this.GetTmsData(this.CurrentDetailData["OrderId"].ToString());
                if (result == false)
                {
                    return;
                }

                result = this.GetComboType();
                if (result == false)
                {
                    return;
                }

                this.GetAccuOutputQty();
            }
        }

        private DualResult GetTmsData(string orderID)
        {
            DualResult result = new DualResult(true);
            DataRow resultDr;
            string chkQrderSql = $@"
select
o.StyleID,
o.StyleUkey,
tms.SewingCPU,
tms.CuttingCPU,
tms.InspectionCPU,
tms.OtherCPU,
tms.OtherPrice,
tms.EMBPrice,
tms.PrintingPrice
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
where   o.FtyGroup = '{this.CurrentMaintain["factoryid"]}'
        and o.ID = '{orderID}'
		and o.Category != 'G'
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
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(chkArticleSql, "10,15", null, null);
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
