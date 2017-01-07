using Ict;
using Ict.Win;
using Sci.Production.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Sci.Win;
using Sci.Data;
using System.Transactions;
using Sci.Win.Tools;
using System.Data.SqlClient;



namespace Sci.Production.Quality
{
    public partial class P07 : Sci.Win.Tems.Input6
    {
        DualResult result;
        string sql, ID, POID, SEQ1, SEQ2, OLDtxtWK, OLDtxtSEQ;
        DataRow ROW;
        DataRow[] rows = null;
        DataTable dtDetail = null;
        int rowsCount = 0;


        public P07(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            GenComboBox();
            this.detailgrid.ContextMenuStrip = detailgridmenus;                
        }

        private void GenComboBox()
        {
            Dictionary<String, String> Result_RowSource = new Dictionary<string, string>();
            Result_RowSource.Add("", "");
            Result_RowSource.Add("Oven Test", "Oven Test");
            Result_RowSource.Add("Wash Test", "Wash Test");
            Result_RowSource.Add("Both Test", "Both Test");
            comboBox1.DataSource = new BindingSource(Result_RowSource, null);
            comboBox1.ValueMember = "Key";
            comboBox1.DisplayMember = "Value";
        }

        //refresh
        protected override void OnDetailEntered()
        {
            DataRow dr;
            string sql_cmd = string.Format(@"select a.ID , b.StyleID , b.SeasonID , b.BrandID , b.CutInLine
	                                             ,GetSCI.MinSciDelivery , CASE WHEN a.Complete = 1 THEN 'Y' WHEN a.Complete = 0 THEN 'N' END as Complete , a.AIRLaboratoryRemark
                                            from po a 
                                            left join Orders b on a.ID = b.POID
                                            cross apply dbo.GetSCI(a.id,'') as GetSCI
                                            where a.id='{0}'", CurrentMaintain["ID"].ToString().Trim());
            if (MyUtility.Check.Seek(sql_cmd, out dr))
            {
                this.sp_text.Text = dr["id"].ToString();
                this.style_text.Text = dr["StyleID"].ToString();
                this.season_text.Text = dr["SeasonID"].ToString();
                this.brand_text.Text = dr["BrandID"].ToString();
                this.remark_text.Text = dr["AIRLaboratoryRemark"].ToString();
                this.complete_text.Text = dr["Complete"].ToString();

                //[Earliest Est. Cutting Date]
                if (dr["CutInLine"] == DBNull.Value) Cutting_text.Text = "";
                else Cutting_text.Value = Convert.ToDateTime(dr["CutInLine"]);

                //[Earliest SCI Del]
                if (dr["MinSciDelivery"] == DBNull.Value) Earliest_text.Text = "";
                else Earliest_text.Value = Convert.ToDateTime(dr["MinSciDelivery"]);

                //[Target Lead time]
                DateTime? targT = null;
                if (!MyUtility.Check.Empty(dr["CutInLine"]) && !MyUtility.Check.Empty(dr["MinSciDelivery"]))
                    targT = Sci.Production.PublicPrg.Prgs.GetTargetLeadTime(dr["CutInLine"], dr["MinSciDelivery"]);
                if (targT != null)
                    Target_text.Value = targT;
                else
                    Target_text.Text = "";

            }

            #region [% of Inspection][Completion Date]
            decimal dRowCount = DetailDatas.Count;
            string inspnum = "0";
            DataTable articleDT = (DataTable)detailgridbs.DataSource;

            if (articleDT.Rows.Count != 0)
            {
                DataRow[] articleAry = articleDT.Select("Result<>'' OR( NonOven='True' and NonWash='True') ");
                if (articleAry.Length > 0)
                {
                    inspnum = Math.Round(((decimal)articleAry.Length / dRowCount) * 100, 2).ToString();
                    Article_text.Text = inspnum + "%";
                }
                else
                {
                    Article_text.Text = "";
                }
            }
            else
            {
                Article_text.Text = "";
            }

            DateTime? CompDate,OvenDate,WashDate; 
            if (inspnum == "100")
            {
                OvenDate= MyUtility.Convert.GetDate(articleDT.Compute("Max(OvenDate)", ""));
                WashDate= MyUtility.Convert.GetDate(articleDT.Compute("Max(WashDate)", ""));
                if (MyUtility.Math.DateMinus(OvenDate,WashDate).TotalSeconds < 0)
	            {
		            CompDate = WashDate;
	            }
                else
	            {
                    CompDate = OvenDate;
	            }
                compl_text.Text = CompDate == null ? "" : ((DateTime)CompDate).ToShortDateString(); 
                ////CompDate = DateTime.Compare((DateTime)articleDT.Compute("Max(OvenDate)", ""),(DateTime)articleDT.Compute("Max(WashDate)", "")) > 0 ? (DateTime)articleDT.Compute("Max(OvenDate)", "") : (DateTime)articleDT.Compute("Max(WashDate)", "") ;                              
          
                //compl_text.Value = CompDate;
            }
            else
            {
                compl_text.Text = "";
            }
            #endregion

            //判斷Grid有無資料 , 沒資料就傳true並關閉 ContextMenu edit & delete
            //contextMenuStrip();

            base.OnDetailEntered();
        }

        protected override void OnDetailGridSetup()
        {
            Ict.Win.UI.DataGridViewCheckBoxColumn col_NonOven;
            Ict.Win.UI.DataGridViewCheckBoxColumn col_NonWash;
            Ict.Win.UI.DataGridViewTextBoxColumn col_Oven;
            Ict.Win.UI.DataGridViewTextBoxColumn col_OvenScale;
            Ict.Win.UI.DataGridViewDateBoxColumn col_OvenDate;
            Ict.Win.UI.DataGridViewTextBoxColumn col_OvenInspector;
            Ict.Win.UI.DataGridViewTextBoxColumn col_OvenRemark;
            Ict.Win.UI.DataGridViewTextBoxColumn col_Wash;
            Ict.Win.UI.DataGridViewTextBoxColumn col_Washscale;
            Ict.Win.UI.DataGridViewDateBoxColumn col_WashDate;
            Ict.Win.UI.DataGridViewTextBoxColumn col_Washinspector;
            Ict.Win.UI.DataGridViewTextBoxColumn col_WashRemark;

            DataGridViewGeneratorTextColumnSettings OvenCell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings OvenScaleCell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorDateColumnSettings OvenDateCell = new DataGridViewGeneratorDateColumnSettings();
            DataGridViewGeneratorTextColumnSettings OvenInspectorCell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings OvenRemarkCell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings WashCell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings WashScaleCell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorDateColumnSettings WashDateCell = new DataGridViewGeneratorDateColumnSettings();
            DataGridViewGeneratorTextColumnSettings WashInspectorCell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings WashRemarkCell = new DataGridViewGeneratorTextColumnSettings();

            #region CellMouseDoubleClick
 
            OvenCell.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.CurrentDetailData;
                if (dr == null) return;
                var frm = new Sci.Production.Quality.P07_Oven(false, this.CurrentDetailData["ID"].ToString(), this.sp_text.Text, this.CurrentDetailData["SEQ1"].ToString(), this.CurrentDetailData["SEQ2"].ToString(), dr);
                frm.ShowDialog(this);
                frm.Dispose();
                //this.RenewData();
            };
            OvenScaleCell.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.CurrentDetailData;
                if (dr == null) return;
                var frm = new Sci.Production.Quality.P07_Oven(false, this.CurrentDetailData["ID"].ToString(), this.sp_text.Text, this.CurrentDetailData["SEQ1"].ToString(), this.CurrentDetailData["SEQ2"].ToString(), dr);
                frm.ShowDialog(this);
                frm.Dispose();
                //this.RenewData();
            };
            OvenDateCell.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.CurrentDetailData;
                if (dr == null) return;
                var frm = new Sci.Production.Quality.P07_Oven(false, this.CurrentDetailData["ID"].ToString(), this.sp_text.Text, this.CurrentDetailData["SEQ1"].ToString(), this.CurrentDetailData["SEQ2"].ToString(), dr);
                frm.ShowDialog(this);
                frm.Dispose();
                //this.RenewData();
            };
            OvenInspectorCell.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.CurrentDetailData;
                if (dr == null) return;
                var frm = new Sci.Production.Quality.P07_Oven(false, this.CurrentDetailData["ID"].ToString(), this.sp_text.Text, this.CurrentDetailData["SEQ1"].ToString(), this.CurrentDetailData["SEQ2"].ToString(), dr);
                frm.ShowDialog(this);
                frm.Dispose();
                //this.RenewData();
            };
            OvenRemarkCell.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.CurrentDetailData;
                if (dr == null) return;
                var frm = new Sci.Production.Quality.P07_Oven(false, this.CurrentDetailData["ID"].ToString(), this.sp_text.Text, this.CurrentDetailData["SEQ1"].ToString(), this.CurrentDetailData["SEQ2"].ToString(), dr);
                frm.ShowDialog(this);
                frm.Dispose();
                //this.RenewData();
            };

            WashCell.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.CurrentDetailData;
                if (dr == null) return;
                var frm = new Sci.Production.Quality.P07_Wash(false, this.CurrentDetailData["ID"].ToString(), this.sp_text.Text, this.CurrentDetailData["SEQ1"].ToString(), this.CurrentDetailData["SEQ2"].ToString(), dr);
                frm.ShowDialog(this);
                frm.Dispose();
               // this.RenewData();
            };
            WashScaleCell.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.CurrentDetailData;
                if (dr == null) return;
                var frm = new Sci.Production.Quality.P07_Wash(false, this.CurrentDetailData["ID"].ToString(), this.sp_text.Text, this.CurrentDetailData["SEQ1"].ToString(), this.CurrentDetailData["SEQ2"].ToString(), dr);
                frm.ShowDialog(this);
                frm.Dispose();
                //this.RenewData();
            };
            WashDateCell.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.CurrentDetailData;
                if (dr == null) return;
                var frm = new Sci.Production.Quality.P07_Wash(false, this.CurrentDetailData["ID"].ToString(), this.sp_text.Text, this.CurrentDetailData["SEQ1"].ToString(), this.CurrentDetailData["SEQ2"].ToString(), dr);
                frm.ShowDialog(this);
                frm.Dispose();
                //this.RenewData();
            };
            WashInspectorCell.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.CurrentDetailData;
                if (dr == null) return;
                var frm = new Sci.Production.Quality.P07_Wash(false, this.CurrentDetailData["ID"].ToString(), this.sp_text.Text, this.CurrentDetailData["SEQ1"].ToString(), this.CurrentDetailData["SEQ2"].ToString(), dr);
                frm.ShowDialog(this);
                frm.Dispose();
                //this.RenewData();
            };
            WashRemarkCell.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.CurrentDetailData;
                if (dr == null) return;
                var frm = new Sci.Production.Quality.P07_Wash(false, this.CurrentDetailData["ID"].ToString(), this.sp_text.Text, this.CurrentDetailData["SEQ1"].ToString(), this.CurrentDetailData["SEQ2"].ToString(), dr);
                frm.ShowDialog(this);
                frm.Dispose();
                //this.RenewData();
            };
            #endregion

            Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("SEQ", header: "SEQ#", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("ExportID", header: "WKNO", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Date("WhseArrival", header: "Arrive W/H Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("SCIRefno", header: "SCI Refno", width: Widths.AnsiChars(17), iseditingreadonly: true)
                .Text("BrandRefno", header: "Refno", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("Supplier", header: "Supplier", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("Colorid", header: "Color", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("Sizespec", header: "Size", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Numeric("ArriveQty", header: "Arrive Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 8, iseditingreadonly: true)
                .Date("InspDeadline", header: "Insp. DeadLine", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Result", header: "Over all Result", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .CheckBox("NonOven", header: "Oven N/A", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_NonOven)
                .Text("Oven", header: "Oven Result", width: Widths.AnsiChars(5), iseditingreadonly: true, settings: OvenCell).Get(out col_Oven)
                .Text("OvenScale", header: "Oven Scale", width: Widths.AnsiChars(5), iseditingreadonly: true, settings: OvenScaleCell).Get(out col_OvenScale)
                .Date("OvenDate", header: "Oven Last Test Date", width: Widths.AnsiChars(10), iseditingreadonly: true, settings: OvenDateCell).Get(out col_OvenDate)
                .Text("OvenInspector", header: "Oven Lab Tech", width: Widths.AnsiChars(10), iseditingreadonly: true, settings: OvenInspectorCell).Get(out col_OvenInspector)
                .Text("OvenRemark", header: "Remark", width: Widths.AnsiChars(10), iseditingreadonly: true, settings: OvenRemarkCell).Get(out col_OvenRemark)
                .CheckBox("nonWash", header: "Wash N/A", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_NonWash)
                .Text("Wash", header: "Wash Result", width: Widths.AnsiChars(5), iseditingreadonly: true, settings: WashCell).Get(out col_Wash)
                .Text("Washscale", header: "Wash Scale", width: Widths.AnsiChars(5), iseditingreadonly: true, settings: WashScaleCell).Get(out col_Washscale)
                .Date("WashDate", header: "Wash Last Date", width: Widths.AnsiChars(10), iseditingreadonly: true, settings: WashDateCell).Get(out col_WashDate)
                .Text("Washinspector", header: "Wash Lab Tech", width: Widths.AnsiChars(10), iseditingreadonly: true, settings: WashInspectorCell).Get(out col_Washinspector)
                .Text("WashRemark", header: "Remark", width: Widths.AnsiChars(10), iseditingreadonly: true, settings: WashRemarkCell).Get(out col_WashRemark)
                .Text("ReceivingID", header: "Receiving ID", width: Widths.AnsiChars(15), iseditingreadonly: true);

            col_NonOven.DefaultCellStyle.BackColor = Color.Pink;
            col_NonWash.DefaultCellStyle.BackColor = Color.Pink;
            col_Oven.DefaultCellStyle.BackColor = Color.PaleGoldenrod;
            col_OvenScale.DefaultCellStyle.BackColor = Color.PaleGoldenrod;
            col_OvenDate.DefaultCellStyle.BackColor = Color.PaleGoldenrod;
            col_OvenInspector.DefaultCellStyle.BackColor = Color.PaleGoldenrod;
            col_OvenRemark.DefaultCellStyle.BackColor = Color.PaleGoldenrod;
            col_Wash.DefaultCellStyle.BackColor = Color.SkyBlue;
            col_Washscale.DefaultCellStyle.BackColor = Color.SkyBlue;
            col_WashDate.DefaultCellStyle.BackColor = Color.SkyBlue;
            col_Washinspector.DefaultCellStyle.BackColor = Color.SkyBlue;
            col_WashRemark.DefaultCellStyle.BackColor = Color.SkyBlue;

        }
        protected override void OnFormLoaded()
        {
            
            detailgridmenus.Items.Clear();
            Helper.Controls.ContextMenu.Generator(this.detailgridmenus).Menu("Modify Oven Test", onclick: (s, e) => OvenTest());
            Helper.Controls.ContextMenu.Generator(this.detailgridmenus).Menu("Modify Wash Test", onclick: (s, e) => WashTest());

            base.OnFormLoaded();
        }

        private void OvenTest()
        {           
            var dr = this.CurrentDetailData;
            if (dr == null) return;
            string currentID = this.CurrentDetailData["ID"].ToString();
            string currentseq1 = this.CurrentDetailData["SEQ1"].ToString();
            string currentseq2 = this.CurrentDetailData["SEQ2"].ToString();

            Sci.Production.Quality.P07_Oven callOvenDetailForm=new P07_Oven(true,this.CurrentDetailData["ID"].ToString(),this.sp_text.Text,this.CurrentDetailData["SEQ1"].ToString(), this.CurrentDetailData["SEQ2"].ToString() , dr);
            
            callOvenDetailForm.ShowDialog(this);
            callOvenDetailForm.Dispose();
            
            this.RenewData();
            OnDetailEntered();

            int rowindex = 0;
            for (int rIdx = 0; rIdx < detailgrid.Rows.Count; rIdx++)
            {
                DataGridViewRow dvr = detailgrid.Rows[rIdx];
                DataRow row = ((DataRowView)dvr.DataBoundItem).Row;

                if (row["ID"].ToString()==currentID && row["SEQ1"].ToString() == currentseq1 && row["SEQ2"].ToString() == currentseq2)
                {
                    rowindex = rIdx;
                    break;
                }
            }
            detailgrid.SelectRowTo(rowindex);
        }
        private void WashTest()
        {
            var dr = this.CurrentDetailData;
            if (dr == null) return;
            Sci.Production.Quality.P07_Wash callWasHDetailForm = new P07_Wash(true, this.CurrentDetailData["ID"].ToString(), this.sp_text.Text, this.CurrentDetailData["SEQ1"].ToString(), this.CurrentDetailData["SEQ2"].ToString(), dr);
            callWasHDetailForm.ShowDialog(this);
            callWasHDetailForm.Dispose();
            this.RenewData();
            OnDetailEntered();
        }
        protected override DualResult OnRenewDataDetailPost(RenewDataPostEventArgs e)
        {
            DataTable dt = (DataTable)e.Details;
            dt.Columns.Add("SEQ", typeof(string));
            dt.Columns.Add("ExportID", typeof(string));
            dt.Columns.Add("WhseArrival", typeof(DateTime));
            dt.Columns.Add("SCIRefno", typeof(string));
            dt.Columns.Add("BrandRefno", typeof(string));
            dt.Columns.Add("supplier", typeof(string));
            dt.Columns.Add("ColorID", typeof(string));
            dt.Columns.Add("Sizespec", typeof(string));
            dt.Columns.Add("ArriveQty", typeof(decimal));
            dt.Columns.Add("ReceivingID", typeof(string));

            foreach (DataRow dr in dt.Rows)
            {
                ID = dr["ID"].ToString().Trim();
                POID = dr["POID"].ToString().Trim();
                SEQ1 = dr["SEQ1"].ToString().Trim();
                SEQ2 = dr["SEQ2"].ToString().Trim();
                sql = string.Format(@"select C.ExportID , C.WhseArrival , B.SCIRefno , B.Refno , B.Suppid + '-' + D.AbbEN as supplier
                                            ,E.ColorID , E.Sizespec , B.ArriveQty , B.ReceivingID
                                    from AIR_Laboratory A
                                    left join AIR B on A.id=B.id
                                    left join Receiving C on C.id=B.receivingID
                                    left join Supp D on D.ID=B.Suppid
                                    left join PO_Supp_Detail E on E.ID=A.POID and E.SEQ1=A.SEQ1 and E.SEQ2=A.SEQ2
                                    where A.id={0} and A.poid='{1}' and A.seq1='{2}' and A.seq2='{3}'"
                                    , ID, POID, SEQ1, SEQ2);
                MyUtility.Check.Seek(sql, out ROW);

                dr["SEQ"] = SEQ1 + "-" + SEQ2;
                dr["ExportID"] = ROW["ExportID"];
                dr["WhseArrival"] = ROW["WhseArrival"];
                dr["SCIRefno"] = ROW["SCIRefno"];
                dr["BrandRefno"] = ROW["Refno"];
                dr["supplier"] = ROW["supplier"];
                dr["ColorID"] = ROW["ColorID"];
                dr["Sizespec"] = ROW["Sizespec"];
                dr["ArriveQty"] = ROW["ArriveQty"];
                dr["ReceivingID"] = ROW["ReceivingID"];

            }
            return base.OnRenewDataDetailPost(e);
        }

        //[Find]
        private void btnFind_Click(object sender, EventArgs e)
        {
            int Count = DetailDatas.Count;
            if (Count == 0) return;
            if (MyUtility.Check.Empty(txtWK.Text) && MyUtility.Check.Empty(txtSEQ.Text)) return;

            int index;
            if (txtWK.Text != OLDtxtWK || txtSEQ.Text != OLDtxtSEQ)
            {
                OLDtxtWK = txtWK.Text;
                OLDtxtSEQ = txtSEQ.Text;

                dtDetail = (DataTable)detailgridbs.DataSource;

                if (!MyUtility.Check.Empty(txtWK.Text) && !MyUtility.Check.Empty(txtSEQ.Text))
                    sql = string.Format(@"ExportID like '%{0}%' and SEQ like '%{1}%'", txtWK.Text.Trim() , txtSEQ.Text.Trim());
                else if (!MyUtility.Check.Empty(txtWK.Text))
                    sql = string.Format(@"ExportID like '%{0}%'", txtWK.Text.Trim());
                else if (!MyUtility.Check.Empty(txtSEQ.Text))
                    sql = string.Format(@"SEQ like '%{0}%'", txtSEQ.Text.Trim());

                rows = dtDetail.Select(sql);
                if (rows.Length == 0) return;
                index = dtDetail.Rows.IndexOf(rows[0]);
                detailgrid.SelectRowTo(index);
                rowsCount = 0;
            }
            else
            {
                if (rows.Length == 0) return;
                rowsCount++;
                if (rowsCount >= rows.Length) rowsCount = 0;
                index = dtDetail.Rows.IndexOf(rows[rowsCount]);
                detailgrid.SelectRowTo(index);
            }
        }

        //[Batch update N/A]
        private void btnBatchUpdate_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)detailgridbs.DataSource;           
            if (MyUtility.Check.Empty(comboBox1.SelectedValue2))
            {
                MyUtility.Msg.WarningBox("< ComboBox > can not be empty!");
                return;
            }

            if (comboBox1.SelectedValue2.ToString() == "Oven Test")
            {
                sql = string.Format("update AIR_Laboratory set NonOven=1 where POID='{0}'", CurrentMaintain["ID"].ToString().Trim());
                result = DBProxy.Current.Execute(null,sql);
                if (result)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dt.Rows[i]["NonOven"] = true;
                    }
                }
            }
            else if (comboBox1.SelectedValue2.ToString() == "Wash Test")
            {
                sql = string.Format("update AIR_Laboratory set NonWash=1 where POID='{0}'", CurrentMaintain["ID"].ToString().Trim());
                result = DBProxy.Current.Execute(null, sql);
                if (result)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dt.Rows[i]["NonWash"] = true;
                    }
                }
            }
            else if (comboBox1.SelectedValue2.ToString() == "Both Test")
            {
                sql = string.Format("update AIR_Laboratory set NonOven=1 , NonWash=1 where POID='{0}'", CurrentMaintain["ID"].ToString().Trim());
                result = DBProxy.Current.Execute(null, sql);
                if (result)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dt.Rows[i]["NonWash"] = true;
                        dt.Rows[i]["NonOven"] = true;
                    }
                }
            }
            this.detailgrid.Update();
            this.detailgrid.Refresh();
            
        }


    }
}
