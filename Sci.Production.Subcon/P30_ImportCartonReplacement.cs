using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Prg;

namespace Sci.Production.Subcon
{
    /// <inheritdoc/>
    public partial class P30_ImportCartonReplacement : Win.Subs.Base
    {
        private DataRow dr_localPO;
        private DataTable dt_localPODetail;
        private DataTable dtlocal;

        /// <inheritdoc/>
        public P30_ImportCartonReplacement(DataRow master, DataTable detail)
        {
            this.InitializeComponent();
            this.dr_localPO = master;
            this.dt_localPODetail = detail;
            this.Text += $" ( Categgory:{this.dr_localPO["category"]} - Supplier:{this.dr_localPO["localsuppid"]} )";
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.gridImport.IsEditingReadOnly = false; // 必設定, 否則CheckBox會顯示圖示
            this.Helper.Controls.Grid.Generator(this.gridImport)
            .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
            .Text("POID", header: "Master SP#", iseditingreadonly: true)
            .Text("OrderID", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(13))
            .Text("StyleID", header: "Style", iseditingreadonly: true, width: Widths.AnsiChars(13))
            .Text("SeasonID", header: "Season", iseditingreadonly: true, width: Widths.AnsiChars(13))
            .Text("Refno", header: "Refno", iseditingreadonly: true)
            .Text("Description", header: "Description", iseditingreadonly: true)
            .Numeric("Qty", header: "Qty", iseditingreadonly: true)
            .Text("UnitID", header: "Unit", iseditingreadonly: true)
            .Numeric("Price", header: "Price", iseditable: true, decimal_places: 4, integer_places: 4, iseditingreadonly: true)
            .Numeric("Amount", header: "Amount", iseditable: true, decimal_places: 4, integer_places: 4, iseditingreadonly: true)
            .Text("Remark", header: "Remark", iseditingreadonly: true)
            .Text("ReplacementLocalItemID", header: "Carton Replacement#", iseditingreadonly: true)
            .Text("BuyerID", header: "Buyer", iseditingreadonly: true)
            ;

            Color backDefaultColor = this.gridImport.DefaultCellStyle.BackColor;
            this.gridImport.RowPrePaint += (s, e) =>
            {
                if (e.RowIndex < 0)
                {
                    return;
                }

                DataRow dr = ((DataRowView)this.gridImport.Rows[e.RowIndex].DataBoundItem).Row;
                #region 變色規則，若該 Row 已經變色則跳過
                if (MyUtility.Convert.GetDecimal(dr["qty"]) == 0)
                {
                    if (this.gridImport.Rows[e.RowIndex].DefaultCellStyle.BackColor != Color.FromArgb(217, 217, 217))
                    {
                        this.gridImport.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(217, 217, 217);
                    }
                }
                else
                {
                    if (this.gridImport.Rows[e.RowIndex].DefaultCellStyle.BackColor != backDefaultColor)
                    {
                        this.gridImport.Rows[e.RowIndex].DefaultCellStyle.BackColor = backDefaultColor;
                    }
                }
                #endregion
            };
        }

        private void BtnFindNow_Click(object sender, EventArgs e)
        {
            this.listControlBindingSource1.DataSource = null;
            if (MyUtility.Check.Empty(this.dateIssueDate.Value1) &&
                MyUtility.Check.Empty(this.dateApproveDate.Value1) &&
                MyUtility.Check.Empty(this.dateSCIDelivery.Value1) &&
                MyUtility.Check.Empty(this.dateSewingInlineDate.Value1))
            {
                MyUtility.Msg.WarningBox(@"< Issue Date > or < Sci Delivery > or < Sewing Inline > or < Approve Date > can't be empty!!");
                this.txtSPNoStart.Focus();
                return;
            }

            string where = string.Empty;
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            if (!MyUtility.Check.Empty(this.dateIssueDate.Value1))
            {
                where += "\r\nand rl.IssueDate between @IssueDate1 and @IssueDate2";
                sqlParameters.Add(new SqlParameter("@IssueDate1", this.dateIssueDate.Value1));
                sqlParameters.Add(new SqlParameter("@IssueDate2", this.dateIssueDate.Value2));
            }

            if (!MyUtility.Check.Empty(this.dateApproveDate.Value1))
            {
                where += "\r\nand cast(rl.ApvDate as date) between @ApvDate1 and @ApvDate2";
                sqlParameters.Add(new SqlParameter("@ApvDate1", this.dateApproveDate.Value1));
                sqlParameters.Add(new SqlParameter("@ApvDate2", this.dateApproveDate.Value2));
            }

            if (!MyUtility.Check.Empty(this.dateSCIDelivery.Value1))
            {
                where += "\r\nand o.SciDelivery between @SciDelivery1 and @SciDelivery2";
                sqlParameters.Add(new SqlParameter("@SciDelivery1", this.dateSCIDelivery.Value1));
                sqlParameters.Add(new SqlParameter("@SciDelivery2", this.dateSCIDelivery.Value2));
            }

            if (!MyUtility.Check.Empty(this.dateSewingInlineDate.Value1))
            {
                where += "\r\nand o.SewInLine between @SewInLine1 and @SewInLine2";
                sqlParameters.Add(new SqlParameter("@SewInLine1", this.dateSewingInlineDate.Value1));
                sqlParameters.Add(new SqlParameter("@SewInLine2", this.dateSewingInlineDate.Value2));
            }

            if (!MyUtility.Check.Empty(this.txtSPNoStart.Text))
            {
                where += "\r\nand rl.OrderID >= @SP1";
                sqlParameters.Add(new SqlParameter("@SP1", this.txtSPNoStart.Text));
            }

            if (!MyUtility.Check.Empty(this.txtSPNoEnd.Text))
            {
                where += "\r\nand rl.OrderID <= @SP2";
                sqlParameters.Add(new SqlParameter("@SP2", this.txtSPNoEnd.Text));
            }

            if (!MyUtility.Check.Empty(this.txtfactory1.Text))
            {
                where += "\r\nand rl.FactoryID=@FactoryID";
                sqlParameters.Add(new SqlParameter("@FactoryID", this.txtfactory1.Text));
            }

            if (!MyUtility.Check.Empty(this.txtbrand.Text))
            {
                where += "\r\nand o.BrandID = @BrandID";
                sqlParameters.Add(new SqlParameter("@BrandID", this.txtbrand.Text));
            }

            string sqlcmd = $@"
select
    Selected = cast(1 as bit),
    rl.POID,
    rl.OrderID,
    o.StyleID,
    o.SeasonID,
    rld.Refno,
    Description = dbo.getitemdesc('Carton', rld.Refno),
    Qty = rld.RequestQty,
    l.UnitID,
    l.Price,
    Amount = isnull(rld.RequestQty * l.Price, 0),
    rld.Remark,
    ReplacementLocalItemID = rl.id,
    b.BuyerID,
    ReasonID = rld.ReplacementLocalItemReasonID,
    Reason = rlr.description,

    o.SciDelivery,
    [std_price] = round(ot.Price,3),
    requestid = '',
    ThreadColorID = '',
    InQty = 0,
    APQty = 0,
    ID = '',
    rl.FactoryID,
    o.SewInLine,
    Delivery = NULL,
    OldSeq1='',
    OldSeq2=''

from ReplacementLocalItem rl WITH (NOLOCK)
inner join orders o WITH (NOLOCK) on o.id = rl.OrderID
inner join ReplacementLocalItem_Detail rld WITH (NOLOCK) on rld.ID = rl.ID
inner join LocalItem l WITH (NOLOCK) on l.RefNo = rld.RefNo
inner join Brand b WITH (NOLOCK) on o.BrandID = b.ID
inner join Factory f WITH (NOLOCK) on f.ID = rl.FactoryID
left join ReplacementLocalItemReason rlr on rlr.id = rld.ReplacementLocalItemReasonID AND rl.type = 'R'
left join Order_TmsCost ot WITH (NOLOCK) on ot.id = o.ID and ot.ArtworkTypeID = 'Carton'

where 1=1
and l.Category = 'Carton'
and rl.SubconName = '{this.dr_localPO["localsuppid"]}'
and rl.Status = 'Confirmed'
and f.IsProduceFty  = 1
and o.PulloutComplete  = 0
and o.Junk = 0
and o.MDivisionID = '{Sci.Env.User.Keyword}'
and not exists(select 1 from LocalPO_Detail lp where lp.ReplacementLocalItemID = rl.ID and lp.Refno = rld.Refno and lp.ReasonID = rld.ReplacementLocalItemReasonID and lp.Id <> '{this.dr_localPO["ID"]}')
-- 排除 this.dt_localPODetail 已經有的
and not exists(select 1 from #tmp lp where lp.ReplacementLocalItemID = rl.ID and lp.Refno = rld.Refno and lp.ReasonID = rld.ReplacementLocalItemReasonID)


{where}";

            DualResult result = MyUtility.Tool.ProcessWithDatatable(this.dt_localPODetail, string.Empty, sqlcmd, out this.dtlocal, paramters: sqlParameters);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            if (this.dtlocal.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found");
                return;
            }

            // 填入Remark
            foreach (DataRow dr in this.dtlocal.Rows)
            {
                if (MyUtility.Check.Seek($@"select category,Finished from orders WITH (NOLOCK) where id = '{dr["orderid"]}'", out DataRow drr))
                {
                    // Bulk 單, 檢查 Price =0
                    if (MyUtility.Convert.GetString(drr["category"]) == "B")
                    {
                        string sql = $@"select price from order_tmscost WITH (NOLOCK) where id='{dr["orderid"]}' and artworktypeid  ='Carton'";
                        decimal price = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup(sql));
                        if (MyUtility.Check.Empty(price))
                        {
                            dr["remark"] = "Price is 0, can not be transfered to local purchase!!";
                            dr["Selected"] = 0;
                        }
                    }

                    // 訂單已關單
                    if (MyUtility.Convert.GetBool(drr["Finished"]))
                    {
                        dr["remark"] += "  This orders already finished, can not be transfered to local purchase!!";
                        dr["Selected"] = 0;
                    }
                }

                // 訂單PulloutComplete
                var strCheckPulloutCompleteSql = $@"
SELECT 1
FROM ORDERS O
outer apply (
	select ShipQty= isnull(sum(ShipQty),0)  from Pullout_Detail where OrderID=O.ID
) pd
outer apply(
	select DiffQty= isnull(SUM(isnull(DiffQty ,0)),0) 
	from InvAdjust I
	left join InvAdjust_Qty IQ on I.ID=IQ.ID
	where OrderID=O.ID
) inv
WHERE O.ID='{dr["OrderID"]}' 
AND (O.Qty-pd.ShipQty-inv.DiffQty = 0)";

                if (MyUtility.Check.Seek(strCheckPulloutCompleteSql))
                {
                    dr["remark"] += "  This orders already PulloutComplete, can not be transfered to local purchase!!";
                    dr["Selected"] = 0;
                }

                // Replacement沒填原因
                if (MyUtility.Check.Empty(dr["ReasonID"]))
                {
                    dr["remark"] += "  lack of Replacement Reason";
                    dr["Selected"] = 0;
                }
            }

            this.listControlBindingSource1.DataSource = this.dtlocal;
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            this.gridImport.ValidateControl();
            DataTable dtImport = (DataTable)this.listControlBindingSource1.DataSource;
            if (MyUtility.Check.Empty(dtImport) || dtImport.Rows.Count == 0)
            {
                return;
            }

            DataRow[] dr2 = dtImport.Select("Selected = 1");
            if (dr2.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select rows first!");
                return;
            }

            foreach (DataRow tmp in dr2)
            {
                tmp["id"] = this.dr_localPO["id"];
                this.dt_localPODetail.ImportRowAdded(tmp);
            }

            // 採買 Carton 會額外製作一份資料採購天地板於另一張採購單
            DataTable tmpdt = dr2.CopyToDataTable();
            string sqlcmd = $@"
select  selected = 1
        , POID
        , OrderID
        , StyleID
        , SciDelivery
        , SeasonID
        , Refno
        , Description
        , ThreadColorID
        , Qty = sum (isnull (Qty, 0))
        , UnitID
        , Price
        , Amount = sum (isnull (Amount, 0))
        , std_Price
        , remark
        , requestid
        , ID
        , FactoryID
        , SewInLine
		, Delivery
        , BuyerID
        , LocalSuppID
from (
    select  t.POID
            , t.OrderID
            , t.StyleID
            , t.SciDelivery
            , t.SeasonID
            , Refno=l.PadRefno
            , Description=dbo.getitemdesc('Carton', l.PadRefno)
            , ThreadColorID = ''
            , Qty = t.qty * l.qty
            , d.UnitID
            , d.Price
            , amount = t.qty * l.qty * d.Price
            , t.std_price
            , remark = ''
            , t.requestid
            , t.id
            , t.FactoryID
            , t.SewInLine
			, t.Delivery
            , t.BuyerID
            , d.LocalSuppid
    from #tmp t
    outer apply (
        select ct = count(1) 
        from LocalItem_CartonCardboardPad lc 
        where lc.Refno = t.RefNo and isnull (t.BuyerID,'') = isnull(lc.Buyer,'')
    )a
    inner join LocalItem_CartonCardboardPad l on t.RefNo = l.RefNo  and iif(a.ct>0, isnull(t.BuyerID,''),'') = isnull(l.Buyer,'')
    inner join LocalItem d WITH (NOLOCK) on l.PadRefno = d.RefNo and junk = 0 and category = 'CARTON'
) detailPads
group by POID, OrderID, StyleID, SciDelivery, SeasonID, Refno
        , Description, ThreadColorID, UnitID, Price, std_Price
        , remark,  requestid, ID, FactoryID, SewInLine, Delivery
        , BuyerID, LocalSuppID
";
            DualResult result = MyUtility.Tool.ProcessWithDatatable(tmpdt, string.Empty, sqlcmd, out DataTable cartonCardboardPad);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            // 複製DataTable結構，若已複製過則不執行，否則會清空資料
            if (P30.dtPadBoardInfo.Columns.Count == 0)
            {
                P30.dtPadBoardInfo = cartonCardboardPad.Clone();
            }

            foreach (DataRow tmp in cartonCardboardPad.Rows)
            {
                // 判斷紙箱、天地板供應商是否相同：相同寫回表身、不同則h存進P30.dtPadBoardInfo後續處理
                if (this.dr_localPO["localsuppid"].ToString() == tmp["localsuppid"].ToString())
                {
                    // 判斷 dtPadBoardInfo 是否已經存在相同的 『OrderID, Refno, ThreadColorID, RequestID』
                    DataRow[] findrow =
                    this.dt_localPODetail.Select(string.Format(
                        @"orderid = '{0}' and refno = '{1}' and threadcolorid = '{2}' and requestID = '{3}'",
                        tmp["orderid"].ToString(),
                        tmp["refno"].ToString(),
                        tmp["threadcolorid"].ToString(),
                        tmp["RequestID"].ToString()));

                    if (findrow.Length > 0)
                    {
                        // 已存在更新 Price 與 Qty
                        findrow[0]["Price"] = tmp["Price"];
                        findrow[0]["qty"] = tmp["qty"];
                    }
                    else
                    {
                        // 不存在則新增
                        tmp["id"] = this.dr_localPO["id"];
                        this.dt_localPODetail.ImportRowAdded(tmp);
                    }
                }
                else
                {
                    // 物料供應商與表頭不同，將資訊整理寫入 dtPadBoardInfo
                    DataRow[] findrow = P30.dtPadBoardInfo.Select(string.Format(
                        @"orderid = '{0}' and refno = '{1}' and threadcolorid = '{2}' and requestID = '{3}'",
                        tmp["orderid"].ToString(),
                        tmp["refno"].ToString(),
                        tmp["threadcolorid"].ToString(),
                        tmp["RequestID"].ToString()));

                    if (findrow.Length > 0)
                    {
                        // 已存在更新 Price 與 Qty
                        findrow[0]["Price"] = tmp["Price"];
                        findrow[0]["qty"] = tmp["qty"];
                    }
                    else
                    {
                        // 不存在則新增
                        tmp["id"] = this.dr_localPO["id"];
                        P30.dtPadBoardInfo.ImportRowAdded(tmp);
                    }
                }
            }

            this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
