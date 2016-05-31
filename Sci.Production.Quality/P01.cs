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
        }
        public P01(string Poid) //for Form直接call form
        {
            InitializeComponent();
            DefaultFilter = string.Format("POID = '{0}'", Poid);
            InsertDetailGridOnDoubleClick = false;
            IsSupportEdit = false;
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
                (Select weavetypeid from Fabric b where b.SCIRefno =a.SCIrefno) as weavetypeid,
                c.Exportid,c.whseArrival,dbo.getPass1(a.Approve) as approve,
                (Select d.colorid from PO_Supp_Detail d Where d.id = a.poid and d.seq1 = a.seq1 and d.seq2 = a.seq2) as Colorid,
                (Select AbbEn From Supp Where a.suppid = supp.id) as SuppEn
                From FIR a Left join Receiving c on c.id = a.receivingid
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
                if(dr["Status"].ToString()=="Confirmed") FinalResult(dr);
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
                if (dr["Status"].ToString() == "Confirmed") FinalResult(dr);
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
                if (dr["Status"].ToString() == "Confirmed") FinalResult(dr);
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
                if (dr["Status"].ToString() == "Confirmed") FinalResult(dr);
            };
            #region set grid
            this.detailgrid.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("SEQ1", header: "SEQ1", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Text("SEQ2", header: "SEQ2", width: Widths.AnsiChars(2), iseditingreadonly: true)
                .Date("whseArrival", header: "Arrive W/H Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Refno", header: "Brand Ref#", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("SCIRefno", header: "SCI Ref#", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("Colorid", header: "Color", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("SuppEn", header: "Supplier", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("ArriveQty", header: "Arrive Qty", width: Widths.AnsiChars(8), integer_places: 10,decimal_places:2,iseditingreadonly:true)
                .Text("weavetypeid", header: "Weave Type", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Date("InspDeadline", header: "Insp. Deadline", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Result", header: "Over all\n Result", width: Widths.AnsiChars(4), iseditingreadonly: true)
                .CheckBox("NonPhysical", header: "Physical N/A", width: Widths.AnsiChars(2), iseditable: true, trueValue: 1, falseValue: 0, settings: nonPhy)
                .Text("Physical", header: "Physical\n Inspection", width: Widths.AnsiChars(4), iseditingreadonly: true)
                .Numeric("TotalInspYds", header: "Act. Ttl Ysd\nInspection", width: Widths.AnsiChars(8), integer_places: 10, decimal_places: 2, iseditingreadonly: true)
                .Date("PhysicalDate", header: "Last Phy.\nInsp. Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .CheckBox("NonWeight", header: "Weight N/A", width: Widths.AnsiChars(2), iseditable: true, trueValue: 1, falseValue: 0)
                .Text("Weight", header: "Weight\n Test", width: Widths.AnsiChars(4), iseditingreadonly: true)
                .Date("WeightDate", header: "Last Wei.\nTest. Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .CheckBox("NonShadeBond", header: "Shade\nBondN/A", width: Widths.AnsiChars(2), iseditable: true, trueValue: 1, falseValue: 0)
                .Text("Shadebond", header: "Shade\nBond", width: Widths.AnsiChars(4), iseditingreadonly: true)
                .Date("ShadeBondDate", header: "Last Shade.\nTest. Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .CheckBox("NonContinuity", header: "Continuity \nN/A", width: Widths.AnsiChars(2), iseditable: true, trueValue: 1, falseValue: 0)
                .Text("Continuity", header: "Continuity", width: Widths.AnsiChars(4), iseditingreadonly: true)
                .Date("ContinuityDate", header: "Last Cont.\nTest. Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Approve", header: "Approve", width: Widths.AnsiChars(10), iseditingreadonly: true) 
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
            DateTime minSciDel;
            string query_cmd = string.Format(" select * from [dbo].[Getsci]('{0}','{1}')", CurrentMaintain["ID"],queryDr["Category"]);
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
            }
            DateTime? targT = Sci.Production.PublicPrg.Prgs.GetTargetLeadTime(queryDr["CUTINLINE"], sciTb.Rows[0]["MinSciDelivery"]);
            if (targT != null) leadtime_box.Text = ((DateTime)targT).ToShortDateString();
            else leadtime_box.Text = "";
            style_box.Text = queryDr["Styleid"].ToString();
            season_box.Text = queryDr["Seasonid"].ToString();
            brand_box.Text = queryDr["brandid"].ToString();
            if (queryDr["cutinline"] == DBNull.Value) estcutdate_box.Text = "";
            else estcutdate_box.Text = Convert.ToDateTime(queryDr["cutinline"]).ToShortDateString();
               
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
            DateTime completedate ,Physicalcompletedate , Weightcompletedate , ShadeBondcompletedate , Continuitycompletedate ;
            if (inspnum == "100")
            {

                Physicalcompletedate = ((DateTime)detailTb.Compute("Max(PhysicalDate)", ""));
                Weightcompletedate = ((DateTime)detailTb.Compute("Max(WeightDate)", ""));
                ShadeBondcompletedate = ((DateTime)detailTb.Compute("Max(ShadeBondDate)", ""));
                Continuitycompletedate = ((DateTime)detailTb.Compute("Max(ContinuityDate)", ""));
                if (DateTime.Compare(Physicalcompletedate, Weightcompletedate) < 0) completedate = Weightcompletedate;
                else completedate = Physicalcompletedate;
                if (DateTime.Compare(completedate, ShadeBondcompletedate) < 0) completedate = ShadeBondcompletedate;
                if (DateTime.Compare(completedate, Continuitycompletedate) < 0) completedate = Continuitycompletedate;

                Complete_box.Text = completedate.ToShortDateString(); ;
            }
            else Complete_box.Text = "";
            
        }

        protected override DualResult ClickSave()
        {
            //因為表頭是PO不能覆蓋其他資料，必需自行存檔
            string save_po_cmd = string.Format("update po set FIRRemark = '{0}' where id = '{1}';", CurrentMaintain["FirRemark"],CurrentMaintain["ID"]);
            
            foreach (DataRow dr in DetailDatas)
            {
                if (dr.RowState == DataRowState.Modified)
                {
                    save_po_cmd = save_po_cmd + string.Format(
                    @"Update FIR Set Result = '{0}',NonPhysical = {1},NonWeight = {2},
                    NonShadeBond = {3},NonContinuity = {4}
                    Where ID = '{5}';"
                    , dr["Result"], dr["NonPhysical"], dr["NonWeight"], dr["NonShadeBond"], dr["NonContinuity"],dr["ID"]);
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
            if (this.EditMode)
            {
                string fin_result = "P";
                if (dr["NonPhysical"].ToString() == "0" && MyUtility.Check.Empty(dr["Physical1"])) fin_result = "";
                if (dr["NonWeight"].ToString() == "0" && MyUtility.Check.Empty(dr["Weight1"])) fin_result = "";
                if (dr["NonShadeBond"].ToString() == "0" && MyUtility.Check.Empty(dr["ShadeBond1"])) fin_result = "";
                if (dr["NonContinuity"].ToString() == "0" && MyUtility.Check.Empty(dr["Continuity1"])) fin_result = "";
                dr["Result1"] = fin_result;
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
                if (index >= find_dr.Length) index = 0;
            }
            detailgridbs.Position = DetailDatas.IndexOf(find_dr[index]);
        }

        private void modifyPhysicalInspectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            var dr =this.CurrentDetailData; if (null == dr) return;
            //DataTable dt = (DataTable)detailgridbs.DataSource
            //var drd = dr["whseArrival"];
            var frm = new Sci.Production.Quality.P01_PhysicalInspection(IsSupportEdit, CurrentDetailData["ID"].ToString(), null, null, dr);
            frm.ShowDialog(this);
            frm.Dispose();
            this.RenewData();
        }
    }
}

