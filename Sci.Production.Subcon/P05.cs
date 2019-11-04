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
using System.Data.SqlClient;
using Sci.Win;
using System.Reflection;
using System.Transactions;

namespace Sci.Production.Subcon
{
    public partial class P05 : Sci.Win.Tems.Input6
    {
        string artworkunit;
        Form batchapprove;


        public P05(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.DefaultFilter = $"MdivisionID = '{Sci.Env.User.Keyword}'";
            gridicon.Append.Enabled = false;
            gridicon.Append.Visible = false;
            gridicon.Insert.Enabled = false;
            gridicon.Insert.Visible = false;

            txtsubconSupplier.TextBox1.Validated += (s, e) =>
            {
                this.CurrentMaintain["localsuppid"] = txtsubconSupplier.TextBox1.Text;
            };

            grid.ColumnHeaderMouseClick += (s, e) =>
            {
                ChangeBrowseColor();
            };
        }



        /// <summary>
        /// Change Browse Color 
        /// 變更Browse 整行顏色
        /// </summary>
        private void ChangeBrowseColor()
        {
            for (int i = 0; i < grid.Rows.Count; i++)
            {
                DataTable dt = (DataTable)this.gridbs.DataSource;
                DataRow dataRow = dt.Rows[i];
                if (!MyUtility.Check.Empty(dataRow["exceed"]))
                {
                    this.grid.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                }
                else
                {
                    this.grid.Rows[i].DefaultCellStyle.BackColor = Color.White;
                }

            }
            grid.Columns["exceed"].Visible = false;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.queryfors.SelectedIndexChanged += (s, e) =>
            {
                string isExceed = MyUtility.Check.Empty(this.queryfors.SelectedValue) ? string.Empty : this.queryfors.SelectedValue.ToString();
                switch (isExceed)
                {
                    case "1":
                    this.DefaultWhere = "Exceed = 1";
                        this.grid.RowsDefaultCellStyle.BackColor = Color.Yellow;
                        this.ReloadDatas();
                        break;
                    default:
                        this.DefaultWhere = string.Empty;
                        this.ReloadDatas();
                        ChangeBrowseColor();
                        break;
                }
                
            };
            ChangeBrowseColor();

            this.reloaddata.Click += (s, e) =>
            {
                ChangeBrowseColor();
            };
        }

        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["ID"].ToString();

            this.DetailSelectCommand = string.Format(@"
select 
ap.OrderID
,o.StyleID
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
from dbo.ArtworkReq_Detail ap
left join dbo.Orders o on ap.OrderID = o.id
left join dbo.ArtworkPO_Detail apo on apo.ID = ap.ArtworkPOID
where ap.id = '{0}'  
ORDER BY ap.OrderID   ", masterID);

            return base.OnDetailSelectCommandPrepare(e);
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            ChangeDetailHeader();
            
            if (this.CurrentMaintain["ID"] == DBNull.Value)
            {
                btnIrrQtyReason.Enabled = false;
            }
            else
            {
                btnIrrQtyReason.Enabled = true;
            }

            #region Status Label
            labStatus.Text = CurrentMaintain["Status"].ToString();
            #endregion
            #region exceed status
            labExceed.Visible = CurrentMaintain["Exceed"].ToString().ToUpper() == "TRUE";
            #endregion
            #region Batch Import, Special record button
            btnBatchImport.Enabled = this.EditMode;
            #endregion
            #region Batch create
            btnBatchCreate.Enabled = !this.EditMode;
            #endregion

            #region Irregular 判斷
            RefreshIrregularQtyReason();
            #endregion

            #region 表尾
            string dateDeptApv = MyUtility.Check.Empty(MyUtility.Convert.GetDate(CurrentMaintain["DeptApvDate"])) ? "" : ((DateTime)MyUtility.Convert.GetDate(CurrentMaintain["DeptApvDate"])).ToString("yyyy/MM/dd HH:mm:ss");


            dispDeptApv.Text = CurrentMaintain["DeptApvName"].ToString() + " - " + MyUtility.GetValue.Lookup("Name", CurrentMaintain["DeptApvName"].ToString(), "Pass1", "ID") + dateDeptApv;

            string dateMgApv = MyUtility.Check.Empty(MyUtility.Convert.GetDate(CurrentMaintain["MgApvDate"])) ? "" : ((DateTime)MyUtility.Convert.GetDate(CurrentMaintain["MgApvDate"])).ToString("yyyy/MM/dd HH:mm:ss");

            dispMgrApv.Text = CurrentMaintain["MgApvName"].ToString() + " - " + MyUtility.GetValue.Lookup("Name", CurrentMaintain["MgApvName"].ToString(), "Pass1", "ID") + dateMgApv;

            string dateClose = MyUtility.Check.Empty(MyUtility.Convert.GetDate(CurrentMaintain["CloseUnCloseDate"])) ? "" : ((DateTime)MyUtility.Convert.GetDate(CurrentMaintain["CloseUnCloseDate"])).ToString("yyyy/MM/dd HH:mm:ss");

            dispClosed.Text = CurrentMaintain["CloseUnCloseName"].ToString() + " - " + MyUtility.GetValue.Lookup("Name", CurrentMaintain["CloseUnCloseName"].ToString(), "Pass1", "ID") + dateClose;
            #endregion
        }

        protected override void EnsureToolbarExt()
        {
            base.EnsureToolbarExt();
            #region 動態ToolBar 參數

            if (!this.EditMode && this.CurrentMaintain != null)
            {
                // Close ChkValue
                if (string.Compare(this.CurrentMaintain["Status"].ToString(), "Closed", true) != 0 && !IsDetailInserting)
                {
                    this.toolbar.cmdClose.Enabled = true;
                }
                else
                {
                    this.toolbar.cmdClose.Enabled = false;
                }

                // unApv ChkValue
                if (string.Compare(this.CurrentMaintain["Status"].ToString(), "Approved", true) == 0 && !MyUtility.Check.Empty(CurrentMaintain["Exceed"]))
                {
                    this.toolbar.cmdUnconfirm.Enabled = true;
                }
                else
                {
                    this.toolbar.cmdUnconfirm.Enabled = false;
                }

                // unCheck ChkValue
                if ((string.Compare(this.CurrentMaintain["Status"].ToString(), "Locked", true) == 0 && !MyUtility.Check.Empty(CurrentMaintain["Exceed"]))
                    || (string.Compare(this.CurrentMaintain["Status"].ToString(), "Approved", true) == 0 && MyUtility.Check.Empty(CurrentMaintain["Exceed"]))
                    )
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
        protected override void OnDetailGridSetup()
        {
            Ict.Win.DataGridViewGeneratorNumericColumnSettings col_ReqQty = new DataGridViewGeneratorNumericColumnSettings();
            col_ReqQty.CellValidating += (s, e) =>
            {
                if (this.EditMode && e.FormattedValue != null)
                {
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    string sqlcmd = $@"
select  OrderQty = sum(q.qty)  
, [AccReqQty] = isnull(ReqQty.value,0) + isnull(PoQty.value,0) + {e.FormattedValue}
from  Order_TmsCost ot
inner join orders o WITH (NOLOCK) on ot.ID = o.ID
inner join order_qty q WITH (NOLOCK) on q.id = o.ID
left join dbo.View_Order_Artworks oa on oa.ID = o.ID AND OA.Article = Q.Article AND OA.SizeCode=Q.SizeCode
left join dbo.View_Style_Artwork vsa on	vsa.StyleUkey = o.StyleUkey 
	and vsa.Article = oa.Article and vsa.ArtworkID = oa.ArtworkID 
	and vsa.ArtworkName = oa.ArtworkName and vsa.ArtworkTypeID = oa.ArtworkTypeID 
	and vsa.PatternCode = oa.PatternCode and vsa.PatternDesc = oa.PatternDesc 
left join Style_Artwork_Quot sao with (nolock) on sao.Ukey = vsa.StyleArtworkUkey and sao.PriceApv = 'Y' and sao.Price > 0
and sao.LocalSuppId = '{CurrentMaintain["localsuppid"]}' 
left join ArtworkType at WITH (NOLOCK) on at.id = oa.ArtworkTypeID
inner join factory f WITH (NOLOCK) on o.factoryid=f.id
outer apply (
        select value = ISNULL(sum(ReqQty),0)
        from ArtworkReq_Detail AD, ArtworkReq a
        where ad.ID=a.ID
		and a.ArtworkTypeID = '{CurrentMaintain["ArtworktypeId"]}' 
		and OrderID = o.ID and ad.PatternCode= isnull(oa.PatternCode,'')
        and ad.PatternDesc = isnull(oa.PatternDesc,'') and ad.ArtworkID = iif(oa.ArtworkID is null,ot.ArtworkTypeID,oa.ArtworkID)
        and a.status != 'Closed' and ad.ArtworkPOID =''
        and a.id != '{dr["id"]}'
) ReqQty
outer apply (
        select value = ISNULL(sum(PoQty),0)
        from ArtworkPO_Detail AD,ArtworkPO A
        where a.ID=ad.ID
		and a.ArtworkTypeID = isnull(ot.ArtworkTypeID,'{CurrentMaintain["ArtworktypeId"]}')
		and OrderID = o.ID and ad.PatternCode= isnull(oa.PatternCode,'')
        and ad.PatternDesc = isnull(oa.PatternDesc,'') and ad.ArtworkID = iif(oa.ArtworkID is null,ot.ArtworkTypeID,oa.ArtworkID)
		and ad.ArtworkReqID=''
) PoQty
where f.IsProduceFty=1
and o.category in ('B','S')
and o.MDivisionID='{Sci.Env.User.Keyword}' 
and ot.ArtworkTypeID like '{CurrentMaintain["artworktypeid"]}%' 
and o.Junk=0
and o.id = '{dr["OrderID"]}'
and isnull(oa.PatternCode,'') = '{dr["PatternCode"]}'
and isnull(oa.PatternDesc,'') = '{dr["PatternDesc"]}'
and isnull(oa.ArtworkID,ot.ArtworkTypeID) = '{dr["ArtworkId"]}'
and ((o.Category = 'B' and  ot.InhouseOSP = 'O') or (o.category = 'S'))
group by ReqQty.value,PoQty.value";
                    DataRow drQty;                    
                    if (MyUtility.Check.Seek(sqlcmd , out drQty))
                    {
                        CurrentDetailData["exceedqty"] = ((decimal)drQty["AccReqQty"] - (int)drQty["OrderQty"]) < 0 ? 0 : (decimal)drQty["AccReqQty"] - (int)drQty["OrderQty"];
                    }

                    CurrentDetailData["ReqQty"] = e.FormattedValue;
                    RefreshIrregularQtyReason();
                }

            };

            #region 欄位設定
            Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("orderid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("StyleID", header: "Style", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Date("sewinline", header: "Sewing Inline", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Date("scidelivery", header: "SCI Delivery", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("ArtworkId", header: "Artwork", width: Widths.AnsiChars(17), iseditingreadonly: true)
            .Text("patterncode", header: "Cut Part", width: Widths.AnsiChars(5), iseditingreadonly: true)
            .Text("PatternDesc", header: "Cut Part Name", width: Widths.AnsiChars(20), iseditingreadonly: true)
            .Numeric("ReqQty", header: "Req. Qty", width: Widths.AnsiChars(6) , settings: col_ReqQty) // 可編輯
            .Numeric("stitch", header: "PCS/Stitch", width: Widths.AnsiChars(3))// 可編輯
            .Numeric("qtygarment", header: "Qty/GMT", width: Widths.AnsiChars(5), maximum: 99, integer_places: 2) // 可編輯
            .Text("Farmout", header: "Farm Out", width: Widths.AnsiChars(5), iseditingreadonly: true)
            .Text("Farmin", header: "Farm In", width: Widths.AnsiChars(5), iseditingreadonly: true)
            .Text("ApQty", header: "A/P Qty", width: Widths.AnsiChars(5), iseditingreadonly: true)
            .Text("exceedqty", header: "Exceed Qty", width: Widths.AnsiChars(5), iseditingreadonly: true)
            .Text("ArtworkPOID", header: "Subcon PO#", width: Widths.AnsiChars(13), iseditingreadonly: true)
            ;
            #endregion
            #region 可編輯欄位變色
            detailgrid.Columns["ReqQty"].DefaultCellStyle.BackColor = Color.Pink;
            detailgrid.Columns["stitch"].DefaultCellStyle.BackColor = Color.Pink;
            detailgrid.Columns["qtygarment"].DefaultCellStyle.BackColor = Color.Pink;
            #endregion

        }

        // 新增時預設資料
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["MdivisionID"] = Sci.Env.User.Keyword;
            CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;
            CurrentMaintain["Reqdate"] = System.DateTime.Today;
            CurrentMaintain["handle"] = Sci.Env.User.UserID;
            CurrentMaintain["Status"] = "New";
            ((DataTable)(detailgridbs.DataSource)).Rows[0].Delete();
        }

        // edit前檢查
        protected override bool ClickEditBefore()
        {   
            if (CurrentMaintain["Status"].ToString() != "New")
            {
                MyUtility.Msg.WarningBox("The record status is not new, can't modify !!");
                return false;
            }

            return base.ClickEditBefore();
        }

        // save前檢查 & 取id
        protected override bool ClickSaveBefore()
        {

            #region 必輸檢查

            if (CurrentMaintain["ReqDate"] == DBNull.Value || string.IsNullOrWhiteSpace(CurrentMaintain["ReqDate"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Req. Date >  canot be empty!", "Warning");
                dateReqDate.Focus();
                return false;
            }

            if (CurrentMaintain["ArtworktypeId"] == DBNull.Value || string.IsNullOrWhiteSpace(CurrentMaintain["ArtworktypeId"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Artwork Type >  canot be empty!", "Warning");
                txtartworktype_ftyArtworkType.Focus();
                return false;
            }

            if (CurrentMaintain["LocalSuppID"] == DBNull.Value || string.IsNullOrWhiteSpace(CurrentMaintain["LocalSuppID"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Supplier >  canot be empty!", "Warning");
                txtsubconSupplier.TextBox1.Focus();
                return false;
            }

            if (CurrentMaintain["Handle"] == DBNull.Value || string.IsNullOrWhiteSpace(CurrentMaintain["Handle"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Handle >  canot be empty!", "Warning");
                txtuserHandle.TextBox1.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["factoryid"]))
            {
                MyUtility.Msg.WarningBox("< Factory >  canot be empty!", "Warning");
                txtmfactory.Focus();
                return false;
            }
           
            #endregion

            foreach (DataRow row in ((DataTable)detailgridbs.DataSource).Select("ReqQty = 0"))
            {
                row.Delete();
            }

            if (DetailDatas.Count == 0)
            {
                MyUtility.Msg.WarningBox("Detail can't be empty", "Warning");
                return false;
            }

            bool isDetailExceedQtyNotZero = this.DetailDatas.Any(s => !MyUtility.Check.Empty(s["ExceedQty"]));
            if (isDetailExceedQtyNotZero)
            {
                CurrentMaintain["Exceed"] = 1;
            }
            else
            {
                CurrentMaintain["Exceed"] = 0;
            }

            // 判斷irregular Reason沒寫不能存檔
            var IrregularQtyReason = new Sci.Production.Subcon.P05_IrregularQtyReason(this.CurrentMaintain["ID"].ToString(), this.CurrentMaintain, (DataTable)detailgridbs.DataSource);

            DataTable dtIrregular = IrregularQtyReason.Check_Irregular_Qty();
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
                CurrentMaintain["id"] = Sci.MyUtility.GetValue.GetID(Sci.Env.User.Factory + "OR", "artworkReq", (DateTime)CurrentMaintain["ReqDate"]);
            }

            return base.ClickSaveBefore();
        }

        protected override void ClickCheck()
        {
            base.ClickCheck();
            DualResult result;
            string sqlcmd;
            string strStatus;

            if (MyUtility.Check.Empty(CurrentMaintain["Exceed"]))
            {
                strStatus = "Approved";
            }
            else
            {
                strStatus = "Locked";
            }

            // 判斷irregular Reason沒寫不能存檔
            var IrregularQtyReason = new Sci.Production.Subcon.P05_IrregularQtyReason(this.CurrentMaintain["ID"].ToString(), this.CurrentMaintain, (DataTable)detailgridbs.DataSource);

            DataTable dtIrregular = IrregularQtyReason.Check_Irregular_Qty();
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
where id = '{CurrentMaintain["id"]}'";
            if (!(result = DBProxy.Current.Execute(null, sqlcmd)))
            {
                ShowErr(sqlcmd, result);
                return;
            }
            MyUtility.Msg.InfoBox("Successfully");
        }

        protected override void ClickUncheck()
        {
            base.ClickUncheck();
            DualResult result;
            String sqlcmd;

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
where id = '{CurrentMaintain["id"]}'";

            if (!(result = DBProxy.Current.Execute(null, sqlcmd)))
            {
                ShowErr(sqlcmd, result);
                return;
            }
            MyUtility.Msg.InfoBox("Successfully");
        }

        protected override void ClickConfirm()
        {
            DualResult result;

            string sqlcmd;
            sqlcmd = $@"
update artworkReq 
set status='Approved'
, MgApvName = '{Env.User.UserID}', MgApvDate = GETDATE() 
, editname='{Env.User.UserID}', editdate = GETDATE() 
where id = '{CurrentMaintain["id"]}'";

            if (!(result = DBProxy.Current.Execute(null, sqlcmd)))
            {
                ShowErr(sqlcmd, result);
                return;
            }
            MyUtility.Msg.InfoBox("Successfully");
            base.ClickConfirm();
        }

        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            DualResult result;
            String sqlcmd;

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
where id = '{CurrentMaintain["id"]}'";
            if (!(result = DBProxy.Current.Execute(null, sqlcmd)))
            {
                ShowErr(sqlcmd, result);
                return;
            }
            MyUtility.Msg.InfoBox("Successfully");
        }

        protected override void ClickClose()
        {
            base.ClickClose();
            String sqlcmd;
            sqlcmd = $@"
update artworkReq 
set status = 'Closed'
, OriStatus = Status
, CloseUnCloseName = '{Env.User.UserID}', CloseUnCloseDate = GETDATE()
, editname='{Env.User.UserID}', editdate = GETDATE() 
where id = '{CurrentMaintain["id"]}'";

            DualResult result;
            if (!(result = DBProxy.Current.Execute(null, sqlcmd)))
            {
                ShowErr(sqlcmd, result);
                return;
            }
            MyUtility.Msg.InfoBox("Successfully");
        }

        protected override void ClickUnclose()
        {
            base.ClickUnclose();
          
            String sqlcmd;           
             sqlcmd = $@"
update artworkReq 
set status = OriStatus
, CloseUnCloseName = '{Env.User.UserID}', CloseUnCloseDate = GETDATE()
, editname='{Env.User.UserID}', editdate = GETDATE() 
where id = '{CurrentMaintain["id"]}'";

            DualResult result;
            if (!(result = DBProxy.Current.Execute(null, sqlcmd)))
            {
                ShowErr(sqlcmd, result);
                return;
            }
            MyUtility.Msg.InfoBox("Successfully");
        }

        protected override void ClickUndo()
        {
            base.ClickUndo();
            this.RenewData();
        }

        protected override bool ClickDeleteBefore()
        {
            if (CurrentMaintain["Status"].ToString() != "New")
            {
                MyUtility.Msg.WarningBox("Data is Approved or Closed cannot delete.", "Warning");
                return false;
            }
            return base.ClickDeleteBefore();
        }

        //print
        protected override bool ClickPrint()
        {
            decimal totalReqQty = 0;
            #region -- 加總明細金額，顯示於表頭 --
            if (!(CurrentMaintain == null))
            {
                
                foreach (DataRow drr in ((DataTable)detailgridbs.DataSource).Rows)
                {
                    totalReqQty += (decimal)drr["ReqQty"];
                }
            }
            #endregion

            //跳轉至PrintForm
            Sci.Production.Subcon.P05_Print callPrintForm = new Sci.Production.Subcon.P05_Print(this.CurrentMaintain, totalReqQty.ToString());
            callPrintForm.ShowDialog(this);
            return true;
        }

        //batch import
        private void btnBatchImport_Click(object sender, EventArgs e)
        {
            var dr = CurrentMaintain; if (null == dr) return;
            if (MyUtility.Check.Empty(this.txtsubconSupplier.TextBox1.Text))
            {
                MyUtility.Msg.WarningBox("Please fill Supplier first!");
                txtsubconSupplier.TextBox1.Focus();
                return;
            }
            if (dr["artworktypeid"] == DBNull.Value)
            {
                MyUtility.Msg.WarningBox("Please fill Artworktype first!");
                txtartworktype_ftyArtworkType.Focus();
                return;
            }
            var frm = new Sci.Production.Subcon.P05_Import(dr, (DataTable)detailgridbs.DataSource);
            frm.ShowDialog(this);

            DataTable dg = (DataTable)detailgridbs.DataSource;
            if (dg.Columns["style"] == null) dg.Columns.Add("Style", typeof(String));
            if (dg.Columns["sewinline"] == null) dg.Columns.Add("sewinline", typeof(DateTime));
            if (dg.Columns["scidelivery"] == null) dg.Columns.Add("scidelivery", typeof(DateTime));
            foreach (DataRow drr in ((DataTable)detailgridbs.DataSource).Rows)
            {
                if (drr.RowState == DataRowState.Deleted) continue;
                DataTable order_dt;
                DBProxy.Current.Select(null, string.Format("select styleid, sewinline, scidelivery from orders WITH (NOLOCK) where id='{0}'", drr["orderid"].ToString()), out order_dt);
                if (order_dt.Rows.Count == 0)
                    break;
                drr["style"] = order_dt.Rows[0]["styleid"].ToString();
                drr["sewinline"] = order_dt.Rows[0]["sewinline"];
                drr["scidelivery"] = order_dt.Rows[0]["scidelivery"];
            }
            this.RenewData();

            #region 檢查異常價格

            RefreshIrregularQtyReason();

            #endregion

        }

        // batch create
        private void btnBatchCreate_Click(object sender, EventArgs e)
        {
            if (this.EditMode) return;
            var frm = new Sci.Production.Subcon.P05_BatchCreate();
            frm.ShowDialog(this);
            ReloadDatas();
            ChangeBrowseColor();
        }

        private void txtartworktype_ftyArtworkType_Validating(object sender, CancelEventArgs e)
        {
            Production.Class.txtartworktype_fty o;
            o = (Production.Class.txtartworktype_fty)sender;

            if ((o.Text != o.OldValue) && this.EditMode)
            {
                ((DataTable)detailgridbs.DataSource).Rows.Clear();
            }
            ChangeDetailHeader();

        }

        private void ChangeDetailHeader()
        {
            #region --動態unit header --
            artworkunit = MyUtility.GetValue.Lookup(string.Format("select artworkunit from artworktype WITH (NOLOCK) where id='{0}'", txtartworktype_ftyArtworkType.Text)).ToString().Trim();
            if (artworkunit == "") artworkunit = "PCS";
            this.detailgrid.Columns["stitch"].HeaderText = artworkunit;
            #endregion
        }


        private void btnIrrPriceReason_Click(object sender, EventArgs e)
        {
            if ((DataTable)detailgridbs.DataSource == null)
            {
                return;
            }

            DataTable detailDatas = ((DataTable)detailgridbs.DataSource).Clone();
            foreach (DataRow dr in ((DataTable)detailgridbs.DataSource).Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    detailDatas.ImportRow(dr);
                }

            }
            var frm = new Sci.Production.Subcon.P05_IrregularQtyReason(this.CurrentMaintain["ID"].ToString(), this.CurrentMaintain, detailDatas);
            frm.ShowDialog(this);

            //畫面關掉後，再檢查一次有無價格異常
            this.ShowWaitMessage("Data Loading...");
            RefreshIrregularQtyReason();
        }

        private void btnBatchApprove_Click(object sender, EventArgs e)
        {
            if (this.Perm.Confirm)
            {
                if (batchapprove == null || batchapprove.IsDisposed)
                {
                    batchapprove = new Sci.Production.Subcon.P05_BatchApprove(reload);
                    batchapprove.Show();
                }
                else
                {
                    batchapprove.Activate();
                }
            }
            else
            {
                MyUtility.Msg.WarningBox("You don't have permission to confirm.");
            }
        }

        public void reload()
        {
            if (this.CurrentDataRow != null)
            {
                string idIndex = string.Empty;
                if (!MyUtility.Check.Empty(CurrentMaintain))
                {
                    if (!MyUtility.Check.Empty(CurrentMaintain["id"]))
                    {
                        idIndex = MyUtility.Convert.GetString(CurrentMaintain["id"]);
                    }
                }
                this.ReloadDatas();
                this.RenewData();
                ChangeBrowseColor();
                if (!MyUtility.Check.Empty(idIndex)) this.gridbs.Position = this.gridbs.Find("ID", idIndex);
            }
        }

        private void P05_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (batchapprove != null)
            {
                batchapprove.Dispose();
            }
        }        

        private void txtsubconSupplier_Validating(object sender, CancelEventArgs e)
        {
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
        /// <param name="showMSG"></param>
        private void RefreshIrregularQtyReason()
        {
            if ((DataTable)detailgridbs.DataSource == null)
            {
                return;
            }

            DataTable detailDatas = ((DataTable)detailgridbs.DataSource).Clone();

            foreach (DataRow dr in ((DataTable)detailgridbs.DataSource).Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    detailDatas.ImportRow(dr);
                }

            }

            this.btnIrrQtyReason.ForeColor = Color.Black;
            var IrregularQtyReason = new Sci.Production.Subcon.P05_IrregularQtyReason(this.CurrentMaintain["ID"].ToString(), this.CurrentMaintain, detailDatas);

            DataTable dtIrregular = IrregularQtyReason.Check_Irregular_Qty();
            this.HideWaitMessage();

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
            MyUtility.Tool.SetupCombox(this.queryfors, 2, 1, ",,1,Irregular Qty");
            this.queryfors.SelectedIndex = 0;
        }
    }

}
