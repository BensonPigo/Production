using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.Planning
{
    /// <summary>
    /// P01
    /// </summary>
    public partial class P01 : Win.Tems.Input6
    {
        private bool firstTime = true;
        private bool data_overload = false;
        private string overload_date;

        /// <summary>
        /// P01
        /// </summary>
        /// <param name="menuitem">PlanningP01</param>
        public P01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.detailgrid.CellValueChanged += new DataGridViewCellEventHandler(this.ComboxChange);
            DataTable dtFty;
            DualResult result;
            string ftyFilter = string.Empty;
            if (result = DBProxy.Current.Select(string.Empty, $@"select id from Factory where IsProduceFty = 1", out dtFty))
            {
                if (dtFty != null || dtFty.Rows.Count > 0)
                {
                    for (int i = 0; i < dtFty.Rows.Count; i++)
                    {
                        ftyFilter += $"'{dtFty.Rows[i]["id"]}',";
                    }
                }
            }

            this.DefaultFilter = $@"(Junk=0 or (Junk=1 and NeedProduction=1)) and (category ='B' or category='S') and Finished = 0 and IsForecast = 0 and FactoryID in ({ftyFilter.Substring(0, ftyFilter.Length - 1)})";
            this.firstTime = false;
        }

        /// <summary>
        /// P01
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        /// <param name="history">history</param>
        public P01(ToolStripMenuItem menuitem, string history)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.detailgrid.CellValueChanged += new DataGridViewCellEventHandler(this.ComboxChange);
            DataTable dtFty;
            DualResult result;
            string ftyFilter = string.Empty;
            if (result = DBProxy.Current.Select(string.Empty, $@"select id from Factory where IsProduceFty = 1", out dtFty))
            {
                if (dtFty != null || dtFty.Rows.Count > 0)
                {
                    for (int i = 0; i < dtFty.Rows.Count; i++)
                    {
                        ftyFilter += $"'{dtFty.Rows[i]["id"]}',";
                    }
                }
            }

            if (history.ToUpper() == "Y")
            {
                this.DefaultFilter = $@"qty > 0 and (category ='B' or category='S') and Finished = 1 and IsForecast = 0 and FactoryID in ({ftyFilter.Substring(0, ftyFilter.Length - 1)}) ";

                // 因為在x86 run 有記憶體限制，如果筆數超過12萬筆就加入時間條件add_date兩年內的
                DataRow dr;
                if (MyUtility.Check.Seek("select count(*),FORMAT(dateadd(YEAR,-2,DATEADD(DD, DATEDIFF(DD, 0, GETDATE()), 0)),'yyyy/MM/dd') from orders where " + this.DefaultFilter, out dr))
                {
                    if ((int)dr[0] > 120000)
                    {
                        this.DefaultFilter = this.DefaultFilter + @" and AddDate >= dateadd(YEAR,-2,DATEADD(DD, DATEDIFF(DD, 0, GETDATE()), 0)) ";
                        this.data_overload = true;
                        this.overload_date = dr[1].ToString();
                    }
                }
            }
            else
            {
                this.DefaultFilter = $@"qty > 0 and (category ='B' or category='S') and Finished = 0 and IsForecast = 0 and FactoryID in ({ftyFilter.Substring(0, ftyFilter.Length - 1)}) ";
            }

            this.Text = "P01 Sub-process master list (History)";
            this.btnBatchApprove.Enabled = false;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            if (this.data_overload)
            {
                MyUtility.Msg.InfoBox(string.Format("Due to excessive data, data show nearly two years only(AddDate from {0}).\r\nFilter other Data, please use the LOCATE function.", this.overload_date));
            }
        }

        private void ComboxChange(object o, DataGridViewCellEventArgs e)
        {
            if (this.EditMode && e.ColumnIndex == 4)
            {
                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                if (dr["inhouseosp"].ToString() == "O")
                {
                    string localSuppId = string.Format(
                            @"
SELECT top 1 QU.LocalSuppId
FROM Order_TmsCost OT WITH (NOLOCK)
INNER JOIN ORDERS WITH (NOLOCK) ON OT.ID = ORDERS.ID
INNER JOIN Style_Artwork SA WITH (NOLOCK) ON OT.ArtworkTypeID = SA.ArtworkTypeID AND ORDERS.StyleUkey = SA.StyleUkey
LEFT JOIN Style_Artwork_Quot QU WITH (NOLOCK) ON QU.Ukey = SA.Ukey
INNER JOIN LocalSupp WITH (NOLOCK) ON LocalSupp.ID = QU.LocalSuppId
WHERE OT.ARTWORKTYPEID = '{1}' 
GROUP BY QU.LocalSuppId,LOCALSUPP.Abb,QU.Mockup",
                            this.CurrentDetailData["ID"],
                            this.CurrentDetailData["Artworktypeid"]);
                    dr["localsuppid"] = MyUtility.GetValue.Lookup(localSuppId, null);
                }

                if (dr["inhouseosp"].ToString() == "I")
                {
                    dr["localsuppid"] = Env.User.Factory;
                }
            }
        }

        /// <summary>
        /// OnDetailEntered
        /// </summary>
        protected override void OnDetailEntered()
        {
            DataRow dr;
            bool result;

            base.OnDetailEntered();

            if (!(this.CurrentMaintain == null))
            {
                result = MyUtility.Check.Seek(string.Format(@"select isnull(sum(qty),0) as cutqty from cuttingOutput_detail_detail WITH (NOLOCK) where CuttingID='{0}'", this.CurrentMaintain["id"]), out dr, null);
                if (result)
                {
                    this.numCutQty.Value = (decimal)dr[0];
                }

                dr = null;
                result = MyUtility.Check.Seek(string.Format(@"select isnull(sum(workday),0) as workday from sewingschedule WITH (NOLOCK) where orderid ='{0}'", this.CurrentMaintain["id"]), out dr, null);
                if (result)
                {
                    this.numNeedPerDay.Value = decimal.Parse(dr[0].ToString());
                }
            }

            this.btnBatchApprove.Enabled = !this.EditMode;
            this.detailgrid.AutoResizeColumns();

            DataTable cutDate_dt;
            string cmd;
            cmd = string.Format(
                @"
                 SELECT DISTINCT C.FirstCutDate FROM Cutting C
                 LEFT JOIN Orders O ON C.ID=O.CuttingSP
                 WHERE O.ID='{0}'", this.CurrentMaintain["id"]);
            DualResult res;
            res = DBProxy.Current.Select(null, cmd, out cutDate_dt);
            if (cutDate_dt.Rows.Count == 0)
            {
                return;
            }

            this.dateFirstCutDate.Value = MyUtility.Convert.GetDate(cutDate_dt.Rows[0]["FirstCutDate"].ToString());
        }

        /// <summary>
        /// OnRenewDataDetailPost
        /// </summary>
        /// <param name="e">e</param>
        /// <returns>DualResult</returns>
        protected override DualResult OnRenewDataDetailPost(RenewDataPostEventArgs e)
        {
            if (!this.tabs.TabPages[0].Equals(this.tabs.SelectedTab))
            {
                foreach (DataRow dr in e.Details.Rows)
                {
                    DataTable order_dt;
                    string mockup = string.Format(
                        @"
Select max(mockup) as mockup
from ORDERS WITH (NOLOCK)
INNER JOIN Style_Artwork SA WITH (NOLOCK) ON ORDERS.StyleUkey = SA.StyleUkey
LEFT JOIN Style_Artwork_Quot QU WITH (NOLOCK) ON QU.Ukey = SA.Ukey
WHERE PriceApv ='Y' AND MOCKUP IS NOT NULL 
AND ORDERS.ID = '{0}' 
and SA.ArtworkTypeID ='{1}' 
and qu.localsuppid = '{2}'  ",
                        dr["ID"],
                        dr["Artworktypeid"],
                        dr["localsuppid"]);
                    DBProxy.Current.Select(null, mockup, out order_dt);
                    if (order_dt.Rows.Count > 0 && !MyUtility.Check.Empty(order_dt.Rows[0]["mockup"]))
                    {
                        dr["mockupdate"] = order_dt.Rows[0]["mockup"];
                    }
                }
            }

            return base.OnRenewDataDetailPost(e);
        }

        /// <summary>
        /// OnDetailGridSetup
        /// </summary>
        protected override void OnDetailGridSetup()
        {
            DataGridViewGeneratorTextColumnSettings ts4 = new DataGridViewGeneratorTextColumnSettings();
            #region Supplier 右鍵開窗 & 按下subcon欄位自動帶值
            ts4.CellMouseClick += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                if (this.EditMode && this.CurrentDetailData["localsuppid"].ToString() != string.Empty)
                {
                    return;
                }

                // [InHouse OSP] = InHouse時，按下[Subcon]儲存格，自動帶出[Subcon]及[Subcon Name]該有的值
                if (this.EditMode && e.Button == MouseButtons.Left)
                {
                    if (this.CurrentDetailData["InhouseOSP"].ToString() == "I")
                    {
                        DataTable dt;
                        this.CurrentDetailData["localsuppid"] = Env.User.Factory;
                        string subconName = string.Format(
                            @"
                            SELECT l.Abb
                            FROM Order_tmscost ot WITH (NOLOCK)
                            left join LocalSupp l WITH (NOLOCK) on l.ID=ot.LocalSuppID
                            where ot.LocalSuppID='{0}' ", this.CurrentDetailData["localsuppid"]);
                        DualResult result = DBProxy.Current.Select(null, subconName, out dt);
                        if (dt.Rows.Count == 0)
                        {
                            this.CurrentDetailData["localsuppname"] = string.Empty;
                            return;
                        }

                        this.CurrentDetailData["localsuppname"] = dt.Rows[0]["Abb"].ToString();
                    }
                }
            };

            ts4.EditingMouseDown += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    Win.Tools.SelectItem item;
                    string sqlcmd;
                    if (this.CurrentDetailData["InhouseOSP"].ToString() == "O")
                    {
                        DataTable dtSelectSupp = null;
                        sqlcmd = string.Format(
                            @"

if (exists (select 1 from ArtworkType where ID = '{1}' and IsArtwork = 1))
begin
	SELECT DISTINCT	QU.LocalSuppId
			, l.Abb
			, QU.Mockup
	FROM Order_TmsCost OT WITH (NOLOCK)
	INNER JOIN ORDERS WITH (NOLOCK) ON OT.ID = ORDERS.ID
	INNER JOIN Style_Artwork SA WITH (NOLOCK) ON OT.ArtworkTypeID = SA.ArtworkTypeID AND ORDERS.StyleUkey = SA.StyleUkey
	LEFT JOIN Style_Artwork_Quot QU WITH (NOLOCK) ON QU.Ukey = SA.Ukey
	INNER JOIN LocalSupp l WITH (NOLOCK) ON l.ID = QU.LocalSuppId
	WHERE OT.ID = '{0}' AND OT.ARTWORKTYPEID='{1}'
	GROUP BY QU.LocalSuppId,l.Abb,QU.Mockup
end
else
begin
	select LocalSuppID = l.ID
			, Abb = l.Abb
			, Mockup = NULL
	from LocalSupp l
	where l.junk = 0
end;",
                            this.CurrentDetailData["ID"],
                            this.CurrentDetailData["Artworktypeid"]);

                        DualResult resule = DBProxy.Current.Select(string.Empty, sqlcmd, out dtSelectSupp);
                        if (resule == true)
                        {
                            item = new Win.Tools.SelectItem(dtSelectSupp, "LocalSuppID,Abb,MockUp", "10,15,12", null, null, null);
                            DialogResult result = item.ShowDialog();
                            if (result == DialogResult.Cancel)
                            {
                                return;
                            }

                            IList<DataRow> x = item.GetSelecteds();
                            this.CurrentDetailData["localsuppid"] = x[0][0];
                            this.CurrentDetailData["localsuppname"] = x[0][1];
                            this.CurrentDetailData["mockupdate"] = x[0][2];
                        }
                        else
                        {
                            MyUtility.Msg.WarningBox(resule.Description);
                        }
                    }
                    else
                    {
                        sqlcmd = @"select DISTINCT l.ID ,l.Abb ,l.Name from dbo.LocalSupp l WITH (NOLOCK)  WHERE l.Junk=0 and IsFactory = 1 order by ID";
                        item = new Win.Tools.SelectItem(sqlcmd, "10,30", null);
                        DialogResult result = item.ShowDialog();
                        if (result == DialogResult.Cancel)
                        {
                            return;
                        }

                        IList<DataRow> x = item.GetSelecteds();
                        this.CurrentDetailData["localsuppid"] = x[0][0];
                        this.CurrentDetailData["localsuppname"] = x[0][1];
                    }

                    this.CurrentDetailData.EndEdit();
                }
            };

            #endregion

            ts4.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                if (this.EditMode && !MyUtility.Check.Empty(e.FormattedValue))
                {
                    if (this.CurrentDetailData["InhouseOSP"].ToString() == "O")
                    {
                        bool exist = false;
                        string exists = string.Format(
                            @"
if (exists (select 1 from ArtworkType where ID = '{1}' and IsArtwork = 1))
begin
	SELECT DISTINCT QU.LocalSuppId
			, l.Abb
			, QU.Mockup
	FROM Order_TmsCost OT WITH (NOLOCK)
	INNER JOIN ORDERS WITH (NOLOCK) ON OT.ID = ORDERS.ID
	INNER JOIN Style_Artwork SA WITH (NOLOCK) ON OT.ArtworkTypeID = SA.ArtworkTypeID AND ORDERS.StyleUkey = SA.StyleUkey
	LEFT JOIN Style_Artwork_Quot QU WITH (NOLOCK) ON QU.Ukey = SA.Ukey
	INNER JOIN LocalSupp l WITH (NOLOCK) ON l.ID = QU.LocalSuppId
	WHERE OT.ID = '{0}' 
			AND OT.ARTWORKTYPEID = '{1}' 
			AND qu.Localsuppid = '{2}'
	GROUP BY QU.LocalSuppId,l.Abb,QU.Mockup
end
else
begin
	select LocalSuppID = l.ID
			, Abb = l.Abb
			, Mockup = ''
	from LocalSupp l
	where l.junk = 0 and l.ID = '{2}'
end;",
                            this.CurrentDetailData["ID"],
                            this.CurrentDetailData["Artworktypeid"],
                            e.FormattedValue);
                        DualResult result = DBProxy.Current.Exists(null, exists, null, out exist);
                        if (!exist)
                        {
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox("Supplier not in Style Quotation or not Mock approved or not Price approved!!", "Warning");
                            return;
                        }
                    }
                }

                if (this.CurrentDetailData["InhouseOSP"].ToString() == "O" && e.FormattedValue.ToString() == string.Empty)
                {
                    this.CurrentDetailData["localsuppname"] = string.Empty;
                    this.CurrentDetailData["localsuppid"] = string.Empty;
                    if (this.CurrentDetailData["mockupdate"].ToString() != string.Empty)
                    {
                        this.CurrentDetailData["mockupdate"] = DBNull.Value;
                    }
                }

                if (this.CurrentDetailData["InhouseOSP"].ToString() == "I" && e.FormattedValue.ToString() == string.Empty)
                {
                    this.CurrentDetailData["localsuppname"] = string.Empty;
                    this.CurrentDetailData["localsuppid"] = string.Empty;
                }
            };

            Ict.Win.UI.DataGridViewComboBoxColumn col_inhouse_osp;
            Dictionary<string, string> comboBox1_RowSource = new Dictionary<string, string>();
            comboBox1_RowSource.Add("O", "OSP");
            comboBox1_RowSource.Add("I", "InHouse");

            DataGridViewGeneratorComboBoxColumnSettings cs = new DataGridViewGeneratorComboBoxColumnSettings();
            cs.EditingControlShowing += (sender, eventArgs) =>
                {
                    if (sender == null || eventArgs == null)
                    {
                        return;
                    }

                    var e = (Ict.Win.UI.DataGridViewEditingControlShowingEventArgs)eventArgs;
                    ComboBox cb = e.Control as ComboBox;
                    if (cb != null)
                    {
                        cb.SelectionChangeCommitted -= this.Cb_SelectionChangeCommitted1;
                        cb.SelectionChangeCommitted += this.Cb_SelectionChangeCommitted1;
                    }
                };

            #region 欄位設定
            this.Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("artworktypeid", header: "Artwork Type", width: Widths.AnsiChars(20), iseditingreadonly: true)
            .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Text("artworkunit", header: "Unit", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Numeric("tms", header: "TMS", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .ComboBox("InhouseOSP", header: "InHouse OSP", settings: cs).Get(out col_inhouse_osp)
            .Text("localsuppid", header: "Subcon", settings: ts4)
            .Text("localsuppname", header: "Subcon Name", iseditingreadonly: true)
            .Date("mockupdate", header: "Mock-up", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Numeric("osp_qty", header: "OSP Qty", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Numeric("osp_farmout", header: "OSP Farm Out", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Numeric("osp_farmin", header: "OSP Farm In", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Numeric("inhouse_qty", header: "InHouse Qty", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Numeric("inhouose_farmout", header: "InHouse Farm Out", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Numeric("inhouose_farmin", header: "InHouse Farm In", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Date("ArtworkInLine", header: "InLine", width: Widths.AnsiChars(10))
            .Date("ArtworkOffLine", header: "OffLine", width: Widths.AnsiChars(10))
            .Date("apvdate", header: "Approve Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("apvname", header: "Approve Name", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("EditName", header: "Edit Name", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Date("EditDate", header: "Edit Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
            ;
            #endregion

            col_inhouse_osp.DataSource = new BindingSource(comboBox1_RowSource, null);
            col_inhouse_osp.ValueMember = "Key";
            col_inhouse_osp.DisplayMember = "Value";

            #region 可編輯欄位變色
            this.detailgrid.Columns[4].DefaultCellStyle.BackColor = Color.Pink;  // PCS/Stitch
            this.detailgrid.Columns[5].DefaultCellStyle.BackColor = Color.Pink;  // Cutpart Name
            this.detailgrid.Columns[14].DefaultCellStyle.BackColor = Color.Pink; // Unit Price
            this.detailgrid.Columns[15].DefaultCellStyle.BackColor = Color.Pink; // Qty/GMT
            #endregion
        }

        private void Cb_SelectionChangeCommitted1(object sender, EventArgs e)
        {
            // 下拉選項是InHouse時，subcon & subcon name自動帶出，若為OSP時 帶出 subcon & subcon name
            string newValue = ((Ict.Win.UI.DataGridViewComboBoxEditingControl)sender).EditingControlFormattedValue.ToString();
            if (newValue == "I")
            {
                DataTable dt;

                this.CurrentDetailData["localsuppid"] = Env.User.Factory;
                string sub = this.CurrentDetailData["localsuppid"].ToString();
                string subconName = string.Format(
                    @"
                            SELECT distinct l.Abb
                            FROM Order_tmscost ot WITH (NOLOCK)
                            left join LocalSupp l WITH (NOLOCK) on l.ID=ot.LocalSuppID
                            where ot.LocalSuppID='{0}'", sub);
                DualResult result = DBProxy.Current.Select(null, subconName, out dt);
                if (dt.Rows.Count == 0)
                {
                    this.CurrentDetailData["localsuppname"] = string.Empty;
                    this.CurrentDetailData["mockupdate"] = DBNull.Value;
                    this.CurrentDetailData["inhouseOSP"] = newValue;
                    return;
                }

                this.CurrentDetailData["localsuppname"] = dt.Rows[0]["Abb"].ToString();
                this.CurrentDetailData["mockupdate"] = DBNull.Value;
            }
            else if (newValue == "O")
            {
                DataTable dt;
                string suppidAndName = string.Format(
                    @"SELECT top 1 QU.LocalSuppId,LocalSupp.Abb,QU.Mockup
                                                    FROM Order_TmsCost OT WITH (NOLOCK)
                                                    INNER JOIN ORDERS WITH (NOLOCK) ON OT.ID = ORDERS.ID
                                                    INNER JOIN Style_Artwork SA WITH (NOLOCK) ON OT.ArtworkTypeID = SA.ArtworkTypeID AND ORDERS.StyleUkey = SA.StyleUkey
                                                    LEFT JOIN Style_Artwork_Quot QU WITH (NOLOCK) ON QU.Ukey = SA.Ukey
                                                    INNER JOIN LocalSupp WITH (NOLOCK) ON LocalSupp.ID = QU.LocalSuppId
                                                    WHERE PriceApv ='Y' AND MOCKUP IS NOT NULL AND OT.ID = '{0}' 
                                                        AND OT.ARTWORKTYPEID = '{1}' 
                                                    GROUP BY QU.LocalSuppId,LOCALSUPP.Abb,QU.Mockup",
                    this.CurrentDetailData["ID"],
                    this.CurrentDetailData["Artworktypeid"]);
                DualResult result = DBProxy.Current.Select(null, suppidAndName, out dt);
                if (dt.Rows.Count == 0)
                {
                    this.CurrentDetailData["localsuppname"] = string.Empty;
                    this.CurrentDetailData["localsuppid"] = string.Empty;
                    this.CurrentDetailData["mockupdate"] = DBNull.Value;
                    this.CurrentDetailData["inhouseOSP"] = newValue;
                    return;
                }

                this.CurrentDetailData["localsuppname"] = dt.Rows[0]["Abb"].ToString();
                this.CurrentDetailData["localsuppid"] = dt.Rows[0]["LocalSuppId"].ToString();
                this.CurrentDetailData["mockupdate"] = dt.Rows[0]["Mockup"].ToString();
            }

            this.CurrentDetailData["inhouseOSP"] = newValue;
        }

        private void BtnBatchApprove_Click(object sender, EventArgs e)
        {
            var frm = new P01_BatchApprove();
            frm.ShowDialog(this);
            this.RenewData();
        }

        /// <summary>
        /// OnEditModeChanged
        /// </summary>
        protected override void OnEditModeChanged()
        {
            base.OnEditModeChanged();
            if (!this.firstTime)
            {
                this.txtcountryDestination.TextBox1.ReadOnly = true;
                this.txtuserPPICMR.TextBox1.ReadOnly = true;
            }
        }

        /// <summary>
        /// OnDetailSelectCommandPrepare
        /// </summary>
        /// <param name="e">e</param>
        /// <returns>DualResult</returns>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();

            this.DetailSelectCommand = string.Format(
                @"select convert(date,null) as mockupdate,order_tmscost.*
                                                                    ,osp.poqty osp_qty,osp.farmout as osp_farmout,osp.farmin as osp_farmin
                                                                    ,osp.poqty inhouse_qty,osp.farmout as inhouse_farmout,osp.farmin as inhouse_farmin
                                                                    ,(select localsupp.abb from localsupp WITH (NOLOCK) where id = order_tmscost.localsuppid) as localsuppname
                                                            from order_tmscost WITH (NOLOCK) 
                                                            inner join artworktype WITH (NOLOCK) on order_tmscost.artworktypeid = artworktype.id 
                                                          left join (select artworkpo.artworktypeid,orderid,poqty,Farmout,farmin 
                                                                         from artworkpo WITH (NOLOCK) ,artworkpo_detail WITH (NOLOCK) 
                                                                        where artworkpo.potype = 'O' and artworkpo.id = artworkpo_detail.id 
                                                                            and artworkpo_detail.orderid = '{0}' and artworkpo.status = 'Approved'  group by artworkpo.artworktypeid,orderid,poqty,Farmout,farmin) osp
                                                                on Order_TmsCost.id = osp.orderid and osp.artworktypeid = Order_TmsCost.ArtworkTypeID
                                                            left join (select artworkpo.artworktypeid,orderid,poqty,Farmout,farmin 
                                                                         from artworkpo WITH (NOLOCK) ,artworkpo_detail WITH (NOLOCK) 
                                                                        where artworkpo.potype = 'I' and artworkpo.id = artworkpo_detail.id 
                                                                            and artworkpo_detail.orderid = '{0}' and artworkpo.status = 'Approved'  group by artworkpo.artworktypeid,orderid,poqty,Farmout,farmin) inhouse
                                                                on Order_TmsCost.id = inhouse.orderid and inhouse.artworktypeid = Order_TmsCost.ArtworkTypeID
                                                         where order_tmscost.id = '{0}' and (order_tmscost.qty > 0 or order_tmscost.tms >0 )
                                                            and artworktype.isSubprocess = 1;--", masterID);

            return base.OnDetailSelectCommandPrepare(e);
        }

        protected override DualResult ClickSave()
        {
            // 修改表身資料,不寫入表頭EditName and EditDate
            ITableSchema pass1Schema;
            var ok = DBProxy.Current.GetTableSchema(null, "Orders", out pass1Schema);
            pass1Schema.IsSupportEditDate = false;
            pass1Schema.IsSupportEditName = false;

            return Result.True;
        }
    }
}
