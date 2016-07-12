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
    public partial class P03 : Sci.Win.Tems.Input6
    {
        private string loginID = Sci.Env.User.UserID;
        private string keyWord = Sci.Env.User.Keyword;
        string find = "";
        int index;
        string ing = "";
        DataRow[] find_dr;
        public P03(ToolStripMenuItem menuitem) : base(menuitem)
        {
            InitializeComponent();
        }
         public P03(string POID)
        {
            IsSupportEdit = false;
        }
        //表身額外的資料來源
        protected override Ict.DualResult OnDetailSelectCommandPrepare(Win.Tems.InputMasterDetail.PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["id"].ToString();
            string cmd = string.Format(
                @"select a.id,a.poid,(a.SEQ1+a.SEQ2) as seq,Receivingid,Refno,a.SCIRefno,
                ArriveQty,
				 (
                Select d.colorid from PO_Supp_Detail d Where d.id = a.poid and d.seq1 = a.seq1 and d.seq2 = a.seq2
                ) as Colorid,
				(
				select Suppid+f.AbbEN as supplier from Supp f where a.Suppid=f.ID
				) as Supplier,
				b.ReceiveSampleDate,b.InspDeadline,b.Result,b.nonCrocking,b.CrockingDate,b.nonHeat,Heat,b.HeatDate,
				b.nonWash,b.Wash,b.WashDate
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
            DataGridViewGeneratorTextColumnSettings detail = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorNumericColumnSettings detail_Int = new DataGridViewGeneratorNumericColumnSettings();


            detail.CellMouseDoubleClick += (s, e) =>
            {

                var dr = this.CurrentDetailData;
                if (dr == null) return;
                // 有疑問!!!!
                var frm = new Sci.Production.Quality.P02_Detail(false, this.CurrentDetailData["ID"].ToString(), dr);
                frm.ShowDialog(this);
                frm.Dispose();
                this.RenewData();
            };

            detail_Int.CellMouseDoubleClick += (s, e) =>
            {

                var dr = this.CurrentDetailData;
                if (dr == null) return;
                // 有疑問!!!!
                var frm = new Sci.Production.Quality.P02_Detail(false, this.CurrentDetailData["ID"].ToString(), dr);
                frm.ShowDialog(this);
                frm.Dispose();
                this.RenewData();
            };


            #region set Grid

            this.detailgrid.IsEditingReadOnly = false;

            Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("SEQ", header: "SEQ", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Text("ExportID", header: "WKNO", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Date("whseArrival", header: "Arrive W/H Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("SCIRefno", header: "SCI Ref#", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("Refno", header: "Brand Ref#", width: Widths.AnsiChars(15), iseditingreadonly: true)                
                .Text("Colorid", header: "Color", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("SuppEn", header: "Supplier", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("ArriveQty", header: "Arrive Qty", width: Widths.AnsiChars(8), integer_places: 10, decimal_places: 2, iseditingreadonly: true)
                .Date("ReceiveSamoleDate", header: "Sample Rcv.Date", width: Widths.AnsiChars(10))//write
                .Date("InspDeadline", header: "Insp. Deadline", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .CheckBox("nonCrocking", header: "Crocking N/A", width: Widths.AnsiChars(1),iseditable:true, trueValue: 1, falseValue: 0)//write
                .Text("Crocking", header: "Crocking Result", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Date("CrockingDate", header: "Crocking Test Date", width: Widths.AnsiChars(10))//write
                .CheckBox("nonHeat", header: "HT N/A", width: Widths.AnsiChars(1), trueValue: 1, falseValue: 0)//write
                .Text("Heat", header: "Heat Result", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Date("HeatDate", header: "Heat Last Test Date", width: Widths.AnsiChars(10))//write
                .CheckBox("nonWash", header: "Wash N/A", width: Widths.AnsiChars(1), trueValue: 1, falseValue: 0)//write
                .Text("Wash", header: "Wash Result", width: Widths.AnsiChars(10), iseditingreadonly: true)//write
                .Date("WashDate", header: "Wash Last Test Date", width: Widths.AnsiChars(10),iseditingreadonly:true)//write
                .Text("ExportID", header: "Receiving ID", width: Widths.AnsiChars(10), iseditingreadonly: true);
                
      
            //detailgrid.Columns[11].DefaultCellStyle.BackColor = Color.LemonChiffon;
            //detailgrid.Columns[12].DefaultCellStyle.BackColor = Color.LemonChiffon;
            //detailgrid.Columns[13].DefaultCellStyle.BackColor = Color.LemonChiffon;
            //detailgrid.Columns[14].DefaultCellStyle.BackColor = Color.LemonChiffon;
            //detailgrid.Columns[15].DefaultCellStyle.BackColor = Color.LemonChiffon;
            //detailgrid.Columns[16].DefaultCellStyle.BackColor = Color.LemonChiffon;
            //detailgrid.Columns[17].DefaultCellStyle.BackColor = Color.LemonChiffon;
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
            string query_cmd = string.Format("select * from Getsci('{0}','{1}')", CurrentMaintain["ID"], queryDr["Category"]);
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
            DateTime? targT = Sci.Production.PublicPrg.Prgs.GetTargetLeadTime(queryDr["CUTINLINE"], sciTb.Rows[0]["MinSciDelivery"]);
            if (targT != null)
            {
                leadtime_box.Text = ((DateTime)targT).ToShortDateString();
            }
            else
            {
                leadtime_box.Text = "";
            }
            style_box.Text = queryDr["Styleid"].ToString();
            season_box.Text = queryDr["Seasonid"].ToString();
            brand_box.Text = queryDr["brandid"].ToString();
            if (queryDr["cutinline"] == DBNull.Value)
            {
                estcutdate_box.Text = "";
            }
            else
            {
                estcutdate_box.Text = Convert.ToDateTime(queryDr["cutinline"]).ToShortDateString();
            }
            mtl_box.Text = CurrentMaintain["Complete"].ToString() == "1" ? "Y" : "";
            decimal detailRowCount = DetailDatas.Count;
            string inspnum = "0";
            DataTable detailTb = (DataTable)detailgridbs.DataSource;
            if (detailRowCount != 0)
            {
                if (detailTb.Rows.Count != 0)
                {
                    DataRow[] inspectAry = detailTb.Select("Result<>''");

                    if (inspectAry.Length > 0)
                    {
                        inspnum = Math.Round(((decimal)inspectAry.Length / detailRowCount) * 100, 2).ToString();
                    }
                }
            }

            insp_box.Text = inspnum;
            DateTime completedate, Physicalcompletedate, Weightcompletedate, ShadeBondcompletedate, Continuitycompletedate;
            if (inspnum == "100")
            {
                Physicalcompletedate = ((DateTime)detailTb.Compute("Max(PhysicalDate)", ""));
                Weightcompletedate = ((DateTime)detailTb.Compute("Max(WeightDate)", ""));
                ShadeBondcompletedate = ((DateTime)detailTb.Compute("Max(ShadeBondDate)", ""));
                Continuitycompletedate = ((DateTime)detailTb.Compute("Max(ContinuityDate)", ""));
                if (DateTime.Compare(Physicalcompletedate, Weightcompletedate) < 0)
                {
                    completedate = Weightcompletedate;
                }
                else
                {
                    completedate = Physicalcompletedate;
                }
                if (DateTime.Compare(completedate, ShadeBondcompletedate) < 0)
                {
                    completedate = ShadeBondcompletedate;
                }
                if (DateTime.Compare(completedate, Continuitycompletedate) < 0)
                {
                    completedate = Continuitycompletedate;
                }

                Complete_box.Text = completedate.ToShortDateString();
            }
            else Complete_box.Text = "";

        }
        protected override DualResult ClickSave()
        {
            //因為表頭是PO不能覆蓋其他資料，必需自行存檔
            string save_po_cmd = string.Format("update po set FirLaboratoryRemark = '{0}' where id = '{1}';", CurrentMaintain["FirLaboratoryRemark"], CurrentMaintain["ID"]);
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
                find_new = string.Format("Exportid='{0}'", wk);
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

        private void modifyCrockingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dr = this.CurrentDetailData; if (null == dr) return;
            var frm = new Sci.Production.Quality.P03_Crocking(IsSupportEdit, CurrentDetailData["ID"].ToString(), dr);
            frm.ShowDialog(this);
            frm.Dispose();
            this.RenewData();
        }
        private void modifyHeatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dr = this.CurrentDetailData; if (null == dr) return;
            var frm = new Sci.Production.Quality.P03_Heat(IsSupportEdit, CurrentDetailData["ID"].ToString(), dr);
            frm.ShowDialog(this);
            frm.Dispose();
            this.RenewData();
        }
        private void modifyWashToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dr = this.CurrentDetailData; if (null == dr) return;
            var frm = new Sci.Production.Quality.P03_Wash(IsSupportEdit, CurrentDetailData["ID"].ToString(), dr);
            frm.ShowDialog(this);
            frm.Dispose();
            this.RenewData();
        }
    }
}
