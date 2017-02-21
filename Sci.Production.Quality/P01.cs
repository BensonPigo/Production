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
using Sci;


namespace Sci.Production.Quality
{
    public partial class P01 : Sci.Win.Tems.Input6
    {
      
        private string loginID = Sci.Env.User.UserID;
        private string keyWord = Sci.Env.User.Keyword;
       
        int index;
        string find = "";
        DataRow[] find_dr;

        public P01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();

            detailgrid.ContextMenuStrip = gridmenu;
        }

        public P01(string Poid) //for Form直接call form
        {
            InitializeComponent();
            DefaultFilter = string.Format("ID = '{0}'", Poid);
            InsertDetailGridOnDoubleClick = false;
            IsSupportEdit = false;
            detailgrid.ContextMenuStrip = gridmenu;
        }

        protected override Ict.DualResult OnDetailSelectCommandPrepare(Win.Tems.InputMasterDetail.PrepareDetailSelectCommandEventArgs e)
        {
            
            string masterID = (e.Master == null) ? "" : e.Master["id"].ToString();
            string cmd = string.Format(
                @"Select a.id,a.poid,SEQ1,SEQ2,Receivingid,Refno,SCIRefno,Suppid,
                ArriveQty,InspDeadline,Result,
                PhysicalEncode,WeightEncode,ShadeBondEncode,ContinuityEncode,
                NonPhysical,Physical,TotalInspYds,PhysicalDate,Physical,
                NonWeight, Weight,WeightDate,Weight,
                NonShadebond,Shadebond,ShadebondDate,shadebond,
                NonContinuity,Continuity,ContinuityDate,Continuity,
                a.Status,ReplacementReportID,(seq1+seq2) as seq,
                (Select weavetypeid from Fabric b WITH (NOLOCK) where b.SCIRefno =a.SCIrefno) as weavetypeid,
                c.Exportid,c.whseArrival,dbo.getPass1(a.Approve) as approve1,approveDate,approve,
                (Select d.colorid from PO_Supp_Detail d WITH (NOLOCK) Where d.id = a.poid and d.seq1 = a.seq1 and d.seq2 = a.seq2) as Colorid,
                (Select ID+' - '+ AbbEn From Supp WITH (NOLOCK) Where a.suppid = supp.id) as SuppEn,
                c.ExportID as Wkno
                From FIR a WITH (NOLOCK) Left join Receiving c WITH (NOLOCK) on c.id = a.receivingid
                Where a.poid='{0}' order by seq1,seq2  ", masterID);
            this.DetailSelectCommand = cmd;
            return base.OnDetailSelectCommandPrepare(e);
        }

        protected override void OnDetailGridSetup()//Grid 設定
        {
            base.OnDetailGridSetup();

            DataGridViewGeneratorCheckBoxColumnSettings nonPhy = new DataGridViewGeneratorCheckBoxColumnSettings();
            DataGridViewGeneratorCheckBoxColumnSettings nonWei = new DataGridViewGeneratorCheckBoxColumnSettings();
            DataGridViewGeneratorCheckBoxColumnSettings nonSha = new DataGridViewGeneratorCheckBoxColumnSettings();
            DataGridViewGeneratorCheckBoxColumnSettings nonCon = new DataGridViewGeneratorCheckBoxColumnSettings();
            DataGridViewGeneratorTextColumnSettings phy = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorDateColumnSettings phyD = new DataGridViewGeneratorDateColumnSettings();
            DataGridViewGeneratorNumericColumnSettings phyYds = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorTextColumnSettings Wei = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorDateColumnSettings WeiD = new DataGridViewGeneratorDateColumnSettings();
            DataGridViewGeneratorTextColumnSettings sha = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorDateColumnSettings shaD = new DataGridViewGeneratorDateColumnSettings();
            DataGridViewGeneratorTextColumnSettings Con = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorDateColumnSettings ConD = new DataGridViewGeneratorDateColumnSettings();
            #region ClickEvent
            phy.CellMouseDoubleClick += (s, e) =>
            {
                    var dr = this.CurrentDetailData; if (null == dr) return;
                    var frm = new Sci.Production.Quality.P01_PhysicalInspection(false, CurrentDetailData["ID"].ToString(), null, null, dr);
                    frm.ShowDialog(this);
                    frm.Dispose();
                    this.RenewData();
            };
            phyD.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.CurrentDetailData; if (null == dr) return;
                var frm = new Sci.Production.Quality.P01_PhysicalInspection(false, CurrentDetailData["ID"].ToString(), null, null, dr);
                frm.ShowDialog(this);
                frm.Dispose();
                this.RenewData();
            };

            phyYds.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.CurrentDetailData; if (null == dr) return;
                var frm = new Sci.Production.Quality.P01_PhysicalInspection(false, CurrentDetailData["ID"].ToString(), null, null, dr);
                frm.ShowDialog(this);
                frm.Dispose();
                this.RenewData();
            };
            Wei.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.CurrentDetailData; if (null == dr) return;
                var frm = new Sci.Production.Quality.P01_Weight(false, CurrentDetailData["ID"].ToString(), null, null, dr);
                frm.ShowDialog(this);
                frm.Dispose();
                this.RenewData();
            };
            WeiD.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.CurrentDetailData; if (null == dr) return;
                var frm = new Sci.Production.Quality.P01_Weight(false, CurrentDetailData["ID"].ToString(), null, null, dr);
                frm.ShowDialog(this);
                frm.Dispose();
                this.RenewData();
            };
            sha.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.CurrentDetailData; if (null == dr) return;
                var frm = new Sci.Production.Quality.P01_ShadeBond(false, CurrentDetailData["ID"].ToString(), null, null, dr);
                frm.ShowDialog(this);
                frm.Dispose();
                this.RenewData();
            };
            shaD.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.CurrentDetailData; if (null == dr) return;
                var frm = new Sci.Production.Quality.P01_ShadeBond(false, CurrentDetailData["ID"].ToString(), null, null, dr);
                frm.ShowDialog(this);
                frm.Dispose();
                this.RenewData();
            };
            Con.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.CurrentDetailData; if (null == dr) return;
                var frm = new Sci.Production.Quality.P01_Continuity(false, CurrentDetailData["ID"].ToString(), null, null, dr);
                frm.ShowDialog(this);
                frm.Dispose();
                this.RenewData();
            };
            ConD.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.CurrentDetailData; if (null == dr) return;
                var frm = new Sci.Production.Quality.P01_Continuity(false, CurrentDetailData["ID"].ToString(), null, null, dr);
                frm.ShowDialog(this);
                frm.Dispose();
                this.RenewData();
            };
            #endregion
            #region Validat & Editable
            nonPhy.CellEditable += (s, e) =>
            {
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                if (dr["Status"].ToString() == "Approved") e.IsEditable = false;
            };
            nonPhy.CellValidating += (s, e) =>
            {
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                dr["NonPhysical"] = e.FormattedValue;
                dr.EndEdit();
                DataTable dt = (DataTable)detailgridbs.DataSource;
                FinalResult(dr);
            };
            nonWei.CellEditable += (s, e) =>
            {
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                if (dr["Status"].ToString() == "Approved") e.IsEditable = false;
            };
            nonWei.CellValidating += (s, e) =>
            {
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                dr["NonWeight"] = e.FormattedValue;
                dr.EndEdit();
                FinalResult(dr);
            };
            nonSha.CellEditable += (s, e) =>
            {
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                if (dr["Status"].ToString() == "Approved") e.IsEditable = false;
            };
            nonSha.CellValidating += (s, e) =>
            {           
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                dr["NonShadeBond"] = e.FormattedValue;
                dr.EndEdit();
                FinalResult(dr);
            };
            nonCon.CellEditable += (s, e) =>
            {
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                if (dr["Status"].ToString() == "Approved") e.IsEditable = false;
            };
            nonCon.CellValidating += (s, e) =>
            {
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                dr["NonContinuity"] = e.FormattedValue;
                dr.EndEdit();
                FinalResult(dr);
            };
            #endregion
            #region set grid
            this.detailgrid.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("SEQ1", header: "SEQ1", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Text("SEQ2", header: "SEQ2", width: Widths.AnsiChars(2), iseditingreadonly: true)
                .Text("WKNO", header: "Wkno", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Date("whseArrival", header: "Arrive W/H Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Refno", header: "Refno", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("SCIRefno", header: "SCI Refno", width: Widths.AnsiChars(26), iseditingreadonly: true)
                .Text("Colorid", header: "Color", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("SuppEn", header: "Supplier", width: Widths.AnsiChars(21), iseditingreadonly: true)
                .Numeric("ArriveQty", header: "Arrive Qty", width: Widths.AnsiChars(10), integer_places: 10,decimal_places:2,iseditingreadonly:true)
                .Text("weavetypeid", header: "Weave Type", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Date("InspDeadline", header: "Insp. Deadline", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Result", header: "Over all\n Result", width: Widths.AnsiChars(4), iseditingreadonly: true)
                .CheckBox("NonPhysical", header: "Physical N/A", width: Widths.AnsiChars(2), iseditable: true, trueValue: 1, falseValue: 0, settings: nonPhy)
                .Text("Physical", header: "Physical\n Inspection", width: Widths.AnsiChars(4), iseditingreadonly: true, settings: phy)
                .Numeric("TotalInspYds", header: "Act. Ttl Ysd\nInspection", width: Widths.AnsiChars(8), integer_places: 10, decimal_places: 2, iseditingreadonly: true, settings: phyYds)
                .Date("PhysicalDate", header: "Last Phy.\nInsp. Date", width: Widths.AnsiChars(10), iseditingreadonly: true,settings:phyD)
                .CheckBox("NonWeight", header: "Weight N/A", width: Widths.AnsiChars(2), iseditable: true, trueValue: 1, falseValue: 0,settings: nonWei)
                .Text("Weight", header: "Weight\n Test", width: Widths.AnsiChars(4), iseditingreadonly: true, settings: Wei)
                .Date("WeightDate", header: "Last Wei.\nTest. Date", width: Widths.AnsiChars(10), iseditingreadonly: true, settings: WeiD)
                .CheckBox("NonShadeBond", header: "Shade\nBondN/A", width: Widths.AnsiChars(2), iseditable: true, trueValue: 1, falseValue: 0,settings:nonSha)
                .Text("Shadebond", header: "Shade\nBond", width: Widths.AnsiChars(4), iseditingreadonly: true,settings: sha)
                .Date("ShadeBondDate", header: "Last Shade.\nTest. Date", width: Widths.AnsiChars(10), iseditingreadonly: true,settings:shaD)
                .CheckBox("NonContinuity", header: "Continuity \nN/A", width: Widths.AnsiChars(2), iseditable: true, trueValue: 1, falseValue: 0,settings:nonCon)
                .Text("Continuity", header: "Continuity", width: Widths.AnsiChars(5), iseditingreadonly: true,settings:Con)
                .Date("ContinuityDate", header: "Last Cont.\nTest. Date", width: Widths.AnsiChars(10), iseditingreadonly: true,settings:ConD)
                .Text("Approve1", header: "Approve", width: Widths.AnsiChars(10), iseditingreadonly: true) 
                .Text("ReplacementReportID", header: "1st Replacement", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("Receivingid", header: "Receiving ID", width: Widths.AnsiChars(13), iseditingreadonly: true);
            detailgrid.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9);
            detailgrid.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9);
            detailgrid.Columns[11].DefaultCellStyle.BackColor = Color.MistyRose;
            detailgrid.Columns[12].DefaultCellStyle.BackColor = Color.LemonChiffon;
            detailgrid.Columns[13].DefaultCellStyle.BackColor = Color.LemonChiffon;
            detailgrid.Columns[14].DefaultCellStyle.BackColor = Color.LemonChiffon;
            detailgrid.Columns[15].DefaultCellStyle.BackColor = Color.MistyRose;
            detailgrid.Columns[16].DefaultCellStyle.BackColor = Color.LightCyan;
            detailgrid.Columns[17].DefaultCellStyle.BackColor = Color.LightCyan;
            detailgrid.Columns[18].DefaultCellStyle.BackColor = Color.MistyRose;
            detailgrid.Columns[19].DefaultCellStyle.BackColor = Color.LightGreen;
            detailgrid.Columns[20].DefaultCellStyle.BackColor = Color.LightGreen;
            detailgrid.Columns[21].DefaultCellStyle.BackColor = Color.MistyRose;
            detailgrid.Columns[22].DefaultCellStyle.BackColor = Color.AntiqueWhite;
            detailgrid.Columns[23].DefaultCellStyle.BackColor = Color.AntiqueWhite;
            #endregion
            
        }

        protected override void OnDetailEntered() 
        {
 	        base.OnDetailEntered();
            
            DataRow queryDr;
            DualResult dResult = PublicPrg.Prgs.QueryQaInspectionHeader(CurrentMaintain["ID"].ToString(), out queryDr);
            if (!dResult)
            {
                ShowErr(dResult);
                return;
            }
           
            DataTable sciTb;
            string query_cmd = string.Format(" select * from [dbo].[Getsci]('{0}','{1}')", CurrentMaintain["ID"],MyUtility.Check.Empty(queryDr)?"": queryDr["Category"]);
            DBProxy.Current.Select(null,query_cmd,out sciTb);
            if (!dResult)
            {
                ShowErr(query_cmd, dResult);
                return;
            }
            if (sciTb.Rows.Count > 0)
            {
                if (sciTb.Rows[0]["MinSciDelivery"] == DBNull.Value) scidelivery_box.Text = "";
                else scidelivery_box.Text = Convert.ToDateTime(sciTb.Rows[0]["MinSciDelivery"]).ToShortDateString();
            }
            else
            {
                scidelivery_box.Text = "";
                estcutdate_box.Text = "";
            }
            DateTime? targT;
            if (MyUtility.Check.Empty(queryDr))
            {
                targT = null;
                estcutdate_box.Text = "";
            }
            else
            {
                 targT = Sci.Production.PublicPrg.Prgs.GetTargetLeadTime(MyUtility.Check.Empty(queryDr) ? "" : queryDr["CUTINLINE"], sciTb.Rows[0]["MinSciDelivery"]);
                if (queryDr["cutinline"] == DBNull.Value) estcutdate_box.Text = "";
                 else estcutdate_box.Text = Convert.ToDateTime(queryDr["cutinline"]).ToShortDateString();
            }
           
            if (targT != null) leadtime_box.Text = ((DateTime)targT).ToShortDateString();
            else leadtime_box.Text = "";
            style_box.Text = MyUtility.Check.Empty(queryDr) ? "" : queryDr["Styleid"].ToString();
            season_box.Text = MyUtility.Check.Empty(queryDr) ? "" : queryDr["Seasonid"].ToString();
            brand_box.Text = MyUtility.Check.Empty(queryDr) ? "" : queryDr["brandid"].ToString();
            // if (queryDr["cutinline"] == DBNull.Value) estcutdate_box.Text = "";
           // else estcutdate_box.Text = Convert.ToDateTime(queryDr["cutinline"]).ToShortDateString();
               
            mtl_box.Text = CurrentMaintain["Complete"].ToString() == "1" ? "Y" : "";
            decimal detailRowCount = DetailDatas.Count;
            string inspnum = "0";
            DataTable detailTb = (DataTable)detailgridbs.DataSource;
            if(detailRowCount!=0) 
            {
                
                if (detailTb.Rows.Count != 0)
                {
                    DataRow[] inspectAry = detailTb.Select("Result<>'' or (nonphysical and nonweight and nonshadebond and noncontinuity)");
                    if (inspectAry.Length > 0)
                    {
                        inspnum = Math.Round(((decimal)inspectAry.Length / detailRowCount) * 100, 2).ToString();
                    }
                }
            }
            insp_box.Text = inspnum;
            DateTime? completedate ,Physicalcompletedate , Weightcompletedate , ShadeBondcompletedate , Continuitycompletedate ;
            if (inspnum == "100")
            {
                
                Physicalcompletedate = MyUtility.Convert.GetDate(detailTb.Compute("Max(PhysicalDate)", ""));
                Weightcompletedate = MyUtility.Convert.GetDate(detailTb.Compute("Max(WeightDate)", "")); //((DateTime)detailTb.Compute("Max(WeightDate)", ""));
                ShadeBondcompletedate = MyUtility.Convert.GetDate(detailTb.Compute("Max(ShadeBondDate)", "")); //((DateTime)detailTb.Compute("Max(ShadeBondDate)", ""));
                Continuitycompletedate = MyUtility.Convert.GetDate(detailTb.Compute("Max(ContinuityDate)", "")); //((DateTime)detailTb.Compute("Max(ContinuityDate)", ""));
                if (MyUtility.Math.DateMinus(Physicalcompletedate, Weightcompletedate).TotalSeconds < 0) completedate = Weightcompletedate;
                else completedate = Physicalcompletedate;

                if (MyUtility.Math.DateMinus(completedate, ShadeBondcompletedate).TotalSeconds < 0) completedate = ShadeBondcompletedate;
                if (MyUtility.Math.DateMinus(completedate, Continuitycompletedate).TotalSeconds < 0) completedate = Continuitycompletedate;

                /*if (DateTime.Compare(Physicalcompletedate, Weightcompletedate) < 0) completedate = Weightcompletedate;
                else completedate = Physicalcompletedate;
                if (DateTime.Compare(completedate, ShadeBondcompletedate) < 0) completedate = ShadeBondcompletedate;
                if (DateTime.Compare(completedate, Continuitycompletedate) < 0) completedate = Continuitycompletedate;
                */
                Complete_box.Text = completedate ==null ? "": ((DateTime)completedate).ToShortDateString(); ;
            }
            else Complete_box.Text = "";

            #region Box 顏色
            ava_box.BackColor = Color.MistyRose;
            ph_box.BackColor = Color.LemonChiffon;
            we_box.BackColor = Color.LightCyan;
            sh_box.BackColor = Color.LightGreen;
            co_box.BackColor = Color.AntiqueWhite;
            #endregion
            this.detailgrid.AutoResizeColumns();
        }

        protected override DualResult ClickSave()
        {
            //因為表頭是PO不能覆蓋其他資料，必需自行存檔
            string save_po_cmd = string.Format("update po set FIRRemark = '{0}' where id = '{1}';", remark_box.Text.ToString(), CurrentMaintain["ID"]);
            
            foreach (DataRow dr in DetailDatas)
            {
                if (dr.RowState == DataRowState.Modified)
                {
                    int nonph = dr["NonPhysical"].ToString() == "True" ? 1 : 0;
                    int nonwei = dr["NonWeight"].ToString() == "True" ? 1 : 0;
                    int nonsha = dr["NonShadeBond"].ToString() == "True" ? 1 : 0;
                    int noncon = dr["NonContinuity"].ToString() == "True" ? 1 : 0;
                    save_po_cmd = save_po_cmd + string.Format(
                    @"Update FIR Set Result = '{0}',NonPhysical = {1},NonWeight = {2},
                    NonShadeBond = {3},NonContinuity = {4},Status = '{6}'
                    Where ID = '{5}';"
                    , dr["Result"], nonph, nonwei, nonsha, noncon, dr["ID"], dr["Status"]);
                }
            }
            DualResult upResult;
            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {

                    if (!(upResult = DBProxy.Current.Execute(null, save_po_cmd)))
                    {
                        _transactionscope.Dispose();
                        return upResult;
                    }
                    
                    _transactionscope.Complete();
                    //MyUtility.Msg.WarningBox("Successfully");
                }
                catch (Exception ex)
                {
                    _transactionscope.Dispose();
                    ShowErr("Commit transaction error.", ex);
                    return Result.True;
                }
            }

            _transactionscope.Dispose();
            _transactionscope = null;
            this.RenewData();
            this.OnDetailEntered();
            EnsureToolbarExt();
            return Result.True;

        }

        public void FinalResult(DataRow dr)
        {
            if (this.EditMode) //Status = Confirm 才會判斷
            {

                string[] returnstr = Sci.Production.PublicPrg.Prgs.GetOverallResult_Status(dr["ID"]);

                dr["Result"] = returnstr[0];
                dr["Status"] = returnstr[1];
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataTable detDtb = (DataTable)detailgridbs.DataSource;
            //移到指定那筆
            string wk = wk_box.Text;
            string seq1 = seq1_box.Text;
            string seq2 = seq2_box.Text;
            string find_new ="";
           
            if (!MyUtility.Check.Empty(wk) )
            {
                find_new = string.Format("Exportid='{0}'",wk);
            }
            if (!MyUtility.Check.Empty(seq1))
            {
               if(!MyUtility.Check.Empty(find_new))
               {
                   find_new = find_new + string.Format(" and SEQ1 = '{0}'",seq1);
               }
               else
               {
                   find_new = string.Format("SEQ1 = '{0}'",seq1);
               }
            }
            if (!MyUtility.Check.Empty(seq2))
            {
               if(!MyUtility.Check.Empty(find_new))
               {
                   find_new = find_new + string.Format(" and SEQ2 = '{0}'",seq2);
               }
               else
               {
                   find_new = string.Format("SEQ2 = '{0}'",seq2);
               }
            }
            if (find != find_new)
            {
                find = find_new;
                find_dr = detDtb.Select(find_new);
                if (find_dr.Length == 0)
                {
                    MyUtility.Msg.WarningBox("Not Found");
                    return;
                }
                else { index = 0; }
            }
            else  
            {
                index++;
                if (find_dr==null)
                {
                    return;
                }
                if (index >= find_dr.Length) index = 0;
            }
            detailgridbs.Position = DetailDatas.IndexOf(find_dr[index]);
        }

        private void modifyPhysicalInspectionToolStripMenuItem_Click(object sender, EventArgs e)
        {           
            var dr =this.CurrentDetailData; if (null == dr) return;
            var frm = new Sci.Production.Quality.P01_PhysicalInspection(IsSupportEdit, CurrentDetailData["ID"].ToString(), null, null, dr);
            frm.ShowDialog(this);
            frm.Dispose();
            //this.RenewData();會讓資料renew導致記憶被洗掉
        }

        
        private void modifyWeightTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dr = this.CurrentDetailData; if (null == dr) return;
            var currentID = this.CurrentDetailData["ID"].ToString();

            var frm = new Sci.Production.Quality.P01_Weight(IsSupportEdit, CurrentDetailData["ID"].ToString(), null, null, dr);
            frm.ShowDialog(this);
            frm.Dispose();
            this.RenewData();
            // 固定滑鼠指向位置,避免被renew影響
            int rowindex = 0;
            for (int rIdx = 0; rIdx < detailgrid.Rows.Count; rIdx++)
            {
                DataGridViewRow dvr = detailgrid.Rows[rIdx];
                DataRow row = ((DataRowView)dvr.DataBoundItem).Row;

                if (row["ID"].ToString() == currentID)
                {
                    rowindex = rIdx;
                    break;
                }
            }
            detailgrid.SelectRowTo(rowindex);
        }

        private void modifyShadeBondToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dr = this.CurrentDetailData; if (null == dr) return;
            var currentID = this.CurrentDetailData["ID"].ToString();
            var frm = new Sci.Production.Quality.P01_ShadeBond(IsSupportEdit, CurrentDetailData["ID"].ToString(), null, null, dr);
            frm.ShowDialog(this);
            frm.Dispose();
            this.RenewData();
            // 固定滑鼠指向位置,避免被renew影響
            int rowindex = 0;
            for (int rIdx = 0; rIdx < detailgrid.Rows.Count; rIdx++)
            {
                DataGridViewRow dvr = detailgrid.Rows[rIdx];
                DataRow row = ((DataRowView)dvr.DataBoundItem).Row;

                if (row["ID"].ToString() == currentID)
                {
                    rowindex = rIdx;
                    break;
                }
            }
            detailgrid.SelectRowTo(rowindex);
        }

        private void modifyContinuityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dr = this.CurrentDetailData; if (null == dr) return;
            var currentID = this.CurrentDetailData["ID"].ToString();
            var frm = new Sci.Production.Quality.P01_Continuity(IsSupportEdit, CurrentDetailData["ID"].ToString(), null, null, dr);
            frm.ShowDialog(this);
            frm.Dispose();
            this.RenewData();
            // 固定滑鼠指向位置,避免被renew影響
            int rowindex = 0;
            for (int rIdx = 0; rIdx < detailgrid.Rows.Count; rIdx++)
            {
                DataGridViewRow dvr = detailgrid.Rows[rIdx];
                DataRow row = ((DataRowView)dvr.DataBoundItem).Row;

                if (row["ID"].ToString() == currentID)
                {
                    rowindex = rIdx;
                    break;
                }
            }
            detailgrid.SelectRowTo(rowindex);

        }
      
    }
}

