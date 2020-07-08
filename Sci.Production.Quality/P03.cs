using Ict;
using Ict.Win;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Sci.Data;
using System.Transactions;
using System.Data.SqlClient;

namespace Sci.Production.Quality
{
    public partial class P03 : Win.Tems.Input6
    {
        private string loginID = Sci.Env.User.UserID;
        private string keyWord = Sci.Env.User.Keyword;
        int index;
        DataRow[] find_dr;

        public P03(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.detailgridmenus.Items.Remove(this.appendmenu);
            this.detailgridmenus.Items.Remove(this.modifymenu);
            this.detailgridmenus.Items.Remove(this.deletemenu);
            foreach (ToolStripItem m in this.contextMenuStrip1.Items)
            {
                // detailgridmenus.Items.Add(m);
                // m.Visible = false;
            }

            // detailgrid.ContextMenuStrip = contextMenuStrip1;
            // detailgridmenus =  contextMenuStrip1;

            // contextMenuStrip1.VisibleChanged += contextMenuStrip1_VisibleChanged;
            this.InsertDetailGridOnDoubleClick = false;
        }

        protected override DetailGridContextMenuMode CurrentDetailGridContextMenuMode()
        {
            // 非編輯狀態不顯示
            if (!this.EditMode)
            {
                return DetailGridContextMenuMode.Editable;
            }

            return DetailGridContextMenuMode.None;
        }

        void contextMenuStrip1_VisibleChanged(object sender, EventArgs e)
        {
            if (this.contextMenuStrip1.Visible && this.EditMode)
            {
                foreach (ToolStripItem m in this.contextMenuStrip1.Items)
                {
                    m.Visible = false;
                }
            }
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.detailgrid.AutoResizeColumns();

            DataRow queryDr;
            DualResult dResult = PublicPrg.Prgs.QueryQaInspectionHeader(this.CurrentMaintain["ID"].ToString(), out queryDr);
            if (!dResult)
            {
                this.ShowErr(dResult);
                return;
            }

            DataTable sciTb;
            string query_cmd = string.Format("select * from Getsci('{0}','{1}')", this.CurrentMaintain["ID"], MyUtility.Check.Empty(queryDr) ? string.Empty : queryDr["Category"]);
            DBProxy.Current.Select(null, query_cmd, out sciTb);
            if (!dResult)
            {
                this.ShowErr(query_cmd, dResult);
                return;
            }

            // 找出Cutinline and MinSciDelivery 比較早的日期
            DateTime? targT = Sci.Production.PublicPrg.Prgs.GetTargetLeadTime(MyUtility.Check.Empty(queryDr) ? string.Empty : queryDr["CUTINLINE"], sciTb.Rows[0]["MinSciDelivery"]);
            if (targT != null)
            {
                this.dateTargetLeadTime.Text = ((DateTime)targT).ToShortDateString();
            }
            else
            {
                this.dateTargetLeadTime.Text = string.Empty;
            }

            this.displayStyle.Text = MyUtility.Check.Empty(queryDr) ? string.Empty : queryDr["Styleid"].ToString();
            this.displaySeason.Text = MyUtility.Check.Empty(queryDr) ? string.Empty : queryDr["Seasonid"].ToString();
            this.displayBrand.Text = MyUtility.Check.Empty(queryDr) ? string.Empty : queryDr["brandid"].ToString();
            if (MyUtility.Check.Empty(queryDr))
            {
                this.dateEarliestEstCutDate.Value = null;
            }
            else
            {
                if (queryDr["cutinline"] == DBNull.Value)
                {
                    this.dateEarliestEstCutDate.Text = string.Empty;
                }
                else
                {
                    this.dateEarliestEstCutDate.Value = MyUtility.Convert.GetDate(queryDr["cutinline"]);
                }
            }

            this.displayMTLCmlpt.Text = this.CurrentMaintain["Complete"].ToString() == "True" ? "Y" : "N";
            decimal detailRowCount = this.DetailDatas.Count;
            string inspnum = "0";
            DataTable detailTb = (DataTable)this.detailgridbs.DataSource;
            if (detailRowCount != 0)
            {
                if (detailTb.Rows.Count != 0)
                {
                    DataRow[] inspectAry = detailTb.Select("Result<>'' or (nonCrocking and nonWash and nonHeat)");

                    if (inspectAry.Length > 0)
                    {
                        inspnum = Math.Round(((decimal)inspectAry.Length / detailRowCount) * 100, 2).ToString();
                    }
                }
            }

            DateTime completedate;
            if (inspnum == "100")
            {
                DataTable dtMaxDate;
                string sqlDate = string.Format(
                    @"select max(date) as MaxDate from (
select  MAX(CrockingDate) AS date  from FIR_Laboratory WITH (NOLOCK) 
where POID='{0}'
union all
select  MAX(HeatDate) as date from FIR_Laboratory WITH (NOLOCK) 
where POID='{0}'
union all
select  MAX(WashDate) AS date from FIR_Laboratory WITH (NOLOCK) 
where POID='{0}'
) a", this.CurrentMaintain["ID"]);
                DBProxy.Current.Select(null, sqlDate, out dtMaxDate);
                if (MyUtility.Check.Empty(dtMaxDate.Rows[0]["MaxDate"]))
                {
                    this.dateCompletionDate.Text = string.Empty;
                }
                else
                {
                    completedate = (DateTime)dtMaxDate.Rows[0]["MaxDate"];
                    this.dateCompletionDate.Text = completedate.ToString("yyyy/MM/dd");
                }
            }
            else
            {
                this.dateCompletionDate.Text = string.Empty;
            }
        }

        // 表身額外的資料來源
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["id"].ToString();
            string cmd = string.Format(
@"select 
    a.id,
    a.poid,
    seq = concat(a.SEQ1,a.SEQ2),
    wkno = c.ExportID,
    c.WhseArrival,
    a.SEQ1,
    a.SEQ2,
    a.Refno,
    a.SCIRefno, 
    b.CrockingEncode,b.HeatEncode,b.WashEncode,
    ArriveQty,
	Colorid = (Select d.colorid from PO_Supp_Detail d WITH (NOLOCK) Where d.id = a.poid and d.seq1 = a.seq1 and d.seq2 = a.seq2),
    Supplier = (select concat(Suppid,f.AbbEN) as supplier from Supp f WITH (NOLOCK) where a.Suppid=f.ID),
    b.ReceiveSampleDate,b.InspDeadline,b.Result,b.Crocking,b.nonCrocking,b.CrockingDate,b.nonHeat,Heat,b.HeatDate,
    b.nonWash,b.Wash,b.WashDate,a.ReceivingID
from FIR a WITH (NOLOCK) 
left join FIR_Laboratory b WITH (NOLOCK) on a.ID=b.ID
left join Receiving c WITH (NOLOCK) on c.id = a.receivingid
Where a.poid='{0}' 
order by a.seq1,a.seq2,a.Refno ",
masterID);
            this.DetailSelectCommand = cmd;
            return base.OnDetailSelectCommandPrepare(e);
        }

        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();

            // Grid 事件屬性: 右鍵跳出新視窗
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
                if (this.EditMode)
                {
                    return;
                }

                var dr = this.CurrentDetailData;
                if (dr == null)
                {
                    return;
                }

                var frm = new P03_Crocking(false, this.CurrentDetailData["ID"].ToString(), null, null, dr);
                frm.ShowDialog(this);
                frm.Dispose();
                this.RenewData();
            };

            crockingD.CellMouseDoubleClick += (s, e) =>
            {
                if (this.EditMode)
                {
                    return;
                }

                var dr = this.CurrentDetailData;
                if (dr == null)
                {
                    return;
                }

                var frm = new P03_Crocking(false, this.CurrentDetailData["ID"].ToString(), null, null, dr);
                frm.ShowDialog(this);
                frm.Dispose();
                this.RenewData();
            };

            wash.CellMouseDoubleClick += (s, e) =>
            {
                if (this.EditMode)
                {
                    return;
                }

                var dr = this.CurrentDetailData;
                if (dr == null)
                {
                    return;
                }

                var frm = new P03_Wash(false, this.CurrentDetailData["ID"].ToString(), null, null, dr);
                frm.ShowDialog(this);
                frm.Dispose();
                this.RenewData();
            };
            washD.CellMouseDoubleClick += (s, e) =>
            {
                if (this.EditMode)
                {
                    return;
                }

                var dr = this.CurrentDetailData;
                if (dr == null)
                {
                    return;
                }

                var frm = new P03_Wash(false, this.CurrentDetailData["ID"].ToString(), null, null, dr);
                frm.ShowDialog(this);
                frm.Dispose();
                this.RenewData();
            };

            heat.CellMouseDoubleClick += (s, e) =>
            {
                if (this.EditMode)
                {
                    return;
                }

                var dr = this.CurrentDetailData;
                if (dr == null)
                {
                    return;
                }

                var frm = new P03_Heat(false, this.CurrentDetailData["ID"].ToString(), null, null, dr);
                frm.ShowDialog(this);
                frm.Dispose();
                this.RenewData();
            };

            heatD.CellMouseDoubleClick += (s, e) =>
            {
                if (this.EditMode)
                {
                    return;
                }

                var dr = this.CurrentDetailData;
                if (dr == null)
                {
                    return;
                }

                var frm = new P03_Heat(false, this.CurrentDetailData["ID"].ToString(), null, null, dr);
                frm.ShowDialog(this);
                frm.Dispose();
                this.RenewData();
            };

            #endregion
            #region Valid & Edit

            nonCrocking.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                dr["nonCrocking"] = e.FormattedValue;
                dr.EndEdit();
                DataTable dt = (DataTable)this.detailgridbs.DataSource;
                this.FinalResult(dr);
            };
            nonWash.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                dr["nonWash"] = e.FormattedValue;
                dr.EndEdit();
                DataTable dt = (DataTable)this.detailgridbs.DataSource;
                this.FinalResult(dr);
            };
            nonHeat.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                dr["nonHeat"] = e.FormattedValue;
                dr.EndEdit();
                DataTable dt = (DataTable)this.detailgridbs.DataSource;
                this.FinalResult(dr);
            };

            #endregion

            #region set Grid

            this.detailgrid.IsEditingReadOnly = false;

            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("SEQ", header: "SEQ", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Text("wkno", header: "WKNO", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Date("WhseArrival", header: "Arrive W/H Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("SCIRefno", header: "SCI Refno", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("Refno", header: "Refno", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("Colorid", header: "Color", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("Supplier", header: "Supplier", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("ArriveQty", header: "Arrive Qty", width: Widths.AnsiChars(8), integer_places: 10, decimal_places: 2, iseditingreadonly: true)
                .Date("ReceiveSampleDate", header: "Sample Rcv.Date", width: Widths.AnsiChars(10)) // write
                .Date("InspDeadline", header: "Insp. Deadline", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Result", header: "All Result", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .CheckBox("nonCrocking", header: "Crocking N/A", width: Widths.AnsiChars(1), iseditable: true, trueValue: 1, falseValue: 0, settings: nonCrocking)
                .Text("Crocking", header: "Crocking Result", width: Widths.AnsiChars(10), iseditingreadonly: true, settings: crocking)
                .Date("CrockingDate", header: "Crocking Test Date", width: Widths.AnsiChars(10), iseditingreadonly: true, settings: crockingD)
                .CheckBox("nonHeat", header: "HT N/A", width: Widths.AnsiChars(1), trueValue: 1, falseValue: 0, settings: nonHeat)
                .Text("Heat", header: "Heat Result", width: Widths.AnsiChars(10), iseditingreadonly: true, settings: heat)
                .Date("HeatDate", header: "Heat Last Test Date", width: Widths.AnsiChars(10), iseditingreadonly: true, settings: heatD)
                .CheckBox("nonWash", header: "Wash N/A", width: Widths.AnsiChars(1), trueValue: 1, falseValue: 0, settings: nonWash)
                .Text("Wash", header: "Wash Result", width: Widths.AnsiChars(10), iseditingreadonly: true, settings: wash)
                .Date("WashDate", header: "Wash Last Test Date", width: Widths.AnsiChars(10), iseditingreadonly: true, settings: washD)
                .Text("ReceivingID", header: "Receiving ID", width: Widths.AnsiChars(15), iseditingreadonly: true);

            this.detailgrid.Columns["ReceiveSampleDate"].DefaultCellStyle.BackColor = Color.MistyRose;
            this.detailgrid.Columns["nonCrocking"].DefaultCellStyle.BackColor = Color.MistyRose;
            this.detailgrid.Columns["Crocking"].DefaultCellStyle.BackColor = Color.LemonChiffon;
            this.detailgrid.Columns["CrockingDate"].DefaultCellStyle.BackColor = Color.LemonChiffon;
            this.detailgrid.Columns["nonHeat"].DefaultCellStyle.BackColor = Color.MistyRose;
            this.detailgrid.Columns["Heat"].DefaultCellStyle.BackColor = Color.LightCyan;
            this.detailgrid.Columns["HeatDate"].DefaultCellStyle.BackColor = Color.LightCyan;
            this.detailgrid.Columns["nonWash"].DefaultCellStyle.BackColor = Color.MistyRose;
            this.detailgrid.Columns["Wash"].DefaultCellStyle.BackColor = Color.LightBlue;
            this.detailgrid.Columns["WashDate"].DefaultCellStyle.BackColor = Color.LightBlue;

            #endregion

        }

        protected override DualResult ClickSave()
        {
            // 因為表頭是PO不能覆蓋其他資料，必需自行存檔
            List<SqlParameter> spam_po = new List<SqlParameter>();
            string save_po_cmd = "update po set FirLaboratoryRemark = @remark where id = @id";
            spam_po.Add(new SqlParameter("@remark", this.CurrentMaintain["FirLaboratoryRemark"]));
            spam_po.Add(new SqlParameter("@id", this.CurrentMaintain["ID"]));

            foreach (DataRow dr in this.DetailDatas)
            {
                if (dr.RowState == DataRowState.Modified)
                {
                    List<SqlParameter> spam_non = new List<SqlParameter>();
                    int nonCk = dr["nonCrocking"].ToString() == "True" ? 1 : 0;
                    int nonWash = dr["nonWash"].ToString() == "True" ? 1 : 0;
                    int nonHeat = dr["nonHeat"].ToString() == "True" ? 1 : 0;
                    string save_non_cmd = "Update FIR_Laboratory set nonCrocking=@nonCk, nonWash=@nonWash, nonHeat=@nonHeat, ReceiveSampleDate=@RSD where id=@id";
                    spam_non.Add(new SqlParameter("@nonCk", nonCk));
                    spam_non.Add(new SqlParameter("@nonWash", nonWash));
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
                    if (!(upResult = DBProxy.Current.Execute(null, save_po_cmd, spam_po)))
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
                    this.ShowErr("Commit transaction error.", ex);
                    return Result.True;
                }
            }
            #region Over All Result 寫入
            DataRow row = this.detailgrid.GetDataRow(this.detailgridbs.Position);

            foreach (DataRow dr in this.DetailDatas)
            {
                if (dr.RowState == DataRowState.Modified)
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

            return Result.True;
        }

        protected override void ClickSaveAfter()
        {
            DualResult upResult;
            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    // 更新PO.FIRLabInspPercent
                    if (!(upResult = DBProxy.Current.Execute(null, $"exec UpdateInspPercent 'FIRLab','{this.CurrentMaintain["ID"]}'")))
                    {
                        _transactionscope.Dispose();
                        return;
                    }

                    _transactionscope.Complete();
                    _transactionscope.Dispose();
                }
                catch (Exception ex)
                {
                    _transactionscope.Dispose();
                    this.ShowErr("Commit transaction error.", ex);
                    return;
                }
            }

            _transactionscope.Dispose();
            _transactionscope = null;
            this.RenewData();
            base.ClickSaveAfter();
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            DataTable detDtb = (DataTable)this.detailgridbs.DataSource;

            // 移到指定那筆
            string wk = this.txtLocateforWK.Text;
            string seq1 = this.txtSEQ1.Text;
            string seq2 = this.txtSEQ2.Text;
            string find = string.Empty;
            string find_new = string.Empty;

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
                this.find_dr = detDtb.Select(find_new);
                if (this.find_dr.Length == 0)
                {
                    MyUtility.Msg.WarningBox("Not Found");
                    return;
                }
                else
                {
                    this.index = 0;
                }
            }
            else
            {
                if (this.find_dr == null)
                {
                    return;
                }
                else
                {
                    this.index++;
                    if (this.index >= this.find_dr.Length)
                    {
                        this.index = 0;
                    }
                }
            }

            this.detailgridbs.Position = this.DetailDatas.IndexOf(this.find_dr[this.index]);
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
            if (MyUtility.Check.Empty(this.detailgrid) || this.detailgrid.RowCount == 0)
            {
                return;
            }

            string currentID = this.CurrentDetailData["ID"].ToString();
            var dr = this.CurrentDetailData;
            var frm = new P03_Crocking(this.IsSupportEdit, this.CurrentDetailData["ID"].ToString(), null, null, dr);
            frm.ShowDialog(this);
            frm.Dispose();
            this.RenewData();

            // 固定滑鼠指向位置,避免被renew影響
            int rowindex = 0;
            for (int rIdx = 0; rIdx < this.detailgrid.Rows.Count; rIdx++)
            {
                DataGridViewRow dvr = this.detailgrid.Rows[rIdx];
                DataRow row = ((DataRowView)dvr.DataBoundItem).Row;

                if (row["ID"].ToString() == currentID)
                {
                    rowindex = rIdx;
                    break;
                }
            }

            this.detailgrid.SelectRowTo(rowindex);
        }

        private void modifyHeatTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.detailgrid) || this.detailgrid.RowCount == 0)
            {
                return;
            }

            string currentID = this.CurrentDetailData["ID"].ToString();
            var dr = this.CurrentDetailData;
            var frm = new P03_Heat(this.IsSupportEdit, this.CurrentDetailData["ID"].ToString(), null, null, dr);
            frm.ShowDialog(this);
            frm.Dispose();
            this.RenewData();

            // 固定滑鼠指向位置,避免被renew影響
            int rowindex = 0;
            for (int rIdx = 0; rIdx < this.detailgrid.Rows.Count; rIdx++)
            {
                DataGridViewRow dvr = this.detailgrid.Rows[rIdx];
                DataRow row = ((DataRowView)dvr.DataBoundItem).Row;

                if (row["ID"].ToString() == currentID)
                {
                    rowindex = rIdx;
                    break;
                }
            }

            this.detailgrid.SelectRowTo(rowindex);
        }

        private void modifyWashTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.detailgrid) || this.detailgrid.RowCount == 0)
            {
                return;
            }

            string currentID = this.CurrentDetailData["ID"].ToString();
            var dr = this.CurrentDetailData;
            var frm = new P03_Wash(this.IsSupportEdit, this.CurrentDetailData["ID"].ToString(), null, null, dr);
            frm.ShowDialog(this);
            frm.Dispose();
            this.RenewData();

            // 固定滑鼠指向位置,避免被renew影響
            int rowindex = 0;
            for (int rIdx = 0; rIdx < this.detailgrid.Rows.Count; rIdx++)
            {
                DataGridViewRow dvr = this.detailgrid.Rows[rIdx];
                DataRow row = ((DataRowView)dvr.DataBoundItem).Row;

                if (row["ID"].ToString() == currentID)
                {
                    rowindex = rIdx;
                    break;
                }
            }

            this.detailgrid.SelectRowTo(rowindex);
        }
    }
}
