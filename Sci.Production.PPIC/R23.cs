using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System;
using Sci.Utility.Excel;
using System.Linq;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// R23
    /// </summary>
    public partial class R23 : Win.Tems.PrintForm
    {
        private string sqlGetData;
        private DataTable dtPrint;
        private DataSet dataSet;
        private List<SqlParameter> listPar = new List<SqlParameter>();

        /// <summary>
        /// Initializes a new instance of the <see cref="R23"/> class.
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public R23(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// ValidateInput
        /// </summary>
        /// <returns>true</returns>
        protected override bool ValidateInput()
        {
            string sqlWhere = string.Empty;
            string sqlWhereResponsibilityDept = string.Empty;
            this.listPar = new List<SqlParameter>();
            this.sqlGetData = string.Empty;

            if (string.IsNullOrEmpty(this.dateRangeSewDate.DateBox1.Value.ToString()) || string.IsNullOrEmpty(this.dateRangeSewDate.DateBox2.Value.ToString()))
            {
                MyUtility.Msg.ErrorBox("<Sewing Date> can't be Empty!");
                return false;
            }

            if (string.IsNullOrEmpty(this.txtSP1.Text) || string.IsNullOrEmpty(this.txtSP1.Text))
            {
                MyUtility.Msg.ErrorBox("<SP#> can't be Empty!");
                return false;
            }

            if (this.dateRangeSewDate.HasValue1)
            {
                sqlWhere += " and (convert(date,s.Inline)  >= @SewDateFrom or (@SewDateFrom between convert(date,s.Inline) and convert(date,s.Offline)))";
                this.listPar.Add(new SqlParameter("@SewDateFrom", this.dateRangeSewDate.DateBox1.Text));
            }

            if (this.dateRangeSewDate.HasValue2)
            {
                sqlWhere += " and (convert(date,s.Offline) <= @SewDateTo or (@SewDateTo between convert(date,s.Inline) and convert(date,s.Offline))) ";
                this.listPar.Add(new SqlParameter("@SewDateTo", this.dateRangeSewDate.DateBox2.Text));
            }

            if (this.dateRangeBDate.HasValue1)
            {
                sqlWhere += " and Orders.BuyerDelivery >= @BDateFrom ";
                this.listPar.Add(new SqlParameter("@BDateFrom", this.dateRangeBDate.DateBox1.Text));
            }

            if (this.dateRangeBDate.HasValue2)
            {
                sqlWhere += " and Orders.BuyerDelivery <= @BDateTo ";
                this.listPar.Add(new SqlParameter("@BDateTo", this.dateRangeBDate.DateBox2.Text));
            }

            if (!string.IsNullOrEmpty(this.txtSP1.Text))
            {
                sqlWhere += " and Orders.ID >= @ID ";
                this.listPar.Add(new SqlParameter("@ID", this.txtSP1.Text));
            }

            if (!string.IsNullOrEmpty(this.txtSP2.Text))
            {
                sqlWhere += " and Orders.ID <= @ID2 ";
                this.listPar.Add(new SqlParameter("@ID2", this.txtSP2.Text));
            }

            if (!string.IsNullOrEmpty(this.txtstyle.Text))
            {
                sqlWhere += " and Orders.StyleID <= @styleID ";
                this.listPar.Add(new SqlParameter("@styleID", this.txtstyle.Text));
            }

            this.sqlGetData = $@"
Select distinct
s.SewingLineID
, Orders.OrderComboID
, Orders.POID
, Orders.ID
, Orders.Styleid
, oc.ColorID
, ColorName = c.[Name]
, Order_Article.Article
, Order_SizeCode.SizeCode
, IsNull(Order_Qty.Qty, 0) Qty
, Orders.BrandID
, Orders.CustCDID
, Orders.CustPONo
, Order_SizeCode.Seq
From SewingSchedule s
inner join dbo.Orders WITH (NOLOCK) on s.OrderID = Orders.ID
Left Join dbo.Order_SizeCode WITH (NOLOCK) On  Order_SizeCode.ID = Orders.POID
Left Join dbo.Order_Article WITH (NOLOCK) On  Order_Article.ID = Orders.ID
inner join dbo.Order_ColorCombo oc on oc.ID = Orders.POID and oc.Article = Order_Article.Article and oc.PatternPanel = 'AA'
inner join dbo.Color c on c.ID = oc.ColorID and c.BrandId = Orders.BrandID
Left Join dbo.Order_Qty WITH (NOLOCK) On  Order_Qty.ID = Orders.ID And Order_Qty.SizeCode = Order_SizeCode.SizeCode And Order_Qty.Article = Order_Article.Article
Left Join dbo.CustCD WITH (NOLOCK) On CustCD.BrandID = Orders.BrandID And CustCD.ID = Orders.CustCDID
Left Join dbo.Factory WITH (NOLOCK) On Factory.ID = Orders.FactoryID
Where 1 = 1
{sqlWhere}

";
            return base.ValidateInput();
        }

        /// <summary>
        /// OnAsyncDataLoad
        /// </summary>
        /// <param name="e">e</param>
        /// <returns>true</returns>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            return DBProxy.Current.Select(null, this.sqlGetData, this.listPar, out this.dtPrint);
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            if (this.dtPrint == null || this.dtPrint.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found.");
                this.SetCount(0);
                return false;
            }

            this.SetCount(this.dtPrint.Rows.Count);
            this.ShowWaitMessage("Excel processing...");

            string excelFile = "\\PPIC_R23.xltx";

            Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + excelFile); // 開excelapp
            if (objApp == null)
            {
                return false;
            }

            Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];
            this.SetExcelSheet(this.dtPrint, objSheets);

            objApp.Visible = true;
            objApp.Rows.AutoFit();
            Marshal.ReleaseComObject(objApp);
            this.HideWaitMessage();

            return true;
        }

        private void SetExcelSheet(DataTable dt, Excel.Worksheet objSheets)
        {
            if (dt.Rows.Count <= 0)
            {
                return;
            }

            int initIdx = 0;

            Excel.Range excelrange;
            int stratcol, columnsCnt, rowsCnt;
            string today = DateTime.Now.ToString("MM/dd");

            foreach (string id in dt.AsEnumerable().Select(s => s["ID"].ToString()).Distinct())
            {
                var dtDetail = dt.AsEnumerable().Where(s => s["ID"].ToString() == id).OrderBy(o => o["Seq"]).ToList();
                columnsCnt = 4 + dtDetail.Count + 1;
                stratcol = 1 + initIdx;

                objSheets.Cells[1 + initIdx, 1] = "LINE";
                objSheets.Cells[1 + initIdx, 1].Font.Bold = true;
                objSheets.Cells[1 + initIdx, 2] = "ITEM";
                objSheets.Cells[1 + initIdx, 3] = "COLOR";
                objSheets.Cells[1 + initIdx, 4] = "COLOR NAME";

                for (int i = 2; i <= 4;)
                {
                    objSheets.Cells[1 + initIdx, i].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LavenderBlush);
                    objSheets.Cells[1 + initIdx, i].Font.Bold = true;
                    i++;
                }

                objSheets.Cells[2 + initIdx, 1] = dtDetail[0]["SewingLineID"];
                objSheets.Cells[2 + initIdx, 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Pink);
                objSheets.Cells[2 + initIdx, 2] = dtDetail[0]["Article"];
                objSheets.Cells[2 + initIdx, 3] = dtDetail[0]["ColorID"];
                objSheets.Cells[2 + initIdx, 3].Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                objSheets.Cells[2 + initIdx, 4] = dtDetail[0]["ColorName"];
                objSheets.Cells[3 + initIdx, 1] = dtDetail[0]["ID"];
                objSheets.Cells[3 + initIdx, 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Pink);

                for (int i = 2; i <= 4;)
                {
                    objSheets.Cells[2 + initIdx, i].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightBlue);
                    i++;
                }

                for (int i = 0; i < dtDetail.Count;)
                {
                    DataRow row = dtDetail[i];
                    objSheets.Cells[1 + initIdx, i + 5] = row["SizeCode"];
                    objSheets.Cells[1 + initIdx, i + 5].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Lavender);
                    objSheets.Cells[1 + initIdx, i + 5].Font.Bold = true;
                    double qty;
                    double.TryParse(row["Qty"].ToString(), out qty);
                    objSheets.Cells[2 + initIdx, i + 5] = qty;
                    i++;
                }

                objSheets.Cells[1 + initIdx, 5 + dtDetail.Count] = "Total Cons";
                objSheets.Cells[1 + initIdx, 5 + dtDetail.Count].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Lavender);
                objSheets.Cells[1 + initIdx, 5 + dtDetail.Count].Font.Bold = true;
                char chars = (char)('D' + dtDetail.Count);
                int totalC = 2 + initIdx;
                objSheets.Cells[2 + initIdx, 5 + dtDetail.Count].Value = string.Format("=SUM({0}{1}:{2}{1})", "E", totalC, chars);

                string sql = @"
If Object_ID('tempdb..#Tmp_Order_Qty') Is Null
	Begin
		Select Orders.ID ,Article ,SizeCode ,Order_Qty.Qty, OriQty Into #Tmp_Order_Qty
		  From dbo.Order_Qty
		 Inner Join dbo.Orders
			On Orders.ID = Order_Qty.ID
		 Where Orders.PoID = @POID;
	End;

	declare @Tmp_Order_Qty dbo.QtyBreakdown
	insert into @Tmp_Order_Qty
	select ID ,Article ,SizeCode ,Qty
        --,SewOutputQty ,SewOutputUpdate--pms does not use this column
        ,OriQty
    from #Tmp_Order_Qty

Select * from GetBOAExpend_NEW(@POID, 0, 1, 0, @Tmp_Order_Qty, 0, 0, 1)

where Article = @Art and ColorID = @Color
and @ID in (Select Data From dbo.SplitString(OrderList,','))
";
                DataTable tmp;
                List<SqlParameter> paras = new List<SqlParameter>();
                paras.Add(new SqlParameter("@POID", dtDetail[0]["POID"].ToString()));
                paras.Add(new SqlParameter("@ID", dtDetail[0]["ID"].ToString()));
                paras.Add(new SqlParameter("@Art", dtDetail[0]["Article"].ToString()));
                paras.Add(new SqlParameter("@Color", dtDetail[0]["ColorID"].ToString()));
                var result = DBProxy.Current.Select(null, sql, paras, out tmp);

                var groupedByRefNo = tmp.AsEnumerable().Select(x => x["RefNo"].ToString())
                .Distinct()
                .ToList();

                foreach (var group in groupedByRefNo)
                {
                    var tmpRef = tmp.AsEnumerable().Where(s => s["RefNo"].ToString() == group.ToString()).ToList();

                    objSheets.Cells[3 + initIdx, 2] = group.ToString();
                    objSheets.Range[objSheets.Cells[3 + initIdx, 2], objSheets.Cells[4 + initIdx, 2]].Merge();
                    objSheets.Cells[3 + initIdx, 4] = "Cons/PC";

                    if (tmpRef.Count == dtDetail.Count)
                    {
                        for (int i = 0; i < tmpRef.Count;)
                        {
                            DataRow row = tmpRef[i];
                            string tmpSizeCode = row["SizeCode"].ToString();
                            DataRow matchedRows = dtDetail.AsEnumerable().FirstOrDefault(r => r["SizeCode"].ToString() == tmpSizeCode);

                            double usageQty, orderQty, scQty;
                            double.TryParse(row["UsageQty"]?.ToString(), out usageQty);
                            double.TryParse(row["OrderQty"]?.ToString(), out orderQty);
                            double.TryParse(matchedRows["Qty"]?.ToString(), out scQty);
                            double qty = Math.Round(usageQty / orderQty, 3);

                            objSheets.Cells[3 + initIdx, i + 5] = qty;
                            objSheets.Cells[4 + initIdx, i + 5] = scQty * qty;
                            objSheets.Cells[4 + initIdx, i + 5].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Plum);
                            i++;
                        }
                    }
                    else if (tmpRef.Count == 1)
                    {
                        for (int i = 0; i < dtDetail.Count;)
                        {
                            DataRow matchedRows = dtDetail[i];
                            DataRow row = tmpRef[0];
                            double usageQty, orderQty, scQty;
                            double.TryParse(row["UsageQty"].ToString(), out usageQty);
                            double.TryParse(row["OrderQty"].ToString(), out orderQty);
                            double.TryParse(matchedRows["Qty"]?.ToString(), out scQty);
                            double qty = Math.Round(usageQty / orderQty, 3);

                            objSheets.Cells[3 + initIdx, i + 5] = qty;
                            objSheets.Cells[4 + initIdx, i + 5] = scQty * qty;
                            objSheets.Cells[4 + initIdx, i + 5].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Plum);
                            i++;
                        }
                    }

                    totalC = 4 + initIdx;
                    objSheets.Cells[4 + initIdx, 5 + dtDetail.Count].Value = string.Format("=SUM({0}{1}:{2}{1})", "E", totalC, chars);

                    initIdx = initIdx + 3;
                }

                objSheets.Range[$"A:{MyExcelPrg.GetExcelColumnName(columnsCnt)}"].Columns.AutoFit();

                objSheets.Range[objSheets.Cells[2 + stratcol, 3], objSheets.Cells[initIdx + 1 , 3]].Merge();

                excelrange = objSheets.get_Range(string.Format("A{1}:{0}{2}", MyExcelPrg.GetExcelColumnName(columnsCnt), stratcol, MyUtility.Convert.GetString(initIdx + 1)), Type.Missing);
                excelrange.Borders[Excel.XlBordersIndex.xlEdgeTop].Weight = Excel.XlBorderWeight.xlThin;
                excelrange.Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlThin;
                excelrange.Borders[Excel.XlBordersIndex.xlEdgeLeft].Weight = Excel.XlBorderWeight.xlThin;
                excelrange.Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = Excel.XlBorderWeight.xlThin;
                excelrange.Borders[Excel.XlBordersIndex.xlInsideHorizontal].Weight = Excel.XlBorderWeight.xlThin;
                excelrange.Borders[Excel.XlBordersIndex.xlInsideVertical].Weight = Excel.XlBorderWeight.xlThin;

                initIdx = initIdx + 2;
            }

            // objSheets.Range[string.Format("A{0}:C{0}", 1 + initIdx)].Merge();
            //objSheets.Cells[2 + initIdx, 1] = "Kpi_Fail";
            //objSheets.Cells[2 + initIdx, 2] = "Brand";
            //objSheets.Cells[2 + initIdx, 3] = "SMR";

            //rowsCnt = dt.Rows.Count;
            //columnsCnt = dt.Columns.Count;
            //objSheets.Cells[3 + initIdx, 1] = dt.Rows[0]["KPIFailed"].ToString();
            //objSheets.Range[string.Format("A{0}:A{1}", 3 + initIdx, rowsCnt + 2 + initIdx)].Merge();
            //excelrange = objSheets.GetRanges(string.Format("D{0}:{1}{0}", 1 + initIdx, MyExcelPrg.GetExcelColumnName(columnsCnt + 1)));
            //excelrange.Merge();
            //excelrange.HorizontalAlignment = Excel.Constants.xlCenter; // 水平置中對齊
            //excelrange.VerticalAlignment = Excel.Constants.xlCenter; // 垂直置中對齊
            //excelrange.WrapText = true; // 自動換列
            //objSheets.Cells[1 + initIdx, 4] = $"=\"SCI Del-Today {today} checking away from sci del.\"&CHAR(10)&\"({today} checking 距離 SCI 剩下天數)\"";
            //objSheets.Cells[1 + initIdx, 4].RowHeight = 30;

            //for (int i = 0; i <= columnsCnt - 4; i++)
            //{
            //    objSheets.Cells[2 + initIdx, i + 4] = dt.Columns[i + 3].ToString();
            //    objSheets.Cells[rowsCnt + 3 + initIdx, i + 4] = string.Format("=Sum({0}{1}:{0}{2})", MyExcelPrg.GetExcelColumnName(i + 4), 3 + initIdx, rowsCnt + 2 + initIdx);
            //}

            //objSheets.Cells[2 + initIdx, columnsCnt + 1] = "Grand Total";

            //object[,] objArray = new object[1, columnsCnt - 1];
            //for (int i = 0; i <= rowsCnt - 1; i++)
            //{
            //    DataRow dr = dt.Rows[i];
            //    for (int j = 0; j <= columnsCnt - 2; j++)
            //    {
            //        objArray[0, j] = dr[j + 1];
            //    }

            //    objSheets.Range[string.Format("B{0}:{1}{0}", i + 3 + initIdx, MyExcelPrg.GetExcelColumnName(columnsCnt))].Value2 = objArray;
            //    objSheets.Range[string.Format("{1}{0}", i + 3 + initIdx, MyExcelPrg.GetExcelColumnName(columnsCnt + 1))].Value = string.Format("=Sum(D{0}:{1}{0})", i + 3 + initIdx, MyExcelPrg.GetExcelColumnName(columnsCnt));
            //}

            //objSheets.Cells[rowsCnt + 3 + initIdx, 1] = "Already failed Total";
            //objSheets.Range[string.Format("A{0}:C{0}", rowsCnt + 3 + initIdx)].Merge();
            //objSheets.Cells[rowsCnt + 3 + initIdx, columnsCnt + 1] = string.Format("=Sum(D{0}:{1}{0})", rowsCnt + 3 + initIdx, MyExcelPrg.GetExcelColumnName(columnsCnt));
            //objSheets.Range["A:C"].Columns.AutoFit();
            //objSheets.Range[string.Format("{0}:{1}", MyExcelPrg.GetExcelColumnName(columnsCnt + 1), MyExcelPrg.GetExcelColumnName(columnsCnt + 2))].Columns.AutoFit();

            //excelrange = objSheets.get_Range(string.Format("A{1}:{0}{2}", MyExcelPrg.GetExcelColumnName(columnsCnt + 1), 1 + initIdx, MyUtility.Convert.GetString(rowsCnt + 3 + initIdx)), Type.Missing);
            //excelrange.Borders[Excel.XlBordersIndex.xlEdgeTop].Weight = Excel.XlBorderWeight.xlThin;
            //excelrange.Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlThin;
            //excelrange.Borders[Excel.XlBordersIndex.xlEdgeLeft].Weight = Excel.XlBorderWeight.xlThin;
            //excelrange.Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = Excel.XlBorderWeight.xlThin;
            //excelrange.Borders[Excel.XlBordersIndex.xlInsideHorizontal].Weight = Excel.XlBorderWeight.xlThin;
            //excelrange.Borders[Excel.XlBordersIndex.xlInsideVertical].Weight = Excel.XlBorderWeight.xlThin;

            //objSheets.Range["A:C"].VerticalAlignment = Excel.Constants.xlCenter;
        }
    }
}
