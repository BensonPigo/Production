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
            this.gridDetail.DataSource = this.detailbs;
            this.gridDetail.IsEditable = true;
            this.gridDetail.IsEditingReadOnly = false;

            #region Grid Settings

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
                    string sqlcmd = $@"select ID,StockType,Description from dbo.MtlLocation where Junk = 0";
                    SelectItem2 item = new SelectItem2(sqlcmd, string.Empty, "12,7,15", string.Empty, null, null, null);
                    item.Size = new System.Drawing.Size(810, 666);
                    DialogResult drResult = item.ShowDialog();
                    if (drResult == DialogResult.Cancel)
                    {
                        return;
                    }

                    dr["Location"] = item.GetSelectedString();
                    dr.EndEdit();
                }
            };

            Ict.Win.DataGridViewGeneratorDateColumnSettings col_StartDate = new DataGridViewGeneratorDateColumnSettings();
            col_StartDate.CellValidating += (s, e) =>
            {
                if (this.detailbs == null || e.RowIndex == -1 || MyUtility.Check.Empty(e.FormattedValue))
                {
                    return;
                }

                DataRow dr = this.gridDetail.GetDataRow(e.RowIndex);
                if (!MyUtility.Check.Empty((DateTime)e.FormattedValue) && !MyUtility.Check.Empty(dr["FinishDate"]))
                {
                    if (DateTime.Compare((DateTime)e.FormattedValue, (DateTime)dr["FinishDate"]) == 1)
                    {
                        MyUtility.Msg.WarningBox("Start Date cannot later than Finish Date.");
                        e.Cancel = true;
                        return;
                    }
                }

                dr["StartDate"] = e.FormattedValue;
                dr.EndEdit();
            };

            Ict.Win.DataGridViewGeneratorDateColumnSettings col_FinishDate = new DataGridViewGeneratorDateColumnSettings();
            col_FinishDate.CellValidating += (s, e) =>
            {
                if (this.detailbs == null || e.RowIndex == -1 || MyUtility.Check.Empty(e.FormattedValue))
                {
                    return;
                }

                DataRow dr = this.gridDetail.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(dr["StartDate"]) && !MyUtility.Check.Empty((DateTime)e.FormattedValue))
                {
                    MyUtility.Msg.WarningBox("Start Date cannot be empty.");
                    e.Cancel = true;
                    return;
                }

                if (!MyUtility.Check.Empty(dr["StartDate"]) && !MyUtility.Check.Empty((DateTime)e.FormattedValue))
                {
                    if (DateTime.Compare((DateTime)dr["StartDate"], (DateTime)e.FormattedValue) == 1)
                    {
                        MyUtility.Msg.WarningBox("Start Date cannot later than Finish Date.");
                        e.Cancel = true;
                        return;
                    }
                }

                dr["FinishDate"] = e.FormattedValue;
                dr.EndEdit();
            };

            Ict.Win.DataGridViewGeneratorMaskedTextColumnSettings mask_StartDate = new DataGridViewGeneratorMaskedTextColumnSettings();
            mask_StartDate.CellValidating += (s, e) => 
            {
                if (e.RowIndex == -1 || MyUtility.Check.Empty(e.FormattedValue))
                {
                    return;
                }

                DataRow dr = this.gridDetail.GetDataRow(e.RowIndex);
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
                        //if (DateTime.Compare((DateTime)e.FormattedValue, (DateTime)dr["FinishDate"]) == 1)
                        if (DateTime.Compare(Convert.ToDateTime(strStartDate), Convert.ToDateTime(strEndDate)) == 1)
                        {
                            MyUtility.Msg.WarningBox("Start Date cannot later than Finish Date.");
                            e.Cancel = true;
                        }
                    }

                    dr["FinishDate"] = e.FormattedValue.ToString().PadRight(12, '0');
                    dr.EndEdit();
                }
                else
                {
                    e.Cancel = true;
                }
            };
            #endregion

            this.Helper.Controls.Grid.Generator(this.gridDetail)
                .CheckBox("Select", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
                .Text("Factory", header: "Factory", width: Widths.AnsiChars(7), iseditingreadonly: true)
                .Text("RQNo", header: "RQ NO", width: Widths.AnsiChars(14), iseditingreadonly: true)
                .Text("SP", header: "SP", width: Widths.AnsiChars(14), iseditingreadonly: true)
                .Text("Department", header: "Department", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Date("RequestDate", header: "Request Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Date("IssueDate", header: "Issue Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("ttlRoll", header: "Total Roll", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("Remark", header: "Remark", width: Widths.AnsiChars(25), iseditingreadonly: true)
                .Text("Worker", header: "Worker", width: Widths.AnsiChars(11), iseditingreadonly: false, settings: col_Worker)
                .Text("Location", header: "Location", width: Widths.AnsiChars(11), iseditingreadonly: true, settings: col_Location)
                //.DateTime("StartDate", header: "Start Date", width: Widths.AnsiChars(20), iseditingreadonly: false, format: DataGridViewDateTimeFormat.yyyyMMddHHmm)
                //.DateTime("FinishDate", header: "Finish Date", width: Widths.AnsiChars(20), iseditingreadonly: false, format: DataGridViewDateTimeFormat.yyyyMMddHHmm)
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

        private void GridDetail_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            //if (e.RowIndex < 0 || this.EditMode == false)
            //{
            //    return;
            //}

            //DataRow dr = this.gridDetail.GetDataRow(e.RowIndex);
            //if (dr == null)
            //{
            //    return;
            //}

            //if (MyUtility.Check.Empty(dr["StartDate"]) && !MyUtility.Check.Empty(dr["FinishDate"]))
            //{
            //    MyUtility.Msg.WarningBox("Start Date cannot be empty.");
            //    e.Cancel = true;
            //    return;
            //}

            //if (!MyUtility.Check.Empty(dr["StartDate"]) && !MyUtility.Check.Empty(dr["FinishDate"]))
            //{
            //    if (DateTime.Compare((DateTime)dr["StartDate"], (DateTime)dr["FinishDate"]) == 1)
            //    {
            //        MyUtility.Msg.WarningBox("Start Date cannot later than Finish Date.");
            //        e.Cancel = true;
            //        return;
            //    }
            //}

            //dr.EndEdit();
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
                //string startDate = MyUtility.Check.Empty(dr["StartDate"]) ? "null" : "'" + ((DateTime)dr["StartDate"]).ToString("yyyy/MM/dd HH:mm") + "'";
                //string endDate = MyUtility.Check.Empty(dr["FinishDate"]) ? "null" : "'" + ((DateTime)dr["FinishDate"]).ToString("yyyy/MM/dd HH:mm") + "'";
                string startDate = MyUtility.Check.Empty(dr["StartDate"]) ? "null" : "'" + this.DateTimeMaskFull(dr["StartDate"].ToString()) + "'";
                string endDate = MyUtility.Check.Empty(dr["FinishDate"]) ? "null" : "'" + this.DateTimeMaskFull(dr["FinishDate"].ToString()) + "'";
                int scan = MyUtility.Check.Empty(dr["Scan"]) ? 0 : 1;

                upd_sql += $@"
update IssueLack
set PrepardWorker = '{dr["Worker"]}' ,PrepardLocation = '{dr["Location"]}' ,PrepareStartDate = {startDate} ,PrepardFinishDate = {endDate}, ScanTransferSlip = {scan}
where id = '{dr["ID"]}'" + Environment.NewLine;
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
,iL.FactoryID
,[RQNo] = iL.RequestID
,[SP] = Lack.OrderID
,[Department] = Lack.Dept
,[RequestDate] = Lack.ApvDate
,[IssueDate] = iL.EditDate
,[ttlRoll] = ttlRoll.value
,iL.Remark
,[Worker] = iL.PrepardWorker
,[Location] = iL.PrepardLocation
,[StartDate] = FORMAT(iL.PrepareStartDate,'yyyyMMddHHmm')
,[FinishDate] = FORMAT(iL.PrepardFinishDate,'yyyyMMddHHmm')
,[PreparingTime] = ''
,[LeadTime]= ''
,[Scan] = iL.ScanTransferSlip
,iL.Id
from IssueLack iL
left join Lack on Lack.ID = iL.RequestID
outer apply(
	select value = count(1)
	from (
		select distinct iL2.POID,iL2.Seq1,iL2.Seq2,iL2.Roll,iL2.Dyelot
		from IssueLack_Detail iL2
		where iL2.Id = il.Id
	) a
)ttlRoll
where 1=1
";
            #endregion

            #region Where 條件
            if (!MyUtility.Check.Empty(this.dateRequestDate.DateBox1.Value))
            {
                sqlcmd += $" and Lack.ApvDate >= '{this.dateRequestDate.DateBox1.Text}'";
            }

            if (!MyUtility.Check.Empty(this.dateRequestDate.DateBox2.Value))
            {
                sqlcmd += $" and Lack.ApvDate <= '{this.dateRequestDate.DateBox2.Text}'";
            }

            if (!MyUtility.Check.Empty(this.txtRequestNo.Text))
            {
                sqlcmd += $" and iL.RequestID = '{this.txtRequestNo.Text}'";
            }

            if (!MyUtility.Check.Empty(this.comboStatus.Text))
            {
                switch (this.comboStatus.Text)
                {
                    case "Preparing":
                        sqlcmd += $" and (iL.PrepareStartDate is not null and iL.PrepardFinishDate is null)";
                        break;
                    case "Finished":
                        sqlcmd += $" and (iL.PrepareStartDate is not null and iL.PrepardFinishDate is not null)";
                        break;
                }
            }

            if (!MyUtility.Check.Empty(this.txtfactory.Text))
            {
                sqlcmd += $" and iL.FactoryID = '{this.txtfactory.Text}'";
            }

            if (!MyUtility.Check.Empty(this.txtSPNo.Text))
            {
                sqlcmd += $" and Lack.OrderID = '{this.txtSPNo.Text}'";
            }
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
            if (tmp_dt == null)
            {
                return;
            }

            for (int i = 0; i < tmp_dt.Rows.Count; i++)
            {
                if (!MyUtility.Check.Empty(tmp_dt.Rows[i]["StartDate"]))
                {
                    // Preparing
                    if (MyUtility.Check.Empty(tmp_dt.Rows[i]["FinishDate"]))
                    {
                        this.gridDetail.Rows[i].DefaultCellStyle.BackColor = this.preparingColor;
                    }
                    else
                    {
                        // Finished
                        this.gridDetail.Rows[i].DefaultCellStyle.BackColor = this.finishedColor;
                    }
                }
            }
        }
    }
}
