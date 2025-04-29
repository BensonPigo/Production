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
                sqlWhere += @" and (convert(date,s.Inline)  >= @SewDateFrom 
                               or (@SewDateFrom between convert(date,s.Inline) and convert(date,s.Offline)))";
                this.listPar.Add(new SqlParameter("@SewDateFrom", this.dateRangeSewDate.DateBox1.Text));
            }

            if (this.dateRangeSewDate.HasValue2)
            {
                sqlWhere += @" and (convert(date,s.Offline) <= @SewDateTo 
                               or (@SewDateTo between convert(date,s.Inline) and convert(date,s.Offline))) ";
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
, Orders.POID
, Orders.ID
, Orders.FactoryID
, Orders.CustCDID
, CustCD.ZipperInsert
, Orders.CustPONo
, Orders.BuyMonth
, Factory.CountryID
, Orders.StyleID
, Order_Article.Article
, Order_SizeCode.Seq
, Order_SizeCode.SizeCode
, IsNull(Order_Qty.Qty, 0) Qty
, color.*
from SewingSchedule s
inner join dbo.Orders WITH (NOLOCK)  on Orders.id = orderid
Left Join dbo.Order_SizeCode WITH (NOLOCK) On  Order_SizeCode.ID = Orders.POID
Left Join dbo.Order_Article WITH (NOLOCK) On  Order_Article.ID = Orders.ID
Left Join dbo.Order_Qty WITH (NOLOCK) On  Order_Qty.ID = Orders.ID
And Order_Qty.SizeCode = Order_SizeCode.SizeCode
And Order_Qty.Article = Order_Article.Article
Left Join dbo.CustCD WITH (NOLOCK) On CustCD.BrandID = Orders.BrandID 
And CustCD.ID = Orders.CustCDID
Left Join dbo.Factory WITH (NOLOCK) On Factory.ID = Orders.FactoryID
Outer Apply ( Select ColorID = oc.ColorID, ColorName=  c.Name  
			  From Order_ColorCombo oc  
              Inner join Color c on c.ID = oc.ColorID
              Where oc.id =Orders.Poid and Article = Order_Article.Article and PatternPanel='FA' and c.BrandId = Orders.BrandID
) as color
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
                return false;
            }

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
            int stratcol, columnsCnt, rowsCnt, totalC;
            string today = DateTime.Now.ToString("MM/dd");

            var distinctRows = dt.AsEnumerable().OrderBy(o => o["ID"])
    .Select(s => new
    {
        SewingLineID = s["SewingLineID"].ToString(),
        POID = s["POID"].ToString(),
        ID = s["ID"].ToString(),
        StyleID = s["StyleID"].ToString(),
        Article = s["Article"].ToString(),
    })
    .Distinct()
    .ToList();

            foreach (var id in distinctRows)
            {
                var dtDetail = dt.AsEnumerable()
                                     .Where(s => s["SewingLineID"].ToString() == id.SewingLineID
                                              && s["POID"].ToString() == id.POID
                                              && s["ID"].ToString() == id.ID
                                              && s["StyleID"].ToString() == id.StyleID
                                              && s["Article"].ToString() == id.Article)
                                     .OrderBy(o => o["Seq"])
                                     .ToList();

                string sql = @"
    If Object_ID('tempdb..#Tmp_BoaExpend') Is Null
         Begin
	        Create Table #Tmp_BoaExpend
	        (  ExpendUkey BigInt Identity(1,1) Not Null, ID Varchar(13), Order_BOAUkey BigInt
				, RefNo VarChar(36), SCIRefNo VarChar(30), Article VarChar(8), ColorID VarChar(6), SuppColor NVarChar(Max)
				, SizeSeq VarChar(2), SizeCode VarChar(8), SizeSpec VarChar(15), SizeUnit VarChar(8), Remark NVarChar(Max)
				, OrderQty Numeric(6,0)
				--, Price Numeric(12,4)--pms does not use this column
				, UsageQty Numeric(11,2), UsageUnit VarChar(8), SysUsageQty  Numeric(11,2)
				, BomZipperInsert VarChar(5), BomCustPONo VarChar(30), Keyword VarChar(Max), Keyword_Original VARCHAR(MAX), Keyword_xml VARCHAR(MAX), OrderList varchar(max), ColorDesc varchar(150), Special nvarchar(max)
				, BomTypeColorID varchar(50), BomTypeSize varchar(50), BomTypeSizeUnit varchar(50), BomTypeZipperInsert varchar(50), BomTypeArticle varchar(50), BomTypeCOO varchar(50)
				, BomTypeGender varchar(50), BomTypeCustomerSize varchar(50), BomTypeDecLabelSize varchar(50), BomTypeBrandFactoryCode varchar(50), BomTypeStyle varchar(50)
				, BomTypeStyleLocation varchar(50), BomTypeSeason varchar(50), BomTypeCareCode varchar(50), BomTypeCustomerPO varchar(50), BomTypeBuyMonth varchar(50), BomTypeBuyerDlvMonth varchar(50)
				, Index Idx_ID NonClustered (ID, Order_BOAUkey, ColorID) -- table index
			);
	    End;	

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


    Insert Into #Tmp_BoaExpend(ExpendUkey, ID, Order_BOAUkey, RefNo, SCIRefNo, Article, ColorID, SuppColor
		, SizeSeq, SizeCode, SizeSpec, SizeUnit, Remark, OrderQty
        --, Price
        , UsageQty
		, UsageUnit, SysUsageQty, BomZipperInsert, BomCustPONo, Keyword, Keyword_Original, Keyword_xml, OrderList, ColorDesc, Special
		, BomTypeColorID, BomTypeSize, BomTypeSizeUnit, BomTypeZipperInsert, BomTypeArticle, BomTypeCOO, BomTypeGender, BomTypeCustomerSize
		, BomTypeDecLabelSize, BomTypeBrandFactoryCode, BomTypeStyle, BomTypeStyleLocation, BomTypeSeason, BomTypeCareCode, BomTypeCustomerPO
        , BomTypeBuyMonth, BomTypeBuyerDlvMonth)
    exec dbo.sp_GetBOAExpend_NEW @POID, 0, 1, 1, @Tmp_Order_Qty, 0, 0, 1

    Select * 
    from #Tmp_BoaExpend
    where Article = @Art
    and @ID in (Select Data From dbo.SplitString(OrderList,','))

    Drop Table #Tmp_BoaExpend;
";
                DataTable tmp;
                List<SqlParameter> paras = new List<SqlParameter>();
                paras.Add(new SqlParameter("@POID", dtDetail[0]["POID"].ToString()));
                paras.Add(new SqlParameter("@ID", dtDetail[0]["ID"].ToString()));
                paras.Add(new SqlParameter("@Art", dtDetail[0]["Article"].ToString()));
                var result = DBProxy.Current.Select(null, sql, paras, out tmp);

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

                var groupedByRefNo = tmp.AsEnumerable().Select(x => x["RefNo"].ToString())
                .Distinct()
                .ToList();

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

                char chars = (char)('D' + dtDetail.Count);
                totalC = 2 + initIdx;
                objSheets.Cells[2 + initIdx, 5 + dtDetail.Count].Value = string.Format("=SUM({0}{1}:{2}{1})", "E", totalC, chars);
                objSheets.Cells[1 + initIdx, 5 + dtDetail.Count] = "Total Cons";
                objSheets.Cells[1 + initIdx, 5 + dtDetail.Count].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Lavender);
                objSheets.Cells[1 + initIdx, 5 + dtDetail.Count].Font.Bold = true;

                foreach (var group in groupedByRefNo)
                {
                    objSheets.Cells[3 + initIdx, 2] = group.ToString();
                    objSheets.Range[objSheets.Cells[3 + initIdx, 2], objSheets.Cells[4 + initIdx, 2]].Merge();
                    objSheets.Cells[3 + initIdx, 4] = "Cons/PC";

                    for (int i = 0; i < dtDetail.Count;)
                    {
                        DataRow row = dtDetail[i];
                        var tmpRef = tmp.AsEnumerable().Where(x => x["SizeCode"].ToString() == row["SizeCode"].ToString() && x["RefNo"].ToString() == group.ToString()).ToList();

                        if (tmpRef.Count > 0)
                        {
                            DataRow rowRef = tmpRef[0];

                            double usageQty, orderQty, scQty;
                            double.TryParse(rowRef["UsageQty"]?.ToString(), out usageQty);
                            double.TryParse(rowRef["OrderQty"]?.ToString(), out orderQty);
                            double.TryParse(row["Qty"]?.ToString(), out scQty);
                            double matchqty = Math.Round(usageQty / orderQty, 3);

                            objSheets.Cells[3 + initIdx, i + 5] = matchqty;
                            objSheets.Cells[4 + initIdx, i + 5] = scQty * matchqty;
                            objSheets.Cells[4 + initIdx, i + 5].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Plum);
                        }

                        var tmpSizeCodefEmpty = tmp.AsEnumerable()
                    .Where(x => (x["SizeCode"] == DBNull.Value || string.IsNullOrEmpty(x["SizeCode"].ToString())) && x["RefNo"].ToString() == group.ToString())
                    .Distinct()
                    .ToList();

                        if (tmpSizeCodefEmpty.Count > 0)
                        {
                            DataRow rowRef = tmpSizeCodefEmpty[0];
                            double usageQty, orderQty, scQty;
                            double.TryParse(rowRef["UsageQty"].ToString(), out usageQty);
                            double.TryParse(rowRef["OrderQty"].ToString(), out orderQty);
                            double.TryParse(row["Qty"]?.ToString(), out scQty);
                            double matchqty = Math.Round(usageQty / orderQty, 3);

                            objSheets.Cells[3 + initIdx, i + 5] = matchqty;
                            objSheets.Cells[4 + initIdx, i + 5] = scQty * matchqty;
                            objSheets.Cells[4 + initIdx, i + 5].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Plum);
                        }

                        i++;
                    }

                    totalC = 4 + initIdx;
                    objSheets.Cells[4 + initIdx, 5 + dtDetail.Count].Value = string.Format("=SUM({0}{1}:{2}{1})", "E", totalC, chars);

                    initIdx = initIdx + 3;
                }

                if (groupedByRefNo.Count == 0)
                {
                    for (int i = 0; i < dtDetail.Count;)
                    {
                        objSheets.Cells[3 + initIdx, i + 5] = 0;
                        objSheets.Cells[4 + initIdx, i + 5] = 0;
                        objSheets.Cells[4 + initIdx, i + 5].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Plum);
                        i++;
                    }

                    totalC = 4 + initIdx;
                    objSheets.Cells[4 + initIdx, 5 + dtDetail.Count].Value = string.Format("=SUM({0}{1}:{2}{1})", "E", totalC, chars);

                    initIdx = initIdx + 3;
                }

                objSheets.Range[$"A:{MyExcelPrg.GetExcelColumnName(columnsCnt)}"].Columns.AutoFit();

                objSheets.Range[objSheets.Cells[2 + stratcol, 3], objSheets.Cells[initIdx + 1, 3]].Merge();

                excelrange = objSheets.get_Range(string.Format("A{1}:{0}{2}", MyExcelPrg.GetExcelColumnName(columnsCnt), stratcol, MyUtility.Convert.GetString(initIdx + 1)), Type.Missing);
                excelrange.Borders[Excel.XlBordersIndex.xlEdgeTop].Weight = Excel.XlBorderWeight.xlThin;
                excelrange.Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlThin;
                excelrange.Borders[Excel.XlBordersIndex.xlEdgeLeft].Weight = Excel.XlBorderWeight.xlThin;
                excelrange.Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = Excel.XlBorderWeight.xlThin;
                excelrange.Borders[Excel.XlBordersIndex.xlInsideHorizontal].Weight = Excel.XlBorderWeight.xlThin;
                excelrange.Borders[Excel.XlBordersIndex.xlInsideVertical].Weight = Excel.XlBorderWeight.xlThin;

                initIdx = initIdx + 2;
            }
        }
    }
}
