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
    public partial class P02 : Sci.Win.Tems.Input6
    {
        // 宣告Context Menu Item
        ToolStripMenuItem edit;

        private string loginID = Sci.Env.User.UserID;
        private string keyWord = Sci.Env.User.Keyword;
        string find = "";
        int index;
        DataRow[] find_dr;

        public P02(ToolStripMenuItem menuitem) : base(menuitem)
        {
            InitializeComponent();
            detailgrid.ContextMenuStrip = detailgridmenus;

        }

        public P02(string POID)
        {
            InitializeComponent();
            DefaultFilter = string.Format("ID = '{0}'", POID);
            InsertDetailGridOnDoubleClick = false;
            IsSupportEdit = false;
            detailgrid.ContextMenuStrip = detailgridmenus;
        }

        //表身額外的資料來源
        protected override Ict.DualResult OnDetailSelectCommandPrepare(Win.Tems.InputMasterDetail.PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["id"].ToString();
            string cmd = string.Format(
                @"Select a.id,a.poid,SEQ1,SEQ2,Receivingid,Refno,SCIRefno,Suppid,
                ArriveQty,InspDeadline,Result,a.InspQty,a.RejectQty,a.Defect,a.Result,a.InspDate,
                (
				select Pass1.Name from Pass1 where a.Inspector = pass1.id
				) AS Inspector,
                a.Remark,a.ReplacementReportID,
                a.Status,ReplacementReportID,(seq1+seq2) as seq,
                (
                Select weavetypeid from Fabric b where b.SCIRefno =a.SCIrefno
                ) as weavetypeid,
                c.ID AS ReceivingID,c.whseArrival,
                (
                Select d.colorid from PO_Supp_Detail d Where d.id = a.poid and d.seq1 = a.seq1 and d.seq2 = a.seq2
                ) as Colorid,
                (
                Select d.SizeSpec from PO_Supp_Detail d Where d.id = a.poid and d.seq1 = a.seq1 and d.seq2 = a.seq2
                ) as Size,
                (
                Select d.StockUnit from PO_Supp_Detail d Where d.id = a.poid and d.seq1 = a.seq1 and d.seq2 = a.seq2
                ) as unit,
                (
                Select AbbEn From Supp Where a.suppid = supp.id
                ) as SuppEn
	                From AIR a Left join Receiving c on c.id = a.receivingid
                Where a.poid='{0}' order by seq1,seq2  ", masterID);
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
                var frm = new Sci.Production.Quality.P02_Detail(false,this.CurrentDetailData["ID"].ToString(),dr);
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
                .Text("SEQ", header: "SEQ1", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Text("ExportID", header: "WKNO", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Date("whseArrival", header: "Arrive W/H Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("SCIRefno", header: "SCI Refno", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("Refno", header: "Refno", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("SuppEn", header: "Supplier", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Colorid", header: "Color", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("Size", header: "Size", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("ArriveQty", header: "Arrive Qty", width: Widths.AnsiChars(8), integer_places: 10, decimal_places: 2, iseditingreadonly: true)
                .Text("unit", header: "Unit", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Date("InspDeadline", header: "Insp. Deadline", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("InspQty", header: "Inspected Qty", width: Widths.AnsiChars(8), integer_places: 10, decimal_places: 2, iseditingreadonly: true, settings: detail_Int)
                .Text("RejectQty", header: "Rehect Qty", width: Widths.AnsiChars(10), iseditingreadonly: true, settings: detail)
                .Text("defect", header: "Defect Type", width: Widths.AnsiChars(10), iseditingreadonly: true, settings: detail)
                .Text("Result", header: "Result", width: Widths.AnsiChars(10), iseditingreadonly: true, settings: detail)
                .Text("Inspdate", header: "Insp. Date", width: Widths.AnsiChars(10), iseditingreadonly: true, settings: detail)
                .Text("Inspector", header: "Inspector", width: Widths.AnsiChars(10), iseditingreadonly: true, settings: detail)
                .Text("Remark", header: "Remark", width: Widths.AnsiChars(10), iseditingreadonly: true, settings: detail   )            
                .Text("ReplacementID", header: "1st ReplacementID", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("ReceivingID", header: "Receiving ID", width: Widths.AnsiChars(15), iseditingreadonly: true);
            detailgrid.Columns[11].DefaultCellStyle.BackColor = Color.LemonChiffon;
            detailgrid.Columns[12].DefaultCellStyle.BackColor = Color.LemonChiffon;
            detailgrid.Columns[13].DefaultCellStyle.BackColor = Color.LemonChiffon;
            detailgrid.Columns[14].DefaultCellStyle.BackColor = Color.LemonChiffon;
            detailgrid.Columns[15].DefaultCellStyle.BackColor = Color.LemonChiffon;
            detailgrid.Columns[16].DefaultCellStyle.BackColor = Color.LemonChiffon;
            detailgrid.Columns[17].DefaultCellStyle.BackColor = Color.LemonChiffon;
            #endregion
         

        }

        protected override void OnFormLoaded()
        {

            this.detailgridmenus.Items.Clear(); // 清空原有的Menu Item
            Helper.Controls.ContextMenu.Generator(this.detailgridmenus).Menu("ModifyDetail", onclick: (s, e) => ModifyDetail()).Get(out edit);
            base.OnFormLoaded();
        }

        protected override void OnDetailEntered()
        {
            contextMenuStrip();
            base.OnDetailEntered();

            DataRow queryDr;
            DualResult dResult = PublicPrg.Prgs.QueryQaInspectionHeader(CurrentMaintain["ID"].ToString(), out queryDr);
            if (!dResult)
            {
                ShowErr(dResult);
                return;
            }
            DataTable sciTb;
            string query_cmd = string.Format("select * from Getsci('{0}','{1}')", CurrentMaintain["ID"], MyUtility.Check.Empty(queryDr) ? "" : queryDr["Category"]);
            DBProxy.Current.Select(null, query_cmd, out sciTb);
            if (!dResult)
            {
                ShowErr(query_cmd, dResult);
                return;
            }
            //Get scidelivery_box.Text  value
            if (sciTb.Rows.Count>0)
            {
                if (sciTb.Rows[0]["MinSciDelivery"]==DBNull.Value)
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
            if (targT!=null)
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
                estcutdate_box.Text = "";
            }
            else
            {
                if (queryDr["cutinline"] == DBNull.Value) estcutdate_box.Text = "";
                else estcutdate_box.Text = Convert.ToDateTime(queryDr["cutinline"]).ToShortDateString();
            }
           
            mtl_box.Text = CurrentMaintain["Complete"].ToString() == "1" ? "Y" : "";
            decimal detailRowCount = DetailDatas.Count;
            string inspnum = "0";
            DataTable detailTb = (DataTable)detailgridbs.DataSource;
            if (detailRowCount!=0)
            {
                if (detailTb.Rows.Count!=0)
                {
                    DataRow[] inspectAry = detailTb.Select("Result<>''");
                    
                    if (inspectAry.Length >0)
                    {
                        inspnum = Math.Round(((decimal)inspectAry.Length / detailRowCount) * 100, 2).ToString();
                    }
                }
            }

            insp_box.Text = inspnum;
            DateTime completedate;
            if (inspnum == "100")
            {
                completedate = ((DateTime)detailTb.Compute("Max(InspDate)", ""));
               

                Complete_box.Text = completedate.ToShortDateString();
            }
            else Complete_box.Text = "";

        }
        protected override DualResult ClickSave()
        {
            //因為表頭是PO不能覆蓋其他資料，必需自行存檔
            string save_po_cmd = string.Format("update po set AirRemark = '{0}' where id = '{1}';", CurrentMaintain["AiRemark"], CurrentMaintain["ID"]);
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

        private void ModifyDetail()
        {
            if (MyUtility.Check.Empty(CurrentDetailData["ID"].ToString())) return;
            string currentID = CurrentDetailData["ID"].ToString();
            var dr = this.CurrentDetailData; if (null == dr) return;
            var frm = new Sci.Production.Quality.P02_Detail(IsSupportEdit, CurrentDetailData["ID"].ToString(), dr);
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
            contextMenuStrip();

        }

        private void contextMenuStrip()
        {
            var dr = this.CurrentDetailData;
            edit.Enabled = true;

            if (dr==null)
            {
                edit.Enabled = false;
                return;
            }
        }

        //private void modifyDetailToolStripMenuItem_Click(object sender, EventArgs e)
        //{            
        //    string currentID = CurrentDetailData["ID"].ToString();
        //    var dr = this.CurrentDetailData; if (null == dr) return;
        //    var frm = new Sci.Production.Quality.P02_Detail(IsSupportEdit, CurrentDetailData["ID"].ToString(), dr);            
        //    frm.ShowDialog(this);
        //    frm.Dispose();
        //    this.RenewData();
        //    this.OnDetailEntered();
        //    // 固定滑鼠指向位置,避免被renew影響
        //    int rowindex = 0;
        //    for (int rIdx = 0; rIdx < detailgrid.Rows.Count; rIdx++)
        //    {
        //        DataGridViewRow dvr = detailgrid.Rows[rIdx];
        //        DataRow row = ((DataRowView)dvr.DataBoundItem).Row;

        //        if (row["ID"].ToString() == currentID)
        //        {
        //            rowindex = rIdx;
        //            break;
        //        }
        //    }
        //    detailgrid.SelectRowTo(rowindex);


        //}

        protected override void OnDetailGridRowChanged()
        {
            contextMenuStrip();
            base.OnDetailGridRowChanged();
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            DataTable detDtb = (DataTable)detailgridbs.DataSource;
            //移到指定那筆
            string wk = wk_box.Text;
            string seq1 = seq1_box.Text;
            string seq2 = seq2_box.Text;
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

    }
}
