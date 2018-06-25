using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Win;
using Sci.Data;
using Excel = Microsoft.Office.Interop.Excel;
using sxrc = Sci.Utility.Excel.SaveXltReportCls;
using Sci.Utility.Excel;

namespace Sci.Production.PPIC
{
    public partial class R11 : Sci.Win.Tems.PrintForm
    {
        private string F;
        private string M;
        private string Gap;
        private string dateRangeReady1;
        private string dateRangeReady2;
        private string dateRangeReadyOrl;
        private DataTable dtFty;
        private DataTable[] dtList;
        private Color BackRed = Color.FromArgb(255, 155, 155);
        private Color FontRed = Color.FromArgb(255, 0, 0);
        private Color BackGreen = Color.FromArgb(167, 255, 190);
        private Color FontGreen = Color.FromArgb(0, 126, 15);
        private Color BackGray = Color.FromArgb(217, 217, 217);

        public R11(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.numDateGap.Text = "2";
        }

        protected override bool ValidateInput()
        {
            this.F = this.txtfactory.Text;
            this.M = this.txtMdivision.Text;
            this.Gap = this.numDateGap.Text;
            this.dateRangeReady1 = MyUtility.Check.Empty(this.dateRangeReadyDate.Value1) ? string.Empty : ((DateTime)this.dateRangeReadyDate.Value1).AddDays(-MyUtility.Convert.GetInt(this.Gap)).ToString("yyyy/MM/dd");
            this.dateRangeReady2 = MyUtility.Check.Empty(this.dateRangeReadyDate.Value2) ? string.Empty : ((DateTime)this.dateRangeReadyDate.Value2).AddDays(-MyUtility.Convert.GetInt(this.Gap)).ToString("yyyy/MM/dd");
            this.dateRangeReadyOrl = MyUtility.Check.Empty(this.dateRangeReadyDate.Value2) ? string.Empty : ((DateTime)this.dateRangeReadyDate.Value2).ToString("yyyy/MM/dd");
            return true;
        }

        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            List<string> listSQLFilter = new List<string>();
            #region Sql where Filter
            if (!this.dateRangeReady1.Empty())
            {
                listSQLFilter.Add($"and CONVERT(date, s.Offline) >= '{this.dateRangeReady1}'");
            }

            if (!this.dateRangeReady2.Empty())
            {
                listSQLFilter.Add($"and CONVERT(date, s.Offline) <= '{this.dateRangeReady2}'");
            }

            if (!this.F.Empty())
            {
                listSQLFilter.Add($"and s.FactoryID = '{this.F}'");
            }

            if (!this.M.Empty())
            {
                listSQLFilter.Add($"and s.MDivisionID = '{this.M}'");
            }
            #endregion

            #region Summery

          
            string sqlFty = $@"
select distinct FactoryID from sewingschedule s
where 1=1
{listSQLFilter.JoinToString($"{Environment.NewLine} ")}";
            StringBuilder sbFtycode = new StringBuilder();
            DBProxy.Current.Select(string.Empty, sqlFty, out dtFty);
            if (dtFty != null )
            {
                for (int i = 0; i < dtFty.Rows.Count; i++)
                {
                    sbFtycode.Append($@"[{dtFty.Rows[i]["FactoryID"].ToString().TrimEnd()}],");
                }
            }
        
            #endregion

            #region Sql Command
            string sqlCmd = $@"


SELECT distinct
[ReadyDate] = convert(date,s.Offline+{MyUtility.Convert.GetInt(Gap)})
,[M] = o.MDivisionID
,[Factory] = o.FactoryID
,[Delivery] = o.BuyerDelivery
,[SPNO] = s.OrderID
,[Category] = o.Category
,[Cancelled] = o.Junk
,[Dest] = (select alias from Country where  o.Dest=id)
,[Style] = o.StyleID
,[OrderType] = o.OrderTypeID
,[PoNo] = o.CustPONo
,[Brand] = o.BrandID
,[Qty] = o.Qty
,[SewingOutputQty] = SewingOutput.Qty
,[InLine] = lineDate.inline
,[OffLine] = lineDate.Offline
,[FirstSewnDate] = SewDate.FirstDate
,[LastSewnDate] = SewDate.LastDate
,[%]= iif(pdm.TotalCTN=0,0, (convert(float,pdm.ClogCTN) / convert(float,pdm.TotalCTN))*100)
,[TtlCTN] = pdm.TotalCTN,[FtyCTN] = pdm.FtyCtn,[ClogCTN] = pdm.ClogCTN
,[cLogRecDate] = pdm.ClogRcvDate
,[FinalInspDate]=cfa.FinalInsDate,cfa.Result
,[CfaName] = cfa.CfaName
,[SewingLine] = SewingLine.Line
,[ShipMode] = os.ShipmodeID
,[Remark]= IIF(iif(pdm.TotalCTN=0,0, (convert(float,pdm.ClogCTN) / convert(float,pdm.TotalCTN))*100)=100,'Pass Ready date','Failed Ready Date')
into #tmp
FROM sewingschedule S
left join Orders o on s.OrderID=o.ID
left join Order_QtyShip os on o.ID=os.Id
outer apply(
	select top 1 [CfaName] =pass1.ID+'-'+pass1.Name
	,case when cfa.Result='P' then 'Pass'
	when cfa.Result='F' then 'Fail'
	else '' end as Result
	,convert(date,cfa.EditDate) FinalInsDate
	from cfa
	left join Pass1 on cfa.CFA=pass1.ID
	where convert(date,cfa.EditDate) <= '{dateRangeReadyOrl}'
	and cfa.OrderID=s.OrderID
	order by cfa.EditDate desc
)cfa
outer apply(
	select sum(QAQty) Qty from SewingOutput_Detail so2
	where so2.OrderId=s.OrderID
)SewingOutput
outer apply(
	select MIN(OutputDate) FirstDate,MAX(OutputDate) LastDate
	from SewingOutput a
	left join SewingOutput_Detail b on a.id=b.id
	where b.OrderId =s.OrderID
)SewDate
outer apply(
	select inline = MIN(Inline), Offline = MAX(Offline)
	from SewingSchedule
	where OrderID=s.OrderID
)lineDate
outer apply( 
	select Sum( pd.CTNQty) PackingCTN ,
	Sum( case when p.Type in ('B', 'L') then pd.CTNQty else 0 end) TotalCTN,
	Sum( case when p.Type in ('B', 'L') and pd.TransferDate is null then pd.CTNQty else 0 end) FtyCtn,
	Sum(case when p.Type in ('B', 'L') and pd.ReceiveDate is not null then pd.CTNQty else 0 end) ClogCTN ,
	Sum(case when p.Type <> 'F' then pd.ShipQty else 0 end) PackingQty ,
	Sum(case when p.Type = 'F' then pd.ShipQty else 0 end) PackingFOCQty ,
	Sum(case when p.Type in ('B', 'L') and p.INVNo <> ''  then pd.ShipQty else 0 end) BookingQty ,
	Max (ReceiveDate) ClogRcvDate,
	MAX(p.PulloutDate)  ActPulloutDate
	from PackingList_Detail pd WITH (NOLOCK) 
    LEFT JOIN PackingList p on pd.ID = p.ID 
    where  pd.OrderID = os.ID 
)  pdm
outer apply(
	select Line = stuff((
		select concat(',',SewingLineID)
		from (
			select distinct SewingLineID
			from SewingSchedule ss
			where s.OrderID=ss.orderid	
			) s
		for xml path ('')
		),1,1,'')
)SewingLine
where 1=1
and pdm.TotalCTN>0
{listSQLFilter.JoinToString($"{Environment.NewLine} ")}
order by ReadyDate,m,Factory

select [M],[Factory],[Delivery],[SPNO],[Category],[Cancelled],[Dest],[Style],[OrderType],[PoNo]
,[Brand],[Qty],[SewingOutputQty],[InLine],[OffLine],[FirstSewnDate],[LastSewnDate],[%]
,[TtlCTN],[FtyCTN],[ClogCTN],[cLogRecDate],[FinalInspDate],Result,[CfaName],[SewingLine],[ShipMode],[Remark]
from #tmp
order by ReadyDate,m,Factory


select m,Fty,[Failed Ready Date],[Pass Ready date],[Grand Total],[Rating]= convert(varchar,Rating)+'%' from (
SELECT [M] = M
,[Fty] = Factory
,[Failed Ready Date] = sum(FtyCTN)
,[Pass Ready date] = sum(ClogCTN)
,[Grand Total] = sum(TtlCTN),
[Rating] = ROUND( convert(float,sum(ClogCTN))/convert(float,sum(TtlCTN)) *100,2)
,ReadyDate
FROM #tmp
where category='B' and Cancelled=0
group by M,Factory,ReadyDate

union all

select m,'Total' Fty
,[Failed Ready Date] = sum(FtyCTN)
,[Pass Ready date] = sum(ClogCTN)
,[Grand Total] = sum(TtlCTN) 
,[Rating] = ROUND( convert(float,sum(ClogCTN))/convert(float,sum(TtlCTN)) *100,2)
,ReadyDate
from #tmp
group by M,ReadyDate

union all

select 'x' m ,'Grand Total' Fty
,[Failed Ready Date] = sum(FtyCTN)
,[Pass Ready date] = sum(ClogCTN)
,[Grand Total] = sum(TtlCTN) 
,[Rating] = ROUND( convert(float,sum(ClogCTN))/convert(float,sum(TtlCTN)) *100,2)
,ReadyDate
from #tmp
group by ReadyDate
) a
order by ReadyDate,M,Fty

select * from (
	SELECT Factory, ReadyDate
	,[Rating] = round(convert(float,sum(ClogCTN))/convert(float,sum(TtlCTN)) *100,2)
	FROM #tmp
	where category='B' and Cancelled=0
	group by Factory,ReadyDate
	) as a
	pivot 
	(
		sum(rating) for factory in ({sbFtycode.ToString().Substring(0, sbFtycode.ToString().Length - 1)})
	) AS PT

drop table #tmp
";
            #endregion

            #region SQL get DataTable
            DualResult result;
            result = DBProxy.Current.Select(null, sqlCmd, out dtList);
            if (!result)
            {
                return result;
            }
            #endregion
            return new Ict.DualResult(true);
        }

        protected override bool OnToExcel(ReportDefinition report)
        {
            #region check resultDT
            if (this.dtList[0] == null || this.dtList[0].Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not Found.");
                return false;
            }
            #endregion
            this.SetCount(this.dtList[0].Rows.Count);
            string xltPath = System.IO.Path.Combine(Sci.Env.Cfg.XltPathDir + "\\PPIC_R11.xltx");
            sxrc sxr = new sxrc(xltPath, keepApp: true);
            sxr.boOpenFile = true;
            sxrc.xltRptTable xrtSummery1 = new sxrc.xltRptTable(dtList[1]);
            xrtSummery1.ShowHeader = false;
            sxr.dicDatas.Add("##Summery", xrtSummery1);

            sxrc.xltRptTable xrtDetail = new sxrc.xltRptTable(dtList[0]);
            xrtDetail.ShowHeader = false;
            sxr.dicDatas.Add("##detail", xrtDetail);

            Microsoft.Office.Interop.Excel.Workbook wkb = sxr.ExcelApp.ActiveWorkbook;
            Microsoft.Office.Interop.Excel.Worksheet wkcolor = wkb.Sheets[1];
            Microsoft.Office.Interop.Excel.Worksheet wkcolor2 = wkb.Sheets[2];

            #region Save Excel
            string excelFile = string.Empty;
            excelFile = Sci.Production.Class.MicrosoftFile.GetName("PPIC_R11");
            sxr.Save(excelFile);
            #endregion

            #region Summery by ReadyDate 變色及調整excel格式邊框
            // 變色
            int cntSummery = dtList[2].Rows.Count;

            DateTime date1 = (DateTime)dateRangeReadyDate.Value1;
            TimeSpan ts1 = new TimeSpan(((DateTime)dateRangeReadyDate.Value1).Ticks);
            TimeSpan ts2 = new TimeSpan(((DateTime)dateRangeReadyDate.Value2).Ticks);
            TimeSpan ts = ts1.Subtract(ts2).Duration();
            int range = ts.Days + 1;
            // Summery header
            wkcolor.Cells[1, 8].Value = "Date";
            wkcolor.Cells[1, 8].Interior.Color = BackGray;
            wkcolor.Cells[1, 8].Font.Bold = true;
            for (int d = 0; d < range; d++)
            {
                wkcolor.Cells[2 + d, 8].Value = date1.AddDays(d);
                wkcolor.Cells[2 + d, 8].Font.Bold = true;
            }

            for (int f = 0; f < dtFty.Rows.Count; f++)
            {
                wkcolor.Cells[1, 9 + f].Value = dtFty.Rows[f]["FactoryID"];
                wkcolor.Cells[1, 9 + f].Interior.Color = BackGray;
                wkcolor.Cells[1, 9 + f].Font.Bold = true;
                string fty = dtFty.Rows[f]["FactoryID"].ToString();

                int cnt = 0;
                for (int i = 0; i < range; i++)
                {
                    if (MyUtility.Convert.GetDate((DateTime)dtList[2].Rows[cnt]["ReadyDate"]) == MyUtility.Convert.GetDate(date1.AddDays(i)))
                    {
                        wkcolor.Cells[2 + i, 9 + f].Value = dtList[2].Rows[cnt][fty] + "%";
                        if (MyUtility.Convert.GetDecimal(dtList[2].Rows[cnt][fty]) > 90)
                        {
                            wkcolor.Cells[2 + i, 9 + f].Interior.Color = BackGreen;
                            wkcolor.Cells[2 + i, 9 + f].Font.Color = FontGreen;
                        }
                        else
                        {
                            wkcolor.Cells[2 + i, 9 + f].Interior.Color = BackRed;
                            wkcolor.Cells[2 + i, 9 + f].Font.Color = FontRed;
                        }
                        cnt++;
                    }
                    else
                    {
                        string rowformat1 = MyExcelPrg.GetExcelColumnName(8);
                        string rowformat2 = MyExcelPrg.GetExcelColumnName(8 + dtFty.Rows.Count);
                        string rowformat3 = MyExcelPrg.GetExcelColumnName(9);
                        wkcolor.Range[$"{rowformat1}{2 + i}", $"{rowformat2}{2 + i}"].Interior.Color = Color.Red;
                        wkcolor.Range[$"{rowformat3}{2 + i}", $"{rowformat2}{2 + i}"].Merge();
                    }
                }
            }

            //畫框線
            Microsoft.Office.Interop.Excel.Range rg1;
            rg1 = wkcolor.Range["H1", $"{MyExcelPrg.GetExcelColumnName(8 + dtFty.Rows.Count)}{range + 1}"];
            rg1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
            rg1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].Weight = 2;
            rg1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
            rg1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].Weight = 2;
            rg1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
            rg1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].Weight = 2;
            rg1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
            rg1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].Weight = 2;
            rg1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideHorizontal].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
            rg1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideHorizontal].Weight = 2;
            rg1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideVertical].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
            rg1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideVertical].Weight = 2;

            #endregion

            #region Summery by M,Fty

            for (int i = 0; i < dtList[1].Rows.Count; i++)
            {
                if (dtList[1].Rows[i]["Fty"].ToString() == "Total")
                {

                    wkcolor.Range[$"B{i + 2}", $"E{i + 2}"].Font.Bold = true;
                }

                if (dtList[1].Rows[i]["Fty"].ToString() == "Grand Total")
                {
                    wkcolor.Cells[i + 2, 1].Value = "";
                    wkcolor.Range[$"B{i + 2}", $"F{i + 2}"].Font.Bold = true;
                }
            }

            //畫框線
            Microsoft.Office.Interop.Excel.Range rg2;
            rg2 = wkcolor.Range["A1", $"F{dtList[1].Rows.Count + 1}"];
            rg2.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
            rg2.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].Weight = 2;
            rg2.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
            rg2.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].Weight = 2;
            rg2.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
            rg2.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].Weight = 2;
            rg2.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
            rg2.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].Weight = 2;
            rg2.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideHorizontal].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
            rg2.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideHorizontal].Weight = 2;
            rg2.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideVertical].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
            rg2.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideVertical].Weight = 2;
            #endregion

            // 最適欄寬
            wkcolor.Cells.EntireColumn.AutoFit();
            wkcolor2.Cells.EntireColumn.AutoFit();
            sxr.FinishSave();

            return true;
        }
    }
}
