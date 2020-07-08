using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Planning
{
    /// <summary>
    /// R18
    /// </summary>
    public partial class R18 : Win.Tems.PrintForm
    {
        private int selectindex = 0;
        private string factory;
        private string mdivision;
        private DateTime? sewingDate1;
        private DateTime? sewingDate2;
        private DataTable printData;
        private DataTable dtDateList;
        private StringBuilder condition = new StringBuilder();
        private StringBuilder datelist = new StringBuilder();

        /// <summary>
        /// R18
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R18(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.txtMdivision.Text = Sci.Env.User.Keyword;
            this.txtfactory.Text = Sci.Env.User.Factory;
            MyUtility.Tool.SetupCombox(this.comboArtworkType, 1, 1, "HT(UA),HT(Non-UA)");
            this.comboArtworkType.SelectedIndex = 0;
            this.dateSewingDate.Value1 = DateTime.Now.Date;
            this.dateSewingDate.Value2 = DateTime.Now.Date.AddMonths(2);
        }

        /// <summary>
        /// ValidateInput
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ValidateInput()
        {
            #region -- 必輸的條件 --
            this.sewingDate1 = this.dateSewingDate.Value1;
            this.sewingDate2 = this.dateSewingDate.Value2;
            #endregion
            this.mdivision = this.txtMdivision.Text;
            this.factory = this.txtfactory.Text;
            this.selectindex = this.comboArtworkType.SelectedIndex;

            DualResult result;
            string sql = string.Format(
                @"
;with expend_date as 
(
    select cast('{0}' as date) workdate,cast('{1}' as date) endDate ,CONVERT(char(10), '{0}', 111) workdateStr
    union all
    select dateadd(day,1,workdate),endDate , CONVERT(CHAR(10),dateadd(day,1,workdate),111) from expend_date 
    where dateadd(day,1,workdate) <= endDate)
    select * from expend_date option (maxrecursion 365)",
                Convert.ToDateTime(this.sewingDate1).ToString("d"),
                Convert.ToDateTime(this.sewingDate2).ToString("d"));
            if (!(result = DBProxy.Current.Select(string.Empty, sql, out this.dtDateList)))
            {
                MyUtility.Msg.ErrorBox("Sewing date can not more than 365 days !!");
                return false;
            }

            if (this.dtDateList.Rows.Count == 0)
            {
                MyUtility.Msg.ErrorBox("Artwork Type data not found, Please inform MIS to check !");
                return false;
            }

            this.datelist.Clear();
            for (int i = 0; i < this.dtDateList.Rows.Count; i++)
            {
                this.datelist.Append(string.Format(@"[{0}],", this.dtDateList.Rows[i]["workdateStr"].ToString()));
            }

            return base.ValidateInput();
        }

        /// <summary>
        /// OnAsyncDataLoad
        /// </summary>
        /// <param name="e">e</param>
        /// <returns>DualResult</returns>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            #region -- sql parameters declare --

            System.Data.SqlClient.SqlParameter sp_factory = new System.Data.SqlClient.SqlParameter();
            sp_factory.ParameterName = "@factory";

            System.Data.SqlClient.SqlParameter sp_mdivision = new System.Data.SqlClient.SqlParameter();
            sp_mdivision.ParameterName = "@MDivision";

            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();

            #endregion

            StringBuilder sqlCmd = new StringBuilder();

            sqlCmd.Append(string.Format(@"
;with sch as (
select SewingSchedule.id,SewingSchedule.factoryid,sewinglineid
,orders.StyleID
,orderid
,AlloQty
,cast(Inline as date) workdate,Inline,Offline,cast(offline as date) offlineDate,StandardOutput,WorkHour,WorkDay,Order_TmsCost.TMS
,dbo.checkholiday(SewingSchedule.factoryid,cast(Inline as date),SewingSchedule.SewingLineID) holiday
,(select AVG(w.hours) avg_workhours from dbo.WorkHour w WITH (NOLOCK) 
	where w.FactoryID = SewingSchedule.FactoryID and w.SewingLineID = SewingSchedule.SewingLineID
	and w.Date between cast(Inline as date) and cast(offline as date) and w.Holiday = 0) avg_workhours
from dbo.SewingSchedule WITH (NOLOCK) 
inner join dbo.orders WITH (NOLOCK) on orders.id = SewingSchedule.OrderID and orders.Category  in ('B','S')
inner join dbo.Order_TmsCost WITH (NOLOCK) on Order_TmsCost.ID = orders.id 
where Order_TmsCost.ArtworkTypeID='HEAT TRANSFER' 
AND Order_TmsCost.TMS > 0 
"));

            #region --- 條件組合  ---
            this.condition.Clear();
            if (!MyUtility.Check.Empty(this.sewingDate1) && !MyUtility.Check.Empty(this.sewingDate2))
            {
                sqlCmd.Append(string.Format(@" AND ((SewingSchedule.Inline BETWEEN '{0:d}' AND '{1}') OR (SewingSchedule.Offline BETWEEN '{0:d}' AND '{1}'))", this.sewingDate1, Convert.ToDateTime(this.sewingDate2).ToString("d")));
                this.condition.Append(string.Format(@"SCI Delivery : {0} ~ {1}", Convert.ToDateTime(this.sewingDate1).ToString("d"), Convert.ToDateTime(this.sewingDate2).ToString("d")));
            }
            else
            {
                if (!MyUtility.Check.Empty(this.sewingDate1))
                {
                    sqlCmd.Append(string.Format(@" and SewingSchedule.Inline >= '{0}' or SewingSchedule.Offline >='{0}' ", Convert.ToDateTime(this.sewingDate1).ToString("d")));
                    this.condition.Append(string.Format(@"SCI Delivery : {0} ~ ", Convert.ToDateTime(this.sewingDate1).ToString("d")));
                }

                if (!MyUtility.Check.Empty(this.sewingDate2))
                {
                    sqlCmd.Append(string.Format(@" and SewingSchedule.Inline <= '{0}' or SewingSchedule.Offline <='{0}'", Convert.ToDateTime(this.sewingDate2).ToString("d")));
                    this.condition.Append(string.Format(@"SCI Delivery :  ~ {0} ", Convert.ToDateTime(this.sewingDate2).ToString("d")));
                }
            }

            if (!MyUtility.Check.Empty(this.mdivision))
            {
                sqlCmd.Append(" and orders.mdivisionid = @MDivision");
                sp_mdivision.Value = this.mdivision;
                cmds.Add(sp_mdivision);
                this.condition.Append(string.Format(@"    M : {0}", this.mdivision));
            }

            if (!MyUtility.Check.Empty(this.factory))
            {
                sqlCmd.Append(" and orders.FtyGroup = @factory");
                sp_factory.Value = this.factory;
                cmds.Add(sp_factory);
                this.condition.Append(string.Format(@"    Factory : {0}", this.factory));
            }

            switch (this.selectindex)
            {
                case 0:
                    sqlCmd.Append(@" AND orders.BrandID = 'U.ARMOUR'");
                    break;
                case 1:
                    sqlCmd.Append(@" AND orders.BrandID != 'U.ARMOUR' ");
                    break;
            }

            this.condition.Append(string.Format(@"    Category : {0}", this.comboArtworkType.SelectedText));

            #endregion

            sqlCmd.Append(string.Format(
                @"union all
select id,factoryid,sewinglineid
,StyleID
,orderid
,AlloQty
,dateadd(day,1,workdate) ,Inline,Offline ,offlineDate  ,StandardOutput,WorkHour,WorkDay,TMS
,dbo.checkholiday(factoryid,dateadd(day,1,workdate),SewingLineID) holiday
,avg_workhours
from sch where  dateadd(day,1,workdate) <= offlineDate
)
, grouping_data as (select  
sch.FactoryID
,sch.SewingLineID
,sch.StyleID
,sch.OrderID
,sch.AlloQty
,sch.inline
,sch.Offline
,sch.workdate
,sch.WorkDay
,sch.WorkHour
,sch.TMS
,sch.StandardOutput
,sch.avg_workhours
,sch.StandardOutput * sch.avg_workhours StdOutputPerDay
,sum(sch.StandardOutput * sch.avg_workhours) over 
(partition by sch.id
 order by factoryid,inline,sewinglineid,orderid rows between unbounded preceding and current row) as running_total_stdoutput
 ,sum(sch.avg_workhours) over 
(partition by sch.id
 order by factoryid,inline,sewinglineid,orderid rows between unbounded preceding and current row) as running_total_workhours
from sch 
where Holiday = 0
)
, detail as (select *
,round(iif(workday = 1
		,iif(workhour=0,null,tms * alloqty / 3600 / 0.8 / workhour)
			,iif(running_total_stdoutput > alloqty	-- 如果累計的Std. Output已經超過alloQty，就依最後剩餘數的數量來計算 (但無法算當天的工作時數)
			, (stdoutputperday - (running_total_stdoutput-alloqty)) * tms / 3600 / 0.8 / avg_workhours
			,tms * StandardOutput / 3600 / 0.8)) -- 因為分子分母都要乘上完整一天的工時，就先相消了	
		,2) TTL_HT_TMS
 from grouping_data
 where running_total_workhours - WorkHour < avg_workhours -- 因為無法依上線時間計算出最真實的工作時數，所以累計工時可能不夠展算到下線日
 )

,theDetail as (
select topdata.factoryid,topdata.sewinglineid,topdata.StyleID,id_list.OrderID,topdata.AlloQty,topdata.Inline,topdata.Offline,topdata.avg_workhours,topdata.TMS,id_list.TTL_HT_TMS,id_list.workdate
from (select distinct orderid,FactoryID,SewingLineID,styleid,TTL_HT_TMS,workdate from detail) as id_list
outer apply(select top 1* from detail dt where dt.FactoryID=id_list.FactoryID and dt.SewingLineID=id_list.SewingLineID and dt.StyleID=id_list.StyleID and dt.OrderID=id_list.OrderID  ) as topdata 
)
select FactoryID,SewingLineID,StyleID,OrderID,AlloQty,Inline,Offline,avg_workhours,TMS,{0}
from theDetail
pivot
    (
        sum(TTL_HT_TMS)
        for workdate  in ({0})
    )as pvt  order by factoryid,sewinglineid,orderid,inline
", this.datelist.ToString().Substring(0, this.datelist.ToString().Length - 1)));

            DBProxy.Current.DefaultTimeout = 1800;
            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), cmds, out this.printData);
            DBProxy.Current.DefaultTimeout = 0;
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return Result.True;
        }

        /// <summary>
        /// OnToExcel
        /// </summary>
        /// <param name="report">report</param>
        /// <returns>bool</returns>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.printData.Rows.Count);

            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.InfoBox("Data not found!");
                return false;
            }

            if (this.printData.Columns.Count > 16384)
            {
                MyUtility.Msg.ErrorBox("Columns of Data is over 16,384 in excel file, please narrow down range of condition.");
                return false;
            }

            if (this.printData.Rows.Count + 6 > 1048576)
            {
                MyUtility.Msg.ErrorBox("Lines of Data is over 1,048,576 in excel file, please narrow down range of condition.");
                return false;
            }

            Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Planning_R18.xltx"); // 預先開啟excel app
            MyUtility.Excel.CopyToXls(this.printData, string.Empty, "Planning_R18.xltx", 1, false, null, objApp);      // 將datatable copy to excel
            objApp.Visible = false;
            Excel.Worksheet objSheet = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
            objSheet.Name = this.comboArtworkType.SelectedValue.ToString();

            Excel.Range range = null;
            for (int i = 0; i < this.dtDateList.Rows.Count; i++)
            {
                objSheet.Cells[1, 10 + i] = this.dtDateList.Rows[i]["workdateStr"].ToString();
                range = (Excel.Range)objSheet.Cells[1, 10 + i];
                range.Select();
                range.HorizontalAlignment = -4108;
                range.VerticalAlignment = -4108;
                range.WrapText = false;
                range.Orientation = 0;
                range.AddIndent = false;
                range.IndentLevel = 0;
                range.ShrinkToFit = false;
                range.EntireColumn.AutoFit();
            }

            #region Save & Show Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Planning_R18");
            Excel.Workbook workbook = objApp.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);
            Marshal.ReleaseComObject(objSheet);
            Marshal.ReleaseComObject(range);
            Marshal.ReleaseComObject(workbook);

            strExcelName.OpenFile();
            #endregion
            return true;
        }
    }
}