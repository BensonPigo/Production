using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;
using Sci.Production.PublicPrg;
using Sci.Win.Tools;

namespace Sci.Production.PPIC
{
    public partial class P08 : Sci.Win.Tems.Input6
    {
        public P08(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            DefaultFilter = string.Format("Type = 'F' and MDivisionID = '{0}'",Sci.Env.User.Keyword);
            InsertDetailGridOnDoubleClick = false;
        }

        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : MyUtility.Convert.GetString(e.Master["ID"]);
            this.DetailSelectCommand = string.Format(@"select rd.*,(left(rd.Seq1+' ',3)+rd.Seq2) as Seq, [dbo].[getMtlDesc](r.POID,rd.Seq1,rd.Seq2,2,0) as Description,
isnull((select top(1) ExportId from Receiving where InvNo = rd.INVNo),'') as ExportID,
CASE rd.Responsibility
WHEN 'M' THEN N'Mill'
WHEN 'S' THEN N'Subcon in Local'
WHEN 'F' THEN N'Factory'
WHEN 'T' THEN N'SCI dep. (purchase / s. mrs / sample room)'
ELSE N''
END as CategoryName
from ReplacementReport r
inner join ReplacementReport_Detail rd on rd.ID = r.ID
where r.ID = '{0}'", masterID);

            return base.OnDetailSelectCommandPrepare(e);
        }

        protected override void EnsureToolbarExt()
        {
            base.EnsureToolbarExt();
            toolbar.cmdJunk.Enabled = !EditMode && CurrentMaintain != null && MyUtility.Convert.GetString(CurrentMaintain["Status"]) != "Junked" && PublicPrg.Prgs.GetAuthority(MyUtility.Convert.GetString(CurrentMaintain["ApplyName"])) && MyUtility.Check.Empty(CurrentMaintain["ApplyDate"]) ? true : false;
            toolbar.cmdCheck.Enabled = !EditMode && CurrentMaintain != null && MyUtility.Convert.GetString(CurrentMaintain["Status"]) == "New" && PublicPrg.Prgs.GetAuthority(MyUtility.Convert.GetString(CurrentMaintain["ApplyName"])) ? true : false;
            toolbar.cmdUncheck.Enabled = !EditMode && CurrentMaintain != null && MyUtility.Convert.GetString(CurrentMaintain["Status"]) == "Checked" && PublicPrg.Prgs.GetAuthority(MyUtility.Convert.GetString(CurrentMaintain["ApplyName"])) ? true : false;
            toolbar.cmdConfirm.Enabled = !EditMode && CurrentMaintain != null && MyUtility.Convert.GetString(CurrentMaintain["Status"]) == "Checked" && PublicPrg.Prgs.GetAuthority(MyUtility.Convert.GetString(CurrentMaintain["ApvName"])) ? true : false;
            toolbar.cmdUnconfirm.Enabled = !EditMode && CurrentMaintain != null && MyUtility.Convert.GetString(CurrentMaintain["Status"]) == "Approved" && PublicPrg.Prgs.GetAuthority(MyUtility.Convert.GetString(CurrentMaintain["ApvName"])) && MyUtility.Check.Empty(CurrentMaintain["TPECFMDate"]) ? true : false;
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();

            button1.Enabled = !EditMode && CurrentMaintain != null && MyUtility.Convert.GetString(CurrentMaintain["Status"]) != "Junked" && !MyUtility.Check.Empty(CurrentMaintain["ApvDate"]) ? true : false;
            label15.Visible = MyUtility.Convert.GetString(CurrentMaintain["Status"]) == "Junked";
            displayBox4.Value = MyUtility.GetValue.Lookup("StyleID", MyUtility.Convert.GetString(CurrentMaintain["POID"]), "Orders", "ID");
            displayBox5.Value = MyUtility.Check.Empty(CurrentMaintain["ApplyDate"]) ? "" : Convert.ToDateTime(CurrentMaintain["ApplyDate"]).ToString(string.Format("{0}", Sci.Env.Cfg.DateTimeStringFormat));
            displayBox6.Value = MyUtility.Check.Empty(CurrentMaintain["ApvDate"]) ? "" : Convert.ToDateTime(CurrentMaintain["ApvDate"]).ToString(string.Format("{0}", Sci.Env.Cfg.DateTimeStringFormat));
            displayBox7.Value = MyUtility.Check.Empty(CurrentMaintain["TPECFMDate"]) ? "" : Convert.ToDateTime(CurrentMaintain["TPECFMDate"]).ToString(string.Format("{0}", Sci.Env.Cfg.DateTimeStringFormat));
            displayBox2.Value = MyUtility.Check.Empty(CurrentMaintain["TPEEditDate"]) ? "" : Convert.ToDateTime(CurrentMaintain["TPEEditDate"]).ToString(string.Format("{0}", Sci.Env.Cfg.DateTimeStringFormat));
            DataRow POData;
            if (MyUtility.Check.Seek(string.Format("select POSMR,POHandle,PCSMR,PCHandle from PO where ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["POID"])), out POData))
            {
                txttpeuser1.DisplayBox1Binding = MyUtility.Convert.GetString(POData["POSMR"]);
                txttpeuser2.DisplayBox1Binding = MyUtility.Convert.GetString(POData["POHandle"]);
                txttpeuser4.DisplayBox1Binding = MyUtility.Convert.GetString(POData["PCSMR"]);
                txttpeuser5.DisplayBox1Binding = MyUtility.Convert.GetString(POData["PCHandle"]);
            }
            else
            {
                txttpeuser1.DisplayBox1Binding = "";
                txttpeuser2.DisplayBox1Binding = "";
                txttpeuser4.DisplayBox1Binding = "";
                txttpeuser5.DisplayBox1Binding = "";
            }
        }

        protected override void OnDetailGridSetup()
        {
            DataGridViewGeneratorNumericColumnSettings estinqty = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings actinqty = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings ttlrequest = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings cturequest = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings occurcost = new DataGridViewGeneratorNumericColumnSettings();
            estinqty.CellZeroStyle = Ict.Win.UI.DataGridViewNumericBoxZeroStyle.Empty;
            actinqty.CellZeroStyle = Ict.Win.UI.DataGridViewNumericBoxZeroStyle.Empty;
            ttlrequest.CellZeroStyle = Ict.Win.UI.DataGridViewNumericBoxZeroStyle.Empty;
            cturequest.CellZeroStyle = Ict.Win.UI.DataGridViewNumericBoxZeroStyle.Empty;
            occurcost.CellZeroStyle = Ict.Win.UI.DataGridViewNumericBoxZeroStyle.Empty;

            base.OnDetailGridSetup();
            Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("Seq", header: "SEQ#", width: Widths.AnsiChars(5), iseditingreadonly: true)
            .Text("RefNo", header: "Refno", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true)
            .Text("INVNo", header: "Invoice#", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Date("ETA", header: "ETA", iseditingreadonly: true)
            .Text("ColorID", header: "Color Code", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Numeric("EstInQty", header: "Est. Rced\r\nQ'ty", decimal_places: 2, width: Widths.AnsiChars(7), settings: estinqty, iseditingreadonly: true)
            .Numeric("ActInQty", header: "Actual Rced\r\nQ'ty", decimal_places: 2, width: Widths.AnsiChars(7), settings: actinqty, iseditingreadonly: true)
            .Numeric("TotalRequest", header: "Inspection Total\r\nReplacement Request\r\nQty", decimal_places: 2, width: Widths.AnsiChars(7), settings: ttlrequest, iseditingreadonly: true)
            .Numeric("AfterCuttingRequest", header: "After Cutting\r\nReplacement\r\nRequest Qty", decimal_places: 2, width: Widths.AnsiChars(7), settings: cturequest, iseditingreadonly: true)
            .Date("DamageSendDate", header: "Damage\r\nSample Sent\r\nDate", iseditingreadonly: true)
            .Text("AWBNo", header: "AWB# Of\r\nDamage\r\nSample", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Date("ReplacementETA", header: "Replacement\r\nETA", iseditingreadonly: true)
            .Numeric("OccurCost", header: "Cost Occurred", decimal_places: 3, width: Widths.AnsiChars(7), settings: occurcost, iseditingreadonly: true)
            .Text("CategoryName", header: "Defect\r\nResponsibility", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .EditText("ResponsibilityReason", header: "Reason", width: Widths.AnsiChars(20), iseditingreadonly: true)
            .EditText("Suggested", header: "Factory Suggested Solution", width: Widths.AnsiChars(30), iseditingreadonly: true);

            detailgrid.CellDoubleClick += (s, e) =>
            {
                if (e.ColumnIndex == 0)
                {
                    Sci.Production.PPIC.P08_InputData callInputDataForm = new Sci.Production.PPIC.P08_InputData(CurrentMaintain);
                    callInputDataForm.Set(this.EditMode, this.DetailDatas, this.CurrentDetailData);
                    callInputDataForm.ShowDialog(this);
                }
            };

        }

        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["MDivisionID"] = Sci.Env.User.Keyword;
            CurrentMaintain["CDate"] = DateTime.Today;
            CurrentMaintain["Status"] = "New";
            CurrentMaintain["Type"] = "F";
        }

        protected override bool ClickEditBefore()
        {
            if (!MyUtility.Check.Empty(CurrentMaintain["ApvDate"]))
            {
                MyUtility.Msg.WarningBox("This record is approved, can't be modified!!");
                return false;
            }

            if (MyUtility.Convert.GetString(CurrentMaintain["Status"]) == "Junked")
            {
                MyUtility.Msg.WarningBox("This record is junked, can't be modified!!");
                return false;
            }
            
            return true;
        }

        protected override bool ClickSaveBefore()
        {
            #region 檢查必輸欄位
            if (MyUtility.Check.Empty(CurrentMaintain["POID"]))
            {
                MyUtility.Msg.WarningBox("SP No. can't empty");
                textBox1.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["ApplyName"]))
            {
                MyUtility.Msg.WarningBox("Prepared by can't empty");
                txtuser1.TextBox1.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["ApvName"]))
            {
                MyUtility.Msg.WarningBox("PPIC/Factory mgr can't empty");
                txtuser2.TextBox1.Focus();
                return false;
            }

            #endregion
            int count = 0; //紀錄表身筆數
            //刪除表身Grid的Seq為空資料
            foreach (DataRow dr in DetailDatas)
            {
                if (MyUtility.Check.Empty(dr["Seq"]))
                {
                    dr.Delete();
                    continue;
                }
                count++;
            }

            if (count == 0)
            {
                MyUtility.Msg.WarningBox("Deatil can't empty!!");
                return false;
            }

            //GetID
            if (IsDetailInserting)
            {
                string id = MyUtility.GetValue.GetID(MyUtility.GetValue.Lookup("KeyWord",MyUtility.GetValue.Lookup("FtyGroup", CurrentMaintain["POID"].ToString(), "Orders", "ID"),"Factory","ID") + MyUtility.Convert.GetString(CurrentMaintain["POID"]).Substring(0, 8), "ReplacementReport", DateTime.Today, 6, "Id", null);
                if (MyUtility.Check.Empty(id))
                {
                    MyUtility.Msg.WarningBox("GetID fail, please try again!");
                    return false;
                }
                CurrentMaintain["ID"] = id;
            }
            return true;
        }

        //SP No.
        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            if (EditMode)
            {
                if (!MyUtility.Check.Empty(textBox1.Text) && textBox1.OldValue != textBox1.Text)
                {
                    //sql參數
                    System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter("@poid", textBox1.Text);
                    System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter("@mdivisionid", Sci.Env.User.Keyword);

                    IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                    cmds.Add(sp1);
                    cmds.Add(sp2);
                    string sqlCmd = "select ID,FtyGroup from Orders where POID = @poid and MDivisionID = @mdivisionid";
                    DataTable OrdersData;
                    DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out OrdersData);

                    if (!result || OrdersData.Rows.Count <= 0)
                    {
                        if (!result)
                        {
                            MyUtility.Msg.WarningBox("Sql connection fail!!\r\n" + result.ToString());
                        }
                        else
                        {
                            MyUtility.Msg.WarningBox("SP No. not found!!");
                        }
                        CurrentMaintain["POID"] = "";
                        CurrentMaintain["FactoryID"] = "";
                        e.Cancel = true;
                        return;
                    }
                    else
                    {
                        CurrentMaintain["POID"] = textBox1.Text;
                        CurrentMaintain["FactoryID"] = OrdersData.Rows[0]["FtyGroup"];
                    }
                }
            }
        }

        //SP No.
        private void textBox1_Validated(object sender, EventArgs e)
        {
            if (EditMode && !MyUtility.Check.Empty(textBox1.Text) && textBox1.OldValue != textBox1.Text)
            {
                //清空表身Grid資料
                foreach (DataRow dr in DetailDatas)
                {
                    dr.Delete();
                }

                string sqlCmd = string.Format(@"select f.Seq1,f.Seq2, left(f.Seq1+' ',3)+f.Seq2 as Seq,f.Refno,
[dbo].getMtlDesc(f.POID,f.Seq1,f.Seq2,2,0) as Description,
isnull(psd.ColorID,'') as ColorID,isnull(r.InvNo,'') as InvNo,iif(e.Eta is null,r.ETA,e.ETA) as ETA,isnull(r.ExportId,'') as ExportId,
isnull(sum(fp.TicketYds),0) as EstInQty, isnull(sum(fp.ActualYds),0) as ActInQty
from FIR f
left join FIR_Physical fp on f.ID = fp.ID
left join PO_Supp_Detail psd on f.POID = psd.ID and f.Seq1 = psd.SEQ1 and f.Seq2 = psd.SEQ2
left join Receiving r on f.ReceivingID = r.Id
left join Export e on r.ExportId = e.ID
where f.POID = '{0}' and f.Result = 'F'
group by f.Seq1,f.Seq2, left(f.Seq1+' ',3)+f.Seq2,f.Refno,[dbo].getMtlDesc(f.POID,f.Seq1,f.Seq2,2,0),psd.ColorID,r.InvNo,iif(e.Eta is null,r.ETA,e.ETA),isnull(r.ExportId,'')", textBox1.Text);
                DataTable FIRData;
                DualResult result = DBProxy.Current.Select(null, sqlCmd, out FIRData);
                if (!result)
                {
                    MyUtility.Msg.WarningBox("Query FIR fail!\r\n" + result.ToString());
                }
                else
                {
                    foreach (DataRow dr in FIRData.Rows)
                    {
                        dr.AcceptChanges();
                        dr.SetAdded();
                        ((DataTable)detailgridbs.DataSource).ImportRow(dr);
                    }
                }
            }
        }

        //Junk
        protected override void ClickJunk()
        {
            base.ClickJunk();
            DualResult result;
            string updateCmd = string.Format("update ReplacementReport set Status = 'Junked', EditName = '{0}', EditDate = GETDATE() where ID = '{1}'", Sci.Env.User.UserID, MyUtility.Convert.GetString(CurrentMaintain["ID"]));
            result = DBProxy.Current.Execute(null, updateCmd);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Junk fail!\r\n" + result.ToString());
                return;
            }
            result = RenewData();
            OnDetailEntered();
            EnsureToolbarExt();
        }

        //Check
        protected override void ClickCheck()
        {
            base.ClickCheck();
            StringBuilder check = new StringBuilder();
            IList<string> updateCmds = new List<string>();
            foreach (DataRow dr in ((DataTable)detailgridbs.DataSource).Rows)
            {
                if (MyUtility.Check.Empty(dr["Responsibility"]) || MyUtility.Check.Empty(dr["ResponsibilityReason"]) || MyUtility.Check.Empty(dr["Suggested"]))
                {
                    check.Append(string.Format("SEQ# {0}\r\n",MyUtility.Convert.GetString(dr["Seq"])));
                }
                updateCmds.Add(string.Format(@"update FIR set ReplacementReportID = '{0}' where ReceivingID in (select distinct r.Id
from ReplacementReport rr
inner join ReplacementReport_Detail rrd on rr.ID = rrd.ID
inner join Receiving r on rrd.INVNo = r.InvNo
inner join Receiving_Detail rd on rd.Id = r.Id and rr.POID = rd.PoId and rrd.Seq1 = rd.Seq1 and rrd.Seq2 = rd.Seq2
where rr.ID = '{0}') and POID = '{1}' and Seq1 = '{2}' and Seq2 = '{3}' and ReplacementReportID = '';",MyUtility.Convert.GetString(CurrentMaintain["ID"]),MyUtility.Convert.GetString(CurrentMaintain["POID"]),MyUtility.Convert.GetString(dr["Seq1"]),MyUtility.Convert.GetString(dr["Seq2"])));
            }
            if (check.Length > 0)
            {
                MyUtility.Msg.WarningBox(check.ToString() + "<Defect Responsibility> and <Reason> and <Factory Suggested Solution> can't empty!!");
                return;
            }

            updateCmds.Add(string.Format("update ReplacementReport set Status = 'Checked', ApplyDate = GETDATE(), EditName = '{0}', EditDate = GETDATE() where ID = '{1}';", Sci.Env.User.UserID, MyUtility.Convert.GetString(CurrentMaintain["ID"])));

            DualResult result = DBProxy.Current.Executes(null, updateCmds);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Check fail!\r\n" + result.ToString());
                return;
            }
            result = RenewData();
            OnDetailEntered();
            EnsureToolbarExt();
        }

        //Uncheck
        protected override void ClickUncheck()
        {
            base.ClickUncheck();
            DialogResult confirmResult;
            confirmResult = MyUtility.Msg.QuestionBox("Are you sure you want to uncheck this data?", caption: "Confirm", buttons: MessageBoxButtons.YesNo);
            if (confirmResult != System.Windows.Forms.DialogResult.Yes)
            {
                return;
            }
            IList<string> updateCmds = new List<string>();
            updateCmds.Add(string.Format("update ReplacementReport set Status = 'New', ApplyDate = Null, EditName = '{0}', EditDate = GETDATE() where ID = '{1}';", Sci.Env.User.UserID, MyUtility.Convert.GetString(CurrentMaintain["ID"])));
            updateCmds.Add(string.Format(@"update FIR 
set ReplacementReportID = isnull((select top(1) rr.ID
						   from ReplacementReport rr
						   inner join ReplacementReport_Detail rrd on rr.ID = rrd.ID
						   where rr.CDate = (select MIN(r.CDate)
											 from ReplacementReport r
											 inner join ReplacementReport_Detail rd on r.ID = rd.ID
											 inner join Receiving rc on rd.INVNo = rc.InvNo
											 where r.ID != '{0}' and r.ApplyDate is not null and r.POID = FIR.POID and rd.Seq1 = FIR.Seq1 and rd.Seq2 = FIR.Seq2 and rc.Id = FIR.ReceivingID)
						   and rr.ID != '{0}' and rr.ApplyDate is not null and rr.POID = FIR.POID and rrd.Seq1 = FIR.Seq1 and rrd.Seq2 = FIR.Seq2),'') 
where ReplacementReportID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["ID"])));

            DualResult result = DBProxy.Current.Executes(null, updateCmds);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Uncheck fail!\r\n" + result.ToString());
                return;
            }
            result = RenewData();
            OnDetailEntered();
            EnsureToolbarExt();
        }

        //Confirm
        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            DualResult result;
            string updateCmd = string.Format("update ReplacementReport set Status = 'Approved', ApvDate = GETDATE(), EditName = '{0}', EditDate = GETDATE() where ID = '{1}'", Sci.Env.User.UserID, MyUtility.Convert.GetString(CurrentMaintain["ID"]));
            result = DBProxy.Current.Execute(null, updateCmd);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Confirm fail!\r\n" + result.ToString());
                return;
            }
            result = RenewData();
            OnDetailEntered();
            EnsureToolbarExt();

            //SendMail
        }

        //Unconfirm
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            DialogResult confirmResult;
            confirmResult = MyUtility.Msg.QuestionBox("Are you sure you want to unconfirm this data?", caption: "Confirm", buttons: MessageBoxButtons.YesNo);
            if (confirmResult != System.Windows.Forms.DialogResult.Yes)
            {
                return;
            }

            DualResult result;
            string updateCmd = string.Format("update ReplacementReport set Status = 'Checked', ApvDate = null, EditName = '{0}', EditDate = GETDATE() where ID = '{1}'", Sci.Env.User.UserID, MyUtility.Convert.GetString(CurrentMaintain["ID"]));
            result = DBProxy.Current.Execute(null, updateCmd);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Unconfirm fail!\r\n" + result.ToString());
                return;
            }
            result = RenewData();
            OnDetailEntered();
            EnsureToolbarExt();
        }

        //Mail to
        private void button1_Click(object sender, EventArgs e)
        {
            SendMail();
        }

        // Mail to
        private void SendMail()
        {
                DataTable allMail;
                string sqlCmd = string.Format(@"select isnull((select EMail from Pass1 where ID = r.ApplyName),'') as ApplyName,
isnull((select Name from Pass1 where ID = r.ApvName),'') as ApvName,
isnull((select EMail from TPEPass1 where ID = o.MRHandle),'') as MRHandle,
isnull((select EMail from TPEPass1 where ID = o.SMR),'') as SMR,
isnull((select EMail from TPEPass1 where ID = p.POHandle),'') as POHandle,
isnull((select EMail from TPEPass1 where ID = p.POSMR),'') as POSMR,
isnull((select EMail from TPEPass1 where ID = p.PCHandle),'') as PCHandle,
isnull((select EMail from TPEPass1 where ID = p.PCSMR),'') as PCSMR
from ReplacementReport r
left join Orders o on o.ID = r.POID
left join PO p on p.ID = o.POID
where r.ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["ID"]));
                DualResult result = DBProxy.Current.Select(null, sqlCmd, out allMail);
                if (!result)
                {
                    MyUtility.Msg.ErrorBox("Query mail list fail.\r\n" + result.ToString());
                    return;
                }

                string mailto = MyUtility.Convert.GetString(allMail.Rows[0]["POSMR"]) + ";" + MyUtility.Convert.GetString(allMail.Rows[0]["POHandle"]) + ";" + MyUtility.Convert.GetString(allMail.Rows[0]["PCSMR"]) + ";" + MyUtility.Convert.GetString(allMail.Rows[0]["PCHandle"]) + ";" + MyUtility.Convert.GetString(allMail.Rows[0]["SMR"]) + ";" + MyUtility.Convert.GetString(allMail.Rows[0]["MRHandle"]) + ";";
                string cc = MyUtility.Convert.GetString(allMail.Rows[0]["ApplyName"]) + ";" + MyUtility.Convert.GetString(allMail.Rows[0]["ApvName"]) + ";";
                string subject = string.Format("{0} - Fabric Replacement report",MyUtility.Convert.GetString(CurrentMaintain["ID"]));
                StringBuilder content = new StringBuilder();
                #region 組Content
                content.Append(@"Hi PO Handle,

Please refer attached replacement report and confirm rcvd in reply. The defect sample will send via courier AWB#      on     .please clarify with supplier and advise the result. Thanks.
If the replacement report can be accept and cfm to proceed, please approve it through system

");
                if (true) //當To Excel的檔案與迴紋針裡的檔案加起來超過10MB的話，就在信件中顯示下面訊息，附件只夾To Excel的檔案
                {
                    content.Append("Due to the attach files is more than 10MB, please ask factory's related person to provide the attach file.");
                }
                #endregion

                var email = new MailTo(Sci.Env.User.MailAddress, mailto, cc, subject, "", content.ToString(), false, true);
                email.ShowDialog(this);
        }
    }
}
