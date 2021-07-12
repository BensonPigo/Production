using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P53 : Sci.Win.Tems.QueryForm
    {
        private Color preparingColor = Color.FromArgb(255, 128, 0);
        private Color finishedColor = Color.Yellow;
        private DataTable dtData = new DataTable();
        private Ict.Win.UI.DataGridViewTextBoxColumn col_Remark;
        private Ict.Win.UI.DataGridViewMaskedTextBoxColumn col_IssueDate;
        private Ict.Win.UI.DataGridViewNumericBoxColumn col_ttlRol;

        /// <inheritdoc/>
        public P53(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.EditMode = true;
            MyUtility.Tool.SetupCombox(this.comboStatus, 2, 1, @",All,Preparing,Preparing,Finished,Finished");
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.displayFinished.BackColor = this.finishedColor;
            this.displayPreparing.BackColor = this.preparingColor;
            this.gridDetail.IsEditable = true;
            this.gridDetail.IsEditingReadOnly = false;

            #region Grid Settings
            /* ISP20210372 移除Location,Worker的開窗跟檢驗事件
            Ict.Win.DataGridViewGeneratorTextColumnSettings col_Worker = new DataGridViewGeneratorTextColumnSettings();
            col_Worker.EditingMouseDown += (s, e) =>
            {
                if (this.detailbs == null || e.RowIndex == -1)
                {
                    return;
                }

                if (e.Button == MouseButtons.Right)
                {
                    DataRow dr = this.gridDetail.GetDataRow(e.RowIndex);
                    string sqlcmd = $@"select ID,Name,ExtNo,Factory from Pass1";
                    SelectItem item = new SelectItem(sqlcmd, "10,15,6,6", dr["Worker"].ToString());
                    DialogResult drResult = item.ShowDialog();
                    if (drResult == DialogResult.Cancel)
                    {
                        return;
                    }

                    dr["Worker"] = item.GetSelecteds()[0]["ID"].ToString().TrimEnd();
                    dr.EndEdit();
                }
            };

            col_Worker.CellValidating += (s, e) =>
            {
                if (this.detailbs == null || e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.gridDetail.GetDataRow(e.RowIndex);
                string oldvalue = dr["Worker"].ToString();
                string newvalue = e.FormattedValue.ToString();

                if (oldvalue.Equals(newvalue))
                {
                    return;
                }

                string sqlcmd = $@"select ID,Name,ExtNo,Factory from Pass1 where id = '{newvalue}'";
                if (MyUtility.Check.Seek(sqlcmd, out DataRow drData))
                {
                    dr["Worker"] = newvalue;
                    dr.EndEdit();
                }
                else
                {
                    dr["Worker"] = string.Empty;
                    dr.EndEdit();
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("<Worker: {0}> doesn't exist in Data!", e.FormattedValue));
                    return;
                }
            };

            Ict.Win.DataGridViewGeneratorTextColumnSettings col_Location = new DataGridViewGeneratorTextColumnSettings();
            col_Location.EditingMouseDown += (s, e) =>
            {
                if (this.detailbs == null || e.RowIndex == -1)
                {
                    return;
                }

                if (e.Button == MouseButtons.Right)
                {
                    DataRow dr = this.gridDetail.GetDataRow(e.RowIndex);
                    string sqlcmd = $@"select ID,StockType,Description from dbo.MtlLocation where Junk = 0 and StockType='B'";
                    SelectItem2 item = new SelectItem2(sqlcmd, string.Empty, "14,7,15", dr["Location"].ToString(), null, null, null);
                    item.Size = new System.Drawing.Size(550, 666);
                    DialogResult drResult = item.ShowDialog();
                    if (drResult == DialogResult.Cancel)
                    {
                        return;
                    }

                    dr["Location"] = item.GetSelectedString();
                    dr.EndEdit();
                }
            };
            */

            Ict.Win.DataGridViewGeneratorMaskedTextColumnSettings mask_IssueDate = new DataGridViewGeneratorMaskedTextColumnSettings();
            mask_IssueDate.CellValidating += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                if (MyUtility.Check.Empty(e.FormattedValue))
                {
                    return;
                }

                DataRow dr = this.gridDetail.GetDataRow(e.RowIndex);

                string strStartDate = this.DateTimeMaskFull(e.FormattedValue.ToString().Replace(" ", "0"));
                if (this.IsDateTimeFormat(strStartDate))
                {
                    dr["IssueDate"] = this.DateTimeMaskFull(e.FormattedValue.ToString());
                    dr.EndEdit();
                }
                else
                {
                    e.Cancel = true;
                }
            };
            Ict.Win.DataGridViewGeneratorMaskedTextColumnSettings mask_StartDate = new DataGridViewGeneratorMaskedTextColumnSettings();
            mask_StartDate.CellValidating += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.gridDetail.GetDataRow(e.RowIndex);

                if (MyUtility.Check.Empty(e.FormattedValue))
                {
                    if (!MyUtility.Check.Empty(dr["FinishDate"]))
                    {
                        MyUtility.Msg.WarningBox("Start Date cannot be empty when Finish Date already exsis.");
                        e.Cancel = true;
                        return;
                    }
                    else
                    {
                        return;
                    }
                }

                string strStartDate = this.DateTimeMaskFull(e.FormattedValue.ToString().Replace(" ", "0"));
                if (this.IsDateTimeFormat(strStartDate))
                {
                    if (!MyUtility.Check.Empty(dr["FinishDate"]))
                    {
                        string strEndDate = this.DateTimeMaskFull(dr["FinishDate"].ToString());
                        if (DateTime.Compare(Convert.ToDateTime(strStartDate), Convert.ToDateTime(strEndDate)) == 1)
                        {
                            MyUtility.Msg.WarningBox("Start Date cannot later than Finish Date.");
                            e.Cancel = true;
                        }
                    }

                    dr["StartDate"] = e.FormattedValue.ToString().PadRight(12, '0');

                    // 直接計算PreparingTime
                    DataRow drPre = this.GetPreparTime(dr);
                    dr["PreparingTime"] = drPre["PreparingTime"];
                    dr["LeadTime"] = drPre["LeadTime"];
                    dr.EndEdit();
                }
                else
                {
                    e.Cancel = true;
                }
            };

            Ict.Win.DataGridViewGeneratorMaskedTextColumnSettings mask_EndDate = new DataGridViewGeneratorMaskedTextColumnSettings();
            mask_EndDate.CellValidating += (s, e) =>
            {
                if (e.RowIndex == -1 || MyUtility.Check.Empty(e.FormattedValue))
                {
                    return;
                }

                DataRow dr = this.gridDetail.GetDataRow(e.RowIndex);
                string strEndDate = this.DateTimeMaskFull(e.FormattedValue.ToString().Replace(" ", "0"));
                if (this.IsDateTimeFormat(strEndDate))
                {
                    if (MyUtility.Check.Empty(dr["StartDate"]))
                    {
                        MyUtility.Msg.WarningBox("Start Date cannot be empty.");
                        e.Cancel = true;
                    }

                    if (!MyUtility.Check.Empty(dr["StartDate"]))
                    {
                        string strStartDate = this.DateTimeMaskFull(dr["StartDate"].ToString());
                        if (DateTime.Compare(Convert.ToDateTime(strStartDate), Convert.ToDateTime(strEndDate)) == 1)
                        {
                            MyUtility.Msg.WarningBox("Start Date cannot later than Finish Date.");
                            e.Cancel = true;
                        }
                    }

                    dr["FinishDate"] = e.FormattedValue.ToString().PadRight(12, '0');

                    // 直接計算PreparingTime
                    DataRow drPre = this.GetPreparTime(dr);
                    dr["PreparingTime"] = drPre["PreparingTime"];
                    dr["LeadTime"] = drPre["LeadTime"];
                    dr.EndEdit();
                }
                else
                {
                    e.Cancel = true;
                }
            };
            #endregion
            this.gridDetail.DataSource = this.detailbs;
            this.Helper.Controls.Grid.Generator(this.gridDetail)
                .CheckBox("Select", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
                .Text("FactoryID", header: "Factory", width: Widths.AnsiChars(7), iseditingreadonly: true)
                .Text("RQNo", header: "RQ NO", width: Widths.AnsiChars(14), iseditingreadonly: true)
                .Text("SP", header: "SP", width: Widths.AnsiChars(14), iseditingreadonly: true)
                .Text("Department", header: "Department", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .DateTime("RequestDate", header: "Request Date", width: Widths.AnsiChars(18), iseditingreadonly: true)
                // .DateTime("IssueDate", header: "Issue Date", width: Widths.AnsiChars(18), iseditingreadonly: true).Get(out this.col_IssueDate)
                .MaskedText("IssueDate", "0000/00/00 00:00", header: "Issue Date", width: Widths.AnsiChars(18), iseditingreadonly: false, settings: mask_IssueDate).Get(out this.col_IssueDate)
                .Numeric("ttlRoll", header: "Total Roll", width: Widths.AnsiChars(8), iseditingreadonly: true).Get(out this.col_ttlRol)
                .Text("Remark", header: "Remark", width: Widths.AnsiChars(25), iseditingreadonly: true).Get(out this.col_Remark)
                .Text("Worker", header: "Worker", width: Widths.AnsiChars(11), iseditingreadonly: false)
                .Text("Location", header: "Location", width: Widths.AnsiChars(11), iseditingreadonly: false)
                .MaskedText("StartDate", "0000/00/00 00:00", header: "Start Date", width: Widths.AnsiChars(18), iseditingreadonly: false, settings: mask_StartDate)
                .MaskedText("FinishDate", "0000/00/00 00:00", header: "Finish Date", width: Widths.AnsiChars(18), iseditingreadonly: false, settings: mask_EndDate)
                .Text("PreparingTime", header: "PreparingTime", width: Widths.AnsiChars(11), iseditingreadonly: true)
                .Text("LeadTime", header: "LeadTime", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .CheckBox("Scan", header: "Scan", width: Widths.AnsiChars(4), iseditable: true, trueValue: 1, falseValue: 0)
                ;

            this.gridDetail.Columns[0].Frozen = true;
            this.gridDetail.Columns["Worker"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridDetail.Columns["Location"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridDetail.Columns["StartDate"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridDetail.Columns["FinishDate"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridDetail.Columns["Scan"].DefaultCellStyle.BackColor = Color.Pink;
            this.ChangeRowColor();
            this.gridDetail.RowEnter += this.Grid_RowEnter;
        }

        private void Grid_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            var data = ((DataRowView)this.gridDetail.Rows[e.RowIndex].DataBoundItem).Row;
            if (data == null)
            {
                return;
            }

            if (MyUtility.Check.Empty(data["RequestID"]))
            {
                this.col_IssueDate.IsEditingReadOnly = false;
                this.col_Remark.IsEditingReadOnly = false;
                this.col_ttlRol.IsEditingReadOnly = false;
            }
            else
            {
                this.col_IssueDate.IsEditingReadOnly = true;
                this.col_Remark.IsEditingReadOnly = true;
                this.col_ttlRol.IsEditingReadOnly = true;
            }
        }

        private string DateTimeMaskFull(string value)
        {
            string strReturn = string.Empty;
            if (value.Length == 12)
            {
                return strReturn = value.Substring(0, 4) + "/" + value.Substring(4, 2) + "/" + value.Substring(6, 2) + " " + value.Substring(8, 2) + ":" + value.Substring(10, 2);
            }
            else
            {
                strReturn = value.PadRight(12, '0');
                return strReturn = strReturn.Substring(0, 4) + "/" + strReturn.Substring(4, 2) + "/" + strReturn.Substring(6, 2) + " " + strReturn.Substring(8, 2) + ":" + strReturn.Substring(10, 2);
            }
        }

        private bool IsDateTimeFormat(string value)
        {
            try
            {
                DateTime.Parse(value);
                return true;
            }
            catch
            {
                MyUtility.Msg.WarningBox("Date format error");
                return false;
            }
        }

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            this.Query();
            this.ChangeRowColor();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            var upd_list = ((DataTable)this.detailbs.DataSource).AsEnumerable().Where(x => x["Select"].EqualDecimal(1)).ToList();
            string upd_sql = string.Empty;
            if (upd_list.Count == 0)
            {
                return;
            }

            foreach (DataRow dr in upd_list)
            {
                if (MyUtility.Check.Empty(dr["RequestID"]))
                {
                    string issueDate = MyUtility.Check.Empty(dr["IssueDate"]) ? "null" : (dr["IssueDate"].ToString().Length == 12) ? "'" + this.DateTimeMaskFull(dr["IssueDate"].ToString()) + "'" : "'" + dr["IssueDate"].ToString() + "'";
                    upd_sql += $@"
update Lack
set WHIssueDate = {issueDate}, TotalRoll = '{dr["ttlRoll"]}', WHRemark = '{dr["Remark"]}'

where id = '{dr["LackID"]}'";
                }

                string startDate = MyUtility.Check.Empty(dr["StartDate"]) ? "null" : "'" + this.DateTimeMaskFull(dr["StartDate"].ToString()) + "'";
                string endDate = MyUtility.Check.Empty(dr["FinishDate"]) ? "null" : "'" + this.DateTimeMaskFull(dr["FinishDate"].ToString()) + "'";
                int scan = MyUtility.Check.Empty(dr["Scan"]) ? 0 : 1;

                upd_sql += $@"
update Lack
set PreparedWorker = '{dr["Worker"]}' ,PreparedLocation = '{dr["Location"]}' ,PreparedStartDate = {startDate} ,PreparedFinishDate = {endDate}, ScanTransferSlip = {scan}
where id = '{dr["LackID"]}'" + Environment.NewLine;

            }

            DualResult result = DBProxy.Current.Execute(null, upd_sql);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            MyUtility.Msg.InfoBox("Save successfully!");
            this.Query();
            this.ChangeRowColor();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Query()
        {
            #region Sql command
            string sqlcmd = $@"

select 
[Select] = 0
,Lack.FactoryID
,[RQNo] = Lack.ID
,[SP] = Lack.OrderID
,[Department] = Lack.Dept
,[RequestDate] = Lack.ApvDate
,[IssueDate] = IIF(il.RequestID is null, format(Lack.WHIssueDate, 'yyyyMMddHHmm') ,format(iL.EditDate, 'yyyyMMddHHmm'))
,[ttlRoll] = IIF(il.RequestID is null, Lack.TotalRoll ,ttlRoll.value)
,[Remark] = IIF(il.RequestID is null, Lack.WHRemark ,iL.Remark)
,[Worker] = Lack.PreparedWorker
,[Location] = Lack.PreparedLocation
,[StartDate] = FORMAT(Lack.PreparedStartDate,'yyyyMMddHHmm')
,[FinishDate] = FORMAT(Lack.PreparedFinishDate,'yyyyMMddHHmm')
,[PreparingTime] = isnull(PreparingTime.value,0)
,[LeadTime]= case when (Lack.PreparedStartDate is null or Lack.PreparedFinishDate is null) then ''
				  when isnull(PreparingTime.ttlMinute,0) <= 420 then 'OK'
				  else 'Not OK' end
,[Scan] = lack.ScanTransferSlip
,[IssueLackID] = iL.Id
,[LackID] = Lack.ID
,Lack.MDivisionID
,il.RequestID
from Lack
left join IssueLack iL on Lack.ID = iL.RequestID
outer apply(
	select value = count(1)
	from (
		select distinct iL2.POID,iL2.Seq1,iL2.Seq2,iL2.Roll,iL2.Dyelot
		from IssueLack_Detail iL2
		where iL2.Id = il.Id
	) a
)ttlRoll
outer apply(
	select value = CONVERT(varchar,sum(minute)/1440) + ' '  -- day
	            + SUBSTRING(CONVERT(VARCHAR, DATEADD(MINUTE, sum(minute), 0), 108),1,5)  -- minute: second
	, ttlMinute = sum(minute)
	from dbo.GetPreparingTime(Lack.PreparedStartDate,Lack.PreparedFinishDate,Lack.MDivisionID)
)PreparingTime
where 1=1
and lack.Status in ('Confirmed','Received') and lack.FabricType  = 'F'
";
            #endregion

            #region Where 條件
            if (!MyUtility.Check.Empty(this.dateRequestDate.DateBox1.Value))
            {
                sqlcmd += $" and Convert(date, Lack.ApvDate) >= '{this.dateRequestDate.DateBox1.Text}'";
            }

            if (!MyUtility.Check.Empty(this.dateRequestDate.DateBox2.Value))
            {
                sqlcmd += $" and Convert(date, Lack.ApvDate) <= '{this.dateRequestDate.DateBox2.Text}'";
            }

            if (!MyUtility.Check.Empty(this.txtRequestNo.Text))
            {
                sqlcmd += $" and Lack.id = '{this.txtRequestNo.Text}'";
            }

            if (!MyUtility.Check.Empty(this.comboStatus.Text))
            {
                switch (this.comboStatus.Text)
                {
                    case "Preparing":
                        sqlcmd += $" and (Lack.PreparedStartDate is not null and Lack.PreparedFinishDate is null)";
                        break;
                    case "Finished":
                        sqlcmd += $" and (Lack.PreparedStartDate is not null and Lack.PreparedFinishDate is not null)";
                        break;
                }
            }

            if (!MyUtility.Check.Empty(this.txtfactory.Text))
            {
                sqlcmd += $" and Lack.FactoryID = '{this.txtfactory.Text}'";
            }

            if (!MyUtility.Check.Empty(this.txtSPNo.Text))
            {
                sqlcmd += $" and Lack.OrderID = '{this.txtSPNo.Text}'";
            }

            sqlcmd += " order by Lack.Id,Lack.FactoryID,iL.ID";
            #endregion

            DualResult result = DBProxy.Current.Select(null, sqlcmd, out this.dtData);
            if (result)
            {
                if (this.dtData.Rows.Count == 0)
                {
                    MyUtility.Msg.InfoBox("Data not found!");
                }

                this.detailbs.DataSource = null;
                this.detailbs.DataSource = this.dtData;
            }
            else
            {
                this.ShowErr(result);
                return;
            }
        }

        private void ChangeRowColor()
        {
            DataTable tmp_dt = (DataTable)this.detailbs.DataSource;

            if (this.gridDetail.Rows.Count > 0 || tmp_dt != null)
            {
                for (int i = 0; i < this.gridDetail.Rows.Count; i++)
                {
                    DataRow dr = this.gridDetail.GetDataRow(i);
                    if (this.gridDetail.Rows.Count <= i || i < 0)
                    {
                        return;
                    }

                    if (!MyUtility.Check.Empty(dr["StartDate"]))
                    {
                        // Preparing
                        if (MyUtility.Check.Empty(dr["FinishDate"]))
                        {
                            this.gridDetail.Rows[i].DefaultCellStyle.BackColor = this.preparingColor;
                            this.gridDetail.Rows[i].DefaultCellStyle.SelectionBackColor = this.preparingColor;
                        }
                        else
                        {
                            // Finished
                            this.gridDetail.Rows[i].DefaultCellStyle.BackColor = this.finishedColor;
                            this.gridDetail.Rows[i].DefaultCellStyle.SelectionBackColor = this.finishedColor;
                        }
                    }
                    else if (MyUtility.Check.Empty(dr["RequestID"]))
                    {
                        this.gridDetail.Rows[i].Cells["IssueDate"].Style.BackColor = Color.Pink;
                        this.gridDetail.Rows[i].Cells["ttlRoll"].Style.BackColor = Color.Pink;
                        this.gridDetail.Rows[i].Cells["Remark"].Style.BackColor = Color.Pink;
                    }
                    else
                    {
                        this.gridDetail.Rows[i].Cells["IssueDate"].Style.BackColor = Color.White;
                        this.gridDetail.Rows[i].Cells["ttlRoll"].Style.BackColor = Color.White;
                        this.gridDetail.Rows[i].Cells["Remark"].Style.BackColor = Color.White;
                    }
                }
            }
        }

        private DataRow GetPreparTime(DataRow dr)
        {
            DataRow drReturn;
            string startTime = MyUtility.Check.Empty(dr["StartDate"]) ? "null" : "'" + this.DateTimeMaskFull(dr["StartDate"].ToString()) + "'";
            string endTime = MyUtility.Check.Empty(dr["FinishDate"]) ? "null" : "'" + this.DateTimeMaskFull(dr["FinishDate"].ToString()) + "'"; 

            string sqlcmd = $@"
select PreparingTime = isnull(CONVERT(varchar,sum(minute)/1440) + ' '  -- day
	            + SUBSTRING(CONVERT(VARCHAR, DATEADD(MINUTE, sum(minute), 0), 108),1,5),0)  -- minute: second
	, ttlMinute = isnull(sum(minute),0)
    , [LeadTime]= case when ({startTime} is null or {endTime} is null) then ''
				  when isnull(isnull(sum(minute),0),0) <= 420 then 'OK'
				  else 'Not OK' end
	from dbo.GetPreparingTime({startTime},{endTime},'{dr["MDivisionID"]}');
";
            if (MyUtility.Check.Seek(sqlcmd, out drReturn))
            {
                return drReturn;
            }
            else
            {
                return dr;
            }
        }

        private void GridDetail_Sorted(object sender, EventArgs e)
        {
            this.ChangeRowColor();
        }
    }
}
