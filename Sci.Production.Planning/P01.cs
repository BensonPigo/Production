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
        public P01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            //this.DefaultFilter = "Finished = 0 and IsForecast = 0";
            this.DetailSelectCommand = @"select convert(date,null) as mockupdate,order_tmscost.*
                                                                    ,osp.poqty osp_qty,osp.farmout as osp_farmout,osp.farmin as osp_farmin
                                                                    ,osp.poqty inhouse_qty,osp.farmout as inhouse_farmout,osp.farmin as inhouse_farmin
                                                                    ,(select localsupp.abb from localsupp where id = order_tmscost.localsuppid) as localsuppname
                                                            from order_tmscost 
                                                            inner join artworktype on order_tmscost.artworktypeid = artworktype.id 
                                                          left join (select artworkpo.artworktypeid,orderid,poqty,Farmout,farmin 
                                                                         from artworkpo,artworkpo_detail 
                                                                        where artworkpo.potype = 'O' and artworkpo.id = artworkpo_detail.id 
                                                                            and artworkpo_detail.orderid = @id and artworkpo.status = 'Approved'  group by artworkpo.artworktypeid,orderid,poqty,Farmout,farmin) osp
                                                                on Order_TmsCost.id = osp.orderid and osp.artworktypeid = Order_TmsCost.ArtworkTypeID
                                                            left join (select artworkpo.artworktypeid,orderid,poqty,Farmout,farmin 
                                                                         from artworkpo,artworkpo_detail 
                                                                        where artworkpo.potype = 'I' and artworkpo.id = artworkpo_detail.id 
                                                                            and artworkpo_detail.orderid = @id and artworkpo.status = 'Approved'  group by artworkpo.artworktypeid,orderid,poqty,Farmout,farmin) inhouse
                                                                on Order_TmsCost.id = inhouse.orderid and inhouse.artworktypeid = Order_TmsCost.ArtworkTypeID
                                                         where order_tmscost.id = @id and (order_tmscost.qty > 0 or order_tmscost.tms >0 )
                                                            and artworktype.isSubprocess = 1;--";
        }
        public P01(ToolStripMenuItem menuitem ,bool history): base(menuitem)
        {
            InitializeComponent();
            if (history)
            {
                this.DefaultFilter = "Finished = 1 and IsForecast = 0";
            }
            else
            {
                this.DefaultFilter = "Finished = 0 and IsForecast = 0";
            }

            this.DetailSelectCommand = @"select order_tmscost.*
                                                                    ,osp.poqty osp_qty,osp.farmout as osp_farmout,osp.farmin as osp_farmin
                                                                    ,osp.poqty inhouse_qty,osp.farmout as inhouse_farmout,osp.farmin as inhouse_farmin
                                                                    ,(select localsupp.abb from localsupp where id = order_tmscost.localsuppid) as localsuppname
                                                            from order_tmscost 
                                                            inner join artworktype on order_tmscost.artworktypeid = artworktype.id 
                                                          left join (select artworkpo.artworktypeid,orderid,poqty,Farmout,farmin 
                                                                         from artworkpo,artworkpo_detail 
                                                                        where artworkpo.potype = 'O' and artworkpo.id = artworkpo_detail.id 
                                                                            and artworkpo_detail.orderid = @id and artworkpo.status = 'Approved'  group by artworkpo.artworktypeid,orderid,poqty,Farmout,farmin) osp
                                                                on Order_TmsCost.id = osp.orderid and osp.artworktypeid = Order_TmsCost.ArtworkTypeID
                                                            left join (select artworkpo.artworktypeid,orderid,poqty,Farmout,farmin 
                                                                         from artworkpo,artworkpo_detail 
                                                                        where artworkpo.potype = 'I' and artworkpo.id = artworkpo_detail.id 
                                                                            and artworkpo_detail.orderid = @id and artworkpo.status = 'Approved'  group by artworkpo.artworktypeid,orderid,poqty,Farmout,farmin) inhouse
                                                                on Order_TmsCost.id = inhouse.orderid and inhouse.artworktypeid = Order_TmsCost.ArtworkTypeID
                                                         where order_tmscost.id = @id and (order_tmscost.qty > 0 or order_tmscost.tms >0 )
                                                            and artworktype.isSubprocess = 1;--";
        }

        //refresh
        protected override void OnDetailEntered()
        {
            DataRow dr;
            bool result;
            
            base.OnDetailEntered();
            
            if (!(CurrentMaintain==null))
            {
                result = myUtility.Seek(string.Format(@"select isnull(sum(qty),0) as cutqty from cuttingOutput_detail_detail where CuttingID='{0}'", CurrentMaintain["id"]), out dr, null);
                if (result) numericBox_cutqty.Value = (decimal)dr[0];
                dr = null;
                result = myUtility.Seek(string.Format(@"select isnull(sum(workday),0) as workday from sewingschedule where orderid ='{0}'", CurrentMaintain["id"]), out dr, null);
                if (result) numericBox_NeedPerDay.Value = decimal.Parse(dr[0].ToString());
            }
            button_batchApprove.Enabled = !this.EditMode;
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
                                                                                            from ORDERS 
                                                                                            INNER JOIN Style_Artwork SA ON ORDERS.StyleUkey = SA.StyleUkey
                                                                                            LEFT JOIN Style_Artwork_Quot QU ON QU.Ukey = SA.Ukey
                                                                                            WHERE PriceApv ='Y' AND MOCKUP IS NOT NULL 
                                                                                                AND ORDERS.ID = '{0}' 
                                                                                                and SA.ArtworkTypeID ='{1}' 
                                                                                                and qu.localsuppid = '{2}'  "
                                                                                        , dr["ID"]
                                                                                        , dr["Artworktypeid"]
                                                                                        , dr["localsuppid"]), out order_dt);
                    if (order_dt.Rows.Count > 0 && !myUtility.Empty(order_dt.Rows[0]["mockup"]))
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
            #region Supplier 右鍵開窗
            ts4.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right && CurrentDetailData["InhouseOSP"].ToString() == "O")
                {
                    Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem
                        (string.Format(@"SELECT QU.LocalSuppId,LOCALSUPP.Abb,QU.Mockup
                                                    FROM Order_TmsCost OT
                                                    INNER JOIN ORDERS ON OT.ID = ORDERS.ID
                                                    INNER JOIN Style_Artwork SA ON OT.ArtworkTypeID = SA.ArtworkTypeID AND ORDERS.StyleUkey = SA.StyleUkey
                                                    LEFT JOIN Style_Artwork_Quot QU ON QU.Ukey = SA.Ukey
                                                    INNER JOIN LocalSupp ON LocalSupp.ID = QU.LocalSuppId
                                                    WHERE PriceApv ='Y' AND MOCKUP IS NOT NULL AND OT.ID = '{0}' AND OT.ARTWORKTYPEID='{1}'
                                                    GROUP BY QU.LocalSuppId,LOCALSUPP.Abb,QU.Mockup", CurrentDetailData["ID"], CurrentDetailData["Artworktypeid"])
                                                                                                                                                                                     , "10,15,12", null,null);
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    CurrentDetailData["localsuppid"] = item.GetSelectedString();
                }
            };
            #endregion

            ts4.CellValidating+=(s,e)=>
            {
                if (this.EditMode && !myUtility.Empty(e.FormattedValue))
                {
                    if (CurrentDetailData["InhouseOSP"].ToString() == "O")
                    {
                        bool exist =false;
                        DualResult result = DBProxy.Current.Exists(null, string.Format(@"SELECT QU.LocalSuppId,LOCALSUPP.Abb,QU.Mockup
                                                    FROM Order_TmsCost OT
                                                    INNER JOIN ORDERS ON OT.ID = ORDERS.ID
                                                    INNER JOIN Style_Artwork SA ON OT.ArtworkTypeID = SA.ArtworkTypeID AND ORDERS.StyleUkey = SA.StyleUkey
                                                    LEFT JOIN Style_Artwork_Quot QU ON QU.Ukey = SA.Ukey
                                                    INNER JOIN LocalSupp ON LocalSupp.ID = QU.LocalSuppId
                                                    WHERE PriceApv ='Y' AND MOCKUP IS NOT NULL AND OT.ID = '{0}' 
                                                        AND OT.ARTWORKTYPEID = '{1}' AND qu.Localsuppid = '{2}'
                                                    GROUP BY QU.LocalSuppId,LOCALSUPP.Abb,QU.Mockup"
                                                    , CurrentDetailData["ID"], CurrentDetailData["Artworktypeid"],e.FormattedValue), null, out exist);
                        if (!exist)
                        {
                            e.Cancel = true;
                            myUtility.WarningBox("Supplier not in Style Quotation or not Mock approved or not Price approved!!", "Warning");
                            return;
                        }
                    }
                }
            };

            Ict.Win.UI.DataGridViewComboBoxColumn col_inhouse_osp;
            Dictionary<String, String> comboBox1_RowSource = new Dictionary<string, string>();
            comboBox1_RowSource.Add("O", "OSP");
            comboBox1_RowSource.Add("I", "InHouse");

            Ict.Win.DataGridViewGeneratorComboBoxColumnSettings cs = new DataGridViewGeneratorComboBoxColumnSettings();
            cs.CellValidating += (s, e) =>
            {
                if (this.EditMode && !myUtility.Empty(e.FormattedValue) && myUtility.Empty(CurrentDetailData["localsuppid"]))
                {
                    if (e.FormattedValue.ToString() == "O")
                    {
                        CurrentDetailData["localsuppid"] = myUtility.Lookup(string.Format(@"SELECT top 1 QU.LocalSuppId
                                                    FROM Order_TmsCost OT
                                                    INNER JOIN ORDERS ON OT.ID = ORDERS.ID
                                                    INNER JOIN Style_Artwork SA ON OT.ArtworkTypeID = SA.ArtworkTypeID AND ORDERS.StyleUkey = SA.StyleUkey
                                                    LEFT JOIN Style_Artwork_Quot QU ON QU.Ukey = SA.Ukey
                                                    INNER JOIN LocalSupp ON LocalSupp.ID = QU.LocalSuppId
                                                    WHERE PriceApv ='Y' AND MOCKUP IS NOT NULL AND OT.ID = '{0}' 
                                                        AND OT.ARTWORKTYPEID = '{1}' 
                                                    GROUP BY QU.LocalSuppId,LOCALSUPP.Abb,QU.Mockup"
                                                    , CurrentDetailData["ID"], CurrentDetailData["Artworktypeid"]), null);
                        CurrentDetailData["inhouseOSP"] = e.FormattedValue;
                    }
                    if (e.FormattedValue.ToString() == "I")
                    {
                        CurrentDetailData["localsuppid"] = Env.User.Factory;
                        CurrentDetailData["inhouseOSP"] = e.FormattedValue;
                    }
                }
            };

            #region 欄位設定
            Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("artworktypeid", header: "Artwork Type", width: Widths.AnsiChars(20), iseditingreadonly: true, settings: ts4)  //0
            .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(6), iseditingreadonly: true)    //1
            .Text("artworkunit", header: "Unit", width: Widths.AnsiChars(6), iseditingreadonly: true, settings: ts4)  //2
            .Numeric("tms", header: "TMS", width: Widths.AnsiChars(6), iseditingreadonly: true)    //3
            .ComboBox("InhouseOSP", header: "InHouse OSP",settings:cs).Get(out col_inhouse_osp)  //4
            .Text("localsuppid", header: "Subcon",settings:ts4)  //5
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

        private void button_batchApprove_Click(object sender, EventArgs e)
        {
            DataRow dr = grid.GetDataRow<DataRow>(grid.GetSelectedRowIndex());
            var frm = new Sci.Production.Planning.P01_BatchApprove(dr["ID"].ToString());
            frm.ShowDialog(this);
            this.RenewData();
        }
    }
}
