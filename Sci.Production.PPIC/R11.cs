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
        private List<string> listSQLFilter;
        private StringBuilder sbFtycode;
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

            this.listSQLFilter = new List<string>();
            #region Sql where Filter
            if (!this.dateRangeReady1.Empty())
            {
                this.listSQLFilter.Add($"and CONVERT(date, s.Offline) >= '{this.dateRangeReady1}'");
            }

            if (!this.dateRangeReady2.Empty())
            {
                this.listSQLFilter.Add($"and CONVERT(date, s.Offline) <= '{this.dateRangeReady2}'");
            }

            if (!this.F.Empty())
            {
                this.listSQLFilter.Add($"and s.FactoryID = '{this.F}'");
            }

            if (!this.M.Empty())
            {
                this.listSQLFilter.Add($"and s.MDivisionID = '{this.M}'");
            }
            #endregion

            #region Summery

            sbFtycode = new StringBuilder();
            string sqlFty = $@"
select distinct FactoryID from sewingschedule s
where 1=1
{this.listSQLFilter.JoinToString($"{Environment.NewLine} ")}";
            DBProxy.Current.Select(string.Empty, sqlFty, out this.dtFty);
            if (this.dtFty != null)
            {
                for (int i = 0; i < this.dtFty.Rows.Count; i++)
                {
                    sbFtycode.Append($@"[{this.dtFty.Rows[i]["FactoryID"].ToString().TrimEnd()}],");
                }
            }

            if (sbFtycode.Length == 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            #endregion
            return true;
        }

        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            #region Sql Command
            string sqlCmd = $@"
SELECT distinct
[ReadyDate] = convert(date,s.Offline+2)
,[M] = o.MDivisionID
,[Factory] = o.FactoryID
,[Delivery] = o.BuyerDelivery
,[SPNO] = s.OrderID
,[Category] = 
case	when o.Category ='B' then 'Bulk'
		when o.Category ='S' then 'Sample'
		when o.Category ='M' then 'Material'
		when o.Category ='O' then 'Other'
		when o.Category ='G' then 'Garment'
		when o.Category ='T' then 'SMLT'
end
,[Cancelled] = o.Junk
,[Dest] = (select alias from Country where  o.Dest=id)
,[Style] = o.StyleID
,[OrderType] = o.OrderTypeID
,[PoNo] = o.CustPONo
,[Brand] = o.BrandID
,[Qty] = os.Qty
,[SewingOutputQty] = isnull(SewingOutput.Qty,0)
,[InLine] = ss.inline
,[OffLine] = convert(date, s.Offline)
,[FirstSewnDate] = SewDate.FirstDate
,[LastSewnDate] = SewDate.LastDate
,[%]= iif(pdm.TotalCTN=0,0, ( isnull(convert(float,Receive.ClogCTN),0) / convert(float,pdm.TotalCTN))*100)
,[TtlCTN] = pdm.TotalCTN,[FtyCTN] = pdm.FtyCtn,[ClogCTN] = isnull(Receive.ClogCTN,0)
,[cLogRecDate] = Receive.ClogRcvDate
,[FinalInspDate]=cfa.FinalInsDate,cfa.Result
,[CfaName] = cfa.CfaName
,[SewingLine] = SewingLine.Line
,[ShipMode] = os.ShipmodeID
,[Remark]= IIF(iif(pdm.TotalCTN=0,0, (isnull(convert(float,Receive.ClogCTN),0) / convert(float,pdm.TotalCTN))*100)=100,'Pass Ready date','Failed Ready Date')
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
	where convert(date,cfa.EditDate) <= CONVERT(date, DATEADD(DAY,{MyUtility.Convert.GetInt(this.Gap)}, s.Offline))
	and cfa.OrderID=s.OrderID
	order by cfa.EditDate desc
)cfa
outer apply(
	select sum(so2.QAQty) Qty 
	from SewingOutput_Detail so2
	left join SewingOutput so1 on so2.ID=so1.ID
	where so2.OrderId=s.OrderID
	and so1.OutputDate <= CONVERT(date, s.Offline)
)SewingOutput
outer apply(
	select MIN(OutputDate) FirstDate,MAX(OutputDate) LastDate
	from SewingOutput a
	left join SewingOutput_Detail b on a.id=b.id
	where b.OrderId =s.OrderID
)SewDate
outer apply(
	select convert(date, min(inline)) inline
	from SewingSchedule 
	where OrderID=s.OrderID 
)ss
outer apply( 
	select Sum( case when p.Type in ('B', 'L') then pd.CTNQty else 0 end) TotalCTN,
	Sum( case when p.Type in ('B', 'L') and pd.TransferDate is null then pd.CTNQty else 0 end) FtyCtn
	from PackingList_Detail pd WITH (NOLOCK) 
    LEFT JOIN PackingList p on pd.ID = p.ID 
    where  pd.OrderID = os.ID 
	and pd.OrderShipmodeSeq = os.Seq
)  pdm
outer apply (
	select Sum(case when p.Type in ('B', 'L') and pd.ReceiveDate is not null then pd.CTNQty else 0 end) ClogCTN ,
	Max (ReceiveDate) ClogRcvDate
	from PackingList_Detail pd WITH (NOLOCK) 
    LEFT JOIN PackingList p on pd.ID = p.ID 
    where  pd.OrderID = os.ID 
	and pd.OrderShipmodeSeq = os.Seq
    and (ReceiveDate) <= CONVERT(date, DATEADD(DAY,{MyUtility.Convert.GetInt(this.Gap)}, s.Offline))	
) Receive
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
{this.listSQLFilter.JoinToString($"{Environment.NewLine} ")}


select [M],[Factory],[Delivery],[SPNO],[Category],[Cancelled],[Dest],[Style],[OrderType],[PoNo]
,[Brand],[Qty],[SewingOutputQty],[InLine],[OffLine],[FirstSewnDate],[LastSewnDate],[%]
,[TtlCTN],[FtyCTN],[ClogCTN],[cLogRecDate],[FinalInspDate],Result,[CfaName],[SewingLine],[ShipMode],[Remark]
from #tmp
order by ReadyDate,m,Factory

select ReadyDate,m,Fty,[Failed Ready Date],[Pass Ready date],[Grand Total],[Rating]= convert(varchar,Rating)+'%' 
from (
SELECT [M] = M
,[Fty] = Factory
,[Failed Ready Date] =fail.cnt
,[Pass Ready date] = pass.cnt
,[Grand Total] =count(*),
[Rating] = iif(count(*)=0,0, ROUND((convert(float, pass.cnt)/convert(float,count(*))*100) ,2))
,ReadyDate
FROM #tmp t
outer apply(
	select count(*) cnt
	FROM #tmp
	where Remark='Failed Ready Date' and Factory=t.Factory and m=t.M and t.ReadyDate=ReadyDate
	and category='Bulk' and Cancelled=0
)fail
outer apply(
	select count(*) cnt
	FROM #tmp
	where Remark='Pass Ready Date' and Factory=t.Factory and m=t.M and t.ReadyDate=ReadyDate
	and category='Bulk' and Cancelled=0
)pass
where category='Bulk' and Cancelled=0
group by ReadyDate,M,Factory ,fail.cnt,pass.cnt

union all

SELECT [M] = M
,[Fty] = 'Total'
,[Failed Ready Date] =fail.cnt
,[Pass Ready date] = pass.cnt
,[Grand Total] =count(*),
[Rating] = iif(count(*)=0,0, ROUND((convert(float, pass.cnt)/convert(float,count(*))*100) ,2))
,ReadyDate
FROM #tmp t
outer apply(
	select count(*) cnt
	FROM #tmp
	where Remark='Failed Ready Date' and m=t.M and t.ReadyDate=ReadyDate
	and category='Bulk' and Cancelled=0
)fail
outer apply(
	select count(*) cnt
	FROM #tmp
	where Remark='Pass Ready Date' and m=t.M and t.ReadyDate=ReadyDate
	and category='Bulk' and Cancelled=0
)pass
where category='Bulk' and Cancelled=0
group by ReadyDate,M ,fail.cnt,pass.cnt

union all

SELECT [M] = 'x'
,[Fty] = 'Grand Total'
,[Failed Ready Date] =fail.cnt
,[Pass Ready date] = pass.cnt
,[Grand Total] =count(*),
[Rating] = iif(count(*)=0,0, ROUND((convert(float, pass.cnt)/convert(float,count(*))*100) ,2))
,ReadyDate
FROM #tmp t
outer apply(
	select count(*) cnt
	FROM #tmp
	where Remark='Failed Ready Date' and t.ReadyDate=ReadyDate
	and category='Bulk' and Cancelled=0
)fail
outer apply(
	select count(*) cnt
	FROM #tmp
	where Remark='Pass Ready Date' and t.ReadyDate=ReadyDate
	and category='Bulk' and Cancelled=0
)pass
where category='Bulk' and Cancelled=0
group by ReadyDate,fail.cnt,pass.cnt
) a
order by ReadyDate,M,Fty

select * from 
(
	SELECT Factory, ReadyDate
	,[Rating] = iif(count(*)=0,0, ROUND((convert(float, pass.cnt)/convert(float,count(*))*100) ,2))
	FROM #tmp t
	outer apply(
		select count(*) cnt
		FROM #tmp
		where Remark='Failed Ready Date' and t.ReadyDate=ReadyDate
		and category='Bulk' and Cancelled=0 and Factory = t.Factory
	)fail
	outer apply(
		select count(*) cnt
		FROM #tmp
		where Remark='Pass Ready Date' and t.ReadyDate=ReadyDate
		and category='Bulk' and Cancelled=0 and Factory = t.Factory
	)pass
	where category='Bulk' and Cancelled=0
	group by Factory,ReadyDate,fail.cnt,pass.cnt
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
            result = DBProxy.Current.Select(null, sqlCmd, out this.dtList);
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
            if (MyUtility.Check.Empty(this.dtList[0]) || this.dtList[0].Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not Found.");
                return false;
            }
            #endregion
            this.SetCount(this.dtList[0].Rows.Count);
            string xltPath = System.IO.Path.Combine(Sci.Env.Cfg.XltPathDir + "\\PPIC_R11.xltx");
            sxrc sxr = new sxrc(xltPath, keepApp: true);
            sxr.boOpenFile = true;
            sxrc.xltRptTable xrtSummery1 = new sxrc.xltRptTable(this.dtList[1]);
            xrtSummery1.ShowHeader = false;
            sxr.dicDatas.Add("##Summery", xrtSummery1);

            sxrc.xltRptTable xrtDetail = new sxrc.xltRptTable(this.dtList[0]);
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
            int cntSummery = this.dtList[2].Rows.Count;

            DateTime date1 = (DateTime)this.dateRangeReadyDate.Value1;
            TimeSpan ts1 = new TimeSpan(((DateTime)this.dateRangeReadyDate.Value1).Ticks);
            TimeSpan ts2 = new TimeSpan(((DateTime)this.dateRangeReadyDate.Value2).Ticks);
            TimeSpan ts = ts1.Subtract(ts2).Duration();
            int range = ts.Days + 1;
            // Summery header
            wkcolor.Cells[1, 9].Value = "Date";
            wkcolor.Cells[1, 9].Interior.Color = this.BackGray;
            wkcolor.Cells[1, 9].Font.Bold = true;
            for (int d = 0; d < range; d++)
            {
                wkcolor.Cells[2 + d, 9].Value = date1.AddDays(d);
                wkcolor.Cells[2 + d, 9].Font.Bold = true;
            }

            for (int f = 0; f < this.dtFty.Rows.Count; f++)
            {
                wkcolor.Cells[1, 10 + f].Value = this.dtFty.Rows[f]["FactoryID"];
                wkcolor.Cells[1, 10 + f].Interior.Color = this.BackGray;
                wkcolor.Cells[1, 10 + f].Font.Bold = true;
                string fty = this.dtFty.Rows[f]["FactoryID"].ToString();

                int cnt = 0;
                for (int i = 0; i < range; i++)
                {
                    if ((this.dtList[2].Rows.Count > cnt) && (MyUtility.Convert.GetDate((DateTime)this.dtList[2].Rows[cnt]["ReadyDate"]) == MyUtility.Convert.GetDate(date1.AddDays(i))))
                    {
                        wkcolor.Cells[2 + i, 10 + f].Value = MyUtility.Check.Empty(this.dtList[2].Rows[cnt][fty]) ? "0%" : this.dtList[2].Rows[cnt][fty] + "%";
                        if (MyUtility.Convert.GetDecimal(this.dtList[2].Rows[cnt][fty]) > 90)
                        {
                            wkcolor.Cells[2 + i, 10 + f].Interior.Color = this.BackGreen;
                            wkcolor.Cells[2 + i, 10 + f].Font.Color = this.FontGreen;
                        }
                        else
                        {
                            wkcolor.Cells[2 + i, 10 + f].Interior.Color = this.BackRed;
                            wkcolor.Cells[2 + i, 10 + f].Font.Color = this.FontRed;
                        }

                        cnt++;
                    }
                    else
                    {
                        string rowformat1 = MyExcelPrg.GetExcelColumnName(9);
                        string rowformat2 = MyExcelPrg.GetExcelColumnName(9 + this.dtFty.Rows.Count);
                        string rowformat3 = MyExcelPrg.GetExcelColumnName(10);
                        wkcolor.Range[$"{rowformat1}{2 + i}", $"{rowformat2}{2 + i}"].Interior.Color = Color.Red;
                        wkcolor.Range[$"{rowformat3}{2 + i}", $"{rowformat2}{2 + i}"].Merge();
                    }
                }
            }

            //畫框線
            Microsoft.Office.Interop.Excel.Range rg1;
            rg1 = wkcolor.Range["I1", $"{MyExcelPrg.GetExcelColumnName(9 + this.dtFty.Rows.Count)}{range + 1}"];
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

            for (int i = 0; i < this.dtList[1].Rows.Count; i++)
            {
                if (this.dtList[1].Rows[i]["Fty"].ToString() == "Total")
                {
                    wkcolor.Range[$"C{i + 2}", $"F{i + 2}"].Font.Bold = true;
                }

                if (this.dtList[1].Rows[i]["Fty"].ToString() == "Grand Total")
                {
                    wkcolor.Cells[i + 2, 2].Value = "";
                    wkcolor.Range[$"C{i + 2}", $"G{i + 2}"].Font.Bold = true;
                }
            }

            //畫框線
            Microsoft.Office.Interop.Excel.Range rg2;
            rg2 = wkcolor.Range["A1", $"G{this.dtList[1].Rows.Count + 1}"];
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
