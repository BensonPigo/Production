using Ict;
using Sci.Data;
using System;
using System.Data;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.IE
{
    /// <summary>
    /// IE_R02
    /// </summary>
    public partial class R02 : Win.Tems.PrintForm
    {
        private string monthS;
        private string monthE;
        private string factory;
        private string sewingLine;
        private DataTable printDataSummary;
        private DataTable printDataDetail;

        /// <summary>
        /// IE_R02
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R02(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            DataTable factory;
            DBProxy.Current.Select(
                null,
                string.Format(
                    @"select '' as FtyGroup 
                      union all
                      select distinct FTYGroup from Factory WITH (NOLOCK) order by FTYGroup"),
                out factory);

            MyUtility.Tool.SetupCombox(this.comboFactory, 1, factory);
            this.comboFactory.Text = Env.User.Factory;
            this.dateTPmonth.Value = DateTime.Now;
        }

        // Sewing Line
        private void TxtSewingLineStart_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            this.txtSewingLine.Text = this.SelectSewingLine(this.txtSewingLine.Text);
        }

        private string SelectSewingLine(string line)
        {
            string sql = string.Format("Select Distinct ID From SewingLine WITH (NOLOCK) {0}", MyUtility.Check.Empty(this.comboFactory.Text) ? string.Empty : string.Format(" where FactoryID = '{0}'", this.comboFactory.Text));
            Win.Tools.SelectItem item = new Win.Tools.SelectItem(sql, "3", line, false, ",")
            {
                Width = 300,
            };
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return string.Empty;
            }
            else
            {
                return item.GetSelectedString();
            }
        }

        /// <summary>
        /// ValidateInput 驗證輸入條件
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ValidateInput()
        {
            DateTime? dtS = MyUtility.Convert.GetDate(this.dateTPmonth.Text + "-01");
            if (!dtS.HasValue)
            {
                MyUtility.Msg.WarningBox("Year / Month can't empty!!");
                return false;
            }

            this.monthS = dtS.Value.ToString("yyyyMMdd");
            this.monthE = dtS.Value.AddMonths(1).AddDays(-1).ToString("yyyyMMdd");
            this.factory = this.comboFactory.Text;
            this.sewingLine = this.txtSewingLine.Text;
            return base.ValidateInput();
        }

        /// <summary>
        /// OnAsyncDataLoad 非同步取資料
        /// </summary>
        /// <param name="e">e</param>
        /// <returns>DualResult</returns>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            DualResult result = this.GetSummary();
            if (!result)
            {
                return result;
            }

            result = this.GetDetail();
            if (!result)
            {
                return result;
            }

            return Ict.Result.True;
        }

        private DualResult GetSummary()
        {
            StringBuilder sqlCmd = new StringBuilder();
            DualResult result;

            sqlCmd.Append(string.Format(
                @"
declare @sql varchar(max) = ''
declare @detial_count int = 1
declare @dayInline as varchar(10)
declare @beginDate as date = '{0}'
declare @endDate as date = '{1}'
declare @dayTable table(dday date)

while @beginDate <= @endDate
begin
	insert into @dayTable(dday)
	values(@beginDate)
	set @beginDate = dateadd(day,1,@beginDate)
end

select distinct FactoryID,SewingLineID, d.dday
into #tmp
from ChgOver c, @dayTable d
where c.Inline >= '{0}' 
and c.Inline < dateadd(day,1,'{1}')
",
                this.monthS,
                this.monthE));

            if (!MyUtility.Check.Empty(this.factory))
            {
                sqlCmd.Append(string.Format(" and c.FactoryID = '{0}'", this.factory));
            }

            if (!MyUtility.Check.Empty(this.sewingLine))
            {
                sqlCmd.Append(string.Format(" and c.SewingLineID = '{0}'", this.sewingLine));
            }

            sqlCmd.Append(string.Format(
                @"

select t.FactoryID
	,[SewingLineID] = right('00' + t.SewingLineID, 2)
	,[dayInline] = t.dday	
	,[dayCount] = count(c.Inline)
into #tmp_ChgOver
from #tmp t
left join (
	select FactoryID, SewingLineID, Inline
	from ChgOver
	where Inline >= '{0}' 
	and Inline < dateadd(day,1,'{1}') 
	group by FactoryID, SewingLineID, Inline
)c on t.FactoryID = c.FactoryID and t.SewingLineID = c.SewingLineID and t.dday = convert(nvarchar(8),c.Inline,112)
group by t.FactoryID, t.SewingLineID, t.dday

select FactoryID, SewingLineID, [totalDayCount] = sum(dayCount)
into #tmp_ChgOver_sumDayCount
from #tmp_ChgOver t
group by FactoryID, SewingLineID

select dayInline
	,ROW_NUMBER() over(order by dayInline) R_ID
into #tmp_ChgOver_GroupbyDayInline
from #tmp_ChgOver 
group by dayInline

select @detial_count = count(*)	from #tmp_ChgOver_GroupbyDayInline 

while(@detial_count > 0)
begin
	set rowcount 1 
	select @dayInline = dayInline from #tmp_ChgOver_GroupbyDayInline 

	set @sql = @sql + '
			
			,isnull(Max(case when t.dayInline ='''+@dayInline+''' then t.dayCount end),0) as ['+ format(cast(@dayInline as date),'yyyy/MM/dd')+']'
	--刪
	delete from #tmp_ChgOver_GroupbyDayInline 

	--計算還剩幾筆，@countTemp > 0繼續
	select @detial_count = count(*) from #tmp_ChgOver_GroupbyDayInline   
end
set rowcount 0  
		
set @sql = '
	select t.FactoryID
		,t.SewingLineID
		,[Total] =  s.totalDayCount
		' + @sql + ' 
	from #tmp_ChgOver t 
	left join #tmp_ChgOver_sumDayCount s on t.FactoryID = s.FactoryID and t.SewingLineID = s.SewingLineID  
	group by t.FactoryID, t.SewingLineID, s.totalDayCount
	order by t.FactoryID, t.SewingLineID
'
--select @sql
exec (@sql)  

drop table #tmp, #tmp_ChgOver ,#tmp_ChgOver_sumDayCount,#tmp_ChgOver_GroupbyDayInline",
                this.monthS,
                this.monthE));

            result = DBProxy.Current.Select(null, sqlCmd.ToString(), out this.printDataSummary);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query Summary data fail\r\n" + result.ToString());
                return failResult;
            }

            return result;
        }

        private DualResult GetDetail()
        {
            StringBuilder sqlCmd = new StringBuilder();
            DualResult result;

            sqlCmd.Append(string.Format(
                @"
select distinct a.FactoryID
	, [SewingLineID] = right('00' + a.SewingLineID, 2)
	, a.Inline
	, [OldSP] = b.OrderID
	, [OldStyle] = b.StyleID
	, [OldComboType] = b.ComboType
	, [OrderID] = c.OrderID
	, [StyleID] = c.StyleID
	, [ComboType] = c.ComboType 
from ChgOver a 
outer apply 
(
	select top 1 OrderID, StyleID, ComboType
	from ChgOver
	where Inline = (select max(Inline) from ChgOver where FactoryID = a.FactoryID and SewingLineID = a.SewingLineID and Inline < a.Inline)
	and FactoryID = a.FactoryID
	and SewingLineID = a.SewingLineID
)b
outer apply 
(
	select top 1 OrderID, StyleID, ComboType
	from ChgOver
	where Inline = (select max(Inline) from ChgOver where FactoryID = a.FactoryID and SewingLineID = a.SewingLineID and Inline = a.Inline)
	and FactoryID = a.FactoryID
	and SewingLineID = a.SewingLineID
)c
where a.Inline >= '{0}'
and a.Inline < dateadd(day, 1, '{1}')",
                this.monthS,
                this.monthE));

            if (!MyUtility.Check.Empty(this.factory))
            {
                sqlCmd.Append(string.Format(" and a.FactoryID = '{0}'", this.factory));
            }

            if (!MyUtility.Check.Empty(this.sewingLine))
            {
                sqlCmd.Append(string.Format(" and a.SewingLineID = '{0}'", this.sewingLine));
            }

            sqlCmd.Append(string.Format(" order by a.FactoryID, right('00' + a.SewingLineID, 2), a.Inline"));

            result = DBProxy.Current.Select(null, sqlCmd.ToString(), out this.printDataDetail);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query Detail data fail\r\n" + result.ToString());
                return failResult;
            }

            return result;
        }

        /// <summary>
        /// OnToExcel 產生Excel
        /// </summary>
        /// <param name="report">report</param>
        /// <returns>bool</returns>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.printDataDetail.Rows.Count);

            if (this.printDataDetail.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            this.ShowWaitMessage("Starting EXCEL...");
            string strXltName = Env.Cfg.XltPathDir + "\\IE_R02_StyleChangeoverReport.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return false;
            }

            Microsoft.Office.Interop.Excel.Worksheet objSheets = excel.ActiveWorkbook.Worksheets[1];   // 取得工作表
            foreach (DataColumn column in this.printDataSummary.Columns)
            {
                int index = this.printDataSummary.Columns.IndexOf(column) + 1;
                objSheets.Cells[1, index] = column.ColumnName;
            }

            MyUtility.Excel.CopyToXls(this.printDataSummary, string.Empty, "IE_R02_StyleChangeoverReport.xltx", 1, false, null, excel); // 將datatable copy to excel
            MyUtility.Excel.CopyToXls(this.printDataDetail, string.Empty, "IE_R02_StyleChangeoverReport.xltx", 1, false, null, excel, true, excel.ActiveWorkbook.Worksheets[2]); // 將datatable copy to excel

            excel.Cells.EntireColumn.AutoFit();
            excel.Cells.EntireRow.AutoFit();
            this.HideWaitMessage();

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("IE_R02_StyleChangeoverReport");
            Microsoft.Office.Interop.Excel.Workbook workbook = excel.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            excel.Quit();
            Marshal.ReleaseComObject(objSheets);    // 釋放sheet
            Marshal.ReleaseComObject(excel);        // 釋放objApp
            Marshal.ReleaseComObject(workbook);

            strExcelName.OpenFile();
            #endregion
            return true;
        }
    }
}
