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
    public partial class R02 : Sci.Win.Tems.PrintForm
    {
        string line1, line2, factory, factoryName;
        DateTime? date1, date2;
        int excludeHolday, excludeSubconin, reportType, orderby;
        DataTable SewOutPutData, printData, excludeInOutTotal, cpuFactor, subprocessData, subconData;
        public R02(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            label10.Text = "** The value in this report are all excluded \r\nsubcon-out, unless the column with \r\n\"included subcon-out\".";
            DataTable factory;
            DBProxy.Current.Select(null, @"select '' as FtyGroup 
union all
select distinct FTYGroup from Factory order by FTYGroup", out factory);

            MyUtility.Tool.SetupCombox(comboBox1, 1, 1, "Included,Excluded");
            MyUtility.Tool.SetupCombox(comboBox2, 1, 1, "Included,Excluded");
            MyUtility.Tool.SetupCombox(comboBox3, 1, 1, "By Date,By Sewing Line");
            MyUtility.Tool.SetupCombox(comboBox4, 1, factory);
            MyUtility.Tool.SetupCombox(comboBox5, 1, 1, "Sewing Line,CPU/Sewer/HR");
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;
            comboBox4.Text = Sci.Env.User.Factory;
            comboBox5.SelectedIndex = 0;
        }

        //Date
        private void dateBox1_Validated(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(dateBox1.Value))
            {
                dateBox2.Value = null;
            }
            else
            {
                dateBox2.Value = (Convert.ToDateTime(dateBox1.Value).AddDays(1 - Convert.ToDateTime(dateBox1.Value).Day)).AddMonths(1).AddDays(-1);
            }
        }

        //Report Type
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox3.SelectedIndex == 0)
            {
                label7.Visible = false;
                comboBox5.Visible = false;
            }
            else
            {
                label7.Visible = true;
                comboBox5.Visible = true;
            }
        }

        private string SelectSewingLine(string line)
        {
            string sql = string.Format("Select Distinct ID From SewingLine{0}", MyUtility.Check.Empty(comboBox4.Text) ? "" : string.Format(" where FactoryID = '{0}'", comboBox4.Text));
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sql, "3", line, false, ",");
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return "";
            }
            else
            {
                return item.GetSelectedString();
            }
        }

        //Sewing Line
        private void textBox1_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            textBox1.Text = SelectSewingLine(textBox1.Text);
        }

        //Sewing Line
        private void textBox2_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            textBox2.Text = SelectSewingLine(textBox2.Text);
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(dateBox1.Value))
            {
                MyUtility.Msg.WarningBox("Date can't empty!!");
                return false;
            }

            if (comboBox1.SelectedIndex == -1)
            {
                MyUtility.Msg.WarningBox("Holiday can't empty!!");
                return false;
            }

            if (comboBox2.SelectedIndex == -1)
            {
                MyUtility.Msg.WarningBox("Subcon-in can't empty!!");
                return false;
            }

            if (comboBox3.SelectedIndex == -1)
            {
                MyUtility.Msg.WarningBox("Report type can't empty!!");
                return false;
            }

            if (comboBox3.SelectedIndex == 1)
            {
                if (comboBox4.SelectedIndex == -1 || comboBox4.SelectedIndex == 0)
                {
                    MyUtility.Msg.WarningBox("Factory can't empty!!");
                    return false;
                }

                if (comboBox5.SelectedIndex == -1)
                {
                    MyUtility.Msg.WarningBox("Order by can't empty!!");
                    return false;
                }
            }
            date1 = dateBox1.Value;
            date2 = dateBox2.Value;
            line1 = textBox1.Text;
            line2 = textBox2.Text;
            factory = comboBox4.Text;
            excludeHolday = comboBox1.SelectedIndex;
            excludeSubconin = comboBox2.SelectedIndex;
            reportType = comboBox3.SelectedIndex;
            orderby = comboBox5.SelectedIndex;
            return base.ValidateInput();
        }

        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            #region 組撈全部Sewing output data SQL
            sqlCmd.Append(string.Format(@"with tmpSewingDetail
as (
select s.OutputDate,s.Category,s.Shift,s.SewingLineID,IIF(sd.QAQty=0,s.Manpower,s.Manpower*sd.QAQty) as ActManPower,
s.Team,sd.OrderId,sd.ComboType,sd.WorkHour,sd.QAQty,sd.InlineQty,isnull(o.Category,'') as OrderCategory,
o.LocalOrder,s.FactoryID,isnull(o.ProgramID,'') as OrderProgram,isnull(mo.ProgramID,'') as MockupProgram,
isnull(o.CPU,0) as OrderCPU,isnull(o.CPUFactor,0) as OrderCPUFactor,isnull(mo.Cpu,0) as MockupCPU,
isnull(mo.CPUFactor,0) as MockupCPUFactor,isnull(o.StyleID,'') as OrderStyle,isnull(mo.StyleID,'') as MockupStyle,isnull(sl.Rate,100)/100 as Rate,
(select StdTMS from System) as StdTMS
from SewingOutput s
inner join SewingOutput_Detail sd on sd.ID = s.ID
left join Orders o on o.ID = sd.OrderId
left join MockupOrder mo on mo.ID = sd.OrderId
left join Style_Location sl on sl.StyleUkey = o.StyleUkey and sl.Location = sd.ComboType
where s.OutputDate between '{0}' and '{1}'", Convert.ToDateTime(date1).ToString("d"), Convert.ToDateTime(date2).ToString("d")));
            if (!MyUtility.Check.Empty(line1))
            {
                sqlCmd.Append(string.Format(" and s.SewingLineID >= '{0}'",line1));
            }
            if (!MyUtility.Check.Empty(line2))
            {
                sqlCmd.Append(string.Format(" and s.SewingLineID <= '{0}'",line2));
            }
            if (!MyUtility.Check.Empty(factory))
            {
                sqlCmd.Append(string.Format(" and s.FactoryID = '{0}'",factory));
            }

            sqlCmd.Append(@"
),
tmpSewingGroup
as (
select OutputDate,Category,Shift,SewingLineID,Sum(ActManPower) as ActManPower1,Team,OrderId,ComboType,
sum(WorkHour) as WorkHour,sum(QAQty) as QAQty,sum(InlineQty) as InlineQty,OrderCategory,
LocalOrder,FactoryID,OrderProgram,MockupProgram,OrderCPU,OrderCPUFactor,MockupCPU,MockupCPUFactor,OrderStyle,
MockupStyle,Rate,StdTMS,IIF(Shift <> 'O' and Category <> 'M' and LocalOrder = 1, 'I',Shift) as LastShift
from tmpSewingDetail
group by OutputDate,Category,Shift,SewingLineID,Team,OrderId,ComboType,OrderCategory,LocalOrder,
FactoryID,OrderProgram,MockupProgram,
OrderCPU,OrderCPUFactor,MockupCPU,MockupCPUFactor,OrderStyle,MockupStyle,Rate,StdTMS
),
tmp1stFilter
as (
select t.*,isnull(w.Holiday,0) as Holiday,IIF(isnull(QAQty,0)=0,ActManPower1,(ActManPower1/QAQty)) as ActManPower
from tmpSewingGroup t
left join WorkHour w on w.FactoryID = t.FactoryID and w.Date = t.OutputDate and w.SewingLineID = t.SewingLineID
where 1 = 1");
            if (excludeSubconin == 1)
            {
                sqlCmd.Append(" and t.LastShift <> 'I'");
            }
            sqlCmd.Append(@"
),
tmp2ndFilter
as (
select * 
from tmp1stFilter t
where 1 = 1");
            if (excludeHolday == 1)
            {
                sqlCmd.Append(" and Holiday = 0");
            }
            sqlCmd.Append(@"
)
select OutputDate,IIF(LastShift='D','Day',IIF(LastShift='N','Night',IIF(LastShift='O','Subcon-Out','Subcon-In'))) as Shift,
Team,SewingLineID,OrderId,IIF(Category='M',MockupStyle,OrderStyle) as Style,QAQty,ActManPower,
IIF(Category='M',MockupProgram,OrderProgram) as Program,
WorkHour,StdTMS,MockupCPU,MockupCPUFactor,OrderCPU,OrderCPUFactor,Rate,Category,LastShift,ComboType,FactoryID
from tmp2ndFilter");
            #endregion
            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out SewOutPutData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query sewing output data fail\r\n" + result.ToString());
                return failResult;
            }

            #region 整理列印資料
            if (reportType == 0)
            {
                try
                {
                    #region 組SQL
                    MyUtility.Tool.ProcessWithDatatable(SewOutPutData, "OutputDate,StdTMS,QAQty,WorkHour,ActManPower,LastShift,MockupCPU,MockupCPUFactor,OrderCPU,OrderCPUFactor,Rate,FactoryID,SewingLineID,Team,Category",
                        @";with tmpQty 
as (
select OutputDate,StdTMS,Sum(QAQty) as QAQty, Sum(WorkHour*ActManPower) as ManHour
from #tmp
where LastShift <> 'O'
group by OutputDate,StdTMS
),
tmpTtlCPU
as (
select OutputDate,Sum(QAQty*IIF(Category = 'M', MockupCPU * MockupCPUFactor, OrderCPU * OrderCPUFactor * Rate)) as TotalCPU
from #tmp
where LastShift <> 'O'
group by OutputDate
),
tmpSubconInCPU
as (
select OutputDate,Sum(QAQty*IIF(Category = 'M', MockupCPU * MockupCPUFactor, OrderCPU * OrderCPUFactor * Rate)) as TotalCPU
from #tmp
where LastShift = 'I'
group by OutputDate
),
tmpSubconOutCPU
as (
select OutputDate,Sum(QAQty*IIF(Category = 'M', MockupCPU * MockupCPUFactor, OrderCPU * OrderCPUFactor * Rate)) as TotalCPU
from #tmp
where LastShift = 'O'
group by OutputDate
),
tmpTtlManPower
as (
select OutputDate,Sum(Manpower) as ManPower from (
select OutputDate,FactoryID,SewingLineID,LastShift,Team,Max(ActManPower) as ManPower
from #tmp
where LastShift <> 'O'
group by OutputDate,FactoryID,SewingLineID,LastShift,Team) a
group by OutputDate
)
select q.OutputDate,q.QAQty,tc.TotalCPU,isnull(ic.TotalCPU,0) as SInCPU,isnull(oc.TotalCPU,0) as SoutCPU,
IIF(q.ManHour = 0,0,Round(isnull(tc.TotalCPU,0)/q.ManHour,2)) as CPUSewer,
IIF(isnull(mp.ManPower,0) = 0,0,Round(q.ManHour/mp.ManPower,2)) as AvgWorkHour,mp.ManPower,q.ManHour,
IIF(q.ManHour*q.StdTMS = 0,0,Round(tc.TotalCPU/(q.ManHour*3600/q.StdTMS)*100,2)) as Eff
from tmpQty q
left join tmpTtlCPU tc on tc.OutputDate = q.OutputDate
left join tmpSubconInCPU ic on ic.OutputDate = q.OutputDate
left join tmpSubconOutCPU oc on oc.OutputDate = q.OutputDate
left join tmpTtlManPower mp on mp.OutputDate = q.OutputDate
order by q.OutputDate",
                        out printData);
                    #endregion
                }
                catch (Exception ex)
                {
                    DualResult failResult = new DualResult(false, "Query print data fail\r\n" + ex.ToString());
                    return failResult;
                }
            }
            else
            {
                try
                {
                    #region 組SQL
                    string sqlcommand = string.Format(@";with tmpQty 
as (
select SewingLineID,StdTMS,Sum(QAQty) as QAQty, Sum(WorkHour*ActManPower) as ManHour
from #tmp
where LastShift <> 'O'
group by SewingLineID,StdTMS
),
tmpTtlCPU
as (
select SewingLineID,Sum(QAQty*IIF(Category = 'M', MockupCPU * MockupCPUFactor, OrderCPU * OrderCPUFactor * Rate)) as TotalCPU
from #tmp
where LastShift <> 'O'
group by SewingLineID
),
tmpSubconInCPU
as (
select SewingLineID,Sum(QAQty*IIF(Category = 'M', MockupCPU * MockupCPUFactor, OrderCPU * OrderCPUFactor * Rate)) as TotalCPU
from #tmp
where LastShift = 'I'
group by SewingLineID
),
tmpSubconOutCPU
as (
select SewingLineID,Sum(QAQty*IIF(Category = 'M', MockupCPU * MockupCPUFactor, OrderCPU * OrderCPUFactor * Rate)) as TotalCPU
from #tmp
where LastShift = 'O'
group by SewingLineID
),
tmpTtlManPower
as (
select SewingLineID,Sum(Manpower) as ManPower from (
select OutputDate,FactoryID,SewingLineID,LastShift,Team,Max(ActManPower) as ManPower
from #tmp
where LastShift <> 'O'
group by OutputDate,FactoryID,SewingLineID,LastShift,Team) a
group by SewingLineID
)
select q.SewingLineID,q.QAQty,tc.TotalCPU,isnull(ic.TotalCPU,0) as SInCPU,isnull(oc.TotalCPU,0) as SoutCPU,
IIF(q.ManHour = 0,0,Round(isnull(tc.TotalCPU,0)/q.ManHour,2)) as CPUSewer,
IIF(isnull(mp.ManPower,0) = 0,0,Round(q.ManHour/mp.ManPower,2)) as AvgWorkHour,mp.ManPower,q.ManHour,
IIF(q.ManHour*q.StdTMS = 0,0,Round(tc.TotalCPU/(q.ManHour*3600/q.StdTMS)*100,2)) as Eff
from tmpQty q
left join tmpTtlCPU tc on tc.SewingLineID = q.SewingLineID
left join tmpSubconInCPU ic on ic.SewingLineID = q.SewingLineID
left join tmpSubconOutCPU oc on oc.SewingLineID = q.SewingLineID
left join tmpTtlManPower mp on mp.SewingLineID = q.SewingLineID
order by {0}", orderby == 0 ? "q.SewingLineID" : "IIF(q.ManHour = 0,0,Round(isnull(oc.TotalCPU,0)/q.ManHour,2))");
                    MyUtility.Tool.ProcessWithDatatable(SewOutPutData, "OutputDate,StdTMS,QAQty,WorkHour,ActManPower,LastShift,MockupCPU,MockupCPUFactor,OrderCPU,OrderCPUFactor,Rate,FactoryID,SewingLineID,Team,Category",
                        sqlcommand,  out printData);
                    #endregion
                }
                catch (Exception ex)
                {
                    DualResult failResult = new DualResult(false, "Query print data fail\r\n" + ex.ToString());
                    return failResult;
                }
            }
            #endregion

            #region 整理Total Exclude Subcon-In & Out
            try
            {
                #region 組SQL
                MyUtility.Tool.ProcessWithDatatable(SewOutPutData, "OutputDate,StdTMS,QAQty,WorkHour,ActManPower,LastShift,MockupCPU,MockupCPUFactor,OrderCPU,OrderCPUFactor,Rate,FactoryID,SewingLineID,Team,Category",
                    @";with tmpQty 
as (
select StdTMS,Sum(QAQty) as QAQty, Sum(WorkHour*ActManPower) as ManHour,
Sum(QAQty*IIF(Category = 'M', MockupCPU * MockupCPUFactor, OrderCPU * OrderCPUFactor * Rate)) as TotalCPU
from #tmp
where LastShift <> 'O' and LastShift <> 'I'
group by StdTMS
),
tmpTtlManPower
as (
select Sum(Manpower) as ManPower from (
select OutputDate,FactoryID,SewingLineID,LastShift,Team,Max(ActManPower) as ManPower
from #tmp
where LastShift <> 'O'
group by OutputDate,FactoryID,SewingLineID,LastShift,Team) a
)
select q.QAQty,q.TotalCPU,
IIF(q.ManHour = 0,0,Round(isnull(q.TotalCPU,0)/q.ManHour,2)) as CPUSewer,
IIF(isnull(mp.ManPower,0) = 0,0,Round(q.ManHour/mp.ManPower,2)) as AvgWorkHour,mp.ManPower,q.ManHour,
IIF(q.ManHour*q.StdTMS = 0,0,Round(q.TotalCPU/(q.ManHour*3600/q.StdTMS)*100,2)) as Eff
from tmpQty q
left join tmpTtlManPower mp on 1 = 1",
                    out excludeInOutTotal);
                #endregion
            }
            catch (Exception ex)
            {
                DualResult failResult = new DualResult(false, "Query total data fail\r\n" + ex.ToString());
                return failResult;
            }
            #endregion 

            #region 整理CPU Factor
            try
            {
                #region 組SQL
                MyUtility.Tool.ProcessWithDatatable(SewOutPutData, "Category,MockupCPUFactor,OrderCPUFactor,QAQty,MockupCPU,OrderCPU,Rate,Style",
                    @";with tmpData
as (
select IIF(Category = 'M',MockupCPUFactor,OrderCPUFactor) as CPUFactor, QAQty,QAQty*IIF(Category = 'M', MockupCPU * MockupCPUFactor, OrderCPU * OrderCPUFactor * Rate) as CPU,Style
from #tmp
),
tmpSumQAQty
as (
select CPUFactor,sum(QAQty) as QAQty from tmpData group by CPUFactor
),
tmpSumCPU
as (
select CPUFactor,sum(CPU) as CPU from tmpData group by CPUFactor
),
tmpCountStyle
as (
select CPUFactor,COUNT(distinct Style) as Style from tmpData group by CPUFactor
)
select q.* ,c.CPU,s.Style
from tmpSumQAQty q
left join tmpSumCPU c on q.CPUFactor = c.CPUFactor
left join tmpCountStyle s on q.CPUFactor = s.CPUFactor",
                    out cpuFactor);
                #endregion
            }
            catch (Exception ex)
            {
                DualResult failResult = new DualResult(false, "Query CPU factor data fail\r\n" + ex.ToString());
                return failResult;
            }
            #endregion

            #region 整理Subprocess資料
            if (printData.Rows.Count > 0)
            {
                try
                {
                    MyUtility.Tool.ProcessWithDatatable(SewOutPutData, "OrderId,ComboType,QAQty,LastShift",
                        @";with tmpArtwork
as (
Select ID from ArtworkType where Classify in ('I','A','P') and IsTtlTMS = 0
),
tmpAllSubprocess
as (
select ot.ArtworkTypeID,a.OrderId,a.ComboType,Round(sum(a.QAQty)*ot.Price*(isnull(sl.Rate,100)/100),2) as Price
from #tmp a
inner join Order_TmsCost ot on ot.ID = a.OrderId
inner join Orders o on o.ID = a.OrderId
inner join tmpArtwork ta on ta.ID = ot.ArtworkTypeID
left join Style_Location sl on sl.StyleUkey = o.StyleUkey and sl.Location = a.ComboType
where ((a.LastShift = 'O' and o.LocalOrder <> 1) or (a.LastShift <> 'O')) 
and ot.Price > 0
group by ot.ArtworkTypeID,a.OrderId,a.ComboType,ot.Price,sl.Rate
)
select ArtworkTypeID,sum(Price) as Price
from tmpAllSubprocess
group by ArtworkTypeID",
                        out subprocessData);
                }
                catch (Exception ex)
                {
                    DualResult failResult = new DualResult(false, "Query sub process data fail\r\n" + ex.ToString());
                    return failResult;
                }
            }
            #endregion

            #region 整理Subcon資料
            if (printData.Rows.Count > 0)
            {
                try
                {
                    MyUtility.Tool.ProcessWithDatatable(SewOutPutData, "SewingLineID,QAQty,LastShift,MockupCPU,MockupCPUFactor,OrderCPU,OrderCPUFactor,Rate,OrderId,Program,Category,FactoryID",
                        @";with tmpSubconIn
as (
Select 'I' as Type,Program as Company,sum(QAQty*IIF(Category = 'M', MockupCPU * MockupCPUFactor, OrderCPU * OrderCPUFactor * Rate)) as TtlCPU, '' as SewingLineID
from #tmp
where LastShift = 'I'
group by Program
),
tmpSubconOut
as (
Select 'O' as Type,s.Description as Company,sum(t.QAQty*IIF(t.Category = 'M', t.MockupCPU * t.MockupCPUFactor, t.OrderCPU * t.OrderCPUFactor * t.Rate)) as TtlCPU, t.SewingLineID
from #tmp t
left join SewingLine s on s.ID = t.SewingLineID and s.FactoryID = t.FactoryID
where LastShift = 'O'
group by s.Description,t.SewingLineID
)
select * from tmpSubconIn
union all
select * from tmpSubconOut",
                        out subconData);
                }
                catch (Exception ex)
                {
                    DualResult failResult = new DualResult(false, "Query subcon data fail\r\n" + ex.ToString());
                    return failResult;
                }
            }
            #endregion
            factoryName = MyUtility.GetValue.Lookup(string.Format("select NameEN from Factory where ID = '{0}'", Sci.Env.User.Factory));
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
            string strXltName = Sci.Env.Cfg.XltPathDir + (reportType == 0 ? "\\Sewing_R02_MonthlyReportByDate.xltx" : "\\Sewing_R02_MonthlyReportBySewingLine.xltx");
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null) return false;
            //excel.Visible = true;
            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            worksheet.Cells[2, 1] = string.Format("Fm:{0}",factoryName);
            worksheet.Cells[3, 1] = string.Format("{0} Monthly CMP Report, MTH:{1} ({2}{3}) {4}",
                MyUtility.Check.Empty(factory) ? "All Factory" : factory, Convert.ToDateTime(date1).ToString("yyyy/MM"), excludeHolday == 0 ? "Included Holiday" : "Excluded Holiday", excludeSubconin == 0 ? ", Subcon-In" : "", reportType == 0 ? "" : "By Sewing Line");
            worksheet.Cells[4, 3] = excludeSubconin == 0 ? "Total CPU Included Subcon-In" : "Total CPU";

            int insertRow = 5;
            object[,] objArray = new object[1, 11];
            foreach (DataRow dr in printData.Rows)
            {
                objArray[0, 0] = dr[0];
                objArray[0, 1] = dr[1];
                objArray[0, 2] = dr[2];
                objArray[0, 3] = dr[3];
                objArray[0, 4] = dr[4];
                objArray[0, 5] = dr[5];
                objArray[0, 6] = dr[6];
                objArray[0, 7] = dr[7];
                objArray[0, 8] = dr[8];
                objArray[0, 9] = dr[9];
                objArray[0, 10] = "";
                worksheet.Range[String.Format("A{0}:K{0}", insertRow)].Value2 = objArray;
                insertRow++;
                //插入一筆Record
                Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(insertRow)), Type.Missing).EntireRow;
                rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
            }
            //將多出來的Record刪除
            DeleteExcelRow(2, insertRow, excel);

            //Total
            worksheet.Cells[insertRow, 2] = string.Format("=SUM(B5:B{0})", MyUtility.Convert.GetString(insertRow - 1));
            worksheet.Cells[insertRow, 3] = string.Format("=SUM(C5:C{0})", MyUtility.Convert.GetString(insertRow - 1));
            worksheet.Cells[insertRow, 4] = string.Format("=SUM(D5:D{0})", MyUtility.Convert.GetString(insertRow - 1));
            worksheet.Cells[insertRow, 5] = string.Format("=SUM(E5:E{0})", MyUtility.Convert.GetString(insertRow - 1));
            worksheet.Cells[insertRow, 6] = string.Format("=ROUND(C{0}/I{0},2)", MyUtility.Convert.GetString(insertRow));
            worksheet.Cells[insertRow, 7] = string.Format("=ROUND(I{0}/H{0},2)", MyUtility.Convert.GetString(insertRow));
            worksheet.Cells[insertRow, 8] = string.Format("=SUM(H5:H{0})", MyUtility.Convert.GetString(insertRow - 1));
            worksheet.Cells[insertRow, 9] = string.Format("=SUM(I5:I{0})", MyUtility.Convert.GetString(insertRow - 1));
            worksheet.Cells[insertRow, 10] = string.Format("=ROUND(C{0}/(I{0}*60*60/1400)*100,2)", MyUtility.Convert.GetString(insertRow));
            insertRow++;
            worksheet.Cells[insertRow, 2] = MyUtility.Convert.GetString(excludeInOutTotal.Rows[0]["QAQty"]);
            worksheet.Cells[insertRow, 3] = MyUtility.Convert.GetString(excludeInOutTotal.Rows[0]["TotalCPU"]);
            worksheet.Cells[insertRow, 6] = MyUtility.Convert.GetString(excludeInOutTotal.Rows[0]["CPUSewer"]);
            worksheet.Cells[insertRow, 7] = MyUtility.Convert.GetString(excludeInOutTotal.Rows[0]["AvgWorkHour"]);
            worksheet.Cells[insertRow, 8] = MyUtility.Convert.GetString(excludeInOutTotal.Rows[0]["ManPower"]);
            worksheet.Cells[insertRow, 9] = MyUtility.Convert.GetString(excludeInOutTotal.Rows[0]["ManHour"]);
            worksheet.Cells[insertRow, 10] = MyUtility.Convert.GetString(excludeInOutTotal.Rows[0]["Eff"]);
            
            //CPU Factor
            insertRow = insertRow + 2;
            worksheet.Cells[insertRow, 3] = excludeSubconin == 0 ? "Total CPU Included Subcon-In" : "Total CPU";
            insertRow++;
            if (cpuFactor.Rows.Count > 2)
            {
                //插入Record
                for (int i = 0; i < cpuFactor.Rows.Count - 2; i++)
                {
                    Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(insertRow+1)), Type.Missing).EntireRow;
                    rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                }
            }
            objArray = new object[1, 4];
            foreach (DataRow dr in cpuFactor.Rows)
            {
                objArray[0, 0] = string.Format("CPU * {0}",MyUtility.Convert.GetString(MyUtility.Math.Round(MyUtility.Convert.GetDecimal(dr["CPUFactor"]),1)));
                objArray[0, 1] = dr["QAQty"];
                objArray[0, 2] = dr["CPU"];
                objArray[0, 3] = dr["Style"];
                
                worksheet.Range[String.Format("A{0}:D{0}", insertRow)].Value2 = objArray;
                insertRow++;
                Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(insertRow)), Type.Missing).EntireRow;
                rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
            }

            DeleteExcelRow(2, insertRow, excel);
            //insertRow

            //Borders.LineStyle 儲存格邊框線
            //Microsoft.Office.Interop.Excel.Range excelRange 
            //    = worksheet.get_Range(string.Format("A{0}:K{0}", MyUtility.Convert.GetString(insertRow)), Type.Missing);
            //excelRange.Borders.LineStyle = 3;
            //excelRange.Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop).LineStyle 
            //    = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;

            //Subprocess
            insertRow = insertRow + 2;
            int insertRec = 0;
            foreach (DataRow dr in subprocessData.Rows)
            {
                insertRec++;
                if (insertRec % 2 == 1)
                {
                    worksheet.Cells[insertRow, 2] = string.Format("{0}CMP", MyUtility.Convert.GetString(dr["ArtworkTypeID"]).PadRight(20, ' '));
                    worksheet.Cells[insertRow, 4] = MyUtility.Convert.GetString(dr["Price"]);
                }
                else
                {
                    worksheet.Cells[insertRow, 6] = string.Format("{0}CMP", MyUtility.Convert.GetString(dr["ArtworkTypeID"]).PadRight(20, ' '));
                    worksheet.Cells[insertRow, 8] = MyUtility.Convert.GetString(dr["Price"]);
                    insertRow++;
                    //插入一筆Record
                    Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(insertRow)), Type.Missing).EntireRow;
                    rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                }
            }

            //Subcon
            insertRow = insertRow + 4;
            int insertSubconIn = 0, insertSubconOut = 0;
            objArray = new object[1, 3];
            if (subconData.Rows.Count > 0)
            {
                foreach (DataRow dr in subconData.Rows)
                {
                    if (MyUtility.Convert.GetString(dr["Type"]) == "I")
                    {
                        insertRow++;
                        insertSubconIn = 1;
                        objArray[0, 0] = dr["Company"];
                        objArray[0, 1] = "";
                        objArray[0, 2] = dr["TtlCPU"];
                        worksheet.Range[String.Format("A{0}:C{0}", insertRow)].Value2 = objArray;

                        //插入一筆Record
                        Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(insertRow+1)), Type.Missing).EntireRow;
                        rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                    }
                    else
                    {
                        if (insertSubconOut == 0)
                        {
                            if (insertSubconIn == 0)
                            {
                                //刪除資料
                                DeleteExcelRow(5, insertRow, excel);
                            }
                            else
                            {
                                //刪除資料
                                DeleteExcelRow(2, insertRow+1, excel);
                                insertRow = insertRow + 3;
                            }
                        }

                        insertSubconOut = 1;
                        insertRow++;
                        objArray[0, 0] = dr["Company"];
                        objArray[0, 1] = "";
                        objArray[0, 2] = dr["TtlCPU"];
                        worksheet.Range[String.Format("A{0}:C{0}", insertRow)].Value2 = objArray;

                        //插入一筆Record
                        Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(insertRow + 1)), Type.Missing).EntireRow;
                        rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                    }

                }
                if (insertSubconOut == 0)
                {
                    //刪除資料
                    DeleteExcelRow(2, insertRow + 1, excel);
                    insertRow = insertRow + 3;
                    DeleteExcelRow(4, insertRow, excel);
                }
                else
                {
                    //刪除資料
                    DeleteExcelRow(2, insertRow + 1, excel);
                }
            }
            else
            {
                DeleteExcelRow(9, insertRow, excel);
            }

            this.HideWaitMessage();
            excel.Visible = true;
            return true;
        }

        private void DeleteExcelRow(int rowCount, int rowLocation,Microsoft.Office.Interop.Excel.Application excel)
        {
            for (int i = 1; i <= rowCount; i++)
            {
                Microsoft.Office.Interop.Excel.Range rng = (Microsoft.Office.Interop.Excel.Range)excel.Rows[rowLocation];
                //rng.Select();
                rng.Delete();
            }
        }
    }
}
