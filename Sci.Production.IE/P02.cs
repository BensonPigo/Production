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
using System.Runtime.InteropServices;


namespace Sci.Production.IE
{
    public partial class P02 : Sci.Win.Tems.Input1
    {
        private string dateTimeMask = "", emptyDTMask = "", empmask, dtmask, type;
        public P02(ToolStripMenuItem menuitem, string Type)
            : base(menuitem)
        {
            InitializeComponent();
            type = Type;
            this.Text = type == "1" ? "P02. Style Changeover Monitor" : "P021. Style Changeover Monitor (History)";
            this.DefaultFilter = type == "1" ? string.Format("MDivisionID = '{0}' AND Status <> 'Closed'", Sci.Env.User.Keyword) : string.Format("MDivisionID = '{0}' AND Status = 'Closed'", Sci.Env.User.Keyword);
            if (type == "2")
           {
               this.IsSupportEdit = false;
            }
            
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
            
        }


        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            label18.Text = CurrentMaintain["Type"].ToString() == "N" ? "New" : "Repeat";
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
                    string newValue = (MyUtility.Check.Empty(textValue.Substring(0, 2)) ? "00" : Convert.ToInt32(textValue.Substring(0, 2)) < 10 ? "0" + Convert.ToInt32(textValue.Substring(0, 2)).ToString() : textValue.Substring(0, 2)) + (MyUtility.Check.Empty(textValue.Substring(2, 2)) ? "00" : Convert.ToInt32(textValue.Substring(2, 2)) < 10 ? "0" + Convert.ToInt32(textValue.Substring(2, 2)).ToString() : textValue.Substring(2, 2));
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
                        //sql參數
                        System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter();
                        System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter();
                        sp1.ParameterName = "@id";
                        sp1.Value = CurrentMaintain["ID"].ToString();
                        sp2.ParameterName = "@orderid";
                        sp2.Value = CurrentMaintain["OrderID"].ToString();

                        IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                        cmds.Add(sp1);
                        cmds.Add(sp2);

                        string insertCmd = @"insert into ChgOver_Check (ID,DayBe4Inline,BaseOn,ChgOverCheckListID)
select @id,DaysBefore,BaseOn,ID from ChgOverCheckList where (UseFor = 'N' or UseFor = 'A') and BrandID = (select BrandID from Orders where ID = @orderid) and Junk = 0";
                        DualResult result = DBProxy.Current.Execute(null, insertCmd, cmds);
                        if (!result)
                        {
                            MyUtility.Msg.ErrorBox("Insert ChgOver_CheckList fail!!\r\n" + result.ToString());
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
                        //sql參數
                        System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter();
                        System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter();
                        sp1.ParameterName = "@id";
                        sp1.Value = CurrentMaintain["ID"].ToString();
                        sp2.ParameterName = "@orderid";
                        sp2.Value = CurrentMaintain["OrderID"].ToString();

                        IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                        cmds.Add(sp1);
                        cmds.Add(sp2);

                        string insertCmd = @"insert into ChgOver_Check (ID,DayBe4Inline,BaseOn,ChgOverCheckListID)
select @id,DaysBefore,BaseOn,ID from ChgOverCheckList where (UseFor = 'R' or UseFor = 'A') and BrandID = (select BrandID from Orders where ID = @orderid) and Junk = 0";
                        DualResult result = DBProxy.Current.Execute(null, insertCmd, cmds);
                        if (!result)
                        {
                            MyUtility.Msg.ErrorBox("Insert ChgOver_CheckList fail!!\r\n" + result.ToString());
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

        //Print
        protected override bool ClickPrint()
        {
            ToExcel(false);
            return base.ClickPrint();
        }

        private bool ToExcel(bool autoSave)
        {
            DataTable dtTitle;
            string OrderID = CurrentMaintain["OrderID"].ToString().Trim();
            string BrandID = MyUtility.GetValue.Lookup(string.Format("select BrandID from Orders where ID = '{0}'", OrderID));
            string StyleID = CurrentMaintain["StyleID"].ToString().Trim();
            string SeasonID = CurrentMaintain["SeasonID"].ToString().Trim();
            string CPU = MyUtility.GetValue.Lookup(string.Format("select CPU from Style where BrandID='{0}' and ID='{1}' and SeasonID='{2}'", BrandID, StyleID, SeasonID));
            string FirstOutputTime = CurrentMaintain["FirstOutputTime"].ToString().Trim();
            FirstOutputTime = MyUtility.Check.Empty(FirstOutputTime) ? "" : FirstOutputTime.Substring(0, 2) + ":" + FirstOutputTime.Substring(2, 2);
            string LastOutputTime = CurrentMaintain["LastOutputTime"].ToString().Trim();
            LastOutputTime = MyUtility.Check.Empty(LastOutputTime) ? "" : LastOutputTime.Substring(0, 2) + ":" + LastOutputTime.Substring(2, 2);
            string FactoryID = CurrentMaintain["FactoryID"].ToString().Trim();
            string SewingLineID = CurrentMaintain["SewingLineID"].ToString().Trim();
            string ComboType = CurrentMaintain["ComboType"].ToString().Trim();

            #region 取出ChgOverTarget.Target，然後再依ChgOver.Inline找出最接近但沒有超過這一天的Target
            string MDivisionID = CurrentMaintain["MDivisionID"].ToString().Trim();
            DateTime Inline = Convert.ToDateTime(CurrentMaintain["Inline"]);
            string Target_COPT = MyUtility.GetValue.Lookup(string.Format(@"Select top 1 Target from ChgOverTarget 
                                                                        where Type = 'COPT' and MDivisionID = '{0}' and EffectiveDate <= '{1}' 
                                                                        Order by EffectiveDate desc", MDivisionID, Inline.ToShortDateString()));
            string Target_COT = MyUtility.GetValue.Lookup(string.Format(@"Select top 1 Target from ChgOverTarget 
                                                                        where Type = 'COT' and MDivisionID = '{0}' and EffectiveDate <= '{1}' 
                                                                        Order by EffectiveDate desc", MDivisionID, Inline.ToShortDateString()));
            #endregion

            string CDCodeID = CurrentMaintain["CDCodeID"].ToString().Trim();
            DataRow TYPE = GetType(ComboType, CDCodeID);

            #region 找出上一筆
            DataRow PreviousDR = null;
            DataRow Previous_TYPE = null;
            string Previous_CPU = string.Empty;
            DataTable dt = (DataTable)gridbs.DataSource;
            int index = dt.Rows.IndexOf(CurrentMaintain);
            if (index > 0)
            {
                PreviousDR = dt.Rows[index - 1];
                string Previous_BrandID = MyUtility.GetValue.Lookup(string.Format("select BrandID from Orders where ID = '{0}'", PreviousDR["OrderID"].ToString().Trim()));
                string Previous_StyleID = PreviousDR["StyleID"].ToString().Trim();
                string Previous_SeasonID = PreviousDR["SeasonID"].ToString().Trim();
                Previous_CPU = MyUtility.GetValue.Lookup(string.Format("select CPU from Style where BrandID='{0}' and ID='{1}' and SeasonID='{2}'", Previous_BrandID, Previous_StyleID, Previous_SeasonID));
                Previous_TYPE = GetType(PreviousDR["ComboType"].ToString().Trim(), PreviousDR["CDCodeID"].ToString().Trim());
            }
            #endregion

            string cmdsql = string.Format("SELECT TOP 1 'CHANGEOVER REPORT'  FROM ChgOver where 1=1");
            DualResult dResult = DBProxy.Current.Select(null, cmdsql, out dtTitle);

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\IE_P02_ChangeoverReport.xlt"); //預先開啟excel app

            if (MyUtility.Excel.CopyToXls(dtTitle, "", "IE_P02_ChangeoverReport.xlt", 2, !autoSave, null, objApp, false))
            {// 將datatable copy to excel
                objApp.Visible = false;  //隱藏，避免使用者誤按
                Microsoft.Office.Interop.Excel._Worksheet objSheet = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
                Microsoft.Office.Interop.Excel._Workbook objBook = objApp.ActiveWorkbook;

                objSheet.Cells[9, 4] = FactoryID;  //Factory
                objSheet.Cells[9, 7] = SewingLineID;  //Line No.
                objSheet.Cells[9, 11] = CurrentMaintain["CellNo2"].ToString().Trim();  //Cell No.
                objSheet.Cells[12, 4] = StyleID;  //Style No.
                objSheet.Cells[13, 4] = CPU;  //CPU/pc
                objSheet.Cells[14, 4] = CDCodeID;  //CD Code
                objSheet.Cells[14, 5] = TYPE["ProdType"];  //Prod. Type
                objSheet.Cells[14, 6] = TYPE["FabricType"];  //Fab. Type

                #region 抓上一筆資料
                if (PreviousDR != null)
                {
                    objSheet.Cells[12, 9] = PreviousDR["StyleID"].ToString().Trim();  //Style No.
                    objSheet.Cells[13, 9] = Previous_CPU;  //CPU/pc
                    objSheet.Cells[14, 9] = PreviousDR["CDCodeID"].ToString().Trim();  //CD Code
                }
                if (Previous_TYPE != null)
                {
                    objSheet.Cells[14, 10] = Previous_TYPE["ProdType"];  //Prod. Type
                    objSheet.Cells[14, 11] = Previous_TYPE["FabricType"];  //Fab. Type
                }
                #endregion

                objSheet.Cells[18, 5] = CurrentMaintain["Type"].ToString() == "N" ? "New" : "Repeat";  //Classification
                objSheet.Cells[19, 4] = CurrentMaintain["Category"].ToString().Trim();  //Category
                objSheet.Cells[20, 4] = Inline.ToShortDateString();  //Inline Date
                objSheet.Cells[21, 5] = Inline.ToString("hh:mm");  //Inline Time(hh:mm)
                objSheet.Cells[22, 6] = FirstOutputTime;  //Time of First Good Output (hh:mm):
                objSheet.Cells[22, 11] = LastOutputTime;  //Time of Last Good Output (hh:mm):

                objSheet.Cells[27, 3] = Target_COPT;  //Target
                objSheet.Cells[27, 8] = Target_COT;  //Target
                objSheet.Cells[27, 4] = CurrentMaintain["COPT"].ToString().Trim();  //Actual
                objSheet.Cells[27, 9] = CurrentMaintain["COT"].ToString().Trim();  //Actual

                #region 32列
                string SewingDate = MyUtility.GetValue.Lookup(string.Format(@"select convert(varchar, min(a.OutputDate), 111) as SewingDate
                                                                        from SewingOutput a
                                                                        left join SewingOutput_Detail b on a.ID=b.ID
                                                                        where b.OrderId='{0}'", OrderID));
                string Sewers_A = MyUtility.GetValue.Lookup(string.Format(@"Select Distinct a.Manpower 
                                                                        from SewingOutput a, SewingOutput_Detail b 
                                                                        where a.Id = b.ID and a.OutputDate = '{0}' and a.Team = 'A' and a.FactoryID = '{1}' 
                                                                        and a.SewingLineID = '{2}' and b.OrderID = '{3}' and b.ComboType = '{4}'", SewingDate, FactoryID, SewingLineID, OrderID, ComboType));
                string Sewers_B = MyUtility.GetValue.Lookup(string.Format(@"Select Distinct a.Manpower 
                                                                        from SewingOutput a, SewingOutput_Detail b 
                                                                        where a.Id = b.ID and a.OutputDate = '{0}' and a.Team = 'B' and a.FactoryID = '{1}' 
                                                                        and a.SewingLineID = '{2}' and b.OrderID = '{3}' and b.ComboType = '{4}'", SewingDate, FactoryID, SewingLineID, OrderID, ComboType));
                string WorkingHours_A = MyUtility.GetValue.Lookup(string.Format(@"Select Sum(b.WorkHour)  
                                                                        from SewingOutput a, SewingOutput_Detail b 
                                                                        where a.Id = b.ID and a.OutputDate = '{0}' and a.Team = 'A' and a.FactoryID = '{1}' 
                                                                        and a.SewingLineID = '{2}' and b.OrderID = '{3}' and b.ComboType = '{4}'", SewingDate, FactoryID, SewingLineID, OrderID, ComboType));
                string WorkingHours_B = MyUtility.GetValue.Lookup(string.Format(@"Select Sum(b.WorkHour)  
                                                                        from SewingOutput a, SewingOutput_Detail b 
                                                                        where a.Id = b.ID and a.OutputDate = '{0}' and a.Team = 'B' and a.FactoryID = '{1}' 
                                                                        and a.SewingLineID = '{2}' and b.OrderID = '{3}' and b.ComboType = '{4}'", SewingDate, FactoryID, SewingLineID, OrderID, ComboType));
                string Target = MyUtility.GetValue.Lookup(string.Format(@"Select top 1 Target from ChgOverTarget 
                                                                        where Type = 'EFF.' and MDivisionID = '{0}' and EffectiveDate <= '{1}' 
                                                                        Order by EffectiveDate desc", MDivisionID, Inline.ToShortDateString()));
                string OutputCMP_A = MyUtility.GetValue.Lookup(string.Format(@"Select Sum(b.QAQty)  
                                                                        from SewingOutput a, SewingOutput_Detail b 
                                                                        where a.Id = b.ID and a.OutputDate = '{0}' and a.Team = 'A' and a.FactoryID = '{1}' 
                                                                        and a.SewingLineID = '{2}' and b.OrderID = '{3}' and b.ComboType = '{4}'", SewingDate, FactoryID, SewingLineID, OrderID, ComboType));
                string OutputCMP_B = MyUtility.GetValue.Lookup(string.Format(@"Select Sum(b.QAQty) 
                                                                        from SewingOutput a, SewingOutput_Detail b 
                                                                        where a.Id = b.ID and a.OutputDate = '{0}' and a.Team = 'B' and a.FactoryID = '{1}' 
                                                                        and a.SewingLineID = '{2}' and b.OrderID = '{3}' and b.ComboType = '{4}'", SewingDate, FactoryID, SewingLineID, OrderID, ComboType));

                if (MyUtility.Check.Empty(CPU)) CPU = "1";
                string Efficiency_A = MyUtility.GetValue.Lookup(string.Format(@"Select  sum(b.QAQty * {5} * AAA.CpuRate)  
                                                                        from SewingOutput a, SewingOutput_Detail b 
                                                                        left join Orders c on c.ID=b.OrderId
                                                                        cross apply dbo.GetCPURate(c.OrderTypeID,c.ProgramID,c.Category,c.BrandID,'O') as AAA
                                                                        where a.Id = b.ID and a.OutputDate = '{0}' and a.Team = 'A' and a.FactoryID = '{1}' and a.SewingLineID = '{2}' 
                                                                        and b.OrderID = '{3}' and b.ComboType = '{4}' ", SewingDate, FactoryID, SewingLineID, OrderID, ComboType, CPU));
                string Efficiency_B = MyUtility.GetValue.Lookup(string.Format(@"Select  sum(b.QAQty * {5} * AAA.CpuRate)  
                                                                        from SewingOutput a, SewingOutput_Detail b 
                                                                        left join Orders c on c.ID=b.OrderId
                                                                        cross apply dbo.GetCPURate(c.OrderTypeID,c.ProgramID,c.Category,c.BrandID,'O') as AAA
                                                                        where a.Id = b.ID and a.OutputDate = '{0}' and a.Team = 'B' and a.FactoryID = '{1}' and a.SewingLineID = '{2}' 
                                                                        and b.OrderID = '{3}' and b.ComboType = '{4}' ", SewingDate, FactoryID, SewingLineID, OrderID, ComboType, CPU));
                string PPH_A = MyUtility.GetValue.Lookup(string.Format(@"Select  sum(b.QAQty * {5} * AAA.CpuRate) / Sum(a.Manpower * b.WorkHour) 
                                                                        from SewingOutput a, SewingOutput_Detail b 
                                                                        left join Orders c on c.ID=b.OrderId
                                                                        cross apply dbo.GetCPURate(c.OrderTypeID,c.ProgramID,c.Category,c.BrandID,'O') as AAA
                                                                        where a.Id = b.ID and a.OutputDate = '{0}' and a.Team = 'A' and a.FactoryID = '{1}' and a.SewingLineID = '{2}' 
                                                                        and b.OrderID = '{3}' and b.ComboType = '{4}' ", SewingDate, FactoryID, SewingLineID, OrderID, ComboType, CPU));
                string PPH_B = MyUtility.GetValue.Lookup(string.Format(@"Select  sum(b.QAQty * {5} * AAA.CpuRate) / Sum(a.Manpower * b.WorkHour) 
                                                                        from SewingOutput a, SewingOutput_Detail b 
                                                                        left join Orders c on c.ID=b.OrderId
                                                                        cross apply dbo.GetCPURate(c.OrderTypeID,c.ProgramID,c.Category,c.BrandID,'O') as AAA
                                                                        where a.Id = b.ID and a.OutputDate = '{0}' and a.Team = 'B' and a.FactoryID = '{1}' and a.SewingLineID = '{2}' 
                                                                        and b.OrderID = '{3}' and b.ComboType = '{4}' ", SewingDate, FactoryID, SewingLineID, OrderID, ComboType, CPU));

                objSheet.Cells[32, 1] = SewingDate;  //Date
                objSheet.Cells[32, 2] = Sewers_A;  //No. of Sewers(Shift A)
                objSheet.Cells[32, 3] = Sewers_B;  //No. of Sewers(Shift B)
                objSheet.Cells[32, 4] = WorkingHours_A;  //Working Hours(Shift A)
                objSheet.Cells[32, 5] = WorkingHours_B;  //Working Hours(Shift B)
                objSheet.Cells[32, 6] = Target;  //Target(Eff)
                objSheet.Cells[32, 8] = OutputCMP_A;  //Output (CMP) (Shift A)
                objSheet.Cells[32, 9] = OutputCMP_B;  //Output (CMP) (Shift B)
                objSheet.Cells[32, 10] = Efficiency_A;  //Efficiency (Shift A)
                objSheet.Cells[32, 11] = Efficiency_B;  //Efficiency (Shift B)
                objSheet.Cells[32, 12] = PPH_A;  //PPH (Shift A)
                objSheet.Cells[32, 13] = PPH_B;  //PPH (Shift B)
                #endregion

                #region Problem Encountered
                DataTable dtProblem;
                string sql = string.Format(@"Select a.IEReasonID , b.Description, a.ShiftA, a.ShiftB 
                                            from ChgOver_Problem a, IEReason b 
                                            where a.IEReasonID = b.ID and a.ID = '{0}' and b.Type = 'CP' ", CurrentMaintain["ID"].ToString().Trim());
                DualResult result = DBProxy.Current.Select(null, sql, out dtProblem);

                //若超過4筆資料，Excel就要在新增列數
                if (dtProblem.Rows.Count > 4)
                {
                    string RowNum, RowStr;
                    int repeat = dtProblem.Rows.Count - 4;

                    Microsoft.Office.Interop.Excel.Range RngToCopy = (Microsoft.Office.Interop.Excel.Range)objSheet.get_Range("A36:J36").EntireRow;
                    Microsoft.Office.Interop.Excel.Range RngToInsert;

                    for (int i = 0; i < repeat; i++)
                    {
                        RowNum = Convert.ToString(40 + i);
                        RowStr = string.Format("A{0}:M{0}", RowNum);
                        RngToInsert = (Microsoft.Office.Interop.Excel.Range)objSheet.get_Range(RowStr).EntireRow;
                        RngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown, RngToCopy.Copy(Type.Missing));
                    }
                }

                Clipboard.Clear();  //清空剪貼簿

                int count = 0;
                foreach (DataRow dr in dtProblem.Rows)
                {
                    objSheet.Cells[36 + count, 1] = dr["IEReasonID"].ToString().Trim() + ":" + dr["Description"].ToString().Trim();  //Problem Encountered
                    objSheet.Cells[36 + count, 6] = dr["ShiftA"].ToString().Trim();  //Shift A
                    objSheet.Cells[36 + count, 10] = dr["ShiftB"].ToString().Trim();  //Shift B
                    count++;
                }
                #endregion
                
                objApp.Visible = true;

                if (objSheet != null) Marshal.FinalReleaseComObject(objSheet);    //釋放sheet
                if (objApp != null) Marshal.FinalReleaseComObject(objApp);          //釋放objApp

            }

            return true;

        }

        private DataRow GetType(string ComboType, string CDCodeID)
        {
            DataRow returnValue = null;
            DataTable dtTemp;
            string sql;
            if (ComboType == "T" || ComboType == "")
                sql = string.Format("select  TopProductionType as ProdType,TopFabricType as FabricType  from CDCode_Content where ID='{0}'", CDCodeID);
            else if (ComboType == "B")
                sql = string.Format("select BottomProductionType as ProdType,BottomFabricType as FabricType  from CDCode_Content where ID='{0}'", CDCodeID);
            else if (ComboType == "I")
                sql = string.Format("select InnerProductionType as ProdType,InnerFabricType as FabricType  from CDCode_Content where ID='{0}'", CDCodeID);
            else
                sql = string.Format("select OuterProductionType as ProdType,OuterFabricType as FabricType  from CDCode_Content where ID='{0}'", CDCodeID);
            DualResult result = DBProxy.Current.Select(null, sql, out dtTemp);
            if (result && dtTemp.Rows.Count > 0) returnValue = dtTemp.Rows[0];
            return returnValue;
        }


    }
}
