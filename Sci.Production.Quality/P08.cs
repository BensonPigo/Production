using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using Ict.Win;
using Sci.Win.Tools;
using System.Transactions;

namespace Sci.Production.Quality
{
    /// <summary>
    /// P08
    /// </summary>
    public partial class P08 : Win.Tems.QueryForm
    {
        /// <summary>
        /// P08
        /// </summary>
        public P08(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.EditMode = true;

            Dictionary<string, string> comboBoxUpdateTime_RowSource = new Dictionary<string, string>
            {
                { "CutTime", "Cut Shadeband Time" },
                { "PasteTime", "Paste Shadeband Time" },
                { "PassQATime", "Pass QA Time" },
            };
            this.comboBoxUpdateTime.DataSource = new BindingSource(comboBoxUpdateTime_RowSource, null);
            this.comboBoxUpdateTime.ValueMember = "Key";
            this.comboBoxUpdateTime.DisplayMember = "Value";
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.dateTimePickerUpdateTime.CustomFormat = "yyyy/MM/dd HH:mm:ss";
            this.dateTimePickerUpdateTime.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            this.GridSetup();
        }

        /// <inheritdoc/>
        private void GridSetup()
        {
            DataGridViewGeneratorTextColumnSettings cellShadebandDocLocationID = new DataGridViewGeneratorTextColumnSettings();
            cellShadebandDocLocationID.CellMouseDoubleClick += (s, e) =>
            {
                this.GridShadebandDocLocationIDCellPop(e.RowIndex);
            };

            cellShadebandDocLocationID.EditingMouseUp += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    this.GridShadebandDocLocationIDCellPop(e.RowIndex);
                }
            };

            cellShadebandDocLocationID.CellValidating += (s, e) =>
            {
                DataRow curDr = this.gridReceiving.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(e.FormattedValue))
                {
                    curDr["ShadebandDocLocationID"] = string.Empty;
                    curDr.EndEdit();
                    return;
                }

                List<SqlParameter> cmds = new List<SqlParameter>()
                {
                    new SqlParameter("@ID", MyUtility.Convert.GetString(e.FormattedValue)),
                };

                string sqlcmd = "select ID, Description from ShadebandDocLocation where Junk = 0 and ID = @ID";
                DataTable dt = new DataTable();
                DBProxy.Current.Select(null, sqlcmd, cmds, out dt);
                if (dt.Rows.Count == 0)
                {
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox($"Shadeband Location <{e.FormattedValue}> not Found");
                    return;
                }

                curDr["ShadebandDocLocationID"] = e.FormattedValue.ToString();
                curDr.EndEdit();
            };

            this.Helper.Controls.Grid.Generator(this.gridReceiving)
                 .CheckBox("select", header: string.Empty, trueValue: 1, falseValue: 0)
                 .Text("ExportID", header: "WK#", width: Widths.AnsiChars(15), iseditingreadonly: true)
                 .Numeric("Packages", header: "Packages", width: Widths.AnsiChars(3), decimal_places: 0, iseditingreadonly: true)
                 .Date("ArriveDate", header: "Arrive W/H \r\n Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Text("POID", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                 .Text("SEQ", header: "SEQ", width: Widths.AnsiChars(8), iseditingreadonly: true)                 
                 .Text("WeaveTypeID", header: "Weave\r\nType", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Text("Roll", header: "Roll#", width: Widths.AnsiChars(5), iseditingreadonly: true)
                 .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true)
                 .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(5), decimal_places: 0, iseditingreadonly: true)
                 .Text("Refno", header: "Ref#", width: Widths.AnsiChars(15), iseditingreadonly: true)
                 .Text("ColorID", header: "Color \r\nName", width: Widths.AnsiChars(6), iseditingreadonly: true)                 
                 .DateTime("CutTime", header: "Cut Shadeband Time", width: Widths.AnsiChars(20))
                 .DateTime("PasteTime", header: "Paste Shadeband Time", width: Widths.AnsiChars(20))
                 .DateTime("PassQATime", header: "Pass QA Time", width: Widths.AnsiChars(20))
                 .Text("ShadebandDocLocationID", header: "Shadeband Location", width: Widths.AnsiChars(10), settings: cellShadebandDocLocationID)
                 ;
            this.gridReceiving.Columns["CutTime"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridReceiving.Columns["PasteTime"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridReceiving.Columns["PassQATime"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridReceiving.Columns["ShadebandDocLocationID"].DefaultCellStyle.BackColor = Color.Pink;
        }

        /// <summary>
        /// ShadebandDocLocationID Pop
        /// </summary>
        /// <param name="rowIndex"></param>
        private void GridShadebandDocLocationIDCellPop(int rowIndex)
        {
            DataRow curDr = this.gridReceiving.GetDataRow(rowIndex);
            string sqlcmd = "select ID, Description from ShadebandDocLocation where Junk = 0";
            DataTable dt = new DataTable();
            DBProxy.Current.Select(null, sqlcmd, out dt);
            SelectItem selectItem = new SelectItem(dt, "ID,Description", "10,25", "ID,Description")
            {
                Width = 800,
            };

            selectItem.ShowDialog();
            if (selectItem.DialogResult == DialogResult.Cancel)
            {
                return;
            }

            DataRow dr = selectItem.GetSelecteds().FirstOrDefault();
            curDr["ShadebandDocLocationID"] = MyUtility.Convert.GetString(dr["ID"]);
            curDr.EndEdit();
        }

        /// <inheritdoc/>
        private void BtnQuery_Click(object sender, EventArgs e)
        {
            this.Query();
        }

        /// <summary>
        /// Query
        /// </summary>
        private void Query()
        {
            string sqlWhere = "where fb.WeaveTypeID in ('KNIT','WOVEN') " + Environment.NewLine;
            string sqlWhere2 = "where fb.WeaveTypeID in ('KNIT','WOVEN') " + Environment.NewLine;

            if (!MyUtility.Check.Empty(this.txtRecivingID.Text))
            {
                sqlWhere += $" and r.ID = '{this.txtRecivingID.Text}'" + Environment.NewLine;
                sqlWhere2 += $" and t.ID = '{this.txtRecivingID.Text}'" + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(this.dateBoxArriveWH.Value1))
            {
                sqlWhere += $" and r.WhseArrival >= '{Convert.ToDateTime(this.dateBoxArriveWH.Value1).ToString("yyyy/MM/dd")}'" + Environment.NewLine;
                sqlWhere2 += $" and t.IssueDate >= '{Convert.ToDateTime(this.dateBoxArriveWH.Value1).ToString("yyyy/MM/dd")}'" + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(this.dateBoxArriveWH.Value2))
            {
                sqlWhere += $" and r.WhseArrival <= '{Convert.ToDateTime(this.dateBoxArriveWH.Value2).ToString("yyyy/MM/dd")}'" + Environment.NewLine;
                sqlWhere2 += $" and t.IssueDate <= '{Convert.ToDateTime(this.dateBoxArriveWH.Value2).ToString("yyyy/MM/dd")}'" + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(this.txtWK.Text))
            {
                sqlWhere += $" and r.ExportID = '{this.txtWK.Text}'" + Environment.NewLine;
                sqlWhere2 += $" and 1=0 " + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(this.txtRef.Text))
            {
                sqlWhere += $" and psd.refno = '{this.txtRef.Text}'" + Environment.NewLine;
                sqlWhere2 += $" and psd.refno = '{this.txtRef.Text}'" + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(this.txtColor.Text))
            {
                sqlWhere += $" and psd.ColorID = '{this.txtColor.Text}'" + Environment.NewLine;
                sqlWhere2 += $" and psd.ColorID = '{this.txtColor.Text}'" + Environment.NewLine;
            }

            if (!this.txtSeq.CheckSeq1Empty() && this.txtSeq.CheckSeq2Empty())
            {
                sqlWhere += $" and rd.seq1 = '{this.txtSeq.Seq1}'";
                sqlWhere2 += $" and td.seq1 = '{this.txtSeq.Seq1}'";
            }
            else if (!this.txtSeq.CheckEmpty(showErrMsg: false))
            {
                sqlWhere += $" and rd.seq1 = '{this.txtSeq.Seq1}' and rd.seq2 = '{this.txtSeq.Seq2}'";
                sqlWhere2 += $" and td.seq1 = '{this.txtSeq.Seq1}' and td.seq2 = '{this.txtSeq.Seq2}'";
            }

            if (!MyUtility.Check.Empty(this.txtSP.Text))
            {
                sqlWhere += $" and rd.POID like '%{this.txtSP.Text}%'" + Environment.NewLine;
                sqlWhere2 += $" and td.POID like '%{this.txtSP.Text}%'" + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(this.txtRoll.Text))
            {
                sqlWhere += $" and rd.roll like '%{this.txtRoll.Text}%'" + Environment.NewLine;
                sqlWhere2 += $" and td.roll like '%{this.txtRoll.Text}%'" + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(this.txtDyelot.Text))
            {
                sqlWhere += $" and rd.dyelot like '%{this.txtDyelot.Text}%'" + Environment.NewLine;
                sqlWhere2 += $" and td.dyelot like '%{this.txtDyelot.Text}%'" + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(this.txtPackage.Text))
            {
                sqlWhere += $" and e.Packages = '{this.txtPackage.Text}'" + Environment.NewLine;
                sqlWhere2 += $" and 1=0" + Environment.NewLine;
            }

            string sqlCmd = $@"
select 
	[select] = cast(0 as bit)
	, r.ExportID
	, e.Packages
	, [ArriveDate] = r.WhseArrival
	, f.POID
    , [SEQ] = CONCAT(f.SEQ1, ' ', f.SEQ2)
	, fb.WeaveTypeID
	, fs.Roll
	, fs.Dyelot
	, psd.Refno
	, psd.ColorID
	, [Qty] = rd.ActualQty
	, fs.CutTime
	, fs.PasteTime
	, fs.PassQATime
	, fs.ShadebandDocLocationID
	, fs.ID
from  Receiving r with (nolock)
inner join Receiving_Detail rd with (nolock) on r.ID = rd.ID
inner join PO_Supp_Detail psd with (nolock) on rd.PoId = psd.ID and rd.Seq1 = psd.SEQ1 and rd.Seq2 = psd.SEQ2
inner join Fabric fb with (nolock) on psd.SCIRefno = fb.SCIRefno
inner join FIR f with (nolock) on r.id = f.ReceivingID and rd.PoId = F.POID and rd.Seq1 = F.SEQ1 and rd.Seq2 = F.SEQ2
inner join FIR_Shadebone fs with (nolock) on f.id = fs.ID and rd.Roll = fs.Roll and rd.Dyelot = fs.Dyelot
inner join Export e with (nolock) on r.ExportId = e.ID
{sqlWhere}
UNION all
select 
	[select] = cast(0 as bit)
	, [ExportID] = ''
	, [Packages] = 0
	, [ArriveDate] = t.IssueDate
	, f.POID
    , [SEQ] = CONCAT(f.SEQ1, ' ', f.SEQ2)
	, fb.WeaveTypeID
	, fs.Roll
	, fs.Dyelot
	, psd.Refno
	, psd.ColorID
	, [Qty] = td.Qty
	, fs.CutTime
	, fs.PasteTime
	, fs.PassQATime
	, fs.ShadebandDocLocationID
	, fs.ID
FROM TransferIn t with (nolock)
INNER JOIN TransferIn_Detail td with (nolock) ON t.ID = td.ID
INNER JOIN PO_Supp_Detail psd with (nolock) on td.PoId = psd.ID and td.Seq1 = psd.SEQ1 and td.Seq2 = psd.SEQ2
INNER JOIN Fabric fb with (nolock) on psd.SCIRefno = fb.SCIRefno
inner join FIR f with (nolock) on t.id = f.ReceivingID and td.PoId = F.POID and td.Seq1 = F.SEQ1 and td.Seq2 = F.SEQ2
inner join FIR_Shadebone fs with (nolock) on f.id = fs.ID and td.Roll = fs.Roll and td.Dyelot = fs.Dyelot
{sqlWhere2}
";

            DataTable dt = new DataTable();
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            if (dt.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found");
                this.listControlBindingSource1.DataSource = dt;
                return;
            }

            this.listControlBindingSource1.DataSource = dt;
        }

        /// <inheritdoc/>
        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
            if (dt == null)
            {
                return;
            }

            var selectedListDataRow = dt.AsEnumerable().Where(s => MyUtility.Convert.GetBool(s["select"]));
            if (!selectedListDataRow.Any())
            {
                MyUtility.Msg.WarningBox("Please select rows first!");
                return;
            }

            string sqlcmd = @"
update fs
	set fs.CutTime = t.CutTime
	, fs.PasteTime = t.PasteTime
	, fs.PassQATime = t.PassQATime
	, fs.ShadebandDocLocationID = t.ShadebandDocLocationID
from FIR_Shadebone fs
inner join #tmp t on t.id = fs.ID and t.Roll = fs.Roll and t.Dyelot = fs.Dyelot
";

            TransactionScope _transactionscope = new TransactionScope();
            Exception errMsg = null;
            DataTable dtUpdate;
            using (_transactionscope)
            {
                try
                {
                    DualResult result;
                    if (!MyUtility.Check.Empty(sqlcmd))
                    {
                        result = MyUtility.Tool.ProcessWithDatatable(selectedListDataRow.CopyToDataTable(), "ID,Roll,Dyelot,CutTime,PasteTime,PassQATime,ShadebandDocLocationID", sqlcmd, out dtUpdate, temptablename: "#tmp");
                        if (!result)
                        {
                            throw result.GetException();
                        }
                    }

                    _transactionscope.Complete();
                }
                catch (Exception ex)
                {
                    errMsg = ex;
                }
            }

            if (!MyUtility.Check.Empty(errMsg))
            {
                this.ShowErr(errMsg);
                return;
            }

            // 將當前所選位置記錄起來後, 待資料重整後定位回去!
            int currentRowIndexInt = this.gridReceiving.CurrentRow.Index;
            int currentColumnIndexInt = this.gridReceiving.CurrentCell.ColumnIndex;
            this.Query();
            this.gridReceiving.CurrentCell = this.gridReceiving[currentColumnIndexInt, currentRowIndexInt];
            this.gridReceiving.FirstDisplayedScrollingRowIndex = currentRowIndexInt;
            MyUtility.Msg.InfoBox("Complete");
        }

        /// <inheritdoc/>
        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 更新勾選欄位
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void BtnUpdateTime_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
            if (dt == null)
            {
                return;
            }

            var selectedListDataRow = dt.AsEnumerable().Where(s => MyUtility.Convert.GetBool(s["select"]));
            if (selectedListDataRow.Any())
            {
                string updateTime = MyUtility.Convert.GetDate(this.dateTimePickerUpdateTime.Text).HasValue ?
                        MyUtility.Convert.GetDate(this.dateTimePickerUpdateTime.Text).Value.ToString("yyyy/MM/dd HH:mm:ss") :
                        string.Empty;
                string comboUpdateTime = this.comboBoxUpdateTime.SelectedValue.ToString();
                foreach (DataRow dr in selectedListDataRow.ToList())
                {
                    dr[comboUpdateTime] = updateTime;
                }
            }
        }
    }
}
