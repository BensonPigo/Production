﻿using System;
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
            txtInLineDate.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Inline", true, System.Windows.Forms.DataSourceUpdateMode.OnValidation, emptyDTMask, Sci.Env.Cfg.DateTimeStringFormat));
            txtInLineDate.Mask = dateTimeMask;
        }

        string StdTMS = ""; DataTable dt1; DataTable dt2;
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();

            string brand = string.Format(@"SELECT BrandID FROM Orders WITH (NOLOCK) 
                             left join ChgOver WITH (NOLOCK) on  Orders.id= ChgOver.OrderID
                             where ChgOver.OrderID ='{0}'", CurrentMaintain["OrderID"].ToString());
            DataTable brandOutput;
            DualResult res = DBProxy.Current.Select(null, brand, out brandOutput);
            if (!res) { this.ShowErr(res); }
            if (brandOutput.Rows.Count < 1)
            {
                txtBrand.Text = "";
            }
            else
            {
                txtBrand.Text = brandOutput.Rows[0]["BrandID"].ToString();
            }
            //  textBox_brand.Visible = false;

            txtBrand.ReadOnly = true;
            labelType.Text = CurrentMaintain["Type"].ToString() == "N" ? "New" : "Repeat";
            string sqlCmd = string.Format(@"with tmpSO
as
(
select distinct s.OutputDate,s.Manpower,s.ID
from SewingOutput s WITH (NOLOCK) , SewingOutput_Detail sd WITH (NOLOCK) 
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
left join SewingOutput_Detail sd WITH (NOLOCK) on s.ID = sd.ID
left join System WITH (NOLOCK) on 1=1
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
isnull((select top (1) iif(InspectQty = 0,0,Round((InspectQty-RejectQty)/InspectQty*100,2)) from RFT WITH (NOLOCK) where OrderID = '{0}' and CDate = SummaryData.OutputDate and SewinglineID = '{2}'),0) as Rft,StdTMS
from SummaryData
order by OutputDate
", CurrentMaintain["OrderID"].ToString(), CurrentMaintain["ComboType"].ToString(), CurrentMaintain["SewingLineID"].ToString(), CurrentMaintain["FactoryID"].ToString());
            DataTable sewingOutput;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out sewingOutput);
            if (sewingOutput.Rows.Count < 1) { StdTMS = "0"; }
            else{StdTMS = sewingOutput.Rows[0]["StdTMS"].ToString();}
            if (result)
            {
                int rec = 0;
                foreach (DataRow dr in sewingOutput.Rows)
                {
                    rec += 1;
                    switch (rec)
                    {
                        case 1:
                            dateSewingDate1stDay.Value = Convert.ToDateTime(dr["OutputDate"]);
                            numEFF1stDay.Value = Convert.ToDecimal(dr["Eff"]);
                            numRFT1stDay.Value = Convert.ToDecimal(dr["Rft"]);
                            break;
                        case 2:
                            dateSewingDate2ndDay.Value = Convert.ToDateTime(dr["OutputDate"]);
                            numEFF2ndDay.Value = Convert.ToDecimal(dr["Eff"]);
                            numRFT2ndDay.Value = Convert.ToDecimal(dr["Rft"]);
                            break;
                        case 3:
                            dateSewingDate3rdDay.Value = Convert.ToDateTime(dr["OutputDate"]);
                            numEFF3rdDay.Value = Convert.ToDecimal(dr["Eff"]);
                            numRFT3rdDay.Value = Convert.ToDecimal(dr["Rft"]);
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
                            dateSewingDate1stDay.Value = null;
                            numEFF1stDay.Value = 0;
                            numRFT1stDay.Value = 0;
                            dateSewingDate2ndDay.Value = null;
                            numEFF2ndDay.Value = 0;
                            numRFT2ndDay.Value = 0;
                            dateSewingDate3rdDay.Value = null;
                            numEFF3rdDay.Value = 0;
                            numRFT3rdDay.Value = 0;
                            break;
                        case 1:
                            dateSewingDate2ndDay.Value = null;
                            numEFF2ndDay.Value = 0;
                            numRFT2ndDay.Value = 0;
                            dateSewingDate3rdDay.Value = null;
                            numEFF3rdDay.Value = 0;
                            numRFT3rdDay.Value = 0;
                            break;
                        case 2:
                            dateSewingDate3rdDay.Value = null;
                            numEFF3rdDay.Value = 0;
                            numRFT3rdDay.Value = 0;
                            break;
                        default:
                            break;
                    }
                }
string ExcelDt = string.Format(@"SELECT C.OrderID,
                                        c.MDivisionID,
	                                    c.FactoryID,
                                        c.SewingLineID,
	                                    CELL.SewingCell,
	                                    c.StyleID,
	                                    sty.CPU,
	                                    c.CDCodeID,
	                                    cd.TopProductionType,
	                                    cd.TopFabricType,
	                                    cd.BottomProductionType,
	                                    cd.BottomFabricType,
	                                    cd.InnerProductionType,
	                                    cd.InnerFabricType,
	                                    cd.OuterProductionType,
	                                    cd.OuterFabricType,
	                                    c.ComboType,
                                        C.SeasonID,
                                        C.COT,
	                                    C.COPT,
	                                    c.Type,
	                                    c.Category,
	                                    c.Inline,
	                                    c.FirstOutputTime,
                                        c.LastOutputTime
                                 FROM ChgOver C WITH (NOLOCK) 
                                 OUTER APPLY (SELECT TOP  1 O.SciDelivery  FROM Orders O WITH (NOLOCK) WHERE O.POID=C.OrderID)SCI 
                                 OUTER APPLY(SELECT S.SewingCell FROM SewingLine S WITH (NOLOCK) WHERE S.ID=C.SewingLineID AND S.FactoryID=C.FactoryID)CELL
                                 outer apply(select * from style t WITH (NOLOCK) where t.id=c.StyleID and t.SeasonID=c.SeasonID)sty
                                 outer apply(select  * from CDCode_Content cd WITH (NOLOCK) where c.CDCodeID=cd.ID)cd
                                 where c.orderid='{0}'
                                 ORDER BY C.FactoryID,C.SewingLineID,C.Inline", CurrentMaintain["orderid"]);
                DualResult result1 = DBProxy.Current.Select(null, ExcelDt, out dt1);
                if (!result1) { this.ShowErr(result1); }

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
        private void txtTimeOfFirstGoodOutput_Validating(object sender, CancelEventArgs e)
        {
            Sci.Win.UI.TextBox prodTextValue = (Sci.Win.UI.TextBox)sender;
            if (EditMode && !MyUtility.Check.Empty(prodTextValue.Text) && prodTextValue.Text != prodTextValue.OldValue)
            {
                string textValue = prodTextValue.Text.ToString().PadRight(4);
                if ((!MyUtility.Check.Empty(textValue.Substring(0, 2)) && Convert.ToInt32(textValue.Substring(0, 2)) > 24) || ((!MyUtility.Check.Empty(textValue.Substring(0, 2)) && Convert.ToInt32(textValue.Substring(0, 2)) == 24) && (!MyUtility.Check.Empty(textValue.Substring(2, 2)) && Convert.ToInt32(textValue.Substring(2, 2)) != 0)) || (!MyUtility.Check.Empty(textValue.Substring(2, 2)) && Convert.ToInt32(textValue.Substring(2, 2)) >= 60))
                {
                    prodTextValue.Text = "";
                    e.Cancel = true; 
                    MyUtility.Msg.WarningBox("The time format is wrong, can't exceed '24:00'!");
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
        private void btnFTYGSD_Click(object sender, EventArgs e)
        {
            Sci.Production.IE.P01 callNextForm = new Sci.Production.IE.P01(CurrentMaintain["StyleID"].ToString(), MyUtility.GetValue.Lookup(string.Format("select BrandID from Orders WITH (NOLOCK) where ID = '{0}'", CurrentMaintain["OrderID"].ToString())), CurrentMaintain["SeasonID"].ToString(), CurrentMaintain["ComboType"].ToString());
            callNextForm.ShowDialog(this);
        }

        //Check List
        private void btnCheckList_Click(object sender, EventArgs e)
        {
            if (CurrentMaintain["Type"].ToString() == "N")
            {
                if (type == "1")
                {
                    if (!MyUtility.Check.Seek(string.Format("select ID from ChgOver_Check WITH (NOLOCK) where ID = '{0}'", CurrentMaintain["ID"].ToString())))
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
                    if (!MyUtility.Check.Seek(string.Format("select ID from ChgOver_Check WITH (NOLOCK) where ID = '{0}'", CurrentMaintain["ID"].ToString())))
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
        private void btnProblem_Click(object sender, EventArgs e)
        {
            if (type == "1")
            {
                if (!MyUtility.Check.Seek(string.Format("select ID from ChgOver_Problem WITH (NOLOCK) where ID = '{0}'", CurrentMaintain["ID"].ToString())))
                {
                    string insertCmd = string.Format(@"insert into ChgOver_Problem (ID,IEReasonID,AddName,AddDate)
select {0},ID,'{1}',GETDATE() from IEReason WI where Type = 'CP' and Junk = 0", CurrentMaintain["ID"].ToString(), Sci.Env.User.UserID);
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

            
        }

        //Print
        protected override bool ClickPrint()
        {
            ToExcel();
            return base.ClickPrint();
        }

        private bool ToExcel()
        {
            DataTable dtTitle;

            string OrderID = dt1.Rows[0]["orderid"].ToString();
            string BrandID = MyUtility.GetValue.Lookup(string.Format("select BrandID from Orders WITH (NOLOCK) where ID = '{0}'", OrderID));
            string StyleID = dt1.Rows[0]["StyleID"].ToString().Trim();
            string SeasonID = dt1.Rows[0]["SeasonID"].ToString().Trim();
            string CPU = MyUtility.GetValue.Lookup(string.Format("select CPU from Style WITH (NOLOCK) where BrandID='{0}' and ID='{1}' and SeasonID='{2}'", BrandID, StyleID, SeasonID));
            string FirstOutputTime = dt1.Rows[0]["FirstOutputTime"].ToString().Trim();
            FirstOutputTime = MyUtility.Check.Empty(FirstOutputTime) ? "" : FirstOutputTime.Substring(0, 2) + ":" + FirstOutputTime.Substring(2, 2);
            string LastOutputTime = dt1.Rows[0]["LastOutputTime"].ToString().Trim();
            LastOutputTime = MyUtility.Check.Empty(LastOutputTime) ? "" : LastOutputTime.Substring(0, 2) + ":" + LastOutputTime.Substring(2, 2);
            string FactoryID = dt1.Rows[0]["FactoryID"].ToString().Trim();
            string SewingLineID = dt1.Rows[0]["SewingLineID"].ToString().Trim();
            string ComboType = dt1.Rows[0]["ComboType"].ToString().Trim();

            #region 取出ChgOverTarget.Target，然後再依ChgOver.Inline找出最接近但沒有超過這一天的Target
            string MDivisionID = dt1.Rows[0]["MDivisionID"].ToString().Trim();
            DateTime Inline = Convert.ToDateTime(dt1.Rows[0]["Inline"]);
            string Target_COPT = MyUtility.GetValue.Lookup(string.Format(@"Select top 1 Target from ChgOverTarget WITH (NOLOCK) 
                                                                        where Type = 'COPT' and MDivisionID = '{0}' and EffectiveDate <= '{1}' 
                                                                        Order by EffectiveDate desc", MDivisionID, Inline.ToShortDateString()));
            string Target_COT = MyUtility.GetValue.Lookup(string.Format(@"Select top 1 Target from ChgOverTarget WITH (NOLOCK) 
                                                                        where Type = 'COT' and MDivisionID = '{0}' and EffectiveDate <= '{1}' 
                                                                        Order by EffectiveDate desc", MDivisionID, Inline.ToShortDateString()));
            #endregion

            string CDCodeID = dt1.Rows[0]["CDCodeID"].ToString().Trim();
            DataRow TYPE = GetType(ComboType, CDCodeID);
            #region 找出上一筆
            
            string Previous_CPU = string.Empty;
            string exceldt2 = string.Format(@"SELECT  * FROM (
                SELECT Top 1 C.OrderID,
                       c.MDivisionID,
                       c.FactoryID,
                       c.SewingLineID,
                       CELL.SewingCell,
                       c.SeasonID,
                       c.StyleID,
                       c.Inline,
                       c.CDCodeID,
                       cd.TopProductionType,
                       cd.TopFabricType    
                FROM ChgOver C WITH (NOLOCK) 
                OUTER APPLY(SELECT S.SewingCell FROM SewingLine S WITH (NOLOCK) WHERE S.ID=C.SewingLineID AND S.FactoryID=C.FactoryID)CELL
                outer apply(select  * from CDCode_Content cd WITH (NOLOCK) where c.CDCodeID=cd.ID)cd
                WHERE Inline >= '{0}'
		        AND OrderID !='{1}' and StyleID <='{2}' and FactoryID >= '{3}' and SewingLineID >='{4}' and CELL.SewingCell >='{5}'
                ORDER BY C.FactoryID,C.SewingLineID,C.Inline asc
                )[Found]", Inline.ToShortDateString(), dt1.Rows[0]["OrderID"], dt1.Rows[0]["StyleID"], dt1.Rows[0]["FactoryID"], dt1.Rows[0]["SewingLineID"], dt1.Rows[0]["SewingCell"]);
                DualResult result2 = DBProxy.Current.Select(null, exceldt2, out dt2);
                if (!result2) { this.ShowErr(result2); }
                string Previous_BrandID = MyUtility.GetValue.Lookup(string.Format("select BrandID from Orders WITH (NOLOCK) where ID = '{0}'", dt2.Rows[0]["OrderID"].ToString().Trim()));
                string Previous_StyleID = dt2.Rows[0]["StyleID"].ToString().Trim();
                string Previous_SeasonID = dt2.Rows[0]["SeasonID"].ToString().Trim();
                Previous_CPU = MyUtility.GetValue.Lookup(string.Format("select CPU from Style WITH (NOLOCK) where BrandID='{0}' and ID='{1}' and SeasonID='{2}'", Previous_BrandID, Previous_StyleID, Previous_SeasonID));
                //Previous_TYPE = GetType(PreviousDR["ComboType"].ToString().Trim(), PreviousDR["CDCodeID"].ToString().Trim());
            
            #endregion

                string cmdsql = string.Format("SELECT TOP 1 'CHANGEOVER REPORT'  FROM ChgOver WITH (NOLOCK) where 1=1");
            DualResult dResult = DBProxy.Current.Select(null, cmdsql, out dtTitle);

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\IE_P02_ChangeoverReport.xltx"); //預先開啟excel app

            if (MyUtility.Excel.CopyToXls(dtTitle, "", "IE_P02_ChangeoverReport.xltx", 2, false, null, objApp, false))
            {    // 將datatable copy to excel
                objApp.Visible = false;  //隱藏，避免使用者誤按
                Microsoft.Office.Interop.Excel._Worksheet objSheet = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
                Microsoft.Office.Interop.Excel._Workbook objBook = objApp.ActiveWorkbook;

                objSheet.Cells[9, 4] = FactoryID;  //Factory
                objSheet.Cells[9, 7] = SewingLineID;  //Line No.
                objSheet.Cells[9, 11] = dt2.Rows[0]["SewingCell"].ToString().Trim();  //Cell No.
          
                objSheet.Cells[12, 4] = StyleID;  //Style No.
                objSheet.Cells[13, 4] = CPU;  //CPU/pc
                objSheet.Cells[14, 4] = CDCodeID;  //CD Code
                objSheet.Cells[14, 5] = dt1.Rows[0]["TopProductionType"];  //Prod. Type
                objSheet.Cells[14, 6] = dt1.Rows[0]["TopFabricType"];  //Fab. Type
                   
                #region 抓上一筆資料
                if (dt1.Rows[0]["SewingLineID"].ToString() == dt2.Rows[0]["SewingLineID"].ToString())
                {
                    objSheet.Cells[12, 9] = dt2.Rows[0]["StyleID"].ToString().Trim();  //Style No.
                    objSheet.Cells[13, 9] = Previous_CPU;  //CPU/pc
                    objSheet.Cells[14, 9] = dt2.Rows[0]["CDCodeID"].ToString().Trim();  //CD Code
                    objSheet.Cells[14, 10] = dt2.Rows[0]["TopProductionType"];  //Prod. Type
                    objSheet.Cells[14, 11] = dt2.Rows[0]["TopFabricType"];  //Fab. Type

                }
                else if (dt1.Rows[0]["SewingLineID"] != dt2.Rows[0]["SewingLineID"])
                { 
                        objSheet.Cells[12, 9] ="";
                        objSheet.Cells[13, 9] ="";
                        objSheet.Cells[14, 9] = "";
                        objSheet.Cells[14, 10] = "";
                        objSheet.Cells[14, 11] =  "";
                }
                   
                #endregion

                objSheet.Cells[18, 5] = dt1.Rows[0]["Type"].ToString() == "N" ? "New" : "Repeat";  //Classification
                objSheet.Cells[19, 4] = dt1.Rows[0]["Category"].ToString().Trim();  //Category
                objSheet.Cells[20, 4] = Inline.ToShortDateString();  //Inline Date
                objSheet.Cells[21, 5] = Inline.ToString("HH:mm");  //Inline Time(HH:mm)
                objSheet.Cells[22, 6] = FirstOutputTime;  //Time of First Good Output (hh:mm):
                objSheet.Cells[22, 11] = LastOutputTime;  //Time of Last Good Output (hh:mm):

                objSheet.Cells[27, 3] = Target_COPT;  //Target
                objSheet.Cells[27, 8] = Target_COT;  //Target
                objSheet.Cells[27, 4] = dt1.Rows[0]["COPT"].ToString().Trim();  //Actual
                objSheet.Cells[27, 9] = dt1.Rows[0]["COT"].ToString().Trim();  //Actual

                #region 32列
                string SewingDate = MyUtility.GetValue.Lookup(string.Format(@"select convert(varchar, min(a.OutputDate), 111) as SewingDate
                                                                    from SewingOutput a WITH (NOLOCK) 
                                                                    left join SewingOutput_Detail b WITH (NOLOCK) on a.ID=b.ID
                                                                    where b.OrderId='{0}'", OrderID));
                string Sewers_A = MyUtility.GetValue.Lookup(string.Format(@"Select Distinct a.Manpower 
                                                                    from SewingOutput a WITH (NOLOCK) , SewingOutput_Detail b WITH (NOLOCK) 
                                                                    where a.Id = b.ID and a.OutputDate = '{0}' and a.Team = 'A' and a.FactoryID = '{1}' 
                                                                    and a.SewingLineID = '{2}' and b.OrderID = '{3}' and b.ComboType = '{4}'", SewingDate, FactoryID, SewingLineID, OrderID, ComboType));
                string Sewers_B = MyUtility.GetValue.Lookup(string.Format(@"Select Distinct a.Manpower 
                                                                    from SewingOutput a WITH (NOLOCK) , SewingOutput_Detail b WITH (NOLOCK) 
                                                                    where a.Id = b.ID and a.OutputDate = '{0}' and a.Team = 'B' and a.FactoryID = '{1}' 
                                                                    and a.SewingLineID = '{2}' and b.OrderID = '{3}' and b.ComboType = '{4}'", SewingDate, FactoryID, SewingLineID, OrderID, ComboType));
                string WorkingHours_A = MyUtility.GetValue.Lookup(string.Format(@"Select Sum(b.WorkHour)  
                                                                    from SewingOutput a WITH (NOLOCK) , SewingOutput_Detail b WITH (NOLOCK) 
                                                                    where a.Id = b.ID and a.OutputDate = '{0}' and a.Team = 'A' and a.FactoryID = '{1}' 
                                                                    and a.SewingLineID = '{2}' and b.OrderID = '{3}' and b.ComboType = '{4}'", SewingDate, FactoryID, SewingLineID, OrderID, ComboType));
                string WorkingHours_B = MyUtility.GetValue.Lookup(string.Format(@"Select Sum(b.WorkHour)  
                                                                    from SewingOutput a WITH (NOLOCK) , SewingOutput_Detail b WITH (NOLOCK) 
                                                                    where a.Id = b.ID and a.OutputDate = '{0}' and a.Team = 'B' and a.FactoryID = '{1}' 
                                                                    and a.SewingLineID = '{2}' and b.OrderID = '{3}' and b.ComboType = '{4}'", SewingDate, FactoryID, SewingLineID, OrderID, ComboType));
                string Target = MyUtility.GetValue.Lookup(string.Format(@"Select top 1 Target from ChgOverTarget  WITH (NOLOCK) 
                                                                    where Type = 'EFF.' and MDivisionID = '{0}' and EffectiveDate <= '{1}' 
                                                                    Order by EffectiveDate desc", MDivisionID, Inline.ToShortDateString()));
                string OutputCMP_A = MyUtility.GetValue.Lookup(string.Format(@"Select Sum(b.QAQty)  
                                                                    from SewingOutput a WITH (NOLOCK) , SewingOutput_Detail b WITH (NOLOCK)  
                                                                    where a.Id = b.ID and a.OutputDate = '{0}' and a.Team = 'A' and a.FactoryID = '{1}' 
                                                                    and a.SewingLineID = '{2}' and b.OrderID = '{3}' and b.ComboType = '{4}'", SewingDate, FactoryID, SewingLineID, OrderID, ComboType));
                string OutputCMP_B = MyUtility.GetValue.Lookup(string.Format(@"Select Sum(b.QAQty) 
                                                                    from SewingOutput a WITH (NOLOCK) , SewingOutput_Detail b  WITH (NOLOCK) 
                                                                    where a.Id = b.ID and a.OutputDate = '{0}' and a.Team = 'B' and a.FactoryID = '{1}' 
                                                                    and a.SewingLineID = '{2}' and b.OrderID = '{3}' and b.ComboType = '{4}'", SewingDate, FactoryID, SewingLineID, OrderID, ComboType));

                if (MyUtility.Check.Empty(CPU)) CPU = "1";
                string Efficiency_A = MyUtility.GetValue.Lookup(string.Format(@"Select IIF('{6}'=0,0,sum(b.QAQty * {5} * AAA.CpuRate) / Sum(a.Manpower * b.WorkHour)*3600 / '{6}'/100)
                                                                    from SewingOutput a WITH (NOLOCK) , SewingOutput_Detail b WITH (NOLOCK)  
                                                                    left join Orders c WITH (NOLOCK) on c.ID=b.OrderId
                                                                    cross apply dbo.GetCPURate(c.OrderTypeID,c.ProgramID,c.Category,c.BrandID,'O') as AAA
                                                                    where a.Id = b.ID and a.OutputDate = '{0}' and a.Team = 'A' and a.FactoryID = '{1}' and a.SewingLineID = '{2}' 
                                                                    and b.OrderID = '{3}' and b.ComboType = '{4}' ", SewingDate, FactoryID, SewingLineID, OrderID, ComboType, CPU, StdTMS));
                string Efficiency_B = MyUtility.GetValue.Lookup(string.Format(@"Select  IIF('{6}'=0,0,sum(b.QAQty * {5} * AAA.CpuRate) / Sum(a.Manpower * b.WorkHour)*3600 / '{6}'/100)  
                                                                    from SewingOutput a WITH (NOLOCK) , SewingOutput_Detail b WITH (NOLOCK) 
                                                                    left join Orders c WITH (NOLOCK) on c.ID=b.OrderId
                                                                    cross apply dbo.GetCPURate(c.OrderTypeID,c.ProgramID,c.Category,c.BrandID,'O') as AAA
                                                                    where a.Id = b.ID and a.OutputDate = '{0}' and a.Team = 'B' and a.FactoryID = '{1}' and a.SewingLineID = '{2}' 
                                                                    and b.OrderID = '{3}' and b.ComboType = '{4}' ", SewingDate, FactoryID, SewingLineID, OrderID, ComboType, CPU, StdTMS));
                string PPH_A = MyUtility.GetValue.Lookup(string.Format(@"Select  sum(b.QAQty * {5} * AAA.CpuRate) / Sum(a.Manpower * b.WorkHour) 
                                                                    from SewingOutput a WITH (NOLOCK) , SewingOutput_Detail b WITH (NOLOCK)  
                                                                    left join Orders c WITH (NOLOCK) on c.ID=b.OrderId
                                                                    cross apply dbo.GetCPURate(c.OrderTypeID,c.ProgramID,c.Category,c.BrandID,'O') as AAA
                                                                    where a.Id = b.ID and a.OutputDate = '{0}' and a.Team = 'A' and a.FactoryID = '{1}' and a.SewingLineID = '{2}' 
                                                                    and b.OrderID = '{3}' and b.ComboType = '{4}' ", SewingDate, FactoryID, SewingLineID, OrderID, ComboType, CPU));
                string PPH_B = MyUtility.GetValue.Lookup(string.Format(@"Select  sum(b.QAQty * {5} * AAA.CpuRate) / Sum(a.Manpower * b.WorkHour) 
                                                                    from SewingOutput a WITH (NOLOCK) , SewingOutput_Detail b WITH (NOLOCK) 
                                                                    left join Orders c WITH (NOLOCK) on c.ID=b.OrderId
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
                                        from ChgOver_Problem a WITH (NOLOCK) , IEReason b WITH (NOLOCK) 
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

                //Clipboard.Clear();  //清空剪貼簿

                int count = 0;
                foreach (DataRow dr in dtProblem.Rows)
                {
                    objSheet.Cells[36 + count, 1] = dr["IEReasonID"].ToString().Trim() + ":" + dr["Description"].ToString().Trim();  //Problem Encountered
                    objSheet.Cells[36 + count, 6] = dr["ShiftA"].ToString().Trim();  //Shift A
                    objSheet.Cells[36 + count, 10] = dr["ShiftB"].ToString().Trim();  //Shift B
                    count++;
                }
                #endregion

                #region Save & Show Excel
                string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("IE_P02_ChangeoverReport");
                Microsoft.Office.Interop.Excel.Workbook workbook = objApp.ActiveWorkbook;
                workbook.SaveAs(strExcelName);
                workbook.Close();
                objApp.Quit();
                Marshal.ReleaseComObject(objSheet);    //釋放sheet
                Marshal.ReleaseComObject(objApp);      //釋放objApp
                Marshal.ReleaseComObject(workbook);

                strExcelName.OpenFile();
                #endregion 
            }

            return true;

        }

        private DataRow GetType(string ComboType, string CDCodeID)
        {
            DataRow returnValue = null;
            DataTable dtTemp;
            string sql;
            if (ComboType == "T" || ComboType == "")
                sql = string.Format("select  TopProductionType as ProdType,TopFabricType as FabricType  from CDCode_Content WITH (NOLOCK) where ID='{0}'", CDCodeID);
            else if (ComboType == "B")
                sql = string.Format("select BottomProductionType as ProdType,BottomFabricType as FabricType  from CDCode_Content WITH (NOLOCK) where ID='{0}'", CDCodeID);
            else if (ComboType == "I")
                sql = string.Format("select InnerProductionType as ProdType,InnerFabricType as FabricType  from CDCode_Content WITH (NOLOCK) where ID='{0}'", CDCodeID);
            else
                sql = string.Format("select OuterProductionType as ProdType,OuterFabricType as FabricType  from CDCode_Content WITH (NOLOCK) where ID='{0}'", CDCodeID);
            DualResult result = DBProxy.Current.Select(null, sql, out dtTemp);
            if (result && dtTemp.Rows.Count > 0) returnValue = dtTemp.Rows[0];
            return returnValue;
        }


    }
}
