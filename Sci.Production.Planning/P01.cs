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


namespace Sci.Production.Planning
{
    public partial class P01 : Sci.Win.Tems.Input6
    {
        bool firstTime = true;
        public P01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.detailgrid.CellValueChanged += new DataGridViewCellEventHandler(ComboxChange);
            this.DefaultFilter = @"qty > 0 and (category ='B' or category='S') and Finished = 0 and IsForecast = 0 ";
            firstTime = false;
        }
        public P01(ToolStripMenuItem menuitem, string history)
            : base(menuitem)
        {
            InitializeComponent();
            this.detailgrid.CellValueChanged += new DataGridViewCellEventHandler(ComboxChange);
            if (history.ToUpper() == "Y")
            {
                this.DefaultFilter = @"qty > 0 and (category ='B' or category='S') and Finished = 1 and IsForecast = 0 ";
            }
            else
            {
                this.DefaultFilter = @"qty > 0 and (category ='B' or category='S') and Finished = 0 and IsForecast = 0 ";
            }
            this.Text = "P01 Sub-process master list (History)";
            this.IsSupportEdit = false;
            this.btnBatchApprove.Enabled = false;
        }

        private void ComboxChange(object o, DataGridViewCellEventArgs e)
        {

            if (this.EditMode && e.ColumnIndex == 4)
            {
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                if (dr["inhouseosp"].ToString() == "O")
                {
                    dr["localsuppid"] = MyUtility.GetValue.Lookup(string.Format(@"SELECT top 1 QU.LocalSuppId
                                                    FROM Order_TmsCost OT WITH (NOLOCK)
                                                    INNER JOIN ORDERS WITH (NOLOCK) ON OT.ID = ORDERS.ID
                                                    INNER JOIN Style_Artwork SA WITH (NOLOCK) ON OT.ArtworkTypeID = SA.ArtworkTypeID AND ORDERS.StyleUkey = SA.StyleUkey
                                                    LEFT JOIN Style_Artwork_Quot QU WITH (NOLOCK) ON QU.Ukey = SA.Ukey
                                                    INNER JOIN LocalSupp WITH (NOLOCK) ON LocalSupp.ID = QU.LocalSuppId
                                                    WHERE OT.ARTWORKTYPEID = '{1}' 
                                                    GROUP BY QU.LocalSuppId,LOCALSUPP.Abb,QU.Mockup"
                                                , CurrentDetailData["ID"], CurrentDetailData["Artworktypeid"]), null);

                }
                if (dr["inhouseosp"].ToString() == "I")
                {
                    dr["localsuppid"] = Env.User.Factory;
                }
            }
        }
        //refresh
        protected override void OnDetailEntered()
        {
            DataRow dr;
            bool result;

            base.OnDetailEntered();

            if (!(CurrentMaintain == null))
            {
                result = MyUtility.Check.Seek(string.Format(@"select isnull(sum(qty),0) as cutqty from cuttingOutput_detail_detail WITH (NOLOCK) where CuttingID='{0}'", CurrentMaintain["id"]), out dr, null);
                if (result) numCutQty.Value = (decimal)dr[0];
                dr = null;
                result = MyUtility.Check.Seek(string.Format(@"select isnull(sum(workday),0) as workday from sewingschedule WITH (NOLOCK) where orderid ='{0}'", CurrentMaintain["id"]), out dr, null);
                if (result) numNeedPerDay.Value = decimal.Parse(dr[0].ToString());
            }
            btnBatchApprove.Enabled = !this.EditMode;
            this.detailgrid.AutoResizeColumns();

            DataTable CutDate_dt;
            string cmd;
            cmd = string.Format(@"
                 SELECT DISTINCT C.FirstCutDate FROM Cutting C
                 LEFT JOIN Orders O ON C.ID=O.CuttingSP
                 WHERE O.ID='{0}'", CurrentMaintain["id"]);
            DualResult res;
            res = DBProxy.Current.Select(null, cmd, out CutDate_dt);
            if (CutDate_dt.Rows.Count == 0) { return; }
            dateFirstCutDate.Value = MyUtility.Convert.GetDate(CutDate_dt.Rows[0]["FirstCutDate"].ToString());

        }
        // grid 加工填值
        protected override DualResult OnRenewDataDetailPost(RenewDataPostEventArgs e)
        {
            if (!tabs.TabPages[0].Equals(tabs.SelectedTab))
            {
                //(e.Details).Columns.Add("mockupdate", typeof(DateTime));
                foreach (DataRow dr in e.Details.Rows)
                {
                    DataTable order_dt;
                    DBProxy.Current.Select(null, string.Format(@"Select max(mockup) as mockup
                                                                                            from ORDERS WITH (NOLOCK)
                                                                                            INNER JOIN Style_Artwork SA WITH (NOLOCK) ON ORDERS.StyleUkey = SA.StyleUkey
                                                                                            LEFT JOIN Style_Artwork_Quot QU WITH (NOLOCK) ON QU.Ukey = SA.Ukey
                                                                                            WHERE PriceApv ='Y' AND MOCKUP IS NOT NULL 
                                                                                                AND ORDERS.ID = '{0}' 
                                                                                                and SA.ArtworkTypeID ='{1}' 
                                                                                                and qu.localsuppid = '{2}'  "
                                                                                        , dr["ID"]
                                                                                        , dr["Artworktypeid"]
                                                                                        , dr["localsuppid"]), out order_dt);
                    if (order_dt.Rows.Count > 0 && !MyUtility.Check.Empty(order_dt.Rows[0]["mockup"]))
                    {
                        dr["mockupdate"] = order_dt.Rows[0]["mockup"];
                    }
                }
            }
            return base.OnRenewDataDetailPost(e);
        }

        // Detail Grid 設定
        protected override void OnDetailGridSetup()
        {

            Ict.Win.DataGridViewGeneratorTextColumnSettings ts4 = new DataGridViewGeneratorTextColumnSettings();
            #region Supplier 右鍵開窗 & 按下subcon欄位自動帶值
            ts4.CellMouseClick += (s, e) => {


                if (!this.EditMode) { return; } //原本有值就不做任何事
                if (this.EditMode && CurrentDetailData["localsuppid"].ToString() != "") { return; }//原本有值就不做任何事
              
                //[InHouse OSP] = InHouse時，按下[Subcon]儲存格，自動帶出[Subcon]及[Subcon Name]該有的值
                if (this.EditMode && e.Button == MouseButtons.Left)
                {
                    if (CurrentDetailData["InhouseOSP"].ToString() == "I")
                    {
                        DataTable dt;
                        CurrentDetailData["localsuppid"] = Env.User.Factory;
                        string SubconName = string.Format(@"
                            SELECT top 1 l.Abb
                            FROM Order_tmscost ot WITH (NOLOCK)
                            left join LocalSupp l WITH (NOLOCK) on l.ID=ot.LocalSuppID
                            where ot.LocalSuppID='{0}'", CurrentDetailData["localsuppid"]);
                        DualResult result = DBProxy.Current.Select(null, SubconName, out dt);
                        if (dt.Rows.Count == 0)
                        { 
                            this.CurrentDetailData["localsuppname"]="";
                             return;
                        }
                        this.CurrentDetailData["localsuppname"] = dt.Rows[0]["Abb"].ToString();
                    }
                }
            };

            ts4.EditingMouseDown += (s, e) =>
            {
                if (!this.EditMode) { return; } //原本有值就不做任何事
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    Sci.Win.Tools.SelectItem item;
                    string sqlcmd;
                    if (CurrentDetailData["InhouseOSP"].ToString() == "O")
                    {
                        sqlcmd = string.Format(@"SELECT QU.LocalSuppId,LOCALSUPP.Abb,QU.Mockup
                                                    FROM Order_TmsCost OT WITH (NOLOCK)
                                                    INNER JOIN ORDERS WITH (NOLOCK) ON OT.ID = ORDERS.ID
                                                    INNER JOIN Style_Artwork SA WITH (NOLOCK) ON OT.ArtworkTypeID = SA.ArtworkTypeID AND ORDERS.StyleUkey = SA.StyleUkey
                                                    LEFT JOIN Style_Artwork_Quot QU WITH (NOLOCK) ON QU.Ukey = SA.Ukey
                                                    INNER JOIN LocalSupp WITH (NOLOCK) ON LocalSupp.ID = QU.LocalSuppId
                                                    WHERE OT.ID = '{0}' AND OT.ARTWORKTYPEID='{1}'
                                                    GROUP BY QU.LocalSuppId,LOCALSUPP.Abb,QU.Mockup", CurrentDetailData["ID"], CurrentDetailData["Artworktypeid"]);
                        item = new Sci.Win.Tools.SelectItem(sqlcmd, "10,15,12", null, null);
                        DialogResult result = item.ShowDialog();
                        if (result == DialogResult.Cancel) { return; }
                        IList<DataRow> x = item.GetSelecteds();
                        CurrentDetailData["localsuppid"] = x[0][0];
                        CurrentDetailData["localsuppname"] = x[0][1];
                        CurrentDetailData["mockupdate"] = x[0][2];
                    }
                    else
                    {
                        sqlcmd = "select id,abb from localsupp WITH (NOLOCK) where junk = 0 and IsFactory = 1 order by ID";
                        item = new Sci.Win.Tools.SelectItem(sqlcmd, "10,30", null);
                        DialogResult result = item.ShowDialog();
                        if (result == DialogResult.Cancel) { return; }
                        IList<DataRow> x = item.GetSelecteds();
                        CurrentDetailData["localsuppid"] = x[0][0];
                        CurrentDetailData["localsuppname"] = x[0][1];                    
                    }
                    this.CurrentDetailData.EndEdit();
                }
            };
           
            #endregion

            ts4.CellValidating += (s, e) =>
            {
                if (!this.EditMode) { return; } //原本有值就不做任何事
                if (this.EditMode && !MyUtility.Check.Empty(e.FormattedValue))
                {
                    if (CurrentDetailData["InhouseOSP"].ToString() == "O")
                    {
                        bool exist = false;
                        DualResult result = DBProxy.Current.Exists(null, string.Format(@"SELECT QU.LocalSuppId,LOCALSUPP.Abb,QU.Mockup
                                                    FROM Order_TmsCost OT WITH (NOLOCK)
                                                    INNER JOIN ORDERS WITH (NOLOCK) ON OT.ID = ORDERS.ID
                                                    INNER JOIN Style_Artwork SA WITH (NOLOCK) ON OT.ArtworkTypeID = SA.ArtworkTypeID AND ORDERS.StyleUkey = SA.StyleUkey
                                                    LEFT JOIN Style_Artwork_Quot QU WITH (NOLOCK) ON QU.Ukey = SA.Ukey
                                                    INNER JOIN LocalSupp WITH (NOLOCK) ON LocalSupp.ID = QU.LocalSuppId
                                                    WHERE OT.ID = '{0}' 
                                                        AND OT.ARTWORKTYPEID = '{1}' AND qu.Localsuppid = '{2}'
                                                    GROUP BY QU.LocalSuppId,LOCALSUPP.Abb,QU.Mockup"
                                                    , CurrentDetailData["ID"], CurrentDetailData["Artworktypeid"], e.FormattedValue), null, out exist);
                        if (!exist)
                        {
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox("Supplier not in Style Quotation or not Mock approved or not Price approved!!", "Warning");
                            return;
                        }  
                    }
                    
                }
                if (CurrentDetailData["InhouseOSP"].ToString() == "O" && e.FormattedValue.ToString() == "")
                {
                    CurrentDetailData["localsuppname"] = "";
                    CurrentDetailData["localsuppid"] = "";
                    if (CurrentDetailData["mockupdate"].ToString() != "") { CurrentDetailData["mockupdate"] = DBNull.Value; }
                }
                if (CurrentDetailData["InhouseOSP"].ToString() == "I" && e.FormattedValue.ToString() == "")
                {
                    CurrentDetailData["localsuppname"] = "";
                    CurrentDetailData["localsuppid"] = "";
                }
            };

            Ict.Win.UI.DataGridViewComboBoxColumn col_inhouse_osp;
            Dictionary<String, String> comboBox1_RowSource = new Dictionary<string, string>();
            comboBox1_RowSource.Add("O", "OSP");
            comboBox1_RowSource.Add("I", "InHouse");

            Ict.Win.DataGridViewGeneratorComboBoxColumnSettings cs = new DataGridViewGeneratorComboBoxColumnSettings();
            //下拉選項顯示
            cs.EditingControlShowing += (sender, eventArgs) =>
                {
                    if (sender == null || eventArgs == null) { return; }
                    var e = ((Ict.Win.UI.DataGridViewEditingControlShowingEventArgs)eventArgs);
                    ComboBox cb = e.Control as ComboBox;
                    if (cb != null)
                    {
                        cb.SelectionChangeCommitted -= Cb_SelectionChangeCommitted1; //清空
                        cb.SelectionChangeCommitted += Cb_SelectionChangeCommitted1;
                    }

                };

            #region 欄位設定
            Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("artworktypeid", header: "Artwork Type", width: Widths.AnsiChars(20), iseditingreadonly: true)  //0
            .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(6), iseditingreadonly: true)    //1
            .Text("artworkunit", header: "Unit", width: Widths.AnsiChars(6), iseditingreadonly: true)  //2
            .Numeric("tms", header: "TMS", width: Widths.AnsiChars(6), iseditingreadonly: true)    //3
            .ComboBox("InhouseOSP", header: "InHouse OSP", settings: cs).Get(out col_inhouse_osp)  //4
            .Text("localsuppid", header: "Subcon", settings: ts4)  //5
            .Text("localsuppname", header: "Subcon Name", iseditingreadonly: true)  //6
            .Date("mockupdate", header: "Mock-up", width: Widths.AnsiChars(10), iseditingreadonly: true)   //7
             .Numeric("osp_qty", header: "OSP Qty", width: Widths.AnsiChars(6), iseditingreadonly: true)    //8
             .Numeric("osp_farmout", header: "OSP Farm Out", width: Widths.AnsiChars(6), iseditingreadonly: true)   //9
             .Numeric("osp_farmin", header: "OSP Farm In", width: Widths.AnsiChars(6), iseditingreadonly: true)   //10
             .Numeric("inhouse_qty", header: "InHouse Qty", width: Widths.AnsiChars(6), iseditingreadonly: true)    //11
             .Numeric("inhouose_farmout", header: "InHouse Farm Out", width: Widths.AnsiChars(6), iseditingreadonly: true)   //12
             .Numeric("inhouose_farmin", header: "InHouse Farm In", width: Widths.AnsiChars(6), iseditingreadonly: true)   //13
            .Date("ArtworkInLine", header: "InLine", width: Widths.AnsiChars(10))   //14
            .Date("ArtworkOffLine", header: "OffLine", width: Widths.AnsiChars(10))   //15
            .Date("apvdate", header: "Approve Date", width: Widths.AnsiChars(10), iseditingreadonly: true)   //16
            .Text("apvname", header: "Approve Name", width: Widths.AnsiChars(8), iseditingreadonly: true)    //17
            ;     //18
            #endregion

            col_inhouse_osp.DataSource = new BindingSource(comboBox1_RowSource, null);
            col_inhouse_osp.ValueMember = "Key";
            col_inhouse_osp.DisplayMember = "Value";

            #region 可編輯欄位變色
            detailgrid.Columns[4].DefaultCellStyle.BackColor = Color.Pink;  //PCS/Stitch
            detailgrid.Columns[5].DefaultCellStyle.BackColor = Color.Pink;  //Cutpart Name
            detailgrid.Columns[14].DefaultCellStyle.BackColor = Color.Pink; //Unit Price
            detailgrid.Columns[15].DefaultCellStyle.BackColor = Color.Pink; //Qty/GMT
            #endregion
        }
        private void Cb_SelectionChangeCommitted1(object sender, EventArgs e)
        {
            //下拉選項是InHouse時，subcon & subcon name自動帶出，若為OSP時 帶出 subcon & subcon name
            string newValue = ((Ict.Win.UI.DataGridViewComboBoxEditingControl)(sender)).EditingControlFormattedValue.ToString();
            if ( newValue== "I")
            {
                DataTable dt;

                this.CurrentDetailData["localsuppid"] = Env.User.Factory;
                string sub = this.CurrentDetailData["localsuppid"].ToString();
                string SubconName = string.Format(@"
                            SELECT distinct l.Abb
                            FROM Order_tmscost ot WITH (NOLOCK)
                            left join LocalSupp l WITH (NOLOCK) on l.ID=ot.LocalSuppID
                            where ot.LocalSuppID='{0}'", sub);
                DualResult result = DBProxy.Current.Select(null, SubconName, out dt);
                if (dt.Rows.Count == 0)
                {
                    this.CurrentDetailData["localsuppname"] = "";
                    this.CurrentDetailData["mockupdate"] = DBNull.Value;
                    CurrentDetailData["inhouseOSP"] = newValue;
                    return;
                }
                this.CurrentDetailData["localsuppname"] = dt.Rows[0]["Abb"].ToString();
                this.CurrentDetailData["mockupdate"] = DBNull.Value;
            }
            else if (newValue == "O")
            {
                DataTable dt;
                string suppidAndName = string.Format(@"SELECT top 1 QU.LocalSuppId,LocalSupp.Abb,QU.Mockup
                                                    FROM Order_TmsCost OT WITH (NOLOCK)
                                                    INNER JOIN ORDERS WITH (NOLOCK) ON OT.ID = ORDERS.ID
                                                    INNER JOIN Style_Artwork SA WITH (NOLOCK) ON OT.ArtworkTypeID = SA.ArtworkTypeID AND ORDERS.StyleUkey = SA.StyleUkey
                                                    LEFT JOIN Style_Artwork_Quot QU WITH (NOLOCK) ON QU.Ukey = SA.Ukey
                                                    INNER JOIN LocalSupp WITH (NOLOCK) ON LocalSupp.ID = QU.LocalSuppId
                                                    WHERE PriceApv ='Y' AND MOCKUP IS NOT NULL AND OT.ID = '{0}' 
                                                        AND OT.ARTWORKTYPEID = '{1}' 
                                                    GROUP BY QU.LocalSuppId,LOCALSUPP.Abb,QU.Mockup"
                                                    , CurrentDetailData["ID"], CurrentDetailData["Artworktypeid"]);
                DualResult result = DBProxy.Current.Select(null, suppidAndName, out dt);
                if (dt.Rows.Count == 0)
                {
                    this.CurrentDetailData["localsuppname"] = "";
                    this.CurrentDetailData["localsuppid"] = "";
                    this.CurrentDetailData["mockupdate"] = DBNull.Value;
                    CurrentDetailData["inhouseOSP"] = newValue;
                    return;
                }
                this.CurrentDetailData["localsuppname"] = dt.Rows[0]["Abb"].ToString();
                this.CurrentDetailData["localsuppid"] = dt.Rows[0]["LocalSuppId"].ToString();
                this.CurrentDetailData["mockupdate"] = dt.Rows[0]["Mockup"].ToString();

            }

            CurrentDetailData["inhouseOSP"] = newValue;
        }

        private void btnBatchApprove_Click(object sender, EventArgs e)
        {
            var frm = new Sci.Production.Planning.P01_BatchApprove();
            frm.ShowDialog(this);
            this.RenewData();
        }
       

        protected override void OnEditModeChanged()
        {
            base.OnEditModeChanged();
            if (!firstTime)
            {
                txtcountryDestination.TextBox1.ReadOnly = true;
                txtuserPPICMR.TextBox1.ReadOnly = true;
            }
            
        }

        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["ID"].ToString();

            this.DetailSelectCommand = string.Format(@"select convert(date,null) as mockupdate,order_tmscost.*
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
    }
}
