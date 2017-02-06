using System;
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
            txtstyle1.Select();
            //grid2.AutoGenerateColumns = true;

            dateRange1.Value1 = DateTime.Now.AddMonths(1);
            dateRange1.Value2 = DateTime.Now.AddMonths(2).AddDays(-1);

            Dictionary<string, string> di_inhouseOsp = new Dictionary<string, string>();
            di_inhouseOsp.Add("", "All");
            di_inhouseOsp.Add("O", "OSP");
            di_inhouseOsp.Add("I", "InHouse");

            comboBox1.DataSource = new BindingSource(di_inhouseOsp, null);
            comboBox1.ValueMember = "Key";
            comboBox1.DisplayMember = "Value";
            comboBox1.SelectedIndex = 2;  //inhouse

            di_inhouseOsp2.Add("O", "OSP");
            di_inhouseOsp2.Add("I", "InHouse");

            comboBox3.DataSource = new BindingSource(di_inhouseOsp2, null);
            comboBox3.ValueMember = "Key";
            comboBox3.DisplayMember = "Value";
            comboBox3.SelectedIndex = 1;  //inhouse

            string artworktype = "BONDING (MACHINE),BONDING (HAND)";
            string[] items2 = artworktype.Split(',');
            comboBox4.Items.AddRange(items2);
            comboBox4.SelectedIndex = 0;

            MyUtility.Tool.SetGridFrozen(this.grid1);
            grid1.RowPostPaint += (s, e) =>
            {
                //DataGridViewRow dvr = detailgrid.Rows[e.RowIndex];
                //DataRow dr = ((DataRowView)dvr.DataBoundItem).Row;
                DataRow dr = grid1.GetDataRow(e.RowIndex);
                if (grid1.Rows.Count <= e.RowIndex || e.RowIndex < 0) return;

                int i = e.RowIndex;
                switch (int.Parse(dr["err"].ToString()))
                {
                    case 1:
                        grid1.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(255, 220, 255);
                        break;
                    case 2:
                        grid1.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(128, 255, 0);
                        break;
                }
               
            };

            Ict.Win.DataGridViewGeneratorTextColumnSettings ts1 = new DataGridViewGeneratorTextColumnSettings();
            ts1.CellMouseDoubleClick += (s, e) =>
            {
                if (e.RowIndex < 0) return;
                DataRow ddr = grid1.GetDataRow<DataRow>(e.RowIndex);
                DataTable dt = (DataTable)listControlBindingSource1.DataSource;
                string colnm = this.grid1.Columns[e.ColumnIndex].DataPropertyName;
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

            this.grid1.CellValueChanged += (s, e) =>
            {
                if (grid1.Columns[e.ColumnIndex].Name == col_chk.Name)
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
                DataRow ddr = grid1.GetDataRow<DataRow>(e.RowIndex);
                DataTable dt = (DataTable)listControlBindingSource1.DataSource;
                string sqlcmd = "";
                if (MyUtility.Check.Empty(ddr["inhouseosp"]))
                {
                    MyUtility.Msg.WarningBox("Please select inhouse or osp first");
                    return;
                }
                if (ddr["inhouseosp"].ToString() == "O")
                    sqlcmd = "select id,abb from localsupp where junk = 0 and IsFactory = 0 order by ID";
                if (ddr["inhouseosp"].ToString() == "I")
                    sqlcmd = "select id,abb from localsupp where junk = 0 and IsFactory = 1 order by ID";
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlcmd, "10,30", null);
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
                DataRow ddr = grid1.GetDataRow<DataRow>(e.RowIndex);//抓到當下的row
                if (ddr["inhouseosp"].ToString() == "O")
                    sqlcmd = "select id,abb from localsupp where junk = 0 and IsFactory = 0 order by ID";
                if (ddr["inhouseosp"].ToString() == "I")
                    sqlcmd = "select id,abb from localsupp where junk = 0 and IsFactory = 1 order by ID";
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
                    MyUtility.Msg.WarningBox("This supp id is wrong");
                    ddr["localSuppid"] = "";
                    ddr["suppnm"] = "";
                    e.Cancel = true;
                    return;
                }
            };
            #endregion

            //設定Grid1的顯示欄位
            this.grid1.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            this.grid1.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.grid1)
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
            foreach (DataGridViewColumn col in grid1.Columns) { col.SortMode = DataGridViewColumnSortMode.NotSortable; } //關掉header排序
            this.grid1.ColumnHeaderMouseClick += grid1_ColumnHeaderMouseClick;
            col_inhouseosp.DataSource = new BindingSource(di_inhouseOsp2, null);
            col_inhouseosp.ValueMember = "Key";
            col_inhouseosp.DisplayMember = "Value";
            grid1.Columns[5].Frozen = true;  //SP#
            Helper.Controls.Grid.Generator(this.grid2)
                .Text("Supplier", header: "Supplier", width: Widths.AnsiChars(6))
                .Numeric("totalqty", header: "M Qty", width: Widths.AnsiChars(8), integer_places: 8, decimal_places: 3, iseditingreadonly: true)
                .Numeric("balance", header: "Balance M", width: Widths.AnsiChars(8), integer_places: 8, iseditingreadonly: true)
                .Numeric("Totaltms", header: "Total Tms", width: Widths.AnsiChars(9), integer_places: 8, iseditingreadonly: true); ;
            
        }
        void grid1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == this.col_Fty.Index)
            {
                if (null != this.dtData)
                {
                    this.dtData.DefaultView.Sort = "factoryID,seasonID,styleID";
                    grid1.DataSource = dtData;

                }
            }
            if (e.ColumnIndex == this.col_style.Index)
            {
                if (null != this.dtData)
                {
                    this.dtData.DefaultView.Sort = "seasonID,styleID";
                    grid1.DataSource = dtData;

                }
            }
        }

        DataTable dtData = null;
        //Query
        private void button1_Click(object sender, EventArgs e)
        {
            
            numericBox4.Value = 0;
          //  DataTable dtData;
            string sewinline_b, sewinline_e, sciDelivery_b, sciDelivery_e, styleid, seasonid, localsuppid, inhouseosp, factoryid,inline_b,inline_e,artworktypeid;
            sewinline_b = null;
            sewinline_e = null;
            sciDelivery_b = null;
            sciDelivery_e = null;
            inline_b = null;
            inline_e = null;

            styleid = txtstyle1.Text;
            seasonid = txtseason1.Text;
            localsuppid = txtsubcon1.TextBox1.Text;
            factoryid = txtmfactory1.Text;
            inhouseosp = comboBox1.SelectedValue.ToString();
            artworktypeid = comboBox4.Text;

            if (dateRange1.Value1 != null) {sewinline_b = this.dateRange1.Text1;}
            if (dateRange1.Value2 != null) { sewinline_e = this.dateRange1.Text2; }

            if (dateRange2.Value1 != null) {sciDelivery_b = this.dateRange2.Text1;}
            if (dateRange2.Value2 != null) { sciDelivery_e = this.dateRange2.Text2; }

            if (dateRange3.Value1 != null) { inline_b = this.dateRange3.Text1; }
            if (dateRange3.Value2 != null) { inline_e = this.dateRange3.Text2; }

            if ((sewinline_b == null && sewinline_e == null) &&
                (sciDelivery_b == null && sciDelivery_e == null))
            {
                MyUtility.Msg.WarningBox("< SCI Delivery > or < Sewing Inline Date > can't be empty!!");
                dateRange2.Focus1();
                return;
            }

            if (MyUtility.Check.Empty(numericBox2.Text))
            {
                MyUtility.Msg.WarningBox("Efficiency can't be empty!!");
                numericBox2.Focus();
                return;
            }

            if (MyUtility.Check.Empty(numericBox3.Text))
            {
                MyUtility.Msg.WarningBox("Work hours can't be empty!!");
                numericBox3.Focus();
                return;
            }
            string orderby="";
            string sqlcmd
                = string.Format(@"SELECT 0 as selected,a.id, a.SciDelivery, a.CutInline, a.CutOffline, a.FactoryID
, a.StyleID, a.SeasonID
, a.Qty AS OrderQty
, a.isforecast,a.poid
, (select cast(t.article as varchar)+';' from (select article from order_qty where id = a.ID group by article )t for xml path('')) as article
,b.InhouseOSP
,b.LocalSuppID
,(select Abb from LocalSupp where id = b.LocalSuppID) suppnm
,b.ArtworkInLine
,b.ArtworkOffLine
,convert(date,c.Inline) as sewinline
,convert(date,c.offline) as SewOffLine
,a.StyleUkey
,isnull((select sum(tmp3.qaqty)  
    from 
    (SELECT article,min(isnull(qaqty,0)) qaqty
	    FROM style_location 
	    left join (
				    SELECT 
					      [ComboType]
					      ,[Article]
					      ,SUM([QAQty]) QAQTY
				      FROM [Production].[dbo].[SewingOutput_Detail] WHERE ORDERID=a.id
				      GROUP BY ComboType,Article) TMP 
	    on style_location.Location = tmp.ComboType where style_location.StyleUkey = a.styleukey
	    group by article) tmp3),0) qaqty
, 0 as stdq
, 0 as err
,'' as msg
,C.alloqty
,c.sewinglineid
,a.scidelivery
,a.buyerdelivery
,round(a.qty/(3600*{0}/b.tms)*100/{1},3) as totalqty
,round((a.qty-(isnull((select sum(tmp3.qaqty)  
    from 
    (SELECT article,min(isnull(qaqty,0)) qaqty
	    FROM style_location 
	    left join (
				    SELECT 
					      [ComboType]
					      ,[Article]
					      ,SUM([QAQty]) QAQTY
				      FROM [Production].[dbo].[SewingOutput_Detail] WHERE ORDERID=a.id
				      GROUP BY ComboType,Article) TMP 
	    on style_location.Location = tmp.ComboType where style_location.StyleUkey = a.styleukey
	    group by article) tmp3),0)))/(3600*{0}/b.tms)*100/{1},3) as balance
,b.tms
,b.qty
,a.qty * b.tms as totaltms
,b.artworktypeid
 FROM (Orders a inner join  Order_tmscost b on a.ID = b.ID) 
inner join SewingSchedule c on a.id = c.OrderID
inner join dbo.factory on factory.id = a.factoryid
 where a.Finished = 0 AND a.Category !='M' 
and b.tms > 0  and factory.mdivisionid='{2}'" + orderby, numericBox3.Text, numericBox2.Text, Sci.Env.User.Keyword);

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
            if (!(MyUtility.Check.Empty(sciDelivery_b)))
            { sqlcmd += string.Format(@" and a.SciDelivery between '{0}' and '{1}'", sciDelivery_b, sciDelivery_e); }
            if (!(string.IsNullOrWhiteSpace(sewinline_b)))
            { sqlcmd += string.Format(@" and not (c.InLine > '{1}' or c.OffLine < '{0}')", sewinline_b, sewinline_e); }
            if (!(string.IsNullOrWhiteSpace(inline_b)))
            { sqlcmd += string.Format(@" and not (b.artworkInLine > '{1}' or b.artworkOffLine < '{0}')", inline_b, inline_e); }
            this.ShowWaitMessage("Querying.... Please wait....");
            int wkdays = 0;
            DateTime inline;
            Ict.DualResult result;
            if (result = DBProxy.Current.Select(null, sqlcmd, out dtData))
            {
                if (dtData.Rows.Count == 0)
                { MyUtility.Msg.WarningBox("Data not found!!"); }
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
            this.grid1.AutoResizeColumns();
            this.gris2.AutoresizeColumns();
        }

        //close
        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Save
        private void button3_Click(object sender, EventArgs e)
        {
            CheckData();
            DualResult result;
            DataTable dt = (DataTable)listControlBindingSource1.DataSource;
            if (dt == null || dt.Rows.Count == 0) return;
            DataRow[] find;
            find = dt.Select("Selected = 1 and err < 2");
            if (find.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select rows first without error message!", "Warnning");
                return;
            }
            DialogResult dResult = MyUtility.Msg.QuestionBox("Are you sure to save it?", "Question", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2);
            if (dResult.ToString().ToUpper() == "NO") return;

            string sqlcmd = "";

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
                item["inhouseosp"] = this.comboBox3.SelectedValue.ToString();
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
                item["localsuppid"] = txtsubcon2.TextBox1.Text;
                item["suppnm"] = txtsubcon2.DisplayBox1.Text;
            }
        }

        //Find SP#
        private void button2_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(listControlBindingSource1.DataSource)) return;
            int index = listControlBindingSource1.Find("id", textBox1.Text.TrimEnd());
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
                this.numericBox4.Value = 0;
            else
                this.numericBox4.Value = decimal.Parse(localPrice.ToString());
        }

        //Filter empty Supp ID , In Line
        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            listControlBindingSource1.Filter = "";
            if (checkBox4.Checked && checkBox3.Checked) listControlBindingSource1.Filter = " localsuppid ='' and ArtworkInLine is null ";
            else
            {
                if (checkBox4.Checked) listControlBindingSource1.Filter = " localsuppid ='' ";
                if (checkBox3.Checked) listControlBindingSource1.Filter = "ArtworkInLine is null";
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
                           TotalQty = grouprows.Sum(r => r.Field<decimal>("totalqty")),
                           Balance = grouprows.Sum(r => r.Field<decimal>("balance")),
                           Totaltms = grouprows.Sum(r => r.Field<decimal>("totaltms"))
                       }).ToList();

            var bs2 = (from rows in ((DataTable)listControlBindingSource1.DataSource).AsEnumerable()
                       group rows by new { localsuppid = "Total" } into grouprows
                       select new
                       {
                           Supplier = grouprows.Key.localsuppid,
                           TotalQty = grouprows.Sum(r =>  r.Field<decimal>("totalqty")),
                           Balance = grouprows.Sum(r => r.Field<decimal>("balance")),
                           Totaltms = grouprows.Sum(r => r.Field<decimal>("totaltms"))
                       }).ToList();
            bs1.AddRange(bs2);
            grid2.DataSource = bs1;
            //grid2.AutoResizeColumns();
            
        }

        //set default supplier from style
        private void button6_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)listControlBindingSource1.DataSource;
            if (dt == null || dt.Rows.Count == 0) return;
            DataRow[] find;
            find = dt.Select("Selected = 1");
            if (find.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select rows first!", "Warnning");
                return;
            }

            foreach (DataRow item in find)
            {
                DataRow dr;
                bool found;
                if (item["inhouseosp"].ToString() == "O")
                {
                    found = MyUtility.Check.Seek(string.Format(@"select top 1 b.LocalSuppId 
                                                                                    ,(select abb from localsupp where id = b.localsuppid) as suppnm
                                                                                     from Style_Artwork a left join style_artwork_quot b on a.Ukey = b.Ukey
                                                                                     where a.StyleUkey={0} AND a.ArtworkTypeID = 'PRINTING' 
                                                                                     and b.PriceApv = 'Y'"
                                                                , item["styleukey"]), out dr, null);
                    if (found)
                    {
                        item["localsuppid"] = dr["localsuppid"];
                        item["suppnm"] = dr["suppnm"];
                    }
                }
            }
        }

        //update inline
        private void button5_Click(object sender, EventArgs e)
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
        private void button7_Click(object sender, EventArgs e)
        {
            CheckData();
        }

        private void CheckData()
        {
            DataTable dt = (DataTable)listControlBindingSource1.DataSource;
            DataRow dr;
            listControlBindingSource1.Filter = "";
            foreach (DataRow item in dt.Select("inhouseosp='O'"))
            {
                item["msg"] = "";
                item["err"] = 0;
                if (MyUtility.Check.Seek(string.Format(@"select b.priceapv,oven,wash,mockup from style_artwork  a inner join style_artwork_quot b 
                                                on a.ukey = b.ukey where a.styleukey = {0} and b.localsuppid = '{1}'"
                    , item["styleukey"], item["localsuppid"]), out dr, null))
                {
                    if (dr["priceapv"].ToString() != "Y")
                    {
                        item["msg"] = "Price was not approved";
                        item["err"] = 2;
                    }
                    else
                    {
                        if (MyUtility.Check.Empty(dr["Oven"]) || MyUtility.Check.Empty(dr["Wash"]) || MyUtility.Check.Empty(dr["Mockup"]))
                        {
                            item["msg"] = "Without oven/wash/mockup test apv.";
                            item["err"] = 1;
                        }
                    }
                }
                else
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
                if (null != dateBox1.Value)
                {
                    item["artworkinline"] = dateBox1.Value;
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
                if (null != dateBox2.Value)
                {
                    item["artworkoffline"] = dateBox2.Value;
                }
                else
                    item["artworkoffline"] = DBNull.Value;
            }
        }

        //find Style#
        private void button6_Click_1(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(listControlBindingSource1.DataSource)) return;
            int index = listControlBindingSource1.Find("Styleid", textBox2.Text.TrimEnd());
            if (index == -1)
            { MyUtility.Msg.WarningBox("Data was not found!!"); }
            else
            { listControlBindingSource1.Position = index; }
        }
    }
}
