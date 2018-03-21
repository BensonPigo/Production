using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Sci.Production.Class;
using System.Linq;
using Ict.Win;
using System.Linq.Dynamic;
using Sci.Win.Tools;

namespace Sci.Production.Packing
{
    /// <summary>
    /// P17
    /// </summary>
    public partial class P17 : Sci.Win.Tems.QueryForm
    {
        private DataTable dt_scanDetail;
        private SelectCartonDetail selecedPK;

        /// <summary>
        /// P17
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P17(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.EditMode = true;
        }

        /// <summary>
        /// OnFormLoaded
        /// </summary>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.numBoxScanQty.ForeColor = Color.Red;
            this.gridSelectCartonDetail.DataSource = this.selcartonBS;
            this.Helper.Controls.Grid.Generator(this.gridSelectCartonDetail)
                .Text("ID", header: "Pack ID", width: Widths.AnsiChars(15))
                .Text("CTNStartNo", header: "CTN#", width: Widths.AnsiChars(6))
                .Text("OrderID", header: "SP#", width: Widths.AnsiChars(15))
                .Text("CustPoNo", header: "PO#", width: Widths.AnsiChars(17))
                .Text("Article", header: "Colorway", width: Widths.AnsiChars(8))
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(15))
                .Text("QtyPerCTN", header: "Qty", width: Widths.AnsiChars(13))
                .Text("ScanQty", header: "Scanned Qty", width: Widths.AnsiChars(13));

            this.gridScanDetail.DataSource = this.scanDetailBS;
            this.Helper.Controls.Grid.Generator(this.gridScanDetail)
                .Text("Article", header: "Colorway", width: Widths.AnsiChars(8))
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(15))
                .Numeric("QtyPerCTN", header: "Qty")
                .Text("Barcode", header: "Barcode", width: Widths.AnsiChars(15))
                .Numeric("ScanQty", header: "Scan Qty");
            this.Tab_Focus("CARTON");
        }

        private void TxtScanCartonSP_Validating(object sender, CancelEventArgs e)
        {
            DualResult result;

            // 檢查是否有正在掃packing未刷完
            if (!MyUtility.Check.Empty(this.txtScanCartonSP.OldValue) && this.numBoxScanQty.Value > 0)
            {
                if (MyUtility.Msg.InfoBox("Do you want to change CTN#?", buttons: MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }
            }

            this.ClearAll("SCAN");
            #region 檢查是否有資料，三個角度

            // 1.=PackingList_Detail.ID+PackingList_Detail.CTNStartNo
            // 2.=Orders.ID
            // 3.=Orders.CustPoNo
            string scanDetail_sql1 = $@"select distinct
                                           pd.ID,
                                           pd.CTNStartNo  ,
                                           pd.OrderID,
                                           o.CustPoNo ,
                                           pd.Article    ,
                                           pd.Color,
                                           pd.SizeCode  ,
                                           pd.QtyPerCTN,
                                          ScanQty = isnull(pd.ScanQty,0),
                                           pd.barcode,
                                           p.BrandID,
                                           o.StyleID,
                                           os.Seq,
                                           pd.Ukey
                                from PackingList_Detail pd WITH (NOLOCK)
                                inner join PackingList p WITH (NOLOCK) on p.ID = pd.ID
                                inner join Orders o WITH (NOLOCK) on o.ID = pd.OrderID
                                left join Order_SizeCode os WITH (NOLOCK) on os.id = o.POID and os.SizeCode = pd.SizeCode
                                where p.Type in ('B','L') and 
                                      (pd.ID + pd.CTNStartNo) = '{this.txtScanCartonSP.Text}'";

            string scanDetail_sql2 = $@"select distinct
                                           pd.ID,
                                           pd.CTNStartNo  ,
                                           pd.OrderID,
                                           o.CustPoNo ,
                                           pd.Article    ,
                                           pd.Color,
                                           pd.SizeCode  ,
                                           pd.QtyPerCTN,
                                          ScanQty = isnull(pd.ScanQty,0),
                                           pd.barcode,
                                           p.BrandID,
                                           o.StyleID,
                                           os.Seq,
                                           pd.Ukey
                                from PackingList_Detail pd WITH (NOLOCK)
                                inner join PackingList p WITH (NOLOCK) on p.ID = pd.ID
                                inner join Orders o WITH (NOLOCK) on o.ID = pd.OrderID
                                left join Order_SizeCode os WITH (NOLOCK) on os.id = o.POID and os.SizeCode = pd.SizeCode
                                where p.Type in ('B','L') and 
                                      o.ID = '{this.txtScanCartonSP.Text}' or
                                      o.CustPoNo = '{this.txtScanCartonSP.Text}'";

            if (result = DBProxy.Current.Select(null, scanDetail_sql1, out this.dt_scanDetail))
            {
                if (this.dt_scanDetail.Rows.Count == 0)
                {
                    if (result = DBProxy.Current.Select(null, scanDetail_sql2, out this.dt_scanDetail))
                    {
                        if (this.dt_scanDetail.Rows.Count == 0)
                        {
                            AutoClosingMessageBox.Show($"<{this.txtScanCartonSP.Text}> Invalid CTN#!!", "Warning", 3000);
                            e.Cancel = true;
                            return;
                        }
                    }
                    else
                    {
                        this.ShowErr(result);
                        return;
                    }
                }

                // 產生comboPKFilter資料
                List<string> srcPKFilter = new List<string>() { string.Empty };
                srcPKFilter.AddRange(this.dt_scanDetail.AsEnumerable().Select(s => s["ID"].ToString()).Distinct().ToList());
                this.comboPKFilter.DataSource = srcPKFilter;

                // 產生select Carton資料
                int cnt_selectCarton = this.LoadSelectCarton();

                if (cnt_selectCarton == 1)
                {
                    // 1.=PackingList_Detail.ID+PackingList_Detail.CTNStartNo
                    if (Convert.ToInt16(this.dt_scanDetail.Compute("Sum(ScanQty)", null)) > 0)
                    {
                        if (MyUtility.Msg.InfoBox("This carton had been scanned, are you sure you want to rescan again?", buttons: MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            if (!(result = this.ClearScanQty(this.dt_scanDetail.Select())))
                            {
                                this.ShowErr(result);
                                return;
                            }
                        }
                        else
                        {
                            e.Cancel = true;
                            return;
                        }
                    }

                    DualResult result_load = this.LoadScanDetail(0);
                    if (!result_load)
                    {
                        this.ShowErr(result_load);
                    }
                }
            }
            else
            {
                this.ShowErr(result);
                return;
            }

            #endregion
        }

        private void Tab_Focus(string type)
        {
            if (type.Equals("EAN"))
            {
                this.tabControlScanArea.SelectedTab = this.tabControlScanArea.TabPages[1];
                this.txtScanEAN.Focus();
                this.numBoxScanQty.Value = 0;
                this.numBoxScanTtlQty.Value = ((DataTable)this.scanDetailBS.DataSource).AsEnumerable().Sum(s => (int)s["QtyPerCTN"]);
            }
            else if (type.Equals("CARTON"))
            {
                this.tabControlScanArea.SelectedTab = this.tabControlScanArea.TabPages[0];
                this.txtScanCartonSP.Focus();
            }
        }

        private DualResult LoadScanDetail(int rowidx)
        {
            DualResult result = new DualResult(true);
            SelectCartonDetail dr = (SelectCartonDetail)this.gridSelectCartonDetail.GetData(rowidx);
            DataRow[] dr_scanDetail = this.dt_scanDetail.Select($"ID = '{dr.ID}' and CTNStartNo = '{dr.CTNStartNo}' and Article = '{dr.Article}'");
            if (dr_scanDetail.Where(s => (short)s["ScanQty"] > 0).Count() > 0)
            {
                if (MyUtility.Msg.InfoBox("Do you want to change CTN#?", buttons: MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (!(result = this.ClearScanQty(dr_scanDetail)))
                    {
                        this.ShowErr(result);
                        return result;
                    }
                }
                else
                {
                    return result;
                }
            }

            this.scanDetailBS.DataSource = dr_scanDetail.OrderBy(s => s["Article"]).ThenBy(s => s["Seq"]).CopyToDataTable();
            this.LoadHeadData(dr);
            this.Tab_Focus("EAN");
            return result;
        }

        private void GridSelectCartonDetail_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
           DualResult result = this.LoadScanDetail(e.RowIndex);
            if (!result)
            {
                this.ShowErr(result);
            }
        }

        /// <summary>
        /// 清空scan區資料(連同db)
        /// </summary>
        /// <param name="tmp">tmp</param>
        /// <returns>result</returns>
        private DualResult ClearScanQty(DataRow[] tmp)
        {
            DualResult result;
            foreach (DataRow dr in tmp)
            {
                dr["ScanQty"] = 0;
            }

            string upd_sql = $@"update PackingList_Detail set ScanQty = 0,ScanEditDate = NULL 
where ID = '{tmp[0]["ID"]}' and CTNStartNo = '{tmp[0]["CTNStartNo"]}' ";
            result = DBProxy.Current.Execute(null, upd_sql);
            this.LoadSelectCarton();
            return result;
        }

        /// <summary>
        /// 重新載入Select表身資料
        /// </summary>
        /// /// <param name="default_where">default_where</param>
        /// <returns>int</returns>
        private int LoadSelectCarton()
        {
            List<SelectCartonDetail> list_selectCarton = (from c in this.dt_scanDetail.AsEnumerable()
                                                          group c by new
                                                          {
                                                              ID = c.Field<string>("ID"),
                                                              CTNStartNo = c.Field<string>("CTNStartNo"),
                                                              CustPoNo = c.Field<string>("CustPoNo"),
                                                              Article = c.Field<string>("Article"),
                                                              BrandID = c.Field<string>("BrandID"),
                                                              StyleID = c.Field<string>("StyleID")
                                                          }
                                                          into g
                                                          select new SelectCartonDetail
                                                          {
                                                              ID = g.Key.ID,
                                                              CTNStartNo = g.Key.CTNStartNo,
                                                              CustPoNo = g.Key.CustPoNo,
                                                              Article = g.Key.Article,
                                                              BrandId = g.Key.BrandID,
                                                              StyleId = g.Key.StyleID,
                                                              OrderID = string.Join("/", g.Select(st => st.Field<string>("OrderID")).Distinct().ToArray()),
                                                              SizeCode = string.Join("/", g.Select(st => st.Field<string>("SizeCode")).ToArray()),
                                                              QtyPerCTN = string.Join("/", g.Select(st => st.Field<int>("QtyPerCTN").ToString()).ToArray()),
                                                              ScanQty = string.Join("/", g.Select(st => st.Field<short>("ScanQty").ToString()).ToArray()),
                                                              TtlScanQty = g.Sum(st => st.Field<short>("ScanQty"))
                                                          }).ToList();
            string default_where = string.Empty;

            // Only not yet scan complete checkbox
            if (this.chkBoxNotScan.Checked)
            {
                default_where = " and TtlScanQty = 0 ";
            }

            // Packing No Filter combobox
            if (!MyUtility.Check.Empty(this.comboPKFilter.SelectedText))
            {
                default_where += $" and ID = '{this.comboPKFilter.SelectedText}' ";
            }

            if (MyUtility.Check.Empty(default_where))
            {
                this.selcartonBS.DataSource = list_selectCarton;
            }
            else
            {
                default_where = " 1 = 1 " + default_where;
                this.selcartonBS.DataSource = list_selectCarton.Where(default_where);
            }

            return list_selectCarton.Count;
        }

        private void LoadHeadData(SelectCartonDetail dr)
        {
            string sum_sql = $@"select TtlCartons= sum(CTNQty),
                                       TtlQty = sum(ShipQty),
                                       TtlPackedCartons = sum(case when ScanQty > 0 and CTNQty>0 then 1 else 0 end),
                                       TtlPackQty = sum(ScanQty)
                                from PackingList_Detail
                                where id = '{dr.ID}'";
            DataRow dr_sum;
            MyUtility.Check.Seek(sum_sql, out dr_sum);

            this.displayPackID.Text = dr.ID;
            this.displayCtnNo.Text = dr.CTNStartNo;
            this.displaySPNo.Text = dr.OrderID;
            this.displayPoNo.Text = dr.CustPoNo;
            this.displayBrand.Text = dr.BrandId;
            this.displayStyle.Text = dr.StyleId;

            this.numBoxttlCatons.Text = dr_sum["TtlCartons"].ToString();
            this.numBoxttlQty.Text = dr_sum["TtlQty"].ToString();
            this.numBoxPackedCartons.Text = dr_sum["TtlPackedCartons"].ToString();
            this.numBoxttlPackQty.Text = ((int)dr_sum["TtlPackQty"] + this.numBoxScanQty.Value).ToString();
            this.numBoxRemainCartons.Text = ((int)dr_sum["TtlCartons"] - (int)dr_sum["TtlPackedCartons"]).ToString();
            this.numBoxRemainQty.Text = ((int)dr_sum["TtlQty"] - (int)dr_sum["TtlPackQty"] - this.numBoxScanQty.Value).ToString();

            this.selecedPK = dr;
        }

        private void ChkBoxNotScan_CheckedChanged(object sender, EventArgs e)
        {
            this.LoadSelectCarton();
        }

        private void ComboPKFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.LoadSelectCarton();
        }

        private void TxtQuickSelCTN_KeyUp(object sender, KeyEventArgs e)
        {
            if (this.gridSelectCartonDetail.RowCount > 0)
            {
                var obj = this.selcartonBS.List.OfType<SelectCartonDetail>().ToList().Find(f => f.CTNStartNo.Equals(this.txtQuickSelCTN.Text));
                var pos = this.selcartonBS.IndexOf(obj);
                this.selcartonBS.Position = pos;
            }
        }

        private void TxtScanEAN_Validating(object sender, CancelEventArgs e)
        {
            if (MyUtility.Check.Empty(this.txtScanEAN.Text))
            {
                return;
            }

            DualResult sql_result;
            int barcode_pos = this.scanDetailBS.Find("Barcode", this.txtScanEAN.Text);

            // 有資料
            if (barcode_pos == -1)
            {
                int no_barcode_cnt = ((DataTable)this.scanDetailBS.DataSource).AsEnumerable().Where(s => MyUtility.Check.Empty(s["Barcode"])).Count();
                if (no_barcode_cnt == 0)
                {
                    AutoClosingMessageBox.Show($"<{this.txtScanEAN.Text}> Invalid barcode !!", "Warning", 3000);
                    this.txtScanEAN.Text = string.Empty;
                    e.Cancel = true;
                    return;
                }
                else
                {
                    DataTable no_barcode_dt = ((DataTable)this.scanDetailBS.DataSource).AsEnumerable().Where(s => MyUtility.Check.Empty(s["Barcode"])).CopyToDataTable();

                    // 有空的barcode就開窗
                    SelectItem sele = new SelectItem(no_barcode_dt, "Article,Color,SizeCode", "8,6,8", string.Empty, headercaptions: "Colorway,Color,Size");
                    DialogResult result = sele.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        e.Cancel = true;
                        return;
                    }

                    var sellist = sele.GetSelecteds();
                    string upd_sql = $@"update PackingList_Detail
set BarCode = '{this.txtScanEAN.Text}'
where PackingList_Detail.Article 
=  '{sellist[0]["Article"]}'
and PackingList_Detail.SizeCode
=  '{sellist[0]["SizeCode"]}'
and PackingList_Detail.ID = '{this.selecedPK.ID}'
and PackingList_Detail.CTNStartNo = '{this.selecedPK.CTNStartNo}'
";
                    if (sql_result = DBProxy.Current.Execute(null, upd_sql))
                    {
                        foreach (DataRow dr in ((DataTable)this.scanDetailBS.DataSource).Rows)
                        {
                            if (dr["Article"].Equals(sellist[0]["Article"]) && dr["SizeCode"].Equals(sellist[0]["SizeCode"]))
                            {
                                dr["Barcode"] = this.txtScanEAN.Text;
                                dr["ScanQty"] = (short)dr["ScanQty"] + 1;
                                this.UpdScanQty((long)dr["Ukey"], (string)dr["Barcode"]);
                                break;
                            }
                        }
                    }
                    else
                    {
                        this.ShowErr(sql_result);
                        return;
                    }
                }
            }
            else
            {
                this.scanDetailBS.Position = barcode_pos;
                DataRowView cur_dr = (DataRowView)this.scanDetailBS.Current;
                int scanQty = (short)cur_dr["ScanQty"];
                int qtyPerCTN = (int)cur_dr["QtyPerCTN"];
                if (scanQty == qtyPerCTN)
                {
                    AutoClosingMessageBox.Show($"This Size scan is complete,can not scan again!!", "Warning", 3000);
                    this.txtScanEAN.Text = string.Empty;
                    e.Cancel = true;
                    return;
                }
                else
                {
                    cur_dr["ScanQty"] = (short)cur_dr["ScanQty"] + 1;
                    this.UpdScanQty((long)cur_dr["Ukey"]);
                }
            }

            // this.scanDetailBS.ResetCurrentItem();
            this.scanDetailBS.ResetBindings(true);

            // 計算scanQty
            this.numBoxScanQty.Value = ((DataTable)this.scanDetailBS.DataSource).AsEnumerable().Sum(s => (short)s["ScanQty"]);

            this.txtScanEAN.Text = string.Empty;

            // 如果都掃完 update PackingList_Detail
            if (this.numBoxScanQty.Value == this.numBoxScanTtlQty.Value)
            {
                string upd_sql = $@"update PackingList_Detail set ScanQty = QtyPerCTN , ScanEditDate = GETDATE(), ScanName = '{Env.User.UserID}' 
                                    where id = '{this.selecedPK.ID}' and CTNStartNo = '{this.selecedPK.CTNStartNo}' and Article = '{this.selecedPK.Article}'";
                if (sql_result = DBProxy.Current.Execute(null, upd_sql))
                {
                    // 檢查下方carton列表是否都掃完
                    int carton_complete = this.dt_scanDetail.AsEnumerable().Where(s => (short)s["ScanQty"] != (int)s["QtyPerCTN"]).Count();
                    if (carton_complete == 0)
                    {
                        this.ClearAll("ALL");
                    }
                    else
                    {
                        this.ClearAll("SCAN");
                        this.LoadSelectCarton();
                    }
                }
                else
                {
                    this.ShowErr(sql_result);
                    return;
                }
            }
            else
            {
                // 讓遊標停留在原地
                e.Cancel = true;
            }
        }

        private void UpdScanQty(long ukey, string barcode = "")
        {
            foreach (DataRow dr in this.dt_scanDetail.Rows)
            {
                if (dr["Ukey"].Equals(ukey))
                {
                    dr["ScanQty"] = (short)dr["ScanQty"] + 1;
                    if (MyUtility.Check.Empty(dr["Barcode"]) && !MyUtility.Check.Empty(barcode))
                    {
                        dr["Barcode"] = barcode;
                    }
                }
            }
        }

        private void ClearAll(string type)
        {
            if (type.Equals("ALL"))
            {
                this.txtScanCartonSP.Text = string.Empty;
                this.numBoxScanQty.Text = string.Empty;
                this.numBoxScanTtlQty.Text = string.Empty;
                this.displayPackID.Text = string.Empty;
                this.displayCtnNo.Text = string.Empty;
                this.displaySPNo.Text = string.Empty;
                this.displayPoNo.Text = string.Empty;
                this.displayBrand.Text = string.Empty;
                this.displayStyle.Text = string.Empty;

                this.numBoxttlCatons.Text = string.Empty;
                this.numBoxttlQty.Text = string.Empty;
                this.numBoxPackedCartons.Text = string.Empty;
                this.numBoxttlPackQty.Text = string.Empty;
                this.numBoxRemainCartons.Text = string.Empty;
                this.numBoxRemainQty.Text = string.Empty;
                this.scanDetailBS.DataSource = null;
                this.selcartonBS.DataSource = null;
                if (this.dt_scanDetail != null)
                {
                    this.dt_scanDetail.Clear();
                }

                this.comboPKFilter.DataSource = null;
                this.Tab_Focus("CARTON");
            }
            else if (type.Equals("SCAN"))
            {
                this.numBoxScanQty.Text = string.Empty;
                this.numBoxScanTtlQty.Text = string.Empty;
                this.displayPackID.Text = string.Empty;
                this.displayCtnNo.Text = string.Empty;
                this.displaySPNo.Text = string.Empty;
                this.displayPoNo.Text = string.Empty;
                this.displayBrand.Text = string.Empty;
                this.displayStyle.Text = string.Empty;

                this.numBoxttlCatons.Text = string.Empty;
                this.numBoxttlQty.Text = string.Empty;
                this.numBoxPackedCartons.Text = string.Empty;
                this.numBoxttlPackQty.Text = string.Empty;
                this.numBoxRemainCartons.Text = string.Empty;
                this.numBoxRemainQty.Text = string.Empty;
                this.scanDetailBS.DataSource = null;
            }
        }

        private class SelectCartonDetail
        {
            private string iD;
            private string cTNStartNo;
            private string custPoNo;
            private string article;
            private string orderID;
            private string sizeCode;
            private string qtyPerCTN;
            private string scanQty;
            private string brandId;
            private string styleId;
            private int ttlScanQty;

            public string ID
            {
                get
                {
                    return this.iD;
                }

                set
                {
                    this.iD = value;
                }
            }

            public string CTNStartNo
            {
                get
                {
                    return this.cTNStartNo;
                }

                set
                {
                    this.cTNStartNo = value;
                }
            }

            public string CustPoNo
            {
                get
                {
                    return this.custPoNo;
                }

                set
                {
                    this.custPoNo = value;
                }
            }

            public string Article
            {
                get
                {
                    return this.article;
                }

                set
                {
                    this.article = value;
                }
            }

            public string OrderID
            {
                get
                {
                    return this.orderID;
                }

                set
                {
                    this.orderID = value;
                }
            }

            public string SizeCode
            {
                get
                {
                    return this.sizeCode;
                }

                set
                {
                    this.sizeCode = value;
                }
            }

            public string QtyPerCTN
            {
                get
                {
                    return this.qtyPerCTN;
                }

                set
                {
                    this.qtyPerCTN = value;
                }
            }

            public string ScanQty
            {
                get
                {
                    return this.scanQty;
                }

                set
                {
                    this.scanQty = value;
                }
            }

            public string BrandId
            {
                get
                {
                    return this.brandId;
                }

                set
                {
                    this.brandId = value;
                }
            }

            public string StyleId
            {
                get
                {
                    return this.styleId;
                }

                set
                {
                    this.styleId = value;
                }
            }

            public int TtlScanQty
            {
                get
                {
                    return ttlScanQty;
                }

                set
                {
                    ttlScanQty = value;
                }
            }
        }
    }
}
