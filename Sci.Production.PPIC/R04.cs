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

namespace Sci.Production.PPIC
{
    public partial class R04 : Sci.Win.Tems.PrintForm
    {
        DateTime? date1, date2;
        string mDivision, factory, pivotContent;
        int reportType,totalFactory;
        DataTable printData,reasonData,pivotData;
        public R04(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();

            MyUtility.Tool.SetupCombox(comboBox1,1, 1, "Fabric,Accessory");
            DataTable mDivision, factory;
            DBProxy.Current.Select(null, "select '' as ID union all select ID from MDivision", out mDivision);
            MyUtility.Tool.SetupCombox(comboBox2, 1, mDivision);
            DBProxy.Current.Select(null, "select '' as ID union all select distinct FtyGroup from Factory", out factory);
            MyUtility.Tool.SetupCombox(comboBox3, 1, factory);
            comboBox1.SelectedIndex = 0;
            comboBox2.Text = Sci.Env.User.Keyword;
            comboBox3.SelectedIndex = -1;

            dateRange1.Value1 = DateTime.Today.AddDays(-1);
            dateRange1.Value2 = DateTime.Today.AddDays(-1);
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            if (comboBox1.SelectedIndex == -1)
            {
                MyUtility.Msg.WarningBox("Report Type can't empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(dateRange1.Value1))
            {
                MyUtility.Msg.WarningBox("Apv. Date can't empty!!");
                return false;
            }
            
            date1 = dateRange1.Value1;
            date2 = dateRange1.Value2;
            reportType = comboBox1.SelectedIndex;
            mDivision = comboBox2.Text;
            factory = comboBox3.Text;

            return base.ValidateInput();
        }

        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            StringBuilder sqlCondition = new StringBuilder();
            sqlCondition.Append(string.Format(@"where l.FabricType = '{0}'
and l.ApvDate between '{1}' and '{2}'", (reportType ==0 ?"F":"A"), Convert.ToDateTime(date1).ToString("d"), Convert.ToDateTime(date2).ToString("d")));
            if (!MyUtility.Check.Empty(mDivision))
            {
                sqlCondition.Append(string.Format(" and l.MDivisionID = '{0}'", mDivision));
            }
            if (!MyUtility.Check.Empty(factory))
            {
                sqlCondition.Append(string.Format(" and l.FactoryID = '{0}'", factory));
            }

            sqlCmd.Append(string.Format(@"select l.MDivisionID,l.FactoryID,l.ID,l.SewingLineID,isnull(s.SewingCell,'') as SewingCell,
isnull(o.StyleID,'') as StyleID,l.OrderID,ld.Seq1+' '+ld.Seq2 as Seq,isnull(c.Name,'') as ColorName,
isnull(psd.Refno,'') as Refno,l.ApvDate,ld.RejectQty,ld.RequestQty,ld.IssueQty,
IIF(l.Status= 'Received',l.EditDate,null) as FinishedDate,IIF(l.Type='R','Replacement','Lacking') as Type,
isnull(IIF(l.FabricType = 'F',pr.Description,pr1.Description),'') as Description,
IIF(l.Status = 'Received',IIF(DATEDIFF(ss,l.ApvDate,l.EditDate) <= 10800,'Y','N'),'N') as OnTime
from Lack l
inner join Lack_Detail ld on l.ID = ld.ID
left join SewingLine s on s.ID = l.SewingLineID
left join Orders o on o.ID = l.OrderID
left join PO_Supp_Detail psd on psd.ID = l.POID and psd.SEQ1 = ld.Seq1 and psd.SEQ2 = ld.Seq2
left join Color c on c.BrandId = o.BrandID and c.ID = psd.ColorID
left join PPICReason pr on pr.Type = 'FL' and ld.PPICReasonID = pr.ID
left join PPICReason pr1 on pr1.Type = 'AL' and ld.PPICReasonID = pr1.ID
{0}
order by l.MDivisionID,l.FactoryID,l.ID", sqlCondition.ToString()));

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }
            
            //有資料的話才繼續撈資料
            if (printData.Rows.Count > 0)
            {
                sqlCmd.Clear();
                sqlCmd.Append(string.Format(@"select distinct ld.PPICReasonID,
isnull(IIF(l.FabricType = 'F',pr.Description,pr1.Description),'') as Description
from Lack l
inner join Lack_Detail ld on l.ID = ld.ID
left join PPICReason pr on pr.Type = 'FL' and ld.PPICReasonID = pr.ID
left join PPICReason pr1 on pr1.Type = 'AL' and ld.PPICReasonID = pr1.ID
{0}
order by PPICReasonID,Description", sqlCondition.ToString()));
                result = DBProxy.Current.Select(null, sqlCmd.ToString(), out reasonData);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Query all reason fail\r\n" + result.ToString());
                    return failResult;
                }

                pivotContent = MyUtility.GetValue.Lookup(string.Format(@"select '['+Description+']'+','
from (
select distinct ld.PPICReasonID+
isnull(IIF(l.FabricType = 'F',pr.Description,pr1.Description),'') as Description
from Lack l
inner join Lack_Detail ld on l.ID = ld.ID
left join PPICReason pr on pr.Type = 'FL' and ld.PPICReasonID = pr.ID
left join PPICReason pr1 on pr1.Type = 'AL' and ld.PPICReasonID = pr1.ID
{0}) a
order by Description
for xml path('')", sqlCondition.ToString()));

                sqlCmd.Clear();
                sqlCmd.Append(string.Format(@"with tmpData
as (
select l.MDivisionID,l.FactoryID,ld.PPICReasonID+
isnull(IIF(l.FabricType = 'F',pr.Description,pr1.Description),'') as Description,
IIF(l.FabricType = 'F',sum(ld.RejectQty),sum(ld.RequestQty)) as RequestQty
from Lack l
inner join Lack_Detail ld on l.ID = ld.ID
left join PPICReason pr on pr.Type = 'FL' and ld.PPICReasonID = pr.ID
left join PPICReason pr1 on pr1.Type = 'AL' and ld.PPICReasonID = pr1.ID
{0}
group by l.MDivisionID,l.FactoryID,ld.PPICReasonID+isnull(IIF(l.FabricType = 'F',pr.Description,pr1.Description),''),l.FabricType)
select *
from tmpData
PIVOT (SUM(RequestQty)
FOR Description IN ({1})
) a
order by MDivisionID,FactoryID", sqlCondition.ToString(), pivotContent.Substring(0, pivotContent.Length - 1)));
                result = DBProxy.Current.Select(null, sqlCmd.ToString(), out pivotData);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Query pivot data fail\r\n" + result.ToString());
                    return failResult;
                }

                totalFactory = MyUtility.Convert.GetInt(MyUtility.GetValue.Lookup(string.Format("select COUNT(distinct FactoryID) from Lack l {0}", sqlCondition.ToString())));
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

            MyUtility.Msg.WaitWindows("Starting EXCEL...");
            string strXltName = Sci.Env.Cfg.XltPathDir + (reportType == 0 ? "PPIC_R04_FabricBCS.xltx" : "PPIC_R04_AccessoryBCS.xltx");
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null) return false;
            excel.DisplayAlerts = false;
            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            //Summary -- Header
            if (reasonData.Rows.Count > 8)
            {
                //新增填Raseon欄位
                for (int i = 9; i <= reasonData.Rows.Count; i++)
                {
                    Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range("L:L", Type.Missing).EntireColumn;
                    rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                }
            }
            else if (reasonData.Rows.Count < 8)
            {
                string columnEng = PublicPrg.Prgs.GetExcelEnglishColumnName(reasonData.Rows.Count + 5);
                //刪除多的Raseon欄位
                for (int i = 1; i <= 8 - reasonData.Rows.Count; i++)
                {
                    Microsoft.Office.Interop.Excel.Range rngToDelete = worksheet.get_Range(string.Format("{0}:{0}", columnEng), Type.Missing).EntireColumn;
                    rngToDelete.Delete(Type.Missing);
                }
            }
            int count = 4;
            foreach (DataRow dr in reasonData.Rows)
            {
                count++;
                worksheet.Cells[1, count] = MyUtility.Convert.GetString(dr["Description"]);
                worksheet.Cells[11, count] = string.Format("=SUM({0}2:{0}10)",PublicPrg.Prgs.GetExcelEnglishColumnName(count));
            }

            //Summary -- Content
            int row = 1;
            foreach (DataRow dr in pivotData.Rows)
            {
                row++;
                worksheet.Cells[row, 1] = MyUtility.Convert.GetString(dr["MDivisionID"]);
                worksheet.Cells[row, 2] = MyUtility.Convert.GetString(dr["FactoryID"]);
                for (int i = 0; i < reasonData.Rows.Count; i++)
                {
                    worksheet.Cells[row, i+5] = MyUtility.Convert.GetDecimal(dr[i+2]);
                }
            }

            //刪除Summary多出來的資料行
            for (int i = pivotData.Rows.Count+2; i <= 10; i++)
            {
                Microsoft.Office.Interop.Excel.Range rng = (Microsoft.Office.Interop.Excel.Range)excel.Rows[pivotData.Rows.Count + 2, Type.Missing];
                rng.Select();
                //rng.Delete(Type.Missing);
                rng.Delete(Microsoft.Office.Interop.Excel.XlDirection.xlUp);
            }

            //填各工廠的明細資料
            string xlsFactory = "";
            int xlsSheet = 1, ttlCount = 0, intRowsStart = 7;
            object[,] objArray = new object[1, 16];
            foreach (DataRow dr in printData.Rows)
            {
                if (MyUtility.Convert.GetString(dr["FactoryID"]) != xlsFactory)
                {
                    if (xlsSheet != 1)
                    {
                        worksheet.Cells[3, 5] = string.Format("=COUNTA(C7:C{0})", MyUtility.Convert.GetString(ttlCount + 6));
                        if (reportType == 0)
                        {
                            worksheet.Cells[4, 5] = string.Format("=COUNTIF(P7:P{0},\"=Y\")", MyUtility.Convert.GetString(ttlCount + 6));
                            worksheet.Cells[3, 16] = string.Format("=SUM(L7:L{0})", MyUtility.Convert.GetString(ttlCount + 6));
                        }
                        else
                        {
                            worksheet.Cells[4, 5] = string.Format("=COUNTIF(O7:O{0},\"=Y\")", MyUtility.Convert.GetString(ttlCount + 6));
                            worksheet.Cells[3, 15] = string.Format("=SUM(J7:J{0})", MyUtility.Convert.GetString(ttlCount + 6));
                        }
                    }

                    xlsSheet++;
                    worksheet = excel.ActiveWorkbook.Worksheets[xlsSheet];
                    worksheet.Name = MyUtility.Convert.GetString(dr["FactoryID"]);
                    worksheet.Cells[4, 7] = string.Format("{0} ~ {1}",Convert.ToDateTime(date1).ToString("d"),Convert.ToDateTime(date2).ToString("d"));
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
                if (reportType == 0)
                {
                    objArray[0, 9] = dr["RejectQty"];
                    objArray[0, 10] = dr["RequestQty"];
                    objArray[0, 11] = dr["IssueQty"];
                    objArray[0, 12] = dr["FinishedDate"];
                    objArray[0, 13] = dr["Type"];
                    objArray[0, 14] = dr["Description"];
                    objArray[0, 15] = dr["OnTime"];
                }
                else
                {
                    objArray[0, 9] = dr["RequestQty"];
                    objArray[0, 10] = dr["IssueQty"];
                    objArray[0, 11] = dr["FinishedDate"];
                    objArray[0, 12] = dr["Type"];
                    objArray[0, 13] = dr["Description"];
                    objArray[0, 14] = dr["OnTime"];
                    objArray[0, 15] = "";
                }
                worksheet.Range[String.Format("A{0}:P{0}", intRowsStart)].Value2 = objArray;
                intRowsStart++;
            }

            worksheet.Cells[3, 5] = string.Format("=COUNTA(C7:C{0})", MyUtility.Convert.GetString(ttlCount + 6));
            if (reportType == 0)
            {
                worksheet.Cells[4, 5] = string.Format("=COUNTIF(P7:P{0},\"=Y\")", MyUtility.Convert.GetString(ttlCount + 6));
                worksheet.Cells[3, 16] = string.Format("=SUM(L7:L{0})", MyUtility.Convert.GetString(ttlCount + 6));
            }
            else
            {
                worksheet.Cells[4, 5] = string.Format("=COUNTIF(O7:O{0},\"=Y\")", MyUtility.Convert.GetString(ttlCount + 6));
                worksheet.Cells[3, 15] = string.Format("=SUM(J7:J{0})", MyUtility.Convert.GetString(ttlCount + 6));
            }
            excel.Visible = true;
            for (int i = xlsSheet+1; i <= 10; i++)
            {
                worksheet = excel.ActiveWorkbook.Worksheets[xlsSheet + 1];
                worksheet.Delete();
            }
            MyUtility.Msg.WaitClear();
            excel.Visible = true;
            return true;
        }
    }
}
