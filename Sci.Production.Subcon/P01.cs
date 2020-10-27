using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.PublicPrg;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Subcon
{
    /// <summary>
    /// P01
    /// </summary>
    public partial class P01 : Win.Tems.Input6
    {
        /// <summary>
        ///  異常價格視窗Grid的異動後DataSource，僅提供P01新增模式下使用
        /// </summary>
        public static DataTable Tmp_ModifyTable { get; set; }

        /// <summary>
        ///  異常價格視窗Grid的異動前DataSource，僅提供P01新增模式下使用
        /// </summary>
        public static DataTable Tmp_OriginDT_FromDB { get; set; }

        private string artworkunit;
        private bool isNeedPlanningB03Quote = false;
        private int IrregularPriceReason_ReasonNullCount = 0;
        private Form batchapprove;

        /// <summary>
        /// P01
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = "MdivisionID = '" + Sci.Env.User.Keyword + "' and POTYPE='O'";
            this.gridicon.Append.Enabled = false;
            this.gridicon.Append.Visible = false;
            this.gridicon.Insert.Enabled = false;
            this.gridicon.Insert.Visible = false;
            this.dateLockDate.ReadOnly = true;
            this.dateApproveDate.ReadOnly = true;
            this.dateCloseDate.ReadOnly = true;
            this.txtsubconSupplier.TextBox1.Validated += (s, e) =>
            {
                if (this.EditMode && this.txtsubconSupplier.TextBox1.Text != this.txtsubconSupplier.TextBox1.OldValue)
                {
                    this.CurrentMaintain["CurrencyID"] = MyUtility.GetValue.Lookup("CurrencyID", this.txtsubconSupplier.TextBox1.Text, "LocalSupp", "ID");
                }

                this.CurrentMaintain["localsuppid"] = this.txtsubconSupplier.TextBox1.Text;
            };

            this.detailgrid.RowsAdded += this.Detailgrid_RowsAdded;
        }

        // 新增時預設資料

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["MdivisionID"] = Sci.Env.User.Keyword;
            this.CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;
            this.CurrentMaintain["issuedate"] = System.DateTime.Today;
            this.CurrentMaintain["potype"] = "O";
            this.CurrentMaintain["handle"] = Sci.Env.User.UserID;
            this.CurrentMaintain["VatRate"] = 0;
            this.CurrentMaintain["Status"] = "New";
            ((DataTable)this.detailgridbs.DataSource).Rows[0].Delete();
            this.txtartworktype_ftyArtworkType.ReadOnly = false;
            this.txtmfactory.ReadOnly = false;
        }

        // delete前檢查 CurrentMaintain["id"]的FarmOut_Detail/FarmIn_Detail有data則不能刪除

        /// <inheritdoc/>
        protected override bool ClickDeleteBefore()
        {
            if (this.CurrentMaintain["Status"].ToString() != "New")
            {
                MyUtility.Msg.WarningBox("Data is approved or closed, can't delete.", "Warning");
                return false;
            }

            // sql參數準備
            IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>
            {
                new System.Data.SqlClient.SqlParameter("@id", this.CurrentMaintain["id"].ToString()),
            };

            // FarmOut_Detail/FarmIn_Detail
            string sqlcmd = @"select a.Farmin,a.Farmout from ArtworkPO_Detail a WITH (NOLOCK) where a.ID=@id";
            DBProxy.Current.Select(null, sqlcmd, paras, out DataTable dt);

            // 有則return
            if (dt.AsEnumerable().Any(r => MyUtility.Convert.GetInt(r["Farmin"]) > 0) ||
                dt.AsEnumerable().Any(r => MyUtility.Convert.GetInt(r["Farmout"]) > 0))
            {
                MyUtility.Msg.WarningBox(string.Format("Some SP# already have Farm In/Out data!!!"), "Warning");
                return false;
            }

            string chkP10exists = $@"
select 1
from ArtworkPO_detail apd with(nolock)
inner join ArtworkAP_detail aad with(nolock) on apd.id = aad.artworkpoid and aad.artworkpo_detailukey = apd.ukey
where  apd.id = '{this.CurrentMaintain["id"]}' 
";
            if (MyUtility.Check.Seek(chkP10exists))
            {
                MyUtility.Msg.WarningBox("Some SP# already have Subcon AP data.");
                return false;
            }

            return base.ClickDeleteBefore();
        }

        private void Detailgrid_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (this.EditMode)
            {
                this.DetalGridCellEditChange(e.RowIndex);
            }
        }

        /// <inheritdoc/>
        protected override DualResult ClickDeletePre()
        {
            string sqlClearArtworkPOID = $"update ArtworkReq_Detail set ArtworkPOID = '' where ArtworkPOID = '{this.CurrentMaintain["ID"]}'";
            DualResult result = DBProxy.Current.Execute(null, sqlClearArtworkPOID);
            if (!result)
            {
                return result;
            }

            return base.ClickDeletePre();
        }

        // edit前檢查

        /// <inheritdoc/>
        protected override bool ClickEditBefore()
        {
            // !EMPTY(APVName) OR !EMPTY(Closed)，只能編輯remark欄。
            if (this.CurrentMaintain["Status"].ToString() != "New")
            {
                var frm = new Sci.Production.PublicForm.EditRemark("artworkpo", "remark", this.CurrentMaintain);
                frm.ShowDialog(this);
                this.RenewData();
                return false;
            }

            return base.ClickEditBefore();
        }

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            #region 如果採購單已建立 AP, 則Supplier欄位不可編輯
            string chkp10exists = $@"
select 1
from ArtworkPO_detail apd with(nolock)
inner join ArtworkAP_detail aad with(nolock) on apd.id = aad.artworkpoid and aad.artworkpo_detailukey = apd.ukey
where  apd.id = '{this.CurrentMaintain["id"]}' 
";
            if (MyUtility.Check.Seek(chkp10exists))
            {
                this.txtsubconSupplier.TextBox1.ReadOnly = true;
            }
            #endregion

            this.txtartworktype_ftyArtworkType.ReadOnly = true;
            this.txtmfactory.ReadOnly = true;

            foreach (DataGridViewRow dr in this.detailgrid.Rows)
            {
                this.DetalGridCellEditChange(dr.Index);
            }
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

            if (this.CurrentMaintain["Delivery"] == DBNull.Value || string.IsNullOrWhiteSpace(this.CurrentMaintain["Delivery"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Delivery Date >  can't be empty!", "Warning");
                this.dateDeliveryDate.Focus();
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

            bool isDetailHasUniPriceZeroData = this.DetailDatas.Any(s => MyUtility.Check.Empty(s["unitprice"]));
            if (isDetailHasUniPriceZeroData)
            {
                MyUtility.Msg.WarningBox("Unit Price cannot be empty.");
                return false;
            }
            #endregion

            #region 如果採購單已建立 AP, 則Supplier更改失敗
            string chkp10exists = $@"
select 1
from ArtworkPO_detail apd with(nolock)
inner join ArtworkAP_detail aad with(nolock) on apd.id = aad.artworkpoid and aad.artworkpo_detailukey = apd.ukey
where  apd.id = '{this.CurrentMaintain["id"]}' 
";
            if (MyUtility.Check.Seek(chkp10exists) && MyUtility.Convert.GetString(this.CurrentMaintain["localsuppid"]) != MyUtility.Convert.GetString(this.CurrentMaintain["localsuppid", DataRowVersion.Original]))
            {
                this.CurrentMaintain["localsuppid"] = this.CurrentMaintain["localsuppid", DataRowVersion.Original];
                MyUtility.Msg.InfoBox("PO had already created AP, supplier cannot modify.");
            }
            #endregion

            foreach (DataRow row in ((DataTable)this.detailgridbs.DataSource).Select("poqty = 0"))
            {
                row.Delete();
            }

            if (this.DetailDatas.Count == 0)
            {
                MyUtility.Msg.WarningBox("Detail can't be empty", "Warning");
                return false;
            }

            if (this.IrregularPriceReason_ReasonNullCount > 0)
            {
                MyUtility.Msg.WarningBox("There is Irregular Price!! Please fix it.");
                return false;
            }

            // 取單號： getID(MyApp.cKeyword+GetDocno('PMS', 'ARTWORKPO1'), 'ARTWORKPO', IssueDate, 2)
            if (this.IsDetailInserting)
            {
                string factorykeyword = Sci.MyUtility.GetValue.Lookup(string.Format("select keyword from dbo.factory WITH (NOLOCK) where ID ='{0}'", this.CurrentMaintain["factoryid"]));
                if (MyUtility.Check.Empty(factorykeyword))
                {
                    MyUtility.Msg.WarningBox("Factory Keyword is empty, Please contact to MIS!!");
                    return false;
                }

                this.CurrentMaintain["id"] = Sci.MyUtility.GetValue.GetID(Sci.Env.User.Keyword + "OS", "artworkpo", (DateTime)this.CurrentMaintain["issuedate"]);
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

            // 除了sample單, 表身不重複欄位 id,artworkid,patterncode,OrderId,ArtworkReqID,Article,SizeCode
            bool dup = this.DetailDatas.AsEnumerable().Where(w => w["Category"].ToString() != "S")
                .GroupBy(g => new
                {
                    id = MyUtility.Convert.GetString(g["id"]),
                    artworkid = MyUtility.Convert.GetString(g["artworkid"]),
                    patterncode = MyUtility.Convert.GetString(g["patterncode"]),
                    OrderId = MyUtility.Convert.GetString(g["OrderId"]),
                    ArtworkReqID = MyUtility.Convert.GetString(g["ArtworkReqID"]),
                    Article = MyUtility.Convert.GetString(g["Article"]),
                    SizeCode = MyUtility.Convert.GetString(g["SizeCode"]),
                })
                .Select(s => new
                {
                    s.Key.id,
                    s.Key.artworkid,
                    s.Key.patterncode,
                    s.Key.OrderId,
                    s.Key.ArtworkReqID,
                    s.Key.Article,
                    s.Key.SizeCode,
                    ct = s.Count(),
                }).Any(a => a.ct > 1);
            if (dup)
            {
                MyUtility.Msg.WarningBox("id,artworkid,patterncode,OrderId,ArtworkReqID,Article,SizeCode detail key cannot duplicate.");
                return false;
            }
            #endregion
            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override DualResult ClickSavePost()
        {
            if (P01.Tmp_ModifyTable != null && P01.Tmp_OriginDT_FromDB != null)
            {
                // 新增模式下的異常價格紀錄寫入DB，是在這裡執行，內容與 P01_IrregularPriceReason 一樣
                StringBuilder sql = new StringBuilder();

                // ModifyTable 去掉 OriginDT_FromDB，剩下的不是新增就是修改
                var insert_Or_Update = Tmp_ModifyTable.AsEnumerable().Except(Tmp_OriginDT_FromDB.AsEnumerable(), DataRowComparer.Default).Where(o => o.Field<string>("SubconReasonID").Trim() != string.Empty);

                // 抓出ReasonID為空的出來刪除
                var delete = Tmp_ModifyTable.AsEnumerable().Where(o => o.Field<string>("SubconReasonID").Trim() == string.Empty);

                foreach (var item in delete)
                {
                    string pOID = item.Field<string>("POID");
                    string artworkType = item.Field<string>("Type");
                    sql.Append($"DELETE FROM [ArtworkPO_IrregularPrice] WHERE POID='{pOID}' AND ArtworkTypeID='{artworkType}'" + Environment.NewLine);
                    sql.Append(" " + Environment.NewLine);
                }

                foreach (var item in insert_Or_Update)
                {
                    string pOID = item.Field<string>("POID");
                    string artworkType = item.Field<string>("Type");
                    string subconReasonID = item.Field<string>("SubconReasonID");
                    decimal pOPrice = item.Field<decimal>("POPrice");
                    decimal standardPrice = item.Field<decimal>("StdPrice");

                    DualResult result = DBProxy.Current.Select(null, $"SELECT * FROM ArtworkPO_IrregularPrice WHERE POID='{pOID}' AND ArtworkTypeID='{artworkType}'", out DataTable dt);
                    if (result)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0]["SubconReasonID"].ToString() != subconReasonID && !string.IsNullOrEmpty(subconReasonID))
                            {
                                sql.Append($"UPDATE [ArtworkPO_IrregularPrice] SET [SubconReasonID]='{subconReasonID}',EditDate=GETDATE(),EditName='{Sci.Env.User.UserID}'" + Environment.NewLine);
                                sql.Append($"                                  WHERE [POID]='{pOID}' AND [ArtworkTypeID]='{artworkType}'" + Environment.NewLine);
                            }
                        }
                        else
                        {
                            sql.Append("INSERT INTO [ArtworkPO_IrregularPrice]([POID],[ArtworkTypeID],[POPrice],[StandardPrice],[SubconReasonID],[AddDate],[AddName])" + Environment.NewLine);
                            sql.Append($"                              VALUES ('{pOID}','{artworkType}',{pOPrice},{standardPrice},'{subconReasonID}',GETDATE(),'{Sci.Env.User.UserID}')" + Environment.NewLine);
                        }
                    }

                    sql.Append(" " + Environment.NewLine);
                }

                if (!MyUtility.Check.Empty(sql.ToString()))
                {
                    DualResult upResult;
                    upResult = DBProxy.Current.Execute(null, sql.ToString());
                    if (!upResult)
                    {
                        this.ShowErr(sql.ToString(), upResult);
                        return upResult;
                    }
                }

                P01.Tmp_ModifyTable = null;
                P01.Tmp_OriginDT_FromDB = null;
            }

            #region update ArtworkReq_Detail.ArtworkPOID
            DualResult updateArtworkReq_DetailResult;
            string sqlUpdateArtworkReq_Detail = string.Empty;

            // 將detail被刪除的部分update ArtworkPOID為空
            foreach (DataRow drDetail in this.GetDetailGridDatasByDeleted())
            {
                sqlUpdateArtworkReq_Detail += $@"
update ArtworkReq_Detail set ArtworkPOID = '' 
        where   ID = '{drDetail["ArtworkReqID", DataRowVersion.Original]}' and 
                OrderID = '{drDetail["OrderID", DataRowVersion.Original]}' and 
                ArtworkId = '{drDetail["ArtworkId", DataRowVersion.Original]}' and 
                PatternCode = '{drDetail["PatternCode", DataRowVersion.Original]}' and 
                PatternDesc = '{drDetail["PatternDesc", DataRowVersion.Original]}' 
";
            }

            if (!MyUtility.Check.Empty(sqlUpdateArtworkReq_Detail))
            {
                updateArtworkReq_DetailResult = DBProxy.Current.Execute(null, sqlUpdateArtworkReq_Detail);
                if (!updateArtworkReq_DetailResult)
                {
                    return updateArtworkReq_DetailResult;
                }
            }

            // 將ArtworkPO_Detail有維護的部分update ArtworkReq_Detail.ArtworkPOID
            updateArtworkReq_DetailResult = Prgs.UpdateArtworkReq_DetailArtworkPOID(this.CurrentMaintain["ID"].ToString());
            if (!updateArtworkReq_DetailResult)
            {
                return updateArtworkReq_DetailResult;
            }

            #endregion

            return base.ClickSavePost();
        }

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();

            this.DetailSelectCommand = string.Format(
                @"
select 
		 a.[ID]
		,a.[OrderID]
        ,a.[Article]
        ,a.[SizeCode]
		,a.[ArtworkId]
		,a.[PatternCode]
		,a.[PatternDesc]
		,a.[CostStitch]
		,a.[Stitch]
		,a.[UnitPrice]
		,a.[Cost]
		,[QtyGarment]=IIF(a.[QtyGarment] IS NULL OR a.[QtyGarment]=0,1,a.[QtyGarment])
		,a.[Price]
		,a.[Amount]
		,a.[Farmout]
		,a.[Farmin]
		,a.[ApQty]
		,a.[PoQty]
		,a.[Ukey]
		,a.[ArtworkTypeID]
		,a.[ExceedQty]
        ,a.[ArtworkReqID]
		,o.*
		, Price = a.unitprice * a.qtygarment
		, Style = o.styleid
		, sewinline = o.sewinline
		, scidelivery = o.scidelivery
from dbo.ArtworkPO_Detail a
left join dbo.Orders o on a.OrderID = o.id
where a.id = '{0}'  ORDER BY a.OrderID ", masterID);

            return base.OnDetailSelectCommandPrepare(e);
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.dateApproveDate.ReadOnly = true;
            this.ChangeDetailHeader();
            #region 判斷Artwork 是否需要在 Planning B03 報價
            this.isNeedPlanningB03Quote = Prgs.CheckNeedPlanningP03Quote(this.CurrentMaintain["artworktypeid"].ToString());
            #endregion

            #region -- 加總明細金額，顯示於表頭 --
            if (!(this.CurrentMaintain == null))
            {
                decimal totalPoQty = 0;
                foreach (DataRow drr in ((DataTable)this.detailgridbs.DataSource).Rows)
                {
                    totalPoQty += (decimal)drr["PoQty"];
                }

                this.numTotalPOQty.Text = totalPoQty.ToString();
            }
            #endregion
            this.numTotal.Text = (Convert.ToDecimal(this.numVat.Text) + Convert.ToDecimal(this.numAmount.Text)).ToString();

            if (this.CurrentMaintain["ID"] == DBNull.Value)
            {
                this.btnIrrPriceReason.Enabled = false;
            }
            else
            {
                this.btnIrrPriceReason.Enabled = true;
            }

            // btnIrprice.Enabled = !this.EditMode;
            #region Status Label
            this.label25.Text = this.CurrentMaintain["Status"].ToString();
            #endregion
            #region exceed status
            this.label17.Visible = this.CurrentMaintain["Exceed"].ToString().ToUpper() == "TRUE";
            #endregion
            #region Batch Import, Special record button
            this.btnBatchImport.Enabled = this.EditMode;
            #endregion
            #region Batch create
            this.btnBatchCreate.Enabled = !this.EditMode;
            #endregion

            #region Irregular Price判斷

            P01_IrregularPriceReason.tmp_IrregularPriceReason_List.Clear();

            this.btnIrrPriceReason.ForeColor = Color.Black;

            DataTable detailDatas = ((DataTable)this.detailgridbs.DataSource).Clone();
            foreach (DataRow dr in ((DataTable)this.detailgridbs.DataSource).Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    detailDatas.ImportRow(dr);
                }
            }

            var frm = new Sci.Production.Subcon.P01_IrregularPriceReason(this.CurrentMaintain["ID"].ToString(), this.CurrentMaintain["FactoryID"].ToString(), this.CurrentMaintain, detailDatas);

            // 取得價格異常DataTable，如果有，則存在 P30的_Irregular_Price_Table，  開啟P30_IrregularPriceReason時後直接丟進去，避免再做一次查詢
            this.ShowWaitMessage("Data Loading...");

            bool has_Irregular_Price = frm.Check_Irregular_Price(false);

            this.HideWaitMessage();

            if (has_Irregular_Price)
            {
                this.btnIrrPriceReason.ForeColor = Color.Red;
            }

            #endregion
        }

        // Detail Grid 設定

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            #region farm out qty 開窗
            Ict.Win.DataGridViewGeneratorTextColumnSettings ts = new DataGridViewGeneratorTextColumnSettings();
            ts.CellMouseDoubleClick += (s, e) =>
            {
                if (!this.EditMode)
                {
                    var dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    if (dr == null)
                    {
                        return;
                    }

                    var frm = new Sci.Production.Subcon.P01_FarmOutList(dr);
                    frm.ShowDialog(this);
                    this.RenewData();
                }
            };
            #endregion
            #region Farm In qty 開窗
            Ict.Win.DataGridViewGeneratorTextColumnSettings ts2 = new DataGridViewGeneratorTextColumnSettings();
            ts2.CellMouseDoubleClick += (s, e) =>
            {
                if (!this.EditMode)
                {
                    var dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    if (dr == null)
                    {
                        return;
                    }

                    var frm = new Sci.Production.Subcon.P01_FarmInList(dr);
                    frm.ShowDialog(this);
                    this.RenewData();
                }
            };
            #endregion
            #region AP qty 開窗
            Ict.Win.DataGridViewGeneratorTextColumnSettings ts3 = new DataGridViewGeneratorTextColumnSettings();
            ts3.CellMouseDoubleClick += (s, e) =>
            {
                if (!this.EditMode)
                {
                    var dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    if (dr == null)
                    {
                        return;
                    }

                    var frm = new Sci.Production.Subcon.P01_Ap(dr);
                    frm.ShowDialog(this);
                    this.RenewData();
                }
            };
            #endregion
            #region Unit Price Valid
            Ict.Win.DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();
            ns.CellValidating += (s, e) =>
            {
                if (this.EditMode && e.FormattedValue != null)
                {
                    this.CurrentDetailData["amount"] = (decimal)this.CurrentDetailData["poqty"] * (decimal)e.FormattedValue * (decimal)this.CurrentDetailData["qtygarment"];
                    this.CurrentDetailData["unitprice"] = e.FormattedValue;
                    this.CurrentDetailData["Price"] = (decimal)e.FormattedValue * (decimal)this.CurrentDetailData["qtygarment"];

                    // 重整異常價格
                    this.RefreshIrregularPriceReason();
                }
            };
            #endregion

            #region qtygarment Valid
            Ict.Win.DataGridViewGeneratorNumericColumnSettings ns2 = new DataGridViewGeneratorNumericColumnSettings();
            ns2.CellValidating += (s, e) =>
            {
                if (this.EditMode && e.FormattedValue != null)
                {
                    if (MyUtility.Check.Empty(e.FormattedValue))
                    {
                        e.FormattedValue = 1.0;
                    }

                    this.CurrentDetailData["amount"] = (decimal)this.CurrentDetailData["poqty"] * Convert.ToDecimal(e.FormattedValue) * (decimal)this.CurrentDetailData["unitprice"];
                    this.CurrentDetailData["qtygarment"] = Convert.ToDecimal(e.FormattedValue);
                    this.CurrentDetailData["Price"] = Convert.ToDecimal(e.FormattedValue) * (decimal)this.CurrentDetailData["unitprice"];

                    // 重整異常價格
                    this.RefreshIrregularPriceReason();
                }
            };
            #endregion
            #region 欄位設定
            this.Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("orderid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true) // 0
            .Text("Style", header: "Style", width: Widths.AnsiChars(15), iseditingreadonly: true) // 1
            .Numeric("PoQty", header: "PO Qty", width: Widths.AnsiChars(6), iseditingreadonly: true) // 2
            .Date("sewinline", header: "SewInLine", width: Widths.AnsiChars(10), iseditingreadonly: true) // 3
            .Date("scidelivery", header: "SciDelivery", width: Widths.AnsiChars(10), iseditingreadonly: true) // 4
            .Text("Article", header: "Article", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("ArtworkId", header: "Artwork", width: Widths.AnsiChars(8), iseditingreadonly: true) // 5
            .Text("SizeCode", header: "Size", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Numeric("coststitch", header: "Cost" + Environment.NewLine + "(PCS/Stitch)", width: Widths.AnsiChars(3), iseditingreadonly: true) // 6
            .Numeric("stitch", header: "PCS/Stitch", width: Widths.AnsiChars(3)) // 7
            .Text("patterncode", header: "Cutpart" + Environment.NewLine + "ID", width: Widths.AnsiChars(5), iseditingreadonly: true) // 8
            .Text("PatternDesc", header: "Cutpart Name", width: Widths.AnsiChars(15), iseditingreadonly: true) // 9
            .Numeric("unitprice", header: "Unit Price", width: Widths.AnsiChars(5), settings: ns, decimal_places: 4, integer_places: 4) // 10
            .Numeric("cost", header: "Cost" + Environment.NewLine + "(USD)", width: Widths.AnsiChars(5), iseditingreadonly: true, decimal_places: 4, integer_places: 4) // 11
            .Numeric("qtygarment", header: "Qty/GMT", width: Widths.AnsiChars(5), settings: ns2, maximum: 99, integer_places: 2) // 12
            .Numeric("Price", header: "Price/GMT", width: Widths.AnsiChars(5), iseditingreadonly: true, decimal_places: 4, integer_places: 5) // 13
            .Numeric("amount", header: "Amount", width: Widths.AnsiChars(8), iseditingreadonly: true, decimal_places: 2, integer_places: 14) // 14
            .Text("farmout", header: "Farm Out", width: Widths.AnsiChars(5), settings: ts, iseditingreadonly: true) // 15
            .Text("farmin", header: "Farm In", width: Widths.AnsiChars(5), settings: ts2, iseditingreadonly: true) // 16
            .Text("apqty", header: "A/P Qty", width: Widths.AnsiChars(5), settings: ts3, iseditingreadonly: true) // 17
            .Text("ArtworkReqID", header: "Subcon Req#", width: Widths.AnsiChars(13), iseditingreadonly: true);     // 18
            #endregion
            #region 可編輯欄位變色
            this.detailgrid.Columns["stitch"].DefaultCellStyle.BackColor = Color.Pink;  // PCS/Stitch
            this.detailgrid.Columns["qtygarment"].DefaultCellStyle.BackColor = Color.Pink; // Qty/GMT
            #endregion
        }

        /// <inheritdoc/>
        protected override void ClickCheck()
        {
            base.ClickCheck();
            DualResult result;
            string sqlcmd;

            sqlcmd = string.Format(
                "update artworkpo set status='Locked', LockName='{0}', LockDate=GETDATE(), editname='{0}', editdate=GETDATE() " +
                            "where id = '{1}'", Env.User.UserID,
                this.CurrentMaintain["id"].ToString());
            if (!(result = DBProxy.Current.Execute(null, sqlcmd)))
            {
                this.ShowErr(sqlcmd, result);
                return;
            }
        }

        /// <inheritdoc/>
        protected override void ClickUncheck()
        {
            base.ClickUncheck();
            DualResult result;
            string sqlcmd;

            DialogResult dResult = MyUtility.Msg.QuestionBox("Un Are you sure to unlock it?", "Question", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2);
            if (dResult.ToString().ToUpper() == "NO")
            {
                return;
            }

            sqlcmd = string.Format(
                "update artworkpo set Status='New', LockName='', LockDate=null, editname='{0}', editdate=GETDATE() " +
                            "where id = '{1}'", Env.User.UserID,
                this.CurrentMaintain["id"]);

            if (!(result = DBProxy.Current.Execute(null, sqlcmd)))
            {
                this.ShowErr(sqlcmd, result);
                return;
            }
        }

        /// <inheritdoc/>
        protected override void ClickConfirm()
        {
            DualResult result;

            string sqlcmd;

            sqlcmd = string.Format(
                "update artworkpo set status='Approved', apvname='{0}', apvdate=GETDATE(), editname='{0}', editdate=GETDATE() " +
                            "where id = '{1}'", Env.User.UserID,
                this.CurrentMaintain["id"]);

            if (!(result = DBProxy.Current.Execute(null, sqlcmd)))
            {
                this.ShowErr(sqlcmd, result);
                return;
            }

            base.ClickConfirm();
        }

        /// <inheritdoc/>
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            DualResult result;
            string checksql = string.Format("select ApQty from ArtworkPO_Detail WITH (NOLOCK) where id = '{0}'", this.CurrentMaintain["id"]);
            string sqlcmd;
            if (!(result = DBProxy.Current.Select(null, checksql, out DataTable checkdt)))
            {
                this.ShowErr(checksql, result);
                return;
            }

            if (checkdt.Rows.Count > 0 && checkdt.AsEnumerable().Any(row => MyUtility.Convert.GetInt(row["ApQty"]) > 0))
            {
                MessageBox.Show("Can not unconfirm");
                return;
            }

            DialogResult dResult = MyUtility.Msg.QuestionBox("Are you sure to unapprove it?", "Question", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2);
            if (dResult.ToString().ToUpper() == "NO")
            {
                return;
            }

            sqlcmd = string.Format(@"update artworkpo set status='Locked', apvname='', apvdate=null, editname='{0}', editdate=GETDATE() where id = '{1}'", Env.User.UserID, this.CurrentMaintain["id"]);
            if (!(result = DBProxy.Current.Execute(null, sqlcmd)))
            {
                this.ShowErr(sqlcmd, result);
                return;
            }
        }

        /// <inheritdoc/>
        protected override void ClickClose()
        {
            base.ClickClose();

            if (!Prgs.GetAuthority(Env.User.UserID) && this.CurrentMaintain["apvname"].ToString() != Env.User.UserID)
            {
                MyUtility.Msg.InfoBox("Only Apporver & leader can close!");
                return;
            }

            string sqlcmd;
            sqlcmd = string.Format(
                "update artworkpo set status='Closed', CloseName='{0}', CloseDate=GETDATE(), editname='{0}', editdate=GETDATE() " +
                            "where id = '{1}'", Env.User.UserID,
                this.CurrentMaintain["id"]);

            DualResult result;
            if (!(result = DBProxy.Current.Execute(null, sqlcmd)))
            {
                this.ShowErr(sqlcmd, result);
                return;
            }
        }

        /// <inheritdoc/>
        protected override void ClickUnclose()
        {
            base.ClickUnclose();
            if (!Prgs.GetAuthority(Env.User.UserID) && this.CurrentMaintain["apvname"].ToString() != Env.User.UserID)
            {
                MyUtility.Msg.InfoBox("Only Apporver & leader can unclose!");
                return;
            }

            string sqlcmd;
            DialogResult dResult = MyUtility.Msg.QuestionBox("Are you sure to unclose it?", "Question", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2);
            if (dResult.ToString().ToUpper() == "NO")
            {
                return;
            }

            sqlcmd = string.Format(
                "update artworkpo set Status='Approved', CloseName='', CloseDate=null, editname='{0}', editdate=GETDATE() " +
                            "where id = '{1}'", Env.User.UserID,
                this.CurrentMaintain["id"]);

            DualResult result;
            if (!(result = DBProxy.Current.Execute(null, sqlcmd)))
            {
                this.ShowErr(sqlcmd, result);
                return;
            }
        }

        /// <inheritdoc/>
        protected override void ClickUndo()
        {
            base.ClickUndo();
            this.RenewData();
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

            var frm = new Sci.Production.Subcon.P01_Import(dr, (DataTable)this.detailgridbs.DataSource, "P01", this.isNeedPlanningB03Quote);
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

                drr["Price"] = (decimal)drr["unitprice"] * (decimal)drr["qtygarment"];
                DBProxy.Current.Select(null, string.Format("select styleid, sewinline, scidelivery from orders WITH (NOLOCK) where id='{0}'", drr["orderid"].ToString()), out DataTable order_dt);
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

            this.RefreshIrregularPriceReason();

            #endregion

        }

        // batch create
        private void BtnBatchCreate_Click(object sender, EventArgs e)
        {
            if (this.EditMode)
            {
                return;
            }

            var frm = new Sci.Production.Subcon.P01_BatchCreate("P01");
            frm.ShowDialog(this);
            this.ReloadDatas();
        }

        // print

        /// <inheritdoc/>
        protected override bool ClickPrint()
        {
            // 跳轉至PrintForm
            Sci.Production.Subcon.P01_Print callPrintForm = new Sci.Production.Subcon.P01_Print(this.CurrentMaintain, this.numTotal.Text, this.numTotalPOQty.Text);
            callPrintForm.ShowDialog(this);
            return true;
        }

        private void Txtartworktype_ftyArtworkType_Validating(object sender, CancelEventArgs e)
        {
            Production.Class.Txtartworktype_fty o;
            o = (Production.Class.Txtartworktype_fty)sender;

            if ((o.Text != o.OldValue) && this.EditMode)
            {
                ((DataTable)this.detailgridbs.DataSource).Rows.Clear();
            }

            this.isNeedPlanningB03Quote = Prgs.CheckNeedPlanningP03Quote(this.txtartworktype_ftyArtworkType.Text);
            this.ChangeDetailHeader();

            // 重置暫存異常價格原因
            P01_IrregularPriceReason.tmp_IrregularPriceReason_List.Clear();
            this.RefreshIrregularPriceReason();
        }

        private void ChangeDetailHeader()
        {
            #region --動態unit header --
            this.artworkunit = MyUtility.GetValue.Lookup(string.Format("select artworkunit from artworktype WITH (NOLOCK) where id='{0}'", this.txtartworktype_ftyArtworkType.Text)).ToString().Trim();
            if (this.artworkunit == string.Empty)
            {
                this.artworkunit = "PCS";
            }

            this.detailgrid.Columns["coststitch"].HeaderText = "Cost" + Environment.NewLine + "(" + this.artworkunit + ")";
            this.detailgrid.Columns["stitch"].HeaderText = this.artworkunit;
            #endregion
        }

        /// <inheritdoc/>
        protected override void OnDetailGridDelete()
        {
            if (((DataTable)this.detailgridbs.DataSource).Rows.Count == 0)
            {
                return;
            }

            DataTable detailDatas = (DataTable)this.detailgridbs.DataSource;

            string chkp10exists = string.Format(
                @"
select distinct aad.orderid,aad.id
from ArtworkPO_detail apd with(nolock)
inner join ArtworkAP_detail aad with(nolock) on apd.id = aad.artworkpoid and aad.artworkpo_detailukey = apd.ukey
where  apd.id = '{0}' and apd.ukey = '{1}'
",
                this.CurrentMaintain["id"],
                this.CurrentDetailData["Ukey"]);
            DualResult result;
            if (result = DBProxy.Current.Select(null, chkp10exists, out DataTable dt))
            {
                if (dt.Rows.Count > 0)
                {
                    StringBuilder p10exists = new StringBuilder();
                    foreach (DataRow dr in dt.Rows)
                    {
                        p10exists.Append(string.Format("Please delete [Subcon][P10]:{0} {1} first !! \r\n", dr["id"], dr["orderid"]));
                    }

                    MyUtility.Msg.WarningBox(p10exists.ToString());
                    return;
                }
            }
            else
            {
                MyUtility.Msg.ErrorBox(result.ToString());
                return;
            }

            // 表身被刪除的資料，暫存的異常價格原因也要刪掉
            if (P01_IrregularPriceReason.tmp_IrregularPriceReason_List.Count > 0)
            {
                string pOID = this.CurrentDetailData["POID"].ToString();
                string artworktypeID = this.CurrentMaintain["ArtworktypeID"].ToString();

                if (P01_IrregularPriceReason.tmp_IrregularPriceReason_List.Where(o => o.POID == pOID && o.ArtWorkType_ID == artworktypeID).FirstOrDefault() != null)
                {
                    P01_IrregularPriceReason.tmp_IrregularPriceReason_List.RemoveAt(
                        P01_IrregularPriceReason.tmp_IrregularPriceReason_List.IndexOf(
                            P01_IrregularPriceReason.tmp_IrregularPriceReason_List.Where(o => o.POID == pOID && o.ArtWorkType_ID == artworktypeID).FirstOrDefault()));
                }
            }

            base.OnDetailGridDelete();

            #region 重整異常價格資訊
            this.RefreshIrregularPriceReason();
            #endregion

        }

        private void BtnIrrPriceReason_Click(object sender, EventArgs e)
        {
            DataTable detailDatas = ((DataTable)this.detailgridbs.DataSource).Clone();
            foreach (DataRow dr in ((DataTable)this.detailgridbs.DataSource).Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    detailDatas.ImportRow(dr);
                }
            }

            var frm = new Sci.Production.Subcon.P01_IrregularPriceReason(this.CurrentMaintain["ID"].ToString(), this.CurrentMaintain["FactoryID"].ToString(), this.CurrentMaintain, detailDatas);
            frm.ShowDialog(this);

            // 畫面關掉後，再檢查一次有無價格異常
            this.btnIrrPriceReason.ForeColor = Color.Black;
            this.ShowWaitMessage("Data Loading...");

            bool has_Irregular_Price = false;

            // 新增模式使用不同function
            if (MyUtility.Check.Empty(this.CurrentMaintain["ID"]))
            {
                has_Irregular_Price = frm.Check_Irregular_Price_Without_PO(false);
            }
            else
            {
                has_Irregular_Price = frm.Check_Irregular_Price(false);
            }

            this.IrregularPriceReason_ReasonNullCount = frm.ReasonNullCount;
            this.HideWaitMessage();

            if (has_Irregular_Price)
            {
                this.btnIrrPriceReason.ForeColor = Color.Red;
            }
        }

        private void BtnBatchApprove_Click(object sender, EventArgs e)
        {
            if (this.Perm.Confirm)
            {
                if (this.batchapprove == null || this.batchapprove.IsDisposed)
                {
                    this.batchapprove = new Sci.Production.Subcon.P01_BatchApprove(this.Reload);
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

        private void P01_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.batchapprove != null)
            {
                this.batchapprove.Dispose();
            }
        }

        private void DetalGridCellEditChange(int index)
        {
            #region 檢查Qty欄位是否可編輯
            string spNo = this.detailgrid.GetDataRow(index)["orderid"].ToString();

            if (!this.IsSampleOrder(spNo) && this.isNeedPlanningB03Quote)
            {
                this.detailgrid.Rows[index].Cells["unitprice"].ReadOnly = true;
                this.detailgrid.Rows[index].Cells["unitprice"].Style.ForeColor = Color.Black;
                this.detailgrid.Rows[index].Cells["unitprice"].Style.BackColor = Color.White; // Unit Price
            }
            else
            {
                this.detailgrid.Rows[index].Cells["unitprice"].ReadOnly = false;
                this.detailgrid.Rows[index].Cells["unitprice"].Style.ForeColor = Color.Red;
                this.detailgrid.Rows[index].Cells["unitprice"].Style.BackColor = Color.Pink; // Unit Price
            }

            #endregion
        }

        private void TxtsubconSupplier_Validating(object sender, CancelEventArgs e)
        {
            if (MyUtility.Check.Empty(this.txtsubconSupplier.TextBox1.Text))
            {
                this.CurrentMaintain["LocalSuppID"] = DBNull.Value;
            }
            #region supplier有調整需清空表身price
            if (!this.isNeedPlanningB03Quote)
            {
                return;
            }

            foreach (DataRow dr in this.DetailDatas)
            {
                string spNo = dr["orderid"].ToString();
                if (!this.IsSampleOrder(spNo))
                {
                    dr["unitprice"] = 0;
                }
            }

            #endregion
        }

        private bool IsSampleOrder(string spNo)
        {
            string sqlCheckSampleOrder = $@"
select 1
from orders with (nolock)
where id = '{spNo}' and Category = 'S'
";
            return MyUtility.Check.Seek(sqlCheckSampleOrder, null);
        }

        /// <summary>
        /// 異常價格紀錄重新整理
        /// </summary>
        /// <param name="showMSG">showMSG</param>
        private void RefreshIrregularPriceReason(bool showMSG = false)
        {
            DataTable detailDatas = ((DataTable)this.detailgridbs.DataSource).Clone();

            foreach (DataRow dr in ((DataTable)this.detailgridbs.DataSource).Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    detailDatas.ImportRow(dr);
                }
            }

            var irregularPriceReason = new Sci.Production.Subcon.P01_IrregularPriceReason(this.CurrentMaintain["ID"].ToString(), this.CurrentMaintain["FactoryID"].ToString(), this.CurrentMaintain, detailDatas);

            P01.Tmp_ModifyTable = null;

            // P01_IrregularPriceReason.tmp_IrregularPriceReason_List.Clear();
            bool has_Irregular_Price = false;

            if (MyUtility.Check.Empty(this.CurrentMaintain["ID"]))
            {
                has_Irregular_Price = irregularPriceReason.Check_Irregular_Price_Without_PO(false);
            }
            else
            {
                has_Irregular_Price = irregularPriceReason.Check_Irregular_Price(false);
            }

            this.IrregularPriceReason_ReasonNullCount = irregularPriceReason.ReasonNullCount;
            this.HideWaitMessage();

            if (has_Irregular_Price)
            {
                this.btnIrrPriceReason.Enabled = true;
                this.btnIrrPriceReason.ForeColor = Color.Red;

                if (showMSG)
                {
                    MyUtility.Msg.WarningBox("There is Irregular Price!! Please fix it.");
                }
            }
            else
            {
                // this.btnIrrPriceReason.Enabled = false;
                this.btnIrrPriceReason.ForeColor = Color.Black;
            }
        }

        private void Txtartworktype_ftyArtworkType_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            this.txtartworktype_ftyArtworkType.ValidateControl();
        }
    }
}
