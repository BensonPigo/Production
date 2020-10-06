using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Win.Tools;
using Sci.Data;
using System.Linq;
using System.Data.SqlClient;
using static Sci.Production.PublicPrg.Prgs;
using System.Text;

namespace Sci.Production.Quality
{
    public partial class P10 : Win.Tems.Input6
    {
        private readonly string loginID = Env.User.UserID;
        private readonly string Factory = Env.User.Keyword;
        private int ReportNoCount = 0;
        private ToolStripMenuItem edit;

        public P10(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.detailgrid.ContextMenuStrip = this.detailgridmenus;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            this.detailgridmenus.Items.Clear(); // 清空原有的Menu Item
            this.Helper.Controls.ContextMenu.Generator(this.detailgridmenus).Menu("Edit this Record's detail", onclick: (s, e) => this.EditThisDetail()).Get(out this.edit);

            base.OnFormLoaded();
        }

        private void EditThisDetail()
        {
            if (this.CurrentDetailData == null)
            {
                MyUtility.Msg.WarningBox("No Detail Data!");
                return;
            }

            string sqlShrinkage = $@"select * from[SampleGarmentTest_Detail] where id = {this.CurrentDetailData["ID"]} and No = {this.CurrentDetailData["No"]} ";
            DataTable tmp;
            DBProxy.Current.Select(null, sqlShrinkage, out tmp);
            if (tmp.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("No Detail data is Saved!!");
                return;
            }

            P10_Detail callNewDetailForm = new P10_Detail(this.EditMode, this.CurrentMaintain, this.CurrentDetailData);
            callNewDetailForm.ShowDialog(this);
            callNewDetailForm.Dispose();
            this.RenewData();
            this.OnDetailEntered();
        }

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["id"].ToString();
            string cmd = string.Format(
                @"
select  sd.id,
        sd.No,
        sd.ReportNo,
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
		,sd.MtlTypeID
        ,sd.Above50NaturalFibres
        ,sd.Above50SyntheticFibres
from SampleGarmentTest_Detail sd WITH (NOLOCK)
left join view_ShowName vs_tech WITH (NOLOCK) on sd.Technician = vs_tech.id
where sd.id='{0}' order by sd.No
", masterID);
            this.DetailSelectCommand = cmd;
            return base.OnDetailSelectCommandPrepare(e);
        }

        private void BtnSend(object sender, EventArgs e)
        {
            if (this.EditMode == true)
            {
                return;
            }

            DualResult result;
            DateTime sendDate = DateTime.Now;
            string sqlcmd = string.Format(
                @"update SampleGarmentTest_Detail set Sender ='{0}',SendDate=@sendate where id='{1}' and No = '{2}';
                                           update SampleGarmentTest set ReleasedDate = @sendate where  id='{1}';", this.loginID, this.CurrentMaintain["ID"], this.CurrentDetailData["No"]);
            List<SqlParameter> listSendate = new List<SqlParameter>() { new SqlParameter("@sendate", sendDate) };
            result = DBProxy.Current.Execute(null, sqlcmd, listSendate);
            if (!result)
            {
                MyUtility.Msg.WarningBox("ErrorMsg: " + result);
                return;
            }

            this.CurrentDetailData["Sender"] = this.loginID;
            this.CurrentDetailData["SendDate"] = sendDate;
            this.CurrentMaintain["ReleasedDate"] = sendDate;

            this.Send_Mail();
        }

        private void Send_Mail()
        {
            string mailto = string.Empty;
            string mailcc = Env.User.MailAddress;
            string subject = "Sample Garment Test - Style #:" + this.displayBoxStyle.Text + ", Season :" + this.displayBoxSeason.Text;
            string content = "Sample Garment Test - Style #:" + this.displayBoxStyle.Text + ", Season :" + this.displayBoxSeason.Text + " had been sent, please receive and confirm";
            var email = new MailTo(Env.Cfg.MailFrom, mailto, mailcc, subject, null, content.ToString(), false, true);
            email.ShowDialog(this);
        }

        private void BtnReceive(object sender, EventArgs e)
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

            string sqlcmd = string.Format(
                @"update SampleGarmentTest_Detail set Receiver ='{0}',ReceivedDate=@receivedDate where id='{1}'  and No = '{2}';
                                            update SampleGarmentTest set ReceivedDate = @receivedDate,Deadline = @deadLine where  id='{1}';", this.loginID, this.CurrentMaintain["ID"], this.CurrentDetailData["No"]);
            List<SqlParameter> sqlpar = new List<SqlParameter>()
            {
                new SqlParameter("@receivedDate", receivedDate),
                new SqlParameter("@deadLine", deadLine),
            };
            result = DBProxy.Current.Execute(null, sqlcmd, sqlpar);
            if (!result)
            {
                MyUtility.Msg.WarningBox("ErrorMsg: " + result);
                return;
            }

            this.CurrentDetailData["Receiver"] = this.loginID;
            this.CurrentDetailData["ReceivedDate"] = receivedDate;
            this.CurrentMaintain["ReceivedDate"] = receivedDate;
            this.CurrentMaintain["Deadline"] = deadLine;
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            DataGridViewGeneratorDateColumnSettings inspDateCell = new DataGridViewGeneratorDateColumnSettings();
            DataGridViewGeneratorTextColumnSettings inspectorCell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings commentsCell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings sendCell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings senderCell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings receiveCell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings receiverCell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorComboBoxColumnSettings resultValid = new DataGridViewGeneratorComboBoxColumnSettings();
            DataGridViewGeneratorComboBoxColumnSettings resultComboCell = new DataGridViewGeneratorComboBoxColumnSettings();
            DataGridViewGeneratorComboBoxColumnSettings mtlTypeIDComboCell = new DataGridViewGeneratorComboBoxColumnSettings();

            Dictionary<string, string> resultCombo = new Dictionary<string, string>();
            resultCombo.Add("Pass", "Pass");
            resultCombo.Add("Fail", "Fail");
            resultComboCell.DataSource = new BindingSource(resultCombo, null);
            resultComboCell.ValueMember = "Key";
            resultComboCell.DisplayMember = "Value";

            Dictionary<string, string> mtlTypeCombo = new Dictionary<string, string>();
            mtlTypeCombo.Add(string.Empty, string.Empty);
            mtlTypeCombo.Add("KNIT", "KNIT");
            mtlTypeCombo.Add("WOVEN", "WOVEN");
            mtlTypeIDComboCell.DataSource = new BindingSource(mtlTypeCombo, null);
            mtlTypeIDComboCell.ValueMember = "Key";
            mtlTypeIDComboCell.DisplayMember = "Value";

            mtlTypeIDComboCell.CellEditable += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(dr["MtlTypeID"]) && this.EditMode)
                {
                    e.IsEditable = true;
                }
                else
                {
                    e.IsEditable = false;
                }
            };

            #region inspDateCell
            inspDateCell.CellValidating += (s, e) =>
            {
                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);

                dr["EditName"] = this.loginID;
                dr["EditDate"] = DateTime.Now;
                if (!MyUtility.Check.Empty(e.FormattedValue))
                {
                    dr["inspdate"] = e.FormattedValue;
                }
            };
            #endregion

            #region inspectorCell

            inspectorCell.EditingMouseDown += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                if (this.EditMode == false)
                {
                    return;
                }

                if (e.Button == MouseButtons.Right)
                {
                    DataRow dr_showname;
                    DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                    string scalecmd = @"select t.id,p.name from Technician t  WITH (NOLOCK) inner join Pass1 p WITH (NOLOCK) on t.id = p.id where t.SampleGarmentWash = 1 and p.Resign is null";
                    SelectItem item1 = new SelectItem(scalecmd, "15,15", dr["Technician"].ToString());
                    DialogResult result = item1.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    dr["Technician"] = item1.GetSelectedString(); // 將選取selectitem value帶入GridView
                    if (MyUtility.Check.Seek(string.Format(@"select * from view_ShowName where id ='{0}'", item1.GetSelectedString()), out dr_showname))
                    {
                        dr["TechnicianName"] = dr_showname["Name_Extno"];
                    }
                }
            };
            inspectorCell.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return; // 非編輯模式
                }

                if (e.RowIndex == -1)
                {
                    return; // 沒東西 return
                }

                if (MyUtility.Check.Empty(e.FormattedValue))
                {
                    return; // 沒資料 return
                }

                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                DataRow dr_cmd;
                DataRow dr_showname;
                if (MyUtility.Check.Empty(e.FormattedValue))
                {
                    dr["Technician"] = string.Empty;
                    dr["TechnicianName"] = string.Empty;
                    return; // 沒資料 return
                }

                string cmd = string.Format(@"select t.id,p.name from Technician t  WITH (NOLOCK) inner join Pass1 p WITH (NOLOCK) on t.id = p.id where t.SampleGarmentWash = 1 and t.id='{0}' and p.Resign is null", e.FormattedValue);

                if (MyUtility.Check.Seek(cmd, out dr_cmd))
                {
                    dr["EditName"] = this.loginID;
                    dr["EditDate"] = DateTime.Now;
                    dr["Technician"] = e.FormattedValue;
                    if (MyUtility.Check.Seek(string.Format(@"select * from view_ShowName where id ='{0}'", e.FormattedValue), out dr_showname))
                    {
                        dr["TechnicianName"] = dr_showname["Name_Extno"];
                    }
                }
                else
                {
                    dr["EditName"] = this.loginID;
                    dr["EditDate"] = DateTime.Now;
                    dr["Technician"] = string.Empty;
                    dr["TechnicianName"] = string.Empty;
                    dr.EndEdit();
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("< Technician: {0}> not found!!!", e.FormattedValue));
                    return;
                }

                this.CurrentDetailData.EndEdit();
                this.Update_detailgrid_CellValidated(e.RowIndex);
            };
            #endregion

            this.Helper.Controls.Grid.Generator(this.detailgrid)
            .Numeric("No", header: "No", integer_places: 8, decimal_places: 0, iseditingreadonly: true, width: Widths.AnsiChars(8))
            .Text("ReportNo", header: "ReportNo", width: Widths.AnsiChars(15))
            .ComboBox("MtlTypeID", header: "Material" + Environment.NewLine + "Type", width: Widths.AnsiChars(10), settings: mtlTypeIDComboCell)
            .Date("Inspdate", header: "Test Date", width: Widths.AnsiChars(10), settings: inspDateCell)
            .ComboBox("Result", header: "Result", width: Widths.AnsiChars(10), settings: resultComboCell)
            .Text("Technician", header: "Technician", width: Widths.AnsiChars(10), settings: inspectorCell)
            .Text("TechnicianName", header: "Technician Name", width: Widths.AnsiChars(20), iseditingreadonly: true)
            .Text("Remark", header: "Comments", width: Widths.AnsiChars(10), settings: commentsCell)
            .Button("Send", null, header: "Send", width: Widths.AnsiChars(5), onclick: this.BtnSend)
            .Text("Sender", header: "Sender", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Date("SendDate", header: "Send Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Button("Receive", null, header: "Receive", width: Widths.AnsiChars(5), onclick: this.BtnReceive)
            .Text("Receiver", header: "Receiver", width: Widths.AnsiChars(5), iseditingreadonly: true)
            .Date("ReceivedDate", header: "Receive Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("MRName", header: "MR Name", width: Widths.AnsiChars(25), iseditingreadonly: true) // addName + addDate
            .Text("LastEditName", header: "Last Edit Name", width: Widths.AnsiChars(25), iseditingreadonly: true); // editName + editDate
        }

        private void Update_detailgrid_CellValidated(int rowIndex)
        {
            this.detailgrid.InvalidateRow(rowIndex);
        }

        /// <inheritdoc/>
        protected override void OnDetailGridInsert(int index = 1)
        {
            base.OnDetailGridInsert(index);
            DataTable dt = (DataTable)this.detailgridbs.DataSource;

            int maxNo;
            if (dt.Rows.Count == 0)
            {
                maxNo = 0;
                base.OnDetailGridInsert(0);
                this.CurrentDetailData["No"] = maxNo + 1;
            }
            else
            {
                maxNo = Convert.ToInt32(dt.Compute("Max(No)", string.Empty));
                this.CurrentDetailData["No"] = maxNo + 1;

                string tmpId = MyUtility.GetValue.GetID(Env.User.Keyword + "GM", "SampleGarmentTest_Detail", DateTime.Today, 2, "ReportNo", null);
                string head = tmpId.Substring(0, 9);
                int seq = Convert.ToInt32(tmpId.Substring(9, 4));

                tmpId = head + string.Format("{0:0000}", seq + this.ReportNoCount);

                this.CurrentDetailData["ReportNo"] = tmpId;
                this.ReportNoCount++;
            }

            this.CurrentDetailData["Result"] = string.Empty;
            this.CurrentDetailData["Remark"] = string.Empty;
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            DataTable detail_dt = (DataTable)this.detailgridbs.DataSource;

            foreach (DataRow dr in ((DataTable)this.detailgridbs.DataSource).Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    if (MyUtility.Check.Empty(dr["MtlTypeID"]))
                    {
                        MyUtility.Msg.WarningBox("Material Type cannot be empty !! ");
                        return false;
                    }
                }
            }

            if (detail_dt.AsEnumerable().Where(s => s.RowState != DataRowState.Deleted).Count() > 0)
            {
                // 更新表頭 ReceiveDate,ReleasedDate ,Deadline,InspDate,Result
                // ReceivedDate
                var receivedDate = detail_dt.AsEnumerable().Where(s => s.RowState != DataRowState.Deleted).Where(s => !MyUtility.Check.Empty(s["ReceivedDate"])).Select(s => s["ReceivedDate"]).Max();
                this.CurrentMaintain["ReceivedDate"] = receivedDate == null ? DBNull.Value : receivedDate;

                // ReleasedDate
                var releasedDate = detail_dt.AsEnumerable().Where(s => s.RowState != DataRowState.Deleted).Where(s => !MyUtility.Check.Empty(s["SendDate"])).Select(s => s["SendDate"]).Max();
                this.CurrentMaintain["ReleasedDate"] = releasedDate == null ? DBNull.Value : releasedDate;

                // Deadline
                if (MyUtility.Check.Empty(this.CurrentMaintain["ReceivedDate"]))
                {
                    this.CurrentMaintain["Deadline"] = DBNull.Value;
                }
                else
                {
                    DayOfWeek dayWeek = ((DateTime)this.CurrentMaintain["ReceivedDate"]).DayOfWeek;
                    if (dayWeek == DayOfWeek.Thursday || dayWeek == DayOfWeek.Friday || dayWeek == DayOfWeek.Saturday)
                    {
                        this.CurrentMaintain["Deadline"] = ((DateTime)this.CurrentMaintain["ReceivedDate"]).AddDays(4);
                    }
                    else
                    {
                        this.CurrentMaintain["Deadline"] = ((DateTime)this.CurrentMaintain["ReceivedDate"]).AddDays(3);
                    }
                }

                // InspDate
                var inspDate = detail_dt.AsEnumerable().Where(s => s.RowState != DataRowState.Deleted).Where(s => !MyUtility.Check.Empty(s["InspDate"])).Select(s => s["InspDate"]).Max();
                this.CurrentMaintain["InspDate"] = inspDate == null ? DBNull.Value : inspDate;

                // Result
                int fail_cnt = detail_dt.AsEnumerable().Where(s => s.RowState != DataRowState.Deleted).Where(s => s["Result"].Equals("Fail")).Count();
                int pass_cnt = detail_dt.AsEnumerable().Where(s => s.RowState != DataRowState.Deleted).Where(s => s["Result"].Equals("Pass")).Count();
                if (fail_cnt == 0 && pass_cnt == 0)
                {
                    this.CurrentMaintain["Result"] = string.Empty;
                }
                else if (fail_cnt > 0)
                {
                    this.CurrentMaintain["Result"] = "Fail";
                }
                else
                {
                    this.CurrentMaintain["Result"] = "Pass";
                }
            }
            else
            {
                this.CurrentMaintain["ReceivedDate"] = DBNull.Value;
                this.CurrentMaintain["ReleasedDate"] = DBNull.Value;
                this.CurrentMaintain["Deadline"] = DBNull.Value;
                this.CurrentMaintain["InspDate"] = DBNull.Value;
                this.CurrentMaintain["Result"] = string.Empty;
            }

            this.ReportNoCount = 0;

            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override DualResult ClickSave()
        {
            DualResult upResult = new DualResult(true);
            string update_cmd = string.Empty;
            foreach (DataRow dr in ((DataTable)this.detailgridbs.DataSource).Rows)
            {
                if (dr.RowState == DataRowState.Deleted)
                {
                    if (!MyUtility.Check.Empty(dr["senddate", DataRowVersion.Original]))
                    {
                        return new DualResult(false, "SendDate is existed, can not delete.", "Warning");
                    }

                    List<SqlParameter> spamDet = new List<SqlParameter>();
                    update_cmd = @"
                        Delete From SampleGarmentTest_Detail WITH (NOLOCK) Where id =@id and no=@no;";
                    spamDet.Add(new SqlParameter("@id", dr["ID", DataRowVersion.Original]));
                    spamDet.Add(new SqlParameter("@no", dr["NO", DataRowVersion.Original]));
                    upResult = DBProxy.Current.Execute(null, update_cmd, spamDet);
                }
            }

            foreach (DataRow dr in ((DataTable)this.detailgridbs.DataSource).Rows)
            {
                if (dr.RowState == DataRowState.Deleted)
                {
                    string delete3sub = $@"
Delete SampleGarmentTest_Detail_Shrinkage  where id = '{this.CurrentMaintain["ID", DataRowVersion.Original]}' and NO = '{dr["NO", DataRowVersion.Original]}'
Delete SampleGarmentTest_Detail_Twisting where id = '{this.CurrentMaintain["ID", DataRowVersion.Original]}' and NO = '{dr["NO", DataRowVersion.Original]}'
Delete SampleGarmentTest_Detail_Appearance where id = '{this.CurrentMaintain["ID", DataRowVersion.Original]}' and NO = '{dr["NO", DataRowVersion.Original]}'
";
                    DBProxy.Current.Execute(null, delete3sub);
                }
                else
                {
                    if (MyUtility.Check.Empty(dr["Status"]))
                    {
                        dr["Status"] = "New";
                    }

                    if (!MyUtility.Check.Seek($"select 1 from SampleGarmentTest_Detail_Shrinkage with(nolock) where id = '{this.CurrentMaintain["ID"]}' and NO = '{dr["NO"]}'"))
                    {
                        List<SqlParameter> spam = new List<SqlParameter>();
                        spam.Add(new SqlParameter("@ID", this.CurrentMaintain["ID"]));
                        spam.Add(new SqlParameter("@NO", dr["NO"]));
                        string insertShrinkage = $@"
select sl.Location
into #Location1
from SampleGarmentTest gt with(nolock)
inner join style s with(nolock) on s.id = gt.StyleID
inner join Style_Location sl with(nolock) on sl.styleukey = s.ukey
where gt.id = @ID and sl.Location !='B'
group by sl.Location
order by sl.Location desc
CREATE TABLE #type1([type] [varchar](20),seq numeric(6,0))
insert into #type1 values('Chest Width',1)
insert into #type1 values('Sleeve Width',2)
insert into #type1 values('Sleeve Length',3)
insert into #type1 values('Back Length',4)
insert into #type1 values('Hem Opening',5)
---
select distinct sl.Location
into #Location2
from SampleGarmentTest gt with(nolock)
inner join style s with(nolock) on s.id = gt.StyleID
inner join Style_Location sl with(nolock) on sl.styleukey = s.ukey
where gt.id = @ID and sl.Location ='B'



CREATE TABLE #type2([type] [varchar](20),seq numeric(6,0))
insert into #type2 values('Waistband (relax)',1)
insert into #type2 values('Hip Width',2)
insert into #type2 values('Thigh Width',3)
insert into #type2 values('Side Seam',4)
insert into #type2 values('Leg Opening',5)


INSERT INTO [dbo].[SampleGarmentTest_Detail_Shrinkage]([ID],[No],[Location],[Type],[seq])
select @ID,@NO,* from #Location1,#type1
INSERT INTO [dbo].[SampleGarmentTest_Detail_Shrinkage]([ID],[No],[Location],[Type],[seq])
select @ID,@NO,* from #Location2,#type2

INSERT INTO [dbo].[SampleGarmentTest_Detail_Twisting]([ID],[No],[Location])
select @ID,@NO,
[Location]=CASE WHEN Location='B' THEN 'BOTTOM'
WHEN Location='I' THEN 'INNER'
WHEN Location='O' THEN 'OUTER'
WHEN Location='T' THEN 'TOP'
ELSE ''
END

from #Location1
INSERT INTO [dbo].[SampleGarmentTest_Detail_Twisting]([ID],[No],[Location])
select @ID,@NO,[Location]=CASE WHEN Location='B' THEN 'BOTTOM'
WHEN Location='I' THEN 'INNER'
WHEN Location='O' THEN 'OUTER'
WHEN Location='T' THEN 'TOP'
ELSE ''
END from #Location2

INSERT INTO [dbo].[SampleGarmentTest_Detail_Appearance]([ID],[No],[Type],[Seq])
values (@ID,@NO,'Printing / Heat Transfer',1)
INSERT INTO [dbo].[SampleGarmentTest_Detail_Appearance]([ID],[No],[Type],[Seq])
values (@ID,@NO,'Label',2)
INSERT INTO [dbo].[SampleGarmentTest_Detail_Appearance]([ID],[No],[Type],[Seq])
values (@ID,@NO,'Zipper / Snap Button / Button / Tie Cord',3)
INSERT INTO [dbo].[SampleGarmentTest_Detail_Appearance]([ID],[No],[Type],[Seq])
values (@ID,@NO,'Discoloration (colour change )',4)
INSERT INTO [dbo].[SampleGarmentTest_Detail_Appearance]([ID],[No],[Type],[Seq])
values (@ID,@NO,'Colour Staining',5)
INSERT INTO [dbo].[SampleGarmentTest_Detail_Appearance]([ID],[No],[Type],[Seq])
values (@ID,@NO,'Pilling',6)
INSERT INTO [dbo].[SampleGarmentTest_Detail_Appearance]([ID],[No],[Type],[Seq])
values (@ID,@NO,'Shrinkage & Twisting',7)
INSERT INTO [dbo].[SampleGarmentTest_Detail_Appearance]([ID],[No],[Type],[Seq])
values (@ID,@NO,'Appearance of garment after wash',8)
";
                        DBProxy.Current.Execute(null, insertShrinkage, spam);
                    }

                    #region 寫入GarmentTest_Detail_FGPT

                    // 取Location
                    List<string> locations = MyUtility.GetValue.Lookup($@"

SELECT STUFF(
	(select DISTINCT ',' + sl.Location
	from Style s
	INNER JOIN Style_Location sl ON s.Ukey = sl.StyleUkey 
	where s.id='{this.CurrentMaintain["StyleID"]}' AND s.BrandID='{this.CurrentMaintain["BrandID"]}' AND s.SeasonID='{this.CurrentMaintain["SeasonID"]}'
	FOR XML PATH('')
	) 
,1,1,'')").Split(',').ToList();

                    bool containsT = locations.Contains("T");
                    bool containsB = locations.Contains("B");

                    string garmentTest_Detail_ID = MyUtility.Convert.GetString(dr["ID"]);
                    string garmentTest_Detail_No = MyUtility.Convert.GetString(dr["NO"]);

                    StringBuilder insertCmd = new StringBuilder();
                    List<SqlParameter> parameters = new List<SqlParameter>();

                    List<FGPT> fGPTs = new List<FGPT>();

                    bool isRugbyFootBall = MyUtility.Check.Seek($@"select 1 from Style s where s.id='{this.CurrentMaintain["StyleID"]}' AND s.BrandID='{this.CurrentMaintain["BrandID"]}' AND s.SeasonID='{this.CurrentMaintain["SeasonID"]}' AND s.ProgramID like '%FootBall%'");
                    bool isLining = MyUtility.Check.Seek($@"select 1 from Style s where s.id='{this.CurrentMaintain["StyleID"]}' AND s.BrandID='{this.CurrentMaintain["BrandID"]}' AND s.SeasonID='{this.CurrentMaintain["SeasonID"]}' AND s.Description  like '%with lining%' ");

                    // 若只有B則寫入Bottom的項目+ALL的項目，若只有T則寫入TOP的項目+ALL的項目，若有B和T則寫入Top+ Bottom的項目+ALL的項目
                    if (containsT && containsB)
                    {
                        fGPTs = GetDefaultFGPT(false, false, true, isRugbyFootBall, isLining, "S");
                    }
                    else if (containsT)
                    {
                        fGPTs = GetDefaultFGPT(containsT, false, false, isRugbyFootBall, isLining, "T");
                    }
                    else
                    {
                        fGPTs = GetDefaultFGPT(false, containsB, false, isRugbyFootBall, isLining, "B");
                    }

                    int idx = 0;

                    foreach (var fGPT in fGPTs)
                    {
                        string location = string.Empty;

                        switch (fGPT.Location)
                        {
                            case "Top":
                                location = "T";
                                break;
                            case "Bottom":
                                location = "B";
                                break;
                            case "Full": // Top+Bottom = Full
                                location = "S";
                                break;
                            default:
                                location = fGPT.Location;
                                break;
                        }

                        insertCmd.Append($@"

INSERT INTO SampleGarmentTest_Detail_FGPT
           (ID,No,Location,Type,TestDetail,TestUnit,Criteria,TestName)
     VALUES
           ( {garmentTest_Detail_ID}
           , {garmentTest_Detail_No}
           , @Location{idx}
           , @Type{idx}
           , @TestDetail{idx}
           , @TestUnit{idx}
           , @Criteria{idx}  
           , @TestName{idx})

");
                        parameters.Add(new SqlParameter($"@Location{idx}", location));
                        parameters.Add(new SqlParameter($"@Type{idx}", fGPT.Type));
                        parameters.Add(new SqlParameter($"@TestDetail{idx}", fGPT.TestDetail));
                        parameters.Add(new SqlParameter($"@TestUnit{idx}", fGPT.TestUnit));
                        parameters.Add(new SqlParameter($"@Criteria{idx}", fGPT.Criteria));
                        parameters.Add(new SqlParameter($"@TestName{idx}", fGPT.TestName));
                        idx++;
                    }

                    // 找不到才Insert
                    if (!MyUtility.Check.Seek($"SELECT 1 FROM SampleGarmentTest_Detail_FGPT WHERE ID ='{garmentTest_Detail_ID}' AND NO='{garmentTest_Detail_No}'"))
                    {
                        DualResult r = DBProxy.Current.Execute(null, insertCmd.ToString(), parameters);
                        if (!r)
                        {
                            this.ShowErr(r);
                        }
                    }
                    #endregion
                }
            }

            DataTable dt = (DataTable)this.detailgridbs.DataSource;

            if (dt.Rows.Count > 0)
            {
                string maxNo = dt.Compute("MAX(NO)", string.Empty).ToString();
                string where = string.Format("NO='{0}'", maxNo);

                if (dt.Select(where).Count() > 0)
                {
                    DataRow detailRow = dt.Select(where)[0];
                    this.CurrentMaintain["Result"] = detailRow["Result"];
                    this.CurrentMaintain["Inspdate"] = detailRow["Inspdate"];
                    this.CurrentMaintain["Remark"] = detailRow["remark"];
                }

                if (string.IsNullOrEmpty(this.CurrentMaintain["AddDate"].ToString()) && dt.Select("NO='1'").Count() > 0)
                {
                    DataRow detailRow = dt.Select(where)[0];
                    this.CurrentMaintain["AddDate"] = detailRow["AddDate"];
                    this.CurrentMaintain["AddName"] = detailRow["AddName"];
                }
            }
            else
            {
                this.CurrentMaintain["Result"] = string.Empty;
                this.CurrentMaintain["Inspdate"] = DBNull.Value;
                this.CurrentMaintain["Remark"] = string.Empty;
            }

            return base.ClickSave();
        }

        /// <inheritdoc/>
        protected override void OnEditModeChanged()
        {
            this.ReportNoCount = 0;
            base.OnEditModeChanged();
        }
    }
}
