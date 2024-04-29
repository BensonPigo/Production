using Ict;
using Sci.Data;
using Sci.Utility.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Quality
{
    /// <inheritdoc/>
    public partial class R16 : Win.Tems.PrintForm
    {
        private DataTable[] printData;
        private string Excelfile;
        private string sp1;
        private string sp2;
        private string inspector;
        private string brand;
        private string refno1;
        private string refno2;
        private DateTime? InspectionDate1;
        private DateTime? InspectionDate2;
        private Color green = Color.FromArgb(153, 204, 0);
        private Color blue = Color.FromArgb(101, 215, 255);
        private Color zero = Color.FromArgb(250, 191, 143);

        /// <summary>
        /// Initializes a new instance of the <see cref="R16"/> class.
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public R16(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.txtbrand.MultiSelect = true;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.InspectionDate1 = this.dateInspectionDate.Value1;
            this.InspectionDate2 = this.dateInspectionDate.Value2;
            this.inspector = this.txtmulituser.TextBox1.Text;
            this.sp1 = this.txtSPStart.Text;
            this.sp2 = this.txtSPEnd.Text;
            this.brand = this.txtbrand.Text;
            this.refno1 = this.txtRefno1.Text;
            this.refno2 = this.txtRefno2.Text;
            if (MyUtility.Check.Empty(this.InspectionDate1) || MyUtility.Check.Empty(this.InspectionDate2))
            {
                MyUtility.Msg.WarningBox("< Inspected Date > cannot be empty!");
                return false;
            }

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            #region 畫面上的條件
            string sqlWhere = string.Empty;

            List<SqlParameter> listPar = new List<SqlParameter>();

            sqlWhere += $@" a.InspDate  BETWEEN @InspectionDateFrom and @InspectionDateTo ";
            listPar.Add(new SqlParameter("@InspectionDateFrom", this.InspectionDate1));
            listPar.Add(new SqlParameter("@InspectionDateTo", this.InspectionDate2));

            if (!MyUtility.Check.Empty(this.inspector))
            {
                sqlWhere += $@" and a.Inspector = @Inspectors ";
                listPar.Add(new SqlParameter("@Inspectors", this.inspector));
            }

            if (!MyUtility.Check.Empty(this.sp1))
            {
                sqlWhere += $@" and (a.POID >= @POIDFrom or @POIDFrom = '') ";
                listPar.Add(new SqlParameter("@POIDFrom", this.sp1));
            }

            if (!MyUtility.Check.Empty(this.sp2))
            {
                sqlWhere += $@" and (a.POID <= @POIDTo or @POIDTo = '') ";
                listPar.Add(new SqlParameter("@POIDTo", this.sp2));
            }

            if (!MyUtility.Check.Empty(this.refno1))
            {
                sqlWhere += $@" and (c.RefNo  >= @RefNoFrom or @RefNoFrom = '') ";
                listPar.Add(new SqlParameter("@RefNoFrom", this.refno1));
            }

            if (!MyUtility.Check.Empty(this.sp2))
            {
                sqlWhere += $@" and  (c.RefNo <= @RefNoTo or @RefNoTo = '') ";
                listPar.Add(new SqlParameter("@RefNoTo", this.refno2));
            }

            if (!MyUtility.Check.Empty(this.brand))
            {
                sqlWhere += $@" and o.BrandID IN ('{this.brand.Split(',').JoinToString("','")}') ";
                //listPar.Add(new SqlParameter("@BrandIDs", this.brand));
            }

            #endregion
            #region 主Sql
            StringBuilder sqlCmd = new StringBuilder();

            if (this.radioDetail.Checked)
            {
                sqlCmd.Append($@"
SELECT   [InspectedDate] = a.InspDate
	,[InspectorID] =a.Inspector
	,[InspectorName] = isnull(Pass1.Name, '')
	,[BrandID] = isnull(o.brandID  , '')
	,[FtyGroup] = isnull(o.FtyGroup , '')
	,[StyleID] = isnull(o.StyleID, '')
	,[SP#] =a.POID
	,[SEQ] = concat(RTRIM(a.SEQ1) ,'-',a.SEQ2)
	,[StockType]=iif(isnull(b. StockType, '') = '', '', (select Name from DropDownList ddl where ddl.id like '%'+b. StockType+'%' and ddl.Type = 'Pms_StockType'))
	,[Wkno] = isnull(b.ExportID, '')
	,a.SuppID
	,[SuppName]=(select AbbEN from Supp where id = a.SuppID)
	,[ATA] = b.WhseArrival
	,[Refno] = RTRIM(isnull(c.RefNo, ''))
	,[Color]= isnull(dbo.GetColorMultipleID(o.BrandID,isnull(d.SpecValue ,'')), '')
	,[ArrivedQty] = isnull(b.StockQty, 0)
	,[InspectQty] = a.InspQty
	,[RejectQty] =a.Rejectqty
	,[Result] = a.Result
	,[Remark]=a.Remark
	,[MCHandle]= isnull(dbo.getPass1_ExtNo(o.MCHandle), '')
FROM  AIR AS A
LEFT JOIN View_AllReceivingDetail b ON b.PoId= a.POID AND b.Seq1 = a.SEQ1 AND b.Seq2 = a.SEQ2 and b.id = a.ReceivingID
LEFT join PO_Supp_Detail c on c.ID = a.poid and c.seq1 = a.seq1 and c.seq2 = a.seq2
left join PO_Supp_Detail_Spec d WITH (NOLOCK) on d.ID = c.id and d.seq1 = c.seq1 and d.seq2 = c.seq2 and d.SpecColumnID = 'Color'
LEFT join Orders o on o.id=a.POID
LEFT JOIN Pass1 on Pass1.ID = a.Inspector
WHERE {sqlWhere}

ORDER BY a.InspDate, a.Inspector, a.POID, a.SEQ1, a.SEQ2

");
            }
            else
            {
                sqlCmd.Append($@"
SELECT   [InspectedDate] = a.InspDate
	,[InspectorID] =a.Inspector
	,[InspectorName] = isnull(Pass1.Name, '')
	,[BrandID] = isnull(o.brandID  , '')
	,[FtyGroup] = isnull(o.FtyGroup , '')
	,[StyleID] = isnull(o.StyleID, '')
	,[SP#] =a.POID
	,[SEQ] = concat(RTRIM(a.SEQ1) ,'-',a.SEQ2)
	,[StockType]=iif(isnull(b. StockType, '') = '', '', (select Name from DropDownList ddl where ddl.id like '%'+b. StockType+'%' and ddl.Type = 'Pms_StockType'))
	,[Wkno] = isnull(b.ExportID, '')
	,a.SuppID
	,[SuppName]=(select AbbEN from Supp where id = a.SuppID)
	,[ATA] = b.WhseArrival
	,[Refno] = RTRIM(isnull(c.RefNo, ''))
	,[Color]= isnull(dbo.GetColorMultipleID(o.BrandID,isnull(d.SpecValue ,'')), '')
	,[ArrivedQty] = isnull(b.StockQty, 0)
	,[InspectQty] = a.InspQty
	,[RejectQty] =a.Rejectqty
	,[Result] = a.Result
	,[Remark]=a.Remark
	,[MCHandle]= isnull(dbo.getPass1_ExtNo(o.MCHandle), '')
into #allAccDetail
FROM  AIR AS A
LEFT JOIN View_AllReceivingDetail b ON b.PoId= a.POID AND b.Seq1 = a.SEQ1 AND b.Seq2 = a.SEQ2 and b.id = a.ReceivingID
LEFT join PO_Supp_Detail c on c.ID = a.poid and c.seq1 = a.seq1 and c.seq2 = a.seq2
left join PO_Supp_Detail_Spec d WITH (NOLOCK) on d.ID = c.id and d.seq1 = c.seq1 and d.seq2 = c.seq2 and d.SpecColumnID = 'Color'
LEFT join Orders o on o.id=a.POID
LEFT JOIN Pass1 on Pass1.ID = a.Inspector
WHERE {sqlWhere}
 

----User Date
select InspectorID ,InspectorName, InspectedDate, InspectQty = sum(InspectQty)
from #allAccDetail
group by InspectorID ,InspectorName, InspectedDate
order by InspectorID ,InspectorName, InspectedDate

DROP TABLE #allAccDetail
");
            }

            #endregion
            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), listPar, out this.printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return Ict.Result.True;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.printData[0].Rows.Count);
            StringBuilder c = new StringBuilder();
            if (this.printData[0].Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            Excel.Application objApp = null;
            string xltx = string.Empty;

            try
            {
                if (this.radioDetail.Checked)
                {
                    xltx = "Quality_R16.xltx";
                    objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\" + xltx); // 預先開啟excel app
                    MyUtility.Excel.CopyToXls(this.printData[0], string.Empty, xltx, 1, true, null, objApp); // 將datatable copy to excel
                }
                else
                {
                    xltx = "Quality_R16_Summery.xltx";
                    objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\" + xltx); // 預先開啟excel app
                    Excel.Worksheet wksheet = objApp.Sheets[1];
//#if DEBUG
//                    objApp.Visible = true;
//#endif

                    List<string> inspectorList = this.printData[0].AsEnumerable().Select(o => o.Field<string>("InspectorID")).Distinct().OrderBy(o => o).ToList();
                    List<DateTime> dateList = this.printData[0].AsEnumerable().Select(o => o.Field<DateTime>("InspectedDate")).Distinct().OrderBy(o => o).ToList();

                    int colStart = 2;
                    int rowStart = 5;
                    int colEnd = colStart; // 3 (c
                    int rowEnd = rowStart; // 7

                    // Copy Column
                    for (int i = 0; i < inspectorList.Count - 2; i++)
                    {
                        Excel.Range r = wksheet.get_Range("C1", "C1").EntireColumn;
                        r.Copy();
                        r.Insert(Excel.XlInsertShiftDirection.xlShiftDown, Excel.XlInsertFormatOrigin.xlFormatFromRightOrBelow); // 新增Column
                        colEnd++;
                    }

                    // Copy Row
                    for (int i = 0; i < dateList.Count - 2; i++)
                    {
                        Excel.Range r = wksheet.get_Range("A6", "A6").EntireRow;
                        r.Copy();
                        r.Insert(Excel.XlInsertShiftDirection.xlShiftDown, Excel.XlInsertFormatOrigin.xlFormatFromRightOrBelow); // 新增Row
                        rowEnd++;
                    }

                    int userCtn = 0;
                    foreach (var inspector in inspectorList)
                    {
                        var byInspector = this.printData[0].AsEnumerable().Where(o => o.Field<string>("InspectorID") == inspector);

                        if (!byInspector.Any())
                        {
                            userCtn++;
                            continue;
                        }

                        wksheet.Cells[rowStart - 3, colStart + userCtn] = MyUtility.Convert.GetString(byInspector.FirstOrDefault()["InspectorID"]);
                        wksheet.Cells[rowStart - 2, colStart + userCtn] = MyUtility.Convert.GetString(byInspector.FirstOrDefault()["InspectorName"]);

                        int dateCtn = 0;
                        foreach (var date in dateList)
                        {
                            wksheet.Cells[rowStart + dateCtn, 1] = date;

                            var byInspectorByData = byInspector.Where(o => o.Field<DateTime>("InspectedDate") == date).ToList();

                            if (!byInspectorByData.Any())
                            {
                                wksheet.Cells[rowStart + dateCtn, colStart + userCtn] = 0;
                                dateCtn++;
                                continue;
                            }

                            wksheet.Cells[rowStart + dateCtn, colStart + userCtn] = MyUtility.Convert.GetFloat(byInspectorByData.FirstOrDefault()["InspectQty"]);
                            dateCtn++;
                        }

                        userCtn++;
                    }

                    objApp.Columns.AutoFit();
                    this.Excelfile = Class.MicrosoftFile.GetName("Quality_R16_Summery");
                    objApp.ActiveWorkbook.SaveAs(this.Excelfile);
                    objApp.Quit();
                    Marshal.ReleaseComObject(wksheet);
                    this.Excelfile.OpenFile();
                }
            }
            catch (Exception ex)
            {
                this.ShowErr(ex);
            }
            finally
            {
                Marshal.ReleaseComObject(objApp);
            }

            return true;
        }
    }
}
