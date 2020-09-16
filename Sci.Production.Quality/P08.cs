using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Quality
{
    /// <inheritdoc/>
    public partial class P08 : Win.Tems.QueryForm
    {
        /// <inheritdoc/>
        public P08(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.EditMode = true;

            Dictionary<string, string> comboBoxUpdateTime_RowSource = new Dictionary<string, string>
            {
                { string.Empty, string.Empty },
                { "CutTime", "Cut Shadeband Time" },
                { "PasteTime", "Paste Shadeband Time" },
                { "PassQATime", "Pass QA Time" },
            };
            this.comboBoxTimeSearch.DataSource = new BindingSource(comboBoxUpdateTime_RowSource, null);
            this.comboBoxTimeSearch.ValueMember = "Key";
            this.comboBoxTimeSearch.DisplayMember = "Value";
            this.comboBoxTimeSearch.SelectedValue = string.Empty;

            this.comboBoxUpdateTime.DataSource = new BindingSource(comboBoxUpdateTime_RowSource, null);
            this.comboBoxUpdateTime.ValueMember = "Key";
            this.comboBoxUpdateTime.DisplayMember = "Value";
            this.comboBoxUpdateTime.SelectedValue = "CutTime";
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.InitDataTimePicker(this.dateTimePickerUpdateTime);
            this.InitDataTimePicker(this.dateTimePickerTimeSearch1);
            this.InitDataTimePicker(this.dateTimePickerTimeSearch2);
            this.GridSetup();
        }

        private void InitDataTimePicker(DateTimePicker timePicker)
        {
            timePicker.CustomFormat = "yyyy/MM/dd HH:mm:ss";
            timePicker.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        }

        /// <inheritdoc/>
        private void GridSetup()
        {
            DataGridViewGeneratorCheckBoxColumnSettings col_Select = new DataGridViewGeneratorCheckBoxColumnSettings();
            DataGridViewGeneratorTextColumnSettings cellShadebandDocLocationID = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings cellShadeBand = new DataGridViewGeneratorTextColumnSettings();

            col_Select.CellValidating += (s, e) =>
            {
                DataRow dr = this.gridReceiving.GetDataRow(e.RowIndex);
                bool isCheck = MyUtility.Convert.GetBool(e.FormattedValue);
                dr["select"] = isCheck;
                dr.EndEdit();
                this.CalPrintPage();
            };

            cellShadebandDocLocationID.CellMouseDoubleClick += (s, e) =>
            {
                this.GridShadebandDocLocationIDCellPop(e.RowIndex, null);
            };

            cellShadebandDocLocationID.EditingMouseUp += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    this.GridShadebandDocLocationIDCellPop(e.RowIndex, null);
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

                if (!this.ShadeandLocation_Validating(e.FormattedValue.ToString()))
                {
                    e.Cancel = true;
                    return;
                }

                curDr["select"] = 1;
                curDr["ShadebandDocLocationID"] = e.FormattedValue;
                curDr.EndEdit();
            };

            cellShadeBand.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.gridReceiving.GetDataRow(e.RowIndex);
                if (dr == null)
                {
                    return;
                }

                this.ShadeBond(dr["FIRID"].ToString());
            };

            this.gridReceiving.CellValueChanged += (s, e) =>
            {
                DataRow dr = this.gridReceiving.GetDataRow(e.RowIndex);
                this.SelectModify(dr);

                DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
                if (e.RowIndex == dt.Rows.Count - 1)
                {
                    this.CalPrintPage();
                }
            };

            this.Helper.Controls.Grid.Generator(this.gridReceiving)
                 .CheckBox("select", header: string.Empty, trueValue: 1, falseValue: 0, settings: col_Select)
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
                 .Text("ColorName", header: "Color Name", width: Widths.AnsiChars(12), iseditingreadonly: true)
                 .DateTime("CutTime", header: "Cut Shadeband Time", width: Widths.AnsiChars(20))
                 .DateTime("PasteTime", header: "Paste Shadeband Time", width: Widths.AnsiChars(20))
                 .DateTime("PassQATime", header: "Pass QA Time", width: Widths.AnsiChars(20))
                 .Text("ShadebandDocLocationID", header: "Shadeband Location", width: Widths.AnsiChars(10), settings: cellShadebandDocLocationID)
                 .Text("ShadeBond", header: "shade \r\n Band", width: Widths.AnsiChars(6), iseditingreadonly: true, settings: cellShadeBand)
                 .Date("ShadeBondDate", header: "Last Shade \r\n Test Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Text("Remark", header: "Remark", width: Widths.AnsiChars(15))
                 ;
            this.gridReceiving.Columns["CutTime"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridReceiving.Columns["PasteTime"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridReceiving.Columns["PassQATime"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridReceiving.Columns["ShadebandDocLocationID"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridReceiving.Columns["Remark"].DefaultCellStyle.BackColor = Color.Pink;
        }

        private void ShadeBond(string firID)
        {
            if (firID.Empty())
            {
                return;
            }

            DataTable dt = this.QueryFir(firID);

            if (dt.Rows[0] == null)
            {
                return;
            }

            var frm = new P01_ShadeBond(false, dt.Rows[0]["ID"].ToString(), null, null, dt.Rows[0]);
            frm.ShowDialog(this);
            frm.Dispose();
            this.Query();
        }

        private void SelectModify(DataRow dr)
        {
            if (!Equals(dr["CutTime"], dr["oldCutTime"]) ||
                !Equals(dr["PasteTime"], dr["oldPasteTime"]) ||
                !Equals(dr["PassQATime"], dr["oldPassQATime"]) ||
                !Equals(dr["ShadebandDocLocationID"], dr["oldShadebandDocLocationID"]) ||
                !Equals(dr["Remark"], dr["oldRemark"]))
            {
                dr["select"] = 1;
                dr.EndEdit();
            }
        }

        /// <summary>
        /// 計算列印頁數 4筆一頁
        /// </summary>
        private void CalPrintPage()
        {
            DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
            if (dt != null || dt.Rows.Count > 0)
            {
                int cnt = MyUtility.Convert.GetInt(dt.Compute("count(select)", "select = 1")); // + (isCheck ? 1 : -1);
                cnt = cnt == 0 ? 0 : (cnt % 4) == 0 ? cnt / 4 : (cnt / 4) + 1;
                this.numericTotalPage.Value = cnt;
            }
        }

        private void TxtShadeandLocation_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Win.UI.TextBox txtLocation = (Win.UI.TextBox)sender;
            this.GridShadebandDocLocationIDCellPop(-2, txtLocation);
        }

        private void TxtShadeandLocation_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Win.UI.TextBox txtLocation = (Win.UI.TextBox)sender;
            if (txtLocation.Text.Empty())
            {
                return;
            }

            if (!this.ShadeandLocation_Validating(txtLocation.Text))
            {
                e.Cancel = true;
                return;
            }
        }

        private void GridShadebandDocLocationIDCellPop(int rowIndex, Win.UI.TextBox txtLocation)
        {
            string sqlcmd = "select ID, Description from ShadebandDocLocation where Junk = 0";
            DBProxy.Current.Select(null, sqlcmd, out DataTable dt);
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
            if (rowIndex > -1)
            {
                DataRow curDr = this.gridReceiving.GetDataRow(rowIndex);
                curDr["ShadebandDocLocationID"] = MyUtility.Convert.GetString(dr["ID"]);
                curDr.EndEdit();
            }
            else
            {
                txtLocation.Text = MyUtility.Convert.GetString(dr["ID"]);
            }
        }

        private bool ShadeandLocation_Validating(string eText)
        {
            List<SqlParameter> cmds = new List<SqlParameter>()
            {
                new SqlParameter("@ID", eText),
            };

            string sqlcmd = "select ID, Description from ShadebandDocLocation where Junk = 0 and ID = @ID";
            DBProxy.Current.Select(null, sqlcmd, cmds, out DataTable dt);
            if (dt.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox($"Shadeband Location <{eText}> not Found");
                return false;
            }

            return true;
        }

        /// <inheritdoc/>
        private void BtnQuery_Click(object sender, EventArgs e)
        {
            this.Query();
            this.InitDataTimePicker(this.dateTimePickerUpdateTime);
        }

        /// <summary>
        /// Query
        /// </summary>
        private void Query()
        {
            string sqlWhere = "where fb.WeaveTypeID in ('KNIT','WOVEN') and o.category in ('B','M','T') " + Environment.NewLine;
            string sqlWhere2 = "where fb.WeaveTypeID in ('KNIT','WOVEN') and o.category in ('B','M','T') " + Environment.NewLine;

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

            if (!MyUtility.Check.Empty(this.txtLocationSearch.Text))
            {
                sqlWhere += $" and fs.ShadebandDocLocationID = '{this.txtLocationSearch.Text}'" + Environment.NewLine;
                sqlWhere2 += $" and fs.ShadebandDocLocationID = '{this.txtLocationSearch.Text}'" + Environment.NewLine;
            }

            switch (this.comboBoxTimeSearch.SelectedValue.ToString().ToUpper())
            {
                case "CUTTIME":
                    sqlWhere += $" and fs.CutTime >= '{this.dateTimePickerTimeSearch1.Text}' and fs.CutTime <= '{this.dateTimePickerTimeSearch2.Text}'" + Environment.NewLine;
                    sqlWhere2 += $" and fs.CutTime >= '{this.dateTimePickerTimeSearch1.Text}' and fs.CutTime <= '{this.dateTimePickerTimeSearch2.Text}'" + Environment.NewLine;
                    break;
                case "PASTETIME":
                    sqlWhere += $" and fs.PasteTime >= '{this.dateTimePickerTimeSearch1.Text}' and fs.PasteTime <= '{this.dateTimePickerTimeSearch2.Text}'" + Environment.NewLine;
                    sqlWhere2 += $" and fs.PasteTime >= '{this.dateTimePickerTimeSearch1.Text}' and fs.PasteTime <= '{this.dateTimePickerTimeSearch2.Text}'" + Environment.NewLine;
                    break;
                case "PASSQATIME":
                    sqlWhere += $" and fs.PassQATime >= '{this.dateTimePickerTimeSearch1.Text}' and fs.PassQATime <= '{this.dateTimePickerTimeSearch2.Text}'" + Environment.NewLine;
                    sqlWhere2 += $" and fs.PassQATime >= '{this.dateTimePickerTimeSearch1.Text}' and fs.PassQATime <= '{this.dateTimePickerTimeSearch2.Text}'" + Environment.NewLine;
                    break;
            }

            string sqlCmd = $@"
select 
	[select] = cast(0 as bit)
	, r.ExportID
	, [Packages] = e.[Packages]
	, [ArriveDate] = r.WhseArrival
	, f.POID
    , [SEQ] = CONCAT(f.SEQ1, ' ', f.SEQ2)
	, fb.WeaveTypeID
	, fs.Roll
	, fs.Dyelot
	, psd.Refno
	, [ColorName] = c.name
	, [Qty] = rd.StockQty
	, fs.CutTime
	, fs.PasteTime
	, fs.PassQATime
	, fs.ShadebandDocLocationID
	, fs.ID
    , f.ShadeBond
    , f.ShadeBondDate
    , fs.Remark
    , [oldCutTime] = fs.CutTime
    , [oldPasteTime] = fs.PasteTime
    , [oldPassQATime] = fs.PassQATime
    , [oldShadebandDocLocationID] = fs.ShadebandDocLocationID
    , [oldRemark] = fs.Remark
    , [FIRID] = f.ID
from  Receiving r with (nolock)
inner join Receiving_Detail rd with (nolock) on r.ID = rd.ID
inner join PO_Supp_Detail psd with (nolock) on rd.PoId = psd.ID and rd.Seq1 = psd.SEQ1 and rd.Seq2 = psd.SEQ2
inner join Fabric fb with (nolock) on psd.SCIRefno = fb.SCIRefno
inner join FIR f with (nolock) on r.id = f.ReceivingID and rd.PoId = F.POID and rd.Seq1 = F.SEQ1 and rd.Seq2 = F.SEQ2
inner join FIR_Shadebone fs with (nolock) on f.id = fs.ID and rd.Roll = fs.Roll and rd.Dyelot = fs.Dyelot
inner join Orders o with (nolock) on o.ID = rd.PoId
left join Color c with (nolock) on psd.ColorID = c.ID and psd.BrandId = c.BrandID
outer apply (
    select [Packages] = sum(e.Packages)
    from Export e with (nolock) 
    where e.Blno in (
        select distinct e2.BLNO
        from Export e2 with (nolock) 
        where r.ExportId = e2.ID
    )
)e
{sqlWhere}
UNION all
select 
	[select] = cast(0 as bit)
	, [ExportID] = ''
	, [Packages] = t.Packages
	, [ArriveDate] = t.IssueDate
	, f.POID
    , [SEQ] = CONCAT(f.SEQ1, ' ', f.SEQ2)
	, fb.WeaveTypeID
	, fs.Roll
	, fs.Dyelot
	, psd.Refno
    , [ColorName] = c.name
	, [Qty] = td.Qty
	, fs.CutTime
	, fs.PasteTime
	, fs.PassQATime
	, fs.ShadebandDocLocationID
	, fs.ID
    , f.ShadeBond
    , f.ShadeBondDate
    , fs.Remark
    , [oldCutTime] = fs.CutTime
    , [oldPasteTime] = fs.PasteTime
    , [oldPassQATime] = fs.PassQATime
    , [oldShadebandDocLocationID] = fs.ShadebandDocLocationID
    , [oldRemark] = fs.Remark
    , [FIRID] = f.ID
FROM TransferIn t with (nolock)
INNER JOIN TransferIn_Detail td with (nolock) ON t.ID = td.ID
INNER JOIN PO_Supp_Detail psd with (nolock) on td.PoId = psd.ID and td.Seq1 = psd.SEQ1 and td.Seq2 = psd.SEQ2
INNER JOIN Fabric fb with (nolock) on psd.SCIRefno = fb.SCIRefno
INNER JOIN FIR f with (nolock) on t.id = f.ReceivingID and td.PoId = F.POID and td.Seq1 = F.SEQ1 and td.Seq2 = F.SEQ2
INNER JOIN FIR_Shadebone fs with (nolock) on f.id = fs.ID and td.Roll = fs.Roll and td.Dyelot = fs.Dyelot
INNER JOIN Orders o with (nolock) on o.ID = td.PoId
left join Color c with (nolock) on psd.ColorID = c.ID and psd.BrandId = c.BrandID
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

        private DataTable QueryFir(string id)
        {
            string sqlCmd =
                $@"select f.*
	                ,r.whseArrival
	                ,r.Exportid
                from FIR f
                Left join Receiving r WITH (NOLOCK) on r.id = f.receivingid
                where f.id = {id} ";
            DataTable dt = new DataTable();
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out dt);
            return dt;
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
    , fs.Remark = t.Remark
from FIR_Shadebone fs
inner join #tmp t on t.id = fs.ID and t.Roll = fs.Roll and t.Dyelot = fs.Dyelot
";

            TransactionScope transactionscope = new TransactionScope();
            Exception errMsg = null;
            using (transactionscope)
            {
                try
                {
                    DualResult result;
                    if (!MyUtility.Check.Empty(sqlcmd))
                    {
                        result = MyUtility.Tool.ProcessWithDatatable(selectedListDataRow.CopyToDataTable(), "ID,Roll,Dyelot,CutTime,PasteTime,PassQATime,ShadebandDocLocationID,Remark", sqlcmd, out DataTable dtUpdate, temptablename: "#tmp");
                        if (!result)
                        {
                            throw result.GetException();
                        }
                    }

                    transactionscope.Complete();
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
                    if (!comboUpdateTime.Empty())
                    {
                        dr[comboUpdateTime] = updateTime;
                    }

                    if (!MyUtility.Check.Empty(this.txtShadeandLocation.Text))
                    {
                        dr["ShadebandDocLocationID"] = this.txtShadeandLocation.Text;
                    }
                }
            }
        }
    }
}
