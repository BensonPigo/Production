﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci;
using Sci.Data;
using System.Linq;

namespace Sci.Production.Planning
{
    public partial class P04 : Sci.Win.Tems.QueryForm
    {
        Dictionary<string, string> di_inhouseOsp2 = new Dictionary<string, string>();
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        DataGridViewColumn col_Fty, col_season, col_style;
        public P04(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.EditMode = true;

        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            Color backDefaultColor = gridFactoryID.DefaultCellStyle.BackColor; 
            txtstyle.Select();
            //grid2.AutoGenerateColumns = true;

            dateSewingInline.Value1 = DateTime.Today.AddMonths(1);
            dateSewingInline.Value2 = DateTime.Today.AddMonths(2).AddDays(-1);

            Dictionary<string, string> di_inhouseOsp = new Dictionary<string, string>();
            di_inhouseOsp.Add("", "All");
            di_inhouseOsp.Add("O", "OSP");
            di_inhouseOsp.Add("I", "InHouse");

            comboOSPInHouse.DataSource = new BindingSource(di_inhouseOsp, null);
            comboOSPInHouse.ValueMember = "Key";
            comboOSPInHouse.DisplayMember = "Value";
            comboOSPInHouse.SelectedIndex = 2;  //inhouse

            di_inhouseOsp2.Add("O", "OSP");
            di_inhouseOsp2.Add("I", "InHouse");

            comboInHouseOSP.DataSource = new BindingSource(di_inhouseOsp2, null);
            comboInHouseOSP.ValueMember = "Key";
            comboInHouseOSP.DisplayMember = "Value";
            comboInHouseOSP.SelectedIndex = 1;  //inhouse

            string artworktype = "BONDING (MACHINE),BONDING (HAND)";
            string[] items2 = artworktype.Split(',');
            comboArtworkType.Items.AddRange(items2);
            comboArtworkType.SelectedIndex = 0;

            gridFactoryID.RowPrePaint += (s, e) => {
                if ( e.RowIndex < 0) return;
                DataRow dr = gridFactoryID.GetDataRow(e.RowIndex);

                #region 變色規則，若該 Row 已經變色則跳過
                switch (dr["err"].ToString())
                {
                    case "1":
                        if (gridFactoryID.Rows[e.RowIndex].DefaultCellStyle.BackColor != Color.FromArgb(255, 220, 255))
                            gridFactoryID.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(255, 220, 255);
                        break;
                    case "2":
                        if (gridFactoryID.Rows[e.RowIndex].DefaultCellStyle.BackColor != Color.FromArgb(128, 255, 0))
                            gridFactoryID.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(128, 255, 0);
                        break;
                    default :
                        if (gridFactoryID.Rows[e.RowIndex].DefaultCellStyle.BackColor != backDefaultColor)
                            gridFactoryID.Rows[e.RowIndex].DefaultCellStyle.BackColor = backDefaultColor;
                        break;
                }
                #endregion 
            };

            Ict.Win.DataGridViewGeneratorTextColumnSettings ts1 = new DataGridViewGeneratorTextColumnSettings();
            ts1.CellMouseDoubleClick += (s, e) =>
            {
                if (e.RowIndex < 0) return;
                DataRow ddr = gridFactoryID.GetDataRow<DataRow>(e.RowIndex);
                DataTable dt = (DataTable)listControlBindingSource1.DataSource;
                string colnm = this.gridFactoryID.Columns[e.ColumnIndex].DataPropertyName;
                string expression = colnm + " = '" + ddr[colnm].ToString().TrimEnd() + "'";
                DataRow[] drfound = dt.Select(expression);

                foreach (var item in drfound)
                {
                    if (item["selected"].ToString() == "0")
                        item["selected"] = 1;
                    else
                        item["selected"] = 0;
                }
            };

            this.gridFactoryID.CellValueChanged += (s, e) =>
            {
                if (gridFactoryID.Columns[e.ColumnIndex].Name == col_chk.Name)
                {
                    this.sum_checkedqty();
                }
            };

            Ict.Win.DataGridViewGeneratorDateColumnSettings ts2 = new DataGridViewGeneratorDateColumnSettings();
            ts2.CellValidating += (s, e) =>
            {

                if (!(MyUtility.Check.Empty(e.FormattedValue)))
                {
                    DataRow dr = ((Sci.Win.UI.Grid)((DataGridViewColumn)s).DataGridView).GetDataRow(e.RowIndex);
                    if (MyUtility.Check.Empty(dr["tapeoffline"])) return;
                    if (DateTime.Compare((DateTime)e.FormattedValue, (DateTime)dr["tapeoffline"]) > 0)
                    {
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox("Inline can't be late than Offline!!");
                    }
                }
            };

            Ict.Win.DataGridViewGeneratorDateColumnSettings ts3 = new DataGridViewGeneratorDateColumnSettings();
            ts3.CellValidating += (s, e) =>
            {

                if (!(MyUtility.Check.Empty(e.FormattedValue)))
                {
                    DataRow dr = ((Sci.Win.UI.Grid)((DataGridViewColumn)s).DataGridView).GetDataRow(e.RowIndex);
                    if (MyUtility.Check.Empty(dr["tapeinline"])) return;
                    if (DateTime.Compare((DateTime)e.FormattedValue, (DateTime)dr["tapeinline"]) < 0)
                    {
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox("Offline can't be earlier than inline!!");
                    }
                }
            };

            Ict.Win.UI.DataGridViewComboBoxColumn col_inhouseosp;

            #region local supplier 右鍵開窗
            Ict.Win.DataGridViewGeneratorTextColumnSettings ts = new DataGridViewGeneratorTextColumnSettings();
            ts.EditingMouseDown += (s, e) =>
            {
                
                if (e.RowIndex < 0) return;
                DataRow ddr = gridFactoryID.GetDataRow<DataRow>(e.RowIndex);
                DataTable dt = (DataTable)listControlBindingSource1.DataSource;
                string sqlcmd = "";
                if (MyUtility.Check.Empty(ddr["inhouseosp"]))
                {
                    MyUtility.Msg.WarningBox("Please select inhouse or osp first");
                    return;
                }
                sqlcmd = string.Format(@"
SELECT QU.localsuppid as id, 
       localsupp.abb as abb, 
       QU.mockup 
FROM   order_tmscost OT WITH (nolock) 
       INNER JOIN orders WITH (nolock) 
               ON OT.id = orders.id 
       INNER JOIN style_artwork SA WITH (nolock) 
               ON OT.artworktypeid = SA.artworktypeid 
                  AND orders.styleukey = SA.styleukey 
       LEFT JOIN style_artwork_quot QU WITH (nolock) 
              ON QU.ukey = SA.ukey 
       INNER JOIN localsupp WITH (nolock) 
               ON localsupp.id = QU.localsuppid 
WHERE  OT.ID = '{0}'
       AND OT.artworktypeid = '{1}' 
GROUP  BY QU.localsuppid, 
          localsupp.abb, 
          QU.mockup 
order by QU.localsuppid ", ddr["ID"].ToString().Trim(),comboArtworkType.Text);
                /*Query 需排除預估的資料,所以不需要進行額外判斷
                if (ddr["inhouseosp"].ToString() == "O")
                    sqlcmd = "select id,abb from localsupp WITH (NOLOCK) where junk = 0 and IsFactory = 0 order by ID";
                if (ddr["inhouseosp"].ToString() == "I")
                    sqlcmd = "select id,abb from localsupp WITH (NOLOCK) where junk = 0 and IsFactory = 1 order by ID";
                 * */
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlcmd, "10,30,15", null);
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    IList<DataRow> x = item.GetSelecteds();
                    ddr["localsuppid"] = x[0][0];
                    ddr["suppnm"] = x[0][1];
                }
            };
            ts.CellValidating += (s, e) =>
            {
                string Code = e.FormattedValue.ToString();//抓到當下的cell值
                string sqlcmd = ""; DataTable dt;
                DataRow ddr = gridFactoryID.GetDataRow<DataRow>(e.RowIndex);//抓到當下的row
                sqlcmd = string.Format(@"
SELECT QU.localsuppid as id, 
       localsupp.abb as abb, 
       QU.mockup 
FROM   order_tmscost OT WITH (nolock) 
       INNER JOIN orders WITH (nolock) 
               ON OT.id = orders.id 
       INNER JOIN style_artwork SA WITH (nolock) 
               ON OT.artworktypeid = SA.artworktypeid 
                  AND orders.styleukey = SA.styleukey 
       LEFT JOIN style_artwork_quot QU WITH (nolock) 
              ON QU.ukey = SA.ukey 
       INNER JOIN localsupp WITH (nolock) 
               ON localsupp.id = QU.localsuppid 
WHERE  OT.ID = '{0}'
       AND OT.artworktypeid = '{1}' 
GROUP  BY QU.localsuppid, 
          localsupp.abb, 
          QU.mockup 
order by QU.localsuppid ", ddr["ID"].ToString().Trim(), comboArtworkType.Text);
                /*Query 需排除預估的資料,所以不需要進行額外判斷
                if (ddr["inhouseosp"].ToString() == "O")
                    sqlcmd = "select id,abb from localsupp WITH (NOLOCK) where junk = 0 and IsFactory = 0 order by ID";
                if (ddr["inhouseosp"].ToString() == "I")
                    sqlcmd = "select id,abb from localsupp WITH (NOLOCK) where junk = 0 and IsFactory = 1 order by ID";
                 * */
                Ict.DualResult result;
                string dtid = ""; string dtabb = "";
                result = DBProxy.Current.Select(null, sqlcmd, out dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dtid = dt.Rows[i]["id"].ToString();
                    dtabb = dt.Rows[i]["abb"].ToString();
                    if (Code == dtid)
                    {
                        ddr["localsuppid"] = dtid;
                        ddr["suppnm"] = dtabb;
                        return;
                    }
                }
                if (Code == "")
                {
                    ddr["localSuppid"] = "";
                    ddr["suppnm"] = "";
                    return;
                }
                if (Code != dtid)
                {
                    ddr["localSuppid"] = "";
                    ddr["suppnm"] = "";
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox("This supp id is wrong");
                    return;
                }
            };
            #endregion

            //設定Grid1的顯示欄位
            this.gridFactoryID.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            this.gridFactoryID.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.gridFactoryID)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(2), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)
                .Text("FactoryID", header: "Fac", width: Widths.AnsiChars(5), settings: ts1, iseditingreadonly: true).Get(out col_Fty)
                .Text("Styleid", header: "Style", width: Widths.AnsiChars(15), settings: ts1, iseditingreadonly: true).Get(out col_style)
                .Text("seasonid", header: "Season", width: Widths.AnsiChars(5), iseditingreadonly: true).Get(out col_season)
                .Text("POID", header: "Mother SP", width: Widths.AnsiChars(13), settings: ts1, iseditingreadonly: true)
                .Text("id", header: "SP#", width: Widths.AnsiChars(13), settings: ts1, iseditingreadonly: true)
                .Text("article", header: "Article", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Numeric("totalqty", header: "M Qty", width: Widths.AnsiChars(8), integer_places: 8,decimal_places:3, iseditingreadonly: true)
                .Numeric("balance", header: "Bal. M", width: Widths.AnsiChars(8), integer_places: 8, decimal_places: 3, iseditingreadonly: true)
                .ComboBox("inhouseosp", header: "OSP/Inhouse").Get(out col_inhouseosp)
                .Text("localSuppid", header: "Supp Id", width: Widths.AnsiChars(6),settings:ts)
                .Text("suppnm", header: "Supplier", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Date("cutinline", header: "Cut Inline", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Date("cutoffline", header: "Cut Offline", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Date("Sewinline", header: "Sew Inline" + Environment.NewLine + "Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Date("sewoffline", header: "Sew Offline" + Environment.NewLine + "Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Date("ArtworkInLine", header: "Inline", width: Widths.AnsiChars(10))
                 .Date("ArtworkOffLine", header: "Offline", width: Widths.AnsiChars(10))
                 .Text("sewinglineid", header: "Line#", width: Widths.AnsiChars(10), settings: ts1, iseditingreadonly: true)
                 .Date("scidelivery", header: "SCI Dlv.", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Date("buyerdelivery", header: "Buyer Dlv.", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Numeric("Stdq", header: "Std. Qty", width: Widths.AnsiChars(8), integer_places: 8, iseditingreadonly: true)
                 .Numeric("qty", header: "Cutparts", width: Widths.AnsiChars(8), integer_places: 8, iseditingreadonly: true)
                 .Numeric("tms", header: "TMS", width: Widths.AnsiChars(3), integer_places: 8, iseditingreadonly: true)
                 .Numeric("OrderQty", header: "Order Qty", width: Widths.AnsiChars(8), integer_places: 8, iseditingreadonly: true)
                 .Text("msg", header: "Error Message", width: Widths.AnsiChars(20), settings: ts1, iseditingreadonly: true)
                  ;
            gridFactoryID.Columns["inhouseosp"].DefaultCellStyle.BackColor = Color.Pink;
            gridFactoryID.Columns["localSuppid"].DefaultCellStyle.BackColor = Color.Pink;
            gridFactoryID.Columns["ArtworkInLine"].DefaultCellStyle.BackColor = Color.Pink;
            gridFactoryID.Columns["ArtworkOffLine"].DefaultCellStyle.BackColor = Color.Pink;
           // foreach (DataGridViewColumn col in grid1.Columns) { col.SortMode = DataGridViewColumnSortMode.NotSortable; } //關掉header排序
            this.gridFactoryID.ColumnHeaderMouseClick += grid1_ColumnHeaderMouseClick;
            col_inhouseosp.DataSource = new BindingSource(di_inhouseOsp2, null);
            col_inhouseosp.ValueMember = "Key";
            col_inhouseosp.DisplayMember = "Value";

            Helper.Controls.Grid.Generator(this.gridSupplier)
                .Text("Supplier", header: "Supplier", width: Widths.AnsiChars(6))
                .Numeric("totalqty", header: "M Qty", width: Widths.AnsiChars(8), integer_places: 8, decimal_places: 3, iseditingreadonly: true)
                .Numeric("balance", header: "Balance M", width: Widths.AnsiChars(8), integer_places: 8, decimal_places: 3, iseditingreadonly: true)
                .Numeric("Totaltms", header: "Total Tms", width: Widths.AnsiChars(9), integer_places: 8, iseditingreadonly: true); ;
            
        }
        void grid1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == this.col_Fty.Index)
            {
                if (null != this.dtData)
                {
                    this.dtData.DefaultView.Sort = "factoryID,seasonID,styleID";
                    gridFactoryID.DataSource = dtData;

                }
            }
            if (e.ColumnIndex == this.col_style.Index)
            {
                if (null != this.dtData)
                {
                    this.dtData.DefaultView.Sort = "seasonID,styleID";
                    gridFactoryID.DataSource = dtData;

                }
            }
        }

        DataTable dtData = null;
        //Query
        private void btnQuery_Click(object sender, EventArgs e)
        {
            
            numCheckedQty.Value = 0;
          //  DataTable dtData;
            string sewinline_b, sewinline_e, sciDelivery_b, sciDelivery_e, styleid, seasonid, localsuppid, inhouseosp, factoryid,inline_b,inline_e,artworktypeid;
            sewinline_b = null;
            sewinline_e = null;
            sciDelivery_b = null;
            sciDelivery_e = null;
            inline_b = null;
            inline_e = null;

            styleid = txtstyle.Text;
            seasonid = txtseason.Text;
            localsuppid = txtsubconSupplier.TextBox1.Text;
            factoryid = txtmfactory.Text;
            inhouseosp = comboOSPInHouse.SelectedValue.ToString();
            artworktypeid = comboArtworkType.Text;

            if (dateSewingInline.Value1 != null) {sewinline_b = this.dateSewingInline.Text1;}
            if (dateSewingInline.Value2 != null) { sewinline_e = this.dateSewingInline.Text2; }

            if (dateSCIDelivery.Value1 != null) {sciDelivery_b = this.dateSCIDelivery.Text1;}
            if (dateSCIDelivery.Value2 != null) { sciDelivery_e = this.dateSCIDelivery.Text2; }

            if (dateInlineDate.Value1 != null) { inline_b = this.dateInlineDate.Text1; }
            if (dateInlineDate.Value2 != null) { inline_e = this.dateInlineDate.Text2; }

            if ((sewinline_b == null && sewinline_e == null) &&
                (sciDelivery_b == null && sciDelivery_e == null))
            {
                dateSCIDelivery.Focus1();
                MyUtility.Msg.WarningBox("< SCI Delivery > or < Sewing Inline Date > can't be empty!!");
                return;
            }

            if (MyUtility.Check.Empty(numEfficiency.Text))
            {
                numEfficiency.Focus();
                MyUtility.Msg.WarningBox("Efficiency can't be empty!!");
                return;
            }

            if (MyUtility.Check.Empty(numWorkHours.Text))
            {
                numWorkHours.Focus();
                MyUtility.Msg.WarningBox("Work hours can't be empty!!");
                return;
            }
            string orderby="";
            string sqlcmd
                = string.Format(@"
SELECT  selected = 0
        , a.id
        , a.SciDelivery
        , a.CutInline
        , a.CutOffline
        , a.FactoryID
        , a.StyleID
        , a.SeasonID
        , OrderQty = a.Qty
        , a.isforecast
        , a.poid
        , article = (select cast (t.article as varchar) + ';' 
                     from (
                        select article 
                        from order_qty WITH (NOLOCK) 
                        where id = a.ID 
                        group by article 
                     ) t 
                     for xml path(''))
        , b.InhouseOSP
        , b.LocalSuppID
        , suppnm = (select Abb 
                    from LocalSupp WITH (NOLOCK) 
                    where id = b.LocalSuppID) 
        , b.ArtworkInLine
        , b.ArtworkOffLine
        , sewinline = convert (date, c.Inline)
        , SewOffLine = convert (date, c.offline)
        , a.StyleUkey
        , qaqty = isnull ((select sum(tmp3.qaqty)  
                           from (
                                SELECT  article
                                        , qaqty = min (isnull (qaqty, 0)) 
	                            FROM style_location WITH (NOLOCK) 
	                            left join (
				                    SELECT  [ComboType]
					                        , [Article]
					                        , QAQTY = SUM ([QAQty]) 
				                    FROM [Production].[dbo].[SewingOutput_Detail] WITH (NOLOCK) 
                                    WHERE ORDERID=a.id
				                    GROUP BY ComboType, Article
                                ) TMP on style_location.Location = tmp.ComboType 
                                where style_location.StyleUkey = a.styleukey
	                            group by article
                           ) tmp3), 0) 
        , stdq = 0
        , err = 0
        , msg = ''
        , C.alloqty
        , c.sewinglineid
        , a.scidelivery
        , a.buyerdelivery
        , totalqty = round (a.qty / (3600 * {0} / b.tms) * 100 / {1}, 3)
        , balance = round ((a.qty
                            - (isnull ((select sum(tmp3.qaqty)  
                                        from (
                                            SELECT  article
                                                    , qaqty = min (isnull (qaqty, 0)) 
	                                        FROM style_location WITH (NOLOCK) 
	                                        left join (
				                                SELECT  [ComboType]
					                                    , [Article]
					                                    , QAQTY = SUM([QAQty]) 
				                                FROM [Production].[dbo].[SewingOutput_Detail] WITH (NOLOCK) 
                                                WHERE ORDERID=a.id
				                                GROUP BY ComboType, Article
                                            ) TMP on style_location.Location = tmp.ComboType 
                                            where style_location.StyleUkey = a.styleukey
	                                        group by article
                                        ) tmp3), 0))
                              )
                            / (3600 * {0} / b.tms) * 100 / {1}, 3)
        , b.tms
        , b.qty
        , totaltms = a.qty * b.tms
        , b.artworktypeid
FROM (Orders a WITH (NOLOCK) 
inner join  Order_tmscost b WITH (NOLOCK) on a.ID = b.ID) 
inner join SewingSchedule c on a.id = c.OrderID
inner join dbo.factory on factory.id = a.factoryid
where   a.Finished = 0 
        and a.Category != 'M'  and factory.IsProduceFty = 1
        and b.tms > 0  
        and a.IsForecast=0 " + orderby, numWorkHours.Text, numEfficiency.Text);

            if (!(MyUtility.Check.Empty(styleid)))
            {sqlcmd += string.Format(@" and a.StyleID = '{0}'", styleid);}
            if (!(MyUtility.Check.Empty(seasonid)))
            { sqlcmd += string.Format(@" and a.SeasonID = '{0}'", seasonid); }
            if (!(MyUtility.Check.Empty(localsuppid)))
            { sqlcmd += string.Format(@"  and b.LocalSuppID='{0}'", localsuppid); }
            if (!(MyUtility.Check.Empty(inhouseosp)))
            { sqlcmd += string.Format(@" and b.InhouseOSP = '{0}'", inhouseosp); }
            if (!(MyUtility.Check.Empty(factoryid)))
            {sqlcmd += string.Format(@" and a.FactoryID ='{0}'", factoryid);}
            if (!(MyUtility.Check.Empty(artworktypeid)))
            { sqlcmd += string.Format(@" and b.artworktypeid = '{0}'", artworktypeid); }
            if (!MyUtility.Check.Empty(sciDelivery_b))
            { sqlcmd += string.Format(@" and a.SciDelivery >= '{0}'", Convert.ToDateTime(sciDelivery_b).ToString("d")); }
            if (!MyUtility.Check.Empty(sciDelivery_e))
            { sqlcmd += string.Format(@" and a.SciDelivery <= '{0}'", Convert.ToDateTime(sciDelivery_e).ToString("d")); }
            if (!(string.IsNullOrWhiteSpace(sewinline_b)))
            { sqlcmd += string.Format(@" and c.OffLine >= '{0}'", Convert.ToDateTime(sewinline_b).ToString("d")); }
            if (!(string.IsNullOrWhiteSpace(sewinline_e)))
            { sqlcmd += string.Format(@" and c.InLine <= '{0}'", Convert.ToDateTime(sewinline_e).ToString("d")); }
            if (!(string.IsNullOrWhiteSpace(inline_b)))
            { sqlcmd += string.Format(@" and b.artworkOffLine >= '{0}'", Convert.ToDateTime(inline_b).ToString("d")); }
            if (!(string.IsNullOrWhiteSpace(inline_e)))
            { sqlcmd += string.Format(@" and b.artworkInLine <= '{0}'", Convert.ToDateTime(inline_e).ToString("d")); }
            sqlcmd += string.Format(@" ORDER BY a.FactoryID, a.StyleID, a.SeasonID,a.ID ");
            this.ShowWaitMessage("Querying.... Please wait....");
            int wkdays = 0;
            DateTime inline;
            Ict.DualResult result;
            if (result = DBProxy.Current.Select(null, sqlcmd, out dtData))
            {
                if (dtData.Rows.Count == 0)
                {
                    this.HideWaitMessage();
                    MyUtility.Msg.WarningBox("Data not found!!"); 
                
                }
                listControlBindingSource1.DataSource = dtData;
                grid2_generate();
              
            }
            foreach (DataRow item in dtData.Rows)
            {
                if (!MyUtility.Check.Empty(item["sewinline"]))//dtData["sewinline"]))
                {
                    inline = PublicPrg.Prgs.GetWorkDate(item["factoryid"].ToString(), -5, (DateTime)item["sewinline"]);
                    decimal stdq = PublicPrg.Prgs.GetStdQ(item["id"].ToString());
                    item["stdq"] = stdq;
                    wkdays = (stdq != '0') ? ' ' : int.Parse(Math.Ceiling((decimal.Parse(item["OrderQty"].ToString()) - decimal.Parse(item["qaqty"].ToString())) / stdq).ToString());
                }
                this.HideWaitMessage();
            }
            this.gridFactoryID.AutoResizeColumns();
            this.gridSupplier.AutoResizeColumns();
        }

        //close
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Save
        private void btnSave_Click(object sender, EventArgs e)
        {
            CheckData();
            DualResult result;
            DataTable dt = (DataTable)listControlBindingSource1.DataSource;
            if (dt == null || dt.Rows.Count == 0) return;

            DataRow[] FindError;
            FindError = dt.Select("Selected = 1 and err > 0");
            if (FindError.Length > 0)
            {
                MyUtility.Msg.WarningBox("Please select rows first without error message!", "Warnning");
                return;
            }
            DialogResult dResult = MyUtility.Msg.QuestionBox("Are you sure to save it?", "Question", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2);
            if (dResult.ToString().ToUpper() == "NO") return;

            string sqlcmd = "";
            DataRow[] find;
            find = dt.Select("Selected = 1 and err = 0");

            foreach (DataRow item in find)
            {

                sqlcmd += @"update order_tmscost ";
                if (MyUtility.Check.Empty(item["artworkInline"]))
                { sqlcmd += "set artworkinline = null "; }
                else
                { sqlcmd += string.Format(@"set artworkinline ='{0}'", ((DateTime)item["artworkinline"]).ToShortDateString()); }
                if (MyUtility.Check.Empty(item["artworkOffline"]))
                { sqlcmd += ",artworkOffline = null "; }
                else
                { sqlcmd += string.Format(@",artworkOffline = '{0}'", ((DateTime)item["artworkOffline"]).ToShortDateString()); }
                sqlcmd += string.Format(@",inhouseosp = '{0}',localsuppid='{1}'", item["inhouseosp"].ToString(),item["localsuppid"].ToString()); 
                sqlcmd += string.Format(@" where id ='{0}' and artworktypeid = '{1}';"
                                                        , item["ID"],item["artworktypeid"]);

            }
            if (!(result = Sci.Data.DBProxy.Current.Execute(null, sqlcmd)))
            {
                MyUtility.Msg.WarningBox("Save failed, Pleaes re-try");
                return;
            }
            else { MyUtility.Msg.InfoBox("Save successful!!"); }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            listControlBindingSource1.EndEdit();
            DataTable dt = (DataTable)listControlBindingSource1.DataSource;
            if (dt == null || dt.Rows.Count == 0) return;
            DataRow[] drfound = dt.Select("selected = 1");

            foreach (var item in drfound)
            {
                item["inhouseosp"] = this.comboInHouseOSP.SelectedValue.ToString();
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            listControlBindingSource1.EndEdit();
            DataTable dt = (DataTable)listControlBindingSource1.DataSource;
            if (dt == null || dt.Rows.Count == 0) return;
            DataRow[] drfound = dt.Select("selected = 1");
            foreach (var item in drfound)
            {
                item["localsuppid"] = txtsubconLocalSuppid.TextBox1.Text;
                item["suppnm"] = txtsubconLocalSuppid.DisplayBox1.Text;
            }
        }

        //Find SP#
        private void btnLocateForSPFind_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(listControlBindingSource1.DataSource)) return;
            int index = listControlBindingSource1.Find("id", txtLocateForSP.Text.TrimEnd());
            if (index == -1)
            { MyUtility.Msg.WarningBox("Data was not found!!"); }
            else
            { listControlBindingSource1.Position = index; }
        }

        private void sum_checkedqty()
        {
            listControlBindingSource1.EndEdit();
            DataTable dt = (DataTable)listControlBindingSource1.DataSource;
            Object localPrice = dt.Compute("Sum(totalqty)", "selected = 1");
            if (MyUtility.Check.Empty(localPrice.ToString()))
                this.numCheckedQty.Value = 0;
            else
                this.numCheckedQty.Value = decimal.Parse(localPrice.ToString());
        }

        //Filter empty Supp ID , In Line
        private void checkSuppID_CheckedChanged(object sender, EventArgs e)
        {
            listControlBindingSource1.Filter = "";
            if (checkSuppID.Checked && checkInLine.Checked) listControlBindingSource1.Filter = " localsuppid ='' and ArtworkInLine is null ";
            else
            {
                if (checkSuppID.Checked) listControlBindingSource1.Filter = " localsuppid ='' ";
                if (checkInLine.Checked) listControlBindingSource1.Filter = "ArtworkInLine is null";
            }
            grid2_generate();
        }

        private void grid2_generate()
        {
            var bs1 = (from rows in ((DataTable)listControlBindingSource1.DataSource).AsEnumerable()
                       group rows by new { localsuppid = rows["localsuppid"].ToString().TrimEnd(), suppnm = rows["suppnm"].ToString().TrimEnd() } into grouprows
                       orderby grouprows.Key.localsuppid
                       select new
                       {
                           Supplier = grouprows.Key.localsuppid + "-" + grouprows.Key.suppnm,
                           TotalQty = grouprows.Sum(r => r.Field<decimal?>("totalqty").GetValueOrDefault(0)),
                           Balance = grouprows.Sum(r => r.Field<decimal?>("balance").GetValueOrDefault(0)),
                           Totaltms = grouprows.Sum(r => r.Field<decimal?>("totaltms").GetValueOrDefault(0))
                       }).ToList();

            var bs2 = (from rows in ((DataTable)listControlBindingSource1.DataSource).AsEnumerable()
                       group rows by new { localsuppid = "Total" } into grouprows
                       select new
                       {
                           Supplier = grouprows.Key.localsuppid,
                           TotalQty = grouprows.Sum(r => r.Field<decimal?>("totalqty").GetValueOrDefault(0)),
                           Balance = grouprows.Sum(r => r.Field<decimal?>("balance").GetValueOrDefault(0)),
                           Totaltms = grouprows.Sum(r => r.Field<decimal?>("totaltms").GetValueOrDefault(0))
                       }).ToList();
            bs1.AddRange(bs2);
            gridSupplier.DataSource = bs1;
            //grid2.AutoResizeColumns();
            
        }

           //update inline
        private void btnUpdateInline_Click(object sender, EventArgs e)
        {
            decimal stdq = 0m;
            int wkdays = 0;
            DateTime inline ;
            DataTable dt = (DataTable)listControlBindingSource1.DataSource;
            if (dt == null || dt.Rows.Count == 0) return;
            DataRow[] find;
            find = dt.Select("Selected = 1");
            if (find.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select rows first!", "Warnning");
                return;
            }
            this.ShowWaitMessage("Updating Inline Date.... Please wait....");
            foreach (DataRow item in find)
            {
                
                if (!MyUtility.Check.Empty(item["sewinline"]))
                {
                    inline = PublicPrg.Prgs.GetWorkDate(item["factoryid"].ToString(), -5, (DateTime)item["sewinline"]);
                    stdq = PublicPrg.Prgs.GetStdQ(item["id"].ToString());
                    item["stdq"] = stdq;
                    if (stdq > 0)
                    {
                        wkdays = int.Parse(Math.Ceiling((decimal.Parse(item["OrderQty"].ToString()) - decimal.Parse(item["qaqty"].ToString())) / stdq).ToString());
                        item["artworkinline"] = inline;
                        item["artworkoffline"] = PublicPrg.Prgs.GetWorkDate(item["factoryid"].ToString(), wkdays, inline);
                    }
                }
            }
            this.HideWaitMessage();
        }

        //Check data
        private void btnCheckData_Click(object sender, EventArgs e)
        {
            CheckData();
        }

        private void CheckData()
        {
            if (listControlBindingSource1.DataSource == null) return;
            DataTable dt = (DataTable)listControlBindingSource1.DataSource;
            DataRow dr;
            listControlBindingSource1.Filter = "";
            foreach (DataRow item in dt.Select("inhouseosp='O'"))
            {
                item["msg"] = "";
                item["err"] = 0;
                if (MyUtility.Check.Seek(string.Format(@"select b.priceapv,oven,wash,mockup from style_artwork  a WITH (NOLOCK) 
                                                        inner join style_artwork_quot b WITH (NOLOCK)
                                                on a.ukey = b.ukey where a.styleukey = {0} and b.localsuppid = '{1}'"
                    , item["styleukey"], item["localsuppid"]), out dr, null) == false)
                {
                    item["msg"] = "Quotation data was not found!!";
                    item["err"] = 2;
                }
            }
            dt.DefaultView.Sort = "err desc";
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            listControlBindingSource1.EndEdit();
            DataTable dt = (DataTable)listControlBindingSource1.DataSource;
            if (dt == null || dt.Rows.Count == 0) return;
            DataRow[] drfound = dt.Select("selected = 1");

            foreach (var item in drfound)
            {
                if (null != dateArtworkInLine.Value)
                {
                    item["artworkinline"] = dateArtworkInLine.Value;
                }
                else
                    item["artworkinline"] = DBNull.Value;
            }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            listControlBindingSource1.EndEdit();
            DataTable dt = (DataTable)listControlBindingSource1.DataSource;
            if (dt == null || dt.Rows.Count == 0) return;
            DataRow[] drfound = dt.Select("selected = 1");
            foreach (var item in drfound)
            {
                if (null != dateArtworkOffLine.Value)
                {
                    item["artworkoffline"] = dateArtworkOffLine.Value;
                }
                else
                    item["artworkoffline"] = DBNull.Value;
            }
        }

        //find Style#
        private void btnLocateForStyleFind_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(listControlBindingSource1.DataSource)) return;
            int index = listControlBindingSource1.Find("Styleid", txtLocateForStyle.Text.TrimEnd());
            if (index == -1)
            { MyUtility.Msg.WarningBox("Data was not found!!"); }
            else
            { listControlBindingSource1.Position = index; }
        }
    }
}
