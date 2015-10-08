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

namespace Sci.Production.IE
{
    public partial class P02 : Sci.Win.Tems.Input1
    {
        private string dateTimeMask = "", emptyDTMask = "", empmask, dtmask,type;
        public P02(ToolStripMenuItem menuitem, string Type)
            : base(menuitem)
        {
            InitializeComponent();
            type = Type;
            this.Text = type == "1" ? "P02. Style Changeover Monitor" : "P021. Style Changeover Monitor (History)";
            this.DefaultFilter = type == "1" ? string.Format("FactoryID = '{0}' AND Status <> 'Closed'", Sci.Env.User.Factory) : string.Format("FactoryID = '{0}' AND Status = 'Closed'", Sci.Env.User.Factory);
            //組InLine date的mask
            for (int i = 0; i < Sci.Env.Cfg.DateTimeStringFormat.Length; i++)
            {
                dtmask = Sci.Env.Cfg.DateTimeStringFormat.Substring(i, 1) == "/" || Sci.Env.Cfg.DateTimeStringFormat.Substring(i, 1) == ":" ? Sci.Env.Cfg.DateTimeStringFormat.Substring(i, 1) : Sci.Env.Cfg.DateTimeStringFormat.Substring(i, 1) == " " ? " " : "0";
                empmask = Sci.Env.Cfg.DateTimeStringFormat.Substring(i, 1) == "/" || Sci.Env.Cfg.DateTimeStringFormat.Substring(i, 1) == ":" ? Sci.Env.Cfg.DateTimeStringFormat.Substring(i, 1) : Sci.Env.Cfg.DateTimeStringFormat.Substring(i, 1) == "s" ? "" : " ";
                dateTimeMask = dateTimeMask + dtmask;
                emptyDTMask = emptyDTMask + empmask;
            }
            textBox4.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Inline", true, System.Windows.Forms.DataSourceUpdateMode.OnValidation, emptyDTMask, Sci.Env.Cfg.DateTimeStringFormat));
            textBox4.Mask = dateTimeMask;

            if (type == "2")
            {
                this.dateBox2.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "SewOutputDate1", true));
                this.dateBox3.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "SewOutputDate2", true));
                this.dateBox4.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "SewOutputDate3", true));
                this.numericBox6.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Efficiency1", true));
                this.numericBox8.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Efficiency2", true));
                this.numericBox10.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Efficiency3", true));
                this.numericBox7.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "RFT1", true));
                this.numericBox9.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "RFT2", true));
                this.numericBox11.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "RFT3", true));
            }
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            label18.Text = CurrentMaintain["Type"].ToString() == "N" ? "New" : "Repeat";
            if (type == "1")
            {
                string sqlCmd = string.Format(@"with tmpSO
as
(
select distinct s.OutputDate,s.Manpower,s.ID
from SewingOutput s, SewingOutput_Detail sd
where s.ID = sd.ID
and sd.OrderId = '{0}'
and sd.ComboType = '{1}'
and s.SewingLineID = '{2}'
and s.FactoryID = '{3}'
),
tmpDetailData
as
(
select s.OutputDate,iif(sd.QAQty = 0,s.Manpower,sd.QAQty*s.Manpower) as ActManP,
(sd.TMS*sd.QAQty) as ttlOutP,sd.QAQty,sd.WorkHour,System.StdTMS
from tmpSO s
left join SewingOutput_Detail sd on s.ID = sd.ID
left join System on 1=1
),
SummaryData
as
(
select OutputDate, sum(ActManP) as ActManP, sum(ttlOutP) as ttlOutP, sum(QAQty) as QAQty,
sum(WorkHour) as WorkHour,StdTMS
from tmpDetailData
group by OutputDate,StdTMS
)
select top (3) OutputDate,iif(QAQty*WorkHour = 0,0,Round(ttlOutP/(round(ActManP/QAQty*WorkHour,2)*3600)*100,1)) as Eff,
isnull((select top (1) iif(InspectQty = 0,0,Round((InspectQty-RejectQty)/InspectQty*100,2)) from RFT where OrderID = '{0}' and CDate = SummaryData.OutputDate and SewinglineID = '{2}'),0) as Rft
from SummaryData
order by OutputDate
", CurrentMaintain["OrderID"].ToString(), CurrentMaintain["ComboType"].ToString(), CurrentMaintain["SewingLineID"].ToString(), CurrentMaintain["FactoryID"].ToString());
                DataTable sewingOutput;
                DualResult result = DBProxy.Current.Select(null, sqlCmd, out sewingOutput);
                if (result)
                {
                    int rec = 0;
                    foreach (DataRow dr in sewingOutput.Rows)
                    {
                        rec += 1;
                        switch (rec)
                        {
                            case 1:
                                dateBox2.Value = Convert.ToDateTime(dr["OutputDate"]);
                                numericBox6.Value = Convert.ToDecimal(dr["Eff"]);
                                numericBox7.Value = Convert.ToDecimal(dr["Rft"]);
                                break;
                            case 2:
                                dateBox3.Value = Convert.ToDateTime(dr["OutputDate"]);
                                numericBox8.Value = Convert.ToDecimal(dr["Eff"]);
                                numericBox9.Value = Convert.ToDecimal(dr["Rft"]);
                                break;
                            case 3:
                                dateBox4.Value = Convert.ToDateTime(dr["OutputDate"]);
                                numericBox10.Value = Convert.ToDecimal(dr["Eff"]);
                                numericBox11.Value = Convert.ToDecimal(dr["Rft"]);
                                break;
                            default:
                                break;
                        }
                    }

                    if (rec < 3)
                    {
                        switch (rec)
                        {
                            case 0:
                                dateBox2.Value = null;
                                numericBox6.Value = 0;
                                numericBox7.Value = 0;
                                dateBox3.Value = null;
                                numericBox8.Value = 0;
                                numericBox9.Value = 0;
                                dateBox4.Value = null;
                                numericBox10.Value = 0;
                                numericBox11.Value = 0;
                                break;
                            case 1:
                                dateBox3.Value = null;
                                numericBox8.Value = 0;
                                numericBox9.Value = 0;
                                dateBox4.Value = null;
                                numericBox10.Value = 0;
                                numericBox11.Value = 0;
                                break;
                            case 2:
                                dateBox4.Value = null;
                                numericBox10.Value = 0;
                                numericBox11.Value = 0;
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }

        protected override bool ClickEditBefore()
        {
            if (!MyUtility.Check.Empty(CurrentMaintain["ApvDate"]))
            {
                MyUtility.Msg.WarningBox("This record is 'Approved', can't be modify!");
                return false;
            }
            return base.ClickEditBefore();
        }

        //Time of First/Last Good Output
        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            Sci.Win.UI.TextBox prodTextValue = (Sci.Win.UI.TextBox)sender;
            if (EditMode && !MyUtility.Check.Empty(prodTextValue.Text) && prodTextValue.Text != prodTextValue.OldValue)
            {
                string textValue = prodTextValue.Text.ToString().PadRight(4);
                if ((!MyUtility.Check.Empty(textValue.Substring(0, 2)) && Convert.ToInt32(textValue.Substring(0, 2)) > 24) || ((!MyUtility.Check.Empty(textValue.Substring(0, 2)) && Convert.ToInt32(textValue.Substring(0, 2)) == 24) && (!MyUtility.Check.Empty(textValue.Substring(2, 2)) && Convert.ToInt32(textValue.Substring(2, 2)) != 0)) || (!MyUtility.Check.Empty(textValue.Substring(2, 2)) && Convert.ToInt32(textValue.Substring(2, 2)) >= 60))
                {
                    MyUtility.Msg.WarningBox("The time format is wrong, can't exceed '24:00'!");
                    prodTextValue.Text = "";
                    e.Cancel = true;
                    return;
                }
                else
                {
                    string newValue = (MyUtility.Check.Empty(textValue.Substring(0, 2))?"00":Convert.ToInt32(textValue.Substring(0, 2)) < 10 ? "0" + Convert.ToInt32(textValue.Substring(0, 2)).ToString() : textValue.Substring(0, 2)) + (MyUtility.Check.Empty(textValue.Substring(2, 2))?"00":Convert.ToInt32(textValue.Substring(2, 2)) < 10 ? "0" + Convert.ToInt32(textValue.Substring(2, 2)).ToString() : textValue.Substring(2, 2));
                    prodTextValue.Text = newValue;
                }
            }
        }

        //FTY GSD
        private void button1_Click(object sender, EventArgs e)
        {
            Sci.Production.IE.P01 callNextForm = new Sci.Production.IE.P01(CurrentMaintain["StyleID"].ToString(), MyUtility.GetValue.Lookup(string.Format("select BrandID from Orders where ID = '{0}'", CurrentMaintain["OrderID"].ToString())), CurrentMaintain["SeasonID"].ToString(), CurrentMaintain["ComboType"].ToString());
            callNextForm.ShowDialog(this);
        }

        //Check List
        private void button2_Click(object sender, EventArgs e)
        {
            if (CurrentMaintain["Type"].ToString() == "N")
            {
                if (type == "1")
                {
                    if (!MyUtility.Check.Seek(string.Format("select ID from ChgOver_Check where ID = '{0}'", CurrentMaintain["ID"].ToString())))
                    {
                        string insertCmd = string.Format(@"insert into ChgOver_Check (ID,DayBe4Inline,BaseOn,ChgOverCheckListID)
select {0},DaysBefore,BaseOn,ID from ChgOverCheckList where (UseFor = 'N' or UseFor = 'A') and Junk = 0", CurrentMaintain["ID"].ToString());
                        DualResult result = DBProxy.Current.Execute(null, insertCmd);
                        if (!result)
                        {
                            MyUtility.Msg.ErrorBox("Insert ChgOver_Problem fail!!\r\n" + result.ToString());
                            return;
                        }
                    }
                    
                }
                Sci.Production.IE.P02_NewCheckList callNextForm = new Sci.Production.IE.P02_NewCheckList(CurrentMaintain["Status"].ToString() == "New", CurrentMaintain["ID"].ToString(), null, null);
                callNextForm.ShowDialog(this);
            }
            else
            {
                if (type == "1")
                {
                    if (!MyUtility.Check.Seek(string.Format("select ID from ChgOver_Check where ID = '{0}'", CurrentMaintain["ID"].ToString())))
                    {
                        string insertCmd = string.Format(@"insert into ChgOver_Check (ID,DayBe4Inline,BaseOn,ChgOverCheckListID)
select {0},DaysBefore,BaseOn,ID from ChgOverCheckList where (UseFor = 'R' or UseFor = 'A') and Junk = 0", CurrentMaintain["ID"].ToString());
                        DualResult result = DBProxy.Current.Execute(null, insertCmd);
                        if (!result)
                        {
                            MyUtility.Msg.ErrorBox("Insert ChgOver_Problem fail!!\r\n" + result.ToString());
                            return;
                        }
                    }

                }
                Sci.Production.IE.P02_RepeatCheckList callNextForm = new Sci.Production.IE.P02_RepeatCheckList(CurrentMaintain["Status"].ToString() == "New", CurrentMaintain["ID"].ToString(), null, null);
                callNextForm.ShowDialog(this);
            }
        }

        //Problem
        private void button3_Click(object sender, EventArgs e)
        {
            if (type == "1")
            {
                if (!MyUtility.Check.Seek(string.Format("select ID from ChgOver_Problem where ID = '{0}'", CurrentMaintain["ID"].ToString())))
                {
                    string insertCmd = string.Format(@"insert into ChgOver_Problem (ID,IEReasonID,AddName,AddDate)
select {0},ID,'{1}',GETDATE() from IEReason where Type = 'CP' and Junk = 0", CurrentMaintain["ID"].ToString(), Sci.Env.User.UserID);
                    DualResult result = DBProxy.Current.Execute(null, insertCmd);
                    if (!result)
                    {
                        MyUtility.Msg.ErrorBox("Insert ChgOver_Problem fail!!\r\n" + result.ToString());
                        return;
                    }
                }
                
            }
            Sci.Production.IE.P02_Problem callNextForm = new Sci.Production.IE.P02_Problem(CurrentMaintain["Status"].ToString() != "Closed", CurrentMaintain["ID"].ToString(), null, null);
            callNextForm.ShowDialog(this);
        }

        //Confirm
        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            string sqlCmd = string.Format("update ChgOver set Status = 'Approved', ApvDate = GETDATE(), EditName = '{0}', EditDate = GETDATE() where ID = '{1}'", Sci.Env.User.UserID, CurrentMaintain["ID"].ToString());

            DualResult result = DBProxy.Current.Execute(null, sqlCmd);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Confirm failed, Pleaes re-try");
            }

            RenewData();
            OnDetailEntered();
            EnsureToolbarExt();
        }

        //UnConfirm
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();

            string sqlCmd = string.Format("update ChgOver set Status = 'New', ApvDate = null, EditName = '{0}', EditDate = GETDATE() where ID = '{1}'", Sci.Env.User.UserID, CurrentMaintain["ID"].ToString());

            DualResult result = DBProxy.Current.Execute(null, sqlCmd);
            if (!result)
            {
                MyUtility.Msg.WarningBox("UnConfirm failed, Pleaes re-try");
            }

            RenewData();
            OnDetailEntered();
            EnsureToolbarExt();
        }
        
    }
}
