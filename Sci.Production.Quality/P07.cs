using Ict;
using Ict.Win;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Sci.Data;
using System.Transactions;

namespace Sci.Production.Quality
{
    public partial class P07 : Win.Tems.Input6
    {
        private DualResult result;
        private string sql;
        private string ID;
        private string POID;
        private string SEQ1;
        private string SEQ2;
        private string OLDtxtWK;
        private string OLDtxtSEQ;
        private DataRow ROW;
        private DataRow[] rows = null;
        private DataTable dtDetail = null;
        private int rowsCount = 0;

        public P07(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.GenComboBox();
            this.detailgrid.ContextMenuStrip = this.detailgridmenus;
        }

        private void GenComboBox()
        {
            Dictionary<string, string> result_RowSource = new Dictionary<string, string>();
            result_RowSource.Add(string.Empty, string.Empty);
            result_RowSource.Add("Oven Test", "Oven Test");
            result_RowSource.Add("Wash Test", "Wash Test");
            result_RowSource.Add("Both Test", "Both Test");
            this.comboOvenWashBoth.DataSource = new BindingSource(result_RowSource, null);
            this.comboOvenWashBoth.ValueMember = "Key";
            this.comboOvenWashBoth.DisplayMember = "Value";
        }

        // refresh

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            DataRow dr;
            this.detailgrid.AutoResizeColumns();
            string sql_cmd = string.Format(
                @"select a.ID , b.StyleID , b.SeasonID , b.BrandID , b.CutInLine
	                                             ,GetSCI.MinSciDelivery , CASE WHEN a.Complete = 1 THEN 'Y' WHEN a.Complete = 0 THEN 'N' END as Complete , a.AIRLaboratoryRemark
                                            from po a WITH (NOLOCK) 
                                            left join Orders b WITH (NOLOCK) on a.ID = b.POID
                                            cross apply dbo.GetSCI(a.id,'') as GetSCI
                                            where a.id='{0}'", this.CurrentMaintain["ID"].ToString().Trim());
            if (MyUtility.Check.Seek(sql_cmd, out dr))
            {
                this.displaySP.Text = dr["id"].ToString();
                this.displayStyle.Text = dr["StyleID"].ToString();
                this.displaySeason.Text = dr["SeasonID"].ToString();
                this.displayBrand.Text = dr["BrandID"].ToString();
                this.editRemark.Text = dr["AIRLaboratoryRemark"].ToString();
                this.displayMtlCmplt.Text = dr["Complete"].ToString();

                // [Earliest Est. Cutting Date]
                if (dr["CutInLine"] == DBNull.Value)
                {
                    this.dateEarliestEstCuttingDate.Text = string.Empty;
                }
                else
                {
                    this.dateEarliestEstCuttingDate.Value = Convert.ToDateTime(dr["CutInLine"]);
                }

                // [Target Lead time]
                DateTime? targT = null;
                if (!MyUtility.Check.Empty(dr["CutInLine"]) && !MyUtility.Check.Empty(dr["MinSciDelivery"]))
                {
                    targT = PublicPrg.Prgs.GetTargetLeadTime(dr["CutInLine"], dr["MinSciDelivery"]);
                }

                if (targT != null)
                {
                    this.dateTargetLeadtime.Value = targT;
                }
                else
                {
                    this.dateTargetLeadtime.Text = string.Empty;
                }
            }

            #region [% of Inspection][Completion Date]
            decimal dRowCount = this.DetailDatas.Count;
            string inspnum = "0";
            DataTable articleDT = (DataTable)this.detailgridbs.DataSource;

            if (articleDT.Rows.Count != 0)
            {
                DataRow[] articleAry = articleDT.Select("Result<>'' OR( NonOven='True' and NonWash='True') ");
                if (articleAry.Length > 0)
                {
                    inspnum = Math.Round(((decimal)articleAry.Length / dRowCount) * 100, 2).ToString();
                }
            }

            DateTime? compDate, ovenDate, washDate;
            if (inspnum == "100")
            {
                ovenDate = MyUtility.Convert.GetDate(articleDT.Compute("Max(OvenDate)", string.Empty));
                washDate = MyUtility.Convert.GetDate(articleDT.Compute("Max(WashDate)", string.Empty));
                if (MyUtility.Math.DateMinus(ovenDate, washDate).TotalSeconds < 0)
                {
                    compDate = washDate;
                }
                else
                {
                    compDate = ovenDate;
                }

                this.dateCompletionDate.Text = compDate == null ? string.Empty : ((DateTime)compDate).ToShortDateString();
            }
            else
            {
                this.dateCompletionDate.Text = string.Empty;
            }
            #endregion

            // 判斷Grid有無資料 , 沒資料就傳true並關閉 ContextMenu edit & delete
            // contextMenuStrip();
            base.OnDetailEntered();
        }

        /// <inheritdoc/>
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

            DataGridViewGeneratorTextColumnSettings ovenCell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings ovenScaleCell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorDateColumnSettings ovenDateCell = new DataGridViewGeneratorDateColumnSettings();
            DataGridViewGeneratorTextColumnSettings ovenInspectorCell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings ovenRemarkCell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings washCell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings washScaleCell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorDateColumnSettings washDateCell = new DataGridViewGeneratorDateColumnSettings();
            DataGridViewGeneratorTextColumnSettings washInspectorCell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings washRemarkCell = new DataGridViewGeneratorTextColumnSettings();

            #region CellMouseDoubleClick

            ovenCell.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.CurrentDetailData;
                if (dr == null)
                {
                    return;
                }

                var frm = new P07_Oven(false, this.CurrentDetailData["ID"].ToString(), this.displaySP.Text, this.CurrentDetailData["SEQ1"].ToString(), this.CurrentDetailData["SEQ2"].ToString(), dr);
                frm.ShowDialog(this);
                frm.Dispose();
            };
            ovenScaleCell.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.CurrentDetailData;
                if (dr == null)
                {
                    return;
                }

                var frm = new P07_Oven(false, this.CurrentDetailData["ID"].ToString(), this.displaySP.Text, this.CurrentDetailData["SEQ1"].ToString(), this.CurrentDetailData["SEQ2"].ToString(), dr);
                frm.ShowDialog(this);
                frm.Dispose();
            };
            ovenDateCell.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.CurrentDetailData;
                if (dr == null)
                {
                    return;
                }

                var frm = new P07_Oven(false, this.CurrentDetailData["ID"].ToString(), this.displaySP.Text, this.CurrentDetailData["SEQ1"].ToString(), this.CurrentDetailData["SEQ2"].ToString(), dr);
                frm.ShowDialog(this);
                frm.Dispose();
            };
            ovenInspectorCell.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.CurrentDetailData;
                if (dr == null)
                {
                    return;
                }

                var frm = new P07_Oven(false, this.CurrentDetailData["ID"].ToString(), this.displaySP.Text, this.CurrentDetailData["SEQ1"].ToString(), this.CurrentDetailData["SEQ2"].ToString(), dr);
                frm.ShowDialog(this);
                frm.Dispose();
            };
            ovenRemarkCell.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.CurrentDetailData;
                if (dr == null)
                {
                    return;
                }

                var frm = new P07_Oven(false, this.CurrentDetailData["ID"].ToString(), this.displaySP.Text, this.CurrentDetailData["SEQ1"].ToString(), this.CurrentDetailData["SEQ2"].ToString(), dr);
                frm.ShowDialog(this);
                frm.Dispose();
            };

            washCell.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.CurrentDetailData;
                if (dr == null)
                {
                    return;
                }

                var frm = new P07_Wash(false, this.CurrentDetailData["ID"].ToString(), this.displaySP.Text, this.CurrentDetailData["SEQ1"].ToString(), this.CurrentDetailData["SEQ2"].ToString(), dr);
                frm.ShowDialog(this);
                frm.Dispose();
            };
            washScaleCell.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.CurrentDetailData;
                if (dr == null)
                {
                    return;
                }

                var frm = new P07_Wash(false, this.CurrentDetailData["ID"].ToString(), this.displaySP.Text, this.CurrentDetailData["SEQ1"].ToString(), this.CurrentDetailData["SEQ2"].ToString(), dr);
                frm.ShowDialog(this);
                frm.Dispose();
            };
            washDateCell.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.CurrentDetailData;
                if (dr == null)
                {
                    return;
                }

                var frm = new P07_Wash(false, this.CurrentDetailData["ID"].ToString(), this.displaySP.Text, this.CurrentDetailData["SEQ1"].ToString(), this.CurrentDetailData["SEQ2"].ToString(), dr);
                frm.ShowDialog(this);
                frm.Dispose();
            };
            washInspectorCell.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.CurrentDetailData;
                if (dr == null)
                {
                    return;
                }

                var frm = new P07_Wash(false, this.CurrentDetailData["ID"].ToString(), this.displaySP.Text, this.CurrentDetailData["SEQ1"].ToString(), this.CurrentDetailData["SEQ2"].ToString(), dr);
                frm.ShowDialog(this);
                frm.Dispose();
            };
            washRemarkCell.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.CurrentDetailData;
                if (dr == null)
                {
                    return;
                }

                var frm = new P07_Wash(false, this.CurrentDetailData["ID"].ToString(), this.displaySP.Text, this.CurrentDetailData["SEQ1"].ToString(), this.CurrentDetailData["SEQ2"].ToString(), dr);
                frm.ShowDialog(this);
                frm.Dispose();
            };
            #endregion

            this.Helper.Controls.Grid.Generator(this.detailgrid)
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
                .Text("Oven", header: "Oven Result", width: Widths.AnsiChars(5), iseditingreadonly: true, settings: ovenCell).Get(out col_Oven)
                .Text("OvenScale", header: "Oven Scale", width: Widths.AnsiChars(5), iseditingreadonly: true, settings: ovenScaleCell).Get(out col_OvenScale)
                .Date("OvenDate", header: "Oven Last Test Date", width: Widths.AnsiChars(10), iseditingreadonly: true, settings: ovenDateCell).Get(out col_OvenDate)
                .Text("OvenInspector", header: "Oven Lab Tech", width: Widths.AnsiChars(10), iseditingreadonly: true, settings: ovenInspectorCell).Get(out col_OvenInspector)
                .Text("OvenRemark", header: "Remark", width: Widths.AnsiChars(10), iseditingreadonly: true, settings: ovenRemarkCell).Get(out col_OvenRemark)
                .CheckBox("nonWash", header: "Wash N/A", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_NonWash)
                .Text("Wash", header: "Wash Result", width: Widths.AnsiChars(5), iseditingreadonly: true, settings: washCell).Get(out col_Wash)
                .Text("Washscale", header: "Wash Scale", width: Widths.AnsiChars(5), iseditingreadonly: true, settings: washScaleCell).Get(out col_Washscale)
                .Date("WashDate", header: "Wash Last Date", width: Widths.AnsiChars(10), iseditingreadonly: true, settings: washDateCell).Get(out col_WashDate)
                .Text("Washinspector", header: "Wash Lab Tech", width: Widths.AnsiChars(10), iseditingreadonly: true, settings: washInspectorCell).Get(out col_Washinspector)
                .Text("WashRemark", header: "Remark", width: Widths.AnsiChars(10), iseditingreadonly: true, settings: washRemarkCell).Get(out col_WashRemark)
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

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            this.detailgridmenus.Items.Clear();
            this.Helper.Controls.ContextMenu.Generator(this.detailgridmenus).Menu("Modify Oven Test", onclick: (s, e) => this.OvenTest());
            this.Helper.Controls.ContextMenu.Generator(this.detailgridmenus).Menu("Modify Wash Test", onclick: (s, e) => this.WashTest());

            base.OnFormLoaded();
        }

        private void OvenTest()
        {
            if (!this.IsSupportEdit || this.EditMode)
            {
                return;
            }

            var dr = this.CurrentDetailData;
            if (dr == null)
            {
                return;
            }

            string currentID = this.CurrentDetailData["ID"].ToString();
            string currentseq1 = this.CurrentDetailData["SEQ1"].ToString();
            string currentseq2 = this.CurrentDetailData["SEQ2"].ToString();

            P07_Oven callOvenDetailForm = new P07_Oven(true, this.CurrentDetailData["ID"].ToString(), this.displaySP.Text, this.CurrentDetailData["SEQ1"].ToString(), this.CurrentDetailData["SEQ2"].ToString(), dr);

            callOvenDetailForm.ShowDialog(this);
            callOvenDetailForm.Dispose();

            this.RenewData();
            this.OnDetailEntered();

            int rowindex = 0;
            for (int rIdx = 0; rIdx < this.detailgrid.Rows.Count; rIdx++)
            {
                DataGridViewRow dvr = this.detailgrid.Rows[rIdx];
                DataRow row = ((DataRowView)dvr.DataBoundItem).Row;

                if (row["ID"].ToString() == currentID && row["SEQ1"].ToString() == currentseq1 && row["SEQ2"].ToString() == currentseq2)
                {
                    rowindex = rIdx;
                    break;
                }
            }

            this.detailgrid.SelectRowTo(rowindex);
        }

        private void WashTest()
        {
            if (!this.IsSupportEdit || this.EditMode)
            {
                return;
            }

            var dr = this.CurrentDetailData;
            if (dr == null)
            {
                return;
            }

            P07_Wash callWasHDetailForm = new P07_Wash(true, this.CurrentDetailData["ID"].ToString(), this.displaySP.Text, this.CurrentDetailData["SEQ1"].ToString(), this.CurrentDetailData["SEQ2"].ToString(), dr);
            callWasHDetailForm.ShowDialog(this);
            callWasHDetailForm.Dispose();
            this.RenewData();
            this.OnDetailEntered();
        }

        /// <inheritdoc/>
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
                this.ID = dr["ID"].ToString().Trim();
                this.POID = dr["POID"].ToString().Trim();
                this.SEQ1 = dr["SEQ1"].ToString().Trim();
                this.SEQ2 = dr["SEQ2"].ToString().Trim();
                this.sql = string.Format(
                    @"select C.ExportID , C.WhseArrival , B.SCIRefno , B.Refno , B.Suppid + '-' + D.AbbEN as supplier
                                            ,E.ColorID , E.Sizespec , B.ArriveQty , B.ReceivingID
                                    from AIR_Laboratory A WITH (NOLOCK) 
                                    left join AIR B WITH (NOLOCK) on A.id=B.id
                                    left join Receiving C WITH (NOLOCK) on C.id=B.receivingID
                                    left join Supp D WITH (NOLOCK) on D.ID=B.Suppid
                                    left join PO_Supp_Detail E WITH (NOLOCK) on E.ID=A.POID and E.SEQ1=A.SEQ1 and E.SEQ2=A.SEQ2
                                    where A.id={0} and A.poid='{1}' and A.seq1='{2}' and A.seq2='{3}'",
                    this.ID, this.POID, this.SEQ1, this.SEQ2);
                MyUtility.Check.Seek(this.sql, out this.ROW);

                dr["SEQ"] = this.SEQ1 + "-" + this.SEQ2;
                dr["ExportID"] = this.ROW["ExportID"];
                dr["WhseArrival"] = this.ROW["WhseArrival"];
                dr["SCIRefno"] = this.ROW["SCIRefno"];
                dr["BrandRefno"] = this.ROW["Refno"];
                dr["supplier"] = this.ROW["supplier"];
                dr["ColorID"] = this.ROW["ColorID"];
                dr["Sizespec"] = this.ROW["Sizespec"];
                dr["ArriveQty"] = this.ROW["ArriveQty"];
                dr["ReceivingID"] = this.ROW["ReceivingID"];
            }

            return base.OnRenewDataDetailPost(e);
        }

        // [Find]
        private void BtnFind_Click(object sender, EventArgs e)
        {
            int count = this.DetailDatas.Count;
            if (count == 0)
            {
                return;
            }

            if (MyUtility.Check.Empty(this.txtLocateforWK.Text) && MyUtility.Check.Empty(this.txtSEQ.Text))
            {
                return;
            }

            int index;
            if (this.txtLocateforWK.Text != this.OLDtxtWK || this.txtSEQ.Text != this.OLDtxtSEQ)
            {
                this.OLDtxtWK = this.txtLocateforWK.Text;
                this.OLDtxtSEQ = this.txtSEQ.Text;

                this.dtDetail = (DataTable)this.detailgridbs.DataSource;

                if (!MyUtility.Check.Empty(this.txtLocateforWK.Text) && !MyUtility.Check.Empty(this.txtSEQ.Text))
                {
                    this.sql = string.Format(@"ExportID like '%{0}%' and SEQ like '%{1}%'", this.txtLocateforWK.Text.Trim(), this.txtSEQ.Text.Trim());
                }
                else if (!MyUtility.Check.Empty(this.txtLocateforWK.Text))
                {
                    this.sql = string.Format(@"ExportID like '%{0}%'", this.txtLocateforWK.Text.Trim());
                }
                else if (!MyUtility.Check.Empty(this.txtSEQ.Text))
                {
                    this.sql = string.Format(@"SEQ like '%{0}%'", this.txtSEQ.Text.Trim());
                }

                this.rows = this.dtDetail.Select(this.sql);
                if (this.rows.Length == 0)
                {
                    return;
                }

                index = this.dtDetail.Rows.IndexOf(this.rows[0]);
                this.detailgrid.SelectRowTo(index);
                this.rowsCount = 0;
            }
            else
            {
                if (this.rows.Length == 0)
                {
                    return;
                }

                this.rowsCount++;
                if (this.rowsCount >= this.rows.Length)
                {
                    this.rowsCount = 0;
                }

                index = this.dtDetail.Rows.IndexOf(this.rows[this.rowsCount]);
                this.detailgrid.SelectRowTo(index);
            }
        }

        // [Batch update N/A]
        private void BtnBatchUpdateNA_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)this.detailgridbs.DataSource;
            if (MyUtility.Check.Empty(this.comboOvenWashBoth.SelectedValue2))
            {
                MyUtility.Msg.WarningBox("< ComboBox > can not be empty!");
                return;
            }

            if (this.comboOvenWashBoth.SelectedValue2.ToString() == "Oven Test")
            {
                this.sql = string.Format("update AIR_Laboratory set NonOven=1 where POID='{0}'", this.CurrentMaintain["ID"].ToString().Trim());
                this.result = DBProxy.Current.Execute(null, this.sql);
                if (this.result)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dt.Rows[i]["NonOven"] = true;
                    }
                }
            }
            else if (this.comboOvenWashBoth.SelectedValue2.ToString() == "Wash Test")
            {
                this.sql = string.Format("update AIR_Laboratory set NonWash=1 where POID='{0}'", this.CurrentMaintain["ID"].ToString().Trim());
                this.result = DBProxy.Current.Execute(null, this.sql);
                if (this.result)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dt.Rows[i]["NonWash"] = true;
                    }
                }
            }
            else if (this.comboOvenWashBoth.SelectedValue2.ToString() == "Both Test")
            {
                this.sql = string.Format("update AIR_Laboratory set NonOven=1 , NonWash=1 where POID='{0}'", this.CurrentMaintain["ID"].ToString().Trim());
                this.result = DBProxy.Current.Execute(null, this.sql);
                if (this.result)
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

        /// <inheritdoc/>
        protected override void ClickSaveAfter()
        {
            DualResult upResult;
            TransactionScope transactionscope = new TransactionScope();
            using (transactionscope)
            {
                try
                {
                    // 更新PO.AIRLabInspPercent
                    if (!(upResult = DBProxy.Current.Execute(null, $"exec UpdateInspPercent 'AIRLab','{this.CurrentMaintain["ID"]}'")))
                    {
                        transactionscope.Dispose();
                        return;
                    }

                    transactionscope.Complete();
                    transactionscope.Dispose();
                }
                catch (Exception ex)
                {
                    transactionscope.Dispose();
                    this.ShowErr("Commit transaction error.", ex);
                    return;
                }
            }

            transactionscope.Dispose();
            transactionscope = null;
            this.RenewData();
            base.ClickSaveAfter();
        }
    }
}
