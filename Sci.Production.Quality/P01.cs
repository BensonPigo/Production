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
        private bool boolFromP01;
       
        int index;
        string find = "";
        DataRow[] find_dr;

        public P01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();            
            detailgridmenus.Items.Remove(appendmenu);
            detailgridmenus.Items.Remove(modifymenu);
            detailgridmenus.Items.Remove(deletemenu);
            detailgridmenus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.modifyPhysicalInspectionToolStripMenuItem,
                this.modifyWeightTestToolStripMenuItem,
                this.modifyShadeBondTestToolStripMenuItem,
                this.modifyContinuityTestToolStripMenuItem,
                this.modifyOdorTestToolStripMenuItem  });
            //關閉表身Grid DoubleClick 會新增row的問題
            InsertDetailGridOnDoubleClick = false;
            boolFromP01 = false;
        }

        public P01(string Poid) //for Form直接call form
        {
            InitializeComponent();
            DefaultFilter = string.Format("ID = '{0}'", Poid);
            InsertDetailGridOnDoubleClick = false;
            IsSupportEdit = false;
            boolFromP01 = true;
        }

        protected override void OnFormLoaded()
        {
            MyUtility.Tool.SetupCombox(this.queryfors, 1, 1, ",last two years data");
            if (boolFromP01)
            {
                this.ExpressQuery = false;
            }
            else
            {
                queryfors.SelectedIndex = 1;
                this.DefaultWhere = " AddDate >= DATEADD(YY,-2,GETDATE()) OR EditDate >= DATEADD(YY,-2,GETDATE())";
                this.ExpressQuery = true;
            }

            base.OnFormLoaded();

            queryfors.SelectedIndexChanged += (s, e) =>
            {
                switch (queryfors.SelectedIndex)
                {
                    case 0:
                        this.DefaultWhere = "";
                        break;
                    case 1:
                        this.DefaultWhere = " AddDate >= DATEADD(YY,-2,GETDATE()) OR EditDate >= DATEADD(YY,-2,GETDATE())";
                        break;
                }
                this.ReloadDatas();
            };
        }

        protected override Ict.DualResult OnDetailSelectCommandPrepare(Win.Tems.InputMasterDetail.PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["id"].ToString();
            string cmd = string.Format(
@"Select a.id,a.poid,a.SEQ1,a.SEQ2,Receivingid,a.Refno,a.SCIRefno,Suppid,
ArriveQty,InspDeadline,Result,
PhysicalEncode,WeightEncode,ShadeBondEncode,ContinuityEncode,
NonPhysical,Physical,TotalInspYds,PhysicalDate,Physical,
NonWeight, Weight,WeightDate,Weight,
NonShadebond,Shadebond,ShadebondDate,shadebond,
NonContinuity,Continuity,ContinuityDate,Continuity,
a.Status,ReplacementReportID,(a.seq1+a.seq2) as seq,
(Select weavetypeid from Fabric b WITH (NOLOCK) where b.SCIRefno =a.SCIrefno) as weavetypeid,
c.Exportid,c.whseArrival,dbo.getPass1(a.Approve) as approve1,approveDate,approve,
d.ColorID,
(Select ID+' - '+ AbbEn From Supp WITH (NOLOCK) Where a.suppid = supp.id) as SuppEn,
c.ExportID as Wkno
,cn.name
,a.nonOdor
,a.Odor
,a.OdorEncode
,a.OdorDate
,[PhysicalInspector] = (select name from pass1 where id = a.PhysicalInspector)
,[WeightInspector] = (select name from pass1 where id = a.WeightInspector)
,[ShadeboneInspector] = (select name from pass1 where id = a.ShadeboneInspector)
,[ContinuityInspector] = (select name from pass1 where id = a.ContinuityInspector)
,[OdorInspector] = (select name from pass1 where id = a.OdorInspector)
From FIR a WITH (NOLOCK) Left join Receiving c WITH (NOLOCK) on c.id = a.receivingid
inner join PO_Supp_Detail d WITH (NOLOCK) on d.id = a.poid and d.seq1 = a.seq1 and d.seq2 = a.seq2
outer apply(select name from color WITH (NOLOCK) where color.id = d.colorid and color.BrandId = d.BrandId)cn
Where a.poid='{0}' order by a.seq1,a.seq2", masterID);
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
            DataGridViewGeneratorCheckBoxColumnSettings nonOdor = new DataGridViewGeneratorCheckBoxColumnSettings();
            DataGridViewGeneratorTextColumnSettings phy = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorDateColumnSettings phyD = new DataGridViewGeneratorDateColumnSettings();
            DataGridViewGeneratorNumericColumnSettings phyYds = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorTextColumnSettings Wei = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorDateColumnSettings WeiD = new DataGridViewGeneratorDateColumnSettings();
            DataGridViewGeneratorTextColumnSettings sha = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorDateColumnSettings shaD = new DataGridViewGeneratorDateColumnSettings();
            DataGridViewGeneratorTextColumnSettings Con = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorDateColumnSettings ConD = new DataGridViewGeneratorDateColumnSettings();
            DataGridViewGeneratorTextColumnSettings Odor = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorDateColumnSettings OdorD = new DataGridViewGeneratorDateColumnSettings();
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

            phyYds.CellFormatting += (s, e) =>
            {
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                string sqlcmd = $@"
select 1 from FIR_Physical with(nolock)
where  id = {dr["id"]}
and TicketYds <> ActualYds
and ActualYds > 0
";
                if (MyUtility.Check.Seek(sqlcmd))
                {
                    e.CellStyle.BackColor = Color.Pink;
                }
                else
                {
                    e.CellStyle.BackColor = Color.LemonChiffon;
                }
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
            Odor.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.CurrentDetailData; if (null == dr) return;
                var frm = new Sci.Production.Quality.P01_Odor(false, CurrentDetailData["ID"].ToString(), null, null, dr);
                frm.ShowDialog(this);
                frm.Dispose();
                this.RenewData();
            };
            OdorD.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.CurrentDetailData; if (null == dr) return;
                var frm = new Sci.Production.Quality.P01_Odor(false, CurrentDetailData["ID"].ToString(), null, null, dr);
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
            nonOdor.CellEditable += (s, e) =>
            {
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                if (dr["Status"].ToString() == "Approved") e.IsEditable = false;
            };
            nonOdor.CellValidating += (s, e) =>
            {
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                dr["nonOdor"] = e.FormattedValue;
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
                .Text("name", header: "Color Name", width: Widths.AnsiChars(6), iseditingreadonly: true)
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
                .CheckBox("NonShadeBond", header: "Shade\nBandN/A", width: Widths.AnsiChars(2), iseditable: true, trueValue: 1, falseValue: 0,settings:nonSha)
                .Text("Shadebond", header: "Shade\nBand", width: Widths.AnsiChars(4), iseditingreadonly: true,settings: sha)
                .Date("ShadeBondDate", header: "Last Shade.\nTest. Date", width: Widths.AnsiChars(10), iseditingreadonly: true,settings:shaD)
                .CheckBox("NonContinuity", header: "Continuity \nN/A", width: Widths.AnsiChars(2), iseditable: true, trueValue: 1, falseValue: 0,settings:nonCon)
                .Text("Continuity", header: "Continuity", width: Widths.AnsiChars(5), iseditingreadonly: true,settings:Con)
                .Date("ContinuityDate", header: "Last Cont.\nTest. Date", width: Widths.AnsiChars(10), iseditingreadonly: true,settings:ConD)

                .CheckBox("nonOdor", header: "Odor \nN/A", width: Widths.AnsiChars(2), iseditable: true, trueValue: 1, falseValue: 0, settings: nonOdor)
                .Text("Odor", header: "Odor", width: Widths.AnsiChars(5), iseditingreadonly: true, settings: Odor)
                .Date("OdorDate", header: "Last Odor\nTest. Date", width: Widths.AnsiChars(10), iseditingreadonly: true, settings: OdorD)


                .Text("Approve1", header: "Approve", width: Widths.AnsiChars(10), iseditingreadonly: true) 
                .Text("ReplacementReportID", header: "1st Replacement", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("Receivingid", header: "Receiving ID", width: Widths.AnsiChars(13), iseditingreadonly: true);
            detailgrid.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9);
            detailgrid.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9);
            //紅粉佳人組
            detailgrid.Columns["NonPhysical"].DefaultCellStyle.BackColor = Color.MistyRose;
            detailgrid.Columns["NonWeight"].DefaultCellStyle.BackColor = Color.MistyRose;
            detailgrid.Columns["NonShadeBond"].DefaultCellStyle.BackColor = Color.MistyRose;
            detailgrid.Columns["NonContinuity"].DefaultCellStyle.BackColor = Color.MistyRose;
            detailgrid.Columns["nonOdor"].DefaultCellStyle.BackColor = Color.MistyRose;
            //檸檬薄紗組
            detailgrid.Columns["Physical"].DefaultCellStyle.BackColor = Color.LemonChiffon;
            detailgrid.Columns["TotalInspYds"].DefaultCellStyle.BackColor = Color.LemonChiffon;
            detailgrid.Columns["PhysicalDate"].DefaultCellStyle.BackColor = Color.LemonChiffon;
            //青藍組
            detailgrid.Columns["Weight"].DefaultCellStyle.BackColor = Color.LightCyan;
            detailgrid.Columns["WeightDate"].DefaultCellStyle.BackColor = Color.LightCyan;            
            //青綠組            
            detailgrid.Columns["Shadebond"].DefaultCellStyle.BackColor = Color.LightGreen;
            detailgrid.Columns["ShadeBondDate"].DefaultCellStyle.BackColor = Color.LightGreen;
            //復古色組
            detailgrid.Columns["Continuity"].DefaultCellStyle.BackColor = Color.AntiqueWhite;
            detailgrid.Columns["ContinuityDate"].DefaultCellStyle.BackColor = Color.AntiqueWhite;
            //Lavender
            detailgrid.Columns["Odor"].DefaultCellStyle.BackColor = Color.Lavender;
            detailgrid.Columns["OdorDate"].DefaultCellStyle.BackColor = Color.Lavender;
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
                //if (sciTb.Rows[0]["MinSciDelivery"] == DBNull.Value) dateEarliestSCIDel.Text = "";
                //else dateEarliestSCIDel.Text = Convert.ToDateTime(sciTb.Rows[0]["MinSciDelivery"]).ToShortDateString();
            }
            else
            {
                //dateEarliestSCIDel.Text = "";
                dateEarliestEstCutDate.Text = "";
            }
            DateTime? targT;
            if (MyUtility.Check.Empty(queryDr))
            {
                targT = null;
                dateEarliestEstCutDate.Text = "";
            }
            else
            {
                 targT = Sci.Production.PublicPrg.Prgs.GetTargetLeadTime(MyUtility.Check.Empty(queryDr) ? "" : queryDr["CUTINLINE"], sciTb.Rows[0]["MinSciDelivery"]);
                if (queryDr["cutinline"] == DBNull.Value) dateEarliestEstCutDate.Text = "";
                 else dateEarliestEstCutDate.Text = Convert.ToDateTime(queryDr["cutinline"]).ToShortDateString();
            }
           
            if (targT != null) dateTargetLeadTime.Text = ((DateTime)targT).ToShortDateString();
            else dateTargetLeadTime.Text = "";
            displayStyle.Text = MyUtility.Check.Empty(queryDr) ? "" : queryDr["Styleid"].ToString();
            displaySeason.Text = MyUtility.Check.Empty(queryDr) ? "" : queryDr["Seasonid"].ToString();
            displayBrand.Text = MyUtility.Check.Empty(queryDr) ? "" : queryDr["brandid"].ToString();
               
            displayMTLCmlpt.Text = (bool)CurrentMaintain["Complete"] ? "Y" : "";
            decimal detailRowCount = DetailDatas.Count;
            string inspnum = "0";
            DataTable detailTb = (DataTable)detailgridbs.DataSource;
            if(detailRowCount!=0) 
            {
                
                if (detailTb.Rows.Count != 0)
                {
                    DataRow[] inspectAry = detailTb.Select("Result<>'' or (nonphysical and nonweight and nonshadebond and noncontinuity and nonOdor)");
                    if (inspectAry.Length > 0)
                    {
                        inspnum = Math.Round(((decimal)inspectAry.Length / detailRowCount) * 100, 2).ToString();
                    }
                }
            }
            //displayofInspection.Text = inspnum;
            DateTime? completedate ,Physicalcompletedate , Weightcompletedate , ShadeBondcompletedate , Continuitycompletedate ;
            if (inspnum == "100")
            {
                
                Physicalcompletedate = MyUtility.Convert.GetDate(detailTb.Compute("Max(PhysicalDate)", ""));
                Weightcompletedate = MyUtility.Convert.GetDate(detailTb.Compute("Max(WeightDate)", "")); 
                ShadeBondcompletedate = MyUtility.Convert.GetDate(detailTb.Compute("Max(ShadeBondDate)", "")); 
                Continuitycompletedate = MyUtility.Convert.GetDate(detailTb.Compute("Max(ContinuityDate)", "")); 
                if (MyUtility.Math.DateMinus(Physicalcompletedate, Weightcompletedate).TotalSeconds < 0) completedate = Weightcompletedate;
                else completedate = Physicalcompletedate;

                if (MyUtility.Math.DateMinus(completedate, ShadeBondcompletedate).TotalSeconds < 0) completedate = ShadeBondcompletedate;
                if (MyUtility.Math.DateMinus(completedate, Continuitycompletedate).TotalSeconds < 0) completedate = Continuitycompletedate;
                             
                dateCompletionDate.Text = completedate ==null ? "": ((DateTime)completedate).ToShortDateString(); ;
            }
            else dateCompletionDate.Text = "";

            #region Box 顏色
            dispLengthofdifference.BackColor = Color.Pink;
            displayavailablemodified.BackColor = Color.MistyRose;
            displayPhysicalInsp.BackColor = Color.LemonChiffon;
            displayWeightTest.BackColor = Color.LightCyan;
            displayShadeBond.BackColor = Color.LightGreen;
            displayContinuity.BackColor = Color.AntiqueWhite;
            displayOdor.BackColor = Color.Lavender;
            #endregion
            this.detailgrid.AutoResizeColumns();
        }

        protected override DualResult ClickSave()
        {
            //因為表頭是PO不能覆蓋其他資料，必需自行存檔
            string save_po_cmd = string.Format("update po set FIRRemark = '{0}' where id = '{1}';", editRemark.Text.ToString(), CurrentMaintain["ID"]);
            
            foreach (DataRow dr in DetailDatas)
            {
                if (dr.RowState == DataRowState.Modified)
                {
                    int nonph = dr["NonPhysical"].ToString() == "True" ? 1 : 0;
                    int nonwei = dr["NonWeight"].ToString() == "True" ? 1 : 0;
                    int nonsha = dr["NonShadeBond"].ToString() == "True" ? 1 : 0;
                    int noncon = dr["NonContinuity"].ToString() == "True" ? 1 : 0;
                    int nonOdor = dr["NonOdor"].ToString() == "True" ? 1 : 0;
                    save_po_cmd = save_po_cmd + string.Format(
                    @"Update FIR Set Result = '{0}',NonPhysical = {1},NonWeight = {2},
                    NonShadeBond = {3},NonContinuity = {4},Status = '{6}',NonOdor={7}
                    Where ID = '{5}';"
                    , dr["Result"], nonph, nonwei, nonsha, noncon, dr["ID"], dr["Status"], nonOdor);
                }
                #region 重新判斷AllResult
                //重新判斷AllResult             
                //string[] returnstr = Sci.Production.PublicPrg.Prgs.GetOverallResult_Status(dr);
                //dr["Result"] = returnstr[0];
                //dr["Status"] = returnstr[1];
                #endregion
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

                    //更新PO.FIRInspPercent
                    if (!(upResult = DBProxy.Current.Execute(null, $"exec UpdateInspPercent 'FIR','{CurrentMaintain["ID"]}'")))
                    {
                        _transactionscope.Dispose();
                        return upResult;
                    }

                    _transactionscope.Complete();
                    _transactionscope.Dispose();                    
                    MyUtility.Msg.InfoBox("Successfully");
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
            
            return Result.True;

        }

        //判斷並回寫Physical OverallResult, Status string[0]=Result, string[1]=status
        public void FinalResult(DataRow dr)
        {
            if (this.EditMode) //Status = Confirm 才會判斷
            {               
                string[] returnstr = Sci.Production.PublicPrg.Prgs.GetOverallResult_Status(dr);

                dr["Result"] = returnstr[0];
                dr["Status"] = returnstr[1];
            }
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            DataTable detDtb = (DataTable)detailgridbs.DataSource;
            //移到指定那筆
            string wk = txtLocateforWK.Text;
            string seq1 = txtSEQ1.Text;
            string seq2 = txtSEQ2.Text;
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
                    MyUtility.Msg.WarningBox("Data not Found.");
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

        override protected DetailGridContextMenuMode CurrentDetailGridContextMenuMode()  
        {
            //非編輯狀態不顯示
            if (!EditMode)
            {
                return DetailGridContextMenuMode.Editable;
            }
            return DetailGridContextMenuMode.None;
        }        

        private void modifyPhysicalInspectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dr = this.CurrentDetailData; if (null == dr) return;
            var currentID = this.CurrentDetailData["ID"].ToString();
            var frm = new Sci.Production.Quality.P01_PhysicalInspection(IsSupportEdit, CurrentDetailData["ID"].ToString(), null, null, dr);
            frm.ShowDialog(this);
            frm.Dispose();
            this.RenewData();
            this.OnDetailEntered();
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

        private void modifyWeightTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dr = this.CurrentDetailData; if (null == dr) return;
            var currentID = this.CurrentDetailData["ID"].ToString();

            var frm = new Sci.Production.Quality.P01_Weight(IsSupportEdit, CurrentDetailData["ID"].ToString(), null, null, dr);
            frm.ShowDialog(this);
            frm.Dispose();
            this.RenewData();
            this.OnDetailEntered();
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

        private void modifyShadeBondTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dr = this.CurrentDetailData; if (null == dr) return;
            var currentID = this.CurrentDetailData["ID"].ToString();
            var frm = new Sci.Production.Quality.P01_ShadeBond(IsSupportEdit, CurrentDetailData["ID"].ToString(), null, null, dr);
            frm.ShowDialog(this);
            frm.Dispose();
            this.RenewData();
            this.OnDetailEntered();
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

        private void modifyContinuityTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dr = this.CurrentDetailData; if (null == dr) return;
            var currentID = this.CurrentDetailData["ID"].ToString();
            var frm = new Sci.Production.Quality.P01_Continuity(IsSupportEdit, CurrentDetailData["ID"].ToString(), null, null, dr);
            frm.ShowDialog(this);
            frm.Dispose();
            this.RenewData();
            //重新計算表頭資料
            this.OnDetailEntered();
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

        private void modifyOdorTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dr = this.CurrentDetailData; if (null == dr) return;
            var currentID = this.CurrentDetailData["ID"].ToString();
            var frm = new Sci.Production.Quality.P01_Odor(IsSupportEdit, CurrentDetailData["ID"].ToString(), null, null, dr);
            frm.ShowDialog(this);
            frm.Dispose();
            this.RenewData();
            //重新計算表頭資料
            this.OnDetailEntered();
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

