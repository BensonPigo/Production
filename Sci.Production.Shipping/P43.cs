using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;
using System.Transactions;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// P43
    /// </summary>
    public partial class P43 : Win.Tems.Input6
    {
        /// <summary>
        /// P43
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P43(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : MyUtility.Convert.GetString(e.Master["ID"]);
            this.DetailSelectCommand = string.Format(
                @"
select 
    agd.id,
	agd.OrderId,
	agd.Article,
	agd.SizeCode,
	agd.Qty,
	BalanceQty=0,
	agd.ReasonID,
	sr.Description
from AdjustGMT_Detail agd with (nolock)
left join ShippingReason sr with (nolock) on sr.id = agd.ReasonID
where agd.id = '{0}'",
                masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        protected override void OnDetailEntered()
        {
            foreach (DataRow dr in this.DetailDatas)
            {
                this.BalanceQty(dr);
            }

            base.OnDetailEntered();
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();

            #region orderID
            DataGridViewGeneratorTextColumnSettings orderID = new DataGridViewGeneratorTextColumnSettings();
            orderID.CellValidating += (s, e) =>
            {
                if (!this.CellValidatingContinue(s, e))
                {
                    return;
                }

                if (MyUtility.Convert.GetString(e.FormattedValue) == MyUtility.Convert.GetString(this.CurrentDetailData["OrderID"]))
                {
                    return;
                }

                string sqlCmd = $@"select 1 from orders o with (nolock),Factory f  with (nolock) where o.id = '{e.FormattedValue}' and o.FactoryID = f.id and f.IsProduceFty = 1";
                if (!MyUtility.Check.Seek(sqlCmd))
                {
                    MyUtility.Msg.WarningBox("Data not found!!");
                    this.CurrentDetailData["OrderID"] = string.Empty;
                }
                else
                {
                    this.CurrentDetailData["OrderID"] = e.FormattedValue;
                }

                this.CurrentDetailData["Article"] = string.Empty;
                this.CurrentDetailData["SizeCode"] = string.Empty;
                this.CurrentDetailData["Qty"] = 0;
                this.CurrentDetailData["BalanceQty"] = 0;
                this.CurrentDetailData["ReasonID"] = string.Empty;
                this.CurrentDetailData["Description"] = string.Empty;
                this.CurrentDetailData.EndEdit();
            };
            #endregion

            #region Article
            DataGridViewGeneratorTextColumnSettings article = new DataGridViewGeneratorTextColumnSettings();
            article.EditingMouseDown += (s, e) =>
            {
                if (!this.EditingMouseDownContinue(s, e))
                {
                    return;
                }

                if (e.Button == MouseButtons.Right)
                {
                    string sqlCmd = $"Select distinct a.Article from Order_Qty a with (nolock) where a.id = '{this.CurrentDetailData["orderid"]}'";
                    Win.Tools.SelectItem item = new Win.Tools.SelectItem(sqlCmd, "10,10", this.CurrentDetailData["Article"].ToString());
                    DialogResult returnResult = item.ShowDialog();
                    if (returnResult == DialogResult.Cancel)
                    {
                        return;
                    }

                    if (item.GetSelecteds().Count > 0)
                    {
                        if (MyUtility.Convert.GetString(item.GetSelecteds()[0]["Article"]) != MyUtility.Convert.GetString(this.CurrentDetailData["Article"]))
                        {
                            this.CurrentDetailData["Article"] = item.GetSelecteds()[0]["Article"];
                            this.CurrentDetailData["SizeCode"] = string.Empty;
                            this.CurrentDetailData["Qty"] = 0;
                            this.CurrentDetailData["BalanceQty"] = 0;
                            this.CurrentDetailData["ReasonID"] = string.Empty;
                            this.CurrentDetailData["Description"] = string.Empty;
                            this.CurrentDetailData.EndEdit();
                        }
                    }
                }
            };

            article.CellValidating += (s, e) =>
            {
                if (!this.CellValidatingContinue(s, e))
                {
                    return;
                }

                if (MyUtility.Convert.GetString(e.FormattedValue) == MyUtility.Convert.GetString(this.CurrentDetailData["Article"]))
                {
                    return;
                }

                string sqlCmd = $"Select distinct a.Article from Order_Qty a with (nolock) where a.id = '{this.CurrentDetailData["orderid"]}' and  a.Article = '{e.FormattedValue}'";
                if (!MyUtility.Check.Seek(sqlCmd))
                {
                    MyUtility.Msg.WarningBox("Data not found!!");
                    this.CurrentDetailData["Article"] = string.Empty;
                }
                else
                {
                    this.CurrentDetailData["Article"] = e.FormattedValue;
                }

                this.CurrentDetailData["SizeCode"] = string.Empty;
                this.CurrentDetailData["Qty"] = 0;
                this.CurrentDetailData["BalanceQty"] = 0;
                this.CurrentDetailData["ReasonID"] = string.Empty;
                this.CurrentDetailData["Description"] = string.Empty;
                this.CurrentDetailData.EndEdit();
            };
            #endregion

            #region Size
            DataGridViewGeneratorTextColumnSettings size = new DataGridViewGeneratorTextColumnSettings();
            size.EditingMouseDown += (s, e) =>
            {
                if (!this.EditingMouseDownContinue(s, e))
                {
                    return;
                }

                if (e.Button == MouseButtons.Right)
                {
                    string sqlCmd = $"Select distinct a.SizeCode from Order_Qty a with (nolock) where a.id = '{this.CurrentDetailData["orderid"]}' and  a.Article = '{this.CurrentDetailData["Article"]}'";
                    Win.Tools.SelectItem item = new Win.Tools.SelectItem(sqlCmd, "10,10", this.CurrentDetailData["SizeCode"].ToString());
                    DialogResult returnResult = item.ShowDialog();
                    if (returnResult == DialogResult.Cancel)
                    {
                        return;
                    }

                    if (item.GetSelecteds().Count > 0)
                    {
                        if (MyUtility.Convert.GetString(item.GetSelecteds()[0]["SizeCode"]) != MyUtility.Convert.GetString(this.CurrentDetailData["SizeCode"]))
                        {
                            this.CurrentDetailData["SizeCode"] = item.GetSelecteds()[0]["SizeCode"];
                            this.CurrentDetailData.EndEdit();
                        }
                    }

                    this.BalanceQty();
                }
            };

            size.CellValidating += (s, e) =>
            {
                if (!this.CellValidatingContinue(s, e))
                {
                    return;
                }

                if (MyUtility.Convert.GetString(e.FormattedValue) == MyUtility.Convert.GetString(this.CurrentDetailData["SizeCode"]))
                {
                    return;
                }

                string sqlCmd = $"Select distinct a.SizeCode from Order_Qty a with (nolock) where a.id = '{this.CurrentDetailData["orderid"]}' and  a.Article = '{this.CurrentDetailData["Article"]}' and a.SizeCode = '{e.FormattedValue}'";
                if (!MyUtility.Check.Seek(sqlCmd))
                {
                    MyUtility.Msg.WarningBox("Data not found!!");
                    this.CurrentDetailData["SizeCode"] = string.Empty;
                }
                else
                {
                    this.CurrentDetailData["SizeCode"] = e.FormattedValue;
                }

                this.CurrentDetailData.EndEdit();
                this.BalanceQty();
            };
            #endregion

            #region qty
            DataGridViewGeneratorNumericColumnSettings qty = new DataGridViewGeneratorNumericColumnSettings();
            qty.CellValidating += (s, e) =>
            {
                this.CurrentDetailData["qty"] = e.FormattedValue;
                this.CurrentDetailData.EndEdit();
                this.BalanceQty();
            };

            #endregion

            #region Reason ID
            DataGridViewGeneratorTextColumnSettings reason = new DataGridViewGeneratorTextColumnSettings();
            reason.EditingMouseDown += (s, e) =>
            {
                if (!this.EditingMouseDownContinue(s, e))
                {
                    return;
                }

                if (e.Button == MouseButtons.Right)
                {
                    string sqlCmd = "Select distinct ID, Description from ShippingReason a with (nolock) WHERE Type='AG' AND junk = 0";
                    Win.Tools.SelectItem item = new Win.Tools.SelectItem(sqlCmd, "10,20", this.CurrentDetailData["ReasonID"].ToString());
                    DialogResult returnResult = item.ShowDialog();
                    if (returnResult == DialogResult.Cancel)
                    {
                        return;
                    }

                    if (item.GetSelecteds().Count > 0)
                    {
                        if (MyUtility.Convert.GetString(item.GetSelecteds()[0]["ID"]) != MyUtility.Convert.GetString(this.CurrentDetailData["ReasonID"]))
                        {
                            this.CurrentDetailData["ReasonID"] = item.GetSelecteds()[0]["ID"];
                            this.CurrentDetailData["Description"] = item.GetSelecteds()[0]["Description"];
                            this.CurrentDetailData.EndEdit();
                        }
                    }
                }
            };

            reason.CellValidating += (s, e) =>
            {
                if (!this.CellValidatingContinue(s, e))
                {
                    return;
                }

                if (MyUtility.Convert.GetString(e.FormattedValue) == MyUtility.Convert.GetString(this.CurrentDetailData["ReasonID"]))
                {
                    return;
                }

                string sqlCmd = $"Select distinct ID, Description from ShippingReason a with (nolock) WHERE Type='AG' AND junk = 0 and id = '{e.FormattedValue}'";
                DataRow reasondr;
                if (!MyUtility.Check.Seek(sqlCmd, out reasondr))
                {
                    MyUtility.Msg.WarningBox("Data not found!!");
                    this.CurrentDetailData["ReasonID"] = string.Empty;
                    this.CurrentDetailData["Description"] = string.Empty;
                }
                else
                {
                    this.CurrentDetailData["ReasonID"] = e.FormattedValue;
                    this.CurrentDetailData["Description"] = reasondr["Description"];
                }

                this.CurrentDetailData.EndEdit();
            };
            #endregion

            this.Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("OrderId", header: "SP#", width: Widths.AnsiChars(16), settings: orderID)
            .Text("Article", header: "Color Way", width: Widths.AnsiChars(8), settings: article)
            .Text("SizeCode", header: "Size", width: Widths.AnsiChars(8), settings: size)
            .Numeric("Qty", header: "Deduct Qty", decimal_places: 0, minimum: 0, settings: qty)
            .Numeric("BalanceQty", header: "Balance Qty", decimal_places: 0, iseditingreadonly: true)
            .Text("ReasonID", header: "Reason ID", width: Widths.AnsiChars(8), settings: reason)
            .Text("Description", header: "Reason Name", width: Widths.AnsiChars(1), iseditingreadonly: true);
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["issuedate"] = DateTime.Now;
            this.CurrentMaintain["MDivisionID"] = Env.User.Keyword;
        }

        /// <inheritdoc/>
        protected override bool ClickEditBefore()
        {
            if (this.CurrentMaintain["Status"].EqualString("CONFIRMED"))
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't modify.", "Warning");
                return false;
            }

            return base.ClickEditBefore();
        }

        protected override void ClickSaveAfter()
        {
            base.ClickSaveAfter();
            this.OnDetailEntered();
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            #region 表身需有資料
            if (this.DetailDatas.Count == 0)
            {
                MyUtility.Msg.WarningBox("< Detail > can't be empty!");
                return false;
            }
            #endregion

            #region 檢查所有明細資料的Balance Qty是否>=0
            StringBuilder balanemsg = new StringBuilder();
            balanemsg.Append("Balacne Qty is not enough!! Please check again.\r\n");
            bool flag = false;
            foreach (DataRow dr in this.DetailDatas)
            {
                if (MyUtility.Convert.GetDecimal(dr["BalanceQty"]) < 0)
                {
                    balanemsg.Append($@"SP#: {dr["OrderId"]} Color Way: {dr["Article"]} Size: {dr["SizeCode"]} Balance: {dr["BalanceQty"]}" + Environment.NewLine);
                    flag = true;
                }
            }

            if (flag)
            {
                MyUtility.Msg.WarningBox(balanemsg.ToString());
                return false;
            }
            #endregion

            #region 檢查所有明細資料的Balance Qty是否>=0
            StringBuilder deductqty = new StringBuilder();
            deductqty.Append("Deduct Qty can't enter 0!! Please check again.\r\n");
            flag = false;
            foreach (DataRow dr in this.DetailDatas)
            {
                if (MyUtility.Convert.GetDecimal(dr["Qty"]) <= 0)
                {
                    deductqty.Append($@"SP#: {dr["OrderId"]} Color Way: {dr["Article"]} Size: {dr["SizeCode"]} " + Environment.NewLine);
                    flag = true;
                }
            }

            if (flag)
            {
                MyUtility.Msg.WarningBox(deductqty.ToString());
                return false;
            }
            #endregion

            #region 檢查所有明細資料都有填入Reason
            StringBuilder reasonmsg = new StringBuilder();
            flag = false;
            foreach (DataRow dr in this.DetailDatas)
            {
                if (MyUtility.Check.Empty(dr["ReasonID"]))
                {
                    reasonmsg.Append($@"SP#: {dr["OrderId"]} Color Way: {dr["Article"]} Size: {dr["SizeCode"]}" + Environment.NewLine);
                    flag = true;
                }
            }

            reasonmsg.Append("Reason can’t be empty!!");
            if (flag)
            {
                MyUtility.Msg.WarningBox(reasonmsg.ToString());
                return false;
            }
            #endregion

            #region GetID
            if (this.IsDetailInserting)
            {
                string id = MyUtility.GetValue.GetID(Env.User.Keyword + "AG", " AdjustGMT ", (DateTime)MyUtility.Convert.GetDate(this.CurrentMaintain["issuedate"]), 2, "Id", null);
                if (MyUtility.Check.Empty(id))
                {
                    MyUtility.Msg.WarningBox("GetID fail, please try again!");
                    return false;
                }

                this.CurrentMaintain["ID"] = id;
            }
            #endregion

            this.CurrentMaintain["Status"] = "New";
            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override void ClickConfirm()
        {
            #region 檢查所有明細資料的Balance Qty是否>=0
            StringBuilder balanemsg = new StringBuilder();
            balanemsg.Append("Balacne Qty is not enough!! Please check again.\r\n");
            bool flag = false;
            foreach (DataRow dr in this.DetailDatas)
            {
                if (MyUtility.Convert.GetDecimal(dr["BalanceQty"]) < 0)
                {
                    balanemsg.Append($@"SP#: {dr["OrderId"]} Color Way: {dr["Article"]} Size: {dr["SizeCode"]} Balance: {dr["BalanceQty"]}" + Environment.NewLine);
                    flag = true;
                }
            }

            if (flag)
            {
                MyUtility.Msg.WarningBox(balanemsg.ToString());
                return;
            }
            #endregion
            base.ClickConfirm();
            #region 更新表頭status
            string sql_updata_status = string.Format(
                @"
update AdjustGMT 
set status = 'Confirmed'
    , editname = '{0}' 
    , editdate = GETDATE()
where id = '{1}'",
                Env.User.UserID,
                this.CurrentMaintain["id"]);
            #endregion

            using (TransactionScope scope = new TransactionScope())
            {
                DualResult upResult;
                if (!(upResult = DBProxy.Current.Execute(null, sql_updata_status)))
                {
                    this.ShowErr(upResult);
                    return;
                }

                scope.Complete();
            }
        }

        /// <inheritdoc/>
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            #region 更新表頭status
            string sql_updata_status = string.Format(
                @"
update AdjustGMT  
set status = 'New'
    , editname = '{0}' 
    , editdate = GETDATE()
where id = '{1}'",
                Env.User.UserID,
                this.CurrentMaintain["id"]);
            #endregion

            using (TransactionScope scope = new TransactionScope())
            {
                DualResult upResult;
                if (!(upResult = DBProxy.Current.Execute(null, sql_updata_status)))
                {
                    this.ShowErr(upResult);
                    return;
                }

                scope.Complete();
            }
        }

        protected override bool ClickDeleteBefore()
        {
            if (this.CurrentMaintain["Status"].EqualString("CONFIRMED"))
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't delete.", "Warning");
                return false;
            }

            return base.ClickDeleteBefore();
        }

        private void BalanceQty(DataRow dr = null)
        {
            dr = dr == null ? this.CurrentDetailData : dr;
            string balanceQtysql = $@"
select balanceQty = 
    isnull(sum(dbo.getMinCompleteSewQty('{dr["OrderId"]}', '{dr["Article"]}','{dr["SizeCode"]}')),0)
    -
    isnull(sum(dbo.getMinCompleteGMTQty('{dr["OrderId"]}', '{dr["Article"]}','{dr["SizeCode"]}')),0)
	-
	isnull((select sum(isnull(ShipQty,0) + isnull(iq.DiffQty,0))
	from Pullout_Detail_Detail pdd with(nolock) 
	left join InvAdjust i with(nolock) on i.OrderID = pdd.OrderId
	left join InvAdjust_Qty iq with(nolock) on iq.Article = pdd.Article and iq.SizeCode = pdd.SizeCode and i.id = iq.id
	where pdd.OrderId = '{dr["OrderId"]}' and pdd.Article = '{dr["Article"]}' and pdd.SizeCode = '{dr["SizeCode"]}'
	),0)
    -
    ({dr["qty"]}+isnull((select sum(qty) from AdjustGMT a with(nolock),AdjustGMT_detail ad with(nolock) 
    where a.id = ad.id and a.Status = 'Confirmed' and a.id !='{dr["id"]}'  and  ad.OrderId = '{dr["OrderId"]}' and ad.Article = '{dr["Article"]}' and ad.SizeCode = '{dr["SizeCode"]}'),0))
";
            dr["BalanceQty"] = MyUtility.Convert.GetInt(MyUtility.GetValue.Lookup(balanceQtysql));
            dr.EndEdit();
        }

        private bool CellValidatingContinue(object s, Ict.Win.UI.DataGridViewCellValidatingEventArgs e)
        {
            if (!this.EditMode)
            {
                return false;
            }

            if (e.RowIndex < 0)
            {
                return false;
            }

            return true;
        }

        private bool EditingMouseDownContinue(object s, Ict.Win.UI.DataGridViewEditingControlMouseEventArgs e)
        {
            if (!this.EditMode)
            {
                return false;
            }

            if (e.RowIndex < 0)
            {
                return false;
            }

            return true;
        }
    }
}
