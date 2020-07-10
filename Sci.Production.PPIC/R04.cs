using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using System.Runtime.InteropServices;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// R04
    /// </summary>
    public partial class R04 : Win.Tems.PrintForm
    {
        private DateTime? date1;
        private DateTime? date2;
        private string mDivision;
        private string factory;
        private string pivotContent;
        private int reportType;
        private int totalFactory;
        private int leadtime;
        private DataTable printData;
        private DataTable reasonData;
        private DataTable pivotData;

        /// <summary>
        /// R04
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public R04(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();

            MyUtility.Tool.SetupCombox(this.comboReportType, 1, 1, "Fabric,Accessory");
            DataTable mDivision, factory;
            DBProxy.Current.Select(null, "select '' as ID union all select ID from MDivision WITH (NOLOCK) ", out mDivision);
            MyUtility.Tool.SetupCombox(this.comboM, 1, mDivision);
            DBProxy.Current.Select(null, "select '' as ID union all select distinct FtyGroup from Factory WITH (NOLOCK) ", out factory);
            MyUtility.Tool.SetupCombox(this.comboFactory, 1, factory);
            MyUtility.Tool.SetupCombox(this.comboLeadtime, 2, 1, "7200,2hrs,10800,3hrs,14400,4hrs");
            this.comboReportType.SelectedIndex = 0;
            this.comboLeadtime.SelectedIndex = 1;
            this.comboM.Text = Env.User.Keyword;
            this.comboFactory.Text = Env.User.Factory;

            this.dateApvDate.Value1 = DateTime.Today.AddDays(-1);
            this.dateApvDate.Value2 = DateTime.Today.AddDays(-1);
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if (this.comboReportType.SelectedIndex == -1)
            {
                MyUtility.Msg.WarningBox("Report Type can't empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(this.dateApvDate.Value1))
            {
                MyUtility.Msg.WarningBox("Apv. Date can't empty!!");
                return false;
            }

            this.date1 = this.dateApvDate.Value1;
            this.date2 = this.dateApvDate.Value2;
            this.reportType = this.comboReportType.SelectedIndex;
            this.mDivision = this.comboM.Text;
            this.factory = this.comboFactory.Text;
            this.leadtime = MyUtility.Convert.GetInt(this.comboLeadtime.SelectedValue);
            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            StringBuilder sqlCondition = new StringBuilder();
            sqlCondition.Append(string.Format(@"where l.FabricType = '{0}' ", this.reportType == 0 ? "F" : "A"));
            if (!MyUtility.Check.Empty(this.date1))
            {
                sqlCondition.Append(string.Format(" and convert(date,l.ApvDate) >= '{0}'", Convert.ToDateTime(this.date1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.date2))
            {
                sqlCondition.Append(string.Format(" and convert(date,l.ApvDate) <= '{0}'", Convert.ToDateTime(this.date2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.mDivision))
            {
                sqlCondition.Append(string.Format(" and l.MDivisionID = '{0}'", this.mDivision));
            }

            if (!MyUtility.Check.Empty(this.factory))
            {
                sqlCondition.Append(string.Format(" and l.FactoryID = '{0}'", this.factory));
            }

            sqlCmd.Append(string.Format(
                @"select distinct l.MDivisionID,l.FactoryID,l.ID,l.SewingLineID
	                    ,[SewingCell] = isnull(s.SewingCell,'')
	                    ,[StyleID] = isnull(o.StyleID,'')
	                    ,l.OrderID
	                    ,[Seq] = concat(ld.Seq1,' ',ld.Seq2)
	                    ,[ColorName] = isnull(c.Name,'')
	                    ,[Refno] = isnull(psd.Refno,'')
	                    ,l.ApvDate
	                    ,ld.RejectQty
	                    ,ld.RequestQty
	                    ,ld.IssueQty
	                    ,[FinishedDate] = IIF(l.Status= 'Received',l.EditDate,null)
	                    ,[Type] = IIF(l.Type='R','Replacement','Lacking')
	                    ,[Description] = isnull(IIF(l.FabricType = 'F',pr.Description,pr1.Description),PPICReasonID)
	                    ,[OnTime] = IIF(l.Status = 'Received',IIF(DATEDIFF(ss,l.ApvDate,l.EditDate) <= {1},'Y','N'),'N')
	                    ,l.Remark
                        ,ld.Process
                    from Lack l WITH (NOLOCK) 
                    inner join Lack_Detail ld WITH (NOLOCK) on l.ID = ld.ID
                    left join SewingLine s WITH (NOLOCK) on s.ID = l.SewingLineID AND S.FactoryID=L.FactoryID
                    left join Orders o WITH (NOLOCK) on o.ID = l.OrderID
                    left join PO_Supp_Detail psd WITH (NOLOCK) on psd.ID = l.POID and psd.SEQ1 = ld.Seq1 and psd.SEQ2 = ld.Seq2
                    left join Color c WITH (NOLOCK) on c.BrandId = o.BrandID and c.ID = psd.ColorID
                    left join PPICReason pr WITH (NOLOCK) on pr.Type = 'FL' and (pr.ID = ld.PPICReasonID or pr.ID = concat('FR','0',ld.PPICReasonID))
                    left join PPICReason pr1 WITH (NOLOCK) on pr1.Type = 'AL' and (pr1.ID = ld.PPICReasonID or pr1.ID = concat('AR','0',ld.PPICReasonID))
                    {0} 
                    order by l.MDivisionID,l.FactoryID,l.ID",
                sqlCondition.ToString(),
                this.leadtime.ToString()));
            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out this.printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            // 有資料的話才繼續撈資料
            if (this.printData.Rows.Count > 0)
            {
                sqlCmd.Clear();
                sqlCmd.Append(string.Format(
                    @"
select distinct Description = isnull(IIF(l.FabricType = 'F',pr.Description,pr1.Description),PPICReasonID)
from Lack l WITH (NOLOCK) 
inner join Lack_Detail ld WITH (NOLOCK) on l.ID = ld.ID
left join PPICReason pr WITH (NOLOCK) on pr.Type = 'FL' and (pr.ID = ld.PPICReasonID or pr.ID = concat('FR','0',ld.PPICReasonID))
left join PPICReason pr1 WITH (NOLOCK) on pr1.Type = 'AL' and (pr1.ID = ld.PPICReasonID or pr1.ID = concat('AR','0',ld.PPICReasonID))
{0}
order by Description", sqlCondition.ToString()));
                result = DBProxy.Current.Select(null, sqlCmd.ToString(), out this.reasonData);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Query all reason fail\r\n" + result.ToString());
                    return failResult;
                }

                this.pivotContent = MyUtility.GetValue.Lookup(string.Format(
                    @"
select concat('[',Description,']',',')
from 
(
	select distinct Description = isnull(IIF(l.FabricType = 'F',pr.Description,pr1.Description),PPICReasonID) 
	from Lack l WITH (NOLOCK) 
	inner join Lack_Detail ld WITH (NOLOCK) on l.ID = ld.ID
	left join PPICReason pr WITH (NOLOCK) on pr.Type = 'FL' and (pr.ID = ld.PPICReasonID or pr.ID = concat('FR','0',ld.PPICReasonID))
	left join PPICReason pr1 WITH (NOLOCK) on pr1.Type = 'AL' and (pr1.ID = ld.PPICReasonID or pr1.ID = concat('AR','0',ld.PPICReasonID))
	{0}
) a
order by Description
for xml path('')", sqlCondition.ToString()));

                sqlCmd.Clear();
                sqlCmd.Append(string.Format(
                    @"
with tmpData as (
	select l.MDivisionID,l.FactoryID
		,Description = isnull(IIF(l.FabricType = 'F',pr.Description,pr1.Description),ld.PPICReasonID)
		,RequestQty = IIF(l.FabricType = 'F',sum(ld.RejectQty),sum(ld.RequestQty))
	from Lack l WITH (NOLOCK) 
	inner join Lack_Detail ld WITH (NOLOCK) on l.ID = ld.ID
	left join PPICReason pr WITH (NOLOCK) on pr.Type = 'FL' and (pr.ID = ld.PPICReasonID or pr.ID = concat('FR','0',ld.PPICReasonID))
	left join PPICReason pr1 WITH (NOLOCK) on pr1.Type = 'AL' and (pr1.ID = ld.PPICReasonID or pr1.ID = concat('AR','0',ld.PPICReasonID))
	{0}
	group by l.MDivisionID,l.FactoryID,isnull(IIF(l.FabricType = 'F',pr.Description,pr1.Description),ld.PPICReasonID),l.FabricType
)
select distinct *
from tmpData
PIVOT 
(
	SUM(RequestQty)
	FOR Description IN ({1})
) a
order by MDivisionID,FactoryID",
                    sqlCondition.ToString(),
                    this.pivotContent.Substring(0, this.pivotContent.Length - 1)));
                result = DBProxy.Current.Select(null, sqlCmd.ToString(), out this.pivotData);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Query pivot data fail\r\n" + result.ToString());
                    return failResult;
                }

                this.totalFactory = MyUtility.Convert.GetInt(MyUtility.GetValue.Lookup(string.Format("select COUNT(distinct FactoryID) from Lack l WITH (NOLOCK) {0}", sqlCondition.ToString())));
            }

            return Ict.Result.True;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.printData.Rows.Count);

            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            this.ShowWaitMessage("Starting EXCEL...");
            string strXltName = Env.Cfg.XltPathDir + (this.reportType == 0 ? "\\PPIC_R04_FabricBCS.xltx" : "\\PPIC_R04_AccessoryBCS.xltx");
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return false;
            }

            excel.DisplayAlerts = false;

            excel.Visible = true;
            for (int i = 0; i < this.pivotData.Rows.Count; i++)
            {
                if (i > 0)
                {
                    Microsoft.Office.Interop.Excel.Worksheet worksheet2 = (Microsoft.Office.Interop.Excel.Worksheet)excel.ActiveWorkbook.Worksheets[2];
                    Microsoft.Office.Interop.Excel.Worksheet worksheetn = (Microsoft.Office.Interop.Excel.Worksheet)excel.ActiveWorkbook.Worksheets[i + 1];
                    worksheet2.Copy(worksheetn);
                    Marshal.ReleaseComObject(worksheet2);
                    Marshal.ReleaseComObject(worksheetn);
                }
            }

            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            // 填各工廠的明細資料
            string xlsFactory = string.Empty;
            int xlsSheet = 1, ttlCount = 0, intRowsStart = 7;
            object[,] objArray = new object[1, 18];
            foreach (DataRow dr in this.printData.Rows)
            {
                if (MyUtility.Convert.GetString(dr["FactoryID"]) != xlsFactory)
                {
                    if (xlsSheet != 1)
                    {
                        worksheet.Cells[3, 5] = string.Format("=COUNTA(C7:C{0})", MyUtility.Convert.GetString(ttlCount + 6));
                        if (this.reportType == 0)
                        {
                            worksheet.Cells[4, 5] = string.Format("=COUNTIF(Q7:Q{0},\"=Y\")", MyUtility.Convert.GetString(ttlCount + 6));
                            worksheet.Cells[3, 16] = string.Format("=SUM(L7:L{0})", MyUtility.Convert.GetString(ttlCount + 6));
                        }
                        else
                        {
                            worksheet.Cells[4, 5] = string.Format("=COUNTIF(P7:P{0},\"=Y\")", MyUtility.Convert.GetString(ttlCount + 6));
                            worksheet.Cells[3, 15] = string.Format("=SUM(J7:J{0})", MyUtility.Convert.GetString(ttlCount + 6));
                        }
                    }

                    xlsSheet++;
                    worksheet = excel.ActiveWorkbook.Worksheets[xlsSheet];
                    worksheet.Name = MyUtility.Convert.GetString(dr["FactoryID"]);
                    worksheet.Cells[3, 8] = string.Format("{0} ~ {1}", Convert.ToDateTime(this.date1).ToString("d"), Convert.ToDateTime(this.date2).ToString("d"));
                    worksheet.Cells[4, 8] = this.comboLeadtime.Text;
                    xlsFactory = MyUtility.Convert.GetString(dr["FactoryID"]);
                    ttlCount = 0;
                    intRowsStart = 7;
                }

                ttlCount++;

                objArray[0, 0] = dr["SewingCell"];
                objArray[0, 1] = dr["SewingLineID"];
                objArray[0, 2] = dr["ID"];
                objArray[0, 3] = dr["StyleID"];
                objArray[0, 4] = dr["OrderID"];
                objArray[0, 5] = dr["Seq"];
                objArray[0, 6] = dr["ColorName"];
                objArray[0, 7] = dr["Refno"];
                objArray[0, 8] = dr["ApvDate"];
                if (this.reportType == 0)
                {
                    objArray[0, 9] = dr["RejectQty"];
                    objArray[0, 10] = dr["RequestQty"];
                    objArray[0, 11] = dr["IssueQty"];
                    objArray[0, 12] = dr["FinishedDate"];
                    objArray[0, 13] = dr["Type"];
                    objArray[0, 14] = dr["Process"];
                    objArray[0, 15] = dr["Description"];
                    objArray[0, 16] = dr["OnTime"];
                    objArray[0, 17] = dr["Remark"];
                }
                else
                {
                    objArray[0, 9] = dr["RequestQty"];
                    objArray[0, 10] = dr["IssueQty"];
                    objArray[0, 11] = dr["FinishedDate"];
                    objArray[0, 12] = dr["Type"];
                    objArray[0, 13] = dr["Process"];
                    objArray[0, 14] = dr["Description"];
                    objArray[0, 15] = dr["OnTime"];
                    objArray[0, 16] = dr["Remark"];
                    objArray[0, 17] = string.Empty;
                }

                worksheet.Range[string.Format("A{0}:R{0}", intRowsStart)].Value2 = objArray;
                intRowsStart++;
            }

            worksheet.Cells[3, 5] = string.Format("=COUNTA(C7:C{0})", MyUtility.Convert.GetString(ttlCount + 6));
            if (this.reportType == 0)
            {
                worksheet.Cells[4, 5] = string.Format("=COUNTIF(Q7:Q{0},\"=Y\")", MyUtility.Convert.GetString(ttlCount + 6));
                worksheet.Cells[3, 16] = string.Format("=SUM(L7:L{0})", MyUtility.Convert.GetString(ttlCount + 6));
            }
            else
            {
                worksheet.Cells[4, 5] = string.Format("=COUNTIF(P7:P{0},\"=Y\")", MyUtility.Convert.GetString(ttlCount + 6));
                worksheet.Cells[3, 15] = string.Format("=SUM(J7:J{0})", MyUtility.Convert.GetString(ttlCount + 6));
            }

            for (int i = 2; i < xlsSheet + 1; i++)
            {
                worksheet = excel.ActiveWorkbook.Worksheets[i];
                worksheet.Rows.AutoFit();
            }

            worksheet = excel.ActiveWorkbook.Worksheets[1];

            // Summary -- Header
            if (this.reasonData.Rows.Count > 8)
            {
                // 新增填Raseon欄位
                for (int i = 9; i <= this.reasonData.Rows.Count; i++)
                {
                    Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range("L:L", Type.Missing).EntireColumn;
                    rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                    Marshal.ReleaseComObject(rngToInsert);
                }
            }
            else if (this.reasonData.Rows.Count < 8)
            {
                string columnEng = PublicPrg.Prgs.GetExcelEnglishColumnName(this.reasonData.Rows.Count + 5);

                // 刪除多的Raseon欄位
                for (int i = 1; i <= 8 - this.reasonData.Rows.Count; i++)
                {
                    Microsoft.Office.Interop.Excel.Range rngToDelete = worksheet.get_Range(string.Format("{0}:{0}", columnEng), Type.Missing).EntireColumn;
                    rngToDelete.Delete(Type.Missing);
                    Marshal.ReleaseComObject(rngToDelete);
                }
            }

            // Summary -- Content
            int row = 2;
            foreach (DataRow dr in this.pivotData.Rows)
            {
                row++;
                worksheet = excel.ActiveWorkbook.Worksheets[1];
                Microsoft.Office.Interop.Excel.Range rng = (Microsoft.Office.Interop.Excel.Range)worksheet.Rows[row, Type.Missing];
                rng.Insert(Microsoft.Office.Interop.Excel.XlDirection.xlDown);
                worksheet.Cells[row, 1] = MyUtility.Convert.GetString(dr["MDivisionID"]);
                worksheet.Cells[row, 2] = MyUtility.Convert.GetString(dr["FactoryID"]);
                worksheet.Cells[row, 3] = string.Format("={0}!E3", MyUtility.Convert.GetString(dr["FactoryID"]));
                worksheet.Cells[row, 4] = string.Format("={0}!E4", MyUtility.Convert.GetString(dr["FactoryID"]));
                for (int i = 0; i < this.reasonData.Rows.Count; i++)
                {
                    worksheet.Cells[row, i + 5] = MyUtility.Convert.GetDecimal(dr[i + 2]);
                }

                worksheet.Cells[row, 5 + this.reasonData.Rows.Count] = string.Format("=SUM({0}!J7:J1048576)", MyUtility.Convert.GetString(dr["FactoryID"]));
                worksheet.Cells[row, 6 + this.reasonData.Rows.Count] = string.Format("=SUM({0}!K7:K1048576)", MyUtility.Convert.GetString(dr["FactoryID"]));
                if (this.reportType == 0)
                {
                worksheet.Cells[row, 7 + this.reasonData.Rows.Count] = string.Format("={0}!O3", MyUtility.Convert.GetString(dr["FactoryID"]));
                worksheet.Cells[row, 8 + this.reasonData.Rows.Count] = string.Format("=D{0}/C{0}", row);
                }
                else
                {
                    worksheet.Cells[row, 7 + this.reasonData.Rows.Count] = string.Format("=D{0}/C{0}", row);
                }

                Marshal.ReleaseComObject(rng);
            }

            // 刪除Summary多出來的資料行
            ((Microsoft.Office.Interop.Excel.Range)worksheet.Rows[2, Type.Missing]).Delete(Microsoft.Office.Interop.Excel.XlDirection.xlUp);

            int count = 4;
            foreach (DataRow dr in this.reasonData.Rows)
            {
                count++;
                worksheet.Cells[1, count] = MyUtility.Convert.GetString(dr["Description"]);
            }

            for (int i = 3; i <= this.reasonData.Rows.Count + (this.reportType == 0 ? 7 : 6); i++)
            {
                worksheet.Cells[row, i] = string.Format("=SUM({0}2:{0}{1})", PublicPrg.Prgs.GetExcelEnglishColumnName(i), row - 1);
            }

            worksheet.Cells[row, (this.reportType == 0 ? 8 : 7) + this.reasonData.Rows.Count] = string.Format("=D{0}/C{0}", row);
            this.HideWaitMessage();

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName(this.reportType == 0 ? "PPIC_R04_FabricBCS" : "PPIC_R04_AccessoryBCS");
            Microsoft.Office.Interop.Excel.Workbook workbook = excel.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(worksheet);
            Marshal.ReleaseComObject(workbook);

            strExcelName.OpenFile();
            #endregion
            return true;
        }
    }
}
