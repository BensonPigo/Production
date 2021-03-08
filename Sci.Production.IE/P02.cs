using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using System.Runtime.InteropServices;
using System.Drawing;

namespace Sci.Production.IE
{
    /// <summary>
    /// IE_P02
    /// </summary>
    public partial class P02 : Win.Tems.Input1
    {
        private string dateTimeMask = string.Empty;
        private string emptyDTMask = string.Empty;
        private string empmask;
        private string dtmask;
        private string type;

        /// <summary>
        /// P02
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        /// <param name="type">Type</param>
        public P02(ToolStripMenuItem menuitem, string type)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.type = type;
            this.Text = this.type == "1" ? "P02. Style Changeover Monitor" : "P021. Style Changeover Monitor (History)";
            this.DefaultFilter = this.type == "1" ? string.Format("MDivisionID = '{0}' AND Status <> 'Closed'", Env.User.Keyword) : string.Format("MDivisionID = '{0}' AND Status = 'Closed'", Env.User.Keyword);
            if (this.type == "2")
           {
               this.IsSupportEdit = false;
            }

            // 組InLine date的mask
            for (int i = 0; i < Env.Cfg.DateTimeStringFormat.Length; i++)
            {
                this.dtmask = Env.Cfg.DateTimeStringFormat.Substring(i, 1) == "/" || Env.Cfg.DateTimeStringFormat.Substring(i, 1) == ":" ? Env.Cfg.DateTimeStringFormat.Substring(i, 1) : Env.Cfg.DateTimeStringFormat.Substring(i, 1) == " " ? " " : "0";
                this.empmask = Env.Cfg.DateTimeStringFormat.Substring(i, 1) == "/" || Env.Cfg.DateTimeStringFormat.Substring(i, 1) == ":" ? Env.Cfg.DateTimeStringFormat.Substring(i, 1) : Env.Cfg.DateTimeStringFormat.Substring(i, 1) == "s" ? string.Empty : " ";
                this.DateTimeMask = this.DateTimeMask + this.dtmask;
                this.emptyDTMask = this.emptyDTMask + this.empmask;
            }

            this.txtInLineDate.DataBindings.Add(new Binding("Text", this.mtbs, "Inline", true, DataSourceUpdateMode.OnValidation, this.emptyDTMask, Env.Cfg.DateTimeStringFormat));
            this.txtInLineDate.Mask = this.DateTimeMask;
        }

        private string StdTMS = string.Empty; private DataTable dt1; private DataTable dt2;

        /// <summary>
        /// DateTimeMask
        /// </summary>
        public string DateTimeMask
        {
            get
            {
                return this.dateTimeMask;
            }

            set
            {
                this.dateTimeMask = value;
            }
        }

        /// <summary>
        /// OnDetailEntered
        /// </summary>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();

            bool existsChgOver_Check = MyUtility.Check.Seek(string.Format("select ID from ChgOver_Check WITH (NOLOCK) where ID = '{0}'", this.CurrentMaintain["ID"].ToString()));
            this.btnCheckList.ForeColor = existsChgOver_Check ? Color.Blue : Color.Black;

            string brand = string.Format(
                @"
SELECT o.BrandID 
    , o.CDCodeNew
	, sty.ProductType
	, sty.FabricType
	, sty.Lining
	, sty.Gender
	, sty.Construction
FROM Orders o WITH (NOLOCK) 
left join ChgOver c WITH (NOLOCK) on o.id= c.OrderID
Outer apply (
	SELECT ProductType = r2.Name
		, FabricType = r1.Name
		, Lining
		, Gender
		, Construction = d1.Name
	FROM Style s WITH(NOLOCK)
	left join DropDownList d1 WITH(NOLOCK) on d1.type= 'StyleConstruction' and d1.ID = s.Construction
	left join Reason r1 WITH(NOLOCK) on r1.ReasonTypeID= 'Fabric_Kind' and r1.ID = s.FabricType
	left join Reason r2 WITH(NOLOCK) on r2.ReasonTypeID= 'Style_Apparel_Type' and r2.ID = s.ApparelType
	where s.ID = c.StyleID
    and s.SeasonID = c.SeasonID
    and s.BrandID = o.BrandID
)sty
where c.OrderID ='{0}'",
                this.CurrentMaintain["OrderID"].ToString());
            DataTable dt;
            DualResult res = DBProxy.Current.Select(null, brand, out dt);
            if (!res)
            {
                this.ShowErr(res);
            }

            this.txtBrand.Text = dt.Rows.Count > 0 ? dt.Rows[0]["BrandID"].ToString() : string.Empty;
            this.displayCDCodeNew.Value = dt.Rows.Count > 0 ? dt.Rows[0]["CDCodeNew"].ToString() : string.Empty;
            this.displayProductType.Value = dt.Rows.Count > 0 ? dt.Rows[0]["ProductType"].ToString() : string.Empty;
            this.displayFabricType.Value = dt.Rows.Count > 0 ? dt.Rows[0]["FabricType"].ToString() : string.Empty;
            this.displayLining.Value = dt.Rows.Count > 0 ? dt.Rows[0]["Lining"].ToString() : string.Empty;
            this.displayGender.Value = dt.Rows.Count > 0 ? dt.Rows[0]["Gender"].ToString() : string.Empty;
            this.displayConstruction.Value = dt.Rows.Count > 0 ? dt.Rows[0]["Construction"].ToString() : string.Empty;

            // textBox_brand.Visible = false;
            this.txtBrand.ReadOnly = true;
            this.labelType.Text = this.CurrentMaintain["Type"].ToString() == "N" ? "New" : "Repeat";
            string sqlCmd = string.Format(
                @"with tmpSO
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
",
                this.CurrentMaintain["OrderID"].ToString(),
                this.CurrentMaintain["ComboType"].ToString(),
                this.CurrentMaintain["SewingLineID"].ToString(),
                this.CurrentMaintain["FactoryID"].ToString());
            DataTable sewingOutput;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out sewingOutput);
            if (sewingOutput.Rows.Count < 1)
            {
                this.StdTMS = "0";
            }
            else
            {
                this.StdTMS = sewingOutput.Rows[0]["StdTMS"].ToString();
            }

            if (result)
            {
                int rec = 0;
                foreach (DataRow dr in sewingOutput.Rows)
                {
                    rec += 1;
                    switch (rec)
                    {
                        case 1:
                            this.dateSewingDate1stDay.Value = Convert.ToDateTime(dr["OutputDate"]);
                            this.numEFF1stDay.Value = Convert.ToDecimal(dr["Eff"]);
                            this.numRFT1stDay.Value = Convert.ToDecimal(dr["Rft"]);
                            break;
                        case 2:
                            this.dateSewingDate2ndDay.Value = Convert.ToDateTime(dr["OutputDate"]);
                            this.numEFF2ndDay.Value = Convert.ToDecimal(dr["Eff"]);
                            this.numRFT2ndDay.Value = Convert.ToDecimal(dr["Rft"]);
                            break;
                        case 3:
                            this.dateSewingDate3rdDay.Value = Convert.ToDateTime(dr["OutputDate"]);
                            this.numEFF3rdDay.Value = Convert.ToDecimal(dr["Eff"]);
                            this.numRFT3rdDay.Value = Convert.ToDecimal(dr["Rft"]);
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
                            this.dateSewingDate1stDay.Value = null;
                            this.numEFF1stDay.Value = 0;
                            this.numRFT1stDay.Value = 0;
                            this.dateSewingDate2ndDay.Value = null;
                            this.numEFF2ndDay.Value = 0;
                            this.numRFT2ndDay.Value = 0;
                            this.dateSewingDate3rdDay.Value = null;
                            this.numEFF3rdDay.Value = 0;
                            this.numRFT3rdDay.Value = 0;
                            break;
                        case 1:
                            this.dateSewingDate2ndDay.Value = null;
                            this.numEFF2ndDay.Value = 0;
                            this.numRFT2ndDay.Value = 0;
                            this.dateSewingDate3rdDay.Value = null;
                            this.numEFF3rdDay.Value = 0;
                            this.numRFT3rdDay.Value = 0;
                            break;
                        case 2:
                            this.dateSewingDate3rdDay.Value = null;
                            this.numEFF3rdDay.Value = 0;
                            this.numRFT3rdDay.Value = 0;
                            break;
                        default:
                            break;
                    }
                }

                string sqlcmd = string.Format(
    @"SELECT C.OrderID,
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
                                 ORDER BY C.FactoryID,C.SewingLineID,C.Inline",
    this.CurrentMaintain["orderid"]);
                DualResult result1 = DBProxy.Current.Select(
                    null,
                    sqlcmd,
                    out this.dt1);
                if (!result1)
                {
                    this.ShowErr(result1);
                }
            }
        }

        /// <summary>
        /// ClickEditBefore
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ClickEditBefore()
        {
            if (!MyUtility.Check.Empty(this.CurrentMaintain["ApvDate"]))
            {
                MyUtility.Msg.WarningBox("This record is 'Approved', can't be modify!");

                return false;
            }

            return base.ClickEditBefore();
        }

        // Time of First/Last Good Output
        private void TxtTimeOfFirstGoodOutput_Validating(object sender, CancelEventArgs e)
        {
            Win.UI.TextBox prodTextValue = (Win.UI.TextBox)sender;
            if (this.EditMode && !MyUtility.Check.Empty(prodTextValue.Text) && prodTextValue.Text != prodTextValue.OldValue)
            {
                string textValue = prodTextValue.Text.ToString().PadRight(4);
                if ((!MyUtility.Check.Empty(textValue.Substring(0, 2)) && Convert.ToInt32(textValue.Substring(0, 2)) > 24) || ((!MyUtility.Check.Empty(textValue.Substring(0, 2)) && Convert.ToInt32(textValue.Substring(0, 2)) == 24) && (!MyUtility.Check.Empty(textValue.Substring(2, 2)) && Convert.ToInt32(textValue.Substring(2, 2)) != 0)) || (!MyUtility.Check.Empty(textValue.Substring(2, 2)) && Convert.ToInt32(textValue.Substring(2, 2)) >= 60))
                {
                    prodTextValue.Text = string.Empty;
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

        // FTY GSD
        private void BtnFTYGSD_Click(object sender, EventArgs e)
        {
            P01 callNextForm = new P01(this.CurrentMaintain["StyleID"].ToString(), MyUtility.GetValue.Lookup(string.Format("select BrandID from Orders WITH (NOLOCK) where ID = '{0}'", this.CurrentMaintain["OrderID"].ToString())), this.CurrentMaintain["SeasonID"].ToString(), this.CurrentMaintain["ComboType"].ToString());
            callNextForm.ShowDialog(this);
        }

        // Check List
        private void BtnCheckList_Click(object sender, EventArgs e)
        {
            string sqlWhere = "(UseFor = 'R' or UseFor = 'A')";
            if (this.CurrentMaintain["Type"].ToString() == "N")
            {
                sqlWhere = "(UseFor = 'N' or UseFor = 'A') ";
            }

            if (this.type == "1" &&
                   !MyUtility.Check.Seek(string.Format("select ID from ChgOver_Check WITH (NOLOCK) where ID = '{0}'", this.CurrentMaintain["ID"].ToString())))
            {
                // sql參數
                System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter();
                System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter();
                System.Data.SqlClient.SqlParameter sp3 = new System.Data.SqlClient.SqlParameter();
                sp1.ParameterName = "@id";
                sp1.Value = this.CurrentMaintain["ID"].ToString();
                sp2.ParameterName = "@orderid";
                sp2.Value = this.CurrentMaintain["OrderID"].ToString();
                sp3.ParameterName = "@ChangeOverDate";
                sp3.Value = this.txtInLineDate.Text;
                sp3.DbType = DbType.Date;

                IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                cmds.Add(sp1);
                cmds.Add(sp2);
                cmds.Add(sp3);

                string insertCmd = $@"
                    declare @SCIDeliver as date, @BrandID as varchar(20), @FactoryID as varchar(20)  
                    select @BrandID = BrandID,@SCIDeliver = SCIDelivery,@FactoryID = FactoryID from Orders where ID = @orderid

                    insert into ChgOver_Check (ID,DayBe4Inline,BaseOn,ChgOverCheckListID,ScheduleDate)
                    select @id,DaysBefore,BaseOn,ID 
                        ,	case BaseOn 
		                        when '1' then dbo.CalculateSchdeuleDate(@ChangeOverDate,DaysBefore,@FactoryID,1)
	                        else dbo.CalculateSchdeuleDate(@SCIDeliver,DaysBefore,@FactoryID,1)
	                        end [SchdeuleDate]
                    from ChgOverCheckList 
                    where  {sqlWhere}
                            and (
                                BrandID = @BrandID
                                or 
                                BrandID is null
                                or
                                BrandID = ''
                            )
                            and Junk = 0";
                DualResult result = DBProxy.Current.Execute(null, insertCmd, cmds);
                if (!result)
                {
                    MyUtility.Msg.ErrorBox("Insert ChgOver_CheckList fail!!\r\n" + result.ToString());
                    return;
                }
            }

            P02_NewCheckList callNextForm = new P02_NewCheckList(string.Compare(this.CurrentMaintain["Status"].ToString(), "New", true) == 0, this.CurrentMaintain["ID"].ToString(), null, null, this.CurrentMaintain["Type"].ToString());
            callNextForm.ShowDialog(this);
        }

        // Problem
        private void BtnProblem_Click(object sender, EventArgs e)
        {
            if (this.type == "1")
            {
                if (!MyUtility.Check.Seek(string.Format("select ID from ChgOver_Problem WITH (NOLOCK) where ID = '{0}'", this.CurrentMaintain["ID"].ToString())))
                {
                    string insertCmd = string.Format(
                        @"insert into ChgOver_Problem (ID,IEReasonID,AddName,AddDate)
select {0},ID,'{1}',GETDATE() from IEReason WI where Type = 'CP' and Junk = 0",
                        this.CurrentMaintain["ID"].ToString(),
                        Env.User.UserID);
                    DualResult result = DBProxy.Current.Execute(null, insertCmd);
                    if (!result)
                    {
                        MyUtility.Msg.ErrorBox("Insert ChgOver_Problem fail!!\r\n" + result.ToString());
                        return;
                    }
                }
            }

            bool canEdit = this.CurrentMaintain["Status"].ToString() != "Closed" &&
                           this.CurrentMaintain["Status"].ToString() != "Approved";

            P02_Problem callNextForm = new P02_Problem(canEdit, this.CurrentMaintain["ID"].ToString(), null, null);
            callNextForm.ShowDialog(this);
        }

        /// <summary>
        /// ClickConfirm
        /// </summary>
        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            string sqlCmd = string.Format("update ChgOver set Status = 'Approved', ApvDate = GETDATE(), EditName = '{0}', EditDate = GETDATE() where ID = '{1}'", Env.User.UserID, this.CurrentMaintain["ID"].ToString());

            DualResult result = DBProxy.Current.Execute(null, sqlCmd);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Confirm failed, Pleaes re-try");
            }
        }

        /// <summary>
        /// UnConfirm
        /// </summary>
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();

            string sqlCmd = string.Format("update ChgOver set Status = 'New', ApvDate = null, EditName = '{0}', EditDate = GETDATE() where ID = '{1}'", Env.User.UserID, this.CurrentMaintain["ID"].ToString());

            DualResult result = DBProxy.Current.Execute(null, sqlCmd);
            if (!result)
            {
                MyUtility.Msg.WarningBox("UnConfirm failed, Pleaes re-try");
            }
        }

        /// <summary>
        /// ClickPrint
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ClickPrint()
        {
            this.ToExcel();
            return base.ClickPrint();
        }

        private bool ToExcel()
        {
            DataTable dtTitle;

            string orderID = this.dt1.Rows[0]["orderid"].ToString();
            string brandID = MyUtility.GetValue.Lookup(string.Format("select BrandID from Orders WITH (NOLOCK) where ID = '{0}'", orderID));
            string styleID = this.dt1.Rows[0]["StyleID"].ToString().Trim();
            string seasonID = this.dt1.Rows[0]["SeasonID"].ToString().Trim();
            string cPU = MyUtility.GetValue.Lookup(string.Format("select CPU from Style WITH (NOLOCK) where BrandID='{0}' and ID='{1}' and SeasonID='{2}'", brandID, styleID, seasonID));
            string firstOutputTime = this.dt1.Rows[0]["FirstOutputTime"].ToString().Trim();
            firstOutputTime = MyUtility.Check.Empty(firstOutputTime) ? string.Empty : firstOutputTime.Substring(0, 2) + ":" + firstOutputTime.Substring(2, 2);
            string lastOutputTime = this.dt1.Rows[0]["LastOutputTime"].ToString().Trim();
            lastOutputTime = MyUtility.Check.Empty(lastOutputTime) ? string.Empty : lastOutputTime.Substring(0, 2) + ":" + lastOutputTime.Substring(2, 2);
            string factoryID = this.dt1.Rows[0]["FactoryID"].ToString().Trim();
            string sewingLineID = this.dt1.Rows[0]["SewingLineID"].ToString().Trim();
            string comboType = this.dt1.Rows[0]["ComboType"].ToString().Trim();

            #region 取出ChgOverTarget.Target，然後再依ChgOver.Inline找出最接近但沒有超過這一天的Target
            string mDivisionID = this.dt1.Rows[0]["MDivisionID"].ToString().Trim();
            DateTime inline = Convert.ToDateTime(this.dt1.Rows[0]["Inline"]);
            string target_COPT = MyUtility.GetValue.Lookup(string.Format(
                @"Select top 1 Target from ChgOverTarget WITH (NOLOCK) 
                                                                        where Type = 'COPT' and MDivisionID = '{0}' and EffectiveDate <= '{1}' 
                                                                        Order by EffectiveDate desc", mDivisionID,
                inline.ToShortDateString()));
            string target_COT = MyUtility.GetValue.Lookup(string.Format(
                @"Select top 1 Target from ChgOverTarget WITH (NOLOCK) 
                                                                        where Type = 'COT' and MDivisionID = '{0}' and EffectiveDate <= '{1}' 
                                                                        Order by EffectiveDate desc", mDivisionID,
                inline.ToShortDateString()));
            #endregion

            string cDCodeID = this.dt1.Rows[0]["CDCodeID"].ToString().Trim();
            DataRow tYPE = this.GetType(comboType, cDCodeID);
            #region 找出上一筆

            string previous_CPU = string.Empty;
            string exceldt2 = string.Format(
                @"SELECT  * FROM (
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
                )[Found]",
                inline.ToShortDateString(),
                this.dt1.Rows[0]["OrderID"],
                this.dt1.Rows[0]["StyleID"],
                this.dt1.Rows[0]["FactoryID"],
                this.dt1.Rows[0]["SewingLineID"],
                this.dt1.Rows[0]["SewingCell"]);
            DualResult result2 = DBProxy.Current.Select(null, exceldt2, out this.dt2);
            if (!result2)
                {
                    this.ShowErr(result2);
                }

            string previous_BrandID = MyUtility.GetValue.Lookup(string.Format("select BrandID from Orders WITH (NOLOCK) where ID = '{0}'", this.dt2.Rows[0]["OrderID"].ToString().Trim()));
            string previous_StyleID = this.dt2.Rows[0]["StyleID"].ToString().Trim();
            string previous_SeasonID = this.dt2.Rows[0]["SeasonID"].ToString().Trim();
            previous_CPU = MyUtility.GetValue.Lookup(string.Format("select CPU from Style WITH (NOLOCK) where BrandID='{0}' and ID='{1}' and SeasonID='{2}'", previous_BrandID, previous_StyleID, previous_SeasonID));

                // Previous_TYPE = GetType(PreviousDR["ComboType"].ToString().Trim(), PreviousDR["CDCodeID"].ToString().Trim());
            #endregion

            string cmdsql = string.Format("SELECT TOP 1 'CHANGEOVER REPORT'  FROM ChgOver WITH (NOLOCK) where 1=1");
            DualResult dResult = DBProxy.Current.Select(null, cmdsql, out dtTitle);

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\IE_P02_ChangeoverReport.xltx"); // 預先開啟excel app

            if (MyUtility.Excel.CopyToXls(dtTitle, string.Empty, "IE_P02_ChangeoverReport.xltx", 2, false, null, objApp, false))
            { // 將datatable copy to excel
                objApp.Visible = false;  // 隱藏，避免使用者誤按
                Microsoft.Office.Interop.Excel._Worksheet objSheet = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
                Microsoft.Office.Interop.Excel._Workbook objBook = objApp.ActiveWorkbook;

                objSheet.Cells[9, 4] = factoryID;  // Factory
                objSheet.Cells[9, 7] = sewingLineID;  // Line No.
                objSheet.Cells[9, 11] = this.dt2.Rows[0]["SewingCell"].ToString().Trim();  // Cell No.

                objSheet.Cells[12, 4] = styleID;  // Style No.
                objSheet.Cells[13, 4] = cPU;  // CPU/pc
                objSheet.Cells[14, 4] = cDCodeID;  // CD Code
                objSheet.Cells[14, 5] = this.dt1.Rows[0]["TopProductionType"];  // Prod. Type
                objSheet.Cells[14, 6] = this.dt1.Rows[0]["TopFabricType"];  // Fab. Type

                #region 抓上一筆資料
                if (this.dt1.Rows[0]["SewingLineID"].ToString() == this.dt2.Rows[0]["SewingLineID"].ToString())
                {
                    objSheet.Cells[12, 9] = this.dt2.Rows[0]["StyleID"].ToString().Trim();  // Style No.
                    objSheet.Cells[13, 9] = previous_CPU;  // CPU/pc
                    objSheet.Cells[14, 9] = this.dt2.Rows[0]["CDCodeID"].ToString().Trim();  // CD Code
                    objSheet.Cells[14, 10] = this.dt2.Rows[0]["TopProductionType"];  // Prod. Type
                    objSheet.Cells[14, 11] = this.dt2.Rows[0]["TopFabricType"];  // Fab. Type
                }
                else if (this.dt1.Rows[0]["SewingLineID"] != this.dt2.Rows[0]["SewingLineID"])
                {
                        objSheet.Cells[12, 9] = string.Empty;
                        objSheet.Cells[13, 9] = string.Empty;
                        objSheet.Cells[14, 9] = string.Empty;
                        objSheet.Cells[14, 10] = string.Empty;
                        objSheet.Cells[14, 11] = string.Empty;
                }

                #endregion

                objSheet.Cells[18, 5] = this.dt1.Rows[0]["Type"].ToString() == "N" ? "New" : "Repeat";  // Classification
                objSheet.Cells[19, 4] = this.dt1.Rows[0]["Category"].ToString().Trim();  // Category
                objSheet.Cells[20, 4] = inline.ToShortDateString();  // Inline Date
                objSheet.Cells[21, 5] = inline.ToString("HH:mm");  // Inline Time(HH:mm)
                objSheet.Cells[22, 6] = firstOutputTime;  // Time of First Good Output (hh:mm):
                objSheet.Cells[22, 11] = lastOutputTime;  // Time of Last Good Output (hh:mm):

                objSheet.Cells[27, 3] = target_COPT;  // Target
                objSheet.Cells[27, 8] = target_COT;  // Target
                objSheet.Cells[27, 4] = this.dt1.Rows[0]["COPT"].ToString().Trim();  // Actual
                objSheet.Cells[27, 9] = this.dt1.Rows[0]["COT"].ToString().Trim();  // Actual

                #region 32列
                string sewingDate = MyUtility.GetValue.Lookup(string.Format(
                    @"select convert(varchar, min(a.OutputDate), 111) as SewingDate
                                                                    from SewingOutput a WITH (NOLOCK) 
                                                                    left join SewingOutput_Detail b WITH (NOLOCK) on a.ID=b.ID
                                                                    where b.OrderId='{0}'", orderID));
                string sewers_A = MyUtility.GetValue.Lookup(string.Format(
                    @"Select Distinct a.Manpower 
                                                                    from SewingOutput a WITH (NOLOCK) , SewingOutput_Detail b WITH (NOLOCK) 
                                                                    where a.Id = b.ID and a.OutputDate = '{0}' and a.Team = 'A' and a.FactoryID = '{1}' 
                                                                    and a.SewingLineID = '{2}' and b.OrderID = '{3}' and b.ComboType = '{4}'",
                    sewingDate,
                    factoryID,
                    sewingLineID,
                    orderID,
                    comboType));
                string sewers_B = MyUtility.GetValue.Lookup(string.Format(
                    @"Select Distinct a.Manpower 
                                                                    from SewingOutput a WITH (NOLOCK) , SewingOutput_Detail b WITH (NOLOCK) 
                                                                    where a.Id = b.ID and a.OutputDate = '{0}' and a.Team = 'B' and a.FactoryID = '{1}' 
                                                                    and a.SewingLineID = '{2}' and b.OrderID = '{3}' and b.ComboType = '{4}'",
                    sewingDate,
                    factoryID,
                    sewingLineID,
                    orderID,
                    comboType));
                string workingHours_A = MyUtility.GetValue.Lookup(string.Format(
                    @"Select Sum(b.WorkHour)  
                                                                    from SewingOutput a WITH (NOLOCK) , SewingOutput_Detail b WITH (NOLOCK) 
                                                                    where a.Id = b.ID and a.OutputDate = '{0}' and a.Team = 'A' and a.FactoryID = '{1}' 
                                                                    and a.SewingLineID = '{2}' and b.OrderID = '{3}' and b.ComboType = '{4}'",
                    sewingDate,
                    factoryID,
                    sewingLineID,
                    orderID,
                    comboType));
                string workingHours_B = MyUtility.GetValue.Lookup(string.Format(
                    @"Select Sum(b.WorkHour)  
                                                                    from SewingOutput a WITH (NOLOCK) , SewingOutput_Detail b WITH (NOLOCK) 
                                                                    where a.Id = b.ID and a.OutputDate = '{0}' and a.Team = 'B' and a.FactoryID = '{1}' 
                                                                    and a.SewingLineID = '{2}' and b.OrderID = '{3}' and b.ComboType = '{4}'",
                    sewingDate,
                    factoryID,
                    sewingLineID,
                    orderID,
                    comboType));
                string target = MyUtility.GetValue.Lookup(string.Format(
                    @"Select top 1 Target from ChgOverTarget  WITH (NOLOCK) 
                                                                    where Type = 'EFF.' and MDivisionID = '{0}' and EffectiveDate <= '{1}' 
                                                                    Order by EffectiveDate desc", mDivisionID,
                    inline.ToShortDateString()));
                string outputCMP_A = MyUtility.GetValue.Lookup(string.Format(
                    @"Select Sum(b.QAQty)  
                                                                    from SewingOutput a WITH (NOLOCK) , SewingOutput_Detail b WITH (NOLOCK)  
                                                                    where a.Id = b.ID and a.OutputDate = '{0}' and a.Team = 'A' and a.FactoryID = '{1}' 
                                                                    and a.SewingLineID = '{2}' and b.OrderID = '{3}' and b.ComboType = '{4}'",
                    sewingDate,
                    factoryID,
                    sewingLineID,
                    orderID,
                    comboType));
                string outputCMP_B = MyUtility.GetValue.Lookup(string.Format(
                    @"Select Sum(b.QAQty) 
                                                                    from SewingOutput a WITH (NOLOCK) , SewingOutput_Detail b  WITH (NOLOCK) 
                                                                    where a.Id = b.ID and a.OutputDate = '{0}' and a.Team = 'B' and a.FactoryID = '{1}' 
                                                                    and a.SewingLineID = '{2}' and b.OrderID = '{3}' and b.ComboType = '{4}'",
                    sewingDate,
                    factoryID,
                    sewingLineID,
                    orderID,
                    comboType));

                if (MyUtility.Check.Empty(cPU))
                {
                    cPU = "1";
                }

                string efficiency_A = MyUtility.GetValue.Lookup(string.Format(
                    @"Select IIF('{6}'=0,0,sum(b.QAQty * {5} * AAA.CpuRate) / Sum(a.Manpower * b.WorkHour)*3600 / '{6}'/100)
                                                                    from SewingOutput a WITH (NOLOCK) , SewingOutput_Detail b WITH (NOLOCK)  
                                                                    left join Orders c WITH (NOLOCK) on c.ID=b.OrderId
                                                                    cross apply dbo.GetCPURate(c.OrderTypeID,c.ProgramID,c.Category,c.BrandID,'O') as AAA
                                                                    where a.Id = b.ID and a.OutputDate = '{0}' and a.Team = 'A' and a.FactoryID = '{1}' and a.SewingLineID = '{2}' 
                                                                    and b.OrderID = '{3}' and b.ComboType = '{4}' ",
                    sewingDate,
                    factoryID,
                    sewingLineID,
                    orderID,
                    comboType,
                    cPU,
                    this.StdTMS));
                string efficiency_B = MyUtility.GetValue.Lookup(string.Format(
                    @"Select  IIF('{6}'=0,0,sum(b.QAQty * {5} * AAA.CpuRate) / Sum(a.Manpower * b.WorkHour)*3600 / '{6}'/100)  
                                                                    from SewingOutput a WITH (NOLOCK) , SewingOutput_Detail b WITH (NOLOCK) 
                                                                    left join Orders c WITH (NOLOCK) on c.ID=b.OrderId
                                                                    cross apply dbo.GetCPURate(c.OrderTypeID,c.ProgramID,c.Category,c.BrandID,'O') as AAA
                                                                    where a.Id = b.ID and a.OutputDate = '{0}' and a.Team = 'B' and a.FactoryID = '{1}' and a.SewingLineID = '{2}' 
                                                                    and b.OrderID = '{3}' and b.ComboType = '{4}' ",
                    sewingDate,
                    factoryID,
                    sewingLineID,
                    orderID,
                    comboType,
                    cPU,
                    this.StdTMS));
                string pPH_A = MyUtility.GetValue.Lookup(string.Format(
                    @"Select  sum(b.QAQty * {5} * AAA.CpuRate) / Sum(a.Manpower * b.WorkHour) 
                                                                    from SewingOutput a WITH (NOLOCK) , SewingOutput_Detail b WITH (NOLOCK)  
                                                                    left join Orders c WITH (NOLOCK) on c.ID=b.OrderId
                                                                    cross apply dbo.GetCPURate(c.OrderTypeID,c.ProgramID,c.Category,c.BrandID,'O') as AAA
                                                                    where a.Id = b.ID and a.OutputDate = '{0}' and a.Team = 'A' and a.FactoryID = '{1}' and a.SewingLineID = '{2}' 
                                                                    and b.OrderID = '{3}' and b.ComboType = '{4}' ",
                    sewingDate,
                    factoryID,
                    sewingLineID,
                    orderID,
                    comboType,
                    cPU));
                string pPH_B = MyUtility.GetValue.Lookup(string.Format(
                    @"Select  sum(b.QAQty * {5} * AAA.CpuRate) / Sum(a.Manpower * b.WorkHour) 
                                                                    from SewingOutput a WITH (NOLOCK) , SewingOutput_Detail b WITH (NOLOCK) 
                                                                    left join Orders c WITH (NOLOCK) on c.ID=b.OrderId
                                                                    cross apply dbo.GetCPURate(c.OrderTypeID,c.ProgramID,c.Category,c.BrandID,'O') as AAA
                                                                    where a.Id = b.ID and a.OutputDate = '{0}' and a.Team = 'B' and a.FactoryID = '{1}' and a.SewingLineID = '{2}' 
                                                                    and b.OrderID = '{3}' and b.ComboType = '{4}' ",
                    sewingDate,
                    factoryID,
                    sewingLineID,
                    orderID,
                    comboType,
                    cPU));

                objSheet.Cells[32, 1] = sewingDate;  // Date
                objSheet.Cells[32, 2] = sewers_A;  // No. of Sewers(Shift A)
                objSheet.Cells[32, 3] = sewers_B;  // No. of Sewers(Shift B)
                objSheet.Cells[32, 4] = workingHours_A;  // Working Hours(Shift A)
                objSheet.Cells[32, 5] = workingHours_B;  // Working Hours(Shift B)
                objSheet.Cells[32, 6] = target;  // Target(Eff)
                objSheet.Cells[32, 8] = outputCMP_A;  // Output (CMP) (Shift A)
                objSheet.Cells[32, 9] = outputCMP_B;  // Output (CMP) (Shift B)
                objSheet.Cells[32, 10] = efficiency_A;  // Efficiency (Shift A)
                objSheet.Cells[32, 11] = efficiency_B;  // Efficiency (Shift B)
                objSheet.Cells[32, 12] = pPH_A;  // PPH (Shift A)
                objSheet.Cells[32, 13] = pPH_B;  // PPH (Shift B)
                #endregion

                #region Problem Encountered
                DataTable dtProblem;
                string sql = string.Format(
                    @"Select a.IEReasonID , b.Description, a.ShiftA, a.ShiftB 
                                        from ChgOver_Problem a WITH (NOLOCK) , IEReason b WITH (NOLOCK) 
                                        where a.IEReasonID = b.ID and a.ID = '{0}' and b.Type = 'CP' ",
                    this.CurrentMaintain["ID"].ToString().Trim());
                DualResult result = DBProxy.Current.Select(null, sql, out dtProblem);

                // 若超過4筆資料，Excel就要在新增列數
                if (dtProblem.Rows.Count > 4)
                {
                    string rowNum, rowStr;
                    int repeat = dtProblem.Rows.Count - 4;

                    Microsoft.Office.Interop.Excel.Range rngToCopy = (Microsoft.Office.Interop.Excel.Range)objSheet.get_Range("A36:J36").EntireRow;
                    Microsoft.Office.Interop.Excel.Range rngToInsert;

                    for (int i = 0; i < repeat; i++)
                    {
                        rowNum = Convert.ToString(40 + i);
                        rowStr = string.Format("A{0}:M{0}", rowNum);
                        rngToInsert = (Microsoft.Office.Interop.Excel.Range)objSheet.get_Range(rowStr).EntireRow;
                        rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown, rngToCopy.Copy(Type.Missing));
                    }
                }

                // Clipboard.Clear();  //清空剪貼簿
                int count = 0;
                foreach (DataRow dr in dtProblem.Rows)
                {
                    objSheet.Cells[36 + count, 1] = dr["IEReasonID"].ToString().Trim() + ":" + dr["Description"].ToString().Trim();  // Problem Encountered
                    objSheet.Cells[36 + count, 6] = dr["ShiftA"].ToString().Trim();  // Shift A
                    objSheet.Cells[36 + count, 10] = dr["ShiftB"].ToString().Trim();  // Shift B
                    count++;
                }
                #endregion

                #region Save & Show Excel
                string strExcelName = Class.MicrosoftFile.GetName("IE_P02_ChangeoverReport");
                Microsoft.Office.Interop.Excel.Workbook workbook = objApp.ActiveWorkbook;
                workbook.SaveAs(strExcelName);
                workbook.Close();
                objApp.Quit();
                Marshal.ReleaseComObject(objSheet);    // 釋放sheet
                Marshal.ReleaseComObject(objApp);      // 釋放objApp
                Marshal.ReleaseComObject(workbook);

                strExcelName.OpenFile();
                #endregion
            }

            return true;
        }

        private DataRow GetType(string comboType, string cDCodeID)
        {
            DataRow returnValue = null;
            DataTable dtTemp;
            string sql;
            if (comboType == "T" || comboType == string.Empty)
            {
                sql = string.Format("select  TopProductionType as ProdType,TopFabricType as FabricType  from CDCode_Content WITH (NOLOCK) where ID='{0}'", cDCodeID);
            }
            else if (comboType == "B")
            {
                sql = string.Format("select BottomProductionType as ProdType,BottomFabricType as FabricType  from CDCode_Content WITH (NOLOCK) where ID='{0}'", cDCodeID);
            }
            else if (comboType == "I")
            {
                sql = string.Format("select InnerProductionType as ProdType,InnerFabricType as FabricType  from CDCode_Content WITH (NOLOCK) where ID='{0}'", cDCodeID);
            }
            else
            {
                sql = string.Format("select OuterProductionType as ProdType,OuterFabricType as FabricType  from CDCode_Content WITH (NOLOCK) where ID='{0}'", cDCodeID);
            }

            DualResult result = DBProxy.Current.Select(null, sql, out dtTemp);
            if (result && dtTemp.Rows.Count > 0)
            {
                returnValue = dtTemp.Rows[0];
            }

            return returnValue;
        }
    }
}
