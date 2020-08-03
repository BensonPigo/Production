using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Ict;
using Ict.Win;
using Sci;
using Sci.Data;
using Sci.Production;

using Sci.Production.PublicPrg;
using System.Linq;
using System.Transactions;
using System.Data.SqlClient;
using Sci.Win;
using System.Reflection;

namespace Sci.Production.Subcon
{
    /// <summary>
    /// P10
    /// </summary>
    public partial class P10 : Sci.Win.Tems.Input6
    {
        /// <summary>
        /// P10
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P10(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = "Mdivisionid = '" + Sci.Env.User.Keyword + "'";
            this.gridicon.Append.Enabled = false;
            this.gridicon.Append.Visible = false;
            this.gridicon.Insert.Enabled = false;
            this.gridicon.Insert.Visible = false;

            this.txtsubconSupplier.TextBox1.Validated += (s, e) =>
            {
                if (this.EditMode && this.txtsubconSupplier.TextBox1.Text != this.txtsubconSupplier.TextBox1.OldValue)
                {
                    this.CurrentMaintain["CurrencyID"] = MyUtility.GetValue.Lookup("CurrencyID", this.txtsubconSupplier.TextBox1.Text, "LocalSupp", "ID");
                    this.CurrentMaintain["Paytermid"] = MyUtility.GetValue.Lookup("paytermid", this.txtsubconSupplier.TextBox1.Text, "LocalSupp", "ID");
                    ((DataTable)this.detailgridbs.DataSource).Rows.Clear();  // 清空表身資料
                }
            };

            this.displayBox1.BackColor = Color.Yellow;
        }

        private void Txtartworktype_ftyArtworkType_Validated(object sender, EventArgs e)
        {
            Production.Class.Txtartworktype_fty o;
            o = (Production.Class.Txtartworktype_fty)sender;

            if ((o.Text != o.OldValue) && this.EditMode)
            {
                ((DataTable)this.detailgridbs.DataSource).Rows.Clear();  // 清空表身資料
                string artworkunit = MyUtility.GetValue.Lookup(string.Format("select artworkunit from artworktype WITH (NOLOCK) where id='{0}'", o.Text));
                if (artworkunit == string.Empty)
                {
                    artworkunit = "PCS";
                }

                this.detailgrid.Columns[3].HeaderText = artworkunit;
            }
        }

        // 新增時預設資料

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["Mdivisionid"] = Sci.Env.User.Keyword;
            this.CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;
            this.CurrentMaintain["ISSUEDATE"] = System.DateTime.Today;
            this.CurrentMaintain["HANDLE"] = Sci.Env.User.UserID;
            this.CurrentMaintain["VatRate"] = 0;
            this.CurrentMaintain["Status"] = "New";
            ((DataTable)this.detailgridbs.DataSource).Rows[0].Delete();
        }

        // delete前檢查

        /// <inheritdoc/>
        protected override bool ClickDeleteBefore()
        {
            if (this.CurrentMaintain["Status"].ToString().ToUpper() == "APPROVED")
            {
                MyUtility.Msg.WarningBox("Data is approved, can't delete.", "Warning");
                return false;
            }

            return base.ClickDeleteBefore();
        }

        // edit前檢查

        /// <inheritdoc/>
        protected override bool ClickEditBefore()
        {
            if (this.CurrentMaintain["status"].ToString() == "Approved")
            {
                var frm = new Sci.Production.PublicForm.EditRemark("artworkap", "remark", this.CurrentMaintain);
                frm.ShowDialog(this);
                this.RenewData();
                return false;
            }

            return base.ClickEditBefore();
        }

        // edit後，更新detail的farm in跟accu. ap qty

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();

            // foreach (DataRow dr in DetailDatas)
            // {
            //    var v = MyUtility.GetValue.Lookup(string.Format("select farmin from artworkpo_detail where ukey = '{0}'", dr["artworkpo_detailukey"].ToString()));
            //    decimal accQty;
            //    Decimal.TryParse(v, out accQty);
            //    dr["Farmin"] = accQty;
            //    var v2 = MyUtility.GetValue.Lookup(string.Format("select apqty from artworkpo_detail where ukey = '{0}'", dr["artworkpo_detailukey"].ToString()));
            //    decimal accQty2;
            //    Decimal.TryParse(v2, out accQty2);
            //    dr["accumulatedqty"] = accQty2;
            //    //無此資料行且結果必=0
            //    //dr["balance"] = (decimal)dr["Farmin"] - (decimal)dr["accumulatedqty"];
            // }
        }

        // detail 新增時設定預設值

        /// <inheritdoc/>
        protected override void OnDetailGridInsert(int index = -1)
        {
            base.OnDetailGridInsert(index);
            this.CurrentDetailData["apqty"] = 0;
        }

        // save前檢查 & 取id

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            #region 必輸檢查
            if (this.CurrentMaintain["LocalSuppID"] == DBNull.Value || string.IsNullOrWhiteSpace(this.CurrentMaintain["LocalSuppID"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Suppiler >  can't be empty!", "Warning");
                this.txtsubconSupplier.TextBox1.Focus();
                return false;
            }

            if (this.CurrentMaintain["issuedate"] == DBNull.Value || string.IsNullOrWhiteSpace(this.CurrentMaintain["issuedate"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Issue Date >  can't be empty!", "Warning");
                this.dateIssueDate.Focus();
                return false;
            }

            if (this.CurrentMaintain["ArtworktypeId"] == DBNull.Value || string.IsNullOrWhiteSpace(this.CurrentMaintain["ArtworktypeId"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Artwork Type >  can't be empty!", "Warning");
                this.txtartworktype_ftyArtworkType.Focus();
                return false;
            }

            if (this.CurrentMaintain["CurrencyID"] == DBNull.Value || string.IsNullOrWhiteSpace(this.CurrentMaintain["CurrencyID"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Currency >  can't be empty!", "Warning");
                return false;
            }

            if (this.CurrentMaintain["Handle"] == DBNull.Value || string.IsNullOrWhiteSpace(this.CurrentMaintain["Handle"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Handle >  can't be empty!", "Warning");
                this.txtuserHandle.TextBox1.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["factoryid"]))
            {
                MyUtility.Msg.WarningBox("< Factory Id >  can't be empty!", "Warning");
                this.txtmfactory.Focus();
                return false;
            }

            if (this.CurrentMaintain["PayTermid"] == DBNull.Value || string.IsNullOrWhiteSpace(this.CurrentMaintain["PayTermid"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Terms >  can't be empty!", "Warning");
                this.txtpayterm_ftyTerms.Focus();
                return false;
            }
            #endregion

            #region 檢查LocalSupp_Bank
            DualResult resultCheckLocalSupp_BankStatus = Prgs.CheckLocalSupp_BankStatus(this.CurrentMaintain["localsuppid"].ToString(), Prgs.CallFormAction.Save);
            if (!resultCheckLocalSupp_BankStatus)
            {
                return false;
            }
            #endregion

            if (this.DetailDatas.Count == 0)
            {
                MyUtility.Msg.WarningBox("Detail can't be empty", "Warning");
                return false;
            }

            #region 表身的來源subconp01單號是否CONFIRM。
            string chkp01 =
                @"
select distinct ap.id
from ArtworkPO ap with(nolock) 
inner join #tmp t on t.artworkpoid = ap.id
where  ap.status = 'New'
";
            DataTable dt;
            DualResult result;
            if (result = MyUtility.Tool.ProcessWithDatatable((DataTable)this.detailgridbs.DataSource, "Artworkpoid", chkp01, out dt))
            {
                if (dt.Rows.Count > 0)
                {
                    StringBuilder chkp01comfirmed = new StringBuilder();
                    foreach (DataRow dr in dt.Rows)
                    {
                        chkp01comfirmed.Append(string.Format("Please confirm [Subcon][P01]:{0} first !!\r\n", dr["id"]));
                    }

                    MyUtility.Msg.WarningBox(chkp01comfirmed.ToString());
                    return false;
                }
            }
            else
            {
                MyUtility.Msg.ErrorBox(result.ToString());
                return false;
            }

            #endregion

            // 取單號：
            if (this.IsDetailInserting)
            {
                string factorykeyword = Sci.MyUtility.GetValue.Lookup(string.Format("select keyword from dbo.factory WITH (NOLOCK) where ID ='{0}'", this.CurrentMaintain["factoryid"]));
                if (MyUtility.Check.Empty(factorykeyword))
                {
                    MyUtility.Msg.WarningBox("Factory Keyword is empty, Please contact to MIS!!");
                    return false;
                }

                this.CurrentMaintain["id"] = Sci.MyUtility.GetValue.GetID(Sci.Env.User.Keyword + "FA", "artworkAP", (DateTime)this.CurrentMaintain["issuedate"]);
                if (MyUtility.Check.Empty(this.CurrentMaintain["id"]))
                {
                    MyUtility.Msg.WarningBox("Server is busy, Please re-try it again", "GetID() Failed");
                    return false;
                }
            }

            #region 加總明細金額至表頭
            string str = MyUtility.GetValue.Lookup(string.Format("Select exact from Currency WITH (NOLOCK) where id = '{0}'", this.CurrentMaintain["currencyId"]), null);
            if (str == null || string.IsNullOrWhiteSpace(str))
            {
                MyUtility.Msg.WarningBox(string.Format("<{0}> is not found in Currency Basic Data , can't save!", this.CurrentMaintain["currencyID"]), "Warning");
                return false;
            }

            int exact = int.Parse(str);
            object detail_a = ((DataTable)this.detailgridbs.DataSource).Compute("sum(amount)", string.Empty);
            this.CurrentMaintain["amount"] = MyUtility.Math.Round((decimal)detail_a, exact);
            this.CurrentMaintain["vat"] = MyUtility.Math.Round((decimal)detail_a * (decimal)this.CurrentMaintain["vatrate"] / 100, exact);
            #endregion

            return base.ClickSaveBefore();
        }

        // 組表身資料

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();
            string artworkTypeID = (e.Master == null) ? string.Empty : e.Master["ArtworkTypeID"].ToString();
            string cmdsql = string.Format(
                @"
select a.* 
, PoQty=isnull(b.PoQty,0)
, [balance]=isnull(b.PoQty,0) - a.AccumulatedQty
,[LocalSuppCtn]=LocalSuppCtn.Val
,b.Article
,b.SizeCode
from ArtworkAP_detail a
left join artworkpo_detail b on a.ArtworkPo_DetailUkey=b.Ukey
OUTER APPLY(
	SELECT [Val]= COUNT(LocalSuppID)
	FROM (
		SELECT DISTINCT apo.LocalSuppID 
		from ArtworkPO_Detail ad
		inner join ArtworkPO apo on apo.id = ad.id
		where ad.OrderID= b.OrderID
		and ad.PatternCode=b.PatternCode
		and ad.PatternDesc =b.PatternDesc
		and apo.ArtworkTypeID = 'PRINTING'
	)tmp
)LocalSuppCtn
where a.id='{0}'
", masterID,
                artworkTypeID);
            this.DetailSelectCommand = cmdsql;
            return base.OnDetailSelectCommandPrepare(e);
        }

        private void AddBalance()
        {
            DataTable details = (DataTable)this.detailgridbs.DataSource;
            if (details.Columns.Contains("balance"))
            {
                return;
            }

            if (!this.tabs.TabPages[0].Equals(this.tabs.SelectedTab))
            {
                details.Columns.Add("poqty", typeof(decimal));
                details.Columns.Add("balance", typeof(decimal));
                decimal poqty;
                foreach (DataRow dr in details.Rows)
                {
                    poqty = 0m;
                    decimal.TryParse(MyUtility.GetValue.Lookup(string.Format("select poqty from artworkpo_detail WITH (NOLOCK) where ukey = {0}", (long)dr["artworkpo_detailukey"])), out poqty);
                    dr["poqty"] = poqty;
                    dr["balance"] = (decimal)dr["farmin"] - (decimal)dr["accumulatedqty"];
                }
            }
        }

        // refresh

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            string artworkunit = MyUtility.GetValue.Lookup(string.Format("select artworkunit from artworktype WITH (NOLOCK) where id='{0}'", this.CurrentMaintain["artworktypeid"])).ToString().Trim();
            if (artworkunit == string.Empty)
            {
                artworkunit = "PCS";
            }

            this.detailgrid.Columns[3].HeaderText = artworkunit;
            if (!(this.CurrentMaintain == null))
            {
                if (!(this.CurrentMaintain["amount"] == DBNull.Value) && !(this.CurrentMaintain["vat"] == DBNull.Value))
                {
                    decimal amount = (decimal)this.CurrentMaintain["amount"] + (decimal)this.CurrentMaintain["vat"];
                    this.numTotal.Text = amount.ToString();
                }
            }

            this.txtsubconSupplier.Enabled = !this.EditMode || this.IsDetailInserting;
            this.txtartworktype_ftyArtworkType.Enabled = !this.EditMode || this.IsDetailInserting;
            this.txtpayterm_ftyTerms.Enabled = !this.EditMode || this.IsDetailInserting;
            this.txtmfactory.Enabled = !this.EditMode || this.IsDetailInserting;
            this.dateApprovedDate.ReadOnly = true;

            this.disExVoucherID.Text = this.CurrentMaintain["ExVoucherID"].ToString();
            #region Status Label
            this.label25.Text = this.CurrentMaintain["status"].ToString();
            #endregion

            #region Batch Import, Special record button
            this.btnImportFromPO.Enabled = this.EditMode;

            #endregion

            this.txtuserAccountant.TextBox1.ReadOnly = true;
            this.txtuserAccountant.TextBox1.IsSupportEditMode = false;

            for (int i = 0; i < this.detailgrid.Rows.Count; i++)
            {
                if ((int)this.detailgrid.Rows[i].Cells["LocalSuppCtn"].Value >= 2)
                {
                    this.detailgrid.Rows[i].Cells["FarmOut"].Style.BackColor = Color.Yellow;
                    this.detailgrid.Rows[i].Cells["farmin"].Style.BackColor = Color.Yellow;
                }
            }
        }

        // Detail Grid 設定 & Detail Vaild

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            #region qtygarment Valid
            Ict.Win.DataGridViewGeneratorNumericColumnSettings ns2 = new DataGridViewGeneratorNumericColumnSettings();
            ns2.CellValidating += (s, e) =>
            {
                if (this.EditMode && e.FormattedValue != null)
                {
                    this.AddBalance();
                    if ((decimal)e.FormattedValue > (decimal)this.CurrentDetailData["balance"] ||
                        (decimal)e.FormattedValue + (decimal)this.CurrentDetailData["accumulatedqty"] > (decimal)this.CurrentDetailData["PoQty"])
                    {
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox("can't over balance and can't over poqty", "Warning");
                        return;
                    }

                    this.CurrentDetailData["amount"] = (decimal)e.FormattedValue * (decimal)this.CurrentDetailData["price"];
                    this.CurrentDetailData["apqty"] = e.FormattedValue;
                }
            };
            #endregion

            #region 欄位設定
            this.Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("Artworkpoid", header: "Artwork PO", width: Widths.AnsiChars(13), iseditingreadonly: true) // 0
            .Text("orderid", header: "SP#", width: Widths.AnsiChars(15), iseditingreadonly: true) // 1
            .Text("Article", header: "Article", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("ArtworkId", header: "Artwork", width: Widths.AnsiChars(8), iseditingreadonly: true) // 2
            .Text("SizeCode", header: "Size", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Numeric("stitch", header: "PCS/Stitch", width: Widths.AnsiChars(5), iseditingreadonly: true) // 3
            .Text("patterncode", header: "CutpartID", width: Widths.AnsiChars(10), iseditingreadonly: true) // 4
            .Text("PatternDesc", header: "Cutpart Name", width: Widths.AnsiChars(15), iseditingreadonly: true) // 5
            .Numeric("price", header: "Price", width: Widths.AnsiChars(5), decimal_places: 4, integer_places: 4, iseditingreadonly: true) // 6
            .Numeric("PoQty", header: "PO Qty", width: Widths.AnsiChars(6), iseditingreadonly: true) // 7
            .Numeric("FarmOut", header: "Farm Out", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Numeric("farmin", header: "Farm In", width: Widths.AnsiChars(6), iseditingreadonly: true) // 8
            .Numeric("accumulatedqty", header: "Accu. Paid Qty", width: Widths.AnsiChars(6), iseditingreadonly: true) // 9
            .Numeric("balance", header: "Balance", width: Widths.AnsiChars(6), iseditingreadonly: true) // 10
            .Numeric("apqty", header: "Qty", width: Widths.AnsiChars(6), settings: ns2) // 11
            .Numeric("amount", header: "Amount", width: Widths.AnsiChars(12), iseditingreadonly: true, decimal_places: 2, integer_places: 14)
            .Numeric("LocalSuppCtn", header: "LocalSuppCtn", width: Widths.AnsiChars(0));  // 12

            #endregion
            #region 可編輯欄位變色
            this.detailgrid.Columns["apqty"].DefaultCellStyle.BackColor = Color.Pink; // qty
            #endregion
            this.detailgrid.Columns["LocalSuppCtn"].Visible = false;
        }

        // Approve

        /// <inheritdoc/>
        protected override void ClickConfirm()
        {
            var zerolist = ((DataTable)this.detailgridbs.DataSource).AsEnumerable().Where(w => MyUtility.Convert.GetDecimal(w["apqty"]) == 0)
                .Select(s => new
                {
                    ArtworkPO = MyUtility.Convert.GetString(s["Artworkpoid"]),
                    SP = MyUtility.Convert.GetString(s["orderid"]),
                    Artwork = MyUtility.Convert.GetString(s["ArtworkId"]),
                    Stitch = MyUtility.Convert.GetDecimal(s["stitch"]),
                    CutpartID = MyUtility.Convert.GetString(s["patterncode"]),
                    CutpartName = MyUtility.Convert.GetString(s["PatternDesc"]),
                    Price = MyUtility.Convert.GetDecimal(s["Price"]),
                    POQty = MyUtility.Convert.GetDecimal(s["PoQty"]),
                    FarmOut = MyUtility.Convert.GetDecimal(s["FarmOut"]),
                    FarmIn = MyUtility.Convert.GetDecimal(s["farmin"]),
                    AccuPaidQty = MyUtility.Convert.GetDecimal(s["accumulatedqty"]),
                    Balance = MyUtility.Convert.GetDecimal(s["Balance"]),
                    Qty = MyUtility.Convert.GetDecimal(s["apqty"]),
                    Amount = MyUtility.Convert.GetDecimal(s["amount"]),
                })
                .ToList();
            if (zerolist.Count > 0)
            {
                string msg = @"The following AP qty cannot be 0!!";
                DataTable dt = this.ToDataTable(zerolist);
                MyUtility.Msg.ShowMsgGrid(dt, msg: msg, caption: "Warning");

                return;
            }

            #region 檢查LocalSupp_Bank
            DualResult resultCheckLocalSupp_BankStatus = Prgs.CheckLocalSupp_BankStatus(this.CurrentMaintain["localsuppid"].ToString(), Prgs.CallFormAction.Confirm);
            if (!resultCheckLocalSupp_BankStatus)
            {
                return;
            }
            #endregion

            var dr = this.CurrentMaintain;
            if (dr == null)
            {
                return;
            }

            string sqlcmd, sqlupd2 = string.Empty, sqlupd3 = string.Empty, ids = string.Empty;
            string sqlupfromAP = string.Empty;
            DualResult result, result2;
            DataTable datacheck;

            #region 檢查表身每一筆資料的ApQty，加上其他已經Approve的ApQty，是否有超過PoQty

            foreach (DataRow detailRow in this.DetailDatas)
            {
                string artworkPo_DetailUkey = detailRow["ArtworkPo_DetailUkey"].ToString();
                string orderID = detailRow["orderID"].ToString();

                // 表身的APQTY
                int currentApQty = MyUtility.Convert.GetInt(detailRow["ApQty"]);

                // 取得已Approve的ApQTY、PoQty
                string chkCmd = $@"
SELECT  [OtherApvApQty]=SUM(ISNULL(aad.apQty,0))
        ,[PoQty] = ISNULL(ArtworkPO_Detail.PoQty,0)
FROM ArtworkAP aa with(nolock)
INNER JOIN ArtworkAP_detail aad with(nolock) ON aad.id = aa.id
OUTER APPLY(
	SELECT PoQty
	FROM ArtworkPO_Detail apd  with(nolock)
	WHERE apd.ID = aad.ArtworkPoID 
	AND aad.ArtworkPo_DetailUkey = apd.Ukey
)ArtworkPO_Detail 
WHERE aa.Status='Approved' AND aad.ArtworkPo_DetailUkey = {artworkPo_DetailUkey}
GROUP BY ArtworkPO_Detail.PoQty
";
                result = DBProxy.Current.Select(null, chkCmd, out datacheck);

                if (!result)
                {
                    this.ShowErr(result);
                }

                if (datacheck.Rows != null && datacheck.Rows.Count > 0)
                {
                    int otherApvApQty = MyUtility.Convert.GetInt(datacheck.Rows[0]["OtherApvApQty"]);
                    int poQty = MyUtility.Convert.GetInt(datacheck.Rows[0]["PoQty"]);

                    if ((otherApvApQty + currentApQty) > poQty)
                    {
                        MyUtility.Msg.InfoBox($"SP#: {orderID} ,Total AP Qty can not more than PO Qty.");
                        return;
                    }
                }
            }
            #endregion

            #region 須檢核其來源單[P10]狀態為CONFIRM。
            string check_p10status = string.Format(
                @"
select distinct ap.id
from ArtworkAP aa with(nolock)
inner join ArtworkAP_detail aad with(nolock) on aad.id = aa.id
inner join ArtworkPO ap with(nolock)on ap.id = aad.ArtworkPoid
where ap.status = 'New' and aa.Id ='{0}'",
                this.CurrentMaintain["id"]);
            DataTable chktb;
            if (result = DBProxy.Current.Select(null, check_p10status, out chktb))
            {
                if (chktb.Rows.Count > 0)
                {
                    string p10id = string.Empty;
                    foreach (DataRow drr in chktb.Rows)
                    {
                        p10id += drr["id"].ToString();
                    }

                    string chkp10msg = string.Format("Please confirm [Subcon][P01]:{0} first !!", p10id);
                    MyUtility.Msg.WarningBox(chkp10msg);
                    return;
                }
            }
            else
            {
                MyUtility.Msg.ErrorBox(result.ToString());
                return;
            }
            #endregion

            #region 檢查po是否close了。
            sqlcmd = string.Format(
                @"select a.id from artworkpo a WITH (NOLOCK) , artworkap_detail b WITH (NOLOCK) 
                            where a.id = b.artworkpoid and a.closed = 1 and b.id = '{0}'", this.CurrentMaintain["id"]);
            if (!(result = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                this.ShowErr(sqlcmd, result);
            }

            if (datacheck.Rows.Count > 0)
            {
                foreach (DataRow drchk in datacheck.Rows)
                {
                    ids += drchk[0].ToString() + ",";
                }

                MyUtility.Msg.WarningBox(string.Format("These POID <{0}> already closed, can't Approve it", ids));
                return;
            }
            #endregion
            #region 檢查exact
            string str = MyUtility.GetValue.Lookup(string.Format("Select exact from Currency WITH (NOLOCK) where id = '{0}'", this.CurrentMaintain["currencyId"]), null);
            if (str == null || string.IsNullOrWhiteSpace(str))
            {
                MyUtility.Msg.WarningBox(string.Format("<{0}> is not found in Currency Basic Data , can't approved!", this.CurrentMaintain["currencyID"]));
                return;
            }
            #endregion

            #region 開始更新相關table資料
            sqlupd3 = string.Format(
                "update artworkap set status='Approved', apvname='{0}', apvdate = GETDATE() , editname = '{0}' , editdate = GETDATE() " +
                                "where id = '{1}'", Env.User.UserID,
                this.CurrentMaintain["id"]);

            #region 從PO更新price資訊(實體欄位) + 加總明細金額至表頭

            sqlupfromAP = $@"
update aad set
	Price=apd.Price,
	Stitch=apd.Stitch,
	--Farmin=apd.Farmin,
	PatternCode=apd.PatternCode,
	PatternDesc=apd.PatternDesc,
    Amount = apd.Price*aad.ApQty
from ArtworkPO_detail apd with(nolock)
inner join ArtworkAP_detail aad with(nolock) on apd.id = aad.artworkpoid and aad.artworkpo_detailukey = apd.ukey
where aad.id = '{this.CurrentMaintain["ID"]}'

declare @exact int = (Select exact from Currency WITH (NOLOCK) where id = '{this.CurrentMaintain["currencyId"]}')
declare @sumAmount numeric(14, 4) = (select sum(amount) from ArtworkAP_detail where id = '{this.CurrentMaintain["ID"]}')
declare @Amount numeric(14, 4) = (select ROUND(@sumAmount, @exact))
declare @Vat numeric(11, 2) = ROUND(@sumAmount * (select VatRate from ArtworkAP where id = '{this.CurrentMaintain["ID"]}') / 100,@exact)
update ArtworkAP set amount = @Amount, vat = @Vat  where ID = '{this.CurrentMaintain["ID"]}';
";
            #endregion

            foreach (DataRow drchk in this.DetailDatas)
            {
                sqlcmd = string.Format(
                    @"select b.artworkpo_detailukey, sum(b.apqty) qty
                                from artworkap a WITH (NOLOCK) , artworkap_detail b WITH (NOLOCK) 
                                where a.id = b.id  and a.status = 'Approved' and b.artworkpo_detailukey ='{0}'
                                group by b.artworkpo_detailukey ", drchk["artworkpo_detailukey"]);

                if (!(result = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
                {
                    this.ShowErr(sqlcmd, result);
                    return;
                }

                if (datacheck.Rows.Count > 0)
                {
                    sqlupd2 += string.Format(
                        "update artworkpo_detail set apqty = {0} where ukey = '{1}';"
                        + Environment.NewLine, (decimal)datacheck.Rows[0]["qty"] + (decimal)drchk["apqty"],
                        drchk["artworkpo_detailukey"]);
                }
                else
                {
                    sqlupd2 += string.Format(
                        "update artworkpo_detail set apqty = {0} where ukey = '{1}';"
                        + Environment.NewLine, (decimal)drchk["apqty"],
                        drchk["artworkpo_detailukey"]);
                }
            }

            TransactionScope transactionscope = new TransactionScope();
            using (transactionscope)
            {
                try
                {
                    if (!(result = DBProxy.Current.Execute(null, sqlupd3)))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(sqlupd3, result);
                        return;
                    }

                    if (!(result2 = DBProxy.Current.Execute(null, sqlupfromAP)))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(result2);
                        return;
                    }

                    if (!(result2 = DBProxy.Current.Execute(null, sqlupd2)))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(sqlupd2, result2);
                        return;
                    }

                    transactionscope.Complete();
                    transactionscope.Dispose();
                    MyUtility.Msg.InfoBox("Approve successful");
                }
                catch (Exception ex)
                {
                    transactionscope.Dispose();
                    this.ShowErr("Commit transaction error.", ex);
                    return;
                }
            }

            transactionscope.Dispose();
            transactionscope = null;

            #endregion
            base.ClickConfirm();
        }

        // unApprove

        /// <inheritdoc/>
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            DialogResult dResult = MyUtility.Msg.QuestionBox("Do you want to unapprove it?", "Question", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2);
            if (dResult.ToString().ToUpper() == "NO")
            {
                return;
            }

            var dr = this.CurrentMaintain;
            if (dr == null)
            {
                return;
            }

            string sqlcmd, sqlupd2 = string.Empty, sqlupd3 = string.Empty, ids = string.Empty;
            DualResult result, result2;
            DataTable datacheck;
            #region 檢查po是否close了。
            sqlcmd = string.Format(
                @"select a.id from artworkpo a WITH (NOLOCK) , artworkap_detail b WITH (NOLOCK) 
                            where a.id = b.artworkpoid and a.closed = 1 and b.id = '{0}'", this.CurrentMaintain["id"]);
            if (!(result = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                this.ShowErr(sqlcmd, result);
            }

            if (datacheck.Rows.Count > 0)
            {
                foreach (DataRow drchk in datacheck.Rows)
                {
                    ids += drchk[0].ToString() + ",";
                }

                MyUtility.Msg.WarningBox(string.Format("These POID <{0}> already closed, can't UnApprove it", ids));
                return;
            }
            #endregion

            #region 開始更新相關table資料

            sqlupd3 = string.Format(
                "update artworkap set status='New',apvname='', apvdate = null , editname = '{0}' , editdate = GETDATE() " +
                               "where id = '{1}'", Env.User.UserID,
                this.CurrentMaintain["id"]);

            foreach (DataRow drchk in this.DetailDatas)
            {
                sqlcmd = string.Format(
                    @"select b.artworkpo_detailukey, sum(b.apqty) qty
                                from artworkap a WITH (NOLOCK) , artworkap_detail b WITH (NOLOCK) 
                                where a.id = b.id  and a.status ='Approved' and b.artworkpo_detailukey ='{0}'
                                group by b.artworkpo_detailukey ", drchk["artworkpo_detailukey"]);

                if (!(result = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
                {
                    this.ShowErr(sqlcmd, result);
                    return;
                }

                if (datacheck.Rows.Count > 0)
                {
                    sqlupd2 += string.Format(
                        "update artworkpo_detail set apqty = {0} where ukey = '{1}';"
                            + Environment.NewLine, (decimal)datacheck.Rows[0]["qty"] - (decimal)drchk["apqty"],
                        drchk["artworkpo_detailukey"]);
                }
                else
                {
                    sqlupd2 += string.Format(
                        "update artworkpo_detail set apqty = {0} where ukey = '{1}';"
                            + Environment.NewLine, 0m,
                        drchk["artworkpo_detailukey"]);
                }
            }

            TransactionScope transactionscope = new TransactionScope();
            using (transactionscope)
            {
                try
                {
                    if (!(result = DBProxy.Current.Execute(null, sqlupd3)))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(sqlupd3, result);
                        return;
                    }

                    if (!(result2 = DBProxy.Current.Execute(null, sqlupd2)))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(sqlupd2, result2);
                        return;
                    }

                    transactionscope.Complete();
                    transactionscope.Dispose();
                    MyUtility.Msg.InfoBox("UnApprove successful");
                }
                catch (Exception ex)
                {
                    transactionscope.Dispose();
                    this.ShowErr("Commit transaction error.", ex);
                    return;
                }
            }

            transactionscope.Dispose();
            transactionscope = null;

            #endregion
        }

        // P10_ImportFromPO
        private void BtnImportFromPO_Click(object sender, EventArgs e)
        {
            var dr = this.CurrentMaintain;
            if (dr == null)
            {
                return;
            }

            if (MyUtility.Check.Empty(dr["localsuppid"]))
            {
                MyUtility.Msg.WarningBox("Please fill Supplier first!");
                this.txtsubconSupplier.TextBox1.Focus();
                return;
            }

            if (MyUtility.Check.Empty(dr["artworktypeid"]))
            {
                MyUtility.Msg.WarningBox("Please fill Artworktype first!");
                this.txtartworktype_ftyArtworkType.Focus();
                return;
            }

            var frm = new Sci.Production.Subcon.P10_ImportFromPO(dr, (DataTable)this.detailgridbs.DataSource);
            frm.ShowDialog(this);
            this.RenewData();
        }

        // print

        /// <inheritdoc/>
        protected override bool ClickPrint()
        {
            DataRow row = this.CurrentMaintain;
            string id = row["ID"].ToString();
            string issuedate = ((DateTime)MyUtility.Convert.GetDate(row["issuedate"])).ToShortDateString();
            string invoice = row["invno"].ToString();
            string remarks = row["Remark"].ToString();

            #region -- 撈表頭資料 --
            List<SqlParameter> pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));
            DualResult result;
            ReportDefinition report = new ReportDefinition();
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Invoice", invoice));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("ID", id));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Remarks", remarks));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Issuedate", issuedate));

            #endregion
            #region -- 撈表身資料 --
            pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));
            DataTable dtDetail;
            string sqlcmd = @"
select F.nameEn
,AddressEN = REPLACE(REPLACE(F.AddressEN,Char(13),''),Char(10),'')
,F.Tel
,ap.LocalSuppID+'-'+L.name AS Supplier
,L.Address
,L.tel
,ap.ID
,A.ArtworkPoID
,A.OrderID
,A.ArtworkId
,A.PatternDesc
,A.Price
,A.ApQty
,format(A.Amount,'#,###,###,##0.00')Amount
,ap.PayTermID+'-'+P.name as Terms
,LocalSuppBank.AccountNo
,LocalSuppBank.AccountName
,LocalSuppBank.BankName
,LocalSuppBank.CountryID+'/'+LocalSuppBank.City as Country
,LocalSuppBank.SWIFTCode
,ap.Handle+CHAR(13)+CHAR(10)+pas.name as PreparedBy
,format(ap.Amount,'#,###,###,##0.00') as Total
,format(ap.Vat,'#,###,###,##0.00') as Vat
,format(ap.Amount+ap.Vat,'#,###,###,##0.00') as GrandTotal
,ap.currencyid as Currency
from DBO.artworkap ap WITH (NOLOCK) 
LEFT JOIN dbo.factory F WITH (NOLOCK) 
    ON  F.ID = ap.factoryid
LEFT JOIN dbo.LocalSupp L WITH (NOLOCK) 
    ON  L.ID = ap.LocalSuppID
LEFT JOIN dbo.Artworkap_Detail A WITH (NOLOCK) 
    ON  A.ID = ap.ID
LEFT JOIN DBO.PayTerm P WITH (NOLOCK) 
    ON P.ID = ap.PayTermID
LEFT JOIN DBO.Pass1 pas WITH (NOLOCK) 
    ON pas.ID = ap.Handle 
OUTER APPLY(
	SELECT   [AccountNo]= IIF(lb.ByCheck=1,'',lbd.Accountno )
	, [AccountName]=IIF(lb.ByCheck=1,'',lbd.AccountName )
	, [BankName]=IIF(lb.ByCheck=1,'',lbd.BankName )
	, [CountryID]=IIF(lb.ByCheck=1,'',lbd.CountryID)
	, [City]=IIF(lb.ByCheck=1,'',lbd.City)
	, [SwiftCode]=IIF(lb.ByCheck=1,'',lbd.SwiftCode )
	FROM LocalSupp_Bank lb
	LEFT JOIN LocalSupp_Bank_Detail lbd ON lb.ID=lbd.ID AND lb.PKey=lbd.Pkey
	WHERE lb.ID=ap.LocalSuppID
	AND lb.ApproveDate = (SElECT MAX(ApproveDate) FROM LocalSupp_Bank WHERE Status='Confirmed' AND ID=ap.LocalSuppID)
	AND lbd.IsDefault=1
)LocalSuppBank 
where ap.ID= @ID";
            result = DBProxy.Current.Select(string.Empty, sqlcmd, pars, out dtDetail);
            if (!result)
            {
                this.ShowErr(sqlcmd, result);
            }

            string rptTitle = dtDetail.Rows[0]["nameEn"].ToString();
            string addressEN = dtDetail.Rows[0]["AddressEN"].ToString();
            string tEL = dtDetail.Rows[0]["Tel"].ToString();
            string supplier = dtDetail.Rows[0]["Supplier"].ToString();
            string address = dtDetail.Rows[0]["Address"].ToString();
            string lTEL = dtDetail.Rows[0]["tel"].ToString();
            string barcode = dtDetail.Rows[0]["ID"].ToString();
            string barcodeView = dtDetail.Rows[0]["ID"].ToString();
            string terms = dtDetail.Rows[0]["Terms"].ToString();
            string aCNO = dtDetail.Rows[0]["AccountNo"].ToString();
            string aCNAME = dtDetail.Rows[0]["AccountName"].ToString();
            string bankName = dtDetail.Rows[0]["BankName"].ToString();
            string country = dtDetail.Rows[0]["Country"].ToString();
            string sWIFCode = dtDetail.Rows[0]["SWIFTCode"].ToString();
            string preparedBy = dtDetail.Rows[0]["PreparedBy"].ToString();
            string total = dtDetail.Rows[0]["Total"].ToString();
            string vAT = dtDetail.Rows[0]["Vat"].ToString();
            string grandTotal = dtDetail.Rows[0]["GrandTotal"].ToString();
            string currency = dtDetail.Rows[0]["Currency"].ToString();

            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("RptTitle", rptTitle));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("TEL", tEL));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Supplier", supplier));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Address", address));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("LTEL", lTEL));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Barcode", barcode));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("BarcodeView", barcodeView));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Terms", terms));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("ACNO", aCNO));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("ACNAME", aCNAME));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("BankName", bankName));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Country", country));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("SWIFCode", sWIFCode));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("PreparedBy", preparedBy));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Total", total));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("VAT", vAT));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("GrandTotal", grandTotal));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Currency", currency));

            if (!addressEN.EndsWith(Environment.NewLine))
            {
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("AddressEN", addressEN + Environment.NewLine));
            }
            else
            {
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("AddressEN", addressEN));
            }

            // 傳 list 資料
            List<P10_PrintData> data = dtDetail.AsEnumerable()
                .Select(row1 => new P10_PrintData()
                {
                    POID = row1["ArtworkPoID"].ToString(),
                    OrderID = row1["OrderID"].ToString(),
                    Pattem = row1["ArtworkId"].ToString(),
                    CutPart = row1["PatternDesc"].ToString(),
                    Price = row1["Price"].ToString(),
                    Qty = row1["ApQty"].ToString(),
                    Amt = row1["Amount"].ToString(),
                }).ToList();

            report.ReportDataSource = data;
            #endregion

            // 指定是哪個 RDLC
            // DualResult result;
            Type reportResourceNamespace = typeof(P10_PrintData);
            Assembly reportResourceAssembly = reportResourceNamespace.Assembly;
            string reportResourceName = "P10_Print.rdlc";

            IReportResource reportresource;
            if (!(result = ReportResources.ByEmbeddedResource(reportResourceAssembly, reportResourceNamespace, reportResourceName, out reportresource)))
            {
                // this.ShowException(result);
                return false;
            }

            report.ReportResource = reportresource;

            // 開啟 report view
            var frm = new Sci.Win.Subs.ReportView(report);
            frm.MdiParent = this.MdiParent;
            frm.Show();

            return true;
        }

        private void BtnRemoveQty0_Click(object sender, EventArgs e)
        {
            for (int i = ((DataTable)this.detailgridbs.DataSource).Rows.Count - 1; i >= 0; i--)
            {
                if (MyUtility.Convert.GetDecimal(((DataTable)this.detailgridbs.DataSource).Rows[i]["apqty"]) == 0)
                {
                    ((DataTable)this.detailgridbs.DataSource).Rows[i].Delete();
                }
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
        /// IsNullable
        /// </summary>
        /// <param name="t">t</param>
        /// <returns>bool</returns>
        public static bool IsNullable(Type t)
        {
            return !t.IsValueType || (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>));
        }

        /// <summary>
        /// GetCoreType
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
    }
}
