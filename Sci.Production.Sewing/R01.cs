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
    public partial class R01 : Sci.Win.Tems.PrintForm
    {
        DateTime? date;
        string factory, team, factoryName;
        int excludeSubconIn;
        DataTable printData,ttlData, subprocessData;

        public R01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            DataTable factory;
            DBProxy.Current.Select(null, "select distinct FTYGroup from Factory WITH (NOLOCK) order by FTYGroup", out factory);
            MyUtility.Tool.SetupCombox(comboBox1, 1, factory);
            MyUtility.Tool.SetupCombox(comboBox2, 1, 1, ",A,B");
            MyUtility.Tool.SetupCombox(comboBox3, 1, 1, "Included,Excluded");
            dateBox1.Value = DateTime.Today.AddDays(-1);
            comboBox1.Text = Sci.Env.User.Factory;
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;
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
                MyUtility.Msg.WarningBox("Factory can't empty!!");
                return false;
            }

            date = dateBox1.Value;
            factory = comboBox1.Text;
            team = comboBox2.Text;
            excludeSubconIn = comboBox3.SelectedIndex;
            return base.ValidateInput();
        }

        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            #region 組撈Data SQL
            sqlCmd.Append(string.Format(@"with tmpSewingDetail
as (
select s.OutputDate,s.Category,s.Shift,s.SewingLineID,IIF(sd.QAQty=0,s.Manpower,s.Manpower*sd.QAQty) as ActManPower,
s.Team,sd.OrderId,sd.ComboType,sd.WorkHour,sd.QAQty,sd.InlineQty,isnull(o.Category,'') as OrderCategory,
o.LocalOrder,isnull(o.CdCodeID,'') as OrderCdCodeID,
isnull(mo.MockupID,'') as MockupCDCodeID,s.FactoryID,
isnull(o.CPU,0) as OrderCPU,isnull(o.CPUFactor,0) as OrderCPUFactor,isnull(mo.Cpu,0) as MockupCPU,
isnull(mo.CPUFactor,0) as MockupCPUFactor,isnull(o.StyleID,'') as OrderStyle,isnull(mo.StyleID,'') as MockupStyle,
isnull(o.SeasonID,'') as OrderSeason,isnull(mo.SeasonID,'') as MockupSeason,isnull(sl.Rate,100)/100 as Rate,
(select StdTMS from System WITH (NOLOCK) ) as StdTMS,isnull(r.InspectQty,0) as InspectQty,isnull(r.RejectQty,0) as RejectQty
from SewingOutput s WITH (NOLOCK) 
inner join SewingOutput_Detail sd WITH (NOLOCK) on sd.ID = s.ID
left join Orders o WITH (NOLOCK) on o.ID = sd.OrderId
left join MockupOrder mo WITH (NOLOCK) on mo.ID = sd.OrderId
left join Style_Location sl WITH (NOLOCK) on sl.StyleUkey = o.StyleUkey and sl.Location = sd.ComboType
left join Rft r WITH (NOLOCK) on r.OrderID = sd.OrderId and r.CDate = s.OutputDate and r.SewinglineID = s.SewingLineID and r.FactoryID = s.FactoryID and r.Shift = s.Shift and r.Team = s.Team
where s.OutputDate = '{0}'
and s.FactoryID = '{1}'", Convert.ToDateTime(date).ToString("d"), factory));

            if (!MyUtility.Check.Empty(team))
            {
                sqlCmd.Append(string.Format(" and s.Team = '{0}'", team));
            }

            sqlCmd.Append(@"),
tmpSewingGroup
as (
select OutputDate,Category,Shift,SewingLineID,Sum(ActManPower) as ActManPower,Team,OrderId,ComboType,
sum(WorkHour) as WorkHour,sum(QAQty) as QAQty,sum(InlineQty) as InlineQty,OrderCategory,
LocalOrder,OrderCdCodeID,MockupCDCodeID,FactoryID,
OrderCPU,OrderCPUFactor,MockupCPU,MockupCPUFactor,OrderStyle,
MockupStyle,OrderSeason,MockupSeason,Rate,StdTMS,InspectQty,RejectQty
from tmpSewingDetail
group by OutputDate,Category,Shift,SewingLineID,Team,OrderId,ComboType,OrderCategory,LocalOrder,
OrderCdCodeID,MockupCDCodeID,FactoryID,
OrderCPU,OrderCPUFactor,MockupCPU,MockupCPUFactor,OrderStyle,MockupStyle,OrderSeason,MockupSeason,Rate,StdTMS,InspectQty,RejectQty
),
tmp1stFilter
as (
select t.*,IIF(t.Shift <> 'O' and t.Category <> 'M' and t.LocalOrder = 1, 'I',t.Shift) as LastShift,
f.Type as FtyType,f.CountryID as FtyCountry,[dbo].getSewingOutputCumulateOfDays(IIF(t.Category <> 'M',OrderStyle,MockupStyle),SewingLineID,OutputDate,FactoryID) as CumulateDate
from tmpSewingGroup t
left join Factory f WITH (NOLOCK) on t.FactoryID = f.ID
)
select IIF(LastShift='D','Day',IIF(LastShift='N','Night',IIF(LastShift='O','Subcon-Out','Subcon-In'))) as Shift,
Team,SewingLineID,OrderId,IIF(Category='M',MockupStyle,OrderStyle) as Style,
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
InlineQty,QAQty-InlineQty as Diff,LastShift,ComboType
from tmp1stFilter
where 1 =1");
            if (excludeSubconIn == 1)
            {
                sqlCmd.Append(" and LastShift <> 'I'");
            }

            sqlCmd.Append(@" order by LastShift,Team,SewingLineID,OrderId");
            #endregion
            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            #region 整理Total資料
            if (printData.Rows.Count > 0)
            {
                try
                {
                    MyUtility.Tool.ProcessWithDatatable(printData, "Shift,Team,SewingLineID,ActManPower,TMS,QAQty,RFT,LastShift",
                        @"
;with SubMaxActManpower
as (
select Shift,Team,SewingLineID,max(ActManPower) as ActManPower
from #tmp
group by Shift,Team,SewingLineID
),
SubSummaryData
as (
select Shift,Team,sum(TMS*QAQty) as TMS,sum(QAQty) as QAQty,AVG(RFT) as RFT
from #tmp
group by Shift,Team
),
SubTotal
as (
select s.Shift,s.Team,(s.TMS/s.QAQty) as TMS,
s.RFT,sum(m.ActManPower) as ActManPower
from SubSummaryData s 
left join SubMaxActManpower m on s.Shift = m.Shift and s.Team = m.Team
group by s.Shift,s.Team,s.RFT,s.TMS,s.QAQty
),
GrandIncludeInOutMaxActManpower
as (
select Shift,Team,SewingLineID,max(ActManPower) as ActManPower
from #tmp
group by Shift,Team,SewingLineID
),
GrandIncludeInOutSummaryData
as (
select sum(TMS*QAQty) as TMS,sum(QAQty) as QAQty,AVG(RFT) as RFT
from #tmp
),
GenTotal1
as (
select (s.TMS/s.QAQty) as TMS,s.RFT,sum(m.ActManPower) as ActManPower
from GrandIncludeInOutSummaryData s
left join GrandIncludeInOutMaxActManpower m on 1 = 1
group by s.TMS,s.QAQty,s.RFT
),
GrandExcludeOutMaxActManpower
as (
select Shift,Team,SewingLineID,max(ActManPower) as ActManPower
from #tmp
where LastShift <> 'O'
group by Shift,Team,SewingLineID
),
GrandExcludeOutSummaryData
as (
select sum(TMS*QAQty) as TMS,sum(QAQty) as QAQty,AVG(RFT) as RFT
from #tmp
where LastShift <> 'O'
),
GenTotal2
as (
select (s.TMS/s.QAQty) as TMS,s.RFT,sum(m.ActManPower) as ActManPower
from GrandExcludeOutSummaryData s
left join GrandExcludeOutMaxActManpower m on 1 = 1
group by s.TMS,s.QAQty,s.RFT
),
GrandExcludeInOutMaxActManpower
as (
select Shift,Team,SewingLineID,max(ActManPower) as ActManPower
from #tmp
where LastShift <> 'O' 
and LastShift <> 'I' 
group by Shift,Team,SewingLineID
),
GrandExcludeInOutSummaryData
as (
select sum(TMS*QAQty) as TMS,sum(QAQty) as QAQty,AVG(RFT) as RFT
from #tmp
where LastShift <> 'O'
and LastShift <> 'I' 
),
GenTotal3
as (
select (s.TMS/s.QAQty) as TMS,s.RFT,sum(m.ActManPower) as ActManPower
from GrandExcludeInOutSummaryData s
left join GrandExcludeInOutMaxActManpower m on 1 = 1
group by s.TMS,s.QAQty,s.RFT
)
select 'Sub' as Type, '1' as Sort, * from SubTotal
union all
select 'Grand' as Type,'2' as Sort,'' as Shift,'' as Team,TMS,RFT,ActManPower from GenTotal1
union all
select 'Grand' as Type,'3' as Sort,'' as Shift,'' as Team,TMS,RFT,ActManPower from GenTotal2
union all
select 'Grand' as Type,'4' as Sort,'' as Shift,'' as Team,TMS,RFT,ActManPower from GenTotal3",
                        out ttlData);
                }
                catch (Exception ex)
                {
                    DualResult failResult = new DualResult(false, "Query total data fail\r\n" + ex.ToString());
                    return failResult;
                }
            }
            #endregion

            #region 整理Subprocess資料
            if (printData.Rows.Count > 0)
            {
                try
                {
                    MyUtility.Tool.ProcessWithDatatable(printData, "OrderId,ComboType,QAQty,LastShift",
                        @"
;with tmpArtwork
as (
Select ID from ArtworkType WITH (NOLOCK) where Classify in ('I','A','P') and IsTtlTMS = 0
),
tmpAllSubprocess
as (
select ot.ArtworkTypeID,a.OrderId,a.ComboType,Round(sum(a.QAQty)*ot.Price*(isnull(sl.Rate,100)/100),2) as Price
from #tmp a
inner join Order_TmsCost ot WITH (NOLOCK) on ot.ID = a.OrderId
inner join Orders o WITH (NOLOCK) on o.ID = a.OrderId
inner join tmpArtwork ta on ta.ID = ot.ArtworkTypeID
left join Style_Location sl WITH (NOLOCK) on sl.StyleUkey = o.StyleUkey and sl.Location = a.ComboType
where ((a.LastShift = 'O' and o.LocalOrder <> 1) or (a.LastShift <> 'O')) 
and ot.Price > 0
group by ot.ArtworkTypeID,a.OrderId,a.ComboType,ot.Price,sl.Rate
)
select ArtworkTypeID,sum(Price) as Price
from tmpAllSubprocess
group by ArtworkTypeID
order by ArtworkTypeID
",
                        out subprocessData);
                }
                catch (Exception ex)
                {
                    DualResult failResult = new DualResult(false, "Query sub process data fail\r\n" + ex.ToString());
                    return failResult;
                }
            }
            #endregion

            factoryName = MyUtility.GetValue.Lookup(string.Format("select NameEN from Factory WITH (NOLOCK) where ID = '{0}'", factory));
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
            string strXltName = Sci.Env.Cfg.XltPathDir + "\\Sewing_R01_DailyCMPReport.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null) return false;
            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            worksheet.Cells[1, 1] = factoryName;
            worksheet.Cells[2, 1] = string.Format("{0} Daily CMP Report, DD.{1} {2}", factory, Convert.ToDateTime(date).ToString("MM/dd"), excludeSubconIn == 1 ? "" : "(Included Subcon-IN)");

            object[,] objArray = new object[1, 19];
            string[] subTtlRowInOut = new string[8];
            string[] subTtlRowExOut = new string[8];
            string[] subTtlRowExInOut = new string[8];

            string shift = MyUtility.Convert.GetString(printData.Rows[0]["Shift"]);
            string team = MyUtility.Convert.GetString(printData.Rows[0]["Team"]);
            int insertRow = 5, startRow = 5, ttlShift = 1, subRows= 0;
            worksheet.Cells[3, 1] = string.Format("{0} SHIFT: {1} Team", shift, team);
            DataRow[] selectRow;
            foreach (DataRow dr in printData.Rows)
            {
                if (shift != MyUtility.Convert.GetString(dr["Shift"]) || team != MyUtility.Convert.GetString(dr["Team"]))
                {
                    //將多出來的Record刪除
                    for (int i = 1; i <= 2; i++)
                    {
                        Microsoft.Office.Interop.Excel.Range rng = (Microsoft.Office.Interop.Excel.Range)excel.Rows[insertRow, Type.Missing];
                        rng.Select();
                        rng.Delete(Microsoft.Office.Interop.Excel.XlDirection.xlUp);
                    }
                    //填入Sub Total資料
                    if (ttlData != null)
                    {
                        selectRow = ttlData.Select(string.Format("Type = 'Sub' and Shift = '{0}' and  Team = '{1}'", shift, team));
                        if (selectRow.Length > 0)
                        {
                            worksheet.Cells[insertRow, 5] = MyUtility.Convert.GetDecimal(selectRow[0]["ActManPower"]);
                            worksheet.Cells[insertRow, 9] = MyUtility.Convert.GetDecimal(selectRow[0]["TMS"]);
                            worksheet.Cells[insertRow, 16] = MyUtility.Convert.GetDecimal(selectRow[0]["RFT"]);
                        }
                    }
                    
                    worksheet.Cells[insertRow, 7] = string.Format("=SUM(G{0}:G{1})", MyUtility.Convert.GetString(startRow), MyUtility.Convert.GetString(insertRow - 1));
                    worksheet.Cells[insertRow, 8] = string.Format("=SUM(H{0}:H{1})", MyUtility.Convert.GetString(startRow), MyUtility.Convert.GetString(insertRow - 1));
                    worksheet.Cells[insertRow, 11] = string.Format("=SUM(K{0}:K{1})", MyUtility.Convert.GetString(startRow), MyUtility.Convert.GetString(insertRow - 1));
                    worksheet.Cells[insertRow, 12] = string.Format("=SUM(L{0}:L{1})", MyUtility.Convert.GetString(startRow), MyUtility.Convert.GetString(insertRow - 1));
                    worksheet.Cells[insertRow, 13] = string.Format("=SUM(M{0}:M{1})", MyUtility.Convert.GetString(startRow), MyUtility.Convert.GetString(insertRow - 1));
                    worksheet.Cells[insertRow, 14] = string.Format("=M{0}/G{0}",MyUtility.Convert.GetString(insertRow));
                    worksheet.Cells[insertRow, 15] = string.Format("=ROUND((M{0}/(G{0}*3600/1400))*100,1)", MyUtility.Convert.GetString(insertRow));
                    worksheet.Cells[insertRow, 18] = string.Format("=SUM(R{0}:R{1})", MyUtility.Convert.GetString(startRow), MyUtility.Convert.GetString(insertRow - 1));
                    worksheet.Cells[insertRow, 19] = string.Format("=SUM(S{0}:S{1})", MyUtility.Convert.GetString(startRow), MyUtility.Convert.GetString(insertRow - 1));

                    subTtlRowInOut[subRows] = MyUtility.Convert.GetString(insertRow);
                    if (shift != "Subcon-Out")
                    {
                        subTtlRowExOut[subRows] = MyUtility.Convert.GetString(insertRow);
                    }
                    if (shift != "Subcon-Out" && shift != "Subcon-In")
                    {
                        subTtlRowExInOut[subRows] = MyUtility.Convert.GetString(insertRow);
                    }

                    //重置參數資料
                    shift = MyUtility.Convert.GetString(dr["Shift"]);
                    team = MyUtility.Convert.GetString(dr["Team"]);
                    worksheet.Cells[insertRow+2, 1] = string.Format("{0} SHIFT: {1} Team", shift, team);
                    insertRow = insertRow + 4;
                    startRow = insertRow;
                    ttlShift++;
                    subRows++;
                }

                objArray[0, 0] = dr["SewingLineID"];
                objArray[0, 1] = dr["OrderId"];
                objArray[0, 2] = dr["Style"];
                objArray[0, 3] = dr["CDNo"];
                objArray[0, 4] = dr["ActManPower"];
                objArray[0, 5] = dr["WorkHour"];
                objArray[0, 6] = dr["ManHour"];
                objArray[0, 7] = dr["TargetCPU"];
                objArray[0, 8] = dr["TMS"];
                objArray[0, 9] = dr["CPUPrice"];
                objArray[0, 10] = dr["TargetQty"];
                objArray[0, 11] = dr["QAQty"];
                objArray[0, 12] = dr["TotalCPU"];
                objArray[0, 13] = dr["CPUSewer"];
                objArray[0, 14] = dr["EFF"];
                objArray[0, 15] = dr["RFT"];
                objArray[0, 16] = dr["CumulateDate"];
                objArray[0, 17] = dr["InlineQty"];
                objArray[0, 18] = dr["Diff"];
                worksheet.Range[String.Format("A{0}:S{0}", insertRow)].Value2 = objArray;
                insertRow++;

                //插入一筆Record
                Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(insertRow)), Type.Missing).EntireRow;
                rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
            }

            //最後一個Shift資料
            //將多出來的Record刪除
            for (int i = 1; i <= 2; i++)
            {
                Microsoft.Office.Interop.Excel.Range rng = (Microsoft.Office.Interop.Excel.Range)excel.Rows[insertRow, Type.Missing];
                rng.Select();
                rng.Delete(Microsoft.Office.Interop.Excel.XlDirection.xlUp);
            }
            //填入Sub Total資料
            if (ttlData != null)
            {
                selectRow = ttlData.Select(string.Format("Type = 'Sub' and Shift = '{0}' and  Team = '{1}'", shift, team));
                if (selectRow.Length > 0)
                {
                    worksheet.Cells[insertRow, 5] = MyUtility.Convert.GetDecimal(selectRow[0]["ActManPower"]);
                    worksheet.Cells[insertRow, 9] = MyUtility.Convert.GetDecimal(selectRow[0]["TMS"]);
                    worksheet.Cells[insertRow, 16] = MyUtility.Convert.GetDecimal(selectRow[0]["RFT"]);
                }
            }
            worksheet.Cells[insertRow, 7] = string.Format("=SUM(G{0}:G{1})", MyUtility.Convert.GetString(startRow), MyUtility.Convert.GetString(insertRow - 1));
            worksheet.Cells[insertRow, 8] = string.Format("=SUM(H{0}:H{1})", MyUtility.Convert.GetString(startRow), MyUtility.Convert.GetString(insertRow - 1));
            worksheet.Cells[insertRow, 11] = string.Format("=SUM(K{0}:K{1})", MyUtility.Convert.GetString(startRow), MyUtility.Convert.GetString(insertRow - 1));
            worksheet.Cells[insertRow, 12] = string.Format("=SUM(L{0}:L{1})", MyUtility.Convert.GetString(startRow), MyUtility.Convert.GetString(insertRow - 1));
            worksheet.Cells[insertRow, 13] = string.Format("=SUM(M{0}:M{1})", MyUtility.Convert.GetString(startRow), MyUtility.Convert.GetString(insertRow - 1));
            worksheet.Cells[insertRow, 14] = string.Format("=M{0}/G{0}", MyUtility.Convert.GetString(insertRow));
            worksheet.Cells[insertRow, 15] = string.Format("=ROUND((M{0}/(G{0}*3600/1400))*100,1)", MyUtility.Convert.GetString(insertRow));
            worksheet.Cells[insertRow, 18] = string.Format("=SUM(R{0}:R{1})", MyUtility.Convert.GetString(startRow), MyUtility.Convert.GetString(insertRow - 1));
            worksheet.Cells[insertRow, 19] = string.Format("=SUM(S{0}:S{1})", MyUtility.Convert.GetString(startRow), MyUtility.Convert.GetString(insertRow - 1));
            subTtlRowInOut[subRows] = MyUtility.Convert.GetString(insertRow);
            if (shift != "Subcon-Out")
            {
                subTtlRowExOut[subRows] = MyUtility.Convert.GetString(insertRow);
            }
            if (shift != "Subcon-Out" && shift != "Subcon-In")
            {
                subTtlRowExInOut[subRows] = MyUtility.Convert.GetString(insertRow);
            }

            //刪除多出來的Shift Record
            for (int i = 1; i <= (8 - ttlShift) * 6; i++)
            {
                Microsoft.Office.Interop.Excel.Range rng = (Microsoft.Office.Interop.Excel.Range)excel.Rows[insertRow+1, Type.Missing];
                rng.Select();
                rng.Delete(Microsoft.Office.Interop.Excel.XlDirection.xlUp);
            }

            insertRow = insertRow + 2;
            //填Grand Total資料
            string ttlManhour, targetCPU, targetQty, qaQty, ttlCPU, prodOutput, diff;
            if (ttlData != null)
            {
                selectRow = ttlData.Select("Type = 'Grand'");
                if (selectRow.Length > 0)
                {
                    for (int i = 0; i < selectRow.Length; i++)
                    {
                        worksheet.Cells[insertRow, 5] = MyUtility.Convert.GetDecimal(selectRow[i]["ActManPower"]);
                        worksheet.Cells[insertRow, 9] = MyUtility.Convert.GetDecimal(selectRow[i]["TMS"]);
                        worksheet.Cells[insertRow, 16] = MyUtility.Convert.GetDecimal(selectRow[i]["RFT"]);
                        ttlManhour = "=";
                        targetCPU = "=";
                        targetQty = "=";
                        qaQty = "=";
                        ttlCPU = "=";
                        prodOutput = "=";
                        diff = "=";
                        #region 組公式
                        if (MyUtility.Convert.GetString(selectRow[i]["Sort"]) == "2")
                        {
                            for (int j = 0; j < 8; j++)
                            {
                                if (!MyUtility.Check.Empty(subTtlRowInOut[j]))
                                {
                                    ttlManhour = ttlManhour + string.Format("G{0}+", subTtlRowInOut[j]);
                                    targetCPU = targetCPU + string.Format("H{0}+", subTtlRowInOut[j]);
                                    targetQty = targetQty + string.Format("K{0}+", subTtlRowInOut[j]);
                                    qaQty = qaQty + string.Format("L{0}+", subTtlRowInOut[j]);
                                    ttlCPU = ttlCPU + string.Format("M{0}+", subTtlRowInOut[j]);
                                    prodOutput = prodOutput + string.Format("R{0}+", subTtlRowInOut[j]);
                                    diff = diff + string.Format("S{0}+", subTtlRowInOut[j]);
                                }
                            }
                        }
                        else if (MyUtility.Convert.GetString(selectRow[i]["Sort"]) == "3")
                        {
                            for (int j = 0; j < 8; j++)
                            {
                                if (!MyUtility.Check.Empty(subTtlRowExOut[j]))
                                {
                                    ttlManhour = ttlManhour + string.Format("G{0}+", subTtlRowExOut[j]);
                                    targetCPU = targetCPU + string.Format("H{0}+", subTtlRowExOut[j]);
                                    targetQty = targetQty + string.Format("K{0}+", subTtlRowExOut[j]);
                                    qaQty = qaQty + string.Format("L{0}+", subTtlRowExOut[j]);
                                    ttlCPU = ttlCPU + string.Format("M{0}+", subTtlRowExOut[j]);
                                    prodOutput = prodOutput + string.Format("R{0}+", subTtlRowExOut[j]);
                                    diff = diff + string.Format("S{0}+", subTtlRowExOut[j]);
                                }
                            }
                        }
                        else
                        {
                            for (int j = 0; j < 8; j++)
                            {
                                if (!MyUtility.Check.Empty(subTtlRowExInOut[j]))
                                {
                                    ttlManhour = ttlManhour + string.Format("G{0}+", subTtlRowExInOut[j]);
                                    targetCPU = targetCPU + string.Format("H{0}+", subTtlRowExInOut[j]);
                                    targetQty = targetQty + string.Format("K{0}+", subTtlRowExInOut[j]);
                                    qaQty = qaQty + string.Format("L{0}+", subTtlRowExInOut[j]);
                                    ttlCPU = ttlCPU + string.Format("M{0}+", subTtlRowExInOut[j]);
                                    prodOutput = prodOutput + string.Format("R{0}+", subTtlRowExInOut[j]);
                                    diff = diff + string.Format("S{0}+", subTtlRowExInOut[j]);
                                }
                            }
                        }
                        #endregion

                        worksheet.Cells[insertRow, 7] = ttlManhour.Substring(0, ttlManhour.Length - 1);
                        worksheet.Cells[insertRow, 8] = targetCPU.Substring(0, targetCPU.Length - 1);
                        worksheet.Cells[insertRow, 11] = targetQty.Substring(0, targetQty.Length - 1);
                        worksheet.Cells[insertRow, 12] = qaQty.Substring(0, qaQty.Length - 1);
                        worksheet.Cells[insertRow, 13] = ttlCPU.Substring(0, ttlCPU.Length - 1);
                        worksheet.Cells[insertRow, 14] = string.Format("=M{0}/G{0}", MyUtility.Convert.GetString(insertRow));
                        worksheet.Cells[insertRow, 15] = string.Format("=ROUND((M{0}/(G{0}*3600/1400))*100,1)", MyUtility.Convert.GetString(insertRow));
                        worksheet.Cells[insertRow, 18] = prodOutput.Substring(0, prodOutput.Length - 1);
                        worksheet.Cells[insertRow, 19] = diff.Substring(0, diff.Length - 1);
                        insertRow++;
                    }
                }
            }

            insertRow = insertRow + 2;
            foreach (DataRow dr in subprocessData.Rows)
            {
                worksheet.Cells[insertRow, 3] = string.Format("{0}CMP US$", MyUtility.Convert.GetString(dr["ArtworkTypeID"]).PadRight(20,' '));
                worksheet.Cells[insertRow, 6] = MyUtility.Convert.GetString(dr["Price"]);
                insertRow++;
                //插入一筆Record
                Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(insertRow)), Type.Missing).EntireRow;
                rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
            }

            this.HideWaitMessage();
            excel.Visible = true;
            return true;
        }
    }
}
