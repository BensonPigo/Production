using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Win.Tems;
using Ict.Win;
using Sci.Win.Tools;
using Sci.Data;
using System.Linq;

namespace Sci.Production.Quality
{
    public partial class P10 : Sci.Win.Tems.Input6
    {
        private string loginID = Sci.Env.User.UserID;
        private string Factory = Sci.Env.User.Keyword;

        public P10(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }

        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["id"].ToString();
            string cmd = string.Format(@"
select  sd.id,
        sd.No,
        sd.InspDate,
        sd.Result,
        sd.Technician,
        TechnicianName = vs_tech.Name_Extno,
        sd.Remark,
        sd.Sender,
        sd.SendDate,
        sd.Receiver,
        sd.ReceivedDate,
        MRName = sd.AddName + ' - ' + Format(sd.AddDate,'yyyy/MM/dd HH:mm:ss'),
        LastEditName = sd.EditName + ' - ' + Format(sd.EditDate,'yyyy/MM/dd HH:mm:ss')
from SampleGarmentTest_Detail sd WITH (NOLOCK)
left join view_ShowName vs_tech WITH (NOLOCK) on sd.Technician = vs_tech.id
where sd.id='{0}' order by sd.No
", masterID);
            this.DetailSelectCommand = cmd;
            return base.OnDetailSelectCommandPrepare(e);
        }

        private void btnSend(object sender, EventArgs e)
        {
            if (this.EditMode == true)
            {
                return;
            }
            
            DualResult result;
            DateTime sendDate = DateTime.Now;
            string sqlcmd = string.Format(@"update SampleGarmentTest_Detail set Sender ='{0}',SendDate='{1}' where id='{2}' and No = '{3}';
                                           update SampleGarmentTest set ReleasedDate = '{1}' where  id='{2}';", loginID, sendDate.ToShortDateString(), CurrentMaintain["ID"], CurrentDetailData["No"]);
            result = DBProxy.Current.Execute(null, sqlcmd);
            if (!result)
            {
                MyUtility.Msg.WarningBox("ErrorMsg: " + result);
                return;
            }
            this.CurrentDetailData["Sender"] = loginID;
            this.CurrentDetailData["SendDate"] = sendDate.ToShortDateString();
            this.CurrentMaintain["ReleasedDate"] = sendDate.ToShortDateString();
            
            Send_Mail();
        }

        private void Send_Mail()
        {

            string mailto = "";
            string mailcc = Env.User.MailAddress;
            string subject = "Sample Garment Test - Style #:" + displayBoxStyle.Text + ", Season :" + displayBoxSeason.Text;
            string content = "Sample Garment Test - Style #:" + displayBoxStyle.Text + ", Season :" + displayBoxSeason.Text + " had been sent, please receive and confirm";
            var email = new MailTo(Sci.Env.Cfg.MailFrom, mailto, mailcc, subject, null, content.ToString(), false, true);
            email.ShowDialog(this);

        }

        private void btnReceive(object sender, EventArgs e)
        {
            if (this.EditMode == true)
            {
                return;
            }
            DateTime receivedDate = DateTime.Now;
            DateTime deadLine;
            DualResult result;
            DayOfWeek dayWeek = receivedDate.DayOfWeek;
            if (dayWeek == DayOfWeek.Thursday || dayWeek == DayOfWeek.Friday || dayWeek == DayOfWeek.Saturday)
            {
                deadLine = receivedDate.AddDays(4);
            }
            else
            {
                deadLine = receivedDate.AddDays(3);
            }
            string sqlcmd = string.Format(@"update SampleGarmentTest_Detail set Receiver ='{0}',ReceivedDate='{1}' where id='{2}'  and No = '{3}';
                                            update SampleGarmentTest set ReceivedDate = '{1}',Deadline = '{4}' where  id='{2}';", loginID, receivedDate.ToShortDateString(), CurrentMaintain["ID"], CurrentDetailData["No"], deadLine.ToShortDateString());
            result = DBProxy.Current.Execute(null, sqlcmd);
            if (!result)
            {
                MyUtility.Msg.WarningBox("ErrorMsg: " + result);
                return;
            }
            this.CurrentDetailData["Receiver"] = loginID;
            this.CurrentDetailData["ReceivedDate"] = receivedDate.ToShortDateString();
            this.CurrentMaintain["ReceivedDate"] = receivedDate.ToShortDateString();
            this.CurrentMaintain["Deadline"] = deadLine;
            
        }

        protected override void OnDetailGridSetup()
        {
            DataGridViewGeneratorDateColumnSettings inspDateCell = new DataGridViewGeneratorDateColumnSettings();
            DataGridViewGeneratorTextColumnSettings inspectorCell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings CommentsCell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings SendCell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings SenderCell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings ReceiveCell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings ReceiverCell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorComboBoxColumnSettings ResultValid = new DataGridViewGeneratorComboBoxColumnSettings();
            DataGridViewGeneratorComboBoxColumnSettings ResultComboCell = new DataGridViewGeneratorComboBoxColumnSettings();

            Dictionary<string, string> ResultCombo = new Dictionary<string, string>();
            ResultCombo.Add("Pass", "Pass");
            ResultCombo.Add("Fail", "Fail");
            ResultComboCell.DataSource = new BindingSource(ResultCombo, null);
            ResultComboCell.ValueMember = "Key";
            ResultComboCell.DisplayMember = "Value";

            #region inspDateCell
            inspDateCell.CellValidating += (s, e) =>
            {
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);

                dr["EditName"] = loginID;
                dr["EditDate"] = DateTime.Now.ToShortDateString();
                if (!MyUtility.Check.Empty(e.FormattedValue))
                {
                    dr["inspdate"] = e.FormattedValue;
                }

            };
            #endregion

            #region inspectorCell

            inspectorCell.EditingMouseDown += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                if (this.EditMode == false) return;
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    DataRow dr_showname;
                    DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                    string scalecmd = @"select t.id,p.name from Technician t  WITH (NOLOCK) inner join Pass1 p WITH (NOLOCK) on t.id = p.id where t.SampleGarmentWash = 1 and p.Resign is null";
                    SelectItem item1 = new SelectItem(scalecmd, "15,15", dr["Technician"].ToString());
                    DialogResult result = item1.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }
                    dr["Technician"] = item1.GetSelectedString(); //將選取selectitem value帶入GridView
                    if (MyUtility.Check.Seek(string.Format(@"select * from view_ShowName where id ='{0}'", item1.GetSelectedString()), out dr_showname))
                    {
                        dr["TechnicianName"] = dr_showname["Name_Extno"];
                    }
                }
            };
            inspectorCell.CellValidating += (s, e) =>
            {
                if (!this.EditMode) return;//非編輯模式 
                if (e.RowIndex == -1) return; //沒東西 return
                if (MyUtility.Check.Empty(e.FormattedValue)) return; // 沒資料 return               

                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                DataRow dr_cmd;
                DataRow dr_showname;
                if (MyUtility.Check.Empty(e.FormattedValue))
                {
                    dr["Technician"] = "";
                    dr["TechnicianName"] = "";
                    return; // 沒資料 return
                }
                string cmd = string.Format(@"select t.id,p.name from Technician t  WITH (NOLOCK) inner join Pass1 p WITH (NOLOCK) on t.id = p.id where t.SampleGarmentWash = 1 and t.id='{0}' and p.Resign is null", e.FormattedValue);

                if (MyUtility.Check.Seek(cmd, out dr_cmd))
                {
                    dr["EditName"] = loginID;
                    dr["EditDate"] = DateTime.Now.ToShortDateString();
                    dr["Technician"] = e.FormattedValue;
                    if (MyUtility.Check.Seek(string.Format(@"select * from view_ShowName where id ='{0}'", e.FormattedValue), out dr_showname))
                    {
                        dr["TechnicianName"] = dr_showname["Name_Extno"];
                    }
                }
                else
                {
                    dr["EditName"] = loginID;
                    dr["EditDate"] = DateTime.Now.ToShortDateString();
                    dr["Technician"] = "";
                    dr["TechnicianName"] = "";
                    dr.EndEdit();
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("< Technician: {0}> not found!!!", e.FormattedValue));
                    return;
                }
                CurrentDetailData.EndEdit();
                this.update_detailgrid_CellValidated(e.RowIndex);
            };
            #endregion

            Helper.Controls.Grid.Generator(this.detailgrid)
            .Numeric("No", header: "No. Of Test", integer_places: 8, decimal_places: 0, iseditingreadonly: true, width: Widths.AnsiChars(8))
            .Date("Inspdate", header: "Test Date", width: Widths.AnsiChars(10), settings: inspDateCell)
            .ComboBox("Result", header: "Result", width: Widths.AnsiChars(10), settings: ResultComboCell)
            .Text("Technician", header: "Technician", width: Widths.AnsiChars(10), settings: inspectorCell)
            .Text("TechnicianName", header: "Technician Name", width: Widths.AnsiChars(20), iseditingreadonly: true)
            .Text("Remark", header: "Comments", width: Widths.AnsiChars(10), settings: CommentsCell)
            .Button("Send", null, header: "Send", width: Widths.AnsiChars(5), onclick: btnSend)
            .Text("Sender", header: "Sender", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Date("SendDate", header: "Send Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Button("Receive", null, header: "Receive", width: Widths.AnsiChars(5), onclick: btnReceive)
            .Text("Receiver", header: "Receiver", width: Widths.AnsiChars(5), iseditingreadonly: true)
            .Date("ReceivedDate", header: "Receive Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("MRName", header: "MR Name", width: Widths.AnsiChars(25), iseditingreadonly: true)// addName + addDate
            .Text("LastEditName", header: "Last Edit Name", width: Widths.AnsiChars(25), iseditingreadonly: true);//editName + editDate
        }

        void update_detailgrid_CellValidated(int RowIndex)
        {
            this.detailgrid.InvalidateRow(RowIndex);
        }

        protected override void OnDetailGridInsert(int index = 1)
        {
            base.OnDetailGridInsert(index);
            DataTable dt = (DataTable)detailgridbs.DataSource;

            int MaxNo;
            if (dt.Rows.Count == 0)
            {
                MaxNo = 0;
                base.OnDetailGridInsert(0);
                CurrentDetailData["No"] = MaxNo + 1;
            }
            else
            {
                MaxNo = Convert.ToInt32(dt.Compute("Max(No)", ""));
                CurrentDetailData["No"] = MaxNo + 1;
            }

        }

        protected override bool ClickSaveBefore()
        {
            DataTable detail_dt = (DataTable)this.detailgridbs.DataSource;
            if (detail_dt.AsEnumerable().Where(s => s.RowState != DataRowState.Deleted).Count() > 0)
            {
                //更新表頭 ReceiveDate,ReleasedDate ,Deadline,InspDate,Result
                //ReceivedDate
                var receivedDate = detail_dt.AsEnumerable().Where(s => s.RowState != DataRowState.Deleted).Where(s => !MyUtility.Check.Empty(s["ReceivedDate"])).Select(s => s["ReceivedDate"]).Max();
                CurrentMaintain["ReceivedDate"] = receivedDate == null ? DBNull.Value : receivedDate;

                //ReleasedDate
                var releasedDate = detail_dt.AsEnumerable().Where(s => s.RowState != DataRowState.Deleted).Where(s => !MyUtility.Check.Empty(s["SendDate"])).Select(s => s["SendDate"]).Max();
                CurrentMaintain["ReleasedDate"] = releasedDate == null ? DBNull.Value : releasedDate;

                //Deadline
                if (MyUtility.Check.Empty(CurrentMaintain["ReceivedDate"]))
                {
                    this.CurrentMaintain["Deadline"] = DBNull.Value;
                }
                else
                {
                    DayOfWeek dayWeek = ((DateTime)CurrentMaintain["ReceivedDate"]).DayOfWeek;
                    if (dayWeek == DayOfWeek.Thursday || dayWeek == DayOfWeek.Friday || dayWeek == DayOfWeek.Saturday)
                    {
                        this.CurrentMaintain["Deadline"] = ((DateTime)CurrentMaintain["ReceivedDate"]).AddDays(4);
                    }
                    else
                    {
                        this.CurrentMaintain["Deadline"] = ((DateTime)CurrentMaintain["ReceivedDate"]).AddDays(3);
                    }
                }

                //InspDate
                var inspDate = detail_dt.AsEnumerable().Where(s => s.RowState != DataRowState.Deleted).Where(s => !MyUtility.Check.Empty(s["InspDate"])).Select(s => s["InspDate"]).Max();
                CurrentMaintain["InspDate"] = inspDate == null ? DBNull.Value : inspDate;

                //Result
                int fail_cnt = detail_dt.AsEnumerable().Where(s => s.RowState != DataRowState.Deleted).Where(s => s["Result"].Equals("Fail")).Count();
                int pass_cnt = detail_dt.AsEnumerable().Where(s => s.RowState != DataRowState.Deleted).Where(s => s["Result"].Equals("Pass")).Count();
                if (fail_cnt == 0 && pass_cnt == 0)
                {
                    CurrentMaintain["Result"] = string.Empty;
                }
                else if (fail_cnt > 0)
                {
                    CurrentMaintain["Result"] = "Fail";
                }
                else
                {
                    CurrentMaintain["Result"] = "Pass";
                }

            }
            else
            {
                CurrentMaintain["ReceivedDate"] = DBNull.Value;
                CurrentMaintain["ReleasedDate"] = DBNull.Value;
                this.CurrentMaintain["Deadline"] = DBNull.Value;
                CurrentMaintain["InspDate"] = DBNull.Value;
                CurrentMaintain["Result"] = string.Empty;
            }
            return base.ClickSaveBefore();
        }

    }
}
