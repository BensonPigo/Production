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
    public partial class P03 : Sci.Win.Tems.Input6
    {
        private string loginID = Sci.Env.User.UserID;
        private string keyWord = Sci.Env.User.Keyword;

        string find = "";
        int index;
        DataRow[] find_dr;
        public P03(ToolStripMenuItem menuitem) : base(menuitem)
        {
            InitializeComponent();
            detailgrid.ContextMenuStrip = contextMenuStrip1;            
        }
         public P03(string POID)
        {
             
            InitializeComponent();
            DefaultFilter = string.Format("POID = '{0}'", POID);
            InsertDetailGridOnDoubleClick = false;
            IsSupportEdit = false;
            detailgrid.ContextMenuStrip = contextMenuStrip1;
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
             string query_cmd = string.Format("select * from Getsci('{0}','{1}')",  CurrentMaintain["ID"], MyUtility.Check.Empty(queryDr) ? "" : queryDr["Category"]);
             DBProxy.Current.Select(null, query_cmd, out sciTb);
             if (!dResult)
             {
                 ShowErr(query_cmd, dResult);
                 return;
             }
             //Get scidelivery_box.Text  value
             if (sciTb.Rows.Count > 0)
             {
                 if (sciTb.Rows[0]["MinSciDelivery"] == DBNull.Value)
                 {
                     scidelivery_box.Text = "";
                 }
                 else
                 {
                     scidelivery_box.Text = Convert.ToDateTime(sciTb.Rows[0]["MinSciDelivery"]).ToShortDateString();
                 }
             }
             else
             {
                 scidelivery_box.Text = "";
             }
             //找出Cutinline and MinSciDelivery 比較早的日期
             DateTime? targT = Sci.Production.PublicPrg.Prgs.GetTargetLeadTime(MyUtility.Check.Empty(queryDr) ? "" : queryDr["CUTINLINE"], sciTb.Rows[0]["MinSciDelivery"]);
             if (targT != null)
             {
                 leadtime_box.Text = ((DateTime)targT).ToShortDateString();
             }
             else
             {
                 leadtime_box.Text = "";
             }
             style_box.Text = MyUtility.Check.Empty(queryDr) ? "" : queryDr["Styleid"].ToString();
             season_box.Text = MyUtility.Check.Empty(queryDr) ? "" : queryDr["Seasonid"].ToString();
             brand_box.Text = MyUtility.Check.Empty(queryDr) ? "" : queryDr["brandid"].ToString();
             if (MyUtility.Check.Empty(queryDr))
             {
                 estcutdate_box.Value = null;
             }
             else
             {
                 if (queryDr["cutinline"] == DBNull.Value) estcutdate_box.Text = "";
                 else estcutdate_box.Value = MyUtility.Convert.GetDate(queryDr["cutinline"]);
                     //Convert.ToDateTime(queryDr["cutinline"]).ToShortDateString();
             }

             mtl_box.Text = CurrentMaintain["Complete"].ToString() == "True" ? "Y" : "N";
             decimal detailRowCount = DetailDatas.Count;
             string inspnum = "0";
             DataTable detailTb = (DataTable)detailgridbs.DataSource;
             if (detailRowCount != 0)
             {
                 if (detailTb.Rows.Count != 0)
                 {
                     //DataRow[] inspectAry = detailTb.Select("Result<>'' or Result in ('Pass','Fail') or (nonCrocking= 'T' or nonWash='T' or nonHeat='T')");
                     DataRow[] inspectAry = detailTb.Select("Result<>'' or (nonCrocking and nonWash and nonHeat)");

                     if (inspectAry.Length > 0)
                     {
                         inspnum = Math.Round(((decimal)inspectAry.Length / detailRowCount) * 100, 2).ToString();
                     }
                 }
             }
             insp_box.Text = inspnum;

             DateTime completedate;
             if (inspnum == "100")
             {
                 completedate = ((DateTime)detailTb.Compute("Max(InspDeadline)", ""));
                 Complete_box.Text = completedate.ToShortDateString();
             }
             else this.Complete_box.Text = "";
             

         }
       
        //表身額外的資料來源
        protected override Ict.DualResult OnDetailSelectCommandPrepare(Win.Tems.InputMasterDetail.PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["id"].ToString();
            string cmd = string.Format(
                @"select 
                a.id,
                a.poid,
                (a.SEQ1+a.SEQ2) as seq,
                c.ExportID as wkno,
                c.WhseArrival,
                a.SEQ1,
                a.SEQ2,
                Receivingid,
                Refno,
                a.SCIRefno, 
                b.CrockingEncode,b.HeatEncode,b.WashEncode,
                ArriveQty,
				 (
                Select d.colorid from PO_Supp_Detail d Where d.id = a.poid and d.seq1 = a.seq1 and d.seq2 = a.seq2
                ) as Colorid,
				(
				select Suppid+f.AbbEN as supplier from Supp f where a.Suppid=f.ID
				) as Supplier,
				b.ReceiveSampleDate,b.InspDeadline,b.Result,b.Crocking,b.nonCrocking,b.CrockingDate,b.nonHeat,Heat,b.HeatDate,
				b.nonWash,b.Wash,b.WashDate,a.ReceivingID
				from FIR a 
				left join FIR_Laboratory b on a.ID=b.ID
				left join Receiving c on c.id = a.receivingid
				Where a.poid='{0}' order by a.seq1,a.seq2,Refno ", masterID);
            this.DetailSelectCommand = cmd;
            return base.OnDetailSelectCommandPrepare(e);
        }
        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();

            //Grid 事件屬性: 右鍵跳出新視窗
            DataGridViewGeneratorTextColumnSettings wash = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorDateColumnSettings washD = new DataGridViewGeneratorDateColumnSettings();
            DataGridViewGeneratorTextColumnSettings heat = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorDateColumnSettings heatD = new DataGridViewGeneratorDateColumnSettings();
            DataGridViewGeneratorTextColumnSettings crocking = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorDateColumnSettings crockingD = new DataGridViewGeneratorDateColumnSettings();
            DataGridViewGeneratorCheckBoxColumnSettings nonCrocking = new DataGridViewGeneratorCheckBoxColumnSettings();
            DataGridViewGeneratorCheckBoxColumnSettings nonWash = new DataGridViewGeneratorCheckBoxColumnSettings();
            DataGridViewGeneratorCheckBoxColumnSettings nonHeat = new DataGridViewGeneratorCheckBoxColumnSettings();

            #region mouseClick
            crocking.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.CurrentDetailData;
                if (dr == null) return;
                var frm = new Sci.Production.Quality.P03_Crocking(false, this.CurrentDetailData["ID"].ToString(), null, null, dr);
                frm.ShowDialog(this);
                frm.Dispose();
                this.RenewData();
            };

            crockingD.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.CurrentDetailData;
                if (dr == null) return;
                var frm = new Sci.Production.Quality.P03_Crocking(false, this.CurrentDetailData["ID"].ToString(), null, null, dr);
                frm.ShowDialog(this);
                frm.Dispose();
                this.RenewData();
            };
            
            wash.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.CurrentDetailData;
                if (dr == null) return;
                var frm = new Sci.Production.Quality.P03_Wash(false, this.CurrentDetailData["ID"].ToString(),null,null, dr);
                frm.ShowDialog(this);
                frm.Dispose();
                this.RenewData();
            };
            washD.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.CurrentDetailData;
                if (dr == null) return;
                var frm = new Sci.Production.Quality.P03_Wash(false, this.CurrentDetailData["ID"].ToString(),null,null, dr);
                frm.ShowDialog(this);
                frm.Dispose();
                this.RenewData();
            };

            heat.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.CurrentDetailData;
                if (dr == null) return;
                var frm = new Sci.Production.Quality.P03_Heat(false, this.CurrentDetailData["ID"].ToString(),null,null, dr);
                frm.ShowDialog(this);
                frm.Dispose();
                this.RenewData();
            };

            heatD.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.CurrentDetailData;
                if (dr == null) return;
                var frm = new Sci.Production.Quality.P03_Heat(false, this.CurrentDetailData["ID"].ToString(), null, null, dr);
                frm.ShowDialog(this);
                frm.Dispose();
                this.RenewData();
            };

            #endregion
            #region Valid & Edit

            nonCrocking.CellValidating += (s, e) =>
                {
                    DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                    dr["nonCrocking"] = e.FormattedValue;
                    dr.EndEdit();
                    DataTable dt = (DataTable)detailgridbs.DataSource;
                    FinalResult(dr);
                };
            nonWash.CellValidating += (s, e) =>
            {
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                dr["nonWash"] = e.FormattedValue;
                dr.EndEdit();
                DataTable dt = (DataTable)detailgridbs.DataSource;
                FinalResult(dr);
            };
            nonHeat.CellValidating += (s, e) =>
            {
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                dr["nonHeat"] = e.FormattedValue;
                dr.EndEdit();
                DataTable dt = (DataTable)detailgridbs.DataSource;
                FinalResult(dr);
            };

            #endregion

            #region set Grid

            this.detailgrid.IsEditingReadOnly = false;

            Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("SEQ", header: "SEQ", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Text("wkno", header: "WKNO", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Date("WhseArrival", header: "Arrive W/H Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("SCIRefno", header: "SCI Refno", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("Refno", header: "Refno", width: Widths.AnsiChars(15), iseditingreadonly: true)                
                .Text("Colorid", header: "Color", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("Supplier", header: "Supplier", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("ArriveQty", header: "Arrive Qty", width: Widths.AnsiChars(8), integer_places: 10, decimal_places: 2, iseditingreadonly: true)
                .Date("ReceiveSampleDate", header: "Sample Rcv.Date", width: Widths.AnsiChars(10))//write
                .Date("InspDeadline", header: "Insp. Deadline", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Result", header: "All Result", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .CheckBox("nonCrocking", header: "Crocking N/A", width: Widths.AnsiChars(1), iseditable: true, trueValue: 1, falseValue: 0, settings: nonCrocking)
                .Text("Crocking", header: "Crocking Result", width: Widths.AnsiChars(10), iseditingreadonly: true,settings:crocking)
                .Date("CrockingDate", header: "Crocking Test Date", width: Widths.AnsiChars(10),iseditingreadonly:true ,settings:crockingD)
                .CheckBox("nonHeat", header: "HT N/A", width: Widths.AnsiChars(1), trueValue: 1, falseValue: 0,settings:nonHeat)
                .Text("Heat", header: "Heat Result", width: Widths.AnsiChars(10), iseditingreadonly: true,settings:heat)
                .Date("HeatDate", header: "Heat Last Test Date", width: Widths.AnsiChars(10),iseditingreadonly:true,settings:heatD)
                .CheckBox("nonWash", header: "Wash N/A", width: Widths.AnsiChars(1), trueValue: 1, falseValue: 0,settings:nonWash)
                .Text("Wash", header: "Wash Result", width: Widths.AnsiChars(10), iseditingreadonly: true,settings:wash)
                .Date("WashDate", header: "Wash Last Test Date", width: Widths.AnsiChars(10),iseditingreadonly:true,settings:washD)
                .Text("ReceivingID", header: "Receiving ID", width: Widths.AnsiChars(10), iseditingreadonly: true);


            detailgrid.Columns[8].DefaultCellStyle.BackColor = Color.MistyRose;
            detailgrid.Columns[11].DefaultCellStyle.BackColor = Color.MistyRose;
            detailgrid.Columns[12].DefaultCellStyle.BackColor = Color.LemonChiffon;
            detailgrid.Columns[13].DefaultCellStyle.BackColor = Color.LemonChiffon;
            detailgrid.Columns[14].DefaultCellStyle.BackColor = Color.MistyRose;
            detailgrid.Columns[15].DefaultCellStyle.BackColor = Color.LightCyan;
            detailgrid.Columns[16].DefaultCellStyle.BackColor = Color.LightCyan;
            detailgrid.Columns[17].DefaultCellStyle.BackColor = Color.MistyRose;
            detailgrid.Columns[18].DefaultCellStyle.BackColor = Color.LightBlue;
            detailgrid.Columns[19].DefaultCellStyle.BackColor = Color.LightBlue;
            
            #endregion


        }
       
        protected override DualResult ClickSave()
        {
            //因為表頭是PO不能覆蓋其他資料，必需自行存檔
           
            
            List<SqlParameter> spam_po = new List<SqlParameter>();                      
            string save_po_cmd = "update po set FirLaboratoryRemark = @remark where id = @id";
            spam_po.Add(new SqlParameter("@remark",CurrentMaintain["FirLaboratoryRemark"]));
            spam_po.Add(new SqlParameter("@id",CurrentMaintain["ID"]));
            
            foreach (DataRow dr in DetailDatas)
            {
                if (dr.RowState==DataRowState.Modified)
                {
                    List<SqlParameter> spam_non = new List<SqlParameter>();  
                    int nonCk = dr["nonCrocking"].ToString() == "True" ? 1 : 0;
                    int nonWash = dr["nonWash"].ToString() == "True" ? 1 : 0;
                    int nonHeat = dr["nonHeat"].ToString() == "True" ? 1 : 0;
                    string save_non_cmd = "Update FIR_Laboratory set nonCrocking=@nonCk, nonWash=@nonWash, nonHeat=@nonHeat, ReceiveSampleDate=@RSD where id=@id";                   
                    spam_non.Add(new SqlParameter("@nonCk", nonCk));
                    spam_non.Add(new SqlParameter("@nonWash",nonWash));
                    spam_non.Add(new SqlParameter("@nonHeat", nonHeat));
                    spam_non.Add(new SqlParameter("@RSD", dr["ReceiveSampleDate"]));
                    spam_non.Add(new SqlParameter("@id", dr["ID"]));
                    DBProxy.Current.Execute(null, save_non_cmd, spam_non);
                    
                }
                
            }
            DualResult upResult;
            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {

                    if (!(upResult = DBProxy.Current.Execute(null, save_po_cmd,spam_po)))
                    {
                        _transactionscope.Dispose();
                        return upResult;
                    }
                  
                    _transactionscope.Complete();
                    MyUtility.Msg.WarningBox("Successfully");
                }
                catch (Exception ex)
                {
                    _transactionscope.Dispose();
                    ShowErr("Commit transaction error.", ex);
                    return Result.True;
                }
            }
            #region Over All Result 寫入
            DataRow row = this.detailgrid.GetDataRow(this.detailgridbs.Position);

            foreach (DataRow dr in DetailDatas)
            {
                if (dr.RowState==DataRowState.Modified)
                {
                    string[] returnstr = Sci.Production.PublicPrg.Prgs.GetOverallResult_Lab(dr["ID"]);
                    string cmdResult = @"update Fir_Laboratory set Result=@Result where id=@id ";
                    List<SqlParameter> spam = new List<SqlParameter>();
                    spam.Add(new SqlParameter("@Result", returnstr[0]));
                    spam.Add(new SqlParameter("@id", dr["ID"]));
                    DBProxy.Current.Execute(null, cmdResult, spam);
                }
            }
           
            #endregion
            _transactionscope.Dispose();
            _transactionscope = null;
            this.RenewData();
            this.OnDetailEntered();
            EnsureToolbarExt();
            return Result.True;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataTable detDtb = (DataTable)detailgridbs.DataSource;
            //移到指定那筆
            string wk = wk_box.Text;
            string seq1 = seq1_box.Text;
            string seq2 = seq2_box.Text;
            string find = "";
            string find_new = "";

            if (!MyUtility.Check.Empty(wk))
            {
                find_new = string.Format("wkno='{0}'", wk);
            }
            if (!MyUtility.Check.Empty(seq1))
            {
                if (!MyUtility.Check.Empty(find_new))
                {
                    find_new = find_new + string.Format(" and SEQ1 = '{0}'", seq1);
                }
                else
                {
                    find_new = string.Format("SEQ1 = '{0}'", seq1);
                }
            }
            if (!MyUtility.Check.Empty(seq2))
            {
                if (!MyUtility.Check.Empty(find_new))
                {
                    find_new = find_new + string.Format(" and SEQ2 = '{0}'", seq2);
                }
                else
                {
                    find_new = string.Format("SEQ2 = '{0}'", seq2);
                }
            }
            if (find != find_new)
            {
                find = find_new;
                find_dr = detDtb.Select(find_new);
                if (find_dr.Length == 0)
                {
                    MyUtility.Msg.WarningBox("Not+ Found");
                    return;
                }
                else { index = 0; }
            }
            else
            {
                if (find_dr == null)
                {
                    return;
                }
                else
                {
                    index++;
                    if (index >= find_dr.Length) index = 0;
                }
                
            }
            detailgridbs.Position = DetailDatas.IndexOf(find_dr[index]);
        }
        //權限 未完成
        private void button_enable()
        {
            //return;
            string menupk = MyUtility.GetValue.Lookup("Pkey", "Sci.Production.Quality.P03", "MenuDetail", "FormName");
            string pass0pk = MyUtility.GetValue.Lookup("FKPass0", loginID, "Pass1", "ID");
            DataRow pass2_dr;
            string pass2_cmd = string.Format("Select * from Pass2 Where FKPass0 ='{0}' and FKMenu='{1}'", pass0pk, menupk);
            int lApprove = 0; //有Confirm權限皆可按Pass的Approve, 有Check權限才可按Fail的Approve(TeamLeader 有Approve權限,Supervisor有Check)
            int lCheck = 0;
            if (MyUtility.Check.Seek(pass2_cmd, out pass2_dr))
            {
                lApprove = pass2_dr["CanConfirm"].ToString() == "True" ? 1 : 0;
                lCheck = pass2_dr["CanCheck"].ToString() == "True" ? 1 : 0;
            }

        }
        public void FinalResult(DataRow dr)
        {
            if (this.EditMode)
            {
                string[] returnResult = Sci.Production.PublicPrg.Prgs.GetOverallResult_Lab(dr["ID"]);
                dr["Result"] = returnResult[0];
            }
        }
      
        private void modifyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string currentID = this.CurrentDetailData["ID"].ToString();
            var dr = this.CurrentDetailData; if (null == dr) return;
            var frm = new Sci.Production.Quality.P03_Crocking(IsSupportEdit, CurrentDetailData["ID"].ToString(),null,null, dr);
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

        private void modifyHeatTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string currentID = this.CurrentDetailData["ID"].ToString();
            var dr = this.CurrentDetailData; if (null == dr) return;
            var frm = new Sci.Production.Quality.P03_Heat(IsSupportEdit, CurrentDetailData["ID"].ToString(),null,null, dr);
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

        private void modifyWashTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string currentID = this.CurrentDetailData["ID"].ToString();
            var dr = this.CurrentDetailData; if (null == dr) return;
            var frm = new Sci.Production.Quality.P03_Wash(IsSupportEdit, CurrentDetailData["ID"].ToString(),null,null, dr);
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
