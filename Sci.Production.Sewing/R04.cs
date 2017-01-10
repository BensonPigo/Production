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


namespace Sci.Production.Sewing
{
    public partial class R04 : Sci.Win.Tems.PrintForm
    {
        DataTable printData;
        DateTime? date1, date2;
        string category, mDivision, factory, brand, cdcode;
        public R04(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            MyUtility.Tool.SetupCombox(comboBox1, 1, 1, ",Bulk,Sample,Local Order,Mockup,Bulk+Sample");
            DataTable mDivision, factory;
            DBProxy.Current.Select(null, "select '' as ID union all select ID from MDivision", out mDivision);
            MyUtility.Tool.SetupCombox(comboBox2, 1, mDivision);
            DBProxy.Current.Select(null, "select '' as ID union all select distinct FtyGroup from Factory", out factory);
            MyUtility.Tool.SetupCombox(comboBox3, 1, factory);
            comboBox1.SelectedIndex = 0;
            comboBox2.Text = Sci.Env.User.Keyword;
            comboBox3.SelectedIndex = 0;
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(dateRange1.Value1))
            {
                MyUtility.Msg.WarningBox("Output Date can't empty!!");
                return false;
            }

            date1 = dateRange1.Value1;
            date2 = dateRange1.Value2;
            category = comboBox1.Text;
            mDivision = comboBox2.Text;
            factory = comboBox3.Text;
            brand = txtbrand1.Text;
            cdcode = txtcdcode1.Text;

            return base.ValidateInput();
        }

        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(string.Format(@"with tmpSewingDetail
as (
select s.OutputDate,s.Category,s.Shift,s.SewingLineID,IIF(sd.QAQty=0,s.Manpower,s.Manpower*sd.QAQty) as ActManPower,
s.Team,sd.OrderId,sd.ComboType,sd.WorkHour,sd.QAQty,sd.InlineQty,isnull(o.Category,'') as OrderCategory,
o.LocalOrder,isnull(o.BrandID,'') as OrderBrandID,isnull(o.CdCodeID,'') as OrderCdCodeID,
isnull(mo.BrandID,'') as MockupBrandID,isnull(mo.MockupID,'') as MockupCDCodeID,s.FactoryID,s.MDivisionID,
isnull(o.ProgramID,'') as OrderProgram,isnull(mo.ProgramID,'') as MockupProgram,isnull(o.OrderTypeID,'') as OrderType,
isnull(o.CPU,0) as OrderCPU,isnull(o.CPUFactor,0) as OrderCPUFactor,isnull(mo.Cpu,0) as MockupCPU,
isnull(mo.CPUFactor,0) as MockupCPUFactor,isnull(o.StyleID,'') as OrderStyle,isnull(mo.StyleID,'') as MockupStyle,
isnull(o.SeasonID,'') as OrderSeason,isnull(mo.SeasonID,'') as MockupSeason,isnull(sl.Rate,100)/100 as Rate,
(select StdTMS from System) as StdTMS,isnull(r.InspectQty,0) as InspectQty,isnull(r.RejectQty,0) as RejectQty
from SewingOutput s
inner join SewingOutput_Detail sd on sd.ID = s.ID
left join Orders o on o.ID = sd.OrderId
left join MockupOrder mo on mo.ID = sd.OrderId
left join Style_Location sl on sl.StyleUkey = o.StyleUkey and sl.Location = sd.ComboType
left join Rft r on r.OrderID = sd.OrderId and r.CDate = s.OutputDate and r.SewinglineID = s.SewingLineID and r.FactoryID = s.FactoryID and r.Shift = s.Shift and r.Team = s.Team
where s.OutputDate between '{0}' and '{1}'", Convert.ToDateTime(date1).ToString("d"), Convert.ToDateTime(date2).ToString("d")));
            if (!MyUtility.Check.Empty(mDivision))
            {
                sqlCmd.Append(string.Format(" and s.MDivisionID = '{0}'", mDivision));
            }
            if (!MyUtility.Check.Empty(factory))
            {
                sqlCmd.Append(string.Format(" and s.FactoryID = '{0}'", factory));
            }
            if (!MyUtility.Check.Empty(category) && category == "Mockup")
            {
                sqlCmd.Append(" and s.Category = 'M'");
            }

            sqlCmd.Append(@"),
tmpSewingGroup
as (
select OutputDate,Category,Shift,SewingLineID,Sum(ActManPower) as ActManPower,Team,OrderId,ComboType,
sum(WorkHour) as WorkHour,sum(QAQty) as QAQty,sum(InlineQty) as InlineQty,OrderCategory,
LocalOrder,OrderBrandID,OrderCdCodeID,MockupBrandID,MockupCDCodeID,FactoryID,MDivisionID,
OrderProgram,MockupProgram,OrderType,OrderCPU,OrderCPUFactor,MockupCPU,MockupCPUFactor,OrderStyle,
MockupStyle,OrderSeason,MockupSeason,Rate,StdTMS,InspectQty,RejectQty
from tmpSewingDetail
group by OutputDate,Category,Shift,SewingLineID,Team,OrderId,ComboType,OrderCategory,LocalOrder,OrderBrandID,
OrderCdCodeID,MockupBrandID,MockupCDCodeID,FactoryID,MDivisionID,OrderProgram,MockupProgram,OrderType,
OrderCPU,OrderCPUFactor,MockupCPU,MockupCPUFactor,OrderStyle,MockupStyle,OrderSeason,MockupSeason,Rate,StdTMS,InspectQty,RejectQty
),
tmp1stFilter
as (
select t.*,IIF(t.Shift <> 'O' and t.Category <> 'M' and t.LocalOrder = 1, 'I',t.Shift) as LastShift,
f.Type as FtyType,f.CountryID as FtyCountry,[dbo].getSewingOutputCumulateOfDays(IIF(t.Category <> 'M',OrderStyle,MockupStyle),SewingLineID,OutputDate,FactoryID) as CumulateDate
from tmpSewingGroup t
left join Factory f on t.FactoryID = f.ID
where 1=1");
            if (!MyUtility.Check.Empty(category) && category != "Mockup")
            {
                if (category != "Bulk")
                {
                    sqlCmd.Append(" and t.OrderCategory = 'B'");
                }
                else if (category != "Sample")
                {
                    sqlCmd.Append(" and t.OrderCategory = 'S'");
                }
                else if (category != "Bulk+Sample")
                {
                    sqlCmd.Append(" and (t.OrderCategory = 'B' or t.OrderCategory = 'S')");
                }
                else
                {
                    sqlCmd.Append(" and t.LocalOrder = 1");
                }
            }
            if (!MyUtility.Check.Empty(brand))
            {
                sqlCmd.Append(string.Format(" and (t.OrderBrandID = '{0}' or t.MockupBrandID = '{0}')", brand));
            }
            if (!MyUtility.Check.Empty(cdcode))
            {
                sqlCmd.Append(string.Format(" and (t.OrderCdCodeID = '{0}' or t.MockupCDCodeID = '{0}')", cdcode));
            }

            sqlCmd.Append(@")
select MDivisionID,FactoryID,FtyType,FtyCountry,OutputDate,SewingLineID,
IIF(LastShift='D','Day',IIF(LastShift='N','Night',IIF(LastShift='O','Subcon-Out','Subcon-In'))) as Shift,
Team,OrderId,IIF(Category='M',MockupBrandID,OrderBrandID) as Brand,
IIF(Category='M','Mockup',IIF(LocalOrder = 1,'Local Order',IIF(OrderCategory='B','Bulk',IIF(OrderCategory='S','Sample','')))) as Category,
IIF(Category='M',MockupProgram,OrderProgram) as Program,OrderType,IIF(Category='M',MockupCPUFactor,OrderCPUFactor) as CPURate,
IIF(Category='M',MockupStyle,OrderStyle) as Style,IIF(Category='M',MockupSeason,OrderSeason) as Season,
IIF(Category='M',MockupCDCodeID,OrderCdCodeID)+'-'+ComboType as CDNo,IIF(QAQty>0,ActManPower/QAQty,ActManPower) as ActManPower,
WorkHour,ROUND(IIF(QAQty>0,ActManPower/QAQty,ActManPower)*WorkHour,2) as ManHour,
ROUND(ROUND(IIF(QAQty>0,ActManPower/QAQty,ActManPower)*WorkHour,2)*3600/StdTMS,2) as TargetCPU,
IIF(Category='M',MockupCPU*MockupCPUFactor,OrderCPU*OrderCPUFactor*Rate)*StdTMS as TMS,
IIF(Category='M',MockupCPU*MockupCPUFactor,OrderCPU*OrderCPUFactor*Rate) as CPUPrice,
IIF(IIF(Category='M',MockupCPU*MockupCPUFactor,OrderCPU*OrderCPUFactor*Rate)>0,ROUND(ROUND(IIF(QAQty>0,ActManPower/QAQty,ActManPower)*WorkHour,2)*3600/StdTMS,2)/IIF(Category='M',MockupCPU*MockupCPUFactor,OrderCPU*OrderCPUFactor*Rate),0) as TargetQty,
QAQty,IIF(Category='M',MockupCPU*MockupCPUFactor,OrderCPU*OrderCPUFactor*Rate)*QAQty as TotalCPU,
IIF(ROUND(IIF(QAQty>0,ActManPower/QAQty,ActManPower)*WorkHour,2)>0,(IIF(Category='M',MockupCPU*MockupCPUFactor,OrderCPU*OrderCPUFactor*Rate)*QAQty)/ROUND(IIF(QAQty>0,ActManPower/QAQty,ActManPower)*WorkHour,2),0) as CPUSewer,
ROUND(IIF(ROUND(IIF(QAQty>0,ActManPower/QAQty,ActManPower)*WorkHour,2)>0,((IIF(Category='M',MockupCPU*MockupCPUFactor,OrderCPU*OrderCPUFactor*Rate)*QAQty)/(ROUND(IIF(QAQty>0,ActManPower/QAQty,ActManPower)*WorkHour,2)*3600/StdTMS))*100,0),1) as EFF,
IIF(InspectQty>0,ROUND((InspectQty-RejectQty)/InspectQty*100,2),0) as RFT,CumulateDate,
IIF(CumulateDate>=10,'>=10',CONVERT(VARCHAR,CumulateDate)) as DateRange,InlineQty,QAQty-InlineQty as Diff
from tmp1stFilter
order by MDivisionID,FactoryID,OutputDate,SewingLineID,LastShift,Team,OrderId");
            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }
            
            return Result.True;
        }

        // 產生Excel
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            SetCount(printData.Rows.Count);

            if (printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            this.ShowWaitMessage("Starting EXCEL...");
            string excelFile = "Sewing_R04_SewingDailyOutputList.xltx";
            bool result = MyUtility.Excel.CopyToXls(printData, "", xltfile: excelFile, headerRow: 1);
            if (!result) { MyUtility.Msg.WarningBox(result.ToString(), "Warning"); }
            this.HideWaitMessage();
            return true;
        }
    }
}
