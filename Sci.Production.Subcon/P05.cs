﻿using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.PublicPrg;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Sci.Production.Subcon
{
    /// <summary>
    /// P05
    /// </summary>
    public partial class P05 : Sci.Win.Tems.Input6
    {
        private string artworkunit;
        private Form batchapprove;

        /// <summary>
        /// P05
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P05(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = $"MdivisionID = '{Sci.Env.User.Keyword}'";
            this.gridicon.Append.Enabled = false;
            this.gridicon.Append.Visible = false;
            this.gridicon.Insert.Enabled = false;
            this.gridicon.Insert.Visible = false;

            this.txtsubconSupplier.TextBox1.Validated += (s, e) =>
            {
                this.CurrentMaintain["localsuppid"] = this.txtsubconSupplier.TextBox1.Text;
            };
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            #region 權限控管

            // 檢查是否擁有Confirm or Check權限
            bool canConfrim = Prgs.GetAuthority(Sci.Env.User.UserID, "P05. Sub-con Requisition", "CanConfirm");
            bool canCheck = Prgs.GetAuthority(Sci.Env.User.UserID, "P05. Sub-con Requisition", "CanCheck");

            if (canConfrim || canCheck || Env.User.IsAdmin)
            {
                this.btnBatchApprove.Enabled = true;
            }
            else
            {
                this.btnBatchApprove.Enabled = false;
            }
            #endregion
        }

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtsubconSupplier.TextBox1.ReadOnly = true;
            this.txtartworktype_ftyArtworkType.ReadOnly = true;
        }

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();

            this.DetailSelectCommand = string.Format(
                @"
select 
ap.OrderID
,o.StyleID
,ap.Article
,ap.SizeCode
,o.SewInLine
,o.SciDelivery
,ap.ArtworkID
,ap.PatternCode
,ap.PatternDesc
,ap.ReqQty
,ap.Stitch
,ap.QtyGarment
,[Farmout] = isnull(apo.Farmout,0)
,[Farmin] = isnull(apo.Farmin,0)
,[ApQty] = isnull(apo.ApQty,0)
,ap.ExceedQty
,ap.ArtworkPOID
,ap.id
,ap.ukey
,o.FactoryID
,f.IsProduceFty
,ap.Remark
from dbo.ArtworkReq_Detail ap with (nolock)
left join dbo.Orders o with (nolock) on ap.OrderID = o.id
left join Factory f with (nolock) on f.ID = o.FactoryID
left join dbo.ArtworkPO_Detail apo with (nolock) on apo.ArtworkReq_Detailukey = ap.ukey
where ap.id = '{0}'  
ORDER BY ap.OrderID   ", masterID);

            return base.OnDetailSelectCommandPrepare(e);
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.ChangeDetailHeader();

            if (this.CurrentMaintain["ID"] == DBNull.Value)
            {
                this.btnIrrQtyReason.Enabled = false;
            }
            else
            {
                this.btnIrrQtyReason.Enabled = true;
            }

            #region Status Label
            this.labStatus.Text = this.CurrentMaintain["Status"].ToString();
            #endregion
            #region exceed status
            this.labExceed.Visible = this.CurrentMaintain["Exceed"].ToString().ToUpper() == "TRUE";
            #endregion
            #region Batch Import, Special record button
            this.btnBatchImport.Enabled = this.EditMode;
            #endregion

            // #region Batch create
            // btnBatchCreate.Enabled = !this.EditMode;
            // #endregion
            #region Irregular 判斷
            this.RefreshIrregularQtyReason();
            #endregion

            #region 表尾
            string dateDeptApv = MyUtility.Check.Empty(MyUtility.Convert.GetDate(this.CurrentMaintain["DeptApvDate"])) ? string.Empty : ((DateTime)MyUtility.Convert.GetDate(this.CurrentMaintain["DeptApvDate"])).ToString("yyyy/MM/dd HH:mm:ss");

            this.dispDeptApv.Text = this.CurrentMaintain["DeptApvName"].ToString() + "-" + MyUtility.GetValue.Lookup("Name", this.CurrentMaintain["DeptApvName"].ToString(), "Pass1", "ID") + " " + dateDeptApv;

            string dateMgApv = MyUtility.Check.Empty(MyUtility.Convert.GetDate(this.CurrentMaintain["MgApvDate"])) ? string.Empty : ((DateTime)MyUtility.Convert.GetDate(this.CurrentMaintain["MgApvDate"])).ToString("yyyy/MM/dd HH:mm:ss");

            this.dispMgrApv.Text = this.CurrentMaintain["MgApvName"].ToString() + "-" + MyUtility.GetValue.Lookup("Name", this.CurrentMaintain["MgApvName"].ToString(), "Pass1", "ID") + " " + dateMgApv;

            string dateClose = MyUtility.Check.Empty(MyUtility.Convert.GetDate(this.CurrentMaintain["CloseUnCloseDate"])) ? string.Empty : ((DateTime)MyUtility.Convert.GetDate(this.CurrentMaintain["CloseUnCloseDate"])).ToString("yyyy/MM/dd HH:mm:ss");

            this.dispClosed.Text = this.CurrentMaintain["CloseUnCloseName"].ToString() + "-" + MyUtility.GetValue.Lookup("Name", this.CurrentMaintain["CloseUnCloseName"].ToString(), "Pass1", "ID") + " " + dateClose;
            #endregion
        }

        /// <inheritdoc/>
        protected override void EnsureToolbarExt()
        {
            base.EnsureToolbarExt();
            #region 動態ToolBar 參數

            if (!this.EditMode && this.CurrentMaintain != null)
            {
                // Close ChkValue
                if (string.Compare(this.CurrentMaintain["Status"].ToString(), "Closed", true) != 0 && !this.IsDetailInserting)
                {
                    this.toolbar.cmdClose.Enabled = true;
                }
                else
                {
                    this.toolbar.cmdClose.Enabled = false;
                }

                // unApv ChkValue
                if (string.Compare(this.CurrentMaintain["Status"].ToString(), "Approved", true) == 0 && !MyUtility.Check.Empty(this.CurrentMaintain["Exceed"]))
                {
                    this.toolbar.cmdUnconfirm.Enabled = true;
                }
                else
                {
                    this.toolbar.cmdUnconfirm.Enabled = false;
                }

                // unCheck ChkValue
                if ((string.Compare(this.CurrentMaintain["Status"].ToString(), "Locked", true) == 0 && !MyUtility.Check.Empty(this.CurrentMaintain["Exceed"]))
                    || (string.Compare(this.CurrentMaintain["Status"].ToString(), "Approved", true) == 0 && MyUtility.Check.Empty(this.CurrentMaintain["Exceed"])))
                {
                    this.toolbar.cmdUncheck.Enabled = true;
                }
                else
                {
                    this.toolbar.cmdUncheck.Enabled = false;
                }
            }
            #endregion
        }

        // Detail Grid 設定

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            Ict.Win.DataGridViewGeneratorNumericColumnSettings col_ReqQty = new DataGridViewGeneratorNumericColumnSettings();
            col_ReqQty.CellValidating += (s, e) =>
            {
                if (this.EditMode && e.FormattedValue != null)
                {
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    string sqlcmd = string.Empty;
                    if (MyUtility.Check.Empty(dr["PatternCode"]))
                    {
                        #region Special Record
                        sqlcmd = $@"
select  [OrderQty] = o.Qty,
        [AccReqQty] = isnull(ReqQty.value,0) + isnull(PoQty.value,0) + {e.FormattedValue}
from orders o with (nolock)
outer apply (
        select value = ISNULL(sum(ReqQty),0)
        from ArtworkReq_Detail AD, ArtworkReq a
        where ad.ID=a.ID
		and a.ArtworkTypeID = '{this.CurrentMaintain["artworktypeid"]}'
		and OrderID = o.ID 
        and ad.PatternCode= ''
        and ad.PatternDesc = ''
        and ad.ArtworkID = '{this.CurrentMaintain["artworktypeid"]}'
        and a.id != '{this.CurrentMaintain["id"]}'
        and a.status != 'Closed'
) ReqQty
outer apply (
        select value = ISNULL(sum(PoQty),0)
        from ArtworkPO_Detail AD,ArtworkPO A
        where a.ID=ad.ID
		and a.ArtworkTypeID = '{this.CurrentMaintain["artworktypeid"]}'
		and OrderID = o.ID 
        and ad.PatternCode= ''
        and ad.PatternDesc = ''
        and ad.ArtworkID = '{this.CurrentMaintain["artworktypeid"]}'
		and ad.ArtworkReqID=''
) PoQty
where o.ID = '{dr["OrderID"]}'
";
                        #endregion
                    }
                    else
                    {
                        #region 一般資料
                        sqlcmd = $@"
select  OrderQty = sum(oa.qty)  
, [AccReqQty] = isnull(ReqQty.value,0) + isnull(PoQty.value,0) + {e.FormattedValue}
from  Order_TmsCost ot
inner join orders o WITH (NOLOCK) on ot.ID = o.ID
cross apply(
	select * 
	from (		
		select a.id,a.ArtworkTypeID,q.Article,q.Qty,q.SizeCode,a.PatternCode,a.PatternDesc,a.ArtworkID,a.ArtworkName
		,rowNo = ROW_NUMBER() over (
			partition by a.id,a.ArtworkTypeID,q.Article,a.PatternCode,a.PatternDesc
				,a.ArtworkID,q.sizecode order by a.AddDate desc)
		from Order_Artwork a WITH (NOLOCK)
		inner join order_qty q WITH (NOLOCK) on q.id = a.ID and  (a.Article = q.Article or a.Article = '----')
		where a.id = o.id
		and a.ArtworkTypeID = '{this.CurrentMaintain["ArtworktypeId"]}'
		) s
	where rowNo = 1 
)oa
outer apply(
	select * 
	from (		
		select *
		,rowNo = ROW_NUMBER() over (
			partition by a.StyleUkey,a.ArtworkTypeID,a.Article,a.PatternCode,a.PatternDesc
				,a.ArtworkID,a.ArtworkName order by a.StyleArtworkUkey desc)
		from View_Style_Artwork a WITH (NOLOCK)
		where a.StyleUkey = o.StyleUkey
		and a.Article = oa.Article and a.ArtworkID = oa.ArtworkID 
		and a.ArtworkName = oa.ArtworkName and a.ArtworkTypeID = oa.ArtworkTypeID 
		and a.PatternCode = oa.PatternCode and a.PatternDesc = oa.PatternDesc 
		) s
	where rowNo = 1 
)vsa
left join Style_Artwork_Quot sao with (nolock) on sao.Ukey = vsa.StyleArtworkUkey and sao.PriceApv = 'Y' and sao.Price > 0
and sao.LocalSuppId = '{this.CurrentMaintain["localsuppid"]}' 
left join ArtworkType at WITH (NOLOCK) on at.id = oa.ArtworkTypeID
inner join factory f WITH (NOLOCK) on o.factoryid=f.id
outer apply (
        select value = ISNULL(sum(ReqQty),0)
        from ArtworkReq_Detail AD, ArtworkReq a
        where ad.ID=a.ID
		and a.ArtworkTypeID = '{this.CurrentMaintain["ArtworktypeId"]}' 
		and OrderID = o.ID and ad.PatternCode= isnull(oa.PatternCode,'')
        and ad.PatternDesc = isnull(oa.PatternDesc,'') 
        and ad.ArtworkID = iif(oa.ArtworkID is null,'{this.CurrentMaintain["ArtworktypeId"]}' ,oa.ArtworkID)
        and a.status != 'Closed' and ad.ArtworkPOID =''
        and a.id != '{dr["id"]}'
) ReqQty
outer apply (
        select value = ISNULL(sum(PoQty),0)
        from ArtworkPO_Detail AD,ArtworkPO A
        where a.ID=ad.ID
		and a.ArtworkTypeID = '{this.CurrentMaintain["ArtworktypeId"]}'
		and OrderID = o.ID 
        and ad.PatternCode= isnull(oa.PatternCode,'')
        and ad.PatternDesc = isnull(oa.PatternDesc,'') 
        and ad.ArtworkID = iif(oa.ArtworkID is null, '{this.CurrentMaintain["ArtworktypeId"]}' , oa.ArtworkID)
		and ad.ArtworkReqID=''
) PoQty
where f.IsProduceFty=1
and o.category in ('B','S')
and o.MDivisionID='{Sci.Env.User.Keyword}' 
and ot.ArtworkTypeID = '{this.CurrentMaintain["artworktypeid"]}' 
and o.Junk=0
and o.id = '{dr["OrderID"]}'
and isnull(oa.PatternCode,'') = '{dr["PatternCode"]}'
and isnull(oa.PatternDesc,'') = '{dr["PatternDesc"]}'
and isnull(oa.ArtworkID,ot.ArtworkTypeID) = '{dr["ArtworkId"]}'
and ((o.Category = 'B' and  ot.InhouseOSP = 'O') or (o.category = 'S'))
group by ReqQty.value,PoQty.value";
                        #endregion
                    }

                    DataRow drQty;
                    if (MyUtility.Check.Seek(sqlcmd, out drQty))
                    {
                        this.CurrentDetailData["exceedqty"] = ((decimal)drQty["AccReqQty"] - (int)drQty["OrderQty"]) < 0 ? 0 : (decimal)drQty["AccReqQty"] - (int)drQty["OrderQty"];
                    }

                    this.CurrentDetailData["ReqQty"] = e.FormattedValue;
                    this.RefreshIrregularQtyReason();
                }
            };

            #region 欄位設定
            this.Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("orderid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("FactoryID", header: "Factory", width: Widths.AnsiChars(5), iseditingreadonly: true)
            .Text("StyleID", header: "Style", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Date("sewinline", header: "Sewing Inline", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Date("scidelivery", header: "SCI Delivery", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("Article", header: "Article", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("ArtworkId", header: "Artwork", width: Widths.AnsiChars(17), iseditingreadonly: true)
            .Text("SizeCode", header: "Size", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("patterncode", header: "Cut Part", width: Widths.AnsiChars(5), iseditingreadonly: true)
            .Text("PatternDesc", header: "Cut Part Name", width: Widths.AnsiChars(20), iseditingreadonly: true)
            .Text("Remark", header: "Remark", iseditingreadonly: true, width: Widths.AnsiChars(15))
            .Numeric("ReqQty", header: "Req. Qty", width: Widths.AnsiChars(6), settings: col_ReqQty) // 可編輯
            .Numeric("stitch", header: "PCS/Stitch", width: Widths.AnsiChars(3)) // 可編輯
            .Numeric("qtygarment", header: "Qty/GMT", width: Widths.AnsiChars(5), maximum: 99, integer_places: 2) // 可編輯
            .Text("Farmout", header: "Farm Out", width: Widths.AnsiChars(5), iseditingreadonly: true)
            .Text("Farmin", header: "Farm In", width: Widths.AnsiChars(5), iseditingreadonly: true)
            .Text("ApQty", header: "A/P Qty", width: Widths.AnsiChars(5), iseditingreadonly: true)
            .Text("exceedqty", header: "Exceed Qty", width: Widths.AnsiChars(5), iseditingreadonly: true)
            .Text("ArtworkPOID", header: "Subcon PO#", width: Widths.AnsiChars(13), iseditingreadonly: true)
            ;
            #endregion
            #region 可編輯欄位變色
            this.detailgrid.Columns["ReqQty"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["stitch"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["qtygarment"].DefaultCellStyle.BackColor = Color.Pink;
            #endregion

        }

        // 新增時預設資料

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["MdivisionID"] = Sci.Env.User.Keyword;
            this.CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;
            this.CurrentMaintain["Reqdate"] = System.DateTime.Today;
            this.CurrentMaintain["handle"] = Sci.Env.User.UserID;
            this.CurrentMaintain["Status"] = "New";
            ((DataTable)this.detailgridbs.DataSource).Rows[0].Delete();
        }

        // edit前檢查

        /// <inheritdoc/>
        protected override bool ClickEditBefore()
        {
            if (this.CurrentMaintain["Status"].ToString() != "New")
            {
                MyUtility.Msg.WarningBox("The record status is not new, can't modify !!");
                return false;
            }

            return base.ClickEditBefore();
        }

        // save前檢查 & 取id

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            #region 必輸檢查

            if (this.CurrentMaintain["ReqDate"] == DBNull.Value || string.IsNullOrWhiteSpace(this.CurrentMaintain["ReqDate"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Req. Date >  canot be empty!", "Warning");
                this.dateReqDate.Focus();
                return false;
            }

            if (this.CurrentMaintain["ArtworktypeId"] == DBNull.Value || string.IsNullOrWhiteSpace(this.CurrentMaintain["ArtworktypeId"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Artwork Type >  canot be empty!", "Warning");
                this.txtartworktype_ftyArtworkType.Focus();
                return false;
            }

            if (this.CurrentMaintain["LocalSuppID"] == DBNull.Value || string.IsNullOrWhiteSpace(this.CurrentMaintain["LocalSuppID"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Supplier >  canot be empty!", "Warning");
                this.txtsubconSupplier.TextBox1.Focus();
                return false;
            }

            if (this.CurrentMaintain["Handle"] == DBNull.Value || string.IsNullOrWhiteSpace(this.CurrentMaintain["Handle"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Handle >  canot be empty!", "Warning");
                this.txtuserHandle.TextBox1.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["factoryid"]))
            {
                MyUtility.Msg.WarningBox("< Factory >  canot be empty!", "Warning");
                this.txtmfactory.Focus();
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

            foreach (DataRow row in ((DataTable)this.detailgridbs.DataSource).Select("ReqQty = 0"))
            {
                row.Delete();
            }

            if (this.DetailDatas.Count == 0)
            {
                MyUtility.Msg.WarningBox("Detail can't be empty", "Warning");
                return false;
            }

            // 判斷irregular Reason沒寫不能存檔
            var irregularQtyReason = new Sci.Production.Subcon.P05_IrregularQtyReason(this.CurrentMaintain["ID"].ToString(), this.CurrentMaintain, (DataTable)this.detailgridbs.DataSource, this.SqlGetBuyBackDeduction);

            DataTable dtIrregular = irregularQtyReason.Check_Irregular_Qty();
            this.UpdateExceedStatus(dtIrregular);
            if (dtIrregular != null)
            {
                bool isReasonEmpty = dtIrregular.AsEnumerable().Any(s => MyUtility.Check.Empty(s["SubconReasonID"]));
                if (isReasonEmpty)
                {
                    MyUtility.Msg.WarningBox("Irregular Qty Reason cannot be empty!");
                    return false;
                }
            }

            if (this.IsDetailInserting)
            {
                this.CurrentMaintain["id"] = Sci.MyUtility.GetValue.GetID(Sci.Env.User.Factory + "OR", "artworkReq", (DateTime)this.CurrentMaintain["ReqDate"]);
            }

            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override DualResult ClickSavePost()
        {
            DataTable dtCheck = ((DataTable)this.detailgridbs.DataSource).Clone();
            foreach (DataRow dr in ((DataTable)this.detailgridbs.DataSource).Rows)
            {
                DataRow drCheck = dtCheck.NewRow();
                if (dr.RowState == DataRowState.Deleted)
                {
                    drCheck["OrderID"] = dr["OrderID", DataRowVersion.Original];
                    drCheck["ArtworkID"] = dr["ArtworkID", DataRowVersion.Original];
                    drCheck["PatternCode"] = dr["PatternCode", DataRowVersion.Original];
                    drCheck["PatternDesc"] = dr["PatternDesc", DataRowVersion.Original];
                    drCheck["ReqQty"] = 0;
                }
                else
                {
                    drCheck["OrderID"] = dr["OrderID"];
                    drCheck["ArtworkID"] = dr["ArtworkID"];
                    drCheck["PatternCode"] = dr["PatternCode"];
                    drCheck["PatternDesc"] = dr["PatternDesc"];
                    drCheck["ReqQty"] = dr["ReqQty"];
                }

                dtCheck.Rows.Add(drCheck);
            }

            DualResult result = this.UpdateIrregularStatusByDelete(dtCheck);
            if (!result)
            {
                return result;
            }

            return base.ClickSavePost();
        }

        /// <inheritdoc/>
        protected override void ClickCheck()
        {
            base.ClickCheck();
            DualResult result;
            string sqlcmd;
            string strStatus;

            #region 檢查生產工廠
            var notProduceFtyDetail = this.DetailDatas.Where(s => !MyUtility.Convert.GetBool(s["IsProduceFty"]));
            if (notProduceFtyDetail.Any())
            {
                MyUtility.Msg.WarningBox("Below SP# which <Factory> is not production factory" + Environment.NewLine + notProduceFtyDetail.Select(s => s["orderid"].ToString()).Distinct().JoinToString(Environment.NewLine));
                return;
            }

            #endregion

            #region 檢查LocalSupp_Bank
            DualResult resultCheckLocalSupp_BankStatus = Prgs.CheckLocalSupp_BankStatus(this.CurrentMaintain["localsuppid"].ToString(), Prgs.CallFormAction.Confirm);
            if (!resultCheckLocalSupp_BankStatus)
            {
                return;
            }
            #endregion

            if (MyUtility.Check.Empty(this.CurrentMaintain["Exceed"]))
            {
                strStatus = "Approved";
            }
            else
            {
                strStatus = "Locked";
            }

            // 判斷irregular Reason沒寫不能存檔
            var irregularQtyReason = new Sci.Production.Subcon.P05_IrregularQtyReason(this.CurrentMaintain["ID"].ToString(), this.CurrentMaintain, (DataTable)this.detailgridbs.DataSource, this.SqlGetBuyBackDeduction);

            DataTable dtIrregular = irregularQtyReason.Check_Irregular_Qty();
            if (dtIrregular != null)
            {
                bool isReasonEmpty = dtIrregular.AsEnumerable().Any(s => MyUtility.Check.Empty(s["SubconReasonID"]));
                if (isReasonEmpty)
                {
                    MyUtility.Msg.WarningBox("Irregular Qty Reason cannot be empty!");
                    return;
                }
            }

            sqlcmd = $@"
update artworkReq 
set status='{strStatus}'
, DeptApvName ='{Env.User.UserID}', DeptApvDate = GETDATE()
, editname='{Env.User.UserID}', editdate=GETDATE() 
where id = '{this.CurrentMaintain["id"]}'";
            if (!(result = DBProxy.Current.Execute(null, sqlcmd)))
            {
                this.ShowErr(sqlcmd, result);
                return;
            }

            MyUtility.Msg.InfoBox("Successfully");
        }

        /// <inheritdoc/>
        protected override void ClickUncheck()
        {
            base.ClickUncheck();
            DualResult result;
            string sqlcmd;

            bool isPoExists = this.DetailDatas.Any(s => !MyUtility.Check.Empty(s["ArtworkPOID"]));
            if (isPoExists)
            {
                MyUtility.Msg.WarningBox("PO has been create, cannot be unCheck!");
                return;
            }

            sqlcmd = $@"
update artworkReq 
set status='New'
, DeptApvName = '', DeptApvDate = null
, editname='{Env.User.UserID}', editdate=GETDATE() 
where id = '{this.CurrentMaintain["id"]}'";

            if (!(result = DBProxy.Current.Execute(null, sqlcmd)))
            {
                this.ShowErr(sqlcmd, result);
                return;
            }

            MyUtility.Msg.InfoBox("Successfully");
        }

        /// <inheritdoc/>
        protected override void ClickConfirm()
        {
            #region 檢查LocalSupp_Bank
            DualResult resultCheckLocalSupp_BankStatus = Prgs.CheckLocalSupp_BankStatus(this.CurrentMaintain["localsuppid"].ToString(), Prgs.CallFormAction.Confirm);
            if (!resultCheckLocalSupp_BankStatus)
            {
                return;
            }
            #endregion

            #region 檢查生產工廠
            var notProduceFtyDetail = this.DetailDatas.Where(s => !MyUtility.Convert.GetBool(s["IsProduceFty"]));
            if (notProduceFtyDetail.Any())
            {
                MyUtility.Msg.WarningBox("Below SP# which <Factory> is not production factory" + Environment.NewLine + notProduceFtyDetail.Select(s => s["orderid"].ToString()).Distinct().JoinToString(Environment.NewLine));
                return;
            }

            #endregion

            DualResult result;

            string sqlcmd;
            sqlcmd = $@"
update artworkReq 
set status='Approved'
, MgApvName = '{Env.User.UserID}', MgApvDate = GETDATE() 
, editname='{Env.User.UserID}', editdate = GETDATE() 
where id = '{this.CurrentMaintain["id"]}'";

            if (!(result = DBProxy.Current.Execute(null, sqlcmd)))
            {
                this.ShowErr(sqlcmd, result);
                return;
            }

            MyUtility.Msg.InfoBox("Successfully");
            base.ClickConfirm();
        }

        /// <inheritdoc/>
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            DualResult result;
            string sqlcmd;

            bool isPoExists = this.DetailDatas.Any(s => !MyUtility.Check.Empty(s["ArtworkPOID"]));
            if (isPoExists)
            {
                MyUtility.Msg.WarningBox("PO has been create, cannot be unconfirm!");
                return;
            }

            sqlcmd = $@"
update artworkReq 
set status='Locked'
, MgApvName = '', MgApvDate = null 
, editname='{Env.User.UserID}', editdate = GETDATE() 
where id = '{this.CurrentMaintain["id"]}'";
            if (!(result = DBProxy.Current.Execute(null, sqlcmd)))
            {
                this.ShowErr(sqlcmd, result);
                return;
            }

            MyUtility.Msg.InfoBox("Successfully");
        }

        /// <inheritdoc/>
        protected override void ClickClose()
        {
            base.ClickClose();
            string sqlcmd;
            sqlcmd = $@"
update artworkReq 
set status = 'Closed'
, OriStatus = Status
, CloseUnCloseName = '{Env.User.UserID}', CloseUnCloseDate = GETDATE()
, editname='{Env.User.UserID}', editdate = GETDATE() 
where id = '{this.CurrentMaintain["id"]}'";

            DualResult result;
            if (!(result = DBProxy.Current.Execute(null, sqlcmd)))
            {
                this.ShowErr(sqlcmd, result);
                return;
            }

            MyUtility.Msg.InfoBox("Successfully");
        }

        /// <inheritdoc/>
        protected override void ClickUnclose()
        {
            base.ClickUnclose();

            string sqlcmd;
            sqlcmd = $@"
update artworkReq 
set status = OriStatus
, CloseUnCloseName = '{Env.User.UserID}', CloseUnCloseDate = GETDATE()
, editname='{Env.User.UserID}', editdate = GETDATE() 
where id = '{this.CurrentMaintain["id"]}'";

            DualResult result;
            if (!(result = DBProxy.Current.Execute(null, sqlcmd)))
            {
                this.ShowErr(sqlcmd, result);
                return;
            }

            MyUtility.Msg.InfoBox("Successfully");
        }

        /// <inheritdoc/>
        protected override void ClickUndo()
        {
            base.ClickUndo();
            this.RenewData();
        }

        /// <inheritdoc/>
        protected override bool ClickDeleteBefore()
        {
            if (this.CurrentMaintain["Status"].ToString() != "New")
            {
                MyUtility.Msg.WarningBox("Data is Approved or Closed cannot delete.", "Warning");
                return false;
            }

            return base.ClickDeleteBefore();
        }

        /// <inheritdoc/>
        protected override DualResult ClickDeletePost()
        {
            DataTable dtDelete = this.DetailDatas.CopyToDataTable();

            foreach (DataRow item in dtDelete.Rows)
            {
                item["ReqQty"] = 0;
            }

            DualResult result = this.UpdateIrregularStatusByDelete(dtDelete);
            if (!result)
            {
                return result;
            }

            return base.ClickDeletePost();
        }

        // print

        /// <inheritdoc/>
        protected override bool ClickPrint()
        {
            decimal totalReqQty = 0;
            #region -- 加總明細金額，顯示於表頭 --
            if (!(this.CurrentMaintain == null))
            {
                foreach (DataRow drr in ((DataTable)this.detailgridbs.DataSource).Rows)
                {
                    totalReqQty += (decimal)drr["ReqQty"];
                }
            }
            #endregion

            // 跳轉至PrintForm
            Sci.Production.Subcon.P05_Print callPrintForm = new Sci.Production.Subcon.P05_Print(this.CurrentMaintain, totalReqQty.ToString());
            callPrintForm.ShowDialog(this);
            return true;
        }

        // batch import
        private void BtnBatchImport_Click(object sender, EventArgs e)
        {
            var dr = this.CurrentMaintain;
            if (dr == null)
            {
                return;
            }

            if (MyUtility.Check.Empty(this.txtsubconSupplier.TextBox1.Text))
            {
                MyUtility.Msg.WarningBox("Please fill Supplier first!");
                this.txtsubconSupplier.TextBox1.Focus();
                return;
            }

            if (dr["artworktypeid"] == DBNull.Value)
            {
                MyUtility.Msg.WarningBox("Please fill Artworktype first!");
                this.txtartworktype_ftyArtworkType.Focus();
                return;
            }

            var frm = new Sci.Production.Subcon.P05_Import(dr, (DataTable)this.detailgridbs.DataSource);
            frm.ParentIForm = this;
            frm.ShowDialog(this);

            DataTable dg = (DataTable)this.detailgridbs.DataSource;
            if (dg.Columns["style"] == null)
            {
                dg.Columns.Add("Style", typeof(string));
            }

            if (dg.Columns["sewinline"] == null)
            {
                dg.Columns.Add("sewinline", typeof(DateTime));
            }

            if (dg.Columns["scidelivery"] == null)
            {
                dg.Columns.Add("scidelivery", typeof(DateTime));
            }

            foreach (DataRow drr in ((DataTable)this.detailgridbs.DataSource).Rows)
            {
                if (drr.RowState == DataRowState.Deleted)
                {
                    continue;
                }

                DataTable order_dt;
                DBProxy.Current.Select(null, string.Format("select styleid, sewinline, scidelivery from orders WITH (NOLOCK) where id='{0}'", drr["orderid"].ToString()), out order_dt);
                if (order_dt.Rows.Count == 0)
                {
                    break;
                }

                drr["style"] = order_dt.Rows[0]["styleid"].ToString();
                drr["sewinline"] = order_dt.Rows[0]["sewinline"];
                drr["scidelivery"] = order_dt.Rows[0]["scidelivery"];
            }

            this.RenewData();

            #region 檢查異常價格

            this.RefreshIrregularQtyReason();

            #endregion

        }

        // batch create
        private void BtnBatchCreate_Click(object sender, EventArgs e)
        {
            if (this.EditMode)
            {
                return;
            }

            var frm = new Sci.Production.Subcon.P05_BatchCreate();
            frm.ParentIForm = this;
            frm.ShowDialog(this);
            this.ReloadDatas();
        }

        private void Txtartworktype_ftyArtworkType_Validating(object sender, CancelEventArgs e)
        {
            Production.Class.Txtartworktype_fty o;
            o = (Production.Class.Txtartworktype_fty)sender;

            if ((o.Text != o.OldValue) && this.EditMode)
            {
                ((DataTable)this.detailgridbs.DataSource).Rows.Clear();
            }

            this.ChangeDetailHeader();
        }

        private void ChangeDetailHeader()
        {
            #region --動態unit header --
            this.artworkunit = MyUtility.GetValue.Lookup(string.Format("select artworkunit from artworktype WITH (NOLOCK) where id='{0}'", this.txtartworktype_ftyArtworkType.Text)).ToString().Trim();
            if (this.artworkunit == string.Empty)
            {
                this.artworkunit = "PCS";
            }

            this.detailgrid.Columns["stitch"].HeaderText = this.artworkunit;
            #endregion
        }

        private void BtnIrrPriceReason_Click(object sender, EventArgs e)
        {
            if ((DataTable)this.detailgridbs.DataSource == null)
            {
                return;
            }

            DataTable detailDatas = ((DataTable)this.detailgridbs.DataSource).Clone();
            foreach (DataRow dr in ((DataTable)this.detailgridbs.DataSource).Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    detailDatas.ImportRow(dr);
                }
            }

            var frm = new Sci.Production.Subcon.P05_IrregularQtyReason(this.CurrentMaintain["ID"].ToString(), this.CurrentMaintain, detailDatas, this.SqlGetBuyBackDeduction);
            frm.ShowDialog(this);

            // 畫面關掉後，再檢查一次有無價格異常
            this.ShowWaitMessage("Data Loading...");
            this.RefreshIrregularQtyReason();
        }

        private void BtnBatchApprove_Click(object sender, EventArgs e)
        {
            if (this.Perm.Confirm || this.Perm.Check)
            {
                if (this.batchapprove == null || this.batchapprove.IsDisposed)
                {
                    this.batchapprove = new Sci.Production.Subcon.P05_BatchApprove(this.Reload, this.SqlGetBuyBackDeduction);
                    this.batchapprove.Show();
                }
                else
                {
                    this.batchapprove.Activate();
                }
            }
            else
            {
                MyUtility.Msg.WarningBox("You don't have permission to confirm.");
            }
        }

        /// <summary>
        /// Reload
        /// </summary>
        public void Reload()
        {
            if (this.CurrentDataRow != null)
            {
                string idIndex = string.Empty;
                if (!MyUtility.Check.Empty(this.CurrentMaintain))
                {
                    if (!MyUtility.Check.Empty(this.CurrentMaintain["id"]))
                    {
                        idIndex = MyUtility.Convert.GetString(this.CurrentMaintain["id"]);
                    }
                }

                this.ReloadDatas();
                this.RenewData();
                if (!MyUtility.Check.Empty(idIndex))
                {
                    this.gridbs.Position = this.gridbs.Find("ID", idIndex);
                }
            }
        }

        private void P05_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.batchapprove != null)
            {
                this.batchapprove.Dispose();
            }
        }

        private void TxtsubconSupplier_Validating(object sender, CancelEventArgs e)
        {
            if (this.CurrentMaintain["LocalSuppID"].ToString() != this.txtsubconSupplier.TextBox1.Text)
            {
                ((DataTable)this.detailgridbs.DataSource).Rows.Clear();
            }

            if (MyUtility.Check.Empty(this.txtsubconSupplier.TextBox1.Text))
            {
                this.CurrentMaintain["LocalSuppID"] = DBNull.Value;
            }
            else
            {
                this.CurrentMaintain["LocalSuppID"] = this.txtsubconSupplier.TextBox1.Text;
            }
        }

        /// <summary>
        /// 異常價格紀錄重新整理
        /// </summary>
        private void RefreshIrregularQtyReason()
        {
            if ((DataTable)this.detailgridbs.DataSource == null)
            {
                return;
            }

            DataTable detailDatas = ((DataTable)this.detailgridbs.DataSource).Clone();

            foreach (DataRow dr in ((DataTable)this.detailgridbs.DataSource).Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    detailDatas.ImportRow(dr);
                }
            }

            this.btnIrrQtyReason.ForeColor = Color.Black;
            var irregularQtyReason = new Sci.Production.Subcon.P05_IrregularQtyReason(this.CurrentMaintain["ID"].ToString(), this.CurrentMaintain, detailDatas, this.SqlGetBuyBackDeduction);

            DataTable dtIrregular = irregularQtyReason.Check_Irregular_Qty();
            this.HideWaitMessage();

            this.UpdateExceedStatus(dtIrregular);
            this.btnIrrQtyReason.Enabled = false;
            if (dtIrregular != null)
            {
                if (dtIrregular.Rows.Count > 0)
                {
                    this.btnIrrQtyReason.Enabled = true;
                    this.btnIrrQtyReason.ForeColor = Color.Red;
                }
            }
        }

        private void P05_FormLoaded(object sender, EventArgs e)
        {
            MyUtility.Tool.SetupCombox(this.queryfors, 2, 1, "0,,1,Exceed Qty");
            this.queryfors.SelectedIndex = 0;
            this.queryfors.SelectedIndexChanged += (s, d) =>
            {
                string hasJunk = MyUtility.Check.Empty(this.queryfors.SelectedValue) ? string.Empty : this.queryfors.SelectedValue.ToString();
                switch (hasJunk)
                {
                    case "0":
                        this.DefaultWhere = string.Empty;
                        break;
                    case "1":
                    default:
                        this.DefaultWhere = "Exceed = 1";
                        break;
                }

                this.ReloadDatas();
            };
        }

        private void BtnSpecialRecord_Click(object sender, EventArgs e)
        {
            var dr = this.CurrentMaintain;
            if (dr == null)
            {
                return;
            }

            if (dr["artworktypeid"] == DBNull.Value)
            {
                MyUtility.Msg.WarningBox("Please fill Artworktype first!");
                this.txtartworktype_ftyArtworkType.Focus();
                return;
            }

            var frm = new Sci.Production.Subcon.P05_SpecialRecord(dr, (DataTable)this.detailgridbs.DataSource);
            frm.ShowDialog(this);
            this.RenewData();
            this.detailgridbs.EndEdit();

            this.RefreshIrregularQtyReason();
        }

        private void UpdateExceedStatus(DataTable dtIrregular)
        {
            bool hasIrregular = false;

            if (dtIrregular != null)
            {
                if (dtIrregular.Rows.Count > 0)
                {
                    hasIrregular = true;
                }
            }

            bool isDetailExceedQtyNotZero = this.DetailDatas.Any(s => !MyUtility.Check.Empty(s["ExceedQty"]));
            if (isDetailExceedQtyNotZero || hasIrregular)
            {
                this.CurrentMaintain["Exceed"] = 1;
            }
            else
            {
                this.CurrentMaintain["Exceed"] = 0;
            }
        }

        /// <summary>
        /// 抓出需扣除的BuyBack訂單數量資料
        /// </summary>
        /// <param name="artworkTypeID">artworkTypeID</param>
        /// <returns>string</returns>
        public string SqlGetBuyBackDeduction(string artworkTypeID)
        {
            string sql = $@"
--抓出此次申請有BuyBack的訂單資料
select  fr.orderID,
        fr.Article,
        fr.SizeCode,
        fr.ArtworkID,
        fr.PatternCode,
        fr.PatternDesc,
        fr.Remark,
        fr.OrderQty,
		fr.LocalSuppID,
        obq.OrderIDFrom,
        [ArticleFrom] = iif(fr.Article = '', '', obq.ArticleFrom),
        [SizeCodeFrom] = iif(fr.SizeCode = '', '', obq.SizeCodeFrom),
        [BuyBackQty] = sum(obq.Qty)
into    #tmpBuyBackReqBase
from #FinalArtworkReq fr 
inner join Order_BuyBack_Qty obq with (nolock) on obq.ID = fr.OrderID and 
                                                  (
                                                    (fr.Article = '' and fr.SizeCode = obq.SizeCode) or
                                                    (fr.Article = obq.SizeCode and fr.SizeCode = '') or
                                                    (fr.Article = '' and fr.SizeCode = '')
                                                  )
where   exists( select 1
                from ArtworkReq_Detail AD with (nolock)
                inner join ArtworkReq a with (nolock) on ad.ID = a.ID
                where a.ArtworkTypeID = '{artworkTypeID}'
		        and ad.OrderID = obq.OrderIDFrom
                and ad.PatternCode = fr.PatternCode
                and ad.PatternDesc = fr.PatternDesc
                and ad.Remark = fr.Remark
                and ad.ArtworkID = fr.ArtworkID
                and a.id != '{artworkTypeID}'
                and a.status != 'Closed') 
Group by    fr.orderID,
            fr.Article,
            fr.SizeCode,
            fr.ArtworkID,
            fr.PatternCode,
            fr.PatternDesc,
            fr.Remark,
            fr.OrderQty,
			fr.LocalSuppID,
            obq.OrderIDFrom,
            iif(fr.Article = '', '', obq.ArticleFrom),
			iif(fr.SizeCode = '', '', obq.SizeCodeFrom)

--將有BuyBack訂單資料進一步抓出有賣給別張訂單的資料           
select  tbbr.OrderIDFrom,
        tbbr.ArticleFrom,
        tbbr.SizeCodeFrom,
        [OrderID] = obq.ID,
        [Article] = iif(tbbr.ArticleFrom = '', '', obq.Article),
        [SizeCode] = iif(tbbr.SizeCodeFrom = '', '', obq.SizeCode),
        tbbr.ArtworkID,
        tbbr.PatternCode,
        tbbr.PatternDesc,
        tbbr.Remark,
		tbbr.LocalSuppID,
        [BuyBackQty] = sum(obq.Qty)
into #tmpBuyBackFrom
from #tmpBuyBackReqBase tbbr
inner join Order_BuyBack_Qty obq on obq.OrderIDFrom = tbbr.OrderIDFrom and
                                    (
                                        (tbbr.ArticleFrom = '' and tbbr.SizeCodeFrom = obq.SizeCodeFrom) or
                                        (tbbr.ArticleFrom = obq.ArticleFrom and tbbr.SizeCodeFrom = '') or
                                        (tbbr.ArticleFrom = '' and tbbr.SizeCodeFrom = '')
                                    )   and
                                    obq.ID <> tbbr.OrderID
group by    tbbr.OrderIDFrom,
            tbbr.ArticleFrom,
            tbbr.SizeCodeFrom,
            obq.ID,
            iif(tbbr.ArticleFrom = '', '', obq.Article),
            iif(tbbr.SizeCodeFrom = '', '', obq.SizeCode),
            tbbr.ArtworkID,
            tbbr.PatternCode,
            tbbr.PatternDesc,
            tbbr.Remark,
		    tbbr.LocalSuppID

--推算出BuyBack的訂單可扣除的數量
select  tbbf.OrderIDFrom,
        tbbf.ArticleFrom,
        tbbf.SizeCodeFrom,
        tbbf.ArtworkID,
        tbbf.PatternCode,
        tbbf.PatternDesc,
        tbbf.Remark,
		tbbf.LocalSuppID,
        [BuyBackReqedQty] = Sum(case when ArtworkReq.val = 0 then 0
                                     when    (OrderQty.val - ArtworkReq.val) > tbbf.BuyBackQty then tbbf.BuyBackQty
                                     when    (OrderQty.val - ArtworkReq.val) < 0 then    0 
                                     else    (OrderQty.val - ArtworkReq.val) end
                                )
into #tmpBuyBackFromResult
from    #tmpBuyBackFrom tbbf
cross apply (   select  val = isnull(sum(Qty),0)
                from    Order_Qty oq  with (nolock)
                where   oq.ID = tbbf.orderID and
                        (
                            (tbbf.Article = '' and tbbf.SizeCode = oq.SizeCode) or
                            (tbbf.Article = oq.Article and tbbf.SizeCode = '') or
                            (tbbf.Article = '' and tbbf.SizeCode = '')
                        )
            )   OrderQty
cross apply (   select val = isnull(sum(AD.ReqQty), 0)
                from ArtworkReq_Detail AD with (nolock)
                inner join ArtworkReq a with (nolock) on ad.ID = a.ID
                where a.ArtworkTypeID = '{artworkTypeID}'
                and ad.OrderID = tbbf.orderID
                and ad.Article = tbbf.Article
                and ad.SizeCode = tbbf.SizeCode
                and ad.PatternCode = tbbf.PatternCode
                and ad.PatternDesc = tbbf.PatternDesc
                and ad.Remark = tbbf.Remark
                and ad.ArtworkID = tbbf.ArtworkID
                and a.id != '{artworkTypeID}'
                and a.status != 'Closed'
            )   ArtworkReq
group by    tbbf.OrderIDFrom,
            tbbf.ArticleFrom,
            tbbf.SizeCodeFrom,
            tbbf.ArtworkID,
            tbbf.PatternCode,
            tbbf.PatternDesc,
            tbbf.Remark,
		    tbbf.LocalSuppID

--算出此次申請的訂單應該被扣掉多少數量
select  tbbr.orderID,
        tbbr.Article,
        tbbr.SizeCode,
        tbbr.ArtworkID,
        tbbr.PatternCode,
        tbbr.PatternDesc,
        tbbr.Remark,
        tbbr.OrderQty,
        tbbr.OrderIDFrom,
        tbbr.BuyBackQty,
		tbbr.LocalSuppID,
        [BuyBackArtworkReq] =   case when (isnull(BuyBackArtworkReq.val, 0) - isnull(tbbfr.BuyBackReqedQty,0)) > tbbr.BuyBackQty then tbbr.BuyBackQty
                                when    (isnull(BuyBackArtworkReq.val, 0) - isnull(tbbfr.BuyBackReqedQty,0)) < 0 then 0
                                else    (isnull(BuyBackArtworkReq.val, 0) - isnull(tbbfr.BuyBackReqedQty,0)) end
into    #tmpBuyBackDeduction
from    #tmpBuyBackReqBase tbbr 
left join   #tmpBuyBackFromResult tbbfr on  tbbfr.OrderIDFrom = tbbr.OrderIDFrom         and
                                            tbbfr.ArticleFrom = tbbr.ArticleFrom         and
                                            tbbfr.SizeCodeFrom = tbbr.SizeCodeFrom       and
                                            tbbfr.PatternCode = tbbr.PatternCode     and
                                            tbbfr.PatternDesc = tbbr.PatternDesc     and
                                            tbbfr.Remark = tbbr.Remark     and
                                            tbbfr.ArtworkID = tbbr.ArtworkID and
											tbbfr.LocalSuppID = tbbr.LocalSuppID
outer apply (   select val = isnull(sum(AD.ReqQty), 0)
                from ArtworkReq_Detail AD with (nolock)
                inner join ArtworkReq a with (nolock) on ad.ID = a.ID
                where a.ArtworkTypeID = '{artworkTypeID}'
		        and ad.OrderID = tbbr.OrderIDFrom
                and ad.Article = tbbr.ArticleFrom
                and ad.SizeCode = tbbr.SizeCodeFrom
                and ad.PatternCode = tbbr.PatternCode
                and ad.PatternDesc = tbbr.PatternDesc
                and ad.Remark = tbbr.Remark
                and ad.ArtworkID = tbbr.ArtworkID
                and a.id != '{artworkTypeID}'
                and a.status != 'Closed') BuyBackArtworkReq
";

            return sql;
        }

        private DualResult UpdateIrregularStatusByDelete(DataTable dtDelete)
        {
            if (dtDelete.Rows.Count == 0)
            {
                return new DualResult(true);
            }

            var irregularQtyReason = new Sci.Production.Subcon.P05_IrregularQtyReason(this.CurrentMaintain["ID"].ToString(), this.CurrentMaintain, dtDelete, this.SqlGetBuyBackDeduction);

            DataTable dtIrregular = irregularQtyReason.GetData();

            if (dtIrregular.Rows.Count == 0)
            {
                return new DualResult(true);
            }

            string sqlUpdateIrregular = $@"
alter table #tmp alter column OrderID varchar(13)
alter table #tmp alter column ArtworkTypeID varchar(20)

delete  ArtworkReq_IrregularQty
where   exists(select 1 from #tmp t where t.OrderID = ArtworkReq_IrregularQty.OrderID and t.ArtworkTypeID = ArtworkReq_IrregularQty.ArtworkTypeID and t.NeedDelete = 1)

update  ai set  ai.StandardQty = t.StandardQty,
                ai.ReqQty = t.ReqQty
from    ArtworkReq_IrregularQty ai
inner join #tmp t on t.OrderID = ai.OrderID and t.ArtworkTypeID = ai.ArtworkTypeID and t.NeedUpdate = 1

select  *
into #ArtworkReq
from ArtworkReq ar with (nolock)
where   exists(select 1 from #tmp t 
            inner join ArtworkReq_Detail ard with (nolock) on t.OrderID = ard.OrderID
            where t.NeedDelete = 1 and t.ArtworkTypeID = ar.ArtworkTypeID and ard.ID =  ar.ID) and
        Exceed = 1

select * from #ArtworkReq

select  *
from ArtworkReq_Detail
where ID in (select ID from #ArtworkReq)
";

            DataTable[] dtResult;
            DualResult result = MyUtility.Tool.ProcessWithDatatable(dtIrregular, string.Empty, sqlUpdateIrregular, out dtResult);
            if (result == false)
            {
                return result;
            }

            DataTable dtArtworkReq = dtResult[0];
            var dtAllArtworkReq_Detail = dtResult[1].AsEnumerable();

            if (dtArtworkReq.Rows.Count > 0)
            {
                foreach (DataRow dr in dtArtworkReq.Rows)
                {
                    DataTable dtArtworkReq_Detail = dtAllArtworkReq_Detail.Where(s => s["ID"].ToString() == dr["ID"].ToString()).CopyToDataTable();
                    var irregularCheck = new Sci.Production.Subcon.P05_IrregularQtyReason(dr["ID"].ToString(), dr, dtArtworkReq_Detail, this.SqlGetBuyBackDeduction);

                    DataTable dtIrregularCheck = irregularCheck.GetData();

                    if (dtIrregularCheck.Rows.Count > 0)
                    {
                        continue;
                    }

                    string sqlFixExceed = $@"update ArtworkReq set Exceed = 0 where ID = '{dr["ID"]}'
update ArtworkReq_Detail set ExceedQty = 0 where ID = '{dr["ID"]}'
";
                    result = DBProxy.Current.Execute(null, sqlFixExceed);
                    if (!result)
                    {
                        return result;
                    }
                }
            }

            return new DualResult(true);
        }
    }
}
